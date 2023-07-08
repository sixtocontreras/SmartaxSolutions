using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Smartax.WebApi.Services.Clases.Seguridad;
using Smartax.WebApi.Services.Models;
using System.Web.Script.Serialization;
using Smartax.WebApi.Services.Clases.Procesos;
using Hangfire;
using System.Threading.Tasks;

namespace Smartax.WebApi.Services.Controllers
{
    //[Authorize]
    [RoutePrefix("serviciosHostSmartax/api")]
    public class ProcessController : ApiController
    {
        [HttpPost, Route("base_gravable")]
        public HttpResponseMessage Post([FromBody]BaseGravable_Req objProcess)
        {
            HttpError _Response = null;
            try
            {
                if (objProcess != null)
                {
                    if (objProcess.id_cliente.ToString().Trim().Length > 0)
                    {
                        #region ENVIAR DOCUMENTO DEL CREDITO AL PROVEEDOR
                        //--AQUI ESCRIBIMOS EL REQUEST DE LA TRX EN LOS LOGS
                        JavaScriptSerializer JsSerializer = new JavaScriptSerializer();
                        //--SERIALIZAR EL REQUEST
                        string JsonRequest = JsSerializer.Serialize(objProcess);
                        FixedData.LogApi.Warn("REQUEST GENERAR BASE GRAVABLE => " + JsonRequest);

                        //--AQUI ENVIAMOS EL PROCESO BACKGROUND JOB
                        BackgroundJob.Enqueue(() => Tasks.GetBaseGravable(objProcess, "test"));

                        string _FechaHora = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                        string _Mensaje = "Señor usuario, el proceso de obtener la base gravable se encuentra en ejecución, en unos minutos le estara llegando un correo con la confirmación del proceso";
                        _Response = new HttpError(_Mensaje) { { "Status", true}, { "fecha_proceso", _FechaHora }, { "Codigo", "00" } };
                        #endregion
                    }
                    else
                    {
                        _Response = new HttpError("El id del cliente es requerido. !") { { "Status", false }, { "Codigo", HttpStatusCode.NoContent } };
                    }
                }
                else
                {
                    _Response = new HttpError("Error no se encontro información para validar !") { { "Status", false }, { "Codigo", HttpStatusCode.NotFound } };
                    FixedData.LogApi.Error("Error no se encontro información para validar !");
                }
            }
            catch (Exception ex)
            {
                string _MsgError = "Error al generar la base gravable del id cliente: [" + objProcess.id_cliente + "]. Motivo: " + ex.InnerException.Message;
                _Response = new HttpError(_MsgError) { { "Status", false }, { "Codigo", HttpStatusCode.InternalServerError } };
                FixedData.LogApi.Error(_MsgError);
            }

            return Request.CreateResponse(_Response);
        }

        [HttpPost, Route("liquidar_impuesto")]
        public HttpResponseMessage Post([FromBody]LiquidarImpuesto_Req objProcess)
        {
            HttpError _Response = null;
            try
            {
                if (objProcess != null)
                {
                    if (objProcess.data_procesar.ToString().Trim().Length > 0)
                    {
                        #region ENVIAR DOCUMENTO DEL CREDITO AL PROVEEDOR
                        //--AQUI ESCRIBIMOS EL REQUEST DE LA TRX EN LOS LOGS
                        JavaScriptSerializer JsSerializer = new JavaScriptSerializer();
                        //--SERIALIZAR EL REQUEST
                        string JsonRequest = JsSerializer.Serialize(objProcess);
                        FixedData.LogApi.Warn("REQUEST GENERAR LIQUIDACION => " + JsonRequest);

                        //--AQUI ENVIAMOS EL PROCESO BACKGROUND JOB
                        BackgroundJob.Enqueue(() => Tasks.GetLiquidarImpuesto(objProcess, "test"));

                        string _FechaHora = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                        string _Mensaje = "Señor usuario, el proceso de generar las liquidaciones se encuentra en ejecución, en unos minutos le estara llegando un correo con la confirmación de terminación del proceso";
                        _Response = new HttpError(_Mensaje) { { "Status", true }, { "fecha_proceso", _FechaHora }, { "Codigo", "00" } };
                        #endregion
                    }
                    else
                    {
                        _Response = new HttpError("Señor usuario, para realizar la liquidación es necesario que seleccione de la lista. !") { { "Status", false }, { "Codigo", HttpStatusCode.NoContent } };
                    }
                }
                else
                {
                    _Response = new HttpError("Error no se encontro información para validar !") { { "Status", false }, { "Codigo", HttpStatusCode.NotFound } };
                    FixedData.LogApi.Error("Error no se encontro información para validar !");
                }
            }
            catch (Exception ex)
            {
                string _MsgError = "Error al procesar la liquidación de impuestos. Motivo: " + ex.InnerException.Message;
                _Response = new HttpError(_MsgError) { { "Status", false }, { "Codigo", HttpStatusCode.InternalServerError } };
                FixedData.LogApi.Error(_MsgError);
            }

            return Request.CreateResponse(_Response);
        }

