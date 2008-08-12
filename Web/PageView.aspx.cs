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

public partial class PageView : BasePage
{
    private int _editPageID = 0;
    //This is the secret sauce to get the uploader to work properly
    protected void Page_Init(object sender, EventArgs e) {
        Body.SkinPath = "skins/office2003/";
        Session["FCKeditor:UserFilesPath"] = Page.ResolveUrl("~/CMSFiles");
		// stolen from 
		// http://www.subsonicproject.com/forums/thread/code-share/2257.aspx
		// Disable viewstate for all edit controls since
		// these won't be used when not editing
		foreach (Control control in pnlEdit.Controls)
			control.EnableViewState = SiteUtility.UserCanEdit();
	}

    protected void Page_Load(object sender, EventArgs e)
    {
        string pageUrl = SubSonic.Utilities.Utility.GetParameter("p");
        if (pageUrl != "newpage.aspx" && pageUrl != "editpage.aspx")
        {
            BuildBasePage(pageUrl);
            ToggleEditor(false);
        }
        else if (SiteUtility.UserCanEdit())
        {
            switch (pageUrl)
            {
                case "newpage.aspx":
                    SetupNewPage();
                    this.BuildBasePage("view/newpage.aspx");
                    break;
                case "editpage.aspx":
                    string pageRef = SubSonic.Utilities.Utility.GetParameter("pRef");
                    this.BuildBasePage(pageRef);
                    LoadEditor();
                    break;
                default:
                    Response.Redirect("~/default.aspx");
                    break;
            }
        }
        else
        {
            Response.Redirect("~/login.aspx");
        }
    }

    #region Page State Methods
    void SetupNewPage() {
        ToggleEditor(true);
        editorTitle.Text = "Create New Page";
		//thisPage.PageTypeID = 0;
		btnDelete.Visible = false;
        //ParentID.Enabled = false;
        btnSetParent.Enabled = false;
		chkShowInMenu.Checked = true;
		pnlStaticURL.Visible = false;
		chkShowEditLinks.Checked = true;
        
        LoadRoles();
        LoadParentDrop();
		LoadPageTypes();

    }

	void ToggleEditor(bool showIt)
	{
        pnlEdit.Visible = showIt;
        pnlPublic.Visible = !showIt;
    }

    void LoadEditor() {
        if (Page.IsPostBack)
            return;
        CMS.Page thisPage = Master.thisPage;
        editPageID = thisPage.PageID;
        ToggleEditor(true);
        editorTitle.Text = "Edit " + thisPage.Title;
        //set the editor
        txtTitle.Text = thisPage.Title;
        txtSummary.Text = thisPage.Summary;
        Body.Value = thisPage.Body;
        txtMenuTitle.Text = thisPage.MenuTitle;
		chkShowInMenu.Checked = thisPage.ShowInMenu;
		txtOrdinal.Text = thisPage.Ordinal.ToString();
		chkShowEditLinks.Checked = thisPage.ShowEditLinks;
		switch (thisPage.PageTypeID)
		{
			case 1:
				txtStaticURL.Text = thisPage.PageUrl;
				break;
		}
		SetPageTypeItems(thisPage.PageTypeID.ToString());

		//set the delete confirmation
        btnDelete.Attributes.Add("onclick", "return CheckDelete();");

        LoadRoles();
        LoadPageHierarchy();
        LoadParentDrop();
		LoadPageTypes();
    }

	void SetPageTypeItems(string pageTypeID)
	{
		if (!String.IsNullOrEmpty(pageTypeID))
		{
			switch (Int32.Parse(pageTypeID))
			{
				case -1:
					pnlStaticURL.Visible = false;
					Body.Visible = false;
					break;
				case 0:
					pnlStaticURL.Visible = false;
					Body.Visible = true;
					break;
				case 1:
					pnlStaticURL.Visible = true;
					Body.Visible = true;
					break;
				default:
					pnlStaticURL.Visible = false;
					//we don't support this page type, so don't show the editor
					ToggleEditor(false);
					break;
			}
		}
	}
    #endregion

    #region Button Events

