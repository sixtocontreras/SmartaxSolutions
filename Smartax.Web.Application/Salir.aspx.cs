using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smartax.Web.Application
{
    public partial class Salir : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                Session.Contents.RemoveAll();
                Session.Clear();
                Session.Abandon();
                ViewState.Clear();
                Response.Redirect("/Default.aspx");
            }
        }
    }
}