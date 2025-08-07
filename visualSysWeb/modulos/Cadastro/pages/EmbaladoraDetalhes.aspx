<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmbaladoraDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.EmbaladoraDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <center>
        <h1 >
            Embaladora</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <br />
    <div id="divConteudo" runat="server" class="conteudo">
        <div class="frameConteudo" style="height: 600px;">
            <div class="MenuEtiqueta" style=" border: solid 1px; padding: 20px 30px; width: 30%; margin-left: 35%;">
                <div class="row">
                    <h2 style="text-align: center">LOGIN FTP</h2>
                </div>
                <div class="row">
                    <div class="panelItem" style="width:15%;">
                        <p>ID</p>
                        <asp:TextBox ID="txtID" runat="server" Width="100%"></asp:TextBox>
                    </div>
                    <div class="panelItem" style="width:82%">
                        <p>Descrição</p>
                        <asp:TextBox ID="txtDescricao" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <p>Usuario</p>
                    <asp:TextBox ID="txtUsuarioFtp" runat="server" CssClass="sem"  Width="100%"></asp:TextBox>

                </div>
                <div class="row">

                    <p>Senha</p>
                    <asp:TextBox ID="txtSenhaFtp" runat="server" type="password" CssClass="sem" Width="100%"></asp:TextBox>


                </div>
                <div class="row">

                    <p>Endereço</p>
                    FTP:\\<asp:TextBox ID="txtEnderecoFtp" runat="server" CssClass="sem" Width="90%"></asp:TextBox>


                </div>


            </div>
        </div>
    </div>


    <asp:Panel ID="PnConfirmaExcluir" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Excluir a Embaladora?"
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
    <asp:ModalPopupExtender ID="modalExcluir" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnConfirmaExcluir" TargetControlID="Label14">
    </asp:ModalPopupExtender>

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
