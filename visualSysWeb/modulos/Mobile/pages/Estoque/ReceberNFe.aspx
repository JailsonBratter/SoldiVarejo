<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReceberNFe.aspx.cs" Inherits="visualSysWeb.modulos.Mobile.pages.Estoque.ReceberNFe" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #iptBarra {
            width: 369px;
        }
    </style>
</head>
<script type="text/javascript">
    function setarValor() {
        try {
            var x = localStorage.getItem('barraLida');
            document.getElementById("<%=txtChave.ClientID %>").Value = localStorage.getItem('barraLida');
            alert(x);
        }
        catch (e) {
            alert('erro:' + e);
        }
    }
</script>

<body>
    <script src="../../js/quagga.min.js" type="text/javascript">></script>
    <script src="../../js/LerBarcodeQuagga.js" type="text/javascript">></script>


    <form id="form1" runat="server">


        <div id="camera">
            <center>
            <p>Comunicar recebimento de NFe</p>
            <asp:TextBox ID="txtChave" runat="server" Width="300px"  ></asp:TextBox>
            <asp:Button ID="btnImportar" runat="server" Text="Importar"  OnClick ="btnImportar_Click"/>
            <br />
            <asp:Label ID="lblMensagem" runat="server" Text="" ForeColor="Blue">Mensagem:</asp:Label>
            </center>

        </div>
        <asp:Timer ID="Timer1" runat="server" Interval="2000" OnTick="Timer1_Tick">
        </asp:Timer>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>   

    </form>
</body>
</html>
