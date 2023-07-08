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
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Parametros.Divipola
{
    public partial class FrmAddOtrasConfiguraciones : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        #region DEFINICION DE OBJETOS DE CLASE
        MunicipioTarifasMinima ObjMunTarMin = new MunicipioTarifasMinima();
        FormularioImpuesto ObjFrmImpuesto = new FormularioImpuesto();
        FormConfiguracion ObjFormConf = new FormConfiguracion();
        UnidadesMedida ObjUnidades = new UnidadesMedida();
        ValorUnidadMedida ObjValorUnidad = new ValorUnidadMedida();
        TiposTarifa ObjTipoTarifa = new TiposTarifa();
        TipoOperacion ObjTipoOper = new TipoOperacion();
        Estado ObjEstado = new Estado();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
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
                        //this.LstTipoImpuesto();
                        this.LstFormConfiguracion();
                        this.ViewState["IdFormularioConfig"] = dtDatos.Rows[0]["idform_configuracion"].ToString().Trim();
                        //this.CmbRenglonForm.SelectedValue = dtDatos.Rows[0]["idform_configuracion"].ToString().Trim();
                        this.CmbRenglonForm.SelectedValue = dtDatos.Rows[0]["idform_configuracion"].ToString().Trim() + "|" + dtDatos.Rows[0]["descripcion_renglon"].ToString().Trim();

                        bool ChkCalcular = Boolean.Parse(dtDatos.Rows[0]["calcular_renglon"].ToString().Trim());
                        this.ChkCalcular.Checked = ChkCalcular;

                        //--AQUI VALIDAMOS EL CHEK A VALIDAR
                        if (ChkCalcular == true)
                        {
                            #region LLENAR LA LISTA DESPLEGABLE
                            //this.LstFormConfiguracion();
                            this.Validador13.Enabled = true;
                            this.Validador14.Enabled = true;
                            this.Validador15.Enabled = true;
                            this.CmbRenglonForm1.Enabled = true;
                            this.CmbRenglonForm1.SelectedValue = dtDatos.Rows[0]["idform_configuracion1"].ToString().Trim();
                            this.CmbSimbolo.Enabled = true;
                            this.CmbSimbolo.SelectedValue = dtDatos.Rows[0]["idtipo_operacion"].ToString().Trim();
                            this.CmbRenglonForm2.Enabled = true;
                            this.CmbRenglonForm2.SelectedValue = dtDatos.Rows[0]["idform_configuracion2"].ToString().Trim();
                            this.CmbSimbolo2.Enabled = true;
                            this.CmbSimbolo2.SelectedValue = dtDatos.Rows[0]["idtipo_operacion2"].ToString().Trim();
                            this.CmbRenglonForm3.Enabled = true;
                            this.CmbRenglonForm3.SelectedValue = dtDatos.Rows[0]["idform_configuracion3"].ToString().Trim();
                            this.CmbSimbolo3.Enabled = true;
                            this.CmbSimbolo3.SelectedValue = dtDatos.Rows[0]["idtipo_operacion3"].ToString().Trim();
                            this.CmbRenglonForm4.Enabled = true;
                            this.CmbRenglonForm4.SelectedValue = dtDatos.Rows[0]["idform_configuracion4"].ToString().Trim();
                            this.CmbSimbolo4.Enabled = true;
                            this.CmbSimbolo4.SelectedValue = dtDatos.Rows[0]["idtipo_operacion4"].ToString().Trim();
                            this.CmbRenglonForm5.Enabled = true;
                            this.CmbRenglonForm5.SelectedValue = dtDatos.Rows[0]["idform_configuracion5"].ToString().Trim();
                            this.CmbSimbolo5.Enabled = true;
                            this.CmbSimbolo5.SelectedValue = dtDatos.Rows[0]["idtipo_operacion5"].ToString().Trim();
                            this.CmbRenglonForm6.Enabled = true;
                            this.CmbRenglonForm6.SelectedValue = dtDatos.Rows[0]["idform_configuracion6"].ToString().Trim();
                            #endregion
                        }
                        else
                        {
                            #region DESABILITAR LOS CONTROLES DE LISTAS
                            this.Validador13.Enabled = false;
                            this.Validador14.Enabled = false;
                            this.Validador15.Enabled = false;
                            this.CmbRenglonForm1.Enabled = false;
                            this.CmbRenglonForm1.SelectedValue = "";
                            this.CmbSimbolo.Enabled = false;
                            this.CmbSimbolo.SelectedValue = "";
                            this.CmbRenglonForm2.Enabled = false;
                            this.CmbRenglonForm2.SelectedValue = "";
                            this.CmbSimbolo2.Enabled = false;
                            this.CmbSimbolo2.SelectedValue = "";
                            this.CmbRenglonForm3.Enabled = false;
                            this.CmbRenglonForm3.SelectedValue = "";
                            this.CmbSimbolo3.Enabled = false;
                            this.CmbSimbolo3.SelectedValue = "";
                            this.CmbRenglonForm4.Enabled = false;
                            this.CmbRenglonForm4.SelectedValue = "";
                            this.CmbSimbolo4.Enabled = false;
                            this.CmbSimbolo4.SelectedValue = "";
                            this.CmbRenglonForm5.Enabled = false;
                            this.CmbRenglonForm5.SelectedValue = "";
                            this.CmbSimbolo5.Enabled = false;
                            this.CmbSimbolo5.SelectedValue = "";
                            this.CmbRenglonForm6.Enabled = false;
                            this.CmbRenglonForm6.SelectedValue = "";
                            #endregion
                        }

                        this.CmbUnidadMedida.SelectedValue = dtDatos.Rows[0]["idunidad_medida"].ToString().Trim();
                        this.LstUnidadesMedida();
                        this.CmbAnioFiscal.SelectedValue = dtDatos.Rows[0]["idvalor_unid_medida"].ToString().Trim() + "|" + dtDatos.Rows[0]["valor_unidad"].ToString().Trim();
                        this.CmbAnioGravable.SelectedValue = dtDatos.Rows[0]["idunid_medida_bg"].ToString().Trim() + "|" + dtDatos.Rows[0]["valor_unidad_bg"].ToString().Trim();
                        this.LstValorUnidadMedida();
                        this.CmbTipoTarifa.SelectedValue = dtDatos.Rows[0]["idtipo_tarifa"].ToString().Trim();
                        //this.LstTipoTarifa();

                        int _IdTipoTarifa = Int32.Parse(dtDatos.Rows[0]["idtipo_tarifa"].ToString().Trim());
                        //--
                        this.TxtCantidadUnidad.Text = dtDatos.Rows[0]["cantidad_medida"].ToString().Trim().Replace(",", ".");
                        this.TxtCantPeriodos.Text = dtDatos.Rows[0]["cantidad_periodos"].ToString().Trim().Replace(",", ".");
                        //double _CantidadMedida = Convert.ToDouble(this.TxtCantidadUnidad.Text.ToString().Trim());
                        //double _CantPeriodos = this.TxtCantPeriodos.Text.ToString().Trim().Length > 0 ? Convert.ToDouble(this.TxtCantPeriodos.Text.ToString().Trim()) : 0;
                        double _ValorUnidad = Convert.ToDouble(dtDatos.Rows[0]["valor_unidad"].ToString().Trim());
                        double _ValorUnidadMedida = Convert.ToDouble(dtDatos.Rows[0]["valor_unid_medida"].ToString().Trim());
                        double _CantidadUnidad = this.TxtCantidadUnidad.Text.ToString().Trim().Length > 0 ? Double.Parse(this.TxtCantidadUnidad.Text.ToString().Trim().Replace(".", ",")) : 0;
                        double _CantPeriodos = this.TxtCantPeriodos.Text.ToString().Trim().Length > 0 ? Double.Parse(this.TxtCantPeriodos.Text.ToString().Trim()) : 0;
                        //--
                        _log.Warn("CANTIDAD UNIDAD => " + _ValorUnidad + ", CANTIDAD UNIDAD ORIGINAL => " + this.TxtCantidadUnidad.Text.ToString().Trim() + ", CANTIDAD UNIDAD => " + _CantidadUnidad + ", CANTIDAD PERIODOS => " + _CantPeriodos);

                        double _TarifaMinima = 0;
                        if (_IdTipoTarifa == 1)         //--PORCENTUAL
                        {
                            _TarifaMinima = ((_CantidadUnidad * _ValorUnidad) / 100);
                        }
                        else if (_IdTipoTarifa == 8)    //--POR UNIDAD
                        {
                            _TarifaMinima = ((_CantidadUnidad * _ValorUnidad) * _CantPeriodos);
                        }

                        if (_ValorUnidadMedida > 0)
                        {
                            this.ChkValorUnidad.Checked = true;
                            this.TxtValorUnidad.Visible = true;
                            this.TxtValorUnidad.Text = _ValorUnidadMedida.ToString();
                            this.LblValorUnidad.Visible = false;
                            this.LblValorUnidad.Text = String.Format(String.Format("{0:$ ###,###,##0}", 0));
                        }
                        else
                        {
                            this.ChkValorUnidad.Checked = false;
                            this.TxtValorUnidad.Visible = false;
                            this.TxtValorUnidad.Text = "";
                            this.LblValorUnidad.Visible = true;
                            this.LblValorUnidad.Text = String.Format(String.Format("{0:$ ###,###,##0}", _ValorUnidad));
                        }
                        this.LblValorTarifaMinima.Text = String.Format(String.Format("{0:$ ###,###,##0}", _TarifaMinima));
                        this.TxtDescripcion.Text = dtDatos.Rows[0]["descripcion_configuracion"].ToString().Trim();
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

        #region LISTA DE COMBOBOX DEL FORMULARIO
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
                DataTable dtDatos = new DataTable();
                dtDatos = ObjFormConf.GetFormConfiguracionRenglon();

                //--# DE RENGLON DEL FORMULARIO PADRE
                this.CmbRenglonForm.DataSource = ObjFormConf.GetFormConfiguracion();
                this.CmbRenglonForm.DataValueField = "idformulario_configuracion";
                this.CmbRenglonForm.DataTextField = "numero_renglon";
                this.CmbRenglonForm.DataBind();

                //--RENGLON A CALCULAR 1
                this.CmbRenglonForm1.DataSource = dtDatos;
                this.CmbRenglonForm1.DataValueField = "idformulario_configuracion";
                this.CmbRenglonForm1.DataTextField = "numero_renglon";
                this.CmbRenglonForm1.DataBind();

                //--RENGLON A CALCULAR 2
                this.CmbRenglonForm2.DataSource = dtDatos;
                this.CmbRenglonForm2.DataValueField = "idformulario_configuracion";
                this.CmbRenglonForm2.DataTextField = "numero_renglon";
                this.CmbRenglonForm2.DataBind();

                //--RENGLON A CALCULAR 3
                this.CmbRenglonForm3.DataSource = dtDatos;
                this.CmbRenglonForm3.DataValueField = "idformulario_configuracion";
                this.CmbRenglonForm3.DataTextField = "numero_renglon";
                this.CmbRenglonForm3.DataBind();

                //--RENGLON A CALCULAR 4
                this.CmbRenglonForm4.DataSource = dtDatos;
                this.CmbRenglonForm4.DataValueField = "idformulario_configuracion";
                this.CmbRenglonForm4.DataTextField = "numero_renglon";
                this.CmbRenglonForm4.DataBind();

                //--RENGLON A CALCULAR 5
                this.CmbRenglonForm5.DataSource = dtDatos;
                this.CmbRenglonForm5.DataValueField = "idformulario_configuracion";
                this.CmbRenglonForm5.DataTextField = "numero_renglon";
                this.CmbRenglonForm5.DataBind();

                //--RENGLON A CALCULAR 4
                this.CmbRenglonForm6.DataSource = dtDatos;
                this.CmbRenglonForm6.DataValueField = "idformulario_configuracion";
                this.CmbRenglonForm6.DataTextField = "numero_renglon";
                this.CmbRenglonForm6.DataBind();
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

        protected void LstSimbolo()
        {
            try
            {
                ObjTipoOper.TipoConsulta = 2;
                ObjTipoOper.IdEstado = 1;
                ObjTipoOper.MostrarSeleccione = "SI";
                ObjTipoOper.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjTipoOper.GetTipoOperacion();
                //--LISTA DE OPERACIONES 1
                this.CmbSimbolo.DataSource = dtDatos;
                this.CmbSimbolo.DataValueField = "idtipo_operacion";
                this.CmbSimbolo.DataTextField = "simbolo";
                this.CmbSimbolo.DataBind();

                //--LISTA DE OPERACIONES 2
                this.CmbSimbolo2.DataSource = dtDatos;
                this.CmbSimbolo2.DataValueField = "idtipo_operacion";
                this.CmbSimbolo2.DataTextField = "simbolo";
                this.CmbSimbolo2.DataBind();

                //--LISTA DE OPERACIONES 3
                this.CmbSimbolo3.DataSource = dtDatos;
                this.CmbSimbolo3.DataValueField = "idtipo_operacion";
                this.CmbSimbolo3.DataTextField = "simbolo";
                this.CmbSimbolo3.DataBind();

                //--LISTA DE OPERACIONES 4
                this.CmbSimbolo4.DataSource = dtDatos;
                this.CmbSimbolo4.DataValueField = "idtipo_operacion";
                this.CmbSimbolo4.DataTextField = "simbolo";
                this.CmbSimbolo4.DataBind();

                //--LISTA DE OPERACIONES 5
                this.CmbSimbolo5.DataSource = dtDatos;
                this.CmbSimbolo5.DataValueField = "idtipo_operacion";
                this.CmbSimbolo5.DataTextField = "simbolo";
                this.CmbSimbolo5.DataBind();
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
                ObjValorUnidad.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                //--
                DataTable dtDatos = new DataTable();
                dtDatos = ObjValorUnidad.GetValorUnidadMedida();
                //--AÑO GRAVABLE
                this.CmbAnioGravable.DataSource = dtDatos;
                this.CmbAnioGravable.DataValueField = "idvalor_unid_medida";
                this.CmbAnioGravable.DataTextField = "anio_valor";
                this.CmbAnioGravable.DataBind();

                //--AÑO FISCAL
                this.CmbAnioFiscal.DataSource = dtDatos;
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
                this.ViewState["IdFormularioConfig"] = "";

                if (this.ViewState["TipoProceso"].ToString().Trim().Equals("UPDATE"))
                {
                    this.LblTitulo.Text = "EDITAR CONCEPTOS COMPLEMENTARIOS";
                    this.ViewState["IdMunTarifaMinima"] = Request.QueryString["IdMunTarifaMinima"].ToString().Trim();
                    this.ViewState["TipoProceso"] = 2;

                    //--AQUI LISTAMOS LOS COMBOBOX
                    this.LstTipoImpuesto();
                    //this.LstUnidadesMedida();
                    //this.LstValorUnidadMedida();
                    this.LstTipoTarifa();
                    this.LstSimbolo();
                    this.LstEstado();
                    ///---
                    this.GetInfoTarifa();
                }
                else
                {
                    this.LblTitulo.Text = "REGISTRAR CONCEPTOS COMPLEMENTARIOS";
                    this.ViewState["IdMunTarifaMinima"] = "";
                    this.ViewState["TipoProceso"] = 1;

                    //--AQUI LISTAMOS LOS COMBOBOX
                    this.LstTipoImpuesto();
                    this.LstUnidadesMedida();
                    this.LstValorUnidadMedida();
                    this.LstTipoTarifa();
                    this.LstSimbolo();
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

        protected void CmbRenglonForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbRenglonForm.SelectedValue.ToString().Trim().Length > 0)
                {
                    string[] _ArrayData = this.CmbRenglonForm.SelectedValue.ToString().Trim().Split('|');
                    this.TxtDescripcion.Text = _ArrayData[1].ToString().Trim();
                    this.ViewState["IdFormularioConfig"] = _ArrayData[0].ToString().Trim();
                }
                else
                {
                    this.ViewState["IdFormularioConfig"] = "";
                    this.TxtDescripcion.Text = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al mostrar la descripción del renglón. Motivo: " + ex.ToString();
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

        protected void ChkCalcular_CheckedChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            if (this.ChkCalcular.Checked)
            {
                this.Validador13.Enabled = true;
                this.Validador14.Enabled = false;
                this.Validador15.Enabled = false;
                this.CmbRenglonForm1.Enabled = true;
                this.CmbRenglonForm1.SelectedValue = "";
                this.CmbSimbolo.Enabled = true;
                this.CmbSimbolo.SelectedValue = "";
                this.CmbSimbolo2.Enabled = true;
                this.CmbSimbolo2.SelectedValue = "";
                this.CmbSimbolo3.Enabled = true;
                this.CmbSimbolo3.SelectedValue = "";
                this.CmbRenglonForm2.Enabled = true;
                this.CmbRenglonForm2.SelectedValue = "";
                this.CmbRenglonForm3.Enabled = true;
                this.CmbRenglonForm3.SelectedValue = "";
                this.CmbRenglonForm4.Enabled = true;
                this.CmbRenglonForm4.SelectedValue = "";
            }
            else
            {
                this.Validador13.Enabled = false;
                this.Validador14.Enabled = false;
                this.Validador15.Enabled = false;
                this.CmbRenglonForm1.Enabled = false;
                this.CmbRenglonForm1.SelectedValue = "";
                this.CmbSimbolo.Enabled = false;
                this.CmbSimbolo.SelectedValue = "";
                this.CmbSimbolo2.Enabled = false;
                this.CmbSimbolo2.SelectedValue = "";
                this.CmbSimbolo3.Enabled = false;
                this.CmbSimbolo3.SelectedValue = "";
                this.CmbRenglonForm2.Enabled = false;
                this.CmbRenglonForm2.SelectedValue = "";
                this.CmbRenglonForm3.Enabled = false;
                this.CmbRenglonForm3.SelectedValue = "";
                this.CmbRenglonForm4.Enabled = false;
                this.CmbRenglonForm4.SelectedValue = "";
            }
        }

        protected void CmbSimbolo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            if (this.CmbSimbolo.SelectedValue.ToString().Trim().Length > 0)
            {
                this.Validador14.Enabled = true;
                this.CmbRenglonForm2.Enabled = true;
                this.CmbRenglonForm2.SelectedValue = "";
                this.Validador15.Enabled = true;
                this.CmbSimbolo2.Enabled = true;
                this.CmbSimbolo2.SelectedValue = "";
                this.CmbRenglonForm3.Enabled = true;
                this.CmbRenglonForm3.SelectedValue = "";
                this.CmbSimbolo3.Enabled = true;
                this.CmbSimbolo3.SelectedValue = "";
                this.CmbRenglonForm4.Enabled = true;
                this.CmbRenglonForm4.SelectedValue = "";
                this.CmbRenglonForm2.Focus();
            }
            else
            {
                this.Validador14.Enabled = false;
                this.CmbRenglonForm2.Enabled = false;
                this.CmbRenglonForm2.SelectedValue = "";
                this.Validador15.Enabled = false;
                this.CmbSimbolo2.Enabled = false;
                this.CmbSimbolo2.SelectedValue = "";
                this.CmbRenglonForm3.Enabled = false;
                this.CmbRenglonForm3.SelectedValue = "";
                this.CmbSimbolo3.Enabled = false;
                this.CmbSimbolo3.SelectedValue = "";
                this.CmbRenglonForm4.Enabled = false;
                this.CmbRenglonForm4.SelectedValue = "";
            }
        }

        protected void CmbSimbolo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            if (this.CmbSimbolo2.SelectedValue.ToString().Trim().Length > 0)
            {
                this.Validador27.Enabled = true;
                this.CmbRenglonForm3.Enabled = true;
                this.CmbRenglonForm3.SelectedValue = "";
                this.CmbSimbolo3.Enabled = true;
                this.CmbSimbolo3.SelectedValue = "";
                this.CmbRenglonForm3.Focus();
            }
            else
            {
                this.Validador27.Enabled = false;
                this.Validador28.Enabled = false;
                this.CmbRenglonForm3.Enabled = false;
                this.CmbRenglonForm3.SelectedValue = "";
                this.CmbSimbolo3.Enabled = false;
                this.CmbSimbolo3.SelectedValue = "";
                this.CmbRenglonForm4.Enabled = false;
                this.CmbRenglonForm4.SelectedValue = "";
            }
        }

        protected void CmbSimbolo3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            if (this.CmbSimbolo3.SelectedValue.ToString().Trim().Length > 0)
            {
                this.Validador28.Enabled = true;
                this.Validador29.Enabled = true;
                this.CmbRenglonForm4.Enabled = true;
                this.CmbRenglonForm4.SelectedValue = "";
                this.CmbSimbolo4.Enabled = true;
                this.CmbSimbolo4.SelectedValue = "";
                this.CmbRenglonForm4.Focus();
            }
            else
            {
                this.Validador27.Enabled = false;
                this.Validador28.Enabled = false;
                this.CmbRenglonForm4.Enabled = false;
                this.CmbRenglonForm4.SelectedValue = "";
                this.CmbSimbolo4.Enabled = false;
                this.CmbSimbolo4.SelectedValue = "";
                this.CmbRenglonForm5.Enabled = false;
                this.CmbRenglonForm5.SelectedValue = "";
            }
        }

        protected void CmbSimbolo4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            if (this.CmbSimbolo4.SelectedValue.ToString().Trim().Length > 0)
            {
                this.Validador28.Enabled = true;
                this.Validador29.Enabled = true;
                this.CmbRenglonForm5.Enabled = true;
                this.CmbRenglonForm5.SelectedValue = "";
                this.CmbSimbolo5.Enabled = true;
                this.CmbSimbolo5.SelectedValue = "";
                this.CmbRenglonForm5.Focus();
            }
            else
            {
                this.Validador27.Enabled = false;
                this.Validador28.Enabled = false;
                this.CmbRenglonForm5.Enabled = false;
                this.CmbRenglonForm5.SelectedValue = "";
                this.CmbSimbolo5.Enabled = false;
                this.CmbSimbolo5.SelectedValue = "";
                this.CmbRenglonForm6.Enabled = false;
                this.CmbRenglonForm6.SelectedValue = "";
            }
        }

        protected void CmbSimbolo5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            if (this.CmbSimbolo5.SelectedValue.ToString().Trim().Length > 0)
            {
                this.Validador30.Enabled = true;
                this.CmbRenglonForm6.Enabled = true;
                this.CmbRenglonForm6.SelectedValue = "";
                this.CmbRenglonForm6.Focus();
            }
            else
            {
                this.Validador30.Enabled = false;
                this.CmbRenglonForm6.Enabled = false;
                this.CmbRenglonForm6.SelectedValue = "";
            }
        }

        protected void CmbUnidadMedida_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            //--
            if (this.CmbUnidadMedida.SelectedValue.ToString().Trim().Equals("4"))
            {
                this.ChkValorUnidad.Enabled = true;
                this.Validador31.Enabled = true;
            }
            else
            {
                this.ChkValorUnidad.Enabled = false;
                this.Validador31.Enabled = false;
            }
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
                    if (this.TxtCantidadUnidad.Text.ToString().Trim().Length > 0)
                    {
                        int _IdTipoTarifa = Int32.Parse(this.CmbTipoTarifa.SelectedValue.ToString().Trim());

                        if (Double.Parse(this.TxtCantidadUnidad.Text.ToString().Trim()) > 0)
                        {
                            //Recalculamos el valor de la tarifa minima.
                            double _Cantidad = Double.Parse(this.TxtCantidadUnidad.Text.ToString().Trim());
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
                            this.TxtCantidadUnidad.Focus();
                        }
                    }
                    else
                    {
                        this.TxtCantidadUnidad.Focus();
                    }
                }
                else
                {
                    this.CmbTipoTarifa.Focus();
                }
            }
        }

        protected void CmbAnioFiscal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CmbAnioFiscal.SelectedValue.ToString().Trim().Length > 0)
            {
                string[] _ArrayData = this.CmbAnioFiscal.SelectedValue.ToString().Trim().Split('|');
                double _ValorUnidad = Double.Parse(_ArrayData[1].ToString().Trim());
                this.LblValorUnidad.Text = String.Format(String.Format("{0:$ ###,###,##0}", _ValorUnidad));
                this.ViewState["IdUnidadMedida"] = _ArrayData[0].ToString().Trim();

                //--AQUI VALIDAMOS QUE SE ALLA SELECCIONADO UN TIPO DE TARIFA
                if (this.CmbTipoTarifa.SelectedValue.ToString().Trim().Length > 0)
                {
                    //--AQUI VALIDAMOS SI LA CANTIDAD TIENE UN VALOR PARA RECALCULAR LA TARIFA MINIMA
                    if (this.TxtCantidadUnidad.Text.ToString().Trim().Length > 0)
                    {
                        int _IdTipoTarifa = Int32.Parse(this.CmbTipoTarifa.SelectedValue.ToString().Trim());

                        if (Double.Parse(this.TxtCantidadUnidad.Text.ToString().Trim()) > 0)
                        {
                            //Recalculamos el valor de la tarifa minima.
                            double _Cantidad = Double.Parse(this.TxtCantidadUnidad.Text.ToString().Trim());
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
                            this.TxtCantidadUnidad.Focus();
                        }
                    }
                    else
                    {
                        this.TxtCantidadUnidad.Focus();
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

                    if (this.TxtCantidadUnidad.Text.ToString().Trim().Length > 0)
                    {
                        if (Double.Parse(this.TxtCantidadUnidad.Text.ToString().Trim()) > 0)
                        {
                            double _ValorUnidad = Double.Parse(this.LblValorUnidad.Text.ToString().Trim().Replace("$ ", "").Replace(".", ""));
                            double _Cantidad = Double.Parse(this.TxtCantidadUnidad.Text.ToString().Trim().Replace(".", ","));
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
                        this.TxtCantidadUnidad.Focus();
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

        protected void ChkValorUnidad_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkValorUnidad.Checked == true)
            {
                this.LblValorUnidad.Visible = false;
                this.TxtValorUnidad.Visible = true;
                this.Validador31.Enabled = true;
                this.TxtValorUnidad.Focus();
            }
            else
            {
                this.LblValorUnidad.Visible = true;
                this.TxtValorUnidad.Visible = false;
                this.TxtValorUnidad.Text = "";
                this.Validador31.Enabled = false;
            }
        }

        protected void TxtValorUnidad_TextChanged(object sender, EventArgs e)
        {
            this.GetValorConcepto();
        }

        protected void TxtCantidad_TextChanged(object sender, EventArgs e)
        {
            this.GetValorConcepto();
            //try
            //{
            //    if (this.CmbTipoTarifa.SelectedValue.ToString().Trim().Length > 0)
            //    {
            //        int _IdTipoTarifa = Int32.Parse(this.CmbTipoTarifa.SelectedValue.ToString().Trim());

            //        if (this.TxtCantidad.Text.ToString().Trim().Length > 0)
            //        {
            //            if (Double.Parse(this.TxtCantidad.Text.ToString().Trim()) >= 0)
            //            {
            //                double _ValorUnidad = 0;
            //                if (this.ChkValorUnidad.Checked == true)
            //                {
            //                    _ValorUnidad = Double.Parse(this.TxtValorUnidad.Text.ToString().Trim());
            //                }
            //                else
            //                {
            //                    _ValorUnidad = Double.Parse(this.LblValorUnidad.Text.ToString().Trim().Replace("$ ", "").Replace(".", ""));
            //                }
            //                double _Cantidad = this.TxtCantidad.Text.ToString().Trim().Length > 0 ? Double.Parse(this.TxtCantidad.Text.ToString().Trim().Replace(".", ",")) : 0;
            //                double _CantPeriodos = this.TxtCantPeriodos.Text.ToString().Trim().Length > 0 ? Double.Parse(this.TxtCantPeriodos.Text.ToString().Trim().Replace(".", ",")) : 0;
            //                double _ValorTarifaMinima = 0;

            //                if (_IdTipoTarifa == 1)
            //                {
            //                    _ValorTarifaMinima = ((_ValorUnidad * _Cantidad) / 100);
            //                }
            //                else
            //                {
            //                    _ValorTarifaMinima = ((_Cantidad * _ValorUnidad) * _CantPeriodos);
            //                }

            //                this.LblValorTarifaMinima.Text = String.Format(String.Format("{0:$ ###,###,##0}", _ValorTarifaMinima));
            //            }
            //            else
            //            {
            //                #region MOSTRAR MENSAJE DE USUARIO
            //                //Mostramos el mensaje porque se produjo un error con la Trx.
            //                this.RadWindowManager1.ReloadOnShow = true;
            //                this.RadWindowManager1.DestroyOnClose = true;
            //                this.RadWindowManager1.Windows.Clear();
            //                this.RadWindowManager1.Enabled = true;
            //                this.RadWindowManager1.EnableAjaxSkinRendering = true;
            //                this.RadWindowManager1.Visible = true;

            //                RadWindow Ventana = new RadWindow();
            //                Ventana.Modal = true;
            //                string _MsgMensaje = "Señor usuario, el valor de la cantidad debe ser mayor cero !";
            //                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
            //                Ventana.ID = "RadWindow2";
            //                Ventana.VisibleOnPageLoad = true;
            //                Ventana.Visible = true;
            //                Ventana.Height = Unit.Pixel(300);
            //                Ventana.Width = Unit.Pixel(600);
            //                Ventana.KeepInScreenBounds = true;
            //                Ventana.Title = "Mensaje del Sistema";
            //                Ventana.VisibleStatusbar = false;
            //                Ventana.Behaviors = WindowBehaviors.Close;
            //                this.RadWindowManager1.Windows.Add(Ventana);
            //                this.RadWindowManager1 = null;
            //                Ventana = null;
            //                #endregion
            //            }
            //        }
            //        else
            //        {
            //            #region MOSTRAR MENSAJE DE USUARIO
            //            //Mostramos el mensaje porque se produjo un error con la Trx.
            //            this.RadWindowManager1.ReloadOnShow = true;
            //            this.RadWindowManager1.DestroyOnClose = true;
            //            this.RadWindowManager1.Windows.Clear();
            //            this.RadWindowManager1.Enabled = true;
            //            this.RadWindowManager1.EnableAjaxSkinRendering = true;
            //            this.RadWindowManager1.Visible = true;

            //            RadWindow Ventana = new RadWindow();
            //            Ventana.Modal = true;
            //            string _MsgMensaje = "Señor usuario, debe ingresar un valor en la cantidad !";
            //            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
            //            Ventana.ID = "RadWindow2";
            //            Ventana.VisibleOnPageLoad = true;
            //            Ventana.Visible = true;
            //            Ventana.Height = Unit.Pixel(300);
            //            Ventana.Width = Unit.Pixel(600);
            //            Ventana.KeepInScreenBounds = true;
            //            Ventana.Title = "Mensaje del Sistema";
            //            Ventana.VisibleStatusbar = false;
            //            Ventana.Behaviors = WindowBehaviors.Close;
            //            this.RadWindowManager1.Windows.Add(Ventana);
            //            this.RadWindowManager1 = null;
            //            Ventana = null;
            //            #endregion
            //        }
            //    }
            //    else
            //    {
            //        #region MOSTRAR MENSAJE DE USUARIO
            //        //Mostramos el mensaje porque se produjo un error con la Trx.
            //        this.RadWindowManager1.ReloadOnShow = true;
            //        this.RadWindowManager1.DestroyOnClose = true;
            //        this.RadWindowManager1.Windows.Clear();
            //        this.RadWindowManager1.Enabled = true;
            //        this.RadWindowManager1.EnableAjaxSkinRendering = true;
            //        this.RadWindowManager1.Visible = true;

            //        RadWindow Ventana = new RadWindow();
            //        Ventana.Modal = true;
            //        string _MsgMensaje = "Señor usuario, debe seleccionar un tipo de tarifa !";
            //        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
            //        Ventana.ID = "RadWindow2";
            //        Ventana.VisibleOnPageLoad = true;
            //        Ventana.Visible = true;
            //        Ventana.Height = Unit.Pixel(300);
            //        Ventana.Width = Unit.Pixel(600);
            //        Ventana.KeepInScreenBounds = true;
            //        Ventana.Title = "Mensaje del Sistema";
            //        Ventana.VisibleStatusbar = false;
            //        Ventana.Behaviors = WindowBehaviors.Close;
            //        this.RadWindowManager1.Windows.Add(Ventana);
            //        this.RadWindowManager1 = null;
            //        Ventana = null;
            //        #endregion
            //    }
            //}
            //catch (Exception ex)
            //{
            //    #region MOSTRAR MENSAJE DE USUARIO
            //    //Mostramos el mensaje porque se produjo un error con la Trx.
            //    this.RadWindowManager1.ReloadOnShow = true;
            //    this.RadWindowManager1.DestroyOnClose = true;
            //    this.RadWindowManager1.Windows.Clear();
            //    this.RadWindowManager1.Enabled = true;
            //    this.RadWindowManager1.EnableAjaxSkinRendering = true;
            //    this.RadWindowManager1.Visible = true;

            //    RadWindow Ventana = new RadWindow();
            //    Ventana.Modal = true;
            //    string _MsgMensaje = "Error al calcular la tarifa mínima del municipio. Motivo: " + ex.ToString();
            //    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
            //    Ventana.ID = "RadWindow2";
            //    Ventana.VisibleOnPageLoad = true;
            //    Ventana.Visible = true;
            //    Ventana.Height = Unit.Pixel(300);
            //    Ventana.Width = Unit.Pixel(600);
            //    Ventana.KeepInScreenBounds = true;
            //    Ventana.Title = "Mensaje del Sistema";
            //    Ventana.VisibleStatusbar = false;
            //    Ventana.Behaviors = WindowBehaviors.Close;
            //    this.RadWindowManager1.Windows.Add(Ventana);
            //    this.RadWindowManager1 = null;
            //    Ventana = null;
            //    #endregion
            //}
        }

        protected void TxtCantPeriodos_TextChanged(object sender, EventArgs e)
        {
            this.GetValorConcepto();
        }

        private void GetValorConcepto()
        {
            try
            {
                if (this.CmbTipoTarifa.SelectedValue.ToString().Trim().Length > 0)
                {
                    int _IdTipoTarifa = Int32.Parse(this.CmbTipoTarifa.SelectedValue.ToString().Trim());

                    if (this.TxtCantPeriodos.Text.ToString().Trim().Length > 0)
                    {
                        if (Double.Parse(this.TxtCantPeriodos.Text.ToString().Trim()) >= 0)
                        {
                            double _ValorUnidad = 0;
                            if (this.ChkValorUnidad.Checked == true)
                            {
                                _ValorUnidad = Double.Parse(this.TxtValorUnidad.Text.ToString().Trim());
                            }
                            else
                            {
                                _ValorUnidad = Double.Parse(this.LblValorUnidad.Text.ToString().Trim().Replace("$ ", "").Replace(FixedData.SeparadorMilesAp, ""));
                            }

                            //double _CantidadUnidad = this.TxtCantidadUnidad.Text.ToString().Trim().Length > 0 ? Double.Parse(this.TxtCantidadUnidad.Text.ToString().Trim().Replace(FixedData.SeparadorDecimalesAp, ".")) : 0;
                            //double _CantPeriodos = this.TxtCantPeriodos.Text.ToString().Trim().Length > 0 ? Double.Parse(this.TxtCantPeriodos.Text.ToString().Trim().Replace(FixedData.SeparadorDecimalesAp, ".")) : 0;
                            double _CantidadUnidad = this.TxtCantidadUnidad.Text.ToString().Trim().Length > 0 ? Double.Parse(this.TxtCantidadUnidad.Text.ToString().Trim().Replace(".", ",")) : 0;
                            double _CantPeriodos = this.TxtCantPeriodos.Text.ToString().Trim().Length > 0 ? Double.Parse(this.TxtCantPeriodos.Text.ToString().Trim()) : 0;
                            double _ValorTarifaMinima = 0;
                            _log.Warn("CANTIDAD UNIDAD => " + _ValorUnidad + ", CANTIDAD UNIDAD ORIGINAL => " + this.TxtCantidadUnidad.Text.ToString().Trim() + ", CANTIDAD UNIDAD => " + _CantidadUnidad + ", CANTIDAD PERIODOS => " + _CantPeriodos);

                            if (_IdTipoTarifa == 1)
                            {
                                _ValorTarifaMinima = ((_ValorUnidad * _CantidadUnidad) / 100);
                            }
                            else
                            {
                                _ValorTarifaMinima = ((_CantidadUnidad * _ValorUnidad) * _CantPeriodos);
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
                //ObjMunTarMin.IdFormuConfiguracion = this.CmbRenglonForm.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm.SelectedValue.ToString().Trim() : null;
                ObjMunTarMin.IdFormuConfiguracion = this.ViewState["IdFormularioConfig"].ToString().Trim().Length > 0 ? this.ViewState["IdFormularioConfig"].ToString().Trim() : null;
                ObjMunTarMin.CalcularRenglon = this.ChkCalcular.Checked == true ? "S" : "N";
                //--CALCULO DE RENGLONES
                ObjMunTarMin.IdFormuConfiguracion1 = this.CmbRenglonForm1.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm1.SelectedValue.ToString().Trim() : null;
                ObjMunTarMin.IdTipoOperacion1 = this.CmbSimbolo.SelectedValue.ToString().Trim().Length > 0 ? this.CmbSimbolo.SelectedValue.ToString().Trim() : null;
                ObjMunTarMin.IdFormuConfiguracion2 = this.CmbRenglonForm2.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm2.SelectedValue.ToString().Trim() : null;
                ObjMunTarMin.IdTipoOperacion2 = this.CmbSimbolo2.SelectedValue.ToString().Trim().Length > 0 ? this.CmbSimbolo2.SelectedValue.ToString().Trim() : null;
                ObjMunTarMin.IdFormuConfiguracion3 = this.CmbRenglonForm3.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm3.SelectedValue.ToString().Trim() : null;
                ObjMunTarMin.IdTipoOperacion3 = this.CmbSimbolo3.SelectedValue.ToString().Trim().Length > 0 ? this.CmbSimbolo3.SelectedValue.ToString().Trim() : null;
                ObjMunTarMin.IdFormuConfiguracion4 = this.CmbRenglonForm4.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm4.SelectedValue.ToString().Trim() : null;
                ObjMunTarMin.IdTipoOperacion4 = this.CmbSimbolo4.SelectedValue.ToString().Trim().Length > 0 ? this.CmbSimbolo4.SelectedValue.ToString().Trim() : null;
                ObjMunTarMin.IdFormuConfiguracion5 = this.CmbRenglonForm5.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm5.SelectedValue.ToString().Trim() : null;
                ObjMunTarMin.IdTipoOperacion5 = this.CmbSimbolo5.SelectedValue.ToString().Trim().Length > 0 ? this.CmbSimbolo5.SelectedValue.ToString().Trim() : null;
                ObjMunTarMin.IdFormuConfiguracion6 = this.CmbRenglonForm6.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm6.SelectedValue.ToString().Trim() : null;
                //--
                ObjMunTarMin.IdUnidadMedida = this.CmbUnidadMedida.SelectedValue.ToString().Trim();
                string[] _ArrayDatos = this.CmbAnioFiscal.SelectedValue.ToString().Trim().Split('|');
                ObjMunTarMin.IdValorUnidadMedida = _ArrayDatos[0].ToString().Trim();
                string[] _ArrayBaseGrav = this.CmbAnioGravable.SelectedValue.ToString().Trim().Split('|');
                ObjMunTarMin.IdUnidadMedidaBaseGravable = _ArrayBaseGrav[0].ToString().Trim();
                ObjMunTarMin.IdTipoTarifa = this.CmbTipoTarifa.SelectedValue.ToString().Trim();
                ObjMunTarMin.ValorConcepto = this.TxtValorUnidad.Text.ToString().Trim().Length > 0 ? this.TxtValorUnidad.Text.ToString().Trim().Replace(",", ".") : "0";
                ObjMunTarMin.CantidadMedida = this.TxtCantidadUnidad.Text.ToString().Trim().Replace(",", ".");
                ObjMunTarMin.CantidadPeriodos = this.TxtCantPeriodos.Text.ToString().Trim().Replace(",", ".");
                ObjMunTarMin.Descripcion = ObjUtils.GetLimpiarCadena(this.TxtDescripcion.Text.ToString().Trim().ToUpper());
                ObjMunTarMin.IdEstado = this.CmbEstado.SelectedValue.ToString().Trim();
                ObjMunTarMin.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                ObjMunTarMin.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjMunTarMin.TipoProceso = Int32.Parse(this.ViewState["TipoProceso"].ToString().Trim()) == 1 ? 1 : 2;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjMunTarMin);
                _log.Warn("REQUEST OTRAS CONFIG. MUNICIPIO => " + jsonRequest);

                int _IdRegistro = 0;
                string _MsgError = "";
                if (ObjMunTarMin.AddUpMunTarMinima(ref _IdRegistro, ref _MsgError))
                {
                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    if (ObjMunTarMin.TipoProceso == 1)
                    {
                        ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                    }
                    else
                    {
                        ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                    }
                    ObjAuditoria.ModuloApp = "OTRAS_CONF_MUNICIPIO";
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