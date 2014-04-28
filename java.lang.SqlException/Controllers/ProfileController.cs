using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using java.lang.SqlException.Models;

namespace java.lang.SqlException.Controllers
{
    public class ProfileController : BaseController
    {
        //
        // GET: /Profile/

        public ActionResult Index()
        {
            var viewModel = new ProfileViewModel();
            viewModel.Id = UserId;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT U.Firstname, U.LastName, U.Address, U.city, U.State, U.ZipCode, U.Telephone, U.EmailAddress, U.DOB, U.Gender FROM [dbo].[User] U Where U.Id = @id";
                cmd.Parameters.AddWithValue("@id", UserId);
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        viewModel.FirstName = data.GetString(0).Trim();
                        viewModel.LastName = data.GetString(1).Trim();
                        viewModel.Address = data.GetString(2).Trim();
                        viewModel.City = data.GetString(3).Trim();
                        viewModel.State = data.GetString(4).Trim();
                        viewModel.ZipCode = data.GetString(5).Trim();
                        viewModel.Telephone = data.GetString(6).Trim();
                        viewModel.EmailAddress = data.GetString(7).Trim();
                        viewModel.DateOfBirth = data.GetDateTime(8);
                        viewModel.Gender = data.GetString(9).Trim();
                    }
                    data.NextResult();

                }
            }

            if (User.IsInRole("Customer"))
            {
                viewModel.IsCustomer = true;
                using (var conn = new SqlConnection(connStr))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "SELECT U.Rating, U.Preferences FROM [dbo].[Customer] U Where U.Id = @id";
                        cmd.Parameters.AddWithValue("@id", UserId);
                        SqlDataReader data = cmd.ExecuteReader();
                        data.Read();
                        viewModel.Rating = data.IsDBNull(0) ? null : (int?)(data.GetInt32(0));
                        viewModel.Preferences = data.GetString(1).Trim();

                    }
                }

                viewModel.Accounts = new List<CreditCardViewModel>();
                using (var conn = new SqlConnection(connStr))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "SELECT A.AccountNumber, A.CreditCardNumber, A.DateAdded FROM [dbo].[Account] A Where A.CustomerId = @id";
                        cmd.Parameters.AddWithValue("@id", UserId);
                        SqlDataReader data = cmd.ExecuteReader();
                        while (data.HasRows)
                        {
                            while (data.Read())
                            {
                                var account = new CreditCardViewModel();
                                account.Id = data.GetInt32(0);
                                account.CardNumber = data.GetString(1).Trim();
                                account.DateAdded = data.GetDateTime(2);
                                viewModel.Accounts.Add(account);
                            }

                            data.NextResult();
                        }
                    }
                }
            }
            else
            {
                viewModel.IsCustomer = false;
            }

            if (User.IsInRole("Manager") || User.IsInRole("CustomerRep"))
            {
                using (var conn = new SqlConnection(connStr))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "SELECT U.SSN, U.StartDate, U.HourlyRate FROM [dbo].[Employee] U Where U.Id = @id";
                        cmd.Parameters.AddWithValue("@id", UserId);
                        SqlDataReader data = cmd.ExecuteReader();
                            while (data.Read())
                            {
                                viewModel.SSN = data.GetInt32(0);
                                viewModel.StartDate = data.GetDateTime(1);
                                viewModel.HourlyRate = data.GetDecimal(2);
                            }
                    }
                }
            }

            viewModel.IsManager = User.IsInRole("Manager");
            viewModel.IsCustRep = User.IsInRole("CustomerRep");
            
            return View(viewModel);
        }

        // This is to create an Account
        public ActionResult CreateAccount()
        {
            var model = new CreditCardViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateAccount(CreditCardViewModel model)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("CreateAccount", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@CardNumber", model.CardNumber));
                command.Parameters.Add(new SqlParameter("@DateAdded", DateTime.Now));
                command.Parameters.Add(new SqlParameter("@CustId", UserId));
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("Index");
        }

        // This is to purchase items
        public ActionResult Purchase(int id)
        {
            var viewModel = new SaleViewModel();
            viewModel.AdId = id;

            var ad = new AdViewModel();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT A.Company, A.NumUnits, A.NumSold, A.ItemName, A.DateAdded, A.UnitPrice, A.AdType FROM [dbo].[Advertisement] A WHERE A.Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        ad.Company = data.GetString(0).Trim();
                        ad.NumUnits = data.GetInt32(1);
                        ad.NumSold = data.GetInt32(2);
                        ad.ItemName = data.GetString(3).Trim();
                        ad.DateAdded = data.GetDateTime(4);
                        ad.UnitPrice = data.GetDecimal(5);
                        ad.AdType = data.GetString(6).Trim();
                    }
                    data.NextResult();

                }

                viewModel.Ad = ad;

                

            }

            viewModel.Accounts = new List<SelectListItem>();
            using (var conn = new SqlConnection(connStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT A.AccountNumber, A.CreditCardNumber FROM [dbo].[Account] A Where A.CustomerId = @id";
                    cmd.Parameters.AddWithValue("@id", UserId);
                    SqlDataReader data = cmd.ExecuteReader();
                    while (data.HasRows)
                    {
                        while (data.Read())
                        {
                            var account = new SelectListItem();
                            account.Value = data.GetInt32(0) + "";
                            account.Text = data.GetString(1).Trim();
                            viewModel.Accounts.Add(account);
                        }

                        data.NextResult();
                    }
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Purchase(SaleViewModel model)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("RecordTransaction", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@NumUnits", model.Num));
                command.Parameters.Add(new SqlParameter("@AdId", model.AdId));
                command.Parameters.Add(new SqlParameter("@AccountNumber", model.AccountId));
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("Index");
        }

        // To edit an account
        public ActionResult EditAccount(int id)
        {
            var viewModel = new CreditCardViewModel();
            viewModel.Id = id;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT A.CreditCardNumber FROM [dbo].[Account] A WHERE A.AccountNumber = @id";
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        viewModel.CardNumber = data.GetString(0).Trim();
                    }
                    data.NextResult();

                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditAccount(CreditCardViewModel model)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("EditAccount", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.Parameters.AddWithValue("@id", model.Id);
                command.Parameters.AddWithValue("@CardNumber", model.CardNumber);
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // To delete an account
        public ActionResult DeleteAccount(int id)
        {
            var viewModel = new CreditCardViewModel();
            viewModel.Id = id;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT A.CreditCardNumber, A.DateAdded FROM [dbo].[Account] A WHERE A.AccountNumber = @id";
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        viewModel.CardNumber = data.GetString(0).Trim();
                        viewModel.DateAdded = data.GetDateTime(1);
                    }
                    data.NextResult();

                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteAccountConfirm(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("DeleteAccount", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@Id", id));
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("Index");
        }

        // TO view the account history
        public ActionResult ViewAccountHistory(int id)
        {
            var viewModel = new List<AccountHistoryViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("GetAccountHistory", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@Id", id));
                conn.Open();
                
                SqlDataReader data = command.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var newRecord = new AccountHistoryViewModel();
                        newRecord.DateOfSale = data.GetDateTime(0);
                        newRecord.Company = data.GetString(1).Trim();
                        newRecord.ItemName = data.GetString(2).Trim();
                        newRecord.NumBought = data.GetInt32(3);
                        newRecord.UnitPrice = data.GetDecimal(4);
                        viewModel.Add(newRecord);

                    }
                    data.NextResult();
                }
            }

            viewModel = viewModel.OrderBy(q => q.DateOfSale).ToList();

            return View(viewModel);
        }
        
    }
}
