﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYGuestAccountControl.Password.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Random password</title>
    <link rel="stylesheet" href="/CSS/Main.css"/>
    <link rel='shortcut icon' href="/favicon.ico" type="image/x-icon" />
</head>
<body>
    <br /><br /><br /><Br />
    <div id="page_container"> 
    <form id="form1" runat="server">
        <div>
            <div class="usernameandpassword_title">Random password:</div>
            <asp:TableCell><div class="usernameandpassword"><asp:Label ID="lblPassword" runat="server" Text=""></asp:Label></div></asp:TableCell>            
        </div>
        <div>
            Refresh the page to generate a new one.
        </div>

    </form>
    </div>
</body>
</html>