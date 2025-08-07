<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmartPhone_NF_Detalhe.aspx.cs" Inherits="visualSysWeb.modulos.Mobile.SmartPhone_NF_Detalhe" %>

<!DOCTYPE html>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Styles/bootstrap.css" rel="stylesheet" />
    <link href="../../Styles/mobile.css" rel="stylesheet" />

    <script language="javascript">

        function fechar() {
            var resposta = Confirm("Deseja abandonar recebimento?");
            if (resposta == true) {
                alert("teste");
                var tab = window.open(window.location, "_top");
                tab.close();
            }
        }

        function isNum(caractere) {

            var strValidos = "0123456789-,."

            if (strValidos.indexOf(caractere) == -1)

                return false;

            return true;

        }

        function validaTecla(campo, event) {

            var BACKSPACE = 8;

            var key;

            var tecla;

            CheckTAB = true;

            if (navigator.appName.indexOf("Netscape") != -1)

                tecla = event.which;

            else

                tecla = event.keyCode;

            key = String.fromCharCode(tecla);

            //alert( 'key: ' + tecla + ' -> campo: ' + campo.value);


            if (tecla == BACKSPACE || tecla == 13)

                return true;

            return (isNum(key));

        }

        function EnterKeyFilter() {
            if (window.event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
            }
        }

    </script>

    <style type="text/css">
        .auto-style1 {
            width: 208px;
        }

        .auto-style2 {
            width: 428px;
        }

        .auto-style3 {
            width: 270px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server" onkeydown="EnterKeyFilter();">
        <div>
            <div>
                <center>
            <p style="font-weight:bold; font-size:larger">Detalhes da Nota de Entrada</p>
                </center>
            </div>
            <br />
            <asp:Panel ID="cabecalho" runat="server" CssClass="frame">
                <table>
                    <tr>
                        <td class="auto-style1">
                            <p>
                                Codigo
                            </p>
                            <asp:Label ID="lblCodigo" runat="server" Width="80px" CssClass="numero"></asp:Label>
                        </td>
                        <td class="auto-style2">
                            <p>
                                CNPJ
                            </p>
                            <asp:Label ID="lblFornecedor_CNPJ" Width="150px" runat="server"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <p>
                                Emissão
                            </p>
                            <asp:Label ID="lblEmissao" runat="server" Width="80px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <p>
                                Fornecedor
                            </p>
                            <asp:Label ID="lblCliente_Fornecedor" runat="server" Width="100%"></asp:Label>
                        </td>
                </table>
                <asp:Label ID="lblError" runat="server" Width="100%" Visible="false" ForeColor="Red"></asp:Label>
                <asp:Button ID="btnReceber" Text="Receber" OnClick="btnReceber_Click" OnClientClick="javascript:return confirm('Confirma recebimento da NFe?');" runat="server" BackColor="Green" ForeColor="White" />
                <asp:Button ID="btnCancelar" Text="Cancelar" OnClick="btnCancelar_Click" OnClientClick="javascript:return confirm('Deseja abandonar o recebimento?');" runat="server" BackColor="Red" ForeColor="White" />
            </asp:Panel>
            <asp:Panel ID="pnlItem" runat="server" CssClass="frame" BackColor="WhiteSmoke">
                <table>
                    <tr style="width: 100%">
                        <td style="width: 20%;">
                            <p>
                                PLU/EAN
                            </p>
                            <asp:TextBox ID="txtPlu" runat="server" CssClass="sem" Width="231px" Enabled="true" AutoPostBack="false" OnTextChanged="txtPlu_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="imgPLU" runat="server" ImageUrl="~/img/confirm.png" Height="30px"
                                Width="30px" OnClick="txtPlu_TextChanged" />
                        </td>
                        <td style="width: 60%;">
                            <p>
                                Descrição
                            </p>
                            <asp:TextBox ID="txtDescricao" runat="server" Width="95%" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="width: 10%; text-align: center">
                            <p>
                                <asp:Label ID="lblRecebido" runat="server" Text="Recebido"></asp:Label>
                            </p>
                            <asp:TextBox ID="txtRecebido" runat="server" CssClass="numero" Width="80%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:ImageButton ID="imgValida" runat="server" ImageUrl="~/img/confirm.png" Height="80px"
                                Width="60px" OnClick="imgValida_Click" ImageAlign="AbsMiddle" />&nbsp;</td>
                    </tr>
                </table>
                Validade
                    <div class="row" style="margin-left: 5px;">
                        <asp:DropDownList ID="ddlDia" runat="server">
                            <asp:ListItem Text="" Value="00" />
                            <asp:ListItem Text="01" Value="01" />
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlMes" runat="server">
                            <asp:ListItem Text="" Value="00" />
                            <asp:ListItem Text="01" Value="01" />
                            <asp:ListItem Text="02" Value="02" />
                            <asp:ListItem Text="03" Value="03" />
                            <asp:ListItem Text="04" Value="04" />
                            <asp:ListItem Text="05" Value="05" />
                            <asp:ListItem Text="06" Value="06" />
                            <asp:ListItem Text="07" Value="07" />
                            <asp:ListItem Text="08" Value="08" />
                            <asp:ListItem Text="09" Value="09" />
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="11" Value="11" />
                            <asp:ListItem Text="12" Value="12" />
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlAno" runat="server">
                            <asp:ListItem Text="" Value="0000" />
                            <asp:ListItem Text="2023" Value="2023" />
                            <asp:ListItem Text="2024" Value="2024" />
                            <asp:ListItem Text="2025" Value="2025" />
                            <asp:ListItem Text="2026" Value="2026" />
                        </asp:DropDownList>
                        ____
                        <asp:CheckBox ID="chkSemValidade" Text="Sem validade" runat="server" OnCheckedChanged="chkSemValidade_CheckedChanged" />
                    </div>
                <asp:Label ID="lblCritica" Text="" runat="server" Visible="false" />
            </asp:Panel>
            <div id="conteudo" runat="server" class="conteudo">
                <asp:Panel ID="addItens" runat="server" CssClass="titulobtn" Visible="false">
                    <asp:ImageButton ID="ImgBtnAddItens" runat="server" ImageUrl="~/img/add.png" Height="40px"
                        Width="40px" OnClick="ImgBtnAddItens_Click" />
                    Incluir item
                    <asp:Label ID="lblIndex" Text="0" runat="server" Visible="false" />
                </asp:Panel>
                <div class="gridnf">
                    <asp:GridView ID="gridItens" runat="server" ForeColor="#333333" GridLines="Vertical"
                        AutoGenerateColumns="False"
                        CssClass="table" OnRowCommand="gridItens_RowCommand">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" ImageUrl="~/img/edit.png" CommandName="Alterar"
                                Text="Alterar">
                                <ControlStyle Height="20px" Width="20px" />
                                <ItemStyle Width="20px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="plu" HeaderText="PLU">
                                <ItemStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Descricao" HeaderText="Descrição" HtmlEncode="false">
                                <ItemStyle Width="200px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="qtde" HeaderText="Qtde" Visible="false">
                                <ItemStyle Width="80px" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="emb" HeaderText="Emb" Visible="false">
                                <ItemStyle Width="40px" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="qtdetotal" HeaderText="Qtde Total" Visible="true">
                                <ItemStyle Width="80px" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="qtdeRecebida" HeaderText="Qtd.Rec.">
                                <ItemStyle Width="20px" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="dataValidade" HeaderText="Validade" Visible="false">
                                <ItemStyle Width="30px" HorizontalAlign="Right" />
                            </asp:BoundField>
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
            <asp:Panel ID="pnForcarRecebimento" runat="server" >
                <asp:Button ID="btnForcarRecebimento" Text="Forçar Recebimento" OnClick="btnForcarRecebimento_Click" OnClientClick="javascript:return confirm('Confirma recebimento da NFe mesmo com críticas?');" runat="server" BackColor="Green" ForeColor="White" />
                <asp:Button ID="btnRecuperarContagem" Text="Recuperar Contagem" OnClick="btnRecuperarContagem_Click" OnClientClick="javascript:return confirm('Confirma recuperação da contagem?');" runat="server" BackColor="Blue" ForeColor="White" />
            </asp:Panel>

        </div>
    </form>
</body>
</html>

