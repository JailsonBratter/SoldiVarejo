<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="NfEntrada.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.pages.NfEntrada"
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            NOTA FISCAL DE ENTRADA</h1>
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
                        <asp:ListItem Value="DATA">DATA</asp:ListItem>
                        <asp:ListItem Value="EMISSAO">EMISSAO</asp:ListItem>
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
            AllowSorting="True" ForeColor="#333333" GridLines="Vertical" OnSorting="gridPesquisa_Sorting" OnRowCommand="gridPesquisa_RowCommand">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Codigo" Text="Codigo" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?codigo={0}&fornecedor={1}&serie={2}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor,serie" SortExpression="Codigo" />
                <asp:HyperLinkField DataTextField="Serie" Text="Serie" Visible="true" HeaderText="Serie"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?codigo={0}&fornecedor={1}&serie={2}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor,serie" SortExpression="Serie">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:HyperLinkField>
                <asp:HyperLinkField DataTextField="cliente_fornecedor" Text="cliente_fornecedor"
                    Visible="true" HeaderText="Fornecedor" DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?codigo={0}&fornecedor={1}&serie={2}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor,serie" SortExpression="Cliente_fornecedor" />
                <asp:HyperLinkField DataTextField="data" Text="Data" Visible="true" HeaderText="DT Entrada"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?codigo={0}&fornecedor={1}&serie={2}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor,serie" SortExpression="Data" />
                <asp:HyperLinkField DataTextField="Emissao" Text="Emissao" Visible="true" HeaderText="DT Emissao"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?codigo={0}&fornecedor={1}&serie={2}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor,serie" SortExpression="Emissao" />
                <asp:HyperLinkField DataTextField="Total" Text="Total" Visible="true" HeaderText="Total"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?codigo={0}&fornecedor={1}&serie={2}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor,serie" SortExpression="Total">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:HyperLinkField>
                <asp:ButtonField ButtonType="Image" ImageUrl="~/modulos/NotaFiscal/imgs/nfmenu.jpg" CommandName="Fiscal" 
                    HeaderText="Fiscal" Text="Fiscal" ItemStyle-Width="40px">
                </asp:ButtonField>
                <asp:BoundField DataField="ValidadoFiscal" Visible="true" HeaderText="Validado" ItemStyle-Width="200"/>
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
