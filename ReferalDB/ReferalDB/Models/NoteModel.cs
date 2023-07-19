using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace ReferalDB.Models
{
    public class NoteModel
    {
        public static MelmarkDBEntities objData = null;
        public string Notes { get; set; }
        public IList<NoteListClass> NoteList { get; set; }
        public NoteModel()
        {
            NoteList = new List<NoteListClass>();
        }

        public static NoteModel BindNote(int Schoolid)
        {
            objData = new MelmarkDBEntities();
            IList<NoteListClass> retunmodel = new List<NoteListClass>();
            NoteModel nte = new NoteModel();
            DateTime dttwntydaybfr = new DateTime();
            dttwntydaybfr = DateTime.Now.AddDays(-20);
            retunmodel = (from x in objData.ref_Notes
                          join objusr in objData.Users
                          on x.CreatedBy equals objusr.UserId
                          join objref in objData.StudentPersonals
                          on x.StudentPersonalId equals objref.StudentPersonalId
                          where x.SchoolId == Schoolid
                          orderby x.NoteId descending
                          select new NoteListClass
                          {
                              NoteId = x.NoteId,
                              Notes = x.Notes,
                              CreatedOn = x.CreatedOn,
                              UserName=objusr.UserLName+","+objusr.UserFName,
                              RefName=objref.LastName+","+objref.FirstName
                          }).ToList();

            nte.NoteList = retunmodel.Where(x=>x.CreatedOn>=dttwntydaybfr).ToList();
            return nte;
        }
    }

    public class NoteListClass
    {
        public int NoteId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserName { get; set; }
        public string RefName { get; set; }
        public string Notes { get; set; }
    }
}