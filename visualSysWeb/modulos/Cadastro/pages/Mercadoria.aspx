<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Mercadoria.aspx.cs" Inherits="visualSysWeb.Cadastro.Mercadoria" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  <script src="../js/Mercadoria.js" type="text/javascript"></script>
    <link href="../css/Mercadoria.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Produtos</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <div class="btnImprimirDireita">
        Limpar Filtros
        <asp:ImageButton ID="imgBtnLimpar" runat="server" OnClick="imgBtnLimpar_Click" ImageUrl="../../../img/botao-apagar.png"
            Style="width: 20px" />
    </div>
    <div class="filter" id="filtrosPesq" runat="server" style="margin-bottom: 0px;">
        <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
        <div class="panelDefault" style="margin-top: 0px;">
            <div class="row">
                <div class="panelItem">
                    <p>
                        PLU</p>
                    <asp:TextBox ID="txtPlu" runat="server" Width="50px" CssClass="sem"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        EAN</p>
                    <asp:TextBox ID="txtEan" runat="server" Width="90px" CssClass="sem"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        REF
                    </p>
                    <asp:TextBox ID="txtRefForn" runat="server" Width="80px" CssClass="sem"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        MERCADORIA</p>
                    <asp:TextBox ID="txtMercadoria" runat="server" Width="200px" CssClass="sem"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        NCM
                    </p>
                    <asp:TextBox ID="txtNcm" runat="server" Width="80px" CssClass="sem"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        CAMPO</p>
                    <asp:DropDownList ID="DllCampo1" runat="server" CssClass="sem">
                        <asp:ListItem Value="">---- </asp:ListItem>
                        <asp:ListItem Value="l.Preco">Venda</asp:ListItem>
                        <asp:ListItem Value="l.Preco_Custo">Custo</asp:ListItem>
                        <asp:ListItem Value="l.Margem">Margem</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="panelItem">
                    <p>
                        ONDE</p>
                    <asp:DropDownList ID="DllOnde" runat="server" CssClass="sem">
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
                    <asp:DropDownList ID="DllCampo2" runat="server" CssClass="sem">
                        <asp:ListItem Value="0">0 </asp:ListItem>
                        <asp:ListItem Value="l.Preco">Venda</asp:ListItem>
                        <asp:ListItem Value="l.Preco_Custo">Custo</asp:ListItem>
                        <asp:ListItem Value="l.Margem">Margem</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="panelItem" cssclass="sem">
                    <br />
                    <asp:CheckBox ID="chkPromocao" runat="server" Text="Promoção" AutoPostBack="true"
                        ForeColor="#006400" OnCheckedChanged="chk_CheckedChanged" Font-Bold="True" />
                </div>
                <div class="panelItem" cssclass="sem">
                    <br />
                    <asp:CheckBox ID="chkInativo" runat="server" Text="Inativo" AutoPostBack="true" ForeColor="Red"
                        Font-Bold="True" OnCheckedChanged="chk_CheckedChanged" ToolTip="Lista produtos Inativados" />
                </div> 
                <div class="panelItem" cssclass="sem">
                    <br />
                    <asp:CheckBox ID="chkPendente" runat="server" Text="Pendentes" AutoPostBack="true" CssClass="linhaPendente"
                        Font-Bold="True" OnCheckedChanged="chk_CheckedChanged" ToolTip="Lista produtos Pendentes" />
                </div>
                <div class="panelItem" cssclass="sem">
                    <br />
                    <asp:CheckBox ID="chkPrecoAtacado" runat="server" Text="Prc Atacado" AutoPostBack="true" CssClass="linhaPendente" ForeColor="Blue"
                        Font-Bold="True" OnCheckedChanged="chk_CheckedChanged" ToolTip="Lista produtos com preço de atacado > 0" />
                </div>
            </div>
            <div class="row">
                <div class="panelItem">
                    <p>
                        GRUPO</p>
                    <asp:TextBox ID="txtGrupo" runat="server" CssClass="sem"> </asp:TextBox>
                    <asp:ImageButton ID="imgBtnGrupo" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                        OnClick="Img_Click" />
                </div>
                <div class="panelItem">
                    <p>
                        SUB GRUPO</p>
                    <asp:TextBox ID="txtSubGrupo" runat="server" CssClass="sem"> </asp:TextBox>
                    <asp:ImageButton ID="imgBtnSubGrupo" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </div>
                <div class="panelItem">
                    <p>
                        DEPARTAMENTO</p>
                    <asp:TextBox ID="txtDepartamento" runat="server" CssClass="sem"> </asp:TextBox>
                    <asp:ImageButton ID="imgBtnDepartamento" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </div>
                <div class="panelItem">
                    <p>
                        FAMILIA</p>
                    <asp:TextBox ID="txtFamilia" runat="server" CssClass="sem"> </asp:TextBox>
                    <asp:ImageButton ID="imgBtnFamilia" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </div>
                <div class="panelItem">
                    <p>
                        DATA</p>
                    <asp:DropDownList ID="ddlTipoData" runat="server" CssClass="sem">
                        <asp:ListItem Value="">---</asp:ListItem>
                        <asp:ListItem Value="l.DATA_ALTERACAO">DATA ALTERAÇÃO</asp:ListItem>
                        <asp:ListItem Value="a.DATA_CADASTRO">DATA CADASTRO</asp:ListItem>
                        <asp:ListItem Value="l.DATA_INVENTARIO">DATA INVENTÁRIO</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="panelItem">
                    <p>
                        De</p>
                    <asp:TextBox ID="txtDe" runat="server" Width="80px" CssClass="DATA" AutoPostBack="true"
                        OnTextChanged="txtDataDe_TextChanged"> </asp:TextBox>
                    <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDe">
                    </asp:CalendarExtender>
                </div>
                <div class="panelItem">
                    <p>
                        Ate</p>
                    <asp:TextBox ID="txtAte" runat="server" Width="80px" CssClass="DATA"> </asp:TextBox>
                    <asp:ImageButton ID="ImgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataAte" runat="server" PopupButtonID="imgDtAte" TargetControlID="txtAte">
                    </asp:CalendarExtender>
                </div>
                   <div class="panelItem">
                    <p>
                        Trib Saida
                    </p>
                    <asp:TextBox ID="txtTribSaida" runat="server" CssClass="sem" Width="50px"> </asp:TextBox>
                    <asp:ImageButton ID="imgBtnTribSaida" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </div>
                 <div class="panelItem">
                    <p>
                       CST PIS/COFINS
                    </p>
                    <asp:TextBox ID="txtCSTPisCofins" runat="server" CssClass="sem" Width="50px"> </asp:TextBox>
                    
                </div>
            </div>
            <div class="row">
                <div class="panelItem">
                    <p>
                        Tipo de Produto</p>
                    <asp:TextBox ID="txtTipo" runat="server" CssClass="sem"> </asp:TextBox>
                    <asp:ImageButton ID="imgBtnTipo" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </div>
                    <div class="panelItem">
                    <p>
                        PLU Relacionado
                    </p>
                    <asp:TextBox ID="txtPLURelacionado" runat="server" CssClass="sem" Width="50px"> </asp:TextBox>
                </div>
            </div>
            <div class="row" style="margin-bottom: 0px;">
                <br />
                <div class="panelItem" style="margin-bottom: 0px;">
                    <h1>
                        <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
                </div>
                  <div class="direitaFechar" style="margin-right: 30px;">

                    <asp:Button ID="btnPagInicio" runat="server" Text="<<" Font-Size="Large" OnClick="btnPag_Click" />
                    <asp:Button ID="btnPagAnterio" runat="server" Text="<" Font-Size="Large" OnClick="btnPag_Click" />
                    <asp:DropDownList ID="ddlPag" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPag_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Button ID="btnPagProximo" runat="server" Text=">" Font-Size="Large" OnClick="btnPag_Click" />
                    <asp:Button ID="btnPagFim" runat="server" Text=">>" Font-Size="Large" OnClick="btnPag_Click" />
                </div>
                
            </div>
        </div>
    </div>
     <div class="row">
         <div class="panelItem" style="margin-top: 0px; margin-left: 10px;">
                    <asp:ImageButton ID="imgBtnSalvarInativos" runat="server" Visible="false" ImageUrl="../../../img/confirm.png"
                        Width="30px" OnClick="imgBtnSalvarInativos_Click" />
                    <asp:Label ID="lblSalvarInativos" runat="server" Text="Ativar Produtos Selecionados"
                        Visible="false" Font-Size="Medium"></asp:Label>
                </div>
        <div class="panelItem" style="margin-bottom: 10px; margin-left: 30px;">
            <asp:Button ID="btnTransmitir" runat="server" OnClick="btnTransmitir_Click" Text="Transmitir WebServer"
                Height="35px" Visible="true" />
        </div>
    </div>

    <div class="gridTable" style="margin-top: -20px;">
        <div style="">
            <asp:GridView ID="gridMercadorias" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
                GridLines="Vertical" AllowSorting="True"  OnSorting="gridMercadoria_Sorting" ClientIDMode="Static"
                
                OnRowCreated="gridMercadorias_RowCreated" OnSelectedIndexChanged="gridMercadorias_SelectedIndexChanged1">
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField >
                         <HeaderTemplate>
                            <asp:CheckBox ID="chkSeleciona" runat="server" onclick="javascript:selecionar(this);" 
                                Checked="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelecionaItem" runat="server"  Checked="false" />
                            <asp:Label ID="lblInativo" Visible="false" Text='<%# Eval("inativo") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:HyperLinkField DataTextField="PLU" Text="---" Visible="true" HeaderText="PLU" 
                        DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="a.PLU" />
                    <asp:HyperLinkField DataTextField="EAN"  Text="-" Visible="true" HeaderText="EAN"
                        DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="b.EAN" />
                    <asp:HyperLinkField DataTextField="Refer" Text="-" Visible="true" HeaderText="Ref"
                        DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="a.Ref_Fornecedor" />
                    <asp:HyperLinkField DataTextField="Mercadoria"  Text="---" Visible="true" HeaderText="Mercadoria"
                        DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="ltrim(rtrim(a.descricao))" />
                    <asp:HyperLinkField DataTextField="Preco_custo" Text="0,00" Visible="true" ItemStyle-HorizontalAlign="Right"
                        HeaderText="Custo" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="l.preco_custo">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="Margem" Text="0,00" Visible="true" ItemStyle-HorizontalAlign="Right"
                        HeaderText="Margem" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="l.margem">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="Preco" Text="0,00" Visible="true" ItemStyle-HorizontalAlign="Right"
                        HeaderText="Preço" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="l.preco">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="Preco_promocao" Text="0,00" Visible="true" ItemStyle-HorizontalAlign="Right"
                        HeaderText="Promo" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="l.Preco_promocao">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="Qtde_Atacado" Text="0,00" Visible="false" ItemStyle-HorizontalAlign="Right"
                        HeaderText="Qtde Atacado" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="a.Qtde_Atacado">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="Preco_Atacado" Text="0,00" Visible="false" ItemStyle-HorizontalAlign="Right"
                        HeaderText="Prc Atacado" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="a.Preco_Atacado">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="Saldo_atual" Text="0,00" Visible="true" ItemStyle-HorizontalAlign="Right"
                        HeaderText="Estoque" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="l.saldo_atual">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="Peso_Bruto" Text="0,00" Visible="true" ItemStyle-HorizontalAlign="Right"
                        HeaderText="Peso Bruto" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="a.peso_bruto">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="Data_alteracao" Text="---" Visible="true" ItemStyle-HorizontalAlign="Right"
                        HeaderText="Ult Alte" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="l.Data_Alteracao">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="ICMS_SAIDA" Text="" Visible="true" HeaderText="ICMS"
                        ItemStyle-HorizontalAlign="Right" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="right(replicate('0',3)+isnull(ltrim(rtrim(t.Indice_ST)),'00'),3)+'-'+RIGHT( REPLICATE('0',4) + CONVERT(VARCHAR(10),CONVERT(MONEY,isnull(t.ICMS_Efetivo,0))), 5) ">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="PISCofins" Text="" Visible="true" HeaderText="PISCofi"
                        ItemStyle-HorizontalAlign="Right" DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="a.cst_saida">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="NCM" Text="" Visible="true" HeaderText="NCM" ItemStyle-HorizontalAlign="Right"
                        DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="isnull(a.cf,'')">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="DataInventario" Text="" Visible="true" HeaderText="Ult Inv" ItemStyle-HorizontalAlign="Right"
                        DataNavigateUrlFormatString="~/modulos/Cadastro/pages/MercadoriaDetalhes.aspx?plu={0}"
                        DataNavigateUrlFields="PLU" SortExpression="l.Data_Inventario">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:HyperLinkField>

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
        </div>
    </div>
     <div class="direitaFechar" style="margin-right: 30px;">
        <br />
        <asp:Button ID="btnPagInicio1" runat="server" Text="<<" Font-Size="Large" OnClick="btnPag_Click" />
        <asp:Button ID="btnPagAnterio1" runat="server" Text="<" Font-Size="Large" OnClick="btnPag_Click" />
        <asp:DropDownList ID="ddlPag1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPag_SelectedIndexChanged">
        </asp:DropDownList>
        <asp:Button ID="btnPagProximo1" runat="server" Text=">" Font-Size="Large" OnClick="btnPag_Click" />
        <asp:Button ID="btnPagFim1" runat="server" Text=">>" Font-Size="Large" OnClick="btnPag_Click" />
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
                    <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
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
    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 200px; overflow: auto;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap;
                        justify-content: center; height: 30px; margin-bottom: 20px;">
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
    <asp:Panel ID="pnTransmitirWebServer" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px; overflow: auto;">
            <h2>
                <asp:Label ID="lblDetalhesTrans" runat="server" Text=""></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div id="divBtnOKTrans" runat="server" style="align-items: center; display: flex;
                        flex-direction: row; flex-wrap: wrap; justify-content: center; height: 30px;
                        margin-bottom: 20px;">
                        <asp:Button ID="btnOkTransmissao" runat="server" Text="Aguarde.." Width="200px" Height="100%"
                            Enabled="false" Font-Size="Larger" OnClick="btnOkTransmissao_Click" />
                    </div>
                </td>
            </tr>
        </table>
        <asp:Timer ID="TimerTrans" runat="server" OnTick="TimerTrans_Tick" Enabled="false"
            Interval="30000">
        </asp:Timer>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalTransmitirWebServer" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnTransmitirWebServer" TargetControlID="lblDetalhesTrans">
    </asp:ModalPopupExtender>
</asp:Content>
