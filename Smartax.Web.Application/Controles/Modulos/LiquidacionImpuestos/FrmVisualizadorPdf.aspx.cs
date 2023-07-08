using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmVisualizadorPdf : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                try
                {

                    this.ViewState["TipoLiquidacion"] = Request.QueryString["TipoLiquidacion"].ToString().Trim();
                    this.ViewState["TipoImpuesto"] = Request.QueryString["TipoImpuesto"].ToString().Trim();
                    //--RUTA DEL ARCHIVO PDF
                    string _RutaVirtual = "";
                    if (FixedData.AmbienteSistema.ToString().Trim().Equals("PRODUCCION"))
                    {
                        _RutaVirtual = HttpContext.Current.Server.MapPath("/" + FixedData.DirectorioVirtual.ToString().Trim() + "\\");
                    }
                    else
                    {
                        string _TipoEjecucion = Request.QueryString["TipoEjecucion"] != null ? Request.QueryString["TipoEjecucion"].ToString().Trim() : "";
                        if (_TipoEjecucion.Equals("POR_LOTE"))
                        {
                            _RutaVirtual = FixedData.DirectorioArchivosApi;
                        }
                        else
                        {
                            _RutaVirtual = HttpContext.Current.Server.MapPath("/" + FixedData.DirectorioVirtual.ToString().Trim() + "\\");
                        }
                        _log.Warn("DIRECTORIO VALIDAR PDF => " + _RutaVirtual);
                    }

                    string _PathDirectorio = _RutaVirtual + "\\" + FixedData.DirectorioArchivos.ToString().Trim() + "\\" + DateTime.Now.ToString("yyyy") + "\\" + this.ViewState["TipoImpuesto"].ToString().Trim() + "\\" + this.ViewState["TipoLiquidacion"].ToString().Trim() + "\\" + "CLIENTE_" + this.Session["IdCliente"].ToString().Trim();
                    string FileName = Request.QueryString["NombreFile"].ToString().Trim();
                    // Indicamos donde vamos a guardar el documento
                    string _PathFile = _PathDirectorio + "\\" + FileName;

                    //Borra todas las salidas del flujo de memoria
                    Response.ClearContent();
                    //Borra todos los encabezados del flujo de memoria
                    Response.ClearHeaders();
                    //Añade una cabecera HTTP al flujo de salida
                    Response.AddHeader("Content-Disposition", "inline;filename=" + _PathFile);
                    //Obtiene o establece el tipo MIME de HTTP del flujo de salida
                    Response.ContentType = "application/pdf";
                    //Escribe el contenido del archivo especificado en un flujo de salida de respuesta HTTP como un bloque de archivos
                    Response.WriteFile(_PathFile);
                    //envía toda la salida del buffer al cliente
                    Response.Flush();
                    //Borra todas las salidas de contenidos del flujo de memoria
                    Response.Clear();
                }
                catch (Exception ex)
                {
                    _log.Error("Error al mostrar el PDF. Motivo: " + ex.Message);
                }
            }
        }
    }
}