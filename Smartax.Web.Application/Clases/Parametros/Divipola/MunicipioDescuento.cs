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
    public class MunicipioDescuento
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
        public object IdMunDescuento { get; set; }
        public object IdMunicipio { get; set; }
        public object IdCliente { get; set; }
        public object IdFormularioImpuesto { get; set; }
        public int AnioGravable { get; set; }
        public string DescripcionDescuento { get; set; }
        public object IdFormConfiguracion1 { get; set; }
        public object IdTipoOperacion1 { get; set; }
        public object IdFormConfiguracion2 { get; set; }
        public object IdTipoOperacion2 { get; set; }
        public object IdFormConfiguracion3 { get; set; }
        public object IdTipoOperacion3 { get; set; }
        public object IdFormConfiguracion4 { get; set; }
        public object IdTipoOperacion4 { get; set; }
        public object IdFormConfiguracion5 { get; set; }
        public object IdTipoOperacion5 { get; set; }
        public object IdFormConfiguracion6 { get; set; }
        public object IdTipoOperacion6 { get; set; }
        public object IdFormConfiguracion7 { get; set; }
        public object IdTipoOperacion7 { get; set; }
        public object IdFormConfiguracion8 { get; set; }
        public object IdTipoOperacion8 { get; set; }
        public object IdFormConfiguracion9 { get; set; }
        public object IdTipoOperacion9 { get; set; }
        public object IdFormConfiguracion10 { get; set; }
        public object IdEstado { get; set; }
        public object IdUsuario { get; set; }
        public string MostrarSeleccione { get; set; }
        public string MotorBaseDatos { get; set; }
        public int TipoConsulta { get; set; }
        public int TipoProceso { get; set; }
        #endregion

        public DataTable GetAllMunicioDescuento()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtDescuentos";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_municipio_descuento", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmun_descuento", IdMunDescuento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idmun_descuento", typeof(Int32));
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("descripcion_formulario");
                    TablaDatos.Columns.Add("anio_gravable", typeof(Int32));
                    TablaDatos.Columns.Add("descripcion_descuento");
                    TablaDatos.Columns.Add("numero_renglon", typeof(Int32));
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idmun_descuento"] = Convert.ToInt32(TheDataReaderPostgreSQL["idmun_descuento"].ToString().Trim());
                            Fila["idformulario_impuesto"] = Convert.ToInt32(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["descripcion_formulario"] = TheDataReaderPostgreSQL["descripcion_formulario"].ToString().Trim();
                            Fila["anio_gravable"] = Convert.ToInt32(TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim());
                            Fila["descripcion_descuento"] = TheDataReaderPostgreSQL["descripcion_descuento"].ToString().Trim();
                            Fila["numero_renglon"] = Convert.ToInt32(TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim());
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_municipio_descuento]. Motivo: " + ex.Message);
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

        public DataTable GetInfoDescuento()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtDescuentos";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_municipio_descuento", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmun_descuento", IdMunDescuento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idmun_descuento", typeof(Int32));
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("anio_gravable", typeof(Int32));
                    TablaDatos.Columns.Add("descripcion_descuento");
                    TablaDatos.Columns.Add("idform_configuracion1");
                    TablaDatos.Columns.Add("idtipo_operacion1");
                    TablaDatos.Columns.Add("idform_configuracion2");
                    TablaDatos.Columns.Add("idtipo_operacion2");
                    TablaDatos.Columns.Add("idform_configuracion3");
                    TablaDatos.Columns.Add("idtipo_operacion3");
                    TablaDatos.Columns.Add("idform_configuracion4");
                    TablaDatos.Columns.Add("idtipo_operacion4");
                    TablaDatos.Columns.Add("idform_configuracion5");
                    TablaDatos.Columns.Add("idtipo_operacion5");
                    TablaDatos.Columns.Add("idform_configuracion6");
                    TablaDatos.Columns.Add("idtipo_operacion6");
                    TablaDatos.Columns.Add("idform_configuracion7");
                    TablaDatos.Columns.Add("idtipo_operacion7");
                    TablaDatos.Columns.Add("idform_configuracion8");
                    TablaDatos.Columns.Add("idtipo_operacion8");
                    TablaDatos.Columns.Add("idform_configuracion9");
                    TablaDatos.Columns.Add("idtipo_operacion9");
                    TablaDatos.Columns.Add("idform_configuracion10");
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DEL DATA READER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idmun_descuento"] = Convert.ToInt32(TheDataReaderPostgreSQL["idmun_descuento"].ToString().Trim());
                            Fila["idformulario_impuesto"] = Convert.ToInt32(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["anio_gravable"] = Convert.ToInt32(TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim());
                            Fila["descripcion_descuento"] = TheDataReaderPostgreSQL["descripcion_descuento"].ToString().Trim();
                            //--
                            Fila["idform_configuracion1"] = TheDataReaderPostgreSQL["idform_configuracion1"].ToString().Trim();
                            Fila["idtipo_operacion1"] = TheDataReaderPostgreSQL["idtipo_operacion1"].ToString().Trim();
                            Fila["idform_configuracion2"] = TheDataReaderPostgreSQL["idform_configuracion2"].ToString().Trim();
                            Fila["idtipo_operacion2"] = TheDataReaderPostgreSQL["idtipo_operacion2"].ToString().Trim();
                            Fila["idform_configuracion3"] = TheDataReaderPostgreSQL["idform_configuracion3"].ToString().Trim();
                            Fila["idtipo_operacion3"] = TheDataReaderPostgreSQL["idtipo_operacion3"].ToString().Trim();
                            Fila["idform_configuracion4"] = TheDataReaderPostgreSQL["idform_configuracion4"].ToString().Trim();
                            Fila["idtipo_operacion4"] = TheDataReaderPostgreSQL["idtipo_operacion4"].ToString().Trim();
                            Fila["idform_configuracion5"] = TheDataReaderPostgreSQL["idform_configuracion5"].ToString().Trim();
                            Fila["idtipo_operacion5"] = TheDataReaderPostgreSQL["idtipo_operacion5"].ToString().Trim();
                            Fila["idform_configuracion6"] = TheDataReaderPostgreSQL["idform_configuracion6"].ToString().Trim();
                            Fila["idtipo_operacion6"] = TheDataReaderPostgreSQL["idtipo_operacion6"].ToString().Trim();
                            Fila["idform_configuracion7"] = TheDataReaderPostgreSQL["idform_configuracion7"].ToString().Trim();
                            Fila["idtipo_operacion7"] = TheDataReaderPostgreSQL["idtipo_operacion7"].ToString().Trim();
                            Fila["idform_configuracion8"] = TheDataReaderPostgreSQL["idform_configuracion8"].ToString().Trim();
                            Fila["idtipo_operacion8"] = TheDataReaderPostgreSQL["idtipo_operacion8"].ToString().Trim();
                            Fila["idform_configuracion9"] = TheDataReaderPostgreSQL["idform_configuracion9"].ToString().Trim();
                            Fila["idtipo_operacion9"] = TheDataReaderPostgreSQL["idtipo_operacion9"].ToString().Trim();
                            Fila["idform_configuracion10"] = TheDataReaderPostgreSQL["idform_configuracion10"].ToString().Trim();
                            Fila["id_estado"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_municipio_descuento]. Motivo: " + ex.Message);
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

        public DataTable GetDetalleDescuento()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtDescuentos";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_municipio_descuento", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmun_descuento", IdMunDescuento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
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
                    TablaDatos.Columns.Add("idtipo_operacion6");
                    TablaDatos.Columns.Add("numero_renglon7");
                    TablaDatos.Columns.Add("idtipo_operacion7");
                    TablaDatos.Columns.Add("numero_renglon8");
                    TablaDatos.Columns.Add("idtipo_operacion8");
                    TablaDatos.Columns.Add("numero_renglon9");
                    TablaDatos.Columns.Add("idtipo_operacion9");
                    TablaDatos.Columns.Add("numero_renglon10");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DEL DATA READER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_municipio"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            //--
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
                            Fila["idtipo_operacion6"] = TheDataReaderPostgreSQL["idtipo_operacion6"].ToString().Trim();
                            Fila["numero_renglon7"] = TheDataReaderPostgreSQL["numero_renglon7"].ToString().Trim();
                            Fila["idtipo_operacion7"] = TheDataReaderPostgreSQL["idtipo_operacion7"].ToString().Trim();
                            Fila["numero_renglon8"] = TheDataReaderPostgreSQL["numero_renglon8"].ToString().Trim();
                            Fila["idtipo_operacion8"] = TheDataReaderPostgreSQL["idtipo_operacion8"].ToString().Trim();
                            Fila["numero_renglon9"] = TheDataReaderPostgreSQL["numero_renglon9"].ToString().Trim();
                            Fila["idtipo_operacion9"] = TheDataReaderPostgreSQL["idtipo_operacion9"].ToString().Trim();
                            Fila["numero_renglon10"] = TheDataReaderPostgreSQL["numero_renglon10"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_municipio_descuento]. Motivo: " + ex.Message);
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

        public bool AddUpDescuento( ref int _IdRegistro, ref string _MsgError)
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_municipio_descuento", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmun_descuento", IdMunDescuento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idformulario_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_descripcion_descuento", DescripcionDescuento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion1", IdFormConfiguracion1);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion1", IdTipoOperacion1);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion2", IdFormConfiguracion2);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion2", IdTipoOperacion2);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion3", IdFormConfiguracion3);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion3", IdTipoOperacion3);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion4", IdFormConfiguracion4);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion4", IdTipoOperacion4);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion5", IdFormConfiguracion5);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion5", IdTipoOperacion5);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion6", IdFormConfiguracion6);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion6", IdTipoOperacion6);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion7", IdFormConfiguracion7);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion7", IdTipoOperacion7);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion8", IdFormConfiguracion8);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion8", IdTipoOperacion8);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion9", IdFormConfiguracion9);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_operacion9", IdTipoOperacion9);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_configuracion10", IdFormConfiguracion10);
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
                _MsgError = "Error al registrar el descuento de pronto pago. Motivo: " + ex.Message.ToString().Trim();
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