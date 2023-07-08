using System;
using Devart.Data.PostgreSql;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Configuration;
using log4net;
using System.Data.OracleClient;
using Smartax.Web.Application.Clases.Seguridad;

namespace Smartax.Web.Application.Clases.Reportes
{
    public class ClaseReportes
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
        PgSqlParameter NpParam = null;
        IDbTransaction Transac = null;

        SqlCommand TheCommandSQLServer = null;
        SqlDataReader TheDataReaderSQLServer = null;

        OracleCommand TheCommandOracle = null;
        OracleDataReader TheDataReaderOracle = null;
        #endregion

        #region DEFINICION DE ATRIBUTOS Y PROPIEDADES
        public object IdTipoReporte { get; set; }
        public object IdCliente { get; set; }
        public object IdFormImpuesto { get; set; }
        public object AnioGravable { get; set; }
        public int IdDepartamento { get; set; }
        public int IdCiudad { get; set; }
        public object IdUsuario { get; set; }
        public object IdEstado { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public int IdEmpresa { get; set; }
        public object IdRol { get; set; }
        public string MotorBaseDatos { get; set; }
        public object IdMunicipio { get; set; }
        public object TipoConsulta { get; set; }
        public int TipoProceso { get; set; }
        public string Mes { get; set; }

        #endregion

