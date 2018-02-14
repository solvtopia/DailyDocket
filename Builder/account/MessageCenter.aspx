<%@ Page Title="Message Center" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="MessageCenter.aspx.vb" Inherits="DailyDocket.Builder.MessageCenter" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .wrap-table-profile {
            width: 100%;
            /*display: block;*/
            margin: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version
            <asp:Label runat="server" ID="lblSSL" Text="s" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="..Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li class="active">Message Center</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="Clients" PageViewID="RadPageView1" Selected="true">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Send Messages" PageViewID="RadPageView2">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="My Notification History" PageViewID="RadPageView8">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Settings" PageViewID="RadPageView3">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" SelectedIndex="0" runat="server" Width="100%">
        <telerik:RadPageView ID="RadPageView1" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <table class="wrap-table-profile">
                <tr>
                    <td style="vertical-align: top; padding-right: 10px;">
                        <telerik:RadListBox ID="lstClients" runat="server" Skin="Metro" Width="100%" AutoPostBack="true" Height="300px">
                        </telerik:RadListBox>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:Panel runat="server" ID="pnlClientRecord">
                            <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage2" SelectedIndex="0" Width="100%">
                                <Tabs>
                                    <telerik:RadTab runat="server" Text="Contact Information" PageViewID="RadPageView4" Selected="true">
                                    </telerik:RadTab>
                                    <telerik:RadTab runat="server" Text="Notification History" PageViewID="RadPageView5">
                                    </telerik:RadTab>
                                </Tabs>
                            </telerik:RadTabStrip>
                            <telerik:RadMultiPage ID="RadMultiPage2" SelectedIndex="0" runat="server" Width="100%">
                                <telerik:RadPageView ID="RadPageView4" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
                                    <table class="wrap-table-profile">
                                        <tr>
                                            <td>Name</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadTextBox ID="txtName" runat="server" Skin="Metro" Width="100%">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Mobile Number</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadTextBox ID="txtMobileNumber" runat="server" Skin="Metro" Width="100%" EmptyMessage="Used For SMS Notifications">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Email Address</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadTextBox ID="txtEmail" runat="server" Skin="Metro" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Address Line 1</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadTextBox ID="txtAddress1" runat="server" Skin="Metro" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Address Line 2</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadTextBox ID="txtAddress2" runat="server" Skin="Metro" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Zip Code</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadTextBox ID="txtZipCode" runat="server" Skin="Metro" Width="100%" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>City, State</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadTextBox ID="txtCity" runat="server" Skin="Metro" Width="100%" Enabled="false" />
                                                ,
                    <telerik:RadDropDownList ID="ddlState" runat="server" Enabled="False" Skin="Metro">
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
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSaveClient" runat="server" Text="Save Changes" />
                                            </td>
                                        </tr>
                                    </table>
                                </telerik:RadPageView>
                                <telerik:RadPageView ID="RadPageView5" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
                                    <telerik:RadAjaxPanel ID="RadAjaxPanel5" runat="server" Width="100%" Height="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
                                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" Height="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
                                            <telerik:RadMultiPage runat="server" ID="RadMultiPage3" Width="100%" Height="350px" SelectedIndex="0">
                                                <telerik:RadPageView runat="server" ID="fraClientSMSLog" Height="350px" />
                                            </telerik:RadMultiPage>
                                        </telerik:RadAjaxPanel>
                                    </telerik:RadAjaxPanel>
                                </telerik:RadPageView>
                            </telerik:RadMultiPage>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <table class="wrap-table-profile">
                <tr>
                    <td style="vertical-align: top;">To</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlTo" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <telerik:RadTabStrip ID="RadTabStrip3" runat="server" MultiPageID="RadMultiPage4" SelectedIndex="0" Width="100%">
                <Tabs>
                    <telerik:RadTab runat="server" Text="Send via Email" PageViewID="RadPageView6" Selected="true">
                    </telerik:RadTab>
                    <telerik:RadTab runat="server" Text="Send via SMS" PageViewID="RadPageView7">
                    </telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="RadMultiPage4" SelectedIndex="0" runat="server" Width="100%">
                <telerik:RadPageView ID="RadPageView6" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
                    <table class="wrap-table-profile">
                        <tr>
                            <td style="vertical-align: top;">Subject</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtSubject" Width="100%" Skin="Metro" /></td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;">Message</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadEditor RenderMode="Auto" runat="server" ID="RadEditor1"
                                    Height="575px" Width="100%" Skin="Metro" ImageManager-ViewPaths="~/images/uploads" ImageManager-DeletePaths="~/images/uploads" ImageManager-UploadPaths="~/images/uploads" ImageManager-MaxUploadFileSize="1024000">
                                </telerik:RadEditor>
                            </td>
                        </tr>
                    </table>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView7" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
                    <table class="wrap-table-profile">
                        <tr>
                            <td style="vertical-align: top;">Message (max 160 characters)</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtSMS" Rows="5" TextMode="MultiLine" Width="100%" MaxLength="160" Skin="Metro" />
                            </td>
                        </tr>
                    </table>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
            <table class="wrap-table-profile">
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnSend" Text="Send" />&nbsp;<asp:Label runat="server" ID="lblError" />
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView3" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <table class="wrap-table-profile">
                <tr>
                    <td>Mobile Number(s) <em>One Per Line</em></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadTextBox ID="txtMobileNumbers" runat="server" Skin="Metro" Width="100%" Rows="4" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
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
                    <td>
                        <asp:Button ID="btnSaveSettings" runat="server" Text="Save Changes" />
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView8" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <table class="wrap-table-profile">
                <tr>
                    <td>
                        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Width="100%" Height="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
                            <telerik:RadMultiPage runat="server" ID="RadMultiPage5" Width="100%" Height="350px" SelectedIndex="0">
                                <telerik:RadPageView runat="server" ID="fraAttorneySMSLog" Height="350px" />
                            </telerik:RadMultiPage>
                        </telerik:RadAjaxPanel>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
