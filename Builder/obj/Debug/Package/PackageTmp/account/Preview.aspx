<%@ Page Title="Welcome to Daily Docket!" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/Dashboard.master" CodeBehind="Preview.aspx.vb" Inherits="DailyDocket.Builder.Preview" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/controls/RecordDetails.ascx" TagPrefix="docket" TagName="RecordDetails" %>

<%@ MasterType VirtualPath="~/masterPages/Dashboard.Master" %>

<%@ Register Assembly="DailyDocket.CommonCore" Namespace="DailyDocket.CommonCore.Controls.Labels" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .wrap-table-login {
            width: 100%;
        }

        .rbLinkButton.fixedWidth {
            padding: 0;
        }

        .auto-style1 {
            text-decoration: underline;
            font-weight: bold;
            color: #cc0000;
        }

        .wrap-table-bottom {
            width: 100%;
            display: block;
        }

            .wrap-table-bottom td {
                display: inline-block;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="breadcrumbContent" runat="server">
    <h1>Dashboard
        <small>Version
            <asp:Label runat="server" ID="lblSSL" Text="s" />
        </small>
    </h1>
    <ol class="breadcrumb">
        <li><a href="../Default.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
        <li class="active">Calendar Preview</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <!-- Small boxes (Stat box) -->
    <div class="row">
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-aqua">
                <div class="inner">
                    <h3>
                        <asp:Label runat="server" ID="lblTotalCases" Text="0" />
                    </h3>

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
                        <asp:Label runat="server" ID="lblTotalAttorneys" Text="0" />
                    </h3>

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
                        <asp:Label runat="server" ID="lblCasesToday" Text="0" />
                    </h3>

                    <p>Cases Added Today</p>
                </div>
            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-red">
                <div class="inner">
                    <h3>
                        <asp:Label runat="server" ID="lblAttorneySMS" Text="0" />
                    </h3>

                    <p>Notifications This Month</p>
                </div>
            </div>
        </div>
        <!-- ./col -->
    </div>
    <!-- /.row -->
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

            <!-- CALENDAR LOOKUP -->
            <div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">Calendar Preview
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
                    <table class="nav-justified">
                        <tr>
                            <td>Our app ensures no more missed court appearances, scheduling headaches or last minute client phones calls.<br />
                                <br />
                                <span style="font-size: large; font-weight: bold;">Enter your bar number</span> below and we&#39;ll see if we have any cases for you ...</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="txtBarNumber" runat="server" Skin="Metro" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadButton ID="btnFind" runat="server" Text="Find My Cases!" ButtonType="LinkButton" Skin="Metro" Width="100%" CssClass="fixedWidth" Font-Bold="True" Font-Size="Large" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <span style="color: #CC0000;">
                                    <asp:Literal ID="lblMessage" runat="server"></asp:Literal>
                                </span>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlStep2" runat="server">
                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblCaseCount" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
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
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table class="nav-justified">
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td><span class="auto-style1">Client Billable Features Include:</span><br />
                                <br />
                                <strong>Text &amp; Email Notifications</strong> to you and your clients every time cases are updated on the AOC<br />
                                <strong>Contact all of your clients</strong> from our Message Center<br />
                                <strong>Use our Mobile App</strong> so you never miss a scheduled appearance!</td>
                        </tr>
                    </table>
                </div>
                <!-- /.box-body -->
            </div>
            <!-- /.box -->
        </div>
        <!-- /.col -->

        <div class="col-md-4">

            <!-- PROFIT PROJECTION -->
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">Profit Projection</h3>

                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse">
                            <i class="fa fa-minus"></i>
                        </button>
                        <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                    </div>
                </div>
                <!-- /.box-header -->
                <div class="box-body">
                    <table class="wrap-table-login">
                        <tr>
                            <td>Monthly Fee you can bill per client, per month for Email and Text Notifications:</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtClientCharge" Width="100%" AutoPostBack="true" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <%--<telerik:RadMultiPage runat="server" ID="RadMultiPage4" Width="100%" Height="500px" SelectedIndex="0">
                            <telerik:RadPageView runat="server" ID="fraProfitChart" Height="450px" />
                        </telerik:RadMultiPage>--%>
                    <asp:Panel runat="server" ID="pnlChart">
                        <table class="wrap-table-login">
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
                </div>
                <!-- /.box-body -->
            </div>
            <!-- /.box -->
        </div>
        <!-- /.col -->
    </div>
    <table class="wrap-table-bottom">
        <tr>
            <td colspan="3">&nbsp;</td>
        </tr>
        <tr>
            <td style="vertical-align: top; width: 33%;">
                <img alt="Attorneys" src="../images/attorneys.jpg" /><br />
                <br />
                <strong>Who We Help</strong><br />
                <br />
                As an Attorney representing clients in multiple courts and counties, you’ll likely agree that organizing and managing calendars can be a major challenge. Struggling to stay on track with clients in different court rooms and counties, and suffering last minute phone calls, are the typical headaches that until now, seemed unavoidable.&nbsp; If you’re ready to eliminate these costly, stressful issues, here’s your opportunity, dailycourtdocket.com</td>
            <td style="vertical-align: top; width: 34%;">
                <img alt="Lawyers" src="../images/lawyers.jpg" /><br />
                <br />
                <strong>Features</strong><br />
                <br />
                Now you can see and manage ALL your AOC court dates in a single app – Here’s what you get!<br />
                <br />
                <ul>
                    <li>All IF, CR, CRS, CVD and CVS Cases directly from the AOC court calendars</li>
                    <li>Sorts by date, time, county, courtroom and sequence number!</li>
                    <li>Shows all your scheduled cases in one click</li>
                    <li>Shows all the details for any case on your list</li>
                    <li>Provides a list of all your clients that have future court dates</li>
                    <li>Provide court date reminders by mobile device TEXT or EMAIL</li>
                    <li>Upsell the automated notification process as a value add to your clients</li>
                </ul>
            </td>
            <td style="vertical-align: top; width: 33%;">
                <img alt="Law Firm" src="../images/law%20firm.jpg" /><br />
                <br />
                <strong>System Overview</strong><br />
                <br />
                By simply entering in your bar number, we use our technology platform and we que the AOC calendars, both civil and criminal, and look for any matches where your name appears. The minute we find a match our system automatically sends you an email and text notification of where you are supposed to appear. We also send a reminder notification 24 hours before the court date. We can even extend this courtesy to your clients!</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
