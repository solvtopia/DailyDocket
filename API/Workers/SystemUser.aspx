<%@ Page Title="DailyDocket.CommonCore &gt; Objects &gt; SystemUser" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/InformationPopUp.master" CodeBehind="SystemUser.aspx.vb" Inherits="DailyDocket.API._SystemUser" %>

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
                        <td style="vertical-align: top; width: 25%;">New(string, string)</td>
                        <td style="vertical-align: top;">userEmail</td>
                        <td style="vertical-align: top; width: 50%;">String</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">&nbsp;</td>
                        <td style="vertical-align: top;">userPassword</td>
                        <td style="vertical-align: top; width: 50%;">String</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">New(string)</td>
                        <td style="vertical-align: top;">deviceId</td>
                        <td style="vertical-align: top; width: 50%;">String</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">New(integer)</td>
                        <td style="vertical-align: top;">userId</td>
                        <td style="vertical-align: top; width: 50%;">Integer</td>
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
                        <td style="vertical-align: top; width: 25%;">Email</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">Password</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">Permissions</td>
                        <td style="vertical-align: top;">
                            <asp:HyperLink ID="HyperLink13" runat="server" NavigateUrl="SystemUserPermissions.aspx" ForeColor="Black">SystemUserPermissions</asp:HyperLink>
                        </td>
                        <td style="vertical-align: top; width: 50%;">Permission level chosen during the User setup within the Utility Wizards System.</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">ImageUrl</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">MobileDeviceId</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">DeviceId associated with the user by the
                            <asp:HyperLink ID="HyperLink15" runat="server" NavigateUrl="CheckLogin_M.aspx" ForeColor="Black">CheckLogin_M</asp:HyperLink>
                            &nbsp;routine on a Mobile device.</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">MobileUserName</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">Used for Technician level permissions only to allow shorter login via Mobile devices.</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">Active</td>
                        <td style="vertical-align: top;">Boolean</td>
                        <td style="vertical-align: top; width: 50%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">ClientID</td>
                        <td style="vertical-align: top;">Integer</td>
                        <td style="vertical-align: top; width: 50%;">Database Record ID of the client the User is associated with in the Utility Wizards system.</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">ApiEnabled</td>
                        <td style="vertical-align: top;">Boolean</td>
                        <td style="vertical-align: top; width: 50%;">Set by Solvtopia Administrators to determine if API access is allowed for a given user.</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">IsAdminUser</td>
                        <td style="vertical-align: top;">Boolean (Read-Only)</td>
                        <td style="vertical-align: top; width: 50%;">Set automatically based on the User&#39;s permission level</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">IsSysAdmin</td>
                        <td style="vertical-align: top;">Boolean (Read-Only)</td>
                        <td style="vertical-align: top; width: 50%;">Set automatically based on the User&#39;s permission level</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">SupervisorID</td>
                        <td style="vertical-align: top;">Integer</td>
                        <td style="vertical-align: top; width: 50%;">Used for Technician level permissions only to allow one user to be associated with another on a supervisor-subordinate basis.</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">APIResponseCode</td>
                        <td style="vertical-align: top;">
                            <asp:HyperLink ID="HyperLink14" runat="server" NavigateUrl="ApiResultCode.aspx" ForeColor="Black">ApiResultCode</asp:HyperLink>
                        </td>
                        <td style="vertical-align: top; width: 50%;">Used in the New constructors to pass information to the system in the event of an error.</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 25%;">APIResponseMessage</td>
                        <td style="vertical-align: top;">String</td>
                        <td style="vertical-align: top; width: 50%;">Used in the New constructors to pass information to the system in the event of an error.</td>
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
                <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="CheckLogin_M.aspx" ForeColor="Black">CheckLogin_M</asp:HyperLink>
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
                <asp:HyperLink ID="HyperLink11" runat="server" NavigateUrl="CheckLogin_M.aspx" ForeColor="Black">CheckLogin_M</asp:HyperLink>
                &nbsp;routine in the
                <asp:HyperLink ID="HyperLink12" runat="server" NavigateUrl="OutputController.aspx" ForeColor="Black">OutputController</asp:HyperLink>
                &nbsp;for code samples</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="informationInfoContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        This object is used to return User details from the
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="OutputController.aspx">OutputController</asp:HyperLink>
        &nbsp;service.
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="informationContent_OutsideAjax" runat="server">
</asp:Content>
