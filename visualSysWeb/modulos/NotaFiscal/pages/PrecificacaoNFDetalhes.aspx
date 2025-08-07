<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PrecificacaoNFDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.pages.PrecificacaoNFDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>Precificação de Produtos por NF</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <asp:Panel ID="cabecalho" runat="server" CssClass="frame">
        <table>
            <tr>
                <td>
                    <p>
                        Codigo
                    </p>
                    <asp:TextBox ID="txtCodigo" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Série
                    </p>
                    <asp:TextBox ID="txtSerie" runat="server" Width="30px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        CNPJ
                    </p>
                    <asp:TextBox ID="txtFornecedor_CNPJ" runat="server" Width="120px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Fornecedor
                    </p>
                    <asp:TextBox ID="TxtNomeFornecedor" runat="server" Width="250px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Emissao
                    </p>
                    <asp:TextBox ID="txtEmissao" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Data
                    </p>
                    <asp:TextBox ID="txtData" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Total
                    </p>
                    <asp:TextBox ID="txtTotal" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>Usuario Cadastro </p>
                    <asp:TextBox ID="txtUsuario" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>Usuario Ult Alteracao </p>
                    <asp:TextBox ID="txtUsuario_Alteracao" runat="server" Width="80px"></asp:TextBox>
                </td>

                <td>
                    <p>
                        Usuario Precificacao
                    </p>
                    <asp:TextBox ID="txtUsuarioPrecificacao" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Data Precificação
                    </p>
                    <asp:TextBox ID="txtDataPrecificacao" runat="server" Width="80px"></asp:TextBox>
                </td>
            </tr>

        </table>
    </asp:Panel>
    <div class="row" style="padding: 20px; margin-left: 10px">
        <div class="panelItem">
            <asp:CheckBox ID="chkTodasFilias" runat="server" Text="Aplicar preços a Todas as Filiais" Checked="false" />
        </div>
        <div class="btnImprimirDireita" style="margin-right:5%">
            <asp:Button ID="btnMargem" runat="server" Text="Aplicar Margem" OnClick="btnMargem_Click" />
        </div>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Vertical"
            AutoGenerateColumns="False" CssClass="table" OnRowEditing="gridItens_RowEditing"
            OnRowCommand="gridItens_RowCommand" OnSelectedIndexChanged="gridItens_SelectedIndexChanged"
            OnRowUpdated="gridItens_RowUpdated" OnSorting="gridItens_Sorting" AllowSorting="True">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="PLU" HeaderText="PLU" ReadOnly="true" SortExpression="mercadoria.plu"></asp:BoundField>
                <asp:BoundField DataField="Descricao" HeaderText="Descrição" ReadOnly="true" SortExpression="mercadoria.descricao"></asp:BoundField>
                <asp:BoundField DataField="Preco_Custo_1" HeaderText="Custo Anterior" ItemStyle-HorizontalAlign="Right"
                    DataFormatString="{0:N2}" ReadOnly="true">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Preco_Custo" HeaderText="Custo NF Atual" ItemStyle-HorizontalAlign="Right"
                    DataFormatString="{0:N2}" ReadOnly="true">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="dif_custo" HeaderText="Diferença Custo" ItemStyle-HorizontalAlign="Right"
                    DataFormatString="{0:N2}" ReadOnly="true">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Margem" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblMargem" runat="server" Text='<%# Eval("Margem","{0:N4}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtMargem" runat="server" Text='<%# Eval("Margem","{0:N4}") %>'
                            Width="100px" CssClass="numero" onkeypress="return numeros(this,event);" OnTextChanged="txtMargem_textChanged"
                            AutoPostBack="True"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:BoundField DataField="Preco" HeaderText="Prc Vda Atual" ItemStyle-HorizontalAlign="Right"
                    DataFormatString="{0:N2}" ReadOnly="true">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Sugestão" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="txtSugestao" runat="server" Text='<%# Eval("Sugestao", "{0:N2}") %>'
                            Width="100px" CssClass="numero" onkeypress="return numeros(this,event);" OnTextChanged="txtSugestao_textChanged"
                            AutoPostBack="True"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle Width="5px" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:BoundField DataField="Codigo_Familia" HeaderText="Familia" ReadOnly="true"></asp:BoundField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged"
                            Checked="true" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelecionaItem" runat="server" AutoPostBack="True" Checked="true"
                            OnCheckedChanged="chkSelecionaItem_CheckedChanged" />
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

    <asp:Panel ID="pnMargem" runat="server" CssClass="frameModal" Style="display: none" DefaultButton="btnConfirmaMargem">
        <div class="cabMenu" style="height: 80px; overflow: auto; margin-bottom:0px;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="Valor Margem" ></asp:Label>
            </h2>

        </div>
        <div style="display: flex; justify-content: space-between; padding: 10px 20px; margin-top: 2q0px; ">
            <div class="panelItem">
                <asp:ImageButton ID="btnConfirmaMargem" runat="server" ImageUrl="~/img/confirm.png"
                    Width="25px" OnClick="btnConfirmaMargem_Click" />
                <asp:Label ID="Label22" runat="server" Text="Confirma"></asp:Label>
            </div>
            <div class="panelItem" style="margin-left: 40%;">
                <asp:ImageButton ID="btnCancelaMargem" runat="server" ImageUrl="~/img/cancel.png"
                    Width="25px" OnClick="btnCancelaMargem_Click" />
                <asp:Label ID="Label23" runat="server" Text="Cancela"></asp:Label>
            </div>
        </div>
        <div class="frame" style="width: 100%; padding:20px">
            <div class="row" style="display:flex; justify-content:center;">
                <div class="panelItem">
                    <p>Margem</p>
                    <asp:TextBox ID="txtMargemPadrao" runat="server" Width="80px" CssClass="numero" />  
                </div>
            </div>
        </div>
       
       
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalMargem" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnMargem" TargetControlID="lblErroPanel">
    </asp:ModalPopupExtender>
</asp:Content>
