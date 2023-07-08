<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmModulosRol.aspx.cs" Inherits="Smartax.Web.Application.Controles.Seguridad.FrmModulosRol" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" HorizontalAlign="NotSet" Width="100%">
                <asp:Panel ID="PanelNavegacion" runat="server" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;" border="0">
                        <tr>
                            <td align="center">
                                <asp:Panel ID="PanelPermisos" runat="server" DefaultButton="BtnGuardar" Style="text-align: center" Width="100%">
                                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;" border="1">
                                        <tr>
                                            <td colspan="6" bgcolor="#999999">
                                                <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="ASIGNAR MODULOS DEL SISTEMA AL ROL" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="ChkSeleccionarTodos" runat="server" AutoPostBack="True" Font-Size="16pt" OnCheckedChanged="ChkSeleccionarTodos_CheckedChanged" Text="Seleccionar todos" ToolTip="Marcar todos las opciones" />
                                            </td>
                                            <td colspan="3">
                                                <asp:CheckBox ID="ChkQuitarTodos" runat="server" AutoPostBack="True" Font-Size="16pt" OnCheckedChanged="ChkQuitarTodos_CheckedChanged" Text="Quitar selección" ToolTip="Desmarcar todas las opciones" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblRol0" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Planeación Fiscal</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol1" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Calendario Trib.</asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="LblRol2" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Tarifas Excesivas</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol3" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Campo Futuro</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol4" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Campo Futuro</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkPlaneacionFiscal" runat="server" Font-Size="16pt" ToolTip="Permite ver las opciones de este modulo" AutoPostBack="True" OnCheckedChanged="ChkPlaneacionFiscal_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkCalendarioTrib" runat="server" Font-Size="16pt" ToolTip="Permite ver el modulo del calendario tributario" Enabled="False" />
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="ChkTarifasExcesivas" runat="server" Font-Size="16pt" ToolTip="Permite ver el modulo de tarifas excesivas" Enabled="False" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkCampoFuturo1" runat="server" Font-Size="16pt" ToolTip="Permite borrar información del sistema" Enabled="False" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkCampoFuturo2" runat="server" Font-Size="16pt" ToolTip="Permite bloquear registros en el sistema" Enabled="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblRol17" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Info. Tributaría</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol21" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Hoja Trabajo</asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="LblRol27" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Declaración Def.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol26" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Ejecución por Lote</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol25" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Validar Liq. Lote</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkInfoTributaria" runat="server" Font-Size="16pt" ToolTip="Permite ver las opciones de este modulo" AutoPostBack="True" OnCheckedChanged="ChkInfoTributaria_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkHojaTrabajo" runat="server" Font-Size="16pt" ToolTip="Permite ver el modulo para la hoja de trabajo" Enabled="False" />
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="ChkDeclaracionDef" runat="server" Font-Size="16pt" ToolTip="Permite ver el modulo declaración definitiva" Enabled="False" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkEjecucionLote" runat="server" Enabled="False" Font-Size="16pt" ToolTip="Permite realizar el proceso de ejecución por lote" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkValidarLiqLote" runat="server" Enabled="False" Font-Size="16pt" ToolTip="Permite validar las liquidaciones por lote" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblRol33" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Consultar Liquid.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol34" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Ficha Técnica</asp:Label>
                                            </td>
                                            <td colspan="2">&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkConsultarLiq" runat="server" Enabled="False" Font-Size="16pt" ToolTip="Permite consultar liquidación de impuestos" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkFichaTecnica" runat="server" Enabled="False" Font-Size="16pt" ToolTip="Permite ver la ficha tecnica de municipios" />
                                            </td>
                                            <td colspan="2">&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblRol35" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Formatos SFC</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol36" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Generación de Datos</asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="LblRol37" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Generar F-321</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol38" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Generar F-525</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol39" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black" ToolTip="Generar archivo plano">Generar Arc. PLano</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkFormatosSfc" runat="server" AutoPostBack="True" Font-Size="16pt" OnCheckedChanged="ChkFormatosSfc_CheckedChanged" ToolTip="modulo para generar los diferentes formatos" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkGeneracionDatos" runat="server" Enabled="False" Font-Size="16pt" ToolTip="Permite generar datos de formatos" />
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="ChkGenerarF321" runat="server" Enabled="False" Font-Size="16pt" ToolTip="Permite generar el formato 321" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkGenerarF525" runat="server" Enabled="False" Font-Size="16pt" ToolTip="Permite generar el formato 525" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkGenerarArcPlanos" runat="server" Enabled="False" Font-Size="16pt" ToolTip="Permite generar archivos planos" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblRol40" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Normatividad</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol41" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Cargar Normatividad</asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="LblRol42" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black" ToolTip="Consultar normatividad">Cons. Normatividad</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol43" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black" ToolTip="Carga masiva de documentos">Carga Masiva Doc.</asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkNormatividad" runat="server" AutoPostBack="True" Font-Size="16pt" OnCheckedChanged="ChkNormatividad_CheckedChanged" ToolTip="modulo para las normatividades" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkCargarNormatividad" runat="server" Enabled="False" Font-Size="16pt" ToolTip="Permite cargar la normatividad" />
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="ChkConsNormatividad" runat="server" Enabled="False" Font-Size="16pt" ToolTip="Permite consultar la normatividad" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkCargaMasNormatividad" runat="server" Enabled="False" Font-Size="16pt" ToolTip="Permite realizar la carga masiva de la normatividad" />
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblRol28" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Control Actividades</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol29" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Mis Actividades</asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="LblRol30" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Monitoreo Actividades</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol31" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Estadistica Activ.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LblRol32" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="14pt" ForeColor="Black">Estadistica Liq.</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="ChkControlActividades" runat="server" AutoPostBack="True" Font-Size="16pt" OnCheckedChanged="ChkControlActividades_CheckedChanged" ToolTip="Permite ver las estadisticas de actividades y liquidación de impuestos" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkMisActividades" runat="server" Font-Size="16pt" ToolTip="Permite visualizar las actividades del usuario conectado" Enabled="False" />
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="ChkMonitoreoAct" runat="server" Font-Size="16pt" ToolTip="Permite ver el monitoreo de actividades asignadas" Enabled="False" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkEstadisticaAct" runat="server" Font-Size="16pt" ToolTip="Permite ver las estadisticas de actividades abiertas y cerradas" Enabled="False" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkEstadisticaLiq" runat="server" Font-Size="16pt" ToolTip="Permite ver las estadisticas de la Liquidación de impuestos" Enabled="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Label ID="LblIdRol" runat="server" CssClass="FormLabels" Font-Size="14pt" ForeColor="#990000" Visible="False"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="LblIdModulo" runat="server" CssClass="FormLabels" Font-Size="14pt" ForeColor="#990000" Visible="False"></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <%-- <tr>
                                        <td colspan="5">
                                            <asp:Button ID="BtnAceptar" runat="server" Font-Bold="True" Font-Size="14pt" Text="Aceptar" Width="120px" OnClick="BtnAceptar_Click" ToolTip="Click para guardar cambios del permiso" />
                                            &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" Width="120px" ToolTip="Click para salir" />
                                        </td>
                                    </tr>--%>
                                        <tr>
                                            <td colspan="6">
                                                <asp:Button ID="BtnGuardar" runat="server" Font-Bold="True" Font-Size="12pt" Height="35px" OnClick="BtnGuardar_Click" Text="Guardar" ToolTip="Click para guardar cambios del permiso" Width="150px" />
                                                &nbsp;<asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="12pt" Height="35px" OnClientClick="window.close()" Text="Salir" ToolTip="Click para salir" Width="150px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
            </telerik:RadAjaxPanel>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
                <div class="loading">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" />
                </div>
            </telerik:RadAjaxLoadingPanel>
        </div>
    </form>
</body>
</html>
