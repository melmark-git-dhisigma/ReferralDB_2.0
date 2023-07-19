using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace ReferalDB.Models
{
    public class UserModel
    {
        public static MelmarkDBEntities objData = null;
        public IList<UserListClass> UserList { get; set; }
        public IList<TeamListClass> TeamList { get; set; }
        public IList<TeamidClass> TeamIdList { get; set; }
        public string TeamName { get; set; }
        public IList<string> userlistfromcl { get; set; }

       
        public int DrpTeamSelc { get; set; }
        public string SelectdPersonals { get; set; }
        public IList<studentDetails> StdList { get; set; }
        

        
        

        public UserModel()
        {
            userlistfromcl = new List<string >();
            UserList = new List<UserListClass>();
            TeamList = new List<TeamListClass>();
            TeamIdList = new List<TeamidClass>();
        }


        public static IList<UserListClass> BindUserList(int SchoolId)
        {
            objData = new MelmarkDBEntities();
            IList<UserListClass> usermodel = new List<UserListClass>();            
            UserModel user = new UserModel();
            var SelectUser = objData.Users.ToList().Where(x=>x.ActiveInd=="A" && x.SchoolId==SchoolId);
            usermodel = (from x in SelectUser
                         select new UserListClass
                          {
                              UserFirstName = x.UserFName,
                              UserLastName = x.UserLName,
                              UserId = x.UserId
                          }).ToList();
            
            
            return usermodel;

        }

        public static IList<TeamidClass> BindteamId(int SchoolId)
        {
            objData = new MelmarkDBEntities();
            IList<TeamidClass> tmmodel = new List<TeamidClass>();
            UserModel user = new UserModel();
            var SelectUser = objData.ReviewTeams.ToList().Where(x => x.SchoolId == SchoolId);
            tmmodel = (from x in SelectUser
                         select new TeamidClass
                         {
                             TeamId = x.TeamId,
                             TeamName = x.TeamName
                         }).ToList();            
            return tmmodel;

        }

        public static UserModel BindReviewTeam(int SchoolId)
        {
            objData = new MelmarkDBEntities();
            IList<TeamListClass> retunmodel = new List<TeamListClass>();
            IList<TeamidClass> Idmodel = new List<TeamidClass>();
            IList<UserListClass> usrmodel = new List<UserListClass>();
            UserModel Objusr = new UserModel();
            retunmodel = (from x in objData.ReviewTeams 
                          join y in objData.TeamMembers
                          on x.TeamId equals y.TeamId 
                          join z in objData.Users
                          on y.UserId equals z.UserId
                          where x.SchoolId==SchoolId
                          select new TeamListClass
                          {
                             TeamId=x.TeamId,
                             TeamName=x.TeamName,
                             UserFirstName=z.UserFName, 
                             UserLastName=z.UserLName
                          }).ToList();

            Idmodel = BindteamId(SchoolId);
            usrmodel = BindUserList(SchoolId);
            Objusr.TeamIdList = Idmodel;
            Objusr.TeamList = retunmodel;
            Objusr.UserList = usrmodel;
            return Objusr;

        }

        public static string EditReviewTeam(int TeamId,int Schoolid)
        {
             
            objData = new MelmarkDBEntities();
            var Reviewtm = objData.ReviewTeams.Where(objreview => objreview.TeamId == TeamId && objreview.SchoolId == Schoolid).SingleOrDefault();
            string TeamDetails = Reviewtm.TeamName;
            var TeamMembers = objData.TeamMembers.Where(objtmembers => objtmembers.TeamId == TeamId && objtmembers.SchoolId == Schoolid).ToList();
            foreach(var member in TeamMembers)
            {
                TeamDetails += "," + "chklist" + member.UserId;
            }
            return TeamDetails;
        }
    }


    public class UserListClass
    {
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public bool userchecked{get;set;}
        
    }

    public class TeamListClass
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }

    }
    public class TeamidClass
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }

    }
    public class studentDetails
    {
        public int studentPersonalId { get; set; }
        public string studentPersonal { get; set; }
    }

}