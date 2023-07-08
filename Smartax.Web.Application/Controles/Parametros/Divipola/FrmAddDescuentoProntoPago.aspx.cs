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
    public partial class FrmAddDescuentoProntoPago : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        #region DEFINICION DE OBJETOS DE CLASE
        MunicipioDescuento ObjMunDescuento = new MunicipioDescuento();
        FormularioImpuesto ObjFrmImpuesto = new FormularioImpuesto();
        FormConfiguracion ObjFormConf = new FormConfiguracion();
        TipoOperacion ObjTipoOper = new TipoOperacion();
        Lista ObjLista = new Lista();
        Estado ObjEstado = new Estado();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        #endregion

        private void GetInfoTarifa()
        {
            try
            {
                ObjMunDescuento.TipoConsulta = 2;
                ObjMunDescuento.IdMunDescuento = Int32.Parse(this.ViewState["IdMunDescuento"].ToString().Trim());
                ObjMunDescuento.IdMunicipio = Int32.Parse(this.ViewState["IdMunicipio"].ToString().Trim());
                ObjMunDescuento.IdCliente = null;
                ObjMunDescuento.IdFormularioImpuesto = null;
                ObjMunDescuento.AnioGravable = -1;
                ObjMunDescuento.IdEstado = null;
                ObjMunDescuento.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjMunDescuento.GetInfoDescuento();

                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        this.CmbTipoImpuesto.SelectedValue = dtDatos.Rows[0]["idformulario_impuesto"].ToString().Trim();
                        this.CmbAnioGravable.SelectedValue = dtDatos.Rows[0]["anio_gravable"].ToString().Trim();
                        this.TxtDescripcion.Text = dtDatos.Rows[0]["descripcion_descuento"].ToString().Trim();
                        //this.LstTipoImpuesto();
                        this.LstFormConfiguracion();
                        //--------DATOS DE LOS RENGLONES
                        this.CmbRenglon1.SelectedValue = dtDatos.Rows[0]["idform_configuracion1"].ToString().Trim();
                        this.CmbTipoOperacion1.SelectedValue = dtDatos.Rows[0]["idtipo_operacion1"].ToString().Trim();
                        this.CmbTipoOperacion1.Enabled = dtDatos.Rows[0]["idtipo_operacion1"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbRenglon2.SelectedValue = dtDatos.Rows[0]["idform_configuracion2"].ToString().Trim();
                        this.CmbRenglon2.Enabled = dtDatos.Rows[0]["idform_configuracion2"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbTipoOperacion2.SelectedValue = dtDatos.Rows[0]["idtipo_operacion2"].ToString().Trim();
                        this.CmbTipoOperacion2.Enabled = dtDatos.Rows[0]["idtipo_operacion2"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbRenglon3.SelectedValue = dtDatos.Rows[0]["idform_configuracion3"].ToString().Trim();
                        this.CmbRenglon3.Enabled = dtDatos.Rows[0]["idform_configuracion3"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbTipoOperacion3.SelectedValue = dtDatos.Rows[0]["idtipo_operacion3"].ToString().Trim();
                        this.CmbTipoOperacion3.Enabled = dtDatos.Rows[0]["idtipo_operacion3"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbRenglon4.SelectedValue = dtDatos.Rows[0]["idform_configuracion4"].ToString().Trim();
                        this.CmbRenglon4.Enabled = dtDatos.Rows[0]["idform_configuracion4"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbTipoOperacion4.SelectedValue = dtDatos.Rows[0]["idtipo_operacion4"].ToString().Trim();
                        this.CmbTipoOperacion4.Enabled = dtDatos.Rows[0]["idtipo_operacion4"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbRenglon5.SelectedValue = dtDatos.Rows[0]["idform_configuracion5"].ToString().Trim();
                        this.CmbRenglon5.Enabled = dtDatos.Rows[0]["idform_configuracion5"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbTipoOperacion5.SelectedValue = dtDatos.Rows[0]["idtipo_operacion5"].ToString().Trim();
                        this.CmbTipoOperacion5.Enabled = dtDatos.Rows[0]["idtipo_operacion5"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbRenglon6.SelectedValue = dtDatos.Rows[0]["idform_configuracion6"].ToString().Trim();
                        this.CmbRenglon6.Enabled = dtDatos.Rows[0]["idform_configuracion6"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbTipoOperacion6.SelectedValue = dtDatos.Rows[0]["idtipo_operacion6"].ToString().Trim();
                        this.CmbTipoOperacion6.Enabled = dtDatos.Rows[0]["idtipo_operacion6"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbRenglon7.SelectedValue = dtDatos.Rows[0]["idform_configuracion7"].ToString().Trim();
                        this.CmbRenglon7.Enabled = dtDatos.Rows[0]["idform_configuracion7"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbTipoOperacion7.SelectedValue = dtDatos.Rows[0]["idtipo_operacion7"].ToString().Trim();
                        this.CmbTipoOperacion7.Enabled = dtDatos.Rows[0]["idtipo_operacion7"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbRenglon8.SelectedValue = dtDatos.Rows[0]["idform_configuracion8"].ToString().Trim();
                        this.CmbRenglon8.Enabled = dtDatos.Rows[0]["idform_configuracion8"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbTipoOperacion8.SelectedValue = dtDatos.Rows[0]["idtipo_operacion8"].ToString().Trim();
                        this.CmbTipoOperacion8.Enabled = dtDatos.Rows[0]["idtipo_operacion8"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbRenglon9.SelectedValue = dtDatos.Rows[0]["idform_configuracion9"].ToString().Trim();
                        this.CmbRenglon9.Enabled = dtDatos.Rows[0]["idform_configuracion9"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbTipoOperacion9.SelectedValue = dtDatos.Rows[0]["idtipo_operacion9"].ToString().Trim();
                        this.CmbTipoOperacion9.Enabled = dtDatos.Rows[0]["idtipo_operacion9"].ToString().Trim().Length > 0 ? true : false;
                        this.CmbRenglon10.SelectedValue = dtDatos.Rows[0]["idform_configuracion10"].ToString().Trim();
                        this.CmbRenglon10.Enabled = dtDatos.Rows[0]["idform_configuracion10"].ToString().Trim().Length > 0 ? true : false;
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
                ObjFrmImpuesto.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

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

        protected void LstAnioGravable()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
                //--LISTAR LOS TIPOS DE OPERACION 1
                this.CmbAnioGravable.DataSource = ObjLista.GetAnios();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los años gravables. Motivo: " + ex.ToString();
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

                //--RENGLON A CALCULAR 1
                this.CmbRenglon1.DataSource = dtDatos;
                this.CmbRenglon1.DataValueField = "idformulario_configuracion";
                this.CmbRenglon1.DataTextField = "numero_renglon";
                this.CmbRenglon1.DataBind();

                //--RENGLON A CALCULAR 2
                this.CmbRenglon2.DataSource = dtDatos;
                this.CmbRenglon2.DataValueField = "idformulario_configuracion";
                this.CmbRenglon2.DataTextField = "numero_renglon";
                this.CmbRenglon2.DataBind();

                //--RENGLON A CALCULAR 3
                this.CmbRenglon3.DataSource = dtDatos;
                this.CmbRenglon3.DataValueField = "idformulario_configuracion";
                this.CmbRenglon3.DataTextField = "numero_renglon";
                this.CmbRenglon3.DataBind();

                //--RENGLON A CALCULAR 4
                this.CmbRenglon4.DataSource = dtDatos;
                this.CmbRenglon4.DataValueField = "idformulario_configuracion";
                this.CmbRenglon4.DataTextField = "numero_renglon";
                this.CmbRenglon4.DataBind();

                //--RENGLON A CALCULAR 5
                this.CmbRenglon5.DataSource = dtDatos;
                this.CmbRenglon5.DataValueField = "idformulario_configuracion";
                this.CmbRenglon5.DataTextField = "numero_renglon";
                this.CmbRenglon5.DataBind();

                //--RENGLON A CALCULAR 6
                this.CmbRenglon6.DataSource = dtDatos;
                this.CmbRenglon6.DataValueField = "idformulario_configuracion";
                this.CmbRenglon6.DataTextField = "numero_renglon";
                this.CmbRenglon6.DataBind();

                //--RENGLON A CALCULAR 7
                this.CmbRenglon7.DataSource = dtDatos;
                this.CmbRenglon7.DataValueField = "idformulario_configuracion";
                this.CmbRenglon7.DataTextField = "numero_renglon";
                this.CmbRenglon7.DataBind();

                //--RENGLON A CALCULAR 8
                this.CmbRenglon8.DataSource = dtDatos;
                this.CmbRenglon8.DataValueField = "idformulario_configuracion";
                this.CmbRenglon8.DataTextField = "numero_renglon";
                this.CmbRenglon8.DataBind();

                //--RENGLON A CALCULAR 9
                this.CmbRenglon9.DataSource = dtDatos;
                this.CmbRenglon9.DataValueField = "idformulario_configuracion";
                this.CmbRenglon9.DataTextField = "numero_renglon";
                this.CmbRenglon9.DataBind();

                //--RENGLON A CALCULAR 10
                this.CmbRenglon10.DataSource = dtDatos;
                this.CmbRenglon10.DataValueField = "idformulario_configuracion";
                this.CmbRenglon10.DataTextField = "numero_renglon";
                this.CmbRenglon10.DataBind();
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

                #region AQUI LLENAMOS CADA UNO DE LOS TIPO DE OPERACION
                //--LISTAR LOS TIPOS DE OPERACION 1
                this.CmbTipoOperacion1.DataSource = dtDatos;
                this.CmbTipoOperacion1.DataValueField = "idtipo_operacion";
                this.CmbTipoOperacion1.DataTextField = "simbolo";
                this.CmbTipoOperacion1.DataBind();

                //--LISTAR LOS TIPOS DE OPERACION 2
                this.CmbTipoOperacion2.DataSource = dtDatos;
                this.CmbTipoOperacion2.DataValueField = "idtipo_operacion";
                this.CmbTipoOperacion2.DataTextField = "simbolo";
                this.CmbTipoOperacion2.DataBind();

                //--LISTAR LOS TIPOS DE OPERACION 3
                this.CmbTipoOperacion3.DataSource = dtDatos;
                this.CmbTipoOperacion3.DataValueField = "idtipo_operacion";
                this.CmbTipoOperacion3.DataTextField = "simbolo";
                this.CmbTipoOperacion3.DataBind();

                //--LISTAR LOS TIPOS DE OPERACION 4
                this.CmbTipoOperacion4.DataSource = dtDatos;
                this.CmbTipoOperacion4.DataValueField = "idtipo_operacion";
                this.CmbTipoOperacion4.DataTextField = "simbolo";
                this.CmbTipoOperacion4.DataBind();

                //--LISTAR LOS TIPOS DE OPERACION 5
                this.CmbTipoOperacion5.DataSource = dtDatos;
                this.CmbTipoOperacion5.DataValueField = "idtipo_operacion";
                this.CmbTipoOperacion5.DataTextField = "simbolo";
                this.CmbTipoOperacion5.DataBind();

                //--LISTAR LOS TIPOS DE OPERACION 6
                this.CmbTipoOperacion6.DataSource = dtDatos;
                this.CmbTipoOperacion6.DataValueField = "idtipo_operacion";
                this.CmbTipoOperacion6.DataTextField = "simbolo";
                this.CmbTipoOperacion6.DataBind();

                //--LISTAR LOS TIPOS DE OPERACION 7
                this.CmbTipoOperacion7.DataSource = dtDatos;
                this.CmbTipoOperacion7.DataValueField = "idtipo_operacion";
                this.CmbTipoOperacion7.DataTextField = "simbolo";
                this.CmbTipoOperacion7.DataBind();

                //--LISTAR LOS TIPOS DE OPERACION 8
                this.CmbTipoOperacion8.DataSource = dtDatos;
                this.CmbTipoOperacion8.DataValueField = "idtipo_operacion";
                this.CmbTipoOperacion8.DataTextField = "simbolo";
                this.CmbTipoOperacion8.DataBind();

                //--LISTAR LOS TIPOS DE OPERACION 9
                this.CmbTipoOperacion9.DataSource = dtDatos;
                this.CmbTipoOperacion9.DataValueField = "idtipo_operacion";
                this.CmbTipoOperacion9.DataTextField = "simbolo";
                this.CmbTipoOperacion9.DataBind();
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
                    this.LblTitulo.Text = "EDITAR DESCUENTO DE PRONTO PAGO POR MUNICIPIO";
                    this.ViewState["IdMunDescuento"] = Request.QueryString["IdMunDescuento"].ToString().Trim();
                    this.ViewState["TipoProceso"] = 2;

                    //--AQUI LISTAMOS LOS COMBOBOX
                    this.LstTipoImpuesto();
                    this.LstAnioGravable();
                    this.LstSimbolo();
                    this.LstEstado();
                    ///---
                    this.GetInfoTarifa();
                }
                else
                {
                    this.LblTitulo.Text = "REGISTRAR DESCUENTO DE PRONTO PAGO POR MUNICIPIO";
                    this.ViewState["IdMunDescuento"] = "";
                    this.ViewState["TipoProceso"] = 1;

                    //--AQUI LISTAMOS LOS COMBOBOX
                    this.LstTipoImpuesto();
                    this.LstAnioGravable();
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

        #region EVENTOS POSTBACK DE CONTROLES DEL FORM
        protected void CmbTipoImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
            this.RadWindowManager1.Enabled = false;
            this.RadWindowManager1.EnableAjaxSkinRendering = false;
            this.RadWindowManager1.Visible = false;

            this.LstFormConfiguracion();
        }

        private void DesabilitarControles()
        {
            this.CmbTipoOperacion1.Enabled = false;
            this.CmbTipoOperacion1.SelectedValue = "";
            this.CmbRenglon2.Enabled = false;
            this.CmbTipoOperacion2.Enabled = false;
            this.CmbRenglon2.SelectedValue = "";
            this.CmbRenglon3.SelectedValue = "";
            this.CmbTipoOperacion1.Enabled = false;
            this.CmbTipoOperacion3.Enabled = false;
            this.CmbRenglon3.SelectedValue = "";
            this.CmbTipoOperacion3.SelectedValue = "";
            this.CmbRenglon4.Enabled = false;
            this.CmbTipoOperacion4.Enabled = false;
            this.CmbRenglon4.SelectedValue = "";
            this.CmbTipoOperacion4.SelectedValue = "";
            this.CmbRenglon5.Enabled = false;
            this.CmbTipoOperacion5.Enabled = false;
            this.CmbRenglon5.SelectedValue = "";
            this.CmbTipoOperacion5.SelectedValue = "";
            this.CmbRenglon6.Enabled = false;
            this.CmbTipoOperacion6.Enabled = false;
            this.CmbRenglon6.SelectedValue = "";
            this.CmbTipoOperacion6.SelectedValue = "";
            this.CmbRenglon7.Enabled = false;
            this.CmbTipoOperacion7.Enabled = false;
            this.CmbRenglon7.SelectedValue = "";
            this.CmbTipoOperacion7.SelectedValue = "";
            this.CmbRenglon8.Enabled = false;
            this.CmbTipoOperacion8.Enabled = false;
            this.CmbRenglon8.SelectedValue = "";
            this.CmbTipoOperacion8.SelectedValue = "";
            this.CmbRenglon9.Enabled = false;
            this.CmbTipoOperacion9.Enabled = false;
            this.CmbRenglon9.SelectedValue = "";
            this.CmbTipoOperacion9.SelectedValue = "";
            this.CmbRenglon10.Enabled = false;
            this.CmbRenglon10.SelectedValue = "";
        }

        private void HabilitarControles()
        {
            this.CmbTipoOperacion1.Enabled = true;
            this.CmbTipoOperacion1.SelectedValue = "";
            this.CmbRenglon2.Enabled = true;
            this.CmbTipoOperacion2.Enabled = true;
            this.CmbRenglon2.SelectedValue = "";
            this.CmbRenglon3.SelectedValue = "";
            this.CmbTipoOperacion1.Enabled = true;
            this.CmbTipoOperacion3.Enabled = true;
            this.CmbRenglon3.SelectedValue = "";
            this.CmbTipoOperacion3.SelectedValue = "";
            this.CmbRenglon4.Enabled = true;
            this.CmbTipoOperacion4.Enabled = true;
            this.CmbRenglon4.SelectedValue = "";
            this.CmbTipoOperacion4.SelectedValue = "";
            this.CmbRenglon5.Enabled = true;
            this.CmbTipoOperacion5.Enabled = true;
            this.CmbRenglon5.SelectedValue = "";
            this.CmbTipoOperacion5.SelectedValue = "";
            this.CmbRenglon6.Enabled = true;
            this.CmbTipoOperacion6.Enabled = true;
            this.CmbRenglon6.SelectedValue = "";
            this.CmbTipoOperacion6.SelectedValue = "";
            this.CmbRenglon7.Enabled = true;
            this.CmbTipoOperacion7.Enabled = true;
            this.CmbRenglon7.SelectedValue = "";
            this.CmbTipoOperacion7.SelectedValue = "";
            this.CmbRenglon8.Enabled = true;
            this.CmbTipoOperacion8.Enabled = true;
            this.CmbRenglon8.SelectedValue = "";
            this.CmbTipoOperacion8.SelectedValue = "";
            this.CmbRenglon9.Enabled = true;
            this.CmbTipoOperacion9.Enabled = true;
            this.CmbRenglon9.SelectedValue = "";
            this.CmbTipoOperacion9.SelectedValue = "";
            this.CmbRenglon10.Enabled = true;
            this.CmbRenglon10.SelectedValue = "";
        }

        protected void CmbRenglon1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbRenglon1.SelectedValue.ToString().Trim().Length > 0)
                {
                    //--AQUI VALIDAMOS QUE EL RENGLON NO SE ENCUENTRE SELECCIONADO EN OTRO CONTROL
                    string _RenglonSel = this.CmbRenglon1.SelectedItem.Text.ToString().Trim();
                    //--
                    if ((_RenglonSel.Equals(this.CmbRenglon2.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon3.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon4.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon5.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon6.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon7.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon8.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon9.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon10.SelectedItem.Text.ToString().Trim())))
                    {
                        this.CmbRenglon1.SelectedValue = "";
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
                        string _MsgMensaje = "Señor usuario. El No. de renglon " + _RenglonSel + " ya se encuentra seleccionado. Por favor seleccione otro número !";
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
                    else
                    {
                        this.CmbTipoOperacion1.Enabled = true;
                        this.CmbTipoOperacion1.Focus();
                    }
                }
                else
                {
                    this.DesabilitarControles();
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
                    string _MsgMensaje = "Señor usuario. Deberá seleccionar por lo menos este campo del formulario !";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbTipoOperacion1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbTipoOperacion1.SelectedValue.ToString().Trim().Length > 0)
                {
                    this.CmbRenglon2.Enabled = true;
                    this.CmbRenglon2.Focus();
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbTipoOperacion1.Enabled = false;
                    this.CmbTipoOperacion1.SelectedValue = "";
                    this.CmbRenglon2.Enabled = false;
                    this.CmbTipoOperacion2.Enabled = false;
                    this.CmbRenglon2.SelectedValue = "";
                    this.CmbRenglon3.SelectedValue = "";
                    this.CmbTipoOperacion3.Enabled = false;
                    this.CmbRenglon3.SelectedValue = "";
                    this.CmbTipoOperacion3.SelectedValue = "";
                    this.CmbRenglon4.Enabled = false;
                    this.CmbTipoOperacion4.Enabled = false;
                    this.CmbRenglon4.SelectedValue = "";
                    this.CmbTipoOperacion4.SelectedValue = "";
                    this.CmbRenglon5.Enabled = false;
                    this.CmbTipoOperacion5.Enabled = false;
                    this.CmbRenglon5.SelectedValue = "";
                    this.CmbTipoOperacion5.SelectedValue = "";
                    this.CmbRenglon6.Enabled = false;
                    this.CmbTipoOperacion6.Enabled = false;
                    this.CmbRenglon6.SelectedValue = "";
                    this.CmbTipoOperacion6.SelectedValue = "";
                    this.CmbRenglon7.Enabled = false;
                    this.CmbTipoOperacion7.Enabled = false;
                    this.CmbRenglon7.SelectedValue = "";
                    this.CmbTipoOperacion7.SelectedValue = "";
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbRenglon8.SelectedValue = "";
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbRenglon2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbRenglon2.SelectedValue.ToString().Trim().Length > 0)
                {
                    //--AQUI VALIDAMOS QUE EL RENGLON NO SE ENCUENTRE SELECCIONADO EN OTRO CONTROL
                    string _RenglonSel = this.CmbRenglon2.SelectedItem.Text.ToString().Trim();
                    //--
                    if ((_RenglonSel.Equals(this.CmbRenglon1.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon3.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon4.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon5.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon6.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon7.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon8.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon9.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon10.SelectedItem.Text.ToString().Trim())))
                    {
                        this.CmbRenglon2.SelectedValue = "";
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
                        string _MsgMensaje = "Señor usuario. El No. de renglon " + _RenglonSel + " ya se encuentra seleccionado. Por favor seleccione otro número !";
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
                    else
                    {
                        this.CmbTipoOperacion2.Enabled = true;
                        this.CmbTipoOperacion2.Focus();
                    }
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbTipoOperacion1.SelectedValue = "";
                    this.CmbRenglon2.Enabled = false;
                    this.CmbTipoOperacion2.Enabled = false;
                    this.CmbTipoOperacion2.SelectedValue = "";
                    this.CmbRenglon3.SelectedValue = "";
                    this.CmbTipoOperacion3.Enabled = false;
                    this.CmbRenglon3.SelectedValue = "";
                    this.CmbTipoOperacion3.SelectedValue = "";
                    this.CmbRenglon4.Enabled = false;
                    this.CmbTipoOperacion4.Enabled = false;
                    this.CmbRenglon4.SelectedValue = "";
                    this.CmbTipoOperacion4.SelectedValue = "";
                    this.CmbRenglon5.Enabled = false;
                    this.CmbTipoOperacion5.Enabled = false;
                    this.CmbRenglon5.SelectedValue = "";
                    this.CmbTipoOperacion5.SelectedValue = "";
                    this.CmbRenglon6.Enabled = false;
                    this.CmbTipoOperacion6.Enabled = false;
                    this.CmbRenglon6.SelectedValue = "";
                    this.CmbTipoOperacion6.SelectedValue = "";
                    this.CmbRenglon7.Enabled = false;
                    this.CmbTipoOperacion7.Enabled = false;
                    this.CmbRenglon7.SelectedValue = "";
                    this.CmbTipoOperacion7.SelectedValue = "";
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbRenglon8.SelectedValue = "";
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbTipoOperacion2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbTipoOperacion2.SelectedValue.ToString().Trim().Length > 0)
                {
                    this.CmbRenglon3.Enabled = true;
                    this.CmbRenglon3.Focus();
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon3.SelectedValue = "";
                    this.CmbTipoOperacion3.Enabled = false;
                    this.CmbRenglon3.SelectedValue = "";
                    this.CmbTipoOperacion3.SelectedValue = "";
                    this.CmbRenglon4.Enabled = false;
                    this.CmbTipoOperacion4.Enabled = false;
                    this.CmbRenglon4.SelectedValue = "";
                    this.CmbTipoOperacion4.SelectedValue = "";
                    this.CmbRenglon5.Enabled = false;
                    this.CmbTipoOperacion5.Enabled = false;
                    this.CmbRenglon5.SelectedValue = "";
                    this.CmbTipoOperacion5.SelectedValue = "";
                    this.CmbRenglon6.Enabled = false;
                    this.CmbTipoOperacion6.Enabled = false;
                    this.CmbRenglon6.SelectedValue = "";
                    this.CmbTipoOperacion6.SelectedValue = "";
                    this.CmbRenglon7.Enabled = false;
                    this.CmbTipoOperacion7.Enabled = false;
                    this.CmbRenglon7.SelectedValue = "";
                    this.CmbTipoOperacion7.SelectedValue = "";
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbRenglon8.SelectedValue = "";
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbRenglon3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbRenglon3.SelectedValue.ToString().Trim().Length > 0)
                {
                    //--AQUI VALIDAMOS QUE EL RENGLON NO SE ENCUENTRE SELECCIONADO EN OTRO CONTROL
                    string _RenglonSel = this.CmbRenglon3.SelectedItem.Text.ToString().Trim();
                    //--
                    if ((_RenglonSel.Equals(this.CmbRenglon1.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon2.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon4.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon5.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon6.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon7.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon8.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon9.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon10.SelectedItem.Text.ToString().Trim())))
                    {
                        this.CmbRenglon3.SelectedValue = "";
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
                        string _MsgMensaje = "Señor usuario. El No. de renglon " + _RenglonSel + " ya se encuentra seleccionado. Por favor seleccione otro número !";
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
                    else
                    {
                        this.CmbTipoOperacion3.Enabled = true;
                        this.CmbTipoOperacion3.Focus();
                    }
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon3.Enabled = false;
                    this.CmbTipoOperacion2.SelectedValue = "";
                    this.CmbTipoOperacion3.Enabled = false;
                    this.CmbTipoOperacion3.SelectedValue = "";
                    this.CmbRenglon4.Enabled = false;
                    this.CmbTipoOperacion4.Enabled = false;
                    this.CmbRenglon4.SelectedValue = "";
                    this.CmbTipoOperacion4.SelectedValue = "";
                    this.CmbRenglon5.Enabled = false;
                    this.CmbTipoOperacion5.Enabled = false;
                    this.CmbRenglon5.SelectedValue = "";
                    this.CmbTipoOperacion5.SelectedValue = "";
                    this.CmbRenglon6.Enabled = false;
                    this.CmbTipoOperacion6.Enabled = false;
                    this.CmbRenglon6.SelectedValue = "";
                    this.CmbTipoOperacion6.SelectedValue = "";
                    this.CmbRenglon7.Enabled = false;
                    this.CmbTipoOperacion7.Enabled = false;
                    this.CmbRenglon7.SelectedValue = "";
                    this.CmbTipoOperacion7.SelectedValue = "";
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbRenglon8.SelectedValue = "";
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbTipoOperacion3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbTipoOperacion3.SelectedValue.ToString().Trim().Length > 0)
                {
                    this.CmbRenglon4.Enabled = true;
                    this.CmbRenglon4.Focus();
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon4.Enabled = false;
                    this.CmbTipoOperacion4.Enabled = false;
                    this.CmbRenglon4.SelectedValue = "";
                    this.CmbTipoOperacion4.SelectedValue = "";
                    this.CmbRenglon5.Enabled = false;
                    this.CmbTipoOperacion5.Enabled = false;
                    this.CmbRenglon5.SelectedValue = "";
                    this.CmbTipoOperacion5.SelectedValue = "";
                    this.CmbRenglon6.Enabled = false;
                    this.CmbTipoOperacion6.Enabled = false;
                    this.CmbRenglon6.SelectedValue = "";
                    this.CmbTipoOperacion6.SelectedValue = "";
                    this.CmbRenglon7.Enabled = false;
                    this.CmbTipoOperacion7.Enabled = false;
                    this.CmbRenglon7.SelectedValue = "";
                    this.CmbTipoOperacion7.SelectedValue = "";
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbRenglon8.SelectedValue = "";
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbRenglon4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbRenglon4.SelectedValue.ToString().Trim().Length > 0)
                {
                    //--AQUI VALIDAMOS QUE EL RENGLON NO SE ENCUENTRE SELECCIONADO EN OTRO CONTROL
                    string _RenglonSel = this.CmbRenglon4.SelectedItem.Text.ToString().Trim();
                    //--
                    if ((_RenglonSel.Equals(this.CmbRenglon1.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon2.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon3.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon5.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon6.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon7.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon8.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon9.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon10.SelectedItem.Text.ToString().Trim())))
                    {
                        this.CmbRenglon4.SelectedValue = "";
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
                        string _MsgMensaje = "Señor usuario. El No. de renglon " + _RenglonSel + " ya se encuentra seleccionado. Por favor seleccione otro número !";
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
                    else
                    {
                        this.CmbTipoOperacion4.Enabled = true;
                        this.CmbTipoOperacion4.Focus();
                    }
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon4.Enabled = false;
                    this.CmbTipoOperacion3.SelectedValue = "";
                    this.CmbTipoOperacion4.Enabled = false;
                    this.CmbTipoOperacion4.SelectedValue = "";
                    this.CmbRenglon5.Enabled = false;
                    this.CmbTipoOperacion5.Enabled = false;
                    this.CmbRenglon5.SelectedValue = "";
                    this.CmbTipoOperacion5.SelectedValue = "";
                    this.CmbRenglon6.Enabled = false;
                    this.CmbTipoOperacion6.Enabled = false;
                    this.CmbRenglon6.SelectedValue = "";
                    this.CmbTipoOperacion6.SelectedValue = "";
                    this.CmbRenglon7.Enabled = false;
                    this.CmbTipoOperacion7.Enabled = false;
                    this.CmbRenglon7.SelectedValue = "";
                    this.CmbTipoOperacion7.SelectedValue = "";
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbRenglon8.SelectedValue = "";
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbTipoOperacion4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbTipoOperacion4.SelectedValue.ToString().Trim().Length > 0)
                {
                    this.CmbRenglon5.Enabled = true;
                    this.CmbRenglon5.Focus();
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon5.Enabled = false;
                    this.CmbTipoOperacion5.Enabled = false;
                    this.CmbRenglon5.SelectedValue = "";
                    this.CmbTipoOperacion5.SelectedValue = "";
                    this.CmbRenglon6.Enabled = false;
                    this.CmbTipoOperacion6.Enabled = false;
                    this.CmbRenglon6.SelectedValue = "";
                    this.CmbTipoOperacion6.SelectedValue = "";
                    this.CmbRenglon7.Enabled = false;
                    this.CmbTipoOperacion7.Enabled = false;
                    this.CmbRenglon7.SelectedValue = "";
                    this.CmbTipoOperacion7.SelectedValue = "";
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbRenglon8.SelectedValue = "";
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbRenglon5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbRenglon5.SelectedValue.ToString().Trim().Length > 0)
                {
                    //--AQUI VALIDAMOS QUE EL RENGLON NO SE ENCUENTRE SELECCIONADO EN OTRO CONTROL
                    string _RenglonSel = this.CmbRenglon5.SelectedItem.Text.ToString().Trim();
                    //--
                    if ((_RenglonSel.Equals(this.CmbRenglon1.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon2.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon3.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon4.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon6.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon7.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon8.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon9.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon10.SelectedItem.Text.ToString().Trim())))
                    {
                        this.CmbRenglon5.SelectedValue = "";
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
                        string _MsgMensaje = "Señor usuario. El No. de renglon " + _RenglonSel + " ya se encuentra seleccionado. Por favor seleccione otro número !";
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
                    else
                    {
                        this.CmbTipoOperacion5.Enabled = true;
                        this.CmbTipoOperacion5.Focus();
                    }
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon5.Enabled = false;
                    this.CmbTipoOperacion4.SelectedValue = "";
                    this.CmbTipoOperacion5.Enabled = false;
                    this.CmbTipoOperacion5.SelectedValue = "";
                    this.CmbRenglon6.Enabled = false;
                    this.CmbTipoOperacion6.Enabled = false;
                    this.CmbRenglon6.SelectedValue = "";
                    this.CmbTipoOperacion6.SelectedValue = "";
                    this.CmbRenglon7.Enabled = false;
                    this.CmbTipoOperacion7.Enabled = false;
                    this.CmbRenglon7.SelectedValue = "";
                    this.CmbTipoOperacion7.SelectedValue = "";
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbRenglon8.SelectedValue = "";
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbTipoOperacion5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbTipoOperacion5.SelectedValue.ToString().Trim().Length > 0)
                {
                    this.CmbRenglon6.Enabled = true;
                    this.CmbRenglon6.Focus();
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon6.Enabled = false;
                    this.CmbTipoOperacion6.Enabled = false;
                    this.CmbRenglon6.SelectedValue = "";
                    this.CmbTipoOperacion6.SelectedValue = "";
                    this.CmbRenglon7.Enabled = false;
                    this.CmbTipoOperacion7.Enabled = false;
                    this.CmbRenglon7.SelectedValue = "";
                    this.CmbTipoOperacion7.SelectedValue = "";
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbRenglon8.SelectedValue = "";
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbRenglon6_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbRenglon6.SelectedValue.ToString().Trim().Length > 0)
                {
                    //--AQUI VALIDAMOS QUE EL RENGLON NO SE ENCUENTRE SELECCIONADO EN OTRO CONTROL
                    string _RenglonSel = this.CmbRenglon6.SelectedItem.Text.ToString().Trim();
                    //--
                    if ((_RenglonSel.Equals(this.CmbRenglon1.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon2.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon3.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon4.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon5.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon7.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon8.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon9.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon10.SelectedItem.Text.ToString().Trim())))
                    {
                        this.CmbRenglon6.SelectedValue = "";
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
                        string _MsgMensaje = "Señor usuario. El No. de renglon " + _RenglonSel + " ya se encuentra seleccionado. Por favor seleccione otro número !";
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
                    else
                    {
                        this.CmbTipoOperacion6.Enabled = true;
                        this.CmbTipoOperacion6.Focus();
                    }
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon6.Enabled = false;
                    this.CmbTipoOperacion5.SelectedValue = "";
                    this.CmbTipoOperacion6.Enabled = false;
                    this.CmbTipoOperacion6.SelectedValue = "";
                    this.CmbRenglon7.Enabled = false;
                    this.CmbTipoOperacion7.Enabled = false;
                    this.CmbRenglon7.SelectedValue = "";
                    this.CmbTipoOperacion7.SelectedValue = "";
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbRenglon8.SelectedValue = "";
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbTipoOperacion6_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbTipoOperacion6.SelectedValue.ToString().Trim().Length > 0)
                {
                    this.CmbRenglon7.Enabled = true;
                    this.CmbRenglon7.Focus();
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon7.Enabled = false;
                    this.CmbTipoOperacion7.Enabled = false;
                    this.CmbRenglon7.SelectedValue = "";
                    this.CmbTipoOperacion7.SelectedValue = "";
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbRenglon8.SelectedValue = "";
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbRenglon7_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbRenglon7.SelectedValue.ToString().Trim().Length > 0)
                {
                    //--AQUI VALIDAMOS QUE EL RENGLON NO SE ENCUENTRE SELECCIONADO EN OTRO CONTROL
                    string _RenglonSel = this.CmbRenglon7.SelectedItem.Text.ToString().Trim();
                    //--
                    if ((_RenglonSel.Equals(this.CmbRenglon1.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon2.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon3.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon4.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon5.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon6.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon8.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon9.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon10.SelectedItem.Text.ToString().Trim())))
                    {
                        this.CmbRenglon7.SelectedValue = "";
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
                        string _MsgMensaje = "Señor usuario. El No. de renglon " + _RenglonSel + " ya se encuentra seleccionado. Por favor seleccione otro número !";
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
                    else
                    {
                        this.CmbTipoOperacion7.Enabled = true;
                        this.CmbTipoOperacion7.Focus();
                    }
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon7.Enabled = false;
                    this.CmbTipoOperacion6.SelectedValue = "";
                    this.CmbTipoOperacion7.Enabled = false;
                    this.CmbTipoOperacion7.SelectedValue = "";
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbRenglon8.SelectedValue = "";
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbTipoOperacion7_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbTipoOperacion7.SelectedValue.ToString().Trim().Length > 0)
                {
                    this.CmbRenglon8.Enabled = true;
                    this.CmbRenglon8.Focus();
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbRenglon8.SelectedValue = "";
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbRenglon8_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbRenglon8.SelectedValue.ToString().Trim().Length > 0)
                {
                    //--AQUI VALIDAMOS QUE EL RENGLON NO SE ENCUENTRE SELECCIONADO EN OTRO CONTROL
                    string _RenglonSel = this.CmbRenglon8.SelectedItem.Text.ToString().Trim();
                    //--
                    if ((_RenglonSel.Equals(this.CmbRenglon1.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon2.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon3.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon4.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon5.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon6.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon7.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon9.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon10.SelectedItem.Text.ToString().Trim())))
                    {
                        this.CmbRenglon8.SelectedValue = "";
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
                        string _MsgMensaje = "Señor usuario. El No. de renglon " + _RenglonSel + " ya se encuentra seleccionado. Por favor seleccione otro número !";
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
                    else
                    {
                        this.CmbTipoOperacion8.Enabled = true;
                        this.CmbTipoOperacion8.Focus();
                    }
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon8.Enabled = false;
                    this.CmbTipoOperacion7.SelectedValue = "";
                    this.CmbTipoOperacion8.Enabled = false;
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbTipoOperacion8_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbTipoOperacion8.SelectedValue.ToString().Trim().Length > 0)
                {
                    this.CmbRenglon9.Enabled = true;
                    this.CmbRenglon9.Focus();
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbRenglon9.SelectedValue = "";
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbRenglon9_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbRenglon9.SelectedValue.ToString().Trim().Length > 0)
                {
                    //--AQUI VALIDAMOS QUE EL RENGLON NO SE ENCUENTRE SELECCIONADO EN OTRO CONTROL
                    string _RenglonSel = this.CmbRenglon9.SelectedItem.Text.ToString().Trim();
                    //--
                    if ((_RenglonSel.Equals(this.CmbRenglon1.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon2.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon3.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon4.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon5.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon6.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon7.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon8.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon10.SelectedItem.Text.ToString().Trim())))
                    {
                        this.CmbRenglon9.SelectedValue = "";
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
                        string _MsgMensaje = "Señor usuario. El No. de renglon " + _RenglonSel + " ya se encuentra seleccionado. Por favor seleccione otro número !";
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
                    else
                    {
                        this.CmbTipoOperacion9.Enabled = true;
                        this.CmbTipoOperacion9.Focus();
                    }
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon9.Enabled = false;
                    this.CmbTipoOperacion8.SelectedValue = "";
                    this.CmbTipoOperacion9.Enabled = false;
                    this.CmbTipoOperacion9.SelectedValue = "";
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbTipoOperacion9_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbTipoOperacion9.SelectedValue.ToString().Trim().Length > 0)
                {
                    this.CmbRenglon10.Enabled = true;
                    this.CmbRenglon10.Focus();
                }
                else
                {
                    #region DESABILITAMOS LOS CONTROLES SIGUIENTES A ESTE CONTROL
                    this.CmbRenglon10.Enabled = false;
                    this.CmbRenglon10.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void CmbRenglon10_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.CmbRenglon10.SelectedValue.ToString().Trim().Length > 0)
                {
                    //--AQUI VALIDAMOS QUE EL RENGLON NO SE ENCUENTRE SELECCIONADO EN OTRO CONTROL
                    string _RenglonSel = this.CmbRenglon10.SelectedItem.Text.ToString().Trim();
                    //--
                    if ((_RenglonSel.Equals(this.CmbRenglon1.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon2.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon3.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon4.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon5.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon6.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon7.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon8.SelectedItem.Text.ToString().Trim())) ||
                        (_RenglonSel.Equals(this.CmbRenglon9.SelectedItem.Text.ToString().Trim())))
                    {
                        this.CmbRenglon10.SelectedValue = "";
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
                        string _MsgMensaje = "Señor usuario. El No. de renglon " + _RenglonSel + " ya se encuentra seleccionado. Por favor seleccione otro número !";
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
                    else
                    {
                        this.CmbEstado.Focus();
                    }
                }
                else
                {
                    this.CmbRenglon10.Enabled = false;
                    this.CmbTipoOperacion9.SelectedValue = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error con este control. Motivo: " + ex.ToString();
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

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ObjMunDescuento.IdMunDescuento = this.ViewState["IdMunDescuento"].ToString().Trim().Length > 0 ? this.ViewState["IdMunDescuento"].ToString().Trim() : null;
                ObjMunDescuento.IdMunicipio = this.ViewState["IdMunicipio"].ToString().Trim();
                ObjMunDescuento.IdFormularioImpuesto = this.CmbTipoImpuesto.SelectedValue.ToString().Trim();
                ObjMunDescuento.AnioGravable = Int32.Parse(this.CmbAnioGravable.SelectedValue.ToString().Trim());
                ObjMunDescuento.DescripcionDescuento = this.TxtDescripcion.Text.ToString().Trim().ToUpper();
                //---
                ObjMunDescuento.IdFormConfiguracion1 = this.CmbRenglon1.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglon1.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdTipoOperacion1 = this.CmbTipoOperacion1.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoOperacion1.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdFormConfiguracion2 = this.CmbRenglon2.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglon2.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdTipoOperacion2 = this.CmbTipoOperacion2.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoOperacion2.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdFormConfiguracion3 = this.CmbRenglon3.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglon3.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdTipoOperacion3 = this.CmbTipoOperacion3.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoOperacion3.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdFormConfiguracion4 = this.CmbRenglon4.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglon4.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdTipoOperacion4 = this.CmbTipoOperacion4.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoOperacion4.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdFormConfiguracion5 = this.CmbRenglon5.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglon5.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdTipoOperacion5 = this.CmbTipoOperacion5.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoOperacion5.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdFormConfiguracion6 = this.CmbRenglon6.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglon6.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdTipoOperacion6 = this.CmbTipoOperacion6.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoOperacion6.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdFormConfiguracion7 = this.CmbRenglon7.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglon7.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdTipoOperacion7 = this.CmbTipoOperacion7.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoOperacion7.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdFormConfiguracion8 = this.CmbRenglon8.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglon8.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdTipoOperacion8 = this.CmbTipoOperacion8.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoOperacion8.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdFormConfiguracion9 = this.CmbRenglon9.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglon9.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdTipoOperacion9 = this.CmbTipoOperacion9.SelectedValue.ToString().Trim().Length > 0 ? this.CmbTipoOperacion9.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdFormConfiguracion10 = this.CmbRenglon10.SelectedValue.ToString().Trim().Length > 0 ? this.CmbRenglon10.SelectedValue.ToString().Trim() : null;
                ObjMunDescuento.IdEstado = this.CmbEstado.SelectedValue.ToString().Trim();
                ObjMunDescuento.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjMunDescuento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjMunDescuento.TipoProceso = Int32.Parse(this.ViewState["TipoProceso"].ToString().Trim()) == 1 ? 1 : 2;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjMunDescuento);
                _log.Warn("REQUEST DESCUENTO PRONTO PAGO MUNICIPIO => " + jsonRequest);

                int _IdRegistro = 0;
                string _MsgError = "";
                if (ObjMunDescuento.AddUpDescuento(ref _IdRegistro, ref _MsgError))
                {
                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                    if (ObjMunDescuento.TipoProceso == 1)
                    {
                        ObjAuditoria.IdTipoEvento = 2;  //--INSERT
                    }
                    else
                    {
                        ObjAuditoria.IdTipoEvento = 3;  //--UPDATE
                    }
                    ObjAuditoria.ModuloApp = "DESCUENTO_PP_MUNICIPIO";
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