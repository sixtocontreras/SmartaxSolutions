using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Parametros.Divipola;
using System.Web.Caching;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Administracion.Clientes
{
    public partial class FrmAddEstablecimiento : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();
        const string quote = "\"";

        ClienteEstablecimiento ObjEstablecimiento = new ClienteEstablecimiento();
        Departamento ObjDpto = new Departamento();
        Municipio ObjMunicipio = new Municipio();
        Estado ObjEstado = new Estado();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            DataTable DtDatos = new DataTable();
            try
            {
                ObjEstablecimiento.TipoConsulta = 1;
                ObjEstablecimiento.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                ObjEstablecimiento.IdEstablecimientoPadre = null;
                ObjEstablecimiento.AnioGravable = null;
                ObjEstablecimiento.CodigoOficina = null;
                ObjEstablecimiento.NombreOficina = null;
                ObjEstablecimiento.IdEstado = null;
                ObjEstablecimiento.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar los formularios
                ObjetoDataTable = ObjEstablecimiento.GetAllEstablecimientos();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idcliente_establecimiento, id_municipio"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);
                this.ViewState["DtEstablecimientos"] = ObjetoDataTable;

                int _NumEstablecimiento = Int32.Parse(this.ViewState["NumEstablecimiento"].ToString().Trim());
                int _EstablecimientosReg = ObjetoDataTable.Rows.Count;

                //--Si completa la cantidad de establecimientos se le quita la opción de poder registrar mas establecimientos
                if (_EstablecimientosReg == _NumEstablecimiento)
                {
                    this.RadGrid1.MasterTableView.CommandItemDisplay = 0;
                }

                //Mostrar los Departamentos
                ObjDpto.TipoConsulta = 2;
                ObjDpto.IdPais = this.Session["IdPais"].ToString().Trim();
                ObjDpto.IdEstado = 1;
                ObjDpto.MostrarSeleccione = "NO";
                ObjDpto.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjDpto.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //--Obtener datos de la cache de dptos
                ObjetoDataTable = new DataTable();
                //Verificar si existen los datos en la cache
                if (Cache.Get(FixedData.GetCacheDptos) == null)
                {
                    ObjetoDataTable = ObjDpto.GetDptos();
                    Cache.Add(FixedData.GetCacheDptos, ObjetoDataTable, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Default, null);
                }
                else
                {
                    DtDatos = new DataTable();
                    DtDatos = (DataTable)Cache.Get(FixedData.GetCacheDptos);
                    ObjetoDataTable = DtDatos.Copy();
                }
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los Municipios
                ObjMunicipio.TipoConsulta = 2;
                ObjMunicipio.IdDpto = null;
                ObjMunicipio.IdEstado = 1;
                ObjMunicipio.MostrarSeleccione = "NO";
                ObjMunicipio.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjMunicipio.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = new DataTable();
                //Verificar si existen los datos en la cache
                if (Cache.Get(FixedData.GetCacheMunicipios) == null)
                {
                    ObjetoDataTable = ObjMunicipio.GetMunicipios();
                    Cache.Add(FixedData.GetCacheMunicipios, ObjetoDataTable, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Default, null);
                }
                else
                {
                    DtDatos = new DataTable();
                    DtDatos = (DataTable)Cache.Get(FixedData.GetCacheMunicipios);
                    ObjetoDataTable = DtDatos.Copy();
                }
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los Estados
                ObjetoDataTable = new DataTable();
                ObjEstado.TipoConsulta = 2;
                ObjEstado.TipoEstado = "INTERFAZ";
                ObjEstado.MostrarSeleccione = "NO";
                ObjEstado.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjEstado.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjEstado.GetEstados();
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
                string _MsgError = "Error al listar los tipos de tarifas. Motivo: " + ex.ToString();
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
                _log.Error(_MsgError);
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

            objNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = this.ViewState["PathUrl"].ToString().Trim();
            objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

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
            if (!objPermiso.PuedeEliminar)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 1].Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //--Aqui capturamos los parametros.
                this.ViewState["IdCliente"] = Request.QueryString["IdCliente"].ToString().Trim();
                this.ViewState["NumEstablecimiento"] = Request.QueryString["NumEstablecimiento"].ToString().Trim();
                this.ViewState["PathUrl"] = Request.QueryString["PathUrl"].ToString().Trim();
                this.ViewState["DtEstablecimientos"] = null;

                //Metodo para aplicar permisos
                if (this.Session["IdCliente"] == null)
                {
                    this.AplicarPermisos();
                }
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
        }

        #region DEFINICION DE METODOS DEL GRID
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid1.DataSource = this.FuenteDatos;
                this.RadGrid1.DataMember = "DtEstablecimientos";

                #region LISTAR DATOS EN EL COMBOBOX
                GridDropDownColumn columna = new GridDropDownColumn();
                columna = (GridDropDownColumn)this.RadGrid1.Columns[18];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Departamento";
                columna.DataField = "id_dpto";
                columna.ListTextField = "nombre_departamento";
                columna.ListValueField = "id_dpto";
                columna.ListDataMember = "DtDptos";

                columna = (GridDropDownColumn)this.RadGrid1.Columns[19];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Municipio";
                columna.DataField = "id_municipio";
                columna.ListTextField = "nombre_municipio";
                columna.ListValueField = "id_municipio";
                columna.ListDataMember = "DtMunicipios";

                columna = (GridDropDownColumn)this.RadGrid1.Columns[21];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Estado";
                columna.DataField = "id_estado";
                columna.ListTextField = "codigo_estado";
                columna.ListValueField = "id_estado";
                columna.ListDataMember = "DtEstados";
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
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del Establecimiento. Motivo: " + ex.ToString();
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
                _log.Error(_MsgError);
                #endregion
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnAddActEconomicas")
                {
                    #region REGISTRAR LAS ACTIVIDADES ECONOMICAS DEL ESTABLECIMIENTO
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdClienteEstablecimiento = Convert.ToInt32(item.GetDataKeyValue("idcliente_establecimiento").ToString().Trim());
                    int _IdMunicipio = Convert.ToInt32(item.GetDataKeyValue("id_municipio").ToString().Trim());
                    string _NombreOficina = item["nombre_oficina"].Text.ToString().Trim();

                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmActividadEconomicaEstab.aspx?IdClienteEstablecimiento=" + _IdClienteEstablecimiento + "&IdMunicipio=" + _IdMunicipio + "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() + "&PathUrl=" + this.ViewState["PathUrl"].ToString().Trim();
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(550);
                    Ventana.Width = Unit.Pixel(1250);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Actividades Economica de Establecimiento: " + _NombreOficina;
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    #endregion
                }
                else if (e.CommandName == "BtnAddEstablecimiento")
                {
                    #region AGREGAR NUEVO ESTABLECIMIENTO DEL CLIENTE
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdEstablecimiento = Convert.ToInt32(item.GetDataKeyValue("idcliente_establecimiento").ToString().Trim());
                    string _CodigoOficina = item["codigo_oficina"].Text.ToString().Trim();
                    string _NombreOficina = item["nombre_oficina"].Text.ToString().Trim();
                    //string _IdDpto = item["id_dpto"].Text.ToString().Trim();
                    string _IdMunicipio = item.GetDataKeyValue("id_municipio").ToString().Trim();
                    int _NumEstablecimiento = Int32.Parse(item["numero_puntos"].Text.ToString().Trim());

                    if (_NumEstablecimiento > 1)
                    {
                        #region MOSTRAR FORMULARIO PARA REGISTRO
                        //--Mandamos abrir el formulario de registro
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;
                        Ventana.Modal = true;

                        string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                        Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddEstablecimientoHijo.aspx?IdEstablecimiento=" + _IdEstablecimiento + "&IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() + "&IdMunicipio=" + _IdMunicipio + "&NumEstablecimiento=" + _NumEstablecimiento + "&PathUrl=" + this.ViewState["PathUrl"].ToString().Trim();
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(570);
                        Ventana.Width = Unit.Pixel(1150);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Agregar Establecimientos Hijos Id: " + _IdEstablecimiento + " - " + _CodigoOficina + " " + _NombreOficina;
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
                        string _MsgMensaje = "Señor usuario, para poder crear establecimientos que dependen de este, debe primero ampliar la cantidad de puntos !";
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
                        _log.Error(_MsgMensaje.Trim());
                        #endregion
                    }
                    #endregion
                }
                else if (e.CommandName == "BtnLoadOficinas")
                {
                    #region EVENTO CLICK PARA EL CARGUE MASIVO DE ESTABLECIMIENTOS
                    //--MANDAMOS ABRIR EL FORM COMO POPUP
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmLoadEstablecimientos.aspx?IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(600);
                    Ventana.Width = Unit.Pixel(1300);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Realizar Cargue Masivo de Establecimientos";
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    #endregion
                }
                else if (e.CommandName == "BtnLoadActividades")
                {
                    #region EVENTO CLICK PARA EL CARGUE MASIVO DE ACTIVIDADES ECONOMICAS
                    //--MANDAMOS ABRIR EL FORM COMO POPUP
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmLoadActEconomicasOfic.aspx?IdCliente=" + this.ViewState["IdCliente"].ToString().Trim() + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(600);
                    Ventana.Width = Unit.Pixel(1300);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Realizar Cargue Masivo de Act. Economicas";
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    #endregion
                }
                else if (e.CommandName == "BtnCalcularPuntos")
                {
                    #region DEFINICION DEL EVENTO CLICK PARA EDITAR IMPUESTO
                    //--OBTENER DATOS DEL DATASET
                    DataSet dtDataSet = new DataSet();
                    dtDataSet = (DataSet)this.ViewState["_FuenteDatos"];
                    //--
                    DataTable dtDatos = new DataTable();
                    dtDatos = dtDataSet.Tables["DtEstablecimientos"];
                    //--
                    #region DEFINIR DATATABLE FINAL
                    DataTable TablaDatos = new DataTable();
                    TablaDatos.TableName = "DtMunicipiosPuntos";
                    //--
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("numero_puntos", typeof(Int32));
                    #endregion

                    //--AQUI SE OBTIENE LOS MUNICIPIOS CON EL NUMWERO DE PUNTOS > 0
                    DataRow[] dataRows1 = dtDatos.Select("numero_puntos > " + 0);
                    //--RECORRER EL DATATABLE
                    foreach (DataRow rowItem1 in dataRows1)
                    {
                        #region OBTENER DATOS DEL DATATABLE A CARGAR
                        int _IdMunicipio = Int32.Parse(rowItem1["id_municipio"].ToString().Trim());
                        int _NumeroPuntos = Int32.Parse(rowItem1["numero_puntos"].ToString().Trim());

                        //--AQUI VALIDAMOS SI YA EL MUNICIPIO EXISTE EN LA DATATABLE TABLADATOS
                        DataRow[] dataRows2 = TablaDatos.Select("id_municipio = " + _IdMunicipio);
                        Int32 _TotalPuntos = 0;
                        if (dataRows2.Length == 1)
                        {
                            #region ACTUALIZAMOS LA CANTIDAD DE PUNTOS SI EXISTE
                            int _CantidadTxCredito = Int32.Parse(dataRows2[0]["numero_puntos"].ToString().Trim());
                            _TotalPuntos = (_CantidadTxCredito + _NumeroPuntos);

                            //--ACTUALIZAR EL REGISTRO DEL DATATABLE
                            dataRows2[0]["numero_puntos"] = _TotalPuntos;
                            TablaDatos.Rows[0].AcceptChanges();
                            TablaDatos.Rows[0].EndEdit();
                            #endregion
                        }
                        else
                        {
                            #region LO AGREGAMOS AL DATATABLE SI NO EXISTE
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_municipio"] = _IdMunicipio;
                            Fila["numero_puntos"] = _NumeroPuntos;
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                        #endregion
                    }

                    string _ArrayData = "";
                    foreach (DataRow rowItem in TablaDatos.Rows)
                    {
                        #region OBTENER DATOS DEL DATATABLE
                        //--
                        int _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                        int _NumeroPuntos = Int32.Parse(rowItem["numero_puntos"].ToString().Trim());

                        //--ARMAMOS EL ARRAY DE LOS DATOS A CARGAR
                        if (_ArrayData.ToString().Trim().Length > 0)
                        {
                            _ArrayData = _ArrayData.ToString().Trim() + "," + quote + "(" + _IdMunicipio + "," + _NumeroPuntos + ")" + quote;
                        }
                        else
                        {
                            _ArrayData = quote + "(" + _IdMunicipio + "," + _NumeroPuntos + ")" + quote;
                        }
                        #endregion
                    }

                    if (_ArrayData.ToString().Trim().Length > 0)
                    {
                        ObjEstablecimiento.ArrayEstablecimientos = _ArrayData.ToString().Trim();
                        ObjEstablecimiento.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                        ObjEstablecimiento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        //--
                        int _IdRegistro = 0;
                        string _MsgError = "";
                        if (ObjEstablecimiento.GetLoadCantidadPuntos(ref _IdRegistro, ref _MsgError))
                        {
                            #region MOSTRAR FORM CON INFORMACION
                            //--MANDAMOS ABRIR EL FORM COMO POPUP
                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;
                            Ventana.Modal = true;

                            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                            Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmVerMunicipiosPuntos.aspx?PathUrl=" + _PathUrl;
                            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(550);
                            Ventana.Width = Unit.Pixel(1100);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Cantidad de Puntos x Municipios";
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
                        string _Mensaje = "Señor usuario, no se encontro información para actualizar el sistema !";
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

                    string _ModuloApp = "OFICINA_CLIENTE";
                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Seguridad/FrmLogsAuditoria.aspx?ModuloApp=" + _ModuloApp + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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

        protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                try
                {
                    GridEditableItem item = (GridEditableItem)e.Item;
                    #region FILTRADOS DE DATOS
                    //Aqui se arma el Autopostback para filtrar las Ciudades por Dpto
                    RadComboBox Cmb_Dpto1 = item["id_dpto"].Controls[0] as RadComboBox;
                    Cmb_Dpto1.AutoPostBack = true;
                    Cmb_Dpto1.SelectedIndexChanged += this.CmbCdad_SelectedIndexChanged;
                    #endregion

                    #region DEFINICION DE VALIDACION DE CAMPOS
                    //--Codigo oficina
                    GridTextBoxColumnEditor editor1 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("codigo_oficina");
                    TableCell cell = (TableCell)editor1.TextBoxControl.Parent;
                    RequiredFieldValidator validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor1.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor1.Visible = true;

                    //--Nombre oficina
                    GridTextBoxColumnEditor editor2 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("nombre_oficina");
                    cell = (TableCell)editor2.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor2.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor2.Visible = true;

                    //--Nombre contacto
                    GridTextBoxColumnEditor editor3 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("nombre_contacto");
                    cell = (TableCell)editor3.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor3.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor3.Visible = true;

                    //----
                    GridTextBoxColumnEditor editor4 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("direccion_contacto");
                    cell = (TableCell)editor4.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor4.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor4.Visible = true;

                    //----
                    GridTextBoxColumnEditor editor5 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("telefono_contacto");
                    cell = (TableCell)editor5.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor5.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor5.Visible = true;

                    //--Validar campo numero de establecimientos
                    GridNumericColumnEditor editor6 = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("numero_puntos");
                    cell = (TableCell)editor6.NumericTextBox.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor6.NumericTextBox.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor6.Visible = true;
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
                    string _MsgError = "Error con el evento RadGrid1_ItemCreated del Establecimiento. Motivo: " + ex.ToString();
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
                    _log.Error(_MsgError);
                    #endregion
                }
            }
        }

        protected void CmbCdad_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                GridEditableItem editedItem = (GridEditableItem)((RadComboBox)sender).NamingContainer;
                RadComboBox CmbDpto = editedItem["id_dpto"].Controls[0] as RadComboBox;
                RadComboBox CmbCiudad = editedItem["id_municipio"].Controls[0] as RadComboBox;
                DataTable DtDatosCiudad = new DataTable();

                //Mostrar los Municipios
                ObjMunicipio.TipoConsulta = 2;
                ObjMunicipio.IdDpto = Int32.Parse(CmbDpto.SelectedValue.ToString().Trim());
                ObjMunicipio.IdEstado = 1;
                ObjMunicipio.MostrarSeleccione = "NO";
                ObjMunicipio.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjMunicipio.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                DtDatosCiudad = ObjMunicipio.GetMunicipios();
                CmbCiudad.DataSource = DtDatosCiudad;
                CmbCiudad.DataBind();
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
                string _MsgError = "Error al filtrar las Ciudades. Motivo: " + ex.ToString();
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
                _log.Error(_MsgError);
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
                string _MsgError = "Error con el evento RadGrid1_PageIndexChanged del Establecimiento. Motivo: " + ex.ToString();
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
                _log.Error(_MsgError);
                #endregion
            }
        }
        #endregion

        #region DEFINICION DEL CRUD
        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = this.FuenteDatos.Tables["DtEstablecimientos"]; ;
            DataRow NuevaFila = TablaDatos.NewRow();
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idcliente_establecimiento"] };
            DataRow[] TodosValores = TablaDatos.Select("", "idcliente_establecimiento", DataViewRowState.CurrentRows); ;

            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

            try
            {
                foreach (DictionaryEntry entry in newValues)
                {
                    NuevaFila[(string)entry.Key] = entry.Value;
                }

                if ((NuevaFila != null))
                {
                    string CmbDpto = (editedItem["id_dpto"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbMunicipio = (editedItem["id_municipio"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;

                    ObjEstablecimiento.IdClienteEstablecimiento = null;
                    ObjEstablecimiento.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                    ObjEstablecimiento.IdEstablecimientoPadre = null;
                    ObjEstablecimiento.CodigoOficina = NuevaFila["codigo_oficina"].ToString().Trim().ToUpper();
                    ObjEstablecimiento.NombreOficina = NuevaFila["nombre_oficina"].ToString().Trim().ToUpper();
                    ObjEstablecimiento.NombreContacto = NuevaFila["nombre_contacto"].ToString().Trim().ToUpper();
                    ObjEstablecimiento.DireccionContacto = NuevaFila["direccion_contacto"].ToString().Trim().ToUpper();
                    ObjEstablecimiento.TelefonoContacto = NuevaFila["telefono_contacto"].ToString().Trim().ToUpper();
                    ObjEstablecimiento.InscritoRit = NuevaFila["inscrito_rit"].ToString().Trim().ToUpper();
                    ObjEstablecimiento.NumeroPuntos = NuevaFila["numero_puntos"].ToString().Trim().ToUpper();
                    ObjEstablecimiento.IdMunicipio = Int32.Parse(NuevaFila["id_municipio"].ToString().Trim());
                    ObjEstablecimiento.IdEstado = NuevaFila["id_estado"].ToString().Trim().ToUpper();
                    ObjEstablecimiento.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjEstablecimiento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjEstablecimiento.TipoProceso = 1;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjEstablecimiento);
                    _log.Warn("REQUEST INSERT OFICINA CLIENTE => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjEstablecimiento.AddUpEstablecimiento(NuevaFila, ref _IdRegistro, ref _MsgError))
                    {
                        #region MOSTRAR DATOS REGISTRADOS EN EL GRID
                        NuevaFila["idcliente_establecimiento"] = _IdRegistro;
                        NuevaFila["nombre_departamento"] = CmbDpto.ToString().Trim();
                        NuevaFila["nombre_municipio"] = CmbMunicipio.ToString().Trim();
                        NuevaFila["codigo_oficina"] = NuevaFila["codigo_oficina"].ToString().Trim().ToUpper();
                        NuevaFila["nombre_oficina"] = NuevaFila["nombre_oficina"].ToString().Trim().ToUpper();
                        NuevaFila["nombre_contacto"] = NuevaFila["nombre_contacto"].ToString().Trim().ToUpper();
                        NuevaFila["direccion_contacto"] = NuevaFila["direccion_contacto"].ToString().Trim().ToUpper();
                        NuevaFila["telefono_contacto"] = NuevaFila["telefono_contacto"].ToString().Trim().ToUpper();
                        NuevaFila["numero_puntos"] = NuevaFila["numero_puntos"].ToString().Trim().ToUpper();
                        NuevaFila["codigo_estado"] = CmbEstado.ToString().Trim();
                        NuevaFila["fecha_registro"] = DateTime.Now;
                        this.FuenteDatos.Tables["DtEstablecimientos"].Rows.Add(NuevaFila);
                        #endregion

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.ModuloApp = "OFICINA_CLIENTE";
                        ObjAuditoria.IdTipoEvento = 2;  //--INSERT
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
                        _log.Error(_MsgError);
                        #endregion
                    }
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
                string _MsgError = "Error al registrar el Establecimiento. Motivo: " + ex.ToString();
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
                _log.Error(_MsgError);
                e.Canceled = true;
                #endregion
            }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatos.Tables["DtEstablecimientos"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idcliente_establecimiento"] };
            DataRow[] changedRows = TablaDatos.Select("idcliente_establecimiento = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["idcliente_establecimiento"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["idcliente_establecimiento"].ToString().Trim());

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

                string CmbDpto = (editedItem["id_dpto"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbMunicipio = (editedItem["id_municipio"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;

                ObjEstablecimiento.IdClienteEstablecimiento = _IdRegistro;
                ObjEstablecimiento.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                ObjEstablecimiento.IdEstablecimientoPadre = null;
                ObjEstablecimiento.CodigoOficina = changedRows[0]["codigo_oficina"].ToString().Trim().ToUpper();
                ObjEstablecimiento.NombreOficina = changedRows[0]["nombre_oficina"].ToString().Trim().ToUpper();
                ObjEstablecimiento.NombreContacto = changedRows[0]["nombre_contacto"].ToString().Trim().ToUpper();
                ObjEstablecimiento.DireccionContacto = changedRows[0]["direccion_contacto"].ToString().Trim().ToUpper();
                ObjEstablecimiento.TelefonoContacto = changedRows[0]["telefono_contacto"].ToString().Trim().ToUpper();
                ObjEstablecimiento.InscritoRit = changedRows[0]["inscrito_rit"].ToString().Trim().ToUpper();
                ObjEstablecimiento.NumeroPuntos = changedRows[0]["numero_puntos"].ToString().Trim().ToUpper();
                ObjEstablecimiento.IdMunicipio = Int32.Parse(changedRows[0]["id_municipio"].ToString().Trim());
                ObjEstablecimiento.IdEstado = changedRows[0]["id_estado"].ToString().Trim().ToUpper();
                ObjEstablecimiento.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjEstablecimiento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjEstablecimiento.TipoProceso = 2;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjEstablecimiento);
                _log.Warn("REQUEST UPDATE OFICINA CLIENTE => " + jsonRequest);

                string _MsgError = "";
                if (ObjEstablecimiento.AddUpEstablecimiento(changedRows[0], ref _IdRegistro, ref _MsgError))
                {
                    #region MOSTRAR DATOS EDITADOS EN EL GRID
                    changedRows[0]["nombre_departamento"] = CmbDpto.ToString().Trim();
                    changedRows[0]["nombre_municipio"] = CmbMunicipio.ToString().Trim();
                    changedRows[0]["codigo_oficina"] = changedRows[0]["codigo_oficina"].ToString().Trim().ToUpper();
                    changedRows[0]["nombre_oficina"] = changedRows[0]["nombre_oficina"].ToString().Trim().ToUpper();
                    changedRows[0]["nombre_contacto"] = changedRows[0]["nombre_contacto"].ToString().Trim().ToUpper();
                    changedRows[0]["direccion_contacto"] = changedRows[0]["direccion_contacto"].ToString().Trim().ToUpper();
                    changedRows[0]["telefono_contacto"] = changedRows[0]["telefono_contacto"].ToString().Trim().ToUpper();
                    changedRows[0]["numero_puntos"] = changedRows[0]["numero_puntos"].ToString().Trim();
                    changedRows[0]["codigo_estado"] = CmbEstado.ToString().Trim().ToUpper();
                    this.FuenteDatos.Tables["DtEstablecimientos"].Rows[0].AcceptChanges();
                    this.FuenteDatos.Tables["DtEstablecimientos"].Rows[0].EndEdit();
                    this.ViewState["_FuenteDatos"] = this.FuenteDatos;
                    #endregion

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                    ObjAuditoria.ModuloApp = "OFICINA_CLIENTE";
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
                string _MsgError = "Error al editar el Establecimiento. Motivo: " + ex.ToString();
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
                _log.Error(_MsgError);
                e.Canceled = true;
                #endregion
            }
        }

        protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatos.Tables["DtEstablecimientos"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idcliente_establecimiento"] };
            DataRow[] changedRows = TablaDatos.Select("idcliente_establecimiento = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["idcliente_establecimiento"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["idcliente_establecimiento"].ToString().Trim());

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

                string _MsgError = "";
                ObjEstablecimiento.IdClienteEstablecimiento = _IdRegistro;
                ObjEstablecimiento.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                ObjEstablecimiento.IdEstablecimientoPadre = null;
                ObjEstablecimiento.CodigoOficina = changedRows[0]["codigo_oficina"].ToString().Trim().ToUpper();
                ObjEstablecimiento.NombreOficina = changedRows[0]["nombre_oficina"].ToString().Trim().ToUpper();
                ObjEstablecimiento.NombreContacto = changedRows[0]["nombre_contacto"].ToString().Trim().ToUpper();
                ObjEstablecimiento.DireccionContacto = changedRows[0]["direccion_contacto"].ToString().Trim().ToUpper();
                ObjEstablecimiento.TelefonoContacto = changedRows[0]["telefono_contacto"].ToString().Trim().ToUpper();
                ObjEstablecimiento.InscritoRit = changedRows[0]["inscrito_rit"].ToString().Trim().ToUpper();
                ObjEstablecimiento.NumeroPuntos = changedRows[0]["numero_puntos"].ToString().Trim().ToUpper();
                ObjEstablecimiento.IdMunicipio = Int32.Parse(changedRows[0]["id_municipio"].ToString().Trim());
                ObjEstablecimiento.IdEstado = changedRows[0]["id_estado"].ToString().Trim().ToUpper();
                ObjEstablecimiento.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjEstablecimiento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjEstablecimiento.TipoProceso = 3;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjEstablecimiento);
                _log.Warn("REQUEST DELETE OFICINA CLIENTE => " + jsonRequest);

                if (ObjEstablecimiento.AddUpEstablecimiento(changedRows[0], ref _IdRegistro, ref _MsgError))
                {
                    this.FuenteDatos.Tables["DtEstablecimientos"].Rows.Find(_IdRegistro).Delete();
                    this.FuenteDatos.Tables["DtEstablecimientos"].AcceptChanges();

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 4;  //--DELETE
                    ObjAuditoria.ModuloApp = "OFICINA_CLIENTE";
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
                string _MsgError = "Error al eliminar el Establecimiento. Motivo: " + ex.ToString();
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
                _log.Error(_MsgError);
                e.Canceled = true;
                #endregion
            }
        }
        #endregion

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                MunicipioActividadEconomica ObjMunActividad = new MunicipioActividadEconomica();
                ClienteEstablecimiento ObjEstablecimiento = new ClienteEstablecimiento();
                //--
                DataTable dtDatos = new DataTable();
                dtDatos = (DataTable)this.ViewState["DtEstablecimientos"];
                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        foreach (DataRow rowItem in dtDatos.Rows)
                        {
                            int _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                            int _IdClienteEstablecimiento = Int32.Parse(rowItem["idcliente_establecimiento"].ToString().Trim());

                            ObjMunActividad.IdMunicipio = _IdMunicipio;
                            ObjMunActividad.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                            ObjMunActividad.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                            DataTable dtActEconomica = new DataTable();
                            dtActEconomica = ObjMunActividad.GetActEconomicaSinAsignar();
                            //--
                            foreach (DataRow rowItem2 in dtActEconomica.Rows)
                            {
                                #region AQUI REALIZAMOS EL PROCESO DE ASIGNACION DE ACTIVIDADES AL ESTABLECIMIENTO
                                //--
                                int _IdMunActEconomica = Int32.Parse(rowItem2["idmun_act_economica"].ToString().Trim());

                                //Asignamos el Cuentas al Comercio seleccionado
                                ObjEstablecimiento.IdEstabActEconomica = null;
                                ObjEstablecimiento.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                ObjEstablecimiento.IdMunActEconomica = _IdMunActEconomica;
                                ObjEstablecimiento.IdPuc = null;
                                ObjEstablecimiento.IdTipoCalculo = 2;   //--POR DEFAULT QUEDA PARA CALCULAR CON LA TARIFA DEL MUNICIPIO
                                ObjEstablecimiento.IdEstado = 1;
                                ObjEstablecimiento.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                                ObjEstablecimiento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                                ObjEstablecimiento.TipoProceso = 1;

                                int _IdRegistro = 0;
                                string _MsgError = "";
                                if (ObjEstablecimiento.AddActEconEstablecimiento(ref _IdRegistro, ref _MsgError))
                                {
                                    _log.Info(_MsgError);
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}