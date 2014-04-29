using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using java.lang.SqlException.Models;

namespace java.lang.SqlException.Controllers
{

    [Authorize(Roles = "Manager")]
    public class ManagerController : BaseController
    {
        //
        // GET: /Manager/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateEmployee()
        {
            var viewModel = new EmployeeViewModel();
            viewModel.DateOfBirth = DateTime.Now;
            viewModel.StartDate = DateTime.Now;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateEmployee(EmployeeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        
                        cmd.CommandText =
                            "INSERT INTO [User] VALUES (@firstname,@lastname,@address,@city,@state,@zipcode,@telephone,@emailaddress,@dateadded,@dateofbirth,@gender,@password)";
                        cmd.Parameters.AddWithValue("@firstname", viewModel.FirstName);
                        cmd.Parameters.AddWithValue("@lastname", viewModel.LastName);
                        cmd.Parameters.AddWithValue("@address", viewModel.Address);
                        cmd.Parameters.AddWithValue("@city", viewModel.City);
                        cmd.Parameters.AddWithValue("@state", viewModel.State);
                        cmd.Parameters.AddWithValue("@zipcode", viewModel.ZipCode);
                        cmd.Parameters.AddWithValue("@telephone", viewModel.Telephone);
                        cmd.Parameters.AddWithValue("@emailaddress", viewModel.EmailAddress);
                        cmd.Parameters.AddWithValue("@dateadded", DateTime.Now);
                        cmd.Parameters.AddWithValue("@dateofbirth", viewModel.DateOfBirth);
                        cmd.Parameters.AddWithValue("@gender", viewModel.Gender);
                        cmd.Parameters.AddWithValue("@password", AccountController.GetMD5Hash(viewModel.Password));
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT Id FROM [dbo].[User] WHERE EmailAddress = @username";
                        cmd.Parameters.AddWithValue("@username", viewModel.EmailAddress);
                        int Id = (int) cmd.ExecuteScalar();

                        cmd.CommandText = "INSERT INTO [Employee] Values (@idForEmp,@ssn,@startdate,@hourlyrate)";
                        cmd.Parameters.AddWithValue("@idForEmp", Id);
                        cmd.Parameters.AddWithValue("@ssn", viewModel.SSN);
                        cmd.Parameters.AddWithValue("@startdate", viewModel.StartDate);
                        cmd.Parameters.AddWithValue("@hourlyrate", viewModel.HourlyRate);
                        cmd.ExecuteNonQuery();

                        return RedirectToAction("ViewEmployees");
                    }
                }
                catch (Exception a)
                {
                    Response.Write(a.Message);
                    ModelState.AddModelError("", a.Message);
                    return View(viewModel);
                }
            }
            return View(viewModel);
        }



        public ActionResult ViewAds()
        {
            var viewModel = new ListOfAdViewModel();
            viewModel.ListOfAds = new List<AdViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT * FROM [dbo].[Advertisement]";
                var data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var newRecord = new AdViewModel();
                        newRecord.Id = data.GetInt32(0);
                        newRecord.Company = data.GetString(1).Trim();
                        newRecord.NumUnits = data.GetInt32(2);
                        newRecord.NumSold = data.GetInt32(3);
                        newRecord.ItemName = data.GetString(4).Trim();
                        newRecord.DateAdded = data.GetDateTime(6);
                        newRecord.UnitPrice = data.GetDecimal(7);
                        newRecord.AdType = data.GetString(8).Trim();
                        viewModel.ListOfAds.Add(newRecord);
                    }
                    data.NextResult();
                }
            }
            return View(viewModel);
        }



        public ActionResult ViewEmployees()
        {
            var listOfEmps = new ListOfEmployeesViewModel();
            listOfEmps.ListOfEmps = new List<EmployeeDashViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "SELECT U.Id, U.FirstName, U.LastName, E.HourlyRate FROM [dbo].[User] U, [dbo].[Employee] E WHERE U.Id = E.Id";
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var newRecord = new EmployeeDashViewModel();
                        newRecord.Id = data.GetInt32(0);
                        newRecord.FirstName = data.GetString(1).Trim();
                        newRecord.LastName = data.GetString(2).Trim();
                        newRecord.Salary = data.GetDecimal(3);
                        listOfEmps.ListOfEmps.Add(newRecord);

                    }
                    data.NextResult();

                }
            }

            return View(listOfEmps);
        }

        public ActionResult DeleteEmployee(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT EmailAddress FROM [dbo].[User] WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                String data = (string) cmd.ExecuteScalar();
                data = data.Trim();
                if (!User.Identity.Name.Equals((string) data))
                {
                    cmd.CommandText = "DELETE FROM [dbo].[Employee] WHERE Id = @id";
                    cmd.CommandText = "DELETE FROM [dbo].[User] WHERE Id = @id";
                    cmd.ExecuteNonQuery();
                }

            }
            return RedirectToAction("ViewEmployees");
        }


        public ActionResult SearchForTransactions()
        {
            var viewModel = new TransactionViewModel();
            return View(viewModel);
        }



        public ActionResult CustomerListForItem()
        {
            var viewModel = new RevenueSummaryListingViewmodel();
            return View(viewModel);
        }

        public ActionResult CustomerListForItemPost(RevenueSummaryListingViewmodel filterView)
        {
            var viewModel = new ListOfEmployeesViewModel();
            viewModel.ListOfEmps = new List<EmployeeDashViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "SELECT C.Id, U.EmailAddress, U.FirstName, U.LastName FROM dbo.Customer C, dbo.Account B, dbo.Sale S, dbo.Advertisement A, [dbo].[User] U WHERE B.CustomerId = C.Id AND S.AccountNumber = B.AccountNumber AND S.AdId = A.Id AND C.Id = U.Id AND A.ItemName = @itemname";
                cmd.Parameters.AddWithValue("@itemname", filterView.ItemName);
                var data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var newRecord = new EmployeeDashViewModel();
                        newRecord.Id = data.GetInt32(0);
                        newRecord.Username = data.GetString(1);
                        newRecord.FirstName = data.GetString(2);
                        newRecord.LastName = data.GetString(3);
                        viewModel.ListOfEmps.Add(newRecord);
                    }
                    data.NextResult();
                }
            }
            return View(viewModel);
        }



        public ActionResult ItemsBasedOnCompany()
        {
            var viewModel = new RevenueSummaryListingViewmodel();
            return View(viewModel);

        }

        public ActionResult ItemsBasedOnCompanyPost(RevenueSummaryListingViewmodel filterCriteria)
        {
            var viewModel = new ListOfAdViewModel();
            viewModel.ListOfAds = new List<AdViewModel>();
            String connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT * FROM dbo.Advertisement WHERE Company = @companyname";
                cmd.Parameters.AddWithValue("@companyname", filterCriteria.CompanyName);
                var data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var newRecord = new AdViewModel();
                        newRecord.Id = data.GetInt32(0);
                        newRecord.Company = data.GetString(1);
                        newRecord.NumUnits = data.GetInt32(2);
                        newRecord.NumSold = data.GetInt32(3);
                        newRecord.ItemName = data.GetString(4);
                        newRecord.DateAdded = data.GetDateTime(6);
                        newRecord.UnitPrice = data.GetDecimal(7);
                        newRecord.AdType = data.GetString(8);
                        viewModel.ListOfAds.Add(newRecord);
                    }
                    data.NextResult();
                }
            }
            return View(viewModel);
        }
    

    public ActionResult RevenueListingFilterCriteria()
        {
            var viewModel = new RevenueSummaryListingViewmodel();
            return View(viewModel);
        }
        

        public ActionResult RevenueListingFilterCirteriaPost(RevenueSummaryListingViewmodel filterCriteria)
        {
            var viewModel = new ListOfRevenueViewModel();
            viewModel.ListOfRevenueSummaryListing = new List<RevenueSummaryListingViewmodel>();
            if(filterCriteria.ItemName == null)
            {
                filterCriteria.ItemName = "";
            }
            if(filterCriteria.ItemType == null)
            {
                filterCriteria.ItemType = "";
            }
            if(filterCriteria.CustomerName==null)
            {
                filterCriteria.CustomerName = "";
            }
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT SUM(S.NumUnits * A.UnitPrice) As TotalAmount FROM dbo.Sale S, dbo.Advertisement A WHERE S.AdId = A.Id AND (A.ItemName = @itemname OR A.AdType = @itemtype OR S.AccountNumber = (SELECT (AccountNumber) FROM dbo.Account WHERE CustomerId = (SELECT (Id) FROM dbo.Customer WHERE Id = (SELECT Id FROM dbo.[User] WHERE EmailAddress = @username))))";
                cmd.Parameters.AddWithValue("@itemname", filterCriteria.ItemName);
                cmd.Parameters.AddWithValue("@itemtype", filterCriteria.ItemType);
                cmd.Parameters.AddWithValue("@username", filterCriteria.CustomerName);
                var data = cmd.ExecuteReader();
                 while (data.HasRows)
                    {
                        while (data.Read())
                        {
                            var newRecord = new RevenueSummaryListingViewmodel();

                            if (!data.IsDBNull(0))
                            {
                                newRecord.TotalAmount = data.GetDecimal(0);
                                viewModel.ListOfRevenueSummaryListing.Add(newRecord);
                            }
                        }
                        data.NextResult();
                    }
                }
            return View(viewModel);
        }

        public ActionResult GetCustRepRevenueSummary()
        {
            var viewModel = new ListOfCustRepSumViewModels();
            viewModel.ListOfCustomerRepViewModels = new List<CustRepRevenueViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "SELECT U.EmailAddress, SUM(A.UnitPrice * A.NumSold) AS GrandTotal FROM dbo.Advertisement A, dbo.CustomerRep C, [dbo].[User] U WHERE A.CreatorId = C.Id AND C.Id = U.Id GROUP BY U.EmailAddress ORDER BY GrandTotal Desc";
                var data = cmd.ExecuteReader();
                 while (data.HasRows)
                    {
                        while (data.Read())
                        {
                            var newRecord = new CustRepRevenueViewModel();
                            if(!data.IsDBNull(1))
                            {
                                newRecord.CustRepName = data.GetString(0).Trim();
                                newRecord.GrandTotal = data.GetDecimal(1);
                                viewModel.ListOfCustomerRepViewModels.Add(newRecord);
                            }

                        }
                        data.NextResult();
                    }
                }
            return View(viewModel);
        }


        public ActionResult ActiveItemList()
        {
            var viewModel = new ListOfAdViewModel();
            viewModel.ListOfAds = new List<AdViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "SELECT A.Id, A.NumSold, A.ItemName, A.AdType, A.Company, A.UnitPrice  FROM dbo.Advertisement A GROUP BY A.Id, A.NumSold, A.Itemname, A.AdType, A.Company, A.UnitPrice ORDER BY A.NumSold desc";
                var data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var newRecord = new AdViewModel();
                        newRecord.Id = data.GetInt32(0);
                        newRecord.NumSold = data.GetInt32(1);
                        newRecord.ItemName = data.GetString(2);
                        newRecord.AdType = data.GetString(3);
                        newRecord.Company = data.GetString(4);
                        newRecord.UnitPrice = data.GetDecimal(5);
                        viewModel.ListOfAds.Add(newRecord);
                    }
                    data.NextResult();
                }
            }

            return View(viewModel);
        }


        public ActionResult GetCustSummary()
        {
            var viewModel = new ListOfCustRepSumViewModels();
            viewModel.ListOfCustomerRepViewModels = new List<CustRepRevenueViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "SELECT U.EmailAddress, SUM(A.UnitPrice * A.NumSold) AS GrandTotal FROM dbo.Advertisement A, dbo.Customer C, dbo.Sale S, dbo.Account B, [dbo].[User] U WHERE S.AdId = A.Id AND B.CustomerId = C.Id AND S.AccountNumber = B.AccountNumber AND C.Id = U.Id GROUP BY U.EmailAddress ORDER BY GrandTotal Desc";
                var data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var newRecord = new CustRepRevenueViewModel();
                        if (!data.IsDBNull(1))
                        {
                            newRecord.CustRepName = data.GetString(0);
                            newRecord.GrandTotal = data.GetDecimal(1);
                            viewModel.ListOfCustomerRepViewModels.Add(newRecord);
                        }

                    }
                    data.NextResult();
                }
            }
            return View(viewModel);
        }



        public ActionResult SearchForTransactionsPost(TransactionViewModel filterCriteria)
        {
            var viewModel = new ListOfTransactionViewModel();
            viewModel.ListOfTransactionViewModels = new List<TransactionViewModel>();
            if (filterCriteria.Username != null)
            {
                string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT S.TransactionId, S.DateOfSale, S.NumUnits, S.AccountNumber, S.AdId FROM dbo.Sale S, dbo.Advertisement A WHERE S.AdId = A.Id AND (S.AccountNumber = (SELECT (AccountNumber) FROM dbo.Account WHERE CustomerId = (SELECT (Id) FROM dbo.Customer WHERE Id = (SELECT Id FROM dbo.[User] WHERE EmailAddress = @username))))";
                    cmd.Parameters.AddWithValue("@username", filterCriteria.Username);
                    var data = cmd.ExecuteReader();
                    while (data.HasRows)
                    {
                        while (data.Read())
                        {
                            var newRecord = new TransactionViewModel();
                            newRecord.Id = data.GetInt32(0);
                            newRecord.DateOfSale = data.GetDateTime(1);
                            newRecord.NumberOfUnits = data.GetInt32(2);
                            newRecord.AccountNumber = data.GetInt32(3);
                            newRecord.AdId = data.GetInt32(4);
                            viewModel.ListOfTransactionViewModels.Add(newRecord);
                        }
                        data.NextResult();
                    }
                }
            }else
            {
                string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT S.TransactionId, S.DateOfSale, S.NumUnits, S.AccountNumber, S.AdId FROM dbo.Sale S, dbo.Advertisement A WHERE S.AdId = A.Id AND (A.ItemName = @itemname)";
                    cmd.Parameters.AddWithValue("@itemname", filterCriteria.ItemName);
                    var data = cmd.ExecuteReader();
                    while (data.HasRows)
                    {
                        while (data.Read())
                        {
                            var newRecord = new TransactionViewModel();
                            newRecord.Id = data.GetInt32(0);
                            newRecord.DateOfSale = data.GetDateTime(1);
                            newRecord.NumberOfUnits = data.GetInt32(2);
                            newRecord.AccountNumber = data.GetInt32(3);
                            newRecord.AdId = data.GetInt32(4);
                            viewModel.ListOfTransactionViewModels.Add(newRecord);
                        }
                        data.NextResult();
                    }
                }
            }
            return View(viewModel);
        }

        public ActionResult SalesReport()
        {
            var viewModel = new SalesReportViewModel();
            viewModel.ListOfMonths = new List<string>();
            viewModel.ListOfMonths.Add("January");
            viewModel.ListOfMonths.Add("February");
            viewModel.ListOfMonths.Add("March");
            viewModel.ListOfMonths.Add("April");
            viewModel.ListOfMonths.Add("May");
            viewModel.ListOfMonths.Add("June");
            viewModel.ListOfMonths.Add("July");
            viewModel.ListOfMonths.Add("August");
            viewModel.ListOfMonths.Add("September");
            viewModel.ListOfMonths.Add("October");
            viewModel.ListOfMonths.Add("November");
            viewModel.ListOfMonths.Add("December");
            return View(viewModel);
        }

        public ActionResult GenerateSalesReport(SalesReportViewModel viewModel)
        {
            var monthId = 0;
            if (viewModel.SelectedMonth.Equals("January"))
            {
                monthId = 1;
            }else if (viewModel.SelectedMonth.Equals("February"))
            {
                monthId = 2;
            }
            else if (viewModel.SelectedMonth.Equals("March"))
            {
                monthId = 3;
            }
            else if (viewModel.SelectedMonth.Equals("April"))
            {
                monthId = 4;
            }
            else if (viewModel.SelectedMonth.Equals("May"))
            {
                monthId = 5;
            }
            else if (viewModel.SelectedMonth.Equals("June"))
            {
                monthId = 6;
            }
            else if (viewModel.SelectedMonth.Equals("July"))
            {
                monthId = 7;
            }
            else if (viewModel.SelectedMonth.Equals("August"))
            {
                monthId = 8;
            }
            else if (viewModel.SelectedMonth.Equals("September"))
            {
                monthId = 9;
            }
            else if (viewModel.SelectedMonth.Equals("October"))
            {
                monthId = 10;
            }
            else if (viewModel.SelectedMonth.Equals("November"))
            {
                monthId = 11;
            }
            else if (viewModel.SelectedMonth.Equals("December"))
            {
                monthId = 12;
            }
            var newViewModel = new ListOfSaleReportViewModels();
            newViewModel.ListOfSalesReportViewModels = new List<SalesReportViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT * FROM dbo.Sale S WHERE MONTH(S.DateOfSale) = @monthid";
                cmd.Parameters.AddWithValue("@monthid", monthId);
                var data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var newRecord = new SalesReportViewModel();
                        newRecord.Id = data.GetInt32(0);
                        newRecord.DateOfSale = data.GetDateTime(1);
                        newRecord.NumUnitsBought = data.GetInt32(2);
                        newRecord.AdId = data.GetInt32(3);
                        newRecord.AccountNumber = data.GetInt32(4);
                        newViewModel.ListOfSalesReportViewModels.Add(newRecord);
                    }
                    data.NextResult();
                }

            }
            return View(newViewModel);
        }




        public ActionResult EditEmployee(int id)
        {
            var viewModel = new EmployeeViewModel();
            viewModel.Id = id;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT U.Firstname, U.LastName, U.Address, U.city, U.State, U.ZipCode, U.Telephone, U.EmailAddress, U.DOB, U.Gender, E.SSN, E.StartDate, E.HourlyRate FROM [dbo].[User] U, [dbo].[Employee] E WHERE U.Id = E.Id AND U.Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        viewModel.FirstName = data.GetString(0);
                        viewModel.LastName = data.GetString(1);
                        viewModel.Address = data.GetString(2);
                        viewModel.City = data.GetString(3);
                        viewModel.State = data.GetString(4);
                        viewModel.ZipCode = data.GetString(5);
                        viewModel.Telephone = data.GetString(6);
                        viewModel.EmailAddress = data.GetString(7);
                        viewModel.DateOfBirth = data.GetDateTime(8);
                        viewModel.Gender = data.GetString(9);
                        viewModel.SSN = data.GetInt32(10);
                        viewModel.StartDate = data.GetDateTime(11);
                        viewModel.HourlyRate = data.GetDecimal(12);
                    }
                    data.NextResult();

                }
            }
            viewModel.Password = "NOT NULLABLE FIELD";
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditEmployee(EmployeeViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "UPDATE U" +
                                      " set U.FirstName = @firstname," +
                                      "U.Lastname = @lastname," +
                                      "U.Address = @address," +
                                      "U.City = @city," +
                                      "U.State = @state," +
                                      "U.ZipCode = @zipcode," +
                                      "U.Telephone = @telephone," +
                                      "U.DOB = @dob," +
                                      "U.Gender = @gender " +
                                      "FROM [dbo].[User] U WHERE U.Id = @id";
                    cmd.Parameters.AddWithValue("@id", viewModel.Id);
                    cmd.Parameters.AddWithValue("@firstname", viewModel.FirstName);
                    cmd.Parameters.AddWithValue("@lastname", viewModel.LastName);
                    cmd.Parameters.AddWithValue("@address", viewModel.Address);
                    cmd.Parameters.AddWithValue("@city", viewModel.City);
                    cmd.Parameters.AddWithValue("@state", viewModel.State);
                    cmd.Parameters.AddWithValue("@zipcode", viewModel.ZipCode);
                    cmd.Parameters.AddWithValue("@telephone", viewModel.Telephone);
                    cmd.Parameters.AddWithValue("@dob", viewModel.DateOfBirth);
                    cmd.Parameters.AddWithValue("@gender", viewModel.Gender);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = 
                                      "UPDATE E " +
                                      "SET E.SSN = @ssn," +
                                      "E.StartDate = @startdate," +
                                      "E.HourlyRate = @hourlyrate" +
                                      " FROM [dbo].[Employee] E " +
                                      "WHERE E.Id  = @id";
                  
                    cmd.Parameters.AddWithValue("@ssn", viewModel.SSN);
                    cmd.Parameters.AddWithValue("@startdate", viewModel.StartDate);
                    cmd.Parameters.AddWithValue("@hourlyrate", viewModel.HourlyRate);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("ViewEmployees");
                }
            }
            return View(viewModel);
        }


    }
}
