<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlLogsAuditoria.ascx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.CtrlLogsAuditoria" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server" Width="100%" DefaultButton="BtnConsultar">
        <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
            <tr>
                <td>
                    <asp:Panel ID="Panel2" runat="server" Width="100%" DefaultButton="BtnConsultar">
                        <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                            <tr>
                                <td align="center" bgcolor="#999999">
                                    <asp:Label ID="LblTitulo" runat="server"
                                        Text="LOGS DE AUDITORIA DEL SISTEMA" Font-Bold="True" Font-Size="14pt" ForeColor="White"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="Panel3" runat="server">
                                        <table style="width:100%;">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="Label11" runat="server" Font-Size="15pt" Text="Fecha Inicial"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label10" runat="server" Font-Size="15pt" Text="Fecha Final"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label8" runat="server" Font-Size="15pt" Text="Evento"></asp:Label>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadDatePicker ID="DtFechaInicial" runat="server">
                                                        <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                        </Calendar>
                                                        <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                        <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy">
                                                        </DateInput>
                                                    </telerik:RadDatePicker>
                                                    <asp:RequiredFieldValidator ID="ValidarDato4" runat="server" ControlToValidate="DtFechaInicial" Display="None" ErrorMessage="La Fecha Inicial del Reporte es Requerida ...!" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                    <cc1:ValidatorCalloutExtender ID="ValidarDato4_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="ValidarDato4">
                                                    </cc1:ValidatorCalloutExtender>
                                                </td>
                                                <td align="center">
                                                    <telerik:RadDatePicker ID="DtFechaFinal" runat="server">
                                                        <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                        </Calendar>
                                                        <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                    </telerik:RadDatePicker>
                                                    <asp:RequiredFieldValidator ID="ValidarDato5" runat="server" ControlToValidate="DtFechaFinal" Display="None" ErrorMessage="La Fecha Final del Reporte es Requerida ...!" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                    <cc1:ValidatorCalloutExtender ID="ValidarDato5_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="ValidarDato5">
                                                    </cc1:ValidatorCalloutExtender>
                                                </td>
                                                <td align="center">
                                                    <asp:DropDownList ID="CmbEvento" runat="server" Font-Size="15pt">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center">
                                                    <asp:Button ID="BtnConsultar" runat="server" Font-Bold="True" Font-Size="13pt" Height="35px" OnClick="BtnConsultar_Click" Text="Consultar" ToolTip="Click para consultar información del logs" ValidationGroup="ValidarDatos" Width="150px" />
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
                <td>
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True"
                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        GridLines="None">
                        <MasterTableView DataKeyNames="idlog_auditoria" NoMasterRecordsText="No hay Registros para Mostrar ...">
                            <Columns>
                                <telerik:GridBoundColumn DataField="idlog_auditoria" EmptyDataText="" HeaderText="Id"
                                    UniqueName="idlog_auditoria" FilterControlWidth="40px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_usuario" HeaderText="Usuario"
                                    UniqueName="nombre_usuario" FilterControlWidth="120px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="tipo_evento" HeaderText="Evento"
                                    UniqueName="tipo_evento" FilterControlWidth="70px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="modulo_app" HeaderText="Modulo"
                                    UniqueName="modulo_app" FilterControlWidth="70px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="url_visitada" HeaderText="Url"
                                    UniqueName="url_visitada" FilterControlWidth="100px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ip_cliente" HeaderText="IP"
                                    UniqueName="ip_cliente" FilterControlWidth="60px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="descripcion_evento" HeaderText="Descripción"
                                    UniqueName="descripcion_evento" Visible="false" FilterControlWidth="60px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="F. Registro"
                                    UniqueName="fecha_registro" FilterControlWidth="100px">
                                </telerik:GridBoundColumn>

                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerInfo" Text="Ver detalle de la auditoria"
                                    UniqueName="BtnVerInfo" ImageUrl="/Imagenes/Iconos/16/img_info.png"
                                    HeaderTooltip="Esta opción le permitirá ver mas información">
                                </telerik:GridButtonColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="LblMensaje" runat="server" Font-Bold="True" Font-Size="13pt" ForeColor="#CC0000"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
    </telerik:RadWindowManager>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
    <div class="loading">
        <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" />
    </div>
</telerik:RadAjaxLoadingPanel>
