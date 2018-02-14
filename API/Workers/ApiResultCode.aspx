<%@ Page Title="DailyDocket.CommonCore &gt; Enums &gt; ApiResultCode" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="ApiResultCode.aspx.vb" Inherits="DailyDocket.API.ApiResultCode" %>

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
                        <td style="vertical-align: top; width: 25%;">success</td>
                        <td style="vertical-align: top;">&nbsp;</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">failed</td>
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
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px; font-family: 'Courier New', Courier, monospace;">Dim svcIn As New UWInput.InputController
            <br />
                Dim resp As UWInput.ApiResponse = svcIn.UpdateCustomerRecord(&quot;ApiKey&quot;, New UWInput.CustomerRecord)
            <br />
                <br />
                If resp.responseCode = UWInput.ApiResultCode.success Then
            <br />
                &nbsp;&nbsp;&nbsp; &#39; code for successful call goes here ...<br />
                Else Me.lblError.Text = resp.responseMessage<br />
                End If</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">Sample C#.NET Code</td>
        </tr>
        <tr>
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px; font-family: 'Courier New', Courier, monospace;">UWInput.InputController svcIn = new UWInput.InputController();
            <br />
                UWInput.ApiResponse resp = svcIn.UpdateCustomerRecord(&quot;ApiKey&quot;, new UWInput.CustomerRecord());
            <br />
                <br />
                if (resp.responseCode == UWInput.ApiResultCode.success) {
            <br />
                &nbsp;&nbsp;&nbsp; // code for successful call goes here ...<br />
                } else {<br />
                &nbsp;&nbsp;&nbsp; this.lblError.Text = resp.responseMessage<br />
                }</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="informationInfoContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        Enumeration used for the responseCode property within the
    <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="ApiResponse.aspx">ApiResponse</asp:HyperLink>
        &nbsp;object.<br />
        <br />
        When a responseCode of &quot;failed&quot; is returned the responseMessage property of the
        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="ApiResponse.aspx">ApiResponse</asp:HyperLink>
        &nbsp;will contain details of any exceptions or issues encountered in the routine.
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
