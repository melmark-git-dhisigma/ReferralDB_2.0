using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Web;

namespace ReferalDBApplicant.Classes
{
    public class clsSessionTab  //:ISerializable
    {
        //protected clsSessionTab(SerializationInfo info, StreamingContext context)
        //{
        //    if (info == null)
        //        throw new ArgumentNullException("info");


        //    mSessTab1 = info.GetInt32("SessTab1");
        //    mSessTab2 = info.GetInt32("SessTab2");
        //    mSessTab3 = info.GetInt32("SessTab3");
        //    mSessTab4 = info.GetInt32("SessTab4");
        //    mSessTab5 = info.GetInt32("SessTab5");
        //    mSessTab6 = info.GetInt32("SessTab6");
        //    mSessTab7 = info.GetInt32("SessTab7");
        //    mSessTab8 = info.GetInt32("SessTab8");


        //}
        //[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        //protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        //{

        //    info.AddValue("SessTab1", mSessTab1);
        //    info.AddValue("SessTab2", mSessTab2);
        //    info.AddValue("SessTab3", mSessTab3);
        //    info.AddValue("SessTab4", mSessTab4);
        //    info.AddValue("SessTab5", mSessTab5);
        //    info.AddValue("SessTab6", mSessTab6);
        //    info.AddValue("SessTab7", mSessTab7);
        //    info.AddValue("SessTab8", mSessTab8);

        //}

        //[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        //void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    if (info == null)
        //        throw new ArgumentNullException("info");

        //    GetObjectData(info, context);
        //}
        //public clsSessionTab()
        //{

        //}


        //private int mSessTab1 = 0;
        //public int SessTab1
        //{
        //    get
        //    {
        //        return mSessTab1;
        //    }
        //    set
        //    {
        //        mSessTab1 = value;
        //    }
        //}

        //private int mSessTab2 = 0;
        //public int SessTab2
        //{
        //    get
        //    {
        //        return mSessTab2;
        //    }
        //    set
        //    {
        //        mSessTab2 = value;
        //    }
        //}

        //private int mSessTab3 = 0;
        //public int SessTab3
        //{
        //    get
        //    {
        //        return mSessTab3;
        //    }
        //    set
        //    {
        //        mSessTab3 = value;
        //    }
        //}

        //private int mSessTab4 = 0;
        //public int SessTab4
        //{
        //    get
        //    {
        //        return mSessTab4;
        //    }
        //    set
        //    {
        //        mSessTab4 = value;
        //    }
        //}

        //private int mSessTab5 = 0;
        //public int SessTab5
        //{
        //    get
        //    {
        //        return mSessTab5;
        //    }
        //    set
        //    {
        //        mSessTab5 = value;
        //    }
        //}

        //private int mSessTab6 = 0;
        //public int SessTab6
        //{
        //    get
        //    {
        //        return mSessTab6;
        //    }
        //    set
        //    {
        //        mSessTab6 = value;
        //    }
        //}

        //private int mSessTab7 = 0;
        //public int SessTab7
        //{
        //    get
        //    {
        //        return mSessTab7;
        //    }
        //    set
        //    {
        //        mSessTab7 = value;
        //    }
        //}

        //private int mSessTab8 = 0;
        //public int SessTab8
        //{
        //    get
        //    {
        //        return mSessTab8;
        //    }
        //    set
        //    {
        //        mSessTab8 = value;
        //    }
        //}





        public int SessTab1 { get; set; }
        public int SessTab2 { get; set; }
        public int SessTab3 { get; set; }
        public int SessTab4 { get; set; }
        public int SessTab5 { get; set; }
        public int SessTab6 { get; set; }
        public int SessTab7 { get; set; }
        public int SessTab8 { get; set; }
    }

}