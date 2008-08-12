//created by Neil C. Meredith (9/10/2007) (see http://www.codeproject.com/KB/ajax/Flash_user_confirmation.aspx)
//modified for c# by Dave Neeley (8/2/2008)
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

public partial class Modules_ContentManager_FlashMessage : System.Web.UI.UserControl
{
	int _Interval = 4000;
    int _FadeInDuration = 500;
    int _FadeInSteps = 20;
    int _FadeOutDuration = 500;
    int _FadeOutSteps = 20;
    //bool _InsertJavascript = true;

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	#region Properties
    /// <summary>
    /// Override the default css class
    /// </summary>
    public string CssClass
	{
        get { return lblMessage.CssClass; }
        set { lblMessage.CssClass = value;}
    }

    /// <summary>
    /// The message to display
    /// </summary>
    public string Message
	{ 
        get {return lblMessage.Text; }
        set{ lblMessage.Text = value;}
    }

    /// <summary>
    /// Time in milliseconds to display the message
    /// </summary>
    public int Interval
	{
        get { return _Interval;}
        set { _Interval = value;}
    }

    public int FadeInDuration
	{
        get {return _FadeInDuration;}
        set { _FadeInDuration = value;}
    }

    public int FadeInSteps
	{
        get {return _FadeInSteps;}
        set{ _FadeInSteps = value;}
    }


    public int FadeOutDuration
	{
        get{return _FadeOutDuration;}
        set{ _FadeOutDuration = value;}
    }

    public int FadeOutSteps
	{
        get{return _FadeOutSteps;}
        set{_FadeOutSteps = value;}
    }

	#endregion

	#region Methods

    public void Display()
	{

        //Set the duration, steps, etc. for the javascript fade in and fade out   
        string js = "fadeIn(//" + lblMessage.ClientID + 
            "//, " + FadeInSteps + 
            ", " + FadeInDuration + 
            ", " + Interval + 
            ", " + FadeOutSteps + 
            ", " + FadeOutDuration + ");";


        //Find the script manager on the page, and the update panel the FlashMessage control
        //is nested in
        ScriptManager sm = ScriptManager.GetCurrent(this.Page);
        UpdatePanel up = (UpdatePanel)GetParentOfType(lblMessage, typeof(UpdatePanel));


        if (sm != null && up != null)
		{
            //The user control is nested in an update panel, register the javascript with the script manager and
            //attach it to the update panel in order to fire it after the async postback
            if (sm.IsInAsyncPostBack)
                ScriptManager.RegisterClientScriptBlock(up, typeof(UpdatePanel), "jscript1", js, true);
		}
        else
		{
            //The user control is not in an update panel (or there is no script manager on the page), 
            //so register the javascript for a normal postback
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "jscript1", js, true);
        }

    }


    /// <summary>
    /// Return the parent control of the given root control, as long as it matches the entered type
    /// </summary>
    private Control GetParentOfType(Control root, System.Type Type)
	{
        Control parent = root.Parent;
        if (parent == null)
            return null;
        if (parent.GetType().ToString() == Type.ToString())
            return root.Parent;
        Control p = GetParentOfType(parent, Type);
        if (p != null)
            return p;
        return null;
    }

	#endregion

}
