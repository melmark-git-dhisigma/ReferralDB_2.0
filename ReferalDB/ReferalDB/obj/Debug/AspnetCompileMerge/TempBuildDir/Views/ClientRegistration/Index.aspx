<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<ReferalDB.Models.RegistrationModel>>" %>
    <script src="../../Scripts/jquery-1.8.2.js"></script>
    <link href="../../CSS/jquery-ui.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-ui-1.8.24.js"></script>
<script src="../../Scripts/jquery.validationEngine-en.js"></script>
<script src="../../Scripts/jquery.validationEngine.js"></script>
    <script type="text/javascript">
        $(function () {

            setTimeout(function () {
                 <% if (ViewBag.Param == 0)
               {%>
                $('#LoadQueue').load('../ClientRegistration/ClientRegistration?data=0|Fill');
                <%}%>
            <% else
               {%>

                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=1');
                $('.EditProfile').css("display", "block");

                setTimeout(function () {
                    $('#LoadQueue').load('../ClientRegistration/ClientRegistration?data=<%= ViewBag.Param%>|Fill');
                }, 500);
            <%}%>
            }, 1000);

            // Loading General Tab as default

            $('#LoadQueue').load('../ClientRegistration/ClientRegistration?data=<%= ViewBag.Param%>|Fill', function (data) { alert(data);});
            $('.imgcontainer').css("display", "block");
            $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=1');
            $('.EditProfile').css("display", "block");
            $('#calender').css("display", "none");

            $('#btnGeneral').css("background", "#23a7e3");
            $('#btnMedical').css("background", "#007ab1");
            $('#btnPlacement').css("background", "#007ab1");
            $('#btnVisitation').css("background", "#007ab1");
            $('#btnLetter').css("background", "#007ab1");
            $('#btnEventLogs').css("background", "#007ab1");
            $('#btnForms').css("background", "#007ab1");
            $('#btnProgress').css("background", "#007ab1");
            $('#btnContact').css("background", "#007ab1");
            $('#btnCallLogs').css("background", "#007ab1");


            $('#dvHeader').html("Client Details : " + $('#tdstudentName').html())
            $('#dvHeader').html("Client Details").html();

            ////////////////////////////////

            $('.leftMenu').click(function () {

                var elmId = $(this).attr('id');
                $('.leftMenu').removeClass('current');
                $(this).addClass('current');
                if (elmId == "btnGeneral") {
                    $('#LoadQueue').load('../ClientRegistration/ClientRegistration?data=<%= ViewBag.Param%>|Fill');
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=1');
                    $('.EditProfile').css("display", "block");
                    $('#calender').css("display", "none");

                    $('#btnGeneral').css("background", "#23a7e3");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");


                    $('#dvHeader').html("Client Details : " + $('#tdstudentName').html())
                    $('#dvHeader').html("Client Details").html();
                }
                if (elmId == "btnMedical") {
                    $('#LoadQueue').load('../Medical/Medical/');
                    //       $('#content').load('/Medical/FillMedicalData/0');
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    $('#calender').css("display", "block");

                    $('#dvHeader').html("Medical Details : " + $('#tdstudentName').html())

                    $('#btnContact').css("background", "#007ab1");
                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#23a7e3");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");

                }
                if (elmId == "btnPlacement") {
                    $('#LoadQueue').load('../Placement/Placement/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    $('#dvHeader').html("Placements : " + $('#tdstudentName').html())


                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#23a7e3");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");

                }
                if (elmId == "btnContact") {
                    $('#LoadQueue').load('../Contact/ListContactVendor/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    $('#dvHeader').html("Contact / Vendor : " + $('#tdstudentName').html())



                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#23a7e3");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");



                }
                if (elmId == "btnVisitation") {
                    $('#LoadQueue').load('../Visitation/Visitation/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
                    $('#dvHeader').html("Visitation / Trip : " + $('#tdstudentName').html());


                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#23a7e3");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");


                }
                if (elmId == "btnEventLogs") {
                    $('#LoadQueue').load('../Event/EventsList/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    $('#dvHeader').html("Event Logs : " + $('#tdstudentName').html())


                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#23a7e3");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");


                }
                if (elmId == "btnForms") {
                    $('#LoadQueue').load('../Forms/ListDocuments/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    $('#dvHeader').html("Documents : " + $('#tdstudentName').html())

                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#23a7e3");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");
                }
                if (elmId == "btnProgress") {
                    $('#LoadQueue').load('../Progress/ProgressRpt/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
                    //$('.rightContainer').hide();

                    $('#dvHeader').html("Progress Reports : " + $('#tdstudentName').html())

                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#23a7e3");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");



                }
                if (elmId == "btnLetter") {
                    $('#LoadQueue').load('../Letter/AllLetter/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
                    //$('.rightContainer').hide();

                    $('#dvHeader').html("Letter Tray : " + $('#tdstudentName').html())

                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#23a7e3");
                    $('#btnCallLogs').css("background", "#007ab1");



                }

                if (elmId == "btnCallLogs") {

                    $('#LoadQueue').load('../CallLog/CallLog/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
                    //$('.rightContainer').hide();

                    $('#dvHeader').html("Call Logs : " + $('#tdstudentName').html())


                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#23a7e3");
                }

                if (elmId == "btnHome") {

                }

            });

         
                      

        });

    </script>


   
                    
                    <div class="middleContainer">

                        <div id="content">
                        </div>

                    </div>



                    <div class="rightContainer">
                        <div class="imgcontainer">
                        </div>
                        <div id="calender" style="float: left; font-size: 12px; margin: 5px 5px 5px 0px; position: relative; width: 160px;"></div>

                    </div>

           