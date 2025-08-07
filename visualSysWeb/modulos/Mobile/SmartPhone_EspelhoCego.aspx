<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmartPhone_EspelhoCego.aspx.cs" Inherits="visualSysWeb.modulos.Mobile.SmartPhone_EspelhoCego" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Styles/bootstrap.css" rel="stylesheet" />
    <link href="../../Styles/mobile.css" rel="stylesheet" />

    <script>

        function fechar()
        {
            this.close();
            window.Close();
            //alert(window.history.length);
            history.go((window.history.length -1) * -1);
            //history.go(-1);
        }
        
        function calculaQtde() {
            var vQtde = document.getElementById("txtQuantidade").value.replace(',', '.');//   getClienteId("txtQuantidade").value.replace(',', '.');
            var vQtdeEmbalagem = document.getElementById("txtEmbalagem").value.replace(',', '.');
            var vQtdeTotal = 0;
            if (vQtde > 0 && vQtdeEmbalagem > 0) {
                vQtdeTotal = (Number(vQtde) * Number(vQtdeEmbalagem));
            }
            else {
                vQtdeTotal = 0;
            }
            document.getElementById("txtQuantidadeTotal").value = Number(vQtdeTotal).toFixed(0).replace(".", ",");
            if ((!vQtdeTotal > 0)) {
                //alert('vou ocultar');
                //documnet.getElementById("btnFechar").dispaly = "none";
                //documnet.getElementById("btnFechar").dispaly = "hidden";
            }
            else {
                //alert('vou mostrar');
                //documnet.getElementById("btnFechar").dispaly = "visible ";
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


            if (tecla == BACKSPACE || tecla == 13) {
                return true;

            }


            return (isNum(key));

        }
        

    </script>

</head>
<body>
    <div>
        <panel id="pnTotal">
            <div>
            <div>
                <h1 class="titulo">
                    <asp:Label ID="lblTitulo" runat="server" Text=""></asp:Label>
                </h1>
                <div>
                    <div class="row">
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <img src="../../img/BarCodeIcone.png" class="img2" style="si" />
                                <label>
                                    Código de Barras
                                </label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <asp:TextBox ID="txtcodigo" runat="server" CssClass="form-control"
                                    Font-Size="90px" Height="150px" OnTextChanged="buscaCodigo"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div  class="row">
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <div class="col-sm-12 text-center">
                                    <asp:Label ID="lblDescricao" Text="Descrição do produto" runat="server" CssClass="form-control"
                                        Font-Size="60px" Height="150px"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div  class="row">
                        <br />
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <p>Quantidade dentro de cada pacote</p>
                                <asp:TextBox ID="txtEmbalagem" runat="server" CssClass="form-control" 
                                    Font-Size="90px" Height="150px"  OnChange="javascript:calculaQtde();"></asp:TextBox>
                            </div>
                            <div class="col-sm-12 text-center">
                                <p>Quantidade de pacotes</p>
                                <asp:TextBox ID="txtQuantidade" runat="server" CssClass="form-control" 
                                    Font-Size="90px" Height="150px" OnChange="javascript:calculaQtde();"></asp:TextBox>
                            </div>
                            <div class="col-sm-12 text-center">
                                <p>Quantidade Total</p>
                                <asp:TextBox ID="txtQuantidadeTotal" runat="server" CssClass="form-control"
                                    Font-Size="90px" Height="150px"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div  class="row">
                        <div class="col-sm-12 text-center">
                            <asp:Button ID="btnFechar" Text="Fechar" runat="server"  OnClientClick="fechar()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </panel>
    </div>
</body>
</html>
