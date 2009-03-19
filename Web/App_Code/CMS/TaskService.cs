using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using TaskScheduler;

namespace CMS
{
	public class TaskService
	{
		public static bool DeleteTask(string taskName)
		{
			ReadOnlyTask rot = new ReadOnlyTask(taskName);
			return rot.Delete();
		}

		public static void RunTask(string taskName)
		{
			ReadOnlyTask rot = new ReadOnlyTask(taskName);
			rot.Run();
		}

		public static void StopTask(string taskName)
		{
			ReadOnlyTask rot = new ReadOnlyTask(taskName);
			rot.Terminate();
		}
	}
}