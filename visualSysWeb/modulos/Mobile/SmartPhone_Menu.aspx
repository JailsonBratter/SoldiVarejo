<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmartPhone_Menu.aspx.cs" Inherits="visualSysWeb.modulos.Mobile.pages.SmartPhone_Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<script>
    function doClick(tela) {
        if (tela == '3') {
            window.open("SmartPhone_Espelho.aspx");
        }
        else {
            window.open("SmartPhone_Resultado.aspx?tela=" + tela);
        }
    }
</script>
<style>
    .container{
        width:100vw;
        height:100vh;
        background: white;
        display:table;
        flex-direction: column;
        justify-content:center;
        align-items:center;
    }
    .box {
        padding:12px 12px 12px 12px;
        margin:5px;
        width:100%;
        color:white;
        font-size:100px;
        vertical-align:middle;
        display:table-cell;
        font-family:'Century Gothic';
        text-align:center;
        float:left;        
    }
    .body{
        margin:0px;
    }

</style>
    <body>
        <div class="container">
            <!--
            <div class="box" style="background:#0080FF" onclick="doClick('0')" >
                <p>Faturamento</p>
            </div>
            <div class="box" style="background:#e04040" onclick="doClick('1')">
                <p>Contas a Pagar</p>
            </div>
            <div class="box" style="background:#67cc3e" onclick="doClick('2')">
                <p >Contas a Receber</p>
            </div> -->
            <div class="box" style="background:#17202A" onclick="doClick('3')">
                <p >Entrada NFe</p>
            </div>
            <!-- <div class="box"><p><a href="./pages/Mobile_Vendas.aspx">VENDAS</a></p></div> -->
        </div>
   </body>
</html>
