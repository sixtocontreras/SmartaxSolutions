<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlFirmantes.ascx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.CtrlFirmantes" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR DATOS DE FIRMANTES" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
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
                        <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="id_firmante, id_rol, idtipo_firma" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <EditFormSettings CaptionDataField="id_firmante"
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
                                <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_firmante" EmptyDataText=""
                                    HeaderText="Id" ReadOnly="True" UniqueName="id_firmante" FilterControlWidth="40px">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="tipo_identificacion" HeaderText="Tipo"
                                    UniqueName="tipo_identificacion" ReadOnly="true" FilterControlWidth="60px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="idtipo_identificacion" DataSourceID="DsTiposIdentificacion"
                                    HeaderText="Tipo" ListDataMember="DtTiposIdentificacion" ListTextField="tipo_identificacion"
                                    ListValueField="idtipo_identificacion" UniqueName="idtipo_identificacion" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="numero_documento" HeaderText="Documento"
                                    UniqueName="numero_documento" MaxLength="15" FilterControlWidth="50px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_completo" HeaderText="Nombre Firmante"
                                    UniqueName="nombre_completo" ReadOnly="true" FilterControlWidth="100px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_funcionario" HeaderText="Nombres"
                                    UniqueName="nombre_funcionario" MaxLength="25" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="apellido_funcionario" HeaderText="Apellidos"
                                    UniqueName="apellido_funcionario" MaxLength="30" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="tarjeta_profesional" HeaderText="Tarjeta Prof."
                                    UniqueName="tarjeta_profesional" MaxLength="20" FilterControlWidth="60px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="numero_contacto" HeaderText="No. Contacto"
                                    UniqueName="numero_contacto" MaxLength="10" FilterControlWidth="60px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="email_contacto" HeaderText="Email Contacto"
                                    UniqueName="email_contacto" MaxLength="60" FilterControlWidth="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridCheckBoxColumn DataField="permite_firmar" HeaderText="Permite Firmar"
                                    UniqueName="permite_firmar" FilterControlWidth="60px">
                                </telerik:GridCheckBoxColumn>

                                <telerik:GridBoundColumn DataField="nombre_rol" HeaderText="Rol"
                                    UniqueName="nombre_rol" ReadOnly="true" FilterControlWidth="60px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_rol" DataSourceID="DsDatosRol"
                                    HeaderText="Rol" ListDataMember="DtRoles" ListTextField="nombre_rol"
                                    ListValueField="id_rol" UniqueName="id_rol" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="tipo_firma" HeaderText="Firma"
                                    UniqueName="tipo_firma" ReadOnly="true" FilterControlWidth="50px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="idtipo_firma" DataSourceID="DsTipoFirma"
                                    HeaderText="Firma" ListDataMember="DtTipoFirma" ListTextField="tipo_firma"
                                    ListValueField="idtipo_firma" UniqueName="idtipo_firma" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                    UniqueName="codigo_estado" ReadOnly="true" FilterControlWidth="60px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_estado" DataSourceID="DatosEstado"
                                    HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                    ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha registro"
                                    UniqueName="fecha_registro" ReadOnly="true" FilterControlWidth="80px">
                                </telerik:GridBoundColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddFirma" Text="Agregar imagen de firma"
                                    UniqueName="BtnAddFirma" ImageUrl="/Imagenes/Iconos/16/img_view.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogType="RadWindow"
                                    ConfirmText="¿Se Encuentra Seguro de Eliminar el registro Seleccionado ...!"
                                    ConfirmTitle="Eliminar el Firmante" Text="Eliminar" UniqueName="DeleteCommand">
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
