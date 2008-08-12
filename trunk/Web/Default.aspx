<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Title="Welcome!" %>
<%@ Register Src="Modules/ContentManager/Paragraph.ascx" TagName="Paragraph" TagPrefix="cms" %>
<%@ MasterType virtualpath="~/site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
    <cms:Paragraph ID=defaultTop runat=server ContentName=default_welcome />
</asp:Content>

