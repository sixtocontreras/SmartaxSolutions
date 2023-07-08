using Devart.Data.PostgreSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Reportes.Formatos
{
    public class Cruzado
    {
        public DataTable Get(int cliente, int periodo, int mes)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select a.idcliente as CodigoEmpresa,{periodo} Vigencia, {mes} Mes, a.orden as Columna321,a.etiqueta Etiqueta321,c.base_resultante,a.subcuenta ,a.nombre Nombrecuenta525, " +
                $"cast(sum(a.total) as varchar(50)) Valor321, cast(sum(a.total) as varchar(50)) Valor525 " +
                $"from archivodev a join tbl_param_f321_f525 c on a.orden = c.columna_321 and c.id_cliente = a.idcliente " +
                $"where a.anio = {periodo} and a.mes = {mes} and a.idcliente = {cliente} " +
                $"group by a.idcliente, a.orden, a.etiqueta, c.base_resultante, a.subcuenta, a.nombre " +
                $"order by a.orden ", _cnn);
            var dt = new DataTable();
            dt.TableName = "infoEntidad";
            _da.Fill(dt);
            return dt;
        }
    }
}