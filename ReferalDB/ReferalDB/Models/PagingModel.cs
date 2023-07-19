using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferalDB.Models
{
    public class PagingModel
    {


        public virtual int CurrentPageIndex { get; set; }
        public virtual int PageSize { get; set; }
        public virtual int TotalRecordCount { get; set; }
        public virtual int PageCount
        {
            get
            {
                if (this.PageSize > 0)
                {
                    int initialCount = this.TotalRecordCount / this.PageSize;
                    int checkCount = this.TotalRecordCount % this.PageSize;
                    int addCount = 0;
                    if (checkCount > 0)
                        addCount = 1;
                    return initialCount + addCount;
                }
                else
                    return 0;
            }
        }
        public int NumericPageCount;

        public virtual string SearchKeyword { get; set; }
        public virtual string FilterStatus { get; set; }

        public PagingModel()
        {
            this.PageSize = 10;
            this.NumericPageCount = 10;
        }
    }
}