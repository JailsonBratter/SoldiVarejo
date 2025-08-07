<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CFOPDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.CFOPDetalhes" %>

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
        <center> <h1>Detalhes do CFOP</h1></center>
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
                            <p>CFOP</p>
                            <asp:TextBox ID="txtCFOP" runat="server" CssClass="numero"></asp:TextBox>
                        </td>


                        <td class="style1">
                            <p>TIPO</p>
                            <asp:DropDownList ID="ddlTipo" runat="server" Height="20px" Width="129px">

                                <asp:ListItem Value="2">Entrada</asp:ListItem>
                                <asp:ListItem Value="1">Saida</asp:ListItem>

                            </asp:DropDownList>
                        </td>


                    </tr>
                    <tr>
                        <td colspan="2">
                            <p>
                                DESCRICAO
                            </p>
                            <asp:TextBox ID="txtDESCRICAO" runat="server" TextMode="MultiLine"
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


    <asp:Panel ID="PnExcluirCFOP" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Excluir a CFOP?"
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
    <asp:ModalPopupExtender ID="modalExluirCFOP" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnExcluirCFOP" TargetControlID="Label14">
    </asp:ModalPopupExtender>
</asp:Content>
