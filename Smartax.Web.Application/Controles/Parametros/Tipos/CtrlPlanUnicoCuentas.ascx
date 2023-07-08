<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlPlanUnicoCuentas.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Tipos.CtrlPlanUnicoCuentas" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR PLAN UNICO DE CUENTAS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnItemCreated="RadGrid1_ItemCreated"
                        OnInsertCommand="RadGrid1_InsertCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        OnDeleteCommand="RadGrid1_DeleteCommand">
                        <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="id_puc" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <EditFormSettings CaptionDataField="id_puc"
                                CaptionFormatString="Editar Registro: {0}"
                                InsertCaption="Agregar Nuevo Registro">
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                                <FormTemplate>
                                </FormTemplate>
                                <PopUpSettings Modal="True" />
                            </EditFormSettings>
                            <CommandItemTemplate>
                                <asp:LinkButton ID="LnkAddNew" runat="server" CommandName="InitInsert" ToolTip="Agregar nuevo registro"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_add.png"/> NUEVO REGISTRO</asp:LinkButton>
                                <asp:LinkButton ID="LnkLoadPuc" runat="server" CommandName="BtnLoadPuc" ToolTip="Realizar cargue masivo del P.U.C."><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/index_add.png"/> CARGUE MASIVO P.U.C</asp:LinkButton>
                                <asp:LinkButton ID="LnkLoadPucImp" runat="server" CommandName="BtnLoadPucImpusto" ToolTip="Cargue masivo de Impuestos al P.U.C."><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/index_add.png"/> CARGUE MASIVO IMPUESTO P.U.C</asp:LinkButton>
                                <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                                <asp:LinkButton ID="LnkActualizarLista" runat="server" CommandName="BtnActualizarLista" ToolTip="Actualizar lista de clientes"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_refresh.png"/> ACTUALIZAR LISTA</asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_puc" EmptyDataText=""
                                    HeaderText="Id" ReadOnly="True" UniqueName="id_puc" FilterControlWidth="40px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="codigo_cuenta" HeaderText="Cod. Cuenta"
                                    UniqueName="codigo_cuenta" MaxLength="20">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="cod_cuenta_padre" HeaderText="Cuenta Padre"
                                    UniqueName="cod_cuenta_padre" MaxLength="20">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_cuenta" HeaderText="Nombre Cuenta"
                                    UniqueName="nombre_cuenta">
                                </telerik:GridBoundColumn>
                                <telerik:GridCheckBoxColumn DataField="base_gravable" HeaderText="Base Gravable"
                                    UniqueName="base_gravable">
                                </telerik:GridCheckBoxColumn>

                                <telerik:GridBoundColumn DataField="tipo_movimiento" HeaderText="Movimiento"
                                    UniqueName="tipo_movimiento" ReadOnly="true" FilterControlWidth="30px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_movimiento" DataSourceID="DtMov"
                                    HeaderText="Movimiento" ListDataMember="DtMovimiento" ListTextField="tipo_movimiento"
                                    ListValueField="id_movimiento" UniqueName="id_movimiento" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="tipo_nivel" HeaderText="Nivel"
                                    UniqueName="tipo_nivel" ReadOnly="true" FilterControlWidth="50px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="idtipo_nivel" DataSourceID="DtNiveles"
                                    HeaderText="Nivel" ListDataMember="DtTipoNivel" ListTextField="tipo_nivel"
                                    ListValueField="idtipo_nivel" UniqueName="idtipo_nivel" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="tipo_naturaleza" HeaderText="Naturaleza"
                                    UniqueName="tipo_naturaleza" ReadOnly="true" FilterControlWidth="60px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="idtipo_naturaleza" DataSourceID="DtNaturaleza"
                                    HeaderText="Naturaleza" ListDataMember="DtTipoNaturaleza" ListTextField="tipo_naturaleza"
                                    ListValueField="idtipo_naturaleza" UniqueName="idtipo_naturaleza" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="codigo_moneda" HeaderText="Moneda"
                                    UniqueName="codigo_moneda" ReadOnly="true" FilterControlWidth="40px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="idtipo_moneda" DataSourceID="DtMoneda"
                                    HeaderText="Moneda" ListDataMember="DtTipoMoneda" ListTextField="codigo_moneda"
                                    ListValueField="idtipo_moneda" UniqueName="idtipo_moneda" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="nombre_cliente" HeaderText="Cliente"
                                    UniqueName="nombre_cliente" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_cliente" DataSourceID="DatosCliente"
                                    HeaderText="Cliente" ListDataMember="DtClientes" ListTextField="nombre_cliente"
                                    ListValueField="id_cliente" UniqueName="id_cliente" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                    UniqueName="codigo_estado" ReadOnly="true" FilterControlWidth="60px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_estado" DataSourceID="DatosEstado"
                                    HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                    ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha registro"
                                    UniqueName="fecha_registro" ReadOnly="true">
                                </telerik:GridBoundColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnImpuesto" Text="Asignar impuesto a la cuenta"
                                    UniqueName="BtnImpuesto" ImageUrl="/Imagenes/Iconos/16/contract.png"
                                    HeaderTooltip="Esta opción le permitirá asignar impuesto a la cuenta">
                                </telerik:GridButtonColumn>
                                <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBloquear" Text="Activar/Inactivar del Comercio"
                                    UniqueName="BtnBloquear" ImageUrl="/Imagenes/Iconos/16/img_block.png"
                                    HeaderTooltip="Esta opción le permitirá Activar o Inactivar el Comercio">
                                </telerik:GridButtonColumn>--%>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogType="RadWindow"
                                    ConfirmText="¿Se Encuentra Seguro de Eliminar el registro Seleccionado ...!"
                                    ConfirmTitle="Eliminar la cuenta del Puc" Text="Eliminar" UniqueName="DeleteCommand">
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
