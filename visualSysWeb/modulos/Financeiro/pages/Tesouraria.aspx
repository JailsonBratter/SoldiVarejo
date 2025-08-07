<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Tesouraria.aspx.cs" Inherits="visualSysWeb.modulos.Tesouraria.pages.Tesouraria" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Tesouraria</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div class="filter" id="filtrosPesq" runat="server" style="width: 100%;">
        <div class="panelItem" style="margin-bottom: 30px;">
            <p>
                De</p>
            <asp:TextBox ID="TxtData" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtData_TextChanged"></asp:TextBox>
            <asp:ImageButton ID="img_TxtData" runat="server" Height="15px" ImageUrl="~/img/calendar.png" />
            <asp:CalendarExtender ID="clnData" runat="server" PopupButtonID="img_TxtData" TargetControlID="TxtData">
            </asp:CalendarExtender>
        </div>
        <div class="panelItem" style="margin-bottom: 30px;">
            <p>
                ate</p>
            <asp:TextBox ID="txtDataAte" runat="server" Width="80px" AutoPostBack="True" OnTextChanged="TxtDataAte_TextChanged"></asp:TextBox>
            <asp:ImageButton ID="img_txtDataAte" runat="server" Height="15px" ImageUrl="~/img/calendar.png" />
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="img_txtDataAte" TargetControlID="txtDataAte">
            </asp:CalendarExtender>
        </div>
        <div id="divOperador" runat="server" class="panelItem" style="margin-bottom: 30px;">
        <p>Operador</p>
             
          <asp:DropDownList ID="ddlOperador" runat="server">
          </asp:DropDownList>
            

        </div>
        <div id="divStatus" runat="server" class="panelItem" style="margin-bottom: 30px;">
        <p>Status</p>
            <asp:DropDownList ID="ddlStatus" runat="server"  
                >
                <asp:ListItem>TODOS</asp:ListItem>
                <asp:ListItem>OPERANDO</asp:ListItem>
                <asp:ListItem>ABERTO</asp:ListItem>
                <asp:ListItem>FECHADO</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="btnImprimirDireita">
            <asp:ImageButton ID="imgBtnRelatorio" runat="server" 
                ImageUrl="~/img/icon_imprimir.gif" onclick="imgBtnRelatorio_Click" />
        </div>
        
        
    </div>
    
    <div id="conteudo" runat="server" class="gridTable">
        <div class="gridTable">
            <asp:GridView ID="gridHistorico" runat="server" AutoGenerateColumns="false" 
                CellPadding="4" ForeColor="#333333" GridLines="Vertical" OnRowCommand="gridHistorico_RowCommand" OnRowDataBound="gridHistorico_RowDataBound">
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>
                    <asp:HyperLinkField DataTextField="id_fechamento" Text="---" Visible="true" HeaderText="ID"
                        DataNavigateUrlFormatString="~/modulos/Financeiro/pages/TesourariaDetalhes.aspx?id={0}&pdv={2}&idfecha={3}&emissao={1}"
                        DataNavigateUrlFields="Operador,data_abertura,pdv,id_fechamento" />
                    
                    <asp:HyperLinkField DataTextField="data_abertura" Text="---" Visible="true" HeaderText="Abertura de Turno"
                        DataNavigateUrlFormatString="~/modulos/Financeiro/pages/TesourariaDetalhes.aspx?id={0}&pdv={2}&idfecha={3}&emissao={1}"
                        DataNavigateUrlFields="Operador,data_abertura,pdv,id_fechamento" />
                    
                    
                    <asp:HyperLinkField DataTextField="data_encerramento" Text="---" Visible="true" HeaderText="Encerramento de Turno"
                        DataNavigateUrlFormatString="~/modulos/Financeiro/pages/TesourariaDetalhes.aspx?id={0}&pdv={2}&idfecha={3}&emissao={1}"
                        DataNavigateUrlFields="Operador,data_abertura,pdv,id_fechamento" />

                    <asp:HyperLinkField DataTextField="data_fechamento" Text="---" Visible="true" HeaderText="Encerramento na Tesouraria"
                        DataNavigateUrlFormatString="~/modulos/Financeiro/pages/TesourariaDetalhes.aspx?id={0}&pdv={2}&idfecha={3}&emissao={1}"
                        DataNavigateUrlFields="Operador,data_abertura,pdv,id_fechamento" />
                    

                    <asp:HyperLinkField DataTextField="PDV" Text="Caixa" Visible="true" HeaderText="Caixa"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/TesourariaDetalhes.aspx?id={0}&pdv={2}&idfecha={3}&emissao={1}"
                        DataNavigateUrlFields="Operador,data_abertura,pdv,id_fechamento" />
                    
                    <asp:HyperLinkField DataTextField="NOME" Text="Operador" Visible="true" HeaderText="Operador"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/TesourariaDetalhes.aspx?id={0}&pdv={2}&idfecha={3}&emissao={1}"
                        DataNavigateUrlFields="Operador,data_abertura,pdv,id_fechamento" />
                    
                    <asp:HyperLinkField DataTextField="STATUS" Text="Status" Visible="true" HeaderText="Status"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/TesourariaDetalhes.aspx?id={0}&pdv={2}&idfecha={3}&emissao={1}"
                        DataNavigateUrlFields="Operador,data_abertura,pdv,id_fechamento" />

                     <asp:HyperLinkField DataTextField="CANCELADOS" Text="0,00" Visible="true" HeaderText="Cancelados"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/TesourariaDetalhes.aspx?id={0}&pdv={2}&idfecha={3}&emissao={1}"
                        DataNavigateUrlFields="Operador,data_abertura,pdv,id_fechamento" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                       

                   <asp:HyperLinkField DataTextField="TOTAL" Text="Status" Visible="true" HeaderText="Total"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/TesourariaDetalhes.aspx?id={0}&pdv={2}&idfecha={3}&emissao={1}"
                        DataNavigateUrlFields="Operador,data_abertura,pdv,id_fechamento" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />

                    <asp:HyperLinkField DataTextField="Operador" Text="Operador" Visible="true" HeaderText="IDOp"
                    DataNavigateUrlFormatString="~/modulos/Financeiro/pages/TesourariaDetalhes.aspx?id={0}&pdv={2}&idfecha={3}&emissao={1}"
                        DataNavigateUrlFields="Operador,data_abertura,pdv,id_fechamento" />

                    <asp:ButtonField ButtonType="Image" ImageUrl="~/modulos/Financeiro/imgs/CadernetaPequeno.png" CommandName="Finalizadoras" 
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
</asp:Content>
