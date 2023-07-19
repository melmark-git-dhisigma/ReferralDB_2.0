<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ReferalDB.Models.HomeDropDataViewModel>" %>

<link href="../../CSS/StyleControl.css" rel="stylesheet" />

<style type="text/css">
    .tdStyleInner {
        color: #6B6B6B;
        font-family: Arial,Helvetica,sans-serif;
        font-size: 12px;
    }

    .imgOver {
        height: 100px;
        width: 100px;
    }

    #page_navigation a {
        padding: 3px;
        border: 1px solid gray;
        margin: 2px;
        color: black;
        text-decoration: none;
    }

    .active_page {
        background: darkblue;
        color: white !important;
    }
</style>
<script type="text/javascript">
    $(document).ready(function () {
        $('#close_x').click(function () {
            $('#dialog').animate({ top: "-300%" }, function () {
                $('#overlay').fadeOut('slow');
            });
        });

        $('#slides').slides({
            preload: true,
            generateNextPrev: true
        });

        var itemCount = 0;
        var value = 'A'
        //$('#butnext').show();
        //$('#butprv').show();



        var flaglim = $('#flage').val();
        $('#butprv').hide();
        $('#butnext').hide();
        if (flaglim == ">") {
            $('#butnext').show();
        }
        if (flaglim == "<") {
            $('#butprv').show();
        }
        if (flaglim == "<>") {
            $('#butnext').show();
            $('#butprv').show();
        }

        $("#butnext,#butprv").on('click', function (e) {
            e.preventDefault();
            var sortArg = "";
            var data;
            var page = $('#curval').val();
            if ($(this).attr("name") == "next") {
                data = page + "*n";
            }
            else {
                data = page + "*p";
            }
            sortArg = 'A';

            window.location.href = "/Dashboard/ListDashboards?argument=" + sortArg + "&bSort=true &Data=" + data + "";
            //$("body").addClass("loading");

            //window.location = "../admin/userMangment?data=" + data;
        });

        itemCount = parseInt(3);

        if (itemCount == 0) {

            var styl = document.getElementById("noMatch").style;
            styl.display = "block";
        }




        /////////Paging//////////
        //how much items per page to show
        var show_per_page = "";
        if ($('.divActiveReferal').length > 0) {
            show_per_page = 10;
        }
        else {
            show_per_page = 5;
        }
        //getting the amount of elements inside content div
        var number_of_items = $('#DashbrdReferral').children().size();
        //calculate the number of pages we are going to have
        var number_of_pages = Math.ceil(number_of_items / show_per_page);

        //set the value of our hidden input fields
        $('#current_page').val(0);
        $('#show_per_page').val(show_per_page);

        //now when we got all we need for the navigation let's make it '

        /* 
        what are we going to have in the navigation?
            - link to previous page
            - links to specific pages
            - link to next page
        */
        var navigation_html = '<a class="previous_link" href="javascript:previous();">Prev</a>';
        var current_link = 0;
        while (number_of_pages > current_link) {
            navigation_html += '<a class="page_link" href="javascript:go_to_page(' + current_link + ')" longdesc="' + current_link + '">' + (current_link + 1) + '</a>';
            current_link++;
        }
        navigation_html += '<a class="next_link" href="javascript:next();">Next</a>';

        $('#page_navigation').html(navigation_html);

        //add active_page class to the first page link
        $('#page_navigation .page_link:first').addClass('active_page');

        //hide all the elements inside content div
        $('#DashbrdReferral').children().hide();

        $('#DashbrdReferral').children().slice(0, show_per_page).removeAttr("style")
        if ($('.divActiveReferal').length > 0) {
            $('#DashbrdReferral').children().slice(0, show_per_page).css("width", "48%")
            $('#DashbrdReferral').children().slice(0, show_per_page).css("float", "left")
            $('#DashbrdReferral').children().slice(0, show_per_page).css("margin-top", "10px")
            $('#DashbrdReferral').children().slice(0, show_per_page).css("margin-left", "10px")
        }
        else {
            $('#DashbrdReferral').children().slice(0, show_per_page).css("width", "100%")
        }



    });
    function previous() {
        $('#content').load('../Dashboard/GetLeftMenu');
        new_page = parseInt($('#current_page').val()) - 1;
        //if there is an item before the current active link run the function
        if ($('.active_page').prev('.page_link').length == true) {
            go_to_page(new_page);
        }

    }

    function next() {
        $('#content').load('../Dashboard/GetLeftMenu');
        new_page = parseInt($('#current_page').val()) + 1;
        //if there is an item after the current active link run the function
        if ($('.active_page').next('.page_link').length == true) {
            go_to_page(new_page);
        }

    }
    function go_to_page(page_num) {
        $('#content').load('../Dashboard/GetLeftMenu');
        //get the number of items shown per page
        var show_per_page = parseInt($('#show_per_page').val());

        //get the element number where to start the slice from
        start_from = page_num * show_per_page;

        //get the element number where to end the slice
        end_on = start_from + show_per_page;

        //hide all children elements of content div, get specific items and show them


        $('#DashbrdReferral').children().css("display", "none")


        //  $('#DashbrdReferral').children().css('display', 'none').slice(start_from, end_on).css('display', 'block');


        $('#DashbrdReferral').children().slice(start_from, end_on).removeAttr("style");
        if ($('.divActiveReferal').length > 0) {
            $('#DashbrdReferral').children().slice(start_from, end_on).css("width", "48%");
            $('#DashbrdReferral').children().slice(start_from, end_on).css("float", "left");
            $('#DashbrdReferral').children().slice(0, show_per_page).css("margin-top", "10px")
            $('#DashbrdReferral').children().slice(0, show_per_page).css("margin-left", "10px")
        }
        else {
            $('#DashbrdReferral').children().slice(start_from, end_on).css("width", "100%");
        }


        //   $('#DashbrdReferral').children().slice(0, show_per_page).removeAttr("style")





        /*get the page link that has longdesc attribute of the current page and add active_page class to it
        and remove that class from previously active page link*/
        $('.page_link[longdesc=' + page_num + ']').addClass('active_page').siblings('.active_page').removeClass('active_page');

        //update the current page input field
        $('#current_page').val(page_num);
    }

