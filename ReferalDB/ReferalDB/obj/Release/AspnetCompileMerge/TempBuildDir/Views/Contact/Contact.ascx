<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ReferalDB.Models.ContactModel>" %>
<%--<script src="../../Scripts/jquery-1.8.2.js" type="text/javascript"></script>--%>
<%--<script src="../../Scripts/jquery-ui-1.8.24.js" type="text/javascript"></script>--%>
<script src="../../Scripts/jquery.validationEngine.js" type="text/javascript"></script>
<script src="../../Scripts/jquery.validationEngine-en.js" type="text/javascript"></script>
<script src="../Scripts/jquery.mask.js" type="text/javascript"></script>
<style type="text/css">
    /*input[type="button"] {
        background-color: #f0f0f0 !important;
        border-top-left-radius: 3px !important;
        border-top-right-radius: 3px !important;
        border-top: 1px solid #808080 !important;
        border-left: 1px solid #808080 !important;
        border-right: 1px solid #808080 !important;
        color: black;
        margin-left: 2px !important;
    }*/

    .contactDiv, .contactListDiv {
        float: left;
        width: 100%;
    }
</style>



<script type="text/javascript">
    $(document).ready(function () {

        $('#ResetContact').click(function () {
            $('.contactLog').hide();
            $('.contact').show();
            $('.documentDiv').hide();
            $('.letterTrayDiv').hide();

            $('.contactTab1').load('../Contact/ListContactVendor/');
            $('.contactTab2').load('../Contact/Index');

        });

        GetNameFieldValidate();
        $('.usPhone').mask('(000)000-0000');

        $('.zipcde').blur(function () {
            $('.zipcde').mask('00000');
            var textCont = $(this).val();
            var preText = "";
            if (textCont.length < 5) {
                for (var i = 0; i < (5 - textCont.length) ; i++) {
                    preText = preText + "0";
                }
            }
            $(this).val(preText + textCont);
        });

        //var style;
        //style = document.getElementById('btnHome').style;
        //style.color = "#29ADF9";
        //style.fontSize = 16;
        //style = document.getElementById('contactHome').style;
        //style.display = "block";
        //style = document.getElementById('contactOther').style;
        //style.display = "none";
        //style = document.getElementById('contactWork').style;
        //style.display = "none";
        //style = document.getElementById('btnHome').style;
        //style.borderBottom = "none";
        //style = document.getElementById('btnWork').style;
        //style.color = "black";
        //style = document.getElementById('btnOther').style;
        //style.color = "black";
    });


    function populate(element) {
        //btnHome btnWork btnOther
        var style;
        var item = element.id;
        if (item == 'btnHome') {
            style = document.getElementById('btnHome').style;
            style.color = "#29ADF9";
            style.fontSize = 16;
            style = document.getElementById('contactHome').style;
            style.display = "block";
            style = document.getElementById('contactOther').style;
            style.display = "none";
            style = document.getElementById('contactWork').style;
            style.display = "none";
            style = document.getElementById(item).style;
            style.borderBottom = "none";
            style = document.getElementById('btnWork').style;
            style.color = "black";
            style = document.getElementById('btnOther').style;
            style.color = "black";
        }
        if (item == 'btnWork') {
            style = document.getElementById('btnWork').style;
            style.color = "#29ADF9";
            style.fontSize = 16;
            style = document.getElementById('contactWork').style;
            style.display = "block";
            style = document.getElementById('contactHome').style;
            style.display = "none";
            style = document.getElementById('contactOther').style;
            style.display = "none";
            style = document.getElementById(item).style;
            style.borderBottom = "none";
            style = document.getElementById('btnHome').style;
            style.color = "black";
            style.backgroundColor = "white";
            style = document.getElementById('btnOther').style;
            style.color = "black";

        }
        if (item == 'btnOther') {
            style = document.getElementById('btnOther').style;
            style.color = "#29ADF9";
            style.fontSize = 16;
            style = document.getElementById('contactOther').style;
            style.display = "block";
            style = document.getElementById('contactHome').style;
            style.display = "none";
            style = document.getElementById('contactWork').style;
            style.display = "none";
            style = document.getElementById(item).style;
            style.borderBottom = "none";
            style = document.getElementById('btnWork').style;
            style.color = "black";
            style = document.getElementById('btnHome').style;
            style.color = "black";
        }

    }

    $('#ddlHomeCountry').change(function () {

        var countryId = $('#ddlHomeCountry').val();
        $.getJSON('../ClientRegistration/getStates', { countryid: countryId }, function (result) {
            var ddlState = $('#ddlHomeState');
            $('#ddlHomeState').empty();

            $.each(result, function (index, item) {
                ddlState.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            //  $('#assistenceCityId').find(":selected").removeAttr('selected');
            $('#ddlHomeState>option:eq(0)').attr('selected', true);
        });


    });
    $('#ddlWorkCountry').change(function () {

        var countryId = $('#ddlWorkCountry').val();
        $.getJSON('../ClientRegistration/getStates', { countryid: countryId }, function (result) {
            var ddlState = $('#ddlWorkState');
            $('#ddlWorkState').empty();

            $.each(result, function (index, item) {
                ddlState.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            //  $('#assistenceCityId').find(":selected").removeAttr('selected');
            $('#ddlWorkState>option:eq(0)').attr('selected', true);
        });


    });
    $('#ddlOtherCountry').change(function () {

        var countryId = $('#ddlOtherCountry').val();
        $.getJSON('../ClientRegistration/getStates', { countryid: countryId }, function (result) {
            var ddlState = $('#ddlOtherState');
            $('#ddlOtherState').empty();

            $.each(result, function (index, item) {
                ddlState.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            //  $('#assistenceCityId').find(":selected").removeAttr('selected');
            $('#ddlOtherState>option:eq(0)').attr('selected', true);
        });


    });


    if (($("#LastName").val() != '')) {
        $("#btnUpdateContact").show();
        $("#btnSaveContact").hide();
    }
    else {
        $("#btnUpdateContact").hide();
        $("#btnSaveContact").show();
    }

    $("#btnUpdateContact").on("click", function (event) {


        //$('#LoadQueue').load('/Contact/UpdateContactDetails/');



    });


    function displayDiv() {
        if (($('#ddlPrntRel :selected').text() == "Parent"))
            $('#shwtxt').show();
        else
            $('#shwtxt').hide();

    }


    $('#noActive').attr('disabled', 'disabled');


    function FillB() {
        var txtB = document.getElementById("ddlDocumentTypeTxt");
        var txtA = document.getElementById("txtA");


        txtB.value = txtA.value;

    }
    function fn_contactSuccess(data) {
        $('.contactTab1').load('../Contact/ListContactVendor/');
        $('.contactTab2').load('../Contact/Index');
    }

    jQuery("#ContactForm").validationEngine();
</script>
<style type="text/css">
    /*table tr td {
        width: 1% !important;
    }*/
</style>
<%using (@Ajax.BeginForm("SaveContacts", "Contact", FormMethod.Post, new AjaxOptions { OnSuccess = "fn_contactSuccess" }, new { id = "ContactForm" }))
  { %>

<div id="partialMainArea" style="padding: 5px;">
    <table class="tblStyle" style="width: 100%;">
        <%=@Html.HiddenFor(m=>m.Id,Model.Id) %>
        <tr>
            <td colspan="5"></td>
        </tr>
        <tr>
            <td style="width: 245px;">
                <span class="lblSpan">Prefix</span><br />
                <%=Html.DropDownListFor(m => m.FirstNamePrefix, Model.FirstNamePrefixList, new {  @class="sd", @tabindex="17"})%></td>
            <%-- <td rowspan="3" style="width:50px;"></td>--%>
            <td></td>
            <%-- <td rowspan="3" style="width:20%;"></td>--%>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>

                <span class="lblSpan">First Name</span><span style="color: red;">*</span><br />
                <%=Html.TextBoxFor(m => m.FirstName, Model.FirstName, new {  @class="validate[required] namefield",maxlength=50,ID="txtA" , @tabindex="18"})%>

                <%-- <%=Html.DropDownListFor(m => m.LastNameSuffix, Model.LastNameSuffixList, new {  @class="sd" })%>--%>
            </td>
            <td>
                <span class="lblSpan">Relationship</span><span style="color: red;">*</span><br />
                <%=Html.DropDownListFor(m=>m.Relation,Model.RelationList,new { @class="validate[required]",ID="ddlPrntRel",onChange = "displayDiv()", @tabindex="21"}) %></td>
            <td style="width: 150px;">
                <%if (Model.Id == 0)
                  { %>
                <div id="shwtxt" style="display: none;">

                    <span style="float: left; margin-left: 0px;">
                        <span class="lblSpan">Username</span><span style="color: red; float: left;">*</span><br />
                        <%=Html.TextBoxFor(m => m.UserID,new { @class = "validate[required,custom[checkUsername]]", ID = "ddlDocumentTypeTxt",value=Model.UserID, @tabindex="24" })%></span>

                </div>
                <%}
                  else
                  {
                      if (Model.UserID != "")
                      { %>
                <div id="Div1" style="display: block;">

                    <span class="lblSpan">Username</span><br />
                    <%=Html.LabelFor(m=>m.UserID,Model.UserID, new {  @class="sd",maxlength=50 }) %>
                </div>
                <%}
                  } %>
            </td>

        </tr>
        <tr>
            <%--<%if (Model.ContactFlag == "Referral")
              { %>
            <td>
                <span class="lblSpan">Relation</span><span style="color: red;">*</span><br/>
                <%=Html.DropDownListFor(m=>m.Relation,Model.RelationList,new { @class="validate[required] inactive",ID="noActive"}) %></td>
            <%}
              else
              { %>--%>
            <td>
                <span class="lblSpan">Middle Name</span><br />
                <%=Html.TextBoxFor(m => m.MiddleName, Model.MiddleName, new {  @class="sd namefield",maxlength=50, @tabindex="19" })%></td>
            <%--<%} %>--%>
            <td>
                <span class="lblSpan">Primary Language</span><br />
                <%=Html.TextBoxFor(m => m.PrimaryLanguage, Model.PrimaryLanguage, new {  @class="sd",maxlength=50 , @tabindex="22"})%></td>
            <td style="width: 150px;"></td>
        </tr>
        <tr>
            <td>


                <span class="lblSpan">Last Name</span><span style="color: red;">*</span><br />
                <%=Html.TextBoxFor(m => m.LastName, Model.LastName, new {  @class="validate[required] namefield",maxlength=50, @tabindex="20" })%>
            </td>
            <td>
                <span class="lblSpan">Relationship Description</span><br />
                <%=Html.TextBoxFor(m => m.Spouse, Model.Spouse, new {  @class="sd",maxlength=50 , @tabindex="23"})%>
            </td>
            <td style="width: 150px;"></td>
        </tr>
        <tr>
            <td></td>

            <td style="width: 100px;"></td>

        </tr>
        <tr>
            <td></td>
            <td></td>
            <td><%--Contact For--%></td>
            <td><%foreach (var item in Model.checkbox)
                  {%>
                <div style="float: left; display: none"><%=@Html.CheckBox("getcheked", item.check, new {value=item.name }) %> <%=item.name %></div>
                <%}%></td>

        </tr>
        <tr>
            <td colspan="6">
                <hr />
                <div onclick="contactBox();" style="cursor: pointer; display: none;">More...</div>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <div id="ContactSection">
                    <div id="Cntactmenu" class="contactListDiv">

                        <%--<table style="margin-bottom: -2px" width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="nomarg">
                                    <input id="btnHome" type="button" value="Home" onclick="populate(this);" style="border-top: 1px solid rgb(128, 128, 128) ! important; border-left: 1px solid rgb(128, 128, 128) ! important; border-right: 1px solid rgb(128, 128, 128) ! important; margin: 2px; background-color: rgb(240, 240, 240) ! important; color: green;" /></td>
                                <td class="nomarg">
                                    <input id="btnWork" type="button" value="Work" onclick="populate(this);" style="border-top: 1px solid rgb(128, 128, 128) ! important; border-left: 1px solid rgb(128, 128, 128) ! important; border-right: 1px solid rgb(128, 128, 128) ! important; margin: 2px; background-color: rgb(240, 240, 240) ! important;" /></td>
                                <td class="nomarg">
                                    <input id="btnOther" type="button" value="Other" onclick="populate(this);" style="border-top: 1px solid rgb(128, 128, 128) ! important; border-left: 1px solid rgb(128, 128, 128) ! important; border-right: 1px solid rgb(128, 128, 128) ! important; margin: 2px; background-color: rgb(240, 240, 240) ! important;" /></td>

                            </tr>
                        </table>--%>

                        <ul class="tabListCnt">
                            <li class="li_Home" style="background-color: #29adf9" onclick="tablistCnt('home')">Home</li>
                            <li class="li_work" onclick="tablistCnt('work')">Work</li>
                            <li class="li_other" onclick="tablistCnt('other')">Other</li>
                        </ul>

                    </div>

                    <div id="contactHome" class="contactDiv">

                        <table style="border: 1px solid #F0F0F0; background: none repeat scroll 0 0 #F0F0F0;" width="100%" cellpadding="0" cellspacing="0" style="background: #F0F0F0">
                            <tr>
                                <td>
                                    <div class="contactHeading">Address</div>
                                </td>
                                <td rowspan="8" style="width: 20px"></td>
                                <td>
                                    <div class="contactHeading">Phone</div>
                                </td>
                                <td rowspan="8" style="width: 50px"></td>
                                <td>
                                    <div style="width: 142px;"></div>
                                </td>
                                <td style="width: 100px;"></td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="lblSpan">Address Type</span><br />
                                    <%=Html.DropDownListFor(m=>m.HomeAddressTypeId,Model.HomeAddressTypeList,new {  ID="ddlAddressListHome", @tabindex="25"}) %></td>
                                <td>
                                    <span class="lblSpan">Primary Contact</span><br />
                                    <%=Html.TextBoxFor(m => m.HomePhone, Model.HomePhone, new {  @class="validate[custom[usPhoneNumber]] usPhone", @tabindex="33" })%></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="lblSpan">Street</span><br />
                                    <%=Html.TextBoxFor(m => m.HomeAddressLine2, Model.HomeAddressLine2, new {  @class="sd",maxlength=50 , @tabindex="26"})%></td>
                                <td>
                                    <span class="lblSpan">Mobile</span><br />
                                    <%=Html.TextBoxFor(m => m.HomeMobilePhone, Model.HomeMobilePhone, new {  @class="validate[custom[usPhoneNumber]] usPhone", @tabindex="34" })%>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="lblSpan">Apartment/Unit</span><br />
                                    <%=Html.TextBoxFor(m => m.HomeAddressLine1, Model.HomeAddressLine1, new { maxlength=50 , @tabindex="27"})%>

                                </td>
                                <td>
                                    <span class="lblSpan">Fax</span><br />
                                    <%=Html.TextBoxFor(m=>m.HomeFax,Model.HomeFax, new { @class="validate[custom[usPhoneNumber]] usPhone", @tabindex="35"})%></td>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">City</span><br />
                <%=Html.TextBoxFor(m => m.HomeCity, Model.HomeCity, new {  maxlength=50, @tabindex="28" })%></td>

            <td>
                <span class="lblSpan">Other</span><br />
                <%=Html.TextBoxFor(m => m.HomeWorkPhone, Model.HomeWorkPhone, new {  @class="validate[custom[usPhoneNumber]] usPhone", @tabindex="36" })%>
            <td>&nbsp;</td>

        </tr>
        <tr>
            <td>
                <span class="lblSpan">State</span><br />
                <%=Html.DropDownListFor(m => m.HomeState, Model.HomeStateList, new {  @class="sd", ID="ddlHomeState", @tabindex="29"})%></td>
            <td>

                <div class="contactHeading">Email</div>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">Zip</span><br />
                <%=Html.TextBoxFor(m => m.HomeZip, Model.HomeZip, new {  @class="sd zipcde",maxlength=5, @tabindex="30" })%></td>
            <td>
                <span class="lblSpan">Home</span><br />
                <%=Html.TextBoxFor(m => m.HomeEmail, Model.HomeEmail, new {  @class="validate[custom[email]]", @tabindex="37" })%>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><span class="lblSpan">County</span><br />
                <%=Html.TextBoxFor(m=>m.HomeCounty,Model.HomeCounty,new { maxlength="50", @tabindex="31"}) %></td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">Country</span><br />
                <%=Html.DropDownListFor(m=>m.HomeCountry,Model.HomeCountryList,new { @class="sd", ID="ddlHomeCountry", @tabindex="32"}) %></td>
        </tr>

    </table>

</div>
<div id="contactWork" class="contactDiv" style="display: none;">
    <table style="border: 1px solid #F0F0F0; background: none repeat scroll 0 0 #F0F0F0;" width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <div class="contactHeading">Address</div>
            </td>
            <td rowspan="8" style="width: 20px"></td>
            <td>
                <div class="contactHeading">Phone</div>
            </td>
            <td rowspan="8" style="width: 50px"></td>
            <td>
                <div style="width: 142px;"></div>
            </td>
            <td style="width: 100px;"></td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">Address Type</span><br />
                <%=Html.DropDownListFor(m=>m.WorkAddressTypeId,Model.WorkAddressTypeList,new { @class="sd", ID="ddlAddressListWork", @tabindex="38"}) %></td>
            <td>
                <span class="lblSpan">Primary Contact</span><br />
                <%=Html.TextBoxFor(m => m.WorkHomePhone, Model.WorkHomePhone, new {  @class="validate[custom[usPhoneNumber]] usPhone" , @tabindex="46"})%></td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">Address 1</span><br />
                <%=Html.TextBoxFor(m => m.WorkAddressLine2, Model.WorkAddressLine2, new {  @class="sd",maxlength=50, @tabindex="39" })%></td>
            <td>
                <span class="lblSpan">Mobile</span><br />
                <%=Html.TextBoxFor(m => m.WorkMobilePhone, Model.WorkMobilePhone, new {  @class="validate[custom[usPhoneNumber]] usPhone", @tabindex="47" })%>
            </td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">Address 2</span><br />
                <%=Html.TextBoxFor(m => m.WorkAddressLine1, Model.WorkAddressLine1, new {  maxlength=50 , @tabindex="40"})%></td>
            <td>
                <span class="lblSpan">Fax</span><br />
                <%=Html.TextBoxFor(m => m.WorkFax, Model.WorkFax, new {@class="validate[custom[usPhoneNumber]] usPhone", @tabindex="48" })%>
            </td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">City</span><br />
                <%=Html.TextBoxFor(m => m.WorkCity, Model.WorkCity, new {  @class="sd",maxlength=50, @tabindex="41" })%></td>
            <td>
                <span class="lblSpan">Other</span><br />
                <%=Html.TextBoxFor(m => m.WorkPhone, Model.WorkPhone, new {  @class="validate[custom[usPhoneNumber]] usPhone", @tabindex="49" })%>
            </td>

        </tr>
        <tr>
            <td>
                <span class="lblSpan">State</span><br />
                <%=Html.DropDownListFor(m => m.WorkState, Model.WorkStateList, new {  @class="sd",  ID="ddlWorkState", @tabindex="42"})%></td>
            <td>
                <div class="contactHeading">Email</div>
            </td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">Zip</span><br />
                <%=Html.TextBoxFor(m => m.WorkZip, Model.WorkZip, new {  @class="sd zipcde",maxlength=5 , @tabindex="43"})%></td>
            <td>
                <span class="lblSpan">Work</span><br />
                <%=Html.TextBoxFor(m => m.WorkEmail, Model.WorkEmail, new {  @class="validate[custom[email]]", @tabindex="50" })%>
            </td>
        </tr>
        <tr>
            <td><span class="lblSpan">County</span><br />
                <%=Html.TextBoxFor(m=>m.WorkCounty,Model.WorkCounty,new { maxlength="50", @tabindex="44"}) %></td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">Country</span><br />
                <%=Html.DropDownListFor(m=>m.WorkCountry,Model.WorkCountryList,new { @class="sd", ID="ddlWorkCountry", @tabindex="45"}) %></td>
        </tr>

    </table>
</div>
<div id="contactOther" class="contactDiv" style="display: none;">
    <table style="border: 1px solid #F0F0F0; background: none repeat scroll 0 0 #F0F0F0;" width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <div class="contactHeading">Address</div>
            </td>
            <td rowspan="8" style="width: 20px"></td>
            <td>
                <div class="contactHeading">Phone</div>
            </td>
            <td rowspan="8" style="width: 50px"></td>
            <td>
                <div style="width: 142px;"></div>
            </td>
            <td style="width: 100px;"></td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">Address Type</span><br />
                <%=Html.DropDownListFor(m=>m.OtherAddressTypeId,Model.OtherAddressTypeList,new { @class="sd", ID="ddlAddressOther", @tabindex="51"}) %></td>
            <td>
                <span class="lblSpan">Primary Contact</span><br />
                <%=Html.TextBoxFor(m => m.OtherHomePhone, Model.OtherHomePhone, new {  @class="validate[custom[usPhoneNumber]] usPhone", @tabindex="59" })%></td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">Address 1</span><br />
                <%=Html.TextBoxFor(m => m.OtherAddressLine2, Model.OtherAddressLine2, new {  @class="sd",maxlength=50 , @tabindex="52"})%></td>
            <td>
                <span class="lblSpan">Mobile</span><br />
                <%=Html.TextBoxFor(m => m.OtherMobilePhone, Model.OtherMobilePhone, new {  @class="validate[custom[usPhoneNumber]] usPhone", @tabindex="60" })%>
            </td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">Address 2</span><br />
                <%=Html.TextBoxFor(m => m.OtherAddressLine1, Model.OtherAddressLine1, new {  maxlength=50 , @tabindex="53"})%></td>
            <td>
                <span class="lblSpan">Fax</span><br />
                <%=Html.TextBoxFor(m => m.OtherFax, Model.OtherFax, new { @class="validate[custom[usPhoneNumber]] usPhone", @tabindex="61"})%>
            </td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">City</span><br />
                <%=Html.TextBoxFor(m => m.OtherCity, Model.OtherCity, new {  @class="sd",maxlength=50, @tabindex="54" })%></td>
            <td>
                <span class="lblSpan">Other</span><br />
                <%=Html.TextBoxFor(m => m.OtherWorkPhone, Model.OtherWorkPhone, new {  @class="validate[custom[usPhoneNumber]] usPhone", @tabindex="62" })%>
            </td>

        </tr>
        <tr>
            <td>
                <span class="lblSpan">State</span><br />
                <%=Html.DropDownListFor(m => m.OtherState, Model.OtherStateList, new {  @class="sd",  ID="ddlOtherState", @tabindex="55"})%></td>
            <td>
                <%-- <span class="lblSpan">Home</span><br />
                                    <%=Html.TextBoxFor(m => m.OtherHomeEmail, Model.OtherHomeEmail, new {  @class="validate[custom[email]]", @tabindex="59" })%>--%>

                <%--<div class="contactHeading">Email</div>--%>
            </td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">Zip</span><br />
                <%=Html.TextBoxFor(m => m.OtherZip, Model.OtherZip, new {  @class="sd zipcde",maxlength=5 , @tabindex="56"})%></td>
            <td>
                <%-- <span class="lblSpan">Home</span><br />
                                    <%=Html.TextBoxFor(m => m.OtherHomeEmail, Model.OtherHomeEmail, new {  @class="validate[custom[email]]", @tabindex="60" })%>--%>
            </td>
        </tr>
        <tr>
            <td><span class="lblSpan">County</span><br />
                <%=Html.TextBoxFor(m=>m.OtherCounty,Model.OtherCounty,new { maxlength="50", @tabindex="57"}) %></td>
        </tr>
        <tr>
            <td>
                <span class="lblSpan">Country</span><br />
                <%=Html.DropDownListFor(m=>m.OtherCountry,Model.OtherCountryList,new { @class="sd", ID="ddlOtherCountry", @tabindex="58"}) %></td>
        </tr>

    </table>
</div>
</div>
            </td>
        </tr>
        <tr>

            <td colspan="5">
                <% if (ViewBag.permission == "true")
                   { %>
                <input id="btnSaveContact" type="submit" value="Save" onclick="" />
                <input id="ResetContact" type="button" value="Reset" />
                <input id="btnUpdateContact" type="submit" value="Update" onclick="" style="display: none" />
                <%} %>
            </td>


        </tr>
</table>
</div>
<%} %>
<script type="text/javascript">
    $(document).ready(function () {

        $('.usPhone').mask('(000)000-0000');

        staffNameAutocomplete();

        $('.moreContact').click(function () {
            // $('#ContactSection').slideUp();
        });

        GetNameFieldValidate();

    });

    function tablistCnt(type) {

        $('.contactDiv').hide();
        $('.tabListCnt li').css('background-color', '#03507d');
        switch (type) {
            case "home":
                $('.li_Home').css('background-color', '#29adf9');
                $('#contactHome').show();
                break;
            case "work":
                $('.li_work').css('background-color', '#29adf9');
                $('#contactWork').show();
                break;
            case "other":
                $('.li_other').css('background-color', '#29adf9');
                $('#contactOther').show();
                break;

        }

    }

    function GetNameFieldValidate() {
        $('.namefield').keypress(function (event) {
            var inputValue = event.which;
            if (((inputValue >= 65 && inputValue <= 90) || (inputValue >= 97 && inputValue <= 122) || (inputValue == 32) || (inputValue == 39) || (inputValue == 45) || (inputValue == 8) || (inputValue == 0))) {
            }
            else {
                event.preventDefault();
            }
        });
    }

    function contactBox() {
        $('#ContactSection').slideToggle();
    }

    function staffNameAutocomplete() {
        $("#ddlDocumentTypeTxt").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "../Contact/searchParentFirstName",
                    type: "POST",
                    dataType: "json",
                    data: { term: request.term },
                    success: function (data) {
                        if ($(data).length == 0) {
                            $('#ddlDocumentTypeTxt').css('border-color', 'Green');
                        }
                        else {
                            $('#ddlDocumentTypeTxt').css('border-color', 'Red');
                        }
                    },
                })
            },
            messages: {
                noResults: '',
                results: function (resultsCount) { }
            }
        });
    }

</script>

