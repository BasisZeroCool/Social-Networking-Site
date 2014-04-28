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
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        public int UserId;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            UserId = User != null ? getUserId(User.Identity.Name) : -1;
            if (User != null && User.IsInRole("Customer"))
            {
                string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
                var preferences = GetUserPreferences(UserId);
                var ads = GetAdsOfType(preferences);
                var rand = new Random();
                var count = ads.Count();
                CreateEditAdViewModel ad;
                if(count >0)
                {
                    ad = ads.Skip(rand.Next(count)).Take(1).Single();
                }
                else
                {
                    ad = getRandAd();
                }

                ViewBag.Advert = ad;

                ViewBag.Suggestions = GetUserSuggestions();

                ViewBag.BestSellers = GetBestSellers();
            }
                
        }

        private List<AdViewModel> GetUserSuggestions()
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            var ads = new List<AdViewModel>();
           
                using (var conn = new SqlConnection(connStr))
                {
                    using (var command = new SqlCommand("GetRecomendations", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@CustId", UserId);
                        SqlDataReader data = command.ExecuteReader();
                        while (data.HasRows)
                        {
                            while (data.Read())
                            {
                                var viewModel = new AdViewModel();
                                viewModel.Id = data.GetInt32(0);
                                viewModel.ItemName = data.GetString(1);
                                ads.Add(viewModel);
                            }
                            data.NextResult();

                        }

                    }
            }

            return ads;
        }

        private List<AdViewModel> GetBestSellers()
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            var ads = new List<AdViewModel>();

            using (var conn = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand("GetBestSellers", conn)
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
                            var viewModel = new AdViewModel();
                            viewModel.Id = data.GetInt32(0);
                            viewModel.ItemName = data.GetString(4);
                            ads.Add(viewModel);
                        }
                        data.NextResult();

                    }

                }
            }

            return ads;
        }
        private CreateEditAdViewModel getRandAd()
    {
        CreateEditAdViewModel ad = null;
        string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
        using (var conn = new SqlConnection(connStr))
        {
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "Select TOP 1 A.Id, A.ItemName, A.Company, A.NumUnits, A.NumSold, A.UnitPrice, A.Content From Advertisement as A ORDER BY NEWID()";
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        ad = new CreateEditAdViewModel();
                        ad.Id = data.GetInt32(0);
                        ad.ItemName = data.GetString(1);
                        ad.Company = data.GetString(2);
                        ad.NumUnits = data.GetInt32(3);
                        ad.NumSold = data.GetInt32(4);
                        ad.UnitPrice = data.GetDecimal(5);
                        ad.Content = data.GetString(6);
                    }
                    data.NextResult();

                }
            }
        }

        return ad;
    }
        
        private List<String> GetUserPreferences(int id)
        {
            var preferences = new List<String>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("GetPreferences", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@CustId", id));
                conn.Open();
                var data = (string)(command.ExecuteScalar());
                foreach (var item in data.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    preferences.Add(item.Trim());
                }
                conn.Close();
            }
            return preferences;
        }

        private List<CreateEditAdViewModel> GetAdsOfType(List<String> preferences)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            var ads = new List<CreateEditAdViewModel>();
            foreach(var adType in preferences)
            {
                using (var conn = new SqlConnection(connStr))
                {
                    using (var command = new SqlCommand("GetAdsByType", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                            conn.Open();
                            command.Parameters.AddWithValue("@AdType", adType);
                            SqlDataReader data = command.ExecuteReader();
                            while (data.HasRows)
                            {
                                while (data.Read())
                                {
                                    var viewModel = new CreateEditAdViewModel();
                                    viewModel.Id = data.GetInt32(0);
                                    viewModel.ItemName = data.GetString(1);
                                    viewModel.Company = data.GetString(2);
                                    viewModel.NumUnits = data.GetInt32(3);
                                    viewModel.NumSold = data.GetInt32(4);
                                    viewModel.UnitPrice = data.GetDecimal(5);
                                    viewModel.Content = data.GetString(6);
                                    ads.Add(viewModel);
                                }
                                data.NextResult();
            
                            }
                        
                    }
                }
            }
            
            return ads;
        }

        private int getUserId(string email)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("Select U.Id From [User] as U Where U.EmailAddress = @email", conn))
            {
                command.Parameters.Add(new SqlParameter("@email", email));
                conn.Open();
                try
                {
                    var data = (Int32)command.ExecuteScalar();
                    return data;
                    conn.Close();
                }
                catch {
                    return -1;
                }
                
            }
        }

    }
}
