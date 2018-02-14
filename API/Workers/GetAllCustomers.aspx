<%@ Page Title="DailyDocket.API &gt; OutputController &gt; GetAllCustomers" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="GetAllCustomers.aspx.vb" Inherits="DailyDocket.API.GetAllCustomers" %>

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
                            &nbsp;&gt; List(of
                            <asp:HyperLink ID="HyperLink4" runat="server" ForeColor="Black" NavigateUrl="CustomerRecord.aspx">CustomerRecord</asp:HyperLink>
                            )</td>
                        <td style="vertical-align: top;">List of all customers in the Utility Wizards system that can be accessed via the apiKey provided.</td>
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
                Dim resp As UWOutput.ApiResponse = svcOut.GetAllCustomers(&quot;ApiKey&quot;)
                <br />
                <br />
                If resp.responseCode = UWOutput.ApiResultCode.success Then
                <br />
                &nbsp;&nbsp;&nbsp; Dim lst As List(Of UWOutput.CustomerRecord) = CType(resp.responseObject, List(Of UWOutput.CustomerRecord))
                <br />
                &nbsp;&nbsp;&nbsp; For Each c As UWOutput.CustomerRecord In lst
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &#39; your code goes here ...
                <br />
                &nbsp;&nbsp;&nbsp; Next
                <br />
                Else Me.lblError.text = resp.responseMessage
                <br />
                End If
            </td>
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
                UWOutput.ApiResponse resp = svcOut.GetAllCustomers(&quot;ApiKey&quot;);
                <br />
                <br />
                if (resp.responseCode == UWOutput.ApiResultCode.success) {
                <br />
                &nbsp;&nbsp;&nbsp; List&lt;UWOutput.CustomerRecord&gt; lst = (List&lt;UWOutput.CustomerRecord&gt;)resp.responseObject;
                <br />
                &nbsp;&nbsp;&nbsp; foreach (UWOutput.CustomerRecord c in lst) {
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; // your code goes here ...
                <br />
                &nbsp;&nbsp;&nbsp; }
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
        This routine is used to get a list of all Customer records associated with the ApiKey provided.
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
