using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smartax.Cronjob.Process.Clases.Models;
using Smartax.Cronjob.Process.Clases.Transactions;
using Smartax.Cronjob.Process.Clases.Utilidades;

namespace Smartax.Cronjob.Task.Clases.Transactions
{
    public class ProcessFileDavibox
    {
        public static Functions objFunctions = new Functions();
        public static EnviarEmails ObjEmails = new EnviarEmails();
        const string quote = "\"";
        const string quote2 = "\'";

        public bool ProcesarArchivosDavibox(FileDavibox_Req objFileDavibox)
        {
            bool _Result = false;
            //--INSTANCIAMOS VARIABLES DE OBJETO PARA EL ENVIO DE EMAILS
            ObjEmails.ServerCorreo = FixedData.ServerCorreoGmail.ToString().Trim();
            ObjEmails.PuertoCorreo = FixedData.PuertoCorreoGmail;
            ObjEmails.EmailDe = FixedData.UserCorreoGmail.ToString().Trim();
            ObjEmails.PassEmailDe = FixedData.PassCorreoGmail.ToString().Trim();
            string _NombreFileDavibox = "";
            try
            {
                #region AQUI REALIZAMOS EL PROCESO DE LOS ARCHIVOS DESCARGADO DEL DAVIBOX
                //--OBTENEMOS DE LA DB LOS DATOS DE ARCHIVOS DEL DAVIBOX
                ProcessDb objProcessDb = new ProcessDb();
                objProcessDb.TipoConsulta = 1;
                objProcessDb.IdEstado = 1;

                //--PASO 1: OBTENER DATOS DE ESTADOS FINANCIEROS CARGADOS AL SISTEMA
                DataTable dtFileDavibox = new DataTable();
                string _MsgError = "";
                dtFileDavibox = objProcessDb.GetArchivosDavibox(ref _MsgError);
                //--
                if (dtFileDavibox != null)
                {
                    if (dtFileDavibox.Rows.Count > 0)
                    {
                        #region AQUI RECORREMOS LOS DATOS DEL DATATABLE
                        //--ESTA VARIABLE ES SOLO PARA EL PROCESO DE CARGUE DE INFO DE TARJETA DE CREDITO
                        int _ContadorTc = 0;
                        foreach (DataRow rowItemEf in dtFileDavibox.Rows)
                        {
                            string _NombreArchivo = rowItemEf["nombre_archivo"].ToString().Trim();
                            string _ExtensionArchivo = rowItemEf["extension_archivo"].ToString().Trim();
                            string _SeparadorArchivo = rowItemEf["separador_archivo"].ToString().Trim();
                            int _LongtitudNombre = Int32.Parse(rowItemEf["longtitud_nombre"].ToString().Trim());
                            string _NombreTabla = rowItemEf["nombre_tabla"].ToString().Trim();
                            string _ManejaCampoTitulo = rowItemEf["maneja_campo_titulo"].ToString().Trim();
                            int _IniciaCampoTitulo = Int32.Parse(rowItemEf["inicia_campo_titulo"].ToString().Trim());
                            int _IniciaCampoDetalle = Int32.Parse(rowItemEf["inicia_campo_detalle"].ToString().Trim());
                            //--AQUI DEFINIMOS EL NOMBRE DEL ARCHIVO DESCARGADO DEL DAVIBOX
                            _NombreFileDavibox = _NombreArchivo + objFileDavibox.mes_procesar + "_" + objFileDavibox.anio_gravable + "." + _ExtensionArchivo;
                            string _PathFilesDavibox = FixedData.PathFileDavibox + "\\" + _NombreFileDavibox;
                            //--AQUI VALIDAMOS SI EL ARCHIVO EXISTE EN LA RUTA
                            if (File.Exists(_PathFilesDavibox))
                            {
                                //--VALIDAMOS EL SEPARADOR DEL ARCHIVO Y CARGA ETL
                                #region AQUI INICIAMOS EL PROCESO DE CARGA DEL ARCHIVO
                                char[] _TipoSeparador = null;
                                _MsgError = "";
                                DataTable dtEtl = new DataTable();
                                //--
                                if (_SeparadorArchivo.ToString().Trim().Equals("TAB"))
                                {
                                    _TipoSeparador = new char[] { '\t' };
                                }
                                else if (_SeparadorArchivo.ToString().Trim().Equals("ESP"))
                                {
                                    _TipoSeparador = new char[] { ' ' };
                                    //--
                                    dtEtl = objFunctions.GetEtl_LongCampo(_ManejaCampoTitulo, _IniciaCampoDetalle, _PathFilesDavibox, _TipoSeparador, ref _MsgError);
                                }
                                else
                                {
                                    _TipoSeparador = new char[] { char.Parse(_SeparadorArchivo.ToString().Trim()) };
                                    //--
                                    dtEtl = objFunctions.GetEtl(_ManejaCampoTitulo, _IniciaCampoDetalle, _PathFilesDavibox, _TipoSeparador, ref _MsgError);
                                }

                                //--
                                if (dtEtl != null)
                                {
                                    if (dtEtl.Rows.Count > 0)
                                    {
                                        #region VALIDAMOS LA TABLA DONDE SE VA A CARGAR LA INFORMACION EN LA DB
                                        string _ArrayData = "", _ErrorProcesar = "N";
                                        int _CantidadTotalReg = 0, _CantidadReg = 0, _CantidadLoteProcesado = 0;
                                        //--
                                        switch (_NombreTabla)
                                        {
                                            case "LEASING_FINANCIERO":
                                                #region CARGAR INFO A LA TABLA LEASING_FINANCIERO
                                                //--AQUI REALIZAMOS EL BORRADO DE LOS DATOS DE LA TABLA
                                                objProcessDb.TipoProceso = 1;
                                                objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();

                                                string _CodError = "";
                                                _MsgError = "";
                                                if (objProcessDb.DlInfoData(ref _CodError, ref _MsgError))
                                                {
                                                    //--AQUI RECORREMOS LOS DATOS DEL DATATABLE
                                                    foreach (DataRow rowItemEtl in dtEtl.Rows)
                                                    {
                                                        #region AQUI OBTENEMOS LOS DATOS DEL DATATABLE
                                                        //--
                                                        string TIPO_IDENT = rowItemEtl["TIPO_IDENT"].ToString().Trim().Replace("\"", "");
                                                        string NUMERO_IDENT = rowItemEtl["NUMERO_IDENT"].ToString().Trim();
                                                        string TIPO_IMP = objFunctions.GetLimpiarCadena(rowItemEtl["TIPO_IMP"].ToString().Trim());
                                                        string SUBTIPO_IMP = objFunctions.GetLimpiarCadena(rowItemEtl["SUBTIPO_IMP"].ToString().Trim());
                                                        double TARIFA = rowItemEtl["TARIFA"].ToString().Trim().Length > 0 ? Double.Parse(rowItemEtl["TARIFA"].ToString().Trim().Replace(",", ".")) : 0;
                                                        double VALOR = rowItemEtl["VALOR"].ToString().Trim().Length > 0 ? Double.Parse(rowItemEtl["VALOR"].ToString().Trim().Replace(",", ".")) : 0;
                                                        string NIT_TESO1 = rowItemEtl["NIT_TESO"].ToString().Trim().Length > 0 ? rowItemEtl["NIT_TESO"].ToString().Trim() : "NA";
                                                        string NIT_TESO = objFunctions.GetLimpiarCadena(NIT_TESO1);
                                                        string NOM_TESO1 = rowItemEtl["NOM_TESO"].ToString().Trim().Length > 0 ? rowItemEtl["NOM_TESO"].ToString().Trim() : "NA";
                                                        string NOM_TESO = objFunctions.GetLimpiarCadena(NOM_TESO1);
                                                        string NUM_CUE = rowItemEtl["NUM_CUE"].ToString().Trim();
                                                        string NATURALEZA = rowItemEtl["NATURALEZA"].ToString().Trim();
                                                        string TIPO_COMPRO = rowItemEtl["TIPO_COMPRO"].ToString().Trim();
                                                        string NUMERO_COMPRO = rowItemEtl["NUMERO_COMPRO"].ToString().Trim();
                                                        string NOMBRE_TERCERO1 = rowItemEtl["NOMBRE_TERCERO"].ToString().Trim().Length > 0 ? rowItemEtl["NOMBRE_TERCERO"].ToString().Trim() : "NA";
                                                        string NOMBRE_TERCERO = objFunctions.GetLimpiarCadena(NOMBRE_TERCERO1);
                                                        double BASE = rowItemEtl["BASE"].ToString().Trim().Length > 0 ? Double.Parse(rowItemEtl["BASE"].ToString().Trim().Replace(",", ".")) : 0;
                                                        string FECHA_REGISTRO = Convert.ToDateTime(rowItemEtl["FECHA_REGISTRO"].ToString().Trim()).ToString("yyyy-MM-dd");
                                                        string DIRECCION_TERCERO1 = rowItemEtl["DIRECCION_TERCERO"].ToString().Trim().Length > 0 ? rowItemEtl["DIRECCION_TERCERO"].ToString().Trim() : "NA";
                                                        string DIRECCION_TERCERO = objFunctions.GetLimpiarCadena(DIRECCION_TERCERO1);
                                                        string CORREO_ELECTRONICO1 = rowItemEtl["CORREO_ELECTRONICO"].ToString().Trim().Length > 0 ? rowItemEtl["CORREO_ELECTRONICO"].ToString().Trim() : "NA";
                                                        string CORREO_ELECTRONICO = CORREO_ELECTRONICO1;
                                                        string DEPARTAMENTO_TERCERO = rowItemEtl["DEPARTAMENTO_TERCERO"].ToString().Trim();
                                                        string CIUDAD_TERCERO = rowItemEtl["CIUDAD_TERCERO"].ToString().Trim();
                                                        string PAIS_TERCERO = rowItemEtl["PAIS_TERCERO"].ToString().Trim();
                                                        string TELEFONO_TERCERO1 = rowItemEtl["TELEFONO_TERCERO"].ToString().Trim().Length > 0 ? rowItemEtl["TELEFONO_TERCERO"].ToString().Trim() : "NA";
                                                        string TELEFONO_TERCERO = TELEFONO_TERCERO1.Trim().Replace("\"", "");

                                                        //--AQUI CONCATENAMOS LOS VALORES DEL ESTADO FINANCIERO
                                                        if (_ArrayData.ToString().Trim().Length > 0)
                                                        {
                                                            _ArrayData = _ArrayData.ToString().Trim() + "," + quote + "(" + TIPO_IDENT + "," + NUMERO_IDENT + "," + TIPO_IMP + "," + SUBTIPO_IMP + "," + TARIFA + "," + VALOR + "," + NIT_TESO + "," + NOM_TESO + "," + NUM_CUE + "," + NATURALEZA + "," + TIPO_COMPRO + "," + NUMERO_COMPRO + "," + NOMBRE_TERCERO + "," + BASE + "," + FECHA_REGISTRO + "," + DIRECCION_TERCERO + "," + CORREO_ELECTRONICO + "," + DEPARTAMENTO_TERCERO + "," + CIUDAD_TERCERO + "," + PAIS_TERCERO + "," + TELEFONO_TERCERO + ")" + quote;
                                                        }
                                                        else
                                                        {
                                                            _ArrayData = quote + "(" + TIPO_IDENT + "," + NUMERO_IDENT + "," + TIPO_IMP + "," + SUBTIPO_IMP + "," + TARIFA + "," + VALOR + "," + NIT_TESO + "," + NOM_TESO + "," + NUM_CUE + "," + NATURALEZA + "," + TIPO_COMPRO + "," + NUMERO_COMPRO + "," + NOMBRE_TERCERO + "," + BASE + "," + FECHA_REGISTRO + "," + DIRECCION_TERCERO + "," + CORREO_ELECTRONICO + "," + DEPARTAMENTO_TERCERO + "," + CIUDAD_TERCERO + "," + PAIS_TERCERO + "," + TELEFONO_TERCERO + ")" + quote;
                                                        }
                                                        _CantidadReg++;
                                                        _CantidadTotalReg++;

                                                        //--AQUI VALIDAMOS LA CANTIDAD DE REGISTROS LEIDOS PARA CARGAR
                                                        if (FixedData.CantidadRegProcesar == _CantidadReg)
                                                        {
                                                            #region AQUI ENVIAMOS A CARGAR LOS DATOS EN LA DB
                                                            //--
                                                            objProcessDb.TipoProceso = 1;
                                                            objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                            objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();
                                                            objProcessDb.ArrayDataLf = _ArrayData.ToString().Trim();
                                                            objProcessDb.ArrayDataLh = null;
                                                            objProcessDb.ArrayDataPg = null;
                                                            objProcessDb.ArrayDataTc = null;
                                                            objProcessDb.ArrayDataIc = null;
                                                            objProcessDb.NombreArchivo = _NombreFileDavibox.ToString().Trim();
                                                            objProcessDb.IdEstado = 1;
                                                            objProcessDb.IdUsuario = 1;

                                                            int _IdRegistro = 0;
                                                            _CodError = ""; _MsgError = "";
                                                            if (objProcessDb.AddLoadFileDavibox(ref _IdRegistro, ref _CodError, ref _MsgError))
                                                            {
                                                                _ArrayData = "";
                                                                _CantidadReg = 0;
                                                                _CantidadLoteProcesado++;
                                                                _ErrorProcesar = "N";
                                                            }
                                                            else
                                                            {
                                                                _ErrorProcesar = "S";
                                                                FixedData.LogApi.Error(_MsgError);
                                                                break;
                                                            }
                                                            #endregion
                                                        }
                                                        #endregion
                                                    }

                                                    #region AQUI SE REALIZA EL CARGUE DEL RESTO DE LOS REGISTROS DEL ULTIMO LOTE
                                                    if (_ArrayData.ToString().Trim().Length > 0)
                                                    {
                                                        if (_ErrorProcesar.Equals("N"))
                                                        {
                                                            #region AQUI ENVIAMOS A CARGAR LOS DATOS EN LA DB
                                                            //--
                                                            objProcessDb.TipoProceso = 1;
                                                            objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                            objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();
                                                            objProcessDb.ArrayDataLf = _ArrayData.ToString().Trim();
                                                            objProcessDb.ArrayDataLh = null;
                                                            objProcessDb.ArrayDataPg = null;
                                                            objProcessDb.ArrayDataTc = null;
                                                            objProcessDb.ArrayDataIc = null;
                                                            objProcessDb.NombreArchivo = _NombreFileDavibox.ToString().Trim();
                                                            objProcessDb.IdEstado = 1;
                                                            objProcessDb.IdUsuario = 1;

                                                            int _IdRegistro = 0;
                                                            _CodError = ""; _MsgError = "";
                                                            if (objProcessDb.AddLoadFileDavibox(ref _IdRegistro, ref _CodError, ref _MsgError))
                                                            {
                                                                _ArrayData = "";
                                                                _CantidadReg = 0;
                                                                _CantidadLoteProcesado++;
                                                                _ErrorProcesar = "N";
                                                            }
                                                            else
                                                            {
                                                                _ErrorProcesar = "S";
                                                                FixedData.LogApi.Error(_MsgError);
                                                                break;
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                    #endregion
                                                    break;
                                                }
                                                else
                                                {
                                                    _ErrorProcesar = "S";
                                                    FixedData.LogApi.Error(_MsgError);
                                                    break;
                                                }
                                            //--
                                            #endregion

                                            case "LEASING_HABITACIONAL":
                                                #region CARGAR INFO A LA TABLA LEASING_HABITACIONAL
                                                #endregion
                                                break;

                                            case "PAGADURIA":
                                                #region CARGAR INFO A LA TABLA PAGADURIA
                                                //--AQUI REALIZAMOS EL BORRADO DE LOS DATOS DE LA TABLA
                                                objProcessDb.TipoProceso = 3;
                                                objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();

                                                _CodError = "";
                                                _MsgError = "";
                                                if (objProcessDb.DlInfoData(ref _CodError, ref _MsgError))
                                                {
                                                    //--AQUI RECORREMOS LOS DATOS DEL DATATABLE
                                                    foreach (DataRow rowItemEtl in dtEtl.Rows)
                                                    {
                                                        #region AQUI OBTENEMOS LOS DATOS DEL DATATABLE
                                                        //--
                                                        string UNIDAD_NEGOCIO = rowItemEtl["UNIDAD_NEGOCIO"].ToString().Trim().Replace("\"", "");
                                                        string ID_COMPROBANTE = rowItemEtl["ID_COMPROBANTE"].ToString().Trim();
                                                        double N_LNEA_COMPROBANTE = rowItemEtl["N_LNEA_COMPROBANTE"].ToString().Trim().Length > 0 ? Double.Parse(rowItemEtl["N_LNEA_COMPROBANTE"].ToString().Trim()) : 0;
                                                        string ID_SET_PROVEEDOR = rowItemEtl["ID_SET_PROVEEDOR"].ToString().Trim();
                                                        string ID_PROVEEDOR = rowItemEtl["ID_PROVEEDOR"].ToString().Trim();
                                                        string NMERO_DE_IDENTIFICACIN = rowItemEtl["NMERO_DE_IDENTIFICACIN"].ToString().Trim().Length > 0 ? rowItemEtl["NMERO_DE_IDENTIFICACIN"].ToString().Trim() : "NA";
                                                        string FECHA_CONTABLE = Convert.ToDateTime(rowItemEtl["FECHA_CONTABLE"].ToString().Trim()).ToString("yyyy-MM-dd");
                                                        string TIPO_DE_IMPUESTO = rowItemEtl["TIPO_DE_IMPUESTO"].ToString().Trim().Length > 0 ? rowItemEtl["TIPO_DE_IMPUESTO"].ToString().Trim() : "NA";
                                                        string CDIGO_DE_ACTIVIDAD = rowItemEtl["CDIGO_DE_ACTIVIDAD"].ToString().Trim().Length > 0 ? rowItemEtl["CDIGO_DE_ACTIVIDAD"].ToString().Trim() : "NA";
                                                        //--
                                                        string IMPORTE_BASE_RETENCIN1 = rowItemEtl["IMPORTE_BASE_RETENCIN"].ToString().Trim().Length > 0 ? rowItemEtl["IMPORTE_BASE_RETENCIN"].ToString().Trim().Replace("*", "0") : "0";
                                                        double IMPORTE_BASE_RETENCIN = Double.Parse(IMPORTE_BASE_RETENCIN1);
                                                        //--
                                                        string BASE_RETENCIN_MONEDA_BASE1 = rowItemEtl["BASE_RETENCIN_MONEDA_BASE"].ToString().Trim().Length > 0 ? rowItemEtl["BASE_RETENCIN_MONEDA_BASE"].ToString().Trim().Replace("*", "0") : "0";
                                                        double BASE_RETENCIN_MONEDA_BASE = Double.Parse(BASE_RETENCIN_MONEDA_BASE1);
                                                        //--
                                                        string IMPORTE_RETENCIN1 = rowItemEtl["IMPORTE_RETENCIN"].ToString().Trim().Length > 0 ? rowItemEtl["IMPORTE_RETENCIN"].ToString().Trim().Replace("*", "0") : "0";
                                                        double IMPORTE_RETENCIN = Double.Parse(IMPORTE_RETENCIN1);
                                                        //--
                                                        string IMPORTE_RETENCIN_MONEDA_BASE1 = rowItemEtl["IMPORTE_RETENCIN_MONEDA_BASE"].ToString().Trim().Length > 0 ? rowItemEtl["IMPORTE_RETENCIN_MONEDA_BASE"].ToString().Trim().Replace("*", "0") : "0";
                                                        double IMPORTE_RETENCIN_MONEDA_BASE = Double.Parse(IMPORTE_RETENCIN_MONEDA_BASE1);
                                                        //--
                                                        string PORCENTAJE_DE_RETENCIN1 = rowItemEtl["PORCENTAJE_DE_RETENCIN"].ToString().Trim().Length > 0 ? rowItemEtl["PORCENTAJE_DE_RETENCIN"].ToString().Trim().Replace("*", "0") : "0";
                                                        double PORCENTAJE_DE_RETENCIN = Double.Parse(PORCENTAJE_DE_RETENCIN1);
                                                        //--
                                                        string MONEDA_TRANSACCIN = rowItemEtl["MONEDA_TRANSACCIN"].ToString().Trim().Length > 0 ? rowItemEtl["MONEDA_TRANSACCIN"].ToString().Trim() : "NA";
                                                        string MONEDA_BASE = rowItemEtl["MONEDA_BASE"].ToString().Trim().Length > 0 ? rowItemEtl["MONEDA_BASE"].ToString().Trim() : "NA";

                                                        //--AQUI CONCATENAMOS LOS VALORES DEL ESTADO FINANCIERO
                                                        if (_ArrayData.ToString().Trim().Length > 0)
                                                        {
                                                            _ArrayData = _ArrayData.ToString().Trim() + "," + quote + "(" + UNIDAD_NEGOCIO + "," + ID_COMPROBANTE + "," + N_LNEA_COMPROBANTE + "," + ID_SET_PROVEEDOR + "," + ID_PROVEEDOR + "," + NMERO_DE_IDENTIFICACIN + "," + FECHA_CONTABLE + "," + TIPO_DE_IMPUESTO + "," + CDIGO_DE_ACTIVIDAD + "," + IMPORTE_BASE_RETENCIN + "," + BASE_RETENCIN_MONEDA_BASE + "," + IMPORTE_RETENCIN + "," + IMPORTE_RETENCIN_MONEDA_BASE + "," + PORCENTAJE_DE_RETENCIN + "," + MONEDA_TRANSACCIN + "," + MONEDA_BASE + ")" + quote;
                                                        }
                                                        else
                                                        {
                                                            _ArrayData = quote + "(" + UNIDAD_NEGOCIO + "," + ID_COMPROBANTE + "," + N_LNEA_COMPROBANTE + "," + ID_SET_PROVEEDOR + "," + ID_PROVEEDOR + "," + NMERO_DE_IDENTIFICACIN + "," + FECHA_CONTABLE + "," + TIPO_DE_IMPUESTO + "," + CDIGO_DE_ACTIVIDAD + "," + IMPORTE_BASE_RETENCIN + "," + BASE_RETENCIN_MONEDA_BASE + "," + IMPORTE_RETENCIN + "," + IMPORTE_RETENCIN_MONEDA_BASE + "," + PORCENTAJE_DE_RETENCIN + "," + MONEDA_TRANSACCIN + "," + MONEDA_BASE + ")" + quote;
                                                        }
                                                        _CantidadReg++;
                                                        _CantidadTotalReg++;

                                                        //--AQUI VALIDAMOS LA CANTIDAD DE REGISTROS LEIDOS PARA CARGAR
                                                        if (FixedData.CantidadRegProcesar == _CantidadReg)
                                                        {
                                                            #region AQUI ENVIAMOS A CARGAR LOS DATOS EN LA DB
                                                            //--
                                                            objProcessDb.TipoProceso = 3;
                                                            objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                            objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();
                                                            objProcessDb.ArrayDataLf = null;
                                                            objProcessDb.ArrayDataLh = null;
                                                            objProcessDb.ArrayDataPg = _ArrayData.ToString().Trim();
                                                            objProcessDb.ArrayDataTc = null;
                                                            objProcessDb.ArrayDataIc = null;
                                                            objProcessDb.NombreArchivo = _NombreFileDavibox.ToString().Trim();
                                                            objProcessDb.IdEstado = 1;
                                                            objProcessDb.IdUsuario = 1;

                                                            int _IdRegistro = 0;
                                                            _CodError = ""; _MsgError = "";
                                                            if (objProcessDb.AddLoadFileDavibox(ref _IdRegistro, ref _CodError, ref _MsgError))
                                                            {
                                                                _ArrayData = "";
                                                                _CantidadReg = 0;
                                                                _CantidadLoteProcesado++;
                                                                _ErrorProcesar = "N";
                                                            }
                                                            else
                                                            {
                                                                _ErrorProcesar = "S";
                                                                FixedData.LogApi.Error(_MsgError);
                                                                break;
                                                            }
                                                            #endregion
                                                        }
                                                        #endregion
                                                    }

                                                    #region AQUI SE REALIZA EL CARGUE DEL RESTO DE LOS REGISTROS DEL ULTIMO LOTE
                                                    if (_ArrayData.ToString().Trim().Length > 0)
                                                    {
                                                        if (_ErrorProcesar.Equals("N"))
                                                        {
                                                            #region AQUI ENVIAMOS A CARGAR LOS DATOS EN LA DB
                                                            //--
                                                            objProcessDb.TipoProceso = 3;
                                                            objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                            objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();
                                                            objProcessDb.ArrayDataLf = null;
                                                            objProcessDb.ArrayDataLh = null;
                                                            objProcessDb.ArrayDataPg = _ArrayData.ToString().Trim();
                                                            objProcessDb.ArrayDataTc = null;
                                                            objProcessDb.ArrayDataIc = null;
                                                            objProcessDb.NombreArchivo = _NombreFileDavibox.ToString().Trim();
                                                            objProcessDb.IdEstado = 1;
                                                            objProcessDb.IdUsuario = 1;

                                                            int _IdRegistro = 0;
                                                            _CodError = ""; _MsgError = "";
                                                            if (objProcessDb.AddLoadFileDavibox(ref _IdRegistro, ref _CodError, ref _MsgError))
                                                            {
                                                                _ArrayData = "";
                                                                _CantidadReg = 0;
                                                                _CantidadLoteProcesado++;
                                                                _ErrorProcesar = "N";
                                                            }
                                                            else
                                                            {
                                                                _ErrorProcesar = "S";
                                                                FixedData.LogApi.Error(_MsgError);
                                                                break;
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                    #endregion
                                                    break;
                                                }
                                                else
                                                {
                                                    FixedData.LogApi.Error(_MsgError);
                                                    break;
                                                }
                                            #endregion

                                            case "TARJETA_CREDITO":
                                                #region CARGAR INFO A LA TABLA TARJETA_CREDITO
                                                //--AQUI REALIZAMOS EL BORRADO DE LOS DATOS DE LA TABLA
                                                objProcessDb.TipoProceso = 4;
                                                //--AQUI VALIDAMOS SI YA EJECUTO EL 1er PROCESO DE LA T.C.
                                                if (_ContadorTc == 0)
                                                {
                                                    objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                    objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();
                                                }
                                                else
                                                {
                                                    objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                    objProcessDb.MesEf = "123";
                                                }

                                                _CodError = "";
                                                _MsgError = "";
                                                if (objProcessDb.DlInfoData(ref _CodError, ref _MsgError))
                                                {
                                                    //--AQUI RECORREMOS LOS DATOS DEL DATATABLE
                                                    foreach (DataRow rowItemEtl in dtEtl.Rows)
                                                    {
                                                        #region AQUI OBTENEMOS LOS DATOS DEL DATATABLE
                                                        //--
                                                        string _Tipo = objFunctions.GetLimpiarCadena(rowItemEtl["tipo"].ToString().Trim());
                                                        string _Impuesto = objFunctions.GetLimpiarCadena(rowItemEtl["impuesto"].ToString().Trim());
                                                        string _CodCiu = rowItemEtl["cod_ciu"].ToString().Trim();
                                                        string _Ciudad = objFunctions.GetLimpiarCadena(rowItemEtl["ciudad"].ToString().Trim());
                                                        string _Nit = rowItemEtl["nit"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["nit"].ToString().Trim()) : "NA";
                                                        int _Tm = rowItemEtl["tm"].ToString().Trim().Length > 0 ? Int32.Parse(rowItemEtl["tm"].ToString().Trim()) : 0;
                                                        string _Marca = rowItemEtl["marca"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["marca"].ToString().Trim()) : "NA";
                                                        string _FechaInicial = Convert.ToDateTime(rowItemEtl["fecha_inicial"].ToString().Trim()).ToString("yyyy-MM-dd");
                                                        string _FechaFinal = Convert.ToDateTime(rowItemEtl["fecha_final"].ToString().Trim()).ToString("yyyy-MM-dd");
                                                        //--
                                                        string _ValorVenta1 = rowItemEtl["valor_venta"].ToString().Trim().Length > 0 ? rowItemEtl["valor_venta"].ToString().Trim().Replace("*", "0") : "0";
                                                        double _ValorVenta = Double.Parse(_ValorVenta1);
                                                        string _Establecimiento = rowItemEtl["establecimiento"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["establecimiento"].ToString().Trim()) : "NA";
                                                        //--
                                                        string _ValorImpuesto1 = rowItemEtl["valor_impuesto"].ToString().Trim().Length > 0 ? rowItemEtl["valor_impuesto"].ToString().Trim().Replace("*", "0") : "0";
                                                        double _ValorImpuesto = Double.Parse(_ValorImpuesto1);
                                                        //--
                                                        string _ValorBase1 = rowItemEtl["valor_base"].ToString().Trim().Length > 0 ? rowItemEtl["valor_base"].ToString().Trim().Replace("*", "0") : "0";
                                                        double _ValorBase = Double.Parse(_ValorBase1);

                                                        //--AQUI CONCATENAMOS LOS VALORES DEL ESTADO FINANCIERO
                                                        if (_ArrayData.ToString().Trim().Length > 0)
                                                        {
                                                            _ArrayData = _ArrayData.ToString().Trim() + "," + quote + "(" + _Tipo + "," + _Impuesto + "," + _CodCiu + "," + _Ciudad + "," + _Nit + "," + _Tm + "," + _Marca + "," + _FechaInicial + "," + _FechaFinal + "," + _ValorVenta + "," + _Establecimiento + "," + _ValorImpuesto + "," + _ValorBase + ")" + quote;
                                                        }
                                                        else
                                                        {
                                                            _ArrayData = quote + "(" + _Tipo + "," + _Impuesto + "," + _CodCiu + "," + _Ciudad + "," + _Nit + "," + _Tm + "," + _Marca + "," + _FechaInicial + "," + _FechaFinal + "," + _ValorVenta + "," + _Establecimiento + "," + _ValorImpuesto + "," + _ValorBase + ")" + quote;
                                                        }
                                                        _CantidadReg++;
                                                        _CantidadTotalReg++;

                                                        //--AQUI VALIDAMOS LA CANTIDAD DE REGISTROS LEIDOS PARA CARGAR
                                                        if (FixedData.CantidadRegProcesar == _CantidadReg)
                                                        {
                                                            #region AQUI ENVIAMOS A CARGAR LOS DATOS EN LA DB
                                                            //--
                                                            objProcessDb.TipoProceso = 4;
                                                            objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                            objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();
                                                            objProcessDb.ArrayDataLf = null;
                                                            objProcessDb.ArrayDataLh = null;
                                                            objProcessDb.ArrayDataPg = null;
                                                            objProcessDb.ArrayDataTc = _ArrayData.ToString().Trim();
                                                            objProcessDb.ArrayDataIc = null;
                                                            objProcessDb.NombreArchivo = _NombreFileDavibox.ToString().Trim();
                                                            objProcessDb.IdEstado = 1;
                                                            objProcessDb.IdUsuario = 1;

                                                            int _IdRegistro = 0;
                                                            _CodError = ""; _MsgError = "";
                                                            if (objProcessDb.AddLoadFileDavibox(ref _IdRegistro, ref _CodError, ref _MsgError))
                                                            {
                                                                _ArrayData = "";
                                                                _CantidadReg = 0;
                                                                _CantidadLoteProcesado++;
                                                                _ErrorProcesar = "N";
                                                            }
                                                            else
                                                            {
                                                                _ErrorProcesar = "S";
                                                                FixedData.LogApi.Error(_MsgError);
                                                                break;
                                                            }
                                                            #endregion
                                                        }
                                                        #endregion
                                                    }

                                                    #region AQUI SE REALIZA EL CARGUE DEL RESTO DE LOS REGISTROS DEL ULTIMO LOTE
                                                    //--
                                                    _ContadorTc++;
                                                    if (_ArrayData.ToString().Trim().Length > 0)
                                                    {
                                                        if (_ErrorProcesar.Equals("N"))
                                                        {
                                                            #region AQUI ENVIAMOS A CARGAR LOS DATOS EN LA DB
                                                            //--
                                                            objProcessDb.TipoProceso = 4;
                                                            objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                            objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();
                                                            objProcessDb.ArrayDataLf = null;
                                                            objProcessDb.ArrayDataLh = null;
                                                            objProcessDb.ArrayDataPg = null;
                                                            objProcessDb.ArrayDataTc = _ArrayData.ToString().Trim();
                                                            objProcessDb.ArrayDataIc = null;
                                                            objProcessDb.NombreArchivo = _NombreFileDavibox.ToString().Trim();
                                                            objProcessDb.IdEstado = 1;
                                                            objProcessDb.IdUsuario = 1;

                                                            int _IdRegistro = 0;
                                                            _CodError = ""; _MsgError = "";
                                                            if (objProcessDb.AddLoadFileDavibox(ref _IdRegistro, ref _CodError, ref _MsgError))
                                                            {
                                                                _ArrayData = "";
                                                                _CantidadReg = 0;
                                                                _CantidadLoteProcesado++;
                                                                _ErrorProcesar = "N";
                                                            }
                                                            else
                                                            {
                                                                _ErrorProcesar = "S";
                                                                FixedData.LogApi.Error(_MsgError);
                                                                break;
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                    #endregion
                                                    break;
                                                }
                                                else
                                                {
                                                    FixedData.LogApi.Error(_MsgError);
                                                    break;
                                                }
                                            #endregion

                                            case "INFO_CONTABLE":
                                                #region CARGAR INFO A LA TABLA INFO_CONTABLE
                                                #endregion
                                                break;

                                            default:
                                                break;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                        _Result = false;
                                        //--
                                        ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                        ObjEmails.Asunto = "REF.: ERROR PROCESO ARCHIVOS DAVIBOX";

                                        string nHora = DateTime.Now.ToString("HH");
                                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                        StringBuilder strDetalleEmail = new StringBuilder();
                                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario no se obtuvo datos con el proceso ETL no cargo información con los archivos descargados del davibox. " + "</h4>" +
                                                                    "<br/><br/>" +
                                                                    "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                                        ObjEmails.Detalle = strDetalleEmail.ToString().Trim();
                                        string _MsgErrorEmail = "";
                                        if (!ObjEmails.SendEmail(ref _MsgErrorEmail))
                                        {
                                            FixedData.LogApi.Error(_MsgErrorEmail);
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                    _Result = false;
                                    //--
                                    ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                    ObjEmails.Asunto = "REF.: ERROR PROCESO ARCHIVOS DAVIBOX";

                                    string nHora = DateTime.Now.ToString("HH");
                                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                    StringBuilder strDetalleEmail = new StringBuilder();
                                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario error con el proceso ETL de los archivos descargados del davibox. " + "</h4>" +
                                                                "<br/><br/>" +
                                                                "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                                    ObjEmails.Detalle = strDetalleEmail.ToString().Trim();
                                    string _MsgErrorEmail = "";
                                    if (!ObjEmails.SendEmail(ref _MsgErrorEmail))
                                    {
                                        FixedData.LogApi.Error(_MsgErrorEmail);
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                                _Result = false;
                                //--
                                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                ObjEmails.Asunto = "REF.: ERROR PROCESO ARCHIVOS DAVIBOX";

                                string nHora = DateTime.Now.ToString("HH");
                                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                                StringBuilder strDetalleEmail = new StringBuilder();
                                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario la ruta del davibox no existe. [ " + _PathFilesDavibox + " ]" + "</h4>" +
                                                            "<br/><br/>" +
                                                            "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                                ObjEmails.Detalle = strDetalleEmail.ToString().Trim();
                                string _MsgErrorEmail = "";
                                if (!ObjEmails.SendEmail(ref _MsgErrorEmail))
                                {
                                    FixedData.LogApi.Error(_MsgErrorEmail);
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                        _Result = false;
                        //--
                        ObjEmails.EmailPara = FixedData.EmailDestinoError;
                        ObjEmails.Asunto = "REF.: ERROR PROCESO ARCHIVOS DAVIBOX";

                        string nHora = DateTime.Now.ToString("HH");
                        string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                        StringBuilder strDetalleEmail = new StringBuilder();
                        strDetalleEmail.Append("<h4>" + strTime + ", señor usuario no se encontro información para procesar los archivos descargados del davibox. " + "</h4>" +
                                                    "<br/><br/>" +
                                                    "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                        ObjEmails.Detalle = strDetalleEmail.ToString().Trim();
                        string _MsgErrorEmail = "";
                        if (!ObjEmails.SendEmail(ref _MsgErrorEmail))
                        {
                            FixedData.LogApi.Error(_MsgErrorEmail);
                        }
                        #endregion
                    }
                }
                else
                {
                    #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                    _Result = false;
                    //--
                    ObjEmails.EmailPara = FixedData.EmailDestinoError;
                    ObjEmails.Asunto = "REF.: ERROR PROCESO ARCHIVOS DAVIBOX";

                    string nHora = DateTime.Now.ToString("HH");
                    string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                    StringBuilder strDetalleEmail = new StringBuilder();
                    strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al obtener los datos de los archivos del davibox. " + "</h4>" +
                                                "<br/><br/>" +
                                                "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                    ObjEmails.Detalle = strDetalleEmail.ToString().Trim();
                    string _MsgErrorEmail = "";
                    if (!ObjEmails.SendEmail(ref _MsgErrorEmail))
                    {
                        FixedData.LogApi.Error(_MsgErrorEmail);
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                #region ENVIAR EMAIL CON EL ERROR OBTENIDO
                _Result = false;
                //--
                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                ObjEmails.Asunto = "REF.: ERROR PROCESO ARCHIVOS DAVIBOX";

                string nHora = DateTime.Now.ToString("HH");
                string strTime = objFunctions.GetTime(Int32.Parse(nHora));
                StringBuilder strDetalleEmail = new StringBuilder();
                strDetalleEmail.Append("<h4>" + strTime + ", señor usuario se produjo un error al realizar el proceso con el archivo del davibox [ " + _NombreFileDavibox + " ] Motivo: " + ex.Message + "</h4>" +
                                            "<br/><br/>" +
                                            "<b>&lt;&lt; Correo Generado Autom&aacute;ticamente. No se reciben respuesta en esta cuenta de correo &gt;&gt;</b>");

                ObjEmails.Detalle = strDetalleEmail.ToString().Trim();
                string _MsgErrorEmail = "";
                if (!ObjEmails.SendEmail(ref _MsgErrorEmail))
                {
                    FixedData.LogApi.Error(_MsgErrorEmail);
                }
                #endregion
            }

            return _Result;
        }

    }
}
