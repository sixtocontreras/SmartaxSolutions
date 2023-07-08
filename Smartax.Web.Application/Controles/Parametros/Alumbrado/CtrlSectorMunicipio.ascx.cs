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
    public partial class CtrlSectorMunicipio : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        ConfAlumbrado ObjConfAlumbrado = new ConfAlumbrado();
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
                ObjetoDataTable = ObjConfAlumbrado.GetAll();
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
                ObjetoDataSet.Tables.Add(ObjConfAlumbrado.GetSectores());

                hdMuni.Value = Newtonsoft.Json.JsonConvert.SerializeObject(ObjConfAlumbrado.GetMunicipios());
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
                this.Page.Title = this.Page.Title + "Parametrización Sector";
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
                RadGrid1.DataMember = "DtSectorMunicipio";

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
                columna = (GridDropDownColumn)this.RadGrid1.Columns[10];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Sector";
                columna.DataField = "id_sector";
                columna.ListTextField = "nombre";
                columna.ListValueField = "id";
                columna.ListDataMember = "DtSectores";

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
                else if (e.CommandName == "BtnConfiguracion")
                {
                    #region VISUALIZAR FORMULARIO CONFIGURACION DE CUENTAS
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdColumna = Convert.ToInt32(item.GetDataKeyValue("id").ToString().Trim());
                    string idmuni = item.Cells[7].Text.Trim();
                    string _NombreMuni = item.Cells[8].Text.Trim(); 
                    string _vigencia = item.Cells[10].Text.Trim(); 
                    string _sector = item.Cells[11].Text.Trim(); 


                    //--Mandamos abrir el formulario de registro
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    Ventana.NavigateUrl = $"/Controles/Administracion/Alumbrado/FrmConfiguracionTarifas.aspx?IdColumna={_IdColumna}&IdMuni={idmuni}&Muni={_NombreMuni}&Vigencia={_vigencia}&IdSector={_sector}&PathUrl=" + _PathUrl;
                    //Ventana.NavigateUrl = "/Controles/Administracion/Clientes/FrmConfImpuestoIca.aspx?IdClienteImpuesto=" + _IdClienteImpuesto + "&IdFormImpuesto=" + _IdFormImpuesto + "&NombreImpuesto=" + _NombreImpuesto + "&IdCliente=" + _IdCliente + "&PathUrl=" + _PathUrl;
                    Ventana.ID = "RadWindow12";
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(720);
                    Ventana.Width = Unit.Pixel(1200);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = $"Alumbrado público: {idmuni} – {_NombreMuni} Vigencia: {_vigencia}";
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
                    RequiredFieldValidator validator2 = new RequiredFieldValidator();
                    validator2.ControlToValidate = editor.NumericTextBox.ID;
                    validator2.ErrorMessage = "Campo Requerido";
                    validator2.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator2);
                    editor.Visible = true;





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
                _MsgError = "El campo departamento y municipio son obligatorios";
                error = true;
            }
            else if(hdMuniSelect.Value =="") {

                _MsgError = "El campo municipio es obligatorio";
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
            string CmbSector = (editedItem["id_sector"].Controls[0] as RadComboBox).SelectedItem.Text;
            int idCmbSector = int.Parse((editedItem["id_sector"].Controls[0] as RadComboBox).SelectedItem.Value);
            string CmbDpto = (editedItem["id_dpto"].Controls[1] as RadComboBox).SelectedItem.Text;
            int idCmbDpto = int.Parse((editedItem["id_dpto"].Controls[1] as RadComboBox).SelectedItem.Value);
            string CmbMuni = hdMuniSelect.Value.Split('|')[1];
            int idCmbMuni = int.Parse(hdMuniSelect.Value.Split('|')[0]);
                      
            DataTable TablaDatos = this.FuenteDatos.Tables["DtSectorMunicipio"];


            var ct = TablaDatos.Select($"id_cliente = {idCmbCliente} AND id_municipio = {idCmbMuni} AND vigencia = {(editedItem["vigencia"].Controls[0] as RadNumericTextBox).Text}").Count();
            if (ct > 0)
            {
                _MsgError = $"Ya existe registro para el municipio – vigencia.";
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
                    _log.Warn("REQUEST INSERT CONF_SECTOR_MUNICIPIO => " + jsonRequest);

                    string datafila = $"{idCmbSector}|{idCmbMuni}|{NuevaFila["Vigencia"]}|{NuevaFila["id_cliente"]}|{idCmbDpto}";
                    int _IdRegistro = 0;
                    if (ObjConfAlumbrado.AddConfEntidad(datafila, Int32.Parse(Session["IdUsuario"].ToString().Trim()), ref _IdRegistro, ref _MsgError))
                    {
                        var munn = ObjConfAlumbrado.GetMunicipios(idCmbDpto);
                        var muniSelect = munn.Select("id_municipio = " + idCmbMuni);
                        NuevaFila["Vigencia"] = NuevaFila["Vigencia"].ToString();

                        NuevaFila["nombre_sector"] = CmbSector.ToString().Trim().ToUpper();
                        NuevaFila["id_sector"] = idCmbSector;
                        NuevaFila["nombre_dpto"] = CmbDpto.ToString().Trim().ToUpper();
                        NuevaFila["id_dpto"] = idCmbDpto;
                        NuevaFila["nombre_municipio"] = CmbMuni.ToString().Trim().ToUpper();
                        NuevaFila["id_municipio"] = idCmbMuni;
                        NuevaFila["nombre_cliente"] = CmbCliente.ToString().Trim().ToUpper();
                        NuevaFila["id_cliente"] = idCmbCliente;
                        NuevaFila["codigo_dane"] = muniSelect[0]["codigo_dane"].ToString();
                        NuevaFila["fecha_registro"] = DateTime.Now;
                        NuevaFila["id"] = _IdRegistro;
                        this.FuenteDatos.Tables["DtSectorMunicipio"].Rows.Add(NuevaFila);

                        _MsgError = "Registro creado con exito.";
                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                        ObjAuditoria.ModuloApp = "CONF_SECTOR_MUNICIPIO";
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
            TablaDatos = this.FuenteDatos.Tables["DtSectorMunicipio"];
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

                string CmbCliente = (editedItem["id_cliente"].Controls[0] as RadComboBox).SelectedItem.Text;
                int idCmbCliente = int.Parse((editedItem["id_cliente"].Controls[0] as RadComboBox).SelectedItem.Value);
                string CmbSector = (editedItem["id_sector"].Controls[0] as RadComboBox).SelectedItem.Text;
                int idCmbSector = int.Parse((editedItem["id_sector"].Controls[0] as RadComboBox).SelectedItem.Value);
                string CmbDpto = (editedItem["id_dpto"].Controls[0] as RadComboBox).SelectedItem.Text;
                int idCmbDpto = int.Parse((editedItem["id_dpto"].Controls[0] as RadComboBox).SelectedItem.Value);
                string CmbMuni = (editedItem["id_municipio"].Controls[0] as RadComboBox).SelectedItem.Text;
                int idCmbMuni = int.Parse((editedItem["id_municipio"].Controls[0] as RadComboBox).SelectedItem.Value);

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjConfAlumbrado);
                _log.Warn("REQUEST UPDATE CONF_SECTOR_MUNICIPIO => " + jsonRequest);

                string _MsgError = "";
                if (ObjConfAlumbrado.UpConfEntidad(changedRows[0], Int32.Parse(Session["IdUsuario"].ToString().Trim()), ref _IdRegistro, ref _MsgError))
                {
                    var munn = ObjConfAlumbrado.GetMunicipios(idCmbDpto);
                    var muniSelect = munn.Select("id_municipio = " + idCmbMuni);
                    changedRows[0]["Vigencia"] = changedRows[0]["Vigencia"].ToString();
                    changedRows[0]["nombre_sector"] = CmbSector.ToString().Trim().ToUpper();
                    changedRows[0]["id_sector"] = idCmbSector;
                    changedRows[0]["nombre_dpto"] = CmbDpto.ToString().Trim().ToUpper();
                    changedRows[0]["id_dpto"] = idCmbDpto;
                    changedRows[0]["nombre_municipio"] = CmbMuni.ToString().Trim().ToUpper();
                    changedRows[0]["id_municipio"] = idCmbMuni;
                    changedRows[0]["nombre_cliente"] = CmbCliente.ToString().Trim().ToUpper();
                    changedRows[0]["codigo_dane"] = muniSelect[0]["codigo_dane"].ToString();
                    changedRows[0]["id_cliente"] = idCmbCliente;
                 
                    this.FuenteDatos.Tables["DtSectorMunicipio"].Rows[0].AcceptChanges();
                    this.FuenteDatos.Tables["DtSectorMunicipio"].Rows[0].EndEdit();

                    _MsgError = "Registro actualizado con exito.";
                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                    ObjAuditoria.ModuloApp = "CONF_SECTOR_MUNICIPIO";
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
            TablaDatos = this.FuenteDatos.Tables["DtSectorMunicipio"];
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
                _log.Warn("REQUEST DELETE CONF_SECTOR_MUNICIPIO => " + jsonRequest);

                if (ObjConfAlumbrado.dltConfEntidad(Int32.Parse(Session["IdUsuario"].ToString().Trim()), ref _IdRegistro, ref _MsgError))
                {
                    this.FuenteDatos.Tables["DtSectorMunicipio"].Rows.Find(_IdRegistro).Delete();
                    this.FuenteDatos.Tables["DtSectorMunicipio"].AcceptChanges();

                    _MsgError = "Registro eliminado con exito.";
                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 4;  //--DELETE
                    ObjAuditoria.ModuloApp = "CONF_SECTOR_MUNICIPIO";
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
                var dpto = ObjConfAlumbrado.GetDptos();
                foreach (DataRow item in dpto.Rows)
                {
                    Field1Combo.Items.Add(new RadComboBoxItem(item[1].ToString().Trim(), item[0].ToString().Trim()));
                }
                //RadComboBox Field2Combo = e.Item.FindControl("id_municipioCombo") as RadComboBox;
                //var muni = ObjConfAlumbrado.GetMunicipios(int.Parse(dpto.Rows[0][0].ToString()));
                //Field2Combo.Items.Clear();
                //foreach (DataRow item in muni.Rows)
                //{
                //    Field2Combo.Items.Add(new RadComboBoxItem(item[2].ToString().Trim(), item[0].ToString().Trim()));
                //}
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "change=true;", true);
            }
        }

        protected void id_dptoCombo_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            
        }
    }
}