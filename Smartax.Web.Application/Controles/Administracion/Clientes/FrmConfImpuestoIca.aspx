<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmConfImpuestoIca.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmConfImpuestoIca" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script language="JavaScript">
        var msg = "¡El botón derecho está desactivado para este sitio !";
        function disableIE() {
            if (document.all) {
                //alert(msg);
                return false;
            }
        }

        function disableNS(e) {
            if (navigator.appName == 'Netscape' && (e.which == 3 || e.which == 2)) {
                //alert(msg);
                return false;
            } else if (navigator.appName == 'Microsoft Internet Explorer' && (event.button == 2)) {
                //alert(msg);
                return false;
            }

            //alert('El Navegador es: ' + navigator.appName);
        }

        if (document.layers) {
            document.captureEvents(Event.MOUSEDOWN);
            document.onmousedown = disableNS;
        } else {
            document.onmouseup = disableNS;
            document.oncontextmenu = disableIE;
        }
        document.oncontextmenu = new Function("return false")
        //document.oncontextmenu = new Function("alert(msg);return false")
    </script>
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:Panel ID="PnlDatos" runat="server" Width="1150px" HorizontalAlign="Center" BorderStyle="Solid">
                    <table cellpadding="4" cellspacing="0" class="Tab" border="1" style="width: 1150px;">
                        <tr>
                            <td colspan="6" align="center" bgcolor="#999999">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="HOJA DE CONFIGURACIÓN IMPUESTO DEL ICA" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="6">
                                <asp:Label ID="LbTitulo0" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="FORMULARIO ÚNICO NACIONAL DE DECLARACIÓN Y PAGO DEL IMPUESTO DE INDUSTRIA Y COMERCIO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label110" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">LIQUIDACIÓN</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label111" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">RENGLON</asp:Label>
                            </td>
                            <td colspan="3" align="center">
                                <asp:Label ID="Label112" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">DESCRIPCIÓN DEL RENGLON</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label113" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">CONFIGURAR</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" rowspan="9">
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/Imagenes/Otros/img_base_gravable.png" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label31" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">8</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon8" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">TOTAL INGRESOS ORDINARIOS Y EXTRAORDINARIOS DEL PERIODO EN TODO EL PAIS</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon8" runat="server" Text="Configurar" Font-Bold="True" OnClick="BtnConfRenglon8_Click" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto8" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                &nbsp;<asp:Label ID="Label32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">9</asp:Label>
                            </td>
                            <td colspan="3">
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon9" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">     MENOS INGRESOS FUERA DE ESTE MUNICIPIO O DISTRITO</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon9" runat="server" Font-Bold="True" OnClick="BtnConfRenglon9_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto9" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">10</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon10" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">TOTAL INGRESOS ORDINARIOS Y EXTRAORDINARIOS EN ESTE MUNICIPIO (RENGLÓN 8 MENOS 9)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon10" runat="server" Font-Bold="True" OnClick="BtnConfRenglon10_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto10" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">11</asp:Label>
                            </td>
                            <td colspan="3">
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon11" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">     MENOS INGRESOS POR DEVOLUCIONES, REBAJAS, DESCUENTOS</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon11" runat="server" Font-Bold="True" OnClick="BtnConfRenglon11_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto11" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center"><asp:Label ID="Label35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">12</asp:Label>
                            </td>
                            <td colspan="3">&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon12" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">     MENOS INGRESOS POR EXPORTACIONES</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon12" runat="server" Font-Bold="True" OnClick="BtnConfRenglon12_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto12" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">13</asp:Label>
                            </td>
                            <td colspan="3">
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon13" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">     MENOS INGRESOS POR VENTA DE ACTIVOS FIJOS</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon13" runat="server" Font-Bold="True" OnClick="BtnConfRenglon13_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto13" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label37" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">14</asp:Label>
                            </td>
                            <td colspan="3">&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon14" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">     MENOS INGRESOS POR ACTIVIDADES EXCLUIDAS O NO SUJETAS Y OTROS INGRESOS NO GRAVADOS</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon14" runat="server" Font-Bold="True" OnClick="BtnConfRenglon14_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto14" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">15</asp:Label>
                            </td>
                            <td colspan="3">&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon15" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">     MENOS INGRESOS POR OTRAS ACTIVIDADES EXENTAS EN ESTE MINICIPIO O DISTRITO (POR ACUERDO)</asp:Label>
                                &nbsp;</td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon15" runat="server" Font-Bold="True" OnClick="BtnConfRenglon15_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto15" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label39" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">16</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon16" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">TOTAL INGRESOS GRAVABLES (RENGLÓN 10 MENOS 11, 12, 13, 14 Y 15)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon16" runat="server" Font-Bold="True" OnClick="BtnConfRenglon16_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto16" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" rowspan="15">
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/Imagenes/Otros/img_liq_privada.png" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label63" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">20</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon20" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">TOTAL IMPUESTOS DE INDUSTRIA Y COMERCIO (RENGLÓN 17+19)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon20" runat="server" Font-Bold="True" OnClick="BtnConfRenglon20_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto20" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label64" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">21</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon21" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">IMPUESTOS DE AVISOS Y TABLEROS (15% del renglón 20)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon21" runat="server" Font-Bold="True" OnClick="BtnConfRenglon21_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto21" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label65" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">22</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon22" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">PAGO POR UNIDADES COMERCIALES ADICIONALES DEL SECTOR FINANCIERO</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon22" runat="server" Font-Bold="True" OnClick="BtnConfRenglon22_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto22" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label66" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">23</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon23" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">SOBRETASA BOMBERIL (Ley 1575 de 2012) (si la hay, liquide según el acuerdo Municipal o distrital)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon23" runat="server" Font-Bold="True" OnClick="BtnConfRenglon23_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto23" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label67" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">24</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">SOBRETASA DE SEGURIDAD (LEY 1421 de 2011) (SI la hay, liquídela según el acuerdo Municipal o distrital)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon24" runat="server" Font-Bold="True" OnClick="BtnConfRenglon24_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label68" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">25</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">TOTAL DE IMPUESTO A CARGO (REGLÓN 20+21+22+23+24)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon25" runat="server" Font-Bold="True" OnClick="BtnConfRenglon25_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label69" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">26</asp:Label>
                            </td>
                            <td colspan="3">
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon26" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">     MENOS VALOR DE EXENCION O EXONERACION SOBRE EL IMPUESTO Y NO SOBRE LOS INGRESOS</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon26" runat="server" Font-Bold="True" OnClick="BtnConfRenglon26_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto26" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label70" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">27</asp:Label>
                            </td>
                            <td colspan="3">
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">     MENOS RETENCIONES que le practicaron a favor de este municipio o distrito en este periodo</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon27" runat="server" Font-Bold="True" OnClick="BtnConfRenglon27_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label71" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">28</asp:Label>
                            </td>
                            <td colspan="3">
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon28" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">     MENOS AUTORRETENCIONES practicadas a favor de este municipio o distrito en este periodo</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon28" runat="server" Font-Bold="True" OnClick="BtnConfRenglon28_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto28" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label72" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">29</asp:Label>
                            </td>
                            <td colspan="3">
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">     MENOS ANTICIPO LIQUIDADO EN EL AÑO ANTERIOR</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon29" runat="server" Font-Bold="True" OnClick="BtnConfRenglon29_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label73" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">30</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">ANTICIPO DEL AÑO SIGUIENTE (Si existe, liquide porcentaje según Acuerdo Municipal o Distrital)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon30" runat="server" Font-Bold="True" OnClick="BtnConfRenglon30_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label74" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">31</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="Label90" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">SANCIONES. </asp:Label>
                                <asp:RadioButton ID="RbExtemporaneidad" runat="server" AutoPostBack="True" Font-Size="8pt" OnCheckedChanged="RbExtemporaneidad_CheckedChanged" Text="Extemporaneidad" />
                                &nbsp;<asp:RadioButton ID="RbCorreccion2" runat="server" AutoPostBack="True" Font-Size="8pt" OnCheckedChanged="RbCorreccion2_CheckedChanged" Text="Corrrección" />
                                &nbsp;<asp:RadioButton ID="RbInexactitud" runat="server" AutoPostBack="True" Font-Size="8pt" OnCheckedChanged="RbInexactitud_CheckedChanged" Text="Inexactitud" />
                                &nbsp;<asp:RadioButton ID="RbOtra" runat="server" AutoPostBack="True" Font-Size="8pt" OnCheckedChanged="RbOtra_CheckedChanged" Text="Otra" />
                                &nbsp;
                                <asp:Label ID="Label91" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">Cuál:</asp:Label>
                                <telerik:RadTextBox ID="TxtSancion" runat="server" EmptyMessage="Descripción" Enabled="False" Font-Size="15pt" Height="25px" MaxLength="120" TabIndex="6" Width="200px">
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle Resize="None" />
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="Validator3" runat="server" ControlToValidate="TxtSancion" Display="None" Enabled="False" ErrorMessage="Este campo es requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validator3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validator3">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                            <td align="center">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label75" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">32</asp:Label>
                            </td>
                            <td colspan="3">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="LblDescRenglon32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">     MENOS SALDO A FAVOR DEL PERIODO ANTERIOR SIN SOLICITUD DE DEVOLUCIÓN O COMPENSACIÒN</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon32" runat="server" Font-Bold="True" OnClick="BtnConfRenglon32_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label76" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">33</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">TOTAL SALDO A CARGO (RENGLÓN 25-26-27-28-29+30+31-32)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon33" runat="server" Font-Bold="True" OnClick="BtnConfRenglon33_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label77" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">34</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">TOTAL SALDO A FAVOR (RENGLÒN 25-26-27-28-29+30+31-32) si el resultado es menor a cero</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon34" runat="server" Font-Bold="True" OnClick="BtnConfRenglon34_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" rowspan="4">
                                <asp:Image ID="Image5" runat="server" Height="100px" ImageUrl="~/Imagenes/Otros/img_pago.png" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label78" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">35</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">VALOR A PAGAR</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon35" runat="server" Font-Bold="True" OnClick="BtnConfRenglon35_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label96" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">36</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">DESCUENTO POR PRONTO PAGO (Si existe, liquídelo según el acuerdo Municipal o distrital)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon36" runat="server" Font-Bold="True" OnClick="BtnConfRenglon36_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label97" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">37</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="Label100" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">INTERESES DE MORA</asp:Label>
                            </td>
                            <td align="right">
                                <telerik:RadNumericTextBox ID="TxtValorRenglon37" runat="server" AutoPostBack="True" EmptyMessage="Intereses" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="10" MinValue="0" OnTextChanged="TxtValorRenglon37_TextChanged" Value="0" Width="160px">
                                    <NegativeStyle HorizontalAlign="Right" Resize="None" />
                                    <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle HorizontalAlign="Center" Resize="None" />
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label98" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">38</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="LblDescRenglon38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">TOTAL A PAGAR (RENGLÒN 35-36+37)</asp:Label>
                            </td>
                            <td align="center">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center" rowspan="3" colspan="3">
                                <asp:Label ID="Label103" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">SECCIÓN PAGO VOLUNTARIO</asp:Label>
                                <br />
                                <asp:Label ID="Label109" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">(Solamente donde exista esta opción)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label104" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">39</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblDescRenglon39" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">LIQUIDE EL VALOR DEL PAGO VOLUNTARIO (Según instrucciones del munucipio/distrito)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon39" runat="server" Font-Bold="True" OnClick="BtnConfRenglon39_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto39" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label105" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">40</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblDescRenglon40" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">TOTAL A PAGAR CON PAGO VOLUNTARIO (Renglón 38 +39)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Button ID="BtnConfRenglon40" runat="server" Font-Bold="True" OnClick="BtnConfRenglon40_Click" Text="Configurar" ToolTip="Click para configurar renglon" />
                                <asp:Label ID="LblIdConfImpuesto40" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Visible="False" Width="13px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label106" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">41</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label108" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">Destino de mi aporte voluntario</asp:Label>
                            </td>
                            <td align="center">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center" colspan="6">
                                <asp:Panel ID="Panel3" runat="server">
                                    <table style="width:100%;">
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnGuardar_Click" Text="Guardar Datos" ToolTip="Click para guardar datos de la configuración" ValidationGroup="ValidarDatos" Width="200px" Visible="False" />
                                                <ajaxToolkit:ConfirmButtonExtender ID="BtnGuardar_ConfirmButtonExtender" runat="server" BehaviorID="BtnGuardar_ConfirmButtonExtender" ConfirmText="¿Señor usuario, se encuentra seguro de guardar la información del Borrador del formulario del ICA ?" TargetControlID="BtnGuardar" />
                                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="200px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="6">
                                &nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
            </telerik:RadAjaxPanel>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
                <div class="loading">
                    <asp:Image ID="Image7" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" />
                </div>
            </telerik:RadAjaxLoadingPanel>
        </div>
    </form>
</body>
</html>
