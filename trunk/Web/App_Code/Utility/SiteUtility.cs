using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Profile;
using System.Text.RegularExpressions;

/// <summary>
/// A class for you to use as needed; extend's SubSonic's core utility set
/// </summary>
public class SiteUtility:SubSonic.Utilities.Utility
{
    /// <summary>
    /// This is the Role that we evaluate for CMS editing privvies
    /// Change as needed
    /// </summary>
	/// 
    public class SiteRoles
    {
        public const string CONTENT_EDITOR_ROLE = "Content Editor";
        public const string CONTENT_CREATOR_ROLE = "Content Creator";
        public const string ADMINISTRATOR_ROLE = "Administrator";
    }

    public class Validation
    {
        public const string ROLE_REGEX= "[0-9a-zA-z][0-9a-zA-Z\\s-]+";
        public const string USER_SEARCH_REGEX = "[0-9a-zA-z][0-9a-zA-Z\\s-]+";
    }

	public const string URL_FOUR_O_FOUR = "~/view/not-found.aspx";

	public static string UserLoginMessage()
	{
		//set any additional profile values here
		ProfileBase p = ProfileBase.Create(HttpContext.Current.User.Identity.Name.ToString());
		if (p == null)
		{
			return String.Concat("Welcome Back ", HttpContext.Current.User.Identity.Name.ToString(), "!");
		}
		else
			return String.Concat("Hi ", p.GetPropertyValue("FirstName") , "!");

	}

	public static string LoginUrl()
	{
		return ResolveUrl("~/login.aspx?ReturnURL=" + HttpContext.Current.Request.Url.PathAndQuery);
	}

	public static string UserID()
	{
		return HttpContext.Current.User.Identity.Name;
	}
    public static bool UserIsAdmin()
	{
		return HttpContext.Current.User.IsInRole(SiteRoles.ADMINISTRATOR_ROLE);
	}

	public static bool UserCanCreate()
	{
        if (HttpContext.Current.User.IsInRole(SiteRoles.CONTENT_CREATOR_ROLE))
			return true;
		else
			return false;
	}

	public static bool UserCanEdit()
	{
        if (HttpContext.Current.User.IsInRole(SiteRoles.CONTENT_EDITOR_ROLE) || HttpContext.Current.User.IsInRole(SiteRoles.CONTENT_CREATOR_ROLE))
			return true;
		else
			return false;
	}

    public static bool UserCanSearch()
    {
        // you can add special logic if search should be limited on your site
        return true;
    }
	public static bool UpdateRoleMembership(string u, CheckBox box)
	{
		return SiteUtility.UpdateRoleMembership(u, box.Text, box.Checked);
	}
	
	public static bool UpdateRoleMembership(string u, string role, bool boxChecked)
	{
		// Array manipulation because cannot use Roles static method (need different appPath).
		bool userInRole = SiteUtility.IsUserInRole(u, role);
		try
		{
			//shows a message in the save method when allowed == false
			if (u == HttpContext.Current.User.Identity.Name.ToString())
				return false;
			if (boxChecked && !userInRole)
			{
				Roles.AddUserToRoles(u, new string[] { role });
				new WebEvents.AddUserToRolesSuccessEvent(HttpContext.Current, u, role).Raise();
			}
			else if (!boxChecked && userInRole)
			{
				Roles.RemoveUserFromRoles(u, new string[] { role });
				new WebEvents.RemoveUserFromRolesSuccessEvent(HttpContext.Current, u, role).Raise();
			}
			return true;
		}
		catch (Exception ex)
		{
			throw new Exception("Unable to update role membership.", ex);
		}
	}

    public static bool IsUserInRole(string u, string[] roles)
    {
        foreach (string r in roles)
        {
            if (IsUserInRole(u, r))
                return true;
        }
        return false;
    }

    public static bool IsUserInRole(string u, string role)
	{
		role = role.Trim();
        if (string.IsNullOrEmpty(u) ||
            string.IsNullOrEmpty(role))
        {
            return false;
        }
        else if (role == "+")
            return UserIsAuthenticated();
        else if (role == "*")
            return true;

		return Roles.IsUserInRole(u, role);
	}

	public static void RemoveUserFromAllRoles(string u)
	{
		string[] roles = Roles.GetRolesForUser(u);
		foreach (string r in roles)
		{
			Roles.RemoveUserFromRoles(u, new string[] { r });
			new WebEvents.RemoveUserFromRolesSuccessEvent(HttpContext.Current, u, r).Raise();
		}
	}

