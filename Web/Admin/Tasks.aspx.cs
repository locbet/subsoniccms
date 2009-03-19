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
		ReadOnlyTaskCollection stSource = new ReadOnlyTaskCollection().Load();
		ReadOnlyTaskCollection st = stSource.Filter("ApplicationName", coll.GetList(), "FullPath", true);
		dgTasks.DataSource = st;
		dgTasks.DataBind();
        Master.Title = Master.Title + ": " + Environment.MachineName;
	}

	protected void LinkButton_Command(object sender, CommandEventArgs e)
	{
		if (String.IsNullOrEmpty(e.CommandArgument.ToString()))
		{
			OnPageError("No task name was specified. Nothing to do.");
			return;
		}

		try
		{
			if (e.CommandName == "DeleteTheTask")
			{
				if (CMS.TaskService.DeleteTask(e.CommandArgument.ToString()))
				{
					OnPageSuccess("Task '" + e.CommandArgument + "' was deleted successfully.");
				}
				else
				{
					OnPageError("Deletion failed or task not found.");
				}
			}
			else if (e.CommandName == "RunTheTask")
			{
				CMS.TaskService.RunTask(e.CommandArgument.ToString());
				OnPageSuccess("Task started successfully");
			}
			else if (e.CommandName == "StopTheTask")
			{
				CMS.TaskService.StopTask(e.CommandArgument.ToString());
				OnPageSuccess("Task stopped successfully");
			}
		}
		catch (Exception ex)
		{
			OnPageError("Unable to complete the requested action. Please refresh the page and try again.", ex);
		}
		//give the system time to try and complete the task (so the UI can actually be updated)
		System.Threading.Thread.Sleep(1000);
		loadAll();
	}

	protected void timerTaskList_Tick(object sender, EventArgs e)
	{
		try
		{
			loadAll();
		}
		catch (Exception ex)
		{
			OnPageError("Unable to load tasks.", ex);
		}
	}

}
