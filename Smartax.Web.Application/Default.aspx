<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Smartax.Web.Application.Default" %>

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
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server">
                            <table style="width: 100%;">
                                <tr>
                                    <td align="center">
                                        <h1>Identificación de Usuarios</h1>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="TxtUsuario" runat="server" placeholder="Usuario" AutoCompleteType="Disabled" MaxLength="15" Width="300px" Font-Size="13pt"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Validator1" runat="server" ControlToValidate="TxtUsuario" Display="None" ErrorMessage="El usuario es requerido !" SetFocusOnError="True" ValidationGroup="ValidarCampos"></asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender ID="Validator1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="Validator1">
                                        </ajaxToolkit:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="TxtPassword" runat="server" placeholder="Password" TextMode="Password" AutoCompleteType="Disabled" MaxLength="16" Width="300px" Font-Size="13pt"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Validator2" runat="server" ControlToValidate="TxtPassword" Display="None" ErrorMessage="El password es requerido !" SetFocusOnError="True" ValidationGroup="ValidarCampos"></asp:RequiredFieldValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender ID="Validator2_ValidatorCalloutExtender" runat="server" BehaviorID="Validator2_ValidatorCalloutExtender" TargetControlID="Validator2">
                                        </ajaxToolkit:ValidatorCalloutExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                        <div class="g-recaptcha" data-sitekey="<%= Smartax.Web.Application.Clases.Seguridad.FixedData.GoogleRecaptchaSiteKey %>"></div>
                                        <%--<div class="g-recaptcha" data-sitekey="6LfsSLcUAAAAALZJpGK2KGMUd6GUSXC7kQZE0oCt"></div>--%>
                                        <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="pnlErrorMsg" Visible="false" runat="server">
                                            <div class="alert alert-danger alert-login">
                                                <asp:Label ID="lblError" runat="server"></asp:Label>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="BtnLogin" runat="server" class="login login-submit" name="login" Text="Ingresar" ValidationGroup="ValidarCampos" OnClick="BtnLogin_Click" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="login-help">
                                            <a href="../FrmResetPassword.aspx">Olvide mi contraseña</a>
                                            <%--<a href="#">Register</a> • <a href="#">Olvide mi contraseña</a>--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
