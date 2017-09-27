<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYGuestAccountControl.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageBody" runat="server">
    
    <div id="logo_container">
        <img id="lskysd_logo" src="/Images/logo_text.gif" alt="Living Sky School Division Logo"/>
    </div>
    
    <asp:Table ID="tblIndexInstructions" runat="server" Visible="True">
        <asp:TableRow>
            <asp:TableCell colspan="2">
                <h1>Activate a guest account</h1>
                <p>Guest accounts are temporary accounts that will automatically self-destruct at the end of the day.</p>
                <p>
                    <b><img src="Images/caution.jpg" class="caution-icon"/> Important notes regarding guest accounts:</b>
                    <ul>
                        <li>Guest accounts are temporary - they will only work on the day that they are activated.</li>
                        <li>Guest accounts are given a random password each time they are activated.</li>
                        <li>It may take a few minutes for your guest account to become available.</li>
                        <li>If you need a guest account for multiple days, you must use this site to request one each day. Guest accounts that last more than a day are not possible.</li>
                        <li>Do not make guest accounts for existing staff or students - they already have accounts that they can use.</li>
                        <li>Guest accounts are logged and monitored more than normal accounts.</li>
                        <li>You may only activate up to <b><asp:Label ID="lblAllowedRequisitionsPerDay" runat="server" Text="0"></asp:Label></b> guest accounts per day. If you require more, please contact the <a href="https://helpdesk.lskysd.ca">Help Desk</a></li>
                        <li>If you have trouble getting your guest account to work, please consider making a <a href="https://helpdesk.lskysd.ca">Help Desk ticket</a>.</li>
                    </ul>
                </p>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Width="50%" HorizontalAlign="Center">                
                <asp:Literal ID="tblCellBatch" runat="server">
                    <b>Batch users</b><br/><a href="/batch">Click here to create multiple guest accounts at a time</a>.
                </asp:Literal>
            </asp:TableCell>            
            <asp:TableCell Width="50%" HorizontalAlign="Center">
                <asp:Literal ID="tblCellLog" runat="server">
                    <b>Log</b><br/><a href="/log">Click here to view the log</a>.
                </asp:Literal>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br /><br />

    <asp:Table ID="tblControls" CssClass="control_table" runat="server" Visible="True">
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2"><b>Please describe why you are requesting a guest account.</b><br/><div class="smalltext">This helps us understand how these accounts get used.</div></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2"><asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Height="50" Width="100%" ></asp:TextBox></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell><asp:Label ID="lblCount" runat="server" Text=""></asp:Label></asp:TableCell>
            <asp:TableCell style="text-align: right;"><asp:Button ID="btnActivate" runat="server" Text="Activate a guest account" OnClick="btnActivate_OnClick" /></asp:TableCell>
       </asp:TableRow>
    </asp:Table>
    
    <asp:Table ID="tblTooMany" CssClass="control_table" runat="server" Visible="false">
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2"><b>Too many guest accounts today</b></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">You can only activate <asp:Label ID="lblAllowedRequisitionsPerDay2" runat="server" Text=""></asp:Label> guest accounts per day. If require more than 3, please contact the <a href="https://helpdesk.lskysd.ca">Help Desk</a> for assistance.</asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    
    
    <asp:Table ID="tblNewAccountInfo" runat="server" CssClass="credential_table" Visible="False">
        <asp:TableRow>
            <asp:TableCell><div class="usernameandpassword_title">Username</div></asp:TableCell>
            <asp:TableCell><div class="usernameandpassword_title">Password</div></asp:TableCell>
        </asp:TableRow>
        
        <asp:TableRow>
            <asp:TableCell><div class="usernameandpassword"><asp:Label ID="lblUsername" runat="server" Text=""></asp:Label></div></asp:TableCell>
            <asp:TableCell><div class="usernameandpassword"><asp:Label ID="lblPassword" runat="server" Text=""></asp:Label></div></asp:TableCell>
        </asp:TableRow>
        
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2"><div class="usernameandpassword_title">&nbsp;&nbsp;</div></asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell ColumnSpan="2"><div class="usernameandpassword_title">This account will self-destruct at</div></asp:TableCell>
        </asp:TableRow>
        
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2"><div class="usernameandpassword"><asp:Label ID="lblExpires" runat="server" Text=""></asp:Label></div></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    
    <asp:Table ID="tblNewAccountInstructions" runat="server" Visible="False">
        <asp:TableRow>
            <asp:TableCell>
                <br/>
                <h2>Using this account on School Division Computers</h2>
                <p>This account can be used to sign into any Windows-based school division computer . It will <i>not</i> have access to the network drives (Z drive, Public/Private, Handin/Handout), and may not have access to most printers.</p>
                
                <h2>Using this account to connect to WiFi</h2>
                <ol>
                    <li>Connect to the <b>Living Sky Public WebAuth</b> wireless network on your device</li>
                    <li>Open a web browser, and try to go to a website such as http://google.com</li>
                    <li>You will be prompted for a username and password</li>
                    <li>Log in using the username and password you see above</li>
                </ol>
                <a href="/">Click here to return to the previous page</a>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br/><br/>
    <asp:Table ID="tblActiveAccounts" runat="server" Visible="False" Width="100%"></asp:Table>
    
    <br/><br/>
    <br/><br/>
    <br/><br/>
    <br/><br/>

</asp:Content>
