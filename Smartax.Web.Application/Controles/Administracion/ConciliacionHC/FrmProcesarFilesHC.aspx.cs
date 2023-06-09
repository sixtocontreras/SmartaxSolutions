﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Drawing.Charts;
using log4net;
using Microsoft.Win32.TaskScheduler;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.ProcessAPIs;
using Smartax.Web.Application.Clases.Seguridad;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Administracion.ConciliacionHC
{
    public partial class FrmProcesarFilesHC : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        ConciliacionesHerramientaCuadre ObjConciliacionHC = new ConciliacionesHerramientaCuadre();
        ProcessAPI ObjProcessAPI = new ProcessAPI();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        EnvioCorreo ObjCorreo = new EnvioCorreo();
        Utilidades ObjUtils = new Utilidades();
        Combox ObjLista = new Combox();

        protected void LstAnioGravable()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                this.CmbAnioGravable.DataSource = ObjLista.GetAnios();
                this.CmbAnioGravable.DataValueField = "id_anio";
                this.CmbAnioGravable.DataTextField = "numero_anio";
                this.CmbAnioGravable.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los años. Motivo: " + ex.ToString();
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

        protected void LstTipoPeriodicidad()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                this.CmbTipoPeriodicidad.DataSource = ObjLista.GetTipoPeriodicidad();
                this.CmbTipoPeriodicidad.DataValueField = "idtipo_periodicidad";
                this.CmbTipoPeriodicidad.DataTextField = "tipo_periodicidad";
                this.CmbTipoPeriodicidad.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los tipo de periodicidad. Motivo: " + ex.ToString();
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

        protected void LstPeriodicidadMensual()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                this.CmbPeriodo.DataSource = ObjLista.GetMeses();
                this.CmbPeriodo.DataValueField = "id_mes";
                this.CmbPeriodo.DataTextField = "numero_mes";
                this.CmbPeriodo.DataBind();

                this.BtnEjecProceso.Enabled = true;
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
                string _MsgMensaje = "Señor usuario. Ocurrio un error al listar la periodicidad mensual. Motivo: " + ex.ToString();
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

        protected void LstPeriodicidadBimestral()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                this.CmbPeriodo.DataSource = ObjLista.GetBimestral();
                this.CmbPeriodo.DataValueField = "id_periodicidad";
                this.CmbPeriodo.DataTextField = "periodicidad";
                this.CmbPeriodo.DataBind();

                this.BtnEjecProceso.Enabled = true;
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
                string _MsgMensaje = "Señor usuario. Ocurrio un error al listar la periodicidad bimestral. Motivo: " + ex.ToString();
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

        protected void LstAplicativo()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                //this.CmbAplicativo.DataSource = ObjLista.GetAplicativo();
                //this.CmbAplicativo.DataValueField = "id_aplicativo";
                //this.CmbAplicativo.DataTextField = "aplicativo";
                //this.CmbAplicativo.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los años. Motivo: " + ex.ToString();
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

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = Request.QueryString["PathUrl"].ToString().Trim();
            objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

            objPermiso.RefrescarPermisos();
            this.BtnEjecProceso.Enabled = false;
            if (!objPermiso.PuedeRegistrar)
            {
                //this.BtnGuardar.Enabled = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //this.AplicarPermisos();
                //--LISTAR COMBOBOX
                this.LstAnioGravable();
                //this.LstAplicativo();
                this.LstTipoPeriodicidad();
            }
        }

        protected void CmbTipoPeriodicidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.CmbTipoPeriodicidad.SelectedValue.ToString().Trim().Equals("1"))
                {
                    this.LstPeriodicidadMensual();
                }
                else
                {
                    this.LstPeriodicidadBimestral();

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void BtnEjecProceso_Click(object sender, EventArgs e)
        {
            try
            {
                int _TipoPeriodicidad = Int32.Parse(this.CmbTipoPeriodicidad.SelectedValue.ToString().Trim());
                int _Periodo = Int32.Parse(this.CmbPeriodo.SelectedValue.ToString().Trim());
                ObjProcessAPI.AnioProcesar = Int32.Parse(this.CmbAnioGravable.SelectedValue.ToString().Trim());
                ObjProcessAPI.MesProcesar = Int32.Parse(this.CmbPeriodo.SelectedValue.ToString().Trim());
                //--GENERAR EL UUID
                Guid _UuId = Guid.NewGuid();
                ObjProcessAPI.UuId = _UuId.ToString().Trim();
                string _DataTarea = this.CmbAnioGravable.SelectedItem.Text.ToString().Trim() + "_" + this.CmbTipoPeriodicidad.SelectedItem.Text.ToString().Trim() + "_" + this.CmbPeriodo.SelectedValue.ToString().Trim();

                string _Mensaje = "";
                if (ObjProcessAPI.GetDownloadFileDavibox(_TipoPeriodicidad, _Periodo, ref _Mensaje))
                {
                    //--ACTUALIZAMOS EL UUID EN LA TABLA DE ARCHIVOS A PROCESAR
                    ObjConciliacionHC.TipoProceso = 2;
                    ObjConciliacionHC.UuId = _UuId.ToString().Trim();
                    ObjConciliacionHC.AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim();
                    ObjConciliacionHC.TipoPeriodicidad = this.CmbTipoPeriodicidad.SelectedValue.ToString().Trim();
                    ObjConciliacionHC.Periodicidad = this.CmbPeriodo.SelectedValue.ToString().Trim();
                    ObjConciliacionHC.TipoArchivo = "PROCESAR_FILE_DAVIBOX";
                    ObjConciliacionHC.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjConciliacionHC.UpUuidFilesDavibox(ref _IdRegistro, ref _MsgError))
                    {
                        //--PROCESO EXITOSO MANDAMOS A CREAR LA TAREA PROGRAMADA
                        //--
                        int _TipoProceso = 5;
                        string _NombreTarea = "FILE_DAVIBOX_" + _DataTarea;
                        _MsgError = "";
                        DeleteTaskSchedulerManual(_NombreTarea, ref _MsgError);

                        //--
                        _MsgError = "";
                        if (CreateTaskSchedulerManual(_TipoProceso, ObjProcessAPI.UuId, _NombreTarea, ref _MsgError))
                        {
                            #region REGISTRO DE LOGS DE AUDITORIA
                            //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                            ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                            ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                            ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                            ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                            ObjAuditoria.ModuloApp = "TASK_FILE_DAVIBOX";
                            ObjAuditoria.UrlVisitada = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                            ObjAuditoria.DescripcionEvento = _TipoProceso + "|" + 3 + "|" + _NombreTarea;
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
                            //Mostramos el mensaje porque se produjo un error con la Trx.
                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;

                            RadWindow Ventana = new RadWindow();
                            Ventana.Modal = true;
                            _Mensaje = "Señor usuario, el proceso de la tarea programada fue creada de forma exitosa dentro algunos minutos le estara llegando un correo con la confirmación de que el proceso ha terminado. !";
                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
                            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(300);
                            Ventana.Width = Unit.Pixel(650);
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
                            //Mostramos el mensaje porque se produjo un error con la Trx.
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
                    else
                    {
                        #region MOSTRAR MENSAJE DE USUARIO
                        this.UpdatePanel1.Update();
                        //Mostramos el mensaje porque se produjo un error con la Trx.
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
                else
                {
                    #region MOSTRAR MENSAJE DE USUARIO
                    this.UpdatePanel1.Update();
                    //Mostramos el mensaje porque se produjo un error con la Trx.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string[] _MsgError = _Mensaje.ToString().Trim().Split('|');
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError[1].ToString().Trim();
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
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al procesar el servicio. Motivo: " + ex.ToString();
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

        private bool CreateTaskSchedulerManual(int _TipoProceso, string _UuId, string _NombreTarea, ref string _MsgError)
        {
            bool Result = false;
            string _NombreProveedor = "";
            try
            {
                //Obtener el servicio en la máquina local
                using (TaskService ts = new TaskService())
                {
                    //--AQUI DEFINIMOS LA FECHA CUANDO SE VA A EJECUTAR EL PROCESO
                    DateTime dtFechaInicio = DateTime.Now;
                    //Crear una nueva definición de tareas y asignar propiedades
                    TaskDefinition td = ts.NewTask();
                    //--AQUI VALIDAMOS EL TIPO DE PROCESO: 1. BASE GRAVABLE, 2. LIQUIDACION POR OFICINAS
                    td.RegistrationInfo.Description = "Tarea Programada de forma Manual para generar el proceso del servicio de davibox";

                    WeeklyTrigger wt = new WeeklyTrigger();
                    string _HoraGenerarTask = DateTime.Now.ToString("HH") + ":" + DateTime.Now.AddMinutes(Int32.Parse(FixedData.MinutosEjecutarTasks.ToString().Trim())).ToString("mm");
                    string[] ArrayHoraGeneracion = _HoraGenerarTask.Split(':');
                    wt.StartBoundary = new DateTime(dtFechaInicio.Year, dtFechaInicio.Month, dtFechaInicio.Day, int.Parse(ArrayHoraGeneracion[0].ToString().Trim()), int.Parse(ArrayHoraGeneracion[1].ToString().Trim()), 0);
                    _log.Error("LA TAREA PROGRAMADA [" + _NombreTarea + "] A LAS => " + _HoraGenerarTask);
                    //--
                    #region AQUI TOMAMOS LOS DIAS DE EJECUCION DE LA TAREA PROGRAMADA
                    //--AQUI OBTENEMOS EL NUMERO DE DIA DE LA SEMANA
                    DateTime dateValue = new DateTime(Int32.Parse(DateTime.Now.ToString("yyyy")), Int32.Parse(DateTime.Now.ToString("MM")), Int32.Parse(DateTime.Now.ToString("dd")));
                    int _NumroDiaSemana = (int)dateValue.DayOfWeek;
                    //--
                    switch (_NumroDiaSemana)
                    {
                        case 1:
                            wt.DaysOfWeek = DaysOfTheWeek.Monday;
                            break;
                        case 2:
                            wt.DaysOfWeek = DaysOfTheWeek.Tuesday;
                            break;
                        case 3:
                            wt.DaysOfWeek = DaysOfTheWeek.Wednesday;
                            break;
                        case 4:
                            wt.DaysOfWeek = DaysOfTheWeek.Thursday;
                            break;
                        case 5:
                            wt.DaysOfWeek = DaysOfTheWeek.Friday;
                            break;
                        case 6:
                            wt.DaysOfWeek = DaysOfTheWeek.Saturday;
                            break;
                        case 7:
                            wt.DaysOfWeek = DaysOfTheWeek.Sunday;
                            break;
                    }
                    #endregion

                    wt.WeeksInterval = 1;
                    wt.Repetition.Duration = new TimeSpan(0, 0, 0);
                    wt.Repetition.Interval = TimeSpan.FromMinutes(0);
                    td.Triggers.Add(wt);

                    #region AQUI OBTENEMOS EL MES ANTERIOR PARA EL BIMESTRE
                    //--
                    //int _PrevMonth = 0;
                    //switch (_Periodo)
                    //{
                    //    case 1:
                    //        _PrevMonth = 1;
                    //        break;
                    //    case 2:
                    //        _PrevMonth = 3;
                    //        break;
                    //    case 3:
                    //        _PrevMonth = 5;
                    //        break;
                    //    case 4:
                    //        _PrevMonth = 7;
                    //        break;
                    //    case 5:
                    //        _PrevMonth = 9;
                    //        break;
                    //    case 6:
                    //        _PrevMonth = 11;
                    //        break;
                    //    default:
                    //        _PrevMonth = (_Periodo - 1);
                    //        break;
                    //}
                    #endregion

                    int _IdCliente = this.Session["IdCliente"] != null ? Int32.Parse(this.Session["IdCliente"].ToString().Trim()) : 4;
                    int _IdTipoPeriodicidad = Int32.Parse(this.CmbTipoPeriodicidad.SelectedValue.ToString().Trim());
                    int _AnioGravable = Int32.Parse(this.CmbAnioGravable.SelectedValue.ToString().Trim());
                    string _MesEf = this.CmbPeriodo.SelectedValue.ToString().Trim();
                    //string _VersionEf = _PrevMonth.ToString();
                    string _VersionEf = _UuId;
                    int _IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    //--
                    td.Actions.Add(new ExecAction(FixedData.PathTasksProgramadas, _TipoProceso + " " + _NombreTarea + " " + _IdCliente + " " + _AnioGravable + " " + _MesEf + " " + _IdTipoPeriodicidad + " " + _VersionEf + " " + _IdUsuario));
                    ts.RootFolder.RegisterTaskDefinition(_NombreTarea.ToString(), td, TaskCreation.CreateOrUpdate, FixedData.UserCreateTasks, FixedData.PassCreateTasks);
                    Result = true;
                    _MsgError = "";
                }
            }
            catch (Exception ex)
            {
                Result = false;
                _MsgError = "Error al generar la tarea programa de forma manual del proveedor [" + _NombreProveedor + "]. Motivo: " + ex.ToString();
                _log.Error(_MsgError);
            }

            return Result;
        }

        private bool DeleteTaskSchedulerManual(string _NombreTarea, ref string _MsgError)
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
                _MsgError = "Señor usuario, ocurrio un error al borrar la tarea programada [" + _NombreTarea + "] del sistema. Motivo: " + ex.Message;
                _log.Error(_MsgError);
            }

            return Result;
        }
    }
}