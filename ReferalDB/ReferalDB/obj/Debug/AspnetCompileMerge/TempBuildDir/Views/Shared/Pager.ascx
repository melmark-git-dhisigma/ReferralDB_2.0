<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ReferalDB.Models.PagingModel>" %>

<div class="pager" style="float: left;">
<%
   // Create Previous link
    Html.RenderPartial("PagerLink", Model, new ViewDataDictionary { { "Text", "Previous " }, { "PageIndex", Model.CurrentPageIndex - 1 }, { "Selected", false }, { "Inactive", Model.CurrentPageIndex == 1 || Model.CurrentPageIndex == 0 } });
   // Ajax.ActionLink("Next+++", "PagerLink", new { PageIndex = Model.CurrentPageIndex + 1, Selected = false, Inactive = Model.CurrentPageIndex == Model.PageCount }, new AjaxOptions { UpdateTargetId = "" });
    for (var i = 1; i <= Model.PageCount; i++)
    {

        Html.RenderPartial("PagerLink", Model, new ViewDataDictionary { { "Text", "   " + i + "   " }, { "PageIndex", Model.CurrentPageIndex - Model.CurrentPageIndex + i }, { "Selected", false }, { "Inactive", Model.CurrentPageIndex == i } });
    } 
   // Create Next link
   Html.RenderPartial("PagerLink", Model, new ViewDataDictionary { { "Text", " Next" }, { "PageIndex", Model.CurrentPageIndex + 1 }, { "Selected", false }, { "Inactive", Model.CurrentPageIndex == Model.PageCount } });
%>
</div>
