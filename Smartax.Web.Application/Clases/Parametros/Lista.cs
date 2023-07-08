using System;
using System.Data;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;

namespace Smartax.Web.Application.Clases.Parametros
{
    public class Lista
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        private string _MostrarSeleccione;

        public string MostrarSeleccione
        {
            get
            {
                return _MostrarSeleccione;
            }

            set
            {
                _MostrarSeleccione = value;
            }
        }

        //--Metodo para listar los años a partir del 2019
        public DataTable GetAnios()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtAnios";
            try
            {
                TablaDatos.Columns.Add("id_anio");
                TablaDatos.Columns.Add("descripcion_anio");

                //Se le suma + 1 para que le asigne un año mas a la Lista
                int nAnio = Int32.Parse(DateTime.Now.ToString("yyyy")) + 2;
                
                if (MostrarSeleccione.ToString().Trim().Equals("SI"))
                {
                    TablaDatos.Rows.Add("", "<< Seleccione >>");
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

        //--Metodo para listar los niveles de las cuentas Puc
        public DataTable GetNivelCuenta()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtNivelCuenta";
            try
            {
                TablaDatos.Columns.Add("idnivel_cuenta");
                TablaDatos.Columns.Add("nivel_cuenta");

                if (MostrarSeleccione.ToString().Trim().Equals("SI"))
                {
                    TablaDatos.Rows.Add("", "<< Seleccione >>");
                }

                TablaDatos.Rows.Add("1", "NIVEL 1");
                TablaDatos.Rows.Add("2", "NIVEL 2");
                TablaDatos.Rows.Add("3", "NIVEL 3");
                TablaDatos.Rows.Add("4", "NIVEL 4");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de Años. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        //--Metodo para listar los movimientos de las cuentas Puc
        public DataTable GetMovimientoCuenta()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtMovimiento";
            try
            {
                TablaDatos.Columns.Add("id_movimiento");
                TablaDatos.Columns.Add("tipo_movimiento");

                if (MostrarSeleccione.ToString().Trim().Equals("SI"))
                {
                    TablaDatos.Rows.Add("", "<< Seleccione >>");
                }

                TablaDatos.Rows.Add("S", "S");
                TablaDatos.Rows.Add("N", "N");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de Años. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        //--Metodo para listar los tipos de separadores del archivo
        public DataTable GetTipoCaracter()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtSeparadoPor";
            try
            {
                TablaDatos.Columns.Add("id_caracter");
                TablaDatos.Columns.Add("tipo_caracter");

                if (MostrarSeleccione.ToString().Trim().Equals("SI"))
                {
                    TablaDatos.Rows.Add("", "<< Seleccione >>");
                }

                TablaDatos.Rows.Add("1", "PUNTO Y COMA (;)");
                TablaDatos.Rows.Add("2", "PIPE (|)");
                TablaDatos.Rows.Add("3", "TABULADO");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de Años. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        //--Metodo para listar las versiones del estado financiero
        public DataTable GetVersionEf()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtVersionEf";
            try
            {
                TablaDatos.Columns.Add("id_version");
                TablaDatos.Columns.Add("version_ef");

                if (MostrarSeleccione.ToString().Trim().Equals("SI"))
                {
                    TablaDatos.Rows.Add("", "<< Seleccione >>");
                }

                TablaDatos.Rows.Add("VERSION_1", "VERSION_1");
                TablaDatos.Rows.Add("VERSION_2", "VERSION_2");
                TablaDatos.Rows.Add("VERSION_3", "VERSION_3");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de Años. Motivo: " + ex.Message);
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
                TablaDatos.Columns.Add("nombre_mes");

                TablaDatos.Rows.Add("", "<< Seleccione >>");
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
                _log.Error("Error al cargar la lista de meses del año. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetPeriodicidadMensual()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtPeriodicidadPagos";
            try
            {
                TablaDatos.Columns.Add("id_periodicidad");
                TablaDatos.Columns.Add("descripcion_periodicidad");

                TablaDatos.Rows.Add("", "<< Seleccione >>");
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
                TablaDatos.Rows.Add("13", "ANUAL");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de periodicidad mensual. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetPeriodicidadBiMensual()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtPeriodicidadPagos";
            try
            {
                TablaDatos.Columns.Add("id_periodicidad");
                TablaDatos.Columns.Add("descripcion_periodicidad");

                TablaDatos.Rows.Add("", "<< Seleccione >>");
                TablaDatos.Rows.Add("2", "I BIMESTRE");
                TablaDatos.Rows.Add("4", "II BIMESTRE");
                TablaDatos.Rows.Add("6", "III BIMESTRE");
                TablaDatos.Rows.Add("8", "IV BIMESTRE");
                TablaDatos.Rows.Add("10", "V BIMESTRE");
                TablaDatos.Rows.Add("12", "VI BIMESTRE");
                TablaDatos.Rows.Add("13", "ANUAL");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de periodicidad mensual. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetPeriodicidadTrimestral()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtPeriodicidadPagos";
            try
            {
                TablaDatos.Columns.Add("id_periodicidad");
                TablaDatos.Columns.Add("descripcion_periodicidad");

                TablaDatos.Rows.Add("", "<< Seleccione >>");
                TablaDatos.Rows.Add("3", "I TRIMESTRE");
                TablaDatos.Rows.Add("6", "II TRIMESTRE");
                TablaDatos.Rows.Add("9", "III TRIMESTRE");
                TablaDatos.Rows.Add("12", "IV TRIMESTRE");
                TablaDatos.Rows.Add("13", "ANUAL");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de periodicidad trimestral. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetPeriodicidadCuatrimestral()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtPeriodicidadPagos";
            try
            {
                TablaDatos.Columns.Add("id_periodicidad");
                TablaDatos.Columns.Add("descripcion_periodicidad");

                TablaDatos.Rows.Add("", "<< Seleccione >>");
                TablaDatos.Rows.Add("4", "I CUATRIMESTRE");
                TablaDatos.Rows.Add("8", "II CUATRIMESTRE");
                TablaDatos.Rows.Add("12", "III CUATRIMESTRE");
                TablaDatos.Rows.Add("13", "ANUAL");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de periodicidad cuatrimestral. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

        public DataTable GetPeriodicidadSemestral()
        {
            DataTable TablaDatos = new DataTable();
            TablaDatos.TableName = "DtPeriodicidadPagos";
            try
            {
                TablaDatos.Columns.Add("id_periodicidad");
                TablaDatos.Columns.Add("descripcion_periodicidad");

                TablaDatos.Rows.Add("", "<< Seleccione >>");
                TablaDatos.Rows.Add("6", "I SEMESTRE");
                TablaDatos.Rows.Add("12", "II SEMESTRE");
                TablaDatos.Rows.Add("13", "ANUAL");
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar la lista de periodicidad semestral. Motivo: " + ex.Message);
            }

            return TablaDatos;
        }

    }
}