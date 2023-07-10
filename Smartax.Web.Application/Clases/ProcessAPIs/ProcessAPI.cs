using System;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using RestSharp;
using log4net;
using Smartax.Web.Application.Clases.Seguridad;
using static Smartax.Web.Application.Clases.ProcessAPIs.ModelApiSmartax;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;

namespace Smartax.Web.Application.Clases.ProcessAPIs
{
    public class ProcessAPI
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        #region DEFINICION DE ATRIBUTOS DE LA CLASE
        public int TipoConsulta { get; set; }
        public int IdEjecucionLote { get; set; }
        public object IdClienteEf { get; set; }
        public object IdCliente { get; set; }
        public object IdClienteEstablecimiento { get; set; }
        public int IdFormImpuesto { get; set; }
        public object IdFormConfiguracion { get; set; }
        public object IdPuc { get; set; }
        public int AnioGravable { get; set; }
        public string VersionEf { get; set; }
        public string MesEf { get; set; }
        public object DataProcesar { get; set; }
        public object Emails { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public int TipoLiquidación { get; set; }

        //--VARIABLES PARA EL SERVICIO DE DAVIBOX
        public string UuId { get; set; }
        public int MesProcesar { get; set; }
        public int AnioProcesar { get; set; }
        public int PrevMonth { get; set; }
        #endregion

        public bool GetProcesoBaseGravable(ref string _MsgError)
        {
            bool _Result = false;
            try
            {
                //--AQUI OBTENEMOS EL TOKEN 
                //string _ResultTokenAut = GetTokenJwt();
                //if (_ResultTokenAut.ToString().Trim().Length > 0)
                //{
                //    string[] _TokenAut = _ResultTokenAut.ToString().Trim().Split('|');
                //    //---
                //    if (_TokenAut[0].ToString().Trim().Equals("00"))
                //    {
                #region ENVIAR LA TRANSACCION AL API DEL SUSER
                //Variables de librería para hacer el POST
                var client = new RestClient(FixedData.BaseUrlSmartax);
                var request = new RestRequest(FixedData.ActionSegmentSmartax + "base_gravable", Method.POST);
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                //request.AddHeader("Authorization", "Bearer " + _TokenAut[1].ToString().Trim() + "");
                //Variable de respuesta a petición POST
                var respPOST = new object();

                //Establecer el DTO para enviar, Setear datos
                BaseGravable_Req conReq = new BaseGravable_Req
                {
                    tipo_consulta = TipoConsulta,
                    idcliente_ef = IdClienteEf,
                    id_cliente = IdCliente,
                    idcliente_establecimiento = IdClienteEstablecimiento,
                    idform_impuesto = IdFormImpuesto,
                    idform_configuracion = IdFormConfiguracion,
                    id_puc = IdPuc,
                    anio_gravable = AnioGravable,
                    version_ef = VersionEf,
                    mes_ef = MesEf,
                    id_usuario = IdUsuario
                };

                //Agregar al Body de la petición
                request.AddJsonBody(conReq);
                //--AQUI MANDAMOS A ESCRIBIR EN LOS LOGS DE AUDITORIA
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(conReq);
                _log.Warn("REQUEST GENERAR BASE GRAVABLE => " + jsonRequest);

                //Llamado a Servicio Rest usando RestSharp            
                //Respuesta como objeto dinámico
                var response = client.ExecuteDynamic(request);
                respPOST = JsonConvert.DeserializeObject<BaseGravable_Resp>(response.Content);

                //Validar código de respuesta
                JavaScriptSerializer JsSerializer = new JavaScriptSerializer();

                ///--Validar código de respuesta
                string _CodRespuesta = "", _MsgRespuesta = "";
                if (((BaseGravable_Resp)respPOST).Codigo != null)
                {
                    var JsonResult = JsSerializer.Deserialize<BaseGravable_Resp>(response.Content);
                    string JsonResponse = JsSerializer.Serialize(JsonResult);
                    _log.Warn("RESPONSE BASE GRAVABLE => " + JsonResponse);

                    //--OK
                    if (JsonResult.Codigo.ToString().Trim().Equals("00"))
                    {
                        #region AQUI OBTENEMOS DATOS DE LA VENTA RECARGA
                        //Valores de retornos.
                        _CodRespuesta = JsonResult.Codigo.ToString().Trim();
                        _MsgRespuesta = JsonResult.Message.ToString().Trim();
                        //--
                        _Result = true;
                        _MsgError = _CodRespuesta + "|" + _MsgRespuesta;
                        #endregion
                    }
                    else
                    {
                        #region ERROR AL REALIZAR LA TX DE CONSULTA
                        //Aqui retornamos los datos de la Respuesta.
                        _CodRespuesta = JsonResult.Codigo != null ? JsonResult.Codigo.ToString().Trim() : "ER";
                        _MsgRespuesta = JsonResult.Message != null ? JsonResult.Message.ToString().Trim() : "Error al realizar el proceso !";
                        //--
                        _Result = false;
                        _MsgError = _CodRespuesta + "|" + _MsgRespuesta;
                        #endregion
                    }
                }
                else
                {
                    #region ERROR AL REALIZAR LA TX DE CONSULTA
                    //Aqui retornamos los datos de la Respuesta de MegaRed.
                    var tokenResponse = JsonConvert.DeserializeObject<BaseGravable_Resp>(response.Content);
                    _CodRespuesta = tokenResponse.Codigo != null ? tokenResponse.Codigo.ToString().Trim() : "ER";
                    _MsgRespuesta = tokenResponse.Message != null ? tokenResponse.Message.ToString().Trim() : "Error desconocido del proveedor !";
                    //--
                    _Result = false;
                    _MsgError = _CodRespuesta + "|" + _MsgRespuesta;
                    #endregion
                }
                #endregion
                //    }
                //    else
                //    {
                //        _Result = false;
                //        _MsgError = _TokenAut[0].ToString().Trim() + "|" + _TokenAut[1].ToString().Trim();
                //    }
                //}
                //else
                //{
                //    _Result = false;
                //    _MsgError = "01" + "|" + "No se genero un token para realizar la transacción. Por favor validar con soporte técnico !";
                //}
            }
            catch (Exception ex)
            {
                //Error
                _Result = false;
                string _Error = "-1|Error al Generar la Base Gravable|" + ex.Message;
            }
            return _Result;
        }

