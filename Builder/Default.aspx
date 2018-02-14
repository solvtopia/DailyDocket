<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/Dashboard.Master" CodeBehind="Default.aspx.vb" Inherits="DailyDocket.Builder._Default4" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/controls/RecordDetails.ascx" TagPrefix="docket" TagName="RecordDetails" %>

<%@ MasterType VirtualPath="~/masterPages/Dashboard.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script src="scripts/radWindowScripts.js"></script>
    <style type="text/css">
        .wrap-table {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version
            <asp:Label runat="server" ID="lblSSL" Text="s" /></small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
        <li class="active">Dashboard</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <asp:PlaceHolder runat="server" ID="pnlCalendar">
        <telerik:RadAjaxPanel ID="MainAjaxPanel" runat="server" Width="100%" Height="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
            <div class="row">
                <!-- Left col -->
                <div class="col-md-8">
                    <div class="row">
                        <div class="col-md-6">
                        </div>
                        <!-- /.col -->

                        <div class="col-md-6">
                        </div>
                        <!-- /.col -->
                    </div>
                    <!-- /.row -->

                    <!-- CALENDAR CONTENT -->
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <table>
                                <tr>
                                    <td style="width: 30px; margin-right: 10px; vertical-align: middle;">
                                        <asp:ImageButton runat="server" ID="ibtnRefresh" ImageUrl="~/images/toolbar/icon_refresh.png" /></td>
                                    <td style="vertical-align: middle;">
                                        <h3 class="box-title">My Calendar</h3>
                                    </td>
                                </tr>
                            </table>

                            <!-- box controls for minimize and close -->
                            <!-- <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div> -->
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <asp:PlaceHolder runat="server" ID="pnlDemo">
                                <table class="wrap-table">
                                    <tr>
                                        <td style="width: 30px; vertical-align: top;">
                                            <asp:Image runat="server" ID="Image2" ImageUrl="~/images/toolbar/icon_about.png" /></td>
                                        <td style="color: #CC0000; font-weight: bold">
                                            <asp:Label runat="server" ID="lblDemoMessage" /></td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server" ID="pnlConflict">
                                <table class="wrap-table">
                                    <tr>
                                        <td style="width: 30px; vertical-align: top;">
                                            <asp:Image runat="server" ID="imgConflict" ImageUrl="~/images/toolbar/icon_warning.png" /></td>
                                        <td style="color: #CC0000; font-weight: bold">We have detected the following possible conflicts on your calendar:
                                        <br />
                                            <br />
                                            <asp:Label runat="server" ID="lblConflict" /></td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server" ID="pnlMissingCounties">
                                <table class="wrap-table">
                                    <tr>
                                        <td style="width: 30px; vertical-align: top;">
                                            <asp:Image runat="server" ID="Image1" ImageUrl="~/images/toolbar/icon_about.png" /></td>
                                        <td style="color: #CC0000; font-weight: bold">We have searched your calendar and based on your cases, Civil records are currently unavailable in the following counties:
                                        <br />
                                            <br />
                                            <asp:Label runat="server" ID="lblMissingCounties" /></td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                            </asp:PlaceHolder>
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
                        </div>
                        <!-- /.box-body -->
                    </div>
                    <!-- /.box -->
                </div>
                <!-- /.col -->

                <div class="col-md-4">

                    <!-- CASE DETAILS -->
                    <asp:PlaceHolder runat="server" ID="pnlDetails">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">Case Details</h3>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                    <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                                </div>
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                <docket:RecordDetails runat="server" ID="RecordDetails" />
                            </div>
                            <!-- /.box-body -->
                        </div>
                    </asp:PlaceHolder>
                    <!-- /.box -->
                </div>
                <!-- /.col -->
            </div>
            <!-- /.row -->
        </telerik:RadAjaxPanel>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="pnlAdminDB">
        <asp:PlaceHolder runat="server" ID="pnlBadges">
            <!-- Small boxes (Stat box) -->
            <div class="row">
                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-aqua">
                        <div class="inner">
                            <h3>
                                <asp:Label runat="server" ID="lblTotalCases" Text="0" /></h3>

                            <p>Cases Added This Month</p>
                        </div>
                    </div>
                </div>
                <!-- ./col -->
                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-green">
                        <div class="inner">
                            <h3>
                                <asp:Label runat="server" ID="lblTotalAttorneys" Text="0" /></h3>

                            <p>Total Attorneys</p>
                        </div>
                    </div>
                </div>
                <!-- ./col -->
                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-yellow">
                        <div class="inner">
                            <h3>
                                <asp:Label runat="server" ID="lblUsersMonth" /></h3>

                            <p>Users This Month</p>
                        </div>
                    </div>
                </div>
                <!-- ./col -->
                <%--<div class="col-lg-3 col-xs-6">
                <!-- small box -->
                <div class="small-box bg-red">
                    <div class="inner">
                        <h3>
                            <asp:Label runat="server" ID="lblAndroid" Text="0" />/<asp:Label runat="server" ID="lblApple" Text="0" /></h3>

                        <p>Devices (Android/Apple)</p>
                    </div>
                </div>
            </div>--%>
                <!-- ./col -->
            </div>
            <!-- /.row -->
            <!-- Small boxes (Stat box) -->
            <div class="row">
                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-aqua">
                        <div class="inner">
                            <h3>
                                <asp:Label runat="server" ID="lblCasesToday" Text="0" /></h3>

                            <p>Cases Added Today</p>
                        </div>
                    </div>
                </div>
                <!-- ./col -->
                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-green">
                        <div class="inner">
                            <h3>
                                <asp:Label runat="server" ID="lblAttorneySMS" Text="0" />/<asp:Label runat="server" ID="lblClientSMS" Text="0" /></h3>

                            <p>Notifications This Month (Attorneys/Clients)</p>
                        </div>
                    </div>
                </div>
                <!-- ./col -->
                <%--<div class="col-lg-3 col-xs-6">
                <!-- small box -->
                <div class="small-box bg-yellow">
                    <div class="inner">
                        <h3>
                            <asp:Label runat="server" ID="lblUsersMonth" /></h3>

                        <p>Users This Month</p>
                    </div>
                </div>
            </div>--%>
                <!-- ./col -->
                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <!-- <div class="small-box bg-red">
                        <div class="inner">
                            <h3>
                                <asp:Label runat="server" ID="Label4" />/<asp:Label runat="server" ID="Label5" /></h3>

                            <p>Devices (Android/Apple)</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-pie-graph"></i>
                        </div>
                    </div>-->
                </div>
                <!-- ./col -->
            </div>
            <!-- /.row -->
        </asp:PlaceHolder>
        <!-- Main row -->
        <div class="row">
            <!-- Left col -->
            <div class="col-md-8">
                <div class="row">
                    <div class="col-md-6">
                    </div>
                    <!-- /.col -->

                    <div class="col-md-6">
                    </div>
                    <!-- /.col -->
                </div>
                <!-- /.row -->

                <!-- SAlES REPORT CONTENT -->
                <asp:PlaceHolder runat="server" ID="pnlSalesReport">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title">Top 15 Attorneys</h3>

                            <!-- box controls for minimize and close -->
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server" Width="100%" Height="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
                                <%--<telerik:RadMultiPage runat="server" ID="RadMultiPage4" Width="100%" Height="350px" SelectedIndex="0">
                                <telerik:RadPageView runat="server" ID="RadPageView5" ContentUrl="~/admin/TopSalesReport.aspx" Height="350px" />
                            </telerik:RadMultiPage>--%>
                                <telerik:RadGrid ID="radTopSalesGrid" runat="server" DataSourceID="SqlDataSource1" GroupPanelPosition="Top" RenderMode="Auto" Skin="Metro" PageSize="30">
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
                                        <asp:Parameter DefaultValue="15" Name="topCount" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </telerik:RadAjaxPanel>
                        </div>
                        <!-- /.box-body -->
                    </div>
                </asp:PlaceHolder>
                <!-- /.box -->
            </div>
            <!-- /.col -->

            <div class="col-md-4">

                <!-- ATTORNEY LOOKUP -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Attorney Lookup</h3>

                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat="server" Width="100%" Height="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
                            <asp:PlaceHolder runat="server" ID="pnlAttorneySearch">
                                <table class="wrap-table">
                                    <tr>
                                        <td>Bar Number</td>
                                    </tr>
                                    <tr>
                                        <td style="margin-left: 40px">
                                            <telerik:RadTextBox ID="txtBarNumber" runat="server" Skin="Metro" Width="100%">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>- OR -</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>First Name</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadTextBox ID="txtFirstName" runat="server" Skin="Metro" Width="100%">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Last Name</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadTextBox ID="txtLastName" runat="server" Skin="Metro" Width="100%">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>State</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadDropDownList ID="ddlState" runat="server" Skin="Metro" Width="100%">
                                                <Items>
                                                    <telerik:DropDownListItem Value="AL" Text="Alabama" />
                                                    <telerik:DropDownListItem Value="AK" Text="Alaska" />
                                                    <telerik:DropDownListItem Value="AZ" Text="Arizona" />
                                                    <telerik:DropDownListItem Value="AR" Text="Arkansas" />
                                                    <telerik:DropDownListItem Value="CA" Text="California" />
                                                    <telerik:DropDownListItem Value="CO" Text="Colorado" />
                                                    <telerik:DropDownListItem Value="CT" Text="Connecticut" />
                                                    <telerik:DropDownListItem Value="DC" Text="District of Columbia" />
                                                    <telerik:DropDownListItem Value="DE" Text="Delaware" />
                                                    <telerik:DropDownListItem Value="FL" Text="Florida" />
                                                    <telerik:DropDownListItem Value="GA" Text="Georgia" />
                                                    <telerik:DropDownListItem Value="HI" Text="Hawaii" />
                                                    <telerik:DropDownListItem Value="ID" Text="Idaho" />
                                                    <telerik:DropDownListItem Value="IL" Text="Illinois" />
                                                    <telerik:DropDownListItem Value="IN" Text="Indiana" />
                                                    <telerik:DropDownListItem Value="IA" Text="Iowa" />
                                                    <telerik:DropDownListItem Value="KS" Text="Kansas" />
                                                    <telerik:DropDownListItem Value="KY" Text="Kentucky" />
                                                    <telerik:DropDownListItem Value="LA" Text="Louisiana" />
                                                    <telerik:DropDownListItem Value="ME" Text="Maine" />
                                                    <telerik:DropDownListItem Value="MD" Text="Maryland" />
                                                    <telerik:DropDownListItem Value="MA" Text="Massachusetts" />
                                                    <telerik:DropDownListItem Value="MI" Text="Michigan" />
                                                    <telerik:DropDownListItem Value="MN" Text="Minnesota" />
                                                    <telerik:DropDownListItem Value="MS" Text="Mississippi" />
                                                    <telerik:DropDownListItem Value="MO" Text="Missouri" />
                                                    <telerik:DropDownListItem Value="MT" Text="Montana" />
                                                    <telerik:DropDownListItem Value="NE" Text="Nebraska" />
                                                    <telerik:DropDownListItem Value="NV" Text="Nevada" />
                                                    <telerik:DropDownListItem Value="NH" Text="New Hampshire" />
                                                    <telerik:DropDownListItem Value="NJ" Text="New Jersey" />
                                                    <telerik:DropDownListItem Value="NM" Text="New Mexico" />
                                                    <telerik:DropDownListItem Value="NY" Text="New York" />
                                                    <telerik:DropDownListItem Value="NC" Text="North Carolina" />
                                                    <telerik:DropDownListItem Value="ND" Text="North Dakota" />
                                                    <telerik:DropDownListItem Value="OH" Text="Ohio" />
                                                    <telerik:DropDownListItem Value="OK" Text="Oklahoma" />
                                                    <telerik:DropDownListItem Value="OR" Text="Oregon" />
                                                    <telerik:DropDownListItem Value="PA" Text="Pennsylvania" />
                                                    <telerik:DropDownListItem Value="RI" Text="Rhode Island" />
                                                    <telerik:DropDownListItem Value="SC" Text="South Carolina" />
                                                    <telerik:DropDownListItem Value="SD" Text="South Dakota" />
                                                    <telerik:DropDownListItem Value="TN" Text="Tennessee" />
                                                    <telerik:DropDownListItem Value="TX" Text="Texas" />
                                                    <telerik:DropDownListItem Value="UT" Text="Utah" />
                                                    <telerik:DropDownListItem Value="VT" Text="Vermont" />
                                                    <telerik:DropDownListItem Value="VA" Text="Virginia" />
                                                    <telerik:DropDownListItem Value="WA" Text="Washington" />
                                                    <telerik:DropDownListItem Value="WV" Text="West Virginia" />
                                                    <telerik:DropDownListItem Value="WI" Text="Wisconsin" />
                                                    <telerik:DropDownListItem Value="WY" Text="Wyoming" />
                                                </Items>
                                            </telerik:RadDropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadButton ID="btnFind" runat="server" Text="Search" ButtonType="LinkButton" Skin="Metro" Width="100%" CssClass="fixedWidth" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server" ID="pnlFound">
                                <table class="wrap-table">
                                    <tr>
                                        <td>Bar Number</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBarNumber" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Name</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Address</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Work Phone</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblWorkPhone" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Email</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEmail" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>License Date</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblLicenseDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>Record ID</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblID" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Username</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblUsername" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Password</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPassword" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table class="wrap-table">
                                                <tr>
                                                    <td colspan="3">The above attorney has
                                                    <asp:Label runat="server" ID="lblCaseCount" Text="0" />
                                                        Cases in our database.</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 33%;">
                                                        <telerik:RadButton ID="btnSaveAttorney" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Save to Database" Width="100%" Enabled="false" />
                                                    </td>
                                                    <td style="width: 34%;">
                                                        <telerik:RadButton ID="btnShowCalendar" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Show Calendar" Width="100%" />
                                                    </td>
                                                    <td style="width: 33%;">
                                                        <telerik:RadButton ID="btnStartOver" runat="server" ButtonType="LinkButton" CssClass="fixedWidth" Skin="Metro" Text="Start Over" Width="100%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:PlaceHolder>
                        </telerik:RadAjaxPanel>
                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->
            </div>
            <!-- /.col -->
        </div>
        <!-- /.row -->
        <!-- Main row -->
        <div class="row">
            <!-- Left col -->
            <div class="col-md-8">
                <div class="row">
                    <div class="col-md-6">
                    </div>
                    <!-- /.col -->

                    <div class="col-md-6">
                    </div>
                    <!-- /.col -->
                </div>
                <!-- /.row -->

                <!-- TEXT MESSAGE LOG -->
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">Notifications Today
                        </h3>

                        <!-- box controls for minimize and close -->
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel5" runat="server" Width="100%" Height="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
                            <telerik:RadDropDownList runat="server" ID="ddlSMSLogType" Width="100%" AutoPostBack="true">
                                <Items>
                                    <telerik:DropDownListItem Value="history" Text="History" />
                                    <telerik:DropDownListItem Value="queue" Text="Queue" />
                                </Items>
                            </telerik:RadDropDownList>
                            <br />
                            <br />
                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" Height="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
                                <telerik:RadMultiPage runat="server" ID="RadMultiPage3" Width="100%" Height="350px" SelectedIndex="0">
                                    <telerik:RadPageView runat="server" ID="fraSMSLog" Height="350px" />
                                </telerik:RadMultiPage>
                            </telerik:RadAjaxPanel>
                        </telerik:RadAjaxPanel>
                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->
            </div>
            <!-- /.col -->

            <div class="col-md-4">

                <!-- PROCESSOR LOG -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Processor Log</h3>

                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Width="100%" Height="100%" HorizontalAlign="NotSet" LoadingPanelID="MainAjaxLoadingPanel">
                            <telerik:RadDropDownList runat="server" ID="ddlProcessorLogType" Width="100%" AutoPostBack="true">
                                <Items>
                                    <telerik:DropDownListItem Value="file" Text="File Processor" />
                                    <telerik:DropDownListItem Value="billing" Text="Billing Processor" />
                                </Items>
                            </telerik:RadDropDownList>
                            <br />
                            <br />
                            <telerik:RadMultiPage runat="server" ID="RadMultiPage2" Width="100%" Height="350px" SelectedIndex="0">
                                <telerik:RadPageView runat="server" ID="fraProcessorLog" Height="350px" />
                            </telerik:RadMultiPage>
                        </telerik:RadAjaxPanel>
                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->
            </div>
            <!-- /.col -->
        </div>
        <!-- /.row -->

    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
