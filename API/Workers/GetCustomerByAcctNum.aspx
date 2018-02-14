<%@ Page Title="DailyDocket.API &gt; OutputController &gt; GetCustomerByAcctNum" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="GetCustomerByAcctNum.aspx.vb" Inherits="DailyDocket.API.GetCustomerByAcctNum" %>

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
                        <td style="vertical-align: top; width: 25%;">apiKey</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">Hashed response based on the UserEmail and UserPassword provided to the
                            <asp:HyperLink ID="HyperLink2" runat="server" ForeColor="Black" NavigateUrl="GetApiKey.aspx">GetApiKey</asp:HyperLink>
                            &nbsp;routine.</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">acctNum</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">Account Number from the billing system.</td>
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
                            <asp:HyperLink ID="HyperLink3" runat="server" ForeColor="Black" NavigateUrl="ApiResponse.aspx">ApiResponse</asp:HyperLink>
                            &nbsp;&gt;&nbsp;
                            <asp:HyperLink ID="HyperLink4" runat="server" ForeColor="Black" NavigateUrl="CustomerRecord.aspx">CustomerRecord</asp:HyperLink>
                        </td>
                        <td style="vertical-align: top;">Record details for the associated Customer</td>
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
                Dim resp As UWOutput.ApiResponse = svcOut.GetCustomerByAcctNum(&quot;ApiKey&quot;, &quot;Cust-Acct-Num&quot;)
                <br />
                <br />
                If resp.responseCode = UWOutput.ApiResultCode.success Then
                <br />
                &nbsp;&nbsp;&nbsp; Dim cust As UWOutput.CustomerRecord = CType(resp.responseObject, UWOutput.CustomerRecord)
                <br />
                &nbsp;&nbsp;&nbsp; &#39; your code goes here ...
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
                UWOutput.ApiResponse resp = svcOut.GetCustomerByAcctNum(&quot;ApiKey&quot;, &quot;Cust-Acct-Num&quot;);
                <br />
                <br />
                if (resp.responseCode == UWOutput.ApiResultCode.success) {
                <br />
                &nbsp;&nbsp;&nbsp; UWOutput.CustomerRecord cust = (UWOutput.CustomerRecord)resp.responseObject;
                <br />
                &nbsp;&nbsp;&nbsp; // your code goes here ...
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
        This routine is used to return a single Customer record from the database by the billing system Account Number.
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
