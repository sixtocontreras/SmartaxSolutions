using System;
using System.Data;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Parametros;
using Smartax.Web.Application.Clases.Seguridad;
using System.Web.Caching;

namespace Smartax.Web.Application.Controles.Parametros.Empresa
{
    public partial class FrmVerInfoEntidad : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        Empresas ObjEmpresa = new Empresas();
        Utilidades ObjUtilidades = new Utilidades();

        public DataSet GetInformacionEntidad()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                //Mostrar copias de funcinarios en el documento
                ObjEmpresa.TipoConsulta = 3;
                ObjEmpresa.IdEmpresa = Request.QueryString["IdEmpresa"].ToString().Trim();
                ObjEmpresa.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();

                ObjetoDataTable = ObjetoDataTable = ObjEmpresa.GetInfoEmpresa();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_empresa"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);
            }
            catch (Exception ex)
            {
                this.LblMensaje.Text = "Error al cargar la información de la entidad. Motivo: " + ex.ToString();
                _log.Error(this.LblMensaje.Text.ToString().Trim());
            }

            return ObjetoDataSet;
        }

        private DataSet FuenteInfoEntidad
        {
            get
            {
                object obj = this.ViewState["_FuenteInfoEntidad"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetInformacionEntidad();
                    this.ViewState["_FuenteInfoEntidad"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteInfoEntidad"];
                }
            }
            set { this.ViewState["_FuenteInfoEntidad"] = value; }
        }

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
            if (!objPermiso.PuedeEliminar)
            {
                RadGrid1.Columns[RadGrid1.Columns.Count - 1].Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //this.AplicarPermisos();
                //ObjUtilidades.CambiarGrillaAEspanol(RadGrid1);
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

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                RadGrid1.DataSource = this.FuenteInfoEntidad;
                RadGrid1.DataMember = "DtEmpresa";
            }
            catch (Exception ex)
            {
                this.LblMensaje.Text = "Error al Intentar Cargar el NeedDataSource, Motivo: " + ex.ToString();
                _log.Error(this.LblMensaje.Text.ToString().Trim());
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
                //this.LblMensaje.Text = "Error con el evento PageIndexChanged. Motivo: " + ex.ToString();
                //_log.Error(this.LblMensaje.Text.ToString().Trim());
            }
        }

    }
}