	public static string ToggleUserApprovedStatus(CheckBox checkBox)
	{
		string userID = null;
		if (!string.IsNullOrEmpty(checkBox.Attributes["Value"]))
			userID = checkBox.Attributes["Value"];

		if (String.IsNullOrEmpty(userID))
			throw new Exception("Invalid user id.");

		MembershipUser user = Membership.FindUsersByName(userID)[userID];
		user.IsApproved = checkBox.Checked;

		Membership.UpdateUser(user);
		string msg = "";
		if (user.IsApproved)
		{
			new WebEvents.AccountApprovedEvent(HttpContext.Current, user.UserName).Raise();
			msg = user.UserName + "'s account has been approved.";
		}
		else
		{
			new WebEvents.AccountUnapprovedEvent(HttpContext.Current, user.UserName).Raise();
			msg = user.UserName + "'s account has been denied access to the site.";
		}
		if (String.IsNullOrEmpty(msg))
			throw new Exception("Unknown error occurred while changing user approval status.");
		return msg;
	}

	public static void DeleteUser(string userName)
	{
		if (String.IsNullOrEmpty(userName))
			throw new Exception("Invalid user id.");
		//we're keeping all the data, except roles. remove those.
		SiteUtility.RemoveUserFromAllRoles(userName);
		Membership.DeleteUser(userName, false);
		new WebEvents.DeleteUserSuccessEvent(HttpContext.Current, userName);
		if (userName == HttpContext.Current.User.Identity.Name.ToString())
		{
			new WebEvents.LogoutSuccessEvent(HttpContext.Current, HttpContext.Current.User.Identity.Name.ToString()).Raise();
			FormsAuthentication.SignOut();
			Redirect("~/default.aspx");
		}
	}
	public static void Redirect(int responseCode)
	{
		Redirect(responseCode, String.Empty);
	}

	public static void Redirect(string newUrl)
	{
		Redirect(0, newUrl);
	}

	public static void Redirect(int responseCode, string returnUrl)
	{
		string newUrl = "";
		switch (responseCode)
		{
			case 403:
				newUrl = String.Concat("~/login.aspx", (!String.IsNullOrEmpty(returnUrl) ? "?ReturnUrl=" + HttpContext.Current.Server.HtmlEncode(returnUrl) : ""));
				new WebEvents.PageAccessFailureEvent(HttpContext.Current, HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.Url.PathAndQuery).Raise();
				break;
			case 404:
				newUrl = URL_FOUR_O_FOUR;
				new WebEvents.PageAccessFailureEvent(HttpContext.Current, HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.Url.PathAndQuery).Raise();
				break;
			default:
				newUrl = returnUrl;
				break;
		}
		HttpContext.Current.Response.Redirect(newUrl, true);
	}
	public static void BuildBasePage(BasePage thisBase, BaseMasterPage thisMaster, string pageUrl)
	{
		//this will throw an error if the page doesn't exist
        CMS.Page thisPage = new CMS.Page();
		try
		{
			if (!pageUrl.ToLower().EndsWith(".aspx"))
				pageUrl = pageUrl + ".aspx";

            thisPage = CMS.ContentService.GetPage(pageUrl);
			if (thisPage == null || thisPage.PageID == 0)
			{
				thisPage = new CMS.Page();
                throw new CMS.PageNotFoundException();
			}
            else if (pageUrl != "not-found.aspx" && pageUrl != "login.aspx" && !IsUserInRole(HttpContext.Current.User.Identity.Name, thisPage.ViewRoles.Split(new char[] { ',', ';' }, 512)) && !IsUserInRole(HttpContext.Current.User.Identity.Name, thisPage.EditRoles.Split(new char[] { ',', ';' }, 512)))
            {
                throw new CMS.PageNotAuthorizedException();
            }

			//add browser window title
			thisBase.Title = thisPage.Title;

			//add the keywords
			HtmlHead head = (System.Web.UI.HtmlControls.HtmlHead)thisBase.Header;

			//Create a htmlMeta object
			HtmlMeta meta = new HtmlMeta();

			//Specify meta attributes
			meta.Attributes.Add("name", "keywords");
			meta.Attributes.Add("content", thisPage.Keywords);

			// Add the meta object to the htmlhead's control collection
			head.Controls.Add(meta);
			thisPage.IsFourOFour = false;
		}
        catch (CMS.PageNotFoundException)
		{
			thisPage.Title = "Oops! Can't find that page...";
			thisBase.Title = thisPage.Title;
			string FourOFourText = SubSonic.Sugar.Files.GetFileText(HttpContext.Current.Server.MapPath("~/CMSFiles/404.htm"));
			thisPage.Body = FourOFourText;
            thisPage.IsFourOFour = true;
		}
        catch 
        {
            throw;
        }
        thisMaster.thisPage = thisPage;
		return;
	}

