<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Site_Login" Title="Login" %>
<%@ MasterType virtualpath="~/site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="centercontent">
    
        <table class="logtable">

            <tr>
                <td>
              
                    <asp:Login ID="Login1" runat="server"
                    PasswordRecoveryText="Forgot Password?" 
                    PasswordRecoveryUrl="~/PasswordRecover.aspx">
						<TextBoxStyle Width="130px" />
					</asp:Login>
                </td>
                
            </tr>
        
        </table>
    
    </div>

</asp:Content>

