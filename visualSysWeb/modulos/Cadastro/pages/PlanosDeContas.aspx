<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PlanosDeContas.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.PlanosDeContas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenuPlanoDeContas">
        <center>
            <h1>
                Planos de Contas</h1>
        </center>
    </div>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="Div1" runat="server" class="FramePlanoDeContas">
        <asp:Panel ID="PnGrupo" runat="server" CssClass="DivPlano" DefaultButton="ImgBtnGrupoPesquisar">
            <center>
                <h1>
                    Grupos</h1>
            </center>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoGrupo" runat="server" Width="50px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricaoGrupo" runat="server" Width="150px"> </asp:TextBox>
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="ImgBtnGrupoPesquisar" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Width="25px" OnClick="ImgBtnGrupoPesquisar_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="ImgBtnGrupoEditar" runat="server" ImageUrl="~/img/edit.png"
                            Width="25px" OnClick="ImgBtnGrupoEditar_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgBtnGrupoExcluir" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="imgBtnGrupoExcluir_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="ImgBtnGrupoAdd" runat="server" ImageUrl="~/img/add.png" Width="25px"
                            OnClick="ImgBtnGrupoAdd_Click" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnGridGrupos" runat="server" CssClass="GridContas">
                <asp:GridView ID="GridGrupos" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" GridLines="Vertical" OnRowDataBound="GridGrupos_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="RdoGrupoItem" runat="server" GroupName="GrGrupo" OnCheckedChanged="RdoGrupoItem_CheckedChanged"
                                    AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="codigo_grupo" HeaderText="Codigo" />
                        <asp:BoundField DataField="descricao_grupo" HeaderText="Grupo" />
                        <asp:BoundField DataField="modalidade" HeaderText="Modalidade" NullDisplayText="" />
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
        <asp:Panel ID="PnSubGrupo" runat="server" CssClass="DivPlano">
            <center>
                <h1>
                    SubGrupos </h1>
            </center>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoSubGrupo" runat="server" Width="50px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricaoSubGrupo" runat="server" Width="150px"> </asp:TextBox>
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgBtnSubGrupoPesquisa" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Width="25px" OnClick="imgBtnSubGrupoPesquisa_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgBtnSubGrupoEditar" runat="server" ImageUrl="~/img/edit.png"
                            Width="25px" OnClick="imgBtnSubGrupoEditar_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgBtnSubGrupoExcluir" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="imgBtnSubGrupoExcluir_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgBtnSubGrupoAdd" runat="server" ImageUrl="~/img/add.png" Width="25px"
                            OnClick="imgBtnSubGrupoAdd_Click" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PnGridSubGrupos" runat="server" CssClass="GridContas">
                <asp:GridView ID="GridSubGrupos" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" GridLines="Vertical" OnRowDataBound="GridSubGrupos_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="RdoSubGrupoItem" runat="server" GroupName="GrSubGrupo" OnCheckedChanged="RdoSubGrupoItem_CheckedChanged"
                                    AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="codigo_SubGrupo" HeaderText="Codigo" />
                        <asp:BoundField DataField="descricao_Subgrupo" HeaderText="SubGrupo" />
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
        <asp:Panel ID="PnCentroCusto" runat="server" CssClass="DivPlano">
            <center>
                <h1>
                    Centro de Custo</h1>
            </center>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoCentroCusto" runat="server" Width="60px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricaoCentroCusto" runat="server" Width="150px"> </asp:TextBox>
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgCentroCustoPesquisar" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Width="25px" OnClick="imgCentroCustoPesquisar_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgCentroCustoEditar" runat="server" ImageUrl="~/img/edit.png"
                            Width="25px" OnClick="imgCentroCustoEditar_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgBtnCentroCustoExcluir" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="imgBtnCentroCustoExcluir_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgCentroCustoAdd" runat="server" ImageUrl="~/img/add.png" Width="25px"
                            OnClick="imgCentroCustoAdd_Click" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PnGridCentroCusto" runat="server" CssClass="GridContas">
                <asp:GridView ID="GridCentroCusto" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" GridLines="Vertical" OnRowDataBound="GridCentroCusto_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="RdoCentroCustoItem" runat="server" GroupName="GrCentroCusto"
                                    OnCheckedChanged="RdoCentroCustoItem_CheckedChanged" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="codigo_Centro_Custo" HeaderText="Codigo" />
                        <asp:BoundField DataField="descricao_centro_custo" HeaderText="CentroCusto" />
                        <asp:BoundField DataField="id_cc" HeaderText="Conta" />
                        <asp:BoundField DataField="Agrupamento" HeaderText="Agr" />

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
    </div>
    <asp:Panel ID="PnDetalhesGrupo" runat="server" CssClass="modalForm" Style="display: none" Height="220px">
        <asp:Panel ID="PnDetalhesGrupoFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Grupo</h1>
                <asp:Label ID="lblErroGrupoDetalhes" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaDetalhesGrupo" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaDetalhesGrupo_Click" />
                        <asp:Label ID="Label8" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaDetalhesGrupo" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaDetalhesGrupo_Click" />
                        <asp:Label ID="Label9" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoGrupoDetalhe" runat="server" Width="50px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricaoGrupoDetalhe" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Modalidade
                        </p>
                        <asp:DropDownList ID="ddlModalidade" runat="server">
                            <asp:ListItem Value="">------</asp:ListItem>
                            <asp:ListItem Value="DESPESAS">DESPESAS</asp:ListItem>
                            <asp:ListItem Value="RECEITAS">RECEITAS</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalGrupoDetalhe" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblErroGrupoDetalhes" DropShadow="true" PopupControlID="PnDetalhesGrupo"
        TargetControlID="lblErroGrupoDetalhes">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnSubGrupoDetalhes" runat="server" CssClass="modalForm" Style="display: none" Height="220px">
        <asp:Panel ID="pnSubGrupoDetalhesFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Sub Grupo</h1>
                <asp:Label ID="lblErrorSubGrupoDetalhes" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="imgBtnConfirmaSubGrupoDetalhes" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="imgBtnConfirmaSubGrupoDetalhes_Click" />
                        <asp:Label ID="Label2" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="imgBtnCancelaSubGrupoDetalhes" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="imgBtnCancelaSubGrupoDetalhes_Click" />
                        <asp:Label ID="Label3" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodGrupoSubGrupo" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Grupo
                        </p>
                        <asp:TextBox ID="txtDescricaoGrupoSubGrupo" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoSubGrupoDetalhes" runat="server" Width="50px"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricaoSubGrupoDetalhes" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalSubGrupoDetalhes" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblErrorSubGrupoDetalhes" DropShadow="true" PopupControlID="pnSubGrupoDetalhes"
        TargetControlID="lblErrorSubGrupoDetalhes">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnCentroCustoDetalhes" runat="server" CssClass="modalForm" Style="display: none" Height="300px">
        <asp:Panel ID="pnCentroCustoDetalhesFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Centro de Custo</h1>
                <asp:Label ID="lblCentroCustoErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="imgBtnConfirmaCentroCustoDetalhes" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="imgBtnConfirmaCentroCustoDetalhes_Click" />
                        <asp:Label ID="Label5" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="imgBtnCancelaCentroCustoDetalhes" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="imgBtnCancelaCentroCustoDetalhes_Click" />
                        <asp:Label ID="Label6" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodGrupoCentroCusto" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Grupo
                        </p>
                        <asp:TextBox ID="txtDescricaoGrupoCentroCusto" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <p>
                            Codigo &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SubGrupo
                        </p>
                        <asp:TextBox ID="txtCodSubGrupoCentroCusto" runat="server" Width="50px" Enabled="false"></asp:TextBox>

                        <asp:TextBox ID="txtDescricaoSubGrupoCentroCusto" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p>
                            Codigo 
                        </p>
                        <asp:TextBox ID="txtCodigoCentroCustoDetalhe" runat="server" Width="80px" MaxLength="9"></asp:TextBox>
                    </td>
                    <td>
                        <p>Descrição</p>
                        <asp:TextBox ID="txtDescricaoCentroCustoDetalhe" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Conta
                        </p>
                        <asp:TextBox ID="txtConta" runat="server" Width="200px"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" Enabled="true"
                            TargetControlID="txtConta" ServiceMethod="GetContas" ServicePath="PlanosDeContas.aspx"
                            MinimumPrefixLength="1" CompletionInterval="0" CompletionSetCount="20" EnableCaching="false"
                            BehaviorID="AutoCompleteExCli" CompletionListCssClass="autocomplete_completionListElement"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                            <Animations>
                                <OnShow>
                                    <Sequence>
                                        <%-- Make the completion list transparent and then show it --%>
                                        <OpacityAction Opacity="0" />
                                        <HideAction Visible="true" />
                            
                                        <%--Cache the original size of the completion list the first time
                                            the animation is played and then set it to zero --%>
                                        <ScriptAction Script="
                                            // Cache the size and setup the initial size
                                            var behavior = $find('AutoCompleteExCli');
                                            if (!behavior._height) {
                                                var target = behavior.get_completionList();
                                                behavior._height = target.offsetHeight - 2;
                                                target.style.height = '0px';
                                            }" />
                            
                                        <%-- Expand from 0px to the appropriate size while fading in --%>
                                        <Parallel Duration=".0">
                                            <FadeIn />
                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteExCli')._height" />
                                        </Parallel>
                                    </Sequence>
                                </OnShow>
                                <OnHide>
                                    <%-- Collapse down to 0px and fade out --%>
                                    <Parallel Duration=".0">
                                        <FadeOut />
                                        <Length PropertyKey="height" StartValueScript="$find('AutoCompleteExCli')._height" EndValue="0" />
                                    </Parallel>
                                </OnHide>
                            </Animations>
                        </asp:AutoCompleteExtender>
                    </td>
                    <td>
                        <p>
                            Agrupamento
                        </p>
                        <asp:TextBox ID="txtAgrupamento" runat="server" Width="80px" MaxLength="4"></asp:TextBox>
                    </td>

                </tr>
                <tr>
                    <td colspan="4">
                        <p>Conta Contábil</p>
                        <div class="panelItem" style="display:flex; align-content:space-between; width: 90%;  border: solid 1px; padding:10px ; border-radius:4px">
                            
                            <div class="panelItem" style="flex:1">
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
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalCentroCustoDetalhes" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblCentroCustoErro" DropShadow="true" PopupControlID="pnCentroCustoDetalhes"
        TargetControlID="lblCentroCustoErro">
    </asp:ModalPopupExtender>
</asp:Content>
