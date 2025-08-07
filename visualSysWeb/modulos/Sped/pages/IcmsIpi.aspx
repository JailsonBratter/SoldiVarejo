<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="IcmsIpi.aspx.cs" Inherits="visualSysWeb.modulos.Sped.pages.IcmsIpi" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/IcmsIpi.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/bootstrap.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height: 800px;">
        <center><h1>Sped Icms-Ipi</h1></center>
        <hr />
        <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
        </asp:Panel>
        <br />
        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        <div class="BlocoPag">
            <p class="cabecalhoBloco">
                Blocos a ser Gerados</p>
            <asp:CheckBox ID="chkTodos" runat="server" Text="Todos" Checked="true" onclick="javascript:chkTodos();" />
            <asp:Panel ID="pnBlocos" runat="server" Style="overflow: auto; height: 90%;">
            </asp:Panel>
        </div>
        <div class="btnImprimirDireita">
            Limpar Arquivos
            <asp:ImageButton ID="imgBtnLimpar" runat="server" ImageUrl="../../../img/botao-apagar.png"
                Style="width: 20px" OnClick="imgBtnLimpar_Click" />
        </div>
        <div class="BlocoPag" style="width: 40%; height: 100px;">
            <p class="cabecalhoBloco">
                Filtros</p>
            <div class="panelItem">
                <p class="">
                    De</p>
                <asp:TextBox ID="txtDe" runat="server" Width="80px" CssClass="DATA" OnTextChanged="txtDe_TextChanged"> </asp:TextBox>
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
            <asp:Panel ID="pnFiltros" runat="server">
            </asp:Panel>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="BlocoPag" style="height: 100px; margin-top: 30px;">
                    <p class="cabecalhoBloco">
                        Geração</p>
                    <div class="panelItem">
                        <asp:Button ID="btnGerarBlocos" runat="server" Text="Gerar Blocos Selecionados" Height="35px"
                            OnClick="btnGerarBlocos_Click" />
                    </div>
                    <div class="panelItem">
                        <asp:Button ID="btnArquivoFinal" runat="server" Text="Gerar Arquivo Final" OnClick="btnArquivoFinal_Click"
                            Height="35px" />
                    </div>
                </div>
                <div id="divPrecessamento" runat="server" class="BlocoPag" visible="false" style="height: 250px;
                    margin-top: 30px;">
                    <p class="cabecalhoBloco">
                        Processamento</p>
                    <div class="panelItem" style="width: 100%;">
                        Inicio:<asp:Label ID="lblInicio" runat="server" Text=""></asp:Label>
                        Tempo:<asp:Label ID="lblTempo" runat="server" Text=""></asp:Label>
                        <br />
                        Arquivos :
                        <asp:Label ID="lblArqGerados" runat="server" Text=""></asp:Label>
                        de
                        <asp:Label ID="lblArquivoSelecionados" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="progress" style="margin-top: 100px;">
                        <div id="barraTotal" runat="server" class="progress-bar progress-bar-warning progress-bar-striped active"
                            role="progressbar" aria-valuenow="30" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                            <asp:Label ID="lblProgressTotal" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    Processando:<asp:Label ID="lblProgressItens" runat="server" Text=""></asp:Label>
                    <div class="progress">
                        <div id="barraItens" runat="server" class="progress-bar progress-bar-warning progress-bar-striped active"
                            role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                        </div>
                    </div>
                    <div class="panelItem">
                        <asp:Button ID="btnCancelaProcesso" runat="server" Text="Cancelar Processamento"
                            OnClick="btnCancelaProcesso_Click" Height="35px" />
                    </div>
                </div>
                <asp:Timer ID="TimerProcessa" runat="server" OnTick="TimerProcessa_Tick" Enabled="false"
                    Interval="30000">
                </asp:Timer>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnGerarBlocos" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <asp:Panel ID="pnConfima" runat="server" CssClass="frameModal" Style="display: none">
        <center>
        <h3>
        <asp:Label ID="Label1" runat="server" Text="Atenção todos os Arquivos do mês selecionado serão excluidos! <br>Confirma a exclusão " ></asp:Label>
        </h3>
        </center>
        <hr />
        <br />
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExclusao_Click" />
                    <asp:Label ID="Label2" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaExclusao" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaExclusao_Click" />
                    <asp:Label ID="Label3" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalConfirma" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfima" TargetControlID="Label1">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirmaCancela" runat="server" CssClass="frameModal" Style="display: none">
        <center>
        <h3>
        <asp:Label ID="Label4" runat="server" Text="Atenção Tem certeza que gostaria de Cancelar o Processamento?" ></asp:Label>
        </h3>
        </center>
        <hr />
        <br />
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaCancelar" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaCancelar_Click" />
                    <asp:Label ID="Label5" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnNaoCancelar" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnBtnNaoCancelar_Click" />
                    <asp:Label ID="Label6" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaCancelamento" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaCancela" TargetControlID="Label4">
    </asp:ModalPopupExtender>

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
</asp:Content>
