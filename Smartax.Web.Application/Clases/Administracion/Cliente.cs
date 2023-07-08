using System;
using Devart.Data.PostgreSql;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Configuration;
using log4net;
using System.Data.SqlClient;
using System.Data.OracleClient;
using Smartax.Web.Application.Clases.Seguridad;

namespace Smartax.Web.Application.Clases.Administracion
{
    public class Cliente
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
        IDbTransaction Transac = null;

        SqlCommand TheCommandSQLServer = null;
        SqlDataReader TheDataReaderSQLServer = null;

        OracleCommand TheCommandOracle = null;
        OracleDataReader TheDataReaderOracle = null;
        #endregion

        #region DEFINICION DE ATRIBUTOS Y PROPIEDADES
        public object IdCliente { get; set; }
        public int IdTipoClasificacion { get; set; }
        public int IdTipoIdentificacion { get; set; }
        public string NumeroDocumento { get; set; }
        public string DigitoVerificacion { get; set; }
        public string RazonSocial { get; set; }
        public string InscritoRit { get; set; }
        public int IdMunUbicacionPrincipal { get; set; }
        public string DireccionCliente { get; set; }
        public string NombreContacto { get; set; }
        public string TelefonoContacto { get; set; }
        public string EmailContacto { get; set; }
        public int NumeroPuntos { get; set; }
        public string TienePresenciaNacional { get; set; }
        public string ConsorcioUnionTemporal { get; set; }
        public string ActividadPatrimAutonomo { get; set; }
        public string GranContribuyente { get; set; }
        public string NumeroPlacaMunicipal { get; set; }
        public string NumeroMatriculaIc { get; set; }
        public string NumeroRit { get; set; }
        public string AvisosTablero { get; set; }
        public object FechaInicioActividades { get; set; }
        public string PasswordUsuario { get; set; }
        public object IdTipoSector { get; set; }
        public object IdEstado { get; set; }
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public int IdEmpresaPadre { get; set; }
        public int IdRol { get; set; }
        public string MostrarSeleccione { get; set; }
        public string MotorBaseDatos { get; set; }
        public string TipoEstado { get; set; }
        public int TipoConsulta { get; set; }
        public int TipoProceso { get; set; }
        #endregion

