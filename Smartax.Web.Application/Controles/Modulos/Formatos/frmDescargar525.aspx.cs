using log4net;
using Smartax.Web.Application.Clases.Administracion;
using Smartax.Web.Application.Clases.Seguridad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Controles.Modulos.Formatos
{
    public partial class frmDescargar525 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        Cliente ObjCliente = new Cliente();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LstCliente();
            }

        }
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al listar los clientes. Motivo: " + ex.ToString();
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

        protected void BtnEjecutar_Click(object sender, EventArgs e)
        {
            try
            {
                var nom_mes = "XXX";

                switch (TxtPeriodoImpuesto.Text)
                {
                    case "1":
                        nom_mes = "ENE";
                        break;
                    case "2":
                        nom_mes = "FEB";
                        break;
                    case "3":
                        nom_mes = "MAR";
                        break;
                    case "4":
                        nom_mes = "ABR";
                        break;
                    case "5":
                        nom_mes = "MAY";
                        break;
                    case "6":
                        nom_mes = "JUN";
                        break;
                    case "7":
                        nom_mes = "JUL";
                        break;
                    case "8":
                        nom_mes = "AGO";
                        break;
                    case "9":
                        nom_mes = "SEP";
                        break;
                    case "10":
                        nom_mes = "OCT";
                        break;
                    case "11":
                        nom_mes = "NOV";
                        break;
                    case "12":
                        nom_mes = "DIC";
                        break;
                }
                var fileName = Server.MapPath($"../../../Archivos/Formatos/{CmbCliente.SelectedValue}/F525_{nom_mes}_{TxtVigencia.Text}.xlsx");
                if (!File.Exists(fileName))
                {
                    string _MsgError = "No existe un formato generado para el cliente o periodo seleccionado.";
                    this.UpdatePanel1.Update();
                    this.UpdatePanel3.Update();
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
                    Ventana.Width = Unit.Pixel(500);
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
                    string baseUrl = $"{ConfigurationManager.AppSettings["UrlBase"]}/Archivos/Formatos/{CmbCliente.SelectedValue}/F525_{nom_mes}_{TxtVigencia.Text}.xlsx";

                    Response.Redirect(baseUrl);

                }
            }
            catch (Exception ex)
            {
                this.UpdatePanel1.Update();
                this.UpdatePanel3.Update();
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
                string _MsgMensaje = "Señor usuario. Ocurrio un Error al descargar el archivo: " + ex.ToString();
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
    }
}