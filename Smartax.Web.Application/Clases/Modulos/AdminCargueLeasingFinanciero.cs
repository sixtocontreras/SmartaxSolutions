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
    public class AdminCargueLeasingFinanciero
    {
        public List<string> GetCodigosDane(CodigoDane codigoDane)
        {
            List<string> codigos = new List<string>();
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var tbl = codigoDane == CodigoDane.Municipio ? "tbl_municipio" : "tbl_departamento";
            using (var command = new PgSqlCommand($"select trim(codigo_dane) from public.{tbl} order by codigo_dane;", _cnn))
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
            var tableName = $"public.tbl_leasing_financiero_{anio}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            using (var createCommand = new PgSqlCommand($@"
                CREATE TABLE IF NOT EXISTS {tableName} (
	                id int4 NOT NULL PRIMARY KEY,
	                tipo_ident varchar(5) NULL,
	                numero_ident varchar(15) NULL,
	                tipo_imp varchar(40) NULL,
	                subtipo_imp varchar(40) NULL,
	                tarifa numeric(6, 2) NULL,
	                valor numeric(20, 2) NULL,
	                nit_teso varchar(15) NULL,
	                nom_teso varchar(50) NULL,
	                num_cue varchar(15) NULL,
	                naturaleza varchar(1) NULL,
	                tipo_compro varchar(2) NULL,
	                numero_compro varchar(15) NULL,
	                nombre_tercero varchar(90) NULL,
	                base numeric(30, 2) NULL,
	                fecha_registro date NULL,
	                direccion_tercero varchar(70) NULL,
	                correo_electronico varchar(70) NULL,
	                departamento_tercero varchar(5) NULL,
	                ciudad_tercero varchar(5) NULL,
	                pais_tercero varchar(5) NULL,
	                telefono_tercero varchar(12) NULL,
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
            var tableName = $"public.tbl_leasing_financiero_{anio}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            using (var createCommand = new PgSqlCommand($@"DELETE FROM {tableName} WHERE DATE_PART('year', fecha_registro) = {anio} AND DATE_PART('month', fecha_registro) = {mes}", _cnn))
            {
                _cnn.Open();
                createCommand.ExecuteNonQuery();
                _cnn.Close();
            }
        }

        public void InsertData(int anio, int startId, string idUsuario, List<LeasingFinanciero> leasingFinancieros)
        {
            var insertTemplate = "({0}, '{1}', '{2}', '{3}', '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', '{11}', {12}, '{13}', {14}, '{15}', '{16}', '{17}', {18}, {19}, {20}, '{21}', {22})";
            
            var lastLeasingFinanciero = leasingFinancieros[leasingFinancieros.Count - 1];
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"INSERT INTO public.tbl_leasing_financiero_{anio} VALUES");
            stringBuilder.Append(Environment.NewLine);

            for (int i = 0; i < leasingFinancieros.Count; i++)
            {
                var leasingFinanciero = leasingFinancieros[i];
                stringBuilder.AppendFormat(insertTemplate, startId, leasingFinanciero.TipoIdent, leasingFinanciero.NumeroIdent,
                    leasingFinanciero.TipoImp, leasingFinanciero.SubtipoImp,  leasingFinanciero.Tarifa.ToString(CultureInfo.InvariantCulture), 
                    leasingFinanciero.Valor.ToString(CultureInfo.InvariantCulture), leasingFinanciero.NitTeso, leasingFinanciero.NomTeso, 
                    leasingFinanciero.NumCue, leasingFinanciero.Naturaleza, leasingFinanciero.TipoCompro, leasingFinanciero.NumeroCompro, 
                    leasingFinanciero.NombreTercero, leasingFinanciero.Base.ToString(CultureInfo.InvariantCulture), 
                    $"{leasingFinanciero.FechaRegistro:yyyy-MM-dd}", leasingFinanciero.DireccionTercero, leasingFinanciero.CorreoElectronico,
                    leasingFinanciero.DepartamentoTercero, leasingFinanciero.CiudadTercero, leasingFinanciero.PaisTercero, leasingFinanciero.TelefonoTercero, idUsuario);

                if (leasingFinanciero == lastLeasingFinanciero)
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

    public enum CodigoDane
    {
        Municipio,
        Departamento
    }

    public class LeasingFinanciero
    {
        public int Id { get; set; }

        public string TipoIdent { get; set; }

        public string NumeroIdent { get; set; }

        public string TipoImp { get; set; }

        public string SubtipoImp { get; set; }

        public float Tarifa { get; set; }

        public float Valor { get; set; }

        public string NitTeso { get; set; }

        public string NomTeso { get; set; }

        public string NumCue { get; set; }

        public char Naturaleza { get; set; }

        public string TipoCompro { get; set; }

        public string NumeroCompro { get; set; }

        public string NombreTercero { get; set; }

        public float Base { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string DireccionTercero { get; set; }

        public string CorreoElectronico { get; set; }

        public string DepartamentoTercero { get; set; }

        public string CiudadTercero { get; set; }

        public string PaisTercero { get; set; }

        public string TelefonoTercero { get; set; }
    }
}