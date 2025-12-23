<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="MercadoriaDetalhes.aspx.cs" Inherits="visualSysWeb.Cadastro.MercadoriaDetalhes"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../../Scripts/ckeditor/ckeditor.js" type="text/javascript"></script>
    <script src="../js/MercadoriaDetalhes2.js"></script>

    <script type="text/javascript">
        function pageLoad() {
            try {


                CKEDITOR.replace('ctl00$MainContent$TabContainer1$TabEcommerce$txtEcommercer');

                timer = setInterval(updateDiv, 100);
                function updateDiv() {
                    for (var i in CKEDITOR.instances) {
                        CKEDITOR.instances[i].updateElement();
                    }
                }
            }
            catch (err) {

            }
        }
    </script>

    <link href="../css/MercadoriaDetalhes0.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h2>
            Detalhes Produto
        </h2>
    </center>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <table>
            <tr>
                <td>
                    <p>
                        PLU
                    </p>
                    <asp:TextBox ID="TxtCodPLU" runat="server" Width="50px" MaxLength="6"></asp:TextBox>
                    <asp:ImageButton ID="ImgPlu" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                        OnClick="ImgPlu_Click" Style="width: 15px" />
                </td>
                <td>
                    <p>
                        EAN
                    </p>
                    <asp:TextBox ID="txtEAN" runat="server" Width="150px"></asp:TextBox>
                    <asp:ImageButton ID="btnEans" runat="server" ImageUrl="~/img/edit.png" Height="15px"
                        Style="width: 15px" OnClick="btnEans_Click" />
                </td>
                <td>
                    <p>
                        Descrição 
                    </p>
                    <asp:TextBox ID="txtDescricao" runat="server" Width="436px" MaxLength="60" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Descrição Resumida
                    </p>
                    <asp:TextBox ID="txtDescResumida" runat="server" Width="267px" MaxLength="40" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Usuario Cadastro
                    </p>
                    <asp:TextBox ID="txtUsuario" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Usuario Ult alteração
                    </p>
                    <asp:TextBox ID="txtUsuarioAlteracao" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" OnActiveTabChanged="TabContainer1_ActiveTabChanged"
            AutoPostBack="true">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabCadastro">
                <HeaderTemplate>
                    Cadastro
                </HeaderTemplate>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <p>
                                    Grupo
                                </p>
                                <asp:TextBox ID="txtCodGrupo" runat="server" Width="80px" CssClass="numero" OnTextChanged="txtCodGrupo_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>&nbsp;
                                <asp:TextBox ID="txtGrupo" runat="server" Width="177px"></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_TxtGrupo" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                            </td>
                            <td>
                                <p>
                                    Sub Grupo
                                </p>
                                <asp:TextBox ID="txtCodSubGrupo" runat="server" Width="80px" CssClass="numero" OnTextChanged="txtCodSubGrupo_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>&nbsp;
                                <asp:TextBox ID="txtSubGrupo" runat="server" Width="177px"></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_TxtSubGrupo" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                            </td>
                            <td>
                                <p>
                                    Departamento
                                </p>
                                <asp:TextBox ID="txtCodDepartamento" runat="server" Width="80px" CssClass="numero"
                                    AutoPostBack="True" OnTextChanged="txtCodDepartamento_TextChanged"></asp:TextBox>&nbsp;
                                <asp:TextBox ID="txtDepartamento" runat="server" Width="174px"></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_TxtDepartamento" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                            </td>
                        </tr>
                        <div id="divCategoria" runat="server">
                            <tr>
                                <td colspan="3">
                                    <div class="panelItem">
                                        <p>Categoria</p>
                                        <asp:TextBox ID="txtCodCategoria" runat="server" Width="150px" CssClass="numero" MaxLength="20"></asp:TextBox>
                                    </div>
                                    <div class="panelItem txtSemTitulo">
                                        <asp:TextBox ID="txtDescCategoria" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
                                        <asp:ImageButton ID="imgBtn_Categoria" runat="server" ImageUrl="~/img/pesquisaM.png"
                                            Height="15px" OnClick="Img_Click" />
                                    </div>
                                    <div class="panelItem">
                                        <p>Seguimento</p>
                                        <asp:TextBox ID="txtCodSeguimento" runat="server" Width="150px" CssClass="numero" MaxLength="20"></asp:TextBox>
                                    </div>
                                    <div class="panelItem txtSemTitulo">

                                        <asp:TextBox ID="txtDescSeguimento" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
                                        <asp:ImageButton ID="imgBtn_Seguimento" runat="server" ImageUrl="~/img/pesquisaM.png"
                                            Height="15px" OnClick="Img_Click" />
                                    </div>
                                    <div class="panelItem ">
                                        <p>SubSeguimento</p>
                                        <div class="row">
                                            <asp:TextBox ID="txtCodSubSeguimento" runat="server" Width="150px" CssClass="numero" MaxLength="20"></asp:TextBox>

                                            <asp:TextBox ID="txtDescSubSeguimento" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtn_SubSeguimento" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" OnClick="Img_Click" />
                                        </div>
                                    </div>

                                </td>

                            </tr>
                            <tr>

                                <td colspan="3">
                                    <div class="panelItem">
                                        <p>Grupo</p>
                                        <asp:TextBox ID="txtCodGrupoCategoria" runat="server" Width="150px" CssClass="numero" MaxLength="20"></asp:TextBox>
                                    </div>
                                    <div class="panelItem txtSemTitulo">

                                        <asp:TextBox ID="txtDescGrupoCategoria" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
                                        <asp:ImageButton ID="imgBtn_GrupoCategoria" runat="server" ImageUrl="~/img/pesquisaM.png"
                                            Height="15px" OnClick="Img_Click" />
                                    </div>


                                    <div class="panelItem ">
                                        <p>Sub Grupo</p>
                                        <asp:TextBox ID="txtCodSubGrupoCategoria" runat="server" Width="150px" CssClass="numero" MaxLength="20"></asp:TextBox>
                                    </div>
                                    <div class="panelItem txtSemTitulo">

                                        <asp:TextBox ID="txtDescSubGrupoCategoria" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
                                        <asp:ImageButton ID="imgBtn_SubGrupoCategoria" runat="server" ImageUrl="~/img/pesquisaM.png"
                                            Height="15px" OnClick="Img_Click" />
                                    </div>
                                </td>
                            </tr>
                        </div>
                        <tr>
                            <td>
                                <p>
                                    Familia
                                </p>
                                <asp:TextBox ID="txtCodFamilia" runat="server" Width="79px" CssClass="numero" OnTextChanged="txtCodFamilia_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>&nbsp;
                                <asp:TextBox ID="txtFamilia" runat="server" Width="182px"></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_TxtFamilia" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                            </td>
                            <td>
                                <p>
                                    Localiza
                                </p>
                                <asp:TextBox ID="txtLocaliza" runat="server" Width="280px"></asp:TextBox>
                            </td>
                            <td>
                                <div class="panelItem">
                                    <p>Ativa CE</p>
                                    <asp:DropDownList ID="ddlAtivarCE" runat="server">
                                        <asp:ListItem Value="0">NÃO</asp:ListItem>
                                        <asp:ListItem Value="1">SIM CARGA</asp:ListItem>
                                        <asp:ListItem Value="2">SIM CARGA E TELA</asp:ListItem>
                                    </asp:DropDownList>
                                </div>


                            </td>

                        </tr>
                        <tr>
                            <td>
                                <p>
                                    Tipo
                                </p>
                                <asp:TextBox ID="txtCodTipo" runat="server" Width="290px" AutoPostBack="True" OnTextChanged="txtCodTipo_TextChanged"></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_TxtTipo" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                            </td>
                            <td>
                                <div class="panelItem">
                                    <p>Und</p>
                                    <asp:TextBox ID="txtUnidade" runat="server" Width="70px"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtn_TxtUnidade" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />&nbsp;
                                   
                                </div>
                                <div class="panelItem">
                                    <p>Und Produção</p>
                                    <asp:TextBox ID="txtUndProducao" runat="server" Width="70px"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtn_txtUndProducao" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />&nbsp;
                                  
                                </div>
                                <div class="panelItem">
                                    <p>Tecla</p>
                                    <asp:TextBox ID="txtTecla" runat="server" Width="80px" CssClass="numero"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div class="panelItem">

                                    <p>
                                        Departamento CE
                                    </p>
                                    <asp:TextBox ID="txtCodDepartamentoCE" runat="server" Width="80px" CssClass="numero"
                                        AutoPostBack="True" OnTextChanged="txtCodDepartamentoCE_TextChanged"></asp:TextBox>&nbsp;
                                <asp:TextBox ID="txtDepartamentoCEDescricao" runat="server" Width="174px"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtn_txtCodDepartamentoCE" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />
                                </div>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <p>
                                    Ultimo Fornecedor
                                </p>
                                <asp:TextBox ID="txtUltimoFornecedor" runat="server" Width="290px"></asp:TextBox>&nbsp;
                                <asp:ImageButton ID="imgBtnUltmioFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="imgBtnUltmioFornecedor_Click" Style="width: 15px" />
                            </td>
                            <td>
                                <p>
                                    Referencia do Fornecedor
                                </p>
                                <asp:TextBox ID="txtReferenciaFornecedor" runat="server" Width="253px"></asp:TextBox>
                                <asp:ImageButton ID="imgRefFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="ImgRefFornecedor_Click1" Style="width: 15px" />
                            </td>
                            <td>
                                <div class="panelItem">
                                    <p>
                                        Data Cadastro
                                    </p>
                                    <asp:TextBox ID="txtDtCadastro" runat="server" Width="100px"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <br />
                                    <asp:CheckBox ID="chkImprimeEtiqueta" runat="server" Text="Imprime Etiqueta " />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    Linha
                                </p>
                                <asp:TextBox ID="TxtCodLinha" runat="server" Width="80px" CssClass="numero"></asp:TextBox>&nbsp;
                                <asp:TextBox ID="TxtDescricaoLinha" runat="server" Width="177px"></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_txtCodLinha" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                            </td>
                            <td>
                                <p>
                                    Cor Linha
                                </p>
                                <asp:TextBox ID="txtCodCorLinha" runat="server" Width="80px" CssClass="numero"></asp:TextBox>&nbsp;
                                <asp:TextBox ID="TxtDescricaoCorLinha" runat="server" Width="177px"></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_txtCodCorLinha" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                            </td>
                            <td>
                                <div class="panelItem">
                                    <p>
                                        Data Alteração
                                    </p>
                                    <asp:TextBox ID="txtDtAlteracao" runat="server" Width="100px"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <br />
                                    <asp:CheckBox ID="chkAlcoolico" runat="server" Text="Bebida Alcoólica " />
                                    <br />
                                    <asp:CheckBox ID="chkImpAux" runat="server" Text="Impressão Auxiliar" />
                                    <br />
                                    <asp:CheckBox ID="ChkVendaComSenha" runat="server" Text="Venda PDV com Senha" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <h3>
                            Balança</h3>
                    </center>
                    <hr />
                    <table>
                        <tr>
                            <td>
                                <p>
                                    Peso Variavel
                                </p>
                                <asp:DropDownList ID="ddlPesoVariavel" runat="server">
                                    <asp:ListItem>NÃO</asp:ListItem>
                                    <asp:ListItem>PESO</asp:ListItem>
                                    <asp:ListItem>UNIDADE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <p>
                                    Etiqueta
                                </p>
                                <asp:TextBox ID="txtEtiqueta" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Tecla balança
                                </p>
                                <asp:TextBox ID="txtTeclaBalanca" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Validade
                                </p>
                                <asp:TextBox ID="txtValidade" runat="server" Width="100px" CssClass="numero" MaxLength="3"></asp:TextBox>
                            </td>
                            <td style="display: none;">
                                <p>
                                    Venda Fracionaria
                                </p>
                                <asp:DropDownList ID="ddlVendaFracionaria" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem>SIM</asp:ListItem>
                                    <asp:ListItem>NAO</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <p>
                                    Bandeja
                                </p>
                                <asp:DropDownList ID="ddlBandeja" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>

                        </tr>
                    </table>
                    <center>
                        <h3>
                            Pesagem</h3>
                    </center>

                    <table>
                        <tr>
                            <td>
                                <p>
                                    Peso Bruto
                                </p>
                                <asp:TextBox ID="txtPesoBruto" runat="server" Width="210px" CssClass="numero"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Peso Liquido
                                </p>
                                <asp:TextBox ID="txtPesoLiquido" runat="server" Width="210px" CssClass="numero"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <h3>
                            Dimenssões</h3>
                    </center>
                    <table>
                        <tr>
                            <td>
                                <p>
                                    Altura
                                </p>
                                <asp:TextBox ID="txtAltura" runat="server" Width="210px" CssClass="numero"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Largura
                                </p>
                                <asp:TextBox ID="txtLargura" runat="server" Width="210px" CssClass="numero"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Profundidade
                                </p>
                                <asp:TextBox ID="txtProfundidade" runat="server" Width="210px" CssClass="numero"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <div class="panelItem">
                                    <p>
                                        Marca
                                    </p>
                                    <asp:TextBox ID="txtMarca" runat="server" Width="420px"></asp:TextBox>
                                </div>


                            </td>
                        </tr>
                    </table>
                    <hr />
                    <center>
                        <h3>
                            Detalhamento de Medicamentos ANVISA</h3>
                        <p>Caso o medicamento não possua código, informar o literal ISENTO e obrigatoriamente informar o motivo da isenção.</p>
                        <br />
                    </center>
                    <table>
                        <tr>
                            <td>
                                <center>
                                <p>Código de produto<br /> na ANVISA
                                </p></center>
                                <asp:TextBox ID="txtCodigoProdutoANVISA" runat="server" Width="130px"></asp:TextBox>
                            </td>
                            <td>
                                <center>
                                <p>
                                    Motivo da isenção <br />na ANVISA
                                </p>

                                </center>
                                <asp:TextBox ID="txtMotivoIsencaoANVISA" placeHolder="Preencher este campo quando o Código de produto for igual a ISENTO" runat="server" Width="500px" MaxLength="255" ></asp:TextBox>
                            </td>
                            <td>
                                <center>
                                <p>
                                    Preço Máximo<br /> ao Consumidor
                                </p>
                                </center>
                                <asp:TextBox ID="txtPrecoMaximoANVISA" runat="server" Width="130px" CssClass="numero"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <div class="row">

                        <div class="pratoDia">
                            <asp:CheckBox ID="chkPratoDia" Text="Prato do dia" runat="server" onclick="pratoDoDia(this)" />
                            <hr />
                            <div id="divPratoDia" class="pratoDiaDetalhe">
                                <asp:CheckBox ID="chkPratoDia_1" Text="Domingo" runat="server" />
                                <asp:CheckBox ID="chkPratoDia_2" Text="Segunda" runat="server" />
                                <asp:CheckBox ID="chkPratoDia_3" Text="Terça" runat="server" />
                                <asp:CheckBox ID="chkPratoDia_4" Text="Quarta" runat="server" />
                                <asp:CheckBox ID="chkPratoDia_5" Text="Quinta" runat="server" />
                                <asp:CheckBox ID="chkPratoDia_6" Text="Sexta" runat="server" />
                                <asp:CheckBox ID="chkPratoDia_7" Text="Sabado" runat="server" />
                            </div>
                        </div>

                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPreco" runat="server" HeaderText="TabPreco">
                <HeaderTemplate>
                    Preço
                </HeaderTemplate>
                <ContentTemplate>
                    <table style="float: left; width: 100%;">
                        <tr>
                            <td>
                                <center>
                                    <h3>
                                        Preço principal</h3>
                                </center>
                                <hr />
                                <table>
                                    <tr>
                                        <td>
                                            <p>
                                                Preço compra
                                            </p>
                                            <asp:TextBox ID="txtPrecoCompra" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                Preço custo
                                            </p>
                                            <asp:TextBox ID="txtPrecoCusto" runat="server" Width="100px" CssClass="numero" AutoPostBack="True"
                                                OnTextChanged="txtPrecoCusto_TextChanged"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                Margem
                                            </p>
                                            <asp:TextBox ID="txtMargem" runat="server" Width="100px" CssClass="numero" AutoPostBack="True"
                                                OnTextChanged="txtMargem_TextChanged"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                Preço venda
                                            </p>
                                            <asp:TextBox ID="txtPrecoVenda" runat="server" Width="100px" CssClass="numero" AutoPostBack="True"
                                                Style="font-size: 20px; text-align: right;" OnTextChanged="txtPrecoVenda_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>
                                                Valor ST
                                            </p>
                                            <asp:TextBox ID="txtValorST" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                Valor Lucro
                                            </p>
                                            <asp:TextBox ID="txtValorLucro" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                Valor IPI
                                            </p>
                                            <asp:TextBox ID="txtValorIPI" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                Preço Delivery
                                            </p>
                                            <asp:TextBox ID="txtPrecoReferencia" runat="server" Width="100px" CssClass="numero"
                                                Style="font-size: 20px; text-align: right;"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>
                                                Valor icms
                                            </p>
                                            <asp:TextBox ID="txtValorIcms" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                Valor PIS/Cofins
                                            </p>
                                            <asp:TextBox ID="txtValorPisConfins" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="btnAplicarLojas" runat="server" Text="Aplicar Preço em Lojas" OnClick="btnAplicarLojas_Click" />
                                            <asp:Button ID="btnHistoricoPreco" runat="server" Text="Ultimos Preços" OnClick="btnHistorioPreco_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <asp:Panel ID="pnPrecoPromocao" runat="server" Style="width: 50%; float: left;">
                                    <center>
                                            <h3>Promoção (cupom e pedido venda)</h3>
                                         </center>
                                    <hr />
                                    <asp:CheckBox ID="chkPromocao" runat="server" Text="Promoção" OnCheckedChanged="chkPromocao_CheckedChanged"
                                        AutoPostBack="True" />
                                    <table>
                                        <tr>
                                            <td>
                                                <p>
                                                    Data Inicio
                                                </p>
                                                <asp:TextBox ID="txtDtInicioPromo" runat="server" Width="80px" CssClass="data"></asp:TextBox>
                                                <asp:ImageButton ID="ImgDtInicioPromocao" ImageUrl="~/img/calendar.png" runat="server"
                                                    Height="15px" />
                                                <asp:CalendarExtender ID="clnDataInicioPromocao" runat="server" PopupButtonID="ImgDtInicioPromocao"
                                                    TargetControlID="txtDtInicioPromo" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <p>
                                                    Data Fim
                                                </p>
                                                <asp:TextBox ID="txtDtFimPromo" runat="server" Width="100px" CssClass="data"></asp:TextBox>
                                                <asp:ImageButton ID="ImgDtFimPromocao" ImageUrl="~/img/calendar.png" runat="server"
                                                    Height="15px" />
                                                <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="ImgDtFimPromocao"
                                                    TargetControlID="txtDtFimPromo" Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <p>
                                                    Preço Promoção
                                                </p>
                                                <asp:TextBox ID="txtPrecoPromocao" runat="server" Width="100px" CssClass="numero"
                                                    Style="font-size: 20px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkPromocaoAutomatica" runat="server" Text="Promoção Automatica" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <div class="panelItem">
                                                    <p>
                                                        Pontos Fidelização
                                                    </p>
                                                    <asp:TextBox ID="txtPontosFidelizacao" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                                                </div>

                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="Panel6" runat="server" Style="width: 45%; float: left; margin-left: 20px; height: 100%;">
                                    <center>
                                                <h3>Atacado (cupom)</h3>
                                                <hr />
                                        </center>
                                    <table>
                                        <tr>
                                            <td>
                                                <p>
                                                    Qtde
                                                </p>
                                                <asp:TextBox ID="txtQtdeAtacado" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <p>
                                                    Margem
                                                </p>
                                                <asp:TextBox ID="txtMargemAtacado" runat="server" Width="80px" CssClass="numero"
                                                    AutoPostBack="True" OnTextChanged="txtMargemAtacado_TextChanged"></asp:TextBox>
                                            </td>
                                            <td>
                                                <p>
                                                    Preço
                                                </p>
                                                <asp:TextBox ID="txtPrecoAtacado" runat="server" Width="80px" CssClass="numero" AutoPostBack="True"
                                                    OnTextChanged="txtPrecoAtacado_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="row">
                                        <center>
                                                <h3>Terceiro Preço</h3>
                                                <hr />
                                        </center>
                                    </div>
                                    <div class="rowFlex" style="min-width:300px ">
                                        <div class="col1">
                                            <p>Margem</p>
                                            <asp:TextBox ID="txtMargemTerceiroPreco" runat="server"  CssClass="numero" style="width:80%"
                                                AutoPostBack="True" OnTextChanged="txtMargemTerceiroPreco_TextChanged"></asp:TextBox> 
                                        </div>
                                        <div class="col1">
                                            <p>Preço</p>
                                            <asp:TextBox ID="txtTerceiroPreco" runat="server"  CssClass="numero" style="width: 100%"
                                                AutoPostBack="True" OnTextChanged="txtTerceiroPreco_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <div class="DivBorda" style="float: left; width: 30%;">
                        <center>
                                    <h3>
                                        Tabela de Preço cliente</h3>
                                </center>
                        <hr />
                        <asp:ImageButton ID="btnAddTbPreco" ImageUrl="~/img/add.png" runat="server" Height="20px"
                            OnClick="btnAddTbPreco_Click" />
                        <asp:GridView ID="gridTabelaPreco" runat="server" CellPadding="4" ForeColor="#333333"
                            AutoGenerateColumns="False" GridLines="None" OnRowCommand="gridTabelaPreco_RowCommand">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/img/edit.png" Text="Excluir">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="codigo_tabela" HeaderText="Tabela" />
                                <asp:BoundField DataField="Desconto" HeaderText="Desc/Acresc %" />
                                <asp:BoundField DataField="Preco_promocao" HeaderText="Preco Promocao" />
                                
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
                        <asp:GridView ID="gridTabelaPrecoMargem" runat="server" CellPadding="4" ForeColor="#333333"
                            AutoGenerateColumns="False" GridLines="None" OnRowCommand="gridTabelaPreco_RowCommand">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/img/edit.png" Text="Excluir">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="codigo_tabela" HeaderText="Tabela" />
                                <asp:BoundField DataField="Margem" HeaderText="Margem" />
                                <asp:BoundField DataField="Preco_promocao" HeaderText="Preço Tabela" />
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
                        </td>
                        <td></td>
                        </tr>
                    </div>
                    <div class="listaDivPreco" style="width: 60%;">
                        <center>
                                        <h3>
                                            Preço Lojas</h3>
                                    </center>
                        <hr />
                        <asp:GridView ID="gridPrecoLojas" runat="server" CellPadding="4" ForeColor="#333333"
                            AutoGenerateColumns="False" GridLines="None" OnRowCommand="gridPrecoLojas_RowCommand">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/img/edit.png" Text="Editar">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="Filial" HeaderText="Filial" />
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                <asp:BoundField DataField="Saldo_Atual" HeaderText="Saldo Atual" />
                                <asp:BoundField DataField="Preco_Compra" HeaderText="Preco Compra" />
                                <asp:BoundField DataField="Preco_Custo" HeaderText="Preco Custo" />
                                <asp:BoundField DataField="Id_contrato" HeaderText="Contrato" />
                                <asp:BoundField DataField="Margem" HeaderText="Margem" />
                                <asp:BoundField DataField="Preco" HeaderText="Preco" />
                                <asp:BoundField DataField="Preco_Promocao" HeaderText="Preco Promocao" />
                                <asp:BoundField DataField="Margem_promocao" HeaderText="Margem Promocao" />
                                <asp:BoundField DataField="Data_Inicio" HeaderText="Data inicio" />
                                <asp:BoundField DataField="Data_Fim" HeaderText="Data Fim" />
                                <asp:BoundField DataField="qtde_atacado" HeaderText="Qtde Atacado" />
                                <asp:BoundField DataField="margem_atacado" HeaderText="Margem Atacado" />
                                <asp:BoundField DataField="preco_atacado" HeaderText="Preco Atacado" />
                                <asp:BoundField DataField="Preco_Custo_1" HeaderText="Custo 1" />
                                <asp:BoundField DataField="Preco_Custo_2" HeaderText="Custo 2" />
                                <asp:BoundField DataField="Estoque_Minimo" HeaderText="Estoque Minimo" />
                                <asp:BoundField DataField="Cobertura" HeaderText="Cobertura" />
                                <asp:BoundField DataField="Ultima_Entrada" HeaderText="Ultima Entrada" />
                                <asp:BoundField DataField="Data_inventario" HeaderText="Data Inventario" />
                                <asp:BoundField DataField="Promocao" HeaderText="Promocao" />
                                <asp:BoundField DataField="Promocao_automatica" HeaderText="Promocao Automatica" />
                                <asp:BoundField DataField="Sugerido" HeaderText="Sugerido" />
                                <asp:BoundField DataField="ingredientes" HeaderText="Ingredientes" />
                                <asp:BoundField DataField="marca" HeaderText="Marca" />
                                <asp:BoundField DataField="validade" HeaderText="Validade" />
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
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabItens" runat="server" HeaderText="TabPanel2">
                <HeaderTemplate>
                    Itens
                </HeaderTemplate>
                <ContentTemplate>
                    <div id="divPluReceita" runat="server" class="itensPLUReceita">
                        <div class="panelItem" style="width: 8%; margin-left: 15%">
                            <p>PLU-Receita</p>
                            <asp:TextBox ID="txtPluReceita" runat="server" Font-Size="Medium" Width="80px"
                                CssClass="numero" MaxLength="20"
                                AutoPostBack="true" OnTextChanged="txtPluReceita_TextChanged"></asp:TextBox>
                        </div>
                        <div class="panelItem" style="width: 20%;">
                            <p>Descrição Resumida</p>
                            <asp:TextBox ID="txtPluReceitaDescricao" runat="server" Width="100%"></asp:TextBox>
                        </div>
                        <div class="panelItem " style="width: 30%; margin-right: 10%; float: right">
                            <div class="panelItem" style="width: 50%;">
                                <p>Tipo</p>
                                <asp:DropDownList ID="ddlTipoProducao" runat="server">
                                    <asp:ListItem>PRODUCAO</asp:ListItem>
                                    <asp:ListItem>ENCOMENDA</asp:ListItem>
                                    <asp:ListItem>MISTO</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="panelItem" style="width: 40%;">
                                <p>Agrupamento</p>
                                <asp:DropDownList ID="ddlAgrupamento" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAgrupamento_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>



                    </div>

                    <div id="divItensIncluidos" runat="server" class="itensIncluidos">
                        <asp:Panel runat="server" CssClass="row" DefaultButton="btnAddItem">
                            <div class="panelItem">

                                <p>
                                    Plu
                                </p>
                                <asp:TextBox ID="txtPluItem" runat="server" Width="80px"></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_txtPluItem" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                            </div>
                            <div class="panelItem">
                                <p>
                                    Descrição
                                </p>
                                <asp:TextBox ID="txtDescricaoItem" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Fator
                                </p>
                                <asp:TextBox ID="txtFatorConversaoItem" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                            </div>

                            <div class="panelItem">
                                <p>
                                    Preco Compra
                                </p>
                                <asp:TextBox ID="txtPrecoCompraItem" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>
                                    Qtde
                                </p>
                                <asp:TextBox ID="txtQtdeItem" runat="server" CssClass="numero" Width="50px"></asp:TextBox>
                            </div>


                            <div class="panelItem">
                                <p>
                                    Und
                                </p>
                                <asp:TextBox ID="txtUndProducaoItem" runat="server" CssClass="numero" Width="50px"
                                    Enabled="false"></asp:TextBox>

                            </div>

                            <div class="panelItem">

                                <div id="divAddItem" runat="server">
                                    <br />
                                    <asp:ImageButton ID="btnAddItem" ImageUrl="~/img/add.png" runat="server" Height="20px"
                                        OnClick="btnConfirmaAddItem_Click" />

                                </div>
                            </div>

                        </asp:Panel>

                        <asp:GridView ID="gridItens" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                            AutoGenerateColumns="False" OnRowCommand="gridItens_RowCommand">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/img/edit.png" Text="Editar" CommandName="Editar">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>

                                <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" Text="Excluir" CommandName="Excluir">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="plu_item" HeaderText="Plu item" />
                                <asp:BoundField DataField="Descricao" HeaderText="Descricao" />
                                <asp:BoundField DataField="Fator_conversao" HeaderText="Fator Embalagem " ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="preco_compra" HeaderText="Preco de compra"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="preco_custo" HeaderText="Preco Considerado"
                                    ItemStyle-HorizontalAlign="Right" />

                                <asp:BoundField DataField="Qtde" HeaderText="Qtde Utilizada"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="und" HeaderText="Und" />

                                <asp:BoundField DataField="Custo_Unitario" HeaderText="Custo Unitario"
                                    ItemStyle-HorizontalAlign="Right" />

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

                    <div id="divTotalisReceita" runat="server" class="itensTotais">

                        <div class="itensProgramacao">
                            <p>Programação Semanal</p>
                            <table class="gridBorda">
                                <tr>
                                    <th>Seg</th>
                                    <th>Ter</th>
                                    <th>Qua</th>
                                    <th>Qui</th>
                                    <th>Sex</th>
                                    <th>Sab</th>
                                    <th>Dom</th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkProgSeg" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkProgTer" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkProgQua" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkProgQui" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkProgSex" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkProgSab" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkProgDom" runat="server" /></td>
                                </tr>
                            </table>
                        </div>

                        <table style="margin: auto; text-align: right; font-size: 10px;">
                            <tr>
                                <td>Multiplicador:</td>
                                <td>
                                    <asp:TextBox ID="txtmultiplicador_producao" runat="server" CssClass="numero" Font-Size="Medium" Width="100px"></asp:TextBox></td>
                                <td>UND</td>
                            </tr>
                            <tr>
                                <td>Produto unitario:</td>
                                <td>
                                    <asp:TextBox ID="txtpeso_receita_unitario" runat="server" CssClass="numero" Font-Size="Medium" Width="100px"></asp:TextBox></td>
                                <td>Kg</td>
                            </tr>
                            <tr>
                                <td>Rendimento desta Receita:</td>
                                <td>
                                    <asp:TextBox ID="txtQtdReceita" runat="server" CssClass="numero" Font-Size="Medium" Width="100px"></asp:TextBox></td>
                                <td>Kg</td>
                            </tr>
                            <tr>
                                <td>Custo Total desta Receita:R$</td>
                                <td>
                                    <asp:TextBox ID="txtCustoTotalReceita" runat="server" Font-Size="Medium" Width="100px" CssClass="numero"></asp:TextBox></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Valor Unit:R$</td>
                                <td>
                                    <asp:TextBox ID="txtValorUnitTransf" runat="server" Font-Size="Medium" Width="100px" CssClass="numero"></asp:TextBox></td>
                                <td></td>
                            </tr>

                        </table>


                        <div class="panelItem" style="width: 100%;">
                            <p style="text-align: center; font-size: medium;">Receita</p>

                            <br />
                            <asp:TextBox ID="txtReceita" runat="server" Height="300px" Width="100%" TextMode="MultiLine"></asp:TextBox>


                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabEstoque" runat="server" HeaderText="TabPanel2">
                <HeaderTemplate>
                    Estoque
                </HeaderTemplate>
                <ContentTemplate>

                    <div class="panel divEstoque" style="width: 25%;">
                        <center>
                            <h3>Geral</h3>
                        </center>
                        <hr />
                        <div class="panelItem estoqueGeral">
                            <p>Estoque mínimo</p>
                            <asp:TextBox ID="txtEstoqueMinimo" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                        </div>
                        <div class="panelItem estoqueGeral">
                            <p>
                                Embalagem(compra)
                            </p>
                            <div class="row">
                                <div class="panelItem">
                                    <asp:TextBox ID="txtEmbalagem" runat="server" CssClass="numero" Width="65px"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtUndCompra" runat="server" CssClass="numero" Width="50px"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtn_txtUndCompra" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="panelItem estoqueGeral">

                            <div id="divSaldo" runat="server" class="panelItem" visible="false">
                                <p>
                                    Saldo Atual
                                </p>
                                <asp:TextBox ID="txtSaldo" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                            </div>
                        </div>
                        <div class="panelItem estoqueGeral">
                            <p>
                                Fator
                            </p>
                            <asp:TextBox ID="txtFator" runat="server" CssClass="numero" Width="65px"></asp:TextBox>
                        </div>
                        <div class="panelItem " style="width: 100%;">
                            <asp:CheckBox ID="chkAvisaEstoqueMinimo" runat="server" Text="Avisa Estoque Minimo" />
                        </div>
                        <div class="panelItem">
                            <asp:Button ID="btnKardex" runat="server" Text="Kardex" OnClick="btnKardex_Click"  Visible="false"/>
                        </div>

                    </div>

                    <div class="panel divEstoque" style="width: 15%;">

                        <center>
                        <h3>
                            Detalhado</h3>
                    </center>
                        <hr />

                        <div class="panelItem">
                            <p>
                                Média Mensal
                            </p>
                            <asp:TextBox ID="txtMediaMensal" runat="server" CssClass="numero" Width="100%"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Cobertura Dia(s)
                            </p>
                            <asp:TextBox ID="txtCoberturaDias" runat="server" Width="100%"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>
                                Margem de Erro
                            </p>
                            <asp:TextBox ID="txtMargemErro" runat="server" CssClass="numero" Width="100%"></asp:TextBox>
                        </div>
                    </div>
                    <div class="panel divEstoque" style="width: 24%;">

                        <center>
                        <h3>
                            Curva ABC</h3>
                    </center>
                        <hr />
                        <div class="panelItem">
                            <asp:CheckBox ID="chkCurvaA" runat="server" Text="Curva A" />
                        </div>
                        <div class="panelItem">
                            <asp:CheckBox ID="chkCurvaB" runat="server" Text="Curva B" />
                        </div>
                        <div class="panelItem">
                            <asp:CheckBox ID="chkCurvaC" runat="server" Text="Curva C" />
                        </div>
                    </div>

                    <div class="panel divEstoque" style="width: 28%">
                        <h3>Conta Financeira Associada</h3>

                        <div id="divLimparCentroCusto" runat="server" class="btnImprimirDireita">
                            <asp:ImageButton ID="imgBtnLimparCentroCusto" runat="server" ImageUrl="~/img/botao-apagar.png"
                                Height="15px" OnClick="imgBtnLimparCentroCusto_Click" />
                            Limpar
                        </div>
                        <hr />
                        <div class="panelItem">
                            <p>Grupo</p>
                            <asp:TextBox ID="txtGrupoCentroCusto" runat="server" Width="30%" CssClass="numero"></asp:TextBox>
                            <asp:TextBox ID="txtDescricaoGrupoCentroCusto" runat="server" Width="55%"></asp:TextBox>
                            <asp:ImageButton ID="imgBtn_txtGrupoCentroCusto" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" OnClick="Img_Click" />
                        </div>
                        <div class="panelItem">
                            <p>Sub Grupo</p>
                            <asp:TextBox ID="txtSubGrupoCentroCusto" runat="server" Width="30%" CssClass="numero"></asp:TextBox>
                            <asp:TextBox ID="txtDescricaoSubGrupoCentroCusto" runat="server" Width="55%"></asp:TextBox>
                            <asp:ImageButton ID="imgBtn_txtSubGrupoCentroCusto" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" OnClick="Img_Click" />

                        </div>

                        <div class="panelItem">
                            <p>
                                Centro Custo
                            </p>
                            <asp:TextBox ID="txtCentroCusto" runat="server" Width="30%" CssClass="numero"></asp:TextBox>
                            <asp:TextBox ID="txtDescricaoCentroCusto" runat="server" Width="55%"></asp:TextBox>
                            <asp:ImageButton ID="imgBtn_TxtCentroCusto" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" OnClick="Img_Click" />


                        </div>

                    </div>
                    <div class="row">
                        <div id="divPluVinculado" runat="server" class="panel" style="width: 60%;">
                            <h3 style="text-align: center">PLU Vinculado</h3>
                            <hr />
                            <div class="panelItem">
                                <p>PLU</p>
                                <asp:TextBox ID="txtPLUVinculado" runat="server" CssClass="numero" Width="100px" AutoPostBack="true" OnTextChanged="txtPLUVinculado_TextChanged" />
                            </div>
                            <div class="panelItem">
                                <p>Descrição</p>
                                <asp:TextBox ID="txtDescricaoVinculado" runat="server" MaxLength="60" Width="300px" />
                            </div>
                            <div class="panelItem">
                                <p>Fator Estoque</p>
                                <asp:TextBox ID="txtFatorVinculado" runat="server" CssClass="numero" Width="100px" />
                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <center>
                                 <hr />
                        <h3>
                            últimos 10 dias de movimentações</h3>
                        <hr />
                        <asp:Label id="NaoControlaEstoque" runat="server" text="ITEM NÃO CONTROLA ESTOQUE" visible="false"/>
                        <asp:GridView ID="Grid10Dias" runat="server" CellPadding="4" ForeColor="#333333" 
                            GridLines="None" AutoGenerateColumns="false" >
                            <Columns>
                                <asp:BoundField DataField="Data" HeaderText="Data" />
                                <asp:BoundField DataField="Qtde_Inicial" HeaderText="Inicial"  DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" ItemStyle-Width="100px"/>
                                <asp:BoundField DataField="Entrada_NFe" HeaderText="Ent NFe" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" ItemStyle-Width="100px"/>
                                <asp:BoundField DataField="Entrada_Outras" HeaderText="Ent Outras" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" ItemStyle-Width="100px"/>
                                <asp:BoundField DataField="Saida_NFe" HeaderText="Sai NFe" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" ItemStyle-Width="100px"/>
                                <asp:BoundField DataField="Saida_Outras" HeaderText="Sai Outras" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" ItemStyle-Width="100px"/>
                                <asp:BoundField DataField="Saida_Cupom" HeaderText="Sai Cupom" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" ItemStyle-Width="100px"/>
                                <asp:BoundField DataField="Saldo" HeaderText="Saldo" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-Font-Bold="true" />
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
                    </center>
                    </div>
                    <div class="row">
                        <div class="panel" style="width: 60%;">
                            <h3 style="text-align: center">Apresentação da Produção</h3>
                            <hr />
                            <div class="panelItem">
                                <p>Descrição (produção)</p>
                                <asp:TextBox ID="txtDescricao_producao" runat="server" MaxLength="60" Width="250px" />
                            </div>
                            <div class="panelItem">
                                <p>Preço Compra</p>
                                <asp:TextBox ID="txtPrecoCompraProducao" runat="server" CssClass="numero" Width="100px" />
                            </div>

                            <div class="panelItem">
                                <p>Embalagem </p>
                                <asp:TextBox ID="txtEmbalagemProducao" AutoPostBack="true" OnTextChanged="txtEmbalagemProducao_TextChanged" runat="server" CssClass="numero" Width="80px" />
                            </div>
                            <div class="panelItem">
                                <h4>/</h4>
                            </div>
                            <div class="panelItem">
                                <p>Und </p>
                                <asp:TextBox ID="txtUndProducao_2" runat="server" Width="50px" />
                                <asp:ImageButton ID="imgBtn_txtUndProducao2" runat="server" ImageUrl="~/img/pesquisaM.png"
                                    Height="15px" OnClick="Img_Click" />
                            </div>
                            <div class="panelItem">
                                <h4>=</h4>
                            </div>

                            <div class="panelItem">
                                <p>Custo Produção </p>
                                <asp:TextBox ID="txtCustoProducao" runat="server" CssClass="numero" Width="100px" />
                            </div>
                        </div>

                    </div>

                    <div id="divSazonalidade" runat="server" visible="false">
                        <center>
                        <h3>
                            Sazonalidade</h3>
                    </center>
                        <hr />
                        <table>
                            <tr>
                                <td>De
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDtSazoDe1" runat="server" Width="90px"></asp:TextBox>
                                    <asp:ImageButton ID="imgDtSazoDe1" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                                    <asp:CalendarExtender ID="cldDtSazoDe1" runat="server" PopupButtonID="imgDtSazoDe1"
                                        TargetControlID="txtDtSazoDe1" Enabled="True">
                                    </asp:CalendarExtender>
                                </td>
                                <td>a
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDtSazoA1" runat="server" Width="90px"></asp:TextBox>
                                    <asp:ImageButton ID="imgDtSazoA1" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                                    <asp:CalendarExtender ID="cldDtSazoA1" runat="server" PopupButtonID="imgDtSazoA1"
                                        TargetControlID="txtDtSazoA1" Enabled="True">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>De
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDtSazoDe2" runat="server" Width="90px"></asp:TextBox>
                                    <asp:ImageButton ID="imgDtSazoDe2" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                                    <asp:CalendarExtender ID="clDtSazoDe2" runat="server" PopupButtonID="imgDtSazoDe2"
                                        TargetControlID="txtDtSazoDe2" Enabled="True">
                                    </asp:CalendarExtender>
                                </td>
                                <td>a
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDtSazoA2" runat="server" Width="90px"></asp:TextBox>
                                    <asp:ImageButton ID="imgDtSazoA2" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                                    <asp:CalendarExtender ID="clnDtSazoA2" runat="server" PopupButtonID="imgDtSazoA2"
                                        TargetControlID="txtDtSazoA2" Enabled="True">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>De
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDtSazoDe3" runat="server" Width="90px"></asp:TextBox>
                                    <asp:ImageButton ID="imgDtSazoDe3" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                                    <asp:CalendarExtender ID="clnDtSazoDe3" runat="server" PopupButtonID="imgDtSazoDe3"
                                        TargetControlID="txtDtSazoDe3" Enabled="True">
                                    </asp:CalendarExtender>
                                </td>
                                <td>a
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDtSazoA3" runat="server" Width="90px"></asp:TextBox>
                                    <asp:ImageButton ID="imgDtSazoA3" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                                    <asp:CalendarExtender ID="clnDtSazoA3" runat="server" PopupButtonID="imgDtSazoA3"
                                        TargetControlID="txtDtSazoA3" Enabled="True">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="row">
                        <center>
                                 <hr />
                        <h3>
                            Historico de Entradas</h3>
                        <hr />
                        <asp:GridView ID="Gridhistorico" runat="server" CellPadding="4" ForeColor="#333333"
                            GridLines="None" AutoGenerateColumns="false">
                             <Columns>
                             <asp:HyperLinkField DataTextField="Filial" Text="---" Visible="true" HeaderText="FILIAL" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?tela=NF001&codigo={0}&fornecedor={1}&serie={2}"
                                DataNavigateUrlFields="Documento,fornecedor,serie" SortExpression="a.PLU" />
                             <asp:HyperLinkField DataTextField="Data" Text="---" Visible="true" HeaderText="Data" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?tela=NF001&codigo={0}&fornecedor={1}&serie={2}"
                                DataNavigateUrlFields="Documento,fornecedor,serie" SortExpression="a.PLU" />
                             <asp:HyperLinkField DataTextField="Fornecedor" Text="---" Visible="true" HeaderText="Fornecedor"  target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?tela=NF001&codigo={0}&fornecedor={1}&serie={2}"
                                DataNavigateUrlFields="Documento,fornecedor,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="Qtde" Text="---" Visible="true" HeaderText="Qtde" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?tela=NF001&codigo={0}&fornecedor={1}&serie={2}"
                                DataNavigateUrlFields="Documento,fornecedor,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="Preco" Text="---" Visible="true" HeaderText="Preco" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?tela=NF001&codigo={0}&fornecedor={1}&serie={2}"
                                DataNavigateUrlFields="Documento,fornecedor,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="IPI" Text="---" Visible="true" HeaderText="IPI" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?tela=NF001&codigo={0}&fornecedor={1}&serie={2}"
                                DataNavigateUrlFields="Documento,fornecedor,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="IVA" Text="---" Visible="true" HeaderText="IVA" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?tela=NF001&codigo={0}&fornecedor={1}&serie={2}"
                                DataNavigateUrlFields="Documento,fornecedor,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="Documento" Text="---" Visible="true" HeaderText="Documento" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?tela=NF001&codigo={0}&fornecedor={1}&serie={2}"
                                DataNavigateUrlFields="Documento,fornecedor,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="CFOP" Text="---" Visible="true" HeaderText="CFOP" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?tela=NF001&codigo={0}&fornecedor={1}&serie={2}"
                                DataNavigateUrlFields="Documento,fornecedor,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="Natureza" Text="---" Visible="true" HeaderText="Natureza" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?tela=NF001&codigo={0}&fornecedor={1}&serie={2}"
                                DataNavigateUrlFields="Documento,fornecedor,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="Usuario" Text="---" Visible="true" HeaderText="Usuario" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?tela=NF001&codigo={0}&fornecedor={1}&serie={2}"
                                DataNavigateUrlFields="Documento,fornecedor,serie" SortExpression="a.PLU" />
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
                    </center>
                    </div>
                    <div class="row">
                        <center>
                                 <hr />
                        <h3>
                            Historico de Saídas</h3>
                        <hr />
                        <asp:GridView ID="GridhistoricoSaida" runat="server" CellPadding="4" ForeColor="#333333"
                            GridLines="None" AutoGenerateColumns="false">
                             <Columns>
                             <asp:HyperLinkField DataTextField="Filial" Text="---" Visible="true" HeaderText="FILIAL" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?tela=NF002&codigo={0}&cliente={1}"
                                DataNavigateUrlFields="Documento,Cliente,serie" SortExpression="a.PLU" />
                             <asp:HyperLinkField DataTextField="Emissao" Text="---" Visible="true" HeaderText="Emissao" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?tela=NF002&codigo={0}&cliente={1}"
                                DataNavigateUrlFields="Documento,Cliente,serie" SortExpression="a.PLU" />
                             <asp:HyperLinkField DataTextField="Cliente" Text="---" Visible="true" HeaderText="Cliente"  target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?tela=NF002&codigo={0}&cliente={1}"
                                DataNavigateUrlFields="Documento,Cliente,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="Qtde" Text="---" Visible="true" HeaderText="Qtde" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?tela=NF002&codigo={0}&cliente={1}"
                                DataNavigateUrlFields="Documento,Cliente,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="Preco" Text="---" Visible="true" HeaderText="Preco" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?tela=NF002&codigo={0}&cliente={1}"
                                DataNavigateUrlFields="Documento,Cliente,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="IPI" Text="---" Visible="true" HeaderText="IPI" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?tela=NF002&codigo={0}&cliente={1}"
                                DataNavigateUrlFields="Documento,Cliente,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="IVA" Text="---" Visible="true" HeaderText="IVA" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?tela=NF002&codigo={0}&cliente={1}"
                                DataNavigateUrlFields="Documento,Cliente,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="Documento" Text="---" Visible="true" HeaderText="Documento" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?tela=NF002&codigo={0}&cliente={1}"
                                DataNavigateUrlFields="Documento,Cliente,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="CFOP" Text="---" Visible="true" HeaderText="CFOP" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?tela=NF002&codigo={0}&cliente={1}"
                                DataNavigateUrlFields="Documento,Cliente,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="Natureza" Text="---" Visible="true" HeaderText="Natureza" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?tela=NF002&codigo={0}&cliente={1}"
                                DataNavigateUrlFields="Documento,Cliente,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="Usuario" Text="---" Visible="true" HeaderText="Usuario" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?tela=NF002&codigo={0}&cliente={1}"
                                DataNavigateUrlFields="Documento,Cliente,serie" SortExpression="a.PLU" />
                                <asp:HyperLinkField DataTextField="Estoque" Text="---" Visible="true" HeaderText="Estoque" target="_blank"
                                DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?tela=NF002&codigo={0}&cliente={1}"
                                DataNavigateUrlFields="Documento,Cliente,serie" SortExpression="a.PLU" />
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
                    </center>
                    </div>

                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabTributacao" runat="server" HeaderText="TabPanel2">
                <HeaderTemplate>
                    Tributação
                </HeaderTemplate>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td colspan="2">
                                <center>
                                    ICMS</center>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="border-style: solid; border-width: 1px;">
                                <table>
                                    <tr>
                                        <td colspan="3">
                                            <center>
                                                <p>
                                                    Entrada</p>
                                            </center>
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <p>
                                                Tributação
                                            </p>
                                            <asp:TextBox ID="txtCodTribEntrada" runat="server" Width="30px" CssClass="numero"></asp:TextBox>&nbsp;
                                            <asp:TextBox ID="txtTribEntrada" runat="server" Width="250px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtn_TxtTribEntrada" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" OnClick="Img_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="border-style: solid; border-width: 1px;">
                                <table>
                                    <tr>
                                        <td colspan="3">
                                            <center>
                                                <p>
                                                    Saida</p>
                                            </center>
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <p>
                                                Tributação
                                            </p>
                                            <asp:TextBox ID="txtCodTribSaida" runat="server" Width="30px" CssClass="numero"></asp:TextBox>&nbsp;
                                            <asp:TextBox ID="txtTribuSaida" runat="server" Width="250px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtn_TxtTribuSaida" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" OnClick="Img_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                                <center>
                                    PIS/COFINS</center>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkPisConfins" runat="server" Text="Incide Pis " />
                            </td>

                        </tr>
                        <tr>
                            <td style="border-style: solid; border-width: 1px;">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="3">
                                            <center>
                                                <p>
                                                    Entrada</p>
                                            </center>
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>
                                                CST
                                            </p>
                                            <asp:TextBox ID="txtCSTEntrada" runat="server" Width="50px" CssClass="numero"></asp:TextBox>&nbsp;
                                            <asp:ImageButton ID="imgBtn_txtCSTEntrada" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" OnClick="Img_Click" />
                                        </td>
                                        <td>
                                            <p>
                                                PIS %
                                            </p>
                                            <asp:TextBox ID="txtPisEntrada" runat="server" Width="80px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                COFINS %
                                            </p>
                                            <asp:TextBox ID="txtCofinsEntrada" runat="server" Width="80px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="border-style: solid; border-width: 1px;">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="3">
                                            <center>
                                                <p>
                                                    Saida</p>
                                            </center>
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>
                                                CST
                                            </p>
                                            <asp:TextBox ID="txtCSTSaida" runat="server" Width="50px" CssClass="numero"></asp:TextBox>&nbsp;
                                            <asp:ImageButton ID="imgBtn_txtCSTSaida" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" OnClick="Img_Click" />
                                        </td>
                                        <td>
                                            <p>
                                                PIS %
                                            </p>
                                            <asp:TextBox ID="txtPisSaida" runat="server" Width="80px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                COFINS %
                                            </p>
                                            <asp:TextBox ID="txtCofinsSaida" runat="server" Width="80px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <center>
                                <br />
                                    <p>
                                        OUTROS</p>
                                    <hr />
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table style="border-style: solid; border-width: 1px;">
                                    <tr>
                                        <td>
                                            <p>
                                                Ipi
                                            </p>
                                            <asp:TextBox ID="txtIpi" runat="server" Width="80px" CssClass="numero"></asp:TextBox>%
                                        </td>

                                        <td>
                                            <p>
                                                Portaria
                                            </p>
                                            <asp:TextBox ID="txtPortaria" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                NCM
                                            </p>
                                            <asp:TextBox ID="txtClassifFiscal" runat="server" Width="100px" CssClass="numero"
                                                MaxLength="8"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtn_txtClassifFiscal" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" OnClick="Img_Click" />
                                        </td>
                                        <td>
                                            <p>
                                                IVA
                                            </p>
                                            <asp:TextBox ID="txtIva" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                CEST
                                            </p>
                                            <asp:TextBox ID="txtCEST" runat="server" Width="100px" CssClass="numero" MaxLength="8"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtn_txtCEST" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" OnClick="Img_Click" />
                                        </td>
                                        <td>
                                            <p>
                                                Cod Beneficio
                                            </p>
                                            <asp:TextBox ID="txtcBenef" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>
                                                Artigo
                                            </p>
                                            <asp:TextBox ID="txtArtigo" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtn_txtArtigo" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" OnClick="Img_Click" />
                                        </td>
                                        <td>
                                            <p>
                                                Pauta
                                            </p>
                                            <asp:TextBox ID="txtPauta" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                N. Exceção
                                            </p>
                                            <asp:TextBox ID="txtNExcecao" runat="server" Width="100px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                Nat. Receita
                                            </p>
                                            <asp:TextBox ID="txtNatReceita" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <p>
                                                Mod Base ST
                                            </p>
                                            <asp:DropDownList ID="ddlModalidadeBaseST" runat="server">
                                                <asp:ListItem Value="-1">----</asp:ListItem>
                                                <asp:ListItem Value="0">Preço Tabelado ou Maximo Sugerido</asp:ListItem>
                                                <asp:ListItem Value="1">Lista Negativa</asp:ListItem>
                                                <asp:ListItem Value="2">Lista Positiva</asp:ListItem>
                                                <asp:ListItem Value="3">Lista Neutra</asp:ListItem>
                                                <asp:ListItem Value="4">Margem Valor Agregado</asp:ListItem>
                                                <asp:ListItem Value="5">Pauta</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <div class="panelItem">
                                                <p>
                                                    Origem Mercadoria
                                                </p>
                                                <asp:DropDownList ID="ddlOrigem" runat="server" Width="350px">
                                                    <asp:ListItem Value="0">Nacional</asp:ListItem>
                                                    <asp:ListItem Value="1">Estrangeira – Importação direta</asp:ListItem>
                                                    <asp:ListItem Value="2">Estrangeira – Adquirida no mercado interno</asp:ListItem>
                                                    <asp:ListItem Value="3">Nacional - Mercadoria/bem Imp sup 40%</asp:ListItem>
                                                    <asp:ListItem Value="4">Nacional - Produção Decreto-Lei nº 288/67</asp:ListItem>
                                                    <asp:ListItem Value="5">Nacional - Mercadoria/bem Imp inf ou igual 40%</asp:ListItem>
                                                    <asp:ListItem Value="6">Estrangeira - Importação direta, sem similar nacional</asp:ListItem>
                                                    <asp:ListItem Value="7">Estrangeira - Adquirida no mercado interno, sem similar nacional</asp:ListItem>
                                                    <asp:ListItem Value="8">Estrangeira - Mercadoria/bem com conteúdo imp superior a 70%</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="panelItem">
                                                <p>
                                                    Escala Relevante
                                                </p>
                                                <asp:DropDownList ID="ddlIndEscala" runat="server" Width="100px">
                                                    <asp:ListItem Value="S" Selected="true">Sim</asp:ListItem>
                                                    <asp:ListItem Value="N">Não</asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="panelItem">
                                                <p>Cnpj Fabricante</p>
                                                <asp:TextBox ID="txtCNPJFabricante" runat="server" CssClass="CNPJ" MaxLength="18" Width="150px" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>CFOP</p>
                                            <asp:TextBox ID="txtCFOP" Width="50" MaxLength="4" runat="server" />
                                            <asp:ImageButton ID="imgBtn_txtCFOP" runat="server" ImageUrl="~/img/pesquisaM.png"
                                                Height="15px" OnClick="Img_Click" />

                                        </td>
                                        <td>
                                            <p>Código NFe XML</p>
                                            <asp:TextBox ID="txtCodigoEmissaoNFe" Width="150" MaxLength="4" runat="server" />

                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <center>
                                <br />
                                    <p>
                                        IBS/CBS/IS</p>
                                    <hr />
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table style="border-style: solid; border-width: 1px;">
                                    <tr>
                                        <td>
                                            <p>
                                                CST (Código de Situação Tributária)
                                            </p>
                                            <asp:DropDownList ID="ddlCSTcTrib" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>
                                                Código de Classificação Tributária (cClassTrib)
                                            </p>
                                            <asp:DropDownList ID="ddlcClassTrib" runat="server" Width="90%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>
                                                Alíquota IVA (cClassTrib)
                                            </p>
                                            <asp:TextBox ID="txtAliquotaIVA_cTrib" runat="server" Width="100px" CssClass="numero"></asp:TextBox>

                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabObservacao" runat="server" HeaderText="TabPanel2">
                <HeaderTemplate>
                    Observação
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="panelItem" style="border-style: solid; border-width: 1px; width: 40%;">
                        <table>
                            <tr>
                                <td>
                                    <p>
                                        Observação
                                    </p>
                                    <asp:TextBox ID="txtAddObs" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                    <p>
                                        Tipo (cardapio)
                                    </p>
                                    <asp:DropDownList ID="ddlTipoObs" runat="server" Enabled="true">
                                        <asp:ListItem Text="escolha" />
                                        <asp:ListItem Text="escolha-unica" />
                                        <asp:ListItem Text="texto" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <p>
                                        Plu Item Adc
                                    </p>
                                    <asp:TextBox ID="txtPluItemAdc" runat="server" Width="40px"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtn_txtPluItemAdc" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <asp:CheckBox ID="chkObsObrigatorio" runat="server" Text="Obrigatorio" AutoPostBack="true" OnCheckedChanged="chkObsObrigatorio_CheckedChanged" />
                                </td>
                                <td>
                                    <p>
                                        Ordem
                                    </p>
                                    <asp:DropDownList ID="ddlObrigatorioOrdem" runat="server" Enabled="false">
                                        <asp:ListItem Text="0" />
                                        <asp:ListItem Text="1" />
                                        <asp:ListItem Text="2" />
                                        <asp:ListItem Text="3" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <br />
                                    <div id="divIncluirObs" runat="server">
                                        <asp:ImageButton ID="imgAddObservacao" ImageUrl="~/img/add.png" runat="server" Height="20px"
                                            OnClick="imgAddObservacao_Click" />
                                        incluir
                                    </div>
                                </td>

                            </tr>
                        </table>
                        <asp:GridView ID="gridObs" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                            AutoGenerateColumns="False" OnRowCommand="gridObs_RowCommand">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" Text="Excluir">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="obs" HeaderText="Observação" />
                                <asp:BoundField DataField="plu_item_adc" HeaderText="Plu Adc" />
                                <asp:BoundField DataField="obrigatorio" HeaderText="Obrigatorio" />
                                <asp:BoundField DataField="obrigatorioOrdem" HeaderText="Ordem" />
                                <asp:BoundField DataField="tipoCardapio" HeaderText="Tipo(Cardapio)" />

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
                    <div class="panelItem" style="border-style: solid; border-width: 1px; width: 55%; margin-left: 10px;">
                        <table>
                            <tr>
                                <td>
                                    <p>
                                        Loja
                                    </p>
                                    <asp:TextBox ID="txtLojaImp" runat="server" Width="50px" AutoPostBack="true" OnTextChanged="txtLojaImp_TextChange"></asp:TextBox>
                                    <asp:TextBox ID="txtLojaFilial" runat="server" Width="100px"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtn_imgBtnLojaImp" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />
                                </td>
                                <td>
                                    <p>
                                        Impressora
                                    </p>
                                    <asp:TextBox ID="txtImpressora" runat="server" Width="50px" AutoPostBack="true" OnTextChanged="txtImpressora_TextChange"></asp:TextBox>
                                    <asp:TextBox ID="txtImpressoraDescricao" runat="server" Width="100px"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtn_imgBtnImpressora" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />
                                </td>
                                <td>
                                    <p>
                                        Observação
                                    </p>
                                    <asp:TextBox ID="txtObsImpressora" runat="server" Width="190px" MaxLength="20"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtn_imgObsImpressora" runat="server" ImageUrl="~/img/pesquisaM.png"
                                        Height="15px" OnClick="Img_Click" />
                                </td>
                                <td>
                                    <br />
                                    <asp:ImageButton ID="imgBtnAddImpressora" ImageUrl="~/img/add.png" runat="server"
                                        Height="20px" OnClick="imgBtnAddImpressora_Click" />
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gridImpressoras" runat="server" CellPadding="4" ForeColor="#333333"
                            GridLines="None" AutoGenerateColumns="False" OnRowCommand="gridImpressora_RowCommand">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" Text="Excluir">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="LOJA" HeaderText="LOJA" />
                                <asp:BoundField DataField="FILIAL" HeaderText="FILIAL" />
                                <asp:BoundField DataField="IMPRESSORA_REMOTA" HeaderText="IMP" />
                                <asp:BoundField DataField="DESCRICAO" HeaderText="IMPRESSORA" />
                                <asp:BoundField DataField="OBSERVACAO" HeaderText="OBSERVACAO" />
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
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabVendasECF" runat="server" HeaderText="TabPanel2">
                <HeaderTemplate>
                    Vendas 
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="Panel5" runat="server" Height="700px">
                        <div class="vendaMercadoria">
                            Venda ultimos 13 meses
                            <asp:GridView class="gridTable" ID="GridVendas" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" CssClass="FrameDivisaoTela" AutoGenerateColumns="False" ShowFooter="true"
                                OnDataBound="Grid_DataBound">
                                <Columns>
                                    <asp:BoundField DataField="MesAno" HeaderText="Mes/Ano" />
                                    <asp:BoundField DataField="Qtde" HeaderText="Qtde" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="vlr" HeaderText="Vlr" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="PrcMD" HeaderText="MD" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
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
                        <div class="vendaMercadoria">
                            Venda ultimos 30 dias
                            <asp:GridView class="gridTable" ID="GridVendasDia" runat="server" CellPadding="4"
                                ForeColor="#333333" GridLines="None" CssClass="FrameDivisaoTela" AutoGenerateColumns="False"
                                ShowFooter="true" OnDataBound="GridVD_DataBound" OnRowDataBound="gridVendasDia_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="Dia" HeaderText="Dia" />
                                    <asp:BoundField DataField="Qtde" HeaderText="Qtde" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="vlr" HeaderText="Vlr" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="PrcMD" HeaderText="MD" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
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
                        <div class="vendaGrafico">
                            <asp:Chart ID="Grafico01" runat="server" Width="500px">
                                <Series>
                                    <asp:Series ChartArea="ChartArea1" Legend="Legend1" Name="Qtde" XValueMember="MesAno"
                                        YValueMembers="Qtde">
                                    </asp:Series>
                                    <asp:Series ChartArea="ChartArea1" Color="red" Legend="Legend1" Name="Vlr" XValueMember="MesAno"
                                        YValueMembers="Vlr">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1">
                                    </asp:ChartArea>
                                </ChartAreas>
                                <Legends>
                                    <asp:Legend Name="Legend1">
                                    </asp:Legend>
                                </Legends>
                                <Titles>
                                    <asp:Title Font="Microsoft Sans Serif, 8pt, style=Bold" Name="title01" Text="Qtde/Vlr ultimos 13 meses">
                                    </asp:Title>
                                </Titles>
                            </asp:Chart>
                            <asp:Chart ID="Grafico02" runat="server" Width="500px">
                                <Series>
                                    <asp:Series ChartArea="ChartArea1" Legend="Legend1" Name="Qtde" XValueMember="Dia"
                                        YValueMembers="Qtde">
                                    </asp:Series>
                                    <asp:Series ChartArea="ChartArea1" Color="red" Legend="Legend1" Name="Vlr" XValueMember="Dia"
                                        YValueMembers="Vlr">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1">
                                    </asp:ChartArea>
                                </ChartAreas>
                                <Legends>
                                    <asp:Legend Name="Legend1">
                                    </asp:Legend>
                                </Legends>
                                <Titles>
                                    <asp:Title Font="Microsoft Sans Serif, 8pt, style=Bold" Name="title01" Text="Qtde/Vlr ultimos 30 dias">
                                    </asp:Title>
                                </Titles>
                            </asp:Chart>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabInformacaoNutricional" runat="server" HeaderText="TabPanel2">
                <HeaderTemplate>
                    Informação Nutricional
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="nutricional">

                        <div class="divNutricional">
                            <table class="tableNutricial">
                                <tr>
                                    <td colspan="4">
                                        <div style="float: left; margin-left: 10%;">
                                            Porção:
                                    <asp:TextBox ID="txtPorcao" CssClass="numero" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                            <asp:DropDownList ID="ddlMedida" runat="server">
                                                <asp:ListItem Value="0">g</asp:ListItem>
                                                <asp:ListItem Value="1">ml</asp:ListItem>
                                                <asp:ListItem Value="2">un</asp:ListItem>
                                            </asp:DropDownList>
                                            (
                                    <asp:TextBox ID="txtPorcaoNumero" CssClass="numero" runat="server" MaxLength="2"
                                        Width="50px"></asp:TextBox>
                                            <asp:DropDownList ID="ddlDiv" runat="server">
                                                <asp:ListItem Value="0">0</asp:ListItem>
                                                <asp:ListItem Value="1">1/4</asp:ListItem>
                                                <asp:ListItem Value="2">1/3</asp:ListItem>
                                                <asp:ListItem Value="3">1/2</asp:ListItem>
                                                <asp:ListItem Value="4">2/3</asp:ListItem>
                                                <asp:ListItem Value="5">3/4</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlDetalhe" runat="server">
                                                <asp:ListItem Value="00">Colher(es) de Sopa</asp:ListItem>
                                                <asp:ListItem Value="01">Colher(es) de Café</asp:ListItem>
                                                <asp:ListItem Value="02">Colher(es) de Chá</asp:ListItem>
                                                <asp:ListItem Value="03">Xícara(s)</asp:ListItem>
                                                <asp:ListItem Value="04">De Xícara(s)</asp:ListItem>
                                                <asp:ListItem Value="05">Unidade(s)</asp:ListItem>
                                                <asp:ListItem Value="06">Pacote(s)</asp:ListItem>
                                                <asp:ListItem Value="07">Fatia(s)</asp:ListItem>
                                                <asp:ListItem Value="08">Fatia(s) Fina(s)</asp:ListItem>
                                                <asp:ListItem Value="09">Pedaço(s)</asp:ListItem>
                                                <asp:ListItem Value="10">Folha(s)</asp:ListItem>
                                                <asp:ListItem Value="11">Pão(es)</asp:ListItem>
                                                <asp:ListItem Value="12">Biscoito(s)</asp:ListItem>
                                                <asp:ListItem Value="13">Bisnaguinha(s)</asp:ListItem>
                                                <asp:ListItem Value="14"> Disco(s)</asp:ListItem>
                                                <asp:ListItem Value="15"> Copo(s)</asp:ListItem>
                                                <asp:ListItem Value="16">Porção(ões)</asp:ListItem>
                                                <asp:ListItem Value="17">Tablete(s)</asp:ListItem>
                                                <asp:ListItem Value="18">Sache(s)</asp:ListItem>
                                                <asp:ListItem Value="19">Almôndega(s)</asp:ListItem>
                                                <asp:ListItem Value="20">Bife(s)</asp:ListItem>
                                                <asp:ListItem Value="21">Filé(s)</asp:ListItem>
                                                <asp:ListItem Value="22">Concha(s)</asp:ListItem>
                                                <asp:ListItem Value="23"> Bala(s)</asp:ListItem>
                                                <asp:ListItem Value="24">Prato(s) Fundo(s)</asp:ListItem>
                                                <asp:ListItem Value="25"> Pitada(s)</asp:ListItem>
                                                <asp:ListItem Value="26">Lata(s))</asp:ListItem>
                                            </asp:DropDownList>
                                            )
                                        </div>
                                    </td>
                                </tr>
                                <tr class="cab">
                                    <td></td>
                                    <td>Não Contem
                                    </td>
                                    <td>Quantidade por porção
                                    </td>
                                    <td>% Valor Diario*
                                    </td>
                                </tr>
                                <tr class="linhaImpar">
                                    <td>Valor Energético(kcal =kj)
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkVlr_energetico_nao" runat="server" Style="float: left; margin-left: 30%;"
                                            AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtvlr_energ_qtde" CssClass="numero" runat="server" Width="50px"
                                            MaxLength="4" Style="float: left; margin-left: 10%;"></asp:TextBox>
                                        =
                                <asp:TextBox ID="txtvlr_energ_qtde_igual" CssClass="numero" runat="server" Width="50px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtvlr_energ_diario" CssClass="numero" runat="server" Width="50px"
                                            Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="linhaPar">
                                    <td>Carboidratos(g)
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkCarboidratos_nao" runat="server" Style="float: left; margin-left: 30%;"
                                            AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcarboidratos_qtde" CssClass="numero" MaxLength="5" Style="float: left; margin-left: 30%;"
                                            runat="server" Width="50px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcarboidratos_vlr_diario" CssClass="numero" runat="server" Style="float: left; margin-left: 30%;"
                                            Width="50px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="linhaImpar">
                                    <td>Proteínas(g)
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkProteinas_nao" runat="server" Style="float: left; margin-left: 30%;"
                                            AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtproteinas_qtde" CssClass="numero" runat="server" MaxLength="4"
                                            Width="50px" Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtproteinas_vlr_diario" CssClass="numero" runat="server" Width="50px"
                                            Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="linhaPar">
                                    <td>Gorduras Totais(g)
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkgorduras_totais_nao" runat="server" Style="float: left; margin-left: 30%;"
                                            AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtgorduras_totais_qtde" CssClass="numero" runat="server" MaxLength="4"
                                            Width="50px" Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtgorduras_totais_vlr_diario" CssClass="numero" runat="server"
                                            Width="50px" Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="linhaImpar">
                                    <td>Gorduras Saturadas(g)
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkgorduras_satu_nao" runat="server" Style="float: left; margin-left: 30%;"
                                            AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtgorduras_satu_qtde" CssClass="numero" MaxLength="4" runat="server"
                                            Width="50px" Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtgorduras_satu_vlr_diario" CssClass="numero" runat="server" Width="50px"
                                            Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="linhaPar">
                                    <td>Gorduras Trans(g)
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkgorduras_trans_nao" runat="server" Style="float: left; margin-left: 30%;"
                                            AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtgorduras_trans_qtde" CssClass="numero" runat="server" MaxLength="4"
                                            Width="50px" Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <div style="float: left; margin-left: 30%;">
                                            ***
                                        </div>
                                    </td>
                                </tr>
                                <tr class="linhaImpar">
                                    <td>Fibras Alimentar(g)
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkfibra_alimen_nao" runat="server" Style="float: left; margin-left: 30%;"
                                            AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfibra_alimen_qtde" CssClass="numero" runat="server" MaxLength="4"
                                            Width="50px" Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfibra_alimen_vlr_diario" CssClass="numero" runat="server" Width="50px"
                                            Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="linhaPar">
                                    <td>Sódio(g)
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chksodio_nao" runat="server" Style="float: left; margin-left: 30%;"
                                            AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtsodio_qtde" CssClass="numero" runat="server" MaxLength="6" Width="50px"
                                            Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtsodio_vlr_diario" CssClass="numero" runat="server" Width="50px"
                                            Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="linhaImpar">
                                    <td>Colesterol(g)
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkcolesterol_nao" runat="server" Style="float: left; margin-left: 30%;"
                                            AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcolesterol_qtde" CssClass="numero" runat="server" MaxLength="6" Width="50px"
                                            Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcolesterol_vlr_diario" CssClass="numero" runat="server" Width="50px"
                                            Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="linhaPar">
                                    <td>Calcio(g)
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkcalcio_nao" runat="server" Style="float: left; margin-left: 30%;"
                                            AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcalcio_qtde" CssClass="numero" runat="server" MaxLength="6" Width="50px"
                                            Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcalcio_vlr_diario" CssClass="numero" runat="server" Width="50px"
                                            Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="linhaImpar">
                                    <td>Ferro(g)
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkferro_nao" runat="server" Style="float: left; margin-left: 30%;"
                                            AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtferro_qtde" CssClass="numero" runat="server" MaxLength="6" Width="50px"
                                            Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtferro_vlr_diario" CssClass="numero" runat="server" Width="50px"
                                            Style="float: left; margin-left: 30%;"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="divInformacoesExtras">
                            <h3>Informaçoes Extras</h3>
                            <br />
                            <asp:TextBox ID="txtInformacoesExtras" TextMode="MultiLine" runat="server" Height="100px" Width="100%" />
                        </div>
                    </div>

                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabEcommerce" runat="server" HeaderText="TabPanel2">
                <HeaderTemplate>
                    E-Commerce
                </HeaderTemplate>
                <ContentTemplate>
                    <div style="display:flex">
                        <div>

                            <div class="row" style="display: flex;">

                                <div style="flex: 2">
                                    <div class="row">
                                        <div class="panelItem">
                                            <p>Descrição WEB</p>
                                            <asp:TextBox ID="txtDescricaoWEB" Width="500px" MaxLength="100" runat="server" Text="" />
                                        </div>
                                        <div class="panelItem">
                                            <p>SKU</p>
                                            <asp:TextBox ID="txtSKU" Width="50px" runat="server" Text="" Enabled="False" />
                                        </div>
                                        <div class="panelItem">
                                            <asp:CheckBox ID="chkConfiguravel" runat="server" Text="Produto Configurável (Magento)" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="panelItem">
                                            <asp:CheckBox ID="chkIntegraWS" runat="server" Text="Integra WS" OnCheckedChanged="chkIntegraWS_CheckedChanged" AutoPostBack="true" />
                                        </div>
                                        <div class="panelItem">
                                            <asp:CheckBox ID="chkAtivoeCommerce" runat="server" Text="Ativa Ecommerce" />
                                        </div>
                                        <div class="panelItem">
                                            <asp:CheckBox ID="chkExcluirProximaIntegracao" runat="server" Text="Excluir Proxima Integração" />
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="panelItem">
                                            <p>ID eCommercer</p>
                                            <asp:TextBox runat="server" ID="txtIdEcommercer" />
                                        </div>
                                        <div class="panelItem">
                                            <p>ID Categoria eCommerce</p>
                                            <asp:TextBox runat="server" ID="txtIDCategoriaeCommerce" />
                                        </div>
                                    </div>


                                    <div class="row">
                                        <div class="panelItem" style="width: 90%">
                                            <h2>Descrição Comercial (até 500 caracteres)</h2>
                                            <asp:TextBox ID="txtDescricaoComercial" TextMode="MultiLine" runat="server" MaxLength="500" CssClass="texto-ecommerce-comercial"></asp:TextBox>
                                            <hr />
                                        </div>
                                    </div>
                                </div>



                            </div>

                            <div id="diviFood" class="row" style="background-color: tomato" runat="server">
                                <p>Conteúdo de Integração Exclusivo do Cardápio iFood</p>
                                <div class="panelItem">
                                    <p>codigoUUID</p>
                                    <asp:TextBox ID="txtCodigoUUID" runat="server" Width="300px" />
                                </div>
                                <div class="panelItem">
                                    <p>Serve</p>
                                    <asp:DropDownList ID="ddlServe" runat="server" Height="20px" Width="200px">
                                        <asp:ListItem Text="NAO SE APLICA" Value="NOT_APPLICABLE" />
                                        <asp:ListItem Text="SERVE 2" Value="SERVES_2" />
                                        <asp:ListItem Text="SERVE 3" Value="SERVES_3" />
                                    </asp:DropDownList>
                                </div>
                                <div class="panelItem">
                                    <p>Restrição Alimentar</p>
                                    <asp:DropDownList ID="ddlRestricaoAlimentar" runat="server" Height="20px" Width="200px">
                                        <asp:ListItem Text="" Value="" />
                                        <asp:ListItem Text="Vegetariano" Value="VEGETARIAN" />
                                        <asp:ListItem Text="Vegano" Value="VEGAN" />
                                        <asp:ListItem Text="Orgânico" Value="ORGANIC" />
                                        <asp:ListItem Text="Sem Glúten" Value="GLUTEN_FREE" />
                                        <asp:ListItem Text="Sem Açúcar" Value="SUGAR_FREE" />
                                        <asp:ListItem Text="Sem Lactose" Value="LAC_FREE" />
                                        <asp:ListItem Text="Natural" Value="NATURAL" />
                                    </asp:DropDownList>
                                </div>
                                <div class="row">
                                    <div class="panelItem">
                                        <p>Inicio</p>
                                        <asp:TextBox ID="txtHrStart" runat="server" CssClass="hora"></asp:TextBox>
                                    </div>
                                    <div class="panelItem">
                                        <p>Término</p>
                                        <asp:TextBox ID="txtHrEnd" runat="server" CssClass="hora"></asp:TextBox>
                                    </div>
                                    <div class="panelItem">
                                        <p>Disponibilidade</p>
                                        <div>
                                            <div>
                                                <asp:CheckBox ID="chkDia_1" Text="Domingo" runat="server" />
                                                <asp:CheckBox ID="chkDia_2" Text="Segunda" runat="server" />
                                                <asp:CheckBox ID="chkDia_3" Text="Terça" runat="server" />
                                                <asp:CheckBox ID="chkDia_4" Text="Quarta" runat="server" />
                                                <asp:CheckBox ID="chkDia_5" Text="Quinta" runat="server" />
                                                <asp:CheckBox ID="chkDia_6" Text="Sexta" runat="server" />
                                                <asp:CheckBox ID="chkDia_7" Text="Sabado" runat="server" />
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="panelItem">
                                    <p>Status</p>
                                    <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="200px">
                                        <asp:ListItem Text="Ativo" Value="1" />
                                        <asp:ListItem Text="Pausado" Value="0" />
                                    </asp:DropDownList>
                                </div>
                                <div class="row">
                                    <div id="divAtualizarApi" runat="server" class="panelItem">
                                        <asp:ImageButton ImageUrl="~/img/arquivo-upload.png" ID="ImgBtnEnviarAPI" runat="server" OnClick="ImgBtnEnviarAPI_Click" />
                                        Atualizar Produto iFood
                                    </div>
                                </div>
                            </div>


                            <div class="row">

                                <h2>Descrição Comercial Completa</h2>
                                <div id="divTextoEcommerce" runat="server" class="texto-ecommerce">
                                </div>
                                <textarea id="txtEcommercer" runat="server"></textarea>

                            </div>
                        </div>
                        <div>
                          
                                <h2 style="text-align: center;">Imagem</h2>
                                <asp:Button ID="btnUpload" runat="server" Text=" Carregar Imagem "
                                    OnClick="btnUpload_Click" CssClass="btnImgEcommercer" />
                                <div style="margin-right: auto; max-height: 700px; width: 280px; overflow: auto;">
                                    <asp:Button ID="btnEnviarFTP" runat="server" Text="Enviar Cardápio" OnClick="btnEnviarFTP_Click" />
                                    <div id="divImagens" runat="server">
                                    </div>
                                </div>
                          </div>

                    </div>
                  
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabEcommerceAPI" runat="server" HeaderText="TabPanel2" Visible="true">
                <HeaderTemplate>
                    E-Commerce API Magento
                </HeaderTemplate>
                <ContentTemplate>
                    <div style="display:flex">
                        <div>
                           <p>Conteúdo de Integração Exclusivo API Magento</p>
  
                            <!-- <div class="row" style="display: flex;">
                                <div style="flex: 2">
                            </div>
                                -->
                            <div class="row" runat="server">
                                <div class="panelItem">
                                    <p>Tipo de Produto</p>
                                    <asp:DropDownList ID="dllTipoProduto" runat="server" Width="150px">
                                        <asp:ListItem Text="RAÇÃO" Value="9" />
                                        <asp:ListItem Text="COMIDA ÚMIDA" Value="10" />
                                        <asp:ListItem Text="PETISCOS" Value="11" />
                                        <asp:ListItem Text="AREIAS" Value="12" />
                                        <asp:ListItem Text="SHAMPOOS" Value="13" />
                                        <asp:ListItem Text="FARMACIA" Value="14" />
                                        <asp:ListItem Text="ACESSÓRIOS" Value="15" />
                                    </asp:DropDownList>
                                </div>
                                <div class="panelItem">
                                    <p>Marca</p>
                                    <asp:DropDownList ID="ddlMarca" runat="server" Width="200px">
                                    </asp:DropDownList>
                                </div>
                                <div class="panelItem">
                                    <p>Unidades por produto</p>
                                    <asp:TextBox ID="txtQtdeUnidadesProduto" runat="server" Width="150px" />
                                </div>
                                <div class="panelItem">
                                    <p>Peso em (g)</p>
                                    <asp:TextBox ID="txtPesoGramas" runat="server"  CssClass="numero" Width="60px" />
                                </div>
                                <div class="panelItem">
                                    <p>Dosagem Recomendada</p>
                                    <asp:TextBox ID="txtDosagemRecomendada" runat="server" Width="200px" />
                                </div>
                                <div class="panelItem">
                                    <p>Outlet</p>
                                    <asp:DropDownList ID="ddlOutlet" runat="server" Width="150px">
                                        <asp:ListItem Text="NÃO" Value="0" />
                                        <asp:ListItem Text="SIM" Value="1" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="div1" class="row"  runat="server">
                                <div class="panelItem" style="border: 1px solid;">
                                    <p>Animal</p>
                                    <asp:CheckBoxList ID="lcAnimal" runat="server" >
                                        <asp:ListItem Text="CAO" Value="CAO" />
                                        <asp:ListItem Text="GATO" Value="GATO" />
                                        <asp:ListItem Text="AVES" Value="AVES" />
                                        <asp:ListItem Text="TODOS" Value="OUTROS" />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="panelItem" style="border: 1px solid;">
                                    <p>Raças</p>
                                    <asp:CheckBoxList ID="lckRaca" runat="server" >
                                        <asp:ListItem Text="TODOS"  />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="panelItem" style="border: 1px solid;">
                                    <p>Porte</p>
                                    <asp:CheckBoxList ID ="lckPorte" runat="server" SelectionMode="Multiple">
                                        <asp:ListItem Text="Gigante" Value="52" />
                                        <asp:ListItem Text="Grande" Value="51" />
                                        <asp:ListItem Text="Médio" Value="50" />
                                        <asp:ListItem Text="Mini" Value="48" />
                                        <asp:ListItem Text="Pequeno" Value="49" />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="panelItem" style="border: 1px solid;">
                                    <p>Idade</p>
                                    <asp:CheckBoxList ID="lckIdade" runat="server" SelectionMode="Multiple">
                                        <asp:ListItem Text="Filhote" Value="18" />
                                        <asp:ListItem Text="Adulto" Value="19" />
                                        <asp:ListItem Text="Sênior" Value="20" />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="panelItem" style="border: 1px solid;">
                                    <p>Sabor</p>
                                    <asp:CheckBoxList ID="lckSabor" runat="server" SelectionMode="Multiple">
                                    </asp:CheckBoxList>
                                </div>
                                <div class="panelItem" style="border: 1px solid;">
                                    <p>Cuidados Especiais</p>
                                    <asp:CheckBoxList ID="lckCuidados" runat="server" SelectionMode="Multiple">
                                    </asp:CheckBoxList>
                                </div>
                                <div class="panelItem" style="border: 1px solid;">
                                    <p>Tipo de Petisco</p>
                                    <asp:CheckBoxList ID="lckTipoPetisco" runat="server" SelectionMode="Multiple">
                                        <asp:ListItem Text="Bifinho" Value="293" />
                                        <asp:ListItem Text="Biscoito" Value="298" />
                                        <asp:ListItem Text="Cookies" Value="292" />
                                        <asp:ListItem Text="Ossos" Value="295" />
                                        <asp:ListItem Text="Palito" Value="296" />
                                        <asp:ListItem Text="Pate" Value="297" />
                                        <asp:ListItem Text="Pedaços" Value="294" />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="panelItem" style="border: 1px solid;">
                                    <p>Odor</p>
                                    <asp:CheckBoxList ID="lckOdor" runat="server" SelectionMode="Multiple">
                                        <asp:ListItem Text="Com Odor" Value="287" />
                                        <asp:ListItem Text="Sem Odor" Value="286" />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="panelItem" style="border: 1px solid;">
                                    <p>Tipo de Areia</p>
                                    <asp:CheckBoxList ID="lckAreia" runat="server" SelectionMode="Multiple">
                                        <asp:ListItem Text="Areia" Value="288" />
                                        <asp:ListItem Text="Biodegradável" Value="291" />
                                        <asp:ListItem Text="Granulado de Madeira" Value="298" />
                                        <asp:ListItem Text="Silica" Value="289" />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="panelItem" style="border: 1px solid;">
                                    <p>Tipo de Higiênico</p>
                                    <asp:CheckBoxList ID="lckTipoHigienico" runat="server" SelectionMode="Multiple">
                                        <asp:ListItem Text="Desinfectante em spray" Value="178" />
                                        <asp:ListItem Text="Espuma" Value="180" />
                                        <asp:ListItem Text="Lenço umedecido" Value="179" />
                                        <asp:ListItem Text="Perfume" Value="176" />
                                        <asp:ListItem Text="Shampoo" Value="177" />
                                        <asp:ListItem Text="Tapete Higiênico" Value="181" />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="panelItem" style="border: 1px solid;">
                                    <p>Tipo de Farmaceutico</p>
                                    <asp:CheckBoxList ID="lckTipoFarmaceutico" runat="server" SelectionMode="Multiple">
                                        <asp:ListItem Text="TODOS" Value="TODOS" />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="panelItem" style="border: 1px solid;">
                                    <p>Tipo de Acessório</p>
                                    <asp:CheckBoxList ID="lckTipoAcessorio" runat="server" SelectionMode="Multiple">
                                        <asp:ListItem Text="TODOS" Value="TODOS" />
                                    </asp:CheckBoxList>
                                </div>
                                <div class="panelItem" style="border: 1px solid;">
                                    <p>Cor</p>
                                    <asp:CheckBoxList ID="lckCor" runat="server" SelectionMode="Multiple">
                                        <asp:ListItem Text="BRANCO" Value="TODOS" />
                                        <asp:ListItem Text="PRETO" Value="TODOS" />
                                        <asp:ListItem Text="AZUL" Value="TODOS" />
                                        <asp:ListItem Text="VERMELHO" Value="TODOS" />
                                        <asp:ListItem Text="AMARELO" Value="TODOS" />
                                        <asp:ListItem Text="TODOS" Value="TODOS" />
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                    </div>
                  
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </div>
    <asp:Panel ID="PnEan" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnEanFrame" runat="server" CssClass="frame">
            <asp:Label ID="lblTituloEan" runat="server" Text="EANS" CssClass="cabMenu"></asp:Label>
            <table class="direita">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnCancelarEan" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                            OnClick="btnCancelarEan_Click" />
                        <asp:Label ID="Label3" runat="server" Text="Fechar"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PnGridEans" runat="server" CssClass="lista" DefaultButton="btnAddEan">
                <table>
                    <tr>
                        <td>
                            <p>
                                <asp:Label ID="lblEanTitulo" runat="server" Text="EAN"></asp:Label>
                            </p>
                            <asp:TextBox ID="TxtAddEan" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnAddEan" runat="server" ImageUrl="~/img/add.png" Width="25px"
                                OnClick="btnAddEan_Click" />
                            <asp:Label ID="lblEanAdd" runat="server" Text="Adicionar"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="GridEan" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    OnRowCommand="GridEan_RowCommand">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" Text="Excluir">
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
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalEan" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnEan" TargetControlID="lblTituloEan">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnTabelaPreco" runat="server" CssClass="modalForm" Style="display: none; height: 250px;">
        <asp:Panel ID="pnTabelaPrecoFrame" runat="server" CssClass="frame" Style="padding: 10px">
            <asp:Label ID="lblTituloTabela" runat="server" Text="Tabela Preço" CssClass="cabMenu"></asp:Label>
            <center>
                <p><asp:Label ID="lblTituloTabelaPreco" runat="server" Text="Para acréscimo, informar um VALOR NEGATIVO no campo DESCONTO"></asp:Label></p>
            </center>
            <div class="row">
                <div class="panelItem" style="margin-left: 30%">
                    <asp:ImageButton ID="btnConfirmaTbPreco" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaTbPreco_Click" />
                    <asp:Label ID="Label8" runat="server" Text="Confirmar"></asp:Label>
                </div>
                <div id="divExcluirTabelaPreco" runat="server">
                    <div class="panelItem" style="margin-left: 15%">
                        <asp:ImageButton ID="ImgBtnExcluirTbPreco" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="ImgBtnExcluirTbPreco_Click" />
                        <asp:Label ID="Label2" runat="server" Text="Excluir"></asp:Label>
                    </div>
                </div>
                <div class="panelItem" style="margin-left: 15%">
                    <asp:ImageButton ID="btnCancelaTbPreco" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaTbPreco_Click" />
                    <asp:Label ID="Label9" runat="server" Text="Fechar"></asp:Label>
                </div>

            </div>
            <hr>
            <div id="divPrecoTabelaMargem" runat="server">
                <div class="row" style="display: flex; justify-content: center;">
                    <div class="panelItem">
                        <p>
                            Preço compra
                        </p>
                        <asp:TextBox ID="txtPrecoTabCompra" runat="server" Width="100px" CssClass="numero" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                    </div>
                    <div class="panelItem">
                        <p>
                            Preço custo
                        </p>
                        <asp:TextBox ID="txtPrecoTabCusto" runat="server" Width="100px" CssClass="numero" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Margem
                        </p>
                        <asp:TextBox ID="txtPrecoTabMargem" runat="server" Width="100px" CssClass="numero" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Preço venda
                        </p>
                        <asp:TextBox ID="txtPrecoTabVenda" runat="server" Width="100px" CssClass="numero"  Enabled="false"
                            Style="font-size: 20px; text-align: right;" ></asp:TextBox>
                    </div>

                </div>
                <div class="row" style="display: flex; justify-content: center;">
                    <div class="panelItem">
                        <p>
                            Codigo Tabela
                        </p>
                        <asp:TextBox ID="txtCodTbMargem" runat="server" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtn_txtCodTbMargem" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </div>
                    <div class="panelItem">
                        <p>
                            Margem
                        </p>
                        <asp:TextBox ID="txtMargTabPreco" runat="server" Width="100px" CssClass="numero" AutoPostBack="True"
                            OnTextChanged="txtMargTabPreco_TextChanged"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Preço Tabela
                        </p>
                        <asp:TextBox ID="txtPrecoVendaTabPreco" runat="server" Width="100px" CssClass="numero" AutoPostBack="True"
                            Style="font-size: 20px; text-align: right;" OnTextChanged="txtPrecoVendaTabPreco_TextChanged"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div id="divPrecoTabelaPadrao" runat="server" class="row" style="display: flex; justify-content: center;">
                <div class="panelItem">
                    <p>
                        Codigo Tabela
                    </p>
                    <asp:TextBox ID="txtCodTbPreco" runat="server" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="imgBtn_txtCodTbPreco" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </div>
                <div class="panelItem">

                    <p>
                        Preço
                    </p>
                    <asp:TextBox ID="txtTbPreco" runat="server" Width="80px" CssClass="numero" ></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Desconto
                    </p>
                    <asp:TextBox ID="txtTbPrecoDesconto" runat="server" CssClass="numero" OnChange="javascript:calculaDescontoPreco()"
                        Width="80px">
                    </asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        Preço Promoção
                    </p>
                    <asp:TextBox ID="txtTbPrecoPromocao" runat="server" CssClass="numero" Width="80px" OnChange="javascript:calculaPrecoDesconto();"></asp:TextBox>
                </div>
                
            </div>
          
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalTbpreco" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnTabelaPreco" TargetControlID="lblTituloTabela">
    </asp:ModalPopupExtender>
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
                    <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    OnRowDataBound="GridLista_RowDataBound" OnSelectedIndexChanged="GridLista_SelectedIndexChanged">
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
        DropShadow="true" PopupControlID="PnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnManterDados" runat="server" CssClass="frameModal" Style="display: none; width: 400px;">
        <h3>
            <center>
            <b><asp:Label ID="Label1" runat="server" Text=" Gostaria de Manter os Dados do ultimo Cadastro? "
                ></asp:Label></b>
                </center>
        </h3>
        <hr />
        <div class="row" style="margin-top: 0px; margin-bottom: 20px;">
            <b>
                <center><asp:CheckBox ID="chkManterPrecos" runat="server" Text="Manter os preços" /></center>
            </b>
        </div>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaManter" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaManter_Click" />
                    <asp:Label ID="Label6" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaManter" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaManter_Click" />
                    <asp:Label ID="Label7" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalManterDados" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnManterDados" TargetControlID="Label1">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnPrecoLoja" runat="server" CssClass="modalForm" Style="display: none; height: 390px;">
        <asp:Panel ID="PnPrecoLojaFrame" runat="server" CssClass="frame">
            <asp:Label ID="lblTituloPrecoLoja" runat="server" Text="Preço Lojas" CssClass="cabMenu"></asp:Label>
            <table>
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaPrecoLoja" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaPrecoLoja_Click" />
                        <asp:Label ID="Label11" runat="server" Text="Confirmar"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaPrecoLoja" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaPrecoLoja_Click" />
                        <asp:Label ID="Label12" runat="server" Text="Fechar"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PnPrecoLojaConteudo" runat="server">
                <table>
                    <tr>
                        <td>
                            <p>
                                Filial
                            </p>
                            <asp:TextBox ID="txtFilial" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Tipo
                            </p>
                            <asp:TextBox ID="txtTipo" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Preço Compra
                            </p>
                            <asp:TextBox ID="txtPrecoCompraLoja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Preço Custo
                            </p>
                            <asp:TextBox ID="txtPrecoCustoLoja" runat="server" Width="90px" AutoPostBack="true"
                                OnTextChanged="txtPrecoCustoLoja_TextChanged">
                            </asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Margem
                            </p>
                            <asp:TextBox ID="txtMargemLoja" runat="server" Width="90px" AutoPostBack="true" OnTextChanged="txtMargemLoja_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Preço
                            </p>
                            <asp:TextBox ID="txtPrecoLoja" runat="server" Width="90px" AutoPostBack="true" OnTextChanged="txtPrecoLoja_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p>
                                Saldo Atual
                            </p>
                            <asp:TextBox ID="txtSaldoAtualLoja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Custo 1
                            </p>
                            <asp:TextBox ID="txtCusto1Loja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Custo 2
                            </p>
                            <asp:TextBox ID="txtCusto2Loja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Est Minimo
                            </p>
                            <asp:TextBox ID="txtEstoqueMinimoLoja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Cobertura
                            </p>
                            <asp:TextBox ID="txtCoberturaLoja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Ultima Entrada
                            </p>
                            <asp:TextBox ID="txtUltimaEntradaLoja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p>
                                Data Inventario
                            </p>
                            <asp:TextBox ID="txtDataInventarioLoja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Marca
                            </p> 
                            <asp:TextBox ID="txtMarcaLoja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <p>
                                Validade
                            </p>
                            <asp:TextBox ID="txtValidadeLoja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td colspan="2" rowspan="3">
                            <p>
                                Ingredientes
                            </p>
                            <asp:TextBox ID="txtIngredientesLoja" runat="server" Width="200px" Height="100px"
                                TextMode="MultiLine">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkPromocaoLoja" Text="Promoção" runat="server" />
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkPromocaoAutomaticaLoja" Text="Promoção Automatica" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p>
                                Preco Promo
                            </p>
                            <asp:TextBox ID="txtPrecoPromocaoLoja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                % Promoção
                            </p>
                            <asp:TextBox ID="txtMargemPromocaoLoja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Data inicio
                            </p>
                            <asp:TextBox ID="txtDataInicioLoja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Data Fim
                            </p>
                            <asp:TextBox ID="txtDataFimLoja" runat="server" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p>
                                Qtde Atacado
                            </p>
                            <asp:TextBox ID="txtQtdeAtacadoLoja" runat="server" Width="90px" CssClass="numero"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Margem Atacado
                            </p>
                            <asp:TextBox ID="txtMargemAtacadoLoja" runat="server" Width="90px" CssClass="numero"
                                OnTextChanged="txtMargemAtacadoLoja_TextChanged" AutoPostBack="true">
                            </asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Preço Atacado
                            </p>
                            <asp:TextBox ID="txtPrecoAtacadoLoja" runat="server" Width="90px" CssClass="numero"
                                AutoPostBack="true">
                            </asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalPrecoLoja" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnPrecoLoja" TargetControlID="lblTituloPrecoLoja">
    </asp:ModalPopupExtender>


    <asp:Panel ID="PnInativar" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Inativar a Mercadoria?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmarInativar" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaInativar_Click" />
                    <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelarInativar" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelarInativar_Click" />
                    <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalInativarMercadoria" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnInativar" TargetControlID="Label14">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnHistorioPreco" runat="server" CssClass="modalForm" Style="display: none; height: 400px; width: 1000px;"><!-- 800px -->
        <asp:Panel ID="Panel3" runat="server" CssClass="frame">
            <div style="border-bottom: solid; border-bottom-width: 1px; height: 40px;">
                <div style="width: 300px; float: left; margin-left: 20px;">
                    <h2>
                        <asp:Label ID="Label17" runat="server" Text="Historico Preço"></asp:Label>
                    </h2>
                </div>
                <div class="Paneldireita" style="float: right;">
                    <br />
                    <asp:ImageButton ID="btnCancelarHistoricoPreco" runat="server" ImageUrl="~/img/cancel.png"
                        Width="20px" OnClick="btnCancelaHistoricoPreco_Click" />
                    Fechar
                </div>
            </div>
            <asp:Panel ID="Panel1" runat="server" CssClass="lista">
                <asp:GridView ID="GridHistoricoPreco" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None">
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
    <asp:ModalPopupExtender ID="modalHistoricoPreco" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnHistorioPreco" TargetControlID="Label17">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnKardex" runat="server" CssClass="modalForm" Style="display: none; height: 400px; width: 800px;"> 
        <asp:Panel ID="Panel10" runat="server" CssClass="frame">
            <div style="border-bottom: solid; border-bottom-width: 1px; height: 40px;">
                <div style="width: 300px; float: left; margin-left: 20px;">
                    <h2>
                        <asp:Label ID="Label25" runat="server" Text="Kardex"></asp:Label>
                    </h2>
                </div>
                <div class="Paneldireita" style="float: right;">
                    <br />
                    <asp:ImageButton ID="btnCancelaKardex" runat="server" ImageUrl="~/img/cancel.png"
                        Width="20px" OnClick="btnCancelaKardex_Click" />
                    Fechar
                </div>
            </div>
            <asp:Panel ID="Panel11" runat="server" CssClass="lista">
                <asp:GridView ID="GridKardex" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None">
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
    <asp:ModalPopupExtender ID="modalKardex" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnKardex" TargetControlID="label25">
    </asp:ModalPopupExtender>





    <asp:Panel ID="pnConfirmaFamilia" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="PnConfirmaFamiliaFrame" runat="server" CssClass="frame">
            <center>
                <h3>
                    <asp:Label ID="Label18" runat="server" Text="Esse produto faz parte de uma Familia ao Alterar o Preço os Itens abaixo Também serão Alterados <br/> Tem Certeza que gostaria de prosseguir com a alteração?  "
                        ForeColor="Red"></asp:Label>
                </h3>
                <hr />
                <table>
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnConfirmaFamilia" runat="server" ImageUrl="~/img/confirm.png"
                                Width="25px" OnClick="btnConfirmaFamilia_Click" />
                            <asp:Label ID="Label19" runat="server" Text="SIM"></asp:Label>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnCancelaFamilia" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="btnCancelarFamilia_Click" />
                            <asp:Label ID="Label20" runat="server" Text="NÃO"></asp:Label>
                        </td>
                    </tr>
                </table>
            </center>
            <asp:Panel ID="Panel4" runat="server" CssClass="lista">
                <asp:GridView ID="GridItensFamilia" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None">
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
    <asp:ModalPopupExtender ID="modalConfirmaFamilia" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaFamilia" TargetControlID="Label18">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnErroTrib" runat="server" CssClass="frameModal" Style="display: none; height: 200px;">
        <h2>
            <b>
                <center>
            <asp:Label ID="lblErroTrib" runat="server" Text=""></asp:Label>
            
        <hr />
          Tem Certeza que gostaria de prosseguir?
              </center>
            </b>
        </h2>
        <br />
        <div class="panelItem" style="margin-left: 20%;">
            <asp:ImageButton ID="btnConfirmaTributacao" runat="server" ImageUrl="~/img/confirm.png"
                Width="25px" OnClick="btnConfirmaTributacao_Click" />
            <asp:Label ID="Label22" runat="server" Text="SIM"></asp:Label>
        </div>
        <div class="panelItem" style="margin-left: 40%;">
            <asp:ImageButton ID="btnCancelarTributacao" runat="server" ImageUrl="~/img/cancel.png"
                Width="25px" OnClick="btnCancelarTributacao_Click" />
            <asp:Label ID="Label23" runat="server" Text="NÃO"></asp:Label>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalErroTrib" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnErroTrib" TargetControlID="lblErroTrib">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnErroEan" runat="server" CssClass="frameModal" Style="display: none; height: 200px;">
        <h2>
            <b>
                <center>
                    O Ean :
            <asp:Label ID="lblErroEan" runat="server" Text=""></asp:Label>
            Não é Valído!
            
        <hr />
          Tem Certeza que gostaria de Incluir?
              </center>
            </b>
        </h2>
        <br />
        <div class="panelItem" style="margin-left: 20%;">
            <asp:ImageButton ID="imgBtnConfirmaErroEan" runat="server" ImageUrl="~/img/confirm.png"
                Width="25px" OnClick="imgBtnConfirmaErroEan_Click" />
            <asp:Label ID="Label21" runat="server" Text="SIM"></asp:Label>
        </div>
        <div class="panelItem" style="margin-left: 40%;">
            <asp:ImageButton ID="imgBtnCancelaErroEan" runat="server" ImageUrl="~/img/cancel.png"
                Width="25px" OnClick="imgBtnCancelaErroEan_Click" />
            <asp:Label ID="Label24" runat="server" Text="NÃO"></asp:Label>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalErroEan" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnErroEan" TargetControlID="lblErroEan">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnDetalhesReferencias" runat="server" CssClass="modalForm" Style="display: none; height: 400px; width: 600px;">
        <asp:Panel ID="Panel8" runat="server" CssClass="frame">
            <div style="border-bottom: solid; border-bottom-width: 1px; height: 40px;">
                <div style="width: 300px; float: left; margin-left: 20px;">
                    <h2>
                        <asp:Label ID="lblRefereciaDetalhe" runat="server" Text="Detalhes Referencia"></asp:Label></h2>
                </div>
                <div class="Paneldireita" style="float: right;">
                    <br />
                    <asp:ImageButton ID="imgBtnFecharReferencia" runat="server" ImageUrl="~/img/cancel.png"
                        Width="20px" OnClick="imgBtnFecharReferencia_Click" />
                    Fechar
                </div>
            </div>
            <asp:Panel ID="Panel9" runat="server" CssClass="lista">
                <asp:GridView ID="gridDetalhesReferencia" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None">
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
    <asp:ModalPopupExtender ID="modalDetalhesReferencia" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnDetalhesReferencias" TargetControlID="lblRefereciaDetalhe">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 50px; margin-bottom: 20px;">
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
    <asp:Panel ID="pnConfirmaExcluirItem" runat="server" CssClass="frameModal" Style="display: none; height: 200px;">
        <h2>
            <b>
                <center>
                    Tem Certeza que gostaria de excluir o item? <br />
              
                <asp:Label ID="lblPluItemExcluir" runat="server" Text="" ForeColor="Red"></asp:Label>
                </center>
            </b>
        </h2>
        <br />
        <div class="panelItem" style="margin-left: 20%;">
            <asp:ImageButton ID="imgBtnConfirmaExcluirItem" runat="server" ImageUrl="~/img/confirm.png"
                Width="25px" OnClick="imgBtnConfirmaExcluirItem_Click" />
            <asp:Label ID="Label10" runat="server" Text="SIM"></asp:Label>
        </div>
        <div class="panelItem" style="margin-left: 40%;">
            <asp:ImageButton ID="imgBrnCancelaExcluirItem" runat="server" ImageUrl="~/img/cancel.png"
                Width="25px" OnClick="imgBrnCancelaExcluirItem_Click" />
            <asp:Label ID="Label13" runat="server" Text="NÃO"></asp:Label>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaExcluirItem" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaExcluirItem" TargetControlID="lblPluItemExcluir">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnExcluirImagem" runat="server" CssClass="frameModal" Style="display: none; height: 400px; padding:40px">
        <h2>
            <b>
                <center>
                    Tem Certeza que gostaria de excluir a Imagem? <br />
                 
                 <asp:Label ID="lblImagemExcluir" runat="server" Text="" ForeColor="Red" visible="false"></asp:Label>
                </center>
            </b>
        </h2>
      
        <br />
        <div class="panelItem" style="margin-left: 20%;">
            <asp:ImageButton ID="ImgBtnConfirmaExluirImagem" runat="server" ImageUrl="~/img/confirm.png"
                Width="25px" OnClick="ImgBtnConfirmaExluirImagem_Click" />
            <asp:Label ID="Label26" runat="server" Text="SIM"></asp:Label>
        </div>
        <div class="panelItem" style="margin-left: 40%;">
            <asp:ImageButton ID="imgBtnCancelaExcluirImagem" runat="server" ImageUrl="~/img/cancel.png"
                Width="25px" OnClick="imgBtnCancelaExcluirImagem_Click" />
            <asp:Label ID="Label27" runat="server" Text="NÃO"></asp:Label>
        </div>
        <asp:Image ID="imgConfirmaExcluir" runat="server" CssClass="ImgEcommerce" />
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaExcluirImagem" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnExcluirImagem" TargetControlID="Label26">
    </asp:ModalPopupExtender>


</asp:Content>
