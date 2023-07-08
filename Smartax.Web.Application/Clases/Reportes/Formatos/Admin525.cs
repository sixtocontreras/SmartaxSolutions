using Devart.Data.PostgreSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Reportes.Formatos
{
    public class Admin525
    {
        public DataTable Get(int cliente, int periodo, int mes)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select * from (select a.idcliente as CodigoEmpresa,{periodo} Vigencia,{mes} Periodo,'01' unidadcaptura,a.subcuenta, a.etiqueta  puc, a.nombre,cast(sum(a.total) as varchar(50)) as valor from archivodev a " +
                $"where a.anio = {periodo} and a.mes = {mes} and a.idcliente = {cliente} and a.subcuenta is not null group by a.idcliente, a.subcuenta, a.nombre, a.etiqueta) a where cast(a.valor as double PRECISION) < 0 " +
                 $" order by a.unidadcaptura ,a.subcuenta  ", _cnn);
            var dt = new DataTable();
            dt.TableName = "infoEntidad";
            _da.Fill(dt);
            return dt;
        }
    }
}