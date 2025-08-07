<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VendasHoraMedia.aspx.cs" Inherits="visualSysWeb.modulos.Relatorios.pages.RelatoriosXml.venda.page.VendasHoraMedia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../../../../../Scripts/chartist/chartist.min.css" rel="stylesheet" />
    <link href="../css/VendasHoraMedia.css" rel="stylesheet" />
     
    <script src="../../../../js/Relatorios.js"></script>
    <script type="text/javascript" src="../../../../../../Scripts/chartist/chartist.min.js"></script>
    <script src="../js/VendasHoraMedia.js"></script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pnRelatorio" runat="server" Visible="true" ScrollBars="Auto" Width="100%">
        <div id="div1" runat="server" class="panelItem">
            <asp:ImageButton ID="ImgBtnVoltar" runat="server" Height="28px" ImageUrl="~/img/icon_voltar.jpg"
                Width="60px" Visible="true" OnClick="ImgBtnVoltar_Click" />
        </div>
        <div id="divBtnRefresh" runat="server" class="panelItem">
            <asp:ImageButton ID="imgBtnRefresh" runat="server" Height="28px" ImageUrl="~/img/Refresh.png"
                Width="35px" OnClick="btnVisualizar_Click" />
            <b style="margin-top: 5px; float: right;">Atualizar</b>
        </div>
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" CssClass="titulos"></asp:Label>
        <div id="divExcel" runat="server" class="btnImprimirDireita">
            <img height="30px;" onclick="javascript:abrirExcel(this)" src="../../../../../../img/excel.png" /><br />

            <b>Excel</b>
        </div>
        <div class="btnImprimirDireita" style="margin-bottom: 0px;">
            <asp:Image ID="Image1" ImageUrl="~/img/icon_imprimir.gif" runat="server" Width="40px" onclick="PrintElem('tbRelatorio')" />
        </div>
        <table id="tbRelatorio" class="gridTable">
            <tr>
                <td class="cabecalhoRelatorio" style="border-top: 1px solid; border-bottom: 1px solid;">
                    <asp:Image ID="imgLog" runat="server" CssClass="titleimg" Width="45px" ImageUrl="~/img/logo.jpg" />
                    <center><h3>Média de Vendas por Hora</h3></center>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblordem" runat="server" Text="" CssClass="titulos"></asp:Label><asp:Label
                        ID="Label1" runat="server" Text="" CssClass="titulos"></asp:Label>
                    <asp:Label ID="lblFiltros" runat="server" Text="Filtros:" CssClass="titulos"></asp:Label><asp:Label
                        ID="Label2" runat="server" Text="" CssClass="titulos"></asp:Label>
                    <br />
                </td>
            </tr>

            <tr>
                <td>
                        <div id="divRelatorio" runat="server" >
                    
                        </div>
                      
                        <div id="grafico1" class="ct-chart ct-square" >

                        </div>
                      <div style="display:inline-block; float:right; margin-right:15%;">
                          
                            <div class="legenda" style="background-color:#4169E1;">Vendas</div>
                            <div class="legenda" style="background-color:#F08080;">Qtdes</div>
                            <div class="legenda" style="background-color:#48D1CC;">Clientes</div>

                        </div>
                 
                       
                    


                </td>
            </tr>

        </table>
    </asp:Panel>
   
</asp:Content>
