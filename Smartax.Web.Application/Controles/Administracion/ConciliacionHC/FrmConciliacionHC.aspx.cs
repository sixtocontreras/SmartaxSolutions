using System;
using System.Linq;
using System.Web.UI.WebControls;
using log4net;
using Microsoft.Win32.TaskScheduler;
using Telerik.Web.UI;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.ProcessAPIs;
using Smartax.Web.Application.Clases.Seguridad;
using System.Data;
using System.IO;

namespace Smartax.Web.Application.Controles.Administracion.ConciliacionHC
{
    public partial class FrmConciliacionHC : System.Web.UI.Page
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
                //ObjLista.MostrarSeleccione = "SI";
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

                //--AQUI OBTENEMOS LOS DATOS PARA REALIZAR EL PROCESO DE CONCILIACION HC
                ObjConciliacionHC.TipoConsulta = 2;
                ObjConciliacionHC.TipoArchivo = "PROCESAR_FILE_DAVIBOX";
                ObjConciliacionHC.IdEstado = 1;
                ObjConciliacionHC.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                this.ViewState["UuIdFolder"] = "NA";

                //--PASO 1: OBTENER DATOS DE LOS ARCHIVOS A PROCESAR
                DataTable dtFileDavibox = new DataTable();
                string _MsgError = "";
                dtFileDavibox = ObjConciliacionHC.GetArchivosDavibox(ref _MsgError);
                //--
                if (dtFileDavibox != null)
                {
                    if (dtFileDavibox.Rows.Count > 0)
                    {
                        this.CmbAnioGravable.SelectedValue = dtFileDavibox.Rows[0]["anio_gravable"].ToString().Trim();
                        this.CmbTipoPeriodicidad.SelectedValue = dtFileDavibox.Rows[0]["tipo_periodicidad"].ToString().Trim();
                        //--AQUI VALIDAMOS EL TIPO DE PERIODICIDAD
                        if (this.CmbTipoPeriodicidad.SelectedValue.ToString().Trim().Equals("1"))
                        {
                            this.LstPeriodicidadMensual();
                        }
                        else
                        {
                            this.LstPeriodicidadBimestral();

                        }
                        //--
                        this.CmbPeriodo.SelectedValue = dtFileDavibox.Rows[0]["periodicidad"].ToString().Trim();
                        this.ViewState["UuIdFolder"] = dtFileDavibox.Rows[0]["uuid_folder"].ToString().Trim();
                    }
                }
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
                ObjProcessAPI.UuId = this.ViewState["UuIdFolder"].ToString().Trim();
                string _DataTarea = this.CmbAnioGravable.SelectedItem.Text.ToString().Trim() + "_" + this.CmbTipoPeriodicidad.SelectedItem.Text.ToString().Trim() + "_" + this.CmbPeriodo.SelectedValue.ToString().Trim();

                string _Mensaje = "", _UuId = "";
                if (ObjProcessAPI.GetConciliarFileDavibox(_TipoPeriodicidad, _Periodo, ref _UuId, ref _Mensaje))
                {
                    _UuId = this.ViewState["UuIdFolder"].ToString().Trim();
                    #region AQUI OBTENEMOS LOS DATOS DE RESPUESTA DEL SERVICIO
                    if (_UuId.ToString().Trim().Length > 0)
                    {
                        #region AQUI RECORREMOS LA RUTA DE LOS ARCHIVOS
                        //--
                        //--ESTA VARIABLE ES SOLO PARA EL PROCESO DE CARGUE DE INFO DE TARJETA DE CREDITO
                        DataTable dtFiles = new DataTable();
                        dtFiles.TableName = "DtFiles";
                        dtFiles = new DataTable();
                        dtFiles.Columns.Add("ID_REGISTRO", typeof(Int32));
                        dtFiles.PrimaryKey = new DataColumn[] { dtFiles.Columns["ID_REGISTRO"] };
                        dtFiles.Columns.Add("NOMBRE_ARCHIVO");
                        //dtFiles.Columns.Add("CANTIDAD_REGISTROS");

                        //--
                        string _PathFile = FixedData.PathFilesCrucesDavibox + "\\" + _UuId.ToString().Trim();
                        string[] files = Directory.GetFiles(_PathFile); // Obtener archivos
                        //--
                        foreach (string NameFile in files)
                        {
                            //--AQUI AGREGAMOS LOS DATOS DEL ARCHIVO AL DATATABLE
                            DataRow Fila = null;
                            Fila = dtFiles.NewRow();
                            Fila["ID_REGISTRO"] = dtFiles.Rows.Count + 1;
                            Fila["NOMBRE_ARCHIVO"] = NameFile.ToString().Trim();
                            dtFiles.Rows.Add(Fila);
                        }

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
                        _Mensaje = "Señor usuario, no se encontro una ruta para buscar los archivos de conciliación. Por favor validar !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
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
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
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

                    int _IdCliente = Int32.Parse(this.Session["IdCliente"].ToString().Trim());
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