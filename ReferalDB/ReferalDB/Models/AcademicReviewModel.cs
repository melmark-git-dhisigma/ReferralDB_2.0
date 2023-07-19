using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace ReferalDB.Models
{
    public class AcademicReviewModel
    {
        public int? numberofrow { get; set; }
        public int? rowid { get; set; }
        public int assignedTo { get; set; }
        public string assign { get; set; }
        public DateTime dueDate { get; set; }
        public int completed { get; set; }
        public string comments { get; set; }
        public int approvedStatus { get; set; }
        public int academicReviewId { get; set; }
        public virtual string DocumentName { get; set; }
        public IList<string> checklist { get; set; }
        public IList<CommmonCheckListViewModel> engineLetter { get; set; }
        public IList<UsersListModel> userList { get; set; }
        public ScheduleParentScreeningViewModel ParentScreening { get; set; }
        public CommonAccRevComntsViewModel commonACCREV { get; set; }
        public IList<CommonCallLogViewModel> commonCallLog { get; set; }
        public IList<LetterTrayViewModel> LetterList { get; set; }
        public IList<DocumentDownloadViewModel> DocList { get; set; }
        public IList<CommonMulHeadViewMode> ChkAll { get; set; }
        public AcademicReviewModel()
        {
            commonACCREV = new CommonAccRevComntsViewModel();
            engineLetter = new List<CommmonCheckListViewModel>();
            userList = new List<UsersListModel>();
            ParentScreening = new ScheduleParentScreeningViewModel();
            commonCallLog = new List<CommonCallLogViewModel>();
            LetterList = new List<LetterTrayViewModel>();
            DocList = new List<DocumentDownloadViewModel>();
            ChkAll = new List<CommonMulHeadViewMode>();
        }
    }
    
}