using System;
using System.Data;
using Telerik.Web.UI;
using log4net;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Parametros.Divipola;

namespace Smartax.Web.Application.Controles.Administracion.EjecucionLote
{
    public partial class FrmAddFiltrosEjecucionXLote : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        EjecucionXLoteFiltros ObjEjecFiltros = new EjecucionXLoteFiltros();
        FormularioImpuesto ObjFrmImpuesto = new FormularioImpuesto();
        Departamento ObjDpto = new Departamento();
        Municipio ObjMunicipio = new Municipio();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();

        #region DEFINICION DE METODOS
        public DataSet GetMunicipiosXDpto()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjMunicipio.TipoConsulta = 1;
                ObjMunicipio.IdDpto = this.CmbDpto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbDpto.SelectedValue.ToString().Trim() : "-1";
                ObjMunicipio.IdMunicipio = null;
                ObjMunicipio.IdEstado = 1;
                ObjMunicipio.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar el plan unico de cuenta
                ObjetoDataTable = ObjMunicipio.GetAllMunicipios();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_municipio"] };
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los municipios del departamento. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(530);
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

        public DataSet GetFiltrosPorEjecucion()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjEjecFiltros.TipoConsulta = 1;
                ObjEjecFiltros.IdEjecucionLote = this.ViewState["IdEjecucionLote"].ToString().Trim();
                ObjEjecFiltros.AnioGravable = null;
                ObjEjecFiltros.IdFormImpuesto = null;
                ObjEjecFiltros.IdDepartamento = null;
                ObjEjecFiltros.IdMunicipio = null;
                ObjEjecFiltros.IdEstado = null;
                ObjEjecFiltros.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar las cuentas puc del cliente
                ObjetoDataTable = ObjEjecFiltros.GetAllEjecucionXLoteFiltro();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idejec_lote_filtro"] };
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
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(530);
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

        protected void LstDepartamentos()
        {
            try
            {
                ObjDpto.TipoConsulta = 2;
                ObjDpto.MostrarSeleccione = "SI";
                ObjDpto.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjDpto.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                this.CmbDpto.DataSource = ObjDpto.GetDptos();
                this.CmbDpto.DataValueField = "id_dpto";
                this.CmbDpto.DataTextField = "nombre_departamento";
                this.CmbDpto.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los departamentos. Motivo: " + ex.ToString();
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
                _log.Error(_MsgMensaje);
                #endregion
            }
        }

