<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Operadores.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.Operadores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Operadores</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <div class="filter" id="filtrosPesq" runat="server">
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        Codigo
                    </p>
                    <asp:TextBox ID="txtId_operador" runat="server" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <p>
                        Nome</p>
                    <asp:TextBox ID="txtNome" runat="server"> </asp:TextBox>
                </td>
                <td>
                    <div class="panelItem" cssclass="sem">
                        <br />
                        <asp:CheckBox ID="chkInativo" runat="server" Text="Inativo" AutoPostBack="true" ForeColor="Red"
                            Font-Bold="True" OnCheckedChanged="chk_CheckedChanged" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="panelItem" style="margin-bottom: 15px;">
        <h1>
            <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label></h1>
    </div>
    <div class="gridTable">
        <div id="divSalvaInativos" runat="server" class="panelItem" style="margin-top: 0px;
            margin-left: 10px;">
            <asp:ImageButton ID="imgBtnSalvarInativos" runat="server"  ImageUrl="../../../img/confirm.png"
                Width="15px" OnClick="imgBtnSalvarInativos_Click" />
            <asp:Label ID="lblSalvarInativos" runat="server" Text="Ativar Operadores Selecionados"
                Font-Size="Medium"></asp:Label>
        </div>
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
            GridLines="Vertical">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField Visible="false">
                    <HeaderTemplate>
                        <%--<asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged"
                                Checked="true" />--%>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelecionaItem" runat="server" Checked="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataTextField="ID_Operador" Text="----" Visible="true" HeaderText="Codigo"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/OperadoresDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="ID_Operador" />
                <asp:HyperLinkField DataTextField="Nome" Text="----" Visible="true" HeaderText="Nome"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/OperadoresDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="ID_Operador" />
                <asp:HyperLinkField DataTextField="Cargo" Text="----" Visible="true" HeaderText="Cargo"
                    DataNavigateUrlFormatString="~/modulos/cadastro/pages/OperadoresDetalhes.aspx?campoIndex={0}"
                    DataNavigateUrlFields="ID_Operador" />
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
