<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PlanosDeContasContabil.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.PlanoContasContabil" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../css/PlanosDeContasContabil.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenuPlanoDeContas">
        <center>
            <h1>
                Plano de contas contábil</h1>
        </center>
    </div>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div class="rowFlex" style="flex-wrap: wrap;">
        <div id="Div1" runat="server" class="framePlanoContabil">
            <asp:Panel ID="PnPlanoNivel1" runat="server" CssClass="DivPlano" >
                <center>
                    <h1>
                        Plano Nível 1</h1>
                </center>
                <table>
                    <tr>
                        <td>
                            <p>
                                ID
                            </p>
                            <asp:TextBox ID="txtIdNivel1" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Codigo
                            </p>
                            <asp:TextBox ID="txtCodigoNivel1" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Descrição
                            </p>
                            <asp:TextBox ID="txtDescricaoNivel1" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                        </td>
                   
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnNivel1Editar" runat="server" ImageUrl="~/img/edit.png"
                                Width="25px" OnClick="ImgBtnNivel1Editar_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="imgBtnNivel1Excluir" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="imgBtnNivel1Excluir_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnNIvel1Add" runat="server" ImageUrl="~/img/add.png" Width="25px"
                                OnClick="ImgBtnNivel1Add_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnGridNivel1" runat="server" CssClass="GridContas">
                    <asp:GridView ID="GridNivel1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Vertical" OnRowDataBound="GridNivel1_RowDataBound">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="RdoItem" runat="server" GroupName="GrGrupo" OnCheckedChanged="RdoNivel1Item_CheckedChanged"
                                        AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="id" HeaderText="Id" />
                            <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                            <asp:BoundField DataField="descricao" HeaderText="Grupo" />
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
         </div>
        <div id="Div2" runat="server" class="framePlanoContabil">
            <asp:Panel ID="PnPlanoNivel2" runat="server" CssClass="DivPlano" >
                <center>
                    <h1>
                        Plano Nível 2</h1>
                </center>
                <table>
                    <tr>
                        <td>
                            <p>
                                ID
                            </p>
                            <asp:TextBox ID="txtIdNivel2" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Codigo
                            </p>
                            <asp:TextBox ID="txtCodigoNivel2" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Descrição
                            </p>
                            <asp:TextBox ID="txtDescricaoNivel2" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnNivel2Editar" runat="server" ImageUrl="~/img/edit.png"
                                Width="25px" OnClick="ImgBtnNivel2Editar_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="imgBtnNivel2Excluir" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="imgBtnNivel2Excluir_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnNIvel2Add" runat="server" ImageUrl="~/img/add.png" Width="25px"
                                OnClick="ImgBtnNivel2Add_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnGridNivel2" runat="server" CssClass="GridContas">
                    <asp:GridView ID="GridNivel2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Vertical" OnRowDataBound="GridNivel2_RowDataBound">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="RdoItem" runat="server" GroupName="GrGrupo" OnCheckedChanged="RdoNivel2Item_CheckedChanged"
                                        AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="id" HeaderText="Id" />
                            <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                            <asp:BoundField DataField="descricao" HeaderText="Grupo" />
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
         </div>
        <div id="Div3" runat="server" class="framePlanoContabil">
            <asp:Panel ID="PnPlanoNivel3" runat="server" CssClass="DivPlano" >
                <center>
                    <h1>
                        Plano Nível 3</h1>
                </center>
                <table>
                    <tr>
                        <td>
                            <p>
                                ID
                            </p>
                            <asp:TextBox ID="txtIdNivel3" runat="server" Width="50px" ></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Codigo
                            </p>
                            <asp:TextBox ID="txtCodigoNivel3" runat="server" Width="50px" ></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Descrição
                            </p>
                            <asp:TextBox ID="txtDescricaoNivel3" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnNivel3Editar" runat="server" ImageUrl="~/img/edit.png"
                                Width="25px" OnClick="ImgBtnNivel3Editar_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="imgBtnNivel3Excluir" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="imgBtnNivel3Excluir_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnNIvel3Add" runat="server" ImageUrl="~/img/add.png" Width="25px"
                                OnClick="ImgBtnNivel3Add_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnGridNivel3" runat="server" CssClass="GridContas">
                    <asp:GridView ID="GridNivel3" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Vertical" OnRowDataBound="GridNivel3_RowDataBound">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="RdoItem" runat="server" GroupName="GrGrupo" OnCheckedChanged="RdoNivel3Item_CheckedChanged"
                                        AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="id" HeaderText="Id" />
                            <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                            <asp:BoundField DataField="descricao" HeaderText="Grupo" />
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
         </div>
        <div id="Div4" runat="server" class="framePlanoContabil">
            <asp:Panel ID="PnPlanoNivel4" runat="server" CssClass="DivPlano" >
            <center>
                <h1>
                    Plano Nível 4</h1>
            </center>
            <table>
                <tr>
                    <td>
                        <p>
                            ID
                        </p>
                        <asp:TextBox ID="txtIdNivel4" runat="server" Width="50px" ></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoNivel4" runat="server" Width="50px" ></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricaoNivel4" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="ImgBtnNivel4Editar" runat="server" ImageUrl="~/img/edit.png"
                            Width="25px" OnClick="ImgBtnNivel4Editar_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgBtnNivel4Excluir" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="imgBtnNivel4Excluir_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="ImgBtnNIvel4Add" runat="server" ImageUrl="~/img/add.png" Width="25px"
                            OnClick="ImgBtnNivel4Add_Click" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnGridNivel4" runat="server" CssClass="GridContas">
                <asp:GridView ID="GridNivel4" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" GridLines="Vertical" OnRowDataBound="GridNivel4_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="RdoItem" runat="server" GroupName="GrGrupo" OnCheckedChanged="RdoNivel4Item_CheckedChanged"
                                    AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="id" HeaderText="Id" />
                        <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                        <asp:BoundField DataField="descricao" HeaderText="Grupo" />
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
        </div>
        <div id="Div5" runat="server" class="framePlanoContabil">
                <asp:Panel ID="PnPlanoNivel5" runat="server" CssClass="DivPlano" >
                    <center>
                        <h1>
                            Plano Nível 5</h1>
                    </center>
                    <table>
                        <tr>
                            <td>
                                <p>
                                    ID
                                </p>
                                <asp:TextBox ID="txtIdNivel5" runat="server" Width="50px" ></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Codigo
                                </p>
                                <asp:TextBox ID="txtCodigoNivel5" runat="server" Width="50px" ></asp:TextBox>
                            </td>
                            <td>
                                <p>
                                    Descrição
                                </p>
                                <asp:TextBox ID="txtDescricaoNivel5" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                            </td>
                            <td>
                                <br />
                                <asp:ImageButton ID="ImgBtnNivel5Editar" runat="server" ImageUrl="~/img/edit.png"
                                    Width="25px" OnClick="ImgBtnNivel5Editar_Click" />
                            </td>
                            <td>
                                <br />
                                <asp:ImageButton ID="imgBtnNivel5Excluir" runat="server" ImageUrl="~/img/cancel.png"
                                    Width="25px" OnClick="imgBtnNivel5Excluir_Click" />
                            </td>
                            <td>
                                <br />
                                <asp:ImageButton ID="ImgBtnNIvel5Add" runat="server" ImageUrl="~/img/add.png" Width="25px"
                                    OnClick="ImgBtnNivel5Add_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnGridNivel5" runat="server" CssClass="GridContas">
                        <asp:GridView ID="GridNivel5" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            ForeColor="#333333" GridLines="Vertical" OnRowDataBound="GridNivel5_RowDataBound">
                            <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:RadioButton ID="RdoItem" runat="server" GroupName="GrGrupo" OnCheckedChanged="RdoNivel2Item_CheckedChanged"
                                            AutoPostBack="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="id" HeaderText="Id" />
                                <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                                <asp:BoundField DataField="descricao" HeaderText="Grupo" />
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
             </div>
    </div>


    <asp:Panel ID="PnDetalhes" runat="server" CssClass="modalForm" Style="display: none; height:200px; width:600px">
        <asp:Panel ID="PnDetalhesNivel1Frame" runat="server" CssClass="frame" style="padding:20px; width:90%; height:70%">
            <div class="rowFlex" >
                <div style="flex: 1 ; border-bottom:solid 1px">
                    <h1 style="text-align:center">Plano Conta Contábil</h1>
                    <asp:Label ID="lblErroDetalhes" runat="server" Text=""></asp:Label>    
                </div>
                
            </div>
            <div class="rowFlex" >
                    <div class="col1" style="padding-left:20%">
                        <asp:ImageButton ID="btnConfirmaDetalhes" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaDetalhes_Click" />
                        <asp:Label ID="Label8" runat="server" Text="Confirma"></asp:Label>
                    </div>
                    <div class="col1" style="padding-left:20%">
                        <asp:ImageButton ID="btnCancelaDetalhes" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaDetalhes_Click" />
                        <asp:Label ID="Label9" runat="server" Text="Cancela"></asp:Label>
                    </div>
            </div>
            <div class="rowFlex">
                <div id="divCodigoPaiDetalhes" runat="server" class="col1">
                    <p>
                        Codigo Pai
                    </p>
                    <asp:TextBox ID="txtCodigoPlanoPaiDetalhe" runat="server" Width="100%" Enabled="false" ></asp:TextBox>
                </div>
                <div class="col1">
                    <p>
                        ID
                    </p>
                    <asp:TextBox ID="txtIdDetalhe" runat="server" Width="100%" MaxLength="3"></asp:TextBox>
                </div>
                <div class="col2">
                    <p>
                        Codigo
                    </p>
                    <asp:TextBox ID="txtCodigoDetalhe" runat="server" Width="100%" MaxLength="30"></asp:TextBox>
                </div>
                <div class="col3">
                    <p>
                        Descrição
                    </p>
                    <asp:TextBox ID="txtDescricaoDetalhe" runat="server" Width="100%" MaxLength="20"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalDetalhe" runat="server"
        PopupControlID="PnDetalhes"
        TargetControlID="lblErroDetalhes">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnError" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="overflow: auto; min-height: 100px;">
            <h2>
                <asp:Label ID="lblErroPanel" runat="server" Text="" ForeColor="Red"></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 30px; margin-bottom: 20px;">
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

    <asp:Panel ID="pnConfirmaExcluir" runat="server" CssClass="frameModal" Style="display: none">
        <div class="cabMenu" style="overflow: auto; min-height: 100px;">
            <h2>Tem certeza que gostaria de Excluir 
                <asp:Label ID="lblExcluirTitle" runat="server" Text="" ></asp:Label>
            </h2>
        </div>
        <table class="frame" style="width: 100%;">
            <tr>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 30px; margin-bottom: 20px;">
                        <asp:Button ID="btnConfirmaExcluir" runat="server" Text="Confirma" Width="200px" Height="100%"
                            Font-Size="Larger" OnClick="btnConfirmaExcluir_Click" />
                    </div>
                </td>
                <td>
                    <div style="align-items: center; display: flex; flex-direction: row; flex-wrap: wrap; justify-content: center; height: 30px; margin-bottom: 20px;">
                        <asp:Button ID="btnCancelarExcluir" runat="server" Text="Cancelar" Width="200px" Height="100%"
                            Font-Size="Larger" OnClick="btnCancelarExcluir_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalExcluirPlano" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaExcluir" TargetControlID="lblExcluirTitle">
    </asp:ModalPopupExtender>


</asp:Content>
