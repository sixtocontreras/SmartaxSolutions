<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddUsersEjecucionXLote.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.EjecucionLote.FrmAddUsersEjecucionXLote" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <style type="text/css">
        .auto-style1 {
            height: 28px;
        }
    </style>
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
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
            <div>
                <asp:Panel ID="Panel1" runat="server" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="3">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="SubTitle" Text="ASIGNAR FILTROS DE EJECUCIÓN POR DEPARTAMENTOS Y MUNICIPIOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">LISTADO DEL PLAN UNICO DE CUENTA</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1" colspan="3">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="5"
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged"
                                    OnItemCommand="RadGrid1_ItemCommand">
                                    <MasterTableView DataKeyNames="id_usuario, id_estado" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id_usuario" EmptyDataText="" FilterControlWidth="40px"
                                                HeaderText="Id" ReadOnly="True" UniqueName="id_usuario">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_usuario" HeaderText="Nombre Usuario"
                                                UniqueName="nombre_usuario" FilterControlWidth="100px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_rol" HeaderText="Perfil usuario"
                                                UniqueName="nombre_rol" FilterControlWidth="70px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                                UniqueName="codigo_estado">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAsignar" Text="Asignar Cuenta"
                                                UniqueName="BtnAsignar" ImageUrl="/Imagenes/Iconos/16/img_add.png">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="LblTitulo2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt">LISTA DE CUENTAS ASIGNADAS x IMPUESTO</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <telerik:RadGrid ID="RadGrid2" runat="server" AllowFilteringByColumn="True" AllowPaging="True"
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None" PageSize="5"
                                    OnItemCommand="RadGrid2_ItemCommand"
                                    OnNeedDataSource="RadGrid2_NeedDataSource"
                                    OnPageIndexChanged="RadGrid2_PageIndexChanged">
                                    <MasterTableView CommandItemDisplay="Top" EditMode="PopUp" DataKeyNames="idejec_lote_usuario, id_usuario, id_estado" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <EditFormSettings CaptionDataField="idejec_lote_usuario"
                                            CaptionFormatString="Id Registro: {0}">
                                            <EditColumn UniqueName="EditCommandColumn1">
                                            </EditColumn>
                                            <FormTemplate>
                                            </FormTemplate>
                                            <PopUpSettings Modal="True" />
                                        </EditFormSettings>
                                        <CommandItemTemplate>
                                            <asp:LinkButton ID="LnkLogsAuditoria" runat="server" CommandName="BtnLogsAuditoria" ToolTip="Ver los logs de auditoria"><img style="border:0px;vertical-align:middle;" alt="" src="../../../Imagenes/Iconos/16/img_info.png"/> LOGS DE AUDITORIA</asp:LinkButton>
                                        </CommandItemTemplate>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idejec_lote_usuario" EmptyDataText="" FilterControlWidth="40px"
                                                HeaderText="Id" UniqueName="idejec_lote_usuario" ReadOnly="True">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_usuario" HeaderText="Nombre Usuario"
                                                UniqueName="nombre_usuario" ReadOnly="True" FilterControlWidth="100px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                                UniqueName="codigo_estado" ReadOnly="True" FilterControlWidth="60px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="F. Registro"
                                                UniqueName="fecha_registro" ReadOnly="True" FilterControlWidth="70px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_actualizacion" HeaderText="F. Actualización"
                                                UniqueName="fecha_actualizacion" ReadOnly="True" FilterControlWidth="70px">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBloquer" Text="Bloquear/DesBloquear"
                                                UniqueName="BtnBloquer" ImageUrl="/Imagenes/Iconos/16/img_block.png">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnDisociar" Text="Desasociar el Usuario"
                                                UniqueName="BtnDisociar" ImageUrl="/Imagenes/Iconos/16/error.png">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Button ID="BtnSalir0" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
            </telerik:RadWindowManager>
        </telerik:RadAjaxPanel>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
            <div class="loading">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" Height="150px" />
                <h3>Espere un momento por favor ...
                </h3>
            </div>
        </telerik:RadAjaxLoadingPanel>
    </form>
</body>
</html>
