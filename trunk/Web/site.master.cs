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

public partial class site : BaseMasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {
            Head1.Controls.Add(new LiteralControl("<script type='text/javascript' src='" + Page.ResolveUrl("~/js/modal/common.js") + "'></script>"));
            Head1.Controls.Add(new LiteralControl("<script type='text/javascript' src='" + Page.ResolveUrl("~/js/modal/subModal.js") + "'></script>"));
            Head1.Controls.Add(new LiteralControl("<link id=\"Link1\" rel=\"stylesheet\" type=\"text/css\" href=\"" + Page.ResolveUrl("~/js/modal/subModal.css") + " runat=\"server\" />"));
            Head1.Controls.Add(new LiteralControl("<link rel=\"stylesheet\" href=\"" + Page.ResolveUrl("~/App_Themes/" + Page.Theme + "/MenuExample.css") + "\" type=\"text/css\" />"));
            Head1.Controls.Add(new LiteralControl("<link rel=\"shortcut icon\" href=\"" + Page.ResolveUrl("~/App_Themes/" + Page.Theme + "/images/favicon.ico") + "\" />"));

            
            siteMenu.PreRender += new EventHandler(siteMenu_PreRender);

			siteMenu.Visible = true;
			SiteMapPath1.Visible = true;
        }
    }

    void siteMenu_PreRender(object sender, EventArgs e) {
        
		toggleEditPanel(ShowEditLinks && (SiteUtility.UserCanEdit() || SiteUtility.UserCanCreate()));
	}



    protected void lnkLogout_Click1(object sender, EventArgs e) {

		//new WebEvents.LogoutSuccessEvent(this, Page.User.Identity.Name.ToString()).Raise();
		FormsAuthentication.SignOut();
         Response.Redirect("~/default.aspx", false);
   }

	public void toggleEditPanel(bool showIt)
	{
		pnlEditBar.Visible = showIt;
		if (pnlEditBar.Visible)
		{
			tdPageEdit.Visible = SiteUtility.UserCanEdit();
			tdPageNew.Visible = SiteUtility.UserCanCreate();
			tdPageList.Visible = SiteUtility.UserCanCreate();
		}

	}

   #region events
	protected void lnkNewPage_Click(object sender, EventArgs e)
   {
	   //redirect to "newpage.aspx"
	   //this is a trigger that tells the page to setup for an add
	   Response.Redirect("~/view/newpage.aspx");
   }

	protected void lnkEdit_Click(object sender, EventArgs e)
   {
	   //redirect to "editpage.aspx"
	   //this is a trigger that tells the page to setup for an edit
		if (!String.IsNullOrEmpty(this.thisPage.PageUrl))
		{
			Response.Redirect("~/view/editpage.aspx?pRef=" + this.thisPage.PageUrl);
		}

   }

	protected void lnkPageList_Click(object sender, EventArgs e)
	{
		Response.Redirect("~/cms/cmspagelist.aspx");
	}

	protected void btnSiteSearch_Click(object sender, EventArgs e)
	{

	}

   #endregion
}