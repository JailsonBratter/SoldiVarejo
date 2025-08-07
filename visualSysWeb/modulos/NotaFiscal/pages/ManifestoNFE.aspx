<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManifestoNFE.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.ManifestoNFE" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../css/ManifetoNFE.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="cabMenu">
        <h1>Manifesto de NFE</h1>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>

    <div class="conteudo">
        <div ID="divFiltro" runat="server" class="row" style="padding: 20px; width: 95%;">
            <div class="panelItem">
                <p>CHAVE</p>
                <asp:TextBox ID="txtChave" runat="server" Width="350px"> </asp:TextBox>
            </div>
            

            <div class="panelItem">
                <p>Razão Social</p>
                <asp:TextBox ID="txtRazaoSocial" runat="server" Width="200px"> </asp:TextBox>
            </div>
            <div class="panelItem">
                <p>Data</p>
                <asp:DropDownList ID="ddlData" runat="server" Width="150px">
                    <asp:ListItem>EMISSAO</asp:ListItem>
                    <asp:ListItem>ENTRADA</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="panelItem">
                <p>De</p>
                <asp:TextBox ID="txtDe" runat="server" Width="100px" MaxLength="10" CssClass="data"> </asp:TextBox>
                <asp:Image ID="ImgDeCalendario" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDeCalendario"
                    TargetControlID="txtDe">
                </asp:CalendarExtender>
            </div>
            <div class="panelItem">
                <p>Ate</p>
                <asp:TextBox ID="txtAte" runat="server" Width="100px" MaxLength="10" CssClass="data"> </asp:TextBox>
                <asp:ImageButton ID="dt_txtAte" runat="server" ImageUrl="~/img/calendar.png" Height="15px" />
                <asp:CalendarExtender ID="ClnDataAte" runat="server" PopupButtonID="Dt_txtAte" TargetControlID="txtAte">
                </asp:CalendarExtender>
            </div>

            <div class="panelItem">
                <p>Status</p>
                <asp:DropDownList ID="ddlStatus" runat="server" Width="150px">
                    <asp:ListItem>TODOS</asp:ListItem>
                    <asp:ListItem>NAO MANIFESTADO</asp:ListItem>
                    <asp:ListItem>CIENCIA OPERACAO</asp:ListItem>
                    <asp:ListItem>CONFIRMADO OPERACAO</asp:ListItem>
                    <asp:ListItem>DESCONHECIMENTO DA OPERACAO</asp:ListItem>
                    <asp:ListItem>OPERACAO NAO REALIZADA</asp:ListItem>
                </asp:DropDownList>

            </div>

            <div class="panelItem">
                <p>Lançado</p>
                <asp:DropDownList ID="ddlLancado" runat="server" Width="80px">
                    <asp:ListItem>TODOS</asp:ListItem>
                    <asp:ListItem>SIM</asp:ListItem>
                    <asp:ListItem>NÃO</asp:ListItem>
                </asp:DropDownList>

            </div>



            <div class="panelitem btnImprimirDireita">
                <asp:ImageButton ID="imgBtnConsultarNotas" ImageUrl="../../../img/NFE.jpg" Width="30px" runat="server" OnClick="imgBtnConsultarNotas_Click" />
                <!--Consultar todas notas  SEFAZ-->
            </div>
            <div class="panelitem btnImprimirDireita">
                <asp:ImageButton ID="imgBtnConsultaChave" ImageUrl="../../../img/NFE.jpg" Width="30px" runat="server" OnClick="imgBtnConsultaChave_Click" />
                Consultar Chave SEFAZ
            </div>
            <div id="divImportarNotasPasta" runat="server" class="panelitem btnImprimirDireita" visible="false" >
                <asp:Button ID="btnImportarNFE" runat="server" OnClick="btnImportarNFE_Click" Text="Importar NFe" />
            </div>

        </div>

        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                <HeaderTemplate>
                    Resumo
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="cabecalhoRelatorio">
                        <asp:Button ID="BtnMesAnterior" Text="<<" CssClass=" btnManifestar btnAnterior" runat="server" OnClick="BtnMesAnterior_Click" />
                        <asp:Label ID="lblMesAtual" Text="MAIO" runat="server" />
                        <asp:Button ID="btnProximoMes" Text=">>" CssClass=" btnManifestar btnProximo" runat="server" OnClick="btnProximoMes_Click" />
                    </div>
                    <div id="divResumo" runat="server" class="calendario">

                        <div class="divDia divCabecalho">
                            DOM
                        </div>
                        <div class="divDia divCabecalho">
                            SEG
                        </div>
                        <div class="divDia divCabecalho">
                            TER
                        </div>
                        <div class="divDia divCabecalho">
                            QUA
                        </div>
                        <div class="divDia divCabecalho">
                            QUI
                        </div>
                        <div class="divDia divCabecalho">
                            SEX
                        </div>
                        <div class="divDia divCabecalho">
                            SAB
                        </div>
                        <%--primeira semana--%>
                        <div id="divS1D1" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS1D1" Text="1" runat="server" />
                            </div>
                            <div id="divConteudoS1D1" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS1D1" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS1D1" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS1D1" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS1D1" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS1D1" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS1D2" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS1D2" Text="2" runat="server" />
                            </div>
                            <div id="divConteudoS1D2" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS1D2" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS1D2" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS1D2" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS1D2" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS1D2" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS1D3" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS1D3" Text="3" runat="server" />
                            </div>
                            <div id="divConteudoS1D3" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS1D3" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS1D3" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS1D3" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS1D3" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS1D3" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS1D4" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS1D4" Text="4" runat="server" />
                            </div>
                            <div id="divConteudoS1D4" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS1D4" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS1D4" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS1D4" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS1D4" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS1D4" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS1D5" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS1D5" Text="5" runat="server" />
                            </div>
                            <div id="divConteudoS1D5" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS1D5" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS1D5" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS1D5" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS1D5" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS1D5" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS1D6" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS1D6" Text="6" runat="server" />
                            </div>
                            <div id="divConteudoS1D6" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS1D6" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS1D6" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS1D6" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS1D6" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS1D6" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS1D7" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS1D7" Text="7" runat="server" />
                            </div>
                            <div id="divConteudoS1D7" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS1D7" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS1D7" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS1D7" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS1D7" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS1D7" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <%--Segunda semana--%>
                        <div id="divS2D1" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS2D1" Text="1" runat="server" />
                            </div>
                            <div id="divConteudoS2D1" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS2D1" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS2D1" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS2D1" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS2D1" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS2D1" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS2D2" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS2D2" Text="2" runat="server" />
                            </div>
                            <div id="divConteudoS2D2" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS2D2" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS2D2" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS2D2" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS2D2" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS2D2" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS2D3" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS2D3" Text="3" runat="server" />
                            </div>
                            <div id="divConteudoS2D3" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS2D3" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS2D3" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS2D3" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS2D3" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS2D3" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS2D4" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS2D4" Text="4" runat="server" />
                            </div>
                            <div id="divConteudoS2D4" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS2D4" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS2D4" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS2D4" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS2D4" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS2D4" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS2D5" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS2D5" Text="5" runat="server" />
                            </div>
                            <div id="divConteudoS2D5" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS2D5" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS2D5" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS2D5" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS2D5" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS2D5" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS2D6" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS2D6" Text="6" runat="server" />
                            </div>
                            <div id="divConteudoS2D6" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS2D6" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS2D6" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS2D6" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS2D6" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS2D6" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS2D7" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS2D7" Text="7" runat="server" />
                            </div>
                            <div id="divConteudoS2D7" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS2D7" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS2D7" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS2D7" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS2D7" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS2D7" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <%--Terceira semana--%>
                        <div id="divS3D1" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS3D1" Text="1" runat="server" />
                            </div>
                            <div id="divConteudoS3D1" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS3D1" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS3D1" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS3D1" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS3D1" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS3D1" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS3D2" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS3D2" Text="2" runat="server" />
                            </div>
                            <div id="divConteudoS3D2" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS3D2" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS3D2" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS3D2" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS3D2" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS3D2" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS3D3" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS3D3" Text="3" runat="server" />
                            </div>
                            <div id="divConteudoS3D3" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS3D3" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS3D3" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS3D3" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS3D3" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS3D3" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS3D4" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS3D4" Text="4" runat="server" />
                            </div>
                            <div id="divConteudoS3D4" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS3D4" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS3D4" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS3D4" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS3D4" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS3D4" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS3D5" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS3D5" Text="5" runat="server" />
                            </div>
                            <div id="divConteudoS3D5" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS3D5" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS3D5" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS3D5" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS3D5" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS3D5" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS3D6" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS3D6" Text="6" runat="server" />
                            </div>
                            <div id="divConteudoS3D6" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS3D6" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS3D6" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS3D6" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS3D6" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS3D6" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS3D7" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS3D7" Text="7" runat="server" />
                            </div>
                            <div id="divConteudoS3D7" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS3D7" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS3D7" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS3D7" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS3D7" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS3D7" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <%--Quarta semana--%>
                        <div id="divS4D1" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS4D1" Text="1" runat="server" />
                            </div>
                            <div id="divConteudoS4D1" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS4D1" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS4D1" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS4D1" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS4D1" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS4D1" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS4D2" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS4D2" Text="2" runat="server" />
                            </div>
                            <div id="divConteudoS4D2" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS4D2" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS4D2" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS4D2" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS4D2" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS4D2" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS4D3" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS4D3" Text="3" runat="server" />
                            </div>
                            <div id="divConteudoS4D3" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS4D3" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS4D3" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS4D3" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS4D3" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS4D3" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS4D4" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS4D4" Text="4" runat="server" />
                            </div>
                            <div id="divConteudoS4D4" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS4D4" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS4D4" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS4D4" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS4D4" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS4D4" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS4D5" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS4D5" Text="5" runat="server" />
                            </div>
                            <div id="divConteudoS4D5" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS4D5" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS4D5" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS4D5" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS4D5" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS4D5" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS4D6" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS4D6" Text="6" runat="server" />
                            </div>
                            <div id="divConteudoS4D6" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS4D6" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS4D6" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS4D6" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS4D6" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS4D6" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS4D7" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS4D7" Text="7" runat="server" />
                            </div>
                            <div id="divConteudoS4D7" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS4D7" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS4D7" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS4D7" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS4D7" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS4D7" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <%--Quinta semana--%>
                        <div id="divS5D1" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS5D1" Text="1" runat="server" />
                            </div>
                            <div id="divConteudoS5D1" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS5D1" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS5D1" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS5D1" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS5D1" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS5D1" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS5D2" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS5D2" Text="2" runat="server" />
                            </div>
                            <div id="divConteudoS5D2" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS5D2" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS5D2" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS5D2" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS5D2" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS5D2" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS5D3" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS5D3" Text="3" runat="server" />
                            </div>
                            <div id="divConteudoS5D3" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS5D3" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS5D3" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS5D3" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS5D3" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS5D3" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS5D4" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS5D4" Text="4" runat="server" />
                            </div>
                            <div id="divConteudoS5D4" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS5D4" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS5D4" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS5D4" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS5D4" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS5D4" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS5D5" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS5D5" Text="5" runat="server" />
                            </div>
                            <div id="divConteudoS5D5" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS5D5" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS5D5" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS5D5" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS5D5" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS5D5" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS5D6" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS5D6" Text="6" runat="server" />
                            </div>
                            <div id="divConteudoS5D6" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS5D6" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS5D6" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS5D6" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS5D6" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS5D6" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS5D7" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS5D7" Text="7" runat="server" />
                            </div>
                            <div id="divConteudoS5D7" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS5D7" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS5D7" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS5D7" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS5D7" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS5D7" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS6D1" runat="server" class="divDia">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS6D1" Text="7" runat="server" />
                            </div>
                            <div id="divConteudoS6D1" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS6D1" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS6D1" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS6D1" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS6D1" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS6D1" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>
                        <div id="divS6D2" runat="server" class="divDia ">
                            <div class="diaTitulo">
                                <asp:Label ID="lblS6D2" Text="7" runat="server" />
                            </div>
                            <div id="divConteudoS6D2" runat="server" class="diaConteudo">
                                <asp:Button ID="btnTodasNotasS6D2" Text="1 nota" CssClass="btnManifestar" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnConfirmadoOperacaoS6D2" Text="1 nota" CssClass="btnManifestar btnConfimaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnCienciaOperacaoS6D2" Text="1 nota" CssClass="btnManifestar btnCienciaOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnDesconhecimentoOperacaoS6D2" Text="1 nota" CssClass="btnManifestar btnDesconhecimentoOperacao" runat="server" OnClick="btnDiaStatus_Click" />
                                <asp:Button ID="btnOperacaoNaoRealizadaS6D2" Text="1 nota" CssClass="btnManifestar btnOpecaoNaoRealizada" runat="server" OnClick="btnDiaStatus_Click" />
                            </div>
                        </div>


                    </div>

                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabDetalhes">
                <HeaderTemplate>
                    Notas Fiscais
                </HeaderTemplate>
                <ContentTemplate>

                    <asp:Button ID="btnConfirmacaoOperacaoManifestar" CssClass="btnManifestarAcao " Text="CONFIRMA OPERACAO" runat="server" OnClick="btnConfirmacaoOperacaoManifestar_Click" />
                    <asp:Button ID="btnCienciaOperacaoManifestar" CssClass="btnManifestarAcao " Text="CIENCIA OPERACAO" runat="server" OnClick="btnCienciaOperacaoManifestar_Click" />
                    <asp:Button ID="btnDesconhecimentoOperacaoManifestar" CssClass="btnManifestarAcao " Text="DESCONHECIMENTO DE OPERACAO" runat="server" OnClick="btnDesconhecimentoOperacaoManifestar_Click" />
                    <asp:Button ID="btnOperacaoNaoRealizadaManifestar" CssClass="btnManifestarAcao " Text="OPERACAO NÃO REALIZADA" runat="server" OnClick="btnOperacaoNaoRealizadaManifestar_Click" />
                    <asp:Button ID="btnBaixarXmls" CssClass="btnManifestarAcao" Text="GERAR XMLS" runat="server" OnClick="btnBaixarXmls_Click" />
                    <div class="panelItem" style="margin-bottom: 15px;">
                        <h1>
                            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
                    </div>
                    <div class="gridnf" style="height:500px">
                    <asp:GridView ID="gridNfs" runat="server" ForeColor="#333333" GridLines="Vertical"
                        AutoGenerateColumns="False" OnRowCommand="gridNfs_RowCommand"
                        CssClass="table" Width="100%" DataKeyNames="status,Xml">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelecionaItem" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelecionaItem_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Chave" HeaderText="Chave">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CNPJ" HeaderText="CNPJ" Visible="false">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RazaoSocial" HeaderText="Razao Social"></asp:BoundField>
                            <asp:BoundField DataField="emissao" HeaderText="Emissão" DataFormatString="{0:dd/MM/yyyy}">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="vNF" HeaderText="Valor" DataFormatString="{0:n2}">
                                <ItemStyle Width="80px" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Xml" HeaderText="Xml">
                                <ItemStyle Width="80px" HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="NotaLancada" HeaderText="Lcto">
                                <ItemStyle Width="80px" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <itemtemplate>
                                    <asp:Button ID="btnItens" Text="ITENS" runat="server" OnClick="btnItens_Click" />
                                </itemtemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SituacaoNFe" HeaderText="Sit.NFe">
                                <ItemStyle Width="80px" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NaturezaOperacao" HeaderText="Nat.Operação">
                                <ItemStyle Width="80px" HorizontalAlign="Center" />
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
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </div>
    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 100px; overflow: auto;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 30px; margin-bottom: 20px;">
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

    <asp:Panel ID="pnRespostasXml" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 100px; overflow: auto;">
            <h2>
                <asp:Label ID="lblResponstaXml" runat="server" Text="" ForeColor="Green"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 30px; margin-bottom: 20px;">
                        <asp:Button ID="btnRespostaXml" runat="server" Text="OK" Width="200px" Height="100%"
                            Font-Size="Larger" OnClick="btnRespostaXml_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalXml" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnRespostasXml" TargetControlID="lblResponstaXml">
    </asp:ModalPopupExtender>

    <asp:Timer ID="TimerXml" runat="server" OnTick="TimerXml_Tick" Enabled="false" Interval="30000">
    </asp:Timer>

    <asp:Panel ID="pnConfirmacaoManifestar" runat="server" CssClass="frameModal" Style="display: none; padding: 4%;">
        <div class="cabMenu" style="height: 130px">
            <h2>Tem Certeza que Gostaria de Manifestar<br />
                <asp:Label ID="lblConfirmaOperacao" runat="server" Text="" ForeColor="Red"></asp:Label><br />
                das Notas Selecionadas?
            </h2>
        </div>
        <div>
            <asp:Button ID="btnConfirmaOperacao" runat="server" Text="SIM"
                OnClick="btnConfirmaOperacao_Click"
                class="btnManifestarAcao btnConfirmacaoManifesto" />

            <asp:Button ID="btnCancelaOperacao" runat="server" Text="NÃO" Height="100%"
                class="btnManifestarAcao btnConfirmacaoManifesto" Style="margin-left: 50%;" OnClick="btnCancelaOperacao_Click" />
        </div>
        <div class="gridTable" style="height: 150px; overflow: auto;">
            <asp:GridView ID="gridNFManifestar" runat="server" ForeColor="#333333" GridLines="Vertical"
                AutoGenerateColumns="False"
                CssClass="table" Width="100%" DataKeyNames="status">
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="CNPJ" HeaderText="CNPJ">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="RazaoSocial" HeaderText="Razao Social"></asp:BoundField>
                    <asp:BoundField DataField="emissao" HeaderText="Emissão" DataFormatString="{0:dd/MM/yyyy}">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField DataField="vNF" HeaderText="Valor" DataFormatString="{0:n2}">
                        <ItemStyle Width="80px" HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status">
                        <ItemStyle HorizontalAlign="Center" />
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
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmarManifestar" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmacaoManifestar" TargetControlID="lblConfirmaOperacao">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnItemDetalhes" runat="server" CssClass="frameModal" Style="display: none; padding: 4%;">
        <div class="cabMenu" style="height: 50px">
            <h2>
                <asp:Label ID="Label1" runat="server" Text="Itens" ></asp:Label></h2>
        </div>
      
        <div style="align-items: flex-end; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: flex-end; height: 30px; margin-bottom: 20px;">
            <asp:ImageButton ImageUrl="~/img/cancel.png" Width="20" Height="20" runat="server" OnClick="Unnamed_Click" />
        </div>
        <div >
            <div class="panelItem col1 "><b>CHAVE:</b>:<asp:Label ID="lblChaveItens" runat="server" Text=""></asp:Label> </div>
        </div>
        <div>
            <div class="panelItem col1"><b>CNPJ:</b><asp:Label ID="lblCnpjItens" runat="server" Text=""></asp:Label>  </div>
            <div class="panelItem col1"><b>Razão Social:</b><asp:Label ID="lblRazaoSocialItens" runat="server" Text=""></asp:Label></div>
            <div class="panelItem col1"><b>Emissão:</b><asp:Label ID="lblEmissaoItens" runat="server" Text=""></asp:Label></div>
            <div class="panelItem col1"><b>Total:</b><asp:Label ID="lblTotalItens" runat="server" Text=""></asp:Label></div>
        </div>   
        <div class="gridTable" style="height: 150px; overflow: auto;">
            <asp:GridView ID="gridItensDetalhes" runat="server" ForeColor="#333333" GridLines="Vertical"
                AutoGenerateColumns="False"
                CssClass="table" Width="100%" >
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>

                    <asp:BoundField DataField="det_nItem" HeaderText="Item"></asp:BoundField>
                    <asp:BoundField DataField="det_prod_cProd" HeaderText="COD REF"></asp:BoundField>
                    <asp:BoundField DataField="det_prod_xProd" HeaderText="Descrição"></asp:BoundField>
                    <asp:BoundField DataField="det_prod_qCom" HeaderText="Qtde" DataFormatString="{0:N0}">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="det_prod_vUnCom" HeaderText="Unitario" DataFormatString="{0:N2}">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="det_prod_vProd" HeaderText="Total" DataFormatString="{0:N2}">
                        <ItemStyle Width="80px" HorizontalAlign="Right" />
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
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalItens" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnItemDetalhes" TargetControlID="Label1">
    </asp:ModalPopupExtender>

</asp:Content>
