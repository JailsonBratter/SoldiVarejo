<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ContaCorrente.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.ContaCorrente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<center><h1>Conta Corrente</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server" >
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        Id Conta</p>
                    <asp:TextBox ID="txtId_Conta" runat="server"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Banco</p>
                    <asp:TextBox ID="txtBanco" runat="server"> </asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
     <div class="panelItem" style="margin-bottom: 15px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
            GridLines="Vertical">
             <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Id_cc" Text="----" Visible="true" HeaderText="Id conta"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ContaCorrenteDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="id_cc" />
                <asp:HyperLinkField DataTextField="Banco" Text="----" Visible="true" HeaderText="Cod Banco"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ContaCorrenteDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="id_cc" />
                <asp:HyperLinkField DataTextField="Nome_Banco" Text="----" Visible="true" HeaderText="Nome Banco"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ContaCorrenteDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="id_cc" />
                <asp:HyperLinkField DataTextField="Agencia" Text="----" Visible="true" HeaderText="Agencia"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ContaCorrenteDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="id_cc" />
                <asp:HyperLinkField DataTextField="conta" Text="----" Visible="true" HeaderText="Conta"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ContaCorrenteDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="id_cc" />
                <asp:HyperLinkField DataTextField="Saldo" Text="----" Visible="true" HeaderText="Saldo"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ContaCorrenteDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="id_cc"  >
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:HyperLinkField>
                <asp:HyperLinkField DataTextField="cCaixa" Text="----" Visible="true" HeaderText="Conta Caixa"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/pages/ContaCorrenteDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="id_cc" />

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
