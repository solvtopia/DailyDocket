<%@ Page Title="DailyDocket.CommonCore &gt; Objects &gt; MobileWorkOrder" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="MobileWorkOrder.aspx.vb" Inherits="DailyDocket.API._MobileWorkOrder" %>

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
                        <td style="vertical-align: top; width: 25%;">ID</td>
                        <td style="vertical-align: top;">Integer</td>
                        <td style="vertical-align: top; width: 50%;">Database Record ID</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">xmlData</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">Full Xml for the Work Order record</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">MobileDeviceId</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">DeviceId for the Technician selected on the Work Order</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">ModuleID</td>
                        <td style="vertical-align: top;">Integer</td>
                        <td style="vertical-align: top; width: 50%;">Database Record ID of the Module this Work Order was created in.</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">FolderID</td>
                        <td style="vertical-align: top;">Integer</td>
                        <td style="vertical-align: top; width: 50%;">Database Record ID for the Parent Folder of the Work Order&#39;s Module.</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">ModuleName</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">Name of the Module this Work Order was created in.</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">ModuleIcon</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">Full Url to the Icon image used for the Work Order&#39;s Module.</td>
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
                <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="GetOpenWorkOrders_M.aspx" ForeColor="Black">GetOpenWorkOrders_M</asp:HyperLink>
                &nbsp;routine in the
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
                <asp:HyperLink ID="HyperLink11" runat="server" NavigateUrl="GetOpenWorkOrders_M.aspx" ForeColor="Black">GetOpenWorkOrders_M</asp:HyperLink>
                &nbsp;routine in the
                <asp:HyperLink ID="HyperLink12" runat="server" NavigateUrl="OutputController.aspx" ForeColor="Black">OutputController</asp:HyperLink>
                &nbsp;for code samples</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="informationInfoContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        This object is used to return Work Order details from the
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="OutputController.aspx">OutputController</asp:HyperLink>
        &nbsp;service.
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
