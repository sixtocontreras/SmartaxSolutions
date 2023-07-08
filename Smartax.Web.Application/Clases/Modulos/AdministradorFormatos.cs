using Devart.Data.PostgreSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Modulos
{
    public class AdministradorFormatos
    {
        public bool GenerarProceso(int idCliente, int periodo, int mes)
        {
            var cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var adp = new PgSqlDataAdapter($"select sp_task_data({idCliente},{periodo},{mes})", cnn);
            var dt = new DataTable();
            adp.Fill(dt);
            if (dt.Rows[0][0].ToString() != "ok")
                return false;
            return true;
        }
    }
}