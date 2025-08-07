<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListaPadraoProdutosDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Estoque.pages.ListaPadraoProdutosDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/ListaPadraoProdutosDetalhes.js"></script>
    <link href="../css/ListaPadraoProdutosDetalhes.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                LISTA PADRAO DE  <asp:Label ID="lblTipoLista" runat="server" Text="" ></asp:Label></h1>
        </center>
    </div>
    <div id="divPage" runat="server">
        <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
        </asp:Panel>
        <br />
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
        <div id="divBtnImprimirEncerrado" runat="server" class="btnImprimirDireita">
            <asp:ImageButton ID="imgBtnImp" runat="server" ImageUrl="~/img/icon_imprimir.gif"
                Height="50px" OnClick="imgBtnImp_Click" />
        </div>
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
                                Nome
                            </p>
                            <asp:TextBox ID="txtNome" runat="server" Width="250px" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                        </div>

                    </td>
                </tr>
            </table>
        </div>
        <div id="conteudo" runat="server" class="conteudo">
            <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabItens">
                    <HeaderTemplate>
                        Itens
                    </HeaderTemplate>
                    <ContentTemplate>

                        <asp:Panel ID="addItensDig" runat="server" CssClass="filter" DefaultButton="imgPlu">
                            <table>
                                <tr>
                                    <td>
                                        <p>
                                            PLU
                                        </p>
                                        <asp:TextBox ID="txtPlu" runat="server" Width="80px"></asp:TextBox><asp:ImageButton
                                            ID="imgPlu" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" Width="15px"
                                            OnClick="imgPlu_Click" />
                                    </td>
                                    <td>
                                        <p>
                                            Descrição
                                        </p>
                                        <asp:TextBox ID="txtDescricaoItem" runat="server" Width="250px" Enabled="False"></asp:TextBox>
                                    </td>

                                    <td>
                                        <br />
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
                            <asp:Button ID="btnPagInicio" runat="server" Text="<<" Font-Size="Large" UseSubmitBehavior="false" OnClick="btnPag_Click" />
                            <asp:Button ID="btnPagAnterio" runat="server" Text="<" Font-Size="Large" UseSubmitBehavior="false" OnClick="btnPag_Click" />
                            <asp:Button ID="btnPagProximo" runat="server" Text=">" Font-Size="Large" UseSubmitBehavior="false" OnClick="btnPag_Click" />
                            <asp:Button ID="btnPagFim" runat="server" Text=">>" Font-Size="Large" UseSubmitBehavior="false" OnClick="btnPag_Click" />
                        </div>

                        <div id="divAvisoItens" runat="server" visible="true" class="row">
                            <div style="border-style: solid; border-width: 1px; float: left; background: #FFFACD; margin-left: 30px; margin-top: 30px; margin-bottom: 20px;">
                                Itens não incluidos.
                            </div>
                        </div>
                        <div class="gridTable">
                            <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Vertical"
                                AutoGenerateColumns="False" CssClass="table" OnRowCommand="gridItens_RowCommand">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="Ordem" HeaderText="Ordem"></asp:BoundField>
                                    <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                                    <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>

                                    <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/Cancel.png" CommandName="Excluir"
                                        Text="Excluir" ItemStyle-HorizontalAlign="Center">
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
                <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabSelecionarItens">
                    <HeaderTemplate>
                        Selecionar Itens
                    </HeaderTemplate>
                    <ContentTemplate>



                        <div id="divSelecionarAdditens" runat="server" style="float: left; width: 98%; border-bottom: solid; padding: 10px; margin-bottom: 30px;">

                            <center>
                                <h2><asp:Label ID="lblMercadoriaLista" runat="server" Text="Inclusão de Produto" ></asp:Label> </h2><br />
                            
                            </center>
                            <hr />
                            <asp:Panel ID="pnFiltraMercadoria" DefaultButton="imgPesquisaMercadoria" runat="server" class="modalFormTelaCheiaframeMini" Style="height: 250px;">
                                <table>
                                    <tr>
                                        <td colspan="3">
                                            <center><b>Filtrar</b></center>
                                            <hr />
                                        </td>
                                    </tr>
                                    <div id="divCompra" runat="server">

                                        <tr>
                                            <td>Grupo:
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtGrupo" runat="server" Width="150px"></asp:TextBox>
                                                <asp:ImageButton ID="imgBtnGrupo" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                                    Width="15px" OnClick="imgBtnGrupo_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>SubGrupo:
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtSubGrupo" runat="server" Width="150px"></asp:TextBox>
                                                <asp:ImageButton ID="imgBtnSubGrupo" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                    Height="15px" Width="15px" OnClick="imgBtnSubGrupo_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Departamento:
                                            </td>
                                            <td colspan="2">
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
                                    </div>
                                    <div id="divProducao" runat="server">
                                        <tr>
                                            <td>Cozinha:
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtCozinha" runat="server" Width="150px"></asp:TextBox>
                                                <asp:ImageButton ID="imgBtnCozinha" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                                    Width="15px" OnClick="imgBtnCozinha_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Agrupamento:
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtAgrupamento" runat="server" Width="150px"></asp:TextBox>
                                                <asp:ImageButton ID="imgBtnAGrupamento" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                    Height="15px" Width="15px" OnClick="imgBtnAGrupamento_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Tipo:
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtTipoProducao" runat="server" Width="150px"></asp:TextBox>
                                                <asp:ImageButton ID="imgBtnTipoProducao" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                    Height="15px" Width="15px" OnClick="imgBtnTipoProducao_Click" />
                                            </td>
                                        </tr>
                                    </div>
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
                                            <asp:TextBox ID="txtDescricaoListaPadrao" runat="server" Width="120px" />
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
                            </asp:Panel>
                            <asp:Panel ID="pnGridMercadoria1" runat="server" CssClass="modalFormTelaCheiaframe"
                                Style="height: 250px; overflow: hidden">
                                <div class="panelItem" style="margin-bottom: 10px; margin-left: 30px;">
                                    <asp:Label ID="lblResultadoPesquisaIncluir" runat="server"></asp:Label>
                                </div>
                                <div class="direitaFechar">
                                    <asp:Button ID="btnPagInicioPesq" runat="server" Text="<<" Font-Size="Large" UseSubmitBehavior="false" OnClick="btnPagPesq_Click" />
                                    <asp:Button ID="btnPagAnterioPesq" runat="server" Text="<" Font-Size="Large" UseSubmitBehavior="false" OnClick="btnPagPesq_Click" />
                                    <asp:Button ID="btnPagProximoPesq" runat="server" Text=">" Font-Size="Large" UseSubmitBehavior="false" OnClick="btnPagPesq_Click" />
                                    <asp:Button ID="btnPagFimPesq" runat="server" Text=">>" Font-Size="Large" UseSubmitBehavior="false" OnClick="btnPagPesq_Click" />
                                </div>

                                <div class="gridTableTelaCheia" style="height: 200px; overflow: auto">
                                    <asp:GridView ID="gridMercadoria1" runat="server" CellPadding="4" ForeColor="#333333"
                                        GridLines="None" AutoGenerateColumns="false">
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
                                            <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                                            <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>

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
                                    Height="20px" Width="20px" OnClick="imgBtnIncluirSelecionados_Click" />Incluir selecionados
                            </div>
                            <br />

                        </div>
                        <br />
                        <center><h3><b>Selecionados </b></h3></center>
                        <div class="panelItem" style="margin-bottom: 10px; margin-left: 30px;">
                            <h1>
                                <asp:Label ID="lblRegistrosSeleciona" runat="server"></asp:Label></h1>
                        </div>
                        <div class="direitaFechar">
                            <asp:Button ID="btnPagInicioSeleciona" runat="server" Text="<<" Font-Size="Large" UseSubmitBehavior="false" OnClick="btnPag_Click" />
                            <asp:Button ID="btnPagAnterioSeleciona" runat="server" Text="<" Font-Size="Large" UseSubmitBehavior="false" OnClick="btnPag_Click" />
                            <asp:Button ID="btnPagProximoSeleciona" runat="server" Text=">" Font-Size="Large" UseSubmitBehavior="false" OnClick="btnPag_Click" />
                            <asp:Button ID="btnPagFimSeleciona" runat="server" Text=">>" Font-Size="Large" UseSubmitBehavior="false" OnClick="btnPag_Click" />
                        </div>
                        <asp:Panel ID="pnGridMercadoriasSelecionado" runat="server" CssClass="modalFormTelaCheialistaDiv"
                            Style="height: 500px;">

                            <div class="gridTable">
                                <asp:GridView ID="gridItensSelecionados" runat="server" ForeColor="#333333" GridLines="Vertical"
                                    AutoGenerateColumns="False" CssClass="table" OnRowCommand="gridItens_RowCommand">
                                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Ordem" HeaderText="Ordem"></asp:BoundField>
                                        <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                                        <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>

                                        <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/Cancel.png" CommandName="Excluir"
                                            Text="Excluir" ItemStyle-HorizontalAlign="Center">
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


                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabImportarExcel">
                    <HeaderTemplate>
                        Importar Itens 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                            <ContentTemplate>

                                <div ID="divExportar" runat="server" style="width: 20%; float: left; padding: 15px;">
                                    <h3 style="text-align: center;">EXPORTAR</h3>
                                    <hr />
                                    <asp:LinkButton ID="linkExportaExcel" runat="server" OnClick="linkExportaExcel_Click" CssClass="btn btn-default" Style="text-decoration: none; color: black; margin-left: 25px;"> 
                                       <img src="../../../img/arquivo-xls.png" width="35px"/> 
                                        <div style="position: relative; left: 10px; top: -10px; display: inline-block; margin-top: -40px; margin-right: 10px;">Exportar Excel</div>
                                    </asp:LinkButton>
                                </div>
                                <div id="divImportar" runat="server" style="width: 70%; float: left; padding: 10px; margin-top: 6px;">

                                    <h3 style="text-align: center;">IMPORTAR EXCEL</h3>
                                    <hr />
                                    <div id="divUpload" runat="server" style="position: relative; width: 70%; margin: auto;">
                                        <div class="panelItem" style="width: 70%;">
                                            <label for='MainContent_TabContainer1_tabImportarExcel_FileArquivo' class="btn btn-default" style="padding: 10px;">
                                                <img src="../../../img/arquivo-xls.png" width="35px" style="display: inline-block;" />
                                                <div style="position: relative; left: 10px; top: -10px; display: inline-block; margin-top: -40px; margin-right: 10px;">Selecionar um arquivo </div>

                                            </label>
                                            <span id='file-name'></span>
                                            <asp:FileUpload ID="FileArquivo" runat="server" onchange="javascript:changeFile(this);"
                                                AllowMultiple="false"
                                                accept="application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                                                Style="display: none;" />
                                        </div>
                                        <div class="panelItem"  style="width: 20%;">
                                            <label for='MainContent_TabContainer1_tabImportarExcel_btnEnviarArquivo' class="btn btn-default" style="padding: 10px;">
                                                <img src="../../../img/arquivo-download.png" width="35px" style="display: inline-block;" />
                                                <div style="position: relative; left: 10px; top: -10px; display: inline-block; margin-top: -40px; margin-right: 10px;">
                                                    Importar Arquivo
                                                </div>

                                            </label>
                                            <asp:Button ID="btnEnviarArquivo" runat="server" CssClass="btn btn-default" Text="" Style="display: none;"
                                                OnClick="btnUpLoad_Click" />
                                        </div>

                                    </div>
                                    <br />
                                    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>

                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnEnviarArquivo" runat="server" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </div>
    </div>

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

    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none"
        DefaultButton="ImgPesquisaLista">
        <asp:Panel ID="Panel3" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px;">
            <center>
                    <h1>
                    <asp:Label ID="lbltituloLista" runat="server" Text=""></asp:Label>
                    </h1>
            </center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                Filtrar
            <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="400px" CssClass="SEM"></asp:TextBox>
                <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="ImgPesquisaLista_Click" />
                <asp:Label ID="lblErroPesquisa" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">


                    <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaLista_Click" />
                    <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="Label2" runat="server" Text="Cancela"></asp:Label>
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
    <asp:ModalPopupExtender ID="modalLista" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="Label2" DropShadow="true" PopupControlID="pnfundo" TargetControlID="lbltituloLista">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnTodosSelecionados" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="overflow: auto; min-height: 100px;">
            <h2>Gostaria de incluir ?
            </h2>
            <asp:Label ID="lblfuckTitulo" Text="" runat="server" />
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div class="frame" style="width: 100%;">
                        <asp:Button ID="btnTodosItensSelecionados" runat="server" Text="Todos" Height="100%"
                            Font-Size="X-Large" OnClick="btnTodosSelecionados_Click" Style="float: left; margin: 20px" />
                        <asp:Button ID="btnApenasTela" runat="server" Text="Pagina" Height="100%"
                            Font-Size="X-Large" OnClick="btnApenasTela_Click" Style="float: right; margin: 20px" />
                    </div>
                </td>
            </tr>
        </table>

    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaTodosSelecionado" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnTodosSelecionados" TargetControlID="lblfuckTitulo">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirmaExcluirItem" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 100px;">
            <h3>
                <asp:Label ID="Label6" runat="server" Text="Tem Certeza que gostaria de Excluir "></asp:Label><br />
                <asp:Label ID="lblPluExcluir" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblPluDescricao" runat="server" Text=""></asp:Label>

                <asp:Label ID="lblFornecedorExcluir" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblFornecedorDescricao" runat="server" Text=""></asp:Label>
                ?
            </h3>
        </div>
        <table style="width: 90%;">
            <tr>
                <td style="padding: 10px; text-align: center;">
                    <asp:ImageButton ID="imgBtnConfirmaExluirItem" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaExluirItem_Click" />
                    <asp:Label ID="Label7" runat="server" Text="SIM"></asp:Label>
                </td>
                <td style="padding: 10px; text-align: center;">
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




    <asp:Panel ID="pnManterListaAnterior" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label3" runat="server" Text="Gostaria de Manter a Lista Anterior?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaManter" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaManter_Click" />
                    <asp:Label ID="Label4" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelarManter" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelarManter_Click" />
                    <asp:Label ID="Label5" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaManter" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnManterListaAnterior" TargetControlID="Label3">
    </asp:ModalPopupExtender>

    

    <asp:Panel ID="pnConfirmaExcluir" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label9" runat="server" Text="Gostaria de Excluir a lista?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="ImgBtnConfirmaExcluir" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="ImgBtnConfirmaExcluir_Click" />
                    <asp:Label ID="Label10" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="ImgBtnCancelaExcluir" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="ImgBtnCancelaExcluir_Click" />
                    <asp:Label ID="Label11" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaExcluir" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaExcluir" TargetControlID="Label9">
    </asp:ModalPopupExtender>




</asp:Content>
