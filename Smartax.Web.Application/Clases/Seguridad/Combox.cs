using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Seguridad
{
    public class Combox
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        public string MostrarSeleccione { get; set; }

        public DataTable GetTipoRptRetencionIca()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtTipoRptRetencionIca";
            try
            {
                TablaDatos.Columns.Add("idrpt_retencionica");
                TablaDatos.Columns.Add("rpt_retencionica");

                if (MostrarSeleccione != null)
                {
                    if (MostrarSeleccione.ToString().Equals("SI"))
                    {
                        TablaDatos.Rows.Add("", "<< Seleccione >>");
                    }
                }
                TablaDatos.Rows.Add("1", "PAGADURIA");
                TablaDatos.Rows.Add("2", "LEASING HABITACIONAL");
                TablaDatos.Rows.Add("3", "LEASING FINANCIERO");
                TablaDatos.Rows.Add("4", "TARJETAS DE CREDITO");
                TablaDatos.Rows.Add("5", "INFORMACION CONTABLE");

            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de reportes de retencion de ica. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetTipoCalculo()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtTipoCalculo";
            try
            {
                TablaDatos.Columns.Add("idtipo_calculo");
                TablaDatos.Columns.Add("tipo_calculo");

                //TablaDatos.Rows.Add("", "<< Seleccione >>");
                TablaDatos.Rows.Add("1", "LEY");
                TablaDatos.Rows.Add("2", "MUNICIPIO");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de horarios. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetDias()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtDias";
            try
            {
                TablaDatos.Columns.Add("id_dia");
                TablaDatos.Columns.Add("numero_dia");

                //TablaDatos.Rows.Add("", "<< Seleccione >>");
                TablaDatos.Rows.Add("01", "01");
                TablaDatos.Rows.Add("02", "02");
                TablaDatos.Rows.Add("03", "03");
                TablaDatos.Rows.Add("04", "04");

                TablaDatos.Rows.Add("05", "05");
                TablaDatos.Rows.Add("06", "06");
                TablaDatos.Rows.Add("07", "07");
                TablaDatos.Rows.Add("08", "08");

                TablaDatos.Rows.Add("09", "09");
                TablaDatos.Rows.Add("10", "10");
                TablaDatos.Rows.Add("11", "11");
                TablaDatos.Rows.Add("12", "12");

                TablaDatos.Rows.Add("13", "13");
                TablaDatos.Rows.Add("14", "14");
                TablaDatos.Rows.Add("15", "15");
                TablaDatos.Rows.Add("16", "16");

                TablaDatos.Rows.Add("17", "17");
                TablaDatos.Rows.Add("18", "18");
                TablaDatos.Rows.Add("19", "19");
                TablaDatos.Rows.Add("20", "20");

                TablaDatos.Rows.Add("21", "21");
                TablaDatos.Rows.Add("22", "22");
                TablaDatos.Rows.Add("23", "23");
                TablaDatos.Rows.Add("24", "24");

                TablaDatos.Rows.Add("25", "25");
                TablaDatos.Rows.Add("26", "26");
                TablaDatos.Rows.Add("27", "27");
                TablaDatos.Rows.Add("28", "28");

                TablaDatos.Rows.Add("29", "29");
                TablaDatos.Rows.Add("30", "30");
                TablaDatos.Rows.Add("31", "31");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de horarios. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetMeses()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtMeses";
            try
            {
                TablaDatos.Columns.Add("id_mes");
                TablaDatos.Columns.Add("numero_mes");

                if (MostrarSeleccione != null)
                {
                    if (MostrarSeleccione.ToString().Equals("SI"))
                    {
                        TablaDatos.Rows.Add("", "<< Seleccione >>");
                    }
                }
                TablaDatos.Rows.Add("01", "ENERO");
                TablaDatos.Rows.Add("02", "FEBRERO");
                TablaDatos.Rows.Add("03", "MARZO");
                TablaDatos.Rows.Add("04", "ABRIL");

                TablaDatos.Rows.Add("05", "MAYO");
                TablaDatos.Rows.Add("06", "JUNIO");
                TablaDatos.Rows.Add("07", "JULIO");
                TablaDatos.Rows.Add("08", "AGOSTO");

                TablaDatos.Rows.Add("09", "SEPTIEMBRE");
                TablaDatos.Rows.Add("10", "OCTUBRE");
                TablaDatos.Rows.Add("11", "NOVIEMBRE");
                TablaDatos.Rows.Add("12", "DICIEMBRE");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de meses. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetAnios()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtAnios";
            try
            {
                TablaDatos.Columns.Add("id_anio");
                TablaDatos.Columns.Add("numero_anio");

                //Se le suma + 1 para que le asigne un año mas a la Lista
                int nAnio = Int32.Parse(DateTime.Now.ToString("yyyy")) + 2;

                if(MostrarSeleccione != null)
                {
                    if (MostrarSeleccione.ToString().Equals("SI"))
                    {
                        TablaDatos.Rows.Add("", "<< Seleccione >>");
                    }
                }

                for (int i = 2021; i <= nAnio; i++)
                {
                    TablaDatos.Rows.Add(i, i);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de Años. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetHorario()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtHorario";
            try
            {
                TablaDatos.Columns.Add("id_horario");
                TablaDatos.Columns.Add("descripcion_horario");

                //TablaDatos.Rows.Add("", "<< Seleccione >>");
                TablaDatos.Rows.Add("00:00", "00:00");
                TablaDatos.Rows.Add("00:15", "00:15");
                TablaDatos.Rows.Add("00:30", "00:30");
                TablaDatos.Rows.Add("00:45", "00:45");

                TablaDatos.Rows.Add("01:00", "01:00");
                TablaDatos.Rows.Add("01:15", "01:15");
                TablaDatos.Rows.Add("01:30", "01:30");
                TablaDatos.Rows.Add("01:45", "01:45");

                TablaDatos.Rows.Add("02:00", "02:00");
                TablaDatos.Rows.Add("02:15", "02:15");
                TablaDatos.Rows.Add("02:30", "02:30");
                TablaDatos.Rows.Add("02:45", "02:45");

                TablaDatos.Rows.Add("03:00", "03:00");
                TablaDatos.Rows.Add("03:15", "03:15");
                TablaDatos.Rows.Add("03:30", "03:30");
                TablaDatos.Rows.Add("03:45", "03:45");

                TablaDatos.Rows.Add("04:00", "04:00");
                TablaDatos.Rows.Add("04:15", "04:15");
                TablaDatos.Rows.Add("04:30", "04:30");
                TablaDatos.Rows.Add("04:45", "04:45");

                TablaDatos.Rows.Add("05:00", "05:00");
                TablaDatos.Rows.Add("05:15", "05:15");
                TablaDatos.Rows.Add("05:30", "05:30");
                TablaDatos.Rows.Add("05:45", "05:45");

                TablaDatos.Rows.Add("06:00", "06:00");
                TablaDatos.Rows.Add("06:15", "06:15");
                TablaDatos.Rows.Add("06:30", "06:30");
                TablaDatos.Rows.Add("06:45", "06:45");

                TablaDatos.Rows.Add("07:00", "07:00");
                TablaDatos.Rows.Add("07:15", "07:15");
                TablaDatos.Rows.Add("07:30", "07:30");
                TablaDatos.Rows.Add("07:45", "07:45");

                TablaDatos.Rows.Add("08:00", "08:00");
                TablaDatos.Rows.Add("08:15", "08:15");
                TablaDatos.Rows.Add("08:30", "08:30");
                TablaDatos.Rows.Add("08:45", "08:45");

                TablaDatos.Rows.Add("09:00", "09:00");
                TablaDatos.Rows.Add("09:15", "09:15");
                TablaDatos.Rows.Add("09:30", "09:30");
                TablaDatos.Rows.Add("09:45", "09:45");

                TablaDatos.Rows.Add("10:00", "10:00");
                TablaDatos.Rows.Add("10:15", "10:15");
                TablaDatos.Rows.Add("10:30", "10:30");
                TablaDatos.Rows.Add("10:45", "10:45");

                TablaDatos.Rows.Add("11:00", "11:00");
                TablaDatos.Rows.Add("11:15", "11:15");
                TablaDatos.Rows.Add("11:30", "11:30");
                TablaDatos.Rows.Add("11:45", "11:45");

                TablaDatos.Rows.Add("12:00", "12:00");
                TablaDatos.Rows.Add("12:15", "12:15");
                TablaDatos.Rows.Add("12:30", "12:30");
                TablaDatos.Rows.Add("12:45", "12:45");

                TablaDatos.Rows.Add("13:00", "13:00");
                TablaDatos.Rows.Add("13:15", "13:15");
                TablaDatos.Rows.Add("13:30", "13:30");
                TablaDatos.Rows.Add("13:45", "13:45");

                TablaDatos.Rows.Add("14:00", "14:00");
                TablaDatos.Rows.Add("14:15", "14:15");
                TablaDatos.Rows.Add("14:30", "14:30");
                TablaDatos.Rows.Add("14:45", "14:45");

                TablaDatos.Rows.Add("15:00", "15:00");
                TablaDatos.Rows.Add("15:15", "15:15");
                TablaDatos.Rows.Add("15:30", "15:30");
                TablaDatos.Rows.Add("15:45", "15:45");

                TablaDatos.Rows.Add("16:00", "16:00");
                TablaDatos.Rows.Add("16:15", "16:15");
                TablaDatos.Rows.Add("16:30", "16:30");
                TablaDatos.Rows.Add("16:45", "16:45");

                TablaDatos.Rows.Add("17:00", "17:00");
                TablaDatos.Rows.Add("17:15", "17:15");
                TablaDatos.Rows.Add("17:30", "17:30");
                TablaDatos.Rows.Add("17:45", "17:45");

                TablaDatos.Rows.Add("18:00", "18:00");
                TablaDatos.Rows.Add("18:15", "18:15");
                TablaDatos.Rows.Add("18:30", "18:30");
                TablaDatos.Rows.Add("18:45", "18:45");

                TablaDatos.Rows.Add("19:00", "19:00");
                TablaDatos.Rows.Add("19:15", "19:15");
                TablaDatos.Rows.Add("19:30", "19:30");
                TablaDatos.Rows.Add("19:45", "19:45");

                TablaDatos.Rows.Add("20:00", "20:00");
                TablaDatos.Rows.Add("20:15", "20:15");
                TablaDatos.Rows.Add("20:30", "20:30");
                TablaDatos.Rows.Add("20:45", "20:45");

                TablaDatos.Rows.Add("21:00", "21:00");
                TablaDatos.Rows.Add("21:15", "21:15");
                TablaDatos.Rows.Add("21:30", "21:30");
                TablaDatos.Rows.Add("21:45", "21:45");

                TablaDatos.Rows.Add("22:00", "22:00");
                TablaDatos.Rows.Add("22:15", "22:15");
                TablaDatos.Rows.Add("22:30", "22:30");
                TablaDatos.Rows.Add("22:45", "22:45");

                TablaDatos.Rows.Add("23:00", "23:00");
                TablaDatos.Rows.Add("23:15", "23:15");
                TablaDatos.Rows.Add("23:30", "23:30");
                TablaDatos.Rows.Add("23:45", "23:45");
                TablaDatos.Rows.Add("23:55", "23:55");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de horarios. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetIntervalo()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtIntervalo";
            try
            {
                TablaDatos.Columns.Add("id_intervalo");
                TablaDatos.Columns.Add("descripcion_intervalo");

                //TablaDatos.Rows.Add("", "<< Seleccione >>");
                TablaDatos.Rows.Add("5", "5");
                TablaDatos.Rows.Add("10", "10");
                TablaDatos.Rows.Add("15", "15");
                TablaDatos.Rows.Add("20", "20");
                TablaDatos.Rows.Add("25", "25");
                TablaDatos.Rows.Add("30", "30");
                TablaDatos.Rows.Add("35", "35");
                TablaDatos.Rows.Add("40", "40");
                TablaDatos.Rows.Add("45", "45");
                TablaDatos.Rows.Add("50", "50");
                TablaDatos.Rows.Add("55", "55");
                TablaDatos.Rows.Add("60", "60");
                TablaDatos.Rows.Add("90", "90");
                TablaDatos.Rows.Add("120", "120");
                TablaDatos.Rows.Add("150", "150");
                TablaDatos.Rows.Add("180", "180");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de intervalos. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetEstadosAprobacion()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtEstadosAprob";
            try
            {
                TablaDatos.Columns.Add("id_aprobacion");
                TablaDatos.Columns.Add("tipo_aprobacion");

                if (MostrarSeleccione != null)
                {
                    if (MostrarSeleccione.ToString().Equals("SI"))
                    {
                        TablaDatos.Rows.Add("", "<< Seleccione >>");
                    }
                }
                TablaDatos.Rows.Add("S", "APROBADO");
                TablaDatos.Rows.Add("N", "RECHAZADA");
                TablaDatos.Rows.Add("P", "PENDIENTE");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de meses. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetAplicativo()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtAplicativo";
            try
            {
                TablaDatos.Columns.Add("id_aplicativo");
                TablaDatos.Columns.Add("aplicativo");

                if (MostrarSeleccione != null)
                {
                    if (MostrarSeleccione.ToString().Equals("SI"))
                    {
                        TablaDatos.Rows.Add("", "<< Seleccione >>");
                    }
                }
                TablaDatos.Rows.Add("1", "CAI");
                TablaDatos.Rows.Add("2", "Leasing Habitacional");
                TablaDatos.Rows.Add("3", "Leasing Financiero");
                TablaDatos.Rows.Add("4", "Tarjeta de Crédito");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de meses. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetTipoPeriodicidad()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtTipoPeriodicidad";
            try
            {
                TablaDatos.Columns.Add("idtipo_periodicidad");
                TablaDatos.Columns.Add("tipo_periodicidad");

                if (MostrarSeleccione != null)
                {
                    if (MostrarSeleccione.ToString().Equals("SI"))
                    {
                        TablaDatos.Rows.Add("", "<< Seleccione >>");
                    }
                }
                TablaDatos.Rows.Add("1", "MENSUAL");
                TablaDatos.Rows.Add("2", "BIMESTRAL");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de meses. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetBimestral()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtBimestral";
            try
            {
                TablaDatos.Columns.Add("id_periodicidad");
                TablaDatos.Columns.Add("periodicidad");

                if (MostrarSeleccione != null)
                {
                    if (MostrarSeleccione.ToString().Equals("SI"))
                    {
                        TablaDatos.Rows.Add("", "<< Seleccione >>");
                    }
                }
                TablaDatos.Rows.Add("1", "1 BIMESTRE");
                TablaDatos.Rows.Add("2", "2 BIMESTRE");
                TablaDatos.Rows.Add("3", "3 BIMESTRE");
                TablaDatos.Rows.Add("4", "4 BIMESTRE");
                TablaDatos.Rows.Add("5", "5 BIMESTRE");
                TablaDatos.Rows.Add("6", "6 BIMESTRE");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de meses. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

    }
}