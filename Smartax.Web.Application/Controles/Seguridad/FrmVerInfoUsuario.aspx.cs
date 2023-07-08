using System;
using System.Data;
using Telerik.Web.UI;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using System.Web.Caching;

namespace Smartax.Web.Application.Controles.Seguridad
{
    public partial class FrmVerInfoUsuario : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        Usuario ObjUser = new Usuario();
        Utilidades ObjUtilidades = new Utilidades();

        public DataSet GetInformacionUsuario()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            try
            {
                //Mostrar copias de funcinarios en el documento
                ObjUser.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                ObjUser.IdEmpresa = Int32.Parse(Session["IdEmpresa"].ToString().Trim());
                ObjUser.IdUsuario = Request.QueryString["IdUsuario"].ToString().Trim(); ;

                ObjetoDataTable = ObjetoDataTable = ObjUser.GetInfoUsuario();
                ObjetoDataTable.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_usuario"] };
                ObjetoDataSet.Tables.Add(ObjetoDataTable);
            }
            catch (Exception ex)
            {
                this.LblMensaje.Text = "Error al cargar la información del funcionario. Motivo: " + ex.ToString();
                _log.Error(this.LblMensaje.Text.ToString().Trim());
            }

            return ObjetoDataSet;
        }

        private DataSet FuenteInfoUser
        {
            get
            {
                object obj = this.ViewState["_FuenteInfoUser"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetInformacionUsuario();
                    this.ViewState["_FuenteInfoUser"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteInfoUser"];
                }
            }
            set { this.ViewState["_FuenteInfoUser"] = value; }
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
                RadGrid1.DataSource = this.FuenteInfoUser;
                RadGrid1.DataMember = "DtUsuario";
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