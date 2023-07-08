<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmConfiguracionCuentas.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Formatos.FrmConfiguracionCuentas" %>

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
    </script>
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:Panel ID="Panel1" runat="server" BorderStyle="None">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 1150px;">
                        <tr>
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="HOJA DE CONFIGURACIÓN DE CUENTAS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="lblIdColumna" runat="server" Visible="false"></telerik:RadLabel>
                                <telerik:RadGrid ID="RadGrid12" runat="server" AllowSorting="True" AllowFilteringByColumn="True" Visible="true"
                                    AutoGenerateColumns="False" AllowPaging="true" PageSize="20" Skin="Default"
                                    OnNeedDataSource="RadGrid12_NeedDataSource"
                                    OnPageIndexChanged="RadGrid12_PageIndexChanged"
                                    OnInsertCommand="RadGrid12_InsertCommand"
                                    OnDeleteCommand="RadGrid12_DeleteCommand"
                                    GridLines="None">
                                    <MasterTableView runat="server" CommandItemDisplay="Top" DataKeyNames="id_cuentas_columnas_f321" EditMode="PopUp" Name="DtCuentas" NoMasterRecordsText="No hay Registros para Mostrar">

                                        <CommandItemTemplate>
                                            <asp:LinkButton ID="LnkAddNew" runat="server" CommandName="InitInsert" ToolTip="Agregar nuevo registro"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_add.png"/> NUEVO REGISTRO</asp:LinkButton>
                                        </CommandItemTemplate>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id_cuentas_columnas_f321" HeaderText="Id"
                                                UniqueName="id_cuentas_columnas_f321" ReadOnly="true" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="cod_cuenta" HeaderText="Codigo Cuenta"
                                                UniqueName="cod_cuenta" ReadOnly="true" EmptyDataText="">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_cuenta" HeaderText="Cuenta"
                                                UniqueName="nombre_cuenta" ReadOnly="true" EmptyDataText="">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridDropDownColumn DataField="id_cuenta" DataSourceID="DatosCuenta"
                                                HeaderText="Cuenta" ListDataMember="DatosCuenta" ListTextField="nombre_cuenta"
                                                ListValueField="id_cuenta" UniqueName="id_cuenta" Visible="false">
                                            </telerik:GridDropDownColumn>

                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteCommand"
                                                ConfirmDialogType="RadWindow"
                                                ConfirmText="¿Esta Seguro de Eliminar el registro Seleccionado de la Lista ....?"
                                                ConfirmTitle="Eliminar Registro" Text="Eliminar Registro">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                        <EditFormSettings PopUpSettings-Modal="true" CaptionDataField="id_param_f321_f525" CaptionFormatString="Editar Registro: {0}"
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
