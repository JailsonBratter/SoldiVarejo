<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SolicitacaoDeEncomenda.aspx.cs" Inherits="visualSysWeb.modulos.Estoque.pages.SolicitacaoDeEncomenda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <center><h1>Solicitação de Encomenda</h1></center>
    <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server" >
        <table>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Codigo</p>
                        <asp:TextBox ID="txtCodigo" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Descricao</p>
                        <asp:TextBox ID="txtDescricao" runat="server" Width="300px"> </asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                           Status</p>
                        <asp:DropDownList ID="ddlStatus" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>INICIADO</asp:ListItem>
                            <asp:ListItem>ENCERRADO</asp:ListItem>
                        </asp:DropDownList>
                        
                    </div>

                </td>
            </tr>
        </table>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" 
            CellPadding="6" ForeColor="#333333" GridLines="Vertical">
             <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="codigo" Text="---" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="SolicitacaoDeEncomendaDetalhes.aspx?codigo={0}"
                    DataNavigateUrlFields="codigo" />
                <asp:HyperLinkField DataTextField="descricao" Text="----" Visible="true" HeaderText="Descricao"
                    DataNavigateUrlFormatString="SolicitacaoDeEncomendaDetalhes.aspx?codigo={0}"
                    DataNavigateUrlFields="codigo" />
                <asp:HyperLinkField DataTextField="data_cadastro" Text="-----" Visible="true" HeaderText="Data Cadastro"
                    DataNavigateUrlFormatString="SolicitacaoDeEncomendaDetalhes.aspx?codigo={0}"
                    DataNavigateUrlFields="codigo" />
                <asp:HyperLinkField DataTextField="usuario_cadastro" Text="----" Visible="true" HeaderText="Usuario"
                    DataNavigateUrlFormatString="SolicitacaoDeEncomendaDetalhes.aspx?codigo={0}"
                    DataNavigateUrlFields="codigo" />
                <asp:HyperLinkField DataTextField="status" Text="---" Visible="true" HeaderText="Status"
                    DataNavigateUrlFormatString="SolicitacaoDeEncomendaDetalhes.aspx?codigo={0}"
                    DataNavigateUrlFields="codigo" />
                <asp:HyperLinkField DataTextField="pedido" Text="---" Visible="true" HeaderText="Pedidos"
                    DataNavigateUrlFormatString="SolicitacaoDeEncomendaDetalhes.aspx?codigo={0}"
                    DataNavigateUrlFields="codigo" />
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
