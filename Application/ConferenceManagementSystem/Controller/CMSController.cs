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
            /*
             * adds a conference
             * pre: conference name (string), conference address (string), conference date (string)
             * post: 
             */
            throw new NotImplementedException();
        }

        public void addPaper(string PaperName, string Topic, string ContentLoc, string AbstractLoc, int SectionID, int AuthorID)
        {
            /*
             * adds a new paper in the DB
             * pre: paper name (string), topic (string), the path in the disc of the paper (string), the content on the disc of the abstract (string), the id of the section for the paper (integer), the author id (integer)
             * post: the paper is added to the AuthorPapers table and Papers table
             */
            List<String> pid;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                try
                {
                    String query = "INSERT INTO Papers(ContentLoc,AbstractLoc,Topic,PaperName,SectionID,isAccepted) VALUES ('" + ContentLoc + "','" + AbstractLoc + "','" + Topic + "','" + PaperName + "'," + SectionID + ",0)";
                    db.Execute(query);
                    pid = db.Query<String>("SELECT ID FROM Papers WHERE ContentLoc='" + ContentLoc + "'").ToList();
                    int pidd = Int32.Parse(pid[0]);
                    String query1 = "INSERT INTO AuthorPapers(AuthorID,PaperID) VALUES (" + AuthorID + "," + pidd + ")";
                    db.Execute(query1);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void updateListener(int id, string firstName, string lastName)
        {
            /*
             * updates the data of a registered listener
             * pre: listener id(int), first name (string), last name (string)
             * post: the listener with the ID = id is updated in the DB
             */
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                String query = "UPDATE Users SET FirstName='"+firstName+"',LastName='"+lastName+"' WHERE ID="+id;
                db.Execute(query);
            }
            
        }

        public void updateAuthor(int id, string firstName, string lastName, string affiliation)
        {
            /*
             * updates the data of an author
             * pre: author id (int), first name (string), last name (string), affiliation (string)
             * post: the author with the ID=id is updated in the tables Authors and Users
             */
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                String query = "UPDATE Users SET FirstName='" + firstName + "',LastName='" + lastName + "' WHERE ID=" + id;
                String query1 = "UPDATE Authors SET Affiliation='" + affiliation + "' WHERE ID=" + id;
                db.Execute(query);
                db.Execute(query1);
            }
        }

        public void updatePCMember(int id, string firstName, string lastName, string affiliation, string website)
        {
            /*
             * updates the data of a PC Member
             * pre: PC Memeber id (integer), first name (string), last name (string), affiliation (string), website (string)
             * post: the PC Member with the ID=id is updated in the Users and the PCMembers tables
             */
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                String query = "UPDATE Users SET FirstName='" + firstName + "',LastName='" + lastName + "' WHERE ID=" + id;
                String query1 = "UPDATE PCMembers SET Affiliation='" + affiliation + "', website='"+website+"' WHERE ID=" + id;
                db.Execute(query);
                db.Execute(query1);
            }
        }

        public void attendConference(Conference conference, User user)
        {
            /*
             * adds a participant to a given conference
             * pre: conference and user
             * post: adds the user id and the conference id in the ConferenceUser table, or throws an exception if the user is already marked as attending the given conference
             */
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

        public List<Paper> getPapers()
        {
            /*
             * gets all the papers from the DB
             * pre: -
             * post: returns a list with all the papers
             */
            List<Paper> papers;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                papers = db.Query<Paper>("SELECT * FROM Papers").ToList();
                return papers;
            }
            
        }

        public List<Paper> getPapersOfSection(Section section)
        {
            /*
             * gets all the papers from a given section from the DB
             * pre: a section (Section)
             * post: returns a list with all the papers from the given section
             */
            List<Paper> papers;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                papers = db.Query<Paper>("SELECT * FROM Papers WHERE SectionID="+section.ID).ToList();
                return papers;
            }
        }

        public List<Conference> getConferences()
        {
            /*
             * get all the conferences from the DB
             * pre: -
             * post: returns a list with all the conferences
             */
            List<Conference> conferences;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                conferences = db.Query<Conference>("SELECT * FROM Conferences").ToList();
                return conferences;
            }

        }

        public List<Section> getSections() {
            /*
             * gets all the sections from the DB
             * pre: -
             * post: returns a list with all the sections
             */
            List<Section> sections;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString)) {
                sections = db.Query<Section>("SELECT * FROM Sections").ToList();
                return sections;
            }
        }

        public void updateSectionDeadline(int id, DateTime newDate) {
            /*
             * updates the PaperDeadline field for the sections with the given id from the table Sections
             * pre: the section id (int) and the new deadline (DateTime)
             * post: -
             */
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString)) {
                String query = "UPDATE Sections SET PaperDeadline='" + newDate + "' WHERE ID=" + id;
                db.Execute(query);
            }
        }

        public List<Section> getSectionsOfConference(Conference conference)
        {
            /*
             * gets all the sections from a given conference
             * pre: a conference (Conference)
             * post: a list with all the sections 
             */
            List<Section> sections;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                sections = db.Query<Section>("SELECT S.ID, S.SectionName, S.RoomName, S.PaperDeadline, S.ChairID, S.ConferenceID FROM Sections S INNER JOIN Conferences C ON S.ConferenceID = C.ID WHERE C.ID="+conference.ID).ToList();
                return sections;
            }
        }

        public List<Conference> getMyConferences(User user)
        {
            List<Conference> conferences;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                conferences = db.Query<Conference>("SELECT C.ID,C.ConferenceName,C.ConferenceAddress,C.ConferenceDate FROM Conferences C INNER JOIN ConferenceUsers U ON C.ID=U.ConferenceID WHERE U.UserID="+user.ID).ToList();
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
                    Author author = db.QueryFirst<Author>("SELECT U.ID, FirstName, LastName, RoleID, email, Username, Passwd, Affiliation FROM Users U INNER JOIN Authors A on U.ID=A.ID WHERE Username='" + username + "' AND Passwd='" + password + "'");
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
