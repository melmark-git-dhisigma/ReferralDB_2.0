<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ReferalDB.Models.GenInfoModel>" %>

<meta name="viewport" content="width=device-width" />
<title>Referral Application Form PE</title>

<%--<script src="../../Scripts/jquery-1.8.2.js"></script>
<script src="../../Scripts/jquery.unobtrusive-ajax.js"></script>
<script src="../../Scripts/jquery.unobtrusive-ajax.min.js"></script>
<script src="../../Scripts/jquery-ui-1.8.24.js"></script>
<link href="../../CSS/jquery-ui-1.10.3.custom.css" rel="stylesheet" />
<link href="../../CSS/jquery-ui.css" rel="stylesheet" />--%>
<script src="../../Scripts/browser.js"></script>
<script src="../Scripts/jquery.mask.js"></script>
<script src="../Scripts/jquery.maskMoney.min.js"></script>

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
    }

    .divSubs {
        width: 100%;
    }

        .divSubs table {
            /**/
            width: 100%;
            margin: -1px auto;
            table-layout: fixed;
            /*display: none;*/
        }

            .divSubs table span {
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

    .subhead-selfskills {
        color: #09681a !important;
        font-family: Arial;
        font-size: 14px;
        height: 10px;
        line-height: 12px;
        padding-right: 1px;
        text-align: center !important;
        font-weight: bold;
        padding-top: 16px;
        padding-bottom: 8px;
        border-bottom: 1px solid #09681a;
    }

    .sub-subhead {
        color: #09681a !important;
        font-family: Arial;
        font-size: 15px;
        height: 10px;
        line-height: 12px;
        padding-right: 1px;
        text-align: left;
        font-weight: bold;
        padding-top: 22px;
        padding-bottom: 12px;
        /*text-decoration: underline;*/

        

    }

    .tblCell {
        border: 1px groove black;
    }

    .fontwhite, .validate {
        position: absolute !important;
        font-size: 20px;
        margin-left: -10px;
        margin-top: 10px;
    }

    input[type="text"] {
        margin-right: 0px !important;
        width: auto !important;
    }

    .tblPattern {
        table-layout: auto;
    }
</style>



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
    }


    .validate {
        color: red;
    }

    .fontwhite {
        color: white;
    }

    .tblCell input[type="text"] {
        margin-right: 0px;
    }

    .tblCell textarea {
        padding: 0px;
    }
</style>


<script type="text/javascript">

    // rbtbornStatusNat rbtbornStatusAdo

    

    function PreventDef(e) {
        e.preventDefault();
    }
    $(document).ready(function () {
       

    });
    function setZipDefault() {

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
        GetNameFieldValidate();
    }
    function getColapse(tableId) {
        // alert(tableId);
        var saveButtonName = $('#btnGenSave').attr('value');

        if (saveButtonName == "Update") {
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
    }//GrossisNumberKey
    function GrossisNumberKey(id, evt) {



        $('#' + id).unbind('blur');
        $('#' + id).blur(function () {
            var textCont = $(this).val();
            var preText = ".00";
            var tr = false;
            if (textCont != "" && textCont != null) {
                for (var i = 0; i < textCont.length ; i++) {
                    if (textCont[i] == ".") {
                        tr = true;
                    }
                }
            }
            var temp;
            if (tr == false)
                temp = textCont + preText;
            else temp = textCont;

            var num = parseFloat(temp);
            num = num.toFixed(2);
            //num = num.toString();
            //$('#' + id).maskMoney({ prefix: '$ ', thousands: ',', decimal: '.', affixesStay: true });
            //alert(num)
            //$('#' + id).val(num);
            //$('#' + id).maskMoney('mask', num);

            //alert(resullt)
        });
        var charCode = (evt.which) ? evt.which : evt.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }


        return true;
    }

    function ZipisNumberKey(id, evt) {



        $('#' + id).unbind('blur');
        $('#' + id).blur(function () {
            var textCont = $(this).val();
            var preText = "";
            if (textCont.length < 5) {
                for (var i = 0; i < (5 - textCont.length) ; i++) {
                    preText = preText + "0";
                }
            }

            $(this).val(preText + textCont);
        });


        var charCode = (evt.which) ? evt.which : evt.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        else {
            if ($('#' + id).val().length > 4) {

                if ((charCode >= 48 && charCode <= 57)) {
                    return false;
                }
            }
        }

        return true;
    }


    function ValidateAlpha(evt) {
        var keyCode = (evt.which) ? evt.which : evt.keyCode
        if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32)

            return false;
        return true;
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

