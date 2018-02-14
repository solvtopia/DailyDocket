<%@ Page Title="DailyDocket.API &gt; InputController &gt; CloseWorkOrder_M" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="CloseWorkOrder_M.aspx.vb" Inherits="DailyDocket.API.CloseWorkOrder_M" %>

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
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">DailyDocket.API</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 20%;">Service Name</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">
                <asp:HyperLink ID="HyperLink1" runat="server" ForeColor="Black" NavigateUrl="InputController.aspx">InputController</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td style="width: 20%;">Service Url</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">https://Api.dailycourtdocket.com/InputController.asmx</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 20%; vertical-align: top;">Input Parameters</td>
            <td style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;">
                <table cellpadding="0" cellspacing="0" class="auto-style1">
                    <tr>
                        <td style="vertical-align: top; width: 25%;">deviceId</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">Unique value used to identify a mobile device</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">id</td>
                        <td style="vertical-align: top;">Integer</td>
                        <td style="vertical-align: top; width: 50%;">Work Order record ID from the Utility Wizards System</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">comments</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">Technician Comments to be saved with the Work Order</td>
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
                            &nbsp;&gt; Boolean</td>
                        <td style="vertical-align: top;">True upon success or False when an exception has been encountered</td>
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
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px;" class="auto-style2">Dim svcIn As New UWInput.InputController
                <br />
                Dim resp As UWInput.ApiResponse = svcIn.CloseWorkOrder_M(&quot;UniqueDeviceId&quot;, 123, &quot;Comments from Mobile App&quot;)
                <br />
                <br />
                If resp.responseCode = UWInput.ApiResultCode.success Then
                <br />
                &nbsp;&nbsp;&nbsp; &#39; your code here ...
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
            <td colspan="2" style="border: 1px solid #C0C0C0; vertical-align: top; background-color: #E6E6E6; padding-left: 10px; font-family: 'Courier New', Courier, monospace;">UWInput.InputController svcIn = new UWInput.InputController();
                <br />
                UWInput.ApiResponse resp = svcIn.CloseWorkOrder_M(&quot;UniqueDeviceId&quot;, 123, &quot;Comments from Mobile App&quot;);
                <br />
                <br />
                if (resp.responseCode == UWInput.ApiResultCode.success) {
                <br />
                &nbsp;&nbsp;&nbsp; // your code here ...
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
    <asp:Panel runat="server" ID="pnlInfo">
        This routine is used to post Technician Comments and automatically update the Work Order status to Completed via a mobile device.<br />
        <br />
        Upon successful login the system will return a
        <asp:HyperLink ID="HyperLink4" runat="server" ForeColor="White" NavigateUrl="SystemUser.aspx">SystemUser</asp:HyperLink>
        &nbsp;object containing details for the user that was retrieved.<br />
        <br />
        Since this is a &quot;mobile-only&quot; routine an ApiKey is not required, however a deviceId must be provided as an input parameter.
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
