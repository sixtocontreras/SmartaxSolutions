using Devart.Data.PostgreSql;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Parametros.ReteICA
{
    public class ConfConceptos
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        internal DataTable GetAll()
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"SELECT id, concepto, cuenta FROM public.tbl_concepto order by id;", _cnn);
            var dt = new DataTable { TableName = "DtConceptos" };
            _da.Fill(dt);
            return dt;
        }

        internal bool Update(string idUsuario, int id, string concepto, string cuenta)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            _cnn.Open();
            var _da = new PgSqlCommand($"UPDATE public.tbl_concepto SET concepto = '{concepto}', cuenta = '{cuenta}', id_usuario_up = {idUsuario}, fecha_actualizacion = '{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}' WHERE id = {id}; ", _cnn);
            var affectedRows = _da.ExecuteNonQuery();
            _cnn.Close();
            return affectedRows == 1;
        }
    }
}