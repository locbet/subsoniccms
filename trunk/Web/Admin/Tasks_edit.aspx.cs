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
using System.Web.Profile;
using TaskScheduler;

public partial class admin_task_edit : BasePage
{
	bool isEditMode = true;
	string taskName = SiteUtility.GetParameter("taskname").Trim();

    protected void Page_Load(object sender, EventArgs e)
    {

		// Is in add mode?
        //
        if (string.IsNullOrEmpty(taskName))
        {
            Page.Title = "Add Task";
			ActionTitle.Text = "Add Task";
            isEditMode = false;
        }
        else
        {
            // We are in edit mode
            //
        }

        if (!IsPostBack)
        {
			try
			{
				LoadDaysOfWeek();
				ToggleTriggers(isEditMode);
				ToggleEdit(isEditMode);
				// Is it in edit mode?
				if (isEditMode)
				{
					LoadAll();
				}
				else
				{
					LoadFlags(null);
					LoadApps(null);
				}
			}
			catch (Exception ex)
			{
				OnPageError("Unable to completely load this page.", ex);
			}
        }
    }

	private void LoadAll()
	{
		try
		{
			ReadOnlyTask rt = new ReadOnlyTask(taskName);
			txtTaskName.Text = rt.Name;
			txtParameters.Text = rt.Parameters;
			txtComment.Text = rt.Comment;
			//txtWorkingDir.Text = rt.WorkingDirectory;
			//txtPriority.Text = rt.Priority.ToString();
			txtStatus.Text = rt.Status.ToString();
			LoadApps(rt);
			LoadFlags(rt);
			LoadTriggers(rt);
		}
		catch (Exception ex)
		{
			OnPageError("Unable to load this task completely.", ex);
		}
	}

    private void LoadApps(ReadOnlyTask rot)
    {
		CMS.TaskApplicationCollection coll = new CMS.TaskApplicationCollection();
		SubSonic.Query qry = new SubSonic.Query(CMS.TaskApplication.Schema);
		qry.AddWhere(CMS.TaskApplication.Columns.HostName, Environment.MachineName);
		coll.LoadAndCloseReader(qry.ExecuteReader());
		if (coll.Count > 0)
		{
			ddlApplicationName.DataSource = coll;
			ddlApplicationName.DataBind();
			if (rot != null && !String.IsNullOrEmpty(rot.ApplicationName))
			{
				int i = coll.Find(CMS.TaskApplication.FullPathColumn.PropertyName, rot.ApplicationName.ToLower(), true);
				if (i >= 0)
				{
					CMS.TaskApplication ta = coll[i];
					if (ta != null && !String.IsNullOrEmpty(ta.Name))
						ddlApplicationName.SelectedValue = ta.Name;
				}
			}
		}
		else
		{
			throw new ArgumentOutOfRangeException("HostName", Environment.MachineName, "This machine has no applications defined for use by the task scheduler. Please define at least one application for this machine.");
		}
		
    }

	private void LoadDaysOfWeek()
	{
		chklDaysOfWeek.Items.Clear();
		foreach (string s in Enum.GetNames(typeof(DaysOfTheWeek)))
		{
			ListItem li = new ListItem(s, s);
			chklDaysOfWeek.Items.Add(li);
		}
	}

    private void LoadFlags(ReadOnlyTask t)
	{
        chklFlagList.Items.Clear();
        foreach (string s in Enum.GetNames(typeof(TaskFlags)))
		{
			ListItem li = new ListItem(s, s);
			if (t != null)
				li.Selected = t.Flags.ToString().Contains(s);
			chklFlagList.Items.Add(li);
		}
	}

	private void LoadTriggers(ReadOnlyTask t)
	{
		if (t.Triggers != null)
		{
			dgTriggerList.DataSource = t.Triggers;
			dgTriggerList.DataBind();
		}
	}

	public void LinkButtonClick(object sender, EventArgs e)
	{
		LinkButton btn = (LinkButton)sender;
		int index = 0;
		switch (btn.CommandName)
		{
			case "deletethetrigger":
				if (Int32.TryParse(btn.CommandArgument, out index))
				{
                    //first get the readonlytask version
                    //we'll use it to loop through the current triggers
                    ReadOnlyTask rt = new ReadOnlyTask(taskName);
                    
                    ScheduledTaskController st = new ScheduledTaskController();
                    Task t = st.OpenTask(taskName);
                    foreach (Trigger tr in t.Triggers)
                    {
                        if (tr.Equals(rt.Triggers[index]))
                        {
                            t.Triggers.Remove(tr);
                            break;
                        }
                    }
                    t.Save();
                    t.Close();
                    LoadTriggers(new ReadOnlyTask(taskName));
                    OnPageSuccess("Trigger successfully deleted.");
                }
				break;
		}
	}

	public void DeleteClick(object sender, EventArgs e)
	{
		try
		{
			DeleteTask(sender, e);
			SiteUtility.Redirect("~/admin/tasks.aspx");
		}
		catch (Exception ex)
		{
			OnPageError("Unable to delete task", ex);
		}
	}

	private void DeleteTask(object sender, EventArgs e)
	{
		if (!isEditMode)
		{
			throw new InvalidOperationException("Can't delete a task that hasn't been saved yet.");
		}
		CMS.TaskService.DeleteTask(taskName);
	}

	public void SaveClick(object sender, EventArgs e)
    {
		try
		{
			SaveTask(sender, e);
			SiteUtility.Redirect("~/admin/tasks_edit.aspx?taskname=" + Server.UrlPathEncode(taskName));
		}
		catch (Exception ex)
		{
			OnPageError("Unable to save task", ex);
		}
    }

