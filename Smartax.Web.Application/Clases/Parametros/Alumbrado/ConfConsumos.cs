using Devart.Data.PostgreSql;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Parametros.Alumbrado
{
    public class ConfConsumos
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        internal DataTable GetAll()
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"SELECT id, a.id_dpto, d.nombre_departamento nombre_dpto, a.id_municipio, m.nombre_municipio , id_oficina, e.nombre_oficina , a.id_cliente, c.razon_social nombre_cliente, vigencia, id_mes, (case id_mes when 1 then 'ENERO'  when 2 then 'FEBRERO'  when 3 then 'MARZO'  when 4 then 'ABRIL' " +
                $" when 5 then 'MAYO' when 6 then 'JUNIO' when 7 then 'JULIO' when 8 then 'AGOSTO' when 9 then 'SEPTIEMBRE' when 10 then 'OCTUBRE' when 11 then 'NOVIEMBRE' when 12 then 'DICIEMBRE' end) nombre_mes, kw_consumo, kw_hora, a.fecha_registro, a.fecha_actualizacion, a.usuario_registro, a.usuario_actualizacion, estado " +
                $"FROM tbl_consumos_alumbrado a join tbl_departamento d on a.id_dpto =d.id_dpto join tbl_municipio m on a.id_municipio =m.id_municipio join tbl_cliente_establecimiento e on cast(a.id_oficina as int4) = cast(e.codigo_oficina as int4) join tbl_cliente c on a.id_cliente =c.id_cliente" 
                //$"order by d.nombre_departamento, m.nombre_municipio, a.vigencia; "
                , _cnn);
            var dt = new DataTable();
            dt.TableName = "DtConsumos";
            _da.Fill(dt);
            return dt;

        }
        internal DataTable GetRow(string id_cliente, string id_oficina, string vigencia, string mes)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"SELECT 1 FROM tbl_consumos_alumbrado where id_cliente={id_cliente} and id_oficina = '{id_oficina}' and vigencia = {vigencia} and id_mes = {mes};",_cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;

        }

        internal DataTable GetOficina(string id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select 1  from tbl_cliente_establecimiento where codigo_oficina = '{id}';", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;

        }

        internal DataTable GetOficinas(int id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select codigo_oficina id,id_municipio municipio, nombre_oficina Nombre  from tbl_cliente_establecimiento where id_municipio = {id} order by nombre_oficina ;", _cnn);
            var dt = new DataTable();
            dt.TableName = "DtOficinas";
            _da.Fill(dt);
            return dt;

        }

        internal List<Oficina> GetOficinas()
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select codigo_oficina id,id_municipio municipio, nombre_oficina Nombre  from tbl_cliente_establecimiento order by nombre_oficina", _cnn);
            var dt = new DataTable();
            dt.TableName = "DtOficinas";
            _da.Fill(dt);
            List<Oficina> ofiList = new List<Oficina>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Oficina ofi = new Oficina();
                ofi.id = Convert.ToInt32(dt.Rows[i]["id"]);
                ofi.municipio = Convert.ToInt32(dt.Rows[i]["municipio"]);
                ofi.nombre = dt.Rows[i]["nombre"].ToString();
                ofiList.Add(ofi);
            }
            return ofiList;

        }
        internal bool AddConfEntidad(string Fila, int user, ref int _IdRegistro, ref string _MsgError)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var data = Fila.Split('|');
            var query = $"INSERT INTO public.tbl_consumos_alumbrado " +
                $"(id_dpto, id_municipio, id_oficina, id_cliente, vigencia, id_mes, kw_consumo, kw_hora, fecha_registro, usuario_registro, estado) " +
                $"VALUES({data[0]}, {data[1]}, '{data[2]}', {data[3]}, {data[4]}, {data[5]}, {data[6].Replace(",",".")}, {data[7].Replace(",", ".")}, now(), {user}, 1);";
            _cnn.Open();
            var com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            query = "SELECT MAX(id) FROM tbl_consumos_alumbrado";
            com = new PgSqlCommand(query, _cnn);
            _IdRegistro = int.Parse(com.ExecuteScalar().ToString());
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }


        internal bool AddConfEntidad(string oficina, string cliente, string vigencia, string mes, string consumo, string kw, string user) {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var query = $"INSERT INTO public.tbl_consumos_alumbrado (id_dpto, id_municipio, id_oficina, id_cliente, vigencia, id_mes, kw_consumo, kw_hora, fecha_registro, usuario_registro, estado) " +
                $"select m.id_dpto,o.id_municipio,o.codigo_oficina,{cliente},{vigencia},{mes},{consumo.Replace(",",".")},{kw.Replace(",", ".")},now(),{user},1 from tbl_cliente_establecimiento o " +
                $"join tbl_municipio m on o.id_municipio = m.id_municipio " +
                $"where o.codigo_oficina = '{oficina}'";
            _cnn.Open();
            var com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        
        }

        internal bool UpConfEntidad(string Fila, int user, ref int _IdRegistro, ref string _MsgError)
        {
            var dt = new DataTable();
            var query = $"select id_cliente, id_dpto, id_municipio, vigencia, id_mes  from tbl_consumos_alumbrado where id <> {_IdRegistro}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var adp = new PgSqlDataAdapter(query, _cnn);
            adp.Fill(dt);
            
            var data = Fila.Split('|');
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == data[3] && dt.Rows[i][1].ToString() == data[0] && dt.Rows[i][2].ToString() == data[1] && dt.Rows[i][3].ToString() == data[4] && dt.Rows[i][4].ToString() == data[5])
                {
                    _MsgError = "Ya existe registro para el municipio – oficina - vigencia - mes.";
                    return false;
                }
            }
            
            query = $"UPDATE public.tbl_consumos_alumbrado SET " +
                $"id_dpto = {data[0]}, id_municipio = {data[1]}, id_oficina = '{data[2]}', id_cliente = {data[3]}, vigencia = {data[4]}, " +
                $"id_mes = {data[5]}, kw_consumo = {data[6]}, kw_hora = {data[7]}, fecha_actualizacion = now(),  usuario_actualizacion = {user} " +
                $"WHERE id = {_IdRegistro}";
            _cnn.Open();
            var com2 = new PgSqlCommand(query, _cnn);
            var rta = com2.ExecuteNonQuery() > 0;
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }
        internal bool dltConfEntidad(int user, ref int _IdRegistro, ref string _MsgError)
        {
            var query = $"DELETE FROM tbl_consumos_alumbrado WHERE id = {_IdRegistro}";

            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var com = new PgSqlCommand(query, _cnn);
            _cnn.Open();
            var rta = com.ExecuteNonQuery() > 0;
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }
    }
    public class Oficina
    {
        public int id { get; set; }
        public int municipio { get; set; }
        public string nombre { get; set; }
    }
}