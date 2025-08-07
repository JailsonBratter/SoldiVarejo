<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RegistrosFiscais.aspx.cs" Inherits="visualSysWeb.modulos.Sped.pages.RegistrosFiscais" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../../Styles/Site.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1>Registros Fiscais</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
    <div class="filter" id="filtrosPesq" runat="server">
    <div class="btnImprimirDireita">
                        Limpar Filtros
                        <asp:ImageButton ID="imgBtnLimpar" runat="server" OnClick="imgBtnLimpar_Click" ImageUrl="../../../img/botao-apagar.png"
                            Style="width: 20px" />
                    </div>
        <table>
            <tr>
                <td>
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
                    <div class="panelItem">
                        <p>
                            CST Icms</p>
                        <asp:TextBox ID="txtCSTICMS" runat="server" Width="100px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            CFOP</p>
                        <asp:TextBox ID="txtCodOperacao" runat="server" Width="100px"></asp:TextBox>
                    </div>
                    <div class="panelItem">
                        <p>
                            Aliquota Icms</p>
                        <asp:TextBox ID="txtAliquotaIcms" runat="server" Width="100px"></asp:TextBox>
                    </div>
                    
                </td>
            </tr>
        </table>
    </div>
    <div class="gridTable">
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" CellPadding="5"
            ForeColor="#333333" GridLines="Vertical" AllowSorting="true" OnSorting="gridPesquisa_Sorting"
            OnRowCommand="gridPesquisa_RowCommand" OnRowDataBound="gridPesquisa_RowDataBound">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="CST_ICMS"
                    HeaderText="CST/ICMS" ItemStyle-ForeColor="Blue"></asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="codigo_operacao"
                    HeaderText="Codigo operação" ItemStyle-ForeColor="Blue"></asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="aliquota_icms"
                    HeaderText="Aliquota Icms" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Right">
                </asp:ButtonField>
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
                    HeaderText="Total Icms ST" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Right">
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="total_ipi"
                    HeaderText="Total IPI" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Right">
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="outras"
                    HeaderText="Outras" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Right">
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="tipoNF"
                    HeaderText="Tipo" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Right">
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Link" CommandName="detalhe" Text="----" DataTextField="Origem"
                    HeaderText="Origem" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Right">
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
