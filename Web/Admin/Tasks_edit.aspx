<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Tasks_edit.aspx.cs" Inherits="admin_task_edit" Title="Edit Task" %>
<%@ MasterType virtualpath="~/site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
<asp:UpdatePanel runat="server" UpdateMode="Conditional"><ContentTemplate>
<table style="width:100%;padding:0;border:0">
<tr>
   <td><h1><asp:literal ID="ActionTitle" runat="server" text="Edit Task" /></h1></td>
</tr>
<tr>
    <td>
        <table>
			<tr>
				<th scope="row"><asp:Label ID="lblApplicationName" runat="server" AssociatedControlID="ddlApplicationName" Text="Application"></asp:Label></th>
				<td><asp:DropDownList ID="ddlApplicationName" runat="server" DataTextField="name" DataValueField="TaskApplicationID">
				</asp:DropDownList></td>
			</tr>
			<tr>
				<th scope="row"><asp:Label ID="lblTaskName" runat="server" AssociatedControlID="txtTaskName" Text="Name"></asp:Label></th>
				<td><asp:TextBox ID="txtTaskName" runat="server" Width="255"></asp:TextBox><asp:RequiredFieldValidator runat="server" ControlToValidate="txtTaskName" ErrorMessage="*Required" Display="Dynamic"></asp:RequiredFieldValidator></td>
			</tr>
			<tr>
				<th scope="row"><asp:Label ID="lblParameters" runat="server" AssociatedControlID="txtParameters" Text="Parameters"></asp:Label></th>
				<td><asp:TextBox ID="txtParameters" runat="server" Width="255"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtParameters" ErrorMessage="*Required" Display="Dynamic"></asp:RequiredFieldValidator></td>
			</tr>
			<tr>
				<th scope="row"><asp:Label ID="lblComment" runat="server" AssociatedControlID="txtComment" Text="Comment"></asp:Label></th>
				<td><asp:TextBox ID="txtComment" runat="server" Width="255"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtComment" ErrorMessage="*Required" Display="Dynamic"></asp:RequiredFieldValidator></td>
			</tr>
<%--		<tr>
				<th scope="row"><asp:Label ID="lblWorkingDir" runat="server" AssociatedControlID="txtWorkingDir" Text="Working Directory"></asp:Label></th>
				<td><asp:TextBox ID="txtWorkingDir" runat="server" Width="255"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtWorkingDir" ErrorMessage="*Required" Display="Dynamic"></asp:RequiredFieldValidator></td>
			</tr>
