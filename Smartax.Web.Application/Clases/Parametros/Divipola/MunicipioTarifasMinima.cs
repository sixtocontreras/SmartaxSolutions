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

namespace Smartax.Web.Application.Clases.Parametros.Divipola
{
    public class MunicipioTarifasMinima
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
        public object IdMunTarMinima { get; set; }
        public object IdMunicipio { get; set; }
        public object IdFormularioImpuesto { get; set; }
        public string CalcularRenglon { get; set; }
        public object IdFormuConfiguracion { get; set; }
        public object IdTipoOperacion1 { get; set; }
        public object IdTipoOperacion2 { get; set; }
        public object IdTipoOperacion3 { get; set; }
        public object IdTipoOperacion4 { get; set; }
        public object IdTipoOperacion5 { get; set; }
        public object IdFormuConfiguracion1 { get; set; }
        public object IdFormuConfiguracion2 { get; set; }
        public object IdFormuConfiguracion3 { get; set; }
        public object IdFormuConfiguracion4 { get; set; }
        public object IdFormuConfiguracion5 { get; set; }
        public object IdFormuConfiguracion6 { get; set; }
        public object IdUnidadMedida { get; set; }
        public object IdValorUnidadMedida { get; set; }
        public object IdUnidadMedidaBaseGravable { get; set; }
        public object IdTipoTarifa { get; set; }
        public string CantidadMedida { get; set; }
        public string CantidadPeriodos { get; set; }
        public string ValorConcepto { get; set; }
        public string Descripcion { get; set; }
        public object IdEstado { get; set; }
        public int IdUsuario { get; set; }        
        public string ArrayData { get; set; }
        public string MostrarSeleccione { get; set; }
        public string MotorBaseDatos { get; set; }
        public int TipoConsulta { get; set; }
        public int TipoProceso { get; set; }
        #endregion

        public DataTable GetAllMunTarMinima()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtMunTarMinima";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_municipio_tar_minima", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmun_tarifa_minima", IdMunTarMinima);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("idmun_tarifa_minima", typeof(Int32));
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("descripcion_formulario");
                    TablaDatos.Columns.Add("anio_tarifa");
                    TablaDatos.Columns.Add("anio_gravable");
                    TablaDatos.Columns.Add("unidad_medida");
                    TablaDatos.Columns.Add("cantidad_medida");
                    TablaDatos.Columns.Add("valor_unidad");
                    TablaDatos.Columns.Add("tarifa_minima");
                    TablaDatos.Columns.Add("numero_renglon");
                    TablaDatos.Columns.Add("numero_renglon1");
                    TablaDatos.Columns.Add("simbolo1");
                    TablaDatos.Columns.Add("numero_renglon2");
                    TablaDatos.Columns.Add("simbolo2");
                    TablaDatos.Columns.Add("numero_renglon3");
                    TablaDatos.Columns.Add("simbolo3");
                    TablaDatos.Columns.Add("numero_renglon4");
                    TablaDatos.Columns.Add("descripcion_configuracion");
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idmun_tarifa_minima"] = Convert.ToInt32(TheDataReaderPostgreSQL["idmun_tarifa_minima"].ToString().Trim());
                            Fila["idformulario_impuesto"] = Convert.ToInt32(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["descripcion_formulario"] = TheDataReaderPostgreSQL["descripcion_formulario"].ToString().Trim();

                            Fila["anio_tarifa"] = TheDataReaderPostgreSQL["anio_valor"].ToString().Trim();
                            Fila["anio_gravable"] = TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim();
                            Fila["unidad_medida"] = TheDataReaderPostgreSQL["unidad_medida"].ToString().Trim();

                            double _CantidadMedida = Convert.ToDouble(TheDataReaderPostgreSQL["cantidad_medida"].ToString().Trim());
                            double _ValorUnidad = Convert.ToDouble(TheDataReaderPostgreSQL["valor_unidad"].ToString().Trim());
                            double _TarifaMinima = (_CantidadMedida * _ValorUnidad);
                            Fila["cantidad_medida"] = String.Format(String.Format("{0:###,###,##0.####}", _CantidadMedida));
                            Fila["valor_unidad"] = String.Format(String.Format("{0:$ ###,###,##0}", _ValorUnidad));
                            Fila["tarifa_minima"] = String.Format(String.Format("{0:$ ###,###,##0}", _TarifaMinima));

                            Fila["numero_renglon"] = TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim();
                            Fila["numero_renglon1"] = TheDataReaderPostgreSQL["numero_renglon1"].ToString().Trim();
                            Fila["simbolo1"] = TheDataReaderPostgreSQL["simbolo1"].ToString().Trim();
                            Fila["numero_renglon2"] = TheDataReaderPostgreSQL["numero_renglon2"].ToString().Trim();
                            Fila["simbolo2"] = TheDataReaderPostgreSQL["simbolo2"].ToString().Trim();
                            Fila["numero_renglon3"] = TheDataReaderPostgreSQL["numero_renglon3"].ToString().Trim();
                            Fila["simbolo3"] = TheDataReaderPostgreSQL["simbolo3"].ToString().Trim();
                            Fila["numero_renglon4"] = TheDataReaderPostgreSQL["numero_renglon4"].ToString().Trim();

                            Fila["descripcion_configuracion"] = TheDataReaderPostgreSQL["descripcion_configuracion"].ToString().Trim();
                            Fila["id_estado"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_municipio_tarifa_minima]. Motivo: " + ex.Message);
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

        public DataTable GetInfoMunTarMinima()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtMunTarMinima";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_municipio_tar_minima", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmun_tarifa_minima", IdMunTarMinima);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINIR COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idmun_tarifa_minima", typeof(Int32));
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("idunidad_medida");
                    TablaDatos.Columns.Add("idvalor_unid_medida");
                    TablaDatos.Columns.Add("anio_valor");
                    TablaDatos.Columns.Add("valor_unidad1");
                    TablaDatos.Columns.Add("valor_unidad");
                    TablaDatos.Columns.Add("idunid_medida_bg");
                    TablaDatos.Columns.Add("anio_gravable");
                    TablaDatos.Columns.Add("valor_unidad_bg");
                    TablaDatos.Columns.Add("idtipo_tarifa");
                    TablaDatos.Columns.Add("cantidad_medida");
                    TablaDatos.Columns.Add("cantidad_periodos");
                    TablaDatos.Columns.Add("idform_configuracion");
                    TablaDatos.Columns.Add("numero_renglon");
                    TablaDatos.Columns.Add("descripcion_renglon");
                    TablaDatos.Columns.Add("calcular_renglon", typeof(Boolean));
                    TablaDatos.Columns.Add("idform_configuracion1");
                    TablaDatos.Columns.Add("idtipo_operacion");
                    TablaDatos.Columns.Add("idform_configuracion2");
                    TablaDatos.Columns.Add("idtipo_operacion2");
                    TablaDatos.Columns.Add("idform_configuracion3");
                    TablaDatos.Columns.Add("idtipo_operacion3");
                    TablaDatos.Columns.Add("idform_configuracion4");
                    TablaDatos.Columns.Add("idtipo_operacion4");
                    TablaDatos.Columns.Add("idform_configuracion5");
                    TablaDatos.Columns.Add("idtipo_operacion5");
                    TablaDatos.Columns.Add("idform_configuracion6");
                    TablaDatos.Columns.Add("descripcion_configuracion");
                    TablaDatos.Columns.Add("valor_unid_medida");
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idmun_tarifa_minima"] = Convert.ToInt32(TheDataReaderPostgreSQL["idmun_tarifa_minima"].ToString().Trim());
                            Fila["idformulario_impuesto"] = Convert.ToInt32(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());

