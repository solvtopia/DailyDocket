<%@ Page Title="DailyDocket.CommonCore &gt; Enums &gt; SystemUserPermissions" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="SystemUserPermissions.aspx.vb" Inherits="DailyDocket.API.SystemUserPermissions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="informationHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="informationContent_Ajax" runat="server">
    <table class="auto-style1">
        <tr>
            <td style="width: 20%;">Namespace</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">DailyDocket.CommonCore.Enums</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 20%; vertical-align: top;">Values</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">
                <table cellpadding="0" cellspacing="0" class="auto-style1">
                    <tr>
                        <td style="vertical-align: top; width: 25%;">Administrator</td>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">Solvtopia</td>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">Supervisor</td>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">SystemAdministrator</td>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">Technician</td>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">User</td>
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
                &nbsp;&nbsp;&nbsp; If usr.Permissions = UWOutput.SystemUserPermissions.Administrator Then
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &#39; your code goes here ...
                <br />
                &nbsp;&nbsp;&nbsp; End If
                <br />
                Else Me.lblError.Text = resp.responseMessage
                <br />
                End If </td>
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
                &nbsp;&nbsp;&nbsp; if (usr.Permissions == UWOutput.SystemUserPermissions.Administrator) { 
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; // your code goes here ... 
                <br />
                &nbsp;&nbsp;&nbsp; } 
                <br />
                } else { 
                <br />
                &nbsp;&nbsp;&nbsp; this.lblError.Text = resp.responseMessage; 
                <br />
                }</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="informationInfoContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        Enumeration used for the Permissions property within the
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="SystemUser.aspx">SystemUser</asp:HyperLink>
        &nbsp;object.
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
