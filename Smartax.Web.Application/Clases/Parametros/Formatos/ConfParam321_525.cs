using Devart.Data.PostgreSql;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using System.Configuration;
using System.Data;

namespace Smartax.Web.Application.Clases.Parametros.Formatos
{
    public class ConfParam321_525
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        internal DataTable GetAll()
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter("select a.*,b.razon_social as nombre_cliente  from tbl_param_f321_f525 a join tbl_cliente b on a.id_cliente = b.id_cliente order by b.razon_social, a.columna_321", _cnn);
            var dt = new DataTable();
            dt.TableName = "Param321525";
            _da.Fill(dt);
            dt.Columns[0].ColumnName = "id_param_f321_f525";
            dt.Columns[1].ColumnName = "columna_321";
            dt.Columns[2].ColumnName = "etiqueta";
            dt.Columns[3].ColumnName = "base_resultante";
            dt.Columns[4].ColumnName = "subcuenta_f525";
            dt.Columns[5].ColumnName = "nombre_cuenta_f525";
            dt.Columns[6].ColumnName = "id_estado";
            dt.Columns[7].ColumnName = "id_usuario_add";
            dt.Columns[8].ColumnName = "id_usuario_up";
            dt.Columns[9].ColumnName = "fecha_registro";
            dt.Columns[10].ColumnName = "fecha_actualizacion";
            dt.Columns[11].ColumnName = "id_cliente";
            dt.Columns[12].ColumnName = "nombre_cliente";
            return dt;

        }

        internal bool UpConfEntidad(DataRow Fila, int user, ref int _IdRegistro, ref string _MsgError)
        {
            var dt = new DataTable();
            var query = $"select id_cliente,columna_321  from tbl_param_f321_f525 where id_param_f321_f525 = {_IdRegistro}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var adp = new PgSqlDataAdapter(query, _cnn);
            adp.Fill(dt);
            if (dt.Rows[0][0].ToString() != Fila["id_cliente"].ToString() || dt.Rows[0][1].ToString() != Fila["columna_321"].ToString())
            {
                query = $"Select count(1) from tbl_param_f321_f525 where id_cliente = {Fila["id_cliente"].ToString()} and columna_321 = '{Fila["columna_321"].ToString()}'";

                _cnn.Open();
                var com = new PgSqlCommand(query, _cnn);
                var rr = com.ExecuteScalar();
                _cnn.Close();
                _cnn.Dispose();
                if (rr.ToString() != "0")
                {
                    _MsgError = "Ya existe un registro creado para el cliente y la columna seleccionadas.";
                    return false;
                }
            }

            query = "UPDATE tbl_param_f321_f525 SET ";
            query += $"columna_321 = '{Fila["columna_321"]}',";
            query += $"etiqueta = '{Fila["etiqueta"].ToString().Trim()}',";
            query += $"base_resultante = '{Fila["base_resultante"]}',";
            query += $"subcuenta_f525 = '{Fila["subcuenta_f525"].ToString().Trim()}',";
            query += $"nombre_cuenta_f525 = '{Fila["nombre_cuenta_f525"].ToString().Trim()}',";
            query += $"id_cliente = {Fila["id_cliente"]},";
            query += $"id_usuario_up = {user}, fecha_actualizacion = NOW() ";
            query += $" WHERE id_param_f321_f525 = {_IdRegistro}";

            _cnn.Open();
            var com2 = new PgSqlCommand(query, _cnn);
            var rta = com2.ExecuteNonQuery() > 0;
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }
        internal bool AddConfEntidad(DataRow Fila, int user, ref int _IdRegistro, ref string _MsgError)
        {
            var query = $"Select count(1) from tbl_param_f321_f525 where id_cliente = {Fila["id_cliente"].ToString()} and columna_321 = '{Fila["columna_321"].ToString()}'";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var com = new PgSqlCommand(query, _cnn);
            _cnn.Open();
            var rta2 = com.ExecuteScalar().ToString();
            if (rta2 != "0")
            {
                _MsgError = "Ya existe un registro creado para el cliente y la columna seleccionadas.";
                _cnn.Close();
                _cnn.Dispose();
                return false;
            }
            query = $"Insert Into tbl_param_f321_f525(columna_321, etiqueta, base_resultante, subcuenta_f525, nombre_cuenta_f525, id_estado, id_usuario_add, id_usuario_up, fecha_registro, fecha_actualizacion, id_cliente)" +
                                                        $" VALUES ('{Fila["columna_321"]}','{Fila["etiqueta"]}','{Fila["base_resultante"]}','{Fila["subcuenta_f525"]}','{Fila["nombre_cuenta_f525"]}',1" +
                                                                    $",{user},NULL,NOW(),NULL,{Fila["id_cliente"]})";

            com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            query = "SELECT MAX(id_param_f321_f525) FROM tbl_param_f321_f525";
            com = new PgSqlCommand(query, _cnn);
            _IdRegistro = int.Parse(com.ExecuteScalar().ToString());
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }
        internal bool dltConfEntidad(int user, ref int _IdRegistro, ref string _MsgError)
        {
            var query = $"DELETE FROM tbl_param_f321_f525 WHERE id_param_f321_f525 = {_IdRegistro}";

            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var com = new PgSqlCommand(query, _cnn);
            _cnn.Open();
            var rta = com.ExecuteNonQuery() > 0;
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }

        internal DataTable GetCuentas(int id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter("select a.id_cuentas_columnas_f321,a.id_puc id_cuenta, b.codigo_cuenta cod_cuenta, b.nombre_cuenta " +
                "from tbl_cuentas_columnas_f321 a join tbl_plan_unico_cuenta b on a.id_puc = b.id_puc " +
                $"where id_param_f321_f525 = {id} order by b.nombre_cuenta", _cnn);
            var dt = new DataTable();
            dt.TableName = "DtCuentas";
            _da.Fill(dt);
            return dt;
        }

        internal DataTable GetCuentasList(int idcliente)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter("select id_puc id_cuenta, concat(codigo_cuenta,'-',nombre_cuenta) nombre_cuenta,codigo_cuenta cod_cuenta " +
                $"from tbl_plan_unico_cuenta where id_estado =1 and id_cliente ={idcliente} order by codigo_cuenta", _cnn);
            var dt = new DataTable();
            dt.TableName = "DatosCuenta";
            _da.Fill(dt);
            return dt;
        }

        internal bool Addcta(DataRow Fila,int idcol, int user, ref int _IdRegistro, ref string _MsgError)
        {
            var query = $"Select count(1) from tbl_cuentas_columnas_f321 where id_puc = {Fila["id_cuenta"].ToString()} and id_param_f321_f525 = '{idcol}'";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var com = new PgSqlCommand(query, _cnn);
            _cnn.Open();
            var rta2 = com.ExecuteScalar().ToString();
            if (rta2 != "0")
            {
                _MsgError = "Ya existe un registro creado para la columna y cuenta seleccionadas.";
                _cnn.Close();
                _cnn.Dispose();
                return false;
            }
            query = $"insert into tbl_cuentas_columnas_f321(id_param_f321_f525,id_puc,id_estado,id_usuario_add,id_usuario_up,fecha_registro,fecha_actualizacion) " +
                $"values({idcol},{Fila["id_cuenta"].ToString()},1,{user},NULL,NOW(),NULL)";

            com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            query = "SELECT MAX(id_cuentas_columnas_f321) FROM tbl_cuentas_columnas_f321";
            com = new PgSqlCommand(query, _cnn);
            _IdRegistro = int.Parse(com.ExecuteScalar().ToString());
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }

        internal bool dltCta(int user, ref int _IdRegistro, ref string _MsgError)
        {
            var query = $"DELETE FROM tbl_cuentas_columnas_f321 WHERE id_cuentas_columnas_f321 = {_IdRegistro}";

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