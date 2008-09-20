using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CMS {
    public partial class Page {

        //used for sorting the lists
        public int Level = 0;
        private bool _IsFourOFour = false;

		public bool IsFourOFour
		{
			get { return _IsFourOFour; }
			set { _IsFourOFour = value; }
		}

        public string FinalUrl
        {
            get { return (PageTypeID == 0 ? "~/view/" : "") + PageUrl; }
        }

    }
}