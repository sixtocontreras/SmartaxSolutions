using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Configuration;
using log4net;
using log4net.Config;
using System.Data.OracleClient;
using System.Data.SqlClient;
using Devart.Data.PostgreSql;
using MySql.Data.MySqlClient;
using Smartax.Web.Application.Clases.Seguridad;

namespace Smartax.Web.Application.Clases.Parametros
{
    public class Empresas
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

        #region DEFINICION DE ATRIBUTOS Y PROPIEDADES DE LA CLASE
        private string _Nit = "";
        private string _NumeroResolucion = "";
        private object _IdEmpresa;
        private object _IdLogoEmpresa;
        private string _NombreEmpresa;
        private object _IdEmpresaPadre;
        private int _IdUsuarioAdd;
        private object _IdUsuarioUp;
        private object _IdEstado;
        private int _IdRol;
        private int _IdParametro;
        private byte[] _ImagenEmpresa;
        private string _TipoEmpresa;
        private string _EmpresaUnica;
        private string _MostrarSeleccione;
        private string _MotorBaseDatos = "";
        private int _TipoConsulta;
        private int _TipoProceso;

        public string Nit
        {
            get { return _Nit; }
            set { _Nit = value; }
        }

        public string NumeroResolucion
        {
            get { return _NumeroResolucion; }
            set { _NumeroResolucion = value; }
        }

        public object IdEmpresa
        {
            get { return _IdEmpresa; }
            set { _IdEmpresa = value; }
        }

        public object IdEmpresaPadre
        {
            get { return _IdEmpresaPadre; }
            set { _IdEmpresaPadre = value; }
        }

        public int IdUsuarioAdd
        {
            get { return _IdUsuarioAdd; }
            set { _IdUsuarioAdd = value; }
        }

        public object IdUsuarioUp
        {
            get { return _IdUsuarioUp; }
            set { _IdUsuarioUp = value; }
        }

        public object IdEstado
        {
            get { return _IdEstado; }
            set { _IdEstado = value; }
        }

        public int IdRol
        {
            get { return _IdRol; }
            set { _IdRol = value; }
        }

        public int IdParametro
        {
            get { return _IdParametro; }
            set { _IdParametro = value; }
        }

        public string TipoEmpresa
        {
            get { return _TipoEmpresa; }
            set { _TipoEmpresa = value; }
        }

        public string EmpresaUnica
        {
            get { return _EmpresaUnica; }
            set { _EmpresaUnica = value; }
        }

        public string MostrarSeleccione
        {
            get { return _MostrarSeleccione; }
            set { _MostrarSeleccione = value; }
        }

        public string MotorBaseDatos
        {
            get { return _MotorBaseDatos; }
            set { _MotorBaseDatos = value; }
        }

        public int TipoConsulta
        {
            get
            {
                return _TipoConsulta;
            }

            set
            {
                _TipoConsulta = value;
            }
        }

        public int TipoProceso
        {
            get { return _TipoProceso; }
            set { _TipoProceso = value; }
        }

        public byte[] ImagenEmpresa
        {
            get
            {
                return _ImagenEmpresa;
            }

            set
            {
                _ImagenEmpresa = value;
            }
        }

        public string NombreEmpresa
        {
            get
            {
                return _NombreEmpresa;
            }

            set
            {
                _NombreEmpresa = value;
            }
        }

        public object IdLogoEmpresa
        {
            get
            {
                return _IdLogoEmpresa;
            }

            set
            {
                _IdLogoEmpresa = value;
            }
        }
        #endregion

