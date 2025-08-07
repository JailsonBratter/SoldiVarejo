<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocumentosEletronicos.aspx.cs" Inherits="visualSysWeb.modulos.Sped.pages.DocumentosEletronicos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>Documentos Eletronicos</h1>
    </center>

    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        <div class="btnImprimirDireita">
            Limpar Filtros
            <asp:ImageButton ID="imgBtnLimpar" runat="server" OnClick="imgBtnLimpar_Click" ImageUrl="../../../img/botao-apagar.png"
                Style="width: 20px" />
        </div>
       
        <div class="container row" style="margin-left:25px">
            <div class="panelItem">
                <p>
                    De
                </p>
                <asp:TextBox ID="txtDataDe" runat="server" AutoPostBack="true" Width="80px"></asp:TextBox>
                <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDataDe">
                </asp:CalendarExtender>
            </div>
            <div class="panelItem">
                <p>
                    Ate
                </p>
                <asp:TextBox ID="txtDataAte" runat="server" Width="80px"></asp:TextBox>
                <asp:ImageButton ID="imgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtAte"
                    TargetControlID="txtDataAte">
                </asp:CalendarExtender>
            </div>
            <div class="panelItem">
                <p>
                    Nro Cupom
                </p>
                <asp:TextBox ID="txtDocumento" runat="server" Width="80px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    Nro Extrato
                </p>
                <asp:TextBox ID="txtExtrato" runat="server" Width="100px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    PDV
                </p>
                <asp:TextBox ID="txtPDV" runat="server" Width="80px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    Nro Serie Equipamento
                </p>
                <asp:TextBox ID="txtSerieEquipamento" runat="server" Width="100px"></asp:TextBox>
            </div>
            <div class="panelItem">
                <p>
                    Tipo</p>
                <asp:DropDownList ID="DllTipo" runat="server" CssClass="sem">
                    <asp:ListItem Value="1">Cupom CFe</asp:ListItem>
                    <asp:ListItem Value="2">NFe Saída</asp:ListItem>
                    <asp:ListItem Value="3">NFe Entrada</asp:ListItem>
                </asp:DropDownList>
            </div>
            
        </div>
    </div>
    <div class="row">
        <div class="panelItem" style="margin-left:25px">
            <h1>
                <asp:Label ID="lblQtdRegistros" runat="server" Text=""></asp:Label>
                Documentos Encontrados
            </h1>
        </div>
        <div class="btnImprimirDireita" style="margin-bottom:10px;  margin-right:50px;">
            <asp:Button ID="btnBaixarArquivos" runat="server" Text="Download Arquivos"
                OnClick="btnBaixarArquivos_Click" CssClass="submitButton" Height="40px" />
        </div>
    </div>


    <div class="gridTable">
        <asp:GridView ID="gridDocumentos" runat="server" AutoGenerateColumns="False" CellPadding="5"
            ForeColor="#333333" GridLines="Vertical" AllowSorting="true" >
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="Documento" HeaderText="Numero Documento"></asp:BoundField>
                <asp:BoundField DataField="Nro_extrato_Sat" HeaderText="Nro Doc Chave"></asp:BoundField>
                <asp:BoundField DataField="Caixa" HeaderText="PDV"></asp:BoundField>
                <asp:BoundField DataField="DataStr" HeaderText="Data" ></asp:BoundField>
                <asp:BoundField DataField="Nro_Serie_Equipamento" HeaderText="Nro Serie Equipamento"></asp:BoundField>
                <asp:BoundField DataField="ID_Chave" HeaderText="Chave"></asp:BoundField>
                <asp:BoundField DataField="ID_Chave_Cancelamento" HeaderText="Chave Cancelado"></asp:BoundField>
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
        <br />
    </div>
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
    <asp:Panel ID="PnGerar" runat="server" CssClass="frameModal" Style="display: none; width: 500px;">
        <div class="cabMenu" style="height: 70px;">
            <h3>
                <asp:Label ID="lblMsgXml" runat="server" Text="Aguarde ! "></asp:Label>


            </h3>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 50px; margin-bottom: 20px;">
                        <asp:Button ID="btnFecharXml" runat="server" Text="OK" Width="200px" Height="100%"
                            Font-Size="Larger" OnClick="btnFecharXml_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalGerar" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnGerar" TargetControlID="lblMsgXml">
    </asp:ModalPopupExtender>

    <asp:Timer ID="TimerGerar" runat="server" OnTick="TimerGerar_Tick" Enabled="false" Interval="30000">
    </asp:Timer>

</asp:Content>
