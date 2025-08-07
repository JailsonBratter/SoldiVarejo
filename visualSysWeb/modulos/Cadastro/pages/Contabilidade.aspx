<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contabilidade.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.Contabilidade" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <center><h1>Contabilidade</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server" >
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <div class="panelItem">
                    <p>
                        Codigo</p>
                    <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                    <p>
                        Nome</p>
                    <asp:TextBox ID="txtNome" runat="server"></asp:TextBox>
                    </div>
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
                <asp:HyperLinkField DataTextField="cod" Text="---" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ComandasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="cod" />
                <asp:HyperLinkField DataTextField="Status" Text="---" Visible="true" HeaderText="Status"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ComandasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Comanda" />
                <asp:HyperLinkField DataTextField="Qtd_itens" Text="---" Visible="true" HeaderText="Qtde itens"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ComandasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Comanda" />
                <asp:HyperLinkField DataTextField="Valor" Text="---" Visible="true" HeaderText="Valor"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ComandasDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="Comanda" />
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
