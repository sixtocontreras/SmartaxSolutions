using System;
using System.Data;
using System.Text;
using System.Configuration;
using log4net;
using Devart.Data.PostgreSql;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Web;

namespace Smartax.Web.Application.Clases.Seguridad
{
    public class Usuario
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        Utilidades ObjUtils = new Utilidades();

        #region DEFINICION DE OBJETOS DE BASE DE DATOS
        IDbConnection myConnectionDb = null;
        string connString = "";

        MySqlCommand TheCommandMySQL = null;
        MySqlDataReader TheDataReaderMySQL = null;

        PgSqlCommand TheCommandPostgreSQL = null;
        PgSqlDataReader TheDataReaderPostgreSQL = null;
        PgSqlDataAdapter TheDataAdapterPostgreSQL;
        PgSqlParameter NpParam = null;
        IDbTransaction Transac = null;

        SqlCommand TheCommandSQLServer = null;
        SqlDataReader TheDataReaderSQLServer = null;

        OracleCommand TheCommandOracle = null;
        OracleDataReader TheDataReaderOracle = null;
        #endregion

        #region DEFINICION DE ATRIBUTOS Y PROPIEDADES
        public object IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string IdentificacionUsuario { get; set; }
        public string LoginUsuario { get; set; }
        public string PasswordUsuario { get; set; }
        public string DireccionUsuario { get; set; }
        public string TelefonoUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string CambiarClave { get; set; }
        public string ManejarFueraOficina { get; set; }
        public string IpEquipoOficina { get; set; }
        public int IdRol { get; set; }
        public int ContadorClaveInvalida { get; set; }
        public object IdCliente { get; set; }
        public string FechaExpClave { get; set; }
        public object IdEstado { get; set; }
        public int IdEmpresa { get; set; }
        public int IdEmpresaPadre { get; set; }
        public int IdUsuarioAdd { get; set; }
        public int IdUsuarioUp { get; set; }
        public object IdImagenUsuario { get; set; }
        public byte[] ImagenUser { get; set; }
        public string MostrarSeleccione { get; set; }
        public string MotorBaseDatos { get; set; }
        public int TipoConsulta { get; set; }
        public int TipoProceso { get; set; }
        #endregion

