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
   [Authorize] 
    public class CircleController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Page(int id)
        {
            var viewModel = new PageViewModel();
            int ownerID = -1;

            // get page and circle info
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT P.PostCount, C.CircleName, C.CircleType, C.Id, C.OwnerId, P.Id FROM [dbo].[Page] as P, Circle as C WHERE C.Id = @id AND P.CircleId = C.Id";
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        viewModel.PostCount = data.GetInt32(0);
                        viewModel.CircleName = data.GetString(1).Trim();
                        viewModel.CircleType = data.GetString(2).Trim();
                        viewModel.CircleId = data.GetInt32(3);
                        ownerID = data.GetInt32(4);
                        viewModel.Id = data.GetInt32(5);
                    }
                    data.NextResult();

                }
            }

            // get post info
            viewModel.Posts = new List<PostViewModel>();
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"SELECT P.Id, P.DateAdded, P.Content, P.AuthorId, U.FirstName, U.LastName, LP.CustomerId FROM [dbo].[Post] as P
LEFT JOIN LikesPost as LP
ON P.Id = LP.PostId
JOIN [User] as U
on P.AuthorId = U.Id
Where P.PageId = @id  AND (LP.CustomerId IS NULL OR LP.CustomerId = @userId)";
                cmd.Parameters.AddWithValue("@id", viewModel.Id);
                cmd.Parameters.AddWithValue("@userId", UserId);
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var post = new PostViewModel();
                        post.Id = data.GetInt32(0);
                        post.DateAdded = data.GetDateTime(1);
                        post.Content = data.GetString(2).Trim();
                        int authorId = data.GetInt32(3);
                        post.UserCanEdit = (ownerID == UserId || authorId == UserId);
                        post.Author = data.GetString(4).Trim() + " " + data.GetString(5).Trim();
                        post.Comments = new List<CommentViewModel>();
                        post.UserLikes = !data.IsDBNull(6);
                        viewModel.Posts.Add(post);
                    }
                    data.NextResult();
                }
            }

            
            foreach (var post in viewModel.Posts)
            {
                // get comment info
                post.Comments = new List<CommentViewModel>();
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT C.Id, C.DateAdded, C.Content, C.AuthorId, U.FirstName, U.LastName, LC.CustomerId FROM [dbo].[Comment] as C
LEFT JOIN LikesComment as LC
ON C.Id = LC.CommentId
JOIN [User] as U
on C.AuthorId = U.Id
Where C.PostId = @id  AND (LC.CustomerId IS NULL OR LC.CustomerId = @userId)";

                    cmd.Parameters.AddWithValue("@id", post.Id);
                    cmd.Parameters.AddWithValue("@userId", UserId);
                    SqlDataReader data = cmd.ExecuteReader();
                    while (data.HasRows)
                    {
                        while (data.Read())
                        {
                            var comment = new CommentViewModel();
                            comment.Id = data.GetInt32(0);
                            comment.DateAdded = data.GetDateTime(1);
                            comment.Content = data.GetString(2).Trim();
                            int authorId = data.GetInt32(3);
                            comment.UserCanEdit = (ownerID == UserId || authorId == UserId);
                            comment.Author = data.GetString(4).Trim() + " " + data.GetString(5).Trim();
                            comment.UserLikes = !data.IsDBNull(6);
                            post.Comments.Add(comment);
                        }
                        data.NextResult();
                    }

                    post.Comments = post.Comments.OrderByDescending(q => q.DateAdded).ToList();
                }

                // get user likes post

            }


            viewModel.Posts = viewModel.Posts.OrderByDescending(q => q.DateAdded).ToList();


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Post(int id, String content, int circleId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("MakeAPost", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@Content", content));
                command.Parameters.Add(new SqlParameter("@AuthorId", UserId));
                command.Parameters.Add(new SqlParameter("@PageId", id));
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }

            return RedirectToAction("Page", new { id = circleId});
        }

        [HttpPost]
        public ActionResult Comment(int postId, String content, int circleId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand("CommentOnAPost", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@Content", content));
                command.Parameters.Add(new SqlParameter("@AuthorId", UserId));
                command.Parameters.Add(new SqlParameter("@PostId", postId));
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("Page", new { id = circleId});
        }

        [HttpPost]
        public ActionResult LikePost(int postId, bool like)
        {
            var action = like ? "LikeAPost" : "UnlikePost";

            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand(action, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@PostId", postId));
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            return Json(new { });
        }

        [HttpPost]
        public ActionResult LikeComment(int commentId, bool like)
        {
            var action = like ? "LikeComment" : "UnlikeComment";

            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand(action, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("@CommentId", commentId));
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            return Json(new { });
        }

        [HttpPost]
        public ActionResult EditPost(int postId, String content)
        {
            bool success = false;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand("EditPost", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@PostId", postId));
                    command.Parameters.Add(new SqlParameter("@NewContent", content));
                    command.Parameters.Add(new SqlParameter("@UserId", UserId));
                    conn.Open();
                    var data = (int) command.ExecuteNonQuery();
                    if (data == 1)
                    {
                        success = true;
                    }
                }
                conn.Close();
            }

            return Json(new { success });
        }
   

        [HttpPost]
        public ActionResult EditComment(int commentId, String content)
        {
            bool success = false;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand("EditComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@CommentId", commentId));
                    command.Parameters.Add(new SqlParameter("@NewContent", content));
                    command.Parameters.Add(new SqlParameter("@UserId", UserId));
                    conn.Open();
                    var data = (int)command.ExecuteNonQuery();
                    if (data == 1)
                    {
                        success = true;
                    }
                }
                conn.Close();
            }

            return Json(new { success });
        }

        [HttpPost]
        public ActionResult DeletePost(int postId)
        {
            bool success = false;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand("RemovePost", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@PostId", postId));
                    command.Parameters.Add(new SqlParameter("@UserId", UserId));
                    conn.Open();
                    var data = (int)command.ExecuteNonQuery();
                    if (data == 1)
                    {
                        success = true;
                    }
                }
                conn.Close();
            }

            return Json(new { success });
        }

        [HttpPost]
        public ActionResult DeleteComment(int commentId)
        {
            bool success = false;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand("RemoveComment", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@CommentId", commentId));
                    command.Parameters.Add(new SqlParameter("@UserId", UserId));
                    conn.Open();
                    var data = (int)command.ExecuteNonQuery();
                    if (data == 1)
                    {
                        success = true;
                    }
                }
                conn.Close();
            }

            return Json(new { success });
        }

       public ActionResult Delete(int id)
       {
           var viewModel = new CircleViewModel();
           viewModel.Id = id;
           return View(viewModel);
       }

       [HttpPost]
        public JsonResult Create(CircleViewModel model)
        {
           try
            {
                string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;

                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand("CreateACircle", conn)
                                 {
                                     CommandType = CommandType.StoredProcedure
                                 })
                {
                    conn.Open();
                    cmd.Parameters.Add(new SqlParameter("@CircleName", model.CircleName));
                    cmd.Parameters.Add(new SqlParameter("@CircleType", model.CircleType));
                    cmd.Parameters.Add(new SqlParameter("@OwnerId", UserId));
                    SqlDataReader data = cmd.ExecuteReader();
                    conn.Close();
                }

                return Json(new { success = true, message = "Circle successfully created." });
            }
            catch(Exception e)
            {
                return Json(new { success = false, message = "Circle creation failed." });
            }
        }

        [HttpPost]
        public ActionResult Edit(EditCircleViewModel viewModel)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("RenameCircle", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                cmd.Parameters.Add(new SqlParameter("@CircleId", viewModel.Id));
                cmd.Parameters.Add(new SqlParameter("@CircleName", viewModel.CircleName));
                cmd.Parameters.Add(new SqlParameter("@CircleType", viewModel.CircleType));
                cmd.Parameters.Add(new SqlParameter("@UserId", UserId));
                SqlDataReader data = cmd.ExecuteReader();
                conn.Close();
            }

            return RedirectToAction("ListCirclesOwned");
        }

        public ActionResult Edit(int id)
        {
            var viewModel = new EditCircleViewModel();
            viewModel.Id = id;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT C.CircleName, C.CircleType FROM [dbo].[Circle] C WHERE C.Id = @id AND C.OwnerId = @UserId";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    SqlDataReader data = cmd.ExecuteReader();
                    while (data.HasRows)
                    {
                        while (data.Read())
                        {
                            viewModel.CircleName = data.GetString(0).Trim();
                            viewModel.CircleType = data.GetString(1).Trim();
                        }
                        data.NextResult();

                    }
                    conn.Close();
                }
            }

            viewModel.CurrentUsers = new List<CustomerViewModel>();
            viewModel.InvitedUsers = new List<CustomerViewModel>();
            viewModel.RequestingUsers = new List<CustomerViewModel>();
            using (var conn = new SqlConnection(connStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT C.FirstName, C.LastName, C.Id, BTC.UserAccepted, BTC.OwnerAccepted FROM [dbo].[CustomerBelongsToCircle] BTC, [User] as C WHERE C.Id = BTC.CustomerId AND BTC.CircleId = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader data = cmd.ExecuteReader();
                    while (data.HasRows)
                    {
                        while (data.Read())
                        {
                            var user = new CustomerViewModel();
                            user.FirstName = data.GetString(0).Trim();
                            user.LastName = data.GetString(1).Trim();
                            user.Id = data.GetInt32(2);
                            var userAccepted = data.GetBoolean(3);
                            var ownerAccepted = data.GetBoolean(4);
                            if (userAccepted && ownerAccepted)
                            {
                                viewModel.CurrentUsers.Add(user);
                            }
                            else if (userAccepted)
                            {
                                viewModel.RequestingUsers.Add(user);
                            }
                            else if (ownerAccepted)
                            {
                                viewModel.InvitedUsers.Add(user);
                            }
                        }
                        data.NextResult();

                    }
                    conn.Close();
                }
            }

            viewModel.UsersWhoDoNotBelongToCircle = new List<SelectListItem>();
            using (var conn = new SqlConnection(connStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = @"SELECT U.FirstName, U.LastName, C.Id
FROM  [User] as U, Customer as C, [Circle] 
WHERE  Circle.Id = @id AND U.Id = C.Id AND C.Id <> Circle.OwnerId AND (SELECT COUNT(*) FROM [dbo].[CustomerBelongsToCircle] BTC WHERE C.Id = BTC.CustomerId AND BTC.CircleId = @id) = 0";
                    cmd.Parameters.AddWithValue("@id", id);
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
                            viewModel.UsersWhoDoNotBelongToCircle.Add(user);
                        }
                        data.NextResult();

                    }
                    conn.Close();
                }
            }

            return View(viewModel);
        }

        public ActionResult DeleteCircleConfirm(int id)
        {

            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("DeleteCircle", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                cmd.Parameters.Add(new SqlParameter("@CircleId", id));
                cmd.Parameters.Add(new SqlParameter("@UserId", UserId));
                cmd.ExecuteReader();
                conn.Close();
            }

            return RedirectToAction("ListCirclesOwned");
        }

        public ActionResult ListCirclesOwned()
        {
            var circles = new List<CircleViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "SELECT C.Id, C.CircleName, C.CircleType FROM [dbo].[Circle] C WHERE C.OwnerId = @UserId";
                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader data = cmd.ExecuteReader();

                while (data.Read())
                {
                    var newRecord = new CircleViewModel();
                    newRecord.Id = data.GetInt32(0);
                    newRecord.CircleName = data.GetString(1).Trim();
                    newRecord.CircleType = data.GetString(2).Trim();
                    circles.Add(newRecord);

                }
                data.NextResult();
            }

            circles = circles.OrderBy(q => q.CircleName).ToList();

            return View("ListOfCirclesOwned", circles);
        }

        public ActionResult ListOtherCircles()
        {
            var circles = new List<CircleViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    @"SELECT C.Id, C.CircleName, C.CircleType, U.FirstName, U.LastName 
FROM [dbo].[Circle] C, [User] as U 
WHERE C.OwnerId = U.Id AND C.OwnerId <> @UserId
AND NOT EXISTS(SELECT * FROM CustomerBelongsToCircle as BTC WHERE BTC.CustomerId = @UserId AND BTC.CircleId = C.Id)";
                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader data = cmd.ExecuteReader();

                while (data.Read())
                {
                    var newRecord = new CircleViewModel();
                    newRecord.Id = data.GetInt32(0);
                    newRecord.CircleName = data.GetString(1).Trim();
                    newRecord.CircleType = data.GetString(2).Trim();
                    newRecord.OwnerName = data.GetString(3).Trim() + " " + data.GetString(4).Trim();
                    circles.Add(newRecord);

                }
                data.NextResult();
            }

            circles = circles.OrderBy(q => q.CircleName).ToList();

            return View(circles);
        }

        public ActionResult GetInvites()
        {
            var circles = new List<CircleViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    @"SELECT C.Id, C.CircleName, C.CircleType, U.FirstName, U.LastName 
FROM [dbo].[Circle] C, [User] as U, CustomerBelongsToCircle as BTC
WHERE C.OwnerId = U.Id AND BTC.CustomerId = @UserId AND BTC.CircleId = C.Id AND OwnerAccepted = 1 AND UserAccepted = 0";
                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader data = cmd.ExecuteReader();

                while (data.Read())
                {
                    var newRecord = new CircleViewModel();
                    newRecord.Id = data.GetInt32(0);
                    newRecord.CircleName = data.GetString(1).Trim();
                    newRecord.CircleType = data.GetString(2).Trim();
                    newRecord.OwnerName = data.GetString(3).Trim() + " " + data.GetString(4).Trim();
                    circles.Add(newRecord);

                }
                data.NextResult();
            }

            circles = circles.OrderBy(q => q.CircleName).ToList();

            return View(circles);
        }

        public ActionResult GetRequests()
        {
            var circles = new List<CircleViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    @"SELECT C.Id, C.CircleName, C.CircleType, U.FirstName, U.LastName 
FROM [dbo].[Circle] C, [User] as U, CustomerBelongsToCircle as BTC
WHERE C.OwnerId = U.Id AND BTC.CustomerId = @UserId AND BTC.CircleId = C.Id AND OwnerAccepted = 0 AND UserAccepted = 1";
                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader data = cmd.ExecuteReader();

                while (data.Read())
                {
                    var newRecord = new CircleViewModel();
                    newRecord.Id = data.GetInt32(0);
                    newRecord.CircleName = data.GetString(1).Trim();
                    newRecord.CircleType = data.GetString(2).Trim();
                    newRecord.OwnerName = data.GetString(3).Trim() + " " + data.GetString(4).Trim();
                    circles.Add(newRecord);

                }
                data.NextResult();
            }

            circles = circles.OrderBy(q => q.CircleName).ToList();

            return View(circles);
        }

        public ActionResult GetCirclesBelongedTo()
        {
            var circlesBelongedTo = new List<CircleViewModel>();
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "SELECT C.Id, C.CircleName, C.CircleType, U.FirstName, U.LastName FROM [dbo].[Circle] C, [User] as U WHERE C.OwnerId = U.Id AND EXISTS(SELECT * FROM [dbo].[CustomerBelongsToCircle] B WHERE B.CircleId = C.Id AND B.CustomerId = @UserId)";

                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader data = cmd.ExecuteReader();
                while (data.HasRows)
                {
                    while (data.Read())
                    {
                        var newRecord = new CircleViewModel();
                        newRecord.Id = data.GetInt32(0);
                        newRecord.CircleName = data.GetString(1).Trim();
                        newRecord.CircleType = data.GetString(2).Trim();
                        newRecord.OwnerName = data.GetString(3).Trim() + " " + data.GetString(4).Trim();
                        circlesBelongedTo.Add(newRecord);
                    }
                    data.NextResult();
                }
            }

            circlesBelongedTo = circlesBelongedTo.OrderBy(q => q.CircleName).ToList();

            return View("CirclesBelongedTo", circlesBelongedTo);

        }

        public ActionResult UnjoinCircle(int id)
        {
            var viewModel = new CircleViewModel();
            viewModel.Id = id;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "DELETE FROM [dbo].[CustomerBelongsToCircle] WHERE CircleId = @id AND CustomerId = @UserId ";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader data = cmd.ExecuteReader();

            }

            return View("Index");
        }

        public ActionResult RemoveFromCircle(int id, int userId)
        {
            var viewModel = new CircleViewModel();
            viewModel.Id = id;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "DELETE FROM [dbo].[CustomerBelongsToCircle] WHERE CircleId = @id AND CustomerId = @UserId ";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@UserId", userId);
                int data = cmd.ExecuteNonQuery();
                if(data == 1)
                    return Json(new {success = true});

            }

            return Json(new {success = false});
        }


        public ActionResult Invite(int? userId, int circleId)
        {
            if (!userId.HasValue)
                return Json(new { success = false });

            bool success = false;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand("AddUserToCircle", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@CircleId", circleId));
                    command.Parameters.Add(new SqlParameter("@CustomerId", userId.Value));
                    conn.Open();
                    var data = (int)command.ExecuteNonQuery();
                    if (data == 1)
                    {
                        success = true;
                    }
                }
                conn.Close();
            }

            return Json(new { success });
        }

        public ActionResult Join(int circleId)
        {
            bool success = false;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand("JoinCircle", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@CircleId", circleId));
                    command.Parameters.Add(new SqlParameter("@UserId", UserId));
                    conn.Open();
                    var data = (int)command.ExecuteNonQuery();
                    if (data == 1)
                    {
                        success = true;
                    }
                }
                conn.Close();
            }

            return Json(new { success });
        }

        public ActionResult AcceptJoin(int id, int userId)
        {
            bool success = false;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand("AcceptJoin", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@CircleId", id));
                    command.Parameters.Add(new SqlParameter("@UserId", userId));
                    conn.Open();
                    var data = (int)command.ExecuteNonQuery();
                    if (data == 1)
                    {
                        success = true;
                    }
                }
                conn.Close();
            }

            return Json(new { success });
        }

        public ActionResult AcceptInvite(int id)
        {
            bool success = false;
            string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand("AcceptInvite", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("@CircleId", id));
                    command.Parameters.Add(new SqlParameter("@UserId", UserId));
                    conn.Open();
                    var data = (int)command.ExecuteNonQuery();
                    if (data == 1)
                    {
                        success = true;
                    }
                }
                conn.Close();
            }

            return Json(new { success });
        }


    }
}
