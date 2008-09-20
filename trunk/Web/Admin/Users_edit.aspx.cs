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
using System.Web.Profile;

public partial class admin_user_edit : BasePage
{
	bool isEditMode = true;
    string userName = HttpContext.Current.Request["username"];
	bool allowed = true;

    protected void Page_Load(object sender, EventArgs e)
    {
		// Is in add mode?
        //
        if (string.IsNullOrEmpty(userName))
        {
            Page.Title = ActionTitle.Text = "Add User";
            isEditMode = false;
            PasswordRow.Visible = true;
            SecretQuestionRow.Visible = Membership.RequiresQuestionAndAnswer ? true : false;
            SecretAnswerRow.Visible = Membership.RequiresQuestionAndAnswer ? true : false;

            if (!Membership.RequiresQuestionAndAnswer)
            {
                ActiveUser.Checked = true;
                ActiveUser.Enabled = false;
            }
        }
        else
        {
            // We are in edit mode
            //
            PasswordRow.Visible = Membership.EnablePasswordRetrieval ? true : false;
            NewPassword.Visible = Membership.EnablePasswordRetrieval ? true : false;

            SecretQuestionRow.Visible = (Membership.RequiresQuestionAndAnswer && Membership.EnablePasswordRetrieval) ? true : false;
            SecretAnswerRow.Visible = (Membership.RequiresQuestionAndAnswer && Membership.EnablePasswordRetrieval) ? true : false;
        }

        if (!IsPostBack)
        {
            PopulateCheckboxes();
            // Is it in edit mode?
            //
            if (isEditMode)
            {
				LoadUserData();
				LoadProfileData();
			}
        }
    }

    private void PopulateCheckboxes()
    {
        if (Roles.Enabled)
        {
            CheckBoxRepeater.DataSource = Roles.GetAllRoles();
            CheckBoxRepeater.DataBind();

            if (CheckBoxRepeater.Items.Count > 0)
                SelectRolesLabel.Text = "Select Roles";
            else
                SelectRolesLabel.Text = "No Roles Defined";
        }
        else
        {
            SelectRolesLabel.Text = "Roles Not Enabled";
        }
    }

	private void LoadUserData()
	{
		UserID.Text = userName;
		UserID.Enabled = false;

		MembershipUser mu = Membership.GetUser(userName);
		if (mu == null)
		{
			return; // Review: scenarios where this happens.
		}

		Email.Text = mu.Email;
		ActiveUser.Checked = mu.IsApproved;
		ActiveUser.Attributes.Add("value", userName);

		if (Membership.EnablePasswordRetrieval)
		{
			// Load old password
			Password.Text = mu.GetPassword();
			Password.Enabled = false;

			// Load old secret question
			SecretQuestion.Text = mu.PasswordQuestion;
		}
		unlockUser.Enabled = mu.IsLockedOut;

	}

	private void LoadProfileData()
	{
		//set any additional profile values here
		ProfileBase p = ProfileBase.Create(userName);
		if (p == null)
		{
			return;
		}
		FirstName.Text = p.GetPropertyValue("FirstName").ToString();
		LastName.Text = p.GetPropertyValue("LastName").ToString();
		CommonName.Text = p.GetPropertyValue("CommonName").ToString();
        ThemePreference.SelectedValue = p.GetPropertyValue("ThemePreference").ToString();
	}

    public void SaveClick(object sender, EventArgs e)
    {
        if (isEditMode)
            UpdateUser(sender, e);
        else
            AddUser(sender, e);
    }

    private void UpdateRoleMembership(string u)
    {
        if (string.IsNullOrEmpty(u))
        {
            return;
        }

        foreach (RepeaterItem i in CheckBoxRepeater.Items)
        {
            CheckBox c = (CheckBox)i.FindControl("checkbox1");
            allowed = SiteUtility.UpdateRoleMembership(u, c);
			if (!allowed)
				break;
        }

        // Clear the checkboxes
        PopulateCheckboxes();
    }

    public void UpdateUser(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            return;
        }

        string userIDText = UserID.Text;
        string emailText = Email.Text;

        string password = null;
        string newPassword = null;
        string question = null;
        string answer = null;
        if (Membership.EnablePasswordRetrieval)
        {
            password = Password.Text.Trim();
            newPassword = NewPassword.Text.Trim();

            if (Membership.RequiresQuestionAndAnswer)
            {
                question = SecretQuestion.Text;
                answer = SecretAnswer.Text;
            }
        }                

