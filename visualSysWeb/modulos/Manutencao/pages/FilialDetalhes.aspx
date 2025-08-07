<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FilialDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.FilialDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes da Filial</h1>
        </center>
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
                            Filial
                        </p>
                        <asp:TextBox ID="txtFilial" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Razão Social
                        </p>
                        <asp:TextBox ID="txtRazaoSocial" runat="server" Width="300px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Fantasia
                        </p>
                        <asp:TextBox ID="txtFantasia" runat="server" Width="200px"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
        <table>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            CNPJ
                        </p>
                        <asp:TextBox ID="txtCnpj" runat="server" Width="150px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            IE
                        </p>
                        <asp:TextBox ID="txtIe" runat="server" Width="150px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Plu Inicial
                        </p>
                        <asp:TextBox ID="txtPluinicial" runat="server" Width="60px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Conversor
                        </p>
                        <asp:TextBox ID="txtConversor" runat="server" Width="120px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Loja
                        </p>
                        <asp:TextBox ID="txtLoja" runat="server" Width="60px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            CST Pis Cofins
                        </p>
                        <asp:TextBox ID="txtCstPisCofins" runat="server" Width="90px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Pis
                        </p>
                        <asp:TextBox ID="txtPis" runat="server" Width="90px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Cofins
                        </p>
                        <asp:TextBox ID="txtCofins" runat="server" Width="90px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Endereco
                        </p>
                        <asp:TextBox ID="txtEndereco" runat="server" Width="250px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Numero
                        </p>
                        <asp:TextBox ID="txtEndereco_Nro" runat="server" Width="60px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Cidade
                        </p>
                        <asp:TextBox ID="txtCidade" runat="server" Width="150px"></asp:TextBox>
                    </div>

                    <div class="panelItem">
                        <p>
                            Bairro
                        </p>
                        <asp:TextBox ID="txtBairro" runat="server" Width="120px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            UF
                        </p>
                        <asp:TextBox ID="txtUF" runat="server" Width="60px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            CEP
                        </p>
                        <asp:TextBox ID="txtCEP" runat="server" Width="90px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <br />
                        <asp:CheckBox ID="chkBaixaCaderneta" Text="Baixa Caderneta" runat="server" />
                    </div>
                    <div class="panelItem">
                        <br />
                        <asp:CheckBox ID="chkGeraVendedor" Text="Gerar Vendedor" runat="server" />
                    </div>
                    <div class="panelItem">
                        <br />
                        <asp:CheckBox ID="chkDesmarcarAlteracoes" Text="Desmarca Alterações" runat="server" />
                    </div>
                       <div class="panelItem">
                        <br />
                        <asp:CheckBox ID="chkProduta" Text="Filial Produtora" runat="server" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Reg Estadual
                        </p>
                        <asp:TextBox ID="txtRegEstadual" runat="server" Width="150px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Reg Federal
                        </p>
                        <asp:TextBox ID="txtRegFederal" runat="server" Width="150px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            RPA
                        </p>
                        <asp:TextBox ID="txtRPA" runat="server" Width="90px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            IR
                        </p>
                        <asp:TextBox ID="txtIr" runat="server" Width="90px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            CSLL
                        </p>
                        <asp:TextBox ID="txtCsll" runat="server" Width="90px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Aliquota Est
                        </p>
                        <asp:TextBox ID="txtAliquota_est" runat="server" Width="70px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Aliquota Fed
                        </p>
                        <asp:TextBox ID="txtAliquota_fed" runat="server" Width="70px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>

                    <div class="panelItem">
                        <p>
                            Serie nfe
                        </p>
                        <asp:TextBox ID="txtSerie_nfe" runat="server" Width="100px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Fone
                        </p>
                        <asp:TextBox ID="txtFone" runat="server" Width="100px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Numero
                        </p>
                        <asp:TextBox ID="txtNumero" runat="server" Width="60px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            ICMSSN
                        </p>
                        <asp:TextBox ID="txtIcmsSN" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            CSOSN
                        </p>
                        <asp:TextBox ID="txtCsoSn" runat="server" Width="100px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            CRT
                        </p>
                        <asp:TextBox ID="txtCRT" runat="server" Width="120px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Dt Fech Estoque
                        </p>
                        <asp:TextBox ID="txtDataFechamentoEstoque" runat="server" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="imgDtFechamentoEstoque" ImageUrl="~/img/calendar.png" runat="server"
                            Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtFechamentoEstoque"
                            TargetControlID="txtDataFechamentoEstoque" Enabled="True">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p>
                            Dt Fech Financeiro
                        </p>
                        <asp:TextBox ID="txtDataFechamentoFinanceiro" runat="server" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="ImgDtFechamentoFinaneiro" ImageUrl="~/img/calendar.png" runat="server"
                            Height="15px" />
                        <asp:CalendarExtender ID="clnDataInicioPromocao" runat="server" PopupButtonID="ImgDtFechamentoFinaneiro"
                            TargetControlID="txtDataFechamentoFinanceiro" Enabled="True">
                        </asp:CalendarExtender>
                    </div>

                    <div class="panelItem">
                        <p>
                            Inicio periodo
                        </p>
                        <asp:TextBox ID="txtInicioPeriodo" CssClass="hora" runat="server" Width="80px"></asp:TextBox>

                    </div>

                    <div class="panelItem">
                        <p>
                            Fim Periodo
                        </p>
                        <asp:TextBox ID="txtFimPeriodo" CssClass="hora" runat="server" Width="80px"></asp:TextBox>

                    </div>

                    <div class="panelItem">
                        <p>
                            Fim Apos meia noite
                        </p>
                        <asp:DropDownList ID="ddlDiasPeriodo" runat="server">
                            <asp:ListItem Value="0">NAO</asp:ListItem>
                            <asp:ListItem Value="1">SIM</asp:ListItem>
                        </asp:DropDownList>

                    </div>

                     <div class="panelItem" style="margin-left:20px;">
                        <p>
                            Tipo PDV
                        </p>
                        <asp:DropDownList ID="ddlPDV" runat="server">
                            <asp:ListItem Value="1">SOLDI PDV</asp:ListItem>
                            <asp:ListItem Value="2">SOLDI PDV 2.0</asp:ListItem>
                            <asp:ListItem Value="3">ZANTHUS</asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Diretorio Gera
                        </p>
                        <asp:TextBox ID="txtDiretorioGera" runat="server" Width="400px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Diretorio Busca Preco
                        </p>
                        <asp:TextBox ID="txtDiretorioBuscaPreco" runat="server" Width="400px"></asp:TextBox>
                    </div>
                    <br />
                    <br />
                    <br />
                    <div class="panelItem">
                        <p>
                            Diretorio Exporta
                        </p>
                        <asp:TextBox ID="txtDiretorio_Exporta" runat="server" Width="400px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Diretorio MultiLoja
                        </p>
                        <asp:TextBox ID="txtDiretorio_multiloja" runat="server" Width="400px"></asp:TextBox>
                    </div>
                    <br />
                    <br />
                    <br />
                    <div class="panelItem">
                        <p>
                            Diretorio Balanca
                        </p>
                        <asp:TextBox ID="txtDiretorioBalanca" runat="server" Width="400px"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
