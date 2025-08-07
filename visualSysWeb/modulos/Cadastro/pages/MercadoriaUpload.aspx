<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MercadoriaUpload.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.MercadoriaUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--<link rel="stylesheet" href="HTTP://blueimp.github.io/Gallery/css/blueimp-gallery.min.css" />
    --%>
    <link href="../../../Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-default" style="width: 80%; display: block; margin: 10%;">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
                EnableScriptLocalization="true" EnablePageMethods="true">
            </asp:ScriptManager>

            <div class="row" style="margin-left: 30px;">

                <asp:Label ID="lblErro" runat="server" Text=""></asp:Label>
            </div>
            <div class="panel-heading">
                <h1>Carregar Imagem  </h1>

            </div>
            <asp:Label ID="lblDetalhes" runat="server" Text=""></asp:Label>
            <asp:Timer ID="TimerImporta" runat="server" OnTick="TimerImporta_Tick" Enabled="false"
                Interval="100000">
            </asp:Timer>
            <div class="panel-body">
                <div id="Div1" runat="server" class="container" style="width: 100%;">
                    <div class=" col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="panel panel-default" style="width: 100%;">
                            <div class="panel-heading">
                                <h4>Informe uma Url </h4>
                            </div>
                            <div class="panel-body">
                                <div id="divUrl" runat="server" class="row">
                                    <div class="col-lg-8 col-md-18 col-sm-8 col-xs-8">
                                        <asp:TextBox ID="txtUrl" runat="server" CssClass="form-control" />
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                        <asp:Button ID="btnUrl" runat="server" CssClass="btn btn-default" Text="Salvar Imagem"
                                            OnClick="btnUrl_Click" />
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="panel-body">
                <div id="conteudo" runat="server" class="container" style="width: 100%;">
                    <div class=" col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="panel panel-default" style="width: 100%;">
                            <div class="panel-heading">
                                <h4>Ou Faça o Upload do um arquivo</h4>
                            </div>
                            <div class="panel-body">
                                <div id="divProcessa" runat="server" class="row">
                                    <div class="col-lg-8 col-md-18 col-sm-8 col-xs-8">
                                        <div class="input-group">
                                            <span class="input-group-btn"><span class="btn btn-primary btn-file" style="margin-left: 0px;" onclick="jsBtn('FileArquivo')">SELECIONE O ARQUIVO 
                                                <asp:FileUpload ID="FileArquivo" runat="server" AllowMultiple="false" Style="display: none;" />
                                            </span></span>
                                            <input id="txt" type="text" class="form-control" readonly>
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                        <asp:Button ID="btnArquivo" runat="server" CssClass="btn btn-default" Text="Salvar Imagem"
                                            OnClick="btnArquivo_Click" />
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="TimerImporta" EventName="Tick" />
                        </Triggers>
                        <ContentTemplate>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                    <asp:Button ID="btnCancela" runat="server" CssClass="btn btn-danger form-control"
                        Text="Cancelar e voltar"
                        Style="margin: 5%; width: 50%;"
                        OnClick="btnCancela_Click" />
                </div>
            </div>
        </div>
        </div>
    </form>

    <script src="../../../Scripts/jquery-1.4.1.min.js"></script>

    <script>
        function jsBtn(btn) {
            document.getElementById(btn).click();
        }

        $("#FileArquivo").change(function () {
            $("#txt").val(this.files[0].name);
        });
    </script>
</body>
</html>
