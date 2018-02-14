<%@ Page Title="System Logs" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="Logs.aspx.vb" Inherits="DailyDocket.Builder.Logs" %>

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
        <li class="active">System Logs</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="Errors" PageViewID="RadPageView1" Selected="true">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="History" PageViewID="RadPageView2">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" SelectedIndex="0" runat="server" Width="100%">
        <telerik:RadPageView ID="RadPageView1" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <table class="nav-justified">
                <tr>
                    <td>Project</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlProject" Width="100%" Skin="Metro" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" DataSourceID="SqlDataSource1" GroupPanelPosition="Top" RenderMode="Auto" Skin="Metro" PageSize="30" CellSpacing="-1" GridLines="Both">
                            <MasterTableView AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="UserID" FilterControlAltText="Filter UserID column" HeaderText="User ID" SortExpression="UserID" UniqueName="UserID" DataType="System.Int32">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter UserName column" HeaderText="Name" SortExpression="UserName" UniqueName="UserName">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Email" FilterControlAltText="Filter Email column" HeaderText="Email" SortExpression="Email" UniqueName="Email">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Password" FilterControlAltText="Filter Password column" HeaderText="Password" SortExpression="Password" UniqueName="Password">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ClientID" DataType="System.Int32" FilterControlAltText="Filter ClientID column" HeaderText="Client ID" SortExpression="ClientID" UniqueName="ClientID">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ClientName" FilterControlAltText="Filter ClientName column" HeaderText="Name" SortExpression="ClientName" UniqueName="ClientName">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Message" FilterControlAltText="Filter Message column" HeaderText="Message" SortExpression="Message" UniqueName="Message">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="dtInserted" DataType="System.DateTime" FilterControlAltText="Filter dtInserted column" HeaderText="Inserted" SortExpression="dtInserted" UniqueName="dtInserted">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <FilterMenu RenderMode="Auto">
                            </FilterMenu>
                            <HeaderContextMenu RenderMode="Auto">
                            </HeaderContextMenu>
                        </telerik:RadGrid>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DailyDocketConnectionString %>" SelectCommand="SELECT [UserID], [UserName], [Email], [Password], [ClientID], [ClientName], [Message], [dtInserted] FROM [vwErrorLog] WHERE ([Project] LIKE @Project) ORDER BY [dtInserted] DESC">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlProject" DefaultValue="builder" Name="Project" PropertyName="SelectedValue" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
            <telerik:RadGrid ID="RadGrid2" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" DataSourceID="SqlDataSource2" GroupPanelPosition="Top" RenderMode="Auto" Skin="Metro" PageSize="30" CellSpacing="-1" GridLines="Both">
                <MasterTableView AutoGenerateColumns="False" DataSourceID="SqlDataSource2">
                    <Columns>
                        <telerik:GridBoundColumn DataField="ItemText" FilterControlAltText="Filter ItemText column" HeaderText="Item" SortExpression="ItemText" UniqueName="ItemText">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="dtInserted" FilterControlAltText="Filter dtInserted column" HeaderText="Inserted" SortExpression="dtInserted" UniqueName="dtInserted" DataType="System.DateTime">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter UserName column" HeaderText="User" SortExpression="UserName" UniqueName="UserName">
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <FilterMenu RenderMode="Auto">
                </FilterMenu>
                <HeaderContextMenu RenderMode="Auto">
                </HeaderContextMenu>
            </telerik:RadGrid>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:DailyDocketConnectionString %>" SelectCommand="SELECT [ItemText], [dtInserted], [UserName] FROM [vwHistory] ORDER BY [dtInserted] DESC">
            </asp:SqlDataSource>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
