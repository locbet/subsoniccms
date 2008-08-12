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
using System.Web.Administration;

public partial class admin_users : BasePage
{
//	private CMS.Page _thisPage = new CMS.Page();

	protected void Page_Load(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        //{
        //    _thisPage = new CMS.Page();
        //    SiteUtility.BuildBasePage((BasePage)this, ref _thisPage, "Admin/Users.aspx");
        //    Master.thisPage = _thisPage;
        //}
	}

	protected void Page_Init(object sender, EventArgs e)
	{
		rxvTextBox1.ValidationExpression = SiteUtility.Validation.USER_SEARCH_REGEX;
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

	public void SearchForUsers(object sender, EventArgs e)
    {
        if (TextBox1.Text != "")
        {
            GridView1.DataSourceID = "";
            SearchForUsers(sender, e, GridView1, SearchByDropDown, TextBox1);
        }
    }

    protected void SearchForUsers(object sender, EventArgs e, GridView dataGrid, DropDownList dropDown, TextBox textBox)
    {
        ICollection coll = null;
        string text = textBox.Text;
        text = text.Replace("*", "%");
        text = text.Replace("?", "_");

		try
		{

			RegexStringValidator r = new RegexStringValidator(SiteUtility.Validation.USER_SEARCH_REGEX);
			r.Validate(text);

			if (dropDown.SelectedIndex == 0 /* userID */)
			{
				coll = Membership.FindUsersByName(text);
			}
			else if (dropDown.SelectedIndex == 1)
			{
				coll = Membership.FindUsersByEmail(text);
			}

			dataGrid.PageIndex = 0;
			dataGrid.DataSource = coll;
			dataGrid.DataBind();
		}
		catch (Exception ex)
		{
			ResultMessage1.ShowFail("Unable to perform the search.", ex);
		}

    }

    public void LinkButtonClick(object sender, CommandEventArgs e)
    {
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
				ResultMessage1.ShowFail(userName + " could not be deleted. ", ex);
			}
        }
    }

	void updateGrid()
	{
		allUsersDataSource.DataBind();
		GridView1.DataBind();
	}

    protected void allUsersDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
    }
}
