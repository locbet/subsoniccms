<%@ Page Language="C#"  AutoEventWireup="true" MasterPageFile="~/site.master"  CodeFile="Tasks.aspx.cs" Inherits="admin_tasks" Title="Task Administration" %>
<%@ MasterType virtualpath="~/site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
<cms:ResultMessage ID="ResultMessage1" runat="server" />

<div id="centercontent">
<div>
	<a href="Tasks_edit.aspx">New Task</a>
</div>
<br />
<asp:DataGrid ID="dgTasks" runat="server" AutoGenerateColumns="false" SkinID="sortable">
	<Columns>
		<asp:HyperLinkColumn DataTextField="Name" HeaderText="Name" DataNavigateUrlField="name" DataNavigateUrlFormatString="tasks_edit.aspx?taskname={0}"></asp:HyperLinkColumn>
		<asp:BoundColumn DataField="Creator" HeaderText="Creator"></asp:BoundColumn>
		<asp:BoundColumn DataField="NextRunTime" HeaderText="NextRunTime"></asp:BoundColumn>
		<asp:BoundColumn DataField="parameters" HeaderText="Parameters"></asp:BoundColumn>
		<asp:BoundColumn DataField="MostRecentRunTime" HeaderText="MostRecentRunTime"></asp:BoundColumn>
		<asp:BoundColumn DataField="ExitCode" HeaderText="MostRecentExitCode"></asp:BoundColumn>
		<asp:BoundColumn DataField="Status" HeaderText="Status"></asp:BoundColumn>
	</Columns>
</asp:DataGrid>
<br />
<br />
<br />
</div>
</asp:Content>
