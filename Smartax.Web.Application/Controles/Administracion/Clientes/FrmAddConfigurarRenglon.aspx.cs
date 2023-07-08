using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.Drawing;
using log4net;
using System.Web.Caching;
using Smartax.Web.Application.Clases.Parametros.Tipos;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Parametros;
using System.Web.Script.Serialization;

namespace Smartax.Web.Application.Controles.Administracion.Clientes
{
    public partial class FrmAddConfigurarRenglon : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        #region DEFINIR OBJETO DE CLASES
        ClienteBaseGravable ObjBaseGrav = new ClienteBaseGravable();
        ClienteEstadosFinanciero ObjClienteEF = new ClienteEstadosFinanciero();
        ClienteEstablecimiento ObjEstablecimiento = new ClienteEstablecimiento();
        //ClientePuc ObjClientePuc = new ClientePuc();
        PlanUnicoCuenta ObjPuc = new PlanUnicoCuenta();
        FormConfiguracion ObjFormConf = new FormConfiguracion();
        Lista ObjLista = new Lista();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        Utilidades ObjUtils = new Utilidades();
        #endregion

        private void GetBaseGravableCuenta()
        {
            try
            {
                ObjBaseGrav.TipoConsulta = 1;
                ObjBaseGrav.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                ObjBaseGrav.IdFormImpuesto = this.ViewState["IdFormImpuesto"].ToString().Trim();
                ObjBaseGrav.IdFormConfiguracion = this.ViewState["IdConfImpuesto"].ToString().Trim();
                ObjBaseGrav.IdClienteEstablecimiento = null;
                ObjBaseGrav.IdPuc = null;
                ObjBaseGrav.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                DataTable dtDatos = new DataTable();
                dtDatos = ObjBaseGrav.GetClienteBaseGravable();

                //if (dtDatos != null)
                //{
                //    if (dtDatos.Rows.Count > 0)
                //    {
                //        this.ViewState["IdClienteBaseGravable"] = dtDatos.Rows[0]["idcliente_base_gravable"].ToString().Trim();
                //        //this.CmbRenglonForm.SelectedValue = dtDatos.Rows[0]["idformulario_configuracion"].ToString().Trim() + "|" + dtDatos.Rows[0]["descripcion_renglon"].ToString().Trim();
                //        this.LblNumRenglon.Text = dtDatos.Rows[0]["numero_renglon"].ToString().Trim();
                //        this.LblDescripcion.Text = dtDatos.Rows[0]["descripcion_renglon"].ToString().Trim();

                //        string _IdClientePuc = dtDatos.Rows[0]["idcliente_puc"].ToString().Trim();
                //        string _DescripcionPuc = dtDatos.Rows[0]["cuenta_contable"].ToString().Trim();
                //        this.TxtCuentaContable.Entries.Clear();
                //        this.TxtCuentaContable.Entries.Add(new AutoCompleteBoxEntry(_DescripcionPuc, _IdClientePuc));

                //        //--AQUI DEFINIMOS CUAL DE LOS VALORES ESTA SELECCIONADO
                //        this.ChkSaldoInicial.Checked = Convert.ToBoolean(dtDatos.Rows[0]["saldo_inicial"].ToString().Trim());
                //        this.ChkMovDebito.Checked = Convert.ToBoolean(dtDatos.Rows[0]["mov_debito"].ToString().Trim());
                //        this.ChkMovCredito.Checked = Convert.ToBoolean(dtDatos.Rows[0]["mov_credito"].ToString().Trim());
                //        this.ChkSaldoFinal.Checked = Convert.ToBoolean(dtDatos.Rows[0]["saldo_final"].ToString().Trim());
                //        this.TxtValorExtracontable.Text = dtDatos.Rows[0]["valor_extracontable"].ToString().Trim();
                //    }
                //    else
                //    {
                //        this.ViewState["IdClienteBaseGravable"] = "";
                //    }
                //}
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
                #endregion
            }
        }

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjBaseGrav.TipoConsulta = 3;
                ObjBaseGrav.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                ObjBaseGrav.IdFormConfiguracion = this.ViewState["IdConfImpuesto"].ToString().Trim();
                ObjBaseGrav.IdClienteEstablecimiento = null;
                ObjBaseGrav.IdPuc = null;
                ObjBaseGrav.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //--OBTENER DATOS DE LA DB
                ObjetoDataTable = ObjBaseGrav.GetBaseGravableRenglon();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idcliente_base_gravable"] };
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
                string _MsgError = "Error al listar las cuentas asociadas al renglon del formulario. Motivo: " + ex.ToString();
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