        [HttpPost, Route("comprobante_contabilizacion")]
        public HttpResponseMessage Post([FromBody]ComprobanteContabilizacion_Req objProcess)
        {
            HttpError _Response = null;
            try
            {
                if (objProcess != null)
                {
                    if (objProcess.idform_impuesto > 0)
                    {
                        #region ENVIAR DOCUMENTO DEL CREDITO AL PROVEEDOR
                        //--AQUI ESCRIBIMOS EL REQUEST DE LA TRX EN LOS LOGS
                        JavaScriptSerializer JsSerializer = new JavaScriptSerializer();
                        //--SERIALIZAR EL REQUEST
                        string JsonRequest = JsSerializer.Serialize(objProcess);
                        FixedData.LogApi.Warn("REQUEST COMPROBANTE CONTABILIZACION => " + JsonRequest);

                        //--AQUI ENVIAMOS EL PROCESO BACKGROUND JOB
                        BackgroundJob.Enqueue(() => Tasks.GetComprobanteContabilizacion(objProcess, "test"));

                        string _FechaHora = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                        string _Mensaje = "Señor usuario, el proceso de generar las liquidaciones se encuentra en ejecución, en unos minutos le estara llegando un correo con la confirmación de terminación del proceso";
                        _Response = new HttpError(_Mensaje) { { "Status", true }, { "fecha_proceso", _FechaHora }, { "Codigo", "00" } };
                        #endregion
                    }
                    else
                    {
                        _Response = new HttpError("Señor usuario, para realizar el proceso es necesario que seleccione un tipo de impuesto. !") { { "Status", false }, { "Codigo", HttpStatusCode.NoContent } };
                    }
                }
                else
                {
                    _Response = new HttpError("Error no se encontro información para validar !") { { "Status", false }, { "Codigo", HttpStatusCode.NotFound } };
                    FixedData.LogApi.Error("Error no se encontro información para validar !");
                }
            }
            catch (Exception ex)
            {
                string _MsgError = "Error al procesar la liquidación de impuestos. Motivo: " + ex.InnerException.Message;
                _Response = new HttpError(_MsgError) { { "Status", false }, { "Codigo", HttpStatusCode.InternalServerError } };
                FixedData.LogApi.Error(_MsgError);
            }

            return Request.CreateResponse(_Response);
        }

        //[HttpPost, Route("base_gravable")]
        //public async Task<HttpResponseMessage> Post([FromBody]BaseGravable_Req objProcess)
        //{
        //    HttpError _Response = null;
        //    try
        //    {
        //        if (objProcess != null)
        //        {
        //            if (objProcess.id_cliente.ToString().Trim().Length > 0)
        //            {
        //                #region METODO PARA LA TRX DE CONSULTA DEL COMERCIO
        //                //--AQUI ESCRIBIMOS EL REQUEST DE LA TRX EN LOS LOGS
        //                JavaScriptSerializer JsSerializer = new JavaScriptSerializer();
        //                //--SERIALIZAR EL REQUEST
        //                string JsonRequest = JsSerializer.Serialize(objProcess);
        //                FixedData.LogApi.Warn("REQUEST GENERAR BASE GRAVABLE => " + JsonRequest);

        //                //--AQUI REALIZAMOS EL PROCESO CON EL API DEL PROVEEDOR
        //                Transactions objTransactions = new Transactions();
        //                await objTransactions.ProcessBaseGravable(objProcess);

        //                _Response = new HttpError("PROCESO BASE GRAVABLE EN EJECUCION") { { "Status", true }, { "Codigo", "00" } };
        //                #endregion
        //            }
        //            else
        //            {
        //                _Response = new HttpError("El id del cliente es requerido. !") { { "Status", false }, { "Codigo", HttpStatusCode.NoContent } };
        //            }
        //        }
        //        else
        //        {
        //            _Response = new HttpError("Error no se encontro información para validar !") { { "Status", false }, { "Codigo", HttpStatusCode.NotFound } };
        //            FixedData.LogApi.Error("Error no se encontro información para validar !");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _Response = new HttpError("Error al generar la base gravable del id cliente: [" + objProcess.id_cliente + "]. Motivo: " + ex.Message) { { "Status", false }, { "Codigo", HttpStatusCode.InternalServerError } };
        //        FixedData.LogApi.Error("Error al generar la base gravable del id cliente: [" + objProcess.id_cliente + "]. Motivo: " + ex.Message);
        //    }

        //    return Request.CreateResponse(_Response);
        //    //return await Task.FromResult(Request.CreateResponse(_Response));
        //}

    }

    public static class Tasks
    {
        public static void GetBaseGravable(BaseGravable_Req objProcess, string s)
        {
            try
            {
                Transactions objTransactions = new Transactions();
                bool _Result = objTransactions.ProcessBaseGravable(objProcess);
                Console.WriteLine(s);
            }
            catch (Exception ex)
            {
                FixedData.LogApi.Error("Error con el hilo GetBaseGravable. Motivo: " + ex.Message);
            }
        }

        public static void GetLiquidarImpuesto(LiquidarImpuesto_Req objProcess, string s)
        {
            try
            {
                LiquidarImpuestos objTransactions = new LiquidarImpuestos();
                bool _Result = objTransactions.ProcessLiquidacion(objProcess);
                Console.WriteLine(s);
            }
            catch (Exception ex)
            {
                FixedData.LogApi.Error("Error con el hilo GetLiquidarImpuesto. Motivo: " + ex.Message);
            }
        }

        public static void GetComprobanteContabilizacion(ComprobanteContabilizacion_Req objProcess, string s)
        {
            try
            {
                LiquidarImpuestos objTransactions = new LiquidarImpuestos();
                //bool _Result = objTransactions.ProcessLiquidacion(objProcess);
                Console.WriteLine(s);
            }
            catch (Exception ex)
            {
                FixedData.LogApi.Error("Error con el hilo GetLiquidarImpuesto. Motivo: " + ex.Message);
            }
        }

        public static void InitializeJobs()
        {
            Console.WriteLine(DateTime.Now.ToString());
        }
    }

}
