<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ReferalDB.Models.HomeDropDataViewModel>" %>

<link href="../../CSS/StyleControl.css" rel="stylesheet" />


<form>
    <div>
        <table style="width: 100%;">
            <tr>
                <td>
                    <div id="tdMsg"></div>
                </td>
            </tr>
        </table>

        <table style="width: 100%;">
            <thead>
                <tr class="HeaderStyle">
                    <td style="width: 20%"><b>Referral Name</b>
                    </td>
                    <td style="width: 20%"><b>User Name</b>
                    </td>
                    <td style="width: 20%"><b>CheckList Name</b>
                    </td>
                    <td style="width: 20%"><b>AssignDate</b>
                    </td>
                </tr>

                <%foreach (var item in Model.CheckDetails)
                  { %>

                <tr>
                    <td><%=item.ReferralName %></td>
                    <td><%=item.UserName %></td>
                    <td><%=item.CheckListName %></td>
                    <td><%=item.AssignDate %></td>

                </tr>


                <%} %>
            </thead>


        </table>

    </div>

</form>
