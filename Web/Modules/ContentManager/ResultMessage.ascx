<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResultMessage.ascx.cs" Inherits="Modules_ContentManager_ResultMessage" %>
<%@ Register Src="~/Modules/ContentManager/FlashMessage.ascx" TagName="LocalFlashMessage" TagPrefix="cms"  %>
<div id="divSuccess" runat="server" class="divSuccess">
	<cms:LocalFlashMessage ID="flashMessageSuccess" runat="server" Message="Default Success Message" CssClass="validationSuccess" />		
</div>
<div id="divFail" runat="server" class="divFail">
	<cms:LocalFlashMessage ID="flashMessageFail" runat="server" Message="Default Fail Message" CssClass="validationFail" />		
</div>
<asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
