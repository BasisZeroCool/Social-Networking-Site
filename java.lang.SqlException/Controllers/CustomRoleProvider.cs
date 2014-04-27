using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace java.lang.SqlException.Controllers
{
    public class CustomRoleProvider : RoleProvider

    {
        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            List<string> response = new List<string>();
           
                string connStr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT Id FROM [dbo].[User] WHERE EmailAddress = @username";
                    cmd.Parameters.AddWithValue("@username", username);
                    int data = (int)cmd.ExecuteScalar();
                    int count = 0;
                    //Customer
                    cmd.CommandText = "SELECT COUNT(*) FROM [dbo].[Customer] WHERE Id = @id ";
                    cmd.Parameters.AddWithValue("@id", data);
                    count = (int)cmd.ExecuteScalar();
                    if(count==1)
                    {
                        response.Add("Customer");
                    }
                    //Employee
                    cmd.CommandText = "SELECT COUNT(*) FROM [dbo].[Employee] WHERE Id = @id2 ";
                    cmd.Parameters.AddWithValue("@id2", data);
                    count = (int)cmd.ExecuteScalar();
                    if (count == 1)
                    {
                        response.Add("Employee");
                    }

                    //SiteManager
                    cmd.CommandText = "SELECT COUNT(*) FROM [dbo].[SiteManager] WHERE Id = @id3 ";
                    cmd.Parameters.AddWithValue("@id3", data);
                    count = (int)cmd.ExecuteScalar();
                    if (count == 1)
                    {
                        response.Add("Manager");
                    }

                    //CustomerRepo
                    cmd.CommandText = "SELECT COUNT(*) FROM [dbo].[CustomerRep] WHERE Id = @id4 ";
                    cmd.Parameters.AddWithValue("@id4", data);
                    count = (int)cmd.ExecuteScalar();
                    if (count == 1)
                    {
                        response.Add("CustomerRep");
                    }
                    
                    return response.ToArray();
                }
            
            
            return new string[0];
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}
