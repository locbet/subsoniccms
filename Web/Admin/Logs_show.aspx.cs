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
using System.IO;
using TaskScheduler;

public partial class admin_logs_show : BasePage
{
	private string _id = "";

	protected void Page_Load(object sender, EventArgs e)
    {
		if (!Page.IsPostBack)
		{
			_id = SiteUtility.GetParameter("id");
		}

		if (!Page.IsPostBack)
		{
			loadAll();
		}
	}

	private void loadAll()
	{
		if (!string.IsNullOrEmpty(_id))
		{
			ASPNetData.WebEventEventCollection coll = new ASPNetData.WebEventEventCollection();
			coll.Add(new ASPNetData.WebEventEvent(_id));
			foreach (ASPNetData.WebEventEvent wee in coll)
			{
				wee.Details = wee.Details.Replace("\r\n", "<br />");
				wee.Details = wee.Details.Replace("\n", "<br />");
			}
			LogEntryDetails.DataSource = coll;
		}
		else
			LogEntryDetails.DataSource = null;
		LogEntryDetails.DataBind();
	}

}
