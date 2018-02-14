﻿<%@ Page Title="Frequently Asked Questions" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="FAQ.aspx.vb" Inherits="DailyDocket.Builder.FAQ" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version
            <asp:Label runat="server" ID="lblSSL" Text="s" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="../Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li>Documentation</li>
        <li class="active">Frequently Asked Questions</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <asp:Panel runat="server" ID="pnlScroll">
        <telerik:RadPanelBar RenderMode="Auto" runat="server" ID="RadPanelBar1" Width="100%" ExpandMode="SingleExpandedItem" AllowCollapseAllItems="true" Skin="Metro" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>