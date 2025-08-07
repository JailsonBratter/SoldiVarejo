<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RelImpostos.aspx.cs" Inherits="visualSysWeb.modulos.Relatorios.pages.RelImpostos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
        <h2>
            Valores aproximados de impostos
        </h2>
    </center>
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
 <asp:Panel ID="Panel2" runat="server" CssClass="filter" >
    
        <table>
            <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>
            <tr>
                <td>
                    <p>
                        De</p>
                    <asp:TextBox ID="txtDe" runat="server" Width="100px" MaxLength="10"> </asp:TextBox>
                    <asp:Image ID="ImgDeCalendario" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDeCalendario"
                        TargetControlID="txtDe">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <p>
                        Ate</p>
                    <asp:TextBox ID="txtAte" runat="server" Width="100px" MaxLength="10"> </asp:TextBox>
                    <asp:ImageButton ID="dt_txtAte" runat="server" ImageUrl="~/img/calendar.png" Height="15px" />
                    <asp:CalendarExtender ID="ClnDataAte" runat="server" PopupButtonID="Dt_txtAte" TargetControlID="txtAte">
                    </asp:CalendarExtender>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
    <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
        <asp:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
            <HeaderTemplate>
                Empresa
            </HeaderTemplate>
            <ContentTemplate>
                <asp:GridView class="gridTable" ID="GridImpostos" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" CssClass="FrameDivisaoTela" AutoGenerateColumns="False" >
                    <Columns>
                        <asp:BoundField DataField="TIPO" HeaderText="Tipo" />
                        <asp:BoundField DataField="VL_CONTABIL" HeaderText="Vlr Contabil" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="BC_ICMS" HeaderText="BC ICMS" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="VL_ICMS" HeaderText="Vlr ICMS" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="ICMS_RETIDO" HeaderText="ICMS Ret" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="VL_IPI" HeaderText="Vlr IPI" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="BC_PIS" HeaderText="BC PIS" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="VL_PIS" HeaderText="Vlr PIS" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="BC_COFINS" HeaderText="BC Cofins" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="VL_COFINS" HeaderText="Vlr Cofins" DataFormatString="{0:n}" ItemStyle-HorizontalAlign="Right" />
                    </Columns>
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
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
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
</asp:Content>
