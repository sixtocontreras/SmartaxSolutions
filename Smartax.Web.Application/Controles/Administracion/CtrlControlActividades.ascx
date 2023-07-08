<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlControlActividades.ascx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.CtrlControlActividades" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server">
        <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR CONTROL DE ACTIVIDADES" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
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
                                    <asp:Button ID="BtnConsultar" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClick="BtnConsultar_Click" TabIndex="4" Text="Consultar" ToolTip="Click para consultar" Width="140px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" OnDeleteCommand="RadGrid1_DeleteCommand" OnInsertCommand="RadGrid1_InsertCommand" OnItemCommand="RadGrid1_ItemCommand" OnItemCreated="RadGrid1_ItemCreated" OnNeedDataSource="RadGrid1_NeedDataSource" OnPageIndexChanged="RadGrid1_PageIndexChanged" OnUpdateCommand="RadGrid1_UpdateCommand">
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="idcontrol_actividad" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <CommandItemTemplate>
                                <asp:LinkButton ID="LnkAddNew" runat="server" CommandName="BtnAddActividad" ToolTip="Agregar nuevo registro"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_add.png"/> NUEVO REGISTRO</asp:LinkButton>
                                <asp:LinkButton ID="LnkAddActualizar" runat="server" CommandName="BtnActualizar" ToolTip="Actualizar lista"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_refresh.png"/> ACTUALIZAR LISTA</asp:LinkButton>
                                <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="idcontrol_actividad" EmptyDataText="" HeaderText="Id" ReadOnly="True" UniqueName="idcontrol_actividad">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="tipo_ctrl_actividad" FilterControlWidth="100px" HeaderText="Tipo Ctrl Actividad" ReadOnly="true" UniqueName="tipo_ctrl_actividad">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="descripcion_formulario" FilterControlWidth="90px" HeaderText="Impuesto" ReadOnly="true" UniqueName="descripcion_formulario">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="anio_gravable" FilterControlWidth="80px" HeaderText="Año Gravable" ReadOnly="true" UniqueName="anio_gravable">
                                </telerik:GridBoundColumn>
                                <%-- <telerik:GridDropDownColumn DataField="idformulario_impuesto" DataSourceID="DatosImpuesto"
                                    HeaderText="Impuesto" ListDataMember="DtFormularioImpuesto" ListTextField="descripcion_formulario"
                                    ListValueField="idformulario_impuesto" UniqueName="idformulario_impuesto" Visible="false">
                                </telerik:GridDropDownColumn>--%>
                                <telerik:GridBoundColumn DataField="nombre_cliente" FilterControlWidth="90px" HeaderText="Cliente" ReadOnly="true" UniqueName="nombre_cliente" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="codigo_actividad" FilterControlWidth="60px" HeaderText="Cod. Actividad" MaxLength="10" UniqueName="codigo_actividad">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="descripcion_actividad" FilterControlWidth="120px" HeaderText="Nombre Tarea" MaxLength="500" UniqueName="descripcion_actividad">
                                </telerik:GridBoundColumn>
                                <telerik:GridNumericColumn DataField="porc_cumplimiento" FilterControlWidth="70px" HeaderText="% Cumplimiento" UniqueName="porc_cumplimiento">
                                </telerik:GridNumericColumn>
                                <telerik:GridCheckBoxColumn DataField="del_sistema" HeaderText="Sistema" UniqueName="del_sistema">
                                </telerik:GridCheckBoxColumn>
                                <telerik:GridBoundColumn DataField="codigo_estado" FilterControlWidth="60px" HeaderText="Estado" ReadOnly="true" UniqueName="codigo_estado">
                                </telerik:GridBoundColumn>
                                <%--<telerik:GridDropDownColumn DataField="id_estado" DataSourceID="DatosEstado"
                                    HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                    ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                </telerik:GridDropDownColumn>--%>
                                <telerik:GridBoundColumn DataField="fecha_modificacion" FilterControlWidth="90px" HeaderText="Fecha modificación" ReadOnly="true" UniqueName="fecha_modificacion">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="fecha_registro" FilterControlWidth="90px" HeaderText="Fecha registro" ReadOnly="true" UniqueName="fecha_registro">
                                </telerik:GridBoundColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnEditar" ImageUrl="/Imagenes/Iconos/16/edit.png" Text="Editar informacion" UniqueName="BtnEditar">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="¿Se Encuentra Seguro de Eliminar el registro Seleccionado ...!" ConfirmTitle="Eliminar la Actividad" Text="Eliminar" UniqueName="DeleteCommand">
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
