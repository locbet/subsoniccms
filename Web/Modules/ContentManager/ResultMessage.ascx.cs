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

/// <summary>
/// This very simple control displays a nicely-formatted result, anywhere on a page
/// </summary>
public partial class Modules_ContentManager_ResultMessage : BaseUserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
	public void HideAll()
	{
	}

	public void ShowSuccess(string message)
    {
        flashMessageSuccess.Message = "<div class=\"divSuccess\"><div class=\"validationSummarySuccess\">" + message + " - " + DateTime.Now + "</div></div>";
        flashMessageSuccess.Interval = 4000;
		flashMessageSuccess.Display();
    }

    public void ShowFail(string message)
    {
		ShowFail(message, null);
    }

	public void ShowFail(string message, Exception ex)
	{
        flashMessageFail.Message = "<div class=\"divFail\"><div class=\"validationSummaryError\">" + message + " - " + DateTime.Now + (SiteUtility.UserIsAdmin() && ex != null && !String.IsNullOrEmpty(ex.Message) ? "<br /><br /> " + ex.Message : "") + "</div></div>";
        flashMessageFail.Interval = 8000;
        flashMessageFail.Display();

		//if (ex != null)
		//{
		//	new WebEvents.InputValidationEvent(this.Page, message + "\r\n" + ex.Message).Raise();
		//}
	}

    protected string GetPath()
    {
        string sPath = Request.ApplicationPath;
        if (sPath == "/")
        {
            sPath = "";
        }
        return sPath;
    }

}
