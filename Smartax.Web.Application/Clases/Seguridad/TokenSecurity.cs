using System;
using Devart.Data.PostgreSql;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Configuration;
using log4net;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using RestSharp;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Smartax.Web.Application.Clases.Seguridad
{
    public class TokenSecurity
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);

        #region DEFINICION DE OBJETOS DE BASE DE DATOS
        IDbConnection myConnectionDb = null;
        string connString = "";

        MySqlCommand TheCommandMySQL = null;
        MySqlDataReader TheDataReaderMySQL = null;

        PgSqlCommand TheCommandPostgreSQL = null;
        PgSqlDataReader TheDataReaderPostgreSQL = null;
        PgSqlDataAdapter TheDataAdapterPostgreSQL;
        PgSqlParameter NpParam = null;
        IDbTransaction Transac = null;

        SqlCommand TheCommandSQLServer = null;
        SqlDataReader TheDataReaderSQLServer = null;

        OracleCommand TheCommandOracle = null;
        OracleDataReader TheDataReaderOracle = null;
        #endregion

        #region DEFINICION DE ATRIBUTOS Y PROPIEDADES
        public object IdToken { get; set; }
        public string NumeroToken { get; set; }
        public int IdUsuario { get; set; }
        public string FechaUso { get; set; }
        public string EstadoToken { get; set; }
        public string NumeroTelefono { get; set; }
        public string MensajeSms { get; set; }
        public string MostrarSeleccione { get; set; }
        public int TipoProceso { get; set; }
        public string MotorBaseDatos { get; set; }
        #endregion

        public bool GetGenerarToken(ref int _IdRegistro, ref string _MsgError)
        {
            bool retValor = false;
            try
            {
                #region OBJETO DE CONEXION A LA DB
                StringBuilder sSQL = new StringBuilder();
                //Aqui pasamos el string de conexion al objeto conection de la base de datos con la que se tiene que conectar
                if ((MotorBaseDatos.ToString().Trim().Equals("PostgreSQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
                    myConnectionDb = new PgSqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("MySQL")))
                {
                    connString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
                    myConnectionDb = new MySqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("SQLServer")))
                {
                    connString = ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                    myConnectionDb = new SqlConnection(connString);
                }
                else if ((MotorBaseDatos.ToString().Trim().Equals("Oracle")))
                {
                    connString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                    myConnectionDb = new OracleConnection(connString);
                }
                else
                {
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
                    return false;
                }

                //Aqui hacemos la debidas conexiones a la base de dato que esta configurada para trabajar 
                //Nota: Solo se permite una configuración de la base de datos en el web.config
                if (myConnectionDb.State != ConnectionState.Open)
                {
                    myConnectionDb.Open();
                }
                #endregion

                //Aqui hacemos los llamados de los sp o consultas a utilizar en la respectiva base de datos
                if (myConnectionDb is PgSqlConnection)
                {
                    #region REALIZAR PROCESO EN LA DB DE POSTGRESQL
                    IDbTransaction Transac = myConnectionDb.BeginTransaction();
                    TheCommandPostgreSQL = new PgSqlCommand("sp_web_crud_generar_token", (PgSqlConnection)myConnectionDb);
                    TheCommandPostgreSQL.CommandType = CommandType.StoredProcedure;
                    //Limpiar parametros
                    TheCommandPostgreSQL.Parameters.Clear();

                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idtoken", IdToken);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_numero_token", NumeroToken);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_idusuario", IdUsuario);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_fecha_uso", FechaUso);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_estado_token", EstadoToken);
                    TheCommandPostgreSQL.Parameters.AddWithValue("@p_in_tipo_proceso", TipoProceso);
                    PgSqlParameter _IdRegRetorno = new PgSqlParameter("@p_out_id_registro", SqlDbType.Int);
                    PgSqlParameter _CodRptaRetorno = new PgSqlParameter("@p_out_cod_rpta", SqlDbType.VarChar);
                    PgSqlParameter _MsgRptaRetorno = new PgSqlParameter("@p_out_msg_rpta", SqlDbType.VarChar);

                    //asignamos los parametros de retornos.
                    _IdRegRetorno.Direction = ParameterDirection.Output;
                    _CodRptaRetorno.Direction = ParameterDirection.Output;
                    _MsgRptaRetorno.Direction = ParameterDirection.Output;
                    TheCommandPostgreSQL.Parameters.Add(_IdRegRetorno);
                    TheCommandPostgreSQL.Parameters.Add(_CodRptaRetorno);
                    TheCommandPostgreSQL.Parameters.Add(_MsgRptaRetorno);

                    object ObjResult = new object();
                    ObjResult = TheCommandPostgreSQL.ExecuteScalar();
                    if (ObjResult != null)
                    {
                        if (Int32.Parse(ObjResult.ToString().Trim()) > 0)
                        {
                            Transac.Commit();
                            _IdRegistro = Int32.Parse(_IdRegRetorno.Value.ToString());
                            _MsgError = _MsgRptaRetorno.Value.ToString();
                            retValor = true;
                        }
                        else
                        {
                            _MsgError = _MsgRptaRetorno.Value.ToString();
                            retValor = false;
                        }
                    }
                    else
                    {
                        Transac.Rollback();
                        Transac.Connection.Close();
                        retValor = false;
                    }
                    #endregion
                }
                else if (myConnectionDb is MySqlConnection)
                {

                }
                else if (myConnectionDb is SqlConnection)
                {
                    //Base de datos SQL Server
                }
                else if (myConnectionDb is OracleConnection)
                {

                }
                else
                {
                    _MsgError = "No existe configurado un Motor de Base de Datos a Trabajar !";
                    _log.Error(_MsgError);
                    return false;
                }
            }
            catch (Exception ex)
            {
                retValor = false;
                _MsgError = "Error al registrar el proceso del Token. Motivo: " + ex.Message.ToString().Trim();
                _log.Error(_MsgError.ToString().Trim());
            }
            finally
            {
                #region FINALIZAR OBJETO DE CONEXION A LA DB
                //Aqui realizamos el cierre de los objetos de conexion abiertos
                if (myConnectionDb is PgSqlConnection)
                {
                    TheCommandPostgreSQL = null;
                }
                else if (myConnectionDb is MySqlConnection)
                {
                    TheCommandMySQL = null;
                }
                else if (myConnectionDb is SqlConnection)
                {
                    TheCommandSQLServer = null;
                }
                else if (myConnectionDb is OracleConnection)
                {
                    TheCommandOracle = null;
                }

                myConnectionDb.Close();
                myConnectionDb.Dispose();
                #endregion
            }

            return retValor;
        }

        public bool GetEnvioTokenSms(ref string _MsgError)
        {
            bool _Result = false;
            try
            {
                //--AQUI OBTENEMOS EL TOKEN 
                string _ResultTokenAut = GetTokenJwt();
                if (_ResultTokenAut.ToString().Trim().Length > 0)
                {
                    string[] _TokenAut = _ResultTokenAut.ToString().Trim().Split('|');
                    //---
                    if (_TokenAut[0].ToString().Trim().Equals("00"))
                    {
                        #region ENVIAR LA TRANSACCION AL API DEL SUSER
                        ////Variables de librería para hacer el POST
                        //var client = new RestClient(FixedData.BaseUrlSer);
                        //var request = new RestRequest(FixedData.ActionSegmentSer + "envio_sms", Method.POST);
                        //request.AddHeader("Content-Type", "application/json; charset=utf-8");
                        //request.AddHeader("Authorization", "Bearer " + _TokenAut[1].ToString().Trim() + "");
                        ////Variable de respuesta a petición POST
                        //var respPOST = new object();

                        ////Establecer el DTO para enviar, Setear datos
                        //EnvioTokenSms conReq = new EnvioTokenSms
                        //{
                        //    numero_celular = NumeroTelefono,
                        //    mensaje_sms = MensajeSms
                        //};

                        ////Agregar al Body de la petición
                        //request.AddJsonBody(conReq);
                        ////--AQUI MANDAMOS A ESCRIBIR EN LOS LOGS DE AUDITORIA
                        //JavaScriptSerializer js = new JavaScriptSerializer();
                        //string jsonRequest = js.Serialize(conReq);
                        //_log.Warn("REQUEST ENVIO DE TOKEN SMS => " + jsonRequest);

                        ////Llamado a Servicio Rest usando RestSharp            
                        ////Respuesta como objeto dinámico
                        //var response = client.ExecuteDynamic(request);
                        //respPOST = JsonConvert.DeserializeObject<EnvioTokenSms_Resp>(response.Content);

                        ////Validar código de respuesta
                        //JavaScriptSerializer JsSerializer = new JavaScriptSerializer();

                        /////--Validar código de respuesta
                        //string _CodRespuesta = "", _MsgRespuesta = "";
                        //if (((EnvioTokenSms_Resp)respPOST).Codigo != null)
                        //{
                        //    var JsonResult = JsSerializer.Deserialize<EnvioTokenSms_Resp>(response.Content);
                        //    string JsonResponse = JsSerializer.Serialize(JsonResult);
                        //    _log.Warn("RESPONSE VALIDAR TARJETA TULLAVE => " + JsonResponse);

                        //    //--OK
                        //    if (JsonResult.Codigo.ToString().Trim().Equals("00"))
                        //    {
                        //        #region AQUI OBTENEMOS DATOS DE LA VENTA RECARGA
                        //        //Valores de retornos.
                        //        _CodRespuesta = JsonResult.Codigo.ToString().Trim();
                        //        _MsgRespuesta = JsonResult.Message.ToString().Trim();
                        //        //--
                        //        _Result = true;
                        //        _MsgError = _CodRespuesta + "|" + _MsgRespuesta;
                        //        #endregion
                        //    }
                        //    else
                        //    {
                        //        #region ERROR AL REALIZAR LA TX DE CONSULTA
                        //        //Aqui retornamos los datos de la Respuesta.
                        //        _CodRespuesta = JsonResult.Codigo != null ? JsonResult.Codigo.ToString().Trim() : "ER";
                        //        _MsgRespuesta = JsonResult.Message != null ? JsonResult.Message.ToString().Trim() : "Error al realizar la transaccion de recarga !";
                        //        //--
                        //        _Result = false;
                        //        _MsgError = _CodRespuesta + "|" + _MsgRespuesta;
                        //        #endregion
                        //    }
                        //}
                        //else
                        //{
                        //    #region ERROR AL REALIZAR LA TX DE CONSULTA
                        //    //Aqui retornamos los datos de la Respuesta de MegaRed.
                        //    var tokenResponse = JsonConvert.DeserializeObject<EnvioTokenSms_Resp>(response.Content);
                        //    _CodRespuesta = tokenResponse.Codigo != null ? tokenResponse.Codigo.ToString().Trim() : "ER";
                        //    _MsgRespuesta = tokenResponse.Message != null ? tokenResponse.Message.ToString().Trim() : "Error desconocido del proveedor !";
                        //    //--
                        //    _Result = false;
                        //    _MsgError = _CodRespuesta + "|" + _MsgRespuesta;
                        //    #endregion
                        //}
                        #endregion
                    }
                    else
                    {
                        _Result = false;
                        _MsgError = _TokenAut[0].ToString().Trim() + "|" + _TokenAut[1].ToString().Trim();
                    }
                }
                else
                {
                    _Result = false;
                    _MsgError = "01" + "|" + "No se genero un token para realizar la transacción. Por favor validar con soporte técnico !";
                }
            }
            catch (Exception ex)
            {
                //Error
                _Result = false;
                string _Error = "-1|Error de Consulta|" + ex.Message;
            }
            return _Result;
        }

        public string GetTokenJwt()
        {
            string _Result = "";
            try
            {
                #region AQUI OBTENEMOS EL TOKEN CON EL PROVEEDOR PUNTO DE PAGO
                ////Variables de librería para hacer el POST
                //var client = new RestClient(FixedData.BaseUrlSer);
                //var request = new RestRequest(FixedData.ActionSegmentSer + "authenticate", Method.POST);
                //request.AddHeader("Content-Type", "application/json; charset=utf-8");
                //request.RequestFormat = DataFormat.Json;

                ////Variable de respuesta a petición POST
                //var respPOST = new object();

                /////Establecer el DTO para enviar, Setear datos
                //Token_Req conReq = new Token_Req
                //{
                //    IdComercio = null,
                //    Username = FixedData.UserWsPdP,
                //    Password = FixedData.PassWsPdP
                //};

                /////--Agregar al Body de la petición
                //request.AddJsonBody(conReq);

                /////--Llamado a Servicio Rest usando RestSharp            
                //var response = client.Execute(request);
                //respPOST = JsonConvert.DeserializeObject<Token_Resp>(response.Content);

                /////--Validar código de respuesta
                //string _CodRespuesta = "", _MsgRespuesta = "";
                //if (((Token_Resp)respPOST).Codigo != null)
                //{
                //    var tokenResponse = JsonConvert.DeserializeObject<Token_Resp>(response.Content);
                //    //--OK
                //    if (tokenResponse.Codigo.ToString().Trim().Equals("00"))
                //    {
                //        #region OBTENEMOS DATOS TX EXITOSA
                //        //Valores de retornos.
                //        _Result = tokenResponse.Codigo.ToString().Trim() + "|" + tokenResponse.token.ToString().Trim();
                //        #endregion
                //    }
                //    else
                //    {
                //        //--AQUI OBTENEMOS EL ERROR AL GENERAR EL TOKEN
                //        _Result = tokenResponse.Codigo.ToString().Trim() + "|" + tokenResponse.Message.ToString().Trim();
                //    }
                //}
                //else
                //{
                //    #region ERROR AL REALIZAR LA TX DE CONSULTA
                //    //Aqui retornamos los datos de la Respuesta de MegaRed.
                //    var tokenResponse = JsonConvert.DeserializeObject<Token_Resp>(response.Content);
                //    _CodRespuesta = tokenResponse.Codigo != null ? tokenResponse.Codigo.ToString().Trim() : "ER";
                //    _MsgRespuesta = tokenResponse.Message != null ? tokenResponse.Message.ToString().Trim() : "Error desconocido del proveedor !";
                //    _Result = _CodRespuesta + "|" + _MsgRespuesta;
                //    #endregion
                //}
                #endregion
            }
            catch (Exception ex)
            {
                _Result = "EX|Error al generar el token de seguridad. Motivo: " + ex.Message;
            }

            return _Result;
        }

        public static bool OnValidationCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

    }
}