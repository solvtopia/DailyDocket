<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Registration.aspx.vb" Inherits="DailyDocket.Builder.Registration" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Daily Docket by Solvtopia, LLC.</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <!-- Bootstrap 3.3.6 -->
    <link rel="stylesheet" href="../bootstrap/css/bootstrap.min.css" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css" />
    <!-- Ionicons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css" />
    <!-- jvectormap -->
    <link rel="stylesheet" href="../plugins/jvectormap/jquery-jvectormap-1.2.2.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="../dist/css/AdminLTE.min.css" />
    <!-- AdminLTE Skins. Choose a skin from the css/skins
       folder instead of downloading all of them to reduce the load. -->
    <link rel="stylesheet" href="../dist/css/skins/skin-red.min.css" />
    <style type="text/css">
        .wrap-table-login {
            width: 100%;
        }

        .rbLinkButton.fixedWidth {
            padding: 0;
        }
    </style>

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadAjaxLoadingPanel ID="MainAjaxLoadingPanel" runat="server" Skin="Metro">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="MainAjaxPanel" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
            <asp:Panel runat="server" ID="pnlStep1">
                <table class="wrap-table-login">
                    <tr>
                        <td><strong>Welcome to Daily Docket!</strong><br />
                            <br />
                            Lets start your registration now!<br />
                            <br />
                            First, lets see if we can find your information automatically ... Please enter your bar number or first name, last name, and state below and we&#39;ll look it up ...</td>
                    </tr>
                    <tr>
                        <td>&nbsp;&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Bar Number</td>
                    </tr>
                    <tr>
                        <td style="margin-left: 40px">
                            <telerik:RadTextBox ID="txtBarNumber" runat="server" Skin="Metro" Width="100%">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;&nbsp;</td>
                    </tr>
                </table>
                <asp:Panel runat="server" ID="pnlHidden1" Visible="false">
                    <table class="wrap-table-login">
                        <tr>
                            <td>- OR -</td>
                        </tr>
                        <tr>
                            <td>&nbsp;&nbsp;</td>
                        </tr>
                        <tr>
                            <td>First Name</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtFirstName" runat="server" Skin="Metro" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Last Name</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtLastName" runat="server" Skin="Metro" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>State</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadDropDownList ID="ddlState" runat="server" Skin="Metro" Width="100%">
                                    <Items>
                                        <telerik:DropDownListItem Value="AL" Text="Alabama" />
                                        <telerik:DropDownListItem Value="AK" Text="Alaska" />
                                        <telerik:DropDownListItem Value="AZ" Text="Arizona" />
                                        <telerik:DropDownListItem Value="AR" Text="Arkansas" />
                                        <telerik:DropDownListItem Value="CA" Text="California" />
                                        <telerik:DropDownListItem Value="CO" Text="Colorado" />
                                        <telerik:DropDownListItem Value="CT" Text="Connecticut" />
                                        <telerik:DropDownListItem Value="DC" Text="District of Columbia" />
                                        <telerik:DropDownListItem Value="DE" Text="Delaware" />
                                        <telerik:DropDownListItem Value="FL" Text="Florida" />
                                        <telerik:DropDownListItem Value="GA" Text="Georgia" />
                                        <telerik:DropDownListItem Value="HI" Text="Hawaii" />
                                        <telerik:DropDownListItem Value="ID" Text="Idaho" />
                                        <telerik:DropDownListItem Value="IL" Text="Illinois" />
                                        <telerik:DropDownListItem Value="IN" Text="Indiana" />
                                        <telerik:DropDownListItem Value="IA" Text="Iowa" />
                                        <telerik:DropDownListItem Value="KS" Text="Kansas" />
                                        <telerik:DropDownListItem Value="KY" Text="Kentucky" />
                                        <telerik:DropDownListItem Value="LA" Text="Louisiana" />
                                        <telerik:DropDownListItem Value="ME" Text="Maine" />
                                        <telerik:DropDownListItem Value="MD" Text="Maryland" />
                                        <telerik:DropDownListItem Value="MA" Text="Massachusetts" />
                                        <telerik:DropDownListItem Value="MI" Text="Michigan" />
                                        <telerik:DropDownListItem Value="MN" Text="Minnesota" />
                                        <telerik:DropDownListItem Value="MS" Text="Mississippi" />
                                        <telerik:DropDownListItem Value="MO" Text="Missouri" />
                                        <telerik:DropDownListItem Value="MT" Text="Montana" />
                                        <telerik:DropDownListItem Value="NE" Text="Nebraska" />
                                        <telerik:DropDownListItem Value="NV" Text="Nevada" />
                                        <telerik:DropDownListItem Value="NH" Text="New Hampshire" />
                                        <telerik:DropDownListItem Value="NJ" Text="New Jersey" />
                                        <telerik:DropDownListItem Value="NM" Text="New Mexico" />
                                        <telerik:DropDownListItem Value="NY" Text="New York" />
                                        <telerik:DropDownListItem Value="NC" Text="North Carolina" />
                                        <telerik:DropDownListItem Value="ND" Text="North Dakota" />
                                        <telerik:DropDownListItem Value="OH" Text="Ohio" />
                                        <telerik:DropDownListItem Value="OK" Text="Oklahoma" />
                                        <telerik:DropDownListItem Value="OR" Text="Oregon" />
                                        <telerik:DropDownListItem Value="PA" Text="Pennsylvania" />
                                        <telerik:DropDownListItem Value="RI" Text="Rhode Island" />
                                        <telerik:DropDownListItem Value="SC" Text="South Carolina" />
                                        <telerik:DropDownListItem Value="SD" Text="South Dakota" />
                                        <telerik:DropDownListItem Value="TN" Text="Tennessee" />
                                        <telerik:DropDownListItem Value="TX" Text="Texas" />
                                        <telerik:DropDownListItem Value="UT" Text="Utah" />
                                        <telerik:DropDownListItem Value="VT" Text="Vermont" />
                                        <telerik:DropDownListItem Value="VA" Text="Virginia" />
                                        <telerik:DropDownListItem Value="WA" Text="Washington" />
                                        <telerik:DropDownListItem Value="WV" Text="West Virginia" />
                                        <telerik:DropDownListItem Value="WI" Text="Wisconsin" />
                                        <telerik:DropDownListItem Value="WY" Text="Wyoming" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <table class="wrap-table-login">
                    <tr>
                        <td>
                            <telerik:RadButton ID="btnFind" runat="server" Text="Look Me Up!" ButtonType="LinkButton" Skin="Metro" Width="100%" CssClass="fixedWidth" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlStep2">
                <asp:Panel runat="server" ID="pnlFound">
                    <table class="wrap-table-login">
                        <tr>
                            <td>This is what we found ... Let us know if it&#39;s you or not?</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Bar Number</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblBarNumber" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Name</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Address</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblAddress" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Work Phone</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblWorkPhone" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Email</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblEmail" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>License Date</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblLicenseDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadButton ID="btnThatsMe" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="That's Me!" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadButton ID="btnNotMe" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="That's NOT Me!" Width="100%" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlNotFound">
                    <table class="wrap-table-login">
                        <tr>
                            <td>Oops! We couldn&#39;t find any records using the information you specified.&nbsp; Click the button below to Try Again ...</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadButton ID="btnTryAgain" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Try Again ..." Width="100%" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlStep3">
                <table class="wrap-table-login">
                    <tr>
                        <td>Great!&nbsp; We took the liberty of searching our database and found
                            <asp:Label ID="lblCaseCount" runat="server"></asp:Label>
                            case(s) for you.<br />
                            <br />
                            The last step is creating a user and we&#39;re all set!&nbsp; Please fill out the form below and click Sign Me Up!<br />
                            <br />
                            A base fee of <span style="text-align: center;">
                                <asp:Label ID="lblMonthlyCharge1" runat="server" />
                            </span>&nbsp;will be charged for your first month&#39;s service.&nbsp; You will receive a confirmation receipt once your Credit/Debit Card has been successfully charged.<br />
                            <br />
                            <asp:Panel ID="pnlErrors" runat="server">
                                We found the following errors while processing your signup ... Please fix these and try again<br />
                                <br />
                                <span style="color: #CC0000">
                                    <asp:Literal ID="litErrors" runat="server"></asp:Literal></span>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Email</td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox ID="txtEmail" runat="server" Skin="Metro" Width="100%">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Password</td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox ID="txtPassword" runat="server" Skin="Metro" Width="100%" TextMode="Password">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Confirm Password</td>
                    </tr>
                    <tr>
                        <td style="margin-left: 40px">
                            <telerik:RadTextBox ID="txtConfirm" runat="server" Skin="Metro" Width="100%" TextMode="Password">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Mobile Number (Used For SMS Notifications)</td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox ID="txtMobileNumber" runat="server" Skin="Metro" Width="100%">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <asp:Panel runat="server" ID="pnlBilling" Visible="false">
                    <table class="wrap-table-login">
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Billing Information</td>
                        </tr>
                        <tr>
                            <td>Address Line 1</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtAddress1" runat="server" Skin="Metro" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Address Line 2</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtAddress2" runat="server" Skin="Metro" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Zip Code</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtZipCode" runat="server" Skin="Metro" Width="100%" AutoPostBack="true">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>City</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtCity" runat="server" Skin="Metro" Width="100%" Enabled="False">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>State</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadDropDownList ID="ddlBillingState" runat="server" Skin="Metro" Width="100%" Enabled="False">
                                    <Items>
                                        <telerik:DropDownListItem Text="Alabama" Value="AL" />
                                        <telerik:DropDownListItem Text="Alaska" Value="AK" />
                                        <telerik:DropDownListItem Text="Arizona" Value="AZ" />
                                        <telerik:DropDownListItem Text="Arkansas" Value="AR" />
                                        <telerik:DropDownListItem Text="California" Value="CA" />
                                        <telerik:DropDownListItem Text="Colorado" Value="CO" />
                                        <telerik:DropDownListItem Text="Connecticut" Value="CT" />
                                        <telerik:DropDownListItem Text="District of Columbia" Value="DC" />
                                        <telerik:DropDownListItem Text="Delaware" Value="DE" />
                                        <telerik:DropDownListItem Text="Florida" Value="FL" />
                                        <telerik:DropDownListItem Text="Georgia" Value="GA" />
                                        <telerik:DropDownListItem Text="Hawaii" Value="HI" />
                                        <telerik:DropDownListItem Text="Idaho" Value="ID" />
                                        <telerik:DropDownListItem Text="Illinois" Value="IL" />
                                        <telerik:DropDownListItem Text="Indiana" Value="IN" />
                                        <telerik:DropDownListItem Text="Iowa" Value="IA" />
                                        <telerik:DropDownListItem Text="Kansas" Value="KS" />
                                        <telerik:DropDownListItem Text="Kentucky" Value="KY" />
                                        <telerik:DropDownListItem Text="Louisiana" Value="LA" />
                                        <telerik:DropDownListItem Text="Maine" Value="ME" />
                                        <telerik:DropDownListItem Text="Maryland" Value="MD" />
                                        <telerik:DropDownListItem Text="Massachusetts" Value="MA" />
                                        <telerik:DropDownListItem Text="Michigan" Value="MI" />
                                        <telerik:DropDownListItem Text="Minnesota" Value="MN" />
                                        <telerik:DropDownListItem Text="Mississippi" Value="MS" />
                                        <telerik:DropDownListItem Text="Missouri" Value="MO" />
                                        <telerik:DropDownListItem Text="Montana" Value="MT" />
                                        <telerik:DropDownListItem Text="Nebraska" Value="NE" />
                                        <telerik:DropDownListItem Text="Nevada" Value="NV" />
                                        <telerik:DropDownListItem Text="New Hampshire" Value="NH" />
                                        <telerik:DropDownListItem Text="New Jersey" Value="NJ" />
                                        <telerik:DropDownListItem Text="New Mexico" Value="NM" />
                                        <telerik:DropDownListItem Text="New York" Value="NY" />
                                        <telerik:DropDownListItem Text="North Carolina" Value="NC" />
                                        <telerik:DropDownListItem Text="North Dakota" Value="ND" />
                                        <telerik:DropDownListItem Text="Ohio" Value="OH" />
                                        <telerik:DropDownListItem Text="Oklahoma" Value="OK" />
                                        <telerik:DropDownListItem Text="Oregon" Value="OR" />
                                        <telerik:DropDownListItem Text="Pennsylvania" Value="PA" />
                                        <telerik:DropDownListItem Text="Rhode Island" Value="RI" />
                                        <telerik:DropDownListItem Text="South Carolina" Value="SC" />
                                        <telerik:DropDownListItem Text="South Dakota" Value="SD" />
                                        <telerik:DropDownListItem Text="Tennessee" Value="TN" />
                                        <telerik:DropDownListItem Text="Texas" Value="TX" />
                                        <telerik:DropDownListItem Text="Utah" Value="UT" />
                                        <telerik:DropDownListItem Text="Vermont" Value="VT" />
                                        <telerik:DropDownListItem Text="Virginia" Value="VA" />
                                        <telerik:DropDownListItem Text="Washington" Value="WA" />
                                        <telerik:DropDownListItem Text="West Virginia" Value="WV" />
                                        <telerik:DropDownListItem Text="Wisconsin" Value="WI" />
                                        <telerik:DropDownListItem Text="Wyoming" Value="WY" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Credit Card Type</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadDropDownList ID="ddlCCType" runat="server" Skin="Metro" Width="100%">
                                    <Items>
                                        <telerik:DropDownListItem Text="American Express" Value="3" />
                                        <telerik:DropDownListItem Text="Discover" Value="2" />
                                        <telerik:DropDownListItem Text="MasterCard" Value="1" />
                                        <telerik:DropDownListItem Text="Visa" Value="0" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Credit Card Number</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtCCNumber" runat="server" Skin="Metro" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Expiration Date</td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" class="wrap-table-login">
                                    <tr>
                                        <td style="width: 49%;">
                                            <telerik:RadDropDownList ID="ddlExpMonth" runat="server" Skin="Metro" Width="100%">
                                                <Items>
                                                    <telerik:DropDownListItem Text="January" Value="01" />
                                                    <telerik:DropDownListItem Text="February" Value="02" />
                                                    <telerik:DropDownListItem Text="March" Value="03" />
                                                    <telerik:DropDownListItem Text="April" Value="04" />
                                                    <telerik:DropDownListItem Text="May" Value="05" />
                                                    <telerik:DropDownListItem Text="June" Value="06" />
                                                    <telerik:DropDownListItem Text="July" Value="07" />
                                                    <telerik:DropDownListItem Text="August" Value="08" />
                                                    <telerik:DropDownListItem Text="September" Value="09" />
                                                    <telerik:DropDownListItem Text="October" Value="10" />
                                                    <telerik:DropDownListItem Text="November" Value="11" />
                                                    <telerik:DropDownListItem Text="December" Value="12" />
                                                </Items>
                                            </telerik:RadDropDownList>
                                        </td>
                                        <td style="text-align: center;">/</td>
                                        <td style="width: 49%;">
                                            <telerik:RadDropDownList ID="ddlExpYear" runat="server" Skin="Metro" Width="100%">
                                                <Items>
                                                </Items>
                                            </telerik:RadDropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>CVV Code</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtCVV" runat="server" Skin="Metro" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table class="wrap-table-login">
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadButton ID="btnSignUp" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Sign Me Up!" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadButton ID="btnStartOver" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Start Over" Width="100%" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlDone">
                <span style="text-align: center; font-weight: bold; font-size: 12pt;">Registration complete!</span>
                <br />
                <br />
                <span style="text-align: center;">Congratulations! You have completed the registration process and your account has been created.<br />
                    <br />
                    You have been registered for a 7 day demo. If you entered Credit/Debit Card information during registration you will be charged for the monthly service amount of
                    <asp:Label runat="server" ID="lblMonthlyCharge" />&nbsp;when your Demo period expires.<br />
                    <br />
                    You will be contacted shortly by one of our sales representatives just in case you have any questions or need assistance.<br />
                    <br />
                    Download our App (<a href="market://details?id=com.solvtopia.dailydocket" target="_blank">Google Play</a> / <a href='itms-apps://itunes.apple.com/us/app/daily-docket/id1200313091?ls=1&mt=8'>Apple App Store</a>).<br />
                    <br />
                    Thank you for your interest in Daily Docket by Solvtopia, LLC!!<br />
                    <br />
                    <telerik:RadButton ID="btnLogin" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Show My AOC Calendar Now" Width="100%" />
                </span>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlHidden" Visible="false" BackColor="#CC0000">
                <asp:Label runat="server" ID="lblClientID" />
                <asp:Label ID="lblUserID" runat="server" />
            </asp:Panel>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
