using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Administracion;
using System.Text;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Administracion.Clientes
{
    public partial class CtrlFirmantes : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        #region DEFINICION DE OBJETOS DE CLASES
        Firmantes ObjFirmante = new Firmantes();
        TipoIdentificacion ObjTipoIde = new TipoIdentificacion();
        SistemaRol ObjRol = new SistemaRol();
        TiposFirma ObjTipoFirma = new TiposFirma();
        Estado ObjEstado = new Estado();
        EnvioCorreo ObjCorreo = new EnvioCorreo();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        Utilidades ObjUtils = new Utilidades();
        #endregion

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjFirmante.TipoConsulta = 1;
                ObjFirmante.IdCliente = this.Session["IdCliente"] != null ? Int32.Parse(this.Session["IdCliente"].ToString().Trim()) : 1;
                ObjFirmante.IdFirmante = null;
                ObjFirmante.IdEstado = null;
                ObjFirmante.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar los formularios
                ObjetoDataTable = ObjFirmante.GetAllFirmantes();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_firmante"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los tipos de identificacion
                ObjetoDataTable = new DataTable();
                ObjTipoIde.TipoConsulta = 2;
                ObjTipoIde.IdEstado = 1;
                ObjTipoIde.MostrarSeleccione = "NO";
                ObjTipoIde.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjTipoIde.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjTipoIde.GetIdentificacion();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar roles del sistema.
                ObjetoDataTable = new DataTable();
                ObjRol.TipoConsulta = 3;
                ObjRol.IdEstado = 1;
                ObjRol.MostrarSeleccione = "NO";
                ObjRol.IdRol = Convert.ToInt32(this.Session["IdRol"].ToString().Trim());
                ObjRol.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjRol.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjRol.GetRoles();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los tipos de firmas
                ObjetoDataTable = new DataTable();
                ObjTipoFirma.TipoConsulta = 2;
                ObjTipoFirma.IdEstado = 1;
                ObjTipoFirma.MostrarSeleccione = "NO";
                ObjTipoFirma.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjTipoFirma.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjTipoFirma.GetTipoFirma();
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
                string _MsgError = "Error al listar los firmantes. Motivo: " + ex.ToString();
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

            //Ocultar la columna de empresa siempre y cuando el Rol sea diferente al de Soporte
            if (Int32.Parse(Session["IdRol"].ToString().Trim()) != 1)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 6].Visible = false;
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 7].Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.Page.Title = this.Page.Title + "Firmantes";
                this.AplicarPermisos();
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
                this.RadGrid1.DataMember = "DtFirmantes";

                #region CARGAR LISTAS PARA SELECCIONAR
                GridDropDownColumn columna = new GridDropDownColumn();
                //--Listar los tipos de identificacion
                columna = (GridDropDownColumn)this.RadGrid1.Columns[2];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Tipo";
                columna.DataField = "idtipo_identificacion";
                columna.ListTextField = "tipo_identificacion";
                columna.ListValueField = "idtipo_identificacion";
                columna.ListDataMember = "DtTiposIdentificacion";

                //--Listar los tipos de cargos
                columna = (GridDropDownColumn)this.RadGrid1.Columns[12];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Rol";
                columna.DataField = "id_rol";
                columna.ListTextField = "nombre_rol";
                columna.ListValueField = "id_rol";
                columna.ListDataMember = "DtRoles";

                //--Listar los tipos de cargos
                columna = (GridDropDownColumn)this.RadGrid1.Columns[14];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Firma";
                columna.DataField = "idtipo_firma";
                columna.ListTextField = "tipo_firma";
                columna.ListValueField = "idtipo_firma";
                columna.ListDataMember = "DtTipoFirma";

                //--Listar los estados
                columna = (GridDropDownColumn)this.RadGrid1.Columns[16];
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
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del Firmante. Motivo: " + ex.ToString();
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

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnAddFirma")
                {
                    #region DEFINICION DEL EVENTO CLICK PARA ASIGNAR IMAGEN DE LA FIRMA
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdFirmante = Convert.ToInt32(item.GetDataKeyValue("id_firmante").ToString().Trim());
                    int _IdRol = Convert.ToInt32(item.GetDataKeyValue("id_rol").ToString().Trim());
                    int _IdTipoFirma = Convert.ToInt32(item.GetDataKeyValue("idtipo_firma").ToString().Trim());
                    string _NombreFirmante = item["nombre_completo"].Text.ToString().Trim();

                    if (_IdTipoFirma == 2)   //--ID TIPO FIRMA 2. FIRMA EN AGUA
                    {
                        #region MOSTRAR MENSAJE DE USUARIO
                        //--Mandamos abrir el formulario de registro
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;
                        Ventana.Modal = true;

                        string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                        Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmAddFirma.aspx?IdFirmante=" + _IdFirmante + "&IdRol=" + _IdRol + "&PathUrl=" + _PathUrl;
                        Ventana.ID = "RadWindow12";
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(530);
                        Ventana.Width = Unit.Pixel(950);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Asignar Imagen del Firmante Id: " + _IdFirmante + " - " + _NombreFirmante;
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
                        string _MsgMensaje = "El Tipo de firma no es valido para cargar la imagen. Contacte a soporte técnico !";
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

                    string _ModuloApp = "FIRMANTE_CLIENTE";
                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
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
                string _MsgMensaje = "Error con el evento ItemCommand de la grilla. Motivo: " + ex.ToString();
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
                    #region DEFINICION DE VALIDACION DE CAMPOS
                    GridEditableItem item = (GridEditableItem)e.Item;
                    GridTextBoxColumnEditor editor = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("numero_documento");
                    TableCell cell1 = (TableCell)editor.TextBoxControl.Parent;
                    RequiredFieldValidator validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell1.Controls.Add(validator);
                    editor.Visible = true;

                    //----
                    GridTextBoxColumnEditor editor2 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("nombre_funcionario");
                    TableCell cell = (TableCell)editor2.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor2.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor2.Visible = true;

                    //----
                    GridTextBoxColumnEditor editor3 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("apellido_funcionario");
                    cell = (TableCell)editor3.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor3.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor3.Visible = true;

                    //----
                    GridTextBoxColumnEditor editor4 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("tarjeta_profesional");
                    cell = (TableCell)editor4.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor4.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor4.Visible = true;

                    //----
                    GridTextBoxColumnEditor editor5 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("numero_contacto");
                    cell = (TableCell)editor5.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor5.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor5.Visible = true;

                    //----
                    GridTextBoxColumnEditor editor6 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("email_contacto");
                    cell = (TableCell)editor6.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor6.TextBoxControl.ID;
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
                    string _MsgError = "Error con el evento ItemCreated. Motivo: " + ex.ToString();
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
                string _MsgError = "Error con el evento PageIndexChanged. Motivo: " + ex.ToString();
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

        #region DEFINICION DEL CRUD
        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = this.FuenteDatos.Tables["DtFirmantes"]; ;
            DataRow NuevaFila = TablaDatos.NewRow();
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_firmante"] };
            DataRow[] TodosValores = TablaDatos.Select("", "id_firmante", DataViewRowState.CurrentRows); ;

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
                    string CmbTipoIde = (editedItem["idtipo_identificacion"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbIdRol = (editedItem["id_rol"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbTipoFirma = (editedItem["idtipo_firma"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;

                    ObjFirmante.IdFirmante = null;
                    ObjFirmante.IdCliente = this.Session["IdCliente"] != null ? Int32.Parse(this.Session["IdCliente"].ToString().Trim()) : 1;
                    ObjFirmante.IdTipoIdentificacion = Int32.Parse(NuevaFila["idtipo_identificacion"].ToString().Trim());
                    ObjFirmante.NumeroDocumento = NuevaFila["numero_documento"].ToString().Trim().ToUpper();
                    ObjFirmante.NombreFuncionario = NuevaFila["nombre_funcionario"].ToString().Trim().ToUpper();
                    ObjFirmante.ApellidoFuncionario = NuevaFila["apellido_funcionario"].ToString().Trim().ToUpper();
                    ObjFirmante.Tarjeta_profesional = NuevaFila["tarjeta_profesional"].ToString().Trim().ToUpper();
                    ObjFirmante.NumeroContacto = NuevaFila["numero_contacto"].ToString().Trim().ToUpper();
                    ObjFirmante.EmailContacto = NuevaFila["email_contacto"].ToString().Trim().ToUpper();
                    ObjFirmante.ImagenFirma = null;
                    //--DATOS DEL USUARIO
                    string _ClaveDinamica = ObjUtils.GetRandom();
                    ObjFirmante.PasswordUsuario = _ClaveDinamica;
                    ObjFirmante.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                    ObjFirmante.IdEmpresaPadre = Int32.Parse(this.Session["IdEmpresaAdmon"].ToString().Trim());
                    ObjFirmante.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjFirmante.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjFirmante.TipoProceso = 1;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjFirmante);
                    _log.Warn("REQUEST INSERT FIRMANTE CLIENTE => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjFirmante.AddUpFirmante(NuevaFila, ref _IdRegistro, ref _MsgError))
                    {
                        #region OBTENER DATOS INGRESADOS
                        NuevaFila["id_firmante"] = _IdRegistro;
                        NuevaFila["tipo_identificacion"] = CmbTipoIde.ToString().Trim();
                        NuevaFila["numero_documento"] = NuevaFila["numero_documento"].ToString().Trim().ToUpper();
                        NuevaFila["permite_firmar"] = NuevaFila["permite_firmar"].ToString().Trim();
                        //--Datos para el envio de correo
                        string _LoginUser = NuevaFila["numero_documento"].ToString().Trim().ToUpper();
                        string _NombreUser = NuevaFila["nombre_funcionario"].ToString().Trim().ToUpper() + " " + NuevaFila["apellido_funcionario"].ToString().Trim().ToUpper();
                        string _TarjetaProfesional = NuevaFila["tarjeta_profesional"].ToString().Trim().ToUpper();
                        string _NumeroContacto = NuevaFila["numero_contacto"].ToString().Trim().ToUpper();
                        string _EmailContacto = NuevaFila["email_contacto"].ToString().Trim().ToUpper();
                        string _Rol = CmbIdRol.ToString().Trim();

                        NuevaFila["nombre_completo"] = _NombreUser;
                        NuevaFila["tarjeta_profesional"] = _TarjetaProfesional;
                        NuevaFila["numero_contacto"] = _NumeroContacto;
                        NuevaFila["email_contacto"] = _EmailContacto;
                        NuevaFila["nombre_rol"] = _Rol;
                        NuevaFila["tipo_firma"] = CmbTipoFirma;
                        NuevaFila["codigo_estado"] = CmbEstado.ToString().Trim();
                        NuevaFila["fecha_registro"] = DateTime.Now;
                        this.FuenteDatos.Tables["DtFirmantes"].Rows.Add(NuevaFila);
                        #endregion

                        #region DEFINICION DEL METODO PARA ENVIO DE CORREO
                        //--Definir valores para envio del email
                        ObjCorreo.StrServerCorreo = Session["ServerCorreo"].ToString().Trim();
                        ObjCorreo.PuertoCorreo = Int32.Parse(Session["PuertoCorreo"].ToString().Trim());
                        ObjCorreo.StrEmailDe = Session["EmailSoporte"].ToString().Trim();
                        ObjCorreo.StrPasswordDe = Session["PasswordEmail"].ToString().Trim();
                        ObjCorreo.StrEmailPara = _EmailContacto;
                        ObjCorreo.StrAsunto = "REF.: REGISTRO DE FIRMANTE";

                        string nHora = DateTime.Now.ToString("HH");
                        string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                        StringBuilder strDetalleEmail = new StringBuilder();
                        strDetalleEmail.Append("<h4>" + strTime + ", Para informarle que su usuario fue registrado exitosamente en la Plataforma y poder ingresar a [" + FixedData.PlatformName + "].</h4>" + "<br/>" +
                                        "<h4>DATOS DEL USUARIO REGISTRADO</h2>" + "<br/>" +
                                        "Nombre: " + _NombreUser + "<br/>" +
                                        "No. Contacto: " + _NumeroContacto + "<br/>" +
                                        "Cargo: " + _Rol + "<br/>" +

                                        "Usuario: " + _LoginUser + "<br/>" +
                                        "Password: " + _ClaveDinamica.ToString().Trim() + "<br/>" +
                                        "<br/><br/>" +

                                        "<h4>INFORMACIÓN DE LA EMPRESA</h4>" + "<br/>" +
                                        "Empresa: " + FixedData.NameEmpresa + "<br/>" +
                                        "Dirección: " + this.Session["DireccionEmpresa"].ToString().Trim() + "<br/>" +
                                        "Url Página: <a href=" + FixedData.PlatformUrlPagina + ">" + FixedData.PlatformUrlPagina + "</a>" + "<br/>" +
                                        "Servicio de atención al cliente : " + this.Session["TelefonoEmpresa"].ToString().Trim() + "<br/>" +
                                        "<br/><br/>" +
                                        "Si presenta algun problema o duda de como ingresar al sistema el Administrador del sistema le atendera y ayudara en sus comentarios." + "<br/>" +
                                        "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                        ObjCorreo.StrDetalle = strDetalleEmail.ToString().Trim();
                        string _MsgErrorEmail = "";
                        if (!ObjCorreo.SendEmailUser(ref _MsgErrorEmail))
                        {
                            _MsgError = _MsgError + " Pero " + _MsgErrorEmail.ToString().Trim();
                        }
                        #endregion

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                        ObjAuditoria.ModuloApp = "FIRMANTE_CLIENTE";
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
                string _MsgError = "Error al registrar el Firmante. Motivo: " + ex.ToString();
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

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatos.Tables["DtFirmantes"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_firmante"] };
            DataRow[] changedRows = TablaDatos.Select("id_firmante = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id_firmante"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["id_firmante"].ToString().Trim());

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

                string CmbTipoIde = (editedItem["idtipo_identificacion"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbIdRol = (editedItem["id_rol"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbTipoFirma = (editedItem["idtipo_firma"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;

                ObjFirmante.IdFirmante = _IdRegistro;
                ObjFirmante.IdCliente = this.Session["IdCliente"] != null ? Int32.Parse(this.Session["IdCliente"].ToString().Trim()) : 1;
                ObjFirmante.IdTipoIdentificacion = Int32.Parse(changedRows[0]["idtipo_identificacion"].ToString().Trim());
                ObjFirmante.NumeroDocumento = changedRows[0]["numero_documento"].ToString().Trim().ToUpper();
                ObjFirmante.NombreFuncionario = changedRows[0]["nombre_funcionario"].ToString().Trim().ToUpper();
                ObjFirmante.ApellidoFuncionario = changedRows[0]["apellido_funcionario"].ToString().Trim().ToUpper();
                ObjFirmante.Tarjeta_profesional = changedRows[0]["tarjeta_profesional"].ToString().Trim().ToUpper();
                ObjFirmante.NumeroContacto = changedRows[0]["numero_contacto"].ToString().Trim().ToUpper();
                ObjFirmante.EmailContacto = changedRows[0]["email_contacto"].ToString().Trim().ToUpper();
                ObjFirmante.ImagenFirma = null;
                ObjFirmante.PasswordUsuario = "";
                ObjFirmante.IdRol = 4;  //ID ROL USUARIO CLIENTES
                ObjFirmante.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                ObjFirmante.IdEmpresaPadre = Int32.Parse(this.Session["IdEmpresaAdmon"].ToString().Trim());
                ObjFirmante.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjFirmante.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjFirmante.TipoProceso = 2;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjFirmante);
                _log.Warn("REQUEST UPDATE FIRMANTE CLIENTE => " + jsonRequest);

                string _MsgError = "";
                if (ObjFirmante.AddUpFirmante(changedRows[0], ref _IdRegistro, ref _MsgError))
                {
                    #region DATOS DE LA EDICION
                    changedRows[0]["tipo_identificacion"] = CmbTipoIde.ToString().Trim();
                    changedRows[0]["numero_documento"] = changedRows[0]["numero_documento"].ToString().Trim().ToUpper();
                    changedRows[0]["permite_firmar"] = changedRows[0]["permite_firmar"].ToString().Trim();
                    //--Datos para el envio de correo
                    string _LoginUser = changedRows[0]["numero_documento"].ToString().Trim().ToUpper();
                    string _NombreUser = changedRows[0]["nombre_funcionario"].ToString().Trim().ToUpper() + " " + changedRows[0]["apellido_funcionario"].ToString().Trim().ToUpper();
                    string _TarjetaProfesional = changedRows[0]["tarjeta_profesional"].ToString().Trim().ToUpper();
                    string _NumeroContacto = changedRows[0]["numero_contacto"].ToString().Trim().ToUpper();
                    string _EmailContacto = changedRows[0]["email_contacto"].ToString().Trim().ToUpper();
                    string _Rol = CmbIdRol.ToString().Trim();

                    changedRows[0]["nombre_completo"] = _NombreUser;
                    changedRows[0]["tarjeta_profesional"] = _TarjetaProfesional;
                    changedRows[0]["numero_contacto"] = _NumeroContacto;
                    changedRows[0]["email_contacto"] = _EmailContacto;
                    changedRows[0]["nombre_rol"] = _Rol;
                    changedRows[0]["tipo_firma"] = CmbTipoFirma;
                    changedRows[0]["codigo_estado"] = CmbEstado.ToString().Trim().ToUpper();
                    this.FuenteDatos.Tables["DtFirmantes"].Rows[0].AcceptChanges();
                    this.FuenteDatos.Tables["DtFirmantes"].Rows[0].EndEdit();
                    #endregion

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                    ObjAuditoria.ModuloApp = "FIRMANTE_CLIENTE";
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
                string _MsgError = "Error al editar el Firmante. Motivo: " + ex.ToString();
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

        protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatos.Tables["DtFirmantes"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_firmante"] };
            DataRow[] changedRows = TablaDatos.Select("id_firmante = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id_firmante"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["id_firmante"].ToString().Trim());

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
                ObjFirmante.IdFirmante = _IdRegistro;
                ObjFirmante.IdCliente = this.Session["IdCliente"] != null ? Int32.Parse(this.Session["IdCliente"].ToString().Trim()) : 1;
                ObjFirmante.IdTipoIdentificacion = Int32.Parse(changedRows[0]["idtipo_identificacion"].ToString().Trim());
                ObjFirmante.NumeroDocumento = changedRows[0]["numero_documento"].ToString().Trim().ToUpper();
                ObjFirmante.NombreFuncionario = changedRows[0]["nombre_funcionario"].ToString().Trim().ToUpper();
                ObjFirmante.ApellidoFuncionario = changedRows[0]["apellido_funcionario"].ToString().Trim().ToUpper();
                ObjFirmante.Tarjeta_profesional = changedRows[0]["tarjeta_profesional"].ToString().Trim().ToUpper();
                ObjFirmante.NumeroContacto = changedRows[0]["numero_contacto"].ToString().Trim().ToUpper();
                ObjFirmante.EmailContacto = changedRows[0]["email_contacto"].ToString().Trim().ToUpper();
                ObjFirmante.ImagenFirma = null;
                ObjFirmante.IdRol = 4;  //ID ROL USUARIO CLIENTES
                ObjFirmante.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                ObjFirmante.IdEmpresaPadre = Int32.Parse(this.Session["IdEmpresaAdmon"].ToString().Trim());
                ObjFirmante.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjFirmante.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjFirmante.TipoProceso = 3;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjFirmante);
                _log.Warn("REQUEST DELETE FIRMANTE CLIENTE => " + jsonRequest);

                if (ObjFirmante.AddUpFirmante(changedRows[0], ref _IdRegistro, ref _MsgError))
                {
                    this.FuenteDatos.Tables["DtFirmantes"].Rows.Find(_IdRegistro).Delete();
                    this.FuenteDatos.Tables["DtFirmantes"].AcceptChanges();

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 4;  //--DELETE
                    ObjAuditoria.ModuloApp = "FIRMANTE_CLIENTE";
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
                string _MsgError = "Error al eliminar el Firmante. Motivo: " + ex.ToString();
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