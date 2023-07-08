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
    public class AdminCargueTarjetaCredito
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
            var tableName = $"public.tbl_tarjeta_credito_{anio}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            using (var createCommand = new PgSqlCommand($@"
                CREATE TABLE IF NOT EXISTS {tableName} (
	                id int4 NOT NULL PRIMARY KEY,
	                tipo varchar(4) NULL,
	                impuesto varchar(9) NULL,
	                cod_ciu varchar(7) NULL,
	                ciudad varchar(14) NULL,
	                nit varchar(27) NULL,
	                tm int4 NULL,
	                marca varchar(19) NULL,
	                fecha_inicial date NULL,
	                fecha_final date NULL,
	                valor_venta numeric(15, 2) NULL,
	                establecimiento varchar(38) NULL,
	                valor_impuesto numeric(15, 2) NULL,
	                valor_base numeric(15, 2) NULL,
                    id_usuario_add int4 NOT NULL,
                    id_usuario_up int4 DEFAULT 1, 
                    fecha_registro timestamp without time zone NOT NULL DEFAULT NOW(),
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
            var tableName = $"public.tbl_tarjeta_credito_{anio}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            using (var createCommand = new PgSqlCommand($@"DELETE FROM {tableName} WHERE DATE_PART('year', fecha_final) = {anio} AND DATE_PART('month', fecha_final) = {mes}", _cnn))
            {
                _cnn.Open();
                createCommand.ExecuteNonQuery();
                _cnn.Close();
            }
        }

        public void InsertData(int anio, int startId, string idUsuario, List<TarjetaCredito> tarjetasCredito)
        {
            var insertTemplate = "({0}, {1}, '{2}', '{3}', '{4}', '{5}', {6}, '{7}', '{8}', '{9}', {10}, '{11}', {12}, {13}, {14})";

            var lastTarjetaCredito = tarjetasCredito[tarjetasCredito.Count - 1];
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"INSERT INTO public.tbl_tarjeta_credito_{anio} VALUES");
            stringBuilder.Append(Environment.NewLine);

            for (int i = 0; i < tarjetasCredito.Count; i++)
            {
                var tarjetaCredito = tarjetasCredito[i];
                stringBuilder.AppendFormat(insertTemplate, startId, tarjetaCredito.Tipo, tarjetaCredito.Impuesto, tarjetaCredito.CodCiu,
                    tarjetaCredito.Ciudad, tarjetaCredito.Nit, tarjetaCredito.Tm, tarjetaCredito.Marca, $"{tarjetaCredito.FechaInicial:yyyy-MM-dd}",
                    $"{tarjetaCredito.FechaFinal:yyyy-MM-dd}", tarjetaCredito.ValorVenta.ToString(CultureInfo.InvariantCulture), 
                    tarjetaCredito.Establecimiento.Replace('\'', '"'), tarjetaCredito.ValorImpuesto.ToString(CultureInfo.InvariantCulture),
                    tarjetaCredito.ValorBase.ToString(CultureInfo.InvariantCulture), idUsuario);

                if (tarjetaCredito == lastTarjetaCredito)
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

    public class TarjetaCredito
    {
        public int Id { get; set; }

        public string Tipo { get; set; }

        public string Impuesto { get; set; }

        public string CodCiu { get; set; }

        public string Ciudad { get; set; }

        public string Nit { get; set; }

        public int Tm { get; set; }

        public string Marca { get; set; }

        public DateTime FechaInicial { get; set; }

        public DateTime FechaFinal { get; set; }

        public float ValorVenta { get; set; }

        public string Establecimiento { get; set; }

        public float ValorImpuesto { get; set; }

        public float ValorBase { get; set; }
    }
}