</script>

<form>
    <div>


        <table style="width: 100%;">


            <%if (Model.RefDetailsMore != null)
              {%>
            <%if (Model.RefDetailsMore.Count > 0)
              {%>
            <tr>
                <td>
                    <table style="width: 100%;" class="gridStyle">
                        <tr>
                            <td style="width: 25%;">
                                <%if (Model.RefDetailsMore[0].RefUrl == "")
                                  {
                                      if (Model.RefDetailsMore[0].RefGender == "1")
                                      {
                                %>
                                <img src="../../Images/Male.png" class="imgOver" />
                                <%   }
                                      else
                                      { 
                                %>
                                <img src="../../Images/Female.png" class="imgOver" />
                                <% }
                                  }
                                  else
                                  {
                                      string RefUrl = Model.RefDetailsMore[0].RefUrl;
                                %>
                                <img src="data:image/gif;base64,<%:RefUrl%>" class="imgOver" />
                                <% }
                            
                                %>
                            </td>
                            <td>

                                <table style="width: 100%;">
                                    <tr>


                                        <%if (Model.RefDetailsMore[0].SType == "Referral")
                                          { %>

                                        <td style="width: 105px">Referral Name </td>
                                        <%}
                                          else
                                          { %>
                                        <td style="width: 105px">Client Name </td>

                                        <%} %>
                                        <td><%:Model.RefDetailsMore[0].ReferralName%></td>
                                    </tr>
                                    <tr style="background-color: white;">
                                        <td>Current Activity </td>
                                        <td><%:Model.RefDetailsMore[0].CurrentStep%></td>
                                    </tr>
                                    <%-- <tr>
                                        <td>User </td>
                                            <td><%:Model.RefDetailsMore[0].UserName%></td>
                                    </tr>--%>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="gridStyle" style="width: 100%;">
                        <% foreach (var item in Model.RefDetailsMore)
                           {


                               string Status = "";
                               string Created = "";
                               string createdtime = "";

                               if (item.Status == "Y")
                               {
                                   Status = "Drafted By";
                                   Created = Convert.ToDateTime(@item.CreatedOn).ToString("MM'/'dd'/'yyyy");
                                   createdtime = Convert.ToDateTime(@item.CreatedOn).ToString("hh:mm tt");
                               }
                               else
                               {
                                   Status = "Submitted By";
                                   Created = Convert.ToDateTime(@item.ModifiedOn).ToString("MM'/'dd'/'yyyy");
                                   createdtime = Convert.ToDateTime(@item.ModifiedOn).ToString("hh:mm tt");
                               }
                               
                            
                               
                               
                        %>



                        <tr>

                            <td class="boxstyleCon" style="font-weight: normal;">


                                <%=item.CurrentStep %> <%=Status %> <%=item.UserName %>
                                <br />
                                (Created On  : <%=Created%> <%=createdtime%>)
                                    
                            </td>

                        </tr>
                        <%} %>
                    </table>


                </td>
            </tr>
        </table>




        <%} %>
        <%} %>



        <%--  <%if (Model.RefDetails.Count > 0)
          {%>
        <input type='hidden' id='current_page' />
        <input type='hidden' id='show_per_page' />
        <div style="width: 100%;" class="" id="DashbrdReferral">
            <% for (var i = 0; i < (Model.RefDetails.Count - 1); i = i + 2)
               { %>
            <table style="width: 100%;" class="tdStyleInner">
                <tr>
                    <td style="height: 100px; width: 33%">
                        <table>
                            <tr>
                                <td>
                                    <%if (Model.RefDetails[i].ImageUrl == "")
                                      {
                                          if (Model.RefDetails[i].Gender == "1")
                                          {
                                    %>
                                    <img src="../../Images/Male.png" class="imgOver" />
                                    <%   }
                                          else
                                          { 
                                    %>
                                    <img src="../../Images/Female.png" class="imgOver" />
                                    <% }
                                      }
                                      else
                                      {
                                          string RefUrl = Model.RefDetails[i].ImageUrl;
                                    %>
                                    <img src="data:image/gif;base64,<%:RefUrl%>" class="imgOver" />
                                    <% }
                            
                                    %>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="width: 65px">Name </td>
                                            <td><%:Model.RefDetails[i].ReferralName%></td>
                                        </tr>
                                        <tr>
                                            <td>Activity </td>
                                            <td><%:Model.RefDetails[i].QueueName%></td>
                                        </tr>
                                        <tr>
                                            <td>User </td>
                                            <td><%:Model.RefDetails[i].UserName%></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>

                    <td style="height: 100px; width: 33%">
                        <table>
                            <tr>
                                <td>

                                    <%if (Model.RefDetails[i + 1].ImageUrl == "")
                                      {
                                          if (Model.RefDetails[i + 1].Gender == "1")
                                          {
                                    %>
                                    <img src="../../Images/Male.png" class="imgOver" />
                                    <%   }
                                          else
                                          { 
                                    %>
                                    <img src="../../Images/Female.png" class="imgOver" />
                                    <% }
                                      }
                                      else
                                      {
                                          string URL = Model.RefDetails[i + 1].ImageUrl;
                                    %>
                                    <img src="data:image/gif;base64,<%:URL%>" class="imgOver" />
                                    <% }
                            
                                    %>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="width: 65px">Name</td>
                                            <td><%:Model.RefDetails[i+1].ReferralName%></td>
                                        </tr>
                                        <tr>
                                            <td>Activity</td>
                                            <td><%:Model.RefDetails[i+1].QueueName%></td>
                                        </tr>
                                        <tr>
                                            <td>User</td>
                                            <td><%:Model.RefDetails[i+1].UserName%></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <%}%>
        </div>
        <div class="clear"></div>
        <div id='page_navigation'></div>
        <%} %>--%>
    </div>

</form>
