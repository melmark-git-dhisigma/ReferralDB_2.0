<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ReferalDB.Models.RegistrationModel>" %>
<style type="text/css">
    div.middleContainer table tr td {
        border-bottom: 1px solid #DDDDDD;
        border-right: 1px solid #DDDDDD;
        color: #525252;
        font-family: Arial,Helvetica,sans-serif;
        font-size: 11px !important;
        font-weight: bold;
        padding: 8px 1px;
        width: 25%;
    }
</style>
<div style="width: 100%">
    <div>
        <table>

            <tr>
                <td class="">First Name </td>
                <td class="">
                    <%=Model.FirstName%></td>
                <td class="">Nick Name</td>
                <td class="nobdr">
                    <%=Model.NickName%></td>
            </tr>
            <tr>
                <td class="">Middle Name</td>
                <td class="">
                    <%=Model.MiddleName%></td>
                <td class="">Last Name </td>
                <td class="nobdr">
                    <%=Model.LastName%>
                    <%=Model.LastNameSuffix%>
                </td>
            </tr>
            <tr>
                <td class="">Admission Date </td>
                <td class="">
                    <%=Model.AdmissinDate%>
                <td class="">Gender</td>
                <%if (Model.Gender == "1")
                  { %>
                <td class="nobdr">Male
                </td>
                <%}
                  else if (Model.Gender == "2")
                  { %>
                <td class="nobdr">Female
                </td>
                <%} %>
            </tr>
            <tr>
                <td class="">Date of Birth </td>
                <td class="">
                    <%=Model.DateOfBirth%>
                <td class="">Race</td>
                <td class="nobdr">
                    <%=Model.StrRace%></td>
            </tr>
            <tr>
                <td class="">Place of Birth</td>
                <td class="">
                    <%=Model.PlaceOfBirth%></td>
                <td class="">Country of Birth</td>
                <td class="nobdr">
                    <%=Model.CountryBirth%></td>
            </tr>
            <tr>
                <td class="">State of Birth</td>
                <td class="">
                    <%=Model.StateBirth%></td>
                <td class="">Citizenship</td>
                <td class="nobdr">
                    <%=Model.CitizenshipBirth%></td>
            </tr>
            <tr>
                <td class="">Height (ft)</td>
                <td class="">
                    <%=Model.Height%></td>
                <td class="">Weight (lbs)</td>
                <td class="nobdr">
                    <%=Model.Weight%></td>
            </tr>
            <tr>
                <td class="">Hair Color</td>
                <td class="">
                    <%=Model.HairColor%></td>
                <td class="">Eye Color</td>
                <td class="nobdr">
                    <%=Model.EyeColor%></td>
            </tr>
            <tr>
                <td class="">Primary Language</td>
                <td class="">
                    <%=Model.PrimaryLanguage%></td>
                <td class="">Legal Competency Status</td>
                <td class="nobdr">
                    <%=Model.LegalCompetencyStatus%></td>
            </tr>
            <tr>
                <td class="">Guardianship Status</td>
                <td class="">
                    <%=Model.GuardianshipStatus%></td>
                <td class="">Other State Agencies Involved With Student</td>
                <td class="nobdr">
                    <%=Model.OtherStateAgenciesInvolvedWithStudent%></td>
            </tr>
            <tr>
                <td class="">Distigushing Marks</td>
                <td class="">
                    <%=Model.DistigushingMarks%></td>
                <td class="">Marital Status of Both Parents</td>
                <td class="nobdr">
                    <%=Model.MaritalStatusofBothParents%></td>
            </tr>
            <tr>
                <td class="">Case Manager Residential</td>
                <td class="">
                    <%=Model.CaseManagerResidential%></td>
                <td class="">Case Manager Educational</td>
                <td class="nobdr">
                    <%=Model.CaseManagerEducational%></td>
            </tr>
            <tr>
                <td class="" style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>Address Information</h4>
                </td>
            </tr>
            <tr>
                <td class="">Address Line 1 </td>
                <td class="">
                    <%=Model.AddressLine1%></td>
                <td class="">Address Line 2 </td>
                <td class="nobdr">
                    <%=Model.AddressLine2%></td>
            </tr>
            <tr>
                <td class="">Address Line 3 </td>
                <td class="">
                    <%=Model.AddressLine3%></td>
                <td class="">Country</td>
                <td class="nobdr">
                    <%=Model.StrCountry%></td>
            </tr>
            <tr>
                <td class="">State</td>
                <td class="">
                    <%=Model.StrState%></td>
                <td class="">City</td>
                <td class="nobdr">
                    <%=Model.City%></td>
            </tr>
            <tr>
                <td class="">Zip</td>
                <td class="">
                    <%=Model.ZipCode%></td>
                <td class="">&nbsp;</td>
                <td class="nobdr">&nbsp;</td>
            </tr>
            <tr>
                <td class="" style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>Emergency Contact - School</h4>
                </td>
            </tr>
            <%if (Model.EmergencyContactFirstName1 != null || Model.EmergencyContactLastName1 != null || Model.EmergencyContactPhone1 != null)
              { %>
            <tr>

                <td class="">First Name</td>
                <td class="">
                    <%=Model.EmergencyContactFirstName1%></td>
                <td class="">Last Name</td>
                <td class="nobdr"><%=Model.EmergencyContactLastName1%></td>
            </tr>
            <tr>
                <td class="">Title</td>
                <td class="">
                    <%=Model.EmergencyContactTitle1%></td>
                <td class="">Phone</td>
                <td class="nobdr"><%=Model.EmergencyContactPhone1%></td>
            </tr>
            <%} %>
            <%if (Model.EmergencyContactFirstName2 != null || Model.EmergencyContactLastName2 != null || Model.EmergencyContactPhone2 != null)
              { %>
            <tr>
                <td class="">First Name</td>
                <td class="">
                    <%=Model.EmergencyContactFirstName2%></td>
                <td class="">Last Name</td>
                <td class="nobdr"><%=Model.EmergencyContactLastName2%></td>
            </tr>
            <tr>
                <td class="">Title</td>
                <td class="">
                    <%=Model.EmergencyContactTitle2%></td>
                <td class="">Phone</td>
                <td class="nobdr"><%=Model.EmergencyContactPhone2%></td>
            </tr>
            <%} %>
            <%if (Model.EmergencyContactFirstName3 != null || Model.EmergencyContactLastName3 != null || Model.EmergencyContactPhone3 != null)
              { %>
            <tr>
                <td class="">First Name</td>
                <td class="">
                    <%=Model.EmergencyContactFirstName3%></td>
                <td class="">Last Name</td>
                <td class="nobdr"><%=Model.EmergencyContactLastName3%></td>
            </tr>
            <tr>
                <td class="">Title</td>
                <td class="">
                    <%=Model.EmergencyContactTitle3%></td>
                <td class="">Phone</td>
                <td class="nobdr"><%=Model.EmergencyContactPhone3%></td>
            </tr>
            <%} %>
            <%if (Model.EmergencyContactFirstName4 != null || Model.EmergencyContactLastName4 != null || Model.EmergencyContactPhone4 != null)
              { %>
            <tr>
                <td class="">First Name</td>
                <td class="">
                    <%=Model.EmergencyContactFirstName4%></td>
                <td class="">Last Name</td>
                <td class="nobdr"><%=Model.EmergencyContactLastName4%></td>
            </tr>
            <tr>
                <td class="">Title</td>
                <td class="">
                    <%=Model.EmergencyContactTitle4%></td>
                <td class="">Phone</td>
                <td class="nobdr"><%=Model.EmergencyContactPhone4%></td>
            </tr>
            <%} %>
            <%if (Model.EmergencyContactFirstName5 != null || Model.EmergencyContactLastName5 != null || Model.EmergencyContactPhone5 != null)
              { %>
            <tr>
                <td class="">First Name</td>
                <td class="">
                    <%=Model.EmergencyContactFirstName5%></td>
                <td class="">Last Name</td>
                <td class="nobdr"><%=Model.EmergencyContactLastName5%></td>
            </tr>
            <tr>
                <td class="">Title</td>
                <td class="">
                    <%=Model.EmergencyContactTitle5%></td>
                <td class="">Phone</td>
                <td class="nobdr"><%=Model.EmergencyContactPhone5%></td>
            </tr>
            <%} %>
            <tr>
                <td class="" style="border-right: none; padding-top: 10px;" style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>Schools Attended</h4>
                </td>
            </tr>
            <%if (Model.SchoolName1 != null || Model.DateFrom1 != null || Model.SchoolAttendedAddress11 != null)
              { %>
            <tr>
                <td class="">School Name</td>
                <td class="">
                    <%=Model.SchoolName1%></td>
                <td class="">Dates Attended<br />
                </td>
                <td class="nobdr">From: &nbsp<%=Model.DateFrom1%>&nbsp To:&nbsp<%=Model.DateTo1%> </td>
            </tr>
            <tr>
                <td class="">Address Line 1</td>
                <td class="">
                    <%=Model.SchoolAttendedAddress11%></td>
                <td class="">Address Line 2</td>
                <td class="nobdr"><%=Model.SchoolAttendedAddress21%></td>
            </tr>
            <tr>
                <td class="">City</td>
                <td class="">
                    <%=Model.SchoolAttendedCity1%></td>
                <td class="">State</td>
                <td class="nobdr"><%=Model.SchoolAttendedState1%></td>
            </tr>
            <%} %>
            <%if (Model.SchoolName2 != null || Model.DateFrom2 != null || Model.SchoolAttendedAddress12 != null)
              { %>
            <tr>
                <td class="">School Name</td>
                <td class="">
                    <%=Model.SchoolName2%></td>
                <td class="">Dates Attended<br />
                </td>
                <td class="nobdr">From:&nbsp<%=Model.DateFrom2%>&nbsp To:&nbsp<%=Model.DateTo2%></td>
            </tr>
            <tr>
                <td class="">Address Line 1</td>
                <td class="">
                    <%=Model.SchoolAttendedAddress12%></td>
                <td class="">Address Line 2</td>
                <td class="nobdr"><%=Model.SchoolAttendedAddress22%></td>
            </tr>
            <tr>
                <td class="">City</td>
                <td class="">
                    <%=Model.SchoolAttendedCity2%></td>
                <td class="">State</td>
                <td class="nobdr"><%=Model.SchoolAttendedState2%></td>
            </tr>
            <%} %>
            <%if (Model.SchoolName3 != null || Model.DateFrom3 != null || Model.SchoolAttendedAddress13 != null)
              { %>
            <tr>
                <td class="">School Name</td>
                <td class="">
                    <%=Model.SchoolName3%></td>
                <td class="">Dates Attended</td>
                <td class="nobdr">From:&nbsp
                    <%=Model.DateFrom3%>&nbsp
                    To:&nbsp
                    <%=Model.DateTo3%></td>
            </tr>
            <tr>
                <td class="">Address Line 1</td>
                <td class="">
                    <%=Model.SchoolAttendedAddress13%></td>
                <td class="">Address Line 2</td>
                <td class="nobdr"><%=Model.SchoolAttendedAddress23%></td>
            </tr>
            <tr>
                <td class="">City</td>
                <td class="">
                    <%=Model.SchoolAttendedCity3%></td>
                <td class="">State</td>
                <td class="nobdr"><%=Model.SchoolAttendedState3%></td>
            </tr>
            <%} %>

            <tr>
                <td style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>Insurance</h4>
                </td>
            </tr>







            <% if (Model.InsuranceList != null)
               {

                   foreach (var Ins in Model.InsuranceList)
                   {
            %>
            <tr>
                <td>Insurance Type</td>
                <td><%=Ins.InsuranceType%></td>
                <td>Policy Number</td>
                <td class="nobdr"><%=Ins.PolicyNumber%></td>
            </tr>

            <tr>
                <td>Policy Holder</td>
                <td><%=Ins.PolicyHolder%></td>
                <td>&nbsp;</td>
                <td class="nobdr">&nbsp;</td>
            </tr>



            <% 
           }
                   
            %>




            <%} %>






          <%--  <tr>
                <td>Insurance Type</td>
                <td><%=Model.InsuranceType%></td>
                <td>Policy Number</td>
                <td class="nobdr"><%=Model.PolicyNumber%></td>
            </tr>
            <tr>
                <td>Policy Holder</td>
                <td><%=Model.PolicyHolder%></td>
                <td>&nbsp;</td>
                <td class="nobdr">&nbsp;</td>
            </tr>--%>






















            <tr>
                <td style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>IEP / Referral Information</h4>
                </td>
            </tr>
            <tr>
                <td>Full Name</td>
                <td><%=Model.ReferralIEPFullName%></td>
                <td>Title</td>
                <td class="nobdr"><%=Model.ReferralIEPTitle%></td>
            </tr>
            <tr>
                <td>Phone</td>
                <td><%=Model.ReferralIEPPhone%></td>
                <td>Referring Agency</td>
                <td class="nobdr"><%=Model.ReferralIEPReferringAgency%></td>
            </tr>
            <tr>
                <td>Source Of Tuition</td>
                <td><%=Model.ReferralIEPSourceofTuition%></td>
                <td>&nbsp;</td>
                <td class="nobdr">&nbsp;</td>
            </tr>
            <tr>
                <td class="" style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>Education History</h4>
                </td>
            </tr>
            <tr>
                <td class="auto-style5">Date Initially Eligible for Special Education</td>
                <td class="auto-style6">
                    <%=Model.DateInitiallyEligibleforSpecialEducation%></td>
                <td class="auto-style7">Date of Most Recent Special Education Evaluations</td>
                <td class="nobdr">
                    <%=Model.DateofMostRecentSpecialEducationEvaluations%></td>
            </tr>
            <tr>
                <td class="">Date of Next Scheduled 3-Year Evaluation</td>
                <td class="">
                    <%=Model.DateofNextScheduled3YearEvaluation%></td>
                <td class="">Current IEP Start Date</td>
                <td class="nobdr">
                    <%=Model.CurrentIEPStartDate%></td>
            </tr>
            <tr>
                <td class="">Current IEP Expiration Date</td>
                <td class="">
                    <%=Model.CurrentIEPExpirationDate%></td>
                <td class="">&nbsp;</td>
                <td class="nobdr">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style9" style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>Discharge Information</h4>
                </td>
            </tr>
            <tr>
                <td class="">Discharge Date</td>
                <td class="">
                    <%=Model.DischargeDate%></td>
                <td class="">Location After Discharge</td>
                <td class="nobdr">
                    <%=Model.LocationAfterDischarge%></td>
            </tr>
            <tr>
                <td class="">Melmark New England&#39;s Follow Up Responsibilities</td>
                <td class="">
                    <%=Model.MelmarkNewEnglandsFollowUpResponsibilities%></td>
                <td class="">&nbsp;</td>
                <td class="nobdr">&nbsp;</td>
            </tr>

        </table>

    </div>
</div>

