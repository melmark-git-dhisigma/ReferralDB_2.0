using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class ApplicantUploadDownload
    {
        public MelmarkDBEntities objData = null;
        public int FileUpload(int StudentId, int SchoolId, string DocName, string DocPath, int UserId)
        {
            int rtrnval = -1;
            objData = new MelmarkDBEntities();
            LookUp lookup = new LookUp();
            lookup = objData.LookUps.Where(obj => obj.LookupType == "Document Type" && obj.LookupName == "Placement Agreement").SingleOrDefault();
            if (lookup != null)
            {
                Document tblDoc = new Document();
                tblDoc.DocumentName = DocName;
                tblDoc.DocumentType = lookup.LookupId;
                tblDoc.DocumentPath = DocPath;
                tblDoc.SchoolId = SchoolId;
                tblDoc.StudentPersonalId = StudentId;
                tblDoc.Status = true;
                tblDoc.UserType = "Staff";
                tblDoc.CreatedBy = UserId;
                tblDoc.CreatedOn = DateTime.Now;
                objData.Documents.Add(tblDoc);
                objData.SaveChanges();
                rtrnval = tblDoc.DocumentId;
            }
            return rtrnval;
        }
    }
}