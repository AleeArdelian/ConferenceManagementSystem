using ConferenceManagementSystem.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ConferenceManagementSystem.Controller
{
    public class CMSController
    {
        public void AddConference(string ConferenceName, string ConferenceAddress, string ConferenceDate)
        {
            throw new NotImplementedException();
        }

        public User LogIN(string username, string password)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                User user = null;
                try
                {
                    user = db.QueryFirst<User>("SELECT * FROM Users WHERE Username='" + username + "' AND Passwd='" + password + "'");
                }
                catch (SqlException)
                {
                    throw new Exception("User not found");
                }
                if (user.RoleID == 1)
                {
                    Author author = db.QueryFirst<Author>("SELECT U.ID, FirstName, LastName, RoleID, email, Username, Passwd, Affiliation FROM Users U INNER JOIN PCMembers A on U.ID=A.ID WHERE Username='" + username + "' AND Passwd='" + password + "'");
                    return author;
                }
                else if(user.RoleID == 2 || user.RoleID == 3 || user.RoleID == 4)
                {
                    PCMember pc = db.QueryFirst<PCMember>("SELECT U.ID, FirstName, LastName, RoleID, email, Username, Passwd, Affiliation, website FROM Users U INNER JOIN PCMembers A on U.ID=A.ID WHERE Username='" + username + "' AND Passwd='" + password + "'");
                    return pc;
                }
                return user;
            }
        }

        public void registerListener(string username, string passwd, string fname, string lname, string email)
        {
            List<String> res;
            List<String> res1;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                res = db.Query<String>("SELECT FirstName FROM Users WHERE Username='" + username + "'").ToList();
                res1 = db.Query<String>("SELECT FirstName FROM Users WHERE email='" + email + "'").ToList();
                if (res.Capacity > 0 || res1.Capacity > 0)
                    throw new Exception("Username/Email already in use");
                else
                {
                    String query = "INSERT INTO Users(FirstName,LastName,Username,Passwd,email,RoleID) values('" +fname+"','"+lname+"','"+username+"','"+passwd+"','"+email+"',5)";
                    db.Execute(query);
                }
            }
        }
    }
}
