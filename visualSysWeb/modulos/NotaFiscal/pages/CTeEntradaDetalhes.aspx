<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CTeEntradaDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.pages.CTeEntradaDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>CT-e (Conhecimento de Transportes Eletrônico)</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="conteudo" runat="server" class="conteudo">
        <asp:Panel ID="tabCTe" runat="server" HeaderText="CT-e">
            <div>
                <table class="table">
                    <tr>
                        <td>Código da situação do documento</td>
                        <td>
                            <asp:DropDownList ID="ddlCTeSitDOC" runat="server">
                                <asp:ListItem Value=""> </asp:ListItem>
                                <asp:ListItem Value="00">Documento Regular </asp:ListItem>
                                <asp:ListItem Value="02">Documento Cancelado</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Código do participante (Fornecedor Frete)</td>
                        <td>
                            <asp:TextBox ID="txtCTeCodigoFornecedor" runat="server" Text="" CssClass="numero"></asp:TextBox>
                            <asp:ImageButton ID="btnimg_txtCTeCodigoFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" OnClick="imgLista_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>Número do documento</td>
                        <td>
                            <asp:TextBox ID="txtCTeNumeroDocumento" runat="server" Text="" CssClass="numero"></asp:TextBox>
                        </td>
                        <td>Série do documento</td>
                        <td>
                            <asp:TextBox ID="txtCTeSerie" runat="server" Text="" CssClass="numero"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Data de emissão</td>
                        <td>
                            <asp:TextBox ID="txtCTeEmissao" runat="server" Width="80px"></asp:TextBox>
                            <asp:ImageButton ID="imgDtCTeEmissao" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                            <asp:CalendarExtender ID="cldCTeEmissao" runat="server" PopupButtonID="imgDtCTeEmissao"
                                TargetControlID="txtCTeEmissao">
                            </asp:CalendarExtender>
                        </td>
                        <td>Data de aquisição/prestação do serviço</td>
                        <td>
                            <asp:TextBox ID="txtCTeAquisicao" runat="server" Width="80px"></asp:TextBox>
                            <asp:ImageButton ID="imgDtCTeAquisicao" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                            <asp:CalendarExtender ID="cldCTeAquisicao" runat="server" PopupButtonID="imgDtCTeAquisicao"
                                TargetControlID="txtCTeAquisicao">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>Data de Vencimento Financeiro</td>
                        <td>
                            <asp:TextBox ID="txtCTeVencimento" runat="server" Width="80px"></asp:TextBox>
                            <asp:ImageButton ID="imgDTCTeVencimento" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                            <asp:CalendarExtender ID="cIdCTeVencimento" runat="server" PopupButtonID="imgDTCTeVencimento"
                                TargetControlID="txtCTeVencimento">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>Chave do Conhecimento de Transporte Eletrônico</td>
                        <td>
                            <asp:TextBox ID="txtCTeChave" runat="server" Text="" Width="400px" CssClass="numero"></asp:TextBox>
                        </td>
                        <td>Tipo de CT-e ou BP-e</td>
                        <td>
                            <asp:DropDownList ID="ddlTipoCTeBPe" runat="server">
                                <asp:ListItem Value=""> </asp:ListItem>
                                <asp:ListItem Value="0">CT-e ou BP-e Normal</asp:ListItem>
                                <asp:ListItem Value="1">CT-e de Complemento de Valores</asp:ListItem>
                                <asp:ListItem Value="2">Ct-e emitido em hipótese de anulação</asp:ListItem>
                                <asp:ListItem Value="3">CT-e substituto do CT-e anulado</asp:ListItem>
                                <asp:ListItem Value="4">BP-e TM</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Chave do BP-e substituído</td>
                        <td>
                            <asp:TextBox ID="txtCTeChaveSubstituicao" runat="server" Text="" Width="400px" CssClass="numero"></asp:TextBox>
                        </td>
                        <td>Tipo de Frete</td>
                        <td>
                            <asp:DropDownList ID="ddlTipoCTeFrete" runat="server">
                                <asp:ListItem Value=""> </asp:ListItem>
                                <asp:ListItem Value="0">Por conta do emitente</asp:ListItem>
                                <asp:ListItem Value="1">Por conta do destinatário/remetente</asp:ListItem>
                                <asp:ListItem Value="2">Por conta de terceiros</asp:ListItem>
                                <asp:ListItem Value="3">Sem cobrança de frete</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Base de cálculo do ICMS</td>
                        <td>
                            <asp:TextBox ID="txtCTeBCICMS" runat="server" Text="" CssClass="numero"></asp:TextBox>
                        </td>
                        <td>Redução de Base</td>
                        <td>
                            <asp:TextBox ID="txtCTeReducao" runat="server" Text="" CssClass="numero"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Alíquota ICMS</td>
                        <td>
                            <asp:TextBox ID="txtCTeAliquota" runat="server" Text="" CssClass="numero"></asp:TextBox>
                        </td>
                        <td>Valor do ICMS</td>
                        <td>
                            <asp:TextBox ID="txtCTeValorICMS" runat="server" Text="" CssClass="numero"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Valor do documento fiscal</td>
                        <td>
                            <asp:TextBox ID="txtCTeValorDocFiscal" runat="server" Text="" CssClass="numero"></asp:TextBox>
                        </td>
                        <td>Valor total da prestaçã de serviço</td>
                        <td>
                            <asp:TextBox ID="txtCTeValorTotalPrestServico" runat="server" Text="" CssClass="numero"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Valor total do desconto</td>
                        <td>
                            <asp:TextBox ID="txtCTeValorTotalDesconto" runat="server" Text="" CssClass="numero"></asp:TextBox>
                        </td>
                        <td>Valor não tributado</td>
                        <td>
                            <asp:TextBox ID="txtCTeValorNaoTributado" runat="server" Text="" CssClass="numero"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Código do município de origem (IBGE)</td>
                        <td>
                            <asp:TextBox ID="txtCTeMunicipioOrigem" runat="server" Text="" CssClass="numero"></asp:TextBox>
                            <asp:ImageButton ID="btnimg_txtCTeMunicipioOrigem" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" OnClick="imgLista_Click" />
                        </td>
                        <td>Código do município de destino (IBGE)</td>
                        <td>
                            <asp:TextBox ID="txtCTeMunicipioDestino" runat="server" Text="" CssClass="numero"></asp:TextBox>
                            <asp:ImageButton ID="btnimg_txtCTeMunicipioDestino" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" OnClick="imgLista_Click" />
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </asp:Panel>
    </div>
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnFFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
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
                    <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="LblCancelaLista" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    OnRowCommand="GridLista_RowCommand" OnRowDataBound="GridLista_RowDataBound">
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
    <asp:ModalPopupExtender ID="modalLista" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnfundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>

</asp:Content>
