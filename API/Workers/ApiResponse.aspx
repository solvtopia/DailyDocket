<%@ Page Title="DailyDocket.CommonCore &gt; Objects &gt; ApiResponse" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="ApiResponse.aspx.vb" Inherits="DailyDocket.API._ApiResponse" %>

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
                        <td style="vertical-align: top; width: 25%;">New()</td>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
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
                        <td style="vertical-align: top; width: 25%;">responseCode</td>
                        <td style="vertical-align: top;">
                            <asp:HyperLink ID="HyperLink2" runat="server" ForeColor="Black" NavigateUrl="ApiResultCode.aspx">ApiResultCode</asp:HyperLink>
                        </td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">responseMessage</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">Details provided when the responseCode has a value of failed</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">responseObject</td>
                        <td style="vertical-align: top;">Object</td>
                        <td style="vertical-align: top; width: 50%;">Generic type to contain resulting items (see specific routines for Individal Types)</td>
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
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;" class="auto-style2">&#39; The ApiResponse object is used to return values from the
                <asp:HyperLink ID="HyperLink1" runat="server" ForeColor="Black" NavigateUrl="InputController.aspx">InputController</asp:HyperLink>
                &nbsp;and
                <asp:HyperLink ID="HyperLink7" runat="server" ForeColor="Black" NavigateUrl="OutputController.aspx">OutputController</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">Sample C#.NET Code</td>
        </tr>
        <tr>
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px; font-family: 'Courier New', Courier, monospace;">// The ApiResponse object is used to return values from the
                <asp:HyperLink ID="HyperLink8" runat="server" ForeColor="Black" NavigateUrl="InputController.aspx">InputController</asp:HyperLink>
                &nbsp;and
                <asp:HyperLink ID="HyperLink9" runat="server" ForeColor="Black" NavigateUrl="OutputController.aspx">OutputController</asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="informationInfoContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        This object is returned for calls to all API routines within both the
        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="InputController.aspx">InputController</asp:HyperLink>
        &nbsp;and
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="OutputController.aspx">OutputController</asp:HyperLink>
        &nbsp;services that return a value.<br />
        <br />
        See the Indiviual routines for specific return types within the responseObject property.
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
