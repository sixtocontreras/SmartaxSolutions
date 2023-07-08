using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using Devart.Data.PostgreSql;
using MySql.Data.MySqlClient;
using System.Text;
using Smartax.WebApi.Services.Clases.Seguridad;
using System.Configuration;
using Smartax.WebApi.Services.Models;
using System.Web.Http;
using System.Threading.Tasks;

namespace Smartax.WebApi.Services.Clases.Procesos
{
    public class Transactions
    {
        EnviarEmails objEmails = new EnviarEmails();
        const string quote = "\"";

        #region DEFINICION DE METODOS PARA PROCESAR BASE GRAVABLE
        public bool ProcessBaseGravable(BaseGravable_Req objBase)
        {
            bool _Result = false;
            try
            {
                FixedData.LogApi.Info("PROCESO INICIADO FECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                //--INSTANCIAMOS EL OBJETO DE CLASE
                ProcessDb objProcessDb = new ProcessDb();
                objProcessDb.TipoConsulta = 5;
                objProcessDb.IdCliente = objBase.id_cliente;
                objProcessDb.IdEstablecimientoPadre = null;
                objProcessDb.IdEstado = 1;
                //--
                DataTable dtEstablecimientos = new DataTable();
                dtEstablecimientos = objProcessDb.GetEstablecimientosCliente();
                if (dtEstablecimientos != null)
                {
                    if (dtEstablecimientos.Rows.Count > 0)
                    {
                        #region AQUI REALIZAMOS EL PROCESO DE LA BASE GRAVABLE POR MUNICIPIO - ESTABLECIMIENTOS
                        //--AQUI MANDAMOS A GENERAR LA TABLA PARA GUARDAR LA INFO DE LA BASE GRAVABLE
                        DataTable dtGuardarBaseGravable = new DataTable();
                        dtGuardarBaseGravable = this.GetTablaDatos();
                        if (dtGuardarBaseGravable != null)
                        {
                            int _ContadorEstablecimiento = 0;
                            foreach (DataRow rowEst in dtEstablecimientos.Rows)
                            {
                                #region OBTENEMOS DATOS DE ESTABLECIMIENTO PARA GENERAR BASE GRAVABLE
                                //--
                                _ContadorEstablecimiento++;
                                int _IdClienteEstablecimiento = Int32.Parse(rowEst["idcliente_establecimiento"].ToString().Trim());
                                int _IdMunicipio = Int32.Parse(rowEst["id_municipio"].ToString().Trim());
                                string _CodigoDane = rowEst["codigo_dane"].ToString().Trim();

                                //--AQUI INSERTAMOS LOS DATOS EN EL DATA TABLE
                                DataRow Fila = null;
                                Fila = dtGuardarBaseGravable.NewRow();
                                Fila["idbase_gravable"] = dtGuardarBaseGravable.Rows.Count + 1;
                                Fila["id_municipio"] = _IdMunicipio;
                                Fila["idformulario_impuesto"] = objBase.idform_impuesto;
                                Fila["id_cliente"] = objBase.id_cliente;
                                Fila["idcliente_establecimiento"] = _IdClienteEstablecimiento;
                                Fila["codigo_dane"] = _CodigoDane;
                                Fila["anio_gravable"] = objBase.anio_gravable;
                                Fila["mes_ef"] = objBase.mes_ef;
                                Fila["version_ef"] = objBase.version_ef;

                                //--
                                objProcessDb.TipoConsulta = 2;  //--objBase.tipo_consulta;
                                objProcessDb.IdCliente = objBase.id_cliente;
                                objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                objProcessDb.IdFormularioImpuesto = objBase.idform_impuesto;
                                objProcessDb.AnioGravable = objBase.anio_gravable;
                                objProcessDb.IdFormConfiguracion = null;
                                objProcessDb.IdPuc = null;
                                objProcessDb.AnioGravable = null;
                                objProcessDb.MesEf = null;
                                objProcessDb.IdEstado = 1;
                                //--
                                DataTable dtBaseGravable = new DataTable();
                                dtBaseGravable = objProcessDb.GetBaseGravable();
                                int _ContadorRow = 0;
                                //--
                                if (dtBaseGravable != null)
                                {
                                    if (dtBaseGravable.Rows.Count > 0)
                                    {
                                        #region AQUI REALIZAMOS EL PROCESO DE GUARDAR EN LA BASE DE DATOS
                                        //--AQUI RECORREMOS EL DATATABLE PARA MOSTRAR LAS ACTIVIDADES ECONOMICAS.
                                        double _SumValorRenglon8 = 0, _SumValorRenglon9 = 0, _SumValorRenglon10 = 0;
                                        double _SumValorRenglon11 = 0, _SumValorRenglon12 = 0, _SumValorRenglon13 = 0;
                                        double _SumValorRenglon14 = 0, _SumValorRenglon15 = 0, _SumValorRenglon26 = 0;
                                        double _SumValorRenglon27 = 0, _SumValorRenglon28 = 0, _SumValorRenglon29 = 0;
                                        double _SumValorRenglon30 = 0, _SumValorRenglon31 = 0, _SumValorRenglon32 = 0;
                                        double _SumValorRenglon33 = 0, _SumValorRenglon34 = 0, _SumValorRenglon35 = 0;
                                        double _SumValorRenglon36 = 0, _SumValorRenglon37 = 0;
                                        foreach (DataRow rowItem in dtBaseGravable.Rows)
                                        {
                                            #region VALORES OBTENIDOS DE LA BASE GRAVABLE
                                            _ContadorRow++;
                                            int _NumRenglon = Int32.Parse(rowItem["numero_renglon"].ToString().Trim());
                                            string _CodigoCuenta = rowItem["codigo_cuenta"].ToString().Trim();
                                            string _SaldoInicial = rowItem["saldo_inicial"].ToString().Trim();
                                            string _MovDebito = rowItem["mov_debito"].ToString().Trim();
                                            string _MovCredito = rowItem["mov_credito"].ToString().Trim();
                                            string _SaldoFinal = rowItem["saldo_final"].ToString().Trim();
                                            string _ValorExtracontable1 = rowItem["valor_extracontable"].ToString().Trim().Replace("-", "");
                                            double _ValorExtracontable = Double.Parse(_ValorExtracontable1);

                                            //--AQUI MANDAMOS A OBTENER LOS VALORES DEL ESTADO FINANCIERO POR CLIENTE
                                            objProcessDb.TipoConsulta = 1;
                                            objProcessDb.NumeroRenglon = _NumRenglon;
                                            objProcessDb.IdCliente = objBase.id_cliente != null ? objBase.id_cliente.ToString().Trim() : null;
                                            objProcessDb.AnioGravable = objBase.anio_gravable;
                                            objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                            //objProcessDb.IdClienteEstablecimiento = objBase.idcliente_establecimiento;
                                            objProcessDb.SaldoInicial = _SaldoInicial;
                                            objProcessDb.MovDebito = _MovDebito;
                                            objProcessDb.MovCredito = _MovCredito;
                                            objProcessDb.SaldoFinal = _SaldoFinal;
                                            objProcessDb.CodigoCuenta = _CodigoCuenta;
                                            objProcessDb.MesEf = objBase.mes_ef.ToString().Trim();

                                            //--AQUI OBTENEMOS EL VALOR A DEFINIR EN EL RENGLON DEL FORM.
                                            double _ValorTotal = 0;
                                            if (_SaldoInicial == "S" || _MovDebito == "S" || _MovCredito == "S" || _SaldoFinal == "S")
                                            {
                                                List<string> _ArrayDatos = objProcessDb.GetEstadoFinanciero();
                                                if (_ArrayDatos.Count > 0)
                                                {
                                                    string _ValorCuenta = _ArrayDatos[1].ToString().Trim();
                                                    _ValorTotal = (Double.Parse(_ValorCuenta) + _ValorExtracontable);
                                                    //--
                                                    FixedData.LogApi.Warn("CONTADOR => " + _ContadorRow + ", No. RENGLON => " + _NumRenglon + ", COD. CUENTA => " + _CodigoCuenta + ", VALOR CUENTA => " + _ValorCuenta);
                                                }
                                            }
                                            else
                                            {
                                                _ValorTotal = _ValorExtracontable;
                                            }
                                            #endregion

                                            #region AQUI OBTENEMOS EL VALOR DE LA BASE BRAVABLE MEDIANTE UN SWITCH
                                            //--AQUI OBTENEMOS LOS DATOS 
                                            switch (_NumRenglon)
                                            {
                                                case 8:
                                                    _SumValorRenglon8 = _SumValorRenglon8 + _ValorTotal;
                                                    break;
                                                case 9:
                                                    _SumValorRenglon9 = _SumValorRenglon9 + _ValorTotal;
                                                    break;
                                                case 11:
                                                    _SumValorRenglon11 = _SumValorRenglon11 + _ValorTotal;
                                                    break;
                                                case 12:
                                                    _SumValorRenglon12 = _SumValorRenglon12 + _ValorTotal;
                                                    break;
                                                case 13:
                                                    _SumValorRenglon13 = _SumValorRenglon13 + _ValorTotal;
                                                    break;
                                                case 14:
                                                    _SumValorRenglon14 = _SumValorRenglon14 + _ValorTotal;
                                                    break;
                                                case 15:
                                                    _SumValorRenglon15 = _SumValorRenglon15 + _ValorTotal;
                                                    break;
                                                case 26:
                                                    _SumValorRenglon26 = _SumValorRenglon26 + _ValorTotal;
                                                    break;
                                                case 27:
                                                    _SumValorRenglon27 = _SumValorRenglon27 + _ValorTotal;
                                                    break;
                                                case 28:
                                                    _SumValorRenglon28 = _SumValorRenglon28 + _ValorTotal;
                                                    break;
                                                case 29:
                                                    _SumValorRenglon29 = _SumValorRenglon29 + _ValorTotal;
                                                    break;
                                                case 30:
                                                    _SumValorRenglon30 = _SumValorRenglon30 + _ValorTotal;
                                                    break;
                                                case 31:
                                                    _SumValorRenglon31 = _SumValorRenglon31 + _ValorTotal;
                                                    break;
                                                case 32:
                                                    _SumValorRenglon32 = _SumValorRenglon32 + _ValorTotal;
                                                    break;
                                                case 33:
                                                    _SumValorRenglon33 = _SumValorRenglon33 + _ValorTotal;
                                                    break;
                                                case 34:
                                                    _SumValorRenglon34 = _SumValorRenglon34 + _ValorTotal;
                                                    break;
                                                case 35:
                                                    _SumValorRenglon35 = _SumValorRenglon35 + _ValorTotal;
                                                    break;
                                                case 36:
                                                    _SumValorRenglon36 = _SumValorRenglon36 + _ValorTotal;
                                                    break;
                                                case 37:
                                                    _SumValorRenglon37 = _SumValorRenglon37 + _ValorTotal;
                                                    break;
                                                default:
                                                    break;
                                            }
                                            #endregion
                                        }

                                        #region REGISTRAMOS LOS DATOS EN EL DATATABLE
                                        //--AQUI REGISTRAMOS LOS DATOS EN EL DATATABLE
                                        Fila["valor_renglon8"] = _SumValorRenglon8;
                                        double _SumValorRenglon9Aux = (_SumValorRenglon8 - _SumValorRenglon9);
                                        Fila["valor_renglon9"] = _SumValorRenglon9Aux;
                                        _SumValorRenglon10 = (_SumValorRenglon8 - _SumValorRenglon9Aux);
                                        Fila["valor_renglon10"] = _SumValorRenglon10;
                                        Fila["valor_renglon11"] = _SumValorRenglon11;
                                        Fila["valor_renglon12"] = _SumValorRenglon12;
                                        Fila["valor_renglon13"] = _SumValorRenglon13;
                                        Fila["valor_renglon14"] = _SumValorRenglon14;
                                        Fila["valor_renglon15"] = _SumValorRenglon15;
                                        Fila["valor_renglon16"] = 0;
                                        Fila["valor_renglon26"] = _SumValorRenglon26;
                                        Fila["valor_renglon27"] = _SumValorRenglon27;
                                        Fila["valor_renglon28"] = _SumValorRenglon28;
                                        Fila["valor_renglon29"] = _SumValorRenglon29;
                                        Fila["valor_renglon30"] = _SumValorRenglon30;
                                        Fila["valor_renglon31"] = _SumValorRenglon31;
                                        Fila["valor_renglon32"] = _SumValorRenglon32;
                                        Fila["valor_renglon33"] = _SumValorRenglon33;
                                        Fila["valor_renglon34"] = _SumValorRenglon34;
                                        Fila["valor_renglon35"] = _SumValorRenglon35;
                                        Fila["valor_renglon36"] = _SumValorRenglon36;
                                        Fila["valor_renglon37"] = _SumValorRenglon37;
                                        dtGuardarBaseGravable.Rows.Add(Fila);
                                        #endregion
                                        //--
                                        #endregion
                                    }
                                    else
                                    {
                                        _Result = false;
                                        string _MsgError = "NO SE OBTUVO DATOS DE LA BASE GRAVABLE";
                                        //--Enviamos la respuesta al cliente
                                        //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "05" } };

                                        //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                                        FixedData.LogApi.Error(_MsgError.ToString().Trim());
                                    }
                                }
                                else
                                {
                                    _Result = false;
                                    string _MsgError = "ERROR AL OBTENER LOS DATOS DE LA BASE GRAVABLE";
                                    //--Enviamos la respuesta al cliente
                                    //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "04" } };

                                    //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                                    FixedData.LogApi.Error(_MsgError.ToString().Trim());
                                }
                                #endregion
                            }

                            //--AQUI MANDAMOS A GUARDAR LOS DATOS EN LA TABLA DE BASE GRAVABLE
                            if (_ContadorEstablecimiento > 0)
                            {
                                #region AQUI OBTENEMOS LOS DATOS DEL DATATABLE PARA ENVIARLOS A LA DB
                                if (dtGuardarBaseGravable.Rows.Count > 0)
                                {
                                    int _IdEstado = 1;
                                    string _ArrayData = "";
                                    foreach (DataRow rowItem in dtGuardarBaseGravable.Rows)
                                    {
                                        #region OBTENER DATOS DEL DATATBALE PARA ENVIAR A LA DB
                                        int _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                                        int _IdFormImpuesto = Int32.Parse(rowItem["idformulario_impuesto"].ToString().Trim());
                                        int _IdCliente = Int32.Parse(rowItem["id_cliente"].ToString().Trim());
                                        int _IdClienteEstablecimiento = Int32.Parse(rowItem["idcliente_establecimiento"].ToString().Trim());
                                        string _CodigoDane = rowItem["codigo_dane"].ToString().Trim();
                                        //int _AnioGravable = Int32.Parse(rowItem["anio_gravable"].ToString().Trim());
                                        //string _MesEf = rowItem["mes_ef"].ToString().Trim();
                                        string _ValorRenglon8 = rowItem["valor_renglon8"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon9 = rowItem["valor_renglon9"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon10 = rowItem["valor_renglon10"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon11 = rowItem["valor_renglon11"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon12 = rowItem["valor_renglon12"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon13 = rowItem["valor_renglon13"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon14 = rowItem["valor_renglon14"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon15 = rowItem["valor_renglon15"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon16 = rowItem["valor_renglon16"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon26 = rowItem["valor_renglon26"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon27 = rowItem["valor_renglon27"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon28 = rowItem["valor_renglon28"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon29 = rowItem["valor_renglon29"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon30 = rowItem["valor_renglon30"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon31 = rowItem["valor_renglon31"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon32 = rowItem["valor_renglon32"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon33 = rowItem["valor_renglon33"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon34 = rowItem["valor_renglon34"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon35 = rowItem["valor_renglon35"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon36 = rowItem["valor_renglon36"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon37 = rowItem["valor_renglon37"].ToString().Trim().Replace(",", ".");
                                        string _VersionEf = rowItem["version_ef"].ToString().Trim();

                                        //--ARMAMOS EL ARRAY DE LOS DATOS A CARGAR
                                        if (_ArrayData.ToString().Trim().Length > 0)
                                        {
                                            _ArrayData = _ArrayData.ToString().Trim() + ", " + quote + "(" + _IdMunicipio + "," + _IdFormImpuesto + "," + _IdCliente + "," + _IdClienteEstablecimiento + "," + _CodigoDane + "," + _ValorRenglon8 + "," + _ValorRenglon9 + "," + _ValorRenglon10 + "," + _ValorRenglon11 + "," + _ValorRenglon12 + "," + _ValorRenglon13 + "," + _ValorRenglon14 + "," + _ValorRenglon15 + "," + _ValorRenglon16 + "," + _ValorRenglon26 + "," + _ValorRenglon27 + "," + _ValorRenglon28 + "," + _ValorRenglon29 + "," + _ValorRenglon30 + "," + _ValorRenglon31 + "," + _ValorRenglon32 + "," + _ValorRenglon33 + "," + _ValorRenglon34 + "," + _ValorRenglon35 + "," + _ValorRenglon36 + "," + _ValorRenglon37 + "," + _IdEstado + ")" + quote;
                                        }
                                        else
                                        {
                                            _ArrayData = quote + "(" + _IdMunicipio + "," + _IdFormImpuesto + "," + _IdCliente + "," + _IdClienteEstablecimiento + "," + _CodigoDane + "," + _ValorRenglon8 + "," + _ValorRenglon9 + "," + _ValorRenglon10 + "," + _ValorRenglon11 + "," + _ValorRenglon12 + "," + _ValorRenglon13 + "," + _ValorRenglon14 + "," + _ValorRenglon15 + "," + _ValorRenglon16 + "," + _ValorRenglon26 + "," + _ValorRenglon27 + "," + _ValorRenglon28 + "," + _ValorRenglon29 + "," + _ValorRenglon30 + "," + _ValorRenglon31 + "," + _ValorRenglon32 + "," + _ValorRenglon33 + "," + _ValorRenglon34 + "," + _ValorRenglon35 + "," + _ValorRenglon36 + "," + _ValorRenglon37 + "," + _IdEstado + ")" + quote;
                                        }
                                        #endregion
                                    }

                                    if (_ArrayData.ToString().Trim().Length > 0)
                                    {
                                        FixedData.LogApi.Warn("DATA A CARGAR EN LA DB => " + _ArrayData.ToString().Trim());
                                        //--
                                        objProcessDb.IdClienteEF = objBase.idcliente_ef;
                                        objProcessDb.AnioGravable = objBase.anio_gravable;
                                        objProcessDb.MesEf = objBase.mes_ef;
                                        objProcessDb.VersionEf = objBase.version_ef;
                                        objProcessDb.ArrayData = _ArrayData;
                                        objProcessDb.IdUsuario = objBase.id_usuario;

                                        int _IdRegistro = 0;
                                        string _MsgError = "";
                                        if (objProcessDb.AddLoadBaseGravable(ref _IdRegistro, ref _MsgError))
                                        {
                                            #region PROCESO PARA EL ENVIO DEL CORREO
                                            _Result = true;
                                            //--
                                            objEmails.EmailPara = FixedData.EnvioEmail.ToString().Trim();
                                            objEmails.EmailCopia = FixedData.EnvioEmailCopia.ToString().Trim();
                                            objEmails.Asunto = "REF.: PROCESO BASE GRAVABLE";
                                            objEmails.Detalle = "Señor usuario, para informarle que el proceso de Generación de la Base Gravable del Estado Financiero [Año Gravable: " + objBase.anio_gravable + ", Mes EF: " + objBase.mes_ef + "], ha sido procesado de forma exitosa.";

                                            string _MsgErrorEmail = "";
                                            if (!objEmails.SendEmailConCopia(ref _MsgErrorEmail))
                                            {
                                                FixedData.LogApi.Error("ERROR AL ENVIAR EL CORREO. MOTIVO: " + _MsgErrorEmail);
                                            }

                                            //--Enviamos la respuesta al cliente
                                            //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "00" } };

                                            //--
                                            FixedData.LogApi.Info("PROCESO FINALIZADO: CANTIDAD ESTABLECIMIENTOS: [" + _ContadorEstablecimiento + "] FECHA & HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                                            _ContadorEstablecimiento = 0;
                                            return _Result;
                                            #endregion
                                        }
                                        else
                                        {
                                            #region PROCESO PARA EL ENVIO DEL CORREO
                                            //--
                                            objEmails.EmailPara = FixedData.EnvioEmail.ToString().Trim();
                                            objEmails.EmailCopia = FixedData.EnvioEmailCopia.ToString().Trim();
                                            objEmails.Asunto = "REF.: ERROR PROCESO BASE GRAVABLE";
                                            objEmails.Detalle = "Señor usuario, ocurrio un error con el proceso de Generación de la Base Gravable del Estado Financiero [Año Gravable: " + objBase.anio_gravable + ", Mes EF: " + objBase.mes_ef + "], por favor validar con soporte técnico para ver que sucede. Posible Motivo: " + _MsgError;

                                            string _MsgErrorEmail = "";
                                            if (!objEmails.SendEmailConCopia(ref _MsgErrorEmail))
                                            {
                                                FixedData.LogApi.Error("ERROR AL ENVIAR EL CORREO. MOTIVO: " + _MsgErrorEmail);
                                            }

                                            //--
                                            _ContadorEstablecimiento = 0;
                                            FixedData.LogApi.Error("PROCESO FINALIZADO PERO OCURRIO UN ERROR. MOTIVO: " + _MsgError + ", FECHA & HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    _Result = false;
                                    string _MsgError = "NO FUE POSIBLE CARGAR LA INFO DE LA BASE GRAVABLE A LA DB";
                                    //--Enviamos la respuesta al cliente
                                    //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "05" } };

                                    //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                                    FixedData.LogApi.Error(_MsgError.ToString().Trim());
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            _Result = false;
                            string _MsgError = "ERROR AL CREAR EL DATA TABLE PARA GUARDAR LOS DATOS DE LA BASE GRAVABLE";
                            //--Enviamos la respuesta al cliente
                            //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "03" } };

                            //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                            FixedData.LogApi.Error(_MsgError.ToString().Trim());
                        }
                        #endregion
                    }
                    else
                    {
                        _Result = false;
                        string _MsgError = "NO SE ENCONTRARON ESTABLECIMIENTOS PARA EL ID CLIENTE: " + objBase.id_cliente;
                        //--Enviamos la respuesta al cliente
                        //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "02" } };

                        //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                        FixedData.LogApi.Error(_MsgError.ToString().Trim());
                    }
                }
                else
                {
                    _Result = false;
                    string _MsgError = "ERROR AL OBTENER LOS DATOS DE ESTABLECIMIENTOS PARA EL ID CLIENTE: " + objBase.id_cliente;
                    //--Enviamos la respuesta al cliente
                    //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "01" } };

                    //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                    FixedData.LogApi.Error(_MsgError.ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                _Result = false;
                string _MsgError = "SE PRODUJO UNA EXCEPCIÓN AL REALIZAR EL PROCESDO DE LA BASE GRAVABLE. MOTIVO: " + ex.Message;
                //--Enviamos la respuesta al cliente
                //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "EX" } };

                //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                FixedData.LogApi.Error(_MsgError.ToString().Trim());
            }

            return _Result;
        }

        public bool ProcessBaseGravable_Old(BaseGravable_Req objBase)
        {
            bool _Result = false;
            try
            {
                FixedData.LogApi.Info("PROCESO INICIADO FECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                //--INSTANCIAMOS EL OBJETO DE CLASE
                ProcessDb objProcessDb = new ProcessDb();
                objProcessDb.TipoConsulta = 5;
                objProcessDb.IdCliente = objBase.id_cliente;
                objProcessDb.IdEstablecimientoPadre = null;
                objProcessDb.IdEstado = 1;
                //--
                DataTable dtEstablecimientos = new DataTable();
                dtEstablecimientos = objProcessDb.GetEstablecimientosCliente();
                if (dtEstablecimientos != null)
                {
                    if (dtEstablecimientos.Rows.Count > 0)
                    {
                        #region AQUI REALIZAMOS EL PROCESO DE LA BASE GRAVABLE POR MUNICIPIO - ESTABLECIMIENTOS
                        //--AQUI MANDAMOS A GENERAR LA TABLA PARA GUARDAR LA INFO DE LA BASE GRAVABLE
                        DataTable dtGuardarBaseGravable = new DataTable();
                        dtGuardarBaseGravable = this.GetTablaDatos();
                        if (dtGuardarBaseGravable != null)
                        {
                            int _ContadorEstablecimiento = 0;
                            foreach (DataRow rowEst in dtEstablecimientos.Rows)
                            {
                                #region OBTENEMOS DATOS DE ESTABLECIMIENTO PARA GENERAR BASE GRAVABLE
                                //--
                                _ContadorEstablecimiento++;
                                int _IdClienteEstablecimiento = Int32.Parse(rowEst["idcliente_establecimiento"].ToString().Trim());
                                int _IdMunicipio = Int32.Parse(rowEst["id_municipio"].ToString().Trim());
                                string _CodigoDane = rowEst["codigo_dane"].ToString().Trim();

                                //--AQUI INSERTAMOS LOS DATOS EN EL DATA TABLE
                                DataRow Fila = null;
                                Fila = dtGuardarBaseGravable.NewRow();
                                Fila["idbase_gravable"] = dtGuardarBaseGravable.Rows.Count + 1;
                                Fila["id_municipio"] = _IdMunicipio;
                                Fila["idformulario_impuesto"] = objBase.idform_impuesto;
                                Fila["id_cliente"] = objBase.id_cliente;
                                Fila["idcliente_establecimiento"] = _IdClienteEstablecimiento;
                                Fila["codigo_dane"] = _CodigoDane;
                                Fila["anio_gravable"] = objBase.anio_gravable;
                                Fila["mes_ef"] = objBase.mes_ef;
                                Fila["version_ef"] = objBase.version_ef;

                                //--
                                objProcessDb.TipoConsulta = 2;  //--objBase.tipo_consulta;
                                objProcessDb.IdCliente = objBase.id_cliente;
                                objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                objProcessDb.IdFormularioImpuesto = objBase.idform_impuesto;
                                objProcessDb.AnioGravable = objBase.anio_gravable;
                                objProcessDb.IdFormConfiguracion = null;
                                objProcessDb.IdPuc = null;
                                objProcessDb.AnioGravable = null;
                                objProcessDb.MesEf = null;
                                objProcessDb.IdEstado = 1;
                                //--
                                DataTable dtBaseGravable = new DataTable();
                                dtBaseGravable = objProcessDb.GetBaseGravable();
                                int _ContadorRow = 0;
                                //--
                                if (dtBaseGravable != null)
                                {
                                    if (dtBaseGravable.Rows.Count > 0)
                                    {
                                        #region AQUI REALIZAMOS EL PROCESO DE GUARDAR EN LA BASE DE DATOS
                                        //--AQUI RECORREMOS EL DATATABLE PARA MOSTRAR LAS ACTIVIDADES ECONOMICAS.
                                        double _SumValorRenglon8 = 0, _SumValorRenglon9 = 0, _SumValorRenglon10 = 0;
                                        double _SumValorRenglon11 = 0, _SumValorRenglon12 = 0, _SumValorRenglon13 = 0;
                                        double _SumValorRenglon14 = 0, _SumValorRenglon15 = 0, _SumValorRenglon26 = 0;
                                        double _SumValorRenglon27 = 0, _SumValorRenglon28 = 0, _SumValorRenglon29 = 0;
                                        double _SumValorRenglon30 = 0, _SumValorRenglon31 = 0, _SumValorRenglon32 = 0;
                                        double _SumValorRenglon33 = 0, _SumValorRenglon34 = 0, _SumValorRenglon35 = 0;
                                        double _SumValorRenglon36 = 0, _SumValorRenglon37 = 0;
                                        foreach (DataRow rowItem in dtBaseGravable.Rows)
                                        {
                                            #region VALORES OBTENIDOS DE LA BASE GRAVABLE
                                            _ContadorRow++;
                                            int _NumRenglon = Int32.Parse(rowItem["numero_renglon"].ToString().Trim());
                                            string _CodigoCuenta = rowItem["codigo_cuenta"].ToString().Trim();
                                            string _SaldoInicial = rowItem["saldo_inicial"].ToString().Trim();
                                            string _MovDebito = rowItem["mov_debito"].ToString().Trim();
                                            string _MovCredito = rowItem["mov_credito"].ToString().Trim();
                                            string _SaldoFinal = rowItem["saldo_final"].ToString().Trim();
                                            string _ValorExtracontable1 = rowItem["valor_extracontable"].ToString().Trim().Replace("-", "");
                                            double _ValorExtracontable = Double.Parse(_ValorExtracontable1);

                                            //--AQUI MANDAMOS A OBTENER LOS VALORES DEL ESTADO FINANCIERO POR CLIENTE
                                            objProcessDb.TipoConsulta = 1;
                                            objProcessDb.NumeroRenglon = _NumRenglon;
                                            objProcessDb.IdCliente = objBase.id_cliente != null ? objBase.id_cliente.ToString().Trim() : null;
                                            objProcessDb.AnioGravable = objBase.anio_gravable;
                                            objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                            //objProcessDb.IdClienteEstablecimiento = objBase.idcliente_establecimiento;
                                            objProcessDb.SaldoInicial = _SaldoInicial;
                                            objProcessDb.MovDebito = _MovDebito;
                                            objProcessDb.MovCredito = _MovCredito;
                                            objProcessDb.SaldoFinal = _SaldoFinal;
                                            objProcessDb.CodigoCuenta = _CodigoCuenta;
                                            objProcessDb.MesEf = objBase.mes_ef.ToString().Trim();

                                            //--AQUI OBTENEMOS EL VALOR A DEFINIR EN EL RENGLON DEL FORM.
                                            double _ValorTotal = 0;
                                            if (_SaldoInicial == "S" || _MovDebito == "S" || _MovCredito == "S" || _SaldoFinal == "S")
                                            {
                                                List<string> _ArrayDatos = objProcessDb.GetEstadoFinanciero();
                                                if (_ArrayDatos.Count > 0)
                                                {
                                                    string _ValorCuenta = _ArrayDatos[1].ToString().Trim();
                                                    _ValorTotal = (Double.Parse(_ValorCuenta) + _ValorExtracontable);
                                                    //--
                                                    FixedData.LogApi.Warn("CONTADOR => " + _ContadorRow + ", No. RENGLON => " + _NumRenglon + ", COD. CUENTA => " + _CodigoCuenta + ", VALOR CUENTA => " + _ValorCuenta);
                                                }
                                            }
                                            else
                                            {
                                                _ValorTotal = _ValorExtracontable;
                                            }
                                            #endregion

                                            #region AQUI OBTENEMOS EL VALOR DE LA BASE BRAVABLE MEDIANTE UN SWITCH
                                            //--AQUI OBTENEMOS LOS DATOS 
                                            switch (_NumRenglon)
                                            {
                                                case 8:
                                                    _SumValorRenglon8 = _SumValorRenglon8 + _ValorTotal;
                                                    break;
                                                case 9:
                                                    _SumValorRenglon9 = _SumValorRenglon9 + _ValorTotal;
                                                    break;
                                                case 11:
                                                    _SumValorRenglon11 = _SumValorRenglon11 + _ValorTotal;
                                                    break;
                                                case 12:
                                                    _SumValorRenglon12 = _SumValorRenglon12 + _ValorTotal;
                                                    break;
                                                case 13:
                                                    _SumValorRenglon13 = _SumValorRenglon13 + _ValorTotal;
                                                    break;
                                                case 14:
                                                    _SumValorRenglon14 = _SumValorRenglon14 + _ValorTotal;
                                                    break;
                                                case 15:
                                                    _SumValorRenglon15 = _SumValorRenglon15 + _ValorTotal;
                                                    break;
                                                case 26:
                                                    _SumValorRenglon26 = _SumValorRenglon26 + _ValorTotal;
                                                    break;
                                                case 27:
                                                    _SumValorRenglon27 = _SumValorRenglon27 + _ValorTotal;
                                                    break;
                                                case 28:
                                                    _SumValorRenglon28 = _SumValorRenglon28 + _ValorTotal;
                                                    break;
                                                case 29:
                                                    _SumValorRenglon29 = _SumValorRenglon29 + _ValorTotal;
                                                    break;
                                                case 30:
                                                    _SumValorRenglon30 = _SumValorRenglon30 + _ValorTotal;
                                                    break;
                                                case 31:
                                                    _SumValorRenglon31 = _SumValorRenglon31 + _ValorTotal;
                                                    break;
                                                case 32:
                                                    _SumValorRenglon32 = _SumValorRenglon32 + _ValorTotal;
                                                    break;
                                                case 33:
                                                    _SumValorRenglon33 = _SumValorRenglon33 + _ValorTotal;
                                                    break;
                                                case 34:
                                                    _SumValorRenglon34 = _SumValorRenglon34 + _ValorTotal;
                                                    break;
                                                case 35:
                                                    _SumValorRenglon35 = _SumValorRenglon35 + _ValorTotal;
                                                    break;
                                                case 36:
                                                    _SumValorRenglon36 = _SumValorRenglon36 + _ValorTotal;
                                                    break;
                                                case 37:
                                                    _SumValorRenglon37 = _SumValorRenglon37 + _ValorTotal;
                                                    break;
                                                default:
                                                    break;
                                            }
                                            #endregion
                                        }

                                        #region REGISTRAMOS LOS DATOS EN EL DATATABLE
                                        //--AQUI REGISTRAMOS LOS DATOS EN EL DATATABLE
                                        Fila["valor_renglon8"] = _SumValorRenglon8;
                                        double _SumValorRenglon9Aux = (_SumValorRenglon8 - _SumValorRenglon9);
                                        Fila["valor_renglon9"] = _SumValorRenglon9Aux;
                                        _SumValorRenglon10 = (_SumValorRenglon8 - _SumValorRenglon9Aux);
                                        Fila["valor_renglon10"] = _SumValorRenglon10;
                                        Fila["valor_renglon11"] = _SumValorRenglon11;
                                        Fila["valor_renglon12"] = _SumValorRenglon12;
                                        Fila["valor_renglon13"] = _SumValorRenglon13;
                                        Fila["valor_renglon14"] = _SumValorRenglon14;
                                        Fila["valor_renglon15"] = _SumValorRenglon15;
                                        Fila["valor_renglon16"] = 0;
                                        Fila["valor_renglon26"] = _SumValorRenglon26;
                                        Fila["valor_renglon27"] = _SumValorRenglon27;
                                        Fila["valor_renglon28"] = _SumValorRenglon28;
                                        Fila["valor_renglon29"] = _SumValorRenglon29;
                                        Fila["valor_renglon30"] = _SumValorRenglon30;
                                        Fila["valor_renglon31"] = _SumValorRenglon31;
                                        Fila["valor_renglon32"] = _SumValorRenglon32;
                                        Fila["valor_renglon33"] = _SumValorRenglon33;
                                        Fila["valor_renglon34"] = _SumValorRenglon34;
                                        Fila["valor_renglon35"] = _SumValorRenglon35;
                                        Fila["valor_renglon36"] = _SumValorRenglon36;
                                        Fila["valor_renglon37"] = _SumValorRenglon37;
                                        dtGuardarBaseGravable.Rows.Add(Fila);
                                        #endregion
                                        //--
                                        #endregion
                                    }
                                    else
                                    {
                                        _Result = false;
                                        string _MsgError = "NO SE OBTUVO DATOS DE LA BASE GRAVABLE";
                                        //--Enviamos la respuesta al cliente
                                        //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "05" } };

                                        //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                                        FixedData.LogApi.Error(_MsgError.ToString().Trim());
                                    }
                                }
                                else
                                {
                                    _Result = false;
                                    string _MsgError = "ERROR AL OBTENER LOS DATOS DE LA BASE GRAVABLE";
                                    //--Enviamos la respuesta al cliente
                                    //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "04" } };

                                    //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                                    FixedData.LogApi.Error(_MsgError.ToString().Trim());
                                }
                                #endregion
                            }

                            //--AQUI MANDAMOS A GUARDAR LOS DATOS EN LA TABLA DE BASE GRAVABLE
                            if (_ContadorEstablecimiento > 0)
                            {
                                #region AQUI OBTENEMOS LOS DATOS DEL DATATABLE PARA ENVIARLOS A LA DB
                                if (dtGuardarBaseGravable.Rows.Count > 0)
                                {
                                    int _IdEstado = 1;
                                    string _ArrayData = "";
                                    foreach (DataRow rowItem in dtGuardarBaseGravable.Rows)
                                    {
                                        #region OBTENER DATOS DEL DATATBALE PARA ENVIAR A LA DB
                                        int _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                                        int _IdFormImpuesto = Int32.Parse(rowItem["idformulario_impuesto"].ToString().Trim());
                                        int _IdCliente = Int32.Parse(rowItem["id_cliente"].ToString().Trim());
                                        int _IdClienteEstablecimiento = Int32.Parse(rowItem["idcliente_establecimiento"].ToString().Trim());
                                        string _CodigoDane = rowItem["codigo_dane"].ToString().Trim();
                                        //int _AnioGravable = Int32.Parse(rowItem["anio_gravable"].ToString().Trim());
                                        //string _MesEf = rowItem["mes_ef"].ToString().Trim();
                                        string _ValorRenglon8 = rowItem["valor_renglon8"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon9 = rowItem["valor_renglon9"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon10 = rowItem["valor_renglon10"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon11 = rowItem["valor_renglon11"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon12 = rowItem["valor_renglon12"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon13 = rowItem["valor_renglon13"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon14 = rowItem["valor_renglon14"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon15 = rowItem["valor_renglon15"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon16 = rowItem["valor_renglon16"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon26 = rowItem["valor_renglon26"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon27 = rowItem["valor_renglon27"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon28 = rowItem["valor_renglon28"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon29 = rowItem["valor_renglon29"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon30 = rowItem["valor_renglon30"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon31 = rowItem["valor_renglon31"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon32 = rowItem["valor_renglon32"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon33 = rowItem["valor_renglon33"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon34 = rowItem["valor_renglon34"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon35 = rowItem["valor_renglon35"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon36 = rowItem["valor_renglon36"].ToString().Trim().Replace(",", ".");
                                        string _ValorRenglon37 = rowItem["valor_renglon37"].ToString().Trim().Replace(",", ".");
                                        string _VersionEf = rowItem["version_ef"].ToString().Trim();

                                        //--ARMAMOS EL ARRAY DE LOS DATOS A CARGAR
                                        if (_ArrayData.ToString().Trim().Length > 0)
                                        {
                                            _ArrayData = _ArrayData.ToString().Trim() + ", " + quote + "(" + _IdMunicipio + "," + _IdFormImpuesto + "," + _IdCliente + "," + _IdClienteEstablecimiento + "," + _CodigoDane + "," + _ValorRenglon8 + "," + _ValorRenglon9 + "," + _ValorRenglon10 + "," + _ValorRenglon11 + "," + _ValorRenglon12 + "," + _ValorRenglon13 + "," + _ValorRenglon14 + "," + _ValorRenglon15 + "," + _ValorRenglon16 + "," + _ValorRenglon26 + "," + _ValorRenglon27 + "," + _ValorRenglon28 + "," + _ValorRenglon29 + "," + _ValorRenglon30 + "," + _ValorRenglon31 + "," + _ValorRenglon32 + "," + _ValorRenglon33 + "," + _ValorRenglon34 + "," + _ValorRenglon35 + "," + _ValorRenglon36 + "," + _ValorRenglon37 + "," + _IdEstado + ")" + quote;
                                        }
                                        else
                                        {
                                            _ArrayData = quote + "(" + _IdMunicipio + "," + _IdFormImpuesto + "," + _IdCliente + "," + _IdClienteEstablecimiento + "," + _CodigoDane + "," + _ValorRenglon8 + "," + _ValorRenglon9 + "," + _ValorRenglon10 + "," + _ValorRenglon11 + "," + _ValorRenglon12 + "," + _ValorRenglon13 + "," + _ValorRenglon14 + "," + _ValorRenglon15 + "," + _ValorRenglon16 + "," + _ValorRenglon26 + "," + _ValorRenglon27 + "," + _ValorRenglon28 + "," + _ValorRenglon29 + "," + _ValorRenglon30 + "," + _ValorRenglon31 + "," + _ValorRenglon32 + "," + _ValorRenglon33 + "," + _ValorRenglon34 + "," + _ValorRenglon35 + "," + _ValorRenglon36 + "," + _ValorRenglon37 + "," + _IdEstado + ")" + quote;
                                        }
                                        #endregion
                                    }

                                    if (_ArrayData.ToString().Trim().Length > 0)
                                    {
                                        FixedData.LogApi.Warn("DATA A CARGAR EN LA DB => " + _ArrayData.ToString().Trim());
                                        //--
                                        objProcessDb.IdClienteEF = objBase.idcliente_ef;
                                        objProcessDb.AnioGravable = objBase.anio_gravable;
                                        objProcessDb.MesEf = objBase.mes_ef;
                                        objProcessDb.VersionEf = objBase.version_ef;
                                        objProcessDb.ArrayData = _ArrayData;
                                        objProcessDb.IdUsuario = objBase.id_usuario;

                                        int _IdRegistro = 0;
                                        string _MsgError = "";
                                        if (objProcessDb.AddLoadBaseGravable(ref _IdRegistro, ref _MsgError))
                                        {
                                            #region PROCESO PARA EL ENVIO DEL CORREO
                                            _Result = true;
                                            //--
                                            objEmails.EmailPara = FixedData.EnvioEmail.ToString().Trim();
                                            objEmails.EmailCopia = FixedData.EnvioEmailCopia.ToString().Trim();
                                            objEmails.Asunto = "REF.: PROCESO BASE GRAVABLE";
                                            objEmails.Detalle = "Señor usuario, para informarle que el proceso de Generación de la Base Gravable del Estado Financiero [Año Gravable: " + objBase.anio_gravable + ", Mes EF: " + objBase.mes_ef + "], ha sido procesado de forma exitosa.";

                                            string _MsgErrorEmail = "";
                                            if (!objEmails.SendEmailConCopia(ref _MsgErrorEmail))
                                            {
                                                FixedData.LogApi.Error("ERROR AL ENVIAR EL CORREO. MOTIVO: " + _MsgErrorEmail);
                                            }

                                            //--Enviamos la respuesta al cliente
                                            //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "00" } };

                                            //--
                                            FixedData.LogApi.Info("PROCESO FINALIZADO: CANTIDAD ESTABLECIMIENTOS: [" + _ContadorEstablecimiento + "] FECHA & HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                                            _ContadorEstablecimiento = 0;
                                            return _Result;
                                            #endregion
                                        }
                                        else
                                        {
                                            #region PROCESO PARA EL ENVIO DEL CORREO
                                            //--
                                            objEmails.EmailPara = FixedData.EnvioEmail.ToString().Trim();
                                            objEmails.EmailCopia = FixedData.EnvioEmailCopia.ToString().Trim();
                                            objEmails.Asunto = "REF.: ERROR PROCESO BASE GRAVABLE";
                                            objEmails.Detalle = "Señor usuario, ocurrio un error con el proceso de Generación de la Base Gravable del Estado Financiero [Año Gravable: " + objBase.anio_gravable + ", Mes EF: " + objBase.mes_ef + "], por favor validar con soporte técnico para ver que sucede. Posible Motivo: " + _MsgError;

                                            string _MsgErrorEmail = "";
                                            if (!objEmails.SendEmailConCopia(ref _MsgErrorEmail))
                                            {
                                                FixedData.LogApi.Error("ERROR AL ENVIAR EL CORREO. MOTIVO: " + _MsgErrorEmail);
                                            }

                                            //--
                                            _ContadorEstablecimiento = 0;
                                            FixedData.LogApi.Error("PROCESO FINALIZADO PERO OCURRIO UN ERROR. MOTIVO: " + _MsgError + ", FECHA & HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    _Result = false;
                                    string _MsgError = "NO FUE POSIBLE CARGAR LA INFO DE LA BASE GRAVABLE A LA DB";
                                    //--Enviamos la respuesta al cliente
                                    //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "05" } };

                                    //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                                    FixedData.LogApi.Error(_MsgError.ToString().Trim());
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            _Result = false;
                            string _MsgError = "ERROR AL CREAR EL DATA TABLE PARA GUARDAR LOS DATOS DE LA BASE GRAVABLE";
                            //--Enviamos la respuesta al cliente
                            //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "03" } };

                            //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                            FixedData.LogApi.Error(_MsgError.ToString().Trim());
                        }
                        #endregion
                    }
                    else
                    {
                        _Result = false;
                        string _MsgError = "NO SE ENCONTRARON ESTABLECIMIENTOS PARA EL ID CLIENTE: " + objBase.id_cliente;
                        //--Enviamos la respuesta al cliente
                        //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "02" } };

                        //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                        FixedData.LogApi.Error(_MsgError.ToString().Trim());
                    }
                }
                else
                {
                    _Result = false;
                    string _MsgError = "ERROR AL OBTENER LOS DATOS DE ESTABLECIMIENTOS PARA EL ID CLIENTE: " + objBase.id_cliente;
                    //--Enviamos la respuesta al cliente
                    //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "01" } };

                    //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                    FixedData.LogApi.Error(_MsgError.ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                _Result = false;
                string _MsgError = "SE PRODUJO UNA EXCEPCIÓN AL REALIZAR EL PROCESDO DE LA BASE GRAVABLE. MOTIVO: " + ex.Message;
                //--Enviamos la respuesta al cliente
                //_Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "EX" } };

                //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                FixedData.LogApi.Error(_MsgError.ToString().Trim());
            }

            return _Result;
        }

        public bool ProcessBaseGravable(BaseGravable_Req objBase, ref HttpError _Response)
        {
            bool _Result = false;
            try
            {
                FixedData.LogApi.Info("PROCESO INICIADO FECHA HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                //--INSTANCIAMOS EL OBJETO DE CLASE
                ProcessDb objProcessDb = new ProcessDb();
                objProcessDb.TipoConsulta = 5;
                objProcessDb.IdCliente = objBase.id_cliente;
                objProcessDb.IdEstablecimientoPadre = null;
                objProcessDb.IdEstado = 1;
                //--
                DataTable dtEstablecimientos = new DataTable();
                dtEstablecimientos = objProcessDb.GetEstablecimientosCliente();
                if (dtEstablecimientos != null)
                {
                    if (dtEstablecimientos.Rows.Count > 0)
                    {
                        #region AQUI REALIZAMOS EL PROCESO DE LA BASE GRAVABLE POR MUNICIPIO - ESTABLECIMIENTOS
                        //--AQUI MANDAMOS A GENERAR LA TABLA PARA GUARDAR LA INFO DE LA BASE GRAVABLE
                        DataTable dtGuardarBaseGravable = new DataTable();
                        dtGuardarBaseGravable = this.GetTablaDatos();
                        if (dtGuardarBaseGravable != null)
                        {
                            int _ContadorEstablecimiento = 0;
                            foreach (DataRow rowEst in dtEstablecimientos.Rows)
                            {
                                #region OBTENEMOS DATOS DE ESTABLECIMIENTO PARA GENERAR BASE GRAVABLE
                                //--
                                _ContadorEstablecimiento++;
                                int _IdClienteEstablecimiento = Int32.Parse(rowEst["idcliente_establecimiento"].ToString().Trim());
                                int _IdMunicipio = Int32.Parse(rowEst["id_municipio"].ToString().Trim());
                                string _CodigoDane = rowEst["codigo_dane"].ToString().Trim();

                                //--AQUI INSERTAMOS LOS DATOS EN EL DATA TABLE
                                DataRow Fila = null;
                                Fila = dtGuardarBaseGravable.NewRow();
                                Fila["idbase_gravable"] = dtGuardarBaseGravable.Rows.Count + 1;
                                Fila["id_municipio"] = _IdMunicipio;
                                Fila["idformulario_impuesto"] = objBase.idform_impuesto;
                                Fila["id_cliente"] = objBase.id_cliente;
                                Fila["idcliente_establecimiento"] = _IdClienteEstablecimiento;
                                Fila["codigo_dane"] = _CodigoDane;
                                Fila["anio_gravable"] = objBase.anio_gravable;
                                Fila["mes_ef"] = objBase.mes_ef;
                                Fila["version_ef"] = objBase.version_ef;

                                //--
                                objProcessDb.TipoConsulta = objBase.tipo_consulta;
                                objProcessDb.IdCliente = objBase.id_cliente;
                                objProcessDb.IdClienteEstablecimiento = _IdClienteEstablecimiento;
                                objProcessDb.IdFormularioImpuesto = objBase.idform_impuesto;
                                objProcessDb.AnioGravable = objBase.anio_gravable;
                                objProcessDb.IdFormConfiguracion = null;
                                objProcessDb.IdPuc = null;
                                //--
                                DataTable dtBaseGravable = new DataTable();
                                dtBaseGravable = objProcessDb.GetBaseGravable();
                                int _ContadorRow = 0;
                                //--
                                if (dtBaseGravable != null)
                                {
                                    if (dtBaseGravable.Rows.Count > 0)
                                    {
                                        #region AQUI REALIZAMOS EL PROCESO DE GUARDAR EN LA BASE DE DATOS
                                        //--AQUI RECORREMOS EL DATATABLE PARA MOSTRAR LAS ACTIVIDADES ECONOMICAS.
                                        double _SumValorRenglon8 = 0, _SumValorRenglon9 = 0, _SumValorRenglon10 = 0;
                                        double _SumValorRenglon11 = 0, _SumValorRenglon12 = 0, _SumValorRenglon13 = 0;
                                        double _SumValorRenglon14 = 0, _SumValorRenglon15 = 0, _SumValorRenglon26 = 0;
                                        double _SumValorRenglon27 = 0, _SumValorRenglon28 = 0, _SumValorRenglon29 = 0;
                                        double _SumValorRenglon30 = 0, _SumValorRenglon31 = 0, _SumValorRenglon32 = 0;
                                        double _SumValorRenglon33 = 0, _SumValorRenglon34 = 0, _SumValorRenglon35 = 0;
                                        double _SumValorRenglon36 = 0, _SumValorRenglon37 = 0;
                                        foreach (DataRow rowItem in dtBaseGravable.Rows)
                                        {
                                            #region VALORES OBTENIDOS DE LA BASE GRAVABLE
                                            _ContadorRow++;
                                            int _NumRenglon = Int32.Parse(rowItem["numero_renglon"].ToString().Trim());
                                            string _CodigoCuenta = rowItem["codigo_cuenta"].ToString().Trim();
                                            string _SaldoInicial = rowItem["saldo_inicial"].ToString().Trim();
                                            string _MovDebito = rowItem["mov_debito"].ToString().Trim();
                                            string _MovCredito = rowItem["mov_credito"].ToString().Trim();
                                            string _SaldoFinal = rowItem["saldo_final"].ToString().Trim();
                                            string _ValorExtracontable1 = rowItem["valor_extracontable"].ToString().Trim().Replace("-", "");
                                            double _ValorExtracontable = Double.Parse(_ValorExtracontable1);

                                            //--AQUI MANDAMOS A OBTENER LOS VALORES DEL ESTADO FINANCIERO POR CLIENTE
                                            objProcessDb.TipoConsulta = 1;
                                            objProcessDb.NumeroRenglon = _NumRenglon;
                                            objProcessDb.IdCliente = objBase.id_cliente != null ? objBase.id_cliente.ToString().Trim() : null;
                                            objProcessDb.AnioGravable = objBase.anio_gravable;
                                            objProcessDb.IdClienteEstablecimiento = objBase.idcliente_establecimiento;
                                            objProcessDb.SaldoInicial = _SaldoInicial;
                                            objProcessDb.MovDebito = _MovDebito;
                                            objProcessDb.MovCredito = _MovCredito;
                                            objProcessDb.SaldoFinal = _SaldoFinal;
                                            objProcessDb.CodigoCuenta = _CodigoCuenta;
                                            objProcessDb.MesEf = objBase.mes_ef.ToString().Trim();

                                            //--AQUI OBTENEMOS EL VALOR A DEFINIR EN EL RENGLON DEL FORM.
                                            double _ValorTotal = 0;
                                            if (_SaldoInicial == "S" || _MovDebito == "S" ||
                                                _MovCredito == "S" || _SaldoFinal == "S")
                                            {
                                                List<string> _ArrayDatos = objProcessDb.GetEstadoFinanciero();
                                                if (_ArrayDatos.Count > 0)
                                                {
                                                    string _ValorCuenta = _ArrayDatos[1].ToString().Trim();
                                                    _ValorTotal = (Double.Parse(_ValorCuenta) + _ValorExtracontable);
                                                    //--
                                                    FixedData.LogApi.Warn("CONTADOR => " + _ContadorRow + ", No. RENGLON => " + _NumRenglon + ", COD. CUENTA => " + _CodigoCuenta + ", VALOR CUENTA => " + _ValorCuenta);
                                                }
                                            }
                                            else
                                            {
                                                _ValorTotal = _ValorExtracontable;
                                            }
                                            #endregion

                                            #region AQUI OBTENEMOS EL VALOR DE LA BASE BRAVABLE MEDIANTE UN SWITCH
                                            //--AQUI OBTENEMOS LOS DATOS 
                                            switch (_NumRenglon)
                                            {
                                                case 8:
                                                    _SumValorRenglon8 = _SumValorRenglon8 + _ValorTotal;
                                                    break;
                                                case 9:
                                                    _SumValorRenglon9 = _SumValorRenglon9 + _ValorTotal;
                                                    break;
                                                case 11:
                                                    _SumValorRenglon11 = _SumValorRenglon11 + _ValorTotal;
                                                    break;
                                                case 12:
                                                    _SumValorRenglon12 = _SumValorRenglon12 + _ValorTotal;
                                                    break;
                                                case 13:
                                                    _SumValorRenglon13 = _SumValorRenglon13 + _ValorTotal;
                                                    break;
                                                case 14:
                                                    _SumValorRenglon14 = _SumValorRenglon14 + _ValorTotal;
                                                    break;
                                                case 15:
                                                    _SumValorRenglon15 = _SumValorRenglon15 + _ValorTotal;
                                                    break;
                                                case 26:
                                                    _SumValorRenglon26 = _SumValorRenglon26 + _ValorTotal;
                                                    break;
                                                case 27:
                                                    _SumValorRenglon27 = _SumValorRenglon27 + _ValorTotal;
                                                    break;
                                                case 28:
                                                    _SumValorRenglon28 = _SumValorRenglon28 + _ValorTotal;
                                                    break;
                                                case 29:
                                                    _SumValorRenglon29 = _SumValorRenglon29 + _ValorTotal;
                                                    break;
                                                case 30:
                                                    _SumValorRenglon30 = _SumValorRenglon30 + _ValorTotal;
                                                    break;
                                                case 31:
                                                    _SumValorRenglon31 = _SumValorRenglon31 + _ValorTotal;
                                                    break;
                                                case 32:
                                                    _SumValorRenglon32 = _SumValorRenglon32 + _ValorTotal;
                                                    break;
                                                case 33:
                                                    _SumValorRenglon33 = _SumValorRenglon33 + _ValorTotal;
                                                    break;
                                                case 34:
                                                    _SumValorRenglon34 = _SumValorRenglon34 + _ValorTotal;
                                                    break;
                                                case 35:
                                                    _SumValorRenglon35 = _SumValorRenglon35 + _ValorTotal;
                                                    break;
                                                case 36:
                                                    _SumValorRenglon36 = _SumValorRenglon36 + _ValorTotal;
                                                    break;
                                                case 37:
                                                    _SumValorRenglon37 = _SumValorRenglon37 + _ValorTotal;
                                                    break;
                                                default:
                                                    break;
                                            }
                                            #endregion
                                        }

                                        #region REGISTRAMOS LOS DATOS EN EL DATATABLE
                                        //--AQUI REGISTRAMOS LOS DATOS EN EL DATATABLE
                                        Fila["valor_renglon8"] = _SumValorRenglon8;
                                        Fila["valor_renglon9"] = _SumValorRenglon9;
                                        _SumValorRenglon10 = (_SumValorRenglon8 - _SumValorRenglon9);
                                        Fila["valor_renglon10"] = _SumValorRenglon10;
                                        Fila["valor_renglon11"] = _SumValorRenglon11;
                                        Fila["valor_renglon12"] = _SumValorRenglon12;
                                        Fila["valor_renglon13"] = _SumValorRenglon13;
                                        Fila["valor_renglon14"] = _SumValorRenglon14;
                                        Fila["valor_renglon15"] = _SumValorRenglon15;
                                        Fila["valor_renglon16"] = 0;
                                        Fila["valor_renglon26"] = _SumValorRenglon26;
                                        Fila["valor_renglon27"] = _SumValorRenglon27;
                                        Fila["valor_renglon28"] = _SumValorRenglon28;
                                        Fila["valor_renglon29"] = _SumValorRenglon29;
                                        Fila["valor_renglon30"] = _SumValorRenglon30;
                                        Fila["valor_renglon31"] = _SumValorRenglon31;
                                        Fila["valor_renglon32"] = _SumValorRenglon32;
                                        Fila["valor_renglon33"] = _SumValorRenglon33;
                                        Fila["valor_renglon34"] = _SumValorRenglon34;
                                        Fila["valor_renglon35"] = _SumValorRenglon35;
                                        Fila["valor_renglon36"] = _SumValorRenglon36;
                                        Fila["valor_renglon37"] = _SumValorRenglon37;
                                        dtGuardarBaseGravable.Rows.Add(Fila);
                                        #endregion
                                        //--
                                        #endregion
                                    }
                                    else
                                    {
                                        _Result = false;
                                        string _MsgError = "NO SE OBTUVO DATOS DE LA BASE GRAVABLE";
                                        //--Enviamos la respuesta al cliente
                                        _Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "05" } };

                                        //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                                        FixedData.LogApi.Error(_MsgError.ToString().Trim());
                                    }
                                }
                                else
                                {
                                    _Result = false;
                                    string _MsgError = "ERROR AL OBTENER LOS DATOS DE LA BASE GRAVABLE";
                                    //--Enviamos la respuesta al cliente
                                    _Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "04" } };

                                    //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                                    FixedData.LogApi.Error(_MsgError.ToString().Trim());
                                }
                                #endregion
                            }

                            //--AQUI MANDAMOS A GUARDAR LOS DATOS EN LA TABLA DE BASE GRAVABLE
                            if (_ContadorEstablecimiento > 0)
                            {
                                #region AQUI OBTENEMOS LOS DATOS DEL DATATABLE PARA ENVIARLOS A LA DB
                                if (dtGuardarBaseGravable.Rows.Count > 0)
                                {
                                    int _IdEstado = 1;
                                    string _ArrayData = "";
                                    foreach (DataRow rowItem in dtGuardarBaseGravable.Rows)
                                    {
                                        #region OBTENER DATOS DEL DATATBALE PARA ENVIAR A LA DB
                                        int _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                                        int _IdFormImpuesto = Int32.Parse(rowItem["idformulario_impuesto"].ToString().Trim());
                                        int _IdCliente = Int32.Parse(rowItem["id_cliente"].ToString().Trim());
                                        int _IdClienteEstablecimiento = Int32.Parse(rowItem["idcliente_establecimiento"].ToString().Trim());
                                        string _CodigoDane = rowItem["codigo_dane"].ToString().Trim();
                                        //int _AnioGravable = Int32.Parse(rowItem["anio_gravable"].ToString().Trim());
                                        //string _MesEf = rowItem["mes_ef"].ToString().Trim();
                                        string _ValorRenglon8 = rowItem["valor_renglon8"].ToString().Trim();
                                        string _ValorRenglon9 = rowItem["valor_renglon9"].ToString().Trim();
                                        string _ValorRenglon10 = rowItem["valor_renglon10"].ToString().Trim();
                                        string _ValorRenglon11 = rowItem["valor_renglon11"].ToString().Trim();
                                        string _ValorRenglon12 = rowItem["valor_renglon12"].ToString().Trim();
                                        string _ValorRenglon13 = rowItem["valor_renglon13"].ToString().Trim();
                                        string _ValorRenglon14 = rowItem["valor_renglon14"].ToString().Trim();
                                        string _ValorRenglon15 = rowItem["valor_renglon15"].ToString().Trim();
                                        string _ValorRenglon16 = rowItem["valor_renglon16"].ToString().Trim();
                                        string _ValorRenglon26 = rowItem["valor_renglon26"].ToString().Trim();
                                        string _ValorRenglon27 = rowItem["valor_renglon27"].ToString().Trim();
                                        string _ValorRenglon28 = rowItem["valor_renglon28"].ToString().Trim();
                                        string _ValorRenglon29 = rowItem["valor_renglon29"].ToString().Trim();
                                        string _ValorRenglon30 = rowItem["valor_renglon30"].ToString().Trim();
                                        string _ValorRenglon31 = rowItem["valor_renglon31"].ToString().Trim();
                                        string _ValorRenglon32 = rowItem["valor_renglon32"].ToString().Trim();
                                        string _ValorRenglon33 = rowItem["valor_renglon33"].ToString().Trim();
                                        string _ValorRenglon34 = rowItem["valor_renglon34"].ToString().Trim();
                                        string _ValorRenglon35 = rowItem["valor_renglon35"].ToString().Trim();
                                        string _ValorRenglon36 = rowItem["valor_renglon36"].ToString().Trim();
                                        string _ValorRenglon37 = rowItem["valor_renglon37"].ToString().Trim();
                                        string _VersionEf = rowItem["version_ef"].ToString().Trim();

                                        //--ARMAMOS EL ARRAY DE LOS DATOS A CARGAR
                                        if (_ArrayData.ToString().Trim().Length > 0)
                                        {
                                            _ArrayData = _ArrayData.ToString().Trim() + ", " + quote + "(" + _IdMunicipio + "," + _IdFormImpuesto + "," + _IdCliente + "," + _IdClienteEstablecimiento + "," + _CodigoDane + "," + _ValorRenglon8 + "," + _ValorRenglon9 + "," + _ValorRenglon10 + "," + _ValorRenglon11 + "," + _ValorRenglon12 + "," + _ValorRenglon13 + "," + _ValorRenglon14 + "," + _ValorRenglon15 + "," + _ValorRenglon16 + "," + _ValorRenglon26 + "," + _ValorRenglon27 + "," + _ValorRenglon28 + "," + _ValorRenglon29 + "," + _ValorRenglon30 + "," + _ValorRenglon31 + "," + _ValorRenglon32 + "," + _ValorRenglon33 + "," + _ValorRenglon34 + "," + _ValorRenglon35 + "," + _ValorRenglon36 + "," + _ValorRenglon37 + "," + _IdEstado + ")" + quote;
                                        }
                                        else
                                        {
                                            _ArrayData = quote + "(" + _IdMunicipio + "," + _IdFormImpuesto + "," + _IdCliente + "," + _IdClienteEstablecimiento + "," + _CodigoDane + "," + _ValorRenglon8 + "," + _ValorRenglon9 + "," + _ValorRenglon10 + "," + _ValorRenglon11 + "," + _ValorRenglon12 + "," + _ValorRenglon13 + "," + _ValorRenglon14 + "," + _ValorRenglon15 + "," + _ValorRenglon16 + "," + _ValorRenglon26 + "," + _ValorRenglon27 + "," + _ValorRenglon28 + "," + _ValorRenglon29 + "," + _ValorRenglon30 + "," + _ValorRenglon31 + "," + _ValorRenglon32 + "," + _ValorRenglon33 + "," + _ValorRenglon34 + "," + _ValorRenglon35 + "," + _ValorRenglon36 + "," + _ValorRenglon37 + "," + _IdEstado + ")" + quote;
                                        }
                                        #endregion
                                    }

                                    if (_ArrayData.ToString().Trim().Length > 0)
                                    {
                                        objProcessDb.AnioGravable = objBase.anio_gravable;
                                        objProcessDb.MesEf = objBase.mes_ef;
                                        objProcessDb.VersionEf = objBase.version_ef;
                                        objProcessDb.ArrayData = _ArrayData;
                                        objProcessDb.IdUsuario = objBase.id_usuario;

                                        int _IdRegistro = 0;
                                        string _MsgError = "";
                                        if (objProcessDb.AddLoadBaseGravable(ref _IdRegistro, ref _MsgError))
                                        {
                                            _Result = true;
                                            //--Enviamos la respuesta al cliente
                                            _Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "00" } };

                                            //--
                                            FixedData.LogApi.Info("PROCESO FINALIZADO: CANTIDAD ESTABLECIMIENTOS: [" + _ContadorEstablecimiento + "] FECHA & HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                                            _ContadorEstablecimiento = 0;
                                        }
                                        else
                                        {
                                            _ContadorEstablecimiento = 0;
                                            FixedData.LogApi.Error("PROCESO FINALIZADO PERO OCURRIO UN ERROR. MOTIVO: " + _MsgError + ", FECHA & HORA: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                                        }
                                    }
                                }
                                else
                                {
                                    _Result = false;
                                    string _MsgError = "NO FUE POSIBLE CARGAR LA INFO DE LA BASE GRAVABLE A LA DB";
                                    //--Enviamos la respuesta al cliente
                                    _Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "05" } };

                                    //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                                    FixedData.LogApi.Error(_MsgError.ToString().Trim());
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            _Result = false;
                            string _MsgError = "ERROR AL CREAR EL DATA TABLE PARA GUARDAR LOS DATOS DE LA BASE GRAVABLE";
                            //--Enviamos la respuesta al cliente
                            _Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "03" } };

                            //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                            FixedData.LogApi.Error(_MsgError.ToString().Trim());
                        }
                        #endregion
                    }
                    else
                    {
                        _Result = false;
                        string _MsgError = "NO SE ENCONTRARON ESTABLECIMIENTOS PARA EL ID CLIENTE: " + objBase.id_cliente;
                        //--Enviamos la respuesta al cliente
                        _Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "02" } };

                        //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                        FixedData.LogApi.Error(_MsgError.ToString().Trim());
                    }
                }
                else
                {
                    _Result = false;
                    string _MsgError = "ERROR AL OBTENER LOS DATOS DE ESTABLECIMIENTOS PARA EL ID CLIENTE: " + objBase.id_cliente;
                    //--Enviamos la respuesta al cliente
                    _Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "01" } };

                    //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                    FixedData.LogApi.Error(_MsgError.ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                _Result = false;
                string _MsgError = "SE PRODUJO UNA EXCEPCIÓN AL REALIZAR EL PROCESDO DE LA BASE GRAVABLE. MOTIVO: " + ex.Message;
                //--Enviamos la respuesta al cliente
                _Response = new HttpError(_MsgError) { { "Status", _Result }, { "Codigo", "EX" } };

                //--ESCRIBIMOS EN EL LOG DE AUDITORIA
                FixedData.LogApi.Error(_MsgError.ToString().Trim());
            }

            return _Result;
        }

        private DataTable GetTablaDatos()
        {
            DataTable dtDatos = new DataTable();
            try
            {
                //--AQUI DEFINIMOS LAS COLUMNAS DEL DATATABLE PARA ALMACENAR LA INFO DE LA BASE GRAVABLE
                DataTable DtBaseGravable = new DataTable();
                DtBaseGravable = new DataTable();
                DtBaseGravable.TableName = "DtBaseGravable";
                DtBaseGravable.Columns.Add("idbase_gravable", typeof(Int32));
                DtBaseGravable.PrimaryKey = new DataColumn[] { DtBaseGravable.Columns["idbase_gravable"] };
                DtBaseGravable.Columns.Add("id_municipio");
                DtBaseGravable.Columns.Add("idformulario_impuesto");
                DtBaseGravable.Columns.Add("id_cliente");
                DtBaseGravable.Columns.Add("idcliente_establecimiento");
                DtBaseGravable.Columns.Add("codigo_dane");
                DtBaseGravable.Columns.Add("anio_gravable");
                DtBaseGravable.Columns.Add("mes_ef");
                DtBaseGravable.Columns.Add("valor_renglon8");
                DtBaseGravable.Columns.Add("valor_renglon9");
                DtBaseGravable.Columns.Add("valor_renglon10");
                DtBaseGravable.Columns.Add("valor_renglon11");
                DtBaseGravable.Columns.Add("valor_renglon12");
                DtBaseGravable.Columns.Add("valor_renglon13");
                DtBaseGravable.Columns.Add("valor_renglon14");
                DtBaseGravable.Columns.Add("valor_renglon15");
                DtBaseGravable.Columns.Add("valor_renglon16");
                DtBaseGravable.Columns.Add("valor_renglon26");
                DtBaseGravable.Columns.Add("valor_renglon27");
                DtBaseGravable.Columns.Add("valor_renglon28");
                DtBaseGravable.Columns.Add("valor_renglon29");
                DtBaseGravable.Columns.Add("valor_renglon30");
                DtBaseGravable.Columns.Add("valor_renglon31");
                DtBaseGravable.Columns.Add("valor_renglon32");
                DtBaseGravable.Columns.Add("valor_renglon33");
                DtBaseGravable.Columns.Add("valor_renglon34");
                DtBaseGravable.Columns.Add("valor_renglon35");
                DtBaseGravable.Columns.Add("valor_renglon36");
                DtBaseGravable.Columns.Add("valor_renglon37");
                DtBaseGravable.Columns.Add("version_ef");
                dtDatos = DtBaseGravable.Copy();
            }
            catch (Exception ex)
            {
                dtDatos = null;
                FixedData.LogApi.Error("ERROR AL GENERAR EL DATA TABLE PARA LA BASE GRAVABLE. MOTIVO: " + ex.Message);
            }

            return dtDatos;
        }

        public static double round(double input)
        {
            double ValorRenglo = Math.Abs(input);
            double _Result = 0;
            double rem = ValorRenglo % 1000;
            //return rem >= 5 ? (num - rem + 10) : (num - rem);
            if (rem >= 500)
            {
                _Result = (double)(1000 * Math.Ceiling(ValorRenglo / 1000));
            }
            else
            {
                _Result = (double)(1000 * Math.Round(ValorRenglo / 1000));
            }

            return _Result;
        }
        #endregion

    }
}