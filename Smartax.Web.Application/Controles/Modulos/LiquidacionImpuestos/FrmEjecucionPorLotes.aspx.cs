using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using log4net;
using System.Web.Caching;
using Microsoft.Win32.TaskScheduler;
using System.Data;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.ProcessAPIs;
using Smartax.Web.Application.Clases.Parametros;
using static Smartax.Web.Application.Clases.ProcessAPIs.ModelApiSmartax;

namespace Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos
{
    public partial class FrmEjecucionPorLotes : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        ProcessAPI objProceso = new ProcessAPI();
        EjecucionXLoteUser ObjEjecUser = new EjecucionXLoteUser();
        EjecucionXLoteFiltros ObjEjecFiltros = new EjecucionXLoteFiltros();
        Firmantes ObjFirmante = new Firmantes();
        Estado ObjEstado = new Estado();
        Combox ObjCombox = new Combox();
        Utilidades ObjUtils = new Utilidades();

        #region AQUI LLENAMOS LAS LISTAS PARA LOS FILTROS
        protected void LstAnioGravable()
        {
            try
            {
                ObjCombox.MostrarSeleccione = "SI";
                this.CmbAnioGravable.DataSource = ObjCombox.GetAnios();
                this.CmbAnioGravable.DataValueField = "id_anio";
                this.CmbAnioGravable.DataTextField = "numero_anio";
                this.CmbAnioGravable.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar el año gravable. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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

        protected void LstMeses()
        {
            try
            {
                ObjCombox.MostrarSeleccione = "SI";
                this.CmbMeses.DataSource = ObjCombox.GetMeses();
                this.CmbMeses.DataValueField = "id_mes";
                this.CmbMeses.DataTextField = "numero_mes";
                this.CmbMeses.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los meses. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                ObjEjecFiltros.AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0 ? this.CmbAnioGravable.SelectedValue.ToString().Trim() : DateTime.Now.ToString("yyyy");
                ObjEjecFiltros.IdFormImpuesto = null;
                ObjEjecFiltros.IdDepartamento = null;
                ObjEjecFiltros.IdMunicipio = null;
                ObjEjecFiltros.IdEstado = 1;
                ObjEjecFiltros.AprobacionJefe = null;
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
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                ObjEjecFiltros.AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0 ? this.CmbAnioGravable.SelectedValue.ToString().Trim() : DateTime.Now.ToString("yyyy");
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
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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

        protected void LstEstadoImpuesto()
        {
            try
            {
                ObjEstado.TipoConsulta = 2;
                ObjEstado.TipoEstado = "LIQ_IMPUESTO";
                ObjEstado.MostrarSeleccione = "SI";
                ObjEstado.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();

                this.CmbEstadoLiquidacion.DataSource = ObjEstado.GetEstados();
                this.CmbEstadoLiquidacion.DataValueField = "id_estado";
                this.CmbEstadoLiquidacion.DataTextField = "codigo_estado";
                this.CmbEstadoLiquidacion.DataBind();
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
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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

        protected void LstEstadosAprobacion()
        {
            try
            {
                ObjCombox.MostrarSeleccione = "SI";
                this.CmbEstadoAprobacion.DataSource = ObjCombox.GetEstadosAprobacion();
                this.CmbEstadoAprobacion.DataValueField = "id_aprobacion";
                this.CmbEstadoAprobacion.DataTextField = "tipo_aprobacion";
                this.CmbEstadoAprobacion.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los estados de aprobación. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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

        protected void GetConsultarMunicipios()
        {
            try
            {
                ObjEjecFiltros.TipoConsulta = 4;
                ObjEjecFiltros.IdEjecucionLote = this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoFiltro.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0 ? this.CmbAnioGravable.SelectedValue.ToString().Trim() : DateTime.Now.ToString("yyyy");
                ObjEjecFiltros.IdFormImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoImpuesto.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.IdDepartamento = this.CmbDepartamento.SelectedValue.ToString().Trim().Length > 0 ? this.CmbDepartamento.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.IdMunicipio = null;
                ObjEjecFiltros.MesDeclararion = this.CmbMeses.SelectedValue.ToString().Trim().Length > 0 ? this.CmbMeses.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.IdEstado = this.CmbEstadoLiquidacion.SelectedValue.ToString().Trim().Length > 0 ? this.CmbEstadoLiquidacion.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.AprobacionJefe = this.CmbEstadoAprobacion.SelectedValue.ToString().Trim().Length > 0 ? this.CmbEstadoAprobacion.SelectedValue.ToString().Trim() : null;
                ObjEjecFiltros.MostrarSeleccione = "NO";
                ObjEjecFiltros.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                string _MsgError = "";
                dtDatos = ObjEjecFiltros.GetEjecucionXLoteFiltroMunicipio(ref _MsgError);
                //--
                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        #region AQUI OBTENEMOS LOS DATOS DE LA DB
                        //--Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                        this.RadWindowManager1.Enabled = false;
                        this.RadWindowManager1.EnableAjaxSkinRendering = false;
                        this.RadWindowManager1.Visible = false;

                        this.ViewState["DtFiltros"] = dtDatos;
                        dtDatos.PrimaryKey = new DataColumn[] { dtDatos.Columns["id_registro, id_cliente, idformulario_impuesto, anio_gravable, id_dpto, nombre_dpto, codigo_dane, id_municipio, nombre_municipio, idcliente_establecimiento, periodicidad_impuesto, fecha_limite"] };
                        this.UpdatePanel1.Update();
                        this.PnlEstado.Visible = true;
                        this.RadGrid1.Rebind();
                        #endregion
                    }
                    else
                    {
                        #region AQUI VALIDAMOS SI EL DPTO TIENE MUNICIPIOS CONFIGURADOS EN EL CALENDARIO TRIBUTARIO
                        ObjEjecFiltros.TipoConsulta = 5;
                        _MsgError = "";
                        dtDatos = ObjEjecFiltros.GetEjecucionXLoteFiltroMunicipio(ref _MsgError);
                        if (dtDatos != null)
                        {
                            if (dtDatos.Rows.Count > 0)
                            {
                                #region AQUI OBTENEMOS LOS DATOS DE LA DB
                                //--Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                                this.RadWindowManager1.Enabled = false;
                                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                                this.RadWindowManager1.Visible = false;

                                this.ViewState["DtFiltros"] = dtDatos;
                                dtDatos.PrimaryKey = new DataColumn[] { dtDatos.Columns["id_registro, id_cliente, idformulario_impuesto, anio_gravable, id_dpto, nombre_dpto, codigo_dane, id_municipio, nombre_municipio, idcliente_establecimiento, periodicidad_impuesto, idmun_calendario_trib, fecha_limite"] };
                                this.UpdatePanel1.Update();
                                this.PnlEstado.Visible = true;
                                this.RadGrid1.Rebind();
                                #endregion
                            }
                            else
                            {
                                #region MOSTRAR MENSAJE DE USUARIO
                                this.PnlEstado.Visible = false;
                                //Mostramos el mensaje porque se produjo un error con la Trx.
                                this.RadWindowManager1.ReloadOnShow = true;
                                this.RadWindowManager1.DestroyOnClose = true;
                                this.RadWindowManager1.Windows.Clear();
                                this.RadWindowManager1.Enabled = true;
                                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                                this.RadWindowManager1.Visible = true;

                                RadWindow Ventana2 = new RadWindow();
                                Ventana2.Modal = true;
                                string _MsgMensaje2 = _MsgError.ToString().Trim().Length > 0 ? _MsgError.ToString().Trim() : "Señor usuario. No se encontraron municipios asociados al departamento. Por favor validar con soporte técnico !";
                                Ventana2.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje2;
                                Ventana2.ID = "RadWindow" + ObjUtils.GetRandom();
                                Ventana2.VisibleOnPageLoad = true;
                                Ventana2.Visible = true;
                                Ventana2.Height = Unit.Pixel(300);
                                Ventana2.Width = Unit.Pixel(550);
                                Ventana2.KeepInScreenBounds = true;
                                Ventana2.Title = "Mensaje del Sistema";
                                Ventana2.VisibleStatusbar = false;
                                Ventana2.Behaviors = WindowBehaviors.Close;
                                this.RadWindowManager1.Windows.Add(Ventana2);
                                this.RadWindowManager1 = null;
                                Ventana2 = null;
                                //--
                                this.ViewState["DtFiltros"] = dtDatos;
                                this.RadGrid1.Rebind();
                                #endregion
                            }
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

                            RadWindow Ventana2 = new RadWindow();
                            Ventana2.Modal = true;
                            string _MsgMensaje2 = _MsgError.ToString().Trim().Length > 0 ? _MsgError.ToString().Trim() : "Señor usuario. Ocurrio un Error al listar los municipios. Por favor validar con soporte técnico !";
                            Ventana2.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje2;
                            Ventana2.ID = "RadWindow" + ObjUtils.GetRandom();
                            Ventana2.VisibleOnPageLoad = true;
                            Ventana2.Visible = true;
                            Ventana2.Height = Unit.Pixel(330);
                            Ventana2.Width = Unit.Pixel(550);
                            Ventana2.KeepInScreenBounds = true;
                            Ventana2.Title = "Mensaje del Sistema";
                            Ventana2.VisibleStatusbar = false;
                            Ventana2.Behaviors = WindowBehaviors.Close;
                            this.RadWindowManager1.Windows.Add(Ventana2);
                            this.RadWindowManager1 = null;
                            Ventana2 = null;
                            //--
                            this.ViewState["DtFiltros"] = dtDatos;
                            this.RadGrid1.Rebind();
                            #endregion
                        }
                        #endregion
                    }
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
                    string _MsgMensaje = _MsgError.ToString().Trim().Length > 0 ? _MsgError.ToString().Trim() : "Señor usuario. Ocurrio un Error al listar los municipios. Por favor validar con soporte técnico !";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                    //--
                    this.ViewState["DtFiltros"] = dtDatos;
                    this.RadGrid1.Rebind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los municipios. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);

                //this.AplicarPermisos();
                this.ViewState["DtFiltros"] = null;
                this.ViewState["DataProcesar"] = "";

                #region AQUI OBTENEMOS LOS DATOS DEL FIRMANTE
                //--DEFINIR VARIABLES PARA OBTENER DATOS DEL FIRMANTE
                //--DATOS DE LOS FIRMANTES
                this.ViewState["DtFirmante1"] = null;
                this.ViewState["DtFirmante2"] = null;
                this.ViewState["IdFirmante1"] = "0";
                this.ViewState["IdFirmante2"] = "0";
                this.ViewState["NombreFirmante1"] = "";
                this.ViewState["NombreFirmante2"] = "";
                this.ViewState["TipoDocFirmante1"] = "";
                this.ViewState["TipoDocFirmante2"] = "";
                this.ViewState["DocFirmante1"] = "";
                this.ViewState["DocFirmante2"] = "";
                this.ViewState["TpFirmante1"] = "";
                this.ViewState["TpFirmante2"] = "";
                this.ViewState["IdRolFirmante1"] = "";
                this.ViewState["IdRolFirmante2"] = "";
                this.ViewState["ImagenFirma1"] = null;
                this.ViewState["ImagenFirma2"] = null;

                #region AQUI OBTENEMOS LOS DATOS DEL FIRMANTE
                //--AQUI OBTENEMOS LOS DATOS DEL FIRMANTE COMO REPRESENTANTE LEGAL
                #region OBTENER DATOS DEL FIRMANTE 1
                //--FIRMANTE UNO
                DataTable dtDatosFirm1 = new DataTable();
                ObjFirmante.TipoConsulta = 4;   //--CON ESTE TIPO DE CONSULTA SOLO OBTENEMOS DATOS DEL REP. LEGAL
                ObjFirmante.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjFirmante.IdFirmante = null;
                ObjFirmante.IdRol = 4;  //--ID ROL
                ObjFirmante.IdEstado = 1;
                ObjFirmante.MostrarSeleccione = "SI";
                ObjFirmante.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                //--
                dtDatosFirm1 = ObjFirmante.GetImagenFirma();

                if (dtDatosFirm1 != null)
                {
                    if (dtDatosFirm1.Rows.Count > 0)
                    {
                        #region OBTENER DATOS DEL FIRMANTE 1
                        this.ViewState["DtFirmante1"] = dtDatosFirm1;

                        if (dtDatosFirm1.Rows.Count > 1)
                        {
                            this.CmbFirmante1.Enabled = true;
                            this.CmbFirmante1.SelectedValue = "0";
                        }
                        else
                        {
                            this.CmbFirmante1.Enabled = false;
                        }
                        //--AQUI HABILITAMOS EL COMBO DE LOS 2do FIRMANTES
                        this.CmbFirmante1.DataSource = dtDatosFirm1;
                        this.CmbFirmante1.DataValueField = "id_firmante";
                        this.CmbFirmante1.DataTextField = "nombre_firmante";
                        this.CmbFirmante1.DataBind();
                        #endregion
                    }
                }
                #endregion

                //--AQUI OBTENEMOS LOS DATOS DEL FIRMANTE COMO REVISOR FISCAL
                #region OBTENER DATOS DEL FIRMANTE 2
                //--FIRMANTE UNO
                DataTable dtDatosFirm2 = new DataTable();
                ObjFirmante.TipoConsulta = 4;   //--CON ESTE TIPO DE CONSULTA SOLO OBTENEMOS DATOS DEL REP. LEGAL
                ObjFirmante.IdCliente = this.Session["IdCliente"].ToString().Trim();
                ObjFirmante.IdFirmante = null;
                ObjFirmante.IdRol = 5;  //--ID ROL REVISOR FISCAL
                ObjFirmante.IdEstado = 1;
                ObjFirmante.MostrarSeleccione = "SI";
                ObjFirmante.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                //--
                dtDatosFirm2 = ObjFirmante.GetImagenFirma();

                if (dtDatosFirm2 != null)
                {
                    if (dtDatosFirm2.Rows.Count > 0)
                    {
                        #region OBTENER DATOS DEL FIRMANTE 2
                        this.ViewState["DtFirmante2"] = dtDatosFirm2;

                        if (dtDatosFirm2.Rows.Count > 1)
                        {
                            //this.Validator6.Enabled = true;
                            this.CmbFirmante2.Enabled = true;
                            this.CmbFirmante2.SelectedValue = "0";
                        }
                        else
                        {
                            //this.Validator6.Enabled = false;
                            this.CmbFirmante2.Enabled = false;
                        }

                        //--AQUI HABILITAMOS EL COMBO DE LOS 2do FIRMANTES
                        this.CmbFirmante2.DataSource = dtDatosFirm2;
                        this.CmbFirmante2.DataValueField = "id_firmante";
                        this.CmbFirmante2.DataTextField = "nombre_firmante";
                        this.CmbFirmante2.DataBind();
                        #endregion
                    }
                }
                #endregion
                //--
                #endregion
                //--
                #endregion

                //Llenar los combox
                this.LstAnioGravable();
                this.LstTipoFiltros();
                this.LstMeses();
                this.LstEstadoImpuesto();
                this.LstEstadosAprobacion();
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
        }

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

        #region DEFINICION DE EVENTOS CONTROLES DEL FORM
        protected void CmbAnioGravable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0)
            {
                this.CmbTipoFiltro.Enabled = true;
                this.CmbTipoFiltro.SelectedValue = "";
                this.CmbTipoFiltro.Focus();
            }
            else
            {
                this.CmbTipoFiltro.Enabled = false;
                this.CmbTipoFiltro.SelectedValue = "";
            }
        }

        protected void CmbTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;
            //--
            if (this.CmbTipoFiltro.SelectedValue.ToString().Trim().Length > 0)
            {
                this.CmbTipoImpuesto.Enabled = true;
                this.CmbTipoImpuesto.SelectedValue = "";
                this.CmbDepartamento.SelectedValue = "";
                //--
                this.LstTipoImpuestosFiltro();
                this.CmbTipoImpuesto.Focus();
            }
            else
            {
                this.CmbTipoImpuesto.Enabled = false;
                this.CmbTipoImpuesto.SelectedValue = "";
                this.CmbDepartamento.Enabled = false;
                this.CmbDepartamento.SelectedValue = "";
            }
        }

        protected void CmbTipoImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;
            //--
            if (this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0)
            {
                this.CmbDepartamento.Enabled = true;
                this.CmbDepartamento.SelectedValue = "";
                //--
                this.LstDptosFiltro();
                this.CmbDepartamento.Focus();
            }
            else
            {
                this.CmbDepartamento.Enabled = false;
                this.CmbDepartamento.SelectedValue = "";
            }
        }

        protected void CmbDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetConsultarMunicipios();
        }

        protected void CmbMeses_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetConsultarMunicipios();
        }

        protected void CmbEstadoLiquidacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetConsultarMunicipios();
        }

