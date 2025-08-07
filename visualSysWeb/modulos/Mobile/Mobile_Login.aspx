<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mobile_Login.aspx.cs" Inherits="visualSysWeb.modulos.mobile.pages.Mobile_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="../../Styles/bootstrap.css" rel="stylesheet" />
    <link href="../../Styles/mobile.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-sm-4 text-right">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Img/Logo.jpg"
                            Width="100%" />

                    </div>

                    <div class="col-sm-8">
                        <br><br>
                        <h1>Soldi Varejo </h1>
                    </div>
                </div>
            </div>
            <hr>
            <div class="panel-body">
                <div class="container">

                    <div class="row">
                        <asp:Label ID="lblerros" runat="server" CssClass="alert-danger"></asp:Label>
                    </div>
                    <div class="row">
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <img src="../../img/ico_empresa.jpg" class="img2" />
                                <label>
                                    Filial
                                </label>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-sm-12 text-center">
                                <asp:DropDownList ID="ddfilial" runat="server" CssClass="form-control"
                                    Font-Size="90px" Height="150px"
                                    OnSelectedIndexChanged="ddfilial_SelectedIndexChanged" 
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <img src="../../img/User.png" class="img2" />
                                <label>
                                    Usuario
                                </label>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-sm-12 text-center">
                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"
                                    Font-Size="90px" Height="150px"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">


                        <div class="col-sm-12">
                            <div class="row">
                                <div class="col-sm-12 text-center">
                                    <img src="../../img/senha.png" class="img2" />
                                    <label>
                                        Senha
                                    </label>
                                </div>
                            </div>
                            <div class="row">

                               
                            <asp:TextBox ID="txtSenha" runat="server" TextMode="Password"
                                AutoPostBack="true" OnTextChanged="txtSenha_TextChanged"
                                CssClass="form-control"  Font-Size="90px" Height="150px"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 btn-block" style="margin-top:150px; margin-bottom:150px;">
                            <asp:Button ID="btnEntrar" runat="server" Text="Entrar" OnClick="LoginButton_Click"
                                CssClass="btn btn-block text-center" Font-Size="80px" Height="150px" Width="100%" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
