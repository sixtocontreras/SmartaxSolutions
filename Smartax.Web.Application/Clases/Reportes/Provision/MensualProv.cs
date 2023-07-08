using Devart.Data.PostgreSql;
using log4net;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Text;
using Smartax.Web.Application.Clases.Seguridad;
using Smartax.Web.Application.Clases.Administracion;

namespace Smartax.Web.Application.Clases.Reportes.Provision
{
    public class MensualProv
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_report_mensual_prov_ica", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_id_cliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_vigencia", Vigencia);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_periodo_impuesto", PeriodoImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_formulario_impuesto", FormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("Número Oficina");
                    TablaDatos.Columns.Add("Nombre Oficina");
                    TablaDatos.Columns.Add("Departamento");
                    TablaDatos.Columns.Add("Código Dane");
                    TablaDatos.Columns.Add("Municipio");
                    TablaDatos.Columns.Add("NIT");
                    TablaDatos.Columns.Add("DV");
                    TablaDatos.Columns.Add("Total Ingresos Ordinarios y Extraordinarios del Municipio");
                    TablaDatos.Columns.Add("Ingresos por Devoluciones, Rebajas y Descuentos");
                    TablaDatos.Columns.Add("Ingresos por Exportación");
                    TablaDatos.Columns.Add("Ingresos por Venta de Activos Fijos");
                    TablaDatos.Columns.Add("Ingresos por Actividades Excluidas o No Sujetas y No Gravados");
                    TablaDatos.Columns.Add("Ingreso por Otras Actividades Exentas en Este Municipio");
                    TablaDatos.Columns.Add("Base Gravable ICA");
                    TablaDatos.Columns.Add("Tarifa de ICA");
                    TablaDatos.Columns.Add("Industria y Comercio");
                    TablaDatos.Columns.Add("Avisos y Tableros");
                    TablaDatos.Columns.Add("Unidades Adicionales (Sector Financiero)");
                    TablaDatos.Columns.Add("Base Gravable Sobretasa Bomberil");
                    TablaDatos.Columns.Add("Sobretasa Bomberil");
                    TablaDatos.Columns.Add("Base Gravable Sobretasa de Seguridad");
                    TablaDatos.Columns.Add("Sobretasa de Seguridad");
                    TablaDatos.Columns.Add("Total Impuesto Provisionado");

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            int _Nivel = Int32.Parse(TheDataReaderPostgreSQL["nivel"].ToString().Trim());
                            int _IdMunicipio = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            //--
                            if (_Nivel == 1)
                            {
                                Fila["Número Oficina"] = TheDataReaderPostgreSQL["Numero_Oficina"].ToString().Trim();
                                Fila["Nombre Oficina"] = TheDataReaderPostgreSQL["Nombre_Oficina"].ToString().Trim();
                            }
                            else
                            {
                                ClienteEstablecimiento objOficina = new ClienteEstablecimiento();
                                objOficina.TipoConsulta = 9;
                                objOficina.IdMunicipio = _IdMunicipio;
                                objOficina.TipoConsulta = 9;
                                objOficina.MotorBaseDatos = MotorBaseDatos.ToString().Trim();
                                //--
                                string _CodigoOficina = "", _NombreOficina = "";
                                bool _Result = objOficina.GetDataOficina(ref _CodigoOficina, ref _NombreOficina);
                                //--
                                Fila["Número Oficina"] = _CodigoOficina;
                                Fila["Nombre Oficina"] = _NombreOficina;
                            }
                            //--
                            Fila["Departamento"] = TheDataReaderPostgreSQL["Departamento"].ToString().Trim();
                            Fila["Código Dane"] = TheDataReaderPostgreSQL["Codigo_Dane"].ToString().Trim();
                            Fila["Municipio"] = TheDataReaderPostgreSQL["Municipio"].ToString().Trim();
                            Fila["NIT"] = TheDataReaderPostgreSQL["NIT"].ToString().Trim();
                            Fila["DV"] = TheDataReaderPostgreSQL["DV"].ToString().Trim();
                            Fila["Total Ingresos Ordinarios y Extraordinarios del Municipio"] = TheDataReaderPostgreSQL["Total_ingresos_ordinarios_y_extraordinarios_del_municipio"].ToString().Trim();
                            Fila["Ingresos por Devoluciones, Rebajas y Descuentos"] = TheDataReaderPostgreSQL["Ingresos_por_devoluciones_rebajas_Descuentos"].ToString().Trim();
                            Fila["Ingresos por Exportación"] = TheDataReaderPostgreSQL["Ingresos_por_exportacion"].ToString().Trim();
                            Fila["Ingresos por Venta de Activos Fijos"] = TheDataReaderPostgreSQL["Ingresos_por_venta_de_activos_fijos"].ToString().Trim();
                            Fila["Ingresos por Actividades Excluidas o No Sujetas y No Gravados"] = TheDataReaderPostgreSQL["Ingresos_por_actividades_excluidas_o_no_gravadas"].ToString().Trim();
                            Fila["Ingreso por Otras Actividades Exentas en Este Municipio"] = TheDataReaderPostgreSQL["Ingreso_por_otras_actividades_exentas_en_este_municipio"].ToString().Trim();
                            Fila["Base Gravable ICA"] = TheDataReaderPostgreSQL["Base_Gravable_ICA"].ToString().Trim();
                            Fila["Tarifa de ICA"] = TheDataReaderPostgreSQL["Tarifa_de_ICA"].ToString().Trim();
                            Fila["Industria y Comercio"] = TheDataReaderPostgreSQL["Industria_y_Comercio"].ToString().Trim();
                            Fila["Avisos y Tableros"] = TheDataReaderPostgreSQL["Avisos_y_tableros"].ToString().Trim();
                            Fila["Unidades Adicionales (Sector Financiero)"] = TheDataReaderPostgreSQL["Unidades_Adicionales"].ToString().Trim();
                            Fila["Base Gravable Sobretasa Bomberil"] = TheDataReaderPostgreSQL["Base_Gravable_Sobretasa"].ToString().Trim();
                            Fila["Sobretasa Bomberil"] = TheDataReaderPostgreSQL["Sobretasa_Bomberil"].ToString().Trim();
                            Fila["Base Gravable Sobretasa de Seguridad"] = TheDataReaderPostgreSQL["Base_Gravable_Sobretasa_a_la_seguridad"].ToString().Trim();
                            Fila["Sobretasa de Seguridad"] = TheDataReaderPostgreSQL["Sobretasa_a_la_seguridad"].ToString().Trim();
                            Fila["Total Impuesto Provisionado"] = TheDataReaderPostgreSQL["Total_Impuesto"].ToString().Trim();
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