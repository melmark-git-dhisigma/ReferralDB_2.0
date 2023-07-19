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

        <table style="width: 100%;" class="gridStyle">
            <thead>
                <tr class="HeaderStyle">
                    <td style="width: 20%"><b>ReferralName</b>
                    </td>
                    <td style="width: 20%"><b>Queue Name</b>
                    </td>
                    <td style="width: 20%"><b>CheckListName</b>
                    </td>
                    <td style="width: 20%"><b>AssignDate</b>
                    </td>
                    <td style="width: 20%"><b>Select</b>
                    </td>
                </tr>

                <%if(Model.CheckDetails!=null)
                  {
                    foreach (var item in Model.CheckDetails)
                  { %>

                <tr>
                    <td><%=item.ReferralName %></td>
                    <td><%=item.QueueName %></td>
                    <td><%=item.CheckListName %></td>
                    <td><%=item.AssignDate %></td>
                    <%--<td><a href="#" onclick="SelectReferral(<%="'"+item.QueueId+"','chk'"%>)">Select</a></td>--%>
                    <td>
                        <% var qId=item.QueueId.Split('_');%>
                        <%if (qId[1] != "28")
                          { %>
                        <a href="#" onclick="SelectReferral(<%="'"+item.QueueId+"','chk',this"%>)">Select</a>
                         <%} else { %>
                       <a href="#" onclick="
                           <%="'"+qId[1]+",Inactive List'"%>)">Select</a>
                       
                        <%} %>
                    </td>
                </tr>


                <%} 
                  }%>
            </thead>


        </table>

    </div>

</form>