        public DataSet GetOficinasCliente()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjEstablecimiento.TipoConsulta = 4;
                ObjEstablecimiento.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                ObjEstablecimiento.IdEstablecimientoPadre = null;
                ObjEstablecimiento.AnioGravable = null;
                ObjEstablecimiento.CodigoOficina = this.TxtCodOficina.Text.ToString().Trim().Length > 0 ? this.TxtCodOficina.Text.ToString().Trim().ToUpper() : null;
                ObjEstablecimiento.NombreOficina = this.TxtNombreOficina.Text.ToString().Trim().Length > 0 ? this.TxtNombreOficina.Text.ToString().Trim().ToUpper() : null;
                ObjEstablecimiento.IdEstablecimientoPadre = null;
                ObjEstablecimiento.IdEstado = 1;
                ObjEstablecimiento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //Mostrar los formularios
                ObjetoDataTable = ObjEstablecimiento.GetLstEstablecimientos();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idcliente_establecimiento, id_municipio"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUAURIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Señor usuario. Error al listar las oficinas. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
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
                    ConjuntoDatos = GetDatosGrilla();
                    this.ViewState["_FuenteDatos"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatos"];
                }
            }
            set { this.ViewState["_FuenteDatos"] = value; }
        }

        private DataSet FuenteDatosOficinas
        {
            get
            {
                object obj = this.ViewState["_FuenteDatosOficinas"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetOficinasCliente();
                    this.ViewState["_FuenteDatosOficinas"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatosOficinas"];
                }
            }
            set { this.ViewState["_FuenteDatosOficinas"] = value; }
        }

        private DataTable LstCuentasContable()
        {
            DataTable dtCuentasContable = new DataTable();
            try
            {
                //Verificar si existen los datos en la cache
                string _IdCliente = this.ViewState["IdCliente"] != null ? this.ViewState["IdCliente"].ToString().Trim() : "4";
                string _CacheCuentasCliente = FixedData.GetCacheCuentasCliente + _IdCliente;
                if (Cache.Get(_CacheCuentasCliente) == null)
                {
                    ObjPuc.TipoConsulta = 2;
                    ObjPuc.IdCliente = _IdCliente;  //--this.ViewState["IdCliente"].ToString().Trim();
                    ObjPuc.IdEstado = 1;
                    ObjPuc.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                    string _MsgError = "";
                    dtCuentasContable = ObjPuc.GetCuentasCliente();
                    if (dtCuentasContable != null)
                    {
                        _log.Warn("PASO 1: CACHE => " + _CacheCuentasCliente);
                        Cache.Add(_CacheCuentasCliente, dtCuentasContable, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Default, null);

                        //Aqui Cargamos el DataTable en el TextBox de los Convenios.
                        this.TxtCuentaContable.DataSource = dtCuentasContable;
                        this.TxtCuentaContable.DataTextField = "cuenta_contable";
                        this.TxtCuentaContable.DataValueField = "id_puc";
                        this.TxtCuentaContable.DataBind();
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
                else
                {
                    _log.Warn("PASO 2: CACHE => " + _CacheCuentasCliente);
                    DataTable DtDatos = new DataTable();
                    DtDatos = (DataTable)Cache.Get(_CacheCuentasCliente);
                    dtCuentasContable = DtDatos.Copy();

                    //Aqui Cargamos el DataTable en el TextBox de los Convenios.
                    this.TxtCuentaContable.DataSource = dtCuentasContable;
                    this.TxtCuentaContable.DataTextField = "cuenta_contable";
                    this.TxtCuentaContable.DataValueField = "id_puc";
                    this.TxtCuentaContable.DataBind();
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                dtCuentasContable = null;
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Error al listar cuentas contables del cliente. Motivo: " + ex.Message;
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

            return dtCuentasContable;
        }

        //private DataTable LstCuentasContable_Old()
        //{
        //    DataTable dtCuentasContable = new DataTable();
        //    try
        //    {
        //        //Verificar si existen los datos en la cache
        //        string _CacheCuentasCliente = FixedData.GetCacheCuentasCliente + this.ViewState["IdCliente"].ToString().Trim();
        //        if (Cache.Get(_CacheCuentasCliente) == null)
        //        {
        //            ObjClientePuc.TipoConsulta = 2;
        //            ObjClientePuc.IdCliente = this.ViewState["IdCliente"].ToString().Trim();
        //            ObjClientePuc.CodigoCuenta = null;
        //            ObjClientePuc.NombreCuenta = null;
        //            ObjClientePuc.BaseGravable = null;
        //            ObjClientePuc.IdEstado = null;
        //            ObjClientePuc.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

        //            string _MsgError = "";
        //            dtCuentasContable = ObjClientePuc.GetCuentasCliente();
        //            if (dtCuentasContable != null)
        //            {
        //                Cache.Add(_CacheCuentasCliente, dtCuentasContable, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Default, null);

        //                //Aqui Cargamos el DataTable en el TextBox de los Convenios.
        //                this.TxtCuentaContable.DataSource = dtCuentasContable;
        //                this.TxtCuentaContable.DataTextField = "cuenta_contable";
        //                this.TxtCuentaContable.DataValueField = "idcliente_puc";
        //                this.TxtCuentaContable.DataBind();
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
        //                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
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
        //            DataTable DtDatos = new DataTable();
        //            DtDatos = (DataTable)Cache.Get(_CacheCuentasCliente);
        //            dtCuentasContable = DtDatos.Copy();

        //            //Aqui Cargamos el DataTable en el TextBox de los Convenios.
        //            this.TxtCuentaContable.DataSource = dtCuentasContable;
        //            this.TxtCuentaContable.DataTextField = "cuenta_contable";
        //            this.TxtCuentaContable.DataValueField = "idcliente_puc";
        //            this.TxtCuentaContable.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        #region MOSTRAR MENSAJE DE USUARIO
        //        dtCuentasContable = null;
        //        //Mostramos el mensaje porque se produjo un error con la Trx.
        //        this.RadWindowManager1.ReloadOnShow = true;
        //        this.RadWindowManager1.DestroyOnClose = true;
        //        this.RadWindowManager1.Windows.Clear();
        //        this.RadWindowManager1.Enabled = true;
        //        this.RadWindowManager1.EnableAjaxSkinRendering = true;
        //        this.RadWindowManager1.Visible = true;

        //        RadWindow Ventana = new RadWindow();
        //        Ventana.Modal = true;
        //        string _MsgError = "Error al listar cuentas contables del cliente. Motivo: " + ex.Message;
        //        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
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
        //        _log.Error(_MsgError);
        //        #endregion
        //    }

        //    return dtCuentasContable;
        //}

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objNavegacion.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = Request.QueryString["PathUrl"].ToString().Trim();
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
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);

                //--AQUI OBTENEMOS LOS VALORES PASADOS POR PARAMETROS
                this.ViewState["IdClienteBaseGravable"] = "";
                this.ViewState["IdConfImpuesto"] = Request.QueryString["IdConfImpuesto"].ToString().Trim();
                this.ViewState["IdFormImpuesto"] = Request.QueryString["IdFormImpuesto"].ToString().Trim();
                this.ViewState["IdCliente"] = Request.QueryString["IdCliente"].ToString().Trim();
                this.LblNombreFormulario.Text = Request.QueryString["NombreImpuesto"].ToString().Trim();
                this.LblNumRenglon.Text = Request.QueryString["NumeroRenglon"].ToString().Trim();
                this.LblDescripcion.Text = Request.QueryString["DescripcionRenglon"].ToString().Trim();

                //--AQUI LLAMAMOS LA LISTA.
                this.LstCuentasContable();

                //--CONSULTAMOS LOS VALORES DE LA BASE GRAVABLE
                this.GetBaseGravableCuenta();
                this.TxtCuentaContable.Focus();
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
                //--METODO PARA LISTAR CUENTAS CONTABLES DEL CLIENTE.
                this.LstCuentasContable();
            }
        }

        #region DEFINICION DE METODOS DEL GRID 1
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid1.DataSource = this.FuenteDatos;
                this.RadGrid1.DataMember = "DtBaseGravable";
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
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del Tipo de Naturaleza. Motivo: " + ex.ToString();
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
                if (e.CommandName == "BtnAddEstablecimiento")
                {
                    #region DEFINICION DE METODO PARA BLOQUEAR LA CUENTA 
                    //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                    this.RadWindowManager1.Enabled = false;
                    this.RadWindowManager1.EnableAjaxSkinRendering = false;
                    this.RadWindowManager1.Visible = false;

                    GridDataItem item = (GridDataItem)e.Item;
                    this.ViewState["IdClienteBaseGravable"] = Convert.ToInt32(item.GetDataKeyValue("idcliente_base_gravable").ToString().Trim());

                    this.LblMensaje.Text = "";
                    this.ModalPopupExtender1.Show();
                    this.RadGrid4.Rebind();
                    #endregion
                }
                else if (e.CommandName == "BtnBloquear")
                {
                    #region DEFINICION DE METODO PARA DISOCIAR LA CUENTA 
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdClienteBaseGravable = Convert.ToInt32(item.GetDataKeyValue("idcliente_base_gravable").ToString().Trim());
                    int _IdEstado = Int32.Parse(item["id_estado"].Text.ToString().Trim());

                    //Disociamos la cuenta seleccionada
                    ObjBaseGrav.IdClienteBaseGravable = _IdClienteBaseGravable;
                    ObjBaseGrav.IdCliente = this.ViewState["IdCliente"].ToString().Trim();
                    ObjBaseGrav.IdFormConfiguracion = this.ViewState["IdFormImpuesto"].ToString().Trim();
                    ObjBaseGrav.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    ObjBaseGrav.IdClienteEstablecimiento = null;
                    ObjBaseGrav.IdEstado = _IdEstado == 1 ? 0 : 1;
                    ObjBaseGrav.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                    ObjBaseGrav.TipoProceso = 4;

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjBaseGrav.AddClienteBaseGravable(ref _IdRegistro, ref _MsgError))
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

                        this.ViewState["_FuenteDatos"] = null;
                        this.RadGrid1.Rebind();
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
                    #endregion
                }
                else if (e.CommandName == "BtnDisociar")
                {
                    #region DEFINICION DE METODO PARA DISOCIAR LA CUENTA 
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdClienteBaseGravable = Convert.ToInt32(item.GetDataKeyValue("idcliente_base_gravable").ToString().Trim());

                    //Disociamos la cuenta seleccionada
                    ObjBaseGrav.IdClienteBaseGravable = _IdClienteBaseGravable;
                    ObjBaseGrav.IdCliente = this.ViewState["IdCliente"].ToString().Trim();
                    ObjBaseGrav.IdFormConfiguracion = this.ViewState["IdFormImpuesto"].ToString().Trim();
                    ObjBaseGrav.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    ObjBaseGrav.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                    ObjBaseGrav.TipoProceso = 5;

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjBaseGrav.AddClienteBaseGravable(ref _IdRegistro, ref _MsgError))
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

                        this.ViewState["_FuenteDatos"] = null;
                        this.RadGrid1.Rebind();
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
                string _MsgError = "Error con el evento RadGrid1_PageIndexChanged del Tipo de Naturaleza. Motivo: " + ex.ToString();
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

        #region DEFINICION EVENTOS OBJETOS DEL FORMULARIO
        protected void ChkSaldoInicial_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                #region OBTENEMOS VALORES PARA REALIZAR LA SUMATORIA
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                if (this.ChkSaldoInicial.Checked == true)
                {
                    this.ChkMovDebito.Checked = false;
                    this.ChkMovCredito.Checked = false;
                    this.ChkSaldoFinal.Checked = false;
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

                if (this.ChkMovDebito.Checked == true)
                {
                    this.ChkSaldoInicial.Checked = false;
                    this.ChkMovCredito.Checked = false;
                    this.ChkSaldoFinal.Checked = false;
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

                if (this.ChkMovCredito.Checked == true)
                {
                    this.ChkSaldoInicial.Checked = false;
                    this.ChkMovDebito.Checked = false;
                    this.ChkSaldoFinal.Checked = false;
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

                if (this.ChkSaldoFinal.Checked == true)
                {
                    this.ChkSaldoInicial.Checked = false;
                    this.ChkMovDebito.Checked = false;
                    this.ChkMovCredito.Checked = false;
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

        #region DEFINICION DE OBJETOS GRID 4
        protected void RadGrid4_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid4.DataSource = this.FuenteDatosOficinas;
                this.RadGrid4.DataMember = "DtEstablecimientos";
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error con el evento NeedDataSource. Motivo: " + ex.ToString();
            }
        }

        protected void RadGrid4_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnAsociar")
                {
                    #region DEFINICION DE METODO PARA DISOCIAR LA CUENTA 
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdClienteEstablecimiento = Convert.ToInt32(item.GetDataKeyValue("idcliente_establecimiento").ToString().Trim());
                    string _CodigoOficina = item["codigo_oficina"].Text.ToString().Trim();
                    string _NombreOficina = item["nombre_oficina"].Text.ToString().Trim();

                    //Disociamos la cuenta seleccionada
                    ObjBaseGrav.IdClienteBaseGravable = this.ViewState["IdClienteBaseGravable"].ToString().Trim();
                    ObjBaseGrav.IdCliente = this.ViewState["IdCliente"].ToString().Trim();
                    ObjBaseGrav.IdFormConfiguracion = this.ViewState["IdFormImpuesto"].ToString().Trim();
                    ObjBaseGrav.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                    ObjBaseGrav.IdEstado = 1;
                    ObjBaseGrav.IdUsuario = Convert.ToInt32(Session["IdUsuario"].ToString().Trim());
                    ObjBaseGrav.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                    ObjBaseGrav.TipoProceso = 3;

                    int _IdRegistro = 0;
                    string _MsgError = "";
                    if (ObjBaseGrav.AddClienteBaseGravable(ref _IdRegistro, ref _MsgError))
                    {
                        //--AQUI BUSCAMOS EL ID DE LA ACTIVIDAD SELECCIONADA
                        DataRow[] dataRows = this.FuenteDatos.Tables["DtBaseGravable"].Select("idcliente_base_gravable = " + ObjBaseGrav.IdClienteBaseGravable);
                        if (dataRows.Length == 1)
                        {
                            dataRows[0]["idcliente_establecimiento"] = _IdClienteEstablecimiento.ToString();
                            dataRows[0]["nombre_oficina"] = _NombreOficina;
                            //--AQUI ACTUALIZAMOS EL DATATAABLE CON EL ESTADO SELECCIONADO
                            this.FuenteDatos.Tables["DtBaseGravable"].Rows[0].AcceptChanges();
                            this.FuenteDatos.Tables["DtBaseGravable"].Rows[0].EndEdit();

                            //--ACTUALIZAR LA LISTA DE LAS ACTIVIDADES
                            this.UpdatePanel1.Update();
                            //this.ViewState["_FuenteDatos"] = null;
                            this.RadGrid1.Rebind();
                            this.LblMensaje.Text = "";
                            this.ModalPopupExtender1.Hide();
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
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error con el evento ItemCommand. Motivo: " + ex.ToString();
            }
        }

        protected void RadGrid4_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            try
            {
                this.RadGrid4.Rebind();
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error con el evento PageIndexChanged. Motivo: " + ex.ToString();
            }
        }

        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            this.ViewState["_FuenteDatosOficinas"] = null;
            this.RadGrid4.Rebind();
        }

        protected void BtnSalir1_Click(object sender, EventArgs e)
        {
            this.LblMensaje.Text = "";
            this.ModalPopupExtender1.Hide();
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
                //ObjBaseGrav.IdClienteBaseGravable = this.ViewState["IdClienteBaseGravable"].ToString().Trim().Length > 0 ? this.ViewState["IdClienteBaseGravable"].ToString().Trim() : null;
                ObjBaseGrav.IdClienteBaseGravable = null;
                ObjBaseGrav.IdCliente = Int32.Parse(this.ViewState["IdCliente"].ToString().Trim());
                ObjBaseGrav.IdFormConfiguracion = this.ViewState["IdConfImpuesto"].ToString().Trim();

                string[] _ArrayData = this.TxtCuentaContable.Text.ToString().Trim().Split('-');
                ObjBaseGrav.IdPuc = _ArrayData[0].ToString().Trim();

                ObjBaseGrav.AnioGravable = DateTime.Now.ToString("yyyy");
                ObjBaseGrav.SaldoInicial = this.ChkSaldoInicial.Checked == true ? "S" : "N";
                ObjBaseGrav.MovDebito = this.ChkMovDebito.Checked == true ? "S" : "N";
                ObjBaseGrav.MovCredito = this.ChkMovCredito.Checked == true ? "S" : "N";
                ObjBaseGrav.SaldoFinal = this.ChkSaldoFinal.Checked == true ? "S" : "N";
                ObjBaseGrav.ValorExtracontable = this.TxtValorExtracontable.Text.ToString().Trim().Length > 0 ? this.TxtValorExtracontable.Text.ToString().Trim().Replace(".", "") : "0";
                ObjBaseGrav.IdClienteEstablecimiento = null;
                ObjBaseGrav.IdEstado = 1;
                ObjBaseGrav.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjBaseGrav.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                //ObjBaseGrav.TipoProceso = this.ViewState["IdClienteBaseGravable"].ToString().Trim().Length > 0 ? 2 : 1;
                ObjBaseGrav.TipoProceso = 1;

                //--AQUI SERIALIZAMOS EL OBJETO CLASE
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(ObjBaseGrav);
                #endregion

                int _IdRegistro = 0;
                string _MsgError = "";
                if (ObjBaseGrav.AddClienteBaseGravable(ref _IdRegistro, ref _MsgError))
                {
                    #region REGISTRO DE LOGS DE AUDITORIA
                    //--AQUI REGISTRAMOS EN LOS LOGS DE AUDITORIA
                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                    ObjAuditoria.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                    ObjAuditoria.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                    ObjAuditoria.ModuloApp = "CONFIGURAR_RENGLON";
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
                    //--LIMPIAMOS VALORES
                    this.TxtCuentaContable.Entries.Clear();
                    this.ChkSaldoInicial.Checked = false;
                    this.ChkMovDebito.Checked = false;
                    this.ChkMovCredito.Checked = false;
                    this.ChkSaldoFinal.Checked = false;
                    this.TxtValorExtracontable.Text = "";
                    this.TxtCuentaContable.Focus();
                    //--ACTUALIZAR LISTA
                    this.ViewState["_FuenteDatos"] = null;
                    this.RadGrid1.Rebind();
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

    }
}