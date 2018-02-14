<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Msg.aspx.vb" Inherits="DailyDocket.Builder.Msg" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width: 100%;" cellpadding="1" cellspacing="2">
                <tr>
                    <td colspan="3" style="vertical-align: top;">The Profile screen is used to view and adjust your personal settings for accessing the Daily Docket project.</td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td style="padding: 20px; vertical-align: top; width: 20%;">&nbsp;</td>
                    <td style="padding: 20px; vertical-align: top;">This screen is divided into 4 sections:<br />
                        <br />
                        <ul>
                            <li>User Details, used to manage the email address password used to access the system as well as mobile numbers for receiving SMS Notifications.</li>
                            <li>Attorney Record, displays the information we have automatically collected from the State Bar Association based on your Bar Number</li>
                            <li>Billing Information, we store a Credit/Debit Card on file for you for processing your monthly subscription fee.&nbsp; Please note that a valid Credit/Debit Card is required or your account will be placed in a Billing Lock and you will not be able to access the system.</li>
                            <li>Settings, used to manage your notification settings</li>
                        </ul>
                        </td>
                    <td style="padding: 20px; vertical-align: top; width: 20%;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="padding: 20px; vertical-align: top; width: 20%;">You can specify multiple mobile numbers to receive SMS Notifications by entering one number per line in the Mobile Number(s) box on the User Details tab.</td>
                    <td style="padding: 20px; vertical-align: top; text-align: center;">
                        <img alt="" style="width: 1024px; height: 520px;" src="/images/uploads/profile_help_screenshot.png" /></td>
                    <td style="padding: 20px; vertical-align: top; width: 20%;">Service may be canceled at any time from this screen by clicking on the Cancel My Service tab and then the Cancel My Service button at the bottom of the screen.</td>
                </tr>
            </table>
            <p>&nbsp;</p>
            <p>&nbsp;</p>
        </div>
    </form>
</body>
</html>
