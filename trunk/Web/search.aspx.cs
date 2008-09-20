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
using System.Collections.Specialized;

public partial class search_default : BasePage
{
	private DataTable dtResults;

	protected void Page_Load(object sender, EventArgs e)
    {
		string q = HttpUtility.UrlDecode(SiteUtility.GetParameter("question"));
		//setup all the data here
		if (!SiteUtility.UserCanSearch())
		{
			Response.Redirect("~/login.aspx?ReturnURL=search/" + (!String.IsNullOrEmpty(q) ? HttpUtility.UrlEncode(q.Trim()) : ""));
		}

		if (!Page.IsPostBack)
		{
			try
			{
				showSearch(q);
			}
			catch (Exception ex)
			{
				ResultMessage1.ShowFail("Failed to load search", ex);
			}
		}
    }

	private void showSearch(string searchText)
	{
		if (!String.IsNullOrEmpty(searchText) && SubSonic.Sugar.Validation.IsAlphaNumeric(searchText, true) && searchText != "default.aspx")
		{
			txtSearch.Text = searchText;
			try
			{
				StringCollection list = Profile.SearchTerms;
				if (!list.Contains(searchText))
				{
					list.Add(searchText);
				}
				dtResults = new DataTable();
				dtResults.Columns.Add("url");
				dtResults.Columns.Add("name");
				dtResults.Columns.Add("description");
				dtResults.Columns.Add("relevance");

				showContent(searchText);

                //TODO: Add a display for zero results here.
				if (dtResults.Rows.Count == 0)
					ResultMessage1.ShowFail("No results found.");
				else if (dtResults.Rows.Count == 1)
					Response.Redirect(dtResults.Rows[0]["url"].ToString());
				else
				{
					rptResults.DataSource = dtResults;
					rptResults.DataBind();
				}
			}
			catch (Exception ex)
			{
				ResultMessage1.ShowFail("Failure loading search results. ", ex);
			}
		}
		else
		{
			ResultMessage1.ShowFail("Invalid search criteria. Please try your search again.");
		}
	}

	private void showContent(string searchText)
	{
        //this is REALLY basic, but its a start
        foreach (CMS.Page p in CMS.ContentService.Search(searchText))
        {
            dtResults.Rows.Add(Page.ResolveUrl(p.FinalUrl),
            p.Title,
            p.Summary,
            100);
        }
	}

	protected void btnSearch_Click(object sender, EventArgs e)
	{
		if (Page.IsValid && SubSonic.Sugar.Validation.IsAlphaNumeric(txtSearch.Text, true))
		{
			Response.Redirect("~/search/" + HttpUtility.UrlEncode(txtSearch.Text.Trim()));
		}
		else
		{
			ResultMessage1.ShowFail("Invalid search criteria. Please try your search again.");
            new WebEvents.InputValidationEvent(this, "Search Validation Failed").Raise();
		}
	}
}
