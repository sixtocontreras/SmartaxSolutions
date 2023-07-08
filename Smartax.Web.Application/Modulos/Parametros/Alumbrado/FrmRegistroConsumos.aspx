<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPageGeneral.Master" CodeBehind="FrmRegistroConsumos.aspx.cs" Inherits="Smartax.Web.Application.Modulos.Parametros.Alumbrado.FrmRegistroConsumos" %>
<%@ Register src="~/Controles/Parametros/Alumbrado/CtrlRegistroConsumos.ascx" tagname="CtrlRegistroConsumos" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
    function setMpio(dpto, mpio, ofi) {
        debugger;
        var muni = JSON.parse(document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdMuni").value);
        var data = "";
        data += "<option value=''>Seleccione</option>";
        for (var i = 0; i < muni.length; i++) {
            if (muni[i].dpto.toString() === dpto) {
                data += "<option " + (muni[i].id === mpio ? "selected='selected'" : "") + " value='" + muni[i].id + "'>" + muni[i].nombre + "</option>";
            }
        }
        document.getElementById("id_municipioCombo").innerHTML = data;
        setOfi(mpio, ofi);
    }
     var setMpioOfiData = function (dpto, mpio,ofi) {
        document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdOfiSelected").value = ofi;        
        document.getElementById("ContentPlaceHolder1_CtrlRegistroConsumos_hdMuniSelect").value = mpio;
    }
    function setOfi(mpio, ofi) {

    }
        
    function reloadPagePrincipal() {
        document.location.reload();
    }
</script>
    <uc1:CtrlRegistroConsumos ID="CtrlRegistroConsumos" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
