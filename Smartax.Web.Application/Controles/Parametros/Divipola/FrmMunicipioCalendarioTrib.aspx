﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMunicipioCalendarioTrib.aspx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.FrmMunicipioCalendarioTrib" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script language="JavaScript">
        var msg = "¡El botón derecho está desactivado para este sitio !";
        function disableIE() {
            if (document.all) {
                //alert(msg);
                return false;
            }
        }

        function disableNS(e) {
            if (navigator.appName == 'Netscape' && (e.which == 3 || e.which == 2)) {
                //alert(msg);
                return false;
            } else if (navigator.appName == 'Microsoft Internet Explorer' && (event.button == 2)) {
                //alert(msg);
                return false;
            }

            //alert('El Navegador es: ' + navigator.appName);
        }

        if (document.layers) {
            document.captureEvents(Event.MOUSEDOWN);
            document.onmousedown = disableNS;
        } else {
            document.onmouseup = disableNS;
            document.oncontextmenu = disableIE;
        }
        document.oncontextmenu = new Function("return false")
        //document.oncontextmenu = new Function("alert(msg);return false")
    </script>
</head>
<body bgcolor="#E6E6E6">
    <form id="form2" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
            <asp:Panel ID="PnlDatos" runat="server" Width="100%" HorizontalAlign="Center">
                <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                    <tr>
                        <td align="center" bgcolor="#999999">
                            <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR CALENDARIO TRIBUTARIO DEL MUNICIPIO" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                                AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                ShowFooter="true" ShowStatusBar="true" AutoGenerateDeleteColumn="false" AllowAutomaticInserts="false"
                                OnNeedDataSource="RadGrid1_NeedDataSource"
                                OnItemCommand="RadGrid1_ItemCommand"
                                OnItemDataBound="RadGrid1_ItemDataBound"
                                OnItemCreated="RadGrid1_ItemCreated"
                                OnPageIndexChanged="RadGrid1_PageIndexChanged"
                                OnInsertCommand="RadGrid1_InsertCommand"
                                OnUpdateCommand="RadGrid1_UpdateCommand"
                                OnDeleteCommand="RadGrid1_DeleteCommand">
                                <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="idmun_calendario_trib" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                    <EditFormSettings CaptionDataField="idmun_calendario_trib"
                                        CaptionFormatString="Editar Registro: {0}"
                                        InsertCaption="Agregar Nuevo Registro">
                                        <EditColumn UniqueName="EditCommandColumn1">
                                        </EditColumn>
                                        <FormTemplate>
                                        </FormTemplate>
                                        <PopUpSettings Modal="True" />
                                    </EditFormSettings>
                                    <CommandItemTemplate>
                                        <asp:LinkButton ID="LnkAddCalendario" runat="server" CommandName="BtnAddCalendario" ToolTip="Registrar info de calendario tributario"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/index_add.png"/> REGISTRAR CALENDARIO</asp:LinkButton>
                                        <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                                        <asp:LinkButton ID="LnkActualizarLista" runat="server" CommandName="BtnActualizarLista" ToolTip="Actualizar lista"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_refresh.png"/> ACTUALIZAR LISTA CALENDARIO</asp:LinkButton>
                                    </CommandItemTemplate>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="idmun_calendario_trib" EmptyDataText="" FilterControlWidth="40px"
                                            HeaderText="Id" ReadOnly="True" UniqueName="idmun_calendario_trib">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="descripcion_formulario" HeaderText="Formulario"
                                            UniqueName="descripcion_formulario" ReadOnly="true" FilterControlWidth="100px">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridDropDownColumn DataField="idformulario_impuesto" DataSourceID="DatosForm"
                                            HeaderText="Formulario" ListDataMember="DtFormularioImpuesto" ListTextField="descripcion_formulario"
                                            ListValueField="idformulario_impuesto" UniqueName="idformulario_impuesto" Visible="false">
                                        </telerik:GridDropDownColumn>--%>

                                        <telerik:GridBoundColumn DataField="periodicidad_impuesto" HeaderText="Periodicidad"
                                            UniqueName="periodicidad_impuesto" ReadOnly="true" FilterControlWidth="50px">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridDropDownColumn DataField="idperiodicidad_impuesto" DataSourceID="DatosPeriodicidad"
                                            HeaderText="Periodicidad" ListDataMember="DtPeriodicidadImpuesto" ListTextField="periodicidad_impuesto"
                                            ListValueField="idperiodicidad_impuesto" UniqueName="idperiodicidad_impuesto" Visible="false">
                                        </telerik:GridDropDownColumn>--%>

                                        <telerik:GridBoundColumn DataField="descripcion_anio" HeaderText="Año Gravable"
                                            UniqueName="descripcion_anio" ReadOnly="true" FilterControlWidth="50px">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridDropDownColumn DataField="id_anio" DataSourceID="DatosAnio"
                                            HeaderText="Año Gravable" ListDataMember="DtAnios" ListTextField="descripcion_anio"
                                            ListValueField="id_anio" UniqueName="id_anio" Visible="false">
                                        </telerik:GridDropDownColumn>--%>

                                        <telerik:GridDateTimeColumn DataField="fecha_limite" HeaderText="Fecha Limite"
                                            UniqueName="fecha_limite" FilterControlWidth="100px">
                                        </telerik:GridDateTimeColumn>
                                        <%--<telerik:GridDateTimeColumn DataField="fecha_final" HeaderText="Fecha Final"
                                            UniqueName="fecha_final">
                                        </telerik:GridDateTimeColumn>--%>

                                        <telerik:GridBoundColumn DataField="valor_descuento1" HeaderText="% Descuento"
                                            UniqueName="valor_descuento1" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridNumericColumn DataField="valor_descuento" HeaderText="% Descuento" FilterControlWidth="70px"
                                            UniqueName="valor_descuento" DataType="System.Double" NumericType="Number" Visible="false">
                                        </telerik:GridNumericColumn>

                                        <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                            UniqueName="codigo_estado" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridDropDownColumn DataField="id_estado" DataSourceID="DatosEstado"
                                            HeaderText="Estado" ListDataMember="DtEstados" ListTextField="codigo_estado"
                                            ListValueField="id_estado" UniqueName="id_estado" Visible="false">
                                        </telerik:GridDropDownColumn>--%>

                                        <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha registro"
                                            UniqueName="fecha_registro" ReadOnly="true" FilterControlWidth="120px">
                                        </telerik:GridBoundColumn>

                                        <%--<telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" ButtonType="ImageButton"
                                            InsertText="Insertar" UpdateText="Actualizar">
                                        </telerik:GridEditCommandColumn>--%>

                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnEditar" Text="Editar informacion"
                                            UniqueName="BtnEditar" ImageUrl="/Imagenes/Iconos/16/edit.png">
                                        </telerik:GridButtonColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogType="RadWindow"
                                            ConfirmText="¿Se Encuentra Seguro de Eliminar el registro Seleccionado ...!"
                                            ConfirmTitle="Eliminar Impuesto" Text="Eliminar" UniqueName="DeleteCommand">
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
                    <tr>
                        <td align="center">
                            <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
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
    </form>
</body>
</html>
