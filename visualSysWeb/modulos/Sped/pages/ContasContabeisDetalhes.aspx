<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ContasContabeisDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Sped.pages.ContasContabeisDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>Contas Contábeis</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />

    <div id="conteudo" runat="server" class="conteudo" style="overflow: auto;">
        <div class="panel" style="margin-left: 25%;margin-bottom: 20px; display:inline-block; width:60%;">
            <div class="row">
                <div class="panelItem">
                    <p>Codigo</p>
                    <asp:TextBox ID="txtCod_conta" runat="server" MaxLength="50" Width="100px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>Data</p>
                    <asp:TextBox ID="txtData" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>Descrição</p>
                    <asp:TextBox ID="txtDescricao" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>Nivel</p>
                    <asp:TextBox ID="txtNivel" runat="server" MaxLength="1" Width="50px"></asp:TextBox>
                </div>
                  <div class="panelItem">
                    <p>Conta Relacionada RFB</p>
                    <asp:TextBox ID="txtContaRelacionada" runat="server" MaxLength="10" Width="150px"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="panelItem">
                    <p>
                        Tipo
                    </p>
                    <asp:DropDownList ID="ddlTipo" runat="server" Height="21px">
                        <asp:ListItem Value="A" Text="A-Analítica(conta)"></asp:ListItem>
                        <asp:ListItem Value="S" Text="S-Sintética(grupo e contas)"></asp:ListItem>

                    </asp:DropDownList>
                </div>
                <div class="panelItem">
                    <p>
                        Natureza
                    </p>
                    <asp:DropDownList ID="ddlNatureza" runat="server" Height="21px">
                        <asp:ListItem Value="01" Text="01-Contas de Ativo"></asp:ListItem>
                        <asp:ListItem Value="02" Text="02-Contas de Passivo"></asp:ListItem>
                        <asp:ListItem Value="03" Text="03-Patrimônio líquido"></asp:ListItem>
                        <asp:ListItem Value="04" Text="04-Contas de resultado"></asp:ListItem>
                        <asp:ListItem Value="05" Text="05-Contas de compensação"></asp:ListItem>
                        <asp:ListItem Value="09" Text="09-Outras"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="panelItem">
                    <p>
                        Entrada/Saida
                    </p>
                    <asp:DropDownList ID="ddlEntradaSaida" runat="server" Height="21px">
                      
                        <asp:ListItem Value="1" Text="Entrada"></asp:ListItem>
                        <asp:ListItem Value="0" Text="Saida"></asp:ListItem>

                    </asp:DropDownList>

                </div>
                
                <div class="panelItem">
                    <p>CNPJ do Estabelecimento</p>
                    <asp:TextBox ID="txtCNPJ" runat="server" MaxLength="18" Width="150px"></asp:TextBox>
                </div>

            </div>
          


        </div>

    </div>

       <asp:Panel ID="pnMsg" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px;">
            <h2>
                <asp:Label ID="lblMsgPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap;
                        justify-content: center; height: 50px; margin-bottom: 20px;">
                        <asp:Button ID="btnOkError" runat="server" Text="OK" Width="200px" Height="100%"
                            Font-Size="Larger" OnClick="btnOkError_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalMsg" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnMsg" TargetControlID="lblMsgPanel">
    </asp:ModalPopupExtender>

   <asp:Panel ID="pnConfirmaExcluir" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="lblConfirmaExcluir" runat="server" Text="Tem Certeza que gostaria de Excluir a Conta?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmarExcluir" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExcluir_Click" />
                    <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelarExcluir" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelarExcluir_Click" />
                    <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluir" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaExcluir" TargetControlID="lblConfirmaExcluir">
    </asp:ModalPopupExtender>
</asp:Content>
