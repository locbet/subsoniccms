using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

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
        public const string ROLE_REGEX= "[0-9a-zA-z][0-9a-zA-Z\\s-]";
        public const string USER_SEARCH_REGEX = "[0-9a-zA-z][0-9a-zA-Z\\s-]";
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
				//new WebEvents.AddUserToRolesSuccessEvent(HttpContext.Current, u, role).Raise();
			}
			else if (!boxChecked && userInRole)
			{
				Roles.RemoveUserFromRoles(u, new string[] { role });
				//new WebEvents.RemoveUserFromRolesSuccessEvent(HttpContext.Current, u, role).Raise();
			}
			return true;
		}
		catch (Exception ex)
		{
			throw new Exception("Unable to update role membership. <br />" + ex.Message, ex);
		}
	}

	public static bool IsUserInRole(string u, string role)
	{
		if (string.IsNullOrEmpty(u) ||
			string.IsNullOrEmpty(role))
		{
			return false;
		}

		return Roles.IsUserInRole(u, role);
	}

	public static void RemoveUserFromAllRoles(string u)
	{
		string[] roles = Roles.GetRolesForUser(u);
		foreach (string r in roles)
		{
			Roles.RemoveUserFromRoles(u, new string[] { r });
			//new WebEvents.RemoveUserFromRolesSuccessEvent(HttpContext.Current, u, r).Raise();
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
			//new WebEvents.AccountApprovedEvent(HttpContext.Current, user.UserName).Raise();
			msg = user.UserName + "'s account has been approved.";
		}
		else
		{
			//new WebEvents.AccountUnapprovedEvent(HttpContext.Current, user.UserName).Raise();
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
		//new WebEvents.DeleteUserSuccessEvent(HttpContext.Current, userName);
		if (userName == HttpContext.Current.User.Identity.Name.ToString())
		{
			//new WebEvents.LogoutSuccessEvent(HttpContext.Current, HttpContext.Current.User.Identity.Name.ToString()).Raise();
			FormsAuthentication.SignOut();
			HttpContext.Current.Response.Redirect("~/default.aspx", false);
		}
	}
	public static void BuildBasePage(BasePage thisBase, BaseMasterPage thisMaster, string pageUrl)
	{
		//this will throw an error if the page doesn't exist
        CMS.Page thisPage = new CMS.Page();
		try
		{

            thisPage = CMS.ContentService.GetPage(pageUrl);
			if (thisPage == null || thisPage.PageID == 0)
			{
				thisPage = new CMS.Page();
				throw new Exception("Page not found.");
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
		catch
		{
			thisPage.Title = "Oops! Can't find that page...";
			thisBase.Title = thisPage.Title;
			string FourOFourText = SubSonic.Sugar.Files.GetFileText(HttpContext.Current.Server.MapPath("~/CMSFiles/404.htm"));
			thisPage.Body = FourOFourText;
			thisPage.IsFourOFour = true;
		}
        thisMaster.thisPage = thisPage;
		return;
	}
}
