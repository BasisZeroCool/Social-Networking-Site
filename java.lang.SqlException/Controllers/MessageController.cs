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
    public class MessageController : BaseController
    {
        //
        // GET: /Message/

        public ActionResult Index()
        {
            var messages = new List<MessageViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "SELECT M.Id, M.DateAdded, M.MessageSubject, U1.FirstName as RecieverName, U2.FirstName as SenderName FROM PrivateMessage M, [dbo].[User] U1, [dbo].[User] U2 WHERE(M.RecieverId = U1.Id AND M.SenderId = U2.Id) AND (U1.Id = @UserId OR U2.Id = @UserId)";
                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader data = cmd.ExecuteReader();

                while (data.Read())
                {
                    var newRecord = new MessageViewModel();
                    newRecord.Id = data.GetInt32(0);
                    newRecord.Date = data.GetDateTime(1);
                    newRecord.Subject = data.GetString(2).Trim();
                    newRecord.Sender = data.GetString(3).Trim();
                    newRecord.Receiver = data.GetString(4).Trim();
                    messages.Add(newRecord);

                }
                data.NextResult();
            }
            return View("Index", messages);
        }

        [HttpPost]
        public ActionResult Create(MessageViewModel model)
        {
             string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;

                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand("SendMessage", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();
                    cmd.Parameters.Add(new SqlParameter("@Sender", UserId));
                    cmd.Parameters.Add(new SqlParameter("@Reciever", model.ReceiverId));
                    cmd.Parameters.Add(new SqlParameter("@Subject", model.Subject));
                    cmd.Parameters.Add(new SqlParameter("@Content", model.Content));
                    cmd.ExecuteReader();
                    conn.Close();
                }

                

                return RedirectToAction("Index");
            }

        public ActionResult Create()
        {
            var viewModel = new MessageViewModel();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            viewModel.MessageRecipients = new List<SelectListItem>();
            using (var conn = new SqlConnection(connStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT U.FirstName, U.LastName, C.Id
FROM  [User] as U, Customer as C
WHERE  U.Id = C.Id AND C.Id <> @id";
                    cmd.Parameters.AddWithValue("@id", UserId);
                    SqlDataReader data = cmd.ExecuteReader();
                    while (data.HasRows)
                    {
                        while (data.Read())
                        {
                            var user = new SelectListItem();
                            var firstName = data.GetString(0).Trim();
                            var lastName = data.GetString(1).Trim();
                            var userid = data.GetInt32(2);

                            user.Text = firstName + " " + lastName;
                            user.Value = userid.ToString();
                            viewModel.MessageRecipients.Add(user);
                        }
                        data.NextResult();

                    }
                    conn.Close();
                }
            }
            return View(viewModel);
        }

        public ActionResult GetMessages()
        {
            var messages = new List<MessageViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "SELECT M.Id, M.DateAdded, M.MessageSubject, M.SenderId, M.RecieverId FROM PrivateMessage M WHERE(M.RecieverId = @UserId OR M.SenderId = @UserId)";
                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader data = cmd.ExecuteReader();

                while (data.Read())
                {
                    var newRecord = new MessageViewModel();
                    newRecord.Id = data.GetInt32(0);
                    newRecord.Date = data.GetDateTime(1);
                    newRecord.Subject = data.GetString(2).Trim();
                    newRecord.Content = data.GetString(3).Trim();
                    newRecord.Sender = data.GetString(4).Trim();
                    newRecord.Receiver = data.GetString(5).Trim();
                    messages.Add(newRecord);

                }
                data.NextResult();
            }
            return View("Index", messages);
        }

        public ActionResult DeleteMessageConfirm(int id)
        {
            var viewModel = new MessageViewModel();
            viewModel.Id = id;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("DeleteMessage", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                cmd.Parameters.Add(new SqlParameter("@id", id));
                cmd.Parameters.Add(new SqlParameter("@UserId", UserId));
                cmd.ExecuteReader();
                conn.Close();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var viewModel = new MessageViewModel();
            viewModel.Id = id;
            return View(viewModel);
        }

        public ActionResult Details(int id)
        {
            var message = new MessageViewModel();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "SELECT TOP 1 M.Id, M.DateAdded, M.MessageSubject, U1.FirstName as RecieverName, U2.FirstName as SenderName, M.Content FROM PrivateMessage M, [dbo].[User] U1, [dbo].[User] U2 WHERE(M.RecieverId = U1.Id AND M.SenderId = U2.Id) AND (U1.Id = @UserId OR U2.Id = @UserId)";
                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader data = cmd.ExecuteReader();

                while (data.Read())
                {
                    message.Id = data.GetInt32(0);
                    message.Date = data.GetDateTime(1);
                    message.Subject = data.GetString(2).Trim();
                    message.Sender = data.GetString(3).Trim();
                    message.Receiver = data.GetString(4).Trim();
                    message.Content = data.GetString(5).Trim();
                }
                data.NextResult();
            }
            return View(message);
        }
    }
}
