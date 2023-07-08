using log4net;
using System;
using System.Configuration;
using System.Data;
using System.Text;
using Telerik.Web.UI;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data.SqlClient;
using Devart.Data.PostgreSql;
using MySql.Data.MySqlClient;
using Smartax.Web.Application.Clases.Seguridad;

namespace Smartax.Web.Application
{
    public partial class Default : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        RadWindow Ventana = new RadWindow();
        Utilidades ObjUtils = new Utilidades();
        LogsAuditoria ObjAuditoria = new LogsAuditoria();
        EnvioCorreo ObjCorreo = new EnvioCorreo();

        #region DEFINICION DE OBJETOS DE BASE DE DATOS
        public static IDbConnection myConnectionDb;
        //IDbConnection myConnectionDb = null;
        string connString = "";
        StringBuilder sSQL = new StringBuilder();

        MySqlCommand TheCommandMySQL = null;
        MySqlDataReader TheDataReaderMySQL = null;

        PgSqlCommand TheCommandPostgreSQL = null;
        PgSqlDataReader TheDataReaderPostgreSQL = null;
        PgSqlParameter NpParam = null;

        SqlCommand TheCommandSQLServer = null;
        SqlDataReader TheDataReaderSQLServer = null;

        OracleCommand TheCommandOracle = null;
        OracleDataReader TheDataReaderOracle = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //Aqui se recoge la informacion de los datos parametrizados en el web.config
                this.Session["MotorBaseDatos"] = ConfigurationManager.AppSettings["BaseDatosUtilizar"].ToString().Trim();
                this.Session["DirectorioVirtual"] = ConfigurationManager.AppSettings["DirectorioVirtual"].ToString().Trim();
                this.Session["DirectorioArchivos"] = ConfigurationManager.AppSettings["DirectorioArchivos"].ToString().Trim();
                this.Session["DiasExpClave"] = ConfigurationManager.AppSettings["DIAS_EXP_CLAVE"].ToString().Trim();
                this.Session["IntentosClaveInvalida"] = ConfigurationManager.AppSettings["INTENTOS_CLAVE_INVALIDA"].ToString().Trim();

                //Datos de servidor de correo.
                this.Session["ServerCorreo"] = ConfigurationManager.AppSettings["SERVER_CORREO_GMAIL"];
                this.Session["PuertoCorreo"] = ConfigurationManager.AppSettings["PUERTO_CORREO_GMAIL"];

                //Datos de email para enviar los correos.
                this.Session["EmailSoporte"] = ConfigurationManager.AppSettings["UsuarioEmail"];
                this.Session["PasswordEmail"] = ConfigurationManager.AppSettings["PasswordEmail"];

                this.TxtUsuario.Text = "";
                this.TxtPassword.Text = "";
                this.TxtUsuario.Focus();
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

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                #region LLAMADO DEL METODO OPARA VALIDAR EL RECAPTCHA DE GOOGLE
                //--Aqui Validamos el Recaptcha obtenido por el usuario.
                if (FixedData.ValidRecaptcha.ToString().Trim().Equals("S"))
                {
                    if (!RecaptchaValidator.IsReCaptchaValid(Request.Form[FixedData.GoogleRecaptchaResponse]))
                    {
                        #region MOSTRAR MENSAJE DE USUARIO
                        this.UpdatePanel1.Update();
                        //Mostramos el mensaje porque se produjo un error con la Trx.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgMensaje = "Señor usuario. El Recaptcha obtenido no es valido. Intentelo nuevamente... !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                        return;
                    }
                }
                #endregion

