<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/site.master" CodeFile="AutoScaffold.aspx.cs" Inherits="AutoScaffold" ValidateRequest="false" EnableEventValidation="false" Title="AutoScaffold" %>
<%@ MasterType virtualpath="~/site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
    <subsonic:Scaffold ID="AutoScaffold1" runat="server" ScaffoldType="Auto"></subsonic:Scaffold>
</asp:Content>