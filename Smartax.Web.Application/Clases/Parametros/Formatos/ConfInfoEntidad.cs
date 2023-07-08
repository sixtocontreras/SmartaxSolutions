using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Devart.Data.PostgreSql;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Configuration;

namespace Smartax.Web.Application.Clases.Parametros.Formatos
{
    public class ConfInfoEntidad
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        
        internal DataTable GetAllConfEntidad()
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter("select a.*,b.razon_social nombre_cliente, c.codigo_estado  codigo_estado from tbl_informacion_cliente_sfc a join tbl_cliente b on a.id_cliente =b.id_cliente join tbl_estado c on a.id_estado =c.id_estado", _cnn);
            var dt = new DataTable();
            dt.TableName = "infoEntidad";
            _da.Fill(dt);
            dt.Columns[0].ColumnName = "id_informacion_cliente_sfc";
            dt.Columns[1].ColumnName = "id_cliente";
            dt.Columns[2].ColumnName = "tipo_entidad";
            dt.Columns[3].ColumnName = "codigo_entidad";
            dt.Columns[4].ColumnName = "nombre_entidad";
            dt.Columns[5].ColumnName = "palabra_clave";
            dt.Columns[6].ColumnName = "area_informacion";
            dt.Columns[7].ColumnName = "tipo_informe";
            dt.Columns[8].ColumnName = "codigo_oficina";
            dt.Columns[9].ColumnName = "tipo_moneda";
            dt.Columns[10].ColumnName = "tipo_informacion";
            dt.Columns[11].ColumnName = "tipo_fideicomiso_fondo";
            dt.Columns[12].ColumnName = "codigo_fideicomiso_fondo";
            dt.Columns[13].ColumnName = "numero_columnas_f321";
            dt.Columns[14].ColumnName = "unidad_captura_tn_f321";
            dt.Columns[15].ColumnName = "unidad_captura_f525";
            dt.Columns[16].ColumnName = "id_estado";
            dt.Columns[17].ColumnName = "id_usuario_add";
            dt.Columns[18].ColumnName = "id_usuario_up";
            dt.Columns[19].ColumnName = "fecha_registro";
            dt.Columns[20].ColumnName = "fecha_actualizacion";
            dt.Columns[21].ColumnName = "nombre_cliente";
            dt.Columns[22].ColumnName = "codigo_estado";
            return dt;
           
        }

        internal bool UpConfEntidad(DataRow Fila,int user, ref int _IdRegistro, ref string _MsgError)
        {
            var query = $"Select id_cliente from tbl_informacion_cliente_sfc where id_informacion_cliente_sfc = {_IdRegistro}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var com = new PgSqlCommand(query, _cnn);
            _cnn.Open();
            var cli = com.ExecuteScalar().ToString();
            if (cli != Fila["id_cliente"].ToString())
            {
                query = $"Select count(1) from tbl_informacion_cliente_sfc where id_cliente = {Fila["id_cliente"].ToString()}";
                com = new PgSqlCommand(query, _cnn);
                if (com.ExecuteScalar().ToString() != "0")
                {
                    _MsgError = "Ya existe un registro creado para el cliente seleccionado.";
                    _cnn.Close();
                    _cnn.Dispose();
                    return false;
                }
            }

            query = "UPDATE tbl_informacion_cliente_sfc SET ";
            query += $"tipo_entidad = '{Fila["tipo_entidad"]}',";
            query += $"codigo_entidad = '{Fila["codigo_entidad"]}',";
            query += $"nombre_entidad = '{Fila["nombre_entidad"]}',";
            query += $"palabra_clave = '{Fila["palabra_clave"]}',";
            query += $"area_informacion = '{Fila["area_informacion"]}',";
            query += $"tipo_informe = '{Fila["tipo_informe"]}',";
            query += $"codigo_oficina = '{Fila["codigo_oficina"]}',";
            query += $"tipo_moneda = '{Fila["tipo_moneda"]}',";
            query += $"tipo_informacion = '{Fila["tipo_informacion"]}',";
            query += $"tipo_fideicomiso_fondo = '{Fila["tipo_fideicomiso_fondo"]}',";
            query += $"codigo_fideicomiso_fondo = '{Fila["codigo_fideicomiso_fondo"]}',";
            query += $"numero_columnas_f321 = '{Fila["numero_columnas_f321"]}',";
            query += $"unidad_captura_tn_f321 = '{Fila["unidad_captura_tn_f321"]}',";
            query += $"unidad_captura_f525 = '{Fila["unidad_captura_f525"]}',";
            query += $"id_estado = {Fila["id_estado"]},";
            query += $"id_cliente = {Fila["id_cliente"]},";
            query += $"id_usuario_up = {user}, fecha_actualizacion = NOW() ";
            query += $" WHERE id_informacion_cliente_sfc = {_IdRegistro}";

            com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }
        internal bool AddConfEntidad(DataRow Fila, int user, ref int _IdRegistro, ref string _MsgError)
        {
            var query = $"Select count(1) from tbl_informacion_cliente_sfc where id_cliente = {Fila["id_cliente"].ToString()}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var com = new PgSqlCommand(query, _cnn);
            _cnn.Open();
            var rta2 = com.ExecuteScalar().ToString();
            if (rta2 != "0")
            {
                _MsgError = "Ya existe un registro creado para el cliente seleccionado.";
                _cnn.Close();
                _cnn.Dispose();
                return false;
            }
            query = $"Insert Into tbl_informacion_cliente_sfc(id_cliente, tipo_entidad, codigo_entidad, nombre_entidad, palabra_clave, area_informacion, tipo_informe, codigo_oficina, tipo_moneda, tipo_informacion, tipo_fideicomiso_fondo, codigo_fideicomiso_fondo, numero_columnas_f321, unidad_captura_tn_f321, unidad_captura_f525, id_estado, id_usuario_add, id_usuario_up, fecha_registro, fecha_actualizacion)" +
                                                        $" VALUES ('{Fila["id_cliente"]}','{Fila["tipo_entidad"]}','{Fila["codigo_entidad"]}','{Fila["nombre_entidad"]}','{Fila["palabra_clave"]}','{Fila["area_informacion"]}" +
                                                                    $"','{Fila["tipo_informe"]}','{Fila["codigo_oficina"]}','{Fila["tipo_moneda"]}','{Fila["tipo_informacion"]}','{Fila["tipo_fideicomiso_fondo"]}','{Fila["codigo_fideicomiso_fondo"]}" +
                                                                    $"','{Fila["numero_columnas_f321"]}','{Fila["unidad_captura_tn_f321"]}','{Fila["unidad_captura_f525"]}',{Fila["id_estado"]},{user},NULL,NOW(),NULL)";

            com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            query = "SELECT MAX(id_informacion_cliente_sfc) FROM tbl_informacion_cliente_sfc";
            com = new PgSqlCommand(query, _cnn);
            _IdRegistro = int.Parse(com.ExecuteScalar().ToString());
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }
        internal bool dltConfEntidad(int user, ref int _IdRegistro, ref string _MsgError)
        {
            var query = $"DELETE FROM tbl_informacion_cliente_sfc WHERE id_informacion_cliente_sfc = {_IdRegistro}";
         
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var com = new PgSqlCommand(query, _cnn);
            _cnn.Open();
            var rta = com.ExecuteNonQuery() > 0;
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }

    }
}