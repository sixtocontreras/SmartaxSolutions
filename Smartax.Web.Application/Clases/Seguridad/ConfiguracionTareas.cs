using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using log4net;
using log4net.Config;
using Devart.Data.PostgreSql;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OracleClient;

namespace Smartax.Web.Application.Clases.Seguridad
{
    public class ConfiguracionTareas
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
        public object IdConfiguracionAlerta { get; set; }
        public int IdTipoTarea { get; set; }
        public int DiasSeguimiento { get; set; }
        public int IdTipoEnvio { get; set; }
        public string FechaInicio { get; set; }
        public string HoraInicio { get; set; }
        public int Intervalo { get; set; }
        public string FechaFin { get; set; }
        public string HoraFin { get; set; }
        public string DiasEnvio { get; set; }
        public int IdRol { get; set; }
        public int IdEmpresa { get; set; }
        public object IdEstado { get; set; }
        public int IdUsuario { get; set; }
        public string MostrarSeleccione { get; set; }
        public string MotorBaseDatos { get; set; }
        public int TipoProceso { get; set; }
        #endregion

        public DataTable GetAllConfiguracionTareas()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtConfiguracion";
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
                    myConnectionDb = new MySql.Data.MySqlClient.MySqlConnection(connString);
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
                    #region AQUI OBTENEMOS LOS DATOS DE LA DB DE POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_configuracion_alertas", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idconfiguracion", IdConfiguracionAlerta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoProceso);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region AQUI DEFINIMOS LAS COLUMNAS DEL DATATBLE
                    TablaDatos.Columns.Add("id_configuracion");
                    TablaDatos.Columns.Add("idtipo_tarea", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_tarea");
                    TablaDatos.Columns.Add("dias_seguimiento");
                    TablaDatos.Columns.Add("fecha_inicio");
                    TablaDatos.Columns.Add("hora_inicio");
                    TablaDatos.Columns.Add("intervalo");
                    TablaDatos.Columns.Add("fecha_fin");
                    TablaDatos.Columns.Add("hora_fin");
                    TablaDatos.Columns.Add("dias_envio");
                    TablaDatos.Columns.Add("idtipo_envio", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_envio");
                    //TablaDatos.Columns.Add("id_empresa", typeof(Int32));
                    //TablaDatos.Columns.Add("nombre_empresa");
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_estado");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_configuracion"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_configuracion"]);
                            Fila["idtipo_tarea"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_tarea"].ToString().Trim());
                            Fila["tipo_tarea"] = TheDataReaderPostgreSQL["tipo_tarea"].ToString().Trim();
                            Fila["dias_seguimiento"] = TheDataReaderPostgreSQL["dias_seguimiento"].ToString().Trim();
                            Fila["fecha_inicio"] = TheDataReaderPostgreSQL["fecha_inicio"].ToString().Trim();
                            Fila["hora_inicio"] = TheDataReaderPostgreSQL["hora_inicio"].ToString().Trim();
                            Fila["intervalo"] = TheDataReaderPostgreSQL["intervalo"].ToString().Trim();
                            Fila["fecha_fin"] = TheDataReaderPostgreSQL["fecha_fin"].ToString().Trim();
                            Fila["hora_fin"] = TheDataReaderPostgreSQL["hora_fin"].ToString().Trim();
                            Fila["dias_envio"] = TheDataReaderPostgreSQL["dias_envio"].ToString().Trim();
                            Fila["idtipo_envio"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_envio"].ToString().Trim());
                            Fila["tipo_envio"] = TheDataReaderPostgreSQL["tipo_envio"].ToString().Trim();
                            //Fila["id_empresa"] = Int32.Parse(TheDataReaderPostgreSQL["id_empresa"].ToString().Trim());
                            //Fila["nombre_empresa"] = TheDataReaderPostgreSQL["nombre_empresa"].ToString().Trim();
                            Fila["id_estado"] = Int32.Parse(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
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
                _log.Error("Error al traer informacion de la Configuracion. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION A LA DB
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
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

        public bool AddUpConfiguracionAlerta(ref int _IdRegistro, ref string _MsgError)
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
                    myConnectionDb = new MySql.Data.MySqlClient.MySqlConnection(connString);
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
                    #region REGISTRAR DATOS EN LA DB DE POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_configuracion_alerta", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //se limpian los parámetros
                    TheCommandPostgreSQL.Parameters.Clear();

                    //comenzamos a mandar cada uno de los parámetros
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idconfiguracion", IdConfiguracionAlerta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_tarea", IdTipoTarea);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_dias_seguimiento", DiasSeguimiento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_envio", IdTipoEnvio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_fecha_inicio", FechaInicio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_hora_inicio", HoraInicio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_intervalo", Intervalo);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_fecha_fin", FechaFin);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_hora_fin", HoraFin);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_dias_envio", DiasEnvio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa", IdEmpresa);
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
                else if (myConnectionDb is MySqlConnection)
                {

                }
                else if (myConnectionDb is SqlConnection)
                {
                    //--BASE DE DATOS DE SQL SERVER
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
                _MsgError = "Error al registrar la tarea programada. Motivo: " + ex.Message.ToString().Trim();
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

    }
}