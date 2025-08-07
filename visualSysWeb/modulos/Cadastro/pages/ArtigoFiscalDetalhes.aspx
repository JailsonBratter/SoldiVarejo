<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ArtigoFiscalDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.ArtigoFiscalDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1 {
            width: 138px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center> <h1>Detalhes do Artigo Fiscal</h1></center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>

    <div id="cabecalho" runat="server" class="frame">
        <!--Coloque aqui os campos do cabe?alho    -->
        <table>
            <tr>
                <td></td>
            </tr>
        </table>
    </div>
    <div class="opcoes">
        <asp:Menu ID="tabMenu" runat="server" Orientation="Horizontal"
            OnMenuItemClick="tabMenu_MenuItemClick" Visible="true">

            <Items>
                <asp:MenuItem Text="Código Fiscal de Operação" Value="tab1" />
            </Items>
            <StaticMenuStyle CssClass="tab" />
            <StaticMenuItemStyle CssClass="item" />
            <StaticSelectedStyle BackColor="Beige" ForeColor="#465c71" />
        </asp:Menu>
    </div>

    <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="view1" runat="server">
                <table>
                    <tr>

                        <td>
                            <p>Codigo do Artigo</p>
                            <asp:TextBox ID="txtArtigoFiscal" runat="server" CssClass="numero"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <p>
                                Descrição
                            </p>
                            <asp:TextBox ID="txtDescricao" runat="server" TextMode="MultiLine"
                                Width="299px" Height="133px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </div>
    <asp:Panel ID="pnFundo" runat="server" CssClass="fundo" Visible="false">
        <asp:Label ID="lbllista" runat="server" Text="Label" CssClass="cabMenu"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaLista_Click"></asp:ImageButton>
                    <asp:Label ID="Label4" runat="server" Text="Seleciona"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>

        <asp:Panel ID="Panel1" runat="server" CssClass="lista">
            <asp:RadioButtonList ID="chkLista" runat="server" Height="50" Width="200">
            </asp:RadioButtonList>
        </asp:Panel>
    </asp:Panel>


    <asp:Panel ID="PnExcluirArtigoFiscal" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Excluir O ArtigoFiscal?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmarInativar" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExclusao_Click" />
                    <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaInativar" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaExclusao_Click" />
                    <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExluirArtigoFiscal" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnExcluirArtigoFiscal" TargetControlID="Label14">
    </asp:ModalPopupExtender>
</asp:Content>
