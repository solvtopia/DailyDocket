<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProfitChart.aspx.vb" Inherits="DailyDocket.Builder.ProfitChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Daily Docket by Solvtopia, LLC.</title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <link rel="stylesheet" href="../dist/css/AdminLTE.min.css" />
    <style type="text/css">
        .auto-style1 {
            width: 100%;
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
        <asp:Panel runat="server" ID="pnlChart">
            <table class="auto-style1">
                <tr>
                    <td>Monthly Daily Docket Attorney Subscription</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblAttorneyCost" Font-Bold="True" ForeColor="#25A0DA" />
                    </td>
                </tr>
                <tr>
                    <td>Monthly Billable Client Service Fees</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblClientFees" Font-Bold="True" ForeColor="#309B46" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadHtmlChart runat="server" ID="RadHtmlChart1" Width="100%" Height="350px" Skin="Metro">
                            <ChartTitle Text="">
                                <Appearance Align="Center" Position="Top" />
                            </ChartTitle>
                            <Legend>
                                <Appearance Visible="false" Position="Bottom" />
                            </Legend>
                        </telerik:RadHtmlChart>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>
