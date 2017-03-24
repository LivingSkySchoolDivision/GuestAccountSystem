<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYGuestAccountControl.Batch.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageBody" runat="server">
    
    <div id="logo_container">
        <img id="lskysd_logo" src="/Images/logo_text.gif" alt="Living Sky School Division Logo"/>
    </div>

    <asp:Table ID="tblIndexInstructions" runat="server" Visible="True">
        <asp:TableRow>
            <asp:TableCell>
                <h1>Activate a batch of guest accounts</h1>
                <p>
                    Batch size is currently limited to: <b><asp:Label ID="lblMaxBatchSize" runat="server" Text="0"></asp:Label></b><br/>
                    There are currently <b><asp:Label ID="lblAvailableGuestAccounts" runat="server" Text="0"></asp:Label></b> accounts available.
                </p>

                <p>Guest accounts are temporary accounts that will automatically self-destruct at the end of the day.</p>
                <p>
                    <b><img src="../Images/caution.jpg" class="caution-icon"/> Important notes regarding guest accounts:</b>
                    <ul>
                        <li>Guest accounts are temporary - they will only work on the day that they are activated.</li>
                        <li>Guest accounts are given a random password each time they are activated.</li>
                        <li>It may take a few minutes for your guest account to become available.</li>
                        <li>If you need a guest account for multiple days, you must use this site to request one each day. Guest accounts that last more than a day are not possible.</li>
                        <li>Do not make guest accounts for existing staff or students - they already have accounts that they can use.</li>
                        <li>Guest accounts are logged and monitored more than normal accounts.</li>
                       <li>If you have trouble getting your guest account to work, please consider making a <a href="https://helpdesk.lskysd.ca">Help Desk ticket</a>.</li>
                    </ul>
                </p>
                <br/><br/>
            </asp:TableCell>
        </asp:TableRow>

    </asp:Table><asp:Table ID="tblControls" CssClass="control_table" runat="server" Visible="True">
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2"><b>Please describe why you are requesting a guest account.</b><br/><div class="smalltext">This helps us understand how these accounts get used.</div></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2"><asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Height="50" Width="100%" ></asp:TextBox></asp:TableCell>
        </asp:TableRow>
        
        <asp:TableRow>
            <asp:TableCell>How many guest accounts do you need?</asp:TableCell>
            <asp:TableCell HorizontalAlign="Right"><asp:DropDownList ID="drpBatchCount" runat="server"></asp:DropDownList></asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell><small>It may take a minute to activate your batch, depending on how many users you are activating at once. <b>Do not reload or refresh this page</b>.</small></asp:TableCell>
            <asp:TableCell style="text-align: right;"><asp:Button ID="btnActivate" runat="server" Text="Activate batch" OnClick="btnActivate_OnClick" /></asp:TableCell>
       </asp:TableRow>
    </asp:Table>
    
    <asp:Table ID="tblAccountInfo" CssClass="control_table" runat="server" Visible="false">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>Username</asp:TableHeaderCell>
            <asp:TableHeaderCell>Password</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>

</asp:Content>
