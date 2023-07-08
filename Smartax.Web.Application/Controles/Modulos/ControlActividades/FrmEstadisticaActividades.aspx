<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmEstadisticaActividades.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.ControlActividades.FrmEstadisticaActividades" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" />
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" EnableAJAX="False" HorizontalAlign="NotSet">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                            <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                                <tr>
                                    <td align="center" bgcolor="#999999">
                                        <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="White" Text="ESTADISTICAS DE ACTIVIDADES"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="Panel1" runat="server">
                                            <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="Label12" runat="server" Font-Size="14pt" Text="Tipo de Consulta"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Label13" runat="server" Font-Size="14pt" Text="Lista de Analistas"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Label14" runat="server" Font-Size="14pt" Text="Año Gravable"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:RadioButtonList ID="RbtTipoConsulta" runat="server" RepeatDirection="Horizontal" Width="400px" AutoPostBack="True" OnSelectedIndexChanged="RbtTipoConsulta_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Value="4">CONSOLIDADO</asp:ListItem>
                                                            <asp:ListItem Value="3">POR ANALISTA</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="CmbUsuarios" runat="server" Font-Size="14pt" ToolTip="Seleccione un analista de la lista" Enabled="False">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="Validador1" runat="server" ControlToValidate="CmbUsuarios" Display="None" Enabled="False" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarDatos"></asp:RequiredFieldValidator>
                                                        <ajaxToolkit:ValidatorCalloutExtender ID="Validador1_ValidatorCalloutExtender" runat="server" BehaviorID="Validador1_ValidatorCalloutExtender" TargetControlID="Validador1">
                                                        </ajaxToolkit:ValidatorCalloutExtender>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="3" ToolTip="Seleccione el año gravable ">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="BtnConsultar" runat="server" Height="40px" ImageUrl="~/Imagenes/Iconos/32/img_search.png" OnClick="BtnConsultar_Click" Width="40px" ValidationGroup="ValidarDatos" />
                                                        &nbsp;<asp:ImageButton ID="BtnSalir" runat="server" Height="40px" ImageUrl="~/Imagenes/Iconos/32/close-icon.png" OnClientClick="window.close()" ToolTip="Salir" Width="40px" />
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td align="center">
                                                        <asp:Panel ID="Panel3" runat="server">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="Label1" runat="server" Font-Size="14pt" Text="Tipo de Actividad"></asp:Label>
                                                                    </td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:DropDownList ID="CmbTipoCtrlActividad" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo ">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td align="center" colspan="4">
                                                        <asp:Label ID="Label30" runat="server" Font-Size="14pt" Text="Tipo Control Actividad"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="4">
                                                        <asp:DropDownList ID="CmbTipoCtrlActividad" runat="server" Font-Size="14pt" TabIndex="1" ToolTip="Seleccione el tipo control de actividad">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="Panel2" runat="server">
                                            <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadHtmlChart ID="RadHtmlChart1" runat="server" Height="300px" Width="500px" Visible="False">
                                                            <PlotArea>
                                                                <Series>
                                                                </Series>
                                                                <XAxis>
                                                                    <Items>
                                                                    </Items>
                                                                </XAxis>
                                                                <YAxis>
                                                                </YAxis>
                                                            </PlotArea>
                                                            <Legend>
                                                                <Appearance Position="Right">
                                                                </Appearance>
                                                            </Legend>
                                                            <ChartTitle Text="">
                                                                <Appearance Position="Top">
                                                                </Appearance>
                                                            </ChartTitle>
                                                            <Legend>
                                                                <Appearance Position="Top" />
                                                            </Legend>
                                                        </telerik:RadHtmlChart>
                                                    </td>
                                                    <td align="center">
                                                        <telerik:RadHtmlChart ID="RadHtmlChart2" runat="server" Height="300px" Width="500px" Visible="False">
                                                            <PlotArea>
                                                                <Series>
                                                                </Series>
                                                                <XAxis>
                                                                    <Items>
                                                                    </Items>
                                                                </XAxis>
                                                                <YAxis>
                                                                </YAxis>
                                                            </PlotArea>
                                                            <Legend>
                                                                <Appearance Position="Right">
                                                                </Appearance>
                                                            </Legend>
                                                            <ChartTitle Text="">
                                                                <Appearance Position="Top">
                                                                </Appearance>
                                                            </ChartTitle>
                                                            <Legend>
                                                                <Appearance Position="Top" />
                                                            </Legend>
                                                        </telerik:RadHtmlChart>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadHtmlChart ID="RadHtmlChart3" runat="server" Height="300px" Width="500px" Visible="False">
                                                            <PlotArea>
                                                                <Series>
                                                                </Series>
                                                                <XAxis>
                                                                    <Items>
                                                                    </Items>
                                                                </XAxis>
                                                                <YAxis>
                                                                </YAxis>
                                                            </PlotArea>
                                                            <Legend>
                                                                <Appearance Position="Right">
                                                                </Appearance>
                                                            </Legend>
                                                            <ChartTitle Text="">
                                                                <Appearance Position="Top">
                                                                </Appearance>
                                                            </ChartTitle>
                                                            <Legend>
                                                                <Appearance Position="Top" />
                                                            </Legend>
                                                        </telerik:RadHtmlChart>
                                                    </td>
                                                    <td align="center">
                                                        <telerik:RadHtmlChart ID="RadHtmlChart4" runat="server" Height="300px" Width="500px" Visible="False">
                                                            <PlotArea>
                                                                <Series>
                                                                </Series>
                                                                <XAxis>
                                                                    <Items>
                                                                    </Items>
                                                                </XAxis>
                                                                <YAxis>
                                                                </YAxis>
                                                            </PlotArea>
                                                            <Legend>
                                                                <Appearance Position="Right">
                                                                </Appearance>
                                                            </Legend>
                                                            <ChartTitle Text="">
                                                                <Appearance Position="Top">
                                                                </Appearance>
                                                            </ChartTitle>
                                                            <Legend>
                                                                <Appearance Position="Top" />
                                                            </Legend>
                                                        </telerik:RadHtmlChart>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadHtmlChart ID="RadHtmlChart5" runat="server" Height="300px" Width="500px" Visible="False">
                                                            <PlotArea>
                                                                <Series>
                                                                </Series>
                                                                <XAxis>
                                                                    <Items>
                                                                    </Items>
                                                                </XAxis>
                                                                <YAxis>
                                                                </YAxis>
                                                            </PlotArea>
                                                            <Legend>
                                                                <Appearance Position="Right">
                                                                </Appearance>
                                                            </Legend>
                                                            <ChartTitle Text="">
                                                                <Appearance Position="Top">
                                                                </Appearance>
                                                            </ChartTitle>
                                                            <Legend>
                                                                <Appearance Position="Top" />
                                                            </Legend>
                                                        </telerik:RadHtmlChart>
                                                    </td>
                                                    <td align="center">
                                                        <telerik:RadHtmlChart ID="RadHtmlChart6" runat="server" Height="300px" Width="500px" Visible="False">
                                                            <PlotArea>
                                                                <Series>
                                                                </Series>
                                                                <XAxis>
                                                                    <Items>
                                                                    </Items>
                                                                </XAxis>
                                                                <YAxis>
                                                                </YAxis>
                                                            </PlotArea>
                                                            <Legend>
                                                                <Appearance Position="Right">
                                                                </Appearance>
                                                            </Legend>
                                                            <ChartTitle Text="">
                                                                <Appearance Position="Top">
                                                                </Appearance>
                                                            </ChartTitle>
                                                            <Legend>
                                                                <Appearance Position="Top" />
                                                            </Legend>
                                                        </telerik:RadHtmlChart>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="LbTitulo0" runat="server" CssClass="SubTitle" Font-Bold="True" Font-Size="16pt" ForeColor="Black" Text="TABLA DE DATOS ESTADISTICA DE ACTIVIDADES"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                                            AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                            OnNeedDataSource="RadGrid1_NeedDataSource"
                                            OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                            <MasterTableView DataKeyNames="id_registro" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="id_registro" EmptyDataText="" HeaderText="Id"
                                                        UniqueName="id_registro" FilterControlWidth="40px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="tipo_ctrl_actividad" HeaderText="Tipo Ctrl Actividad"
                                                        UniqueName="tipo_ctrl_actividad" FilterControlWidth="100px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="codigo_actividad" HeaderText="Cód. Actividad"
                                                        UniqueName="codigo_actividad" FilterControlWidth="50px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="descripcion_actividad" HeaderText="Nombre Tarea"
                                                        UniqueName="descripcion_actividad" FilterControlWidth="100px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad"
                                                        UniqueName="cantidad" FilterControlWidth="50px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="porcentaje" HeaderText="Porcentaje"
                                                        UniqueName="porcentaje" FilterControlWidth="50px">
                                                    </telerik:GridBoundColumn>
                                                    <%--<telerik:GridBoundColumn DataField="total" HeaderText="Total"
                                                        UniqueName="total" FilterControlWidth="50px">
                                                    </telerik:GridBoundColumn>--%>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">&nbsp;</td>
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
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" />
                </div>
            </telerik:RadAjaxLoadingPanel>
        </div>
    </form>
</body>
</html>
