<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ReferalDB.Models.ListContactModel>" %>
<style type="text/css">
    .pager span, .pager a {
        padding: 2px;
        border: 1px solid gray;
        float: left;
        margin: 3px;
    }
</style>
<script type="text/javascript">
    $("#tblContacList tr:odd").css("background-color", "#F3F3F3");
    //$('#imagePanel').load('/Contact/ImageUploadPanel');

    function loadFunction(id, type) {

        if (type == "Edit") {
            $('.contactTab2').load('../Contact/fillContactDetails/' + id);
        }
        if (type == "Delete") {
            var confirmDel = confirm("Are you sure want to delete?");
            if (confirmDel == true) {
            $.get("../Contact/DeleteContactDetails?id=" + id, function (Result) {
                if (Result == "Success") {
                    $('.contactTab1').load('../Contact/ListContactVendor/');
                    $('.contactTab2').load('../Contact/Index');
                }
            });
        }
        }
    }
    $('#btnAddContact').on("click", function (event) {
        $('#LoadQueue').load('../Contact/Index');
    });
</script>
<div id="partialMainArea">
    <% if (TempData["notice"] != null)
       { %>
    <p style="width:100%;color:red;font-size:14px;font-weight:bold"><%= Html.Encode(TempData["notice"]) %></p>
    <% } %>
    <div style="width: 30%; float: left; display: none">
        <select id="ddlContact" name="ddlContact" style="width: 150px" onchange="ListContacts()">
            <option>-Select-</option>
            <option>Contact</option>
            <option>Vendor</option>
        </select>
    </div>

    <div style="width: 30%; float: left; display: none">
        <%-- <%=Html.TextBoxFor(m => m.Searchtext, Model.Searchtext, new {  @class="validate[required] " })%>--%>
        <input id="btnSearch" type="button" value="Search" onclick="" />
    </div>
    <% if (Model.listContacts.Count!=0) {%>
    <div style="float: left; width: 100%; max-height:200px; overflow-y:auto;">
        <table id="tblContacList" style="width: 100%">
            <tr class="HeaderStyle">
                <th class="tdLabel">Name</th>
                <th class="tdLabel">Relationship</th>
                <th class="tdLabel">Relationship Description</th>
                <th class="tdLabel" style="width:40px;">Edit</th>
                <th class="tdLabel" style="width:40px;">Delete</th>
            </tr>
            <%foreach (var item in Model.listContacts)
              {
            %>
            <tr class="RowStyle">
                <td><%= item.Name %></td>
                <td><%= item.Relation %></td>
                <td><%= item.RelationDesc %></td>
                <td>
                    <img src="../Images/editicon.PNG" style="cursor:pointer" onclick="loadFunction(<%= item.ContactId %>,'Edit'); " /></td>
                <td>
                   <%if(ViewBag.permission == "true"){ %>
                 
                    <img src="../Images/trash.PNG" style="cursor:pointer; background-color:black;" onclick="loadFunction(<%= item.ContactId %>,'Delete');" /></td>
              <%} %>
            </tr>
            <%
              } %>
        </table>
        <%--<div class=""><% Html.RenderPartial("Pager", Model.pageModel); %></div>--%>
        <div style="width: 5%; float: right; margin-top: 30px;">
            <%--<input id="btnAddContact" type="button" value="Add Contact" onclick="" />--%>
        </div>
    </div>
    <% } %>

</div>
<%--<div id="imagePanel" style="border: 1px solid #CCCCCC; float: left; margin-left: 5px; margin-right: 20px; margin-top: 20px; width: 18%;">
</div>--%>


