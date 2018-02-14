<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Availability.aspx.vb" Inherits="DailyDocket.Builder.Availability" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Daily Docket by Solvtopia, LLC.</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <style type="text/css">
        .wrap-table-login {
            width: 100%;
        }

        .rbLinkButton.fixedWidth {
            padding: 0;
        }

        body {
            font-family: "Source Sans Pro", sans-serif;
            color: #fff;
            /*font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;*/
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="MainAjaxPanel" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
            <table class="wrap-table-login">
                <tr>
                    <td>Daily Docket is currently processing cases for the following counties:</td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;</td>
                </tr>
            </table>
            <asp:Table runat="server" ID="tblCounties" Width="100%" CellPadding="1" CellSpacing="2" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
