using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using BuisinessLayer;
using System.Web.Mvc;
using ReferalDB.Controllers;

namespace ReferalDB.Models
{
    public class EngineViewModels
    {
        public static MelmarkDBEntities objData = null;
        public int SchoolId { get; set; }

        public int LetterEngineItemId { get; set; }
        public string ItemContent { get; set; }
        public int CreatedBy { get; set; }
        public int QueueId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int LetterEngineId { get; set; }
        public bool ApproveStatus { get; set; }
        public int? ApproveStatusVal { get; set; }
        public int? ApproveSt { get; set; }
        public string LetterEngineName { get; set; }
        public string ChecklistEngineName { get; set; }
        public string LetterAssigneStatus { get; set; }
        public string LetterTempType { get; set; }
       
        public int? QueueTypeId { get; set; }
        public IList<itemListClass> listItem { get; set; }
        public IList<ContentListClass> ContentlistItem { get; set; }
        public IList<QueueStatus> QueueList { get; set; }
        public IList<SelectListItem> QueueItems { get; set; }
        public IList<ChecklistOtherClass> checklistother { get; set; }
        public IList<ChecklistitemOtherClass> checklistitemother { get; set; }
       
        public static EngineViewModels BindLetterEngine(int SchoolId)
        {
            objData = new MelmarkDBEntities();
            IList<itemListClass> retunmodel = new List<itemListClass>();
            IList<ContentListClass> retunmodel1 = new List<ContentListClass>();
            EngineViewModels Engine = new EngineViewModels();
            var SelectLetterTemplate = objData.LetterEngines.ToList().Where(x => x.SchoolId == SchoolId && (x.QueueId != 6 && x.QueueId != 8 &&
                         x.QueueId != 20 && x.QueueId != 21 && x.QueueId != 22 && x.QueueId != 23 && x.QueueId != 24));
            retunmodel = (from x in SelectLetterTemplate    
                          orderby x.LetterEngineId descending 
                            select new itemListClass
                            {
                                LetterEngineName = x.LetterEngineName,
                                LetterEngineId = x.LetterEngineId,
                                LetterEngineType=x.LetterType,
                                QueueId=x.QueueId,
                                ApproveStatus =(bool)x.ApproveStatus
                                
                            }).ToList();
            //set QName
            if (retunmodel.Count > 0)
            {
                foreach (var item in retunmodel)
                {
                    if (item.QueueId != null)
                    {
                        //get QueueName
                        var QueueList = objData.ref_Queue.Where(objQueue => objQueue.QueueId == item.QueueId).ToList();
                        item.QueueName = QueueList[0].QueueName;
                    }
                }
            }

            var SelectChkTemplate = objData.LetterEngineItems.ToList().Where(x => x.SchoolId == SchoolId);
            retunmodel1 = (from x in SelectChkTemplate
                          select new ContentListClass
                          {
                              chkEngineId = x.LetterEngineId,
                              chkEngineitemId = x.LetterEngineItemId,
                              ChecklistContent = x.ItemContent
                          }).ToList();


            Engine.listItem = retunmodel;
            Engine.ContentlistItem = retunmodel1;
            return Engine;

        }




        public static EngineViewModels LoadChecklistEngineItem(int SchoolId)
        {
            objData = new MelmarkDBEntities();
            IList<QueueStatus> QueueList = new List<QueueStatus>();
            IList<ChecklistOtherClass> ChecklistHeader = new List<ChecklistOtherClass>();
            IList<ChecklistitemOtherClass> chklistitem = new List<ChecklistitemOtherClass>();
            EngineViewModels Engine = new EngineViewModels();
            var Queue = (from objqueue in objData.ref_Queue
                         where objqueue.MasterId != 0 && (objqueue.QueueType != "NA" && objqueue.QueueType != "RT" && objqueue.QueueType != "MO" && objqueue.QueueType != "AT" &&
                         objqueue.QueueType != "SM" && objqueue.QueueType != "IP" && objqueue.QueueType != "PM" && objqueue.QueueType != "CM" && objqueue.QueueType != "CT" && objqueue.QueueType != "OTH")// && objqueue.QueueType != "CR" && objqueue.QueueType != "AR" && objqueue.QueueType != "CA")
                         select new QueueStatus
                         {
                             QueueName=objqueue.QueueName,
                             QueueId=objqueue.QueueId
                         }).ToList();
            var Checklistheading = (from objchk in objData.ref_Checklist
                                    where objchk.ChecklistHeaderId==0
                                    select new ChecklistOtherClass
                                    {
                                        ChecklistId = objchk.ChecklistId,
                                        ChecklistName=objchk.ChecklistName,
                                        QueueId = objchk.QueueId
                                    }).ToList();
            var checklistitem = (from objchk in objData.ref_Checklist
                                 where objchk.ChecklistHeaderId != 0
                                 select new ChecklistitemOtherClass
                                 {
                                     ChecklistId = objchk.ChecklistHeaderId,
                                     ChecklistitemId = objchk.ChecklistId,
                                     ChecklistitemName = objchk.ChecklistName
                                 }).ToList();


            Engine.QueueList = Queue;
            Engine.checklistother = Checklistheading;
            Engine.checklistitemother = checklistitem;
            return Engine;

        }


        public EngineViewModels()
        {
            listItem = new List<itemListClass>();
            ContentlistItem = new List<ContentListClass>();
            QueueList = new List<QueueStatus>();
            QueueItems = new List<SelectListItem>();
            checklistother =new List<ChecklistOtherClass>();
            checklistitemother = new List<ChecklistitemOtherClass>();
            EngineController engin=new EngineController();
            QueueItems = engin.FillDropQueueList();
        }

    }

    public class itemListClass
    {
        public int LetterEngineId { get; set; }
        public string LetterEngineName { get; set; }
        public string LetterEngineType { get; set; }
        public bool ApproveStatus { get; set; }
        public int? QueueId { get; set; }
        public string QueueName { get; set; }
    }
    public class ContentListClass
    {
        public int chkEngineId { get; set; }
        public int chkEngineitemId { get; set; }
        public string ChecklistContent { get; set; }
    }
    public class QueueStatus
    {
        public int QueueId { get; set; }
        public string QueueName { get;set; }
        public string QueueType { get; set; }
    }

    public class ChecklistOtherClass
    {
        public int QueueId { get; set; }
        public int ChecklistId { get; set; }
        public string ChecklistName { get; set; }
    }

    public class ChecklistitemOtherClass
    {
        public int ChecklistId { get; set; }
        public int ChecklistitemId { get; set; }
        public string ChecklistitemName { get; set; }
    }

}