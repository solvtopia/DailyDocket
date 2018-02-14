<%@ Page Title="Manage Profile" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.Master" CodeBehind="Profile.aspx.vb" Inherits="DailyDocket.Builder.Profile" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .wrap-table-profile {
            width: 100%;
            margin: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version
            <asp:Label runat="server" ID="lblSSL" Text="s" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li class="active">Manage Profile</li>
    </ol>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="User Details" PageViewID="RadPageView1" Selected="true">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Attorney Record" PageViewID="RadPageView2">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Billing Information" PageViewID="RadPageView3">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Settings" PageViewID="RadPageView4">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Cancel My Service" PageViewID="RadPageView5">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" SelectedIndex="0" runat="server" Width="100%">
        <telerik:RadPageView ID="RadPageView1" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <table class="wrap-table-profile">
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <asp:Image ID="imgAvatar" runat="server" Height="80px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 25px;">
                        <asp:Image ID="imgHelp_Question" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
                    </td>
                    <td>Name</td>
                </tr>
                <tr>
                    <td style="width: 25px;">&nbsp;</td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
                    </td>
                    <td>Email Address</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <telerik:RadTextBox ID="txtEmail" runat="server" Skin="Metro" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
                    </td>
                    <td>Password</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <telerik:RadTextBox ID="txtPassword" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="Image8" runat="server" ImageUrl="~/images/toolbar/icon_help.png" ToolTip="Enter the wording for your question" />
                    </td>
                    <td>Mobile Number(s) <em>One Per Line</em></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <telerik:RadTextBox ID="txtMobileNumber" runat="server" Skin="Metro" Width="100%" Rows="4" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <table class="wrap-table-profile">
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>Information in the attorney record is gathered and updated automatically from the NC Bar Association.</td>
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
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView3" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <table class="wrap-table-profile">
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <asp:Panel runat="server" ID="pnlBillingLock">
                    <tr>
                        <td style="color: #CC0000">
                            <asp:Label runat="server" ID="lblBillingLock" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td>Billing Information</td>
                </tr>
                <tr>
                    <td>Name On Card</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadTextBox ID="txtNameCard" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadTextBox>
                    </td>
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
                        <telerik:RadTextBox ID="txtCity" runat="server" Enabled="False" Skin="Metro" Width="100%">
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
                                <td>
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
                                <td style="text-align: center;">&nbsp;/ &nbsp;</td>
                                <td>
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
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView4" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <table class="wrap-table-profile">
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>Notification Settings</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>Receive Notifications Via</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadDropDownList ID="ddlNotificationType" runat="server" AutoPostBack="True" Skin="Metro" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblNotificationLevel" runat="server" Text="Notification Level"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadDropDownList ID="ddlNotificationLevel" runat="server" AutoPostBack="True" Skin="Metro" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMaxSMSPerBatch" runat="server" Text="Maximum Notifications Per Batch"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadTextBox ID="txtMaxSMSPerBatch" runat="server" Skin="Metro" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>Mobile Device IDs</td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="litDeviceIDs" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView5" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <table style="width: 100%; margin: 10px;" cellpadding="1" cellspacing="2">
                <tr>
                    <td>We&#39;re sorry to see you go but respect your decision and want to say thank you for trying Daily Docket.&nbsp; We know it&#39;s an investment of your time, and we appreciate you giving us a chance.<br />
                        <br />
                        <span style="color: #cc0000">Please note, this can only be undone by the Solvtopia, LLC Support Team!<br />
                        </span>
                        <br />
                        <telerik:RadButton ID="btnCancelService" runat="server" Skin="Metro" Text="Cancel My Service" />
                        <br />
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
    <asp:Panel runat="server" ID="pnlHidden" Visible="false" BackColor="#CC0000">
        <asp:Label runat="server" ID="lblEditID" />
        <telerik:RadComboBox ID="ddlClient" runat="server" Skin="Metro" Width="100px">
        </telerik:RadComboBox>
        <asp:CheckBox ID="chkApiAccess" runat="server" Enabled="False" />
        <asp:CheckBox ID="chkWebAccess" runat="server" Enabled="False" />
        <telerik:RadTextBox ID="txtOneSignal" runat="server" Skin="Metro" Width="100px">
        </telerik:RadTextBox>
        <telerik:RadComboBox ID="ddlPermissions" runat="server" AutoPostBack="True" Enabled="False" Skin="Metro" Width="100px" />
        <telerik:RadComboBox ID="ddlSupervisor" runat="server" Enabled="False" Skin="Metro" Width="100px">
        </telerik:RadComboBox>
        <telerik:RadTextBox ID="txtDeviceID" runat="server" Enabled="False" Skin="Metro" Width="100px">
        </telerik:RadTextBox>
        <telerik:RadComboBox ID="ddlDeviceType" runat="server" AutoPostBack="True" Enabled="False" Skin="Metro" Width="100px" />
    </asp:Panel>
    <table>
        <tr>
            <td colspan="4">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 25px">&nbsp;</td>
            <td>
                <telerik:RadButton ID="btnSave" runat="server" Text="Save" Skin="Metro" />
            </td>
            <td>
                <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" Skin="Metro" />
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
