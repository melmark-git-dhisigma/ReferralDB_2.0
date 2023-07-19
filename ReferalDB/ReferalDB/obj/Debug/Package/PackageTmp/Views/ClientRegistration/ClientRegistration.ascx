<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ReferalDB.Models.RegistrationModel>" %>
<script src="../../Documents/JS/jquery-1.8.2.js"></script>
<link href="../../Documents/CSS/jquery-ui.css" rel="stylesheet" />
<script src="../../Documents/JS/jquery-ui-1.11.2.js"></script>

<script src="../../Documents/JS/jquery.validationEngine-en.js"></script>
<script src="../../Documents/JS/jquery.validationEngine.js"></script>
<script src="../../Documents/JS/jquery.form.js"></script>
<style type="text/css">
   
    #rightSidePanel {
        /*margin-top: -2548px !important;*/
    }

    p.MColumnHead {
        margin-bottom: .0001pt;
        page-break-after: avoid;
        punctuation-wrap: simple;
        text-autospace: none;
        font-size: 10.0pt;
        font-family: "Arial","sans-serif";
        font-weight: bold;
        margin-left: 0cm;
        margin-right: 0cm;
        margin-top: 0cm;
    }

    p.MTableText {
        margin: 3.0pt 0cm;
        punctuation-wrap: simple;
        text-autospace: none;
        font-size: 9.0pt;
        font-family: "Times New Roman","serif";
    }
