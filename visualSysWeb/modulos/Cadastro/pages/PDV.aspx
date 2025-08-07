<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PDV.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.PDV" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>PDV</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server" visible="true">
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        PDV</p>
                    <asp:TextBox ID="txtPDV" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="panelItem" style="margin-bottom: 15px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" GridLines="Vertical"
            CellPadding="5" ForeColor="#333333">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="PDV" Text="---" Visible="true" HeaderText="PDV"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/PDVDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="PDV" />
                <asp:HyperLinkField DataTextField="Modelo" Text="---" Visible="true" HeaderText="Modelo"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/PDVDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="PDV" />
                <asp:HyperLinkField DataTextField="FabEqu" Text="---" Visible="true" HeaderText="Numero de Série"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/PDVDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="PDV" />
                
                <asp:HyperLinkField DataTextField="SAT" Text="---" Visible="true" HeaderText="SAT"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/PDVDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="PDV" />

                <asp:HyperLinkField DataTextField="Tipo_Carga" Text="---" Visible="true" HeaderText="Tipo de Carga"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/PDVDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="PDV" />
                
                <asp:HyperLinkField DataTextField="Endereco" Text="---" Visible="true" HeaderText="Endereco"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/PDVDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="PDV" />
                
                <asp:HyperLinkField DataTextField="Data_ult_atualizacao" Text="---" Visible="true" HeaderText="Ult Carga"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/PDVDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="PDV" />
                
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        <br />
    </div>
</asp:Content>
