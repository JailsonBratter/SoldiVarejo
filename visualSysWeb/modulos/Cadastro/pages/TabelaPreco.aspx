<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="True" CodeBehind="TabelaPreco.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.TabelaPreco" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>Tabelas de preco</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        <table>
            <tr>
                <td>
                    <p>
                        Codigo Tabela
                    </p>
                    <asp:TextBox ID="TxtCodigo_Tabela" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Nro Tabela
                    </p>
                    <asp:TextBox ID="txtNroTabela" runat="server"></asp:TextBox>
                </td>
                <td>
                    <p>Tipo</p>
                    <asp:DropDownList ID="ddlTipo" runat="server">
                        <asp:ListItem Text="TODOS" />
                        <asp:ListItem Text="DESCONTO" />
                        <asp:ListItem Text="ACRESCIMO" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div class="panelItem" style="margin-bottom: 15px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label>
        </h1>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
            GridLines="Vertical">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Codigo_tabela" Text="----" Visible="true" HeaderText="Codigo Tabela"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/TabelaPrecoDetalhes.aspx?codigo_tabela={0}"
                    DataNavigateUrlFields="codigo_tabela" />
                <asp:HyperLinkField DataTextField="Nro_tabela" Text="----" Visible="true" HeaderText="Nro Tabela"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/TabelaPrecoDetalhes.aspx?codigo_tabela={0}"
                    DataNavigateUrlFields="codigo_tabela" />
                <asp:HyperLinkField DataTextField="Porc" Text="----" Visible="true" HeaderText="Porc %"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/TabelaPrecoDetalhes.aspx?codigo_tabela={0}"
                    DataNavigateUrlFields="codigo_tabela" />
                <asp:HyperLinkField DataTextField="Tipo" Text="----" Visible="true" HeaderText="Tipo"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/TabelaPrecoDetalhes.aspx?codigo_tabela={0}"
                    DataNavigateUrlFields="codigo_tabela" />
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


    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 50px; margin-bottom: 20px;">
                        <asp:Button ID="btnOkError" runat="server" Text="OK" Width="200px" Height="100%"
                            Font-Size="Larger" OnClick="btnOkError_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalError" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnError" TargetControlID="lblErroPanel">
    </asp:ModalPopupExtender>
</asp:Content>
