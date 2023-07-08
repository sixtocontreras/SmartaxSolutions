<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlClienteImpuestos.ascx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.CtrlClienteImpuestos" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REALIZAR CONFIGURACIÓN DE IMPUESTOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnInsertCommand="RadGrid1_InsertCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        OnDeleteCommand="RadGrid1_DeleteCommand">
                        <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="idcliente_impuesto, idformulario_impuesto" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <Columns>
                                <telerik:GridBoundColumn DataField="idcliente_impuesto" EmptyDataText=""
                                    HeaderText="Id" ReadOnly="True" UniqueName="idcliente_impuesto">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="id_cliente" HeaderText="Id" EmptyDataText=""
                                    UniqueName="id_cliente" Visible="false" ReadOnly="True">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_cliente" HeaderText="Cliente"
                                    UniqueName="nombre_cliente" ReadOnly="true">
                                </telerik:GridBoundColumn>

                                <%--<telerik:GridBoundColumn DataField="idformulario_impuesto" HeaderText="Id" EmptyDataText=""
                                    UniqueName="idformulario_impuesto" Visible="false" ReadOnly="True">
                                </telerik:GridBoundColumn>--%>
                                <telerik:GridBoundColumn DataField="descripcion_formulario" HeaderText="Formulario"
                                    UniqueName="descripcion_formulario" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="idformulario_impuesto" DataSourceID="DatosForm"
                                    HeaderText="Formulario" ListDataMember="DtFormularioImpuesto" ListTextField="descripcion_formulario"
                                    ListValueField="idformulario_impuesto" UniqueName="idformulario_impuesto" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                    UniqueName="codigo_estado" ReadOnly="true">
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
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnConfiguracion" Text="Configuración del Impuesto"
                                    UniqueName="BtnConfiguracion" ImageUrl="/Imagenes/Iconos/16/img_comportamiento.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnContabilizacion" Text="Contabilización de Renglones"
                                    UniqueName="BtnContabilizacion" ImageUrl="/Imagenes/Iconos/16/script_edit.png">
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
