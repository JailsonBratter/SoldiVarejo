<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PrecificacaoNF.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.pages.PrecificacaoNF"
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            PRECIFICAÇÃO POR NOTA FISCAL <br />
            <asp:Label ID="txtAviso" Text="" runat="server" ForeColor="Red"></asp:Label>
        </h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server" visible="True">
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
                        Serie</p>
                    <asp:TextBox ID="txtSerie" runat="server" AutoPostBack="true" OnTextChanged="txtSerie_TextChanged"></asp:TextBox>
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
                    <asp:TextBox ID="txtDe" runat="server" Width="100px"> </asp:TextBox>
                    <asp:Image ID="ImgDeCalendario" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDeCalendario"
                        TargetControlID="txtDe">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Ate</p>
                    <asp:TextBox ID="txtAte" runat="server" Width="100px"> </asp:TextBox>
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
                        <asp:ListItem Value="DATA_PRECIFICACAO">DATA PRECIFICAÇÃO</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <p>
                        Estado</p>
                    <asp:DropDownList ID="ddlEstado" runat="server">
                        <asp:ListItem Value="0">PENDENTE</asp:ListItem>
                        <asp:ListItem Value="1">PRECIFICADO</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            PageSize="20" OnPageIndexChanging="gridPesquisa_PageIndexChanging" CellPadding="5"
            ForeColor="#333333" GridLines="Vertical">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Codigo" Text="Codigo" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/PrecificacaoNFDetalhes.aspx?codigo={0}&fornecedor={1}&serie={2}"
                    DataNavigateUrlFields="Codigo,Fornecedor,Serie" />
                <asp:HyperLinkField DataTextField="Serie" Text="Serie" Visible="true" HeaderText="Serie"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/PrecificacaoNFDetalhes.aspx?codigo={0}&fornecedor={1}&serie={2}"
                    DataNavigateUrlFields="Codigo,Fornecedor,Serie" />
                <asp:HyperLinkField DataTextField="cliente_fornecedor" Text="cliente_fornecedor"
                    Visible="true" HeaderText="Fornecedor" DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/PrecificacaoNFDetalhes.aspx?codigo={0}&fornecedor={1}&serie={2}"
                    DataNavigateUrlFields="Codigo,Fornecedor,Serie" />
                <asp:HyperLinkField DataTextField="Data" Text="Data" Visible="true" HeaderText="Data"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/PrecificacaoNFDetalhes.aspx?codigo={0}&fornecedor={1}&serie={2}"
                    DataNavigateUrlFields="Codigo,Fornecedor,Serie" />
                <asp:HyperLinkField DataTextField="Emissao" Text="Emissao" Visible="true" HeaderText="Emissao"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/PrecificacaoNFDetalhes.aspx?codigo={0}&fornecedor={1}&serie={2}"
                    DataNavigateUrlFields="Codigo,Fornecedor,Serie" />
                <asp:HyperLinkField DataTextField="Total" Text="Total" Visible="true" HeaderText="Total"
                    DataTextFormatString="{0:n}" DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/PrecificacaoNFDetalhes.aspx?codigo={0}&fornecedor={1}&serie={2}"
                    DataNavigateUrlFields="Codigo,Fornecedor,Serie" ItemStyle-HorizontalAlign="Right" />
                <asp:HyperLinkField DataTextField="Estado" Text="Estado" Visible="true" HeaderText="Estado"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/PrecificacaoNFDetalhes.aspx?codigo={0}&fornecedor={1}&serie={2}"
                    DataNavigateUrlFields="Codigo,Fornecedor,Serie" />
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
                        Width="25px" OnClick="btnConfirmaLista_Click" />
                    <asp:Label ID="Label2" runat="server" Text="Confirma"></asp:Label>
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
    <asp:ModalPopupExtender ID="modalFornecedor" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancelaLista" DropShadow="true" PopupControlID="pnfundo"
        TargetControlID="lblErroPesquisa">
    </asp:ModalPopupExtender>
</asp:Content>