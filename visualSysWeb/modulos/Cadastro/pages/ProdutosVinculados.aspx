<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProdutosVinculados.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.ProdutosVinculados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/Mercadoria.js" type="text/javascript"></script>
    <link href="../css/Mercadoria.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Produtos Vinculados</h1>
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
                        MERCADORIA</p>
                    <asp:TextBox ID="txtMercadoria" runat="server" Width="200px" CssClass="sem"></asp:TextBox>
                </div>
                <div class="panelItem">
                    <p>
                        NCM
                    </p>
                    <asp:TextBox ID="txtNcm" runat="server" Width="80px" CssClass="sem"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <div class="gridTable" style="margin-top: -20px;">
        <div style="">
            <asp:GridView ID="gridMercadorias" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
                GridLines="Vertical" AllowSorting="True"  OnSorting="gridMercadoria_Sorting" ClientIDMode="Static"
                
                OnRowCreated="gridMercadorias_RowCreated" OnSelectedIndexChanged="gridMercadorias_SelectedIndexChanged1">
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>
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
    </div></asp:Content>
