using log4net;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Reportes.Provision;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Misc;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Reportes.Provision
{
    public partial class CtrlRepAcumuladoProvICA : UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        Cliente ObjCliente = new Cliente();
        AcumuladoProv ObjCompProv = new AcumuladoProv();

        protected void LstCliente()
        {
            try
            {
                ObjCliente.TipoConsulta = 2;
                ObjCliente.IdEstado = 1;
                ObjCliente.MostrarSeleccione = "SI";
                ObjCliente.IdRol = Convert.ToInt32(Session["IdRol"].ToString().Trim());
                ObjCliente.IdEmpresa = Convert.ToInt32(Session["IdEmpresa"].ToString().Trim());
                ObjCliente.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                this.CmbCliente.DataSource = ObjCliente.GetClientes();
                this.CmbCliente.DataValueField = "id_cliente";
                this.CmbCliente.DataTextField = "nombre_cliente";
                this.CmbCliente.DataBind();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los tipos de documentos. Motivo: " + ex.ToString();
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

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.PathUrl = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
            objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

            objPermiso.RefrescarPermisos();
            if (!objPermiso.PuedeRegistrar)
            {
                //this.BtnGenerar.Enabled = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.BtnGenerar);
            if (!(this.Page.IsPostBack))
            {
                this.AplicarPermisos();

                //Llenar los combox
                this.LstCliente();
            }
        }

        protected void BtnGenerar_Click(object sender, EventArgs e)
        {

            ObjCompProv.IdCliente = this.CmbCliente.SelectedValue.Trim();
            ObjCompProv.Vigencia = Int32.Parse(this.TxtVigencia.Text.Trim());
            ObjCompProv.PeriodoImpuesto = Int32.Parse(this.TxtPeriodoImpuesto1.Text.Trim()); ;
            ObjCompProv.IdUsuario = Int32.Parse(this.Session["IdUsuario"].ToString().Trim());
            ObjCompProv.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            DataTable datos = ObjCompProv.GetReport();

            byte[] bytes = MiscReportes.DataTableByArrayBite(datos, true, "|");
            string nombre = "st_reporte_acumulado_prov_ica_" + ObjCompProv.PeriodoImpuesto.ToString().PadLeft(2, '0') + "_" + ObjCompProv.Vigencia + ".csv";

            Response.Clear();
            Response.ContentType = "application/CSV";
            Response.AddHeader("content-disposition", "attachment;    filename=" + nombre);
            Response.BinaryWrite(bytes);
            Response.End();
        }
    }
}