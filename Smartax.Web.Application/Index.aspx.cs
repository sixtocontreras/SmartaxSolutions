using System;
using System.Web.Caching;

namespace Smartax.Web.Application
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(this.Session["Login"]) == "")
            {
                Response.Redirect("/");
                //if (Request.ServerVariables["SCRIPT_NAME"].ToLower() != "/index.aspx" && Request.ServerVariables["SCRIPT_NAME"] != "/")
                //{
                //    Response.Redirect("/");
                //}
            }
        }

        protected override void SavePageStateToPersistenceMedium(object state)
        {
            string str = string.Format("VS_{0}_{1}", Request.UserHostAddress, DateTime.Now.Ticks);
            Cache.Add(str, state, null, DateTime.Now.AddMinutes(Session.Timeout), TimeSpan.Zero, CacheItemPriority.Default, null);
            ClientScript.RegisterHiddenField("__VIEWSTATE_KEY", str);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            string str = Request.Form["__VIEWSTATE_KEY"];
            if (!str.StartsWith("VS_"))
            {
                throw new Exception("Invalid ViewState");
            }
            return Cache[str];
        }

    }
}