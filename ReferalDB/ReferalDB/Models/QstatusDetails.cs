using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class QstatusDetails
    {
        public int QueueStatusId;
        public string Type;
        public bool CurrentStatus;
        public string DraftStatus;
        public int qProcess;
        public QstatusDetails()
        {
            QueueStatusId = 0;
        }
    }
}