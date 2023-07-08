using Devart.Data.PostgreSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;

namespace Smartax.Web.Application.Clases.Modulos
{
    public class AdminCarguePagaduria
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
            var tableName = $"public.tbl_pagaduria_{anio}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            using (var createCommand = new PgSqlCommand($@"
                CREATE table if not exists {tableName} (
                    id int4 NOT NULL PRIMARY KEY,
                    unidad_negocio varchar(10) NULL,
                    id_comprobante varchar(10) NULL,
                    n_linea_comprobante int8 NULL,
                    id_set_proveedor varchar(6) NULL,
                    id_proveedor varchar(15) NULL,
                    numero_identificacion varchar(15) NULL,
                    fecha_contable date NULL,
                    tipo_impuesto varchar(10) NULL,
                    codigo_actividad varchar(10) NULL,
                    importe_base_retencion numeric(22, 2) NULL,
                    base_retencion_moneda_base numeric(22, 2) NULL,
                    importe_retencion numeric(22, 2) NULL,
                    importe_retencion_moneda_base numeric(22, 2) NULL,
                    porcentaje_retencion numeric(22, 2) NULL,
                    moneda_transaccion varchar(6) NULL,
                    moneda_base varchar(6) NULL,
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
            var tableName = $"public.tbl_pagaduria_{anio}";
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            using (var createCommand = new PgSqlCommand($@"DELETE FROM {tableName} WHERE DATE_PART('year', fecha_contable) = {anio} AND DATE_PART('month', fecha_contable) = {mes}", _cnn))
            {
                _cnn.Open();
                createCommand.ExecuteNonQuery();
                _cnn.Close();
            }
        }

        public void InsertData(int anio, int startId, string idUsuario, List<Pagaduria> pagadurias)
        {
            var insertTemplate = "({0}, '{1}', {2}, {3}, '{4}', {5}, '{6}', '{7}', '{8}', '{9}', {10}, {11}, {12}, {13}, {14}, '{15}', '{16}', {17})";
          
        
            var lastPagaduria = pagadurias[pagadurias.Count - 1];
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"INSERT INTO public.tbl_pagaduria_{anio} VALUES ");
            stringBuilder.Append(Environment.NewLine);

            for (int i = 0; i < pagadurias.Count; i++)
            {
                var pagaduria = pagadurias[i];
                stringBuilder.AppendFormat(insertTemplate, startId, 
                    pagaduria.UnidadNegocio, pagaduria.IdComprobante, pagaduria.LineaComprobante, pagaduria.IdSetProveedor, 
                    pagaduria.IdProveedor, pagaduria.NumeroIdentificacion, $"{pagaduria.FechaContable:yyyy-MM-dd}", 
                    pagaduria.TipoImpuesto, pagaduria.CodigoActividad, pagaduria.ImporteBaseRetencion.ToString(CultureInfo.InvariantCulture), 
                    pagaduria.BaseRetencionMonedaBase.ToString(CultureInfo.InvariantCulture),
                    pagaduria.ImporteRetencion.ToString(CultureInfo.InvariantCulture), 
                    pagaduria.ImporteRetencionMonedaBase.ToString(CultureInfo.InvariantCulture), 
                    pagaduria.PorcentajeRetencion.ToString(CultureInfo.InvariantCulture), pagaduria.MonedaTransaccion, pagaduria.MonedaBase, idUsuario);

                if (pagaduria == lastPagaduria)
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
                var command = new PgSqlCommand($"{stringBuilder}", _cnn, transaction);
                command.ExecuteNonQuery();
                transaction.Commit(); 
            }
        }
    }

    public class Pagaduria
    {
        public string UnidadNegocio { get; set; }

        public string IdComprobante { get; set; }

        public int LineaComprobante { get; set; }

        public string IdSetProveedor { get; set; }

        public string IdProveedor { get; set; }

        public string NumeroIdentificacion { get; set; }

        public DateTime FechaContable { get; set; }

        public string TipoImpuesto { get; set; }

        public string CodigoActividad { get; set; }

        public float ImporteBaseRetencion { get; set; }

        public float BaseRetencionMonedaBase { get; set; }

        public float ImporteRetencion { get; set; }

        public float ImporteRetencionMonedaBase { get; set; }

        public float PorcentajeRetencion { get; set; }

        public string MonedaTransaccion { get; set; }

        public string MonedaBase { get; set; }
    }
}