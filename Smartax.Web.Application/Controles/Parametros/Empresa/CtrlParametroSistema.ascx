<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlParametroSistema.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Empresa.CtrlParametroSistema" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    .auto-style1 {
        height: 21px;
    }
</style>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="PARAMETRIZAR TIPO DE LICENCIA A EMPRESA" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
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
                        <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="idparametro_sistema" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <EditFormSettings CaptionDataField="idparametro_sistema"
                                CaptionFormatString="Editar Registro: {0}"
                                InsertCaption="Agregar Nuevo Registro">
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                                <FormTemplate>
                                </FormTemplate>
                                <PopUpSettings Modal="True" />
                            </EditFormSettings>
                            <Columns>
                                <telerik:GridBoundColumn DataField="idparametro_sistema" EmptyDataText=""
                                    HeaderText="Id" ReadOnly="True" UniqueName="idparametro_sistema">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="nombre_empresa" HeaderText="Empresa"
                                    UniqueName="nombre_empresa" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_empresa" DataSourceID="DatosEmpresas"
                                    HeaderText="Empresa" ListDataMember="DtEmpresas" ListTextField="nombre_empresa"
                                    ListValueField="id_empresa" UniqueName="id_empresa" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="tipo_licencia" HeaderText="Descripción"
                                    UniqueName="tipo_licencia" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="idtipo_licencia" DataSourceID="DatosLicencia"
                                    HeaderText="Tipo Licencia" ListDataMember="DtTipoLicencia" ListTextField="tipo_licencia"
                                    ListValueField="idtipo_licencia" UniqueName="idtipo_licencia" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridDateTimeColumn DataField="fecha_mensualidad" HeaderText="Fecha Mensualidad"
                                    UniqueName="fecha_mensualidad" Visible="false">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridDateTimeColumn DataField="fecha_pruebas" HeaderText="Fecha Prueba"
                                    UniqueName="fecha_pruebas" Visible="false">
                                </telerik:GridDateTimeColumn>

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

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar Datos" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerInfo" Text="Ver mas informacion de la Licencia"
                                    UniqueName="BtnVerInfo" ImageUrl="/Imagenes/Iconos/16/img_info.png"
                                    HeaderTooltip="Esta opción le permitirá ver mas información">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteCommand"
                                    ConfirmDialogType="RadWindow"
                                    ConfirmText="¿Se Encuentra Seguro de Eliminar el registro seleccionado ....?"
                                    ConfirmTitle="Eliminar Parametro Licencia" Text="Eliminar Usuario">
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