        #region DEFINICION DE METODOS PARA OBTENER LOS DATOS DE LA BB
        public DataTable GetAllUsuarios()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtUsuarios";
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return null;
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }
                #endregion

                //Aqui hacemos los llamados de los sp o consultas a utilizar en la respectiva base de datos
                if (myConnectionDb is PgSqlConnection)
                {
                    #region OBTENER DATOS DE LA DB POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_usuarios", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DATATABLE
                    TablaDatos.Columns.Add("id_usuario", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_completo_usuario");
                    TablaDatos.Columns.Add("nombre_usuario");
                    TablaDatos.Columns.Add("apellido_usuario");
                    TablaDatos.Columns.Add("identificacion_usuario");
                    TablaDatos.Columns.Add("login_usuario");
                    TablaDatos.Columns.Add("direccion_usuario");
                    TablaDatos.Columns.Add("telefono_usuario");
                    TablaDatos.Columns.Add("email_usuario");
                    TablaDatos.Columns.Add("cambiar_clave");
                    TablaDatos.Columns.Add("manejar_fuera_oficina");
                    TablaDatos.Columns.Add("ip_equipo_oficina");
                    TablaDatos.Columns.Add("idmedio_envio_token", typeof(Int32));
                    TablaDatos.Columns.Add("medio_envio");
                    TablaDatos.Columns.Add("id_rol", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_rol");
                    TablaDatos.Columns.Add("id_empresa", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_empresa");
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("password_nuevo");
                    TablaDatos.Columns.Add("fecha_registro");
                    TablaDatos.Columns.Add("fecha_ult_ingreso");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_usuario"] = Int32.Parse(TheDataReaderPostgreSQL["id_usuario"].ToString().Trim());
                            Fila["nombre_completo_usuario"] = TheDataReaderPostgreSQL["nombre_usuario"].ToString().Trim() + " " + TheDataReaderPostgreSQL["apellido_usuario"].ToString().Trim(); ;
                            Fila["nombre_usuario"] = TheDataReaderPostgreSQL["nombre_usuario"].ToString().Trim();
                            Fila["apellido_usuario"] = TheDataReaderPostgreSQL["apellido_usuario"].ToString().Trim();
                            Fila["identificacion_usuario"] = TheDataReaderPostgreSQL["identificacion_usuario"].ToString().Trim();
                            Fila["login_usuario"] = TheDataReaderPostgreSQL["login_usuario"].ToString().Trim();
                            Fila["direccion_usuario"] = TheDataReaderPostgreSQL["direccion_usuario"].ToString().Trim();
                            Fila["telefono_usuario"] = TheDataReaderPostgreSQL["telefono_usuario"].ToString().Trim();
                            Fila["email_usuario"] = TheDataReaderPostgreSQL["email_usuario"].ToString().Trim();

                            if (TheDataReaderPostgreSQL["cambiar_clave"].ToString().Trim().Equals("S"))
                            {
                                Fila["cambiar_clave"] = true;
                            }
                            else
                            {
                                Fila["cambiar_clave"] = false;
                            }

                            if (TheDataReaderPostgreSQL["manejar_fuera_oficina"].ToString().Trim().Equals("S"))
                            {
                                Fila["manejar_fuera_oficina"] = true;
                            }
                            else
                            {
                                Fila["manejar_fuera_oficina"] = false;
                            }

                            Fila["ip_equipo_oficina"] = TheDataReaderPostgreSQL["ip_equipo_oficina"].ToString().Trim();
                            Fila["idmedio_envio_token"] = Int32.Parse(TheDataReaderPostgreSQL["idmedio_envio_token"].ToString().Trim());
                            Fila["medio_envio"] = TheDataReaderPostgreSQL["medio_envio"].ToString().Trim();
                            Fila["id_rol"] = Int32.Parse(TheDataReaderPostgreSQL["id_rol"].ToString().Trim());
                            Fila["nombre_rol"] = TheDataReaderPostgreSQL["nombre_rol"].ToString().Trim();
                            Fila["id_empresa"] = Int32.Parse(TheDataReaderPostgreSQL["id_empresa"].ToString().Trim());
                            Fila["nombre_empresa"] = TheDataReaderPostgreSQL["nombre_empresa"].ToString().Trim();
                            Fila["id_estado"] = Int32.Parse(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["password_nuevo"] = "";
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            Fila["fecha_ult_ingreso"] = TheDataReaderPostgreSQL["fecha_ult_ingreso"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    #endregion
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //Para Base de Datos Oracle
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al obtener los datos de la Tabla [tbl_usuario]. Motivo: " + ex.Message);
            }
            finally
            {
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    TheDataReaderPostgreSQL.Close();
                    TheDataReaderPostgreSQL = null;
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    TheCommandMySQL = null;
                    TheDataReaderMySQL.Close();
                    TheDataReaderMySQL = null;
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                    TheDataReaderSQLServer.Close();
                    TheDataReaderSQLServer = null;
                }
                else if (myConnectionDb is OracleConnection)
                {
                    TheCommandOracle = null;
                    TheDataReaderOracle.Close();
                    TheDataReaderOracle = null;
                }
                myConnectionDb.Close();
                myConnectionDb.Dispose();
            }

            return TablaDatos;
        }

        public DataTable GetUsuarios()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtUsuarios";
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return null;
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }
                #endregion

                //Aqui hacemos los llamados de los sp o consultas a utilizar en la respectiva base de datos
                if (myConnectionDb is PgSqlConnection)
                {
                    #region OBTENER DATOS DE LA DB POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_usuarios", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("id_usuario");
                    TablaDatos.Columns.Add("nombre_usuario");

                    if (TheDataReaderPostgreSQL != null)
                    {
                        if (MostrarSeleccione.ToString().Trim().Equals("SI"))
                        {
                            TablaDatos.Rows.Add("", "<< Seleccione >>");
                        }
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_usuario"] = TheDataReaderPostgreSQL["id_usuario"].ToString().Trim();
                            Fila["nombre_usuario"] = TheDataReaderPostgreSQL["nombre_usuario"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                        }
                    }
                    #endregion
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //Para Base de Datos Oracle
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al obtener los datos de la Tabla [tbl_usuario]. Motivo: " + ex.Message);
            }
            finally
            {
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    TheDataReaderPostgreSQL.Close();
                    TheDataReaderPostgreSQL = null;
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    TheCommandMySQL = null;
                    TheDataReaderMySQL.Close();
                    TheDataReaderMySQL = null;
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                    TheDataReaderSQLServer.Close();
                    TheDataReaderSQLServer = null;
                }
                else if (myConnectionDb is OracleConnection)
                {
                    TheCommandOracle = null;
                    TheDataReaderOracle.Close();
                    TheDataReaderOracle = null;
                }
                myConnectionDb.Close();
                myConnectionDb.Dispose();
            }

            return TablaDatos;
        }

        public DataTable GetInfoUsuario()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtUsuario";
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return null;
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }
                #endregion

                //Aqui hacemos los llamados de los sp o consultas a utilizar en la respectiva base de datos
                if (myConnectionDb is PgSqlConnection)
                {
                    #region OBTENER DATOS DE LA DB DE POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_info_usuario", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_id_usuario", IdUsuario);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINIR COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("id_usuario", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_usuario");
                    TablaDatos.Columns.Add("apellido_usuario");
                    TablaDatos.Columns.Add("identificacion_usuario");
                    TablaDatos.Columns.Add("login_usuario");
                    TablaDatos.Columns.Add("direccion_usuario");
                    TablaDatos.Columns.Add("telefono_usuario");
                    TablaDatos.Columns.Add("email_usuario");
                    TablaDatos.Columns.Add("cambiar_clave");
                    TablaDatos.Columns.Add("manejar_fuera_oficina");
                    TablaDatos.Columns.Add("ip_equipo_oficina");
                    TablaDatos.Columns.Add("id_rol", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_rol");
                    TablaDatos.Columns.Add("id_empresa", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_empresa");
                    //TablaDatos.Columns.Add("id_tipo_usuario", typeof(Int32));
                    //TablaDatos.Columns.Add("tipo_usuario");
                    //TablaDatos.Columns.Add("id_comercio");
                    //TablaDatos.Columns.Add("razon_social_comercio");
                    //TablaDatos.Columns.Add("direccion_comercio");
                    //TablaDatos.Columns.Add("telefono_fijo_comercio");
                    //TablaDatos.Columns.Add("email_comercio");
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_estado");
                    //TablaDatos.Columns.Add("imagen_usuario", typeof(Byte[]));
                    TablaDatos.Columns.Add("observacion_usuario");
                    TablaDatos.Columns.Add("fecha_registro");
                    TablaDatos.Columns.Add("fecha_ult_ingreso");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DEL DATA READER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_usuario"] = Int32.Parse(TheDataReaderPostgreSQL["id_usuario"].ToString().Trim());
                            Fila["nombre_usuario"] = TheDataReaderPostgreSQL["nombre_usuario"].ToString().Trim();
                            Fila["apellido_usuario"] = TheDataReaderPostgreSQL["apellido_usuario"].ToString().Trim();
                            Fila["identificacion_usuario"] = TheDataReaderPostgreSQL["identificacion_usuario"].ToString().Trim();
                            Fila["login_usuario"] = TheDataReaderPostgreSQL["login_usuario"].ToString().Trim();
                            Fila["direccion_usuario"] = TheDataReaderPostgreSQL["direccion_usuario"].ToString().Trim();
                            Fila["telefono_usuario"] = TheDataReaderPostgreSQL["telefono_usuario"].ToString().Trim();
                            Fila["email_usuario"] = TheDataReaderPostgreSQL["email_usuario"].ToString().Trim();

                            if (TheDataReaderPostgreSQL["cambiar_clave"].ToString().Trim().Equals("S"))
                            {
                                Fila["cambiar_clave"] = "SI";
                            }
                            else
                            {
                                Fila["cambiar_clave"] = "NO";
                            }

                            if (TheDataReaderPostgreSQL["manejar_fuera_oficina"].ToString().Trim().Equals("S"))
                            {
                                Fila["manejar_fuera_oficina"] = "SI";
                            }
                            else
                            {
                                Fila["manejar_fuera_oficina"] = "NO";
                            }

                            Fila["ip_equipo_oficina"] = TheDataReaderPostgreSQL["ip_equipo_oficina"].ToString().Trim();
                            Fila["id_rol"] = Int32.Parse(TheDataReaderPostgreSQL["id_rol"].ToString().Trim());
                            Fila["nombre_rol"] = TheDataReaderPostgreSQL["nombre_rol"].ToString().Trim();
                            Fila["id_empresa"] = Int32.Parse(TheDataReaderPostgreSQL["id_empresa"].ToString().Trim());
                            Fila["nombre_empresa"] = TheDataReaderPostgreSQL["nombre_empresa"].ToString().Trim();
                            //Fila["id_tipo_usuario"] = Int32.Parse(TheDataReaderPostgreSQL["id_tipo_usuario"].ToString().Trim());
                            //Fila["tipo_usuario"] = TheDataReaderPostgreSQL["tipo_usuario"].ToString().Trim();

                            //Datos del comercio al que pertenece el usuario
                            //Fila["id_comercio"] = TheDataReaderPostgreSQL["id_comercio"].ToString().Trim();
                            //Fila["razon_social_comercio"] = TheDataReaderPostgreSQL["razon_social_comercio"].ToString().Trim();
                            //Fila["direccion_comercio"] = TheDataReaderPostgreSQL["direccion_comercio"].ToString().Trim();
                            //Fila["telefono_fijo_comercio"] = TheDataReaderPostgreSQL["telefono_fijo_comercio"].ToString().Trim();
                            //Fila["email_comercio"] = TheDataReaderPostgreSQL["email_comercio"].ToString().Trim();

                            Fila["id_estado"] = Int32.Parse(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();

                            //if (TheDataReaderPostgreSQL["imagen_usuario"].ToString().Trim().Length > 0)
                            //{
                            //    Fila["imagen_usuario"] = (Byte[])TheDataReaderPostgreSQL["imagen_usuario"];
                            //}
                            //else
                            //{
                            //    Utilidades ObjUtils = new Utilidades();
                            //    Byte[] ImagenByte = null;
                            //    string strImgDefault = "Imagenes/Default/img_user.png";
                            //    string cRutaImagen = HttpContext.Current.Server.MapPath("/" + strImgDefault.ToString().Trim());

                            //    ImagenByte = ObjUtils.GetImagenBytes(cRutaImagen);

                            //    Fila["imagen_usuario"] = ImagenByte;
                            //}
                            Fila["observacion_usuario"] = TheDataReaderPostgreSQL["observacion_usuario"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            Fila["fecha_ult_ingreso"] = TheDataReaderPostgreSQL["fecha_ult_ingreso"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //Para Base de Datos Oracle
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al obtener los datos de la Tabla [tbl_usuario]. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION A LA DB
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    TheDataReaderPostgreSQL.Close();
                    TheDataReaderPostgreSQL = null;
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    TheCommandMySQL = null;
                    TheDataReaderMySQL.Close();
                    TheDataReaderMySQL = null;
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                    TheDataReaderSQLServer.Close();
                    TheDataReaderSQLServer = null;
                }
                else if (myConnectionDb is OracleConnection)
                {
                    TheCommandOracle = null;
                    TheDataReaderOracle.Close();
                    TheDataReaderOracle = null;
                }
                myConnectionDb.Close();
                myConnectionDb.Dispose();
                #endregion
            }

            return TablaDatos;
        }
        #endregion

        #region DEFINICION DE METODOS PARA REGISTRAR Y ACTUALIZAR INFORMACION EN LA BD
        //Metodo para registrar datos del usuario desde la Interfaz del Popup de Telerik.
        public bool AddUpUsuario(DataRow Fila, ref int _IdRegistro, ref string _MsgError)
        {
            bool retValor = false;
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return retValor;
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }
                #endregion

                //Aqui hacemos los llamados de los sp o consultas a utilizar en la respectiva base de datos
                if (myConnectionDb is PgSqlConnection)
                {
                    #region REGISTRO DE USUARIO DB POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_usuario", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    #region DEFINICION DE PARAMETROS DE ENTRADA AL SP
                    //--VALIDAMOS EL TIPO DE PROCESO
                    string _LoginUser = "";
                    if (TipoProceso == 2)
                    {
                        if (Int32.Parse(IdUsuario.ToString().Trim()) == 1)
                        {
                            _LoginUser = Fila["login_usuario"].ToString().Trim();
                        }
                        else
                        {
                            _LoginUser = Fila["identificacion_usuario"].ToString().Trim();
                        }
                    }
                    else
                    {
                        _LoginUser = Fila["identificacion_usuario"].ToString().Trim();
                    }
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_nombre_usuario", Fila["nombre_usuario"].ToString().Trim().ToUpper());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_apellido_usuario", Fila["apellido_usuario"].ToString().Trim().ToUpper());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_identificacion_usuario", Fila["identificacion_usuario"].ToString().Trim().ToUpper());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_login_usuario", _LoginUser.ToString().Trim().ToUpper());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_password_usuario", ObjUtils.GetHashPassword(_LoginUser.ToString().Trim(), PasswordUsuario.ToString().Trim()));
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_direccion_usuario", Fila["direccion_usuario"].ToString().Trim().ToUpper());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_telefono_usuario", Fila["telefono_usuario"].ToString().Trim().ToUpper());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_email_usuario", Fila["email_usuario"].ToString().Trim().ToUpper());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_cambiar_clave", CambiarClave);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_fecha_exp_clave", FechaExpClave);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_manejar_fuera_oficina", Convert.ToBoolean(Fila["manejar_fuera_oficina"]) == true ? "S" : "N");
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_ip_equipo_oficina", Fila["ip_equipo_oficina"].ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmedio_envio_token", Fila["idmedio_envio_token"].ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", Fila["id_rol"].ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente_padre", IdCliente);

                    //--Para validar el id estado en el Bloqueo o Desbloqueo del usuario
                    if (TipoProceso == 4)
                    {
                        TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", Fila["id_estado"].ToString().Trim().Equals("1") ? 0 : 1);
                    }
                    else
                    {
                        TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", Fila["id_estado"].ToString().Trim());
                    }

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa", Fila["id_empresa"]);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa_padre", IdEmpresaPadre);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario_add", IdUsuarioAdd);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario_up", IdUsuarioUp);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_proceso", TipoProceso);
                    PgSqlParameter _IdRegRetorno = new PgSqlParameter("@p_out_id_registro", SqlDbType.Int);
                    PgSqlParameter _CodRptaRetorno = new PgSqlParameter("@p_out_cod_rpta", SqlDbType.VarChar);
                    PgSqlParameter _MsgRptaRetorno = new PgSqlParameter("@p_out_msg_rpta", SqlDbType.VarChar);

                    //asignamos los parametros de retornos.
                    _IdRegRetorno.Direction = ParameterDirection.Output;
                    _CodRptaRetorno.Direction = ParameterDirection.Output;
                    _MsgRptaRetorno.Direction = ParameterDirection.Output;
                    TheCommandPostgreSQL.Parameters.Add(_IdRegRetorno);
                    TheCommandPostgreSQL.Parameters.Add(_CodRptaRetorno);
                    TheCommandPostgreSQL.Parameters.Add(_MsgRptaRetorno);
                    #endregion

                    object ObjResult = new object();
                    ObjResult = TheCommandPostgreSQL.ExecuteScalar();
                    if (ObjResult != null)
                    {
                        if (Int32.Parse(ObjResult.ToString().Trim()) > 0)
                        {
                            Transac.Commit();
                            _IdRegistro = Int32.Parse(_IdRegRetorno.Value.ToString());
                            _MsgError = _CodRptaRetorno.Value.ToString() + "|" + _MsgRptaRetorno.Value.ToString();
                            retValor = true;
                        }
                        else
                        {
                            _MsgError = _MsgRptaRetorno.Value.ToString();
                            retValor = false;
                        }
                    }
                    else
                    {
                        Transac.Rollback();
                        Transac.Connection.Close();
                        retValor = false;
                    }
                    #endregion
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //--BASE DE DATOS MYSQL
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //--BASE DE DATOS ORACLE
                }
                else
                {
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
                    return false;
                }
            }
            catch (Exception ex)
            {
                retValor = false;
                _MsgError = "Error con el proceso del usuario. Motivo: " + ex.Message.ToString().Trim();
                _log.Error(_MsgError.ToString().Trim());
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION A LA DB
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    TheCommandMySQL = null;
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                }
                else if (myConnectionDb is OracleConnection)
                {
                    TheCommandOracle = null;
                }

                myConnectionDb.Close();
                myConnectionDb.Dispose();
                #endregion
            }

            return retValor;
        }

        //Metodo para registrar datos del usuario desde la Interfaz del Popup de Telerik.
        public bool SetProcesoUsuario(ref int _IdRegistro, ref string _MsgError)
        {
            bool retValor = false;
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return retValor;
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }
                #endregion

                //Aqui hacemos los llamados de los sp o consultas a utilizar en la respectiva base de datos
                if (myConnectionDb is PgSqlConnection)
                {
                    #region REGISTRO DE USUARIO DB POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_proceso_usuario", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    #region DEFINICION DE PARAMETROS DE ENTRADA AL SP
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_nombre_usuario", NombreUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_login_usuario", LoginUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_password_usuario", ObjUtils.GetHashPassword(LoginUsuario.ToString().Trim(), PasswordUsuario.ToString().Trim()));
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_email_usuario", EmailUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_cambiar_clave", CambiarClave);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_fecha_exp_clave", FechaExpClave);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario_up", IdUsuarioUp);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_proceso", TipoProceso);
                    PgSqlParameter _IdRegRetorno = new PgSqlParameter("@p_out_id_registro", SqlDbType.Int);
                    PgSqlParameter _CodRptaRetorno = new PgSqlParameter("@p_out_cod_rpta", SqlDbType.VarChar);
                    PgSqlParameter _MsgRptaRetorno = new PgSqlParameter("@p_out_msg_rpta", SqlDbType.VarChar);

                    //asignamos los parametros de retornos.
                    _IdRegRetorno.Direction = ParameterDirection.Output;
                    _CodRptaRetorno.Direction = ParameterDirection.Output;
                    _MsgRptaRetorno.Direction = ParameterDirection.Output;
                    TheCommandPostgreSQL.Parameters.Add(_IdRegRetorno);
                    TheCommandPostgreSQL.Parameters.Add(_CodRptaRetorno);
                    TheCommandPostgreSQL.Parameters.Add(_MsgRptaRetorno);
                    #endregion

                    object ObjResult = new object();
                    ObjResult = TheCommandPostgreSQL.ExecuteScalar();
                    if (ObjResult != null)
                    {
                        if (Int32.Parse(ObjResult.ToString().Trim()) > 0)
                        {
                            Transac.Commit();
                            _IdRegistro = Int32.Parse(_IdRegRetorno.Value.ToString());
                            _MsgError = _MsgRptaRetorno.Value.ToString();
                            retValor = true;
                        }
                        else
                        {
                            _MsgError = _MsgRptaRetorno.Value.ToString();
                            retValor = false;
                        }
                    }
                    else
                    {
                        Transac.Rollback();
                        Transac.Connection.Close();
                        retValor = false;
                    }
                    #endregion
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //--BASE DE DATOS MYSQL
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //--BASE DE DATOS ORACLE
                }
                else
                {
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
                    return false;
                }
            }
            catch (Exception ex)
            {
                retValor = false;
                _MsgError = "Error con el proceso del usuario. Motivo: " + ex.Message.ToString().Trim();
                _log.Error(_MsgError.ToString().Trim());
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION A LA DB
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    TheCommandMySQL = null;
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                }
                else if (myConnectionDb is OracleConnection)
                {
                    TheCommandOracle = null;
                }

                myConnectionDb.Close();
                myConnectionDb.Dispose();
                #endregion
            }

            return retValor;
        }

        //Metodo para realizar procesos con el usuario en el sistema
        public bool GetBloqueoUsuario(ref string _MsgError)
        {
            bool retValor = false;
            Utilidades ObjUtils = new Utilidades();
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return retValor;
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }
                #endregion

                //Aqui hacemos los llamados de los sp o consultas a utilizar en la respectiva base de datos
                if (myConnectionDb is PgSqlConnection)
                {
                    #region DEFINICION DEL SP DE LA DB POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_set_bloqueo_usuario", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_usuario", LoginUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_password", PasswordUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_contador", ContadorClaveInvalida);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_proceso", TipoProceso);
                    PgSqlParameter _IdRegRetorno = new PgSqlParameter("@p_out_id_registro", SqlDbType.Int);
                    PgSqlParameter _CodRptaRetorno = new PgSqlParameter("@p_out_cod_rpta", SqlDbType.VarChar);
                    PgSqlParameter _MsgRptaRetorno = new PgSqlParameter("@p_out_msg_rpta", SqlDbType.VarChar);

                    //asignamos los parametros de retornos.
                    _IdRegRetorno.Direction = ParameterDirection.Output;
                    _CodRptaRetorno.Direction = ParameterDirection.Output;
                    _MsgRptaRetorno.Direction = ParameterDirection.Output;
                    TheCommandPostgreSQL.Parameters.Add(_IdRegRetorno);
                    TheCommandPostgreSQL.Parameters.Add(_CodRptaRetorno);
                    TheCommandPostgreSQL.Parameters.Add(_MsgRptaRetorno);

                    object ObjResult = new object();
                    ObjResult = TheCommandPostgreSQL.ExecuteScalar();
                    if (ObjResult != null)
                    {
                        if (Int32.Parse(ObjResult.ToString().Trim()) > 0)
                        {
                            Transac.Commit();
                            //_IdRegistro = Int32.Parse(_IdRegRetorno.Value.ToString());
                            _MsgError = _MsgRptaRetorno.Value.ToString();
                            retValor = true;
                        }
                        else
                        {
                            _MsgError = _MsgRptaRetorno.Value.ToString();
                            retValor = false;
                        }
                    }
                    else
                    {
                        Transac.Rollback();
                        Transac.Connection.Close();
                        retValor = false;
                    }
                    #endregion                    
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //--BASE DE DATOS MYSQL
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //--BASE DE DATOS ORACLE
                }
                else
                {
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
                    return false;
                }
            }
            catch (Exception ex)
            {
                retValor = false;
                _MsgError = "Error al registrar el proceso del usuario. Motivo: " + ex.Message.ToString().Trim();
                _log.Error(_MsgError.ToString().Trim());
            }
            finally
            {
                #region FINALIZACION DEL OBJETO DE CONEXION DE LA DB
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    TheCommandMySQL = null;
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                }
                else if (myConnectionDb is OracleConnection)
                {
                    TheCommandOracle = null;
                }

                myConnectionDb.Close();
                myConnectionDb.Dispose();
                #endregion
            }

            return retValor;
        }

        //Metodo para cargar la imagen del usuario al sistema.
        public bool AddUpImagenUsuario(ref int _IdRegistro, ref string _MsgError)
        {
            bool retValor = false;
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return retValor;
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }
                #endregion

                //Aqui hacemos los llamados de los sp o consultas a utilizar en la respectiva base de datos
                if (myConnectionDb is PgSqlConnection)
                {
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_imagen_usuario", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario_img", IdImagenUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_imagen_usuario", ImagenUser);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_proceso", TipoProceso);
                    PgSqlParameter _IdRegRetorno = new PgSqlParameter("@p_out_id_registro", SqlDbType.Int);
                    PgSqlParameter _CodRptaRetorno = new PgSqlParameter("@p_out_cod_rpta", SqlDbType.VarChar);
                    PgSqlParameter _MsgRptaRetorno = new PgSqlParameter("@p_out_msg_rpta", SqlDbType.VarChar);

                    //asignamos los parametros de retornos.
                    _IdRegRetorno.Direction = ParameterDirection.Output;
                    _CodRptaRetorno.Direction = ParameterDirection.Output;
                    _MsgRptaRetorno.Direction = ParameterDirection.Output;
                    TheCommandPostgreSQL.Parameters.Add(_IdRegRetorno);
                    TheCommandPostgreSQL.Parameters.Add(_CodRptaRetorno);
                    TheCommandPostgreSQL.Parameters.Add(_MsgRptaRetorno);

                    object ObjResult = new object();
                    ObjResult = TheCommandPostgreSQL.ExecuteScalar();
                    if (ObjResult != null)
                    {
                        if (Int32.Parse(ObjResult.ToString().Trim()) > 0)
                        {
                            Transac.Commit();
                            _IdRegistro = Int32.Parse(_IdRegRetorno.Value.ToString());
                            _MsgError = _MsgRptaRetorno.Value.ToString();
                            retValor = true;
                        }
                        else
                        {
                            _MsgError = _MsgRptaRetorno.Value.ToString();
                            retValor = false;
                        }
                    }
                    else
                    {
                        Transac.Rollback();
                        Transac.Connection.Close();
                        retValor = false;
                    }
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Base de datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_crud_sistema_rol";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (System.Data.SqlClient.SqlConnection)myConnectionDb;
                    //se limpian los parámetros
                    TheCommandSQLServer.Parameters.Clear();

                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idusuario_img", IdImagenUsuario);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_imagen_usuario", ImagenUser);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_tipo_proceso", TipoProceso);

                    //declaramos el parámetro de retorno
                    SqlParameter ValorRetorno = new SqlParameter("@ValorRetorno", SqlDbType.Int);
                    //asignamos el valor de retorno
                    ValorRetorno.Direction = ParameterDirection.Output;
                    TheCommandSQLServer.Parameters.Add(ValorRetorno);
                    //ejecutamos el procedimiento almacenado.
                    TheCommandSQLServer.ExecuteNonQuery();
                    // traemos el valor de retorno
                    int Valor_Retornado = Int32.Parse(ValorRetorno.Value.ToString().Trim());

                    //dependiendo del valor de retorno se asigna la variable success
                    //si el procedimiento retorna un 1 la operación se realizó con éxito
                    //de no ser así se mantiene en false y pr lo tanto falló la operación
                    if (Valor_Retornado >= 1)
                    {
                        if (TipoProceso == 1)
                        {
                            _IdRegistro = Valor_Retornado;
                            _MsgError = "La imagen del usuario [" + NombreUsuario + "] ha sido registrado exitosamente.";
                        }
                        else if (TipoProceso == 2)
                        {
                            _MsgError = "La imagen del usuario [" + NombreUsuario + "] han sido editados exitosamente.";
                        }
                        else if (TipoProceso == 3)
                        {
                            _MsgError = "La imagen del usuario [" + NombreUsuario + "] ah sido eliminado del sistema.";
                        }

                        retValor = true;
                    }
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //Base de datos Oracle
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    retValor = false;
                }
            }
            catch (Exception ex)
            {
                retValor = false;
                _MsgError = "Error al guardar la imagen del Usuario [" + IdUsuario + "].. Motivo: " + ex.Message;
                _log.Error(_MsgError.ToString().Trim());
            }
            finally
            {
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    TheDataReaderPostgreSQL = null;
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    TheCommandMySQL = null;
                    TheDataReaderMySQL.Close();
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                    TheDataReaderSQLServer.Close();
                }
                else if (myConnectionDb is OracleConnection)
                {
                    TheCommandOracle = null;
                    TheDataReaderOracle.Close();
                }
                myConnectionDb.Close();
                myConnectionDb.Dispose();
            }

            return retValor;
        }
        #endregion

    }
}