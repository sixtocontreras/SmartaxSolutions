<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmEjecucionPorLotes.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos.FrmEjecucionPorLotes" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
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
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" />
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                            <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                                <tr>
                                    <td align="center" bgcolor="#999999" class="auto-style1" colspan="4">
                                        <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">PROCESO DE LIQUIDACION DE IMPUESTOS POR LOTE</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Panel ID="Panel1" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="Label27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Label28" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo de Filtro</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Label29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo de Impuesto</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Label30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Departamento</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione un año de la lista" AutoPostBack="True" OnSelectedIndexChanged="CmbAnioGravable_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbAnioGravable" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarConsulta"></asp:RequiredFieldValidator>
                                                        <cc1:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                                        </cc1:ValidatorCalloutExtender>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="CmbTipoFiltro" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbTipoFiltro_SelectedIndexChanged" TabIndex="2" ToolTip="Seleccione el tipo filtro" Enabled="False">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="Validador2" runat="server" ControlToValidate="CmbTipoFiltro" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarConsulta"></asp:RequiredFieldValidator>
                                                        <cc1:ValidatorCalloutExtender ID="Validador2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador2">
                                                        </cc1:ValidatorCalloutExtender>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="CmbTipoImpuesto" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbTipoImpuesto_SelectedIndexChanged" TabIndex="3" ToolTip="Seleccione el tipo de impuesto" Enabled="False">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="CmbTipoImpuesto" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarConsulta"></asp:RequiredFieldValidator>
                                                        <cc1:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                                                        </cc1:ValidatorCalloutExtender>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="CmbDepartamento" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbDepartamento_SelectedIndexChanged" TabIndex="4" ToolTip="Seleccione el departamento" Enabled="False">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="Validador4" runat="server" ControlToValidate="CmbDepartamento" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarConsulta"></asp:RequiredFieldValidator>
                                                        <cc1:ValidatorCalloutExtender ID="Validador4_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador4">
                                                        </cc1:ValidatorCalloutExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="Label35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Mes Impuesto</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Label33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado de la Declaración</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Label36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado de Aprobación</asp:Label>
                                                    </td>
                                                    <td align="center">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:DropDownList ID="CmbMeses" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbMeses_SelectedIndexChanged" TabIndex="5" ToolTip="Seleccione un mes de la lista">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="CmbEstadoLiquidacion" runat="server" Font-Size="15pt" TabIndex="6" ToolTip="Seleccione un estado de la lista" AutoPostBack="True" OnSelectedIndexChanged="CmbEstadoLiquidacion_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="CmbEstadoAprobacion" runat="server" Font-Size="15pt" TabIndex="7" ToolTip="Seleccione un estado de la lista" AutoPostBack="True" OnSelectedIndexChanged="CmbEstadoAprobacion_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="BtnConsultar" runat="server" ImageUrl="~/Imagenes/Iconos/32/img_search.png" OnClick="BtnConsultar_Click" TabIndex="8" ToolTip="Click para realizar proceso de consulta" ValidationGroup="ValidarConsulta" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:Panel ID="Panel2" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td align="center" class="auto-style2">
                                                        <asp:Label ID="LblTitulo0" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">LISTADO DE MUNICIPIOS A PROCESAR LIQUIDACIÓN</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AllowMultiRowSelection="True"
                                                            AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True" PageSize="12"
                                                            OnNeedDataSource="RadGrid1_NeedDataSource"
                                                            OnItemCommand="RadGrid1_ItemCommand"
                                                            OnSelectedIndexChanged="RadGrid1_SelectedIndexChanged"
                                                            OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                                            <MasterTableView DataKeyNames="id_registro, id_cliente, idformulario_impuesto, anio_gravable, id_dpto, nombre_dpto, codigo_dane, id_municipio, nombre_municipio, idcliente_establecimiento, idliquidacion_lote, idperiodicidad_impuesto, idperiodicidad_pago, codigo_periodicidad, periodicidad_impuesto, fecha_limite" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                                                <Columns>
                                                                    <telerik:GridClientSelectColumn UniqueName="id_registro">
                                                                    </telerik:GridClientSelectColumn>
                                                                    <telerik:GridBoundColumn DataField="id_registro" EmptyDataText=""
                                                                        HeaderText="Id" UniqueName="id_registro" FilterControlWidth="40px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="estado_declaracion" HeaderText="Estado Declaración"
                                                                        UniqueName="estado_declaracion" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="estado_revision" HeaderText="Estado Revisión"
                                                                        UniqueName="estado_revision" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="estado_aprobacion" HeaderText="Estado Aprobación"
                                                                        UniqueName="estado_aprobacion" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="nombre_dpto" HeaderText="Departamento"
                                                                        UniqueName="nombre_dpto" Visible="false" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="codigo_dane" HeaderText="Cod. Dane"
                                                                        UniqueName="codigo_dane" FilterControlWidth="50px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                                                        UniqueName="nombre_municipio" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="periodicidad_impuesto" HeaderText="Periodicidad"
                                                                        UniqueName="periodicidad_impuesto" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="vencida" HeaderText="Vencida"
                                                                        UniqueName="vencida" FilterControlWidth="60px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="fecha_limite" HeaderText="Fecha Vencimiento"
                                                                        UniqueName="fecha_limite" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerInfo" Text="Ver Información de la Validación"
                                                                        UniqueName="BtnVerInfo" ImageUrl="/Imagenes/Iconos/16/img_info.png">
                                                                    </telerik:GridButtonColumn>
                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerFormulario" Text="Ver Liquidación Impuesto"
                                                                        UniqueName="BtnVerFormulario" ImageUrl="/Imagenes/Iconos/16/img_view.png">
                                                                    </telerik:GridButtonColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                            <ClientSettings>
                                                                <Selecting AllowRowSelect="true" />
                                                            </ClientSettings>
                                                            <SelectedItemStyle BackColor="Yellow" BorderColor="Purple" BorderStyle="Dashed" BorderWidth="1px" />
                                                        </telerik:RadGrid>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Panel ID="PnlEstado" runat="server" Visible="False">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="Label39" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado de la Declaración a Procesar</asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="Label34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Ingrese los Correos separados por Punto y Coma (;) donde llegara la Notificación</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:RadioButtonList ID="RbEstadoDeclaracion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RbEstadoDeclaracion_SelectedIndexChanged" RepeatDirection="Horizontal" TabIndex="10" ToolTip="Es consorcio unión temporal ?">
                                                                            <asp:ListItem Value="2">PRELIMINAR</asp:ListItem>
                                                                            <asp:ListItem Value="3">DEFINITIVO</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                        <asp:RequiredFieldValidator ID="Validador5" runat="server" ControlToValidate="RbEstadoDeclaracion" Display="None" ErrorMessage="Debe seleccionar este campo !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                                        <cc1:ValidatorCalloutExtender ID="Validador5_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador5">
                                                                        </cc1:ValidatorCalloutExtender>
                                                                    </td>
                                                                    <td align="center">
                                                                        <telerik:RadTextBox ID="TxtEmailsConfirmacion" runat="server" EmptyMessage="email1@dominio.com;email2@dominio.com.co;email3@dominio.net;email4@dominio.gov" Font-Size="15pt" Height="60px" MaxLength="1000" TabIndex="3" TextMode="MultiLine" Width="600px">
                                                                            <EmptyMessageStyle Resize="None" />
                                                                            <ReadOnlyStyle Resize="None" />
                                                                            <FocusedStyle Resize="None" />
                                                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                                                            <InvalidStyle Resize="None" />
                                                                            <HoveredStyle Resize="None" />
                                                                            <EnabledStyle Resize="None" />
                                                                        </telerik:RadTextBox>
                                                                        <asp:RequiredFieldValidator ID="Validador6" runat="server" ControlToValidate="TxtEmailsConfirmacion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                                        <cc1:ValidatorCalloutExtender ID="Validador6_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador6">
                                                                        </cc1:ValidatorCalloutExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2">
                                                                        <asp:Panel ID="Panel3" runat="server">
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="Label37" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Seleccione un Representante Legal</asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="Label38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Seleccione un Contador o Revisor Fiscal</asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <asp:DropDownList ID="CmbFirmante1" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbFirmante1_SelectedIndexChanged" TabIndex="1" ToolTip="Seleccione un nombre" Enabled="False">
                                                                                        </asp:DropDownList>
                                                                                        <asp:RequiredFieldValidator ID="Validador7" runat="server" ControlToValidate="CmbFirmante1" Display="None" ErrorMessage="Debe seleccionar el firmante 1 de la lista !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                                                        <cc1:ValidatorCalloutExtender ID="Validador7_ValidatorCalloutExtender" runat="server" BehaviorID="Validador7_ValidatorCalloutExtender" TargetControlID="Validador7">
                                                                                        </cc1:ValidatorCalloutExtender>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:DropDownList ID="CmbFirmante2" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbFirmante2_SelectedIndexChanged" TabIndex="1" ToolTip="Seleccione un nombre de firmante" Enabled="False">
                                                                                        </asp:DropDownList>
                                                                                        <asp:RequiredFieldValidator ID="Validador8" runat="server" ControlToValidate="CmbFirmante2" Display="None" ErrorMessage="Debe seleccionar el firmante 2 de la lista !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                                                        <cc1:ValidatorCalloutExtender ID="Validador8_ValidatorCalloutExtender" runat="server" BehaviorID="Validador8_ValidatorCalloutExtender" TargetControlID="Validador8">
                                                                                        </cc1:ValidatorCalloutExtender>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="Label19" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Nota: </asp:Label>
                                        <asp:Label ID="Label18" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="11pt">Señor usuario, recuerde que una vez da clic sobre el botón “Ejecutar Proceso”, el mismo se realizará en “Background”, cuando el proceso finalice recibirá un correo de confirmación.</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Button ID="BtnEjecutar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnEjecutar_Click" Text="Ejecutar Proceso" ToolTip="Click para ejecutar el proceso por Lote" ValidationGroup="ValidarDatos" Width="180px" Height="40px" TabIndex="8" Enabled="False" />
                                        &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="180px" Height="40px" TabIndex="9" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="LblMensaje" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </telerik:RadAjaxPanel>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
                <div class="loading">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" Height="150px" />
                    <h3>Espere un momento por favor ...</h3>
                </div>
            </telerik:RadAjaxLoadingPanel>
        </div>
    </form>
</body>
</html>
