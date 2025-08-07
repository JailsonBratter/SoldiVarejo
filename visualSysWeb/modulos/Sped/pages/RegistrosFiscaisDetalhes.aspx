<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RegistrosFiscaisDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.Sped.pages.RegistrosFiscaisDetalhes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Detalhes dos Registros Fiscais</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
    <div class="filter" id="filtrosPesq" runat="server">
        <table>
            <tr>
                <h1>
            </tr>
            <tr>
                <td>
                    <div class="panelItem">
                        <p>
                            De</p>
                        <asp:TextBox ID="txtDataDe" runat="server" Enabled="false" Width="100px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Ate</p>
                        <asp:TextBox ID="txtDataAte" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            CST Icms</p>
                        <asp:TextBox ID="txtCSTICMS" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Codigo Operação</p>
                        <asp:TextBox ID="txtCodOperacao" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Aliquota Icms</p>
                        <asp:TextBox ID="txtAliquotaIcms" runat="server" Width="100px" Enabled =" false"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Tipo NF</p>
                        <asp:TextBox ID="txtTipoNF" runat="server" Width="100px" Enabled =" false"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Documento</p>
                        <asp:TextBox ID="txtNota" runat="server" Width="100px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Participante</p>
                        <asp:TextBox ID="txtFornecedor" runat="server" Width="100px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            PLU</p>
                        <asp:TextBox ID="txtPLU" runat="server" Width="100px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Origem</p>
                        <asp:TextBox ID="txtOrigem" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:ImageButton ID="ImgBtnVoltar" runat="server" Height="28px" ImageUrl="~/img/icon_voltar.jpg"
        Width="60px"  OnClick="ImgBtnVoltar_Click" />
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" CellPadding="5"
            ForeColor="#333333" GridLines="Vertical" AllowSorting="true" OnSorting="gridPesquisa_Sorting"
            OnRowCommand="gridPesquisa_RowCommand">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="Nota"
                    HeaderText="Nota" ItemStyle-ForeColor="Blue"></asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="Serie"
                    HeaderText="Serie" ItemStyle-ForeColor="Blue"></asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="Cliente_fornecedor"
                    HeaderText="Fornecedor" ItemStyle-ForeColor="Blue"></asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="num_item"
                    HeaderText="n.Item" ItemStyle-ForeColor="Blue"></asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="PLU"
                    HeaderText="PLU" ItemStyle-ForeColor="Blue"></asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="Total_operacao"
                    HeaderText="Total Operação" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Right">
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="Base_icms"
                    HeaderText="Base Icms" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Right">
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="Total_icms"
                    HeaderText="Total Icms" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Right">
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="Base_icms_st"
                    HeaderText="Base Icms ST" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Right">
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="TOTAL_ICMS_ST"
                    HeaderText="TOTAL ICMS ST" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Right">
                </asp:ButtonField>
                
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="total_ipi"
                    HeaderText="Total IPI" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Right">
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="tipoNF"
                    HeaderText="Tipo NF" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Right">
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
        <br />
    </div>
</asp:Content>
