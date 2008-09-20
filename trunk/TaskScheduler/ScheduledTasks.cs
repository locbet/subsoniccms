using System;
using System.IO;
using System.Runtime.InteropServices;
using TaskSchedulerInterop;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace TaskScheduler {
	/// <summary>
	/// ScheduledTasks represents the a computer's Scheduled Tasks folder.  Using a ScheduledTasks
	/// object, you can discover the names of the tasks in the folder and you can open a task
	/// to work with it.  You can also create or delete tasks.
	/// </summary>
	/// <remarks>
	/// A ScheduledTasks object holds a COM interface that can be released by calling <see cref="Dispose()"/>.
	/// </remarks>
	public class ScheduledTasks : IDisposable {
		/// <summary>
		/// Underlying COM interface.
		/// </summary>
		private ITaskScheduler its = null;

		// --- Contructors ---

		/// <summary>
		/// Constructor to use Scheduled tasks of a remote computer identified by a UNC
		/// name.  The calling process must have administrative privileges on the remote machine. 
		/// May throw exception if the computer's task scheduler cannot be reached, and may
		/// give strange results if the argument is not in UNC format.
		/// </summary>
		/// <param name="computer">The remote computer's UNC name, e.g. "\\DALLAS".</param>
		/// <exception cref="ArgumentException">The Task Scheduler could not be accessed.</exception>
		public ScheduledTasks(string computer) : this() {
			its.SetTargetComputer(computer);
		}

		/// <summary>
		/// Constructor to use Scheduled Tasks of the local computer.
		/// </summary>
		public ScheduledTasks() {
			CTaskScheduler cts = new CTaskScheduler();
			its = (ITaskScheduler)cts;
		}

		// --- Methods ---
 
		private string[] GrowStringArray(string[] s, uint n) {
			string[] sl = new string[s.Length + n];
			for (int i=0; i<s.Length; i++) { sl[i] = s[i];}
			return sl;
		}

		/// <summary>
		/// Return the names of all scheduled tasks.  The names returned include the file extension ".job";
		/// methods requiring a task name can take the name with or without the extension.
		/// </summary>
		/// <returns>The names in a string array.</returns>
		public string[] GetTaskNames() {
			const int TASKS_TO_FETCH = 10;
			string[] taskNames = {};
			int nTaskNames = 0;

			IEnumWorkItems ienum;
			its.Enum(out ienum);

			uint nFetchedTasks;
			IntPtr pNames;

			while ( ienum.Next( TASKS_TO_FETCH, out pNames, out nFetchedTasks ) >= 0 &&
				nFetchedTasks > 0 ) {
				taskNames = GrowStringArray(taskNames, nFetchedTasks);
				while ( nFetchedTasks > 0 ) {
					IntPtr name = Marshal.ReadIntPtr( pNames, (int)--nFetchedTasks * IntPtr.Size );
					taskNames[nTaskNames++] = Marshal.PtrToStringUni(name);
					Marshal.FreeCoTaskMem(name);
				}
				Marshal.FreeCoTaskMem( pNames );
			}
			return taskNames;

		}

		/// <summary>
		/// Creates a new task on the system with the given <paramref name="name" />.
		/// </summary>
		/// <remarks>Task names follow normal filename character restrictions.  The name
		/// will be come the name of the file used to store the task (with .job added).</remarks>
		/// <param name="name">Name for the new task.</param>
		/// <returns>Instance of new task.</returns>
		/// <exception cref="ArgumentException">There is an existing task with the given name.</exception>
		public Task CreateTask(string name) {
			Task tester = OpenTask(name);
			if (tester != null) {
				tester.Close();
				throw new ArgumentException("The task \"" + name + "\" already exists.");
			}
			try {
				object o;
				its.NewWorkItem(name, ref CTaskGuid, ref ITaskGuid, out o);
				ITask iTask = (ITask)o;
				return new Task(iTask, name);
			}
			catch {
				return null;
			}
		}

		/// <summary>
		/// Deletes the task of the given <paramref name="name" />.
		/// </summary>
		/// <remarks>If you delete a task that is open, a subsequent save will throw an
		/// exception.  You can save to a filename, however, to create a new task.</remarks>
		/// <param name="name">Name of task to delete.</param>
		/// <returns>True if the task was deleted, false if the task wasn't found.</returns>
		public bool DeleteTask(string name) {
			try {
				its.Delete(name);
				return true;
			}
			catch {
				return false;
			}
		}

		/// <summary>
		/// Opens the task with the given <paramref name="name" />.  An open task holds COM interfaces
		/// which are released by the Task's Close() method.
		/// </summary>
		/// <remarks>If the task does not exist, null is returned.</remarks>
		/// <param name="name">Name of task to open.</param>
		/// <returns>An instance of a Task, or null if the task name couldn't be found.</returns>
		public Task OpenTask(string name) {
			try {
				object o;
				its.Activate(name, ref ITaskGuid, out o);
				ITask iTask = (ITask)o;
				return new Task(iTask, name);
			}
			catch {
				return null;
			}
		}


		#region Implementation of IDisposable
		/// <summary>
		/// The internal COM interface is released.  Further access to the
		/// object will throw null reference exceptions.
		/// </summary>
		public void Dispose() {
			Marshal.ReleaseComObject(its);
			its = null;
		}
			#endregion

		// Two Guids for calls to ITaskScheduler methods Activate(), NewWorkItem(), and IsOfType()
		internal static Guid ITaskGuid;
		internal static Guid CTaskGuid;
		static ScheduledTasks() {
			ITaskGuid = Marshal.GenerateGuidForType(typeof(ITask));
			CTaskGuid = Marshal.GenerateGuidForType(typeof(CTask));
		}
	}

	/// <summary>
	/// A strong typed collection of ScheduledTasks
	/// </summary>
	public class ScheduledTasksCollection : CollectionBase
	{
		/// <summary>
		/// creates a new instance of the ScheduledTasksCollection
		/// </summary>
		public ScheduledTasksCollection() { }

		/// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// </summary>
		/// <param name="propertyName">Name of the property to compare.</param>
		/// <param name="items">The list of items to use for matching</param>
		/// <returns>ScheduledTasksCollection</returns>
		public ScheduledTasksCollection Filter(string propertyName, IList items)
		{
			return Filter(propertyName, items, propertyName, false);
		}

		/// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// </summary>
		/// <param name="propertyName">Name of the property to compare.</param>
		/// <param name="items">The list of items to use for matching</param>
		/// <param name="ignoreCase">Ignore case when matching string types</param>
		/// <returns>ScheduledTasksCollection</returns>
		public ScheduledTasksCollection Filter(string propertyName, IList items, bool ignoreCase)
		{
			return Filter(propertyName, items, propertyName, ignoreCase);
		}

		/// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// </summary>
		/// <param name="propertyName">Name of the property to compare.</param>
		/// <param name="items">The list of items to use for matching</param>
		/// <param name="itemsPropertyName">the name of the property to compare from the items list.</param>
		/// <returns>ScheduledTasksCollection</returns>
		public ScheduledTasksCollection Filter(string propertyName, IList items, string itemsPropertyName, bool ignoreCase)
		{
			for (int i = this.Count - 1; i > -1; i--)
			{
				ReadOnlyTask o = this[i];
				foreach (object item in items)
				{
					bool remove = false;
					System.Reflection.PropertyInfo pi = o.GetType().GetProperty(propertyName);
					System.Reflection.PropertyInfo pitem = item.GetType().GetProperty(itemsPropertyName);
					if (pi.CanRead && pitem.CanRead)
					{
						object val = pi.GetValue(o, null);
						object valItem = pitem.GetValue(item, null);
						if (!val.Equals(valItem))
						{
							//case insensitve match for string types
							if (val.GetType() != typeof(String) || (val.GetType() == typeof(String) && String.Compare(val.ToString(), valItem.ToString(), ignoreCase) != 0))
								remove = true;
						}
					}
					if (remove)
					{
						this.Remove(o);
						break;
					}
				}
			}
			return this;
		}


		/// <summary>
		/// Loads a collection of ReadOnlyTasks
		/// </summary>
		/// <returns></returns>
		public ScheduledTasksCollection Load()
		{
			ScheduledTasks st = new ScheduledTasks();
			string[] taskNames = st.GetTaskNames();
			// Open each task, dump info to console
			foreach (string name in taskNames)
			{
				try
				{
					Task t = st.OpenTask(name);
					//Console.WriteLine("--> " + name + " ");
					List.Add(new ReadOnlyTask(t));
					//t.Close();
				}
				catch (Exception ex)
				{
					if (!ex.Message.Contains("unknown user name or bad password"))
						throw ex;
				}
			}
			return this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(ReadOnlyTask item)
		{
			return List.Add(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, ReadOnlyTask item)
		{
			List.Insert(index, item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		public void Remove(ReadOnlyTask item)
		{
			List.Remove(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(ReadOnlyTask item)
		{
			return List.Contains(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(ReadOnlyTask item)
		{
			return List.IndexOf(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(ReadOnlyTask[] array, int index)
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public ReadOnlyTask this[int index]
		{
			get { return (ReadOnlyTask)List[index]; }
			set { List[index] = value; }
		}

	}

}