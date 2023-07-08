using System;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using Microsoft.Win32.TaskScheduler;
using System.Web.Script.Serialization;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros;

namespace Smartax.Web.Application.Controles.Seguridad
{
    public partial class FrmAddConfiguracionAlerta : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        ConfiguracionTareas ObjConfiguracion = new ConfiguracionTareas();
        TipoEnvioTareas ObjTipoEnvio = new TipoEnvioTareas();
        TiposProceso ObjTiposProceso = new TiposProceso();
        Combox ObjCmb = new Combox();
        Estado ObjEstado = new Estado();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();

        #region DEFINICION DE LISTAS
        private void GetInfoConfiguracion()
        {
            try
            {
                ObjConfiguracion.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();
                ObjConfiguracion.IdConfiguracionAlerta = Int32.Parse(this.ViewState["IdConfiguracion"].ToString().Trim());
                ObjConfiguracion.TipoProceso = 2;

                DataTable dtDatos = new DataTable();
                dtDatos = ObjConfiguracion.GetAllConfiguracionTareas();

                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        this.CmbTipoTarea.Enabled = false;
                        this.CmbTipoTarea.SelectedValue = dtDatos.Rows[0]["idtipo_tarea"].ToString().Trim();
                        string _IdTipoEnvio = dtDatos.Rows[0]["idtipo_envio"].ToString().Trim();
                        if (_IdTipoEnvio.Equals("1"))   //PARCIALES
                        {
                            this.CmbTipoEnvio.SelectedValue = _IdTipoEnvio;
                            this.CmbHoraFin.Enabled = true;
                            this.CmbIntervalo.Enabled = true;
                        }
                        else
                        {
                            this.CmbTipoEnvio.SelectedValue = _IdTipoEnvio;
                            this.CmbHoraFin.Enabled = false;
                            this.CmbIntervalo.Enabled = false;
                        }

                        this.CmbEstado.SelectedValue = dtDatos.Rows[0]["id_estado"].ToString().Trim();
                        this.dtFechaInicio.SelectedDate = Convert.ToDateTime(dtDatos.Rows[0]["fecha_inicio"].ToString().Trim());
                        this.dtFechaFin.SelectedDate = Convert.ToDateTime(dtDatos.Rows[0]["fecha_fin"].ToString().Trim());

                        this.CmbInicio.SelectedValue = dtDatos.Rows[0]["hora_inicio"].ToString().Trim();
                        this.CmbIntervalo.SelectedValue = dtDatos.Rows[0]["intervalo"].ToString().Trim();
                        this.CmbHoraFin.SelectedValue = dtDatos.Rows[0]["hora_fin"].ToString().Trim();

                        //Creamos un array con los dia de ejecucion
                        string[] _ArrayDias = dtDatos.Rows[0]["dias_envio"].ToString().Trim().Split('|');
                        foreach (string value in _ArrayDias)
                        {
                            int nDia = Int32.Parse(value);
                            switch (nDia)
                            {
                                case 1:
                                    this.ChkDiasEnvio.Items[0].Selected = true;
                                    break;
                                case 2:
                                    this.ChkDiasEnvio.Items[1].Selected = true;
                                    break;
                                case 3:
                                    this.ChkDiasEnvio.Items[2].Selected = true;
                                    break;
                                case 4:
                                    this.ChkDiasEnvio.Items[3].Selected = true;
                                    break;
                                case 5:
                                    this.ChkDiasEnvio.Items[4].Selected = true;
                                    break;
                                case 6:
                                    this.ChkDiasEnvio.Items[5].Selected = true;
                                    break;
                                case 7:
                                    this.ChkDiasEnvio.Items[6].Selected = true;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        this.ViewState["IdentificacionOld"] = "";
                        this.ViewState["CreditoComercioOld"] = "0";
                    }
                }
                else
                {
                    this.ViewState["IdentificacionOld"] = "";
                    this.ViewState["CreditoComercioOld"] = "0";
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Error al mostrar los datos de la configuracion. Motivo: " + ex.ToString();
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

        protected void LstTipoTarea()
        {
            try
            {
                ObjTiposProceso.IdEstado = 1;
                ObjTiposProceso.MostrarSeleccione = "NO";
                ObjTiposProceso.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

                this.CmbTipoTarea.DataSource = ObjTiposProceso.GetTipoProceso();
                this.CmbTipoTarea.DataValueField = "idtipo_tarea";
                this.CmbTipoTarea.DataTextField = "tipo_tarea";
                this.CmbTipoTarea.DataBind();
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar los tipos de tareas. Motivo: " + ex.Message);
            }
        }

        protected void LstTipoEnvio()
        {
            try
            {
                ObjTipoEnvio.IdEstado = 1;
                ObjTipoEnvio.MostrarSeleccione = "SI";
                ObjTipoEnvio.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

                this.CmbTipoEnvio.DataSource = ObjTipoEnvio.GetTipoEnvio();
                this.CmbTipoEnvio.DataValueField = "idtipo_envio";
                this.CmbTipoEnvio.DataTextField = "tipo_envio";
                this.CmbTipoEnvio.DataBind();
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar los tipo envios de alertas. Motivo: " + ex.Message);
            }
        }

        protected void LstEstado()
        {
            try
            {
                ObjEstado.TipoConsulta = 2;
                ObjEstado.IdEstado = null;
                ObjEstado.MostrarSeleccione = "NO";
                ObjEstado.TipoEstado = "INTERFAZ";
                ObjEstado.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

                this.CmbEstado.DataSource = ObjEstado.GetEstados();
                this.CmbEstado.DataValueField = "id_estado";
                this.CmbEstado.DataTextField = "codigo_estado";
                this.CmbEstado.DataBind();
            }
            catch (Exception ex)
            {
                _log.Error("Error al listar los estados. Motivo: " + ex.ToString());
            }
        }

        protected void LstHorario()
        {
            try
            {
                DataTable DtHorario = new DataTable();
                DtHorario = ObjCmb.GetHorario();

                this.CmbInicio.DataSource = DtHorario;
                this.CmbInicio.DataValueField = "id_horario";
                this.CmbInicio.DataTextField = "descripcion_horario";
                this.CmbInicio.DataBind();

                this.CmbHoraFin.DataSource = DtHorario;
                this.CmbHoraFin.DataValueField = "id_horario";
                this.CmbHoraFin.DataTextField = "descripcion_horario";
                this.CmbHoraFin.DataBind();

                this.CmbIntervalo.DataSource = ObjCmb.GetIntervalo();
                this.CmbIntervalo.DataValueField = "id_intervalo";
                this.CmbIntervalo.DataTextField = "descripcion_intervalo";
                this.CmbIntervalo.DataBind();
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar los horarios. Motivo: " + ex.Message);
            }
        }
        #endregion

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objNavegacion.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = Request.QueryString["PathUrl"].ToString().Trim();
            objPermiso.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

            objPermiso.RefrescarPermisos();
            if (!objPermiso.PuedeLeer)
            {
                this.PanelDatos.Enabled = false;
            }
            if (!objPermiso.PuedeRegistrar)
            {
                this.BtnGuardarDatos.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.AplicarPermisos();
                this.ViewState["TipoProceso"] = Request.QueryString["TipoProceso"].ToString().Trim();
                this.ViewState["IdConfiguracion"] = "0";

                //Aqui listamos los combox en las listas
                this.LstTipoTarea();
                this.LstEstado();
                this.LstHorario();
                this.LstTipoEnvio();
                //--
                this.dtFechaInicio.SelectedDate = DateTime.Now;
                this.dtFechaInicio.MinDate = DateTime.Now;
                this.dtFechaFin.SelectedDate = DateTime.Now;
                this.dtFechaFin.MinDate = DateTime.Now;

                if (this.ViewState["TipoProceso"].Equals("1"))
                {
                    this.LblTitulo.Text = "REGISTRAR INFORMACIÓN DEL PROCESO O ALERTA";

                    this.dtFechaInicio.MaxDate = DateTime.Now;
                    this.dtFechaInicio.SelectedDate = DateTime.Now;
                    this.dtFechaFin.MaxDate = DateTime.Now;
                    this.dtFechaFin.SelectedDate = DateTime.Now;
                }
                else if (this.ViewState["TipoProceso"].Equals("2"))
                {
                    this.LblTitulo.Text = "EDITAR INFORMACIÓN DEL PROCESO O ALERTA";

                    this.ViewState["IdConfiguracion"] = Request.QueryString["IdConfiguracion"].ToString().Trim();
                    this.GetInfoConfiguracion();
                }
            }
        }

        protected void CmbTipoEnvio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.CmbTipoEnvio.SelectedValue.ToString().Trim().Equals("1"))
                {
                    this.CmbHoraFin.SelectedValue = "00:00";
                    this.CmbHoraFin.Enabled = true;
                    this.CmbIntervalo.SelectedValue = "5";
                    this.CmbIntervalo.Enabled = true;
                }
                else
                {
                    this.CmbHoraFin.SelectedValue = "00:00";
                    this.CmbHoraFin.Enabled = false;
                    this.CmbIntervalo.SelectedValue = "5";
                    this.CmbIntervalo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _MsgError = "Error al obtener al validar el tipo de proceso. Motivo: " + ex.Message;
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(150);
                Ventana.Width = Unit.Pixel(400);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                #endregion
            }
        }

