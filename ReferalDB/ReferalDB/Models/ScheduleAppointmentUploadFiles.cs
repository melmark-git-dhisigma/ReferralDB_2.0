using ReferalDB.AppFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReferalDB.Models
{
    public class ScheduleAppointmentUploadFiles
    {
        Other_Functions objOther = new Other_Functions();
        public virtual string IEPName { get; set; }
        public virtual bool? Verified { get; set; }
        public virtual int Varify { get; set; }
        public virtual string SignedBy { get; set; }
        public virtual DateTime? SignedOn { get; set; }
        public virtual int IEPId { get; set; }
        public virtual string IEPPath { get; set; }
        public virtual int QueueStatusId { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual IList<DocumentList2> DocumentList { get; set; }
        public virtual string DocumentName{get;set;}
        public virtual string DocumentType { get; set; }
        public virtual string UploadFile { get; set; }
        public virtual IEnumerable<SelectListItem> DocumentTypeList { get; set; }
        public virtual string OtherFDocType { get; set; }
        public virtual string DocVisible { get; set; }
        public ScheduleAppointmentUploadFiles()
        {
            DocumentList = new List<DocumentList2>();
            DocumentTypeList = objOther.getDocumentType();
        }
    }
    public class DocumentList2
    {
        public virtual string DocName { get; set; }
        public virtual int DocId { get; set; }
        public virtual string DocPath { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual string CreatedDate { get; set; }

    }

   

    public class DocumentListWithBinary2
    {
        public virtual string DocName { get; set; }
        public virtual string ContentType { get; set; }
        public virtual int DocId { get; set; }
        public virtual byte[] Data { get; set; }
    }

}