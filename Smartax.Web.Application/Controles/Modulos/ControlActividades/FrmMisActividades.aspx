<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMisActividades.aspx.cs" Inherits="Smartax.Web.Application.Controles.Modulos.ControlActividades.FrmMisActividades" %>
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
                            <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="GESTIONAR MIS ACTIVIDADES " Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                                AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                                OnNeedDataSource="RadGrid1_NeedDataSource"
                                OnItemCommand="RadGrid1_ItemCommand"
                                OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                <MasterTableView DataKeyNames="idcontrol_activ_rol, idcontrol_actividad, id_estado" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="idcontrol_activ_rol" EmptyDataText="" FilterControlWidth="40px"
                                            HeaderText="Id" ReadOnly="True" UniqueName="idcontrol_activ_rol">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion_actividad" HeaderText="Actividad"
                                            UniqueName="descripcion_actividad" FilterControlWidth="200px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="fecha_inicial" HeaderText="Fecha Inicial"
                                            UniqueName="fecha_inicial" FilterControlWidth="60px">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridBoundColumn DataField="fecha_final" HeaderText="Fecha Final"
                                            UniqueName="fecha_final" FilterControlWidth="60px">
                                        </telerik:GridBoundColumn>--%>

                                        <telerik:GridBoundColumn DataField="fecha_final" EmptyDataText="" HeaderText="Fecha Final"
                                            UniqueName="fecha_final" ReadOnly="true" FooterText="TOTAL %:" FilterControlWidth="60px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridNumericColumn DataField="porc_cumplimiento" DataType="System.Double" HeaderText="% Cumplimiento" FilterControlWidth="80px"
                                            SortExpression="porc_cumplimiento" UniqueName="porc_cumplimiento" Aggregate="Sum" DataFormatString="{0:#,##0}" FooterAggregateFormatString="{0:C0}">
                                        </telerik:GridNumericColumn>

                                        <telerik:GridBoundColumn DataField="fecha_cierre" HeaderText="Fecha Cierre"
                                            UniqueName="fecha_cierre" FilterControlWidth="60px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="estado_actividad" HeaderText="Estado"
                                            UniqueName="estado_actividad" FilterControlWidth="60px">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnCerrarAct" Text="Cerrar esta actividad"
                                            UniqueName="BtnCerrarAct" ImageUrl="/Imagenes/Iconos/16/check.png">
                                        </telerik:GridButtonColumn>

                                        <%--<telerik:GridTemplateColumn UniqueName="aceptar_tarifa">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="ToggleRowSelection"
                                                    AutoPostBack="True" />
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="headerChkbox" runat="server" OnCheckedChanged="ToggleSelectedState"
                                                    AutoPostBack="True" />
                                            </HeaderTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                    </Columns>
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
