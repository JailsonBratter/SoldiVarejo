<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ImprimeEtiquetas.aspx.cs" Inherits="visualSysWeb.modulos.Dispositivos.pages.ImprimeEtiquetas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1 >
            Impressão de Etiquetas</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnConfirmaPgFrame" runat="server" CssClass="frame">
            <asp:Label ID="lbllista" runat="server" Text="Escolha As etiquetas que serão impressas"
                CssClass="cabMenu"></asp:Label>
            <table class="frame">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaLista_Click" />
                        <asp:Label ID="Label4" runat="server" Text="Seleciona"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaLista_Click" />
                        <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged"
                                    Checked="true" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked="true" />
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
    <asp:ModalPopupExtender ID="modalEtiquetas" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
    <div class="conteudo">
        <div class="frameConteudo">
            <div class="MenuEtiqueta">
                <asp:Image ID="ImgEtiqueta" runat="server" Height="79px" Width="183px" ImageUrl="~/modulos/Dispositivos/imgs/etiqueta0.JPG" />
                <br />
                Modelo Etiqueta
                <asp:RadioButtonList ID="rdoEtiquetas" runat="server" OnSelectedIndexChanged="rdoEtiquetas_SelectedIndexChanged"
                    AutoPostBack="True">
                    <asp:ListItem Value="0" Selected="True">Gôndola</asp:ListItem>
                    <asp:ListItem Value="1">3/4 Gôndola</asp:ListItem>
                    <asp:ListItem Value="2">1/2 Gôndola</asp:ListItem>
                    <asp:ListItem Value="3">Produto</asp:ListItem>
                    <asp:ListItem Value="4">Produto Unidade</asp:ListItem>
                    <asp:ListItem Value="5">Promoção</asp:ListItem>
                    <asp:ListItem Value="6">Oferta</asp:ListItem>
                    <asp:ListItem Value="7">1/2 Gôndola Simples</asp:ListItem>
                    <asp:ListItem Value="8">Produto Simples</asp:ListItem>
                    <asp:ListItem Value="9">Produto Validade</asp:ListItem>
                    <asp:ListItem Value="10">Vertical</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="MenuEtiquetaDireita">
                <div class="filter" id="filtrosPesq" runat="server">
                    <table>
                        <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                        <br />
                        <asp:Label ID="lblPesquisaFiltro" runat="server" Text="" ForeColor="Blue"></asp:Label>
                        <tr>
                            <td colspan="2">
                                <p>
                                    Plu / EAN
                                </p>
                                <asp:TextBox ID="txtPluEan" runat="server" Width="286px"></asp:TextBox>
                            </td>
                            <td colspan="3">
                                <p>
                                    Descrição</p>
                                <asp:TextBox ID="txtDescricao" runat="server" Width="300"> </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    NF</p>
                                <asp:TextBox ID="txtNF" runat="server" Width="100px"> </asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    FORNECEDOR</p>
                                <asp:TextBox ID="txtFornecedor" runat="server"> </asp:TextBox>
                                <asp:ImageButton ID="btnimg_txtFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgLista_Click" />
                            </td>
                            <td>
                                <p>
                                    FAMILIA</p>
                                <asp:TextBox ID="txtfamilia" runat="server" Width="100px"> </asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Data Cadastro</p>
                                <asp:TextBox ID="txtDtCadastro" runat="server" Width="80px"> </asp:TextBox>
                                <asp:ImageButton ID="ImgDtCadastro" ImageUrl="~/img/calendar.png" runat="server"
                                    Height="15px" />
                                <asp:CalendarExtender ID="clnDataCadastro" runat="server" PopupButtonID="ImgDtCadastro"
                                    TargetControlID="txtDtCadastro" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <p>
                                    Data Alteração</p>
                                <asp:TextBox ID="txtDtalteracao" runat="server" Width="80px"> </asp:TextBox>
                                <asp:ImageButton ID="ImgDtAlteracao" ImageUrl="~/img/calendar.png" runat="server"
                                    Height="15px" />
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="ImgDtAlteracao"
                                    TargetControlID="txtDtalteracao" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkImprimeEtiqueta" Text="Só Alterados " runat="server" Checked="false" />
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="chkDesmarcarImprime" Text="Desmarcar Imprime Etiqueta " runat="server"
                                    Checked="false" AutoPostBack="true" OnCheckedChanged="chkDesmarcarImprime_CheckedChanged" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Panel ID="pnLimpar" runat="server" Visible="true" CssClass="titulobtn">
                    <asp:ImageButton ID="ImgBtnLimpar" runat="server" Height="25px" ImageUrl="~/img/botao-apagar.png"
                        Width="27px" OnClick="ImgBtnLimpar_Click" />
                    Limpar
                </asp:Panel>
                <asp:Panel ID="pnImprimir" runat="server" Visible="true" CssClass="titulobtn">
                    <asp:ImageButton ID="imgBtnImprimir" runat="server" Height="25px" ImageUrl="../../../img/icon_imprimir.gif"
                        Width="27px" OnClick="imgBtnImprimir_Click" />
                    Imprimir
                </asp:Panel>
                <div class="gridTableEtiqueta">
                    <asp:GridView ID="gridImpressao" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="gridPesquisa_PageIndexChanging"
                        CellPadding="6" ForeColor="#333333" GridLines="Vertical" Width="100%" OnRowCommand="gridImpressao_RowCommand">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField></asp:TemplateField>
                            <asp:HyperLinkField DataTextField="plu" Text="PLU" Visible="true" HeaderText="PLU" />
                            <asp:HyperLinkField DataTextField="EAN" Text="EAN" Visible="true" HeaderText="EAN" />
                            <asp:HyperLinkField DataTextField="descricao" Text="Descrição" Visible="true" HeaderText="Descrição" />
                            <asp:HyperLinkField DataTextField="preco" Text="Descrição" Visible="true" HeaderText="Preço">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataTextField="preco_promocao" Text="Descrição" Visible="true"
                                HeaderText="Promoção">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:HyperLinkField>
                            <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/plus.png" CommandName="mais"
                                Text="+">
                                <ControlStyle Height="15px" Width="15px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:ButtonField>
                            <asp:TemplateField HeaderText="Qde" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txQtde" runat="server" Text='<%# Eval("Qtd") %>' Width="80px" CssClass="numero"
                                        onkeypress="return numeros(this,event);" OnTextChanged="txtQtde_textChanged"
                                        AutoPostBack="True"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/remove.png" CommandName="menos"
                                Text="-">
                                <ControlStyle Height="15px" Width="15px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:ButtonField>
                            <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="excluir"
                                Text="X">
                                <ControlStyle ForeColor="Red" Height="20px" Width="20px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:ButtonField>
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
                <asp:Label ID="lblImpressao" runat="server" Text=""></asp:Label></center>
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="pnFundoFornecedor" runat="server" CssClass="modalForm" Style="display: none"
        DefaultButton="ImgPesquisaLista">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px;">
            <center><h1><asp:Label ID="lbltitulofornecedor" runat="server" Text="ESCOLHA O FORNECEDOR"></asp:Label></h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                Filtrar
                <asp:TextBox ID="TxtPesquisaFornecedor" runat="server" Width="400px"></asp:TextBox>
                <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="ImgPesquisaLista_Click" />
                <asp:Label ID="lblErroPesquisa" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="btnConfirmaFornecedor" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaFornecedor_Click" />
                    <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaFornecedor" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaFornecedor_Click" />
                    <asp:Label ID="Label2" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel3" runat="server" CssClass="lista">
                <asp:GridView ID="GridFornecedor" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" OnRowDataBound="GridLista_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="RdoFornecedorItem" runat="server" GroupName="GrFornecedorItem" />
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
    <asp:ModalPopupExtender ID="modalListafornecedor" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnFundoFornecedor" TargetControlID="lbltitulofornecedor">
    </asp:ModalPopupExtender>
    <br />
    <br />
    <br />
</asp:Content>
