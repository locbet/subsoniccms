using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Text;
/// <summary>
/// Base Page for all Starter Site .aspx pages.
/// </summary>
public class BasePage : System.Web.UI.Page
{
    protected override void OnPreInit(System.EventArgs e)
    {
        base.OnPreInit(e);


        Regex re = new Regex("/[\\w]+/");
        string pageUrl = re.Replace(this.Request.Url.AbsolutePath, "", 1);
        if (pageUrl.ToLower() != "cms/cmsparagraph.aspx")
        {
            System.Web.Profile.ProfileBase MyProfile;
            MyProfile = HttpContext.Current.Profile;
            this.MasterPageFile = "~/site.master"; //MyProfile.GetPropertyValue("MasterFilePreference");
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
			pageUrl = m.Request.Url.AbsolutePath;
			//remove the first section of the absolute path, 
			//which is presented like /sitename/somefile.aspx
			//if your cms is running from multiple hosts, you'll probably want to remove this.
			Regex re = new Regex("/[\\w]+/");
			pageUrl = re.Replace(pageUrl, "", 1);
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
                Regex re = new Regex("/[\\w]+/");
                pageUrl = re.Replace(this.Request.Url.PathAndQuery, "", 1);
                Response.Redirect("~/login.aspx?ReturnUrl=" + pageUrl);
            }
            catch
            {
                new WebEvents.PageAccessFailureEvent(this.Request, this.User.Identity.Name.ToString(), pageUrl).Raise();
                Server.Transfer("~/error.htm");
            }
		}
		if (m.thisPage == null || m.thisPage.IsFourOFour)
		{
			new WebEvents.PageAccessFailureEvent(this.Request, this.User.Identity.Name.ToString(), pageUrl).Raise();
			if (pageUrl.ToLower() != "notfound.aspx")
                Response.Redirect("~/view/notfound.aspx");
		}
		else
		{
            if (!this.IsPostBack)
			    new WebEvents.PageAccessSuccessEvent(this.Request, this.User.Identity.Name.ToString(), pageUrl).Raise();
		}

    }
}
