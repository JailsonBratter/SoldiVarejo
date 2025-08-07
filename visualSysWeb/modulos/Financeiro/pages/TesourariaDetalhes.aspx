<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="TesourariaDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Financeiro.pages.TesourariaDetalhes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        <!--
        .th {
            background-position: center;
            position: relative;
            top: BLOCKED EXPRESSION;
        }
        -->
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../js/TesourariaDetalhes.js" type="text/javascript"></script>
    <center><h1>Tesouraria Detalhes</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <asp:Label runat="server" ID="lblerror" Text=""></asp:Label>
    <div id="BtnEstorno" runat="server" class="direitaFechar">
        <asp:ImageButton ID="imgBtnEstorno" runat="server" ImageUrl="~/img/cancel.png" Height="25px"
            OnClick="imgBtnEstorno_Click" Visible="true" />
        Estornar
    </div>
    <asp:Panel ID="PnCabecalho" runat="server" CssClass="frame">
        <h2>
            <div class="panel" style="width: 100%; margin-top: 3px;">
                <div class="panelItem" style="margin-left: 30px;">
                    <b>Id Movimento:</b>
                    <asp:Label runat="server" ID="lblIdMovimento" Text=""></asp:Label>
                </div>

                <div class="panelItem" style="margin-left: 30px;">
                    <b>Inicio:</b>
                    <asp:Label runat="server" ID="lblInicioPeriodo" Text=""></asp:Label>
                </div>
                <div class="panelItem" style="margin-left: 30px;">
                    <b>Fim:</b>
                    <asp:Label runat="server" ID="lblFimPeriodo" Text=""></asp:Label>
                </div>
                <div class="panelItem" style="margin-left: 30px;">
                    <b>Caixa:</b>
                    <asp:Label runat="server" ID="lblcaixa" Text=""></asp:Label>
                </div>
                <div class="panelItem" style="margin-left: 30px;">
                    <b>Operador:</b>
                    <asp:Label runat="server" ID="lblOperador" Text=""></asp:Label>
                </div>
                <div id="divReprocessa" runat="server" class="panelItem" style="margin-left: 30px;">
                    <b>Atualizar Registros</b>
                    <asp:ImageButton ID="imgBtnReprocessa" runat="server" ImageUrl="~/img/Refresh.png" Width="15px" OnClick="imgBtnReprocessa_Click" />
                </div>

                <div class="panelItem" style="float: right; margin-right: 30px;">
                    <asp:Button ID="btnEncerrar" runat="server" Text="Encerrar" OnClick="btnEncerrar_Click"
                        CssClass="submitButton" Height="40px" />
                </div>
                <div id="divImprimir" runat="server" visible="false" class="panelItem" style="float: right; margin-right: 30px;">
                    <asp:ImageButton ID="imgBtnImprimir" runat="server" ImageUrl="~/img/icon_imprimir.gif"
                        Height="50px" OnClick="imgBtnImprimir_Click" />
                </div>
            </div>
        </h2>
    </asp:Panel>
    <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
        <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="tabCadastro">
            <HeaderTemplate>
                Financeiro
            </HeaderTemplate>
            <ContentTemplate>
                <asp:Panel ID="Pnfinanceiro" runat="server" CssClass="gridTable">
                    <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Vertical"
                        AutoGenerateColumns="False" CssClass="table" ShowFooter="True"
                        OnDataBound="gridItens_DataBound" AllowSorting="True"
                        OnSorting="gridItens_Sorting">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />

                        <Columns>
                            <asp:BoundField DataField="COD_FINALIZADORA" HeaderText="ID" SortExpression="1"></asp:BoundField>
                            <asp:BoundField DataField="FINALIZADORA" HeaderText="Finalizadora" SortExpression="2"></asp:BoundField>
                            <asp:TemplateField HeaderText="Registrado">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRegistrado" runat="server" Text='<%# Eval("REGISTRADO") %>' Enabled="false"
                                        Width="100px" OnKeyPress="javascript:return autoTab(this,event);" CssClass="numero"></asp:TextBox>
                                    <asp:ImageButton ID="imgEditarFinalizadora" ImageUrl="../../../img/edit.png" Width="20px"
                                        runat="server" OnClick="imgEditarFinalizadora_Click" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblRegistradoTotal" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Valor">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtValor" runat="server" Text='<%# Eval("VALOR") %>' CssClass="numero"
                                        onchange="atualizaGrid()" Width="100px" OnKeyPress="javascript:return numeros(this,event);"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblValorTotal" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Diferenca">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiferenca" runat="server" Text='<%# Eval("DIFERENCA") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblDiferencaTotal" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Deposito">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDeposito" runat="server" Text='<%# Eval("Deposito") %>' CssClass="numero"
                                        Width="100px" OnKeyPress="javascript:return numeros(this,event);"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblDeposito" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
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
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
            <HeaderTemplate>
                Adicionais
            </HeaderTemplate>
            <ContentTemplate>
                <div class="gridTable">


                    <asp:GridView ID="gridCancelados" runat="server" ForeColor="#333333" GridLines="Vertical"
                        AutoGenerateColumns="False" CssClass="table" Width="50%"
                        OnDataBound="gridItens_DataBound" AllowSorting="True"
                        OnSorting="gridItens_Sorting">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />

                        <Columns>
                            <asp:BoundField DataField="Cancelado" HeaderText="Cancelados" SortExpression="1"></asp:BoundField>
                            <asp:BoundField DataField="Total" HeaderText="Totais" SortExpression="2" ItemStyle-HorizontalAlign="Right"></asp:BoundField>

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
    </asp:TabContainer>
    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none" DefaultButton="ImgPesquisaLista">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" Style="font-size: 20px;">
            <br />
            <center><h1>Detalhes da finalizadora</asp:Label></h1></center>
            <asp:Label ID="lblErroPesquisa" runat="server" Text="" ForeColor="Red"></asp:Label>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                <asp:Label ID="lblFiltroFinalizadora" runat="server" Text="" Visible="false"></asp:Label>

                Valor :
                <asp:TextBox ID="txtValorFiltro" runat="server" Text="" CssClass="numero" Width="100px"
                    OnKeyPress="javascript:return numeros(this,event);"></asp:TextBox>
                <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="ImgPesquisaLista_Click" />
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
            <div class="panel" style="width: 20%; float: left; margin-top: -10px; margin-left: 5%;">
                <p>Qtde:</p>
                <asp:Label ID="lblTotalReg" runat="server" Text="" Style="font-size: 100px;"></asp:Label>
                <p>
                    Total:
                </p>
                <asp:Label ID="lblValorReg" runat="server" Text="" Style="font-size: 60px;"></asp:Label>

            </div>

            <asp:Panel ID="Panel2" runat="server" CssClass="lista" Style="width: 60%;">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
                    GridLines="None" OnRowDataBound="GridLista_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <HeaderStyle CssClass="th" />
                    <Columns>
                        <asp:BoundField DataField="cupom" HeaderText="Cupom"></asp:BoundField>
                        <asp:BoundField DataField="total" HeaderText="Valor"></asp:BoundField>
                        <asp:BoundField DataField="hora" HeaderText="Hora"></asp:BoundField>

                        <asp:TemplateField HeaderText="Finalizadora">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlFinalizadora" runat="server" Visible="false" OnSelectedIndexChanged="ddlFinalizadora_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:Label ID="lblFinalizadora" runat="server" Text='<%# Eval("id_finalizadora") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Autorizadora">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlAutorizadora" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="ddlAutorizadora_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:Label ID="lblAutorizadora" runat="server" Text='<%# Eval("Autorizadora") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cartão">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlCartao" runat="server" Visible="false" Width="100%">
                                </asp:DropDownList>
                                <asp:Label ID="lblCartao" runat="server" Text='<%# Eval("id_cartao") %>'></asp:Label>
                                <asp:Label ID="lblAutorizacao" runat="server" Text='<%# Eval("autorizacao") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Sequencia" HeaderText="Seq"></asp:BoundField>
                        <asp:TemplateField HeaderText="NSU" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCodAutorizacao" runat="server" Text='<%# Eval("Autorizacao") %>'
                                    Width="60px" MaxLength="20" CssClass="numero" Enabled="false"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle Width="5px" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnCartao" ImageUrl="../../../img/edit.png" Width="15px"
                                    runat="server" OnClick="imgBtnCartao_Click" />
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
        DropShadow="true" PopupControlID="pnFundo" TargetControlID="lblErroPesquisa">
    </asp:ModalPopupExtender>

    <asp:Panel ID="PnEncerrar" runat="server" CssClass="frameModal" Style="display: none; width: 400px;">
        <h2>
            <asp:Label ID="Label14" runat="server" Text="Tem Certeza que gostaria de Fechar o Caixa?"
                CssClass="cabMenu" Style="height: 40px;"></asp:Label>
        </h2>
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

    <asp:Panel ID="pnConfirmaProcessamento" runat="server" CssClass="frameModal" Style="display: none; width: 400px;">
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Tem Certeza que gostaria de Limpar as alterações no movimento?"
                CssClass="cabMenu" Style="height: 40px;"></asp:Label>
        </h2>
        <asp:Label runat="server" ID="Label6" Text=""></asp:Label>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaProcessar" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaProcessar_Click" />
                    <asp:Label ID="Label2" runat="server" Text="SIM"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelaProcessar" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaProcessar_Click" />
                    <asp:Label ID="Label3" runat="server" Text="NÃO"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalProcessamento" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaProcessamento" TargetControlID="Label1">
    </asp:ModalPopupExtender>

    <asp:Panel ID="PnSenha" runat="server" CssClass="frameModal" Style="display: none"
        DefaultButton="imgBtnConfirmaEstorno">

        <h2>
            <asp:Label ID="lblTituloSenha" runat="server" Text="Digite a Senha para ESTORNAR o fechamento."
                CssClass="cabMenu" Style="height: 80px; padding: 3px; font-size: medium;"></asp:Label>
        </h2>
        <asp:Label runat="server" ID="lblErroEstorno" Text=""></asp:Label>

        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaEstorno" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaEstorno_Click" />
                    <asp:Label ID="Label5" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelaEstorno" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaEstorno_Click" />
                    <asp:Label ID="Label17" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="font-size: 25px;">
                    <center>
                        SENHA:
                         
                        <asp:TextBox ID="txtSAutorizacao" runat="server"    Width="200px" Height="50px"   Font-Size="X-Large" autocomplete="off" TextMode="password"  ></asp:TextBox>
                    </center>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalSenha" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnSenha" TargetControlID="lblTituloSenha">
    </asp:ModalPopupExtender>



</asp:Content>
