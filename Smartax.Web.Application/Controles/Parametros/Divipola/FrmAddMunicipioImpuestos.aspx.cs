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
    public partial class FrmAddMunicipioImpuestos : System.Web.UI.Page
    {
        #region OBJETOS DE CLASES
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        MunicipioImpuestos ObjMunImpuesto = new MunicipioImpuestos();
        FormularioImpuesto ObjFrmImpuesto = new FormularioImpuesto();
        FormConfiguracion ObjFormConf = new FormConfiguracion();
        PeriodicidadPagos ObjPeriodicidad = new PeriodicidadPagos();
        TiposTarifa ObjTipoTarifa = new TiposTarifa();
        TipoOperacion ObjTipoOper = new TipoOperacion();
        Lista objLista = new Lista();
        Estado ObjEstado = new Estado();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        #endregion

        private void GetInfoImpuesto()
        {
            try
            {
                ObjMunImpuesto.TipoConsulta = 2;
                ObjMunImpuesto.IdMunImpuesto = Int32.Parse(this.ViewState["IdMunImpuesto"].ToString().Trim());
                ObjMunImpuesto.IdMunicipio = Int32.Parse(this.ViewState["IdMunicipio"].ToString().Trim());
                ObjMunImpuesto.IdEstado = null;
                ObjMunImpuesto.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjMunImpuesto.GetInfoMunImpuesto();

                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        this.CmbTipoImpuesto.SelectedValue = dtDatos.Rows[0]["idformulario_impuesto"].ToString().Trim();
                        this.LstTipoImpuesto();
                        this.ViewState["IdFormularioConfig"] = dtDatos.Rows[0]["idform_configuracion"].ToString().Trim();
                        this.LstFormConfiguracion();
                        this.CmbRenglonForm.SelectedValue = dtDatos.Rows[0]["idform_configuracion"].ToString().Trim() + "|" + dtDatos.Rows[0]["descripcion_renglon"].ToString().Trim();
                        this.CmbAnioGravable.SelectedValue = dtDatos.Rows[0]["anio_gravable"].ToString().Trim();
                        //this.CmbRenglonForm.SelectedItem.Text = dtDatos.Rows[0]["numero_renglon"].ToString().Trim();
                        this.LstPeriodicidad();
                        this.CmbPeriodicidad.SelectedValue = dtDatos.Rows[0]["id_periodicidad"].ToString().Trim();
                        this.CmbTipoTarifa.SelectedValue = dtDatos.Rows[0]["idtipo_tarifa"].ToString().Trim();
                        this.LstTipoTarifa();

                        bool ChkCalcular = Boolean.Parse(dtDatos.Rows[0]["calcular_renglon"].ToString().Trim());
                        this.ChkCalcular.Checked = ChkCalcular;

                        //--AQUI VALIDAMOS EL CHEK A VALIDAR
                        if (ChkCalcular == true)
                        {
                            #region LLENAR LA LISTA DESPLEGABLE
                            this.LstFormConfiguracionOperacion();
                            this.Validador13.Enabled = false;
                            this.Validador14.Enabled = false;
                            this.Validador15.Enabled = false;
                            this.CmbRenglonForm1.Enabled = true;
                            this.CmbRenglonForm1.SelectedValue = dtDatos.Rows[0]["idform_configuracion1"].ToString().Trim();
                            this.CmbSimbolo.Enabled = true;
                            this.CmbSimbolo.SelectedValue = dtDatos.Rows[0]["idtipo_operacion1"].ToString().Trim();
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
                            this.LstFormConfiguracionOperacion();
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

                        this.TxtDescripcion.Text = dtDatos.Rows[0]["descripcion_renglon"].ToString().Trim();
                        double _ValorTarifa = Double.Parse(dtDatos.Rows[0]["valor_tarifa"].ToString().Trim());
                        this.TxtValorTarifa.Text = _ValorTarifa.ToString().Trim().Replace(",", ".");
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
                dtDatos = ObjFormConf.GetFormConfiguracion(); //--ObjFormConf.GetFormConfiguracionRenglon();

                this.CmbRenglonForm.DataSource = dtDatos; //--ObjFormConf.GetFormConfiguracion();
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

        protected void LstFormConfiguracionOperacion()
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
                #endregion
            }
        }

        protected void LstAnioGravable()
        {
            try
            {
                objLista.MostrarSeleccione = "SI";
                this.CmbAnioGravable.DataSource = objLista.GetAnios();
                this.CmbAnioGravable.DataValueField = "id_anio";
                this.CmbAnioGravable.DataTextField = "descripcion_anio";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los años gravable. Motivo: " + ex.ToString();
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

        protected void LstPeriodicidad()
        {
            try
            {
                ObjPeriodicidad.TipoConsulta = 2;
                ObjPeriodicidad.IdFormularioImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim();
                ObjPeriodicidad.IdEstado = 1;
                ObjPeriodicidad.MostrarSeleccione = "SI";
                ObjPeriodicidad.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbPeriodicidad.DataSource = ObjPeriodicidad.GetPeriodicidadPagos();
                this.CmbPeriodicidad.DataValueField = "id_periodicidad";
                this.CmbPeriodicidad.DataTextField = "descripcion_periodicidad";
                this.CmbPeriodicidad.DataBind();
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

        protected void LstTipoTarifa()
        {
            try
            {
                ObjTipoTarifa.TipoConsulta = 3;
                ObjTipoTarifa.Interfaz = 1;
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
                //--LISTAR COMBOX RENGLONES + OPERACION
                this.LstSimbolo();
                this.LstAnioGravable();

                if (this.ViewState["TipoProceso"].ToString().Trim().Equals("UPDATE"))
                {
                    this.LblTitulo.Text = "EDITAR IMPUESTO DEL MUNICIPIO";
                    this.ViewState["IdMunImpuesto"] = Request.QueryString["IdMunImpuesto"].ToString().Trim();
                    this.ViewState["TipoProceso"] = 2;
                    //--
                    this.GetInfoImpuesto();
                }
                else
                {
                    this.LblTitulo.Text = "REGISTRAR IMPUESTOS DEL MUNICIPIO";
                    this.ViewState["IdMunImpuesto"] = "";
                    this.ViewState["TipoProceso"] = 1;

                    //--AQUI LISTAMOS LOS COMBOBOX
                    this.LstTipoImpuesto();
                    //this.LstPeriodicidad();
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

            //--LISTAR POR FILTROS DEL TIPO DE IMPUESTO
            this.LstPeriodicidad();
            this.LstFormConfiguracion();
            this.LstFormConfiguracionOperacion();
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

        protected void CmbTipoTarifa_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            if (this.CmbTipoTarifa.SelectedValue.ToString().Trim().Length > 0)
            {
                if (this.CmbTipoTarifa.SelectedValue.ToString().Trim().Equals("6"))
                {
                    this.TxtValorTarifa.Enabled = false;
                    this.TxtValorTarifa.Text = "0";
                }
                else
                {
                    this.TxtValorTarifa.Enabled = true;
                    this.TxtValorTarifa.Text = "";
                    this.TxtValorTarifa.Focus();
                }
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
                this.Validador13.Enabled = false;
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
                //this.Validador29.Enabled = true;
                this.CmbRenglonForm4.Enabled = true;
                this.CmbRenglonForm4.SelectedValue = "";
                this.CmbSimbolo4.Enabled = true;
                this.CmbSimbolo4.SelectedValue = "";
                this.CmbRenglonForm4.Focus();
            }
            else
            {
                //this.Validador27.Enabled = false;
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
                this.Validador29.Enabled = true;
                this.CmbRenglonForm5.Enabled = true;
                this.CmbRenglonForm5.SelectedValue = "";
                this.CmbSimbolo5.Enabled = true;
                this.CmbSimbolo5.SelectedValue = "";
                this.CmbRenglonForm5.Focus();
            }
            else
            {
                this.Validador29.Enabled = false;
                this.Validador30.Enabled = false;
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
        #endregion

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ObjMunImpuesto.IdMunImpuesto = this.ViewState["IdMunImpuesto"].ToString().Trim().Length > 0 ? this.ViewState["IdMunImpuesto"].ToString().Trim() : null;
                ObjMunImpuesto.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                ObjMunImpuesto.IdFormularioImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim();
                ObjMunImpuesto.AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim();
                ObjMunImpuesto.IdPeriodicidad = this.CmbPeriodicidad.SelectedValue.ToString().Trim();

                //--TOMAR LA DESCRIPCION
                string _DescripcionImpuesto1 = this.TxtDescripcion.Text.ToString().Trim();
                string _DescripcionImpuesto2 = _DescripcionImpuesto1.Replace("Ñ", "N").Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U");
                string _DescripcionImpuesto = _DescripcionImpuesto2.Replace("ñ", "n").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
                ObjMunImpuesto.DescripcionImpuesto = _DescripcionImpuesto;
                ObjMunImpuesto.OperacionRenglon = null;
                //--CALCULO DE RENGLONES
                ObjMunImpuesto.CalcularRenglon = this.ChkCalcular.Checked == true ? "S" : "N";
                ObjMunImpuesto.IdFormularioConfig = this.ViewState["IdFormularioConfig"].ToString().Trim();
                ObjMunImpuesto.IdFormuConfiguracion1 = this.CmbRenglonForm1.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm1.SelectedValue.ToString().Trim() : null;
                ObjMunImpuesto.IdTipoOperacion1 = this.CmbSimbolo.SelectedValue.ToString().Trim().Length > 0 ? this.CmbSimbolo.SelectedValue.ToString().Trim() : null;
                ObjMunImpuesto.IdFormuConfiguracion2 = this.CmbRenglonForm2.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm2.SelectedValue.ToString().Trim() : null;
                ObjMunImpuesto.IdTipoOperacion2 = this.CmbSimbolo2.SelectedValue.ToString().Trim().Length > 0 ? this.CmbSimbolo2.SelectedValue.ToString().Trim() : null;
                ObjMunImpuesto.IdFormuConfiguracion3 = this.CmbRenglonForm3.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm3.SelectedValue.ToString().Trim() : null;
                ObjMunImpuesto.IdTipoOperacion3 = this.CmbSimbolo3.SelectedValue.ToString().Trim().Length > 0 ? this.CmbSimbolo3.SelectedValue.ToString().Trim() : null;
                ObjMunImpuesto.IdFormuConfiguracion4 = this.CmbRenglonForm4.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm4.SelectedValue.ToString().Trim() : null;
                ObjMunImpuesto.IdTipoOperacion4 = this.CmbSimbolo4.SelectedValue.ToString().Trim().Length > 0 ? this.CmbSimbolo4.SelectedValue.ToString().Trim() : null;
                ObjMunImpuesto.IdFormuConfiguracion5 = this.CmbRenglonForm5.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm5.SelectedValue.ToString().Trim() : null;
                ObjMunImpuesto.IdTipoOperacion5 = this.CmbSimbolo5.SelectedValue.ToString().Trim().Length > 0 ? this.CmbSimbolo5.SelectedValue.ToString().Trim() : null;
                ObjMunImpuesto.IdFormuConfiguracion6 = this.CmbRenglonForm6.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglonForm6.SelectedValue.ToString().Trim() : null;
                //--
                ObjMunImpuesto.IdTipoTarifa = this.CmbTipoTarifa.SelectedValue.ToString().Trim();
                ObjMunImpuesto.ValorTarifa = this.TxtValorTarifa.Text.ToString().Trim().Replace(",", ".");
                ObjMunImpuesto.IdEstado = this.CmbEstado.SelectedValue.ToString().Trim();
                ObjMunImpuesto.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                ObjMunImpuesto.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjMunImpuesto.TipoProceso = Int32.Parse(this.ViewState["TipoProceso"].ToString().Trim()) == 1 ? 1 : 2;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjMunImpuesto);
                _log.Warn("REQUEST MUNICIPIO IMPUESTOS => " + jsonRequest);

                int _IdRegistro = 0;
                string _MsgError = "";
                if (ObjMunImpuesto.AddUpMunImpuesto(ref _IdRegistro, ref _MsgError))
                {
                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    if (ObjMunImpuesto.TipoProceso == 1)
                    {
                        ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                    }
                    else
                    {
                        ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                    }
                    ObjAuditoria.ModuloApp = "MUNICIPIO_IMPUESTO";
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