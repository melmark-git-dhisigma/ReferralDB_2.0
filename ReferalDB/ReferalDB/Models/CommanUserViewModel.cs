using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class CommanUserViewModel
    {
        public IEnumerable<UsersListModel> userList { get; set; }
        public string userIdz { get; set; }
        public string CheckListId { get; set; }
        public int ChkCount { get; set; }
        public string ChkCountj { get; set; }
        public string ChkCounti { get; set; }
    }
}