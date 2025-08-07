<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="TransferenciaContas.aspx.cs" Inherits="visualSysWeb.modulos.Financeiro.pages.TransferenciaContas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h1>
            Movimentação Bancaria</h1>
    </center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <div class="btnImprimirDireita" style="margin-bottom: 0px;">
        <asp:Image ID="Image1" ImageUrl="~/img/icon_imprimir.gif" runat="server" Width="40px"
            onclick="PrintElem('divGrid')" />
    </div>
    <div class="btnImprimirDireita">
        Limpar Filtros
        <asp:ImageButton ID="imgBtnLimpar" runat="server" OnClick="imgBtnLimpar_Click" ImageUrl="../../../img/botao-apagar.png"
            Style="width: 20px" />
    </div>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Codigo</p>
                        <asp:TextBox ID="txtId" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Descrição</p>
                        <asp:TextBox ID="txtDescricao" runat="server" Width="200px"> </asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            De:</p>
                        <asp:TextBox ID="txtDtDe" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="txtDtDe_Change"> </asp:TextBox>
                        <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDtDe">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p>
                            Ate:</p>
                        <asp:TextBox ID="txtDtAte" runat="server" Width="80px"> </asp:TextBox>
                        <asp:ImageButton ID="imgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="clnDtAte" runat="server" PopupButtonID="imgDtAte" TargetControlID="txtDtAte">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p>
                            Origem</p>
                        <asp:TextBox ID="txtOrigem" runat="server" Width="100px"> </asp:TextBox>
                        <asp:ImageButton ID="imgBtnOrigem" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </div>
                    <div class="panelItem">
                        <p>
                            Destino</p>
                        <asp:TextBox ID="txtDestino" runat="server" Width="100px"> </asp:TextBox>
                        <asp:ImageButton ID="imgBtnDestino" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </div>
                    <div class="panelItem">
                        <p>
                            Tipo</p>
                        <asp:DropDownList ID="ddlTipo" runat="server" Height="25px">
                            <asp:ListItem>TODOS</asp:ListItem>
                            <asp:ListItem>TRANSFERENCIA</asp:ListItem>
                            <asp:ListItem>SAQUE</asp:ListItem>
                            <asp:ListItem>DEPOSITO</asp:ListItem>
                            <asp:ListItem>LANCAMENTO CREDITO</asp:ListItem>
                            <asp:ListItem>LANCAMENTO DEBITO</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>
                            Status</p>
                        <asp:DropDownList ID="ddlStatus" runat="server" Height="25px">
                            <asp:ListItem>TODOS</asp:ListItem>
                            <asp:ListItem>CONCLUIDA</asp:ListItem>
                            <asp:ListItem>ESTORNADA</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>
                            Centro Custo</p>
                        <asp:TextBox ID="txtCentroDeCusto" runat="server" Width="200px"> </asp:TextBox>
                        <asp:ImageButton ID="imgBtnCentroDeCusto" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="Img_Click" />
                    </div>


                    
                </td>
            </tr>
        </table>
    </div>
    <div class="panelItem" style="margin-top: 10px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
     <div id="divTotal" runat="server" visible="false" class="panelItem" style="margin-top: 10px; float:right; margin-right:40px;">
        <h1>TOTAL: R$ <asp:Label ID="lblTotalValores" runat="server" Text="R$ 0,00"></asp:Label></h1>
    </div>
    <div class="gridTable" id="divGrid">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
            GridLines="Vertical">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="id" Text="--" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/TransferenciaContasDetalhes.aspx?id={0}&filial={1}"
                    DataNavigateUrlFields="id,filial" />
                <asp:HyperLinkField DataTextField="dataBr" Text="--" Visible="true" HeaderText="Data"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/TransferenciaContasDetalhes.aspx?id={0}&filial={1}"
                    DataNavigateUrlFields="id,filial" />
                <asp:HyperLinkField DataTextField="centro_custo" Text="--" Visible="true" HeaderText="Centro Custo"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/TransferenciaContasDetalhes.aspx?id={0}&filial={1}"
                    DataNavigateUrlFields="id,filial" />
                
                <asp:HyperLinkField DataTextField="Tipo" Text="--" Visible="true" HeaderText="Tipo"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/TransferenciaContasDetalhes.aspx?id={0}&filial={1}"
                    DataNavigateUrlFields="id,filial" />
                <asp:HyperLinkField DataTextField="Descricao" Text="--" Visible="true" HeaderText="Descricao"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/TransferenciaContasDetalhes.aspx?id={0}&filial={1}"
                    DataNavigateUrlFields="id,filial" />
                <asp:HyperLinkField DataTextField="conta_origem" Text="--" Visible="true" HeaderText="Origem"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/TransferenciaContasDetalhes.aspx?id={0}&filial={1}"
                    DataNavigateUrlFields="id,filial" />
                <asp:HyperLinkField DataTextField="conta_destino" Text="--" Visible="true" HeaderText="Destino"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/TransferenciaContasDetalhes.aspx?id={0}&filial={1}"
                    DataNavigateUrlFields="id,filial" />
                <asp:HyperLinkField DataTextField="valor" Text="--" Visible="true" HeaderText="Valor"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/TransferenciaContasDetalhes.aspx?id={0}&filial={1}"
                    DataNavigateUrlFields="id,filial" DataTextFormatString="R$ {0:n}" ItemStyle-HorizontalAlign="Right"
                    HeaderStyle-HorizontalAlign="Right" />
                <asp:HyperLinkField DataTextField="status" Text="--" Visible="true" HeaderText="Status"
                    DataNavigateUrlFormatString="~/modulos/financeiro/pages/TransferenciaContasDetalhes.aspx?id={0}&filial={1}"
                    DataNavigateUrlFields="id,filial" />
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
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px;">
            <center>
                <h1>
                    <asp:Label ID="lbllista" runat="server" Text=""></asp:Label></h1>
            </center>
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
    <script>
        function PrintElem(elem) {


            var txtId = document.getElementById('MainContent_txtId');
            var txtDescricao = document.getElementById('MainContent_txtDescricao');
            var txtDtDe = document.getElementById('MainContent_txtDtDe');
            var txtDtAte = document.getElementById('MainContent_txtDtAte');
            var txtOrigem = document.getElementById('MainContent_txtOrigem');
            var txtDestino = document.getElementById('MainContent_txtDestino');
            var ddlTipo = document.getElementById('MainContent_ddlTipo');
            var ddlStatus = document.getElementById('MainContent_ddlStatus');



            var mywindow = window.open('', 'PRINT', 'width=1200');

            mywindow.document.write('<html><head><title>Negociacao Detalhes</title>');
            mywindow.document.write('</head><body >');

            mywindow.document.write('<table width=100%>');
            mywindow.document.write('<tr>');
            mywindow.document.write('<td>');
            mywindow.document.write('<b>CODIGO:</b>' + txtId.value);
            mywindow.document.write('</td>');

            mywindow.document.write('<td>');
            mywindow.document.write('<b>DESCRICAO:</b>' + txtDescricao.value);
            mywindow.document.write('</td>');

            mywindow.document.write('<td>');
            mywindow.document.write('<b>DE:</b>' + txtDtDe.value);
            mywindow.document.write('</td>');

            mywindow.document.write('<td>');
            mywindow.document.write('<b>ATE:</b>' + txtDtAte.value);
            mywindow.document.write('</td>');


            mywindow.document.write('</tr>');
            mywindow.document.write('<tr>');

            mywindow.document.write('<td>');
            mywindow.document.write('<b>ORIGEM:</b>' + txtOrigem.value);
            mywindow.document.write('</td>');

            mywindow.document.write('<td>');
            mywindow.document.write('<b>DESTINO:</b>' + txtDestino.value);
            mywindow.document.write('</td>');

            mywindow.document.write('<td>');
            mywindow.document.write('<b>TIPO:</b>' + ddlTipo.value);
            mywindow.document.write('</td>');




            mywindow.document.write('<td>');
            mywindow.document.write('<b>STATUS :</b> RS' + ddlStatus.value);
            mywindow.document.write('</td>');



            mywindow.document.write('</tr>');

            mywindow.document.write('</table>');

            mywindow.document.write(document.getElementById(elem).innerHTML);
            mywindow.document.write('</body></html>');

            mywindow.document.close(); // necessary for IE >= 10
            mywindow.focus(); // necessary for IE >= 10*/

            mywindow.print();
            mywindow.close();



            return true;
        }
    </script>
</asp:Content>
