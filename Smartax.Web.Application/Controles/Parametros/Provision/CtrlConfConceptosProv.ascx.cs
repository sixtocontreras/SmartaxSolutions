using log4net;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Parametros.Provision;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Seguridad;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Parametros.Provision
{
    public partial class CtrlConfConceptosProv : UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        ConfConceptosProv ObjConceptosProv = new ConfConceptosProv();
        PlanUnicoCuenta ObjPlanUnicoCuenta = new PlanUnicoCuenta();
        FormConfiguracion ObjFormConfiguracion = new FormConfiguracion();
        ConceptosProv ObjConcep = new ConceptosProv();
        Estado ObjEstado = new Estado();
        Cliente ObjCliente = new Cliente();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjConceptosProv.TipoConsulta = 1;
                ObjConceptosProv.IdEstado = null;
                ObjConceptosProv.IdConceptosProvIca = null;
                ObjConceptosProv.Renglon = null;
                ObjConceptosProv.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjConceptosProv.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar los tipos de regimen
                ObjetoDataTable = ObjConceptosProv.GetAllConfConceptosProv();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_conf_conceptos_prov_ica"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los Clientes
                ObjetoDataTable = new DataTable();
                ObjCliente.TipoConsulta = 2;
                ObjCliente.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                if (ObjCliente.IdCliente != null)
                {
                    ObjCliente.MostrarSeleccione = "NO";
                }
                else
                {
                    ObjCliente.MostrarSeleccione = "SI";
                }
                ObjCliente.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjCliente.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjCliente.GetClientes();
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

                //Mostrar los conceptos 
                ObjetoDataTable = new DataTable();
                ObjConcep.TipoConsulta = 2;
                ObjConcep.MostrarSeleccione = "NO";
                ObjConcep.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjConcep.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjConcep.GetConceptosProv();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar reglones
                ObjetoDataTable = new DataTable();
                ObjFormConfiguracion.TipoConsulta = 2;
                ObjFormConfiguracion.MostrarSeleccione = "NO";
                ObjFormConfiguracion.IdFormularioImpuesto = ConfigurationManager.AppSettings["ID_FORMULARIO_IMPUESTO"].ToString();
                ObjFormConfiguracion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjFormConfiguracion.GetFormConfiguracionRenglon();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar PUC
                ObjetoDataTable = new DataTable();
                ObjPlanUnicoCuenta.TipoConsulta = 1;
                ObjPlanUnicoCuenta.IdEstado = 1;
                ObjPlanUnicoCuenta.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjPlanUnicoCuenta.GetAllPuc();
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
                string _MsgError = "Error al listar los tipos de convenio. Motivo: " + ex.ToString();
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
                this.Page.Title = this.Page.Title + "Configuración conceptos";
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
                RadGrid1.DataSource = this.FuenteDatos;
                RadGrid1.DataMember = "DtConfConceptosProv";

                GridDropDownColumn columna = new GridDropDownColumn();
                //--MOSTRAR LISTA DE CLEINTES
                columna = (GridDropDownColumn)this.RadGrid1.Columns[2];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Cliente";
                columna.DataField = "id_cliente";
                columna.ListTextField = "nombre_cliente";
                columna.ListValueField = "id_cliente";
                columna.ListDataMember = "DtClientes";

                //--MOSTRAR LISTA DE ESTADOS
                columna = (GridDropDownColumn)this.RadGrid1.Columns[14];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Estado";
                columna.DataField = "id_estado";
                columna.ListTextField = "codigo_estado";
                columna.ListValueField = "id_estado";
                columna.ListDataMember = "DtEstados";

                //--MOSTRAR LISTA DE CONCEPTOS
                columna = (GridDropDownColumn)this.RadGrid1.Columns[5];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Concepto";
                columna.DataField = "id_conceptos_prov_ica";
                columna.ListTextField = "desc_concepto";
                columna.ListValueField = "id_conceptos_prov_ica";
                columna.ListDataMember = "DtConceptosProv";

                //--MOSTRAR LISTA DE REGLONES
                columna = (GridDropDownColumn)this.RadGrid1.Columns[12];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Renglón";
                columna.DataField = "idformulario_configuracion";
                columna.ListTextField = "numero_renglon";
                columna.ListValueField = "idformulario_configuracion";
                columna.ListDataMember = "DtFormConfiguracion";

                //-MOSTRAR CUENTAS DEBITO
                columna = (GridDropDownColumn)this.RadGrid1.Columns[7];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Cuenta Débito";
                columna.DataField = "id_cta_debito";
                columna.ListTextField = "codigo_cuenta";
                columna.ListValueField = "id_puc";
                columna.ListDataMember = "DtPlanUnicoCuentas";

                //-MOSTRAR CUENTAS DEBITO
                columna = (GridDropDownColumn)this.RadGrid1.Columns[9];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Cuenta Crédito";
                columna.DataField = "id_cta_credito";
                columna.ListTextField = "codigo_cuenta";
                columna.ListValueField = "id_puc";
                columna.ListDataMember = "DtPlanUnicoCuentas";

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
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del tipo de comision. Motivo: " + ex.ToString();
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
                if (e.CommandName == "BtnLogsAuditoria")
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

                    string _ModuloApp = "CONF_CONCEPTOS_PROVISION_ICA";
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
                else if (e.CommandName == "BtnEjecProvision")
                {
                    #region DEFINICION DEL EVENTO CLICK PARA EJECUTAR LA PROVISION
                    //--MANDAMOS ABRIR EL FORM COMO POPUP
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = "/Controles/Parametros/Provision/FrmEjecProvision.aspx?PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow12";
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(300);
                    Ventana.Width = Unit.Pixel(600);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Ejecutar Provisión: ";
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
                    GridEditableItem item = (GridEditableItem)e.Item;
                    GridTextBoxColumnEditor editor = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("centro_costo");
                    TableCell cell1 = (TableCell)editor.TextBoxControl.Parent;
                    RequiredFieldValidator validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell1.Controls.Add(validator);
                    editor.Visible = true;

                    //--
                    GridTextBoxColumnEditor editor2 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("vigencia");
                    cell1 = (TableCell)editor2.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor2.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell1.Controls.Add(validator);
                    editor2.Visible = true;
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
                    string _MsgError = "Error con el evento RadGrid1_ItemCreated del tipo de comision. Motivo: " + ex.ToString();
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
                string _MsgError = "Error con el evento RadGrid1_PageIndexChanged del tipo de comision. Motivo: " + ex.ToString();
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
            DataTable TablaDatos = this.FuenteDatos.Tables["DtConfConceptosProv"]; ;
            DataRow NuevaFila = TablaDatos.NewRow();
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_conf_conceptos_prov_ica"] };
            DataRow[] TodosValores = TablaDatos.Select("", "id_conf_conceptos_prov_ica", DataViewRowState.CurrentRows); ;

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
                    string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbCliente = (editedItem["id_cliente"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbConcepto = (editedItem["id_conceptos_prov_ica"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbRenglon = (editedItem["idformulario_configuracion"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbCtaDebito = (editedItem["id_puc_debito"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbCtaCredito = (editedItem["id_puc_credito"].Controls[0] as RadComboBox).SelectedItem.Text;

                    ObjConceptosProv.IdConfConceptosProvIca = null;
                    ObjConceptosProv.IdCliente = NuevaFila["id_cliente"] != null ? NuevaFila["id_cliente"].ToString().Trim() : null;
                    ObjConceptosProv.Vigencia = Int32.Parse(NuevaFila["vigencia"].ToString().Trim());
                    ObjConceptosProv.IdConceptosProvIca = NuevaFila["id_conceptos_prov_ica"].ToString().Trim();
                    ObjConceptosProv.CtaDebito = NuevaFila["id_cta_debito"].ToString().Trim();
                    ObjConceptosProv.CtaCredito = NuevaFila["id_cta_credito"].ToString().Trim();
                    ObjConceptosProv.CentroCosto= NuevaFila["centro_costo"].ToString().Trim().ToUpper();
                    ObjConceptosProv.Renglon = NuevaFila["idformulario_configuracion"].ToString().Trim();
                    ObjConceptosProv.IdEstado = NuevaFila["id_estado"].ToString().Trim();
                    ObjConceptosProv.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjConceptosProv.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                    ObjConceptosProv.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
                    ObjConceptosProv.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjConceptosProv.TipoProceso = 1;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjConceptosProv);
                    _log.Warn("REQUEST INSERT CONF CONCEPTOS PROVISION => " + jsonRequest);

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjConceptosProv.AddUpConceptoProv(NuevaFila, ref _IdRegistro, ref _MsgError))
                    {
                        NuevaFila["id_conf_conceptos_prov_ica"] = _IdRegistro;
                        NuevaFila["nombre_cliente"] = CmbCliente.ToString().Trim().Length > 0 ? CmbCliente.ToString().Trim() : "";
                        NuevaFila["vigencia"] = NuevaFila["vigencia"].ToString().Trim().ToUpper();
                        NuevaFila["id_conceptos_prov_ica"] = NuevaFila["id_conceptos_prov_ica"].ToString().Trim().ToUpper();
                        NuevaFila["desc_concepto"] = CmbConcepto.ToString().Trim();
                        NuevaFila["id_cta_debito"] = NuevaFila["id_cta_debito"].ToString().Trim().ToUpper();
                        NuevaFila["cta_debito"] = CmbCtaDebito.ToString().Trim();
                        NuevaFila["id_cta_credito"] = NuevaFila["id_cta_credito"].ToString().Trim().ToUpper();
                        NuevaFila["cta_credito"] = CmbCtaCredito.ToString().Trim();
                        NuevaFila["centro_costo"] = NuevaFila["centro_costo"].ToString().Trim().ToUpper();
                        NuevaFila["renglon"] = NuevaFila["idformulario_configuracion"].ToString().Trim().ToUpper();
                        NuevaFila["numero_renglon"] = CmbRenglon.ToString().Trim();
                        NuevaFila["codigo_estado"] = CmbEstado.ToString().Trim();
                        NuevaFila["fecha_registro"] = DateTime.Now;
                        this.FuenteDatos.Tables["DtConfConceptosProv"].Rows.Add(NuevaFila);

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                        ObjAuditoria.ModuloApp = "CONF_CONCEPTOS_PROVISION_ICA";
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
                string _MsgError = "Error al registrar el tipo de comision. Motivo: " + ex.ToString();
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
            TablaDatos = this.FuenteDatos.Tables["DtConfConceptosProv"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_conf_conceptos_prov_ica"] };
            DataRow[] changedRows = TablaDatos.Select("id_conf_conceptos_prov_ica = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id_conf_conceptos_prov_ica"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["id_conf_conceptos_prov_ica"].ToString().Trim());

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

                string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbCliente = (editedItem["id_cliente"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbConcepto = (editedItem["id_conceptos_prov_ica"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbRenglon = (editedItem["idformulario_configuracion"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbCtaDebito = (editedItem["id_puc_debito"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbCtaCredito = (editedItem["id_puc_credito"].Controls[0] as RadComboBox).SelectedItem.Text;

                ObjConceptosProv.IdConceptosProvIca = _IdRegistro;
                ObjConceptosProv.IdCliente = changedRows[0]["id_cliente"] != null ? changedRows[0]["id_cliente"].ToString().Trim() : null;
                ObjConceptosProv.Vigencia = Int32.Parse(changedRows[0]["vigencia"].ToString().Trim());
                ObjConceptosProv.IdConceptosProvIca = changedRows[0]["id_conceptos_prov_ica"].ToString().Trim();
                ObjConceptosProv.CtaDebito = changedRows[0]["id_cta_debito"].ToString().Trim();
                ObjConceptosProv.CtaCredito = changedRows[0]["id_cta_credito"].ToString().Trim();
                ObjConceptosProv.CentroCosto = changedRows[0]["centro_costo"].ToString().Trim().ToUpper();
                ObjConceptosProv.Renglon = changedRows[0]["idformulario_configuracion"].ToString().Trim();
                ObjConceptosProv.IdEstado = changedRows[0]["id_estado"].ToString().Trim();
                ObjConceptosProv.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjConceptosProv.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                ObjConceptosProv.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
                ObjConceptosProv.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjConceptosProv.TipoProceso = 2;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjConceptosProv);
                _log.Warn("REQUEST UPDATE CONF CONCEPTOS PROVISION => " + jsonRequest);

                string _MsgError = "";
                if (ObjConceptosProv.AddUpConceptoProv(changedRows[0], ref _IdRegistro, ref _MsgError))
                {
                    changedRows[0]["nombre_cliente"] = CmbCliente.ToString().Trim().ToUpper();
                    changedRows[0]["vigencia"] = changedRows[0]["vigencia"].ToString().Trim().ToUpper();
                    changedRows[0]["id_conceptos_prov_ica"] = changedRows[0]["id_conceptos_prov_ica"].ToString().Trim().ToUpper();
                    changedRows[0]["desc_concepto"] = CmbConcepto.ToString().Trim().ToUpper();
                    changedRows[0]["id_cta_debito"] = changedRows[0]["id_cta_debito"].ToString().Trim().ToUpper();
                    changedRows[0]["cta_debito"] = CmbCtaDebito.ToString().Trim().ToUpper();
                    changedRows[0]["id_cta_credito"] = changedRows[0]["id_cta_credito"].ToString().Trim().ToUpper();
                    changedRows[0]["cta_credito"] = CmbCtaCredito.ToString().Trim().ToUpper();
                    changedRows[0]["centro_costo"] = changedRows[0]["centro_costo"].ToString().Trim().ToUpper();
                    changedRows[0]["renglon"] = changedRows[0]["renglon"].ToString().Trim().ToUpper();
                    changedRows[0]["numero_renglon"] = CmbRenglon.ToString().Trim().ToUpper();
                    changedRows[0]["codigo_estado"] = CmbEstado.ToString().Trim().ToUpper();
                    this.FuenteDatos.Tables["DtConfConceptosProv"].Rows[0].AcceptChanges();
                    this.FuenteDatos.Tables["DtConfConceptosProv"].Rows[0].EndEdit();

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                    ObjAuditoria.ModuloApp = "CONF_CONCEPTOS_PROVISION_ICA";
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
                string _MsgError = "Error al editar el tipo de comision. Motivo: " + ex.ToString();
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

        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatos.Tables["DtConfConceptosProv"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_conf_conceptos_prov_ica"] };
            DataRow[] changedRows = TablaDatos.Select("id_conf_conceptos_prov_ica = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id_conf_conceptos_prov_ica"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["id_conf_conceptos_prov_ica"].ToString().Trim());

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
                ObjConceptosProv.IdConfConceptosProvIca = _IdRegistro;
                ObjConceptosProv.IdCliente = changedRows[0]["id_cliente"] != null ? changedRows[0]["id_cliente"].ToString().Trim() : null;
                ObjConceptosProv.Vigencia = Int32.Parse(changedRows[0]["vigencia"].ToString().Trim());
                ObjConceptosProv.IdConceptosProvIca = changedRows[0]["id_conceptos_prov_ica"].ToString().Trim(); 
                ObjConceptosProv.CtaDebito = changedRows[0]["id_cta_debito"].ToString().Trim();
                ObjConceptosProv.CtaCredito = changedRows[0]["id_cta_credito"].ToString().Trim();
                ObjConceptosProv.CentroCosto = changedRows[0]["centro_costo"].ToString().Trim();
                ObjConceptosProv.Renglon = changedRows[0]["idformulario_configuracion"].ToString().Trim();
                ObjConceptosProv.IdEstado = changedRows[0]["id_estado"].ToString().Trim();
                ObjConceptosProv.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjConceptosProv.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                ObjConceptosProv.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
                ObjConceptosProv.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjConceptosProv.TipoProceso = 3;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjConceptosProv);
                _log.Warn("REQUEST DELETE CONF CONCEPTOS PROVISION => " + jsonRequest);

                if (ObjConceptosProv.AddUpConceptoProv(changedRows[0], ref _IdRegistro, ref _MsgError))
                {
                    this.FuenteDatos.Tables["DtConfConceptosProv"].Rows.Find(_IdRegistro).Delete();
                    this.FuenteDatos.Tables["DtConfConceptosProv"].AcceptChanges();

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 4;  //--DELETE
                    ObjAuditoria.ModuloApp = "COSNCEPTOS PROVIOSIÓN";
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
                string _MsgError = "Error al eliminar el tipo de comision. Motivo: " + ex.ToString();
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