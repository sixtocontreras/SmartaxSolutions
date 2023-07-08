using Devart.Data.PostgreSql;
using SpreadsheetLight;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;

namespace Smartax.Web.Application.Clases.Administracion
{
    public class AdministradorExcel
    {
        
        public DataTable GenerateFormat321(int periodo, int cliente, string rutaBase, int mes)
        {
            var tbplano = new DataTable();
            tbplano.Columns.Add("Columna");
            tbplano.Columns.Add("UnidadC");
            tbplano.Columns.Add("SubCta");
            tbplano.Columns.Add("Valor");
            var dt = get(cliente, periodo);
            var generalData = GeneralData(cliente);
            var suma = GetSum(cliente, periodo);

            double sumtotalBase = 0;
            double sumBaseR = 0;
            var dpto = dt.First().Depto;
            var mun = dt.First().Municipio;
            var dane = dt.First().codDane;
            var columnas = Columnas.get();

            var nom_mes = "XXX";
            var max_dia_mes = "99";

            switch (mes)
            {
                case 1:
                    nom_mes = "ENE";
                    max_dia_mes = "31";
                    break;
                case 2:
                    nom_mes = "FEB";
                    max_dia_mes = "28";
                    if (periodo == 2020 | periodo == 2024 | periodo == 2028 | periodo == 2032) max_dia_mes = "29";
                    break;
                case 3:
                    nom_mes = "MAR";
                    max_dia_mes = "31";
                    break;
                case 4:
                    nom_mes = "ABR";
                    max_dia_mes = "30";
                    break;
                case 5:
                    nom_mes = "MAY";
                    max_dia_mes = "31";
                    break;
                case 6:
                    nom_mes = "JUN";
                    max_dia_mes = "30";
                    break;
                case 7:
                    nom_mes = "JUL";
                    max_dia_mes = "31";
                    break;
                case 8:
                    nom_mes = "AGO";
                    max_dia_mes = "31";
                    break;
                case 9:
                    nom_mes = "SEP";
                    max_dia_mes = "30";
                    break;
                case 10:
                    nom_mes = "OCT";
                    max_dia_mes = "31";
                    break;
                case 11:
                    nom_mes = "NOV";
                    max_dia_mes = "30";
                    break;
                case 12:
                    nom_mes = "DIC";
                    max_dia_mes = "31";
                    break;
            }

            string fileName = rutaBase + $@"Archivos\Formatos\{cliente}\F321_{nom_mes}_{periodo}.xlsx";

            if (!Directory.Exists(rutaBase + $@"Archivos\Formatos\{cliente}"))
                Directory.CreateDirectory(rutaBase + $@"Archivos\Formatos\{cliente}");
            if (File.Exists(fileName))
                File.Delete(fileName);
            File.Copy(rutaBase + @"Archivos\Formatos\Plantillas\Template321.xlsx", fileName);

            
            using (SLDocument sl = new SLDocument(fileName))
            {
                sl.SetCellValue("B6", generalData[2].ToString());
                sl.SetCellValue("D6", generalData[3].ToString());
                sl.SetCellValue("F6", generalData[4].ToString());
                sl.SetCellValue("M6", $"{max_dia_mes}-{mes}-{dt.First().anio}");

                var headers = dt.Where(x => x.Orden != 0).Select(x => new { x.Orden, x.Etiqueta }).Distinct().ToList();

                sl.SetCellValue($"{columnas[suma.Key]}11", suma.Value);
                foreach (var item in headers)
                {
                    sl.SetCellValue($"{columnas[item.Orden]}11",item.Etiqueta);
                }
                uint row = 13;
                var general = true;
                foreach (var item in dt)
                {
                    string unidad = string.Empty;
                    if ((item.codDane+item.Depto) != (dane+dpto))
                    {
                        
                        sl.SetCellValue($"{columnas[suma.Key]}{row}", sumBaseR);
                        //InsertInCell(ref ws, columnas[suma.Key], row, val, false, "#,##0.00");
                        unidad = dt.First(x => x.codDane.Equals(dane) && x.Municipio.Trim().Equals(mun) && x.Depto.Equals(dpto)).UnidadCaptura;

                        sl.SetCellValue($"{columnas[38]}{row}", unidad);
                        if (sumBaseR != 0)
                        {
                            var rowTable1 = tbplano.NewRow();
                            rowTable1["Columna"] = suma.Key;
                            rowTable1["UnidadC"] = unidad;
                            rowTable1["SubCta"] = dane;
                            rowTable1["Valor"] = sumBaseR;
                            tbplano.Rows.Add(rowTable1);
                        }
                        foreach (var hd in headers)
                        {
                            var data = sl.GetCellValueAsString($"{columnas[hd.Orden]}{row}");
                            if(string.IsNullOrEmpty(data))
                                sl.SetCellValue($"{columnas[hd.Orden]}{row}", 0);
                        }
                        //InsertInCell(ref ws, columnas[38], row, unidad, false, string.Empty);
                        dane = item.codDane;
                        mun = item.Municipio;
                        row++;
                        general = true;
                        sumBaseR = 0;
                    }
                    if (dpto != item.Depto)
                    {
                        var dataSum = dt.Where(x => x.Depto.Equals(dpto)).GroupBy(x => x.Orden).Select(x => new { Orden = x.Key, Valor = x.Sum(y => y.Valor) });
                        if (dpto != 3)
                        {

                            sl.SetCellValue($"A{row}", "999");
                            sl.SetCellValue($"B{row}", $"TOTAL {dt.First(x => x.Depto.Equals(dpto)).nombreDepto.ToUpper()}");
                        }
                        //InsertInCell(ref ws, "A", row, "999", true, string.Empty);
                        //InsertInCell(ref ws, "B", row, $"TOTAL {dt.First(x => x.Depto.Equals(dpto)).nombreDepto.ToUpper()}", true, string.Empty);
                        if (dataSum.Any(x => x.Orden.Equals(0)) && dataSum.Count() == 1)
                        {
                            foreach (var hd in headers)
                            {
                                if (dpto != 3)
                                {
                                    sl.SetCellValue($"{columnas[hd.Orden]}{row}", 0);
                                    //InsertInCell(ref ws, columnas[hd.Orden], row, 0, true, "#,##0.00");
                                }

                            }
                        }
                        else
                        {
                            sumtotalBase = 0;
                            foreach (var sum in dataSum)
                            {
                                if (!sum.Orden.Equals(0))
                                {
                                    if (dpto != 3)
                                    {
                                        sl.SetCellValue($"{columnas[sum.Orden]}{row}", sum.Valor);
                                    }
                                    sumtotalBase += sum.Valor;
                                    if (sum.Valor != 0)
                                    {
                                        var rowTable = tbplano.NewRow();
                                        rowTable["Columna"] = sum.Orden;
                                        rowTable["UnidadC"] = unidad;
                                        rowTable["SubCta"] = "999";
                                        rowTable["Valor"] = sum.Valor;
                                        tbplano.Rows.Add(rowTable);
                                    }
                                    //InsertInCell(ref ws, columnas[sum.Orden], row, sum.Valor, true, "#,##0.00");
                                }
                            }
                        }
                        if (dpto != 3)
                        {
                            sl.SetCellValue($"{columnas[suma.Key]}{row}", sumtotalBase);
                            sl.SetCellValue($"{columnas[38]}{row}", unidad);
                        }
                        if (sumtotalBase != 0)
                        {
                            var rowTable = tbplano.NewRow();
                            rowTable["Columna"] = suma.Key;
                            rowTable["UnidadC"] = unidad;
                            rowTable["SubCta"] = "999";
                            rowTable["Valor"] = sumtotalBase;
                            tbplano.Rows.Add(rowTable);
                        }
                        foreach (var hd in headers)
                        {
                            var data = sl.GetCellValueAsString($"{columnas[hd.Orden]}{row}");
                            if ((string.IsNullOrEmpty(data)) && dpto != 3)
                                sl.SetCellValue($"{columnas[hd.Orden]}{row}", 0);
                        }
                        //InsertInCell(ref ws, columnas[suma.Key], row, val, true, "#,##0.00");
                        //InsertInCell(ref ws, columnas[38], row, unidad, true, string.Empty);
                        if (dpto != 3)
                        {
                            row++;
                        }

                        dpto = item.Depto;
                        row++;
                        sumtotalBase = 0;

                    }
                    if (general)
                    {
                        sl.SetCellValue($"A{row}", item.codDane);
                        sl.SetCellValue($"B{row}", item.Municipio.Trim());
                        //InsertInCell(ref ws, "A", row, item.codDane, false, string.Empty);
                        //InsertInCell(ref ws, "B", row, item.Municipio.Trim(), false, string.Empty);
                        general = false;
                    }
                    if (item.Orden.Equals(0))
                    {
                        if (dt.Count(x => x.Municipio.Equals(item.Municipio)) == 1)
                        {
                            foreach (var hd in headers)
                            {
                                sl.SetCellValue($"{columnas[hd.Orden]}{row}", 0);
                                //InsertInCell(ref ws, columnas[hd.Orden], row, 0, false, "#,##0.00");
                            }
                        }
                    }
                    else
                    {
                        sl.SetCellValue($"{columnas[item.Orden]}{row}", item.Valor);
                        sumBaseR += item.Valor;
                        if (item.Valor != 0)
                        {
                            var rowTable2 = tbplano.NewRow();
                            rowTable2["Columna"] = item.Orden;
                            rowTable2["UnidadC"] = item.UnidadCaptura;
                            rowTable2["SubCta"] = dane;
                            rowTable2["Valor"] = item.Valor;
                            tbplano.Rows.Add(rowTable2);
                        }
                    }


                }
                sl.SetCellValue($"{columnas[suma.Key]}{row}", sumBaseR);               
                //InsertInCell(ref ws, columnas[suma.Key], row, val2, false, "#,##0.00");

                var unidad2 = dt.First(x => x.codDane.Equals(dane) && x.Municipio.Trim().Equals(mun)).UnidadCaptura;
                sl.SetCellValue($"{columnas[38]}{row}", unidad2);
                if (sumBaseR != 0)
                {
                    var rowTable3 = tbplano.NewRow();
                    rowTable3["Columna"] = suma.Key;
                    rowTable3["UnidadC"] = unidad2;
                    rowTable3["SubCta"] = dane;
                    rowTable3["Valor"] = sumBaseR;
                    tbplano.Rows.Add(rowTable3);
                }
                //InsertInCell(ref ws, columnas[38], row, unidad2, false, string.Empty);
                row++;

                var dataSum2 = dt.Where(x => x.Depto.Equals(dpto)).GroupBy(x => x.Orden).Select(x => new { Orden = x.Key, Valor = x.Sum(y => y.Valor) });

                sl.SetCellValue($"A{row}", "999");
                sl.SetCellValue($"B{row}", $"TOTAL {dt.First(x => x.Depto.Equals(dpto)).nombreDepto.ToUpper()}");
                //InsertInCell(ref ws, "A", row, "999", true, string.Empty);
                //InsertInCell(ref ws, "B", row, $"TOTAL {dt.First(x => x.Depto.Equals(dpto)).nombreDepto.ToUpper()}", true, string.Empty);
                if (dataSum2.Any(x => x.Orden.Equals(0)) && dataSum2.Count() == 1)
                {
                    foreach (var hd in headers)
                    {

                        sl.SetCellValue($"{columnas[hd.Orden]}{row}", 0);
                        //InsertInCell(ref ws, columnas[hd.Orden], row, 0, true, "#,##0.00");

                    }
                }
                else
                {
                    sumtotalBase = 0;
                    foreach (var sum in dataSum2)
                    {
                        if (!sum.Orden.Equals(0))
                        {
                            sl.SetCellValue($"{columnas[sum.Orden]}{row}", sum.Valor);
                            sumtotalBase += sum.Valor;
                            if (sum.Valor != 0)
                            {
                                var rowTable4 = tbplano.NewRow();
                                rowTable4["Columna"] = sum.Orden;
                                rowTable4["UnidadC"] = unidad2;
                                rowTable4["SubCta"] = "999";
                                rowTable4["Valor"] = sum.Valor;
                                tbplano.Rows.Add(rowTable4);
                            }
                            //InsertInCell(ref ws, columnas[sum.Orden], row, sum.Valor, true, "#,##0.00");
                        }
                    }
                }

                sl.SetCellValue($"{columnas[suma.Key]}{row}", sumtotalBase);
                sl.SetCellValue($"{columnas[38]}{row}", unidad2);


                //InsertInCell(ref ws, columnas[suma.Key], row, val3, true, "#,##0.00");
                //InsertInCell(ref ws, columnas[38], row, unidad2, false, string.Empty);
                row++;
                row++;


                sl.SetCellValue($"A{row}", "005");
                sl.SetCellValue($"B{row}", "TOTAL NACIONAL");
                //InsertInCell(ref ws, "A", row, "005", true, string.Empty);
                //InsertInCell(ref ws, "B", row, "TOTAL NACIONAL", true, string.Empty);
                double sumNacBase = 0;
                //InsertInCell(ref ws, columnas[suma.Key], row, dt.Sum(x => x.Valor), true, "#,##0.00");
                foreach (var item in headers)
                {
                    var datoSum = dt.Where(x => x.Orden.Equals(item.Orden)).Sum(x => x.Valor);
                    sl.SetCellValue($"{columnas[item.Orden]}{row}", datoSum);
                    sumNacBase += datoSum;
                    if (datoSum != 0)
                    {
                        var rowTable4 = tbplano.NewRow();
                        rowTable4["Columna"] = item.Orden;
                        rowTable4["UnidadC"] = generalData[14].ToString();
                        rowTable4["SubCta"] = "005";
                        rowTable4["Valor"] = datoSum;
                        tbplano.Rows.Add(rowTable4);
                    }
                    //InsertInCell(ref ws, columnas[item.Orden], row, dt.Where(x => x.Orden.Equals(item.Orden)).Sum(x => x.Valor), true, "#,##0.00");
                }

                sl.SetCellValue($"{columnas[suma.Key]}{row}", sumNacBase);
                sl.SetCellValue($"{columnas[38]}{row}", generalData[14].ToString());
                if (sumNacBase != 0)
                {
                    var rowTable = tbplano.NewRow();
                    rowTable["Columna"] = suma.Key;
                    rowTable["UnidadC"] = generalData[14].ToString();
                    rowTable["SubCta"] = "005";
                    rowTable["Valor"] = sumNacBase;
                    tbplano.Rows.Add(rowTable);
                }
                //InsertInCell(ref ws, columnas[38], row, generalData[14].ToString(), true, string.Empty);
                sl.Save();
            }
            return tbplano;



        }
        
