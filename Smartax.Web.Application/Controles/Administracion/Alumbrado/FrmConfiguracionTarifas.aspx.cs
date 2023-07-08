using log4net;
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

namespace Smartax.Web.Application.Controles.Administracion.Alumbrado
{
    public partial class FrmConfiguracionTarifas : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        RadWindow Ventana = new RadWindow();

        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        ConfAlumbrado ObjConfInf = new ConfAlumbrado();


        public DataSet GetDatosGrilla(int id)
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                //Mostrar listado cuentas configuradas
                ObjetoDataTable = ObjConfInf.GetTarifasAll(id);
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                ObjetoDataSet.Tables.Add(ObjConfInf.GetTiposSectorEspecial());

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
                string _MsgError = "Error al listar las cuentas. Motivo: " + ex.ToString();
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
                    lblIdColumna.Text = ViewState["IdCol"].ToString();
                    var col = int.Parse(ViewState["IdCol"].ToString());
                    ConjuntoDatos = GetDatosGrilla(col);
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
                this.RadGrid12.Visible = false;
            }
            if (!objPermiso.PuedeRegistrar)
            {
                this.RadGrid12.MasterTableView.CommandItemDisplay = 0;
            }
            if (!objPermiso.PuedeModificar)
            {
                this.RadGrid12.Columns[RadGrid12.Columns.Count - 2].Visible = false;
            }
            if (!objPermiso.PuedeEliminar)
            {
                this.RadGrid12.Columns[RadGrid12.Columns.Count - 1].Visible = false;
            }

            //Ocultar la columna de empresa siempre y cuando el Rol sea diferente al de Soporte
            if (Int32.Parse(Session["IdRol"].ToString().Trim()) != 1)
            {
                this.RadGrid12.Columns[RadGrid12.Columns.Count - 6].Visible = false;
                this.RadGrid12.Columns[RadGrid12.Columns.Count - 7].Visible = false;
            }

            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid12);
                this.ViewState["IdCol"] = Request.QueryString["IdColumna"].ToString().Trim();
                
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid12);
            }
        }

        #region DEFINICION DE METODOS DEL GRID
        protected void RadGrid12_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid12.DataSource = this.FuenteDatos;
                RadGrid12.DataMember = "DtTarifas";
                var sector = Request.QueryString["IdSector"].ToString().ToLower().Trim();
                if (sector == "especial")
                {
                    RadGrid12.Columns[2].Visible = false;
                    RadGrid12.Columns[3].Visible = false;
                    RadGrid12.Columns[4].Visible = false;
                    RadGrid12.Columns[5].Visible = false;
                    RadGrid12.Columns[6].Visible = false;
                    RadGrid12.Columns[8].Visible = true;
                    ((GridNumericColumn)RadGrid12.MasterTableView.GetColumnSafe("min_kw")).ReadOnly = true;
                    ((GridNumericColumn)RadGrid12.MasterTableView.GetColumnSafe("max_kw")).ReadOnly = true;
                    ((GridNumericColumn)RadGrid12.MasterTableView.GetColumnSafe("consumo")).ReadOnly = true;
                    ((GridNumericColumn)RadGrid12.MasterTableView.GetColumnSafe("tarifa_minima")).ReadOnly = true;
                    RadGrid12.Columns[7].HeaderText = "Tarifa";
                }
                else
                {
                    RadGrid12.Columns[8].Visible = false;
                    ((GridDropDownColumn)RadGrid12.MasterTableView.GetColumnSafe("tipo")).ReadOnly = true;
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
                string _MsgError = "Error con el evento RadGrid12_NeedDataSource del tipo de comision. Motivo: " + ex.ToString();
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



        protected void RadGrid12_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            try
            {
                RadGrid12.Rebind();
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
                string _MsgError = "Error con el evento RadGrid12_PageIndexChanged del tipo de comision. Motivo: " + ex.ToString();
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
        protected void RadGrid12_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            var _MsgError = string.Empty;
            DataTable TablaDatos = this.FuenteDatos.Tables["DtTarifas"];
            DataRow NuevaFila = TablaDatos.NewRow();
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id"] };
            DataRow[] TodosValores = TablaDatos.Select("", "id", DataViewRowState.CurrentRows);
            var sector = Request.QueryString["IdSector"].ToString().ToLower().Trim();
            if (TablaDatos.Rows.Count > 0 && sector == "especial")
            {
                _MsgError = "No puede ingresar mas de 1 registro para el sector al que pertenece esta parametrizacion.";
                //RadWindowManager1.RadAlert(_MsgError, 400, 200, "Error", "reloadPage", "../../Imagenes/Iconos/16/delete.png");
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
            else
            {

                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                try
                {
                    foreach (DictionaryEntry entry in newValues)
                    {
                        NuevaFila[(string)entry.Key] = entry.Value;
                    }

                    var error2 = false;
                    if (sector != "especial")
                    {
                        try {
                            int.Parse(NuevaFila["min_kw"].ToString());
                        }
                        catch (Exception) {
                            error2 = true;
                            _MsgError = "El consumo inicial debe ser un numero entero";
                        }
                        try
                        {
                            int.Parse(NuevaFila["max_kw"].ToString());
                        }
                        catch (Exception)
                        {
                            error2 = true;
                            _MsgError = "El consumo final debe ser un numero entero";
                        }
                        


                    }
                    if (error2)
                    {

                        //RadWindowManager1.RadAlert(_MsgError, 400, 200, "Error", "reloadPage", "../../Imagenes/Iconos/16/delete.png");
                       
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
                        _log.Error(_MsgError);
                        Ventana = null;
                        #endregion
                    }
                    else
                    {
                        if ((NuevaFila != null))
                        {


                            //--AQUI SERIALIZAMOS EL OBJETO CLASE
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            string jsonRequest = js.Serialize(ObjConfInf);
                            _log.Warn("REQUEST INSERT CONF_PARAM_TARIFAS => " + jsonRequest);

                            int _IdRegistro = 0;
                            var rtaProcess = false;
                            if (sector == "especial")
                                rtaProcess = ObjConfInf.AddTarifaEspecial(NuevaFila, int.Parse(lblIdColumna.Text), Int32.Parse(Session["IdUsuario"].ToString().Trim()), ref _IdRegistro, ref _MsgError);
                            else
                                rtaProcess = ObjConfInf.AddTarifa(NuevaFila, int.Parse(lblIdColumna.Text), Int32.Parse(Session["IdUsuario"].ToString().Trim()), ref _IdRegistro, ref _MsgError);

                            if (rtaProcess)
                            {

                                NuevaFila["consumokwh"] = $"de {NuevaFila["min_kw"].ToString()} hasta {NuevaFila["max_kw"].ToString()}";
                                NuevaFila["id"] = _IdRegistro;
                                this.FuenteDatos.Tables["DtTarifas"].Rows.Add(NuevaFila);

                                #region REGISTRO DE LOGS DE AUDITORIA
                                //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                                ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                                ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                                ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                                ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                                ObjAuditoria.ModuloApp = "CONF_PARAM_TARIFAS";
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

                                _MsgError = "Registro creado con exito.";
                                //RadScriptManager.RegisterStartupScript(this, this.GetType(), "ok", "alertMsj('" + _MsgError + "');", true);
                                //RadWindowManager1.RadAlert(_MsgError, 400, 200, "Ok", "reloadPage", "../../Imagenes/Iconos/16/check.png");
                                //_log.Info(_MsgError);
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
                                //e.Canceled = true;
                                #endregion
                            }
                            else
                            {
                                //RadWindowManager1.RadAlert(_MsgError, 400, 200, "Error", "reloadPage", "../../Imagenes/Iconos/16/delete.png");
                                //_log.Error(_MsgError);
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
                }
                catch (Exception ex)
                {
                    _MsgError = "Error al registrar el tipo de comision. Motivo: " + ex.ToString();
                    //RadWindowManager1.RadAlert(_MsgError, 400, 200, "Error", "reloadPage", "../../Imagenes/Iconos/16/delete.png");
                    //_log.Error(_MsgError);
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

                    string _ModuloApp = "CONF_PARAM_TARIFAS";
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
        protected void RadGrid12_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatos.Tables["DtTarifas"];
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
                string jsonRequest = js.Serialize(ObjConfInf);
                _log.Warn("REQUEST DELETE CONF_PARAM_TARIFAS => " + jsonRequest);

                if (ObjConfInf.dltTarifa(Int32.Parse(Session["IdUsuario"].ToString().Trim()), ref _IdRegistro, ref _MsgError))
                {
                    this.FuenteDatos.Tables["DtTarifas"].Rows.Find(_IdRegistro).Delete();
                    this.FuenteDatos.Tables["DtTarifas"].AcceptChanges();

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.IdTipoEvento = 4;  //--DELETE
                    ObjAuditoria.ModuloApp = "CONF_PARAM_321_525";
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

                    _MsgError = "Registro eliminado con exito.";
                    //RadWindowManager1.RadAlert(_MsgError, 400, 200, "Ok", "", "../../Imagenes/Iconos/16/check.png");
                    //_log.Info(_MsgError);
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
                    //RadWindowManager1.RadAlert(_MsgError, 400, 200, "Error", "alertMsj('" + _MsgError + "')", "../../Imagenes/Iconos/16/delete.png");
                    //_log.Error(_MsgError);
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

        protected void RadGrid12_ItemCreated(object sender, GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                try
                {
                    GridEditableItem item = (GridEditableItem)e.Item;

                    GridTextBoxColumnEditor editor = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("clasificacion");
                    TableCell cell = (TableCell)editor.TextBoxControl.Parent;
                    RequiredFieldValidator validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor.Visible = true;

                    GridNumericColumnEditor editor1 = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("tarifa_maxima");
                    TableCell cell1 = (TableCell)editor1.NumericTextBox.Parent;
                    RequiredFieldValidator validator1 = new RequiredFieldValidator();
                    validator1.ControlToValidate = editor1.NumericTextBox.ID;
                    validator1.ErrorMessage = "Campo Requerido";
                    validator1.Display = ValidatorDisplay.Dynamic;
                    cell1.Controls.Add(validator1);
                    editor1.Visible = true;


                    var sector = Request.QueryString["IdSector"].ToString().ToLower().Trim();
                    if (sector != "especial")
                    {
                        GridNumericColumnEditor editor2 = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("min_kw");
                        TableCell cell2 = (TableCell)editor2.NumericTextBox.Parent;
                        RequiredFieldValidator validator2 = new RequiredFieldValidator();
                        validator2.ControlToValidate = editor2.NumericTextBox.ID;
                        validator2.ErrorMessage = "Campo Requerido";
                        validator2.Display = ValidatorDisplay.Dynamic;
                        cell2.Controls.Add(validator2);
                        editor2.Visible = true;

                        GridNumericColumnEditor editor3 = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("max_kw");
                        TableCell cell3 = (TableCell)editor3.NumericTextBox.Parent;
                        RequiredFieldValidator validator3 = new RequiredFieldValidator();
                        validator3.ControlToValidate = editor3.NumericTextBox.ID;
                        validator3.ErrorMessage = "Campo Requerido";
                        validator3.Display = ValidatorDisplay.Dynamic;
                        cell3.Controls.Add(validator3);
                        editor3.Visible = true;

                        GridNumericColumnEditor editor4 = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("tarifa_minima");
                        TableCell cell4 = (TableCell)editor4.NumericTextBox.Parent;
                        RequiredFieldValidator validator4 = new RequiredFieldValidator();
                        validator4.ControlToValidate = editor4.NumericTextBox.ID;
                        validator4.ErrorMessage = "Campo Requerido";
                        validator4.Display = ValidatorDisplay.Dynamic;
                        cell4.Controls.Add(validator4);
                        editor4.Visible = true;

                        GridNumericColumnEditor editor5 = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("consumo");
                        TableCell cell5 = (TableCell)editor5.NumericTextBox.Parent;
                        RequiredFieldValidator validator5 = new RequiredFieldValidator();
                        validator5.ControlToValidate = editor5.NumericTextBox.ID;
                        validator5.ErrorMessage = "Campo Requerido";
                        validator5.Display = ValidatorDisplay.Dynamic;
                        cell5.Controls.Add(validator5);
                        RangeValidator validator51 = new RangeValidator();
                        validator51.ControlToValidate = editor5.NumericTextBox.ID;
                        validator51.MinimumValue = "0";
                        validator51.MinimumValue = "100";
                        validator51.ErrorMessage = "El valor debe ser menor o igual a 100";
                        validator51.Display = ValidatorDisplay.Dynamic;
                        cell5.Controls.Add(validator5);
                        editor5.Visible = true;

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
    }
}