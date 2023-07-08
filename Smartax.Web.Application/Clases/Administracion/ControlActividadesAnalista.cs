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

namespace Smartax.Web.Application.Clases.Administracion
{
    public class ControlActividadesAnalista
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        Utilidades objUtils = new Utilidades();

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
        public object IdCtrlActividadAnalista { get; set; }
        public object IdControlActividad { get; set; }
        public object IdTipoCtrlActividad { get; set; }
        public object IdFormularioImpuesto { get; set; }
        public object AnioGravable { get; set; }
        public object IdCliente { get; set; }
        public string CodigoActividad { get; set; }
        public string DescripcionActividad { get; set; }
        public string CantidadSi { get; set; }
        public string CantidadNo { get; set; }
        public object IdEstado { get; set; }
        public object IdRol { get; set; }
        public object IdUsuario { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string MostrarSeleccione { get; set; }
        public string MotorBaseDatos { get; set; }
        public int TipoConsulta { get; set; }
        public int TipoProceso { get; set; }
        #endregion

        public DataTable GetAllActividades()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtControlActividades";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_ctrl_actividades_analista", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcontrol_actividad", IdControlActividad);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_ctrl_actividad", IdTipoCtrlActividad);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario_analista", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region AQUI DEFINIMOS LAS COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idctrl_actividades_analista", typeof(Int32));
                    TablaDatos.Columns.Add("idcontrol_actividad", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_actividad");
                    TablaDatos.Columns.Add("descripcion_actividad");
                    TablaDatos.Columns.Add("idtipo_ctrl_actividades", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_ctrl_actividad");
                    TablaDatos.Columns.Add("doble_cantidad");
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("descripcion_formulario");
                    TablaDatos.Columns.Add("anio_gravable");
                    TablaDatos.Columns.Add("id_cliente");
                    TablaDatos.Columns.Add("nombre_cliente");
                    TablaDatos.Columns.Add("idusuario_analista");
                    TablaDatos.Columns.Add("usuario_analista");
                    TablaDatos.Columns.Add("cantidad", typeof(Double));
                    TablaDatos.Columns.Add("cantidad_si", typeof(Double));
                    TablaDatos.Columns.Add("cantidad_no", typeof(Double));
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_modificacion");
                    TablaDatos.Columns.Add("fecha_registro");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idctrl_actividades_analista"] = Int32.Parse(TheDataReaderPostgreSQL["idctrl_actividades_analista"].ToString().Trim());
                            Fila["idcontrol_actividad"] = Int32.Parse(TheDataReaderPostgreSQL["idcontrol_actividad"].ToString().Trim());
                            Fila["codigo_actividad"] = TheDataReaderPostgreSQL["codigo_actividad"].ToString().Trim();
                            Fila["descripcion_actividad"] = TheDataReaderPostgreSQL["descripcion_actividad"].ToString().Trim();
                            Fila["idtipo_ctrl_actividades"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_ctrl_actividades"].ToString().Trim());
                            Fila["tipo_ctrl_actividad"] = TheDataReaderPostgreSQL["tipo_ctrl_actividad"].ToString().Trim();
                            Fila["doble_cantidad"] = TheDataReaderPostgreSQL["doble_cantidad"].ToString().Trim();
                            Fila["idformulario_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["descripcion_formulario"] = TheDataReaderPostgreSQL["descripcion_formulario"].ToString().Trim();
                            Fila["anio_gravable"] = TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim();
                            Fila["id_cliente"] = TheDataReaderPostgreSQL["id_cliente"].ToString().Trim();
                            Fila["nombre_cliente"] = TheDataReaderPostgreSQL["nombre_cliente"].ToString().Trim();
                            Fila["idusuario_analista"] = TheDataReaderPostgreSQL["idusuario_analista"].ToString().Trim();
                            Fila["usuario_analista"] = TheDataReaderPostgreSQL["usuario_analista"].ToString().Trim();

                            Fila["cantidad"] = Double.Parse(TheDataReaderPostgreSQL["cantidad"].ToString().Trim());
                            Fila["cantidad_si"] = Double.Parse(TheDataReaderPostgreSQL["cantidad_si"].ToString().Trim());
                            Fila["cantidad_no"] = Double.Parse(TheDataReaderPostgreSQL["cantidad_no"].ToString().Trim());

                            Fila["id_estado"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_modificacion"] = TheDataReaderPostgreSQL["fecha_modificacion"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_control_actividades]. Motivo: " + ex.Message);
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

        public DataTable GetMonitoreoActividades()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtMonitoreoActividades";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_monitoreo_actividades", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_ctrl_actividad", IdTipoCtrlActividad);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario_analista", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region AQUI DEFINIMOS LAS COLUMNAS DEL DATATABLE
                    //TablaDatos.Columns.Add("idctrl_actividades_analista", typeof(Int32));
                    TablaDatos.Columns.Add("id_registro", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_ctrl_actividad");
                    TablaDatos.Columns.Add("idcontrol_actividad", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_actividad");
                    TablaDatos.Columns.Add("descripcion_actividad");
                    TablaDatos.Columns.Add("cantidad", typeof(Double));
                    TablaDatos.Columns.Add("cantidad_si", typeof(Double));
                    TablaDatos.Columns.Add("porcentaje_si", typeof(Double));
                    TablaDatos.Columns.Add("cantidad_no", typeof(Double));
                    TablaDatos.Columns.Add("porcentaje_no", typeof(Double));
                    TablaDatos.Columns.Add("total", typeof(Double));
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            //Fila["idctrl_actividades_analista"] = Int32.Parse(TheDataReaderPostgreSQL["idctrl_actividades_analista"].ToString().Trim());
                            Fila["id_registro"] = TablaDatos.Rows.Count + 1;
                            Fila["tipo_ctrl_actividad"] = TheDataReaderPostgreSQL["tipo_ctrl_actividad"].ToString().Trim();
                            Fila["idcontrol_actividad"] = Int32.Parse(TheDataReaderPostgreSQL["idcontrol_actividad"].ToString().Trim());
                            Fila["codigo_actividad"] = TheDataReaderPostgreSQL["codigo_actividad"].ToString().Trim();
                            Fila["descripcion_actividad"] = TheDataReaderPostgreSQL["descripcion_actividad"].ToString().Trim();
                            //--
                            double _Cantidad = Double.Parse(TheDataReaderPostgreSQL["cantidad"].ToString().Trim());
                            double _CantidadSi = Double.Parse(TheDataReaderPostgreSQL["cantidad_si"].ToString().Trim());
                            double _CantidadNo = Double.Parse(TheDataReaderPostgreSQL["cantidad_no"].ToString().Trim());
                            double _Total = (_CantidadSi + _CantidadNo);
                            //--
                            Fila["cantidad"] = _Cantidad;
                            Fila["cantidad_si"] = _CantidadSi;
                            Fila["porcentaje_si"] = 0;
                            Fila["cantidad_no"] = _CantidadNo;
                            Fila["porcentaje_no"] = 0;
                            Fila["total"] = _Total;
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_control_actividades]. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    if(TheDataReaderPostgreSQL != null)
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

        public DataTable GetEstadisticaActividades()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtEstadisticaAct";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_monitoreo_actividades", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_ctrl_actividad", IdTipoCtrlActividad);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario_analista", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region AQUI DEFINIMOS LAS COLUMNAS DEL DATATABLE
                    //TablaDatos.Columns.Add("idctrl_actividades_analista", typeof(Int32));
                    TablaDatos.Columns.Add("id_registro", typeof(Int32));
                    TablaDatos.Columns.Add("idtipo_ctrl_actividades", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_ctrl_actividad");
                    TablaDatos.Columns.Add("idcontrol_actividad", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_actividad");
                    TablaDatos.Columns.Add("descripcion_actividad");
                    TablaDatos.Columns.Add("cantidad", typeof(Double));
                    TablaDatos.Columns.Add("porcentaje", typeof(Double));
                    //TablaDatos.Columns.Add("total", typeof(Double));
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();

                            int _IdTipoCtrlActividad = Int32.Parse(TheDataReaderPostgreSQL["idtipo_ctrl_actividades"].ToString().Trim());
                            DataRow[] dataRows = TablaDatos.Select("idtipo_ctrl_actividades = " + _IdTipoCtrlActividad);
                            if (dataRows.Length > 0)
                            {
                                double _TotalCantidadAntes = 0;
                                foreach (DataRow rowItem in dataRows)
                                {
                                    double _CantidadAntes = Double.Parse(rowItem["cantidad"].ToString().Trim());
                                    _TotalCantidadAntes = _TotalCantidadAntes + _CantidadAntes;
                                }

                                #region AQUI ACTUALIZAMOS EL REGISTRO EN EL DATATABLE
                                //--
                                double _CantidadNueva = Double.Parse(TheDataReaderPostgreSQL["cantidad"].ToString().Trim());
                                double _TotalCantidad = (_CantidadNueva + _TotalCantidadAntes);
                                double _Porcentaje1 = _TotalCantidadAntes > 0 ? ((_TotalCantidadAntes / _TotalCantidad) * 100) : 0;
                                double _Porcentaje2 = _CantidadNueva > 0 ? ((_CantidadNueva / _TotalCantidad) * 100) : 0;

                                //--AQUI ACTUALIZAMOS EL % DEL REGISTRO ANTERIOR
                                dataRows[0]["porcentaje"] = Math.Round(_Porcentaje1, 2);
                                TablaDatos.Rows[0].AcceptChanges();
                                TablaDatos.Rows[0].EndEdit();

                                //--AQUI ACTUALIZAMOS EL % DEL REGISTRO ANTERIOR
                                #region SE AGREGA EL NUEVO REGISTRO EN EL DATATABLE
                                //Fila["idctrl_actividades_analista"] = Int32.Parse(TheDataReaderPostgreSQL["idctrl_actividades_analista"].ToString().Trim());
                                Fila["id_registro"] = TablaDatos.Rows.Count + 1;
                                Fila["idtipo_ctrl_actividades"] = _IdTipoCtrlActividad;
                                Fila["tipo_ctrl_actividad"] = TheDataReaderPostgreSQL["tipo_ctrl_actividad"].ToString().Trim();
                                Fila["idcontrol_actividad"] = Int32.Parse(TheDataReaderPostgreSQL["idcontrol_actividad"].ToString().Trim());
                                Fila["codigo_actividad"] = TheDataReaderPostgreSQL["codigo_actividad"].ToString().Trim();
                                Fila["descripcion_actividad"] = TheDataReaderPostgreSQL["descripcion_actividad"].ToString().Trim();
                                //--
                                Fila["cantidad"] = _CantidadNueva;
                                Fila["porcentaje"] = Math.Round(_Porcentaje2, 2);
                                TablaDatos.Rows.Add(Fila);
                                #endregion
                                //--
                                #endregion
                            }
                            else
                            {
                                #region SE AGREGA EL NUEVO REGISTRO EN EL DATATABLE
                                //Fila["idctrl_actividades_analista"] = Int32.Parse(TheDataReaderPostgreSQL["idctrl_actividades_analista"].ToString().Trim());
                                Fila["id_registro"] = TablaDatos.Rows.Count + 1;
                                Fila["idtipo_ctrl_actividades"] = _IdTipoCtrlActividad;
                                Fila["tipo_ctrl_actividad"] = TheDataReaderPostgreSQL["tipo_ctrl_actividad"].ToString().Trim();
                                Fila["idcontrol_actividad"] = Int32.Parse(TheDataReaderPostgreSQL["idcontrol_actividad"].ToString().Trim());
                                Fila["codigo_actividad"] = TheDataReaderPostgreSQL["codigo_actividad"].ToString().Trim();
                                Fila["descripcion_actividad"] = TheDataReaderPostgreSQL["descripcion_actividad"].ToString().Trim();
                                //--
                                double _Cantidad = Double.Parse(TheDataReaderPostgreSQL["cantidad"].ToString().Trim());
                                //--
                                Fila["cantidad"] = _Cantidad;
                                Fila["porcentaje"] = 0;
                                TablaDatos.Rows.Add(Fila);
                                #endregion
                            }
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_control_actividades]. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                    if(TheDataReaderPostgreSQL != null)
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

        public DataTable GetEstadisticaImpuestos()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtEstadisticaImpuestos";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_monitoreo_actividades", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_ctrl_actividad", IdTipoCtrlActividad);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormularioImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario_analista", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region AQUI DEFINIMOS LAS COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("id_registro", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_impuesto");
                    //--AQUI VALIDAMOS EL TIPO DE CONSULTA 
                    if(TipoConsulta == 6)
                    {
                        TablaDatos.Columns.Add("nombre_usuario");
                    }
                    TablaDatos.Columns.Add("estado_liquidacion");
                    TablaDatos.Columns.Add("cantidad", typeof(Double));
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_registro"] = TablaDatos.Rows.Count + 1;
                            Fila["tipo_impuesto"] = TheDataReaderPostgreSQL["tipo_impuesto"].ToString().Trim();
                            //--AQUI VALIDAMOS EL TIPO DE CONSULTA 
                            if (TipoConsulta == 6)
                            {
                                Fila["nombre_usuario"] = TheDataReaderPostgreSQL["nombre_usuario"].ToString().Trim();
                            }
                            Fila["estado_liquidacion"] = TheDataReaderPostgreSQL["estado_liquidacion"].ToString().Trim();
                            Fila["cantidad"] = Double.Parse(TheDataReaderPostgreSQL["cantidad"].ToString().Trim());
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
                _log.Error("Error al obtener los datos para la estadistica de impuestos. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    if(TheDataReaderPostgreSQL != null)
                    {
                        TheCommandPostgreSQL = null;
                        if (TheDataReaderPostgreSQL != null)
                        {
                            TheDataReaderPostgreSQL.Close();
                        }
                        TheDataReaderPostgreSQL = null;
                    }
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

        public bool AddUpControlActividades(ref int _IdRegistro, ref string _MsgError)
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_ctrl_actividad_analista", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();
                    //--
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idctrl_actividad_analista", IdCtrlActividadAnalista);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcontrol_actividad", IdControlActividad);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtipo_ctrl_actividad", IdTipoCtrlActividad);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idcliente", IdCliente);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario_analista", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_cantidad_si", CantidadSi.ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_cantidad_no", CantidadNo.ToString().Trim());
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
                _MsgError = "Error al registrar la actividad del analista. Motivo: " + ex.Message.ToString().Trim();
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