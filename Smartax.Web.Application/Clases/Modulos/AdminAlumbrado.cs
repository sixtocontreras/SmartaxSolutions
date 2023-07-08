using Devart.Data.PostgreSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Modulos
{
    public class AdminAlumbrado
    {
        internal DataTable GetCliente(int id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter("select i.idtipo_identificacion, c.numero_documento, c.digito_verificacion,c.razon_social,c.direccion_cliente, d.nombre_departamento,m.nombre_municipio,c.email_contacto,c.telefono_contacto  " +
                "from tbl_cliente c join tbl_tipo_identificacion i on c.idtipo_identificacion = i.idtipo_identificacion join tbl_municipio m on c.idmun_ubicacion_principal = m.id_municipio " +
                $"join tbl_departamento d on m.id_dpto = d.id_dpto where c.id_cliente = {id}", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;

        }
        internal DataTable GetUbicacion(string id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select m.nombre_municipio,d.nombre_departamento, m.id_municipio  from tbl_municipio m join tbl_departamento d on m.id_dpto =d.id_dpto where m.codigo_dane = '{id}'", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;

        }
        internal DataTable GetUVT(string anio)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select valor_unidad  from tbl_valor_unid_medida where idunidad_medida ={ConfigurationManager.AppSettings["IdUnidadMedida"]} and anio_valor = {anio}", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;

        }
        internal DataTable GetData(string anio, string idmunicipio, string periodicidad, int cliente)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select * from tbl_impuesto_alumbrado where vigencia ={anio} and id_municipio ={idmunicipio} and periodicidad = '{periodicidad}' and id_cliente = {cliente};", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;
        }
        internal DataTable GetData(string anio, string idmunicipio)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select p.id_periodicidad, p.descripcion_periodicidad, pp.idperiodicidad_impuesto, pp.periodicidad_impuesto, mct.fecha_limite,mct.valor_descuento  from tbl_municipio_impuesto m " +
                $"join tbl_periodicidad_pago p on m.id_periodicidad = p.id_periodicidad " +
                $"join tbl_periodicidad_impuesto pp on pp.idperiodicidad_pago = p.id_periodicidad " +
                $"join tbl_municipio_calendario_trib mct on mct.id_municipio = m.id_municipio and " +
                $"mct.idformulario_impuesto = m.idformulario_impuesto and mct.anio_gravable = {anio} " +
                $"and mct.idperiodicidad_impuesto = pp.idperiodicidad_impuesto " +
                $"where m.id_municipio = {idmunicipio} and m.idformulario_impuesto = {ConfigurationManager.AppSettings["IdImpuesto"]} " +
                $"and m.anio_gravable = {anio} " +
                $"order by pp.idperiodicidad_impuesto", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;

        }
        internal DataTable GetDatosCalculados(double uvt, string idperiodicidad, string idmunicipio, string vigencia, string cliente)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select a.*,tks.clasificacion renglon1,tks.consumo renglon6,tks.tarifa_minima renglon81,tks.tarifa_maxima renglon91," +
                $"round(((a.renglon4*tks.consumo)/100)/1000)*1000 renglon7,round((tks.tarifa_minima*{uvt})/1000)*1000 renglon82,round((tks.tarifa_maxima*{uvt})/1000)*1000 renglon92 " +
                $"from(" +
                $"select sm.id,s.id idsector, s.nombre, sum(c.kw_consumo) renglon2, avg(c.kw_hora) renglon3, " +
                $"round((sum(c.kw_consumo) * avg(c.kw_hora)) / 1000) * 1000 renglon4, sum(c.kw_consumo) / (mp.mes_fin - mp.mes_ini + 1) division " +
                $"from tbl_sector_municipio sm " +
                $"join tbl_sector s on sm.id_sector = s.id " +
                $"join tbl_meses_periodicidad_impuesto mp on mp.idperiodicidad_impuesto = {idperiodicidad} " +
                $"join tbl_consumos_alumbrado c on c.id_municipio = sm.id_municipio and c.id_cliente = sm.id_cliente and c.vigencia = sm.vigencia  and c.id_mes between mp.mes_ini and mp.mes_fin " +
                $"where sm.id_municipio = {idmunicipio} and sm.vigencia = {vigencia} and sm.id_cliente = {cliente} " +
                $"group by s.nombre, mp.mes_fin, mp.mes_ini, sm.id,s.id ) a " +
                $"join tbl_kwh_sector tks on a.division between tks.min_kw and tks.max_kw and a.id = tks.id_sector_municipio ", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;

        }

        internal DataTable GetDatosCalculados(string idmunicipio, string vigencia, string cliente)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select c.clasificacion, c.tarifa_maxima, c.tipo, vum.valor_unidad , c.tarifa_maxima * vum.valor_unidad  from tbl_sector_municipio sm  " +
                $"join tbl_kwh_sector c on c.id_sector_municipio = sm.id " +
                $"join tbl_valor_unid_medida vum on c.tipo = vum.idunidad_medida " +
                $"where sm.id_municipio = {idmunicipio} and sm.vigencia = {vigencia} and sm.id_cliente = {cliente} and vum.anio_valor = {vigencia}", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;

        }
        internal DataTable GetSector(string idmunicipio, string vigencia, string cliente)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select id_sector from tbl_sector_municipio s " +
                $"where s.id_municipio = {idmunicipio} and vigencia = {vigencia} and id_cliente = {cliente}", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;

        }


        internal DataTable GetFirmantes(bool contador, string cliente)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select id_firmante, concat(nombre_funcionario,apellido_funcionario) nombre,numero_documento,tarjeta_profesional,imagen_firma, id_rol, idtipo_identificacion  " +
                $"from tbl_firmante tf where id_rol in({ConfigurationManager.AppSettings[$"{(contador ? "contador" : "firmante")}"]})", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;
        }

        internal bool InsertData(AlumbradoDto data, int user, ref int _IdRegistro)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            _cnn.Open();
            var query = $"INSERT INTO public.tbl_impuesto_alumbrado " +
                $"(id_cliente, cod_dane, municipio, id_municipio, dpto, vigencia, uvt, fechamax, periodicidad, cliente_nombres, cliente_id_doc, cliente_numdoc, cliente_dv, " +
                $"cliente_direccion, cliente_municipio, cliente_dpto, cliente_mail, cliente_tel, cliente_cel, id_sector, renglon1, renglon2, renglon3, renglon4, renglon5, renglon6, " +
                $"renglon7, renglon8_1, renglon8_2, renglon9_1, renglon9_2, renglon10, renglon11, renglon12, renglon13, renglon14_1, renglon14_2, renglon15, id_firmante, id_contador, " +
                $"fecha_registro, usuario_registro, estado, porcentaje) " +
                $"VALUES({data.id_cliente}, '{data.cod_dane}', '{data.municipio}', {data.id_municipio}, '{data.dpto}', {data.vigencia}, {data.uvt}, cast('{data.fechaMax.ToString("yyyyMMdd")}' as date), '{data.periodicidad}', " +
                $"'{data.cliente_nombres}', {data.cliente_id_doc}, '{data.cliente_numdoc}', '{data.cliente_dv}', '{data.cliente_direccion}', '{data.cliente_municipio}', '{data.cliente_dpto}', " +
                $"'{data.cliente_mail}', '{data.cliente_tel}', '{data.cliente_cel}', {data.id_sector}, '{data.renglon1}', '{data.renglon2}', '{data.renglon3}', '{data.renglon4}', '{data.renglon5}', " +
                $"'{data.renglon6}', '{data.renglon7}', '{data.renglon8_1}', '{data.renglon8_2}', '{data.renglon9_1}', '{data.renglon9_2}', '{data.renglon10}', '{data.renglon11}', '{data.renglon12}', " +
                $"'{data.renglon13}', '{data.renglon14_1}', '{data.renglon14_2}', '{data.renglon15}', {data.id_firmante}, {data.id_contador}, now(), {user}, 1, {data.porcentaje});";

            var com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            query = "SELECT MAX(id) FROM tbl_impuesto_alumbrado";
            com = new PgSqlCommand(query, _cnn);
            _IdRegistro = int.Parse(com.ExecuteScalar().ToString());
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }

        internal bool DeleteData(int id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            _cnn.Open();
            var query = $"DELETE FROM public.tbl_impuesto_alumbrado WHERE id={id};";

            var com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
           
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }

        internal bool UpdateData(AlumbradoDto data, int user)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            _cnn.Open();
            var query = $"UPDATE public.tbl_impuesto_alumbrado set " +
                $"id_cliente= {data.id_cliente}, cod_dane = '{data.cod_dane}',  municipio='{data.municipio}', id_municipio={data.id_municipio}, dpto='{data.dpto}', vigencia={data.vigencia},uvt= {data.uvt}," +
                $" fechaMax=cast('{data.fechaMax.ToString("yyyyMMdd")}' as date), periodicidad='{data.periodicidad}', cliente_nombres='{data.cliente_nombres}', cliente_id_doc={data.cliente_id_doc}, cliente_numdoc='{data.cliente_numdoc}', " +
                $" cliente_dv='{data.cliente_dv}', cliente_direccion='{data.cliente_direccion}', cliente_municipio='{data.cliente_municipio}', cliente_dpto='{data.cliente_dpto}', " +
                $" cliente_mail='{data.cliente_mail}', cliente_tel='{data.cliente_tel}', cliente_cel='{data.cliente_cel}', id_sector={data.id_sector}, renglon1='{data.renglon1}', renglon2='{data.renglon2}', " +
                $" renglon3='{data.renglon3}', renglon4='{data.renglon4}', renglon5 ='{data.renglon5}', renglon6='{data.renglon6}', renglon7='{data.renglon7}', renglon8_1='{data.renglon8_1}', " +
                $" renglon8_2='{data.renglon8_2}', renglon9_1='{data.renglon9_1}', renglon9_2='{data.renglon9_2}', renglon10='{data.renglon10}', renglon11='{data.renglon11}', renglon12='{data.renglon12}', " +
                $"renglon13='{data.renglon13}', renglon14_1='{data.renglon14_1}', renglon14_2='{data.renglon14_2}', renglon15='{data.renglon15}', id_firmante={data.id_firmante}, id_contador={data.id_contador}," +
                $" fecha_actualizacion=now(), usuario_actualizacion={user}, estado=1, porcentaje={data.porcentaje} where id={data.id};";

            var com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }
        internal DataTable GetDataPdf(int id)
        {
        var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select pp.descripcion_periodicidad  ,p.periodicidad_impuesto,ic.tipo_identificacion, s.nombre, idf.tipo_identificacion, ff.numero_documento, " +
                $"concat(trim(ff.nombre_funcionario), ' ', trim(ff.apellido_funcionario)), ff.imagen_firma, idc.tipo_identificacion, fc.numero_documento, " +
                $"(case when fc.id_rol = 6 then 'Contador' else 'Revisor Fiscal' end), fc.tarjeta_profesional ,concat(trim(fc.nombre_funcionario), ' ', trim(fc.apellido_funcionario)), " +
                $"fc.imagen_firma, a.* from tbl_impuesto_alumbrado a " +
                $"join tbl_periodicidad_impuesto p on a.periodicidad = cast(p.idperiodicidad_impuesto as varchar(10)) " +
                $"join tbl_periodicidad_pago pp on p.idperiodicidad_pago = pp.id_periodicidad " +
                $"join tbl_tipo_identificacion ic on ic.idtipo_identificacion = a.cliente_id_doc " +
                $"join tbl_sector s on a.id_sector = s.id " +
                $"join tbl_firmante ff on ff.id_firmante = a.id_firmante " +
                $"join tbl_tipo_identificacion idf on idf.idtipo_identificacion = ff.idtipo_identificacion " +
                $"join tbl_firmante fc on fc.id_firmante = a.id_contador " +
                $"join tbl_tipo_identificacion idc on idc.idtipo_identificacion = fc.idtipo_identificacion " +
                $"where a.id = {id}; ", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;
        }
    }

    public class AlumbradoDto
    {
        public int id { get; set; }
        public int id_cliente { get; set; }
        public string cod_dane { get; set; }
        public string municipio { get; set; }
        public int id_municipio { get; set; }
        public string dpto { get; set; }
        public int vigencia { get; set; }
        public double uvt { get; set; }
        public DateTime fechaMax { get; set; }
        public string periodicidad { get; set; }
        public string cliente_nombres { get; set; }
        public int cliente_id_doc { get; set; }
        public string cliente_numdoc { get; set; }
        public string cliente_dv { get; set; }
        public string cliente_direccion { get; set; }
        public string cliente_municipio { get; set; }
        public string cliente_dpto { get; set; }
        public string cliente_mail { get; set; }
        public string cliente_tel { get; set; }
        public string cliente_cel { get; set; }
        public int id_sector { get; set; }
        public string renglon1 { get; set; }
        public string renglon2 { get; set; }
        public string renglon3 { get; set; }
        public string renglon4 { get; set; }
        public string renglon5 { get; set; }
        public string renglon6 { get; set; }
        public string renglon7 { get; set; }
        public string renglon8_1 { get; set; }
        public string renglon8_2 { get; set; }
        public string renglon9_1 { get; set; }
        public string renglon9_2 { get; set; }
        public string renglon10 { get; set; }
        public string renglon11 { get; set; }
        public string renglon12 { get; set; }
        public string renglon13 { get; set; }
        public string renglon14_1 { get; set; }
        public string renglon14_2 { get; set; }
        public string renglon15 { get; set; }
        public int id_firmante { get; set; }
        public int id_contador { get; set; }
        public int porcentaje { get; set; }
    }
}