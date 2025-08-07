<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SugestaoCompra.aspx.cs" Inherits="visualSysWeb.modulos.Estoque.pages.SugestaoCompra" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Sugestão de Compra</h1>
    </center>
    <hr />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <asp:Panel ID="pnSugestao" runat="server">
        <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
        </asp:Panel>
        <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
        <div class="panelDefault">
            <div class="row">
                <div class="panelItem">
                    <p>
                        PLU</p>
                    <asp:TextBox ID="txtPlu" runat="server" Width="50px" AutoPostBack="true" OnTextChanged="txtPlu_TextChanged"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        EAN</p>
                    <asp:TextBox ID="txtEan" runat="server" Width="90px" AutoPostBack="true" OnTextChanged="txtEan_TextChanged"></asp:TextBox>
                </div>
                 <div class="panelItem">
                    <p>
                        REF
                    </p>
                    <asp:TextBox ID="txtRefForn" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="txtRefForn_TextChanged"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        MERCADORIA</p>
                    <asp:TextBox ID="txtMercadoria" runat="server" Width="200px" AutoPostBack="true"
                        OnTextChanged="txtMercadoria_TextChanged"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        NCM
                    </p>
                    <asp:TextBox ID="txtNcm" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="txtMercadoria_TextChanged"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        CAMPO</p>
                    <asp:DropDownList ID="DllCampo1" runat="server">
                        <asp:ListItem Value="">---- </asp:ListItem>
                        <asp:ListItem Value="l.Preco">Venda</asp:ListItem>
                        <asp:ListItem Value="l.Preco_Custo">Custo</asp:ListItem>
                        <asp:ListItem Value="l.Margem">Margem</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="panelItem">
                    <p>
                        ONDE</p>
                    <asp:DropDownList ID="DllOnde" runat="server">
                        <asp:ListItem Value="=">é igual a </asp:ListItem>
                           <asp:ListItem Value="<">é menor que</asp:ListItem>
                        <asp:ListItem Value="<=">é menor ou igual a</asp:ListItem>
                        <asp:ListItem Value=">">é maior que</asp:ListItem>
                        <asp:ListItem Value=">=">é maior ou igual a </asp:ListItem>
                        <asp:ListItem Value="<">é menor que</asp:ListItem>
                        <asp:ListItem Value="<=">é menor ou igual a</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="panelItem">
                    <p>
                        CAMPO</p>
                    <asp:DropDownList ID="DllCampo2" runat="server">
                        <asp:ListItem Value="0">0 </asp:ListItem>
                        <asp:ListItem Value="l.Preco">Venda</asp:ListItem>
                        <asp:ListItem Value="l.Preco_Custo">Custo</asp:ListItem>
                        <asp:ListItem Value="l.Margem">Margem</asp:ListItem>
                    </asp:DropDownList>
                </div>
               
                <div class="panelItem">
                    <p>
                        PRAZO ENTREGA</p>
                    <asp:TextBox ID="txtPrazo" runat="server" Width="80px" AutoPostBack="true" Text="5"
                        OnTextChanged="txtRefForn_TextChanged"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <asp:CheckBox ID="chkEmbalagem" runat="server" Checked="true" Text="Considerar Embalagem" />
                </div>
            </div>
            <div class="row">
                <div class="panelItem">
                    <p>
                        GRUPO</p>
                    <asp:TextBox ID="txtGrupo" runat="server" Width="130px"> </asp:TextBox>
                    <asp:ImageButton ID="imgBtnGrupo" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                        OnClick="Img_Click" />
                </div>
                <div class="panelItem">
                    <p>
                        SUB GRUPO</p>
                    <asp:TextBox ID="txtSubGrupo" runat="server" Width="150px"> </asp:TextBox>
                    <asp:ImageButton ID="imgBtnSubGrupo" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </div>
                <div class="panelItem">
                    <p>
                        DEPARTAMENTO</p>
                    <asp:TextBox ID="txtDepartamento" runat="server" Width="130px"> </asp:TextBox>
                    <asp:ImageButton ID="imgBtnDepartamento" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </div>
                <div class="panelItem">
                    <p>
                        FAMILIA</p>
                    <asp:TextBox ID="txtFamilia" runat="server"> </asp:TextBox>
                    <asp:ImageButton ID="imgBtnFamilia" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </div>
                <div class="panelItem">
                    <p>
                        DATA</p>
                    <asp:DropDownList ID="ddlTipoData" runat="server">
                        <asp:ListItem Value="">---</asp:ListItem>
                        <asp:ListItem Value="l.DATA_ALTERACAO">DATA ALTERAÇÃO</asp:ListItem>
                        <asp:ListItem Value="a.DATA_CADASTRO">DATA CADASTRO</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="panelItem">
                    <p>
                        De</p>
                    <asp:TextBox ID="txtDe" runat="server" Width="80px" CssClass="DATA" AutoPostBack="true" MaxLength="10"
                        OnTextChanged="txtDataDe_TextChanged"> </asp:TextBox>
                    <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDe">
                    </asp:CalendarExtender>
                </div>
                <div class="panelItem">
                    <p>
                        Ate</p>
                    <asp:TextBox ID="txtAte" runat="server" Width="80px" CssClass="DATA" MaxLength="10"> </asp:TextBox>
                    <asp:ImageButton ID="ImgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataAte" runat="server" PopupButtonID="imgDtAte" TargetControlID="txtAte">
                    </asp:CalendarExtender>
                </div>
            </div>
            <div class="row">
                <div class="panelItem">
                    <p>
                        Fornecedor</p>
                    <asp:TextBox ID="txtFornecedor" runat="server" Width="200px"> </asp:TextBox>
                    <asp:ImageButton ID="imgFornecedor" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </div>
                <div class="panelItem">
                    <p>
                        Marca</p>
                    <asp:TextBox ID="txtMarca" runat="server" Width="200px"> </asp:TextBox>
                </div>
                <span class="Paneldireita">
                    <h2>
                        Total
                        <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label></h2>
                </span>
            </div>
        </div>
        <asp:Panel ID="Panel1" runat="server" CssClass="gridTable">
            <asp:GridView ID="gridMercadorias" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
                GridLines="Vertical" AllowSorting="True" OnSorting="gridMercadoria_Sorting" OnRowCreated="gridMercadorias_RowCreated"
                OnRowDataBound="gridMercadorias_RowDataBound">
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="PLU" HeaderText="PLU" ReadOnly="true" SortExpression="PLU">
                    </asp:BoundField>
                    <asp:BoundField DataField="EAN" HeaderText="EAN" ReadOnly="true" SortExpression="EAN">
                    </asp:BoundField>
                    <asp:BoundField DataField="REF" HeaderText="REF" ReadOnly="true" SortExpression="REF">
                    </asp:BoundField>
                    <asp:BoundField DataField="DESCRICAO" HeaderText="DESCRIÇÃO" ReadOnly="true" SortExpression="DESCRICAO">
                    </asp:BoundField>
                    <asp:BoundField DataField="EMB" HeaderText="EMB" ReadOnly="true" SortExpression="EMB"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SALDO" HeaderText="SALDO" ReadOnly="true" SortExpression="SALDO"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PRC_CUSTO" HeaderText="PRC CUSTO" ReadOnly="true" SortExpression="PRC_CUSTO"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                     <asp:BoundField DataField="MARGEM" HeaderText="MARG" ReadOnly="true" SortExpression="PRC_CUSTO"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PRC_VENDA" HeaderText="PRC VENDA" ReadOnly="true" SortExpression="PRC_VENDA"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MES5" ReadOnly="true" SortExpression="MES5" ItemStyle-HorizontalAlign="Right">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MES4" ReadOnly="true" SortExpression="MES4" ItemStyle-HorizontalAlign="Right">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MES3" ReadOnly="true" SortExpression="MES3" ItemStyle-HorizontalAlign="Right">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MES2" ReadOnly="true" SortExpression="MES2" ItemStyle-HorizontalAlign="Right">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ULT_30D" HeaderText="ULT 30D" ReadOnly="true" SortExpression="ULT_30D"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="VDA_MED" HeaderText="VDA MED" ReadOnly="true" SortExpression="VDA_MED"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="COB_CAD" HeaderText="COB CAD" ReadOnly="true" SortExpression="COB_CAD"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="COB_DIAS" HeaderText="COB DIAS" ReadOnly="true" SortExpression="COB_DIAS"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SUG_UNID" HeaderText="SUG (UNID)" ReadOnly="true" SortExpression="SUG_UNID"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="QTDE COMPRA (UNID)" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblQtdeCompra" runat="server" Text='<%# Eval("QTDE_COMPRA") %>' Visible="false"></asp:Label>
                            <asp:TextBox ID="txtQtde_Compra" runat="server" Text='<%# Eval("QTDE_COMPRA") %>'
                                Width="80px" CssClass="numero" onkeypress="return numeros(this,event);" AutoPostBack="true"
                                OnTextChanged="txtQtde_Compra_TextChanged"></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged"
                                Checked="true" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked="true" />
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
            <br />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pnCotacaoEscolha" runat="server" Visible="false" CssClass="modalFormTelaInteiraConfirma">
        <div style="border-bottom: solid; border-bottom-width: 1px; height: 40px;">
            <div style="width: 600px; float: left;">
                <h2>
                    Você Gostaria de ?</h2>
            </div>
            <div class="Paneldireita">
                <asp:ImageButton ID="imgBtnFechar" ImageUrl="~/img/cancel.png" runat="server" Width="20px"
                    OnClick="imgBtnFechar_Click" />
                Fechar
            </div>
        </div>
        <br />
        <asp:Button ID="btnNovaCotacao" runat="server" Text="Criar uma Nova Cotação" OnClick="btnNovaCotacao_Click"
            CssClass="submitButton" Height="40px" /><br />
        <h2>
            Incluir em uma cotação aberta</h2>
        <div class="gridTable">
            <asp:GridView ID="gridCotacaoAberta" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
                GridLines="Vertical" AllowSorting="True">
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="COTACAO" HeaderText="COTAÇÃO" ReadOnly="true" SortExpression="PLU">
                    </asp:BoundField>
                    <asp:BoundField DataField="DATA" HeaderText="DATA" ReadOnly="true" SortExpression="EAN">
                    </asp:BoundField>
                    <asp:BoundField DataField="DESCRICAO" HeaderText="DESCRIÇÃO" ReadOnly="true" SortExpression="DESCRICAO">
                    </asp:BoundField>
                    <asp:TemplateField HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Button ID="btnAdicionaCotacao" runat="server" Text="ADICIONA" OnClick="btnAdicionaCotacao_Click"
                                Width="100%" Height="30px" />
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
    <asp:Panel ID="PnEscolher" runat="server" CssClass="frameModal" Style="display: none">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Voce gostaria de incluir os itens Selecionados em  uma Cotação ou Gerar um Pedido de compra?"
                CssClass="cabMenu"></asp:Label>
        </h2>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnCotacao" runat="server" ImageUrl="~/modulos/Manutencao/imgs/Cotacao.png"
                        Width="30px" OnClick="btnCotacao_Click" />
                    <asp:Label ID="Label6" runat="server" Text="Cotação"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnPedido" runat="server" ImageUrl="~/modulos/Pedidos/imgs/pedidoCompra.jpg"
                        Width="30px" OnClick="btnPedido_Click" />
                    <asp:Label ID="Label7" runat="server" Text="Pedido"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelarEscolha" runat="server" ImageUrl="~/img/cancel.png"
                        Width="30px" />
                    <asp:Label ID="Label2" runat="server" Text="Cancelar"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalEscolher" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnEscolher" TargetControlID="Label1">
    </asp:ModalPopupExtender>


    <!-- *** MODAL OCORRÊNCIAS  -->

    <asp:Panel ID="pnOcorrencias" runat="server" CssClass="modalForm" Style="display: none; height: 380px;">
        <asp:Panel ID="Panel7" runat="server" CssClass="frame">
                <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="imgBtnCancelaOcorrencias" runat="server" ImageUrl="~/img/cancel.png" Width="25px" OnClick="imgBtnCancelaOcorrencias_Click" />
                        <asp:Label ID="Label26" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel3" runat="server" CssClass="lista">
                <asp:GridView ID="gridOcorrencias" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="Data" HeaderText="Data"></asp:BoundField>
                        <asp:BoundField DataField="Ocorrencia" HeaderText="Ocorrencia"></asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                        <asp:BoundField DataField="Usuario" HeaderText="Usuario"></asp:BoundField>
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
    <asp:ModalPopupExtender ID="modalOcorrencia" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="Label5" DropShadow="true" PopupControlID="pnOcorrencias" TargetControlID="Label5">
    </asp:ModalPopupExtender>

</asp:Content>
