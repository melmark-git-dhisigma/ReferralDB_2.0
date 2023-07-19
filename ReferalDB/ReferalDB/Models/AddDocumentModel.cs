using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReferalDB.Models
{
    public class AddDocumentModel
    {
        public virtual int Id { get; set; }
        public virtual IEnumerable<SelectListItem> DocumentTypeList { get; set; }
        public virtual int? DocumentType { get; set; }
        public virtual IEnumerable<SelectListItem> DocumentModuleList { get; set; }
        public virtual int? DocumentModule { get; set; }
        public virtual string DocumentName { get; set; }
        public HttpFileCollectionBase profilePicture { get; set; }
        public virtual string Other { get; set; }
    }
}