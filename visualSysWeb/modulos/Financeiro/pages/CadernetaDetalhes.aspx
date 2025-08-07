<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CadernetaDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Financeiro.pages.CadernetaDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes da caderneta</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <!--Coloque aqui os campos do cabe?alho    -->
        <asp:Panel ID="Panel1" runat="server" DefaultButton="imgBtnFiltrar">
            <div class="btnImprimirDireita">
                <asp:ImageButton ID="imgBtnImpressao" runat="server" ImageUrl="../../../img/icon_imprimir.gif"
                    OnClick="imgBtnImpressao_Click" />
            </div>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo Cliente</p>
                        <asp:TextBox ID="txtCodigo_Cliente" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            CNPJ / CPF
                        </p>
                        <asp:TextBox ID="txtCnpj" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Nome</p>
                        <asp:TextBox ID="txtNomeCliente" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Situação</p>
                        <asp:TextBox ID="txtSituacao" runat="server" Width="80px" Enabled="false" Style="font-size: 20px; "></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Limite</p>
                        <asp:TextBox ID="txtLimite" runat="server" Width="80px" Enabled="false" Style="font-size: 20px; text-align: right;"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            <asp:LinkButton ID="lnkReceber" runat="server" OnClick="lnkReceber_Click">Conta a Receber</asp:LinkButton>
                           </p>
                        <asp:TextBox ID="txtUtilizadoContaReceber" runat="server" Width="80px" Enabled="false" Style="font-size: 20px; text-align: right;"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Cardeneta</p>
                    <asp:TextBox ID="txtTotalCardeneta" runat="server" Width="80px" Enabled="false" Style="font-size: 20px; text-align: right;"></asp:TextBox>    
                    </td>
                    <td>
                        <p>
                            
                            Utilizado</p>
                        <asp:TextBox ID="txtUtilizado" runat="server" Width="80px" Enabled="false" Style="font-size: 20px; text-align: right;"></asp:TextBox>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <p>
                            De</p>
                        <asp:TextBox ID="txtDe" runat="server" CssClass="DATA" Width="80px" AutoPostBack="True"
                            OnTextChanged="txtDe_TextChanged"></asp:TextBox>
                        <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDe">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <p>
                            Ate</p>
                        <asp:TextBox ID="txtAte" runat="server" CssClass="DATA" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="ImgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="clnDataAte" runat="server" PopupButtonID="imgDtAte" TargetControlID="txtAte">
                        </asp:CalendarExtender>
                    </td>
                    <td colspan="4">
                        <p>
                            TIPO</p>
                        <asp:DropDownList ID="ddlTipoFiltro" runat="server">
                            <asp:ListItem Value="">---</asp:ListItem>
                            <asp:ListItem Value="DEBITO">DEBITO</asp:ListItem>
                            <asp:ListItem Value="CREDITO">CREDITO</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="imgBtnFiltrar" ImageUrl="~/img/pesquisaM.png"
                            runat="server" Height="15px" OnClick="imgBtnFiltrar_Click" />
                        Filtrar
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <div id="conteudo" runat="server" class="conteudo">
        <asp:Panel ID="addItens" runat="server" CssClass="titulobtn">
            <asp:ImageButton ID="ImgBtnAddItens" runat="server" ImageUrl="~/img/add.png" Height="20px"
                Width="20px" OnClick="ImgBtnAddItens_Click" />
            Incluir item
        </asp:Panel>
        <br />
        <asp:GridView ID="gridCaderneta" runat="server" AutoGenerateColumns="False" CellPadding="5"
            ForeColor="#333333" GridLines="Vertical" OnRowCommand="gridCaderneta_RowCommand">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="Emissao_caderneta" HeaderText="Emissao"></asp:BoundField>
                <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                <asp:BoundField DataField="Documento_Caderneta" HeaderText="Documento"></asp:BoundField>
                <asp:BoundField DataField="Historico_Caderneta" HeaderText="Historico"></asp:BoundField>
                <asp:BoundField DataField="Total_Caderneta" HeaderText="Total"></asp:BoundField>
                <asp:BoundField DataField="Caixa_Caderneta" HeaderText="Caixa"></asp:BoundField>
                <asp:BoundField DataField="lancamento" HeaderText="Lancamento"></asp:BoundField>
                <asp:BoundField DataField="usuario" HeaderText="Usuario"></asp:BoundField>
                <asp:BoundField DataField="data_inclusao" HeaderText="Incluido"></asp:BoundField>
                
                
                <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" CommandName="Alterar"
                    Text="Excluir">
                    <ControlStyle Height="20px" Width="20px" />
                    <ItemStyle Width="20px" />
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
        <asp:Panel ID="pnItens" runat="server" CssClass="modalForm" Style="display: none" Height="200px">
            <asp:Panel ID="pnItensFrame" runat="server" CssClass="frame">
                <div class="cabMenu">
                    <h1>
                        Dados do Item</h1>
                    <asp:Label ID="lblErrorItens" runat="server" Text=""></asp:Label>
                </div>
                <table class="cabMenu">
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnConfirmaItens" runat="server" ImageUrl="~/img/confirm.png"
                                Width="25px" OnClick="btnConfirmaItens_Click" />
                            <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnCancelaItem" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                                OnClick="btnCancelaItem_Click" />
                            <asp:Label ID="Label2" runat="server" Text="Cancela"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <p>
                                Emissao</p>
                            <asp:TextBox ID="txtEmissao" runat="server" Width="80px" CssClass="DATA"></asp:TextBox>
                            <asp:ImageButton ID="imgEmissao" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgEmissao"
                                TargetControlID="txtEmissao">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <p>
                                Tipo</p>
                            <asp:DropDownList ID="ddlTipo" runat="server">
                                <asp:ListItem Value="DEBITO">DEBITO</asp:ListItem>
                                <asp:ListItem Value="CREDITO">CREDITO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <p>
                                Documento</p>
                            <asp:TextBox ID="txtDocumento" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Historico</p>
                            <asp:TextBox ID="txtHistorico" runat="server" Width="200px" MaxLength="30"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Total</p>
                            <asp:TextBox ID="txtTotal" runat="server" CssClass="numero" Width="100px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Caixa</p>
                            <asp:TextBox ID="txtCaixa" runat="server" CssClass="numero" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Lançamento</p>
                            <asp:TextBox ID="txtLancamento" runat="server" Width="100px" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
        <asp:ModalPopupExtender ID="ModalItens" runat="server" BackgroundCssClass="modalBackground"
            CancelControlID="lblError" DropShadow="true" PopupControlID="pnItens" TargetControlID="lblError">
        </asp:ModalPopupExtender>
        <asp:Panel ID="PnConfirmaExclusao" runat="server" CssClass="modalForm" Style="display: none" Height="200px">
            <asp:Panel ID="pnExlusaoFrame" runat="server" CssClass="frame">
                <div class="cabMenu">
                    <h1>
                        TEM CERTEZA QUE GOSTARIA DE EXCLUIR O ITEM?</h1>
                    <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                </div>
                <table class="cabMenu">
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnConfirmaExcluir" runat="server" ImageUrl="~/img/confirm.png"
                                Width="25px" OnClick="btnConfirmaExcluir_Click" />
                            <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnCancelarExcluir" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="btnCancelaExcluir_Click" />
                            <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <p>
                                Emissao</p>
                            <asp:Label ID="lblEmissao" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <p>
                                Tipo</p>
                            <asp:Label ID="lblTipo" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <p>
                                Documento</p>
                            <asp:Label ID="lblDocumento" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <p>
                                Historico</p>
                            <asp:Label ID="lblHistorico" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <p>
                                Total</p>
                            <asp:Label ID="lbltotal" runat="server" Text="Cancela"></asp:Label>
                        </td>
                        <td>
                            <p>
                                Caixa</p>
                            <asp:Label ID="lblCaixa" runat="server" Text="Cancela"></asp:Label>
                        </td>
                        <td>
                            <p>
                                Lançamento</p>
                            <asp:Label ID="lblLancamento" runat="server" Text="Cancela"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
        <asp:ModalPopupExtender ID="ModalConfirmaExlusao" runat="server" BackgroundCssClass="modalBackground"
            DropShadow="true" PopupControlID="PnConfirmaExclusao" TargetControlID="Label3">
        </asp:ModalPopupExtender>
    </div>
</asp:Content>
