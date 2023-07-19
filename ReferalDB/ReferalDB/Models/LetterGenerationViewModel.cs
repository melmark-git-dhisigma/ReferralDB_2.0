using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using ReferalDB.Controllers;

namespace ReferalDB.Models
{
    public class LetterGenerationViewModel
    {
        public static MelmarkDBEntities objData = null; //---- List 3 - Task #30 [20-Oct-2020] ---// 
        public virtual IList<LetterList> LetterLists { get; set; }
        public virtual string refFlag { get; set; }
        public IList<SelectListItem> QueueItems { get; set; } //---- List 3 - Task #30 [20-Oct-2020] ---// 
        public int? QueueTypeId { get; set; } //---- List 3 - Task #30 [20-Oct-2020] ---// 

        public LetterGenerationViewModel()
        {
            LetterLists = new List<LetterList>();
            QueueItems = new List<SelectListItem>(); //---- List 3 - Task #30 [20-Oct-2020] ---// 
        }
    }
    public class LetterList
    {
        public virtual string LetterName { get; set; }
        public virtual string ReferralName { get; set; }
        public virtual string ReferralFName { get; set; }
        public virtual string ReferralLName { get; set; }
        public virtual string RecipientName { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual bool status { get; set; }
        public virtual DateTime? SentOn { get; set; }
        public virtual string checkListval { get; set; }
        public virtual int LetterTrayId { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual string SentDate { get; set; }
        public virtual int LetterQueueId { get; set; } //---- List 3 - Task #30 [20-Oct-2020] ---// 
    }


}