        try
        {
            MembershipUser mu = Membership.GetUser(userIDText);

			UpdateRoleMembership(userIDText);

			mu.Email = Email.Text;
            mu.IsApproved = ActiveUser.Checked;



            Membership.UpdateUser(mu);

            // Are we allowed to change secret question & answer?
            // We will need old password for this.
            //
            if (Membership.EnablePasswordRetrieval &&
                Membership.RequiresQuestionAndAnswer &&
                password != null && 
                question != null && answer != null)
            {
                mu.ChangePasswordQuestionAndAnswer(password, question, answer);
            }

            // Are we allowed to change the password?
            // We will need old password for this.
            //
            if (Membership.EnablePasswordRetrieval &&
                !string.IsNullOrEmpty(password) &&
                !string.IsNullOrEmpty(newPassword))
            {
                mu.ChangePassword(password, newPassword);
            }

			//update additional profile properties, if any
			ProfileBase p = ProfileBase.Create(mu.UserName);
			if (p == null)
			{
				return;
			}
			p.SetPropertyValue("FirstName", FirstName.Text);
			p.SetPropertyValue("LastName", LastName.Text);
			p.SetPropertyValue("CommonName", CommonName.Text);
            p.SetPropertyValue("ThemePreference", ThemePreference.SelectedValue);
			p.Save();

			//if the user executing this page is the user that is being modified, reset the site map.
			if (Profile.UserName == mu.UserName)
			{
				ResetSiteMap();
			}
            ResultMessage1.ShowSuccess("User details were successfully updated" + (!allowed ? ", <span class=\"validationError\">except for role memberships. You can't change your own roles -- you must ask another administrator to do that for you.</span>" : "."));
        }
        catch (Exception ex)
        {
            ResultMessage1.ShowFail("Failed to update user details. ", ex);
        }
    }

    public void AddUser(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            return;
        }

        MembershipCreateStatus createStatus = MembershipCreateStatus.Success;

        string userIDText = UserID.Text;
        string emailText = Email.Text;
        bool isActive = ActiveUser.Checked;
        string password = Password.Text;
        string question = "";
        string answer = "";
        if (Membership.RequiresQuestionAndAnswer)
        {
            question = SecretQuestion.Text;
            answer = SecretAnswer.Text;
        }

        try
        {
            MembershipUser mu = null;

            if (Membership.RequiresQuestionAndAnswer)
            {
                mu = Membership.CreateUser(userIDText, password, emailText, question, answer, isActive, out createStatus);
            }
            else
            {
                mu = Membership.CreateUser(userIDText, password, emailText);
            }

			if (createStatus == MembershipCreateStatus.Success &&
				(mu != null && !string.IsNullOrEmpty(mu.UserName)))
			{
				new WebEvents.CreateUserSuccessEvent(this, userIDText).Raise();
				UpdateRoleMembership(mu.UserName);
			}
			else
			{
				throw new Exception("Unknown failure occurred while creating user.");
			}

			//update additional profile properties, if any
			ProfileBase p = ProfileBase.Create(mu.UserName);
			if (p == null)
			{
				return;
			}
			p.SetPropertyValue("FirstName", FirstName.Text);
			p.SetPropertyValue("LastName", LastName.Text);
			p.SetPropertyValue("CommonName", CommonName.Text);
            p.SetPropertyValue("ThemePreference", ThemePreference.SelectedValue);
			p.Save();

            SaveButton.Enabled = false;

            ResultMessage1.ShowSuccess("User has been successfully created.");
        }
        catch (Exception ex)
        {
            ResultMessage1.ShowFail("Failed to create new user. ", ex);
			SaveButton.Enabled = false;
		}
    }

    public bool IsUserInRole(string roleName)
    {
        //this method is called from the aspx page on databind.
		return SiteUtility.IsUserInRole(userName, roleName);  
    }

	protected void ResetPassword_Click(object sender, EventArgs e)
	{
		try
		{
			MembershipUser mu = Membership.GetUser(UserID.Text);
			mu.ResetPassword();
			ResultMessage1.ShowSuccess("User password has been reset. User will receive an email with the new password");
		}
		catch(Exception ex)
		{
			ResultMessage1.ShowFail("Could not reset user password. ", ex);
		}
	}
	protected void unlockAccount_Click(object sender, EventArgs e)
	{
		try
		{
			MembershipUser mu = Membership.GetUser(UserID.Text);
			if(mu.UnlockUser())
			{
				Membership.UpdateUser(mu);
				new WebEvents.AccountUnlockedEvent(this, mu.UserName);
				ResultMessage1.ShowSuccess("User account unlocked"); 
			}
			else
			{
				throw new Exception("Could not unlock user account.");
			}
		}
		catch (Exception ex)
		{
			ResultMessage1.ShowFail("Could not unlock user account. ", ex ); 	
		}
	}

	public void EnabledChanged(object sender, EventArgs e)
	{
		CheckBox checkBox = sender as CheckBox;
		if (checkBox == null)
			return;

		try
		{
			ResultMessage1.ShowSuccess(SiteUtility.ToggleUserApprovedStatus(checkBox));
		}
		catch (Exception ex)
		{
			ResultMessage1.ShowFail("Unable to change account approval status. ", ex);
		}
	}


	protected void ResetSiteMap()
	{
		SubSonicSiteMapProvider siteMap = (SubSonicSiteMapProvider)SiteMap.Provider;
		siteMap.Reload();
	}

}
