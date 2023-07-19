<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ReferalDB.Models.LoginModel>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Login</title>
    
    <script src="../../Scripts/jquery-1.8.2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function login() {
            alert('hi')
            $('#divLogin').load('../Login/Login/');
        }
        
        //$('#btnLogin').on('click', function (event) {
        //    alert('hi')
        //   // 
        //});
    </script>
</head>

<body>
    <%using (@Ajax.BeginForm("Login", "Login", FormMethod.Post, new AjaxOptions { UpdateTargetId = "divLogin" }, new { id = "divLogin", enctype = "multipart/form-data" }))
    { %>
    <%--<%using (Html.BeginForm("Login", "Login", FormMethod.Post))
      { %>--%>
    <div id="divLogin">
        
        <div align="center" style="padding-top: 200px;"><b>REFERRAL LOGIN</b></div>
        <table style="padding-top: 50px; width: 100%;">
            <tr>
                <td align="right" width="45%">Username : </td>
                <td><%=Html.TextBoxFor(m=>Model.UserName) %></td>
            </tr>
            <tr>
                <td align="right">Password : </td>
                <td><%=Html.PasswordFor(m=>Model.Password) %></td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <input type="submit" id="btnLogin" value="Login" <%--onclick="login()"--%>/></td>
            </tr>
            <tr>
                <td></td>
                <% if(Model!=null)
                   { %>
                <td><%=Model.Message %></td>
                <%} %>
            </tr>
        </table>
        

    </div>
    <%} %>
</body>
</html>