        public bool GetProcesoLiquidacion(LiquidarImpuesto_Req objData, ref string _MsgError)
        {
            bool _Result = false;
            try
            {
                //--AQUI OBTENEMOS EL TOKEN 
                //string _ResultTokenAut = GetTokenJwt();
                //if (_ResultTokenAut.ToString().Trim().Length > 0)
                //{
                //    string[] _TokenAut = _ResultTokenAut.ToString().Trim().Split('|');
                //    //---
                //    if (_TokenAut[0].ToString().Trim().Equals("00"))
                //    {
                #region ENVIAR LA TRANSACCION AL API DEL SUSER
                //Variables de librería para hacer el POST
                var client = new RestClient(FixedData.BaseUrlSmartax);
                var request = new RestRequest(FixedData.ActionSegmentSmartax + "liquidar_impuesto", Method.POST);
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                //request.AddHeader("Authorization", "Bearer " + _TokenAut[1].ToString().Trim() + "");
                //Variable de respuesta a petición POST
                var respPOST = new object();

                //Establecer el DTO para enviar, Setear datos
                LiquidarImpuesto_Req conReq = new LiquidarImpuesto_Req
                {
                    estado_liquidacion = objData.estado_liquidacion,
                    idejecucion_lote = objData.idejecucion_lote,
                    tipo_impuesto = objData.tipo_impuesto,
                    data_procesar = objData.data_procesar.ToString().Trim(),
                    emails_confirmar = objData.emails_confirmar.ToString().Trim(),
                    id_usuario = objData.id_usuario,
                    nombre_usuario = objData.nombre_usuario,
                    info_firmante1 = objData.info_firmante1,
                    info_firmante2 = objData.info_firmante2
                };

                //Agregar al Body de la petición
                request.AddJsonBody(conReq);
                //--AQUI MANDAMOS A ESCRIBIR EN LOS LOGS DE AUDITORIA
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(conReq);
                _log.Warn("REQUEST PROCESAR LIQUIDACION x LOTE => " + jsonRequest);

                //Llamado a Servicio Rest usando RestSharp            
                //Respuesta como objeto dinámico
                var response = client.ExecuteDynamic(request);
                respPOST = JsonConvert.DeserializeObject<BaseGravable_Resp>(response.Content);

                //Validar código de respuesta
                JavaScriptSerializer JsSerializer = new JavaScriptSerializer();

                ///--Validar código de respuesta
                string _CodRespuesta = "", _MsgRespuesta = "";
                if (((BaseGravable_Resp)respPOST).Codigo != null)
                {
                    var JsonResult = JsSerializer.Deserialize<BaseGravable_Resp>(response.Content);
                    string JsonResponse = JsSerializer.Serialize(JsonResult);
                    _log.Warn("RESPONSE PROCESAR LIQUIDACION x LOTE => " + JsonResponse);

                    //--OK
                    if (JsonResult.Codigo.ToString().Trim().Equals("00"))
                    {
                        #region AQUI OBTENEMOS DATOS DE LA VENTA RECARGA
                        //Valores de retornos.
                        _CodRespuesta = JsonResult.Codigo.ToString().Trim();
                        _MsgRespuesta = JsonResult.Message.ToString().Trim();
                        //--
                        _Result = true;
                        _MsgError = _CodRespuesta + "|" + _MsgRespuesta;
                        #endregion
                    }
                    else
                    {
                        #region ERROR AL REALIZAR LA TX DE CONSULTA
                        //Aqui retornamos los datos de la Respuesta.
                        _CodRespuesta = JsonResult.Codigo != null ? JsonResult.Codigo.ToString().Trim() : "ER";
                        _MsgRespuesta = JsonResult.Message != null ? JsonResult.Message.ToString().Trim() : "Error al realizar el proceso !";
                        //--
                        _Result = false;
                        _MsgError = _CodRespuesta + "|" + _MsgRespuesta;
                        #endregion
                    }
                }
                else
                {
                    #region ERROR AL REALIZAR LA TX DE CONSULTA
                    //Aqui retornamos los datos de la Respuesta de MegaRed.
                    var tokenResponse = JsonConvert.DeserializeObject<BaseGravable_Resp>(response.Content);
                    _CodRespuesta = tokenResponse.Codigo != null ? tokenResponse.Codigo.ToString().Trim() : "ER";
                    _MsgRespuesta = tokenResponse.Message != null ? tokenResponse.Message.ToString().Trim() : "Error desconocido del proveedor !";
                    //--
                    _Result = false;
                    _MsgError = _CodRespuesta + "|" + _MsgRespuesta;
                    #endregion
                }
                #endregion
                //    }
                //    else
                //    {
                //        _Result = false;
                //        _MsgError = _TokenAut[0].ToString().Trim() + "|" + _TokenAut[1].ToString().Trim();
                //    }
                //}
                //else
                //{
                //    _Result = false;
                //    _MsgError = "01" + "|" + "No se genero un token para realizar la transacción. Por favor validar con soporte técnico !";
                //}
            }
            catch (Exception ex)
            {
                //Error
                _Result = false;
                string _Error = "-1|Error con el proceso de Liquidación Impuestos|" + ex.Message;
            }
            return _Result;
        }

