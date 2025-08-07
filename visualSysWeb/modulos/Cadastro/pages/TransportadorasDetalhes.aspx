<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="TransportadorasDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.TransportadorasDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes da Transportadora</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <br />
    <br />
    <div id="conteudo" runat="server" class="frame" enableviewstate="false">
        <table>
            <tr>
                <td colspan="4">
                    <p>
                        Nome transportadora</p>
                    <asp:TextBox ID="txtNome_transportadora" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan=4">
                    <p>
                        Razao social</p>
                    <asp:TextBox ID="txtRazao_social" runat="server" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        CNPJ</p>
                    <asp:TextBox ID="txtcnpj" runat="server" CssClass="CNPJ"  Width="120px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        IE</p>
                    <asp:TextBox ID="txtie" runat="server"  Width="150px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Telefone</p>
                    <asp:TextBox ID="txtTelefones" runat="server"  Width="100px"></asp:TextBox>
                </td>
                <td >
                    <p>
                        Placa</p>
                    <asp:TextBox ID="txtPlaca" runat="server"  Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan ="2">
                    <p>
                        Endereco</p>
                    <asp:TextBox ID="txtendereco" runat="server"  Width="280px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Cidade</p>
                    <asp:TextBox ID="txtcidade" runat="server"  Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Estado</p>
                    <asp:TextBox ID="txtestado" runat="server"  Width="30px"></asp:TextBox>
                </td>
                
            </tr>
            <tr>
                <td>
                    <asp:CheckBox  ID="chkPadrao" Text="Default NF" runat="server" />
                </td>
            </tr>
        </table>
    </div>
      <asp:Panel ID="PnConfirmaExcluir" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Excluir a Transportadora?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmarExcluir" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExclusao_Click" />
                    <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelarExcluir" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaExclusao_Click" />
                    <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluir" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnConfirmaExcluir" TargetControlID="Label14">
    </asp:ModalPopupExtender>
</asp:Content>
