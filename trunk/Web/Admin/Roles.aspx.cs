using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class admin_roles : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
    {
		if (!Page.IsPostBack)
		{
			ddlRole.DataSource = allRolesDataSource;
			ddlRole.DataBind();
			updateUsersInRole(ddlRole.SelectedValue);
		}
	}

	protected void Page_Init(object sender, EventArgs e)
	{
		rxvTxtRoleName.ValidationExpression = SiteUtility.Validation.ROLE_REGEX;
	}

    public void LinkButtonClick(object sender, CommandEventArgs e)
    {
        string roleName;

		if (e.CommandName.Equals("deletetherole"))
        {
			try
			{
				roleName = (string)e.CommandArgument;
				RegexStringValidator r = new RegexStringValidator(SiteUtility.Validation.ROLE_REGEX);
				r.Validate(roleName);
				Roles.DeleteRole(roleName, true);
				//new WebEvents.RemoveRolesSuccessEvent(this, roleName).Raise();
				updateGrid();
				ResultMessage1.ShowSuccess(roleName + " was successfully deleted.");
			}
			catch (Exception ex)
			{
				ResultMessage1.ShowFail("Unable to delete role. ", ex);
			}
		}

		if (e.CommandName.Equals("deletetheuser"))
		{
			string userName = (string)e.CommandArgument;
			try
			{
				SiteUtility.DeleteUser(userName);
				updateGrid();
				ResultMessage1.ShowSuccess(userName + " was successfully deleted.");
			}
			catch (Exception ex)
			{
				ResultMessage1.ShowFail(userName + " could not be deleted.", ex);
			}
		}

        if (e.CommandName.Equals("add"))
        {
			try
			{
				roleName = txtRoleName.Text.Trim();
				RegexStringValidator r = new RegexStringValidator(SiteUtility.Validation.ROLE_REGEX);
				r.Validate(roleName);
				Roles.CreateRole(roleName);
				//new WebEvents.AddRolesSuccessEvent(this, roleName).Raise();
				updateGrid();
				ResultMessage1.ShowSuccess(roleName + " was successfully added.");
			}
			catch (Exception ex)
			{
				ResultMessage1.ShowFail("Unable to add role.", ex);
			}
        }
    }

	void updateGrid()
	{
		allRolesDataSource.DataBind();
		GridView1.DataBind();
		ddlRole.DataSource = allRolesDataSource;
		ddlRole.DataBind();
	}

	protected void ddlRoles_SelectedIndexChanged (object sender, EventArgs e)
	{
		updateUsersInRole(ddlRole.SelectedValue);
	}

	void updateUsersInRole(string role)
	{
		string[] items = Roles.GetUsersInRole(ddlRole.SelectedValue);
		MembershipUserCollection coll = new MembershipUserCollection();
		foreach (string item in items)
		{
			MembershipUser u = Membership.GetUser(item);
			if (u != null)
				coll.Add(Membership.GetUser(item));
		}

		GridView2.DataSource = coll;
		GridView2.DataBind();
	}

	public void EnabledChanged(object sender, EventArgs e)
	{
		CheckBox checkBox = sender as CheckBox;
		if (checkBox == null)
			return;

		try
		{
			ResultMessage1.ShowSuccess(SiteUtility.ToggleUserApprovedStatus(checkBox));
		}
		catch (Exception ex)
		{
			ResultMessage1.ShowFail("Unable to change account approval status.", ex);
		}
	}

}
