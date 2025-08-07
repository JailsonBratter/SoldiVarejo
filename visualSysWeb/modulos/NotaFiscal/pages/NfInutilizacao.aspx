<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="NfInutilizacao.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.pages.NfInutilizacao" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Inutilização de Numero de Nota Fiscal
            </h1>
        </center>
    </div>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <br />
    <div class="frame">
        <br />
        <div class="FrameDivisaoMeioTela">
            Serie:
            <asp:TextBox ID="txtSerie" runat="server" Width="50px"></asp:TextBox>
            Numero Inicial:
            <asp:TextBox ID="txtNumeroInicial" runat="server" Width="90px"></asp:TextBox>
            Numero Final:
            <asp:TextBox ID="txtNumeroFinal" runat="server" Width="90px"></asp:TextBox>
            <br />
            <p>
                Justificativa</p>
            <asp:TextBox ID="txtJustificativa" runat="server" Width="460px" Height="100px" TextMode="MultiLine"
                MaxLength="255"></asp:TextBox>
            <br />
            <asp:Button ID="BtnInutilizar" runat="server" Text="Inutilizar" Font-Size="Larger"
                Height="53px" Width="172px" OnClick="BtnInutilizar_Click1" />
        </div>
        <div class="FrameDivisaoMeioTela">
            <p>
                NUMERAÇÃO INUTILIZADAS</p>
            <asp:GridView ID="gridItens" runat="server" ForeColor=" #333333" GridLines="Vertical"
                AutoGenerateColumns="False" CssClass="table">
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="Data" HeaderText="Data">
                        <ItemStyle Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Serie" HeaderText="Serie">
                        <ItemStyle Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="N_Inicio" HeaderText="Inicio">
                        <ItemStyle Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="N_fim" HeaderText="Fim">
                        <ItemStyle Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="protocolo" HeaderText="protocolo">
                        <ItemStyle Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Usuario" HeaderText="Usuario">
                        <ItemStyle Width="40px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Justificativa" HeaderText="Justificativa">
                        <ItemStyle Width="200px" />
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
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnConfirma" runat="server" CssClass="frameModal" Style="display: none">
                <asp:Label ID="Label14" runat="server" Text="Confirma a Inutilização da Numeração? "
                    CssClass="cabMenu"></asp:Label>
                <table class="frame">
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnConfirmaInutilizacao" runat="server" ImageUrl="~/img/confirm.png"
                                Width="25px" OnClick="btnConfirmaInutilizacao_Click" />
                            <asp:Label ID="Label15" runat="server" Text="Confirma"></asp:Label>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnCancelaInutilizacao" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="btnCancelaInutilizacao_Click" />
                            <asp:Label ID="Label16" runat="server" Text="Cancela"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:ModalPopupExtender ID="modalPnConfirma" runat="server" BackgroundCssClass="modalBackground"
                DropShadow="true" PopupControlID="pnConfirma" TargetControlID="Label14">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnResposta" runat="server" CssClass="frameModal" Style="display: none">
                <div class="frameRespota">
                    <div class="direitaFechar">
                        <asp:ImageButton ID="imgBtnFechar" runat="server" ImageUrl="~/img/cancel.png" Width="15px" />
                        <asp:Label ID="Label2" runat="server" Text="Fechar"></asp:Label>
                    </div>
                    <div class="cabMenu">
                        RESPOSTA DA SOLICITAÇÃO</div>
                    <asp:Label ID="lblRespostaInutilizacao" runat="server" Text=""></asp:Label>
                    <br />
                </div>
            </asp:Panel>
            <asp:ModalPopupExtender ID="modalResposta" runat="server" BackgroundCssClass="modalBackground"
                DropShadow="true" PopupControlID="pnResposta" TargetControlID="Label2" CancelControlID="imgBtnFechar">
            </asp:ModalPopupExtender>
            <asp:Timer ID="TimerXml" runat="server" OnTick="TimerXml_Tick" Enabled="false" Interval="30000">
            </asp:Timer>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnConfirmaInutilizacao" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
