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
using System.Collections.Generic;

namespace Smartax.Web.Application.Clases.Modulos
{
    public class ConsultaLiqImpuesto
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
        public object NumeroRenglon { get; set; }
        public object IdCliente { get; set; }
        public object IdClienteEstablecimiento { get; set; }
        public object IdFormularioImpuesto { get; set; }
        public object CodigoDane { get; set; }
        public object CodigoCuenta { get; set; }
        public object IdMunCalendarioTrib { get; set; }
        public object IdMunicipio { get; set; }
        public object AnioGravable { get; set; }
        public object IdPeriodicidad { get; set; }
        public object PeriodicidadImpuesto { get; set; }
        public object CodigoImpuesto { get; set; }
        public object MesEf { get; set; }
        public string FechaInicial { get; set; }
        public string SaldoInicial { get; set; }
        public string MovDebito { get; set; }
        public string MovCredito { get; set; }
        public string SaldoFinal { get; set; }
        public string MotorBaseDatos { get; set; }
        public object IdEstado { get; set; }
        public int TipoConsulta { get; set; }
        public int TipoImpuesto { get; set; }
        #endregion

        public DataTable GetConsultarDatos()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtConsultarDatos";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_consulta_liq_impuesto", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_impuesto", TipoImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_codigo_dane", CodigoDane);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE CAMPOS
                    TablaDatos.Columns.Add("idcliente_establecimiento", typeof(Int32));
                    TablaDatos.Columns.Add("idmun_oficina", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("municipio_oficina");
                    TablaDatos.Columns.Add("iddpto_oficina", typeof(Int32));
                    TablaDatos.Columns.Add("dpto_oficina");
                    TablaDatos.Columns.Add("numero_puntos", typeof(Int32));
                    TablaDatos.Columns.Add("id_cliente", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_cliente");
                    TablaDatos.Columns.Add("idtipo_identificacion", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_identificacion");
                    TablaDatos.Columns.Add("numero_documento");
                    TablaDatos.Columns.Add("digito_verificacion");
                    TablaDatos.Columns.Add("consorcio_union_temporal");
                    TablaDatos.Columns.Add("actividad_patrim_autonomo");
                    TablaDatos.Columns.Add("idmun_cliente", typeof(Int32));
                    TablaDatos.Columns.Add("municipio_cliente");
                    TablaDatos.Columns.Add("iddpto_cliente", typeof(Int32));
                    TablaDatos.Columns.Add("dpto_cliente");
                    TablaDatos.Columns.Add("direccion_cliente");
                    TablaDatos.Columns.Add("telefono_contacto");
                    TablaDatos.Columns.Add("email_contacto");
                    TablaDatos.Columns.Add("idtipo_clasificacion", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_clasificacion");
                    TablaDatos.Columns.Add("numero_placa_municipal");
                    TablaDatos.Columns.Add("numero_matricula_ic");
                    TablaDatos.Columns.Add("numero_rit");
                    TablaDatos.Columns.Add("avisos_tablero");
                    TablaDatos.Columns.Add("liquidacion_mixta");
                    TablaDatos.Columns.Add("fecha_inicio_actividades");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DE LA CONSULTA
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idcliente_establecimiento"] = Int32.Parse(TheDataReaderPostgreSQL["idcliente_establecimiento"].ToString().Trim());
                            Fila["idmun_oficina"] = Int32.Parse(TheDataReaderPostgreSQL["idmun_oficina"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["municipio_oficina"] = TheDataReaderPostgreSQL["municipio_oficina"].ToString().Trim();
                            Fila["iddpto_oficina"] = Int32.Parse(TheDataReaderPostgreSQL["iddpto_oficina"].ToString().Trim());
                            Fila["dpto_oficina"] = TheDataReaderPostgreSQL["dpto_oficina"].ToString().Trim();
                            Fila["numero_puntos"] = Int32.Parse(TheDataReaderPostgreSQL["numero_puntos"].ToString().Trim());
                            Fila["id_cliente"] = Int32.Parse(TheDataReaderPostgreSQL["id_cliente"].ToString().Trim());
                            Fila["nombre_cliente"] = TheDataReaderPostgreSQL["nombre_cliente"].ToString().Trim();
                            Fila["idtipo_identificacion"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_identificacion"].ToString().Trim());
                            Fila["tipo_identificacion"] = TheDataReaderPostgreSQL["tipo_identificacion"].ToString().Trim();
                            Fila["numero_documento"] = TheDataReaderPostgreSQL["numero_documento"].ToString().Trim();
                            Fila["digito_verificacion"] = TheDataReaderPostgreSQL["digito_verificacion"].ToString().Trim();
                            Fila["consorcio_union_temporal"] = TheDataReaderPostgreSQL["consorcio_union_temporal"].ToString().Trim();
                            Fila["actividad_patrim_autonomo"] = TheDataReaderPostgreSQL["actividad_patrim_autonomo"].ToString().Trim();
                            Fila["idmun_cliente"] = Int32.Parse(TheDataReaderPostgreSQL["idmun_cliente"].ToString().Trim());
                            Fila["municipio_cliente"] = TheDataReaderPostgreSQL["municipio_cliente"].ToString().Trim();
                            Fila["iddpto_cliente"] = Int32.Parse(TheDataReaderPostgreSQL["iddpto_cliente"].ToString().Trim());
                            Fila["dpto_cliente"] = TheDataReaderPostgreSQL["dpto_cliente"].ToString().Trim();
                            Fila["direccion_cliente"] = TheDataReaderPostgreSQL["direccion_cliente"].ToString().Trim();
                            Fila["telefono_contacto"] = TheDataReaderPostgreSQL["telefono_contacto"].ToString().Trim();
                            Fila["email_contacto"] = TheDataReaderPostgreSQL["email_contacto"].ToString().Trim();
                            Fila["idtipo_clasificacion"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_clasificacion"].ToString().Trim());
                            Fila["tipo_clasificacion"] = TheDataReaderPostgreSQL["tipo_clasificacion"].ToString().Trim();
                            Fila["numero_placa_municipal"] = TheDataReaderPostgreSQL["numero_placa_municipal"].ToString().Trim();
                            Fila["numero_matricula_ic"] = TheDataReaderPostgreSQL["numero_matricula_ic"].ToString().Trim();
                            Fila["numero_rit"] = TheDataReaderPostgreSQL["numero_rit"].ToString().Trim();
                            Fila["avisos_tablero"] = TheDataReaderPostgreSQL["avisos_tablero"].ToString().Trim();
                            Fila["liquidacion_mixta"] = TheDataReaderPostgreSQL["liquidacion_mixta"].ToString().Trim();
                            Fila["fecha_inicio_actividades"] = TheDataReaderPostgreSQL["fecha_inicio_actividades"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            break;
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
                _log.Error("Error al obtener los datos del municipio. Motivo: " + ex.Message);
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

        public DataTable GetBaseGravable()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtBaseGravable";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_consulta_liq_valores", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente_establecimiento", IdClienteEstablecimiento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idformulario_impuesto", TipoImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("numero_renglon", typeof(Int32));
                    TablaDatos.Columns.Add("valor_total");

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DE LA CONSULTA
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["numero_renglon"] = Int32.Parse(TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim());
                            Fila["valor_total"] = TheDataReaderPostgreSQL["valor_total"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    #endregion
                }
                else if (myConnectionDb is SqlConnection)
                {
                    #region OBTENER DATOS DE LA BASE DE DATOS DE SQL SERVER
                    //Para Base de Datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_get_consulta_act_economica";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;

                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idcliente_establecimiento", IdClienteEstablecimiento);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    #region DEFINICION DE CAMPOS
                    TablaDatos.Columns.Add("idestab_act_economica", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_actividad");
                    TablaDatos.Columns.Add("idtipo_tarifa", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_tarifa");
                    TablaDatos.Columns.Add("tarifa_ley", typeof(Double));
                    TablaDatos.Columns.Add("tarifa_municipio", typeof(Double));
                    #endregion

                    if (TheDataReaderSQLServer != null)
                    {
                        while (TheDataReaderSQLServer.Read())
                        {
                            #region OBTENER DATOS DE LA CONSULTA
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idestab_act_economica"] = Int32.Parse(TheDataReaderSQLServer["idestab_act_economica"].ToString().Trim());
                            Fila["codigo_actividad"] = TheDataReaderSQLServer["codigo_actividad"].ToString().Trim();
                            Fila["idtipo_tarifa"] = Int32.Parse(TheDataReaderSQLServer["idtipo_tarifa"].ToString().Trim());
                            Fila["tipo_tarifa"] = TheDataReaderSQLServer["descripcion_tarifa"].ToString().Trim();
                            Fila["tarifa_ley"] = Double.Parse(TheDataReaderSQLServer["tarifa_ley"].ToString().Trim());
                            Fila["tarifa_municipio"] = Double.Parse(TheDataReaderSQLServer["tarifa_municipio"].ToString().Trim());
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_cliente_estab_act_economica]. Motivo: " + ex.Message);
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

        public List<string> GetEstadoFinanciero()
        {
            List<string> _ArrayDatos = new List<string>();
            //DataTable TablaDatos = new DataTable();
            //TablaDatos.TableName = "DtEstadoFinanciero";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_estado_financiero", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_numero_renglon", NumeroRenglon);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente_estab", IdClienteEstablecimiento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mes_ef", MesEf);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_saldo_inicial", SaldoInicial);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mov_debito", MovDebito);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mov_credito", MovCredito);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_saldo_final", SaldoFinal);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_codigo_cuenta", CodigoCuenta);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    //TablaDatos.Columns.Add("codigo_cuenta");
                    //TablaDatos.Columns.Add("valor_ef", typeof(Double));
                    //TablaDatos.Columns.Add("mov_debito", typeof(Double));
                    //TablaDatos.Columns.Add("mov_credito", typeof(Double));
                    //TablaDatos.Columns.Add("saldo_final", typeof(Double));

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DE LA CONSULTA
                            //_ArrayDatos.Add(TheDataReaderPostgreSQL["valor_default"].ToString().Trim());
                            _ArrayDatos.Add(TheDataReaderPostgreSQL["codigo_cuenta"].ToString().Trim());

                            if (SaldoInicial.Equals("S"))
                            {
                                _ArrayDatos.Add(TheDataReaderPostgreSQL["saldo_inicial"].ToString().Trim().Replace("-", ""));
                            }
                            else if (MovDebito.Equals("S"))
                            {
                                _ArrayDatos.Add(TheDataReaderPostgreSQL["mov_debito"].ToString().Trim().Replace("-", ""));
                            }
                            else if (MovCredito.Equals("S"))
                            {
                                _ArrayDatos.Add(TheDataReaderPostgreSQL["mov_credito"].ToString().Trim().Replace("-", ""));
                            }
                            else if (SaldoFinal.Equals("S"))
                            {
                                _ArrayDatos.Add(TheDataReaderPostgreSQL["saldo_final"].ToString().Trim().Replace("-", ""));
                            }
                            else
                            {
                                _ArrayDatos.Add("0");
                            }

                            //Fila["mov_debito"] = Double.Parse(TheDataReaderPostgreSQL["mov_debito"].ToString().Trim());
                            //Fila["mov_credito"] = Double.Parse(TheDataReaderPostgreSQL["mov_credito"].ToString().Trim());
                            //Fila["saldo_final"] = Double.Parse(TheDataReaderPostgreSQL["saldo_final"].ToString().Trim());
                            //TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    #endregion
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL SERVER
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
                    return _ArrayDatos;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al obtener los datos de la Tabla [tbl_estado_financiero]. Motivo: " + ex.Message);
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

            return _ArrayDatos;
        }

        public DataTable GetImpuestosMunicipio()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtImpuestosMunicipio";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_consulta_imp_municipio", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region AQUI DEFINIMOS LAS COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("anio_gravable", typeof(Int32));
                    TablaDatos.Columns.Add("numero_renglon", typeof(Int32));
                    TablaDatos.Columns.Add("descripcion_renglon");
                    TablaDatos.Columns.Add("idtipo_tarifa", typeof(Int32));
                    TablaDatos.Columns.Add("valor_tarifa");
                    TablaDatos.Columns.Add("calcular_renglon");
                    TablaDatos.Columns.Add("numero_renglon1");
                    TablaDatos.Columns.Add("idtipo_operacion1");
                    TablaDatos.Columns.Add("numero_renglon2");
                    TablaDatos.Columns.Add("idtipo_operacion2");
                    TablaDatos.Columns.Add("numero_renglon3");
                    TablaDatos.Columns.Add("idtipo_operacion3");
                    TablaDatos.Columns.Add("numero_renglon4");
                    TablaDatos.Columns.Add("idtipo_operacion4");
                    TablaDatos.Columns.Add("numero_renglon5");
                    TablaDatos.Columns.Add("idtipo_operacion5");
                    TablaDatos.Columns.Add("numero_renglon6");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DE LA CONSULTA
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["anio_gravable"] = Int32.Parse(TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim());
                            Fila["numero_renglon"] = Int32.Parse(TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim());
                            Fila["descripcion_renglon"] = TheDataReaderPostgreSQL["descripcion_renglon"].ToString().Trim();
                            Fila["idtipo_tarifa"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_tarifa"].ToString().Trim());
                            Fila["valor_tarifa"] = TheDataReaderPostgreSQL["valor_tarifa"].ToString().Trim();
                            //--
                            Fila["calcular_renglon"] = TheDataReaderPostgreSQL["calcular_renglon"].ToString().Trim();
                            Fila["numero_renglon1"] = TheDataReaderPostgreSQL["numero_renglon1"].ToString().Trim();
                            Fila["idtipo_operacion1"] = TheDataReaderPostgreSQL["idtipo_operacion1"].ToString().Trim();
                            Fila["numero_renglon2"] = TheDataReaderPostgreSQL["numero_renglon2"].ToString().Trim();
                            Fila["idtipo_operacion2"] = TheDataReaderPostgreSQL["idtipo_operacion2"].ToString().Trim();
                            Fila["numero_renglon3"] = TheDataReaderPostgreSQL["numero_renglon3"].ToString().Trim();
                            Fila["idtipo_operacion3"] = TheDataReaderPostgreSQL["idtipo_operacion3"].ToString().Trim();
                            Fila["numero_renglon4"] = TheDataReaderPostgreSQL["numero_renglon4"].ToString().Trim();
                            Fila["idtipo_operacion4"] = TheDataReaderPostgreSQL["idtipo_operacion4"].ToString().Trim();
                            Fila["numero_renglon5"] = TheDataReaderPostgreSQL["numero_renglon5"].ToString().Trim();
                            Fila["idtipo_operacion5"] = TheDataReaderPostgreSQL["idtipo_operacion5"].ToString().Trim();
                            Fila["numero_renglon6"] = TheDataReaderPostgreSQL["numero_renglon6"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_cliente_estab_act_economica]. Motivo: " + ex.Message);
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

        public DataTable GetConsultarActEconomica()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtConsultarActEconomica";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_consulta_act_economica", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente_establecimiento", IdClienteEstablecimiento);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE CAMPOS
                    //TablaDatos.Columns.Add("idestab_act_economica", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_actividad");
                    TablaDatos.Columns.Add("codigo_cuenta");
                    TablaDatos.Columns.Add("idtipo_tarifa", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_tarifa");
                    TablaDatos.Columns.Add("idtipo_calculo", typeof(Int32));
                    TablaDatos.Columns.Add("tarifa_ley", typeof(Double));
                    TablaDatos.Columns.Add("tarifa_municipio", typeof(Double));
                    TablaDatos.Columns.Add("saldo_inicial", typeof(Double));
                    TablaDatos.Columns.Add("mov_debito", typeof(Double));
                    TablaDatos.Columns.Add("mov_credito", typeof(Double));
                    TablaDatos.Columns.Add("saldo_final", typeof(Double));
                    TablaDatos.Columns.Add("saldo_final2");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DE LA CONSULTA
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            //--
                            string _CodigoActividad = TheDataReaderPostgreSQL["codigo_actividad"].ToString().Trim();
                            DataRow[] dataRows = TablaDatos.Select("codigo_actividad = '" + _CodigoActividad + "'");
                            if (dataRows.Length == 1)
                            {
                                #region EDITAR LA ACTIVIDAD EN EL DATATABLE
                                double _SaldoInicial = Double.Parse(dataRows[0]["saldo_inicial"].ToString().Trim().Replace(".", ""));
                                double _SaldoInicial2 = TheDataReaderPostgreSQL["saldo_inicial"].ToString().Trim().Length > 0 ? Double.Parse(TheDataReaderPostgreSQL["saldo_inicial"].ToString().Trim()) : 0;
                                double _MovDebito = Double.Parse(dataRows[0]["mov_debito"].ToString().Trim());
                                double _MovDebito2 = TheDataReaderPostgreSQL["mov_debito"].ToString().Trim().Length > 0 ? Double.Parse(TheDataReaderPostgreSQL["mov_debito"].ToString().Trim()) : 0;
                                double _MovCredito = Double.Parse(dataRows[0]["mov_credito"].ToString().Trim());
                                double _MovCredito2 = TheDataReaderPostgreSQL["mov_credito"].ToString().Trim().Length > 0 ? Double.Parse(TheDataReaderPostgreSQL["mov_credito"].ToString().Trim()) : 0;
                                double _SaldoFinal = Double.Parse(dataRows[0]["saldo_final"].ToString().Trim());
                                double _SaldoFinal2 = TheDataReaderPostgreSQL["saldo_final"].ToString().Trim().Length > 0 ? Double.Parse(TheDataReaderPostgreSQL["saldo_final"].ToString().Trim()) : 0;
                                //--
                                dataRows[0]["saldo_inicial"] = _SaldoInicial + _SaldoInicial2;
                                dataRows[0]["mov_debito"] = _MovDebito + _MovDebito2;
                                dataRows[0]["mov_credito"] = _MovCredito + _MovCredito2;
                                dataRows[0]["saldo_final"] = _SaldoFinal + _SaldoFinal2;
                                TablaDatos.Rows[0].AcceptChanges();
                                TablaDatos.Rows[0].EndEdit();
                                #endregion
                            }
                            else
                            {
                                #region AGREGAR UNA NUEVA ACTIVIDAD AL DATATABLE
                                //Fila["idestab_act_economica"] = Int32.Parse(TheDataReaderPostgreSQL["idestab_act_economica"].ToString().Trim());
                                Fila["codigo_actividad"] = TheDataReaderPostgreSQL["codigo_actividad"].ToString().Trim();
                                Fila["codigo_cuenta"] = TheDataReaderPostgreSQL["codigo_cuenta"].ToString().Trim();

                                Fila["idtipo_tarifa"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_tarifa"].ToString().Trim());
                                Fila["tipo_tarifa"] = TheDataReaderPostgreSQL["descripcion_tarifa"].ToString().Trim();

                                Fila["idtipo_calculo"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_calculo"].ToString().Trim());
                                Fila["tarifa_ley"] = Double.Parse(TheDataReaderPostgreSQL["tarifa_ley"].ToString().Trim());
                                Fila["tarifa_municipio"] = Double.Parse(TheDataReaderPostgreSQL["tarifa_municipio"].ToString().Trim());

                                Fila["saldo_inicial"] = TheDataReaderPostgreSQL["saldo_inicial"].ToString().Trim().Length > 0 ? Double.Parse(TheDataReaderPostgreSQL["saldo_inicial"].ToString().Trim()) : 0;
                                Fila["mov_debito"] = TheDataReaderPostgreSQL["mov_debito"].ToString().Trim().Length > 0 ? Double.Parse(TheDataReaderPostgreSQL["mov_debito"].ToString().Trim()) : 0;
                                Fila["mov_credito"] = TheDataReaderPostgreSQL["mov_credito"].ToString().Trim().Length > 0 ? Double.Parse(TheDataReaderPostgreSQL["mov_credito"].ToString().Trim()) : 0;
                                double _SaldoFinal = TheDataReaderPostgreSQL["saldo_final"].ToString().Trim().Length > 0 ? Double.Parse(TheDataReaderPostgreSQL["saldo_final"].ToString().Trim()) : 0;
                                Fila["saldo_final"] = _SaldoFinal;
                                Fila["saldo_final2"] = String.Format(String.Format("{0:$ ###,###,##0}", _SaldoFinal));
                                TablaDatos.Rows.Add(Fila);
                                #endregion
                            }
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_cliente_estab_act_economica]. Motivo: " + ex.Message);
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

        public DataTable GetOtrasConfMunicipio()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtOtrasConfMunicipio";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_consulta_tarifa_min", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE CAMPOS
                    TablaDatos.Columns.Add("idmun_tarifa_minima", typeof(Int32));
                    TablaDatos.Columns.Add("numero_renglon");
                    TablaDatos.Columns.Add("calcular_renglon");
                    TablaDatos.Columns.Add("numero_renglon1");
                    TablaDatos.Columns.Add("idtipo_operacion1");
                    TablaDatos.Columns.Add("numero_renglon2");
                    TablaDatos.Columns.Add("idtipo_operacion2");
                    TablaDatos.Columns.Add("numero_renglon3");
                    TablaDatos.Columns.Add("idtipo_operacion3");
                    TablaDatos.Columns.Add("numero_renglon4");
                    TablaDatos.Columns.Add("idtipo_operacion4");
                    TablaDatos.Columns.Add("numero_renglon5");
                    TablaDatos.Columns.Add("idtipo_operacion5");
                    TablaDatos.Columns.Add("numero_renglon6");
                    TablaDatos.Columns.Add("idunidad_medida", typeof(Int32));
                    TablaDatos.Columns.Add("idtipo_tarifa", typeof(Int32));
                    TablaDatos.Columns.Add("valor_unidad", typeof(Double));
                    TablaDatos.Columns.Add("cantidad_medida", typeof(Double));
                    TablaDatos.Columns.Add("cantidad_periodos", typeof(Double));
                    TablaDatos.Columns.Add("valor_unid_medida", typeof(Double));
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DE LA CONSULTA
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idmun_tarifa_minima"] = Int32.Parse(TheDataReaderPostgreSQL["idmun_tarifa_minima"].ToString().Trim());
                            Fila["numero_renglon"] = TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim();
                            Fila["calcular_renglon"] = TheDataReaderPostgreSQL["calcular_renglon"].ToString().Trim();
                            Fila["numero_renglon1"] = TheDataReaderPostgreSQL["numero_renglon1"].ToString().Trim();
                            Fila["idtipo_operacion1"] = TheDataReaderPostgreSQL["idtipo_operacion1"].ToString().Trim();
                            Fila["numero_renglon2"] = TheDataReaderPostgreSQL["numero_renglon2"].ToString().Trim();
                            Fila["idtipo_operacion2"] = TheDataReaderPostgreSQL["idtipo_operacion2"].ToString().Trim();
                            Fila["numero_renglon3"] = TheDataReaderPostgreSQL["numero_renglon3"].ToString().Trim();
                            Fila["idtipo_operacion3"] = TheDataReaderPostgreSQL["idtipo_operacion3"].ToString().Trim();
                            Fila["numero_renglon4"] = TheDataReaderPostgreSQL["numero_renglon4"].ToString().Trim();
                            Fila["idtipo_operacion4"] = TheDataReaderPostgreSQL["idtipo_operacion4"].ToString().Trim();
                            Fila["numero_renglon5"] = TheDataReaderPostgreSQL["numero_renglon5"].ToString().Trim();
                            Fila["idtipo_operacion5"] = TheDataReaderPostgreSQL["idtipo_operacion5"].ToString().Trim();
                            Fila["numero_renglon6"] = TheDataReaderPostgreSQL["numero_renglon6"].ToString().Trim();

                            Fila["idunidad_medida"] = Int32.Parse(TheDataReaderPostgreSQL["idunidad_medida"].ToString().Trim());
                            Fila["idtipo_tarifa"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_tarifa"].ToString().Trim());
                            Fila["valor_unidad"] = Double.Parse(TheDataReaderPostgreSQL["valor_unidad"].ToString().Trim());
                            Fila["cantidad_medida"] = Double.Parse(TheDataReaderPostgreSQL["cantidad_medida"].ToString().Trim());
                            Fila["cantidad_periodos"] = Double.Parse(TheDataReaderPostgreSQL["cantidad_periodos"].ToString().Trim());
                            Fila["valor_unid_medida"] = Double.Parse(TheDataReaderPostgreSQL["valor_unid_medida"].ToString().Trim());
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_cliente_estab_act_economica]. Motivo: " + ex.Message);
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

        public string GetTarifaCalendario()
        {
            string _Result = "";
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
                    return _Result;
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_fecha_calendario", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcalendario_trib", IdMunCalendarioTrib);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_periodicidad_pago", IdPeriodicidad);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_codigo_impuesto", CodigoImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_fecha_limite", FechaInicial);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            _Result = TheDataReaderPostgreSQL["fecha_limite"].ToString().Trim() + "|" + TheDataReaderPostgreSQL["valor_descuento"].ToString().Trim();
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
                    return _Result;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al obtener los datos de la Tabla [tbl_cliente_estab_act_economica]. Motivo: " + ex.Message);
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

            return _Result;
        }

        public int GetCantidadPuntosMunicipio()
        {
            int _Result = 0;
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
                    return _Result;
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_municipios_cant_puntos", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            _Result = Int32.Parse(TheDataReaderPostgreSQL["cantidad_puntos"].ToString().Trim());
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
                    return _Result;
                }
            }
            catch (Exception ex)
            {
                _Result = 0;
                _log.Error("Error al obtener los datos de la Tabla [tbl_municipio_cant_puntos]. Motivo: " + ex.Message);
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

            return _Result;
        }

        public DataTable GetConsultaLiqOficinas()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtLiquidacionOficinas";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_consulta_tarifa_min", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE CAMPOS
                    TablaDatos.Columns.Add("idmun_tarifa_minima", typeof(Int32));
                    TablaDatos.Columns.Add("numero_renglon");
                    TablaDatos.Columns.Add("calcular_renglon");
                    TablaDatos.Columns.Add("numero_renglon1");
                    TablaDatos.Columns.Add("idtipo_operacion");
                    TablaDatos.Columns.Add("numero_renglon2");
                    TablaDatos.Columns.Add("idunidad_medida", typeof(Int32));
                    TablaDatos.Columns.Add("idtipo_tarifa", typeof(Int32));
                    TablaDatos.Columns.Add("valor_unidad", typeof(Double));
                    TablaDatos.Columns.Add("cantidad_medida", typeof(Double));
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DE LA CONSULTA
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idmun_tarifa_minima"] = Int32.Parse(TheDataReaderPostgreSQL["idmun_tarifa_minima"].ToString().Trim());
                            Fila["numero_renglon"] = TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim();
                            Fila["calcular_renglon"] = TheDataReaderPostgreSQL["calcular_renglon"].ToString().Trim();
                            Fila["numero_renglon1"] = TheDataReaderPostgreSQL["numero_renglon1"].ToString().Trim();
                            Fila["idtipo_operacion"] = TheDataReaderPostgreSQL["idtipo_operacion"].ToString().Trim();
                            Fila["numero_renglon2"] = TheDataReaderPostgreSQL["numero_renglon2"].ToString().Trim();
                            Fila["idunidad_medida"] = Int32.Parse(TheDataReaderPostgreSQL["idunidad_medida"].ToString().Trim());
                            Fila["idtipo_tarifa"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_tarifa"].ToString().Trim());
                            Fila["valor_unidad"] = Double.Parse(TheDataReaderPostgreSQL["valor_unidad"].ToString().Trim());
                            Fila["cantidad_medida"] = Double.Parse(TheDataReaderPostgreSQL["cantidad_medida"].ToString().Trim());
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_cliente_estab_act_economica]. Motivo: " + ex.Message);
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

    }
}