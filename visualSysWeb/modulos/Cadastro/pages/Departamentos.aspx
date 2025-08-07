<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Departamentos.aspx.cs" Inherits="visualSysWeb.modulos.Cadastro.pages.Departamentos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cabMenuPlanoDeContas">
        <center>
            <h1>
                Departamentos</h1>
        </center>
    </div>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    <div id="Div1" runat="server" class="FramePlanoDeContas">
        <asp:Panel ID="PnGrupo" runat="server" CssClass="DivPlano" DefaultButton="ImgBtnGrupoPesquisar">
            <center>
                <h1>
                    Grupos</h1>
            </center>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoGrupo" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricaoGrupo" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="ImgBtnGrupoPesquisar" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Width="25px" OnClick="ImgBtnGrupoPesquisar_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="ImgBtnGrupoEditar" runat="server" ImageUrl="~/img/edit.png"
                            Width="25px" OnClick="ImgBtnGrupoEditar_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgBtnGrupoExcluir" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="imgBtnGrupoExcluir_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="ImgBtnGrupoAdd" runat="server" ImageUrl="~/img/add.png" Width="25px"
                            OnClick="ImgBtnGrupoAdd_Click" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnGridGrupos" runat="server" CssClass="GridContas">
                <asp:GridView ID="GridGrupos" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" GridLines="Vertical" OnRowDataBound="GridGrupos_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="RdoGrupoItem" runat="server" GroupName="GrGrupo" OnCheckedChanged="RdoGrupoItem_CheckedChanged"
                                    AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="codigo_grupo" HeaderText="Codigo" />
                        <asp:BoundField DataField="descricao_grupo" HeaderText="Grupo" />
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
        <asp:Panel ID="PnSubGrupo" runat="server" CssClass="DivPlano">
            <center>
                <h1>
                    SubGrupos</h1>
            </center>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoSubGrupo" runat="server" Width="50px" MaxLength="6"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricaoSubGrupo" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgBtnSubGrupoPesquisa" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Width="25px" OnClick="imgBtnSubGrupoPesquisa_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgBtnSubGrupoEditar" runat="server" ImageUrl="~/img/edit.png"
                            Width="25px" OnClick="imgBtnSubGrupoEditar_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgBtnSubGrupoExcluir" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="imgBtnSubGrupoExcluir_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgBtnSubGrupoAdd" runat="server" ImageUrl="~/img/add.png" Width="25px"
                            OnClick="imgBtnSubGrupoAdd_Click" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PnGridSubGrupos" runat="server" CssClass="GridContas">
                <asp:GridView ID="GridSubGrupos" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" GridLines="Vertical" OnRowDataBound="GridSubGrupos_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="RdoSubGrupoItem" runat="server" GroupName="GrSubGrupo" OnCheckedChanged="RdoSubGrupoItem_CheckedChanged"
                                    AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="codigo_SubGrupo" HeaderText="Codigo" />
                        <asp:BoundField DataField="descricao_Subgrupo" HeaderText="SubGrupo" />
                        
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
        <asp:Panel ID="PnDepartamento" runat="server" CssClass="DivPlano">
            <center>
                <h1>
                    Departamentos</h1>
            </center>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoDepartamento" runat="server" Width="60px" MaxLength="9"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricaoDepartamento" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgDepartamentoPesquisar" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Width="25px" OnClick="imgDepartamentoPesquisar_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgDepartamentoEditar" runat="server" ImageUrl="~/img/edit.png"
                            Width="25px" OnClick="imgDepartamentoEditar_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgBtnDepartamentoExcluir" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="imgBtnDepartamentoExcluir_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:ImageButton ID="imgDepartamentoAdd" runat="server" ImageUrl="~/img/add.png"
                            Width="25px" OnClick="imgDepartamentoAdd_Click" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PnGridDepartamento" runat="server" CssClass="GridContas">
                <asp:GridView ID="GridDepartamento" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" GridLines="Vertical" OnRowDataBound="GridDepartamento_RowDataBound">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="RdoDepartamentoItem" runat="server" GroupName="GrDepartamento"
                                    OnCheckedChanged="RdoDepartamentoItem_CheckedChanged" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="codigo_Departamento" HeaderText="Codigo" />
                        <asp:BoundField DataField="descricao_Departamento" HeaderText="Departamento" />
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

        <div id="divCategoriasAll" runat="server" class="row" style="float: left;" visible="true">
            <div id="divCategoria" runat="server" class="DivPlano">
                <center>
                <h1>
                    Categorias</h1>
            </center>
                <table>
                    <tr>
                        <td>
                            <p>
                                Codigo
                            </p>
                            <asp:TextBox ID="txtCodigoCategoria" runat="server" Width="60px" MaxLength="9"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Descrição
                            </p>
                            <asp:TextBox ID="txtDescricaoCategoria" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnCategoriaPesquisa" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Width="25px" OnClick="ImgBtnCategoriaPesquisa_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnCategoriaEditar" runat="server" ImageUrl="~/img/edit.png"
                                Width="25px" OnClick="ImgBtnCategoriaEditar_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnCategoriaExcluir" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="ImgBtnCategoriaExcluir_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnCategoriaAdd" runat="server" ImageUrl="~/img/add.png"
                                Width="25px" OnClick="ImgBtnCategoriaAdd_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="PanelCategoria" runat="server" CssClass="GridContas">
                    <asp:GridView ID="gridCategorias" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Vertical" OnRowDataBound="gridCategorias_RowDataBound">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="RdoCategoriaItem" runat="server" GroupName="GrCategoria"
                                        OnCheckedChanged="RdoCategoriaItem_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                            <asp:BoundField DataField="descricao" HeaderText="Categoria" />
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
            </div>
            <div id="divSeguimento" runat="server" class="DivPlano">
                <center>
                    <h1>
                        Seguimento</h1>
                </center>
                <table>
                    <tr>
                        <td>
                            <p>
                                Codigo
                            </p>
                            <asp:TextBox ID="txtCodigoSeguimento" runat="server" Width="60px" MaxLength="9"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Descrição
                            </p>
                            <asp:TextBox ID="txtDescricaoSeguimento" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnSeguimentoPesquisa" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Width="25px" OnClick="ImgBtnSeguimentoPesquisa_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnSeguimentoEditar" runat="server" ImageUrl="~/img/edit.png"
                                Width="25px" OnClick="ImgBtnSeguimentoEditar_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnSeguimentoExcluir" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="ImgBtnSeguimentoExcluir_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnSeguimentoAdd" runat="server" ImageUrl="~/img/add.png"
                                Width="25px" OnClick="ImgBtnSeguimentoAdd_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="PanelSeguimento" runat="server" CssClass="GridContas">
                    <asp:GridView ID="gridSeguimento" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Vertical" OnRowDataBound="gridSeguimento_RowDataBound">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="RdoSeguimentoItem" runat="server" GroupName="GrSeguimento"
                                        OnCheckedChanged="RdoSeguimentoItem_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                            <asp:BoundField DataField="descricao" HeaderText="Categoria" />
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
            </div>
            <div id="divSubSeguimento" runat="server" class="DivPlano">
                <center>
                <h1>
                    Subseguimento</h1>
            </center>
                <table>
                    <tr>
                        <td>
                            <p>
                                Codigo
                            </p>
                            <asp:TextBox ID="txtCodigoSubseguimento" runat="server" Width="60px" MaxLength="9"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Descrição
                            </p>
                            <asp:TextBox ID="txtDescricaoSubseguimento" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnSubseguimentoPesquisa" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Width="25px" OnClick="ImgBtnSubseguimentoPesquisa_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnSubseguimentoEditar" runat="server" ImageUrl="~/img/edit.png"
                                Width="25px" OnClick="ImgBtnSubseguimentoEditar_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnSubseguimentoExcluir" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="ImgBtnSubseguimentoExcluir_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnSubseguimentoAdd" runat="server" ImageUrl="~/img/add.png"
                                Width="25px" OnClick="ImgBtnSubseguimentoAdd_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="PanelSubseguimento" runat="server" CssClass="GridContas">
                    <asp:GridView ID="gridSubSeguimento" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Vertical" OnRowDataBound="gridSubSeguimento_RowDataBound">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="RdoSubSeguimentoItem" runat="server" GroupName="GrSubSeguimento"
                                        OnCheckedChanged="RdoSubSeguimentoItem_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                            <asp:BoundField DataField="descricao" HeaderText="Subseguimento" />
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
            </div>
            <div id="divCategoriaGrupo" runat="server" class="DivPlano">
                <center>
                <h1>
                    Grupo</h1>
            </center>
                <table>
                    <tr>
                        <td>
                            <p>
                                Codigo
                            </p>
                            <asp:TextBox ID="txtCodigoCategoriaGrupo" runat="server" Width="60px" MaxLength="9"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Descrição
                            </p>
                            <asp:TextBox ID="txtDescricaoCategoriaGrupo" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnCategoriaGrupoPesquisa" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Width="25px" OnClick="ImgBtnCategoriaGrupoPesquisa_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnCategoriaGrupoEditar" runat="server" ImageUrl="~/img/edit.png"
                                Width="25px" OnClick="ImgBtnCategoriaGrupoEditar_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnCategoriaGrupoExcluir" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="ImgBtnCategoriaGrupoExcluir_Click1" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnCategoriaGrupoAdd" runat="server" ImageUrl="~/img/add.png"
                                Width="25px" OnClick="ImgBtnCategoriaGrupoAdd_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="PanelCategoriaGrupo" runat="server" CssClass="GridContas">
                    <asp:GridView ID="gridCategoriasGrupo" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Vertical" OnRowDataBound="gridCategoriasGrupo_RowDataBound">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="RdoCategoriaGrupoItem" runat="server" GroupName="GrCategoriaGrupo"
                                        OnCheckedChanged="RdoCategoriaGrupoItem_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                            <asp:BoundField DataField="descricao" HeaderText="Categoria" />
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
            </div>
            <div id="divCategoriaSubGrupo" runat="server" class="DivPlano">
                <center>
                <h1>
                    SubGrupo</h1>
            </center>
                <table>
                    <tr>
                        <td>
                            <p>
                                Codigo
                            </p>
                            <asp:TextBox ID="txtCodigoCategoriaSubGrupo" runat="server" Width="60px" MaxLength="9"></asp:TextBox>
                        </td>
                        <td>
                            <p>
                                Descrição
                            </p>
                            <asp:TextBox ID="txtDescricaoCategoriaSubGrupo" runat="server" Width="150px" MaxLength="20"> </asp:TextBox>
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnCategoriaSubGrupoPesquisa" runat="server" ImageUrl="~/img/pesquisaM.png"
                                Width="25px" OnClick="ImgBtnCategoriaSubGrupoPesquisa_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnCategoriaSubGrupoEditar" runat="server" ImageUrl="~/img/edit.png"
                                Width="25px" OnClick="ImgBtnCategoriaSubGrupoEditar_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnCategoriaSubGrupoExcluir" runat="server" ImageUrl="~/img/cancel.png"
                                Width="25px" OnClick="ImgBtnCategoriaSubGrupoExcluir_Click" />
                        </td>
                        <td>
                            <br />
                            <asp:ImageButton ID="ImgBtnCategoriaSubGrupoAdd" runat="server" ImageUrl="~/img/add.png"
                                Width="25px" OnClick="ImgBtnCategoriaSubGrupoAdd_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel1" runat="server" CssClass="GridContas">
                    <asp:GridView ID="gridCategoriasSubGrupo" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Vertical" OnRowDataBound="gridCategoriasSubGrupo_RowDataBound">
                        <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="RdoCategoriaSubGrupoItem" runat="server" GroupName="GrCategoriaSubGrupo"
                                        OnCheckedChanged="RdoCategoriaSubGrupoItem_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                            <asp:BoundField DataField="descricao" HeaderText="Categoria" />
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
            </div>

        </div>
    </div>
    <asp:Panel ID="PnDetalhesGrupo" runat="server" CssClass="modalForm" Style="display: none; height:200px">
        <asp:Panel ID="PnDetalhesGrupoFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Grupo</h1>
                <asp:Label ID="lblErroGrupoDetalhes" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnConfirmaDetalhesGrupo" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="btnConfirmaDetalhesGrupo_Click" />
                        <asp:Label ID="Label8" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnCancelaDetalhesGrupo" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="btnCancelaDetalhesGrupo_Click" />
                        <asp:Label ID="Label9" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoGrupoDetalhe" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricaoGrupoDetalhe" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalGrupoDetalhe" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblErroGrupoDetalhes" DropShadow="true" PopupControlID="PnDetalhesGrupo"
        TargetControlID="lblErroGrupoDetalhes">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnSubGrupoDetalhes" runat="server" CssClass="modalForm" Style="display: none; height: 250px;">
        <asp:Panel ID="pnSubGrupoDetalhesFrame" runat="server" CssClass="frame">
            <div class="cabMenu">
                <h1>Sub Grupo</h1>
                <asp:Label ID="lblErrorSubGrupoDetalhes" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="imgBtnConfirmaSubGrupoDetalhes" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="imgBtnConfirmaSubGrupoDetalhes_Click" />
                        <asp:Label ID="Label2" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="imgBtnCancelaSubGrupoDetalhes" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="imgBtnCancelaSubGrupoDetalhes_Click" />
                        <asp:Label ID="Label3" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodGrupoSubGrupo" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Grupo
                        </p>
                        <asp:TextBox ID="txtDescricaoGrupoSubGrupo" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoSubGrupoDetalhes" runat="server" Width="50px" MaxLength="6"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricaoSubGrupoDetalhes" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                    </td>
                    <tr>
                        <td>
                            <p>Codigo SAP</p>
                            <asp:TextBox ID="txtCodigo_SAP" runat="server" Width="80px" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalSubGrupoDetalhes" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblErrorSubGrupoDetalhes" DropShadow="true" PopupControlID="pnSubGrupoDetalhes"
        TargetControlID="lblErrorSubGrupoDetalhes">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnDepartamentoDetalhes" runat="server" CssClass="modalForm" Style="display: none; height: 600px; top: 0;">
        <asp:Panel ID="pnDepartamentoDetalhesFrame" runat="server" CssClass="frame" Style="height: 550px;">
            <div class="cabMenu">
                <h1>Departamento</h1>
                <asp:Label ID="lblDepartamentoErro" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="imgBtnConfirmaDepartamentoDetalhes" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="imgBtnConfirmaDepartamentoDetalhes_Click" />
                        <asp:Label ID="Label5" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="imgBtnCancelaDepartamentoDetalhes" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="imgBtnCancelaDepartamentoDetalhes_Click" />
                        <asp:Label ID="Label6" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodGrupoDepartamento" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Grupo
                        </p>
                        <asp:TextBox ID="txtDescricaoGrupoDepartamento" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodSubGrupoDepartamento" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            SubGrupo
                        </p>
                        <asp:TextBox ID="txtDescricaoSubGrupoDepartamento" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoDepartamentoDetalhe" runat="server" Width="80px" MaxLength="6"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Descrição
                        </p>
                        <asp:TextBox ID="txtDescricaoDepartamentoDetalhe" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkCardapio" Text="Cardapio" runat="server" Font-Size="20px" /></td>

                </tr>

            </table>

            <div class="panelItem">
                <asp:ImageButton ID="imgAddFornecedor" runat="server" ImageUrl="~/img/add.png" Width="25px"
                    OnClick="imgAddFornecedor_Click" />

                <asp:Label ID="lblIncluirFornecedor" runat="server" Text="Incluir Novo Fornecedor"></asp:Label>

            </div>
            <div class="gridTable" style="height: 150px; overflow: auto;">
                <asp:GridView ID="gridFornecedorDepartamento" runat="server" AutoGenerateColumns="False"
                    CellPadding="4" ForeColor="#333333" GridLines="Vertical" OnRowCommand="gridFornecedorDepartamento_RowCommand">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" CommandName="Excluir"
                            Text="Excluir">
                            <ControlStyle Height="20px" Width="20px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="Fornecedor" HeaderText="Fornecedor" />
                        <asp:BoundField DataField="CNPJ" HeaderText="CNPJ" />
                        <asp:BoundField DataField="Razao_social" HeaderText="Razao_social" />
                        <asp:BoundField DataField="Contrato" HeaderText="Contrato" />
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

            <div class="panelItem">
                <asp:ImageButton ID="imgBtAddImpressora" runat="server" ImageUrl="~/img/add.png" Width="25px"
                    OnClick="imgBtAddImpressora_Click" />

                <asp:Label ID="Label1" runat="server" Text="Incluir Impressora Remota"></asp:Label>

            </div>
            <div class="gridTable" style="height: 200px; overflow: auto;">
                <asp:GridView ID="gridImpressoras" runat="server" AutoGenerateColumns="False"
                    CellPadding="4" ForeColor="#333333" GridLines="Vertical" OnRowCommand="gridImpressoras_RowCommand">
                    <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/img/cancel.png" CommandName="Excluir"
                            Text="Excluir">
                            <ControlStyle Height="20px" Width="20px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="Loja" HeaderText="Loja" />
                        <asp:BoundField DataField="Filial" HeaderText="Filial" />
                        <asp:BoundField DataField="impressora_remota" HeaderText="Cod" />
                        <asp:BoundField DataField="descricao" HeaderText="Impressora" />


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
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalDepartamentoDetalhes" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="lblDepartamentoErro" DropShadow="true" PopupControlID="pnDepartamentoDetalhes"
        TargetControlID="lblDepartamentoErro">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnCategoria" runat="server" CssClass="modalForm" Style="display: none; height: 250px; top: 0;">
        <asp:Panel ID="Panel4" runat="server" CssClass="frame" Style="height: 200px;">
            <div class="cabMenu">
                <h1>
                    <asp:Label ID="lblIncluirCategoriaDetalhe" runat="server" Text=""></asp:Label>
                    Categoria</h1>
                <asp:Label ID="lblErrorCategoriaDetalhes" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="ImgBtnConfirmaCategoriaDetalhes" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="ImgBtnConfirmaCategoriaDetalhes_Click" />
                        <asp:Label ID="Label18" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgBtnCancelaCategoriaDetalhes" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="ImgBtnCancelaCategoriaDetalhes_Click" />
                        <asp:Label ID="Label19" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodigoGrupoCategoriaDetalhes" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Grupo
                        </p>
                        <asp:TextBox ID="txtDescricaoGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodSubGrupoCategoriaDetalhes" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            SubGrupo
                        </p>
                        <asp:TextBox ID="txtDescricaoSubGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodDepartamentoCategoriaDetalhes" runat="server" Width="100px" MaxLength="6" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Departamento
                        </p>
                        <asp:TextBox ID="txtDescricaoDepartamentoCategoriaDetalhes" runat="server" Width="200px" MaxLength="20" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodigoCategoriaDetalhes" runat="server" Width="100px" MaxLength="20" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Categoria</p>
                            <asp:TextBox ID="txtDescricaoCategoriaDetalhes" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                        </div>

                    </td>

                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalCategoriaDetalhes" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnCategoria"
        TargetControlID="lblErrorCategoriaDetalhes">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnSeguimento" runat="server" CssClass="modalForm" Style="display: none; height: 250px; top: 0;">
        <asp:Panel ID="Panel5" runat="server" CssClass="frame" Style="height: 200px;">
            <div class="cabMenu">
                <h1>
                    <asp:Label ID="lblIncluirSeguimentoDetalhe" runat="server" Text=""></asp:Label>
                    Seguimento</h1>
                <asp:Label ID="lblErrorSeguimentoDetalhes" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="ImgBtnConfirmaSeguimentoDetalhes" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="ImgBtnConfirmaSeguimentoDetalhes_Click" />
                        <asp:Label ID="LblConfirmaSeg" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgBtnCancelaSeguimentoDetalhes" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="ImgBtnCancelaSeguimentoDetalhes_Click1" />
                        <asp:Label ID="lblCancelaSeg" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtcodGrupoSeguimentoDetalhes" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Grupo
                        </p>
                        <asp:TextBox ID="txtDescGrupoSeguimentoDetalhes" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodSubGrupoSeguimentoDetalhes" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            SubGrupo
                        </p>
                        <asp:TextBox ID="txtDescSubGrupoSeguimentoDetalhes" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodDepartamentoSeguimentoDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="6"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Departamento
                        </p>
                        <asp:TextBox ID="txtDescDepartamentoSeguimentoDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodCategoriaSeguimentoDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Categoria</p>
                            <asp:TextBox ID="txtDescCategoriaSeguimentoDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodSeguimentoDetalhes" runat="server" Width="100px" MaxLength="20" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Seguimento</p>
                            <asp:TextBox ID="txtDescSeguimentoDetalhes" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                        </div>

                    </td>

                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalSeguiemntoDetalhes" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnSeguimento"
        TargetControlID="lblErrorSeguimentoDetalhes">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnSubSeguimento" runat="server" CssClass="modalForm" Style="display: none; height: 250px; top: 0; width: 99%;">
        <asp:Panel ID="Panel6" runat="server" CssClass="frame" Style="height: 200px;">
            <div class="cabMenu">
                <h1>
                    <asp:Label ID="lblIncluirSubSeguimento" runat="server" Text=""></asp:Label>
                    Sub Seguimento</h1>
                <asp:Label ID="lblErrorSubSeguimentoDetalhes" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="ImgBtnConfirmaSubSeguimentoDetalhes" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="ImgBtnConfirmaSubSeguimentoDetalhes_Click" />
                        <asp:Label ID="Label22" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgBtnCancelaSubSeguimentoDetalhes" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="ImgBtnCancelaSubSeguimentoDetalhes_Click" />
                        <asp:Label ID="Label23" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtcodGrupoSubSeguimentoDetalhes" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Grupo
                        </p>
                        <asp:TextBox ID="txtDescGrupoSubSeguimentoDetalhes" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodSubGrupoSubSeguimentoDetalhes" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            SubGrupo
                        </p>
                        <asp:TextBox ID="txtDescSubGrupoSubSeguimentoDetalhes" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodDepartamentoSubSeguimentoDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="6"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Departamento
                        </p>
                        <asp:TextBox ID="txtDescDepartamentoSubSeguimentoDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodCategoriaSubSeguimentoDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Categoria</p>
                            <asp:TextBox ID="txtDescCategoriaSubSeguimentoDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodSeguimentoSubSeguimentoDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Seguimento</p>
                            <asp:TextBox ID="txtDescSeguimentoSubSeguimentoDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodSubSeguimentoDetalhes" runat="server" Width="100px" MaxLength="20" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>SubSeguimento</p>
                            <asp:TextBox ID="txtDescSubSeguimentoDetalhes" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                        </div>

                    </td>

                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalSubSeguimentoDetalhes" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnSubSeguimento"
        TargetControlID="lblErrorSubSeguimentoDetalhes">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnGrupoCategoria" runat="server" CssClass="modalForm" Style="display: none; height: 300px; top: 0; width: 99%;">
        <asp:Panel ID="Panel7" runat="server" CssClass="frame" Style="height: 250px;">
            <div class="cabMenu">
                <h1>
                    <asp:Label ID="lblIncluirGrupoCategoria" runat="server" Text=""></asp:Label>
                    Grupo </h1>
                <asp:Label ID="lblErroGrupoCategoria" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="ImgBtnConfirmaGrupoCategoriaDetalhes" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="ImgBtnConfirmaGrupoCategoriaDetalhes_Click" />
                        <asp:Label ID="LblConfSubSeguimento" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgBtnCancelaGrupoCategoriaDetalhes" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="ImgBtnCancelaGrupoCategoriaDetalhes_Click" />
                        <asp:Label ID="lblCancelaGr" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtcodGrupoGrupoCategoriatoDetalhes" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Grupo
                        </p>
                        <asp:TextBox ID="txtDescGrupoGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodSubGrupoGrupoCategoriaDetalhes" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            SubGrupo
                        </p>
                        <asp:TextBox ID="txtDescSubGrupoGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodDepartamentoGrupoCategoriaDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="6"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Departamento
                        </p>
                        <asp:TextBox ID="txtDescDepartamentoGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodCategoriaGrupoCategoriaDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Categoria</p>
                            <asp:TextBox ID="txtDescCategoriaGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodSeguimentoGrupoCategoriaDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Seguimento</p>
                            <asp:TextBox ID="txtDescSeguimentoGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodSubSeguimentoGrupoCategoriaDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>SubSeguimento</p>
                            <asp:TextBox ID="txtDescSubSeguimentoGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>

                    </td>

                </tr>
                <tr>
                    <td colspan="6">
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodGrupoCategoriaDetalhes" runat="server" Width="100px" MaxLength="20" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Grupo</p>
                            <asp:TextBox ID="txtDescGrupoCategoriaDetalhes" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalGrupoCategoriaDetalhes" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnGrupoCategoria"
        TargetControlID="lblErroGrupoCategoria">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnSubGrupoCategoria" runat="server" CssClass="modalForm" Style="display: none; height: 300px; top: 0; width: 99%;">
        <asp:Panel ID="PanelSubGrupo" runat="server" CssClass="frame" Style="height: 250px;">
            <div class="cabMenu">
                <h1>
                    <asp:Label ID="lblIncluirSubGrupoCategoria" runat="server" Text=""></asp:Label>
                    Sub Grupo </h1>
                <asp:Label ID="lblErrorSubGrupoCategoria" runat="server" Text=""></asp:Label>
            </div>
            <table class="cabMenu">
                <tr>
                    <td>
                        <asp:ImageButton ID="ImgBtnConfirmaSubGrupoCategoriaDetalhes" runat="server" ImageUrl="~/img/confirm.png"
                            Width="25px" OnClick="ImgBtnConfirmaSubGrupoCategoriaDetalhes_Click" />
                        <asp:Label ID="LblConfSubgrupoSeguimento" runat="server" Text="Confirma"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgBtnCancelaSubGrupoCategoriaDetalhes" runat="server" ImageUrl="~/img/cancel.png"
                            Width="25px" OnClick="ImgBtnCancelaSubGrupoCategoriaDetalhes_Click" />
                        <asp:Label ID="lblCancelasubGr" runat="server" Text="Cancela"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtcodGrupoSubGrupoCategoriatoDetalhes" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Grupo
                        </p>
                        <asp:TextBox ID="txtDescGrupoSubGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodSubGrupoSubGrupoCategoriaDetalhes" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            SubGrupo
                        </p>
                        <asp:TextBox ID="txtDescSubGrupoSubGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Codigo
                        </p>
                        <asp:TextBox ID="txtCodDepartamentoSubGrupoCategoriaDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="6"></asp:TextBox>
                    </td>
                    <td>
                        <p>
                            Departamento
                        </p>
                        <asp:TextBox ID="txtDescDepartamentoSubGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodCategoriaSubGrupoCategoriaDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Categoria</p>
                            <asp:TextBox ID="txtDescCategoriaSubGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodSeguimentoSubGrupoCategoriaDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Seguimento</p>
                            <asp:TextBox ID="txtDescSeguimentoSubGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodSubSeguimentoSubGrupoCategoriaDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>SubSeguimento</p>
                            <asp:TextBox ID="txtDescSubSeguimentoSubGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>

                    </td>

                </tr>
                <tr>

                    <td colspan="6">
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodGrupoCategoriaSubGrupoCategoriaDetalhes" runat="server" Width="100px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Grupo</p>
                            <asp:TextBox ID="txtDescGrupoCategoriaSubGrupoCategoriaDetalhes" runat="server" Width="200px" Enabled="false" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Codigo</p>
                            <asp:TextBox ID="txtCodSubGrupoCategoria" runat="server" Width="100px" MaxLength="20" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="panelItem">
                            <p>Sub Grupo</p>
                            <asp:TextBox ID="txtDescSubGrupoCategoria" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalSubGrupoCategoriaDetalhes" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnSubGrupoCategoria"
        TargetControlID="lblErrorSubGrupoCategoria">
    </asp:ModalPopupExtender>



    <asp:Panel ID="pnFundo" runat="server" CssClass="modalForm" Style="display: none">
        <asp:Panel ID="pnFundoFrame" runat="server" CssClass="frame" DefaultButton="ImgPesquisaLista"
            Style="font-size: 20px;">
            <center><h1><asp:Label ID="lbllista" runat="server" Text="" ></asp:Label></h1></center>
            <hr />
            <div class="row" style="font-size: 20px; width: 70%; float: left; margin-left: 20px;">
                Filtrar
                <asp:TextBox ID="TxtPesquisaLista" runat="server" Width="400px"></asp:TextBox>
                <asp:ImageButton ID="ImgPesquisaLista" runat="server" ImageUrl="~/img/pesquisaM.png"
                    Height="15px" OnClick="ImgPesquisaLista_Click" />
                <asp:Label ID="lblErroPesquisa" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <div class="panel" style="font-size: 20px; width: 20%; float: left; margin-top: -10px;">
                <div class="row" style="margin-top: 0; margin-bottom: 10px;">
                    <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnFechar_Click" />
                    <asp:Label ID="Label4" runat="server" Text="Confirma"></asp:Label>
                </div>
                <div class="row">
                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaLista_Click" />
                    <asp:Label ID="LblCancelaLista" runat="server" Text="Cancela"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="Panel2" runat="server" CssClass="lista">
                <asp:GridView ID="GridLista" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    OnRowDataBound="GridLista_RowDataBound">
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
    <asp:ModalPopupExtender ID="modalPnFundo" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnFundo" TargetControlID="lbllista">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnConfirma" runat="server" CssClass="frameModal" Style="display: none">
        <span class="cabMenu">Confirma Excluir o Fornecedor:
            <asp:Label ID="lblCodFornecExcluir" runat="server" Text=""></asp:Label>
            ? </span>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="btnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExclusao_Click" />
                    <asp:Label ID="Label12" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="btnCancelaExclusao" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaExclusao_Click" />
                    <asp:Label ID="Label13" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalPnConfirma" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirma" TargetControlID="lblCodFornecExcluir">
    </asp:ModalPopupExtender>


    <asp:Panel ID="pnConfirmaExcluirDepartamento" runat="server" CssClass="frameModal" Style="display: none">
        <span class="cabMenu">Confirma Excluir o Departamento:
            <asp:Label ID="lblExcluirDepartamento" runat="server" Text=""></asp:Label>
            ? </span>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="btnConfirmaExclusaoDepartamento_Click" />
                    <asp:Label ID="Label7" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="btnCancelaExclusaoDepartamento_Click" />
                    <asp:Label ID="Label10" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaExcluirDepartamento" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaExcluirDepartamento" TargetControlID="lblExcluirDepartamento">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnIncluirImpressoraRemota" runat="server" CssClass="frameModal" Style="display: none; width: 500px; height: 200px;">
        <span class="cabMenu">Incluir Impressora </span>
        <asp:Label ID="lblErrorImpressora" runat="server" Text=""></asp:Label>

        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaAddImpressora" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaAddImpressora_Click" />
                    <asp:Label ID="Label14" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelaAddImpressora" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaAddImpressora_Click" />
                    <asp:Label ID="Label15" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>

        </table>
        <hr />
        <table class="frame" style="width: 100%; margin-left: 10px;">
            <tr>
                <td>
                    <div class="panelItem">
                        <p>Loja</p>
                        <asp:TextBox ID="txtLoja" runat="server" Width="50px" CssClass="numero" AutoPostBack="true" OnTextChanged="txtLoja_TextChange" OnKeyPress="javascript:return numeros(this,event);"></asp:TextBox>
                        <asp:TextBox ID="txtDescLoja" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                        <asp:ImageButton ID="imgPesquisaLoja" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="imgPesquisaImpressora_Click" />

                    </div>
                    <div class="panelItem">
                        <p>Impressora</p>
                        <asp:TextBox ID="txtImpressora" runat="server" Width="50px" CssClass="numero" OnKeyPress="javascript:return numeros(this,event);"
                            AutoPostBack="true" OnTextChanged="txtImpressora_TextChange"></asp:TextBox>
                        <asp:TextBox ID="txtDescImpressora" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                        <asp:ImageButton ID="imgPesquisaImpressora" runat="server" ImageUrl="~/img/pesquisaM.png"
                            Height="15px" OnClick="imgPesquisaImpressora_Click" />

                    </div>
                </td>

            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalAddImpressora" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnIncluirImpressoraRemota" TargetControlID="Label15">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnConfirmaExcluirImpressora" runat="server" CssClass="frameModal" Style="display: none">
        <span class="cabMenu">Confirma Excluir Impressora:
            <asp:Label ID="lblConfirmaImpressoraExcluir" runat="server" Text=""></asp:Label>
            ? </span>
        <table class="frame">
            <tr>
                <td>
                    <asp:ImageButton ID="imgBtnConfirmaExclusaoImpressora" runat="server" ImageUrl="~/img/confirm.png"
                        Width="25px" OnClick="imgBtnConfirmaExclusaoImpressora_Click" />
                    <asp:Label ID="Label16" runat="server" Text="Confirma"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="imgBtnCancelaExclusaoImpressora" runat="server" ImageUrl="~/img/cancel.png"
                        Width="25px" OnClick="imgBtnCancelaExclusaoImpressora_Click" />
                    <asp:Label ID="Label17" runat="server" Text="Cancela"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="modalConfirmaExcluirImpressora" runat="server" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupControlID="pnConfirmaExcluirImpressora" TargetControlID="lblConfirmaImpressoraExcluir">
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

</asp:Content>
