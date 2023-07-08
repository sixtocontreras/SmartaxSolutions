using Devart.Data.PostgreSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Administracion
{
    public class AdministracionPlanos
    {
        public void GenerarPlano(int periodo, int cliente, string rutaBase, DataTable data321, DataTable data525, int mes)
        {
            var nom_mes = "XXX";
            var max_dia_mes = "99";
            var num_mes = "99";

            switch (mes)
            {
                case 1:
                    nom_mes = "ENE";
                    num_mes = "01";
                    max_dia_mes = "31";
                    break;
                case 2:
                    nom_mes = "FEB";
                    num_mes = "02";
                    max_dia_mes = "28";
                    if (periodo == 2020 | periodo == 2024 | periodo == 2028 | periodo == 2032) max_dia_mes = "29";
                    break;
                case 3:
                    nom_mes = "MAR";
                    num_mes = "03";
                    max_dia_mes = "31";
                    break;
                case 4:
                    nom_mes = "ABR";
                    num_mes = "04";
                    max_dia_mes = "30";
                    break;
                case 5:
                    nom_mes = "MAY";
                    num_mes = "05";
                    max_dia_mes = "31";
                    break;
                case 6:
                    nom_mes = "JUN";
                    num_mes = "06";
                    max_dia_mes = "30";
                    break;
                case 7:
                    nom_mes = "JUL";
                    num_mes = "07";
                    max_dia_mes = "31";
                    break;
                case 8:
                    nom_mes = "AGO";
                    num_mes = "08";
                    max_dia_mes = "31";
                    break;
                case 9:
                    nom_mes = "SEP";
                    num_mes = "09";
                    max_dia_mes = "30";
                    break;
                case 10:
                    nom_mes = "OCT";
                    num_mes = "10";
                    max_dia_mes = "31";
                    break;
                case 11:
                    nom_mes = "NOV";
                    num_mes = "11";
                    max_dia_mes = "30";
                    break;
                case 12:
                    nom_mes = "DIC";
                    num_mes = "12";
                    max_dia_mes = "31";
                    break;
            }

            string fileName = rutaBase + $@"Archivos\Formatos\{cliente}\F321_{nom_mes}_{periodo}.txt";
            if (File.Exists(fileName))
                File.Delete(fileName);
            var data = get(cliente, periodo, num_mes, max_dia_mes);
            var list321 = (from DataRow dr in data321.Rows
                           select new ObjPlano
                           {
                               Orden = int.Parse(dr[0].ToString()),
                               UnidadC = dr[1].ToString(),
                               SubCta = dr[2].ToString(),
                               Valor = double.Parse(dr[3].ToString())

                           }).OrderBy(x => x.Orden).ThenBy(x => x.UnidadC).ThenBy(x =>x.SubCta).ToList();
            var list525 = (from DataRow dr in data525.Rows
                           select new ObjPlano
                           {
                               Orden = int.Parse(dr[0].ToString()),
                               UnidadC = dr[1].ToString(),
                               SubCta = dr[2].ToString(),
                               Valor = double.Parse(dr[3].ToString())

                           }).OrderBy(x => x.Orden).ThenBy(x => x.UnidadC).ThenBy(x => x.SubCta).ToList();
            var ord = 3;
            foreach (var item in list525)
            {
                item.Id = ord;
                ord++;
            }
            ord++;

            foreach (var item in list321)
            {
                if (item.SubCta == "999" && item.UnidadC == "03") 
                {
                    item.Id = 99999;
                    
                }
                else
                {
                    item.Id = ord;
                    ord++;
                }
            }

            list321.RemoveAll(x => x.Id == 99999);

            data.AddRange(list525.Select(x => $"{x.Id.ToString().PadLeft(5, '0')}452501{x.UnidadC.PadLeft(2, '0')}{x.SubCta.PadLeft(3, '0')}{(x.Valor > 0 ? "+" : "-")}{Math.Abs(x.Valor).ToString("N2").Replace(".", "").Replace(",", ".").PadLeft(20, '0')}"));
            var row2 = data[1];
            row2 = (list321[0].Id - 1).ToString().PadLeft(5, '0') + row2.Substring(5);
            data.Add(row2);
            data.AddRange(list321.Select(x => $"{x.Id.ToString().PadLeft(5, '0')}4321{x.Orden.ToString().PadLeft(2, '0')}{x.UnidadC.PadLeft(2, '0')}{x.SubCta.PadLeft(3, '0')}{(x.Valor > 0 ? "+" : "-")}{Math.Abs(x.Valor).ToString("N2").Replace(".", "").Replace(",", ".").PadLeft(20, '0')}"));
            data.Add((data.Count() + 1).ToString().PadLeft(5, '0') + "5");
            data[0] = data[0].Replace("[numReg]", (data.Count()).ToString().PadLeft(5, '0'));
            File.WriteAllLines(fileName, data);
        }


        private List<string> get(int idcliente, int periodo, string num_mes, string max_dia_mes)
        {
            var cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var adp = new PgSqlDataAdapter($"select * from sp_plano({idcliente}, {periodo}, '{num_mes}', '{max_dia_mes}')", cnn);
            //var adp = new PgSqlDataAdapter($"select * from sp_plano({idcliente}, {periodo})", cnn);
            var dt = new DataTable();
            adp.Fill(dt);
            return (from DataRow dr in dt.Rows
                    select new
                    {
                         data = dr[2].ToString()

                    }).Select(x=>x.data).ToList();
        }
    }
    public class ObjPlano {
        public int Id { get; set; }
        public int Orden { get; set; }
        public string UnidadC { get; set; }
        public string SubCta { get; set; }
        public double Valor { get; set; }
    }
}