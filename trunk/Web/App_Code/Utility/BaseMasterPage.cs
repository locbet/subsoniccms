/// <summary>
/// Base Master Page for all Start Site user controls
/// </summary>
using System;
using System.Text;
using System.Collections.Generic;

public class BaseMasterPage : System.Web.UI.MasterPage
{
    private string _Title = "";
    private string _Summary = "";
    private string _Body = "";
    private System.DateTime _ModifiedOn = System.DateTime.MinValue;
    private string _ModifiedBy = "";
    private CMS.Page _thisPage;
    private bool _ShowEditLinks = false;

    public string Title
    {
        get { return _Title; }
        set { _Title = value; }
    }

    public string Summary
    {
        get { return _Summary; }
        set { _Summary = value; }
    }

    public string Body
    {
        get { return _Body; }
        set { _Body = value; }
    }

    public System.DateTime ModifiedOn
    {
        get { return _ModifiedOn; }
        set { _ModifiedOn = value; }
    }

    public string ModifiedBy
    {
        get { return _ModifiedBy; }
        set { _ModifiedBy = value; }
    }

    public bool ShowEditLinks
    {
        get { return _ShowEditLinks; }
        set { _ShowEditLinks = value; }
    }

    public CMS.Page thisPage
    {
        get { return _thisPage; }
        set { _thisPage = value; setPageValues(); }
    }

    public void setPageValues()
    {
        Title = _thisPage.Title;
        Summary = _thisPage.Summary;
        Body = _thisPage.Body;
        ModifiedBy = _thisPage.ModifiedBy;
        ModifiedOn = _thisPage.ModifiedOn;
        ShowEditLinks = _thisPage.ShowEditLinks;
    }

	public virtual void OnPageError(string message)
	{
		Response.Write(message);
	}

	public virtual void OnPageError(string message, Exception ex)
	{
		Response.Write(message);
		if (SiteUtility.UserIsAdmin() && ex != null)
			Response.Write(ex.Message);
	}

	///GCS-689: Support display of a list of errors in OnPageError
	public virtual void OnPageError(IList<string> list, Exception ex)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("<ul>");
		foreach (string s in list)
		{
			sb.Append("<li>");
			sb.Append(s);
			sb.Append("</li>");
		}
		sb.Append("</ul>");
		OnPageError(sb.ToString(), ex);
	}

	public virtual void OnPageSuccess(string message)
	{
		Response.Write(message);
	}

}