<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UsuariosDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.usuarios.pages.UsuariosDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" >
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes do usuarios_web</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="conteudo" runat="server" class="conteudo" style="top: 100px; float: left; width: 100%;">
        <div class="panelDefault">
            <div class="row">
                <div class="panelItem">
                    <p>
                        ID
                    </p>
                    <asp:TextBox ID="txtId" runat="server" Width="80px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Filial
                    </p>
                    <asp:TextBox ID="txtFilial" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="btnFilial" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                        OnClick="lista_click" Width="15px" />
                </div>
                <div class="panelItem">
                    <p>
                        Usuario
                    </p>
                    <asp:TextBox ID="txtusuario" runat="server"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Nome
                    </p>
                    <asp:TextBox ID="txtnome" runat="server"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Senha
                    </p>
                    <asp:TextBox ID="txtsenha" runat="server" TextMode="Password" CssClass="SEM"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <br />
                    <asp:CheckBox ID="chkadm" runat="server" Text="adm" />
                </div>
            </div>
            <br />
            <br />
            <div class="row">
                <hr />
                <div class="row">
                    <div class="panelItem">
                        <center><h3>Configurações de envio de Email</h3></center>
                    </div>
                </div>
                <hr />
                <div class="panelItem">
                    <p>
                        Endereço SMTP
                    </p>
                    <asp:TextBox ID="txtHost" runat="server" CssClass="SEM"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Porta
                    </p>
                    <asp:TextBox ID="txtPorta" runat="server" CssClass="Numero" Width="80px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Email
                    </p>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="SEM" Width="300px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Email Senha
                    </p>
                    <asp:TextBox ID="txtEmailSenha" runat="server" TextMode="Password" CssClass="SEM"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="row">
                    <div class="panelItem">
                        <center><h3>Configurações de Funcionario e Operador</h3></center>
                    </div>
                </div>
                <br />
                <hr />

                <div class="panelItem">
                    <p>
                        Codigo Funcionario
                    </p>
                    <asp:TextBox ID="txtCodigo_funcionario" runat="server" CssClass="Numero" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnCodigo_funcionario" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                        OnClick="lista_click" Width="15px" />
                </div>
                <div class="panelItem">
                    <p>
                        Codigo Operador
                    </p>
                    <asp:TextBox ID="txtId_operador" runat="server" CssClass="Numero" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnId_operador" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                        OnClick="lista_click" Width="15px" />
                </div>
            </div>
            <div class="row">
                <div class="row">
                    <div class="panelItem">
                        <center><h3>Configurações adicionais</h3></center>
                    </div>
                </div>
                <br />
                <hr />

                <div class="panelItem">
                    <p>
                        Limitar acesso a cliente GRUPO EMPRESA 
                    </p>
                    <asp:TextBox ID="txtGruupoEmpresa" runat="server" CssClass="Numero" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="imgListaGrupoEmpresa" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                        OnClick="lista_click" Width="15px" />
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="pnConfima" runat="server" CssClass="frameModal" Style="display: none">
        <asp:Label ID="Label1" runat="server" Text="Confirma Exclusão" CssClass="cabMenu"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExclusao_Click" />
                    <asp:Label ID="Label2" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaExclusao" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaExclusao_Click" />
                    <asp:Label ID="Label3" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluir" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfima" TargetControlID="Label1">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame">
            <asp:Label ID="lbllista" runat="server" Text="" CssClass="cabMenu"></asp:Label>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                Filtrar
                <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="400px"></asp:TextBox>
                <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="ImgPesquisaLista_Click" />
                <asp:Label ID="lblErroPesquisa" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnFechar_Click" />
                    <asp:Label ID="Label4" runat="server" Text="Seleciona"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    OnRowDataBound="GridLista_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="RdoListaItem" runat="server" GroupName="GrlistaItem" />
                            </ItemTemplate>
                        </asp:TemplateField>
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
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalPnFundo" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
</asp:Content>
