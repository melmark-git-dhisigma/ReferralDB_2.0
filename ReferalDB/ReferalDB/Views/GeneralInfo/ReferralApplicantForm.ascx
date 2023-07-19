<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<meta name="viewport" content="width=device-width" />
<title>GeneralInfoData</title>

<%--<script src="../../Scripts/jquery-1.8.2.js"></script>--%>
<%--<script src="../../Scripts/jquery.unobtrusive-ajax.js"></script>
<script src="../../Scripts/jquery.unobtrusive-ajax.min.js"></script>--%>
<%--<script src="../../Scripts/jquery-ui-1.8.24.js"></script>
<link href="../../CSS/jquery-ui-1.10.3.custom.css" rel="stylesheet" />
<link href="../../CSS/jquery-ui.css" rel="stylesheet" />--%>
<script src="../../Scripts/browser.js"></script>
<script src="../Scripts/jquery.mask.js"></script>
<style type="text/css">
    .headPanel {
        background-color: #1EB53A;
        color: #FFFFFF;
        float: left;
        font-family: Arial;
        font-size: 15px;
        font-weight: bold;
        height: 35px;
        line-height: 35px;
        margin: 0 0 -3px;
        padding-left: 5px;
        text-align: left;
        cursor: pointer;
        width: 99.5%;
        margin: 0 auto 2px;
    }

    #popupDiv {
        background-color: #ada3a3;
        display: none;
        height: 26px;
        margin: auto;
        padding: 5px;
        position: fixed;
        top: 50%;
        width: 78%;
        font-size: 18px;
        color: black;
        z-index: 10000;
        left: 400px;
        font-family: Arial;
    }


    .lblBold {
        color: #09681a !important;
        font-family: Arial;
        font-size: 17px;
        font-weight: normal;
        padding: 10px 0 10px 15px !important;
        margin: 0 0 18px;
        text-align: left;
        border-bottom: 1px solid #09681a;
        /*border-top: 1px solid #09681a;*/
    }

    .divSubs {
        width: 100%;
    }

        .divSubs table {
            border-left: 2px solid #E5E5E5;
            border-right: 2px solid #E5E5E5;
            width: 100%;
            margin: -1px auto;
            table-layout: fixed;
            /*display: none;*/
        }

    .clsTabDiv {
        display: none;
    }

    #tblData1 {
        display: block;
    }

    .divSubs table tr td {
        font-family: Arial;
        color: #666;
        padding-right: 1px;
        text-align: left;
        line-height: 12px;
        height: 10px;
        width: 160px;
    }



    input[type=text] {
        height: 25px;
        border: 1px solid #D7CECE;
    }

        input[type=text]:focus {
            background-color: #f1eded;
        }

    select:focus {
        background-color: #f1eded;
    }

    .ui-datepicker-month {
        width: 49% !important;
    }

    .ui-datepicker-year {
        width: 49% !important;
    }

    select {
        height: 25px;
        border: 1px solid #D7CECE;
    }

    .tdText {
        color: #666666;
        font-family: Arial;
        font-size: 13px;
        height: 10px;
        line-height: 12px;
        padding-right: 1px;
        text-align: left;
    }

    .tdPassage {
        color: #666666;
        font-family: Arial;
        font-size: 12px;
        font-weight: bold;
        height: 14px;
        line-height: 20px;
        padding: 3px;
        text-align: left;
    }

    .NFButton {
        background-color: #03507D;
        background-position: 0 0;
        border: medium none;
        border-radius: 5px 5px 5px 5px;
        color: #FFFFFF;
        cursor: pointer;
        font-family: Arial,Helvetica,sans-serif;
        font-size: 12px;
        font-weight: bold;
        height: 26px;
        text-decoration: none;
        width: 91px;
    }

        .NFButton:visited {
            color: #FFFFFF;
        }

    .tblPattern {
        width: 100%;
        border: 1px groove black;
    }

    .tblHeader {
        color: #666666;
        font-family: Arial;
        font-size: 13px;
        height: 10px;
        line-height: 12px;
        padding-right: 1px;
        text-align: left;
        font-weight: bold;
        border: 1px groove black;
    }

    .subhead {
        color: #09681a !important;
        font-family: Arial;
        font-size: 14px;
        height: 10px;
        line-height: 12px;
        padding-right: 1px;
        text-align: left;
        font-weight: bold;
        padding-top: 16px;
        padding-bottom: 8px;
    }

    .fontwhite, .validate {
        position: absolute !important;
        font-size: 20px;
        margin-left: -10px;
        margin-top: 10px;
    }

    .tblCell {
        border: 1px groove black;
    }

    .validate {
        color: red;
    }

    input[type="text"] {
        margin-right: 0px !important;
        width: auto !important;
    }

    .tblPattern {
        table-layout: auto;
    }

    .fontwhite {
        color: white;
    }
