<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GraficoVendas.aspx.cs" Inherits="visualSysWeb.modulos.Gerencial.pages.GraficoVendas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <link href="../../../Styles/bootstrap.css" rel="stylesheet" />
    <link href="../../../Scripts/chartist/chartist.min.css" rel="stylesheet" />
    <script src="../../../Scripts/chartist/chartist.min.js"></script>
    <script src="../../../Scripts/JScriptsPadrao.js"></script>
    <script src="../js/GraficoVendas.js"></script>

    <style>
        .ct-series-b .ct-bar, .ct-series-b .ct-line, .ct-series-b .ct-point, .ct-series-b .ct-slice-donut {
           
             stroke: #F08080;
        }

        .ct-series-a .ct-bar, .ct-series-a .ct-line, .ct-series-a .ct-point, .ct-series-a .ct-slice-donut {
            stroke: #4169E1;
        }
        .grid{
            width:100%;
        }
        .ct-chart{
            margin-top:0;
            padding:0;
            border:solid;
            max-height:300px;
        }
        .
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-1">
                Filtros:
                    
            </div>
            
            <div class="col-md-11">
                <div class="panelItem">
                     De
                    <asp:TextBox ID="txtDe" runat="server" Width="100px"> </asp:TextBox>
                    <asp:Image ID="imgDtDe" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="clnDataDe" runat="server" PopupButtonID="imgDtDe"
                        TargetControlID="txtDe">
                    </asp:CalendarExtender>

                </div>
                <div class="panelItem">
                     Ate
                    <asp:TextBox ID="txtAte" runat="server" Width="100px"> </asp:TextBox>
                    <asp:Image ID="imgDtAte" ImageUrl="~/img/calendar.png" runat="server" Height="15px" />
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgDtAte"
                        TargetControlID="txtAte">
                    </asp:CalendarExtender>

                </div>
                
            </div>
            
         </div>
        <div class="row">
            <div class="col-md-6">
                <div class="row">
                <div class="col-md-6 ">
                    Filiais: <asp:TextBox ID="txtFiliais1" runat="server"></asp:TextBox>
                     <asp:ImageButton ID="imgBtnFiliais1" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                        OnClick="imgBtnFiltro_Click" />
                    <asp:GridView ID="gridVendas" runat="server" AutoGenerateColumns="False" CellPadding="5"
                        ForeColor="#333333" GridLines="Vertical" AllowSorting="true"
                        CssClass="grid"
                        >
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="MesAno" HeaderText="Desc" />
                            <asp:BoundField DataField="qtde" HeaderText="Qtde" />
                            <asp:BoundField DataField="vlr" HeaderText="Valor" />
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#4169E1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </div>
                <div class="col-md-6 gridnf" >
                    <asp:TextBox ID="txtFiliais2" runat="server" Width="200"></asp:TextBox>
                     <asp:ImageButton ID="imgBtnFiliais2" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                        OnClick="imgBtnFiltro_Click" />
                    <asp:GridView ID="gridVendas2" runat="server" AutoGenerateColumns="False" CellPadding="5"
                        ForeColor="#333333" GridLines="Vertical" AllowSorting="true"
                         CssClass="grid"
                       >
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="qtde" HeaderText="Qtde" />
                            <asp:BoundField DataField="vlr" HeaderText="Valor" />
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#F08080" Font-Bold="True" ForeColor="White" />
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
            </div>
            <div class="col-md-6">
                <div class="row">

                    <h3>Quantidade</h3>
                    <div id="grafico1" class="ct-chart ct-square"></div>
                
                    <h3>Valor</h3>
                    <div id="grafico2" class="ct-chart ct-square"></div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
