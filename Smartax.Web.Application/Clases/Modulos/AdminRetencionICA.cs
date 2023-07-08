using Devart.Data.PostgreSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Modulos
{
    public class AdminRetencionICA
    {
        internal DataTable GetCliente(int id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter("select i.idtipo_identificacion, c.numero_documento, c.digito_verificacion,c.razon_social,c.direccion_cliente, d.nombre_departamento,m.nombre_municipio,c.email_contacto,c.telefono_contacto,gran_contribuyente  " +
                "from tbl_cliente c join tbl_tipo_identificacion i on c.idtipo_identificacion = i.idtipo_identificacion join tbl_municipio m on c.idmun_ubicacion_principal = m.id_municipio " +
                $"join tbl_departamento d on m.id_dpto = d.id_dpto where c.id_cliente = {id}", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;

        }

        internal DataTable GetClienteContribuyente(int id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter("SELECT CL.id_cliente, CL.idtipo_clasificacion,TC.tipo_clasificacion  " +
                "FROM tbl_cliente CL,tbl_tipo_clasificacion TC " +
                "WHERE CL.idtipo_clasificacion = TC.idtipo_clasificacion " +
                $"and  id_cliente = {id}", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;

        }

        internal DataTable GetClienteTA(int id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter("SELECT CL.id_cliente, CL.idtipo_sector,TS.tipo_sector  " +
                "FROM tbl_cliente CL,tbl_tipo_sector TS " +
                "WHERE CL.idtipo_sector = TS.idtipo_sector " +
                $"AND   id_cliente = {id}", _cnn);
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

        internal DataTable GetUVT(string anio)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select valor_unidad  from tbl_valor_unid_medida where idunidad_medida ={ConfigurationManager.AppSettings["IdUnidadMedida"]} and anio_valor = {anio}", _cnn);
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
                $"where m.id_municipio = {idmunicipio} and m.idformulario_impuesto = {ConfigurationManager.AppSettings["IdReteICA"]} " +
                $"order by pp.idperiodicidad_impuesto", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;

        }

        internal DataTable GetData(string anio, string idmunicipio, string periodicidad, int cliente)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select * from tbl_impuesto_rete_ica where anio_declarable = {anio} and id_municipio = {idmunicipio} and id_periodo = {periodicidad} and id_cliente = {cliente};", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;
        }

        internal bool DeleteData(int id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            _cnn.Open();
            var query = $"DELETE FROM public.tbl_impuesto_rete_ica WHERE id = {id};";

            var com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;

            _cnn.Close();

            _cnn.Dispose();
            return rta;
        }

        internal bool InsertData(FormularioReteICA data, int user, ref int _IdRegistro)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            _cnn.Open();
            var query = $"INSERT INTO public.tbl_impuesto_rete_ica" +
                        $"(id_cliente, cod_dane, id_municipio, nombre_municipio, nombre_departamento, fecha_maxima, anio_declarable, " +
                        $" periodicidad, id_periodo, renglon15, renglon16, renglon17, renglon18, renglon19, renglon20, renglon21, renglon22, " +
                        $" renglon23, renglon24, renglon25, renglon26, renglon27, renglon28, renglon29, renglon30, renglon31, renglon32, " +
                        $"renglon33, renglon34, renglon35, renglon36, renglon37, renglon38, renglon39, renglon40, renglon41, renglon42, " +
                        $"id_firmante, id_contador, usuario_registro, nombre_razon_social, tipo_iden, numero_iden, digito_v, direccion_not, " + 
                        $"depto_not, ciu_not, correoe_not, tipo_cont, tipo_act, gran_contribuyente, id_estado, id_tipo, telefono_not, " +
                        $"nombre_firmante, nombre_contador, tipo_iden_firmante, tipo_iden_contador, numero_iden_firmante, numero_iden_contador, tarjeta_profesional) " +
                        $"VALUES({data.IdCliente}, '{data.CodigoDane}', {data.IdMunicipio}, '{data.NombreMunicipio}', " +
                        $"'{data.NombreDepartamento}', '{data.FechaMaxima:yyyy-MM-dd}', {data.AnioDeclarable}, '{data.Periodicidad}', " +
                        $"{data.IdPeriodo}, '{data.Renglon15}', '{data.Renglon16}', '{data.Renglon17}', '{data.Renglon18}', '{data.Renglon19}', " +
                        $"'{data.Renglon20}', '{data.Renglon21}', '{data.Renglon22}', '{data.Renglon23}', '{data.Renglon24}', '{data.Renglon25}', " +
                        $"'{data.Renglon26}', '{data.Renglon27}', '{data.Renglon28}','{data.Renglon29}','{data.Renglon30}','{data.Renglon31}'," +
                        $"'{data.Renglon32}','{data.Renglon33}','{data.Renglon34}','{data.Renglon35}','{data.Renglon36}','{data.Renglon37}'," +
                        $"'{data.Renglon38}','{data.Renglon39}','{data.Renglon40}','{data.Renglon41}','{data.Renglon42}',{data.IdFirmador}," +
                        $"{data.IdContador}, {user}, '{data.nombre_razon_social}', '{data.tipo_iden}', '{data.numero_iden}', '{data.digito_v}'," +
                        $"'{data.direccion_not}', '{data.depto_not}', '{data.ciu_not}', '{data.correoe_not}', '{data.tipo_cont}', '{data.tipo_act}', " +
                        $"'{data.gran_contribuyente}',2,1,'{data.telefono_not}','{data.nombre_firmante}', '{data.nombre_contador}', " +
                        $"'{data.tipo_iden_firmante}','{data.tipo_iden_contador}','{data.numero_iden_firmante}','{data.numero_iden_contador}','{data.tarjeta_profesional}') ;";

            var com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            query = "SELECT MAX(id) FROM tbl_impuesto_rete_ica";
            com = new PgSqlCommand(query, _cnn);
            _IdRegistro = int.Parse(com.ExecuteScalar().ToString());
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }

        internal DataTable GetUbicacion(string id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select m.nombre_municipio,d.nombre_departamento, m.id_municipio  from tbl_municipio m join tbl_departamento d on m.id_dpto =d.id_dpto where m.codigo_dane = '{id}'", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;

        }

        internal DataTable GetDatosCalculados(string idmunicipio, string vigencia, string cliente)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select c.clasificacion, c.tarifa_maxima  from tbl_sector_municipio sm  " +
                $"join tbl_kwh_sector c on c.id_sector_municipio = sm.id where sm.id_municipio = {idmunicipio} and sm.vigencia = {vigencia} and sm.id_cliente = {cliente}", _cnn);
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

        internal DataTable GetFilas15_20(int anio, int periodicidad, string codigoDane, out DataTable renglonCuentaConfig)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _daFilas15_20 = new PgSqlDataAdapter($"SELECT * FROM public.sp_filas15_20_ica({anio}, {periodicidad}, '{codigoDane}');", _cnn);
            var dtFilas15_20 = new DataTable();
            _daFilas15_20.Fill(dtFilas15_20);

            var _daRenglonCuenta = new PgSqlDataAdapter($"select tc.id, cri.renglon from tbl_concepto_renglon_ica cri inner join tbl_concepto tc on tc.cuenta = cri.cuenta where cri.renglon between 15 and 20; ", _cnn);
            var dtrenglonCuenta = new DataTable();
            _daRenglonCuenta.Fill(dtrenglonCuenta);
            renglonCuentaConfig = dtrenglonCuenta;
            return dtFilas15_20;
        }

        internal DataTable GetFilas26_31(int anio, int periodicidad, string codigoDane, out DataTable renglonCuentaConfig)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _daFilas26_31 = new PgSqlDataAdapter($"SELECT * FROM public.sp_filas26_31_ica({anio}, {periodicidad}, '{codigoDane}');", _cnn);
            var dtFilas26_31 = new DataTable();
            _daFilas26_31.Fill(dtFilas26_31);
            
            var _daRenglonCuenta = new PgSqlDataAdapter($"select * from tbl_concepto_renglon_ica where renglon between 26 and 31;", _cnn);
            var dtrenglonCuenta = new DataTable();
            _daRenglonCuenta.Fill(dtrenglonCuenta);
            renglonCuentaConfig = dtrenglonCuenta;
            return dtFilas26_31;
        }

        internal DataRow GetTipoTarifa(int municipioId, int configuracionId, int anio)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"SELECT tmi.valor_tarifa, tmi.idtipo_tarifa, ttt.descripcion_tarifa " +
                $"FROM tbl_municipio_impuesto tmi " +
                $"inner join tbl_tipo_tarifa ttt on (tmi.idtipo_tarifa = ttt.idtipo_tarifa) " +
                $"WHERE tmi.id_municipio = {municipioId} " +
                $"AND tmi.idform_configuracion = {configuracionId} AND tmi.anio_gravable = {anio}; ", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        internal DataRow GetOtrasConfiguraciones(int municipioId, int formimpuestoId, int anio)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"SELECT tmtm.idtipo_tarifa, tmtm.cantidad_medida, tmtm.cantidad_periodos, tvum.valor_unidad " +
                $"FROM tbl_municipio_tarifa_minima tmtm " +
                $"INNER JOIN tbl_valor_unid_medida tvum ON(tmtm.idvalor_unid_medida = tvum.idvalor_unid_medida) " +
                $"INNER JOIN tbl_valor_unid_medida tvum2 ON(tmtm.idunid_medida_bg = tvum2.idvalor_unid_medida) " +
                $"WHERE tmtm.id_municipio = {municipioId} " +
                $"AND tmtm.idformulario_impuesto = {formimpuestoId} AND tvum2.anio_valor = {anio}; ", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        internal bool UpdateData(FormularioReteICA data, int user)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            _cnn.Open();
            var query = $"UPDATE public.tbl_impuesto_rete_ica " +
                        $"SET id_cliente = {data.IdCliente}, cod_dane = '{data.CodigoDane}', id_municipio = {data.IdMunicipio}, " +
                        $"nombre_municipio = '{data.NombreMunicipio}', nombre_departamento = '{data.NombreDepartamento}', " +
                        $"fecha_maxima = '{data.FechaMaxima:yyyy-MM-dd}', anio_declarable = {data.AnioDeclarable}, periodicidad = '{data.Periodicidad}', " +
                        $"id_periodo = '{data.IdPeriodo}', renglon15 = '{data.Renglon15}', renglon16 = '{data.Renglon16}', renglon17 = '{data.Renglon17}', " +
                        $"renglon18 = '{data.Renglon18}', renglon19 = '{data.Renglon19}', renglon20 = '{data.Renglon20}', renglon21 = '{data.Renglon21}', " +
                        $"renglon22 = '{data.Renglon22}', renglon23 = '{data.Renglon23}', renglon24 = '{data.Renglon24}', renglon25 = '{data.Renglon25}', " +
                        $"renglon26 = '{data.Renglon26}', renglon27 = '{data.Renglon27}', renglon28 = '{data.Renglon28}', renglon29 = '{data.Renglon29}', " +
                        $"renglon30 = '{data.Renglon30}', renglon31 = '{data.Renglon31}', renglon32 = '{data.Renglon32}', renglon33 = '{data.Renglon33}', " +
                        $"renglon34 = '{data.Renglon34}', renglon35 = '{data.Renglon35}', renglon36 = '{data.Renglon36}', renglon37 = '{data.Renglon37}', " +
                        $"renglon38 = '{data.Renglon38}', renglon39 = '{data.Renglon39}', renglon40 = '{data.Renglon40}', renglon41 = '{data.Renglon41}', " +
                        $"renglon42 = '{data.Renglon42}', id_firmante = {data.IdFirmador}, id_contador = {data.IdContador}, usuario_registro = {user} " +
                        $"WHERE id = {data.Id};";

            var com = new PgSqlCommand(query, _cnn);
            var rta = com.ExecuteNonQuery() > 0;
            _cnn.Close();
            _cnn.Dispose();
            return rta;
        }

        internal DataTable GetDataPdf(int id)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select iri.cod_dane, iri.nombre_departamento, iri.nombre_municipio, " +
                $"iri.anio_declarable,iri.periodicidad, iri.renglon15, iri.renglon16, iri.renglon17, iri.renglon18, iri.renglon19," +
                $"iri.renglon20, iri.renglon21, iri.renglon22, iri.renglon23, iri.renglon24," +
                $"iri.renglon25, iri.renglon26, iri.renglon27, iri.renglon28, iri.renglon29," +
                $"iri.renglon30, iri.renglon31, iri.renglon32, iri.renglon33, iri.renglon34," +
                $"iri.renglon35, iri.renglon36, iri.renglon37, iri.renglon38, iri.renglon39," +
                $"iri.renglon40, iri.renglon41, iri.renglon42, to_char(iri.fecha_maxima,'dd-MM-YYYY') fecha_maxima, " +
                $"iri.nombre_razon_social, iri.tipo_iden, iri.numero_iden, iri.digito_v, iri.direccion_not, iri.depto_not, " +
                $"iri.ciu_not, iri.correoe_not, iri.tipo_cont, iri.tipo_act, iri.gran_contribuyente, iri.telefono_not, " +
                $"iri.nombre_firmante, iri.nombre_contador, iri.tipo_iden_firmante, iri.tipo_iden_contador, " +
                $"iri.numero_iden_firmante, iri.numero_iden_contador, iri.tarjeta_profesional " +
                $"from tbl_impuesto_rete_ica iri " +
                $"where iri.id = {id}; ", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;
        }
    }

    public class FormularioReteICA
    {
        public int Id { get; set; }

        public int IdCliente { get; set; }

        public string CodigoDane { get; set; }

        public int IdMunicipio { get; set; }

        public string NombreMunicipio { get; set; }

        public string NombreDepartamento { get; set; }

        public DateTime FechaMaxima { get; set; }

        public int AnioDeclarable { get; set; }

        public string Periodicidad { get; set; }

        public int IdPeriodo { get; set; }

        public string Renglon15 { get; set; }

        public string Renglon16 { get; set; }

        public string Renglon17 { get; set; }

        public string Renglon18 { get; set; }

        public string Renglon19 { get; set; }

        public string Renglon20 { get; set; }

        public string Renglon21 { get; set; }

        public string Renglon22 { get; set; }

        public string Renglon23 { get; set; }

        public string Renglon24 { get; set; }

        public string Renglon25 { get; set; }

        public string Renglon26 { get; set; }

        public string Renglon27 { get; set; }

        public string Renglon28 { get; set; }

        public string Renglon29 { get; set; }

        public string Renglon30 { get; set; }

        public string Renglon31 { get; set; }

        public string Renglon32 { get; set; }

        public string Renglon33 { get; set; }

        public string Renglon34 { get; set; }

        public string Renglon35 { get; set; }

        public string Renglon36 { get; set; }

        public string Renglon37 { get; set; }

        public string Renglon38 { get; set; }

        public string Renglon39 { get; set; }

        public string Renglon40 { get; set; }

        public string Renglon41 { get; set; }

        public string Renglon42 { get; set; }

        public int IdFirmador { get; set; }

        public int IdContador { get; set; }

        public string nombre_razon_social { get; set; }

        public string tipo_iden { get; set; }

        public string numero_iden { get; set; }

        public string digito_v { get; set; }

        public string direccion_not { get; set; }

        public string depto_not { get; set; }

        public string ciu_not { get; set; }

        public string correoe_not { get; set; }

        public string tipo_cont { get; set; }

        public string tipo_act { get; set; }

        public string gran_contribuyente { get; set; }

        public string telefono_not { get; set; }

        public string celular_not { get; set; }

        public string nombre_firmante { get; set; }

        public string nombre_contador { get; set; }

        public string tipo_iden_firmante { get; set; }

        public string tipo_iden_contador { get; set; }

        public string numero_iden_firmante { get; set; }

        public string numero_iden_contador { get; set; }

        public string tarjeta_profesional { get; set; }

    }
}