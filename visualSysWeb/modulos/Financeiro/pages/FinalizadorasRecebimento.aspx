<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FinalizadorasRecebimento.aspx.cs" 
    Inherits="visualSysWeb.modulos.Financeiro.pages.FinalizadorasRecebimento" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../js/TesourariaDetalhes.js" type="text/javascript"></script>
    <center><h1>Recebimento Finalizadoras</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div class="filter" id="filtrosPesq" runat="server" style="width: 100%;">
             
    </div>
    <div id="conteudo" runat="server" class="gridTable">
        <div class="gridTable">
            <asp:GridView ID="gridFinalizadora" runat="server" AutoGenerateColumns="false" 
                CellPadding="4" ForeColor="#333333" GridLines="Vertical" OnRowCommand="gridFinalizadora_RowCommand">
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="cupom" Visible="true" HeaderText="Cupom"/>
                    
                    <asp:BoundField DataField="hora_venda" Visible="true" HeaderText="Hora"/>
                    
                    <asp:BoundField DataField="sequencia" Visible="true" HeaderText="Seq."/>

                    <asp:BoundField DataField="finalizadora" Visible="true" HeaderText="cod.Fin."/>

                    <asp:BoundField DataField="id_finalizadora" Visible="true" HeaderText="Nome Finalizadora"/>

                    <asp:BoundField DataField="total" Visible="true" HeaderText="Total" ItemStyle-HorizontalAlign="Right"/>
                    
                    <asp:BoundField DataField="id_cartao" Visible="true" HeaderText="Cartao"/>

                     <asp:BoundField DataField="autorizacao" Visible="true" HeaderText="Cod.Autorizacao"/>

                   <asp:BoundField DataField="id_bandeira" Visible="true" HeaderText="Bandeira"/>

                   <asp:BoundField DataField="rede_cartao" Visible="true" HeaderText="Rede"/>

                    <asp:ButtonField ButtonType="Image" ImageUrl="~/modulos/Financeiro/imgs/CadernetaPequeno.png" CommandName="editar" 
                        HeaderText="Editar" Text="" ItemStyle-Width="20px" ItemStyle-Height="20px">
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
    </div>

    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none" DefaultButton="btnConfirmaLista">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" Style="font-size: 20px;">
            <br />
            <center><h1>Correção finalizadora</h1></center>
            <hr />
            <div id="divErro" class="row">
                <asp:Label ID="lblErro" runat="server" Text="" visible="false" Width="100px" Font-Size="Small"></asp:Label>
            </div>
            <table>
                <tr>
                    <div style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                        <table>
                            <tr>
                                <td>
                                    Valor Anterior : 
                                    <asp:Label ID="lblValor" runat="server" Text=""  Width="100px"></asp:Label>
                                </td>
                                <td>
                                     Valor Finalizadora: 
                                    <asp:Label ID="lblValorFinalizadora" runat="server" Text=""  Width="100px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </tr>
                <tr>
                    <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                        <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                            <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                                Width="25px" OnClick="btnConfirmaLista_Click" />
                            <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
                        </div>
                        <div class="row">
                            <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="btnCancelaLista_Click" />
                            <asp:Label ID="LblCancelaLista" runat="server" Text="Cancela"></asp:Label>
                        </div>
                    </div>
                </tr>
            </table>
            <div id="dadosGerais">
                <!-- <div class="row" style="width: 20%; float: left; margin-top: -10px; margin-left: 5%; font-size:small;"> -->
                <div>
                    <p>Dados</p>
                    <table>
                        <tr>
                            <td>
                                <p>Finalizadora</p>
                                <asp:DropDownList ID="ddlFinalizadora" runat="server" Visible="true" OnSelectedIndexChanged="ddlFinalizadora_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>                        
                            </td>
                            <td>
                                <p>Autorizadora</p>
                                <asp:DropDownList ID="ddlAutorizadora" runat="server" Visible="false" Width="100%" OnSelectedIndexChanged="ddlAutorizadora_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <p>Cartão</p>
                                <asp:DropDownList ID="ddlCartao" runat="server" Visible="false" Width="100%">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <p>Codigo Autorização</p>
                                <asp:TextBox ID="txtCodAutorizacao" runat="server" Visible="false"  OnKeyPress="javascript:return numeros(this,event);"/>
                            </td>
                            <td>
                                <p>Valor</p>
                                <asp:TextBox ID="txtValor" runat="server" Visible="true" OnKeyPress="javascript:return numeros(this,event);"/>
                            </td>
                            <td>
                                <asp:ImageButton ID="btnAdicionar" runat="server" ImageUrl="~/img/add.png" OnClick="btnAdicionar_Click" Width="20px" Height="20px" /><p>Adicionar</p>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblCupom" runat="server" Text="" Style="font-size: 100px;"></asp:Label>
                </div>

                <asp:Panel ID="Panel2" runat="server" CssClass="lista" Style="width: 60%;">
                    <div>
                         <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
                        GridLines="None" OnRowCommand="GridLista_RowCommand" >
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <HeaderStyle CssClass="th" />
                        <Columns>
                            <asp:BoundField DataField="finalizadora" HeaderText="Finalizadora" />
                            <asp:BoundField DataField="autorizadora" HeaderText="Autorizadora" />
                            <asp:BoundField DataField="cartao" HeaderText="Cartao" />
                            <asp:BoundField DataField="codigoAutorizacao" HeaderText="CodigoAutorizacao" />
                            <asp:BoundField DataField="valor" HeaderText="Valor" />
                            <asp:ButtonField ButtonType="Image" ImageUrl="../../../img/cancel.png" CommandName="excluir"
                                HeaderText="Del" Text="" ItemStyle-Width="10px" ItemStyle-Height="10px">
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
                </asp:Panel>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalPnFundo" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnFundo" TargetControlID="lblValor">
    </asp:ModalPopupExtender>

</asp:Content>
