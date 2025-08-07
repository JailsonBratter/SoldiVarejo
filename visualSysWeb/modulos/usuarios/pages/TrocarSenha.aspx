<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="TrocarSenha.aspx.cs" Inherits="visualSysWeb.modulos.usuarios.pages.TrocarSenha" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Alterar Senha</h1></center>
    <hr />
    <asp:Label ID="lblerros" runat="server" ForeColor="red"></asp:Label>
    <div class="login" style="margin-top:1px;">
        <table>
            <tr>
                <td>
                    <img src="../../../img/senha.png" class="imgIco" />
                </td>
                <td>
                    Senha Atual
                </td>
                <td class="style1">
                    <asp:TextBox ID="txtSenha" runat="server" TextMode="Password" CssClass="SEM" Width="135px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <img src="../../../img/senha.png" class="imgIco" />
                </td>
                <td>
                    Nova Senha
                </td>
                <td class="style1">
                    <asp:TextBox ID="txtNovaSenha" runat="server" TextMode="Password" CssClass="SEM"
                        Width="135px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <img src="../../../img/senha.png" class="imgIco" />
                </td>
                <td>
                    Confirma Senha
                </td>
                <td class="style1">
                    <asp:TextBox ID="txtConfirmaNova" runat="server" TextMode="Password" CssClass="SEM"
                        Width="135px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
                <td>
                    <p>
                        <center>
                             <asp:Button ID="btnSalvar" runat="server" Text="Salvar" onclick="LoginButton_Click" 
                                            CssClass="btnEntrar" />
                        </center>
                    </p>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
