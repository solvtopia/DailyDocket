<%@ Page Title="DailyDocket.API &gt; OutputController &gt; GetApiKey" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="GetApiKey.aspx.vb" Inherits="DailyDocket.API.GetApiKey" %>

<asp:Content ID="Content1" ContentPlaceHolderID="informationHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="informationContent_Ajax" runat="server">
    <table class="auto-style1">
        <tr>
            <td style="width: 20%;">Namespace</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">DailyDocket.API</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 20%;">Service Name</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">
                <asp:HyperLink ID="HyperLink1" runat="server" ForeColor="Black" NavigateUrl="OutputController.aspx">OutputController</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td style="width: 20%;">Service Url</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">https://Api.dailycourtdocket.com/OutputController.asmx</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 20%; vertical-align: top;">Input Parameters</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">
                <table cellpadding="0" cellspacing="0" class="auto-style1">
                    <tr>
                        <td style="vertical-align: top; width: 25%;">userEmail</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">userPassword</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 20%; vertical-align: top;">Return Value</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">
                <table cellpadding="0" cellspacing="0" class="auto-style1">
                    <tr>
                        <td style="vertical-align: top; width: 25%;">
                            <asp:HyperLink ID="HyperLink3" runat="server" ForeColor="Black" NavigateUrl="ApiKeyResult.aspx">ApiKeyResult</asp:HyperLink>
                        </td>
                        <td style="vertical-align: top;">&nbsp;</td>
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
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px; font-family: 'Courier New', Courier, monospace;">Dim svcOut As New UWOutput.OutputController
                <br />
                Dim key As String = svcOut.GetApiKey(&quot;user@email.com&quot;, &quot;U$3rP@$$w0rd&quot;).ApiKey</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">Sample C#.NET Code</td>
        </tr>
        <tr>
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px; font-family: 'Courier New', Courier, monospace;">UWOutput.OutputController svcOut = new UWOutput.OutputController();
                <br />
                string key = svcOut.GetApiKey(&quot;user@email.com&quot;, &quot;U$3rP@$$w0rd&quot;).ApiKey;</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="informationInfoContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        This routine is used to obtain a valid ApiKey which is used in all non-mobile routines in the
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="InputController.aspx">InputController</asp:HyperLink>
        &nbsp;and
        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="OutputController.aspx">OutputController</asp:HyperLink>
        .<br />
        <br />
        Mobile routines (designated by an _M in the routine name) do not require an ApiKey to validate access.
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
