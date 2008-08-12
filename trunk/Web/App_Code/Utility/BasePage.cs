using System.Web;
/// <summary>
/// Base Page for all Starter Site .aspx pages.
/// </summary>
public class BasePage : System.Web.UI.Page
{
    protected override void OnPreInit(System.EventArgs e)
    {
        base.OnPreInit(e);
        //System.Web.Profile.ProfileBase MyProfile;

        //MyProfile = HttpContext.Current.Profile;

        this.MasterPageFile = "~/site.master"; //MyProfile.GetPropertyValue("MasterFilePreference");

        this.Theme = "Default"; //MyProfile.GetPropertyValue("ThemePreference");

    }

    protected override void OnLoad(System.EventArgs e)
    {
        base.OnLoad(e);

        BuildBasePage(null);
    }

    public void BuildBasePage()
    {
        BuildBasePage(null);
    }

    public void BuildBasePage(string pageUrl)
    {
        BaseMasterPage m = (BaseMasterPage)this.Master;
        if (pageUrl == null || string.IsNullOrEmpty(pageUrl))
            pageUrl = m.Request.Url.AbsolutePath.Replace("/SubSonicCMS/", "");
        
        if (m.thisPage == null)
            SiteUtility.BuildBasePage(this, m, pageUrl);
        if (m.thisPage == null)
            Response.Redirect("~/CMSFiles/404.htm");
        else if (m.thisPage.IsFourOFour && m.thisPage.PageTypeID == 1)
            Response.Redirect("~/view/notfound.aspx");
    }
}
