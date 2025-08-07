<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Tributacao.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.Tributacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Tributacao</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server" >
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        Codigo</p>
                    <asp:TextBox ID="txtCodigo" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Descricao</p>
                    <asp:TextBox ID="txtDescricao" runat="server"> </asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
     <div class="panelItem" style="margin-bottom: 15px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False"  
            ForeColor="#333333" GridLines="Vertical">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:HyperLinkField DataTextField="Codigo_Tributacao" Text="----" Visible="true"
                    HeaderText="Codigo" DataNavigateUrlFormatString="~/modulos/cadastro/pages/TributacaoDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_tributacao" />
                <asp:HyperLinkField DataTextField="Descricao_Tributacao" Text="----"
                    Visible="true" HeaderText="Tributacao" DataNavigateUrlFormatString="~/modulos/cadastro/pages/TributacaoDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_tributacao" />
                <asp:HyperLinkField DataTextField="Entrada_ICMS" Text="----" Visible="true"
                    HeaderText="ICMS Entrada" DataNavigateUrlFormatString="~/modulos/cadastro/pages/TributacaoDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_tributacao"/>

                <asp:HyperLinkField DataTextField="Saida_ICMS" Text="----" Visible="true" HeaderText="ICMS SAIDA"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/TributacaoDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_tributacao" />
                
                <asp:HyperLinkField DataTextField="Indice_ST" Text="----" Visible="true" HeaderText="Indice ST"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/TributacaoDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_tributacao" />
                <asp:HyperLinkField DataTextField="csosn" Text="----" Visible="true" HeaderText="CSOSN"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/TributacaoDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_tributacao" />
             
                <asp:HyperLinkField DataTextField="Redutor" Text="----" Visible="true" HeaderText="Redutor"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/TributacaoDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="codigo_tributacao" />
                
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