	/// <summary>
	/// grabbed this from Rick Strahl's most excellent blog:
	/// http://www.west-wind.com/Weblog/posts/154812.aspx
	/// Returns a site relative HTTP path from a partial path starting out with a ~.
	/// Same syntax that ASP.Net internally supports but this method can be used
	/// outside of the Page framework.
	/// 
	/// Works like Control.ResolveUrl including support for ~ syntax
	/// but returns an absolute URL.
	/// </summary>
	/// <param name="originalUrl">Any Url including those starting with ~</param>
	/// <returns>relative url</returns>
	public static string ResolveUrl(string originalUrl)
	{
		if (originalUrl == null)
			return null;

		// *** Absolute path - just return
		if (originalUrl.IndexOf("://") != -1)
			return originalUrl;

		// *** Fix up image path for ~ root app dir directory
		if (originalUrl.StartsWith("~"))
		{
			string newUrl = "";
			if (HttpContext.Current != null)
				newUrl = HttpContext.Current.Request.ApplicationPath +
					  originalUrl.Substring(1).Replace("//", "/");
			else
				// *** Not context: assume current directory is the base directory
				throw new ArgumentException("Invalid URL: Relative URL not allowed.");

			// *** Just to be sure fix up any double slashes
			return newUrl;
		}

		return originalUrl;
	}

	/// <summary>
	/// This method returns a fully qualified absolute server Url which includes
	/// the protocol, server, port in addition to the server relative Url.
	/// 
	/// Works like Control.ResolveUrl including support for ~ syntax
	/// but returns an absolute URL.
	/// </summary>
	/// <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
	/// <param name="forceHttps">if true forces the url to use https</param>
	/// <returns></returns>
	public static string ResolveServerUrl(string serverUrl, bool forceHttps)
	{
		// *** Is it already an absolute Url?
		if (serverUrl.IndexOf("://") > -1)
			return serverUrl;
		// *** Start by fixing up the Url an Application relative Url
		string newUrl = ResolveUrl(serverUrl);
		Uri originalUri = HttpContext.Current.Request.Url;
		newUrl = (forceHttps ? "https" : originalUri.Scheme) +
				 "://" + originalUri.Authority + newUrl;
		return newUrl;
	}

	/// <summary>
	/// This method returns a fully qualified absolute server Url which includes
	/// the protocol, server, port in addition to the server relative Url.
	/// 
	/// It work like Page.ResolveUrl, but adds these to the beginning.
	/// This method is useful for generating Urls for AJAX methods
	/// </summary>
	/// <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
	/// <returns></returns>
	public static string ResolveServerUrl(string serverUrl)
	{
		return ResolveServerUrl(serverUrl, false);
	}

	/// <summary>
	///remove the first section of the absolute path, 
	///which is presented like /sitename/somefile.aspx
	/// </summary>
	/// <param name="url"></param>
	/// <returns></returns>
	public static string RemoveRootFromUrl(string url)
	{
		string baseUrl = SiteUtility.ResolveUrl("~");
		//new WebEvents.InputValidationEvent(HttpContext.Current, "BaseUrl for " + url + " is " + baseUrl).Raise();
		if (url.StartsWith(baseUrl))
		{
			//new WebEvents.InputValidationEvent(HttpContext.Current, "Replacing " + baseUrl + " in " + url).Raise();
			Regex re = new Regex(baseUrl);
			url = re.Replace(url, "", 1);
		}
		if (url.StartsWith("/"))
		{
			Regex re = new Regex("/+");
			url = re.Replace(url, "", 1);
			//new WebEvents.InputValidationEvent(HttpContext.Current, "Replaced / in url, left with " + url).Raise();
		}
		return url;
	}
}
