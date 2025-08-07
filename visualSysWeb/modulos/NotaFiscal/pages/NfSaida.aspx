<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="NfSaida.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.pages.NfSaida"
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>EMISSÃO DE NOTA FISCAL</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <div class="btnImprimirDireita">
        Limpar Filtros
        <asp:ImageButton ID="imgBtnLimpar" runat="server" OnClick="imgBtnLimpar_Click" ImageUrl="../../../img/botao-apagar.png"
            Style="width: 20px" />
    </div>
    <br />
    <div class="filter" id="filtrosPesq" runat="server" visible="True">
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        Codigo</p>
                    <asp:TextBox ID="txtCodigo" runat="server" AutoPostBack="true" Width="100px" OnTextChanged="txtCodigo_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Destinatario</p>
                    <asp:DropDownList ID="Ddlcli_fornec" runat="server">
                        <asp:ListItem Value="CLIENTE">CLIENTE</asp:ListItem>
                        <asp:ListItem Value="FORNECEDOR">FORNECEDOR</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtCliente" runat="server" AutoPostBack="true" OnTextChanged="txtCodigo_TextChanged"> </asp:TextBox>
                    <asp:ImageButton ID="imgCliente" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                        OnClick="imgCliente_Click" />
                </td>
                <td>
                    <p>
                        Pesquisar Por</p>
                    <asp:DropDownList ID="DllTipoPesquisa" runat="server">
                        <asp:ListItem Value="">---- </asp:ListItem>
                        <asp:ListItem Value="DATA">DATA</asp:ListItem>
                        <asp:ListItem Value="EMISSAO">EMISSAO</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <p>
                        De</p>
                    <asp:TextBox ID="txtDe" runat="server" Width="100px"> </asp:TextBox>
                    <asp:Image ID="ImgDeCalendario" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDeCalendario"
                        TargetControlID="txtDe">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Ate</p>
                    <asp:TextBox ID="txtAte" runat="server" Width="100px"> </asp:TextBox>
                    <asp:ImageButton ID="dt_txtAte" runat="server" ImageUrl="~/img/calendar.png" Height="15px" />
                    <asp:CalendarExtender ID="ClnDataAte" runat="server" PopupButtonID="Dt_txtAte" TargetControlID="txtAte">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Status</p>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Value="">---- </asp:ListItem>
                        <asp:ListItem Value="DIGITACAO">DIGITACAO</asp:ListItem>
                        <asp:ListItem Value="VALIDADO">VALIDADO</asp:ListItem>
                        <asp:ListItem Value="TRANSMITIDO">TRANSMITIDO</asp:ListItem>
                        <asp:ListItem Value="AUTORIZADO">AUTORIZADO</asp:ListItem>
                        <asp:ListItem Value="CANCELADA">CANCELADA</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <br />
                    <asp:CheckBox ID="chkCartaCorrecao" Text="Contem Carta de correção" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div class="panelItem" style="margin-bottom: 15px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="false" CellPadding="5"
            AllowSorting="True" OnSorting="gridPesquisa_Sorting" ForeColor="#333333" GridLines="Vertical" onrowcommand="gridPesquisa_RowCommand">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Codigo" Text="Codigo" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?codigo={0}&cliente={1}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor" SortExpression="Codigo" />
                <asp:HyperLinkField DataTextField="cliente_fornecedor" Text="cliente_fornecedor"
                    Visible="true" HeaderText="Cod" DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?codigo={0}&cliente={1}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor" SortExpression="cliente_fornecedor" />
                <asp:HyperLinkField DataTextField="Cliente" Text="---" Visible="true" HeaderText="Destinatario"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?codigo={0}&cliente={1}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor" SortExpression="Cliente" />
                <asp:HyperLinkField DataTextField="Data" Text="Data" Visible="true" HeaderText="Data"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?codigo={0}&cliente={1}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor" SortExpression="Data" />
                <asp:HyperLinkField DataTextField="Emissao" Text="Emissao" Visible="true" HeaderText="Emissao"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?codigo={0}&cliente={1}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor" SortExpression="Emissao" />
                <asp:HyperLinkField DataTextField="Total" Text="Total" Visible="true" ItemStyle-HorizontalAlign="Right"
                    HeaderText="Total" DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?codigo={0}&cliente={1}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor" SortExpression="total" />
                <asp:HyperLinkField DataTextField="Status" Text="DIGITACAO" Visible="true" HeaderText="Status"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?codigo={0}&cliente={1}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor" SortExpression="status" />
                <asp:ButtonField ButtonType="Image"  ImageUrl="../../../img/ico-doc-danfe.png" CommandName="Danfe"
                                    Text="" HeaderText="Pré     Danfe">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                 <asp:ButtonField ButtonType="Image"  ImageUrl="../../../img/email.png" CommandName="Email"
                                    Text="" HeaderText="Email">
                                    <ControlStyle Height="20px" Width="20px" />
                                </asp:ButtonField>
                <asp:HyperLinkField DataTextField="XML" Text="" Visible="true" HeaderText="XML"
                    DataNavigateUrlFormatString="~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?codigo={0}&cliente={1}"
                    DataNavigateUrlFields="Codigo,cliente_Fornecedor" SortExpression="XML" />
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
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
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
            <asp:TextBox ID="TxtPesquisaLista" runat="server"  Width="400px"
                ></asp:TextBox>
            <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                Height="15px" OnClick="ImgPesquisaLista_Click" />
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    OnRowCommand="GridLista_RowCommand" OnRowDataBound="GridLista_RowDataBound">
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
        DropShadow="true" PopupControlID="pnFundo" TargetControlID="lbltituloLista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirmaEnvioEmail" runat="server" CssClass="frameModal" Style="display: none;
        height: 180px; width: 500px;">
        <b>
            
            <center>
            <h2>
                    TEM CERTEZA  QUE GOSTARIA DE REENVIAR O EMAIL ?
            </h2>
                    <br />NOTA:  <asp:Label ID="lblCodigoNota" runat="server" Text="" ></asp:Label> 
                    DEST:  <asp:Label ID="lblDestinatario" runat="server" Text="" ></asp:Label> 
                    <asp:Label ID="lblNomeDestinatario" runat="server" Text="" ></asp:Label> 
                    <br />
                    </center>
                
            
        </b>
        <hr />
        <asp:Label ID="Label19" runat="server" Text="" ForeColor="Red"></asp:Label>
        <br />
        <div class="panelItem" style="margin-left: 20%;">
            <asp:ImageButton ID="imgBtnConfirmaEnvioEmail" runat="server" ImageUrl="~/img/confirm.png"
                Width="25px" OnClick="imgBtnConfirmaEnvioEmail_Click" />
            <asp:Label ID="Label20" runat="server" Text="Confirma"></asp:Label>
        </div>
        <div class="panelItem" style="margin-left: 20%;">
            <asp:ImageButton ID="imgBtnCancelaEnvioEmail" runat="server" ImageUrl="~/img/cancel.png"
                Width="25px" OnClick="imgBtnCancelaEnvioEmail_Click" />
            <asp:Label ID="Label21" runat="server" Text="Cancela"></asp:Label>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaEnvioEmail" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaEnvioEmail" TargetControlID="lblCodigoNota">
    </asp:ModalPopupExtender>
</asp:Content>
