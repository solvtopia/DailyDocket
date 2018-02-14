<%@ Page Title="Sales Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="SalesReport.aspx.vb" Inherits="DailyDocket.Builder.SalesReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/ContentPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version <asp:Label runat="server" ID="lblSSL" Text="s" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="../Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li>Administrator Options</li>
        <li class="active">Sales Report</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="Cases by Attorney" PageViewID="RadPageView1" Selected="true">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Cases by County" PageViewID="RadPageView2">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" SelectedIndex="0" runat="server" Width="100%">
        <telerik:RadPageView ID="RadPageView1" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" DataSourceID="SqlDataSource1" GroupPanelPosition="Top" RenderMode="Auto" Skin="Metro" PageSize="30">
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
                    <asp:Parameter DefaultValue="0" Name="topCount" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <telerik:RadGrid ID="RadGrid2" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" DataSourceID="SqlDataSource2" GroupPanelPosition="Top" RenderMode="Auto" Skin="Metro" CellSpacing="-1" GridLines="Both" PageSize="30">
                <MasterTableView AutoGenerateColumns="False" DataSourceID="SqlDataSource2">
                    <Columns>
                        <telerik:GridBoundColumn DataField="CivilCount" DataType="System.Int32" FilterControlAltText="Filter CivilCount column" HeaderText="Civil Count" SortExpression="CivilCount" UniqueName="CivilCount" AllowFiltering="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CriminalCount" FilterControlAltText="Filter CriminalCount column" HeaderText="Criminal Count" SortExpression="CriminalCount" UniqueName="CriminalCount" DataType="System.Int32" AllowFiltering="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="State" FilterControlAltText="Filter State column" HeaderText="State" SortExpression="State" UniqueName="State">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="County" FilterControlAltText="Filter County column" HeaderText="County" SortExpression="County" UniqueName="County">
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <FilterMenu RenderMode="Auto">
                </FilterMenu>
                <HeaderContextMenu RenderMode="Auto">
                </HeaderContextMenu>
            </telerik:RadGrid>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:DailyDocketConnectionString %>" SelectCommand="SELECT [CivilCount], [CriminalCount], [State], [County] FROM [vwCountySales] ORDER BY [TotalCount] DESC"></asp:SqlDataSource>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
