<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResultMessage.ascx.cs" Inherits="Modules_ContentManager_ResultMessage" %>
<%@ Register Src="~/Modules/ContentManager/FlashMessage.ascx" TagName="FlashMessage1" TagPrefix="gcs" %>
<div id="divSuccess" runat="server" class="divSuccess" visible="false">
	<gcs:FlashMessage1 ID="flashMessageSuccess" runat="server" Message="Default Success Message" CssClass="validationSuccess" />		
</div>
<div id="divFail"  runat="server" class="divFail" visible="false">
	<gcs:FlashMessage1 ID="flashMessageFail" runat="server" Message="Default Error Message" CssClass="validationError" />		
</div>
<asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
