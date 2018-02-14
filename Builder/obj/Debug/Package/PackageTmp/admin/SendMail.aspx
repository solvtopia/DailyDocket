<%@ Page Title="System Messages" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="SendMail.aspx.vb" Inherits="DailyDocket.Builder.SendMail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version <asp:Label runat="server" ID="lblSSL" Text="s" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="../Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li>Administrator Options</li>
        <li class="active">System Messages</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="vertical-align: top;">To</td>
        </tr>
        <tr>
            <td>
                <telerik:RadDropDownList runat="server" ID="ddlTo" Width="100%" />
            </td>
        </tr>
        <tr>
            <td>CC</td>
        </tr>
        <tr>
            <td>
                <telerik:RadTextBox ID="txtCC" Runat="server" Width="100%">
                </telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>BCC</td>
        </tr>
        <tr>
            <td>
                <telerik:RadTextBox ID="txtBCC" Runat="server" Width="100%">
                </telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
    </table>
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="Send via Email" PageViewID="RadPageView1" Selected="true">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Send via SMS" PageViewID="RadPageView2">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" SelectedIndex="0" runat="server" Width="100%">
        <telerik:RadPageView ID="RadPageView1" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <table class="nav-justified">
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
                        <telerik:RadEditor RenderMode="Auto" runat="server" ID="txtEmail"
                            Height="575px" Width="100%" Skin="Metro">
                        </telerik:RadEditor>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <table class="nav-justified">
                <tr>
                    <td style="vertical-align: top;">Message (max 160 characters)</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSMS" Rows="10" TextMode="MultiLine" Width="100%" MaxLength="160" Skin="Metro" />
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
    <table class="nav-justified">
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnSend" Text="Send" />&nbsp;<asp:Label runat="server" ID="lblError" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
