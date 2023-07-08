<%@ Page Language="C#" MasterPageFile="~/MasterPageGeneral.Master" AutoEventWireup="true" CodeBehind="FrmCruzado.aspx.cs" Inherits="Smartax.Web.Application.Modulos.Reportes.Formatos.FrmCruzado" %>
<%@ Register src="~/Controles/Reportes/Formatos/CtrlCruzado.ascx" tagname="Cruzado" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Cruzado ID="Cruzado" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>