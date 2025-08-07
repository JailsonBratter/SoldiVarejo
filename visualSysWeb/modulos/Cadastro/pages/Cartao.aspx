<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Cartao.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.Cartao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>cartao</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        <div class="panelDefault">
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <div class="row">
                <div class="panelItem">
                    <p>
                        Id Cartão</p>
                    <asp:TextBox ID="txtIdCartao" runat="server"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Id Finalizadora</p>
                    <asp:TextBox ID="txtIdFinalizadora" runat="server"> </asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Bandeira</p>
                    <asp:TextBox ID="txtBandeira" runat="server"> </asp:TextBox>
                </div>
            </div>
        </div>
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
                <asp:HyperLinkField DataTextField="nro_Finalizadora" Text="nro_Finalizadora" Visible="true"
                    HeaderText="Nro Finalizadora" DataNavigateUrlFormatString="~/modulos/Cadastro/Pages/cartaoDetalhes.aspx?id_cartao={0}&nro_finalizadora={1}"
                    DataNavigateUrlFields="id_cartao,nro_Finalizadora" />
                <asp:HyperLinkField DataTextField="id_cartao" Text="id_cartao" Visible="true" HeaderText="Id Cartão"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/Pages/cartaoDetalhes.aspx?id_cartao={0}&nro_finalizadora={1}"
                    DataNavigateUrlFields="id_cartao,nro_Finalizadora" />
                <asp:HyperLinkField DataTextField="data" Text="data" Visible="true" HeaderText="Data"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/Pages/cartaoDetalhes.aspx?id_cartao={0}&nro_finalizadora={1}"
                    DataNavigateUrlFields="id_cartao,nro_Finalizadora" />
                <asp:HyperLinkField DataTextField="centro_custo" Text="centro_custo" Visible="true"
                    HeaderText="Centro Custo" DataNavigateUrlFormatString="~/modulos/Cadastro/Pages/cartaoDetalhes.aspx?id_cartao={0}&nro_finalizadora={1}"
                    DataNavigateUrlFields="id_cartao,nro_Finalizadora" />
                <asp:HyperLinkField DataTextField="fornecedor" Text="fornecedor" Visible="true" HeaderText="Fornecedor"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/Pages/cartaoDetalhes.aspx?id_cartao={0}&nro_finalizadora={1}"
                    DataNavigateUrlFields="id_cartao,nro_Finalizadora" />
                <asp:HyperLinkField DataTextField="bandeira" Text="bandeira" Visible="true" HeaderText="Bandeira"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/Pages/cartaoDetalhes.aspx?id_cartao={0}&nro_finalizadora={1}"
                    DataNavigateUrlFields="id_cartao,nro_Finalizadora" />
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
