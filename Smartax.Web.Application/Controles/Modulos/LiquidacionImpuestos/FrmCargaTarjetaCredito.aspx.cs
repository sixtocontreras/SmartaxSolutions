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
    public partial class FrmCargaTarjetaCredito : Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        private static FixedWidthColumnResolver<TarjetaCredito> resolver;
        private AdminCargueTarjetaCredito cargueTarjetaCredito = new AdminCargueTarjetaCredito();
        private readonly NumberFormatInfo format = new NumberFormatInfo
        {
            NumberDecimalSeparator = "."
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var codigos = cargueTarjetaCredito.GetCodigosDane();

                resolver = new FixedWidthColumnResolver<TarjetaCredito>();
                resolver.Register(0, 4, tc => tc.Tipo);
                resolver.Register(4, 7, tc => tc.Impuesto);
                resolver.Register(13, 7, tc => tc.CodCiu); // new ContainsConverter(codigos));
                resolver.Register(20, 14, tc => tc.Ciudad);
                resolver.Register(34, 27, tc => tc.Nit);
                resolver.Register(61, 2, tc => tc.Tm);
                resolver.Register(63, 19, tc => tc.Marca);
                resolver.Register(82, 8, tc => tc.FechaInicial, new DateTimeValueConverter(ConfigurationManager.AppSettings.Get("formatoFechaTarjetaCredito"), CultureInfo.InvariantCulture));
                resolver.Register(93, 8, tc => tc.FechaFinal, new DateTimeValueConverter(ConfigurationManager.AppSettings.Get("formatoFechaTarjetaCredito"), CultureInfo.InvariantCulture));
                resolver.Register(104, 15, tc => tc.ValorVenta, new FloatValueConverter(format));
                resolver.Register(119, 38, tc => tc.Establecimiento);
                resolver.Register(157, 15, tc => tc.ValorImpuesto, new FloatValueConverter(format));
                resolver.Register(172, 13, tc => tc.ValorBase, new FloatValueConverter(format));
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
                var list = new List<TarjetaCredito>();
                var errors = new List<string>();

                using (var streamReader = new StreamReader(file.InputStream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        List<string> lineErrors;
                        var tarjetaCredito = resolver.Load(line, out lineErrors);

                        if (lineErrors.Count > 0)
                        {
                            errors.AddRange(lineErrors);
                            continue;
                        }

                        if (IsValidToLoad(tarjetaCredito, anio, mes))
                            list.Add(tarjetaCredito);
                    }
                }

                if (errors.Count == 0)
                {
                    var idUsuario = $"{Session["IdUsuario"]}";
                    var id = cargueTarjetaCredito.CreateTableIfNotExists(anio);
                    cargueTarjetaCredito.DeleteRows(anio, mes);
                    cargueTarjetaCredito.InsertData(anio, id, idUsuario, list);
                    MostrarMensaje("Datos cargados existosamente");
                }
                else
                {
                    var st = string.Join("||", errors);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newWindow", $"download('cargue_tarjeta_credito.txt', '{st}') ", true);
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

        private bool IsValidToLoad(TarjetaCredito tarjetaCredito, int anio, int mes)
        {
            if (tarjetaCredito == null)
            {
                return false;
            }

            var isFechaValid = tarjetaCredito.FechaFinal.Year == anio && tarjetaCredito.FechaFinal.Month == mes;
            var isImpuestoValid = tarjetaCredito.Impuesto == "RETEICA";
            return isFechaValid && isImpuestoValid;
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Index.aspx");
        }
    }
}