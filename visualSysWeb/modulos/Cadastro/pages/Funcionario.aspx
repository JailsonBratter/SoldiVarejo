<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Funcionario.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.Funcionario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Funcionarios</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        Codigo</p>
                    <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Nome</p>
                    <asp:TextBox ID="txtNome" runat="server"> </asp:TextBox>
                </td>
                <td>
                    <p>
                        Função</p>
                    <asp:DropDownList ID="ddlFuncao" runat="server">
                    </asp:DropDownList>
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
                <asp:HyperLinkField DataTextField="codigo" Text="----" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/Pages/FuncionarioDetalhes.aspx?nome={0}&codigo={1}"
                    DataNavigateUrlFields="nome,codigo" />
                <asp:HyperLinkField DataTextField="Nome" Text="----" Visible="true" HeaderText="User"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/Pages/FuncionarioDetalhes.aspx?nome={0}&codigo={1}"
                    DataNavigateUrlFields="nome,codigo" />
                <asp:HyperLinkField DataTextField="Nome2" Text="----" Visible="true" HeaderText="Nome"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/Pages/FuncionarioDetalhes.aspx?nome={0}&codigo={1}"
                    DataNavigateUrlFields="nome,codigo" />
                <asp:HyperLinkField DataTextField="sobrenome" Text="----" Visible="true" HeaderText="Sobrenome"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/Pages/FuncionarioDetalhes.aspx?nome={0}&codigo={1}"
                    DataNavigateUrlFields="nome,codigo" />
                <asp:HyperLinkField DataTextField="Funcao" Text="----" Visible="true" HeaderText="Função"
                    DataNavigateUrlFormatString="~/modulos/Cadastro/Pages/FuncionarioDetalhes.aspx?nome={0}&codigo={1}"
                    DataNavigateUrlFields="nome,codigo" />
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
    </div>
</asp:Content>
