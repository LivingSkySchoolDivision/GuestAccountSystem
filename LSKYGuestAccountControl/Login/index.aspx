<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYGuestAccountControl.Login.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageBody" runat="server">
    
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
            <asp:TableCell ColumnSpan="2"><img src="../lock.png" /> <asp:Button ID="btnLogin" Runat="server" Text="Login" OnClick="btnLogin_Click"></asp:Button></asp:TableCell>
        </asp:TableRow>
    </asp:Table>    
</asp:Content>
