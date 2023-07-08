<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlClientes.ascx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.CtrlClientes" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR INFORMACIÓN DE CLIENTES" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged">
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="id_cliente" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <CommandItemTemplate>
                                <asp:LinkButton ID="LnkAddCliente" runat="server" CommandName="BtnAddCliente" ToolTip="Agregar nuevo cliente"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/user1_add.png"/> CREAR NUEVO CLIENTE</asp:LinkButton>
                                <asp:LinkButton ID="LnkActualizarLista" runat="server" CommandName="BtnActualizarLista" ToolTip="Actualizar lista de clientes"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_refresh.png"/> ACTUALIZAR LISTA CLIENTES</asp:LinkButton>
                                <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_cliente" EmptyDataText="" FilterControlWidth="40px"
                                    HeaderText="Id" ReadOnly="True" UniqueName="id_cliente">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="numero_documento" HeaderText="No. Documento"
                                    UniqueName="numero_documento">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="razon_social" HeaderText="Razón Social"
                                    UniqueName="razon_social" FilterControlWidth="160px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                    UniqueName="nombre_municipio">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="direccion_cliente" HeaderText="Dirección"
                                    UniqueName="direccion_cliente">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_contacto" HeaderText="Contacto"
                                    UniqueName="nombre_contacto" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="telefono_contacto" HeaderText="Teléfono"
                                    UniqueName="telefono_contacto" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="numero_puntos" HeaderText="No. Puntos"
                                    UniqueName="numero_puntos">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                    UniqueName="codigo_estado" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha registro"
                                    UniqueName="fecha_registro" ReadOnly="true">
                                </telerik:GridBoundColumn>

                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnEditar" Text="Editar informacion"
                                    UniqueName="BtnEditar" ImageUrl="/Imagenes/Iconos/16/edit.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerInfo" Text="Ver mas informacion detallada"
                                    UniqueName="BtnVerInfo" ImageUrl="/Imagenes/Iconos/16/img_info.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddEstablecimiento" Text="Agregar establecimientos"
                                    UniqueName="BtnAddEstablecimiento" ImageUrl="/Imagenes/Iconos/16/earth.png">
                                </telerik:GridButtonColumn>
                                <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddPuc" Text="Configurar Puc del Cliente"
                                    UniqueName="BtnAddPuc" ImageUrl="/Imagenes/Iconos/16/index_add.png">
                                </telerik:GridButtonColumn>--%>
                                <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddActEconomicas" Text="Agregar actividades economicas"
                                    UniqueName="BtnAddActEconomicas" ImageUrl="/Imagenes/Iconos/16/contract.png">
                                </telerik:GridButtonColumn>--%>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBloquear" Text="Activar/Inactivar"
                                    UniqueName="BtnBloquear" ImageUrl="/Imagenes/Iconos/16/img_block.png">
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings PopUpSettings-Modal="true">
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                                <FormTemplate>
                                </FormTemplate>
                                <PopUpSettings Modal="True" />
                            </EditFormSettings>
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
