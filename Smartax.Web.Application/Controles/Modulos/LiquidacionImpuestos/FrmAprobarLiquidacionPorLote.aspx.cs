using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using log4net;
using System.Text;
using System.Data;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Seguridad;
using System.Web.Caching;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmAprobarLiquidacionPorLote : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        EjecucionXLoteFiltros ObjValidacion = new EjecucionXLoteFiltros();
        EnvioCorreo ObjCorreo = new EnvioCorreo();
        Utilidades ObjUtils = new Utilidades();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.ViewState["IdLiquidacionLote"] = Request.QueryString["IdLiquidacionLote"].ToString().Trim();
                this.LblAnioGravable.Text = Request.QueryString["AnioGravable"].ToString().Trim();
                this.LblEstadoLiquidacion.Text = Request.QueryString["EstadoLiquidacion"].ToString().Trim();
                this.LblTipoImpuesto.Text = Request.QueryString["TipoImpuesto"].ToString().Trim();
                this.LblPeriodicidad.Text = Request.QueryString["Periodicidad"].ToString().Trim();
                this.LblDepartamento.Text = Request.QueryString["NombreDpto"].ToString().Trim();
                this.LblCodDane.Text = Request.QueryString["CodigoDane"].ToString().Trim();
                this.LblMunicipio.Text = Request.QueryString["NombreMunicipio"].ToString().Trim();
                this.LblAnalista.Text = Request.QueryString["NombreAnalista"].ToString().Trim();
                this.ViewState["EmailAnalista"] = Request.QueryString["EmailAnalista"].ToString().Trim();
                this.TxtEmailsNotificacion.Text = this.ViewState["EmailAnalista"].ToString().Trim();
                this.ViewState["FechaLimite"] = Request.QueryString["FechaLimite"].ToString().Trim();

                #region GENERAR TABLA DE DATOS PARA HTM
                //--GENERAR TABLA DE DATOS
                DataTable dtDatosMostrar = new DataTable();
                dtDatosMostrar = this.GetTablaDatos();
                DataRow Fila = null;
                Fila = dtDatosMostrar.NewRow();
                Fila["codigo_dane"] = this.LblCodDane.Text.ToString().Trim();
                Fila["departamento"] = this.LblDepartamento.Text.ToString().Trim();
                Fila["municipio"] = this.LblMunicipio.Text.ToString().Trim();
                Fila["periodicidad"] = this.LblPeriodicidad.Text.ToString().Trim();
                Fila["fecha_vencimiento"] = this.ViewState["FechaLimite"].ToString().Trim();
                Fila["nombre_usuario"] = this.LblAnalista.Text.ToString().Trim();
                dtDatosMostrar.Rows.Add(Fila);
                this.ViewState["dtDatosMostrar"] = dtDatosMostrar;
                #endregion

                this.RbEstadoAprobacion.Focus();
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

        private DataTable GetTablaDatos()
        {
            DataTable DtLiquidaciones = new DataTable();
            try
            {
                #region DEFINIR COLUMNAS DEL DATATABLE
                //--DEFINIR COLUMNAS
                DtLiquidaciones = new DataTable();
                DtLiquidaciones.TableName = "DtLiquidaciones";
                //DtLiquidacionIca.Columns.Add("id_registro", typeof(Int32));
                //DtLiquidacionIca.PrimaryKey = new DataColumn[] { DtLiquidacionIca.Columns["idliquid_impuesto"] };
                DtLiquidaciones.Columns.Add("codigo_dane");
                DtLiquidaciones.Columns.Add("departamento");
                DtLiquidaciones.Columns.Add("municipio");
                DtLiquidaciones.Columns.Add("periodicidad");
                DtLiquidaciones.Columns.Add("fecha_vencimiento");
                DtLiquidaciones.Columns.Add("nombre_usuario");
                #endregion
            }
            catch (Exception ex)
            {
                DtLiquidaciones = null;
                _log.Error("Error al generar el datatable de datos. Motivo: " + ex.Message);
            }

            return DtLiquidaciones;
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ObjValidacion.IdEjecucionLote = this.ViewState["IdLiquidacionLote"].ToString().Trim();
                ObjValidacion.ArrayData = null;
                ObjValidacion.AprobacionJefe = this.RbEstadoAprobacion.SelectedValue.ToString().Trim().Equals("1") ? "S" : "N";
                ObjValidacion.Observacion = ObjUtils.GetLimpiarCadena(this.TxtObservacion.Text.ToString().Trim().ToUpper());
                ObjValidacion.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjValidacion.TipoProceso = 2;
                ObjValidacion.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

                int _IdRegistro = 0;
                string _MsgError = "";
                if (ObjValidacion.GetProcesarLiquidacionLote(ref _IdRegistro, ref _MsgError))
                {
                    #region DEFINICION DEL METODO PARA ENVIO DE CORREO
                    //--Definir valores para envio del email
                    ObjCorreo.StrServerCorreo = FixedData.ServerCorreoGmail.ToString().Trim();
                    ObjCorreo.PuertoCorreo = FixedData.PuertoCorreoGmail;
                    ObjCorreo.StrEmailDe = FixedData.UsuarioEmail.ToString().Trim();
                    ObjCorreo.StrPasswordDe = FixedData.PasswordEmail.ToString().Trim();
                    string[] _ListaEmails = this.TxtEmailsNotificacion.Text.ToString().Trim().Split(';');
                    if (_ListaEmails.Length > 1)
                    {
                        ObjCorreo.StrEmailPara = this.ViewState["EmailAnalista"].ToString().Trim();
                        ObjCorreo.StrEmailCopia = this.TxtEmailsNotificacion.Text.ToString().Trim();
                    }
                    else
                    {
                        ObjCorreo.StrEmailPara = this.ViewState["EmailAnalista"].ToString().Trim();
                        ObjCorreo.StrEmailCopia = "";
                    }
                    ObjCorreo.StrAsunto = "REF.: VALIDACION LIQUIDACION x LOTE";

                    string nHora = DateTime.Now.ToString("HH");
                    string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                    StringBuilder strDetalleEmail = new StringBuilder();
                    string _TituloTablaHtml = "LISTA DE LIQUIDACIONES DE ICA PROCESADAS";

                    DataTable dtDatosMostrar = new DataTable();
                    dtDatosMostrar = (DataTable)this.ViewState["dtDatosMostrar"];
                    string _TableHtml = GetTableHtml(_TituloTablaHtml, dtDatosMostrar);

                    //strDetalleEmail.Append("<h4>" + strTime + ", Señor " + this.LblAnalista.Text.ToString().Trim() + ", el proceso de revisión a la ejecución masiva de las declaraciones del tipo de impuesto " + this.LblTipoImpuesto.Text.ToString().Trim() + " ha finalizado para los siguientes municipios." + "</h4>" + "<br/>");
                    if (ObjValidacion.AprobacionJefe.Equals("S"))
                    {
                        strDetalleEmail.Append("<h4>" + strTime + ", Señor " + this.LblAnalista.Text.ToString().Trim() + ", el proceso de validación del impuesto de " + this.LblTipoImpuesto.Text.ToString().Trim() + " ha finalizado para el siguiente municipio." + "</h4>" + "<br/>");
                    }
                    else
                    {
                        strDetalleEmail.Append("<h4>" + strTime + ", Señor " + this.LblAnalista.Text.ToString().Trim() + ", para que por favor se haga una revisión a la liquidación del impuesto de " + this.LblTipoImpuesto.Text.ToString().Trim() + " del siguiente municipio." + "</h4>" + "<br/>");
                    }

                    strDetalleEmail.Append("<h4>" + "Observación: " + this.TxtObservacion.Text.ToString().Trim() + "</h4>" + "<br/><br/>");
                    strDetalleEmail.Append(_TableHtml.ToString() + "<br/><br/><br/>");

                    ObjCorreo.StrDetalle = strDetalleEmail.ToString().Trim();
                    string _MsgErrorEmail = "";
                    if (!ObjCorreo.SendEmailConCopia(ref _MsgErrorEmail))
                    {
                        _MsgError = _MsgError + " Pero " + _MsgErrorEmail.ToString().Trim();
                    }
                    #endregion

                    #region MOSTRAR MENSAJE DE USUARIO
                    this.UpdatePanel1.Update();
                    this.RbEstadoAprobacion.Enabled = false;
                    this.TxtEmailsNotificacion.Enabled = false;
                    this.TxtObservacion.Enabled = false;
                    this.BtnGuardar.Enabled = false;
                    //MOSTRAR LA VENTANA DEL POPUP
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(300);
                    Ventana.Width = Unit.Pixel(600);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Mensaje del Sistema";
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    #endregion
                }
                else
                {
                    #region MOSTRAR MENSAJE DE USUARIO
                    this.UpdatePanel1.Update();
                    //MOSTRAR LA VENTANA DEL POPUP
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(300);
                    Ventana.Width = Unit.Pixel(600);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Mensaje del Sistema";
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                this.UpdatePanel1.Update();
                //MOSTRAR LA VENTANA DEL POPUP
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Error con el evento ItemCommand de la grilla. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                #endregion
            }
        }

        public static string GetTableHtml(string _TituloTablaHtml, DataTable DtDatos)
        {
            StringBuilder TableHtml = new StringBuilder();
            try
            {
                //Table start.
                TableHtml.Append("<table border = '1'>");
                TableHtml.Append("<tr align='center' valign='middle' >");
                TableHtml.Append("<th colspan=" + DtDatos.Columns.Count + "> " + _TituloTablaHtml + " </th> ");
                TableHtml.Append("</tr>");

                //Building the Header row.
                TableHtml.Append("<tr>");
                foreach (DataColumn column in DtDatos.Columns)
                {
                    TableHtml.Append("<th>");
                    TableHtml.Append(column.ColumnName.ToString().ToUpper());
                    TableHtml.Append("</th>");
                }
                TableHtml.Append("</tr>");

                //Building the Data rows.
                foreach (DataRow row in DtDatos.Rows)
                {
                    TableHtml.Append("<tr>");
                    foreach (DataColumn column in DtDatos.Columns)
                    {
                        TableHtml.Append("<td>");
                        TableHtml.Append(row[column.ColumnName]);
                        TableHtml.Append("</td>");
                    }
                    TableHtml.Append("</tr>");
                }

                //Table end.
                TableHtml.Append("</table>");

            }
            catch (Exception ex)
            {
                TableHtml.Append("");
                _log.Error("Error al obtener la Tabla Html. Motivo: " + ex.Message);
            }

            return TableHtml.ToString();
        }

    }
}