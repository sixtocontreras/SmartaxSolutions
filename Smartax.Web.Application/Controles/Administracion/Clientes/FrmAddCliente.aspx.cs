using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.Caching;
using System.Text;
using System.Web.Script.Serialization;
using log4net;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Parametros.Divipola;

namespace Smartax.Web.Application.Controles.Administracion.Clientes
{
    public partial class FrmAddCliente : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        #region DEFINICION DE OBJETOS DE CLASES
        Cliente ObjCliente = new Cliente();
        Departamento ObjDpto = new Departamento();
        Municipio ObjMunicipio = new Municipio();
        TipoIdentificacion ObjTipoIde = new TipoIdentificacion();
        TiposClasificacion ObjTipoClasificacion = new TiposClasificacion();
        Estado ObjEstado = new Estado();
        TipoSector ObjTipoSector = new TipoSector();
        EnvioCorreo ObjCorreo = new EnvioCorreo();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        Utilidades ObjUtils = new Utilidades();
        #endregion

        #region DEFINICION METODOS LISTAR COMBOBOX
        protected void LstTipoIdentificacion()
        {
            try
            {
                ObjTipoIde.TipoConsulta = 2;
                ObjTipoIde.IdEstado = 1;
                ObjTipoIde.MostrarSeleccione = "SI";
                ObjTipoIde.IdRol = Convert.ToInt32(Session["IdRol"].ToString().Trim());
                ObjTipoIde.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjTipoIde.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbTipoIdentificacion.DataSource = ObjTipoIde.GetIdentificacion();
                this.CmbTipoIdentificacion.DataValueField = "idtipo_identificacion";
                this.CmbTipoIdentificacion.DataTextField = "tipo_identificacion";
                this.CmbTipoIdentificacion.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los tipos de documentos. Motivo: " + ex.ToString();
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
                _log.Error(_MsgMensaje);
                #endregion
            }
        }

        protected void LstTipoClasificacion()
        {
            try
            {
                ObjTipoClasificacion.TipoConsulta = 2;
                ObjTipoClasificacion.IdEstado = 1;
                ObjTipoClasificacion.MostrarSeleccione = "SI";
                ObjTipoClasificacion.IdRol = Convert.ToInt32(Session["IdRol"].ToString().Trim());
                ObjTipoClasificacion.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjTipoClasificacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbTipoClasificacion.DataSource = ObjTipoClasificacion.GetTipoClasificacion();
                this.CmbTipoClasificacion.DataValueField = "idtipo_clasificacion";
                this.CmbTipoClasificacion.DataTextField = "tipo_clasificacion";
                this.CmbTipoClasificacion.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los tipos de clasificacion. Motivo: " + ex.ToString();
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
                _log.Error(_MsgMensaje);
                #endregion
            }
        }

        protected void LstTipoSector()
        {
            try
            {
                ObjTipoSector.TipoConsulta = 2;
                ObjTipoSector.IdEstado = null;
                ObjTipoSector.MostrarSeleccione = "SI";
                ObjTipoSector.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbTipoSector.DataSource = ObjTipoSector.GetTipoSector();
                this.CmbTipoSector.DataValueField = "idtipo_sector";
                this.CmbTipoSector.DataTextField = "tipo_sector";
                this.CmbTipoSector.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los estados. Motivo: " + ex.ToString();
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
                #endregion
            }
        }

        protected void LstDepartamentos()
        {
            try
            {
                ObjDpto.TipoConsulta = 2;
                ObjDpto.IdEstado = 1;
                ObjDpto.MostrarSeleccione = "SI";
                ObjDpto.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjDpto.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbDepartamento.DataSource = ObjDpto.GetDptos();
                this.CmbDepartamento.DataValueField = "id_dpto";
                this.CmbDepartamento.DataTextField = "nombre_departamento";
                this.CmbDepartamento.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los departamentos. Motivo: " + ex.ToString();
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
                #endregion
            }
        }

        protected void LstEstado()
        {
            try
            {
                ObjEstado.TipoConsulta = 2;
                ObjEstado.IdEstado = null;
                ObjEstado.TipoEstado = "INTERFAZ";
                ObjEstado.MostrarSeleccione = "NO";
                ObjEstado.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbEstado.DataSource = ObjEstado.GetEstados();
                this.CmbEstado.DataValueField = "id_estado";
                this.CmbEstado.DataTextField = "codigo_estado";
                this.CmbEstado.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los estados. Motivo: " + ex.ToString();
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
                _log.Error(_MsgMensaje);
                #endregion
            }
        }
        #endregion

