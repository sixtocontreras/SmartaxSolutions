using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.IO;
using System.IO.Compression;
using Telerik.Web.UI;
using Telerik.Web.UI.PersistenceFramework;
using log4net;
using log4net.Config;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros;

namespace Smartax.Web.Application.Controles.Parametros.Empresa
{
    public partial class CtrlParametroSistema : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        ParametrosSistema ObjParametro = new ParametrosSistema();
        TipoLicencia ObjTipoLicencia = new TipoLicencia();
        Empresas ObjEmpresa = new Empresas();
        Estado ObjEstado = new Estado();
        Utilidades ObjUtils = new Utilidades();

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjParametro.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjParametro.IdEstado = null;

                //Mostrar los parametros del sistema asignado a las empresas
                ObjetoDataTable = ObjParametro.GetAllParametrosSistema();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idparametro_sistema"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar empresas
                ObjetoDataTable = new DataTable();
                ObjEmpresa.TipoConsulta = 2;
                ObjEmpresa.IdRol = Int32.Parse(Session["IdRol"].ToString().Trim());
                ObjEmpresa.IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString().Trim());
                ObjEmpresa.IdEstado = 1;
                ObjEmpresa.MostrarSeleccione = "NO";
                ObjEmpresa.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjEmpresa.GetEmpresas();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los tipos de licencias
                ObjetoDataTable = new DataTable();
                ObjTipoLicencia.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjTipoLicencia.IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString().Trim());
                ObjTipoLicencia.IdEstado = 1;
                ObjTipoLicencia.MostrarSeleccione = "NO";
                ObjetoDataTable = ObjTipoLicencia.GetTipoLicencia();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los Estados
                ObjetoDataTable = new DataTable();
                ObjEstado.TipoConsulta = 2;
                ObjEstado.IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString().Trim());
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
                string _MsgError = "Error al cargar los parametros de tipos de licencia. Motivo: " + ex.ToString();
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
                RadGrid1.Columns[RadGrid1.Columns.Count - 2].Visible = false;
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
                this.Page.Title = this.Page.Title + "Parametros Tipo Licencia";
                this.AplicarPermisos();
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid1.DataSource = this.FuenteDatos;
                RadGrid1.DataMember = "DtParametrosSistema";

                GridDropDownColumn columna = new GridDropDownColumn();
                //Listado de Empresas
                columna = (GridDropDownColumn)this.RadGrid1.Columns[2];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Empresa";
                columna.DataField = "id_empresa";
                columna.ListTextField = "nombre_empresa";
                columna.ListValueField = "id_empresa";
                columna.ListDataMember = "DtEmpresas";

                //Listado de Tipo de Licencia
                columna = (GridDropDownColumn)this.RadGrid1.Columns[4];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Parametros Tipo Licencia";
                columna.DataField = "idtipo_licencia";
                columna.ListTextField = "tipo_licencia";
                columna.ListValueField = "idtipo_licencia";
                columna.ListDataMember = "DtTipoLicencia";

                //Listado de Estados
                columna = (GridDropDownColumn)this.RadGrid1.Columns[8];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Estado";
                columna.DataField = "id_estado";
                columna.ListTextField = "codigo_estado";
                columna.ListValueField = "id_estado";
                columna.ListDataMember = "DtEstados";
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
                string _MsgError = "Error con el evento NeedDataSource. Motivo: " + ex.Message;
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

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "BtnVerInfo")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdParametro = Int32.Parse(item.GetDataKeyValue("idparametro_sistema").ToString().Trim());
                    string strTipoLicencia = item["tipo_licencia"].Text.ToString().Trim();

                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/ControlUsuario/Parametros/FrmVerInfoParametrosLicencia.aspx?IdParametroSistema=" + _IdParametro;
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(430);
                    Ventana.Width = Unit.Pixel(750);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Tipo Licencia: " + strTipoLicencia;
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
                string _MsgError = "Error con el evento ItemCommand. Motivo: " + ex.Message;
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
                string _MsgError = "Error con el evento PageIndexChanged. Motivo: " + ex.Message;
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

        #region DEFINICION DEL CRUD
        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = this.FuenteDatos.Tables["DtParametrosSistema"]; ;
            DataRow NuevaFila = TablaDatos.NewRow();
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idparametro_sistema"] };
            DataRow[] TodosValores = TablaDatos.Select("", "idparametro_sistema", DataViewRowState.CurrentRows); ;

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
                    string CmbEmpresa = (editedItem["id_empresa"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbTipoLicencia = (editedItem["idtipo_licencia"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    ObjParametro.IdParametroSistema = null;
                    ObjParametro.IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString().Trim());
                    ObjParametro.IdUsuarioAdd = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                    ObjParametro.IdUsuarioUp = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                    ObjParametro.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                    ObjParametro.TipoProceso = 1;

                    if (ObjParametro.AddUpParametroSistema(NuevaFila, ref _IdRegistro, ref _MsgError))
                    {
                        NuevaFila["idparametro_sistema"] = _IdRegistro;
                        NuevaFila["nombre_empresa"] = CmbEmpresa.ToString().Trim().ToUpper();
                        NuevaFila["tipo_licencia"] = CmbTipoLicencia.ToString().Trim().ToUpper();
                        NuevaFila["codigo_estado"] = CmbEstado.ToString().Trim().ToUpper();
                        NuevaFila["fecha_registro"] = DateTime.Now;
                        this.FuenteDatos.Tables["DtParametrosSistema"].Rows.Add(NuevaFila);
                        //--
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
                string _MsgError = "Error al registrar la parametrización del tipo de licencia. Motivo: " + ex.Message;
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
                e.Canceled = true;
                #endregion
            }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatos.Tables["DtParametrosSistema"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idparametro_sistema"] };
            DataRow[] changedRows = TablaDatos.Select("idparametro_sistema = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["idparametro_sistema"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["idparametro_sistema"].ToString().Trim());

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

                string CmbEmpresa = (editedItem["id_empresa"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbTipoLicencia = (editedItem["idtipo_licencia"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;

                string _MsgError = "";
                ObjParametro.IdParametroSistema = _IdRegistro;
                ObjParametro.IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString().Trim());
                ObjParametro.IdUsuarioAdd = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                ObjParametro.IdUsuarioUp = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                ObjParametro.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjParametro.TipoProceso = 2;

                if (ObjParametro.AddUpParametroSistema(changedRows[0], ref _IdRegistro, ref _MsgError))
                {
                    changedRows[0]["nombre_empresa"] = CmbEmpresa.ToString().Trim().ToUpper();
                    changedRows[0]["tipo_licencia"] = CmbTipoLicencia.ToString().Trim().ToUpper();
                    changedRows[0]["codigo_estado"] = CmbEstado.ToString().Trim().ToUpper();
                    this.FuenteDatos.Tables["DtParametrosSistema"].Rows[0].AcceptChanges();
                    this.FuenteDatos.Tables["DtParametrosSistema"].Rows[0].EndEdit();

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
                string _MsgError = "Error al editar la parametrización del tipo de licencia. Motivo: " + ex.Message;
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
                e.Canceled = true;
                #endregion
            }
        }

        protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatos.Tables["DtParametrosSistema"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["idparametro_sistema"] };
            DataRow[] changedRows = TablaDatos.Select("idparametro_sistema = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["idparametro_sistema"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["idparametro_sistema"].ToString().Trim());

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
                ObjParametro.IdParametroSistema = _IdRegistro;
                ObjParametro.IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString().Trim());
                ObjParametro.IdUsuarioAdd = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                ObjParametro.IdUsuarioUp = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                ObjParametro.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjParametro.TipoProceso = 3;

                if (ObjParametro.AddUpParametroSistema(changedRows[0], ref _IdRegistro, ref _MsgError))
                {
                    this.FuenteDatos.Tables["DtParametrosSistema"].Rows.Find(_IdRegistro).Delete();
                    this.FuenteDatos.Tables["DtParametrosSistema"].AcceptChanges();

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
                string _MsgError = "Error al eliminar la parametrización del tipo de licencia. Motivo: " + ex.Message;
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
                e.Canceled = true;
                #endregion
            }
        }
        #endregion

    }
}