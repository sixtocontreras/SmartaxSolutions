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
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmCargaLeasingFinanciero : Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        private static CsvColumnResolver<LeasingFinanciero> resolver;
        private AdminCargueLeasingFinanciero cargueLeasingFinanciero = new AdminCargueLeasingFinanciero();
        private readonly NumberFormatInfo format = new NumberFormatInfo
        {
            NumberDecimalSeparator = "."
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var codigosMunicipio = cargueLeasingFinanciero.GetCodigosDane(CodigoDane.Municipio);
                var codigosDepartamento = cargueLeasingFinanciero.GetCodigosDane(CodigoDane.Departamento);

                resolver = new CsvColumnResolver<LeasingFinanciero>();
                resolver.Register(0, lf => lf.TipoIdent);
                resolver.Register(1, lf => lf.NumeroIdent);
                resolver.Register(2, lf => lf.TipoImp);
                resolver.Register(3, lf => lf.SubtipoImp);
                resolver.Register(4, lf => lf.Tarifa, new FloatValueConverter(format));
                resolver.Register(5, lf => lf.Valor, new FloatValueConverter(format));
                resolver.Register(6, lf => lf.NitTeso);
                resolver.Register(7, lf => lf.NomTeso);
                resolver.Register(8, lf => lf.NumCue);
                resolver.Register(9, lf => lf.Naturaleza);
                resolver.Register(10, lf => lf.TipoCompro);
                resolver.Register(11, lf => lf.NumeroCompro);
                resolver.Register(12, lf => lf.NombreTercero);
                resolver.Register(13, lf => lf.Base, new FloatValueConverter(format));
                resolver.Register(14, lf => lf.FechaRegistro, new DateTimeValueConverter(ConfigurationManager.AppSettings.Get("formatoFechaLeasingFinanciero"), CultureInfo.InvariantCulture));
                resolver.Register(15, lf => lf.DireccionTercero);
                resolver.Register(16, lf => lf.CorreoElectronico);
                resolver.Register(17, lf => lf.DepartamentoTercero); // new ContainsConverter(codigosDepartamento));
                resolver.Register(18, lf => lf.CiudadTercero); // new ContainsConverter(codigosMunicipio));
                resolver.Register(19, lf => lf.PaisTercero); //pendiente
                resolver.Register(20, lf => lf.TelefonoTercero); 
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
                var list = new List<LeasingFinanciero>();
                var errors = new List<string>();

                using (var streamReader = new StreamReader(file.InputStream))
                {
                    var separator = new string[] { ConfigurationManager.AppSettings.Get("separadorLeasingFinanciero") };
                    streamReader.ReadLine(); // discard headers
                    while (!streamReader.EndOfStream)
                    {
                        List<string> lineErrors;
                        var line = streamReader.ReadLine();
                        var fields = line.Split(separator, StringSplitOptions.None);
                        var leasingFinanciero = resolver.Load(fields, out lineErrors);


                        if (lineErrors.Count > 0)
                        {
                            errors.AddRange(lineErrors);
                            continue;
                        }
                        
                        if (IsValidToLoad(leasingFinanciero, anio, mes))
                            list.Add(leasingFinanciero);
                    }
                }

                if (errors.Count == 0)
                {
                    var idUsuario = $"{Session["IdUsuario"]}";
                    var id = cargueLeasingFinanciero.CreateTableIfNotExists(anio);
                    cargueLeasingFinanciero.DeleteRows(anio, mes);
                    cargueLeasingFinanciero.InsertData(anio, id, idUsuario, list);
                    MostrarMensaje("Datos cargados existosamente");
                }
                else
                {
                    var st = string.Join("||", errors);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newWindow", $"download('cargue_leasing_financiero.txt', '{st}') ", true);
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

        private bool IsValidToLoad(LeasingFinanciero leasingFinanciero, int anio, int mes)
        {
            if (leasingFinanciero == null)
            {
                return false;
            }

            var isDateValid = leasingFinanciero.FechaRegistro.Year == anio && leasingFinanciero.FechaRegistro.Month == mes; 
            //var isTipoImpuestoValid = leasingFinanciero.TipoImp == "RETEICA";
            return isDateValid;
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Index.aspx");
        }
    }
}