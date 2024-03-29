using System;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Security.Permissions;
using System.Data.Common;
using System.Data;
using System.Web.Caching;
using System.Collections;

/// <summary>
/// Shamelessly stolen from Jeff Prosise's excellent article on SiteMapProviders
/// I liked it, but needed to do this the SubSonic way...
/// Modified by the following post:
/// http://www.subsonicproject.com/forums/thread/2.0.x-starter-site/1195.aspx
/// The modification allows for static pages to be automatically included in the site map
/// without resorting to hardcoding stuff in site.master.cs.
/// </summary>
[SqlClientPermission(SecurityAction.Demand, Unrestricted = true)]
public class SubSonicSiteMapProvider : StaticSiteMapProvider
{

    private Dictionary<int, SiteMapNode> _nodes = new Dictionary<int, SiteMapNode>(16);
    private readonly object _lock = new object();
	private SiteMapNode _root;

    public override void Initialize(string name, NameValueCollection config)
    {
        // Verify that config isn't null
        if (config == null)
            throw new ArgumentNullException("config");

        // Assign the provider a default name if it doesn't have one
        if (String.IsNullOrEmpty(name))
            name = "SubSonicSiteMapProvider";

        // Add a default "description" attribute to config if the
        // attribute doesn't exist or is empty
        if (string.IsNullOrEmpty(config["description"]))
        {
            config.Remove("description");
            config.Add("description", "SubSonic site map provider");
        }

        // Call the base class's Initialize method
        base.Initialize(name, config);


    }

    /// <summary>
    /// Creates the site map node.
    /// </summary>
    /// <param name="link">The link.</param>
    /// <param name="rewrittenDirectory">The rewritten directory.</param>
    /// <param name="pageHash">The page hash.</param>
    /// <returns></returns>
    private SiteMapNode CreateSiteMapNode(CMS.Page link, string rewrittenDirectory, Hashtable pageHash)
    {
        SiteMapNode node = null;

		if (link.ShowInMenu)
		{
            string[] rolelist = null;
            if (!String.IsNullOrEmpty(link.ViewRoles))
            {
                rolelist = link.ViewRoles.Split(new char[] { ',', ';' }, 512);
            }

            switch (link.PageTypeID)
			{
				case -1:
					{
						node = new SiteMapNode(this, link.PageID.ToString(), "", link.MenuTitle, link.Summary, rolelist, null, null, null);
						break;
					}
				case 1:
					{
						node = new SiteMapNode(this, link.PageID.ToString(), link.PageUrl, link.MenuTitle, link.Summary, rolelist, null, null, null);
						break;
					}
				default:
					{
						node = new SiteMapNode(this, link.PageID.ToString(), rewrittenDirectory + link.PageUrl, link.MenuTitle, link.Summary, rolelist, null, null, null);
						break;
					}
			}
			if (!pageHash.ContainsKey(link.PageID))
			{
				pageHash.Add(link.PageID, node);
			}
		}
        return node;
    }

    public override SiteMapNode BuildSiteMap()
    {
		lock (_lock)
        {
            // Return immediately if this method has been called before
            if (_root != null)
                return _root;


            Hashtable pageHash = new Hashtable();
            CMS.PageCollection links = new CMS.PageCollection().OrderByAsc("ordinal").Load();

			//you can change this as needed
			string rewrittenDirectory = "~/view/";

			rewrittenDirectory = VirtualPathUtility.Combine(System.Web.HttpRuntime.AppDomainAppVirtualPath, rewrittenDirectory);

			// Normalize url
			rewrittenDirectory = VirtualPathUtility.ToAbsolute(rewrittenDirectory);

			foreach (CMS.Page link in links)
			{
				if (link.ParentID == null && link.PageUrl == "default.aspx" && link.Ordinal == -1)
				{
					_root = CreateSiteMapNode(link, rewrittenDirectory, pageHash);
					break;
				}
			}
					
						
			//top level node

			if (_root == null)
			{
				_root = new SiteMapNode(this, "0", "~/default.aspx", "Home", "Return to main page", null, null, null, null);
			}
            AddNode(_root, null);

            foreach (CMS.Page link in links)
            {

                if (link.ParentID == null)
                {
                    SiteMapNode node = CreateSiteMapNode(link, rewrittenDirectory, pageHash);
					if (node != null && node.Key != _root.Key)
	                    AddNode(node, _root);


                }
            }

            //add in the child nodes
            foreach (CMS.Page link in links)
            {
               
                if (link.ParentID != null)
                {
                    // Create a SiteMapNode
                    SiteMapNode node = CreateSiteMapNode(link, rewrittenDirectory, pageHash);
                    SiteMapNode parentNode = null;
					if (node != null)
					{
						// First check if the parent has been created in the hash
						if (pageHash.ContainsKey(link.ParentID))
						{
							parentNode = (SiteMapNode)pageHash[link.ParentID];
						}
						else
						{


							//loop the nodes to find the parent
							foreach (CMS.Page pLink in links)
							{
								if (pLink.PageID == link.ParentID)
								{
									parentNode = CreateSiteMapNode(pLink, rewrittenDirectory, pageHash);
									break;
								}
							}
						}
						AddNode(node, parentNode);
					}
                }
            }


            return _root;
        }
    }

    protected override SiteMapNode GetRootNodeCore()
    {
        lock (_lock)
        {
            BuildSiteMap();
            return _root;
        }
    }

    public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
    {
        //return base.IsAccessibleToUser(context, node);
        //borrowed this method from http://fredrik.nsquared2.com/viewpost.aspx?PostID=272
        if (node == null)
            throw new ArgumentNullException("node");

        if (context == null)
            throw new ArgumentNullException("context");

        if (!this.SecurityTrimmingEnabled)
            return true;

        if ((node.Roles != null) && (node.Roles.Count > 0))
        {
            foreach (string role in node.Roles)
            {
                if (SiteUtility.IsUserInRole(context.User.Identity.Name, role))
                    return true;
            }
        }
        return false;
    }

    public void Reload()
    {
        lock (_lock)
        {
            // Refresh the site map
            Clear();
            _nodes.Clear();
            _root = null;
        }
    }

}