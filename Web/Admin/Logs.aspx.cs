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

public partial class admin_logs : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
    {
		if (!Page.IsPostBack)
		{
			loadAll();
		}
	}

	private void loadAll()
	{
		dgWebEvents.DataSource = new ASPNetData.WebEventEventCollection().Where(ASPNetData.WebEventEvent.Columns.EventTime, SubSonic.Comparison.GreaterOrEquals, SubSonic.Sugar.Dates.DaysAgo(Int32.Parse(ConfigurationManager.AppSettings["DefaultLogDaysToView"].ToString()))).OrderByDesc(ASPNetData.WebEventEvent.Columns.EventTime).Load();
		dgWebEvents.DataBind();
	}

}
