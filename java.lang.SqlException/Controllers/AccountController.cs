using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using java.lang.SqlException.Models;

namespace java.lang.SqlException.Controllers
{
    public class AccountController : BaseController
    {

        public ActionResult Register()
        {
            var viewModel = new RegisterViewModel();
            viewModel.DateOfBirth = DateTime.Now;
            return View(viewModel);
        }

        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);
            return RedirectToAction("Login");
        }


        [HttpPost]
        public ActionResult Register(RegisterViewModel viewModel)
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
                        cmd.Parameters.AddWithValue("@password", GetMD5Hash(viewModel.Password));
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT Id FROM [dbo].[User] WHERE EmailAddress = @username";
                        cmd.Parameters.AddWithValue("@username", viewModel.EmailAddress);
                        int Id = (int)cmd.ExecuteScalar();

                        cmd.CommandText = "INSERT INTO [Customer] Values (@idForCust,'',0)";
                        cmd.Parameters.AddWithValue("@idForCust", Id);
                        cmd.ExecuteNonQuery();

                        return RedirectToAction("Login");
                    }
                }
                catch (Exception a)
                {
                    Response.Write(a.Message);
                    ModelState.AddModelError("",a.Message);
                    return View(viewModel);
                }
            }
            return View(viewModel);
        }


        public ActionResult Login()
        {
            var viewModel = new AccountViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Login(AccountViewModel viewModel, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var correct = false;
                var passwordAsHash = GetMD5Hash(viewModel.Password);
                try
                {
                    string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "SELECT Password FROM [dbo].[User] WHERE EmailAddress = @username";
                        cmd.Parameters.AddWithValue("@username", viewModel.Username);
                        String data = (string)cmd.ExecuteScalar();
                        data = data.Trim();
                        if (data.Equals(passwordAsHash))
                        {
                            FormsAuthentication.SetAuthCookie(viewModel.Username, false);
                            if (!String.IsNullOrEmpty(ReturnUrl) && ReturnUrl != "/")
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                 
                        }
                        else
                        {
                            ModelState.AddModelError("","Password or Username incorrect!");
                            return View(viewModel);
                        }
                    }
                }
                catch (Exception a)
                {
                    Response.Write(a.Message);
                }
            }
            return View(viewModel);
        }

        public static string GetMD5Hash(string input)
        {
            var md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

    
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }


        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