--%>
<%--		<tr>
				<th scope="row"><asp:Label ID="lblPriority" runat="server" AssociatedControlID="txtPriority" Text="Priority"></asp:Label></th>
				<td><asp:TextBox ID="txtPriority" runat="server" Width="255"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPriority" ErrorMessage="*Required" Display="Dynamic"></asp:RequiredFieldValidator></td>
			</tr>--%>
			<tr>
				<th scope="row"><asp:Label ID="lblStatus" runat="server" AssociatedControlID="txtStatus" Text="Status"></asp:Label></th>
				<td><asp:TextBox ID="txtStatus" runat="server" Width="255" Enabled="false"></asp:TextBox></td>
			</tr>
			<tr>
				<th scope="row"><asp:Label ID="lblFlagList" runat="server" AssociatedControlID="chklFlagList" Text="Flags"></asp:Label></th>
				<td><asp:CheckBoxList ID="chklFlagList" runat="server"/></td>
			</tr>
			<tr runat="server" id="trTriggerList" visible="false">
				<th scope="row"><asp:Label ID="lblTriggerList" runat="server" AssociatedControlID="dgTriggerList" Text="Triggers"></asp:Label></th>
				<td><asp:DataGrid ID="dgTriggerList" runat="server" AutoGenerateColumns="false" SkinID="sortable">
						<Columns>
							<asp:BoundColumn DataField="BeginDate" HeaderText="Start"></asp:BoundColumn>
							<asp:BoundColumn DataField="EndDate" HeaderText="End"></asp:BoundColumn>
							<asp:BoundColumn DataField="DurationMinutes" HeaderText="Duration (minutes)"></asp:BoundColumn>
							<asp:BoundColumn DataField="IntervalMinutes" HeaderText="Interval (minutes)"></asp:BoundColumn>
							<asp:BoundColumn DataField="Disabled" HeaderText="Disabled"></asp:BoundColumn>
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:linkButton runat="server" id="linkButton1" text="Delete" commandname="deletethetrigger" commandargument='<%# Container.ItemIndex.ToString()%>' forecolor="black" oncommand="LinkButtonClick"/>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
				</td>
			</tr>
			<tr>
				<th scope="row"><asp:Label ID="Label1" runat="server" Text="Add New Trigger"></asp:Label></th>
				<td><div id="divNewTriggerMessage" runat="server" visible="true">
					(Save the new task before adding triggers)
					</div>
				    <div id="divNewTrigger" runat="server" visible="false">
				        <table>
				            <tr>
				                <th scope="row">Add New Trigger</th>
				                <td><asp:CheckBox ID="cbAddNewTrigger" runat="server" Checked="false" /></td>
				            </tr>
				            <tr>
				                <th scope="row">Trigger Type</th>
				                <td><asp:DropDownList ID="ddlTriggerType" runat="server">
				                        <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
				                        <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
				                    </asp:DropDownList>
        				        </td>
				            </tr>
				            <tr>
				                <th scope="row">Start Date</th>
				                <td><subsonic:CalendarControl ID="calStartDate" runat="server"></subsonic:CalendarControl></td>
				            </tr>
				            <tr>
				                <th scope="row">Repeat Every</th>
				                <td><asp:TextBox ID="txtRepeatHour" runat="server" Text="0" TextMode="SingleLine" MaxLength="3"></asp:TextBox><asp:RangeValidator ID="rvTxtRepeatHour" runat="server" Display="Dynamic" ControlToValidate="txtRepeatHour" ErrorMessage="Please enter a number between 0 and 23" MinimumValue="0" MaximumValue="23" Type="Integer"></asp:RangeValidator>
                                    <ajax:NumericUpDownExtender ID="nudTxtRepeatHour" runat="server" Minimum="0" Maximum="23" Width="50" TargetControlID="txtRepeatHour"></ajax:NumericUpDownExtender>&nbsp;Hours&nbsp;
                                    <asp:TextBox ID="txtRepeatMinute" runat="server" Text="0" TextMode="SingleLine" MaxLength="3"></asp:TextBox><asp:RangeValidator ID="rvTxtRepeatMinute" runat="server" Display="Dynamic" ControlToValidate="txtRepeatMinute" ErrorMessage="Please enter a number between 0 and 59" MinimumValue="0" MaximumValue="59" Type="Integer"></asp:RangeValidator>
                                    <ajax:NumericUpDownExtender ID="nudTxtRepeatMinute" runat="server" Minimum="0" Maximum="59" Width="50" TargetControlID="txtRepeatMinute"></ajax:NumericUpDownExtender>&nbsp;Minutes
                                </td>
				            </tr>

				            <tr>
				                <th scope="row">Repeat Until</th>
				                <td><subsonic:CalendarControl ID="calEndDate" runat="server"></subsonic:CalendarControl></td>
				            </tr>
				            <tr>
				                <th scope="row">End Date</th>
				                <td><asp:CheckBox ID="cbRepeatUntilIsEndDate" runat="server" Text="Use repeat until date as end date" Checked="false"  /></td>
				            </tr>
	            			<tr>
								<th scope="row"><asp:Label ID="lblDaysOfWeek" runat="server" AssociatedControlID="chklDaysOfWeek" Text="Days of Week (Weekly Tasks)"></asp:Label></th>
								<td><asp:CheckBoxList ID="chklDaysOfWeek" runat="server"/></td>
							</tr>

				        </table>
				    </div>
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
                    <a href="tasks.aspx" class="frmbutton">Back to task list.</a>
                </td>
            </tr>
        </table>                        
    </td>
</tr>
</table>
</ContentTemplate><Triggers><asp:AsyncPostBackTrigger ControlID="SaveButton" EventName="Click" /></Triggers></asp:UpdatePanel>
</asp:Content>
