<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlEstadosFinancieros.ascx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.CtrlEstadosFinancieros" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="width: 100%;">            
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="CARGAR ESTADOS FINANCIEROS DEL CLIENTE" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnUpdateCommand="RadGrid1_UpdateCommand">
                        <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="idcliente_estado_financiero, id_cliente, mes_ef, id_estado" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <EditFormSettings CaptionDataField="idcliente_estado_financiero"
                                CaptionFormatString="Editar Registro: {0}"
                                InsertCaption="Agregar Nuevo Registro">
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                                <FormTemplate>
                                </FormTemplate>
                                <PopUpSettings Modal="True" />
                            </EditFormSettings>
                            <CommandItemTemplate>
                                <asp:LinkButton ID="LnkAddEstadoFinanc" runat="server" CommandName="BtnAddEstadoFinanc" ToolTip="Cargar nuevo estado financiero"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/money_add.png"/> CARGAR NUEVO ESTADO FINANCIERO</asp:LinkButton>
                                <asp:LinkButton ID="LnkActualizarLista" runat="server" CommandName="BtnActualizarLista" ToolTip="Actualizar lista de clientes"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_refresh.png"/> ACTUALIZAR LISTA</asp:LinkButton>
                                <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="idcliente_estado_financiero" EmptyDataText=""
                                    HeaderText="Id" ReadOnly="True" UniqueName="idcliente_estado_financiero">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_cliente" HeaderText="Razón Social"
                                    UniqueName="nombre_cliente" ReadOnly="true" FilterControlWidth="160px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                    UniqueName="nombre_municipio" ReadOnly="true">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="descripcion_anio" HeaderText="Año Gravable"
                                    UniqueName="descripcion_anio" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_anio" DataSourceID="DatosAnios"
                                    HeaderText="Año Gravable" ListDataMember="DtAnios" ListTextField="descripcion_anio"
                                    ListValueField="id_anio" UniqueName="id_anio" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="nombre_mes" HeaderText="Mes del EF"
                                    UniqueName="nombre_mes" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="version_ef" HeaderText="Versión del EF"
                                    UniqueName="version_ef" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="proceso_liquidacion" HeaderText="Proceso"
                                    UniqueName="proceso_liquidacion" Visible="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                    UniqueName="codigo_estado" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_estado" DataSourceID="DatosEstado"
                                    HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                    ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="estado_proceso" HeaderText="Base Gravable"
                                    UniqueName="estado_proceso" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha registro"
                                    UniqueName="fecha_registro" ReadOnly="true">
                                </telerik:GridBoundColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnLiquidar" Text="Liquidar impuesto ICA x Oficina"
                                    UniqueName="BtnLiquidar" ImageUrl="/Imagenes/Iconos/16/window_edit.png">
                                </telerik:GridButtonColumn>--%>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerInfo" Text="Ver detalle estado financiero "
                                    UniqueName="BtnVerInfo" ImageUrl="/Imagenes/Iconos/16/money_dollar.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerBaseGravable" Text="Ver Base Gravable"
                                    UniqueName="BtnVerBaseGravable" ImageUrl="/Imagenes/Iconos/16/img_info.png">
                                </telerik:GridButtonColumn>
                                <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAnular" Text="Anular Estado Financiero"
                                    UniqueName="BtnAnular" ImageUrl="/Imagenes/Iconos/16/img_block.png">
                                </telerik:GridButtonColumn>--%>
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
