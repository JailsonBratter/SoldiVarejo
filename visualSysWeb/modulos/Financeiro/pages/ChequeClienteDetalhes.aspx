<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ChequeClienteDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Financeiro.pages.ChequeClienteDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenu">
        <center>
            <h1>
                Detalhes do cheque</h1>
        </center>
    </div>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="cabecalho" runat="server" class="frame">
        <!--Coloque aqui os campos do cabe?alho    -->
        <table>
            <tr>
                <td>
                    <p>
                        Codigo Cliente</p>
                    <asp:TextBox ID="txtCodigo_Cliente" runat="server" Width="50px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Nome Cliente</p>
                    <asp:TextBox ID="txtNome_Cliente" runat="server" Width="400px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="conteudo" runat="server" class="conteudo" enableviewstate="false">
        <asp:Panel ID="addItens" runat="server" CssClass="titulobtn">
            <asp:ImageButton ID="ImgBtnAddItens" runat="server" ImageUrl="~/img/add.png" Height="20px"
                Width="20px" OnClick="ImgBtnAddItens_Click" />
            Incluir Cheque
        </asp:Panel>
        <asp:Panel ID="PnFiltros" runat="server" CssClass="titulobtn" DefaultButton="imgBtnFiltro">
            <table>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlTipoPesquisa" runat="server">
                            <asp:ListItem Value="Emissao_cheque">EMISSAO</asp:ListItem>
                            <asp:ListItem Value="deposito_cheque">DEPOSITO</asp:ListItem>
                            <asp:ListItem Value="data_cadastro">DATA</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        De
                        <asp:TextBox ID="txtDe" runat="server" Width="80px" CssClass="DATA"> </asp:TextBox>
                        <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDe">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        Ate
                        <asp:TextBox ID="txtAte" runat="server" Width="80px" CssClass="DATA"> </asp:TextBox>
                        <asp:ImageButton ID="ImgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="clnDataAte" runat="server" PopupButtonID="imgDtAte" TargetControlID="txtAte">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        Numero Cheque
                        <asp:TextBox ID="txtNumeroChequePesquisa" runat="server" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:ImageButton ID="imgBtnFiltro" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                        Filtrar
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <div class="gridTable">
            <asp:GridView ID="gridCheque" runat="server" ForeColor=" #333333" GridLines="Vertical"
                AutoGenerateColumns="False" OnRowDataBound="gridCheque_RowDataBound" OnRowCommand="gridCheque_RowCommand">
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>
                    <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/edit.png" CommandName="Alterar"
                        Text="Alterar">
                        <ControlStyle Height="20px" Width="20px" />
                        <ItemStyle Width="20px" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="emissao_Cheque" HeaderText="Emissao" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundField>
                    <asp:BoundField DataField="Deposito_cheque" HeaderText="Deposito" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundField>
                    <asp:BoundField DataField="Banco_cheque" HeaderText="Banco" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundField>
                    <asp:BoundField DataField="Agencia_cheque" HeaderText="Agencia" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundField>
                    <asp:BoundField DataField="Numero_cheque" HeaderText="Numero" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundField>
                    <asp:BoundField DataField="Total_cheque" HeaderText="Total" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundField>
                    <asp:BoundField DataField="Documento_cheque" HeaderText="Documento" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundField>
                    <asp:BoundField DataField="Compensado_Cheque" HeaderText="Compensado" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundField>
                    <asp:BoundField DataField="Devolvido_cheque" HeaderText="Devolvido" HeaderStyle-HorizontalAlign="Center">
                    </asp:BoundField>
                    <asp:BoundField DataField="Data_Cadastro" HeaderText="Data" HeaderStyle-HorizontalAlign="Center">
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
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px;">
            <center><h1><asp:Label ID="lbllista" runat="server" Text="" ></asp:Label></h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                Filtrar
                <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="400px"></asp:TextBox>
                <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="ImgPesquisaLista_Click" />
                <asp:Label ID="lblErroPesquisa" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnFechar_Click" />
                    <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="LblCancelaLista" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
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
    <asp:ModalPopupExtender ID="modalPnFundo" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnItens" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnItensFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>
                    Dados do Cheque</h1>
                <asp:Label ID="lblErroItem" runat="server" Text=""></asp:Label>
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
                        <asp:Label ID="lblCancelaItem" runat="server" Text="Cancela"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnExcluirCheque" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnExcluirCheque_Click" />
                        <asp:Label ID="lblExcluir" runat="server" Text="Excluir"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Emissao</p>
                        <asp:TextBox ID="txtEmissao" runat="server" CssClass="DATA" Width="70px"></asp:TextBox>
                        <asp:ImageButton ID="imgDtEmissao" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtEmissao"
                            TargetControlID="txtEmissao">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <p>
                            Deposito</p>
                        <asp:TextBox ID="txtDeposito" runat="server" CssClass="DATA" Width="70px"></asp:TextBox>
                        <asp:ImageButton ID="imgDtDeposito" ImageUrl="~/img/calendar.png" runat="server"
                            Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgDtDeposito"
                            TargetControlID="txtDeposito">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <p>
                            Numero Banco</p>
                        <asp:TextBox ID="txtBanco" runat="server" CssClass="numero" Width="100px" MaxLength="3"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Agencia</p>
                        <asp:TextBox ID="txtAgencia" runat="server" CssClass="numero" Width="100px" MaxLength="5"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Numero Cheque</p>
                        <asp:TextBox ID="txtNumeroCheque" runat="server" CssClass="numero" Width="80px" MaxLength="9"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            TOTAL :</p>
                        <asp:TextBox ID="TxtTotal" runat="server" CssClass="numero" Width="100px"> </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p>
                            Documento Cheque</p>
                        <asp:TextBox ID="txtDocumento" runat="server" Width="130px" MaxLength="24"></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkCompensado" Text="Compensado" runat="server" />
                    </td>
                    <td colspan="4">
                        <asp:Label ID="lblDevolvido" runat="server" Text="Devolvido" Font-Size="X-Large"
                            Visible="false"></asp:Label>
                        <asp:Button ID="BtnDevolvido" runat="server" Text="Devolver" OnClick="BtnDevolvido_Click" />
                    </td>
                </tr>
                </tr>
                <td colspan="3">
                    <p>
                        Dono do Cheque</p>
                    <asp:TextBox ID="txtResponsavelCheque" runat="server" Width="300px" MaxLength="50"></asp:TextBox>
                </td>
                <td colspan="3">
                    <p>
                        Telefone</p>
                    <asp:TextBox ID="txtResponsavelTelefone" runat="server" Width="150px" CssClass="numero"
                        MaxLength="30"></asp:TextBox>
                </td>
                </tr>
                <td colspan="6">
                    <p>
                        Observação</p>
                    <asp:TextBox ID="txtObservacao" runat="server" MaxLength="250" TextMode="MultiLine"
                        Width="500px" Height="60"></asp:TextBox>
                    Observação digitar no máximo 250 caracteres.
                </td>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalItens" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblError" DropShadow="true" PopupControlID="pnItens" TargetControlID="lblError">
    </asp:ModalPopupExtender>
</asp:Content>
