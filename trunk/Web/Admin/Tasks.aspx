<%@ Page Language="C#"  AutoEventWireup="true" MasterPageFile="~/site.master"  CodeFile="Tasks.aspx.cs" Inherits="admin_tasks" Title="Task Administration" %>
<%@ MasterType virtualpath="~/site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
<div id="centercontent">
<div>
	<a href="Tasks_edit.aspx">New Task</a>
</div>
<br />
<asp:UpdateProgress ID="uprogReportStatus" runat="server">
<ProgressTemplate>
	<div class="progress">
		Updating....<img src='<%= Page.ResolveUrl("~/images/spinner.gif") %>' alt="Updating..." />
	</div>
</ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="upnlTasks" runat="server" ChildrenAsTriggers="true" UpdateMode="conditional">
<ContentTemplate>
	<asp:DataGrid ID="dgTasks" runat="server" AutoGenerateColumns="false" SkinID="sortable">
		<Columns>
			<asp:HyperLinkColumn DataTextField="Name" HeaderText="Name" DataNavigateUrlField="name" DataNavigateUrlFormatString="tasks_edit.aspx?taskname={0}"></asp:HyperLinkColumn>
			<asp:BoundColumn DataField="Creator" HeaderText="Creator"></asp:BoundColumn>
			<asp:BoundColumn DataField="NextRunTime" HeaderText="NextRunTime"></asp:BoundColumn>
			<asp:BoundColumn DataField="parameters" HeaderText="Parameters"></asp:BoundColumn>
			<asp:BoundColumn DataField="MostRecentRunTime" HeaderText="MostRecentRunTime"></asp:BoundColumn>
			<asp:BoundColumn DataField="ExitCode" HeaderText="MostRecentExitCode"></asp:BoundColumn>
			<asp:BoundColumn DataField="Status" HeaderText="Status"></asp:BoundColumn>
			<asp:TemplateColumn>
				<HeaderTemplate>
					Actions
				</HeaderTemplate>
				<ItemTemplate>
					<asp:LinkButton runat="server" Text="Stop" OnCommand="LinkButton_Command" CommandName="StopTheTask" CommandArgument='<%#Eval("name") %>' Visible='<%# Eval("IsRunning") %>'></asp:LinkButton>
					<asp:LinkButton runat="server" Text="Run" OnCommand="LinkButton_Command" CommandName="RunTheTask" CommandArgument='<%#Eval("name") %>' Visible='<%# !(bool)Eval("IsRunning") %>'></asp:LinkButton>
					<asp:LinkButton runat="server" Text="Delete" OnCommand="LinkButton_Command" CommandName="DeleteTheTask" CommandArgument='<%#Eval("name") %>'></asp:LinkButton>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
	<asp:Timer ID="timerTaskList" runat="server" Interval="15000" OnTick="timerTaskList_Tick"></asp:Timer>
</ContentTemplate>
</asp:UpdatePanel>
<br />
<br />
<br />
</div>
</asp:Content>
