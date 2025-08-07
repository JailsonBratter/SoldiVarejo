<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FechamentoEstoque.aspx.cs" Inherits="visualSysWeb.modulos.Sped.pages.FechamentoEstoque" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/bootstrap.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <center><h1>Fechamento de Estoque</h1></center>
        <hr />
        <asp:Panel ID="pnBtns" runat="server" CssClass="cabMenu">
        </asp:Panel>
        <asp:Label ID="lblErro" runat="server" Text="" ForeColor="Red"></asp:Label>
        <br />
    </div>

    <asp:Panel id="pnConteudo" runat="server" CssClass="cabMenu" style=" border-bottom-style: solid; border-width: 0px;height:100%; font-size:35px">
        <table width="100%" >
            <tr>
                <td>
                    <div style"font-size:30px">
                        <p>
                            Data do próximo fechamento
                        </p>
                            <asp:TextBox ID="txtDataFechamento" runat="server" Width="150px" AutoPostBack="true" style="font-size:25px"> </asp:TextBox>
                            <asp:ImageButton ID="ImgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                            <asp:CalendarExtender ID="clnDataFechamento" runat="server" PopupButtonID="imgDtDe" TargetControlID="txtDataFechamento">
                            </asp:CalendarExtender>
                    </div>
                    <div >
                        <asp:Button ID="btnFecharMes" runat="server" Text="Fechar Mes"
                            OnClick="btnFecharMes_Click" CssClass="submitButton" Height="40px" />
                    </div>

                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
