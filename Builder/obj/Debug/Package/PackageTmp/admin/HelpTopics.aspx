<%@ Page Title="Help Topics" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="HelpTopics.aspx.vb" Inherits="DailyDocket.Builder.HelpTopics" %>

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
        <li>Administrator Options</li>
        <li class="active">Help Topics</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="vertical-align: top;" colspan="2">Help Type</td>
        </tr>
        <tr>
            <td style="vertical-align: top;" colspan="2">
                <telerik:RadDropDownList runat="server" ID="ddlType" AutoPostBack="true">
                    <Items>
                        <telerik:DropDownListItem Text="Context Help" Value="help" />
                        <telerik:DropDownListItem Text="FAQ" Value="faq" />
                    </Items>
                </telerik:RadDropDownList>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top;" colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td style="vertical-align: top;">Topics</td>
            <td style="vertical-align: top; text-align: right;">
                <telerik:RadButton ID="btnAddTopic" runat="server" Text="Add Topic" Skin="Metro" />
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top;" colspan="2">
                <asp:Table ID="tblTopics" runat="server" CellPadding="1" CellSpacing="2" Width="100%" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
