<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmResetPassword.aspx.cs" Inherits="Smartax.Web.Application.FrmResetPassword" %>
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
                            <td colspan="3" align="center">
                                <h1>Resetear Password</h1>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:TextBox ID="TxtIdentificacion" runat="server" placeholder="No. Documento" AutoCompleteType="Disabled" MaxLength="100" Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Validator1" runat="server" ControlToValidate="TxtIdentificacion" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarCampos"></asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="Validator1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validator1">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:TextBox ID="TxtEmail" runat="server" AutoCompleteType="Disabled" MaxLength="100" placeholder="Email" Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Validator2" runat="server" ControlToValidate="TxtEmail" Display="None" ErrorMessage="Campo requerido !" SetFocusOnError="True" ValidationGroup="ValidarCampos"></asp:RequiredFieldValidator>
                                <cc1:ValidatorCalloutExtender ID="Validator2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validator2">
                                </cc1:ValidatorCalloutExtender>
                                <asp:RegularExpressionValidator ID="ValidadorEmail" runat="server" ControlToValidate="TxtEmail" Display="None" ErrorMessage="&lt;b&gt;Señor usuario&lt;/b&gt;&lt;br/&gt;Email no valido&lt;br/&gt;" SetFocusOnError="True" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" ValidationGroup="ValidarConsulta"></asp:RegularExpressionValidator>
                                <cc1:ValidatorCalloutExtender ID="ValidadorEmail_ValidatorCalloutExtender" runat="server" BehaviorID="RegularExpressionValidator1_ValidatorCalloutExtender" TargetControlID="ValidadorEmail">
                                </cc1:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:Button ID="BtnResetPass" runat="server" class="login login-submit" name="login" Text="Resetear Password" ValidationGroup="ValidarCampos" Width="200px" OnClick="BtnResetPass_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="login-help">
                                    <a aria-setsize="10" href="../Default.aspx">Iniciar Sesi&oacute;n</a>
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
