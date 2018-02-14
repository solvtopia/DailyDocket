<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RecordDetails.ascx.vb" Inherits="DailyDocket.Builder.RecordDetails" %>
<style>
    .myListBoxes {
        border-color: #f0f0f0;
        border-style: solid;
        border-width: 1px;
    }

    .rlbGroup, .rlbList {
        overflow: auto !important;
    }

    .auto-style1 {
        width: 100%;
    }
</style>
<telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0" Width="100%">
    <Tabs>
        <telerik:RadTab runat="server" Text="Details" PageViewID="RadPageView1" Selected="true">
        </telerik:RadTab>
        <telerik:RadTab runat="server" Text="All Parties" PageViewID="RadPageView2">
        </telerik:RadTab>
        <telerik:RadTab runat="server" Text="Raw View" PageViewID="RadPageView3">
        </telerik:RadTab>
        <telerik:RadTab runat="server" Text="Not My Case" PageViewID="RadPageView4">
        </telerik:RadTab>
    </Tabs>
</telerik:RadTabStrip>
<telerik:RadMultiPage ID="RadMultiPage1" SelectedIndex="0" runat="server" Width="100%">
    <telerik:RadPageView ID="RadPageView1" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
        <table style="width: 100%; margin: 10px;" cellpadding="1" cellspacing="2">
            <tr>
                <td style="font-weight: bold;">Case Number</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCaseNumber" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Sequence Number</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRecordNumber" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Session Date</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSessionDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Session Time</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSessionTime" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Court Room</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCourtRoom" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Location</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLocation" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Presiding Judge</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblJudge" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td style="font-weight: bold;">County</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCounty" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight: bold;">State</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblState" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Record Type</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRecordType" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Panel runat="server" ID="pnlCivil">
            <table style="width: 100%; margin: 10px;" cellpadding="1" cellspacing="2">
                <tr>
                    <td style="font-weight: bold;">Case Type</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblType" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="font-weight: bold;">Days Since Filing</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDaysSinceFiling" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold;">Length of Trial</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblLengthOfTrial" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="pnlCivilIssues">
                <table style="width: 100%; margin: 10px;" cellpadding="1" cellspacing="2">

                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold;">Issues or Events</td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadListBox ID="lstIssuesOrEvents" runat="server" Width="100%" CssClass="myListBoxes" RenderMode="Auto"></telerik:RadListBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlCivilNotes">
                <table style="width: 100%; margin: 10px;" cellpadding="1" cellspacing="2">
                    <tr>
                        <td style="font-weight: bold;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold;">Notes</td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadListBox ID="lstCivilNotes" runat="server" Width="100%" CssClass="myListBoxes" RenderMode="Auto"></telerik:RadListBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlCriminal">
            <table style="width: 100%; margin: 10px;" cellpadding="1" cellspacing="2">
                <tr>
                    <td style="font-weight: bold;">Clerk</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClerk" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold;">Bond</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblBond" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold;">True Bill of Indictment Date</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTB" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold;">Continuances</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblContinuances" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="font-weight: bold;">Charges</td>
                </tr>
                <tr>
                    <td>
                        <asp:Table runat="server" ID="tblCharges" Width="100%"></asp:Table>
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="pnlCriminalNotes">
                <table style="width: 100%; margin: 10px;" cellpadding="1" cellspacing="2">
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold;">Notes</td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadListBox ID="lstCriminalNotes" runat="server" Width="100%" CssClass="myListBoxes" RenderMode="Auto"></telerik:RadListBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
    </telerik:RadPageView>
    <telerik:RadPageView ID="RadPageView2" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
        <asp:Panel runat="server" ID="pnlCivilParties">
            <table style="width: 100%; margin: 10px;" cellpadding="1" cellspacing="2">
                <tr>
                    <td style="font-weight: bold;">Plaintiff(s)</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadListBox ID="lstCivilPlaintiff" runat="server" CssClass="myListBoxes" Width="100%" RenderMode="Auto">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold;">Plaintiff Attorney(s)</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadListBox ID="lstCivilPlaintiffAttorney" runat="server" CssClass="myListBoxes" Width="100%" RenderMode="Auto">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="font-weight: bold;">Defendant(s)</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadListBox ID="lstCivilDefendant" runat="server" CssClass="myListBoxes" Width="100%" RenderMode="Auto">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold;">Defendant Attorney(s)</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadListBox ID="lstCivilDefendantAttorney" runat="server" CssClass="myListBoxes" Width="100%" RenderMode="Auto">
                        </telerik:RadListBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlCriminalParties">
            <table style="width: 100%; margin: 10px;" cellpadding="1" cellspacing="2">
                <tr>
                    <td style="font-weight: bold;">Prosecutor(s)</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadListBox ID="lstProsecutor" runat="server" CssClass="myListBoxes" Width="100%" RenderMode="Auto">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="font-weight: bold;">Assistant District Attorney(s)</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadListBox ID="lstAssistantDA" runat="server" CssClass="myListBoxes" Width="100%" RenderMode="Auto">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="font-weight: bold;">Defendant(s)</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadListBox ID="lstCriminalDefendant" runat="server" CssClass="myListBoxes" Width="100%" RenderMode="Auto">
                        </telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold;">Defendant Attorney(s)</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadListBox ID="lstCriminalDefendantAttorney" runat="server" CssClass="myListBoxes" Width="100%" RenderMode="Auto">
                        </telerik:RadListBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </telerik:RadPageView>
    <telerik:RadPageView ID="RadPageView3" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
        <asp:Panel runat="server" ID="pnlScrollRawView" ScrollBars="Auto">
            <span style="font-family: 'Courier New'; font-size: small;">
                <asp:Table runat="server" ID="tblRawView" Width="100%"></asp:Table>
            </span>
        </asp:Panel>
    </telerik:RadPageView>
    <telerik:RadPageView ID="RadPageView4" runat="server" BorderColor="#f0f0f0" BorderStyle="Solid" BorderWidth="1px">
        <table style="width: 100%; margin: 10px;" cellpadding="1" cellspacing="2">
            <tr>
                <td>If you&#39;re absolutely sure this is not your case and no longer wish to see it on your calendar please click the button below.<br />
                    <br />
                    <span style="color: #cc0000">Please note, this can only be undone by the Solvtopia, LLC Support Team!</span><br />
                    <br />
                    <telerik:RadButton ID="btnNotMine" runat="server" Text="Get Rid Of It!" ButtonType="LinkButton" Skin="Metro" />
                </td>
            </tr>
        </table>
    </telerik:RadPageView>
</telerik:RadMultiPage>
<asp:Panel runat="server" ID="pnlHidden" BackColor="#CC0000" Visible="false">
    <asp:Label runat="server" ID="lblUrlString" />
</asp:Panel>
