<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CartaCorrecaoHtmlPdf.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.pages.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../../Styles/bootstrap.css" rel="stylesheet" />
    <link href="../css/CartaCorrecaoHtmlPdf.css" rel="stylesheet" />
    <title></title>
</head>
<body style="background-color:#FFFFFF">
    <form id="form1" runat="server">
        <div class="container borderComplete" style="height: 100%">
            <!--Emitente -->
            <div class=" col-xs-12 borderComplete" id="Emitente" style="height: auto">
                 <!--Cabecalho -->
                <div class="row col-xs-12 borderbottom " style="height: auto" id="DvCabecalho">
                    <div class="col-xs-6 text-center " style="height: auto">
                        <!-- Info Emit-->
                        <div class="col-xs-4 SPadLateral" style="padding-top: 15px">
                            <asp:Image ID="ImgLogo" runat="server" ImageAlign="Left" CssClass="img-responsive" />
                        </div>
                        <div class="col-xs-8">
                            <h5>IDENTIFICAÇÃO DO EMITENTE</h5>
                            <asp:Label ID="LblRazaoSEmit" runat="server" Font-Size="X-Large" Font-Bold="True"></asp:Label>
                            <br />
                            <asp:Label ID="LblEnderecoEmit" runat="server" Font-Size="Small"></asp:Label>
                            <br />
                            <asp:Label ID="LblTelEmit" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                        <!-- End Info Emit-->

                    </div>
                    <div class="col-xs-6 text-center borderLeft SPadLateral" style="height: auto">
                         <!-- Info Cod Barra-->
                        <div class=" col-xs-12 borderbottom">
                            <h4>CC-e</h4>
                            <h1>CARTA DE CORREÇÃO ELETRÔNICA DE NF-e</h1>
                        </div>
                        <div class="col-xs-12 borderbottom">
                            <h5 class="text-left">CHAVE DE ACESSO DA NF-e</h5>
                            <asp:Label ID="LblChave" runat="server" Text="" Font-Size="Small" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="col-xs-12" style="height: auto; padding-bottom: 5px; padding-top: 3px">
                            <asp:Image ID="ImgCodBarras" runat="server" Height="63px" Width="100%" CssClass="img-responsive" />
                        </div>
                        <!-- End Info Cod Barra-->
                    </div>
                    
                </div>
                <!--End Cabecalho -->

                <div class="row col-xs-12 SPadLateral text-center" style="margin: 0">
                    <div class="col-xs-6 col-sm-6 SPadLateral" style="margin: 0">
                        <!--IE -->
                        <div class="col-xs-5" id="DvIE">
                            <h5>INSCRIÇÃO ESTADUAL</h5>
                            <asp:Label ID="LblIEEmit" runat="server" Font-Size="Medium" Font-Bold="True"></asp:Label>
                        </div>
                        <!--End IE -->
                        <!--CNPJ -->
                        <div class="col-xs-6 borderLeft" id="DvCNPJ">
                            <h5>CNPJ</h5>
                            <asp:Label ID="LblCNPJEmit" runat="server" Font-Size="Medium" Font-Bold="True"></asp:Label>
                        </div>
                        <!--End CNPJ -->
                    </div>
                    <div class="col-xs-6 col-sm-6 SPadLateral">
                        <div class=" col-xs-2 col-sm-2 borderLeft">
                            <h5>MODELO</h5>
                            <asp:Label ID="LblModelo" runat="server" Text="55" Font-Size="Small" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="col-xs-2 col-sm-2 borderLeft">
                            <h5>SÉRIE</h5>
                            <asp:Label ID="LblSerie" runat="server" Text="1" Font-Size="Small" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="col-xs-3 col-sm-3 borderLeft">
                            <h5>NÚMERO DA NF-e</h5>
                            <asp:Label ID="LblNumNFE" runat="server" Font-Size="Small" Font-Bold="True"></asp:Label>
                        </div>
                        <div class=" col-xs-3 col-sm-3 borderLeft">
                            <h5>EMISSÃO</h5>
                            <asp:Label ID="LblDTEmissao" runat="server" Font-Size="Small" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="col-xs-2 col-sm-2 borderLeft">
                            <h5>FOLHA</h5>
                            <asp:Label ID="LblFolha" runat="server" Text="1/1" Font-Size="Small" Font-Bold="True"></asp:Label>
                        </div>
                    </div>
                </div>
                <!--End Row -->
            </div>
            <!--End Emitente -->

            <!-- DESTINATÁRIO -->
            <p>DESTINATÁRIO / REMETENTE</p>
            <div class="row col-xs-12 borderComplete" id="DESTINATARIO">
                <div class="row col-xs-12 borderbottom">
                    <div class="col-xs-10 ">
                        <h5>NOME / RAZÃO SOCIAL</h5>
                        <asp:Label ID="LblRazaoSDest" runat="server" Font-Size="Small"></asp:Label>
                    </div>
                    <div class="col-xs-2 borderLeft">
                        <h5>CNPJ / CPF</h5>
                        <asp:Label ID="LblCNPJDest" runat="server" Font-Size="Small" Font-Bold="True"></asp:Label>
                    </div>
                </div>
                <div class="row col-xs-12 borderbottom">
                    <div class="col-xs-7 ">
                        <h5>ENDEREÇO</h5>
                        <asp:Label ID="LblEnderecoDest" runat="server" Font-Size="Small"></asp:Label>
                    </div>
                    <div class="col-xs-4 borderLeft">
                        <h5>BAIRRO / DISTRITO</h5>
                        <asp:Label ID="LblBairroDest" runat="server" Font-Size="Small"></asp:Label>
                    </div>
                    <div class="col-xs-1 borderLeft" style="padding-left: 10px;">
                        <h5>CEP</h5>
                        <asp:Label ID="LblCEPDest" runat="server" Font-Size="Small"></asp:Label>
                    </div>
                </div>
                <div class="row col-xs-12">
                    <div class="col-xs-7">
                        <h5>MUNICÍPIO</h5>
                        <asp:Label ID="LblMunicipioDest" runat="server" Font-Size="Small"></asp:Label>
                    </div>
                    <div class="col-xs-1 borderLeft">
                        <h5>UF</h5>
                        <asp:Label ID="LblUFDest" runat="server" Font-Size="Small"></asp:Label>
                    </div>
                    <div class="col-xs-2 borderLeft">
                        <h5>FONE / FAX</h5>
                        <asp:Label ID="LblTelDest" runat="server" Font-Size="Small"></asp:Label>
                    </div>
                    <div class="col-xs-2 borderLeft">
                        <h5>INSCRIÇÃO ESTADUAL</h5>
                        <asp:Label ID="LblIEDest" runat="server" Font-Size="Small"></asp:Label>
                    </div>
                </div>
            </div>
            <!--End Row DESTINATÁRIO -->

            <p>CONDIÇÃO DE USO</p>
            <div class="row col-xs-12 borderComplete">
                <h5 style="font-size: 13px">A Carta de Correção é disciplinada pelo § 1º-A do art. 7º do Convênio S/N, de 15 de dezembro de 1970 e pode ser utilizada para regularização de erro ocorrido na emissão de documento fiscal, desde que o erro não esteja relacionado com: I - as variáveis que determinam o valor do imposto tais como: base de cálculo, alíquota, diferença de preço, quantidade, valor da operação ou da prestação; II - a correção de dados cadastrais que implique mudança do remetente ou do destinatário; III - a data de emissão ou de saída.</h5>
            </div>

            <!--Correcoes -->
            <p>EVENTOS / CORREÇÕES</p>
            <div class="row col-xs-12 borderComplete" style="height: 400px">
                <!--Border bottom -->
                <div class="col-xs-12 borderbottom">
                    <div class="col-xs-1 " id="DVSeq">
                        <h5>Seq.</h5>
                        <asp:Label ID="LblSequencia" runat="server" Text="1" Font-Size="Small" Font-Bold="True"></asp:Label>
                    </div>
                    <div class="col-xs-5 " id="DVStatus">
                        <h5>STATUS/MOTIVO</h5>
                        <asp:Label ID="LblStatus" runat="server" Text="135 Evento registrado e vinculado a NF-e" Font-Size="Small" Font-Bold="True"></asp:Label>
                    </div>
                    <div class="col-xs-3 " id="DVDt">
                        <h5>DATA DO REGISTRO</h5>
                        <asp:Label ID="LblDtRegistro" runat="server" Font-Size="Small" Font-Bold="True"></asp:Label>
                    </div>
                    <div class="col-xs-3 " id="DVProt">
                        <h5>NÚMERO DO PROTOCOLO</h5>
                        <asp:Label ID="LblProtocolo" runat="server" Font-Size="Small" Font-Bold="True"></asp:Label>
                    </div>
                </div>
                <!--End Border bottom -->
                <div style="padding: 8px">
                    <asp:Label ID="LblCarta" runat="server" Font-Size="Medium"></asp:Label>
                </div>
            </div>
            <!--End Correcoes -->

        </div>
        <!--End Conteiner -->
    </form>
</body>
</html>
