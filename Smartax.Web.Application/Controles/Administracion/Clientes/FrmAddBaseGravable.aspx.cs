using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Administracion;
using System.Data;
using Smartax.Web.Application.Clases.Parametros;

namespace Smartax.Web.Application.Controles.Administracion.Clientes
{
    public partial class FrmAddBaseGravable : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        ClienteBaseGravable ObjBaseGrav = new ClienteBaseGravable();
        ClienteEstadosFinanciero ObjClienteEF = new ClienteEstadosFinanciero();
        ClienteEstablecimiento ObjClienteEst = new ClienteEstablecimiento();
        FormConfiguracion ObjFormConf = new FormConfiguracion();
        Lista ObjLista = new Lista();

        private void GetBaseGravableCuenta()
        {
            try
            {
                ObjBaseGrav.TipoConsulta = 1;
                ObjBaseGrav.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                ObjBaseGrav.IdPuc = Int32.Parse(this.ViewState["IdClientePuc"].ToString().Trim());
                ObjBaseGrav.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjBaseGrav.GetClienteBaseGravable();

                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        this.ViewState["IdClienteBaseGravable"] = dtDatos.Rows[0]["idcliente_base_gravable"].ToString().Trim();
                        this.CmbAnioGravable.SelectedValue = dtDatos.Rows[0]["anio_gravable"].ToString().Trim();
                        this.CmbRenglonForm.SelectedValue = dtDatos.Rows[0]["idform_configuracion"].ToString().Trim() + "|" + dtDatos.Rows[0]["descripcion_renglon"].ToString().Trim();
                        this.LblDescripcion.Text = dtDatos.Rows[0]["descripcion_renglon"].ToString().Trim();
                        //--
                        GetValorEstadoFinanciero();

                        //--AQUI DEFINIMOS CUAL DE LOS VALORES ESTA SELECCIONADO
                        this.ChkSaldoInicial.Checked = Boolean.Parse(dtDatos.Rows[0]["saldo_inicial"].ToString().Trim());
                        this.ChkMovDebito.Checked = Boolean.Parse(dtDatos.Rows[0]["mov_debito"].ToString().Trim());
                        this.ChkMovCredito.Checked = Boolean.Parse(dtDatos.Rows[0]["mov_credito"].ToString().Trim());
                        this.ChkSaldoFinal.Checked = Boolean.Parse(dtDatos.Rows[0]["saldo_final"].ToString().Trim());

                        this.TxtValorExtracontable.Text = dtDatos.Rows[0]["valor_extracontable"].ToString().Trim();
                        string _SaldoInicial = this.LblSaldoInicial.Text.ToString().Trim().Replace("$ ", "").Replace(".", "");
                        string _MovDebito = this.LblMovDebito.Text.ToString().Trim().Replace("$ ", "").Replace(".", "");
                        string _MovCredito = this.LblMovCredito.Text.ToString().Trim().Replace("$ ", "").Replace(".", "");
                        string _SaldoFinal = this.LblSaldoFinal.Text.ToString().Trim().Replace("$ ", "").Replace(".", "");
                        string _ValorExtracontable = this.TxtValorExtracontable.Text.ToString().Trim().Replace(".", "");

                        double _ValorTotal = Double.Parse(_SaldoInicial) + Double.Parse(_MovDebito) + Double.Parse(_MovCredito) + Double.Parse(_SaldoFinal) + Double.Parse(_ValorExtracontable);
                        this.LblValorTotal.Text = String.Format(String.Format("{0:$ ###,###,##0}", _ValorTotal));
                    }
                    else
                    {
                        this.ViewState["IdClienteBaseGravable"] = "";
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al mostrar los valores del estado financiero con la cuenta seleccionada. Motivo: " + ex.ToString();
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

        private void GetConsolidadoEstFinanciero()
        {
            try
            {
                //--AQUI OBTENEMOS LO VALORES ENVIAR COMO PARAMETROS AL SP
                ObjClienteEF.TipoConsulta = this.ViewState["TipoConsulta"].ToString().Trim().Length > 0 ? Int32.Parse(this.ViewState["TipoConsulta"].ToString().Trim()) : -1;
                ObjClienteEF.AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0 ? this.CmbAnioGravable.SelectedValue.ToString().Trim() : "-1";
                ObjClienteEF.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                ObjClienteEF.IdClienteEstablecimiento = this.ViewState["IdClienteEstablecimiento"].ToString().Trim().Length > 0 ? this.ViewState["IdClienteEstablecimiento"].ToString().Trim() : null;
                ObjClienteEF.CodigoCuenta = this.ViewState["CodigoCuenta"].ToString().Trim();
                ObjClienteEF.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjClienteEF.GetConsolidadoEstadoFinanciero();

                if (dtDatos != null)
                {
                    if (dtDatos.Rows.Count > 0)
                    {
                        //--AQUI DESHABILITAMOS LOS CONTROLES PARA LOS VALORES
                        this.ChkSaldoInicial.Enabled = true;
                        this.ChkMovDebito.Enabled = true;
                        this.ChkMovCredito.Enabled = true;
                        this.ChkSaldoFinal.Enabled = true;

                        this.LblSaldoInicial.Text = dtDatos.Rows[0]["saldo_inicial_mn"].ToString().Trim();
                        this.LblMovDebito.Text = dtDatos.Rows[0]["debito_mn"].ToString().Trim();
                        this.LblMovCredito.Text = dtDatos.Rows[0]["credito_mn"].ToString().Trim();
                        this.LblSaldoFinal.Text = dtDatos.Rows[0]["saldo_final_mn"].ToString().Trim();
                    }
                    else
                    {
                        this.ChkSaldoInicial.Enabled = false;
                        this.ChkMovDebito.Enabled = false;
                        this.ChkMovCredito.Enabled = false;
                        this.ChkSaldoFinal.Enabled = false;
                        this.LblSaldoInicial.Text = "$ 0.0";
                        this.LblMovDebito.Text = "$ 0.0";
                        this.LblMovCredito.Text = "$ 0.0";
                        this.LblSaldoFinal.Text = "$ 0.0";

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
                        string _MsgMensaje = "Señor usuario, No se encontro información de estado financiero con el No. de cuenta seleccionado, por favor validar... !";
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
                else
                {
                    this.ChkSaldoInicial.Enabled = false;
                    this.ChkMovDebito.Enabled = false;
                    this.ChkMovCredito.Enabled = false;
                    this.ChkSaldoFinal.Enabled = false;
                    this.LblSaldoInicial.Text = "$ 0.0";
                    this.LblMovDebito.Text = "$ 0.0";
                    this.LblMovCredito.Text = "$ 0.0";
                    this.LblSaldoFinal.Text = "$ 0.0";

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
                    string _MsgMensaje = "Señor usuario, No se encontro información de estado financiero con el No. de cuenta seleccionado, por favor validar... !";
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
                this.ChkSaldoInicial.Enabled = false;
                this.ChkMovDebito.Enabled = false;
                this.ChkMovCredito.Enabled = false;
                this.ChkSaldoFinal.Enabled = false;
                this.LblSaldoInicial.Text = "$ 0.0";
                this.LblMovDebito.Text = "$ 0.0";
                this.LblMovCredito.Text = "$ 0.0";
                this.LblSaldoFinal.Text = "$ 0.0";

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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al mostrar los valores del estado financiero con la cuenta seleccionada. Motivo: " + ex.ToString();
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
                ObjFormConf.IdFormularioImpuesto = this.ViewState["IdFormImpuesto"].ToString().Trim();
                ObjFormConf.IdEstado = 1;
                ObjFormConf.MostrarSeleccione = "SI";
                ObjFormConf.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbRenglonForm.DataSource = ObjFormConf.GetFormConfiguracion();
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

        protected void LstAniosGravables()
        {
            try
            {
                ObjLista.MostrarSeleccione = "SI";
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

        private DataTable LstOficinasMunicipio()
        {
            DataTable dtEstablecimiento = new DataTable();
            try
            {
                ObjClienteEst.TipoConsulta = 2;
                ObjClienteEst.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                ObjClienteEst.IdEstablecimientoPadre = null;
                ObjClienteEst.IdEstado = 1;
                ObjClienteEst.MostrarSeleccione = "NO";
                ObjClienteEst.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                if (this.ViewState["dtComercios"] != null)
                {
                    //Aqui Cargamos el DataTable en el TextBox de los Convenios.
                    this.TxtMunicipio.DataSource = (DataTable)this.ViewState["dtComercios"];
                    this.TxtMunicipio.DataTextField = "nombre_oficina";
                    this.TxtMunicipio.DataValueField = "idcliente_establecimiento";
                    this.TxtMunicipio.DataBind();
                }
                else
                {
                    dtEstablecimiento = ObjClienteEst.GetEstablecimientos();
                    if (dtEstablecimiento != null)
                    {
                        //Aqui Cargamos el DataTable en el TextBox de los Convenios.
                        this.ViewState["dtComercios"] = dtEstablecimiento;
                        this.TxtMunicipio.DataSource = (DataTable)this.ViewState["dtComercios"];
                        this.TxtMunicipio.DataTextField = "nombre_oficina";
                        this.TxtMunicipio.DataValueField = "idcliente_establecimiento";
                        this.TxtMunicipio.DataBind();
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
                        string _MsgMensaje = "Señor usuario. No se encontro establecimientos registrados del cliente. Por favor validar !";
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
                dtEstablecimiento = null;
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los establecimientos. Motivo: " + ex.ToString();
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

            return dtEstablecimiento;
        }

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objNavegacion.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = this.ViewState["PathUrl"].ToString().Trim();
            objPermiso.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

            objPermiso.RefrescarPermisos();
            if (!objPermiso.PuedeRegistrar)
            {
                this.BtnGuardar.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //--OBTENER VALOR DE PARAMETROS
                this.ViewState["IdClientePuc"] = Request.QueryString["IdClientePuc"].ToString().Trim();
                this.ViewState["IdFormImpuesto"] = Request.QueryString["IdFormImpuesto"].ToString().Trim();
                this.ViewState["CodigoCuenta"] = Request.QueryString["CodigoCuenta"].ToString().Trim();
                this.ViewState["IdCliente"] = Request.QueryString["IdCliente"].ToString().Trim();
                this.LblNombreFormulario.Text = Request.QueryString["NombreForm"].ToString().Trim();
                this.ViewState["PathUrl"] = Request.QueryString["PathUrl"].ToString().Trim();
                this.ViewState["IdClienteBaseGravable"] = "";
                this.ViewState["dtComercios"] = null;
                this.ViewState["TipoConsulta"] = "1";
                this.ViewState["IdClienteEstablecimiento"] = "";

                //Metodo para aplicar permisos
                if (this.Session["IdCliente"] == null)
                {
                    this.AplicarPermisos();
                }

                //--CONSULTAMOS LOS VALORES EN EL ESTADO FINANCIERO
                //this.GetConsolidadoEstFinanciero();

                //--METODOS PARA LISTAR DATOS.
                this.LstFormConfiguracion();
                this.LstAniosGravables();
                this.LstOficinasMunicipio();

                //--CONSULTAMOS LOS VALORES DE LA BASE GRAVABLE
                this.GetBaseGravableCuenta();
            }
            else
            {
                //--METODO PARA LISTAR LOS ESTABLECIMIENTOS DEL CLIENTE.
                this.LstOficinasMunicipio();
            }
        }

        #region DEFINICION DE EVENTOS OBJETOS
        protected void CmbAnioGravable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0)
            {
                this.CmbRenglonForm.Enabled = true;
                this.CmbRenglonForm.Focus();
            }
            else
            {
                this.CmbRenglonForm.Enabled = false;
                this.CmbRenglonForm.SelectedValue = "";
            }
        }

        protected void CmbRenglonForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetValorEstadoFinanciero();
        }

        private void GetValorEstadoFinanciero()
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
                    this.LblDescripcion.Text = _ArrayData[1].ToString().Trim();

                    #region AQUI VALIDAMOS EL No DE RENGLON DEL FRM PARA CALCULAR VALORES DEL E. F.
                    //--AQUI VALIDAMOS EL RENGLON DEL FORMULARIO.
                    if (this.CmbRenglonForm.SelectedItem.Text.ToString().Trim().Equals("8"))
                    {
                        this.ViewState["TipoConsulta"] = "1";
                        this.TxtMunicipio.Entries.Clear();
                        this.TxtMunicipio.Enabled = false;
                        this.Validador3.Enabled = false;

                        //--AQUI EJECUTAMOS EL METODO DE LA CONSULTA DE LA DB
                        this.GetConsolidadoEstFinanciero();
                    }
                    else if (this.CmbRenglonForm.SelectedItem.Text.ToString().Trim().Equals("10"))
                    {
                        //--AQUI DESHABILITAMOS LOS CONTROLES PARA LOS VALORES
                        this.ChkSaldoInicial.Enabled = false;
                        this.ChkMovDebito.Enabled = false;
                        this.ChkMovCredito.Enabled = false;
                        this.ChkSaldoFinal.Enabled = false;
                        this.LblSaldoInicial.Text = "$ 0.0";
                        this.LblMovDebito.Text = "$ 0.0";
                        this.LblMovCredito.Text = "$ 0.0";
                        this.LblSaldoFinal.Text = "$ 0.0";

                        this.ViewState["TipoConsulta"] = "2";
                        this.TxtMunicipio.Enabled = true;
                        this.Validador3.Enabled = true;
                        this.TxtMunicipio.Focus();
                    }
                    else
                    {
                        //--AQUI DESHABILITAMOS LOS CONTROLES PARA LOS VALORES
                        this.ChkSaldoInicial.Enabled = false;
                        this.ChkMovDebito.Enabled = false;
                        this.ChkMovCredito.Enabled = false;
                        this.ChkSaldoFinal.Enabled = false;
                        this.LblSaldoInicial.Text = "$ 0.0";
                        this.LblMovDebito.Text = "$ 0.0";
                        this.LblMovCredito.Text = "$ 0.0";
                        this.LblSaldoFinal.Text = "$ 0.0";

                        this.ViewState["TipoConsulta"] = "1";
                        this.TxtMunicipio.Entries.Clear();
                        this.TxtMunicipio.Enabled = false;
                        this.Validador3.Enabled = false;
                    }
                    #endregion
                }
                else
                {
                    this.LblDescripcion.Text = "";
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

        protected void TxtMunicipio_TextChanged(object sender, AutoCompleteTextEventArgs e)
        {
            try
            {
                //--AQUI VALIDAMOS QUE EL AÑO GRAVABLE ESTE SELECCIONADO
                if (this.CmbAnioGravable.SelectedValue.ToString().Trim().Length > 0)
                {
                    string[] _ArrayOficina = e.Text.ToString().Trim().Split('-');

                    this.ViewState["TipoConsulta"] = "2";
                    this.ViewState["IdClienteEstablecimiento"] = _ArrayOficina[0].ToString().Trim();
                    this.GetConsolidadoEstFinanciero();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al realizar la consulta. Motivo: " + ex.ToString();
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

        protected void ChkSaldoInicial_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                #region OBTENEMOS VALORES PARA REALIZAR LA SUMATORIA
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                string _strSaldoInicial = this.LblSaldoInicial.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strMovDebito = this.LblMovDebito.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strMovCredito = this.LblMovCredito.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strSaldoFinal = this.LblSaldoFinal.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");

                double _SaldoInicial = this.ChkSaldoInicial.Checked == true ? Double.Parse(_strSaldoInicial) : 0;
                double _MovDebito = this.ChkMovDebito.Checked == true ? Double.Parse(_strMovDebito) : 0;
                double _MovCredito = this.ChkMovCredito.Checked == true ? Double.Parse(_strMovCredito) : 0;
                double _SaldoFinal = this.ChkSaldoFinal.Checked == true ? Double.Parse(_strSaldoFinal) : 0;

                double _ValorExtracontable = this.TxtValorExtracontable.Text.ToString().Trim().Length > 0 ? Convert.ToDouble(this.TxtValorExtracontable.Text.ToString().Trim()) : 0;
                double _ValorTotal = this.LblValorTotal.Text.ToString().Trim().Length > 0 ? Convert.ToDouble(this.LblValorTotal.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "")) : 0;
                double _Resultado = (_SaldoInicial + _MovDebito + _MovCredito + _SaldoFinal + _ValorExtracontable);
                this.LblValorTotal.Text = String.Format(String.Format("{0:$ ###,###,##0}", _Resultado));

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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al calcular el valor. Motivo: " + ex.ToString();
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

        protected void ChkMovDebito_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                #region OBTENEMOS VALORES PARA REALIZAR LA SUMATORIA
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                string _strSaldoInicial = this.LblSaldoInicial.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strMovDebito = this.LblMovDebito.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strMovCredito = this.LblMovCredito.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strSaldoFinal = this.LblSaldoFinal.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");

                double _SaldoInicial = this.ChkSaldoInicial.Checked == true ? Double.Parse(_strSaldoInicial) : 0;
                double _MovDebito = this.ChkMovDebito.Checked == true ? Double.Parse(_strMovDebito) : 0;
                double _MovCredito = this.ChkMovCredito.Checked == true ? Double.Parse(_strMovCredito) : 0;
                double _SaldoFinal = this.ChkSaldoFinal.Checked == true ? Double.Parse(_strSaldoFinal) : 0;

                double _ValorExtracontable = this.TxtValorExtracontable.Text.ToString().Trim().Length > 0 ? Convert.ToDouble(this.TxtValorExtracontable.Text.ToString().Trim()) : 0;
                double _ValorTotal = this.LblValorTotal.Text.ToString().Trim().Length > 0 ? Convert.ToDouble(this.LblValorTotal.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "")) : 0;
                double _Resultado = (_SaldoInicial + _MovDebito + _MovCredito + _SaldoFinal + _ValorExtracontable);
                this.LblValorTotal.Text = String.Format(String.Format("{0:$ ###,###,##0}", _Resultado));
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al calcular el valor. Motivo: " + ex.ToString();
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

        protected void ChkMovCredito_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                #region OBTENEMOS VALORES PARA REALIZAR LA SUMATORIA
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                string _strSaldoInicial = this.LblSaldoInicial.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strMovDebito = this.LblMovDebito.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strMovCredito = this.LblMovCredito.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strSaldoFinal = this.LblSaldoFinal.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");

                double _SaldoInicial = this.ChkSaldoInicial.Checked == true ? Double.Parse(_strSaldoInicial) : 0;
                double _MovDebito = this.ChkMovDebito.Checked == true ? Double.Parse(_strMovDebito) : 0;
                double _MovCredito = this.ChkMovCredito.Checked == true ? Double.Parse(_strMovCredito) : 0;
                double _SaldoFinal = this.ChkSaldoFinal.Checked == true ? Double.Parse(_strSaldoFinal) : 0;

                double _ValorExtracontable = this.TxtValorExtracontable.Text.ToString().Trim().Length > 0 ? Convert.ToDouble(this.TxtValorExtracontable.Text.ToString().Trim()) : 0;
                double _ValorTotal = this.LblValorTotal.Text.ToString().Trim().Length > 0 ? Convert.ToDouble(this.LblValorTotal.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "")) : 0;
                double _Resultado = (_SaldoInicial + _MovDebito + _MovCredito + _SaldoFinal + _ValorExtracontable);
                this.LblValorTotal.Text = String.Format(String.Format("{0:$ ###,###,##0}", _Resultado));
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al calcular el valor. Motivo: " + ex.ToString();
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

        protected void ChkSaldoFinal_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                #region OBTENEMOS VALORES PARA REALIZAR LA SUMATORIA
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                string _strSaldoInicial = this.LblSaldoInicial.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strMovDebito = this.LblMovDebito.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strMovCredito = this.LblMovCredito.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strSaldoFinal = this.LblSaldoFinal.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");

                double _SaldoInicial = this.ChkSaldoInicial.Checked == true ? Double.Parse(_strSaldoInicial) : 0;
                double _MovDebito = this.ChkMovDebito.Checked == true ? Double.Parse(_strMovDebito) : 0;
                double _MovCredito = this.ChkMovCredito.Checked == true ? Double.Parse(_strMovCredito) : 0;
                double _SaldoFinal = this.ChkSaldoFinal.Checked == true ? Double.Parse(_strSaldoFinal) : 0;

                double _ValorExtracontable = this.TxtValorExtracontable.Text.ToString().Trim().Length > 0 ? Convert.ToDouble(this.TxtValorExtracontable.Text.ToString().Trim()) : 0;
                double _ValorTotal = this.LblValorTotal.Text.ToString().Trim().Length > 0 ? Convert.ToDouble(this.LblValorTotal.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "")) : 0;
                double _Resultado = (_SaldoInicial + _MovDebito + _MovCredito + _SaldoFinal + _ValorExtracontable);
                this.LblValorTotal.Text = String.Format(String.Format("{0:$ ###,###,##0}", _Resultado));
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al calcular el valor. Motivo: " + ex.ToString();
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

        protected void TxtValorExtracontable_TextChanged(object sender, EventArgs e)
        {
            try
            {
                #region OBTENEMOS VALORES PARA REALIZAR LA SUMATORIA
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                string _strSaldoInicial = this.LblSaldoInicial.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strMovDebito = this.LblMovDebito.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strMovCredito = this.LblMovCredito.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _strSaldoFinal = this.LblSaldoFinal.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");

                double _SaldoInicial = this.ChkSaldoInicial.Checked == true ? Double.Parse(_strSaldoInicial) : 0;
                double _MovDebito = this.ChkMovDebito.Checked == true ? Double.Parse(_strMovDebito) : 0;
                double _MovCredito = this.ChkMovCredito.Checked == true ? Double.Parse(_strMovCredito) : 0;
                double _SaldoFinal = this.ChkSaldoFinal.Checked == true ? Double.Parse(_strSaldoFinal) : 0;

                double _ValorExtracontable = this.TxtValorExtracontable.Text.ToString().Trim().Length > 0 ? Convert.ToDouble(this.TxtValorExtracontable.Text.ToString().Trim()) : 0;
                double _ValorTotal = this.LblValorTotal.Text.ToString().Trim().Length > 0 ? Convert.ToDouble(this.LblValorTotal.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "")) : 0;
                double _Resultado = (_SaldoInicial + _MovDebito + _MovCredito + _SaldoFinal + _ValorExtracontable);
                this.LblValorTotal.Text = String.Format(String.Format("{0:$ ###,###,##0}", _Resultado));
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al calcular el valor. Motivo: " + ex.ToString();
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
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                #region DEFINICION DE CAMPOS PARA ALMACENAR
                ObjBaseGrav.IdClienteBaseGravable = this.ViewState["IdClienteBaseGravable"].ToString().Trim().Length > 0 ? this.ViewState["IdClienteBaseGravable"].ToString().Trim() : null;
                ObjBaseGrav.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                ObjBaseGrav.IdPuc = Int32.Parse(this.ViewState["IdClientePuc"].ToString().Trim());
                string[] _ArrayData = this.CmbRenglonForm.SelectedValue.ToString().Trim().Split('|');
                ObjBaseGrav.IdFormConfiguracion = _ArrayData[0].ToString().Trim();
                ObjBaseGrav.AnioGravable = this.CmbAnioGravable.SelectedValue.ToString().Trim();

                string _SaldoInicial = this.LblSaldoInicial.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _MovDebito = this.LblMovDebito.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _MovCredito = this.LblMovCredito.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                string _SaldoFinal = this.LblSaldoFinal.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");

                ObjBaseGrav.SaldoInicial = this.ChkSaldoInicial.Checked == true ? "S" : "N";
                ObjBaseGrav.MovDebito = this.ChkMovDebito.Checked == true ? "S" : "N";
                ObjBaseGrav.MovCredito = this.ChkMovCredito.Checked == true ? "S" : "N";
                ObjBaseGrav.SaldoFinal = this.ChkSaldoFinal.Checked == true ? "S" : "N";
                //ObjBaseGrav.SaldoInicial = this.ChkSaldoInicial.Checked == true ? _SaldoInicial : "0";
                //ObjBaseGrav.MovDebito = this.ChkMovDebito.Checked == true ? _MovDebito : "0";
                //ObjBaseGrav.MovCredito = this.ChkMovCredito.Checked == true ? _MovCredito : "0";
                //ObjBaseGrav.SaldoFinal = this.ChkSaldoFinal.Checked == true ? _SaldoFinal : "0";

                ObjBaseGrav.ValorExtracontable = this.TxtValorExtracontable.Text.ToString().Trim().Length > 0 ? this.TxtValorExtracontable.Text.ToString().Trim().Replace(".", "") : "0";
                ObjBaseGrav.ValorTotal = this.LblValorTotal.Text.ToString().Trim().Replace("$ ", "").Replace("$", "").Replace(".", "");
                ObjBaseGrav.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjBaseGrav.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                ObjBaseGrav.TipoProceso = this.ViewState["IdClienteBaseGravable"].ToString().Trim().Length > 0 ? 2 : 1;
                #endregion

                int _IdRegistro = 0;
                string _MsgError = "";
                if (ObjBaseGrav.AddClienteBaseGravable(ref _IdRegistro, ref _MsgError))
                {
                    this.ViewState["IdClienteBaseGravable"] = _IdRegistro;
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
                    string _MsgMensaje = "No se encontro información de estado financiero con el No. de cuenta seleccionado, por favor validar... !";
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
                    _log.Error(_MsgMensaje);
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al guardar los datos. Motivo: " + ex.ToString();
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

        protected void BtnPrueba_Click(object sender, EventArgs e)
        {
            //this.TxtMunicipio.
        }
    }
}