        public DataTable GenerateFormat525(int periodo, int cliente, string rutaBase, int mes)
        {
            var tbplano = new DataTable();
            tbplano.Columns.Add("Columna");
            tbplano.Columns.Add("UnidadC");
            tbplano.Columns.Add("SubCta");
            tbplano.Columns.Add("Valor");
            var dt = getTotal(cliente,periodo);
            var generalData = GeneralData(cliente);


            var nom_mes = "XXX";
            var max_dia_mes = "99";

            switch (mes)
            {
                case 1:
                    nom_mes = "ENE";
                    max_dia_mes = "31";
                    break;
                case 2:
                    nom_mes = "FEB";
                    max_dia_mes = "28";
                    if (periodo == 2020 | periodo == 2024 | periodo == 2028 | periodo == 2032) max_dia_mes = "29";
                    break;
                case 3:
                    nom_mes = "MAR";
                    max_dia_mes = "31";
                    break;
                case 4:
                    nom_mes = "ABR";
                    max_dia_mes = "30";
                    break;
                case 5:
                    nom_mes = "MAY";
                    max_dia_mes = "31";
                    break;
                case 6:
                    nom_mes = "JUN";
                    max_dia_mes = "30";
                    break;
                case 7:
                    nom_mes = "JUL";
                    max_dia_mes = "31";
                    break;
                case 8:
                    nom_mes = "AGO";
                    max_dia_mes = "31";
                    break;
                case 9:
                    nom_mes = "SEP";
                    max_dia_mes = "30";
                    break;
                case 10:
                    nom_mes = "OCT";
                    max_dia_mes = "31";
                    break;
                case 11:
                    nom_mes = "NOV";
                    max_dia_mes = "30";
                    break;
                case 12:
                    nom_mes = "DIC";
                    max_dia_mes = "31";
                    break;
            }


            string fileName = rutaBase + $@"Archivos\Formatos\{cliente}\F525_{nom_mes}_{periodo}.xlsx";
            if (!Directory.Exists(rutaBase + $@"Archivos\Formatos\{cliente}"))
                Directory.CreateDirectory(rutaBase + $@"Archivos\Formatos\{cliente}");
            if (File.Exists(fileName))
                File.Delete(fileName);
            File.Copy(rutaBase + @"Archivos\Formatos\Plantillas\Template525.xlsx", fileName);
            
            using (SLDocument sl = new SLDocument(fileName))
            {
                sl.SetCellValue($"B7", generalData[2].ToString());
                sl.SetCellValue($"C7", $"{generalData[3].ToString()}                            {generalData[4].ToString()}");
                sl.SetCellValue($"E7", $"{max_dia_mes}-{mes}-{dt.First().anio}");
                //ws.Rows[6].Columns[1].Value = generalData[2].ToString();
                //ws.Rows[6].Columns[2].Value = $"{generalData[3].ToString()}                            {generalData[4].ToString()}";
                //ws.Rows[6].Columns[5].Value = $"31-12-{dt.First().anio}";
                uint row = 13;
                foreach (var item in dt)
                {

                    sl.SetCellValue($"A{row}", item.subCuenta);
                    sl.SetCellValue($"B{row}", item.Etiqueta);
                    sl.SetCellValue($"C{row}", item.Nombre);
                    sl.SetCellValue($"D{row}", item.Valor);
                    sl.SetCellValue($"F{row}", generalData[15].ToString());
                    if (item.Valor != 0)
                    {
                        var rowTable4 = tbplano.NewRow();
                        rowTable4["Columna"] = 1;
                        rowTable4["UnidadC"] = generalData[15].ToString();
                        rowTable4["SubCta"] = item.subCuenta;
                        rowTable4["Valor"] = item.Valor;
                        tbplano.Rows.Add(rowTable4);
                    }
                    //InsertInCell(ref ws, "A", row, item.subCuenta, false, string.Empty);
                    //InsertInCell(ref ws, "B", row, item.Etiqueta, false, string.Empty);
                    //InsertInCell(ref ws, "C", row, item.Nombre, false, string.Empty);
                    //InsertInCell(ref ws, "D", row, item.Valor, false, "#,##0.00");
                    //InsertInCell(ref ws, "F", row, generalData[15].ToString(), false, string.Empty);
                    row++;
                }

                sl.Save();
                
            }
            return tbplano;

            
        }
        
