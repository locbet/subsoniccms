using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace TaskScheduler
{
	/// <summary>
	/// A disconnected version of the <see cref="Task"/> class.
	/// </summary>
	public class ReadOnlyTask
	{
		private Hashtable _props;

		/// <summary>
		/// Creates a new instance of the ReadOnlyTask object.
		/// </summary>
		public ReadOnlyTask() 
		{
			_props = new Hashtable();
			Triggers = new TriggerCollection();
			Flags = new TaskFlags();
		}

		/// <summary>
		/// Creates a new instance of the ReadOnlyTask object.
		/// </summary>
		/// <param name="taskName">name of an existing task</param>
		public ReadOnlyTask(string taskName)
		{
			ScheduledTasks st = new ScheduledTasks();
			Task t = st.OpenTask(taskName);
			Load(t);
		}

		/// <summary>
		/// Creates a new instance of the ReadOnlyTask object.
		/// </summary>
		/// <param name="taskName">name of an existing task</param>
		/// <param name="computerName">name of the computer that contains the task</param>
		public ReadOnlyTask(string taskName, string computerName)
		{
			ScheduledTasks st = new ScheduledTasks(computerName);
			Task t = st.OpenTask(taskName);
			Load(t);
		}
		
		/// <summary>
		/// Creates a new instance of the ReadOnlyTask object by using an existing Task object.
		/// </summary>
		/// <param name="t">The existing Task object to load. The Task will automatically be closed after it is loaded.</param>
		public ReadOnlyTask(Task t)
		{
			Load(t);
		}

		private void Load(Task t)
		{
			_props = new Hashtable();
			this.AccountName = t.AccountName;
			this.ApplicationName = t.ApplicationName;
			this.Comment = t.Comment;
			this.Creator = t.Creator;
			//this.ErrorRetryCount = t.ErrorRetryCount;
			//this.ErrorRetryInterval = t.ErrorRetryInterval;
			this.ExitCode = t.ExitCode;
			this.Flags = t.Flags;
			//this.FlagsEx = t.FlagsEx;
			this.Hidden = t.Hidden;
			this.IdleWaitDeadlineMinutes = t.IdleWaitDeadlineMinutes;
			this.IdleWaitMinutes = t.IdleWaitDeadlineMinutes;
			this.MaxRunTime = t.MaxRunTime;
			this.MaxRunTimeLimited = t.MaxRunTimeLimited;
			this.MostRecentRunTime = t.MostRecentRunTime;
			this.Name = t.Name;
			this.NextRunTime = t.NextRunTime;
			this.Parameters = t.Parameters;
			this.Priority = t.Priority;
			this.Status = t.Status;
			//this.Tag = t.Tag;
			this.WorkingDirectory = t.WorkingDirectory;
			if (t.Triggers != null)
				this.Triggers = new TriggerCollection().Load(t.Triggers);
			else
				this.Triggers = new TriggerCollection();
			t.Close();
		}

		/// <summary>
		/// Creates a task from this read-only task instance
		/// </summary>
		public void Save()
		{
			Save(null);
		}

		/// <summary>
		/// Creates a task from this read-only task instance
		/// </summary>
		/// <param name="computerName">name of the computer where the task should be saved</param>
		public void Save(string computerName)
		{
			ScheduledTasks st;
			if (computerName == null)
				st = new ScheduledTasks();
			else
				st = new ScheduledTasks(computerName);

			Task t;
			try 
			{
				t = st.CreateTask(this.Name);
			} 
			catch (Exception ex)
			{
				if (ex.Message.Contains("already exists"))
				{
					t = st.OpenTask(this.Name);
				}
				else
					throw ex;
			}
			t.ApplicationName = this.ApplicationName;
			t.Parameters = this.Parameters;
			t.Comment = this.Comment;
			t.Creator = this.Creator;
			t.WorkingDirectory = (this.WorkingDirectory == null ? String.Empty : this.WorkingDirectory);
			string acct = this.AccountName;
			if (acct=="") 
			{
				t.SetAccountInformation(acct, (string)null);
			} 
			else if (acct == Environment.UserName) 
			{
				t.SetAccountInformation(acct, (string)null);
			} 
			else 
			{
				t.SetAccountInformation(acct, this.AccountPassword);
			}
			//t.Hidden = true;
			t.IdleWaitDeadlineMinutes = this.IdleWaitDeadlineMinutes;
			t.IdleWaitMinutes = this.IdleWaitMinutes;
			t.MaxRunTime = this.MaxRunTime;
			t.Priority = this.Priority;
			t.Flags = this.Flags;
			foreach (Trigger tr in this.Triggers)
			{
                bool addIt = true;
                foreach (Trigger trExist in t.Triggers)
                {
                    if (trExist.Equals(tr))
                    {
                        addIt = false;
                        break;
                    }
                }
                if (addIt)
                    t.Triggers.Add(tr);
			}
			t.Save();
			t.Close();
		}

		#region Properties

		/// <summary>
		/// Gets the list of triggers associated with the task.
		/// </summary>
		public TriggerCollection Triggers
		{
			get
			{
				return (TriggerCollection)_props["triggers"];
			}
			set
			{
				_props["triggers"] = value;
			}
		}

		/// <summary>
		/// Gets/sets the <see cref="TaskFlags"/> associated with the current task. 
		/// </summary>
		public TaskFlags Flags
		{
			get
			{
				return (TaskFlags)_props["flags"];
			}
			set
			{
				_props["flags"] = value;
			}
		}

		/// <summary>
		/// Gets the name of the task.  The name is also the filename (plus a .job extension)
		/// the Task Scheduler uses to store the task information.  
		/// </summary>
		public string Name {
			get 
			{
				return (String)_props["name"];
			}
			set
			{
				_props["name"] = value;
			}
		}

		/// <summary>
		/// Gets/sets the application filename that task is to run.  Get returns 
		/// an absolute pathname.  A name searched with the PATH environment variable can
		/// be assigned, and the path search is done when the task is saved.
		/// </summary>
		public string ApplicationName 
		{
			get
			{
				return (String)_props["applicationname"];
			}
			set
			{
				_props["applicationname"] = value;
			}
		}

		/// <summary>
		/// Gets the name of the account under which the task process will run.
		/// </summary>
		public string AccountName {
			get
			{
				return (String)_props["accountname"];
			}
			set
			{
				_props["accountname"] = value;
			}
		}

		/// <summary>
		/// Gets the password of the account used to execute the task
		/// </summary>
		public System.Security.SecureString AccountPassword
		{
			get
			{
				return (System.Security.SecureString)_props["accountpassword"];
			}
			set
			{
				_props["accountpassword"] = value;
			}
		}

		/// <summary>
		/// Gets/sets the comment associated with the task.  The comment appears in the 
		/// Scheduled Tasks user interface.
		/// </summary>
		public string Comment {
			get
			{
				return (String)_props["comment"];
			}
			set
			{
				_props["comment"] = value;
			}
		}

		/// <summary>
		/// Gets/sets the creator of the task.  If no value is supplied, the system
		/// fills in the account name of the caller when the task is saved.
		/// </summary>
		public string Creator {
			get
			{
				return (String)_props["creator"];
			}
			set
			{
				_props["creator"] = value;
			}
		}

		/// <summary>
		/// Gets/sets the number of times to retry task execution after failure. (Not implemented.)
		/// </summary>
		private short ErrorRetryCount {
			get
			{
				return (short)_props["errorretrycount"];
			}
			set
			{
				_props["errorretrycount"] = value;
			}
		}

		/// <summary>
		/// Gets/sets the time interval, in minutes, to delay between error retries. (Not implemented.)
		/// </summary>
		private short ErrorRetryInterval {
			get
			{
				return (short)_props["errorretryinterval"];
			}
			set
			{
				_props["errorretryinterval"] = value;
			}
		}

		/// <summary>
		/// Gets the Win32 exit code from the last execution of the task.  If the task failed
		/// to start on its last run, the reason is returned as an exception.  Not updated while
		/// in an open task;  the property does not change unless the task is closed and re-opened.
		/// <exception>Various exceptions for a task that couldn't be run.</exception>
		/// </summary>
		public int ExitCode {
			get
			{
				return (int)_props["exitcode"];
			}
			set
			{
				_props["exitcode"] = value;
			}
		}

		/// <summary>
		/// Gets/sets how long the system must remain idle, even after the trigger
		/// would normally fire, before the task will run. 
		/// </summary>
		public short IdleWaitMinutes {
			get
			{
				return (short)_props["idlewaitminutes"];
			}
			set
			{
				_props["idlewaitminutes"] = value;
			}
		}

		/// <summary>
		/// Gets/sets the maximum number of minutes that Task Scheduler will wait for a 
		/// required idle period to occur. 
		/// </summary>
		public short IdleWaitDeadlineMinutes {
			get
			{
				return (short)_props["idlewaitdeadlineminutes"];
			}
			set
			{
				_props["idlewaitdeadlineminutes"] = value;
			}
		}

		/// <summary>
		/// <p>Gets/sets the maximum length of time the task is permitted to run.
		/// Setting MaxRunTime also affects the value of <see cref="Task.MaxRunTimeLimited"/>.
		/// </p>
		/// <p>The longest MaxRunTime implemented is 0xFFFFFFFE milliseconds, or 
		/// about 50 days.  If you set a TimeSpan longer than that, the
		/// MaxRunTime will be unlimited.</p>
		/// </summary>
		/// <Remarks>
		/// </Remarks>
		public TimeSpan MaxRunTime {
			get
			{
				return (TimeSpan)_props["maxruntime"];
			}
			set
			{
				_props["maxruntime"] = value;
			}
		}

		/// <summary>
		/// <p>If the maximum run time is limited, the task will be terminated after 
		/// <see cref="Task.MaxRunTime"/> expires.  Setting the value to FALSE, i.e. unlimited,
		/// invalidates MaxRunTime.</p> 
		/// <p>The Task Scheduler service will try to send a WM_CLOSE message when it needs to terminate
		/// a task.  If the message can't be sent, or the task does not respond with three minutes,
		/// the task will be terminated using TerminateProcess.</p> 
		/// </summary>
		public bool MaxRunTimeLimited {
			get
			{
				return (bool)_props["maxruntimelimited"];
			}
			set
			{
				_props["maxruntimelimited"] = value;
			}
		}

		/// <summary>
		/// Gets the most recent time the task began running.  <see cref="DateTime.MinValue"/> 
		/// returned if the task has not run.
		/// </summary>
		public DateTime MostRecentRunTime {
			get
			{
				return (DateTime)_props["MostRecentRunTime"];
			}
			set
			{
				_props["MostRecentRunTime"] = value;
			}
		}

		/// <summary>
		/// Gets the next time the task will run. Returns <see cref="DateTime.MinValue"/> 
		/// if the task is not scheduled to run.
		/// </summary>
		public DateTime NextRunTime {
			get
			{
				return (DateTime)_props["NextRunTime"];
			}
			set
			{
				_props["NextRunTime"] = value;
			}
		}

		/// <summary>
		/// Gets/sets the command-line parameters for the task.
		/// </summary>
		public string Parameters {
			get
			{
				return (String)_props["parameters"];
			}
			set
			{
				_props["parameters"] = value;
			}
		}

		/// <summary>
		/// Gets/sets the priority for the task process.  
		/// Note:  ProcessPriorityClass defines two levels (AboveNormal and BelowNormal) that are
		/// not documented in the task scheduler interface and can't be use on Win 98 platforms.
		/// </summary>
		public System.Diagnostics.ProcessPriorityClass Priority {
			get
			{
				return (System.Diagnostics.ProcessPriorityClass)_props["priority"];
			}
			set
			{
				_props["priority"] = value;
			}
		}

		/// <summary>
		/// Gets the status of the task.  Returns <see cref="TaskStatus"/>.
		/// Not updated while a task is open.
		/// </summary>
		public TaskStatus Status {
			get
			{
				return (TaskStatus)(int)_props["status"];
			}
			set
			{
				_props["status"] = value;
			}
		}

		/// <summary>
		/// Extended Flags associated with a task. These are associated with the ITask com interface
		/// and none are currently defined.
		/// </summary>
		private int FlagsEx {
			get
			{
				return (int)_props["flagsEx"];
			}
			set
			{
				_props["flagsEx"] = value;
			}
		}

		/// <summary>
		/// Gets/sets the initial working directory for the task.
		/// </summary>
		public string WorkingDirectory {
			get
			{
				return (String)_props["workingdirectory"];
			}
			set
			{
				_props["workingdirectory"] = value;
			}
		}

		/// <summary>
		/// Hidden tasks are stored in files with
		/// the hidden file attribute so they don't appear in the Explorer user interface.
		/// Because there is a special interface for Scheduled Tasks, they don't appear
		/// even if Explorer is set to show hidden files.
		/// Functionally equivalent to TaskFlags.Hidden.
		/// </summary>
		public bool Hidden {
			get
			{
				return (bool)_props["hidden"];
			}
			set
			{
				_props["hidden"] = value;
			}
		}
		/// <summary>
		/// Gets/sets arbitrary data associated with the task.  The tag can be used for any purpose
		/// by the client, and is not used by the Task Scheduler.  Known as WorkItemData in the
		/// IWorkItem com interface.
		/// </summary>
		public object Tag {
			get
			{
				return _props["tag"];
			}
			set
			{
				_props["tag"] = value;
			}
		}
		#endregion


	}
}
