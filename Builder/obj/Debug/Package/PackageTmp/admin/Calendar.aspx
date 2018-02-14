<%@ Page Title="Attorney Calendar" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/ContentPage.master" CodeBehind="Calendar.aspx.vb" Inherits="DailyDocket.Builder.Calendar" %>

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
        <li class="active">Attorney Calendar</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <asp:PlaceHolder runat="server" ID="pnlDates">
        <table class="wrap-table">
            <tr>
                <td>Based on our records you are currently scheduled to appear in court on the following days:</td>
            </tr>
            <tr>
                <td>
                    <asp:Table ID="tblDates" runat="server" CellPadding="1" CellSpacing="2" Width="100%">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <telerik:RadGrid ID="RadSearchGridMobile" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="MetroTouch">
        <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true">
        </ClientSettings>
        <MasterTableView DataKeyNames="ID" ShowHeader="false">
            <GroupByExpressions>
                <telerik:GridGroupByExpression>
                    <SelectFields>
                        <telerik:GridGroupByField FieldAlias="Session" FieldName="GroupDate" FormatString="{0:D}"
                            HeaderValueSeparator=": "></telerik:GridGroupByField>
                    </SelectFields>
                    <GroupByFields>
                        <telerik:GridGroupByField FieldName="GroupDate" SortOrder="Ascending"></telerik:GridGroupByField>
                    </GroupByFields>
                </telerik:GridGroupByExpression>
            </GroupByExpressions>
            <Columns>
                <telerik:GridTemplateColumn HeaderText="Work Order Data" UniqueName="TemplateColumn">
                    <ItemTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <asp:Label ID='lblSubject' runat='server' Font-Bold='True' ForeColor='#27AAD0' Text='<%# Eval("Subject") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID='lblSessionDate' runat='server' ForeColor='#27AAD0' Text='<%# Eval("Start") %>'></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn HeaderText="ID" UniqueName="ID" DataField="ID" Display="false" />
                <telerik:GridBoundColumn HeaderText="Subject" UniqueName="Subject" DataField="Subject" Display="false" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <telerik:RadScheduler ID="radScheduler" runat="server" AllowDelete="False" AllowEdit="False" AllowInsert="False" DataKeyField="ID" DataSubjectField="Subject"
        DataStartField="Start" DataEndField="End" OverflowBehavior="Expand" SelectedView="MonthView" RenderMode="Auto">
    </telerik:RadScheduler>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
