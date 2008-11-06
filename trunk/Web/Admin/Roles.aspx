<%@ Page Language="C#"  AutoEventWireup="true" MasterPageFile="~/site.master"  CodeFile="Roles.aspx.cs" Inherits="admin_roles" Title="Roles Administration" %>
<%@ MasterType virtualpath="~/site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
<div id="centercontent">
<br />
    <b><a href="users.aspx">Users</a></b> | <b>Roles</b>
<br />
<br />

    <asp:GridView ID="GridView1" runat="server"  SkinID="subsonicSkin"
        AutoGenerateColumns="false" 
        DataSourceID="allRolesDataSource" 
        EmptyDataText="There are no matching roles in the system." 
        Font-Italic="False">
        <Columns>
            <asp:TemplateField HeaderText="Role Name" ItemStyle-Width="200">
                <ItemTemplate>
                    <%# Container.DataItem %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton runat="server" id="linkButton1" text="Delete" commandname="deletetherole" CommandArgument='<%# Container.DataItem %>' forecolor="black" oncommand="LinkButtonClick" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataRowStyle Font-Italic="True" />
    </asp:GridView>
    
    <br />
    
    <div>
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtRoleName" runat="server" ValidationGroup="new" CssClass="adminlabel" MaxLength="50"/>
                    <asp:Button runat="server" id="linkButton2" text="Add" commandname="add" forecolor="black" oncommand="LinkButtonClick" ValidationGroup="new" CssClass="frmbutton" />
                    <asp:RequiredFieldValidator ID="RoleNameRequiredFieldValidator" runat="server" ControlToValidate="txtRoleName" Display="Dynamic" EnableClientScript="true" ValidationGroup="new">required</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="rxvTxtRoleName" runat="server" ControlToValidate="txtRoleName" ValidationGroup="new" Display="Dynamic" Text="Roles must be at least two characters in length, and contain a combination of numbers and letters. Spaces and hypens are also allowed, as long as they are not the first character." ></asp:RegularExpressionValidator>
                </td>
            </tr>
        </table>
    </div>
    
    <h2>Users in Roles</h2>
    <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRoles_SelectedIndexChanged"></asp:DropDownList>
    <asp:GridView ID="GridView2" runat="server" SkinID="subsonicSkin"
        AutoGenerateColumns="False" 
        EmptyDataText="No users in the selected role." 
        Font-Italic="False" >
        <Columns>
            <asp:templatefield headertext="Approved" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center">
                <headerstyle horizontalalign="center"/>
                <itemstyle horizontalalign="center"/>
                <itemtemplate>
                    <asp:checkBox runat="server" id="CheckBox1" oncheckedchanged="EnabledChanged" autopostback="true" checked='<%#DataBinder.Eval(Container.DataItem, "IsApproved")%>' Value='<%#DataBinder.Eval(Container.DataItem, "UserName")%>'/>
                </itemtemplate>
            </asp:templatefield>
            <asp:templatefield headertext="Locked" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center">
                <headerstyle horizontalalign="center"/>
                <itemstyle horizontalalign="center"/>
                <itemtemplate>
                    <asp:checkBox runat="server" id="CheckBox2" Enabled="false" checked='<%#DataBinder.Eval(Container.DataItem, "IsLockedOut")%>' Value='<%#DataBinder.Eval(Container.DataItem, "UserName")%>'/>
                </itemtemplate>
            </asp:templatefield>
            <asp:templatefield runat="server" headertext="User Name" ItemStyle-Width="160" >
                <itemtemplate>
                    <a href='users_edit.aspx?username=<%#Eval("UserName")%>'><%#DataBinder.Eval(Container.DataItem, "UserName")%></a>
                </itemtemplate>
            </asp:templatefield>                            
            <asp:TemplateField HeaderText="Email" ItemStyle-Width="180" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:HyperLink ID="EmailLink" runat="server" NavigateUrl='<%# Eval("Email", "mailto:{0}") %>' Text='<%# Eval("Email") %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CreationDate" HeaderText="Created On" ReadOnly="True" SortExpression="CreationDate" ItemStyle-HorizontalAlign="Center"/>
            <asp:BoundField DataField="LastLoginDate" HeaderText="Last Login" SortExpression="LastLoginDate" ItemStyle-HorizontalAlign="Center"/>
            <asp:templatefield runat="server" ItemStyle-HorizontalAlign="Center">
                <itemtemplate>
                    <asp:linkButton runat="server" id="linkButton2" text="Delete" commandname="deletetheuser" commandargument='<%#DataBinder.Eval(Container.DataItem, "UserName")%>' forecolor="black" oncommand="LinkButtonClick"/>
                </itemtemplate>
            </asp:templatefield>
        </Columns>
        <EmptyDataRowStyle Font-Italic="True" />
    </asp:GridView>


    <asp:ObjectDataSource ID="allRolesDataSource" runat="server"
        SelectMethod="GetAllRoles"
        TypeName="System.Web.Security.Roles" >
    </asp:ObjectDataSource>
</div>
</asp:Content>
