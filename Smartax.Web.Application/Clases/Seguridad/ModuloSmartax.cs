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

namespace Smartax.Web.Application.Clases.Seguridad
{
    public class ModuloSmartax
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
        public object IdModulo { get; set; }
        public object IdRol { get; set; }
        public bool PlaneacionFiscal { get; set; }
        public bool CalendarioTributario { get; set; }
        public bool TarifasExcesivas { get; set; }
        //--
        public bool InformacionTributaria { get; set; }
        public bool HojaTrabajo { get; set; }
        public bool DeclaracionDefinitiva { get; set; }
        public bool EjecucionPorLote { get; set; }
        public bool ValidacionLiqLote { get; set; }
        public bool ConsultaLiquidacion { get; set; }
        public bool FichaTecnica { get; set; }
        //--
        public bool FormatosSfc { get; set; }
        public bool GeneracionDatos { get; set; }
        public bool GenerarF321 { get; set; }
        public bool GenerarF525 { get; set; }
        public bool GenerarArchivoPlano { get; set; }
        //--
        public bool Normatividad { get; set; }
        public bool CargarNormatividad { get; set; }
        public bool ConsultarNormatividad { get; set; }
        public bool CargaMasivaDoc { get; set; }
        //--
        public bool ControlActividades { get; set; }
        public bool MisActividades { get; set; }
        public bool MonitoreoActividades { get; set; }
        public bool EstadisticaActividades { get; set; }
        public bool EstadisticaLiquidacion { get; set; }
        //--
        public int IdUsuario { get; set; }
        public string MostrarSeleccione { get; set; }
        public string MotorBaseDatos { get; set; }
        public int TipoConsulta { get; set; }
        public int TipoProceso { get; set; }
        #endregion

        public bool DevolverModulos()
        {
            bool retValor = false;
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_modulos_smartax", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            Normatividad = Convert.ToBoolean(TheDataReaderPostgreSQL["normatividad"]);
                            CargarNormatividad = Convert.ToBoolean(TheDataReaderPostgreSQL["cargar_normatividad"]);
                            ConsultarNormatividad = Convert.ToBoolean(TheDataReaderPostgreSQL["consultar_normatividad"]);
                            CargaMasivaDoc = Convert.ToBoolean(TheDataReaderPostgreSQL["carga_masiva_doc"]);
                            //--
                            PlaneacionFiscal = Convert.ToBoolean(TheDataReaderPostgreSQL["planeacion_fiscal"]);
                            CalendarioTributario = Convert.ToBoolean(TheDataReaderPostgreSQL["calendario_tributario"]);
                            TarifasExcesivas = Convert.ToBoolean(TheDataReaderPostgreSQL["tarifas_excesivas"]);
                            //--
                            InformacionTributaria = Convert.ToBoolean(TheDataReaderPostgreSQL["informacion_tributaria"]);
                            HojaTrabajo = Convert.ToBoolean(TheDataReaderPostgreSQL["hoja_trabajo"]);
                            DeclaracionDefinitiva = Convert.ToBoolean(TheDataReaderPostgreSQL["declaracion_definitiva"]);
                            EjecucionPorLote = Convert.ToBoolean(TheDataReaderPostgreSQL["ejecucion_por_lote"]);
                            ValidacionLiqLote = Convert.ToBoolean(TheDataReaderPostgreSQL["validacion_liq_lote"]);
                            ConsultaLiquidacion = Convert.ToBoolean(TheDataReaderPostgreSQL["consulta_liquidacion"]);
                            FichaTecnica = Convert.ToBoolean(TheDataReaderPostgreSQL["ficha_tecnica"]);
                            //--
                            FormatosSfc = Convert.ToBoolean(TheDataReaderPostgreSQL["formatos_sfc"]);
                            GeneracionDatos = Convert.ToBoolean(TheDataReaderPostgreSQL["generacion_datos"]);
                            GenerarF321 = Convert.ToBoolean(TheDataReaderPostgreSQL["generar_f321"]);
                            GenerarF525 = Convert.ToBoolean(TheDataReaderPostgreSQL["generar_f525"]);
                            GenerarArchivoPlano = Convert.ToBoolean(TheDataReaderPostgreSQL["generar_archivo_plano"]);
                            //--
                            //Normatividad = Convert.ToBoolean(TheDataReaderPostgreSQL["normatividad"]);
                            //CargarNormatividad = Convert.ToBoolean(TheDataReaderPostgreSQL["cargar_normatividad"]);
                            //ConsultarNormatividad = Convert.ToBoolean(TheDataReaderPostgreSQL["consultar_normatividad"]);
                            //CargaMasivaDoc = Convert.ToBoolean(TheDataReaderPostgreSQL["carga_masiva_doc"]);
                            //--
                            ControlActividades = Convert.ToBoolean(TheDataReaderPostgreSQL["control_actividades"]);
                            MisActividades = Convert.ToBoolean(TheDataReaderPostgreSQL["mis_actividades"]);
                            MonitoreoActividades = Convert.ToBoolean(TheDataReaderPostgreSQL["monitoreo_act"]);
                            EstadisticaActividades = Convert.ToBoolean(TheDataReaderPostgreSQL["estadistica_act"]);
                            EstadisticaLiquidacion = Convert.ToBoolean(TheDataReaderPostgreSQL["estadistica_liq"]);
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
                _log.Error("Se produjo una exepción con el Metodo [DevolverModulos]. Motivo: " + ex.Message);
                retValor = false;
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION DE LA DB
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
                #endregion
            }

            return retValor;
        }

        public bool AddUpModulos(ref int _IdRegistro, ref string _MsgError)
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
                    #region PROCESO CON EL SP DE LA BD POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_modulos_smartax", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmodulo", IdModulo);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_planeacion_fiscal", PlaneacionFiscal == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_calendario_tributario", CalendarioTributario == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tarifas_excesivas", TarifasExcesivas == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_informacion_tributaria", InformacionTributaria == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_hoja_trabajo", HojaTrabajo == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_declaracion_def", DeclaracionDefinitiva == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_consulta_liquidacion", ConsultaLiquidacion == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_ficha_tecnica", FichaTecnica == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_ejecucion_lote", EjecucionPorLote == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_validacion_liq_lote", ValidacionLiqLote == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_formatos_sfc", FormatosSfc == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_generacion_datos", GeneracionDatos == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_generar_f321", GenerarF321 == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_generar_f525", GenerarF525 == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_generar_archivo_plano", GenerarArchivoPlano == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_normatividad", Normatividad == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_cargar_normatividad", CargarNormatividad == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_consultar_normatividad", ConsultarNormatividad == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_carga_masiva_doc", CargaMasivaDoc == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_control_act", ControlActividades == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mis_actividades", MisActividades == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_monitoreo_act", MonitoreoActividades == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_estadistica_act", EstadisticaActividades == true ? 1 : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_estadistica_liq", EstadisticaLiquidacion == true ? 1 : 0);
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
                _MsgError = "Error al registrar el modulo. Motivo: " + ex.Message.ToString().Trim();
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