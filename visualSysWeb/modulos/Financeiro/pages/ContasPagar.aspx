<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ContasPagar.aspx.cs" Inherits="visualSysWeb.modulos.Financeiro.pages.ContasPagar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Contas a Pagar</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <div class="btnImprimirDireita">
        Limpar Filtros
        <asp:ImageButton ID="imgBtnLimpar" runat="server" OnClick="imgBtnLimpar_Click" ImageUrl="../../../img/botao-apagar.png"
            Style="width: 20px" />
    </div>
    <br />
    <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div class="filter" id="filtrosPesq" runat="server" visible="true">
        <table>
            <tr>
                <td>
                    <p>
                        Documento</p>
                    <asp:TextBox ID="txtDocumento" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Fornecedor</p>
                    <asp:TextBox ID="txtFornecedor" runat="server"> </asp:TextBox>
                    <asp:ImageButton ID="imgBtnFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </td>
                <td>
                    <p>
                        Valor</p>
                    <asp:TextBox ID="txtValor" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        De</p>
                    <asp:TextBox ID="txtDe" runat="server" Width="80px" CssClass="DATA" AutoPostBack="true"
                        OnTextChanged="txtDataDe_TextChanged"> </asp:TextBox>
                    <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDe">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Ate</p>
                    <asp:TextBox ID="txtAte" runat="server" Width="80px" CssClass="DATA"> </asp:TextBox>
                    <asp:ImageButton ID="ImgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataAte" runat="server" PopupButtonID="imgDtAte" TargetControlID="txtAte">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Pesquisar Por</p>
                    <asp:DropDownList ID="DllTipoPesquisa" runat="server">
                        <asp:ListItem Value="EMISSAO">EMISSAO</asp:ListItem>
                        <asp:ListItem Value="VENCIMENTO">VENCIMENTO</asp:ListItem>
                        <asp:ListItem Value="ENTRADA">ENTRADA</asp:ListItem>
                        <asp:ListItem Value="PAGAMENTO">PAGAMENTO</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <p>
                        Tipo Pagamento</p>
                    <asp:DropDownList ID="ddlTipoPagamento" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <p>
                        Status</p>
                    <asp:DropDownList ID="DllStatus" runat="server">
                        <asp:ListItem Value="" Selected="True">PREVISTO</asp:ListItem>
                        <asp:ListItem Value="1" >ABERTO</asp:ListItem>
                        <asp:ListItem Value="2">CONCLUIDO</asp:ListItem>
                        <asp:ListItem Value="3">CANCELADO</asp:ListItem>
                        <asp:ListItem Value="4">LANCADO</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td >
                    <p>
                        Centro de Custo</p>
                    <asp:TextBox ID="txtCentroCusto" runat="server" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnCentroCusto" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </td>
                <td >
                    <p>
                        Cheque</p>
                    <asp:TextBox ID="txtCheque" runat="server" Width="200px"></asp:TextBox>
                </td>
                <td>
                    <p>Lançamento Simples</p>
                     <asp:DropDownList ID="ddlSimples" runat="server">
                        <asp:ListItem Value="" Selected="True">TODOS</asp:ListItem>
                        <asp:ListItem Value="1" >SIM</asp:ListItem>
                        <asp:ListItem Value="0" >NÃO</asp:ListItem>
                     </asp:DropDownList>
                </td>
                <td colspan="7">
                    <asp:CheckBox ID="chkConferido" runat="server" Text="Conferido" 
                             />
                </td>
            </tr>
        </table>
    </div>
     <div class="panelItem" style="margin-top: 10px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="gridTable">
        <div style="width: 102px" class="titulobtn">
            <asp:Button ID="BtnBaixar" runat="server" Text="Baixar" OnClick="BtnBaixar_Click" />
        </div>
        <div class="titulobtn">
            <asp:Button ID="BtnCancelarBaixa" runat="server" Text="Estornar Baixa" OnClick="BtnBaixar_Click" />
        </div>
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" DataKeyNames="documento,fornecedor,valor,emissao,vencimento,status,id_cc"
            CellPadding="5" ForeColor="#333333" GridLines="Vertical" AllowSorting="True"
            OnSorting="gridPesquisa_Sorting">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelecionaItem" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelecionaItem_CheckedChanged" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataTextField="Documento" Text="Documento" Visible="true" HeaderText="Documento"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="Documento" />
                <asp:HyperLinkField DataTextField="Fornecedor" Text="----" Visible="true" HeaderText="Fornecedor"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="Fornecedor" />
                <asp:HyperLinkField DataTextField="Valor" Text="Valor" Visible="true" HeaderText="Valor"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="Valor" />
                <asp:HyperLinkField DataTextField="Entrada" Text="---" Visible="true" HeaderText="DT Entrada"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="entrada" />
                <asp:HyperLinkField DataTextField="Pagamento" Text="---" Visible="true" HeaderText="DT Pagamento"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="pagamento" />
                <asp:HyperLinkField DataTextField="emissao" Text="Emissao" Visible="true" HeaderText="DT Emissão"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="emissao" />
                <asp:HyperLinkField DataTextField="Vencimento" Text="Vencimento" Visible="true" HeaderText="DT Vencimento"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="vencimento" />
                <asp:HyperLinkField DataTextField="tipo_pagamento" Text="----" Visible="true" HeaderText="Tipo Pagamento"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="tipo_pagamento" />
                <asp:HyperLinkField DataTextField="status" Text="Status" Visible="true" HeaderText="Status"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="status" />
                <asp:HyperLinkField DataTextField="codigo_centro_custo" Text="---" Visible="true"
                    HeaderText="Centro Custo" DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="codigo_centro_custo" />
                <asp:HyperLinkField DataTextField="Numero_cheque" Text="---" Visible="true" HeaderText="Cheque"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="Numero_cheque" />
                <asp:HyperLinkField DataTextField="Conferido" Text="---" Visible="true" HeaderText="Conferido"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="Conferido" />
                <asp:HyperLinkField DataTextField="P_SIMPLES" Text="---" Visible="true" HeaderText="Simples"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/ContasPagarDetalhes.aspx?documento={0}&fornecedor={1}&valor={2}"
                    DataNavigateUrlFields="Documento,fornecedor,valor" SortExpression="P_SIMPLES" />
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
        <center>
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
    <asp:Panel ID="PnGrid" runat="server" CssClass="modalFormCentro" Style="display: none;
        width: 1000px;">
        <div style="background: #FFFFFF; width: 98%; margin: auto auto auto auto;">
        <asp:Label ID="lblGridtitulo" runat="server" Text="Confirma Baixa" CssClass="cabMenu"></asp:Label>
        <asp:Label ID="lblErroGrid" runat="server" Text="" ForeColor="Red" CssClass="cabMenu"
            Visible="false"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaGrid" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaGrid_Click" />
                    <asp:Label ID="Label2" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaGrid" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                        OnClick="btnCancelaGrid_Click" />
                    <asp:Label ID="Label3" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
        <div class="row" style="width: 20%;  margin-left: 40px; margin-right:60px; margin-bottom:0px; margin-top:0px; float:left;">
                Conta Corrente:
                <asp:DropDownList ID="ddlContaCorrenteTodas" OnSelectedIndexChanged="ddlContaCorrenteTodas_SelectedIndexChanged" AutoPostBack="true" runat="server">
                </asp:DropDownList>
                
                <asp:Label ID="lblBanco" runat="server" Text=""></asp:Label>

                <br />
                Data Pagamento<br />
                <asp:TextBox ID="txtDtpgTodas" runat="server" Width="80px" OnTextChanged="txtDtpgTodas_TextChanged" AutoPostBack="true"  > </asp:TextBox>
                    <asp:ImageButton ID="imgDtPgTodas" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtPgTodas" TargetControlID="txtDtpgTodas">
                    </asp:CalendarExtender>

            </div>
            <div class="row" style="width: 40%; margin-left: 0px; margin-bottom:10px; float:left;">
                <p>Observação:</p>
               <asp:TextBox ID="txtObsBaixa" runat="server" Width="500px" Height="40px" TextMode="MultiLine"></asp:TextBox>
            </div>
        <asp:Panel ID="Panel3" runat="server" CssClass="lista">
            <asp:GridView ID="GridBaixas" runat="server" CellPadding="4" ForeColor="#333333"
                GridLines="None" AutoGenerateColumns="False" OnRowDataBound="GridBaixas_RowDataBound"
                DataKeyNames="documento,fornecedor,valor,vencimento">
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
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
                <Columns>
                    <asp:BoundField DataField="documento" HeaderText="Documento" />
                    <asp:BoundField DataField="fornecedor" HeaderText="Fornecedor" />
                    <asp:BoundField DataField="Valor" HeaderText="Valor" />
                    <asp:BoundField DataField="vencimento" HeaderText="Vencimento" />
                    <asp:TemplateField HeaderText="Valor Pago">
                        <ItemTemplate>
                            <asp:TextBox ID="txtValorPago" Width="70px" CssClass="numero"  runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Data Pagamento">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDataPago" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="ImgDtPg" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                                <asp:CalendarExtender ID="clnDataPg" runat="server" PopupButtonID="ImgDtPg" TargetControlID="txtDataPago">
                                </asp:CalendarExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Conta Corrente" HeaderStyle-HorizontalAlign="Center">
                            
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlContaCorrente" runat="server" Width="100px">
                                </asp:DropDownList>
                                <asp:Label ID="lblConta" runat="server" Text='<%# Eval("id_cc") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalGrid" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnGrid" TargetControlID="Label2">
    </asp:ModalPopupExtender>
</asp:Content>
