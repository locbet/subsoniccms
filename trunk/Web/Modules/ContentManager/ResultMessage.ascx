<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResultMessage.ascx.cs" Inherits="Modules_ContentManager_ResultMessage" %>
<%@ Register Src="~/Modules/ContentManager/FlashMessage.ascx" TagName="FlashMessage1" TagPrefix="gcs" %>
<div id="divSuccess" runat="server" class="divSuccess" visible="false">
	<div style="float:left;">
		<img src="<%=Page.ResolveUrl("~/images/icons/icon_check.gif")%>" style="vertical-align:middle" alt="Check" width="40px;"/>	
	</div>
	<div style="float:left;clear:left;padding-left:20px;">
		<gcs:FlashMessage1 ID="flashMessageSuccess" runat="server" Message="Default Success Message" CssClass="validationSuccess" />		
		<asp:Label id="lblSuccess" runat="server" CssClass="validationSuccess"></asp:Label>
	</div>
</div>
<div id="divFail"  runat="server" class="divFail" visible="false">
	<div style="float:left;">
		<img src="<%=Page.ResolveUrl("~/images/icons/icon_error.gif")%>" style="vertical-align:middle" alt="Error" />
	</div>
	<div style="float:left;clear:left;padding-left:20px;">
		<gcs:FlashMessage1 ID="flashMessageFail" runat="server" Message="Default Error Message" CssClass="validationError" />		
		<asp:Label id="lblFail" runat="server" CssClass="validationError"></asp:Label>
	</div>
</div>
<asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
