using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Management;
using System.Web;

namespace WebEvents
{
	public class InputValidationEvent: WebFailureAuditEvent
	{
		private Exception _ex;
		public InputValidationEvent(object sender, string message)
			: base(message, sender, WebEventCodes.WebExtendedBase + 1)
		{ }

		public InputValidationEvent(object sender, string message, Exception ex)
			: base(message, sender, WebEventCodes.WebExtendedBase + 1)
		{
			_ex = ex;
		}

		public override void FormatCustomEventDetails(WebEventFormatter formatter)
		{
			base.FormatCustomEventDetails(formatter);

			if (_ex != null)
			{
				// Add custom data.
				formatter.AppendLine("Exception Details:");
				formatter.IndentationLevel += 1;
				formatter.AppendLine("Message: " + _ex.Message);
				formatter.AppendLine("Source: " + _ex.Source);
				formatter.AppendLine("Trace: " + _ex.StackTrace);

				// Display custom event message.
				formatter.AppendLine("");

				formatter.IndentationLevel -= 1;
			}
		}
	}

	public class LoginFailureAccountLockedEvent : WebAuthenticationFailureAuditEvent
	{
		public LoginFailureAccountLockedEvent(object sender, string nameToAuthenticate)
			: base("Login Failure - Account Locked", sender, WebEventCodes.WebExtendedBase + 2, nameToAuthenticate)
		{
		}

	}

	public class LoginFailureEvent : WebAuthenticationFailureAuditEvent
	{
		public LoginFailureEvent(object sender, string nameToAuthenticate)
			: base("Login Failure", sender, WebEventCodes.WebExtendedBase + 3, nameToAuthenticate)
		{
		}
	}

	public class LoginSuccessEvent : WebAuthenticationSuccessAuditEvent
	{
		public LoginSuccessEvent(object sender, string nameToAuthenticate)
			: base("Login Success", sender, WebEventCodes.WebExtendedBase + 4, nameToAuthenticate)
		{
		}
	}

	public class LogoutSuccessEvent : WebAuthenticationSuccessAuditEvent
	{
		public LogoutSuccessEvent(object sender, string nameToAuthenticate)
			: base("Logout Success", sender, WebEventCodes.WebExtendedBase + 5, nameToAuthenticate)
		{
		}
	}

	public class RemoveUserFromRolesSuccessEvent : WebSuccessAuditEvent
	{
		private static string _user;
		private static string _roles;
		public RemoveUserFromRolesSuccessEvent(object sender, string user, string roles)
			: base("Remove User From Roles", sender, WebEventCodes.WebExtendedBase + 6)
		{
			_user = user;
			_roles = roles;
		}

		public override void FormatCustomEventDetails(WebEventFormatter formatter)
        {
            base.FormatCustomEventDetails(formatter);

            // Add custom data.
            formatter.AppendLine("");
            formatter.IndentationLevel += 1;
            formatter.AppendLine(string.Format("Requested By: {0}",
                HttpContext.Current.User.Identity.Name));
            formatter.AppendLine(string.Format("Affected User: {0}",
                _user));

            // Display custom event message.
			formatter.AppendLine(string.Format("Removed from Roles: {0}",
				_roles));

            formatter.IndentationLevel -= 1;
        }  

	
	
	}

	public class AddUserToRolesSuccessEvent : WebSuccessAuditEvent
	{
		private static string _user;
		private static string _roles;
		public AddUserToRolesSuccessEvent(object sender, string user, string roles)
			: base("Add User To Roles", sender, WebEventCodes.WebExtendedBase + 7)
		{
			_user = user;
			_roles = roles;
		}

		public override void FormatCustomEventDetails(WebEventFormatter formatter)
		{
			base.FormatCustomEventDetails(formatter);

			// Add custom data.
			formatter.AppendLine("");
			formatter.IndentationLevel += 1;
			formatter.AppendLine(string.Format("Requested By: {0}",
				HttpContext.Current.User.Identity.Name));
			formatter.AppendLine(string.Format("Affected User: {0}",
				_user));

			// Display custom event message.
			formatter.AppendLine(string.Format("Added to Roles: {0}",
				_roles));

			formatter.IndentationLevel -= 1;
		}



	}

