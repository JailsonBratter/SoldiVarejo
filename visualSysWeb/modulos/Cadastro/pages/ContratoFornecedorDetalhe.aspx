<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ContratoFornecedorDetalhe.aspx.cs" Inherits="visualSysWeb.modulos.Manutencao.pages.ContratoFornecedorDetalhe" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center><h1>Detalhes do Contrato Fornecedor</h1></center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <table>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Codigo</p>
                        <asp:TextBox ID="txtid_contrato" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Descrição</p>
                        <asp:TextBox ID="txtDescricao" runat="server" MaxLength="40" Width="450px" Style="font-size: 20px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Fornecedor</p>
                        <asp:TextBox ID="txtfornecedor" runat="server" Width="400px" AutoPostBack="true"
                            OnTextChanged="txtfornecedor_TextChanged" Style="font-size: 20px"></asp:TextBox>
                        <asp:ImageButton ID="imgFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" Width="15px" OnClick="Img_Click" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Vigência De</p>
                        <asp:TextBox ID="txtData_inicio" runat="server" Width="100px" CssClass="DATA" Style="font-size: 20px"></asp:TextBox>
                        <asp:ImageButton ID="imgDtInicio" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="clnDataDeInicio" runat="server" PopupButtonID="imgDtInicio"
                            TargetControlID="txtData_inicio">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p>
                            Ate</p>
                        <asp:TextBox ID="txtdata_validade" runat="server" Width="100px" CssClass="DATA" Style="font-size: 20px"></asp:TextBox>
                        <asp:ImageButton ID="ImgDtDeValidade" ImageUrl="~/img/calendar.png" runat="server"
                            Height="15px" />
                        <asp:CalendarExtender ID="clnDataDeValidade" runat="server" PopupButtonID="imgDtDeValidade"
                            TargetControlID="txtdata_validade">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p>
                            Forma de Pagamento</p>
                        <asp:TextBox ID="txtprazo" runat="server" Width="200px" Style="font-size: 20px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Valor Minimo</p>
                        <asp:TextBox ID="txtQtdeminima" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>Tipo Reajuste</p>
                        <asp:DropDownList ID="ddlTipoReajuste" runat="server" AutoPostBack="true"
                            onselectedindexchanged="ddlTipoReajuste_SelectedIndexChanged">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Mes</asp:ListItem>
                                <asp:ListItem>Ano</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p><asp:Label ID="lblDiaMesReajuste" runat="server" Text="Mes"></asp:Label></p>
                        <asp:TextBox ID="txtDiaMesReajuste" runat="server" Width="80px" CssClass="numero" MaxLength="2"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Forma de Reajuste</p>
                        <asp:TextBox ID="txtFormaDeReajuste" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Prazo Entrega</p>
                        <asp:TextBox ID="txtPrazoEntrega" runat="server" Width="80px" CssClass="numero" Style="font-size: 20px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Data Cadastro</p>
                        <asp:TextBox ID="txtdata_Cadastro" runat="server" Width="100px"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" runat="server">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" AutoPostBack="true" OnActiveTabChanged="TabContainer1_ActiveTabChanged">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                <HeaderTemplate>
                    Itens Selecionados
                </HeaderTemplate>
                <ContentTemplate>
                     <asp:Panel ID="addItensDig" runat="server" CssClass="filter" DefaultButton="imgPlu">
                            <table>
                                <tr>
                                    <td>
                                        <p>
                                            PLU/EAN</p>
                                        <asp:TextBox ID="txtPlu" runat="server" Width="150px"></asp:TextBox><asp:ImageButton
                                            ID="imgPlu" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" Width="15px"
                                            OnClick="imgPlu_Click" />
                                    </td>
                                    <td>
                                        <p>
                                            Descrição</p>
                                        <asp:TextBox ID="txtDescricaoItem" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                    </td> 
                                    <td>
                                        <p>
                                            Und</p>
                                        <asp:TextBox ID="txtUndItem" runat="server" Width="50px" Enabled="False"></asp:TextBox>
                                    </td>
                                   
                                    <td>
                                        <p>
                                            Valor</p>
                                        <asp:TextBox ID="txtValor" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                              
                                    <td>
                                        <asp:ImageButton ID="imgBtnAddItemRapido" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                            Width="20px" OnClick="imgBtnAddItemRapido_Click" />Incluir item
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <div class="panelItem" style="margin-bottom: 10px; margin-left: 30px;">
                            <h1>
                                <asp:Label ID="lblRegistros" runat="server"></asp:Label></h1>
                        </div>
                        <div class="direitaFechar">
                            <asp:Button ID="btnItensDigitados" runat="server" Text="Digitados" Font-Size="Large" OnClick="btnDigitado_Click" />
                            <asp:Button ID="btnPagInicio" runat="server" Text="<<" Font-Size="Large" OnClick="btnPag_Click" />
                            <asp:Button ID="btnPagAnterio" runat="server" Text="<" Font-Size="Large" OnClick="btnPag_Click" />
                            <asp:Button ID="btnPagProximo" runat="server" Text=">" Font-Size="Large" OnClick="btnPag_Click" />
                            <asp:Button ID="btnPagFim" runat="server" Text=">>" Font-Size="Large" OnClick="btnPag_Click" />
                        </div>
                    <div class="gridTable">
                       
                        <asp:GridView ID="gridItens" runat="server" AutoGenerateColumns="False" CellPadding="6"
                            Width="100%" ForeColor="#333333" GridLines="Vertical" OnRowDataBound="GridItens_RowDataBound">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="PLU" HeaderText="PLU" ReadOnly="True"></asp:BoundField>
                                <asp:BoundField DataField="Descricao" HeaderText="Descrição" ReadOnly="True"></asp:BoundField>
                                <asp:BoundField DataField="und" HeaderText="UND" ReadOnly="True"></asp:BoundField>
                                <asp:TemplateField HeaderText="Valor">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValorItem" runat="server" Text='<%# Eval("vlr","{0:N4}") %>'
                                            Width="100px" CssClass="numero" onkeypress="return numeros(this,event);"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" />
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
                  
                </ContentTemplate>
            </asp:TabPanel>
           
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel4">
                <HeaderTemplate>
                    Selecionar Itens</HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="pnMercadoriaListaFrame" runat="server" CssClass="modalFormTelaCheiaframeDiv"
                        Style="height: 700px;" DefaultButton="imgPesquisaMercadoria">
                        <asp:Label ID="lblMercadoriaLista" runat="server" Text="Inclusão de Produto" CssClass="cabMenuLista"></asp:Label><br />
                        <div id="Div1" runat="server" class="modalFormTelaCheiaframeMini" style="height: 250px;">
                            <table>
                                <tr>
                                    <td colspan="3">
                                        <center><b>Filtrar</b></center>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Grupo:
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlGrupo" runat="server" AutoPostBack="True" Width="200px"
                                            OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SubGrupo:
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlSubGrupo" runat="server" AutoPostBack="True" Width="200px"
                                            OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Departamento:
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="True" Width="200px"
                                            OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Linha
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlLinha" runat="server" AutoPostBack="True" Width="200px"
                                            OnSelectedIndexChanged="ddlLinha_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Plu/EAN/Descricao:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtfiltromercadoria" runat="server" Width="200px" OnTextChanged="txtfiltromercadoria_TextChanged"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="imgPesquisaMercadoria" runat="server" ImageUrl="~/img/pesquisaM.png"
                                            Height="20px" OnClick="ImgPesquisaMercadoria_Click" />Filtrar
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="imgLimpar" runat="server" ImageUrl="~/img/botao-apagar.png"
                                            Height="20px" OnClick="imgLimpar_Click" />Limpar
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:Panel ID="pnGridMercadoria1" runat="server" CssClass="modalFormTelaCheiaframe"
                            Style="height: 250px;">
                            <div class="gridTableTelaCheia">
                                <asp:GridView ID="gridMercadoria1" runat="server" CellPadding="4" ForeColor="#333333"
                                    GridLines="None">
                                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelecionaMercadoria_CheckedChanged" />
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
                            item</div>
                        <br />
                        <center><h3><b>Selecionados </b></h3></center>
                        <asp:Panel ID="pnGridMercadoriasSelecionado" runat="server" CssClass="modalFormTelaCheialistaDiv"
                            Style="height: 300px;">
                            <div class="gridTableTelaCheia">
                                <asp:GridView ID="gridMercadoriasSelecionadas" runat="server" ForeColor="#333333"
                                    GridLines="Vertical" AutoGenerateColumns="False" OnRowCommand="gridMercadoriasSelecionadas_RowCommand"
                                    CssClass="table">
                                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="PLU" HeaderText="PLU" ReadOnly="True"></asp:BoundField>
                                        <asp:BoundField DataField="Descricao" HeaderText="Descrição" ReadOnly="True"></asp:BoundField>
                                        <asp:BoundField DataField="und" HeaderText="UND" ReadOnly="True"></asp:BoundField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" CommandName="Excluir"
                                            Text="Excluir">
                                            <ControlStyle Height="20px" Width="20px" />
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
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                </ContentTemplate>
            </asp:TabPanel>
              <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel2">
                <HeaderTemplate>
                    Filiais
                </HeaderTemplate>
                <ContentTemplate>
                      <div >
                        <asp:Panel ID="pnAddFilial" runat="server" CssClass="titulobtn" Style="margin-left: 0px;">
                            <asp:ImageButton ID="imgBtnAddFilial" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                Width="20px" OnClick="ImgBtnAddFilias_Click" />
                            Incluir Filial
                        </asp:Panel>
                        <asp:GridView ID="gridFilial" runat="server" AutoGenerateColumns="False" CellPadding="6"
                            Width="100%" ForeColor="#333333" GridLines="Vertical" OnRowCommand="gridFilial_RowCommand">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" CommandName="Excluir"
                                    Text="Excluir">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="FILIAL" HeaderText="FILIAL" ReadOnly="True"></asp:BoundField>
                                <asp:BoundField DataField="CNPJ" HeaderText="CNPJ" ReadOnly="True"></asp:BoundField>
                                <asp:BoundField DataField="Razao_Social" HeaderText="Razão Social" ReadOnly="True">
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
        </asp:TabContainer>
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
    <asp:Panel ID="pnAddItensDepartamento" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="Panel4" runat="server" CssClass="frame" DefaultButton="imgBtnPesquisaItem"
            Style="font-size: 20px;">
            <center><h1><asp:Label ID="lblTituloItensDepartamento" runat="server" Text="Escolha os Itens!" ></asp:Label></h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                Filtrar
                <asp:TextBox ID="txtPesquisaItem" runat="server" Width="400px"></asp:TextBox>
                <asp:ImageButton ID="imgBtnPesquisaItem" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="imgBtnPesquisaItem_Click" />
                <asp:Label ID="lblErroIncluirItem" runat="server" Text="" ForeColor="Red"></asp:Label>
                <asp:Label ID="lblCodItem" runat="server" Visible="false" Text="" />
            </div>
            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="imgBtnConfirmaAddItens" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaAddItens_Click" />
                    <asp:Label ID="Label3" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="imgBtnCancelaAddItens" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelarAddItens_Click" />
                    <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel5" runat="server" CssClass="lista">
                <asp:GridView ID="gridAddItens" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None">
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
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalAddItem" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnAddItensDepartamento" TargetControlID="lblTituloItensDepartamento">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnConfirma" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Excluir o Item?"
                CssClass="cabMenu"></asp:Label>
            <asp:Label ID="lblItemExcluir" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblFilialExcluir" runat="server" Text="" Visible="false"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmarExcluir" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmarExcluir_Click" />
                    <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelarExcluir" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelarExcluir_Click" />
                    <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaExcluir" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnConfirma" TargetControlID="Label16">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirmaExcluir" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label1" runat="server" Text="Tem Certeza que gostaria de Excluir o Contrato?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaExcluirContrato" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaExcluirContrato_Click" />
                    <asp:Label ID="Label8" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelarExcluirContrato" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelarExcluirContrato_Click" />
                    <asp:Label ID="Label9" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaExcluirContrato" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaExcluir" TargetControlID="Label9">
    </asp:ModalPopupExtender>

     <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style=" overflow: auto; min-height:100px;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 30px; margin-bottom: 20px;">
                        <asp:Button ID="btnOkError" runat="server" Text="OK" Width="200px" Height="100%"
                            Font-Size="Larger" OnClick="btnOkError_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalError" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnError" TargetControlID="lblErroPanel">
    </asp:ModalPopupExtender>

</asp:Content>
