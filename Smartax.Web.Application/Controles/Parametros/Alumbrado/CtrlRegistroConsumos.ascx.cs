using log4net;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Parametros.Alumbrado;
using Smartax.Web.Application.Clases.Seguridad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Parametros.Alumbrado
{
    public partial class CtrlRegistroConsumos : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        ConfAlumbrado ObjConfAlumbrado = new ConfAlumbrado();
        ConfConsumos ObjConfConsumos = new ConfConsumos();
        Cliente ObjCliente = new Cliente();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                //Mostrar los tipos de regimen
                ObjetoDataTable = ObjConfConsumos.GetAll();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los Clientes
                ObjetoDataTable = new DataTable();
                ObjCliente.TipoConsulta = 2;
                ObjCliente.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjCliente.MostrarSeleccione = "NO";
                ObjCliente.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjCliente.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjCliente.GetClientes();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                ObjetoDataSet.Tables.Add(ObjConfAlumbrado.GetDptos());

                var dtMeses = new DataTable();
                dtMeses.Columns.Add("id");
                dtMeses.Columns.Add("nombre");
                dtMeses.Rows.Add(1, "Enero");
                dtMeses.Rows.Add(2, "Febrero");
                dtMeses.Rows.Add(3, "Marzo");
                dtMeses.Rows.Add(4, "Abril");
                dtMeses.Rows.Add(5, "Mayo");
                dtMeses.Rows.Add(6, "Junio");
                dtMeses.Rows.Add(7, "Julio");
                dtMeses.Rows.Add(8, "Agosto");
                dtMeses.Rows.Add(9, "Septiembre");
                dtMeses.Rows.Add(10, "Octubre");
                dtMeses.Rows.Add(11, "Noviembre");
                dtMeses.Rows.Add(12, "Diciembre");
                dtMeses.TableName = "DtMes";
                ObjetoDataSet.Tables.Add(dtMeses);

                hdMuni.Value = Newtonsoft.Json.JsonConvert.SerializeObject(ObjConfAlumbrado.GetMunicipios());
                hdOfi.Value = Newtonsoft.Json.JsonConvert.SerializeObject(ObjConfConsumos.GetOficinas());
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
                Ventana.Height = Unit.Pixel(250);
                Ventana.Width = Unit.Pixel(500);
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.Page.Title = this.Page.Title + "REGISTRAR CONSUMOS Y COSTOS DE ALUMBRADO PUBLICO";
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
                RadGrid1.DataMember = "DtConsumos";

                GridDropDownColumn columna = new GridDropDownColumn();
                columna = (GridDropDownColumn)this.RadGrid1.Columns[2];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Cliente";
                columna.DataField = "id_cliente";
                columna.ListTextField = "nombre_cliente";
                columna.ListValueField = "id_cliente";
                columna.ListDataMember = "DtClientes";

                //RadComboBox Field1Combo = this.RadGrid1.FindControl("id_dptoCombo") as RadComboBox;
                //Field1Combo.DataSourceID = this.RadGrid1.DataSourceID;
                //Field1Combo.DataTextField = "nombre_departamento";
                //Field1Combo.DataValueField = "id_dpto";
                //Field1Combo.DataMember = "DtDptos";
                //Field1Combo.DataBind();


                GridDropDownColumn columna2 = new GridDropDownColumn();
                columna = (GridDropDownColumn)this.RadGrid1.Columns[11];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Mes";
                columna.DataField = "id_mes";
                columna.ListTextField = "nombre";
                columna.ListValueField = "id";
                columna.ListDataMember = "DtMes";

                //GridDropDownColumn columna4 = new GridDropDownColumn();
                //columna = (GridDropDownColumn)this.RadGrid1.Columns[7];
                //columna.DataSourceID = this.RadGrid1.DataSourceID;
                //columna.HeaderText = "Municipio";
                //columna.DataField = "id_municipio";
                //columna.ListTextField = "nombre_municipio";
                //columna.ListValueField = "id_municipio";
                //columna.ListDataMember = "DtMunicipios";


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
                Ventana.Height = Unit.Pixel(250);
                Ventana.Width = Unit.Pixel(500);
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

                    string _ModuloApp = "CONF_SECTOR_MUNICIPIO";
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
                else if (e.CommandName == "btnCargueMasivo")
                {
                    #region VISUALIZAR FORMULARIO CONFIGURACION DE CUENTAS
                   

                    //--Mandamos abrir el formulario de registro
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    
                    //this.RadWindowManager1.OnClientClose = "location.reload()";
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = $"/Controles/Administracion/Alumbrado/FrmCargueMasivo.aspx?reload=true&PathUrl=" + _PathUrl;
                    //Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmConfImpuestoIca.aspx?IdClienteImpuesto=" + _IdClienteImpuesto + "&IdFormImpuesto=" + _IdFormImpuesto + "&NombreImpuesto=" + _NombreImpuesto + "&IdCliente=" + _IdCliente + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow12";
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(720);
                    Ventana.Width = Unit.Pixel(1200);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = $"Cargue Masivo";
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
                Ventana.Height = Unit.Pixel(260);
                Ventana.Width = Unit.Pixel(520);
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


                    GridNumericColumnEditor editor = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("vigencia");
                    TableCell cell = (TableCell)editor.NumericTextBox.Parent;
                    RequiredFieldValidator validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor.NumericTextBox.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor.Visible = true;


                    GridNumericColumnEditor editor1 = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("kw_consumo");
                    TableCell cell1 = (TableCell)editor1.NumericTextBox.Parent;
                    RequiredFieldValidator validator1 = new RequiredFieldValidator();
                    validator1.ControlToValidate = editor1.NumericTextBox.ID;
                    validator1.ErrorMessage = "Campo Requerido";
                    validator1.Display = ValidatorDisplay.Dynamic;
                    cell1.Controls.Add(validator1);
                    editor1.Visible = true;

                    GridNumericColumnEditor editor2 = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("kw_hora");
                    TableCell cell2 = (TableCell)editor2.NumericTextBox.Parent;
                    RequiredFieldValidator validator2 = new RequiredFieldValidator();
                    validator2.ControlToValidate = editor2.NumericTextBox.ID;
                    validator2.ErrorMessage = "Campo Requerido";
                    validator2.Display = ValidatorDisplay.Dynamic;
                    cell2.Controls.Add(validator2);
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
                    Ventana.Height = Unit.Pixel(250);
                    Ventana.Width = Unit.Pixel(500);
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
                Ventana.Height = Unit.Pixel(250);
                Ventana.Width = Unit.Pixel(500);
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
            var error = false;
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            string _MsgError = "";
            if ((editedItem["id_dpto"].Controls[1] as RadComboBox).SelectedItem.Value == "")
            {
                _MsgError = "El campo departamento, municipio y Oficina son obligatorios";
                error = true;
            }
            else if (hdMuniSelect.Value == "")
            {

                _MsgError = "El campo municipio y Oficina son obligatorios";
                error = true;
            }
            else if (hdOfiSelected.Value == "")
            {

                _MsgError = "El campo Oficina es obligatorio";
                error = true;
            }
            if (error)
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
                Ventana.Height = Unit.Pixel(250);
                Ventana.Width = Unit.Pixel(500);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Error(_MsgError);
                #endregion
                return;
            }

            string CmbCliente = (editedItem["id_cliente"].Controls[0] as RadComboBox).SelectedItem.Text;
            int idCmbCliente = int.Parse((editedItem["id_cliente"].Controls[0] as RadComboBox).SelectedItem.Value);
            string CmbMes = (editedItem["id_mes"].Controls[0] as RadComboBox).SelectedItem.Text;
            int idCmbMes = int.Parse((editedItem["id_mes"].Controls[0] as RadComboBox).SelectedItem.Value);
            string CmbDpto = (editedItem["id_dpto"].Controls[1] as RadComboBox).SelectedItem.Text;
            int idCmbDpto = int.Parse((editedItem["id_dpto"].Controls[1] as RadComboBox).SelectedItem.Value);
            string CmbMuni = hdMuniSelect.Value.Split('|')[1];
            int idCmbMuni = int.Parse(hdMuniSelect.Value.Split('|')[0]);
            string CmbOfi = hdOfiSelected.Value.Split('|')[1];
            int idCmbOfi = int.Parse(hdOfiSelected.Value.Split('|')[0]);

            DataTable TablaDatos = this.FuenteDatos.Tables["DtConsumos"];


            var ct = TablaDatos.Select($"id_cliente = {idCmbCliente} AND id_municipio = {idCmbMuni} AND id_oficina = {idCmbOfi} AND vigencia = {(editedItem["vigencia"].Controls[0] as RadNumericTextBox).Text} AND id_mes = {idCmbMes}").Count();
            if (ct > 0)
            {
                _MsgError = $"Ya existe registro para el municipio – oficina - vigencia - mes.";
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
                Ventana.Height = Unit.Pixel(250);
                Ventana.Width = Unit.Pixel(500);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Error(_MsgError);
                #endregion
                return;
            }

            DataRow NuevaFila = TablaDatos.NewRow();
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id"] };
            DataRow[] TodosValores = TablaDatos.Select("", "id", DataViewRowState.CurrentRows);

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


                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjConfAlumbrado);
                    _log.Warn("REQUEST INSERT CONF_CONSUMOS_ALUMBRADO => " + jsonRequest);

                    string datafila = $"{idCmbDpto}|{idCmbMuni}|{idCmbOfi}|{NuevaFila["id_cliente"]}|{NuevaFila["Vigencia"]}|{idCmbMes}|{NuevaFila["kw_consumo"]}|{NuevaFila["kw_hora"]}";
                    int _IdRegistro = 0;
                    if (ObjConfConsumos.AddConfEntidad(datafila, Int32.Parse(Session["IdUsuario"].ToString().Trim()), ref _IdRegistro, ref _MsgError))
                    {
                        var munn = ObjConfAlumbrado.GetMunicipios(idCmbDpto);
                        var muniSelect = munn.Select("id_municipio = " + idCmbMuni);
                        var ofi = ObjConfConsumos.GetOficinas(idCmbMuni);
                        var ofiSelect = ofi.Select("id = " + idCmbOfi);
                        NuevaFila["Vigencia"] = NuevaFila["Vigencia"].ToString();
                        NuevaFila["kw_consumo"] = NuevaFila["kw_consumo"].ToString();
                        NuevaFila["kw_hora"] = NuevaFila["kw_hora"].ToString();

                        NuevaFila["nombre_mes"] = CmbMes.ToString().Trim().ToUpper();
                        NuevaFila["id_mes"] = idCmbMes;
                        NuevaFila["nombre_dpto"] = CmbDpto.ToString().Trim().ToUpper();
                        NuevaFila["id_dpto"] = idCmbDpto;
                        NuevaFila["nombre_municipio"] = CmbMuni.ToString().Trim().ToUpper();
                        NuevaFila["id_municipio"] = idCmbMuni;
                        NuevaFila["id_oficina"] = idCmbOfi;
                        NuevaFila["nombre_oficina"] = CmbOfi.ToString().Trim().ToUpper();
                        NuevaFila["nombre_cliente"] = CmbCliente.ToString().Trim().ToUpper();
                        NuevaFila["id_cliente"] = idCmbCliente;
                        NuevaFila["fecha_registro"] = DateTime.Now;
                        NuevaFila["id"] = _IdRegistro;
                        this.FuenteDatos.Tables["DtConsumos"].Rows.Add(NuevaFila);

                        _MsgError = "Registro creado con exito.";
                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                        ObjAuditoria.ModuloApp = "CONF_CONSUMOS_ALUMBRADO";
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
                        Ventana.Height = Unit.Pixel(250);
                        Ventana.Width = Unit.Pixel(500);
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
                        Ventana.Height = Unit.Pixel(250);
                        Ventana.Width = Unit.Pixel(500);
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
                _MsgError = "Error al registrar el tipo de comision. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(250);
                Ventana.Width = Unit.Pixel(500);
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
            TablaDatos = this.FuenteDatos.Tables["DtConsumos"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id"] };
            DataRow[] changedRows = TablaDatos.Select("id = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["id"].ToString().Trim());

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
                int vigencia = int.Parse((editedItem["vigencia"].Controls[0] as RadNumericTextBox).Text);
                double kw_consumo = double.Parse((editedItem["kw_consumo"].Controls[0] as RadNumericTextBox).Text);
                double kw_hora = double.Parse((editedItem["kw_hora"].Controls[0] as RadNumericTextBox).Text);
                string CmbCliente = (editedItem["id_cliente"].Controls[0] as RadComboBox).SelectedItem.Text;
                int idCmbCliente = int.Parse((editedItem["id_cliente"].Controls[0] as RadComboBox).SelectedItem.Value);
                string CmbMes = (editedItem["id_mes"].Controls[0] as RadComboBox).SelectedItem.Text;
                int idCmbMes = int.Parse((editedItem["id_mes"].Controls[0] as RadComboBox).SelectedItem.Value);
                string CmbDpto = (editedItem["id_dpto"].Controls[1] as RadComboBox).SelectedItem.Text;
                int idCmbDpto = int.Parse((editedItem["id_dpto"].Controls[1] as RadComboBox).SelectedItem.Value);
                string CmbMuni = hdMuniSelect.Value.Split('|')[1];
                int idCmbMuni = int.Parse(hdMuniSelect.Value.Split('|')[0]);
                string CmbOfi = hdOfiSelected.Value.Split('|')[1];
                int idCmbOfi = int.Parse(hdOfiSelected.Value.Split('|')[0]);

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjConfAlumbrado);
                _log.Warn("REQUEST UPDATE CONF_CONSUMOS_ALUMBRADO => " + jsonRequest);

                string datafila = $"{idCmbDpto}|{idCmbMuni}|{idCmbOfi}|{idCmbCliente}|{vigencia}|{idCmbMes}|{kw_consumo}|{kw_hora}";
                string _MsgError = "";
                if (ObjConfConsumos.UpConfEntidad(datafila, Int32.Parse(Session["IdUsuario"].ToString().Trim()), ref _IdRegistro, ref _MsgError))
                {
                    changedRows[0]["vigencia"] = changedRows[0]["vigencia"].ToString();
                    changedRows[0]["kw_consumo"] = changedRows[0]["kw_consumo"].ToString();
                    changedRows[0]["kw_hora"] = changedRows[0]["kw_hora"].ToString();
                    changedRows[0]["nombre_mes"] = CmbMes.ToString().Trim().ToUpper();
                    changedRows[0]["id_mes"] = idCmbMes;
                    changedRows[0]["nombre_dpto"] = CmbDpto.ToString().Trim().ToUpper();
                    changedRows[0]["id_dpto"] = idCmbDpto;
                    changedRows[0]["nombre_municipio"] = CmbMuni.ToString().Trim().ToUpper();
                    changedRows[0]["id_municipio"] = idCmbMuni;
                    changedRows[0]["nombre_oficina"] = CmbOfi.ToString().Trim().ToUpper();
                    changedRows[0]["id_oficina"] = idCmbOfi;
                    changedRows[0]["nombre_cliente"] = CmbCliente.ToString().Trim().ToUpper();
                    changedRows[0]["id_cliente"] = idCmbCliente;

                    this.FuenteDatos.Tables["DtConsumos"].Rows[0].AcceptChanges();
                    this.FuenteDatos.Tables["DtConsumos"].Rows[0].EndEdit();

                    _MsgError = "Registro actualizado con exito.";
                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                    ObjAuditoria.ModuloApp = "CONF_CONSUMOS_ALUMBRADO";
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
                    Ventana.Height = Unit.Pixel(250);
                    Ventana.Width = Unit.Pixel(500);
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
                    Ventana.Height = Unit.Pixel(250);
                    Ventana.Width = Unit.Pixel(500);
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
                Ventana.Height = Unit.Pixel(250);
                Ventana.Width = Unit.Pixel(500);
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
            TablaDatos = this.FuenteDatos.Tables["DtConsumos"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id"] };
            DataRow[] changedRows = TablaDatos.Select("id = " + Int32.Parse(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id"].ToString()));
            int _IdRegistro = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["id"].ToString().Trim());

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

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjConfAlumbrado);
                _log.Warn("REQUEST DELETE CONF_CONSUMOS_ALUMBRADO => " + jsonRequest);

                if (ObjConfConsumos.dltConfEntidad(Int32.Parse(Session["IdUsuario"].ToString().Trim()), ref _IdRegistro, ref _MsgError))
                {
                    this.FuenteDatos.Tables["DtConsumos"].Rows.Find(_IdRegistro).Delete();
                    this.FuenteDatos.Tables["DtConsumos"].AcceptChanges();

                    _MsgError = "Registro eliminado con exito.";
                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 4;  //--DELETE
                    ObjAuditoria.ModuloApp = "CONF_CONSUMOS_ALUMBRADO";
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
                    Ventana.Height = Unit.Pixel(250);
                    Ventana.Width = Unit.Pixel(500);
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
                    Ventana.Height = Unit.Pixel(250);
                    Ventana.Width = Unit.Pixel(500);
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
                Ventana.Height = Unit.Pixel(250);
                Ventana.Width = Unit.Pixel(500);
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


        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                RadComboBox Field1Combo = e.Item.FindControl("id_dptoCombo") as RadComboBox;
                var mpio = e.Item.FindControl("id_municipioCombo") as System.Web.UI.HtmlControls.HtmlSelect;
                var ofi = e.Item.FindControl("id_oficinaCombo") as System.Web.UI.HtmlControls.HtmlSelect;
                var dpto = ObjConfAlumbrado.GetDptos();
                foreach (DataRow item in dpto.Rows)
                {
                    Field1Combo.Items.Add(new RadComboBoxItem(item[1].ToString().Trim(), item[0].ToString().Trim()));
                }
                if (e.Item.ItemIndex >= 0)
                {
                    DataRow row = null;

                    var dataItem = (GridEditFormItem)e.Item;
                    var cell = ((TextBox)dataItem["id"].Controls[0]).Text;
                    foreach (DataRow item in ((DataView)((dynamic)e.Item.DataItem).DataView).Table.Rows)
                    {
                        if(item[0].ToString().Trim() == cell)
                        {
                            row = item;
                            break;
                        }
                    }
                    var mun = ObjConfAlumbrado.GetMunicipios(int.Parse(row[1].ToString()));
                    var of = ObjConfConsumos.GetOficinas(int.Parse(row[3].ToString()));
                    var index = 0;
                    var index2 = 0;
                    var ix = 0;
                    foreach (DataRow item in mun.Rows)
                    {
                        if (item[0].ToString() == row[3].ToString())
                            index = ix;
                        mpio.Items.Add(new ListItem(item[2].ToString(), item[0].ToString()));
                        ix++;
                    }
                    mpio.SelectedIndex = index + 1;

                        //mpio.InnerHtml = lstmun;
                    ix = 0;
                    foreach (DataRow item in of.Rows)
                    {
                        if (int.Parse(item[0].ToString()) == int.Parse(row[5].ToString()))
                            index2 = ix;
                        ofi.Items.Add(new ListItem(item[2].ToString(), item[0].ToString()));
                        ix++;
                    }

                    ofi.SelectedIndex = index2 + 1;
                    Field1Combo.SelectedValue = row[1].ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "setData", "setMpioOfiData('" + row[1].ToString() + "','" + row[3].ToString() + "','" + row[5].ToString() + "');", true);
                    hdMuniSelect.Value = $"{row[3].ToString()}|{row[4].ToString()}";
                    hdOfiSelected.Value = $"{row[5].ToString()}|{row[6].ToString()}";
                }
            }
        }

        protected void id_dptoCombo_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }
    }
}