﻿using System;
using Devart.Data.PostgreSql;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Configuration;
using log4net;
using System.Data.SqlClient;
using System.Data.OracleClient;
using Smartax.Web.Application.Clases.Seguridad;

namespace Smartax.Web.Application.Clases.Parametros.Tipos
{
    public class FormConfiguracion
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
        public object IdFormConfiguracion { get; set; }
        public object IdFormularioImpuesto { get; set; }
        public int NumeroRenglon { get; set; }
        public string DescripcionRenglon { get; set; }
        public object NumeroOrden { get; set; }
        public object IdEstado { get; set; }
        public int IdUsuario { get; set; }
        public string MostrarSeleccione { get; set; }
        public string MotorBaseDatos { get; set; }
        public int TipoConsulta { get; set; }
        public int TipoProceso { get; set; }
        #endregion

        public DataTable GetAllFormConfiguracion()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtFormConfiguracion";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_formulario_configuracion", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idformulario_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("idformulario_configuracion", typeof(Int32));
                    TablaDatos.Columns.Add("numero_renglon", typeof(Int32));
                    TablaDatos.Columns.Add("descripcion_renglon");
                    TablaDatos.Columns.Add("numero_orden", typeof(Int32));
                    TablaDatos.Columns.Add("renglon_calculado", typeof(Boolean));
                    TablaDatos.Columns.Add("renglon_contabilizar", typeof(Boolean));
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idformulario_configuracion"] = Convert.ToInt32(TheDataReaderPostgreSQL["idformulario_configuracion"].ToString().Trim());
                            Fila["numero_renglon"] = Convert.ToInt32(TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim());
                            Fila["descripcion_renglon"] = TheDataReaderPostgreSQL["descripcion_renglon"].ToString().Trim();
                            Fila["numero_orden"] = Convert.ToInt32(TheDataReaderPostgreSQL["numero_orden"].ToString().Trim());
                            Fila["renglon_calculado"] = TheDataReaderPostgreSQL["renglon_calculado"].ToString().Trim().Equals("S") ? true : false;
                            Fila["renglon_contabilizar"] = TheDataReaderPostgreSQL["renglon_contabilizar"].ToString().Trim().Equals("S") ? true : false;

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
                _log.Error("Error al obtener los datos de la Tabla [tbl_formulario_configuracion]. Motivo: " + ex.Message);
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

        public DataTable GetFormConfiguracion()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtFormConfiguracion";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_formulario_configuracion", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idformulario_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("idformulario_configuracion");
                    TablaDatos.Columns.Add("numero_renglon");

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
                            Fila["idformulario_configuracion"] = TheDataReaderPostgreSQL["idformulario_configuracion"].ToString().Trim() + "|" + TheDataReaderPostgreSQL["descripcion_renglon"].ToString().Trim();
                            Fila["numero_renglon"] = TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_formulario_configuracion]. Motivo: " + ex.Message);
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

        public DataTable GetFormConfiguracionRenglon()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtFormConfiguracion";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_formulario_configuracion", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idformulario_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("idformulario_configuracion");
                    TablaDatos.Columns.Add("numero_renglon");

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
                            Fila["idformulario_configuracion"] = TheDataReaderPostgreSQL["idformulario_configuracion"].ToString().Trim();
                            Fila["numero_renglon"] = TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim();
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
                    TheCommandSQLServer.CommandText = "sp_web_get_formulario_configuracion";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    //Limpiar parametros
                    TheCommandSQLServer.Parameters.Clear();

                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idformulario_impuesto", IdFormularioImpuesto);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    TablaDatos.Columns.Add("idformulario_configuracion", typeof(Int32));
                    TablaDatos.Columns.Add("numero_renglon");

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
                            Fila["idformulario_configuracion"] = Convert.ToInt32(TheDataReaderSQLServer["idformulario_configuracion"].ToString().Trim());
                            Fila["numero_renglon"] = TheDataReaderSQLServer["numero_renglon"].ToString().Trim() + "|" + TheDataReaderSQLServer["descripcion_renglon"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_formulario_configuracion]. Motivo: " + ex.Message);
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

        public bool AddUpFormConfiguracion(DataRow Fila, ref int _IdRegistro, ref string _MsgError)
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_formulario_configuracion", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    string _DescripcionRenglon1 = Fila["descripcion_renglon"].ToString().Trim();
                    string _DescripcionRenglon2 = _DescripcionRenglon1.Replace("Ñ", "N").Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U");
                    DescripcionRenglon = _DescripcionRenglon2.Replace("ñ", "n").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
                    //--
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idformulario_config", IdFormConfiguracion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idformulario_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_numero_renglon", Fila["numero_renglon"].ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_descripcion_renglon", DescripcionRenglon);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_numero_orden", Fila["numero_orden"].ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_renglon_calculado", Boolean.Parse(Fila["renglon_calculado"].ToString().Trim()) == true ? "S" : "N");
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_renglon_contabilizar", Boolean.Parse(Fila["renglon_contabilizar"].ToString().Trim()) == true ? "S" : "N");
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
                    #region REGISTRAR DATO CON EL SP EN LA DB SQL SERVER
                    //Base de datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_crud_formulario_configuracion";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (System.Data.SqlClient.SqlConnection)myConnectionDb;
                    //se limpian los parámetros
                    TheCommandSQLServer.Parameters.Clear();

                    DescripcionRenglon = Fila["descripcion_renglon"].ToString().Trim().ToUpper();
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idformulario_config", IdFormConfiguracion);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idformulario_impuesto", IdFormularioImpuesto);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_numero_renglon", Fila["numero_renglon"].ToString().Trim());
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_descripcion_renglon", DescripcionRenglon);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idestado", Fila["id_estado"].ToString().Trim());
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
                            _MsgError = "La configuracion [" + DescripcionRenglon + "] ha sido registrado exitosamente.";
                        }
                        else if (TipoProceso == 2)
                        {
                            _MsgError = "La configuracion [" + DescripcionRenglon + "] han sido editados exitosamente.";
                        }
                        else if (TipoProceso == 3)
                        {
                            _MsgError = "La configuracion [" + DescripcionRenglon + "] ah sido eliminado del sistema.";
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
                _MsgError = "Error al registrar el Tipo de tarifa. Motivo: " + ex.Message.ToString().Trim();
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