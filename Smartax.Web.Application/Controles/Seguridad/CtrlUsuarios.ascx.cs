using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Text;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Seguridad
{
    public partial class CtrlUsuarios : System.Web.UI.UserControl
    {
        #region DEFINICION DE OBJETOS DE CLASES
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        Usuario ObjUser = new Usuario();
        MedioEnvioToken ObjMedio = new MedioEnvioToken();
        Empresas ObjEmpresa = new Empresas();
        SistemaRol ObjRol = new SistemaRol();
        Estado ObjEstado = new Estado();
        Utilidades ObjUtils = new Utilidades();
        EnvioCorreo ObjCorreo = new EnvioCorreo();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        #endregion  

        public DataSet GetObtenerDatos()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                //Mostrar usuarios
                ObjUser.TipoConsulta = 1;
                ObjUser.IdUsuario = null;
                ObjUser.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjUser.IdEstado = null;
                ObjUser.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
                ObjUser.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                ObjUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                ObjetoDataTable = ObjUser.GetAllUsuarios();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_usuario"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar roles del sistema.
                ObjetoDataTable = new DataTable();
                ObjRol.TipoConsulta = 2;
                ObjRol.IdEstado = 1;
                ObjRol.MostrarSeleccione = "NO";
                ObjRol.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
                ObjRol.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjRol.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjRol.GetRoles();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar medios de envio token.
                ObjetoDataTable = new DataTable();
                ObjMedio.IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString().Trim());
                ObjMedio.IdRol = Int32.Parse(Session["IdRol"].ToString().Trim());
                ObjMedio.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjMedio.MostrarSeleccione = "NO";
                ObjetoDataTable = ObjMedio.GetMedioEnvio();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar datos de las empresas registradas y activas
                ObjetoDataTable = new DataTable();
                ObjEmpresa.TipoConsulta = 2;
                ObjEmpresa.IdEstado = 1;
                ObjEmpresa.MostrarSeleccione = "NO";
                ObjEmpresa.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
                ObjEmpresa.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                ObjEmpresa.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjetoDataTable = ObjEmpresa.GetEmpresas();
                ObjetoDataSet.Tables.Add(ObjetoDataTable);

                //Mostrar los Estados
                ObjetoDataTable = new DataTable();
                ObjEstado.TipoConsulta = 2;
                ObjEstado.IdEstado = 1;
                ObjEstado.TipoEstado = "INTERFAZ_USER";  //INTERFAZ: MUESTRA LOS ESTADOS INACTIVO Y ACTIVOS, PROCESOS: MUESTRA EL RESTO DE LOS ESTADOS MENOS LOS ANTERIORES
                ObjEstado.MostrarSeleccione = "NO";
                ObjEstado.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                ObjEstado.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al cargar los Datos de usuarios. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(270);
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

        private DataSet FuenteDatosUser
        {
            get
            {
                object obj = this.ViewState["_FuenteDatosUser"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetObtenerDatos();
                    this.ViewState["_FuenteDatosUser"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatosUser"];
                }
            }
            set { this.ViewState["_FuenteDatosUser"] = value; }
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
                //this.RadGrid1.Columns[RadGrid1.Columns.Count - 2].Visible = false;
            }
            if (!objPermiso.PuedeModificar)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 2].Visible = false;
            }
            //if (!objPermiso.PuedeEliminar)
            //{
            //    this.RadGrid1.Columns[RadGrid1.Columns.Count - 1].Visible = false;
            //}
            
            int _IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
            if (_IdRol != 1)
            {
                this.RadGrid1.Columns[RadGrid1.Columns.Count - 6].Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.Page.Title = this.Page.Title + "Usuarios Internos";
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
                RadGrid1.DataSource = this.FuenteDatosUser;
                RadGrid1.DataMember = "DtUsuarios";

                GridDropDownColumn columna = new GridDropDownColumn();
                //Medio de envio del token.
                columna = (GridDropDownColumn)this.RadGrid1.Columns[13];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Envio";
                columna.DataField = "idmedio_envio_token";
                columna.ListTextField = "medio_envio";
                columna.ListValueField = "idmedio_envio_token";
                columna.ListDataMember = "DtMedioEnvio";

                //Roles y Perfiles
                columna = (GridDropDownColumn)this.RadGrid1.Columns[15];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Perfil";
                columna.DataField = "id_rol";
                columna.ListTextField = "nombre_rol";
                columna.ListValueField = "id_rol";
                columna.ListDataMember = "DtRoles";

                //Empresas
                columna = (GridDropDownColumn)this.RadGrid1.Columns[17];
                columna.DataSourceID = this.RadGrid1.DataSourceID;
                columna.HeaderText = "Empresa";
                columna.DataField = "id_empresa";
                columna.ListTextField = "nombre_empresa";
                columna.ListValueField = "id_empresa";
                columna.ListDataMember = "DtEmpresas";

                //Estados
                columna = (GridDropDownColumn)this.RadGrid1.Columns[19];
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con el metodo NeedDataSource. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
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

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnVerInfo")
                {
                    #region VER INFORMACION DETALLADA USUARIO
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdUsuario = Int32.Parse(item.GetDataKeyValue("id_usuario").ToString().Trim());
                    string strNombreUsuario = item["nombre_usuario"].Text.ToString().Trim() + " " + item["apellido_usuario"].Text.ToString().Trim();

                    RadWindow VentanaWin = new RadWindow();
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    VentanaWin.Modal = true;

                    string PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                    VentanaWin.NavigateUrl = "/Controles/Seguridad/FrmVerInfoUsuario.aspx?IdUsuario=" + _IdUsuario;
                    VentanaWin.ID = "RadWindow1";
                    VentanaWin.VisibleOnPageLoad = true;
                    VentanaWin.Visible = true;
                    VentanaWin.Height = Unit.Pixel(460);
                    VentanaWin.Width = Unit.Pixel(800);
                    VentanaWin.KeepInScreenBounds = true;
                    VentanaWin.Title = "Nombre del Usuario: " + strNombreUsuario;
                    VentanaWin.VisibleStatusbar = false;
                    VentanaWin.Behaviors = WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Resize;
                    this.RadWindowManager1.Windows.Add(VentanaWin);
                    this.RadWindowManager1 = null;
                    VentanaWin = null;
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con el metodo ItemCommand. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
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

        protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                try
                {
                    GridEditableItem item = (GridEditableItem)e.Item;
                    GridNumericColumnEditor editor = (GridNumericColumnEditor)item.EditManager.GetColumnEditor("identificacion_usuario");
                    TableCell cell1 = (TableCell)editor.NumericTextBox.Parent;
                    RequiredFieldValidator validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor.NumericTextBox.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell1.Controls.Add(validator);
                    editor.Visible = true;

                    //----
                    GridTextBoxColumnEditor editor2 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("nombre_usuario");
                    TableCell cell = (TableCell)editor2.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor2.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);

                    //----
                    GridTextBoxColumnEditor editor3 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("apellido_usuario");
                    cell = (TableCell)editor3.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor3.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);

                    //----
                    GridTextBoxColumnEditor editor4 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("direccion_usuario");
                    cell = (TableCell)editor4.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor4.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);

                    //----
                    GridTextBoxColumnEditor editor5 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("telefono_usuario");
                    cell = (TableCell)editor5.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor5.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);
                    editor5.Visible = true;

                    //----
                    GridTextBoxColumnEditor editor6 = (GridTextBoxColumnEditor)item.EditManager.GetColumnEditor("email_usuario");
                    cell = (TableCell)editor6.TextBoxControl.Parent;
                    validator = new RequiredFieldValidator();
                    validator.ControlToValidate = editor6.TextBoxControl.ID;
                    validator.ErrorMessage = "Campo Requerido";
                    validator.Display = ValidatorDisplay.Dynamic;
                    cell.Controls.Add(validator);

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
                    string _MsgMensaje = "Señor usuario. Ocurrio un Error con el metodo ItemCreated. Motivo: " + ex.ToString();
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
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
        }

        protected void RadGrid1_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            try
            {
                this.RadGrid1.Rebind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con el metodo PageIndexChanged. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
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
        #endregion

        #region DEFINICION DEL CRUD DE USUARIOS
        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = this.FuenteDatosUser.Tables["DtUsuarios"]; ;
            DataRow NuevaFila = TablaDatos.NewRow();
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_usuario"] };
            DataRow[] TodosValores = TablaDatos.Select("", "id_usuario", DataViewRowState.CurrentRows); ;

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
                    string CmbMedioEnvio = (editedItem["idmedio_envio_token"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string _IdPerfil = (editedItem["id_rol"].Controls[0] as RadComboBox).SelectedValue.ToString().Trim();
                    string CmbPerfil = (editedItem["id_rol"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;
                    string CmbEmpresa = (editedItem["id_empresa"].Controls[0] as RadComboBox).SelectedItem.Text;
                    //--
                    string _ClaveDinamica = ObjUtils.GetRandom();
                    ObjUser.IdUsuario = null;
                    ObjUser.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                    string _IdentificacionUser = NuevaFila["identificacion_usuario"].ToString().Trim().ToUpper();
                    string _NombreUser = NuevaFila["nombre_usuario"].ToString().Trim().ToUpper();
                    string _ApellidoUser = NuevaFila["apellido_usuario"].ToString().Trim().ToUpper();
                    string _LoginUser = NuevaFila["identificacion_usuario"].ToString().Trim().ToUpper();
                    string _DireccionUser = NuevaFila["direccion_usuario"].ToString().Trim().ToUpper();
                    string _TelefonoUser = NuevaFila["telefono_usuario"].ToString().Trim().ToUpper();
                    string _EmailUser = NuevaFila["email_usuario"].ToString().Trim().ToUpper();
                    //--
                    ObjUser.IdentificacionUsuario = _IdentificacionUser;
                    ObjUser.NombreUsuario = _NombreUser;
                    ObjUser.ApellidoUsuario = _ApellidoUser;
                    ObjUser.LoginUsuario = _LoginUser;
                    ObjUser.DireccionUsuario = _DireccionUser;
                    ObjUser.TelefonoUsuario = _TelefonoUser;
                    ObjUser.EmailUsuario = _EmailUser;
                    ObjUser.IdEstado = Int32.Parse(NuevaFila["id_estado"].ToString().Trim());
                    ObjUser.IdEmpresa = Int32.Parse(NuevaFila["id_empresa"].ToString().Trim());
                    ObjUser.CambiarClave = "S";
                    ObjUser.ManejarFueraOficina = Boolean.Parse(NuevaFila["manejar_fuera_oficina"].ToString().Trim()) == true ? "S" : "N";
                    ObjUser.IpEquipoOficina = NuevaFila["ip_equipo_oficina"].ToString().Trim().ToUpper();
                    //ObjUser.ManejarFueraOficina = "N";
                    ObjUser.PasswordUsuario = _ClaveDinamica.ToString().Trim();
                    ObjUser.FechaExpClave = DateTime.Now.AddDays(Int32.Parse(this.Session["DiasExpClave"].ToString().Trim())).ToString("yyyy-MM-dd");
                    ObjUser.IdUsuarioAdd = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjUser.IdUsuarioUp = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjUser.IdEmpresaPadre = Int32.Parse(this.Session["IdEmpresaAdmon"].ToString().Trim());
                    ObjUser.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
                    ObjUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjUser.TipoProceso = 1;

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjUser);

                    int _IdUsuario = 0;
                    string _MsgMensaje = "";
                    if (ObjUser.AddUpUsuario(NuevaFila, ref _IdUsuario, ref _MsgMensaje))
                    {
                        //--Aqui obtenemos los datos del usuario.
                        NuevaFila["id_usuario"] = _IdUsuario;
                        NuevaFila["nombre_completo_usuario"] = _NombreUser + " " + _ApellidoUser;
                        NuevaFila["nombre_usuario"] = _NombreUser;
                        NuevaFila["apellido_usuario"] = _ApellidoUser;
                        NuevaFila["login_usuario"] = _LoginUser;
                        NuevaFila["direccion_usuario"] = _DireccionUser;
                        NuevaFila["telefono_usuario"] = _TelefonoUser;
                        NuevaFila["email_usuario"] = _EmailUser;
                        NuevaFila["cambiar_clave"] = false;
                        NuevaFila["manejar_fuera_oficina"] = false;
                        NuevaFila["medio_envio"] = CmbMedioEnvio.ToString().Trim().ToUpper();
                        NuevaFila["nombre_rol"] = CmbPerfil.ToString().Trim().ToUpper();
                        NuevaFila["nombre_empresa"] = CmbEmpresa.ToString().Trim().ToUpper();
                        NuevaFila["codigo_estado"] = CmbEstado.ToString().Trim().ToUpper();
                        this.FuenteDatosUser.Tables["DtUsuarios"].Rows.Add(NuevaFila);

                        #region DEFINICION DEL METODO PARA ENVIO DE CORREO
                        //--Definir valores para envio del email
                        ObjCorreo.StrServerCorreo = Session["ServerCorreo"].ToString().Trim();
                        ObjCorreo.PuertoCorreo = Int32.Parse(Session["PuertoCorreo"].ToString().Trim());
                        ObjCorreo.StrEmailDe = Session["EmailSoporte"].ToString().Trim();
                        ObjCorreo.StrPasswordDe = Session["PasswordEmail"].ToString().Trim();
                        ObjCorreo.StrEmailPara = NuevaFila["email_usuario"].ToString().Trim();
                        ObjCorreo.StrAsunto = "REF.: REGISTRO DE USUARIO";

                        string nHora = DateTime.Now.ToString("HH");
                        string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                        StringBuilder strDetalleEmail = new StringBuilder();
                        strDetalleEmail.Append("<h4>" + strTime + ", Para informarle que su usuario fue registrado exitosamente en la Plataforma y poder ingresar al [" + FixedData.PlatformName + "].</h4>" + "<br/>" +
                                        "<h4>DATOS DEL USUARIO REGISTRADO</h2>" + "<br/>" +
                                        "Nombre: " + _NombreUser + " " + _ApellidoUser + "<br/>" +
                                        "Usuario: " + _LoginUser + "<br/>" +
                                        "Password: " + _ClaveDinamica.ToString().Trim() + "<br/>" +

                                        "Dirección: " + _DireccionUser + "<br/>" +
                                        "Celular: " + _TelefonoUser + "<br/>" +
                                        "Email: " + _EmailUser + "<br/>" +
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
                            _MsgMensaje = _MsgMensaje + " Pero " + _MsgErrorEmail.ToString().Trim();
                        }
                        #endregion

                        #region REGISTRO DE LOGS DE AUDITORIA
                        //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                        ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                        ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                        ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                        ObjAuditoria.ModuloApp = "USUARIOS";
                        //--TIPOS DE EVENTO: 1. LOGIN, 2. INSERT, 3. UPDATE, 4. DELETE, 5. CONSULTA
                        ObjAuditoria.IdTipoEvento = 2;
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
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
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
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al registrar el usuario del Comercio. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
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
                e.Canceled = true;
                #endregion
            }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatosUser.Tables["DtUsuarios"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_usuario"] };
            DataRow[] changedRows = TablaDatos.Select("id_usuario = " + editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id_usuario"].ToString());

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

                string CmbMedioEnvio = (editedItem["idmedio_envio_token"].Controls[0] as RadComboBox).SelectedItem.Text;
                string _IdPerfil = (editedItem["id_rol"].Controls[0] as RadComboBox).SelectedValue.ToString().Trim();
                string CmbPerfil = (editedItem["id_rol"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbEstado = (editedItem["id_estado"].Controls[0] as RadComboBox).SelectedItem.Text;
                string CmbEmpresa = (editedItem["id_empresa"].Controls[0] as RadComboBox).SelectedItem.Text;

                ObjUser.IdUsuario = Int32.Parse(changedRows[0]["id_usuario"].ToString().Trim());
                ObjUser.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjUser.IdentificacionUsuario = changedRows[0]["identificacion_usuario"].ToString().Trim().ToUpper();
                ObjUser.NombreUsuario = changedRows[0]["nombre_usuario"].ToString().Trim().ToUpper();
                ObjUser.ApellidoUsuario = changedRows[0]["apellido_usuario"].ToString().Trim().ToUpper();
                ObjUser.LoginUsuario = changedRows[0]["identificacion_usuario"].ToString().Trim().ToUpper();
                ObjUser.DireccionUsuario = changedRows[0]["direccion_usuario"].ToString().Trim().ToUpper();
                ObjUser.TelefonoUsuario = changedRows[0]["telefono_usuario"].ToString().Trim().ToUpper();
                ObjUser.EmailUsuario = changedRows[0]["email_usuario"].ToString().Trim().ToUpper();
                ObjUser.IdEstado = Int32.Parse(changedRows[0]["id_estado"].ToString().Trim());
                ObjUser.IdEmpresa = Int32.Parse(changedRows[0]["id_empresa"].ToString().Trim());
                ObjUser.CambiarClave = Boolean.Parse(changedRows[0]["cambiar_clave"].ToString().Trim()) == true ? "S" : "N";
                ObjUser.ManejarFueraOficina = Boolean.Parse(changedRows[0]["manejar_fuera_oficina"].ToString().Trim()) == true ? "S" : "N";
                ObjUser.IpEquipoOficina = changedRows[0]["ip_equipo_oficina"].ToString().Trim().ToUpper();
                string _ClaveDinamica = ObjUtils.GetRandom();
                ObjUser.PasswordUsuario = _ClaveDinamica.ToString().Trim();
                ObjUser.FechaExpClave = DateTime.Now.AddDays(Int32.Parse(this.Session["DiasExpClave"].ToString().Trim())).ToString("yyyy-MM-dd");
                ObjUser.IdUsuarioAdd = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjUser.IdUsuarioUp = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjUser.IdEmpresaPadre = Int32.Parse(this.Session["IdEmpresaAdmon"].ToString().Trim());
                ObjUser.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
                ObjUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjUser.TipoProceso = 2;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjUser);

                int _IdUsuario = 0;
                string _MsgError = "", _MsgInfo = "";
                if (ObjUser.AddUpUsuario(changedRows[0], ref _IdUsuario, ref _MsgError))
                {
                    string _NombreUsuario = changedRows[0]["nombre_usuario"].ToString().Trim().ToUpper() + " " + changedRows[0]["apellido_usuario"].ToString().Trim().ToUpper();
                    changedRows[0]["nombre_completo_usuario"] = _NombreUsuario;
                    changedRows[0]["nombre_usuario"] = changedRows[0]["nombre_usuario"].ToString().Trim().ToUpper();
                    changedRows[0]["apellido_usuario"] = changedRows[0]["apellido_usuario"].ToString().Trim().ToUpper();
                    changedRows[0]["direccion_usuario"] = changedRows[0]["direccion_usuario"].ToString().Trim().ToUpper();
                    changedRows[0]["telefono_usuario"] = changedRows[0]["telefono_usuario"].ToString().Trim().ToUpper();
                    changedRows[0]["email_usuario"] = changedRows[0]["email_usuario"].ToString().Trim().ToUpper();
                    changedRows[0]["medio_envio"] = CmbMedioEnvio.ToString().Trim().ToUpper();
                    changedRows[0]["nombre_rol"] = CmbPerfil.ToString().Trim().ToUpper();
                    changedRows[0]["nombre_empresa"] = CmbEmpresa.ToString().Trim().ToUpper();
                    changedRows[0]["codigo_estado"] = CmbEstado.ToString().Trim().ToUpper();
                    changedRows[0].EndEdit();

                    #region DEFINICION DEL METODO PARA ENVIO DE CORREO
                    //--AQUI VALIDAMOS EL CODIGO DE RESPUESTA SI ES 01 SE CAMBIO TAMBIEN LA CLAVE Y SE ENVIA EL CORREO
                    string[] _Mensaje = _MsgError.ToString().Trim().Split('|');
                    _MsgInfo = _Mensaje[1].ToString().Trim();
                    if (_Mensaje[0].ToString().Trim().Equals("01"))
                    {
                        //--Definir valores para envio del email
                        ObjCorreo.StrServerCorreo = Session["ServerCorreo"].ToString().Trim();
                        ObjCorreo.PuertoCorreo = Int32.Parse(Session["PuertoCorreo"].ToString().Trim());
                        ObjCorreo.StrEmailDe = Session["EmailSoporte"].ToString().Trim();
                        ObjCorreo.StrPasswordDe = Session["PasswordEmail"].ToString().Trim();
                        ObjCorreo.StrEmailPara = changedRows[0]["email_usuario"].ToString().Trim();
                        ObjCorreo.StrAsunto = "REF.: EDITAR DATOS DE USUARIO";

                        string nHora = DateTime.Now.ToString("HH");
                        string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                        StringBuilder strDetalleEmail = new StringBuilder();
                        strDetalleEmail.Append("<h4>" + strTime + ", Para informarle que su usuario le fue cambiado su numero de documento por esa razón se le genero una nueva clave para poder ingresar al [" + FixedData.PlatformName + "].</h4>" + "<br/>" +
                                        "<h4>DATOS DEL USUARIO EDITADO</h2>" + "<br/>" +
                                        "Nombre: " + _NombreUsuario + "<br/>" +
                                        "Usuario: " + ObjUser.LoginUsuario + "<br/>" +
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
                            _MsgInfo = _MsgInfo + " Pero " + _MsgErrorEmail.ToString().Trim();
                        }
                    }
                    #endregion

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.ModuloApp = "USUARIOS";
                    //--TIPOS DE EVENTO: 1. LOGIN, 2. INSERT, 3. UPDATE, 4. DELETE, 5. CONSULTA
                    ObjAuditoria.IdTipoEvento = 3;
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
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgInfo;
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al editar los datos del usuario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
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
                e.Canceled = true;
                #endregion
            }
        }

        protected void RadGrid1_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            int _IdUsuario = 0;
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            DataTable TablaDatos = new DataTable();
            TablaDatos = this.FuenteDatosUser.Tables["DtUsuarios"];
            TablaDatos.PrimaryKey = new DataColumn[] { TablaDatos.Columns["id_usuario"] };
            DataRow[] changedRows = TablaDatos.Select("id_usuario = " + editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["id_usuario"].ToString());
            _IdUsuario = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["id_usuario"].ToString().Trim());

            try
            {
                ObjUser.IdUsuario = Int32.Parse(changedRows[0]["id_usuario"].ToString().Trim());
                ObjUser.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjUser.IdentificacionUsuario = changedRows[0]["identificacion_usuario"].ToString().Trim().ToUpper();
                ObjUser.NombreUsuario = changedRows[0]["nombre_usuario"].ToString().Trim().ToUpper();
                ObjUser.ApellidoUsuario = changedRows[0]["apellido_usuario"].ToString().Trim().ToUpper();
                ObjUser.LoginUsuario = changedRows[0]["identificacion_usuario"].ToString().Trim().ToUpper();
                ObjUser.DireccionUsuario = changedRows[0]["direccion_usuario"].ToString().Trim().ToUpper();
                ObjUser.TelefonoUsuario = changedRows[0]["telefono_usuario"].ToString().Trim().ToUpper();
                ObjUser.EmailUsuario = changedRows[0]["email_usuario"].ToString().Trim().ToUpper();
                ObjUser.IdEstado = Int32.Parse(changedRows[0]["id_estado"].ToString().Trim());
                ObjUser.IdEmpresa = Int32.Parse(changedRows[0]["id_empresa"].ToString().Trim());
                ObjUser.CambiarClave = Boolean.Parse(changedRows[0]["cambiar_clave"].ToString().Trim()) == true ? "S" : "N";
                ObjUser.ManejarFueraOficina = Boolean.Parse(changedRows[0]["manejar_fuera_oficina"].ToString().Trim()) == true ? "S" : "N";
                ObjUser.IpEquipoOficina = changedRows[0]["ip_equipo_oficina"].ToString().Trim().ToUpper();
                string _ClaveDinamica = ObjUtils.GetRandom();
                ObjUser.PasswordUsuario = _ClaveDinamica.ToString().Trim();
                ObjUser.FechaExpClave = DateTime.Now.AddDays(Int32.Parse(this.Session["DiasExpClave"].ToString().Trim())).ToString("yyyy-MM-dd");
                ObjUser.IdUsuarioAdd = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjUser.IdUsuarioUp = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjUser.IdEmpresaPadre = Int32.Parse(this.Session["IdEmpresaAdmon"].ToString().Trim());
                ObjUser.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
                ObjUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjUser.TipoProceso = 4;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjUser);

                string _MsgError = "";
                if (ObjUser.AddUpUsuario(changedRows[0], ref _IdUsuario, ref _MsgError))
                {
                    //Aqui eliminamos el registro del usuario de la lista
                    TablaDatos.Rows.Find(_IdUsuario).Delete();
                    TablaDatos.AcceptChanges();

                    //Actualizamos la Lista
                    this.ViewState["_FuenteDatosUser"] = null;
                    this.RadGrid1.Rebind();

                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.ModuloApp = "USUARIOS";
                    //--TIPOS DE EVENTO: 1. LOGIN, 2. INSERT, 3. UPDATE, 4. DELETE, 5. CONSULTA
                    ObjAuditoria.IdTipoEvento = 4;
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al eliminar el usuario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
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
                e.Canceled = true;
                #endregion
            }
        }
        #endregion

    }
}