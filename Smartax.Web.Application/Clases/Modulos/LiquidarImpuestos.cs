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

namespace Smartax.Web.Application.Clases.Modulos
{
    public class LiquidarImpuestos
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
        public object IdLiquidImpuesto { get; set; }
        public object IdMunicipio { get; set; }
        public object IdFormularioImpuesto { get; set; }
        public object IdCliente { get; set; }
        public object IdClienteEstablecimiento { get; set; }
        public object CodigoDane { get; set; }
        public object AnioGravable { get; set; }
        public string MesImpuesto { get; set; }
        public string FechaMaxPresentacion { get; set; }
        public string FechaLiquidacion { get; set; }
        public string PeriodoImpuesto { get; set; }
        public object PeriodicidadImpuesto { get; set; }
        public object OpcionUso { get; set; }
        public string NumDeclaracion { get; set; }
        public string ValorRenglon8 { get; set; }
        public string ValorRenglon9 { get; set; }
        public string ValorRenglon10 { get; set; }
        public string ValorRenglon11 { get; set; }
        public string ValorRenglon12 { get; set; }
        public string ValorRenglon13 { get; set; }
        public string ValorRenglon14 { get; set; }
        public string ValorRenglon15 { get; set; }
        public string ValorRenglon16 { get; set; }
        public string ValorActividad1 { get; set; }
        public string ValorActividad2 { get; set; }
        public string ValorActividad3 { get; set; }
        public string ValorOtrasAct { get; set; }
        public string ValorRenglon17 { get; set; }
        public string ValorRenglon19 { get; set; }
        public string ValorRenglon20 { get; set; }
        public string ValorRenglon21 { get; set; }
        public string ValorRenglon22 { get; set; }
        public string ValorRenglon23 { get; set; }
        public string ValorRenglon24 { get; set; }
        public string ValorRenglon25 { get; set; }
        public string ValorRenglon26 { get; set; }
        public string ValorRenglon27 { get; set; }
        public string ValorRenglon28 { get; set; }
        public string ValorRenglon29 { get; set; }
        public string ValorRenglon30 { get; set; }
        public string TarifaIca { get; set; }
        public string BaseGravBomberil { get; set; }
        public string BaseGravSeguridad { get; set; }
        public string ValorRenglon31 { get; set; }
        public object Sanciones { get; set; }
        public string DescripcionSancionOtro { get; set; }
        public string ValorSancion { get; set; }
        public string ValorRenglon32 { get; set; }
        public string ValorRenglon33 { get; set; }
        public string ValorRenglon34 { get; set; }
        public string ValorRenglon35 { get; set; }
        public string ValorRenglon36 { get; set; }
        public string InteresMora { get; set; }
        public string ValorRenglon37 { get; set; }
        public string ValorRenglon38 { get; set; }
        public string ValorPagoVoluntario { get; set; }
        public object DestinoAportesVol { get; set; }
        public string ValorRenglon39 { get; set; }
        public string ValorRenglon40 { get; set; }
        public string ValorRenglon41 { get; set; }
        public string ValorRenglon42 { get; set; }
        public string ValorRenglon43 { get; set; }
        public string ValorRenglon44 { get; set; }
        public object IdFirmante1 { get; set; }
        public object IdFirmante2 { get; set; }
        public object IdEstado { get; set; }
        public object TipoEjecucion { get; set; }
        public object IdUsuario { get; set; }
        public string MotorBaseDatos { get; set; }
        public int TipoConsulta { get; set; }
        public int TipoProceso { get; set; }
        #endregion

