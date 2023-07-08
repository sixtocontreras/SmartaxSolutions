using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Telerik.Web.UI;
using Smartax.Web.Application.Clases.Seguridad;
using System.Configuration;

namespace Smartax.Web.Application.Controles.General
{
    public partial class CtrlModulos : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();
        ModuloSmartax ObjModulos = new ModuloSmartax();
        Utilidades objUtils = new Utilidades();

        private void AplicarPermisos()
        {
            try
            {
                ObjModulos.IdRol = Int32.Parse(Session["IdRol"].ToString().Trim());
                ObjModulos.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                ObjModulos.DevolverModulos();
                //--
                #region MODULOS DE PLANEACION FISCAL
                if (!ObjModulos.PlaneacionFiscal)
                {
                    this.ImgPlaneacionFiscal.Visible = false;
                }

                if (!ObjModulos.CalendarioTributario)
                {
                    this.ImgCalendarioTributario.Visible = false;
                }

                if (!ObjModulos.TarifasExcesivas)
                {
                    this.ImgTarifasExcesivas.Visible = false;
                }
                #endregion

                #region MODULOS DE INFORMACION TRIBUTARIA
                if (!ObjModulos.InformacionTributaria)
                {
                    this.ImgInfoTributaria.Visible = false;
                }

                if (!ObjModulos.HojaTrabajo)
                {
                    this.ImgModLiquidacionIca.Visible = false;
                }

                if (!ObjModulos.DeclaracionDefinitiva)
                {
                    this.ImgModBorradorAutoIca.Visible = false;
                }

                if (!ObjModulos.EjecucionPorLote)
                {
                    this.ImgEjecucionPorLotes.Visible = false;
                }

                if (!ObjModulos.ValidacionLiqLote)
                {
                    this.ImgValidarLiqLotes.Visible = false;
                }

                if (!ObjModulos.ConsultaLiquidacion)
                {
                    this.ImgConsultarImpLiquidado.Visible = false;
                }

                if (!ObjModulos.FichaTecnica)
                {
                    this.ImgFichaTecnica.Visible = false;
                }
                #endregion

                #region MODULOS DE FORMATOS SFC
                if (!ObjModulos.FormatosSfc)
                {
                    this.imgFormatosSFC.Visible = false;
                }

                if (!ObjModulos.GeneracionDatos)
                {
                    this.btnProcess.Visible = false;
                }

                if (!ObjModulos.GenerarF321)
                {
                    this.btn321.Visible = false;
                }

                if (!ObjModulos.GenerarF525)
                {
                    this.btn525.Visible = false;
                }

                if (!ObjModulos.GenerarArchivoPlano)
                {
                    this.btnFiles.Visible = false;
                }
                #endregion

                #region MODULOS DE NORMATIVIDAD
                if (!ObjModulos.Normatividad)
                {
                    this.ImageNormativa.Visible = false;
                }

                if (!ObjModulos.CargarNormatividad)
                {
                    this.ImgModuloNormatividad.Visible = false;
                }

                if (!ObjModulos.ConsultarNormatividad)
                {
                    this.ImgModuloNormatividad_consulta.Visible = false;
                }

                if (!ObjModulos.CargaMasivaDoc)
                {
                    this.ImgCarga_masiva.Visible = false;
                }
                #endregion

                #region MODULOS DE CONTROL DE ACTIVIDADES
                if (!ObjModulos.ControlActividades)
                {
                    this.ImgMisActividades.Visible = false;
                }

                if (!ObjModulos.MisActividades)
                {
                    this.ImgMonitoreoAct.Visible = false;
                }

                if (!ObjModulos.MonitoreoActividades)
                {
                    this.ImgEstadisticaAct.Visible = false;
                }

                if (!ObjModulos.EstadisticaActividades)
                {
                    this.ImgEstadisticaAct.Visible = false;
                }

                if (!ObjModulos.EstadisticaLiquidacion)
                {
                    this.ImgEstadisticaLiq.Visible = false;
                }
                #endregion

                if (ObjModulos.PlaneacionFiscal == false && ObjModulos.CalendarioTributario == false &&
                    ObjModulos.TarifasExcesivas == false && ObjModulos.InformacionTributaria == false &&
                    ObjModulos.HojaTrabajo == false && ObjModulos.DeclaracionDefinitiva == false &&
                    ObjModulos.ConsultaLiquidacion == false && ObjModulos.FichaTecnica == false)
                {
                    this.PnlTrxGeneral.Visible = false;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al Aplicar los permisos de los modulos. Motivo: " + ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                if (this.Session["IdRol"] != null)
                {
                    this.AplicarPermisos();
                }
                else
                {
                    this.PnlTrxGeneral.Visible = false;
                }
            }
        }

        #region DEFINICION EVENTOS BOTONES DEL PANEL GENERAL O PRINCIPAL
        protected void ImgPlaneacionFiscal_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlPlaneacionFiscal.Visible = true;
        }

