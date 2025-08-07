<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ContratoFornecedor.aspx.cs" Inherits="visualSysWeb.modulos.Manutencao.pages.ContratoFornecedor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Contrato Fornecedor</h1></center>
    <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        <table>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            Codigo</p>
                        <asp:TextBox ID="txtCodigo" runat="server" Width="80px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Fornecedor</p>
                        <asp:TextBox ID="txtFornecedor" runat="server" Width="150px"> </asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>Tipo Data</p>
                        <asp:DropDownList ID="ddlTipo" runat="server">
                            <asp:ListItem>Cadastro</asp:ListItem>
                            <asp:ListItem>Validade</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="panelItem">
                        <p>
                            De</p>
                        <asp:TextBox ID="txtDataDe" runat="server" OnTextChanged="txtDataDe_TextChanged"
                            AutoPostBack="true" Width="100px"></asp:TextBox>
                        <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDataDe">
                        </asp:CalendarExtender>
                    </div>
                    <div class="panelItem">
                        <p>
                            Ate</p>
                        <asp:TextBox ID="txtDataAte" runat="server" Width="100px"></asp:TextBox>
                        <asp:ImageButton ID="imgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtAte"
                            TargetControlID="txtDataAte">
                        </asp:CalendarExtender>
                    </div>
                </td>
            </tr>
        </table>
    </div>
     <div class="panelItem" style="margin-bottom: 0px;">
                    <h1>
                        <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
                </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" CellPadding="6"
            ForeColor="#333333" GridLines="Vertical">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="id_contrato" Text="---" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="ContratoFornecedorDetalhe.aspx?codigo={0}"
                    DataNavigateUrlFields="id_contrato" />
                <asp:HyperLinkField DataTextField="fornecedor" Text="----" Visible="true" HeaderText="Fornecedor"
                    DataNavigateUrlFormatString="ContratoFornecedorDetalhe.aspx?codigo={0}"
                    DataNavigateUrlFields="id_contrato" />

               <asp:HyperLinkField DataTextField="Descricao" Text="----" Visible="true" HeaderText="Descricao"
                    DataNavigateUrlFormatString="ContratoFornecedorDetalhe.aspx?codigo={0}"
                    DataNavigateUrlFields="id_contrato" />
            
                <asp:HyperLinkField DataTextField="data_cadastro" Text="-----" Visible="true" HeaderText="Data Cadastro"
                    DataNavigateUrlFormatString="ContratoFornecedorDetalhe.aspx?codigo={0}"
                    DataNavigateUrlFields="id_contrato" />
                <asp:HyperLinkField DataTextField="data_validade" Text="----" Visible="true" HeaderText="Data Validade"
                    DataNavigateUrlFormatString="ContratoFornecedorDetalhe.aspx?codigo={0}"
                    DataNavigateUrlFields="id_contrato" />
                <asp:HyperLinkField DataTextField="prazo" Text="---" Visible="true" HeaderText="Prazo"
                    DataNavigateUrlFormatString="ContratoFornecedorDetalhe.aspx?codigo={0}"
                    DataNavigateUrlFields="id_contrato" />
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
</asp:Content>