        public DataTable GetRptRetencionIca(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtRptRetencionIca";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_reportes_retencion_ica", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_reporte", TipoProceso);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_mes", Mes);

                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    //Reporte de Pagaduria
                    if (TipoProceso == 1)
                    {
                        #region DEFINICION DE COLUMNAS DEL DATATABLE
                        TablaDatos.Columns.Add("id", typeof(Int32));
                        TablaDatos.Columns.Add("unidad_negocio");
                        TablaDatos.Columns.Add("id_comprobante");
                        TablaDatos.Columns.Add("n_linea_comprobante", typeof(Int32));
                        TablaDatos.Columns.Add("id_set_proveedor");
                        TablaDatos.Columns.Add("id_proveedor");
                        TablaDatos.Columns.Add("numero_identificacion");
                        TablaDatos.Columns.Add("fecha_contable");
                        TablaDatos.Columns.Add("tipo_impuesto");
                        TablaDatos.Columns.Add("codigo_actividad");
                        TablaDatos.Columns.Add("importe_base_retencion");
                        TablaDatos.Columns.Add("base_retencion_moneda_base");
                        TablaDatos.Columns.Add("importe_retencion");
                        TablaDatos.Columns.Add("importe_retencion_moneda_base");
                        TablaDatos.Columns.Add("porcentaje_retencion");
                        TablaDatos.Columns.Add("moneda_transaccion");
                        TablaDatos.Columns.Add("moneda_base");
                        TablaDatos.Columns.Add("id_usuario_add");
                        TablaDatos.Columns.Add("id_usuario_up");
                        TablaDatos.Columns.Add("fecha_registro");
                        TablaDatos.Columns.Add("fecha_actualizacion");
                        TablaDatos.Columns.Add("id_estado");

                        #endregion

                        if (TheDataReaderPostgreSQL != null)
                        {
                            while (TheDataReaderPostgreSQL.Read())
                            {
                                #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                                DataRow Fila = null;
                                Fila = TablaDatos.NewRow();
                                Fila["id"] = Int32.Parse(TheDataReaderPostgreSQL["id"].ToString().Trim());
                                Fila["unidad_negocio"] = TheDataReaderPostgreSQL["unidad_negocio"].ToString().Trim();
                                Fila["id_comprobante"] = TheDataReaderPostgreSQL["id_comprobante"].ToString().Trim();
                                Fila["n_linea_comprobante"] = Int32.Parse(TheDataReaderPostgreSQL["n_linea_comprobante"].ToString().Trim());
                                Fila["id_set_proveedor"] = TheDataReaderPostgreSQL["id_set_proveedor"].ToString().Trim();
                                Fila["id_proveedor"] = TheDataReaderPostgreSQL["id_proveedor"].ToString().Trim();
                                Fila["numero_identificacion"] = TheDataReaderPostgreSQL["numero_identificacion"].ToString().Trim();
                                Fila["fecha_contable"] = TheDataReaderPostgreSQL["fecha_contable"].ToString().Trim();
                                Fila["tipo_impuesto"] = TheDataReaderPostgreSQL["tipo_impuesto"].ToString().Trim();
                                Fila["codigo_actividad"] = TheDataReaderPostgreSQL["codigo_actividad"].ToString().Trim();
                                Fila["importe_base_retencion"] = TheDataReaderPostgreSQL["importe_base_retencion"].ToString().Trim();
                                Fila["base_retencion_moneda_base"] = TheDataReaderPostgreSQL["base_retencion_moneda_base"].ToString().Trim();
                                Fila["importe_retencion"] = TheDataReaderPostgreSQL["importe_retencion"].ToString().Trim();
                                Fila["importe_retencion_moneda_base"] = TheDataReaderPostgreSQL["importe_retencion_moneda_base"].ToString().Trim();
                                Fila["porcentaje_retencion"] = TheDataReaderPostgreSQL["porcentaje_retencion"].ToString().Trim();
                                Fila["moneda_transaccion"] = TheDataReaderPostgreSQL["moneda_transaccion"].ToString().Trim();
                                Fila["moneda_base"] = TheDataReaderPostgreSQL["moneda_base"].ToString().Trim();
                                Fila["id_usuario_add"] = TheDataReaderPostgreSQL["id_usuario_add"].ToString().Trim();
                                Fila["id_usuario_up"] = TheDataReaderPostgreSQL["id_usuario_up"].ToString().Trim();
                                Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                                Fila["fecha_actualizacion"] = TheDataReaderPostgreSQL["fecha_actualizacion"].ToString().Trim();
                                Fila["id_estado"] = TheDataReaderPostgreSQL["id_estado"].ToString().Trim();

                                TablaDatos.Rows.Add(Fila);
                                #endregion
                            }
                        }
                        _MsgError = "";
                        #endregion
                    }
                    else if (TipoProceso == 2)
                    {
                        #region DEFINICION DE COLUMNAS DEL DATATABLE
                        TablaDatos.Columns.Add("id", typeof(Int32));
                        TablaDatos.Columns.Add("ica_consecutivo");
                        TablaDatos.Columns.Add("fecha_registro");
                        TablaDatos.Columns.Add("email_address");
                        TablaDatos.Columns.Add("area_a_la_que_corresponde");
                        TablaDatos.Columns.Add("nombre_del_vendedor_o_persona_que_entrego_el_bien");
                        TablaDatos.Columns.Add("nit");
                        TablaDatos.Columns.Add("digito_de_verificacion");
                        TablaDatos.Columns.Add("numero_de_la_obligacion");
                        TablaDatos.Columns.Add("sucursal_de_radicacion_de_credito");
                        TablaDatos.Columns.Add("direccion_del_domicilio_principal_del_vendedor_o_persona_que_en");
                        TablaDatos.Columns.Add("ciudad_del_domicilio_principal_de_vendedor_o_persona_que_entreg");
                        TablaDatos.Columns.Add("numero_telefonico");
                        TablaDatos.Columns.Add("correo_electronico");
                        TablaDatos.Columns.Add("fecha_de_desembolso_o_escrituracion");
                        TablaDatos.Columns.Add("mes_de_la_retencion");
                        TablaDatos.Columns.Add("anio_de_la_escrituracion");
                        TablaDatos.Columns.Add("nit_del_municipio_donde_se_encuentra_ubicado_el_inmueble");
                        TablaDatos.Columns.Add("calidad_tributaria");
                        TablaDatos.Columns.Add("es_un_activo_fijo");
                        TablaDatos.Columns.Add("codigo_dane_del_municipio_del_domicilio_principal_del_vendedor");
                        TablaDatos.Columns.Add("valor_de_la_compra_o_dacion");
                        TablaDatos.Columns.Add("tarifa_de_retencion");
                        TablaDatos.Columns.Add("digite_el_valor_de_retencion_de_ica");
                        TablaDatos.Columns.Add("ciudad_de_ubicacion_del_inmueble");
                        TablaDatos.Columns.Add("mes_del_desembolso");
                        TablaDatos.Columns.Add("url_archivo_generado");
                        TablaDatos.Columns.Add("esta_retencion_corresponde_a_una_operacion_del_anio_actual");
                        TablaDatos.Columns.Add("url_archivo_cargado");
                        TablaDatos.Columns.Add("id_usuario_add");
                        TablaDatos.Columns.Add("id_usuario_up");
                        TablaDatos.Columns.Add("fecha_registro_aud");
                        TablaDatos.Columns.Add("fecha_actualizacion");
                        TablaDatos.Columns.Add("id_estado");

                        #endregion

                        if (TheDataReaderPostgreSQL != null)
                        {
                            while (TheDataReaderPostgreSQL.Read())
                            {
                                #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                                DataRow Fila = null;
                                Fila = TablaDatos.NewRow();
                                Fila["id"] = Int32.Parse(TheDataReaderPostgreSQL["id"].ToString().Trim());
                                Fila["ica_consecutivo"] = TheDataReaderPostgreSQL["ica_consecutivo"].ToString().Trim();
                                Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                                Fila["email_address"] = TheDataReaderPostgreSQL["email_address"].ToString().Trim();
                                Fila["area_a_la_que_corresponde"] = TheDataReaderPostgreSQL["area_a_la_que_corresponde"].ToString().Trim();
                                Fila["nombre_del_vendedor_o_persona_que_entrego_el_bien"] = TheDataReaderPostgreSQL["nombre_del_vendedor_o_persona_que_entrego_el_bien"].ToString().Trim();
                                Fila["nit"] = TheDataReaderPostgreSQL["nit"].ToString().Trim();
                                Fila["digito_de_verificacion"] = TheDataReaderPostgreSQL["digito_de_verificacion"].ToString().Trim();
                                Fila["numero_de_la_obligacion"] = TheDataReaderPostgreSQL["numero_de_la_obligacion"].ToString().Trim();
                                Fila["sucursal_de_radicacion_de_credito"] = TheDataReaderPostgreSQL["sucursal_de_radicacion_de_credito"].ToString().Trim();
                                Fila["direccion_del_domicilio_principal_del_vendedor_o_persona_que_en"] = TheDataReaderPostgreSQL["direccion_del_domicilio_principal_del_vendedor_o_persona_que_en"].ToString().Trim();
                                Fila["ciudad_del_domicilio_principal_de_vendedor_o_persona_que_entreg"] = TheDataReaderPostgreSQL["ciudad_del_domicilio_principal_de_vendedor_o_persona_que_entreg"].ToString().Trim();
                                Fila["numero_telefonico"] = TheDataReaderPostgreSQL["numero_telefonico"].ToString().Trim();
                                Fila["correo_electronico"] = TheDataReaderPostgreSQL["correo_electronico"].ToString().Trim();
                                Fila["fecha_de_desembolso_o_escrituracion"] = TheDataReaderPostgreSQL["fecha_de_desembolso_o_escrituracion"].ToString().Trim();
                                Fila["mes_de_la_retencion"] = TheDataReaderPostgreSQL["mes_de_la_retencion"].ToString().Trim();
                                Fila["anio_de_la_escrituracion"] = TheDataReaderPostgreSQL["anio_de_la_escrituracion"].ToString().Trim();
                                Fila["nit_del_municipio_donde_se_encuentra_ubicado_el_inmueble"] = TheDataReaderPostgreSQL["nit_del_municipio_donde_se_encuentra_ubicado_el_inmueble"].ToString().Trim();
                                Fila["calidad_tributaria"] = TheDataReaderPostgreSQL["calidad_tributaria"].ToString().Trim();
                                Fila["es_un_activo_fijo"] = TheDataReaderPostgreSQL["es_un_activo_fijo"].ToString().Trim();
                                Fila["codigo_dane_del_municipio_del_domicilio_principal_del_vendedor"] = TheDataReaderPostgreSQL["codigo_dane_del_municipio_del_domicilio_principal_del_vendedor"].ToString().Trim();
                                Fila["valor_de_la_compra_o_dacion"] = TheDataReaderPostgreSQL["valor_de_la_compra_o_dacion"].ToString().Trim();
                                Fila["tarifa_de_retencion"] = TheDataReaderPostgreSQL["tarifa_de_retencion"].ToString().Trim();
                                Fila["digite_el_valor_de_retencion_de_ica"] = TheDataReaderPostgreSQL["digite_el_valor_de_rentencion_de_ica"].ToString().Trim();
                                Fila["ciudad_de_ubicacion_del_inmueble"] = TheDataReaderPostgreSQL["ciudad_de_ubicacion_del_inmueble"].ToString().Trim();
                                Fila["mes_del_desembolso"] = TheDataReaderPostgreSQL["mes_del_desembolso"].ToString().Trim();
                                Fila["url_archivo_generado"] = TheDataReaderPostgreSQL["url_archivo_generado"].ToString().Trim();
                                Fila["esta_retencion_corresponde_a_una_operacion_del_anio_actual"] = TheDataReaderPostgreSQL["esta_retencion_corresponde_a_una_operacion_del_anio_actual"].ToString().Trim();
                                Fila["url_archivo_cargado"] = TheDataReaderPostgreSQL["url_archivo_cargado"].ToString().Trim();
                                Fila["id_usuario_add"] = TheDataReaderPostgreSQL["id_usuario_add"].ToString().Trim();
                                Fila["id_usuario_up"] = TheDataReaderPostgreSQL["id_usuario_up"].ToString().Trim();
                                Fila["fecha_registro_aud"] = TheDataReaderPostgreSQL["fecha_registro_aud"].ToString().Trim();
                                Fila["fecha_actualizacion"] = TheDataReaderPostgreSQL["fecha_actualizacion"].ToString().Trim();
                                Fila["id_estado"] = TheDataReaderPostgreSQL["id_estado"].ToString().Trim();

                                TablaDatos.Rows.Add(Fila);
                                #endregion
                            }
                        }
                        _MsgError = "";
                    }
                    else if (TipoProceso == 3)
                    {
                        #region DEFINICION DE COLUMNAS DEL DATATABLE
                        TablaDatos.Columns.Add("id", typeof(Int32));
                        TablaDatos.Columns.Add("tipo_ident");
                        TablaDatos.Columns.Add("numero_ident");
                        TablaDatos.Columns.Add("tipo_imp");
                        TablaDatos.Columns.Add("subtipo_imp");
                        TablaDatos.Columns.Add("tarifa");
                        TablaDatos.Columns.Add("valor");
                        TablaDatos.Columns.Add("nit_teso");
                        TablaDatos.Columns.Add("nom_teso");
                        TablaDatos.Columns.Add("num_cue");
                        TablaDatos.Columns.Add("naturaleza");
                        TablaDatos.Columns.Add("tipo_compro");
                        TablaDatos.Columns.Add("numero_compro");
                        TablaDatos.Columns.Add("nombre_tercero");
                        TablaDatos.Columns.Add("base");
                        TablaDatos.Columns.Add("fecha_registro");
                        TablaDatos.Columns.Add("direccion_tercero");
                        TablaDatos.Columns.Add("correo_electronico");
                        TablaDatos.Columns.Add("departamento_tercero");
                        TablaDatos.Columns.Add("ciudad_tercero");
                        TablaDatos.Columns.Add("pais_tercero");
                        TablaDatos.Columns.Add("telefono_tercero");
                        TablaDatos.Columns.Add("id_usuario_add");
                        TablaDatos.Columns.Add("id_usuario_up");
                        TablaDatos.Columns.Add("fecha_registro_aud");
                        TablaDatos.Columns.Add("fecha_actualizacion");
                        TablaDatos.Columns.Add("id_estado");

                        #endregion

                        if (TheDataReaderPostgreSQL != null)
                        {
                            while (TheDataReaderPostgreSQL.Read())
                            {
                                #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                                DataRow Fila = null;
                                Fila = TablaDatos.NewRow();
                                Fila["id"] = Int32.Parse(TheDataReaderPostgreSQL["id"].ToString().Trim());
                                Fila["tipo_ident"] = TheDataReaderPostgreSQL["tipo_ident"].ToString().Trim();
                                Fila["numero_ident"] = TheDataReaderPostgreSQL["numero_ident"].ToString().Trim();
                                Fila["tipo_imp"] = TheDataReaderPostgreSQL["tipo_imp"].ToString().Trim();
                                Fila["subtipo_imp"] = TheDataReaderPostgreSQL["subtipo_imp"].ToString().Trim();
                                Fila["tarifa"] = TheDataReaderPostgreSQL["tarifa"].ToString().Trim();
                                Fila["valor"] = TheDataReaderPostgreSQL["valor"].ToString().Trim();
                                Fila["nit_teso"] = TheDataReaderPostgreSQL["nit_teso"].ToString().Trim();
                                Fila["nom_teso"] = TheDataReaderPostgreSQL["nom_teso"].ToString().Trim();
                                Fila["num_cue"] = TheDataReaderPostgreSQL["num_cue"].ToString().Trim();
                                Fila["naturaleza"] = TheDataReaderPostgreSQL["naturaleza"].ToString().Trim();
                                Fila["tipo_compro"] = TheDataReaderPostgreSQL["tipo_compro"].ToString().Trim();
                                Fila["numero_compro"] = TheDataReaderPostgreSQL["numero_compro"].ToString().Trim();
                                Fila["nombre_tercero"] = TheDataReaderPostgreSQL["nombre_tercero"].ToString().Trim();
                                Fila["base"] = TheDataReaderPostgreSQL["base"].ToString().Trim();
                                Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                                Fila["direccion_tercero"] = TheDataReaderPostgreSQL["direccion_tercero"].ToString().Trim();
                                Fila["correo_electronico"] = TheDataReaderPostgreSQL["correo_electronico"].ToString().Trim();
                                Fila["departamento_tercero"] = TheDataReaderPostgreSQL["departamento_tercero"].ToString().Trim();
                                Fila["ciudad_tercero"] = TheDataReaderPostgreSQL["ciudad_tercero"].ToString().Trim();
                                Fila["pais_tercero"] = TheDataReaderPostgreSQL["pais_tercero"].ToString().Trim();
                                Fila["telefono_tercero"] = TheDataReaderPostgreSQL["telefono_tercero"].ToString().Trim();
                                Fila["id_usuario_add"] = TheDataReaderPostgreSQL["id_usuario_add"].ToString().Trim();
                                Fila["id_usuario_up"] = TheDataReaderPostgreSQL["id_usuario_up"].ToString().Trim();
                                Fila["fecha_registro_aud"] = TheDataReaderPostgreSQL["fecha_registro_aud"].ToString().Trim();
                                Fila["fecha_actualizacion"] = TheDataReaderPostgreSQL["fecha_actualizacion"].ToString().Trim();
                                Fila["id_estado"] = TheDataReaderPostgreSQL["id_estado"].ToString().Trim();


                                TablaDatos.Rows.Add(Fila);
                                #endregion
                            }
                        }
                        _MsgError = "";
                    }
                    else if (TipoProceso == 4)
                    {
                        #region DEFINICION DE COLUMNAS DEL DATATABLE
                        TablaDatos.Columns.Add("id", typeof(Int32));
                        TablaDatos.Columns.Add("tipo");
                        TablaDatos.Columns.Add("impuesto");
                        TablaDatos.Columns.Add("cod_ciu");
                        TablaDatos.Columns.Add("ciudad");
                        TablaDatos.Columns.Add("nit");
                        TablaDatos.Columns.Add("tm");
                        TablaDatos.Columns.Add("marca");
                        TablaDatos.Columns.Add("fecha_inicial");
                        TablaDatos.Columns.Add("fecha_final");
                        TablaDatos.Columns.Add("valor_venta ");
                        TablaDatos.Columns.Add("establecimiento");
                        TablaDatos.Columns.Add("valor_impuesto");
                        TablaDatos.Columns.Add("valor_base");
                        TablaDatos.Columns.Add("id_usuario_add");
                        TablaDatos.Columns.Add("id_usuario_up");
                        TablaDatos.Columns.Add("fecha_registro");
                        TablaDatos.Columns.Add("fecha_actualizacion");
                        TablaDatos.Columns.Add("id_estado");

                        #endregion

                        if (TheDataReaderPostgreSQL != null)
                        {
                            while (TheDataReaderPostgreSQL.Read())
                            {
                                #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                                DataRow Fila = null;
                                Fila = TablaDatos.NewRow();
                                Fila["id"] = Int32.Parse(TheDataReaderPostgreSQL["id"].ToString().Trim());
                                Fila["tipo"] = TheDataReaderPostgreSQL["tipo"].ToString().Trim();
                                Fila["impuesto"] = TheDataReaderPostgreSQL["impuesto"].ToString().Trim();
                                Fila["cod_ciu"] = TheDataReaderPostgreSQL["cod_ciu"].ToString().Trim();
                                Fila["ciudad"] = TheDataReaderPostgreSQL["ciudad"].ToString().Trim();
                                Fila["nit"] = TheDataReaderPostgreSQL["nit"].ToString().Trim();
                                Fila["tm"] = TheDataReaderPostgreSQL["tm"].ToString().Trim();
                                Fila["marca"] = TheDataReaderPostgreSQL["marca"].ToString().Trim();
                                Fila["fecha_inicial"] = TheDataReaderPostgreSQL["fecha_inicial"].ToString().Trim();
                                Fila["fecha_final"] = TheDataReaderPostgreSQL["fecha_final"].ToString().Trim();
                                Fila["valor_venta "] = TheDataReaderPostgreSQL["valor_venta"].ToString().Trim();
                                Fila["establecimiento"] = TheDataReaderPostgreSQL["establecimiento"].ToString().Trim();
                                Fila["valor_impuesto"] = TheDataReaderPostgreSQL["valor_impuesto"].ToString().Trim();
                                Fila["valor_base"] = TheDataReaderPostgreSQL["valor_base"].ToString().Trim();
                                Fila["id_usuario_add"] = TheDataReaderPostgreSQL["id_usuario_add"].ToString().Trim();
                                Fila["id_usuario_up"] = TheDataReaderPostgreSQL["id_usuario_up"].ToString().Trim();
                                Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                                Fila["fecha_actualizacion"] = TheDataReaderPostgreSQL["fecha_actualizacion"].ToString().Trim();
                                Fila["id_estado"] = TheDataReaderPostgreSQL["id_estado"].ToString().Trim();

                                TablaDatos.Rows.Add(Fila);
                                #endregion
                            }
                        }
                        _MsgError = "";
                    }
                    else if (TipoProceso == 5)
                    {
                        #region DEFINICION DE COLUMNAS DEL DATATABLE
                        TablaDatos.Columns.Add("id", typeof(Int32));
                        TablaDatos.Columns.Add("un");
                        TablaDatos.Columns.Add("g_libros");
                        TablaDatos.Columns.Add("libro");
                        TablaDatos.Columns.Add("cuenta");
                        TablaDatos.Columns.Add("sucursal");
                        TablaDatos.Columns.Add("dependencia");
                        TablaDatos.Columns.Add("id_asiento");
                        TablaDatos.Columns.Add("fecha_comprobante");
                        TablaDatos.Columns.Add("fecha_proceso");
                        TablaDatos.Columns.Add("descripcion");
                        TablaDatos.Columns.Add("debito");
                        TablaDatos.Columns.Add("credito");
                        TablaDatos.Columns.Add("auxiliar");
                        TablaDatos.Columns.Add("referencia");
                        TablaDatos.Columns.Add("usuario");
                        TablaDatos.Columns.Add("id_comprobante");
                        TablaDatos.Columns.Add("estado");
                        TablaDatos.Columns.Add("real");
                        TablaDatos.Columns.Add("id_usuario_add");
                        TablaDatos.Columns.Add("id_usuario_up");
                        TablaDatos.Columns.Add("fecha_registro");
                        TablaDatos.Columns.Add("fecha_actualizacion");
                        TablaDatos.Columns.Add("id_estado");

                        #endregion

                        if (TheDataReaderPostgreSQL != null)
                        {
                            while (TheDataReaderPostgreSQL.Read())
                            {
                                #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                                DataRow Fila = null;
                                Fila = TablaDatos.NewRow();
                                Fila["id"] = Int32.Parse(TheDataReaderPostgreSQL["id"].ToString().Trim());
                                Fila["un"] = TheDataReaderPostgreSQL["un"].ToString().Trim();
                                Fila["g_libros"] = TheDataReaderPostgreSQL["g_libros"].ToString().Trim();
                                Fila["libro"] = TheDataReaderPostgreSQL["libro"].ToString().Trim();
                                Fila["cuenta"] = TheDataReaderPostgreSQL["cuenta"].ToString().Trim();
                                Fila["sucursal"] = TheDataReaderPostgreSQL["sucursal"].ToString().Trim();
                                Fila["dependencia"] = TheDataReaderPostgreSQL["dependencia"].ToString().Trim();
                                Fila["id_asiento"] = TheDataReaderPostgreSQL["id_asiento"].ToString().Trim();
                                Fila["fecha_comprobante"] = TheDataReaderPostgreSQL["fecha_comprobante"].ToString().Trim();
                                Fila["fecha_proceso"] = TheDataReaderPostgreSQL["fecha_proceso"].ToString().Trim();
                                Fila["descripcion"] = TheDataReaderPostgreSQL["descripcion"].ToString().Trim();
                                Fila["debito"] = TheDataReaderPostgreSQL["debito"].ToString().Trim();
                                Fila["credito"] = TheDataReaderPostgreSQL["credito"].ToString().Trim();
                                Fila["auxiliar"] = TheDataReaderPostgreSQL["auxiliar"].ToString().Trim();
                                Fila["referencia"] = TheDataReaderPostgreSQL["referencia"].ToString().Trim();
                                Fila["usuario"] = TheDataReaderPostgreSQL["usuario"].ToString().Trim();
                                Fila["id_comprobante"] = TheDataReaderPostgreSQL["id_comprobante"].ToString().Trim();
                                Fila["estado"] = TheDataReaderPostgreSQL["estado"].ToString().Trim();
                                Fila["real"] = TheDataReaderPostgreSQL["real"].ToString().Trim();
                                Fila["id_usuario_add"] = TheDataReaderPostgreSQL["id_usuario_add"].ToString().Trim();
                                Fila["id_usuario_up"] = TheDataReaderPostgreSQL["id_usuario_up"].ToString().Trim();
                                Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                                Fila["fecha_actualizacion"] = TheDataReaderPostgreSQL["fecha_actualizacion"].ToString().Trim();
                                Fila["id_estado"] = TheDataReaderPostgreSQL["id_estado"].ToString().Trim();


                                TablaDatos.Rows.Add(Fila);
                                #endregion
                            }
                        }
                        _MsgError = "";
                    }

                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_get_reportes_retencion_ica]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptAcuerdosMunicipales(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtRptAcuerdosMunicipales";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_acuerdos_municipales", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idacuerdo_municipal", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("nombre_impuesto");
                    TablaDatos.Columns.Add("tipo_normatividad");
                    TablaDatos.Columns.Add("nombre_archivo");
                    TablaDatos.Columns.Add("fecha_acuerdo");
                    TablaDatos.Columns.Add("fecha_registro");

                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idacuerdo_municipal"] = Int32.Parse(TheDataReaderPostgreSQL["idacuerdo_municipal"].ToString().Trim());
                            Fila["nombre_departamento"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["nombre_impuesto"] = TheDataReaderPostgreSQL["nombre_impuesto"].ToString().Trim();
                            Fila["tipo_normatividad"] = TheDataReaderPostgreSQL["tipo_normatividad"].ToString().Trim();
                            Fila["nombre_archivo"] = TheDataReaderPostgreSQL["nombre_archivo"].ToString().Trim();
                            Fila["fecha_acuerdo"] = TheDataReaderPostgreSQL["fecha_acuerdo"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();

                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_acuerdos_municipales]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptMunicipios(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtRptMunicipios";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_municipios", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("id_registro", typeof(Int32));
                    TablaDatos.Columns.Add("id_dpto", typeof(Int32));
                    TablaDatos.Columns.Add("cod_dane_dpto");
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("cod_dane_municipio");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("numero_nit");
                    TablaDatos.Columns.Add("digito_verificacion");
                    TablaDatos.Columns.Add("nombre_contacto");
                    TablaDatos.Columns.Add("direccion_contacto");
                    TablaDatos.Columns.Add("telefono_contacto");
                    TablaDatos.Columns.Add("email_contacto");
                    TablaDatos.Columns.Add("liquidacion_mixta");
                    TablaDatos.Columns.Add("linea_nacional");
                    TablaDatos.Columns.Add("indicativo");
                    TablaDatos.Columns.Add("horario_atencion");
                    TablaDatos.Columns.Add("url");
                    TablaDatos.Columns.Add("chat");
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("estado_municipio");
                    TablaDatos.Columns.Add("fecha_registro");
                    TablaDatos.Columns.Add("fecha_actualizacion");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_registro"] = TablaDatos.Rows.Count + 1;
                            Fila["id_dpto"] = Int32.Parse(TheDataReaderPostgreSQL["id_dpto"].ToString().Trim());
                            Fila["cod_dane_dpto"] = TheDataReaderPostgreSQL["cod_dane_dpto"].ToString().Trim();
                            Fila["nombre_departamento"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["cod_dane_municipio"] = TheDataReaderPostgreSQL["cod_dane_municipio"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["numero_nit"] = objUtils.GetLimpiarCadena(TheDataReaderPostgreSQL["numero_nit"].ToString().Trim());
                            Fila["digito_verificacion"] = TheDataReaderPostgreSQL["digito_verificacion"].ToString().Trim();
                            Fila["nombre_contacto"] = objUtils.GetLimpiarCadena(TheDataReaderPostgreSQL["nombre_contacto"].ToString().Trim());
                            Fila["direccion_contacto"] = objUtils.GetLimpiarCadena(TheDataReaderPostgreSQL["direccion_contacto"].ToString().Trim());
                            Fila["telefono_contacto"] = objUtils.GetLimpiarCadena(TheDataReaderPostgreSQL["telefono_contacto"].ToString().Trim());
                            Fila["email_contacto"] = objUtils.GetLimpiarCadena(TheDataReaderPostgreSQL["email_contacto"].ToString().Trim());
                            Fila["liquidacion_mixta"] = TheDataReaderPostgreSQL["liquidacion_mixta"].ToString().Trim();
                            Fila["linea_nacional"] = objUtils.GetLimpiarCadena(TheDataReaderPostgreSQL["linea_nacional"].ToString().Trim());
                            Fila["indicativo"] = objUtils.GetLimpiarCadena(TheDataReaderPostgreSQL["indicativo"].ToString().Trim());
                            Fila["horario_atencion"] = objUtils.GetLimpiarCadena(TheDataReaderPostgreSQL["horario_atencion"].ToString().Trim());
                            Fila["url"] = objUtils.GetLimpiarCadena(TheDataReaderPostgreSQL["url"].ToString().Trim());
                            Fila["chat"] = objUtils.GetLimpiarCadena(TheDataReaderPostgreSQL["chat"].ToString().Trim());
                            Fila["id_estado"] = Int32.Parse(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["estado_municipio"] = TheDataReaderPostgreSQL["estado_municipio"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            Fila["fecha_actualizacion"] = TheDataReaderPostgreSQL["fecha_actualizacion"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_municipios]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptEstablecimientos(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtRptEstablecimientos";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_establecimientos", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idcliente_establecimiento", typeof(Int32));
                    TablaDatos.Columns.Add("id_dpto", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("codigo_oficina");
                    TablaDatos.Columns.Add("nombre_oficina");
                    TablaDatos.Columns.Add("nombre_contacto");
                    TablaDatos.Columns.Add("direccion_contacto");
                    TablaDatos.Columns.Add("telefono_contacto");
                    TablaDatos.Columns.Add("inscrito_rit");
                    TablaDatos.Columns.Add("numero_establecimiento", typeof(Int32));
                    TablaDatos.Columns.Add("numero_placa_municipal");
                    TablaDatos.Columns.Add("numero_matricula_ic");
                    TablaDatos.Columns.Add("numero_rit");
                    TablaDatos.Columns.Add("avisos_tablero");
                    TablaDatos.Columns.Add("sucursal");
                    TablaDatos.Columns.Add("oficina_pagadora");
                    TablaDatos.Columns.Add("fecha_inicio_actividades");
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idcliente_establecimiento"] = Int32.Parse(TheDataReaderPostgreSQL["idcliente_establecimiento"].ToString().Trim());
                            Fila["id_dpto"] = Int32.Parse(TheDataReaderPostgreSQL["id_dpto"].ToString().Trim());
                            Fila["nombre_departamento"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["codigo_oficina"] = TheDataReaderPostgreSQL["codigo_oficina"].ToString().Trim();
                            Fila["nombre_oficina"] = TheDataReaderPostgreSQL["nombre_oficina"].ToString().Trim();
                            Fila["nombre_contacto"] = TheDataReaderPostgreSQL["nombre_contacto"].ToString().Trim();
                            Fila["direccion_contacto"] = TheDataReaderPostgreSQL["direccion_contacto"].ToString().Trim();
                            Fila["telefono_contacto"] = TheDataReaderPostgreSQL["telefono_contacto"].ToString().Trim();
                            Fila["inscrito_rit"] = TheDataReaderPostgreSQL["inscrito_rit"].ToString().Trim().Equals("S") ? "SI" : "NO";
                            Fila["numero_establecimiento"] = Int32.Parse(TheDataReaderPostgreSQL["numero_puntos"].ToString().Trim());
                            //---
                            Fila["numero_placa_municipal"] = TheDataReaderPostgreSQL["numero_placa_municipal"].ToString().Trim();
                            Fila["numero_matricula_ic"] = TheDataReaderPostgreSQL["numero_matricula_ic"].ToString().Trim();
                            Fila["numero_rit"] = TheDataReaderPostgreSQL["numero_rit"].ToString().Trim();
                            Fila["avisos_tablero"] = TheDataReaderPostgreSQL["avisos_tablero"].ToString().Trim().Equals("S") ? "SI" : "NO";
                            Fila["sucursal"] = TheDataReaderPostgreSQL["sucursal"].ToString().Trim();
                            Fila["oficina_pagadora"] = TheDataReaderPostgreSQL["oficina_pagadora"].ToString().Trim().Equals("S") ? "SI" : "NO";
                            Fila["fecha_inicio_actividades"] = TheDataReaderPostgreSQL["fecha_inicio_actividades"].ToString().Trim().Length > 0 ? TheDataReaderPostgreSQL["fecha_inicio_actividades"].ToString().Trim() : "";

                            Fila["id_estado"] = Int32.Parse(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_comercios]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptMunicipiosCantPuntos(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtMunicipiosCantPuntos";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_get_municipios_cant_puntos", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_consulta", TipoConsulta);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idmunicipio", IdMunicipio);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idmunicipio_cant_puntos", typeof(Int32));
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("numero_oficinas", typeof(Int32));
                    TablaDatos.Columns.Add("fecha_registro");
                    TablaDatos.Columns.Add("fecha_actualizacion");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idmunicipio_cant_puntos"] = Int32.Parse(TheDataReaderPostgreSQL["idmunicipio_cant_puntos"].ToString().Trim());
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["numero_oficinas"] = Int32.Parse(TheDataReaderPostgreSQL["cantidad_puntos"].ToString().Trim());
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            Fila["fecha_actualizacion"] = TheDataReaderPostgreSQL["fecha_actualizacion"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
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
                _MsgError = "Error al obtener los datos de la Tabla [tbl_municipio_cant_puntos]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptCalendarioTribut(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtCalendarioTribut";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_calendariotribut", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idmun_calendario_trib", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_formulario");
                    TablaDatos.Columns.Add("anio_gravable");
                    TablaDatos.Columns.Add("periodicidad_impuesto");
                    TablaDatos.Columns.Add("periodicidad_pago");
                    TablaDatos.Columns.Add("fecha_limite");
                    TablaDatos.Columns.Add("valor_descuento");
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idmun_calendario_trib"] = Int32.Parse(TheDataReaderPostgreSQL["idmun_calendario_trib"].ToString().Trim());
                            Fila["nombre_departamento"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["idformulario_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["nombre_formulario"] = TheDataReaderPostgreSQL["nombre_formulario"].ToString().Trim();
                            Fila["anio_gravable"] = TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim();
                            Fila["periodicidad_impuesto"] = TheDataReaderPostgreSQL["periodicidad_impuesto"].ToString().Trim();
                            Fila["periodicidad_pago"] = TheDataReaderPostgreSQL["periodicidad_pago"].ToString().Trim();
                            Fila["fecha_limite"] = TheDataReaderPostgreSQL["fecha_limite"].ToString().Trim();
                            Fila["valor_descuento"] = TheDataReaderPostgreSQL["valor_descuento"].ToString().Trim();
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_comercios]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptActEconomicas(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtActEconomicas";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_act_economicas", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idmun_act_economica", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_formulario");
                    TablaDatos.Columns.Add("anio_gravable");
                    TablaDatos.Columns.Add("idtipo_actividad", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_actividad");
                    TablaDatos.Columns.Add("codigo_agrupacion");
                    TablaDatos.Columns.Add("codigo_actividad");
                    TablaDatos.Columns.Add("descripcion_actividad");
                    TablaDatos.Columns.Add("idtipo_tarifa", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_tarifa");
                    TablaDatos.Columns.Add("tarifa_ley");
                    TablaDatos.Columns.Add("tarifa_municipio");
                    TablaDatos.Columns.Add("numero_acuerdo");
                    TablaDatos.Columns.Add("numero_articulo");
                    //TablaDatos.Columns.Add("version_registro");
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    TablaDatos.Columns.Add("fecha_modificacion");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idmun_act_economica"] = Int32.Parse(TheDataReaderPostgreSQL["idmun_act_economica"].ToString().Trim());
                            Fila["nombre_departamento"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["idformulario_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["nombre_formulario"] = TheDataReaderPostgreSQL["nombre_formulario"].ToString().Trim();
                            Fila["anio_gravable"] = TheDataReaderPostgreSQL["anio_actividad"].ToString().Trim();
                            Fila["idtipo_actividad"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_actividad"].ToString().Trim());
                            Fila["tipo_actividad"] = TheDataReaderPostgreSQL["tipo_actividad"].ToString().Trim();
                            Fila["codigo_agrupacion"] = TheDataReaderPostgreSQL["codigo_agrupacion"].ToString().Trim();
                            Fila["codigo_actividad"] = TheDataReaderPostgreSQL["codigo_actividad"].ToString().Trim();
                            Fila["descripcion_actividad"] = TheDataReaderPostgreSQL["descripcion_actividad"].ToString().Trim();
                            Fila["idtipo_tarifa"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_tarifa"].ToString().Trim());
                            Fila["tipo_tarifa"] = TheDataReaderPostgreSQL["tipo_tarifa"].ToString().Trim();
                            Fila["tarifa_ley"] = TheDataReaderPostgreSQL["tarifa_ley"].ToString().Trim();
                            Fila["tarifa_municipio"] = TheDataReaderPostgreSQL["tarifa_municipio"].ToString().Trim();
                            Fila["numero_acuerdo"] = TheDataReaderPostgreSQL["numero_acuerdo"].ToString().Trim();
                            Fila["numero_articulo"] = TheDataReaderPostgreSQL["numero_articulo"].ToString().Trim();
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            Fila["fecha_modificacion"] = TheDataReaderPostgreSQL["fecha_modificacion"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_comercios]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptImpMunicipio(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtImpMunicipio";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_imp_municipios", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idmun_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_formulario");
                    TablaDatos.Columns.Add("anio_gravable", typeof(Int32));     //--7
                    TablaDatos.Columns.Add("id_periodicidad", typeof(Int32));
                    TablaDatos.Columns.Add("periodicidad_pago");
                    TablaDatos.Columns.Add("descripcion_impuesto");
                    TablaDatos.Columns.Add("operacion_renglon");
                    TablaDatos.Columns.Add("idtipo_tarifa", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_tarifa");
                    TablaDatos.Columns.Add("valor_tarifa");     //--14
                    //TablaDatos.Columns.Add("version_registro");
                    TablaDatos.Columns.Add("idform_configuracion", typeof(Int32));
                    TablaDatos.Columns.Add("numero_renglon");
                    TablaDatos.Columns.Add("descripcion_renglon");
                    TablaDatos.Columns.Add("calcular_renglon");
                    //--CONFIGURACION 1
                    TablaDatos.Columns.Add("idform_configuracion1");
                    TablaDatos.Columns.Add("numero_renglon1");
                    //TablaDatos.Columns.Add("descripcion_renglon1");
                    TablaDatos.Columns.Add("tipo_operacion1");
                    //--CONFIGURACION 2
                    TablaDatos.Columns.Add("idform_configuracion2");
                    TablaDatos.Columns.Add("numero_renglon2");
                    //TablaDatos.Columns.Add("descripcion_renglon2");
                    TablaDatos.Columns.Add("tipo_operacion2");
                    //--CONFIGURACION 3
                    TablaDatos.Columns.Add("idform_configuracion3");
                    TablaDatos.Columns.Add("numero_renglon3");
                    //TablaDatos.Columns.Add("descripcion_renglon3");
                    TablaDatos.Columns.Add("tipo_operacion3");
                    //--CONFIGURACION 4
                    TablaDatos.Columns.Add("idform_configuracion4");
                    TablaDatos.Columns.Add("numero_renglon4");
                    //TablaDatos.Columns.Add("descripcion_renglon4");
                    TablaDatos.Columns.Add("tipo_operacion4");
                    //--CONFIGURACION 5
                    TablaDatos.Columns.Add("idform_configuracion5");
                    TablaDatos.Columns.Add("numero_renglon5");
                    //TablaDatos.Columns.Add("descripcion_renglon5");
                    TablaDatos.Columns.Add("tipo_operacion5");
                    //--CONFIGURACION 6
                    TablaDatos.Columns.Add("idform_configuracion6");
                    TablaDatos.Columns.Add("numero_renglon6");
                    //TablaDatos.Columns.Add("descripcion_renglon6");

                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    TablaDatos.Columns.Add("fecha_modificacion");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idmun_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idmun_impuesto"].ToString().Trim());
                            Fila["nombre_departamento"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["idformulario_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["nombre_formulario"] = TheDataReaderPostgreSQL["nombre_formulario"].ToString().Trim();
                            Fila["anio_gravable"] = Int32.Parse(TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim());
                            Fila["id_periodicidad"] = Int32.Parse(TheDataReaderPostgreSQL["id_periodicidad"].ToString().Trim());
                            Fila["periodicidad_pago"] = TheDataReaderPostgreSQL["periodicidad_pago"].ToString().Trim();
                            Fila["descripcion_impuesto"] = TheDataReaderPostgreSQL["descripcion_impuesto"].ToString().Trim();
                            Fila["operacion_renglon"] = TheDataReaderPostgreSQL["operacion_renglon"].ToString().Trim();
                            Fila["idtipo_tarifa"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_tarifa"].ToString().Trim());
                            Fila["tipo_tarifa"] = TheDataReaderPostgreSQL["tipo_tarifa"].ToString().Trim();
                            Fila["valor_tarifa"] = TheDataReaderPostgreSQL["valor_tarifa"].ToString().Trim();
                            Fila["idform_configuracion"] = TheDataReaderPostgreSQL["idform_configuracion"].ToString().Trim();
                            Fila["numero_renglon"] = TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim();
                            Fila["descripcion_renglon"] = TheDataReaderPostgreSQL["descripcion_renglon"].ToString().Trim();
                            Fila["calcular_renglon"] = TheDataReaderPostgreSQL["calcular_renglon"].ToString().Trim();
                            //--CONFIGURACION 1
                            Fila["idform_configuracion1"] = TheDataReaderPostgreSQL["idform_configuracion1"].ToString().Trim();
                            Fila["numero_renglon1"] = TheDataReaderPostgreSQL["numero_renglon1"].ToString().Trim();
                            //Fila["descripcion_renglon1"] = TheDataReaderPostgreSQL["descripcion_renglon1"].ToString().Trim();
                            Fila["tipo_operacion1"] = TheDataReaderPostgreSQL["tipo_operacion1"].ToString().Trim();
                            //--CONFIGURACION 2
                            Fila["idform_configuracion2"] = TheDataReaderPostgreSQL["idform_configuracion2"].ToString().Trim();
                            Fila["numero_renglon2"] = TheDataReaderPostgreSQL["numero_renglon2"].ToString().Trim();
                            //Fila["descripcion_renglon2"] = TheDataReaderPostgreSQL["descripcion_renglon2"].ToString().Trim();
                            Fila["tipo_operacion2"] = TheDataReaderPostgreSQL["tipo_operacion2"].ToString().Trim();
                            //--CONFIGURACION 3
                            Fila["idform_configuracion3"] = TheDataReaderPostgreSQL["idform_configuracion3"].ToString().Trim();
                            Fila["numero_renglon3"] = TheDataReaderPostgreSQL["numero_renglon3"].ToString().Trim();
                            //Fila["descripcion_renglon3"] = TheDataReaderPostgreSQL["descripcion_renglon3"].ToString().Trim();
                            Fila["tipo_operacion3"] = TheDataReaderPostgreSQL["tipo_operacion3"].ToString().Trim();
                            //--CONFIGURACION 4
                            Fila["idform_configuracion4"] = TheDataReaderPostgreSQL["idform_configuracion4"].ToString().Trim();
                            Fila["numero_renglon4"] = TheDataReaderPostgreSQL["numero_renglon4"].ToString().Trim();
                            //Fila["descripcion_renglon4"] = TheDataReaderPostgreSQL["descripcion_renglon4"].ToString().Trim();
                            Fila["tipo_operacion4"] = TheDataReaderPostgreSQL["tipo_operacion4"].ToString().Trim();
                            //--CONFIGURACION 5
                            Fila["idform_configuracion5"] = TheDataReaderPostgreSQL["idform_configuracion5"].ToString().Trim();
                            Fila["numero_renglon5"] = TheDataReaderPostgreSQL["numero_renglon5"].ToString().Trim();
                            //Fila["descripcion_renglon5"] = TheDataReaderPostgreSQL["descripcion_renglon5"].ToString().Trim();
                            Fila["tipo_operacion5"] = TheDataReaderPostgreSQL["tipo_operacion5"].ToString().Trim();
                            //--CONFIGURACION 6
                            Fila["idform_configuracion6"] = TheDataReaderPostgreSQL["idform_configuracion6"].ToString().Trim();
                            Fila["numero_renglon6"] = TheDataReaderPostgreSQL["numero_renglon6"].ToString().Trim();
                            //Fila["descripcion_renglon6"] = TheDataReaderPostgreSQL["descripcion_renglon6"].ToString().Trim();

                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            Fila["fecha_modificacion"] = TheDataReaderPostgreSQL["fecha_modificacion"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_comercios]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptMunicipioAutoret(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtMunicipioAutoret";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_municipios_autoret", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idmun_autoretenciones", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("nombre_formulario");
                    TablaDatos.Columns.Add("numero_renglon", typeof(Int32));
                    TablaDatos.Columns.Add("valor_autoretencion", typeof(Double));
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    TablaDatos.Columns.Add("fecha_actualizacion");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idmun_autoretenciones"] = Int32.Parse(TheDataReaderPostgreSQL["idmun_autoretenciones"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["nombre_formulario"] = TheDataReaderPostgreSQL["descripcion_formulario"].ToString().Trim();
                            Fila["numero_renglon"] = Int32.Parse(TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim());
                            Fila["valor_autoretencion"] = Double.Parse(TheDataReaderPostgreSQL["valor_autoretencion"].ToString().Trim());
                            //--
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            Fila["fecha_actualizacion"] = TheDataReaderPostgreSQL["fecha_modificacion"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_comercios]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptOtrasConfiguraciones(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtOtrasConfiguraciones";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_otras_configuraciones", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idotras_config", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_formulario");
                    TablaDatos.Columns.Add("idunidad_medida", typeof(Int32));
                    TablaDatos.Columns.Add("unidad_medida");
                    TablaDatos.Columns.Add("descripcion_unid_medida");
                    TablaDatos.Columns.Add("idvalor_unid_medida", typeof(Int32));
                    TablaDatos.Columns.Add("anio_fiscal");
                    TablaDatos.Columns.Add("idunid_medida_bg", typeof(Int32));
                    TablaDatos.Columns.Add("anio_gravable");
                    TablaDatos.Columns.Add("valor_anio_fiscal");
                    TablaDatos.Columns.Add("cantidad_medida");
                    TablaDatos.Columns.Add("cantidad_periodos");
                    TablaDatos.Columns.Add("valor_unid_medida");
                    TablaDatos.Columns.Add("descripcion_configuracion");
                    TablaDatos.Columns.Add("idtipo_tarifa", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_tarifa");
                    //TablaDatos.Columns.Add("version_registro");
                    TablaDatos.Columns.Add("idform_configuracion", typeof(Int32));
                    TablaDatos.Columns.Add("numero_renglon");
                    TablaDatos.Columns.Add("descripcion_renglon");
                    TablaDatos.Columns.Add("calcular_renglon");
                    //--CONFIGURACION 1
                    TablaDatos.Columns.Add("idform_configuracion1");
                    TablaDatos.Columns.Add("numero_renglon1");
                    //TablaDatos.Columns.Add("descripcion_renglon1");
                    TablaDatos.Columns.Add("tipo_operacion1");
                    //--CONFIGURACION 2
                    TablaDatos.Columns.Add("idform_configuracion2");
                    TablaDatos.Columns.Add("numero_renglon2");
                    //TablaDatos.Columns.Add("descripcion_renglon2");
                    TablaDatos.Columns.Add("tipo_operacion2");
                    //--CONFIGURACION 3
                    TablaDatos.Columns.Add("idform_configuracion3");
                    TablaDatos.Columns.Add("numero_renglon3");
                    //TablaDatos.Columns.Add("descripcion_renglon3");
                    TablaDatos.Columns.Add("tipo_operacion3");
                    //--CONFIGURACION 4
                    TablaDatos.Columns.Add("idform_configuracion4");
                    TablaDatos.Columns.Add("numero_renglon4");
                    //TablaDatos.Columns.Add("descripcion_renglon4");
                    TablaDatos.Columns.Add("tipo_operacion4");
                    //--CONFIGURACION 5
                    TablaDatos.Columns.Add("idform_configuracion5");
                    TablaDatos.Columns.Add("numero_renglon5");
                    //TablaDatos.Columns.Add("descripcion_renglon5");
                    TablaDatos.Columns.Add("tipo_operacion5");
                    //--CONFIGURACION 6
                    TablaDatos.Columns.Add("idform_configuracion6");
                    TablaDatos.Columns.Add("numero_renglon6");
                    //TablaDatos.Columns.Add("descripcion_renglon6");

                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    TablaDatos.Columns.Add("fecha_modificacion");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idotras_config"] = Int32.Parse(TheDataReaderPostgreSQL["idmun_tarifa_minima"].ToString().Trim());
                            Fila["nombre_departamento"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["idformulario_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["nombre_formulario"] = TheDataReaderPostgreSQL["nombre_formulario"].ToString().Trim();
                            Fila["idunidad_medida"] = Int32.Parse(TheDataReaderPostgreSQL["idunidad_medida"].ToString().Trim());
                            Fila["unidad_medida"] = TheDataReaderPostgreSQL["unidad_medida"].ToString().Trim();
                            Fila["descripcion_unid_medida"] = TheDataReaderPostgreSQL["descripcion_unid_medida"].ToString().Trim();
                            //--
                            Fila["idvalor_unid_medida"] = Int32.Parse(TheDataReaderPostgreSQL["idvalor_unid_medida"].ToString().Trim());
                            Fila["anio_fiscal"] = TheDataReaderPostgreSQL["anio_fiscal"].ToString().Trim();
                            Fila["idunid_medida_bg"] = Int32.Parse(TheDataReaderPostgreSQL["idunid_medida_bg"].ToString().Trim());
                            Fila["anio_gravable"] = TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim();
                            //--
                            Fila["valor_anio_fiscal"] = TheDataReaderPostgreSQL["valor_anio_fiscal"].ToString().Trim();
                            Fila["cantidad_medida"] = TheDataReaderPostgreSQL["cantidad_medida"].ToString().Trim();
                            Fila["cantidad_periodos"] = TheDataReaderPostgreSQL["cantidad_periodos"].ToString().Trim();
                            Fila["valor_unid_medida"] = TheDataReaderPostgreSQL["valor_unid_medida"].ToString().Trim();
                            Fila["descripcion_configuracion"] = TheDataReaderPostgreSQL["descripcion_configuracion"].ToString().Trim();
                            Fila["idtipo_tarifa"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_tarifa"].ToString().Trim());
                            Fila["tipo_tarifa"] = TheDataReaderPostgreSQL["tipo_tarifa"].ToString().Trim();
                            //--
                            Fila["idform_configuracion"] = TheDataReaderPostgreSQL["idform_configuracion"].ToString().Trim();
                            Fila["numero_renglon"] = TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim();
                            Fila["descripcion_renglon"] = TheDataReaderPostgreSQL["descripcion_renglon"].ToString().Trim();
                            Fila["calcular_renglon"] = TheDataReaderPostgreSQL["calcular_renglon"].ToString().Trim();
                            //--CONFIGURACION 1
                            Fila["idform_configuracion1"] = TheDataReaderPostgreSQL["idform_configuracion1"].ToString().Trim();
                            Fila["numero_renglon1"] = TheDataReaderPostgreSQL["numero_renglon1"].ToString().Trim();
                            //Fila["descripcion_renglon1"] = TheDataReaderPostgreSQL["descripcion_renglon1"].ToString().Trim();
                            Fila["tipo_operacion1"] = TheDataReaderPostgreSQL["tipo_operacion1"].ToString().Trim();
                            //--CONFIGURACION 2
                            Fila["idform_configuracion2"] = TheDataReaderPostgreSQL["idform_configuracion2"].ToString().Trim();
                            Fila["numero_renglon2"] = TheDataReaderPostgreSQL["numero_renglon2"].ToString().Trim();
                            //Fila["descripcion_renglon2"] = TheDataReaderPostgreSQL["descripcion_renglon2"].ToString().Trim();
                            Fila["tipo_operacion2"] = TheDataReaderPostgreSQL["tipo_operacion2"].ToString().Trim();
                            //--CONFIGURACION 3
                            Fila["idform_configuracion3"] = TheDataReaderPostgreSQL["idform_configuracion3"].ToString().Trim();
                            Fila["numero_renglon3"] = TheDataReaderPostgreSQL["numero_renglon3"].ToString().Trim();
                            //Fila["descripcion_renglon3"] = TheDataReaderPostgreSQL["descripcion_renglon3"].ToString().Trim();
                            Fila["tipo_operacion3"] = TheDataReaderPostgreSQL["tipo_operacion3"].ToString().Trim();
                            //--CONFIGURACION 4
                            Fila["idform_configuracion4"] = TheDataReaderPostgreSQL["idform_configuracion4"].ToString().Trim();
                            Fila["numero_renglon4"] = TheDataReaderPostgreSQL["numero_renglon4"].ToString().Trim();
                            //Fila["descripcion_renglon4"] = TheDataReaderPostgreSQL["descripcion_renglon4"].ToString().Trim();
                            Fila["tipo_operacion4"] = TheDataReaderPostgreSQL["tipo_operacion4"].ToString().Trim();
                            //--CONFIGURACION 5
                            Fila["idform_configuracion5"] = TheDataReaderPostgreSQL["idform_configuracion5"].ToString().Trim();
                            Fila["numero_renglon5"] = TheDataReaderPostgreSQL["numero_renglon5"].ToString().Trim();
                            //Fila["descripcion_renglon5"] = TheDataReaderPostgreSQL["descripcion_renglon5"].ToString().Trim();
                            Fila["tipo_operacion5"] = TheDataReaderPostgreSQL["tipo_operacion5"].ToString().Trim();
                            //--CONFIGURACION 6
                            Fila["idform_configuracion6"] = TheDataReaderPostgreSQL["idform_configuracion6"].ToString().Trim();
                            Fila["numero_renglon6"] = TheDataReaderPostgreSQL["numero_renglon6"].ToString().Trim();
                            //Fila["descripcion_renglon6"] = TheDataReaderPostgreSQL["descripcion_renglon6"].ToString().Trim();

                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            Fila["fecha_modificacion"] = TheDataReaderPostgreSQL["fecha_modificacion"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_comercios]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptPlanUnicoCuenta(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtRptPlanUnicoCuenta";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_puc", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region AQUI DEFINIMOS LAS COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("id_puc", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_cuenta");
                    TablaDatos.Columns.Add("cod_cuenta_padre");
                    TablaDatos.Columns.Add("nombre_cuenta");
                    TablaDatos.Columns.Add("base_gravable");
                    TablaDatos.Columns.Add("id_movimiento");
                    TablaDatos.Columns.Add("tipo_movimiento");
                    TablaDatos.Columns.Add("idtipo_nivel", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_nivel");
                    TablaDatos.Columns.Add("idtipo_naturaleza", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_naturaleza");
                    TablaDatos.Columns.Add("idtipo_moneda", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_moneda");
                    TablaDatos.Columns.Add("id_cliente");
                    TablaDatos.Columns.Add("nombre_cliente");
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
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
                            Fila["id_puc"] = Int32.Parse(TheDataReaderPostgreSQL["id_puc"].ToString().Trim());
                            Fila["codigo_cuenta"] = TheDataReaderPostgreSQL["codigo_cuenta"].ToString().Trim();
                            Fila["cod_cuenta_padre"] = TheDataReaderPostgreSQL["cod_cuenta_padre"].ToString().Trim();
                            Fila["nombre_cuenta"] = TheDataReaderPostgreSQL["nombre_cuenta"].ToString().Trim();
                            Fila["base_gravable"] = TheDataReaderPostgreSQL["base_gravable"].ToString().Trim();
                            Fila["id_movimiento"] = TheDataReaderPostgreSQL["movimiento"].ToString().Trim();
                            Fila["tipo_movimiento"] = TheDataReaderPostgreSQL["movimiento"].ToString().Trim();

                            Fila["idtipo_nivel"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_nivel"].ToString().Trim());
                            Fila["tipo_nivel"] = TheDataReaderPostgreSQL["tipo_nivel"].ToString().Trim();
                            Fila["idtipo_naturaleza"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_naturaleza"].ToString().Trim());
                            Fila["tipo_naturaleza"] = TheDataReaderPostgreSQL["tipo_naturaleza"].ToString().Trim();
                            Fila["idtipo_moneda"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_moneda"].ToString().Trim());
                            Fila["codigo_moneda"] = TheDataReaderPostgreSQL["codigo_moneda"].ToString().Trim();
                            Fila["id_cliente"] = TheDataReaderPostgreSQL["id_cliente"].ToString().Trim();
                            Fila["nombre_cliente"] = TheDataReaderPostgreSQL["nombre_cliente"].ToString().Trim();

                            Fila["id_estado"] = Int32.Parse(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_comercios]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptConfiguracionImp(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtConfiguracionImp";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_configuracion_imp", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idcliente_base_gravable", typeof(Int32));
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("formulario_impuesto");
                    TablaDatos.Columns.Add("anio_gravable");
                    TablaDatos.Columns.Add("idform_configuracion", typeof(Int32));
                    TablaDatos.Columns.Add("numero_renglon");
                    TablaDatos.Columns.Add("descripcion_renglon");
                    TablaDatos.Columns.Add("renglon_calculado");
                    TablaDatos.Columns.Add("id_puc", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_cuenta");
                    TablaDatos.Columns.Add("cod_cuenta_padre");
                    TablaDatos.Columns.Add("nombre_cuenta");
                    TablaDatos.Columns.Add("base_gravable");
                    TablaDatos.Columns.Add("movimiento");
                    TablaDatos.Columns.Add("tipo_nivel");
                    TablaDatos.Columns.Add("saldo_inicial");
                    TablaDatos.Columns.Add("mov_debito");
                    TablaDatos.Columns.Add("mov_credito");
                    TablaDatos.Columns.Add("saldo_final");
                    TablaDatos.Columns.Add("valor_extracontable", typeof(Double));
                    TablaDatos.Columns.Add("idcliente_establecimiento");
                    TablaDatos.Columns.Add("codigo_oficina");
                    TablaDatos.Columns.Add("nombre_oficina");
                    TablaDatos.Columns.Add("id_estado", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    TablaDatos.Columns.Add("fecha_modificacion");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idcliente_base_gravable"] = Int32.Parse(TheDataReaderPostgreSQL["idcliente_base_gravable"].ToString().Trim());
                            Fila["idformulario_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["formulario_impuesto"] = TheDataReaderPostgreSQL["formulario_impuesto"].ToString().Trim();
                            Fila["anio_gravable"] = TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim();
                            Fila["idform_configuracion"] = Int32.Parse(TheDataReaderPostgreSQL["idform_configuracion"].ToString().Trim());
                            Fila["numero_renglon"] = TheDataReaderPostgreSQL["numero_renglon"].ToString().Trim();
                            Fila["descripcion_renglon"] = TheDataReaderPostgreSQL["descripcion_renglon"].ToString().Trim();
                            Fila["renglon_calculado"] = TheDataReaderPostgreSQL["renglon_calculado"].ToString().Trim();

                            Fila["id_puc"] = Int32.Parse(TheDataReaderPostgreSQL["id_puc"].ToString().Trim());
                            Fila["codigo_cuenta"] = TheDataReaderPostgreSQL["codigo_cuenta"].ToString().Trim();
                            Fila["cod_cuenta_padre"] = TheDataReaderPostgreSQL["cod_cuenta_padre"].ToString().Trim();
                            Fila["nombre_cuenta"] = TheDataReaderPostgreSQL["nombre_cuenta"].ToString().Trim();
                            Fila["base_gravable"] = TheDataReaderPostgreSQL["base_gravable"].ToString().Trim();
                            Fila["movimiento"] = TheDataReaderPostgreSQL["movimiento"].ToString().Trim();
                            Fila["tipo_nivel"] = TheDataReaderPostgreSQL["tipo_nivel"].ToString().Trim();

                            Fila["saldo_inicial"] = TheDataReaderPostgreSQL["saldo_inicial"].ToString().Trim();
                            Fila["mov_debito"] = TheDataReaderPostgreSQL["mov_debito"].ToString().Trim();
                            Fila["mov_credito"] = TheDataReaderPostgreSQL["mov_credito"].ToString().Trim();
                            Fila["saldo_final"] = TheDataReaderPostgreSQL["saldo_final"].ToString().Trim();
                            double _ValorExtracontable = Double.Parse(TheDataReaderPostgreSQL["valor_extracontable"].ToString().Trim());
                            //Fila["valor_extracontable"] = String.Format(String.Format("{0:$ ###,###,##0}", _ValorExtracontable));
                            Fila["valor_extracontable"] = _ValorExtracontable;
                            //--
                            Fila["idcliente_establecimiento"] = TheDataReaderPostgreSQL["idcliente_establecimiento"].ToString().Trim();
                            Fila["codigo_oficina"] = TheDataReaderPostgreSQL["codigo_oficina"].ToString().Trim();
                            Fila["nombre_oficina"] = TheDataReaderPostgreSQL["nombre_oficina"].ToString().Trim();
                            //--
                            Fila["id_estado"] = Int32.Parse(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            Fila["fecha_modificacion"] = TheDataReaderPostgreSQL["fecha_modificacion"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_comercios]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptControlActividades(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtControlActividades";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_control_act", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region AQUI DEFINIMOS LAS COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idcontrol_actividad", typeof(Int32));
                    TablaDatos.Columns.Add("idtipo_ctrl_actividades", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_ctrl_actividad");
                    TablaDatos.Columns.Add("doble_cantidad");
                    TablaDatos.Columns.Add("idformulario_impuesto", typeof(Int32));
                    TablaDatos.Columns.Add("descripcion_formulario");
                    TablaDatos.Columns.Add("anio_gravable");
                    TablaDatos.Columns.Add("id_cliente");
                    TablaDatos.Columns.Add("nombre_cliente");
                    TablaDatos.Columns.Add("codigo_actividad");
                    TablaDatos.Columns.Add("descripcion_actividad");
                    TablaDatos.Columns.Add("porc_cumplimiento", typeof(Double));
                    TablaDatos.Columns.Add("del_sistema", typeof(bool));
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
                            Fila["idcontrol_actividad"] = Int32.Parse(TheDataReaderPostgreSQL["idcontrol_actividad"].ToString().Trim());
                            Fila["idtipo_ctrl_actividades"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_ctrl_actividades"].ToString().Trim());
                            Fila["tipo_ctrl_actividad"] = TheDataReaderPostgreSQL["tipo_ctrl_actividad"].ToString().Trim();
                            Fila["doble_cantidad"] = TheDataReaderPostgreSQL["doble_cantidad"].ToString().Trim();
                            Fila["idformulario_impuesto"] = Int32.Parse(TheDataReaderPostgreSQL["idformulario_impuesto"].ToString().Trim());
                            Fila["descripcion_formulario"] = TheDataReaderPostgreSQL["descripcion_formulario"].ToString().Trim();
                            Fila["anio_gravable"] = TheDataReaderPostgreSQL["anio_gravable"].ToString().Trim();
                            Fila["id_cliente"] = TheDataReaderPostgreSQL["id_cliente"].ToString().Trim();
                            Fila["nombre_cliente"] = TheDataReaderPostgreSQL["nombre_cliente"].ToString().Trim();
                            Fila["codigo_actividad"] = TheDataReaderPostgreSQL["codigo_actividad"].ToString().Trim();
                            Fila["descripcion_actividad"] = TheDataReaderPostgreSQL["descripcion_actividad"].ToString().Trim();
                            Fila["porc_cumplimiento"] = Double.Parse(TheDataReaderPostgreSQL["porc_cumplimiento"].ToString().Trim());
                            Fila["del_sistema"] = TheDataReaderPostgreSQL["del_sistema"].ToString().Trim().Equals("S") ? true : false;

                            Fila["id_estado"] = Int32.Parse(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_modificacion"] = TheDataReaderPostgreSQL["fecha_modificacion"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_control_act]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptCtrlActividadAnalista(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtControlActividades";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_ctrl_act_analista", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idform_impuesto", IdFormImpuesto);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_anio_gravable", AnioGravable);
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

                            Fila["id_estado"] = Int32.Parse(TheDataReaderPostgreSQL["id_estado"].ToString().Trim());
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_modificacion"] = TheDataReaderPostgreSQL["fecha_modificacion"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_control_act]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptBancoCuentas(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtBancoCuentas";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_bancocuentas", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("idmunicipio_banco", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_departamento");
                    TablaDatos.Columns.Add("id_municipio", typeof(Int32));
                    TablaDatos.Columns.Add("codigo_dane");
                    TablaDatos.Columns.Add("nombre_municipio");
                    TablaDatos.Columns.Add("id_banco", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_banco");
                    TablaDatos.Columns.Add("idtipo_cuenta", typeof(Int32));
                    TablaDatos.Columns.Add("tipo_cuenta");
                    TablaDatos.Columns.Add("idmedio_pago", typeof(Int32));
                    TablaDatos.Columns.Add("medio_pago");
                    TablaDatos.Columns.Add("numero_cuenta");
                    TablaDatos.Columns.Add("cheque");
                    TablaDatos.Columns.Add("version_registro");
                    TablaDatos.Columns.Add("codigo_estado");
                    TablaDatos.Columns.Add("fecha_registro");
                    TablaDatos.Columns.Add("fecha_modificacion");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["idmunicipio_banco"] = Int32.Parse(TheDataReaderPostgreSQL["idmunicipio_banco"].ToString().Trim());
                            Fila["nombre_departamento"] = TheDataReaderPostgreSQL["nombre_departamento"].ToString().Trim();
                            Fila["id_municipio"] = Int32.Parse(TheDataReaderPostgreSQL["id_municipio"].ToString().Trim());
                            Fila["codigo_dane"] = TheDataReaderPostgreSQL["codigo_dane"].ToString().Trim();
                            Fila["nombre_municipio"] = TheDataReaderPostgreSQL["nombre_municipio"].ToString().Trim();
                            Fila["id_banco"] = Int32.Parse(TheDataReaderPostgreSQL["id_banco"].ToString().Trim());
                            Fila["nombre_banco"] = TheDataReaderPostgreSQL["nombre_banco"].ToString().Trim();
                            Fila["idtipo_cuenta"] = Int32.Parse(TheDataReaderPostgreSQL["idtipo_cuenta"].ToString().Trim());
                            Fila["tipo_cuenta"] = TheDataReaderPostgreSQL["tipo_cuenta"].ToString().Trim();
                            Fila["idmedio_pago"] = Int32.Parse(TheDataReaderPostgreSQL["idmedio_pago"].ToString().Trim());
                            Fila["medio_pago"] = TheDataReaderPostgreSQL["medio_pago"].ToString().Trim();

                            Fila["numero_cuenta"] = TheDataReaderPostgreSQL["numero_cuenta"].ToString().Trim();
                            Fila["cheque"] = TheDataReaderPostgreSQL["cheque"].ToString().Trim();
                            Fila["version_registro"] = TheDataReaderPostgreSQL["version_registro"].ToString().Trim();
                            Fila["codigo_estado"] = TheDataReaderPostgreSQL["codigo_estado"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            Fila["fecha_modificacion"] = TheDataReaderPostgreSQL["fecha_modificacion"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_comercios]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptUsuarios(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtUsuarios";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_usuarios", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINICION DE COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("id_usuario", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_usuario");
                    TablaDatos.Columns.Add("apellido_usuario");
                    TablaDatos.Columns.Add("identificacion_usuario");
                    TablaDatos.Columns.Add("login_usuario");
                    TablaDatos.Columns.Add("direccion_usuario");
                    TablaDatos.Columns.Add("telefono_usuario");
                    TablaDatos.Columns.Add("email_usuario");
                    TablaDatos.Columns.Add("manejar_fuera_oficina");
                    TablaDatos.Columns.Add("ip_equipo_usuario");
                    TablaDatos.Columns.Add("nombre_rol");
                    TablaDatos.Columns.Add("estado_usuario");
                    TablaDatos.Columns.Add("usuario_registra");
                    TablaDatos.Columns.Add("usuario_edita");
                    TablaDatos.Columns.Add("fecha_exp_clave");
                    TablaDatos.Columns.Add("fecha_registro");
                    TablaDatos.Columns.Add("fecha_actualizacion");
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region AQUI OBTENEMOS LOS DATOS DEL DATAREADER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_usuario"] = Int32.Parse(TheDataReaderPostgreSQL["id_usuario"].ToString().Trim());
                            Fila["nombre_usuario"] = TheDataReaderPostgreSQL["nombre_usuario"].ToString().Trim();
                            Fila["apellido_usuario"] = TheDataReaderPostgreSQL["apellido_usuario"].ToString().Trim();
                            Fila["identificacion_usuario"] = TheDataReaderPostgreSQL["identificacion_usuario"].ToString().Trim();
                            Fila["login_usuario"] = TheDataReaderPostgreSQL["login_usuario"].ToString().Trim();
                            Fila["direccion_usuario"] = TheDataReaderPostgreSQL["direccion_usuario"].ToString().Trim();
                            Fila["telefono_usuario"] = TheDataReaderPostgreSQL["telefono_usuario"].ToString().Trim();
                            Fila["email_usuario"] = TheDataReaderPostgreSQL["email_usuario"].ToString().Trim();
                            Fila["manejar_fuera_oficina"] = TheDataReaderPostgreSQL["manejar_fuera_oficina"].ToString().Trim();
                            Fila["ip_equipo_usuario"] = TheDataReaderPostgreSQL["ip_equipo_oficina"].ToString().Trim();
                            Fila["nombre_rol"] = TheDataReaderPostgreSQL["nombre_rol"].ToString().Trim();
                            Fila["estado_usuario"] = TheDataReaderPostgreSQL["estado_usuario"].ToString().Trim();
                            Fila["usuario_registra"] = TheDataReaderPostgreSQL["usuario_registra"].ToString().Trim();
                            Fila["usuario_edita"] = TheDataReaderPostgreSQL["usuario_edita"].ToString().Trim();
                            Fila["fecha_exp_clave"] = TheDataReaderPostgreSQL["fecha_exp_clave"].ToString().Trim();
                            Fila["fecha_registro"] = TheDataReaderPostgreSQL["fecha_registro"].ToString().Trim();
                            Fila["fecha_actualizacion"] = TheDataReaderPostgreSQL["fecha_actualizacion"].ToString().Trim();
                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_comercios]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

        public DataTable GetRptRolPermisos(ref string _MsgError)
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtRolPermisos";
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
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_reporte_rol_permisos", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idrol", IdRol);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idestado", IdEstado);
                    TheDataReaderPostgreSQL = TheCommandPostgreSQL.ExecuteReader();

                    #region DEFINIR COLUMNAS DEL DATATABLE
                    TablaDatos.Columns.Add("id_perfil", typeof(Int32));
                    TablaDatos.Columns.Add("nombre_perfil");
                    TablaDatos.Columns.Add("id_navegacion", typeof(Int32));
                    TablaDatos.Columns.Add("titulo_opcion");
                    TablaDatos.Columns.Add("descripcion_opcion");
                    TablaDatos.Columns.Add("puede_leer", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_registrar", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_modificar", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_eliminar", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_bloquear", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_anular", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_exportar", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_liq_borrador", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_liq_definitivo", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_ver_formulario", typeof(Boolean));
                    TablaDatos.Columns.Add("puede_configurar", typeof(Boolean));
                    #endregion

                    if (TheDataReaderPostgreSQL != null)
                    {
                        while (TheDataReaderPostgreSQL.Read())
                        {
                            #region OBTENER DATOS DEL DATA READER
                            DataRow Fila = null;
                            Fila = TablaDatos.NewRow();
                            Fila["id_perfil"] = Int32.Parse(TheDataReaderPostgreSQL["id_rol"].ToString().Trim());
                            Fila["nombre_perfil"] = TheDataReaderPostgreSQL["nombre_perfil"].ToString().Trim();
                            Fila["id_navegacion"] = Int32.Parse(TheDataReaderPostgreSQL["id_navegacion"].ToString().Trim());
                            Fila["titulo_opcion"] = TheDataReaderPostgreSQL["titulo_opcion"].ToString().Trim();
                            Fila["descripcion_opcion"] = TheDataReaderPostgreSQL["descripcion_opcion"].ToString().Trim();

                            if (TheDataReaderPostgreSQL["puede_leer"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_leer"] = true;
                            }
                            else
                            {
                                Fila["puede_leer"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_registrar"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_registrar"] = true;
                            }
                            else
                            {
                                Fila["puede_registrar"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_modificar"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_modificar"] = true;
                            }
                            else
                            {
                                Fila["puede_modificar"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_eliminar"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_eliminar"] = true;
                            }
                            else
                            {
                                Fila["puede_eliminar"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_bloquear"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_bloquear"] = true;
                            }
                            else
                            {
                                Fila["puede_bloquear"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_anular"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_anular"] = true;
                            }
                            else
                            {
                                Fila["puede_anular"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_exportar"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_exportar"] = true;
                            }
                            else
                            {
                                Fila["puede_exportar"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_liq_borrador"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_liq_borrador"] = true;
                            }
                            else
                            {
                                Fila["puede_liq_borrador"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_liq_definitivo"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_liq_definitivo"] = true;
                            }
                            else
                            {
                                Fila["puede_liq_definitivo"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_ver_formulario"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_ver_formulario"] = true;
                            }
                            else
                            {
                                Fila["puede_ver_formulario"] = false;
                            }

                            if (TheDataReaderPostgreSQL["puede_configurar"].ToString().Trim().Equals("1"))
                            {
                                Fila["puede_configurar"] = true;
                            }
                            else
                            {
                                Fila["puede_configurar"] = false;
                            }

                            TablaDatos.Rows.Add(Fila);
                            #endregion
                        }
                    }
                    _MsgError = "";
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    //Para Base de Datos MySQL
                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Para Base de Datos SQL Server
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
                TablaDatos = null;
                _MsgError = "Error con el sp [sp_web_reporte_comercios]. Motivo: " + ex.Message;
                _log.Error(_MsgError);
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

    }
}