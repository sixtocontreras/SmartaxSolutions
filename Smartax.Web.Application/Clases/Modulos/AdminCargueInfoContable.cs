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
    public class AdminCargueInfoContable
    {
        private readonly NumberFormatInfo format = new NumberFormatInfo
        {
            NumberDecimalSeparator = "."
        };

        public int CreateTableIfNotExists(int anio)
        {
            var tableName = $"public.tbl_info_contable_{anio}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            using (var createCommand = new PgSqlCommand($@"
                CREATE TABLE IF NOT EXISTS {tableName} (
	                id int4 NOT NULL PRIMARY KEY,
	                un varchar(6) NULL,
	                g_libros varchar(6) NULL,
	                libro varchar(6) NULL,
	                cuenta varchar(15) NULL,
	                sucursal varchar(2) NULL,
	                dependencia varchar(10) NULL,
	                id_asiento varchar(15) NULL,
	                fecha_comprobante date NULL,
	                fecha_proceso date NULL,
	                descripcion varchar(50) NULL,
	                debito numeric(25, 2) NULL,
	                credito numeric(25, 2) NULL,
	                auxiliar varchar(15) NULL,
	                referencia varchar(20) NULL,
	                usuario varchar(15) NULL,
	                id_comprobante varchar(30) NULL,
	                estado varchar(20) NULL,
	                real varchar(1) NULL,
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
            var tableName = $"public.tbl_info_contable_{anio}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            using (var createCommand = new PgSqlCommand($@"DELETE FROM {tableName} WHERE DATE_PART('year', fecha_comprobante) = {anio} AND DATE_PART('month', fecha_comprobante) = {mes}", _cnn))
            {
                _cnn.Open();
                createCommand.ExecuteNonQuery();
                _cnn.Close();
            }
        }

        public void DepuraRows_OD(int anio, int mes)
        {
            var tableName = $"public.tbl_info_contable_{anio}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            using (var createCommand = new PgSqlCommand($@"DELETE FROM {tableName} WHERE SUBSTRING(id_comprobante,1,2) = 'OD'", _cnn))
            {
                _cnn.Open();
                createCommand.ExecuteNonQuery();
                _cnn.Close();
            }
        }

        public void DepuraRows_PG_TC(int anio, int mes)
        {
            var tableName = $"public.tbl_info_contable_{anio}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            using (var createCommand = new PgSqlCommand($@"DELETE FROM {tableName} WHERE cuenta in (SELECT cuenta from tbl_concepto WHERE id = 4) AND SUBSTRING(id_comprobante,1,2) = 'PG' AND descripcion = 'RETEICA ESTABLECIMIENTOS DE CO'", _cnn))
            {
                _cnn.Open();
                createCommand.ExecuteNonQuery();
                _cnn.Close();
            }
        }

        public void DepuraRows_PC_TC(int anio, int mes)
        {
            var tableName = $"public.tbl_info_contable_{anio}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            using (var createCommand = new PgSqlCommand($@"DELETE FROM {tableName} WHERE cuenta in (SELECT cuenta from tbl_concepto WHERE id = 4) AND SUBSTRING(id_comprobante,1,2) = 'PC'", _cnn))
            {
                _cnn.Open();
                createCommand.ExecuteNonQuery();
                _cnn.Close();
            }
        }

        public void InsertData(string idUsuario, int anio, int startId, List<InformacionContable> informacionesContables)
        {
            var insertTemplate = "({0}, '{1}', '{2}', '{3}', '{4}', '{5}', {6}, '{7}', '{8}', '{9}', '{10}', {11}, {12}, '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', {19})";

            var lastInformacionContable = informacionesContables[informacionesContables.Count - 1];
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"INSERT INTO public.tbl_info_contable_{anio} VALUES");
            stringBuilder.Append(Environment.NewLine);

            for (int i = 0; i < informacionesContables.Count; i++)
            {
                var informacionContable = informacionesContables[i];
                stringBuilder.AppendFormat(insertTemplate, startId, informacionContable.Un, informacionContable.GLibros,
                    informacionContable.Libro, informacionContable.Cuenta, informacionContable.Sucursal, informacionContable.Dependencia,
                    informacionContable.IdAsiento, $"{informacionContable.FechaComprobante:yyyy-MM-dd}",
                    $"{informacionContable.FechaProceso:yyyy-MM-dd}", informacionContable.Descripcion, informacionContable.Debito.ToString(format),
                    informacionContable.Credito.ToString(format), informacionContable.Auxiliar, informacionContable.Referencia,
                    informacionContable.Usuario, informacionContable.IdComprobante, informacionContable.Estado, informacionContable.Real,
                    idUsuario);

                if (informacionContable == lastInformacionContable)
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

    public class InformacionContable
    {
        public int Id { get; set; }

        public string Un { get; set; }

        public string GLibros { get; set; }

        public string Libro { get; set; }

        public string Cuenta { get; set; }

        public string Sucursal { get; set; }

        public string Dependencia { get; set; }

        public string IdAsiento { get; set; }

        public DateTime FechaComprobante { get; set; }

        public DateTime FechaProceso { get; set; }

        public string Descripcion { get; set; }

        public float Debito { get; set; }

        public float Credito { get; set; }

        public string Auxiliar { get; set; }

        public string Referencia { get; set; }

        public string Usuario { get; set; }

        public string IdComprobante { get; set; }

        public string Estado { get; set; }

        public char Real { get; set; }
    }
}