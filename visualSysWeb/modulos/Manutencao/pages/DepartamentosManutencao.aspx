<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DepartamentosManutencao.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.DepartamentosManutencao" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../../Styles/bootstrap.css" rel="stylesheet" />
    <link href="../css/DepartamentosManutencao.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row text-center">
            <h1>Departamento Manutenção de Itens</h1>
            <hr />
        </div>

        <div class="row">

            <div class="col-md-5 ">


                <div class="panel panel-default" style="width: 100%;">
                    <div class="row">

                        <div class="col-lg-3">
                            <label>Grupo</label>
                        </div>
                        <div class="col-lg-9">
                            <asp:TextBox ID="txtCodGrupo1" runat="server" Width="30%" AutoPostBack="true"  OnTextChanged="txt_TextChanged"/>
                            <asp:TextBox ID="txtDescricaoGrupo1" runat="server" Width="60%" Enabled="false" />
                              <asp:ImageButton ID="imgBtnGrupo1" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                Width="15px" OnClick="imgLista_Click" />
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-lg-3">
                            <label>SubGrupo</label>
                        </div>
                        <div class="col-lg-9">
                            <asp:TextBox ID="txtCodSubGrupo1" runat="server" Width="30%" AutoPostBack="true"  OnTextChanged="txt_TextChanged" />
                            <asp:TextBox ID="txtDescricaoSubGrupo1" runat="server" Width="60%" Enabled="false" />
                              <asp:ImageButton ID="imgBtnSubGrupo1" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                Width="15px" OnClick="imgLista_Click" />
                        </div>
                    </div>
                    <div class="row">

                        <div class="col-lg-3">
                            <label>Departamento</label>
                        </div>
                        <div class="col-lg-9">
                            <asp:TextBox ID="txtCodDepartamento1" runat="server" Width="30%"  AutoPostBack="true"  OnTextChanged="txt_TextChanged"/>
                            <asp:TextBox ID="txtDescricaoDepartamento1" runat="server" Width="60%" Enabled="false" />
                              <asp:ImageButton ID="imgBtnDepartamento1" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                Width="15px" OnClick="imgLista_Click" />
                        </div>
                    </div> 
                    <div class="row">

                        <div class="col-lg-3">
                            <label>Descrição</label>
                        </div>
                        <div class="col-lg-9">
                            <asp:TextBox ID="txtDescricao1" runat="server" Width="100%"  AutoPostBack="true" OnTextChanged="txtDescricao1_TextChanged" />
                              
                        </div>
                    </div> 
                    
                </div>


            </div>
            <div class="col-md-1 colunaCentro ">
            </div>
            <div class="col-md-5">
                <div class="panel panel-default" style="width: 100%;">
                    <div class="row">

                        <div class="col-lg-3">
                            <label>Grupo</label>
                        </div>
                        <div class="col-lg-9">
                            <asp:TextBox ID="txtCodGrupo2" runat="server" Width="30%" AutoPostBack="true"  OnTextChanged="txt_TextChanged" />
                            <asp:TextBox ID="txtDescricaoGrupo2" runat="server" Width="60%" Enabled="false" />
                            <asp:ImageButton ID="imgBtnGrupo2" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                Width="15px" OnClick="imgLista_Click" />
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-lg-3">
                            <label>SubGrupo</label>
                        </div>
                        <div class="col-lg-9">
                            <asp:TextBox ID="txtCodSubGrupo2" runat="server" Width="30%" AutoPostBack="true"  OnTextChanged="txt_TextChanged" />
                            <asp:TextBox ID="txtDescricaoSubGrupo2" runat="server" Width="60%" Enabled="false" />
                            <asp:ImageButton ID="imgBtnSubGrupo2" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                Width="15px" OnClick="imgLista_Click" />
                        </div>
                    </div>
                    <div class="row">

                        <div class="col-lg-3">
                            <label>Departamento</label>
                        </div>
                        <div class="col-lg-9">
                            <asp:TextBox ID="txtCodDepartamento2" runat="server" Width="30%"  AutoPostBack="true"  OnTextChanged="txt_TextChanged"/>
                            <asp:TextBox ID="txtDescricaoDepartamento2" runat="server" Width="60%" Enabled="false" />
                            <asp:ImageButton ID="imgBtnDepartamento2" runat="server" ImageUrl="~/img/pesquisaM.png" Height="15px"
                                Width="15px" OnClick="imgLista_Click" />
                        </div>
                    </div>
                    <div class="row">

                        <div class="col-lg-3">
                            <label>Descrição</label>
                        </div>
                        <div class="col-lg-9">
                            <asp:TextBox ID="txtDescricao2" runat="server" Width="100%" AutoPostBack="true" OnTextChanged="txtDescricao2_TextChanged"  />
                              
                        </div>
                    </div>
                </div>


            </div>
        </div>
        <div class="row">
            <div class="col-lg-5" style="overflow:auto;height:500px;"">
                <asp:GridView ID="gridProdutos1" runat="server" AutoGenerateColumns="false" CssClass="table">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="true" OnCheckedChanged="chkSeleciona1_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelecionaItem" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="plu" HeaderText="PLU" />
                        <asp:BoundField DataField="ean" HeaderText="EAN" />
                        <asp:BoundField DataField="ref" HeaderText="REF" />
                        <asp:BoundField DataField="descricao" HeaderText="DESCRICAO" />
                        <asp:BoundField DataField="preco" HeaderText="PRECO" 
                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />


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
            <div class="col-md-1 colunaCentro">

                <div class="row text-center">
                    <div class="col-md-12">

                        <asp:Button ID="btnEsquerda" runat="server" Text="←" OnClick="btnEsquerda_Click"  CssClass="btn btn-default" Font-Size="XX-Large" />

                    </div>
                </div>
                <div class="row text-center">
                    <asp:Button ID="btnDireita" runat="server" Text="→" OnClick="btnDireita_Click" CssClass="btn btn-default" Font-Size="XX-Large" />
                </div>
            </div>
            <div class="col-lg-5" style="overflow:auto; height:500px;">
                <asp:GridView ID="gridProdutos2" runat="server" AutoGenerateColumns="false" CssClass="table">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSeleciona" runat="server" AutoPostBack="true" OnCheckedChanged="chkSeleciona2_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelecionaItem" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="plu" HeaderText="PLU" />
                        <asp:BoundField DataField="ean" HeaderText="EAN" />
                        <asp:BoundField DataField="ref" HeaderText="REF" />
                        <asp:BoundField DataField="descricao" HeaderText="DESCRICAO" />
                        <asp:BoundField DataField="preco" HeaderText="PRECO" 
                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
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



    </div>


     <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px;">
            <center><h1><asp:Label ID="lbltituloLista" runat="server" Text="" ></asp:Label></h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                Filtrar
                <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="400px"></asp:TextBox>
                <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="ImgPesquisaLista_Click" />
                <asp:Label ID="lblErroPesquisa" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                <div class="row1" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaLista_Click" />
                    <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row1">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="Label5" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CssClass="table"
                    OnRowDataBound="GridLista_RowDataBound" >
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="RdoListaItem" runat="server" GroupName="GrlistaItem" />
                            </ItemTemplate>
                        </asp:TemplateField>
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
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalLista" runat="server" BackgroundCssClass="modalBackground"
         DropShadow="true" PopupControlID="pnFundo" TargetControlID="lbltituloLista">
    </asp:ModalPopupExtender>

     <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="height: 120px;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap;
                        justify-content: center; height: 50px; margin-bottom: 20px;">
                        <asp:Button ID="btnOkError" runat="server" Text="OK" Width="200px" Height="100%"
                            Font-Size="Larger" OnClick="btnOkError_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalError" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnError" TargetControlID="lblErroPanel">
    </asp:ModalPopupExtender>

</asp:Content>
