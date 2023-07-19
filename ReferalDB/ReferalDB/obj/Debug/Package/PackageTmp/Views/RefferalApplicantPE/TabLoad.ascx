<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ReferalDB.Models.GenInfoModel>" %>


<script type="text/javascript">

    try {

        $("#frm1").validationEngine();
        $("#frm4").validationEngine();
        $("#frm5").validationEngine();
        $("#frm6").validationEngine();

        $('#frm1,#frm2,#frm3,#frm4,#frm5,#frm6,#frm7,#frm8').ajaxForm(options);




        var date = new Date();
        date.setDate(date.getDate());

        function PreventDef(e) {
            e.preventDefault();
        }

        $(".datepicker").datepicker(
            {
                changeMonth: true,
                changeYear: true,
                showAnim: "fadeIn",
                yearRange: 'c-100:c+100',
                maxDate: date,
                // dateFormat: "dd-mm-yy",
                //yearRange: 'c-113:c+27',

                /* fix buggy IE focus functionality */
                fixFocusIE: false,

            });
    }
    catch (ex)
    { }

</script>


<%=MvcHtmlString.Create(ViewBag.html)%>