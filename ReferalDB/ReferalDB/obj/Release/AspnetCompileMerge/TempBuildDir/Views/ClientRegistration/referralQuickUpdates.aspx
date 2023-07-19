<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>referralQuickUpdates</title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</head>
<body>
    <div>
        <fieldset>
            <legend>Student Details</legend>
            <div>

                <table class="auto-style1">
                    <tr>
                        <td>Student Id</td>
                        <td>
                            <input type="text" /></td>
                        <td rowspan="8" style="width: 25%;">&nbsp;</td>
                        <td>Street</td>
                        <td>
                            <input type="text" /></td>
                    </tr>
                    <tr>
                        <td>First Name</td>
                        <td>
                            <input type="text" /></td>
                        <td>Aprt</td>
                        <td>
                            <input type="text" /></td>
                    </tr>
                    <tr>
                        <td>Last Name</td>
                        <td>
                            <input type="text" /></td>
                        <td>City</td>
                        <td>
                            <input type="text" /></td>
                    </tr>
                    <tr>
                        <td>DOB</td>
                        <td>
                            <input type="text" /></td>
                        <td>State</td>
                        <td>
                            <input type="text" /></td>
                    </tr>
                    <tr>
                        <td>Gender</td>
                        <td>&nbsp;</td>
                        <td>Zip</td>
                        <td>
                            <input type="text" /></td>
                    </tr>
                    <tr>
                        <td>Date Of Referral</td>
                        <td>
                            <input type="text" /></td>
                        <td>Date Ref. Recieve Letter</td>
                        <td>
                            <input type="text" /></td>
                    </tr>
                    <tr>
                        <td>Status</td>
                        <td>&nbsp;</td>
                        <td>Funding Approved</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>

            </div>
        </fieldset>
    </div>
    <div class="bottomDiv">
        <div class="divContLog">
            <div class="divContLogSave">

                <table class="auto-style1">
                    <tr>
                        <td style="width: 10%">1/1/2015</td>
                        <td style="width: 20%">Misc Contact</td>
                        <td style="width: 40%">fadsfasdf faldsfjljfds faldskfj asdfasd fadsl fsd fsdd s fsdd fs fs fsd fsd fsf df sdf sfd fds f dsf sdf sdf s fsd fdsfsd </td>
                        <td style="width: 20%">user1</td>
                        <td style="width: 10%">Edit</td>
                    </tr>
                    <tr>
                        <td style="width: 10%">1/1/2015</td>
                        <td style="width: 20%">Misc Contact</td>
                        <td style="width: 40%">fadsfasdf faldsfjljfds faldskfj asdfasd fadsl fsd fsdd s fsdd fs fs fsd fsd fsf df sdf sfd fds f dsf sdf sdf s fsd fdsfsd </td>
                        <td style="width: 20%">user1</td>
                        <td style="width: 10%">Edit</td>
                    </tr>
                </table>

            </div>
            <div class="divContLogList">


                <table class="auto-style1">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Date</legend>
                                <input type="text" style="width: 100%;" />
                            </fieldset>
                        </td>
                        <td>
                            <fieldset>
                                <legend>Date</legend>
                                <input type="text" style="width: 100%;" />
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">

                            <fieldset><legend>Note</legend>
                            <textarea style="width: 100%; height: 100px;"></textarea>
                                </fieldset>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Date</legend>
                                <input style="width: 100%;" type="text" />
                            </fieldset>
                        </td>
                        <td>
                            <input type="button" value="Save" /><input type="button" value="Reset" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>


            </div>
        </div>
    </div>
</body>
</html>
