﻿using Aspose.Cells;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartax.Cronjob.Process.Clases.Utilidades
{
    public class Functions
    {
        public string GetTime(int nHora)
        {
            string ResultTime = "";
            try
            {
                if (nHora <= 12)
                {
                    ResultTime = "Buenos días";
                }
                else if (nHora <= 18)
                {
                    ResultTime = "Buenas tarde";
                }
                else
                {
                    ResultTime = "Buenas noche";
                }
            }
            catch (Exception ex)
            {
                FixedData.LogApi.Error("Error al obtener el Time. Motivo: " + ex.Message);
            }

            return ResultTime;
        }

        //Método para convertir la Imagen a Bytes
        public Byte[] GetImagenBytes(string strRutaImagen)
        {
            Byte[] Arreglo = null;
            try
            {
                FileStream Imagen = new FileStream(strRutaImagen, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                Arreglo = new Byte[Imagen.Length];
                BinaryReader reader = new BinaryReader(Imagen);
                Arreglo = reader.ReadBytes(Convert.ToInt32(Imagen.Length));
                reader.Close();
            }
            catch (Exception ex)
            {
                //Aqui hacemos el llamado de la Función que guardar los Logs de Errores que suceden en el sistema
                FixedData.LogApi.Error("Error al pasar la Imagen a Binario. Motivo: " + ex.Message.ToString().Trim());
            }

            return Arreglo;
        }

        //--METODO ETL PARA EXTRAER DATOS DEL ARCHIVO MEDIANTE UN SEPARADOR
        public DataTable GetEtl(string _ManejaCampoTitulo, int _IniciaCampoDetalle, string strPathFile, char[] _Separador, ref string _MsgError)
        {
            DataTable dtEtl = new DataTable();
            dtEtl.TableName = "DtDatos";
            int _ContadorFilas = 0;
            try
            {
                StreamReader sr = new StreamReader(strPathFile);
                string[] headers = sr.ReadLine().Split(_Separador, StringSplitOptions.RemoveEmptyEntries);
                int ContadorRow = 0;

                if (_ManejaCampoTitulo.Trim().Equals("S"))
                {
                    //Creamos el encabezado del DataTable
                    foreach (string dtColumn in headers)
                    {
                        //dtEtl.Columns.Add(dtColumn.ToString().Trim().Replace("\"", ""));
                        dtEtl.Columns.Add(this.GetLimpiarCadena(dtColumn.ToString().Trim()).Replace(" ", "_").Replace("  ", "_"));
                    }
                }
                else
                {
                    ContadorRow = _IniciaCampoDetalle;
                }

                //Insertamos los datos al Datatable
                string[] csvRows = File.ReadAllLines(strPathFile);
                string[] fields = null;
                foreach (string csvRow in csvRows)
                {
                    try
                    {
                        //fields = csvRow.Split(_Separador, StringSplitOptions.RemoveEmptyEntries);
                        fields = csvRow.Split(_Separador);
                        if (fields.Length > 0)
                        {
                            //--AQUI VALIDAMOS QUE CODIGO DE OFICINA Y # DE CUENTA VENGAN LLENOS
                            //if (fields[0].ToString().Trim().Length > 0 && fields[1].ToString().Trim().Length > 0)
                            //{
                            if (ContadorRow != 0)
                            {
                                try
                                {
                                    DataRow ItemRow = dtEtl.NewRow();
                                    ItemRow.ItemArray = fields;
                                    dtEtl.Rows.Add(ItemRow);
                                    _ContadorFilas++;
                                }
                                catch (Exception ex)
                                {
                                    _MsgError = "1. Error al obtener los datos de la fila [" + _ContadorFilas + "] del archivo en el proceso de ETL. Motivo: " + ex.Message;
                                    FixedData.LogApi.Error(_MsgError);
                                    return dtEtl;
                                }
                            }
                            //}
                            ContadorRow++;
                        }
                    }
                    catch (Exception ex)
                    {
                        _MsgError = "2. Error al realizar el proceso ETL. Motivo: " + ex.Message;
                        FixedData.LogApi.Error(_MsgError);
                        return dtEtl;
                    }
                }

                _MsgError = "";
                sr.Close();
            }
            catch (Exception ex)
            {
                dtEtl = null;
                _MsgError = "3. Error al realizar el proceso ETL. Motivo: " + ex.Message;
                FixedData.LogApi.Error(_MsgError);
            }

            return dtEtl;
        }

        //--METODO ETL PARA EXTRAER DATOS DEL ARCHIVO MEDIANTE LINEA DEL ARCHIVO
        public DataTable GetEtl_LongCampo(string _ManejaCampoTitulo, string _NombreTabla, int _IniciaCampoDetalle, string strPathFile, ref string _MsgError)
        {
            DataTable dtEtl = new DataTable();
            dtEtl.TableName = "DtDatos";
            int _ContadorFilas = 0;
            try
            {
                //--AQUI VALIDAMOS EL TIPO DE TABLA
                if (_NombreTabla.Trim().Equals("TARJETA_CREDITO"))
                {
                    #region PASO 1: AQUI DEFINIMOS LA COLUMNAS DEL DATATABLE TARJETA_CREDITO
                    //Creamos el DataTable donde se almacenaran las Facturas a Pagar.
                    dtEtl = new DataTable();
                    dtEtl.Columns.Add("id_registro", typeof(Int32));
                    dtEtl.PrimaryKey = new DataColumn[] { dtEtl.Columns["id_registro"] };
                    dtEtl.Columns.Add("tipo");
                    dtEtl.Columns.Add("impuesto");
                    dtEtl.Columns.Add("cod_ciu");
                    dtEtl.Columns.Add("ciudad");
                    dtEtl.Columns.Add("nit");
                    dtEtl.Columns.Add("tm");
                    dtEtl.Columns.Add("marca");
                    dtEtl.Columns.Add("fecha_inicial");
                    dtEtl.Columns.Add("fecha_final");
                    dtEtl.Columns.Add("valor_venta");
                    dtEtl.Columns.Add("establecimiento");
                    dtEtl.Columns.Add("valor_impuesto");
                    dtEtl.Columns.Add("valor_base");
                    #endregion
                }
                else if (_NombreTabla.Trim().Equals("INFO_CONTABLE"))
                {
                    #region PASO 1: AQUI DEFINIMOS LA COLUMNAS DEL DATATABLE INFO_CONTABLE
                    //Creamos el DataTable donde se almacenaran las Facturas a Pagar.
                    dtEtl = new DataTable();
                    dtEtl.Columns.Add("ID_REGISTRO", typeof(Int32));
                    dtEtl.PrimaryKey = new DataColumn[] { dtEtl.Columns["ID_REGISTRO"] };
                    dtEtl.Columns.Add("UN");
                    dtEtl.Columns.Add("G_LIBROS");
                    dtEtl.Columns.Add("LIBRO");
                    dtEtl.Columns.Add("CUENTA");
                    dtEtl.Columns.Add("SUCURSAL");
                    dtEtl.Columns.Add("DEPENDENCIA");
                    dtEtl.Columns.Add("ID_DE_ASIENTO");
                    dtEtl.Columns.Add("FECHA_COMPROBANTE");
                    dtEtl.Columns.Add("FECHA_PROCESO");
                    dtEtl.Columns.Add("DESCRIPCION");
                    dtEtl.Columns.Add("DEBITO");
                    dtEtl.Columns.Add("CREDITO");
                    dtEtl.Columns.Add("AUXILIAR");
                    dtEtl.Columns.Add("REFERENCIA");
                    dtEtl.Columns.Add("USUARIO");
                    dtEtl.Columns.Add("ID_COMPROBANTE");
                    dtEtl.Columns.Add("ESTADO");
                    dtEtl.Columns.Add("REAL");
                    #endregion
                }

                //--AQUI COMENZAMOS A LEER EL ARCHIVO
                StreamReader sr = new StreamReader(strPathFile);
                string line = sr.ReadLine();
                //Continue to read until you reach end of file
                while (line != null)
                {
                    //--AQUI VALIDAMOS QUE LAS 2 PRIMERAS POSICIONES DE LA LINEA SEA 06 QUE INDICA EL DETALLE
                    if (_ManejaCampoTitulo.Trim().Equals("N"))
                    {
                        _ContadorFilas++;
                        //--AQUI VALIDAMOS EL TIPO DE TABLA
                        if (_NombreTabla.Trim().Equals("TARJETA_CREDITO"))
                        {
                            int _LogLinea = line.ToString().Trim().Length;
                            if (_LogLinea == 199)
                            {
                                #region AQUI OBTENEMOS LOS DATOS DE CADA LINEA DEL ARCHIVO
                                //--
                                string _Tipo = line.ToString().Trim().Substring(0, 3);
                                string _Impuesto = line.ToString().Trim().Substring(4, 9);
                                string _CodCiu = line.ToString().Trim().Substring(13, 7);
                                string _Ciudad = line.ToString().Trim().Substring(20, 14);
                                string _Nit = line.ToString().Trim().Substring(34, 27);
                                string _Tm = line.ToString().Trim().Substring(61, 2);
                                string _Marca = line.ToString().Trim().Substring(63, 19);
                                string _FechaInicial = line.ToString().Trim().Substring(82, 11);
                                string _FechaFinal = line.ToString().Trim().Substring(93, 11);
                                string _ValorVenta = line.ToString().Trim().Substring(104, 15);
                                string _Establecimiento = line.ToString().Trim().Substring(119, 38);
                                string _ValorImpuesto = line.ToString().Trim().Substring(157, 15);
                                string _ValorBase1 = line.ToString().Trim().Substring(172, 14);
                                string _DecimalesValorBase = line.ToString().Trim().Substring(186, 3);
                                string _ValorBase = _ValorBase1 + "." + _DecimalesValorBase;
                                //--
                                DataRow Fila = null;
                                Fila = dtEtl.NewRow();
                                Fila["id_registro"] = dtEtl.Rows.Count + 1;
                                Fila["tipo"] = _Tipo.ToString().Trim();
                                Fila["impuesto"] = _Impuesto.ToString().Trim();
                                Fila["cod_ciu"] = _CodCiu.ToString().Trim();
                                Fila["ciudad"] = _Ciudad.ToString().Trim();
                                Fila["nit"] = _Nit.ToString().Trim();
                                Fila["tm"] = _Tm.ToString().Trim();
                                Fila["marca"] = _Marca.ToString().Trim();
                                Fila["fecha_inicial"] = _FechaInicial.ToString().Trim().Substring(0, 4) + "-" + _FechaInicial.ToString().Trim().Substring(4, 2) + "-" + _FechaInicial.ToString().Trim().Substring(6, 2);
                                Fila["fecha_final"] = _FechaFinal.ToString().Trim().Substring(0, 4) + "-" + _FechaFinal.ToString().Trim().Substring(4, 2) + "-" + _FechaFinal.ToString().Trim().Substring(6, 2);
                                Fila["valor_venta"] = _ValorVenta.ToString().Trim();
                                Fila["establecimiento"] = _Establecimiento.ToString().Trim();
                                Fila["valor_impuesto"] = _ValorImpuesto.ToString().Trim();
                                Fila["valor_base"] = _ValorBase.ToString().Trim();
                                dtEtl.Rows.Add(Fila);
                                line = sr.ReadLine();
                                #endregion
                            }
                            else
                            {
                                line = sr.ReadLine();
                            }
                        }
                        else if (_NombreTabla.Trim().Equals("INFO_CONTABLE"))
                        {
                            if (_ContadorFilas >= _IniciaCampoDetalle)
                            {
                                int _LogLinea = line.ToString().Trim().Length;
                                if(_LogLinea == 239)
                                {
                                    #region AQUI OBTENEMOS LOS DATOS DE CADA LINEA DEL ARCHIVO
                                    //--
                                    string _UN = "NA";
                                    string _GLIBROS = "NA";
                                    string _LIBRO = "NA";
                                    string _CUENTA = line.ToString().Trim().Substring(7, 12);
                                    string _SUCURSAL = line.ToString().Trim().Substring(19, 4);
                                    string _DEPENDENCIA = line.ToString().Trim().Substring(116, 8);
                                    //--
                                    string _TIPO = line.ToString().Trim().Substring(34, 4).ToString().Trim();
                                    string _DEPCOMP = line.ToString().Trim().Substring(38, 6).ToString().Trim();
                                    string _NRCOMP = line.ToString().Trim().Substring(44, 6).ToString().Trim();
                                    string _IDASIENTO = _TIPO + "" + _DEPCOMP + "" + _NRCOMP;
                                    //--
                                    string _FechaComprobante1 = line.ToString().Trim().Substring(23, 11);
                                    DateTime _FechaComprobante2 = _FechaComprobante1.ToString().Trim().Length > 0 ? Convert.ToDateTime(_FechaComprobante1.ToString()) : DateTime.Now;
                                    string _FECHA_COMPROBANTE = _FechaComprobante2.ToString("yyyy-MM-dd");
                                    //--FECHA DE PROCESO
                                    int _MesFechaComp = (_FechaComprobante2.Month);
                                    DateTime _PrimerDiaDelMes = new DateTime(_FechaComprobante2.Year, _MesFechaComp, 1);
                                    DateTime _UltimoDiaDelMes = _PrimerDiaDelMes.AddMonths(1).AddDays(-1);
                                    string _FECHA_PROCESO = _UltimoDiaDelMes.ToString("yyyy-MM-dd");
                                    //--
                                    string _DESCRIPCION = "NA";
                                    //--EL TIPO DE NATURALEZA (1. DEBITO, 2. CREDITO)
                                    string _TipoNaturaleza = line.ToString().Trim().Substring(124, 16).ToString().Trim();
                                    string _DEBITO = "", _CREDITO = "", _Valor = "0";
                                    if (_TipoNaturaleza.Trim().Equals("1"))
                                    {
                                        _Valor = line.ToString().Trim().Substring(227, 12).ToString().Trim().Replace(",", "");
                                        _DEBITO = _Valor.ToString().Trim();
                                        _CREDITO = "0";
                                    }
                                    else
                                    {
                                        _Valor = line.ToString().Trim().Substring(227, 12).ToString().Trim().Replace(",", "");
                                        _DEBITO = "0";
                                        _CREDITO = _Valor.ToString().Trim();
                                    }
                                    //--
                                    string _AUXILIAR = line.ToString().Trim().Substring(62, 12);
                                    string _REFERENCIA = line.ToString().Trim().Substring(177, 18);
                                    string _USUARIO = "NA";
                                    string _IDCOMPROBANTE = _TIPO + "" + _DEPCOMP + "" + _NRCOMP;
                                    string _ESTADO = "NA";
                                    string _REAL = "X";
                                    //--
                                    DataRow Fila = null;
                                    Fila = dtEtl.NewRow();
                                    Fila["ID_REGISTRO"] = dtEtl.Rows.Count + 1;
                                    Fila["UN"] = _UN.ToString().Trim();
                                    Fila["G_LIBROS"] = _GLIBROS.ToString().Trim();
                                    Fila["LIBRO"] = _LIBRO.ToString().Trim();
                                    Fila["CUENTA"] = _CUENTA.ToString().Trim();
                                    Fila["SUCURSAL"] = _SUCURSAL.ToString().Trim();
                                    Fila["DEPENDENCIA"] = _DEPENDENCIA.ToString().Trim();
                                    Fila["ID_DE_ASIENTO"] = _IDASIENTO.ToString().Trim();
                                    Fila["FECHA_COMPROBANTE"] = _FECHA_COMPROBANTE.ToString().Trim();
                                    Fila["FECHA_PROCESO"] = _FECHA_PROCESO.ToString().Trim();
                                    Fila["DESCRIPCION"] = _DESCRIPCION.ToString().Trim();
                                    Fila["DEBITO"] = _DEBITO.ToString().Trim();
                                    Fila["CREDITO"] = _CREDITO.ToString().Trim();
                                    Fila["AUXILIAR"] = _AUXILIAR.ToString().Trim();
                                    Fila["REFERENCIA"] = _REFERENCIA.ToString().Trim();
                                    Fila["USUARIO"] = _USUARIO.ToString().Trim();
                                    Fila["ID_COMPROBANTE"] = _IDCOMPROBANTE.ToString().Trim();
                                    Fila["ESTADO"] = _ESTADO;
                                    Fila["REAL"] = _REAL;
                                    dtEtl.Rows.Add(Fila);
                                    line = sr.ReadLine();
                                    #endregion
                                }
                                else
                                {
                                    line = sr.ReadLine();
                                }
                            }
                            else
                            {
                                line = sr.ReadLine();
                            }
                        }
                    }
                    else
                    {
                        line = sr.ReadLine();
                    }
                }
                _MsgError = "";
                sr.Close();
            }
            catch (Exception ex)
            {
                dtEtl = null;
                _MsgError = "3. Error al realizar el proceso ETL en la linea No. " + _ContadorFilas + " del archivo. Motivo: " + ex.Message;
                FixedData.LogApi.Error(_MsgError);
            }

            return dtEtl;
        }

        //--METODO ETL PARA EXTRAER DATOS DEL ARCHIVO DE EXCEL
        public DataTable GetEtl_Excel(string _NombreTabla, int _IniciaCampoTitulo, int _IniciaCampoDetalle, string strPathFile, ref string _MsgError)
        {
            DataTable dtEtl = new DataTable();
            dtEtl.TableName = "DtDatos";
            try
            {
                #region PASO 1: AQUI DEFINIMOS EL DATATABLE PARA ALMACENAR LOS DATOS
                //--AQUI DEFINIMOS EL DATATABLE
                //Creamos el DataTable donde se almacenaran las Facturas a Pagar.
                dtEtl = new DataTable();
                dtEtl.Columns.Add("ID_REGISTRO", typeof(Int32));
                dtEtl.PrimaryKey = new DataColumn[] { dtEtl.Columns["ID_REGISTRO"] };

                // Cargar archivo de Excel
                Workbook wb = new Workbook(strPathFile.Trim());
                // Obtener todas las hojas de trabajo
                WorksheetCollection collection = wb.Worksheets;

                // Recorra todas las hojas de trabajo
                for (int worksheetIndex = 0; worksheetIndex < collection.Count; worksheetIndex++)
                {
                    // Obtener hoja de trabajo usando su índice
                    Worksheet worksheet = collection[worksheetIndex];
                    // Imprimir el nombre de la hoja de trabajo
                    Console.WriteLine("Worksheet: " + worksheet.Name);

                    // Obtener el número de filas y columnas
                    int rows = worksheet.Cells.MaxDataRow + 1;
                    int cols = worksheet.Cells.MaxDataColumn + 1;

                    // Bucle a través de filas
                    for (int i = 0; i < rows; i++)
                    {
                        DataRow Fila = null;
                        if (i >= _IniciaCampoDetalle)
                        {
                            Fila = dtEtl.NewRow();
                        }

                        // Recorra cada columna en la fila seleccionada
                        for (int j = 0; j < cols; j++)
                        {
                            if (i == _IniciaCampoTitulo)
                            {
                                string _NombreColumna = "";
                                if (_NombreTabla.Trim().Equals("LEASING_HABITACIONAL"))
                                {
                                    if (j == _IniciaCampoTitulo)
                                    {
                                        //--AQUI DEFINIMOS ESTE NOMBRE YA QUE EN EL ARCHIVO SE VE UN GUION (-)
                                        _NombreColumna = "ICA_CONSECUTIVO";
                                    }
                                    else
                                    {
                                        //--AQUI OBTENEMOS EL NOMBRE DE LA COLUMNA
                                        _NombreColumna = this.GetLimpiarCadena(worksheet.Cells[i, j].Value.ToString().Trim().Replace(" ", "_"));
                                    }
                                }
                                else
                                {
                                    _NombreColumna = this.GetLimpiarCadena(worksheet.Cells[i, j].Value.ToString().Trim().Replace(" ", "_"));
                                }
                                dtEtl.Columns.Add(_NombreColumna);
                                Console.Write(worksheet.Cells[i, j].Value.ToString().Trim() + " | ");
                            }
                            else if (i >= _IniciaCampoDetalle)
                            {
                                string _NombreColumna = "";
                                if (_NombreTabla.Trim().Equals("LEASING_HABITACIONAL"))
                                {
                                    if (j == _IniciaCampoTitulo)
                                    {
                                        //--AQUI DEFINIMOS ESTE NOMBRE YA QUE EN EL ARCHIVO SE VE UN GUION (-)
                                        _NombreColumna = "ICA_CONSECUTIVO";
                                    }
                                    else
                                    {
                                        //--AQUI OBTENEMOS EL NOMBRE DE LA COLUMNA
                                        _NombreColumna = this.GetLimpiarCadena(worksheet.Cells[_IniciaCampoTitulo, j].Value.ToString().Trim().Replace(" ", "_"));
                                    }
                                }
                                else
                                {
                                    //--AQUI OBTENEMOS EL NOMBRE DE LA COLUMNA
                                    _NombreColumna = this.GetLimpiarCadena(worksheet.Cells[_IniciaCampoTitulo, j].Value.ToString().Trim().Replace(" ", "_"));
                                }

                                //--
                                Fila["ID_REGISTRO"] = dtEtl.Rows.Count + 1;
                                string _DataExcel1 = worksheet.Cells[i, j].Value != null ? worksheet.Cells[i, j].Value.ToString().Trim() : "NA";
                                string _DataExcel = _DataExcel1.ToString().Trim().Length > 0 ? _DataExcel1.ToString().Trim() : "NA";
                                Fila[_NombreColumna] = _DataExcel;
                                //dtEtl.Rows.Add(Fila);
                                Console.Write(_DataExcel + " | ");
                            }
                        }
                        //--
                        if (Fila != null)
                        {
                            dtEtl.Rows.Add(Fila);
                        }
                        // Salto de línea de impresión
                        Console.WriteLine(" ");
                    }
                }
                _MsgError = "";
                #endregion
            }
            catch (Exception ex)
            {
                dtEtl = null;
                _MsgError = "3. Error al realizar el proceso ETL con el archivo de excel. Motivo: " + ex.Message;
                FixedData.LogApi.Error(_MsgError);
            }

            return dtEtl;
        }

        public string GetLimpiarCadena(string _Cadena)
        {
            string _Result = "";
            try
            {
                _Result = _Cadena.ToString().Trim().ToUpper().Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U").Replace("Ñ", "N").Replace("(", "").Replace(")", "").Replace("*", "").Replace("°", "").Replace("-", " ").Replace(";", "").Replace(".", "").Replace(",", "").Replace("¿", "").Replace("?", "").Replace("[", "").Replace("]", "").Replace("=", "").Replace("&", "").Replace("%", "").Replace("$", "").Replace("#", "").Replace("\"", "").Replace("!", "").Replace("'", "").Replace("/", "").Replace("\"", "").Replace("Nº", "").Replace("�", "");
                //_Result = _Cadena.ToString().Trim().Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U").Replace("Ñ", "N");
            }
            catch (Exception ex)
            {
                _Result = _Cadena;
            }

            return _Result;
        }

    }
}
