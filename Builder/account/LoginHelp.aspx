<%@ Page Title="Forgot Password" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="LoginHelp.aspx.vb" Inherits="DailyDocket.Builder.LoginHelp" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .wrap-table-login {
            width: 100%;
        }

        .rbLinkButton.fixedWidth {
            padding: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version
            <asp:Label runat="server" ID="lblSSL" Text="s" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#">System Login</a></li>
        <li class="active">Forgot Password</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <telerik:RadAjaxPanel ID="MainAjaxPanel" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
        <table class="wrap-table-login">
            <tr>
                <td>Enter the mobile number or email address associated with your account below:</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Email Address</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtEmail" runat="server" AutoPostBack="True" Skin="Metro" Width="100%" />
                </td>
            </tr>
            <tr>
                <td>Mobile Number</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID="txtMobileNumber" runat="server" Skin="Metro" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton ID="btnLogin" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Style="left: 1px; top: 0px" Text="Get My Password" Width="100%" />
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Label ID="lblMessage" runat="server" ForeColor="#CC0000"></asp:Label>
                    <br />
                    <br />
                </td>
            </tr>
        </table>
    </telerik:RadAjaxPanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
