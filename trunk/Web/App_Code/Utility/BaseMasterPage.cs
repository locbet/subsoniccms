/// <summary>
/// Base Master Page for all Start Site user controls
/// </summary>
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

}