        private List<DataDB> get(int idcliente, int anio)
        {
            var cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var adp = new PgSqlDataAdapter($"select * from archivodev where idcliente = {idcliente} and anio = {anio} order by unidadCaptura,codigodane, orden ", cnn);
            var dt = new DataTable();
            adp.Fill(dt);
            return (from DataRow dr in dt.Rows
                    select new DataDB()
                    {
                        codDane = dr[0].ToString(),
                        subCuenta = dr[1].ToString(),
                        Municipio = dr[2].ToString(),
                        Valor = double.Parse(dr[3].ToString() == "" ? "0" : dr[3].ToString()),
                        Orden = int.Parse(dr[4].ToString() == "" ? "0" : dr[4].ToString()),
                        Etiqueta = dr[5].ToString(),
                        Nombre = dr[6].ToString(),
                        Depto = int.Parse(dr[8].ToString()),
                        nombreDepto = dr[9].ToString(),
                        UnidadCaptura = dr[10].ToString(),
                        anio = int.Parse(dr[11].ToString())

                    }).ToList();
        }

        private List<DataDB> getTotal(int idcliente, int anio)
        {
            var cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var adp = new PgSqlDataAdapter($"select b.subcuenta_f525 subcuenta,b.etiqueta, b.nombre_cuenta_f525 nombre,a.total, {anio} anio from tbl_param_f321_f525 b left join (select subcuenta, etiqueta, nombre, sum(total) total,anio from archivodev where idcliente = {idcliente} and anio = {anio} and subcuenta is not null group by subcuenta, etiqueta, nombre, anio) a on a.subcuenta = b.subcuenta_f525 where b.subcuenta_f525 is not null order by b.subcuenta_f525", cnn);
            var dt = new DataTable();
            adp.Fill(dt);
            return (from DataRow dr in dt.Rows
                    select new DataDB()
                    {
                        subCuenta = dr[0].ToString(),
                        Etiqueta = dr[1].ToString(),
                        Nombre = dr[2].ToString(),
                        Valor = double.Parse(dr[3].ToString() == "" ? "0" : dr[3].ToString()),
                        anio = int.Parse(dr[4].ToString())
                    }).ToList();
        }

