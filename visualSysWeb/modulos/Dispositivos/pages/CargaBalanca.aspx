<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CargaBalanca.aspx.cs" Inherits="visualSysWeb.modulos.Dispositivos.pages.CargaBalanca" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><h1 style="margin-left: 40px">Carga Balanças</h1></center>
    <hr />
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">
    </asp:Panel>
    <br />
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div class="MenuEtiqueta">
        Modelo Etiqueta
            <asp:RadioButtonList ID="rdoBalanca" runat="server"
                AutoPostBack="True" Height="82px" Width="932px" OnSelectedIndexChanged="rdoBalanca_SelectedIndexChanged">
                <asp:ListItem Value="0" Selected="True">Toledo MGV /III/IV</asp:ListItem>
                <asp:ListItem Value="1">Toledo MGV V</asp:ListItem>
                <asp:ListItem Value="6">Toledo MGV+Nutrição</asp:ListItem>

                <asp:ListItem Value="2">Filizola</asp:ListItem>
                <asp:ListItem Value="3">Elgin</asp:ListItem>
                <asp:ListItem Value="4">Urano</asp:ListItem>
                <asp:ListItem Value="5">Outra</asp:ListItem>
                <asp:ListItem Value="7">Sunnyvale Embaladora</asp:ListItem>
            </asp:RadioButtonList>
    </div>
    <div class="MenuEtiquetaDireita" style="width:30%">
        <asp:Image ID="imgBalanca" runat="server" Height="263px"
            ImageUrl="~/modulos/Dispositivos/imgs/balanca0.jpg" Width="250px" />
    </div>
    <div id="divEmbaladoras" runat="server" visible="false" class="MenuEtiquetaDireita" style="width: 30%; height:100%; border:solid 1px; padding:5px;">
        <h4>Embaladoras </h4>
        <br />
            <asp:GridView  ID="gridEmbaladoras" runat="server" AutoGenerateColumns="False"
                GridLines="Vertical" CellPadding="5" ForeColor="#333333">
                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleciona_CheckedChanged" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelecionaItem" runat="server" AutoPostBack="True"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="id" HeaderText="ID" />
                    <asp:BoundField DataField="Descricao" HeaderText="Descrição" ItemStyle-Width="200px" />
                    <asp:BoundField DataField="End_FTP" HeaderText="Endereco" />

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
    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 50px; margin-bottom: 20px;">
                        <asp:Button ID="btnOkError" runat="server" Text="OK" Width="200px" Height="100%"
                            Font-Size="Larger" OnClick="btnOkError_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalError" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnError" TargetControlID="lblErroPanel">
    </asp:ModalPopupExtender>
</asp:Content>