                            Fila["idunidad_medida"] = TheDataReaderPostgreSQL["idunidad_medida"].ToString().Trim();
                            Fila["idvalor_unid_medida"] = TheDataReaderPostgreSQL["idvalor_unid_medida"].ToString().Trim();
                            Fila["anio_valor"] = TheDataReaderPostgreSQL["anio_valor"].ToString().Trim();
                            Fila["valor_unidad"] = TheDataReaderPostgreSQL["valor_unidad"].ToString().Trim();
                            //--
                            Fila["idunid_medida_bg"] = TheDataReaderPostgreSQL["idunid_medida_bg"].ToString().Trim();
                            Fila["anio_gravable"] = TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim();
                            Fila["valor_unidad_bg"] = TheDataReaderPostgreSQL["valor_unidad_bg"].ToString().Trim();
                            //--
                            Fila["idtipo_tarifa"] = TheDataReaderPostgreSQL["idtipo_tarifa"].ToString().Trim();
                            Fila["cantidad_medida"] = TheDataReaderPostgreSQL["cantidad_medida"].ToString().Trim();
                            Fila["cantidad_periodos"] = TheDataReaderPostgreSQL["cantidad_periodos"].ToString().Trim();
                            //--
                            Fila["idform_configuracion"] = TheDataReaderPostgreSQL["idform_configuracion"].ToString().Trim();
                            Fila["numero_renglon"] = TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim();
                            Fila["descripcion_renglon"] = TheDataReaderPostgreSQL["descripcion_renglon"].ToString().Trim();
                            Fila["calcular_renglon"] = TheDataReaderPostgreSQL["calcular_renglon"].ToString().Trim().Equals("S") ? true : false;
                            //--
                            Fila["idform_configuracion1"] = TheDataReaderPostgreSQL["idform_configuracion1"].ToString().Trim();
                            Fila["idtipo_operacion"] = TheDataReaderPostgreSQL["idtipo_operacion"].ToString().Trim();
                            Fila["idform_configuracion2"] = TheDataReaderPostgreSQL["idform_configuracion2"].ToString().Trim();
                            Fila["idtipo_operacion2"] = TheDataReaderPostgreSQL["idtipo_operacion2"].ToString().Trim();
                            Fila["idform_configuracion3"] = TheDataReaderPostgreSQL["idform_configuracion3"].ToString().Trim();
                            Fila["idtipo_operacion3"] = TheDataReaderPostgreSQL["idtipo_operacion3"].ToString().Trim();
                            Fila["idform_configuracion4"] = TheDataReaderPostgreSQL["idform_configuracion4"].ToString().Trim();
                            Fila["idtipo_operacion4"] = TheDataReaderPostgreSQL["idtipo_operacion4"].ToString().Trim();
                            Fila["idform_configuracion5"] = TheDataReaderPostgreSQL["idform_configuracion5"].ToString().Trim();
                            Fila["idtipo_operacion5"] = TheDataReaderPostgreSQL["idtipo_operacion5"].ToString().Trim();
                            Fila["idform_configuracion6"] = TheDataReaderPostgreSQL["idform_configuracion6"].ToString().Trim();

