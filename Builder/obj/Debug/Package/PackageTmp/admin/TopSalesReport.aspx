<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TopSalesReport.aspx.vb" Inherits="DailyDocket.Builder.TopSalesReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Daily Docket by Solvtopia, LLC.</title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <link rel="stylesheet" href="../dist/css/AdminLTE.min.css" />
    <style type="text/css">
        .wrap-table {
            width: 100%;
            display: block;
        }

            .wrap-table td {
                display: inline-block;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                </asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <div style="font-size: small;">
            <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="SqlDataSource1" GroupPanelPosition="Top" RenderMode="Auto" Skin="Metro" PageSize="30">
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="AttorneyID" DataSourceID="SqlDataSource1">
                    <Columns>
                        <telerik:GridBoundColumn AllowFiltering="false" DataField="AttorneyID" DataType="System.Int32" FilterControlAltText="Filter AttorneyID column" HeaderText="Attorney ID" ReadOnly="True" SortExpression="AttorneyID" UniqueName="AttorneyID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="xName" FilterControlAltText="Filter xName column" HeaderText="Name" SortExpression="xName" UniqueName="xName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn AllowFiltering="false" DataField="CivilCount" DataType="System.Int32" FilterControlAltText="Filter CivilCount column" HeaderText="Civil Count" SortExpression="CivilCount" UniqueName="CivilCount">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn AllowFiltering="false" DataField="CriminalCount" DataType="System.Int32" FilterControlAltText="Filter CriminalCount column" HeaderText="Criminal Count" SortExpression="CriminalCount" UniqueName="CriminalCount">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="xBarNumber" DataType="System.Int32" FilterControlAltText="Filter xBarNumber column" HeaderText="Bar Number" ReadOnly="True" SortExpression="xBarNumber" UniqueName="xBarNumber">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="xState" FilterControlAltText="Filter xState column" HeaderText="State" ReadOnly="True" SortExpression="xState" UniqueName="xState">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="xWorkPhone" FilterControlAltText="Filter xWorkPhone column" HeaderText="Work Phone" ReadOnly="True" SortExpression="xWorkPhone" UniqueName="xWorkPhone">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="xEmail" FilterControlAltText="Filter xEmail column" HeaderText="Email" ReadOnly="True" SortExpression="xEmail" UniqueName="xEmail">
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DailyDocketConnectionString %>" SelectCommand="procSalesReport" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter DefaultValue="20" Name="topCount" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
