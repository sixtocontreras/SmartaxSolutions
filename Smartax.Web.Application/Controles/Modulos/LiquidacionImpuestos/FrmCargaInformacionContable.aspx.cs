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
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmCargaInformacionContable : Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        private static CsvColumnResolver<InformacionContable> resolver;
        private AdminCargueInfoContable cargueInfoContable = new AdminCargueInfoContable();
        private readonly NumberFormatInfo format = new NumberFormatInfo
        {
            NumberDecimalSeparator = "."
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                resolver = new CsvColumnResolver<InformacionContable>();
                resolver.Register(0, ic => ic.Un);
                resolver.Register(1, ic => ic.GLibros);
                resolver.Register(2, ic => ic.Libro);
                resolver.Register(3, ic => ic.Cuenta);
                resolver.Register(4, ic => ic.Sucursal);
                resolver.Register(5, ic => ic.Dependencia);
                resolver.Register(6, ic => ic.IdAsiento);
                resolver.Register(7, ic => ic.FechaComprobante, new DateTimeValueConverter(ConfigurationManager.AppSettings.Get("formatoFechaInfoContable"), CultureInfo.InvariantCulture));
                resolver.Register(8, ic => ic.FechaProceso, new DateTimeValueConverter(ConfigurationManager.AppSettings.Get("formatoFechaInfoContable"), CultureInfo.InvariantCulture));
                resolver.Register(9, ic => ic.Descripcion);
                resolver.Register(10, ic => ic.Debito, new FloatValueConverter(format));
                resolver.Register(11, ic => ic.Credito, new FloatValueConverter(format));
                resolver.Register(12, ic => ic.Auxiliar);
                resolver.Register(13, ic => ic.Referencia);
                resolver.Register(14, ic => ic.Usuario);
                resolver.Register(15, ic => ic.IdComprobante);
                resolver.Register(16, ic => ic.Estado);
                resolver.Register(17, ic => ic.Real); 
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
                var list = new List<InformacionContable>();
                var errors= new List<string>();

                using (var streamReader = new StreamReader(file.InputStream))
                {
                    var separator = new string[] { ConfigurationManager.AppSettings.Get("separadorInfoContable") };
                    streamReader.ReadLine(); // discard headers
                    while (!streamReader.EndOfStream)
                    {
                        List<string> lineErrors;
                        var line = streamReader.ReadLine();
                        var fields = line.Split(separator, StringSplitOptions.None);
                        var infoContable = resolver.Load(fields, out lineErrors);

                        if (lineErrors.Count > 0)
                        {
                            errors.AddRange(lineErrors);
                            continue;
                        }
                        
                        if (IsValidToLoad(infoContable, anio, mes))
                            list.Add(infoContable);
                    }
                }

                if (errors.Count == 0)
                {
                    var idUsuario = $"{Session["IdUsuario"]}";
                    var id = cargueInfoContable.CreateTableIfNotExists(anio);
                    cargueInfoContable.DeleteRows(anio, mes);
                    cargueInfoContable.InsertData(idUsuario, anio, id, list);
                    cargueInfoContable.DepuraRows_OD(anio, mes);
                    cargueInfoContable.DepuraRows_PG_TC(anio, mes);
                    cargueInfoContable.DepuraRows_PC_TC(anio, mes);
                    MostrarMensaje("Datos cargados existosamente");
                }
                else
                {
                    var st = string.Join("||", errors);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newWindow", $"download('cargue_informacion_contable.txt', '{st}') ", true);
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

        private bool IsValidToLoad(InformacionContable informacionContable, int anio, int mes)
        {
            if (informacionContable == null)
            {
                return false;
            }

            
            var isInDateRange = informacionContable.FechaComprobante.Year == anio && informacionContable.FechaComprobante.Month == mes;
            var isBifrs = informacionContable.GLibros == "BIFRS" && informacionContable.Libro == "BIFRS";

            return isInDateRange && isBifrs;
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Index.aspx");
        }
    }
}