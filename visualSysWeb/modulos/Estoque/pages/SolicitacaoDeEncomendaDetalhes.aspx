<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SolicitacaoDeEncomendaDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Estoque.pages.SolicitacaoDeEncomendaDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Solicitação de Encomenda Detalhes</h1>
        </center>
    </div>
    <div id="divPage" runat="server">
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
                                Codigo
                            </p>
                            <asp:TextBox ID="txtCodigo" runat="server" Width="80px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Descrição
                            </p>
                            <asp:TextBox ID="txtDescricao" runat="server" Width="250px" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Data
                            </p>
                            <asp:TextBox ID="txtDataCadastro" runat="server" Width="80px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Usuario
                            </p>
                            <asp:TextBox ID="txtUsuarioCadastro" runat="server" Width="80px"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Status
                            </p>
                            <asp:TextBox ID="txtstatus" runat="server"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <asp:Button ID="btnEncerrar" runat="server" Text="Encerrar" OnClick="btnEncerrar_Click"
                                CssClass="submitButton" Height="40px" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="conteudo" runat="server" class="conteudo">
            <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" AutoPostBack="true" OnActiveTabChanged="TabContainer1_ActiveTabChanged">
                <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                    <HeaderTemplate>
                        Itens Adicionados
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:Panel ID="addItensDig" runat="server" CssClass="filter" DefaultButton="imgPlu">
                            <table>
                                <tr>
                                    <td>
                                        <p>
                                            PLU/EAN
                                        </p>
                                        <asp:TextBox ID="txtPlu" runat="server" Width="150px"></asp:TextBox><asp:ImageButton
                                            ID="imgPlu" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" Width="15px"
                                            OnClick="imgPlu_Click" />
                                    </td>
                                    <td>
                                        <p>
                                            Descrição
                                        </p>
                                        <asp:TextBox ID="txtDescricaoItem" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td>
                                        <p>
                                            Und
                                        </p>
                                        <asp:TextBox ID="txtUnidadeItem" runat="server" Width="50px" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td>
                                        <p>
                                            <asp:Label ID="lblContado" runat="server" Text="Qtde"></asp:Label>
                                        </p>
                                        <asp:TextBox ID="txtQtde" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImgBtnAddItens" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                            Width="20px" OnClick="ImgBtnAddItens_Click" />Incluir item
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
                            <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Vertical"
                                AutoGenerateColumns="False" OnRowCommand="gridItens_RowCommand" CssClass="table"
                                OnRowEditing="gridItens_RowEditing">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="ordem" HeaderText="Ordem" ItemStyle-Width="80px"></asp:BoundField>
                                    <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                                    <asp:BoundField DataField="EAN" HeaderText="EAN"></asp:BoundField>
                                    <asp:BoundField DataField="ref_fornecedor" HeaderText="Ref"></asp:BoundField>
                                    <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                                    <asp:BoundField DataField="Und" HeaderText="Und"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Qtde Solicitada" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGridQtde" runat="server" Text='<%# Eval("Qtde") %>' Width="90%" OnKeyPress="javascript:return autoTab(this,event);"
                                                CssClass="numero"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OBS" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtObs" runat="server" Text='<%# Eval("Obs") %>' Width="95%" Height="50px" TextMode="MultiLine" MaxLength="500" OnKeyPress="javascript:return autoTab(this,event);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/Cancel.png" CommandName="Excluir"
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

                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel4">
                    <HeaderTemplate>
                        Selecionar Itens
                    </HeaderTemplate>
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
                                        <td>Grupo:
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblGrupo" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtGrupo" runat="server" Width="150px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnGrupo" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                                Width="15px" OnClick="imgBtnGrupo_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>SubGrupo:
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblSubGrupo" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtSubGrupo" runat="server" Width="150px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnSubGrupo" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" Width="15px" OnClick="imgBtnSubGrupo_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Departamento:
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblDepartamento" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtDepartamento" runat="server" Width="150px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnDepartamento" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" Width="15px" OnClick="imgBtnDepartamento_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Linha
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlLinha" runat="server" AutoPostBack="True" Width="200px"
                                                OnSelectedIndexChanged="ddlLinha_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Plu/EAN/Descricao:
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtfiltromercadoria" runat="server" Width="200px" OnTextChanged="txtfiltromercadoria_TextChanged"
                                                autocomplete="off"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Listas Salvas
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcodListaPadrao" runat="server" Width="30px" AutoPostBack="true" OnTextChanged="txtcodListaPadrao_TextChanged" />
                                            <asp:TextBox ID="txtDescricaoListaPadrao" runat="server" Width="150px" />
                                            <asp:ImageButton
                                                ID="imgBtnListaPadrao" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                                Width="15px" OnClick="imgBtnListaPadrao_Click" />
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
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="pnGridMercadoria1" runat="server" CssClass="modalFormTelaCheiaframe"
                                Style="height: 250px;">
                                <div class="diaSemana">
                                    Programação Semanal:
                                    <asp:Label ID="lblProgramação" Text="" runat="server" />
                                </div>
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
                                item
                            </div>
                            <br />
                            <center><h3><b>Selecionados </b></h3></center>
                            <asp:Panel ID="pnGridMercadoriasSelecionado" runat="server" CssClass="modalFormTelaCheialistaDiv"
                                Style="height: 300px;">
                                <asp:Button ID="btnSalvarListaPadrao" Text="Salvar Lista" runat="server" OnClick="btnSalvarListaPadrao_Click" Style="margin-left: 30px;" />
                                <div class="gridTableTelaCheia">
                                    <asp:GridView ID="gridMercadoriasSelecionadas" runat="server" ForeColor="#333333"
                                        GridLines="Vertical" AutoGenerateColumns="False" OnRowCommand="gridItensSelecao_RowCommand"
                                        CssClass="table">
                                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="ordem" HeaderText="Ordem" ItemStyle-Width="80px"></asp:BoundField>
                                            <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                                            <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                                            <asp:BoundField DataField="Und" HeaderText="Und"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Qtde Solicitada" ItemStyle-Width="80px">
                                                <ItemTemplate>

                                                    <asp:TextBox ID="txtGridQtde" runat="server" Text='<%# Eval("Qtde") %>' Width="90%" OnKeyPress="javascript:return autoTab(this,event);"
                                                        CssClass="numero"></asp:TextBox>

                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OBS" ItemStyle-Width="200px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtObs" runat="server" Text='<%# Eval("Obs") %>' Width="95%" Height="50px" TextMode="MultiLine" MaxLength="500" OnKeyPress="javascript:return autoTab(this,event);"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/Cancel.png" CommandName="Excluir"
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

            </asp:TabContainer>
        </div>
    </div>

    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none"
        DefaultButton="ImgPesquisaLista">
        <asp:Panel ID="Panel3" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista">
            <div class="cabMenu">
                <h1>
                    <asp:Label ID="lbltituloLista" runat="server" Text=""></asp:Label>
                </h1>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaLista_Click" />
                        <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaLista_Click" />
                        <asp:Label ID="Label2" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            Filtrar
            <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="600px" CssClass="SEM"></asp:TextBox>
            <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                Height="15px" OnClick="ImgPesquisaLista_Click" />
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
    <asp:ModalPopupExtender ID="modalLista" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="Label2" DropShadow="true" PopupControlID="pnfundo" TargetControlID="lbltituloLista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnEncerrar" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Encerrar A Solicitação?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmarEncerrar" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaEncerrar_Click" />
                    <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelarInativar" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelarEncerrar_Click" />
                    <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalEncerrar" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnEncerrar" TargetControlID="Label14">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnConfirmaExcluirItem" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label6" runat="server" Text="Tem Certeza que gostaria de Excluir o Item?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <asp:Label ID="lblPluExcluir" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lbllinhaGrid" runat="server" Text="" Visible="false"></asp:Label>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaExluirItem" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaExluirItem_Click" />
                    <asp:Label ID="Label7" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelaExluirItem" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaExluirItem_Click" />
                    <asp:Label ID="Label8" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluirItem" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaExcluirItem" TargetControlID="Label6">
    </asp:ModalPopupExtender>



    <asp:Panel ID="pnSalvarListaPadrao" runat="server" CssClass="frameModal" Style="display: none; width: 400px;">

        <h2>

            <asp:Label ID="Label4" runat="server" Text="Informe o nome da nova Lista"
                CssClass="cabMenu"></asp:Label>
        </h2>

        <asp:TextBox ID="txtNomeNovaListaPadrao" runat="server" Width="95%" Font-Size="X-Large" Style="margin-left: 10px; margin-bottom: 10px;" />
        <table style="width: 100%">
            <tr>
                <td>
                    <center>
                    <asp:ImageButton ID="imgBtnSalvarListaPadrao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnSalvarListaPadrao_Click" />
                    <asp:Label ID="Label5" runat="server" Text="SALVAR"></asp:Label>
                 </center>
                </td>
                <td>
                    <center>
                    <asp:ImageButton ID="imgBtnCancelaListaPadrao" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" />
                    <asp:Label ID="Label12" runat="server" Text="CANCELAR"></asp:Label>
                    </center>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalSalvarListaPadrao" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnSalvarListaPadrao" TargetControlID="Label4"
        CancelControlID="imgBtnCancelaListaPadrao">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="overflow: auto; min-height: 100px;">
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
