using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using ReferalDB.Models;
using ReferalDB.AppFunctions;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace ReferalDB.Models
{
    public class ListContactModel
    {
        public virtual string Searchtext { get; set; }
        public virtual IList<GridList> listContacts { get; set; }
        public virtual PagingModel pageModel { get; set; }
        public static MelmarkDBEntities RPCobj = new MelmarkDBEntities();
        static int homeContact = 1, studentAddress = 1, Active = 1;
        public static clsSession1 sess1 = null;
        public static clsSession sess = null;
        
        public static ListContactModel fillContacts(int page, int pageSize)
        {
            ContactPersonal stdtContactPersonal = new ContactPersonal();
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            ListContactModel listModel = new ListContactModel();
            IList<GridList> retunmodel = new List<GridList>();
            listModel.pageModel.CurrentPageIndex = page;
            listModel.pageModel.PageSize = pageSize;
            IList<ContactPersonal> result = new List<ContactPersonal>();
            if (sess != null)
            {
                try
                {

                    retunmodel = (from objContactPersonal in RPCobj.ContactPersonals
                                  join objContactRelation in RPCobj.StudentContactRelationships on objContactPersonal.ContactPersonalId equals objContactRelation.ContactPersonalId
                                  join objLookUp in RPCobj.LookUps on objContactRelation.RelationshipId equals objLookUp.LookupId
                           //       join objStdtAddresRel in RPCobj.StudentAddresRels on objContactPersonal.StudentPersonalId equals objStdtAddresRel.StudentPersonalId
                             //     join objAddressList in RPCobj.AddressLists on objStdtAddresRel.AddressId equals objAddressList.AddressId
                                  where (objContactPersonal.StudentPersonalId == sess.ReferralId && objContactPersonal.Status == Active )
                                  select new GridList
                                  {
                                      ContactId = objContactPersonal.ContactPersonalId,
                                      Name = objContactPersonal.LastName + " " + objContactPersonal.FirstName,
                                      Relation = objLookUp.LookupName,
                                      RelationDesc = objContactPersonal.Spouse,
                                      ProjectType = objContactPersonal.ContactFlag
                                  }).ToList();
                   
                    //listModel.pageModel.TotalRecordCount = retunmodel.Count;
                    //retunmodel = retunmodel.OrderByDescending(objContacts => objContacts.ContactId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    listModel.listContacts = retunmodel;
                    //if (listModel.pageModel.PageSize > listModel.pageModel.TotalRecordCount) { listModel.pageModel.PageSize = listModel.pageModel.TotalRecordCount; }
                    //if (listModel.pageModel.TotalRecordCount == 0) { listModel.pageModel.CurrentPageIndex = 0; }

                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }

            }

         

            return listModel;
        }

        public ListContactModel()
        {
            pageModel = new PagingModel();
            listContacts = new List<GridList>();
        }
    }
    public class GridList
    {
        public virtual int ContactId { get; set; }
        public virtual string Name { get; set; }
        public virtual string RelationDesc { get; set; }
        public virtual string Relation { get; set; }
        public virtual AddressList addrlist { get; set; }
     
        public virtual string ProjectType { get; set; }
    }

}