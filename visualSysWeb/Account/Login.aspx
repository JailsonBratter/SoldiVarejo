<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="visualSysWeb.Account.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #txtUsuario
        {
        }
        .style1
        {
            width: 193px;
        }
        #txtSenha
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="login" >
       <center> <asp:Label ID="lblerros" runat="server" ForeColor="red" Font-Size="Medium"></asp:Label>
           <asp:LinkButton ID="lnkChave" runat="server" Visible="false" 
               onclick="lnkChave_Click"><br />Inserir Nova Chave de Licença!</asp:LinkButton>
       </center>
        <table >
            <tr>
                <td>
                    <img src="../img/ico_empresa.jpg" class="imgIco" />
                </td>
                <td>
                    Filial
                </td>
                <td class="style1">
                    <asp:DropDownList ID="ddfilial" runat="server" Height="20px" Width="143px" OnSelectedIndexChanged="ddfilial_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <asp:Panel ID="pnLogin" runat="server" Visible = "true">
                <tr>
                    <td>
                        <img src="../img/User.png" class="imgIco" />
                    </td>
                    <td>
                        Usuario
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtUsuario" runat="server" Width="135px" autocomplete="off" AutoCompleteType="Disabled" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <img src="../img/Senha.png" class="imgIco" />
                    </td>
                    <td>
                        Senha
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtSenha" runat="server" TextMode="Password" Width="135px" AutoPostBack="true"
                            OnTextChanged="txtSenha_TextChanged" autocomplete="off" AutoCompleteType="Disabled" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td  colspan="3">
                        <center>
                            <p>
                                <asp:Button ID="btnEntrar" runat="server" Text="Entrar" OnClick="LoginButton_Click"
                                    CssClass="btnEntrar" />
                            </p>
                        </center>
                    </td>
                </tr>
            </asp:Panel>
        </table>
        <asp:Panel ID="PnChave" runat="server" Visible="false">
            <table>
                <tr>
                    <td class="style1">
                        <p>
                            CHAVE DE VALIDAÇÃO</p>
                        <asp:TextBox ID="txtChave" runat="server" TextMode="MultiLine" Width="300px" Height="100PX"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <center>
                            <p>
                                <asp:Button ID="btnValidaChave" runat="server" Text="Salvar Chave" 
                                    OnClick="btnValidaChave_Click" />
                            </p>
                        </center>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
