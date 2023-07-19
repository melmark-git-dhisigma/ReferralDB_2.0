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

namespace ReferalDB.Reports
{
    public partial class ReferralReports : System.Web.UI.Page
    {
        public clsSession sess = null;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
               

                RVReferralReport.Visible = false; 
            }
        }

        protected void LoadState()
        {
            DataTable Dt;
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
                    Dt = new DataTable();
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
            hdnMenu.Value= "RefTrackActive";
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
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Status...");
                    ddlStatus.Focus();
                }
            }
            if (hdnType.Value == "Age")
            {
                if (txtStartAge.Text != "" && txtEndAge.Text != "")
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

       


    }
}