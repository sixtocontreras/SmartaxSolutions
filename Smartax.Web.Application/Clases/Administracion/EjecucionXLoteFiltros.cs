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
    public class EjecucionXLoteFiltros
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
        IDbTransaction Transac = null;

        SqlCommand TheCommandSQLServer = null;
        SqlDataReader TheDataReaderSQLServer = null;

        OracleCommand TheCommandOracle = null;
        OracleDataReader TheDataReaderOracle = null;
        #endregion

        #region DEFINICION DE ATRIBUTOS Y PROPIEDADES
        public object IdEjecucionLoteFiltro { get; set; }
        public object IdLiquidacionLote { get; set; }
        public object IdEjecucionLote { get; set; }
        public object AnioGravable { get; set; }
        public object IdFormImpuesto { get; set; }
        public object IdDepartamento { get; set; }
        public object IdMunicipio { get; set; }
        public object Observacion { get; set; }
        public object IdEstado { get; set; }
        public int IdEmpresa { get; set; }
        public int IdRol { get; set; }
        public int IdUsuario { get; set; }
        public string MesDeclararion { get; set; }
        public string AprobacionJefe { get; set; }
        public string MostrarSeleccione { get; set; }
        public string MotorBaseDatos { get; set; }
        public object ArrayData { get; set; }
        public int TipoConsulta { get; set; }
        public int TipoProceso { get; set; }
        #endregion

        public DataTable GetAllEjecucionXLoteFiltro()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtEjecucionXLoteFiltro";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_ejecucion_lote_filtros", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idejecucion_lote", IdEjecucionLote);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_iddepartamento", IdDepartamento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mes", MesDeclararion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_aprobacion_jefe", AprobacionJefe);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("idejec_lote_filtro", typeof(Int32));
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("descripcion_formulario");
                    TablaDatos.Columns.Add("id_dpto", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_dpto");
                    TablaDatos.Columns.Add("id_municipio");
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("idestado_declaracion");
                    TablaDatos.Columns.Add("estado_declaracion");
                    TablaDatos.Columns.Add("idestado_filtro", typeof(Int32));
                    TablaDatos.Columns.Add("estado_filtro");
                    TablaDatos.Columns.Add("fecha_registro");
                    TablaDatos.Columns.Add("fecha_actualizacion");

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idejec_lote_filtro"] = Convert.ToInt32(TheDataReaderPostgreSQL["idejec_lote_filtro"].ToString().Trim());
                            Fila["idformulario_impuesto"] = Convert.ToInt32(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["descripcion_formulario"] = TheDataReaderPostgreSQL["descripcion_formulario"].ToString().Trim();

                            Fila["id_dpto"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_dpto"].ToString().Trim());
                            Fila["nombre_dpto"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = TheDataReaderPostgreSQL["id_municipio"].ToString().Trim();
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();

                            Fila["idestado_declaracion"] = TheDataReaderPostgreSQL["idestado_declaracion"].ToString().Trim();
                            Fila["estado_declaracion"] = TheDataReaderPostgreSQL["estado_declaracion"].ToString().Trim();
                            Fila["idestado_filtro"] = Convert.ToInt32(TheDataReaderPostgreSQL["idestado_filtro"].ToString().Trim());
                            Fila["estado_filtro"] = TheDataReaderPostgreSQL["estado_filtro"].ToString().Trim();

                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            Fila["fecha_actualizacion"] = TheDataReaderPostgreSQL["fecha_actualizacion"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_ejecucion_lote]. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    if (TheDataReaderPostgreSQL != null)
                    {
                        TheDataReaderPostgreSQL.Close();
                    }
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

        public DataTable GetEjecucionXLoteFiltroTipoImp()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtEjecucionXLoteFiltro";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_ejecucion_lote_filtros", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idejecucion_lote", IdEjecucionLote);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_iddepartamento", IdDepartamento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mes", MesDeclararion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_aprobacion_jefe", AprobacionJefe);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("idformulario_impuesto");
                    TablaDatos.Columns.Add("descripcion_formulario");

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
                            Fila["idformulario_impuesto"] = TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim();
                            Fila["descripcion_formulario"] = TheDataReaderPostgreSQL["descripcion_formulario"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_ejecucion_lote]. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    if (TheDataReaderPostgreSQL != null)
                    {
                        TheDataReaderPostgreSQL.Close();
                    }
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

        public DataTable GetEjecucionXLoteFiltroDpto()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtEjecucionXLoteFiltro";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_ejecucion_lote_filtros", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idejecucion_lote", IdEjecucionLote);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_iddepartamento", IdDepartamento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mes", MesDeclararion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_aprobacion_jefe", AprobacionJefe);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("id_dpto");
                    TablaDatos.Columns.Add("nombre_dpto");

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
                            Fila["id_dpto"] = TheDataReaderPostgreSQL["id_dpto"].ToString().Trim();
                            Fila["nombre_dpto"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_ejecucion_lote]. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    if (TheDataReaderPostgreSQL != null)
                    {
                        TheDataReaderPostgreSQL.Close();
                    }
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

        public DataTable GetEjecucionXLoteFiltroMunicipio(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtEjecucionXLoteFiltro";
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
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_ejecucion_lote_filtros", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idejecucion_lote", IdEjecucionLote);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_iddepartamento", IdDepartamento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mes", MesDeclararion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_aprobacion_jefe", AprobacionJefe);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region AQUI DEFINIMOS LAS COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("id_registro", typeof(Int32));
                    TablaDatos.Columns.Add("id_cliente", typeof(Int32));
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("anio_gravable", typeof(Int32));
                    TablaDatos.Columns.Add("id_dpto", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_dpto");
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("idcliente_establecimiento", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("idestado_declaracion");
                    TablaDatos.Columns.Add("estado_declaracion");
                    TablaDatos.Columns.Add("estado_revision");
                    TablaDatos.Columns.Add("estado_aprobacion");
                    TablaDatos.Columns.Add("idliquidacion_lote");
                    TablaDatos.Columns.Add("idperiodicidad_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("idperiodicidad_pago", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_periodicidad");
                    TablaDatos.Columns.Add("periodicidad_impuesto");
                    TablaDatos.Columns.Add("vencida");
                    //TablaDatos.Columns.Add("idmun_calendario_trib", typeof(Int32));
                    TablaDatos.Columns.Add("fecha_limite");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DE LA DB DE POSTGRESQL
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_registro"] = TablaDatos.Rows.Count + 1;
                            Fila["id_cliente"] = Int32.Parse(TheDataReaderPostgreSQL["id_cliente"].ToString().Trim());
                            Fila["idformulario_impuesto"] = IdFormImpuesto;
                            Fila["anio_gravable"] = AnioGravable;
                            //Fila["id_registro"] = Int32.Parse(TheDataReaderPostgreSQL["numero_registro"].ToString().Trim());
                            Fila["id_dpto"] = Int32.Parse(TheDataReaderPostgreSQL["id_dpto"].ToString().Trim());
                            Fila["nombre_dpto"] = TheDataReaderPostgreSQL["nombre_dpto"].ToString().Trim();
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["idcliente_establecimiento"] = Int32.Parse(TheDataReaderPostgreSQL["idcliente_establecimiento"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();

                            //--DATOS DEL ESTADO DE LA DECLARACION
                            Fila["idestado_declaracion"] = TheDataReaderPostgreSQL["idestado_declaracion"].ToString().Trim();
                            Fila["estado_declaracion"] = TheDataReaderPostgreSQL["estado_declaracion"].ToString().Trim();
                            Fila["estado_revision"] = TheDataReaderPostgreSQL["estado_revision"].ToString().Trim();
                            Fila["estado_aprobacion"] = TheDataReaderPostgreSQL["estado_aprobacion"].ToString().Trim();

                            //--DATOS DE PERIODICIDAD DEL IMPUESTO
                            Fila["idliquidacion_lote"] = TheDataReaderPostgreSQL["idliquidacion_lote"].ToString().Trim();
                            Fila["idperiodicidad_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idperiodicidad_impuesto"].ToString().Trim());
                            Fila["idperiodicidad_pago"] = Int32.Parse(TheDataReaderPostgreSQL["idperiodicidad_pago"].ToString().Trim());
                            Fila["codigo_periodicidad"] = TheDataReaderPostgreSQL["codigo_periodicidad"].ToString().Trim();
                            Fila["periodicidad_impuesto"] = TheDataReaderPostgreSQL["periodicidad_impuesto"].ToString().Trim();

                            //--AQUI OBTENEMOS LA FECHA LIMITE PARA DEFINIR EL ESTADO DE VENCIMIENTO
                            string _FechaLimite1 = TheDataReaderPostgreSQL["fecha_limite"].ToString().Trim();
                            DateTime _FechaLimite = _FechaLimite1.ToString().Trim().Length > 0 ? DateTime.Parse(_FechaLimite1) : DateTime.Now;
                            DateTime _FechaActual = DateTime.Now;
                            //--
                            if (_FechaLimite < _FechaActual)
                            {
                                Fila["vencida"] = "SI";
                            }
                            else
                            {
                                Fila["vencida"] = "NO";
                            }

                            //Fila["idmun_calendario_trib"] = Int32.Parse(TheDataReaderPostgreSQL["idmun_calendario_trib"].ToString().Trim());
                            Fila["fecha_limite"] = _FechaLimite.ToString("dd-MM-yyyy");
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                        _MsgError = "";
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
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                _MsgError = "Error al obtener los datos de la Tabla [tbl_ejecucion_lote]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    if (TheDataReaderPostgreSQL != null)
                    {
                        TheDataReaderPostgreSQL.Close();
                    }
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

        public DataTable GetValidarLiquidacionXLote(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtLiquidacionXLote";
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
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_liquidacion_lote", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idliquidacion_lote", IdLiquidacionLote);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idejecucion_lote", IdEjecucionLote);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_iddepartamento", IdDepartamento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINIR COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idliquidacion_lote", typeof(Int32));
                    TablaDatos.Columns.Add("id_cliente", typeof(Int32));
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_impuesto");
                    TablaDatos.Columns.Add("anio_gravable", typeof(Int32));
                    TablaDatos.Columns.Add("id_dpto", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_dpto");
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("idcliente_establecimiento", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("idperiodicidad_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("periodo_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_periodicidad", typeof(Int32));
                    TablaDatos.Columns.Add("periodicidad_impuesto");
                    TablaDatos.Columns.Add("idestado_liquidacion", typeof(Int32));
                    TablaDatos.Columns.Add("estado_liquidacion");
                    TablaDatos.Columns.Add("revision_jefe");
                    TablaDatos.Columns.Add("aprobacion_jefe");
                    TablaDatos.Columns.Add("vencida");
                    TablaDatos.Columns.Add("fecha_limite");
                    TablaDatos.Columns.Add("id_usuario", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_analista");
                    TablaDatos.Columns.Add("email_analista");
                    TablaDatos.Columns.Add("fecha_registro");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DE DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            //Fila["id_registro"] = TablaDatos.Rows.Count + 1;
                            Fila["idliquidacion_lote"] = Int32.Parse(TheDataReaderPostgreSQL["idliquidacion_lote"].ToString().Trim());
                            Fila["id_cliente"] = Int32.Parse(TheDataReaderPostgreSQL["id_cliente"].ToString().Trim());
                            Fila["idformulario_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["tipo_impuesto"] = TheDataReaderPostgreSQL["tipo_impuesto"].ToString().Trim();
                            Fila["anio_gravable"] = Int32.Parse(TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim());
                            Fila["id_dpto"] = Int32.Parse(TheDataReaderPostgreSQL["id_dpto"].ToString().Trim());
                            Fila["nombre_dpto"] = TheDataReaderPostgreSQL["nombre_dpto"].ToString().Trim();
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["idcliente_establecimiento"] = Int32.Parse(TheDataReaderPostgreSQL["idcliente_establecimiento"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            //--
                            Fila["idperiodicidad_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idperiodicidad_impuesto"].ToString().Trim());
                            Fila["periodo_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["periodo_impuesto"].ToString().Trim());
                            Fila["codigo_periodicidad"] = Int32.Parse(TheDataReaderPostgreSQL["codigo_periodicidad"].ToString().Trim());
                            Fila["periodicidad_impuesto"] = TheDataReaderPostgreSQL["periodicidad_impuesto"].ToString().Trim();
                            //--
                            Fila["idestado_liquidacion"] = Int32.Parse(TheDataReaderPostgreSQL["idestado_liquidacion"].ToString().Trim());
                            Fila["estado_liquidacion"] = TheDataReaderPostgreSQL["estado_liquidacion"].ToString().Trim();
                            Fila["revision_jefe"] = TheDataReaderPostgreSQL["revision_jefe"].ToString().Trim().Equals("S") ? "SI" : "NO";
                            //--
                            string _AprobacionJefe = TheDataReaderPostgreSQL["aprobacion_jefe"].ToString().Trim();
                            switch (_AprobacionJefe)
                            {
                                case "P":
                                    Fila["aprobacion_jefe"] = "PENDIENTE";
                                    break;
                                case "S":
                                    Fila["aprobacion_jefe"] = "SI";
                                    break;
                                case "N":
                                    Fila["aprobacion_jefe"] = "NO";
                                    break;
                                default:
                                    Fila["aprobacion_jefe"] = "PENDIENTE";
                                    break;
                            }

                            //--AQUI OBTENEMOS LA FECHA LIMITE PARA DEFINIR EL ESTADO DE VENCIMIENTO
                            string _FechaLimite1 = TheDataReaderPostgreSQL["fecha_limite"].ToString().Trim();
                            DateTime _FechaLimite = _FechaLimite1.ToString().Trim().Length > 0 ? DateTime.Parse(_FechaLimite1) : DateTime.Now;
                            DateTime _FechaActual = DateTime.Now;
                            //--
                            if (_FechaLimite < _FechaActual)
                            {
                                Fila["vencida"] = "SI";
                            }
                            else
                            {
                                Fila["vencida"] = "NO";
                            }

                            Fila["fecha_limite"] = _FechaLimite1;

                            Fila["id_usuario"] = Int32.Parse(TheDataReaderPostgreSQL["id_usuario"].ToString().Trim());
                            Fila["nombre_analista"] = TheDataReaderPostgreSQL["nombre_analista"].ToString().Trim();
                            Fila["email_analista"] = TheDataReaderPostgreSQL["email_analista"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                        _MsgError = "";
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
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                _MsgError = "Error al obtener los datos de la Tabla [tbl_ejecucion_lote]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    if (TheDataReaderPostgreSQL != null)
                    {
                        TheDataReaderPostgreSQL.Close();
                    }
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

        public DataTable GetInfoValidacionLiquidacionXLote(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtLiquidacionXLote";
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
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_liquidacion_lote", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idliquidacion_lote", IdLiquidacionLote);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idejecucion_lote", IdEjecucionLote);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_iddepartamento", IdDepartamento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINIR COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idliquidacion_lote", typeof(Int32));
                    TablaDatos.Columns.Add("id_cliente", typeof(Int32));
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_impuesto");
                    TablaDatos.Columns.Add("anio_gravable", typeof(Int32));
                    TablaDatos.Columns.Add("id_dpto", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_dpto");
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("idcliente_establecimiento", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("idperiodicidad_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("periodo_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_periodicidad", typeof(Int32));
                    TablaDatos.Columns.Add("periodicidad_impuesto");
                    TablaDatos.Columns.Add("idestado_liquidacion", typeof(Int32));
                    TablaDatos.Columns.Add("estado_liquidacion");
                    TablaDatos.Columns.Add("revision_jefe");
                    TablaDatos.Columns.Add("aprobacion_jefe");
                    TablaDatos.Columns.Add("vencida");
                    TablaDatos.Columns.Add("fecha_limite");
                    TablaDatos.Columns.Add("id_usuario", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_analista");
                    TablaDatos.Columns.Add("email_analista");
                    TablaDatos.Columns.Add("observacion_validacion");
                    TablaDatos.Columns.Add("usuario_valida");
                    TablaDatos.Columns.Add("email_valida");
                    TablaDatos.Columns.Add("fecha_validacion");
                    TablaDatos.Columns.Add("fecha_registro");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DE DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            //Fila["id_registro"] = TablaDatos.Rows.Count + 1;
                            Fila["idliquidacion_lote"] = Int32.Parse(TheDataReaderPostgreSQL["idliquidacion_lote"].ToString().Trim());
                            Fila["id_cliente"] = Int32.Parse(TheDataReaderPostgreSQL["id_cliente"].ToString().Trim());
                            Fila["idformulario_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["tipo_impuesto"] = TheDataReaderPostgreSQL["tipo_impuesto"].ToString().Trim();
                            Fila["anio_gravable"] = Int32.Parse(TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim());
                            Fila["id_dpto"] = Int32.Parse(TheDataReaderPostgreSQL["id_dpto"].ToString().Trim());
                            Fila["nombre_dpto"] = TheDataReaderPostgreSQL["nombre_dpto"].ToString().Trim();
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["idcliente_establecimiento"] = Int32.Parse(TheDataReaderPostgreSQL["idcliente_establecimiento"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            //--
                            Fila["idperiodicidad_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idperiodicidad_impuesto"].ToString().Trim());
                            Fila["periodo_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["periodo_impuesto"].ToString().Trim());
                            Fila["codigo_periodicidad"] = Int32.Parse(TheDataReaderPostgreSQL["codigo_periodicidad"].ToString().Trim());
                            Fila["periodicidad_impuesto"] = TheDataReaderPostgreSQL["periodicidad_impuesto"].ToString().Trim();
                            //--
                            Fila["idestado_liquidacion"] = Int32.Parse(TheDataReaderPostgreSQL["idestado_liquidacion"].ToString().Trim());
                            Fila["estado_liquidacion"] = TheDataReaderPostgreSQL["estado_liquidacion"].ToString().Trim();
                            Fila["revision_jefe"] = TheDataReaderPostgreSQL["revision_jefe"].ToString().Trim().Equals("S") ? "SI" : "NO";
                            //--
                            string _AprobacionJefe = TheDataReaderPostgreSQL["aprobacion_jefe"].ToString().Trim();
                            switch (_AprobacionJefe)
                            {
                                case "P":
                                    Fila["aprobacion_jefe"] = "PENDIENTE";
                                    break;
                                case "S":
                                    Fila["aprobacion_jefe"] = "SI";
                                    break;
                                case "N":
                                    Fila["aprobacion_jefe"] = "NO";
                                    break;
                                default:
                                    Fila["aprobacion_jefe"] = "PENDIENTE";
                                    break;
                            }

                            //--AQUI OBTENEMOS LA FECHA LIMITE PARA DEFINIR EL ESTADO DE VENCIMIENTO
                            string _FechaLimite1 = TheDataReaderPostgreSQL["fecha_limite"].ToString().Trim();
                            DateTime _FechaLimite = _FechaLimite1.ToString().Trim().Length > 0 ? DateTime.Parse(_FechaLimite1) : DateTime.Now;
                            DateTime _FechaActual = DateTime.Now;
                            //--
                            if (_FechaLimite < _FechaActual)
                            {
                                Fila["vencida"] = "SI";
                            }
                            else
                            {
                                Fila["vencida"] = "NO";
                            }

                            Fila["fecha_limite"] = _FechaLimite1;

                            Fila["id_usuario"] = Int32.Parse(TheDataReaderPostgreSQL["id_usuario"].ToString().Trim());
                            Fila["nombre_analista"] = TheDataReaderPostgreSQL["nombre_analista"].ToString().Trim();
                            Fila["email_analista"] = TheDataReaderPostgreSQL["email_analista"].ToString().Trim();
                            Fila["observacion_validacion"] = TheDataReaderPostgreSQL["observacion_validacion"].ToString().Trim();
                            Fila["usuario_valida"] = TheDataReaderPostgreSQL["usuario_valida"].ToString().Trim();
                            Fila["email_valida"] = TheDataReaderPostgreSQL["email_valida"].ToString().Trim();
                            Fila["fecha_validacion"] = TheDataReaderPostgreSQL["fecha_validacion"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                        _MsgError = "";
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
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                _MsgError = "Error al obtener los datos de la Tabla [tbl_ejecucion_lote]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    if (TheDataReaderPostgreSQL != null)
                    {
                        TheDataReaderPostgreSQL.Close();
                    }
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

        public DataTable GetEjecucionXLoteFiltroMunicipio_Old(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtEjecucionXLoteFiltro";
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
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_ejecucion_lote_filtros", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idejecucion_lote", IdEjecucionLote);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_iddepartamento", IdDepartamento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mes", MesDeclararion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_aprobacion_jefe", AprobacionJefe);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("id_municipio");
                    TablaDatos.Columns.Add("nombre_municipio");

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
                            Fila["id_municipio"] = TheDataReaderPostgreSQL["id_municipio"].ToString().Trim() + "|" + TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                        }
                        _MsgError = "";
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
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                _MsgError = "Error al obtener los datos de la Tabla [tbl_ejecucion_lote]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    if (TheDataReaderPostgreSQL != null)
                    {
                        TheDataReaderPostgreSQL.Close();
                    }
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

        public int GetValidacionEstadoDeclaracion()
        {
            int _IdEstadoDeclaracion = 0;
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
                    return 0;
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_ejecucion_lote_filtros", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idejecucion_lote", IdEjecucionLote);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_iddepartamento", IdDepartamento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mes", MesDeclararion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_aprobacion_jefe", AprobacionJefe);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            _IdEstadoDeclaracion = TheDataReaderPostgreSQL["idestado_declaracion"].ToString().Trim().Length > 0 ? Int32.Parse(TheDataReaderPostgreSQL["idestado_declaracion"].ToString().Trim()) : 2;
                            break;
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
                    return _IdEstadoDeclaracion;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al obtener los datos de la Tabla [sp_web_get_ejecucion_lote_filtros]. Motivo: " + ex.Message);
                return _IdEstadoDeclaracion;
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    if (TheDataReaderPostgreSQL != null)
                    {
                        TheDataReaderPostgreSQL.Close();
                    }
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

            return _IdEstadoDeclaracion;
        }

        public bool AddUpEjecucionXLoteFiltro(DataRow Fila, ref int _IdRegistro, ref string _MsgError)
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_ejecucion_lote_filtros", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idejec_lote_filtro", IdEjecucionLoteFiltro);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idejecucion_lote", IdEjecucionLote);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_iddpto", Fila["id_dpto"].ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", Fila["id_municipio"].ToString().Trim().Length > 0 ? Fila["id_municipio"].ToString().Trim() : null);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", Fila["id_estado"].ToString().Trim());
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
                _MsgError = "Error al registrar la ejecucion de lote por filtros. Motivo: " + ex.Message.ToString().Trim();
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

        public bool AddUpEjecucionXLoteFiltro(ref int _IdRegistro, ref string _MsgError)
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_ejecucion_lote_filtros", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idejec_lote_filtro", IdEjecucionLoteFiltro);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idejecucion_lote", IdEjecucionLote);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_iddpto", IdDepartamento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_observacion_aprob_rechazo", Observacion);
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
                _MsgError = "Error al registrar la ejecucion de lote por filtros. Motivo: " + ex.Message.ToString().Trim();
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

        public bool GetProcesarLiquidacionLote(ref int _IdRegistro, ref string _MsgError)
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_task_liquidacion_lote", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idliquidacion_lote", IdEjecucionLote);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_row_liquidacion", string.Format("{{{0}}}", string.Join(",", ArrayData)));
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_aprobacion_jefe", AprobacionJefe);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_observacion", Observacion);
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
                        if (Int32.Parse(_IdRegRetorno.Value.ToString()) > 0)
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
                _MsgError = "Error al cargar la liquidacion x lote. Motivo: " + ex.Message.ToString().Trim();
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
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                }

                myConnectionDb.Close();
                myConnectionDb.Dispose();
                #endregion
            }

            return retValor;
        }

    }
}