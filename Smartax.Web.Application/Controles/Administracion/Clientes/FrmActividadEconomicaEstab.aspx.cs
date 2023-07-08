using System;
using System.Data;
using Telerik.Web.UI;
using log4net;
using System.Drawing;
using System.Web.Caching;
using System.Web.UI.WebControls;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Parametros.Divipola;
using System.Collections;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Administracion.Clientes
{
    public partial class FrmActividadEconomicaEstab : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        MunicipioActividadEconomica ObjMunActividad = new MunicipioActividadEconomica();
        ClienteEstablecimiento ObjEstablecimiento = new ClienteEstablecimiento();
        ClientePuc ObjClientePuc = new ClientePuc();
        Combox ObjLista = new Combox();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();

        #region DEFINICION DE METODOS
        public DataSet GetActividadesEconSinAsignar()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjMunActividad.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                ObjMunActividad.IdClienteEstablecimiento = this.ViewState["IdClienteEstablecimiento"].ToString().Trim();
                ObjMunActividad.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar las actividades economicas del municipio o establecimiento del cliente
                ObjetoDataTable = ObjMunActividad.GetActEconomicaSinAsignar();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idmun_act_economica"] };
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
                string _MsgError = "Error al listar los Actividad economicas. Motivo: " + ex.ToString();
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

            return ObjetoDataSet;
        }

        public DataSet GetActividadesEconAsignadas()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjEstablecimiento.TipoConsulta = 1;
                ObjEstablecimiento.IdClienteEstablecimiento = this.ViewState["IdClienteEstablecimiento"].ToString().Trim();
                ObjEstablecimiento.IdEstado = null;
                ObjEstablecimiento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar las actividades economicas del municipio o establecimiento del cliente
                ObjetoDataTable = ObjEstablecimiento.GetActEconomicaAsignadas();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idestab_act_economica"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los tipos de calculos
                ObjetoDataTable = new DataTable();
                ObjetoDataTable = ObjLista.GetTipoCalculo();
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
                string _MsgError = "Error al listar los Actividad economicas del establecimiento. Motivo: " + ex.ToString();
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
                ObjClientePuc.CodigoCuenta = this.TxtCodCuenta.Text.ToString().Trim().Length > 0 ? this.TxtCodCuenta.Text.ToString().Trim().ToUpper() : null;
                ObjClientePuc.NombreCuenta = this.TxtNombreCuenta.Text.ToString().Trim().Length > 0 ? this.TxtNombreCuenta.Text.ToString().Trim().ToUpper() : null;
                ObjClientePuc.BaseGravable = "S";
                ObjClientePuc.IdEstado = null;
                ObjClientePuc.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar las cuentas puc del cliente
                ObjetoDataTable = ObjClientePuc.GetClientePuc();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idcliente_puc, id_puc"] };
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

        private DataSet FuenteDatosActEconSinAsignar
        {
            get
            {
                object obj = this.ViewState["_FuenteDatosActEconSinAsignar"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetActividadesEconSinAsignar();
                    this.ViewState["_FuenteDatosActEconSinAsignar"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatosActEconSinAsignar"];
                }
            }
            set { this.ViewState["_FuenteDatosActEconSinAsignar"] = value; }
        }

        private DataSet FuenteDatosActEconAsignadas
        {
            get
            {
                object obj = this.ViewState["_FuenteDatosActEconAsignadas"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetActividadesEconAsignadas();
                    this.ViewState["_FuenteDatosActEconAsignadas"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatosActEconAsignadas"];
                }
            }
            set { this.ViewState["_FuenteDatosActEconAsignadas"] = value; }
        }

        private DataSet FuenteDatosPucCliente
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

            objNavegacion.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = this.ViewState["PathUrl"].ToString().Trim();
            objPermiso.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

            //objPermiso.RefrescarPermisos();
            //if (!objPermiso.PuedeRegistrar)
            //{
            //    this.RadGrid1.Columns[RadGrid1.Columns.Count - 1].Visible = false;
            //}
            //if (!objPermiso.PuedeModificar)
            //{
            //    this.RadGrid2.Columns[RadGrid2.Columns.Count - 1].Visible = false;
            //}
            //if (!objPermiso.PuedeBloquear)
            //{
            //    this.RadGrid2.Columns[RadGrid2.Columns.Count - 2].Visible = false;
            //}

            //if (Int32.Parse(Session["IdRol"].ToString().Trim()) != 1)
            //{
            //    this.RadGrid1.Columns[RadGrid1.Columns.Count - 3].Visible = false;

            //}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //--Aqui capturamos los parametros.
                this.ViewState["IdClienteEstablecimiento"] = Request.QueryString["IdClienteEstablecimiento"].ToString().Trim();
                this.ViewState["IdMunicipio"] = Request.QueryString["IdMunicipio"].ToString().Trim();
                this.ViewState["IdCliente"] = Request.QueryString["IdCliente"].ToString().Trim();
                this.ViewState["PathUrl"] = Request.QueryString["PathUrl"].ToString().Trim();

                //this.AplicarPermisos();
                //ObjUtils.CambiarGrillaAEspanol(RadGrid1);
                //ObjUtils.CambiarGrillaAEspanol(RadGrid2);
            }
            else
            {
                //ObjUtils.CambiarGrillaAEspanol(RadGrid1);
                //ObjUtils.CambiarGrillaAEspanol(RadGrid2);
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
                this.RadGrid1.DataSource = this.FuenteDatosActEconSinAsignar;
                this.RadGrid1.DataMember = "DtActEconomicaSinAsignar";
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
                if (e.CommandName == "BtnAsignar")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdMunActEconomica = Convert.ToInt32(item.GetDataKeyValue("idmun_act_economica").ToString().Trim());
                    //_NombreCuenta = item["nombre_cuenta"].Text.ToString().Trim();

                    #region ASIGNAR CODIGO DE CUENTA AL CLIENTE
                    //Asignamos el Cuentas al Comercio seleccionado
                    ObjEstablecimiento.IdEstabActEconomica = null;
                    ObjEstablecimiento.IdClienteEstablecimiento = Convert.ToInt32(this.ViewState["IdClienteEstablecimiento"].ToString().Trim());
                    ObjEstablecimiento.IdMunActEconomica = _IdMunActEconomica;
                    ObjEstablecimiento.IdPuc = null;
                    ObjEstablecimiento.IdTipoCalculo = 2;   //--POR DEFAULT QUEDA PARA CALCULAR CON LA TARIFA DEL MUNICIPIO
                    ObjEstablecimiento.IdEstado = 1;
                    ObjEstablecimiento.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    ObjEstablecimiento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjEstablecimiento.TipoProceso = 1;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjEstablecimiento);
                    _log.Warn("REQUEST INSERT ACT. ECON. OFICINA CLIENTE => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjEstablecimiento.AddActEconEstablecimiento(ref _IdRegistro, ref _MsgError))
                    {
                        //--AQUI BORRAMOS LA CACHE
                        string _CacheCuentasCliente = FixedData.GetCacheCuentasCliente + this.ViewState["IdCliente"].ToString().Trim();
                        Cache.Remove(_CacheCuentasCliente);

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
                        #endregion

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                        ObjAuditoria.ModuloApp = "ACT_ECON_OFICINA_CLIENTE";
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

                        this.ViewState["_FuenteDatosActEconSinAsignar"] = null;
                        this.RadGrid1.Rebind();

                        this.ViewState["_FuenteDatosActEconAsignadas"] = null;
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
            }
        }
        #endregion

        #region DEFINICION DE OBJETOS GRID 2
        protected void RadGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid2.DataSource = this.FuenteDatosActEconAsignadas;
                this.RadGrid2.DataMember = "DtActEconomicaAsignadas";

                GridDropDownColumn columna = new GridDropDownColumn();
                columna = (GridDropDownColumn)this.RadGrid2.Columns[10];
                columna.DataSourceID = this.RadGrid2.DataSourceID;
                columna.HeaderText = "Tipo Calculo";
                columna.DataField = "idtipo_calculo";
                columna.ListTextField = "tipo_calculo";
                columna.ListValueField = "idtipo_calculo";
                columna.ListDataMember = "DtTipoCalculo";
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

        protected void RadGrid2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                string _EstadoUsuario = "";
                if (e.CommandName == "BtnDisociar")
                {
                    #region DEFINICION DE METODO PARA DISOCIAR LA CUENTA 
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdEstabActEconomica = Convert.ToInt32(item.GetDataKeyValue("idestab_act_economica").ToString().Trim());
                    int _IdMunActEconomica = Convert.ToInt32(item.GetDataKeyValue("idmun_act_economica").ToString().Trim());
                    int _IdEstado = Convert.ToInt32(item.GetDataKeyValue("id_estado").ToString().Trim());
                    _EstadoUsuario = item["codigo_estado"].Text.ToString().Trim();

                    //Disociamos la cuenta seleccionada
                    ObjEstablecimiento.IdEstabActEconomica = _IdEstabActEconomica;
                    ObjEstablecimiento.IdClienteEstablecimiento = Convert.ToInt32(this.ViewState["IdClienteEstablecimiento"].ToString().Trim());
                    ObjEstablecimiento.IdMunActEconomica = _IdMunActEconomica;
                    ObjEstablecimiento.IdPuc = null;
                    ObjEstablecimiento.IdEstado = _IdEstado == 1 ? 0 : 1;
                    ObjEstablecimiento.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    ObjEstablecimiento.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                    ObjEstablecimiento.TipoProceso = 4;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjEstablecimiento);
                    _log.Warn("REQUEST DELETE ACT. ECON. OFICINA CLIENTE => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjEstablecimiento.AddActEconEstablecimiento(ref _IdRegistro, ref _MsgError))
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
                        #endregion

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 4;  //--DELETE
                        ObjAuditoria.ModuloApp = "ACT_ECON_OFICINA_CLIENTE";
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

                        this.ViewState["_FuenteDatosActEconSinAsignar"] = null;
                        this.RadGrid1.Rebind();

                        this.ViewState["_FuenteDatosActEconAsignadas"] = null;
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
                else if (e.CommandName == "BtnCuenta")
                {
                    #region DEFINICION DE METODO PARA BLOQUEAR LA CUENTA 
                    //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                    this.RadWindowManager1.Enabled = false;
                    this.RadWindowManager1.EnableAjaxSkinRendering = false;
                    this.RadWindowManager1.Visible = false;

                    GridDataItem item = (GridDataItem)e.Item;
                    this.ViewState["IdEstabActEconomica"] = Convert.ToInt32(item.GetDataKeyValue("idestab_act_economica").ToString().Trim());
                    this.ViewState["IdMunActEconomica"] = Convert.ToInt32(item.GetDataKeyValue("idmun_act_economica").ToString().Trim());
                    this.ViewState["IdEstado"] = Convert.ToInt32(item.GetDataKeyValue("id_estado").ToString().Trim());

                    this.LblMensaje.Text = "";
                    this.ModalPopupExtender1.Show();
                    this.RadGrid4.Rebind();
                    #endregion
                }
                else if (e.CommandName == "BtnBloquer")
                {
                    #region DEFINICION DE METODO PARA BLOQUEAR LA CUENTA 
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdEstabActEconomica = Convert.ToInt32(item.GetDataKeyValue("idestab_act_economica").ToString().Trim());
                    int _IdMunActEconomica = Convert.ToInt32(item.GetDataKeyValue("idmun_act_economica").ToString().Trim());
                    int _IdEstado = Convert.ToInt32(item.GetDataKeyValue("id_estado").ToString().Trim());
                    _EstadoUsuario = item["codigo_estado"].Text.ToString().Trim();

                    //Disociamos la cuenta seleccionada
                    ObjEstablecimiento.IdEstabActEconomica = _IdEstabActEconomica;
                    ObjEstablecimiento.IdClienteEstablecimiento = Convert.ToInt32(this.ViewState["IdClienteEstablecimiento"].ToString().Trim());
                    ObjEstablecimiento.IdMunActEconomica = _IdMunActEconomica;
                    ObjEstablecimiento.IdEstado = _IdEstado == 1 ? 0 : 1;
                    ObjEstablecimiento.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    ObjEstablecimiento.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                    ObjEstablecimiento.TipoProceso = 2;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjEstablecimiento);
                    _log.Warn("REQUEST UPDATE ACT. ECON. OFICINA CLIENTE => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjEstablecimiento.AddActEconEstablecimiento(ref _IdRegistro, ref _MsgError))
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
                        #endregion

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                        ObjAuditoria.ModuloApp = "ACT_ECON_OFICINA_CLIENTE";
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

                        this.ViewState["_FuenteDatosActEconSinAsignar"] = null;
                        this.RadGrid1.Rebind();

                        this.ViewState["_FuenteDatosActEconAsignadas"] = null;
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
                else if (e.CommandName == "BtnDuplicar")
                {
                    #region DEFINICION DE METODO PARA BLOQUEAR LA CUENTA 
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdEstabActEconomica = Convert.ToInt32(item.GetDataKeyValue("idestab_act_economica").ToString().Trim());
                    int _IdMunActEconomica = Convert.ToInt32(item.GetDataKeyValue("idmun_act_economica").ToString().Trim());
                    int _IdEstado = Convert.ToInt32(item.GetDataKeyValue("id_estado").ToString().Trim());
                    _EstadoUsuario = item["codigo_estado"].Text.ToString().Trim();

                    //Asignamos el Cuentas al Comercio seleccionado
                    ObjEstablecimiento.IdEstabActEconomica = null;
                    ObjEstablecimiento.IdClienteEstablecimiento = Convert.ToInt32(this.ViewState["IdClienteEstablecimiento"].ToString().Trim());
                    ObjEstablecimiento.IdMunActEconomica = _IdMunActEconomica;
                    ObjEstablecimiento.IdPuc = null;
                    ObjEstablecimiento.IdTipoCalculo = 2;   //--POR DEFAULT QUEDA PARA CALCULAR CON LA TARIFA DEL MUNICIPIO
                    ObjEstablecimiento.IdEstado = 1;
                    ObjEstablecimiento.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    ObjEstablecimiento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjEstablecimiento.TipoProceso = 1;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjEstablecimiento);
                    _log.Warn("REQUEST INSERT ACT. ECON. OFICINA CLIENTE => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjEstablecimiento.AddActEconEstablecimiento(ref _IdRegistro, ref _MsgError))
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
                        #endregion

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                        ObjAuditoria.ModuloApp = "ACT_ECON_OFICINA_CLIENTE";
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

                        //this.ViewState["_FuenteDatosActEconSinAsignar"] = null;
                        //this.RadGrid1.Rebind();

                        this.ViewState["_FuenteDatosActEconAsignadas"] = null;
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
                else if (e.CommandName == "BtnLogsAuditoria")
                {
                    #region DEFINICION DEL EVENTO CLICK PARA EDITAR IMPUESTO
                    //--MANDAMOS ABRIR EL FORM COMO POPUP
                    this.UpdatePanel2.Update();
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string _ModuloApp = "ACT_ECON_OFICINA_CLIENTE";
                    string _PathUrl = this.ViewState["PathUrl"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Seguridad/FrmLogsAuditoria.aspx?ModuloApp=" + _ModuloApp + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow12";
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(500);
                    Ventana.Width = Unit.Pixel(1000);
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
            }
        }

        protected void RadGrid2_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatosActEconAsignadas.Tables["DtActEconomicaAsignadas"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idestab_act_economica"] };
            DataRow[] changedRows = TablaDatos.Select("idestab_act_economica = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["idestab_act_economica"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["idestab_act_economica"].ToString().Trim());

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

                string CmbIdTipoCalculo = (editedItem["idtipo_calculo"].Controls[0] as RadComboBox).SelectedValue.ToString().Trim();
                string CmbTipoCalculo = (editedItem["idtipo_calculo"].Controls[0] as RadComboBox).SelectedItem.Text;

                //Asignamos el Cuentas al Comercio seleccionado
                ObjEstablecimiento.IdEstabActEconomica = _IdRegistro;
                ObjEstablecimiento.IdClienteEstablecimiento = Convert.ToInt32(this.ViewState["IdClienteEstablecimiento"].ToString().Trim());
                ObjEstablecimiento.IdMunActEconomica = null;
                ObjEstablecimiento.IdPuc = null;
                ObjEstablecimiento.IdTipoCalculo = CmbIdTipoCalculo;
                ObjEstablecimiento.IdEstado = 1;
                ObjEstablecimiento.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                ObjEstablecimiento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjEstablecimiento.TipoProceso = 3;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjEstablecimiento);
                _log.Warn("REQUEST UPDATE ACT. ECON. OFICINA CLIENTE => " + jsonRequest);

                string _MsgError = "";
                if (ObjEstablecimiento.AddActEconEstablecimiento(ref _IdRegistro, ref _MsgError))
                {
                    changedRows[0]["tipo_calculo"] = CmbTipoCalculo.ToString().Trim().ToUpper();
                    this.FuenteDatosActEconAsignadas.Tables["DtActEconomicaAsignadas"].Rows[0].AcceptChanges();
                    this.FuenteDatosActEconAsignadas.Tables["DtActEconomicaAsignadas"].Rows[0].EndEdit();

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                    ObjAuditoria.ModuloApp = "ACT_ECON_OFICINA_CLIENTE";
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
                string _MsgError = "Error al editar el tipo de moneda. Motivo: " + ex.ToString();
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

        #region DEFINICION DE OBJETOS GRID 4
        protected void RadGrid4_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid4.DataSource = this.FuenteDatosPucCliente;
                this.RadGrid4.DataMember = "DtClientePuc";
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error con el evento NeedDataSource. Motivo: " + ex.ToString();
            }
        }

        protected void RadGrid4_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnAsociar")
                {
                    #region DEFINICION DE METODO PARA DISOCIAR LA CUENTA 
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdPuc = Convert.ToInt32(item.GetDataKeyValue("id_puc").ToString().Trim());
                    string _CodigoCuenta = item["codigo_cuenta"].Text.ToString().Trim();

                    //Disociamos la cuenta seleccionada
                    ObjEstablecimiento.IdEstabActEconomica = this.ViewState["IdEstabActEconomica"].ToString().Trim();
                    ObjEstablecimiento.IdClienteEstablecimiento = Convert.ToInt32(this.ViewState["IdClienteEstablecimiento"].ToString().Trim());
                    ObjEstablecimiento.IdMunActEconomica = Int32.Parse(this.ViewState["IdMunActEconomica"].ToString().Trim());
                    ObjEstablecimiento.IdPuc = _IdPuc;
                    ObjEstablecimiento.IdEstado = this.ViewState["IdEstado"].ToString().Trim();
                    ObjEstablecimiento.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    ObjEstablecimiento.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                    ObjEstablecimiento.TipoProceso = 2;

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjEstablecimiento.AddActEconEstablecimiento(ref _IdRegistro, ref _MsgError))
                    {
                        //--AQUI BUSCAMOS EL ID DE LA ACTIVIDAD SELECCIONADA
                        DataRow[] dataRows = this.FuenteDatosActEconAsignadas.Tables["DtActEconomicaAsignadas"].Select("idestab_act_economica = " + this.ViewState["IdEstabActEconomica"].ToString().Trim());
                        if (dataRows.Length == 1)
                        {
                            dataRows[0]["id_puc"] = _IdPuc.ToString();
                            dataRows[0]["codigo_cuenta"] = _CodigoCuenta;
                            //--AQUI ACTUALIZAMOS EL DATATAABLE CON EL ESTADO SELECCIONADO
                            this.FuenteDatosActEconAsignadas.Tables["DtActEconomicaAsignadas"].Rows[0].AcceptChanges();
                            this.FuenteDatosActEconAsignadas.Tables["DtActEconomicaAsignadas"].Rows[0].EndEdit();

                            //--ACTUALIZAR LA LISTA DE LAS ACTIVIDADES
                            this.UpdatePanel2.Update();
                            this.RadGrid2.Rebind();
                            this.LblMensaje.Text = "";
                            this.ModalPopupExtender1.Hide();
                        }

                        //this.ViewState["_FuenteDatosActEconAsignadas"] = null;
                        //this.RadGrid2.Rebind();
                    }
                    else
                    {
                        this.LblMensaje.ForeColor = Color.Red;
                        this.LblMensaje.Text = _MsgError;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error con el evento ItemCommand. Motivo: " + ex.ToString();
            }
        }

        protected void RadGrid4_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            try
            {
                this.RadGrid4.Rebind();
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error con el evento PageIndexChanged. Motivo: " + ex.ToString();
            }
        }

        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            this.ViewState["_FuenteDatosClientePuc"] = null;
            this.RadGrid4.Rebind();
        }

        protected void BtnSalir1_Click(object sender, EventArgs e)
        {
            this.LblMensaje.Text = "";
            this.ModalPopupExtender1.Hide();
        }
        #endregion

    }
}