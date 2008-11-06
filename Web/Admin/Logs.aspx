<%@ Page Language="C#"  AutoEventWireup="true" MasterPageFile="~/site.master"  CodeFile="Logs.aspx.cs" Inherits="admin_logs" Title="Log Viewer" %>
<%@ MasterType virtualpath="~/site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
<div id="centercontent">
<br />
<asp:DataGrid ID="dgWebEvents" runat="server" AutoGenerateColumns="false" SkinID="sortable" >
	<Columns>
		<asp:HyperLinkColumn DataNavigateUrlField="EventID" DataNavigateUrlFormatString="logs_show.aspx?id={0}" DataTextField="EventID" DataTextFormatString="Details"></asp:HyperLinkColumn>
		<asp:BoundColumn DataField="EventTime" HeaderText="Event Time"></asp:BoundColumn>
		<asp:BoundColumn DataField="Message" HeaderText="Message"></asp:BoundColumn>
		<asp:BoundColumn DataField="RequestUrl" HeaderText="URL"></asp:BoundColumn>
	</Columns>
</asp:DataGrid>
<br />
<br />
<br />
</div>
</asp:Content>