        private KeyValuePair<int, string> GetSum(int idcliente, int anio)
        {
            var cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var adp = new PgSqlDataAdapter($"select columna_321 orden, etiqueta  from tbl_param_f321_f525 tpff where base_resultante ='S' and id_cliente = {idcliente}", cnn);
            var dt = new DataTable();
            adp.Fill(dt);
            return (from DataRow dr in dt.Rows
                    select new KeyValuePair<int, string>(int.Parse(dr[0].ToString()), dr[1].ToString())).First();
        }

        private DataRow GeneralData(int idcliente)
        {
            var cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var adp = new PgSqlDataAdapter($"select * from tbl_informacion_cliente_sfc where id_cliente = {idcliente}", cnn);
            var dt = new DataTable();
            adp.Fill(dt);
            return dt.Rows[0];
        }

    }
    public class DataDB
    {
        public string codDane { get; set; }
        public string subCuenta { get; set; }
        public string Municipio { get; set; }
        public double Valor { get; set; }
        public int Orden { get; set; }
        public string Etiqueta { get; set; }
        public string Nombre { get; set; }
        public int Depto { get; set; }
        public string nombreDepto { get; set; }
        public string UnidadCaptura { get; set; }
        public int anio { get; set; }
    }
    public static class Columnas
    {
        public static Dictionary<int, string> get()
        {
            var dic = new Dictionary<int, string>();
            dic.Add(1, "C");
            dic.Add(2, "D");
            dic.Add(3, "E");
            dic.Add(4, "F");
            dic.Add(5, "G");
            dic.Add(6, "H");
            dic.Add(7, "I");
            dic.Add(8, "J");
            dic.Add(9, "K");
            dic.Add(10, "L");
            dic.Add(11, "M");
            dic.Add(12, "N");
            dic.Add(13, "O");
            dic.Add(14, "P");
            dic.Add(15, "Q");
            dic.Add(16, "R");
            dic.Add(17, "S");
            dic.Add(18, "T");
            dic.Add(19, "U");
            dic.Add(20, "V");
            dic.Add(21, "W");
            dic.Add(22, "X");
            dic.Add(23, "Y");
            dic.Add(24, "Z");
            dic.Add(25, "AA");
            dic.Add(26, "AB");
            dic.Add(27, "AC");
            dic.Add(28, "AD");
            dic.Add(29, "AE");
            dic.Add(30, "AF");
            dic.Add(31, "AG");
            dic.Add(32, "AH");
            dic.Add(33, "AI");
            dic.Add(34, "AJ");
            dic.Add(35, "AK");
            dic.Add(36, "AL");
            dic.Add(37, "AM");
            dic.Add(38, "AN");
            return dic;
        }
    }
}