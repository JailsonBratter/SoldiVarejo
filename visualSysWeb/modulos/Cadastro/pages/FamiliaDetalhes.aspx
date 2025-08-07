<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FamiliaDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.FamiliaDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes da familia</h1>
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
                <td>
                    <p>
                        Codigo</p>
                    <asp:TextBox ID="txtCodigo_familia" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Descrição</p>
                    <asp:TextBox ID="txtDescricao_Familia" runat="server" Width="201px" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                <HeaderTemplate>
                    Itens
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="addItens" runat="server" CssClass="titulobtn">
                        <asp:ImageButton ID="ImgBtnAddItens" runat="server" ImageUrl="~/img/add.png" Height="20px"
                            Width="20px" OnClick="ImgBtnAddItens_Click" />
                        Incluir item
                    </asp:Panel>
                    <div class="gridTable">
                        <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Vertical"
                            AutoGenerateColumns="False" OnRowDataBound="gridItens_RowDataBound" OnRowCommand="gridItens_RowCommand"
                            CssClass="table">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" Text="Excluir">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                                <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                                <asp:BoundField DataField="Custo" HeaderText="Custo">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Margem" HeaderText="Margem">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="preco" HeaderText="Preço">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="promocao" HeaderText="Promocao">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
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
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel2">
                <HeaderTemplate>
                    Detalhes
                </HeaderTemplate>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <p>
                                    Preço Familia</p>
                                <asp:TextBox ID="txtPrecoFamilia" runat="server" Width="80px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Qtd de Etiquetas</p>
                                <asp:TextBox ID="txtQtdEtiquetas" runat="server" Width="50px"></asp:TextBox>
                            </td>
                            <td>
                                <div style="border-style: solid; border-width: 1px;">
                                    <asp:CheckBox ID="chkImprimeEtiquetaItens" runat="server" Text="Imprimir Etiquetas dos Itens" />
                                    <br />
                                    <asp:CheckBox ID="chkAplicarTodasFilias" runat="server" Text="Aplicar a Todas as Filias" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </div>
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Label ID="lbllista" runat="server" Text="Label" CssClass="frameDiv"></asp:Label>
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
        <asp:Panel ID="Panel1" runat="server" CssClass="lista">
            <asp:RadioButtonList ID="chkLista" runat="server" Height="50" Width="200">
            </asp:RadioButtonList>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pnConfirma" runat="server" CssClass="modalForm" Style="display: none">
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
    <asp:ModalPopupExtender ID="modalPnConfirma" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirma" TargetControlID="Label1">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnMercadoriaLista" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnMercadoriaListaFrame" runat="server" CssClass="frameDiv" DefaultButton="imgPesquisaMercadoria" Style="font-size: 20px;">
            <center><h1><asp:Label ID="lblMercadoriaLista" runat="server" Text="Selecione os Produtos" ></asp:Label></h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
            
            Filtrar
            <asp:TextBox ID="txtfiltromercadoria" runat="server" Width="400px" 
                OnTextChanged="txtfiltromercadoria_TextChanged"></asp:TextBox>
            <asp:ImageButton ID="imgPesquisaMercadoria" runat="server" ImageUrl="~/img/pesquisaM.png"
                Height="15px" OnClick="ImgPesquisaMercadoria_Click" />
                    <asp:Label ID="lblErroPesquisa" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                        <asp:ImageButton ID="btnFecharMecadoria" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnFecharMecadoria_Click" />
                        <asp:Label ID="Label10" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                        <asp:ImageButton ID="btnCancelarMercadoria" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaMercadoria_Click" />
                        <asp:Label ID="Label11" runat="server" Text="Cancela"></asp:Label>
                    </div>
            </div>
            
            <asp:Panel ID="pnGridMercadoria1" runat="server" CssClass="listaDiv">
                <asp:GridView ID="gridMercadoria1" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelecionaItem" runat="server" />
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
            <asp:Panel ID="Panel2" runat="server" CssClass="frameCentral">
                <table>
                    <tr>
                        <td>
                            <p>
                                Adicionar</p>
                            <asp:ImageButton ID="AddSelecionado" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                Width="20px" OnClick="ImgBtnAddSelecionado_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnGridMercadoriasSelecionado" runat="server" CssClass="listaDiv">
                <asp:GridView ID="GridMercadoriaSelecionado" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" AutoGenerateColumns="false" OnRowCommand="GridMercadoriaSelecionado_RowCommand">
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="Excluir"
                            Text="Excluir">
                            <ControlStyle Height="20px" Width="20px" />
                            <ItemStyle Width="20px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="Plu" HeaderText="PLU"></asp:BoundField>
                        <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                        <asp:BoundField DataField="custo" HeaderText="Custo"></asp:BoundField>
                        <asp:BoundField DataField="Margem" HeaderText="Margem"></asp:BoundField>
                        <asp:BoundField DataField="preco" HeaderText="Preco"></asp:BoundField>
                        <asp:BoundField DataField="promocao" HeaderText="promocao"></asp:BoundField>
                    </Columns>
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
                </asp:GridView>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalMercadorialista" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnMercadoriaLista" TargetControlID="lblMercadoriaLista">
    </asp:ModalPopupExtender>
</asp:Content>
