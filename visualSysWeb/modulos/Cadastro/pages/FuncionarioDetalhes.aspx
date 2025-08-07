<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FuncionarioDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.FuncionarioDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center> <h1>Detalhes do funcionario</h1></center>
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
                        Codigo
                    </p>
                    <asp:TextBox ID="txtcodigo" runat="server" Width="70px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        User
                    </p>
                    <asp:TextBox ID="txtNome" runat="server" Width="150px" MaxLength="20" CssClass="Sem"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Funcao
                    </p>
                    <asp:DropDownList ID="ddlFuncao" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <p>
                        Senha
                    </p>
                    <asp:TextBox TextMode="Password" ID="txtsenha" runat="server" Width="100px"></asp:TextBox>
                </td>

            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" >
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                <HeaderTemplate>
                    Cadastro
                </HeaderTemplate>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td colspan="2">
                                <p>
                                    Nome
                                </p>
                                <asp:TextBox ID="txtNome2" runat="server" Width="300px"></asp:TextBox>
                            </td>
                            <td colspan="3">
                                <p>
                                    Sobrenome
                                </p>
                                <asp:TextBox ID="txtsobrenome" runat="server" Width="400px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    Inicio
                                </p>
                                <asp:TextBox ID="txtInicio" runat="server" Width="80px" CssClass="hora"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Fim
                                </p>
                                <asp:TextBox ID="txtFim" runat="server" Width="80px" CssClass="hora"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Comissao
                                </p>
                                <asp:TextBox ID="txtComissao" runat="server" Width="100px" CssClass="numero"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <p>
                                    Nascimento
                                </p>
                                <asp:TextBox ID="txtdata_nascimento" runat="server" Width="100px" CssClass="data"></asp:TextBox>
                                <asp:ImageButton ID="ImgDtNacimento" ImageUrl="~/img/calendar.png" runat="server"
                                    Height="15px" />
                                <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="ImgDtNacimento"
                                    TargetControlID="txtdata_nascimento">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <p>
                                    Endereco
                                </p>
                                <asp:TextBox ID="txtEndereco" runat="server" Width="300px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Bairro
                                </p>
                                <asp:TextBox ID="txtbairro" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Cidade
                                </p>
                                <asp:TextBox ID="txtcidade" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Estado
                                </p>
                                <asp:TextBox ID="txtestado" runat="server" Width="70px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    CEP
                                </p>
                                <asp:TextBox ID="txtcep" runat="server" Width="100px" CssClass="CEP"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Telefone
                                </p>
                                <asp:TextBox ID="txttelefone" runat="server" Width="100px" CssClass="TELEFONE"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Celular
                                </p>
                                <asp:TextBox ID="txtcelular" runat="server" Width="100px" CssClass="TELEFONE"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Praca
                                </p>
                                <asp:TextBox ID="txtpraca" runat="server" Width="80px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkusa_palm" runat="server" Text="Utiliza Palm" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkcancela_item" runat="server" Text="Cancela Item" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkUtiliza_agenda" runat="server" Text="Utiliza Agenda" />

                            </td>
                            <td>
                                <asp:CheckBox ID="chkUsa_Terminal" runat="server" Text="Utiliza Terminal" />

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    Banco
                                </p>
                                <asp:TextBox ID="txtbanco" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Conta
                                </p>
                                <asp:TextBox ID="txtCONTA" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Agencia
                                </p>
                                <asp:TextBox ID="txtAgencia" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td colspan="4">
                                <p>
                                    Nome Correntista
                                </p>
                                <asp:TextBox ID="txtNome_correntista" runat="server" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="2">
                                <p>
                                    sp_mercadoria_plu
                                </p>
                                <asp:TextBox ID="txtsp_mercadoria_plu" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <p>
                                    sp_mercadoria_dpto
                                </p>
                                <asp:TextBox ID="txtsp_mercadoria_dpto" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <p>
                                    sp_mercadoria_descricao
                                </p>
                                <asp:TextBox ID="txtsp_mercadoria_descricao" runat="server" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <p>
                                    sp_grupo
                                </p>
                                <asp:TextBox ID="txtsp_grupo" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <p>
                                    sp_sgrupo
                                </p>
                                <asp:TextBox ID="txtsp_sgrupo" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <p>
                                    sp_departamento
                                </p>
                                <asp:TextBox ID="txtsp_departamento" runat="server" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel2">
                <HeaderTemplate>
                    Metas
                </HeaderTemplate>
                <ContentTemplate>
                    <h2 style="text-align:center;">Metas</h2>
                    <hr />
                    <div class="panelItem" style="margin-left:15px;">
                        <asp:ImageButton ID="imgAddDepartamento" runat="server" ImageUrl="~/img/add.png"
                            Width="25px" OnClick="imgAddDepartamento_Click" />
                        Incluir Novo Departamento
                    </div>
                    <div class="gridTable">
                        <asp:GridView ID="gridDepartamentos" runat="server" ForeColor="#333333" GridLines="Vertical"
                            AutoGenerateColumns="False" OnRowCommand="gridDepartamentos_RowCommand" CssClass="table">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/Cancel.png" CommandName="Alterar"
                                    Text="Excluir">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="codigo_departamento" HeaderText="Departamento"></asp:BoundField>
                                <asp:BoundField DataField="Descricao_departamento" HeaderText="Descrição"></asp:BoundField>
                                <asp:TemplateField HeaderText="Meta">
                                    <ItemTemplate>
                                        <center><asp:TextBox ID="txtMeta" runat="server" Text='<%# Eval("Meta") %>' Width="80px"
                                            CssClass="numero"  OnKeyPress="javascript:return autoTab(this,event);"  ></asp:TextBox></center>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
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

        </asp:TabContainer>

    </div>
    <asp:Panel ID="PnConfirmaExcluir" runat="server" CssClass="frameModal" Style="display: none">
        <h3 style="text-align:center;">
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria Excluir o Funcionario?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmarExcluir" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExclusao_Click" />
                    <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelarExcluir" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaExclusao_Click" />
                    <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluir" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnConfirmaExcluir" TargetControlID="Label14">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnExcluirMeta" runat="server" CssClass="frameModal" Style="display: none;padding:5px;">
        <h3 CssClass="cabMenu" style="text-align:center;">
          Tem Certeza que gostaria Excluir o Departamento <br />
            <asp:Label ID="lblDepartamentoExcluir" Text="text" runat="server" />
            -<asp:Label ID="lblDescricaoDepartamento" Text="text" runat="server" />?"
               
            

        </h3>
        <hr />
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnExcluirDepartamento" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnExcluirDepartamento_Click" />
                    <asp:Label ID="Label2" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelaExcluirDepartamento" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaExcluirDepartamento_Click" />
                    <asp:Label ID="Label3" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalExcluirDepartamento" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnExcluirMeta" TargetControlID="lblDepartamentoExcluir">
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
                        Width="25px" OnClick="btnConfirmaLista_Click" />
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
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalPnFundo" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
</asp:Content>
