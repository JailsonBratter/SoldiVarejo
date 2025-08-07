<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="TributacaoDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.TributacaoDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes da tributacao</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <table>
            <tr>
                <td>
                    <p>
                        Codigo</p>
                    <asp:TextBox ID="txtCodigo_Tributacao" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Descricao</p>
                    <asp:TextBox ID="txtDescricao_Tributacao" runat="server" Width="250px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
        <table>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            ICMS Entrada</p>
                        <asp:TextBox ID="txtEntrada_ICMS" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            ICMS Saida</p>
                        <asp:TextBox ID="txtSaida_ICMS" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Indice ST</p>
                        <asp:TextBox ID="txtIndice_ST" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Redutor</p>
                        <asp:TextBox ID="txtRedutor" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            ICMS Efetivo</p>
                        <asp:TextBox ID="txtICMS_Efetivo" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Numero ECF</p>
                        <asp:TextBox ID="txtNro_ECF" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            CSOSN</p>
                        <asp:TextBox ID="txtCsosn" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            CFOP</p>
                        <asp:TextBox ID="txtCfop" runat="server" Width="80px"></asp:TextBox>
                    </div>
                      <div class="panelItem">
                        <p>
                            CFOP Entrada</p>
                        <asp:TextBox ID="txtCFOPEntrada" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            CST Sped</p>
                        <asp:TextBox ID="txtCstSped" runat="server" Width="80px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <asp:CheckBox ID="chkGera_Mapa" runat="server" Text="Gera Mapa" />
                    </div>
                    <div class="panelItem">
                        <asp:CheckBox ID="chkIncide_ICMS" runat="server" Text="Incide ICMS" />
                    </div>
                    <div class="panelItem">
                        <asp:CheckBox ID="chkIncide_ICM_Subistituicao" runat="server" Text="Incide ICM Substituicao" />
                    </div>
                    <div class="panelItem">
                        <asp:CheckBox ID="chkIpiEmOutrasDespesas" runat="server" Text="IPI em Outras Despesas" />
                    </div>
                    <div class="panelItem">
                        <asp:CheckBox ID="chkIcmsStEmOutrasDespesas" runat="server" Text="ICMS ST em Outras Despesas" />
                    </div>
                </td>
            </tr>
         
        </table>
    </div>
    <asp:Panel ID="PnConfirmaExcluir" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria Excluir a Tributação?"
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
