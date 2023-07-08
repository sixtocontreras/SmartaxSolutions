<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlControlActividadesAnalista.ascx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.CtrlControlActividadesAnalista" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server" bgcolor="#E6E6E6">
        <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="CONTROL DE ACTIVIDADES DE ANALISTAS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="Panel2" runat="server">
                        <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo Control Actividad</asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="Label33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Tipo de Impuesto</asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="Label31" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt">Año Gravable</asp:Label>
                                </td>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:DropDownList ID="CmbTipoCtrlActividad" runat="server" Font-Size="15pt" TabIndex="1" ToolTip="Seleccione el tipo ">
                                    </asp:DropDownList>
                                </td>
                                <td align="center">
                                    <asp:DropDownList ID="CmbTipoImpuesto" runat="server" Font-Size="15pt" TabIndex="2" ToolTip="Seleccione el impuesto">
                                    </asp:DropDownList>
                                </td>
                                <td align="center">
                                    <asp:DropDownList ID="CmbAnioGravable" runat="server" Font-Size="15pt" TabIndex="3" ToolTip="Seleccione el año gravable ">
                                    </asp:DropDownList>
                                </td>
                                <td align="center">
                                    <asp:Button ID="BtnConsultar0" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnConsultar_Click" TabIndex="4" Text="Consultar" ToolTip="Click para consultar" Width="140px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" GridLines="None"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged">
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="idcontrol_actividad, idtipo_ctrl_actividades, idformulario_impuesto" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <CommandItemTemplate>
                                <%--<asp:LinkButton ID="LnkAddNew" runat="server" CommandName="BtnAddActividad" ToolTip="Agregar nuevo registro"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_add.png"/> NUEVO REGISTRO</asp:LinkButton>--%>
                                <asp:LinkButton ID="LnkAddActualizar" runat="server" CommandName="BtnActualizar" ToolTip="Actualizar lista"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_refresh.png"/> ACTUALIZAR LISTA</asp:LinkButton>
                                <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="idcontrol_actividad" EmptyDataText=""
                                    HeaderText="Id" ReadOnly="True" UniqueName="idcontrol_actividad">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="tipo_ctrl_actividad" HeaderText="Tipo Ctrl Actividad"
                                    UniqueName="tipo_ctrl_actividad" ReadOnly="true" FilterControlWidth="100px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="descripcion_formulario" HeaderText="Impuesto"
                                    UniqueName="descripcion_formulario" ReadOnly="true" FilterControlWidth="90px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="anio_gravable" HeaderText="Año Gravable"
                                    UniqueName="anio_gravable" ReadOnly="true" FilterControlWidth="80px">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="codigo_actividad" HeaderText="Cod. Actividad"
                                    UniqueName="codigo_actividad" MaxLength="10" FilterControlWidth="60px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="descripcion_actividad" HeaderText="Nombre Actividad"
                                    UniqueName="descripcion_actividad" MaxLength="500" FilterControlWidth="120px">
                                </telerik:GridBoundColumn>
                                <%--<telerik:GridNumericColumn DataField="porc_cumplimiento" HeaderText="% Cumplimiento"
                                    UniqueName="porc_cumplimiento" FilterControlWidth="70px">
                                </telerik:GridNumericColumn>
                                <telerik:GridCheckBoxColumn DataField="del_sistema" HeaderText="Sistema"
                                    UniqueName="del_sistema">
                                </telerik:GridCheckBoxColumn>--%>

                                <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                    UniqueName="codigo_estado" ReadOnly="true" FilterControlWidth="60px">
                                </telerik:GridBoundColumn>

                                <%--<telerik:GridBoundColumn DataField="fecha_modificacion" HeaderText="Fecha modificación"
                                    UniqueName="fecha_modificacion" ReadOnly="true" FilterControlWidth="90px">
                                </telerik:GridBoundColumn>--%>
                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha registro"
                                    UniqueName="fecha_registro" ReadOnly="true" FilterControlWidth="90px">
                                </telerik:GridBoundColumn>

                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddActividad" Text="Gestionar Actividad"
                                    UniqueName="BtnAddActividad" ImageUrl="/Imagenes/Iconos/16/index_add.png">
                                </telerik:GridButtonColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
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
