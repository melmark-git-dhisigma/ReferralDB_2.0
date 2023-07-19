using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web.SessionState;


namespace BuisinessLayer
{
    public class clsGeneral
    {
        static clsSession sess = null;

        public static string getPageURL()
        {
            string PageUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            return PageUrl;
        }
        public clsSession getSession()
        {
            sess = new clsSession();

            

            return sess;
        }
        public static string sucessMsg(string Msg)
        {
            return "<div class='valid_box'>" + Msg + ".</div>";
        }
        public static string failedMsg(string Msg)
        {
            return "<div class='error_box'>" + Msg + ".</div>";
        }
        public static string warningMsg(string Msg)
        {
            return "<div class='warning_box'>" + Msg + ".</div>";
        }

        public static string sucessMsgWithoutQuote(string Msg)
        {
            return "<div class=valid_box>" + Msg + ".</div>";
        }
        public static string failedMsgWithoutQuote(string Msg)
        {
            return "<div class=error_box>" + Msg + ".</div>";
        }
        public static string warningMsgWithoutQuote(string Msg)
        {
            return "<div class=warning_box>" + Msg + ".</div>";
        }

        public static bool IsPhoneNumber(string Number)
        {
            string rgx = @"[\(\d]{3}\)[\d]{3}-[\d]{4}$";
            //string rgx = @"^[2-9]\d{2}-\d{3}-\d{4}$";
            if (System.Text.RegularExpressions.Regex.IsMatch(Number, rgx))
            {
                return (true);
            }
            else
                return (false);
        }

        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
    }
}