</style>


<script type="text/javascript">

    function PreventDef(e) {
        e.preventDefault();
    }

    function getColapse(tableId) {


        var saveButtonName = $('#btnGenSave').attr('value');

        if (saveButtonName == "Update") {

            // alert(tableId);
            var CurrentTab = $('#hdnQueue').val().split(',');
            var permission = <%: ViewBag.permission%>
            permission = permission.toString();
            if (CurrentTab[1] == 'ClientList' || permission == 'false') {
                $('#MidContent').find("input[type=submit]").css("display", "none");
            }
            if ($("#" + tableId).css('display') == 'none') {

                var tbId;
                for (var i = 1; i < 9; i++) {

                    tbId = "tblData" + i.toString();

                    $('#' + tbId).hide();
                }
                $("#" + tableId).slideToggle('slow');
            }
            else {

                $("#" + tableId).slideUp('slow');
            }
        }
        else {
            alert("Plese save the General Information first.");
        }
    }


    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

        return true;
    }


    function ValidateAlpha(evt) {
        var keyCode = (evt.which) ? evt.which : evt.keyCode
        if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32)

            return false;
        return true;
    }

    function isValidKey(e) {

        var charCode = e.keyCode || e.which;
        if (charCode == 8 || charCode == 46)
            return false;

        return true;
    }

    function numbersOnlyT(e, txt) {
        var inputValue = event.which;
        if ((inputValue >= 48 && inputValue <= 57)) {
            var value = $(txt).val();
            if (value.length > 2) {
                event.preventDefault();
            }

        }
        else {
            event.preventDefault();
        }

    }

    function numbersOnlyTs(e, txt) {
        var inputValue = event.which;
        if ((inputValue >= 48 && inputValue <= 57)) {
            var value = $(txt).val();
            if (value.length > 2) {
                event.preventDefault();
            }

        }
        else {
            event.preventDefault();
        }

    }
    function decimalCheck(e, txt) {
        var textValue = (e.which) ? e.which : e.keyCode;

        var number = /[0-9]/;
        var value = $(txt).val();

        var valueSplit = value.split('.');


        if (textValue == 8 || textValue == 9 || textValue == 45 || textValue == 46 || number.test(String.fromCharCode(textValue))) {
            if (valueSplit.length > 1) {
                if (textValue == 46) {

                    return false;
                }
                else {
                    if (value.length == 3) {
                        if (textValue == 50) {
                            return false;
                        }
                    }
                }

                if (value.length > 4) {
                    return false;
                }

            }
            else {
                if (textValue != 46 && textValue != 8) {
                    if (value.length > 1) {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    }


</script>

<style type="text/css">
    .selectbg11 {
        border: medium none !important;
        font-size: 9px;
        margin-right: 8px;
        margin-top: 10px;
        width: 165px;
    }
</style>


<style type="text/css">
    #ajaxloader {
        display: none;
        position: fixed;
        z-index: 1000;
        top: 0;
        left: 0;
        height: 100%;
        width: 100%;
        /*left:20%;*/
        background: rgba(255,255,255, .8 ) url('../Images/LoaderRound.gif') 50% 50% no-repeat;
    }

    .divmsgshow {
        position: fixed;
        padding-top: 5px;
        height: 22px;
        width: 20%;
        top: 40%;
        left: 48%;
        text-align: center;
        background-color: green;
        font-weight: bold;
        color: white;
        display: none;
        z-index: 9999;
    }
</style>

<script type="text/javascript">


    $(document).ready(function () {

        $("#txtlblSchoolzipcodeResp").attr('maxlength', '5');
        $("#txtlblSchoolzipcode").attr('maxlength', '5');


    });

    var options = {
        success: showResponse  // post-submit callback        
    };

    function GetSelected(controlId) {

        if (controlId.checked == true) {
            controlId.value = 'checked';
            controlId.checked = true;
        }
        else {
            controlId.value = 'unchecked';
            controlId.checked = false;
        }



    }

    function setCountry() {
        var buttonVal = $('#btnGenSave').attr('value');
        if (buttonVal == "Save") {
            //alert(buttonVal);
            var stateVal = 0;
            var countryId = $('.ddlRefCountry').val();
            var countryIdOther = $('.ddlOtherCountry').val();

            $.getJSON('../GeneralInfo/GetStates', { countryId: countryId }, function (result) {


                //$('#txtState').empty();

                $.each(result, function (index, item) {
                    $('.drpClass').append($('<option/>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });

            });



            $.getJSON('../GeneralInfo/GetStates', { countryId: countryIdOther }, function (result) {


                //$('#txtState').empty();

                $.each(result, function (index, item) {
                    $('.drpClass').append($('<option/>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });

            });
        }
        GetNameFieldValidate();
        setZipDefault();
    }

    function setZipDefault() {
        GetNameFieldValidate();
        $('input[maxlength=5]').blur(function () {
            var textCont = $(this).val();
            var preText = "";
            if (textCont.length < 5) {
                for (var i = 0; i < (5 - textCont.length) ; i++) {
                    preText = preText + "0";
                }
            }

            $(this).val(preText + textCont);

        });
    }

    function GetNameFieldValidate() {
        $('.namefield').keypress(function (event) {
            //if (event.ctrlKey)
            //isCtrl = true;
            var inputValue = event.which;
            if (((inputValue >= 65 && inputValue <= 90) || (inputValue >= 97 && inputValue <= 122) || (inputValue == 32) || (inputValue == 39) || (inputValue == 45) || (inputValue == 8) || (inputValue == 0))) {
            }
            else {
                event.preventDefault();
            }
        });
    }

    function PreventDef(e) {
        e.preventDefault();
    }




    function showResponse(responseText, statusText, xhr, $form) {

        if (responseText == "No file selected") {
            alert("No file selected...");
        }
        else if (responseText == "Invalid file format") {
            alert("Invalid file format");
        }
        else if (responseText == "Invalid file") {
            alert("Invalid file. Only .jpg, .png, .bmp, .jpeg image formats are allowed");
        }
        else {

            var idVal = $(responseText)
            //alert($(idVal).find('.tabcls').val());

            var tbleId = $(idVal).find('.tabcls').val();
            if (tbleId != "undefined") {

                if (tbleId.length > 0) {
                    $('#TemplatePart' + tbleId).html(responseText);
                    //   forIe9();
                    var table = "tblData" + tbleId;
                    document.getElementById(table).style.display = 'block';
                    $('#showmsg').html('Data Saved Successfully');
                    $('#showmsg').fadeIn();
                    $('#showmsg').delay(4000).fadeOut();

                    //$('#content').load('../Dashboard/GetLeftMenu');

                    $('#TemplatePart' + tbleId).load('../GeneralInfo/Section1/' + tbleId, setDefaultContls);

                }
                else {
                    $('#showmsg').html('Some Error: Please try Later.... ');
                    $('#showmsg').fadeIn();
                    $('#showmsg').delay(4000).fadeOut();
                }
            } else {
                $('#showmsg').html('Some Error: Please try Later.... ');
                $('#showmsg').fadeIn();
                $('#showmsg').delay(4000).fadeOut();
            }
        }

    }

    $(function () {

        $('#TemplatePart1').load('../GeneralInfo/Section1/1', setDefaultContls);
        $('#TemplatePart2').load('../GeneralInfo/Section1/2', setDefaultContls);
        $('#TemplatePart3').load('../GeneralInfo/Section1/3', setDefaultContls);
        $('#TemplatePart4').load('../GeneralInfo/Section1/4', setDefaultContls);
        $('#TemplatePart5').load('../GeneralInfo/Section1/5', setDefaultContls);
        $('#TemplatePart6').load('../GeneralInfo/Section1/6', setDefaultContls);
        $('#TemplatePart7').load('../GeneralInfo/Section1/7', setDefaultContls);
        $('#TemplatePart8').load('../GeneralInfo/Section1/8', setDefaultContls);



        $("#txtPhyZipcode").attr('maxlength', '5');
        $("#txtInsZipcode").attr('maxlength', '5');
        $("#txtDenInsZipcode").attr('maxlength', '5');

        $("#txtSecInsZipcode").attr('maxlength', '5');
        $("#txtlblPhysicianZipcode").attr('maxlength', '5');
        $("#txtEyelblPhysicianZipcode").attr('maxlength', '5');
        $("#txtlblPhysicianZipcodeEar").attr('maxlength', '5');
        $("#txtDentallblPhysicianZipcode").attr('maxlength', '5');
        $("#txtOtherlblPhysicianZipcode").attr('maxlength', '5');



        $("#txtlblSchoolzipcodeResp").attr('maxlength', '5');
        $("#txtlblSchoolzipcode").attr('maxlength', '5');

    });

    function setDefaultContls() {

        setCountry();
        setZipDefault();
        setCountryBirth();
        setMask();
        $('#rbtbornStatusNat').change(function () {
            $('#rbtbornStatusAdo').prop('checked', false);
            $('#rbtbornStatusNat').val('checked');
            $('#rbtbornStatusAdo').val('unchecked');
        });
        $('#rbtbornStatusAdo').change(function () {
            $('#rbtbornStatusNat').prop('checked', false);
            $('#rbtbornStatusAdo').val('checked');
            $('#rbtbornStatusNat').val('unchecked');
        });


        $("#txtMthrDob").datepicker("option", "onSelect", function (dateText, inst) {
            var today = new Date(), // today date object
      birthday_val = $("#txtMthrDob").val().split('/') // input value
            birthday = new Date(birthday_val[2], birthday_val[0] - 1, birthday_val[1]) // birthday date object
            todayYear = today.getFullYear() // today year
            todayMonth = today.getMonth() // today month
            todayDay = today.getDate() // today day of month
            birthdayYear = birthday.getFullYear() // birthday year
            birthdayMonth = birthday.getMonth() // birthday month
            birthdayDay = birthday.getDate() // birthday day of month
            // calculate age
            age = (todayMonth == birthdayMonth && todayDay > birthdayDay) ?
                  todayYear - birthdayYear : (todayMonth > birthdayMonth) ?
                        todayYear - birthdayYear : todayYear - birthdayYear - 1;

            $('#txtlblMotherEmployerAge').val(age);
        });

        $("#txtFthrDob").datepicker("option", "onSelect", function (dateText, inst) {
            var today = new Date(), // today date object
      birthday_val = $("#txtFthrDob").val().split('/') // input value
            birthday = new Date(birthday_val[2], birthday_val[0] - 1, birthday_val[1]) // birthday date object
            todayYear = today.getFullYear() // today year
            todayMonth = today.getMonth() // today month
            todayDay = today.getDate() // today day of month
            birthdayYear = birthday.getFullYear() // birthday year
            birthdayMonth = birthday.getMonth() // birthday month
            birthdayDay = birthday.getDate() // birthday day of month
            // calculate age
            age = (todayMonth == birthdayMonth && todayDay > birthdayDay) ?
                  todayYear - birthdayYear : (todayMonth > birthdayMonth) ?
                        todayYear - birthdayYear : todayYear - birthdayYear - 1;

            $('#txtlblFatherEmployerAge').val(age);
        });





        setFatherMaritalStatus();
        setFthrUsCitizen();
        setMthrUsCitizen()
        setMotherMaritalStatus();
    }

    function setMthrUsCitizen() {
        $('#rbtMthrUSCitizenYes').change(function () {
            $('#rbtMthrUSCitizenNo').prop('checked', false);
            $('#rbtMthrUSCitizenYes').val('checked');
            $('#rbtMthrUSCitizenNo').val('unchecked');
        });
        $('#rbtMthrUSCitizenNo').change(function () {
            $('#rbtMthrUSCitizenYes').prop('checked', false);
            $('#rbtMthrUSCitizenNo').val('checked');
            $('#rbtMthrUSCitizenYes').val('unchecked');
        });
    }

    function setFthrUsCitizen() {
        $('#rbtFthrUSCitizenYes').change(function () {
            $('#rbtFthrUSCitizenNo').prop('checked', false);
            $('#rbtFthrUSCitizenYes').val('checked');
            $('#rbtFthrUSCitizenNo').val('unchecked');
        });
        $('#rbtFthrUSCitizenNo').change(function () {
            $('#rbtFthrUSCitizenYes').prop('checked', false);
            $('#rbtFthrUSCitizenNo').val('checked');
            $('#rbtFthrUSCitizenYes').val('unchecked');
        });
    }



    function setMotherMaritalStatus() {
        $('#rbtMotherMarried').change(function () {
            $('#rbtMotherSeperated').prop('checked', false);
            $('#rbtMotherDivorced').prop('checked', false);
            $('#rbtMotherRemarried').prop('checked', false);
            $('#rbtMotherWidowed').prop('checked', false);
            $('#rbtMotherSingle').prop('checked', false);
            $('#rbtMotherMarried').val('checked');
            $('#rbtMotherSeperated').val('unchecked');
            $('#rbtMotherDivorced').val('unchecked');
            $('#rbtMotherRemarried').val('unchecked');
            $('#rbtMotherWidowed').val('unchecked');
            $('#rbtMotherSingle').val('unchecked');
        });

        $('#rbtMotherSeperated').change(function () {
            $('#rbtMotherMarried').prop('checked', false);
            $('#rbtMotherDivorced').prop('checked', false);
            $('#rbtMotherRemarried').prop('checked', false);
            $('#rbtMotherWidowed').prop('checked', false);
            $('#rbtMotherSingle').prop('checked', false);
            $('#rbtMotherSeperated').val('checked');
            $('#rbtMotherMarried').val('unchecked');
            $('#rbtMotherDivorced').val('unchecked');
            $('#rbtMotherRemarried').val('unchecked');
            $('#rbtMotherWidowed').val('unchecked');
            $('#rbtMotherSingle').val('unchecked');
        });
        $('#rbtMotherDivorced').change(function () {
            $('#rbtMotherMarried').prop('checked', false);
            $('#rbtMotherSeperated').prop('checked', false);
            $('#rbtMotherRemarried').prop('checked', false);
            $('#rbtMotherWidowed').prop('checked', false);
            $('#rbtMotherSingle').prop('checked', false);
            $('#rbtMotherDivorced').val('checked');
            $('#rbtMotherMarried').val('unchecked');
            $('#rbtMotherSeperated').val('unchecked');
            $('#rbtMotherRemarried').val('unchecked');
            $('#rbtMotherWidowed').val('unchecked');
            $('#rbtMotherSingle').val('unchecked');
        });
        $('#rbtMotherRemarried').change(function () {
            $('#rbtMotherMarried').prop('checked', false);
            $('#rbtMotherSeperated').prop('checked', false);
            $('#rbtMotherDivorced').prop('checked', false);
            $('#rbtMotherWidowed').prop('checked', false);
            $('#rbtMotherSingle').prop('checked', false);
            $('#rbtMotherRemarried').val('checked');
            $('#rbtMotherMarried').val('unchecked');
            $('#rbtMotherSeperated').val('unchecked');
            $('#rbtMotherDivorced').val('unchecked');
            $('#rbtMotherWidowed').val('unchecked');
            $('#rbtMotherSingle').val('unchecked');
        });
        $('#rbtMotherWidowed').change(function () {
            $('#rbtMotherMarried').prop('checked', false);
            $('#rbtMotherSeperated').prop('checked', false);
            $('#rbtMotherDivorced').prop('checked', false);
            $('#rbtMotherRemarried').prop('checked', false);
            $('#rbtMotherSingle').prop('checked', false);
            $('#rbtMotherWidowed').val('checked');
            $('#rbtMotherMarried').val('unchecked');
            $('#rbtMotherSeperated').val('unchecked');
            $('#rbtMotherDivorced').val('unchecked');
            $('#rbtMotherRemarried').val('unchecked');
            $('#rbtMotherSingle').val('unchecked');
        });
        $('#rbtMotherSingle').change(function () {
            $('#rbtMotherMarried').prop('checked', false);
            $('#rbtMotherSeperated').prop('checked', false);
            $('#rbtMotherDivorced').prop('checked', false);
            $('#rbtMotherRemarried').prop('checked', false);
            $('#rbtMotherWidowed').prop('checked', false);
            $('#rbtMotherSingle').val('checked');
            $('#rbtMotherMarried').val('unchecked');
            $('#rbtMotherSeperated').val('unchecked');
            $('#rbtMotherDivorced').val('unchecked');
            $('#rbtMotherRemarried').val('unchecked');
            $('#rbtMotherWidowed').val('unchecked');
        });

    }

    function setFatherMaritalStatus() {
        $('#rbtFatherMarried').change(function () {
            $('#rbtFatherSeperated').prop('checked', false);
            $('#rbtFatherDivorced').prop('checked', false);
            $('#rbtFatherRemarried').prop('checked', false);
            $('#rbtFatherWidowed').prop('checked', false);
            $('#rbtFatherSingle').prop('checked', false);
            $('#rbtFatherMarried').val('checked');
            $('#rbtFatherSeperated').val('unchecked');
            $('#rbtFatherDivorced').val('unchecked');
            $('#rbtFatherRemarried').val('unchecked');
            $('#rbtFatherWidowed').val('unchecked');
            $('#rbtFatherSingle').val('unchecked');
        });

        $('#rbtFatherSeperated').change(function () {
            $('#rbtFatherMarried').prop('checked', false);
            $('#rbtFatherDivorced').prop('checked', false);
            $('#rbtFatherRemarried').prop('checked', false);
            $('#rbtFatherWidowed').prop('checked', false);
            $('#rbtFatherSingle').prop('checked', false);
            $('#rbtFatherSeperated').val('checked');
            $('#rbtFatherMarried').val('unchecked');
            $('#rbtFatherDivorced').val('unchecked');
            $('#rbtFatherRemarried').val('unchecked');
            $('#rbtFatherWidowed').val('unchecked');
            $('#rbtFatherSingle').val('unchecked');
        });
        $('#rbtFatherDivorced').change(function () {
            $('#rbtFatherMarried').prop('checked', false);
            $('#rbtFatherSeperated').prop('checked', false);
            $('#rbtFatherRemarried').prop('checked', false);
            $('#rbtFatherWidowed').prop('checked', false);
            $('#rbtFatherSingle').prop('checked', false);
            $('#rbtFatherDivorced').val('checked');
            $('#rbtFatherMarried').val('unchecked');
            $('#rbtFatherSeperated').val('unchecked');
            $('#rbtFatherRemarried').val('unchecked');
            $('#rbtFatherWidowed').val('unchecked');
            $('#rbtFatherSingle').val('unchecked');
        });
        $('#rbtFatherRemarried').change(function () {
            $('#rbtFatherMarried').prop('checked', false);
            $('#rbtFatherSeperated').prop('checked', false);
            $('#rbtFatherDivorced').prop('checked', false);
            $('#rbtFatherWidowed').prop('checked', false);
            $('#rbtFatherSingle').prop('checked', false);
            $('#rbtFatherRemarried').val('checked');
            $('#rbtFatherMarried').val('unchecked');
            $('#rbtFatherSeperated').val('unchecked');
            $('#rbtFatherDivorced').val('unchecked');
            $('#rbtFatherWidowed').val('unchecked');
            $('#rbtFatherSingle').val('unchecked');
        });
        $('#rbtFatherWidowed').change(function () {
            $('#rbtFatherMarried').prop('checked', false);
            $('#rbtFatherSeperated').prop('checked', false);
            $('#rbtFatherDivorced').prop('checked', false);
            $('#rbtFatherRemarried').prop('checked', false);
            $('#rbtFatherSingle').prop('checked', false);
            $('#rbtFatherWidowed').val('checked');
            $('#rbtFatherMarried').val('unchecked');
            $('#rbtFatherSeperated').val('unchecked');
            $('#rbtFatherDivorced').val('unchecked');
            $('#rbtFatherRemarried').val('unchecked');
            $('#rbtFatherSingle').val('unchecked');
        });
        $('#rbtFatherSingle').change(function () {
            $('#rbtFatherMarried').prop('checked', false);
            $('#rbtFatherSeperated').prop('checked', false);
            $('#rbtFatherDivorced').prop('checked', false);
            $('#rbtFatherRemarried').prop('checked', false);
            $('#rbtFatherWidowed').prop('checked', false);
            $('#rbtFatherSingle').val('checked');
            $('#rbtFatherMarried').val('unchecked');
            $('#rbtFatherSeperated').val('unchecked');
            $('#rbtFatherDivorced').val('unchecked');
            $('#rbtFatherRemarried').val('unchecked');
            $('#rbtFatherWidowed').val('unchecked');
        });

    }

    function setCountryBirth() {
        var buttonVal = $('#btnBirthDetSave').attr('value');
        if (buttonVal == "Save") {
            var stateVal = 0;
            var countryId = $('.ddlRefCountry').val();
            var countryIdOther = $('.ddlOtherCountry').val();
            $.getJSON('../GeneralInfo/GetStates', { countryId: countryId }, function (result) {
                //$('#txtState').empty();

                $.each(result, function (index, item) {
                    $('.drpClass').append($('<option/>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });
            });


            $.getJSON('../GeneralInfo/GetStates', { countryId: countryIdOther }, function (result) {
                //$('#txtState').empty();

                $.each(result, function (index, item) {
                    $('.drpClass').append($('<option/>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });
            });

            setZipDefault();
        }
        GetNameFieldValidate();
    }

    function selectFileImage(input) {
    }

    function ShowDocuments() {
        $('#LoadLetterTray').load("../GeneralInfo/UploadedFilesView?Add_doc=0", function (responseTxt, statusTxt, xhr) {  // To load Letter tray
            $('#LoadLetterTray').html(responseTxt);
        });


        // $('#LoadLetterTray').load("../GeneralInfo/LetterGenerationView"); // To load Letter tray


        $("#divUploads").slideToggle();
    }
    function closeDiv(obj) {
        $("#divUploads").hide();
    }

</script>

<%--<script type="text/javascript">
    $(document).ajaxStart(function () {
        $('#ajaxloader').fadeIn();
    }).ajaxStop(function () {
        $('#ajaxloader').fadeOut();
    });

</script>--%>
<script type="text/javascript">
    function setMask() {
        $('.usPhone').mask('(000)000-0000');
        $('.ssnNo').mask('000-00-0000');
    }
</script>

<div id="divUploads" class="popUpStyle" style="width: 76%; height: 70%; left: 11%; top: 15%; overflow-y: auto; overflow-x: hidden">
    <a id="close_x" onclick="closeDiv(divUploads)" class="close sprited1" href="#" style="">
        <img src="../Images/button_red_close.png" height="18" width="18" alt="" style="float: right; margin-right: 22px; margin-top: 16px; z-index: 300" /></a>

    <br />
    <hr />

    <div id="LoadLetterTray">
    </div>

</div>
<div id="alldata" class="accordion">

    <div id="TemplatePart1">
    </div>
    <div id="showmsg" class="divmsgshow"></div>
    <div id="TemplatePart2"></div>
    <div id="TemplatePart3"></div>
    <div id="TemplatePart4"></div>
    <div id="TemplatePart5"></div>
    <div id="TemplatePart6"></div>
    <div id="TemplatePart7"></div>
    <div id="TemplatePart8"></div>


    <%-- <div id="ajaxloader"></div>--%>
</div>




