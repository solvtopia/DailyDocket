<%@ Page Title="DailyDocket.API &gt; OutputController &gt; CheckLogin_M" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="CheckLogin_M.aspx.vb" Inherits="DailyDocket.API.CheckLogin_M" %>

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
                    <tr>
                        <td style="vertical-align: top; width: 25%;">deviceId</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">Unique value used to identify a mobile device</td>
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
                            <asp:HyperLink ID="HyperLink2" runat="server" ForeColor="Black" NavigateUrl="ApiResponse.aspx">ApiResponse</asp:HyperLink>
                            &nbsp;&gt;
            <asp:HyperLink ID="HyperLink3" runat="server" ForeColor="Black" NavigateUrl="SystemUser.aspx">SystemUser</asp:HyperLink>
                        </td>
                        <td style="vertical-align: top;">Object containing the details for the user upon successful login.</td>
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
                Dim resp As UWOutput.ApiResponse = svcOut.CheckLogin_M(&quot;user@email.com&quot;, &quot;U$3rP@$$w0rd&quot;, &quot;UniqueDeviceId&quot;)
                <br />
                <br />
                If resp.responseCode = UWOutput.ApiResultCode.success Then
            <br />
                &nbsp;&nbsp;&nbsp; Dim usr As UWOutput.SystemUser = CType(resp.responseObject, UWOutput.SystemUser)
            <br />
                Else Me.lblError.text = resp.responseMessage
            <br />
                End If</td>
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
                UWOutput.ApiResponse resp = svcOut.CheckLogin_M(&quot;user@email.com&quot;, &quot;U$3rP@$$w0rd&quot;, &quot;UniqueDeviceId&quot;);
                <br />
                <br />
                if (resp.responseCode == UWOutput.ApiResultCode.success) { 
            <br />
                &nbsp;&nbsp;&nbsp; UWOutput.SystemUser usr = (UWOutput.SystemUser)resp.responseObject; 
            <br />
                } else { 
            <br />
                &nbsp;&nbsp;&nbsp; this.lblError.text = resp.responseMessage; 
            <br />
                }</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="informationInfoContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        This routine is used to validate login credentials passed from a mobile device.<br />
        <br />
        Upon successful login the system will return a
        <asp:HyperLink ID="HyperLink4" runat="server" ForeColor="White" NavigateUrl="SystemUser.aspx">SystemUser</asp:HyperLink>
        &nbsp;object containing details for the user that was retrieved.<br />
        <br />
        Since this is a &quot;mobile-only&quot; routine an ApiKey is not required. The ApiKey provided as an input parameter is saved automatically with the User record for use in all mobile routines.
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
