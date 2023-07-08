using Devart.Data.PostgreSql;
using log4net;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Smartax.Web.Application.Clases.Seguridad
{
    public class SistemaPermiso
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

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
        public int IdRol;
        public int IdNavegacion;
        public string NombreOpcionMenu;
        public int RolSistema;
        public bool PuedeLeer;
        public bool PuedeRegistrar;
        public bool PuedeModificar;
        public bool PuedeEliminar;
        public bool PuedeBloquear;
        public bool PuedeAnular;
        public bool PuedeExportar;
        public bool PuedeConfigurar;
        public bool PuedeLiqBorrador;
        public bool PuedeLiqDefinitivo;
        public bool PuedeVerFormulario;
        public string PathUrl;
        public int IdUsuario;
        public int IdUsuarioAdd;
        public int IdEmpresa;
        public string MostrarSeleccione;
        public string MotorBaseDatos;
        public int TipoProceso;
        #endregion

        #region DEFINICION DE METODOS PARA ALICAR PERMISOS EN EL SISTEMA
        //Método para refrescar los permisos
        public void RefrescarPermisos()
        {
            try
            {
                SistemaNavegacion ObjNavegacion = new SistemaNavegacion();
                ObjNavegacion.MotorBaseDatos = MotorBaseDatos;

                //string strPagina = HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString().Trim();
                IdNavegacion = ObjNavegacion.TraerNavegacionID(PathUrl);

                if (IdNavegacion != 0)
                {
                    this.ValidarAccesoFrm(IdUsuario, IdNavegacion);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al refrescar los permisos. Motivo: " + ex.Message.ToString().Trim());
            }
        }

        //Esta funcion nos sirve para determinar si el usuario tiene acceso a la opcion determinada
        public bool TienePermisos(IDbConnection myConnectionDb, int IdNavegacion, int IdUsuario, string BaseDatos)
        {
            bool TienePermisos = false;
            try
            {
                SistemaPermiso ObjPermiso = new SistemaPermiso();
                ObjPermiso.MotorBaseDatos = BaseDatos.ToString().Trim();
                ObjPermiso.ValidarAccesoMenu(myConnectionDb, IdUsuario, IdNavegacion);
                if (IdRol == 1)
                {
                    //Id rol de soporte
                    TienePermisos = true;
                }
                else
                {
                    TienePermisos = ObjPermiso.PuedeLeer;
                }
            }
            catch (Exception ex)
            {
                TienePermisos = false;
                _log.Error("Error en la función [TienePermisos]. Motivo: " + ex.Message);
            }
            return TienePermisos;
        }

        //Esta funcion nos sirve para determinar si el usuario tiene acceso a la opcion especificada en IdNavegacion
        public void ValidarAccesoMenu(IDbConnection myConnectionDb, int IdUsuario, int IdNavegacion)
        {
            PgSqlCommand TheCommandPostgreSQL_VA = null;
            PgSqlDataReader TheDataReaderPostgreSQL_VA = null;
            try
            {
                //Aqui revisamos que la conexion a la BD se encuentre abierta para no volver abrir la conexion
                if (myConnectionDb == null || myConnectionDb.State != ConnectionState.Open)
                {
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
                    }
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }

                //Aqui hacemos los llamados de los sp o consultas a utilizar en la respectiva base de datos
                if (myConnectionDb is PgSqlConnection)
                {
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL_VA = new PgSqlCommand("sp_web_validar_permiso", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL_VA.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL_VA.Parameters.AddWithValue("@p_in_id_usuario", IdUsuario);
                    TheDataReaderPostgreSQL_VA = TheCommandPostgreSQL_VA.ExecuteReader();

                    if (TheDataReaderPostgreSQL_VA != null)
                    {
                        while (TheDataReaderPostgreSQL_VA.Read())
                        {
                            //Luego se consulta los permisos de dicho rol en el ID de Navegacion
                            DevolverPermiso(myConnectionDb, Convert.ToInt32(TheDataReaderPostgreSQL_VA["id_rol"].ToString().Trim()), IdNavegacion);
                        }
                    }
                }
                else if (myConnectionDb is MySqlConnection) //MySQL
                {

                }
                else if (myConnectionDb is SqlConnection)   //SQLSERVER
                {
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_validar_permiso";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_id_usuario", IdUsuario);
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    if (TheDataReaderSQLServer != null)
                    {
                        while (TheDataReaderSQLServer.Read())
                        {
                            //Luego se consulta los permisos de dicho rol en el ID de Navegacion
                            DevolverPermiso(myConnectionDb, Convert.ToInt32(TheDataReaderSQLServer["id_rol"].ToString().Trim()), IdNavegacion);
                        }
                    }
                }
                else if (myConnectionDb is OracleConnection)    //ORACLE
                {

                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                }
            }
            catch (Exception ex)
            {
                _log.Error("Se produjo una exepción con el Metodo [ValidarAcceso]. Motivo: " + ex.Message);
            }
            finally
            {
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL_VA = null;
                    TheDataReaderPostgreSQL_VA.Close();
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
            }
        }

        //Esta funcion nos sirve para determinar lo que el suaurio puede realizar dentro de la Interfaz o Formulario
        public void ValidarAccesoFrm(int IdUsuario, int IdNavegacion)
        {
            PgSqlCommand TheCommandPostgreSQL_VA = null;
            PgSqlDataReader TheDataReaderPostgreSQL_VA = null;
            try
            {
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
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }

                //Aqui hacemos los llamados de los sp o consultas a utilizar en la respectiva base de datos
                if (myConnectionDb is PgSqlConnection)
                {
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL_VA = new PgSqlCommand("sp_web_validar_permiso", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL_VA.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL_VA.Parameters.AddWithValue("@p_in_id_usuario", IdUsuario);
                    TheDataReaderPostgreSQL_VA = TheCommandPostgreSQL_VA.ExecuteReader();

                    if (TheDataReaderPostgreSQL_VA != null)
                    {
                        while (TheDataReaderPostgreSQL_VA.Read())
                        {
                            //Luego se consulta los permisos de dicho rol en el ID de Navegacion
                            DevolverPermiso(myConnectionDb, Convert.ToInt32(TheDataReaderPostgreSQL_VA["id_rol"].ToString().Trim()), IdNavegacion);
                        }
                    }
                }
                else if (myConnectionDb is MySqlConnection) //MySQL
                {

                }
                else if (myConnectionDb is SqlConnection)   //SQLSERVER
                {
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_validar_permiso";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_id_usuario", IdUsuario);
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    if (TheDataReaderSQLServer != null)
                    {
                        while (TheDataReaderSQLServer.Read())
                        {
                            //Luego se consulta los permisos de dicho rol en el ID de Navegacion
                            DevolverPermiso(myConnectionDb, Convert.ToInt32(TheDataReaderSQLServer["id_rol"].ToString().Trim()), IdNavegacion);
                        }
                    }
                }
                else if (myConnectionDb is OracleConnection)    //ORACLE
                {

                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                }
            }
            catch (Exception ex)
            {
                _log.Error("Se produjo una exepción con el Metodo [ValidarAcceso]. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION A LA DB
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL_VA = null;
                    if (TheDataReaderPostgreSQL_VA != null)
                    {
                        TheDataReaderPostgreSQL_VA.Close();
                    }
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
                #endregion
            }
        }

        //Funcion para devolver los permisos que tiene el usuario en la opcion de menu
        public bool DevolverPermiso(IDbConnection myConnectionDb, int RolID, int IdNavegacion)
        {
            bool retValor = false;
            PgSqlCommand TheCommandPostgreSQL_DP = null;
            PgSqlDataReader TheDataReaderPostgreSQL_DP = null;
            try
            {
                //Aqui revisamos que la conexion a la BD se encuentre abierta para no volver abrir la conexion
                if (myConnectionDb == null || myConnectionDb.State != ConnectionState.Open)
                {
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
                        retValor = false;
                        _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    }
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }

                //Aqui hacemos los llamados de los sp o consultas a utilizar en la respectiva base de datos
                if (myConnectionDb is PgSqlConnection)
                {
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL_DP = new PgSqlCommand("sp_web_get_devolver_permiso", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL_DP.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL_DP.Parameters.AddWithValue("@p_in_id_navegacion", IdNavegacion);
                    TheCommandPostgreSQL_DP.Parameters.AddWithValue("@p_in_id_rol", RolID);
                    TheDataReaderPostgreSQL_DP = TheCommandPostgreSQL_DP.ExecuteReader();

                    if (TheDataReaderPostgreSQL_DP != null)
                    {
                        while (TheDataReaderPostgreSQL_DP.Read())
                        {
                            PuedeLeer = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_leer"]);
                            PuedeRegistrar = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_registrar"]);
                            PuedeModificar = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_modificar"]);
                            PuedeEliminar = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_eliminar"]);
                            PuedeBloquear = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_bloquear"]);
                            PuedeAnular = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_anular"]);
                            PuedeExportar = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_exportar"]);
                            PuedeLiqBorrador = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_liq_borrador"]);
                            PuedeLiqDefinitivo = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_liq_definitivo"]);
                            PuedeVerFormulario = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_ver_formulario"]);
                            retValor = true;
                            break;
                        }
                    }
                    else
                    {
                        retValor = false;
                    }
                }
                else if (myConnectionDb is MySqlConnection) //MYSQL
                {

                }
                else if (myConnectionDb is SqlConnection)   //SQLSERVER
                { 
                }
                else if (myConnectionDb is OracleConnection)    //ORACLE
                {

                }
                else
                {
                    retValor = false;
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                }

                return retValor;
            }
            catch (Exception ex)
            {
                _log.Error("Se produjo una exepción con el Metodo [DevolverPermiso]. Motivo: " + ex.Message);
                retValor = false;
            }
            finally
            {
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL_DP = null;
                    TheDataReaderPostgreSQL_DP.Close();
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
            }

            return retValor;
        }

        //Funcion para devolver los permisos que tiene el rol seleccionado
        public bool DevolverPermisoMenu(int RolID, int IdNavegacion)
        {
            bool retValor = false;
            PgSqlCommand TheCommandPostgreSQL_DP = null;
            PgSqlDataReader TheDataReaderPostgreSQL_DP = null;
            try
            {
                #region OBTENER OBJETO DE CONEXION DE LA DB
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
                    retValor = false;
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
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
                    TheCommandPostgreSQL_DP = new PgSqlCommand("sp_web_get_devolver_permiso", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL_DP.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL_DP.Parameters.AddWithValue("@p_in_id_navegacion", IdNavegacion);
                    TheCommandPostgreSQL_DP.Parameters.AddWithValue("@p_in_id_rol", RolID);
                    TheDataReaderPostgreSQL_DP = TheCommandPostgreSQL_DP.ExecuteReader();

                    if (TheDataReaderPostgreSQL_DP != null)
                    {
                        while (TheDataReaderPostgreSQL_DP.Read())
                        {
                            PuedeLeer = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_leer"]);
                            PuedeRegistrar = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_registrar"]);
                            PuedeModificar = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_modificar"]);
                            PuedeEliminar = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_eliminar"]);
                            PuedeBloquear = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_bloquear"]);
                            PuedeAnular = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_anular"]);
                            PuedeExportar = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_exportar"]);
                            PuedeConfigurar = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_configurar"]);
                            PuedeLiqBorrador = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_liq_borrador"]);
                            PuedeLiqDefinitivo = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_liq_definitivo"]);
                            PuedeVerFormulario = Convert.ToBoolean(TheDataReaderPostgreSQL_DP["puede_ver_formulario"]);
                            retValor = true;
                            break;
                        }
                    }
                    else
                    {
                        retValor = false;
                    }
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection) //MYSQL
                {

                }
                else if (myConnectionDb is SqlConnection)   //SQLSERVER
                {
                    //--SQL SERVER
                }
                else if (myConnectionDb is OracleConnection)    //ORACLE
                {

                }
                else
                {
                    retValor = false;
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                }

                return retValor;
            }
            catch (Exception ex)
            {
                _log.Error("Se produjo una exepción con el Metodo [DevolverPermisoMenu]. Motivo: " + ex.Message);
                retValor = false;
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION DE LA DB
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL_DP = null;
                    TheDataReaderPostgreSQL_DP.Close();
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
                #endregion
            }

            return retValor;
        }
        #endregion

        public DataTable GetPermisosRol()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtPermisosRol";
            try
            {
                #region OBJETO DE CONEXION A LA BASE DE DATOS
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_permisos_rol", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINIR COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("id_navegacion", typeof(Int32));
                    TablaDatos.Columns.Add("titulo_opcion");
                    TablaDatos.Columns.Add("descripcion_opcion");
                    TablaDatos.Columns.Add("puede_leer", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_registrar", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_modificar", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_eliminar", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_bloquear", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_anular", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_exportar", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_liq_borrador", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_liq_definitivo", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_ver_formulario", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_configurar", typeof(Boolean));
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DEL DATA READER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_navegacion"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_navegacion"].ToString().Trim());
                            Fila["titulo_opcion"] = TheDataReaderPostgreSQL["titulo_opcion"].ToString().Trim();
                            Fila["descripcion_opcion"] = TheDataReaderPostgreSQL["descripcion_opcion"].ToString().Trim();

                            if (TheDataReaderPostgreSQL["puede_leer"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_leer"] = true;
                            }
                            else
                            {
                                Fila["puede_leer"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_registrar"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_registrar"] = true;
                            }
                            else
                            {
                                Fila["puede_registrar"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_modificar"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_modificar"] = true;
                            }
                            else
                            {
                                Fila["puede_modificar"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_eliminar"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_eliminar"] = true;
                            }
                            else
                            {
                                Fila["puede_eliminar"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_bloquear"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_bloquear"] = true;
                            }
                            else
                            {
                                Fila["puede_bloquear"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_anular"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_anular"] = true;
                            }
                            else
                            {
                                Fila["puede_anular"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_exportar"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_exportar"] = true;
                            }
                            else
                            {
                                Fila["puede_exportar"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_liq_borrador"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_liq_borrador"] = true;
                            }
                            else
                            {
                                Fila["puede_liq_borrador"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_liq_definitivo"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_liq_definitivo"] = true;
                            }
                            else
                            {
                                Fila["puede_liq_definitivo"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_ver_formulario"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_ver_formulario"] = true;
                            }
                            else
                            {
                                Fila["puede_ver_formulario"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_configurar"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_configurar"] = true;
                            }
                            else
                            {
                                Fila["puede_configurar"] = false;
                            }

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
                return TablaDatos;
                _log.Error("Error al obtener los datos de la Tabla [tbl_sistema_permiso]. Motivo: " + ex.Message);
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

        public bool AddUpPermisos(ref int _IdRegistro, ref string _MsgError)
        {
            bool retValor = false;
            try
            {
                #region OBJETO DE CONEXION A LA DB
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
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
                    return false;
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
                    #region REGISTRAR INFO EN LA DB DE POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_sistema_permiso", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idnavegacion", IdNavegacion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_nombre_opcion", NombreOpcionMenu);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_puede_leer", PuedeLeer == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_puede_registrar", PuedeRegistrar == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_puede_modificar", PuedeModificar == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_puede_eliminar", PuedeEliminar == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_puede_bloquear", PuedeBloquear == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_puede_anular", PuedeAnular == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_puede_exportar", PuedeExportar == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_puede_configurar", PuedeConfigurar == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_puede_liq_borrador", PuedeLiqBorrador == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_puede_liq_definitivo", PuedeLiqDefinitivo == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_puede_ver_formulario", PuedeVerFormulario == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuarioAdd);
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
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {

                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                }
                else if (myConnectionDb is OracleConnection)
                {

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
                _MsgError = "Error al registrar el proceso del permiso. Motivo: " + ex.Message.ToString().Trim();
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

        public bool SetProcesoSistemaPermisoRol(ref int _IdRegistro, ref string _MsgError)
        {
            bool retValor = false;
            try
            {
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
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
                    return false;
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }

                //Aqui hacemos los llamados de los sp o consultas a utilizar en la respectiva base de datos
                if (myConnectionDb is PgSqlConnection)
                {
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_set_proceso_sistema_permiso", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idnavegacion", IdNavegacion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_nombre_rol", NombreOpcionMenu);
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
                    //BASE DE DATOS MYSQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_set_proceso_sistema_permiso";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (System.Data.SqlClient.SqlConnection)myConnectionDb;
                    //se limpian los parámetros
                    TheCommandSQLServer.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idnavegacion", IdNavegacion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_nombre_rol", NombreOpcionMenu);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_proceso", TipoProceso);

                    //declaramos el parámetro de retorno
                    SqlParameter ValorRetorno = new SqlParameter("@ValorRetorno", SqlDbType.Int);
                    //asignamos el valor de retorno
                    ValorRetorno.Direction = ParameterDirection.Output;
                    TheCommandSQLServer.Parameters.Add(ValorRetorno);
                    //ejecutamos el procedimiento almacenado.
                    TheCommandSQLServer.ExecuteNonQuery();
                    // traemos el valor de retorno
                    int Valor_Retornado = Convert.ToInt32(ValorRetorno.Value);

                    //dependiendo del valor de retorno se asigna la variable success
                    //si el procedimiento retorna un 1 la operación se realizó con éxito
                    //de no ser así se mantiene en false y pr lo tanto falló la operación
                    if (Valor_Retornado >= 1)
                    {
                        if (TipoProceso == 1)
                        {
                            _IdRegistro = Valor_Retornado;
                            _MsgError = "El permiso ha sido registrado exitosamente.";
                        }
                        else if (TipoProceso == 2)
                        {
                            _MsgError = "Los datos del permiso han sido editados exitosamente.";
                        }
                        else if (TipoProceso == 3)
                        {
                            _MsgError = "El permiso ah sido eliminado del sistema.";
                        }

                        retValor = true;
                    }
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //BASE DE DATOS ORACLE.
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
                _MsgError = "Error al registrar el proceso del permiso. Motivo: " + ex.Message.ToString().Trim();
                _log.Error(_MsgError.ToString().Trim());
            }
            finally
            {
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
            }

            return retValor;
        }

    }
}