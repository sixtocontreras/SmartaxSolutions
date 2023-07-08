<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlMunicipios.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.CtrlMunicipios" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR INFORMACIÓN DE MUNICIPIOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnItemCreated="RadGrid1_ItemCreated"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged"
                        OnInsertCommand="RadGrid1_InsertCommand"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        OnDeleteCommand="RadGrid1_DeleteCommand">
                        <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="id_municipio" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <EditFormSettings CaptionDataField="id_municipio"
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
                                <asp:LinkButton ID="LnkLoadCalendario" runat="server" CommandName="BtnLoadCalendario" ToolTip="Realizar cargue masivo de Calendario"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/index_add.png"/> CARGAR CALENDARIO</asp:LinkButton>
                                <asp:LinkButton ID="LnkLoadActEconomica" runat="server" CommandName="BtnLoadActEconomica" ToolTip="Realizar cargue masivo de Actividades Economicas"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/index_add.png"/> CARGAR ACT. ECONOMICAS</asp:LinkButton>
                                <asp:LinkButton ID="LnkLoadImpMunicipio" runat="server" CommandName="BtnLoadImpMunicipio" ToolTip="Realizar cargue masivo de Impuestos Municipio"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/index_add.png"/> CARGAR IMPUESTOS</asp:LinkButton>
                                <asp:LinkButton ID="LnkLoadOtrasConf" runat="server" CommandName="BtnLoadOtrasConfig" ToolTip="Realizar cargue masivo de otras configuraciones"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/index_add.png"/> CARGAR OTRAS CONFIGURACIONES</asp:LinkButton>
                                <asp:LinkButton ID="LnkLoadBancos" runat="server" CommandName="BtnLoadBancoCuentas" ToolTip="Realizar cargue masivo de Bancos y Cuentas"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/index_add.png"/> CARGAR BANCOS, CUENTAS</asp:LinkButton>
                                <asp:LinkButton ID="LnkLoadAuto" runat="server" CommandName="BtnLoadAutoretenciones" ToolTip="Realizar cargue masivo de Autoretenciones"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/index_add.png"/> CARGAR AUTORETENCIONES</asp:LinkButton>
                                <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_municipio" EmptyDataText=""
                                    HeaderText="Id" ReadOnly="True" UniqueName="id_municipio" FilterControlWidth="40px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_departamento" HeaderText="Departamento"
                                    UniqueName="nombre_departamento" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_dpto" DataSourceID="DsDptos"
                                    HeaderText="Dpto" ListDataMember="DtDptos" ListTextField="nombre_departamento"
                                    ListValueField="id_dpto" UniqueName="id_dpto" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="codigo_dane_mun" HeaderText="Cod. Dane"
                                    UniqueName="codigo_dane_mun" MaxLength="10" FilterControlWidth="50px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                    UniqueName="nombre_municipio" MaxLength="30" FilterControlWidth="100px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="numero_nit" HeaderText="Nit"
                                    UniqueName="numero_nit" MaxLength="12" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="digito_verificacion" HeaderText="Dig. Verificacion"
                                    UniqueName="digito_verificacion" MaxLength="1" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_contacto" HeaderText="Contacto"
                                    UniqueName="nombre_contacto" MaxLength="60" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="direccion_contacto" HeaderText="Dirección"
                                    UniqueName="direccion_contacto" MaxLength="100" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="telefono_contacto" HeaderText="Teléfono"
                                    UniqueName="telefono_contacto" MaxLength="20" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="email_contacto" HeaderText="Email"
                                    UniqueName="email_contacto" MaxLength="60" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridCheckBoxColumn DataField="liquidacion_mixta" HeaderText="Liquidacion Mixta"
                                    UniqueName="liquidacion_mixta" Visible="false">
                                </telerik:GridCheckBoxColumn>

                                <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                    UniqueName="codigo_estado" ReadOnly="true" FilterControlWidth="60px">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="id_estado" DataSourceID="DsEstado"
                                    HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                    ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                </telerik:GridDropDownColumn>

                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha registro"
                                    UniqueName="fecha_registro" ReadOnly="true">
                                </telerik:GridBoundColumn>

                                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" ButtonType="ImageButton"
                                    InsertText="Insertar" UpdateText="Actualizar">
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnVerInfo" Text="Ver informacion del municipio"
                                    UniqueName="BtnVerInfo" ImageUrl="/Imagenes/Iconos/16/img_info.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddAcuerdosMun" Text="Consultar Documentos"
                                    UniqueName="BtnAddAcuerdosMun" ImageUrl="/Imagenes/Iconos/16/user-message.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddImpuesto" Text="Asignar impuestos del municipio"
                                    UniqueName="BtnAddImpuesto" ImageUrl="/Imagenes/Iconos/16/index_add.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddActEconomicas" Text="Asignar Actividades economicas del municipio"
                                    UniqueName="BtnAddActEconomicas" ImageUrl="/Imagenes/Iconos/16/contract.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnOtrasConfig" Text="Otras configuraciones"
                                    UniqueName="BtnOtrasConfig" ImageUrl="/Imagenes/Iconos/16/money_add.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddCalendario" Text="Calendario tributario del municipio"
                                    UniqueName="BtnAddCalendario" ImageUrl="/Imagenes/Iconos/16/calendar.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddAuto" Text="Autoretenciones del municipio"
                                    UniqueName="BtnAddAuto" ImageUrl="/Imagenes/Iconos/16/money_dollar.png">
                                </telerik:GridButtonColumn>
                                <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddDescuento" Text="Configurar descuentos pronto pago"
                                    UniqueName="BtnAddDescuento" ImageUrl="/Imagenes/Iconos/16/img_comportamiento.png">
                                </telerik:GridButtonColumn>--%>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddBanco" Text="Registrar bancos al municipio"
                                    UniqueName="BtnAddBanco" ImageUrl="/Imagenes/Iconos/16/img_asociar.png">
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogType="RadWindow"
                                    ConfirmText="¿Se Encuentra Seguro de Eliminar el registro Seleccionado ...!"
                                    ConfirmTitle="Eliminar Municipio" Text="Eliminar" UniqueName="DeleteCommand">
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
