<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masterPages/Layout.Master" CodeBehind="Default.aspx.vb" Inherits="DailyDocket.API._Default" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/masterPages/Layout.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script src="scripts/radWindowScripts.js"></script>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="pageHeaderContent" runat="server">
    <asp:Label ID="lblHeader" runat="server" Text="Overview"></asp:Label>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="pageContent_Ajax" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        The Utility Wizards API is designed to provide Input and Output access to the main system databases in a controlled environment allowing us to maintain the database integrity of our system.<br />
        <br />
        The API project is broken into 2 primary services, these are the
        <asp:LinkButton ID="lnkInputController" runat="server" ForeColor="Black" NavigateUrl="InputController.aspx">InputController</asp:LinkButton>
        &nbsp;and
        <asp:LinkButton ID="lnkOutputController" runat="server" ForeColor="Black" NavigateUrl="OutputController.aspx">OutputController</asp:LinkButton>
        .&nbsp; Each has methods and routines designed to perform specific tasks within the Utility Wizards system.<br />
        <br />
        To maximize compatibility across a broad array of platforms and platform versions including Microsoft Windows, Mac OS, Android, and iOS the services have been build using ASMX technology as opposed to some newer technologies such as MVC or WebAPI.&nbsp; All routines have been tested in both VB.NET and C#.NET and each routine document in the navigation tree on the left contains sample code in both languages.<br />
        <br />
        The native Utility Wizards Mobile Apps for Android and iOS are built using the same API as we offer to our 3rd Party Vendors, which allows us to keep not only our own projects up to date but also those built by 3rd Parties to interface with our system.<br />
        <br />
        Email notifications will be sent out for any new releases to the API and updated documents are posted regularly with additional sample code in both VB.NET and C#.NET.&nbsp; Specific use examples are available upon request.<br />
        <br />
        Currently the API only skims the surface of the UtilityWizards CommonCore library, which is used as the basis for the full Utility Wizards platform, and we plan to expose more features as we continue to build our API allowing 3rd Party Vendors more and more access to the Utility Wizards system.<br />
        <br />
        <br />
        If you have any questions about anything you see in this documentation or would like to request access or new features please contact Solvtopia, LLC.
        <br />
        <br />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="pageContent_OutsideAjax" runat="server">
</asp:Content>
