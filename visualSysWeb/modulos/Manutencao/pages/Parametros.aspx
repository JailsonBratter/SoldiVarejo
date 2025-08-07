<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Parametros.aspx.cs" Inherits="visualSysWeb.modulos.Manutencao.pages.Parametros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../css/Parametros.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%;">
        <tr>
            <td>
                <div class="row">
                    <div id="divDireita" runat="server" class="divContainer">
                        <h2>PARAMETROS</h2>
                        <hr>
                        <div class="row">
                            Filtro:
                            <asp:TextBox ID="txtFiltro" runat="server" Width="80%" />
                            <asp:ImageButton ID="ImgBtnFiltrar" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                Width="15px" OnClick="ImgBtnFiltrar_Click" />
                        </div>
                        <div class="row gridTable">
                            <asp:GridView ID="gridParametros" runat="server" AutoGenerateColumns="false" CssClass="table"
                                OnRowDataBound="gridParametros_RowDataBound">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rdoItem" Text="" runat="server" GroupName="GrlistaItem" OnCheckedChanged="rdoItem_CheckedChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PARAMETRO" HeaderText="PARAMETRO" />
                                    <asp:BoundField DataField="Str_Valor" HeaderText="VALOR ATUAL" />
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

                    <div id="divEsquerda" runat="server" class="divContainer">
                        <h2>DETALHES</h2>
                       <hr />
                        <h2>
                            <asp:Label ID="lblParametro" runat="server" />

                        </h2>

                        <div class="row">
                            <div class="panelItem">
                                <p>Valor Atual</p>
                                <asp:TextBox ID="txtValorAtual" runat="server" Width="100%" OnTextChanged="txtValorAtual_TextChanged"  AutoPostBack="true"/>
                                <asp:DropDownList ID="ddlValorAtual" Visible="false" runat="server" OnSelectedIndexChanged="ddlValorAtual_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="TRUE" />
                                    <asp:ListItem Text="FALSE" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                           
                                <p>Detalhes</p>
                                <asp:TextBox ID="txtDetalhes" TextMode="MultiLine" Enabled="false" runat="server" 
                                    Height="200px" Width="100%" />
                           
                </div>

                </div>

            </td>
        </tr>
    </table>
</asp:Content>
