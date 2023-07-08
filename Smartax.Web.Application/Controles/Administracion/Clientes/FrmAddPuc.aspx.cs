using System;
using System.Data;
using Telerik.Web.UI;
using log4net;
using System.Web.Caching;
using System.Web.UI.WebControls;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using System.Collections;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Administracion.Clientes
{
    public partial class FrmAddPuc : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        PlanUnicoCuenta ObjPuc = new PlanUnicoCuenta();
        ClientePuc ObjClientePuc = new ClientePuc();
        FormularioImpuesto ObjFrmImpuesto = new FormularioImpuesto();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();

        #region DEFINICION DE METODOS
        public DataSet GetPuc()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjPuc.TipoConsulta = 1;
                ObjPuc.IdCliente = this.ViewState["IdCliente"].ToString().Trim();
                ObjPuc.IdEstado = 1;
                ObjPuc.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar el plan unico de cuenta
                ObjetoDataTable = ObjPuc.GetAllPuc();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_puc"] };
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar el plan unico de cuentas. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(230);
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

        public DataSet GetPucCliente()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjClientePuc.TipoConsulta = 1;
                ObjClientePuc.IdCliente = this.ViewState["IdCliente"].ToString().Trim();
                ObjClientePuc.NombreCuenta = null;
                ObjClientePuc.BaseGravable = null;
                ObjClientePuc.IdEstado = null;
                ObjClientePuc.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar las cuentas puc del cliente
                ObjetoDataTable = ObjClientePuc.GetClientePuc();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idcliente_puc"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUAURIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Error al listar las cuentas del cliente. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(230);
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

        protected void LstTipoImpuesto()
        {
            try
            {
                ObjFrmImpuesto.TipoConsulta = 2;
                ObjFrmImpuesto.IdEstado = 1;
                ObjFrmImpuesto.MostrarSeleccione = "SI";
                ObjFrmImpuesto.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbTipoImpuesto.DataSource = ObjFrmImpuesto.GetFormularioImpuesto();
                this.CmbTipoImpuesto.DataValueField = "idformulario_impuesto";
                this.CmbTipoImpuesto.DataTextField = "descripcion_formulario";
                this.CmbTipoImpuesto.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los tipos de clasificacion. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
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
                _log.Error(_MsgMensaje);
                #endregion
            }
        }

        private DataSet FuenteDatosPuc
        {
            get
            {
                object obj = this.ViewState["_FuenteDatosPuc"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetPuc();
                    this.ViewState["_FuenteDatosPuc"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatosPuc"];
                }
            }
            set { this.ViewState["_FuenteDatosPuc"] = value; }
        }

        private DataSet FuenteDatosAsignados
        {
            get
            {
                object obj = this.ViewState["_FuenteDatosClientePuc"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetPucCliente();
                    this.ViewState["_FuenteDatosClientePuc"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatosClientePuc"];
                }
            }
            set { this.ViewState["_FuenteDatosClientePuc"] = value; }
        }
        #endregion

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = this.ViewState["PathUrl"].ToString().Trim();
            objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

            //objPermiso.RefrescarPermisos();
            //if (!objPermiso.PuedeRegistrar)
            //{
            //    this.RadGrid1.Columns[RadGrid1.Columns.Count - 1].Visible = false;
            //}
            if (Int32.Parse(Session["IdRol"].ToString().Trim()) != 1)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 2].Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //--OBTENER VALOR DE PARAMETROS
                this.ViewState["IdCliente"] = Request.QueryString["IdCliente"].ToString().Trim();
                this.ViewState["PathUrl"] = Request.QueryString["PathUrl"].ToString().Trim();
                //--APLICAR LOS PERMISOS
                //this.AplicarPermisos();

                ///--Metodo para aplicar permisos
                if (this.Session["IdCliente"] != null)
                {
                    this.RadGrid1.Columns[RadGrid1.Columns.Count - 2].Visible = false;
                }

                this.LstTipoImpuesto();
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
                ObjUtils.CambiarGrillaAEspanol(RadGrid2);
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
                ObjUtils.CambiarGrillaAEspanol(RadGrid2);
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

        #region DEFINICION DE OBJETOS GRID 1
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid1.DataSource = this.FuenteDatosPuc;
                this.RadGrid1.DataMember = "DtPlanUnicoCuentas";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al Intentar Cargar el NeedDataSource, Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(230);
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
                string _NombreCuenta = "";
                if (e.CommandName == "BtnAsignar")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdPuc = Convert.ToInt32(item.GetDataKeyValue("id_puc").ToString().Trim());
                    _NombreCuenta = item["nombre_cuenta"].Text.ToString().Trim();
                    //_EstadoUsuario = item["codigo_estado"].Text.ToString().Trim();
                    string _IdFormulario = this.CmbTipoImpuesto.SelectedValue.ToString().Trim();

                    if (_IdFormulario.Length > 0)
                    {
                        #region ASIGNAR CODIGO DE CUENTA AL CLIENTE
                        //Asignamos el Cuentas al Comercio seleccionado
                        ObjClientePuc.IdClientePuc = null;
                        ObjClientePuc.IdCliente = Convert.ToInt32(this.ViewState["IdCliente"].ToString().Trim());
                        ObjClientePuc.IdFormularioImpuesto = _IdFormulario;
                        ObjClientePuc.IdPuc = _IdPuc;
                        ObjClientePuc.NombreCuenta = _NombreCuenta;
                        ObjClientePuc.BaseGravable = false;
                        ObjClientePuc.IdEstado = 1;
                        ObjClientePuc.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                        ObjClientePuc.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                        ObjClientePuc.TipoProceso = 1;

                        //--AQUI SERIALIZAMOS EL OBJETO CLASE
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string jsonRequest = js.Serialize(ObjClientePuc);
                        _log.Warn("REQUEST INSERT PUC CLIENTE => " + jsonRequest);

                        int _IdRegistro = 0;
                        string _MsgError = "";
                        if (ObjClientePuc.AddUpClientePuc(ref _IdRegistro, ref _MsgError))
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
                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                            Ventana.ID = "RadWindow2";
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(230);
                            Ventana.Width = Unit.Pixel(600);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Mensaje del Sistema";
                            Ventana.VisibleStatusbar = false;
                            Ventana.Behaviors = WindowBehaviors.Close;
                            this.RadWindowManager1.Windows.Add(Ventana);
                            this.RadWindowManager1 = null;
                            Ventana = null;
                            #endregion

                            #region REGISTRO DE LOGS DE AUDITORIA
                            //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                            ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                            ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                            ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                            ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                            ObjAuditoria.ModuloApp = "PUC_CLIENTE";
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

                            //this.ViewState["_FuenteDatosPuc"] = null;
                            //this.RadGrid1.Rebind();

                            this.ViewState["_FuenteDatosClientePuc"] = null;
                            this.RadGrid2.Rebind();
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
                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                            Ventana.ID = "RadWindow2";
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(230);
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
                        //Mostramos el mensaje porque se produjo un error con la Trx.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgError = "Señor usuario, debe seleccionar el tipo de formulario para la cuenta del Puc !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                        Ventana.ID = "RadWindow2";
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(230);
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con el evento ItemCommand de la grilla. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(230);
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
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con el evento RadGrid1_PageIndexChanged. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(230);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Error(_MsgMensaje);
            }
        }
        #endregion

        #region DEFINICION DE OBJETOS GRID 2
        protected void RadGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid2.DataSource = this.FuenteDatosAsignados;
                RadGrid2.DataMember = "DtClientePuc";
            }
            catch (Exception ex)
            {
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al Intentar Cargar el NeedDataSource, Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(230);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Error(_MsgMensaje);
            }
        }

        protected void RadGrid2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                string _NombreCuenta = "";
                string _EstadoUsuario = "";
                if (e.CommandName == "BtnBaseGravable")
                {
                    #region DEFINICION DE BASE GRAVABLE POR CODIGO DE CUENTA
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdClientePuc = Convert.ToInt32(item.GetDataKeyValue("idcliente_puc").ToString().Trim());
                    int _IdFormImpuesto = Convert.ToInt32(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    string _CodigoCuenta = item["codigo_cuenta"].Text.ToString().Trim();
                    string _NombreForm = item["descripcion_formulario"].Text.ToString().Trim();

                    //--Mandamos abrir el formulario de registro
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string _IdCliente = this.ViewState["IdCliente"].ToString().Trim();
                    string _PathUrl = this.ViewState["PathUrl"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddBaseGravable.aspx?IdClientePuc=" + _IdClientePuc + "&IdFormImpuesto=" + _IdFormImpuesto + "&CodigoCuenta=" + _CodigoCuenta + "&IdCliente=" + _IdCliente + "&NombreForm=" + _NombreForm + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow12";
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(570);
                    Ventana.Width = Unit.Pixel(1120);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Configurar Base Gravable Id: " + _IdClientePuc + " Cod. Cuenta: " + _CodigoCuenta;
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;

                    #endregion
                }
                else if (e.CommandName == "BtnDisociar")
                {
                    #region DEFINICION DE METODO PARA DISOCIAR LA CUENTA 
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdClientePuc = Convert.ToInt32(item.GetDataKeyValue("idcliente_puc").ToString().Trim());
                    int _IdFormImpuesto = Convert.ToInt32(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdPuc = Convert.ToInt32(item.GetDataKeyValue("id_puc").ToString().Trim());
                    int _IdEstado = Convert.ToInt32(item.GetDataKeyValue("id_estado").ToString().Trim());
                    _NombreCuenta = item["nombre_cuenta"].Text.ToString().Trim();
                    _EstadoUsuario = item["codigo_estado"].Text.ToString().Trim();

                    //Disociamos la cuenta seleccionada
                    ObjClientePuc.IdClientePuc = _IdClientePuc;
                    ObjClientePuc.IdCliente = Convert.ToInt32(this.ViewState["IdCliente"].ToString().Trim());
                    ObjClientePuc.IdFormularioImpuesto = _IdFormImpuesto;
                    ObjClientePuc.IdPuc = _IdPuc;
                    ObjClientePuc.NombreCuenta = _NombreCuenta;
                    ObjClientePuc.IdEstado = _IdEstado == 1 ? 0 : 1;
                    ObjClientePuc.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    ObjClientePuc.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                    ObjClientePuc.TipoProceso = 4;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjClientePuc);
                    _log.Warn("REQUEST DELETE PUC CLIENTE => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjClientePuc.AddUpClientePuc(ref _IdRegistro, ref _MsgError))
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
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                        Ventana.ID = "RadWindow2";
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(230);
                        Ventana.Width = Unit.Pixel(600);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Mensaje del Sistema";
                        Ventana.VisibleStatusbar = false;
                        Ventana.Behaviors = WindowBehaviors.Close;
                        this.RadWindowManager1.Windows.Add(Ventana);
                        this.RadWindowManager1 = null;
                        Ventana = null;
                        #endregion

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 4;  //--ELIMINAR
                        ObjAuditoria.ModuloApp = "PUC_CLIENTE";
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

                        //this.ViewState["_FuenteDatosPuc"] = null;
                        //this.RadGrid1.Rebind();

                        this.ViewState["_FuenteDatosClientePuc"] = null;
                        this.RadGrid2.Rebind();
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
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                        Ventana.ID = "RadWindow2";
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(230);
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
                else if (e.CommandName == "BtnBloquer")
                {
                    #region DEFINICION DE METODO PARA BLOQUEAR LA CUENTA 
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdClientePuc = Convert.ToInt32(item.GetDataKeyValue("idcliente_puc").ToString().Trim());
                    int _IdFormImpuesto = Convert.ToInt32(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdPuc = Convert.ToInt32(item.GetDataKeyValue("id_puc").ToString().Trim());
                    int _IdEstado = Convert.ToInt32(item.GetDataKeyValue("id_estado").ToString().Trim());
                    _NombreCuenta = item["nombre_cuenta"].Text.ToString().Trim();
                    _EstadoUsuario = item["codigo_estado"].Text.ToString().Trim();

                    //Disociamos la cuenta seleccionada
                    ObjClientePuc.IdClientePuc = _IdClientePuc;
                    ObjClientePuc.IdCliente = Convert.ToInt32(this.ViewState["IdCliente"].ToString().Trim());
                    ObjClientePuc.IdFormularioImpuesto = _IdFormImpuesto;
                    ObjClientePuc.IdPuc = _IdPuc;
                    ObjClientePuc.NombreCuenta = _NombreCuenta;
                    ObjClientePuc.IdEstado = _IdEstado == 1 ? 0 : 1;
                    ObjClientePuc.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    ObjClientePuc.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                    ObjClientePuc.TipoProceso = 3;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjClientePuc);
                    _log.Warn("REQUEST UPDATE PUC CLIENTE => " + jsonRequest);

                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    object routes_list = json_serializer.DeserializeObject(jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjClientePuc.AddUpClientePuc(ref _IdRegistro, ref _MsgError))
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
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                        Ventana.ID = "RadWindow2";
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(230);
                        Ventana.Width = Unit.Pixel(600);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Mensaje del Sistema";
                        Ventana.VisibleStatusbar = false;
                        Ventana.Behaviors = WindowBehaviors.Close;
                        this.RadWindowManager1.Windows.Add(Ventana);
                        this.RadWindowManager1 = null;
                        Ventana = null;
                        #endregion

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                        ObjAuditoria.ModuloApp = "PUC_CLIENTE";
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

                        //this.ViewState["_FuenteDatosPuc"] = null;
                        //this.RadGrid1.Rebind();

                        this.ViewState["_FuenteDatosClientePuc"] = null;
                        this.RadGrid2.Rebind();
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
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                        Ventana.ID = "RadWindow2";
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(230);
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
                else if (e.CommandName == "BtnLogsAuditoria")
                {
                    #region DEFINICION DEL EVENTO CLICK PARA EDITAR IMPUESTO
                    //--MANDAMOS ABRIR EL FORM COMO POPUP
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string _ModuloApp = "PUC_CLIENTE";
                    string _PathUrl = this.ViewState["PathUrl"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Seguridad/FrmLogsAuditoria.aspx?ModuloApp=" + _ModuloApp + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow12";
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(550);
                    Ventana.Width = Unit.Pixel(1100);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Detalle Logs de Auditoria. Modulo: " + _ModuloApp;
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
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
                string _MsgMensaje = "Señor usuario, Error con el evento ItemCommand de la grilla. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(230);
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

        protected void RadGrid2_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            try
            {
                RadGrid2.Rebind();
            }
            catch (Exception ex)
            {
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con el evento RadGrid1_PageIndexChanged. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(230);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Error(_MsgMensaje);
            }
        }

        protected void RadGrid2_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatosAsignados.Tables["DtClientePuc"];
            //TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idcliente_puc, idformulario_impuesto, id_puc, id_estado"] };
            //DataRow[] changedRows = TablaDatos.Select("idcliente_puc = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["idcliente_puc"].ToString()));
            int _IdClientePuc = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["idcliente_puc"].ToString().Trim());
            DataRow[] changedRows = TablaDatos.Select("idcliente_puc = '" + _IdClientePuc + "'");
            //int _IdClientePuc = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["idcliente_puc"].ToString().Trim());

            if (changedRows.Length != 1)
            {
                e.Canceled = true;
                return;
            }

            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);
            changedRows[0].BeginEdit();

            try
            {
                foreach (DictionaryEntry entry in newValues)
                {
                    changedRows[0][(string)entry.Key] = entry.Value;
                }

                ObjClientePuc.IdClientePuc = _IdClientePuc;
                ObjClientePuc.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                ObjClientePuc.NombreCuenta = changedRows[0]["nombre_cuenta"].ToString().Trim();
                ObjClientePuc.BaseGravable = Convert.ToBoolean(changedRows[0]["base_gravable"].ToString().Trim());
                ObjClientePuc.IdPuc = null;
                ObjClientePuc.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjClientePuc.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjClientePuc.TipoProceso = 2;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjClientePuc);
                _log.Warn("REQUEST UPDATE PUC CLIENTE => " + jsonRequest);

                int _IdRegistro = 0;
                string _MsgError = "";
                if (ObjClientePuc.AddUpClientePuc(changedRows[0], ref _IdRegistro, ref _MsgError))
                {
                    changedRows[0]["nombre_cuenta"] = changedRows[0]["nombre_cuenta"].ToString().Trim();
                    changedRows[0]["base_gravable"] = Convert.ToBoolean(changedRows[0]["base_gravable"].ToString().Trim());
                    this.FuenteDatosAsignados.Tables["DtClientePuc"].Rows[0].AcceptChanges();
                    this.FuenteDatosAsignados.Tables["DtClientePuc"].Rows[0].EndEdit();

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                    ObjAuditoria.ModuloApp = "PUC_CLIENTE";
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
                    Ventana.ID = "RadWindow2";
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
                    _log.Info(_MsgError);
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
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                    Ventana.ID = "RadWindow2";
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
                    _log.Error(_MsgError);
                    #endregion
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
                string _MsgError = "Error al al asignar el dispositivo al usuario. Motivo: " + ex.Message;
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow2";
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
                _log.Error(_MsgError);
                e.Canceled = true;
                #endregion
            }
        }
        #endregion

    }
}