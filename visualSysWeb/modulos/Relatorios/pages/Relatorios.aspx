<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Relatorios.aspx.cs" Inherits="visualSysWeb.modulos.Relatorios.pages.Relatorios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../../Scripts/Excel/jquery.base64.js"></script>
    <script src="../../../Scripts/Excel/jquery.btechco.excelexport.js"></script>
    <script src="../js/Relatorios0.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <tr>
            <td>

                <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                <asp:Panel ID="pnSelecionaNovoRelatorio" runat="server" CssClass="FrameDivisaoTela">
                    <asp:Label ID="LblTitulo" runat="server" Text="Label" CssClass="titulos"></asp:Label>
                    <br />
                    <br />
                    <asp:RadioButtonList ID="RdoRelatorios" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RdoRelatorios_SelectedIndexChanged"
                        RepeatColumns="4" Width="100%">
                    </asp:RadioButtonList>
                    <hr />
                </asp:Panel>
                <asp:Panel ID="PnFrameFiltrosRelatorios" runat="server" CssClass="FrameFiltrosRelatorio" DefaultButton="ImgBtnVisualizarREL"
                    Visible="false">
                    <div class="btnImprimirDireita">
                        Limpar Filtros
            <asp:ImageButton ID="imgBtnLimpar" runat="server" OnClick="imgBtnLimpar_Click" ImageUrl="../../../img/botao-apagar.png"
                Style="width: 20px" />
                    </div>
                    <asp:Panel ID="Pnfiltros" runat="server" CssClass="FrameDivisaoTelaDireita">
                        <!--DATAS -->
                        <asp:ImageButton ID="ImageButton1" runat="server" Visible="false" />
                        <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                        <asp:CalendarExtender ID="cln1" runat="server" PopupButtonID="ImageButton1" TargetControlID="TextBox1">
                        </asp:CalendarExtender>
                        <asp:CalendarExtender ID="cln2" runat="server" PopupButtonID="ImageButton1" TargetControlID="TextBox1">
                        </asp:CalendarExtender>
                        <asp:CalendarExtender ID="cln3" runat="server" PopupButtonID="ImageButton1" TargetControlID="TextBox1">
                        </asp:CalendarExtender>
                        <asp:CalendarExtender ID="cln4" runat="server" PopupButtonID="ImageButton1" TargetControlID="TextBox1">
                        </asp:CalendarExtender>
                        <asp:CalendarExtender ID="cln5" runat="server" PopupButtonID="ImageButton1" TargetControlID="TextBox1">
                        </asp:CalendarExtender>
                    </asp:Panel>
                    <asp:Panel ID="pnBtnVisualiar" runat="server">
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <p>VISUALIZAR</p>
                        <asp:ImageButton ID="ImgBtnVisualizarREL" runat="server" ImageUrl="~/img/visualizar.png"
                            Width="80px" OnClick="btnVisualizar_Click" />
                        <br />
                        <br />
                        <br />
                        <br />
                    </asp:Panel>
                </asp:Panel>
                <div class="panelItem">
                    <asp:ImageButton ID="ImgBtnVoltar" runat="server" Height="28px" ImageUrl="~/img/icon_voltar.jpg"
                        Width="60px" Visible="false" OnClick="ImgBtnVoltar_Click" />
                </div>

                <div id="divBtnRefresh" runat="server" class="panelItem" visible="false">
                    <asp:ImageButton ID="btnPesquisar" runat="server" Height="28px" ImageUrl="~/img/Refresh.png"
                        Width="35px" OnClick="btnVisualizar_Click" ToolTip="F4" />
                    <b style="margin-top: 5px; float: right;">Atualizar</b>
                </div>

                <div id="divExcel" runat="server" class="btnImprimirDireita" visible="false">
                   <a href="#" onclick="javascript:abrirExcel(this)">
                        <img height="30px;" src="../../../img/excel.png" /><br />
                    </a>
                    <b>Excel</b>
                </div>

                <%-- <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/modulos/Relatorios/pages/RelatorioPrint.aspx"
                CssClass="btnImprimirDireita" Visible="false" Target="_blank">
                <asp:Image ID="Image1" ImageUrl="~/img/icon_imprimir.gif" runat="server" Width="40px" />
            </asp:HyperLink>--%>
                <div class="btnImprimirDireita" id="divImpressao" runat="server" style="margin-bottom: 0px;" visible="false">
                    <a href="#" onclick="PrintElem('MainContent_pnRelatorio')">
                        <asp:Image ID="Image3" ImageUrl="~/img/icon_imprimir.gif" runat="server" Width="40px" />
                    </a>
                </div>
                <asp:Panel ID="pnRelatorio" runat="server" Visible="false" ScrollBars="Auto" Width="100%">
                    <div class="cabecalhoRelatorio">
                        <asp:Image ID="imgLog" runat="server" name="logoBratter" CssClass="titleimg" />
                        <h1><asp:Label ID="lblCabecalho" runat="server" Text="Cabeçalho"></asp:Label></h1>
                        
                    </div>
                    <asp:Panel ID="pnRelatorioVisualizar" runat="server">
                        
                        <asp:Label ID="lblOrdemTitulo" runat="server" Text="Ordem:" CssClass="titulos"></asp:Label><asp:Label
                            ID="lblordem" runat="server" Text="" CssClass="titulos"></asp:Label>
                        <asp:Label ID="Label2" runat="server" Text="Filtros:" CssClass="titulos"></asp:Label><asp:Label
                            ID="lblFiltros" runat="server" Text="" CssClass="titulos"></asp:Label>
                        <asp:GridView ID="Gridrelatorio" runat="server" CellPadding="4" ForeColor="Black"
                            GridLines="Vertical" AllowSorting="True" Width="100%" BorderStyle="Solid" OnPageIndexChanging="Gridrelatoiro_PageIndexChanging"
                            OnSorting="GridView1_Sorting" BorderWidth="1px" OnDataBound="Gridrelatorio_DataBound"
                            OnRowCreated="Gridrelatorio_RowCreated" OnRowDataBound="Gridrelatorio_RowDataBound"
                            BorderColor="Black" BackColor="black" ShowFooter="True">
                            <AlternatingRowStyle BackColor="#D3D3D3" ForeColor="#284775" />
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Font-Size="Medium" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" BorderColor="Black"
                                BorderStyle="Solid" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </asp:Panel>
                    <div class="rodapeRelatorio">
                        <asp:Label ID="LblRodape" runat="server" Text="rodape"></asp:Label>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
                    <asp:Panel ID="PnConfirmaFamiliaFrame" runat="server" CssClass="frame">
                        <asp:Label ID="lbllista" runat="server" Text="Label" CssClass="cabMenu"></asp:Label>
                        <table>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                                        Width="25px" OnClick="btnFechar_Click" />
                                    <asp:Label ID="Label4" runat="server" Text="Seleciona"></asp:Label>
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                                        Width="25px" OnClick="btnCancelaLista_Click" />
                                    <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="Panel1" runat="server" CssClass="lista">
                            <asp:GridView ID="gridListaChk" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkListaTodos" runat="server" AutoPostBack="True" OnCheckedChanged="chkListaTodos_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkLista" runat="server" />
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
                        <%--  <asp:Panel ID="Panel1" runat="server" CssClass="lista">
                <asp:CheckBoxList ID="chkLista" runat="server" Height="50">
                </asp:CheckBoxList>
            </asp:Panel>--%>
                    </asp:Panel>
                </asp:Panel>
                <asp:ModalPopupExtender ID="modalPnFundo" runat="server" BackgroundCssClass="modalBackground"
                    DropShadow="true" PopupControlID="pnFundo" TargetControlID="lbllista">
                </asp:ModalPopupExtender>
                <asp:Panel ID="PnFundoFiltro" runat="server" CssClass="modalForm" Style="display: none">
                    <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
                        Style="font-size: 20px;">
                        <center><h1><asp:Label ID="lblTituloFiltro" runat="server" Text="" ></asp:Label></h1></center>
                        <hr />
                        <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                            Filtrar:
                <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="400px"></asp:TextBox>
                            <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Height="15px" OnClick="ImgPesquisaLista_Click" />
                            <asp:Label ID="lblErroPesquisa" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
                        <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                            <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                                <asp:ImageButton ID="btnConfirmaFiltro" runat="server" ImageUrl="~/img/confirm.png"
                                    Width="25px" OnClick="btnFecharFiltro_Click" />
                                <asp:Label ID="Label6" runat="server" Text="Confirma"></asp:Label>
                            </div>
                            <div class="row">
                                <asp:ImageButton ID="btnCancelaFiltro" runat="server" ImageUrl="~/img/cancel.png"
                                    Width="25px" OnClick="btnCancelaFiltro_Click" />
                                <asp:Label ID="Label7" runat="server" Text="Cancela"></asp:Label>
                            </div>
                        </div>
                        <asp:Panel ID="Panel4" runat="server" CssClass="lista">
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
                <asp:ModalPopupExtender ID="modalPnFiltro" runat="server" BackgroundCssClass="modalBackground"
                    DropShadow="true" PopupControlID="PnFundoFiltro" TargetControlID="lblTituloFiltro">
                </asp:ModalPopupExtender>

            </td>
        </tr>
    </table>

    <div id="somehiddentable" style="display:none;">

    </div>
</asp:Content>
