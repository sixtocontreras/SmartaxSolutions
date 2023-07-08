using Devart.Data.PostgreSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Smartax.Web.Application.Clases.Modulos
{
    public class AdminCargueLeasingHabitacional
    {
        public List<string> GetCodigosDane()
        {
            List<string> codigos = new List<string>();
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);

            using (var command = new PgSqlCommand($"select trim(codigo_dane) from public.tbl_municipio order by codigo_dane;", _cnn))
            {
                _cnn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var codigo = reader.GetString(0);
                    codigos.Add(codigo);
                }

                _cnn.Close();
                return codigos;
            }
        }

        public int CreateTableIfNotExists(int anio)
        {
            var tableName = $"public.tbl_leasing_habitacional_{anio}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            using (var createCommand = new PgSqlCommand($@"
                CREATE table if not exists {tableName} (
	                id int4 NOT NULL PRIMARY KEY,
	                ica_consecutivo varchar(15) NULL,
	                fecha_registro date NULL,
	                email_address varchar(70) NULL,
	                area_a_la_que_corresponde varchar(30) NULL,
	                nombre_del_vendedor_o_persona_que_entrego_el_bien varchar(70) NULL,
	                nit varchar(15) NULL,
	                digito_de_verificacion varchar(1) NULL,
	                numero_de_la_obligacion varchar(30) NULL,
	                sucursal_de_radicacion_de_credito varchar(30) NULL,
	                direccion_del_domicilio_principal_del_vendedor_o_persona_que_en varchar(70) NULL,
	                ciudad_del_domicilio_principal_de_vendedor_o_persona_que_entreg varchar(70) NULL,
	                numero_telefonico varchar(20) NULL,
	                correo_electronico varchar(70) NULL,
	                fecha_de_desembolso_o_escrituracion date NULL,
	                mes_de_la_retencion varchar(20) NULL,
	                anio_de_la_escrituracion varchar(4) NULL,
	                nit_del_municipio_donde_se_encuentra_ubicado_el_inmueble varchar(15) NULL,
	                calidad_tributaria varchar(30) NULL,
	                es_un_activo_fijo varchar(2) NULL,
	                codigo_dane_del_municipio_del_domicilio_principal_del_vendedor varchar(5) NULL,
	                valor_de_la_compra_o_dacion numeric(30, 2) NULL,
	                tarifa_de_retencion numeric(10, 2) NULL,
	                digite_el_valor_de_rentencion_de_ica numeric(20, 2) NULL,
	                ciudad_de_ubicacion_del_inmueble varchar(40) NULL,
	                mes_del_desembolso varchar(20) NULL,
	                url_archivo_generado varchar(120) NULL,
	                esta_retencion_corresponde_a_una_operacion_del_anio_actual varchar(120) NULL,
	                url_archivo_cargado varchar(120) NULL,
                    id_usuario_add int4 NOT NULL,
                    id_usuario_up int4 DEFAULT 1, 
                    fecha_registro_aud timestamp without time zone NOT NULL DEFAULT NOW(),
                    fecha_actualizacion timestamp without time zone DEFAULT NOW(),
                    id_estado int4 DEFAULT 1
                );", _cnn))
            {
                _cnn.Open();

                createCommand.ExecuteNonQuery();
            }

            using (var command = new PgSqlCommand($"select coalesce(max(id), 0) + 1 from {tableName};", _cnn))
            {
                var reader = command.ExecuteReader();
                reader.Read();
                int id = reader.GetInt32(0);
                _cnn.Close();
                return id;
            }
        }

        public void DeleteRows(int anio, int mes)
        {
            var tableName = $"public.tbl_leasing_habitacional_{anio}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            using (var createCommand = new PgSqlCommand($@"DELETE FROM {tableName} WHERE DATE_PART('year', fecha_registro) = {anio} AND DATE_PART('month', fecha_registro) = {mes}", _cnn))
            {
                _cnn.Open();
                createCommand.ExecuteNonQuery();
                _cnn.Close();
            }
        }

        public void InsertData(int anio, int startId, string idUsuario, List<LeasingHabitacional> leasingsHabitacionales)
        {
            var insertTemplate = "({0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', {7}, {8}, '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', {16}, '{17}', '{18}', '{19}', '{20}', {21}, {22}, {23}, '{24}', '{25}', '{26}', '{27}', '{28}', {29})";
            
            var lastPagaduria = leasingsHabitacionales[leasingsHabitacionales.Count - 1];
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"INSERT INTO public.tbl_leasing_habitacional_{anio} VALUES ");
            stringBuilder.Append(Environment.NewLine);

            for (int i = 0; i < leasingsHabitacionales.Count; i++)
            {
                var leasingHabitacional = leasingsHabitacionales[i];
                stringBuilder.AppendFormat(insertTemplate, startId, leasingHabitacional.IcaConsecutivo, $"{leasingHabitacional.FechaRegistro:yyyy-MM-dd H:m:s}",
                    leasingHabitacional.Email, leasingHabitacional.Area, leasingHabitacional.NombreVendedor, leasingHabitacional.Nit,
                    leasingHabitacional.DigitoVerificacion, $"'{leasingHabitacional.NumeroObligacion}'", leasingHabitacional.SucursalRadicacion,
                    leasingHabitacional.DireccionDomicilio, leasingHabitacional.CiudadDomicilio, leasingHabitacional.NumeroTelefonico,
                    leasingHabitacional.Correo, $"{leasingHabitacional.FechaDesembolso:yyyy-MM-dd}", leasingHabitacional.MesRetencion,
                    leasingHabitacional.AnioEscrituracion, leasingHabitacional.NitMunicipio, leasingHabitacional.CalidadTributaria,
                    leasingHabitacional.ActivoFijo, leasingHabitacional.CodigoDaneMunicipio, leasingHabitacional.ValorCompra.ToString(CultureInfo.InvariantCulture),
                    leasingHabitacional.TarifaRetencion.ToString(CultureInfo.InvariantCulture), leasingHabitacional.ValorRetencion.ToString(CultureInfo.InvariantCulture), leasingHabitacional.CiudadUbicacionInmueble,
                    leasingHabitacional.MesDesembolso, leasingHabitacional.UrlArchivoGenerado, leasingHabitacional.RetencionCorrespondiente,
                    leasingHabitacional.UrlArchivoCargado, idUsuario);

                if (leasingHabitacional == lastPagaduria)
                {
                    continue;
                }

                stringBuilder.Append(",");
                stringBuilder.Append(Environment.NewLine);
                startId += 1;
            }

            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            _cnn.Open();
            using (var transaction = _cnn.BeginTransaction())
            {
                try
                {
                    byte[] bytes = Encoding.Default.GetBytes($"{stringBuilder}");
                    var utfCommand = Encoding.UTF8.GetString(bytes);
                    var command = new PgSqlCommand(utfCommand, _cnn, transaction);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }
        }
    }

    public class LeasingHabitacional
    {
        public string IcaConsecutivo { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string Email { get; set; }

        public string Area { get; set; }

        public string NombreVendedor { get; set; }

        public string Nit { get; set; }

        public char DigitoVerificacion { get; set; }

        public string NumeroObligacion { get; set; }

        public string SucursalRadicacion { get; set; }

        public string DireccionDomicilio { get; set; }

        public string CiudadDomicilio { get; set; }

        public string NumeroTelefonico { get; set; }

        public string Correo { get; set; }

        public DateTime FechaDesembolso { get; set; }

        public string MesRetencion { get; set; }

        public string AnioEscrituracion { get; set; }

        public string NitMunicipio { get; set; }

        public string CalidadTributaria { get; set; }

        public string ActivoFijo { get; set; }

        public string CodigoDaneMunicipio { get; set; }

        public float ValorCompra { get; set; }

        public float TarifaRetencion { get; set; }

        public float ValorRetencion { get; set; }

        public string CiudadUbicacionInmueble { get; set; }

        public string MesDesembolso { get; set; }

        public string UrlArchivoGenerado { get; set; }

        public string RetencionCorrespondiente { get; set; }

        public string UrlArchivoCargado { get; set; }
    }
}