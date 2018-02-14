<%@ Page Title="DailyDocket.CommonCore &gt; Objects &gt; CustomerRecord" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="CustomerRecord.aspx.vb" Inherits="DailyDocket.API._CustomerRecord" %>

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
                    <tr>
                        <td style="vertical-align: top; width: 25%;">New(integer)</td>
                        <td style="vertical-align: top;">id</td>
                        <td style="vertical-align: top; width: 50%;">Integer</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">New(string)</td>
                        <td style="vertical-align: top;">custAcctNum</td>
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
                        <td style="vertical-align: top; width: 25%;">ID</td>
                        <td style="vertical-align: top;">Integer</td>
                        <td style="vertical-align: top; width: 50%;">Database Record ID</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">Name</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">AccountNumber</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">CustomValues</td>
                        <td style="vertical-align: top;">List(Of
    <asp:HyperLink ID="HyperLink14" runat="server" NavigateUrl="NameValuePair.aspx" ForeColor="Black">NameValuePair</asp:HyperLink>
                            )</td>
                        <td style="vertical-align: top; width: 50%;">Allows for Custom Fields to be added to the Customer Record</td>
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
    <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="GetAllCustomers.aspx" ForeColor="Black">GetAllCustomers</asp:HyperLink>
                &nbsp;and
    <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="GetCustomerByAcctNum.aspx" ForeColor="Black">GetCustomerByAcctNum</asp:HyperLink>
                &nbsp;routines in the
    <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="OutputController.aspx" ForeColor="Black">OutputController</asp:HyperLink>
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
    <asp:HyperLink ID="HyperLink11" runat="server" NavigateUrl="GetAllCustomers.aspx" ForeColor="Black">GetAllCustomers</asp:HyperLink>
                &nbsp;and
    <asp:HyperLink ID="HyperLink12" runat="server" NavigateUrl="GetCustomerByAcctNum.aspx" ForeColor="Black">GetCustomerByAcctNum</asp:HyperLink>
                &nbsp;routines in the
    <asp:HyperLink ID="HyperLink13" runat="server" NavigateUrl="OutputController.aspx" ForeColor="Black">OutputController</asp:HyperLink>
                &nbsp;for code samples</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="informationInfoContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        This object is used to return Customer details from the
    <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="OutputController.aspx">OutputController</asp:HyperLink>
        &nbsp;service as well as is specified as an input parameter in the
        <asp:HyperLink ID="HyperLink15" runat="server" NavigateUrl="UpdateCustomerRecord.aspx">UpdateCustomerRecord</asp:HyperLink>
        &nbsp;method of the
        <asp:HyperLink ID="HyperLink16" runat="server" NavigateUrl="InputController.aspx">InputController</asp:HyperLink>
        .</asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
