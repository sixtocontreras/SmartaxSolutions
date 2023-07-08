using Devart.Data.PostgreSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Reportes.Formatos
{
    public class Admin321
    {
        public DataTable Get(int cliente, int periodo, int mes)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($"select * from (select a.idcliente as CodigoEmpresa,{periodo} Vigencia,{mes} Periodo,a.unidadcaptura,a.nombredepto as NombreDepartamento," +
                $" a.codigodane as SubCuenta,a.municipio as NombreMunicipio,orden as Columna, cast(sum(total) as varchar(50)) as Valor from archivodev a " +
                $" where a.anio = {periodo} and a.mes = {mes} and a.idcliente ={cliente} and a.orden is not null group by a.idcliente,a.unidadcaptura,a.nombredepto,a.codigodane,a.municipio,a.orden  " +
                $"order by a.unidadcaptura ,a.codigodane ,a.orden ) x where cast(x.valor as double PRECISION) < 0 ", _cnn);
            var dt = new DataTable();
            dt.TableName = "infoEntidad";
            _da.Fill(dt);
            return dt;
        }
    }
}