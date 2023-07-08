using log4net;
using Smartax.Web.Application.Clases.Modulos;
using Smartax.Web.Application.Clases.Modulos.Cargue;
using Smartax.Web.Application.Clases.Modulos.Cargue.Converters;
using Smartax.Web.Application.Clases.Seguridad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmCargaPagaduria : Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        private static CsvColumnResolver<Pagaduria> resolver;
        private AdminCarguePagaduria carguePagaduria = new AdminCarguePagaduria();

        private readonly NumberFormatInfo format = new NumberFormatInfo
        {
            NumberDecimalSeparator = "."
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var codigos = carguePagaduria.GetCodigosDane();

                resolver = new CsvColumnResolver<Pagaduria>();

                resolver.Register(0, pag => pag.UnidadNegocio);
                resolver.Register(1, pag => pag.IdComprobante);
                resolver.Register(2, pag => pag.LineaComprobante, new IntegerValueConverter(format));
                resolver.Register(3, pag => pag.IdSetProveedor);
                resolver.Register(4, pag => pag.IdProveedor);
                resolver.Register(5, pag => pag.NumeroIdentificacion);
                resolver.Register(6, pag => pag.FechaContable, new DateTimeValueConverter(ConfigurationManager.AppSettings.Get("formatoFechaPagaduria"), CultureInfo.InvariantCulture));
                resolver.Register(7, pag => pag.TipoImpuesto);
                resolver.Register(8, pag => pag.CodigoActividad); // new ContainsConverter(codigos));
                resolver.Register(9, pag => pag.ImporteBaseRetencion, new FloatValueConverter(format));
                resolver.Register(10, pag => pag.BaseRetencionMonedaBase, new FloatValueConverter(format));
                resolver.Register(11, pag => pag.ImporteRetencion, new FloatValueConverter(format));
                resolver.Register(12, pag => pag.ImporteRetencionMonedaBase, new FloatValueConverter(format));
                resolver.Register(13, pag => pag.PorcentajeRetencion, new FloatValueConverter(format));
                resolver.Register(14, pag => pag.MonedaTransaccion);
                resolver.Register(15, pag => pag.MonedaBase);
            }
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                if (upl.UploadedFiles.Count < 1)
                {
                    return;
                }

                RadWindowManager1.Windows.Clear();
                var anio = int.Parse(txtAnio.Text);
                var mes = int.Parse(txtMes.Text);
                var file = upl.UploadedFiles[0];
                var list = new List<Pagaduria>();
                var errors = new List<string>();

                using (var streamReader = new StreamReader(file.InputStream))
                {
                    var separator = new string[] { ConfigurationManager.AppSettings.Get("separadorPagaduria") };
                    streamReader.ReadLine(); // discard headers
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        var fields = line.Split(separator, StringSplitOptions.None);

                        var lst = new List<string>();
                        var pagaduria = resolver.Load(fields, out lst);

                        if (lst.Count > 0)
                        {
                            errors.AddRange(lst);
                            continue;
                        }

                        if (IsValidToLoad(pagaduria, anio, mes))
                            list.Add(pagaduria);
                    }
                }

                if (errors.Count == 0)
                {
                    var idUsuario = $"{Session["IdUsuario"]}";
                    var id = carguePagaduria.CreateTableIfNotExists(anio);
                    carguePagaduria.DeleteRows(anio, mes);
                    carguePagaduria.InsertData(anio, id, idUsuario, list);
                    MostrarMensaje("Datos cargados existosamente");
                }
                else
                {
                    var st = string.Join("||", errors);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newWindow", $"download('cargue_pagaduria.txt', '{st}') ", true);
                    MostrarMensaje("Archivo con errores.");
                }
            }
            catch (Exception ex)
            {
                string _MsgError = "Error al cargar el archivo. Motivo: " + ex.ToString();
                MostrarMensaje(_MsgError);
                _log.Error(_MsgError);
            }
        }

        private void MostrarMensaje(string msg)
        {
            #region MOSTRAR MENSAJE DE USUARIO
            //Mostramos el mensaje porque se produjo un error con la Trx.
            this.RadWindowManager1.ReloadOnShow = true;
            this.RadWindowManager1.DestroyOnClose = true;
            this.RadWindowManager1.Windows.Clear();
            this.RadWindowManager1.Enabled = true;
            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            this.RadWindowManager1.Visible = true;

            RadWindow Ventana = new RadWindow
            {
                Modal = true,
                NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + msg,
                ID = "RadWindow2",
                VisibleOnPageLoad = true,
                Visible = true,
                Height = Unit.Pixel(250),
                Width = Unit.Pixel(500),
                KeepInScreenBounds = true,
                Title = "Mensaje del Sistema",
                VisibleStatusbar = false,
                Behaviors = WindowBehaviors.Close
            };
            this.RadWindowManager1.Windows.Add(Ventana);
            this.RadWindowManager1 = null;
            Ventana = null;

            #endregion
        }

        private bool IsValidToLoad(Pagaduria pagaduria, int anio, int mes)
        {
            if (pagaduria == null)
            {
                return false;
            }

            var isValidDate = pagaduria.FechaContable.Year == anio && pagaduria.FechaContable.Month == mes;
            var isValidUnidadNegocio = pagaduria.UnidadNegocio == "APDAV";
            var isValidTipoImpuesto = pagaduria.TipoImpuesto == "RFICA";
            return isValidDate && isValidUnidadNegocio && isValidTipoImpuesto;
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Index.aspx");
        }
    }
}