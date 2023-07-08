using System;
using System.Web.Caching;
using log4net;
using Telerik.Web.UI;
using System.Data;
using Smartax.Web.Application.Clases.Parametros.Divipola;
using Smartax.Web.Application.Clases.Seguridad;
using System.Web.UI.WebControls;
using Smartax.Web.Application.Clases.Modulos;
using NativeExcel;
using System.Drawing;

namespace Smartax.Web.Application.Controles.Modulos.PlaneacionFiscal
{
    public partial class FrmVerFechaLimitePago : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        RadWindow Ventana = new RadWindow();

        MunicipioDescuento ObjMunDescuento = new MunicipioDescuento();
        LiquidarImpuestos ObjLiqImpuesto = new LiquidarImpuestos();
        MunicipioCalendarioTributario ObjMunCalendario = new MunicipioCalendarioTributario();
        Utilidades ObjUtils = new Utilidades();

        public DataSet GetDatosGrilla()
        {
            DataSet ObjetoDataSet = new DataSet();
            DataTable ObjetoDataTable = new DataTable();
            DataTable dtLiqImpuesto = new DataTable();
            DataTable dtVencimientos = new DataTable();
            try
            {
                #region OBTENER DATOS DE LA DB
                ObjMunDescuento.TipoConsulta = 3;
                ObjMunDescuento.IdMunDescuento = null;
                ObjMunDescuento.IdMunicipio = null;
                ObjMunDescuento.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                ObjMunDescuento.IdFormularioImpuesto = this.ViewState["IdImpuesto"].ToString().Trim();
                ObjMunDescuento.AnioGravable = Int32.Parse(this.ViewState["AnioGravable"].ToString().Trim());
                ObjMunDescuento.IdEstado = 1;
                ObjMunDescuento.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                //--PASO 1. OBTENEMOS LA PARAMETRIZACION DE DESCUENTO REALIZADA EN EL MUNICIPIO
                DataTable dtRenglones = new DataTable();
                dtRenglones = ObjMunDescuento.GetDetalleDescuento();

                if (dtRenglones != null)
                {
                    if (dtRenglones.Rows.Count > 0)
                    {
                        #region PASO 2. OBTENEMOS DATOS DE LA LIQUIDACION DEL FORMULARIO EN BORRADOR O LIQUIDADO
                        //--
                        ObjLiqImpuesto.IdFormularioImpuesto = this.ViewState["IdImpuesto"].ToString().Trim();
                        ObjLiqImpuesto.TipoConsulta = 3;
                        ObjLiqImpuesto.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                        ObjLiqImpuesto.CodigoDane = null;
                        ObjLiqImpuesto.AnioGravable = Int32.Parse(this.ViewState["AnioGravable"].ToString().Trim());
                        ObjLiqImpuesto.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                        //--OBTENER DATOS DE LA LIQUIDACION DEL IMPUESTO
                        dtLiqImpuesto = ObjLiqImpuesto.GetLiquidacionForm();
                        #endregion

                        #region AQUI OBTENEMOS LOS VENCIMIENTO DE CADA MUNICIPIO
                        ObjMunCalendario.TipoConsulta = 3;
                        ObjMunCalendario.IdMunicipio = null;
                        ObjMunCalendario.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                        ObjMunCalendario.IdEstado = 1;
                        ObjMunCalendario.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                        //Mostrar los impuestos por municipio
                        dtVencimientos = ObjMunCalendario.GetVencimientosMunicipio();
                        dtVencimientos.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_registro"] };
                        #endregion

                        //--AQUI DEFINIMOS LA VARIABLE QUE VA HACER LA SUMATORIA GENERAL
                        double _ValorTotal = 0;
                        foreach (DataRow rowItem in dtRenglones.Rows)
                        {
                            #region AQUI OBTENEMOS LOS VALORES PARAMETRIZADO PARA OBTENER EL DESCUENTO
                            int _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                            int _NumRenglon1 = Int32.Parse(rowItem["numero_renglon1"].ToString().Trim());
                            int _TipoOperacion1 = rowItem["idtipo_operacion1"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion1"].ToString().Trim()) : 0;
                            int _NumRenglon2 = rowItem["numero_renglon2"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["numero_renglon2"].ToString().Trim()) : 0;
                            int _TipoOperacion2 = rowItem["idtipo_operacion2"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion2"].ToString().Trim()) : 0;
                            int _NumRenglon3 = rowItem["numero_renglon3"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["numero_renglon3"].ToString().Trim()) : 0;
                            int _TipoOperacion3 = rowItem["idtipo_operacion3"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion3"].ToString().Trim()) : 0;
                            int _NumRenglon4 = rowItem["numero_renglon4"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["numero_renglon4"].ToString().Trim()) : 0;
                            int _TipoOperacion4 = rowItem["idtipo_operacion4"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion4"].ToString().Trim()) : 0;
                            int _NumRenglon5 = rowItem["numero_renglon5"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["numero_renglon5"].ToString().Trim()) : 0;
                            int _TipoOperacion5 = rowItem["idtipo_operacion5"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion5"].ToString().Trim()) : 0;
                            int _NumRenglon6 = rowItem["numero_renglon6"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["numero_renglon6"].ToString().Trim()) : 0;
                            int _TipoOperacion6 = rowItem["idtipo_operacion6"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion6"].ToString().Trim()) : 0;
                            int _NumRenglon7 = rowItem["numero_renglon7"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["numero_renglon7"].ToString().Trim()) : 0;
                            int _TipoOperacion7 = rowItem["idtipo_operacion7"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion7"].ToString().Trim()) : 0;
                            int _NumRenglon8 = rowItem["numero_renglon8"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["numero_renglon8"].ToString().Trim()) : 0;
                            int _TipoOperacion8 = rowItem["idtipo_operacion8"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion8"].ToString().Trim()) : 0;
                            int _NumRenglon9 = rowItem["numero_renglon9"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["numero_renglon9"].ToString().Trim()) : 0;
                            int _TipoOperacion9 = rowItem["idtipo_operacion9"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["idtipo_operacion9"].ToString().Trim()) : 0;
                            int _NumRenglon10 = rowItem["numero_renglon10"].ToString().Trim().Length > 0 ? Int32.Parse(rowItem["numero_renglon10"].ToString().Trim()) : 0;
                            //--DEFINIR VARIABLES DE VALORES
                            double _ValorRenglon1 = 0, _ValorRenglon2 = 0, _ValorRenglon3 = 0, _ValorRenglon4 = 0, _ValorRenglon5 = 0;
                            double _ValorRenglon6 = 0, _ValorRenglon7 = 0, _ValorRenglon8 = 0, _ValorRenglon9 = 0, _ValorRenglon10 = 0;
                            #endregion

                            #region PASO 2. OBTENEMOS DATOS DE LA LIQUIDACION DEL FORMULARIO EN BORRADOR O LIQUIDADO
                            //--
                            if (dtLiqImpuesto != null)
                            {
                                if (dtLiqImpuesto.Rows.Count > 0)
                                {
                                    DataRow[] dataRows = dtLiqImpuesto.Select("id_municipio = " + _IdMunicipio);
                                    //--
                                    foreach (DataRow rowItem2 in dataRows)
                                    {
                                        #region AQUI OBTENEMOS LOS VALORES DE LOS RENGLONES CONFIGURADOS COMO DESCUENTO
                                        //--RENGLON 1
                                        #region OBTENER VALOR DE LIQUIDACION DEL RENGLON 1
                                        switch (_NumRenglon1)
                                        {
                                            case 8:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon8"].ToString().Trim());
                                                break;
                                            case 9:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon9"].ToString().Trim());
                                                break;
                                            case 10:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon10"].ToString().Trim());
                                                break;
                                            case 11:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon11"].ToString().Trim());
                                                break;
                                            case 12:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon12"].ToString().Trim());
                                                break;
                                            case 13:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon13"].ToString().Trim());
                                                break;
                                            case 14:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon14"].ToString().Trim());
                                                break;
                                            case 15:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon15"].ToString().Trim());
                                                break;
                                            case 16:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon16"].ToString().Trim());
                                                break;
                                            case 17:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon17"].ToString().Trim());
                                                break;
                                            case 19:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon19"].ToString().Trim());
                                                break;
                                            case 20:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon20"].ToString().Trim());
                                                break;
                                            case 21:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon21"].ToString().Trim());
                                                break;
                                            case 22:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon22"].ToString().Trim());
                                                break;
                                            case 23:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon23"].ToString().Trim());
                                                break;
                                            case 24:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon24"].ToString().Trim());
                                                break;
                                            case 25:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon25"].ToString().Trim());
                                                break;
                                            case 26:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon26"].ToString().Trim());
                                                break;
                                            case 27:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon27"].ToString().Trim());
                                                break;
                                            case 28:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon28"].ToString().Trim());
                                                break;
                                            case 29:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon29"].ToString().Trim());
                                                break;
                                            case 30:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon30"].ToString().Trim());
                                                break;
                                            case 31:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon31"].ToString().Trim());
                                                break;
                                            case 32:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon32"].ToString().Trim());
                                                break;
                                            case 33:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon33"].ToString().Trim());
                                                break;
                                            case 34:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon34"].ToString().Trim());
                                                break;
                                            case 35:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon35"].ToString().Trim());
                                                break;
                                            case 36:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon36"].ToString().Trim());
                                                break;
                                            case 37:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon37"].ToString().Trim());
                                                break;
                                            case 38:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon38"].ToString().Trim());
                                                break;
                                            case 39:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon39"].ToString().Trim());
                                                break;
                                            case 40:
                                                _ValorRenglon1 = Double.Parse(rowItem2["valor_renglon40"].ToString().Trim());
                                                break;
                                            default:
                                                break;
                                        }
                                        #endregion
                                        //--RENGLON 2
                                        #region OBTENER VALOR DE LIQUIDACION DEL RENGLON 2
                                        switch (_NumRenglon2)
                                        {
                                            case 8:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon8"].ToString().Trim());
                                                break;
                                            case 9:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon9"].ToString().Trim());
                                                break;
                                            case 10:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon10"].ToString().Trim());
                                                break;
                                            case 11:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon11"].ToString().Trim());
                                                break;
                                            case 12:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon12"].ToString().Trim());
                                                break;
                                            case 13:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon13"].ToString().Trim());
                                                break;
                                            case 14:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon14"].ToString().Trim());
                                                break;
                                            case 15:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon15"].ToString().Trim());
                                                break;
                                            case 16:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon16"].ToString().Trim());
                                                break;
                                            case 17:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon17"].ToString().Trim());
                                                break;
                                            case 19:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon19"].ToString().Trim());
                                                break;
                                            case 20:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon20"].ToString().Trim());
                                                break;
                                            case 21:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon21"].ToString().Trim());
                                                break;
                                            case 22:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon22"].ToString().Trim());
                                                break;
                                            case 23:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon23"].ToString().Trim());
                                                break;
                                            case 24:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon24"].ToString().Trim());
                                                break;
                                            case 25:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon25"].ToString().Trim());
                                                break;
                                            case 26:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon26"].ToString().Trim());
                                                break;
                                            case 27:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon27"].ToString().Trim());
                                                break;
                                            case 28:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon28"].ToString().Trim());
                                                break;
                                            case 29:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon29"].ToString().Trim());
                                                break;
                                            case 30:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon30"].ToString().Trim());
                                                break;
                                            case 31:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon31"].ToString().Trim());
                                                break;
                                            case 32:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon32"].ToString().Trim());
                                                break;
                                            case 33:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon33"].ToString().Trim());
                                                break;
                                            case 34:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon34"].ToString().Trim());
                                                break;
                                            case 35:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon35"].ToString().Trim());
                                                break;
                                            case 36:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon36"].ToString().Trim());
                                                break;
                                            case 37:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon37"].ToString().Trim());
                                                break;
                                            case 38:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon38"].ToString().Trim());
                                                break;
                                            case 39:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon39"].ToString().Trim());
                                                break;
                                            case 40:
                                                _ValorRenglon2 = Double.Parse(rowItem2["valor_renglon40"].ToString().Trim());
                                                break;
                                            default:
                                                break;
                                        }
                                        #endregion
                                        //--RENGLON 3
                                        #region OBTENER VALOR DE LIQUIDACION DEL RENGLON 3
                                        switch (_NumRenglon3)
                                        {
                                            case 8:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon8"].ToString().Trim());
                                                break;
                                            case 9:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon9"].ToString().Trim());
                                                break;
                                            case 10:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon10"].ToString().Trim());
                                                break;
                                            case 11:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon11"].ToString().Trim());
                                                break;
                                            case 12:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon12"].ToString().Trim());
                                                break;
                                            case 13:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon13"].ToString().Trim());
                                                break;
                                            case 14:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon14"].ToString().Trim());
                                                break;
                                            case 15:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon15"].ToString().Trim());
                                                break;
                                            case 16:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon16"].ToString().Trim());
                                                break;
                                            case 17:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon17"].ToString().Trim());
                                                break;
                                            case 19:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon19"].ToString().Trim());
                                                break;
                                            case 20:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon20"].ToString().Trim());
                                                break;
                                            case 21:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon21"].ToString().Trim());
                                                break;
                                            case 22:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon22"].ToString().Trim());
                                                break;
                                            case 23:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon23"].ToString().Trim());
                                                break;
                                            case 24:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon24"].ToString().Trim());
                                                break;
                                            case 25:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon25"].ToString().Trim());
                                                break;
                                            case 26:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon26"].ToString().Trim());
                                                break;
                                            case 27:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon27"].ToString().Trim());
                                                break;
                                            case 28:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon28"].ToString().Trim());
                                                break;
                                            case 29:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon29"].ToString().Trim());
                                                break;
                                            case 30:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon30"].ToString().Trim());
                                                break;
                                            case 31:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon31"].ToString().Trim());
                                                break;
                                            case 32:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon32"].ToString().Trim());
                                                break;
                                            case 33:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon33"].ToString().Trim());
                                                break;
                                            case 34:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon34"].ToString().Trim());
                                                break;
                                            case 35:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon35"].ToString().Trim());
                                                break;
                                            case 36:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon36"].ToString().Trim());
                                                break;
                                            case 37:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon37"].ToString().Trim());
                                                break;
                                            case 38:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon38"].ToString().Trim());
                                                break;
                                            case 39:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon39"].ToString().Trim());
                                                break;
                                            case 40:
                                                _ValorRenglon3 = Double.Parse(rowItem2["valor_renglon40"].ToString().Trim());
                                                break;
                                            default:
                                                break;
                                        }
                                        #endregion
                                        //--RENGLON 4
                                        #region OBTENER VALOR DE LIQUIDACION DEL RENGLON 4
                                        switch (_NumRenglon4)
                                        {
                                            case 8:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon8"].ToString().Trim());
                                                break;
                                            case 9:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon9"].ToString().Trim());
                                                break;
                                            case 10:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon10"].ToString().Trim());
                                                break;
                                            case 11:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon11"].ToString().Trim());
                                                break;
                                            case 12:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon12"].ToString().Trim());
                                                break;
                                            case 13:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon13"].ToString().Trim());
                                                break;
                                            case 14:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon14"].ToString().Trim());
                                                break;
                                            case 15:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon15"].ToString().Trim());
                                                break;
                                            case 16:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon16"].ToString().Trim());
                                                break;
                                            case 17:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon17"].ToString().Trim());
                                                break;
                                            case 19:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon19"].ToString().Trim());
                                                break;
                                            case 20:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon20"].ToString().Trim());
                                                break;
                                            case 21:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon21"].ToString().Trim());
                                                break;
                                            case 22:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon22"].ToString().Trim());
                                                break;
                                            case 23:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon23"].ToString().Trim());
                                                break;
                                            case 24:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon24"].ToString().Trim());
                                                break;
                                            case 25:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon25"].ToString().Trim());
                                                break;
                                            case 26:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon26"].ToString().Trim());
                                                break;
                                            case 27:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon27"].ToString().Trim());
                                                break;
                                            case 28:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon28"].ToString().Trim());
                                                break;
                                            case 29:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon29"].ToString().Trim());
                                                break;
                                            case 30:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon30"].ToString().Trim());
                                                break;
                                            case 31:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon31"].ToString().Trim());
                                                break;
                                            case 32:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon32"].ToString().Trim());
                                                break;
                                            case 33:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon33"].ToString().Trim());
                                                break;
                                            case 34:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon34"].ToString().Trim());
                                                break;
                                            case 35:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon35"].ToString().Trim());
                                                break;
                                            case 36:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon36"].ToString().Trim());
                                                break;
                                            case 37:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon37"].ToString().Trim());
                                                break;
                                            case 38:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon38"].ToString().Trim());
                                                break;
                                            case 39:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon39"].ToString().Trim());
                                                break;
                                            case 40:
                                                _ValorRenglon4 = Double.Parse(rowItem2["valor_renglon40"].ToString().Trim());
                                                break;
                                            default:
                                                break;
                                        }
                                        #endregion
                                        //--RENGLON 5
                                        #region OBTENER VALOR DE LIQUIDACION DEL RENGLON 5
                                        switch (_NumRenglon5)
                                        {
                                            case 8:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon8"].ToString().Trim());
                                                break;
                                            case 9:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon9"].ToString().Trim());
                                                break;
                                            case 10:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon10"].ToString().Trim());
                                                break;
                                            case 11:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon11"].ToString().Trim());
                                                break;
                                            case 12:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon12"].ToString().Trim());
                                                break;
                                            case 13:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon13"].ToString().Trim());
                                                break;
                                            case 14:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon14"].ToString().Trim());
                                                break;
                                            case 15:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon15"].ToString().Trim());
                                                break;
                                            case 16:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon16"].ToString().Trim());
                                                break;
                                            case 17:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon17"].ToString().Trim());
                                                break;
                                            case 19:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon19"].ToString().Trim());
                                                break;
                                            case 20:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon20"].ToString().Trim());
                                                break;
                                            case 21:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon21"].ToString().Trim());
                                                break;
                                            case 22:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon22"].ToString().Trim());
                                                break;
                                            case 23:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon23"].ToString().Trim());
                                                break;
                                            case 24:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon24"].ToString().Trim());
                                                break;
                                            case 25:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon25"].ToString().Trim());
                                                break;
                                            case 26:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon26"].ToString().Trim());
                                                break;
                                            case 27:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon27"].ToString().Trim());
                                                break;
                                            case 28:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon28"].ToString().Trim());
                                                break;
                                            case 29:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon29"].ToString().Trim());
                                                break;
                                            case 30:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon30"].ToString().Trim());
                                                break;
                                            case 31:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon31"].ToString().Trim());
                                                break;
                                            case 32:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon32"].ToString().Trim());
                                                break;
                                            case 33:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon33"].ToString().Trim());
                                                break;
                                            case 34:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon34"].ToString().Trim());
                                                break;
                                            case 35:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon35"].ToString().Trim());
                                                break;
                                            case 36:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon36"].ToString().Trim());
                                                break;
                                            case 37:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon37"].ToString().Trim());
                                                break;
                                            case 38:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon38"].ToString().Trim());
                                                break;
                                            case 39:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon39"].ToString().Trim());
                                                break;
                                            case 40:
                                                _ValorRenglon5 = Double.Parse(rowItem2["valor_renglon40"].ToString().Trim());
                                                break;
                                            default:
                                                break;
                                        }
                                        #endregion
                                        //--RENGLON 6
                                        #region OBTENER VALOR DE LIQUIDACION DEL RENGLON 6
                                        switch (_NumRenglon6)
                                        {
                                            case 8:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon8"].ToString().Trim());
                                                break;
                                            case 9:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon9"].ToString().Trim());
                                                break;
                                            case 10:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon10"].ToString().Trim());
                                                break;
                                            case 11:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon11"].ToString().Trim());
                                                break;
                                            case 12:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon12"].ToString().Trim());
                                                break;
                                            case 13:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon13"].ToString().Trim());
                                                break;
                                            case 14:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon14"].ToString().Trim());
                                                break;
                                            case 15:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon15"].ToString().Trim());
                                                break;
                                            case 16:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon16"].ToString().Trim());
                                                break;
                                            case 17:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon17"].ToString().Trim());
                                                break;
                                            case 19:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon19"].ToString().Trim());
                                                break;
                                            case 20:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon20"].ToString().Trim());
                                                break;
                                            case 21:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon21"].ToString().Trim());
                                                break;
                                            case 22:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon22"].ToString().Trim());
                                                break;
                                            case 23:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon23"].ToString().Trim());
                                                break;
                                            case 24:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon24"].ToString().Trim());
                                                break;
                                            case 25:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon25"].ToString().Trim());
                                                break;
                                            case 26:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon26"].ToString().Trim());
                                                break;
                                            case 27:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon27"].ToString().Trim());
                                                break;
                                            case 28:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon28"].ToString().Trim());
                                                break;
                                            case 29:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon29"].ToString().Trim());
                                                break;
                                            case 30:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon30"].ToString().Trim());
                                                break;
                                            case 31:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon31"].ToString().Trim());
                                                break;
                                            case 32:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon32"].ToString().Trim());
                                                break;
                                            case 33:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon33"].ToString().Trim());
                                                break;
                                            case 34:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon34"].ToString().Trim());
                                                break;
                                            case 35:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon35"].ToString().Trim());
                                                break;
                                            case 36:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon36"].ToString().Trim());
                                                break;
                                            case 37:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon37"].ToString().Trim());
                                                break;
                                            case 38:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon38"].ToString().Trim());
                                                break;
                                            case 39:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon39"].ToString().Trim());
                                                break;
                                            case 40:
                                                _ValorRenglon6 = Double.Parse(rowItem2["valor_renglon40"].ToString().Trim());
                                                break;
                                            default:
                                                break;
                                        }
                                        #endregion
                                        //--RENGLON 7
                                        #region OBTENER VALOR DE LIQUIDACION DEL RENGLON 7
                                        switch (_NumRenglon7)
                                        {
                                            case 8:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon8"].ToString().Trim());
                                                break;
                                            case 9:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon9"].ToString().Trim());
                                                break;
                                            case 10:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon10"].ToString().Trim());
                                                break;
                                            case 11:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon11"].ToString().Trim());
                                                break;
                                            case 12:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon12"].ToString().Trim());
                                                break;
                                            case 13:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon13"].ToString().Trim());
                                                break;
                                            case 14:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon14"].ToString().Trim());
                                                break;
                                            case 15:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon15"].ToString().Trim());
                                                break;
                                            case 16:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon16"].ToString().Trim());
                                                break;
                                            case 17:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon17"].ToString().Trim());
                                                break;
                                            case 19:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon19"].ToString().Trim());
                                                break;
                                            case 20:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon20"].ToString().Trim());
                                                break;
                                            case 21:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon21"].ToString().Trim());
                                                break;
                                            case 22:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon22"].ToString().Trim());
                                                break;
                                            case 23:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon23"].ToString().Trim());
                                                break;
                                            case 24:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon24"].ToString().Trim());
                                                break;
                                            case 25:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon25"].ToString().Trim());
                                                break;
                                            case 26:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon26"].ToString().Trim());
                                                break;
                                            case 27:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon27"].ToString().Trim());
                                                break;
                                            case 28:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon28"].ToString().Trim());
                                                break;
                                            case 29:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon29"].ToString().Trim());
                                                break;
                                            case 30:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon30"].ToString().Trim());
                                                break;
                                            case 31:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon31"].ToString().Trim());
                                                break;
                                            case 32:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon32"].ToString().Trim());
                                                break;
                                            case 33:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon33"].ToString().Trim());
                                                break;
                                            case 34:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon34"].ToString().Trim());
                                                break;
                                            case 35:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon35"].ToString().Trim());
                                                break;
                                            case 36:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon36"].ToString().Trim());
                                                break;
                                            case 37:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon37"].ToString().Trim());
                                                break;
                                            case 38:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon38"].ToString().Trim());
                                                break;
                                            case 39:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon39"].ToString().Trim());
                                                break;
                                            case 40:
                                                _ValorRenglon7 = Double.Parse(rowItem2["valor_renglon40"].ToString().Trim());
                                                break;
                                            default:
                                                break;
                                        }
                                        #endregion
                                        //--RENGLON 8
                                        #region OBTENER VALOR DE LIQUIDACION DEL RENGLON 8
                                        switch (_NumRenglon8)
                                        {
                                            case 8:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon8"].ToString().Trim());
                                                break;
                                            case 9:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon9"].ToString().Trim());
                                                break;
                                            case 10:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon10"].ToString().Trim());
                                                break;
                                            case 11:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon11"].ToString().Trim());
                                                break;
                                            case 12:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon12"].ToString().Trim());
                                                break;
                                            case 13:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon13"].ToString().Trim());
                                                break;
                                            case 14:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon14"].ToString().Trim());
                                                break;
                                            case 15:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon15"].ToString().Trim());
                                                break;
                                            case 16:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon16"].ToString().Trim());
                                                break;
                                            case 17:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon17"].ToString().Trim());
                                                break;
                                            case 19:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon19"].ToString().Trim());
                                                break;
                                            case 20:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon20"].ToString().Trim());
                                                break;
                                            case 21:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon21"].ToString().Trim());
                                                break;
                                            case 22:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon22"].ToString().Trim());
                                                break;
                                            case 23:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon23"].ToString().Trim());
                                                break;
                                            case 24:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon24"].ToString().Trim());
                                                break;
                                            case 25:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon25"].ToString().Trim());
                                                break;
                                            case 26:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon26"].ToString().Trim());
                                                break;
                                            case 27:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon27"].ToString().Trim());
                                                break;
                                            case 28:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon28"].ToString().Trim());
                                                break;
                                            case 29:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon29"].ToString().Trim());
                                                break;
                                            case 30:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon30"].ToString().Trim());
                                                break;
                                            case 31:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon31"].ToString().Trim());
                                                break;
                                            case 32:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon32"].ToString().Trim());
                                                break;
                                            case 33:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon33"].ToString().Trim());
                                                break;
                                            case 34:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon34"].ToString().Trim());
                                                break;
                                            case 35:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon35"].ToString().Trim());
                                                break;
                                            case 36:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon36"].ToString().Trim());
                                                break;
                                            case 37:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon37"].ToString().Trim());
                                                break;
                                            case 38:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon38"].ToString().Trim());
                                                break;
                                            case 39:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon39"].ToString().Trim());
                                                break;
                                            case 40:
                                                _ValorRenglon8 = Double.Parse(rowItem2["valor_renglon40"].ToString().Trim());
                                                break;
                                            default:
                                                break;
                                        }
                                        #endregion
                                        //--RENGLON 9
                                        #region OBTENER VALOR DE LIQUIDACION DEL RENGLON 9
                                        switch (_NumRenglon9)
                                        {
                                            case 8:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon8"].ToString().Trim());
                                                break;
                                            case 9:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon9"].ToString().Trim());
                                                break;
                                            case 10:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon10"].ToString().Trim());
                                                break;
                                            case 11:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon11"].ToString().Trim());
                                                break;
                                            case 12:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon12"].ToString().Trim());
                                                break;
                                            case 13:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon13"].ToString().Trim());
                                                break;
                                            case 14:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon14"].ToString().Trim());
                                                break;
                                            case 15:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon15"].ToString().Trim());
                                                break;
                                            case 16:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon16"].ToString().Trim());
                                                break;
                                            case 17:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon17"].ToString().Trim());
                                                break;
                                            case 19:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon19"].ToString().Trim());
                                                break;
                                            case 20:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon20"].ToString().Trim());
                                                break;
                                            case 21:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon21"].ToString().Trim());
                                                break;
                                            case 22:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon22"].ToString().Trim());
                                                break;
                                            case 23:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon23"].ToString().Trim());
                                                break;
                                            case 24:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon24"].ToString().Trim());
                                                break;
                                            case 25:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon25"].ToString().Trim());
                                                break;
                                            case 26:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon26"].ToString().Trim());
                                                break;
                                            case 27:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon27"].ToString().Trim());
                                                break;
                                            case 28:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon28"].ToString().Trim());
                                                break;
                                            case 29:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon29"].ToString().Trim());
                                                break;
                                            case 30:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon30"].ToString().Trim());
                                                break;
                                            case 31:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon31"].ToString().Trim());
                                                break;
                                            case 32:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon32"].ToString().Trim());
                                                break;
                                            case 33:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon33"].ToString().Trim());
                                                break;
                                            case 34:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon34"].ToString().Trim());
                                                break;
                                            case 35:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon35"].ToString().Trim());
                                                break;
                                            case 36:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon36"].ToString().Trim());
                                                break;
                                            case 37:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon37"].ToString().Trim());
                                                break;
                                            case 38:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon38"].ToString().Trim());
                                                break;
                                            case 39:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon39"].ToString().Trim());
                                                break;
                                            case 40:
                                                _ValorRenglon9 = Double.Parse(rowItem2["valor_renglon40"].ToString().Trim());
                                                break;
                                            default:
                                                break;
                                        }
                                        #endregion
                                        //--RENGLON 10
                                        #region OBTENER VALOR DE LIQUIDACION DEL RENGLON 10
                                        switch (_NumRenglon10)
                                        {
                                            case 8:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon8"].ToString().Trim());
                                                break;
                                            case 9:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon9"].ToString().Trim());
                                                break;
                                            case 10:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon10"].ToString().Trim());
                                                break;
                                            case 11:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon11"].ToString().Trim());
                                                break;
                                            case 12:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon12"].ToString().Trim());
                                                break;
                                            case 13:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon13"].ToString().Trim());
                                                break;
                                            case 14:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon14"].ToString().Trim());
                                                break;
                                            case 15:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon15"].ToString().Trim());
                                                break;
                                            case 16:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon16"].ToString().Trim());
                                                break;
                                            case 17:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon17"].ToString().Trim());
                                                break;
                                            case 19:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon19"].ToString().Trim());
                                                break;
                                            case 20:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon20"].ToString().Trim());
                                                break;
                                            case 21:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon21"].ToString().Trim());
                                                break;
                                            case 22:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon22"].ToString().Trim());
                                                break;
                                            case 23:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon23"].ToString().Trim());
                                                break;
                                            case 24:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon24"].ToString().Trim());
                                                break;
                                            case 25:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon25"].ToString().Trim());
                                                break;
                                            case 26:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon26"].ToString().Trim());
                                                break;
                                            case 27:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon27"].ToString().Trim());
                                                break;
                                            case 28:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon28"].ToString().Trim());
                                                break;
                                            case 29:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon29"].ToString().Trim());
                                                break;
                                            case 30:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon30"].ToString().Trim());
                                                break;
                                            case 31:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon31"].ToString().Trim());
                                                break;
                                            case 32:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon32"].ToString().Trim());
                                                break;
                                            case 33:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon33"].ToString().Trim());
                                                break;
                                            case 34:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon34"].ToString().Trim());
                                                break;
                                            case 35:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon35"].ToString().Trim());
                                                break;
                                            case 36:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon36"].ToString().Trim());
                                                break;
                                            case 37:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon37"].ToString().Trim());
                                                break;
                                            case 38:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon38"].ToString().Trim());
                                                break;
                                            case 39:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon39"].ToString().Trim());
                                                break;
                                            case 40:
                                                _ValorRenglon10 = Double.Parse(rowItem2["valor_renglon40"].ToString().Trim());
                                                break;
                                            default:
                                                break;
                                        }
                                        #endregion
                                        #endregion
                                    }

                                    //--AQUI REALIZAMOS LA OPERACION PARA OBTENER LA SUMATORIA TOTAL DEL MUNICIPIO
                                    #region AQUI REALIZAMOS LA OPERACION DEL VALOR TOTAL
                                    //--TIPOS DE OPERACION: 1. SUMA, 2. RESTA
                                    if (_TipoOperacion1 > 0)
                                    {
                                        if (_TipoOperacion1 == 1)
                                        {
                                            _ValorTotal = _ValorRenglon1 + _ValorRenglon2;
                                        }
                                        else if (_TipoOperacion1 == 2)
                                        {
                                            _ValorTotal = _ValorRenglon1 - _ValorRenglon2;
                                        }
                                    }
                                    else
                                    {
                                        _ValorTotal = _ValorRenglon1;
                                    }

                                    //--
                                    if (_TipoOperacion2 > 0)
                                    {
                                        if (_TipoOperacion2 == 1)
                                        {
                                            _ValorTotal = _ValorTotal + _ValorRenglon3;
                                        }
                                        else if (_TipoOperacion2 == 2)
                                        {
                                            _ValorTotal = _ValorTotal - -_ValorRenglon3;
                                        }
                                    }

                                    //--
                                    if (_TipoOperacion3 > 0)
                                    {
                                        if (_TipoOperacion3 == 1)
                                        {
                                            _ValorTotal = _ValorTotal + _ValorRenglon4;
                                        }
                                        else if (_TipoOperacion3 == 2)
                                        {
                                            _ValorTotal = _ValorTotal - _ValorRenglon4;
                                        }
                                    }

                                    //--
                                    if (_TipoOperacion4 > 0)
                                    {
                                        if (_TipoOperacion4 == 1)
                                        {
                                            _ValorTotal = _ValorTotal + _ValorRenglon5;
                                        }
                                        else if (_TipoOperacion4 == 2)
                                        {
                                            _ValorTotal = _ValorTotal - _ValorRenglon5;
                                        }
                                    }

                                    //--
                                    if (_TipoOperacion5 > 0)
                                    {
                                        if (_TipoOperacion5 == 1)
                                        {
                                            _ValorTotal = _ValorTotal + _ValorRenglon6;
                                        }
                                        else if (_TipoOperacion5 == 2)
                                        {
                                            _ValorTotal = _ValorTotal - _ValorRenglon6;
                                        }
                                    }

                                    //--
                                    if (_TipoOperacion6 > 0)
                                    {
                                        if (_TipoOperacion6 == 1)
                                        {
                                            _ValorTotal = _ValorTotal + _ValorRenglon7;
                                        }
                                        else if (_TipoOperacion6 == 2)
                                        {
                                            _ValorTotal = _ValorTotal - _ValorRenglon7;
                                        }
                                    }

                                    //--
                                    if (_TipoOperacion7 > 0)
                                    {
                                        if (_TipoOperacion7 == 1)
                                        {
                                            _ValorTotal = _ValorTotal + _ValorRenglon8;
                                        }
                                        else if (_TipoOperacion7 == 2)
                                        {
                                            _ValorTotal = _ValorTotal - _ValorRenglon8;
                                        }
                                    }

                                    //--
                                    if (_TipoOperacion8 > 0)
                                    {
                                        if (_TipoOperacion8 == 1)
                                        {
                                            _ValorTotal = _ValorTotal + _ValorRenglon9;
                                        }
                                        else if (_TipoOperacion8 == 2)
                                        {
                                            _ValorTotal = _ValorTotal - _ValorRenglon9;
                                        }
                                    }

                                    //--
                                    if (_TipoOperacion9 > 0)
                                    {
                                        if (_TipoOperacion9 == 1)
                                        {
                                            _ValorTotal = _ValorTotal + _ValorRenglon10;
                                        }
                                        else if (_TipoOperacion9 == 2)
                                        {
                                            _ValorTotal = _ValorTotal - _ValorRenglon10;
                                        }
                                    }
                                    #endregion

                                    //--AQUI OBTENEMOS EL DESCUENTO GANADO POR PRONTO PAGO POR MUNICIPIO
                                    #region DESCUENTO GANADO POR PRONTO PAGO POR MUNICIPIO
                                    if (dtVencimientos != null)
                                    {
                                        if (dtVencimientos.Rows.Count > 0)
                                        {
                                            //--AQUI FILTRAMOS POR MUNICIPIO
                                            DataRow[] dataRows3 = dtVencimientos.Select("id_municipio = " + _IdMunicipio);
                                            //--
                                            int _ContadorRows = 0;
                                            foreach (DataRow rowItem3 in dataRows3)
                                            {
                                                double _PorcDescuento = Double.Parse(dataRows3[_ContadorRows]["valor_descuento"].ToString().Trim().Replace("$ ", "").Replace(".", ""));
                                                double _DescuentoGanado = ((_ValorTotal * _PorcDescuento) / 100);
                                                dataRows3[_ContadorRows]["descuento_ganado"] = String.Format(String.Format("{0:$ ###,###,##0}", _DescuentoGanado));
                                                dtVencimientos.Rows[0].AcceptChanges();
                                                dtVencimientos.Rows[0].EndEdit();
                                                _ContadorRows++;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }

                        //--AQUI ADICIONAMOS EL DATATABLE DE VENCIMIENTOS AL DATASET
                        this.ViewState["DtExportar"] = dtVencimientos;
                        ObjetoDataSet.Tables.Add(dtVencimientos);
                    }
                    else
                    {
                        #region AQUI OBTENEMOS LOS VENCIMIENTO DE CADA MUNICIPIO
                        ObjMunCalendario.TipoConsulta = 3;
                        ObjMunCalendario.IdMunicipio = null;
                        ObjMunCalendario.IdCliente = this.Session["IdCliente"] != null ? this.Session["IdCliente"].ToString().Trim() : null;
                        ObjMunCalendario.IdEstado = 1;
                        ObjMunCalendario.MotorBaseDatos = this.Session["MotorBaseDatos"].ToString().Trim();

                        //Mostrar los impuestos por municipio
                        dtVencimientos = ObjMunCalendario.GetVencimientosMunicipio();
                        dtVencimientos.PrimaryKey = new DataColumn[] { ObjetoDataTable.Columns["id_registro"] };

                        //--AQUI ADICIONAMOS EL DATATABLE DE VENCIMIENTOS AL DATASET
                        DataTable DtDatos = new DataTable();
                        DtDatos = dtVencimientos.Clone();
                        this.ViewState["DtExportar"] = DtDatos;
                        ObjetoDataSet.Tables.Add(DtDatos);
                        #endregion

                        #region MOSTRAR MENSAJE DE USUARIO
                        //Mostramos el mensaje porque se produjo un error con la Trx.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgError = "Señor usuario, no se encontro información con los datos suministrados. Por favor validar e intentar nuevamente !";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                        Ventana.ID = "RadWindow2";
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(300);
                        Ventana.Width = Unit.Pixel(600);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Mensaje del Sistema";
                        Ventana.VisibleStatusbar = false;
                        Ventana.Behaviors = WindowBehaviors.Close;
                        this.RadWindowManager1.Windows.Add(Ventana);
                        this.RadWindowManager1 = null;
                        Ventana = null;
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Error al listar los tipos de convenio. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                #endregion
            }

            return ObjetoDataSet;
        }

        private DataSet FuenteDatos
        {
            get
            {
                object obj = this.ViewState["_FuenteDatos"];
                if (((obj != null)))
                {
                    return (DataSet)obj;
                }
                else
                {
                    DataSet ConjuntoDatos = new DataSet();
                    ConjuntoDatos = GetDatosGrilla();
                    this.ViewState["_FuenteDatos"] = ConjuntoDatos;
                    return (DataSet)this.ViewState["_FuenteDatos"];
                }
            }
            set { this.ViewState["_FuenteDatos"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page.IsPostBack))
            {
                //--OBTENER VALORES
                this.ViewState["IdImpuesto"] = Request.QueryString["IdImpuesto"].ToString().Trim();
                this.ViewState["AnioGravable"] = Request.QueryString["AnioGravable"].ToString().Trim();
                this.ViewState["DtExportar"] = null;
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
            else
            {
                ObjUtils.CambiarGrillaAEspanol(RadGrid1);
            }
        }

        protected override void SavePageStateToPersistenceMedium(object state)
        {
            string str = string.Format("VS_{0}_{1}", Request.UserHostAddress, DateTime.Now.Ticks);
            Cache.Add(str, state, null, DateTime.Now.AddMinutes(Session.Timeout), TimeSpan.Zero, CacheItemPriority.Default, null);
            ClientScript.RegisterHiddenField("__VIEWSTATE_KEY", str);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            string str = Request.Form["__VIEWSTATE_KEY"];
            if (!str.StartsWith("VS_"))
            {
                throw new Exception("Invalid ViewState");
            }
            return Cache[str];
        }

        #region DEFINICION DE METODOS DEL GRID
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                this.RadGrid1.DataSource = this.FuenteDatos;
                this.RadGrid1.DataMember = "DtVencMunicipio";
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Error con el evento RadGrid1_NeedDataSource del Impuesto del municipio. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Error(_MsgError);
                #endregion
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "BtnExportarExcel")
                {
                    DataTable dtExportar = new DataTable();
                    dtExportar = (DataTable)this.ViewState["DtExportar"];
                    ExportarDatosExcel(dtExportar);
                }
                else
                {
                    //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                    this.RadWindowManager1.Enabled = false;
                    this.RadWindowManager1.EnableAjaxSkinRendering = false;
                    this.RadWindowManager1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgMensaje = "Error con el evento ItemCommand de la grilla. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgMensaje;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Error(_MsgMensaje.Trim());
                #endregion
            }
        }

        protected void RadGrid1_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            try
            {
                RadGrid1.Rebind();
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                //Mostramos el mensaje porque se produjo un error con la Trx.
                this.RadWindowManager1.ReloadOnShow = true;
                this.RadWindowManager1.DestroyOnClose = true;
                this.RadWindowManager1.Windows.Clear();
                this.RadWindowManager1.Enabled = true;
                this.RadWindowManager1.EnableAjaxSkinRendering = true;
                this.RadWindowManager1.Visible = true;

                RadWindow Ventana = new RadWindow();
                Ventana.Modal = true;
                string _MsgError = "Error con el evento RadGrid1_PageIndexChanged del Impuesto del municipio. Motivo: " + ex.ToString();
                Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                Ventana.ID = "RadWindow2";
                Ventana.VisibleOnPageLoad = true;
                Ventana.Visible = true;
                Ventana.Height = Unit.Pixel(300);
                Ventana.Width = Unit.Pixel(600);
                Ventana.KeepInScreenBounds = true;
                Ventana.Title = "Mensaje del Sistema";
                Ventana.VisibleStatusbar = false;
                Ventana.Behaviors = WindowBehaviors.Close;
                this.RadWindowManager1.Windows.Add(Ventana);
                this.RadWindowManager1 = null;
                Ventana = null;
                _log.Error(_MsgError);
                #endregion
            }
        }
        #endregion

        //Metodo que permite exportar la informacion a excel.
        protected void ExportarDatosExcel(DataTable DtDatosExportar)
        {
            try
            {
                if (DtDatosExportar.Rows.Count > 0)
                {
                    //Aqui se comienza a escribir los datos en el archivo de excel que sera enviado por correo
                    DateTime FechaActual = DateTime.Now;
                    string _MesActual = Convert.ToString(FechaActual.ToString("MMMM"));
                    string _DiaActual = Convert.ToString(FechaActual.ToString("dd"));

                    //Console.WriteLine("Generando el archivo de excel. \n");
                    string cNombreFileExcel = "RptVencimientos_" + _DiaActual + "_" + _MesActual + ".xlsx";

                    #region IMPRIMIR ENCABEZADO DEL ARCHIVO DE EXCEL
                    IWorkbook book = Factory.CreateWorkbook();
                    IWorksheet sheet = book.Worksheets.Add();
                    int Row = 4;
                    int ContadorRow = 3;
                    int ContadorCol = 2;
                    int Contador = 0;
                    int CantidadCol = DtDatosExportar.Columns.Count + 1;

                    sheet.Range[2, 2, 2, CantidadCol].Merge();
                    string strNombreEncabezadoReporte = "FECHAS DE VENCIMIENTOS CALENDARIO TRIBUTARIO";
                    sheet.Range[2, 2, 2, CantidadCol].Value = strNombreEncabezadoReporte;
                    sheet.Range[2, 2, 2, CantidadCol].Font.Size = 18;
                    sheet.Range[2, 2, 2, CantidadCol].ColumnWidth = 30;
                    sheet.Range[2, 2, 2, CantidadCol].Font.Bold = true;
                    //sheet.Range[2, 2, 2, CantidadCol].Font.Color = Color.White;
                    sheet.Range[2, 2, 2, CantidadCol].Interior.Color = Color.Silver;
                    sheet.Range[2, 2, 2, CantidadCol].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    sheet.Range[2, 2, 2, CantidadCol].Borders.LineStyle = XlLineStyle.xlContinuous;
                    sheet.Range[2, 2, 2, CantidadCol].Borders.Weight = XlBorderWeight.xlMedium;
                    #endregion

                    for (int ncol = 0; ncol < DtDatosExportar.Columns.Count; ncol++)
                    {
                        #region IMPRIMIR NOMBRE DE COLUMNAS DEL REPORTE
                        //AQUI OBTENEMOS LOS NOMBRES DE LAS COLUMNAS DEL DATATABLE
                        string strNombreColum = DtDatosExportar.Columns[ncol].ColumnName.ToString().Trim().ToUpper();
                        sheet.Range[ContadorRow, ContadorCol].Value = strNombreColum;
                        sheet.Range[ContadorRow, ContadorCol].Font.Bold = true;
                        sheet.Range[ContadorRow, ContadorCol].Font.Size = 12;
                        sheet.Range[ContadorRow, ContadorCol].ColumnWidth = 10;
                        sheet.Range[ContadorRow, ContadorCol].Interior.Color = Color.Silver;
                        sheet.Range[ContadorRow, ContadorCol].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        sheet.Range[ContadorRow, ContadorCol].Borders.LineStyle = XlLineStyle.xlDash;
                        sheet.Range[ContadorRow, ContadorCol].Borders.Weight = XlBorderWeight.xlMedium;
                        #endregion

                        Row = 4;
                        for (int nrow = 0; nrow < DtDatosExportar.Rows.Count; nrow++)
                        {
                            #region DETALLE DE LAS TRANSACCIONES EN EL ARCHIVO DE EXCEL
                            if (ncol == 7)
                            {
                                sheet.Cells[Row, ContadorCol].Value = DtDatosExportar.Rows[nrow][ncol].ToString().Trim() + "%";
                                sheet.Cells[Row, ContadorCol].Font.Size = 12;
                                sheet.Cells[Row, ContadorCol].ColumnWidth = 20;
                            }
                            else
                            {
                                sheet.Cells[Row, ContadorCol].Value = DtDatosExportar.Rows[nrow][ncol].ToString().Trim();
                                sheet.Cells[Row, ContadorCol].Font.Size = 12;
                                sheet.Cells[Row, ContadorCol].ColumnWidth = 20;
                            }
                            //sheet.Cells[Row, ContadorCol].Font.Color = Color.White;

                            Row++;
                            Contador++;
                            #endregion
                        }

                        ContadorCol++;
                    }

                    if (Contador > 0)
                    {
                        #region DESCARGAR EL ARCHIVO DE EXCEL
                        //Aqui deshabilitamos el control RadWindowManager1 para que no vuelva a mostrar la ventana del Popup
                        this.RadWindowManager1.Enabled = false;
                        this.RadWindowManager1.EnableAjaxSkinRendering = false;
                        this.RadWindowManager1.Visible = false;

                        //Abrir el archivo de excel
                        book.Worksheets["Sheet1"].Name = "VENCIMIENTOS_CALENDARIO";
                        book.Worksheets["VENCIMIENTOS_CALENDARIO"].UsedRange.Autofit();
                        Response.Clear();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + cNombreFileExcel.ToString().Trim());
                        book.SaveAs(Response.OutputStream, XlFileFormat.xlOpenXMLWorkbook);
                        //book.SaveAs(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                        #endregion
                    }
                    else
                    {
                        #region MOSTRAR MENSAJE DE USUARIO
                        //Mostramos el mensaje porque se produjo un error con la Trx.
                        this.RadWindowManager1.ReloadOnShow = true;
                        this.RadWindowManager1.DestroyOnClose = true;
                        this.RadWindowManager1.Windows.Clear();
                        this.RadWindowManager1.Enabled = true;
                        this.RadWindowManager1.EnableAjaxSkinRendering = true;
                        this.RadWindowManager1.Visible = true;

                        RadWindow Ventana = new RadWindow();
                        Ventana.Modal = true;
                        string _MsgError = "No hay información para mostrar en el reporte de excel.";
                        Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                        Ventana.ID = "RadWindow2";
                        Ventana.VisibleOnPageLoad = true;
                        Ventana.Visible = true;
                        Ventana.Height = Unit.Pixel(300);
                        Ventana.Width = Unit.Pixel(600);
                        Ventana.KeepInScreenBounds = true;
                        Ventana.Title = "Mensaje del Sistema";
                        Ventana.VisibleStatusbar = false;
                        Ventana.Behaviors = WindowBehaviors.Close;
                        this.RadWindowManager1.Windows.Add(Ventana);
                        this.RadWindowManager1 = null;
                        Ventana = null;
                        _log.Error(_MsgError);
                        #endregion
                    }
                }
                else
                {
                    #region MOSTRAR MENSAJE DE USUARIO
                    //Mostramos el mensaje porque se produjo un error con la Trx.
                    this.RadWindowManager1.ReloadOnShow = true;
                    this.RadWindowManager1.DestroyOnClose = true;
                    this.RadWindowManager1.Windows.Clear();
                    this.RadWindowManager1.Enabled = true;
                    this.RadWindowManager1.EnableAjaxSkinRendering = true;
                    this.RadWindowManager1.Visible = true;

                    RadWindow Ventana = new RadWindow();
                    Ventana.Modal = true;
                    string _MsgError = "No hay información para exportar a Excel.";
                    Ventana.NavigateUrl = "/Controles/General/FrmMensaje.aspx?strMensaje=" + _MsgError;
                    Ventana.ID = "RadWindow2";
                    Ventana.VisibleOnPageLoad = true;
                    Ventana.Visible = true;
                    Ventana.Height = Unit.Pixel(300);
                    Ventana.Width = Unit.Pixel(600);
                    Ventana.KeepInScreenBounds = true;
                    Ventana.Title = "Mensaje del Sistema";
                    Ventana.VisibleStatusbar = false;
                    Ventana.Behaviors = WindowBehaviors.Close;
                    this.RadWindowManager1.Windows.Add(Ventana);
                    this.RadWindowManager1 = null;
                    Ventana = null;
                    _log.Error(_MsgError);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region MOSTRAR MENSAJE DE USUARIO
                string _Excepcion = ex.Message.ToString().Trim().Replace(".", "");
                _log.Error(_Excepcion);
                #endregion
            }
        }

    }
}