</style>
<script type="text/javascript">




    $(document).ready(function () {
        $('.imgcontainer').css("display", "none");
        // $(".datepicker").datepicker();admissionDate 
        //$(".datepicker").datepicker({
        //    changeMonth: true,
        //    changeYear: true,
        //    showAnim: "fadeIn",
        //    yearRange: 'c-30:c+30',


        //    /* fix buggy IE focus functionality */
        //    fixFocusIE: false,

        //    /* blur needed to correctly handle placeholder text */
        //    onSelect: function (dateText, inst) {
        //        this.fixFocusIE = true;
        //        $(this).blur().change().focus();
        //    },
        //    onClose: function (dateText, inst) {
        //        this.fixFocusIE = true;
        //        this.focus();
        //    },
        //    beforeShow: function (input, inst) {
        //        var result = $.browser.msie ? !this.fixFocusIE : true;
        //        this.fixFocusIE = false;
        //        return result;
        //    }
        //}).attr('readonly', 'true');

        $("#CurrentIEPStartDate").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true,
        });

        $("#CurrentIEPExpirationDate").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true,
        });

        $("#DischargeDate").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true,
        });

        $("#DateInitiallyEligibleforSpecialEducation").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true,
        });

        $("#DateofMostRecentSpecialEducationEvaluations").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true,
        });

        $("#DateofNextScheduled3YearEvaluation").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true,
        });

        $("#admissionDate").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true,

            /* blur needed to correctly handle placeholder text */
            onSelect: function (dateText, inst) {
                this.fixFocusIE = true;
                $(this).blur().change().focus();
                $(this).blur().change().focus();



            },
            onClose: function (dateText, inst) {
                this.fixFocusIE = true;
                this.focus();
            },
            beforeShow: function (input, inst) {
                var result = $.browser.msie ? !this.fixFocusIE : true;
                this.fixFocusIE = false;
                return result;
            }
        }).attr('readonly', 'true');

        $("#dateOfBirth").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true,

            /* blur needed to correctly handle placeholder text */
            onSelect: function (dateText, inst) {
                this.fixFocusIE = true;
                $(this).blur().change().focus();
                var dt1 = $('#admissionDate');
                var startDate = $(this).datepicker('getDate');
                dt1.datepicker('option', 'minDate', startDate);


            },
            onClose: function (dateText, inst) {
                this.fixFocusIE = true;
                this.focus();
            },
            beforeShow: function (input, inst) {
                var result = $.browser.msie ? !this.fixFocusIE : true;
                this.fixFocusIE = false;
                return result;
            }
        }).attr('readonly', 'true');


        $("#DateFrom1").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            /* fix buggy IE focus functionality */
            fixFocusIE: false,
            /* blur needed to correctly handle placeholder text */
            onClose: function (dateText, inst) {
                this.fixFocusIE = true;
                this.focus();
            },
            beforeShow: function (input, inst) {
                var result = $.browser.msie ? !this.fixFocusIE : true;
                this.fixFocusIE = false;
                return result;
            },
            onSelect: function (dateText, inst) {
                this.fixFocusIE = true;
                $(this).blur().change().focus();
                var dt2 = $('#DateTo1');
                var startDate = $(this).datepicker('getDate');
                startDate.setDate(startDate.getDate());
                var minDate = $(this).datepicker('getDate');
                dt2.datepicker('option', 'minDate', minDate);

            }
        }).attr('readonly', 'true');
        $('#DateTo1').datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',


            /* fix buggy IE focus functionality */
            fixFocusIE: false,

            /* blur needed to correctly handle placeholder text */
            onSelect: function (dateText, inst) {
                this.fixFocusIE = true;
                $(this).blur().change().focus();
                var dt1 = $('#DateFrom1');
                var startDate = $(this).datepicker('getDate');
                dt1.datepicker('option', 'maxDate', startDate);
            },
            onClose: function (dateText, inst) {
                this.fixFocusIE = true;
                this.focus();
            },
            beforeShow: function (input, inst) {
                var result = $.browser.msie ? !this.fixFocusIE : true;
                this.fixFocusIE = false;
                return result;
            }
        }).attr('readonly', 'true');

        $(document).ready(function () {
            $("#DateFrom2").datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: "fadeIn",
                yearRange: 'c-30:c+30',
                /* fix buggy IE focus functionality */
                fixFocusIE: false,
                /* blur needed to correctly handle placeholder text */
                onClose: function (dateText, inst) {
                    this.fixFocusIE = true;
                    this.focus();
                },
                beforeShow: function (input, inst) {
                    var result = $.browser.msie ? !this.fixFocusIE : true;
                    this.fixFocusIE = false;
                    return result;
                },
                onSelect: function (dateText, inst) {
                    this.fixFocusIE = true;
                    $(this).blur().change().focus();
                    var dt2 = $('#DateTo2');
                    var startDate = $(this).datepicker('getDate');
                    startDate.setDate(startDate.getDate());
                    var minDate = $(this).datepicker('getDate');
                    dt2.datepicker('option', 'minDate', minDate);

                }
            }).attr('readonly', 'true');
            $('#DateTo2').datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: "fadeIn",
                yearRange: 'c-30:c+30',


                /* fix buggy IE focus functionality */
                fixFocusIE: false,

                /* blur needed to correctly handle placeholder text */
                onSelect: function (dateText, inst) {
                    this.fixFocusIE = true;
                    $(this).blur().change().focus();
                    var dt1 = $('#DateFrom2');
                    var startDate = $(this).datepicker('getDate');
                    dt1.datepicker('option', 'maxDate', startDate);
                },
                onClose: function (dateText, inst) {
                    this.fixFocusIE = true;
                    this.focus();
                },
                beforeShow: function (input, inst) {
                    var result = $.browser.msie ? !this.fixFocusIE : true;
                    this.fixFocusIE = false;
                    return result;
                }
            }).attr('readonly', 'true');
        });
        $(document).ready(function () {
            $("#DateFrom3").datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: "fadeIn",
                yearRange: 'c-30:c+30',
                /* fix buggy IE focus functionality */
                fixFocusIE: false,
                /* blur needed to correctly handle placeholder text */
                onClose: function (dateText, inst) {
                    this.fixFocusIE = true;
                    this.focus();
                },
                beforeShow: function (input, inst) {
                    var result = $.browser.msie ? !this.fixFocusIE : true;
                    this.fixFocusIE = false;
                    return result;
                },
                onSelect: function (dateText, inst) {
                    this.fixFocusIE = true;
                    $(this).blur().change().focus();
                    var dt2 = $('#DateTo3');
                    var startDate = $(this).datepicker('getDate');
                    startDate.setDate(startDate.getDate());
                    var minDate = $(this).datepicker('getDate');
                    dt2.datepicker('option', 'minDate', minDate);

                }
            }).attr('readonly', 'true');
            $('#DateTo3').datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: "fadeIn",
                yearRange: 'c-30:c+30',


                /* fix buggy IE focus functionality */
                fixFocusIE: false,

                /* blur needed to correctly handle placeholder text */
                onSelect: function (dateText, inst) {
                    this.fixFocusIE = true;
                    $(this).blur().change().focus();
                    var dt1 = $('#DateFrom3');
                    var startDate = $(this).datepicker('getDate');
                    dt1.datepicker('option', 'maxDate', startDate);
                },
                onClose: function (dateText, inst) {
                    this.fixFocusIE = true;
                    this.focus();
                },
                beforeShow: function (input, inst) {
                    var result = $.browser.msie ? !this.fixFocusIE : true;
                    this.fixFocusIE = false;
                    return result;
                }
            }).attr('readonly', 'true');
        });
    });
    // var regex = /^[a-zA-Z ]*$/;
    //HairColor
    $(function () {
        $('#EyeColor').keyup(function (e) {
            var textValue = $(this).val().trim();
            var englishValue = /[a-zA-Z\ ]/;
            for (var i = 0; i < textValue.length; i++) {

                if (!englishValue.test(textValue[i])) {
                    return false;
                }
            }
            return true;
        });
    });


    jQuery('.charectorsOnly').keypress(function (event) {

        var textValue = (event.which) ? event.which : event.keyCode;
        var number = /[a-zA-Z ]*/;
        var value = $(this).val();
        var valueSplit = value.split('.');

        if (textValue == 8 || textValue == 9 || textValue == 45 || textValue == 46 || number.test(String.fromCharCode(textValue))) {
            if (valueSplit.length > 1) {
                if (textValue == 46) {
                    return false;
                }

                if (value.length > 50) {
                    return false;
                }

            }
            else {
                if (textValue != 46 && textValue != 8) {
                    if (value.length > 2) {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    });

    jQuery('.numbersOnly').keypress(function (event) {

        var textValue = (event.which) ? event.which : event.keyCode;
        var number = /[0-9]/;
        var value = $(this).val();
        var valueSplit = value.split('.');

        if (textValue == 8 || textValue == 9 || textValue == 45 || textValue == 46 || number.test(String.fromCharCode(textValue))) {
            if (valueSplit.length > 1) {
                if (textValue == 46) {
                    return false;
                }

                if (value.length > 5) {
                    return false;
                }

            }
            else {
                if (textValue != 46 && textValue != 8) {
                    if (value.length > 2) {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    });

    jQuery('.phoneNumbers').keypress(function (event) {

        var textValue = (event.which) ? event.which : event.keyCode;
        var number = "^(\([0-9]{3}\) |[0-9]{3}-)[0-9]{3}-[0-9]{4}";
        var value = $(this).val();
        var valueSplit = value.split('.');

        if (textValue == 8 || textValue == 9 || textValue == 45 || textValue == 46 || number.test(String.fromCharCode(textValue))) {


            if (valueSplit.length > 1) {
                if (textValue == 46) {
                    return false;
                }

                if (value.length > 5) {
                    return false;
                }

            }
            else {
                if (textValue != 46) {
                    if (value.length > 2) {
                        return false;
                    }

                }


            }



            return true;


        }


        return false;
    });
    //^(\([0-9]{3}\) |[0-9]{3}-)[0-9]{3}-[0-9]{4}$


    $('#ddlCountryOfBirth').change(function () {

        var countryId = $('#ddlCountryOfBirth').val();
        $.getJSON('../ClientRegistration/getStates', { countryid: countryId }, function (result) {
            var ddlState = $('#ddlStateOfBirth');
            $('#ddlStateOfBirth').empty();

            $.each(result, function (index, item) {
                ddlState.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            //  $('#assistenceCityId').find(":selected").removeAttr('selected');
            $('#ddlStateOfBirth>option:eq(0)').attr('selected', true);
        });


    });





    $('#ddlStateOfBirth').change(function () {

    });
    $('#ddlCountry').change(function () {
        var countryId = $('#ddlCountry').val();
        $.getJSON('../ClientRegistration/getStates', { countryid: countryId }, function (result) {
            var ddlState = $('#ddlState');
            $('#ddlState').empty();

            $.each(result, function (index, item) {
                ddlState.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            //  $('#assistenceCityId').find(":selected").removeAttr('selected');
            $('#ddlState>option:eq(0)').attr('selected', true);
        });
    });
    $('#ddlState').change(function () {

    });
    $('#ddlZip').change(function () {

    });
    $('#ddlGender').change(function () {

    });


    function Redirect() {
        alert("Data Saved Sucessfully");
        window.location.href = "../Client/ListClients?argument=*&bSort=false";

    }

    jQuery("#registrationForm").validationEngine();

    //$("#btnList").on("click", function (event) {
    //    //event.preventDefault();

    //    //if (jQuery("#registrationForm").validationEngine('validate')) {
    //    $('#content').load('/ClientRegistration/fillCLientDetails/');


    //    //}
    //});
    var options = {
        success: showResponse  // post-submit callback 
    };
    $('#registrationForm').ajaxForm(options);

    function showResponse(responseText, statusText, xhr, $form) {
        var item = responseText.split('|');
        if (item[0] == "Sucess") {
            $('#LoadQueue').load('../ClientRegistration/ClientRegistration?data=' + item[1] + '|Fill');
            $('.imgcontainer').css("display", "block");
            $('.imgcontainer').load('../Contact/ImageUploadPanel');
            $('.EditProfile').css("display", "block");
        }
        else {

        }
    }

</script>

<%--<%using (@Ajax.BeginForm("SaveClients", "ClientRegistration", FormMethod.Post, new AjaxOptions { UpdateTargetId = "content", OnSuccess = "Redirect" }, new { id = "registrationForm", enctype = "multipart/form-data" }))
  { %>--%>

<%using (@Html.BeginForm("SaveClients", "ClientRegistration", FormMethod.Post, new { id = "registrationForm", enctype = "multipart/form-data" }))
  { %>

<div style="width: 100%">
    <div>
        <table>
            <tr>
                <td class="">First Name <span style="color: red">*</span></td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.FirstName, Model.FirstName, new {  @class="validate[required] ",maxlength=50 })%></td>
                <td class="auto-style3">Nick Name</td>
                <td class="nobdr">
                    <%=Html.TextBoxFor(m => m.NickName, Model.NickName, new {  @class="sd",maxlength=50})%></td>
            </tr>
            <tr>
                <td class="">Middle Name</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.MiddleName, Model.MiddleName, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Last Name <span style="color: red">*</span></td>
                <td class="nobdr">
                    <%=Html.TextBoxFor(m => m.LastName, Model.LastName, new {  @class="validate[required]",maxlength=50 })%>
                    <%=Html.DropDownListFor(m => m.LastNameSuffix, Model.LastNameSuffixList, new {  @class="sd" })%>
                </td>
            </tr>
            <tr>
                <td class="">Date of Birth <span style="color: red">*</span></td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.DateOfBirth, Model.DateOfBirth, new { @class = "validate[required] datepicker", ID = "dateOfBirth" })%>

                <td class="auto-style3">Gender</td>
                <td class="nobdr">
                    <%=Html.DropDownListFor(m => m.Gender, Model.GenderList, new {  @class="sd",ID = "ddlGender"  })%></td>
            </tr>
            <tr>
                <td class="">Admission Date <span style="color: red">*</span></td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.AdmissinDate, Model.AdmissinDate, new {  @class=" validate[required] datepicker",ID = "admissionDate" })%>
                <td class="auto-style3">Race</td>
                <td class="nobdr">
                    <%=Html.DropDownListFor(m => m.Race, Model.RaceList, new {  @class="sd",ID = "ddlRace" })%></td>
            </tr>
            <tr>
                <td class="">Place of Birth</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.PlaceOfBirth, Model.PlaceOfBirth, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Country of Birth</td>
                <td class="nobdr">
                    <%=Html.DropDownListFor(m => m.CountryofBirth, Model.CountryOfBirthList, new {  @class="sd",ID = "ddlCountryOfBirth" })%></td>
            </tr>
            <tr>
                <td class="">State of Birth</td>
                <td class="">
                    <%=Html.DropDownListFor(m => m.StateOfBirth, Model.StateOfBirthList, new {  @class="sd",ID = "ddlStateOfBirth" })%></td>
                <td class="auto-style3">Citizenship</td>
                <td class="nobdr">
                    <%=Html.DropDownListFor(m => m.Citizenship, Model.CitizenshipList, new {  @class="sd",ID = "ddlCityzenShip" })%></td>
            </tr>
            <tr>
                <td class="">Height (ft)</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.Height, Model.Height, new {  @class="numbersOnly",ID="txtHeight" })%></td>
                <td class="auto-style3">Weight (lbs)</td>
                <td class="nobdr">
                    <%=Html.TextBoxFor(m => m.Weight, Model.Weight, new {  @class="numbersOnly", ID="txtWeight" })%></td>
            </tr>
            <tr>
                <td class="">Hair Color</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.HairColor, Model.HairColor, new {  @class="validate[custom[englishOnlywithspaces]]",maxlength=50 })%></td>
                <td class="auto-style3">Eye Color</td>
                <td class="nobdr">
                    <%=Html.TextBoxFor(m => m.EyeColor, Model.EyeColor, new {  @class="validate[custom[englishOnlywithspaces]]",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">Primary Language</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.PrimaryLanguage, Model.PrimaryLanguage, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Legal Competency Status</td>
                <td class="nobdr">
                    <%=Html.TextBoxFor(m => m.LegalCompetencyStatus, Model.LegalCompetencyStatus, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">Guardianship Status</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.GuardianshipStatus, Model.GuardianshipStatus, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Other State Agencies Involved With Student</td>
                <td class="nobdr">
                    <%=Html.TextBoxFor(m => m.OtherStateAgenciesInvolvedWithStudent, Model.OtherStateAgenciesInvolvedWithStudent, new {  @class="sd",maxlength=250 })%></td>
            </tr>
            <tr>
                <td class="">Distigushing Marks</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.DistigushingMarks, Model.DistigushingMarks, new {  @class="sd" })%></td>
                <td class="auto-style3">Marital Status of Both Parents</td>
                <td class="nobdr">
                    <%=Html.TextBoxFor(m => m.MaritalStatusofBothParents, Model.MaritalStatusofBothParents, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">Case Manager Residential</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.CaseManagerResidential, Model.CaseManagerResidential, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Case Manager Educational</td>
                <td class="nobdr">
                    <%=Html.TextBoxFor(m => m.CaseManagerEducational, Model.CaseManagerEducational, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="" colspan="4"><h4>Address Information</h4></td>
            </tr>
            <tr>
                <td class="">Address Line 1 <span style="color: red">*</span></td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.AddressLine1, Model.AddressLine1, new {  @class="validate[required] ",maxlength=50 })%></td>
                <td class="auto-style3">Address Line 2 <span style="color: red">*</span></td>
                <td class="nobdr">
                    <%=Html.TextBoxFor(m => m.AddressLine2, Model.AddressLine2, new {  @class="validate[required] ",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">Address Line 3 <span style="color: red">*</span></td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.AddressLine3, Model.AddressLine3, new {  @class="validate[required] ",maxlength=50 })%></td>
                <td class="auto-style3">Country</td>
                <td class="nobdr">
                    <%=Html.DropDownListFor(m => m.Country, Model.CountryList, new {  @class="sd",ID = "ddlCountry" })%></td>
            </tr>
            <tr>
                <td class="">State</td>
                <td class="">
                    <%=Html.DropDownListFor(m => m.State, Model.StateList, new {  @class="sd",ID = "ddlState" })%></td>
                <td class="auto-style3">City</td>
                <td class="nobdr">
                    <%=Html.TextBoxFor(m => m.City, Model.City, new {  @class="sd" })%></td>
            </tr>
            <tr>
                <td class="">Zip</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.ZipCode, Model.ZipCode, new {  @class="sd",maxlength=5,ID = "" })%></td>
                <td class="auto-style3">&nbsp;</td>
                <td class="nobdr">&nbsp;</td>
            </tr>
            <tr>
                <td class="" colspan="4"><h4>Emergency Contact - School</h4></td>
            </tr>
            <tr>
                <td class="">First Name</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName1, Model.EmergencyContactFirstName1, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Last Name</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.EmergencyContactLastName1, Model.EmergencyContactLastName1, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">Title</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle1, Model.EmergencyContactTitle1, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Phone</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.EmergencyContactPhone1, Model.EmergencyContactPhone1, new {  @class="validate[custom[usPhoneNumber]]" })%></td>
            </tr>
            <tr>
                <td class="">First Name</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName2, Model.EmergencyContactFirstName2, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Last Name</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.EmergencyContactLastName2, Model.EmergencyContactLastName2, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">Title</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle2, Model.EmergencyContactTitle2, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Phone</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.EmergencyContactPhone2, Model.EmergencyContactPhone2, new {  @class="validate[custom[usPhoneNumber]]" })%></td>
            </tr>
            <tr>
                <td class="">First Name</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName3, Model.EmergencyContactFirstName3, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Last Name</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.EmergencyContactLastName3, Model.EmergencyContactLastName3, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">Title</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle3, Model.EmergencyContactTitle3, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Phone</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.EmergencyContactPhone3, Model.EmergencyContactPhone3, new {  @class="validate[custom[usPhoneNumber]]" })%></td>
            </tr>
            <tr>
                <td class="">First Name</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName4, Model.EmergencyContactFirstName4, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Last Name</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.EmergencyContactLastName4, Model.EmergencyContactLastName4, new {  @class="sd" ,maxlength=50})%></td>
            </tr>
            <tr>
                <td class="">Title</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle4, Model.EmergencyContactTitle4, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Phone</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.EmergencyContactPhone4, Model.EmergencyContactPhone4, new {  @class="validate[custom[usPhoneNumber]]" })%></td>
            </tr>
            <tr>
                <td class="">First Name</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName5, Model.EmergencyContactFirstName5, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Last Name</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.EmergencyContactLastName5, Model.EmergencyContactLastName5, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">Title</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle5, Model.EmergencyContactTitle5, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Phone</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.EmergencyContactPhone5, Model.EmergencyContactPhone5, new {  @class="validate[custom[usPhoneNumber]]" })%></td>
            </tr>
            <tr>
                <td class="" colspan="4"><h4>Schools Attended</h4></td>
            </tr>
            <tr>
                <td class="">School Name</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.SchoolName1, Model.SchoolName1, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Dates Attended<br />
                    (MM/dd/yyyy)</td>
                <td class="nobdr">From<br />
                    <%=Html.TextBoxFor(m => m.DateFrom1, Model.DateFrom1, new {  @class = "datepicker",ID = "DateFrom1" })%><br />
                    To<br />
                    <%=Html.TextBoxFor(m => m.DateTo1, Model.DateTo1, new {  @class = "datepicker",ID = "DateTo1" })%> </td>
            </tr>
            <tr>
                <td class="">Address Line 1</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.SchoolAttendedAddress11, Model.SchoolAttendedAddress11, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Address Line 2</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.SchoolAttendedAddress21, Model.SchoolAttendedAddress21, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">City</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.SchoolAttendedCity1, Model.SchoolAttendedCity1, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">State</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.SchoolAttendedState1, Model.SchoolAttendedState1, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">School Name</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.SchoolName2, Model.SchoolName2, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Dates Attended<br />
                    (MM/dd/yyyy)</td>
                <td class="nobdr">From<br />
                    <%=Html.TextBoxFor(m => m.DateFrom2, Model.DateFrom2, new {  @class = "datepicker" ,ID = "DateFrom2" })%><br />
                    To<br />
                    <%=Html.TextBoxFor(m => m.DateTo2, Model.DateTo2, new {  @class = "datepicker",ID = "DateTo2" })%></td>
            </tr>
            <tr>
                <td class="">Address Line 1</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.SchoolAttendedAddress12, Model.SchoolAttendedAddress12, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Address Line 2</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.SchoolAttendedAddress22, Model.SchoolAttendedAddress22, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">City</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.SchoolAttendedCity2, Model.SchoolAttendedCity2, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">State</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.SchoolAttendedState2, Model.SchoolAttendedState2, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">School Name</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.SchoolName3, Model.SchoolName3, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Dates Attended<br />
                    (MM/dd/yyyy)</td>
                <td class="nobdr">From<br />
                    <%=Html.TextBoxFor(m => m.DateFrom3, Model.DateFrom3, new {  @class = "datepicker",ID = "DateFrom3" })%><br />
                    To<br />
                    <%=Html.TextBoxFor(m => m.DateTo3, Model.DateTo3, new {  @class = "datepicker",ID = "DateTo3" })%></td>
            </tr>
            <tr>
                <td class="">Address Line 1</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.SchoolAttendedAddress13, Model.SchoolAttendedAddress13, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">Address Line 2</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.SchoolAttendedAddress23, Model.SchoolAttendedAddress23, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">City</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.SchoolAttendedCity3, Model.SchoolAttendedCity3, new {  @class="sd",maxlength=50 })%></td>
                <td class="auto-style3">State</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.SchoolAttendedState3, Model.SchoolAttendedState3, new {  @class="sd",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="4"><h4>Insurance</h4></td>
            </tr>
            <tr>
                <td>Insurance Type</td>
                <td><%=Html.TextBoxFor(m => m.InsuranceType, Model.InsuranceType, new {  @class="sd",maxlength=50 })%></td>
                <td>Policy Number</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.PolicyNumber, Model.PolicyNumber, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td>Policy Holder</td>
                <td><%=Html.TextBoxFor(m => m.PolicyHolder, Model.PolicyHolder, new {  @class="sd",maxlength=50 })%></td>
                <td>&nbsp;</td>
                <td class="nobdr">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4"><h4>IEP / Referral Information</h4></td>
            </tr>
            <tr>
                <td>Full Name</td>
                <td><%=Html.TextBoxFor(m => m.ReferralIEPFullName, Model.ReferralIEPFullName, new {  @class="sd",maxlength=250 })%></td>
                <td>Title</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.ReferralIEPTitle, Model.ReferralIEPTitle, new {  @class="sd",maxlength=250 })%></td>
            </tr>
            <tr>
                <td>Phone</td>
                <td><%=Html.TextBoxFor(m => m.ReferralIEPPhone, Model.ReferralIEPPhone, new {  @class="validate[custom[usPhoneNumber]]",maxlength=50 })%></td>
                <td>Referring Agency</td>
                <td class="nobdr"><%=Html.TextBoxFor(m => m.ReferralIEPReferringAgency, Model.ReferralIEPReferringAgency, new {  @class="sd" })%></td>
            </tr>
            <tr>
                <td>Source Of Tuition</td>
                <td><%=Html.TextBoxFor(m => m.ReferralIEPSourceofTuition, Model.ReferralIEPSourceofTuition, new {  @class="sd" })%></td>
                <td>&nbsp;</td>
                <td class="nobdr">&nbsp;</td>
            </tr>
            <tr>
                <td class="" colspan="4"><h4>Education History</h4></td>
            </tr>
            <tr>
                <td class="auto-style5">Date Initially Eligible for Special Education</td>
                <td class="auto-style6">
                    <%=Html.TextBoxFor(m => m.DateInitiallyEligibleforSpecialEducation, Model.DateInitiallyEligibleforSpecialEducation, 
            new {  @class="datepicker", ID = "DateInitiallyEligibleforSpecialEducation"  })%></td>
                <td class="auto-style7">Date of Most Recent Special Education Evaluations</td>
                <td class="nobdr">
                    <%=Html.TextBoxFor(m => m.DateofMostRecentSpecialEducationEvaluations, Model.DateofMostRecentSpecialEducationEvaluations, 
            new {  @class="datepicker", ID = "DateofMostRecentSpecialEducationEvaluations"  })%></td>
            </tr>
            <tr>
                <td class="">Date of Next Scheduled 3-Year Evaluation</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.DateofNextScheduled3YearEvaluation, Model.DateofNextScheduled3YearEvaluation, 
            new {  @class="datepicker", ID = "DateofNextScheduled3YearEvaluation"  })%></td>
                <td class="auto-style3">Current IEP Start Date</td>
                <td class="nobdr">
                    <%=Html.TextBoxFor(m => m.CurrentIEPStartDate, Model.CurrentIEPStartDate, new {  @class="datepicker", ID = "CurrentIEPStartDate"  })%></td>
            </tr>
            <tr>
                <td class="">Current IEP Expiration Date</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.CurrentIEPExpirationDate, Model.CurrentIEPExpirationDate, new {  @class="datepicker", ID = "CurrentIEPExpirationDate"  })%></td>
                <td class="auto-style3">&nbsp;</td>
                <td class="nobdr">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style9" colspan="4"><h4>Discharge Information</h4></td>
            </tr>
            <tr>
                <td class="">Discharge Date</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.DischargeDate, Model.DischargeDate, new {  @class="datepicker", ID = "DischargeDate"  })%></td>
                <td class="auto-style3">Location After Discharge</td>
                <td class="nobdr">
                    <%=Html.TextBoxFor(m => m.LocationAfterDischarge, Model.LocationAfterDischarge, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td class="">Melmark New England&#39;s Follow Up Responsibilities</td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.MelmarkNewEnglandsFollowUpResponsibilities, Model.MelmarkNewEnglandsFollowUpResponsibilities, new {  @class="sd" })%></td>
                <td class="auto-style3">&nbsp;</td>
                <td class="nobdr">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4" style="margin-left: 100px">
                    <% if (ViewBag.permission == "true")
                       {
                        if (Model.Id > 0)
                      { %>
                    <input id="btnUpdate" type="submit" value="Update" style="margin-left: 5%" />
                    <%}
                      else
                      {%>
                    <input id="btnSubmit" type="submit" value="Save" style="margin-left: 5%" /><% }
                                                                                                  }%>
                </td>
            </tr>
        </table>
    </div>
</div>
    <div class="rightContainer">
    <div id="rightSidePanel" >
        <table>
            <tr>
                <%if (Model.Id > 0)
                  { 
                %>
                <td id="tdstudentName" class="nobdr"><%=Model.LastName %> , <%=Model.FirstName %></td>
                <%}
                  else
                  { } %>
            </tr>
            <tr>
                <%if (Model.Id > 0)
                  {%>
                <td class="nobdr"><%=Model.StudentId %></td>
                <% }
                  else
                  { } %>
            </tr>
            <tr>
                <td class="nobdr">
                    <%-- <%=Html.DisplayFor(m=>m.ImageUrl) %>--%>
                    <img id="stdImage" alt="No Image Uploaded" src="data:image/gif;base64,<%=Model.ImageUrl %>" /></td>
            </tr>
            <tr>
               <%if (Model.ImageUrl != null)
                  {%>
                <td class="nobdr">
                    <input type="file" id="profilePicture" name="profilePicture" style="width: 210px;" />
                    <%-- <%=Html.TextBoxFor(m => m.profilePicture, new { type = "file" })%>--%>
                </td>
                <% }
                  else
                  {  %>
                <td class="nobdr">
                    <input type="file" id="File1" name="profilePicture" style="width: 210px;"
                        class="validate[required] " />
                </td>
                <%-- <%=Html.TextBoxFor(m => m.profilePicture, new { type = "file" })%>--%>
                <%} %>
            </tr>
            <tr>
                <td class="nobdr">
                    <%if (Model.PhotoReleasePermission == true)
                      { %>

                    <%}
                      else
                      { %>

                    <%} %>
                    <%-- <input id="chkPhotoPermission" type="checkbox" onchange="" />Photo Release Permitted</td>--%>
            </tr>

        </table>
    </div>
        </div>


<%} %>


