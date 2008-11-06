/// <summary>
/// Base Page for all Start Site user controls
/// </summary>
using System;
public class BaseUserControl : System.Web.UI.UserControl
{
	/// <summary>
	/// implement this method to pass errors to the master page for display
	/// </summary>
	/// <param name="message">the error message to display</param>
	public void OnPageError(string message)
	{
		OnPageError(message, null);
	}

	/// <summary>
	/// implement this method to pass errors to the master page for display
	/// </summary>
	/// <param name="message">the error message to display</param>
	/// <param name="ex">the exception to display/log</param>
	public void OnPageError(string message, Exception ex)
	{
		if (this.Page.Master is BaseMasterPage)
		{
			BaseMasterPage m = (BaseMasterPage)this.Page.Master;
			if (m != null)
			{
				m.OnPageError(message, ex);
			}
		}

	}

	/// <summary>
	/// implement this method to pass success notes to the master page for display
	/// </summary>
	/// <param name="message">the text to display</param>
	public void OnPageSuccess(string message)
	{
		if (this.Page.Master is BaseMasterPage)
		{
			BaseMasterPage m = (BaseMasterPage)this.Page.Master;
			if (m != null)
			{
				m.OnPageSuccess(message);
			}
		}

	}


}
