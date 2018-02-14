<%@ Page Title="DailyDocket.CommonCore &gt; Objects &gt; ApiKeyResult" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="ApiKeyResult.aspx.vb" Inherits="DailyDocket.API._ApiKeyResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="informationHeadContent" runat="server">
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
                    <tr>
                        <td style="vertical-align: top; width: 25%;">New(string, string)</td>
                        <td style="vertical-align: top;">userEmail</td>
                        <td style="vertical-align: top; width: 50%;">String</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">&nbsp;</td>
                        <td style="vertical-align: top;">userPassword</td>
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
                        <td style="vertical-align: top; width: 25%;">UserEmail</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">UserPassword</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">ApiKey</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">Hashed response based on the UserEmail and UserPassword Provided</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">responseCode</td>
                        <td style="vertical-align: top;">
                            <asp:HyperLink ID="HyperLink1" runat="server" ForeColor="Black" NavigateUrl="ApiResultCode.aspx">ApiResultCode</asp:HyperLink>
                        </td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">responseMessage</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">Details provided when the responseCode has a value of failed</td>
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
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px; font-family: 'Courier New', Courier, monospace;">Dim svcOut As New UWOutput.OutputController<br />
                Dim key As String = svcOut.GetApiKey(&quot;user@email.com&quot;, &quot;U$3rP@$$w0rd&quot;).ApiKey</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">Sample C#.NET Code</td>
        </tr>
        <tr>
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px; font-family: 'Courier New', Courier, monospace;">UWOutput.OutputController svcOut = new UWOutput.OutputController();<br />
                string key = svcOut.GetApiKey(&quot;user@email.com&quot;, &quot;U$3rP@$$w0rd&quot;).ApiKey;</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="informationInfoContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        This object is used to return a hashed key based on a UserEmail and UserPassword provided in the New(string, string) constructor.<br />
        <br />
        An API key is required to access all Non-Mobile routines within both the
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="InputController.aspx">InputController</asp:HyperLink>
        &nbsp;and
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="OutputController.aspx">OutputController</asp:HyperLink>
        .
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