        protected void BtnGuardarDatos_Click(object sender, EventArgs e)
        {
            string _MsgError = "";
            try
            {
                //Aqui validamos que allan seleccionado un dia de la Lista.
                string strSelectedDays = "";
                foreach (ListItem item in this.ChkDiasEnvio.Items)
                {
                    if (item.Selected)
                    {
                        strSelectedDays += item.Value + "|";
                    }
                }

                if (strSelectedDays.Length > 0)
                {                    //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                    this.RadWindowManager1.Enabled = false;
                    this.RadWindowManager1.EnableAjaxSkinRendering = false;
                    this.RadWindowManager1.Visible = false;

                    strSelectedDays = strSelectedDays.Substring(0, strSelectedDays.Length - 1);
                    //--
                    string _NombreTarea = "TASK_ACTIVIDAD_USUARIOS";
                    _MsgError = "";
                    DeleteTaskScheduler(_NombreTarea, ref _MsgError);
                    if (CreateTaskScheduler(_NombreTarea, ref _MsgError))
                    {
                        #region OBTENEMOS VALORES DE ATRIBUTOS DE OBJETO
                        //--AQUI OBTENEMOS LOS VALORES DEL OBJETO
                        ObjConfiguracion.IdConfiguracionAlerta = Int32.Parse(this.ViewState["TipoProceso"].ToString().Trim()) == 1 ? null : this.ViewState["IdConfiguracion"].ToString().Trim();
                        ObjConfiguracion.IdTipoTarea = Int32.Parse(this.CmbTipoTarea.SelectedValue.Trim());
                        ObjConfiguracion.DiasSeguimiento = Int32.Parse(this.TxtNumeroDias.Text.ToString().Trim());
                        ObjConfiguracion.IdTipoEnvio = Int32.Parse(this.CmbTipoEnvio.SelectedValue.Trim());
                        ObjConfiguracion.FechaInicio = DateTime.Now.ToString("yyyy-MM-dd");
                        ObjConfiguracion.HoraInicio = this.CmbInicio.SelectedItem.Text.ToString().Trim();
                        ObjConfiguracion.Intervalo = Int32.Parse(this.CmbIntervalo.SelectedItem.Text.Trim());
                        ObjConfiguracion.FechaFin = DateTime.Now.ToString("yyyy-MM-dd");
                        ObjConfiguracion.HoraFin = this.CmbHoraFin.SelectedValue.Trim();
                        ObjConfiguracion.DiasEnvio = strSelectedDays;
                        ObjConfiguracion.IdEstado = this.CmbEstado.SelectedValue.Trim();
                        ObjConfiguracion.IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString().Trim());
                        ObjConfiguracion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                        ObjConfiguracion.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();
                        ObjConfiguracion.TipoProceso = Int32.Parse(this.ViewState["TipoProceso"].ToString().Trim());

                        //--AQUI IMPRIMIMOS EN EL LOG DE AUDITORIA
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string jsonRequest = js.Serialize(ObjConfiguracion);
                        _log.Warn("REQUEST CONFIGURAR TAREA PROGRAMADA => " + jsonRequest);
                        #endregion

                        int _IdRegistro = 0;
                        _MsgError = "";
                        if (ObjConfiguracion.AddUpConfiguracionAlerta(ref _IdRegistro, ref _MsgError))
                        {
                            #region REGISTRO DE LOGS DE AUDITORIA
                            //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                            ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                            ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                            ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                            ObjAuditoria.ModuloApp = "TAREAS_PROGRAMADAS";
                            //--TIPOS DE EVENTO: 1. LOGIN, 2. INSERT, 3. UPDATE, 4. DELETE, 5. CONSULTA
                            ObjAuditoria.IdTipoEvento = 2;
                            ObjAuditoria.UrlVisitada = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                            ObjAuditoria.DescripcionEvento = jsonRequest;
                            ObjAuditoria.IPCliente = ObjUtils.GetIPAddress().ToString().Trim();
                            ObjAuditoria.TipoProceso = 1;

                            //'Agregar Auditoria del sistema
                            string _MsgErrorLogs = "";
                            if (!ObjAuditoria.AddAuditoria(ref _MsgErrorLogs))
                            {
                                _log.Error(_MsgErrorLogs);
                            }
                            #endregion

                            #region MOSTRAR MENSAJE DE USUARIO
                            this.UpdatePanel1.Update();
                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;
                            Ventana.Modal = true;

                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(300);
                            Ventana.Width = Unit.Pixel(600);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Mensaje del Sistema";
                            Ventana.VisibleStatusbar = false;
                            Ventana.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move;
                            this.RadWindowManager1.Windows.Add(Ventana);
                            this.RadWindowManager1 = null;
                            Ventana = null;
                            #endregion
                        }
                        else
                        {
                            #region MOSTRAR MENSAJE DE USUARIO
                            this.UpdatePanel1.Update();
                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;
                            Ventana.Modal = true;

                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(300);
                            Ventana.Width = Unit.Pixel(600);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Mensaje del Sistema";
                            Ventana.VisibleStatusbar = false;
                            Ventana.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move;
                            this.RadWindowManager1.Windows.Add(Ventana);
                            this.RadWindowManager1 = null;
                            Ventana = null;
                            #endregion
                        }
                    }
                }
                else
                {
                    #region MOSTRAR MENSAJE DE USUARIO
                    this.UpdatePanel1.Update();
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    _MsgError = "Señor usuario, debe seleccionar al menos un día de la lista.";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(300);
                    Ventana.Width = Unit.Pixel(600);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Mensaje del Sistema";
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    return;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                this.UpdatePanel1.Update();
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                _MsgError = "Error al guardar la configuración. Motivo: " + ex.Message;
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                #endregion
            }
        }

