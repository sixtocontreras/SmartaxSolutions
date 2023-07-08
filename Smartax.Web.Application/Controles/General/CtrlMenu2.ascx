<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlMenu2.ascx.cs" Inherits="Smartax.Web.Application.Controles.General.CtrlMenu2" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%= Smartax.Web.Application.Clases.Seguridad.FixedData.PlatformName + " " + Smartax.Web.Application.Clases.Seguridad.FixedData.PlatformVersion %></title>
    <link rel="stylesheet" href="style/menu.css" media="screen" type="text/css" />
    <script src="https://www.google.com/recaptcha/api.js"></script>
</head>
<body>
    <div class="login-card">
        <ul class="menu">
            <li><a href="#">Sistema Smartax</a></li>
            <%--<li><a href="#">Photography</a></li>
            <li><a href="#">Coding</a></li>
            <li><a href="#">Applications</a></li>
            <li><a href="#">Portfolio</a></li>
            <li><a href="#">Contact</a></li>--%>
        </ul>
        <ul class="menu2">
            <telerik:radmenu id="MenuDinamico" runat="server" borderstyle="None" font-size="16pt"
                enableoverlay="False" style="z-index: 2900" width="100%">
                <CollapseAnimation Duration="200" Type="OutQuint" />
                <DataBindings>
                    <telerik:RadMenuItemBinding ToolTipField="descripcion_opcion" />
                </DataBindings>
            </telerik:radmenu>

        </ul>
    </div>
</body>
</html>
