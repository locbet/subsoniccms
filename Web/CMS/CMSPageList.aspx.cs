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

public partial class Admin_CMSPageList : BasePage
{
    //protected CMS.Page thisPage = new CMS.Page();

	protected void Page_Load(object sender, EventArgs e)
    {
        //SiteUtility.BuildBasePage((BasePage)this, ref thisPage, "cms/cmspagelist.aspx");
        //Master.thisPage = thisPage;
		grdAllPages.DataSource = new SubSonic.Query("CMS_page").WHERE("Deleted", 0).ExecuteReader();
		grdAllPages.DataBind();
    }

	public void LinkButtonClick(object sender, CommandEventArgs e)
	{
		int pageID = 0;
		switch (e.CommandName.ToString())
		{
			case "EditPage":
				pageID = Int32.Parse(e.CommandArgument.ToString());
				if (pageID != 0)
				{
					CMS.Page p = CMS.ContentService.GetPage(pageID);
					if (p.IsLoaded && p.PageUrl != "")
					{
						Response.Redirect("~/view/editpage.aspx?pRef=" + p.PageUrl);
					}
				}
				break;
			case "ViewPage":
				pageID = Int32.Parse(e.CommandArgument.ToString());
				if (pageID != 0)
				{
					CMS.Page p = CMS.ContentService.GetPage(pageID);
					if (p.IsLoaded && p.PageUrl != "")
					{
						Response.Redirect((p.PageTypeID == 1 ? "~/" : "~/view/") + p.PageUrl);
					}
				}
				break;
		}
	}
}
