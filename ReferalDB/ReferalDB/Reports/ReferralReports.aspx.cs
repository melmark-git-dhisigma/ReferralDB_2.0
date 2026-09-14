using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Drawing.Printing;
using System.IO;
using BuisinessLayer;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.IO.Compression;
using System.Text;
using System.Drawing;
using NPOI.XSSF.UserModel;  
using NPOI.SS.UserModel;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Web.UI.HtmlControls;
using NPOI.SS.Formula.Functions;
using DataLayer;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.VariantTypes;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Org.BouncyCastle.Utilities.Encoders;
using NPOI.SS.Util;
namespace ReferalDB.Reports
{
    public partial class ReferralReports : System.Web.UI.Page
    {
        public clsSession sess = null;
        System.Data.DataTable alldata;
        protected void Page_Load(object sender, EventArgs e)
        {
            Btnexport.Visible = false;
            Btnexport1.Visible = false;
            Btnexport3.Visible = false;
            btnexporttr.Visible = false;
            btnexportqtr.Visible = false;
            btnexportloc.Visible = false;

            contactdrop.Visible = false;
            ddlReferrals.Visible = false;
            contactshow.Visible = false;
            nodata.Visible = false;
            nodata.Text = "";
            contactdrop.Visible = false;
            if (!IsPostBack)
            {
               

                RVReferralReport.Visible = false; 
            }
        }

        protected void LoadState()
        {
            System.Data.DataTable Dt;
            SqlCommand cmd = null;
            SqlDataAdapter DAdap = null;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString();
            con.Open();
            Page.Title = "Something";
            using (cmd = new SqlCommand("SELECT LookupId,LookupName FROM LookUp WHERE LookupType='State'", con))
            {
                // if (blnTrans) cmd.Transaction = Trans;
                using (DAdap = new SqlDataAdapter(cmd))
                {
                    Dt = new System.Data.DataTable();
                    DAdap.Fill(Dt);
                }
            }
            ddlState.DataSource = Dt;
            ddlState.DataTextField = "LookupName";
            ddlState.DataValueField = "LookupId";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("---------------Select--------------", "0"));
            ddlState.SelectedValue = "0";
        }

        [Serializable]
        public class CustomReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
        {

            // local variable for network credential.
            private string _UserName;
            private string _PassWord;
            private string _DomainName;

            public CustomReportCredentials(string UserName, string PassWord, string DomainName)
            {
                _UserName = UserName;
                _PassWord = PassWord;
                _DomainName = DomainName;
            }

            public System.Security.Principal.WindowsIdentity ImpersonationUser
            {
                get
                {
                    return null;  // not use ImpersonationUser
                }
            }
            public ICredentials NetworkCredentials
            {
                get
                {
                    // use NetworkCredentials
                    return new NetworkCredential(_UserName, _PassWord, _DomainName);
                }
            }
            public bool GetFormsCredentials(out Cookie authCookie, out string user,
                out string password, out string authority)
            {

                // not use FormsCredentials unless you have implements a custom autentication.
                authCookie = null;
                user = password = authority = null;
                return false;
            }
        }



