using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Administracion;
using System.Web.Caching;
using Microsoft.Win32.TaskScheduler;
using System.Data;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmEjecucionPorLoteIca : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        EjecucionXLoteUser ObjEjecUser = new EjecucionXLoteUser();
        EjecucionXLoteFiltros ObjEjecFiltros = new EjecucionXLoteFiltros();

        protected void LstTipoFiltros()
        {
            try
            {
                ObjEjecUser.TipoConsulta = 2;
                ObjEjecUser.IdEjecucionLote = null;
                int _IdRol = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                if (_IdRol == 1 || _IdRol == 2)
                {
                    ObjEjecUser.IdUsuario = null;
                }
                else
                {
                    ObjEjecUser.IdUsuario = this.Session["IdUsuario"] != null ? this.Session["IdUsuario"].ToString().Trim() : null;
                }
                ObjEjecUser.IdEstado = 1;
                ObjEjecUser.MostrarSeleccione = "SI";
                ObjEjecUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                this.CmbTipoFiltro.DataSource = ObjEjecUser.GetEjecucionXLoteUser();
                this.CmbTipoFiltro.DataValueField = "idejecucion_lote";
                this.CmbTipoFiltro.DataTextField = "nombre_proceso";
                this.CmbTipoFiltro.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los tipos de filtros. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(330);
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

        protected void LstTipoImpuestosFiltro()
        {
            try
            {
                ObjEjecFiltros.TipoConsulta = 2;
                ObjEjecFiltros.IdEjecucionLote = this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoFiltro.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.IdFormImpuesto = null;
                ObjEjecFiltros.IdDepartamento = null;
                ObjEjecFiltros.IdMunicipio = null;
                ObjEjecFiltros.IdEstado = 1;
                ObjEjecFiltros.MostrarSeleccione = "SI";
                ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                this.CmbTipoImpuesto.DataSource = ObjEjecFiltros.GetEjecucionXLoteFiltroTipoImp();
                this.CmbTipoImpuesto.DataValueField = "idformulario_impuesto";
                this.CmbTipoImpuesto.DataTextField = "descripcion_formulario";
                this.CmbTipoImpuesto.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los tipos de impuestos. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(330);
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

        protected void LstDptosFiltro()
        {
            try
            {
                ObjEjecFiltros.TipoConsulta = 3;
                ObjEjecFiltros.IdEjecucionLote = this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoFiltro.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedValue.ToString().Trim() : "-1";
                ObjEjecFiltros.IdDepartamento = null;
                ObjEjecFiltros.IdMunicipio = null;
                ObjEjecFiltros.IdEstado = 1;
                ObjEjecFiltros.MostrarSeleccione = "SI";
                ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjEjecFiltros.GetEjecucionXLoteFiltroDpto();
                this.CmbDepartamento.DataSource = dtDatos;
                this.CmbDepartamento.DataValueField = "id_dpto";
                this.CmbDepartamento.DataTextField = "nombre_dpto";
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
                Ventana.Height = Unit.Pixel(330);
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

        protected void LstMunicipioFiltro()
        {
            try
            {
                ObjEjecFiltros.TipoConsulta = 4;
                ObjEjecFiltros.IdEjecucionLote = this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoFiltro.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.IdDepartamento = this.CmbDepartamento.SelectedValue.ToString().Trim().Length > 0 ? this.CmbDepartamento.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.IdMunicipio = null;
                ObjEjecFiltros.IdEstado = 1;
                ObjEjecFiltros.MostrarSeleccione = "SI";
                ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                String _MsgError = "";
                dtDatos = ObjEjecFiltros.GetEjecucionXLoteFiltroMunicipio(ref _MsgError);
                this.CmbMunicipio.DataSource = dtDatos;
                this.CmbMunicipio.DataValueField = "id_municipio";
                this.CmbMunicipio.DataTextField = "nombre_municipio";
                this.CmbMunicipio.DataBind();

                //--AQUI VALIDAMOS EL ESTADO DE LA DECLARACION
                ObjEjecFiltros.TipoConsulta = 5;
                int _IdEstadoDeclaracion = ObjEjecFiltros.GetValidacionEstadoDeclaracion();
                if (_IdEstadoDeclaracion == 2)      //--PRELIMINAR
                {
                    this.RbEstadoDeclaracion.ClearSelection();
                    this.RbEstadoDeclaracion.SelectedValue = _IdEstadoDeclaracion.ToString();
                    this.Validador6.Enabled = true;
                }
                else if (_IdEstadoDeclaracion == 3) //--DEFINITIVO
                {
                    this.RbEstadoDeclaracion.ClearSelection();
                    this.RbEstadoDeclaracion.SelectedValue = _IdEstadoDeclaracion.ToString();
                    this.Validador6.Enabled = true;
                    this.BtnEjecutar.Enabled = false;
                    this.LblMensaje.Text = "Señor usuario esta declaración ya se encuentra en estado DEFINITIVA";
                    this.LblMensaje.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    this.RbEstadoDeclaracion.ClearSelection();
                    this.Validador6.Enabled = false;
                    this.BtnEjecutar.Enabled = false;
                    this.LblMensaje.Text = "Señor usuario esta declaración ya se encuentra en estado DEFINITIVA";
                    this.LblMensaje.ForeColor = System.Drawing.Color.Red;
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los municipios. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(330);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //this.AplicarPermisos();
                //this.ViewState["TipoProceso"] = Request.QueryString["TipoProceso"].ToString().Trim();

                //Llenar los combox
                this.LstTipoFiltros();
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

        protected void CmbTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;
            //--
            this.LstTipoImpuestosFiltro();
        }

        protected void CmbTipoImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;
            //--
            this.LstDptosFiltro();
        }

        protected void CmbDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;
            //--
            this.LstMunicipioFiltro();
        }

        protected void CmbMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                #region VALIDAR EL ESTADO DE LA DECLARACION
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;
                //--VALIDAR QUE EL MUNICIPIO SE ALLA SELECCIONADO
                if(this.CmbMunicipio.SelectedValue.ToString().Trim().Length > 0)
                {
                    #region OBTENER EL ESTADO DE LA DECLARACION 
                    ObjEjecFiltros.TipoConsulta = 5;
                    ObjEjecFiltros.IdEjecucionLote = this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoFiltro.SelectedValue.ToString().Trim() : null;
                    ObjEjecFiltros.IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedValue.ToString().Trim() : "-1";
                    ObjEjecFiltros.IdDepartamento = this.CmbDepartamento.SelectedValue.ToString().Trim().Length > 0 ? this.CmbDepartamento.SelectedValue.ToString().Trim() : null;
                    //--
                    string[] _ArrayMunicipio = this.CmbMunicipio.SelectedValue.ToString().Trim().Split('|');
                    ObjEjecFiltros.IdMunicipio = _ArrayMunicipio[0].ToString().Trim();
                    ObjEjecFiltros.IdEstado = 1;
                    ObjEjecFiltros.MostrarSeleccione = "SI";
                    ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                    //--AQUI VALIDAMOS EL ESTADO DE LA DECLARACION
                    int _IdEstadoDeclaracion = ObjEjecFiltros.GetValidacionEstadoDeclaracion();
                    if (_IdEstadoDeclaracion == 2)      //--PRELIMINAR
                    {
                        this.RbEstadoDeclaracion.ClearSelection();
                        this.RbEstadoDeclaracion.SelectedValue = _IdEstadoDeclaracion.ToString();
                        this.Validador6.Enabled = true;
                    }
                    else if (_IdEstadoDeclaracion == 3) //--DEFINITIVO
                    {
                        this.RbEstadoDeclaracion.ClearSelection();
                        this.RbEstadoDeclaracion.SelectedValue = _IdEstadoDeclaracion.ToString();
                        this.Validador6.Enabled = true;
                        this.BtnEjecutar.Enabled = false;
                        this.LblMensaje.Text = "Señor usuario esta declaración ya se encuentra en estado DEFINITIVA";
                        this.LblMensaje.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        this.RbEstadoDeclaracion.ClearSelection();
                        this.Validador6.Enabled = false;
                        this.BtnEjecutar.Enabled = false;
                        this.LblMensaje.Text = "Señor usuario esta declaración ya se encuentra en estado DEFINITIVA";
                        this.LblMensaje.ForeColor = System.Drawing.Color.Red;
                    }
                    #endregion
                }
                else
                {
                    #region OBTENER EL ESTADO DE LA DECLARACION 
                    ObjEjecFiltros.TipoConsulta = 5;
                    ObjEjecFiltros.IdEjecucionLote = this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoFiltro.SelectedValue.ToString().Trim() : null;
                    ObjEjecFiltros.IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedValue.ToString().Trim() : "-1";
                    ObjEjecFiltros.IdDepartamento = this.CmbDepartamento.SelectedValue.ToString().Trim().Length > 0 ? this.CmbDepartamento.SelectedValue.ToString().Trim() : null;
                    ObjEjecFiltros.IdMunicipio = null;
                    ObjEjecFiltros.IdEstado = 1;
                    ObjEjecFiltros.MostrarSeleccione = "SI";
                    ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                    //--AQUI VALIDAMOS EL ESTADO DE LA DECLARACION
                    int _IdEstadoDeclaracion = ObjEjecFiltros.GetValidacionEstadoDeclaracion();
                    if (_IdEstadoDeclaracion == 2)      //--PRELIMINAR
                    {
                        this.RbEstadoDeclaracion.ClearSelection();
                        this.RbEstadoDeclaracion.SelectedValue = _IdEstadoDeclaracion.ToString();
                        this.Validador6.Enabled = true;
                    }
                    else if (_IdEstadoDeclaracion == 3) //--DEFINITIVO
                    {
                        this.RbEstadoDeclaracion.ClearSelection();
                        this.RbEstadoDeclaracion.SelectedValue = _IdEstadoDeclaracion.ToString();
                        this.Validador6.Enabled = true;
                        this.BtnEjecutar.Enabled = false;
                        this.LblMensaje.Text = "Señor usuario esta declaración ya se encuentra en estado DEFINITIVA";
                        this.LblMensaje.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        this.RbEstadoDeclaracion.ClearSelection();
                        this.Validador6.Enabled = false;
                        this.BtnEjecutar.Enabled = false;
                        this.LblMensaje.Text = "Señor usuario esta declaración ya se encuentra en estado DEFINITIVA";
                        this.LblMensaje.ForeColor = System.Drawing.Color.Red;
                    }
                    #endregion
                }
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al validar el estado de la declaración. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(330);
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

        protected void BtnEjecutar_Click(object sender, EventArgs e)
        {
            try
            {
                //using (TaskService ts = new TaskService())
                //{
                //    Task t = ts.GetTask(taskName);
                //    if (t != null)
                //    {
                //        // get status here or get runtime
                //        var isEnabled = t.Enabled;
                //        var runs = t.GetRunTimes(startDate, endDate);
                //    }
                //}

                // Get the service on the remote machine
                //using (TaskService ts = new TaskService(@"\\RemoteServer"))
                using (TaskService ts = new TaskService())
                {
                    // Create a new task definition and assign properties
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Does something";

                    // Create a trigger that will fire the task at this time every other day
                    td.Triggers.Add(new DailyTrigger { DaysInterval = 2 });

                    // Create an action that will launch Notepad whenever the trigger fires
                    td.Actions.Add(new ExecAction("notepad.exe", "D:\\PRUEBA\\prueba.txt", null));

                    // Register the task in the root folder
                    ts.RootFolder.RegisterTaskDefinition(@"Test", td);
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al realizar la Ejecución por Lote. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(330);
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
}