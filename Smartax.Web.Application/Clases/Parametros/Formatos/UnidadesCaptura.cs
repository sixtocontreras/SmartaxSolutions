using Devart.Data.PostgreSql;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using System.Configuration;
using System.Data;

namespace Smartax.Web.Application.Clases.Parametros.Formatos
{
    public class UnidadesCaptura
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        
        internal DataTable GetAll()
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter("select a.*,b.razon_social as nombre_cliente,c.codigo_estado, d.nombre_departamento  from tbl_unidad_captura_f321 a join tbl_cliente b on a.id_cliente = b.id_cliente join tbl_estado c on a.id_estado =c.id_estado join tbl_departamento d on a.id_dpto =d.id_dpto", _cnn);
            var dt = new DataTable();
            dt.TableName = "infoEntidad";
            _da.Fill(dt);
            dt.Columns[0].ColumnName = "id_unidad_captura_f321";
            dt.Columns[1].ColumnName = "id_cliente";
            dt.Columns[2].ColumnName = "unidad_captura";
            dt.Columns[3].ColumnName = "id_dpto";
            dt.Columns[4].ColumnName = "id_estado";
            dt.Columns[5].ColumnName = "id_usuario_add";
            dt.Columns[6].ColumnName = "id_usuario_up";
            dt.Columns[7].ColumnName = "fecha_registro";
            dt.Columns[8].ColumnName = "fecha_actualizacion";
            dt.Columns[9].ColumnName = "nombre_cliente";
            dt.Columns[10].ColumnName = "codigo_estado";
            dt.Columns[11].ColumnName = "nombre_departamento";
            return dt;
           
        }

        internal bool update(DataRow Fila,int user, ref int _IdRegistro, ref string _MsgError)
        {
            var dt = new DataTable();
            var query = $"select id_cliente,unidad_captura  from tbl_unidad_captura_f321 where id_unidad_captura_f321 = {_IdRegistro}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var adp = new PgSqlDataAdapter(query, _cnn);
            adp.Fill(dt);
            _cnn.Open();
            if (dt.Rows[0][0].ToString() != Fila["id_cliente"].ToString() || dt.Rows[0][1].ToString() != Fila["unidad_captura"].ToString())
            {
                query = $"Select count(1) from tbl_unidad_captura_f321 where id_cliente = {Fila["id_cliente"].ToString()} and unidad_captura = '{Fila["unidad_captura"].ToString()}'";
                var com = new PgSqlCommand(query, _cnn);
                if (com.ExecuteScalar().ToString() != "0")
                {
                    _MsgError = "Ya existe un registro creado para el cliente y la unidad de captura seleccionadas.";
                    _cnn.Close();
                    _cnn.Dispose();
                    return false;
                }
            }

            query = "UPDATE tbl_unidad_captura_f321 SET ";
            query += $"unidad_captura = '{Fila["unidad_captura"]}',";
            query += $"id_dpto = '{Fila["id_dpto"]}',";
            query += $"id_estado = {Fila["id_estado"]},";
            query += $"id_cliente = {Fila["id_cliente"]},";
            query += $"id_usuario_up = {user}, fecha_actualizacion = NOW() ";
            query += $" WHERE id_unidad_captura_f321 = {_IdRegistro}";

            var com2 = new PgSqlCommand(query, _cnn);
            var rta = com2.ExecuteNonQuery() > 0;
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }
        internal bool Add(DataRow Fila, int user, ref int _IdRegistro, ref string _MsgError)
        {
            var query = $"Select count(1) from tbl_unidad_captura_f321 where id_cliente = {Fila["id_cliente"].ToString()} and unidad_captura = '{Fila["unidad_captura"].ToString()}'";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var com = new PgSqlCommand(query, _cnn);
            _cnn.Open();
            var rta2 = com.ExecuteScalar().ToString();
            if (rta2 != "0")
            {
                _MsgError = "Ya existe un registro creado para el cliente y la unidad de captura seleccionadas.";
                _cnn.Close();
                _cnn.Dispose();
                return false;
            }
            query = $"INSERT INTO tbl_unidad_captura_f321 (id_cliente, unidad_captura, id_dpto, id_estado, id_usuario_add, id_usuario_up, fecha_registro, fecha_actualizacion) "+
                    $" VALUES ('{Fila["id_cliente"]}','{Fila["unidad_captura"]}','{Fila["id_dpto"]}','{Fila["id_estado"]}',{user},NULL,NOW(),NULL)";

            com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            query = "SELECT MAX(id_unidad_captura_f321) FROM tbl_unidad_captura_f321 ";
            com = new PgSqlCommand(query, _cnn);
            _IdRegistro = int.Parse(com.ExecuteScalar().ToString());
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }
        internal bool delete(int user, ref int _IdRegistro, ref string _MsgError)
        {
            var query = $"DELETE FROM tbl_unidad_captura_f321 WHERE id_unidad_captura_f321 = {_IdRegistro}";
         
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