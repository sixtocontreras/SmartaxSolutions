<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlMunicipioTarifasExcesivas.ascx.cs" Inherits="Smartax.Web.Application.Controles.Parametros.Divipola.CtrlMunicipioTarifasExcesivas" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="width: 100%;">
            <tr>
                <td align="center" bgcolor="#999999">
                    <asp:Label ID="LbTitulo" runat="server" CssClass="SubTitle" Text="REGISTRAR TARIFAS EXCESIVAS POR MUNICIPIOS" Font-Bold="True" Font-Size="16pt" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowFilteringByColumn="True"
                        OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand"
                        OnPageIndexChanged="RadGrid1_PageIndexChanged">
                        <MasterTableView DataKeyNames="id_municipio" Name="Grilla" NoMasterRecordsText="No hay Registros para Mostrar">
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_municipio" EmptyDataText=""
                                    HeaderText="Id" ReadOnly="True" UniqueName="id_municipio">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_departamento" HeaderText="Departamento"
                                    UniqueName="nombre_departamento" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="codigo_dane_mun" HeaderText="Cod. Dane"
                                    UniqueName="codigo_dane_mun" MaxLength="10">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_municipio" HeaderText="Municipio"
                                    UniqueName="nombre_municipio" MaxLength="30">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="numero_nit" HeaderText="Nit"
                                    UniqueName="numero_nit" MaxLength="12" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="digito_verificacion" HeaderText="Dig. Verificacion"
                                    UniqueName="digito_verificacion" MaxLength="1" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre_contacto" HeaderText="Contacto"
                                    UniqueName="nombre_contacto" MaxLength="60" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="direccion_contacto" HeaderText="Dirección"
                                    UniqueName="direccion_contacto" MaxLength="100" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="telefono_contacto" HeaderText="Teléfono"
                                    UniqueName="telefono_contacto" MaxLength="20" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="email_contacto" HeaderText="Email"
                                    UniqueName="email_contacto" MaxLength="60" Visible="false">
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn DataField="codigo_estado" HeaderText="Estado"
                                    UniqueName="codigo_estado" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="fecha_registro" HeaderText="Fecha registro"
                                    UniqueName="fecha_registro" ReadOnly="true">
                                </telerik:GridBoundColumn>

                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="BtnAddTarifa" Text="Tarifas excesivas"
                                    UniqueName="BtnAddTarifa" ImageUrl="/Imagenes/Iconos/16/money_add.png">
                                </telerik:GridButtonColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
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
