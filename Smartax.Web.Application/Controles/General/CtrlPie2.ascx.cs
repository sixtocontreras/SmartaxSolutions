using Smartax.Web.Application.Clases.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Smartax.Web.Application.Controles.General
{
    public partial class CtrlPie2 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //--Aquyi mandamos a imprimir los datos en el Pie de la pagina.
                DateTime FechaActual = DateTime.Now;
                string AnnoActual = Convert.ToString(FechaActual.ToString("yyyy"));
                this.LblVersion.Text = FixedData.NameEmpresa + " - " + FixedData.PlatformName + " " + FixedData.PlatformVersion;
                this.LblAnioActual.Text = AnnoActual.ToString().Trim() + " " + Session["NameEmpresa"];
            }
        }

    }
}