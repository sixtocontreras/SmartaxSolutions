<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmLiquidacionAlumbrado.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos.FrmLiquidacionAlumbrado" %>
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
    <style type="text/css">
        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
            height: 25px;
        }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

            .RadInput .riTextBox {
                height: 17px;
            }

        .auto-style1 {
            height: 29px;
        }
    </style>
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:HiddenField ID="hdCliente" runat="server" />
                 <asp:Panel ID="pnlSelectCliente" runat="server" Width="1200px" HorizontalAlign="Center" BorderStyle="Solid">
                    <table cellpadding="0" cellspacing="0" class="Tab" border="0" style="width: 1200px;">
                        <tr>
                            <td colspan="13" align="center" bgcolor="#999999">
                                <asp:Label ID="Label4" runat="server" CssClass="SubTitle" Text="LIQUIDACIÓN DE ALUMBRADO PÚBLICO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center"  colspan="13">
                                <br />
                                <asp:Label ID="Label8" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="11pt" Text="Cliente"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center"  colspan="13">
                                    <asp:DropDownList ID="CmbCliente" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione Cliente">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbCliente" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                        </tr>
                         <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="8"  align="center">
                                <asp:Button ID="BtnCargar" runat="server" Font-Bold="True" Font-Size="14pt" Text="Cargar" ToolTip="Click para cargar la liquidacion" Width="120px" OnClick="BtnCargar_Click" ValidationGroup="ValidarDatos" />
                                <asp:Button ID="BtnSalirModal" runat="server" Font-Bold="True" Font-Size="14pt" Text="Salir" ToolTip="Click para cerrar la ventana" Width="120px" OnClick="BtnSalirModal_Click"  />
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
                <asp:Panel ID="PnlDatos" runat="server" Visible="false" Width="1200px" HorizontalAlign="Center" BorderStyle="Solid">
                    <table cellpadding="4" cellspacing="0" class="Tab" border="1" style="width: 1200px;">
                        <tr>
                            <td colspan="13" align="center" bgcolor="#999999">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="LIQUIDACIÓN DE ALUMBRADO PÚBLICO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="13">
                                <asp:Label ID="LbTitulo0" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="FORMULARIO UNIVERSAL DEL IMPUESTO DE ALUMBRADO PUBLICO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td  colspan="2" align="right">
                                <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt" Width="140px">MUNICIPIO O DISTRITO:</asp:Label>
                            </td>
                            
                            <td  align="center" colspan="6">
                                <asp:HiddenField ID="hdIdRegistro" runat="server" />
                                <asp:HiddenField ID="hdIdMunicipio" runat="server"/>
                                <asp:Label ID="LblNombreMunicipio" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            
                            <td align="center" colspan="2">
                                <asp:Label ID="Label2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt" Width="155px">Código DANE</asp:Label>
                            </td>
                            <td  colspan="2">
                                <telerik:RadNumericTextBox ID="TxtCodDane" runat="server" AutoPostBack="True" EmptyMessage="Cod. Dane" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="10" MinValue="1" OnTextChanged="TxtCodDane_TextChanged" TabIndex="1" Width="120px" DataType="System.Int32">
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
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Label ID="Label3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">DEPARTAMENTO:</asp:Label>
                            </td>
                            <td  align="center" colspan="6">
                                <asp:Label ID="LblNombreDpto" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" Width="400px"></asp:Label>

                            </td>
                            <td align="center" colspan="3">
                            </td>
                           
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Label ID="Label5" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">AÑO GRAVABLE:</asp:Label>
                            </td>
                            <td colspan="8" >
                            </td>
                            <td colspan="2" align="center">
                                <asp:HiddenField runat="server" ID="hdUVT" />
                                <asp:Label ID="Label14" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">UR. UVT</asp:Label>
                            </td>
                        </tr> 
                        <tr>
                            <td colspan="2" align="center">
                                 <telerik:RadNumericTextBox ID="TxtAnioGravable" runat="server" EmptyMessage="Año Gravable" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="4" MinValue="1" TabIndex="2" Width="120px" AutoPostBack="True" OnTextChanged="TxtAnioGravable_TextChanged">
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
                            <td colspan="8" >
                            </td>
                            <td colspan="2" align="right">
                                <asp:Label ID="lblUVT" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="7">                                
                                <asp:Label ID="Label6" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">Declaración</asp:Label>
                            </td>
                            <td >
                                <asp:Label ID="Label15" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">DD</asp:Label>

                            </td>
                            <td >
                                <asp:Label ID="Label16" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">MM</asp:Label>

                            </td>
                            <td colspan="2">
                                <asp:Label ID="Label22" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">AAAA</asp:Label>

                            </td>
                        </tr>
                         <tr>
                            <td></td>
                            <td colspan="3">
                                <asp:RadioButton Text="Inicial" runat="server" Checked="true"  ></asp:RadioButton>
                                <asp:RadioButton Text="Corrección" runat="server"  Checked="false"></asp:RadioButton>
                            </td>
                            <td >    
                             Declaración que corrige                            
                            </td>
                            <td >                                
                            </td>
                            <td colspan="2">     
                                Fecha Máxima de presentación
                            </td>
                            <td >
                                <asp:HiddenField ID="hdfechaMax" runat="server" />
                                <asp:Label ID="lblDia" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>

                            </td>
                            <td >
                                <asp:Label ID="lblMes" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>

                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblAnio" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="12"  align="center">
                                <asp:Label ID="Label23" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt">Periodicidad</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="12" align="center">
                                <asp:Label ID="lblTituloPeriodicidad" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="8pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"  align="center">
                                <asp:DropDownList ID="ddlPeriodicidad" AutoPostBack="true" Visible="false" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione" OnSelectedIndexChanged="ddlPeriodicidad_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                           
                        </tr>
                        <tr>
                            <td rowspan="6" align="center">
                                <Label  class="FormLabels" style="font-weight:500; font-size:10pt; transform: rotate(180deg); writing-mode: vertical-rl;" >A-INFORMACIÓN DEL CONTRIBUYENTE</Label>
                            </td>
                            <td colspan="5">
                                <asp:Label ID="Label18" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Nombres y Apellidos ó Razón Social del Establecimiento</asp:Label>
                            </td>
                            <td colspan="6">
                                
                                <asp:Label ID="lblNombres" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td colspan="5" align="center">
                                <asp:RadioButtonList runat="server" ID="rblDocCliente" RepeatDirection="Vertical" RepeatLayout="Table" RepeatColumns="3" Enabled="false">
                                    <asp:ListItem Value="1">Cédula Ciudadania</asp:ListItem>
                                    <asp:ListItem Value="2">NIT</asp:ListItem>
                                    <asp:ListItem Value="3">Cédula Extranjeria</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">N°</asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblNumDoc" runat="server" CssClass="FormLabels"  Font-Size="11pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">DV</asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblDV2" runat="server" CssClass="FormLabels"  Font-Size="11pt" ></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td colspan="3">
                                <asp:Label ID="Label19" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">Dirección de Notificación</asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblDireccion" runat="server" CssClass="FormLabels"  Font-Size="11pt" ></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="Label11" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">Departamento</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDptoCliente" runat="server" CssClass="FormLabels"  Font-Size="11pt" ></asp:Label>
                            </td>
                            <td colspan="2" >
                                <asp:Label ID="Label13" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" Width="13px">Municipio</asp:Label>
                            </td>
                            <td >
                                <asp:Label ID="lblMunicipioCliente" runat="server" CssClass="FormLabels"  Font-Size="11pt" ></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td colspan="3">
                                <asp:Label ID="Label10" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" >Correo Electrónico de Notificación</asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblCorreo" runat="server" CssClass="FormLabels"  Font-Size="11pt" ></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="Label17" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" >Teléfono Fijo</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTelFijo" runat="server" CssClass="FormLabels" Font-Size="11pt" ></asp:Label>
                            </td>
                            <td colspan="2" >
                                <asp:Label ID="Label122" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt" >Teléfono Celular</asp:Label>
                            </td>
                            <td >
                                <asp:Label ID="lblCel" runat="server" CssClass="FormLabels"  Font-Size="11pt" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="11">
                                <asp:Label ID="Label20" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Sector</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" align="center">
                               <asp:RadioButtonList runat="server" ID="rblSector" RepeatDirection="Vertical" RepeatLayout="Table" RepeatColumns="5" Enabled="false">
                                    <asp:ListItem Value="1">Comercial</asp:ListItem>
                                    <asp:ListItem Value="2">Industrial</asp:ListItem>
                                    <asp:ListItem Value="3">Servicios</asp:ListItem>
                                    <asp:ListItem Value="4">Público u Oficial</asp:ListItem>
                                    <asp:ListItem Value="5">Especial</asp:ListItem>
                                </asp:RadioButtonList>

                            </td>
                        </tr>
                         <tr>
                            <td rowspan="5" align="center">
                                <Label  class="FormLabels" style="font-weight:500; font-size:11pt; transform: rotate(180deg); writing-mode: vertical-rl;" >B - BASE GRAVABLE</Label>
                            </td>
                            <td>
                                <asp:Label ID="Label42" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">1</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label21" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Clasificación según el Sector al que pertenece el Contribuyente</asp:Label>
                            </td>
                            <td colspan="2">
                                
                                <asp:Label ID="lblClasificacion" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                             <td>
                                <asp:Label ID="Label24" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">2</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Kilovatios Consumidos Mes KW/HM según Rango del Sector (Kilovatios por Hora Mensual)</asp:Label>
                            </td>
                            <td colspan="2" align="center"> 
                                <asp:Label ID="lblRenglon2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>

                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:Label ID="Label26" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">3</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Costo Kilovatio por Hora (KW/H) (Información suministrada por la empresa comercializadora de energía)</asp:Label>
                            </td>
                            <td colspan="2" align="center">
                                
                                <asp:Label ID="lblRenglon3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                
                            </td>
                        </tr>
                         <tr>
                             <td>
                                <asp:Label ID="Label28" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">4</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Base Gravable Impuesto de Alumbrado Público (Multiplique Renglón 2 por Renglón 3)</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                
                                <asp:Label ID="lblBaseGravable" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">5</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label31" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Base Gravable Impuesto de Alumbrado Público Sector Especial (Excepto Valor Absoluto) </asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                
                                <asp:Label ID="lblBaseGravableEspecial" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>


                        <tr>
                            <td rowspan="5" align="center">
                                <Label  class="FormLabels" style="font-weight:500; font-size:10pt; transform: rotate(180deg); writing-mode: vertical-rl;" >C - LIQUIDACIÓN</Label>
                            </td>
                            <td>
                                <asp:Label ID="Label32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">6</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">
                                    Tarifa del Impuesto de Alumbrado Público según Sector seleccionado (Renglón 1)
                                </asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                
                                <asp:Label ID="lblTarifaImpuesto" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                             <td>
                                <asp:Label ID="Label35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">7</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Impuesto de Alumbrado Público (Renglón 4 por Renglón 6)
                                </asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                
                                <asp:Label ID="lblImpuesto" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:Label ID="Label38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">8</asp:Label>
                            </td>
                            <td colspan="6">
                                <asp:Label ID="Label39" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Impuesto de Alumbrado Público Mínimo a Pagar según clasificación (Renglón 1)</asp:Label>
                            </td>
                             <td colspan="2">                                
                                <asp:Label ID="lblUVTMin" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                
                                <asp:Label ID="lblPagoMinimo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                             <td>
                                <asp:Label ID="Label41" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">9</asp:Label>
                            </td>
                            <td colspan="6">
                                <asp:Label ID="Label43" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Impuesto de Alumbrado Público Máximo a Pagar según clasificación (Renglón 1)</asp:Label>
                            </td>
                             <td colspan="2">                                
                                <asp:Label ID="lblUVTMax" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                
                                <asp:Label ID="lblPagoMax" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label45" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">10</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label46" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Impuesto de Alumbrado Público a Pagar según Grupo del Sector Especial (Multiplique Vr. Unidad de Medida por Renglón 5)</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                
                                <asp:Label ID="lblImpuestoEspecial" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>



                        <tr>
                            <td rowspan="5" align="center">
                                <Label  class="FormLabels" style="font-weight:500; font-size:10pt; transform: rotate(180deg); writing-mode: vertical-rl;" >D - PAGOS</Label>
                            </td>
                            <td>
                                <asp:Label ID="Label48" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">11</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label49" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Total Impuesto de Alumbrado Público a Cargo</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                
                                <asp:Label ID="lblTotalImp" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                             <td>
                                <asp:Label ID="Label51" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">12</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label53" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">(-) Menos Descuento Por Pronto Pago</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:HiddenField ID="hddescuentoporcentaje" runat="server" />
                                <asp:Label ID="lblDescuento" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:Label ID="Label55" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">13</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label56" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">(+) Más Intereses de Mora</asp:Label>
                            </td>
                            <td colspan="2" align="center">
                                 <telerik:RadNumericTextBox ID="txtMora" runat="server" EnabledStyle-HorizontalAlign="Right" EmptyMessage="$" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="8" MinValue="0" TabIndex="2" Width="120px" AutoPostBack="True" OnTextChanged="txtMora_TextChanged">
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
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TxtAnioGravable" Display="None" ErrorMessage="Este campo es requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validator2">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                        </tr>
                         <tr>
                             <td>
                                <asp:Label ID="Label58" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">14</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label59" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">(+) Más Sanciones</asp:Label>
                                 <asp:RadioButtonList runat="server" ID="rblSanciones" RepeatDirection="Vertical" RepeatLayout="Table" RepeatColumns="5" >
                                    <asp:ListItem Value="1" Selected="True">Extemporaneidad</asp:ListItem>
                                    <asp:ListItem Value="2">Corrección</asp:ListItem>
                                    <asp:ListItem Value="3">Inexactitud</asp:ListItem>
                                    <asp:ListItem Value="4">Emplazamiento</asp:ListItem>
                                    <asp:ListItem Value="5">Otra</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td colspan="2" align="center">
                                 <telerik:RadNumericTextBox ID="txtSanciones" runat="server" EmptyMessage="$" EnabledStyle-HorizontalAlign="Right" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="8" MinValue="0" TabIndex="2" Width="120px" AutoPostBack="True" OnTextChanged="txtSanciones_TextChanged">
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
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtAnioGravable" Display="None" ErrorMessage="Este campo es requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validator2">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label61" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">15</asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:Label ID="Label62" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Total Impuesto de Alumbrado Público a Pagar (Renglón 11 - 12 + 13 + 14)</asp:Label>
                            </td>
                            <td colspan="2"  align="right">
                                
                                <asp:Label ID="lblTotal" align="right" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>


                        <tr>
                            <td rowspan="3" align="center">
                                <Label  class="FormLabels" style="font-weight:500; font-size:10pt; transform: rotate(180deg); writing-mode: vertical-rl;" >E - FIRMAS</Label>
                            </td>
                            <td colspan="5">
                                <asp:Label ID="Label37" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Firma del declarante</asp:Label>
                            </td>
                            <td colspan="6">
                                 <asp:RadioButtonList runat="server" ID="rblContador" RepeatDirection="Vertical" RepeatLayout="Table" RepeatColumns="5" Enabled="false">
                                    <asp:ListItem Value="1">Contador Público</asp:ListItem>
                                    <asp:ListItem Value="2">Revisor Fiscal</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                         <tr>
                             <td colspan="5">
                                <asp:Image ID="imgFirmante" runat="server" />
                            </td>
                            <td colspan="6">
                                <asp:Image ID="imgContador" runat="server" />
                            </td>
                        </tr>
                         <tr>
                             <td colspan="5">
                                <asp:Label ID="Label34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Nombre</asp:Label>
                                   <asp:DropDownList ID="ddlFirmante" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFirmante_SelectedIndexChanged" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione...">
                                </asp:DropDownList>
                            </td>
                            <td colspan="6">
                                <asp:Label ID="Label40" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="11pt">Nombre</asp:Label>
                                  <asp:DropDownList ID="ddlContador" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlContador_SelectedIndexChanged" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione...">
                                </asp:DropDownList>
                            </td>
                        </tr>
                         <tr>
                             <td colspan="5">
                                <asp:RadioButton Text="CC" Enabled="false" runat="server" ID="rbccFirma"  Checked="false"></asp:RadioButton>                                 
                                <asp:Label ID="lblccfirmante" runat="server" CssClass="FormLabels"  Font-Size="11pt"></asp:Label>
                                <asp:RadioButton Text="CE" Enabled="false" runat="server" ID="rbceFirma" Checked="false"></asp:RadioButton>                                 
                                <asp:Label ID="lblcefirmante" runat="server" CssClass="FormLabels"  Font-Size="11pt"></asp:Label>
                            </td>
                            <td colspan="6">
                                <asp:RadioButton Text="CC" Enabled="false" runat="server" ID="rbccConta" Checked="false"></asp:RadioButton>           
                                <asp:Label ID="lblccConta" runat="server" CssClass="FormLabels"  Font-Size="11pt"></asp:Label>
                                <asp:RadioButton Text="CE" Enabled="false" runat="server" ID="rbceConta"  Checked="false"></asp:RadioButton>           
                                <asp:Label ID="lblceConta" runat="server" CssClass="FormLabels"  Font-Size="11pt"></asp:Label>
                                <asp:RadioButton Text="TP" Enabled="false" runat="server"  ID="rbtpConta" Checked="false"></asp:RadioButton>           
                                <asp:Label ID="lbltpConta" runat="server" CssClass="FormLabels"  Font-Size="11pt"></asp:Label>
                            </td>
                        </tr>
                      
                        <tr>
                            <td align="center" colspan="13">
                                <asp:Panel ID="Panel3" runat="server">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label120" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="10pt">ACCIONES DE BOTONES</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px"  Text="Guardar" ToolTip="Click para guardar el definitivo del formularion del alumbrado" Enabled="false" ValidationGroup="ValidarDatos" Width="200px" OnClick="BtnGuardar_Click" />
                                                <telerik:RadButton  Font-Bold="True" Text="Borrar" ID="BtnBorrar" runat="server" Font-Size="14pt" Height="40px"  ToolTip="Click para borrar el formulario" Width="200px" Enabled="false"  OnClick="BtnBorrar_Click">
                                                    <ConfirmSettings  Title="Confirmación" ConfirmText="¿Esta seguro que desea eliminar este formulario de liquidación?"/>
                                                </telerik:RadButton>
                                               &nbsp;<asp:Button ID="btnPdf" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px"  Text="Descargar pdf" ToolTip="Click para descargar el formulartio" Width="200px" Enabled="false" OnClick="btnPdf_Click" />
                                                <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="200px" OnClick="BtnSalir_Click"/>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
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
