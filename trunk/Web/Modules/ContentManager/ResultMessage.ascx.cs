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
		divSuccess.Visible = false;
		divFail.Visible = false;
	}

	public void ShowSuccess(string message)
    {
        divSuccess.Visible = true;
        divFail.Visible = false;
        lblSuccess.Text = message + " - " + DateTime.Now;
		flashMessageSuccess.Message = message + " - " + DateTime.Now;
		flashMessageSuccess.Display();
    }

    public void ShowFail(string message)
    {
		ShowFail(message, null);
    }

	public void ShowFail(string message, Exception ex)
	{
		divSuccess.Visible = false;
		divFail.Visible = true;
		lblFail.Text = message + " - " + DateTime.Now + (SiteUtility.UserIsAdmin() && ex != null && !String.IsNullOrEmpty(ex.Message)? "<br /><br /> " + ex.Message : "");
		flashMessageFail.Message = message + " - " + DateTime.Now + (SiteUtility.UserIsAdmin() && ex != null && !String.IsNullOrEmpty(ex.Message) ? "<br /><br /> " + ex.Message : "");
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
