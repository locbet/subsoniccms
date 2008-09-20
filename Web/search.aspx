<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="search_default" Title="Untitled Page" %>
<%@ MasterType virtualpath="~/site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMessageBar" Runat="Server">
	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
		<ContentTemplate>
		<div style="width:100%">
			<cms:ResultMessage ID="ResultMessage1" runat="server" />
		</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<div style="width:100%">
<asp:Panel ID="pnlTxtSearch" runat="server" DefaultButton="btnSearch">
	<asp:TextBox ID="txtSearch" runat="server" MaxLength="100" TextMode="SingleLine" ValidationGroup="searchValidation" CausesValidation="true" Width="300px"></asp:TextBox>
	<ajax:TextBoxWatermarkExtender ID="txtweSearch" runat="server" TargetControlID="txtSearch" WatermarkText="Search" WatermarkCssClass="watermarktext"></ajax:TextBoxWatermarkExtender>
	<ajax:AutoCompleteExtender ID="aceSearch" runat="server" TargetControlID="txtSearch" ServiceMethod="GetSearchCompletionList" ServicePath="~/entities.asmx" MinimumPrefixLength="1" ></ajax:AutoCompleteExtender>
	<asp:Button ID="btnSearch" runat="server" Text="Go!" SkinID="btnSearch" CausesValidation="true" ValidationGroup="searchValidation" OnClick="btnSearch_Click"/>
	<asp:RequiredFieldValidator ID="rfvTxtSearch" runat="server" ControlToValidate="txtSearch" Display="Dynamic" ErrorMessage="*Required" ValidationGroup="searchValidation"></asp:RequiredFieldValidator>
	<asp:RegularExpressionValidator ID="rxvTxtSearch" runat="server" ControlToValidate="txtSearch" Display="Dynamic" ErrorMessage="Alpha-Numeric characters only. Try removing all punctuation from the search criteria and running the search again." ValidationExpression="[a-zA-Z0-9][a-zA-Z0-9\s]+" ValidationGroup="searchValidation"></asp:RegularExpressionValidator>
	<br />
	<br />
</asp:Panel>


<asp:Repeater ID="rptResults" runat="server">
	<ItemTemplate>
		<div class="searchResult">
			<a href="<%# DataBinder.Eval(Container.DataItem, "url") %>" target="_blank"><%# DataBinder.Eval(Container.DataItem, "name") %></a><br />
			<div class="searchDescription"><%# DataBinder.Eval(Container.DataItem, "description") %></div>
		</div>
	</ItemTemplate>
	<AlternatingItemTemplate>
		<div class="searchResultAlt">
			<a href="<%# DataBinder.Eval(Container.DataItem, "url") %>" target="_blank"><%# DataBinder.Eval(Container.DataItem, "name") %></a><br />
			<div class="searchDescription"><%# DataBinder.Eval(Container.DataItem, "description") %></div>
		</div>
	</AlternatingItemTemplate>
</asp:Repeater>
</div>
</asp:Content>

