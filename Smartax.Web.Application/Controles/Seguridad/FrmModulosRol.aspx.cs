using System;
using System.Web;
using System.Web.Caching;
using System.Data;
using log4net;
using Telerik.Web.UI;
using System.Drawing;
using System.Web.UI.WebControls;
using Smartax.Web.Application.Clases.Seguridad;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Seguridad
{
    public partial class FrmModulosRol : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        ModuloSmartax ObjModulos = new ModuloSmartax();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        Utilidades ObjUtils = new Utilidades();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                ViewState["MenuID"] = null;
                this.LblIdRol.Text = Request.QueryString["RolID"].ToString().Trim();

                //Aqui llamamos la función que Muestra el menu de navegacion
                GetModulosSistema();
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

        private void GetModulosSistema()
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                //--PASAR PARAMETROS PARA OBTENER MODULOS
                ObjModulos.IdRol = this.LblIdRol.Text.ToString().Trim();
                ObjModulos.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                if (ObjModulos.DevolverModulos())
                {
                    #region OBTENER DATOS DE LA DB
                    //--MODULO 1
                    this.ChkPlaneacionFiscal.Checked = ObjModulos.PlaneacionFiscal;
                    this.ChkCalendarioTrib.Enabled = ObjModulos.CalendarioTributario == true ? true : false;
                    this.ChkCalendarioTrib.Checked = ObjModulos.CalendarioTributario;
                    this.ChkTarifasExcesivas.Enabled = ObjModulos.TarifasExcesivas == true ? true : false;
                    this.ChkTarifasExcesivas.Checked = ObjModulos.TarifasExcesivas;
                    this.ChkCampoFuturo1.Checked = false;
                    this.ChkCampoFuturo2.Checked = false;
                    //--MODULO 2
                    this.ChkInfoTributaria.Checked = ObjModulos.InformacionTributaria;
                    if (this.ChkInfoTributaria.Checked == true)
                    {
                        #region HABILITAR Y SELECCIONAR
                        this.ChkHojaTrabajo.Enabled = true;
                        this.ChkHojaTrabajo.Checked = ObjModulos.HojaTrabajo;
                        this.ChkDeclaracionDef.Enabled = true;
                        this.ChkDeclaracionDef.Checked = ObjModulos.DeclaracionDefinitiva;
                        this.ChkEjecucionLote.Enabled = true;
                        this.ChkEjecucionLote.Checked = ObjModulos.EjecucionPorLote;
                        this.ChkValidarLiqLote.Enabled = true;
                        this.ChkValidarLiqLote.Checked = ObjModulos.ValidacionLiqLote;
                        this.ChkConsultarLiq.Enabled = true;
                        this.ChkConsultarLiq.Checked = ObjModulos.ConsultaLiquidacion;
                        this.ChkFichaTecnica.Enabled = true;
                        this.ChkFichaTecnica.Checked = ObjModulos.FichaTecnica;
                        #endregion
                    }
                    else
                    {
                        #region DESABILITAR Y NO SELECCIONAR
                        this.ChkHojaTrabajo.Enabled = false;
                        this.ChkHojaTrabajo.Checked = false;
                        this.ChkDeclaracionDef.Enabled = false;
                        this.ChkDeclaracionDef.Checked = false;
                        this.ChkEjecucionLote.Enabled = false;
                        this.ChkEjecucionLote.Checked = false;
                        this.ChkValidarLiqLote.Enabled = false;
                        this.ChkValidarLiqLote.Checked = false;
                        this.ChkConsultarLiq.Enabled = false;
                        this.ChkConsultarLiq.Checked = false;
                        this.ChkFichaTecnica.Enabled = false;
                        this.ChkFichaTecnica.Checked = false;
                        #endregion
                    }

                    //--MODULO 3
                    this.ChkFormatosSfc.Checked = ObjModulos.FormatosSfc;
                    if (this.ChkFormatosSfc.Checked == true)
                    {
                        #region HABILITAR Y SELECCIONAR
                        this.ChkGeneracionDatos.Enabled = true;
                        this.ChkGeneracionDatos.Checked = ObjModulos.GeneracionDatos;
                        this.ChkGenerarF321.Enabled = true;
                        this.ChkGenerarF321.Checked = ObjModulos.GenerarF321;
                        this.ChkGenerarF525.Enabled = true;
                        this.ChkGenerarF525.Checked = ObjModulos.GenerarF525;
                        this.ChkGenerarArcPlanos.Enabled = true;
                        this.ChkGenerarArcPlanos.Checked = ObjModulos.GenerarArchivoPlano;
                        #endregion
                    }
                    else
                    {
                        #region DESABILITAR Y NO SELECCIONAR
                        this.ChkGeneracionDatos.Enabled = false;
                        this.ChkGeneracionDatos.Checked = false;
                        this.ChkGenerarF321.Enabled = false;
                        this.ChkGenerarF321.Checked = false;
                        this.ChkGenerarF525.Enabled = false;
                        this.ChkGenerarF525.Checked = false;
                        this.ChkGenerarArcPlanos.Enabled = false;
                        this.ChkGenerarArcPlanos.Checked = false;
                        #endregion
                    }

                    //--MODULO 4
                    this.ChkNormatividad.Checked = ObjModulos.Normatividad;
                    if (this.ChkNormatividad.Checked == true)
                    {
                        #region HABILITAR Y SELECCIONAR
                        this.ChkCargarNormatividad.Enabled = true;
                        this.ChkCargarNormatividad.Checked = ObjModulos.CargarNormatividad;
                        this.ChkConsNormatividad.Enabled = true;
                        this.ChkConsNormatividad.Checked = ObjModulos.ConsultarNormatividad;
                        this.ChkCargaMasNormatividad.Enabled = true;
                        this.ChkCargaMasNormatividad.Checked = ObjModulos.CargaMasivaDoc;
                        #endregion
                    }
                    else
                    {
                        #region DESABILITAR Y NO SELECCIONAR
                        this.ChkCargarNormatividad.Enabled = false;
                        this.ChkCargarNormatividad.Checked = false;
                        this.ChkConsNormatividad.Enabled = false;
                        this.ChkConsNormatividad.Checked = false;
                        this.ChkCargaMasNormatividad.Enabled = false;
                        this.ChkCargaMasNormatividad.Checked = false;
                        #endregion
                    }

                    //--MODULO 5
                    this.ChkControlActividades.Checked = ObjModulos.ControlActividades;
                    if (this.ChkControlActividades.Checked == true)
                    {
                        #region HABILITAR Y SELECCIONAR
                        this.ChkMisActividades.Enabled = true;
                        this.ChkMisActividades.Checked = ObjModulos.MisActividades;
                        this.ChkMonitoreoAct.Enabled = true;
                        this.ChkMonitoreoAct.Checked = ObjModulos.MonitoreoActividades;
                        this.ChkEstadisticaAct.Enabled = true;
                        this.ChkEstadisticaAct.Checked = ObjModulos.EstadisticaActividades;
                        this.ChkEstadisticaLiq.Enabled = true;
                        this.ChkEstadisticaLiq.Checked = ObjModulos.EstadisticaLiquidacion;
                        #endregion
                    }
                    else
                    {
                        #region DESABILITAR Y NO SELECCIONAR
                        this.ChkMisActividades.Enabled = false;
                        this.ChkMisActividades.Checked = false;
                        this.ChkMonitoreoAct.Enabled = false;
                        this.ChkMonitoreoAct.Checked = false;
                        this.ChkEstadisticaAct.Enabled = false;
                        this.ChkEstadisticaAct.Checked = false;
                        this.ChkEstadisticaLiq.Enabled = false;
                        this.ChkEstadisticaLiq.Checked = false;
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region DESABILITAR CONTROLES DEL FORM
                    //--MODULO 1
                    this.ChkPlaneacionFiscal.Checked = false;
                    this.ChkCalendarioTrib.Enabled = false;
                    this.ChkCalendarioTrib.Checked = false;
                    this.ChkTarifasExcesivas.Enabled = false;
                    this.ChkTarifasExcesivas.Checked = false;
                    this.ChkCampoFuturo1.Enabled = false;
                    this.ChkCampoFuturo2.Enabled = false;
                    //--MODULO 2
                    this.ChkInfoTributaria.Checked = false;
                    this.ChkHojaTrabajo.Enabled = false;
                    this.ChkHojaTrabajo.Checked = false;
                    this.ChkDeclaracionDef.Enabled = false;
                    this.ChkDeclaracionDef.Checked = false;
                    this.ChkEjecucionLote.Enabled = false;
                    this.ChkEjecucionLote.Checked = false;
                    this.ChkValidarLiqLote.Enabled = false;
                    this.ChkValidarLiqLote.Checked = false;
                    this.ChkConsultarLiq.Enabled = false;
                    this.ChkConsultarLiq.Checked = false;
                    this.ChkFichaTecnica.Enabled = false;
                    this.ChkFichaTecnica.Checked = false;
                    //--MODULO 3
                    this.ChkControlActividades.Checked = false;
                    this.ChkMisActividades.Enabled = false;
                    this.ChkMisActividades.Checked = false;
                    this.ChkMonitoreoAct.Enabled = false;
                    this.ChkMonitoreoAct.Checked = false;
                    this.ChkEstadisticaAct.Enabled = false;
                    this.ChkEstadisticaAct.Checked = false;
                    this.ChkEstadisticaLiq.Enabled = false;
                    this.ChkEstadisticaLiq.Checked = false;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENASJE DE USUARIO
                //mostramos la ventana del mensaje del sistema.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                //Aqui mostramos el mensaje del proceso al usuario
                string _MsgError = "Error al obtener modulos del sistema. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow1";
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

        #region EVENTOS DE SELECCION DEL FORM
        protected void ChkSeleccionarTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSeleccionarTodos.Checked == true)
            {
                #region HABILITAR CONTROLES DEL FORMULARIO
                this.ChkQuitarTodos.Checked = false;
                //--MODULO 1
                this.ChkPlaneacionFiscal.Checked = true;
                this.ChkCalendarioTrib.Enabled = true;
                this.ChkCalendarioTrib.Checked = true;
                this.ChkTarifasExcesivas.Enabled = true;
                this.ChkTarifasExcesivas.Checked = true;
                this.ChkCampoFuturo1.Checked = true;
                this.ChkCampoFuturo2.Checked = true;
                //--MODULO 2
                this.ChkInfoTributaria.Checked = true;
                this.ChkHojaTrabajo.Enabled = true;
                this.ChkHojaTrabajo.Checked = true;
                this.ChkDeclaracionDef.Enabled = true;
                this.ChkDeclaracionDef.Checked = true;
                this.ChkEjecucionLote.Enabled = true;
                this.ChkEjecucionLote.Checked = true;
                this.ChkValidarLiqLote.Enabled = true;
                this.ChkValidarLiqLote.Checked = true;
                this.ChkConsultarLiq.Enabled = true;
                this.ChkConsultarLiq.Checked = true;
                this.ChkFichaTecnica.Enabled = true;
                this.ChkFichaTecnica.Checked = true;
                //--MODULO 3
                this.ChkFormatosSfc.Checked = true;
                this.ChkGeneracionDatos.Enabled = true;
                this.ChkGeneracionDatos.Checked = true;
                this.ChkGenerarF321.Enabled = true;
                this.ChkGenerarF321.Checked = true;
                this.ChkGenerarF525.Enabled = true;
                this.ChkGenerarF525.Checked = true;
                this.ChkGenerarArcPlanos.Enabled = true;
                this.ChkGenerarArcPlanos.Checked = true;
                //--MODULO 4
                this.ChkNormatividad.Checked = true;
                this.ChkCargarNormatividad.Enabled = true;
                this.ChkCargarNormatividad.Checked = true;
                this.ChkConsNormatividad.Enabled = true;
                this.ChkConsNormatividad.Checked = true;
                this.ChkCargaMasNormatividad.Enabled = true;
                this.ChkCargaMasNormatividad.Checked = true;
                //--MODULO 5
                this.ChkControlActividades.Checked = true;
                this.ChkMisActividades.Enabled = true;
                this.ChkMisActividades.Checked = true;
                this.ChkMonitoreoAct.Enabled = true;
                this.ChkMonitoreoAct.Checked = true;
                this.ChkEstadisticaAct.Enabled = true;
                this.ChkEstadisticaAct.Checked = true;
                this.ChkEstadisticaLiq.Enabled = true;
                this.ChkEstadisticaLiq.Checked = true;
                #endregion
            }
            else
            {
                #region DESHABILITAR CONTROLES DEL FORMULARIO
                this.ChkQuitarTodos.Checked = false;
                //--MODULO 1
                this.ChkPlaneacionFiscal.Checked = false;
                this.ChkCalendarioTrib.Enabled = false;
                this.ChkCalendarioTrib.Checked = false;
                this.ChkTarifasExcesivas.Enabled = false;
                this.ChkTarifasExcesivas.Checked = false;
                this.ChkCampoFuturo1.Checked = false;
                this.ChkCampoFuturo2.Checked = false;
                //--MODULO 2
                this.ChkInfoTributaria.Checked = false;
                this.ChkHojaTrabajo.Enabled = false;
                this.ChkHojaTrabajo.Checked = false;
                this.ChkDeclaracionDef.Enabled = false;
                this.ChkDeclaracionDef.Checked = false;
                this.ChkEjecucionLote.Enabled = false;
                this.ChkEjecucionLote.Checked = false;
                this.ChkValidarLiqLote.Enabled = false;
                this.ChkValidarLiqLote.Checked = false;
                this.ChkConsultarLiq.Enabled = false;
                this.ChkConsultarLiq.Checked = false;
                this.ChkFichaTecnica.Enabled = false;
                this.ChkFichaTecnica.Checked = false;
                //--MODULO 3
                this.ChkFormatosSfc.Checked = false;
                this.ChkGeneracionDatos.Enabled = false;
                this.ChkGeneracionDatos.Checked = false;
                this.ChkGenerarF321.Enabled = false;
                this.ChkGenerarF321.Checked = false;
                this.ChkGenerarF525.Enabled = false;
                this.ChkGenerarF525.Checked = false;
                this.ChkGenerarArcPlanos.Enabled = false;
                this.ChkGenerarArcPlanos.Checked = false;
                //--MODULO 4
                this.ChkNormatividad.Checked = false;
                this.ChkCargarNormatividad.Enabled = false;
                this.ChkCargarNormatividad.Checked = false;
                this.ChkConsNormatividad.Enabled = false;
                this.ChkConsNormatividad.Checked = false;
                this.ChkCargaMasNormatividad.Enabled = false;
                this.ChkCargaMasNormatividad.Checked = false;
                //--MODULO 5
                this.ChkControlActividades.Checked = false;
                this.ChkMisActividades.Enabled = false;
                this.ChkMisActividades.Checked = false;
                this.ChkMonitoreoAct.Enabled = false;
                this.ChkMonitoreoAct.Checked = false;
                this.ChkEstadisticaAct.Enabled = false;
                this.ChkEstadisticaAct.Checked = false;
                this.ChkEstadisticaLiq.Enabled = false;
                this.ChkEstadisticaLiq.Checked = false;
                #endregion
            }
        }

        protected void ChkQuitarTodos_CheckedChanged(object sender, EventArgs e)
        {
            #region DESHABILITAR CONTROLES DEL FORMULARIO
            this.ChkSeleccionarTodos.Checked = false;
            //--MODULO 1
            this.ChkPlaneacionFiscal.Checked = false;
            this.ChkCalendarioTrib.Enabled = false;
            this.ChkCalendarioTrib.Checked = false;
            this.ChkTarifasExcesivas.Enabled = false;
            this.ChkTarifasExcesivas.Checked = false;
            this.ChkCampoFuturo1.Checked = false;
            this.ChkCampoFuturo2.Checked = false;
            //--MODULO 2
            this.ChkInfoTributaria.Checked = false;
            this.ChkHojaTrabajo.Enabled = false;
            this.ChkHojaTrabajo.Checked = false;
            this.ChkDeclaracionDef.Enabled = false;
            this.ChkDeclaracionDef.Checked = false;
            this.ChkEjecucionLote.Enabled = false;
            this.ChkEjecucionLote.Checked = false;
            this.ChkValidarLiqLote.Enabled = false;
            this.ChkValidarLiqLote.Checked = false;
            this.ChkConsultarLiq.Enabled = false;
            this.ChkConsultarLiq.Checked = false;
            this.ChkFichaTecnica.Enabled = false;
            this.ChkFichaTecnica.Checked = false;
            //--MODULO 3
            this.ChkFormatosSfc.Checked = false;
            this.ChkGeneracionDatos.Enabled = false;
            this.ChkGeneracionDatos.Checked = false;
            this.ChkGenerarF321.Enabled = false;
            this.ChkGenerarF321.Checked = false;
            this.ChkGenerarF525.Enabled = false;
            this.ChkGenerarF525.Checked = false;
            this.ChkGenerarArcPlanos.Enabled = false;
            this.ChkGenerarArcPlanos.Checked = false;
            //--MODULO 4
            this.ChkNormatividad.Checked = false;
            this.ChkCargarNormatividad.Enabled = false;
            this.ChkCargarNormatividad.Checked = false;
            this.ChkConsNormatividad.Enabled = false;
            this.ChkConsNormatividad.Checked = false;
            this.ChkCargaMasNormatividad.Enabled = false;
            this.ChkCargaMasNormatividad.Checked = false;
            //--MODULO 5
            this.ChkControlActividades.Checked = false;
            this.ChkMisActividades.Enabled = false;
            this.ChkMisActividades.Checked = false;
            this.ChkMonitoreoAct.Enabled = false;
            this.ChkMonitoreoAct.Checked = false;
            this.ChkEstadisticaAct.Enabled = false;
            this.ChkEstadisticaAct.Checked = false;
            this.ChkEstadisticaLiq.Enabled = false;
            this.ChkEstadisticaLiq.Checked = false;
            #endregion
        }

        protected void ChkPlaneacionFiscal_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkPlaneacionFiscal.Checked == true)
            {
                this.ChkCalendarioTrib.Enabled = true;
                this.ChkTarifasExcesivas.Enabled = true;
                this.ChkCalendarioTrib.Enabled = true;
            }
            else
            {
                this.ChkCalendarioTrib.Enabled = false;
                this.ChkCalendarioTrib.Checked = false;
                this.ChkTarifasExcesivas.Enabled = false;
                this.ChkTarifasExcesivas.Checked = false;
                this.ChkCalendarioTrib.Enabled = false;
                this.ChkCalendarioTrib.Checked = false;
            }
        }

        protected void ChkInfoTributaria_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkInfoTributaria.Checked == true)
            {
                this.ChkHojaTrabajo.Enabled = true;
                this.ChkDeclaracionDef.Enabled = true;
                this.ChkEjecucionLote.Enabled = true;
                this.ChkValidarLiqLote.Enabled = true;
                this.ChkConsultarLiq.Enabled = true;
                this.ChkFichaTecnica.Enabled = true;
            }
            else
            {
                this.ChkHojaTrabajo.Enabled = false;
                this.ChkHojaTrabajo.Checked = false;
                this.ChkDeclaracionDef.Enabled = false;
                this.ChkDeclaracionDef.Checked = false;
                this.ChkEjecucionLote.Enabled = false;
                this.ChkEjecucionLote.Checked = false;
                this.ChkValidarLiqLote.Enabled = false;
                this.ChkValidarLiqLote.Checked = false;
                this.ChkConsultarLiq.Enabled = false;
                this.ChkConsultarLiq.Checked = false;
                this.ChkFichaTecnica.Enabled = false;
                this.ChkFichaTecnica.Checked = false;
            }
        }

        protected void ChkFormatosSfc_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkFormatosSfc.Checked == true)
            {
                this.ChkGeneracionDatos.Enabled = true;
                this.ChkGenerarF321.Enabled = true;
                this.ChkGenerarF525.Enabled = true;
                this.ChkGenerarArcPlanos.Enabled = true;
            }
            else
            {
                this.ChkGeneracionDatos.Enabled = false;
                this.ChkGeneracionDatos.Checked = false;
                this.ChkGenerarF321.Enabled = false;
                this.ChkGenerarF321.Checked = false;
                this.ChkGenerarF525.Enabled = false;
                this.ChkGenerarF525.Checked = false;
                this.ChkGenerarArcPlanos.Enabled = false;
                this.ChkGenerarArcPlanos.Checked = false;
            }
        }

        protected void ChkNormatividad_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkNormatividad.Checked == true)
            {
                this.ChkCargarNormatividad.Enabled = true;
                this.ChkConsNormatividad.Enabled = true;
                this.ChkCargaMasNormatividad.Enabled = true;
            }
            else
            {
                this.ChkCargarNormatividad.Enabled = false;
                this.ChkCargarNormatividad.Checked = false;
                this.ChkConsNormatividad.Enabled = false;
                this.ChkConsNormatividad.Checked = false;
                this.ChkCargaMasNormatividad.Enabled = false;
                this.ChkCargaMasNormatividad.Checked = false;
            }
        }

        protected void ChkControlActividades_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkControlActividades.Checked == true)
            {
                this.ChkMisActividades.Enabled = true;
                this.ChkMonitoreoAct.Enabled = true;
                this.ChkEstadisticaAct.Enabled = true;
                this.ChkEstadisticaLiq.Enabled = true;
            }
            else
            {
                this.ChkMisActividades.Enabled = false;
                this.ChkMisActividades.Checked = false;
                this.ChkMonitoreoAct.Enabled = false;
                this.ChkMonitoreoAct.Checked = false;
                this.ChkEstadisticaAct.Enabled = false;
                this.ChkEstadisticaAct.Checked = false;
                this.ChkEstadisticaLiq.Enabled = false;
                this.ChkEstadisticaLiq.Checked = false;
            }
        }
        #endregion

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                #region OBTENER DATOS PARA EL OBJETO DE CLASE
                ObjModulos.IdModulo = null;
                ObjModulos.IdRol = Int32.Parse(this.LblIdRol.Text.ToString().Trim());

                ObjModulos.PlaneacionFiscal = this.ChkPlaneacionFiscal.Checked == true ? true : false;
                ObjModulos.CalendarioTributario = this.ChkCalendarioTrib.Checked == true ? true : false;
                ObjModulos.TarifasExcesivas = this.ChkTarifasExcesivas.Checked == true ? true : false;
                //--
                ObjModulos.InformacionTributaria = this.ChkInfoTributaria.Checked == true ? true : false;
                ObjModulos.HojaTrabajo = this.ChkHojaTrabajo.Checked == true ? true : false;
                ObjModulos.DeclaracionDefinitiva = this.ChkDeclaracionDef.Checked == true ? true : false;
                ObjModulos.EjecucionPorLote = this.ChkEjecucionLote.Checked == true ? true : false;
                ObjModulos.ValidacionLiqLote = this.ChkValidarLiqLote.Checked == true ? true : false;
                ObjModulos.ConsultaLiquidacion = this.ChkConsultarLiq.Checked == true ? true : false;
                ObjModulos.FichaTecnica = this.ChkFichaTecnica.Checked == true ? true : false;
                //--
                ObjModulos.FormatosSfc = this.ChkFormatosSfc.Checked == true ? true : false;
                ObjModulos.GeneracionDatos = this.ChkGeneracionDatos.Checked == true ? true : false;
                ObjModulos.GenerarF321 = this.ChkGenerarF321.Checked == true ? true : false;
                ObjModulos.GenerarF525 = this.ChkGenerarF525.Checked == true ? true : false;
                ObjModulos.GenerarArchivoPlano = this.ChkGenerarArcPlanos.Checked == true ? true : false;
                //--
                ObjModulos.Normatividad = this.ChkNormatividad.Checked == true ? true : false;
                ObjModulos.CargarNormatividad = this.ChkCargarNormatividad.Checked == true ? true : false;
                ObjModulos.ConsultarNormatividad = this.ChkConsNormatividad.Checked == true ? true : false;
                ObjModulos.CargaMasivaDoc = this.ChkCargaMasNormatividad.Checked == true ? true : false;
                //--
                ObjModulos.ControlActividades = this.ChkControlActividades.Checked == true ? true : false;
                ObjModulos.MisActividades = this.ChkMisActividades.Checked == true ? true : false;
                ObjModulos.MonitoreoActividades = this.ChkMonitoreoAct.Checked == true ? true : false;
                ObjModulos.EstadisticaActividades = this.ChkEstadisticaAct.Checked == true ? true : false;
                ObjModulos.EstadisticaLiquidacion = this.ChkEstadisticaLiq.Checked == true ? true : false;
                //--
                ObjModulos.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjModulos.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjModulos.TipoProceso = 1;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjModulos);
                #endregion

                int _IdRegistro = 0;
                string _MsgError = "";
                if (ObjModulos.AddUpModulos(ref _IdRegistro, ref _MsgError))
                {
                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.ModuloApp = "MODULOS_ROL";
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
                    //mostramos la ventana del mensaje del sistema.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    //Aqui mostramos el mensaje del proceso al usuario
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                    Ventana.ID = "RadWindow1";
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
                    //mostramos la ventana del mensaje del sistema.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    //Aqui mostramos el mensaje del proceso al usuario
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                    Ventana.ID = "RadWindow1";
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
                //mostramos la ventana del mensaje del sistema.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;
                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                //Aqui mostramos el mensaje del proceso al usuario
                string _MsgError = "Error al asignar el Permiso a la opción de menu [" + this.ViewState["DescripcionMenu"].ToString().Trim() + "]. Motivo: " + ex.Message.ToString().Trim();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow1";
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
}