        protected void ImgInfoTributaria_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlLiquidacion.Visible = true;
        }

        protected void ImgControlActividades_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlActividades.Visible = true;
        }
        protected void imgFormatosSFC_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlFormatosSFC.Visible = true;
        }
        #endregion

        #region DEFINICION EVENTOS BOTONES DEL PANEL PLANEACION FISCAL
        protected void ImgCalendarioTributario_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO CALENDARIO TRIBUTARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/PlaneacionFiscal/FrmVerCalendarioMeses.aspx?TipoConsulta=1" + "&PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(660);
                Ventana.Width = Unit.Pixel(1100);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Ver Calendario Tributario";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al listar el calendario tributario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgTarifasExcesivas_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO CALENDARIO TRIBUTARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/PlaneacionFiscal/FrmVerTarifasExcesivas.aspx?TipoConsulta=2" + "&PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(650);
                Ventana.Width = Unit.Pixel(1350);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Ver Tarifas Excesivas";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al listar las tarifas excesivas. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgRegresarPlaneacion_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = true;
            this.PnlPlaneacionFiscal.Visible = false;
        }
        #endregion

        #region DEFINICION EVENTOS BOTONES LIQUIDACION DE IMPUESTO
        //--
        #region MODULO LIQUIDACION IMPUESTO DEL ICA
        protected void ImgModLiquidacionIca_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlLiquidacionIca.Visible = true;
            this.PnlLiquidacion.Visible = false;
        }

        protected void ImgLiqBorradorIca_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO LIQUIDACION DEL IMPUESTO ICA
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmLiquidarBorradorIca.aspx?IdFormImpuesto=" + FixedData.IDFORM_IMPUESTO_ICA + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(700);
                Ventana.Width = Unit.Pixel(1260);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Borrador del Impuesto ICA";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de liquidacion de impuesto. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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
        
        protected void ImgLiqImpuestoDefinitivo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO LIQUIDACION DEINITIVA DEL IMPUESTO ICA
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmLiquidarDefinitivoIca.aspx?IdFormImpuesto=" + FixedData.IDFORM_IMPUESTO_ICA + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(700);
                Ventana.Width = Unit.Pixel(1260);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Definitivo del Impuesto ICA";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de liquidacion de impuesto. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgRegresarLiqIca_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlLiquidacion.Visible = true;
            this.PnlLiquidacionIca.Visible = false;
        }
        #endregion

        //--
        #region MODULO LIQUIDACION IMPUESTO DE AUTORETENCION DEL ICA
        protected void ImgModBorradorAutoIca_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlLiquidacionAutoIca.Visible = true;
            this.PnlLiquidacion.Visible = false;
        }

        protected void ImgLiqBorradorAutoIca_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO LIQUIDACION DEL IMPUESTO ICA
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmLiquidarBorradorAutoIca.aspx?IdFormImpuesto=" + FixedData.IDFORM_IMPUESTO_AUTORETE_ICA + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(700);
                Ventana.Width = Unit.Pixel(1300);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Borrador del Impuesto Autoretención del ICA";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de liquidacion de impuesto. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgLiqDefinitivoAutoIca_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO LIQUIDACION DEL IMPUESTO ICA
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmLiquidarDefinitivoAutoIca.aspx?IdFormImpuesto=" + FixedData.IDFORM_IMPUESTO_AUTORETE_ICA + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(700);
                Ventana.Width = Unit.Pixel(1300);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Borrador del Impuesto Autoretención del ICA";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de liquidacion de impuesto. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgRegresarLiqAutoIca_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlLiquidacion.Visible = true;
            this.PnlLiquidacionAutoIca.Visible = false;
        }
        #endregion

        protected void ImgEjecucionPorLotes_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO LIQUIDACION DEINITIVA DEL IMPUESTO ICA
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmEjecucionPorLotes.aspx?IdFormImpuesto=" + FixedData.IDFORM_IMPUESTO_ICA + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(650);
                Ventana.Width = Unit.Pixel(1200);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Realizar Ejecución por Lotes de Impuestos";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de la ejecución por Lotes del Ica. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgProcesoContabilizacion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO COMPROBANTE DE CONTABILIZACION
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmProcesoContabilizacion.aspx?IdFormImpuesto=" + FixedData.IDFORM_IMPUESTO_ICA + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(650);
                Ventana.Width = Unit.Pixel(1200);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Realizar Proceso Comprobante de Contabilización";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario del proceso de contabilización. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgValidarLiqLotes_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO PARA VALIDACION DE LIQUIDACION DE IMPUESTOS POR LOTE
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmValidarLiquidacionesPorLote.aspx?TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(680);
                Ventana.Width = Unit.Pixel(1260);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Validar Liquidación de Impuestos por Lote";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de la ejecución por Lotes del Ica. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgFichaTecnica_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.Redirect("/Modulos/Administracion/FrmFichaTecnica.aspx");
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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgConsultarImpLiquidado_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.Redirect("/Modulos/Administracion/FrmConsultaLiquidacionIca.aspx?IdFormImpuesto=" + FixedData.IDFORM_IMPUESTO_ICA);
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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgRegresarModLiquidacion_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = true;
            this.PnlLiquidacion.Visible = false;
        }
        #endregion

        #region DEFINICION EVENTOS BOTONES CONTROL DE ACTIVIDADES
        protected void ImgMisActividades_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO DE MIS ACTIVIDADES
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/ControlActividades/FrmMisActividades.aspx?IdFormImpuesto=" + FixedData.IDFORM_IMPUESTO_ICA + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(550);
                Ventana.Width = Unit.Pixel(1150);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Ver Mis Actividades";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de liquidacion de impuesto. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgMonitoreoAct_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO DE MONITOREO DE ACTIVIDADES
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/ControlActividades/FrmMonitoreoActividades.aspx?IdFormImpuesto=" + FixedData.IDFORM_IMPUESTO_ICA + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(700);
                Ventana.Width = Unit.Pixel(1260);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Monitoreo de Actividades";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de liquidacion de impuesto. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgEstadisticaAct_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO ESTADISTICA DE ACTIVIDADES
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/ControlActividades/FrmEstadisticaActividades.aspx?IdFormImpuesto=" + FixedData.IDFORM_IMPUESTO_ICA + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(700);
                Ventana.Width = Unit.Pixel(1300);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Estadistica de Actividades";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de liquidacion de impuesto. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgEstadisticaLiq_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO ESTADISTICAS DE LIQUIDACIONES
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/ControlActividades/FrmEstadisticaLiqImpuesto.aspx?IdFormImpuesto=" + FixedData.IDFORM_IMPUESTO_ICA + "&TipoProceso=" + _TipoProceso + "&PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(600);
                Ventana.Width = Unit.Pixel(1200);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Liquidación de Impuestos";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de liquidacion de impuesto. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgRegresarActiv_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = true;
            this.PnlActividades.Visible = false;
        }
        #endregion
        
        #region DEFINICION METODOS GENERACION FORMATOS SFC

        protected void ImgRegresarFormatos_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = true;
            this.PnlFormatosSFC.Visible = false;
        }
        protected void ImgGenerarProceso_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;
                
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/Formatos/FrmGeneracionFormatos.aspx?PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(500);
                Ventana.Width = Unit.Pixel(1000);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Ejecutar Formatos 321 - 525";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de generacion F-321/525. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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
        protected void ImgDescarga321_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/Formatos/FrmDescargar321.aspx?PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(500);
                Ventana.Width = Unit.Pixel(1000);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Descargar Formato 321";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de Descargar Formato 321. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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
        protected void ImgDescarga525_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/Formatos/FrmDescargar525.aspx?PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(500);
                Ventana.Width = Unit.Pixel(1000);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Descargar Formato 525";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de Descargar Formato 525. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgDescargaPlano_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/Formatos/FrmDescargarPlano.aspx?PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(500);
                Ventana.Width = Unit.Pixel(1000);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Descargar Archivo Plano";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de Descargar Formato 525. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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
        #endregion

        #region DEFINICION EVENTOS ALUMBRADO PUBLICO
        protected void imgAlumbradoPublico_Click(object sender, ImageClickEventArgs e)
        {
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlLiquidacionAlumbrado.Visible = true;
            this.PnlLiquidacion.Visible = false;

        }

        protected void imgHojaAlumbrado_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO LIQUIDACION DEL IMPUESTO ICA
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmLiquidacionAlumbrado.aspx?PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(700);
                Ventana.Width = Unit.Pixel(1260);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "LIQUIDACIÓN DE ALUMBRADO PÚBLICO";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de liquidacion de impuesto. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgRegresarLiqAlum_Click(object sender, ImageClickEventArgs e)
        {
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlLiquidacion.Visible = true;
            this.PnlLiquidacionAlumbrado.Visible = false;
        }
        #endregion

        protected void imgIca_Click(object sender, ImageClickEventArgs e)
        {
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.pnlReteIca.Visible = true;
            this.PnlLiquidacion.Visible = false;

        }

        protected void imgReteIca_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO LIQUIDACION DEL IMPUESTO ICA
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmRetencionICA.aspx?PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(700);
                Ventana.Width = Unit.Pixel(1260);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "LIQUIDACIÓN DE RETENCION DE INDUSTRIA Y COMERCIO";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de Rete ICA. Motivo: " + ex.ToString();
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
                #endregion
            }
        }

        protected void imgAtras_Click(object sender, ImageClickEventArgs e)
        {
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.pnlReteIca.Visible = false;
            this.PnlLiquidacion.Visible = true;

        }

        protected void imgDefinitivaIca_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR FORMULARIO LIQUIDACION DEINITIVA DEL IMPUESTO ICA
                //--AQUI HABILITAMOS EL OBJETO PARA MOSTRAR EL FORMULARIO
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                Ventana.Modal = true;

                string _TipoProceso = "1";
                string _PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                Ventana.NavigateUrl = "/Controles/Modulos/LiquidacionImpuestos/FrmRetencionICADefinitivo.aspx?PathUrl=" + _PathUrl;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(700);
                Ventana.Width = Unit.Pixel(1260);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Definitivo del Impuesto ICA";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
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
                string _MsgMensaje = "Error al mostrar el formulario de liquidacion de impuesto. Motivo: " + ex.ToString();
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


        #region DEFINICION DE METODOS SOBRE NORMATIVIDAD
        protected void ImgControlNormatividad_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlPlanormatividad.Visible = true;
        }

        protected void ImgRegresarNprmatividad_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = true;
            this.PnlPlanormatividad.Visible = false;
        }

        protected void ImgVerModuloNormativa_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                #region MOSTRAR MODULO DE NORMATIVIDAD
                string _UrlNormatividad = ConfigurationManager.AppSettings["Url_normatividad_Cargar_Normativa"].ToString().Trim();
                Response.Redirect(_UrlNormatividad);
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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgVerModuloNormativa_consulta_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                #region MOSTRAR MODULO DE NORMATIVIDAD
                string _UrlNormatividad = ConfigurationManager.AppSettings["Url_normatividad_Consultar_Normativa"].ToString().Trim();
                Response.Redirect(_UrlNormatividad);
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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgVerCargaMasiva_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                #region MOSTRAR MODULO DE NORMATIVIDAD
                string _UrlNormatividad = ConfigurationManager.AppSettings["Url_CARGA_MASIVA"].ToString().Trim();
                Response.Redirect(_UrlNormatividad);
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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgControlReportes_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlPlanormatividad.Visible = false;
            this.PnlPlanoReporte.Visible = true;
        }

        protected void ImgRegresareporte_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = true;
            this.PnlPlanormatividad.Visible = false;
            this.PnlPlanoReporte.Visible = false;
        }

        protected void ImgAnexos_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlPlanormatividad.Visible = false;
            this.PnlPlanoReporte.Visible = false;
            this.Panelanexos.Visible = true;
        }
        protected void ImgRegresarAnexos_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlPlanormatividad.Visible = false;
            this.PnlPlanoReporte.Visible = true;
            this.Panelanexos.Visible = false;
        }


        protected void ImageRenta_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                string Url_generar_archivo_renta = ConfigurationManager.AppSettings["Url_generar_archivo_renta"].ToString().Trim();
                Response.Redirect(Url_generar_archivo_renta);

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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgAutoretencion_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlPlanormatividad.Visible = false;
            this.PnlPlanoReporte.Visible = false;
            this.Panelanexos.Visible = false;
            this.Panelautoretencion.Visible = true;
        }
        protected void ImgRegresArautoretencion_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlPlanormatividad.Visible = false;
            this.PnlPlanoReporte.Visible = false;
            this.Panelanexos.Visible = true;
            this.Panelautoretencion.Visible = false;
        }

        protected void ImageConsulAutore_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                string Url_consulta_autoretencion = ConfigurationManager.AppSettings["Url_consulta_autoretencion"].ToString().Trim();
                Response.Redirect(Url_consulta_autoretencion);

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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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
        protected void ImageGenerAutore_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                string Url_generar_archivo_autoretencion = ConfigurationManager.AppSettings["Url_generar_archivo_autoretencion"].ToString().Trim();
                Response.Redirect(Url_generar_archivo_autoretencion);

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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgPredial_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlPlanormatividad.Visible = false;
            this.PnlPlanoReporte.Visible = false;
            this.Panelanexos.Visible = false;
            this.Panelautoretencion.Visible = false;
            this.PanelPredial.Visible = true;
        }
        protected void ImgRegresPredial_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlPlanormatividad.Visible = false;
            this.PnlPlanoReporte.Visible = false;
            this.Panelanexos.Visible = true;
            this.Panelautoretencion.Visible = false;
            this.PanelPredial.Visible = false;
        }
        protected void ImageCarguePredial_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                string Url_cargue_anexo_rent_imp_predial = ConfigurationManager.AppSettings["Url_cargue_anexo_rent_imp_predial"].ToString().Trim();
                Response.Redirect(Url_cargue_anexo_rent_imp_predial);

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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImageConsultPredial_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                string Url_consulta_anexo_rent_imp_predial = ConfigurationManager.AppSettings["Url_consulta_anexo_rent_imp_predial"].ToString().Trim();
                Response.Redirect(Url_consulta_anexo_rent_imp_predial);

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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImageGenerarPredial_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                string Url_generar_anexo_rent_imp_predial = ConfigurationManager.AppSettings["Url_generar_anexo_rent_imp_predial"].ToString().Trim();
                Response.Redirect(Url_generar_anexo_rent_imp_predial);

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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgControlRequerimiento_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlPlanormatividad.Visible = false;
            this.PnlPlanoReporte.Visible = false;
            this.PnlPlanoImageRequerimiento.Visible = true;
        }

        protected void ImgregRegresaRequerimiento_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = true;
            this.PnlPlanormatividad.Visible = false;
            this.PnlPlanoReporte.Visible = false;
            this.PnlPlanoImageRequerimiento.Visible = false;
        }

        protected void ImageRecDocumentos_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                string Url_Administrar_Documentos = ConfigurationManager.AppSettings["Url_Administrar_Documentos"].ToString().Trim();
                Response.Redirect(Url_Administrar_Documentos);

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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImageConsulRequeri_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                string Url_Consulta_Requerimientos = ConfigurationManager.AppSettings["Url_Consulta_Requerimientos"].ToString().Trim();
                Response.Redirect(Url_Consulta_Requerimientos);

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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImageGraficaRequeri_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                string Url_Gráficas_Estadíticas = ConfigurationManager.AppSettings["Url_Gráficas_Estadíticas"].ToString().Trim();
                Response.Redirect(Url_Gráficas_Estadíticas);

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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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


        protected void ImgControlcustodia_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = false;
            this.PnlPlanormatividad.Visible = false;
            this.PnlPlanoReporte.Visible = false;
            this.PnlPlanoImageRequerimiento.Visible = false;
            this.PnlPlanoImagecustodia.Visible = true;

        }

        protected void ImgRegresaControlcustodia_Click(object sender, ImageClickEventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.PnlTrxGeneral.Visible = true;
            this.PnlPlanormatividad.Visible = false;
            this.PnlPlanoReporte.Visible = false;
            this.PnlPlanoImageRequerimiento.Visible = false;
            this.PnlPlanoImagecustodia.Visible = false;
        }
        protected void ImageCargue_Manual_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                string Url_Cargue_Manual = ConfigurationManager.AppSettings["Url_Cargue_Manual"].ToString().Trim();
                Response.Redirect(Url_Cargue_Manual);

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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImageCargue_Masivo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                string Url_Cargue_Masivo = ConfigurationManager.AppSettings["Url_Cargue_Masivo"].ToString().Trim();
                Response.Redirect(Url_Cargue_Masivo);

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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImageConsulta_claraciones_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                string Url_Consulta_Declaraciones = ConfigurationManager.AppSettings["Url_Consulta_Declaraciones"].ToString().Trim();
                Response.Redirect(Url_Consulta_Declaraciones);

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
                string _MsgMensaje = "Error al mostrar el formulario. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

        protected void ImgConciliacionHC_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                #region MOSTRAR DATOS DE CONCILIACION HC
                Response.Redirect("/Modulos/Administracion/ConciliacionHC/FrmConciliacionHC.aspx");
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
                string _MsgMensaje = "Error al mostrar el formulario del proceso de conciliacion HC. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow" + objUtils.GetRandom();
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

    }
}