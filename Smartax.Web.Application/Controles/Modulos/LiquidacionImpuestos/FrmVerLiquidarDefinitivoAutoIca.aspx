﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVerLiquidarDefinitivoAutoIca.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos.FrmVerLiquidarDefinitivoAutoIca" %>
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
                <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center" BorderStyle="Solid">
                    <table cellpadding="4" cellspacing="0" class="Tab" border="1" style="width: 1200px;">
                        <tr>
                            <td colspan="13" align="center" bgcolor="#999999">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="HOJA DE TRABAJO PARA LA LIQUIDACIÓN DEFINITIVA" Font-Bold="True" Font-Size="14pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="13">
                                <asp:Label ID="LbTitulo0" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="AUTORETENCIÓNES Y RETENCIÓN DE INDUSTRIA Y COMERCIO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" colspan="2" align="right">
                                <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt" Width="140px">MUNICIPIO O DISTRITO:</asp:Label>
                            </td>
                            <td rowspan="2">
                                <telerik:RadNumericTextBox ID="TxtCodDane" runat="server" EmptyMessage="Cod. Dane" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="10" MinValue="1" TabIndex="1" Width="120px" DataType="System.Int32" Enabled="False">
                                    <NegativeStyle Resize="None" />
                                    <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle HorizontalAlign="Center" Resize="None" />
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="Validator1" runat="server" ControlToValidate="TxtCodDane" Display="None" ErrorMessage="Este campo es requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validator1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validator1">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                            <td rowspan="2" align="center" colspan="3">
                                <asp:Label ID="LblNombreMunicipio" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            <td align="center" colspan="7">
                                <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt" Width="155px">Fecha Max presentación</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="Label14" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">dd</asp:Label>
                            </td>
                            <td align="center" colspan="3">
                                <asp:Label ID="Label15" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">mm</asp:Label>
                            </td>
                            <td align="center" colspan="2">
                                <asp:Label ID="Label16" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">aaaa</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Label ID="Label3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">DEPARTAMENTO:</asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td align="center" colspan="3">
                                <asp:Label ID="LblNombreDpto" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Width="400px"></asp:Label>
                            </td>
                            <td align="center" colspan="2">
                                <asp:DropDownList ID="CmbFecha1" runat="server" Font-Size="12pt">
                                </asp:DropDownList>
                            </td>
                            <td align="center" colspan="3">
                                <asp:DropDownList ID="CmbFecha2" runat="server" Font-Size="12pt">
                                </asp:DropDownList>
                            </td>
                            <td align="center" colspan="2">
                                <asp:DropDownList ID="CmbFecha3" runat="server" Font-Size="12pt">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right" rowspan="2">
                                <asp:Label ID="Label5" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">AÑO GRAVABLE:</asp:Label>
                            </td>
                            <td rowspan="2">
                                <telerik:RadNumericTextBox ID="TxtAnioGravable" runat="server" EmptyMessage="Año Gravable" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="4" MinValue="1" TabIndex="2" Width="120px" Enabled="False">
                                    <NegativeStyle Resize="None" />
                                    <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle HorizontalAlign="Center" Resize="None" />
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="Validator2" runat="server" ControlToValidate="TxtAnioGravable" Display="None" ErrorMessage="Este campo es requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validator2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validator2">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                            <td align="center" colspan="10">
                                <asp:Label ID="LblPeriodicidad" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">PERIODICIDAD DE PAGO</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10" align="center">
                                <asp:RadioButtonList ID="RbPeriodoMensual" runat="server" RepeatDirection="Horizontal" Visible="False" Width="100%">
                                    <asp:ListItem Value="1">Enero</asp:ListItem>
                                    <asp:ListItem Value="2">Febrero</asp:ListItem>
                                    <asp:ListItem Value="3">Marzo</asp:ListItem>
                                    <asp:ListItem Value="4">Abril</asp:ListItem>
                                    <asp:ListItem Value="5">Mayo</asp:ListItem>
                                    <asp:ListItem Value="6">Junio</asp:ListItem>
                                    <asp:ListItem Value="7">Julio</asp:ListItem>
                                    <asp:ListItem Value="8">Agosto</asp:ListItem>
                                    <asp:ListItem Value="9">Septiembre</asp:ListItem>
                                    <asp:ListItem Value="10">Octubre</asp:ListItem>
                                    <asp:ListItem Value="11">Noviembre</asp:ListItem>
                                    <asp:ListItem Value="12">Diciembre</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RadioButtonList ID="RbPeriodoBimestral" runat="server" RepeatDirection="Horizontal" Visible="False" Width="100%">
                                    <asp:ListItem Value="2|13">Enero-Febrero</asp:ListItem>
                                    <asp:ListItem Value="4|14">Marzo-Abril</asp:ListItem>
                                    <asp:ListItem Value="6|15">Mayo-Junio</asp:ListItem>
                                    <asp:ListItem Value="8|16">Julio-Agosto</asp:ListItem>
                                    <asp:ListItem Value="10|17">Septiembre-Octubre</asp:ListItem>
                                    <asp:ListItem Value="12|18">Noviembre-Diciembre</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RadioButtonList ID="RbPeriodoTrimestral" runat="server" RepeatDirection="Horizontal" Visible="False" Width="1100px">
                                    <asp:ListItem Value="3|19">Enero-Febrero-Marzo</asp:ListItem>
                                    <asp:ListItem Value="6|20">Abril-Mayo-Junio</asp:ListItem>
                                    <asp:ListItem Value="9|21">Julio-Agosto-Septiembre</asp:ListItem>
                                    <asp:ListItem Value="12|22">Octubre-Noviembre-Diciembre</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RadioButtonList ID="RbPeriodoCuatrimestral" runat="server" RepeatDirection="Horizontal" Visible="False" Width="1100px">
                                    <asp:ListItem Value="4|23">Enero-Febrero-Marzo-Abril</asp:ListItem>
                                    <asp:ListItem Value="8|24">Mayo-Junio-Julio-Agosto</asp:ListItem>
                                    <asp:ListItem Value="12|25">Septiembre-Noviembre-Diciembre</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RadioButtonList ID="RbPeriodoSemestral" runat="server" RepeatDirection="Horizontal" Visible="False" Width="1000px">
                                    <asp:ListItem Value="6|26">Enero-Febrero-Marzo-Abril-Mayo-Junio</asp:ListItem>
                                    <asp:ListItem Value="12|27">Julio-Agosto-Septiembre-Noviembre-Diciembre</asp:ListItem>
                                </asp:RadioButtonList>
                                <%--<asp:RadioButtonList ID="RbPeriodoImpuesto" runat="server" RepeatDirection="Horizontal" Width="550px">
                                    <asp:ListItem Value="1">ene-feb</asp:ListItem>
                                    <asp:ListItem Value="2">mar-abr</asp:ListItem>
                                    <asp:ListItem Value="3">may-jun</asp:ListItem>
                                    <asp:ListItem Value="4">jul-ago</asp:ListItem>
                                    <asp:ListItem Value="5">sep-oct</asp:ListItem>
                                    <asp:ListItem Value="6">nov-dic</asp:ListItem>
                                    <asp:ListItem Value="7">anual</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="Validator4" runat="server" ControlToValidate="RbPeriodoImpuesto" Display="None" ErrorMessage="Este campo es requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validator4_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validator4">
                                </ajaxToolkit:ValidatorCalloutExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2" rowspan="2">
                                <asp:Label ID="Label7" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">OPCIÓN DE USO:</asp:Label>
                            </td>
                            <td colspan="4" rowspan="2">
                                <asp:RadioButton ID="RbDeclaracionInicial" runat="server" Font-Size="8pt" Text="DECLARACIÓN INICIAL" Checked="True" Enabled="False" />
                                &nbsp;<asp:RadioButton ID="RbSoloPago" runat="server" Font-Size="8pt" Text="SOLO PAGO" Enabled="False" />
                                &nbsp;<asp:RadioButton ID="RbCorreccion" runat="server" Font-Size="8pt" Text="CORRECCIÓN" Enabled="False" />
                                &nbsp;<asp:Label ID="Label9" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Declaración que corrige No.:</asp:Label>
                                <telerik:RadNumericTextBox ID="TxtNumCorreccion" runat="server" EmptyMessage="Número" Enabled="False" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="20" MinValue="1" TabIndex="2" Width="120px">
                                    <NegativeStyle Resize="None" />
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
                            <td align="right" rowspan="2">
                                <asp:Label ID="Label11" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">Fecha:</asp:Label>
                            </td>
                            <td align="center" colspan="3">
                                <asp:Label ID="Label10" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">dd</asp:Label>
                            </td>
                            <td align="center" colspan="2">
                                <asp:Label ID="Label12" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">mm</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label13" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">aaaa</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1" colspan="6" align="center">
                                <asp:Label ID="LblFechaActual" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="9" align="center">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/Otros/img_info_contribuyente.png" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label18" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">1</asp:Label>
                            </td>
                            <td colspan="11">
                                <asp:Label ID="Label17" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">NOMBRES Y APELLIDOS O RAZÓN SOCIAL:</asp:Label>
                                <asp:Label ID="LblNombreCliente" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label19" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">2</asp:Label>
                            </td>
                            <td colspan="11">
                                <asp:RadioButton ID="RbCedulaCiudadania" runat="server" Font-Size="8pt" Text="C.C." Enabled="False" />
                                &nbsp;<asp:RadioButton ID="RbNit" runat="server" Font-Size="8pt" Text="NIT" Enabled="False" />
                                &nbsp;<asp:RadioButton ID="RbCedulaExtranjeria" runat="server" Font-Size="8pt" Text="C.E." Enabled="False" />
                                &nbsp;<asp:Label ID="LblNumDocumento" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">No.:_________________</asp:Label>
                                &nbsp;<asp:Label ID="LblDv" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">DV:______</asp:Label>
                                &nbsp;<asp:RadioButton ID="RbUnionTemporal" runat="server" Font-Size="8pt" Text="Es Consorcio o Unión Temporal" Enabled="False" />
                                &nbsp;<asp:RadioButton ID="RbPatrimonioAut" runat="server" Font-Size="8pt" Text="Realiza activ. a través de Patrimonio Autónomo" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" rowspan="3">
                                <asp:Label ID="Label20" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">3</asp:Label>
                            </td>
                            <td colspan="11">
                                <asp:Label ID="Label24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">DIRECCIÓN DE NOTIFICACIÓN:</asp:Label>
                                <asp:Label ID="LblDireccionNotificacion" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="Label25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">MUNICIPIO O DISTRITO DE LA DIRECCIÓN</asp:Label>
                                &nbsp;</td>
                            <td colspan="7" align="center">
                                <asp:Label ID="Label26" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">DEPARTAMENTO:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:Label ID="LblMunicipioDirNotificacion" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt"></asp:Label>
                            </td>
                            <td align="center" colspan="7">
                                <asp:Label ID="LblDptoDirNotificacion" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" rowspan="4">
                                <asp:Label ID="Label21" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">4</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TELÉFONO</asp:Label>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label28" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">5. CORREO ELECTRÓNICO</asp:Label>
                                &nbsp;</td>
                            <td>
                                <asp:Label ID="Label29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt" Width="145px">6. No. ESTABLECIMIENTOS</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">7. CLASIFICACIÓN</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblTelefono" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblEmail" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="9pt"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblNumEstablecimientos" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt"></asp:Label>
                            </td>
                            <td align="center" colspan="8">
                                <asp:Label ID="LblClasificacion" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label122" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">No. PLACA MUNICIPAL</asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label123" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">No. MATRICULA DE I.C.</asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label124" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">No. DEL RIT</asp:Label>
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="Label125" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">AVISOS Y TABLEROS</asp:Label>
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="Label126" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">FECHA INICIO DE ACTIVIDADES</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblNumPlacaMunicipal" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblNumMatriculaIc" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblNumeroRit" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt"></asp:Label>
                            </td>
                            <td align="center" colspan="4">
                                <asp:Label ID="LblAvisosTableros" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt"></asp:Label>
                            </td>
                            <td align="center" colspan="4">
                                <asp:Label ID="LblFechaInicioActividades" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="9" align="center">
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/Imagenes/Otros/img_base_gravable.png" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label31" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">8</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="Label40" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL INGRESOS ORDINARIOS Y EXTRAORDINARIOS DEL PERIODO EN TODO EL PAIS</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                &nbsp;&nbsp;
                                <asp:Label ID="LblValorRenglon8" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;<asp:Label ID="Label32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">9</asp:Label>
                            </td>
                            <td colspan="4">&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Label41" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">     MENOS INGRESOS FUERA DE ESTE MUNICIPIO O DISTRITO</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon9" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">10</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="Label42" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL INGRESOS ORDINARIOS Y EXTRAORDINARIOS EN ESTE MUNICIPIO (RENGLÓN 8 MENOS 9)</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon10" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">11</asp:Label>
                            </td>
                            <td colspan="4">&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Label43" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">     MENOS INGRESOS POR DEVOLUCIONES, REBAJAS, DESCUENTOS</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon11" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">12</asp:Label>
                            </td>
                            <td colspan="4">&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Label44" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">     MENOS INGRESOS POR EXPORTACIONES</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon12" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">13</asp:Label>
                            </td>
                            <td colspan="4">&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Label45" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">     MENOS INGRESOS POR VENTA DE ACTIVOS FIJOS</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon13" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label37" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">14</asp:Label>
                            </td>
                            <td colspan="4">&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Label46" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">     MENOS INGRESOS POR ACTIVIDADES EXCLUIDAS O NO SUJETAS Y OTROS INGRESOS NO GRAVADOS</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon14" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">15</asp:Label>
                            </td>
                            <td colspan="4">&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Label47" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">     MENOS INGRESOS POR OTRAS ACTIVIDADES EXENTAS EN ESTE MINICIPIO O DISTRITO (POR ACUERDO)</asp:Label>
                                &nbsp;</td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon15" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label39" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">16</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="Label48" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL INGRESOS POR OPERACIONES GRAVADAS BASE DE AUTORRETENCION (RENGLÓN 10 MENOS 11, 12, 13, 14 Y 15)</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon16" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="7" align="center">
                                <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/Otros/img_act_gravadas.png" />
                            </td>
                            <td align="center" colspan="2">
                                <asp:Label ID="Label49" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">ACTIVIDADES GRAVADAS</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label50" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">CÓDIGO</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label51" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">INGRESOS GRAVADOS</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblTipoTarifaAct" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt" Width="130px">TARIFA (por mil)</asp:Label>
                            </td>
                            <td colspan="7" align="center">
                                <asp:Label ID="Label118" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt" Width="130px">IMPUESTO</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="Label53" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">ACTIVIDAD 1 (PRINCIPAL)</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblCodActividad1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:Label ID="LblIngresosActividad1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblTarifaActividad1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon17" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="Label54" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">ACTIVIDAD 2</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblCodActividad2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:Label ID="LblIngresosActividad2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblTarifaActividad2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon18" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="Label55" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">ACTIVIDAD 3</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblCodActividad3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:Label ID="LblIngresosActividad3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblTarifaActividad3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon19" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="Label56" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">OTRAS ACTIVIDADES</asp:Label>
                            </td>
                            <td align="center">
                                <asp:LinkButton ID="LnkVerDesagregacion" runat="server" Font-Size="9pt" OnClick="LnkVerDesagregacion_Click">VER DESAGREGACIÓN</asp:LinkButton>
                            </td>
                            <td align="right">
                                <asp:Label ID="LblIngresosActividad4" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblTarifaActividad4" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorDesagregacion" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="3">
                                <asp:Label ID="Label57" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL INGRESOS GRAVADOS</asp:Label>
                            </td>
                            <td align="right">
                                <asp:Label ID="LblTotalIngresosGravados" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label58" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">17. TOTAL IMPUESTOS:</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblTotalImpuesto" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label59" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">18</asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label60" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt" Width="130px">GENERACIÓN DE ENERGÍA</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="Label61" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt" Width="130px">CAPACIDAD INSTALADA</asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="LblCapacidadInstalada" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label62" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt" Width="150px">19. IMPUESTO LEY 56 DE 1981</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblTotalImpuestosLey" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="6" align="center">
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/Imagenes/Otros/img_autoretencion.png" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label63" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">20</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="Label79" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL AUTORRETENCIONES DE INDUSTRIA Y COMERCIO  PRACTICADAS EN EL PERIODO (Base x Tarifa)</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon20" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label64" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">21</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon21" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL AUTORRETENCIONES DE AVISOS Y TABLEROS</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon21" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label65" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">22</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="Label81" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL AUTORRETENCION POR UNIDADES ADICIONALES  DEL SECTOR FINANCIERO</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon22" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label66" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">23</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon23" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL AUTORRETENCIONES SOBRETASA BOMBERIL </asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon23" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label67" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">24</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL AUTORRETENCIONES SOBRETASA  SEGURIDAD</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label68" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">25</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">SUB TOTAL AUTORRETENCIONES (REGLÓN 20+21+22+23+24)</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" rowspan="5">
                                <asp:Image ID="Image8" runat="server" ImageUrl="~/Imagenes/Otros/img_anticipos.png" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label69" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">26</asp:Label>
                            </td>
                            <td colspan="4">&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon26" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">     MENOS: ANTICIPOS REALIZADOS SEGÚN PERIODO A DECLARAR( M.B.T.C.S.A)</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon26" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label70" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">27</asp:Label>
                            </td>
                            <td colspan="4">&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">     MENOS : ANTICIPO  DE LA VIGENCIA  AÑO ANTERIOR.</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label71" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">28</asp:Label>
                            </td>
                            <td colspan="4">&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon28" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">     MENOS: SALDO A FAVOR  SEGÚN EL PERIODO A DECLARAR ( M.B.T.C.S.A)</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon28" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label72" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">29</asp:Label>
                            </td>
                            <td colspan="4">&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LblDescRenglon29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">     MENOS: SALDO A FAVOR DE LA VIGENCIA AÑO ANTERIOR</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label73" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">30</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL AUTORRETENCIONES ( Renglon 25 menos 26 al 29)</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" rowspan="8">
                                <asp:Image ID="Image9" runat="server" ImageUrl="~/Imagenes/Otros/img_liquidaciones.png" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label74" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">31</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon31" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">RETENCION DE ICA  ACTIVIDAD INDUSTRIAL</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon31" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label75" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">32</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">RETENCION DE ICA  ACTIVIDAD COMERCIAL</asp:Label>
                            </td>
                            <td align="right" colspan="7">
                                <asp:Label ID="LblValorRenglon32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label76" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">33</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">RETENCION DE ICA  ACTIVIDAD SERVICIOS</asp:Label>
                            </td>
                            <td align="right" colspan="7">
                                <asp:Label ID="LblValorRenglon33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label77" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">34</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">RETENCION DE ICA ACTIVIDAD POR SERVICIOS FINANCIEROS</asp:Label>
                            </td>
                            <td align="right" colspan="7">
                                <asp:Label ID="LblValorRenglon34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label78" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">35</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">RETENCION POR TARJETAS DEBITO Y CREDITO</asp:Label>
                            </td>
                            <td align="right" colspan="7">
                                <asp:Label ID="LblValorRenglon35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label96" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">36</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">RETENCION  POR OTROS CONCEPTOS</asp:Label>
                            </td>
                            <td align="right" colspan="7">
                                <asp:Label ID="LblValorRenglon36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label97" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">37</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon37" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">Menos: TOTAL RETENCIONES A TITULO DE INDUSTRIA Y COMERCIO  QUE LE PRACTICARON DURANTE EL PERIODO</asp:Label>
                            </td>
                            <td align="right" colspan="7">
                                <asp:Label ID="LblValorRenglon37" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label98" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">38</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL RETENCIONES (REGLÓN 31+32+33+34+35+36 - RENGLON 37)</asp:Label>
                            </td>
                            <td align="right" colspan="7">
                                <asp:Label ID="LblValorRenglon38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" rowspan="3">
                                <asp:Image ID="Image10" runat="server" Height="130px" ImageUrl="~/Imagenes/Otros/img_subtotales.png" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label135" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">39</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="Label90" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">SANCIONES. </asp:Label>
                                <asp:RadioButton ID="RbExtemporaneidad" runat="server" Font-Size="8pt" Text="Extemporaneidad" Enabled="False" />
                                &nbsp;<asp:RadioButton ID="RbCorreccion2" runat="server" Font-Size="8pt" Text="Corrrección" Enabled="False" />
                                &nbsp;<asp:RadioButton ID="RbInexactitud" runat="server" Font-Size="8pt" Text="Inexactitud" Enabled="False" />
                                &nbsp;<asp:RadioButton ID="RbOtra" runat="server" Font-Size="8pt" Text="Otra" Enabled="False" />
                                &nbsp;
                                <asp:Label ID="Label91" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">Cuál:</asp:Label>
                                <telerik:RadTextBox ID="TxtDescripcionSancion" runat="server" EmptyMessage="Descripción" Enabled="False" Font-Size="15pt" Height="25px" MaxLength="120" TabIndex="6" Width="200px">
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle Resize="None" />
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="Validator3" runat="server" ControlToValidate="TxtDescripcionSancion" Display="None" Enabled="False" ErrorMessage="Este campo es requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validator3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validator3">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                            <td align="right" colspan="7">
                                <telerik:RadNumericTextBox ID="TxtValorRenglon39" runat="server" EmptyMessage="Valor" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="10" MinValue="0" TabIndex="7" Value="0" Width="160px" Enabled="False">
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
                                <asp:Label ID="Label136" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">40</asp:Label>
                            </td>
                            <td colspan="4">&nbsp;<asp:Label ID="LblDescRenglon40" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL AUTORRETENCIONES  ( RENGLON 30)</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon40" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label137" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">41</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon41" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL RETENCIONES DE INDUSTRIA Y COMERCIO ( RENGLON 38)</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon41" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" rowspan="3">
                                <asp:Image ID="Image5" runat="server" Height="100px" ImageUrl="~/Imagenes/Otros/img_pago.png" />
                            </td>
                            <td align="center">
                                <asp:Label ID="Label139" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">42</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon42" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">VALOR A PAGAR ( RENGLON 39 + RENGLON 40 + RENGLON 41)</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon42" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label140" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">43</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon43" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">MAS INTERESES DE MORA</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <telerik:RadNumericTextBox ID="TxtValorRenglon43" runat="server" EmptyMessage="Intereses" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="10" MinValue="0" TabIndex="8" Value="0" Width="160px" Enabled="False">
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
                                <asp:Label ID="Label141" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">44</asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:Label ID="LblDescRenglon44" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">TOTAL A PAGAR  ( RENGLON 42 + RENGLON 43)</asp:Label>
                            </td>
                            <td colspan="7" align="right">
                                <asp:Label ID="LblValorRenglon44" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Image ID="Image6" runat="server" Height="100px" ImageUrl="~/Imagenes/Otros/img_firmas.png" />
                            </td>
                            <td align="center" colspan="12">
                                <asp:Panel ID="Panel1" runat="server">
                                    <table border="1" style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label109" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt" Width="280px">FIRMA DEL DECLARANTE</asp:Label>
                                                <br />
                                                <telerik:RadBinaryImage ID="ImgFirma1" runat="server" Height="40px" ResizeMode="Fill" Width="180px" />
                                                <br />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RbFirmaContador" runat="server" Font-Size="8pt" Text="FIRMA DEL CONTADOR" Enabled="False" />
                                                <asp:RadioButton ID="RbFirmaRevFiscal" runat="server" Font-Size="8pt" Text="REVISOR FISCAL" Enabled="False" />
                                                <br />
                                                <telerik:RadBinaryImage ID="ImgFirma2" runat="server" Height="40px" ResizeMode="Fill" Width="180px" />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label110" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">NOMBRE:</asp:Label>
                                                &nbsp;<asp:DropDownList ID="CmbFirmante1" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione un nombre" Enabled="False">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="Validator5" runat="server" ControlToValidate="CmbFirmante1" Display="None" ErrorMessage="Este campo es requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                <ajaxToolkit:ValidatorCalloutExtender ID="Validator5_ValidatorCalloutExtender" runat="server" BehaviorID="Validator5_ValidatorCalloutExtender" TargetControlID="Validator5">
                                                </ajaxToolkit:ValidatorCalloutExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label112" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="7pt">NOMBRE:</asp:Label>
                                                &nbsp;<asp:DropDownList ID="CmbFirmante2" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione un nombre de firmante" Enabled="False">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="Validator6" runat="server" ControlToValidate="CmbFirmante2" Display="None" ErrorMessage="Este campo es requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                <ajaxToolkit:ValidatorCalloutExtender ID="Validator6_ValidatorCalloutExtender" runat="server" BehaviorID="Validator6_ValidatorCalloutExtender" TargetControlID="Validator6">
                                                </ajaxToolkit:ValidatorCalloutExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="RbCedulaCiudFirm1" runat="server" Enabled="False" Font-Size="8pt" Text="C.C." />
                                                <asp:RadioButton ID="RbCedulaExtrFirm1" runat="server" Enabled="False" Font-Size="8pt" Text="C.E." />
                                                &nbsp;<asp:Label ID="LblNumeroFirm1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">No.:_________________</asp:Label>
                                                &nbsp;</td>
                                            <td>
                                                <asp:RadioButton ID="RbCedulaCiudFirm2" runat="server" Font-Size="8pt" Text="C.C." Enabled="False" />
                                                <asp:RadioButton ID="RbCedulaExtrFirm2" runat="server" Font-Size="8pt" Text="C.E." Enabled="False" />
                                                &nbsp;<asp:Label ID="LblNumDocFirm2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">No.:_________________</asp:Label>
                                                <asp:RadioButton ID="RbTarjetaProfFirm2" runat="server" Checked="True" Font-Size="8pt" Text="T.P." />
                                                &nbsp;<asp:Label ID="LblNumTpFirm2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">No.:_________________</asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="13">
                                <asp:Panel ID="Panel3" runat="server">
                                    <table style="width:100%;">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label120" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">ACCIONES DE BOTONES</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="BtnVerDetalleCuentas" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnVerDetalleCuentas_Click" Text="Ver Detalle Cuentas" ToolTip="Click para ver el detalle de las cuentas" Width="200px" />
                                                &nbsp;<asp:Button ID="BtnVerFormulario" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnVerFormulario_Click" Text="Ver Formulario" ToolTip="Click para visualizar el formulario definitivo" Width="200px" />
                                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="200px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="13">
                                <asp:Panel ID="Panel2" runat="server">
                                    <table style="width:100%;" border="1">
                                        <tr>
                                            <td align="center">
                                                <br />
                                                <br />
                                                <asp:Label ID="Label113" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="9pt" Width="350px">ESPACIO PARA CÓDIGO DE BARRAS</asp:Label>
                                                <br />
                                                <br />
                                                <br />
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="Label114" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="9pt">ESPACIO PARA NÚMERO DE REFERENCIA RECAUDO FORMULARIO No.</asp:Label>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <br />
                                <asp:Label ID="Label115" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt" Width="100px">ESPACIO PARA CÓDIGO QR</asp:Label>
                                <br />
                                <br />
                            </td>
                            <td align="center" colspan="2">
                                <asp:Label ID="Label116" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt" Width="140px">ESPACIO PARA SELLO O TIMBRE</asp:Label>
                            </td>
                            <td align="center" colspan="10">
                                <asp:Label ID="Label117" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt" Width="350px">ESPACIO PARA SERIAL AUTOMATICO DE TRANSACCIÓN O MECANISMO DE INDENTIFICACIÓN DE RECAUDO</asp:Label>
                            </td>
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
