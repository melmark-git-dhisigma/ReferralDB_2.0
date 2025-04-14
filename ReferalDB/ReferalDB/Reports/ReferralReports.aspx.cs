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
namespace ReferalDB.Reports
{
    public partial class ReferralReports : System.Web.UI.Page
    {
        public clsSession sess = null;
        System.Data.DataTable alldata;
        protected void Page_Load(object sender, EventArgs e)
        {
            Btnexport.Visible = false;
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
            Btnexport.Visible = false;
            allgrid.Visible = false;
            if (highcheck.Checked == false)
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
                 ViewState["alldata"] = DataTableToJson(alldata);
                allgrid.DataSource = alldata;
                allgrid.DataBind();
                string script2 = "hideoverlay();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "show2", script2, true);
                Btnexport.Visible = true;

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
            var distinctRows = Dt.AsEnumerable()
                            .GroupBy(row => row["Referral Name"])
                            .Select(group => group.First())
                            .CopyToDataTable();
            return distinctRows;
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
            if (Dt != null && Dt.Rows.Count > 0)
            {
                var distinctRows = Dt.AsEnumerable()
                            .GroupBy(row => row["Referral Name"])
                            .Select(group => group.First())
                            .CopyToDataTable();
                return distinctRows;
            }
            else
            {
                return Dt;
            }
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
            var distinctRows = Dt.AsEnumerable()
                            .GroupBy(row => row["Referral Name"])
                            .Select(group => group.First())
                            .CopyToDataTable();
            return distinctRows;
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
                reporttable.Visible = false;
            reporttable.InnerHtml = "";
            Btnexport.Visible = false;
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
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            Btnexport.Visible = false;
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
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            Btnexport.Visible = false;
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
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            Btnexport.Visible = false;
            allgrid.Visible = false;
            hdnMenu.Value = "RefContact";
            RVReferralReport.SizeToReportContent = true;
            tdMsg.InnerHtml = "";
            RVReferralReport.Visible = false;
            HeadingDiv.Visible = true;
            divfunded.Visible = false;
            HeadingDiv.InnerHtml = "All Contact Events";
            referralage.Visible = false;
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

        protected void LbtnRefFunded_Click(object sender, EventArgs e)
        {
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            Btnexport.Visible = false;
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
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            Btnexport.Visible = false;
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
        }

        protected void LbtnRefBirthdateQuarter_Click(object sender, EventArgs e)
        {
            reporttable.Visible = false;
            reporttable.InnerHtml = "";
            Btnexport.Visible = false;
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
                    if (highcheck.Checked == false)
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
                        ViewState["alldata"] = DataTableToJson(alldata);
                        string htmlTable = GenerateHtmlTable(alldata);
                        reporttable.Visible = true;
                        reporttable.InnerHtml = htmlTable;
                        string script3 = "Applypagination();";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "show3", script3, true);
                        Btnexport.Visible = true;
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
                    if (highcheck.Checked == false)
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
                            Btnexport.Visible = true;
                        }
                        else
                        {
                            reporttable.Visible = true;
                            reporttable.InnerHtml = "No data available";
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

            sb.Append("<table id='trackingactive' class='display' border='1' style='width: 80%; border-collapse: collapse;'>");

            sb.Append("<thead>");
            sb.Append("<tr>");
            foreach (DataColumn column in dataTable.Columns)
            {
                sb.AppendFormat("<th style='background-color: #111184; color: white; padding: 8px; text-align: left;'>{0}</th>", column.ColumnName);
            }
            sb.Append("</tr>");
            sb.Append("</thead>");

            sb.Append("<tbody>");
            foreach (DataRow row in dataTable.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn column in dataTable.Columns)
                {
                    sb.AppendFormat("<td style='padding: 8px; text-align: left;'>{0}</td>", row[column]);
                }
                sb.Append("</tr>");
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
                tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Funding status");
                ddlFundingStatus.Focus();
            }

        }

        protected void btnlocation_Click(object sender, EventArgs e)
        {
            RVReferralReport.Visible = false;
            if (ddlState.SelectedItem.Value != "0" && txtcity.Text != "")
            {
                tdMsg.InnerHtml = "";
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
            else if (ddlState.SelectedItem.Value == "0")
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Please select state");
                ddlState.Focus();
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter city");
                txtcity.Focus();
            }
        }

        protected void btnquarter_Click(object sender, EventArgs e)
        {
            RVReferralReport.Visible = false;
            if (ddlQuarter.SelectedItem.Value != "0")
            {
                tdMsg.InnerHtml = "";
                RVReferralReport.Visible = true;
                sess = (clsSession)Session["UserSession"];                
                RVReferralReport.ServerReport.ReportServerCredentials = new CustomReportCredentials(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"], ConfigurationManager.AppSettings["Domain"]);
                RVReferralReport.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReferralReportQuarter"];
                RVReferralReport.ShowParameterPrompts = false;
                ReportParameter[] parm = new ReportParameter[2];
                parm[0] = new ReportParameter("SchoolID", sess.SchoolId.ToString());
                parm[1] = new ReportParameter("Quarter", ddlQuarter.SelectedItem.Value);
                this.RVReferralReport.ServerReport.SetParameters(parm);
                RVReferralReport.ServerReport.Refresh();
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Please select birthdate quarter");
                ddlQuarter.Focus();
            }
        }


        protected void btnexport_Click(object sender, EventArgs e)
        {
            alldata = JsonToDataTable(ViewState["alldata"].ToString());
        sess = (clsSession)Session["UserSession"];
                 string Filename = "ReferralReport" + ".xlsx";
             Filename = Server.UrlEncode(Filename);
                        ExportToExcel(alldata, Filename, Response);
          
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


    }

}