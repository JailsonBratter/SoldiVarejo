<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NFFiscalDetalhes.aspx.cs" Inherits="visualSysWeb.modulos.NotaFiscal.pages.NFFiscalDetalhes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
    <style type="text/css">
        .auto-style1 {
            width: 134px;
        }
    </style>
    <script src="../js/NFFiscalDetalhes.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fecharJanela() {
            window.close();
        }

    </script>
    <center><h1>Detalhes dos Registros Fiscais De Entrada</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
    <div class="filter" id="filtrosPesq" runat="server">
    </div>
    <div class="gridTable">
        <asp:Panel runat="server">
            <table>
                <tr>
                    <td>
                        <div class="panelItem">
                            <p>Base ICMS:</p>
                            <asp:TextBox ID="txtTBCICMS" runat="server" Text="0,00" CssClass="numero"></asp:TextBox>
                            <p>Valor ICMS:</p>
                            <asp:TextBox ID="txtTValorICMS" runat="server" Text="0,00" CssClass="numero"></asp:TextBox>
                            <br /><asp:Label ID="lblSN" runat="server" Visible="False" Text="SIMPLES NACIONAL" Font-Bold="True" ForeColor="Red" ></asp:Label>
                        </div>
                    </td>
                    <td>
                        <div class="panelItem">
                            <p>Base PIS/COFINS:</p>
                            <asp:TextBox ID="txtTBCPISCofins" runat="server" Text="0,00" CssClass="numero"></asp:TextBox>
                            <p>Valor PIS:</p>
                            <asp:TextBox ID="txtTValorPPIS" runat="server" Text="0,00" CssClass="numero"></asp:TextBox>
                            <p>Valor COFINS:</p>
                            <asp:TextBox ID="txtTValorCofins" runat="server" Text="0,00" CssClass="numero"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="panelItem">
                            <p>Base ICMS ST:</p>
                            <asp:TextBox ID="txtBCST" runat="server" Text="0,00" CssClass="numero"></asp:TextBox>
                            <p>Valor ICMS ST:</p>
                            <asp:TextBox ID="txtValorST" runat="server" Text="0,00" CssClass="numero"></asp:TextBox>
                            <p>Valor IPI:</p>
                            <asp:TextBox ID="txtValorIPI" runat="server" Text="0,00" CssClass="numero"></asp:TextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblValidacaoFiscal" runat="server" Text="" />
        <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" CellPadding="5"
            ForeColor="#333333" GridLines="Vertical" AllowSorting="true" OnSorting="gridPesquisa_Sorting"
            OnRowCommand="gridPesquisa_RowCommand" OnRowDataBound="gridPesquisa_RowDataBound">
            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="Seq" HeaderText="Seq">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="ncm" HeaderText="NCM">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="plu" HeaderText="PLU">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>

                <asp:TemplateField HeaderText="CST ICMS" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblCSTICMS" runat="server" Text='<%# Eval("cst_icms","{0:N0}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtCSTICMS" runat="server" Text='<%# Eval("cst_icms", "{0:N0}") %>'
                            Width="30px" CssClass="numero" onkeypress="return numeros(this, event);" AutoPostBack="true"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="CFOP" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblCFOP" runat="server" Text='<%# Eval("cfop","{0:0}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtCFOP" runat="server" Text='<%# Eval("cfop", "{0:0}") %>'
                            Width="30px" CssClass="numero" onkeypress="return numeros(this, event);" AutoPostBack="true"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Aliq.ICMS" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblAliqICMS" runat="server" Text='<%# Eval("aliq_icms","{0:N2}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtAliqICMS" runat="server" Text='<%# Eval("aliq_icms", "{0:N2}") %>'
                            Width="40px" CssClass="numero" onkeypress="return numeros(this, event);" AutoPostBack="true"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="total_item" HeaderText="VL ITEM">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>

                <asp:TemplateField HeaderText="BC ICMS" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblBCICMS" runat="server" Text='<%# Eval("bc_icms","{0:N2}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtBCICMS" runat="server" Text='<%# Eval("bc_icms", "{0:N2}") %>'
                            Width="60px" CssClass="numero" onkeypress="return numeros(this, event);" AutoPostBack="true"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Red.BC" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblRedutorBC" runat="server" Text='<%# Eval("redutor_base","{0:N3}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtRedutorBC" runat="server" Text='<%# Eval("redutor_base", "{0:N3}") %>'
                            Width="40px" CssClass="numero" onkeypress="return numeros(this, event);" AutoPostBack="true"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Vlr ICMS" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblVlrICMs" runat="server" Text='<%# Eval("vl_icms","{0:N2}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtVlrICMs" runat="server" Text='<%# Eval("vl_icms", "{0:N2}") %>'
                            Width="60px" CssClass="numero txtVlrICMs" onkeypress="return numeros(this, event);" AutoPostBack="true"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Aliq. ICMS SN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblAliqSN" runat="server" Text='<%# Eval("aliq_sn","{0:N2}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtAliqSN" runat="server" Text='<%# Eval("aliq_sn", "{0:N2}") %>'
                            Width="40px" CssClass="numero" onkeypress="return numeros(this, event);" AutoPostBack="true"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Vlr. ICMS SN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblValorSN" runat="server" Text='<%# Eval("valor_sn","{0:N2}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtValorSN" runat="server" Text='<%# Eval("valor_sn", "{0:N2}") %>'
                            Width="40px" CssClass="numero" onkeypress="return numeros(this, event);" AutoPostBack="true"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="bcicmsst" HeaderText="BC ICMS ST">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="vlicmsst" HeaderText="Vlr ICMS ST">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="ipi" HeaderText="Aliq IPI">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="vl_ipi" HeaderText="Vlr IPI">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>

                <asp:TemplateField HeaderText="CST Pis/C" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblCSTPISCofins" runat="server" Text='<%# Eval("cst_piscofins","{0:0}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtCSTPisCofins" runat="server" MaxLength="2" Text='<%# Eval("cst_piscofins", "{0:0}") %>'
                            Width="30px" CssClass="numero txtCSTPisCofins" onkeypress="return numeros(this, event);" onchange="atualizaPIS(this);" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="BC Pis/C" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblBCPisCofins" runat="server" Text='<%# Eval("bc_piscofins","{0:N2}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtBCPisCofins" runat="server" Text='<%# Eval("bc_piscofins", "{0:N2}") %>'
                            Width="60px" CssClass="numero txtBCPisCofins" onkeypress="return numeros(this, event);" AutoPostBack="false" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Aliq PIS" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblAliqPIS" runat="server" Text='<%# Eval("aliq_pis","{0:N2}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtAliqPIS" runat="server" Text='<%# Eval("aliq_pis", "{0:N2}") %>'
                            Width="30px" CssClass="numero txtAliqPIS" onkeypress="return numeros(this, event);" AutoPostBack="false"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Vlr PIS" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblVlrPIS" runat="server" Text='<%# Eval("vl_pis","{0:N2}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtVlrPIS" runat="server" Text='<%# Eval("vl_pis", "{0:N2}") %>'
                            Width="40px" CssClass="numero txtVlrPIS" onkeypress="return numeros(this, event);" AutoPostBack="false"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Aliq Cofins" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblAliqCofins" runat="server" Text='<%# Eval("aliq_cofins","{0:N2}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txt1AliqCofins" runat="server" Text='<%# Eval("aliq_cofins", "{0:N2}") %>'
                            Width="30px" CssClass="numero txt1AliqCofins" onkeypress="return numeros(this, event);" AutoPostBack="false"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Vlr Cofins" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblVlrCofins" runat="server" Text='<%# Eval("vl_cofins","{0:N2}") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtVlrcofins" runat="server" Text='<%# Eval("vl_cofins", "{0:N2}") %>'
                            Width="60px" CssClass="numero txtVlrcofins" onkeypress="return numeros(this, event);" AutoPostBack="false"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField>
                    <itemtemplate>
                        <asp:Button ID="btnCorrigir" Text="Corrigir" runat="server" OnClick="btnCorrigir_Click"/>
                    </itemtemplate>
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
        <br />
    </div>
</asp:Content>