<script type="text/javascript">
    function setMask() {
        $('.usPhone').mask('(000)000-0000');
        $('.ssnNo').mask('000-00-0000');


        //     $('.grossIncome').mask('000000.00');

        $('#txtPreheight,#txtPreWeight').css('margin-right', '0px');
    }
</script>



<script type="text/javascript">


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


    function showResponse(responseText, statusText, xhr, $form) {
        //  alert(responseText);
        if (responseText == "") {
            $("html, body").animate({ scrollTop: 0 }, "slow");//clsTabDiv
            var height2 = $(".clsTabDiv").height()
            $("#overlay").css('height', height2);
            $("#overlay").show();
            $("#divresponse").show();
        }
        else if (responseText == "No file selected") {
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

            if (tbleId.length > 0) {
                $('#TemplatePart' + tbleId).html(responseText);

                var table = "tblData" + tbleId;
                document.getElementById(table).style.display = 'block';
                $('#showmsg').html('Data Saved Successfully');
                $('#showmsg').fadeIn();
                $('#showmsg').delay(4000).fadeOut();


                //$('#content').load('../Dashboard/GetLeftMenu');
                //if(parseInt(bowser.version)==9){

                if (tbleId == 1)
                    $('#TemplatePart' + tbleId).load('../RefferalApplicantPE/TabLoad/' + tbleId, setDefaultContl1);
                if (tbleId == 2 || tbleId == 3 || tbleId == 4)
                    $('#TemplatePart' + tbleId).load('../RefferalApplicantPE/TabLoad/' + tbleId, setDefaultContl);
                if (tbleId == 5)
                    $('#TemplatePart' + tbleId).load('../RefferalApplicantPE/TabLoad/' + tbleId, setDefaultContl5);
                if (tbleId == 6)
                    $('#TemplatePart' + tbleId).load('../RefferalApplicantPE/TabLoad/' + tbleId, setDefaultContl6);
                //}

            }
            else {
                $('#showmsg').html('Some Error: Please try Later.... ');
                $('#showmsg').fadeIn();
                $('#showmsg').delay(4000).fadeOut();
            }

        }

    }

    $(function () {
        $('#TemplatePart1').load('../RefferalApplicantPE/TabLoad/1', setDefaultContl1);
        $('#TemplatePart2').load('../RefferalApplicantPE/TabLoad/2', setDefaultContl);
        $('#TemplatePart3').load('../RefferalApplicantPE/TabLoad/3', setDefaultContl);
        $('#TemplatePart4').load('../RefferalApplicantPE/TabLoad/4', setDefaultContl);
        $('#TemplatePart5').load('../RefferalApplicantPE/TabLoad/5', setDefaultContl5);
        $('#TemplatePart6').load('../RefferalApplicantPE/TabLoad/6', setDefaultContl6);
        //$('#TemplatePart7').load('/RefferalApplicantPE/TabLoad/7', setZipDefault);
        $('#TemplatePart7').load('../RefferalApplicantPE/TabLoad/7', setDefaultContl);

    });
    function setDefaultContl() {

        setZipDefault();

    }

    function setDefaultContl1() {

       
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
        $('.grossIncome').maskMoney();

        setZipDefault();
        setCountry();

    }
    function setDefaultContl5() {

        setZipDefault();
        setCountryMedical();

        //var span;
        //$('span').each(function () {
        //    if ($(this).html() == 'Medical History') {
        //        span = $(this);
        //    }
        //});

        $('#tblData5').css('table-layout', 'auto');
        $('#tblData5').find('.tblPattern').css('table-layout', 'auto');

    }
    function setDefaultContl6() {
        setZipDefault();
        setCountryLegal();

        setMask();

    }
    function setCountryLegal() {
        var buttonVal = $('#btnPresentSkillsSave').attr('value');
        if (buttonVal == "Save") {
            var stateVal = 0;
            var countryId = $('.ddlRefCountry').val();
            var countryIdOther = $('.ddlOtherCountry').val();

            $.getJSON('../RefferalApplicantPE/GetStates', { countryId: countryId }, function (result) {


                //$('#txtState').empty();

                $.each(result, function (index, item) {
                    $('.drpClass').append($('<option/>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });

            });



            $.getJSON('../RefferalApplicantPE/GetStates', { countryId: countryIdOther }, function (result) {


                //$('#txtState').empty();

                $.each(result, function (index, item) {
                    $('.drpClass').append($('<option/>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });

            });
        }
        setZipDefault();
    }

    function setCountryMedical() {
        var buttonVal = $('#btnRecretionSave').attr('value');
        if (buttonVal == "Save") {
            var stateVal = 0;
            var countryId = $('.ddlRefCountry').val();
            var countryIdOther = $('.ddlOtherCountry').val();

            $.getJSON('../RefferalApplicantPE/GetStates', { countryId: countryId }, function (result) {


                //$('#txtState').empty();

                $.each(result, function (index, item) {
                    $('.drpClass').append($('<option/>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });

            });



            $.getJSON('../RefferalApplicantPE/GetStates', { countryId: countryIdOther }, function (result) {


                //$('#txtState').empty();

                $.each(result, function (index, item) {
                    $('.drpClass').append($('<option/>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });

            });
        }
        setZipDefault();
    }

    function setCountry() {
        var buttonVal = $('#btnGenSave').attr('value');
        if (buttonVal == "Save") {
            //alert(buttonVal);
            var stateVal = 0;
            var countryId = $('.ddlRefCountry').val();
            var countryIdOther = $('.ddlOtherCountry').val();

            $.getJSON('../RefferalApplicantPE/GetStates', { countryId: countryId }, function (result) {


                //$('#txtState').empty();

                $.each(result, function (index, item) {
                    $('.drpClass').append($('<option/>', {
                        value: item.Value,
                        text: item.Text
                    }));
                });

            });



            $.getJSON('../RefferalApplicantPE/GetStates', { countryId: countryIdOther }, function (result) {


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

    function GetNameFieldValidate() {
        $('.namefield').keypress(function (event) {
            var inputValue = event.which;
            if (((inputValue >= 65 && inputValue <= 90) || (inputValue >= 97 && inputValue <= 122) || (inputValue == 32) || (inputValue == 39) || (inputValue == 45) || (inputValue == 8))) {
            }
            else {
                event.preventDefault();
            }
        });
    }

    function ShowDocuments() {

        $('#LoadLetterTray').load("../GeneralInfo/UploadedFilesView?Add_doc=0", function (responseTxt, statusTxt, xhr) {  // To load Letter tray
            $('#LoadLetterTray').html(responseTxt);
        });
        $("#divUploads").slideToggle();
    }
    function closeDiv(obj) {
        $("#divUploads").hide();
    }
    function closeDivresp(obj) {
        $("#divresponse").hide();
        $('#overlay').hide();
    }

</script>

<div id="divUploads" class="popUpStyle" style="width: 76%; height: 70%; left: 11%; top: 15%; overflow-y: auto; overflow-x: hidden">
    <a id="close_x" onclick="closeDiv(divUploads)" class="close sprited1" href="#" style="">
        <img src="../Images/button_red_close.png" height="18" width="18" alt="" style="float: right; margin-right: 22px; margin-top: 16px; z-index: 300" /></a>

    <br />
    <hr />

    <div id="LoadLetterTray"></div>

</div>

<div id="overlay" style="width: 100%; position: absolute; background-color: #ccc; opacity: .3; z-index: 999;"></div>
<div id="divresponse" class="popUpStyle" style="overflow-y: auto; overflow-x: hidden; display: block; height: 15%; width: 32%; left: 36%; top: 49%; display: none; z-index: 1000;">
    <a id="A1" onclick="closeDivresp(divresponse)" class="close sprited1" href="#" style="">
        <img src="../Images/button_red_close.png" height="13" width="13" alt="" style="float: right; margin-right: 22px; margin-top: 16px; z-index: 300" /></a>

    <br />
    <hr />

    <div id="Div2">
        <table style="width: 100%">
            <tr>
                <td style="text-align: center;">
                    <h3>An Error Has Occured. Please Contact Administrator.</h3>
                </td>
            </tr>

        </table>


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
    <%-- <div id="TemplatePart7"></div>--%>
    <div id="TemplatePart7"></div>

    <%-- <div id="ajaxloader"></div>--%>
</div>







