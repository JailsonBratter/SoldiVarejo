<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FechamentoEstoque.aspx.cs" Inherits="visualSysWeb.modulos.Estoque.pages.FechamentoEstoque" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Fechamento de Estoque</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <div class="panelItem">

                        <p>
                            Id Fechamento
                        </p>
                        <asp:TextBox ID="txtIdFechamento" runat="server" Width="100px"></asp:TextBox>
                    </div>
                    
                    <div class="panelItem">
                        <p>
                            Data Cadastro
                        </p>
                        <asp:TextBox ID="txtDtCadatro" runat="server" Width="80px" MaxLength="10"> </asp:TextBox>
                        <asp:ImageButton ID="ImgDtCadastro" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="clnDtCadastro" runat="server" PopupButtonID="ImgDtCadastro" TargetControlID="txtDtCadatro">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p>
                            Data Fechamento
                        </p>
                        <asp:TextBox ID="txtDtFechamento" runat="server" Width="80px" MaxLength="10"> </asp:TextBox>
                        <asp:ImageButton ID="imgDtFechamento" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="cldDtFechamento" runat="server" PopupButtonID="imgDtFechamento"
                            TargetControlID="txtDtFechamento">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p>
                            Usuario
                        </p>
                        <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
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
                <asp:HyperLinkField DataTextField="id_Fechamento" Text="---" Visible="true" HeaderText="Id"
                    DataNavigateUrlFormatString="~/modulos/Manutencao/pages/FechamentoEstoque.aspx?idFechamento={0}&Tela=MA005"
                    DataNavigateUrlFields="id_Fechamento" />
                <asp:HyperLinkField DataTextField="data_cadastro" Text="---" Visible="true" HeaderText="Cadastro"
                    DataNavigateUrlFormatString="~/modulos/Manutencao/pages/FechamentoEstoque.aspx?idFechamento={0}&Tela=MA005"
                    DataNavigateUrlFields="id_Fechamento" />
                <asp:HyperLinkField DataTextField="data_fechamento" Text="---" Visible="true" HeaderText="Fechamento"
                    DataNavigateUrlFormatString="~/modulos/Manutencao/pages/FechamentoEstoque.aspx?idFechamento={0}&Tela=MA005"
                    DataNavigateUrlFields="id_Fechamento" />
                <asp:HyperLinkField DataTextField="usuario" Text="---" Visible="true" HeaderText="Usuario"
                    DataNavigateUrlFormatString="~/modulos/Manutencao/pages/FechamentoEstoque.aspx?idFechamento={0}&Tela=MA005"
                    DataNavigateUrlFields="id_Fechamento" />

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
