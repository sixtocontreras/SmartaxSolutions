<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlOpcionesMenu.ascx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.CtrlOpcionesMenu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR OPCIONES DE MENU" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnItemCreated="RadGrid1_ItemCreated"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnInsertCommand="RadGrid1_InsertCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        OnDeleteCommand="RadGrid1_DeleteCommand"
                        GridLines="None">
                        <MasterTableView DataKeyNames="id_navegacion" EditMode="PopUp" CommandItemDisplay="Top" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <EditFormSettings CaptionDataField="id_navegacion"
                                CaptionFormatString="Editar Registro: {0}"
                                InsertCaption="Agregar Nuevo Registro">
                                <EditColumn UniqueName="EditCommandColumn1">
                                </EditColumn>
                                <FormTemplate>
                                </FormTemplate>
                                <PopUpSettings Modal="True" />
                            </EditFormSettings>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_navegacion" EmptyDataText=""
                                    HeaderText="Id" UniqueName="id_navegacion" ReadOnly="true" FilterControlWidth="50px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="titulo_opcion" EmptyDataText=""
                                    HeaderText="Titulo" UniqueName="titulo_opcion">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="descripcion_opcion" EmptyDataText=""
                                    HeaderText="Descripcion" UniqueName="descripcion_opcion" FilterControlWidth="160px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="url_opcion" EmptyDataText=""
                                    HeaderText="Url" UniqueName="url_opcion" FilterControlWidth="160px">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="titulo_opcion2" EmptyDataText="" FilterControlWidth="110px"
                                    HeaderText="Opción Padre" UniqueName="titulo_opcion2" ReadOnly="true" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="padre_id" DataSourceID="Datos"
                                    HeaderText="Opción Padre" ListDataMember="DtPadreNavegacion" ListTextField="titulo_opcion"
                                    ListValueField="padre_id" UniqueName="padre_id">
                                </telerik:GridDropDownColumn>

                                <telerik:GridNumericColumn DataField="orden_opcion" HeaderText="No. Orden"
                                    UniqueName="orden_opcion" DataType="System.Int32" NumericType="Number" FilterControlWidth="50px">
                                </telerik:GridNumericColumn>

                                <telerik:GridBoundColumn DataField="tipo_menu" EmptyDataText=""
                                    HeaderText="Tipo Opción" UniqueName="tipo_menu" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="idtipo_menu" DataSourceID="DatosMenu"
                                    HeaderText="Tipo Opción" ListDataMember="DtTipoMenu" ListTextField="tipo_menu"
                                    ListValueField="idtipo_menu" UniqueName="idtipo_menu" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Modificar opción" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteCommand"
                                    ConfirmDialogType="RadWindow"
                                    ConfirmText="¿Esta Seguro Eliminar la Opciòn del Menu Seleccionada ....?"
                                    ConfirmTitle="Eliminar Opcion de Menu" Text="Eliminar Opcion de Menu">
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
        <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" Height="150px" />
        <h3>Espere un momento por favor ...
        </h3>
    </div>
</telerik:RadAjaxLoadingPanel>
