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

public partial class admin_tasks : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
    {
		if (!Page.IsPostBack)
		{
			try
			{
				loadAll();
			}
			catch (Exception ex)
			{
				OnPageError("Unable to load tasks", ex);
			}
		}
	}

	private void loadAll()
	{
		CMS.TaskApplicationCollection coll = new CMS.TaskApplicationCollection();
		SubSonic.Query qry = new SubSonic.Query(CMS.TaskApplication.Schema);
		qry.AddWhere(CMS.TaskApplication.Columns.HostName, Environment.MachineName);
		coll.LoadAndCloseReader(qry.ExecuteReader());

		//filter the list of tasks against the approved exe list
		ScheduledTasksCollection stSource = new ScheduledTasksCollection().Load();
		ScheduledTasksCollection st = stSource.Filter("ApplicationName", coll.GetList(), "FullPath", true);
		dgTasks.DataSource = st;
		dgTasks.DataBind();
        Master.Title = Master.Title + ": " + Environment.MachineName;
	}
}
