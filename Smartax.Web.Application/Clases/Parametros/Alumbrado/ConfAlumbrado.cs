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
    public class ConfAlumbrado
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        internal DataTable GetAll()
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select a.id,c.razon_social nombre_cliente, a.id_cliente,upper(d.nombre_departamento) nombre_dpto, a.id_dpto, m.codigo_dane ,upper(m.nombre_municipio) nombre_municipio , a.id_municipio ,a.vigencia, s.nombre  nombre_sector, a.id_sector, a.fecha_registro " +
                $"from tbl_sector_municipio a " +
                $"join tbl_departamento d on a.id_dpto = d.id_dpto " +
                $"join tbl_municipio m on a.id_municipio = m.id_municipio " +
                $"join tbl_cliente c on a.id_cliente = c.id_cliente " +
                $"join tbl_sector s on a.id_sector = s.id order by d.nombre_departamento, m.nombre_municipio, a.vigencia; ", _cnn);
            var dt = new DataTable();
            dt.TableName = "DtSectorMunicipio";
            _da.Fill(dt);
            return dt;

        }
        internal DataTable GetDptos()
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select id_dpto, upper(nombre_departamento) nombre_departamento  from tbl_departamento td order by codigo_dane;", _cnn);
            var dt = new DataTable();
            dt.TableName = "DtDptos";
            _da.Fill(dt);
            var ss = dt.NewRow();
            ss[1] = "<< Seleccione >>";
            dt.Rows.InsertAt(ss, 0);
            return dt;

        }
        internal DataTable GetMunicipios(int id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select id_municipio,codigo_dane, upper(nombre_municipio) nombre_municipio  from tbl_municipio where id_dpto = {id} order by codigo_dane ;", _cnn);
            var dt = new DataTable();
            dt.TableName = "DtMunicipios";
            _da.Fill(dt);
            return dt;

        }


        internal List<municipios> GetMunicipios()
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select id_dpto, id_municipio,codigo_dane, upper(nombre_municipio) nombre_municipio  from tbl_municipio  order by codigo_dane ;", _cnn);
            var dt = new DataTable();
            dt.TableName = "DtMunicipios";
            _da.Fill(dt);
            List<municipios> muniList = new List<municipios>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                municipios muni = new municipios();
                muni.id = Convert.ToInt32(dt.Rows[i]["id_municipio"]);
                muni.dpto = Convert.ToInt32(dt.Rows[i]["id_dpto"]);
                muni.dane = Convert.ToInt32(dt.Rows[i]["codigo_dane"]);
                muni.nombre = dt.Rows[i]["nombre_municipio"].ToString();
                muniList.Add(muni);
            }
            return muniList;

        }
        internal DataTable GetSectores()
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select id, upper(nombre) nombre  from tbl_sector order by nombre;", _cnn);
            var dt = new DataTable();
            dt.TableName = "DtSectores";
            _da.Fill(dt);
            return dt;

        }

        internal DataTable GetTiposSectorEspecial()
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select idtipo_especial,descripcion_tipo_especial from tbl_tipos_especial order by idtipo_especial;", _cnn);
            var dt = new DataTable();
            dt.TableName = "DtTiposSectoresEspecial";
            _da.Fill(dt);
            return dt;
        }

        internal bool AddConfEntidad(string Fila, int user, ref int _IdRegistro, ref string _MsgError)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);

            var query = $"INSERT INTO public.tbl_sector_municipio " +
                $"(id_sector, id_municipio, vigencia, id_cliente, fecha_registro, usuario_registro, estado, id_dpto) " +
                $"VALUES({Fila.Split('|')[0]}, {Fila.Split('|')[1]},{Fila.Split('|')[2]}, {Fila.Split('|')[3]}, now(), {user}, 1, {Fila.Split('|')[4]}); ";
            _cnn.Open();
            var com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            query = "SELECT MAX(id) FROM tbl_sector_municipio";
            com = new PgSqlCommand(query, _cnn);
            _IdRegistro = int.Parse(com.ExecuteScalar().ToString());
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }
        internal bool UpConfEntidad(DataRow Fila, int user, ref int _IdRegistro, ref string _MsgError)
        {
            var dt = new DataTable();
            var query = $"select id_cliente, id_dpto, id_municipio, vigencia  from tbl_sector_municipio where id = {_IdRegistro}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var adp = new PgSqlDataAdapter(query, _cnn);
            adp.Fill(dt);
            if (dt.Rows[0][0].ToString() == Fila["id_cliente"].ToString() && dt.Rows[0][1].ToString() == Fila["id_dpto"].ToString() && dt.Rows[0][2].ToString() == Fila["id_municipio"].ToString() && dt.Rows[0][3].ToString() == Fila["vigencia"].ToString())
            {
                _MsgError = "Ya existe un registro creado para el cliente y la columna seleccionadas.";
                return false;
            }

            query = "UPDATE tbl_sector_municipio SET ";
            query += $"id_cliente = {Fila["id_cliente"]},";
            query += $"id_dpto = {Fila["id_dpto"].ToString().Trim()},";
            query += $"id_municipio = {Fila["id_municipio"]},";
            query += $"id_sector = {Fila["id_sector"].ToString().Trim()},";
            query += $"vigencia = {Fila["vigencia"].ToString().Trim()},";
            query += $"usuario_actualizacion = {user}, fecha_actualizacion = NOW() ";
            query += $" WHERE id = {_IdRegistro}";

            _cnn.Open();
            var com2 = new PgSqlCommand(query, _cnn);
            var rta = com2.ExecuteNonQuery() > 0;
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }
        internal bool dltConfEntidad(int user, ref int _IdRegistro, ref string _MsgError)
        {
            var query = $"DELETE FROM tbl_kwh_sector WHERE id_sector_municipio = {_IdRegistro}";

            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var com = new PgSqlCommand(query, _cnn);
            _cnn.Open();
            com.ExecuteNonQuery();
            query = $"DELETE FROM tbl_sector_municipio WHERE id = {_IdRegistro}";
            com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }


        internal DataTable GetTarifasAll(int id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select id,clasificacion, concat('de ',min_kw,' hasta ',max_kw) consumokwh,min_kw, max_kw, consumo, tarifa_minima ,tarifa_maxima,tipo " +
                $"from tbl_kwh_sector where id_sector_municipio = {id} ; ", _cnn);
            var dt = new DataTable();
            dt.TableName = "DtTarifas";
            _da.Fill(dt);
            return dt;

        }
        internal bool AddTarifa(DataRow Fila, int id, int user, ref int _IdRegistro, ref string _MsgError)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);

            var query = $"INSERT INTO public.tbl_kwh_sector " +
                $"(clasificacion, min_kw, max_kw, id_sector_municipio, consumo, tarifa_minima, tarifa_maxima, usuario_registro, fecha_registro) " +
                $"VALUES('{Fila["clasificacion"]}', {Fila["min_kw"]},{Fila["max_kw"]}, {id}, {Fila["consumo"].ToString().Replace(",", ".")}, {Fila["tarifa_minima"].ToString().Replace(",", ".")}, {Fila["tarifa_maxima"].ToString().Replace(",", ".")},{user},now()); ";
            _cnn.Open();
            var com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            query = "SELECT MAX(id) FROM tbl_kwh_sector";
            com = new PgSqlCommand(query, _cnn);
            _IdRegistro = int.Parse(com.ExecuteScalar().ToString());
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }
        internal bool AddTarifaEspecial(DataRow Fila, int id, int user, ref int _IdRegistro, ref string _MsgError)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);

            var query = $"INSERT INTO public.tbl_kwh_sector " +
                $"(clasificacion,id_sector_municipio, tarifa_maxima, usuario_registro, fecha_registro, tipo) " +
                $"VALUES('{Fila["clasificacion"]}', {id}, {Fila["tarifa_maxima"].ToString().Replace(",",".")},{user},now(),{Fila["tipo"]}); ";
            _cnn.Open();
            var com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            query = "SELECT MAX(id) FROM tbl_kwh_sector";
            com = new PgSqlCommand(query, _cnn);
            _IdRegistro = int.Parse(com.ExecuteScalar().ToString());
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }


        internal bool dltTarifa(int user, ref int _IdRegistro, ref string _MsgError)
        {
            var query = $"DELETE FROM tbl_kwh_sector WHERE id = {_IdRegistro}";

            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var com = new PgSqlCommand(query, _cnn);
            _cnn.Open();
            var rta = com.ExecuteNonQuery() > 0;
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }


    }
    public class municipios
    {
        public int id { get; set; }
        public int dpto { get; set; }
        public int dane { get; set; }
        public string nombre { get; set; }
    }
}