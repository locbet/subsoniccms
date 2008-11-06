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
using SubSonic.Utilities;

public partial class Site_Login : BasePage
{
	protected void Page_Load(object sender, EventArgs e)
    {
		if (!Page.IsPostBack)
		{
			Control ctl = Login1.FindControl("LoginButton");
			if (ctl != null && ctl is IButtonControl)
				Login1.Attributes.Add("onkeypress", string.Format("javascript:return WebForm_FireDefaultButton(event, '{0}')", ctl.ClientID));
		}
    }

    protected void NewRegistration(object sender, EventArgs e) {
        string redir = Utility.GetParameter("ReturnUrl");
        if (redir != string.Empty) {
			SiteUtility.Redirect(redir);
        } else {
			SiteUtility.Redirect("default.aspx");
        }
    }
}
