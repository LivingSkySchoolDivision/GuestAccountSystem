<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYGuestAccountControl.Login.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageBody" runat="server">
    
    <div id="logo_container">
        <img id="lskysd_logo" src="../Images/logo_text.gif" alt="Living Sky School Division Logo"/>
    </div>

    <h1>Requisition a Guest Account</h1>
    <p>Guest accounts are temporary accounts that will automatically self-destruct at the end of the day.</p>
    <p>
        <b>Guest accounts can be used for:</b>
        <ul>
            <li>Connecting wireless devices to the <b>Living Sky Public WebAuth</b> wireless network, if the device owner doesn't already have an account in our system.</li>
            <li>Guest / Presenter / Parent access to computers in your school</li>
        </ul>
    </p>
    <p>
        <b>Who can use this site to requisition guest accounts?</b>
        <ul>
            <li>School secretaries</li>
            <li>School librarians</li>
            <li>School principals & vice principals</li>
            <li><a href="https://helpdesk.lskysd.ca">Help desk staff</a></li>
        </ul>
    </p>
    <p><a href="https://makeapassword.lskysd.ca">Click here to just generate a random password.</a></p>
    <br/><br/>
    <asp:Table ID="tblAlreadyLoggedIn" runat="server" HorizontalAlign="Center" Visible="false">
        <asp:TableRow>
            <asp:TableCell>
            <div class="large_infobox" style="width: 450px; margin-left: auto; margin-right: auto; background-color: white;" id="Div1">
            You are currently logged in as: <asp:Label ID="lblUsername" runat="server" Text="" Font-Bold="true"></asp:Label> <br /><a href="../index.aspx">Click here to go to the main menu.</a>
            </div>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table> 

    <asp:Table ID="tblErrorMessage" runat="server" HorizontalAlign="Center" Visible="false">
        <asp:TableRow>
            <asp:TableCell>
                <div class="large_infobox" style="width: 450px; margin-left: auto; margin-right: auto; background-color: white;" id="error_box">
                    <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
                </div>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table> 
    
     <asp:Table ID="tblLoginform" runat="server" class="LoginFormContainer">
        <asp:TableRow>
            <asp:TableCell><div style="font-size: 10pt;">Username</div></asp:TableCell>
            <asp:TableCell><asp:TextBox ID="txtUsername" Width="200" Runat="server" ></asp:TextBox></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell><div style="font-size: 10pt;">Password</div></asp:TableCell>
            <asp:TableCell><asp:TextBox ID="txtPassword" Width="200" Runat="server" TextMode="Password"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2"><div style="color: red; font-weight: bold;font-size: 10pt;"><asp:Label ID="lblStatus" runat="server" Text="" /></div></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2"><img src="../Images/lock.png" /> <asp:Button ID="btnLogin" Runat="server" Text="Login" OnClick="btnLogin_Click"></asp:Button></asp:TableCell>
        </asp:TableRow>
    </asp:Table>    
</asp:Content>
