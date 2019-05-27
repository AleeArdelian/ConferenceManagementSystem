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
using ConferenceManagementSystem.Domain;

namespace ConferenceManagementSystem.Controller
{
    public class CMSController
    {
        public void AddConference(string ConferenceName, string ConferenceAddress, string ConferenceDate)
        {
            throw new NotImplementedException();
        }

        public void attendConference(Conference conference, User user)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                try
                {
                    String query = "INSERT INTO ConferenceUsers(ConferenceID,UserID) VALUES(" + conference.ID + "," + user.ID + ")";
                    db.Execute(query);
                }
                catch (SqlException)
                {
                    throw new Exception("you already attend this conference");
                }
            }


        }

        public List<Conference> getConferences()
        {
            List<Conference> conferences;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                conferences = db.Query<Conference>("SELECT * FROM Conferences").ToList();
                return conferences;
            }

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

        public void registerAuthor(string username, string passwd, string fname, string lname, string email, string affiliation)
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
                    String query = "INSERT INTO Users(FirstName,LastName,Username,Passwd,email,RoleID) values('" + fname + "','" + lname + "','" + username + "','" + passwd + "','" + email + "',1)";
                    db.Execute(query);
                    User user = db.QueryFirst<User>("SELECT * FROM Users WHERE Username='" + username + "' AND Passwd='" + passwd + "'");
                    String query1 = "INSERT INTO Authors(ID,Affiliation) values("+user.ID+",'" + affiliation + "')";
                    db.Execute(query1);
                }
            }
        }

        public void registerPCMember(string username, string passwd, string fname, string lname, string email, string affiliation, string website)
        {
            List<String> res;
            List<String> res1;
            List<String> res2;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                res = db.Query<String>("SELECT FirstName FROM Users WHERE Username='" + username + "'").ToList();
                res1 = db.Query<String>("SELECT FirstName FROM Users WHERE email='" + email + "'").ToList();
                res2 = db.Query<String>("SELECT RoleID FROM ChosenPC WHERE email='" + email + "'").ToList();
                if (res.Capacity > 0 || res1.Capacity > 0)
                    throw new Exception("Username/Email already in use");
                if (res2.Capacity == 0)
                    throw new Exception("You don't have the right to register as a PC Member");
                else
                {
                    int xv = Int32.Parse(res2[0]);
                    String query = "INSERT INTO Users(FirstName,LastName,Username,Passwd,email,RoleID) values('" + fname + "','" + lname + "','" + username + "','" + passwd + "','" + email + "',"+xv+")";
                    db.Execute(query);
                    User user = db.QueryFirst<User>("SELECT * FROM Users WHERE Username='" + username + "' AND Passwd='" + passwd + "'");
                    String query1 = "INSERT INTO PCMembers(ID,Affiliation,website,isReviewer) values(" + user.ID + ",'" + affiliation + "','" + website + "',0)";
                    db.Execute(query1);
                }
            }
        }
    }
}
