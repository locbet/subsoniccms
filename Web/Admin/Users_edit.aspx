<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Users_edit.aspx.cs" Inherits="admin_user_edit" Title="Edit User" %>
<%@ MasterType virtualpath="~/site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">

<table style="width:100%;padding:0;border:0">
<tr>
	<td><cms:ResultMessage ID="ResultMessage1" runat="server" /></td>
</tr>
<tr>
   <td><h1><asp:literal ID="ActionTitle" runat="server" text="Edit User" /></h1></td>
</tr>
<tr>
    <td>
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="UserID" Text="Username: " CssClass="adminlabel"/>
                </td>
                <td>
                    <asp:textbox runat="server" id="UserID" maxlength="255" tabindex="101" Columns="30"  CssClass="adminlabel"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserID" Display="Dynamic" EnableClientScript="true">required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" AssociatedControlID="Email" Text="Email Address: " CssClass="adminlabel"/>
                </td>
                <td>
                    <asp:textbox runat="server" id="Email" maxlength="128" tabindex="102" Columns="30" CssClass="adminlabel" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Email" Display="Dynamic" EnableClientScript="true">required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" AssociatedControlID="FirstName" Text="First Name: " CssClass="adminlabel"/>
                </td>
                <td>
                    <asp:textbox runat="server" id="FirstName" maxlength="128" tabindex="103" Columns="30" CssClass="adminlabel" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="FirstName" Display="Dynamic" EnableClientScript="true">required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" AssociatedControlID="LastName" Text="Last Name: " CssClass="adminlabel"/>
                </td>
                <td>
                    <asp:textbox runat="server" id="LastName" maxlength="128" tabindex="103" Columns="30" CssClass="adminlabel" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="LastName" Display="Dynamic" EnableClientScript="true">required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" AssociatedControlID="CommonName" Text="Common Name: " CssClass="adminlabel"/>
                </td>
                <td>
                    <asp:textbox runat="server" id="CommonName" maxlength="128" tabindex="103" Columns="30" CssClass="adminlabel" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="CommonName" Display="Dynamic" EnableClientScript="true">required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" AssociatedControlID="ThemePreference" Text="Theme Preference" CssClass="adminlabel"/>
                </td>
                <td>
                    <asp:DropDownList ID="ThemePreference" runat="server">
                        <asp:ListItem Text="Default" Value="Default"></asp:ListItem>
                        <asp:ListItem Text="Blue" Value="Blue"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ThemePreference" Display="Dynamic" EnableClientScript="true">required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="PasswordRow" runat="server" visible="false">
                <td>
                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="Password" Text="Password: "/>
                </td>
                <td>
                    <asp:textbox runat="server" id="Password" maxlength="50" tabindex="103" Columns="20" TextMode="Password" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Password" Display="Dynamic" EnableClientScript="true">required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="NewPasswordRow" runat="server" visible="false">
                <td>
                    <asp:Label ID="lblNewPassword" runat="server" AssociatedControlID="NewPassword" Text="New Password: "/>
                </td>
                <td>
                    <asp:textbox runat="server" id="NewPassword" maxlength="50" tabindex="104" Columns="20" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="NewPassword" Display="Dynamic" EnableClientScript="true">required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="SecretQuestionRow" runat="server" visible="false">
                <td>
                    <asp:Label ID="Label5" runat="server" AssociatedControlID="SecretQuestion" Text="Secret Question: "/>
                </td>
                <td>
                    <asp:textbox runat="server" id="SecretQuestion" maxlength="128" tabindex="105" Columns="30" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="SecretQuestion" Display="Dynamic" EnableClientScript="true">required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="SecretAnswerRow" runat="server" visible="false">
                <td>
                    <asp:Label ID="Label6" runat="server" AssociatedControlID="SecretAnswer" Text="Secret Answer: "/>
                </td>
                <td>
                    <asp:textbox runat="server" id="SecretAnswer" maxlength="128" tabindex="106" Columns="30" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="SecretAnswer" Display="Dynamic" EnableClientScript="true">required</asp:RequiredFieldValidator>
                </td>
            </tr>  
            <tr>
                <td>
                    <asp:checkbox runat="server" id="ActiveUser" text="Active User" TabIndex="107" Checked="True" OnCheckedChanged="EnabledChanged" AutoPostBack="true"/>
                </td>
                <td><asp:button runat="server" id="unlockUser" text="Unlock Account" TabIndex="108" OnClick="unlockAccount_Click" CssClass="frmbutton"/>
                </td>
            </tr>   
                                  
            <tr>
                <td colspan="2">
					<br />
					<br />
                    <h1><asp:label runat="server" id="SelectRolesLabel" text="Select User Roles"/></h1>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:repeater runat="server" id="CheckBoxRepeater">
                        <itemtemplate>
                            <asp:checkbox runat="server" id="checkBox1" text='<%# Container.DataItem.ToString()%>' checked='<%# IsUserInRole(Container.DataItem.ToString())%>' CssClass="adminlabel"/>
                            <br/>
                        </itemtemplate>
                    </asp:repeater>
                </td>
            </tr>
            <tr>                                
               <td colspan="2">
                    <br />
                    <asp:button runat="server" id="SaveButton" onClick="SaveClick" text="Save" width="100" CssClass="frmbutton"/>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br /><br />
                    <a href="users.aspx" class="frmbutton">Back to user list.</a>
                </td>
            </tr>
        </table>                        
    </td>
</tr>
</table>

</asp:Content>
