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
    public class CustomerRepController : BaseController
    {
        //
        // GET: /CustomerRep/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAds()
        {
            var ads = new List<AdViewModel>();

            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("GetEmployeesAds", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@email", User.Identity.Name));
                conn.Open();
                var data = command.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var item = new AdViewModel();
                        item.Id = data.GetInt32(0);
                        item.Company = data.GetString(1).Trim();
                        item.NumUnits = data.GetInt32(2);
                        item.NumSold = data.GetInt32(3);
                        item.ItemName = data.GetString(4).Trim();
                        item.DateAdded = data.GetDateTime(6);
                        item.UnitPrice = data.GetDecimal(7);
                        item.AdType = data.GetString(8).Trim();
                        ads.Add(item);
                    }
                    data.NextResult();

                }
                conn.Close();
            }
            return View("ViewAds", ads);
        }

        public ActionResult ViewAd(int id)
        {
            var viewModel = new CreateEditAdViewModel();
            viewModel.Id = id;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT A.Company, A.NumUnits, A.NumSold, A.ItemName, A.Content, A.DateAdded, A.UnitPrice, A.AdType FROM [dbo].[Advertisement] A WHERE A.Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        viewModel.Company = data.GetString(0).Trim();
                        viewModel.NumUnits = data.GetInt32(1);
                        viewModel.NumSold = data.GetInt32(2);
                        viewModel.ItemName = data.GetString(3).Trim();
                        viewModel.Content = data.GetString(4).Trim();
                        viewModel.DateAdded = data.GetDateTime(5);
                        viewModel.UnitPrice = data.GetDecimal(6);
                        viewModel.AdType = data.GetString(7).Trim();
                    }
                    data.NextResult();

                }
            }

            return View(viewModel);
        }

        public ActionResult CreateAd()
        {
            var viewModel = new CreateEditAdViewModel();
            return View(viewModel);
        }

        public ActionResult EditAd(int id)
        {
            var viewModel = new CreateEditAdViewModel();
            viewModel.Id = id;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT A.Company, A.NumUnits, A.NumSold, A.ItemName, A.Content, A.DateAdded, A.UnitPrice, A.AdType FROM [dbo].[Advertisement] A WHERE A.Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        viewModel.Company = data.GetString(0).Trim();
                        viewModel.NumUnits = data.GetInt32(1);
                        viewModel.NumSold = data.GetInt32(2);
                        viewModel.ItemName = data.GetString(3).Trim();
                        viewModel.Content = data.GetString(4);
                        viewModel.DateAdded = data.GetDateTime(5);
                        viewModel.UnitPrice = data.GetDecimal(6);
                        viewModel.AdType = data.GetString(7).Trim();
                    }
                    data.NextResult();

                }
            }
            return View(viewModel);
        }


        public ActionResult DeleteAd(int id)
        {
            var viewModel = new CreateEditAdViewModel();
            viewModel.Id = id;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT A.Company, A.NumUnits, A.NumSold, A.ItemName, A.Content, A.DateAdded, A.UnitPrice, A.AdType FROM [dbo].[Advertisement] A WHERE A.Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        viewModel.Company = data.GetString(0).Trim();
                        viewModel.NumUnits = data.GetInt32(1);
                        viewModel.NumSold = data.GetInt32(2);
                        viewModel.ItemName = data.GetString(3).Trim();
                        viewModel.Content = data.GetString(4).Trim();
                        viewModel.DateAdded = data.GetDateTime(5);
                        viewModel.UnitPrice = data.GetDecimal(6);
                        viewModel.AdType = data.GetString(7).Trim();
                    }
                    data.NextResult();

                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteAdConfirm(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("DeleteAd", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@Id", id));
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("ViewAds");
        }

        [HttpPost]
        public ActionResult EditAd(CreateEditAdViewModel model)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("EditAd", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@Id", model.Id));
                command.Parameters.Add(new SqlParameter("@AdType", model.AdType));
                command.Parameters.Add(new SqlParameter("@Company", model.Company));
                command.Parameters.Add(new SqlParameter("@Content", model.Content));
                command.Parameters.Add(new SqlParameter("@ItemName", model.ItemName));
                command.Parameters.Add(new SqlParameter("@NumUnits", model.NumUnits));
                command.Parameters.Add(new SqlParameter("@UnitPrice", model.UnitPrice));
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("ViewAds");  
        }
        
        [HttpPost]
        public ActionResult CreateAd(CreateEditAdViewModel model)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("CreateAd", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@AdType", model.AdType));
                command.Parameters.Add(new SqlParameter("@Company", model.Company));
                command.Parameters.Add(new SqlParameter("@Content", model.Content));
                command.Parameters.Add(new SqlParameter("@CreatorId", UserId));
                command.Parameters.Add(new SqlParameter("@ItemName", model.ItemName));
                command.Parameters.Add(new SqlParameter("@NumUnits", model.NumUnits));
                command.Parameters.Add(new SqlParameter("@UnitPrice", model.UnitPrice));
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("ViewAds");  
        }

        public ActionResult RecordTransaction()
        {
            var viewModel = new RecordSaleViewModel();

            viewModel.Ads = getRepsAds(User.Identity.Name);
            viewModel.Accounts = getUserAccounts();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RecordTransaction(RecordSaleViewModel model)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("RecordTransaction", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@NumUnits", model.NumUnits));
                command.Parameters.Add(new SqlParameter("@AdId", model.AdId));
                command.Parameters.Add(new SqlParameter("@AccountNumber", model.AccountId));
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("Index");
        }

        private List<SelectListItem> getUserAccounts()
        {
            var accounts = new List<SelectListItem>();

            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("GetUserAccounts", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                var data = command.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var item = new SelectListItem();
                        item.Value = data.GetInt32(0) + "";
                        var account = data.GetString(1).Trim();
                        var username = data.GetString(2).Trim();
                        item.Text = account + " (" + username.Trim() + ")";
                        accounts.Add(item);
                    }
                    data.NextResult();

                }
                conn.Close();
            }
            return accounts;
        }

        private List<SelectListItem> getRepsAds(string email)
        {
            var ads = new List<SelectListItem>();

            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("GetEmployeesAds", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@email", email));
                conn.Open();
                var data = command.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var item = new SelectListItem();
                        item.Value = data.GetInt32(0) + "";
                        item.Text = data.GetString(4).Trim();
                        ads.Add(item);
                    }
                    data.NextResult();

                }
                conn.Close();
            }
            return ads;
        }

        public ActionResult ListCustomers()
        {
            var customers = new List<CustomerViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "SELECT U.Id, U.FirstName, U.LastName, U.EmailAddress, C.Rating FROM [dbo].[User] U, [dbo].[Customer] C WHERE U.Id = C.Id";
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var newRecord = new CustomerViewModel();
                        newRecord.Id = data.GetInt32(0);
                        newRecord.FirstName = data.GetString(1).Trim();
                        newRecord.LastName = data.GetString(2).Trim();
                        newRecord.Username = data.GetString(3).Trim();
                        newRecord.Rating = data.IsDBNull(4) ? null : (Int32?)(data.GetInt32(4));
                        customers.Add(newRecord);

                    }
                    data.NextResult();
                }
            }

            customers = customers.OrderBy(q => q.Username).ToList();

            return View("ListCustomers", customers);
        }

        public ActionResult EditCustomer(int id)
        {
            var viewModel = new EditCustmerViewModel();
            viewModel.Id = id;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT U.Firstname, U.LastName, U.Address, U.city, U.State, U.ZipCode, U.Telephone, U.EmailAddress, U.DOB, U.Gender, C.Rating, C.Preferences FROM [dbo].[User] U, [dbo].[Customer] C WHERE U.Id = C.Id AND U.Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
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
                        viewModel.Rating = data.IsDBNull(10) ? null : (int?) (data.GetInt32(10));
                        viewModel.Preferences = data.GetString(11).Trim();
                    }
                    data.NextResult();

                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditCustomer(EditCustmerViewModel model)
        {
             string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
             using (var conn = new SqlConnection(connStr))
             using (var command = new SqlCommand("UpdateCustomerInfo", conn)
             {
                 CommandType = CommandType.StoredProcedure
             })
             {
                 command.Parameters.Add(new SqlParameter("@UserName", model.Id));
                 command.Parameters.Add(new SqlParameter("@Preferences", model.Preferences));
                 command.Parameters.Add(new SqlParameter("@FirstName", model.FirstName));
                 command.Parameters.Add(new SqlParameter("@LastName", model.LastName));
                 command.Parameters.Add(new SqlParameter("@Address", model.Address));
                 command.Parameters.Add(new SqlParameter("@City", model.City));
                 command.Parameters.Add(new SqlParameter("@State", model.State));
                 command.Parameters.Add(new SqlParameter("@ZipCode", model.ZipCode));
                 command.Parameters.Add(new SqlParameter("@Telephone", model.Telephone));
                 command.Parameters.Add(new SqlParameter("@EmailAddress", model.EmailAddress));
                 command.Parameters.Add(new SqlParameter("@Rating", model.Rating));
                 conn.Open();
                 command.ExecuteNonQuery();
                 conn.Close();
             }
            return RedirectToAction("ListCustomers");
        }

        public ActionResult DeleteCustomer(int id)
        {
            var viewModel = new EditCustmerViewModel();
            viewModel.Id = id;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT U.Firstname, U.LastName, U.Address, U.city, U.State, U.ZipCode, U.Telephone, U.EmailAddress, U.DOB, U.Gender, C.Rating, C.Preferences FROM [dbo].[User] U, [dbo].[Customer] C WHERE U.Id = C.Id AND U.Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
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
                        viewModel.Rating = data.IsDBNull(10) ? null : (int?)(data.GetInt32(10));
                        viewModel.Preferences = data.GetString(11).Trim();
                    }
                    data.NextResult();

                }
            }
            return View(viewModel);
        }


        public ActionResult DeleteCustomerConfirm(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("DeleteUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@UserId", id));
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("ListCustomers");
        }

        [HttpPost]
        public ActionResult CreateCustomer(EditCustmerViewModel model)
        {
            int id = -1;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("CreateUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@FirstName", model.FirstName));
                command.Parameters.Add(new SqlParameter("@LastName", model.LastName));
                command.Parameters.Add(new SqlParameter("@Address", model.Address));
                command.Parameters.Add(new SqlParameter("@City", model.City));
                command.Parameters.Add(new SqlParameter("@State", model.State));
                command.Parameters.Add(new SqlParameter("@ZipCode", model.ZipCode));
                command.Parameters.Add(new SqlParameter("@Telephone", model.Telephone));
                command.Parameters.Add(new SqlParameter("@EmailAddress", model.EmailAddress));
                command.Parameters.Add(new SqlParameter("@DOB", model.DateOfBirth));
                command.Parameters.Add(new SqlParameter("@Gender", model.Gender));
                command.Parameters.Add(new SqlParameter("@Password", AccountController.GetMD5Hash(model.Password)));
                conn.Open();
                var temp = command.ExecuteScalar();
                var type = temp.GetType();
                id = (Int32)((Decimal)temp);
                conn.Close();
            }

            if (id <= 0)
                throw new ArgumentException("Insert User failed");

            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("CreateCustomer", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@Id", id));
                command.Parameters.Add(new SqlParameter("@Preferences", model.Preferences));
                command.Parameters.Add(new SqlParameter("@Rating", model.Rating));
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("ListCustomers");
        }

        public ActionResult CreateCustomer()
        {
            var model = new EditCustmerViewModel();
            return View(model);
        }

        public ActionResult ProduceMaillingList()
        {
            return View();
        }

        public ActionResult GetSuggestionsForUser(int id)
        {
            var ads = new List<AdViewModel>();
            
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("GetRecomendations", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@CustId", id));
                conn.Open();
                var data = command.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var viewModel = new AdViewModel();
                        viewModel.Id = data.GetInt32(0);
                        viewModel.ItemName = data.GetString(1).Trim();
                        ads.Add(viewModel);
                    }
                    data.NextResult();

                }
                conn.Close();
            }
            return View(ads);
        }

        public ActionResult MailingList()
        {
            var customers = new List<CustomerViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("MailingList", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                SqlDataReader data = command.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var newRecord = new CustomerViewModel();
                        newRecord.FirstName = data.GetString(0).Trim();
                        newRecord.LastName = data.GetString(1).Trim();
                        newRecord.Username = data.GetString(2).Trim();
                        customers.Add(newRecord);

                    }
                    data.NextResult();
                }
            }

            customers = customers.OrderBy(q => q.Username).ToList();

            return View(customers);
        }

    }
}
