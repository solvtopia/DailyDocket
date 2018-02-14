<%@ Page Title="Help Editor" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="HelpEditor.aspx.vb" Inherits="DailyDocket.Builder.HelpEditor" %>

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
        <li>Help Topics</li>
        <li class="active">Editor</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="vertical-align: top;">Source</td>
        </tr>
        <tr>
            <td>
                <telerik:RadTextBox runat="server" ID="txtSource" Width="100%" Skin="Metro" /></td>
        </tr>
        <tr>
            <td style="vertical-align: top;">Content</td>
        </tr>
        <tr>
            <td>
                <telerik:RadEditor RenderMode="Auto" runat="server" ID="txtContent"
                    Height="575px" Width="100%" Skin="Metro" ImageManager-ViewPaths="~/images/uploads" ImageManager-DeletePaths="~/images/uploads" ImageManager-UploadPaths="~/images/uploads" ImageManager-MaxUploadFileSize="1024000">
                </telerik:RadEditor>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnSave" Text="Save Changes" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnCancel" Text="Cancel" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
