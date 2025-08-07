<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmartPhone_Resultado.aspx.cs" Inherits="visualSysWeb.modulos.Mobile.pages.SmartPhone_Resultado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        a:link {
            color: #034af3;
        }

        .container {
            width: 100%;
            ##height:100vh;
            background: white;
            display: table;
            ##flex-direction: column;
            justify-content: center;
            align-items: center;
            font-size: 60px;
        }

        .titulo {
            text-align: center;
            align-items: center;
            font-size: 70px;
        }

        .table {
            width: 100%;
            margin-bottom: .5em;
            table-layout: fixed;
        }

        .coluna {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div>
            <div>
                <h1 class="titulo">
                    <asp:Label ID="lblTitulo" runat="server" Text=""></asp:Label>
                </h1>
                <table class="table" style="width: 100%">
                    <tr>
                        <td><h2>Dia: 
                            <asp:DropDownList ID="ddlDias" CssClass="form-control"
                                Font-Size="90px" Height="150px"
                                OnSelectedIndexChanged="ddlDias_SelectedIndexChanged"
                                AutoPostBack="true" runat="server">
                        
                            </asp:DropDownList>
                        </h2>
                        </td>
                    </tr>
                    <tr class="coluna">
                        <td class="container">
                            <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                PageSize="50" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%">
                                <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="Coluna" HeaderText="Status" ItemStyle-HorizontalAlign="left">
                                        <HeaderStyle Width="200px" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Valor" DataFormatString="{0:n}" HeaderText="Valor" ItemStyle-HorizontalAlign="Right">
                                        <HeaderStyle Width="200px" />
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VisualizaGrafico" HeaderText="VisualizaGrafico" Visible="False" />
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

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Chart ID="Grafico1" runat="server" Width="800px" Height="800px">
                                <Series>
                                    <asp:Series ChartType="Pie" Legend="Legend1" Name="Series1"
                                        XValueMember="Coluna" YValueMembers="Valor" IsValueShownAsLabel="True" LabelFormat="{0:n}">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1">
                                        <Area3DStyle Enable3D="true" Inclination="20" />
                                    </asp:ChartArea>
                                </ChartAreas>
                                <Legends>
                                    <asp:Legend Name="Legend1" Font="Microsoft Sans Serif, 20pt, style=Bold">
                                    </asp:Legend>
                                </Legends>
                                <Titles>
                                    <asp:Title Name="Title1" Font="Microsoft Sans Serif, 8pt, style=Bold"
                                        Text="Vendas" Visible="False">
                                    </asp:Title>
                                </Titles>
                            </asp:Chart>
                        </td>

                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
