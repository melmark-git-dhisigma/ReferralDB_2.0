using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class TeamAssignViewModel
    {
        public IList<teamUserDetails> TeamUsers { get; set; }
        public bool iSSubmitted { get; set; }
        public TeamAssignViewModel()
        {
            TeamUsers = new List<teamUserDetails>();
        }

        public class teamUserDetails
        {

            public int TeamAssignId { get; set; }
            public int TeamId { get; set; }
            public string TeamName { get; set; }
            public bool Complete { get; set; }
            public bool IsPresent { get; set; }
            public string checkListval { get; set; }
            public string UserNames { get; set; }
            public teamUserDetails()
            {

            }
        }


    }
    public class userNames
    {
        public int TeamId { get; set; }
        public List<string> UserNames { get; set; }
        public userNames()
        {
            UserNames = new List<string>();
        }
        //public string> UserNames { get; set; } 
    }
}