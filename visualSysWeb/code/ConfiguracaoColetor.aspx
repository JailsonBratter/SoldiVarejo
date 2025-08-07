<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracaoColetor.aspx.cs" Inherits="visualSysWeb.code.ConfiguracaoColetor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../Styles/bootstrap.css" rel="stylesheet" />
    <link href="../Styles/Site1.css" rel="stylesheet" />
    <title>Config Coletor</title>
</head>
<body>
    <form runat="server">
        <div class="container" runat="server" style="height: 100%; font-size: 15px;">
            <div id="divColetor" class="panel" style="width: 100%; height: 100%; float: left;">
                <div style="margin-left: 30px;">
                    <div class="row">
                        <div class="panelItem" style="width: 100%;">
                            <asp:RadioButtonList ID="RdoTipoDeArquivo" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="True" OnSelectedIndexChanged="RdoTipoDeArquivo_SelectedIndexChanged" Width="100%">
                                <asp:ListItem Value="0" Selected="True">Tamanho Fixo</asp:ListItem>
                                <asp:ListItem Value="1">Caracter Delimitador</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <asp:Label ID="lblDel" runat="server" Text="Delimitador" Visible="False"></asp:Label>
                        </div>
                        <div class="panelItem">
                            <asp:TextBox ID="txtDel" runat="server" Width="20px" Visible="False"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <div class="panelItem" style="width: 80px; float: left;">
                                PLU/EAN:
                            </div>
                            <asp:Label ID="lblInicioPlu" runat="server" Text="Inicio"></asp:Label>
                            <asp:TextBox ID="txtinicioPlu" runat="server" Width="30px"></asp:TextBox>
                            <asp:Label ID="lblFimPlu" runat="server" Text="Fim"></asp:Label>
                            <asp:TextBox ID="txtFimPlu" runat="server" Width="30px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <div class="panelItem" style="width: 80px; float: left;">
                                Contado
                            </div>
                            <asp:Label ID="lblInicioContado" runat="server" Text="Inicio"></asp:Label>
                            <asp:TextBox ID="txtInicioContado" runat="server" Width="30px"></asp:TextBox>

                            <asp:Label ID="lblFimContado" runat="server" Text="Fim"></asp:Label>
                            <asp:TextBox ID="txtFimContado" runat="server" Width="30px"></asp:TextBox>
                            Casas Decimais
                            <asp:TextBox ID="txtDecimaisContado" runat="server" Width="30px"></asp:TextBox>
                        </div>
                    </div>
                    <div id="divPreco" runat="server" class="row">
                        <div class="panelItem">
                            <div class="panelItem" style="width: 80px; float: left;">
                                Preco
                            </div>
                            <asp:Label ID="lblInicioPreco" runat="server" Text="Inicio"></asp:Label>
                            <asp:TextBox ID="txtPrecoInicio" runat="server" Width="30px"></asp:TextBox>

                            <asp:Label ID="lblPrecoFim" runat="server" Text="Fim"></asp:Label>
                            <asp:TextBox ID="txtPrecoFim" runat="server" Width="30px"></asp:TextBox>
                            Casas Decimais
                            <asp:TextBox ID="txtPrecoCasasDecimais" runat="server" Width="30px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panelItem">
                            <asp:Button ID="btnSalvarConfigColetor" runat="server" Text="Salvar" OnClick="btnSalvarConfigColetor_Click" />
                        </div>
                    </div>
                </div>


            </div>
        </div>

    </form>

</body>
</html>