        #region DEFINICION DE METODOS PARA OBTENER LOS DATOS DE LA BB
        //Aqui retornamos un DataTable con los datos de las empresas padres
        public DataTable GetAllEmpresaPadre()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtEmpresas";
            try
            {
                #region DEFINICION DEL OBJETO DE CONEXION A LA DB.
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
                    #region OBTNER DATOS DE LA DB POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_empresa", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros 
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa", IdEmpresa);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINIR CAMPOS DEL DATATABLE
                    TablaDatos.Columns.Add("id_empresa", typeof(Int32));
                    TablaDatos.Columns.Add("nit_empresa");
                    TablaDatos.Columns.Add("nombre_empresa");
                    TablaDatos.Columns.Add("emblema_empresa");
                    TablaDatos.Columns.Add("direccion_empresa");
                    TablaDatos.Columns.Add("telefono_empresa");
                    TablaDatos.Columns.Add("cant_empresas_registrar");
                    TablaDatos.Columns.Add("email_empresa");

                    TablaDatos.Columns.Add("id_pais");
                    TablaDatos.Columns.Add("nombre_pais");
                    TablaDatos.Columns.Add("id_dpto");
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio");
                    TablaDatos.Columns.Add("nombre_municipio");

                    TablaDatos.Columns.Add("tipo_empresa");
                    TablaDatos.Columns.Add("empresa_unica");
                    TablaDatos.Columns.Add("id_estado");
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_empresa"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_empresa"]);
                            Fila["nit_empresa"] = TheDataReaderPostgreSQL["nit_empresa"].ToString().Trim();
                            Fila["nombre_empresa"] = TheDataReaderPostgreSQL["nombre_empresa"].ToString().Trim();
                            Fila["emblema_empresa"] = TheDataReaderPostgreSQL["emblema_empresa"].ToString().Trim();
                            Fila["direccion_empresa"] = TheDataReaderPostgreSQL["direccion_empresa"].ToString().Trim();
                            Fila["telefono_empresa"] = TheDataReaderPostgreSQL["telefono_empresa"].ToString().Trim();
                            Fila["cant_empresas_registrar"] = Convert.ToInt32(TheDataReaderPostgreSQL["cant_empresas_registrar"]);
                            Fila["email_empresa"] = TheDataReaderPostgreSQL["email_empresa"].ToString().Trim();

                            Fila["id_pais"] = TheDataReaderPostgreSQL["id_pais"].ToString().Trim();
                            Fila["nombre_pais"] = TheDataReaderPostgreSQL["nombre_pais"].ToString().Trim();
                            Fila["id_dpto"] = TheDataReaderPostgreSQL["id_dpto"].ToString().Trim();
                            Fila["nombre_departamento"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = TheDataReaderPostgreSQL["id_municipio"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();

                            if ((TheDataReaderPostgreSQL["tipo_empresa"].ToString().Trim().Equals("P")))
                            {
                                Fila["tipo_empresa"] = true;
                            }
                            else
                            {
                                Fila["tipo_empresa"] = false;
                            }

                            if ((TheDataReaderPostgreSQL["empresa_unica"].ToString().Trim().Equals("S")))
                            {
                                Fila["empresa_unica"] = true;
                            }
                            else
                            {
                                Fila["empresa_unica"] = false;
                            }

                            Fila["id_estado"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_estado"]);
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    #endregion
                }
                else if (myConnectionDb is SqlConnection)
                {
                    #region OBTENER DATOS DE LA DB DE SQL SERVER
                    //Para Base de Datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_get_empresa";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    //Limpiar parametros 
                    TheCommandSQLServer.Parameters.Clear();

                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_id_rol", IdRol);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_id_empresa", IdEmpresa);
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    TablaDatos.Columns.Add("id_empresa", typeof(Int32));
                    TablaDatos.Columns.Add("nit_empresa");
                    TablaDatos.Columns.Add("nombre_empresa");
                    TablaDatos.Columns.Add("emblema_empresa");
                    TablaDatos.Columns.Add("direccion_empresa");
                    TablaDatos.Columns.Add("telefono_empresa");
                    TablaDatos.Columns.Add("cant_empresas_registrar");
                    TablaDatos.Columns.Add("email_empresa");

                    TablaDatos.Columns.Add("id_pais");
                    TablaDatos.Columns.Add("nombre_pais");
                    TablaDatos.Columns.Add("id_dpto");
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio");
                    TablaDatos.Columns.Add("nombre_municipio");

                    TablaDatos.Columns.Add("tipo_empresa");
                    TablaDatos.Columns.Add("empresa_unica");
                    TablaDatos.Columns.Add("saldo_cupo");
                    TablaDatos.Columns.Add("id_estado");
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");

                    if (TheDataReaderSQLServer != null)
                    {
                        while (TheDataReaderSQLServer.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_empresa"] = Convert.ToInt32(TheDataReaderSQLServer["id_empresa"]);
                            Fila["nit_empresa"] = TheDataReaderSQLServer["nit_empresa"].ToString().Trim();
                            Fila["nombre_empresa"] = TheDataReaderSQLServer["nombre_empresa"].ToString().Trim();
                            Fila["emblema_empresa"] = TheDataReaderSQLServer["emblema_empresa"].ToString().Trim();
                            Fila["direccion_empresa"] = TheDataReaderSQLServer["direccion_empresa"].ToString().Trim();
                            Fila["telefono_empresa"] = TheDataReaderSQLServer["telefono_empresa"].ToString().Trim();
                            Fila["cant_empresas_registrar"] = Convert.ToInt32(TheDataReaderSQLServer["cant_empresas_registrar"]);
                            Fila["email_empresa"] = TheDataReaderSQLServer["email_empresa"].ToString().Trim();

                            Fila["id_pais"] = TheDataReaderSQLServer["id_pais"].ToString().Trim();
                            Fila["nombre_pais"] = TheDataReaderSQLServer["nombre_pais"].ToString().Trim();
                            Fila["id_dpto"] = TheDataReaderSQLServer["id_dpto"].ToString().Trim();
                            Fila["nombre_departamento"] = TheDataReaderSQLServer["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = TheDataReaderSQLServer["id_municipio"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderSQLServer["nombre_municipio"].ToString().Trim();

                            if ((TheDataReaderSQLServer["tipo_empresa"].ToString().Trim().Equals("P")))
                            {
                                Fila["tipo_empresa"] = true;
                            }
                            else
                            {
                                Fila["tipo_empresa"] = false;
                            }

                            if ((TheDataReaderSQLServer["empresa_unica"].ToString().Trim().Equals("S")))
                            {
                                Fila["empresa_unica"] = true;
                            }
                            else
                            {
                                Fila["empresa_unica"] = false;
                            }

                            Fila["saldo_cupo"] = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(TheDataReaderSQLServer["saldo_cupo"].ToString().Trim())));
                            Fila["id_estado"] = Convert.ToInt32(TheDataReaderSQLServer["id_estado"]);
                            Fila["codigo_estado"] = TheDataReaderSQLServer["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderSQLServer["fecha_registro"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_empresa]. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION A LA DB
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

        //Aqui retornamos un DataTable con los datos de las empresas hijas
        public DataTable GetAllEmpresaHija()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtEmpresasHijas";
            try
            {
                #region DEFINICION DEL OBJETO DE CONEXION A LA DB.
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
                    #region OBTNER DATOS DE LA DB POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_empresa", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros 
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa", IdEmpresa);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINIR CAMPOS DEL DATATABLE
                    TablaDatos.Columns.Add("idempresa_hija", typeof(Int32));
                    TablaDatos.Columns.Add("nit_empresa");
                    TablaDatos.Columns.Add("nombre_empresa");
                    TablaDatos.Columns.Add("emblema_empresa");
                    TablaDatos.Columns.Add("direccion_empresa");
                    TablaDatos.Columns.Add("telefono_empresa");
                    TablaDatos.Columns.Add("cant_empresas_registrar");
                    TablaDatos.Columns.Add("email_empresa");

                    TablaDatos.Columns.Add("id_pais");
                    TablaDatos.Columns.Add("nombre_pais");
                    TablaDatos.Columns.Add("id_dpto");
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio");
                    TablaDatos.Columns.Add("nombre_municipio");

                    TablaDatos.Columns.Add("tipo_empresa");
                    TablaDatos.Columns.Add("empresa_unica");
                    TablaDatos.Columns.Add("id_estado");
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idempresa_hija"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_empresa"]);
                            Fila["nit_empresa"] = TheDataReaderPostgreSQL["nit_empresa"].ToString().Trim();
                            Fila["nombre_empresa"] = TheDataReaderPostgreSQL["nombre_empresa"].ToString().Trim();
                            Fila["emblema_empresa"] = TheDataReaderPostgreSQL["emblema_empresa"].ToString().Trim();
                            Fila["direccion_empresa"] = TheDataReaderPostgreSQL["direccion_empresa"].ToString().Trim();
                            Fila["telefono_empresa"] = TheDataReaderPostgreSQL["telefono_empresa"].ToString().Trim();
                            Fila["cant_empresas_registrar"] = Convert.ToInt32(TheDataReaderPostgreSQL["cant_empresas_registrar"]);
                            Fila["email_empresa"] = TheDataReaderPostgreSQL["email_empresa"].ToString().Trim();

                            Fila["id_pais"] = TheDataReaderPostgreSQL["id_pais"].ToString().Trim();
                            Fila["nombre_pais"] = TheDataReaderPostgreSQL["nombre_pais"].ToString().Trim();
                            Fila["id_dpto"] = TheDataReaderPostgreSQL["id_dpto"].ToString().Trim();
                            Fila["nombre_departamento"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = TheDataReaderPostgreSQL["id_municipio"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();

                            if ((TheDataReaderPostgreSQL["tipo_empresa"].ToString().Trim().Equals("P")))
                            {
                                Fila["tipo_empresa"] = true;
                            }
                            else
                            {
                                Fila["tipo_empresa"] = false;
                            }

                            if ((TheDataReaderPostgreSQL["empresa_unica"].ToString().Trim().Equals("S")))
                            {
                                Fila["empresa_unica"] = true;
                            }
                            else
                            {
                                Fila["empresa_unica"] = false;
                            }

                            Fila["id_estado"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_estado"]);
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    #endregion
                }
                else if (myConnectionDb is SqlConnection)
                {
                    #region OBTENER DATOS DE LA DB DE SQL SERVER
                    //Para Base de Datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_get_empresa";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    //Limpiar parametros 
                    TheCommandSQLServer.Parameters.Clear();

                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_id_rol", IdRol);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_id_empresa", IdEmpresa);
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    TablaDatos.Columns.Add("id_empresa", typeof(Int32));
                    TablaDatos.Columns.Add("nit_empresa");
                    TablaDatos.Columns.Add("nombre_empresa");
                    TablaDatos.Columns.Add("emblema_empresa");
                    TablaDatos.Columns.Add("direccion_empresa");
                    TablaDatos.Columns.Add("telefono_empresa");
                    TablaDatos.Columns.Add("cant_empresas_registrar");
                    TablaDatos.Columns.Add("email_empresa");

                    TablaDatos.Columns.Add("id_pais");
                    TablaDatos.Columns.Add("nombre_pais");
                    TablaDatos.Columns.Add("id_dpto");
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio");
                    TablaDatos.Columns.Add("nombre_municipio");

                    TablaDatos.Columns.Add("tipo_empresa");
                    TablaDatos.Columns.Add("empresa_unica");
                    TablaDatos.Columns.Add("saldo_cupo");
                    TablaDatos.Columns.Add("id_estado");
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");

                    if (TheDataReaderSQLServer != null)
                    {
                        while (TheDataReaderSQLServer.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_empresa"] = Convert.ToInt32(TheDataReaderSQLServer["id_empresa"]);
                            Fila["nit_empresa"] = TheDataReaderSQLServer["nit_empresa"].ToString().Trim();
                            Fila["nombre_empresa"] = TheDataReaderSQLServer["nombre_empresa"].ToString().Trim();
                            Fila["emblema_empresa"] = TheDataReaderSQLServer["emblema_empresa"].ToString().Trim();
                            Fila["direccion_empresa"] = TheDataReaderSQLServer["direccion_empresa"].ToString().Trim();
                            Fila["telefono_empresa"] = TheDataReaderSQLServer["telefono_empresa"].ToString().Trim();
                            Fila["cant_empresas_registrar"] = Convert.ToInt32(TheDataReaderSQLServer["cant_empresas_registrar"]);
                            Fila["email_empresa"] = TheDataReaderSQLServer["email_empresa"].ToString().Trim();

                            Fila["id_pais"] = TheDataReaderSQLServer["id_pais"].ToString().Trim();
                            Fila["nombre_pais"] = TheDataReaderSQLServer["nombre_pais"].ToString().Trim();
                            Fila["id_dpto"] = TheDataReaderSQLServer["id_dpto"].ToString().Trim();
                            Fila["nombre_departamento"] = TheDataReaderSQLServer["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = TheDataReaderSQLServer["id_municipio"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderSQLServer["nombre_municipio"].ToString().Trim();

                            if ((TheDataReaderSQLServer["tipo_empresa"].ToString().Trim().Equals("P")))
                            {
                                Fila["tipo_empresa"] = true;
                            }
                            else
                            {
                                Fila["tipo_empresa"] = false;
                            }

                            if ((TheDataReaderSQLServer["empresa_unica"].ToString().Trim().Equals("S")))
                            {
                                Fila["empresa_unica"] = true;
                            }
                            else
                            {
                                Fila["empresa_unica"] = false;
                            }

                            Fila["saldo_cupo"] = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(TheDataReaderSQLServer["saldo_cupo"].ToString().Trim())));
                            Fila["id_estado"] = Convert.ToInt32(TheDataReaderSQLServer["id_estado"]);
                            Fila["codigo_estado"] = TheDataReaderSQLServer["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderSQLServer["fecha_registro"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_empresa]. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION A LA DB
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

        //Aqui retornamos un DataTable con la informacion de la empresa seleccionada
        public DataTable GetInfoEmpresa()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtEmpresa";
            try
            {
                #region DEFINICION DEL OBJETO DE CONEXION A LA DB.
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
                    #region OBTNER DATOS DE LA DB POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_empresa", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros 
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa", IdEmpresa);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINIR CAMPOS DEL DATATABLE
                    TablaDatos.Columns.Add("id_empresa", typeof(Int32));
                    TablaDatos.Columns.Add("nit_empresa");
                    TablaDatos.Columns.Add("nombre_empresa");
                    TablaDatos.Columns.Add("emblema_empresa");
                    TablaDatos.Columns.Add("direccion_empresa");
                    TablaDatos.Columns.Add("telefono_empresa");
                    TablaDatos.Columns.Add("cant_empresas_registrar");
                    TablaDatos.Columns.Add("email_empresa");

                    TablaDatos.Columns.Add("id_pais");
                    TablaDatos.Columns.Add("nombre_pais");
                    TablaDatos.Columns.Add("id_dpto");
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio");
                    TablaDatos.Columns.Add("nombre_municipio");

                    TablaDatos.Columns.Add("tipo_empresa");
                    TablaDatos.Columns.Add("empresa_unica");
                    TablaDatos.Columns.Add("id_estado");
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_empresa"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_empresa"]);
                            Fila["nit_empresa"] = TheDataReaderPostgreSQL["nit_empresa"].ToString().Trim();
                            Fila["nombre_empresa"] = TheDataReaderPostgreSQL["nombre_empresa"].ToString().Trim();
                            Fila["emblema_empresa"] = TheDataReaderPostgreSQL["emblema_empresa"].ToString().Trim();
                            Fila["direccion_empresa"] = TheDataReaderPostgreSQL["direccion_empresa"].ToString().Trim();
                            Fila["telefono_empresa"] = TheDataReaderPostgreSQL["telefono_empresa"].ToString().Trim();
                            Fila["cant_empresas_registrar"] = Convert.ToInt32(TheDataReaderPostgreSQL["cant_empresas_registrar"]);
                            Fila["email_empresa"] = TheDataReaderPostgreSQL["email_empresa"].ToString().Trim();

                            Fila["id_pais"] = TheDataReaderPostgreSQL["id_pais"].ToString().Trim();
                            Fila["nombre_pais"] = TheDataReaderPostgreSQL["nombre_pais"].ToString().Trim();
                            Fila["id_dpto"] = TheDataReaderPostgreSQL["id_dpto"].ToString().Trim();
                            Fila["nombre_departamento"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = TheDataReaderPostgreSQL["id_municipio"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();

                            if ((TheDataReaderPostgreSQL["tipo_empresa"].ToString().Trim().Equals("P")))
                            {
                                Fila["tipo_empresa"] = true;
                            }
                            else
                            {
                                Fila["tipo_empresa"] = false;
                            }

                            if ((TheDataReaderPostgreSQL["empresa_unica"].ToString().Trim().Equals("S")))
                            {
                                Fila["empresa_unica"] = true;
                            }
                            else
                            {
                                Fila["empresa_unica"] = false;
                            }

                            Fila["id_estado"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_estado"]);
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    #endregion
                }
                else if (myConnectionDb is SqlConnection)
                {
                    #region OBTENER DATOS DE LA DB DE SQL SERVER
                    //Para Base de Datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_get_empresa";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    //Limpiar parametros 
                    TheCommandSQLServer.Parameters.Clear();

                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_id_rol", IdRol);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_id_empresa", IdEmpresa);
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    TablaDatos.Columns.Add("id_empresa", typeof(Int32));
                    TablaDatos.Columns.Add("nit_empresa");
                    TablaDatos.Columns.Add("nombre_empresa");
                    TablaDatos.Columns.Add("emblema_empresa");
                    TablaDatos.Columns.Add("direccion_empresa");
                    TablaDatos.Columns.Add("telefono_empresa");
                    TablaDatos.Columns.Add("cant_empresas_registrar");
                    TablaDatos.Columns.Add("email_empresa");

                    TablaDatos.Columns.Add("id_pais");
                    TablaDatos.Columns.Add("nombre_pais");
                    TablaDatos.Columns.Add("id_dpto");
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio");
                    TablaDatos.Columns.Add("nombre_municipio");

                    TablaDatos.Columns.Add("tipo_empresa");
                    TablaDatos.Columns.Add("empresa_unica");
                    TablaDatos.Columns.Add("saldo_cupo");
                    TablaDatos.Columns.Add("id_estado");
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");

                    if (TheDataReaderSQLServer != null)
                    {
                        while (TheDataReaderSQLServer.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_empresa"] = Convert.ToInt32(TheDataReaderSQLServer["id_empresa"]);
                            Fila["nit_empresa"] = TheDataReaderSQLServer["nit_empresa"].ToString().Trim();
                            Fila["nombre_empresa"] = TheDataReaderSQLServer["nombre_empresa"].ToString().Trim();
                            Fila["emblema_empresa"] = TheDataReaderSQLServer["emblema_empresa"].ToString().Trim();
                            Fila["direccion_empresa"] = TheDataReaderSQLServer["direccion_empresa"].ToString().Trim();
                            Fila["telefono_empresa"] = TheDataReaderSQLServer["telefono_empresa"].ToString().Trim();
                            Fila["cant_empresas_registrar"] = Convert.ToInt32(TheDataReaderSQLServer["cant_empresas_registrar"]);
                            Fila["email_empresa"] = TheDataReaderSQLServer["email_empresa"].ToString().Trim();

                            Fila["id_pais"] = TheDataReaderSQLServer["id_pais"].ToString().Trim();
                            Fila["nombre_pais"] = TheDataReaderSQLServer["nombre_pais"].ToString().Trim();
                            Fila["id_dpto"] = TheDataReaderSQLServer["id_dpto"].ToString().Trim();
                            Fila["nombre_departamento"] = TheDataReaderSQLServer["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = TheDataReaderSQLServer["id_municipio"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderSQLServer["nombre_municipio"].ToString().Trim();

                            if ((TheDataReaderSQLServer["tipo_empresa"].ToString().Trim().Equals("P")))
                            {
                                Fila["tipo_empresa"] = true;
                            }
                            else
                            {
                                Fila["tipo_empresa"] = false;
                            }

                            if ((TheDataReaderSQLServer["empresa_unica"].ToString().Trim().Equals("S")))
                            {
                                Fila["empresa_unica"] = true;
                            }
                            else
                            {
                                Fila["empresa_unica"] = false;
                            }

                            Fila["saldo_cupo"] = String.Format(String.Format("{0:$ ###,###,##0}", Double.Parse(TheDataReaderSQLServer["saldo_cupo"].ToString().Trim())));
                            Fila["id_estado"] = Convert.ToInt32(TheDataReaderSQLServer["id_estado"]);
                            Fila["codigo_estado"] = TheDataReaderSQLServer["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderSQLServer["fecha_registro"].ToString().Trim();
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
                _log.Error("Error al obtener los datos de la Tabla [tbl_empresa]. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION A LA DB
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

        //Aqui retornamos un DataTable con la informacion de la empresa seleccionada
        public DataTable GetEmpresas()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtEmpresas";
            try
            {
                #region DEFINICION DEL OBJETO DE CONEXION A LA DB.
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_empresa", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar los parametros de entrada
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa", IdEmpresa);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("id_empresa");
                    TablaDatos.Columns.Add("nombre_empresa");

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
                            Fila["id_empresa"] = TheDataReaderPostgreSQL["id_empresa"].ToString().Trim();
                            Fila["nombre_empresa"] = TheDataReaderPostgreSQL["nombre_empresa"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                        }
                    }
                    #endregion                    
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //--Base de datos de MySql
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //--Base de datos de Oracle
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al obtener la informacion de las empresas. Motivo: " + ex.Message);
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION A LA DB
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
                    TheDataReaderMySQL.Close();
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                    TheDataReaderSQLServer.Close();
                    TheDataReaderSQLServer.Close();
                }
                else if (myConnectionDb is OracleConnection)
                {
                    TheCommandOracle = null;
                    TheDataReaderOracle.Close();
                    TheDataReaderOracle.Close();
                }

                myConnectionDb.Close();
                myConnectionDb.Dispose();
                #endregion
            }

            return TablaDatos;
        }

        public int GetCantidadEmpresasRegistradas()
        {
            int CantidadEmpresa = 0;
            try
            {
                #region DEFINICION DEL OBJETO DE CONEXION A LA DB.
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
                    return CantidadEmpresa;
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
                    //Base de datos Postgresql
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_cantidad_empresas", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros 
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_id_empresa_padre", IdEmpresa);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            CantidadEmpresa = Convert.ToInt32(TheDataReaderPostgreSQL["cantidad_empresas"].ToString().Trim());
                        }
                    }
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Base de datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_cantidad_empresas";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;

                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_id_empresa_padre", IdEmpresa);
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    if (TheDataReaderSQLServer != null)
                    {
                        while (TheDataReaderSQLServer.Read())
                        {
                            CantidadEmpresa = Convert.ToInt32(TheDataReaderSQLServer["cantidad_empresas"].ToString().Trim());
                        }
                    }
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //Base de datos Oracle
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return CantidadEmpresa;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al traer informacion de la tabla empresas. Motivo: " + ex.Message);
            }
            finally
            {
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL.Dispose();
                    TheCommandPostgreSQL = null;
                    TheDataReaderPostgreSQL.Close();
                    TheDataReaderPostgreSQL = null;
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    TheCommandMySQL.Dispose();
                    TheCommandMySQL = null;
                    TheDataReaderMySQL.Close();
                    TheDataReaderMySQL = null;
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer.Dispose();
                    TheCommandSQLServer = null;
                    TheDataReaderSQLServer.Close();
                    TheDataReaderSQLServer = null;
                }
                else if (myConnectionDb is OracleConnection)
                {
                    TheCommandOracle.Dispose();
                    TheCommandOracle = null;
                    TheDataReaderOracle.Close();
                    TheDataReaderOracle = null;
                }

                myConnectionDb.Close();
                myConnectionDb.Dispose();
            }

            return CantidadEmpresa;
        }

        public double GetSaldoEmpresa()
        {
            double _SaldoEmpresa = 0;
            try
            {
                #region DEFINICION DEL OBJETO DE CONEXION A LA DB.
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
                    return _SaldoEmpresa;
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
                    //Base de datos Postgresql
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_empresa_saldo", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros 
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa", IdEmpresa);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            _SaldoEmpresa = Convert.ToDouble(TheDataReaderPostgreSQL["saldo_cupo"].ToString().Trim());
                        }
                    }
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Base de datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_get_empresa_saldo";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;

                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idempresa", IdEmpresa);
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    if (TheDataReaderSQLServer != null)
                    {
                        while (TheDataReaderSQLServer.Read())
                        {
                            _SaldoEmpresa = Convert.ToInt32(TheDataReaderSQLServer["saldo_cupo"].ToString().Trim());
                        }
                    }
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //Base de datos Oracle
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return _SaldoEmpresa;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al traer informacion de la tabla empresas. Motivo: " + ex.Message);
            }
            finally
            {
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL.Dispose();
                    TheCommandPostgreSQL = null;
                    TheDataReaderPostgreSQL.Close();
                    TheDataReaderPostgreSQL = null;
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    TheCommandMySQL.Dispose();
                    TheCommandMySQL = null;
                    TheDataReaderMySQL.Close();
                    TheDataReaderMySQL = null;
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer.Dispose();
                    TheCommandSQLServer = null;
                    TheDataReaderSQLServer.Close();
                    TheDataReaderSQLServer = null;
                }
                else if (myConnectionDb is OracleConnection)
                {
                    TheCommandOracle.Dispose();
                    TheCommandOracle = null;
                    TheDataReaderOracle.Close();
                    TheDataReaderOracle = null;
                }

                myConnectionDb.Close();
                myConnectionDb.Dispose();
            }

            return _SaldoEmpresa;
        }

        #endregion

        public bool AddUpEmpresa(DataRow Fila, ref int _IdRegistro, ref string _MsgError)
        {
            bool retValor = false;
            try
            {
                #region DEFINICION DEL OBJETO DE CONEXION A LA DB.
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
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_empresa", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa", IdEmpresa);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_nit_empresa", Fila["nit_empresa"].ToString().Trim().ToUpper());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_nombre_empresa", Fila["nombre_empresa"].ToString().Trim().ToUpper());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_emblema_empresa", Fila["emblema_empresa"].ToString().Trim().ToUpper());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_direccion_empresa", Fila["direccion_empresa"].ToString().Trim().ToUpper());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_telefono_empresa", Fila["telefono_empresa"].ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_email_empresa", Fila["email_empresa"].ToString().Trim().ToUpper());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_cant_empresas_registrar", Fila["cant_empresas_registrar"].ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa_padre", IdEmpresaPadre);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_id_pais", Fila["id_pais"]);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_id_dpto", Fila["id_dpto"]);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_id_ciudad", Fila["id_municipio"]);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_empresa", TipoEmpresa.ToString().Trim());
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_empresa_unica", Convert.ToBoolean(Fila["empresa_unica"].ToString().Trim()) == true ? "S" : "N");
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", Fila["id_estado"]);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_iduser_add", IdUsuarioAdd);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_iduser_up", IdUsuarioUp);
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
                }
                else if (myConnectionDb is MySqlConnection)
                {

                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_crud_empresa";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (System.Data.SqlClient.SqlConnection)myConnectionDb;
                    //se limpian los parámetros
                    TheCommandSQLServer.Parameters.Clear();

                    NombreEmpresa = Fila["nombre_empresa"].ToString().Trim().ToUpper();
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idempresa", IdEmpresa);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_nit_empresa", Fila["nit_empresa"].ToString().Trim().ToUpper());
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_nombre_empresa", NombreEmpresa);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_emblema_empresa", Fila["emblema_empresa"].ToString().Trim().ToUpper());
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_direccion_empresa", Fila["direccion_empresa"].ToString().Trim().ToUpper());
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_telefono_empresa", Fila["telefono_empresa"].ToString().Trim());
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_email_empresa", Fila["email_empresa"].ToString().Trim().ToUpper());
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_cant_empresas_registrar", Fila["cant_empresas_registrar"]);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idempresa_padre", IdEmpresaPadre);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_id_pais", Fila["id_pais"]);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_id_dpto", Fila["id_dpto"]);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_id_ciudad", Fila["id_municipio"]);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_tipo_empresa", TipoEmpresa.ToString().Trim());
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_empresa_unica", Convert.ToBoolean(Fila["empresa_unica"].ToString().Trim()) == true ? "S" : "N");
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idestado", Fila["id_estado"]);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_iduser_add", IdUsuarioAdd);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_iduser_up", IdUsuarioUp);
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
                            _MsgError = "La empresa [" + NombreEmpresa + "]. ah sido registrada de forma exitosa";
                        }
                        else if (TipoProceso == 2)
                        {
                            _MsgError = "Los datos de la empresa [" + NombreEmpresa + "]. han sido editados de forma exitosa";
                        }
                        else if (TipoProceso == 3)
                        {
                            _MsgError = "La empresa [" + NombreEmpresa + "]. han sido eliminado de forma exitosa";
                        }

                        retValor = true;
                    }

                    TheCommandSQLServer.Dispose();
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
                _MsgError = "Error al registrar el proceso del parametro del sistema. Motivo: " + ex.Message.ToString().Trim();
                _log.Error(_MsgError.ToString().Trim());
            }
            finally
            {
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
            }

            return retValor;
        }

        #region DEFINICION DE OBJETOS IMAGEN ENTIDAD
        //Aqui retornamos un DataTable con los datos de la imagen de la empresa seleccionada
        public DataTable GetImagenEmpresa()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtImagenEmpresa";
            try
            {
                #region DEFINICION DEL OBJETO DE CONEXION A LA DB.
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
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_imagen_empresa", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar los parametros de entrada
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_id_empresa", IdEmpresa);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    TablaDatos.Columns.Add("id_imagen_empresa", typeof(Int32));
                    TablaDatos.Columns.Add("id_empresa", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_empresa");
                    TablaDatos.Columns.Add("direccion_empresa");
                    TablaDatos.Columns.Add("telefono_empresa");
                    TablaDatos.Columns.Add("imagen_entidad", typeof(Byte[]));

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_imagen_empresa"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_imagen_empresa"]);
                            Fila["id_empresa"] = Convert.ToInt32(TheDataReaderPostgreSQL["id_empresa"]);
                            Fila["nombre_empresa"] = TheDataReaderPostgreSQL["nombre_empresa"].ToString().Trim();
                            Fila["direccion_empresa"] = TheDataReaderPostgreSQL["direccion_empresa"].ToString().Trim();
                            Fila["telefono_empresa"] = TheDataReaderPostgreSQL["telefono_empresa"].ToString().Trim();

                            if (TheDataReaderPostgreSQL["imagen_empresa"].ToString().Trim().Length > 0)
                            {
                                Fila["imagen_entidad"] = (Byte[])TheDataReaderPostgreSQL["imagen_empresa"];
                            }
                            else
                            {
                                Utilidades ObjUtils = new Utilidades();
                                Byte[] ImagenByte = null;
                                string strImgDefault = "Temas/Default/img_empresa.png";
                                string cRutaImagen = HttpContext.Current.Server.MapPath("/" + strImgDefault.ToString().Trim());

                                ImagenByte = ObjUtils.GetImagenBytes(cRutaImagen);

                                Fila["imagen_entidad"] = ImagenByte;
                            }
                            TablaDatos.Rows.Add(Fila);
                        }
                    }
                }
                else if (myConnectionDb is MySqlConnection)
                {

                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_get_imagen_empresa";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    //Limpiar los parametros de entrada
                    TheCommandSQLServer.Parameters.Clear();

                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_id_empresa", IdEmpresa);
                    TheDataReaderSQLServer = TheCommandSQLServer.ExecuteReader();

                    TablaDatos.Columns.Add("id_imagen_empresa", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_empresa");
                    TablaDatos.Columns.Add("emblema_empresa");
                    TablaDatos.Columns.Add("direccion_empresa");
                    TablaDatos.Columns.Add("telefono_empresa");
                    TablaDatos.Columns.Add("imagen_entidad", typeof(Byte[]));

                    if (TheDataReaderSQLServer != null)
                    {
                        while (TheDataReaderSQLServer.Read())
                        {
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_imagen_empresa"] = Convert.ToInt32(TheDataReaderSQLServer["id_imagen_empresa"]);
                            Fila["nombre_empresa"] = TheDataReaderSQLServer["nombre_empresa"].ToString().Trim();
                            Fila["emblema_empresa"] = TheDataReaderSQLServer["emblema_empresa"].ToString().Trim();
                            Fila["direccion_empresa"] = TheDataReaderSQLServer["direccion_empresa"].ToString().Trim();
                            Fila["telefono_empresa"] = TheDataReaderSQLServer["telefono_empresa"].ToString().Trim();

                            if (TheDataReaderSQLServer["imagen_empresa"].ToString().Trim().Length > 0)
                            {
                                Fila["imagen_entidad"] = (Byte[])TheDataReaderSQLServer["imagen_empresa"];
                            }
                            else
                            {
                                Utilidades ObjUtils = new Utilidades();
                                Byte[] ImagenByte = null;
                                string strImgDefault = "Temas/Azul/Imagenes/anonimo.gif";
                                string cRutaImagen = HttpContext.Current.Server.MapPath("/" + strImgDefault.ToString().Trim());

                                ImagenByte = ObjUtils.GetImagenBytes(cRutaImagen);

                                Fila["imagen_entidad"] = ImagenByte;
                            }
                            TablaDatos.Rows.Add(Fila);
                        }
                    }
                }
                else if (myConnectionDb is OracleConnection)
                {

                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    return TablaDatos;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al traer informacion de la tabla Navegación. Motivo: " + ex.Message);
            }
            finally
            {
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
                    TheDataReaderMySQL.Close();
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                    TheDataReaderSQLServer.Close();
                    TheDataReaderSQLServer.Close();
                }
                else if (myConnectionDb is OracleConnection)
                {
                    TheCommandOracle = null;
                    TheDataReaderOracle.Close();
                    TheDataReaderOracle.Close();
                }

                myConnectionDb.Close();
                myConnectionDb.Dispose();
            }

            return TablaDatos;
        }

        //Metodo para registrar los datos de la imagen de la empresa.
        public bool AddUpImagenEmpresa(ref int _IdRegistro, ref string _MsgError)
        {
            bool retValor = false;
            try
            {
                #region DEFINICION DEL OBJETO DE CONEXION A LA DB.
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
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_imagen_empresa", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa_img", IdLogoEmpresa);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idempresa", IdEmpresa);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_imagen_empresa", ImagenEmpresa);
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
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Base de datos MySql
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                    TheCommandSQLServer = new SqlCommand();
                    TheCommandSQLServer.CommandText = "sp_web_crud_imagen_empresa";
                    TheCommandSQLServer.CommandType = CommandType.StoredProcedure;
                    TheCommandSQLServer.Connection = (SqlConnection)myConnectionDb;
                    //se limpian los parámetros
                    TheCommandSQLServer.Parameters.Clear();

                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idempresa_img", IdLogoEmpresa);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_idempresa", IdEmpresa);
                    TheCommandSQLServer.Parameters.AddWithValue("@p_in_imagen_empresa", ImagenEmpresa);
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
                            //_IdRegistro = Valor_Retornado;
                            _MsgError = "La empresa [" + NombreEmpresa + "] ha sido registrada exitosamente ..!";
                        }
                        else if (TipoProceso == 2)
                        {
                            _MsgError = "Los datos la empresa [" + NombreEmpresa + "] han sido editados exitosamente.";
                        }
                        else if (TipoProceso == 3)
                        {
                            _MsgError = "La empresa [" + NombreEmpresa + "] ha sido eliminada exitosamente.";
                        }

                        retValor = true;
                    }
                }
                else if (myConnectionDb is OracleConnection)
                {
                    //Base de datos Oracle
                }
                else
                {
                    _log.Error("No existe configurado un Motor de Base de Datos a Trabajar !");
                    retValor = false;
                }
            }
            catch (Exception ex)
            {
                retValor = false;
                _MsgError = "Error al realizar el proceso con la Empresa [" + NombreEmpresa + "]. Motivo: " + ex.Message.ToString().Trim();
                _log.Error(_MsgError.ToString().Trim());
            }
            finally
            {
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
            }

            return retValor;
        }
        #endregion

    }
}