using log4net;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Reportes.Formatos;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Misc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Reportes.Formatos
{
    public partial class Ctrl321 : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        #region DEFINICION DE OBJETOS DE CLASES
        Cliente ObjCliente = new Cliente();
        Admin321 _admin321 = new Admin321();
        #endregion


        #region DEFINICION METODOS LISTAR COMBOBOX
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
                Ventana.Height = Unit.Pixel(260);
                Ventana.Width = Unit.Pixel(550);
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

        private void AplicarPermisos()
        {
            SistemaPermiso objPermiso = new SistemaPermiso();
            SistemaNavegacion objNavegacion = new SistemaNavegacion();

            objNavegacion.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
            objNavegacion.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
            //objPermiso.PathUrl = Request.QueryString["PathUrl"].ToString().Trim();
            objPermiso.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

            objPermiso.RefrescarPermisos();
            if (!objPermiso.PuedeRegistrar)
            {
                //this.BtnGenerar.Enabled = false;
            }
        }

        protected void BtnGenerar_Click(object sender, EventArgs e)
        {            
            DataTable datos = _admin321.Get(int.Parse(this.CmbCliente.SelectedValue.Trim()), int.Parse(this.TxtVigencia.Text.Trim()),int.Parse(this.TxtPeriodoImpuesto.Text.Trim()));
            datos.Columns[0].ColumnName = "Codigo Empresa";
            datos.Columns[1].ColumnName = "Vigencia";
            datos.Columns[2].ColumnName = "Periodo";
            datos.Columns[3].ColumnName = "Unidad de Captura";
            datos.Columns[4].ColumnName = "Nombre Departamento";
            datos.Columns[5].ColumnName = "Subcuenta";
            datos.Columns[6].ColumnName = "Nombre Municipio";
            datos.Columns[7].ColumnName = "Columna";
            datos.Columns[8].ColumnName = "Valor";
            foreach (DataRow item in datos.Rows)
            {
                item[8] = item[8].ToString().Replace(",", ".");
            }
            byte[] bytes = MiscReportes.DataTableByArrayBite(datos, true, "|");
            string nombre = "st_saldos_contrarios_f321_" + this.TxtPeriodoImpuesto.Text.Trim() +"_" + this.TxtVigencia.Text.Trim() + ".csv";

            Response.Clear();
            Response.ContentType = "application/CSV";
            Response.AddHeader("content-disposition", "attachment;    filename=" + nombre);
            Response.BinaryWrite(bytes);
            Response.End();
        }
    }
}