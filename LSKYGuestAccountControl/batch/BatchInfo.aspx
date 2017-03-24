<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="BatchInfo.aspx.cs" Inherits="LSKYGuestAccountControl.Batch.BatchInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageBody" runat="server">
    <asp:Literal ID="litHeading" runat="server"></asp:Literal>
    <asp:Table ID="tblAccounts" runat="server" Width="100%" Visible="False">
    </asp:Table>
</asp:Content>
