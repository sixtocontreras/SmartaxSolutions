<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlConsultaLiquidacionImpuestos.ascx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.Consulta.CtrlConsultaLiquidacionImpuestos" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<body bgcolor="#E6E6E6">
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                    <table cellpadding="4" cellspacing="0" class="Tab" border="0" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="CONSULTAR INFORMACIÓN DE LIQUIDACIÓN DE IMPUESTOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Panel ID="Panel2" runat="server">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label6" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo de Impuesto</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="Label7" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Usuario Liquida</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="Label9" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Estado Liquidación</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="Label1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Código Dane</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="Label8" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                                            </td>
                                            <td align="center">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:DropDownList ID="CmbTipoImpuesto" runat="server" AutoPostBack="True" Font-Size="15pt" OnSelectedIndexChanged="CmbTipoImpuesto_SelectedIndexChanged" TabIndex="1" ToolTip="Seleccione el formulario de impuesto">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="CmbUsuarios" runat="server" Font-Size="15pt" TabIndex="2" ToolTip="Seleccione el usuario de la Lista">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="CmbEstado" runat="server" Font-Size="15pt" TabIndex="2" ToolTip="Seleccione el estado de la Lista">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <telerik:RadNumericTextBox ID="TxtCodDane" runat="server" AutoPostBack="True" DataType="System.Int32" EmptyMessage="Cod. Dane" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="10" MinValue="0" OnTextChanged="TxtCodDane_TextChanged" TabIndex="3" Width="120px">
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
                                            <td align="center">
                                                <telerik:RadNumericTextBox ID="TxtAnioGravable" runat="server" AutoPostBack="True" EmptyMessage="Año Gravable" Font-Bold="False" Font-Size="15pt" Height="30px" MaxLength="4" MinValue="1" OnTextChanged="TxtAnioGravable_TextChanged" TabIndex="4" Width="150px">
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
                                            <td align="center">
                                                <asp:Button ID="BtnConsultar" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnConsultar_Click" TabIndex="4" Text="Consultar" ToolTip="Click para realizar consulta de información" ValidationGroup="ValidarDatos" Width="160px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True"
                                    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None"
                                    OnItemCommand="RadGrid1_ItemCommand"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                    <MasterTableView DataKeyNames="idliquid_impuesto, idformulario_impuesto, id_municipio, id_cliente, idcliente_establecimiento, id_estado, idfirmante_1, idfirmante_2" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idliquid_impuesto" EmptyDataText="" FilterControlWidth="40px" HeaderText="Id"
                                                UniqueName="idliquid_impuesto">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="anio_gravable" FilterControlWidth="40px" HeaderText="Año Gravable"
                                                UniqueName="anio_gravable">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_dane" FilterControlWidth="50px" HeaderText="Cód. Dane"
                                                UniqueName="codigo_dane">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_municipio" FilterControlWidth="50px" HeaderText="Municipio"
                                                UniqueName="nombre_municipio">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="periodo_impuesto" HeaderText="Periodo Impuesto"
                                                UniqueName="periodo_impuesto" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion_periodo" HeaderText="Periodo Impuesto"
                                                UniqueName="descripcion_periodo">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="idperiodicidad_impuesto" HeaderText="IdPeriodicidad Impuesto"
                                                UniqueName="idperiodicidad_impuesto" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="periodicidad_impuesto" HeaderText="Periodicidad"
                                                UniqueName="periodicidad_impuesto">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="opcion_uso" HeaderText="Opción de Uso"
                                                UniqueName="opcion_uso" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion_opcion_uso" HeaderText="Opción de Uso"
                                                UniqueName="descripcion_opcion_uso">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="num_declaracion" HeaderText="No. Declaración"
                                                UniqueName="num_declaracion" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="sanciones" HeaderText="Sanciones"
                                                UniqueName="sanciones" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion_sancion" HeaderText="Sanciones"
                                                UniqueName="descripcion_sancion" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion_otro" HeaderText="Otro"
                                                UniqueName="descripcion_otro" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_sancion" FilterControlWidth="60px" HeaderText="V. Sanción"
                                                UniqueName="valor_sancion" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="total_pagar" FilterControlWidth="60px" HeaderText="Total Pagar"
                                                UniqueName="total_pagar">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="destino_aporte_vol" HeaderText="Destino Aporte"
                                                UniqueName="destino_aporte_vol" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="tipo_ejecucion" FilterControlWidth="60px" HeaderText="Tipo Ejecución"
                                                UniqueName="tipo_ejecucion">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="estado_liquidacion" FilterControlWidth="60px" HeaderText="Estado"
                                                UniqueName="estado_liquidacion">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_borrador" FilterControlWidth="80px" HeaderText="F. Borrador"
                                                UniqueName="fecha_borrador">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_definitivo" FilterControlWidth="80px" HeaderText="F. Definitivo"
                                                UniqueName="fecha_definitivo">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_anula" FilterControlWidth="80px" HeaderText="F. Anulación"
                                                UniqueName="fecha_anula">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerInfo" ImageUrl="/Imagenes/Iconos/16/img_info.png"
                                                Text="Ver mas informacion detallada" UniqueName="BtnVerInfo">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerBorrador" ImageUrl="/Imagenes/Iconos/16/window_edit.png"
                                                Text="Ver Liquidación Borrador" UniqueName="BtnVerBorrador">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerDefinitivo" ImageUrl="/Imagenes/Iconos/16/style_edit.png"
                                                Text="Liquidar o Ver Definitivo" UniqueName="BtnVerDefinitivo">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerFormulario" ImageUrl="/Imagenes/Iconos/16/img_view.png"
                                                Text="Ver Formulario" UniqueName="BtnVerFormulario">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAnularLiquidacion"
                                                ConfirmText="¿Señor usuario, se encuentra seguro de realizar la anulación de la liquidación del impuesto ?"
                                                ConfirmTitle="Anulación de Liquidaciones" ImageUrl="/Imagenes/Iconos/16/img_block.png"
                                                Text="Anular Liquidación" UniqueName="BtnAnularLiquidacion">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                                </telerik:RadWindowManager>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
        <div class="loading">
            <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" />
        </div>
    </telerik:RadAjaxLoadingPanel>
</body>
</html>
