<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmRolActividades.aspx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.FrmRolActividades" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
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
                            <td align="center" bgcolor="#999999">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="SubTitle" Text="ASIGNAR CONTROL DE ACTIVIDADES AL ROL" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblTitulo1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">CONTROL DE ACTIVIDADES SIN ASIGNAR</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" OnItemCommand="RadGrid1_ItemCommand" OnNeedDataSource="RadGrid1_NeedDataSource" OnPageIndexChanged="RadGrid1_PageIndexChanged" PageSize="5">
                                    <MasterTableView DataKeyNames="idcontrol_actividad, id_cliente" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="idcontrol_actividad" EmptyDataText="" FilterControlWidth="40px"
                                                HeaderText="Id" ReadOnly="True" UniqueName="idcontrol_actividad">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_cliente" HeaderText="Cliente"
                                                UniqueName="nombre_cliente" FilterControlWidth="100px">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="descripcion_actividad" HeaderText="Descripción Actividad"
                                                UniqueName="descripcion_actividad" FilterControlWidth="140px">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                                UniqueName="codigo_estado">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAsignar" Text="Asignar Actividad"
                                                UniqueName="BtnAsignar" ImageUrl="/Imagenes/Iconos/16/img_add.png">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblTitulo2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt">CONTROL DE ACTIVIDADES ASIGNADAS</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <telerik:RadGrid ID="RadGrid2" runat="server" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                                            GridLines="None" PageSize="5"
                                            OnItemCommand="RadGrid2_ItemCommand"
                                            OnNeedDataSource="RadGrid2_NeedDataSource"
                                            OnPageIndexChanged="RadGrid2_PageIndexChanged"
                                            OnUpdateCommand="RadGrid2_UpdateCommand">
                                            <MasterTableView EditMode="PopUp" CommandItemDisplay="Top" DataKeyNames="idcontrol_activ_rol, idcontrol_actividad, id_estado" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="idcontrol_activ_rol" EmptyDataText="" HeaderText="Id"
                                                        UniqueName="idcontrol_activ_rol" FilterControlWidth="40px" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="nombre_cliente" HeaderText="Cliente"
                                                        UniqueName="nombre_cliente" ReadOnly="true" FilterControlWidth="100px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="descripcion_actividad" HeaderText="Descripcion Actividad"
                                                        UniqueName="descripcion_actividad" ReadOnly="true" FilterControlWidth="120px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridDateTimeColumn DataField="fecha_inicial" HeaderText="Fecha Inicial"
                                                        UniqueName="fecha_inicial" FilterControlWidth="60px">
                                                    </telerik:GridDateTimeColumn>
                                                    <telerik:GridDateTimeColumn DataField="fecha_final" HeaderText="Fecha Final"
                                                        UniqueName="fecha_final" FilterControlWidth="60px">
                                                    </telerik:GridDateTimeColumn>
                                                    <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                                        UniqueName="codigo_estado" ReadOnly="true">
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" ButtonType="ImageButton"
                                                        InsertText="Insertar" UpdateText="Actualizar">
                                                    </telerik:GridEditCommandColumn>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnBloquer" Text="Bloquear/DesBloquear"
                                                        UniqueName="BtnBloquer" ImageUrl="/Imagenes/Iconos/16/img_block.png">
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnDisociar" Text="Desasociar la Cuenta"
                                                        UniqueName="BtnDisociar" ImageUrl="/Imagenes/Iconos/16/error.png">
                                                    </telerik:GridButtonColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
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
                <h3>Espere un momento por favor ...</h3>
            </div>
        </telerik:RadAjaxLoadingPanel>

    </form>
</body>
</html>