                            Fila["descripcion_configuracion"] = TheDataReaderPostgreSQL["descripcion_configuracion"].ToString().Trim();
                            Fila["valor_unid_medida"] = TheDataReaderPostgreSQL["valor_unid_medida"].ToString().Trim();
                            Fila["id_estado"] = TheDataReaderPostgreSQL["id_estado"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_municipio_tarifa_minima]. Motivo: " + ex.Message);
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

        public bool AddUpMunTarMinima(DataRow Fila, ref int _IdRegistro, ref string _MsgError)
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_municipio_tar_minima", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmun_tarifa_minima", IdMunTarMinima);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idformulario_impuesto", Fila["idformulario_impuesto"].ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_tarifa", Fila["id_anio"].ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tarifa_minima", Fila["tarifa_minima"].ToString().Trim().Replace(",", "."));
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", Fila["id_estado"]);
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
                    #region REGISTRAR DATO CON EL SP EN LA DB SQL SERVER
                    //Base de datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_crud_municipio_tar_minima";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (System.Data.SqlClient.SqlConnection)myConnectionDb;
                    //se limpian los parámetros
                    TheCommandSQLServer.Parameters.Clear();

                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idmun_tarifa_minima", IdMunTarMinima);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idformulario_impuesto", Fila["idformulario_impuesto"].ToString().Trim());
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_anio_tarifa", Fila["id_anio"].ToString().Trim());
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_tarifa_minima", Fila["tarifa_minima"].ToString().Trim().Replace(",", "."));
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idestado", Fila["id_estado"]);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
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
                            _MsgError = "La tarifa ha sido registrada exitosamente.";
                        }
                        else if (TipoProceso == 2)
                        {
                            _MsgError = "La tarifa ha sido editada exitosamente.";
                        }
                        else if (TipoProceso == 3)
                        {
                            _MsgError = "La tarifa ha sido eliminada del sistema.";
                        }

                        retValor = true;
                    }

                    TheCommandSQLServer.Dispose();
                    #endregion
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
                _MsgError = "Error al registrar la tarifa minima del municipio. Motivo: " + ex.Message.ToString().Trim();
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

        public bool AddUpMunTarMinima(ref int _IdRegistro, ref string _MsgError)
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_municipio_tar_minima", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmun_tarifa_minima", IdMunTarMinima);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idformulario_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_calcular_renglon", CalcularRenglon);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion", IdFormuConfiguracion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion1", IdFormuConfiguracion1);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion", IdTipoOperacion1);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion2", IdFormuConfiguracion2);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion2", IdTipoOperacion2);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion3", IdFormuConfiguracion3);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion3", IdTipoOperacion3);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion4", IdFormuConfiguracion4);

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion4", IdTipoOperacion4);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion5", IdFormuConfiguracion5);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion5", IdTipoOperacion5);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion6", IdFormuConfiguracion6);

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idunidad_medida", IdUnidadMedida);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idvalor_unid_medida", IdValorUnidadMedida);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idunid_medida_bg", IdUnidadMedidaBaseGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_tarifa", IdTipoTarifa);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_cantidad_medida", CantidadMedida);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_cantidad_periodo", CantidadPeriodos);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_concepto", ValorConcepto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_descripcion", Descripcion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
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
                _MsgError = "Error al registrar la tarifa minima del municipio. Motivo: " + ex.Message.ToString().Trim();
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

        public bool GetLoadOtrasConfig(ref int _IdRegistro, ref string _MsgError)
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_load_otras_config", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_row_data", string.Format("{{{0}}}", string.Join(",", ArrayData)));
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
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
                _MsgError = "Error al realizar el proceso del cargue del archivo. Motivo: " + ex.Message.ToString().Trim();
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