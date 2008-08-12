<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="CMSPageList.aspx.cs" Inherits="Admin_CMSPageList" Title="CMS Pages" %>
<%@ MasterType virtualpath="~/site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
    <asp:GridView ID="grdAllPages" runat="server" SkinID="subsonicSkin"
        AutoGenerateColumns="False" 
        DataSourceID="" 
        EmptyDataText="No active pages." 
        Font-Italic="False" >
        <Columns>
            <asp:BoundField DataField="pageID" HeaderText="ID" ReadOnly="True" SortExpression="pageID" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="title" HeaderText="Page Title" ReadOnly="True" SortExpression="Title" ItemStyle-HorizontalAlign="Left"/>
            <asp:BoundField DataField="menutitle" HeaderText="Menu Title" ReadOnly="True" SortExpression="menuTitle" ItemStyle-HorizontalAlign="Left"/>
            <asp:templatefield headertext="URL" ItemStyle-Width="160" >
                <itemtemplate>
                    <asp:linkButton runat="server" id="linkButton2" text='<%#DataBinder.Eval(Container.DataItem, "PageURL")%>' commandname="ViewPage" commandargument='<%#DataBinder.Eval(Container.DataItem, "pageID")%>' forecolor="black" oncommand="LinkButtonClick"/>
                </itemtemplate>
            </asp:templatefield>                            
            <asp:templatefield ItemStyle-HorizontalAlign="Center">
                <itemtemplate>
                    <asp:linkButton runat="server" id="linkButton3" text="Edit" commandname="EditPage" commandargument='<%#DataBinder.Eval(Container.DataItem, "pageID")%>' forecolor="black" oncommand="LinkButtonClick"/>
                </itemtemplate>
            </asp:templatefield>
            <asp:BoundField DataField="pageTypeID" HeaderText="Page Type" ReadOnly="True" SortExpression="pageTypeID" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="Ordinal" HeaderText="Ordinal" ReadOnly="True" SortExpression="Ordinal" ItemStyle-HorizontalAlign="Right"/>
        </Columns>
        <EmptyDataRowStyle Font-Italic="True" />
    </asp:GridView>
</asp:Content>

