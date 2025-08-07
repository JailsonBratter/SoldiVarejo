<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="NaturezaOperacaoDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.NaturezaOperacaoDetalhes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center> <h1>Detalhes do natureza_operacao</h1></center>
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
                    <div class="panelItem">
                        <p>
                            Codigo_operacao
                        </p>
                        <asp:TextBox ID="txtCodigo_operacao" runat="server" MaxLength="4" Width="80px" CssClass="numero"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Descricao
                        </p>
                        <asp:TextBox ID="txtDescricao" runat="server" Width="400px" MaxLength="60"></asp:TextBox>
                    </div>
                </td>
            </tr>

        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
        <table width="550px">
            <tr>
                <td>
                    <div class="panelItem">

                        <div class="panelItem" style="width:100px; border-width: 1px; border-style: solid">

                            <asp:RadioButtonList ID="rdoSaida" runat="server">
                                <asp:ListItem>NF Saida</asp:ListItem>
                                <asp:ListItem>NF Entrada</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="panelItem" style="border-width: 1px; border-style: solid">
                            <asp:RadioButtonList ID="rdoPreco" runat="server">
                                <asp:ListItem>Preco Venda</asp:ListItem>
                                <asp:ListItem>Preco Compra</asp:ListItem>
                            </asp:RadioButtonList>

                        </div>
                        <br />
                        <div class="panelItem" style="width: 215px; border-style: solid; border-width: 1px;">
                            <div class="panelItem">

                                <asp:CheckBox ID="chkNF_devolucao" runat="server" Text="NF devolucao" />
                            </div>
                            <div class="panelItem">

                                <asp:CheckBox ID="chkImprime_NF" runat="server" Text="Imprime NFe" />
                            </div>
                            <div class="panelItem">

                                <asp:CheckBox ID="chkDestOrigem" runat="server" Text="CNPJ Dest = Origem" />
                            </div>
                        </div>
                        <br />  
                       
                        <div class="panelItem" style="width: 215px; border-style: solid; border-width: 1px;">
                            <div class="panelItem" style="width:100%">
                                <asp:CheckBox ID="chkUtilizaCfop" runat="server" Text="Utiliza CFOP" />
                            </div>
                            <div class="panelItem">
                                <p>CFOP</p>
                                <asp:TextBox ID="txtCFOP" runat="server" Width="80px" MaxLength="4"></asp:TextBox>
                            </div>
                            <div class="panelItem">
                                <p>CFOP ST</p>
                                <asp:TextBox ID="txtCfopSt" runat="server" Width="80px" MaxLength="4"></asp:TextBox>
                            </div>
                        </div>
                        
                    </div>
                    <div class="panelItem" style="width: 272px; border-width: 1px; border-style: solid">
                        <div class="panelItem">
                            <asp:CheckBox ID="chkIncide_ICMS" runat="server" Text="Incide ICMS" />
                        </div>
                        <div class="panelItem" style=" margin-left: 10px;">
                               Cod Trib <span style="font-size: 9px;">*Saida</span>
                            <asp:TextBox ID="txtCStIcmsSaida" runat="server" Width="80px" MaxLength="2"></asp:TextBox>


                        </div>
                        <br />
                        <asp:CheckBox ID="chkincide_ST" runat="server" Text="Incide ICMS ST"  />
                        <br />
                        <asp:CheckBox ID="chkdespesas_Base" runat="server" Text="Despesas na Base ICMS" Visible="false" />
                        <br />
                        <asp:CheckBox ID="chkIpi_base" runat="server" Text="IPI na Base ICMS" Visible="false" />
                        <br />
                        <asp:CheckBox ID="chkIncide_IPI" runat="server" Text="Incide IPI" />
                        <br />
                        <div class="panelItem">
                            <asp:CheckBox ID="chkincide_PisCofins" runat="server" Text="Incide Pis e Cofins" />
                        </div>
                        <div class="panelItem" style="margin-top: -20px; margin-left: 10px;">
                            <p>
                                CST Pis/Cofins <span style="font-size: 9px;">*Entrada</span>
                            </p>
                            <asp:TextBox ID="txtCstPisCofins" runat="server" Width="80px" MaxLength="2"></asp:TextBox>
                            <p>
                                CST ICMS <span style="font-size: 9px;">*Entrada/Saída</span>
                            </p>
                            <asp:TextBox ID="txtCSTICMS" runat="server" Width="80px" MaxLength="2"></asp:TextBox>

                        </div>

                    </div>
                    <div class="panelItem" style="border-width: 1px; border-style: solid">
                        <asp:CheckBox ID="chkGera_custo" runat="server" Text="Atualiza custo do produto" Width="180px" />
                        <asp:CheckBox ID="chkTipo_movimentacao" runat="server" Text="Tipo movimentacao" Width="180px" />
                        <asp:CheckBox ID="chkGera_caderneta" runat="server" Text="Gera caderneta" />
                        <br />
                        <asp:CheckBox ID="chkBaixa_estoque" runat="server" Text="Atualiza estoque" Width="180px" />
                        <asp:CheckBox ID="chkGera_apagar_receber" runat="server" Text="Lança Financeiro" Width="180px" />
                        <asp:CheckBox ID="chkPermite_Desconto" runat="server" Text="Permite Desconto" />
                        <br />
                        <asp:CheckBox ID="chkGera_venda" runat="server" Text="Considera venda" />
                        <asp:CheckBox ID="ChkGera_Custo_Medio" runat="server" Text="Atualiza custo médio"  />
                        <asp:CheckBox ID="ChkGera_Precificacao" runat="server" Text="Precifica NF" />
                        <asp:CheckBox ID="chkDifal" runat="server" Text="DIFAL" />

                    </div>
                    <div class="panelItem" style="border-width: 1px; border-style: solid">
                        <asp:CheckBox ID="chkInativa" runat="server" Text="Inativa" Width="180px" />
                    </div>
                    <br />



                </td>
            </tr>

        </table>
    </div>
</asp:Content>