	public class CreateUserSuccessEvent : WebSuccessAuditEvent
	{
		private static string _user;
		public CreateUserSuccessEvent(object sender, string user)
			: base("Created User", sender, WebEventCodes.WebExtendedBase + 8)
		{
			_user = user;
		}

		public override void FormatCustomEventDetails(WebEventFormatter formatter)
		{
			base.FormatCustomEventDetails(formatter);

			// Add custom data.
			formatter.AppendLine("");
			formatter.IndentationLevel += 1;
			formatter.AppendLine(string.Format("Requested By: {0}",
				HttpContext.Current.User.Identity.Name));
			formatter.AppendLine(string.Format("New User: {0}",
				_user));

			formatter.IndentationLevel -= 1;
		}



	}

	public class AccountUnlockedEvent : WebSuccessAuditEvent
	{
		private static string _user;
		public AccountUnlockedEvent(object sender, string user)
			: base("Account Unlocked", sender, WebEventCodes.WebExtendedBase + 9)
		{
			_user = user;
		}

		public override void FormatCustomEventDetails(WebEventFormatter formatter)
		{
			base.FormatCustomEventDetails(formatter);

			// Add custom data.
			formatter.AppendLine("");
			formatter.IndentationLevel += 1;
			formatter.AppendLine(string.Format("Requested By: {0}",
				HttpContext.Current.User.Identity.Name));
			formatter.AppendLine(string.Format("Affected User: {0}",
				_user));

			formatter.IndentationLevel -= 1;
		}

	}

	public class DeleteUserSuccessEvent : WebSuccessAuditEvent
	{
		private static string _user;
		public DeleteUserSuccessEvent(object sender, string user)
			: base("Deleted User", sender, WebEventCodes.WebExtendedBase + 10)
		{
			_user = user;
		}

		public override void FormatCustomEventDetails(WebEventFormatter formatter)
		{
			base.FormatCustomEventDetails(formatter);

			// Add custom data.
			formatter.AppendLine("");
			formatter.IndentationLevel += 1;
			formatter.AppendLine(string.Format("Requested By: {0}",
				HttpContext.Current.User.Identity.Name));
			formatter.AppendLine(string.Format("Deleted User: {0}",
				_user));

			formatter.IndentationLevel -= 1;
		}

	}

	public class AccountApprovedEvent : WebSuccessAuditEvent
	{
		private static string _user;
		public AccountApprovedEvent(object sender, string user)
			: base("Account Approved", sender, WebEventCodes.WebExtendedBase + 11)
		{
			_user = user;
		}

		public override void FormatCustomEventDetails(WebEventFormatter formatter)
		{
			base.FormatCustomEventDetails(formatter);

			// Add custom data.
			formatter.AppendLine("");
			formatter.IndentationLevel += 1;
			formatter.AppendLine(string.Format("Requested By: {0}",
				HttpContext.Current.User.Identity.Name));
			formatter.AppendLine(string.Format("Affected User: {0}",
				_user));

			formatter.IndentationLevel -= 1;
		}

	}

	public class AccountUnapprovedEvent : WebSuccessAuditEvent
	{
		private static string _user;
		public AccountUnapprovedEvent(object sender, string user)
			: base("Account Unapproved", sender, WebEventCodes.WebExtendedBase + 12)
		{
			_user = user;
		}

		public override void FormatCustomEventDetails(WebEventFormatter formatter)
		{
			base.FormatCustomEventDetails(formatter);

			// Add custom data.
			formatter.AppendLine("");
			formatter.IndentationLevel += 1;
			formatter.AppendLine(string.Format("Requested By: {0}",
				HttpContext.Current.User.Identity.Name));
			formatter.AppendLine(string.Format("Affected User: {0}",
				_user));

			formatter.IndentationLevel -= 1;
		}

	}

