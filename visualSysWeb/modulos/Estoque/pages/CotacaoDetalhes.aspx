<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CotacaoDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Estoque.pages.CotacaoDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center> <h1>Detalhes da Cotação</h1></center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <!--Coloque aqui os campos do cabecalho    -->
        <table>
            <tr>
                <td>
                    <p>
                        Cotacao
                    </p>
                    <asp:TextBox ID="txtCotacao" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Descrição
                    </p>
                    <asp:TextBox ID="txtdescricao" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Data
                    </p>
                    <asp:TextBox ID="txtData" runat="server" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="imgDtData" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtData"
                        TargetControlID="txtData">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Status
                    </p>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>ABERTO</asp:ListItem>
                        <asp:ListItem>FECHADA</asp:ListItem>
                        <asp:ListItem>FINALIZADA</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <p>
                        Usuario
                    </p>
                    <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Button ID="btnFinalizar" runat="server" Text="Finalizar" OnClick="btnFinalizar_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo">
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
                            AutoGenerateColumns="False" OnRowCommand="gridItens_RowCommand" CssClass="table">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/Cancel.png" CommandName="Alterar"
                                    Text="Excluir">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="Mercadoria" HeaderText="Plu"></asp:BoundField>
                                <asp:BoundField DataField="EAN" HeaderText="EAN"></asp:BoundField>

                                <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                                <asp:TemplateField HeaderText="Quantidade">
                                    <ItemTemplate>
                                        <center><asp:TextBox ID="txtQtdItem" runat="server" Text='<%# Eval("Quantidade") %>' Width="80px"
                                            CssClass="numero" OnTextChanged="txtQtdItem_TextChanged" OnKeyPress="javascript:return autoTab(this,event);"  ></asp:TextBox></center>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Embalagem">
                                    <ItemTemplate>
                                        <center><asp:TextBox ID="txtEmbalagem" runat="server" Text='<%# Eval("Embalagem") %>' Width="80px"
                                            CssClass="numero" OnTextChanged="txtEmbalagemItem_TextChanged" OnKeyPress="javascript:return autoTab(this,event);"  ></asp:TextBox></center>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="Vencedor" HeaderText="Vencedor">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Preco_Compra" HeaderText="Preço Compra">
                                    <HeaderStyle HorizontalAlign="Right" />
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
                    Mercadorias
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="Div300-400">
                        <div class="gridTable">
                            <asp:GridView ID="GridItensMercadoria" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" ForeColor="#333333" GridLines="Vertical" OnRowDataBound="GridItensMercadoria_RowDataBound">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton ID="RdoItem" runat="server" GroupName="Gritem" OnCheckedChanged="RdoItem_CheckedChanged"
                                                AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Mercadoria" HeaderText="Plu" />
                                    <asp:BoundField DataField="Descricao" HeaderText="Descrição" />
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
                    </div>
                    <div class="Div60porc">
                        <div class="Div300-650">
                            <div class="gridTable">
                                <asp:GridView ID="gridFornecedor" runat="server" ForeColor="#333333" GridLines="Vertical"
                                    AutoGenerateColumns="False" CssClass="table">
                                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Fornecedor" HeaderText="Fornecedor"></asp:BoundField>
                                        <asp:BoundField DataField="Qtde" HeaderText="Qtde" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                        <asp:BoundField DataField="Embalagem" HeaderText="Embalagem" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                        <asp:BoundField DataField="Preco" HeaderText="Preco" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                        <asp:BoundField DataField="Prazo_pgto" HeaderText="Prazo Pgto" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                        <asp:BoundField DataField="Prazo_Entrega" HeaderText="Prazo Entrega" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                        <asp:BoundField DataField="Usuario" HeaderText="Usuario"></asp:BoundField>

                                        <asp:TemplateField HeaderText="OBS">

                                            <ItemTemplate>
                                                <asp:LinkButton Text='<%# Eval("OBS") %>' runat="server" OnClick="linkObs_Click" />
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
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel3">
                <HeaderTemplate>
                    Pedido
                </HeaderTemplate>
                <ContentTemplate>
                    <div id="divEnviarTodosEmails" runat="server" class="btnImprimirDireita" style="padding: 20px; font-size: 30px; margin-right: 30px;">
                        <asp:ImageButton ID="imgBtnEnviarTodosEmails" ImageUrl="../../../img/email.png" runat="server" OnClick="imgBtnEnviarTodosEmails_Click" />
                        <span style="padding-top: 20px; margin-top: -10px">Enviar Todos</span>
                    </div>
                    <div class="gridTable">
                        <asp:GridView ID="gridPedido" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
                            GridLines="Vertical" OnRowCommand="gridPedido_RowCommand">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:HyperLinkField DataTextField="Pedido" Text="----" HeaderText="Pedido" DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoCompraDetalhes.aspx?campoIndex={0}&antigapg=~/modulos/Estoque/pages/CotacaoDetalhes.aspx?campoIndex={1}"
                                    DataNavigateUrlFields="pedido,cotacao" />
                                <asp:HyperLinkField DataTextField="Fornecedor" Text="---" HeaderText="Fornecedor"
                                    DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoCompraDetalhes.aspx?campoIndex={0}&antigapg=~/modulos/Estoque/pages/CotacaoDetalhes.aspx?campoIndex={1}"
                                    DataNavigateUrlFields="pedido,cotacao" />
                                <asp:HyperLinkField DataTextField="Total" Text="---" HeaderText="Total" DataNavigateUrlFormatString="~/modulos/Pedidos/pages/pedidoCompraDetalhes.aspx?campoIndex={0}&antigapg=~/modulos/Estoque/pages/CotacaoDetalhes.aspx?campoIndex={1}"
                                    DataNavigateUrlFields="pedido,cotacao" />
                                <asp:ButtonField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ButtonType="Image" ImageUrl="../../../img/email.png" CommandName="Email"
                                    HeaderText="Enviar Email" Text="Email"></asp:ButtonField>
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
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabItensNaoCotados">
                <HeaderTemplate>
                    Itens não cotados
                </HeaderTemplate>
                <ContentTemplate>
                    <div id="divBtnImprimirNaoCotados" runat="server" class="btnImprimirDireita">
                        <asp:ImageButton ID="imgBtnImpNaoCotados" runat="server" ImageUrl="~/img/icon_imprimir.gif"
                            Height="50px" OnClick="imgBtnImpNaoCotados_Click" />
                    </div>
                    <div class="gridTable">
                        <asp:GridView ID="gridItensNaoCotados" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
                            GridLines="Vertical">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="Mercadoria" HeaderText="Plu" />
                                <asp:BoundField DataField="Descricao" HeaderText="Descrição" />
                                <asp:BoundField DataField="Quantidade" HeaderText="Quantidade" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                <asp:BoundField DataField="Embalagem" HeaderText="Embalagem" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
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

        </asp:TabContainer>
    </div>
    <asp:Panel ID="pnMercadoriaLista" runat="server" CssClass="modalFormTelaCheia" Style="display: none">
        <asp:Panel ID="pnMercadoriaListaFrame" runat="server" CssClass="modalFormTelaCheiaframeDiv"
            DefaultButton="imgPesquisaMercadoria">
            <asp:Label ID="lblMercadoriaLista" runat="server" Text="Inclusão de Produto" CssClass="cabMenuLista"></asp:Label>
            <table class="tabelaLista">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnFecharMecadoria" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnFecharMecadoria_Click" />
                        <asp:Label ID="Label10" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelarMercadoria" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaMercadoria_Click" />
                        <asp:Label ID="Label11" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Panel ID="PnFiltro" runat="server" CssClass="modalFormTelaCheiaframeMini">
                <table>
                    <tr>
                        <td colspan="2">
                            <center><b>Filtrar</b></center>
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>Grupo:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGrupo" runat="server" AutoPostBack="true" Width="200px"
                                OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>SubGrupo:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSubGrupo" runat="server" AutoPostBack="true" Width="200px"
                                OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Departamento:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="true" Width="200px"
                                OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Plu/EAN/Descricao:
                        </td>
                        <td>
                            <asp:TextBox ID="txtfiltromercadoria" runat="server" Width="200px" OnTextChanged="txtfiltromercadoria_TextChanged"
                                autocomplete="off"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ImageButton ID="imgPesquisaMercadoria" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="20px" OnClick="ImgPesquisaMercadoria_Click" />
                            Filtrar
                        </td>
                        <td>
                            <asp:ImageButton ID="imgLimpar" runat="server" ImageUrl="~/img/botao-apagar.png"
                                Height="20px" OnClick="imgLimpar_Click" />
                            Limpar
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnGridMercadoria1" runat="server" CssClass="modalFormTelaCheiaframe">
                <div class="gridTableTelaCheia">
                    <asp:GridView ID="gridMercadoria1" runat="server" CellPadding="4" ForeColor="#333333"
                        GridLines="None" OnRowDataBound="GridMercadoria1_RowDataBound" Width="115%">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="true" OnCheckedChanged="chkSeleciona_CheckedChanged" />
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
                </div>
            </asp:Panel>
            <div class="btnImprimirDireita">
                <asp:ImageButton ID="imgBtnIncluirSelecionados" runat="server" ImageUrl="~/img/add.png"
                    Height="20px" Width="20px" OnClick="imgBtnIncluirSelecionados_Click" />Incluir
                item
            </div>
            <br />
            <center><h3><b>Selecionados </b></h3></center>
            <asp:Panel ID="pnGridMercadoriasSelecionado" runat="server" CssClass="modalFormTelaCheialistaDiv">
                <div class="gridTableTelaCheia">
                    <asp:GridView ID="GridMercadoriaSelecionado" runat="server" CellPadding="4" ForeColor="#333333"
                        GridLines="None" AutoGenerateColumns="false" OnRowCommand="GridMercadoriaSelecionado_RowCommand"
                        OnRowDataBound="GridMercadoriaSelecionado_RowDataBound">
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="Alterar"
                                Text="Alterar">
                                <ControlStyle Height="20px" Width="20px" />
                                <ItemStyle Width="20px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="Plu" HeaderText="PLU"></asp:BoundField>
                            <asp:BoundField DataField="Ean" HeaderText="EAN"></asp:BoundField>
                            <asp:BoundField DataField="Referencia" HeaderText="Referencia"></asp:BoundField>
                            <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                            <asp:TemplateField HeaderText="Qtd">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQtd" runat="server" Text='<%# Eval("QTD") %>' Width="30px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Emb">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtEmb" runat="server" Text='<%# Eval("embalagem") %>' Width="30px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="preco" HeaderText="Preco"></asp:BoundField>
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
                </div>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalMercadorialista" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnMercadoriaLista" TargetControlID="lblMercadoriaLista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnEncerrar" runat="server" CssClass="frameModal" Style="display: none">
        <div class="btnImprimirDireita">
            <asp:ImageButton ID="imgFechar" runat="server" ImageUrl="~/img/cancel.png" Width="15px" />Fechar
        </div>
        <h2>
            <asp:Label ID="Label14" runat="server" Text="Deseja finalizar gerando apenas um pedido para o grande vencedor?"
                CssClass="cabMenu"></asp:Label>
        </h2>
        <table style="width: 100%">
            <tr>
                <td>
                    <center>
                    <asp:ImageButton ID="btnGrandeVencedor" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnGrandeVencedor_Click" />
                    <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                 </center>
                </td>
                <td>
                    <center>
                    <asp:ImageButton ID="btnMelhoresPrecos" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnMelhoresPrecos_Click" />
                    <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                    </center>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalEncerrar" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnEncerrar" TargetControlID="Label14">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirmaEmail" runat="server" CssClass="frameModal" Style="display: none">
        <div class="btnImprimirDireita">
            <asp:ImageButton ID="imgFecharEmail" runat="server" ImageUrl="~/img/cancel.png" Width="15px" />Fechar
        </div>
        <h2>
            <asp:Label ID="lblPedidoEmail" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblMensagemEmail" runat="server" Text="Tem Certeza que gostaria de Enviar o email "
                CssClass="cabMenu"></asp:Label>
        </h2>
        <table style="width: 100%">
            <tr>
                <td>
                    <center>
                    <asp:ImageButton ID="btnEnviarEmail" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnEnviarEmail_Click" />
                    <asp:Label ID="lblSimEmail" runat="server" Text="SIM"></asp:Label>
                 </center>
                </td>
                <td>
                    <center>
                    <asp:ImageButton ID="btnCancelaEmail" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancela_Click" />
                    <asp:Label ID="lblNaoEmail" runat="server" Text="NÃO"></asp:Label>
                    </center>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaEmail" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaEmail" TargetControlID="lblMensagemEmail"
        CancelControlID="imgFecharEmail">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnObsFornecedor" runat="server" CssClass="frameModal" Style="display: none; height: 250px; width: 500px; padding: 10px;">

        <div class="btnImprimirDireita">
            <br />
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/cancel.png" Width="15px" />Fechar
        </div>
        <h2>Observações do Fornecedor</h2>
        <hr />
        <div class="frame" style="height: 200px; font-size: large; overflow: auto;">
            <asp:Label ID="lblObsFornecedor" Text="" runat="server" />
        </div>

    </asp:Panel>
    <asp:ModalPopupExtender ID="modalObsFornecedor" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnObsFornecedor" TargetControlID="lblObsFornecedor"
        CancelControlID="ImageButton1">
    </asp:ModalPopupExtender>
</asp:Content>
