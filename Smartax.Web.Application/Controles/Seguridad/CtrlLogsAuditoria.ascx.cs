using System;
using System.Drawing;
using System.Data;
using log4net;
using Telerik.Web.UI;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Parametros;
using System.Web.UI.WebControls;
using Smartax.Web.Application.Clases.Parametros.Tipos;

namespace Smartax.Web.Application.Controles.Seguridad
{
    public partial class CtrlLogsAuditoria : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CtrlLogsAuditoria));
        RadWindow Ventana = new RadWindow();

        Empresas ObjEmpresa = new Empresas();
        Usuario ObjUser = new Usuario();
        TipoEvento objTipoEvento = new TipoEvento();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        Utilidades ObjUtils = new Utilidades();

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                ObjAuditoria.TipoConsulta = 2;
                ObjAuditoria.IdTipoEvento = this.CmbEvento.SelectedValue.ToString().Trim().Length > 0 ? this.CmbEvento.SelectedValue.ToString().Trim() : null;
                ObjAuditoria.ModuloApp = null;
                DateTime dtFechaInicial = Convert.ToDateTime(this.DtFechaInicial.SelectedDate);
                DateTime dtFechaFinal = Convert.ToDateTime(this.DtFechaFinal.SelectedDate);
                ObjAuditoria.FechaInicial = Convert.ToString(dtFechaInicial.ToString("yyyy-MM-dd")) + " 00:00:00";
                ObjAuditoria.FechaFinal = Convert.ToString(dtFechaFinal.ToString("yyyy-MM-dd")) + " 23:59:59";
                ObjAuditoria.IdUsuario = null;  //--Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
                ObjAuditoria.IdEmpresa = null;  //--this.CmbEmpresa.SelectedValue.ToString().Trim().Length > 0 ? Int32.Parse(this.CmbEmpresa.SelectedValue.ToString().Trim()) : Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
                ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //--AQUI SE OBTIENE LOS DATOS DE LA DB
                ObjetoDataTable = ObjAuditoria.GetLogsAuditoria();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["idlog_auditoria"] };
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
                string _MsgError = "Error al listar los logs de auditoria. Motivo: " + ex.ToString();
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

            return ObjetoDataSet;
        }

        private DataSet FuenteDatosLogs
        {
            get
            {
                object obj = this.ViewState["_FuenteDatosLogs"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetDatosGrilla();
                    this.ViewState["_FuenteDatosLogs"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatosLogs"];
                }
            }
            set { this.ViewState["_FuenteDatosLogs"] = value; }
        }

        //protected void LstEmpresa()
        //{
        //    try
        //    {
        //        ObjEmpresa.TipoConsulta = 2;
        //        ObjEmpresa.IdEstado = 1;
        //        ObjEmpresa.MostrarSeleccione = "SI";
        //        ObjEmpresa.IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString().Trim());
        //        ObjEmpresa.IdRol = Int32.Parse(Session["IdRol"].ToString().Trim());
        //        ObjEmpresa.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

        //        this.CmbEmpresa.DataSource = ObjEmpresa.GetEmpresas();
        //        this.CmbEmpresa.DataValueField = "id_empresa";
        //        this.CmbEmpresa.DataTextField = "nombre_empresa";
        //        this.CmbEmpresa.DataBind();
        //        LblMensaje.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        LblMensaje.Text = "Error al Listar las empresas. Motivo: " + ex.Message.ToString().Trim();
        //    }
        //}

        //protected void LstUsuario()
        //{
        //    try
        //    {
        //        ObjUser.TipoConsulta = 2;
        //        ObjUser.IdRol = Int32.Parse(this.Session["IdRol"].ToString().Trim());
        //        ObjUser.IdEmpresa = Int32.Parse(this.Session["IdEmpresa"].ToString().Trim());
        //        ObjUser.IdEstado = null;
        //        ObjUser.MostrarSeleccione = "SI";
        //        ObjUser.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

        //        this.CmbUsuario.DataSource = ObjUser.GetUsuarios();
        //        this.CmbUsuario.DataValueField = "id_usuario";
        //        this.CmbUsuario.DataTextField = "nombre_usuario";
        //        this.CmbUsuario.DataBind();
        //        LblMensaje.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        LblMensaje.Text = "Error al Listar los usuarios. Motivo: " + ex.Message.ToString().Trim();
        //    }
        //}

        protected void LstTipoEvento()
        {
            try
            {
                objTipoEvento.TipoConsulta = 2;
                objTipoEvento.IdEstado = 1;
                objTipoEvento.MostrarSeleccione = "SI";
                objTipoEvento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                this.CmbEvento.DataSource = objTipoEvento.GetTipoEvento();
                this.CmbEvento.DataValueField = "idtipo_evento";
                this.CmbEvento.DataTextField = "tipo_evento";
                this.CmbEvento.DataBind();
                LblMensaje.Text = "";
            }
            catch (Exception ex)
            {
                LblMensaje.Text = "Error al Listar los tipos de eventos. Motivo: " + ex.Message.ToString().Trim();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                this.Page.Title = "Logs de Auditoria";
                this.LstTipoEvento();
                //this.LstEmpresa();

                DateTime dFechaActual = DateTime.Now;
                //DateTime dFechaInicial = dFechaFinal.AddDays(-1);

                this.DtFechaInicial.MaxDate = DateTime.Now;
                //this.DtFechaFinal.MaxDate = DateTime.Now;
                this.DtFechaInicial.SelectedDate = dFechaActual;
                this.DtFechaFinal.SelectedDate = dFechaActual.AddDays(1);
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
        }

        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                this.RadWindowManager1.Enabled = false;
                this.RadWindowManager1.EnableAjaxSkinRendering = false;
                this.RadWindowManager1.Visible = false;

                //--
                this.ViewState["DtAuditoria"] = null;
                this.ViewState["_FuenteDatosLogs"] = null;
                this.RadGrid1.Rebind();
            }
            catch (Exception ex)
            {
                _log.Error("Error al realizar la consulta. Motivo: " + ex.Message);
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid1.DataSource = this.FuenteDatosLogs;
                RadGrid1.DataMember = "DtAuditoria";
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error al Intentar Cargar el NeedDataSource, Motivo: " + ex.ToString();
                _log.Error(this.LblMensaje.Text.ToString().Trim());
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnVerInfo")
                {
                    #region VER DETALLE DE LA INFORMACIÓN
                    GridDataItem item = (GridDataItem)e.Item;
                    int _IdLogAuditoria = Int32.Parse(item.GetDataKeyValue("idlog_auditoria").ToString().Trim());
                    string _TipoEvento = item["tipo_evento"].Text.ToString().Trim();
                    string _ModuloApp = item["modulo_app"].Text.ToString().Trim();
                    string _DescripcionEvento = item["descripcion_evento"].Text.ToString().Trim();

                    //--MANDAMOS ABRIR EL FORM COMO POPUP
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;
                    Ventana.Modal = true;

                    Ventana.NavigateUrl = "/Controles/Seguridad/FrmVerDetalleAuditoria.aspx?DescripcionEvento=" + _DescripcionEvento;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(550);
                    Ventana.Width = Unit.Pixel(1100);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Ver Detalle Auditoria. Id: " + _IdLogAuditoria + ", Evento: " + _TipoEvento + ", Modulo: " + _ModuloApp;
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
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
                string _MsgMensaje = "Error con el evento ItemCommand. Motivo: " + ex.ToString();
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

        protected void RadGrid1_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            try
            {
                RadGrid1.Rebind();
            }
            catch (Exception ex)
            {
                this.LblMensaje.ForeColor = Color.Red;
                this.LblMensaje.Text = "Error con el evento RadGrid1_PageIndexChanged. Motivo: " + ex.ToString();
                _log.Error(this.LblMensaje.Text.ToString().Trim());
            }
        }

    }
}