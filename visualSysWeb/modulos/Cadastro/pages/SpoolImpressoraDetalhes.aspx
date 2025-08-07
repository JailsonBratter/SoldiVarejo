<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SpoolImpressoraDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.SpoolImpressoraDetalhes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center> <h1>Detalhes do Spool_impressoras</h1></center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <!--Coloque aqui os campos do cabe?alho    -->
        <table>
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo">
        <table>
            <tr>
                <td>
                    <p> Id Trm</p>
                    <asp:TextBox ID="txtid_trm" runat="server" CssClass="numero"></asp:TextBox>
                </td>
                <td>
                    <p> Impressora Remota</p>
                    <asp:TextBox ID="txtimpressoraRemota" runat="server" CssClass="numero"></asp:TextBox>
                </td>
                <td>
                    <p>Descricao</p>
                    <asp:TextBox ID="txtDescricao" runat="server" MaxLength="40"></asp:TextBox>
                </td>
                <td>
                    <p> Ativo</p>
                    <asp:TextBox ID="txtativo" runat="server" CssClass="numero"></asp:TextBox>
                </td>
            </tr>
        </table>
        </div>
</asp:Content>
