using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using Telerik.Web.UI;
using System.Web.Caching;
using System.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros.Divipola;

namespace Smartax.Web.Application.Controles.Parametros.Empresa
{
    public partial class CtrlEmpresa : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        #region DEFINICION DE OBJETOS DE CLASES
        Empresas ObjEmpresa = new Empresas();
        Pais ObjPais = new Pais();
        Departamento ObjDpto = new Departamento();
        Municipio ObjMunicipio = new Municipio();
        Estado ObjEstado = new Estado();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        Utilidades ObjUtils = new Utilidades();
        #endregion

        #region DEFINICION DE METODOS OBTENER DATOS EMPRESAS
        //Funcion para devolver datos de empresas padres
        public DataSet GetEmpresasPadres()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            DataTable DtDatos = new DataTable();
            try
            {
                ObjEmpresa.TipoConsulta = 1;
                ObjEmpresa.IdEstado = null;
                ObjEmpresa.IdRol = Convert.ToInt32(this.Session["IdRol"].ToString().Trim());
                ObjEmpresa.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjEmpresa.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                if (ObjEmpresa.IdRol != 1)
                {
                    this.RadGrid1.MasterTableView.CommandItemDisplay = 0;
                }

                //Mostrar las empresas administradoras
                ObjetoDataTable = ObjEmpresa.GetAllEmpresaPadre();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idempresa_hija"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los Paises
                ObjPais.TipoConsulta = 2;
                ObjPais.IdPais = this.Session["IdPais"].ToString().Trim();
                ObjPais.IdEstado = 1;
                ObjPais.MostrarSeleccione = "NO";
                ObjPais.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                //--Obtener datos de la cache de dptos
                ObjetoDataTable = new DataTable();
                //Verificar si existen los datos en la cache
                if (Cache.Get(FixedData.GetCachePais) == null)
                {
                    ObjetoDataTable = ObjPais.GetPaises();
                    Cache.Add(FixedData.GetCachePais, ObjetoDataTable, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Default, null);
                }
                else
                {
                    DtDatos = new DataTable();
                    DtDatos = (DataTable)Cache.Get(FixedData.GetCachePais);
                    ObjetoDataTable = DtDatos.Copy();
                }
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

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
                ObjEstado.TipoEstado = "INTERFAZ";  //INTERFAZ: MUESTRA LOS ESTADOS INACTIVO Y ACTIVOS, PROCESOS: MUESTRA EL RESTO DE LOS ESTADOS MENOS LOS ANTERIORES
                ObjEstado.MostrarSeleccione = "NO";
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
                string _MsgError = "Error al cargar los datos de la empresa. Motivo: " + ex.Message;
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

        //Funcion para devolver datos de empresas hijas
        public DataSet GetEmpresasHijas()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            DataTable DtDatos = new DataTable();
            try
            {
                ObjEmpresa.TipoConsulta = 3;
                ObjEmpresa.IdEstado = null;
                ObjEmpresa.IdRol = Convert.ToInt32(this.Session["IdRol"].ToString().Trim());
                ObjEmpresa.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjEmpresa.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                if (ObjEmpresa.IdRol != 1)
                {
                    this.RadGrid1.MasterTableView.CommandItemDisplay = 0;
                }

                //Mostrar las empresas administradoras
                ObjetoDataTable = ObjEmpresa.GetAllEmpresaHija();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idempresa_hija"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los Paises
                ObjPais.TipoConsulta = 2;
                ObjPais.IdPais = this.Session["IdPais"].ToString().Trim();
                ObjPais.IdEstado = 1;
                ObjPais.MostrarSeleccione = "NO";
                ObjPais.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = new DataTable();
                ObjetoDataTable = ObjPais.GetPaises();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los Departamentos
                ObjDpto.TipoConsulta = 2;
                ObjDpto.IdPais = null;
                ObjDpto.IdEstado = 1;
                ObjDpto.MostrarSeleccione = "NO";
                ObjDpto.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjDpto.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

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
                ObjEstado.TipoEstado = "INTERFAZ";  //INTERFAZ: MUESTRA LOS ESTADOS INACTIVO Y ACTIVOS, PROCESOS: MUESTRA EL RESTO DE LOS ESTADOS MENOS LOS ANTERIORES
                ObjEstado.MostrarSeleccione = "NO";
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
                string _MsgError = "Error al cargar los datos de la empresa. Motivo: " + ex.Message;
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

        private DataSet FuenteDatosEmpresas
        {
            get
            {
                object obj = this.ViewState["_FuenteDatosEmpresas"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetEmpresasPadres();
                    this.ViewState["_FuenteDatosEmpresas"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatosEmpresas"];
                }
            }
            set { this.ViewState["_FuenteDatosEmpresas"] = value; }
        }

        private DataSet FuenteDatosSedes
        {
            get
            {
                object obj = this.ViewState["_FuenteDatosSedes"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetEmpresasHijas();
                    this.ViewState["_FuenteDatosSedes"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatosSedes"];
                }
            }
            set { this.ViewState["_FuenteDatosSedes"] = value; }
        }
        #endregion

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

            objPermiso.RefrescarPermisos();
            if (!objPermiso.PuedeLeer)
            {
                RadGrid1.Visible = false;
            }
            if (!objPermiso.PuedeRegistrar)
            {
                RadGrid1.MasterTableView.CommandItemDisplay = 0;
            }
            if (!objPermiso.PuedeModificar)
            {
                RadGrid1.Columns[RadGrid1.Columns.Count - 3].Visible = false;
            }
            if (!objPermiso.PuedeEliminar)
            {
                RadGrid1.Columns[RadGrid1.Columns.Count - 1].Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.Page.Title = this.Page.Title + "Empresas";
                this.AplicarPermisos();
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
        }

        #region DEFINICION DE EVENTOS DE LA GRILLA
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid1.DataSource = this.FuenteDatosEmpresas;
                this.RadGrid1.DataMember = "DtEmpresas";

                #region LISTAR DATOS EN EL COMBOBOX
                GridDropDownColumn columna = new GridDropDownColumn();
                columna = (GridDropDownColumn)this.RadGrid1.Columns[10];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Pais";
                columna.DataField = "id_pais";
                columna.ListTextField = "nombre_pais";
                columna.ListValueField = "id_pais";
                columna.ListDataMember = "DtPaises";

                columna = (GridDropDownColumn)this.RadGrid1.Columns[11];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Departamento";
                columna.DataField = "id_dpto";
                columna.ListTextField = "nombre_departamento";
                columna.ListValueField = "id_dpto";
                columna.ListDataMember = "DtDptos";

                columna = (GridDropDownColumn)this.RadGrid1.Columns[13];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Municipio";
                columna.DataField = "id_municipio";
                columna.ListTextField = "nombre_municipio";
                columna.ListValueField = "id_municipio";
                columna.ListDataMember = "DtMunicipios";

                columna = (GridDropDownColumn)this.RadGrid1.Columns[15];
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
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource. Motivo: " + ex.Message;
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

        protected void RadGrid1_DetailTableDataBind(object source, GridDetailTableDataBindEventArgs e)
        {
            try
            {
                GridDataItem parentItem = e.DetailTableView.ParentItem as GridDataItem;
                switch (e.DetailTableView.Name)
                {
                    case "DtEmpresasHijas":
                        this.ViewState["IdEmpresaPadre"] = parentItem.GetDataKeyValue("id_empresa").ToString().Trim();
                        e.DetailTableView.DataSource = this.GetEmpresasHijas();

                        if (ObjEmpresa.IdRol != 1)
                        {
                            //Retorna el Numero de Empresas Registradas en el Sistema
                            Session["CantidadEmpresaRegistradas"] = ObjEmpresa.GetCantidadEmpresasRegistradas();

                            if (Int32.Parse(Session["CantidadEmpresaRegistradas"].ToString().Trim()) >= Int32.Parse(Session["CanEmpresaHijasRegistrar"].ToString().Trim()))
                            {
                                e.DetailTableView.CommandItemDisplay = 0;
                            }
                        }
                        break;

                    default:
                        break;
                }

                if (parentItem.Edit)
                {
                    return;
                }
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
                string _MsgError = "Error con el evento RadGrid1_DetailTableDataBind. Motivo: " + ex.Message;
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
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                switch (e.Item.OwnerTableView.Name)
                {
                    case "DtEmpresas":
                        if (e.CommandName == "Delete")
                        {
                            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                            this.RadWindowManager1.Enabled = false;
                            this.RadWindowManager1.EnableAjaxSkinRendering = false;
                            this.RadWindowManager1.Visible = false;

                        }
                        else if (e.CommandName == "BtnVerInfo")
                        {
                            GridDataItem item = (GridDataItem)e.Item;
                            int _IdEmpresa = Convert.ToInt32(item.GetDataKeyValue("id_empresa").ToString().Trim());
                            string _NombreEmpresa = item["nombre_empresa"].Text.ToString().Trim();

                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;
                            Ventana.Modal = true;

                            string PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                            Ventana.NavigateUrl = "/Controles/Parametros/FrmVerInfoEntidad.aspx?IdEmpresa=" + _IdEmpresa;
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(510);
                            Ventana.Width = Unit.Pixel(820);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Nombre de la Empresa: " + _NombreEmpresa;
                            Ventana.VisibleStatusbar = false;
                            Ventana.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize;
                            this.RadWindowManager1.Windows.Add(Ventana);

                            this.RadWindowManager1 = null;
                            Ventana = null;
                        }
                        else if (e.CommandName == "BtnAddLogo")
                        {
                            GridDataItem item = (GridDataItem)e.Item;
                            int _IdEmpresa = Convert.ToInt32(item.GetDataKeyValue("id_empresa").ToString().Trim());
                            string _NombreEmpresa = item["nombre_empresa"].Text.ToString().Trim();

                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;
                            Ventana.Modal = true;

                            string PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                            Ventana.NavigateUrl = "/Controles/Parametros/FrmAddLogoEmpresa.aspx?IdEmpresa=" + _IdEmpresa + "&NombreEntidad=" + _NombreEmpresa;
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(350);
                            Ventana.Width = Unit.Pixel(740);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Nombre de la Empresa: " + _NombreEmpresa;
                            Ventana.VisibleStatusbar = false;
                            Ventana.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize;
                            this.RadWindowManager1.Windows.Add(Ventana);

                            this.RadWindowManager1 = null;
                            Ventana = null;
                        }
                        else if (e.CommandName == "BtnAddMovimiento")
                        {
                            GridDataItem item = (GridDataItem)e.Item;
                            int _IdEmpresa = Convert.ToInt32(item.GetDataKeyValue("id_empresa").ToString().Trim());
                            string _NombreEmpresa = item["nombre_empresa"].Text.ToString().Trim();
                            string _SaldoActualEmpresa = item["saldo_cupo"].Text.ToString().Trim().Length > 0 ? item["saldo_cupo"].Text.ToString().Trim() : "$ 0.0";

                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;
                            Ventana.Modal = true;

                            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                            Ventana.NavigateUrl = "/Controles/Parametros/FrmAddMovimientosEmpresa.aspx?IdEmpresa=" + _IdEmpresa + "&SaldoActualEmpresa=" + _SaldoActualEmpresa + "&PathUrl=" + _PathUrl;
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(500);
                            Ventana.Width = Unit.Pixel(970);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Nombre de la Empresa: " + _NombreEmpresa;
                            Ventana.VisibleStatusbar = false;
                            Ventana.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize;
                            this.RadWindowManager1.Windows.Add(Ventana);

                            this.RadWindowManager1 = null;
                            Ventana = null;
                        }
                        else
                        {
                            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                            this.RadWindowManager1.Enabled = false;
                            this.RadWindowManager1.EnableAjaxSkinRendering = false;
                            this.RadWindowManager1.Visible = false;
                        }
                        break;

                    case "DtEmpresasHijas":
                        if (e.CommandName == "Delete")
                        {
                            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                            this.RadWindowManager1.Enabled = false;
                            this.RadWindowManager1.EnableAjaxSkinRendering = false;
                            this.RadWindowManager1.Visible = false;

                        }
                        else if (e.CommandName == "BtnVerInfo")
                        {
                            GridDataItem item = (GridDataItem)e.Item;
                            int _IdEmpresa = Convert.ToInt32(item.GetDataKeyValue("idempresa_hija").ToString().Trim());
                            string _NombreEmpresa = item["nombre_empresa"].Text.ToString().Trim();

                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;
                            Ventana.Modal = true;

                            string PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                            Ventana.NavigateUrl = "/Controles/Parametros/FrmVerInfoEntidad.aspx?IdEmpresa=" + _IdEmpresa;
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(510);
                            Ventana.Width = Unit.Pixel(820);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Nombre de la Empresa: " + _NombreEmpresa;
                            Ventana.VisibleStatusbar = false;
                            Ventana.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize;
                            this.RadWindowManager1.Windows.Add(Ventana);

                            this.RadWindowManager1 = null;
                            Ventana = null;
                        }
                        else if (e.CommandName == "BtnAddLogo")
                        {
                            GridDataItem item = (GridDataItem)e.Item;
                            int _IdEmpresa = Convert.ToInt32(item.GetDataKeyValue("idempresa_hija").ToString().Trim());
                            string _NombreEmpresa = item["nombre_empresa"].Text.ToString().Trim();

                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;
                            Ventana.Modal = true;

                            string PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                            Ventana.NavigateUrl = "/Controles/Parametros/FrmAddLogoEmpresa.aspx?IdEmpresa=" + _IdEmpresa + "&NombreEntidad=" + _NombreEmpresa;
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(350);
                            Ventana.Width = Unit.Pixel(740);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Nombre de la Empresa: " + _NombreEmpresa;
                            Ventana.VisibleStatusbar = false;
                            Ventana.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize;
                            this.RadWindowManager1.Windows.Add(Ventana);

                            this.RadWindowManager1 = null;
                            Ventana = null;
                        }
                        else if (e.CommandName == "BtnAddMovimiento")
                        {
                            GridDataItem item = (GridDataItem)e.Item;
                            int _IdEmpresa = Convert.ToInt32(item.GetDataKeyValue("idempresa_hija").ToString().Trim());
                            string _NombreEmpresa = item["nombre_empresa"].Text.ToString().Trim();
                            string _SaldoActualEmpresa = item["saldo_cupo"].Text.ToString().Trim().Length > 0 ? item["saldo_cupo"].Text.ToString().Trim() : "$ 0.0";

                            this.RadWindowManager1.ReloadOnShow = true;
                            this.RadWindowManager1.DestroyOnClose = true;
                            this.RadWindowManager1.Windows.Clear();
                            this.RadWindowManager1.Enabled = true;
                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                            this.RadWindowManager1.Visible = true;
                            Ventana.Modal = true;

                            string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                            Ventana.NavigateUrl = "/Controles/Parametros/FrmAddMovimientosEmpresa.aspx?IdEmpresa=" + _IdEmpresa + "&SaldoActualEmpresa=" + _SaldoActualEmpresa + "&PathUrl=" + _PathUrl;
                            Ventana.VisibleOnPageLoad = true;
                            Ventana.Visible = true;
                            Ventana.Height = Unit.Pixel(500);
                            Ventana.Width = Unit.Pixel(970);
                            Ventana.KeepInScreenBounds = true;
                            Ventana.Title = "Nombre de la Empresa: " + _NombreEmpresa;
                            Ventana.VisibleStatusbar = false;
                            Ventana.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize;
                            this.RadWindowManager1.Windows.Add(Ventana);

                            this.RadWindowManager1 = null;
                            Ventana = null;
                        }
                        else
                        {
                            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                            this.RadWindowManager1.Enabled = false;
                            this.RadWindowManager1.EnableAjaxSkinRendering = false;
                            this.RadWindowManager1.Visible = false;
                        }
                        break;
                }
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
                string _MsgError = "Error con el evento RadGrid1_ItemCommand. Motivo: " + ex.Message;
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
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (((e.Item is GridEditableItem) && (((GridEditableItem)e.Item).IsInEditMode)))
            {
                try
                {
                    GridEditableItem parentItem = (GridEditableItem)e.Item;
                    GridEditableItem item = (GridEditableItem)e.Item;
                    GridEditableItem ItemEditado = (GridEditableItem)e.Item;

                    switch (e.Item.OwnerTableView.Name)
                    {
                        case "DtEmpresasHijas":
                            GridDropDownListColumnEditor CmbPais = (GridDropDownListColumnEditor)item.EditManager.GetColumnEditor("id_pais");
                            GridDropDownListColumnEditor CmbDpto = (GridDropDownListColumnEditor)item.EditManager.GetColumnEditor("id_dpto");
                            GridDropDownListColumnEditor CmbCiudad = (GridDropDownListColumnEditor)item.EditManager.GetColumnEditor("id_municipio");
                            GridDropDownListColumnEditor CmbEstado = (GridDropDownListColumnEditor)item.EditManager.GetColumnEditor("id_estado");
                            DataTable DtDatos = new DataTable();

                            //Mostrar los Paises
                            ObjPais.TipoConsulta = 2;
                            ObjPais.IdPais = this.Session["IdPais"].ToString().Trim();
                            ObjPais.IdEstado = 1;
                            ObjPais.MostrarSeleccione = "NO";
                            ObjPais.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                            //--Obtener datos de la cache
                            if (Cache.Get(FixedData.GetCachePais) == null)
                            {
                                DtDatos = ObjPais.GetPaises();
                                CmbPais.DataSource = DtDatos;
                                Cache.Add(FixedData.GetCachePais, DtDatos, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Default, null);
                            }
                            else
                            {
                                CmbPais.DataSource = (DataTable)Cache.Get(FixedData.GetCachePais);
                            }
                            CmbPais.DataMember = "DtPaises";
                            CmbPais.DataTextField = "nombre_pais";
                            CmbPais.DataValueField = "id_pais";
                            CmbPais.DataBind();
                            if (ItemEditado.ItemIndex >= 0)
                            {
                                CmbPais.SelectedText = parentItem["id_pais"].Text.ToString().Trim();
                            }
                            else
                            {
                                CmbPais.SelectedIndex = 0;
                            }

                            //Mostrar los Departamentos
                            ObjDpto.TipoConsulta = 2;
                            ObjDpto.IdPais = null;
                            ObjDpto.IdEstado = 1;
                            ObjDpto.MostrarSeleccione = "NO";
                            ObjDpto.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                            ObjDpto.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                            //--Obtener datos de cache dptos
                            if (Cache.Get(FixedData.GetCacheDptos) == null)
                            {
                                DtDatos = ObjDpto.GetDptos();
                                CmbDpto.DataSource = DtDatos;
                                Cache.Add(FixedData.GetCacheDptos, DtDatos, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Default, null);
                            }
                            else
                            {
                                CmbDpto.DataSource = (DataTable)Cache.Get(FixedData.GetCacheDptos);
                            }
                            CmbDpto.DataMember = "DtDptos";
                            CmbDpto.DataTextField = "nombre_departamento";
                            CmbDpto.DataValueField = "id_dpto";
                            CmbDpto.DataBind();
                            if (ItemEditado.ItemIndex >= 0)
                            {
                                CmbDpto.SelectedText = parentItem["id_dpto"].Text.ToString().Trim();
                            }
                            else
                            {
                                CmbDpto.SelectedIndex = 0;
                            }

                            //Mostrar los Municipios
                            ObjMunicipio.TipoConsulta = 2;
                            ObjMunicipio.IdDpto = null;
                            ObjMunicipio.IdEstado = 1;
                            ObjMunicipio.MostrarSeleccione = "NO";
                            ObjMunicipio.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                            ObjMunicipio.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                            //--Obtener datos de cache municipios
                            if (Cache.Get(FixedData.GetCacheMunicipios) == null)
                            {
                                DtDatos = ObjMunicipio.GetMunicipios();
                                CmbCiudad.DataSource = DtDatos;
                                Cache.Add(FixedData.GetCacheMunicipios, DtDatos, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Default, null);
                            }
                            else
                            {
                                CmbCiudad.DataSource = (DataTable)Cache.Get(FixedData.GetCacheMunicipios);
                            }
                            CmbCiudad.DataMember = "DtMunicipios";
                            CmbCiudad.DataTextField = "nombre_municipio";
                            CmbCiudad.DataValueField = "id_municipio";
                            CmbCiudad.DataBind();
                            if (ItemEditado.ItemIndex >= 0)
                            {
                                CmbCiudad.SelectedText = parentItem["id_municipio"].Text.ToString().Trim();
                            }
                            else
                            {
                                CmbCiudad.SelectedIndex = 0;
                            }

                            //Mostrar Estados
                            ObjEstado.TipoConsulta = 2;
                            ObjEstado.TipoEstado = "INTERFAZ";  //INTERFAZ: MUESTRA LOS ESTADOS INACTIVO Y ACTIVOS, PROCESOS: MUESTRA EL RESTO DE LOS ESTADOS MENOS LOS ANTERIORES
                            ObjEstado.MostrarSeleccione = "NO";
                            ObjEstado.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                            CmbEstado.DataSource = ObjEstado.GetEstados();

                            CmbEstado.DataMember = "DtEstados";
                            CmbEstado.DataTextField = "codigo_estado";
                            CmbEstado.DataValueField = "id_estado";
                            CmbEstado.DataBind();
                            if (ItemEditado.ItemIndex >= 0)
                            {
                                CmbEstado.SelectedText = parentItem["id_estado"].Text.ToString().Trim();
                            }
                            else
                            {
                                CmbEstado.SelectedIndex = 0;
                            }
                            break;
                        default:
                            break;
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
                    string _MsgError = "Error con el evento RadGrid1_ItemDataBound. Motivo: " + ex.Message;
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
        }

        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                try
                {
                    GridEditableItem item = (GridEditableItem)e.Item;
                    #region FILTRADOS DE DATOS
                    //Aqui se arma el Autopostback para filtrar los Dptos por Pais
                    RadComboBox Cmb_Pais1 = item["id_pais"].Controls[0] as RadComboBox;
                    Cmb_Pais1.AutoPostBack = true;
                    Cmb_Pais1.SelectedIndexChanged += this.CmbDpto_SelectedIndexChanged;

                    //Aqui se arma el Autopostback para filtrar las Ciudades por Dpto
                    RadComboBox Cmb_Dpto1 = item["id_dpto"].Controls[0] as RadComboBox;
                    Cmb_Dpto1.AutoPostBack = true;
                    Cmb_Dpto1.SelectedIndexChanged += this.CmbCdad_SelectedIndexChanged;
                    #endregion

                    #region DEFINICION DE VALIDACION DE CAMPOS
                    GridTextBoxColumnEditor editor = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("nit_empresa");
                    TableCell cell1 = (TableCell)editor.TextBoxControl.Parent;
                    RequiredFieldValidator validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell1.Controls.Add(validator);
                    editor.Visible = true;

                    //----
                    GridTextBoxColumnEditor editor2 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("nombre_empresa");
                    TableCell cell = (TableCell)editor2.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor2.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);

                    //----
                    GridTextBoxColumnEditor editor6 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("emblema_empresa");
                    cell = (TableCell)editor6.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor6.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);

                    //----
                    GridTextBoxColumnEditor editor3 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("direccion_empresa");
                    cell = (TableCell)editor3.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor3.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);

                    //----
                    GridTextBoxColumnEditor editor4 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("telefono_empresa");
                    cell = (TableCell)editor4.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    //editor4.TextBoxControl.ID = "editor_Telefono";
                    validator.ControlToValidate = editor4.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);

                    //----
                    GridTextBoxColumnEditor editor5 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("email_empresa");
                    cell = (TableCell)editor5.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor5.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);

                    //--Validar campo cantidad producto
                    GridNumericColumnEditor editor7 = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("cant_empresas_registrar");
                    TableCell cell3 = (TableCell)editor7.NumericTextBox.Parent;
                    RequiredFieldValidator validator7 = new RequiredFieldValidator();
                    validator7.ControlToValidate = editor7.NumericTextBox.ID;
                    validator7.ErrorMessage = "Campo Requerido";
                    validator7.Display = ValidatorDisplay.Dynamic;
                    cell3.Controls.Add(validator7);
                    editor7.Visible = true;
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
                    string _MsgError = "Error con el evento RadGrid1_ItemCreated. Motivo: " + ex.Message;
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
                string _MsgError = "Error con el evento RadGrid1_PageIndexChanged. Motivo: " + ex.Message;
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
            }
        }

        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            try
            {
                RadGrid radgrid2 = (RadGrid)sender;
                radgrid2.MasterTableView.GetColumnSafe("BtnAddValor").Visible = false;
                //foreach (GridDataItem item in RadGrid1.MasterTableView.GetItems(GridItemType.Item))
                //{
                //    ImageButton btn1 = (ImageButton)(ImageButton)item["BtnAddValor"].Controls[0];//accessing the Button by its name
                //    btn1.Enabled = false;
                //}

                //foreach (GridDataItem row in RadGrid1.Items)
                //{
                //    if ((row.Cells..Item[42].Text.ToString.Trim.Equals("S")))
                //    {
                //        (RadGrid1.MasterTableView.GetColumn("BtnProcesar") as GridButtonColumn).Text = "Matricular";
                //        RadGrid1.MasterTableView.Rebind();
                //    }
                //    else
                //    {
                //        (RadGrid1.MasterTableView.GetColumn("BtnProcesar") as GridButtonColumn).Text = "Matricular";
                //        RadGrid1.MasterTableView.Rebind();
                //    }
                //}

            }
            catch (Exception ex)
            {
                _log.Error("Error al cambiar el estado de boton de rango de valores. Motivo: " + ex.Message);
            }
        }

        protected void CmbDpto_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                GridEditableItem editedItem = (GridEditableItem)((RadComboBox)sender).NamingContainer;
                RadComboBox CmbPais = editedItem["id_pais"].Controls[0] as RadComboBox;
                RadComboBox CmbDpto = editedItem["id_dpto"].Controls[0] as RadComboBox;
                DataTable DtDatosDpto = new DataTable();

                //Mostrar los Departamentos
                ObjDpto.TipoConsulta = 2;
                ObjDpto.IdPais = Int32.Parse(CmbPais.SelectedValue.ToString().Trim());
                ObjDpto.IdEstado = 1;
                ObjDpto.MostrarSeleccione = "NO";
                ObjDpto.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                ObjDpto.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                DtDatosDpto = ObjDpto.GetDptos();
                CmbDpto.DataSource = DtDatosDpto;
                CmbDpto.DataBind();
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
                string _MsgError = "Error al filtrar los Departamentos. Motivo: " + ex.ToString();
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
        #endregion

        #region DEFINICION DE METODOS PARA EL CRUD EMPRESAS
        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            DataRow NuevaFila = null;
            DataRow[] TodosValores = null;
            Hashtable newValues = new Hashtable();

            string nit_empresa = "";
            string nombre_empresa = "";
            string emblema_empresa = "";
            string direccion_empresa = "";
            string telefono_empresa = "";
            string email_empresa = "";

            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);
            switch (e.Item.OwnerTableView.Name)
            {
                case "DtEmpresas":
                    TablaDatos = this.FuenteDatosEmpresas.Tables["DtEmpresas"];
                    NuevaFila = TablaDatos.NewRow();
                    TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_empresa"] };
                    TodosValores = TablaDatos.Select("", "id_empresa", DataViewRowState.CurrentRows);

                    try
                    {
                        foreach (DictionaryEntry entry in newValues)
                        {
                            NuevaFila[(string)entry.Key] = entry.Value;
                        }

                        if ((NuevaFila != null))
                        {
                            int _IdEmpresa = 0;
                            ObjEmpresa.IdEmpresa = null;
                            ObjEmpresa.IdEmpresaPadre = null;
                            ObjEmpresa.IdUsuarioAdd = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                            ObjEmpresa.IdUsuarioUp = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                            ObjEmpresa.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                            ObjEmpresa.TipoEmpresa = "P";
                            ObjEmpresa.EmpresaUnica = "";
                            ObjEmpresa.TipoProceso = 1;
                            string _MsgError = "";

                            if (ObjEmpresa.AddUpEmpresa(NuevaFila, ref _IdEmpresa, ref _MsgError))
                            {
                                NuevaFila["id_empresa"] = _IdEmpresa;
                                NuevaFila["nit_empresa"] = NuevaFila["nit_empresa"].ToString().Trim().ToUpper();
                                NuevaFila["nombre_empresa"] = NuevaFila["nombre_empresa"].ToString().Trim().ToUpper();
                                NuevaFila["emblema_empresa"] = NuevaFila["emblema_empresa"].ToString().Trim().ToUpper();
                                NuevaFila["direccion_empresa"] = NuevaFila["direccion_empresa"].ToString().Trim().ToUpper();
                                NuevaFila["telefono_empresa"] = NuevaFila["telefono_empresa"].ToString().Trim().ToUpper();
                                NuevaFila["cant_empresas_registrar"] = NuevaFila["cant_empresas_registrar"].ToString().Trim().ToUpper();
                                NuevaFila["empresa_unica"] = NuevaFila["empresa_unica"].ToString().Trim().ToUpper();
                                NuevaFila["id_pais"] = NuevaFila["id_pais"].ToString().Trim();
                                NuevaFila["id_dpto"] = NuevaFila["id_dpto"].ToString().Trim();
                                NuevaFila["id_municipio"] = NuevaFila["id_municipio"].ToString().Trim();
                                NuevaFila["id_estado"] = NuevaFila["id_estado"].ToString().Trim();

                                nit_empresa = NuevaFila["nit_empresa"].ToString().Trim();
                                nombre_empresa = NuevaFila["nombre_empresa"].ToString().Trim();
                                emblema_empresa = NuevaFila["emblema_empresa"].ToString().Trim().ToLower();
                                direccion_empresa = NuevaFila["direccion_empresa"].ToString().Trim();
                                telefono_empresa = NuevaFila["telefono_empresa"].ToString().Trim();
                                email_empresa = NuevaFila["email_empresa"].ToString().Trim();
                                this.FuenteDatosEmpresas.Tables["DtEmpresas"].Rows.Add(NuevaFila);

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
                            }
                            else
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
                            }
                        }
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
                        string _MsgError = "Error al registrar la empresa padre. Motivo: " + ex.ToString();
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
                    }
                    break;

