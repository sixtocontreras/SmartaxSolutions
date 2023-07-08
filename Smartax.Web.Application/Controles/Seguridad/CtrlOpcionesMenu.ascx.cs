using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Seguridad;

namespace Smartax.Web.Application.Controles.Seguridad
{
    public partial class CtrlOpcionesMenu : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        SistemaNavegacion ObjNavegacion = new SistemaNavegacion();
        TipoMenu ObjTipo = new TipoMenu();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();

        public DataSet GetObtenerDatos()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                //Mostrar las opciones de menu
                ObjNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                //ObjNavegacion.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());

                ObjetoDataTable = ObjNavegacion.GetAllNavegacion();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_navegacion"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar las Opciones de Menu padres
                ObjetoDataTable = new DataTable();
                ObjNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjNavegacion.MostrarSeleccione = "SI";
                ObjetoDataTable = ObjNavegacion.GetPadreNavegacion();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los tipos de menu
                ObjetoDataTable = new DataTable();
                ObjTipo.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjTipo.MostrarSeleccione = "NO";
                ObjetoDataTable = ObjTipo.GetTipoMenu();
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
                string _MsgError = "Error al cargar los datos. Motivo: " + ex.ToString();
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
                _log.Error(_MsgError);
                #endregion
            }

            return ObjetoDataSet;
        }

        private DataSet FuenteDatosGrilla
        {
            get
            {
                object obj = this.ViewState["_FuenteDatosGrilla"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetObtenerDatos();
                    this.ViewState["_FuenteDatosGrilla"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatosGrilla"];
                }
            }
            set { this.ViewState["_FuenteDatosGrilla"] = value; }
        }

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            objPermiso.PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();

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
                this.Page.Title = this.Page.Title + "Opciones de Menu";
                this.AplicarPermisos();
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
                //this.Aviso1.OcultarAviso();
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
        }

        #region METODOS DEL GRID
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid1.DataSource = this.FuenteDatosGrilla;
                RadGrid1.DataMember = "DtNavegacion";

                GridDropDownColumn columna = new GridDropDownColumn();
                columna = (GridDropDownColumn)this.RadGrid1.Columns[5];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Padre";
                columna.DataField = "padre_id";
                columna.ListTextField = "titulo_opcion";
                columna.ListValueField = "padre_id";
                columna.ListDataMember = "DtPadreNavegacion";

                columna = (GridDropDownColumn)this.RadGrid1.Columns[8];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Tipo Opción";
                columna.DataField = "idtipo_menu";
                columna.ListTextField = "tipo_menu";
                columna.ListValueField = "idtipo_menu";
                columna.ListDataMember = "DtTipoMenu";
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
                string _MsgError = "Error con el metodo NeedDataSource. Motivo: " + ex.ToString();
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
                _log.Error(_MsgError);
                #endregion
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                    this.RadWindowManager1.Enabled = false;
                    this.RadWindowManager1.EnableAjaxSkinRendering = false;
                    this.RadWindowManager1.Visible = false;
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
                string _MsgMensaje = "Error con el evento ItemCommand del grid. Motivo: " + ex.ToString();
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
                _log.Error(_MsgMensaje.Trim());
                #endregion
            }
        }

        protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                try
                {
                    #region DEFINICION DE CAMPOS A VALIDAR
                    GridEditableItem item = (GridEditableItem)e.Item;
                    GridTextBoxColumnEditor editor = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("titulo_opcion");
                    TableCell cell = (TableCell)editor.TextBoxControl.Parent;
                    RequiredFieldValidator validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor.Visible = true;

                    //----
                    GridTextBoxColumnEditor editor2 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("descripcion_opcion");
                    cell = (TableCell)editor2.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    //editor2.TextBoxControl.ID = "editor_Nombre";
                    validator.ControlToValidate = editor2.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);

                    //----
                    GridTextBoxColumnEditor editor3 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("url_opcion");
                    cell = (TableCell)editor3.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor3.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);

                    //----
                    GridNumericColumnEditor editor4 = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("orden_opcion");
                    cell = (TableCell)editor4.NumericTextBox.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor4.NumericTextBox.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor4.Visible = true;
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
                    string _MsgError = "Error con el metodo ItemCreated. Motivo: " + ex.ToString();
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
                string _MsgError = "Error con el metodo PageIndex. Motivo: " + ex.ToString();
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
                _log.Error(_MsgError);
                #endregion
            }
        }
        #endregion

        #region DEFINICION DEL CRUD DE NAVEHACION
        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = this.FuenteDatosGrilla.Tables["DtNavegacion"]; ;
            DataRow NuevaFila = TablaDatos.NewRow();
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_navegacion"] };
            DataRow[] TodosValores = TablaDatos.Select("", "id_navegacion", DataViewRowState.CurrentRows); ;

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
                    string CmbTipoMenu = (editedItem["idtipo_menu"].Controls[0] as RadComboBox).SelectedItem.Text;

                    int _IdRegistro = 0;
                    ObjNavegacion.IdNavegacion = null;
                    ObjNavegacion.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    //ObjNavegacion.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                    ObjNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                    ObjNavegacion.TipoProceso = 1;

                    string _MsgError = "";
                    if (ObjNavegacion.AddUpNavegacion(NuevaFila, ref _IdRegistro, ref _MsgError))
                    {
                        NuevaFila["id_navegacion"] = _IdRegistro;
                        NuevaFila["titulo_opcion"] = NuevaFila["titulo_opcion"].ToString().Trim();
                        NuevaFila["descripcion_opcion"] = NuevaFila["descripcion_opcion"].ToString().Trim();
                        NuevaFila["url_opcion"] = NuevaFila["url_opcion"].ToString().Trim();
                        NuevaFila["padre_id"] = NuevaFila["padre_id"];
                        NuevaFila["orden_opcion"] = NuevaFila["orden_opcion"].ToString().Trim();
                        NuevaFila["tipo_menu"] = CmbTipoMenu.ToString().Trim();
                        this.FuenteDatosGrilla.Tables["DtNavegacion"].Rows.Add(NuevaFila);

                        //Aqui registramos en el log de auditoria
                        //ObjAuditoria.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                        //ObjAuditoria.IdUsuario = Convert.ToInt32(Session["IdUsuario"]);
                        ////ObjAuditoria.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
                        //ObjAuditoria.PaginaWeb = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                        //ObjAuditoria.Descripcion = NuevaFila["id_navegacion"].ToString().Trim() + "|" + NuevaFila["titulo_opcion"].ToString().Trim().ToLower() + "|" + NuevaFila["descripcion_opcion"].ToString().Trim().ToLower() + "|" + NuevaFila["url_opcion"].ToString().Trim();
                        //ObjAuditoria.Ip = ObjUtils.GetIPAddress().ToString().Trim();
                        //ObjAuditoria.Tipo = "WEB";
                        //ObjAuditoria.TipoProceso = 1;
                        //ObjAuditoria.Evento = "AGREGAR OPCION MENU";

                        ////'Agregar Auditoria del sistema
                        //string _MsgErrorLogs = "";
                        //if (!ObjAuditoria.AddAuditoria(ref _MsgErrorLogs))
                        //{
                        //    _log.Info(_MsgErrorLogs);
                        //}

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
                        _log.Error(_MsgError);
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
                        Ventana.Height = Unit.Pixel(230);
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
                string _MsgError = "Error con el registro. Motivo: " + ex.Message;
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
                _log.Error(_MsgError);
                e.Canceled = true;
                #endregion
            }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatosGrilla.Tables["DtNavegacion"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_navegacion"] };
            int _IdNavegacion = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["id_navegacion"].ToString().Trim());
            DataRow[] changedRows = TablaDatos.Select("id_navegacion = " + _IdNavegacion);

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

                string CmbTipoMenu = (editedItem["idtipo_menu"].Controls[0] as RadComboBox).SelectedItem.Text;
                ObjNavegacion.IdNavegacion = _IdNavegacion;
                ObjNavegacion.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                //ObjNavegacion.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjNavegacion.TipoProceso = 2;
                string _MsgError = "";

                if (ObjNavegacion.AddUpNavegacion(changedRows[0], ref _IdNavegacion, ref _MsgError))
                {
                    changedRows[0]["tipo_menu"] = CmbTipoMenu.ToString().Trim().ToUpper();
                    this.FuenteDatosGrilla.Tables["DtNavegacion"].Rows[0].AcceptChanges();
                    this.FuenteDatosGrilla.Tables["DtNavegacion"].Rows[0].EndEdit();

                    //Aqui registramos en el log de auditoria
                    //ObjAuditoria.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                    //ObjAuditoria.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    ////ObjAuditoria.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                    //ObjAuditoria.PaginaWeb = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    //ObjAuditoria.Descripcion = changedRows[0]["id_navegacion"].ToString().Trim() + "|" + changedRows[0]["titulo_opcion"].ToString().Trim().ToLower() + "|" + changedRows[0]["descripcion_opcion"].ToString().Trim().ToLower();
                    //ObjAuditoria.Ip = ObjUtils.GetIPAddress().ToString().Trim();
                    //ObjAuditoria.Tipo = "WEB";
                    //ObjAuditoria.TipoProceso = 1;
                    //ObjAuditoria.Evento = "EDITAR OPCION MENU";

                    ////'Agregar Auditoria del sistema
                    //string _MsgErrorLogs = "";
                    //if (!ObjAuditoria.AddAuditoria(ref _MsgErrorLogs))
                    //{
                    //    _log.Error(_MsgErrorLogs);
                    //}

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
                string _MsgError = "Error con la edición. Motivo: " + ex.Message;
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
                _log.Error(_MsgError);
                e.Canceled = true;
                #endregion
            }
        }

        protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            int _IdNavegacion = 0;
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatosGrilla.Tables["DtNavegacion"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_navegacion"] };
            _IdNavegacion = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["id_navegacion"].ToString().Trim());
            DataRow[] changedRows = TablaDatos.Select("id_navegacion = " + _IdNavegacion);

            try
            {
                string _MsgError = "";
                ObjNavegacion.IdNavegacion = _IdNavegacion;
                ObjNavegacion.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                //ObjNavegacion.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjNavegacion.TipoProceso = 3;

                if (ObjNavegacion.AddUpNavegacion(changedRows[0], ref _IdNavegacion, ref _MsgError))
                {
                    string strDescripcion = changedRows[0]["id_navegacion"].ToString().Trim() + "|" + changedRows[0]["titulo_opcion"].ToString().Trim().ToLower() + "|" + changedRows[0]["descripcion_opcion"].ToString().Trim().ToLower();
                    this.FuenteDatosGrilla.Tables["DtNavegacion"].Rows.Find(_IdNavegacion).Delete();
                    this.FuenteDatosGrilla.Tables["DtNavegacion"].AcceptChanges();

                    //Aqui registramos en el log de auditoria
                    //ObjAuditoria.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                    //ObjAuditoria.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    ////ObjAuditoria.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                    //ObjAuditoria.PaginaWeb = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    //ObjAuditoria.Descripcion = strDescripcion;
                    //ObjAuditoria.Ip = ObjUtils.GetIPAddress().ToString().Trim();
                    //ObjAuditoria.Tipo = "WEB";
                    //ObjAuditoria.TipoProceso = 1;
                    //ObjAuditoria.Evento = "ELIMINACION DE OPCION";

                    ////'Agregar Auditoria del sistema
                    //string _MsgErrorLogs = "";
                    //if (!ObjAuditoria.AddAuditoria(ref _MsgErrorLogs))
                    //{
                    //    _log.Error(_MsgErrorLogs);
                    //}

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
                string _MsgError = "Error al eliminar. Motivo: " + ex.Message;
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
                _log.Error(_MsgError);
                e.Canceled = true;
                #endregion
            }
        }
        #endregion

    }
}