        private DataSet FuenteDatosMunicipios
        {
            get
            {
                object obj = this.ViewState["_FuenteDatosMunicipios"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetMunicipiosXDpto();
                    this.ViewState["_FuenteDatosMunicipios"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatosMunicipios"];
                }
            }
            set { this.ViewState["_FuenteDatosMunicipios"] = value; }
        }

        private DataSet FuenteDatosAsignados
        {
            get
            {
                object obj = this.ViewState["_FuenteDatosFiltros"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetFiltrosPorEjecucion();
                    this.ViewState["_FuenteDatosFiltros"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatosFiltros"];
                }
            }
            set { this.ViewState["_FuenteDatosFiltros"] = value; }
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
                //--CAMBIAR IDIOMA A ESPAÑOL LOS FILTROS DEL GRID
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
                ObjUtils.CambiarGrillaAEspanol(RadGrid2);

                //--OBTENER VALOR DE PARAMETROS
                this.ViewState["IdEjecucionLote"] = Request.QueryString["IdEjecucionLote"].ToString().Trim();
                this.ViewState["PathUrl"] = Request.QueryString["PathUrl"].ToString().Trim();
                //--APLICAR LOS PERMISOS
                //this.AplicarPermisos();

                ///--Metodo para aplicar permisos
                if (this.Session["IdCliente"] != null)
                {
                    this.RadGrid1.Columns[RadGrid1.Columns.Count - 2].Visible = false;
                }

                this.LstTipoImpuesto();
                this.LstDepartamentos();
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
        protected void CmbDpto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;
                //--
                this.ViewState["_FuenteDatosMunicipios"] = null;
                this.RadGrid1.Rebind();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void BtnAddDpto_Click(object sender, EventArgs e)
        {
            try
            {
                #region REGISTRAR SOLO EL DEPARTAMENTO SELECCIONADO
                //--AQUI VALIDAMOS QUE ALLA SELECCIONA UN TIPO DE IMPUESTO
                int _IdTipoImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? Int32.Parse(this.CmbTipoImpuesto.SelectedValue.ToString().Trim()) : -1;

                if (_IdTipoImpuesto > 0)
                {
                    //--VALIDAR SI HAY SELECCIONADO UN DPTO 
                    if (this.CmbDpto.SelectedValue.ToString().Trim().Length > 0)
                    {
                        //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                        this.RadWindowManager1.Enabled = false;
                        this.RadWindowManager1.EnableAjaxSkinRendering = false;
                        this.RadWindowManager1.Visible = false;

                        //--ADICIONAMOS EL REGISTRO EN LA DB
                        ObjEjecFiltros.IdEjecucionLoteFiltro = null;
                        ObjEjecFiltros.IdEjecucionLote = Convert.ToInt32(this.ViewState["IdEjecucionLote"].ToString().Trim());
                        ObjEjecFiltros.IdFormImpuesto = _IdTipoImpuesto;
                        ObjEjecFiltros.IdDepartamento = this.CmbDpto.SelectedValue.ToString().Trim();
                        ObjEjecFiltros.IdMunicipio = null;
                        ObjEjecFiltros.IdEstado = 1;
                        ObjEjecFiltros.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjEjecFiltros.TipoProceso = 1;

                        //--AQUI SERIALIZAMOS EL OBJETO CLASE
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string jsonRequest = js.Serialize(ObjEjecFiltros);
                        _log.Warn("REQUEST INSERT FILTRO EJECUCION LOTE => " + jsonRequest);

                        int _IdRegistro = 0;
                        string _MsgError = "";
                        if (ObjEjecFiltros.AddUpEjecucionXLoteFiltro(ref _IdRegistro, ref _MsgError))
                        {
                            this.CmbDpto.SelectedValue = "";
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

                            #region REGISTRO DE LOGS DE AUDITORIA
                            //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                            ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                            ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                            ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                            ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                            ObjAuditoria.ModuloApp = "EJECUCION_LOTE_FILTRO";
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

                            //this.ViewState["_FuenteDatosMunicipios"] = null;
                            //this.RadGrid1.Rebind();

                            this.ViewState["_FuenteDatosFiltros"] = null;
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
                        //Mostramos el mensaje porque se produjo un error con la Trx.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgError = "Señor usuario, debe seleccionar un departamento de la lista !";
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
                    //Mostramos el mensaje porque se produjo un error con la Trx.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string _MsgError = "Señor usuario, debe seleccionar un tipo de impuesto de la lista !";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(300);
                    Ventana.Width = Unit.Pixel(530);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Mensaje del Sistema";
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    this.CmbTipoImpuesto.Focus();
                    #endregion
                }
                #endregion
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al adicionar el departamento. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(530);
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

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid1.DataSource = this.FuenteDatosMunicipios;
                this.RadGrid1.DataMember = "DtMunicipios";
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
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(530);
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
                string _NombreMunicipio = "";
                if (e.CommandName == "BtnAsignar")
                {
                    #region ASIGNAR EL MUNICIPIO AL FILTRO
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdMunicipio = Convert.ToInt32(item.GetDataKeyValue("id_municipio").ToString().Trim());
                    int _IdDepartamento = Convert.ToInt32(item.GetDataKeyValue("id_dpto").ToString().Trim());
                    _NombreMunicipio = item["nombre_municipio"].Text.ToString().Trim();
                    //--AQUI VALIDAMOS QUE ALLA SELECCIONA UN TIPO DE IMPUESTO
                    int _IdTipoImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? Int32.Parse(this.CmbTipoImpuesto.SelectedValue.ToString().Trim()) : -1;

                    if (_IdTipoImpuesto > 0)
                    {
                        //--ADICIONAMOS EL REGISTRO EN LA DB
                        ObjEjecFiltros.IdEjecucionLoteFiltro = null;
                        ObjEjecFiltros.IdEjecucionLote = Convert.ToInt32(this.ViewState["IdEjecucionLote"].ToString().Trim());
                        ObjEjecFiltros.IdFormImpuesto = _IdTipoImpuesto;
                        ObjEjecFiltros.IdDepartamento = _IdDepartamento;
                        ObjEjecFiltros.IdMunicipio = _IdMunicipio;
                        ObjEjecFiltros.Observacion = null;
                        ObjEjecFiltros.IdEstado = 1;
                        ObjEjecFiltros.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjEjecFiltros.TipoProceso = 1;

                        //--AQUI SERIALIZAMOS EL OBJETO CLASE
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string jsonRequest = js.Serialize(ObjEjecFiltros);
                        _log.Warn("REQUEST INSERT FILTRO EJECUCION LOTE => " + jsonRequest);

                        int _IdRegistro = 0;
                        string _MsgError = "";
                        if (ObjEjecFiltros.AddUpEjecucionXLoteFiltro(ref _IdRegistro, ref _MsgError))
                        {
                            #region REGISTRO DE LOGS DE AUDITORIA
                            //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                            ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                            ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                            ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                            ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                            ObjAuditoria.ModuloApp = "EJECUCION_LOTE_FILTRO";
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
                            this.CmbDpto.SelectedValue = "";
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
                            Ventana.Width = Unit.Pixel(530);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Mensaje del Sistema";
                            Ventana.VisibleStatusbar = false;
                            Ventana.Behaviors = WindowBehaviors.Close;
                            this.RadWindowManager1.Windows.Add(Ventana);
                            this.RadWindowManager1 = null;
                            Ventana = null;
                            //--
                            this.ViewState["_FuenteDatosFiltros"] = null;
                            this.RadGrid2.Rebind();
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
                            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(300);
                            Ventana.Width = Unit.Pixel(530);
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
                        //Mostramos el mensaje porque se produjo un error con la Trx.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgError = "Señor usuario, debe seleccionar un tipo de impuesto de la lista !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(300);
                        Ventana.Width = Unit.Pixel(530);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Mensaje del Sistema";
                        Ventana.VisibleStatusbar = false;
                        Ventana.Behaviors = WindowBehaviors.Close;
                        this.RadWindowManager1.Windows.Add(Ventana);
                        this.RadWindowManager1 = null;
                        Ventana = null;
                        this.CmbTipoImpuesto.Focus();
                        #endregion
                    }
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con el evento ItemCommand de la grilla. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(530);
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
                this.RadGrid1.Rebind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con el evento RadGrid1_PageIndexChanged. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(530);
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

        #region DEFINICION DE OBJETOS GRID 2
        protected void RadGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid2.DataSource = this.FuenteDatosAsignados;
                this.RadGrid2.DataMember = "DtEjecucionXLoteFiltro";
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
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(530);
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

        protected void RadGrid2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                string _EstadoUsuario = "";
                if (e.CommandName == "BtnBaseGravable")
                {
                    #region DEFINICION DE BASE GRAVABLE POR CODIGO DE CUENTA
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdClientePuc = Convert.ToInt32(item.GetDataKeyValue("idejec_lote_filtro").ToString().Trim());
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
                    int _IdEjecLoteFiltro = Convert.ToInt32(item.GetDataKeyValue("idejec_lote_filtro").ToString().Trim());
                    int _IdTipoImpuesto = Convert.ToInt32(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdDpto = Convert.ToInt32(item.GetDataKeyValue("id_dpto").ToString().Trim());
                    int _IdEstado = Convert.ToInt32(item.GetDataKeyValue("idestado_filtro").ToString().Trim());
                    _EstadoUsuario = item["estado_filtro"].Text.ToString().Trim();

                    //Disociamos la cuenta seleccionada
                    ObjEjecFiltros.IdEjecucionLoteFiltro = _IdEjecLoteFiltro;
                    ObjEjecFiltros.IdEjecucionLote = Convert.ToInt32(this.ViewState["IdEjecucionLote"].ToString().Trim());
                    ObjEjecFiltros.IdFormImpuesto = _IdTipoImpuesto;
                    ObjEjecFiltros.IdDepartamento = _IdDpto;
                    ObjEjecFiltros.IdMunicipio = null;
                    ObjEjecFiltros.Observacion = null;
                    ObjEjecFiltros.IdEstado = _IdEstado == 1 ? 0 : 1;
                    ObjEjecFiltros.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjEjecFiltros.TipoProceso = 3;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjEjecFiltros);
                    _log.Warn("REQUEST DELETE FILTRO EJECUCION LOTE => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjEjecFiltros.AddUpEjecucionXLoteFiltro(ref _IdRegistro, ref _MsgError))
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
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(300);
                        Ventana.Width = Unit.Pixel(530);
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
                        ObjAuditoria.ModuloApp = "EJECUCION_LOTE_FILTRO";
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

                        this.ViewState["_FuenteDatosFiltros"] = null;
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
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(300);
                        Ventana.Width = Unit.Pixel(530);
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
                    #region DEFINICION DE METODO PARA DISOCIAR EL FILTRO
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdEjecLoteFiltro = Convert.ToInt32(item.GetDataKeyValue("idejec_lote_filtro").ToString().Trim());
                    int _IdTipoImpuesto = Convert.ToInt32(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdDpto = Convert.ToInt32(item.GetDataKeyValue("id_dpto").ToString().Trim());
                    int _IdEstado = Convert.ToInt32(item.GetDataKeyValue("idestado_filtro").ToString().Trim());
                    _EstadoUsuario = item["estado_filtro"].Text.ToString().Trim();

                    //Disociamos la cuenta seleccionada
                    ObjEjecFiltros.IdEjecucionLoteFiltro = _IdEjecLoteFiltro;
                    ObjEjecFiltros.IdEjecucionLote = Convert.ToInt32(this.ViewState["IdEjecucionLote"].ToString().Trim());
                    ObjEjecFiltros.IdFormImpuesto = _IdTipoImpuesto;
                    ObjEjecFiltros.IdDepartamento = _IdDpto;
                    ObjEjecFiltros.IdMunicipio = null;
                    ObjEjecFiltros.Observacion = null;
                    ObjEjecFiltros.IdEstado = _IdEstado == 1 ? 0 : 1;
                    ObjEjecFiltros.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjEjecFiltros.TipoProceso = 4;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjEjecFiltros);
                    _log.Warn("REQUEST BLOQUEO FILTRO EJECUCION LOTE => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjEjecFiltros.AddUpEjecucionXLoteFiltro(ref _IdRegistro, ref _MsgError))
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
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(300);
                        Ventana.Width = Unit.Pixel(530);
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
                        ObjAuditoria.ModuloApp = "EJECUCION_LOTE_FILTRO";
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

                        //this.ViewState["_FuenteDatosMunicipios"] = null;
                        //this.RadGrid1.Rebind();

                        this.ViewState["_FuenteDatosFiltros"] = null;
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
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(300);
                        Ventana.Width = Unit.Pixel(530);
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
                else if (e.CommandName == "BtnAprobacion")
                {
                    #region DEFINICION DE METODO PARA REALIZAR APROBACIÓN O RECHAZO DE LA DECLARACION
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdEjecLoteFiltro = Convert.ToInt32(item.GetDataKeyValue("idejec_lote_filtro").ToString().Trim());
                    int _IdTipoImpuesto = Convert.ToInt32(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdDpto = Convert.ToInt32(item.GetDataKeyValue("id_dpto").ToString().Trim());
                    int _IdEstadoFiltro = Convert.ToInt32(item.GetDataKeyValue("idestado_filtro").ToString().Trim());
                    string _IdEstadoDeclaracion = item.GetDataKeyValue("idestado_declaracion").ToString().Trim();
                    _EstadoUsuario = item["estado_filtro"].Text.ToString().Trim();

                    if (_IdEstadoFiltro == 1)
                    {
                        //--AQUI VALIDAMOS QUE EL ESTADO DE LA DECLARACION ESTE VACIO O EN ESTADO PRELIMINAR
                        //if (_IdEstadoDeclaracion.ToString().Trim().Length == 0 || _IdEstadoDeclaracion.ToString().Trim().Equals("2"))
                        if (_IdEstadoDeclaracion.ToString().Trim().Equals("2"))
                        {
                            #region MOSTRAR EL FORM DE APLICACION DE ESTADO
                            //--AQUI HABILITAMOS EL POPUP PARA EL REGISTRO DE INFORMACION
                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;
                            Ventana.Modal = true;

                            string _PathUrl = this.ViewState["PathUrl"].ToString().Trim();
                            Ventana.NavigateUrl = "/Controles/Administracion/EjecucionLote/FrmAprobarRechazarDeclaracion.aspx?IdEjecLoteFiltro=" + _IdEjecLoteFiltro + "&IdEjecucionLote=" + this.ViewState["IdEjecucionLote"].ToString().Trim() + "&IdTipoImpuesto=" + _IdTipoImpuesto + "&IdDpto=" + _IdDpto + "&PathUrl=" + _PathUrl;
                            Ventana.ID = "RadWindow3";
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(400);
                            Ventana.Width = Unit.Pixel(800);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Aplicar Estado Declaración del Id Filtro: " + _IdEjecLoteFiltro;
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
                            //Mostramos el mensaje porque se produjo un error con la Trx.
                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;

                            RadWindow Ventana = new RadWindow();
                            Ventana.Modal = true;
                            string _MsgMensaje = "Señor usuario, El estado de la declaración debe encontrarse en estado PRELIMINAR para realizar este proceso. Por favor validar e intenlo nuevamente !";
                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(300);
                            Ventana.Width = Unit.Pixel(530);
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
                        //Mostramos el mensaje porque se produjo un error con la Trx.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgMensaje = "Señor usuario, Este filtro se encuentra en estado diferente al ACTIVO. Por favor validar e intenlo nuevamente !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(300);
                        Ventana.Width = Unit.Pixel(530);
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

                    string _ModuloApp = "EJECUCION_LOTE_FILTRO";
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
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(530);
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
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(530);
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
    }
}