<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmValidarLiquidacionesPorLote.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.LiquidacionImpuestos.FrmValidarLiquidacionesPorLote" %>
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
                                    <td align="center" bgcolor="#999999" class="auto-style1">
                                        <asp:Label ID="LblTitulo" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="16pt" ForeColor="White">VALIDAR LIQUIDACIÓN DE IMPUESTOS POR LOTE</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
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
                                                        <asp:Label ID="Label35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Label30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Departamento</asp:Label>
                                                    </td>
                                                    <td align="center">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el año gravable del impuesto" AutoPostBack="True" OnSelectedIndexChanged="CmbAnioGravable_SelectedIndexChanged">
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
                                                        <asp:DropDownList ID="CmbTipoImpuesto" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbTipoImpuesto_SelectedIndexChanged" TabIndex="3" ToolTip="Seleccione el formulario aplicar en el filtro" Enabled="False">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="CmbEstadoLiquidacion" runat="server" Enabled="False" Font-Size="15pt" TabIndex="4" ToolTip="Seleccione el estado de la liquidación">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="CmbDepartamento" runat="server" Font-Size="15pt" TabIndex="5" ToolTip="Seleccione un departamento" Enabled="False">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="BtnConsultar" runat="server" ImageUrl="~/Imagenes/Iconos/32/img_search.png" OnClick="BtnConsultar_Click" ToolTip="Click para realizar proceso de consulta" TabIndex="6" ValidationGroup="ValidarConsulta" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="Panel2" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td align="center" class="auto-style2">
                                                        <asp:Label ID="LblTitulo0" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">LISTA DE LIQUIDACIONES POR MUNICIPIOS</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AllowMultiRowSelection="True"
                                                            AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True" PageSize="12"
                                                            OnNeedDataSource="RadGrid1_NeedDataSource"
                                                            OnItemCommand="RadGrid1_ItemCommand"
                                                            OnSelectedIndexChanged="RadGrid1_SelectedIndexChanged"
                                                            OnPageIndexChanged="RadGrid1_PageIndexChanged" TabIndex="7">
                                                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="idliquidacion_lote, anio_gravable, id_cliente, id_usuario, nombre_analista, idformulario_impuesto, tipo_impuesto, anio_gravable, id_dpto, nombre_dpto, codigo_dane, id_municipio, nombre_municipio, idcliente_establecimiento, idperiodicidad_impuesto, periodo_impuesto, codigo_periodicidad, periodicidad_impuesto, idestado_liquidacion, fecha_limite" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                                                <CommandItemTemplate>
                                                                    <asp:LinkButton ID="LnkActualizarLista" runat="server" CommandName="BtnActualizarLista" ToolTip="Actualizar lista"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_refresh.png"/> ACTUALIZAR LISTA</asp:LinkButton>
                                                                </CommandItemTemplate>
                                                                <Columns>
                                                                    <telerik:GridClientSelectColumn UniqueName="id_registro">
                                                                    </telerik:GridClientSelectColumn>
                                                                    <telerik:GridBoundColumn DataField="id_registro" EmptyDataText=""
                                                                        HeaderText="Id" UniqueName="id_registro" FilterControlWidth="40px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="estado_liquidacion" HeaderText="Estado Liquidación"
                                                                        UniqueName="estado_liquidacion" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="revision_jefe" HeaderText="Revisión Jefe"
                                                                        UniqueName="revision_jefe" FilterControlWidth="70px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="aprobacion_jefe" HeaderText="Aprobación Jefe"
                                                                        UniqueName="aprobacion_jefe" FilterControlWidth="70px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="tipo_impuesto" HeaderText="Tipo Impuesto"
                                                                        UniqueName="tipo_impuesto" Visible="false" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="nombre_dpto" HeaderText="Departamento"
                                                                        UniqueName="nombre_dpto" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="codigo_dane" HeaderText="Cod. Dane"
                                                                        UniqueName="codigo_dane" FilterControlWidth="40px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                                                        UniqueName="nombre_municipio" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="periodicidad_impuesto" HeaderText="Periodicidad"
                                                                        UniqueName="periodicidad_impuesto" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="vencida" HeaderText="Vencida"
                                                                        UniqueName="vencida" FilterControlWidth="40px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="fecha_limite" HeaderText="Fecha Vencimiento"
                                                                        UniqueName="fecha_limite" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="nombre_analista" HeaderText="Analista"
                                                                        UniqueName="nombre_analista" Visible="false" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="email_analista" HeaderText="Email"
                                                                        UniqueName="email_analista" Visible="false" FilterControlWidth="80px">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAprobar" Text="Aprobar o Rechazar Liquidación"
                                                                        UniqueName="BtnAprobar" ImageUrl="/Imagenes/Iconos/16/style_edit.png">
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
                                                        <asp:Panel ID="PnlRespuesta" runat="server" Visible="False">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td align="center" colspan="2">
                                                                        <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">INFORMACIÓN PARA NOTIFICACIÓN DE LAS VALIDACIONES</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2">
                                                                        <asp:Label ID="Label41" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="14pt">Estado de la Validación de la Liquidación</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2">
                                                                        <asp:RadioButtonList ID="RbEstadoAprobacion" runat="server" RepeatDirection="Horizontal" TabIndex="5" ToolTip="Es consorcio unión temporal ?">
                                                                            <asp:ListItem Value="1">APROBADA</asp:ListItem>
                                                                            <asp:ListItem Value="2">RECHAZADA</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                        <asp:RequiredFieldValidator ID="Validador3" runat="server" ControlToValidate="RbEstadoAprobacion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                                        <cc1:ValidatorCalloutExtender ID="Validador3_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador3">
                                                                        </cc1:ValidatorCalloutExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="Label36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Observación de la Validación de Liquidaciones Impuestos</asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="Label37" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Ingrese los Correos separados por Punto y Coma (;) donde llegara la Notificación</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadTextBox ID="TxtObservacionValidacion" runat="server" EmptyMessage="Observación de la Validación" Font-Size="15pt" Height="60px" MaxLength="1000" TabIndex="7" TextMode="MultiLine" Width="500px">
                                                                            <EmptyMessageStyle Resize="None" />
                                                                            <ReadOnlyStyle Resize="None" />
                                                                            <FocusedStyle Resize="None" />
                                                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                                                            <InvalidStyle Resize="None" />
                                                                            <HoveredStyle Resize="None" />
                                                                            <EnabledStyle Resize="None" />
                                                                        </telerik:RadTextBox>
                                                                        <asp:RequiredFieldValidator ID="Validador4" runat="server" ControlToValidate="TxtObservacionValidacion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                                        <cc1:ValidatorCalloutExtender ID="Validador4_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador4">
                                                                        </cc1:ValidatorCalloutExtender>
                                                                    </td>
                                                                    <td align="center">
                                                                        <telerik:RadTextBox ID="TxtEmailsNotificacion" runat="server" EmptyMessage="email1@dominio.com;email2@dominio.com.co;email3@dominio.net;email4@dominio.gov" Font-Size="15pt" Height="60px" MaxLength="1000" TabIndex="7" TextMode="MultiLine" Width="500px">
                                                                            <EmptyMessageStyle Resize="None" />
                                                                            <ReadOnlyStyle Resize="None" />
                                                                            <FocusedStyle Resize="None" />
                                                                            <DisabledStyle HorizontalAlign="Center" Resize="None" />
                                                                            <InvalidStyle Resize="None" />
                                                                            <HoveredStyle Resize="None" />
                                                                            <EnabledStyle Resize="None" />
                                                                        </telerik:RadTextBox>
                                                                        <asp:RequiredFieldValidator ID="Validador5" runat="server" ControlToValidate="TxtEmailsNotificacion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                                        <cc1:ValidatorCalloutExtender ID="Validador5_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validador5">
                                                                        </cc1:ValidatorCalloutExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2">
                                                                        <asp:Label ID="Label38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Nota: </asp:Label>
                                                                        <asp:Label ID="Label39" runat="server" CssClass="FormLabels" Font-Bold="False" Font-Size="11pt">Señor usuario, recuerde que seleccionando uno o varios item de la lista usted podrá dar respuesta de la validación realizada a las liquidaciones por lote, y serán notificada las personas de los correos ingresados.</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2">
                                                                        <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnGuardar_Click" TabIndex="8" Text="Enviar Notificación" ToolTip="Click para guardar validación y enviar notificación" ValidationGroup="ValidarDatos" Width="200px" />
                                                                        &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" TabIndex="9" Text="Salir" ToolTip="Salir" Width="200px" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">&nbsp;</td>
                                                                    <td align="center">&nbsp;</td>
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
                                    <td>
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
