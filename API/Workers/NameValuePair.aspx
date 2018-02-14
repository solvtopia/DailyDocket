<%@ Page Title="DailyDocket.CommonCore &gt; Objects &gt; NameValuePair" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="NameValuePair.aspx.vb" Inherits="DailyDocket.API._NameValuePair" %>

<asp:Content ID="Content1" ContentPlaceHolderID="informationHeadContent" runat="server">
    <style type="text/css">
        .auto-style2 {
            font-family: "Courier New", Courier, monospace;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="informationContent_Ajax" runat="server">
    <table class="auto-style1">
        <tr>
            <td style="width: 20%;">Namespace</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">DailyDocket.CommonCore.Objects</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 20%; vertical-align: top;">Constructors</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">
                <table cellpadding="0" cellspacing="0" class="auto-style1">
                    <tr>
                        <td style="vertical-align: top; width: 25%;">New(string, string)</td>
                        <td style="vertical-align: top;">n</td>
                        <td style="vertical-align: top; width: 50%;">String</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">&nbsp;</td>
                        <td style="vertical-align: top;">v</td>
                        <td style="vertical-align: top; width: 50%;">String</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 20%; vertical-align: top;">Properties</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">
                <table cellpadding="0" cellspacing="0" class="auto-style1">
                    <tr>
                        <td style="vertical-align: top; width: 25%;">Name</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">value</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">Sample VB.NET Code</td>
        </tr>
        <tr>
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;" class="auto-style2">&#39; See the
                <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="UpdateCustomerRecord.aspx" ForeColor="Black">UpdateCustomerRecord</asp:HyperLink>
                &nbsp;routine in the
                <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="InputController.aspx" ForeColor="Black">InputController</asp:HyperLink>
                &nbsp;for code samples</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">Sample C#.NET Code</td>
        </tr>
        <tr>
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px; font-family: 'Courier New', Courier, monospace;">// See the
                <asp:HyperLink ID="HyperLink17" runat="server" NavigateUrl="UpdateCustomerRecord.aspx" ForeColor="Black">UpdateCustomerRecord</asp:HyperLink>
                &nbsp;routine in the
                <asp:HyperLink ID="HyperLink18" runat="server" NavigateUrl="InputController.aspx" ForeColor="Black">InputController</asp:HyperLink>
                &nbsp;for code samples</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="informationInfoContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        This Public Structure is used to store Name/Value pairs in a list collection within the
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="UpdateCustomerRecord.aspx">UpdateCustomerRecord</asp:HyperLink>
        &nbsp;method of the
        <asp:HyperLink ID="HyperLink16" runat="server" NavigateUrl="InputController.aspx">InputController</asp:HyperLink>
        .
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