	public void SaveAndRunClick(object sender, EventArgs e)
	{
		try
		{
			SaveTask(sender, e, true);
			SiteUtility.Redirect("~/admin/tasks_edit.aspx?taskname=" + Server.UrlPathEncode(taskName));
		}
		catch (Exception ex)
		{
			OnPageError("Unable to save task", ex);
		}
	}

	private void SaveTask(object sender, EventArgs e)
	{
		SaveTask(sender, e, false);
	}

	private void SaveTask(object sender, EventArgs e, bool executeAfterSave)
	{
		if (!Page.IsValid)
		{
			return;
		}
		ReadOnlyTask rot;
		if (isEditMode)
			rot = new ReadOnlyTask(txtTaskName.Text);
		else
			rot = new ReadOnlyTask();
		rot.Name = txtTaskName.Text;
		taskName = rot.Name;
		rot.Parameters = txtParameters.Text;
		rot.Comment = txtComment.Text;
        rot.Creator = ConfigurationManager.ConnectionStrings["ScheduledTaskAccount"].ToString();
		//rot.WorkingDirectory = txtWorkingDir.Text;
        rot.AccountName = ConfigurationManager.ConnectionStrings["ScheduledTaskAccount"].ToString();
		System.Security.SecureString pwd = new System.Security.SecureString();
        foreach (char c in ConfigurationManager.ConnectionStrings["ScheduledTaskAccountPassword"].ToString().ToCharArray())
        {
            pwd.AppendChar(c);
        }
        rot.AccountPassword = pwd;
		rot.IdleWaitDeadlineMinutes = 20;
		rot.IdleWaitMinutes = 10;
		rot.MaxRunTime = new TimeSpan(1, 0, 0);
		rot.Priority = System.Diagnostics.ProcessPriorityClass.Normal;
        SaveTrigger(rot);
        int taskAppID = 0;
        if (Int32.TryParse(ddlApplicationName.SelectedValue, out taskAppID))
        {
            CMS.TaskApplication ta = new CMS.TaskApplication(taskAppID);
            rot.ApplicationName = ta.FullPath;
            rot.WorkingDirectory = (ta.WorkingDirectory == null ? string.Empty : ta.WorkingDirectory);
        }
        else
        {
            throw new ArgumentOutOfRangeException("Invalid application.");
        }
		rot.Flags = new TaskFlags();
		foreach (ListItem li in chklFlagList.Items)
		{
			if (li.Selected)
				rot.Flags = rot.Flags | (TaskFlags)Enum.Parse(typeof(TaskFlags), li.Text);
		}

		rot.Save();

		if (executeAfterSave)
		{
			rot.Run();
		}
	}

    public void SaveTrigger(ReadOnlyTask rot)
    {
        if (cbAddNewTrigger.Checked)
        {
            DateTime startDate = (calStartDate.SelectedDate ?? DateTime.Now);
            DateTime endDate = (calEndDate.SelectedDate ?? DateTime.Now);
			int duration = 0;
            int repeatHours = 0;
            int repeatMinutes = 0;

            switch (ddlTriggerType.SelectedValue)
            {
                case "Daily":
                    DailyTrigger dt = new DailyTrigger((short)startDate.Hour, (short)startDate.Minute);
					duration = (int)SubSonic.Sugar.Dates.DiffMinutes(endDate, startDate);
					dt.BeginDate = startDate;
                    dt.DurationMinutes = (duration < 0 ? 0 : (duration > 1440 ? 1440 : duration));
                    if (Int32.TryParse(txtRepeatHour.Text, out repeatHours) && Int32.TryParse(txtRepeatMinute.Text, out repeatMinutes))
                        dt.IntervalMinutes = (repeatHours * 60) + repeatMinutes;
                    else
                        dt.IntervalMinutes = 0;
                    if (cbRepeatUntilIsEndDate.Checked)
                        dt.EndDate = endDate;
                    rot.Triggers.Add(dt);
					//rot.NextRunTime = (dt.BeginDate > DateTime.Now ? dt.BeginDate : DateTime.Now);
                    break;
				case "Weekly":
					DaysOfTheWeek dow = new DaysOfTheWeek();
					foreach (ListItem li in chklDaysOfWeek.Items)
					{
						if (li.Selected)
							dow = dow | (DaysOfTheWeek)Enum.Parse(typeof(DaysOfTheWeek), li.Text);
					}
					duration = ((int)SubSonic.Sugar.Dates.DiffDays(endDate, startDate) % 7); 
					WeeklyTrigger wt = new WeeklyTrigger((short)startDate.Hour, (short)startDate.Minute, dow, (short)(duration));
					wt.BeginDate = startDate;
					duration = ((int)SubSonic.Sugar.Dates.DiffHours(endDate, startDate) % 24);
					wt.DurationMinutes = (duration < 0 ? 0 : duration);
					if (Int32.TryParse(txtRepeatHour.Text, out repeatHours) && Int32.TryParse(txtRepeatMinute.Text, out repeatMinutes))
						wt.IntervalMinutes = (repeatHours * 60) + repeatMinutes;
					else
						wt.IntervalMinutes = 0;
					if (cbRepeatUntilIsEndDate.Checked)
						wt.EndDate = endDate;
					rot.Triggers.Add(wt);
					//rot.NextRunTime = (wt.BeginDate > DateTime.Now ? wt.BeginDate : DateTime.Now);
					break;
			}
            LoadTriggers(new ReadOnlyTask(taskName));
            LoadAll();
        }
    }

	private void ToggleTriggers(bool showIt)
	{
		divNewTrigger.Visible = showIt;
		divNewTriggerMessage.Visible = !showIt;
		trTriggerList.Visible = showIt;
	}

	private void ToggleEdit(bool enableIt)
	{
		DeleteButton.Enabled = enableIt;
		txtTaskName.Enabled = !enableIt;
	}
}
