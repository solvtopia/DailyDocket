<%@ Page Title="New User Registration" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="Register.aspx.vb" Inherits="DailyDocket.Builder.Register" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .wrap-table-login {
            width: 100%;
        }

        .rbLinkButton.fixedWidth {
            padding: 0;
            top: 0px;
            left: 1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version <asp:Label runat="server" ID="lblSSL" Text="s" /></small>
    </h1>
    <ol class="breadcrumb">
        <li class="active">New User Registration</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" Width="100%" Height="650px" SelectedIndex="0">
        <telerik:RadPageView runat="server" ID="fraRegister" Height="650px" ContentUrl="~/account/Registration.aspx" />
    </telerik:RadMultiPage>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
