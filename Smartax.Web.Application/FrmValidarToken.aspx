<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmValidarToken.aspx.cs" Inherits="Smartax.Web.Application.FrmValidarToken" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%= Smartax.Web.Application.Clases.Seguridad.FixedData.PlatformName + " " + Smartax.Web.Application.Clases.Seguridad.FixedData.PlatformVersion %></title>
    <link rel="shortcut icon" type="image/x-icon" href="~/Imagenes/Iconos/16/favicon.png">
    <link rel="stylesheet" href="style/style1.css" media="screen" type="text/css" />
    <link rel='stylesheet' href='http://codepen.io/assets/libs/fullpage/jquery-ui.css'>
    <script src='http://codepen.io/assets/libs/fullpage/jquery_and_jqueryui.js'></script>
    <script src='https://www.google.com/recaptcha/api.js?hl=es'></script>
    </head>
<body>
    <div class="login-card">
        <div class="login-help">
            <asp:Image ID="Image4" runat="server" ImageUrl="~/Imagenes/General/img_smartax.png" Height="80px" />
        </div>
        <form id="form1" runat="server">
            <telerik:RadScriptManager ID="ScriptManager1" runat="server" EnableTheming="True">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                    </asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                    </asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                    </asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            </telerik:RadAjaxManager>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" HorizontalAlign="NotSet">
                <asp:Panel ID="Panel1" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td align="center">
                                <h1>Validación de Token</h1>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblTipoEnvio" runat="server" Font-Size="13pt">Medio de Envio del Token</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblMedioEnviado" runat="server" Font-Bold="True" Font-Size="15pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblNumeroToken" runat="server" Font-Size="13pt">Número de Token</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <telerik:RadNumericTextBox ID="TxtNumeroToken" Runat="server" DataType="System.Int32" Height="40px" MaxValue="999999" Width="200px" Enabled="False" Font-Size="25pt" MinValue="100000" TabIndex="1">
                                    <NegativeStyle Resize="None" />
                                    <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle Resize="None" />
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="Validator2" runat="server" ControlToValidate="TxtNumeroToken" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarCampos"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validator2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validator2">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:LinkButton ID="LnkGenerarToken" runat="server" OnClick="LnkGenerarToken_Click" TabIndex="2">Generar nuevo Token</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="LblTipoEnvio0" runat="server" CssClass="FormLabels" Font-Bold="True" Font-Size="12pt" ForeColor="#CC0000">Importante:</asp:Label>
                                <asp:Label ID="LblLeyenda" runat="server" Font-Size="9pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="BtnValidarToken" runat="server" class="login login-submit" name="login" OnClick="BtnValidarToken_Click" Text="Validar Token" ValidationGroup="ValidarCampos" Width="200px" TabIndex="3" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="login-help">
                                    <a href="../Default.aspx">Iniciar Sesión</a>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
            </telerik:RadAjaxPanel>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="30">
                <div class="loading">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/General/loading.gif" Width="150px" Height="150px" />
                    <h3>Un momento por favor ¡Conectando! ...
                    </h3>
                </div>
            </telerik:RadAjaxLoadingPanel>
        </form>
    </div>
</body>
</html>
