<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="~/Reports/ReferralReports.aspx.cs" Inherits="ReferalDB.Reports.ReferralReports" %>


<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
 
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="../Scripts/jquery-1.8.2.js" type="text/javascript"></script>
    <link href="../CSS/StyleMaster.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/StyleControl.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/StyleLeftPanel.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/StyleBars.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#UserName').load('../Dashboard/GetUserName');// To load User Name 
            $.get("../Dashboard/GetTitleReport", function (data) {
                document.title = data;
                
            });
            var MenuType = document.getElementById('hdnMenu').value;
            if (MenuType != "") {
                ChangeSelectedMenu(MenuType);
            }
                       
        });


        function ChangeSelectedMenu(MenuClass) {
            if ($(".ActiveRef")[0]){
                $('.ActiveRef').attr('class', 'allexp');
            }
            $('#' + MenuClass).attr('class', 'ActiveRef');
        }




        function validate(key) {
            var keycode = (key.which) ? key.which : key.keyCode;
            //comparing pressed keycodes
            if (!(keycode == 8 || keycode == 46) && (keycode < 48 || keycode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <style type="text/css">
        .linkstyle {
            padding:8px;
        }
        
    </style>
    <title id="tileid"></title>
    
</head>
    

<body>
    <form id="form1" runat="server">
        <div class="mainContainer">
            <div class="topHead">
                <a class="admin" href="#">
                    <div id="UserName"></div>
                </a>
                <a class="logout" href="../Login/Logout">Logout</a>
            </div>
            <div class="Dashboard-logo"></div>
            <div class="contentPart">
                <div class="logoContainer">
                    <div class="imgcorner">
                    </div>
                    <div> <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:HiddenField ID="hdnType" runat="server" />
                    </div>
                    <ul>
                        <li onclick=""><a href="../AdminView/AdminView">
                            <img src="../Images/visual.png" width="26" height="23" alt="" /><br>
                            Admin</a></li>
                        <li onclick=""><a href="../Dashboard/Dashboard">
                            <img src="../Images/home.png" width="26" height="23" alt="" /><br>
                            Home</a></li>
                        <%--<li onclick=""><a href="">
                            <img src="../Images/Reports.png" width="26" height="23" alt="" /><br>
                            Reports</a></li>--%>

                    </ul>
                </div>
                <div class="clear"></div>

                <div class="ContentAreaContainer">
                    <div class="leftContainer" style="width: 19%">

                        <ul>


                            <li id="" class="accordion" onclick="" style="position: static;">
                                <h5 id="AllReferral" class="allexp">
                                    <span class="dd"></span>
                                    <asp:LinkButton ID="LbtnAllReferral" CssClass="linkstyle" runat="server" Text="All Referrals" ToolTip="All Referrals" ForeColor="White" Height="70%" Width="100%" OnClick="LbtnAllReferral_Click"></asp:LinkButton>
                             
                                </h5>
                            </li>

                            <li id="Li1" class="accordion" onclick="" style="position: static;">
                                <h5 id="RefTrackActive" class="allexp" >
                                    <span class="dd"></span>
                                   
                                    <asp:LinkButton ID="LbtnRefTrackActive" CssClass="linkstyle" runat="server" Text="All Referrals Tracking Active" ToolTip="All Referrals Tracking Active" ForeColor="White" Height="70%" Width="100%" OnClick="LbtnRefTrackActive_Click" ></asp:LinkButton>
                                </h5>
                            </li>
                            <li id="Li2" class="accordion" onclick="" style="position: static;">
                                <h5 id="RefAgeRange" class="allexp"  >
                                    <span class="dd"></span>
                           
                                    <asp:LinkButton ID="LbtnRefAgeRange" CssClass="linkstyle" runat="server" Text="All Referrals by Age Range" ToolTip="All Referrals by Age Range" ForeColor="White" Height="70%" Width="100%" OnClick="LbtnRefAgeRange_Click" ></asp:LinkButton>
                                </h5>
                            </li>

                            <li id="Li3" class="accordion" onclick="" style="position: static;">
                                <h5 id="TackingActiveAge" class="allexp"  >
                                    <span class="dd"></span>
                             
                                    <asp:LinkButton ID="LbtnTackingActiveAge" CssClass="linkstyle" runat="server" Text="All Referrals Tracking Active by Age Range" ToolTip="All Referrals Tracking Active by Age Range" ForeColor="White" Height="70%" Width="100%" OnClick="LbtnTackingActiveAge_Click" ></asp:LinkButton>
                                </h5>
                            </li>
                            <li id="Li4" class="accordion" onclick="" style="position: static;">
                                <h5 id="RefContact" class="allexp"  >
                                    <span class="dd"></span>
                             
                                    <asp:LinkButton ID="LbtnRefContact" runat="server" CssClass="linkstyle" Text="All Contact Events" ToolTip="All Contact Events" ForeColor="White" Height="70%" Width="100%" OnClick="LbtnRefContact_Click" ></asp:LinkButton>
                                </h5>
                            </li>
                           
                            <li id="Li6" class="accordion" onclick="" style="position: static;">
                                <h5 id="RefFunded" class="allexp"  >
                                    <span class="dd"></span>
                              
                                    <asp:LinkButton ID="LbtnRefFunded" runat="server" CssClass="linkstyle" Text="All Referrals by Funded vs. Not Funded" ToolTip="All Referrals by Funded vs. Not Funded" ForeColor="White" Height="70%" Width="100%" OnClick="LbtnRefFunded_Click" ></asp:LinkButton>
                                </h5>
                            </li>
                            <li id="Li7" class="accordion" onclick="" style="position: static;">
                                <h5 id="RefLocation" class="allexp"  >
                                    <span class="dd"></span>
                               
                                    <asp:LinkButton ID="LbtnRefLocation" runat="server" CssClass="linkstyle" Text="All Referrals by Location" ToolTip="All Referrals by Location" ForeColor="White" Height="70%" Width="100%" OnClick="LbtnRefLocation_Click" ></asp:LinkButton>
                                </h5>
                            </li>
                            <li id="Li8" class="accordion" onclick="" style="position: static;">
                                <h5 id="RefBirthdateQuarter" class="allexp"  >
                                    <span class="dd"></span>
                     
                                    <asp:LinkButton ID="LbtnRefBirthdateQuarter" runat="server" CssClass="linkstyle" Text="All Referrals by Birthdate Quarter" ToolTip="All Referrals by Birthdate Quarter" ForeColor="White" Height="70%" Width="100%" OnClick="LbtnRefBirthdateQuarter_Click" ></asp:LinkButton>
                                </h5>
                            </li>




                        </ul>
                    </div>
                    <div class="middleContainer" id="MidContent" style="width:79%">
                                         
                        <div class="headingDivBar" style="width: 100%" id="HeadingDiv" runat="server" visible="false">

                       

                            </div>
                          <div  style="float: left; width: 100%"  id="tdMsg" runat="server">
                                
                            </div>
                        <br />
                        <div class="clear"></div>
                        <div>
                            <div id="referralage" runat="server" visible="false">
                            <table style="width:100%" >
                                <tr>
                                    <td >
                                        <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label></td><td>
                                        <asp:DropDownList ID="ddlStatus" runat="server">
                                            <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                                            <asp:ListItem Value="AV">Active</asp:ListItem>
                                            <asp:ListItem Value="IL">Inactive</asp:ListItem>
                                        </asp:DropDownList>
                                        </td>
                                    <td><asp:Label ID="lblageStart" runat="server" Text="Age Between"></asp:Label></td><td>
                                        <asp:TextBox ID="txtStartAge" runat="server" onkeypress="return validate(event)" ></asp:TextBox></td> <td><asp:Label ID="lblageend" runat="server" Text="And"></asp:Label></td>
                                    <td><asp:TextBox ID="txtEndAge" runat="server" onkeypress="return validate(event)" ></asp:TextBox></td>
                                    <td>
                                        <asp:Button ID="btnShowReport" runat="server" Text="Show Report" OnClick="btnShowReport_Click" /></td>
                                </tr>
                            </table>
                                </div>
                            <div id="divfunded" runat="server" visible="false">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:15%">
                                            <asp:Label ID="lblfunding" runat="server"  Text="Funding Status"></asp:Label>
                                        </td>
                                        <td style="width:25%">
                                            <asp:DropDownList ID="ddlFundingStatus" runat="server">
                                                <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                                                <asp:ListItem Value="FD">Funded</asp:ListItem>
                                                <asp:ListItem Value="NF">Not Funded</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:15%">
                                            <asp:Button ID="btnshowgraph" runat="server" Text="Show Report" OnClick="btnshowgraph_Click" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                             <div id="divlocation" runat="server" visible="false">
                                <table style="width:100%;float:left">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStatusdata" runat="server"  Text="State"></asp:Label>
                                        </td>
                                        <td >
                                            <asp:DropDownList ID="ddlState" runat="server">                                               
                                            </asp:DropDownList>
                                        </td>
                                        <td><asp:Label ID="lblcity" runat="server"  Text="City"></asp:Label></td>
                                        <td > <asp:TextBox ID="txtcity" runat="server" ></asp:TextBox> </td>
                                        <td >
                                            <asp:Button ID="btnlocation" runat="server" Text="Show Report" OnClick="btnlocation_Click"  />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                             <div id="divbirthdate" runat="server" visible="false">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:15%">
                                            <asp:Label ID="Label1" runat="server"  Text="Birthdate Quarter"></asp:Label>
                                        </td>
                                        <td style="width:25%">
                                            <asp:DropDownList ID="ddlQuarter" runat="server">
                                                <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                                                <asp:ListItem Value="1">January - March</asp:ListItem>
                                                <asp:ListItem Value="2">April - June</asp:ListItem>
                                                <asp:ListItem Value="3">July - September</asp:ListItem>
                                                <asp:ListItem Value="4">October - December</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:15%">
                                            <asp:Button ID="btnquarter" runat="server" Text="Show Report" OnClick="btnquarter_Click"  />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div style="width:100%;overflow-x:auto">
                              <rsweb:ReportViewer ID="RVReferralReport" runat="server" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowBackButton="false" ShowCredentialPrompts="false" ShowDocumentMapButton="true" ShowExportControls="false" ShowFindControls="false" ShowPageNavigationControls="true" ShowParameterPrompts="true" ShowPrintButton="false" ShowPromptAreaButton="true" ShowRefreshButton="false" ShowToolBar="true" ShowWaitControlCancelLink="true" ShowZoomControl="false"   Width="100%"  Visible="false" AsyncRendering="true" Height="1000px">

                                    <ServerReport     ReportServerUrl="<%$ appSettings:ReportUrl %>" />                            

                                </rsweb:ReportViewer>
                     
                            </div>
                        <div>
                            <asp:HiddenField ID="hdnMenu" runat="server" />
                        </div>
                    </div>



                    <div class="clear"></div>
                </div>

                <div class="clear"></div>
            </div>

            <div class="clear"></div>
            <div class="footer">
                <img src="../Images/smllogo.JPG" width="109" height="18" />
                <div class="copyright">Copyright 2013 Melmark.org , Allrights Reserved.</div>
            </div>


            <div class="clear"></div>
        </div>





    </form>





</body>


</html>
