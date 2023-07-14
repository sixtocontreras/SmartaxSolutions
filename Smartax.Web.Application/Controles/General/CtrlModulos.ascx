<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlModulos.ascx.cs" Inherits="Smartax.Web.Application.Controles.General.CtrlModulos" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<body bgcolor="#E6E6E6">
    <form id="form2">
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="PnlTrxGeneral" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td colspan="8" align="center" bgcolor="#999999">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="MODULOS DEL SISTEMA" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImgPlaneacionFiscal" runat="server" ImageUrl="~/Imagenes/Modulos/img_planeacion_fiscal.png" OnClick="ImgPlaneacionFiscal_Click" /></td>
                            <td align="center">
                                <asp:ImageButton ID="ImgInfoTributaria" runat="server" ImageUrl="~/Imagenes/Modulos/img_info_tributaria.png" OnClick="ImgInfoTributaria_Click" /></td>
                            <td align="center">
                                <asp:ImageButton ID="ImgControlActividades" runat="server" ImageUrl="~/Imagenes/Modulos/img_control_actividades.png" OnClick="ImgControlActividades_Click" /></td>
                            <td align="center">
                                <asp:ImageButton ID="imgFormatosSFC" runat="server" ImageUrl="~/Imagenes/Modulos/img_sfc.png" OnClick="imgFormatosSFC_Click" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImageNormativa" runat="server" ImageUrl="~/Imagenes/Modulos/img_normatividad.png" OnClick="ImgControlNormatividad_Click" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImageReporte" runat="server" ImageUrl="~/Imagenes/Modulos/img_reportes.png" OnClick="ImgControlReportes_Click" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImageRequerimiento" runat="server" ImageUrl="~/Imagenes/Modulos/img_Control_Requerimientos.png" OnClick="ImgControlRequerimiento_Click" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImageCustodia" runat="server" ImageUrl="~/Imagenes/Modulos/img_Custodia_Imagenes.png" OnClick="ImgControlcustodia_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="~/Imagenes/Modulos/img_info_tributaria.png" Visible="False" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/Imagenes/Modulos/img_info_tributaria.png" Visible="False" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/Imagenes/Modulos/img_info_tributaria.png" Visible="False" />
                            </td>
                            <td align="center">&nbsp;</td>
                            <td align="center">&#160;</td>
                        </tr>
                        <tr>
                            <td>&#160;</td>
                            <td>&#160;</td>
                            <td>&#160;</td>
                            <td>&#160;</td>
                            <td>&#160;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="PnlPlaneacionFiscal" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Width="100%" Visible="False">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td colspan="4" align="center" bgcolor="#999999">
                                <asp:Label ID="Label1" runat="server" CssClass="SubTitle" Text="MODULOS PLANEACION FISCAL" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImgCalendarioTributario" runat="server" ImageUrl="~/Imagenes/Modulos/img_calendarios_tributarios.png" OnClick="ImgCalendarioTributario_Click" ToolTip="Ver calendarios tributarios por municipios" /></td>
                            <td align="center">
                                <asp:ImageButton ID="ImgTarifasExcesivas" runat="server" ImageUrl="~/Imagenes/Modulos/img_tarifas_excesivas.png" OnClick="ImgTarifasExcesivas_Click" ToolTip="Ver tarifas excesivas por municipios" /></td>
                            <td align="center">
                                <asp:ImageButton ID="ImgRegresarPlaneacion" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgRegresarPlaneacion_Click" ToolTip="Click para regresar al panel principal" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label2" runat="server" Width="400px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="PnlLiquidacion" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Width="100%" Visible="False">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td colspan="6" align="center" bgcolor="#999999">
                                <asp:Label ID="Label4" runat="server" CssClass="SubTitle" Text="MODULOS INFORMACIÓN TRIBUTARÍA" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImgModLiquidacionIca" runat="server" ImageUrl="~/Imagenes/Modulos/img_modulo_ica.png" OnClick="ImgModLiquidacionIca_Click" ToolTip="Modulo para la liquidación del impuesto de ICA" /></td>
                            <td align="center">
                                <asp:ImageButton ID="ImgModBorradorAutoIca" runat="server" ImageUrl="~/Imagenes/Modulos/img_modulo_autoretencionica.png" OnClick="ImgModBorradorAutoIca_Click" ToolTip="Modulo de Liquidación de Autoretención" /></td>
                            <td align="center">
                                <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/Imagenes/Modulos/img_ReteIca.png" OnClick="imgIca_Click" ToolTip="Módulo para la liquidación de Retención de ICA" /></td>
                            <td align="center">
                                <asp:ImageButton ID="imgAlumbradoPublico" runat="server" ImageUrl="~/Imagenes/Modulos/img_AlumbradoPublico.png" OnClick="imgAlumbradoPublico_Click" ToolTip="Liquidar alumbrado publico" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgEjecucionPorLotes" runat="server" ImageUrl="~/Imagenes/Modulos/img_ejecucion_lotes.png" OnClick="ImgEjecucionPorLotes_Click" ToolTip="Ejecución por Lotes" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgValidarLiqLotes" runat="server" ImageUrl="~/Imagenes/Modulos/img_validar_liq_lotes.png" OnClick="ImgValidarLiqLotes_Click" ToolTip="Validar Liquidaciones por Lote" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImgConsultarImpLiquidado" runat="server" ImageUrl="~/Imagenes/Modulos/img_consulta.png" OnClick="ImgConsultarImpLiquidado_Click" ToolTip="Consultar impuestos en borrado" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgFichaTecnica" runat="server" ImageUrl="~/Imagenes/Modulos/img_ficha_tecnica.png" OnClick="ImgFichaTecnica_Click" ToolTip="Consultar ficha técnica por municipio" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgProcesoContabilizacion" runat="server" ImageUrl="~/Imagenes/Modulos/img_proceso_contabilizacion.png" OnClick="ImgProcesoContabilizacion_Click" ToolTip="Ejecutar Proceso Comprobante Contabilización" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgHerramientaCuadre" runat="server" ImageUrl="~/Imagenes/Modulos/img_herramienta_cuadre.png" OnClick="ImgHerramientaCuadre_Click" ToolTip="Configurar proceso de conciliacion HC" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgRegresarModLiquidacion" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgRegresarModLiquidacion_Click" ToolTip="Click para regresar al panel principal" />
                            </td>
                            <td align="center">&#160;</td>
                            <td align="center">&#160;</td>
                        </tr>
                        <tr>
                            <td>&#160;</td>
                            <td>&#160;</td>
                            <td>&#160;</td>
                            <td>&#160;</td>
                            <td>&#160;</td>
                            <td>&#160;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="PnlLiquidacionAlumbrado" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Width="100%" Visible="False">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td colspan="5" align="center" bgcolor="#999999">
                                <asp:Label ID="Label11" runat="server" CssClass="SubTitle" Text="LIQUIDACIÓN DE ALUMBRADO PÚBLICO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="imgHojaAlumbrado" runat="server" ImageUrl="~/Imagenes/Modulos/img_liquidacion_obligacion.png" ToolTip="Hoja de Trabajo Impuesto" OnClick="imgHojaAlumbrado_Click" /></td>
                            <td align="center">
                                <asp:ImageButton ID="ImgRegresarLiqAlum" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" ToolTip="Click para regresar al panel principal" OnClick="ImgRegresarLiqAlum_Click" /></td>
                            <td align="center">
                                <asp:Label ID="Label12" runat="server" Width="300px"></asp:Label></td>
                            <td align="center">&#160;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&#160;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="pnlReteIca" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Visible="False" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Label23" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="LIQUIDACIÓN DE RETEICA"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="imgReteIca" runat="server" ImageUrl="~/Imagenes/Modulos/img_liquidacion_obligacion.png" OnClick="imgReteIca_Click" ToolTip="Hoja de Trabajo Impuesto" /></td>
                            <td align="center">
                                <asp:ImageButton ID="imgDefinitivaIca" runat="server" ImageUrl="~/Imagenes/Modulos/img_definitivo_imp.png" OnClick="imgDefinitivaIca_Click" ToolTip="Liquidación definitiva del Ica" /></td>
                            <td align="center">
                                <asp:ImageButton ID="imgAtras" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="imgAtras_Click" ToolTip="Click para regresar al panel principal" /></td>
                            <td align="center">&#160;</td>
                            <td align="center">&#160;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="PnlLiquidacionIca" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Visible="False" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Label6" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="LIQUIDACIÓN DE IMPUESTOS DEL ICA"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImgLiqBorradorIca" runat="server" ImageUrl="~/Imagenes/Modulos/img_liquidacion_obligacion.png" OnClick="ImgLiqBorradorIca_Click" ToolTip="Liquidación borrador impuesto de ICA" /></td>
                            <td align="center">
                                <asp:ImageButton ID="ImgLiqImpuestoDefinitivo" runat="server" ImageUrl="~/Imagenes/Modulos/img_definitivo_imp.png" OnClick="ImgLiqImpuestoDefinitivo_Click" ToolTip="Liquidación definitiva del Ica" /></td>
                            <td align="center">
                                <asp:ImageButton ID="ImgRegresarLiqIca" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgRegresarLiqIca_Click" ToolTip="Click para regresar al panel principal" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label7" runat="server" Width="300px"></asp:Label>
                            </td>
                            <td align="center">&#160;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&#160;</td>
                            <td>&#160;</td>
                            <td>&#160;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="PnlLiquidacionAutoIca" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Visible="False" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Label8" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="LIQUIDACIÓN DE IMPUESTOS AUTORETENCIÓN DEL ICA"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImgLiqBorradorAutoIca" runat="server" ImageUrl="~/Imagenes/Modulos/img_liquidacion_obligacion.png" OnClick="ImgLiqBorradorAutoIca_Click" ToolTip="Liquidación borrador impuesto Autoretención de ICA" /></td>
                            <td align="center">
                                <asp:ImageButton ID="ImgLiqDefinitivoAutoIca" runat="server" ImageUrl="~/Imagenes/Modulos/img_definitivo_autoretencionica.png" OnClick="ImgLiqDefinitivoAutoIca_Click" ToolTip="Liquidación definitiva del Ica" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgRegresarLiqAutoIca" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgRegresarLiqAutoIca_Click" ToolTip="Click para regresar al panel principal" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label113" runat="server" Width="300px"></asp:Label>
                            </td>
                            <td align="center">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&#160;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PnlHerramientaHc" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Visible="False" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Label24" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="PROCESAR ARCHIVOS Y HERRAMIENTA DE CUADRE"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImgProcesarFilesHc" runat="server" ImageUrl="~/Imagenes/Modulos/img_procesar_archivos.png" OnClick="ImgProcesarFilesHc_Click" ToolTip="Permite procesar los archivos descargados del Davibox" /></td>
                            <td align="center">
                                <asp:ImageButton ID="ImgConciliacionHC" runat="server" ImageUrl="~/Imagenes/Modulos/img_conciliacion_hc.png" OnClick="ImgConciliacionHC_Click" ToolTip="Permite realizar conciliacion Hc" /></td>
                            <td align="center">
                                <asp:ImageButton ID="ImgProcesarFilesAjustes" runat="server" ImageUrl="~/Imagenes/Modulos/img_procesar_archivos_ajustes.png" OnClick="ImgProcesarFilesAjustes_Click" ToolTip="Permite procesar los archivos de ajustes" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgRegresarHc" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgRegresarHc_Click" ToolTip="Click para regresar al panel principal" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label112" runat="server" Width="300px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&#160;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="PnlActividades" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Visible="False" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Label5" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="MODULO DE CONTROL DE ACTIVIDADES"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImgMisActividades" runat="server" ImageUrl="~/Imagenes/Modulos/img_mis_actividades.png" OnClick="ImgMisActividades_Click" ToolTip="Ver el Control de mis Actividades" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgMonitoreoAct" runat="server" ImageUrl="~/Imagenes/Modulos/img_monitoreo_actividades.png" OnClick="ImgMonitoreoAct_Click" ToolTip="Liquidación definitiva de impuestos ICA" />
                            </td>
                            </td align="center">
                            <asp:ImageButton ID="ImgEstadisticaAct" runat="server" ImageUrl="~/Imagenes/Modulos/img_estadistica_actividades.png" OnClick="ImgEstadisticaAct_Click" ToolTip="Ver estadisticas de actividades" />
                            <td align="center">
                                <asp:ImageButton ID="ImgEstadisticaLiq" runat="server" ImageUrl="~/Imagenes/Modulos/img_estadistica_liquidaciones.png" OnClick="ImgEstadisticaLiq_Click" ToolTip="Permite ver estadisticas de liquidación de impuestos" />
                            </td>
                            </td align="center">
                            <asp:ImageButton ID="ImgRegresarActiv" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgRegresarActiv_Click" ToolTip="Click para regresar al panel principal" />
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="PnlFormatosSFC" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Visible="False" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Label10" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="MODULOS FORMATOS SFC"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="btnProcess" runat="server" ImageUrl="~/Imagenes/Modulos/img_generacion_datos.png" OnClick="ImgGenerarProceso_Click" ToolTip="Generacion de Formatos F-321 / 525" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="btn321" runat="server" ImageUrl="~/Imagenes/Modulos/img_f_321.png" OnClick="ImgDescarga321_Click" ToolTip="Generar Excel f-321" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="btn525" runat="server" ImageUrl="~/Imagenes/Modulos/img_f_525.png" OnClick="ImgDescarga525_Click" ToolTip="Generar Excel f-525" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="btnFiles" runat="server" ImageUrl="~/Imagenes/Modulos/img_plano.png" OnClick="ImgDescargaPlano_Click" ToolTip="Generar plano f-321/525" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="btnReturn" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgRegresarFormatos_Click" ToolTip="Click para regresar al panel principal" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="PnlPlanormatividad" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Visible="False" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Labelnormatividad" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="MODULOS NORMATIVA"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImgModuloNormatividad" runat="server" ImageUrl="~/Imagenes/Modulos/img_normatividad_cargar.png" OnClick="ImgVerModuloNormativa_Click" ToolTip="Cargar Normativa" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgModuloNormatividad_consulta" runat="server" ImageUrl="~/Imagenes/Modulos/img_normatividad_consulta.png" OnClick="ImgVerModuloNormativa_consulta_Click" ToolTip="Consultar Normativa" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgCarga_masiva" runat="server" ImageUrl="~/Imagenes/Modulos/img_carga_masiva_documentos.png" OnClick="ImgVerCargaMasiva_Click" ToolTip="carga masiva de documentos" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgRegresarNorvativa" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgRegresarNprmatividad_Click" ToolTip="Click para regresar al panel principal" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label111" runat="server" Width="400px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="PnlPlanoReporte" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Visible="False" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Labelreportes" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="MODULOS REPORTES"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImageAnexos" runat="server" ImageUrl="~/Imagenes/Modulos/img_Anexos.png" OnClick="ImgAnexos_Click" ToolTip="Anexos" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgRegresarReporte" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgRegresareporte_Click" ToolTip="Click para regresar al panel principal" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label13" runat="server" Width="400px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="Panelanexos" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Visible="False" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Label3" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="MODULOS ANEXOS"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImageRenta" runat="server" ImageUrl="~/Imagenes/Modulos/img_Renta.png" OnClick="ImageRenta_Click" ToolTip="Renta" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImageAutoretenciones" runat="server" ImageUrl="~/Imagenes/Modulos/img_Autoretenciones.png" OnClick="ImgAutoretencion_Click" ToolTip="Autoretenciones" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="Imagepredial" runat="server" ImageUrl="~/Imagenes/Modulos/img_predial.png" OnClick="ImgPredial_Click" ToolTip="Predial" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgRegresarAnexos" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgRegresarAnexos_Click" ToolTip="Click para regresar al panel Reportes" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label14" runat="server" Width="400px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="Panelautoretencion" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Visible="False" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Label15" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="MODULOS AUTORETENCION"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImageConsulta_autoretencion" runat="server" ImageUrl="~/Imagenes/Modulos/img_Consulta_autoretencion.png" OnClick="ImageConsulAutore_Click" ToolTip="Consulta información de autoretencion" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="Generar_archivo_autoretencion" runat="server" ImageUrl="~/Imagenes/Modulos/img_Generar_archivo_autoretencion.png" OnClick="ImageGenerAutore_Click" ToolTip="Generar del archivo de EXCEL autoretencion" />
                            </td>
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgRegresarAutoretencion" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgRegresArautoretencion_Click" ToolTip="Click para regresar al panel Anexos" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label16" runat="server" Width="400px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="PanelPredial" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Visible="False" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Label17" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="MODULOS PREDIAL"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImagCarguePredial" runat="server" ImageUrl="~/Imagenes/Modulos/img_Cargue_anexo_rent_imp_predial.png" OnClick="ImageCarguePredial_Click" ToolTip="Cargue de archivo Anexo Renta Pagos Impuesto Predial" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImagConsultPredial" runat="server" ImageUrl="~/Imagenes/Modulos/img_Consulta_anexo_rent_imp_predial.png" OnClick="ImageConsultPredial_Click" ToolTip="Consulta información anexo renta pagos Impuesto predial" />
                            </td>
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImagGeneraPredial" runat="server" ImageUrl="~/Imagenes/Modulos/img_Generar_anexo_rent_imp_predial.png" OnClick="ImageGenerarPredial_Click" ToolTip="Generar Reporte anexo renta pagos Impuesto predial" />
                            </td>
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImgRegresarPredial" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgRegresPredial_Click" ToolTip="Click para regresar al panel Anexos" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label18" runat="server" Width="400px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="PnlPlanoImageRequerimiento" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Visible="False" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Label19" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="MODULO CONTROL DE REQUERIMIENTOS"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImageRecDocumentos" runat="server" ImageUrl="~/Imagenes/Modulos/img_Administrar_Documentos.png" OnClick="ImageRecDocumentos_Click" ToolTip="Administrar Documentos" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImageConsulRequeri" runat="server" ImageUrl="~/Imagenes/Modulos/img_Consulta_Requerimientos.png" OnClick="ImageConsulRequeri_Click" ToolTip="Consulta Requerimientos" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImageGraficaRequeri" runat="server" ImageUrl="~/Imagenes/Modulos/img_Gráficas_Estadíticas.png" OnClick="ImageGraficaRequeri_Click" ToolTip="Gráficas y Estadíticas" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImageregresoReque" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgregRegresaRequerimiento_Click" ToolTip="Click para regresar al modulo de sistemas" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label20" runat="server" Width="400px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel ID="PnlPlanoImagecustodia" runat="server" Style="border-color: AliceBlue; border-style: solid; padding: 1px 4px; z-index: 1;" Visible="False" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="5">
                                <asp:Label ID="Label21" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="MODULO CONTROL CUSTODIA DE IMÁGENES"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Imagenes/Modulos/img_Cargue_Manual.png" OnClick="ImageCargue_Manual_Click" ToolTip="Cargue Manual" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Imagenes/Modulos/img_Cargue_Masivo.png" OnClick="ImageCargue_Masivo_Click" ToolTip="Cargue Masivo" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Imagenes/Modulos/img_Consulta_Declaraciones.png" OnClick="ImageConsulta_claraciones_Click" ToolTip="Consulta Declaraciones" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Imagenes/Modulos/img_regresar.png" OnClick="ImgRegresaControlcustodia_Click" ToolTip="Click para regresar al modulo de sistemas" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label22" runat="server" Width="400px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
