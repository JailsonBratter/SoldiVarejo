<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FornecedorDetalhes.aspx.cs" Inherits="visualSysWeb.Cadastro.FornecedorDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes do Fornecedor</h1>
        </center>
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
                            Fornecedor
                        </p>
                        <asp:TextBox ID="txtFornecedor" runat="server" Width="200px" MaxLength="19"></asp:TextBox>
                    </div>
                    <div class="panelItem">

                        <p>Tipo</p>
                        <asp:DropDownList ID="ddlTipoFornecedor" runat="server">
                            <asp:ListItem Text="ADMINISTRATIVO">
                            </asp:ListItem>
                            <asp:ListItem Text="COMERCIAL">
                            </asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </td>
                <td>
                    <p>
                        Usuario
                    </p>
                    <asp:TextBox ID="txtUsuario" runat="server" Width="106px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Usuario Ult Alteracao
                    </p>
                    <asp:TextBox ID="txtUsuarioAlteracao" runat="server" Width="106px"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <asp:CheckBox ID="chkFormulario_proprio" runat="server" Text="Sem Formulario Próprio" />
                    </div>
                    <div class="panelItem">
                        <asp:CheckBox ID="chkpessoa_fisica" runat="server" Text="Pessoa Fisica" OnCheckedChanged="chkpessoa_fisica_CheckedChanged"
                            AutoPostBack="true" />
                    </div>
                </td>

            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabCadastro">
                <HeaderTemplate>
                    Cadastro
                </HeaderTemplate>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <p>
                                    Razao_social
                                </p>
                                <asp:TextBox ID="txtRazao_social" runat="server" Width="300px" MaxLength="49"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <p>
                                    Nome Fantasia
                                </p>
                                <asp:TextBox ID="txtNome_Fantasia" runat="server" Width="300px" MaxLength="29"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    <asp:Label ID="lblCnpj_Cpf" runat="server" Text="CNPJ" CssClass="SEM"></asp:Label>
                                </p>
                                <asp:TextBox ID="txtCNPJ" runat="server" MaxLength="18" CssClass="CNPJ"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    IE
                                </p>
                                <asp:TextBox ID="txtIE" runat="server" Width="106px" OnTextChanged="txtIE_TextChanged"></asp:TextBox>
                            </td>


                        </tr>

                        <tr>
                            <td>
                                <p>
                                    Endereco
                                </p>
                                <asp:TextBox ID="txtEndereco" runat="server" Width="300px" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Numero
                                </p>
                                <asp:TextBox ID="txtEndereco_nro" runat="server" Width="73px" MaxLength="8"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Bairro
                                </p>
                                <asp:TextBox ID="txtBairro" runat="server" Width="175px" MaxLength="30"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    CEP
                                </p>
                                <asp:TextBox ID="txtCEP" runat="server" Width="100px" MaxLength="9" OnTextChanged="txtCEP_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_txtCEP" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                                    OnClick="lista_click" />
                            </td>
                            <td>
                                <p>
                                    UF
                                </p>
                                <asp:TextBox ID="txtUF" runat="server" Width="36px" MaxLength="2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <p>
                                    Cidade
                                </p>
                                <asp:TextBox ID="txtCidade" runat="server" Width="297px" AutoPostBack="True" OnTextChanged="txtCidade_TextChanged"
                                    MaxLength="60"></asp:TextBox>
                                <asp:ImageButton ID="imgBtnCidade" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                                    OnClick="lista_click" />
                            </td>
                            <td>
                                <p>
                                    Codigo Municipio
                                </p>
                                <asp:TextBox ID="txtCodMunicipio" runat="server" Width="90px" MaxLength="7"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Senha
                                </p>
                                <asp:TextBox ID="txtsenha" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Centro Custo
                                </p>
                                <asp:TextBox ID="txtCentroCusto" runat="server" Width="150px" MaxLength="30"></asp:TextBox>
                                <asp:ImageButton ID="imgBtnCentroCusto" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                                    OnClick="lista_click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <p>
                                    Indicador de IE
                                </p>
                                <asp:DropDownList ID="ddlTipoIE" runat="server">
                                    <asp:ListItem Value="1">Contribuinte ICMS</asp:ListItem>
                                    <asp:ListItem Value="2">Contribuinte isento de Inscrição no cadastro de Contribuintes</asp:ListItem>
                                    <asp:ListItem Value="9">Não Contribuinte, que pode ou não possuir Inscrição</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="chkProdutor_rural" runat="server" Text="Produtor Rural" />
                                <asp:CheckBox ID="chkBaseIpi" runat="server" Text="IPI na Base ICMS" />01
                                <asp:CheckBox ID="chkDespesasBase" runat="server" Text="Despesas na Base ICMS" />
                                <asp:CheckBox ID="chkInativo" runat="server" Text="Inativo" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <p>
                                    Regime Tributário
                                </p>
                                <asp:DropDownList ID="ddlCRT" runat="server">
                                    <asp:ListItem Value="0"></asp:ListItem>
                                    <asp:ListItem Value="1">Simples Nacional</asp:ListItem>
                                    <asp:ListItem Value="2">Simples Nacional, excesso sublimite</asp:ListItem>
                                    <asp:ListItem Value="3">Regime Normal (Real/Presumido)</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <p>Conta Contábil</p>
                                <div class="panelItem" style="display: flex; align-content: space-between; width: 90%; border: solid 1px; padding: 10px; border-radius: 4px">

                                    <div class="panelItem" style="flex: 1">
                                        <p>Crédito </p>
                                        <asp:TextBox ID="txtContaContabilCredito" Width="200px" runat="server" />
                                    </div>
                                    <div class="panelItem" style="flex: 1">
                                        <p>Débito</p>
                                        <asp:TextBox ID="txtContaContabilDebito" Width="200px" runat="server" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabContato">
                <HeaderTemplate>
                    Contato
                </HeaderTemplate>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <div class="panelItem">
                                    <p>
                                        E-mail
                                    </p>
                                    <asp:TextBox ID="txtEmail" runat="server" Width="200px" AutoPostBack="True" OnTextChanged="txtEmail_TextChanged"
                                        MaxLength="49" CssClass="SEM"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Site
                                    </p>
                                    <asp:TextBox ID="txtSite" runat="server" Width="300px" MaxLength="99" CssClass="SEM"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <div class="panelItem">
                                    <p>
                                        Contato
                                    </p>
                                    <asp:TextBox ID="txtContato1" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Cargo
                                    </p>
                                    <asp:TextBox ID="txtCargo1" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Telefone
                                    </p>
                                    <asp:TextBox ID="txtTelefone1" runat="server" Width="100px" MaxLength="15" CssClass="TELEFONE"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Telefone 2
                                    </p>
                                    <asp:TextBox ID="txtTelefone1_2" runat="server" Width="100px" MaxLength="15" CssClass="TELEFONE"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Telefone 3
                                    </p>
                                    <asp:TextBox ID="txtTelefone1_3" runat="server" Width="100px" MaxLength="15" CssClass="TELEFONE"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Email
                                    </p>
                                    <asp:TextBox ID="txtEmail1" runat="server" Width="200px" AutoPostBack="True" OnTextChanged="txtEmail_TextChanged"
                                        MaxLength="49" CssClass="SEM"></asp:TextBox>
                                </div>
                        </tr>
                        <tr>
                            <td>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtContato2" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtCargo2" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtTelefone2" runat="server" Width="100px" MaxLength="15" CssClass="TELEFONE"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtTelefone2_2" runat="server" Width="100px" MaxLength="15" CssClass="TELEFONE"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtTelefone2_3" runat="server" Width="100px" MaxLength="15" CssClass="TELEFONE"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtEmail2" runat="server" Width="200px" AutoPostBack="True" OnTextChanged="txtEmail_TextChanged"
                                        MaxLength="49" CssClass="SEM"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtContato3" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtCargo3" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtTelefone3" runat="server" Width="100px" MaxLength="15" CssClass="TELEFONE"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtTelefone3_2" runat="server" Width="100px" MaxLength="15" CssClass="TELEFONE"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtTelefone3_3" runat="server" Width="100px" MaxLength="15" CssClass="TELEFONE"></asp:TextBox>
                                </div>
                                <div class="panelItem">
                                    <asp:TextBox ID="txtEmail3" runat="server" Width="200px" AutoPostBack="True" OnTextChanged="txtEmail_TextChanged"
                                        MaxLength="49" CssClass="SEM"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <h3>Outros Contatos</h3>
                    <table>
                        <tr>
                            <td>
                                <p>
                                    Meio Comunicação
                                </p>
                                <asp:TextBox ID="txtMeioComunicacao" runat="server" Width="180px"></asp:TextBox>
                                <asp:ImageButton ID="imgMeioComunicacao" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                                    OnClick="lista_click" />
                            </td>
                            <td>
                                <p>
                                    Id Meio Comunicação
                                </p>
                                <asp:TextBox ID="txtIdMeioComunicacao" runat="server" Width="180px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Contato
                                </p>
                                <asp:TextBox ID="txtContatoComunicacao" runat="server" Width="180px"></asp:TextBox>
                            </td>
                            <td>
                                <br />
                                <asp:ImageButton ID="btnAddComunicacao" runat="server" Height="31px" ImageUrl="~/img/add.png"
                                    Width="31px" OnClick="btnAddComunicacao_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gridMeioComunicacao" runat="server" CellPadding="4" ForeColor="#333333"
                        GridLines="None" OnRowCommand="gridMeioComunicacao_RowCommand" OnRowDeleting="gridMeioComunicacao_RowDeleting">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" CommandName="Delete"
                                Text="Excluir" Visible="true">
                                <ControlStyle Width="18px" />
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
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabMercadoria">
                <HeaderTemplate>
                    Mercadoria
                </HeaderTemplate>
                <ContentTemplate>

                    <asp:GridView ID="gridMercadorias" runat="server" CellPadding="4" ForeColor="#333333"
                        OnRowDataBound="GridMercadoria_RowDataBound"
                        OnRowCommand="gridMercadorias_RowCommand"
                        AutoGenerateColumns="false"
                        GridLines="Vertical">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/img/edit.png" CommandName="Editar">
                                <ControlStyle Height="20px" Width="20px" />
                                <ItemStyle Width="20px" />
                            </asp:ButtonField>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" CommandName="Excluir">
                                <ControlStyle Height="20px" Width="20px" />
                                <ItemStyle Width="20px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="plu" HeaderText="PLU" />
                            <asp:BoundField DataField="ean" HeaderText="EAN" />
                            <asp:BoundField DataField="codigo_referencia" HeaderText="Referencia" />
                            <asp:BoundField DataField="descricao" HeaderText="Descrição" />
                            <asp:BoundField DataField="Descricao_NF" HeaderText="Descrição NF" />
                            <asp:BoundField DataField="Embalagem" HeaderText="Embalagem" />
                            <asp:BoundField DataField="preco_compra" HeaderText="Preço Compra" DataFormatString="{0:n2}" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="preco_custo" HeaderText="Preço Custo" DataFormatString="{0:n2}" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="importado_nf" />
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
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabAdicionais">
                <HeaderTemplate>
                    Adicionais
                </HeaderTemplate>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <p>
                                    Desc Esp
                                </p>
                                <asp:TextBox ID="txtDesc_exp" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Ultima Cont do Estoque
                                </p>
                                <asp:TextBox ID="txtUltima_Contagem_do_Estoque" runat="server" Width="161px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Desc Coml
                                </p>
                                <asp:TextBox ID="txtDesc_Coml" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Adc Mkt
                                </p>
                                <asp:TextBox ID="txtAdc_Mkt" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Desc Finan
                                </p>
                                <asp:TextBox ID="txtDesc_Finan" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Adc Finan
                                </p>
                                <asp:TextBox ID="txtAdc_Finan" runat="server" Width="101px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    Bonificacao
                                </p>
                                <asp:TextBox ID="txtBonificacao" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Adc_Perda
                                </p>
                                <asp:TextBox ID="txtAdc_Perda" runat="server" Width="161px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Prazo
                                </p>
                                <asp:TextBox ID="txtPrazo" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Adc_Frete
                                </p>
                                <asp:TextBox ID="txtAdc_Frete" runat="server"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <p>
                                    condicao_pagamento
                                </p>
                                <asp:TextBox ID="txtcondicao_pagamento" runat="server" Width="250px" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabObservacoes">
                <HeaderTemplate>
                    Observações
                </HeaderTemplate>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <p>
                                    obs
                                </p>
                                <asp:TextBox ID="txtobs" runat="server" Height="100px" Width="564px" TextMode="MultiLine"
                                    CssClass="sem"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabDepartamentos">
                <HeaderTemplate>
                    Departamentos
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="panelItem">
                        <asp:ImageButton ID="imgAddDepartamento" runat="server" ImageUrl="~/img/add.png"
                            Width="25px" OnClick="imgAddDepartamento_Click" />
                        Incluir Novo Departamento
                    </div>
                    <div class="gridTable" style="height: 200px; overflow: auto;">
                        <asp:GridView ID="gridFornecedorDepartamento" runat="server" AutoGenerateColumns="False"
                            CellPadding="4" ForeColor="#333333" GridLines="Vertical" OnRowCommand="gridFornecedorDepartamento_RowCommand">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" CommandName="Excluir"
                                    Text="Excluir">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="codigo_departamento" HeaderText="Codigo" />
                                <asp:BoundField DataField="Grupo" HeaderText="Grupo" />
                                <asp:BoundField DataField="SubGrupo" HeaderText="SubGrupo" />
                                <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
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
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabOcorrencias">
                <HeaderTemplate>
                    Ocorrências
                </HeaderTemplate>
                <ContentTemplate>
                    <h3>Ocorrências</h3>
                    <table>
                        <tr>
                            <td>
                                <p>
                                    Ocorrência (até 500 caracteres)
                                </p>
                                <asp:TextBox ID="txtOcorrencia" TextMode="MultiLine" runat="server" MaxLength="500" width="300px" Height="62px"></asp:TextBox>
                            </td>
                            <td>
                                <p>Status</p>
                                <asp:DropDownList ID="ddStatus" runat="server">
                                    <asp:ListItem Text="PENDENTE" />
                                    <asp:ListItem Text="FECHADO" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <br />
                                <asp:ImageButton ID="btnAddOcorrencia" runat="server" Height="31px" ImageUrl="~/img/add.png"
                                    Width="31px" OnClick="btnAddOcorrencia_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gridOcorrencia" runat="server" CellPadding="4" ForeColor="#333333"
                        GridLines="None" OnRowCommand="gridOcorrencia_RowCommand" AutoGenerateColumns="False" OnRowDataBound="gridOcorrencia_RowDataBound">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/edit.png" CommandName="Alterar"
                                    Text="Alterar">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="Data" HeaderText="Data" />
                                <asp:BoundField DataField="Ocorrencia" HeaderText="Ocorrencia" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
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
                </ContentTemplate>
            </asp:TabPanel>

        </asp:TabContainer>

    </div>

    <asp:Panel ID="PnExcluirFornecedor" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Excluir o Fornecedor?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmarExclusao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmarExclusao_Click" />
                    <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaExclusao" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelarExclusao_Click" />
                    <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluirFornecedor" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnExcluirFornecedor" TargetControlID="Label14">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirmaSalvar" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label3" runat="server" Text="O CNPJ/CPF ou  a Inscrição Estadual não é Valida gostaria de Salvar mesmo Assim?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfimaSalvar" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmarSalvar_Click" />
                    <asp:Label ID="Label4" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelarSalvar" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaSalvar_Click" />
                    <asp:Label ID="Label5" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaSalvar" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnConfirmaSalvar" TargetControlID="Label3">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirmaDepartamento" runat="server" CssClass="frameModal" Style="display: none">
        <span class="cabMenu">Confirma Excluir o Departamento:
            <asp:Label ID="lblCodFornecExcluir" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblDepartamento" runat="server" Text=""></asp:Label>
            ? </span>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExclusaoDepartamento_Click" />
                    <asp:Label ID="Label12" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                        OnClick="btnCancelaExclusaoDepartamento_Click" />
                    <asp:Label ID="Label13" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalPnConfirmaDepartamento" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaDepartamento" TargetControlID="lblDepartamento">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnConfirmaExcluirProduto" runat="server" CssClass="frameModal" Style="display: none; width: 400px">
        <h3>
            <asp:Label ID="Label6" runat="server" Text="Tem Certeza que gostaria de Excluir o Produto?"
                CssClass="cabMenu" Style="height: 100px;"></asp:Label>

        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="ImgBtnConfirmaExcluirProduto" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="ImgBtnConfirmaExcluirProduto_Click" />
                    <asp:Label ID="Label7" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="ImgBtnCancelaExcluirProduto" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="ImgBtnCancelaExcluirProduto_Click" Style="margin-left: 20%" />
                    <asp:Label ID="Label8" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluirProduto" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaExcluirProduto" TargetControlID="Label6">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnEditarProduto" runat="server" CssClass="frameModal" Style="display: none; width: 650px;">
        <h3>
            <asp:Label ID="Label9" runat="server" Text="Detalhes do Produto"
                CssClass="cabMenu"></asp:Label>
            <asp:Label ID="lblErroDetalhesProduto" runat="server" Text="" ForeColor="Red"></asp:Label>

        </h3>


        <table style="width: 90%; margin: auto;">
            <tr>
                <td>
                    <div class="panelItem" style="margin-left: 20%;">
                        <asp:ImageButton ID="imgBtnSalvarEditarProduto" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="imgBtnSalvarEditarProduto_Click" />
                        <asp:Label ID="Label10" runat="server" Text="Salvar"></asp:Label>
                    </div>
                    <div class="panelItem" style="margin-left: 30%;">

                        <asp:ImageButton ID="imgBtnCancelaEditarProduto" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="imgBtnCancelaEditarProduto_Click" />
                        <asp:Label ID="Label11" runat="server" Text="Cancelar"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>Plu</p>
                        <asp:TextBox ID="txtPLU_produto" runat="server" Width="80px" />
                        <asp:ImageButton ID="imgTxtPLu_produto" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                            OnClick="lista_click" />

                    </div>
                    <div class="panelItem">
                        <p>EAN</p>
                        <asp:TextBox ID="txtEAN_produto" runat="server" Width="150px" />
                    </div>
                    <div class="panelItem">
                        <p>Referencia</p>
                        <asp:TextBox ID="txtReferencia_produto" runat="server" Width="150px" />
                    </div>
                    <div class="panelItem">
                        <p>Embalagem</p>
                        <asp:TextBox ID="txtEmbalagem_produto" runat="server" Width="80px" CssClass="numero" />
                    </div>
                    <div class="panelItem">
                        <p>Descrição</p>
                        <asp:TextBox ID="txtDescricao_produto" runat="server" Width="500px" />
                    </div>
                    <div class="panelItem">
                        <p>Descricao NF</p>
                        <asp:TextBox ID="txtDescricao_NF_produto" runat="server" Width="500px" MaxLength="40" />
                    </div>

                    <div class="panelItem">
                        <p>Preço Compra</p>
                        <asp:TextBox ID="txtPrecoCompra_produto" runat="server" Width="80px" CssClass="numero" />
                    </div>
                    <div class="panelItem">
                        <p>Preço Custo</p>
                        <asp:TextBox ID="txtPrecoCusto_produto" runat="server" Width="80px" CssClass="numero" />
                    </div>

                </td>
            </tr>


        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalEditarProduto" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnEditarProduto" TargetControlID="Label9">
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
        DropShadow="true" PopupControlID="pnfundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>

</asp:Content>