                #region DEFINICION DEL CONEXION STRING DE LA BD
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((Session["MotorBaseDatos"].ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((Session["MotorBaseDatos"].ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySql.Data.MySqlClient.MySqlConnection(connString);
                }
                else if ((Session["MotorBaseDatos"].ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((Session["MotorBaseDatos"].ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    //this.LblMsg.ForeColor = Color.Red;
                    //this.LblMsg.Text = "Señor usuario. No existe configurado un Motor de Base de Datos a Trabajar !";
                    return;
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }
                #endregion

                #region DEFINICION DEL CONEXION STRING DE LA BD
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((Session["MotorBaseDatos"].ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((Session["MotorBaseDatos"].ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySql.Data.MySqlClient.MySqlConnection(connString);
                }
                else if ((Session["MotorBaseDatos"].ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((Session["MotorBaseDatos"].ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    //this.LblMsg.ForeColor = Color.Red;
                    //this.LblMsg.Text = "Señor usuario. No existe configurado un Motor de Base de Datos a Trabajar !";
                    return;
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }
                #endregion

                //Aqui Obtenemos el Hash del Usuario y Password ingresado.
                string _Hash = ObjUtils.GetHashPassword(this.TxtUsuario.Text.ToString().Trim(), this.TxtPassword.Text.ToString().Trim());

                //Aqui hacemos los llamados de los sp o consultas a utilizar en la respectiva base de datos
                if (myConnectionDb is PgSqlConnection)
                {
                    #region DEFINICION DEL STORE PROCEDURE
                    //--Aqui Iniciamos el store procedure
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_login_web", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_usuario", this.TxtUsuario.Text.ToString().Trim().ToUpper());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_password", _Hash.ToString().Trim());
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();
                    #endregion

                    int EstadoUser = 0;
                    DateTime dtFechaExpiraClave;
                    if (TheDataReaderPostgreSQL != null)
                    {
                        if (TheDataReaderPostgreSQL.Read())
                        {
                            //this.LblMsg.Text = "";
                            string _OutRespuesta = TheDataReaderPostgreSQL["p_out_cod_rpta"].ToString().Trim();
                            string _OutMensaje = TheDataReaderPostgreSQL["p_out_msg_rpta"].ToString().Trim();

                            if (_OutRespuesta.Equals("00"))
                            {
                                EstadoUser = Convert.ToInt32(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                                if (EstadoUser == 1)
                                {
                                    #region DEFINICION DE LA FECHA DE CADUCIDAD
                                    dtFechaExpiraClave = TheDataReaderPostgreSQL["fecha_exp_clave"].ToString().Trim().Length > 0 ? Convert.ToDateTime(TheDataReaderPostgreSQL["fecha_exp_clave"].ToString().Trim()) : DateTime.Now;
                                    //Aqui validamos si la empresa esta como tipo CLIENTE o es de PRUEBAS
                                    if (Int32.Parse(TheDataReaderPostgreSQL["idtipo_licencia"].ToString().Trim()) != 1)
                                    {
                                        //Aqui validamos si la empresa es Padre
                                        DateTime _FechaCaducidad = DateTime.Now;
                                        if (Int32.Parse(TheDataReaderPostgreSQL["idtipo_licencia"].ToString().Trim()) == 2)
                                        {
                                            //Aqui obtenemos la fecha de validacion de la mensualidad
                                            _FechaCaducidad = Convert.ToDateTime(TheDataReaderPostgreSQL["fecha_mensualidad"].ToString().Trim()).AddDays(5);
                                        }
                                        else if (Int32.Parse(TheDataReaderPostgreSQL["idtipo_licencia"].ToString().Trim()) == 3)
                                        {
                                            //Aqui obtenemos la fecha de validacion de las pruebas
                                            _FechaCaducidad = Convert.ToDateTime(TheDataReaderPostgreSQL["fecha_pruebas"].ToString().Trim()).AddDays(5);
                                        }

                                        //Aqui validamos si la fecha de Pago ya caduco
                                        if (DateTime.Now > _FechaCaducidad)
                                        {
                                            string _MsgResult = "";
                                            if (Int32.Parse(TheDataReaderPostgreSQL["idtipo_licencia"].ToString().Trim()) == 2)
                                            {
                                                _MsgResult = "Señor Usuario: Para informarle que la fecha de Pago de la Mensualidad ya caduco el [" + Convert.ToString(_FechaCaducidad.ToString("dd-MM-yyyy")) + "]. Póngase en contacto con el Administrador ó el Proveedor del sistema !";
                                            }
                                            else if (Int32.Parse(TheDataReaderPostgreSQL["idtipo_licencia"].ToString().Trim()) == 3)
                                            {
                                                _MsgResult = "Señor Usuario: Para informarle que la fecha de Pruebas ya caduco el [" + Convert.ToString(_FechaCaducidad.ToString("dd-MM-yyyy")) + "]. Póngase en contacto con el Administrador ó el Proveedor del sistema !";
                                            }

                                            #region MOSTRAR MENSAJE DE USUARIO
                                            //Mostramos el mensaje porque se produjo un error con la Trx.
                                            this.RadWindowManager1.ReloadOnShow = true;
                                            this.RadWindowManager1.DestroyOnClose = true;
                                            this.RadWindowManager1.Windows.Clear();
                                            this.RadWindowManager1.Enabled = true;
                                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                                            this.RadWindowManager1.Visible = true;

                                            Ventana = new RadWindow();
                                            Ventana.Modal = true;
                                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgResult;
                                            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                                            return;
                                        }
                                    }

                                    //Aqui Validamos si el usuario puede ingresar al sistema desde afuera de la oficina.
                                    string IPCliente = ObjUtils.GetIPAddress().ToString().Trim();
                                    if (TheDataReaderPostgreSQL["manejar_fuera_oficina"].ToString().Trim().Equals("S"))
                                    {
                                        if (!TheDataReaderPostgreSQL["ip_equipo_oficina"].ToString().Trim().Equals(IPCliente.ToString().Trim()))
                                        {
                                            #region MOSTRAR MENSAJE DE USUARIO
                                            //Mostramos el mensaje porque se produjo un error con la Trx.
                                            this.RadWindowManager1.ReloadOnShow = true;
                                            this.RadWindowManager1.DestroyOnClose = true;
                                            this.RadWindowManager1.Windows.Clear();
                                            this.RadWindowManager1.Enabled = true;
                                            this.RadWindowManager1.EnableAjaxSkinRendering = true;
                                            this.RadWindowManager1.Visible = true;

                                            Ventana = new RadWindow();
                                            Ventana.Modal = true;
                                            string _Mensaje = "Señor Usuario: La IP [" + IPCliente + "] no se encuentra registrada. Póngase en contacto con el Administrador ó el Proveedor del sistema !";
                                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
                                            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                                            return;
                                        }
                                    }
                                    #endregion

                                    #region DEFINICIONN DE VARIABLES DE SESION

                                    #region DATOS DEL USUARIO
                                    //Datos del usuario
                                    this.Session["IdUsuario"] = TheDataReaderPostgreSQL["id_usuario"].ToString().Trim().ToUpper();
                                    this.Session["NombreCompletoUsuario"] = TheDataReaderPostgreSQL["nombres"].ToString().Trim().ToUpper() + " " + TheDataReaderPostgreSQL["apellidos"].ToString().Trim().ToUpper();
                                    this.Session["NombreUsuario"] = TheDataReaderPostgreSQL["nombres"].ToString().Trim().ToUpper();
                                    this.Session["ApellidoUsuario"] = TheDataReaderPostgreSQL["apellidos"].ToString().Trim().ToUpper();
                                    this.Session["Identificacion"] = TheDataReaderPostgreSQL["identificacion_usuario"].ToString().Trim().ToUpper();
                                    this.Session["LoginUsuario"] = TheDataReaderPostgreSQL["login"].ToString().Trim().ToUpper();
                                    this.Session["IdRol"] = TheDataReaderPostgreSQL["id_rol"].ToString().Trim().ToUpper();
                                    this.Session["NombreRol"] = TheDataReaderPostgreSQL["nombre_rol"].ToString().Trim().ToUpper();
                                    this.Session["CambiarClave"] = TheDataReaderPostgreSQL["cambiar_clave"].ToString().Trim().ToUpper();
                                    this.Session["DireccionUser"] = TheDataReaderPostgreSQL["direccion_user"].ToString().Trim().ToUpper();
                                    this.Session["TelefonoUser"] = TheDataReaderPostgreSQL["telefono_usuario"].ToString().Trim().ToUpper();
                                    this.Session["IpPcUser"] = TheDataReaderPostgreSQL["ip_equipo_oficina"].ToString().Trim().ToUpper();
                                    this.Session["EmailUsuario"] = TheDataReaderPostgreSQL["email_user"].ToString().Trim().ToUpper();
                                    this.Session["ManejaFueraOficina"] = TheDataReaderPostgreSQL["manejar_fuera_oficina"].ToString().Trim().ToUpper();
                                    this.Session["IdEstadoUsuario"] = TheDataReaderPostgreSQL["id_estado"].ToString().Trim();
                                    this.Session["IdMedioEnvio"] = TheDataReaderPostgreSQL["idmedio_envio_token"].ToString().Trim();
                                    #endregion

                                    #region DATOS DEL CLIENTE
                                    //Datos de sessiones para el usuario del comercio.
                                    if (TheDataReaderPostgreSQL["idcliente_padre"].ToString().Trim().Length > 0)
                                    {
                                        this.Session["IdCliente"] = TheDataReaderPostgreSQL["idcliente_padre"].ToString().Trim();
                                        this.Session["NombreCliente"] = TheDataReaderPostgreSQL["razon_social"].ToString().Trim();
                                        this.Session["DireccionCliente"] = TheDataReaderPostgreSQL["direccion_cliente"].ToString().Trim();
                                        this.Session["EmailContacto"] = TheDataReaderPostgreSQL["email_contacto"].ToString().Trim();
                                        this.Session["IdCiudadCliente"] = TheDataReaderPostgreSQL["idciudad_cliente"].ToString().Trim();
                                        this.Session["IdTipoSector"] = TheDataReaderPostgreSQL["idtipo_sector"].ToString().Trim();
                                        //--DATOS DEL FIRMANTE
                                        this.Session["IdFirmante"] = TheDataReaderPostgreSQL["id_firmante"].ToString().Trim().Length > 0 ? TheDataReaderPostgreSQL["id_firmante"].ToString().Trim() : null;
                                        this.Session["IdTipoIdentificacion"] = TheDataReaderPostgreSQL["idtipo_identificacion"].ToString().Trim();
                                        this.Session["NumeroDocumento"] = TheDataReaderPostgreSQL["numero_documento"].ToString().Trim();
                                        this.Session["NombreFirmante"] = TheDataReaderPostgreSQL["nombre_firmante"].ToString().Trim();
                                        this.Session["ImagenFirma"] = TheDataReaderPostgreSQL["imagen_firma"] != null ? TheDataReaderPostgreSQL["imagen_firma"] : null;
                                    }
                                    else
                                    {
                                        this.Session["IdCliente"] = null;
                                        this.Session["NombreCliente"] = null;
                                        this.Session["DireccionCliente"] = null;
                                        this.Session["EmailContacto"] = null;
                                        this.Session["IdCiudadCliente"] = null;
                                        this.Session["IdTipoSector"] = null;
                                        //--DATOS DEL FIRMANTE
                                        this.Session["IdFirmante"] = null;
                                        this.Session["IdTipoIdentificacion"] = null;
                                        this.Session["NumeroDocumento"] = null;
                                        this.Session["NombreFirmante"] = null;
                                        this.Session["ImagenFirma"] = null;
                                    }
                                    #endregion

                                    #region DATOS DE LA IMAGEN DE LA EMPRESA Y USUARIO
                                    //Imagen fotografia del usuario
                                    if (TheDataReaderPostgreSQL["imagen_usuario"].ToString().Trim().Length > 0)
                                    {
                                        this.Session["ImagenUsuario"] = (Byte[])TheDataReaderPostgreSQL["imagen_usuario"];
                                    }
                                    else
                                    {
                                        this.Session["ImagenUsuario"] = null;
                                    }

                                    //Imagen Logo de la empresa
                                    if (TheDataReaderPostgreSQL["imagen_empresa"].ToString().Trim().Length > 0)
                                    {
                                        this.Session["ImagenEmpresa"] = (Byte[])TheDataReaderPostgreSQL["imagen_empresa"];
                                    }
                                    else
                                    {
                                        this.Session["ImagenEmpresa"] = null;
                                    }
                                    this.Session["DtCacheMenu"] = "Menu" + Convert.ToString(this.Session["IdRol"]).ToString().Trim();
                                    #endregion

                                    #region DATOS DE LA EMPRESA
                                    //Datos de la Empresa Logeada
                                    this.Session["Nivel"] = 1;
                                    //this.Session["EstadoEmpresa"] = TheDataReaderPostgreSQL["EstadoEmpresa"];
                                    this.Session["IdEmpresa"] = TheDataReaderPostgreSQL["id_empresa"];
                                    this.Session["IdPais"] = TheDataReaderPostgreSQL["id_pais"].ToString().Trim();
                                    this.Session["NombreEmpresa"] = TheDataReaderPostgreSQL["nombre_empresa"].ToString().Trim().ToUpper();
                                    this.Session["NitEmpresa"] = TheDataReaderPostgreSQL["nit_empresa"].ToString().Trim().ToUpper();
                                    this.Session["EmblemaEmpresa"] = TheDataReaderPostgreSQL["emblema_empresa"].ToString().Trim().ToUpper();
                                    this.Session["DireccionEmpresa"] = TheDataReaderPostgreSQL["direccion_empresa"].ToString().Trim().ToUpper();
                                    this.Session["TelefonoEmpresa"] = TheDataReaderPostgreSQL["telefono_empresa"].ToString().Trim().ToUpper();
                                    this.Session["EmailEmpresa"] = TheDataReaderPostgreSQL["email_empresa"].ToString().Trim();
                                    this.Session["CanEmpresaHijasRegistrar"] = TheDataReaderPostgreSQL["CanEmpresaHijasRegistrar"].ToString().Trim();
                                    this.Session["TipoEmpresa"] = TheDataReaderPostgreSQL["tipo_empresa"].ToString().Trim();
                                    this.Session["EmpresaUnica"] = TheDataReaderPostgreSQL["empresa_unica"].ToString().Trim();
                                    this.Session["cEmailPara"] = "sixtocf@hotmail.com";

                                    //Datos de la Empresa Administradora o Padre
                                    //this.Session["EstadoEmpresaAdmin"] = TheDataReaderPostgreSQL["EstadoEmpresaAdmin"];
                                    this.Session["IdEmpresaAdmon"] = TheDataReaderPostgreSQL["IdEmpresaAdmon"];
                                    this.Session["NombreEmpresaAdmon"] = TheDataReaderPostgreSQL["NombreEmpresaAdmon"].ToString().Trim().ToUpper();
                                    this.Session["NitEmpresaAdmon"] = TheDataReaderPostgreSQL["NitEmpresaAdmon"].ToString().Trim().ToUpper();
                                    this.Session["EmblemaEmpresaAdmon"] = TheDataReaderPostgreSQL["EmblemaEmpresaAdmon"].ToString().Trim().ToUpper();
                                    this.Session["DireccionEmpresaAdmon"] = TheDataReaderPostgreSQL["DireccionEmpresaAdmon"].ToString().Trim().ToUpper();
                                    this.Session["TelefonoEmpresaAdmon"] = TheDataReaderPostgreSQL["TelefonoEmpresaAdmon"].ToString().Trim().ToUpper();
                                    this.Session["EmailEnviaEmpresaAdmon"] = TheDataReaderPostgreSQL["EmailEnviaEmpresaAdmon"].ToString().Trim();
                                    this.Session["CanEmpresaAdmonRegistrar"] = TheDataReaderPostgreSQL["CanEmpresaAdmonRegistrar"].ToString().Trim();
                                    #endregion
                                    #endregion

                                    #region AQUI REALIZAMOS EL PROCESO DE REINICIO CONTADOR DE CALVE INVALIDA DEL USUARIO
                                    Usuario objUser = new Usuario();
                                    objUser.LoginUsuario = this.TxtUsuario.Text.ToString().Trim().ToUpper();
                                    objUser.PasswordUsuario = _Hash.ToString().Trim();
                                    objUser.ContadorClaveInvalida = Int32.Parse(this.Session["IntentosClaveInvalida"].ToString().Trim());
                                    objUser.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();
                                    objUser.TipoProceso = 2;

                                    string _MsgError = "";
                                    if (objUser.GetBloqueoUsuario(ref _MsgError))
                                    {
                                        _log.Info(_MsgError);
                                    }
                                    else
                                    {
                                        _log.Error(_MsgError);
                                    }
                                    #endregion

                                    #region DEFINICION DE LOGS DE AUDITORIA
                                    //--AQUI SE SETEAN LOS DATOS PARA LA AUDITORIA
                                    ObjAuditoria.IdEmpresa = Convert.ToInt32(this.Session["IdEmpresa"].ToString().Trim());
                                    ObjAuditoria.IdUsuario = Convert.ToInt32(this.Session["IdUsuario"].ToString().Trim());
                                    ObjAuditoria.IdTipoEvento = 1;  //--INSERT
                                    ObjAuditoria.ModuloApp = "LOGIN";
                                    ObjAuditoria.UrlVisitada = Request.ServerVariables["PATH_INFO"].ToString().Trim();
                                    ObjAuditoria.DescripcionEvento = "INICIO DE SESION";
                                    ObjAuditoria.IPCliente = ObjUtils.GetIPAddress().ToString().Trim();
                                    ObjAuditoria.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();
                                    ObjAuditoria.TipoProceso = 1;

                                    string _MsgErrorLogs = "";
                                    //'Agregar Auditoria del sistema
                                    if (!ObjAuditoria.AddAuditoria(ref _MsgErrorLogs))
                                    {
                                        _log.Error(_MsgErrorLogs);
                                    }

                                    if (TheDataReaderPostgreSQL["cambiar_clave"].ToString().Trim().Equals("S"))
                                    {
                                        this.Session["Login"] = 2;
                                        Response.Redirect("/FrmCambiarPassword.aspx");
                                    }
                                    else
                                    {
                                        //Aqui validamos si la fecha para cambio de clave ya expiro.
                                        if (DateTime.Now > dtFechaExpiraClave)
                                        {
                                            this.Session["Login"] = 2;
                                            this.Session["CambiarClave"] = "S";
                                            Response.Redirect("/FrmCambiarPassword.aspx");
                                        }
                                        else
                                        {
                                            //Aqui validamos que el usuario no tenga para valide Token.
                                            if (this.Session["IdMedioEnvio"].Equals("3"))
                                            {
                                                this.Session["Login"] = 2;
                                                Response.Redirect("/Index.aspx");
                                            }
                                            else
                                            {
                                                this.Session["Login"] = 1;
                                                Response.Redirect("/FrmValidarToken.aspx");
                                            }
                                        }
                                    }
                                    #endregion

                                    TheDataReaderPostgreSQL.Close();
                                    return;
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

                                    Ventana = new RadWindow();
                                    Ventana.Modal = true;
                                    string _Mensaje = "El Usuario con el que Intenta ingresar al sistema no se encuentra ACTIVO para ingresar al sistema. Por favor póngase en contacto con el Administrador del sistema !";
                                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
                                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                                    return;
                                    #endregion
                                }
                            }
                            else
                            {
                                //--VALIDAR EL CODIGO RPTA DEL LOGIN
                                if (_OutRespuesta.Equals("02"))
                                {
                                    #region AQUI REALIZAMOS EL PROCESO DE BLOQUEO DEL USUARIO
                                    Usuario objUser = new Usuario();
                                    objUser.LoginUsuario = this.TxtUsuario.Text.ToString().Trim().ToUpper();
                                    objUser.PasswordUsuario = _Hash.ToString().Trim();
                                    objUser.ContadorClaveInvalida = Int32.Parse(this.Session["IntentosClaveInvalida"].ToString().Trim());
                                    objUser.MotorBaseDatos = FixedData.BaseDatosUtilizar.ToString().Trim();
                                    objUser.TipoProceso = 1;

                                    string _MsgError = "";
                                    if (objUser.GetBloqueoUsuario(ref _MsgError))
                                    {
                                        #region MOSTRAR MENSAJE DE USUARIO
                                        //Mostramos el mensaje porque se produjo un error con la Trx.
                                        this.RadWindowManager1.ReloadOnShow = true;
                                        this.RadWindowManager1.DestroyOnClose = true;
                                        this.RadWindowManager1.Windows.Clear();
                                        this.RadWindowManager1.Enabled = true;
                                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                                        this.RadWindowManager1.Visible = true;

                                        Ventana = new RadWindow();
                                        Ventana.Modal = true;
                                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                                        return;
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

                                        Ventana = new RadWindow();
                                        Ventana.Modal = true;
                                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                                        return;
                                        #endregion
                                    }
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

                                    Ventana = new RadWindow();
                                    Ventana.Modal = true;
                                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _OutMensaje;
                                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                                    return;
                                    #endregion
                                }
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

                            Ventana = new RadWindow();
                            Ventana.Modal = true;
                            string _Mensaje = "Usuario o Password son incorrectos. Intente nuevamente por favor. !";
                            Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
                            Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                            return;
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

                        Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _Mensaje = "Usuario o Password son incorrectos. Intente nuevamente por favor. !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
                        Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                        return;
                    }
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Base de datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //Base de datos Oracle
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

                    Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string _Mensaje = "Señor usuario. No existe configurado un Motor de Base de Datos a Trabajar !";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
                    Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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
                    return;
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

                Ventana = new RadWindow();
                Ventana.Modal = true;
                string _Mensaje = "Señor usuario. ocurrio un error al Iniciar sesión. Motivo: " + ex.Message;
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _Mensaje;
                Ventana.ID = "RadWindow" + ObjUtils.GetRandom();
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