        #region DEFINICION DE METODOS PARA OBTENER DATOS DE CLIENTES Y MUNICIPIOS
        private void GetInfoCliente()
        {
            try
            {
                ObjCliente.TipoConsulta = 3;
                ObjCliente.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                ObjCliente.IdEstado = null;
                ObjCliente.IdRol = Int32.Parse(Session["IdRol"].ToString().Trim());
                ObjCliente.IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString().Trim());
                ObjCliente.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjCliente.GetInfoCliente();

                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        this.CmbTipoClasificacion.SelectedValue = dtDatos.Rows[0]["idtipo_clasificacion"].ToString().Trim();
                        this.LstTipoClasificacion();
                        this.CmbTipoIdentificacion.SelectedValue = dtDatos.Rows[0]["idtipo_identificacion"].ToString().Trim();
                        this.LstTipoIdentificacion();
                        this.TxtNumDocumento.Text = dtDatos.Rows[0]["numero_documento"].ToString().Trim();
                        this.LblDv.Text = dtDatos.Rows[0]["digito_verificacion"].ToString().Trim();
                        this.TxtRazonSocial.Text = dtDatos.Rows[0]["razon_social"].ToString().Trim();
                        this.RbInscripcionRit.SelectedValue = dtDatos.Rows[0]["inscrito_rit"].ToString().Trim().Equals("S") ? "1" : "2";
                        this.LblUbicacionDpto.Text = dtDatos.Rows[0]["nombre_departamento"].ToString().Trim();
                        this.LblUbicacionPrincipal.Text = dtDatos.Rows[0]["id_municipio"].ToString().Trim() + " - " + dtDatos.Rows[0]["nombre_municipio"].ToString().Trim();
                        this.TxtDireccion.Text = dtDatos.Rows[0]["direccion_cliente"].ToString().Trim();
                        this.TxtContacto.Text = dtDatos.Rows[0]["nombre_contacto"].ToString().Trim();
                        this.TxtTelefono.Text = dtDatos.Rows[0]["telefono_contacto"].ToString().Trim();
                        this.TxtEmail.Text = dtDatos.Rows[0]["email_contacto"].ToString().Trim();
                        this.TxtNumPuntos.Text = dtDatos.Rows[0]["numero_puntos"].ToString().Trim();
                        this.RbPresenciaNacional.SelectedValue = dtDatos.Rows[0]["tiene_presencia_nacional"].ToString().Trim().Equals("S") ? "1" : "2";
                        this.RbConsorcioUnionTemp.SelectedValue = dtDatos.Rows[0]["consorcio_union_temporal"].ToString().Trim().Equals("S") ? "1" : "2";
                        this.RbGranContribuyente.SelectedValue = dtDatos.Rows[0]["gran_contribuyente"].ToString().Trim().Equals("S") ? "1" : "2";
                        //this.TxtNumPlacaMunicipal.Text = dtDatos.Rows[0]["numero_placa_municipal"].ToString().Trim();
                        //this.TxtNumMatriculaIc.Text = dtDatos.Rows[0]["numero_matricula_ic"].ToString().Trim();
                        //this.TxtNumRit.Text = dtDatos.Rows[0]["numero_rit"].ToString().Trim();
                        //this.RbAvisosTablero.SelectedValue = dtDatos.Rows[0]["avisos_tablero"].ToString().Trim().Equals("S") ? "1" : "2";
                        //string dtFIncioActividades = dtDatos.Rows[0]["fecha_inicio_actividades"].ToString().Trim().Length > 0 ? dtDatos.Rows[0]["fecha_inicio_actividades"].ToString().Trim() : "";
                        //this.dtFechaInicioAct.SelectedDate = dtFIncioActividades.ToString().Trim().Length > 0 ? Convert.ToDateTime(dtFIncioActividades.ToString().Trim()) : DateTime.Now;
                        this.CmbTipoSector.SelectedValue = dtDatos.Rows[0]["idtipo_sector"].ToString().Trim();
                        this.CmbEstado.SelectedValue = dtDatos.Rows[0]["id_estado"].ToString().Trim();
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
                        string _MsgMensaje = "No se encontro información con el Cliente seleccionado... !";
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
                        _log.Error(_MsgMensaje);
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al mostrar los datos del cliente. Motivo: " + ex.ToString();
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
                _log.Error(_MsgMensaje);
                #endregion
            }
        }

        public DataSet GetMunicipios()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjMunicipio.TipoConsulta = 4;
                ObjMunicipio.IdDpto = this.CmbDepartamento.SelectedValue.ToString().Trim().Length > 0 ? this.CmbDepartamento.SelectedValue.ToString().Trim() : null;
                ObjMunicipio.IdMunicipio = null;
                ObjMunicipio.CodigoDane = this.TxtCodDane.Text.ToString().Trim().Length > 0 ? this.TxtCodDane.Text.ToString().Trim().ToUpper() : null;
                ObjMunicipio.NombreMunicipio = this.TxtNomMunicipio.Text.ToString().Trim().Length > 0 ? this.TxtNomMunicipio.Text.ToString().Trim().ToUpper() : null;
                ObjMunicipio.IdEstado = null;
                ObjMunicipio.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //--LISTA DE MUNICIPIOS
                ObjetoDataTable = ObjMunicipio.GetLstMunicipios();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_municipio"] };
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
                string _MsgError = "Error al listar los municipios. Motivo: " + ex.ToString();
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
                    ConjuntoDatos = GetMunicipios();
                    this.ViewState["_FuenteDatos"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatos"];
                }
            }
            set { this.ViewState["_FuenteDatos"] = value; }
        }
        #endregion

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = Request.QueryString["PathUrl"].ToString().Trim();
            objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

            objPermiso.RefrescarPermisos();
            if (!objPermiso.PuedeRegistrar)
            {
                this.BtnGuardar.Enabled = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.AplicarPermisos();
                this.ViewState["TipoProceso"] = Request.QueryString["TipoProceso"].ToString().Trim();

                //--LISTAR COMBOBOX
                this.LstTipoIdentificacion();
                this.LstTipoClasificacion();
                this.LstTipoSector();
                this.LstDepartamentos();
                this.LstEstado();

                if (this.ViewState["TipoProceso"].Equals("INSERT"))
                {
                    this.LblTitulo.Text = "REGISTRAR INFORMACIÓN DEL CLIENTE";
                }
                else if (this.ViewState["TipoProceso"].Equals("UPDATE"))
                {
                    this.LblTitulo.Text = "EDITAR INFORMACIÓN DEL CLIENTE";

                    //--Obtenemos los valores de parametros
                    this.ViewState["IdCliente"] = Request.QueryString["IdCliente"].ToString().Trim();
                    this.GetInfoCliente();
                }
            }
        }

        #region DEFINICION DE EVENTOS DEL PAGE
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            string str = string.Format("VS_{0}_{1}", Request.UserHostAddress, DateTime.Now.Ticks);
            Cache.Add(str, state, null, DateTime.Now.AddMinutes(Session.Timeout), TimeSpan.Zero, CacheItemPriority.Default, null);
            ClientScript.RegisterHiddenField("__VIEWSTATE_KEY", str);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            string str = Request.Form["__VIEWSTATE_KEY"];
            if (!str.StartsWith("VS_"))
            {
                throw new Exception("Invalid ViewState");
            }
            return Cache[str];
        }
        #endregion

        #region DEFINICION DE METODOS DEL GRID DE MUNICIPIOS
        protected void BtnConsultar_Click(object sender, ImageClickEventArgs e)
        {
            this.ViewState["_FuenteDatos"] = null;
            this.RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid1.DataSource = this.FuenteDatos;
                this.RadGrid1.DataMember = "DtMunicipios";
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
                if (e.CommandName == "BtnSeleccion")
                {
                    #region ASIGNAR IMPUESTOS DEL MUNICIPIO
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdMunicipio = Convert.ToInt32(item.GetDataKeyValue("id_municipio").ToString().Trim());
                    string _NombreDpto = item["nombre_departamento"].Text.ToString().Trim();
                    string _NombreMunicipio = item["nombre_municipio"].Text.ToString().Trim();

                    this.LblMsg.Text = "";
                    this.ModalPopupExtender1.Hide();

                    //--ACTUALIZAR FORM
                    this.UpdatePanel2.Update();
                    this.UpdatePanel5.Update();
                    this.LblUbicacionDpto.Text = _NombreDpto;
                    this.LblUbicacionPrincipal.Text = _IdMunicipio + " - " + _NombreMunicipio;
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

        protected void TxtNumDocumento_TextChanged(object sender, EventArgs e)
        {
            this.LblDv.Text = ObjCliente.GetDigitoVerificacion(this.TxtNumDocumento.Text.ToString().Trim()).ToString();
            this.TxtRazonSocial.Focus();
        }

        protected void BtnAddUbicacion_Click(object sender, ImageClickEventArgs e)
        {
            this.LblMsg.Text = "";
            this.ModalPopupExtender1.Show();
        }

        protected void BtnSalir2_Click(object sender, EventArgs e)
        {
            this.LblMsg.Text = "";
            this.ModalPopupExtender1.Hide();
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string[] _Ubicacion = this.LblUbicacionPrincipal.Text.ToString().Trim().Split('-');

                if (_Ubicacion[0].ToString().Trim().Length > 0)
                {
                    #region DEFINICION DE VARIABLES OBJETO CLIENTE
                    if (this.ViewState["TipoProceso"].Equals("INSERT"))
                    {
                        ObjCliente.IdCliente = null;
                        ObjCliente.TipoProceso = 1;
                    }
                    else if (this.ViewState["TipoProceso"].Equals("UPDATE"))
                    {
                        ObjCliente.IdCliente = this.ViewState["IdCliente"].ToString().Trim();
                        ObjCliente.TipoProceso = 2;
                    }

                    ObjCliente.IdTipoClasificacion = Int32.Parse(this.CmbTipoClasificacion.SelectedValue.ToString().Trim());
                    ObjCliente.IdTipoIdentificacion = Int32.Parse(this.CmbTipoIdentificacion.SelectedValue.ToString().Trim());
                    ObjCliente.NumeroDocumento = this.TxtNumDocumento.Text.ToString().Trim();
                    ObjCliente.DigitoVerificacion = this.LblDv.Text.ToString().Trim();
                    ObjCliente.RazonSocial = this.TxtRazonSocial.Text.ToString().Trim().ToUpper();
                    ObjCliente.InscritoRit = this.RbInscripcionRit.SelectedValue.ToString().Trim().Equals("1") ? "S" : "N";
                    ObjCliente.IdMunUbicacionPrincipal = Int32.Parse(_Ubicacion[0].ToString().Trim());
                    ObjCliente.DireccionCliente = this.TxtDireccion.Text.ToString().Trim().ToUpper();
                    ObjCliente.NombreContacto = this.TxtContacto.Text.ToString().Trim().ToUpper();
                    ObjCliente.TelefonoContacto = this.TxtTelefono.Text.ToString().Trim().ToUpper();
                    ObjCliente.EmailContacto = this.TxtEmail.Text.ToString().Trim().ToUpper();
                    ObjCliente.NumeroPuntos = Int32.Parse(this.TxtNumPuntos.Text.ToString().Trim());
                    ObjCliente.TienePresenciaNacional = this.RbPresenciaNacional.SelectedValue.ToString().Trim().Equals("1") ? "S" : "N";
                    ObjCliente.ConsorcioUnionTemporal = this.RbConsorcioUnionTemp.SelectedValue.ToString().Trim().Equals("1") ? "S" : "N";
                    ObjCliente.ActividadPatrimAutonomo = this.RbActividadPatrimAut.SelectedValue.ToString().Trim().Equals("1") ? "S" : "N";
                    ObjCliente.GranContribuyente = this.RbGranContribuyente.SelectedValue.ToString().Trim().Equals("1") ? "S" : "N";
                    //ObjCliente.NumeroPlacaMunicipal = this.TxtNumPlacaMunicipal.Text.ToString().Trim().ToUpper();
                    //ObjCliente.NumeroMatriculaIc = this.TxtNumMatriculaIc.Text.ToString().Trim().ToUpper();
                    //ObjCliente.NumeroRit = this.TxtNumRit.Text.ToString().Trim().ToUpper();
                    //ObjCliente.AvisosTablero = this.RbAvisosTablero.SelectedValue.ToString().Trim().Equals("1") ? "S" : "N";
                    //string dtFechaInicio = this.dtFechaInicioAct.SelectedDate.ToString().Trim().Length > 0 ? this.dtFechaInicioAct.SelectedDate.ToString().Trim() : "";
                    //ObjCliente.FechaInicioActividades = dtFechaInicio.ToString().Trim().Length > 0 ? Convert.ToDateTime(dtFechaInicio.ToString().Trim()).ToString("yyyy-MM-dd") : null;
                    //--DATOS DEL USUARIO
                    string _LoginUser = ObjCliente.NumeroDocumento + ObjCliente.DigitoVerificacion;
                    string _ClaveDinamica = ObjUtils.GetRandom();
                    ObjCliente.PasswordUsuario = _ClaveDinamica;
                    ObjCliente.IdTipoSector = this.CmbTipoSector.SelectedValue.ToString().Trim();
                    ObjCliente.IdEstado = this.CmbEstado.SelectedValue.ToString().Trim();
                    ObjCliente.IdRol = 3;   //ID ROL: ADMIN CLIENTE
                    ObjCliente.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                    ObjCliente.IdEmpresaPadre = Int32.Parse(this.Session["IdEmpresaAdmon"].ToString().Trim());
                    ObjCliente.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjCliente.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                    //--AQUI SERIALIZAMOS EL OBJETO CLASE
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonRequest = js.Serialize(ObjCliente);
                    _log.Warn("REQUEST CLIENTE => " + jsonRequest);
                    #endregion

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjCliente.AddUpCliente(ref _IdRegistro, ref _MsgError))
                    {
                        if (this.ViewState["TipoProceso"].Equals("INSERT"))
                        {
                            #region DEFINICION DEL METODO PARA ENVIO DE CORREO
                            //--Definir valores para envio del email
                            ObjCorreo.StrServerCorreo = Session["ServerCorreo"].ToString().Trim();
                            ObjCorreo.PuertoCorreo = Int32.Parse(Session["PuertoCorreo"].ToString().Trim());
                            ObjCorreo.StrEmailDe = Session["EmailSoporte"].ToString().Trim();
                            ObjCorreo.StrPasswordDe = Session["PasswordEmail"].ToString().Trim();
                            ObjCorreo.StrEmailPara = ObjCliente.EmailContacto;
                            ObjCorreo.StrAsunto = "REF.: REGISTRO DE USUARIO";

                            string nHora = DateTime.Now.ToString("HH");
                            string strTime = ObjUtils.GetTime(Int32.Parse(nHora));
                            StringBuilder strDetalleEmail = new StringBuilder();
                            strDetalleEmail.Append("<h4>" + strTime + ", Para informarle que su usuario fue registrado exitosamente en la Plataforma y poder ingresar a [" + FixedData.PlatformName + "].</h4>" + "<br/>" +
                                            "<h4>DATOS DEL USUARIO REGISTRADO</h2>" + "<br/>" +
                                            "Nombre: " + ObjCliente.RazonSocial + "<br/>" +
                                            "Dirección: " + ObjCliente.DireccionCliente + "<br/>" +
                                            "Celular: " + ObjCliente.TelefonoContacto + "<br/>" +
                                            "Email: " + ObjCliente.EmailContacto + "<br/>" +

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
                            ObjAuditoria.ModuloApp = "CLIENTE";
                            //--TIPOS DE EVENTO: 1. LOGIN, 2. INSERT, 3. UPDATE, 4. DELETE, 5. CONSULTA
                            if(ObjCliente.TipoProceso == 1)
                            {
                                ObjAuditoria.IdTipoEvento = 2;
                            }
                            else
                            {
                                ObjAuditoria.IdTipoEvento = 3;
                            }
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
                        }
                        else if (this.ViewState["TipoProceso"].Equals("UPDATE"))
                        {
                            #region REGISTRO DE LOGS DE AUDITORIA
                            //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                            ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                            ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                            ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                            ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                            ObjAuditoria.ModuloApp = "CLIENTE";
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
                        }

                        this.UpdatePanel1.Update();
                        this.UpdatePanel3.Update();
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
                        _log.Info(_MsgError);
                        #endregion
                    }
                    else
                    {
                        this.UpdatePanel1.Update();
                        this.UpdatePanel3.Update();
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
                else
                {
                    this.UpdatePanel1.Update();
                    this.UpdatePanel3.Update();
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
                    string _MsgError = "Señor usuario, debe seleccionar la ubicacion principal donde esta ubicado el cliente ...!";
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
            catch (Exception ex)
            {
                this.UpdatePanel1.Update();
                this.UpdatePanel3.Update();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al guardar los datos del Cliente. Motivo: " + ex.ToString();
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
                _log.Error(_MsgMensaje);
                #endregion
            }
        }

    }
}