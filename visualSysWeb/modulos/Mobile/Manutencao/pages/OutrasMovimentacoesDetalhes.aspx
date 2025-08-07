<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OutrasMovimentacoesDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Manutencao.pages.OutrasMovimentacoesDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Outras Movimentações Detalhes</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <table>
            <tr>
                <td>
                    <p>
                        Codigo</p>
                    <asp:TextBox ID="txtCodigo_inventario" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Tipo de Mov</p>
                    <asp:TextBox ID="txtTipoMovimentacao" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    <asp:ImageButton ID="BtnImgTipoMovimentacao" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" Width="15px" OnClick="img_Click" />
                </td>
                <td>
                    <p>
                        Descricao Inventario</p>
                    <asp:TextBox ID="txtDescricao_inventario" runat="server" Width="250px" OnChange="javascript:this.value = this.value.toUpperCase();"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Data</p>
                    <asp:TextBox ID="txtData" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Usuario</p>
                    <asp:TextBox ID="txtUsuario" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Status</p>
                    <asp:TextBox ID="txtstatus" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:ImageButton ID="ImgBtnImprimir" runat="server" Height="33px" ImageUrl="~/modulos/Relatorios/imgs/relatorio.gif"
                        OnClick="ImgBtnImprimir_Click" Width="34px" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Button ID="btnEncerrar" runat="server" Text="Encerrar" OnClick="btnEncerrar_Click" />
                    <asp:Button ID="btnImprimir" runat="server" OnClick="btnImprimir_Click" Text="Imprimir"
                        Width="75px" />
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                <HeaderTemplate>
                    Itens</HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="addItens" runat="server" CssClass="titulobtn" DefaultButton="imgPlu">
                        <table>
                            <tr>
                                <td>
                                    <p>
                                        PLU/EAN</p>
                                    <asp:TextBox ID="txtPlu" runat="server" Width="150px"></asp:TextBox><asp:ImageButton
                                        ID="imgPlu" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px" Width="15px"
                                        OnClick="imgPlu_Click" />
                                </td>
                                <td>
                                    <p>
                                        Descrição</p>
                                    <asp:TextBox ID="txtDescricao" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                </td>
                                <td>
                                    <p>
                                        SaldoAtual</p>
                                    <asp:TextBox ID="TxtSaldoAtual" runat="server" Width="80px" Enabled="False"></asp:TextBox>
                                </td>
                                <td>
                                    <p>
                                        Vlr Unit</p>
                                    <asp:TextBox ID="txtCusto" runat="server" Width="80px"></asp:TextBox>
                                </td>
                                <td>
                                    <p>
                                        <asp:Label ID="lblContado" runat="server" Text="Contado"></asp:Label></p>
                                    <asp:TextBox ID="txtContado" runat="server" Width="80px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:ImageButton ID="ImgBtnAddItens" runat="server" ImageUrl="~/img/add.png" Height="20px"
                                        Width="20px" OnClick="ImgBtnAddItens_Click" />Incluir item
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <div class="gridTable">
                        <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Vertical"
                            AutoGenerateColumns="False" OnRowDataBound="gridItens_RowDataBound" OnRowCommand="gridItens_RowCommand"
                            CssClass="table">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/edit.png" CommandName="Alterar"
                                    Text="Alterar">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="PLU" HeaderText="PLU"></asp:BoundField>
                                <asp:BoundField DataField="Descricao" HeaderText="Descrição"></asp:BoundField>
                                <asp:BoundField DataField="Saldo_atual" HeaderText="Saldo Atual"></asp:BoundField>
                                <asp:BoundField DataField="Custo" HeaderText="Vlr Unit"></asp:BoundField>
                                <asp:BoundField DataField="Contada" HeaderText="Contado"></asp:BoundField>
                                <asp:BoundField DataField="Qtde" HeaderText="Qtde"></asp:BoundField>
                                <asp:BoundField DataField="Diferenca" HeaderText="Diferenca"></asp:BoundField>
                                <asp:BoundField DataField="Total" HeaderText="Total"></asp:BoundField>
                                <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/Cancel.png" CommandName="Excluir"
                                    Text="Alterar">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
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
            <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel2">
                <HeaderTemplate>
                    Coletor</HeaderTemplate>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:RadioButtonList ID="RdoTipoDeArquivo" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="True" OnSelectedIndexChanged="RdoTipoDeArquivo_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Selected="True">Tamanho Fixo</asp:ListItem>
                                    <asp:ListItem Value="1">Caracter Delimitador</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td rowspan="5">
                                <p>
                                    Arquivo a Ser importado</p>
                                <asp:AjaxFileUpload ID="AjaxFileUpload1" runat="server" Width="500px" MaximumNumberOfFiles="1"
                                    OnUploadComplete="AjaxFileUpload1_UploadComplete" AllowedFileTypes="" />
                            </td>
                            </td></tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDelimitador" runat="server" Text="Delimitador" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDemilitador" runat="server" Width="20px" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PLU/EAN:
                            </td>
                            <td>
                                <asp:Label ID="lblInicioPlu" runat="server" Text="Inicio"></asp:Label><asp:TextBox
                                    ID="txtinicioPlu" runat="server" Width="30px"></asp:TextBox><asp:Label ID="lblFimPlu"
                                        runat="server" Text="Fim"></asp:Label><asp:TextBox ID="txtFimPlu" runat="server"
                                            Width="30px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Contado
                            </td>
                            <td>
                                <asp:Label ID="lblInicioContado" runat="server" Text="Inicio"></asp:Label><asp:TextBox
                                    ID="txtInicioContado" runat="server" Width="30px"></asp:TextBox><asp:Label ID="lblFimContado"
                                        runat="server" Text="Fim"></asp:Label><asp:TextBox ID="txtFimContado" runat="server"
                                            Width="30px"></asp:TextBox>Casas Decimais<asp:TextBox ID="txtDecimaisContado" runat="server"
                                                Width="30px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnImportarArquivo" runat="server" Text="Importar Arquivo Carregado"
                                    Height="50px" OnClick="btnImportarArquivo_Click" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </div>
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none"
        DefaultButton="ImgPesquisaLista">
        <asp:Panel ID="Panel3" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista">
            <div class="cabMenu">
                <h1>
                    <asp:Label ID="lbltituloLista" runat="server" Text=""></asp:Label>
                </h1>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaLista_Click" />
                        <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaLista_Click" />
                        <asp:Label ID="Label2" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            Filtrar
            <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="600px" CssClass="SEM"></asp:TextBox>
            <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                Height="15px" OnClick="ImgPesquisaLista_Click" />
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
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
    <asp:ModalPopupExtender ID="modalLista" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="Label2" DropShadow="true" PopupControlID="pnfundo" TargetControlID="lbltituloLista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnEncerrar" runat="server" CssClass="frameModal" Style="display: none">
        <h3>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Encerrar A Movimentacao?"
                CssClass="cabMenu"></asp:Label>
        </h3>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmarEncerrar" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaEncerrar_Click" />
                    <asp:Label ID="Label15" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelarInativar" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelarEncerrar_Click" />
                    <asp:Label ID="Label16" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalEncerrar" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnEncerrar" TargetControlID="Label14">
    </asp:ModalPopupExtender>
</asp:Content>
