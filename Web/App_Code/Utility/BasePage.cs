using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Text;
using System;
/// <summary>
/// Base Page for all Starter Site .aspx pages.
/// </summary>
public class BasePage : System.Web.UI.Page
{
    protected override void OnPreInit(System.EventArgs e)
    {
        base.OnPreInit(e);
		string pageUrl = SiteUtility.RemoveRootFromUrl(this.Request.Url.AbsolutePath);
		System.Web.Profile.ProfileBase MyProfile;
		MyProfile = HttpContext.Current.Profile;

		//if a master page is already declared for this BasePage, try updating it
		if (!string.IsNullOrEmpty(this.MasterPageFile))
        {
            this.MasterPageFile = "~/site.master"; //MyProfile.GetPropertyValue("MasterFilePreference");
            this.Theme = (string.IsNullOrEmpty(MyProfile.GetPropertyValue("ThemePreference").ToString()) ? "Default" : MyProfile.GetPropertyValue("ThemePreference").ToString());
        }
		else if (String.IsNullOrEmpty(this.Theme))
		{
			///GCS-690: New Email page isn't themed
			this.Theme = (string.IsNullOrEmpty(MyProfile.GetPropertyValue("ThemePreference").ToString()) ? "Default" : MyProfile.GetPropertyValue("ThemePreference").ToString());
		}

    }

    protected override void OnLoad(System.EventArgs e)
    {
        if (this.Master != null)
            BuildBasePage(null);
		base.OnLoad(e);
	}

    public void BuildBasePage()
    {
        BuildBasePage(null);
    }

    public void BuildBasePage(string pageUrl)
    {
        BaseMasterPage m = (BaseMasterPage)this.Master;
		if (pageUrl == null || string.IsNullOrEmpty(pageUrl))
		{
			pageUrl = SiteUtility.RemoveRootFromUrl(m.Request.Url.AbsolutePath);
			if (pageUrl == "pageview.aspx")
			{
				string _pageUrl = SiteUtility.GetParameter("p");
				pageUrl = (string.IsNullOrEmpty(_pageUrl) ? pageUrl : _pageUrl.ToLower().Replace("newpage.aspx","view/newpage.aspx"));
                if (pageUrl.ToLower() == "editpage.aspx")
                {
                    _pageUrl = string.Empty;
                    _pageUrl = SiteUtility.GetParameter("pRef");
                    pageUrl = (string.IsNullOrEmpty(_pageUrl) ? pageUrl : _pageUrl);
                }
			}
		}

		if (m.thisPage == null)
		{
            try
            {
                SiteUtility.BuildBasePage(this, m, pageUrl);
            }
            catch (CMS.PageNotAuthorizedException)
            {
                new WebEvents.PageAccessFailureEvent(this.Request, this.User.Identity.Name, pageUrl).Raise();
				pageUrl = SiteUtility.RemoveRootFromUrl(this.Request.RawUrl);				
                SiteUtility.Redirect(403, pageUrl);
            }
            catch (Exception ex)
            {
				new WebEvents.InputValidationEvent(this.Request, ex.Message, ex).Raise();
                new WebEvents.PageAccessFailureEvent(this.Request, this.User.Identity.Name.ToString(), pageUrl).Raise();
                Server.Transfer("~/error.htm");
            }
		}
		if (m.thisPage == null || m.thisPage.IsFourOFour)
		{
			new WebEvents.PageAccessFailureEvent(this.Request, this.User.Identity.Name.ToString(), pageUrl).Raise();
			if (pageUrl.ToLower() != "not-found.aspx")
				SiteUtility.Redirect(404, null);
		}
		else
		{
			if (this.IsPostBack || (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["LoadBalanceUptimePages"]) && Request.Url.OriginalString.ToLower().Contains(ConfigurationManager.AppSettings["LoadBalanceUptimePages"].ToLower())))
			{
				//do nothing
			}
			else
			{
				    new WebEvents.PageAccessSuccessEvent(this.Request, this.User.Identity.Name.ToString(), pageUrl).Raise();
			}
		}

    }


	/// <summary>
	/// implement this method to pass errors to the master page for display
	/// </summary>
	/// <param name="message">the error message to display</param>
	public void OnPageError(string message)
	{
		OnPageError(message, null);
	}

	/// <summary>
	/// implement this method to pass errors to the master page for display
	/// </summary>
	/// <param name="message">the error message to display</param>
	/// <param name="ex">the exception to display/log</param>
	public void OnPageError(string message, Exception ex)
	{
		if (this.Master is BaseMasterPage)
		{
			BaseMasterPage m = (BaseMasterPage)this.Master;
			if (m != null)
			{
				m.OnPageError(message, ex);
			}
		}
		//Don't response.write here--let the master page decide

	}

	///GCS-689: Support display of a list of errors in OnPageError
	public void OnPageError(System.Collections.Generic.IList<string> list, Exception ex)
	{
		if (this.Master is BaseMasterPage)
		{
			BaseMasterPage m = (BaseMasterPage)this.Master;
			if (m != null)
			{
				m.OnPageError(list, ex);
			}
		}
		//Don't response.write here--let the master page decide

    }

	/// <summary>
	/// implement this method to pass success notes to the master page for display
	/// </summary>
	/// <param name="message">the text to display</param>
	public void OnPageSuccess(string message)
	{
		if (this.Master is BaseMasterPage)
		{
			BaseMasterPage m = (BaseMasterPage)this.Master;
			if (m != null)
			{
				m.OnPageSuccess(message);
			}
		}
		//Don't response.write here--let the master page decide

	}

}
