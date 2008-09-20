<%@ Page Language="C#"  AutoEventWireup="true" MasterPageFile="~/site.master"  CodeFile="Logs_show.aspx.cs" Inherits="admin_logs_show" Title="Log Viewer" %>
<%@ MasterType virtualpath="~/site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
<cms:ResultMessage ID="ResultMessage1" runat="server" />

<div id="centercontent">
<br />
<asp:DetailsView ID="LogEntryDetails" runat="server" AutoGenerateRows="False" DataKeyNames="EventId" EnableViewState="false" SkinID="LogDetail">
    <Fields>
        <asp:BoundField DataField="EventId" HeaderText="EventId" ReadOnly="True" SortExpression="EventId" />
        <asp:BoundField DataField="EventTimeUtc" HeaderText="EventTimeUtc" SortExpression="EventTimeUtc" />
        <asp:BoundField DataField="EventTime" HeaderText="EventTime" SortExpression="EventTime" />
        <asp:BoundField DataField="EventType" HeaderText="EventType" SortExpression="EventType" />
        <asp:BoundField DataField="EventSequence" HeaderText="EventSequence" SortExpression="EventSequence" />
        <asp:BoundField DataField="EventOccurrence" HeaderText="EventOccurrence" SortExpression="EventOccurrence" />
        <asp:BoundField DataField="EventCode" HeaderText="EventCode" SortExpression="EventCode" />
        <asp:BoundField DataField="EventDetailCode" HeaderText="EventDetailCode" SortExpression="EventDetailCode" />
        <asp:BoundField DataField="Message" HeaderText="Message" SortExpression="Message" />
        <asp:BoundField DataField="ApplicationPath" HeaderText="ApplicationPath" SortExpression="ApplicationPath" />
        <asp:BoundField DataField="ApplicationVirtualPath" HeaderText="ApplicationVirtualPath"
            SortExpression="ApplicationVirtualPath" />
        <asp:BoundField DataField="MachineName" HeaderText="MachineName" SortExpression="MachineName" />
        <asp:BoundField DataField="RequestUrl" HeaderText="RequestUrl" SortExpression="RequestUrl" />
        <asp:BoundField DataField="ExceptionType" HeaderText="ExceptionType" SortExpression="ExceptionType" />
        <asp:BoundField DataField="Details" HeaderText="Details" SortExpression="Details" HtmlEncode="false" />
    </Fields>
</asp:DetailsView>
<br />
<br />
<br />
</div>
</asp:Content>
