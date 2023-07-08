using Devart.Data.PostgreSql;
using log4net;
using MySql.Data.MySqlClient;
using Smartax.Web.Application.Clases.Seguridad;
using System;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Text;

namespace Smartax.Web.Application.Clases.Reportes.Provision
{
    public class RegistroContableProv
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_report_registro_contable_prov_ica", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_id_cliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_vigencia", Vigencia);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_periodo_impuesto", PeriodoImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_formulario_impuesto", FormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("Concepto Provision");
                    TablaDatos.Columns.Add("Cuenta Contable");
                    TablaDatos.Columns.Add("Nombre Cuenta");
                    TablaDatos.Columns.Add("Concepto Asiento");
                    TablaDatos.Columns.Add("Código Oficina");
                    TablaDatos.Columns.Add("Nombre Oficina");
                    TablaDatos.Columns.Add("Código DANE");
                    TablaDatos.Columns.Add("Nombre Municipio");
                    TablaDatos.Columns.Add("NIT");
                    TablaDatos.Columns.Add("DV");
                    TablaDatos.Columns.Add("Departamento");
                    TablaDatos.Columns.Add("Débito");
                    TablaDatos.Columns.Add("Crédito");
                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["Concepto Provision"] = TheDataReaderPostgreSQL["concepto_asiento"].ToString().Trim();
                            Fila["Cuenta Contable"] = TheDataReaderPostgreSQL["codigo_cuenta"].ToString().Trim();
                            Fila["Nombre Cuenta"] = TheDataReaderPostgreSQL["nombre_cuenta"].ToString().Trim();
                            Fila["Concepto Asiento"] = TheDataReaderPostgreSQL["concepto_asiento"].ToString().Trim();
                            Fila["Código Oficina"] = TheDataReaderPostgreSQL["codigo_oficina"].ToString().Trim();
                            Fila["Nombre Oficina"] = TheDataReaderPostgreSQL["nombre_oficina"].ToString().Trim();
                            Fila["Código DANE"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["Nombre Municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["NIT"] = TheDataReaderPostgreSQL["numero_nit"].ToString().Trim();
                            Fila["DV"] = TheDataReaderPostgreSQL["digito_verificacion"].ToString().Trim();
                            Fila["Departamento"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["Débito"] = TheDataReaderPostgreSQL["debito"].ToString().Trim();
                            Fila["Crédito"] = TheDataReaderPostgreSQL["credito"].ToString().Trim();
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
    }
}