    protected void btnCancel_Click(object sender, EventArgs e) {
        Response.Redirect((Master.thisPage.PageUrl != "" ? "~/view/" + Master.thisPage.PageUrl : "~/default.aspx"));
    }
    protected void btnSave_Click(object sender, EventArgs e) {

        SavePage();
    }

    protected void btnDelete_Click(object sender, EventArgs e) {
        DeletePage();
    }

    protected void btnSetParent_Click(object sender, EventArgs e) {
        ResetParent();
    }

	protected void ddlPageType_SelectedIndexChanged(object sender, EventArgs e)
	{
		SetPageTypeItems(ddlPageType.SelectedValue);
	}


    #endregion

    #region Control loaders

    void LoadRoles() {
        if (chkRoles.Items.Count == 0) {
            string[] roles = Roles.GetAllRoles();
            chkRoles.Items.Clear();
            foreach (string s in roles) {
                ListItem item = new ListItem(s);
                chkRoles.Items.Add(item);
            }

            if (Master.thisPage != null)
            {
                //set the roles
                foreach (ListItem item in chkRoles.Items)
                {
                    if (Master.thisPage.Roles == "*")
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        if (Master.thisPage.Roles.Contains(item.Value))
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
        }
    }

    void LoadParentDrop() {
        if (ParentID.Items.Count == 0) {
            ListItemCollection items = GetPageHierarchy();
            foreach (ListItem item in items) {
                ParentID.Items.Add(item);
            }        //remove this page from the list, since it can't
            //be it's own parent
            if (Master.thisPage != null)
            {
                foreach (ListItem item in items)
                {
                    if (item.Value == Master.thisPage.PageID.ToString())
                    {
                        items.Remove(item);
                        break;
                    }
                }
            }


            //set the default
            ParentID.Items.Insert(0, new ListItem("--None--", "0"));

            //set the selection
            if (Master.thisPage != null && Master.thisPage.ParentID != null) {
                ParentID.SelectedValue = Master.thisPage.ParentID.ToString();
            } else {
                ParentID.SelectedIndex = 0;
            }
        }
    }

    ListItemCollection pageHierarchy=null;
    ListItemCollection GetPageHierarchy() {
        CMS.PageCollection pages = CMS.ContentService.GetHierarchicalPageCollection();

        if (pageHierarchy == null) {
            pageHierarchy = new ListItemCollection();
            lstHierarchy.Items.Clear();
            ListItem item;
            foreach(CMS.Page page in pages) {
                
                string lvlIndicator = string.Empty;
                for (int i = 0; i < page.Level; i++)
                    lvlIndicator += " - ";

                item = new ListItem(lvlIndicator + page.Title, page.PageID.ToString());

                pageHierarchy.Add(item);
            }
        }
        return pageHierarchy;
    }
    
    void LoadPageHierarchy() {
        ListItemCollection items = GetPageHierarchy();
        lstHierarchy.Items.Clear();
        foreach (ListItem item in items) {
            lstHierarchy.Items.Add(item);
        }
        try {
            lstHierarchy.SelectedValue = Master.thisPage.PageID.ToString();
        } catch {

        }


    }

	void LoadPageTypes()
	{
		if (!Page.IsPostBack)
		{

			//empty the current list
			ddlPageType.Items.Clear();
			CMS.PageTypeCollection ptc = new CMS.PageTypeCollection();
			ptc.Load();
			foreach (CMS.PageType pt in ptc)
			{
				string[] rolelist = null;
				if (!String.IsNullOrEmpty(pt.Roles))
				{
					rolelist = pt.Roles.Split(new char[] { ',', ';' }, 512);
				}

				foreach (string role in rolelist)
				{
					if (User.IsInRole(role))
					{
						ddlPageType.Items.Add(new ListItem(pt.Name, pt.Id.ToString()));
						break;
					}
				}

			}
			ddlPageType.SelectedValue = (Master.thisPage != null && !String.IsNullOrEmpty(Master.thisPage.PageTypeID.ToString()) ? Master.thisPage.PageTypeID.ToString() : "0");
		}
	}

    #endregion

    #region Service Calls

    void SavePage() {
        //set the editor
        string pageUrl = SubSonic.Utilities.Utility.GetParameter("p");
        CMS.Page thisPage = (!String.IsNullOrEmpty(pageUrl) && pageUrl == "newpage.aspx" ? new CMS.Page() : Master.thisPage);
        thisPage.Title = txtTitle.Text;
        thisPage.Summary = txtSummary.Text;
		thisPage.PageTypeID = Int32.Parse(ddlPageType.SelectedValue);
		switch (thisPage.PageTypeID)
		{
			case -1:
				thisPage.PageUrl = (!String.IsNullOrEmpty(thisPage.PageUrl) ? thisPage.PageUrl : "MenuItemOnly.aspx");
				break;
			case 1:
				thisPage.PageUrl = txtStaticURL.Text;
				break;
		}
		thisPage.Body = Body.Value;
		thisPage.MenuTitle = txtMenuTitle.Text;
        thisPage.Keywords = txtKeywords.Text;
		thisPage.Ordinal = Int32.Parse(txtOrdinal.Text);

        string selectedRoles = string.Empty;
        bool isAllRoles = true;
        foreach (ListItem item in chkRoles.Items) {
            if (item.Selected) {
                selectedRoles += item.Value + ",";
            } else {
                isAllRoles = false;
            }
        }
        if (isAllRoles) {
            selectedRoles = "*";
        } else {
            if (selectedRoles.Length > 0) {
                selectedRoles = selectedRoles.Remove(selectedRoles.Length - 1, 1);
            } else {
                selectedRoles = "*";
            }
        }

        thisPage.Roles = selectedRoles;

        if (ParentID.SelectedIndex != 0) {
            thisPage.ParentID = int.Parse(ParentID.SelectedValue);
        } else {
            thisPage.ParentID = null;
        }

		thisPage.ShowInMenu = chkShowInMenu.Checked;
		thisPage.ShowEditLinks = chkShowEditLinks.Checked;

        bool haveError = false;
        try {
            CMS.ContentService.SavePage(thisPage);
            ResetSiteMap();
        } catch (Exception x){
            haveError = true;
            ResultMessage1.ShowFail(x.Message);
        }
        //redirect to it
		if (!haveError)
		{
			Response.Redirect((thisPage.PageTypeID == 1 ? "" : "~/view/") + thisPage.PageUrl);
		}
    }

    void DeletePage() {
        //delete the page and send to the admin page list
        bool haveError = false;
        try {
            CMS.ContentService.DeletePage(Master.thisPage.PageID);
            this.ResetSiteMap();
        } catch (Exception x) {
            ResultMessage1.ShowFail(x.Message);
            haveError = true;
            ToggleEditor(true);
        }
		if (!haveError)
		{
			//some users won't be able to see this screen, so they will be redirected to the login.
			Response.Redirect("~/cms/cmspagelist.aspx");
		}
    }

    void ResetParent() {
        ToggleEditor(true);
        //reset the parent and reload the page
        try {
            int? newParentID = null;
            if(ParentID.SelectedIndex!=0)
                newParentID=int.Parse(ParentID.SelectedValue);

            CMS.ContentService.ChangeParent(Master.thisPage.PageID, newParentID);
            LoadPageHierarchy();
            ResetSiteMap();

        } catch (Exception x) {
            ResultMessage1.ShowFail(x.Message);
        }

    }

    void ResetSiteMap() {
        SubSonicSiteMapProvider siteMap = (SubSonicSiteMapProvider)SiteMap.Provider;
        siteMap.Reload();

    }

    #endregion

    #region props

    public int editPageID
    {
        get { return (_editPageID > 0 ? _editPageID : (ViewState[String.Concat(this.Page.UniqueID, this.ClientID) + "_editPageID"] != null ? (int)ViewState[String.Concat(this.Page.UniqueID, this.ClientID) + "_editPageID"] : 0)); }
        set { _editPageID = value; ViewState[String.Concat(this.Page.UniqueID, this.ClientID) + "_editPageID"] = value; }
    }
    
    #endregion
}
