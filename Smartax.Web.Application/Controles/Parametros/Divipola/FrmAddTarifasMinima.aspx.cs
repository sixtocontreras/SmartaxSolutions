using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Web.Caching;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Parametros.Divipola;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros;

namespace Smartax.Web.Application.Controles.Parametros.Divipola
{
    public partial class FrmAddTarifasMinima : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        #region DEFINICION DE OBJETOS DE CLASE
        MunicipioTarifasMinima ObjMunTarMin = new MunicipioTarifasMinima();
        FormularioImpuesto ObjFrmImpuesto = new FormularioImpuesto();
        FormConfiguracion ObjFormConf = new FormConfiguracion();
        UnidadesMedida ObjUnidades = new UnidadesMedida();
        ValorUnidadMedida ObjValorUnidad = new ValorUnidadMedida();
        TiposTarifa ObjTipoTarifa = new TiposTarifa();
        Estado ObjEstado = new Estado();
        Utilidades ObjUtils = new Utilidades();
        #endregion

        private void GetInfoTarifa()
        {
            try
            {
                ObjMunTarMin.TipoConsulta = 2;
                ObjMunTarMin.IdMunTarMinima = Int32.Parse(this.ViewState["IdMunTarifaMinima"].ToString().Trim());
                ObjMunTarMin.IdMunicipio = Int32.Parse(this.ViewState["IdMunicipio"].ToString().Trim());
                ObjMunTarMin.IdEstado = null;
                ObjMunTarMin.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjMunTarMin.GetInfoMunTarMinima();

                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        this.CmbTipoImpuesto.SelectedValue = dtDatos.Rows[0]["idformulario_impuesto"].ToString().Trim();
                        this.LstTipoImpuesto();

                        bool ChkCalcular = Boolean.Parse(dtDatos.Rows[0]["calcular_renglon"].ToString().Trim());
                        this.ChkCalcular.Checked = ChkCalcular;

                        //--AQUI VALIDAMOS EL CHEK A VALIDAR
                        if (ChkCalcular == true)
                        {
                            this.LstFormConfiguracion();
                            this.Validador8.Enabled = true;
                            this.CmbRenglonForm.Enabled = true;
                            this.CmbRenglonForm.SelectedValue = dtDatos.Rows[0]["idformulario_configuracion"].ToString().Trim();
                        }
                        else
                        {
                            this.Validador8.Enabled = false;
                            this.CmbRenglonForm.Enabled = false;
                            this.CmbRenglonForm.SelectedValue = "";
                        }

                        this.CmbUnidadMedida.SelectedValue = dtDatos.Rows[0]["idunidad_medida"].ToString().Trim();
                        this.LstUnidadesMedida();
                        this.CmbAnioGravable.SelectedValue = dtDatos.Rows[0]["idvalor_unid_medida"].ToString().Trim() + "|" + dtDatos.Rows[0]["valor_unidad"].ToString().Trim();
                        this.LstValorUnidadMedida();
                        this.CmbTipoTarifa.SelectedValue = dtDatos.Rows[0]["idtipo_tarifa"].ToString().Trim();
                        this.LstTipoTarifa();

                        int _IdTipoTarifa = Int32.Parse(dtDatos.Rows[0]["idtipo_tarifa"].ToString().Trim());

                        this.TxtCantidad.Text = dtDatos.Rows[0]["cantidad_medida"].ToString().Trim();
                        double _CantidadMedida = Convert.ToDouble(this.TxtCantidad.Text.ToString().Trim());
                        double _ValorUnidad = Convert.ToDouble(dtDatos.Rows[0]["valor_unidad"].ToString().Trim());

                        double _TarifaMinima = 0;
                        if (_IdTipoTarifa == 1)         //--PORCENTUAL
                        {
                            _TarifaMinima = ((_CantidadMedida * _ValorUnidad) / 100);
                        }
                        else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                        {
                            _TarifaMinima = (_CantidadMedida * _ValorUnidad);
                        }

                        this.LblValorUnidad.Text = String.Format(String.Format("{0:$ ###,###,##0}", _ValorUnidad));
                        this.LblValorTarifaMinima.Text = String.Format(String.Format("{0:$ ###,###,##0}", _TarifaMinima));
                        this.CmbEstado.SelectedValue = dtDatos.Rows[0]["id_estado"].ToString().Trim();
                        this.LstEstado();
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
                        string _MsgMensaje = "No se encontro información con la tarifa seleccionada... !";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al mostrar los datos de la tarifa. Motivo: " + ex.ToString();
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

        #region LISTA DE COMBO BOX DEL FORMULARIO
        protected void LstTipoImpuesto()
        {
            try
            {
                ObjFrmImpuesto.TipoConsulta = 2;
                ObjFrmImpuesto.IdEstado = 1;
                ObjFrmImpuesto.MostrarSeleccione = "SI";
                ObjFrmImpuesto.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbTipoImpuesto.DataSource = ObjFrmImpuesto.GetFormularioImpuesto();
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

        protected void LstFormConfiguracion()
        {
            try
            {
                ObjFormConf.TipoConsulta = 2;
                ObjFormConf.IdFormularioImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim();
                ObjFormConf.IdEstado = 1;
                ObjFormConf.MostrarSeleccione = "SI";
                ObjFormConf.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbRenglonForm.DataSource = ObjFormConf.GetFormConfiguracionRenglon();
                this.CmbRenglonForm.DataValueField = "idformulario_configuracion";
                this.CmbRenglonForm.DataTextField = "numero_renglon";
                this.CmbRenglonForm.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar. Motivo: " + ex.ToString();
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

        protected void LstUnidadesMedida()
        {
            try
            {
                ObjUnidades.TipoConsulta = 2;
                ObjUnidades.IdEstado = 1;
                ObjUnidades.MostrarSeleccione = "SI";
                ObjUnidades.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbUnidadMedida.DataSource = ObjUnidades.GetUnidadMedidas();
                this.CmbUnidadMedida.DataValueField = "idunidad_medida";
                this.CmbUnidadMedida.DataTextField = "descripcion_medida";
                this.CmbUnidadMedida.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar las unidades de medidas. Motivo: " + ex.ToString();
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

        protected void LstValorUnidadMedida()
        {
            try
            {
                ObjValorUnidad.TipoConsulta = 2;
                ObjValorUnidad.IdUnidadMedida = this.CmbUnidadMedida.SelectedValue.ToString().Trim().Length > 0 ? this.CmbUnidadMedida.SelectedValue.ToString().Trim() : null;
                ObjValorUnidad.IdEstado = 1;
                ObjValorUnidad.MostrarSeleccione = "SI";
                ObjValorUnidad.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                //--AÑO GRAVABLE
                this.CmbAnioGravable.DataSource = ObjValorUnidad.GetValorUnidadMedida();
                this.CmbAnioGravable.DataValueField = "idvalor_unid_medida";
                this.CmbAnioGravable.DataTextField = "anio_valor";
                this.CmbAnioGravable.DataBind();

                //--AÑO FISCAL
                this.CmbAnioFiscal.DataSource = ObjValorUnidad.GetValorUnidadMedida();
                this.CmbAnioFiscal.DataValueField = "idvalor_unid_medida";
                this.CmbAnioFiscal.DataTextField = "anio_valor";
                this.CmbAnioFiscal.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar año gravable unidad de medida. Motivo: " + ex.ToString();
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

        protected void LstTipoTarifa()
        {
            try
            {
                ObjTipoTarifa.TipoConsulta = 3;
                ObjTipoTarifa.Interfaz = 2;
                ObjTipoTarifa.IdEstado = 1;
                ObjTipoTarifa.MostrarSeleccione = "SI";
                ObjTipoTarifa.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbTipoTarifa.DataSource = ObjTipoTarifa.GetTipoTarifa();
                this.CmbTipoTarifa.DataValueField = "idtipo_tarifa";
                this.CmbTipoTarifa.DataTextField = "descripcion_tarifa";
                this.CmbTipoTarifa.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar. Motivo: " + ex.ToString();
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
                this.BtnGuardar.Visible = false;
            }
            if (!objPermiso.PuedeModificar)
            {
                this.BtnGuardar.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //this.AplicarPermisos();
                //--AQUI OBTENEMOS LOS PARAMETROS ENVIADOS
                this.ViewState["IdMunicipio"] = Request.QueryString["IdMunicipio"].ToString().Trim();
                this.ViewState["TipoProceso"] = Request.QueryString["TipoProceso"].ToString().Trim();

                if (this.ViewState["TipoProceso"].ToString().Trim().Equals("UPDATE"))
                {
                    this.LblTitulo.Text = "EDITAR TARIFA MINIMA DEL MUNICIPIO";
                    this.ViewState["IdMunTarifaMinima"] = Request.QueryString["IdMunTarifaMinima"].ToString().Trim();
                    this.ViewState["TipoProceso"] = 2;
                    this.GetInfoTarifa();
                }
                else
                {
                    this.LblTitulo.Text = "REGISTRAR TARIFAS MINIMAS DEL MUNICIPIO";
                    this.ViewState["IdMunTarifaMinima"] = "";
                    this.ViewState["TipoProceso"] = 1;

                    //--AQUI LISTAMOS LOS COMBOBOX
                    this.LstTipoImpuesto();
                    this.LstUnidadesMedida();
                    this.LstValorUnidadMedida();
                    this.LstTipoTarifa();
                    this.LstEstado();
                }
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

        #region DEFINICION DE EVENTOS DE LISTAS
        protected void CmbTipoImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.LstFormConfiguracion();
        }

        protected void ChkCalcular_CheckedChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            if (this.ChkCalcular.Checked)
            {
                this.Validador8.Enabled = true;
                this.CmbRenglonForm.Enabled = true;
                this.CmbRenglonForm.SelectedValue = "";
            }
            else
            {
                this.Validador8.Enabled = false;
                this.CmbRenglonForm.Enabled = false;
                this.CmbRenglonForm.SelectedValue = "";
            }
        }

        protected void CmbUnidadMedida_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.LstValorUnidadMedida();
        }

        protected void CmbAnioGravable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0)
            {
                string[] _ArrayData = this.CmbAnioGravable.SelectedValue.ToString().Trim().Split('|');
                double _ValorUnidad = Double.Parse(_ArrayData[1].ToString().Trim());
                this.LblValorUnidad.Text = String.Format(String.Format("{0:$ ###,###,##0}", _ValorUnidad));
                this.ViewState["IdUnidadMedida"] = _ArrayData[0].ToString().Trim();

                //--AQUI VALIDAMOS QUE SE ALLA SELECCIONADO UN TIPO DE TARIFA
                if (this.CmbTipoTarifa.SelectedValue.ToString().Trim().Length > 0)
                {
                    //--AQUI VALIDAMOS SI LA CANTIDAD TIENE UN VALOR PARA RECALCULAR LA TARIFA MINIMA
                    if (this.TxtCantidad.Text.ToString().Trim().Length > 0)
                    {
                        int _IdTipoTarifa = Int32.Parse(this.CmbTipoTarifa.SelectedValue.ToString().Trim());

                        if (Double.Parse(this.TxtCantidad.Text.ToString().Trim()) > 0)
                        {
                            //Recalculamos el valor de la tarifa minima.
                            double _Cantidad = Double.Parse(this.TxtCantidad.Text.ToString().Trim());
                            double _ValorTarifaMinima = 0;

                            if (_IdTipoTarifa == 1)
                            {
                                _ValorTarifaMinima = ((_ValorUnidad * _Cantidad) / 100);
                            }
                            else
                            {
                                _ValorTarifaMinima = (_Cantidad * _ValorUnidad);
                            }

                            this.LblValorTarifaMinima.Text = String.Format(String.Format("{0:$ ###,###,##0}", _ValorTarifaMinima));
                        }
                        else
                        {
                            this.TxtCantidad.Focus();
                        }
                    }
                    else
                    {
                        this.TxtCantidad.Focus();
                    }
                }
                else
                {
                    this.CmbTipoTarifa.Focus();
                }
            }
        }

        protected void CmbTipoTarifa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbTipoTarifa.SelectedValue.ToString().Trim().Length > 0)
                {
                    int _IdTipoTarifa = Int32.Parse(this.CmbTipoTarifa.SelectedValue.ToString().Trim());

                    if (_IdTipoTarifa == 1)
                    {
                        this.LblCantidad.Text = "Porcentaje";
                    }
                    else
                    {
                        this.LblCantidad.Text = "Cantidad Unidad";
                    }

                    if (this.TxtCantidad.Text.ToString().Trim().Length > 0)
                    {
                        if (Double.Parse(this.TxtCantidad.Text.ToString().Trim()) > 0)
                        {
                            double _ValorUnidad = Double.Parse(this.LblValorUnidad.Text.ToString().Trim().Replace("$ ", "").Replace(".", ""));
                            double _Cantidad = Double.Parse(this.TxtCantidad.Text.ToString().Trim().Replace(".", ","));
                            double _ValorTarifaMinima = 0;

                            if (_IdTipoTarifa == 1)
                            {
                                _ValorTarifaMinima = ((_ValorUnidad * _Cantidad) / 100);
                            }
                            else
                            {
                                _ValorTarifaMinima = (_Cantidad * _ValorUnidad);
                            }

                            this.LblValorTarifaMinima.Text = String.Format(String.Format("{0:$ ###,###,##0}", _ValorTarifaMinima));
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
                            string _MsgMensaje = "Señor usuario, el valor de la cantidad debe ser mayor cero !";
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
                    else
                    {
                        this.TxtCantidad.Focus();
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
                    string _MsgMensaje = "Señor usuario, debe seleccionar un tipo de tarifa !";
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
                string _MsgMensaje = "Error al calcular la tarifa mínima del municipio. Motivo: " + ex.ToString();
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

        protected void TxtCantidad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.CmbTipoTarifa.SelectedValue.ToString().Trim().Length > 0)
                {
                    int _IdTipoTarifa = Int32.Parse(this.CmbTipoTarifa.SelectedValue.ToString().Trim());

                    if (this.TxtCantidad.Text.ToString().Trim().Length > 0)
                    {
                        if (Double.Parse(this.TxtCantidad.Text.ToString().Trim()) > 0)
                        {
                            double _ValorUnidad = Double.Parse(this.LblValorUnidad.Text.ToString().Trim().Replace("$ ", "").Replace(".", ""));
                            double _Cantidad = Double.Parse(this.TxtCantidad.Text.ToString().Trim().Replace(".", ","));
                            double _ValorTarifaMinima = 0;

                            if (_IdTipoTarifa == 1)
                            {
                                _ValorTarifaMinima = ((_ValorUnidad * _Cantidad) / 100);
                            }
                            else
                            {
                                _ValorTarifaMinima = (_Cantidad * _ValorUnidad);
                            }

                            this.LblValorTarifaMinima.Text = String.Format(String.Format("{0:$ ###,###,##0}", _ValorTarifaMinima));
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
                            string _MsgMensaje = "Señor usuario, el valor de la cantidad debe ser mayor cero !";
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
                        string _MsgMensaje = "Señor usuario, debe ingresar un valor en la cantidad !";
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
                    string _MsgMensaje = "Señor usuario, debe seleccionar un tipo de tarifa !";
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
                string _MsgMensaje = "Error al calcular la tarifa mínima del municipio. Motivo: " + ex.ToString();
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
        #endregion

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ObjMunTarMin.IdMunTarMinima = this.ViewState["IdMunTarifaMinima"].ToString().Trim().Length > 0 ? this.ViewState["IdMunTarifaMinima"].ToString().Trim() : null;
                ObjMunTarMin.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                ObjMunTarMin.IdFormularioImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim();
                ObjMunTarMin.CalcularRenglon = this.ChkCalcular.Checked == true ? "S" : "N";
                ObjMunTarMin.IdFormuConfiguracion = this.CmbRenglonForm.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm.SelectedValue.ToString().Trim() : null;
                ObjMunTarMin.IdUnidadMedida = this.CmbUnidadMedida.SelectedValue.ToString().Trim();
                string[] _ArrayDatos = this.CmbAnioGravable.SelectedValue.ToString().Trim().Split('|');
                ObjMunTarMin.IdValorUnidadMedida = _ArrayDatos[0].ToString().Trim();
                ObjMunTarMin.IdTipoTarifa = this.CmbTipoTarifa.SelectedValue.ToString().Trim();
                ObjMunTarMin.CantidadMedida = this.TxtCantidad.Text.ToString().Trim().Replace(",", ".");
                ObjMunTarMin.IdEstado = this.CmbEstado.SelectedValue.ToString().Trim();
                ObjMunTarMin.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                ObjMunTarMin.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjMunTarMin.TipoProceso = Int32.Parse(this.ViewState["TipoProceso"].ToString().Trim()) == 1 ? 1 : 2;

                int _IdRegistro = 0;
                string _MsgError = "";
                if (ObjMunTarMin.AddUpMunTarMinima(ref _IdRegistro, ref _MsgError))
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
                string _MsgError = "Error al registrar la tarifa minima del municipio. Motivo: " + ex.ToString();
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
                #endregion
            }
        }
    }
}