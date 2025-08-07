<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CargaPDV.aspx.cs" Inherits="visualSysWeb.modulos.Dispositivos.pages.CargaPDV" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1 style="margin-left: 40px">
            Carga PDV</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div style="position: relative; margin-top: 100px; width:100%; overflow: auto">

        <div style="display: flex; justify-content: flex-start;margin-bottom:20px" >

            <div class="cargaPDVDireita" >
                <asp:Image ID="imgPDV" runat="server" ImageUrl="~/modulos/Dispositivos/imgs/PDV0.jpg"
                    Style="margin-bottom: 10px; margin-top: 0px;" Width="150px" />
                <div class="cargaPDVDireita" style="width: 70%;">
                    Tipo de PDV
                        <asp:RadioButtonList ID="rdoPDV" runat="server" AutoPostBack="True" Height="25px"
                            OnSelectedIndexChanged="rdoPDV_SelectedIndexChanged">
                            <asp:ListItem Value="0" Selected="True">Soldi PDV</asp:ListItem>
                            <asp:ListItem Value="1">Zanthus</asp:ListItem>
                            <asp:ListItem Value="2">Busca Preço</asp:ListItem>
                        </asp:RadioButtonList>
                </div>

            </div>
            <asp:Panel ID="pnPDV" runat="server" CssClass="cargaPDVDireita" Style="max-height: 500px;">
                <div class="panel" id="divTipoCarga" runat="server" style="border-style: solid; border-width: 1px; width: 90%;">
                    <span class="cabMenu" style="margin-bottom: 0px; font-size: medium;">Tipo de Carga</span>
                    <asp:Panel ID="pnTipoZanthus" runat="server">
                        <asp:RadioButtonList ID="rdoZanthusTipoArquivo" runat="server" AutoPostBack="True" Width="100%" Font-Size="medium"
                            Height="25px" OnSelectedIndexChanged="rdoZanthusTipoArquivo_SelectedIndexChanged"
                            RepeatDirection="Horizontal">
                            <asp:ListItem Value="0" Selected="True">Mercadoria</asp:ListItem>
                            <asp:ListItem Value="1">Clientes</asp:ListItem>
                        </asp:RadioButtonList>
                    </asp:Panel>

                    <asp:CheckBox ID="chkAlterados" runat="server" Text="Somente os Alterados" Font-Size="medium"
                        Checked="true" AutoPostBack="true" OnCheckedChanged="chkAlterados_CheckedChanged" />
                    <br />
                    <asp:CheckBox ID="chkcargaTotal" runat="server" Text="Carga Total" Checked="false"
                        Font-Size="medium" AutoPostBack="true" OnCheckedChanged="chkTotal_CheckedChanged" />
                    <br />
                </div>
                <div class="panel" id="divOpcoes" runat="server" style="border-style: solid; border-width: 1px; width: 90%; margin-top: 20px;">
                    <span class="cabMenu" style="margin-bottom: 0px; font-size: medium;">Outras Opções</span>
                    <asp:CheckBox ID="chkDesmarcarAlteracoes" runat="server" Text="Desmarcar Alterados"
                        Font-Size="medium" Checked="false" />
                    <br />
                    <asp:CheckBox ID="chkGeraBuscaPreco" runat="server" Text="Gerar Busca Preço" Checked="false"
                        Font-Size="medium" Enabled="false" />
                    <br />


                    <asp:CheckBox ID="chkOperadores" runat="server" Text="Carga de operadores"
                        Font-Size="medium" Checked="false" />
                    <br />

                    <asp:CheckBox ID="chkGerarComCPF" runat="server" Text="Gerar CPF como codigo do cliente"
                        Font-Size="medium"
                        Checked="false" Visible="false" />

                </div>
                <br />
            </asp:Panel>
            <div id="divPdvsCadastrados" runat="server" class="cargaPDVDireita" style="margin-top:25px" >
                    <asp:GridView class="gridTable" ID="gridPesquisa" runat="server" AutoGenerateColumns="False"
                        GridLines="Vertical" CellPadding="5" ForeColor="#333333">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" Checked="true" OnCheckedChanged="chkSeleciona_CheckedChanged" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelecionaItem" runat="server" AutoPostBack="True" Checked="true" OnCheckedChanged="chkSelecionaItem_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Caixa" HeaderText="PDV" />
                            <asp:BoundField DataField="Diretorio_Carga" HeaderText="Destino" ItemStyle-Width="200px" />
                            <asp:BoundField DataField="Data_ult_atualizacao" HeaderText="Ult Carga" />

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
            <div class="cargaPDVDireita" style="padding:0px 10px;">
                <asp:ImageButton ImageUrl="../../../img/Refresh.png" runat="server" Width="20px" OnClick="ImgBtnAtulizaHistorico_Click" />Atualizar
                <asp:GridView class="gridTable" ID="gridHistoricoCargas" runat="server" AutoGenerateColumns="False"
                    GridLines="Vertical" CellPadding="5" ForeColor="#333333">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="id_carga" HeaderText="CARGA" />
                        <asp:BoundField DataField="caixas" HeaderText="Caixas" HtmlEncode="false" />
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
            <div id="divArquivos" runat="server" class="cargaPDVDireita">
                <p>
                    Arquivos pendentes de processamento
                </p>
                <asp:Button ID="btnReenviar" runat="server" Text="Reenviar" Width="80px" OnClick="btnReenviar_Click" />
                <asp:Button ID="btnLimpar" runat="server" Text="Desprezar" Width="80px" OnClick="btnLimpar_Click" />
                <asp:ListBox ID="ltbArquivosPendentes" runat="server" Height="246px" Width="212px"
                    Enabled="False"></asp:ListBox>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnConfirma" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px;">
            <h2>
                <asp:Label ID="Label7" runat="server" Text="Você selecionou a geração de carga total!<br> isso pode gerar um GRANDE VOLUME DE DADOS na rede, <BR> Tem Certeza que gostaria de prosseguir com a operação ?"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center;">
                        <asp:ImageButton ID="btnConfirmaGerarTotal" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaGerarTotal_Click" />
                        <asp:Label ID="Label12" runat="server" Text="Sim"></asp:Label>
                    </div>
                </td>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center;">
                        <asp:ImageButton ID="btnCancelaGerarTotal" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaGerarTotal_Click" />
                        <asp:Label ID="Label13" runat="server" Text="Cancela"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalPnConfirma" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirma" TargetControlID="Label7">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 50px; margin-bottom: 20px;">
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

    <asp:Panel ID="PnAguarde" runat="server" CssClass="frameModal" Style="display: none">
        <div class="frame">
            <h1>
                <asp:Label ID="lblDetalhesAguarde" runat="server"></asp:Label></h1>
            <br />
            <center><img src="../../../img/aguarde.gif" /></center>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalaguarde" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnAguarde" TargetControlID="lblDetalhesAguarde">
    </asp:ModalPopupExtender>

    <asp:Timer ID="TimerCarga" runat="server" OnTick="TimerCarga_Tick" Enabled="false"
        Interval="1000">
    </asp:Timer>
</asp:Content>
