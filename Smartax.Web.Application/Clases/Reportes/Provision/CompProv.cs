using Devart.Data.PostgreSql;
using log4net;
using MySql.Data.MySqlClient;
using Smartax.Web.Application.Clases.Seguridad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Smartax.Web.Application.Clases.Reportes.Provision
{
    public class CompProv
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
        public int Vigencia { get; set; }
        public int PeriodoImpuesto { get; set; }
        public int FormularioImpuesto { get; set; }
        public int IdUsuario { get; set; }
        public string MotorBaseDatos { get; set; }
        #endregion

        internal DataTable GetReport()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtRepCompProv";

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
                    #region OBTENER DATOS DE LA DB DE POSTGRES
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_report_comp_prov_ica", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_id_cliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_vigencia", Vigencia);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_periodo_impuesto", PeriodoImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_formulario_impuesto", FormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);

                    //var ss = new PgSqlDataAdapter(TheCommandPostgreSQL);
                    //ss.Fill(TablaDatos);

                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("Código Empresa");
                    TablaDatos.Columns.Add("Consecutivo del Comprobante");
                    TablaDatos.Columns.Add("Fecha Contabilización");
                    TablaDatos.Columns.Add("Oficina");
                    TablaDatos.Columns.Add("Centro de Costo");
                    TablaDatos.Columns.Add("Descripción Comprobante");
                    TablaDatos.Columns.Add("Asiento");
                    TablaDatos.Columns.Add("Cuenta");
                    TablaDatos.Columns.Add("Concepto del Asiento");
                    TablaDatos.Columns.Add("Débito");
                    TablaDatos.Columns.Add("Crédito");
                    TablaDatos.Columns.Add("Tipo de Identificación");
                    TablaDatos.Columns.Add("Número de Identificación");
                    TablaDatos.Columns.Add("Digito de Verificación");
                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["Código Empresa"] = TheDataReaderPostgreSQL["Codigo_empresa"].ToString().Trim();
                            Fila["Consecutivo del Comprobante"] = TheDataReaderPostgreSQL["Consecutivo_del_Comprobante"].ToString().Trim();
                            Fila["Fecha Contabilización"] = TheDataReaderPostgreSQL["Fecha_Contabilizacion"].ToString().Trim();
                            Fila["Oficina"] = TheDataReaderPostgreSQL["Oficina"].ToString().Trim();
                            Fila["Centro de Costo"] = TheDataReaderPostgreSQL["Centro_de_Costo"].ToString().Trim();
                            Fila["Descripción Comprobante"] = TheDataReaderPostgreSQL["Descripcion_Comprobante"].ToString().Trim();
                            Fila["Asiento"] = TheDataReaderPostgreSQL["Asiento"].ToString().Trim();
                            Fila["Cuenta"] = TheDataReaderPostgreSQL["Cuenta"].ToString().Trim();
                            Fila["Concepto del Asiento"] = TheDataReaderPostgreSQL["Concepto_del_Asiento"].ToString().Trim();
                            Fila["Débito"] = TheDataReaderPostgreSQL["Debito"].ToString().Trim();
                            Fila["Crédito"] = TheDataReaderPostgreSQL["Credito"].ToString().Trim();
                            Fila["Tipo de Identificación"] = TheDataReaderPostgreSQL["Tipo_de_identificacion"].ToString().Trim();
                            Fila["Número de Identificación"] = TheDataReaderPostgreSQL["Numero_de_identificacion"].ToString().Trim();
                            Fila["Digito de Verificación"] = TheDataReaderPostgreSQL["Digito_De_Verificacion"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_estado]. Motivo: " + ex.Message);
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
                    //TheDataReaderMySQL.Close();
                    //TheDataReaderMySQL = null;
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                    //TheDataReaderSQLServer.Close();
                    //TheDataReaderSQLServer = null;
                }
                else if (myConnectionDb is OracleConnection)
                {
                    TheCommandOracle = null;
                    //TheDataReaderOracle.Close();
                    //TheDataReaderOracle = null;
                }
                myConnectionDb.Close();
                myConnectionDb.Dispose();
                #endregion
            }

            return TablaDatos;
        }
    }
}