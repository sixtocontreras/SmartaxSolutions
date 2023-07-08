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

namespace Smartax.Web.Application.Clases.Modulos
{
    public class ModulosApp
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
        public object IdCliente { get; set; }
        public object IdMunicipio { get; set; }
        public string Anio { get; set; }
        public string Mes { get; set; }
        public object IdFormImpuesto { get; set; }
        public object IdEstado { get; set; }
        public string MotorBaseDatos { get; set; }
        public int TipoConsulta { get; set; }
        #endregion

        #region METODOS PARA EL MODULO PLANEACION FISCAL
        public DataTable GetCalendarioTributario()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtCalendarioTributario";
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
                    #region OBTENER DATOS DE LA BASE DE DATOS POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_calendario_tributario", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio", Anio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mes", Mes);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("descripcion_formulario");
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("anio_gravable");
                    TablaDatos.Columns.Add("periodicidad_impuesto");
                    TablaDatos.Columns.Add("dia_limite");
                    TablaDatos.Columns.Add("fecha_limite");
                    TablaDatos.Columns.Add("valor_descuento");
                    TablaDatos.Columns.Add("vencido");

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["descripcion_formulario"] = TheDataReaderPostgreSQL["descripcion_formulario"].ToString().Trim();
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["anio_gravable"] = TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim();
                            Fila["periodicidad_impuesto"] = TheDataReaderPostgreSQL["periodicidad_impuesto"].ToString().Trim();
                            Fila["dia_limite"] = TheDataReaderPostgreSQL["dia_limite"].ToString().Trim();

                            //--AQUI OBTENEMOS LA FECHA LIMITE PARA DEFINIR EL ESTADO DE VENCIMIENTO
                            string _FechaLimite1 = TheDataReaderPostgreSQL["fecha_limite"].ToString().Trim();
                            DateTime _FechaLimite = _FechaLimite1.ToString().Trim().Length > 0 ? DateTime.Parse(_FechaLimite1) : DateTime.Now;
                            DateTime _FechaActual = DateTime.Now;
                            //--
                            if (_FechaLimite.Date < _FechaActual.Date)
                            {
                                Fila["vencido"] = "S";
                            }
                            else if (_FechaLimite.Date == _FechaActual.Date)
                            {
                                Fila["vencido"] = "P";
                            }
                            else
                            {
                                Fila["vencido"] = "N";
                            }

                            //Fila["idmun_calendario_trib"] = Int32.Parse(TheDataReaderPostgreSQL["idmun_calendario_trib"].ToString().Trim());
                            Fila["fecha_limite"] = _FechaLimite.ToString("dd-MM-yyyy");
                            //Fila["fecha_limite"] = TheDataReaderPostgreSQL["fecha_limite"].ToString().Trim();

                            Fila["valor_descuento"] = TheDataReaderPostgreSQL["valor_descuento"].ToString().Trim() + "%";
                            //Fila["vencido"] = TheDataReaderPostgreSQL["vencidos"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_tipo_menu]. Motivo: " + ex.Message);
            }
            finally
            {
                #region OBJETO DE FINALIZACION CONEXION A LA DB
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
                    TheDataReaderOracle = null;
                }
                myConnectionDb.Close();
                myConnectionDb.Dispose();
                #endregion
            }

            return TablaDatos;
        }

        public DataTable GetTarifasExcesivas()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtTarifasExcesivas";
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
                    #region OBTENER DATOS DE LA BASE DE DATOS POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_planeacion_fiscal", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("idmun_act_economica", typeof(Int32));
                    TablaDatos.Columns.Add("anio_actividad");
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("formulario_impuesto");
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("tipo_actividad");
                    TablaDatos.Columns.Add("codigo_actividad");
                    TablaDatos.Columns.Add("descripcion_actividad");
                    TablaDatos.Columns.Add("tarifa_ley");
                    TablaDatos.Columns.Add("tarifa_municipio");
                    TablaDatos.Columns.Add("numero_acuerdo");
                    TablaDatos.Columns.Add("numero_articulo");

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idmun_act_economica"] = Int32.Parse(TheDataReaderPostgreSQL["idmun_act_economica"].ToString().Trim());
                            Fila["anio_actividad"] = TheDataReaderPostgreSQL["anio_actividad"].ToString().Trim();
                            Fila["idformulario_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["formulario_impuesto"] = TheDataReaderPostgreSQL["formulario_impuesto"].ToString().Trim();
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["tipo_actividad"] = TheDataReaderPostgreSQL["tipo_actividad"].ToString().Trim();
                            Fila["codigo_actividad"] = TheDataReaderPostgreSQL["codigo_actividad"].ToString().Trim();
                            Fila["descripcion_actividad"] = TheDataReaderPostgreSQL["descripcion_actividad"].ToString().Trim();
                            Fila["tarifa_ley"] = TheDataReaderPostgreSQL["tarifa_ley"].ToString().Trim();
                            Fila["tarifa_municipio"] = TheDataReaderPostgreSQL["tarifa_municipio"].ToString().Trim();
                            Fila["numero_acuerdo"] = TheDataReaderPostgreSQL["numero_acuerdo"].ToString().Trim();
                            Fila["numero_articulo"] = TheDataReaderPostgreSQL["numero_articulo"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_tipo_menu]. Motivo: " + ex.Message);
            }
            finally
            {
                #region OBJETO DE FINALIZACION CONEXION A LA DB
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
                    TheDataReaderOracle = null;
                }
                myConnectionDb.Close();
                myConnectionDb.Dispose();
                #endregion
            }

            return TablaDatos;
        }
        #endregion

    }
}