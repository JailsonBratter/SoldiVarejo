<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ClienteDetalhes.aspx.cs" Inherits="visualSysWeb.Cadastro.ClienteDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../css/ClienteDetalhes.css" rel="stylesheet" />
    <script src="../js/ClienteDetalhes1.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>Cliente</h1>
    </center>
    <hr />
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
                            Codigo Cliente
                        </p>
                        <asp:TextBox ID="txtCodigo_Cliente" runat="server" MaxLength="11" CssClass="numero" Width="100px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnProximaSequencia" runat="server" Visible="false" Height="15px" ImageUrl="~/img/pesquisaM.jpg" OnClick="ImgBtnProximaSequnecia_Click" />
                    </div>
                    <div class="panelItem">

                        <p>
                            Codigo tabela
                        </p>
                        <asp:TextBox ID="txtCodigo_tabela" runat="server" Width="114px" OnTextChanged="txtCodigo_tabela_TextChanged" AutoPostBack="true"></asp:TextBox>
                        <asp:ImageButton ID="imgBtn_txtCodigo_tabela" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg" AutoPostBack="true"
                            OnClick="lista_click" />
                    </div>
                    
                    <div class="panelItem">

                        <p>
                            Grupo Empresa
                        </p>
                        <asp:TextBox ID="txtGrupoEmpresa" runat="server" Width="50px" AutoPostBack="true" OnTextChanged="txtGrupoEmpresa_TextChanged"></asp:TextBox>
                        <asp:TextBox ID="txtNomeGrupoEmpresa" runat="server" Width="114px"></asp:TextBox>
                        <asp:ImageButton ID="imgBtn_txtGrupoEmpresa" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                            OnClick="lista_click" />
                    </div>
                    <div class="panelItem">
                        <p>
                            Vendedor
                        </p>
                        <asp:TextBox ID="txtvendedor" runat="server" Width="109px"></asp:TextBox>
                        <asp:ImageButton ID="imgBtn_txtvendedor" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                            OnClick="lista_click" />
                    </div>
                    <div class="panelItem">
                        <p>
                            Nome Cliente
                        </p>
                        <asp:TextBox ID="txtNome_Cliente" runat="server" Width="300px" MaxLength="49" onchange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Situacao
                        </p>
                        <asp:TextBox ID="txtSituacao" runat="server" Width="77px"></asp:TextBox>
                        <asp:ImageButton ID="imgBtn_txtSituacao" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                            OnClick="lista_click" />
                    </div>

                </td>
            </tr>
            <tr>
                <td>

                    <div class="panelItem">
                        <br />
                        <asp:CheckBox ID="chkPessoa_Juridica" runat="server" Text="Pessoa_Juridica" OnCheckedChanged="chkPessoa_Juridica_CheckedChanged"
                            AutoPostBack="true" />
                    </div>
                    <div class="panelItem">
                        <p>
                            <asp:Label ID="lblCpf" runat="server" Text="CPF"></asp:Label>
                        </p>
                        <asp:TextBox ID="txtCNPJ" runat="server" AutoPostBack="true" OnTextChanged="txtCNPJ_TextChanged"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            <asp:Label ID="lblRg" runat="server" Text="RG"></asp:Label>
                        </p>
                        <asp:TextBox ID="txtIE" runat="server"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Nome Fantasia
                        </p>
                        <asp:TextBox ID="txtnome_fantasia" runat="server" Width="300px" MaxLength="49" onchange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Data cadastro
                        </p>
                        <asp:TextBox ID="txtdata_cadastro" runat="server" Enabled="false" Width="92px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Usuario
                        </p>
                        <asp:TextBox ID="txtUsuario" runat="server" Width="106px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Usuario Ult Alteracao
                        </p>
                        <asp:TextBox ID="txtUsuarioAlteracao" runat="server" Width="106px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="panelItem" >
                        
                            <asp:CheckBox ID="chkTerceiroPreco" runat="server"  Text="Terceiro Preço" OnCheckedChanged="chkTerceiroPreco_CheckedChanged" AutoPostBack="true" />
                            
                    </div>
                    
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="conteudo" runat="server" CssClass="conteudo">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabCadastro">
                <HeaderTemplate>
                    Cadastro
                </HeaderTemplate>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <p>
                                    Endereco
                                </p>
                                <asp:TextBox ID="txtEndereco" runat="server" Width="316px" MaxLength="59" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Numero
                                </p>
                                <asp:TextBox ID="txtendereco_nro" runat="server" Width="50px" MaxLength="7"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    CEP
                                </p>
                                <asp:TextBox ID="txtCEP" runat="server" Width="123px" class="numero" OnTextChanged="txtCEP_TextChanged"
                                    MaxLength="9" AutoPostBack="True"></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_txtCEP" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                                    OnClick="lista_click" />
                            </td>
                            <td>
                                <p>
                                    Complemento
                                </p>
                                <asp:TextBox ID="txtcomplemento_end" runat="server" MaxLength="29" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Bairro
                                </p>
                                <asp:TextBox ID="txtBairro" runat="server" MaxLength="49" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    Cidade
                                </p>
                                <asp:TextBox ID="txtCidade" runat="server" Width="277px" MaxLength="49" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_txtCidade" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                                    OnClick="lista_click" />
                            </td>
                            <td>
                                <p>
                                    UF
                                </p>
                                <asp:TextBox ID="txtUF" runat="server" Width="50px" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Data Nascimento
                                </p>
                                <asp:TextBox ID="txtData_Nascimento" runat="server" Width="80px" MaxLength="10"></asp:TextBox>
                                <asp:ImageButton ID="ImgDtDeCalendario" ImageUrl="~/img/calendar.png" runat="server"
                                    Height="15px" />
                                <asp:CalendarExtender ID="clnData" runat="server" PopupButtonID="imgDtDeCalendario"
                                    TargetControlID="txtData_Nascimento" Enabled="True">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <p>
                                    Estado Civil
                                </p>
                                <asp:TextBox ID="txtEstado_civil" runat="server" MaxLength="29" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <p>
                                    Historico
                                </p>
                                <asp:TextBox ID="txtHistorico" runat="server" Width="791px" Height="60px" TextMode="MultiLine"
                                    CssClass="SEM"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    Nome Conjuge
                                </p>
                                <asp:TextBox ID="txtNome_conjuge" runat="server" Width="317px" MaxLength="49" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <p>
                                    Naturalidade
                                </p>
                                <asp:TextBox ID="txtNaturalidade" runat="server" Width="175px" MaxLength="19" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Renda Mensal
                                </p>
                                <asp:TextBox ID="txtRenda_Mensal" runat="server" CssClass="numero" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>

                            <td colspan="5">
                                <div class="panelItem">
                                    <p>Indicador de IE *(Consumidor Final)</p>
                                    <asp:DropDownList ID="ddlTipoIE" runat="server">
                                        <asp:ListItem Value="1">Contribuinte ICMS</asp:ListItem>
                                        <asp:ListItem Value="2">Contribuinte isento de Inscrição no cadastro de Contribuintes</asp:ListItem>
                                        <asp:ListItem Value="9">Consumidor Final, que pode ou não possuir Inscrição</asp:ListItem>

                                    </asp:DropDownList>
                                </div>
                                <div class="panelItem">
                                    <p>
                                        Contato
                                    </p>
                                    <asp:TextBox ID="txtContato" runat="server" Width="317px" MaxLength="49" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                                </div>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div class="panelItem" style="width: 40%;">


                                    <div class="panelItem">
                                        <asp:CheckBox ID="ChkIvaDescricao" Text="Apresentar ultima ST na Descrição da NFE"
                                            runat="server" />
                                    </div>
                                    <br />
                                    <br />
                                    <div class="panelItem">
                                        <asp:CheckBox ID="chkICM_Isento" runat="server" Text="Icm Isento" />

                                    </div>

                                </div>
                                <div class="panelItem" style="width: 40%;">

                                    <div class="panelItem">
                                        <asp:CheckBox ID="chkOpt_Simples_nac" Text="Optante pelo Simples Nacional"
                                            runat="server" />
                                    </div>
                                    <br />
                                    <br />
                                    <div class="panelItem">
                                        <asp:CheckBox ID="chkContaAssinada" Text="Conta Assinada"
                                            runat="server" />

                                    </div>
                                </div>

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
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabAdicionais">
                <HeaderTemplate>
                    Adicionais
                </HeaderTemplate>
                <ContentTemplate>
                    <center>
                        <h1>
                            Endereço de cobrança</h1>
                    </center>
                    <hr />
                    <table>
                        <tr>
                            <td>
                                <p>
                                    Endereco
                                </p>
                                <asp:TextBox ID="txtEndereco_ent" runat="server" Width="191px" MaxLength="49" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Numero
                                </p>
                                <asp:TextBox ID="txtendereco_ent_nro" runat="server" Width="55px" MaxLength="7"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Complemento
                                </p>
                                <asp:TextBox ID="txtComplemento_ent" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                            </td>

                            <td>
                                <p>
                                    Cep
                                </p>
                                <asp:TextBox ID="txtCep_ent" runat="server" Width="108px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Bairro
                                </p>
                                <asp:TextBox ID="txtBairro_ent" runat="server" MaxLength="29" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Cidade
                                </p>
                                <asp:TextBox ID="txtCidade_ent" runat="server" Width="180px" MaxLength="29" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Uf
                                </p>
                                <asp:TextBox ID="txtUf_ent" runat="server" Width="48px" MaxLength="2" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <h1>
                            Meios de Comunicação</h1>
                    </center>
                    <hr />
                    <table>
                        <tr>
                            <td>
                                <p>
                                    Meio Comunicação
                                </p>
                                <asp:TextBox ID="txtMeioComunicacao" runat="server" Width="180px" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                                <asp:ImageButton ID="imgBtn_txtMeioComunicacao" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                                    OnClick="lista_click" />
                            </td>
                            <td>
                                <p>
                                    Id Meio Comunicação
                                </p>
                                <asp:TextBox ID="txtIdMeioComunicacao" runat="server" Width="180px" CssClass="sem"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Contato
                                </p>
                                <asp:TextBox ID="txtContatoComunicacao" runat="server" Width="180px" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td>
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
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabBancarias">
                <HeaderTemplate>
                    Bancarias
                </HeaderTemplate>
                <ContentTemplate>
                    <center>
                        <h1>
                            Informações Bancarias</h1>
                    </center>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <p>
                                                Numero Banco
                                            </p>
                                            <asp:TextBox ID="txtNumeroBanco" runat="server" Width="180px" MaxLength="3"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtn_txtNumeroBanco" runat="server" Height="15px" ImageUrl="~/img/pesquisaM.jpg"
                                                OnClick="lista_click" />
                                            <td>
                                                <p>
                                                    Nome banco
                                                </p>
                                                <asp:TextBox ID="txtNomeBanco" runat="server" Width="180px" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                                            </td>
                                        </td>
                                        <td>
                                            <p>
                                                Agencia
                                            </p>
                                            <asp:TextBox ID="txtAgencia" runat="server" Width="180px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                Conta
                                            </p>
                                            <asp:TextBox ID="txtConta" runat="server" Width="180px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>
                                                Telefone
                                            </p>
                                            <asp:TextBox ID="txtTelefone" runat="server" Width="180px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                Contato
                                            </p>
                                            <asp:TextBox ID="txtContatoBanco" runat="server" Width="180px" OnChange="javascript:this.value = this.value.toUpperCase();"
                                                MaxLength="30"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgAddBanco" runat="server" Height="31px" ImageUrl="~/img/add.png"
                                                Width="31px" OnClick="imgAddBanco_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gridbanco" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                                    OnRowCommand="gridbanco_RowCommand" OnRowDeleting="gridbanco_RowDeleting">
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
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr />
                                <table>
                                    <tr>
                                        <td>
                                            <p>
                                                Lugar
                                            </p>
                                            <asp:TextBox ID="txtLugarClienteEntrega" runat="server" Width="180px" MaxLength="19"
                                                OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                Endereço
                                            </p>
                                            <asp:TextBox ID="txtEnderecoClienteEntrega" runat="server" Width="180px" MaxLength="49"
                                                OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                UF
                                            </p>
                                            <asp:TextBox ID="txtUfClienteEntrega" runat="server" Width="50px" MaxLength="2"
                                                OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>
                                                CEP
                                            </p>
                                            <asp:TextBox ID="txtCepClienteEntrega" runat="server" Width="180px" MaxLength="9"></asp:TextBox>
                                        </td>
                                        <td>
                                            <p>
                                                Cidade
                                            </p>
                                            <asp:TextBox ID="txtCidadeClienteEntrega" runat="server" Width="180px" MaxLength="59"
                                                OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ImgAddClienteEntrega" runat="server" Height="31px" ImageUrl="~/img/add.png"
                                                Width="31px" OnClick="imgAddClienteEntrega_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="gridLugarEntrega" runat="server" CellPadding="4" ForeColor="#333333"
                                    GridLines="None" OnRowCommand="gridLugarEntrega_RowCommand" OnRowDeleting="gridLugarEntrega_RowDeleting">
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
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabEntregas" runat="server" HeaderText="TabEntregas">
                <HeaderTemplate>
                    Entregas
                </HeaderTemplate>
                <ContentTemplate>
                    <center>
                        <h1>
                            Endereços de Entrega</h1>
                    </center>
                    <hr />
                    <table>
                        <tr>
                            <td>
                                <p>
                                    Endereço
                                </p>
                                <asp:TextBox ID="txtEnderecoEntrega" runat="server" Width="180px" MaxLength="49"
                                    OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Numero
                                </p>
                                <asp:TextBox ID="txtNumeroEntrega" runat="server" Width="180px" MaxLength="8" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Bairro
                                </p>
                                <asp:TextBox ID="txtBairroEntrega" runat="server" Width="180px" MaxLength="30" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    Cidade
                                </p>
                                <asp:TextBox ID="txtCidadeEntrega" runat="server" Width="180px" MaxLength="30" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    UF
                                </p>
                                <asp:TextBox ID="txtUFEntrega" runat="server" Width="50px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    CEP
                                </p>
                                <asp:TextBox ID="txtCepEntrega" runat="server" Width="180px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="ImageButton1" runat="server" Height="31px" ImageUrl="~/img/add.png"
                                    Width="31px" OnClick="imgAddEnderecosEntrega_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gridEnderecosEntrega" runat="server" CellPadding="4" ForeColor="#333333"
                        GridLines="None" OnRowCommand="gridEnderecosEntrega_RowCommand" OnRowDeleting="gridEnderecosEntrega_RowDeleting">
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" CommandName="Delete"
                                Text="Excluir" Visible="true">
                                <ControlStyle Width="18px" />
                            </asp:ButtonField>
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
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPagamentos" runat="server" HeaderText="TabPanel5">
                <HeaderTemplate>
                    Pagamentos
                </HeaderTemplate>
                <ContentTemplate>

                    <div class="panel" style="border: 1px solid; width: 100%; height: 1000px;">
                        <div class="row">
                            <div class="panelItem" style="font-size: 15px">
                                <p>
                                    Ult Compra
                                </p>
                                <asp:TextBox ID="txtDataUltimaCompra" runat="server" Width="120px" Style="font-size: 20px"></asp:TextBox>
                            </div>
                            <div class="panelItem" style="font-size: 15px">
                                <p>
                                    Maior Compra
                                </p>
                                <asp:TextBox ID="txtMaiorCompra" runat="server" Width="120px" CssClass="numero" Style="font-size: 20px"></asp:TextBox>
                            </div>
                            <div class="panelItem" style="font-size: 15px; margin-left: 30px;">
                                <p>
                                    Concluido
                                </p>
                                <asp:TextBox ID="txtConcluido" runat="server" Width="120px" CssClass="numero" Style="font-size: 20px"></asp:TextBox>
                            </div>
                            <div class="panelItem" style="font-size: 15px;">
                                <p>
                                    A vencer
                                </p>
                                <asp:TextBox ID="txtAberto" runat="server" Width="120px" CssClass="numero" Style="font-size: 20px"></asp:TextBox>
                            </div>
                            <div class="panelItem" style="font-size: 15px;">
                                <p>
                                    Vencidos
                                </p>
                                <asp:TextBox ID="txtAtrasados" runat="server" Width="120px" CssClass="numero" Style="font-size: 20px"></asp:TextBox>
                            </div>
                            <div class="panelItem" style="font-size: 15px">
                                <p>
                                    Limite Credito
                                </p>
                                <asp:TextBox ID="txtLimite_Credito" runat="server" CssClass="numero" Width="120px" Style="font-size: 20px"></asp:TextBox>
                            </div>

                            <div class="panelItem" style="font-size: 15px;">
                                <div class="panelItem">
                                    <p>
                                        Conta a Receber
                                    </p>
                                    <asp:TextBox ID="txtUtilizadoReceber" runat="server" CssClass="numero" Width="120px" Style="font-size: 20px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="panelItem" style="font-size: 15px;">
                                <div class="panelItem">
                                    <p>
                                        Caderneta
                                    </p>
                                    <asp:TextBox ID="txtUtilizado" runat="server" CssClass="numero" Width="120px" Style="font-size: 20px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="panelItem" style="font-size: 15px;">
                                <div class="panelItem">
                                    <p>
                                        Utilizado
                                    </p>
                                    <asp:TextBox ID="txtTotalUtlizado" runat="server" CssClass="numero" Width="120px" Style="font-size: 20px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="panelItem" style="font-size: 15px;">
                                <p>
                                    Status
                                </p>
                                <asp:DropDownList ID="ddlStatus" runat="server" Style="font-size: 20px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                    <asp:ListItem>PREVISTO</asp:ListItem>
                                    <asp:ListItem>CONCLUIDO</asp:ListItem>
                                    <asp:ListItem>A VENCER</asp:ListItem>
                                    <asp:ListItem>VENCIDOS</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div style="border-style: solid; border-width: 1px; float: right; background: #FFFACD; margin-left: 30px; margin-top: 30px; margin-bottom: 20px;">
                                Titulos dos ultimos
                                    <asp:TextBox ID="txtDiasPagamentos" runat="server" Text="12" Width="40px" CssClass="numero" AutoPostBack="true" OnTextChanged="txtDiasPagamentos_TextChanged" />
                                meses
                            </div>
                        </div>
                        <div class="panelItem" style="margin-bottom: 0px;">
                            <h1>Contas a Receber</h1>
                        </div>

                        <div class="row" style="height: 250px; overflow: auto;">
                            <asp:GridView ID="gridPagamenos" runat="server" CellPadding="4" ForeColor="#333333"
                                AutoGenerateColumns="False" GridLines="None" Width="100%"
                                OnDataBound="gridPagamenos_DataBound">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:HyperLinkField DataTextField="Documento" Text="---" HeaderText="DOCUMENTO" DataNavigateUrlFormatString="~/modulos/Financeiro/pages/ContasReceberDetalhes.aspx?campoIndex={0}&codCliente={1}&emissao={2}&tela=FN002"
                                        DataNavigateUrlFields="Documento,codigo_cliente,Emissao" SortExpression="Documento"
                                        Target="_blank">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:HyperLinkField>
                                    <asp:BoundField DataField="EMISSAO" HeaderText="EMISSAO">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VENCIMENTO" HeaderText="VENCIMENTO">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PAGAMENTO" HeaderText="PAGAMENTO">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VALOR" HeaderText="VALOR" DataFormatString="R$ {0:n}">
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="STATUS" HeaderText="STATUS">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ID_CC" HeaderText="BANCO">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
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
                        <div class="panelItem" style="margin-bottom: 0px;">
                            <h1>Caderneta</h1>
                        </div>

                        <div class="row" style="height: 250px; overflow: auto;">
                            <asp:GridView ID="gridCaderneta" runat="server" CellPadding="4" ForeColor="#333333"
                                AutoGenerateColumns="False" GridLines="None" Width="100%">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="Documento_Caderneta" HeaderText="Documento"></asp:BoundField>
                                    <asp:BoundField DataField="Emissao_caderneta" HeaderText="Emissao"></asp:BoundField>
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                                    <asp:BoundField DataField="Historico_Caderneta" HeaderText="Historico"></asp:BoundField>
                                    <asp:BoundField DataField="Total_Caderneta" HeaderText="Total" DataFormatString="R$ {0:n}">
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Caixa_Caderneta" HeaderText="Caixa"></asp:BoundField>
                                    <asp:BoundField DataField="lancamento" HeaderText="Lancamento"></asp:BoundField>
                                    <asp:BoundField DataField="usuario" HeaderText="Usuario"></asp:BoundField>
                                    <asp:BoundField DataField="data_inclusao" HeaderText="Incluido"></asp:BoundField>
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
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabGerarSenhaAcesso" runat="server" HeaderText="TabPanel5" Visible="false">
                <HeaderTemplate>
                    Gerar Senha Acesso
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="panelItem" style="width:80%;  ">


                        <div class="panelItem" style="margin-left: 20px">
                            <asp:CheckBox ID="chkHabilitaF9" runat="server" Text="Habilitar F9" />
                        </div>
                        <table>
                            <tr>
                                <td colspan="2">
                                    <p>
                                        Chave Atual
                                    </p>
                                    <asp:TextBox ID="txtChaveAtual" runat="server" Width="300px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <p>
                                        Vencimento
                                    </p>
                                    <asp:TextBox ID="txtDataVencimentoChave" runat="server" Width="80px" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgDtVencimentoChave" ImageUrl="~/img/calendar.png" runat="server"
                                        Height="15px" />
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtVencimentoChave"
                                        TargetControlID="txtDataVencimentoChave" Enabled="True">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnGerarNovaSenha" runat="server" Text="Gerar Nova Senha" Height="50px"
                                        Width="200px" Font-Size="Large" OnClick="btnGerarNovaSenha_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="panelItem">
                        <asp:GridView ID="gridModulos" runat="server" CellPadding="4" ForeColor="#333333"
                            AutoGenerateColumns="False" GridLines="None" Width="100%">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="nome" HeaderText="Modulos"></asp:BoundField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkTodos" onclick="javascript:chkTodos();" runat="server" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked='<%# Eval("Acesso") %>'  />
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
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabPet">
                <HeaderTemplate>
                    Pets
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="row">
                        <asp:Panel ID="addItens" runat="server" CssClass="titulobtn">
                            <asp:ImageButton ID="ImgBtnAddPet" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                Width="20px" OnClick="ImgBtnAddPet_Click" />
                            Incluir Pet
                        </asp:Panel>
                    </div>
                    <asp:GridView ID="gridPet" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                        OnRowCommand="gridPet_RowCommand" AutoGenerateColumns="false">
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/img/edit.png" Text="Editar" Visible="true">
                                <ControlStyle Width="18px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="Codigo_pet" HeaderText="Codigo Pet" />
                            <asp:BoundField DataField="Nome_pet" HeaderText="Nome" />
                            <asp:BoundField DataField="Sexo" HeaderText="Sexo" />
                            <asp:BoundField DataField="Cor" HeaderText="Cor" />
                            <asp:BoundField DataField="Data_Nascimento" HeaderText="Nascimento" />
                            <asp:BoundField DataField="Pelagem" HeaderText="Pelagem" />
                            <asp:BoundField DataField="Porte" HeaderText="Porte" />
                            <asp:BoundField DataField="Raca" HeaderText="Raça" />
                            <asp:BoundField DataField="Ultimo_cio" HeaderText="Ultimo cio" />
                            <asp:BoundField DataField="Especie" HeaderText="Especie" />
                            <asp:BoundField DataField="Pedigree" HeaderText="Pedigree" />
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
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="TabFidelidade">
                <HeaderTemplate>
                    Fidelidade
                </HeaderTemplate>
                <ContentTemplate>

                    <div class="panel" style="border: 1px solid; width: 100%; height: 1000px;">
                        <div class="row">
                            <div class="panelItem" style="float: right; font-size: 15px">
                                <p>
                                    Total Pontos
                                </p>
                                <asp:TextBox ID="txtTotalPontos" runat="server" Width="120px" CssClass="numero" Style="font-size: 20px"></asp:TextBox>
                            </div>

                        </div>
                        <div class="row" style="margin-bottom: 0px; text-align: center;">
                            <h1 style="text-align: center;">Historico de Compras</h1>
                        </div>
                        <div id="divNaoRegistrado" runat="server" class="row" visible="false">
                            <h2>Sem Registros</h2>
                        </div>
                        <div class="row" style="height: 500px; overflow: auto;">
                            <asp:GridView ID="gridFidelidade" runat="server" CellPadding="4" ForeColor="#333333"
                                AutoGenerateColumns="False" GridLines="None" Width="80%">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="Data_Venda" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Caixa_saida" HeaderText="Caixa Saida">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Documento" HeaderText="Documento">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PLU" HeaderText="PLU">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descricao" HeaderText="Descrição">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Qtde_Pontos" HeaderText="Pontos" DataFormatString="{0:n}">
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
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

                    </div>
                </ContentTemplate>
            </asp:TabPanel>


        </asp:TabContainer>
    </asp:Panel>
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
                    OnRowDataBound="GridLista_RowDataBound" >
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
            <div id="divAddTabela" runat="server">
                <asp:ImageButton ID="imgBtnAddTabela" runat="server" ImageUrl="~/img/add.png" Width="25px"
                    OnClick="imgBtnAddTabela_Click" />
                <asp:Label ID="lblAddTabela" runat="server" Text="  Adicionar nova Tabela"></asp:Label>
            </div>
            <div class="panelItem" id="divAddGrupoEmpresa" runat="server" visible="false">
                <asp:ImageButton ID="imgBtnAddGrupoEmpresa" runat="server" ImageUrl="~/img/add.png"
                    Width="25px" OnClick="imgBtnAddGrupoEmpresa_Click" />
                Adicionar Novo Grupo
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalFundo" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
    <%-- <asp:Panel ID="pnIncluirNovaTabela" runat="server" CssClass="frameModal" Style="display: none">
        <asp:Label ID="Label7" runat="server" Text="Informe os Dados da nova Tabela" CssClass="cabMenu"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="imgbtnConfirmaAddNovaTabela" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgbtnConfirmaAddNovaTabela_Click" />
                    <asp:Label ID="Label10" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgbtnCancelaAddNovaTabela" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgbtnCancelaAddNovaTabela_Click" />
                    <asp:Label ID="Label11" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
        <div class="panel">
            <div class="panelItem">
                <p>
                    Codigo Tabela</p>
                <asp:TextBox ID="txtCodigoNovaTab" runat="server" Width="80px" MaxLength="6"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    Porc</p>
                <asp:TextBox ID="txtPorcNovaTab" runat="server" Width="80px"></asp:TextBox>
            </div>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirma" TargetControlID="Label2">
    </asp:ModalPopupExtender>--%>
    <asp:Panel ID="pnConfirma" runat="server" CssClass="frameModal" Style="display: none">
        <asp:Label ID="Label1" runat="server" Text="Confirma Inativação do Cliente?" CssClass="cabMenu"></asp:Label>
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
        DropShadow="true" PopupControlID="pnConfirma" TargetControlID="Label2">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnPets" runat="server" CssClass="modalForm" Style="display: none; height: 500px; width: 89%; margin-left: 5%;">
        <asp:Panel ID="PnPetFrame" runat="server" CssClass="frame" Width="98%" Height="95%">
            <div class="cabMenu">
                <h1>Dados do Pet</h1>
                <asp:Label ID="lblErroPet" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaPet" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaPet_Click" />
                        <asp:Label ID="Label8" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaPet" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                            OnClick="btnCancelaPet_Click" />
                        <asp:Label ID="Label9" runat="server" Text="Cancela"></asp:Label>
                    </td>
                    <%-- <td>
                        <asp:ImageButton ID="btnExcluiPet" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                            OnClick="btnExcluiPet_Click" />
                        <asp:Label ID="Label6" runat="server" Text="Excluir Pet"></asp:Label>
                    </td>--%>
                </tr>
            </table>
            <div class="panelItem" style="width: 76%; margin-left: 5px;">
                <div class="row">
                    <div class="panelItem">
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoPet" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Nome
                        </p>
                        <asp:TextBox ID="txtNomePet" runat="server" Width="300px" MaxLength="20" CssClass="campoObrigatorio"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Sexo
                        </p>
                        <asp:DropDownList ID="ddlSexo" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>FEMEA</asp:ListItem>
                            <asp:ListItem>MACHO</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>
                            Cor
                        </p>
                        <asp:DropDownList ID="ddlCor" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>
                            Data Nascimento
                        </p>
                        <asp:TextBox ID="txtDtNascimentoPet" runat="server" Width="100px" CssClass="DATA" MaxLength="10"></asp:TextBox>
                        <asp:Image ID="imgDtNascimentoPet" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgDtNascimentoPet"
                            TargetControlID="txtDtNascimentoPet">
                        </asp:CalendarExtender>
                    </div>
                </div>
                <div class="row">
                    <div class="panelItem">
                        <p>
                            Pelagem
                        </p>
                        <asp:DropDownList ID="ddlPelagem" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>
                            Porte
                        </p>
                        <asp:DropDownList ID="ddlPorte" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>
                            Raca
                        </p>
                        <asp:DropDownList ID="ddlRaca" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>
                            Especie
                        </p>
                        <asp:DropDownList ID="ddlEspecie" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>
                            Ultimo Cio
                        </p>
                        <asp:TextBox ID="txtUltimoCio" runat="server" Width="100px" CssClass="DATA"></asp:TextBox>
                        <asp:Image ID="imgUltimoCio" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" PopupButtonID="imgUltimoCio"
                            TargetControlID="txtUltimoCio">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p>
                            Pedigree
                        </p>
                        <asp:TextBox ID="txtPedigree" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                    </div>
                </div>
                <div class="panelItem" style="width: 30%;">


                    <div class="row" style="width: 100%">
                        <center><h3>Vacinas</h3></center>
                        <hr />
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <p>
                                Vacina
                            </p>
                            <asp:DropDownList ID="ddlVacina" runat="server" Width="80px">
                            </asp:DropDownList>
                        </div>
                        <div class="panelItem">
                            <p>
                                Data Vacina
                            </p>
                            <asp:TextBox ID="txtDtVacina" runat="server" Width="80px" CssClass="DATA"></asp:TextBox>
                            <asp:Image ID="imgDtVacina" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgDtVacina"
                                TargetControlID="txtDtVacina">
                            </asp:CalendarExtender>
                        </div>
                        <div class="panelItem">
                            <br />
                            <asp:ImageButton ID="imgBtnAddVacina" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                Width="20px" OnClick="imgBtnAddVacina_Click" />
                            Vacina
                        </div>
                    </div>
                    <div class="row" style="height: 150px; width: 100%; overflow: auto; border: 1px solid; margin-top: 0px;">
                        <asp:GridView ID="gridVacinas" runat="server" CellPadding="4" ForeColor="#333333"
                            GridLines="None" OnRowCommand="gridVacinas_RowCommand" Width="100%" AutoGenerateColumns="false">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" Text="Editar" Visible="true">
                                    <ControlStyle Width="18px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="Vacina" HeaderText="Vacina" />
                                <asp:BoundField DataField="Data_Ultima_Vacina" HeaderText="Data Vacina" />
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
                </div>
                <div class="panelItem" style="width: 60%; margin-left: 10px;">

                    <div class="row" style="width: 90%">
                        <center><h3>Prontuario</h3></center>
                        <hr />
                    </div>
                    <div class="row">

                        <div class="panelItem">
                            <br />
                            <asp:ImageButton ID="ImgBtnAddProntuario" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                Width="20px" OnClick="ImgBtnAddProntuario_Click" />
                            Prontuario
                        </div>
                        <%--<div class="panelItem">
                             <br />
                            <asp:ImageButton ID="ImgBtnExcluirProntuario" runat="server" ImageUrl="~/img/cancel.png" Height="20px"
                                Width="20px" OnClick="ImgBtnExcluirProntuario_Click" />
                            Excluir
                        </div>--%>
                    </div>
                    <div class="row" style="overflow: auto;">
                        <div class="panelItem" style="overflow: auto; height: 170px; width: 40%; border: 1px solid; margin-top: 0px;">
                            <asp:GridView ID="gridProntuarios" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" OnRowCommand="gridProntuarios_RowCommand" Width="100%" OnRowDataBound="gridProntuarios_RowDataBound" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton ID="RdoProntuarioSelecionado" runat="server" AutoPostBack="true" OnCheckedChanged="RdoProntuarioSelecionado_CheckedChanged" GroupName="GrProntuario" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                    <asp:BoundField DataField="Data" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data" />

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
                        <div class="panelItem" style="width: 50%; margin-left: 10px;">
                            <p>
                                Observação <span class="btnImprimirDireita">Hora:
                                <asp:TextBox ID="txtHoraObservacaoVeterinario" Width="80px" type="time" AutoPostBack="true" OnTextChanged="txtHoraObservacaoVeterinario_TextChanged" runat="server" />
                                </span>
                            </p>
                            <asp:TextBox ID="TxtObservacaoVeterinario" AutoPostBack="true"
                                OnTextChanged="TxtObservacaoVeterinario_TextChanged" TextMode="MultiLine" CssClass="sem"
                                Height="140px" Width="100%" runat="server" />

                        </div>


                    </div>
                </div>
            </div>
            <div class="panelItem" style="width: 22%; height: 400px;">
                <div class="row" style="width: 100%">
                    <center><h3>Imagens </h3></center>
                    <hr />
                </div>
                <div class="panelItem">
                    <br />
                    <asp:ImageButton ID="ImgBtnNovaImagem" runat="server" ImageUrl="~/img/add.png" Height="20px"
                        Width="20px" OnClick="ImgBtnNovaImagem_Click" />
                    Adicionar Nova Imagem
                </div>

                <div style="border: solid 1px; width: 100%; height: 90%;">
                    <asp:GridView ID="gridImagens" runat="server" CellPadding="4" ForeColor="#333333"
                        GridLines="None" OnRowCommand="gridImagens_RowCommand" Width="95%" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Imagem" HeaderText="Imagem" />
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/img/Img.png" Text="" Visible="true">
                                <ControlStyle Width="18px" />
                            </asp:ButtonField>

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

            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalPet" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblErroPet" DropShadow="true" PopupControlID="PnPets" TargetControlID="lblErroPet">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnAddGrupoEmpresa" runat="server" CssClass="modalForm" Style="display: none; height: 200px; width: 300px;">
        <div class="frame" style="width: 95%; height: 90%;">
            <asp:Label ID="Label7" runat="server" Text="Novo Grupo" CssClass="cabMenu" Font-Size="Larger"></asp:Label>

            <div class="row">
                <div class="panelItem" style="margin-left: 10%;">

                    <asp:ImageButton ID="imgBtnConfirmaIncluirGrupo" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaIncluirGrupo_Click" />
                    <asp:Label ID="Label10" runat="server" Text="Confirma"></asp:Label>

                </div>
                <div class="panelItem" style="margin-left: 20%;">
                    <asp:ImageButton ID="imgBtnCancelarIncluirGrupo" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelarIncluirGrupo_Click" />
                    <asp:Label ID="Label11" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="panelItem" style="margin-left: 10%;">
                    <p>Nome Grupo</p>
                    <asp:TextBox ID="txtNomeNovoGrupoEmpresa" runat="server" Width="200px" MaxLength="40"></asp:TextBox>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalIncluirNovoGrupoEmpresa" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnAddGrupoEmpresa" TargetControlID="Label7">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnExcluirProntuario" runat="server" CssClass="frameModal" Style="display: none">
        <asp:Label ID="Label12" runat="server" Text="Confirma Excluir Prontuario?" CssClass="cabMenu"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaExcluirProntuario" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaExcluirProntuario_Click" />
                    <asp:Label ID="Label13" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelaExcluirProntuario" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaExcluirProntuario_Click" />
                    <asp:Label ID="Label14" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluirProntuario" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnExcluirProntuario" TargetControlID="Label12">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnExcluirPet" runat="server" CssClass="frameModal" Style="display: none">
        <asp:Label ID="Label15" runat="server" Text="Confirma Excluir PEt?" CssClass="cabMenu"></asp:Label>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnExcluirPet" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnExcluirPet_Click" />
                    <asp:Label ID="Label16" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelaExcluirPet" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaExcluirPet_Click" />
                    <asp:Label ID="Label17" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluirPEt" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnExcluirPet" TargetControlID="Label15">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnIncluirImagem" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu">
            <asp:Label ID="Label6" runat="server" Text="Escolha a imagem?"></asp:Label>
            <asp:Button ID="btnFecharAddImagem" runat="server" CssClass="btn btn-default btnImprimirDireita"
                Text="X-Fechar" OnClick="btnFecharAddImagem_Click" Style="height: 100%;" />
        </div>
        <table style="width: 90%">
            <tr>
                <td>Nome:
                    <asp:TextBox ID="txtNomeImagem" Width="80%" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="panelItem" style="width: 60%;">

                                <label for='MainContent_FileArquivo' class="btn btn-default" style="padding: 10px;">
                                    <img src="../../../img/Img.png" width="35px" style="display: inline-block;" />
                                    <div style="position: relative; left: 10px; top: -10px; display: inline-block; margin-top: -40px; margin-right: 10px;">Selecionar um arquivo </div>

                                </label>
                                <br>
                                <span id='file-name'></span>
                                <asp:FileUpload ID="FileArquivo" runat="server" onchange="javascript:changeFile(this);"
                                    AllowMultiple="false"
                                    accept=".png,.jpg,.jpeg,.gif"
                                    Style="display: none;" />
                            </div>
                            <div class="panelItem" style="width: 20%;">
                                <label for='MainContent_btnEnviarArquivo' class="btn btn-default" style="padding: 10px;">
                                    <img src="../../../img/arquivo-download.png" width="35px" style="display: inline-block;" />
                                    <div style="position: relative; left: 10px; top: -10px; display: inline-block; margin-top: 0px; margin-right: 10px;">
                                        Importar Arquivo
                                    </div>

                                </label>
                                <asp:Button ID="btnEnviarArquivo" runat="server" CssClass="btn btn-default" Text="" Style="display: none;"
                                    OnClick="btnEnviarArquivo_Click" />
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnEnviarArquivo" runat="server" />
                        </Triggers>
                    </asp:UpdatePanel>

                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalIncluirImagem" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnIncluirImagem" TargetControlID="Label6">
    </asp:ModalPopupExtender>
</asp:Content>