        protected void LbtnAllReferral_Click(object sender, EventArgs e)
        {
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            nodata.Visible = false;
            nodata.Text = "";
            Btnexport.Visible = false;
            allgrid.Visible = false;
            contactdrop.Visible = false;
            ddlReferrals.Visible = false;
            contactshow.Visible = false;
            if (!highcheck.Checked == false)
            {
                allgrid.Visible = false;
                hdnMenu.Value = "AllReferral";
                RVReferralReport.SizeToReportContent = false;
                tdMsg.InnerHtml = "";
                HeadingDiv.Visible = true;
                divfunded.Visible = false;
                referralage.Visible = false;
                HeadingDiv.InnerHtml = "All Referrals";
                RVReferralReport.Visible = true;
                sess = (clsSession)Session["UserSession"];
                RVReferralReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVReferralReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReferralReport"];
                RVReferralReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[1];
                parm[0] = new ReportParameter("SchoolID", sess.SchoolId.ToString());
                this.RVReferralReport.ServerReport.SetParameters(parm);
                RVReferralReport.ServerReport.Refresh();
                divlocation.Visible = false;
                divbirthdate.Visible = false;
                Btnexport.Visible = false;

            }
            else
            {
                hdnMenu.Value = "AllReferral";
                tdMsg.InnerHtml = "";
                HeadingDiv.Visible = true;
                divfunded.Visible = false;
                referralage.Visible = false;
                HeadingDiv.InnerHtml = "All Referrals";
                RVReferralReport.Visible = false;
                sess = (clsSession)Session["UserSession"];
                divlocation.Visible = false;
                divbirthdate.Visible = false;
                allgrid.Visible = true;
                 alldata = GetData(sess.SchoolId.ToString());
                if (alldata != null && alldata.Rows.Count > 0)
                {
                 ViewState["alldata"] = DataTableToJson(alldata);
                allgrid.DataSource = alldata;
                allgrid.DataBind();
                    
                    Btnexport.Visible = true;
                }
                else
                {
                    allgrid.Visible = false;
                    reporttable.Visible = true;
                    reporttable.InnerHtml = "No data available";
                    Btnexport.Visible = false;
                    Btnexport1.Visible = false;
                    Btnexport3.Visible = false;
                    btnexporttr.Visible = false;
                    btnexportqtr.Visible = false;
                    btnexportloc.Visible = false;




                }
                string script2 = "hideoverlay();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "show2", script2, true);

            }

        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            allgrid.PageIndex = e.NewPageIndex;
            alldata = JsonToDataTable(ViewState["alldata"].ToString());
            allgrid.DataSource = alldata;
            allgrid.DataBind();
            allgrid.AllowPaging = true;
        }
        private System.Data.DataTable GetTrackData(string scoolid,string status)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
            string qry = "SELECT SD.SchoolId ,SD.[StudentPersonalId] ,SD.LastName+','+SD.FirstName AS studentPersonalName ,CASE WHEN [ImageUrl] IS NULL OR [ImageUrl]='' THEN CASE WHEN SD.Gender=1 THEN  (SELECT FormatImg FROM [dbo].[DefaultImage] WHERE Sex='M')"
                         + "ELSE  (SELECT FormatImg FROM [dbo].[DefaultImage] WHERE Sex='F')      END ELSE [ImageUrl] END AS [ImageUrl]     ,CASE WHEN SD.Gender=1 THEN 'Male' ELSE 'Female' END Gender ,CONVERT(VARCHAR(10), SD.[BirthDate], 101) AS [BirthDate]  ,CONVERT(VARCHAR(10),"
                         + "SD.[AdmissionDate], 101) AS [DateOfReferral] ,DATEDIFF(YEAR,SD.BirthDate,GETDATE())  - (CASE WHEN DATEADD(YY,DATEDIFF(YEAR,SD.BirthDate,GETDATE()),SD.BirthDate) >  GETDATE() THEN 1 ELSE 0 END) AS Age    ,SD.[PlaceOfBirth]   ,ADL.[City] AS [City] ,(SELECT LookupName FROM LookUp WHERE LookupType = 'State' AND LookupId = ADL.StateProvince) AS State ,CASE WHEN SD.InactiveList='True' THEN 'IL' ELSE 'AV' END AS QueueType"
                         + " FROM  [dbo].[StudentPersonal] SD   INNER JOIN StudentAddresRel SDR ON SDR.StudentPersonalId=SD.StudentPersonalId  INNER JOIN AddressList ADL ON ADL.AddressId=SDR.AddressId  WHERE StudentType='Referral'  ORDER BY SD.[AdmissionDate] DESC";
            SqlCommand cmd = new SqlCommand(qry, conn);
            cmd.CommandTimeout = 1200;
          try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                Dt.Columns.Add("Referral Name", typeof(string));
                Dt.Columns.Add("Gender", typeof(string));
                Dt.Columns.Add("Birth Date", typeof(string));
                Dt.Columns.Add("Age", typeof(string));
                Dt.Columns.Add("Date of Referral", typeof(string));
                Dt.Columns.Add("City", typeof(string));
                Dt.Columns.Add("State", typeof(string));
                if (dt != null && dt.Rows.Count > 0)
                {
                    var distinctRows = dt.AsEnumerable()
                                    .GroupBy(row => row["StudentPersonalId"])
                                    .Select(group => group.First())
                                    .CopyToDataTable();
                    dt = distinctRows;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["QueueType"].ToString() == status)
                        {
                            DataRow row = Dt.NewRow();
                            if (dt.Rows[i]["studentPersonalName"] != null)
                            {
                                row["Referral Name"] = dt.Rows[i]["studentPersonalName"].ToString(); ;
                            }
                            if (dt.Rows[i]["BirthDate"] != null)
                            {
                                row["Birth Date"] = dt.Rows[i]["BirthDate"].ToString();
                            }
                            if (dt.Rows[i]["Gender"] != null)
                            {
                                row["Gender"] = dt.Rows[i]["Gender"].ToString();
                            }
                            if (dt.Rows[i]["Age"] != null)
                            {
                                row["Age"] = dt.Rows[i]["Age"].ToString();
                            }
                            if (dt.Rows[i]["DateOfReferral"] != null)
                            {
                                row["Date of Referral"] = dt.Rows[i]["DateOfReferral"].ToString();
                            }
                            if (dt.Rows[i]["City"] != null)
                            {
                                row["City"] = dt.Rows[i]["City"].ToString();
                            }
                            if (dt.Rows[i]["State"] != null)
                            {
                                row["State"] = dt.Rows[i]["State"].ToString();
                            }
                            Dt.Rows.Add(row);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
           
          return Dt;
        }
        private System.Data.DataTable GetAgeData(string scoolid, string txtStartAge, string txtEndAge)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
            String proc = "[dbo].[ReferralReportProcedure]";
            SqlCommand cmd = new SqlCommand(proc, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SchoolId", Convert.ToInt32(scoolid));
            cmd.CommandTimeout = 1200;
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                Dt.Columns.Add("Referral Name", typeof(string));
                Dt.Columns.Add("Gender", typeof(string));
                Dt.Columns.Add("Birth Date", typeof(string));
                Dt.Columns.Add("Age", typeof(string));
                Dt.Columns.Add("Date of Referral", typeof(string));
                Dt.Columns.Add("City", typeof(string));
                Dt.Columns.Add("State", typeof(string));
                if (dt != null && dt.Rows.Count > 0)
                {
                    var distinctRows = dt.AsEnumerable()
                                    .GroupBy(row => row["StudentPersonalId"])
                                    .Select(group => group.First())
                                    .CopyToDataTable();
                    dt = distinctRows;
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(dt.Rows[i]["Age"]) >= Convert.ToInt32(txtStartAge) && Convert.ToInt32(dt.Rows[i]["Age"]) <= Convert.ToInt32(txtEndAge))
                        {
                            DataRow row = Dt.NewRow();
                            if (dt.Rows[i]["studentPersonalName"] != null)
                            {
                                row["Referral Name"] = dt.Rows[i]["studentPersonalName"].ToString(); ;
                            }
                            if (dt.Rows[i]["BirthDate"] != null)
                            {
                                row["Birth Date"] = dt.Rows[i]["BirthDate"].ToString();
                            }
                            if (dt.Rows[i]["Gender"] != null)
                            {
                                row["Gender"] = dt.Rows[i]["Gender"].ToString();
                            }
                            if (dt.Rows[i]["Age"] != null)
                            {
                                row["Age"] = dt.Rows[i]["Age"].ToString();
                            }
                            if (dt.Rows[i]["DateOfReferral"] != null)
                            {
                                row["Date of Referral"] = dt.Rows[i]["DateOfReferral"].ToString();
                            }
                            if (dt.Rows[i]["City"] != null)
                            {
                                row["City"] = dt.Rows[i]["City"].ToString();
                            }
                            if (dt.Rows[i]["State"] != null)
                            {
                                row["State"] = dt.Rows[i]["State"].ToString();
                            }
                            Dt.Rows.Add(row);
                        }
                    }
                }
               

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
          
                return Dt;
            }
        private System.Data.DataTable GetActiveAgeData(string scoolid, string txtStartAge, string txtEndAge, string schoolid, string status)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
            string qry = "SELECT SchoolId,SD.[StudentPersonalId],SD.LastName + ',' + SD.FirstName AS studentPersonalName, CASE WHEN[ImageUrl] IS NULL OR[ImageUrl] = '' THEN CASE WHEN SD.Gender = 1 THEN" +
     " (SELECT FormatImg FROM[dbo].[DefaultImage] WHERE Sex = 'M') ELSE(SELECT FormatImg FROM[dbo].[DefaultImage] WHERE Sex = 'F')  END ELSE[ImageUrl] END AS[ImageUrl]" +
            ", CASE WHEN SD.Gender = 1 THEN 'Male' ELSE 'Female' END Gender, CONVERT(VARCHAR(10), SD.[BirthDate], 101) AS[BirthDate]" +
     ",DATEDIFF(YEAR, SD.BirthDate, GETDATE()) - (CASE WHEN DATEADD(YY, DATEDIFF(YEAR, SD.BirthDate, GETDATE()),SD.BirthDate) > GETDATE() THEN 1 ELSE 0 END) AS Age" +
     ", SD.[PlaceOfBirth] ,[Height],[Weight],ADL.[City] AS[City] ,CONVERT(VARCHAR(10), SD.[AdmissionDate], 101) AS[DateOfReferral]" +
    " ,(SELECT LookupName FROM LookUp WHERE LookupType = 'State' AND LookupId = ADL.StateProvince) AS State, CASE WHEN InactiveList = 'True' THEN 'IL' ELSE 'AV' END AS QueueType" +
      " FROM [dbo].[StudentPersonal] SD INNER JOIN StudentAddresRel SDR ON SDR.StudentPersonalId = SD.StudentPersonalId INNER JOIN AddressList ADL ON ADL.AddressId = SDR.AddressId" +
      " WHERE StudentType = 'Referral' ORDER BY SD.AdmissionDate DESC";
            SqlCommand cmd = new SqlCommand(qry, conn);
            cmd.CommandTimeout = 1200;
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                Dt.Columns.Add("Referral Name", typeof(string));
                Dt.Columns.Add("Gender", typeof(string));
                Dt.Columns.Add("Birth Date", typeof(string));
                Dt.Columns.Add("Age", typeof(string));
                Dt.Columns.Add("Date of Referral", typeof(string));
                Dt.Columns.Add("City", typeof(string));
                Dt.Columns.Add("State", typeof(string));
                if (dt != null && dt.Rows.Count > 0)
                {
                    var distinctRows = dt.AsEnumerable()
                                    .GroupBy(row => row["StudentPersonalId"])
                                    .Select(group => group.First())
                                    .CopyToDataTable();
                    dt = distinctRows;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(dt.Rows[i]["Age"]) >= Convert.ToInt32(txtStartAge) && Convert.ToInt32(dt.Rows[i]["Age"]) <= Convert.ToInt32(txtEndAge) && dt.Rows[i]["QueueType"].ToString() == status && dt.Rows[i]["SchoolId"].ToString() == schoolid)
                        {
                            DataRow row = Dt.NewRow();
                            if (dt.Rows[i]["studentPersonalName"] != null)
                            {
                                row["Referral Name"] = dt.Rows[i]["studentPersonalName"].ToString(); ;
                            }
                            if (dt.Rows[i]["BirthDate"] != null)
                            {
                                row["Birth Date"] = dt.Rows[i]["BirthDate"].ToString();
                            }
                            if (dt.Rows[i]["Gender"] != null)
                            {
                                row["Gender"] = dt.Rows[i]["Gender"].ToString();
                            }
                            if (dt.Rows[i]["Age"] != null)
                            {
                                row["Age"] = dt.Rows[i]["Age"].ToString();
                            }
                            if (dt.Rows[i]["DateOfReferral"] != null)
                            {
                                row["Date of Referral"] = dt.Rows[i]["DateOfReferral"].ToString();
                            }
                            if (dt.Rows[i]["City"] != null)
                            {
                                row["City"] = dt.Rows[i]["City"].ToString();
                            }
                            if (dt.Rows[i]["State"] != null)
                            {
                                row["State"] = dt.Rows[i]["State"].ToString();
                            }
                            Dt.Rows.Add(row);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            
                return Dt;
           
            }
        private System.Data.DataTable GetData(string scoolid)
        {
            System.Data.DataTable Dt =new System.Data.DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
            SqlCommand cmd = new SqlCommand("ReferralReportProcedure", conn);
            cmd.CommandTimeout = 1200;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SchoolId", scoolid);
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                Dt.Columns.Add("Referral Name", typeof(string));
                Dt.Columns.Add("Birth Date", typeof(string));
                Dt.Columns.Add("Gender", typeof(string));
                Dt.Columns.Add("Age", typeof(string));
                Dt.Columns.Add("Date of Referral", typeof(string));
                Dt.Columns.Add("City", typeof(string));
                Dt.Columns.Add("State", typeof(string));
                if (dt != null && dt.Rows.Count > 0)
                {
                    var distinctRows = dt.AsEnumerable()
                                    .GroupBy(row => row["StudentPersonalId"])
                                    .Select(group => group.First())
                                    .CopyToDataTable();
                    dt = distinctRows;
                    for (int i = 0; i < dt.Rows.Count; i++)  
                    {
                        DataRow row = Dt.NewRow();
                        if (dt.Rows[i]["studentPersonalName"] != null)
                        {
                            row["Referral Name"] = dt.Rows[i]["studentPersonalName"].ToString(); ;  
                        }
                        if (dt.Rows[i]["BirthDate"] != null)
                        {
                            row["Birth Date"] = dt.Rows[i]["BirthDate"].ToString();
                        }
                        if (dt.Rows[i]["Gender"] != null)
                        {
                            row["Gender"] = dt.Rows[i]["Gender"].ToString();
                        }
                        if (dt.Rows[i]["Age"] != null)
                        {
                            row["Age"] = dt.Rows[i]["Age"].ToString();
                        }
                        if (dt.Rows[i]["DateOfReferral"] != null)
                        {
                            row["Date of Referral"] = dt.Rows[i]["DateOfReferral"].ToString();
                        }
                        if (dt.Rows[i]["City"] != null)
                        {
                            row["City"] = dt.Rows[i]["City"].ToString();
                        }
                        if (dt.Rows[i]["State"] != null)
                        {
                            row["State"] = dt.Rows[i]["State"].ToString();
                        }
                        Dt.Rows.Add(row);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
           return Dt;
        }
        private string DataTableToJson(System.Data.DataTable dt)
        {
            var rows = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                var rowDict = new Dictionary<string, object>();
                foreach (DataColumn column in dt.Columns)
                {
                    rowDict[column.ColumnName] = row[column];
                }
                rows.Add(rowDict);
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 2147483647;
            return CompressString(serializer.Serialize(rows));
        }

        private System.Data.DataTable JsonToDataTable(string jsonString)
        {
            jsonString = DecompressString(jsonString);
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 2147483647;
            var rows = serializer.Deserialize<List<Dictionary<string, object>>>(jsonString);
            System.Data.DataTable dt = new System.Data.DataTable();
            if (rows.Count > 0)
            {
                foreach (var column in rows[0].Keys)
                {
                    dt.Columns.Add(column);
                }

                foreach (var rowDict in rows)
                {
                    var row = dt.NewRow();
                    foreach (var column in rowDict.Keys)
                    {
                        row[column] = rowDict[column];
                    }
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        public  string CompressString(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var ms = new MemoryStream())
            {
                using (var gzip = new GZipStream(ms, CompressionMode.Compress))
                {
                    gzip.Write(bytes, 0, bytes.Length);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public  string DecompressString(string compressedStr)
        {
            var bytes = Convert.FromBase64String(compressedStr);
            using (var ms = new MemoryStream(bytes))
            using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
            using (var reader = new StreamReader(gzip, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        protected void ClearAgeStatus()
        {
            txtEndAge.Text = "";
            txtStartAge.Text = "";
            ddlStatus.SelectedValue= "0";
            tdMsg.InnerHtml = "";
        }

        protected void LbtnRefTrackActive_Click(object sender, EventArgs e)
        {
            contactdrop.Visible = false;
            ddlReferrals.Visible = false;
            contactshow.Visible = false;
                reporttable.Visible = false;
            reporttable.InnerHtml = "";
            nodata.Visible = false;
            nodata.Text = "";
            Btnexport.Visible = false;
            Btnexport1.Visible = false;
            Btnexport3.Visible = false;
            btnexporttr.Visible = false;
            btnexportqtr.Visible = false;
            btnexportloc.Visible = false;

            allgrid.Visible = false;
                hdnMenu.Value = "RefTrackActive";
                RVReferralReport.SizeToReportContent = false;
                ClearAgeStatus();
                HeadingDiv.Visible = true;
                HeadingDiv.InnerHtml = "All Referrals Tracking Active";
                referralage.Visible = true;
                hdnType.Value = "Active";
                lblageStart.Visible = false;
                txtStartAge.Visible = false;
                lblageend.Visible = false;
                txtEndAge.Visible = false;
                lblStatus.Visible = true;
                ddlStatus.Visible = true;
                divfunded.Visible = false;
                divlocation.Visible = false;
                divbirthdate.Visible = false;
                RVReferralReport.Visible = false;
            
            
        }

        protected void LbtnRefAgeRange_Click(object sender, EventArgs e)
        {
            contactdrop.Visible = false;
            ddlReferrals.Visible = false;
            contactshow.Visible = false;
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            nodata.Visible = false;
            nodata.Text = "";
            Btnexport.Visible = false;
            Btnexport1.Visible = false;
            Btnexport3.Visible = false;
            btnexporttr.Visible = false;
            btnexportqtr.Visible = false;
            btnexportloc.Visible = false;

            allgrid.Visible = false;
            hdnMenu.Value = "RefAgeRange";
            RVReferralReport.SizeToReportContent = false;
            ClearAgeStatus();
            HeadingDiv.Visible = true;
            HeadingDiv.InnerHtml = "All Referrals by Age Range";
            referralage.Visible = true;
            hdnType.Value = "Age";
            lblStatus.Visible = false;
            ddlStatus.Visible = false;
            lblageStart.Visible = true;
            txtStartAge.Visible = true;
            lblageend.Visible = true;
            txtEndAge.Visible = true;
            divfunded.Visible = false;
            divlocation.Visible = false;
            divbirthdate.Visible = false;
            RVReferralReport.Visible = false;
        }

        protected void LbtnTackingActiveAge_Click(object sender, EventArgs e)
        {
            contactdrop.Visible = false;
            ddlReferrals.Visible = false;
            contactshow.Visible = false;
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            nodata.Visible = false;
            nodata.Text = "";
            Btnexport.Visible = false;
            Btnexport1.Visible = false;
            Btnexport3.Visible = false;
            btnexporttr.Visible = false;
            btnexportqtr.Visible = false;
            btnexportloc.Visible = false;

            allgrid.Visible = false;
            hdnMenu.Value = "TackingActiveAge";
            RVReferralReport.SizeToReportContent = false;
            ClearAgeStatus();
            HeadingDiv.Visible = true;
            HeadingDiv.InnerHtml = "All Referrals Tracking Active by Age Range";
            referralage.Visible = true;
            hdnType.Value = "ActiveAge";
            lblStatus.Visible = true;
            ddlStatus.Visible = true;
            lblageStart.Visible = true;
            txtStartAge.Visible = true;
            lblageend.Visible = true;
            txtEndAge.Visible = true;
            divfunded.Visible = false;
            divlocation.Visible = false;
            divbirthdate.Visible = false;
            RVReferralReport.Visible = false;
        }

        protected void LbtnRefContact_Click(object sender, EventArgs e)
        {
            contactdrop.Visible = false;
            ddlReferrals.Visible = false;
            contactshow.Visible = false;
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            nodata.Visible = false;
            nodata.Text = "";
            Btnexport.Visible = false;
            Btnexport1.Visible = false;
            Btnexport3.Visible = false;
            btnexporttr.Visible = false;
            btnexportqtr.Visible = false;
            btnexportloc.Visible = false;

            allgrid.Visible = false;
            hdnMenu.Value = "RefContact";
            RVReferralReport.SizeToReportContent = true;
            tdMsg.InnerHtml = "";
            RVReferralReport.Visible = false;
            HeadingDiv.Visible = true;
            divlocation.Visible = false;
            divbirthdate.Visible = false;
            divfunded.Visible = false;
            HeadingDiv.InnerHtml = "Client/Contact/Vendor";
            referralage.Visible = false;
            if (!highcheck.Checked == false)
            {
            RVReferralReport.Visible = true;
            sess = (clsSession)Session["UserSession"];
            RVReferralReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
            RVReferralReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReferralReportContact"];
            RVReferralReport.ShowParameterPrompts = false;
            ReportParameter[] parm = new ReportParameter[1];
            parm[0] = new ReportParameter("SchoolID", sess.SchoolId.ToString());
            this.RVReferralReport.ServerReport.SetParameters(parm);
            RVReferralReport.ServerReport.Refresh();
            divlocation.Visible = false;
            divbirthdate.Visible = false;
        }
            else
            {
                
                contactdrop.Visible = true;
                ddlReferrals.Visible = true;
                contactshow.Visible = true;
                LoadReferrals();
                sess = (clsSession)Session["UserSession"];
                System.Data.DataTable dt = Getallcontact(sess.SchoolId.ToString(),"0");

                if (dt != null && dt.Rows.Count > 0)
                {
                    ViewState["alldata"] = DataTableToJson(dt);
                    string htmlTable = GenerateHtmlTablecont(dt);
                    string script4 = "showoverlaycont();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "show6", script4, true);
                    reporttable.Visible = true;
                    reporttable.InnerHtml = htmlTable;

                    string script3 = "Applypagination2();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "show5", script3, true);
                    
                    Btnexport1.Visible = true;
                }
                else
                {
                    reporttable.Visible = true;
                    reporttable.InnerHtml = "No data available";
                    Btnexport.Visible = false;
                    Btnexport1.Visible = false;
                    Btnexport3.Visible = false;
                    btnexporttr.Visible = false;
                    btnexportqtr.Visible = false;
                    btnexportloc.Visible = false;


                }
                string script2 = "hideoverlay();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "show7", script2, true);
            }
        }
        private void LoadReferrals()
        {
            sess = (clsSession)Session["UserSession"];
            string connStr = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "select StudentPersonalId AS id, lastname +' '+firstname AS Name  from StudentPersonal where SchoolId="+sess.SchoolId +" and StudentType='Referral'";  
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                ddlReferrals.DataSource = reader;
                ddlReferrals.DataTextField = "Name";
                ddlReferrals.DataValueField = "Id";
                ddlReferrals.DataBind();
                conn.Close();
            }
        }

        protected void LbtnRefFunded_Click(object sender, EventArgs e)
        {
            contactdrop.Visible = false;
            ddlReferrals.Visible = false;
            contactshow.Visible = false;
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            nodata.Visible = false;
            nodata.Text = "";
            Btnexport.Visible = false;
            Btnexport1.Visible = false;
            Btnexport3.Visible = false;
            btnexporttr.Visible = false;
            btnexportqtr.Visible = false;
            btnexportloc.Visible = false;

            allgrid.Visible = false;
            hdnMenu.Value = "RefFunded";
            RVReferralReport.SizeToReportContent = false;
            ddlFundingStatus.SelectedValue = "0";
            tdMsg.InnerHtml = "";
            HeadingDiv.Visible = true;
            divfunded.Visible = true;
            HeadingDiv.InnerHtml = "All Referrals by Funded vs. Not Funded";
            referralage.Visible = false;
            divlocation.Visible = false;
            divbirthdate.Visible = false;
            RVReferralReport.Visible = false;
        }

        protected void LbtnRefLocation_Click(object sender, EventArgs e)
        {
            contactdrop.Visible = false;
            ddlReferrals.Visible = false;
            contactshow.Visible = false;
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            nodata.Visible = false;
            nodata.Text = "";
            Btnexport.Visible = false;
            Btnexport1.Visible = false;
            Btnexport3.Visible = false;
            btnexporttr.Visible = false;
            btnexportqtr.Visible = false;
            btnexportloc.Visible = false;

            allgrid.Visible = false;
            hdnMenu.Value = "RefLocation";
            ddlState.DataSource = null;
            RVReferralReport.SizeToReportContent = false;
            txtcity.Text = "";
            tdMsg.InnerHtml = "";
            RVReferralReport.Visible = false;
            HeadingDiv.Visible = true;
            HeadingDiv.InnerHtml = "All Referrals by Location";
            divfunded.Visible = false;
            referralage.Visible = false;
            divlocation.Visible = true;
            divbirthdate.Visible = false;
            LoadState();
            if (!highcheck.Checked == true)
            {
                sess = (clsSession)Session["UserSession"];
                alldata = GetLocationData(sess.SchoolId.ToString(), txtcity.Text, ddlState.SelectedItem.Value);
                if (alldata != null && alldata.Rows.Count > 0)
                {
                    ViewState["alldata"] = DataTableToJson(alldata);
                    string htmlTable = GenerateHtmlTable(alldata);
                    reporttable.Visible = true;
                    reporttable.InnerHtml = htmlTable;
                    string script3 = "Applypagination();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "show9", script3, true);
                    Btnexport.Visible = false;
                    btnexportloc.Visible = true;
        }
                else
                {

                    nodata.Visible = true;
                    nodata.Text = "No data available";
                    Btnexport.Visible = false;
                    Btnexport1.Visible = false;
                    Btnexport3.Visible = false;
                    btnexporttr.Visible = false;
                    btnexportqtr.Visible = false;
                    btnexportloc.Visible = false;


                }
                string script2 = "hideoverlay();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "show10", script2, true);
            }
            else
            {
                RVReferralReport.Visible = true;
                sess = (clsSession)Session["UserSession"];
                RVReferralReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVReferralReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReferralReportLocation"];
                RVReferralReport.ShowParameterPrompts = false;
                ddlState.SelectedIndex = 0;
                txtcity.Text = "";
                ReportParameter[] parm = new ReportParameter[3];
                parm[0] = new ReportParameter("SchoolID", sess.SchoolId.ToString());
                parm[1] = new ReportParameter("State", ddlState.SelectedItem.Value);
                parm[2] = new ReportParameter("City", txtcity.Text);
                this.RVReferralReport.ServerReport.SetParameters(parm);
                RVReferralReport.ServerReport.Refresh();
            }
        }

        protected void LbtnRefBirthdateQuarter_Click(object sender, EventArgs e)
        {
            contactdrop.Visible = false;
            ddlReferrals.Visible = false;
            contactshow.Visible = false;
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            nodata.Visible = false;
            nodata.Text = "";
            Btnexport.Visible = false;
            Btnexport1.Visible = false;
            Btnexport3.Visible = false;
            btnexporttr.Visible = false;
            btnexportqtr.Visible = false;
            btnexportloc.Visible = false;
            allgrid.Visible = false;
            hdnMenu.Value = "RefBirthdateQuarter";
            RVReferralReport.SizeToReportContent = false;
            ddlQuarter.SelectedValue = "0";
            tdMsg.InnerHtml = "";
            HeadingDiv.Visible = true;
            HeadingDiv.InnerHtml = "All Referrals by Birthdate Quarter";
            divbirthdate.Visible = true;
            divfunded.Visible = false;
            referralage.Visible = false;
            divlocation.Visible = false;
            RVReferralReport.Visible = false;
            if (!highcheck.Checked == true)
            {
                tdMsg.InnerHtml = "";
                RVReferralReport.Visible = false;
                sess = (clsSession)Session["UserSession"];

                alldata = GetQuarterData(sess.SchoolId.ToString(), ddlQuarter.SelectedItem.Value);
                if (alldata != null && alldata.Rows.Count > 0)
                {
                    ViewState["alldata"] = DataTableToJson(alldata);
                    string htmlTable = GenerateHtmlTable(alldata);
                    RVReferralReport.Visible = false;
                    reporttable.Visible = true;
                    reporttable.InnerHtml = htmlTable;
                    string script3 = "Applypagination();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "show11", script3, true);
                    Btnexport.Visible = false;
                    btnexportqtr.Visible = true;
        }
                else
                {
                    reporttable.Visible = true;
                    reporttable.InnerHtml = "No data available";
                    Btnexport.Visible = false;
                    Btnexport1.Visible = false;
                    Btnexport3.Visible = false;
                    btnexporttr.Visible = false;
                    btnexportqtr.Visible = false;
                    btnexportloc.Visible = false;



                }
                string script2 = "hideoverlay();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "show12", script2, true);
            }
            else
            {
                tdMsg.InnerHtml = "";
                RVReferralReport.Visible = true;
                sess = (clsSession)Session["UserSession"];
                RVReferralReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVReferralReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReferralReportQuarter"];
                RVReferralReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[2];
                parm[0] = new ReportParameter("SchoolID", sess.SchoolId.ToString());

                List<string> allQuarters = new List<string>();

                foreach (ListItem item in ddlQuarter.Items)
                {
                    if (item.Value != "0")
                    {
                        allQuarters.Add(item.Value);
                    }
                }

                parm[1] = new ReportParameter("Quarter", allQuarters.ToArray());
                //parm[1] = new ReportParameter("Quarter", ddlQuarter.SelectedItem.Value);
                this.RVReferralReport.ServerReport.SetParameters(parm);
                RVReferralReport.ServerReport.Refresh();
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            RVReferralReport.Visible = false;
            sess = (clsSession)Session["UserSession"];
            RVReferralReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
            if (hdnType.Value == "Active")
            {
                if (ddlStatus.SelectedItem.Value != "0")
                {
                    if (!highcheck.Checked == false)
                    {
                        RVReferralReport.Visible = true;
                        tdMsg.InnerHtml = "";
                        RVReferralReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReferralReportStatus"];
                        RVReferralReport.ShowParameterPrompts = false;
                        ReportParameter[] parm = new ReportParameter[2];
                        parm[0] = new ReportParameter("SchoolID", sess.SchoolId.ToString());
                        parm[1] = new ReportParameter("Status", ddlStatus.SelectedItem.Value);
                        this.RVReferralReport.ServerReport.SetParameters(parm);
                    }
                    else
                    {

                        RVReferralReport.Visible = true;
                        tdMsg.InnerHtml = "";
                        alldata = GetTrackData(sess.SchoolId.ToString(), ddlStatus.SelectedItem.Value);
                        if (alldata != null && alldata.Rows.Count > 0)
                        {
                            ViewState["alldata"] = DataTableToJson(alldata);
                            string htmlTable = GenerateHtmlTable(alldata);
                            reporttable.Visible = true;
                            reporttable.InnerHtml = htmlTable;
                            string script3 = "Applypagination();";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "show3", script3, true);
                            Btnexport.Visible = false;
                            btnexporttr.Visible = true;
                           
                        }
                        else
                        {
                            reporttable.Visible = true;
                            reporttable.InnerHtml = "No data available";
                            Btnexport.Visible = false;
                            Btnexport1.Visible = false;
                            Btnexport3.Visible = false;
                            btnexporttr.Visible = false;
                            btnexportqtr.Visible = false;
                            btnexportloc.Visible = false;


                        }
                        string script2 = "hideoverlay();";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "show4", script2, true);


                    }
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Status...");
                    ddlStatus.Focus();
                }
            }
            if (hdnType.Value == "Age")
            {
                if (txtStartAge.Text != "" && txtEndAge.Text != "")
                {
                    if (!highcheck.Checked == false)
                    {
                    RVReferralReport.Visible = true;
                    tdMsg.InnerHtml = "";
                    RVReferralReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReferralReportAge"];
                    RVReferralReport.ShowParameterPrompts = false;
                    ReportParameter[] parm = new ReportParameter[3];
                    parm[0] = new ReportParameter("SchoolID", sess.SchoolId.ToString());
                    parm[1] = new ReportParameter("AgeStart", txtStartAge.Text);
                    parm[2] = new ReportParameter("AgeEnd", txtEndAge.Text);
                    this.RVReferralReport.ServerReport.SetParameters(parm);
                }
                    else
                    {
                        RVReferralReport.Visible = false;
                        tdMsg.InnerHtml = "";
                        alldata = GetAgeData(sess.SchoolId.ToString(), txtStartAge.Text, txtEndAge.Text);
                        if (alldata != null && alldata.Rows.Count > 0)
                        {
                            ViewState["alldata"] = DataTableToJson(alldata);
                            string htmlTable = GenerateHtmlTable(alldata);
                            reporttable.Visible = true;
                            reporttable.InnerHtml = htmlTable;
                            string script3 = "Applypagination();";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "show5", script3, true);
                            Btnexport.Visible = false;
                            btnexporttr.Visible = true;

                        }
                        else
                        {
                            reporttable.Visible = true;
                            reporttable.InnerHtml = "No data available";
                            Btnexport.Visible = false;
                            Btnexport1.Visible = false;
                            Btnexport3.Visible = false;
                            btnexporttr.Visible = false;
                            btnexportqtr.Visible = false;
                            btnexportloc.Visible = false;



                        }
                        string script2 = "hideoverlay();";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "show6", script2, true);
                    }
                }
                else if (txtStartAge.Text == "")
                {
                    tdMsg.InnerHtml=clsGeneral.warningMsg("Please enter starting age");
                    txtStartAge.Focus();
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter ending age");
                    txtEndAge.Focus();
                }
            }
            if (hdnType.Value == "ActiveAge")
            {
                if (txtStartAge.Text != "" && txtEndAge.Text != "" && ddlStatus.SelectedItem.Value!="0")
                {
                    if (!highcheck.Checked == false)
                    {
                    RVReferralReport.Visible = true;
                    tdMsg.InnerHtml = "";
                    RVReferralReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReferralReportAgeStatus"];
                    RVReferralReport.ShowParameterPrompts = false;
                    ReportParameter[] parm = new ReportParameter[4];
                    parm[0] = new ReportParameter("SchoolID", sess.SchoolId.ToString());
                    parm[1] = new ReportParameter("Status", ddlStatus.SelectedItem.Value);
                    parm[2] = new ReportParameter("AgeStart", txtStartAge.Text);
                    parm[3] = new ReportParameter("AgeEnd", txtEndAge.Text);
                    this.RVReferralReport.ServerReport.SetParameters(parm);
                }
                    else
                    {
                        RVReferralReport.Visible = false;
                        tdMsg.InnerHtml = "";
                        alldata = GetActiveAgeData(sess.SchoolId.ToString(), txtStartAge.Text, txtEndAge.Text, sess.SchoolId.ToString(), ddlStatus.SelectedItem.Value);
                        if (alldata != null && alldata.Rows.Count > 0)
                        {
                            ViewState["alldata"] = DataTableToJson(alldata);
                            string htmlTable = GenerateHtmlTable(alldata);
                            reporttable.Visible = true;
                            reporttable.InnerHtml = htmlTable;
                            string script3 = "Applypagination();";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "show7", script3, true);
                            Btnexport.Visible = false;
                            btnexporttr.Visible = true;

                        }
                        else
                        {
                            reporttable.Visible = true;
                            reporttable.InnerHtml = "No data available";
                            Btnexport.Visible = false;
                            Btnexport1.Visible = false;
                            Btnexport3.Visible = false;
                            btnexporttr.Visible = false;
                            btnexportqtr.Visible = false;
                            btnexportloc.Visible = false;



                        }
                        string script2 = "hideoverlay();";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "show8", script2, true);
                    }
                }
                else if (ddlStatus.SelectedItem.Value == "0")
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Status...");
                    ddlStatus.Focus();
                }
                else if (txtStartAge.Text == "")
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter starting age");
                    txtStartAge.Focus();
                }
                else if (txtEndAge.Text == "")
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter ending age");
                    txtEndAge.Focus();
                }
                else if (Convert.ToInt32(txtStartAge.Text) > Convert.ToInt32(txtEndAge.Text))
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Age condition is not valid");
                    txtStartAge.Focus();
                }
            }

            RVReferralReport.ServerReport.Refresh();
        }
        private string GenerateHtmlTable(System.Data.DataTable dataTable)
        {
          

            StringBuilder sb = new StringBuilder();

            sb.Append("<table id='trackingactive' class='display' border='1' style='width: 80%; border-collapse: collapse; text-align: center; vertical-align: middle;'>");

            sb.Append("<thead>");
            sb.Append("<tr>");
            foreach (DataColumn column in dataTable.Columns)
            {
                sb.AppendFormat("<th style='background-color: #111184; color: white; padding: 8px; text-align: center;'>{0}</th>", column.ColumnName);
            }
            sb.Append("</tr>");
            sb.Append("</thead>");

            sb.Append("<tbody>");
            int rowIndex = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                string rowStyle = (rowIndex % 2 == 0)
                    ? "background-color: white;"
                    : "background-color: rgba(0, 0, 0, 0.08);";

                sb.AppendFormat("<tr style='{0}'>", rowStyle);

                foreach (DataColumn column in dataTable.Columns)
                {
                    sb.AppendFormat("<td style='padding: 8px; text-align: center;'>{0}</td>", row[column]);
                }
                sb.Append("</tr>");
                rowIndex++;
            }
            sb.Append("</tbody>");

            sb.Append("</table>");

            return sb.ToString();

        }

        protected void btnshowgraph_Click(object sender, EventArgs e)
        {
            RVReferralReport.Visible = false;
            if(ddlFundingStatus.SelectedItem.Value!="0")
            {
                if (!highcheck.Checked == false)
                {
            tdMsg.InnerHtml = "";
            RVReferralReport.Visible = true;
            sess = (clsSession)Session["UserSession"];
            RVReferralReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
            RVReferralReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReferralReportFund"];
            RVReferralReport.ShowParameterPrompts = false;
            ReportParameter[] parm = new ReportParameter[2];
            parm[0] = new ReportParameter("Schoolid", sess.SchoolId.ToString());
            parm[1] = new ReportParameter("Fund", ddlFundingStatus.SelectedItem.Value);
            this.RVReferralReport.ServerReport.SetParameters(parm);
            RVReferralReport.ServerReport.Refresh();
            }
            else
            {
                    reporttable.Visible = false;
                    reporttable.InnerHtml = "";
                    nodata.Visible = false;
                    nodata.Text = "";
                    RVReferralReport.Visible = false;
                    sess = (clsSession)Session["UserSession"];
                    System.Data.DataTable dt = Getfunddata(sess.SchoolId.ToString(), ddlFundingStatus.SelectedItem.Value);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                    string htmlTable = GenerateHtmlTablefund(dt, ddlFundingStatus.SelectedItem.Value);
                    reporttable.Visible = true;
                    reporttable.InnerHtml = htmlTable;
                    string script3 = "Applypagination();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "show11", script3, true);
                    Btnexport.Visible = false;
                        Btnexport1.Visible = false;
                    Btnexport3.Visible = true;
                        btnexporttr.Visible = false;
                    }
                    else
                    {
                        reporttable.Visible = true;
                        reporttable.InnerHtml = "No data available";
                        Btnexport.Visible = false;
                        Btnexport1.Visible = false;
                        Btnexport3.Visible = false;
                        btnexporttr.Visible = false;

                        btnexportqtr.Visible = false;
                        btnexportloc.Visible = false;



                    }
                    string script2 = "hideoverlay();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "show12", script2, true);
                }
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Funding status");
                ddlFundingStatus.Focus();
            }

        }

        protected void btnlocation_Click(object sender, EventArgs e)
        {
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            nodata.Visible = false;
            nodata.Text = "";
            RVReferralReport.Visible = false;
            //if (ddlState.SelectedItem.Value != "0")
            //{
                tdMsg.InnerHtml = "";
                if (!highcheck.Checked == false)
                {
                RVReferralReport.Visible = true;
                sess = (clsSession)Session["UserSession"];
                RVReferralReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVReferralReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReferralReportLocation"];
                RVReferralReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[3];
                parm[0] = new ReportParameter("SchoolID", sess.SchoolId.ToString());
                parm[1] = new ReportParameter("State", ddlState.SelectedItem.Value);
                parm[2] = new ReportParameter("City", txtcity.Text);
                this.RVReferralReport.ServerReport.SetParameters(parm);
                RVReferralReport.ServerReport.Refresh();
            }
                else
                {
                    sess = (clsSession)Session["UserSession"];
                    alldata = GetLocationData(sess.SchoolId.ToString(), txtcity.Text, ddlState.SelectedItem.Value);
                    if (alldata != null && alldata.Rows.Count > 0)
                    {
                        ViewState["alldata"] = DataTableToJson(alldata);
                        string htmlTable = GenerateHtmlTable(alldata);
                        reporttable.Visible = true;
                        reporttable.InnerHtml = htmlTable;
                        string script3 = "Applypagination();";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "show9", script3, true);
                        Btnexport.Visible = false;
                    btnexportloc.Visible = true;
                }
                else
                    {
                        
                        nodata.Visible = true;
                        nodata.Text = "No data available";
                        Btnexport.Visible = false;
                        Btnexport1.Visible = false;
                        Btnexport3.Visible = false;
                    btnexporttr.Visible = false;
                    btnexportqtr.Visible = false;
                    btnexportloc.Visible = false;



                }
                string script2 = "hideoverlay();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "show10", script2, true);
                }
            //}
            //else if (ddlState.SelectedItem.Value == "0")
            //else
            //{
            //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please select state");
            //    ddlState.Focus();
            //}
            //else
            //{
            //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter city");
            //    txtcity.Focus();
            //}
            }

        protected void btnquarter_Click(object sender, EventArgs e)
        {
            RVReferralReport.Visible = false;
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            nodata.Visible = false;
            nodata.Text = "";
            //if (ddlQuarter.SelectedItem.Value != "0")
            //{
                tdMsg.InnerHtml = "";
                 if (!highcheck.Checked == false)
                 {
                     tdMsg.InnerHtml = "";
                RVReferralReport.Visible = true;
                sess = (clsSession)Session["UserSession"];                
                RVReferralReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVReferralReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReferralReportQuarter"];
                RVReferralReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[2];
                parm[0] = new ReportParameter("SchoolID", sess.SchoolId.ToString());
                if (ddlQuarter.SelectedIndex > 0)
                {
                    parm[1] = new ReportParameter("Quarter", ddlQuarter.SelectedItem.Value);
                }
                else
                {
                    List<string> allQuarters = new List<string>();

                    foreach (ListItem item in ddlQuarter.Items)
                    {
                        if (item.Value != "0")
                        {
                            allQuarters.Add(item.Value);
                        }
                    }

                    parm[1] = new ReportParameter("Quarter", allQuarters.ToArray());
                }
                //parm[1] = new ReportParameter("Quarter", ddlQuarter.SelectedItem.Value);
                this.RVReferralReport.ServerReport.SetParameters(parm);
                RVReferralReport.ServerReport.Refresh();

            }
            else
            {
                     tdMsg.InnerHtml = "";
                     RVReferralReport.Visible = false;
                     sess = (clsSession)Session["UserSession"];

                     alldata = GetQuarterData(sess.SchoolId.ToString(), ddlQuarter.SelectedItem.Value);
                     if (alldata != null && alldata.Rows.Count > 0)
                     {
                         ViewState["alldata"] = DataTableToJson(alldata);
                         string htmlTable = GenerateHtmlTable(alldata);
                         reporttable.Visible = true;
                         reporttable.InnerHtml = htmlTable;
                         string script3 = "Applypagination();";
                         ScriptManager.RegisterStartupScript(this, this.GetType(), "show11", script3, true);
                         Btnexport.Visible = false;
                        btnexporttr.Visible = false;
                    btnexportqtr.Visible = true;
                     }
                     else
                     {
                         reporttable.Visible = true;
                         reporttable.InnerHtml = "No data available";
                         Btnexport.Visible = false;
                         Btnexport1.Visible = false;
                         Btnexport3.Visible = false;
                    btnexporttr.Visible = false;
                    btnexportqtr.Visible = false;
                    btnexportloc.Visible = false;



                }
                string script2 = "hideoverlay();";
                     ScriptManager.RegisterStartupScript(this, this.GetType(), "show12", script2, true);
                 }
            //}
            //else
            //{
            //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please select birthdate quarter");
            //    ddlQuarter.Focus();
            //}
            }
        private System.Data.DataTable GetQuarterData(string scoolid, string quart)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
            String proc = "[dbo].[ReferralReportProcedure]";
            SqlCommand cmd = new SqlCommand(proc, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SchoolId", Convert.ToInt32(scoolid));
            cmd.CommandTimeout = 1200;
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                Dt.Columns.Add("Referral Name", typeof(string));
                Dt.Columns.Add("Age", typeof(string));
                Dt.Columns.Add("Gender", typeof(string));
                Dt.Columns.Add("Date of Referral", typeof(string));
                Dt.Columns.Add("Birthdate", typeof(string));
                Dt.Columns.Add("City", typeof(string));
                Dt.Columns.Add("State", typeof(string));
                if (dt != null && dt.Rows.Count > 0)
                {
                    
                        var distinctRows = dt.AsEnumerable()
                                    .GroupBy(row => row["StudentPersonalId"])
                                    .Select(group => group.First())
                                    .CopyToDataTable();
                        dt= distinctRows;
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["mMonth"].ToString().Trim() == quart || quart=="0")
                        {
                            DataRow row = Dt.NewRow();
                            if (dt.Rows[i]["studentPersonalName"] != null)
                            {
                                row["Referral Name"] = dt.Rows[i]["studentPersonalName"].ToString(); ;
                            }
                            if (dt.Rows[i]["Age"] != null)
                            {
                                row["Age"] = dt.Rows[i]["Age"].ToString();
                            }
                            if (dt.Rows[i]["Gender"] != null)
                            {
                                row["Gender"] = dt.Rows[i]["Gender"].ToString();
                            }

                            if (dt.Rows[i]["DateOfReferral"] != null)
                            {
                                row["Date of Referral"] = dt.Rows[i]["DateOfReferral"].ToString();
                            }
                            if (dt.Rows[i]["BirthDate"] != null)
                            {
                                row["Birthdate"] = dt.Rows[i]["BirthDate"].ToString();
                            }
                            if (dt.Rows[i]["City"] != null)
                            {
                                row["City"] = dt.Rows[i]["City"].ToString().Trim();
                            }
                            if (dt.Rows[i]["State"] != null)
                            {
                                row["State"] = dt.Rows[i]["State"].ToString().Trim();
                            }
                            Dt.Rows.Add(row);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
          
                return Dt;
            }
        private System.Data.DataTable GetLocationData(string scoolid, string city, string state)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
            String proc = "[dbo].[ReferralReportProcedure]";
            SqlCommand cmd = new SqlCommand(proc, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SchoolId", Convert.ToInt32(scoolid));
            cmd.CommandTimeout = 1200;
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                Dt.Columns.Add("Referral Name", typeof(string));
                Dt.Columns.Add("Gender", typeof(string));
                Dt.Columns.Add("Birth Date", typeof(string));
                Dt.Columns.Add("Date of Referral", typeof(string));
                Dt.Columns.Add("City", typeof(string));
                Dt.Columns.Add("State", typeof(string));
                if (dt != null && dt.Rows.Count > 0)
                {
                    var distinctRows = dt.AsEnumerable()
                                    .GroupBy(row => row["StudentPersonalId"])
                                    .Select(group => group.First())
                                    .CopyToDataTable();
                    dt = distinctRows;

                    string citySearch = (city != null) ? city.Trim().ToLower() : "";
                    bool cityProvided = !string.IsNullOrWhiteSpace(citySearch);
                    bool stateProvided = state != "0";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string rowCity = (dt.Rows[i]["City"] != null && dt.Rows[i]["City"] != DBNull.Value)
                            ? dt.Rows[i]["City"].ToString().Trim().ToLower()
                            : "";

                        string rowState = (dt.Rows[i]["StateProvince"] != null && dt.Rows[i]["StateProvince"] != DBNull.Value)
                            ? dt.Rows[i]["StateProvince"].ToString().Trim()
                            : "";

                        bool cityMatch = !cityProvided || rowCity.Contains(citySearch);
                        bool stateMatch = !stateProvided || rowState == state;

                        if (cityMatch && stateMatch)
                        {
                            DataRow row = Dt.NewRow();

                            if (dt.Rows[i]["studentPersonalName"] != DBNull.Value)
                                row["Referral Name"] = dt.Rows[i]["studentPersonalName"].ToString();

                            if (dt.Rows[i]["BirthDate"] != DBNull.Value)
                                row["Birth Date"] = dt.Rows[i]["BirthDate"].ToString();

                            if (dt.Rows[i]["Gender"] != DBNull.Value)
                                row["Gender"] = dt.Rows[i]["Gender"].ToString();

                            if (dt.Rows[i]["DateOfReferral"] != DBNull.Value)
                                    row["Date of Referral"] = dt.Rows[i]["DateOfReferral"].ToString();

                            if (dt.Rows[i]["City"] != DBNull.Value)
                                    row["City"] = dt.Rows[i]["City"].ToString().Trim();

                            if (dt.Rows[i]["State"] != DBNull.Value)
                                    row["State"] = dt.Rows[i]["State"].ToString().Trim();

                                Dt.Rows.Add(row);
                            }
                        }


                    
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
          
                return Dt;
            }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            alldata = JsonToDataTable(ViewState["alldata"].ToString());
        sess = (clsSession)Session["UserSession"];
                 string Filename = "ReferralReport" + ".xlsx";
             Filename = Server.UrlEncode(Filename);
                        ExportToExcel(alldata, Filename, Response);
          
        }
        protected void btnexport2_Click(object sender, EventArgs e)
        {
            if (ViewState["data"] != null)
            {
                System.Data.DataTable dt = JsonToDataTable(ViewState["data"].ToString());

                ExportToExcelcontact(dt, "ContactEvent");
            }


        }
        private System.Data.DataTable BuildDataTable(System.Data.DataTable dt, List<string> distinctRelations)
    {
            System.Data.DataTable result = new System.Data.DataTable();
            result.Columns.Add("Referral Name");
            result.Columns.Add("Birth Date");
            result.Columns.Add("Date Of Referral");
            result.Columns.Add("Gender");
            result.Columns.Add("City");
            result.Columns.Add("State");

            Dictionary<string, string> relMap = new Dictionary<string, string>();
            foreach (string rel in distinctRelations)
            {
                string safe = chngColumnName(rel);
                relMap[rel] = safe;
                result.Columns.Add(safe + "cname");
                result.Columns.Add(safe + "occ");
                result.Columns.Add(safe + "plang");
            }

            var grouped = dt.AsEnumerable().GroupBy(row => row["StudentPersonalId"].ToString());

            foreach (var group in grouped)
            {
                var first = group.First();
                DataRow newRow = result.NewRow();
                newRow["Referral Name"] = first["ReferralName"];
                newRow["Birth Date"] = first["BirthDate"];
                newRow["Date Of Referral"] = first["DateOfReferral"];
                newRow["Gender"] = first["Gender"];
                newRow["City"] = first["City"];
                newRow["State"] = first["State"];

                foreach (var row in group)
                {
                    string rel = row["Relationship"].ToString();
                    if (relMap.ContainsKey(rel))
                    {
                        string safe = relMap[rel];
                        newRow[safe + "cname"] = row["ContactName"];
                        newRow[safe + "occ"] = row["Occupation"];
                        newRow[safe + "plang"] = row["ContactPrimaryLanguage"];
                    }
                }

                result.Rows.Add(newRow);
            }

            return result;
        }

        private void ExportToExcel(System.Data.DataTable dt, string Filename, HttpResponse response)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");

            ICellStyle headerStyle = workbook.CreateCellStyle();
            headerStyle.FillForegroundColor = IndexedColors.LightBlue.Index;
            headerStyle.FillPattern = FillPattern.SolidForeground;
            headerStyle.Alignment = HorizontalAlignment.Center;  
            headerStyle.VerticalAlignment = VerticalAlignment.Center; 

            IRow headerRow = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = headerRow.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
                cell.CellStyle = headerStyle;
            }

            ICellStyle dataStyle = workbook.CreateCellStyle();
            dataStyle.Alignment = HorizontalAlignment.Center; 
            dataStyle.VerticalAlignment = VerticalAlignment.Center;  

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                    cell.CellStyle = dataStyle; 
                }
            }

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                int columnLength = dt.Columns[i].ColumnName.Length;  
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    int cellLength = dt.Rows[j][i].ToString().Length;
                    columnLength = Math.Max(columnLength, cellLength);  
                }

                sheet.SetColumnWidth(i, (columnLength + 2) * 256);  
            }

            response.Clear();
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.AddHeader("Content-Disposition", "attachment;filename=" + Filename);

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                response.BinaryWrite(ms.ToArray());
            }

            response.End();

        }

        private System.Data.DataTable Getallcontact(string scoolid,string ids)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());
            string qry = "";
            if (ids == "0")
            {
                 qry = "SELECT   SP.StudentPersonalId, SP.SchoolId, SP.LastName AS ReferralLast , SP.FirstName AS ReferralFirst, LP.LookupName AS Relationship,  CASE WHEN SP.Gender = '1' THEN 'Male' ELSE 'Female' END AS Gender, CONVERT(VARCHAR(10), SP.BirthDate, 101) AS BirthDate, CONVERT(VARCHAR(10),SP.AdmissionDate,101) AS DateOfReferral, " +
                                " CP.LastName AS LastName,  CP.FirstName AS FirstName, CASE "
        + " WHEN SAR.ContactSequence = '1' THEN 'Home'"
      + "  WHEN SAR.ContactSequence = '2' THEN 'Work'"
       + "  WHEN SAR.ContactSequence = '3' THEN 'Other'"
     + " END AS TYPE,ADL.STREETNAME + CHAR(13) + CHAR(10) + "
       + "  CASE WHEN ADL.ApartmentType IS NULL THEN ' ' ELSE ADL.ApartmentType END  + CHAR(13) + CHAR(10) + "
        + "  CASE WHEN ADL.City IS NULL THEN ' ' ELSE ADL.City END AS   streetaddress, ADL.Phone AS PHONE, ADL.Mobile AS MOBILE,	ADL.PrimaryEmail AS EMAIL,"
     + " ADL.[City] AS [City], (SELECT LookupName FROM LookUp WHERE LookupType = 'State' AND  LookupId = ADL.StateProvince) AS State   FROM   "
    + " StudentPersonal AS SP INNER JOIN  StudentAddresRel AS SAR ON SP.StudentPersonalId = SAR.StudentPersonalId INNER JOIN AddressList ADL "
      + " ON ADL.AddressId=SAR.AddressId INNER JOIN  ContactPersonal AS CP ON CP.ContactPersonalId = SAR.ContactPersonalId"
      + "  INNER JOIN  StudentContactRelationship AS SCR ON SCR.ContactPersonalId = SAR.ContactPersonalId INNER JOIN "
      + "   LookUp AS LP ON LP.LookupId = SCR.RelationshipId WHERE  "
        + " (SP.StudentType = 'Referral') AND (SAR.ContactSequence <> 0) AND CP.Status=1  "
        + "  ORDER BY SP.AdmissionDate DESC";
            }
            else
            {
                 qry = "SELECT   SP.StudentPersonalId, SP.SchoolId, SP.LastName AS ReferralLast , SP.FirstName AS ReferralFirst, LP.LookupName AS Relationship,  CASE WHEN SP.Gender = '1' THEN 'Male' ELSE 'Female' END AS Gender, CONVERT(VARCHAR(10), SP.BirthDate, 101) AS BirthDate, CONVERT(VARCHAR(10),SP.AdmissionDate,101) AS DateOfReferral, " +
                                                " CP.LastName AS LastName,  CP.FirstName AS FirstName, CASE "
                        + " WHEN SAR.ContactSequence = '1' THEN 'Home'"
                      + "  WHEN SAR.ContactSequence = '2' THEN 'Work'"
                       + "  WHEN SAR.ContactSequence = '3' THEN 'Other'"
                     + " END AS TYPE,ADL.STREETNAME + CHAR(13) + CHAR(10) + "
                       + "  CASE WHEN ADL.ApartmentType IS NULL THEN ' ' ELSE ADL.ApartmentType END  + CHAR(13) + CHAR(10) + "
                        + "  CASE WHEN ADL.City IS NULL THEN ' ' ELSE ADL.City END AS   streetaddress, ADL.Phone AS PHONE, ADL.Mobile AS MOBILE,	ADL.PrimaryEmail AS EMAIL,"
                     + " ADL.[City] AS [City], (SELECT LookupName FROM LookUp WHERE LookupType = 'State' AND  LookupId = ADL.StateProvince) AS State   FROM   "
                    + " StudentPersonal AS SP INNER JOIN  StudentAddresRel AS SAR ON SP.StudentPersonalId = SAR.StudentPersonalId INNER JOIN AddressList ADL "
                      + " ON ADL.AddressId=SAR.AddressId INNER JOIN  ContactPersonal AS CP ON CP.ContactPersonalId = SAR.ContactPersonalId"
                      + "  INNER JOIN  StudentContactRelationship AS SCR ON SCR.ContactPersonalId = SAR.ContactPersonalId INNER JOIN "
                      + "   LookUp AS LP ON LP.LookupId = SCR.RelationshipId WHERE  "
                        + " (SP.StudentType = 'Referral') AND (SAR.ContactSequence <> 0) AND CP.Status=1  "
                        + "  AND SP.StudentPersonalId in ("+ids+") ORDER BY SP.AdmissionDate DESC";
            }
            SqlCommand cmd = new SqlCommand(qry, conn);
            cmd.CommandTimeout = 1200;
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }


        }
        string Getrow(DataRow row, string columnName)
        {
            if (row.IsNull(columnName))
                return string.Empty;

            var value = row[columnName].ToString();
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        string Addlines(IEnumerable<DataRow> rows, string columnName)
        {
            var list = rows.Select(r =>
            {
                var value = r.IsNull(columnName) ? "" : r[columnName].ToString().Trim();
                return string.IsNullOrWhiteSpace(value) ? "&nbsp;" : value; // preserve row height if empty
            }).ToList();

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < list.Count; i++)
            {
                result.Append(list[i]);
                if (i < list.Count - 1)
                {
                    result.Append("<div style='border-bottom:1px solid gray; margin:4px 0;'></div>");
                }
            }

            return result.ToString();
        }



        private string GenerateHtmlTablecont(System.Data.DataTable dt)
        {
            var uniqueRows = dt.AsEnumerable().Distinct(DataRowComparer.Default);
            dt = uniqueRows.CopyToDataTable();

            StringBuilder html = new StringBuilder();

            html.Append("<table id='trackingactive' class='display' border='1' style='width: 100%; border-collapse: collapse; table-layout: auto;white-space: nowrap; width: max-content;text-align: center; vertical-align: middle;'>");
            html.Append("<thead>");
            html.Append("<tr style='background-color: #111184; color: white; height: 40px;'>");
            html.Append("<th>Referral Last</th>");
            html.Append("<th>Referral First</th>");
            html.Append("<th>DOB</th>");
            html.Append("<th>Admission Date</th>");
            html.Append("<th>Relationship</th>");
            html.Append("<th>Last</th>");
            html.Append("<th>First</th>");
            html.Append("<th>Type</th>");
            html.Append("<th>Street Address</th>");
            html.Append("<th>Phone</th>");
            html.Append("<th>Mobile</th>");
            html.Append("<th>Email</th>");
            html.Append("</tr>");
            html.Append("</thead>");

            html.Append("<tbody>");

            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt.Columns.Add("sid", typeof(string));
            Dt.Columns.Add("Referrallast", typeof(string));
            Dt.Columns.Add("Referralfirst", typeof(string));
            Dt.Columns.Add("DOB", typeof(string));
            Dt.Columns.Add("Admissiondate", typeof(string));
            Dt.Columns.Add("Relationship", typeof(string));
            Dt.Columns.Add("last", typeof(string));
            Dt.Columns.Add("first", typeof(string));
            Dt.Columns.Add("type", typeof(string));
            Dt.Columns.Add("street", typeof(string));
            Dt.Columns.Add("phone", typeof(string));
            Dt.Columns.Add("mobile", typeof(string));
            Dt.Columns.Add("email", typeof(string));

            foreach (DataRow rows in dt.Rows)
            {
                DataRow drow = Dt.NewRow();
                drow["sid"] = rows["StudentPersonalId"].ToString();
                drow["Referrallast"] = Getrow(rows, "ReferralLast");
                drow["Referralfirst"] = Getrow(rows, "ReferralFirst");
                drow["DOB"] = Getrow(rows, "BirthDate");
                drow["Admissiondate"] = Getrow(rows, "DateOfReferral");
                drow["Relationship"] = Getrow(rows, "Relationship");
                drow["last"] = Getrow(rows, "LastName");
                drow["first"] = Getrow(rows, "FirstName");
                drow["type"] = Getrow(rows, "TYPE");
                drow["street"] = Getrow(rows, "streetaddress");
                drow["phone"] = Getrow(rows, "PHONE");
                drow["mobile"] = Getrow(rows, "MOBILE");
                drow["email"] = Getrow(rows, "EMAIL");
                Dt.Rows.Add(drow);
            }

            ViewState["data"] = DataTableToJson(Dt);
            var groupedStudents = Dt.AsEnumerable().GroupBy(r => r["sid"].ToString());

            int rowIndex = 0;

            foreach (var studentGroup in groupedStudents)
            {
                var first = studentGroup.First();

                string relationships = Addlines(studentGroup, "Relationship");
                string lastNames = Addlines(studentGroup, "last");
                string firstNames = Addlines(studentGroup, "first");
                string types = Addlines(studentGroup, "type");
                string streets = Addlines(studentGroup, "street");
                string phones = Addlines(studentGroup, "phone");
                string mobiles = Addlines(studentGroup, "mobile");
                string emails = Addlines(studentGroup, "email");

                // Alternate row colors
                string rowStyle = rowIndex % 2 == 0
                    ? "style='background-color: white;'"
                    : "style='background-color: rgba(0, 0, 0, 0.08);'";

                html.AppendFormat("<tr {0}>", rowStyle);
                html.AppendFormat("<td>{0}</td>", first["Referrallast"]);
                html.AppendFormat("<td>{0}</td>", first["Referralfirst"]);
                html.AppendFormat("<td>{0}</td>", first["DOB"]);
                html.AppendFormat("<td>{0}</td>", first["Admissiondate"]);
                html.AppendFormat("<td>{0}</td>", relationships);
                html.AppendFormat("<td>{0}</td>", lastNames);
                html.AppendFormat("<td>{0}</td>", firstNames);
                html.AppendFormat("<td>{0}</td>", types);
                html.AppendFormat("<td>{0}</td>", streets);
                html.AppendFormat("<td>{0}</td>", phones);
                html.AppendFormat("<td>{0}</td>", mobiles);
                html.AppendFormat("<td>{0}</td>", emails);
                html.Append("</tr>");

                rowIndex++;
            }

            html.Append("</tbody>");
            html.Append("</table>");

            return html.ToString();
        }


        private string chngColumnName(string input)
        {
            return input.Replace(" ", "_").Replace("/", "_").Replace("-", "_");
        }
        private System.Data.DataTable Getfunddata(string scoolid, string status)
        {
            string qer = "SELECT StudentPersonalId,QueueProcess,QueueStatusId,StaffName,Nameofcontact,studentName,SchoolId,DateOfReferral,City,State,ImageUrl,Funded"
         + " FROM (SELECT StudentPersonalId,QueueProcess,FUNDVSNONFUND.QueueStatusId,REFCL.StaffName,REFCL.Nameofcontact,studentName,FUNDVSNONFUND.SchoolId,"
        + " DateOfReferral,City,State,ImageUrl,Funded FROM (SELECT *,"
        + "(SELECT QueueStatusId FROM ref_QueueStatus WHERE StudentPersonalId=FUND.StudentPersonalId AND QueueProcess=FUND.QueueProcess AND "
        + " QueueId=(SELECT QueueId FROM ref_Queue WHERE QueueType='FV')  AND Draft='N' AND CurrentStatus='false' ) AS QueueStatusId"
         + " FROM (SELECT SP.StudentPersonalId,SP.LastName+','+SP.FirstName AS studentName,SP.SchoolId,CONVERT(VARCHAR(10), SP.[AdmissionDate], 101) AS [DateOfReferral],"
         + " ADL.City AS City,(SELECT LookupName FROM LookUp WHERE LookupType = 'State' AND LookupId = ADL.StateProvince) AS State, (SELECT MAX(QueueProcess) FROM ref_QueueStatus WHERE StudentPersonalId=SP.StudentPersonalId) AS QueueProcess,"
         + " CASE WHEN SP.ImageUrl IS NULL OR SP.ImageUrl='' THEN CASE WHEN Gender=1 THEN (SELECT FormatImg FROM [dbo].[DefaultImage] WHERE Sex='M')"
                + " ELSE  (SELECT FormatImg FROM [dbo].[DefaultImage] WHERE Sex='F')"
                + " END ELSE [ImageUrl] END AS [ImageUrl],CASE WHEN SP.FundingVerification='True' THEN 'FD' ELSE 'NF' END  Funded"
         + " FROM StudentPersonal SP INNER JOIN StudentAddresRel SDR ON SDR.StudentPersonalId=SP.StudentPersonalId"
             + " INNER JOIN AddressList ADL ON ADL.AddressId=SDR.AddressId WHERE SP.StudentType='Referral') FUND) FUNDVSNONFUND"
         + " LEFT JOIN ref_CallLogs AS REFCL ON REFCL.QueueStatusId=FUNDVSNONFUND.QueueStatusId)  FUNDEDVSNONFUNDED"
         + " ORDER BY DATEPART(YEAR,DateOfReferral) DESC, DATEPART(MONTH,DateOfReferral) DESC, DATEPART(DAY,DateOfReferral) DESC";

            System.Data.DataTable Dt = new System.Data.DataTable();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString());

            SqlCommand cmd = new SqlCommand(qer, conn);
            cmd.CommandTimeout = 1200;
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private string GenerateHtmlTablefund(System.Data.DataTable dt, string status)
        {
            //ViewState["data"] = DataTableToJson(dt);
            StringBuilder html = new StringBuilder();
            html.Append("<table id='trackingactive' class='display' border='1' style='width: 80%; border-collapse: collapse;text-align: center; vertical-align: middle;'>");
            html.Append("<thead>");
            html.Append("<tr style='background-color: #111184; color: white; height: 40px;'>");
            html.Append("<th rowspan='2'>Referral Name</th>");
            html.Append("<th rowspan='2'>Date Of Referral</th>");
            html.Append("<th rowspan='2'>City</th>");
            html.Append("<th rowspan='2'>State</th>");
            html.Append("<th colspan='2'>Contact log details</th>");

            html.Append("</tr>");
            html.Append("<tr style='background-color: #111184; color: white; height: 40px;'>");

            html.Append("<th> Name of contact</th><th>Staff Name</th>");
            html.Append("</tr>");

            html.Append("</thead>");

            html.Append("<tbody>");

            System.Data.DataTable Dt = new System.Data.DataTable();
            Dt.Columns.Add("ReferralName", typeof(string));
            Dt.Columns.Add("DateofReferral", typeof(string));
            Dt.Columns.Add("City", typeof(string));
            Dt.Columns.Add("State", typeof(string));
            Dt.Columns.Add("nameofcontact", typeof(string));
            Dt.Columns.Add("staffname", typeof(string));

            if (dt != null && dt.Rows.Count > 0)
            {
                var distinctRows = dt.AsEnumerable()
                                .GroupBy(row => row["StudentPersonalId"])
                                .Select(group => group.First())
                                .CopyToDataTable();
                dt = distinctRows;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Funded"].ToString() == status)
                    {
                        DataRow row = Dt.NewRow();
                        row["ReferralName"] = dt.Rows[i]["studentName"].ToString(); ;
                        row["DateofReferral"] = dt.Rows[i]["DateOfReferral"].ToString();
                        row["City"] = dt.Rows[i]["City"].ToString();
                        row["State"] = dt.Rows[i]["State"].ToString();
                        row["nameofcontact"] = dt.Rows[i]["Nameofcontact"].ToString();
                        row["staffname"] = dt.Rows[i]["StaffName"].ToString();
                        Dt.Rows.Add(row);
                    }
                }
                ViewState["data"] = DataTableToJson(Dt);

            }
            int rowIndex = 0;
            foreach (DataRow rows in Dt.Rows)
            {
                string rowStyle = (rowIndex % 2 == 0)
                    ? "background-color: white;"
                    : "background-color: rgba(0, 0, 0, 0.08);";

                html.AppendFormat("<tr style='{0}'>", rowStyle);
                html.AppendFormat("<td style='height:40px;'>{0}</td>", rows["ReferralName"]);
                html.AppendFormat("<td>{0}</td>", rows["DateofReferral"]);
                html.AppendFormat("<td>{0}</td>", rows["City"]);
                html.AppendFormat("<td>{0}</td>", rows["State"]);
                html.AppendFormat("<td>{0}</td>", rows["nameofcontact"]);
                html.AppendFormat("<td>{0}</td>", rows["staffname"]);
                html.Append("</tr>");

                rowIndex++;
            }

            html.Append("</tbody>");
            html.Append("</table>");
            return html.ToString();
        }

        protected void btnexport3_Click(object sender, EventArgs e)
        {

            System.Data.DataTable dt = JsonToDataTable(ViewState["data"].ToString());


            var Dt = BuildDataTablefund(dt);

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Referral Report");

            ICellStyle headerStyle = workbook.CreateCellStyle();
            headerStyle.FillForegroundColor = IndexedColors.LightBlue.Index;
            headerStyle.FillPattern = FillPattern.SolidForeground;
            headerStyle.Alignment = HorizontalAlignment.Center;

            headerStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            headerStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            headerStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            headerStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

            headerStyle.TopBorderColor = IndexedColors.White.Index;
            headerStyle.BottomBorderColor = IndexedColors.White.Index;
            headerStyle.LeftBorderColor = IndexedColors.White.Index;
            headerStyle.RightBorderColor = IndexedColors.White.Index;

            IFont headerFont = workbook.CreateFont();
            headerFont.IsBold = true;
            headerFont.Color = IndexedColors.White.Index;
            headerStyle.SetFont(headerFont);
            int rowIndex = 0;

            IRow headerRow1 = sheet.CreateRow(rowIndex++);
            IRow headerRow2 = sheet.CreateRow(rowIndex++);

            string[] fixedHeaders = { "Referral Name", "Date Of Referral", "City", "State" };

            int colIndex = 0;

            foreach (var header in fixedHeaders)
            {
                var cell = headerRow1.CreateCell(colIndex);
                cell.SetCellValue(header);
                cell.CellStyle = headerStyle;
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, colIndex, colIndex));
                colIndex++;
            }

            var cell1 = headerRow1.CreateCell(colIndex);
            cell1.SetCellValue("Contact log details");
            cell1.CellStyle = headerStyle;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, colIndex, colIndex + 1));

            var sub1 = headerRow2.CreateCell(colIndex++);
            sub1.SetCellValue("Name of contact");
            sub1.CellStyle = headerStyle;

            var sub2 = headerRow2.CreateCell(colIndex++);
            sub2.SetCellValue("Staff Name");
            sub2.CellStyle = headerStyle;


            foreach (DataRow dataRow in Dt.Rows)
            {
                IRow excelRow = sheet.CreateRow(rowIndex++);
                colIndex = 0;

                foreach (var header in fixedHeaders)
                {
                    excelRow.CreateCell(colIndex++).SetCellValue(dataRow[header].ToString());
                }

                string reltn = chngColumnName("Contact log details");
                excelRow.CreateCell(colIndex++).SetCellValue(dataRow["nameofcontact"].ToString());
                excelRow.CreateCell(colIndex++).SetCellValue(dataRow["StaffName"].ToString());
            }
            for (int i = 0; i < colIndex; i++)
            {
                sheet.SetColumnWidth(i, 25 * 256); 
            }

            using (MemoryStream exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=Referral_All_FundedvsNonfunded_Report.xlsx");
                HttpContext.Current.Response.BinaryWrite(exportData.ToArray());
                HttpContext.Current.Response.End();
            }

        }
        private void ExportToExcelcontact(System.Data.DataTable dt, string fileName)
{
    IWorkbook workbook = new XSSFWorkbook();
    ISheet sheet = workbook.CreateSheet("Referrals");
    var headerStyle = workbook.CreateCellStyle();
    headerStyle.FillForegroundColor = IndexedColors.Blue.Index;
    headerStyle.FillPattern = FillPattern.SolidForeground;
    var headerFont = workbook.CreateFont();
    headerFont.Color = IndexedColors.White.Index;
    headerFont.IsBold = true;
    headerStyle.SetFont(headerFont);
    int rowIndex = 0;
    IRow header = sheet.CreateRow(rowIndex++);
    IRow hdrRow = sheet.CreateRow(0);
    string[] headers = { "Referral Last", "Referral First", "DOB", "Admission Date",
                         "Relationship", "Last", "First", "Type", "Street", "Phone", "Mobile", "Email" };
    for (int i = 0; i < headers.Length; i++)
    {
        var cell = hdrRow.CreateCell(i);
        cell.SetCellValue(headers[i]);
        cell.CellStyle = headerStyle;
    }
    sheet.SetColumnWidth(0, 20 * 256); 
    sheet.SetColumnWidth(1, 20 * 256); 
    sheet.SetColumnWidth(2, 15 * 256); 
    sheet.SetColumnWidth(3, 18 * 256); 
    sheet.SetColumnWidth(4, 20 * 256); 
    sheet.SetColumnWidth(5, 20 * 256); 
    sheet.SetColumnWidth(6, 20 * 256); 
    sheet.SetColumnWidth(7, 15 * 256); 
    sheet.SetColumnWidth(8, 30 * 256); 
    sheet.SetColumnWidth(9, 15 * 256); 
    sheet.SetColumnWidth(10, 15 * 256); 
    sheet.SetColumnWidth(11, 30 * 256); 

    var grouped = dt.AsEnumerable().GroupBy(r => r["sid"].ToString());
    foreach (var group in grouped)
    {
        int startRow = rowIndex;
        bool first = true;
        foreach (var dr in group)
        {
            IRow r = sheet.CreateRow(rowIndex);
            int col = 0;
            if (first)
            {
                r.CreateCell(col++).SetCellValue(dr["Referrallast"].ToString());
                r.CreateCell(col++).SetCellValue(dr["Referralfirst"].ToString());
                r.CreateCell(col++).SetCellValue(dr["DOB"].ToString());
                r.CreateCell(col++).SetCellValue(dr["Admissiondate"].ToString());
                first = false;
            }
            else
            {
                col += 4; // skip those cols
            }
            r.CreateCell(col++).SetCellValue(dr["Relationship"].ToString());
            r.CreateCell(col++).SetCellValue(dr["last"].ToString());
            r.CreateCell(col++).SetCellValue(dr["first"].ToString());
            r.CreateCell(col++).SetCellValue(dr["type"].ToString());
            r.CreateCell(col++).SetCellValue(dr["street"].ToString());
            r.CreateCell(col++).SetCellValue(dr["phone"].ToString());
            r.CreateCell(col++).SetCellValue(dr["mobile"].ToString());
            r.CreateCell(col++).SetCellValue(dr["email"].ToString());
            rowIndex++;
        }
        int endRow = rowIndex - 1;
        if (endRow > startRow)
        {
            for (int c = 0; c < 4; c++)
                sheet.AddMergedRegion(new CellRangeAddress(startRow, endRow, c, c));
        }
    }

    //for (int i = 0; i < headers.Length; i++)
    //    sheet.AutoSizeColumn(i);

    using (var ms = new MemoryStream())
    {
        workbook.Write(ms);
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.AddHeader("content-disposition",
            string.Format("attachment;filename={0}.xlsx", fileName));
        HttpContext.Current.Response.ContentType =
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        HttpContext.Current.Response.BinaryWrite(ms.ToArray());
        HttpContext.Current.Response.End();
    }
    
}
        private System.Data.DataTable BuildDataTablefund(System.Data.DataTable dt)
        {
            System.Data.DataTable result = new System.Data.DataTable();
            result.Columns.Add("Referral Name");
            result.Columns.Add("Date Of Referral");
            result.Columns.Add("City");
            result.Columns.Add("State");
            result.Columns.Add("nameofcontact");
            result.Columns.Add("StaffName");


            for (int i = 0; i < dt.Rows.Count; i++)
            {

                DataRow row = result.NewRow();
                row["Referral Name"] = dt.Rows[i]["ReferralName"].ToString(); ;
                row["Date Of Referral"] = dt.Rows[i]["DateOfReferral"].ToString();
                    row["City"] = dt.Rows[i]["City"].ToString();
                    row["State"] = dt.Rows[i]["State"].ToString();
                    row["nameofcontact"] = dt.Rows[i]["Nameofcontact"].ToString();
                    row["staffname"] = dt.Rows[i]["StaffName"].ToString();
                    result.Rows.Add(row);
                
            }

            return result;
        }

        protected void btncontactshow_Click(object sender, EventArgs e)
        {
            var selectedItems = ddlReferrals.Items.Cast<ListItem>().Where(i => i.Selected).ToList();
            System.Data.DataTable dt = null;
            sess = (clsSession)Session["UserSession"];
            
            if (selectedItems.Count == 0 || selectedItems.Count == ddlReferrals.Items.Count)
            {
                dt = Getallcontact(sess.SchoolId.ToString(), "0");
               
    }
            else
            {
                string studs = string.Join(",", selectedItems.Select(i => i.Value));
                dt = Getallcontact(sess.SchoolId.ToString(), studs);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                ViewState["alldata"] = DataTableToJson(dt);
                string htmlTable = GenerateHtmlTablecont(dt);
                reporttable.Visible = true;
                reporttable.InnerHtml = htmlTable;
                string script3 = "Applypagination2();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "show5", script3, true);
                Btnexport1.Visible = true;
            }
            else
            {
                reporttable.Visible = true;
                reporttable.InnerHtml = "No data available";
                Btnexport.Visible = false;
                Btnexport1.Visible = false;
                Btnexport3.Visible = false;
                btnexporttr.Visible = false;
                btnexportqtr.Visible = false;
                btnexportloc.Visible = false;



            }
            string script2 = "hideoverlay();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "show6", script2, true);
        }

    }

}