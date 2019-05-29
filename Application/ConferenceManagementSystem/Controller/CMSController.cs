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
        public void addSection(string name, string room, DateTime date, int confId, int chairId)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                try
                {
                    String query = "INSERT INTO Sections VALUES ('" + name + "', '" + room + "', '" + date + "', " +chairId + ", " + confId + ")";
                    db.Execute(query);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void deleteSection(int id)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                String query = "DELETE FROM Sections WHERE ID = " + id.ToString();
                db.Execute(query);
            }
        }

        public void addReview(int paperId, int reviewerId, string qualifier, string comments)
        {
            List<String> papers;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {

                papers = db.Query<String>("SELECT Qualifier from Reviews WHERE PaperID=" + paperId).ToList();
                if( papers.Count > 4)
                {
                    throw new Exception("There are already 4 reviewers on this paper!");
                }
                String query = "INSERT INTO Reviews(PaperID,ReviewerID,Qualifier,Comments) VALUES (" + paperId + "," + reviewerId + ",'" + qualifier + "','" + comments + "')";
                db.Execute(query);

            }
        }
        public void AddConference(string ConferenceName, string ConferenceAddress, DateTime ConferenceDate)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                try
                {
                    String query = "INSERT INTO Conferences VALUES ('" + ConferenceName + "','" + ConferenceAddress + "','" + ConferenceDate.ToString() + "')";
                    db.Execute(query);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<ChosenPcMember> getChosen()
        {
            List<ChosenPcMember> pcs;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                pcs = db.Query<ChosenPcMember>("SELECT email, RoleName from ChosenPC C INNER JOIN Roles R ON C.RoleID = R.ID ").ToList();
                return pcs;
            }
        }

        public void addChosen(string email, string role)
        {
            int roleId = getRoleId(role);
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                String query = "INSERT INTO ChosenPC VALUES ('" + email + "'," + roleId + ")";
                db.Execute(query);
            }
        }

        public void deleteChosen(string email)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                String query = "DELETE FROM ChosenPC WHERE Email = '" + email + "'";
                db.Execute(query);
            }
        }

        private int getRoleId(string role)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                int roleId = db.QueryFirst<int>("SELECT ID from Roles WHERE RoleName = '" + role + "'");
                return roleId;
            }
        }

        public void addPaper(string PaperName, string Topic, string ContentLoc, string AbstractLoc, int SectionID, int AuthorID, int RoleID)
        {
            List<String> pid;
            List<String> affiliations;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                try
                {
                    string aff = "regular Member";
                    String query = "INSERT INTO Papers(ContentLoc,AbstractLoc,Topic,PaperName,SectionID,isAccepted) VALUES ('" + ContentLoc + "','" + AbstractLoc + "','" + Topic + "','" + PaperName + "'," + SectionID + ",0)";
                    db.Execute(query);
                    pid = db.Query<String>("SELECT ID FROM Papers WHERE ContentLoc='" + ContentLoc + "'").ToList();
                    int pidd = Int32.Parse(pid[0]);
                    
                    if (RoleID == 4)
                    {
                        affiliations = db.Query<String>( "SELECT Affiliation from Authors WHERE ID=" + AuthorID).ToList();
                        if (affiliations.Count == 0)
                        {
                            String query4 = "INSERT INTO Authors(ID,Affiliation) VALUES (" + AuthorID + ",'" + aff + "')";
                            db.Execute(query4);
                        }
                    }
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
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                String query = "UPDATE Users SET FirstName='"+firstName+"',LastName='"+lastName+"' WHERE ID="+id;
                db.Execute(query);
            }
            
        }

        public void updateAuthor(int id, string firstName, string lastName, string affiliation)
        {
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
            List<Paper> papers;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                papers = db.Query<Paper>("SELECT * FROM Papers").ToList();
                return papers;
            }
            
        }

        public List<Paper> getPapersOfSection(Section section)
        {
            List<Paper> papers;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                papers = db.Query<Paper>("SELECT * FROM Papers WHERE SectionID="+section.ID).ToList();
                return papers;
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

        public List<Section> getSections(int confId)
        {
            List<Section> section;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                section = db.Query<Section>("SELECT * FROM Sections WHERE ConferenceID = " + confId.ToString()).ToList();
                return section;
            }

        }

        public List<Section> getSections() {
            List<Section> sections;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString)) {
                sections = db.Query<Section>("SELECT * FROM Sections").ToList();
                return sections;
            }
        }

        public void updateSectionDeadline(int id, DateTime newDate) {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString)) {
                String query = "UPDATE Sections SET PaperDeadline='" + newDate + "' WHERE ID=" + id;
                db.Execute(query);
            }
        }

        public List<Section> getSectionsOfConference(Conference conference)
        {
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
                user = db.QueryFirst<User>("SELECT * FROM Users WHERE Username='" + username + "' AND Passwd='" + password + "'");

 
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
                    String query1 = "INSERT INTO Authors(ID,Affiliation) values('" +user.ID+ "','" + affiliation + ")";
                    db.Execute(query1);
                }
            }
        }

        public List<Review> GetReviewsForPaper(Paper paper)
        {
            List<Review> reviews;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                reviews = db.Query<Review>("SELECT * FROM Reviews WHERE PaperId = " + paper.ID).ToList();
                return reviews;
            }
        }

        public void reevalPaper(Paper paper)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cmsDatabase"].ConnectionString))
            {
                db.Execute("UPDATE Reviews SET ReevalRequest = 1 WHERE ID=" + paper.ID);
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