        public bool GetDownloadFileDavibox(int TipoPeriodicidad, int _Periodo, ref string _MsgError)
        {
            bool _Result = false;
            try
            {
                #region ENVIAR LA TRANSACCION AL API DE DAVIBOX
                //Variables de librería para hacer el POST
                var client = new RestClient(FixedData.BaseUrlDavibox);
                var request = new RestRequest("sftp_download", Method.POST);
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                //request.AddHeader("Authorization", "Bearer " + _TokenAut[1].ToString().Trim() + "");
                //Variable de respuesta a petición POST
                var respPOST = new object();
                //string myuuidAsString = _UuId.ToString();
                DownloadFileDavibox_Req conReq = null;

                //--AQUI VALIDAMOS EL TIPO DE PERIODICIDAD (1. MENSUAL, 2. BIMESTRAL)
                if (TipoPeriodicidad == 1)
                {
                    //Establecer el DTO para enviar, Setear datos
                    conReq = new DownloadFileDavibox_Req
                    {
                        uuid = UuId.ToString().Trim(),
                        month = MesProcesar,
                        year = AnioProcesar,
                        bimonthly = false
                    };
                }
                else
                {
                    #region AQUI OBTENEMOS EL MES ANTERIOR PARA EL BIMESTRE
                    //--
                    int _PrevMonth = 0;
                    switch (_Periodo)
                    {
                        case 1:
                            _PrevMonth = 1;
                            break; 
                        case 2:
                            _PrevMonth = 3;
                            break;
                        case 3:
                            _PrevMonth = 5;
                            break;
                        case 4:
                            _PrevMonth = 7;
                            break;
                        case 5:
                            _PrevMonth = 9;
                            break;
                        case 6:
                            _PrevMonth = 11;
                            break;
                        default:
                            _PrevMonth = (MesProcesar - 1);
                            break;
                    }

                    //Establecer el DTO para enviar, Setear datos
                    conReq = new DownloadFileDavibox_Req
                    {
                        uuid = UuId.ToString().Trim(),
                        month = MesProcesar,
                        year = AnioProcesar,
                        bimonthly = true,
                        prevmonth = _PrevMonth
                    };
                    #endregion
                }

                //Agregar al Body de la petición
                request.AddJsonBody(conReq);
                //--AQUI MANDAMOS A ESCRIBIR EN LOS LOGS DE AUDITORIA
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonRequest = js.Serialize(conReq);
                _log.Warn("REQUEST DOWNLOAD FILES DAVIBOX => " + jsonRequest);

                //Llamado a Servicio Rest usando RestSharp            
                //Respuesta como objeto dinámico
                var response = client.ExecuteDynamic(request);
                respPOST = JsonConvert.DeserializeObject<DownloadFileDavibox_Resp>(response.Content);

                //Validar código de respuesta
                JavaScriptSerializer JsSerializer = new JavaScriptSerializer();

                ///--Validar código de respuesta
                string _CodRespuesta = "", _MsgRespuesta = "";
                if (((DownloadFileDavibox_Resp)respPOST).transferedfiles != null)
                {
                    var JsonResult = JsSerializer.Deserialize<DownloadFileDavibox_Resp>(response.Content);
                    string JsonResponse = JsSerializer.Serialize(JsonResult);
                    _log.Warn("RESPONSE PROCESAR LIQUIDACION x LOTE => " + JsonResponse);

                    //--OK
                    if (JsonResult.transferedfiles.Count > 0)
                    {
                        #region AQUI OBTENEMOS DATOS DE LA VENTA RECARGA
                        //Valores de retornos.
                        _CodRespuesta = "00"; //--JsonResult.Codigo.ToString().Trim();
                        _MsgRespuesta = "exitoso";   //--JsonResult.Message.ToString().Trim();
                        //--
                        _Result = true;
                        _MsgError = _CodRespuesta + "|" + _MsgRespuesta;
                        #endregion
                    }
                    else
                    {
                        #region ERROR AL REALIZAR LA TX DE CONSULTA
                        //--VALIDAR SI VIENE LLENO EL OBJETO DEL NOMBRE DE ARCHIVOS SIN ENCONTRAR
                        StringBuilder _NombreFiles = new StringBuilder();
                        if (JsonResult.failedfiles.Count > 0)
                        {
                            foreach (string _NameFile in JsonResult.failedfiles)
                            {
                                if (_NombreFiles.ToString().Trim().Length > 0)
                                {
                                    _NombreFiles.Append(_NameFile);
                                }
                                else
                                {
                                    _NombreFiles.Append(_NameFile);
                                }
                            }
                            //--
                            //_MsgRespuesta = "failedfiles => " + _NombreFiles;
                        }
                        else
                        {
                            _MsgRespuesta = "Erro al obtener los archivos no encontrados en el davibox !";
                            _NombreFiles.Append(_MsgRespuesta);
                        }
                        //Aqui retornamos los datos de la Respuesta.
                        _CodRespuesta = "ER";
                        //--
                        _Result = false;
                        _MsgError = _CodRespuesta + "|" + _NombreFiles;
                        #endregion
                    }
                }
                else
                {
                    #region ERROR AL REALIZAR LA TX DE CONSULTA
                    //Aqui retornamos los datos de la Respuesta de MegaRed.
                    var tokenResponse = JsonConvert.DeserializeObject<DownloadFileDavibox_Resp>(response.Content);
                    //_CodRespuesta = tokenResponse.Codigo != null ? tokenResponse.Codigo.ToString().Trim() : "ER";
                    //_MsgRespuesta = tokenResponse.Message != null ? tokenResponse.Message.ToString().Trim() : "Error desconocido del proveedor !";
                    ////--
                    //_Result = false;
                    //_MsgError = _CodRespuesta + "|" + _MsgRespuesta;
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                //Error
                _Result = false;
                string _Error = "-1|Error con el proceso de Liquidación Impuestos|" + ex.Message;
            }
            return _Result;
        }

        public string GetTokenJwt()
        {
            string _Result = "";
            try
            {
                #region OBTENEMOS EL TOKEN DE AUTENTICACION DE LOS SERVICIOS
                //Variables de librería para hacer el POST
                var client = new RestClient(FixedData.BaseUrlSmartax);
                var request = new RestRequest(FixedData.ActionSegmentSmartax + "authenticate", Method.POST);
                request.AddHeader("Content-Type", "application/json; charset=utf-8");
                request.RequestFormat = DataFormat.Json;

                //Variable de respuesta a petición POST
                var respPOST = new object();

                ///Establecer el DTO para enviar, Setear datos
                Token_Req conReq = new Token_Req
                {
                    Username = FixedData.UserWsSmartax,
                    Password = FixedData.PassWsSmartax
                };

                ///--Agregar al Body de la petición
                request.AddJsonBody(conReq);

                ///--Llamado a Servicio Rest usando RestSharp            
                var response = client.Execute(request);
                respPOST = JsonConvert.DeserializeObject<Token_Resp>(response.Content);

                ///--Validar código de respuesta
                string _CodRespuesta = "", _MsgRespuesta = "";
                if (((Token_Resp)respPOST).Codigo != null)
                {
                    var tokenResponse = JsonConvert.DeserializeObject<Token_Resp>(response.Content);
                    //--OK
                    if (tokenResponse.Codigo.ToString().Trim().Equals("00"))
                    {
                        #region OBTENEMOS DATOS TX EXITOSA
                        //Valores de retornos.
                        _Result = tokenResponse.Codigo.ToString().Trim() + "|" + tokenResponse.token.ToString().Trim();
                        #endregion
                    }
                    else
                    {
                        //--AQUI OBTENEMOS EL ERROR AL GENERAR EL TOKEN
                        _Result = tokenResponse.Codigo.ToString().Trim() + "|" + tokenResponse.Message.ToString().Trim();
                    }
                }
                else
                {
                    #region ERROR AL REALIZAR LA TX DE CONSULTA
                    //Aqui retornamos los datos de la Respuesta de MegaRed.
                    var tokenResponse = JsonConvert.DeserializeObject<Token_Resp>(response.Content);
                    _CodRespuesta = tokenResponse.Codigo != null ? tokenResponse.Codigo.ToString().Trim() : "ER";
                    _MsgRespuesta = tokenResponse.Message != null ? tokenResponse.Message.ToString().Trim() : "Error desconocido del proveedor !";
                    _Result = _CodRespuesta + "|" + _MsgRespuesta;
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                _Result = "EX|Error al generar el token de seguridad. Motivo: " + ex.Message;
            }

            return _Result;
        }

    }
}