        public DataTable GetConsultarLiquidacion()
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

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idliquid_impuesto", IdLiquidImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_codigo_dane", CodigoDane);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_periodicidad_impuesto", PeriodicidadImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE CAMPOS
                    //--NOTA IMPORTANTE: En esta consulta no se tuvo en cuenta los valores de los renglones 8, 9, 10, ..., etc
                    //--ya que estos datos los estamos obteniendo directamente del estado financiero.
                    TablaDatos.Columns.Add("idliquid_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("id_cliente", typeof(Int32));
                    TablaDatos.Columns.Add("idcliente_establecimiento", typeof(Int32));
                    TablaDatos.Columns.Add("anio_gravable");
                    TablaDatos.Columns.Add("periodo_impuesto");
                    if (Int32.Parse(IdFormularioImpuesto.ToString().Trim()) == FixedData.IDFORM_IMPUESTO_AUTORETE_ICA)
                    {
                        TablaDatos.Columns.Add("periodicidad_impuesto");
                    }
                    TablaDatos.Columns.Add("fecha_max_presentacion");
                    TablaDatos.Columns.Add("fecha_liquidacion");
                    TablaDatos.Columns.Add("opcion_uso", typeof(Int32));
                    TablaDatos.Columns.Add("num_declaracion");
                    TablaDatos.Columns.Add("sanciones");
                    TablaDatos.Columns.Add("descripcion_sancion_otro");
                    TablaDatos.Columns.Add("valor_sancion");
                    TablaDatos.Columns.Add("interes_mora");
                    if (Int32.Parse(IdFormularioImpuesto.ToString().Trim()) == FixedData.IDFORM_IMPUESTO_ICA)
                    {
                        TablaDatos.Columns.Add("valor_pago_voluntario");
                        TablaDatos.Columns.Add("destino_pago_voluntario");
                    }
                    TablaDatos.Columns.Add("idfirmante_1");
                    TablaDatos.Columns.Add("idfirmante_2");
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("estado_liquidacion");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DE LA CONSULTA
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idliquid_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idliquid_impuesto"].ToString().Trim());
                            Fila["id_cliente"] = Int32.Parse(TheDataReaderPostgreSQL["id_cliente"].ToString().Trim());
                            Fila["idcliente_establecimiento"] = Int32.Parse(TheDataReaderPostgreSQL["idcliente_establecimiento"].ToString().Trim());
                            Fila["anio_gravable"] = TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim();
                            Fila["periodo_impuesto"] = TheDataReaderPostgreSQL["periodo_impuesto"].ToString().Trim();
                            if (Int32.Parse(IdFormularioImpuesto.ToString().Trim()) == FixedData.IDFORM_IMPUESTO_AUTORETE_ICA)
                            {
                                Fila["periodicidad_impuesto"] = TheDataReaderPostgreSQL["periodicidad_impuesto"].ToString().Trim();
                            }
                            Fila["fecha_max_presentacion"] = TheDataReaderPostgreSQL["fecha_max_presentacion"].ToString().Trim();
                            Fila["fecha_liquidacion"] = TheDataReaderPostgreSQL["fecha_liquidacion"].ToString().Trim();
                            Fila["opcion_uso"] = Int32.Parse(TheDataReaderPostgreSQL["opcion_uso"].ToString().Trim());
                            Fila["num_declaracion"] = TheDataReaderPostgreSQL["num_declaracion"].ToString().Trim();
                            Fila["sanciones"] = TheDataReaderPostgreSQL["sanciones"].ToString().Trim();
                            Fila["descripcion_sancion_otro"] = TheDataReaderPostgreSQL["descripcion_sancion_otro"].ToString().Trim().Length > 0 ? TheDataReaderPostgreSQL["descripcion_sancion_otro"].ToString().Trim() : "";
                            Fila["valor_sancion"] = TheDataReaderPostgreSQL["valor_sancion"].ToString().Trim();
                            Fila["interes_mora"] = TheDataReaderPostgreSQL["interes_mora"].ToString().Trim();
                            if (Int32.Parse(IdFormularioImpuesto.ToString().Trim()) == FixedData.IDFORM_IMPUESTO_ICA)
                            {
                                Fila["valor_pago_voluntario"] = TheDataReaderPostgreSQL["valor_pago_voluntario"].ToString().Trim();
                                Fila["destino_pago_voluntario"] = TheDataReaderPostgreSQL["destino_pago_voluntario"].ToString().Trim().Length > 0 ? TheDataReaderPostgreSQL["destino_pago_voluntario"].ToString().Trim() : "";
                            }

                            Fila["idfirmante_1"] = TheDataReaderPostgreSQL["idfirmante_1"].ToString().Trim().Length > 0 ? TheDataReaderPostgreSQL["idfirmante_1"].ToString().Trim() : "";
                            Fila["idfirmante_2"] = TheDataReaderPostgreSQL["idfirmante_2"].ToString().Trim().Length > 0 ? TheDataReaderPostgreSQL["idfirmante_2"].ToString().Trim() : "";
                            Fila["id_estado"] = Int32.Parse(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["estado_liquidacion"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_tipo_menu]. Motivo: " + ex.Message);
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

        public DataTable GetAllLiquidacionForm()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtLiquidacionForm";
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

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idliquid_impuesto", IdLiquidImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_codigo_dane", CodigoDane);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_periodicidad_impuesto", PeriodicidadImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idliquid_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("id_cliente", typeof(Int32));
                    TablaDatos.Columns.Add("idcliente_establecimiento", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("anio_gravable", typeof(Int32));
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("periodo_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("descripcion_periodo");
                    TablaDatos.Columns.Add("idperiodicidad_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("periodicidad_impuesto");
                    TablaDatos.Columns.Add("opcion_uso", typeof(Int32));
                    TablaDatos.Columns.Add("descripcion_opcion_uso");
                    TablaDatos.Columns.Add("num_declaracion");
                    TablaDatos.Columns.Add("sanciones");
                    TablaDatos.Columns.Add("descripcion_sancion");

                    TablaDatos.Columns.Add("valor_sancion");
                    TablaDatos.Columns.Add("interes_mora");
                    TablaDatos.Columns.Add("total_pagar");
                    TablaDatos.Columns.Add("valor_pago_voluntario");
                    TablaDatos.Columns.Add("destino_pago_voluntario");

                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("estado_liquidacion");
                    TablaDatos.Columns.Add("tipo_ejecucion");
                    TablaDatos.Columns.Add("idfirmante_1");
                    TablaDatos.Columns.Add("idfirmante_2");

                    TablaDatos.Columns.Add("usuario_liquida");
                    TablaDatos.Columns.Add("usuario_modifica");
                    TablaDatos.Columns.Add("usuario_anula");

                    TablaDatos.Columns.Add("fecha_anula");
                    TablaDatos.Columns.Add("fecha_borrador");
                    TablaDatos.Columns.Add("fecha_definitivo");
                    TablaDatos.Columns.Add("fecha_registro");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idliquid_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idliquid_impuesto"].ToString().Trim());
                            Fila["idformulario_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["id_cliente"] = Int32.Parse(TheDataReaderPostgreSQL["id_cliente"].ToString().Trim());
                            Fila["idcliente_establecimiento"] = Int32.Parse(TheDataReaderPostgreSQL["idcliente_establecimiento"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["anio_gravable"] = Int32.Parse(TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim());
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();

                            Fila["periodo_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["periodo_impuesto"].ToString().Trim());
                            Fila["descripcion_periodo"] = TheDataReaderPostgreSQL["descripcion_periodo"].ToString().Trim();
                            Fila["idperiodicidad_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idperiodicidad_impuesto"].ToString().Trim());
                            Fila["periodicidad_impuesto"] = TheDataReaderPostgreSQL["periodicidad_impuesto"].ToString().Trim();

                            Fila["opcion_uso"] = Int32.Parse(TheDataReaderPostgreSQL["opcion_uso"].ToString().Trim());
                            Fila["descripcion_opcion_uso"] = TheDataReaderPostgreSQL["descripcion_opcion_uso"].ToString().Trim();
                            Fila["num_declaracion"] = TheDataReaderPostgreSQL["num_declaracion"].ToString().Trim();

                            Fila["sanciones"] = TheDataReaderPostgreSQL["sanciones"].ToString().Trim();
                            Fila["descripcion_sancion"] = TheDataReaderPostgreSQL["descripcion_sancion"].ToString().Trim();

                            double _ValorSancion = Double.Parse(TheDataReaderPostgreSQL["valor_sancion"].ToString().Trim());
                            Fila["valor_sancion"] = String.Format(String.Format("{0:$ ###,###,##0}", _ValorSancion));

                            double _InteresMora = Double.Parse(TheDataReaderPostgreSQL["interes_mora"].ToString().Trim());
                            Fila["interes_mora"] = String.Format(String.Format("{0:$ ###,###,##0}", _InteresMora));

                            double _TotalPagar = Double.Parse(TheDataReaderPostgreSQL["total_pagar"].ToString().Trim());
                            Fila["total_pagar"] = String.Format(String.Format("{0:$ ###,###,##0}", _TotalPagar));

                            double _ValorPagoVoluntario = Double.Parse(TheDataReaderPostgreSQL["valor_pago_voluntario"].ToString().Trim());
                            Fila["valor_pago_voluntario"] = String.Format(String.Format("{0:$ ###,###,##0}", _ValorPagoVoluntario));
                            Fila["destino_pago_voluntario"] = TheDataReaderPostgreSQL["destino_pago_voluntario"].ToString().Trim().Length > 0 ? TheDataReaderPostgreSQL["destino_pago_voluntario"].ToString().Trim() : "";

                            Fila["id_estado"] = Int32.Parse(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["estado_liquidacion"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["tipo_ejecucion"] = TheDataReaderPostgreSQL["tipo_ejecucion"].ToString().Trim();
                            Fila["idfirmante_1"] = TheDataReaderPostgreSQL["idfirmante_1"].ToString().Trim();
                            Fila["idfirmante_2"] = TheDataReaderPostgreSQL["idfirmante_2"].ToString().Trim();

                            Fila["usuario_liquida"] = TheDataReaderPostgreSQL["usuario_liquida"].ToString().Trim();
                            Fila["usuario_modifica"] = TheDataReaderPostgreSQL["usuario_modifica"].ToString().Trim();
                            Fila["usuario_anula"] = TheDataReaderPostgreSQL["usuario_anula"].ToString().Trim();

                            Fila["fecha_anula"] = TheDataReaderPostgreSQL["fecha_anula"].ToString().Trim();
                            Fila["fecha_borrador"] = TheDataReaderPostgreSQL["fecha_borrador"].ToString().Trim();
                            Fila["fecha_definitivo"] = TheDataReaderPostgreSQL["fecha_definitivo"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
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
                _log.Error("Error al obtener los datos del impuesto. Motivo: " + ex.Message);
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

            return TablaDatos;
        }

        public DataTable GetInfoLiquidacionForm()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtLiquidacionForm";
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

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idliquid_impuesto", IdLiquidImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_codigo_dane", CodigoDane);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_periodicidad_impuesto", PeriodicidadImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idliquid_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane_dpto");
                    TablaDatos.Columns.Add("nombre_dpto");
                    TablaDatos.Columns.Add("codigo_dane_munic");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("formulario_impuesto");
                    TablaDatos.Columns.Add("total_pagar");
                    TablaDatos.Columns.Add("nombre_firmante1");
                    TablaDatos.Columns.Add("nombre_firmante2");
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("usuario_borrador");
                    TablaDatos.Columns.Add("usuario_definitivo");
                    TablaDatos.Columns.Add("usuario_anula");
                    TablaDatos.Columns.Add("fecha_borrador");
                    TablaDatos.Columns.Add("fecha_definitivo");
                    TablaDatos.Columns.Add("fecha_anula");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idliquid_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idliquid_impuesto"].ToString().Trim());
                            Fila["codigo_dane_dpto"] = TheDataReaderPostgreSQL["codigo_dane_dpto"].ToString().Trim();
                            Fila["nombre_dpto"] = TheDataReaderPostgreSQL["nombre_dpto"].ToString().Trim();
                            Fila["codigo_dane_munic"] = TheDataReaderPostgreSQL["codigo_dane_munic"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["formulario_impuesto"] = TheDataReaderPostgreSQL["formulario_impuesto"].ToString().Trim();

                            double _TotalPagar = Double.Parse(TheDataReaderPostgreSQL["total_pagar"].ToString().Trim());
                            Fila["total_pagar"] = String.Format(String.Format("{0:$ ###,###,##0}", _TotalPagar));

                            Fila["nombre_firmante1"] = TheDataReaderPostgreSQL["nombre_firmante1"].ToString().Trim();
                            Fila["nombre_firmante2"] = TheDataReaderPostgreSQL["nombre_firmante2"].ToString().Trim();
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();

                            Fila["usuario_borrador"] = TheDataReaderPostgreSQL["usuario_borrador"].ToString().Trim();
                            Fila["usuario_definitivo"] = TheDataReaderPostgreSQL["usuario_definitivo"].ToString().Trim();
                            Fila["usuario_anula"] = TheDataReaderPostgreSQL["usuario_anula"].ToString().Trim();

                            Fila["fecha_borrador"] = TheDataReaderPostgreSQL["fecha_borrador"].ToString().Trim();
                            Fila["fecha_definitivo"] = TheDataReaderPostgreSQL["fecha_definitivo"].ToString().Trim();
                            Fila["fecha_anula"] = TheDataReaderPostgreSQL["fecha_anula"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_tipo_menu]. Motivo: " + ex.Message);
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

        public DataTable GetLiquidacionForm()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtLiquidacionForm";
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

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idliquid_impuesto", IdLiquidImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_codigo_dane", CodigoDane);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE CAMPOS
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("renglon8", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon8", typeof(Double));
                    TablaDatos.Columns.Add("renglon9", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon9", typeof(Double));
                    TablaDatos.Columns.Add("renglon10", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon10", typeof(Double));
                    TablaDatos.Columns.Add("renglon11", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon11", typeof(Double));
                    TablaDatos.Columns.Add("renglon12", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon12", typeof(Double));
                    TablaDatos.Columns.Add("renglon13", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon13", typeof(Double));
                    TablaDatos.Columns.Add("renglon14", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon14", typeof(Double));
                    TablaDatos.Columns.Add("renglon15", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon15", typeof(Double));
                    TablaDatos.Columns.Add("renglon16", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon16", typeof(Double));
                    TablaDatos.Columns.Add("renglon17", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon17", typeof(Double));
                    TablaDatos.Columns.Add("renglon19", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon19", typeof(Double));
                    TablaDatos.Columns.Add("renglon20", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon20", typeof(Double));
                    TablaDatos.Columns.Add("renglon21", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon21", typeof(Double));
                    TablaDatos.Columns.Add("renglon22", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon22", typeof(Double));
                    TablaDatos.Columns.Add("renglon23", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon23", typeof(Double));
                    TablaDatos.Columns.Add("renglon24", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon24", typeof(Double));
                    TablaDatos.Columns.Add("renglon25", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon25", typeof(Double));
                    TablaDatos.Columns.Add("renglon26", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon26", typeof(Double));
                    TablaDatos.Columns.Add("renglon27", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon27", typeof(Double));
                    TablaDatos.Columns.Add("renglon28", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon28", typeof(Double));
                    TablaDatos.Columns.Add("renglon29", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon29", typeof(Double));
                    TablaDatos.Columns.Add("renglon30", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon30", typeof(Double));
                    TablaDatos.Columns.Add("renglon31", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon31", typeof(Double));
                    TablaDatos.Columns.Add("renglon32", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon32", typeof(Double));
                    TablaDatos.Columns.Add("renglon33", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon33", typeof(Double));
                    TablaDatos.Columns.Add("renglon34", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon34", typeof(Double));
                    TablaDatos.Columns.Add("renglon35", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon35", typeof(Double));
                    TablaDatos.Columns.Add("renglon36", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon36", typeof(Double));
                    TablaDatos.Columns.Add("renglon37", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon37", typeof(Double));
                    TablaDatos.Columns.Add("renglon38", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon38", typeof(Double));
                    TablaDatos.Columns.Add("renglon39", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon39", typeof(Double));
                    TablaDatos.Columns.Add("renglon40", typeof(Int32));
                    TablaDatos.Columns.Add("valor_renglon40", typeof(Double));
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DE LA CONSULTA
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            //--
                            Fila["renglon8"] = 8;
                            Fila["valor_renglon8"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon8"].ToString().Trim());
                            Fila["renglon9"] = 9;
                            Fila["valor_renglon9"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon9"].ToString().Trim());
                            Fila["renglon10"] = 10;
                            Fila["valor_renglon10"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon10"].ToString().Trim());
                            Fila["renglon11"] = 11;
                            Fila["valor_renglon11"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon11"].ToString().Trim());
                            Fila["renglon12"] = 12;
                            Fila["valor_renglon12"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon12"].ToString().Trim());
                            Fila["renglon13"] = 13;
                            Fila["valor_renglon13"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon13"].ToString().Trim());
                            Fila["renglon14"] = 14;
                            Fila["valor_renglon14"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon14"].ToString().Trim());
                            Fila["renglon15"] = 15;
                            Fila["valor_renglon15"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon15"].ToString().Trim());
                            Fila["renglon16"] = 16;
                            Fila["valor_renglon16"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon16"].ToString().Trim());
                            Fila["renglon17"] = 17;
                            Fila["valor_renglon17"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon17"].ToString().Trim());
                            Fila["renglon19"] = 19;
                            Fila["valor_renglon19"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon19"].ToString().Trim());
                            Fila["renglon20"] = 20;
                            Fila["valor_renglon20"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon20"].ToString().Trim());
                            Fila["renglon21"] = 21;
                            Fila["valor_renglon21"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon21"].ToString().Trim());
                            Fila["renglon22"] = 22;
                            Fila["valor_renglon22"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon22"].ToString().Trim());
                            Fila["renglon23"] = 23;
                            Fila["valor_renglon23"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon23"].ToString().Trim());
                            Fila["renglon24"] = 24;
                            Fila["valor_renglon24"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon24"].ToString().Trim());
                            Fila["renglon25"] = 25;
                            Fila["valor_renglon25"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon25"].ToString().Trim());
                            Fila["renglon26"] = 26;
                            Fila["valor_renglon26"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon26"].ToString().Trim());
                            Fila["renglon27"] = 27;
                            Fila["valor_renglon27"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon27"].ToString().Trim());
                            Fila["renglon28"] = 28;
                            Fila["valor_renglon28"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon28"].ToString().Trim());
                            Fila["renglon29"] = 29;
                            Fila["valor_renglon29"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon29"].ToString().Trim());
                            Fila["renglon30"] = 30;
                            Fila["valor_renglon30"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon30"].ToString().Trim());
                            Fila["renglon31"] = 31;
                            Fila["valor_renglon31"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon31"].ToString().Trim());
                            Fila["renglon32"] = 32;
                            Fila["valor_renglon32"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon32"].ToString().Trim());
                            Fila["renglon33"] = 33;
                            Fila["valor_renglon33"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon33"].ToString().Trim());
                            Fila["renglon34"] = 34;
                            Fila["valor_renglon34"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon34"].ToString().Trim());
                            Fila["renglon35"] = 35;
                            Fila["valor_renglon35"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon35"].ToString().Trim());
                            Fila["renglon36"] = 36;
                            Fila["valor_renglon36"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon36"].ToString().Trim());
                            Fila["renglon37"] = 37;
                            Fila["valor_renglon37"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon37"].ToString().Trim());
                            Fila["renglon38"] = 38;
                            Fila["valor_renglon38"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon38"].ToString().Trim());
                            Fila["renglon39"] = 39;
                            Fila["valor_renglon39"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon39"].ToString().Trim());
                            Fila["renglon40"] = 40;
                            Fila["valor_renglon40"] = Double.Parse(TheDataReaderPostgreSQL["valor_renglon40"].ToString().Trim());
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_tipo_menu]. Motivo: " + ex.Message);
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

        public int GetContadorImpuestos()
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_contador_impuestos", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idperiodicidad_impuesto", PeriodicidadImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mes", MesImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            _Result = Int32.Parse(TheDataReaderPostgreSQL["cantidad_impuestos"].ToString().Trim());
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_tipo_menu]. Motivo: " + ex.Message);
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

        public bool AddUpLiquidacionImpuestoIca(ref int _IdRegistro, ref string _MsgError)
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
                    #region DEFINICION PARAMETROS SP POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_liquid_imp_ica", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idliquid_impuesto", IdLiquidImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente_establecimiento", IdClienteEstablecimiento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_codigo_dane", CodigoDane);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_fecha_max_presentacion", FechaMaxPresentacion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_fecha_liquidacion", FechaLiquidacion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_periodo_impuesto", PeriodoImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_periodicidad_impuesto", PeriodicidadImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_opcion_uso", OpcionUso);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_num_declaracion", NumDeclaracion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon8", ValorRenglon8);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon9", ValorRenglon9);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon10", ValorRenglon10);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon11", ValorRenglon11);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon12", ValorRenglon12);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon13", ValorRenglon13);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon14", ValorRenglon14);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon15", ValorRenglon15);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon16", ValorRenglon16);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_actividad1", ValorActividad1);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_actividad2", ValorActividad2);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_actividad3", ValorActividad3);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_otras_act", ValorOtrasAct);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon17", ValorRenglon17);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon19", ValorRenglon19);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon20", ValorRenglon20);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon21", ValorRenglon21);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon22", ValorRenglon22);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon23", ValorRenglon23);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon24", ValorRenglon24);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon25", ValorRenglon25);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon26", ValorRenglon26);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon27", ValorRenglon27);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon28", ValorRenglon28);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon29", ValorRenglon29);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon30", ValorRenglon30);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tarifa_ica", TarifaIca);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_base_grav_bomberil", BaseGravBomberil);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_base_grav_seguridad", BaseGravSeguridad);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_sanciones", Sanciones != null ? Sanciones : 0);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_descripcion_otro", DescripcionSancionOtro.ToString().Trim().Length > 0 ? DescripcionSancionOtro : "NA");
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_sancion", ValorSancion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon32", ValorRenglon32);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon33", ValorRenglon33);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon34", ValorRenglon34);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon35", ValorRenglon35);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon36", ValorRenglon36);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_interes_mora", InteresMora);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon38", ValorRenglon38);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_pago_vol", ValorPagoVoluntario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_destino_pago_vol", DestinoAportesVol.ToString().Trim().Length > 0 ? DestinoAportesVol : "NA");
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon40", ValorRenglon40);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idfirmante_1", IdFirmante1 != null ? IdFirmante1 : -1);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idfirmante_2", IdFirmante2 != null ? IdFirmante2 : -1);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_ejecucion", TipoEjecucion);
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
                _MsgError = "Error al guardar los datos de la liquidación. Motivo: " + ex.Message.ToString().Trim();
                _log.Error(_MsgError.ToString().Trim());
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
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
                    TheCommandSQLServer.Dispose();
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

        public bool AddUpLiquidacionImpuestoAutoIca(ref int _IdRegistro, ref string _MsgError)
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
                    #region DEFINICION PARAMETROS SP POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_liquid_imp_autoica", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idliquid_impuesto", IdLiquidImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente_establecimiento", IdClienteEstablecimiento);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_codigo_dane", CodigoDane);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_fecha_max_presentacion", FechaMaxPresentacion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_fecha_liquidacion", FechaLiquidacion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_periodo_impuesto", PeriodoImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_periodicidad_impuesto", PeriodicidadImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_opcion_uso", OpcionUso);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_num_declaracion", NumDeclaracion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon8", ValorRenglon8);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon9", ValorRenglon9);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon10", ValorRenglon10);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon11", ValorRenglon11);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon12", ValorRenglon12);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon13", ValorRenglon13);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon14", ValorRenglon14);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon15", ValorRenglon15);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon16", ValorRenglon16);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_actividad1", ValorActividad1);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_actividad2", ValorActividad2);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_actividad3", ValorActividad3);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_otras_act", ValorOtrasAct);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon17", ValorRenglon17);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon19", ValorRenglon19);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon20", ValorRenglon20);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon21", ValorRenglon21);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon22", ValorRenglon22);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon23", ValorRenglon23);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon24", ValorRenglon24);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon25", ValorRenglon25);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon26", ValorRenglon26);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon27", ValorRenglon27);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon28", ValorRenglon28);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon29", ValorRenglon29);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon30", ValorRenglon30);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon31", ValorRenglon31);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon32", ValorRenglon32);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon33", ValorRenglon33);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon34", ValorRenglon34);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon35", ValorRenglon35);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon36", ValorRenglon36);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon37", ValorRenglon37);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon38", ValorRenglon38);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_sanciones", Sanciones);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_descripcion_otro", DescripcionSancionOtro);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon39", ValorSancion);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon40", ValorRenglon40);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon41", ValorRenglon41);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon42", ValorRenglon42);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon43", InteresMora);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_valor_renglon44", ValorRenglon44);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idfirmante_1", IdFirmante1);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idfirmante_2", IdFirmante2);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_ejecucion", TipoEjecucion);
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
                _MsgError = "Error al guardar los datos de la liquidación. Motivo: " + ex.Message.ToString().Trim();
                _log.Error(_MsgError.ToString().Trim());
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
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
                    TheCommandSQLServer.Dispose();
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