<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlPie2.ascx.cs" Inherits="Smartax.Web.Application.Controles.General.CtrlPie2" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="style/menu.css" media="screen" type="text/css" />
</head>
<body>
    <div class="login-card">
        <ul class="menu3">
            <table align="left" cellpadding="10" cellspacing="0">
                <tr>
                    <td align="left">&nbsp;</td>
                    <td align="left">Copyright ©<b><asp:Label ID="LblAnioActual" runat="server" Text="Año"></asp:Label>
                    </b>&nbsp;Derechos Reservados del Autor.<br />
                        <b>
                            <asp:Label ID="LblVersion" runat="server"></asp:Label></b>
                    </td>
                    <td>
                        <img alt="Powered By Sistemas & Soluciones Informaticas SSI S.A.S."
                            src="/imagenes/General/pow_by_aspnet.png" style="width: 88px; height: 31px" />
                    </td>
                </tr>
            </table>
        </ul>
    </div>
</body>
</html>
