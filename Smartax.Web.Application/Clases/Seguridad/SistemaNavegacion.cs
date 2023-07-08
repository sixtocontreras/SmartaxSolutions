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
    public class SistemaNavegacion
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
        private object _IdNavegacion;
        private string _TituloOpcion;
        private string _DescripcionOpcion;
        private string _UrlOpcion;
        private object _PadreOpcion;
        private int _OrdenOpcion;
        private int _IdTipoMenu;
        private object _IdEstado;
        private int _IdRol;
        private int _IdEmpresa;
        private int _IdUsuario;
        private string _MostrarSeleccione;
        private string _MotorBaseDatos;
        private int _TipoProceso;

        public object IdNavegacion
        {
            get
            {
                return _IdNavegacion;
            }

            set
            {
                _IdNavegacion = value;
            }
        }

        public string TituloOpcion
        {
            get
            {
                return _TituloOpcion;
            }

            set
            {
                _TituloOpcion = value;
            }
        }

        public string DescripcionOpcion
        {
            get
            {
                return _DescripcionOpcion;
            }

            set
            {
                _DescripcionOpcion = value;
            }
        }

        public string UrlOpcion
        {
            get
            {
                return _UrlOpcion;
            }

            set
            {
                _UrlOpcion = value;
            }
        }

        public object PadreOpcion
        {
            get
            {
                return _PadreOpcion;
            }

            set
            {
                _PadreOpcion = value;
            }
        }

        public int OrdenOpcion
        {
            get
            {
                return _OrdenOpcion;
            }

            set
            {
                _OrdenOpcion = value;
            }
        }

        public int IdTipoMenu
        {
            get
            {
                return _IdTipoMenu;
            }

            set
            {
                _IdTipoMenu = value;
            }
        }

        public string MostrarSeleccione
        {
            get
            {
                return _MostrarSeleccione;
            }

            set
            {
                _MostrarSeleccione = value;
            }
        }

        public string MotorBaseDatos
        {
            get
            {
                return _MotorBaseDatos;
            }

            set
            {
                _MotorBaseDatos = value;
            }
        }

        public int TipoProceso
        {
            get
            {
                return _TipoProceso;
            }

            set
            {
                _TipoProceso = value;
            }
        }

        public object IdEstado
        {
            get
            {
                return _IdEstado;
            }

            set
            {
                _IdEstado = value;
            }
        }

        public int IdRol
        {
            get
            {
                return _IdRol;
            }

            set
            {
                _IdRol = value;
            }
        }

        public int IdEmpresa
        {
            get
            {
                return _IdEmpresa;
            }

            set
            {
                _IdEmpresa = value;
            }
        }

        public int IdUsuario
        {
            get
            {
                return _IdUsuario;
            }

            set
            {
                _IdUsuario = value;
            }
        }
        #endregion

        #region DEFINICION DE METODOS PARA PERMISOS Y ARMAR MENU DE OPCIONES
        public int TraerNavegacionID(string UrlNavegacion)
        {
            int Result = 0;
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
                    return 0;
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_sistema_navegacion", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_url", UrlNavegacion.ToString().Trim());
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            //Datos de las opciones principal del menu
                            IdNavegacion = Int32.Parse(TheDataReaderPostgreSQL["id_navegacion"].ToString().Trim());
                            TituloOpcion = TheDataReaderPostgreSQL["titulo_opcion"].ToString().Trim();
                            DescripcionOpcion = TheDataReaderPostgreSQL["descripcion_opcion"].ToString().Trim();
                            Result = Int32.Parse(IdNavegacion.ToString().Trim());
                            break;
                        }
                    }
                    else
                    {
                        return Result;
                    }
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Base de datos MySql.
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_sistema_navegacion";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandSQLServer.Parameters.Clear();

                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_url", UrlNavegacion.ToString().Trim());
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    if (TheDataReaderSQLServer != null)
                    {
                        while (TheDataReaderSQLServer.Read())
                        {
                            //Datos de las opciones principal del menu
                            IdNavegacion = Int32.Parse(TheDataReaderSQLServer["id_navegacion"].ToString().Trim());
                            TituloOpcion = TheDataReaderSQLServer["titulo_opcion"].ToString().Trim();
                            DescripcionOpcion = TheDataReaderSQLServer["descripcion_opcion"].ToString().Trim();
                            Result = Int32.Parse(IdNavegacion.ToString().Trim());
                            break;
                        }
                    }
                    else
                    {
                        return Result;
                    }
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //Base de datos Oracle.
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return Result;
                }
            }
            catch (Exception ex)
            {
                Result = 0;
                _log.Error("Error al traer informacion de la tabla Navegación. Motivo: " + ex.Message);
            }
            finally
            {
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    if(TheDataReaderPostgreSQL != null)
                    {
                        TheDataReaderPostgreSQL.Close();
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
            }
            return Result;
        }

        //Aqui retornamos en un Dataset el menu completo con sus respectivos opciones de sub-menu
        public DataSet GetMenu()
        {
            DataSet obtNavDataSet = new DataSet();
            DataTable obtNavDataTable = new DataTable();
            obtNavDataSet.DataSetName = "DtMenus";
            obtNavDataTable.TableName = "DtMenu";

            PgSqlCommand TheCommandPostgreSQL = null;
            PgSqlDataReader TheDataReaderPostgreSQL = null;
            try
            {
                #region DEFINICION DEL OBJETOD DE CONEXION A LA DB
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_sistema_navegacion", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    DataColumn[] keys = new DataColumn[2];
                    keys[0] = obtNavDataTable.Columns.Add("MenuID");
                    obtNavDataTable.PrimaryKey = keys;
                    obtNavDataTable.Columns.Add("titulo_opcion");
                    obtNavDataTable.Columns.Add("descripcion_opcion");
                    obtNavDataTable.Columns.Add("url_opcion");
                    obtNavDataTable.Columns.Add("ParentID");

                    SistemaPermiso objPermiso = new SistemaPermiso();
                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            objPermiso.IdRol = IdRol;
                            if (objPermiso.TienePermisos(myConnectionDb, Convert.ToInt32(TheDataReaderPostgreSQL["id_navegacion"].ToString().Trim()), IdUsuario, MotorBaseDatos.ToString().Trim()))
                            {
                                DataRow objRow = null;
                                objRow = obtNavDataTable.NewRow();
                                objRow["MenuID"] = TheDataReaderPostgreSQL["id_navegacion"].ToString().Trim();
                                objRow["titulo_opcion"] = TheDataReaderPostgreSQL["titulo_opcion"].ToString().Trim();
                                objRow["descripcion_opcion"] = TheDataReaderPostgreSQL["descripcion_opcion"].ToString().Trim();
                                objRow["url_opcion"] = TheDataReaderPostgreSQL["url_opcion"].ToString().Trim();

                                if ((TheDataReaderPostgreSQL["padre_opcion"] != null))
                                {
                                    objRow["ParentID"] = TheDataReaderPostgreSQL["padre_opcion"];
                                }
                                else
                                {
                                    objRow["ParentID"] = DBNull.Value;
                                }

                                obtNavDataTable.Rows.Add(objRow);
                                obtNavDataTable = CargarArbolMenu(myConnectionDb, obtNavDataTable, Convert.ToInt32(TheDataReaderPostgreSQL["id_navegacion"].ToString().Trim()), IdUsuario);
                            }
                        }
                    }

                    //Datos del arbol.
                    obtNavDataSet.Tables.Add(obtNavDataTable);
                    DataRelation relation = new DataRelation("ParentChild", obtNavDataSet.Tables["DtMenu"].Columns["MenuID"], obtNavDataSet.Tables["DtMenu"].Columns["ParentID"], true);
                    relation.Nested = true;
                    obtNavDataSet.Relations.Add(relation);
                    #endregion
                }
                else if (myConnectionDb is SqlConnection)
                {
                    #region OBTENER DATOS DE LA DB DE SQL SERVER
                    //SQL SERVER
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_mapa_sistema_navegacion";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    DataColumn[] keys = new DataColumn[2];
                    keys[0] = obtNavDataTable.Columns.Add("MenuID");
                    obtNavDataTable.PrimaryKey = keys;
                    obtNavDataTable.Columns.Add("titulo_opcion");
                    obtNavDataTable.Columns.Add("descripcion_opcion");
                    obtNavDataTable.Columns.Add("URL");
                    obtNavDataTable.Columns.Add("ParentID");

                    SistemaPermiso objPermiso = new SistemaPermiso();
                    if (TheDataReaderSQLServer != null)
                    {
                        while (TheDataReaderSQLServer.Read())
                        {
                            if (objPermiso.TienePermisos(myConnectionDb, Convert.ToInt32(TheDataReaderSQLServer["id_navegacion"].ToString().Trim()), IdUsuario, MotorBaseDatos.ToString().Trim()))
                            {
                                DataRow objRow = null;
                                objRow = obtNavDataTable.NewRow();
                                objRow["MenuID"] = TheDataReaderSQLServer["NavegacionId"].ToString().Trim();
                                objRow["titulo_opcion"] = TheDataReaderSQLServer["titulo_opcion"].ToString().Trim();
                                objRow["descripcion_opcion"] = TheDataReaderSQLServer["descripcion_opcion"].ToString().Trim();
                                objRow["url_opcion"] = TheDataReaderSQLServer["url_opcion"].ToString().Trim();

                                if ((TheDataReaderSQLServer["padre_opcion"] != null))
                                {
                                    objRow["ParentID"] = TheDataReaderSQLServer["padre_opcion"];
                                }
                                else
                                {
                                    objRow["ParentID"] = DBNull.Value;
                                }

                                obtNavDataTable.Rows.Add(objRow);
                                obtNavDataTable = CargarArbolMenu(myConnectionDb, obtNavDataTable, Convert.ToInt32(TheDataReaderSQLServer["id_navegacion"].ToString().Trim()), IdUsuario);
                            }
                        }
                    }

                    //Datos del arbol
                    obtNavDataSet.Tables.Add(obtNavDataTable);
                    DataRelation relation = new DataRelation("ParentChild", obtNavDataSet.Tables["DtMenu"].Columns["MenuID"], obtNavDataSet.Tables["DtMenu"].Columns["ParentID"], true);
                    relation.Nested = true;
                    obtNavDataSet.Relations.Add(relation);
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //MySQL
                }
                else if (myConnectionDb is OracleConnection)    //ORACLE
                {

                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return obtNavDataSet;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al traer informacion de la tabla Navegación. Motivo: " + ex.Message);
            }
            finally
            {
                #region DEFINICION DEL FINALLY
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    TheDataReaderPostgreSQL.Close();
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

            return obtNavDataSet;
        }

        //Aqui asignamos los hijos de cada una de las opciones de menu 
        private DataTable CargarArbolMenu(IDbConnection myConnectionDb, DataTable obtNavDataTable, int MenuID, int IdUsuario)
        {
            PgSqlCommand TheCommandPostgreSQL_AM = null;
            PgSqlDataReader TheDataReaderPostgreSQL_AM = null;
            try
            {
                //Aqui revisamos que la conexion a la BD se encuentre abierta para no volver abrir la conexion
                if (myConnectionDb == null || myConnectionDb.State != ConnectionState.Open)
                {
                    #region OBJETO DE CONEXION A LA DB
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
                    #endregion
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
                    #region OBTENER DATOS DE LA DB DE POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL_AM = new PgSqlCommand("sp_web_padre_sistema_navegacion", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL_AM.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL_AM.Parameters.AddWithValue("@p_in_padre", MenuID);
                    TheDataReaderPostgreSQL_AM = TheCommandPostgreSQL_AM.ExecuteReader();

                    SistemaPermiso objPermiso = new SistemaPermiso();
                    if (TheDataReaderPostgreSQL_AM != null)
                    {
                        while (TheDataReaderPostgreSQL_AM.Read())
                        {
                            if (objPermiso.TienePermisos(myConnectionDb, Convert.ToInt32(TheDataReaderPostgreSQL_AM["id_navegacion"].ToString().Trim()), IdUsuario, MotorBaseDatos.ToString().Trim()))
                            {
                                DataRow objRow = null;
                                objRow = obtNavDataTable.NewRow();
                                objRow["MenuID"] = TheDataReaderPostgreSQL_AM["id_navegacion"].ToString().Trim();
                                objRow["titulo_opcion"] = TheDataReaderPostgreSQL_AM["titulo_opcion"].ToString().Trim();
                                objRow["descripcion_opcion"] = TheDataReaderPostgreSQL_AM["descripcion_opcion"].ToString().Trim();
                                objRow["url_opcion"] = TheDataReaderPostgreSQL_AM["url_opcion"].ToString().Trim();
                                objRow["ParentID"] = MenuID;

                                obtNavDataTable.Rows.Add(objRow);
                                obtNavDataTable = CargarArbolMenu(myConnectionDb, obtNavDataTable, Convert.ToInt32(TheDataReaderPostgreSQL_AM["id_navegacion"].ToString().Trim()), IdUsuario);
                            }
                        }
                    }
                    #endregion
                }
                else if (myConnectionDb is SqlConnection)   //SQLSERVER
                {
                    #region OBTENER DATOS DE LA DB DE SQL SERVER
                    SqlCommand TheCommandSQLServer = new SqlCommand();
                    SqlDataReader TheDataReader = null;
                    TheCommandSQLServer.CommandText = "sp_padre_sistema_navegacion";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_padre", MenuID);
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    TheDataReader = TheCommandSQLServer.ExecuteReader();

                    SistemaPermiso objPermiso = new SistemaPermiso();
                    if (TheDataReader != null)
                    {
                        while (TheDataReader.Read())
                        {
                            if (objPermiso.TienePermisos(myConnectionDb, Convert.ToInt32(TheDataReader["NavegacionId"].ToString().Trim()), IdUsuario, MotorBaseDatos.ToString().Trim()))
                            {
                                DataRow objRow = null;
                                objRow = obtNavDataTable.NewRow();
                                objRow["MenuID"] = TheDataReader["NavegacionId"].ToString().Trim();
                                objRow["titulo_opcion"] = TheDataReader["titulo_opcion"].ToString().Trim();
                                objRow["descripcion_opcion"] = TheDataReader["descripcion_opcion"].ToString().Trim();
                                objRow["url_opcion"] = TheDataReader["url_opcion"].ToString().Trim();
                                objRow["ParentID"] = MenuID;

                                obtNavDataTable.Rows.Add(objRow);
                                obtNavDataTable = CargarArbolMenu(myConnectionDb, obtNavDataTable, Convert.ToInt32(TheDataReader["NavegacionId"].ToString().Trim()), IdUsuario);
                            }
                        }
                    }
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection) //MySQL
                {

                }
                else if (myConnectionDb is OracleConnection)    //ORACLE
                {

                }
                else
                {
                    obtNavDataTable = null;
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                }
            }
            catch (Exception ex)
            {
                _log.Error("Se produjo una exepción con el Metodo [CargarArbolMenu]. Motivo: " + ex.Message);
            }
            finally
            {
                #region DEFINICION DEL FINALLY
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL_AM = null;
                    TheDataReaderPostgreSQL_AM.Close();
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
                #endregion
            }

            return obtNavDataTable;
        }
        #endregion

        #region DEFINICION DE METODOS PARA OBTENER LOS DATOS DE LA BD.
        public DataTable GetAllNavegacion()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtNavegacion";
            try
            {
                #region DEFINICION DEL OBJETO DE CONEXION A LA DB
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_datos_all_sistema_navegacion", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("id_navegacion", typeof(Int32));
                    TablaDatos.Columns.Add("titulo_opcion");
                    TablaDatos.Columns.Add("descripcion_opcion");
                    TablaDatos.Columns.Add("url_opcion");
                    TablaDatos.Columns.Add("padre_id");
                    TablaDatos.Columns.Add("titulo_opcion2");
                    TablaDatos.Columns.Add("orden_opcion", typeof(Int32));
                    TablaDatos.Columns.Add("idtipo_menu", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_menu");
                    TablaDatos.Columns.Add("fecha_registro");

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_navegacion"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_navegacion"].ToString().Trim());
                            Fila["titulo_opcion"] = TheDataReaderPostgreSQL["titulo_opcion"].ToString().Trim();
                            Fila["descripcion_opcion"] = TheDataReaderPostgreSQL["descripcion_opcion"].ToString().Trim();
                            Fila["url_opcion"] = TheDataReaderPostgreSQL["url_opcion"].ToString().Trim();

                            if (TheDataReaderPostgreSQL["padre_opcion"].ToString().Trim().Length > 0)
                            {
                                Fila["padre_id"] = Int32.Parse(TheDataReaderPostgreSQL["padre_opcion"].ToString().Trim());
                                Fila["titulo_opcion2"] = TheDataReaderPostgreSQL["titulo_opcion"].ToString().Trim();
                            }
                            else
                            {
                                Fila["padre_id"] = DBNull.Value;
                                Fila["titulo_opcion2"] = DBNull.Value;
                            }

                            Fila["orden_opcion"] = Convert.ToInt32(TheDataReaderPostgreSQL["orden_opcion"].ToString().Trim());
                            Fila["idtipo_menu"] = Convert.ToInt32(TheDataReaderPostgreSQL["idtipo_menu"].ToString().Trim());
                            Fila["tipo_menu"] = TheDataReaderPostgreSQL["tipo_menu"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
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
                    #region OBTENER DATOS DE LA DB SQLSERVER
                    //Para Base de Datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_datos_all_sistema_navegacion";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    TablaDatos.Columns.Add("id_navegacion", typeof(Int32));
                    TablaDatos.Columns.Add("titulo_opcion");
                    TablaDatos.Columns.Add("descripcion_opcion");
                    TablaDatos.Columns.Add("url_opcion");
                    TablaDatos.Columns.Add("padre_id");
                    TablaDatos.Columns.Add("orden_opcion", typeof(Int32));
                    TablaDatos.Columns.Add("idtipo_menu", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_menu");
                    TablaDatos.Columns.Add("fecha_registro");

                    if (TheDataReaderSQLServer != null)
                    {
                        while (TheDataReaderSQLServer.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_navegacion"] = Convert.ToInt32(TheDataReaderSQLServer["id_navegacion"].ToString().Trim());
                            Fila["titulo_opcion"] = TheDataReaderSQLServer["titulo_opcion"].ToString().Trim();
                            Fila["descripcion_opcion"] = TheDataReaderSQLServer["descripcion_opcion"].ToString().Trim();
                            Fila["url_opcion"] = TheDataReaderSQLServer["url_opcion"].ToString().Trim();

                            if (TheDataReaderSQLServer["padre_opcion"].ToString().Trim().Length > 0)
                            {
                                Fila["padre_id"] = Int32.Parse(TheDataReaderSQLServer["padre_opcion"].ToString().Trim());
                            }
                            else
                            {
                                Fila["padre_id"] = DBNull.Value;
                            }

                            Fila["orden_opcion"] = Convert.ToInt32(TheDataReaderSQLServer["orden_opcion"].ToString().Trim());
                            Fila["idtipo_menu"] = Convert.ToInt32(TheDataReaderSQLServer["id_tipo_menu"].ToString().Trim());
                            Fila["tipo_menu"] = TheDataReaderSQLServer["tipo_menu"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderSQLServer["fecha_registro"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                        }
                    }
                    #endregion
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_tipo_licencia]. Motivo: " + ex.Message);
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

        public DataTable GetPadreNavegacion()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtPadreNavegacion";
            try
            {
                #region DEFINICION DEL OBJETO DE CONEXION A LA DB
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_datos_all_sistema_navegacion_padre", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("padre_id");
                    TablaDatos.Columns.Add("titulo_opcion");

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
                            Fila["padre_id"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_navegacion"].ToString().Trim());
                            Fila["titulo_opcion"] = TheDataReaderPostgreSQL["titulo_opcion"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                        }
                    }
                    #endregion
                }
                else if (myConnectionDb is SqlConnection)
                {
                    #region OBTENER DATOS DE LA DB SQL SERVER
                    //Para Base de Datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_datos_all_sistema_rol";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    TablaDatos.Columns.Add("padre_id", typeof(Int32));
                    TablaDatos.Columns.Add("titulo_opcion");

                    if (TheDataReaderSQLServer != null)
                    {
                        if (MostrarSeleccione.ToString().Trim().Equals("SI"))
                        {
                            TablaDatos.Rows.Add("", "<< Seleccione >>");
                        }
                        while (TheDataReaderSQLServer.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["padre_id"] = Convert.ToInt32(TheDataReaderSQLServer["padre_id"].ToString().Trim());
                            Fila["titulo_opcion"] = TheDataReaderSQLServer["titulo_opcion"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                        }
                    }
                    #endregion
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_tipo_licencia]. Motivo: " + ex.Message);
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

        public bool AddUpNavegacion(DataRow Fila, ref int _IdRegistro, ref string _MsgError)
        {
            bool retValor = false;
            try
            {
                #region DEFINICION DEL OBJETO DE CONEXION A LA DB
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_sistema_navegacion", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idnavegacion", IdNavegacion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_titulo_opcion", Fila["titulo_opcion"].ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_descripcion_opcion", Fila["descripcion_opcion"].ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_url_opcion", Fila["url_opcion"].ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_padre_opcion", Fila["padre_id"]);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_orden_opcion", Fila["orden_opcion"]);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_menu", Fila["idtipo_menu"]);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario_add", IdUsuario);
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

                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_crud_sistema_navegacion";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (System.Data.SqlClient.SqlConnection)myConnectionDb;
                    //se limpian los parámetros
                    TheCommandSQLServer.Parameters.Clear();

                    TituloOpcion = Fila["titulo_opcion"].ToString().Trim();
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idnavegacion", IdNavegacion);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_titulo_opcion", TituloOpcion);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_descripcion_opcion", Fila["descripcion_opcion"].ToString().Trim());
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_url_opcion", Fila["url_opcion"].ToString().Trim());
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_padre_opcion", Fila["padre_id"]);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_orden_opcion", Fila["orden_opcion"]);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idtipo_menu", Fila["idtipo_menu"]);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_tipo_proceso", TipoProceso);

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
                            _MsgError = "La opción de menu [" + TituloOpcion + "] ha sido registrado exitosamente.";
                        }
                        else if (TipoProceso == 2)
                        {
                            _MsgError = "La opción de menu [" + TituloOpcion + "] han sido editados exitosamente.";
                        }
                        else if (TipoProceso == 3)
                        {
                            _MsgError = "La opción de menu [" + TituloOpcion + "] ah sido eliminado del sistema.";
                        }

                        retValor = true;
                    }
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
                _MsgError = "Error al registrar el proceso del rol. Motivo: " + ex.Message.ToString().Trim();
                _log.Error(_MsgError.ToString().Trim());
            }
            finally
            {
                #region METODO FINALLY
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
        #endregion

    }
}