        private bool CreateTaskScheduler(string _NombreTarea, ref string _MsgError)
        {
            bool Result = false;
            try
            {
                //Obtener el servicio en la máquina local
                using (TaskService ts = new TaskService())
                {
                    //Crear una nueva definición de tareas y asignar propiedades
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Tarea Programada de forma Manual para generar el proceso de actividad de usuarios en el sistema Smartax";

                    WeeklyTrigger wt = new WeeklyTrigger();
                    int idx = 0;
                    string[] ArrayHoraInicio = this.CmbInicio.SelectedItem.Text.Split(':');
                    wt.StartBoundary = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(ArrayHoraInicio[0]), int.Parse(ArrayHoraInicio[1]), 0);

                    foreach (ListItem item in this.ChkDiasEnvio.Items)
                    {
                        if (item.Selected)
                        {
                            switch (Int32.Parse(item.Value))
                            {
                                case 1:
                                    wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Monday : wt.DaysOfWeek | DaysOfTheWeek.Monday);
                                    break;
                                case 2:
                                    wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Tuesday : wt.DaysOfWeek | DaysOfTheWeek.Tuesday);
                                    break;
                                case 3:
                                    wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Wednesday : wt.DaysOfWeek | DaysOfTheWeek.Wednesday);
                                    break;
                                case 4:
                                    wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Thursday : wt.DaysOfWeek | DaysOfTheWeek.Thursday);
                                    break;
                                case 5:
                                    wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Friday : wt.DaysOfWeek | DaysOfTheWeek.Friday);
                                    break;
                                case 6:
                                    wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Saturday : wt.DaysOfWeek | DaysOfTheWeek.Saturday);
                                    break;
                                case 7:
                                    wt.DaysOfWeek = (idx == 0 ? DaysOfTheWeek.Sunday : wt.DaysOfWeek | DaysOfTheWeek.Sunday);
                                    break;
                            }
                            idx++;
                        }
                    }

                    wt.WeeksInterval = 1;

                    //Configurar la hora de inicio y fin de la tarea.
                    string[] ArrayHoraFin = this.CmbHoraFin.SelectedItem.Text.Split(':');
                    int startHour = int.Parse(ArrayHoraInicio[0].ToString().Trim());
                    int endHour = int.Parse(ArrayHoraFin[0].ToString().Trim());
                    int startMinute = int.Parse(ArrayHoraInicio[1].ToString().Trim());
                    int endMinute = int.Parse(ArrayHoraFin[1].ToString().Trim());

                    int _IdTipoEnvio = Int32.Parse(this.CmbTipoEnvio.SelectedValue.ToString().Trim());
                    if (_IdTipoEnvio == 1)
                    {
                        wt.Repetition.Duration = new TimeSpan((endHour - startHour), Math.Abs(endMinute - startMinute), 0);
                        wt.Repetition.Interval = TimeSpan.FromMinutes(Int32.Parse(this.CmbIntervalo.SelectedItem.Text));
                    }
                    else
                    {
                        //wt.Repetition.Duration = new TimeSpan((endHour - startHour), Math.Abs(endMinute - startMinute), 0);
                        //wt.Repetition.Interval = TimeSpan.FromMinutes(Int32.Parse(this.CmbIntervalo.SelectedItem.Text));
                    }
                    td.Triggers.Add(wt);

                    int _TipoProceso = 6;   //--TASK_ACTIVIDAD_USUARIOS
                    int _IdFormImpuesto = 1;
                    int _IdCliente = 4;
                    int _AnioGravable = Int32.Parse(DateTime.Now.ToString("yyyy"));
                    string _MesEf = DateTime.Now.ToString("MM");
                    string _VersionEf = "VERSION_1";
                    int _IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    //--
                    td.Actions.Add(new ExecAction(FixedData.PathTasksProgramadas, _TipoProceso + " " + _NombreTarea + " " + _IdCliente + " " + _AnioGravable + " " + _MesEf + " " + _IdFormImpuesto + " " + _VersionEf + " " + _IdUsuario));
                    ts.RootFolder.RegisterTaskDefinition(_NombreTarea.ToString(), td, TaskCreation.CreateOrUpdate, FixedData.UserCreateTasks, FixedData.PassCreateTasks);
                    Result = true;
                    _MsgError = "";
                }
            }
            catch (Exception ex)
            {
                Result = false;
                _MsgError = "Error al crear la tarea programada [" + _NombreTarea + "]. Motivo: " + ex.Message;
            }

            return Result;
        }

        private bool DeleteTaskScheduler(string _NombreTarea, ref string _MsgError)
        {
            bool Result = false;
            try
            {
                using (TaskService ts = new TaskService())
                {
                    ts.RootFolder.DeleteTask(_NombreTarea.ToString().Trim());
                    _MsgError = "";
                    Result = true;
                    _MsgError = "Señor usuario, la tarea programada [" + _NombreTarea + "] ha sido borrada del sistema de forma exitosa.";
                    _log.Info(_MsgError);
                }
            }
            catch (Exception ex)
            {
                Result = false;
                _MsgError = "Error al borrar la tarea programada [" + _NombreTarea + "] del sistema. Motivo: " + ex.Message;
            }

            return Result;
        }

    }
}