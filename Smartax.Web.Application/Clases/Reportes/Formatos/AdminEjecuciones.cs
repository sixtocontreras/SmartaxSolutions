using Devart.Data.PostgreSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Reportes.Formatos
{
    public class AdminEjecuciones
    {
        public DataTable Get(int cliente)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select id_cliente CodigoEmpresa,vigencia,periodo,fecha_registro FechaHoraEjecucion from tbl_ejecuciones where id_cliente = {cliente} order by fecha_registro", _cnn);
            var dt = new DataTable();
            dt.TableName = "infoEntidad";
            _da.Fill(dt);
            return dt;
        }
    }
}