        protected void CmbEstadoAprobacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetConsultarMunicipios();
        }

        protected void BtnConsultar_Click(object sender, ImageClickEventArgs e)
        {
            this.GetConsultarMunicipios();
        }

        protected void RbEstadoDeclaracion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;
            //--
            if (this.RbEstadoDeclaracion.SelectedValue.ToString().Trim().Length > 0)
            {
                //--AQUI VALIDAMOS SI EL ESTADO ES SELECCIONADO ES DEFINITIVO
                if (this.RbEstadoDeclaracion.SelectedValue.ToString().Trim().Equals("3"))
                {
                    this.Validador7.Enabled = true;
                    this.Validador8.Enabled = true;
                }
                else
                {
                    this.Validador7.Enabled = true;
                    this.Validador8.Enabled = false;
                }
                //--HABILITAR CONTROLES
                this.CmbFirmante1.Enabled = true;
                this.CmbFirmante2.Enabled = true;
                this.BtnEjecutar.Enabled = true;
                this.TxtEmailsConfirmacion.Focus();
            }
            else
            {
                this.BtnEjecutar.Enabled = false;
                this.TxtEmailsConfirmacion.Focus();
            }
        }

        protected void CmbFirmante1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                int _IdFirmante = Int32.Parse(this.CmbFirmante1.SelectedValue.ToString().Trim());
                if (_IdFirmante > 0)
                {
                    #region AQUI OBTENEMOS LOS DATOS DEL FIRMANTE 1
                    //--
                    DataTable dtDatos = new DataTable();
                    dtDatos = (DataTable)this.ViewState["DtFirmante1"];
                    DataRow[] dataRows = dtDatos.Select("id_firmante = " + _IdFirmante);
                    if (dataRows.Length == 1)
                    {
                        #region AQUI OBTENEMOS LOS DATOS DEL FIRMANTE
                        //this.UpdatePanel1.Update();
                        this.ViewState["IdFirmante1"] = dataRows[0]["id_firmante"].ToString().Trim();
                        this.ViewState["NombreFirmante1"] = dataRows[0]["nombre_firmante"].ToString().Trim();
                        this.ViewState["TipoDocFirmante1"] = dataRows[0]["idtipo_identificacion"].ToString().Trim();
                        this.ViewState["DocFirmante1"] = dataRows[0]["numero_documento"].ToString().Trim();
                        this.ViewState["TpFirmante1"] = dataRows[0]["numero_tp"].ToString().Trim();
                        this.ViewState["IdRolFirmante1"] = dataRows[0]["id_rol"].ToString().Trim();

                        if (dataRows[0]["imagen_firma"].ToString().Trim().Length > 0)
                        {
                            Byte[] ImagenByte = (Byte[])dataRows[0]["imagen_firma"];
                            this.ViewState["ImagenFirma1"] = ImagenByte;
                        }
                        else
                        {
                            this.ViewState["ImagenFirma1"] = null;
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region MOSTRAR MENSAJE DE USUARIO
                    //--DATOS DE LOS FIRMANTES
                    this.ViewState["DtFirmante1"] = null;
                    this.ViewState["IdFirmante1"] = "0";
                    this.ViewState["IdRolFirmante1"] = "";
                    this.ViewState["ImagenFirma1"] = null;

                    //Mostramos el mensaje porque se produjo un error con la Trx.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string _MsgMensaje = "Señor usuario, debe seleccionar un firmante de la lista !";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                    Ventana.ID = "RadWindow2" + ObjUtils.GetRandom();
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
                //--DATOS DE LOS FIRMANTES
                this.ViewState["DtFirmante1"] = null;
                this.ViewState["IdFirmante1"] = "0";
                this.ViewState["IdRolFirmante1"] = "";
                this.ViewState["ImagenFirma1"] = null;

                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al obtener los datos del firmante 1. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2" + ObjUtils.GetRandom();
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

        protected void CmbFirmante2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                int _IdFirmante = Int32.Parse(this.CmbFirmante2.SelectedValue.ToString().Trim());
                if (_IdFirmante > 0)
                {
                    #region AQUI OBTENEMOS LOS DATOS DEL FIRMANTE 2
                    //--
                    DataTable dtDatos = new DataTable();
                    dtDatos = (DataTable)this.ViewState["DtFirmante2"];
                    DataRow[] dataRows = dtDatos.Select("id_firmante = " + _IdFirmante);
                    if (dataRows.Length == 1)
                    {
                        #region AQUI OBTENEMOS LOS DATOS DEL FIRMANTE
                        //this.UpdatePanel1.Update();
                        this.ViewState["IdFirmante2"] = dataRows[0]["id_firmante"].ToString().Trim();
                        this.ViewState["NombreFirmante2"] = dataRows[0]["nombre_firmante"].ToString().Trim();
                        this.ViewState["TipoDocFirmante2"] = dataRows[0]["idtipo_identificacion"].ToString().Trim();
                        this.ViewState["DocFirmante2"] = dataRows[0]["numero_documento"].ToString().Trim();
                        this.ViewState["TpFirmante2"] = dataRows[0]["numero_tp"].ToString().Trim();
                        this.ViewState["IdRolFirmante2"] = dataRows[0]["id_rol"].ToString().Trim();

                        if (dataRows[0]["imagen_firma"].ToString().Trim().Length > 0)
                        {
                            Byte[] ImagenByte = (Byte[])dataRows[0]["imagen_firma"];
                            this.ViewState["ImagenFirma2"] = ImagenByte;
                        }
                        else
                        {
                            this.ViewState["ImagenFirma2"] = null;
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region MOSTRAR MENSAJE DE USUARIO
                    //--DATOS DE LOS FIRMANTES
                    this.ViewState["DtFirmante2"] = null;
                    this.ViewState["IdFirmante2"] = "0";
                    this.ViewState["IdRolFirmante2"] = "";
                    this.ViewState["ImagenFirma2"] = null;

                    //Mostramos el mensaje porque se produjo un error con la Trx.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string _MsgMensaje = "Señor usuario, debe seleccionar un firmante de la lista !";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                    Ventana.ID = "RadWindow2" + ObjUtils.GetRandom();
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
                //--DATOS DE LOS FIRMANTES
                this.ViewState["DtFirmante2"] = null;
                this.ViewState["IdFirmante2"] = "0";
                this.ViewState["IdRolFirmante2"] = "";
                this.ViewState["ImagenFirma2"] = null;

                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al obtener los datos del firmante 1. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2" + ObjUtils.GetRandom();
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
        #endregion

        #region DEFINICION DE METODOS DEL GRID
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                DataTable dtDatos = new DataTable();
                dtDatos = (DataTable)this.ViewState["DtFiltros"];

                if (dtDatos != null)
                {
                    this.RadGrid1.DataSource = dtDatos; //--this.FuenteDatos;
                    this.RadGrid1.DataMember = "DtEjecucionXLoteFiltro";
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
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del tipo de moneda. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                    #region VER INFORMACION DEL BORRADOR DEL FORMULARIO DEL ICA
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdRegistro = Int32.Parse(item.GetDataKeyValue("id_registro").ToString().Trim());
                    int _AnioGravable = Int32.Parse(item.GetDataKeyValue("anio_gravable").ToString().Trim());
                    int _IdFormularioImpuesto = Int32.Parse(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdDepartamento = Int32.Parse(item.GetDataKeyValue("id_dpto").ToString().Trim());
                    int _IdLiquidacionLote = item.GetDataKeyValue("idliquidacion_lote").ToString().Trim().Length > 0 ? Int32.Parse(item.GetDataKeyValue("idliquidacion_lote").ToString().Trim()) : 0;
                    string _TipoImpuesto = this.CmbTipoImpuesto.SelectedItem.Text.ToString().Trim(); //--item["tipo_impuesto"].Text.ToString().Trim();
                    string _NombreMunicipio = item["nombre_municipio"].Text.ToString().Trim();

                    if (_IdLiquidacionLote > 0)
                    {
                        #region MOSTRAR EL FORM DE APROBAR O RECAHZAR
                        this.UpdatePanel1.Update();
                        //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;
                        Ventana.Modal = true;

                        //--string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                        Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmVerInfoValidacion.aspx?IdLiquidacionLote=" + _IdLiquidacionLote +
                        "&AnioGravable=" + _AnioGravable + "&IdFormImpuesto=" + _IdFormularioImpuesto + "&IdDepartamento=" + _IdDepartamento;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(500);
                        Ventana.Width = Unit.Pixel(1000);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Ver Info Detallada Validación Id: " + _IdLiquidacionLote + ", Tipo Impuesto: " + _TipoImpuesto + ", Municipio: " + _NombreMunicipio;
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
                        this.UpdatePanel1.Update();
                        //MOSTRAR LA VENTANA DEL POPUP
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgMensaje = "Señor usuario, esta declaración aun no ha sido procesada por el ANALISTA o validada por el jefe !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                    #endregion
                }
                else if (e.CommandName == "BtnVerFormulario")
                {
                    #region VISUALIZAR EL FORM DEL IMPUESTO
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdLiquidacionLote = item.GetDataKeyValue("idliquidacion_lote").ToString().Trim().Length > 0 ? Int32.Parse(item.GetDataKeyValue("idliquidacion_lote").ToString().Trim()) : 0;
                    int _IdFormularioImpuesto = Int32.Parse(item.GetDataKeyValue("idformulario_impuesto").ToString().Trim());
                    int _IdMunicipio = Int32.Parse(item.GetDataKeyValue("id_municipio").ToString().Trim());
                    int _IdCliente = Int32.Parse(item.GetDataKeyValue("id_cliente").ToString().Trim());
                    int _IdClienteEstablecimiento = Int32.Parse(item.GetDataKeyValue("idcliente_establecimiento").ToString().Trim());
                    int _PeriodoImpuesto = Int32.Parse(item.GetDataKeyValue("idperiodicidad_pago").ToString().Trim());
                    int _CodigoPeriodicidad = Int32.Parse(item.GetDataKeyValue("codigo_periodicidad").ToString().Trim());
                    string _CodigoDane = item["codigo_dane"].Text.ToString().Trim();
                    //int _IdEstado = Int32.Parse(item.GetDataKeyValue("idestado_liquidacion").ToString().Trim());
                    string _EstadoLiquidacion = item["estado_declaracion"].Text.ToString().Trim();

                    if (_IdLiquidacionLote > 0)
                    {
                        #region MOSTRAR EL IMPUESTO EN PDF Y PARA IMPRIMIR
                        this.UpdatePanel1.Update();
                        //--AQUI HABILITAMOS EL FORM DEL IMPUESTO
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;
                        Ventana.Modal = true;

                        string _TipoImpuesto = "";
                        switch (_IdFormularioImpuesto)
                        {
                            case 1:
                                _TipoImpuesto = "ICA\\POR_LOTE";
                                break;
                            case 2:
                                _TipoImpuesto = "AUTORETENCION_ICA\\POR_LOTE";
                                break;
                            default:
                                break;
                        }

                        string _TipoLiquidacion = _EstadoLiquidacion.ToString().Equals("PRELIMINAR") ? "BORRADOR" : "DEFINITIVO";
                        string _TipoEjecucion = "POR_LOTE";
                        Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmVerImpuesto.aspx?IdMunicipio=" + _IdMunicipio + "&IdFormImpuesto=" + _IdFormularioImpuesto + "&CodigoDane=" + _CodigoDane +
                            "&CodigoPeriodicidad=" + _CodigoPeriodicidad.ToString().PadLeft(2, '0') + "&TipoImpuesto=" + _TipoImpuesto + "&TipoLiquidacion=" + _TipoLiquidacion + "&TipoEjecucion=" + _TipoEjecucion;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(670);
                        Ventana.Width = Unit.Pixel(1100);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Visualizar Liquidación Impuesto";
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
                        this.UpdatePanel1.Update();
                        //MOSTRAR LA VENTANA DEL POPUP
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgMensaje = "Señor usuario, esta declaración aun no ha sido procesada por el ANALISTA o validada por el jefe !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                    //--
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
                this.UpdatePanel1.Update();
                //MOSTRAR LA VENTANA DEL POPUP
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
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                string _MsgError = "Error con el evento RadGrid1_PageIndexChanged del tipo de moneda. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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

        protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GridDataItem selectedItem = (GridDataItem)RadGrid1.SelectedItems[0];
            //string value = selectedItem["id_registro"].Text;
            //--
            string _JsonResponse = "";
            foreach (GridDataItem item in RadGrid1.MasterTableView.Items)
            {
                if (item.Selected == true)
                {
                    string _DataItem = item.KeyValues.ToString().Trim();
                    if (_JsonResponse.ToString().Trim().Length > 0)
                    {
                        _JsonResponse = _JsonResponse + "," + _DataItem;
                    }
                    else
                    {
                        _JsonResponse = _DataItem;
                    }
                }
                //CheckBox chk = (CheckBox)i["RowSelect"].Controls[0];
            }

            //--OBTENER LA DATA A PROCESAR
            this.ViewState["DataProcesar"] = @"[" + _JsonResponse + @"]";
            _log.Warn("DATA PROCESAR IMPUESTO => " + this.ViewState["DataProcesar"].ToString().Trim());
        }
        #endregion

        protected void BtnEjecutar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ViewState["DataProcesar"].ToString().Trim().Length > 0)
                {
                    #region AQUI OBTENEMOS LO VALORES A ENVIAR AL SERIVICIO
                    LiquidarImpuesto_Req objData = new LiquidarImpuesto_Req();
                    objData.estado_liquidacion = this.RbEstadoDeclaracion.SelectedValue.ToString().Trim().Length > 0 ? Int32.Parse(this.RbEstadoDeclaracion.SelectedValue.ToString().Trim()) : -1;
                    objData.idejecucion_lote = Int32.Parse(this.CmbTipoFiltro.SelectedValue.ToString().Trim());
                    objData.tipo_impuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim().Length > 0 ? Int32.Parse(this.CmbTipoImpuesto.SelectedValue.ToString().Trim()) : 0;
                    objData.data_procesar = this.ViewState["DataProcesar"].ToString().Trim();
                    objData.emails_confirmar = this.TxtEmailsConfirmacion.Text.ToString().Trim().ToUpper();
                    objData.id_usuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    objData.nombre_usuario = this.Session["NombreCompletoUsuario"].ToString().Trim();
                    //--AQUI VALIDAMOS EL ESTADO DE LA LIQUIDACION
                    if (objData.estado_liquidacion == 2)
                    {
                        #region VALIDAMOS SI HAY FIRMANTES SELECCIONADOS
                        if (!(Int32.Parse(this.ViewState["IdFirmante1"].ToString().Trim()) > 0))
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
                            string _Mensaje = "Señor usuario, debe seleccionar el REPRESENTANTE LEGAL de la lista para generar liquidación preliminar !";
                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
                            Ventana.ID = "RadWindow2" + ObjUtils.GetRandom();
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
                            return;
                            #endregion
                        }
                        #endregion
                    }
                    else if (objData.estado_liquidacion == 3)
                    {
                        #region VALIDAMOS SI HAY FIRMANTES SELECCIONADOS
                        if (!(Int32.Parse(this.ViewState["IdFirmante1"].ToString().Trim()) > 0 &&
                            Int32.Parse(this.ViewState["IdFirmante2"].ToString().Trim()) > 0))
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
                            string _Mensaje = "Señor usuario, debe seleccionar un REPRESENTANTE LEGAL, CONTADOR o REVISOR FISCAR de la lista para generar liquidación definitivas !";
                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
                            Ventana.ID = "RadWindow2" + ObjUtils.GetRandom();
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
                            return;
                            #endregion
                        }
                        #endregion
                    }
                    //--OBTENEMOS LOS DATOS DEL FIRMANTE 1
                    firmante objFirmante = new firmante();
                    objFirmante.id_firmante = Int32.Parse(this.ViewState["IdFirmante1"].ToString().Trim()) > 0 ? this.ViewState["IdFirmante1"].ToString().Trim() : null;
                    objFirmante.nombre_firmante = this.ViewState["NombreFirmante1"].ToString().Trim().Length > 0 ? this.ViewState["NombreFirmante1"].ToString().Trim() : "";
                    objFirmante.tipo_documento = this.ViewState["TipoDocFirmante1"].ToString().Trim().Length > 0 ? this.ViewState["TipoDocFirmante1"].ToString().Trim() : "";
                    objFirmante.numero_documento = this.ViewState["DocFirmante1"].ToString().Trim().Length > 0 ? this.ViewState["DocFirmante1"].ToString().Trim() : "";
                    objFirmante.numero_tp = this.ViewState["TpFirmante1"].ToString().Trim().Length > 0 ? this.ViewState["TpFirmante1"].ToString().Trim() : "";
                    objFirmante.id_rol = this.ViewState["IdRolFirmante1"].ToString().Trim().Length > 0 ? Int32.Parse(this.ViewState["IdRolFirmante1"].ToString().Trim()) : 0;
                    objFirmante.imagen_firma = (byte[])this.ViewState["ImagenFirma1"];
                    objData.info_firmante1 = objFirmante;

                    //--OBTENEMOS LOS DATOS DEL FIRMANTE 2
                    objFirmante = new firmante();
                    objFirmante.id_firmante = Int32.Parse(this.ViewState["IdFirmante2"].ToString().Trim()) > 0 ? this.ViewState["IdFirmante2"].ToString().Trim() : null;
                    objFirmante.nombre_firmante = this.ViewState["NombreFirmante2"].ToString().Trim().Length > 0 ? this.ViewState["NombreFirmante2"].ToString().Trim() : "";
                    objFirmante.tipo_documento = this.ViewState["TipoDocFirmante2"].ToString().Trim().Length > 0 ? this.ViewState["TipoDocFirmante2"].ToString().Trim() : "";
                    objFirmante.numero_documento = this.ViewState["DocFirmante2"].ToString().Trim().Length > 0 ? this.ViewState["DocFirmante2"].ToString().Trim() : "";
                    objFirmante.numero_tp = this.ViewState["TpFirmante2"].ToString().Trim().Length > 0 ? this.ViewState["TpFirmante2"].ToString().Trim() : "";
                    objFirmante.id_rol = this.ViewState["IdRolFirmante2"].ToString().Trim().Length > 0 ? Int32.Parse(this.ViewState["IdRolFirmante2"].ToString().Trim()) : 0;
                    objFirmante.imagen_firma = (byte[])this.ViewState["ImagenFirma2"];
                    objData.info_firmante2 = objFirmante;
                    #endregion

                    string _MsgError = "";
                    if (objProceso.GetProcesoLiquidacion(objData, ref _MsgError))
                    {
                        #region MOSTRAR MENSAJE DE USUARIO
                        string[] _ArrayData = _MsgError.ToString().Trim().Split('|');
                        //Mostramos el mensaje porque se produjo un error con la Trx.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _ArrayData[0].ToString().Trim() + " - " + _ArrayData[1].ToString().Trim();
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                        string[] _ArrayData = _MsgError.ToString().Trim().Split('|');
                        //Mostramos el mensaje porque se produjo un error con la Trx.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _ArrayData[0].ToString().Trim() + " - " + _ArrayData[1].ToString().Trim();
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                    string _Mensaje = "Señor usuario, no hay item seleccionado para realizar el proceso por lote. Por favor validar los filtros !";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al realizar la Ejecución por Lote. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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