using log4net;
using System;
using System.Web.UI;
using System.IO;
using System.Data;
using System.Web.Caching;
using Smartax.Web.Application.Clases.Seguridad;

namespace Smartax.Web.Application.Controles.General
{
    public partial class CtrlMenu : System.Web.UI.UserControl
    {
        protected static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //this.RadMenu1.Visible = false;
                if (string.IsNullOrEmpty(Convert.ToString(this.Session["Login"])))
                {
                    //this.RadMenu1.Visible = false;
                }
                else if (this.Session["Login"].Equals(1))
                {
                    //this.RadMenu1.Visible = true;
                    this.MenuDinamico.Visible = false;
                }
                else
                {
                    if ((this.Session["CambiarClave"].ToString().Trim().Length > 0))
                    {
                        if ((this.Session["CambiarClave"].ToString().Trim() == "N"))
                        {
                            CargaMenu();
                            MenuPreRender();
                        }
                        else
                        {
                            //this.RadMenu1.Visible = true;
                            if ((this.Session["CambiarClave"].ToString().Trim() == "S") & (Request.ServerVariables["SCRIPT_NAME"].ToString().ToLower()) != "/FrmCambioClave.aspx")
                            {
                                //this.RadMenu1.Visible = true;
                                this.Session["Login"] = 1;
                                //Response.Redirect("FrmClave.aspx");
                            }
                        }
                    }
                    else
                    {
                        //this.RadMenu1.Visible = true;
                        if ((Session["CambiarClave"].ToString().Trim().Length == 0) & (Request.ServerVariables["SCRIPT_NAME"].ToString().ToLower()) != "/FrmClave.aspx")
                        {
                            //this.RadMenu1.Visible = true;
                            Session["Login"] = 1;
                            Response.Redirect("FrmClave.aspx");
                        }
                    }
                }
            }
        }

        private void CargaMenu()
        {
            try
            {
                DataSet MenuDS = new DataSet();
                SistemaNavegacion objNavigation = new SistemaNavegacion();
                SistemaPermiso objPermiso = new SistemaPermiso();
                objNavigation.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                objNavigation.IdRol = Int32.Parse(Session["IdRol"].ToString().Trim());
                objNavigation.MotorBaseDatos = Session["MotorBaseDatos"].ToString().Trim();
                objPermiso.IdUsuario = Int32.Parse(Session["IdUsuario"].ToString().Trim());
                objPermiso.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                string DatosCache = Session["DtCacheMenu"].ToString().Trim();
                if (Cache.Get(DatosCache) == null)
                {
                    MenuDS = objNavigation.GetMenu();
                    Cache.Add(DatosCache, MenuDS, null, DateTime.Now.AddHours(24), TimeSpan.Zero, CacheItemPriority.Default, null);
                }
                else
                {
                    MenuDS = (DataSet)Cache.Get(DatosCache);
                }

                if ((MenuDS.Tables["DtMenu"] != null))
                {
                    if (MenuDS.Tables["DtMenu"].Rows.Count > 0)
                    {
                        this.MenuDinamico.DataSource = MenuDS;
                        this.MenuDinamico.DataFieldID = "MenuID";
                        this.MenuDinamico.DataValueField = "MenuID";
                        this.MenuDinamico.DataNavigateUrlField = "url_opcion".ToString().Trim();
                        this.MenuDinamico.DataTextField = "titulo_opcion";
                        this.MenuDinamico.Font.Size = 16;

                        if ((MenuDS.Tables["DtMenu"].Columns["ParentID"] != null))
                        {
                            this.MenuDinamico.DataFieldParentID = "ParentID";
                        }

                        this.MenuDinamico.DataBind();
                    }
                    else
                    {
                        MenuDS = null;
                        this.MenuDinamico.Visible = false;
                    }
                }
                else
                {
                    MenuDS = null;
                    this.MenuDinamico.Visible = false;
                }
            }
            catch (Exception ex)
            {
                //logger.Error("Error al cargar el menu. Motivo: " & ex.Message.ToString.Trim)
            }
        }

        private void MenuPreRender()
        {
            foreach (Telerik.Web.UI.RadMenuItem MenuItem in MenuDinamico.Items)
            {
                if (MenuItem.Items.Count > 0)
                {
                    CargaMenuImagenes(MenuItem);
                }
            }
        }

        private void CargaMenuImagenes(Telerik.Web.UI.RadMenuItem Item)
        {
            foreach (Telerik.Web.UI.RadMenuItem MenuItem in Item.Items)
            {
                FileInfo Archivo = new FileInfo(MapPath("/Imagenes/Menu/16/" + MenuItem.Value + ".png"));
                if (Archivo.Exists)
                {
                    MenuItem.ImageUrl = "/Imagenes/Menu/16/" + MenuItem.Value + ".png";
                }
                if (MenuItem.Items.Count > 0)
                {
                    CargaMenuImagenes(MenuItem);
                }
            }
        }

    }
}