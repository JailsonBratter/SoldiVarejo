<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PDVDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.PDVDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style3 {
            width: 77px;
        }

        .style4 {
            width: 461px;
        }

        .style5 {
            width: 235px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes do PDV</h1>
        </center>
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

    <div id="conteudo" runat="server" class="conteudo" enableviewstate="false" style="margin-bottom:30px;padding:20px;">
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="view1" runat="server">
                <table>
                    <tr>
                        <td class="style3">
                            <p>
                                Nro PDV
                            </p>
                            <asp:TextBox ID="txtPDV" runat="server" CssClass="numero" Width="68px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Modelo
                            </p>
                            <asp:TextBox ID="txtModelo" runat="server" CssClass="texto" Width="142px"></asp:TextBox>
                        </td>
                        <td class="style5">
                            <p>
                                Numero de Série
                            </p>
                            <asp:TextBox ID="txtNumeroSerie" runat="server" CssClass="texto" Width="235px" MaxLength="20"></asp:TextBox>
                        </td>


                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="panelItem">
                                <asp:CheckBox ID="chkSat" runat="server" Text="UTILIZA SAT" />
                            </div>
                            <br>
                            <br>
                            <div class="panelItem">
                                <asp:CheckBox ID="chkCargaAutomatica" runat="server" Text="Carga Automatica" />
                            </div>
                            <br>
                            <br>
                            <div class="panelItem">
                                <asp:CheckBox ID="chkIntegracaoVendasAutomatica" runat="server" Text="Integração Vendas Automaticas" />
                            </div>
                        </td>
                        <td>
                            
                            <div class="panelItem">

                                <p>
                                    Ultima Carga
                                </p>
                                <asp:TextBox ID="txtUltimaAtualizacao" runat="server" Width="80px"></asp:TextBox>
                            </div>
                         
                            <div class="panelItem">

                                <p>
                                    Ultima Integracao Vendas
                                </p>
                                <asp:TextBox ID="txtUltimaIntegracaoVendas" runat="server" Width="80px"></asp:TextBox>
                            </div>


                        </td>

                    </tr>
                    <tr>
                        <td colspan="3">
                            <div class="panelItem" style="width:98%;">

                                <p>
                                    Connection String
                                </p>
                                <asp:TextBox ID="txtConnectionString" runat="server" Width="100%"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="border-style: solid; border-width: 1px; margin-top:20px;">
                    <tr>
                        <td colspan="2">
                            <h1>Tipo de carga</h1>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <br />
                            <asp:RadioButton ID="rdoLinkServer" runat="server" GroupName="EscolhaTipo" AutoPostBack="true" OnCheckedChanged="rdoTipoCarga_Change" />

                        </td>
                        <td>

                            <p>
                                Link Server
                            </p>
                            <asp:TextBox ID="txtLinkServer" runat="server" Width="460px" CssClass="sem"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnLink" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                OnClick="Img_Click" />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <br />

                            <asp:RadioButton ID="rdoDiretorioCarga" runat="server" GroupName="EscolhaTipo" AutoPostBack="true" OnCheckedChanged="rdoTipoCarga_Change" />

                        </td>
                        <td>
                            <p>
                                Diretorio de Carga
                            </p>
                            <asp:TextBox ID="txtDiretorio_Carga" runat="server" Width="460px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />

                            <asp:RadioButton ID="rdoCargaServico" runat="server" GroupName="EscolhaTipo" AutoPostBack="true" OnCheckedChanged="rdoTipoCarga_Change" />

                        </td>
                        <td>
                          <div style=" margin-top:15px">Carga por Serviço </div> 
                        </td>
                    </tr>

                </table>
            </asp:View>
        </asp:MultiView>
    </div>

    <asp:Panel ID="PnExcluirPDV" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Excluir a PDV?"
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
    <asp:ModalPopupExtender ID="modalExluirPDV" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnExcluirPDV" TargetControlID="Label14">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px;">
            <center><h1><asp:Label ID="lbllista" runat="server" Text="" ></asp:Label></h1></center>
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
                    <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
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