                case "DtEmpresasHijas":
                    TablaDatos = this.FuenteDatosSedes.Tables["DtEmpresasHijas"];
                    NuevaFila = TablaDatos.NewRow();
                    TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idempresa_hija"] };
                    TodosValores = TablaDatos.Select("", "idempresa_hija", DataViewRowState.CurrentRows);

                    try
                    {
                        foreach (DictionaryEntry entry in newValues)
                        {
                            NuevaFila[(string)entry.Key] = entry.Value;
                        }

                        if ((NuevaFila != null))
                        {
                            int _IdEmpresa = 0;
                            ObjEmpresa.IdEmpresa = null;
                            ObjEmpresa.IdEmpresaPadre = Convert.ToInt32(this.ViewState["IdEmpresaPadre"].ToString().Trim());
                            ObjEmpresa.IdUsuarioAdd = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                            ObjEmpresa.IdUsuarioUp = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                            ObjEmpresa.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                            ObjEmpresa.TipoEmpresa = "H";
                            ObjEmpresa.EmpresaUnica = "";
                            ObjEmpresa.TipoProceso = 1;
                            string _MsgError = "";

                            if (ObjEmpresa.IdRol != 1)
                            {
                                if (Int32.Parse(Session["CantidadEmpresaRegistradas"].ToString().Trim()) < Int32.Parse(Session["CanEmpresaHijasRegistrar"].ToString().Trim()))
                                {
                                    Session["CantidadEmpresaRegistradas"] = Int32.Parse(Session["CantidadEmpresaRegistradas"].ToString().Trim()) + 1;
                                }
                                else
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
                                    _MsgError = "Señor Usuario: no puede ingresar mas empresas. Ya llego a su limite de registro ...!";
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
                                    return;
                                }
                            }

                            if (ObjEmpresa.AddUpEmpresa(NuevaFila, ref _IdEmpresa, ref _MsgError))
                            {
                                NuevaFila["idempresa_hija"] = _IdEmpresa;
                                NuevaFila["nit_empresa"] = NuevaFila["nit_empresa"].ToString().Trim().ToUpper();
                                NuevaFila["nombre_empresa"] = NuevaFila["nombre_empresa"].ToString().Trim().ToUpper();
                                NuevaFila["emblema_empresa"] = NuevaFila["emblema_empresa"].ToString().Trim().ToUpper();
                                NuevaFila["direccion_empresa"] = NuevaFila["direccion_empresa"].ToString().Trim().ToUpper();
                                NuevaFila["telefono_empresa"] = NuevaFila["telefono_empresa"].ToString().Trim().ToUpper();
                                NuevaFila["cant_empresas_registrar"] = NuevaFila["cant_empresas_registrar"].ToString().Trim().ToUpper();
                                NuevaFila["empresa_unica"] = NuevaFila["empresa_unica"].ToString().Trim().ToUpper();
                                NuevaFila["id_pais"] = NuevaFila["id_pais"].ToString().Trim();
                                NuevaFila["id_dpto"] = NuevaFila["id_dpto"].ToString().Trim();
                                NuevaFila["id_municipio"] = NuevaFila["id_municipio"].ToString().Trim();
                                NuevaFila["id_estado"] = NuevaFila["id_estado"].ToString().Trim();

                                nit_empresa = NuevaFila["nit_empresa"].ToString().Trim().ToUpper();
                                nombre_empresa = NuevaFila["nombre_empresa"].ToString().Trim().ToUpper();
                                emblema_empresa = NuevaFila["emblema_empresa"].ToString().Trim().ToUpper();
                                direccion_empresa = NuevaFila["direccion_empresa"].ToString().Trim().ToUpper();
                                telefono_empresa = NuevaFila["telefono_empresa"].ToString().Trim();
                                email_empresa = NuevaFila["email_empresa"].ToString().Trim().ToUpper();
                                this.FuenteDatosSedes.Tables["DtEmpresasHijas"].Rows.Add(NuevaFila);

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
                            }
                            else
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
                            }
                        }
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
                        string _MsgError = "Error al registrar la empresa Hija. Motivo: " + ex.Message;
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
                    }
                    break;
            }
        }

        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            DataRow[] changedRows = null;
            Hashtable newValues = null;
            int _IdEmpresa = 0;

            switch (e.Item.OwnerTableView.Name)
            {
                case "DtEmpresas":
                    TablaDatos = this.FuenteDatosEmpresas.Tables["DtEmpresas"];
                    TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_empresa"] };
                    changedRows = TablaDatos.Select("id_empresa = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id_empresa"].ToString().Trim()));
                    _IdEmpresa = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["id_empresa"].ToString().Trim());

                    if (changedRows.Length != 1)
                    {
                        e.Canceled = true;
                        return;
                    }

                    newValues = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);
                    changedRows[0].BeginEdit();
                    try
                    {
                        foreach (DictionaryEntry entry in newValues)
                        {
                            changedRows[0][(string)entry.Key] = entry.Value;
                        }

                        _IdEmpresa = Int32.Parse(changedRows[0]["id_empresa"].ToString().Trim());
                        ObjEmpresa.IdEmpresa = _IdEmpresa;
                        ObjEmpresa.IdEmpresaPadre = null; //Convert.ToInt32(Session["IdEmpresaAdmon"].ToString().Trim());
                        ObjEmpresa.IdUsuarioAdd = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                        ObjEmpresa.IdUsuarioUp = Session["IdUsuario"].ToString().Trim();
                        ObjEmpresa.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                        ObjEmpresa.TipoEmpresa = "P";
                        ObjEmpresa.EmpresaUnica = "";
                        ObjEmpresa.TipoProceso = 2;
                        string _MsgError = "";

                        if (ObjEmpresa.AddUpEmpresa(changedRows[0], ref _IdEmpresa, ref _MsgError))
                        {
                            this.FuenteDatosEmpresas.Tables["DtEmpresas"].Rows[0].AcceptChanges();
                            this.FuenteDatosEmpresas.Tables["DtEmpresas"].Rows[0].EndEdit();

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
                        }
                        else
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
                        }
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
                        string _MsgError = "Error al editar los datos de la empresa padre. Motivo: " + ex.Message;
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
                    }
                    break;

                case "DtEmpresasHijas":
                    TablaDatos = this.FuenteDatosSedes.Tables["DtEmpresasHijas"];
                    TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idempresa_hija"] };
                    changedRows = TablaDatos.Select("idempresa_hija = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["idempresa_hija"].ToString().Trim()));
                    _IdEmpresa = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["idempresa_hija"].ToString().Trim());

                    if (changedRows.Length != 1)
                    {
                        e.Canceled = true;
                        return;
                    }

                    newValues = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);
                    changedRows[0].BeginEdit();
                    try
                    {
                        foreach (DictionaryEntry entry in newValues)
                        {
                            changedRows[0][(string)entry.Key] = entry.Value;
                        }

                        _IdEmpresa = Int32.Parse(changedRows[0]["idempresa_hija"].ToString().Trim());
                        ObjEmpresa.IdEmpresa = _IdEmpresa;
                        ObjEmpresa.IdEmpresaPadre = Convert.ToInt32(this.ViewState["IdEmpresaPadre"].ToString().Trim());
                        ObjEmpresa.IdUsuarioAdd = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                        ObjEmpresa.IdUsuarioUp = Session["IdUsuario"].ToString().Trim();
                        ObjEmpresa.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                        ObjEmpresa.TipoEmpresa = "P";
                        ObjEmpresa.EmpresaUnica = "";
                        ObjEmpresa.TipoProceso = 2;
                        string _MsgError = "";

                        if (ObjEmpresa.AddUpEmpresa(changedRows[0], ref _IdEmpresa, ref _MsgError))
                        {
                            this.FuenteDatosSedes.Tables["DtEmpresasHijas"].Rows[0].AcceptChanges();
                            this.FuenteDatosSedes.Tables["DtEmpresasHijas"].Rows[0].EndEdit();

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
                        }
                        else
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
                        }
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
                        string _MsgError = "Error al editar los datos de la empresa hija. Motivo: " + ex.Message;
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
                    }
                    break;
            }
        }

        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            DataRow[] changedRows = null;
            string strDataEliminada = "";
            int _IdEmpresa = 0;

            switch (e.Item.OwnerTableView.Name)
            {
                case "DtEmpresas":
                    try
                    {
                        TablaDatos = this.FuenteDatosEmpresas.Tables["DtEmpresas"];
                        TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_empresa"] };
                        changedRows = TablaDatos.Select("id_empresa = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id_empresa"].ToString().Trim()));
                        _IdEmpresa = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["id_empresa"].ToString().Trim());

                        ObjEmpresa.IdEmpresa = _IdEmpresa;
                        ObjEmpresa.IdEmpresaPadre = Convert.ToInt32(Session["IdEmpresaAdmon"].ToString().Trim());
                        ObjEmpresa.IdUsuarioAdd = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                        ObjEmpresa.IdUsuarioUp = Session["IdUsuario"].ToString().Trim();
                        ObjEmpresa.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                        ObjEmpresa.TipoEmpresa = "P";
                        ObjEmpresa.EmpresaUnica = "";
                        ObjEmpresa.TipoProceso = 3;
                        string _MsgError = "";

                        if (ObjEmpresa.AddUpEmpresa(changedRows[0], ref _IdEmpresa, ref _MsgError))
                        {
                            strDataEliminada = changedRows[0]["id_empresa"] + "|" + changedRows[0]["nit_empresa"].ToString().Trim() + "|" + changedRows[0]["nombre_empresa"].ToString().Trim() + "|" + changedRows[0]["direccion_empresa"].ToString().Trim();
                            this.FuenteDatosEmpresas.Tables["DtEmpresas"].Rows.Find(_IdEmpresa).Delete();
                            this.FuenteDatosEmpresas.Tables["DtEmpresas"].AcceptChanges();

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
                        }
                        else
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
                        }
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
                        string _MsgError = "Error al eliminar la empresa padre. Motivo: " + ex.Message;
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
                    }
                    break;

                case "DtEmpresasHijas":
                    try
                    {
                        TablaDatos = this.FuenteDatosSedes.Tables["DtEmpresasHijas"];
                        TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idempresa_hija"] };
                        changedRows = TablaDatos.Select("idempresa_hija = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["idempresa_hija"].ToString().Trim()));
                        _IdEmpresa = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["idempresa_hija"].ToString().Trim());

                        ObjEmpresa.IdEmpresa = _IdEmpresa;
                        ObjEmpresa.IdEmpresaPadre = Convert.ToInt32(this.ViewState["IdEmpresaPadre"].ToString().Trim());
                        ObjEmpresa.IdUsuarioAdd = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                        ObjEmpresa.IdUsuarioUp = Session["IdUsuario"].ToString().Trim();
                        ObjEmpresa.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                        ObjEmpresa.TipoEmpresa = "H";
                        ObjEmpresa.TipoProceso = 3;
                        string _MsgError = "";

                        if (ObjEmpresa.AddUpEmpresa(changedRows[0], ref _IdEmpresa, ref _MsgError))
                        {
                            strDataEliminada = changedRows[0]["idempresa_hija"] + "|" + changedRows[0]["nit_empresa"].ToString().Trim() + "|" + changedRows[0]["nombre_empresa"].ToString().Trim() + "|" + changedRows[0]["direccion_empresa"].ToString().Trim();
                            this.FuenteDatosSedes.Tables["DtEmpresasHijas"].Rows.Find(_IdEmpresa).Delete();
                            this.FuenteDatosSedes.Tables["DtEmpresasHijas"].AcceptChanges();

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
                        }
                        else
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
                        }
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
                        string _MsgError = "Error al eliminar la empresa hija. Motivo: " + ex.Message;
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
                    }
                    break;
            }
        }
        #endregion

    }
}