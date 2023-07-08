<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAddFirma.aspx.cs" Inherits="Smartax.Web.Application.Controles.Administracion.Clientes.FrmAddFirma" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server"></telerik:RadAjaxManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" EnableAJAX="False" HorizontalAlign="NotSet">
                <asp:Panel ID="Panel1" runat="server" Width="100%">
                    <table cellpadding="4" cellspacing="0" class="Tab" style="width: 100%;">
                        <tr>
                            <td align="center" bgcolor="#999999" colspan="7">
                                <asp:Label ID="LblTitulo" runat="server" CssClass="SubTitle" Text="CARGAR IMAGEN DE FIRMA" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label13" runat="server" Text="Imagen:"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:FileUpload ID="FileExaminar" runat="server" Width="400px" />
                            </td>
                            <td align="right">&nbsp;</td>
                            <td align="left">&nbsp;</td>
                            <td align="right">&nbsp;</td>
                            <td align="left">
                                <asp:Button ID="BtnAdicionar" runat="server" Font-Bold="True" Font-Size="14pt" OnClick="BtnAdicionar_Click" Text="Adicionar" ToolTip="Click para agregar la foto" Width="120px" />
                            </td>
                            <td align="left">
                                <asp:Button ID="BtnSalir" runat="server" Font-Bold="True" Font-Size="14pt" OnClientClick="window.close()" Text="Salir" ToolTip="Salir" Width="120px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="auto-style1" colspan="7">
                                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="2"
                                    AllowSorting="True" AutoGenerateColumns="False" GridLines="None"
                                    OnNeedDataSource="RadGrid1_NeedDataSource"
                                    OnItemCommand="RadGrid1_ItemCommand"
                                    OnPageIndexChanged="RadGrid1_PageIndexChanged">
                                    <MasterTableView DataKeyNames="id_firmante, nombre_archivo" Name="Grilla" NoMasterRecordsText="No hay fotografías para Mostrar">
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="ColumnID" HeaderText="Id">
                                                <ItemTemplate>
                                                    <table id="Table2" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td width="100%">
                                                                <%# DataBinder.Eval(Container.DataItem, "id_firmante") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ColumnAcompanante" HeaderText="Descripción">
                                                <ItemTemplate>
                                                    <table id="Table2" cellspacing="1" cellpadding="1" width="300">
                                                        <tr>
                                                            <td width="100%">
                                                                <asp:Label ID="Label1" runat="server" ForeColor="Black" Font-Bold="true" Text="NOMBRE: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_firmante") %>
                                                                <br />
                                                                <asp:Label ID="Label2" runat="server" ForeColor="Black" Font-Bold="true" Text="ROL: "></asp:Label>
                                                                <%# DataBinder.Eval(Container.DataItem, "nombre_rol") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ColumnImg" HeaderText="Imagen">
                                                <ItemTemplate>
                                                    <table id="Table2" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td width="50%">
                                                                <telerik:RadBinaryImage ID="RadBinaryImage1" runat="server" DataValue='<%# DataBinder.Eval(Container.DataItem, "imagen_firma") %>' Height="100px" Width="100px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ColumnEliminar" HeaderText="Eliminar">
                                                <ItemTemplate>
                                                    <table id="Table2" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td width="50%">
                                                                <asp:ImageButton ID="BtnEliminar" runat="server" CausesValidation="False"
                                                                    CommandName="BtnEliminar" CommandArgument='<%# Eval ( "imagen_firma" ) %>'
                                                                    ImageUrl="~/Imagenes/Iconos/32/img_cancel.png" Text="Elimnar imagen de la Firma"
                                                                    ToolTip="Click para eliminar la imagen" Style="padding: 3px 3px !important;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="7">
                                <asp:Label ID="LblMensaje" runat="server" CssClass="FormLabels" Font-Size="14pt" ForeColor="#990000"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="7">
                                &nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
            </telerik:RadAjaxPanel>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="" Transparency="30">
                <div class="loading">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" Height="150px" />
                    <h3>Espere un momento por favor ...
                    </h3>
                </div>
            </telerik:RadAjaxLoadingPanel>
        </div>
    </form>
</body>
</html>
