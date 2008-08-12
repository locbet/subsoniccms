<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="PageView.aspx.cs" Inherits="PageView" Title="Untitled Page" %>
<%@ MasterType virtualpath="~/site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
<asp:Panel ID="pnlPublic" runat="server">
</asp:Panel>

<asp:Panel ID="pnlEdit" runat="server">
    <asp:UpdatePanel ID="upnlEdit" runat="server">
    <ContentTemplate>
    <h1><asp:Label ID="editorTitle" runat="server"></asp:Label></h1>
    <div>
        <cms:ResultMessage id="ResultMessage1" runat="server" />

        <p>
			<b>Title</b><br />
			<i>This shows at the top of each page. If this is a dynamic page, the page's URL is based on this title.</i><br />
			<asp:TextBox ID="txtTitle" runat="server" Width="461px"></asp:TextBox><br />
				<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle"
					ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
        </p>
        <p> 
			<b>Summary</b><br />
			<i>Used for searches and summary listings - create a quick summary of what this page is about.</i><br />
			<i>The summary appears underneat the Title, and also appears as mouseover text on the menu item.</i><br />
			<asp:TextBox ID="txtSummary" runat="server" Width="800px" Height="50px" TextMode="MultiLine"></asp:TextBox>
        </p>
        <p>
	        <b>Show in Menu</b><br />
            <asp:CheckBox runat="server" ID="chkShowInMenu" /><i> Check this box to make the page appear in the site menu at the top of each page.</i>
        </p>
		<p>
			<b>Menu Title</b><br />
			<i>The name this page uses in the site menu system.</i><br />
			<asp:TextBox ID="txtMenuTitle" runat="server" Width="200px"></asp:TextBox><br />
				<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMenuTitle"
					ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
		</p>
        <p>
			<b>Location</b><br />
			<i>Where this page is currently located in the hierarchy.</i><br />
				<asp:ListBox ID="lstHierarchy" runat="server" Height="120px" Width="350px" Enabled="False"></asp:ListBox>
        </p>
        <p>
          <b>Parent Page</b><br />
          <i>Select "None" to make this page a new default menu item--it will always be visible on the menu bar (without clicking anything).</i><br />
          <asp:DropDownList ID="ParentID" runat="server"></asp:DropDownList>
            <asp:Button ID="btnSetParent" runat="server" Text="Set" OnClick="btnSetParent_Click" />
        </p>
        <p>
        <b>Ordinal</b><br />
        <i>The order in which this page should be added to the site menu, relative to the other pages in the group.</i><br />
        <asp:TextBox ID="txtOrdinal" runat="server" Text="99" TextMode="SingleLine" MaxLength="3"></asp:TextBox><asp:RangeValidator ID="rvTxtOrdinal" runat="server" Display="Dynamic" ControlToValidate="txtOrdinal" ErrorMessage="Please enter a number between 0 and 99" MinimumValue="0" MaximumValue="99"></asp:RangeValidator>
        <ajax:NumericUpDownExtender ID="nudTxtOrdinal" runat="server" Minimum="1" Maximum="99" Width="50" TargetControlID="txtOrdinal"></ajax:NumericUpDownExtender>
        </p>
        <p>
        <b>Keywords</b><br />
        <i>These values are put into the META tag, which is used by search engines.</i><br />
        <asp:TextBox ID="txtKeywords" runat="server" Width="460px" Height="97px" TextMode="MultiLine"></asp:TextBox>
        </p>
        <p>
			<b>Show Edit Links on page</b><br />
	        <i>Determines if the "Edit", "New" and "Page List" buttons should appear for the page.</i><br />
            <asp:CheckBox runat="server" ID="chkShowEditLinks"></asp:CheckBox>
        </p>

        <p>
			<b>Roles</b><br />
	        <i>Determines which user groups can edit content on the page.</i><br />
            <asp:CheckBoxList runat="server" ID="chkRoles"></asp:CheckBoxList>
        </p>
<%--        </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upnlPageType" runat="server">
			<ContentTemplate>
--%>		<p>
	            <b>Page Type</b><br />
	            <i>The type of page you are setting up. The default is "Dynamic Content".</i><br />
	            <asp:DropDownList ID="ddlPageType" runat="server" OnSelectedIndexChanged="ddlPageType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </p>
            <asp:Panel ID="pnlStaticURL" runat="server" Visible="false">
            <p>
	            <b>Static Content</b><br />
	            <i>Attach an existing static page. Write this in URL form, relative to the site root.</i><br />
	            <asp:TextBox ID="txtStaticURL" runat="server" ></asp:TextBox>
            </p>
            </asp:Panel>
            <p>
	            <b>Body</b><br />
	            <fck:FCKeditor id="Body" BasePath="~/FCKeditor/" runat="server"  Height="700" Width="800"/>
            </p>
            <p>
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false"/>
                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
               
            </p>
			</ContentTemplate>
			<Triggers>
			    <asp:AsyncPostBackTrigger ControlID="ddlPageType" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />			    
			</Triggers>
		</asp:UpdatePanel>
    </div>

    <script type="text/javascript">
    function CheckDelete(){
    		
	    return confirm("Delete this page? This action cannot be undone...");

    }

    </script>
</asp:Panel>
</asp:Content>

