using System;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmVerImpuesto : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.ViewState["IdMunicipio"] = Request.QueryString["IdMunicipio"].ToString().Trim();
                int _IdFormImpuesto = Int32.Parse(Request.QueryString["IdFormImpuesto"].ToString().Trim());
                this.ViewState["CodigoDane"] = Request.QueryString["CodigoDane"].ToString().Trim();
                this.ViewState["CodigoPeriodicidad"] = Request.QueryString["CodigoPeriodicidad"] != null ? Request.QueryString["CodigoPeriodicidad"].ToString().Trim() : "";
                this.ViewState["TipoLiquidacion"] = Request.QueryString["TipoLiquidacion"].ToString().Trim();
                this.ViewState["TipoImpuesto"] = Request.QueryString["TipoImpuesto"].ToString().Trim();
                this.ViewState["TipoEjecucion"] = Request.QueryString["TipoEjecucion"] != null ? Request.QueryString["TipoEjecucion"].ToString().Trim() : "";

                //--AQUI VALIDAMOS EL TIPO DE IMPUESTO 1. ICA, 2. AUTO ICA
                //string _NombreFile = "";
                //if (_IdFormImpuesto == 1)
                //{
                //string _NombreFile = "FORMULARIO_" + this.ViewState["CodigoDane"].ToString().Trim() + "_" + this.ViewState["IdMunicipio"].ToString().Trim() + ".pdf";
                string _NombreFile = "FORMULARIO_" + this.ViewState["CodigoDane"].ToString().Trim() + "_" + this.ViewState["IdMunicipio"].ToString().Trim() + "_" + this.ViewState["CodigoPeriodicidad"].ToString().Trim() + ".pdf";
                //}
                //else
                //{
                //    string _NombreFile = "FORMULARIO_" + this.ViewState["CodigoDane"].ToString().Trim() + "_" + this.ViewState["IdMunicipio"].ToString().Trim() + "_" + this.ViewState["CodigoPeriodicidad"].ToString().Trim() + ".pdf";
                //}

                MyIframe.Attributes["src"] = "/Controles/Modulos/LiquidacionImpuestos/FrmVisualizadorPdf.aspx?NombreFile=" + _NombreFile +
                    "&TipoImpuesto=" + this.ViewState["TipoImpuesto"].ToString().Trim() +
                    "&TipoLiquidacion=" + this.ViewState["TipoLiquidacion"].ToString().Trim() +
                    "&TipoEjecucion=" + this.ViewState["TipoEjecucion"].ToString().Trim();
            }
        }
    }
}