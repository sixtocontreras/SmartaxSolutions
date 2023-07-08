<%@ Page Language="C#" MasterPageFile="~/MasterPageGeneral.Master" AutoEventWireup="true" CodeBehind="FrmEjecuciones.aspx.cs" Inherits="Smartax.Web.Application.Modulos.Reportes.Formatos.FrmEjecuciones" %>
<%@ Register src="~/Controles/Reportes/Formatos/CtrlEjecuciones.ascx" tagname="CtrlEjecuciones" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CtrlEjecuciones ID="CtrlEjecuciones" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
