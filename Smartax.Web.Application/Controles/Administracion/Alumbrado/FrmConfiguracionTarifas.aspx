<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmConfiguracionTarifas.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Alumbrado.FrmConfiguracionTarifas" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
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
    </script>
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
            <script>
                function alertMsj(msj) {
                    alert(msj);
                }
            </script>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:Panel ID="Panel1" runat="server" BorderStyle="None">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 1150px;">
                        <tr>
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="HOJA DE CONFIGURACIÓN DE TARIFAS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="lblIdColumna" runat="server" Visible="false"></telerik:RadLabel>
                                <telerik:RadGrid ID="RadGrid12" runat="server" AllowSorting="True" AllowFilteringByColumn="True" Visible="true"
                                    AutoGenerateColumns="False" AllowPaging="true" PageSize="20" Skin="Default"
                                    OnNeedDataSource="RadGrid12_NeedDataSource"
                                    OnItemCommand="RadGrid1_ItemCommand"
                                    OnItemCreated="RadGrid12_ItemCreated"
                                    OnPageIndexChanged="RadGrid12_PageIndexChanged"
                                    OnInsertCommand="RadGrid12_InsertCommand"
                                    OnDeleteCommand="RadGrid12_DeleteCommand"
                                    GridLines="None">
                                    <MasterTableView runat="server" CommandItemDisplay="Top" DataKeyNames="id" EditMode="PopUp" Name="DtTarifas" NoMasterRecordsText="No hay Registros para Mostrar">

                                        <CommandItemTemplate>
                                            <asp:LinkButton ID="LnkAddNew" runat="server" CommandName="InitInsert" ToolTip="Agregar nuevo registro"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_add.png"/> NUEVO REGISTRO</asp:LinkButton>
                                            <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                                        </CommandItemTemplate>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id" HeaderText="Id"
                                                UniqueName="id" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="clasificacion" HeaderText="Clasificacion"
                                                UniqueName="clasificacion" EmptyDataText="">
                                            </telerik:GridBoundColumn>




                                            <%--> 
                                <telerik:GridTemplateColumn DataField="tipo" HeaderText="Tipo" Visible="false" UniqueName="tipo" >
                                    <EditItemTemplate>
                                        <telerik:RadDropDownList ID="RadRadioButtonListTipo" runat="server">
                                            <Items>
                                                <telerik:DropDownListItem Text="UVT" Value="0" Selected="true" />
                                                <telerik:DropDownListItem Text="SMMLV" Value="1" />
                                                <telerik:DropDownListItem Text="SMDLV" Value="2" />
                                                <telerik:DropDownListItem Text="VABS" Value="3" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>

                                                <--%>


                                            <telerik:GridBoundColumn DataField="consumokwh" HeaderText="Consumo KW/H (rango)"
                                                UniqueName="consumokwh" ReadOnly="true" EmptyDataText="">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridNumericColumn DecimalDigits="0" MinValue="0" MaxValue="999999999" DataField="min_kw" HeaderText="Consumo KW/H Inicial"
                                                UniqueName="min_kw" Visible="false" EmptyDataText="">
                                            </telerik:GridNumericColumn>
                                            <telerik:GridNumericColumn DecimalDigits="0" DataField="max_kw"  MinValue="0" MaxValue="999999999" HeaderText="Consumo KW/H Final"
                                                UniqueName="max_kw" Visible="false" EmptyDataText="">
                                            </telerik:GridNumericColumn>
                                            <telerik:GridNumericColumn DataField="consumo" MaxValue="100" MinValue="0" DecimalDigits="2" DataType="System.Decimal" HeaderText="% Consumo KW/H"
                                                UniqueName="consumo"  EmptyDataText="">
                                            </telerik:GridNumericColumn>
                                            <telerik:GridNumericColumn DataField="tarifa_minima" MinValue="0" MaxValue="999999999" DecimalDigits="2" HeaderText="Tarifa Mínima en UVT"
                                                UniqueName="tarifa_minima"  EmptyDataText="">
                                            </telerik:GridNumericColumn>
                                            <telerik:GridNumericColumn DataField="tarifa_maxima" MinValue="0" MaxValue="999999999" DecimalDigits="2" HeaderText="Tarifa Máxima en UVT"
                                                UniqueName="tarifa_maxima"  EmptyDataText="">
                                            </telerik:GridNumericColumn>
                                            <telerik:GridDropDownColumn DataField="tipo" DataSourceID="DatosTipoSectorEspecial"
                                                HeaderText="Tipo" ListDataMember="DtTiposSectoresEspecial" ListTextField="descripcion_tipo_especial"
                                                ListValueField="idtipo_especial" UniqueName="tipo" Visible="false">
                                            </telerik:GridDropDownColumn>


                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteCommand"
                                                ConfirmDialogType="RadWindow"
                                                ConfirmText="¿Esta Seguro de Eliminar el registro Seleccionado de la Lista ....?"
                                                ConfirmTitle="Eliminar Registro" Text="Eliminar Registro">
                                            </telerik:GridButtonColumn>
                                        </Columns>









                                        <EditFormSettings PopUpSettings-Modal="true" CaptionDataField="id" CaptionFormatString="Editar Registro: {0}"
                                            InsertCaption="Agregar Nuevo Registro">
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
                                <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" Height="40px" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="200px" />
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
        </div>
    </form>
</body>
</html>
