﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Devart.Data.PostgreSql;
using MySql.Data.MySqlClient;
using Smartax.WebApi.Services.Models;
using Smartax.WebApi.Services.Clases.Seguridad;
using System.Text;
using System.Configuration;

namespace Smartax.WebApi.Services.Clases.Procesos
{
    public class ProcessDb
    {
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

        #region DEFINICION DE ATRIBUTOS DE LA CLASE
        public object NumeroRenglon { get; set; }
        public object IdClienteEF { get; set; }
        public object IdCliente { get; set; }
        public object IdDepartamento { get; set; }
        public object IdMunCalendarioTrib { get; set; }
        public object IdMunicipio { get; set; }
        public object IdClienteEstablecimiento { get; set; }
        public object IdEstablecimientoPadre { get; set; }
        public object IdFormularioImpuesto { get; set; }
        public object IdFormConfiguracion { get; set; }
        public object IdPuc { get; set; }
        public string SaldoInicial { get; set; }
        public string MovDebito { get; set; }
        public string MovCredito { get; set; }
        public string SaldoFinal { get; set; }
        public object AnioGravable { get; set; }
        public object IdPeriodicidad { get; set; }
        public object CodigoImpuesto { get; set; }
        public object MesEf { get; set; }
        public object VersionEf { get; set; }
        public object CodigoCuenta { get; set; }
        public int IdEstado { get; set; }
        public string TipoEjecucion { get; set; }
        public object IdFirmante1 { get; set; }
        public object IdFirmante2 { get; set; }
        public int IdUsuario { get; set; }
        public string ArrayData { get; set; }
        public string FechaInicial { get; set; }
        public int TipoConsulta { get; set; }
        public int TipoProceso { get; set; }
        #endregion