        public DataTable GetAllClientes()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtClientes";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_cliente", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINIR COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("id_cliente", typeof(Int32));
                    TablaDatos.Columns.Add("idtipo_clasificacion", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_clasificacion");
                    TablaDatos.Columns.Add("idtipo_identificacion", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_identificacion");
                    TablaDatos.Columns.Add("numero_documento");
                    TablaDatos.Columns.Add("razon_social");
                    TablaDatos.Columns.Add("inscrito_rit");
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("direccion_cliente");
                    TablaDatos.Columns.Add("nombre_contacto");
                    TablaDatos.Columns.Add("telefono_contacto");
                    TablaDatos.Columns.Add("email_contacto");
                    TablaDatos.Columns.Add("numero_puntos", typeof(Int32));
                    TablaDatos.Columns.Add("tiene_presencia_nacional");
                    TablaDatos.Columns.Add("consorcio_union_temporal");
                    TablaDatos.Columns.Add("actividad_patrim_autonomo");
                    //TablaDatos.Columns.Add("numero_placa_municipal");
                    //TablaDatos.Columns.Add("numero_matricula_ic");
                    //TablaDatos.Columns.Add("numero_rit");
                    //TablaDatos.Columns.Add("avisos_tablero");
                    //TablaDatos.Columns.Add("fecha_inicio_actividades");
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DEL DATAREADER 
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_cliente"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_cliente"].ToString().Trim());
                            Fila["idtipo_clasificacion"] = Convert.ToInt32(TheDataReaderPostgreSQL["idtipo_clasificacion"].ToString().Trim());
                            Fila["tipo_clasificacion"] = TheDataReaderPostgreSQL["tipo_clasificacion"].ToString().Trim();
                            Fila["idtipo_identificacion"] = Convert.ToInt32(TheDataReaderPostgreSQL["idtipo_identificacion"].ToString().Trim());
                            Fila["tipo_identificacion"] = TheDataReaderPostgreSQL["tipo_identificacion"].ToString().Trim();
                            Fila["numero_documento"] = TheDataReaderPostgreSQL["numero_documento"].ToString().Trim();
                            Fila["razon_social"] = TheDataReaderPostgreSQL["razon_social"].ToString().Trim();
                            Fila["inscrito_rit"] = TheDataReaderPostgreSQL["inscrito_rit"].ToString().Trim();
                            Fila["id_municipio"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["direccion_cliente"] = TheDataReaderPostgreSQL["direccion_cliente"].ToString().Trim();
                            Fila["nombre_contacto"] = TheDataReaderPostgreSQL["nombre_contacto"].ToString().Trim();
                            Fila["telefono_contacto"] = TheDataReaderPostgreSQL["telefono_contacto"].ToString().Trim();
                            Fila["email_contacto"] = TheDataReaderPostgreSQL["email_contacto"].ToString().Trim();
                            Fila["numero_puntos"] = Convert.ToInt32(TheDataReaderPostgreSQL["numero_puntos"].ToString().Trim());
                            Fila["tiene_presencia_nacional"] = TheDataReaderPostgreSQL["tiene_presencia_nacional"].ToString().Trim();
                            Fila["consorcio_union_temporal"] = TheDataReaderPostgreSQL["consorcio_union_temporal"].ToString().Trim();
                            Fila["actividad_patrim_autonomo"] = TheDataReaderPostgreSQL["actividad_patrim_autonomo"].ToString().Trim();
                            //Fila["numero_placa_municipal"] = TheDataReaderPostgreSQL["numero_placa_municipal"].ToString().Trim();
                            //Fila["numero_matricula_ic"] = TheDataReaderPostgreSQL["numero_matricula_ic"].ToString().Trim();
                            //Fila["numero_rit"] = TheDataReaderPostgreSQL["numero_rit"].ToString().Trim();
                            //Fila["avisos_tablero"] = TheDataReaderPostgreSQL["avisos_tablero"].ToString().Trim();
                            //Fila["fecha_inicio_actividades"] = TheDataReaderPostgreSQL["fecha_inicio_actividades"].ToString().Trim();
                            Fila["id_estado"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_cliente]. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
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

        public DataTable GetInfoCliente()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtClientes";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_cliente", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINIR COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("id_cliente", typeof(Int32));
                    TablaDatos.Columns.Add("idtipo_clasificacion", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_clasificacion");
                    TablaDatos.Columns.Add("idtipo_identificacion", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_identificacion");
                    TablaDatos.Columns.Add("numero_documento");
                    TablaDatos.Columns.Add("digito_verificacion");
                    TablaDatos.Columns.Add("razon_social");
                    TablaDatos.Columns.Add("inscrito_rit");
                    TablaDatos.Columns.Add("id_dpto", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("direccion_cliente");
                    TablaDatos.Columns.Add("nombre_contacto");
                    TablaDatos.Columns.Add("telefono_contacto");
                    TablaDatos.Columns.Add("email_contacto");
                    TablaDatos.Columns.Add("numero_puntos", typeof(Int32));
                    TablaDatos.Columns.Add("tiene_presencia_nacional");
                    TablaDatos.Columns.Add("consorcio_union_temporal");
                    TablaDatos.Columns.Add("actividad_patrim_autonomo");
                    TablaDatos.Columns.Add("gran_contribuyente");
                    //TablaDatos.Columns.Add("numero_placa_municipal");
                    //TablaDatos.Columns.Add("numero_matricula_ic");
                    //TablaDatos.Columns.Add("numero_rit");
                    //TablaDatos.Columns.Add("avisos_tablero");
                    //TablaDatos.Columns.Add("fecha_inicio_actividades");
                    TablaDatos.Columns.Add("idtipo_sector", typeof(Int32));
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DEL DATAREADER 
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_cliente"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_cliente"].ToString().Trim());
                            Fila["idtipo_clasificacion"] = Convert.ToInt32(TheDataReaderPostgreSQL["idtipo_clasificacion"].ToString().Trim());
                            Fila["tipo_clasificacion"] = TheDataReaderPostgreSQL["tipo_clasificacion"].ToString().Trim();
                            Fila["idtipo_identificacion"] = Convert.ToInt32(TheDataReaderPostgreSQL["idtipo_identificacion"].ToString().Trim());
                            Fila["tipo_identificacion"] = TheDataReaderPostgreSQL["tipo_identificacion"].ToString().Trim();
                            Fila["numero_documento"] = TheDataReaderPostgreSQL["numero_documento"].ToString().Trim();
                            Fila["digito_verificacion"] = TheDataReaderPostgreSQL["digito_verificacion"].ToString().Trim();
                            Fila["razon_social"] = TheDataReaderPostgreSQL["razon_social"].ToString().Trim();
                            Fila["inscrito_rit"] = TheDataReaderPostgreSQL["inscrito_rit"].ToString().Trim();
                            Fila["id_dpto"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_dpto"].ToString().Trim());
                            Fila["nombre_departamento"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["direccion_cliente"] = TheDataReaderPostgreSQL["direccion_cliente"].ToString().Trim();
                            Fila["nombre_contacto"] = TheDataReaderPostgreSQL["nombre_contacto"].ToString().Trim();
                            Fila["telefono_contacto"] = TheDataReaderPostgreSQL["telefono_contacto"].ToString().Trim();
                            Fila["email_contacto"] = TheDataReaderPostgreSQL["email_contacto"].ToString().Trim();
                            Fila["numero_puntos"] = Convert.ToInt32(TheDataReaderPostgreSQL["numero_puntos"].ToString().Trim());
                            Fila["tiene_presencia_nacional"] = TheDataReaderPostgreSQL["tiene_presencia_nacional"].ToString().Trim();
                            Fila["consorcio_union_temporal"] = TheDataReaderPostgreSQL["consorcio_union_temporal"].ToString().Trim();
                            Fila["actividad_patrim_autonomo"] = TheDataReaderPostgreSQL["actividad_patrim_autonomo"].ToString().Trim();
                            Fila["gran_contribuyente"] = TheDataReaderPostgreSQL["gran_contribuyente"].ToString().Trim();
                            //Fila["numero_placa_municipal"] = TheDataReaderPostgreSQL["numero_placa_municipal"].ToString().Trim();
                            //Fila["numero_matricula_ic"] = TheDataReaderPostgreSQL["numero_matricula_ic"].ToString().Trim();
                            //Fila["numero_rit"] = TheDataReaderPostgreSQL["numero_rit"].ToString().Trim();
                            //Fila["avisos_tablero"] = TheDataReaderPostgreSQL["avisos_tablero"].ToString().Trim();
                            //Fila["fecha_inicio_actividades"] = TheDataReaderPostgreSQL["fecha_inicio_actividades"].ToString().Trim();
                            Fila["idtipo_sector"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_sector"].ToString().Trim());
                            Fila["id_estado"] = Int32.Parse(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_cliente]. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
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

        public DataTable GetClientes()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtClientes";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_cliente", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("id_cliente");
                    TablaDatos.Columns.Add("nombre_cliente");

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
                            Fila["id_cliente"] = TheDataReaderPostgreSQL["id_cliente"].ToString().Trim();
                            Fila["nombre_cliente"] = TheDataReaderPostgreSQL["razon_social"].ToString().Trim();
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
                    TheCommandSQLServer.CommandText = "sp_web_get_cliente";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    //Limpiar parametros
                    TheCommandSQLServer.Parameters.Clear();

                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    TablaDatos.Columns.Add("id_cliente");
                    TablaDatos.Columns.Add("razon_social");

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
                            Fila["id_cliente"] = TheDataReaderSQLServer["id_cliente"].ToString().Trim();
                            Fila["razon_social"] = TheDataReaderSQLServer["razon_social"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_cliente]. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
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

        public bool AddUpCliente(ref int _IdRegistro, ref string _MsgError)
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
                    #region REGISTRAR DATO CON EL SP EN LA DB POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_cliente", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_clasificacion", IdTipoClasificacion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_identificacion", IdTipoIdentificacion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_numero_documento", NumeroDocumento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_dig_verificacion", DigitoVerificacion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_razon_social", RazonSocial);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_inscrito_rit", InscritoRit);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmun_ubicacion", IdMunUbicacionPrincipal);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_direccion_cliente", DireccionCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_nombre_contacto", NombreContacto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_telefono_contacto", TelefonoContacto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_email_contacto", EmailContacto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_numero_puntos", NumeroPuntos);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_presencia_nacional", TienePresenciaNacional);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_consorcio_union_tem", ConsorcioUnionTemporal);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_actividad_patrim_aut", ActividadPatrimAutonomo);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_gran_contribuyente", GranContribuyente);
                    //TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_numero_placa_municipal", NumeroPlacaMunicipal);
                    //TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_numero_matricula_ic", NumeroMatriculaIc);
                    //TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_numero_rit", NumeroRit);
                    //TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_avisos_tablero", AvisosTablero);
                    //TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_finicio_actividades", FechaInicioActividades);
                    string _LoginUser = NumeroDocumento + DigitoVerificacion;
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_login_usuario", _LoginUser);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_password_usuario", ObjUtils.GetHashPassword(_LoginUser, PasswordUsuario));
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_sector", IdTipoSector);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa", IdEmpresa);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa_padre", IdEmpresaPadre);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
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
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //--Base de datos Mysql
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //--Base de datos Oracle
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
                _MsgError = "Error al registrar el cliente. Motivo: " + ex.Message.ToString().Trim();
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
        
        public int GetDigitoVerificacion(string _NumeroDocumento)
        {
            int digito = 0, acum = 0, residuo = 0;
            char[] nit_array = _NumeroDocumento.ToCharArray();
            int[] primos = { 3, 7, 13, 17, 19, 23, 29, 37, 41, 43, 47, 53, 39, 67, 71 };
            int max = nit_array.Length;

            for (int i = 0; i < _NumeroDocumento.Length; i++)
            {
                acum += Int32.Parse((nit_array[max - 1]).ToString()) * primos[i];
                max--;
            }

            residuo = acum % 11;
            if (residuo > 1)
            {
                digito = 11 - residuo;
            }
            else
            {
                digito = residuo;
            }

            return digito;
        }

    }
}