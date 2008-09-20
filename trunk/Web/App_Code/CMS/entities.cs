using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using AjaxControlToolkit;
using System.Web.Script.Services;
using System.Xml;

namespace CMS
{
	/// <summary>
	/// Summary description for entities
	/// </summary>
	[WebService(Namespace = "http://localhost/SubSonicCMS/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	[System.Web.Script.Services.ScriptService()]
	public class entities : System.Web.Services.WebService
	{

		public entities()
		{
			//
		}

		[WebMethod]
		[ScriptMethod]
		public string[] GetSearchCompletionList(string prefixText, int count)
		{
			StringCollection list = (StringCollection)HttpContext.Current.Profile["SearchTerms"];
			List<string> suggestions = new List<string>();
			foreach (string s in list)
			{
				if (s.StartsWith(prefixText))
				{
					suggestions.Add(s);
				}
			}
			return suggestions.ToArray();
		}

	}
}
