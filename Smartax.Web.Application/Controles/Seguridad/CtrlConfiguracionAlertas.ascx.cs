using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;

namespace Smartax.Web.Application.Controles.Seguridad
{
    public partial class CtrlConfiguracionAlertas : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        #region DEFINICION DE OBJETOS DE CLASES
        ConfiguracionTareas ObjConfiguracion = new ConfiguracionTareas();
        Utilidades ObjUtils = new Utilidades();
        #endregion

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            DataTable DtDatos = new DataTable();
            try
            {
                ObjConfiguracion.IdEstado = null;
                ObjConfiguracion.IdRol = Int32.Parse(Session["IdRol"].ToString().Trim());
                ObjConfiguracion.IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString().Trim());
                ObjConfiguracion.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();
                ObjConfiguracion.TipoProceso = 1;

                //Mostrar los Seguridad
                ObjetoDataTable = ObjConfiguracion.GetAllConfiguracionTareas();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_configuracion, idtipo_tarea, idtipo_envio, id_estado"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);
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
                string _MsgMensaje = "Error al cargar los Datos de las configuración de las tareas. Motivo: " + ex.ToString();
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

            return ObjetoDataSet;
        }

        private DataSet FuenteDatos
        {
            get
            {
                object obj = this.ViewState["_FuenteDatos"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetDatosGrilla();
                    this.ViewState["_FuenteDatos"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatos"];
                }
            }
            set { this.ViewState["_FuenteDatos"] = value; }
        }

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objNavegacion.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            objPermiso.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

            objPermiso.RefrescarPermisos();
            if (!objPermiso.PuedeLeer)
            {
                this.RadGrid1.Visible = false;
            }
            if (!objPermiso.PuedeRegistrar)
            {
                this.RadGrid1.MasterTableView.CommandItemDisplay = 0;
            }
            if (!objPermiso.PuedeModificar)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 2].Visible = false;
            }
            if (!objPermiso.PuedeBloquear)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 1].Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.Page.Title = this.Page.Title + "Configurar Tareas Programadas";
                this.AplicarPermisos();
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
        }

        #region DEFINICION EVENTOS DE GRID
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid1.DataSource = this.FuenteDatos;
                RadGrid1.DataMember = "DtConfiguracion";
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
                string _MsgMensaje = "Error con el evento NeedDataSource. Motivo: " + ex.ToString();
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

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnAddConfiguracion")
                {
                    #region AQUI MOSTRAMOS EL FORM
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    int _IdConfiguracion = -1;
                    int _TipoProceso = 1;
                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Seguridad/FrmAddConfiguracionAlerta.aspx?IdConfiguracion=" + _IdConfiguracion + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(510);
                    Ventana.Width = Unit.Pixel(820);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Registrar nueva Tarea Programada";
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    #endregion
                }
                else if (e.CommandName == "BtnActualizarLista")
                {
                    this.ViewState["_FuenteDatos"] = null;
                    this.RadGrid1.Rebind();
                }
                else if (e.CommandName == "BtnEditar")
                {
                    #region MOSTRAMOS EL FORM PARA EDITAR LA TAREA PROGRAMADA
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdConfiguracion = Int32.Parse(item.GetDataKeyValue("id_configuracion").ToString().Trim());
                    int _IdTipoTarea = Int32.Parse(item.GetDataKeyValue("idtipo_tarea").ToString().Trim());
                    int _IdTipoEnvio = Int32.Parse(item.GetDataKeyValue("idtipo_envio").ToString().Trim());
                    int _IdEstado = Int32.Parse(item.GetDataKeyValue("id_estado").ToString().Trim());
                    string _NombreTarea = item["tipo_tarea"].Text.ToString().Trim();
                    string _Estado = item["codigo_estado"].Text.ToString().Trim();

                    if (_IdEstado == 1)
                    {
                        #region AQUI MOSTRAMOS EL FORM
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;
                        Ventana.Modal = true;

                        int _TipoProceso = 2;
                        string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                        Ventana.NavigateUrl = "/Controles/Seguridad/FrmAddConfiguracionAlerta.aspx?IdConfiguracion=" + _IdConfiguracion + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(510);
                        Ventana.Width = Unit.Pixel(820);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Editar Información de la Configuración: " + _NombreTarea;
                        Ventana.VisibleStatusbar = false;
                        Ventana.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize;
                        this.RadWindowManager1.Windows.Add(Ventana);
                        this.RadWindowManager1 = null;
                        Ventana = null;
                        #endregion
                    }
                    else
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
                        string _MsgMensaje = "Señor usuario. La Configuración se encuentra en estado [" + _Estado + "]. Realize el Proceso de Activarla para realizar la Gestión.";
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
                    #endregion
                }
                else if (e.CommandName == "BtnBloquear")
                {
                    #region AQUI SE REALIZA EL PROCESO DEL BLOQUEO DE LA TAREA
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdComercio = Convert.ToInt32(item.GetDataKeyValue("id_configuracion").ToString().Trim());
                    string _NombreComercio = item["razon_social_comercio"].Text.ToString().Trim();
                    string _NumeroIdentificacion = item["numero_identificacion"].Text.ToString().Trim();
                    string _Estado = item["codigo_estado"].Text.ToString().Trim();

                    //ObjConfiguracion.IdComercio = _IdComercio;
                    //ObjConfiguracion.IdTipoNivel = 1;
                    //ObjConfiguracion.IdTipoIdentificacion = 1;
                    //ObjConfiguracion.NumeroIdentificacion = _NumeroIdentificacion;
                    //ObjConfiguracion.RazonSocialComercio = _NombreComercio;
                    //ObjConfiguracion.IdPais = 1;
                    //ObjConfiguracion.IdDpto = 1;
                    //ObjConfiguracion.IdCiudad = 1;
                    //ObjConfiguracion.DireccionComercio = "";
                    //ObjConfiguracion.TelefonoFijoComercio = "";
                    //ObjConfiguracion.EmailComercio = "";
                    //ObjConfiguracion.NombreContacto1Comercio = "";
                    //ObjConfiguracion.ApellidoContacto1Comercio = "";
                    //ObjConfiguracion.TelContacto1Comercio = "";
                    //ObjConfiguracion.NombreContacto2Comercio = "";
                    //ObjConfiguracion.TelContacto2Comercio = "";
                    //ObjConfiguracion.LatitudComercio = 0;
                    //ObjConfiguracion.LongitudComercio = 0;
                    //ObjConfiguracion.ObtenerMejorTarifa = false;
                    //ObjConfiguracion.IdTipoRed = 1;
                    //ObjConfiguracion.IdTipoComercio = 1;
                    //ObjConfiguracion.IdTipoRegimen = 1;
                    //ObjConfiguracion.IdComercioPadre = null;

                    //ObjConfiguracion.IdEstado = _Estado.Equals("ACTIVO") ? 0 : 1;
                    //ObjConfiguracion.IdUsuarioAdd = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    //ObjConfiguracion.IdUsuarioUp = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    //ObjConfiguracion.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                    //ObjConfiguracion.IdRol = Convert.ToInt32(Session["IdRol"].ToString().Trim());
                    //ObjConfiguracion.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();
                    //ObjConfiguracion.TipoProceso = 4;

                    //int _IdRegistro = 0;
                    //string _MsgError = "";
                    //if (ObjConfiguracion.AddUpComercio(ref _IdRegistro, ref _MsgError))
                    //{
                    //    //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                    //    this.RadWindowManager1.Enabled = false;
                    //    this.RadWindowManager1.EnableAjaxSkinRendering = false;
                    //    this.RadWindowManager1.Visible = false;

                    //    this.Aviso1.Mensaje = _MsgError;
                    //    this.Aviso1.Titulo = "Registrar Seguridad";
                    //    this.Aviso1.TipoImagen = "Editado";
                    //    this.Aviso1.MostrarAviso();
                    //    this.Aviso1.AsignarImagen();
                    //    _log.Info(this.Aviso1.Mensaje.ToString().Trim());
                    //    this.ViewState["_FuenteDatos"] = null;
                    //    this.RadGrid1.Rebind();
                    //}
                    //else
                    //{
                    //    //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                    //    this.RadWindowManager1.Enabled = false;
                    //    this.RadWindowManager1.EnableAjaxSkinRendering = false;
                    //    this.RadWindowManager1.Visible = false;

                    //    this.Aviso1.Mensaje = _MsgError;
                    //    this.Aviso1.Titulo = "Registrar Seguridad";
                    //    this.Aviso1.TipoImagen = "Error";
                    //    this.Aviso1.MostrarAviso();
                    //    this.Aviso1.AsignarImagen();
                    //    _log.Error(this.Aviso1.Mensaje.ToString().Trim());
                    //}
                    #endregion
                }
                else
                {
                    //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                    this.RadWindowManager1.Enabled = false;
                    this.RadWindowManager1.EnableAjaxSkinRendering = false;
                    this.RadWindowManager1.Visible = false;
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
                string _MsgMensaje = "Error con el evento ItemCommand. Motivo: " + ex.ToString();
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

        protected void RadGrid1_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            try
            {
                RadGrid1.Rebind();
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
                string _MsgMensaje = "Error con el evento PageIndexChanged. Motivo: " + ex.ToString();
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
        #endregion

    }
}