	public class AccountLockedEvent : WebSuccessAuditEvent
	{
		private static string _user;
		public AccountLockedEvent(object sender, string user)
			: base("Account Locked", sender, WebEventCodes.WebExtendedBase + 13)
		{
			_user = user;
		}

		public override void FormatCustomEventDetails(WebEventFormatter formatter)
		{
			base.FormatCustomEventDetails(formatter);

			// Add custom data.
			formatter.AppendLine("");
			formatter.IndentationLevel += 1;
			formatter.AppendLine(string.Format("Requested By: {0}",
				HttpContext.Current.User.Identity.Name));
			formatter.AppendLine(string.Format("Affected User: {0}",
				_user));

			formatter.IndentationLevel -= 1;
		}

	}

	public class AddRolesSuccessEvent : WebSuccessAuditEvent
	{
		private static string _roles;
		public AddRolesSuccessEvent(object sender, string roles)
			: base("Add Roles", sender, WebEventCodes.WebExtendedBase + 14)
		{
			_roles = roles;
		}

		public override void FormatCustomEventDetails(WebEventFormatter formatter)
		{
			base.FormatCustomEventDetails(formatter);

			// Add custom data.
			formatter.AppendLine("");
			formatter.IndentationLevel += 1;
			formatter.AppendLine(string.Format("Requested By: {0}",
				HttpContext.Current.User.Identity.Name));

			// Display custom event message.
			formatter.AppendLine(string.Format("New Roles: {0}",
				_roles));

			formatter.IndentationLevel -= 1;
		}



	}

	public class RemoveRolesSuccessEvent : WebSuccessAuditEvent
	{
		private static string _roles;
		public RemoveRolesSuccessEvent(object sender, string roles)
			: base("Remove Roles", sender, WebEventCodes.WebExtendedBase + 15)
		{
			_roles = roles;
		}

		public override void FormatCustomEventDetails(WebEventFormatter formatter)
		{
			base.FormatCustomEventDetails(formatter);

			// Add custom data.
			formatter.AppendLine("");
			formatter.IndentationLevel += 1;
			formatter.AppendLine(string.Format("Requested By: {0}",
				HttpContext.Current.User.Identity.Name));

			// Display custom event message.
			formatter.AppendLine(string.Format("Removed Roles: {0}",
				_roles));

			formatter.IndentationLevel -= 1;
		}



	}

	public class PageAccessSuccessEvent : WebAuthenticationSuccessAuditEvent
	{
		private static string _pageUrl = "";
		public PageAccessSuccessEvent(object sender, string userName, string pageUrl)
			: base("Page Access Success", sender, WebEventCodes.WebExtendedBase + 16, userName)
		{
			_pageUrl = pageUrl;
		}

		public override void FormatCustomEventDetails(WebEventFormatter formatter)
		{
			base.FormatCustomEventDetails(formatter);

			// Add custom data.
			formatter.AppendLine("");
			formatter.IndentationLevel += 1;
			formatter.AppendLine(string.Format("Requested By: {0}",
				HttpContext.Current.User.Identity.Name));

			formatter.AppendLine(string.Format("Requested Page: {0}",
				_pageUrl));

			formatter.IndentationLevel -= 1;
		}

	}

	public class PageAccessFailureEvent : WebAuthenticationFailureAuditEvent
	{
		private static string _pageUrl = "";
		public PageAccessFailureEvent(object sender, string userName, string pageUrl)
			: base("Page Access Failure", sender, WebEventCodes.WebExtendedBase + 17, userName)
		{
			_pageUrl = pageUrl;
		}

		public override void FormatCustomEventDetails(WebEventFormatter formatter)
		{
			base.FormatCustomEventDetails(formatter);

			// Add custom data.
			formatter.AppendLine("");
			formatter.IndentationLevel += 1;
			formatter.AppendLine(string.Format("Requested By: {0}",
				HttpContext.Current.User.Identity.Name));

			formatter.AppendLine(string.Format("Requested Page: {0}",
				_pageUrl));

			formatter.IndentationLevel -= 1;
		}
	}
}
