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
    public partial class FrmCargaLeasingHabitacional : Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        private static CsvColumnResolver<LeasingHabitacional> resolver;
        private AdminCargueLeasingHabitacional cargueLeasingHabitacional = new AdminCargueLeasingHabitacional();

        private readonly NumberFormatInfo format = new NumberFormatInfo
        {
            NumberDecimalSeparator = ","
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var codigos = cargueLeasingHabitacional.GetCodigosDane();

                resolver = new CsvColumnResolver<LeasingHabitacional>();
                resolver.Register(0, lh => lh.IcaConsecutivo);
                resolver.Register(1, lh => lh.FechaRegistro, new DateTimeValueConverter(ConfigurationManager.AppSettings.Get("formatoFechaRegistroLeasingHabitacional"), CultureInfo.InvariantCulture));
                resolver.Register(2, lh => lh.Email);
                resolver.Register(3, lh => lh.Area);
                resolver.Register(4, lh => lh.NombreVendedor);
                resolver.Register(5, lh => lh.Nit);
                resolver.Register(6, lh => lh.DigitoVerificacion);
                resolver.Register(7, lh => lh.NumeroObligacion);
                resolver.Register(8, lh => lh.SucursalRadicacion);
                resolver.Register(9, lh => lh.DireccionDomicilio);
                resolver.Register(10, lh => lh.CiudadDomicilio);
                resolver.Register(11, lh => lh.NumeroTelefonico);
                resolver.Register(12, lh => lh.Correo);
                resolver.Register(13, lh => lh.FechaDesembolso, new DateTimeValueConverter(ConfigurationManager.AppSettings.Get("formatoFechaDesembolsoLeasingHabitacional"), CultureInfo.InvariantCulture));
                resolver.Register(14, lh => lh.MesRetencion);
                resolver.Register(15, lh => lh.AnioEscrituracion);
                resolver.Register(16, lh => lh.NitMunicipio);
                resolver.Register(17, lh => lh.CalidadTributaria);
                resolver.Register(18, lh => lh.ActivoFijo);
                resolver.Register(19, lh => lh.CodigoDaneMunicipio, new ContainsConverter(codigos));
                resolver.Register(20, lh => lh.ValorCompra, new FloatValueConverter(format));
                resolver.Register(21, lh => lh.TarifaRetencion, new FloatValueConverter(format));
                resolver.Register(22, lh => lh.ValorRetencion, new FloatValueConverter(format));
                resolver.Register(23, lh => lh.CiudadUbicacionInmueble);
                resolver.Register(24, lh => lh.MesDesembolso);
                resolver.Register(25, lh => lh.UrlArchivoGenerado);
                resolver.Register(26, lh => lh.RetencionCorrespondiente);
                resolver.Register(27, lh => lh.UrlArchivoCargado); 
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
                var list = new List<LeasingHabitacional>();
                var errors = new List<string>();

                using (var streamReader = new StreamReader(file.InputStream))
                {
                    var separator = new string[] { ConfigurationManager.AppSettings.Get("separadorLeasingHabitacional") };
                    streamReader.ReadLine(); // discard headers
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        var fields = line.Split(separator, StringSplitOptions.None);

                        List<string> lineErrors;
                        var leasingHabitacional = resolver.Load(fields, out lineErrors);

                        if (lineErrors.Count > 0)
                        {
                            errors.AddRange(lineErrors);
                            continue;
                        }

                        if (IsValidToLoad(leasingHabitacional, anio, mes))
                            list.Add(leasingHabitacional);
                    }
                }

                if (errors.Count == 0)
                {
                    var idUsuario = $"{Session["IdUsuario"]}";
                    var id = cargueLeasingHabitacional.CreateTableIfNotExists(anio);
                    cargueLeasingHabitacional.DeleteRows(anio, mes);
                    cargueLeasingHabitacional.InsertData(anio, id, idUsuario, list);
                    MostrarMensaje("Datos cargados existosamente");
                }
                else
                {
                    var st = string.Join("||", errors);
                    //var ur = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/" + $"Archivos/Formatos/{CmbCliente.SelectedValue}/F321_DIC_{TxtVigencia.Text}.txt";
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newWindow", $"download('F321_DIC_{TxtVigencia.Text}.txt','{st}');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newWindow", $"download('cargue_leasing_habitacional.txt', '{st}') ", true);
                    //byte[] bytes = System.IO.File.ReadAllBytes(fileName);
                    //var st = string.Join("||", File.ReadAllLines(fileName));
                    //String file = Convert.ToBase64String(bytes);
                    ////var ur = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/" + $"Archivos/Formatos/{CmbCliente.SelectedValue}/F321_DIC_{TxtVigencia.Text}.txt";
                    ////ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newWindow", $"download('F321_DIC_{TxtVigencia.Text}.txt','{st}');", true);
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newWindow", $"download('F321_DIC_{TxtVigencia.Text}.txt', '{st}') ", true);
                    ////aqui deberia ir la descarga, pero nada de lo que intente funciono. aiuda
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

        private bool IsValidToLoad(LeasingHabitacional leasingHabitacional, int anio, int mes)
        {
            if (leasingHabitacional == null)
            {
                return false;
            }

            var isDateValid = leasingHabitacional.FechaRegistro.Year == anio && leasingHabitacional.FechaRegistro.Month == mes;
            var isRetencionGreaterThanZero = leasingHabitacional.ValorRetencion > 0;
            return isDateValid && isRetencionGreaterThanZero;
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Index.aspx");
        }
    }
}