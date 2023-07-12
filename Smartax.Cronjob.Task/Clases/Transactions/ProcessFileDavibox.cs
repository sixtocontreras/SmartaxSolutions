using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Win32.TaskScheduler;
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
                        DataTable dtFiles = new DataTable();
                        dtFiles.TableName = "DtFiles";
                        dtFiles = new DataTable();
                        dtFiles.Columns.Add("ID_REGISTRO", typeof(Int32));
                        dtFiles.PrimaryKey = new DataColumn[] { dtFiles.Columns["ID_REGISTRO"] };
                        dtFiles.Columns.Add("NOMBRE_ARCHIVO");
                        dtFiles.Columns.Add("CANTIDAD_REGISTROS");

                        int _ContadorTc = 0, _ContadorIc = 0;
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
                                //--AQUI AGREGAMOS LOS DATOS DEL ARCHIVO AL DATATABLE
                                DataRow Fila = null;
                                Fila = dtFiles.NewRow();
                                Fila["ID_REGISTRO"] = dtFiles.Rows.Count + 1;
                                Fila["NOMBRE_ARCHIVO"] = _NombreFileDavibox.ToString().Trim();
                                //dtFiles.Rows.Add(Fila);

                                //--VALIDAMOS EL SEPARADOR DEL ARCHIVO Y CARGA ETL
                                #region AQUI INICIAMOS EL PROCESO DE CARGA DEL ARCHIVO
                                char[] _TipoSeparador = null;
                                _MsgError = "";
                                DataTable dtEtl = new DataTable();
                                //--
                                if (_SeparadorArchivo.ToString().Trim().Equals("TAB"))
                                {
                                    _TipoSeparador = new char[] { '\t' };
                                    //--
                                    dtEtl = objFunctions.GetEtl(_ManejaCampoTitulo, _IniciaCampoDetalle, _PathFilesDavibox, _TipoSeparador, ref _MsgError);
                                }
                                else if (_SeparadorArchivo.ToString().Trim().Equals("ESP"))
                                {
                                    _TipoSeparador = new char[] { ' ' };
                                    //--
                                    dtEtl = objFunctions.GetEtl_LongCampo(_ManejaCampoTitulo, _NombreTabla, _IniciaCampoDetalle, _PathFilesDavibox, ref _MsgError);
                                }
                                else if (_SeparadorArchivo.ToString().Trim().Equals("EXCEL"))
                                {
                                    //--
                                    dtEtl = objFunctions.GetEtl_Excel(_NombreTabla, _IniciaCampoTitulo, _IniciaCampoDetalle, _PathFilesDavibox, ref _MsgError);
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
                                        //--
                                        Fila["CANTIDAD_REGISTROS"] = String.Format(String.Format("{0:###,###,##0}", dtEtl.Rows.Count));
                                        dtFiles.Rows.Add(Fila);

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
                                                //--AQUI REALIZAMOS EL BORRADO DE LOS DATOS DE LA TABLA
                                                objProcessDb.TipoProceso = 2;
                                                objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();

                                                _CodError = ""; _MsgError = "";
                                                if (objProcessDb.DlInfoData(ref _CodError, ref _MsgError))
                                                {
                                                    //--AQUI RECORREMOS LOS DATOS DEL DATATABLE
                                                    foreach (DataRow rowItemEtl in dtEtl.Rows)
                                                    {
                                                        #region AQUI OBTENEMOS LOS DATOS DEL DATATABLE
                                                        //--
                                                        string ICA_CONSECUTIVO = rowItemEtl["ICA_CONSECUTIVO"].ToString().Trim().Replace("\"", "");
                                                        string FECHA_REGISTRO1 = rowItemEtl["FECHA_REGISTRO"].ToString().Trim().Equals("NA") ? "" : rowItemEtl["FECHA_REGISTRO"].ToString().Trim();
                                                        string FECHA_REGISTRO = FECHA_REGISTRO1.ToString().Trim().Length > 0 ? Convert.ToDateTime(FECHA_REGISTRO1).ToString("yyyy-MM-dd") : "";
                                                        //--
                                                        string EMAIL_ADDRESS = rowItemEtl["EMAIL_ADDRESS"].ToString().Trim();
                                                        string AREA_A_LA_QUE_CORRESPONDE = objFunctions.GetLimpiarCadena(rowItemEtl["AREA_A_LA_QUE_CORRESPONDE"].ToString().Trim());
                                                        string NOMBRE_DEL_VENDEDOR_O_PERSONA_QUE_ENTREGO_EL_BIEN1 = rowItemEtl["NOMBRE_DEL_VENDEDOR_O_PERSONA_QUE_ENTREGO_EL_BIEN"].ToString().Trim().Length > 0 ? rowItemEtl["NOMBRE_DEL_VENDEDOR_O_PERSONA_QUE_ENTREGO_EL_BIEN"].ToString().Trim() : "NA";
                                                        string NOMBRE_DEL_VENDEDOR_O_PERSONA_QUE_ENTREGO_EL_BIEN = objFunctions.GetLimpiarCadena(NOMBRE_DEL_VENDEDOR_O_PERSONA_QUE_ENTREGO_EL_BIEN1);
                                                        string NIT = rowItemEtl["NIT"].ToString().Trim();
                                                        string DIGITO_DE_VERIFICACION = rowItemEtl["DIGITO_DE_VERIFICACION"].ToString().Trim();
                                                        string NUMERO_DE_LA_OBLIGACION = rowItemEtl["NUMERO_DE_LA_OBLIGACION"].ToString().Trim();
                                                        string SUCURSAL_DE_RADICACION_DE_CREDITO1 = rowItemEtl["SUCURSAL_DE_RADICACION_DE_CREDITO"].ToString().Trim().Length > 0 ? rowItemEtl["SUCURSAL_DE_RADICACION_DE_CREDITO"].ToString().Trim() : "NA";
                                                        string SUCURSAL_DE_RADICACION_DE_CREDITO = objFunctions.GetLimpiarCadena(SUCURSAL_DE_RADICACION_DE_CREDITO1);
                                                        string DIRECCION_DEL_DOMICILIO_PRINCIPAL1 = rowItemEtl["DIRECCION_DEL_DOMICILIO_PRINCIPAL_DEL_VENDEDOR_O_PERSONA_QUE_ENTREGA_EL_INMUEBLE"].ToString().Trim().Length > 0 ? rowItemEtl["DIRECCION_DEL_DOMICILIO_PRINCIPAL_DEL_VENDEDOR_O_PERSONA_QUE_ENTREGA_EL_INMUEBLE"].ToString().Trim() : "NA";
                                                        string DIRECCION_DEL_DOMICILIO_PRINCIPAL = objFunctions.GetLimpiarCadena(DIRECCION_DEL_DOMICILIO_PRINCIPAL1);
                                                        string CIUDAD_DEL_DOMICILIO_PRINCIPAL1 = rowItemEtl["CIUDAD_DEL_DOMICILIO_PRINCIPAL_DE_VENDEDOR_O_PERSONA_QUE_ENTREGA_EL_INMUEBLE"].ToString().Trim().Length > 0 ? rowItemEtl["CIUDAD_DEL_DOMICILIO_PRINCIPAL_DE_VENDEDOR_O_PERSONA_QUE_ENTREGA_EL_INMUEBLE"].ToString().Trim() : "NA";
                                                        string CIUDAD_DEL_DOMICILIO_PRINCIPAL = objFunctions.GetLimpiarCadena(CIUDAD_DEL_DOMICILIO_PRINCIPAL1);
                                                        string NUMERO_TELEFONICO = rowItemEtl["NUMERO_TELEFONICO"].ToString().Trim().Length > 0 ? rowItemEtl["NUMERO_TELEFONICO"].ToString().Trim() : "NA";
                                                        string CORREO_ELECTRONICO = rowItemEtl["CORREO_ELECTRONICO"].ToString().Trim().Length > 0 ? rowItemEtl["CORREO_ELECTRONICO"].ToString().Trim() : "NA";
                                                        //--
                                                        string FECHA_DE_DESEMBOLSO1 = rowItemEtl["FECHA_DE_DESEMBOLSO_O_ESCRITURACION"].ToString().Trim().Equals("NA") ? "" : rowItemEtl["FECHA_DE_DESEMBOLSO_O_ESCRITURACION"].ToString().Trim();
                                                        string FECHA_DE_DESEMBOLSO = FECHA_DE_DESEMBOLSO1.ToString().Trim().Length > 0 ? Convert.ToDateTime(FECHA_DE_DESEMBOLSO1).ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                                                        //--
                                                        string MES_DE_LA_RETENCION = rowItemEtl["MES_DE_LA_RETENCION"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["MES_DE_LA_RETENCION"].ToString().Trim()) : "NA";
                                                        string ANO_DE_LA_ESCRITURACION = rowItemEtl["ANO_DE_LA_ESCRITURACION"].ToString().Trim().Length > 0 ? rowItemEtl["ANO_DE_LA_ESCRITURACION"].ToString().Trim() : "NA";
                                                        string NIT_DEL_MUNICIPIO = rowItemEtl["NIT_DEL_MUNICIPIO_DONDE_SE_ENCUENTRA_UBICADO_EL_INMUEBLE"].ToString().Trim().Length > 0 ? rowItemEtl["NIT_DEL_MUNICIPIO_DONDE_SE_ENCUENTRA_UBICADO_EL_INMUEBLE"].ToString().Trim() : "NA";
                                                        string CALIDAD_TRIBUTARIA = rowItemEtl["CALIDAD_TRIBUTARIA"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["CALIDAD_TRIBUTARIA"].ToString().Trim()) : "NA";
                                                        string ES_UN_ACTIVO_FIJO = rowItemEtl["ES_UN_ACTIVO_FIJO"].ToString().Trim().Length > 0 ? rowItemEtl["ES_UN_ACTIVO_FIJO"].ToString().Trim() : "NA";
                                                        string CODIGO_DANE_DEL_MUNICIPIO = rowItemEtl["CODIGO_DANE_DEL_MUNICIPIO_DEL_DOMICILIO_PRINCIPAL_DEL_VENDEDOR"].ToString().Trim().Length > 0 ? rowItemEtl["CODIGO_DANE_DEL_MUNICIPIO_DEL_DOMICILIO_PRINCIPAL_DEL_VENDEDOR"].ToString().Trim() : "NA";

                                                        string VALOR_DE_LA_COMPRA = rowItemEtl["VALOR_DE_LA_COMPRA_O_DACION"].ToString().Trim().Length > 0 ? rowItemEtl["VALOR_DE_LA_COMPRA_O_DACION"].ToString().Trim().Replace("-", "0").Replace(",", ".") : "0";
                                                        double VALOR_DE_LA_COMPRA_O_DACION = Double.Parse(VALOR_DE_LA_COMPRA);
                                                        string TARIFA_DE_RETENCION1 = rowItemEtl["TARIFA_DE_RETENCION"].ToString().Trim().Length > 0 ? rowItemEtl["TARIFA_DE_RETENCION"].ToString().Trim().Replace("-", "0").Replace(",", ".") : "0";
                                                        double TARIFA_DE_RETENCION = Double.Parse(TARIFA_DE_RETENCION1);
                                                        string DIGITE_EL_VALOR_DE_RENTENCION_DE_ICA1 = rowItemEtl["DIGITE_EL_VALOR_DE_RENTENCION_DE_ICA"].ToString().Trim().Length > 0 ? rowItemEtl["DIGITE_EL_VALOR_DE_RENTENCION_DE_ICA"].ToString().Trim().Replace("-", "0").Replace("$", "").Replace(",", "") : "0";
                                                        double DIGITE_EL_VALOR_DE_RENTENCION_DE_ICA = Double.Parse(DIGITE_EL_VALOR_DE_RENTENCION_DE_ICA1);
                                                        string CIUDAD_DE_UBICACION_DEL_INMUEBLE = rowItemEtl["CIUDAD_DE_UBICACION_DEL_INMUEBLE"].ToString().Trim().Length > 0 ? rowItemEtl["CIUDAD_DE_UBICACION_DEL_INMUEBLE"].ToString().Trim() : "NA";
                                                        string MES_DEL_DESEMBOLSO = rowItemEtl["MES_DEL_DESEMBOLSO"].ToString().Trim().Length > 0 ? rowItemEtl["MES_DEL_DESEMBOLSO"].ToString().Trim() : "NA";
                                                        string URL_ARCHIVO_GENERADO = rowItemEtl["URL_ARCHIVO_GENERADO"].ToString().Trim().Length > 0 ? rowItemEtl["URL_ARCHIVO_GENERADO"].ToString().Trim() : "NA";
                                                        string ESTA_RETENCION_CORRESPONDE = rowItemEtl["ESTA_RETENCION_CORRESPONDE_A_UNA_OPERACION_DEL_ANO_ACTUAL"].ToString().Trim().Length > 0 ? rowItemEtl["ESTA_RETENCION_CORRESPONDE_A_UNA_OPERACION_DEL_ANO_ACTUAL"].ToString().Trim() : "NA";
                                                        string URL_ARCHIVO_CARGADO = rowItemEtl["URL_ARCHIVO_CARGADO"].ToString().Trim().Length > 0 ? rowItemEtl["URL_ARCHIVO_CARGADO"].ToString().Trim() : "NA";

                                                        //--AQUI CONCATENAMOS LOS VALORES DEL ESTADO FINANCIERO
                                                        if (_ArrayData.ToString().Trim().Length > 0)
                                                        {
                                                            _ArrayData = _ArrayData.ToString().Trim() + "," + quote + "(" + ICA_CONSECUTIVO + "," + FECHA_REGISTRO + "," + EMAIL_ADDRESS + "," + AREA_A_LA_QUE_CORRESPONDE + "," + NOMBRE_DEL_VENDEDOR_O_PERSONA_QUE_ENTREGO_EL_BIEN + "," + NIT + "," + DIGITO_DE_VERIFICACION + "," + NUMERO_DE_LA_OBLIGACION + "," + SUCURSAL_DE_RADICACION_DE_CREDITO + "," + DIRECCION_DEL_DOMICILIO_PRINCIPAL + "," + CIUDAD_DEL_DOMICILIO_PRINCIPAL + "," + NUMERO_TELEFONICO + "," + CORREO_ELECTRONICO + "," + FECHA_DE_DESEMBOLSO + "," + MES_DE_LA_RETENCION + "," + ANO_DE_LA_ESCRITURACION + "," + NIT_DEL_MUNICIPIO + "," + CALIDAD_TRIBUTARIA + "," + ES_UN_ACTIVO_FIJO + "," + CODIGO_DANE_DEL_MUNICIPIO + "," + VALOR_DE_LA_COMPRA_O_DACION + "," + TARIFA_DE_RETENCION + "," + DIGITE_EL_VALOR_DE_RENTENCION_DE_ICA + "," + CIUDAD_DE_UBICACION_DEL_INMUEBLE + "," + MES_DEL_DESEMBOLSO + "," + URL_ARCHIVO_GENERADO + "," + ESTA_RETENCION_CORRESPONDE + "," + URL_ARCHIVO_CARGADO + ")" + quote;
                                                        }
                                                        else
                                                        {
                                                            _ArrayData = quote + "(" + ICA_CONSECUTIVO + "," + FECHA_REGISTRO + "," + EMAIL_ADDRESS + "," + AREA_A_LA_QUE_CORRESPONDE + "," + NOMBRE_DEL_VENDEDOR_O_PERSONA_QUE_ENTREGO_EL_BIEN + "," + NIT + "," + DIGITO_DE_VERIFICACION + "," + NUMERO_DE_LA_OBLIGACION + "," + SUCURSAL_DE_RADICACION_DE_CREDITO + "," + DIRECCION_DEL_DOMICILIO_PRINCIPAL + "," + CIUDAD_DEL_DOMICILIO_PRINCIPAL + "," + NUMERO_TELEFONICO + "," + CORREO_ELECTRONICO + "," + FECHA_DE_DESEMBOLSO + "," + MES_DE_LA_RETENCION + "," + ANO_DE_LA_ESCRITURACION + "," + NIT_DEL_MUNICIPIO + "," + CALIDAD_TRIBUTARIA + "," + ES_UN_ACTIVO_FIJO + "," + CODIGO_DANE_DEL_MUNICIPIO + "," + VALOR_DE_LA_COMPRA_O_DACION + "," + TARIFA_DE_RETENCION + "," + DIGITE_EL_VALOR_DE_RENTENCION_DE_ICA + "," + CIUDAD_DE_UBICACION_DEL_INMUEBLE + "," + MES_DEL_DESEMBOLSO + "," + URL_ARCHIVO_GENERADO + "," + ESTA_RETENCION_CORRESPONDE + "," + URL_ARCHIVO_CARGADO + ")" + quote;
                                                        }
                                                        _CantidadReg++;
                                                        _CantidadTotalReg++;

                                                        //--AQUI VALIDAMOS LA CANTIDAD DE REGISTROS LEIDOS PARA CARGAR
                                                        if (FixedData.CantidadRegProcesar == _CantidadReg)
                                                        {
                                                            #region AQUI ENVIAMOS A CARGAR LOS DATOS EN LA DB
                                                            //--
                                                            objProcessDb.TipoProceso = 2;
                                                            objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                            objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();
                                                            objProcessDb.ArrayDataLf = null;
                                                            objProcessDb.ArrayDataLh = _ArrayData.ToString().Trim();
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
                                                            objProcessDb.TipoProceso = 2;
                                                            objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                            objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();
                                                            objProcessDb.ArrayDataLf = null;
                                                            objProcessDb.ArrayDataLh = _ArrayData.ToString().Trim();
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
                                                #region CARGAR INFO A LA TABLA TARJETA_CREDITO
                                                //--AQUI REALIZAMOS EL BORRADO DE LOS DATOS DE LA TABLA
                                                objProcessDb.TipoProceso = 5;
                                                //--AQUI VALIDAMOS SI YA EJECUTO EL 1er PROCESO DE LA T.C.
                                                if (_ContadorIc == 0)
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
                                                        string _UN = rowItemEtl["UN"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["UN"].ToString().Trim()) : "NA";
                                                        string _GLIBROS = rowItemEtl["G_LIBROS"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["G_LIBROS"].ToString().Trim()) : "NA";
                                                        string _LIBRO = rowItemEtl["LIBRO"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["LIBRO"].ToString().Trim()) : "NA";
                                                        string _CUENTA = rowItemEtl["CUENTA"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["CUENTA"].ToString().Trim()) : "NA";
                                                        string _SUCURSAL = rowItemEtl["SUCURSAL"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["SUCURSAL"].ToString().Trim()) : "NA";
                                                        string _DEPENDENCIA = rowItemEtl["DEPENDENCIA"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["DEPENDENCIA"].ToString().Trim()) : "NA";
                                                        string _IDASIENTO = rowItemEtl["ID_DE_ASIENTO"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["ID_DE_ASIENTO"].ToString().Trim()) : "NA";
                                                        string _FECHA_COMPROBANTE = Convert.ToDateTime(rowItemEtl["FECHA_COMPROBANTE"].ToString().Trim()).ToString("yyyy-MM-dd");
                                                        string _FECHA_PROCESO = Convert.ToDateTime(rowItemEtl["FECHA_PROCESO"].ToString().Trim()).ToString("yyyy-MM-dd");
                                                        string _DESCRIPCION = rowItemEtl["DESCRIPCION"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["DESCRIPCION"].ToString().Trim()) : "NA";
                                                        //--
                                                        string _DEBITO1 = rowItemEtl["DEBITO"].ToString().Trim().Length > 0 ? rowItemEtl["DEBITO"].ToString().Trim().Replace("*", "0") : "0";
                                                        double _DEBITO = Double.Parse(_DEBITO1);
                                                        //--
                                                        string _CREDITO1 = rowItemEtl["CREDITO"].ToString().Trim().Length > 0 ? rowItemEtl["CREDITO"].ToString().Trim().Replace("*", "0") : "0";
                                                        double _CREDITO = Double.Parse(_CREDITO1);
                                                        string _AUXILIAR = rowItemEtl["AUXILIAR"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["AUXILIAR"].ToString().Trim()) : "NA";
                                                        string _REFERENCIA = rowItemEtl["REFERENCIA"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["REFERENCIA"].ToString().Trim()) : "NA";
                                                        string _USUARIO = rowItemEtl["USUARIO"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["USUARIO"].ToString().Trim()) : "NA";
                                                        string _IDCOMPROBANTE = rowItemEtl["ID_COMPROBANTE"].ToString().Trim().Length > 0 ? rowItemEtl["ID_COMPROBANTE"].ToString().Trim() : "NA";
                                                        string _ESTADO = rowItemEtl["ESTADO"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["ESTADO"].ToString().Trim()) : "NA";
                                                        string _REAL = rowItemEtl["REAL"].ToString().Trim().Length > 0 ? objFunctions.GetLimpiarCadena(rowItemEtl["REAL"].ToString().Trim()) : "X";

                                                        //--AQUI CONCATENAMOS LOS VALORES DEL ESTADO FINANCIERO
                                                        if (_ArrayData.ToString().Trim().Length > 0)
                                                        {
                                                            _ArrayData = _ArrayData.ToString().Trim() + "," + quote + "(" + _UN + "," + _GLIBROS + "," + _LIBRO + "," + _CUENTA + "," + _SUCURSAL + "," + _DEPENDENCIA + "," + _IDASIENTO + "," + _FECHA_COMPROBANTE + "," + _FECHA_PROCESO + "," + _DESCRIPCION + "," + _DEBITO + "," + _CREDITO + "," + _AUXILIAR + "," + _REFERENCIA + "," + _USUARIO + "," + _IDCOMPROBANTE + "," + _ESTADO + "," + _REAL + ")" + quote;
                                                        }
                                                        else
                                                        {
                                                            _ArrayData = quote + "(" + _UN + "," + _GLIBROS + "," + _LIBRO + "," + _CUENTA + "," + _SUCURSAL + "," + _DEPENDENCIA + "," + _IDASIENTO + "," + _FECHA_COMPROBANTE + "," + _FECHA_PROCESO + "," + _DESCRIPCION + "," + _DEBITO + "," + _CREDITO + "," + _AUXILIAR + "," + _REFERENCIA + "," + _USUARIO + "," + _IDCOMPROBANTE + "," + _ESTADO + "," + _REAL + ")" + quote;
                                                        }
                                                        _CantidadReg++;
                                                        _CantidadTotalReg++;

                                                        //--AQUI VALIDAMOS LA CANTIDAD DE REGISTROS LEIDOS PARA CARGAR
                                                        if (FixedData.CantidadRegProcesar == _CantidadReg)
                                                        {
                                                            #region AQUI ENVIAMOS A CARGAR LOS DATOS EN LA DB
                                                            //--
                                                            objProcessDb.TipoProceso = 5;
                                                            objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                            objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();
                                                            objProcessDb.ArrayDataLf = null;
                                                            objProcessDb.ArrayDataLh = null;
                                                            objProcessDb.ArrayDataPg = null;
                                                            objProcessDb.ArrayDataTc = null;
                                                            objProcessDb.ArrayDataIc = _ArrayData.ToString().Trim();
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
                                                    _ContadorIc++;
                                                    if (_ArrayData.ToString().Trim().Length > 0)
                                                    {
                                                        if (_ErrorProcesar.Equals("N"))
                                                        {
                                                            #region AQUI ENVIAMOS A CARGAR LOS DATOS EN LA DB
                                                            //--
                                                            objProcessDb.TipoProceso = 5;
                                                            objProcessDb.AnioGravable = objFileDavibox.anio_gravable;
                                                            objProcessDb.MesEf = objFileDavibox.mes_procesar.ToString().Trim();
                                                            objProcessDb.ArrayDataLf = null;
                                                            objProcessDb.ArrayDataLh = null;
                                                            objProcessDb.ArrayDataPg = null;
                                                            objProcessDb.ArrayDataTc = null;
                                                            objProcessDb.ArrayDataIc = _ArrayData.ToString().Trim();
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

                        #region AQUI VALIDAMOS LA CANTIDAD DE ARCHIVOS DEL DAVIBOX PROCESADOS 
                        //--VALIDAMOS SI LA LISTA VIENE LLENA
                        if (dtFiles != null)
                        {
                            if (dtFiles.Rows.Count > 0)
                            {
                                #region AQUI OBTENEMOS LOS DATOS DEL DATABLE PARA ENVIAR EL EMAIL
                                //--AQUI BORRAMOS LA TAREA PROGRAMADA
                                DeleteTaskSchedulerManual(objFileDavibox.nombre_tarea.ToString().Trim());

                                //--
                                StringBuilder strDetalleEmail = new StringBuilder();
                                //--
                                string _TituloTablaHtml = "LISTA DE ARCHIVOS DEL DAVIBOX PROCESADOS EN SMARTAX";
                                string _TableHtml = GetTableHtml(dtFiles, _TituloTablaHtml);
                                strDetalleEmail.Append(_TableHtml.ToString() + "<br/><br/><br/>");

                                //--INSTANCIAMOS VARIABLES DE OBJETO PARA EL ENVIO DE EMAILS
                                ObjEmails.EmailPara = FixedData.EmailDestinoError;
                                ObjEmails.EmailCopia = FixedData.EnvioEmailCopia;
                                ObjEmails.Asunto = "REF.: LISTA DE ARCHIVOS DEL DAVIBOX PROCESADOS SMARTAX";
                                ObjEmails.Detalle = "Señor usuario a continuación se relacionan los archivos que fueron cargados en la base de datos de smartax. Por favor podría validar la información descargado un reporte de Retención de Ica." + "\n\n" + strDetalleEmail.ToString().Trim();
                                //--
                                _MsgError = "";
                                if (ObjEmails.SendEmailConCopia(ref _MsgError))
                                {
                                    strDetalleEmail = new StringBuilder();
                                    FixedData.LogApi.Info(_MsgError);
                                }
                                else
                                {
                                    FixedData.LogApi.Error(_MsgError);
                                }
                                #endregion
                            }
                        }
                        #endregion
                        //--
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

        private static bool DeleteTaskSchedulerManual(string _NombreTarea)
        {
            bool Result = false;
            string _MsgError = "";
            try
            {
                using (TaskService ts = new TaskService())
                {
                    //string _NombreTarea = "TAREA MANUAL [" + _NombreProveedor + "]";
                    ts.RootFolder.DeleteTask(_NombreTarea.ToString().Trim());

                    Result = true;
                    _MsgError = "LA TAREA PROGRAMADA [" + _NombreTarea + "] HA SIDO BORRADA DEL SERVIDOR.";
                    FixedData.LogApi.Info(_MsgError);
                }
            }
            catch (Exception ex)
            {
                Result = false;
                _MsgError = "ERROR AL BORRAR LA TAREA PROGRAMADA [" + _NombreTarea + "] DEL SERVIDOR. MOTIVO: " + ex.Message;
                FixedData.LogApi.Error(_MsgError);
            }

            return Result;
        }

        public static string GetTableHtml(DataTable DtDatos, string _TituloTablaHtml)
        {
            StringBuilder TableHtml = new StringBuilder();
            try
            {
                //Table start.
                TableHtml.Append("<table border = '1'>");
                TableHtml.Append("<tr align='center' valign='middle' >");
                TableHtml.Append("<th colspan=" + DtDatos.Columns.Count + "> " + _TituloTablaHtml + " </th> ");
                TableHtml.Append("</tr>");

                //Building the Header row.
                TableHtml.Append("<tr>");
                foreach (DataColumn column in DtDatos.Columns)
                {
                    TableHtml.Append("<th>");
                    TableHtml.Append(column.ColumnName.ToString().ToUpper());
                    TableHtml.Append("</th>");
                }
                TableHtml.Append("</tr>");

                //Building the Data rows.
                foreach (DataRow row in DtDatos.Rows)
                {
                    TableHtml.Append("<tr>");
                    foreach (DataColumn column in DtDatos.Columns)
                    {
                        TableHtml.Append("<td>");
                        TableHtml.Append(row[column.ColumnName]);
                        TableHtml.Append("</td>");
                    }
                    TableHtml.Append("</tr>");
                }

                //Table end.
                TableHtml.Append("</table>");

            }
            catch (Exception ex)
            {
                TableHtml.Append("");
                FixedData.LogApi.Error("Error al obtener la Tabla Html. Motivo: " + ex.Message);
            }

            return TableHtml.ToString();
        }

    }
}
