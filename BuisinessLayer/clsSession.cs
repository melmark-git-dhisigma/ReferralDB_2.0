using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

[Serializable]
public class clsSession : ISerializable
{
    public clsSession()
    {

    }

    protected clsSession(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            throw new ArgumentNullException("info");
        mIsLogin = info.GetBoolean("mIsLogin");
        mSessionID = info.GetString("mSessionID");
        mUserName = info.GetString("mUserName");
        mGender = info.GetString("mGender");
        mLoginId = info.GetInt32("mLoginId");
        mSchoolId = info.GetInt32("mSchoolId");
        mLoginTime = info.GetString("mLoginTime");
        mRoleId = info.GetInt32("mRoleId");
        mRoleName = info.GetString("mRoleName");
        mReferralId = info.GetInt32("mReferralId");
        mRoleCode = info.GetString("mRoleCode");
        mIsApproved = info.GetString("mIsApproved");
        mAddressId = info.GetInt32("mAddressId");

    }
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {

        info.AddValue("mIsLogin", mIsLogin);
        info.AddValue("mSessionID", mSessionID);
        info.AddValue("mUserName", mUserName);
        info.AddValue("mGender", mGender);
        info.AddValue("mLoginId", mLoginId);
        info.AddValue("mSchoolId", mSchoolId);
        info.AddValue("mLoginTime", mLoginTime);
        info.AddValue("mRoleId", mRoleId);
        info.AddValue("mRoleName", mRoleName);
        info.AddValue("mReferralId", mReferralId);
        info.AddValue("mRoleCode", mRoleCode);
        info.AddValue("mIsApproved", mIsApproved);
        info.AddValue("mAddressId", mAddressId);
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            throw new ArgumentNullException("info");

        GetObjectData(info, context);
    }



    private bool mIsLogin = false;
    public bool Is = true;


    public bool IsLogin
    {
        get
        {
            return mIsLogin;
        }
        set
        {
            mIsLogin = value;
        }
    }

    private string mSessionID = "";

    public string SessionID
    {
        get
        {
            return mSessionID;
        }
        set
        {
            mSessionID = value;
        }
    }



    private string mUserName = "";

    public string UserName
    {
        get
        {
            return mUserName;
        }
        set
        {
            mUserName = value;
        }
    }

    private string mGender = "";

    public string Gender
    {
        get
        {
            return mGender;
        }
        set
        {
            mGender = value;
        }
    }

    private int mLoginId = 0;

    public int LoginId
    {
        get
        {
            return mLoginId;
        }
        set
        {
            mLoginId = value;
        }
    }

    private int mSchoolId = 0;

    public int SchoolId
    {
        get
        {
            return mSchoolId;
        }
        set
        {
            mSchoolId = value;
        }
    }

    private int mReferralId = 0;

    public int ReferralId
    {
        get
        {
            return mReferralId;
        }
        set
        {
            mReferralId = value;
        }
    }


    private int mCurrentProcessId = 0;

    public int CurrentProcessId
    {
        get
        {
            return mCurrentProcessId;
        }
        set
        {
            mCurrentProcessId = value;
        }
    }

    private string mLoginTime = "";

    public string LoginTime
    {
        get
        {
            return mLoginTime;
        }
        set
        {
            mLoginTime = value;
        }
    }

    private int mRoleId = 0;

    public int RoleId
    {
        get
        {
            return mRoleId;
        }
        set
        {
            mRoleId = value;
        }
    }


    private string mRoleName = "";

    public string RoleName
    {
        get
        {
            return mRoleName;
        }
        set
        {
            mRoleName = value;
        }
    }

    private string mRoleCode = "";
    public string RoleCode
    {
        get
        {
            return mRoleCode;
        }
        set
        {
            mRoleCode = value;
        }
    }

    private string mIsApproved = "";
    public string IsApproved
    {
        get
        {
            return mIsApproved;
        }
        set
        {
            mIsApproved = value;
        }
    }

    private int mAddressId = 0;
    public int AddressId
    {
        get
        {
            return mAddressId;
        }
        set
        {
            mAddressId = value;
        }
    }

    

    public int SessTab1 { get; set; }
    public int SessTab2 { get; set; }
    public int SessTab3 { get; set; }
    public int SessTab4 { get; set; }
    public int SessTab5 { get; set; }
    public int SessTab6 { get; set; }
    public int SessTab7 { get; set; }
    public int SessTab8 { get; set; }

}