        #region PROCESO DE DB PARA BASE GRAVABLE
        public DataTable GetEstablecimientosCliente()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtEstablecimientos";
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_cliente_establecimiento", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestablecimiento", IdEstablecimientoPadre);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_codigo_oficina", null);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_nombre_oficina", null);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("idcliente_establecimiento");
                    TablaDatos.Columns.Add("id_municipio");
                    TablaDatos.Columns.Add("codigo_dane");

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idcliente_establecimiento"] = TheDataReaderPostgreSQL["idcliente_establecimiento"].ToString().Trim();
                            Fila["id_municipio"] = TheDataReaderPostgreSQL["id_municipio"].ToString().Trim();
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
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
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                FixedData.LogApi.Error("Error al obtener los datos de la Tabla [tbl_cliente_establecimiento]. Motivo: " + ex.Message);
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

        public DataTable GetBaseGravable()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtBaseGravable";
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_base_gravable", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente_establecimiento", IdClienteEstablecimiento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion", IdFormConfiguracion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idpuc", IdPuc);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mes_ef", MesEf);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("numero_renglon", typeof(Int32));
                    TablaDatos.Columns.Add("id_puc", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_cuenta");
                    TablaDatos.Columns.Add("saldo_inicial");
                    TablaDatos.Columns.Add("mov_debito");
                    TablaDatos.Columns.Add("mov_credito");
                    TablaDatos.Columns.Add("saldo_final");
                    TablaDatos.Columns.Add("valor_extracontable", typeof(Double));

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["numero_renglon"] = Convert.ToInt32(TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim());
                            Fila["id_puc"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_puc"].ToString().Trim());
                            Fila["codigo_cuenta"] = TheDataReaderPostgreSQL["codigo_cuenta"].ToString().Trim();

                            Fila["saldo_inicial"] = TheDataReaderPostgreSQL["saldo_inicial"].ToString().Trim();
                            Fila["mov_debito"] = TheDataReaderPostgreSQL["mov_debito"].ToString().Trim();
                            Fila["mov_credito"] = TheDataReaderPostgreSQL["mov_credito"].ToString().Trim();
                            Fila["saldo_final"] = TheDataReaderPostgreSQL["saldo_final"].ToString().Trim();
                            Fila["valor_extracontable"] = Convert.ToDouble(TheDataReaderPostgreSQL["valor_extracontable"].ToString().Trim());
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
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                FixedData.LogApi.Error("Error al obtener los datos de la Tabla [tbl_base_gravable_" + AnioGravable + "]. Motivo: " + ex.Message);
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
                if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
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

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DE LA CONSULTA
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
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return _ArrayDatos;
                }
            }
            catch (Exception ex)
            {
                FixedData.LogApi.Error("Error al obtener los datos de la Tabla [tbl_estado_financiero]. Motivo: " + ex.Message);
            }
            finally
            {
                #region OBJETO DE FINALIZACION CONEXION A LA DB
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

        public bool AddLoadBaseGravable(ref int _IdRegistro, ref string _MsgError)
        {
            bool retValor = false;
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_load_estado_financiero", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcleinte_ef", IdClienteEF);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mes_ef", MesEf);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_version_ef", VersionEf);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_row_ef", string.Format("{{{0}}}", string.Join(",", ArrayData)));
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    PgSqlParameter _IdRegRetorno = new PgSqlParameter("@p_out_id_registro", SqlDbType.Int);
                    PgSqlParameter _CodRptaRetorno = new PgSqlParameter("@p_out_cod_rpta", SqlDbType.VarChar);
                    PgSqlParameter _MsgRptaRetorno = new PgSqlParameter("@p_out_msg_rpta", SqlDbType.VarChar);

                    //--AQUI OBTENEMOS LOS PARAMETROS DE RETORNOS
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
                    FixedData.LogApi.Error(_MsgError);
                    return false;
                }
            }
            catch (Exception ex)
            {
                retValor = false;
                _MsgError = "Error al cargar la base gravable. Motivo: " + ex.Message.ToString().Trim();
                FixedData.LogApi.Error(_MsgError.ToString().Trim());
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
        #endregion

        #region PROCESO DE DB PARA LIQUIDACION DE IMPUESTOS
        public DataTable GetConsultarDatos(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtConsultarDatos";
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_task_get_consulta_liq_impuesto", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_iddpto", IdDepartamento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE CAMPOS
                    TablaDatos.Columns.Add("idcliente_establecimiento", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_oficina");
                    TablaDatos.Columns.Add("iddpto_oficina", typeof(Int32));
                    TablaDatos.Columns.Add("dpto_oficina");
                    TablaDatos.Columns.Add("idmun_oficina", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("municipio_oficina");
                    TablaDatos.Columns.Add("numero_puntos", typeof(Int32));
                    TablaDatos.Columns.Add("id_cliente", typeof(Int32));
                    TablaDatos.Columns.Add("idtipo_sector", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_cliente");
                    TablaDatos.Columns.Add("idtipo_identificacion", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_identificacion");
                    TablaDatos.Columns.Add("numero_documento");
                    TablaDatos.Columns.Add("digito_verificacion");
                    TablaDatos.Columns.Add("consorcio_union_temporal");
                    TablaDatos.Columns.Add("actividad_patrim_autonomo");
                    TablaDatos.Columns.Add("iddpto_cliente", typeof(Int32));
                    TablaDatos.Columns.Add("dpto_cliente");
                    TablaDatos.Columns.Add("idmun_cliente", typeof(Int32));
                    TablaDatos.Columns.Add("municipio_cliente");
                    TablaDatos.Columns.Add("direccion_cliente");
                    TablaDatos.Columns.Add("telefono_contacto");
                    TablaDatos.Columns.Add("email_contacto");
                    TablaDatos.Columns.Add("idtipo_clasificacion", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_clasificacion");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DE LA CONSULTA
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idcliente_establecimiento"] = Int32.Parse(TheDataReaderPostgreSQL["idcliente_establecimiento"].ToString().Trim());
                            Fila["nombre_oficina"] = TheDataReaderPostgreSQL["nombre_oficina"].ToString().Trim();
                            Fila["idmun_oficina"] = Int32.Parse(TheDataReaderPostgreSQL["idmun_oficina"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["municipio_oficina"] = TheDataReaderPostgreSQL["municipio_oficina"].ToString().Trim();
                            Fila["iddpto_oficina"] = Int32.Parse(TheDataReaderPostgreSQL["iddpto_oficina"].ToString().Trim());
                            Fila["dpto_oficina"] = TheDataReaderPostgreSQL["dpto_oficina"].ToString().Trim();
                            Fila["numero_puntos"] = Int32.Parse(TheDataReaderPostgreSQL["numero_puntos"].ToString().Trim());
                            Fila["id_cliente"] = Int32.Parse(TheDataReaderPostgreSQL["id_cliente"].ToString().Trim());
                            Fila["idtipo_sector"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_sector"].ToString().Trim());
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
                else
                {
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    FixedData.LogApi.Error(_MsgError);
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                _MsgError = "Error al obtener los datos de la Tabla [tbl_cliente]. Motivo: " + ex.Message;
                FixedData.LogApi.Error(_MsgError);
            }
            finally
            {
                #region OBJETO DE FINALIZACION CONEXION A LA DB
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
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                    TheDataReaderSQLServer.Close();
                    TheDataReaderSQLServer = null;
                }
                myConnectionDb.Close();
                myConnectionDb.Dispose();
                #endregion
            }

            return TablaDatos;
        }

        public DataTable GetBaseGravableImpuesto()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtBaseGravable";
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_base_gravable", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente_establecimiento", IdClienteEstablecimiento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion", IdFormConfiguracion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idpuc", IdPuc);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mes_ef", MesEf);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region AQUI DEFINIMOS LAS COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idbase_gravable", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon8");
                    TablaDatos.Columns.Add("valor_renglon9");
                    TablaDatos.Columns.Add("valor_renglon10");
                    TablaDatos.Columns.Add("valor_renglon11");
                    TablaDatos.Columns.Add("valor_renglon12");
                    TablaDatos.Columns.Add("valor_renglon13");
                    TablaDatos.Columns.Add("valor_renglon14");
                    TablaDatos.Columns.Add("valor_renglon15");
                    TablaDatos.Columns.Add("valor_renglon16");
                    TablaDatos.Columns.Add("valor_renglon26");
                    TablaDatos.Columns.Add("valor_renglon27");
                    TablaDatos.Columns.Add("valor_renglon28");
                    TablaDatos.Columns.Add("valor_renglon29");
                    TablaDatos.Columns.Add("valor_renglon30");
                    TablaDatos.Columns.Add("valor_renglon31");
                    TablaDatos.Columns.Add("valor_renglon32");
                    TablaDatos.Columns.Add("valor_renglon33");
                    TablaDatos.Columns.Add("valor_renglon34");
                    TablaDatos.Columns.Add("valor_renglon35");
                    TablaDatos.Columns.Add("valor_renglon36");
                    TablaDatos.Columns.Add("valor_renglon37");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idbase_gravable"] = Int32.Parse(TheDataReaderPostgreSQL["idbase_gravable"].ToString().Trim());
                            Fila["valor_renglon8"] = TheDataReaderPostgreSQL["valor_renglon8"].ToString().Trim();
                            Fila["valor_renglon9"] = TheDataReaderPostgreSQL["valor_renglon9"].ToString().Trim();
                            Fila["valor_renglon10"] = TheDataReaderPostgreSQL["valor_renglon10"].ToString().Trim();
                            Fila["valor_renglon11"] = TheDataReaderPostgreSQL["valor_renglon11"].ToString().Trim();
                            Fila["valor_renglon12"] = TheDataReaderPostgreSQL["valor_renglon12"].ToString().Trim();
                            Fila["valor_renglon13"] = TheDataReaderPostgreSQL["valor_renglon13"].ToString().Trim();
                            Fila["valor_renglon14"] = TheDataReaderPostgreSQL["valor_renglon14"].ToString().Trim();
                            Fila["valor_renglon15"] = TheDataReaderPostgreSQL["valor_renglon15"].ToString().Trim();
                            Fila["valor_renglon16"] = TheDataReaderPostgreSQL["valor_renglon16"].ToString().Trim();
                            Fila["valor_renglon26"] = TheDataReaderPostgreSQL["valor_renglon26"].ToString().Trim();
                            Fila["valor_renglon27"] = TheDataReaderPostgreSQL["valor_renglon27"].ToString().Trim();
                            Fila["valor_renglon28"] = TheDataReaderPostgreSQL["valor_renglon28"].ToString().Trim();
                            Fila["valor_renglon29"] = TheDataReaderPostgreSQL["valor_renglon29"].ToString().Trim();
                            Fila["valor_renglon30"] = TheDataReaderPostgreSQL["valor_renglon30"].ToString().Trim();
                            Fila["valor_renglon31"] = TheDataReaderPostgreSQL["valor_renglon31"].ToString().Trim();
                            Fila["valor_renglon32"] = TheDataReaderPostgreSQL["valor_renglon32"].ToString().Trim();
                            Fila["valor_renglon33"] = TheDataReaderPostgreSQL["valor_renglon33"].ToString().Trim();
                            Fila["valor_renglon34"] = TheDataReaderPostgreSQL["valor_renglon34"].ToString().Trim();
                            Fila["valor_renglon35"] = TheDataReaderPostgreSQL["valor_renglon35"].ToString().Trim();
                            Fila["valor_renglon36"] = TheDataReaderPostgreSQL["valor_renglon36"].ToString().Trim();
                            Fila["valor_renglon37"] = TheDataReaderPostgreSQL["valor_renglon37"].ToString().Trim();
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
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                FixedData.LogApi.Error("Error al obtener los datos de la Tabla [tbl_cliente_base_gravable]. Motivo: " + ex.Message);
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

        public DataTable GetConsultarActEconomica()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtConsultarActEconomica";
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
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
                    TablaDatos.Columns.Add("idestab_act_economica", typeof(Int32));
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
                            Fila["idestab_act_economica"] = Int32.Parse(TheDataReaderPostgreSQL["idestab_act_economica"].ToString().Trim());
                            Fila["codigo_actividad"] = TheDataReaderPostgreSQL["codigo_actividad"].ToString().Trim();
                            Fila["codigo_cuenta"] = TheDataReaderPostgreSQL["codigo_cuenta"].ToString().Trim();

                            Fila["idtipo_tarifa"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_tarifa"].ToString().Trim());
                            Fila["tipo_tarifa"] = TheDataReaderPostgreSQL["descripcion_tarifa"].ToString().Trim();

                            Fila["idtipo_calculo"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_calculo"].ToString().Trim());
                            Fila["tarifa_ley"] = Double.Parse(TheDataReaderPostgreSQL["tarifa_ley"].ToString().Trim());
                            Fila["tarifa_municipio"] = Double.Parse(TheDataReaderPostgreSQL["tarifa_municipio"].ToString().Trim());

                            Fila["saldo_inicial"] = Double.Parse(TheDataReaderPostgreSQL["saldo_inicial"].ToString().Trim());
                            Fila["mov_debito"] = Double.Parse(TheDataReaderPostgreSQL["mov_debito"].ToString().Trim());
                            Fila["mov_credito"] = Double.Parse(TheDataReaderPostgreSQL["mov_credito"].ToString().Trim());
                            double _SaldoFinal = Double.Parse(TheDataReaderPostgreSQL["saldo_final"].ToString().Trim());
                            Fila["saldo_final"] = _SaldoFinal;
                            Fila["saldo_final2"] = String.Format(String.Format("{0:$ ###,###,##0}", _SaldoFinal));
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
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                FixedData.LogApi.Error("Error al obtener los datos de la Tabla [tbl_cliente_estab_act_economica]. Motivo: " + ex.Message);
            }
            finally
            {
                #region OBJETO DE FINALIZACION CONEXION A LA DB
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
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                    TheDataReaderSQLServer.Close();
                    TheDataReaderSQLServer = null;
                }
                myConnectionDb.Close();
                myConnectionDb.Dispose();
                #endregion
            }

            return TablaDatos;
        }

        public DataTable GetImpuestosMunicipio()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtImpuestosMunicipio";
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
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
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region AQUI DEFINIMOS LAS COLUMNAS DEL DATATABLE
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
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                FixedData.LogApi.Error("Error al obtener los datos de la Tabla [tbl_cliente_estab_act_economica]. Motivo: " + ex.Message);
            }
            finally
            {
                #region OBJETO DE FINALIZACION CONEXION A LA DB
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
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                    TheDataReaderSQLServer.Close();
                    TheDataReaderSQLServer = null;
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
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
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
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                FixedData.LogApi.Error("Error al obtener los datos de la Tabla [tbl_cliente_estab_act_economica]. Motivo: " + ex.Message);
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
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_fecha_calendario", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcalendario_trib", IdMunCalendarioTrib);
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
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return _Result;
                }
            }
            catch (Exception ex)
            {
                FixedData.LogApi.Error("Error al obtener los datos de la Tabla [tbl_cliente_estab_act_economica]. Motivo: " + ex.Message);
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

        public bool GetProcesarImpuestos(ref int _IdRegistro, ref string _MsgError)
        {
            bool retValor = false;
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_task_liquidacion_impuesto", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idformulario_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_row_liquidacion", string.Format("{{{0}}}", string.Join(",", ArrayData)));
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_ejecucion", TipoEjecucion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idfirmante1", IdFirmante1);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idfirmante2", IdFirmante2);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    PgSqlParameter _CodRptaRetorno = new PgSqlParameter("@p_out_cod_rpta", SqlDbType.VarChar);
                    PgSqlParameter _MsgRptaRetorno = new PgSqlParameter("@p_out_msg_rpta", SqlDbType.VarChar);

                    //asignamos los parametros de retornos.
                    _CodRptaRetorno.Direction = ParameterDirection.Output;
                    _MsgRptaRetorno.Direction = ParameterDirection.Output;
                    TheCommandPostgreSQL.Parameters.Add(_CodRptaRetorno);
                    TheCommandPostgreSQL.Parameters.Add(_MsgRptaRetorno);

                    object ObjResult = new object();
                    ObjResult = TheCommandPostgreSQL.ExecuteScalar();
                    if (ObjResult != null)
                    {
                        if (ObjResult.ToString().Trim().Equals("00"))
                        {
                            Transac.Commit();
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
                    FixedData.LogApi.Error(_MsgError);
                    return false;
                }
            }
            catch (Exception ex)
            {
                retValor = false;
                _MsgError = "Error al procesar la liquidacion del impuesto [" + IdFormularioImpuesto + "]. Motivo: " + ex.Message.ToString().Trim();
                FixedData.LogApi.Error(_MsgError.ToString().Trim());
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

        public bool GetProcesarLiquidacionLote(ref int _IdRegistro, ref string _MsgError)
        {
            bool retValor = false;
            try
            {
                #region DEFINICION OBJETO DE CONEXION A LA DB.
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((FixedData.MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    FixedData.LogApi.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
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

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idliquidacion_lote", null);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_row_liquidacion", string.Format("{{{0}}}", string.Join(",", ArrayData)));
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_aprobacion_jefe", "");
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_observacion", "");
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
                    FixedData.LogApi.Error(_MsgError);
                    return false;
                }
            }
            catch (Exception ex)
            {
                retValor = false;
                _MsgError = "Error al cargar la liquidacion x lote. Motivo: " + ex.Message.ToString().Trim();
                FixedData.LogApi.Error(_MsgError.ToString().Trim());
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
        #endregion

    }
}