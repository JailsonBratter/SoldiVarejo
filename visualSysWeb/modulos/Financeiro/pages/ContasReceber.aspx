<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ContasReceber.aspx.cs" Inherits="visualSysWeb.modulos.Financeiro.pages.ContasReceber" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Contas a Receber</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <div class="btnImprimirDireita">
        Limpar Filtros
        <asp:ImageButton ID="imgBtnLimpar" runat="server" OnClick="imgBtnLimpar_Click" ImageUrl="../../../img/botao-apagar.png"
            Style="width: 20px" />
    </div>
    <br />
    <div class="filter" id="filtrosPesq" runat="server" visible="true">
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        Documento</p>
                    <asp:TextBox ID="txtDocumento" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Cliente</p>
                    <asp:TextBox ID="txtcliente" runat="server" Width="54px"></asp:TextBox>
                    <asp:TextBox ID="txtNomeCliente" runat="server" Width="114px"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnCliente" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </td>
                 <td>
                    <p>
                        CNPJ</p>
                    <asp:TextBox ID="txtCNPJ" runat="server" Width="150px"></asp:TextBox>
                   
                </td>
                <td>
                    <p>
                        Valor</p>
                    <asp:TextBox ID="txtValor" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        De</p>
                    <asp:TextBox ID="txtDe" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="txtDataDe_TextChanged"> </asp:TextBox>
                    <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDe">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Ate</p>
                    <asp:TextBox ID="txtAte" runat="server" Width="80px"> </asp:TextBox>
                    <asp:ImageButton ID="ImgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataAte" runat="server" PopupButtonID="imgDtAte" TargetControlID="txtAte">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Pesquisar Por</p>
                    <asp:DropDownList ID="DllTipoPesquisa" runat="server">
                        <asp:ListItem Value="EMISSAO">EMISSAO</asp:ListItem>
                        <asp:ListItem Value="VENCIMENTO">VENCIMENTO</asp:ListItem>
                        <asp:ListItem Value="PAGAMENTO">PAGAMENTO</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <p>
                        Status</p>
                    <asp:DropDownList ID="DllStatus" runat="server">
                        <asp:ListItem Value="">PREVISTO</asp:ListItem>
                        <asp:ListItem Value="1" Selected="True">ABERTO</asp:ListItem>
                        <asp:ListItem Value="2">CONCLUIDO</asp:ListItem>
                        <asp:ListItem Value="3">CANCELADO</asp:ListItem>
                        <asp:ListItem Value="7">SUSPENSO</asp:ListItem>
                    </asp:DropDownList>
                </td>
               
            </tr>
            <tr>
                 <td>
                    <p>
                        Centro de Custo</p>
                    <asp:TextBox ID="txtCentroCusto" runat="server" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="imgBtnCentroCusto" runat="server" ImageUrl="~/img/pesquisaM.png"
                        Height="15px" OnClick="Img_Click" />
                </td>
                <td colspan="7">
                    <p>
                        Cartao</p>
                    <asp:DropDownList ID="ddlTipoCartao" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div class="panelItem" style="margin-top: 10px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="panelItem" style="margin-top: 10px; float: right; margin-right: 40px;">
        <h1>TOTAL:<asp:Label ID="lblTotalValores" runat="server" Text="R$ 100,00"></asp:Label></h1>
        <h1>SELEC :
                <asp:Label ID="lblSelecionado" runat="server" Text="R$ 0,00"></asp:Label></h1>
    </div>
    <br />
    <div class="gridTable">
        <div class="row">
            <div id="divTitulo" class="titulobtn" style="margin-left: 30px;">
                <asp:Button ID="BtnBaixar" runat="server" Text="Baixar" OnClick="BtnBaixar_Click" Width="100px" Height="30px" Font-Size="Larger" />
            </div>
            <div id="divCancelaBaixa" class="titulobtn">
                <asp:Button ID="BtnCancelarBaixa" runat="server" Text="Cancelar Baixa" OnClick="BtnBaixar_Click" Width="150px" Height="30px" Font-Size="Larger" />
            </div>
           
            <div class="direitaFechar" style="margin-right: 30px;">

                <asp:Button ID="btnPagInicio" runat="server" Text="<<" Font-Size="Large" OnClick="btnPag_Click" />
                <asp:Button ID="btnPagAnterio" runat="server" Text="<" Font-Size="Large" OnClick="btnPag_Click" />
                <asp:DropDownList ID="ddlPag" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPag_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Button ID="btnPagProximo" runat="server" Text=">" Font-Size="Large" OnClick="btnPag_Click" />
                <asp:Button ID="btnPagFim" runat="server" Text=">>" Font-Size="Large" OnClick="btnPag_Click" />
            </div>

        </div>
        <div class="divGrid">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" DataKeyNames="documento,codigo_cliente,valor,emissao,vencimento,status,Nome_cliente,id_cc"
            CellPadding="5" ForeColor="#333333" GridLines="Vertical" AllowSorting="True"
            OnSorting="gridPesquisa_Sorting">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelecionaItem" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelecionaItem_CheckedChanged" />
                    </ItemTemplate>
                </asp:TemplateField>
              <asp:HyperLinkField DataTextField="Documento" Text="---" Visible="true" HeaderText="Documento"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/ContasReceberDetalhes.aspx?campoIndex={0}&codCliente={1}&emissao={2}"
                    DataNavigateUrlFields="Documento,codigo_cliente,Emissao" SortExpression="Documento" />
                <asp:HyperLinkField DataTextField="Codigo_Cliente" Text="---" Visible="true" HeaderText="Codigo Cliente"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/ContasReceberDetalhes.aspx?campoIndex={0}&codCliente={1}&emissao={2}"
                    DataNavigateUrlFields="Documento,codigo_cliente,Emissao" SortExpression="Codigo_cliente" />
                <asp:HyperLinkField DataTextField="Nome_cliente" Text="---" Visible="true" HeaderText="Cliente"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/ContasReceberDetalhes.aspx?campoIndex={0}&codCliente={1}&emissao={2}"
                    DataNavigateUrlFields="Documento,codigo_cliente,Emissao" SortExpression="Nome_cliente" />
                <asp:HyperLinkField DataTextField="Valor" Text="---" Visible="true" HeaderText="Valor"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/ContasReceberDetalhes.aspx?campoIndex={0}&codCliente={1}&emissao={2}"
                    DataNavigateUrlFields="Documento,codigo_cliente,Emissao" SortExpression="Valor" DataTextFormatString="{0:n}"
                    ItemStyle-HorizontalAlign="Right" />
                <asp:HyperLinkField DataTextField="Emissao" Text="---" Visible="true" HeaderText="DT Emissao"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/ContasReceberDetalhes.aspx?campoIndex={0}&codCliente={1}&emissao={2}"
                    DataNavigateUrlFields="Documento,codigo_cliente,Emissao" SortExpression="emissao" />
                <asp:HyperLinkField DataTextField="Vencimento" Text="---" Visible="true" HeaderText="DT Vencimento"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/ContasReceberDetalhes.aspx?campoIndex={0}&codCliente={1}&emissao={2}"
                    DataNavigateUrlFields="Documento,codigo_cliente,Emissao" SortExpression="vencimento" />
                <asp:HyperLinkField DataTextField="Entrada" Text="---" Visible="true" HeaderText="DT Entrada"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/ContasReceberDetalhes.aspx?campoIndex={0}&codCliente={1}&emissao={2}"
                    DataNavigateUrlFields="Documento,codigo_cliente,Emissao" SortExpression="entrada" />
                <asp:HyperLinkField DataTextField="Pagamento" Text="---" Visible="true" HeaderText="DT Pagamento"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/ContasReceberDetalhes.aspx?campoIndex={0}&codCliente={1}&emissao={2}"
                    DataNavigateUrlFields="Documento,codigo_cliente,Emissao" SortExpression="pagamento" />
                <asp:HyperLinkField DataTextField="status" Text="---" Visible="true" HeaderText="Status"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/ContasReceberDetalhes.aspx?campoIndex={0}&codCliente={1}&emissao={2}"
                    DataNavigateUrlFields="Documento,codigo_cliente,Emissao" SortExpression="status" />
                <asp:HyperLinkField DataTextField="codigo_centro_custo" Text="---" HeaderText="Centro Custo" Visible="true" HeaderStyle-HorizontalAlign="Center"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/ContasReceberDetalhes.aspx?campoIndex={0}&codCliente={1}&emissao={2}"
                    DataNavigateUrlFields="Documento,codigo_cliente,Emissao" SortExpression="codigo_centro_custo" />
                <asp:HyperLinkField DataTextField="id_cartao" Text="---" Visible="true" HeaderText="Forma PG" HeaderStyle-HorizontalAlign="Center"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/ContasReceberDetalhes.aspx?campoIndex={0}&codCliente={1}&emissao={2}"
                    DataNavigateUrlFields="Documento,codigo_cliente,Emissao" SortExpression="cartao.id_cartao" />
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
        <div class="direitaFechar" style="margin-right: 30px;">
            <br />
            <asp:Button ID="btnPagInicio1" runat="server" Text="<<" Font-Size="Large" OnClick="btnPag_Click" />
            <asp:Button ID="btnPagAnterio1" runat="server" Text="<" Font-Size="Large" OnClick="btnPag_Click" />
            <asp:DropDownList ID="ddlPag1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPag_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:Button ID="btnPagProximo1" runat="server" Text=">" Font-Size="Large" OnClick="btnPag_Click" />
            <asp:Button ID="btnPagFim1" runat="server" Text=">>" Font-Size="Large" OnClick="btnPag_Click" />
        </div>
        <br />
        <br />
            <br />
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
                    <asp:Label ID="Label1" runat="server" Text="Confirma"></asp:Label>
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
    <asp:Panel ID="PnGrid" runat="server" CssClass="modalFormCentro" Style="display: none;
        width: 1000px;">
        <div style="background: #FFFFFF; width: 98%; margin: auto auto auto auto;">
            <asp:Label ID="lblGridtitulo" runat="server" Text="Confirma Baixa" CssClass="cabMenu"></asp:Label>
            <asp:Label ID="lblErroGrid" runat="server" Text="" ForeColor="Red" CssClass="cabMenu"
                Visible="false"></asp:Label>
            <table class="frame">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaGrid" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaGrid_Click" />
                        <asp:Label ID="Label2" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaGrid" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                            OnClick="btnCancelaGrid_Click" />
                        <asp:Label ID="Label3" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <div class="row" style="width: 20%;  margin-left: 40px; margin-right:60px; margin-bottom:0px; margin-top:0px; float:left;">
                Conta Corrente:
                <asp:DropDownList ID="ddlContaCorrenteTodas" AutoPostBack="true"  
                    runat="server" 
                    onselectedindexchanged="ddlContaCorrenteTodas_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label ID="lblBanco" runat="server" Text=""  ></asp:Label>
                <br />
                Data Pagamento<br />
                <asp:TextBox ID="txtDtpgTodas" runat="server" Width="80px" AutoPostBack="true" 
                    ontextchanged="txtDtpgTodas_TextChanged"   > </asp:TextBox>
                    <asp:ImageButton ID="imgDtPgTodas" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtPgTodas" TargetControlID="txtDtpgTodas">
                    </asp:CalendarExtender>

            </div>
            <div class="row" style="width: 40%; margin-left: 0px; margin-bottom:10px; float:left;">
                <p>Observação:</p>
               <asp:TextBox ID="txtObsBaixa" runat="server" Width="500px" Height="40px" TextMode="MultiLine"></asp:TextBox>
            </div>
            <asp:Panel ID="Panel3" runat="server" CssClass="lista">
                <asp:GridView ID="GridBaixas" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataKeyNames="documento,cliente,valor,vencimento,emissao" ForeColor="#333333" GridLines="None"
                    OnRowDataBound="GridBaixas_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
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
                    <Columns>
                        <asp:BoundField DataField="documento" HeaderText="Documento" />
                        <asp:BoundField DataField="cliente" HeaderText="Cod" />
                        <asp:BoundField DataField="NomeCliente" HeaderText="Cliente" />
                        <asp:BoundField DataField="Valor" HeaderText="Valor" />
                        <asp:BoundField DataField="vencimento" HeaderText="Vencimento" />
                        <asp:TemplateField HeaderText="Valor Pago" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtValorPago" runat="server" Width="70px" CssClass="numero"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Pagamento" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDataPago" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="ImgDtPg" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                                <asp:CalendarExtender ID="clnDataPg" runat="server" PopupButtonID="ImgDtPg" TargetControlID="txtDataPago">
                                </asp:CalendarExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Conta Corrente" HeaderStyle-HorizontalAlign="Center">
                            
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlContaCorrente" runat="server" Width="100px">
                                </asp:DropDownList>
                                <asp:Label ID="lblConta" runat="server" Text='<%# Eval("id_cc") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalGrid" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="PnGrid" TargetControlID="Label2">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfima" runat="server" CssClass="frameModal" Style="display: none">
        <center>
            <asp:Label ID="lblConfirmaTitulo" runat="server" Text="Foi informada uma Data de pagamento menor que a Data de emissão do titulo <br /> Gostaria de Salvar? "
                CssClass="titulobtn"></asp:Label>
        </center>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaData" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaData_Click" />
                    <asp:Label ID="lblConfirma" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaData" runat="server" ImageUrl="~/img/cancel.png" Width="25px"
                        OnClick="btnCancelaData_Click" />
                    <asp:Label ID="lblCcancela" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalConfirma" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfima" TargetControlID="lblConfirmaTitulo">
    </asp:ModalPopupExtender>
</asp:Content>
