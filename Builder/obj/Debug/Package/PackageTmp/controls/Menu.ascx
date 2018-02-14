<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Menu.ascx.vb" Inherits="DailyDocket.Builder.Menu" %>
<ul class="sidebar-menu">
    <li class="header">MAIN NAVIGATION</li>
    <li runat="server" id="liDashboard">
        <asp:LinkButton runat="server" ID="lnkDashboard">
                <i class="fa fa-dashboard"></i><span>Dashboard</span>
        </asp:LinkButton>
    </li>
    <asp:PlaceHolder runat="server" ID="pnlMainOptions">
        <li runat="server" id="liHelp">
            <asp:LinkButton runat="server" ID="lnkHelp"><i class="fa fa-book"></i><span>Help Me With This</span></asp:LinkButton></li>
        <li runat="server" id="liFAQ">
            <asp:LinkButton runat="server" ID="lnkFaq"><i class="fa fa-book"></i><span>F.A.Q.</span></asp:LinkButton></li>
        <li runat="server" id="liMessages">
            <asp:LinkButton runat="server" ID="lnkMessages"><i class="fa fa-users"></i><span>Message Center</span></asp:LinkButton>
        </li>
        <asp:PlaceHolder runat="server" ID="pnlAdmin">
            <li class="header">ADMINISTRATOR OPTIONS</li>
            <li runat="server" id="liSalesReport">
                <asp:LinkButton runat="server" ID="lnkSalesReport"><i class="fa fa-file-text-o"></i><span>Sales Report</span></asp:LinkButton>
            </li>
            <li runat="server" id="liUsers">
                <asp:LinkButton runat="server" ID="lnkUsers"><i class="fa fa-users"></i><span>System Users</span></asp:LinkButton>
            </li>
            <li runat="server" id="liMailer">
                <asp:LinkButton runat="server" ID="lnkMailer"><i class="fa fa-envelope-o"></i><span>System Messages</span></asp:LinkButton>
            </li>
            <li runat="server" id="liLogs">
                <asp:LinkButton runat="server" ID="lnkLogs"><i class="fa fa-file-code-o"></i><span>System Logs</span></asp:LinkButton>
            </li>
            <li runat="server" id="liHelpTopics">
                <asp:LinkButton runat="server" ID="lnkHelpTopics"><i class="fa fa-book"></i><span>Help Topics</span></asp:LinkButton>
            </li>
        </asp:PlaceHolder>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="pnlLoginOptions">
        <li runat="server" id="liLogin">
            <asp:LinkButton runat="server" ID="lnkLogin"><i class="fa fa-asdf"></i><span>Login</span></asp:LinkButton></li>
        <li runat="server" id="liForgotPassword">
            <asp:LinkButton runat="server" ID="lnkForgotPassword"><i class="fa fa-asdf"></i><span>Forgot Password</span></asp:LinkButton></li>
        <li runat="server" id="liRegister">
            <asp:LinkButton runat="server" ID="lnkRegister"><i class="fa fa-asdf"></i><span>Register</span></asp:LinkButton></li>
    </asp:PlaceHolder>
</ul>
<asp:Panel runat="server" ID="pnlHidden" BackColor="#CC0000" Visible="false">
    <asp:Label runat="server" ID="lblUrlString" />
</asp:Panel>
