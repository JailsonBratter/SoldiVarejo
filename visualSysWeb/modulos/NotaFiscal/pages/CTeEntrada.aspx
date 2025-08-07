<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CTeEntrada.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.pages.CTeEntrada"
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Conhecimento de Transporte</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Panel ID="Panel2" runat="server" CssClass="filter">
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        Codigo</p>
                    <asp:TextBox ID="txtCodigo" runat="server" AutoPostBack="true" OnTextChanged="txtCodigo_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Fornecedor</p>
                    <asp:TextBox ID="txtFornecedor" runat="server" AutoPostBack="true" OnTextChanged="txtCodigo_TextChanged"> </asp:TextBox>
                    <asp:ImageButton ID="imgFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="imgFornecedor_Click" />
                </td>
                <td>
                    <p>
                        De</p>
                    <asp:TextBox ID="txtDe" runat="server" Width="100px" MaxLength="10"> </asp:TextBox>
                    <asp:Image ID="ImgDeCalendario" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDeCalendario"
                        TargetControlID="txtDe">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Ate</p>
                    <asp:TextBox ID="txtAte" runat="server" Width="100px" MaxLength="10"> </asp:TextBox>
                    <asp:ImageButton ID="dt_txtAte" runat="server" ImageUrl="~/img/calendar.png" Height="15px" />
                    <asp:CalendarExtender ID="ClnDataAte" runat="server" PopupButtonID="Dt_txtAte" TargetControlID="txtAte">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Pesquisar Por</p>
                    <asp:DropDownList ID="DllTipoPesquisa" runat="server">
                        <asp:ListItem Value="">---- </asp:ListItem>
                        <asp:ListItem Value="ARQUISICAO">ARQUISIÇÃO</asp:ListItem>
                        <asp:ListItem Value="EMISSAO">EMISSÃO</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div class="panelItem" style="margin-bottom: 15px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" CellPadding="5"
            AllowSorting="True" ForeColor="#333333" GridLines="Vertical" OnSorting="gridPesquisa_Sorting" >
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Numero_Documento" Text="numero_documento" Visible="true" HeaderText="Nro Documento"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/CTeEntradaDetalhes.aspx?chave={0}"
                    DataNavigateUrlFields="chave" SortExpression="Codigo" />
                <asp:HyperLinkField DataTextField="Serie" Text="Serie" Visible="true" HeaderText="Serie"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/CTeEntradaDetalhes.aspx?chave={0}"
                    DataNavigateUrlFields="chave" SortExpression="Serie">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:HyperLinkField>
                <asp:HyperLinkField DataTextField="Fornecedor" Text="Fornecedor" Visible="true" HeaderText="Fornecedor"
                    DataNavigateUrlFormatString ="~/modulos/NotaFiscal/pages/CTeEntradaDetalhes.aspx?chave={0}"
                    DataNavigateUrlFields="chave" SortExpression="Fornecedor"/>
                <asp:HyperLinkField DataTextField="Valor_Documento" Text="Valor_Documento" Visible="true" HeaderText="Total"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/CTeEntradaDetalhes.aspx?chave={0}"
                    DataNavigateUrlFields="chave" SortExpression="Valor_Documento">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:HyperLinkField>
                <asp:HyperLinkField DataTextField="Emissao" Text="Emissao" Visible="true" HeaderText="DT Emissao"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/CTeEntradaDetalhes.aspx?cchave={0}"
                    DataNavigateUrlFields="chave" SortExpression="Emissao" />
                <asp:HyperLinkField DataTextField="aquisicao" Text="Aquisicao" Visible="true" HeaderText="DT Aquisicao"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/CTeEntradaDetalhes.aspx?chave={0}"
                    DataNavigateUrlFields="chave" SortExpression="Aquisicao" />
                <asp:HyperLinkField DataTextField="Chave" Text="Chave" Visible="true" HeaderText="Chave"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/CTeEntradaDetalhes.aspx?chave={0}"
                    DataNavigateUrlFields="chave" SortExpression="Chave">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:HyperLinkField>
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
        <br />
    </div>
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
                    <asp:Label ID="LblCancelaLista" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel1" runat="server" CssClass="lista">
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
    <asp:ModalPopupExtender ID="modalFornecedor" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancelaLista" DropShadow="true" PopupControlID="pnfundo"
        TargetControlID="lblPesquisaErro">
    </asp:ModalPopupExtender>
</asp:Content>
