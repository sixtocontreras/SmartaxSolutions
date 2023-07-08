using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smartax.Web.Application.Controles.Seguridad
{
    public partial class FrmVerDetalleAuditoria : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.LblMensaje.Text = Request.QueryString["DescripcionEvento"].ToString().Trim();
            }
        }
    }
}