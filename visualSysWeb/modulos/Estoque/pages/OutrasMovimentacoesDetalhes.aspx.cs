using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using AjaxControlToolkit;
using System.Collections;
using visualSysWeb.code;


namespace visualSysWeb.modulos.Estoque.pages
{
    public partial class OutrasMovimentacoesDetalhes : visualSysWeb.code.PagePadrao
    {
        int linhaAtivaGridView = 0;
        private const string ProgressKey = "ProgressoAtual";

        protected void Page_Load(object sender, EventArgs e)
        {

            User usr = (User)Session["User"];
            if (!IsPostBack)
            {
                Session[ProgressKey] = 0;
                Conexao.executarSql("DELETE FROM lerComandaImporta");
                //txtComandasImportadas.Enabled = false;
                Conexao.preencherDDL(ddTipo, "Select Movimentacao='' ,Movimentacao1='' union Select Movimentacao ,Movimentacao1= Movimentacao from tipo_movimentacao  ", "Movimentacao", "Movimentacao1", usr);
            }


            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    inventarioDAO obj = new inventarioDAO(usr);
                    status = "incluir";

                    txtstatus.Text = "INICIADO";
                    txtData.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtUsuario.Text = usr.getNome();
                    obj.status = "INICIADO";
                    obj.Data = DateTime.Now;
                    obj.Usuario = usr.getNome();

                    Session.Remove("objInventario" + urlSessao());
                    Session.Add("objInventario" + urlSessao(), obj);

                    incluir(pnBtn);
                    HabilitarCampos(true);

                    if (!Funcoes.valorParametro("INCLUI_CATEGORIA", null).ToUpper().Equals("TRUE"))
                    {
                        ddlCategoria.Visible = false;
                        ddlSeguimento.Visible = false;
                    }
                    else
                    {
                        ddlCategoria.Visible = true;
                        ddlSeguimento.Visible = true;
                    }

                    //carregarGrupos("", "", "");
                    //carregarLinhas();
                }
            }
            else
            {
                if (Request.Params["campoIndex"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            String index = Request.Params["campoIndex"].ToString();// colocar o campo index da tabela
                            status = "visualizar";
                            inventarioDAO obj = new inventarioDAO(index, usr);
                            obj.gridInicio = 0;
                            obj.gridFim = 100;
                            Session.Remove("objInventario" + urlSessao());
                            Session.Add("objInventario" + urlSessao(), obj);

                            carregarDados();
                            //carregarGrupos("", "", "");
                        }
                        if (status.Equals("visualizar"))
                        {
                            HabilitarCampos(false);
                        }
                        else
                        {
                            HabilitarCampos(true);
                        }

                    }
                    catch (Exception err)
                    {
                        lblError.Text = err.Message;
                    }
                }
            }
            carregabtn(pnBtn);
            camposnumericos();
            //carregarGrupos(ddlGrupo.Text, ddlSubGrupo.Text, ddlDepartamento.Text, ddlCategoria.Text, ddlSeguimento.Text);
            //RdoTipoDeArquivo.Attributes.Add("onclick", "JavaScript: radioButtonListOnClick(this);");

        }

        protected void HabilitarCampos(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
            EnabledButtons(gridItens, enable);
            btnEncerrar.Visible = enable;
            //AjaxFileUpload1.Visible = enable;
            gridItens.Columns[0].Visible = enable;
            gridItens.Columns[12].Visible = enable;
            //RdoTipoDeArquivo.Enabled = enable;
            if (enable)
            {

                txtPlu.Enabled = !ddTipo.Text.Trim().Equals("");
                txtCusto.Enabled = true;
            }
            if (txtstatus.Text.Equals("ENCERRADO"))
            {
                imgBtnImpEncerrado.Visible = true;
                divBtnImprimirEncerrado.Visible = true;
                divBtnImprimirContagem.Visible = false;
                divBtnImprimirConferencia.Visible = false;
            }
            else
            {
                imgBtnImpEncerrado.Visible = false;
                imgBtnImprimir.Visible = !enable;
                imgBtnConferencia.Visible = !enable;
                divBtnImprimirContagem.Visible = !enable;
                divBtnImprimirConferencia.Visible = !enable;
                
            }


            divInventarioCompleto.Visible = ddTipo.Text.ToUpper().Trim().Equals("INVENTARIO");
            btnCriarInventarioCompleto.Visible = ddTipo.Text.ToUpper().Trim().Equals("INVENTARIO");
            if (status.Equals("incluir"))
            {
                EnabledControls(gridItens, false);
            }
            //divAvisoInventarioCompleto.Visible = chkInventarioCompleto.Visible;

            gridCampos();
            if (ddTipo.Text.Equals(""))
            {
                divAvisoTipo.Visible = true;
                conteudo.Visible = false;

            }
            else
            {
                divAvisoTipo.Visible = false;
                conteudo.Visible = true;
            }

        }

        private void gridCampos()
        {
            inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
            if (obj.tSaidaEntrada == 2)
            {
                lblContado.Text = "Contado";
                gridItens.Columns[8].Visible = true; // contado txt

                gridItens.Columns[9].Visible = false; // quantidade txt

                gridItens.Columns[10].Visible = true; //diferenca colu
                gridItens.Columns[11].Visible = false; // total 

                gridMercadoriasSelecionadas.Columns[7].Visible = true;
                gridMercadoriasSelecionadas.Columns[9].Visible = true;

                gridMercadoriasSelecionadas.Columns[8].Visible = false;
                gridMercadoriasSelecionadas.Columns[10].Visible = false;
            }
            else
            {
                lblContado.Text = "Qtde";
                gridItens.Columns[8].Visible = false; // contado
                gridItens.Columns[10].Visible = false; // diferenca
                gridItens.Columns[9].Visible = true; // quantidade

                gridItens.Columns[11].Visible = true; // total

                gridMercadoriasSelecionadas.Columns[7].Visible = false;
                gridMercadoriasSelecionadas.Columns[9].Visible = false;

                gridMercadoriasSelecionadas.Columns[8].Visible = true;
                gridMercadoriasSelecionadas.Columns[10].Visible = true;
            }

        }
        private void limparCampos()
        {
            LimparCampos(cabecalho);
            LimparCampos(conteudo);
        }

        private void camposnumericos()
        {
            ArrayList campos = new ArrayList();
            campos.Add("txtContado");
            campos.Add("txtCusto");
            FormataCamposNumericos(campos, conteudo);

            ArrayList camposInteiros = new ArrayList();
            camposInteiros.Add("txtPlu");
            camposInteiros.Add("txtinicioPlu");
            camposInteiros.Add("txtFimPlu");
            camposInteiros.Add("txtInicioContado");
            camposInteiros.Add("txtFimContado");
            camposInteiros.Add("txtDecimaisContado");

            FormataCamposInteiros(camposInteiros, conteudo);

        }
        protected bool validaCamposObrigatorios()
        {

            ddTipo.BackColor = System.Drawing.Color.White;
            if (ddTipo.Text.Trim().Equals(""))
            {
                ddTipo.BackColor = System.Drawing.Color.Red;
                throw new Exception("Selecione um tipo de movimentacao");
            }

            inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
            //if (obj.arrItens.Count == 0)
            //{
            //    throw new Exception("Não foi adicionado nenhum Item");
            //}

            if (validaCampos(cabecalho) && validaCampos(conteudo))
                return true;
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "ddTipo",
                                    "txtDescricao_inventario",
                                    "",
                                    ""
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array



            if (campo.ID != null)
            {
                if (campo.ID.Equals("ddTipo") && !status.Equals("incluir"))
                {
                    return true;

                }

                String[] campos = { "txtCodigo_inventario",
                                   "txtUsuario",
                                   "txtData",
                                    "txtDescricao",
                                    "TxtSaldoAtual",
                                    "txtstatus"
                              };

                return existeNoArray(campos, campo.ID.Trim());
            }
            else
                return false;
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("OutrasMovimentacoesDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            if (txtstatus.Text.Equals("ENCERRADO"))
            {
                lblError.Text = " NÃO É POSSIVEL FAZER MAIS ALTERAÇÕES ";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                editar(pnBtn);
                HabilitarCampos(true);
            }
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("OutrasMovimentacoes.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            lblError.Text = "Não é possivel Excluir o Registro";
            lblError.ForeColor = System.Drawing.Color.Red;
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            salvarMovimentacao(txtstatus.Text);
        }

        protected void salvarMovimento()
        {
            try
            {

                if (validaCamposObrigatorios())
                {

                    //carregarDadosObj();
                    inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];

                    obj.salvar(status.Equals("incluir"), (percentual) => {
                        Session[ProgressKey] = percentual;
                    });
                    obj.CarregarItens();


                    //Efetua limpeza da comanda
                    DataTable dt = Conexao.GetTable("SELECT * FROM lercomandaimporta", null, false);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            User usr = (User)Session["User"];

                            if (isnumeroint(row[0].ToString()))
                            {
                                string strAuxiliar = ""; 
                                try
                                {
                                    //Libera o Status na COMANDA_CONTROLE
                                    strAuxiliar = "UPDATE Comanda_Controle SET STATUS = '00' WHERE comanda = " + row[0].ToString();
                                    Conexao.executarSql(strAuxiliar);
                                    //Apaga informações da tabela COMANDA com cupom = 0
                                    strAuxiliar = "DELETE FROM Comanda  WHERE comanda = " + row[0].ToString() + " AND cupom = 0";
                                    Conexao.executarSql(strAuxiliar);
                                    //Cancela itens na COMANDA_ITEM
                                    strAuxiliar = "UPDATE Comanda_Item SET Data_Cancelamento = GETDATE(), Usuario_Cancelamento = '" + usr.getNome() + "'  WHERE comanda = " + row[0].ToString() + " AND Cupom = 0";
                                    Conexao.executarSql(strAuxiliar);
                                }
                                catch
                                {

                                }
                            }
                        }
                    }

                    //Session.Remove("objInventario" + urlSessao());
                    //Session.Add("objInventario" + urlSessao(), obj);
                    Session.Add("resultadoMovimento", "Salvo com Sucesso");



                }
                else
                {

                    Session.Add("erroMovimento", "Campo Obrigatorio não preenchido");

                }
            }
            catch (Exception err)
            {

                Session.Add("erroMovimento", err.Message);

            }
        }

        private void salvarMovimentacao(String strStatus)
        {
            txtstatus.Text = strStatus;
            if (strStatus.Equals("ENCERRADO"))
            {
                updPnl.Visible = true; //novo
                Timer1.Enabled = true; //novo
                TimerAguarde.Interval = 450;
                TimerAguarde.Enabled = false;
            }
            else
            {
                TimerAguarde.Interval = 450;
                TimerAguarde.Enabled = true;
                updPnl.Visible = false; //novo
                Timer1.Enabled = false; //novo
            }
            carregarDadosObj();
            System.Threading.Thread th = new System.Threading.Thread(salvarMovimento);
            th.Start();
            //modalaguarde.Show();


        }
        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("OutrasMovimentacoes.aspx");//colocar endereco pagina de pesquisa
        }

        //--Atualizar DaoForm 
        private void carregarDados()
        {
            inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
            ddTipo.Text = obj.tipoMovimentacao.ToString();
            txtCodigo_inventario.Text = obj.Codigo_inventario.ToString();
            txtDescricao_inventario.Text = obj.Descricao_inventario.ToString();
            txtData.Text = obj.DataBr();
            txtUsuario.Text = obj.Usuario.ToString();
            txtstatus.Text = obj.status.ToString();
            chkInventarioCompleto.Checked = obj.inventario_completo;
            chkDepartamento.Checked = obj.quebraDepartamento;
            lblRegistros.Text = (obj.gridInicio + 1) + " ate " + (obj.arrItens.Count > 100 ? obj.gridFim : obj.arrItens.Count) + " de " + obj.arrItens.Count.ToString() + " Registro(s) Adicionado(s)";

            //obj.ordernarItens();
            gridItens.DataSource = obj.Itens;
            gridItens.DataBind();

            if (status.Equals("incluir"))
            {
                EnabledControls(gridItens, false);
            }

            gridMercadoriasSelecionadas.DataSource = obj.Itens;
            gridMercadoriasSelecionadas.DataBind();



            //carregarMercadorias();
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
            obj.tipoMovimentacao = ddTipo.Text;
            obj.Codigo_inventario = txtCodigo_inventario.Text;
            obj.Descricao_inventario = txtDescricao_inventario.Text;
            obj.Data = DateTime.Parse(txtData.Text);
            obj.Usuario = txtUsuario.Text;
            obj.status = txtstatus.Text;
            obj.inventario_completo = chkInventarioCompleto.Checked;
            obj.quebraDepartamento = chkDepartamento.Checked;
            if (obj.arrItens.Count > 0)
            {
                if (!status.Equals("incluir"))
                {
                    for (int i = 0; i < gridItens.Rows.Count; i++)
                    {
                        TextBox txtGridQtde = (TextBox)gridItens.Rows[i].FindControl("txtGridQtde");
                        TextBox txtGridContado = (TextBox)gridItens.Rows[i].FindControl("txtGridContado");
                        inventario_itensDAO item = (inventario_itensDAO)obj.arrItens[i + obj.gridInicio];
                        if (obj.tSaidaEntrada == 2)
                        {
                            item.Contada = Decimal.Parse(txtGridContado.Text);
                        }
                        else
                        {
                            item.Contada = Decimal.Parse(txtGridQtde.Text);
                        }
                    }
                }
            }
            Session.Remove("objInventario" + urlSessao());
            Session.Add("objInventario" + urlSessao(), obj);
        }

        protected void ImgBtnAddItens_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtPlu.Text.Equals("") && !txtContado.Text.Equals(""))
                {
                    carregarDadosObj();
                    User usr = (User)Session["User"];
                    inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
                    inventario_itensDAO item = new inventario_itensDAO(usr);
                    item.PLU = txtPlu.Text;

                    //Incluso linha abaixo para trazer dados do produtos (ean,  REF)
                    MercadoriaDAO produto = new MercadoriaDAO(txtPlu.Text, usr);

                    if (txtContado.Text.Trim().Length > 11 || txtContado.Text.Equals(""))
                    {
                        txtContado.Focus();
                        txtContado.Text = "";
                        throw new Exception("Qtde invalida");
                    }

                    item.Saldo_atual = Decimal.Parse(TxtSaldoAtual.Text);
                    item.EAN = produto.eanPrimeiro;
                    item.Referencia = produto.Ref_fornecedor;
                    item.Contada = (txtContado.Text.Trim().Equals("") ? 0 : decimal.Parse(txtContado.Text));
                    item.Custo = (txtCusto.Text.Trim().Equals("") ? 0 : decimal.Parse(txtCusto.Text));
                    obj.addItens(item);

                    Session.Remove("objInventario" + urlSessao());
                    Session.Add("objInventario" + urlSessao(), obj);
                    carregarDados();
                    lblError.Text = "Item incluido";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    txtPlu.Text = "";
                    txtDescricao.Text = "";
                    TxtSaldoAtual.Text = "";
                    txtContado.Text = "";
                    txtCusto.Text = "";

                    addItens.DefaultButton = "imgPlu";
                    txtPlu.Focus();
                }
                else
                {
                    txtPlu.Focus();
                    throw new Exception("Qtde invalida");
                }

            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void gridItens_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.Equals("Alterar"))
            {
                txtPlu.Text = gridItens.Rows[index].Cells[2].Text;
                txtDescricao.Text = gridItens.Rows[index].Cells[5].Text;
                TxtSaldoAtual.Text = gridItens.Rows[index].Cells[6].Text;
                txtCusto.Text = gridItens.Rows[index].Cells[7].Text;
                txtContado.Text = gridItens.Rows[index].Cells[8].Text;
                txtContado.Focus();
                addItens.DefaultButton = "ImgBtnAddItens";

            }
            else
            {
                inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
                obj.removeItens(index);
                carregarDados();
            }

        }
        protected void img_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("TipoMovimentacao");
        }

        protected void imgPlu_Click(object sender, ImageClickEventArgs e)
        {
            if (!txtPlu.Text.Trim().Equals(""))
            {
                carregarPlu();
            }
            else
            {
                exibeLista("PLU");
            }
        }
        protected void imgFornecedor_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("Fornecedor");
        }

        protected void imgMarca_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("Marca");
        }
        protected void carregarPlu()
        {
            User usr = (User)Session["User"];
            SqlDataReader rsplu = null;
            try
            {


                //rsplu = Conexao.consulta("Select top 1 mercadoria.PLU,mercadoria.descricao, isnull(mercadoria_loja.saldo_atual,0)saldo_atual,isnull(mercadoria_loja.preco_custo,0)custo FROM mercadoria inner join mercadoria_loja on mercadoria.plu=mercadoria_loja.plu left join ean on mercadoria.plu=ean.plu INNER JOIN Tipo ON Tipo.Tipo = Mercadoria.Tipo AND ISNULL(Tipo.Estoque, 0) = 1 WHERE mercadoria_loja.filial='" + usr.getFilial() + "' and ( mercadoria.plu='" + txtPlu.Text + "' or ean.EAN='" + txtPlu.Text + "')", null, false);

                rsplu = Conexao.consulta("Select top 1 mercadoria.PLU,mercadoria.descricao, isnull(mercadoria_loja.saldo_atual,0)saldo_atual,isnull(mercadoria_loja.preco_custo,0)custo FROM mercadoria inner join mercadoria_loja on mercadoria.plu=mercadoria_loja.plu left join ean on mercadoria.plu=ean.plu INNER JOIN Tipo ON Tipo.Tipo = Mercadoria.Tipo  WHERE mercadoria_loja.filial='" + usr.getFilial() + "' and ( mercadoria.plu='" + txtPlu.Text + "' or ean.EAN='" + txtPlu.Text + "')", null, false);
                if (rsplu.Read())
                {
                    txtPlu.Text = rsplu["plu"].ToString();
                    txtDescricao.Text = rsplu["descricao"].ToString();
                    TxtSaldoAtual.Text = Decimal.Parse(rsplu["saldo_atual"].ToString()).ToString();
                    txtCusto.Text = Decimal.Parse(rsplu["custo"].ToString()).ToString();
                    txtContado.Focus();
                    addItens.DefaultButton = "ImgBtnAddItens";
                }
                else
                {
                    lblError.Text = "Produto não encontrado!!";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    txtPlu.Focus();

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rsplu != null)
                    rsplu.Close();
            }

        }

        protected void exibeLista(String campo)
        {
            Session.Remove("campoLista" + urlSessao());
            Session.Add("campoLista" + urlSessao(), campo);

            String sqlLista = "";

            switch (campo)
            {
                case "PLU":
                    lbltituloLista.Text = "Escolha uma mercadoria";
                    sqlLista = "select mercadoria.PLU, ISNULL(mercadoria.Ref_Fornecedor,'') as REFERENCIA, DESCRICAO,isnull(ean.ean,'') EAN from mercadoria left join ean on ean.plu=mercadoria.plu INNER JOIN Tipo ON Tipo.Tipo = Mercadoria.Tipo AND ISNULL(Tipo.Estoque, 0) = 1 WHERE ";
                    sqlLista = sqlLista + " mercadoria.plu like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%'  or mercadoria.Ref_fornecedor like '%" + TxtPesquisaLista.Text + "%' or (ean like '%" + TxtPesquisaLista.Text + "%') ORDER BY DESCRICAO";

                    break;
                case "Fornecedor":
                    lbltituloLista.Text = "Escolha um  Fornecedor";
                    sqlLista = "select Fornecedor,CNPJ,Razao_social from Fornecedor " +
                               " where Fornecedor like '%" + TxtPesquisaLista.Text + "%'  or " +
                               " Razao_social like '%" + TxtPesquisaLista.Text + "%' or  " +
                               "  Nome_Fantasia = '%" + TxtPesquisaLista.Text + "%' or  " +
                               " replace(replace(REPLACE(cnpj,'.',''),'/',''),'-','') like '" + TxtPesquisaLista.Text.Replace(".", "").Replace("-", "").Replace("/", "") + "%' order by fornecedor ";
                    break;
                case "Marca":
                    lbltituloLista.Text = "Escolha uma Marca";
                    sqlLista = "select distinct marca = isnull(marca, '') from Mercadoria " +
                               " where Marca like '%" + TxtPesquisaLista.Text + "%'  order by Marca";
                    break;
                case "ListaPadrao":
                    lbltituloLista.Text = "Escolha a Lista de produtos";
                    sqlLista = "Select Id, Descricao , Qtde_Itens = (Select count(*) from LISTA_PADRAO_ITENS WHERE ID_LISTA = ID)from LISTA_PADRAO where tipo ='INV.AGRUPA' ";
                    break;
            }
            User usr = (User)Session["User"];
            GridLista.DataSource = Conexao.GetTable(sqlLista, null, (campo.ToUpper().Equals("LISTAPADRAO") ? false : true));
            GridLista.DataBind();

            modalLista.Show();
            TxtPesquisaLista.Text = "";
            TxtPesquisaLista.Focus();
        }

        protected void btnEncerrar_Click(object sender, EventArgs e)
        {
            Session.Remove("execSalvar" + urlSessao());
            modalEncerrar.Show();

        }

        protected String ListaSelecionada(int index)
        {
            foreach (GridViewRow item in GridLista.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoListaItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {
                        return item.Cells[index].Text;
                    }
                }
            }

            return "";
        }
        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalLista.Hide();
        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {

            String itemLista = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            switch (itemLista)
            {
                case "PLU":
                    txtPlu.Text = ListaSelecionada(1);
                    carregarPlu();
                    break;
                case "Fornecedor":
                    txtFornecedor.Text = ListaSelecionada(1);
                    carregarMercadorias(false);
                    break;
                case "Marca":
                    txtMarca.Text = ListaSelecionada(1);
                    carregarMercadorias(false);
                    break;
                case "ListaPadrao":
                    txtcodListaPadrao.Text = ListaSelecionada(1);
                    txtDescricaoListaPadrao.Text = ListaSelecionada(2);
                    carregarMercadorias(false);
                    break;
            }

            modalLista.Hide();
        }

        private void Ler()
        {

            try
            {


                Coletor coletor = new Coletor();
                carregarDadosObj();
                //String arquivo = Server.MapPath("") + "\\importacao\\ArqImportado.txt";

                User usr = (User)Session["User"];
                String endereco = (usr.filial.ip.Equals("::1") ? "c:" : @"\\" + usr.filial.ip) + "\\Coletor";
                String arquivo = ddlArquivos.SelectedValue;

                String strSQL = "";
                arquivo = endereco + "\\" + arquivo;
                string[] lines = System.IO.File.ReadAllLines(arquivo);
                inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
                Hashtable tbPlus = new Hashtable();
                //int vdecimal = int.Parse(txtDecimaisContado.Text) + 1;
                String strPlus = "";
                //int inicioPlu = int.Parse(txtinicioPlu.Text) - 1;
                //int tamPlu = 0;
                //int inicioContado = int.Parse(txtInicioContado.Text) - 1;
                //int tamContado = 0;
                //if (!coletor.coletorFixo)
                //{
                //    tamPlu = (int.Parse(txtFimPlu.Text)) - inicioPlu;
                //    tamContado = (int.Parse(txtFimContado.Text)) - inicioContado;
                //}
                String contado = "0";
                String plu = "";
                int iLinha = 0;


                Conexao.executarSql("delete from lerArquivoImporta ");
                foreach (string line in lines)
                {
                    iLinha++;

                    if (line.Length > 0)
                    {

                        if (coletor.coletorFixo)
                        {
                            plu = line.Substring(coletor.pluInicio-1, coletor.tamPlu);
                            contado = line.Substring(coletor.contadoInicio-1, coletor.contadoFim);
                        }
                        else
                        {
                            string[] arrLinha = line.Split(coletor.delimitador[0]);
                            plu = arrLinha[coletor.pluInicio-1];
                            contado = arrLinha[coletor.contadoInicio-1];
                        }
                        long lPlu = 0;
                        long.TryParse(plu.Trim(), out lPlu);
                        String sqlImpo = "";
                        if (!tbPlus.ContainsKey(lPlu.ToString()))
                        {
                            tbPlus.Add(lPlu.ToString(), contado.Trim());
                            sqlImpo = "insert into lerArquivoImporta values ('" + lPlu.ToString() + "');";
                            Conexao.executarSql(sqlImpo);
                        }
                        else
                        {
                            String contPlu = tbPlus[lPlu.ToString()].ToString();
                            contado = (Decimal.Parse(contPlu) + Decimal.Parse(contado)).ToString();
                            tbPlus[lPlu.ToString()] = contado;
                        }




                        Session.Remove("resultadoImportaColetor");
                        Session.Add("resultadoImportaColetor", "lido " + iLinha.ToString() + " linhas do arquivo");
                    }
                }

                strSQL = "Select EAN = ISNULL((SELECT TOP 1 EAN.EAN FROM EAN WHERE EAN.PLU = MERCADORIA.PLU), ''), ";
                strSQL += "mercadoria.PLU,mercadoria.descricao, isnull(mercadoria_loja.saldo_atual,0)saldo_atual , ";
                strSQL += "isnull(mercadoria.Ref_Fornecedor, '') as Referencia ";
                strSQL += ",mercadoria_loja.preco, mercadoria_loja.preco_custo ";
                strSQL += " from mercadoria inner join mercadoria_loja on mercadoria.plu=mercadoria_loja.plu ";
                strSQL += "where (mercadoria_loja.filial='" + usr.getFilial() + "') AND  (mercadoria.plu in(Select codigo from lerArquivoImporta) ";
                strSQL += "or mercadoria.plu in(SELECT  EAN.PLU FROM EAN WHERE convert(numeric,EAN.EAN  ) IN(Select codigo from lerArquivoImporta))) ";
                strSQL += "group by mercadoria.PLU,mercadoria.descricao, isnull(mercadoria_loja.saldo_atual,0),isnull(mercadoria.Ref_Fornecedor, ''),mercadoria_loja.preco, mercadoria_loja.preco_custo ";


                SqlDataReader rsplu = null;
                try
                {


                    rsplu = Conexao.consulta(strSQL, null, false);

                    while (rsplu.Read())
                    {

                        inventario_itensDAO item = new inventario_itensDAO(usr);
                        item.PLU = rsplu["plu"].ToString();

                        if (rsplu["ean"].ToString().Trim().Length > 6)
                        {
                            item.EAN = rsplu["ean"].ToString().Trim();
                        }
                        else
                        {
                            item.EAN = "";
                        }
                        item.Referencia = rsplu["Referencia"].ToString();
                        item.Saldo_atual = Decimal.Parse(rsplu["saldo_atual"].ToString());

                        if (tbPlus.ContainsKey(item.PLU.Trim()))
                            contado = tbPlus[item.PLU.Trim()].ToString();
                        else
                        {



                            long loEan = 0;

                            long.TryParse(rsplu["ean"].ToString().Trim(), out loEan);

                            String strEan = loEan.ToString();

                            if (!tbPlus.ContainsKey(strEan))
                            {
                                SqlDataReader rsEans = Conexao.consulta("Select ean from ean where plu ='" + item.PLU + "' and ean <>'" + strEan + "'", usr, false);
                                while (rsEans.Read())
                                {
                                    long.TryParse(rsEans["ean"].ToString().Trim(), out loEan);


                                    if (tbPlus.ContainsKey(loEan.ToString()))
                                    {

                                        strEan = loEan.ToString();
                                        item.EAN = strEan;
                                        break;
                                    }

                                }
                                if (rsEans != null)
                                    rsEans.Close();
                            }
                            try
                            {

                                contado = tbPlus[strEan].ToString();
                            }
                            catch (Exception err)
                            {

                                throw new Exception(err.Message + " não encontrado" + strEan);
                            }
                        }

                        //int casaDec = (contado.Length-vdecimal);
                        //item.Contada = Decimal.Parse(contado.Substring(0, casaDec) + '.' + contado.Substring(casaDec, vdecimal));
                        item.Contada = Decimal.Parse(contado) / int.Parse("1" + "".PadRight(coletor.contadoDecimal, '0'));
                        if (obj.tSaidaEntrada == 1)
                        {

                            Decimal.TryParse(rsplu["preco"].ToString(), out item.Custo);
                        }
                        else
                        {
                            Decimal.TryParse(rsplu["preco_custo"].ToString(), out item.Custo);
                        }


                        obj.addItens(item);
                        Session.Remove("resultadoImportaColetor");
                        Session.Add("resultadoImportaColetor", obj.arrItens.Count.ToString() + " itens Importados");
                    }
                }
                catch (Exception err)
                {

                    throw new Exception(err.Message); ;
                }
                finally
                {
                    if (rsplu != null)
                        rsplu.Close();
                }

                Session.Remove("objInventario" + urlSessao());
                Session.Add("objInventario" + urlSessao(), obj);
                carregarDados();
                TabContainer1.ActiveTabIndex = 0;
                Session.Remove("resultadoImportaColetor");
                Session.Add("resultadoImportaColetor", "Todos " + obj.arrItens.Count.ToString() + " itens Importados com sucesso");

                if (!Funcoes.existePasta(endereco + "//importado"))
                {
                    System.IO.Directory.CreateDirectory(endereco + "//importado");
                }

                System.IO.File.Move(arquivo, endereco + "//importado//" + DateTime.Now.ToString("yyyy.MM.dd.hh.mm") + ddlArquivos.SelectedValue);
                checaArquivosImportar();

            }
            catch (Exception err)
            {
                Session.Remove("erroImportaColetor");
                Session.Add("erroImportaColetor", err.Message);


            }


        }

        //protected void AjaxFileUpload1_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        //{
        //    try
        //    {

        //        String caminho = Server.MapPath("") + "\\importacao\\ArqImportado.txt";
        //        AjaxFileUpload1.SaveAs(caminho);


        //    }
        //    catch (Exception err)
        //    {

        //        lblError.Text = "Erro de importação erro: " + err.Message;
        //    }
        //}




        protected void RdoTipoDeArquivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (RdoTipoDeArquivo.SelectedValue.Equals("0"))
            //{
            //    txtDel.Text = "";
            //    txtDel.Visible = false;
            //    lblDel.Visible = false;
            //    lblFimPlu.Visible = true;
            //    txtFimPlu.Visible = true;
            //    lblFimContado.Visible = true;
            //    txtFimContado.Visible = true;
            //    lblInicioPlu.Text = "Inicio";
            //    lblInicioContado.Text = "Inicio";
            //    txtinicioPlu.Focus();
            //}
            //else
            //{
            //    lblDel.Visible = true;
            //    txtDel.Visible = true;
            //    txtDel.Focus();
            //    txtFimPlu.Text = "";
            //    lblFimPlu.Visible = false;
            //    txtFimPlu.Visible = false;
            //    lblFimContado.Visible = false;
            //    txtFimContado.Text = "";
            //    txtFimContado.Visible = false;
            //    lblInicioPlu.Text = "Posição";
            //    lblInicioContado.Text = "Posição";
            //}
        }

        protected void imgLeArquivo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                TimerImportaColetor.Interval = 450;
                TimerImportaColetor.Enabled = true;

                System.Threading.Thread th = new System.Threading.Thread(Ler);
                th.Start();

                modalaguarde.Show();

            }
            catch (Exception err)
            {

                lblError.Text = "Erro na importacao do arquivo :" + err.Message;
            }
        }

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            String itemLista = (String)Session["campoLista" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            exibeLista(itemLista);
        }

        protected void GridLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoListaItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridLista.*GrlistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void ddlTipoMovimentacao_SelectedIndexChanged(object sender, EventArgs e)
        {

            carregarDadosObj();
            carregarDados();
        }

        protected void btnConfirmaEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            // Evitar Duplo click
            String exec = (String)Session["execSalvar" + urlSessao()];
            if (exec != null)
                return;
            else
                Session.Add("execSalvar" + urlSessao(), "executando");


            salvarMovimentacao("ENCERRADO");
            modalEncerrar.Hide();
        }

        protected void btnCancelarEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            modalEncerrar.Hide();
        }



        protected String GerarArquivo(inventarioDAO inv)
        {

            String strLinhaReg = "";
            User usr = (User)Session["User"];
            string strSpace = "";
            Decimal decTotalOM = 0;
            Decimal intItem = 0;
            Decimal decVlrUnit = 0;
            SqlDataReader rs = Conexao.consulta("SELECT Inventario_Itens.*, Mercadoria.Descricao FROM Inventario_Itens INNER JOIN Mercadoria ON Inventario_Itens.PLU = Mercadoria.PLU WHERE Inventario_Itens.Codigo_Inventario = " + inv.Codigo_inventario, usr, false);

            if (rs != null)
            {
                strLinhaReg = "**** OUTRAS MOVIMENTACOES ***************************************************" + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "Acerto Nro: " + inv.Codigo_inventario.ToString().PadLeft(6, '0') + " Tipo: " + inv.tipoMovimentacao + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "Descricao do Acerto: " + inv.Descricao_inventario + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "Usuario: " + inv.Usuario + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "Data: " + inv.Data.ToString("dd/MM/yyyy") + (char)13 + (char)10;

                strLinhaReg = strLinhaReg + "===============================================================================" + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "Qtde  Uni Cod            Descricao                             Unit  Total Item" + (char)13 + (char)10;
                strLinhaReg = strLinhaReg + "===============================================================================" + (char)13 + (char)10;

                while (rs.Read())
                {
                    intItem = Decimal.Parse(rs["Contada"].ToString());
                    decVlrUnit = Decimal.Parse(rs["Custo"].ToString());
                    strLinhaReg = strLinhaReg + intItem.ToString("N2").PadLeft(5, ' ') + " ";
                    strLinhaReg = strLinhaReg + Conexao.retornaUmValor("select isnull(UND,'UN') from mercadoria where plu='" + rs["PLU"].ToString() + "'", null);
                    strLinhaReg = strLinhaReg + Conexao.retornaUmValor("select isnull(Ref_Fornecedor,'') from mercadoria where plu='" + rs["PLU"].ToString() + "'", null).PadRight(7, ' ') + "-";
                    strLinhaReg = strLinhaReg + rs["PLU"].ToString().PadLeft(6, '0') + " ";
                    if (rs["Descricao"].ToString().ToString().Length > 35)
                    {
                        strLinhaReg = strLinhaReg + Funcoes.RemoverAcentos(rs["Descricao"].ToString()).Substring(0, 35).PadRight(35, ' ');
                    }
                    else
                    {
                        strLinhaReg = strLinhaReg + Funcoes.RemoverAcentos(rs["Descricao"].ToString()).PadRight(35, ' ');
                    }
                    strLinhaReg = strLinhaReg + decVlrUnit.ToString("N2").PadLeft(9, ' ');
                    decTotalOM += (intItem * decVlrUnit);
                    //strLinhaReg = strLinhaReg + item.unitario.ToString("N2").PadLeft(12, ' ');
                    strLinhaReg = strLinhaReg + (intItem * decVlrUnit).ToString("N2").PadLeft(11, ' ');
                    strLinhaReg = strLinhaReg + (char)13 + (char)10;
                    strLinhaReg = strLinhaReg + "-------------------------------------------------------------------------------" + (char)13 + (char)10;
                }
            }
            strLinhaReg = strLinhaReg + strSpace.PadLeft(55, ' ') + "Vlr do Acerto:" + decTotalOM.ToString("N2").PadLeft(10, ' ') + (char)13 + (char)10; ;

            strLinhaReg = strLinhaReg + "Imp.:" + DateTime.Now.ToString() + (char)13 + (char)10;
            return strLinhaReg;
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {

        }


        protected void TimerAguarde_Tick(object sender, EventArgs e)
        {
            String resultado = (String)Session["resultadoMovimento"];
            String error = (String)Session["erroMovimento"];
            String aborta = (String)Session["abortaMovimento"];

            if (aborta != null)
            {
                TimerAguarde.Enabled = false;
                Session.Remove("abortaMovimento");
            }
            else if (resultado != null)
            {
                Session.Remove("resultadoMovimento");
                TimerAguarde.Enabled = false;
                lblError.Text = resultado;
                lblError.ForeColor = System.Drawing.Color.Blue;
                visualizar(pnBtn);
                carregarDados();
                HabilitarCampos(false);


            }
            else if (error != null)
            {
                Session.Remove("erroMovimento");
                TimerAguarde.Enabled = false;
                lblError.Text = "Erro" + error;
                lblError.ForeColor = System.Drawing.Color.Red;
            }

            if (TimerAguarde.Enabled)
            {
                //int registros = 0;
                //if (Session["contItens"] != null)
                //{
                //    registros += 10;

                //}
                //else
                //{
                //    registros = (int)Session["contItens"];
                //    registros += 10;
                //}
                //Session.Remove("contItens");
                //Session.Add("contItens", registros);
                //Label3.Text = registros.ToString() + " Atualizados! Não Feche a Tela ate o fim da atualização! ";
                modalaguarde.Show();
            }
            else
            {
                modalaguarde.Hide();
            }


        }


        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, "", "");

        }

        protected void ddlSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, ddlSubGrupo.Text, "");

        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, ddlSubGrupo.Text, ddlDepartamento.Text);

        }
        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, ddlSubGrupo.Text, ddlDepartamento.Text);

        }
        protected void ddlSeguimento_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarGrupos(ddlGrupo.Text, ddlSubGrupo.Text, ddlDepartamento.Text);

        }
        protected void ddlLinha_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarMercadorias(false);
        }
        private void carregarGrupos(String grupo, String subGrupo, String departamento, String categoria = "", String seguimento = "")
        {

            String sqlGrupo = "select Codigo_Grupo,Descricao_Grupo from Grupo";
            String sqlSubGrupo = "Select codigo_subgrupo, descricao_subgrupo from subgrupo " + (!ddlGrupo.Text.Equals("") ? " where codigo_grupo='" + ddlGrupo.SelectedValue + "'" : "");
            String sqlDepartamento = "Select codigo_departamento, descricao_departamento from W_BR_CADASTRO_DEPARTAMENTO ";

            String sqlCategoria = "SELECT codigo, descricao FROM Categorias " + (!ddlDepartamento.Text.Equals("") ? " WHERE codigo_departamento = '" + ddlDepartamento.SelectedValue + "'" : "");
            String sqlSeguimento = "SELECT codigo, descricao FROM Categorias " + (!ddlCategoria.Text.Equals("") ? " WHERE codigo_departamento = '" + ddlCategoria.SelectedValue + "'" : "");

            String sqlWhereDep = "";
            if (!ddlGrupo.Text.Equals(""))
            {
                sqlWhereDep = " where codigo_grupo='" + ddlGrupo.SelectedValue + "'";
            }
            if (!ddlSubGrupo.Text.Equals(""))
            {
                if (sqlWhereDep.Length > 0)
                    sqlWhereDep += " and ";
                else
                    sqlWhereDep += " where ";

                sqlWhereDep += " codigo_subgrupo='" + ddlSubGrupo.SelectedValue + "'";
            }

            if (!ddlDepartamento.Text.Equals(""))
            {
                if (sqlWhereDep.Length > 0)
                    sqlWhereDep += " and ";
                else
                    sqlWhereDep += " where ";

                sqlWhereDep += " codigo_subgrupo='" + ddlSubGrupo.SelectedValue + "'";

            }

            Conexao.preencherDDL1Branco(ddlGrupo, sqlGrupo, "Descricao_grupo", "codigo_grupo", null);
            Conexao.preencherDDL1Branco(ddlSubGrupo, sqlSubGrupo, "descricao_subgrupo", "codigo_subgrupo", null);

            if (!ddlDepartamento.SelectedValue.Equals(""))
            {
                Conexao.preencherDDL1Branco(ddlCategoria, sqlCategoria, "descricao", "codigo", null);
                if (!ddlCategoria.Text.Equals(""))
                {
                    Conexao.preencherDDL1Branco(ddlSeguimento, sqlSeguimento, "descricao", "codigo", null);
                }
            }


            Conexao.preencherDDL1Branco(ddlDepartamento, sqlDepartamento + sqlWhereDep, "descricao_departamento", "codigo_departamento", null);


            ddlGrupo.Text = grupo;
            ddlSubGrupo.Text = subGrupo;
            ddlDepartamento.Text = departamento;
            ddlCategoria.Text = categoria;
            ddlSeguimento.Text = seguimento;

            if (grupo.Equals("") && subGrupo.Equals("") && departamento.Equals(""))
            {
                carregarMercadorias(true);
            }
            else
            {
                carregarMercadorias(false);
            }
        }
        private void carregarLinhas()
        {
            String sqlLinha = "Select codigo=convert(varchar(3),linha.codigo_linha)+convert(varchar(3),cor_linha.codigo_cor),  linha= linha.descricao_linha+'-'+cor_linha.descricao_cor from linha " +
                                                                " inner join cor_linha on linha.codigo_linha = cor_linha.codigo_linha";
            Conexao.preencherDDL1Branco(ddlLinha, sqlLinha, "linha", "codigo", null);
        }
        protected void carregarMercadorias(bool limitar)
        {
            if (IsPostBack)
            {
                verificaSelecionados();
            }
            lblMercadoriaLista.Text = "Inclusão de Produto";
            //lblMercadoriaLista.ForeColor = Label1.ForeColor;

            if (ddlGrupo.Text.Equals("") &&
                ddlSubGrupo.Text.Equals("") &&
                ddlDepartamento.Text.Equals("") &&
                ddlLinha.Text.Equals("") &&
                txtFornecedor.Text.Equals("") &&
                txtMarca.Text.Equals("") &&
                txtfiltromercadoria.Text.Equals("") &&
                ddlCategoria.Text.Equals("") &&
                ddlSeguimento.Text.Equals(""))
            {
                limitar = true;
            }


            User usr = (User)Session["user"];
            String sqlMercadoria = "Select distinct mercadoria.plu PLU," +
                                                   " isnull(ean.ean,'---')EAN," +
                                                   " mercadoria.Ref_fornecedor REFERENCIA, " +
                                                   " mercadoria.descricao DESCRICAO, " +
                                                   " mercadoria_loja.preco_Custo as [PRC COMPRA]," +
                                                   " mercadoria_loja.saldo_atual SALDO, " +
                                                   " mercadoria_loja.preco as [PRC VENDA] " +
                                             " from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu " +
                                               " left join ean on mercadoria.plu=ean.plu  " +
                                               //" inner join W_BR_CADASTRO_DEPARTAMENTO on mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento "+
                                               " left join Fornecedor_Mercadoria on mercadoria.PLU = Fornecedor_Mercadoria.PLU  AND Mercadoria_Loja.Filial = Fornecedor_Mercadoria.Filial " +
                                    " where (mercadoria_loja.filial='" + usr.getFilial() + "') AND ISNULL(mercadoria.Inativo, 0) = 0 ";
            if (isnumero(txtfiltromercadoria.Text))
            {
                if (txtfiltromercadoria.Text.Length <= 6)
                {
                    sqlMercadoria += " and mercadoria.plu = '" + txtfiltromercadoria.Text + "' ";
                }
                else
                {
                    sqlMercadoria += " and (ean like '%" + txtfiltromercadoria.Text + "%')";
                }
            }
            else
            {
                if (txtfiltromercadoria.Text.Length > 0)
                {

                    sqlMercadoria += " and (mercadoria.descricao like '%" + txtfiltromercadoria.Text + "%' or mercadoria.Ref_fornecedor like '%" + txtfiltromercadoria.Text + "%')";
                }


                if (!ddlGrupo.Text.Equals(""))
                {
                    sqlMercadoria += " and substring(mercadoria.codigo_departamento,1,3)='" + ddlGrupo.SelectedValue.PadLeft(3, '0') + "' ";
                }
                if (!ddlSubGrupo.Text.Equals(""))
                {
                    sqlMercadoria += " and substring(mercadoria.codigo_departamento,1,6) ='" + ddlSubGrupo.SelectedValue + "' ";

                }
                if (!ddlDepartamento.Text.Equals(""))
                {
                    sqlMercadoria += " and mercadoria.codigo_departamento ='" + ddlDepartamento.SelectedValue + "' ";
                }

                if (!ddlCategoria.Text.Equals(""))
                {
                    sqlMercadoria += " and mercadoria.Categoria = '" + ddlCategoria.SelectedValue + "'";
                }

                if (!ddlSeguimento.Text.Equals(""))
                {
                    sqlMercadoria += " AND mercadoria.Seguimento = '" + ddlSeguimento.SelectedValue + "'";
                }
            }

            if (!ddlLinha.Text.Equals(""))
            {
                sqlMercadoria += " and convert(varchar(3),isnull(Cod_Linha,''))+CONVERT(varchar(3),isnull(Cod_Cor_Linha,'')) ='" + ddlLinha.SelectedValue + "'";
            }

            if (!txtFornecedor.Text.Trim().Equals(""))
            {
                sqlMercadoria += " and (Fornecedor_Mercadoria.Fornecedor ='" + txtFornecedor.Text + "') ";
            }

            if (!txtMarca.Text.Trim().Equals(""))
            {
                sqlMercadoria += " AND Mercadoria.Marca like '" + txtMarca.Text + "%'";
            }

            if (!txtcodListaPadrao.Text.Trim().Equals(""))
            {
                sqlMercadoria += " AND Mercadoria.PLU IN(SELECT PLU FROM Lista_Padrao_Itens WHERE Lista_Padrao_Itens.ID_Lista = " + txtcodListaPadrao.Text.Trim() + ")";
            }
            //if Funcoes.valorParametro("PEDIDO_SIMPLES", usr).ToUpper()
            //voltar aqui 22042015

            gridMercadoria1.DataSource = Conexao.GetTable(sqlMercadoria + " order by mercadoria.descricao", usr, limitar);
            gridMercadoria1.DataBind();




        }
        private void verificaSelecionados()
        {
            carregarDadosObj();
            foreach (GridViewRow item in gridMercadoria1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                if (chk.Checked)
                {
                    incluirMercadoria(chk);
                    chk.Checked = false;
                }
            }
            carregarDados();
        }
        private void incluirMercadoria(CheckBox ck1)
        {
            GridViewRow linha = (GridViewRow)ck1.NamingContainer;


            User usr = (User)Session["User"];
            inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
            inventario_itensDAO item = new inventario_itensDAO(usr);
            item.PLU = linha.Cells[1].Text;
            item.EAN = linha.Cells[2].Text.Replace("---", "").Replace("&nbsp;", "");
            item.Referencia = linha.Cells[3].Text.Replace("---", "").Replace("&nbsp;", "");
            item.Saldo_atual = Decimal.Parse(linha.Cells[6].Text);
            item.Contada = 0;
            item.Custo = Decimal.Parse(linha.Cells[5].Text);
            item.Venda = Decimal.Parse(linha.Cells[7].Text);

            obj.addItens(item);

            Session.Remove("objInventario" + urlSessao());
            Session.Add("objInventario" + urlSessao(), obj);



        }
        protected void ImgPesquisaMercadoria_Click(object sender, ImageClickEventArgs e)
        {
            carregarMercadorias(false);
        }
        protected void txtfiltromercadoria_TextChanged(object sender, EventArgs e)
        {
            if (txtfiltromercadoria.Text.Length > 0)
            {
                carregarMercadorias(false);
            }
            else
            {
                carregarMercadorias(true);
            }
        }
        protected void imgLimpar_Click(object sender, ImageClickEventArgs e)
        {
            limparSelecaoMercadoria();
            carregarMercadorias(true);
        }
        protected void limparSelecaoMercadoria()
        {
            ddlGrupo.Text = "";
            ddlSubGrupo.Text = "";
            ddlDepartamento.Text = "";
            ddlCategoria.Text = "";
            ddlSeguimento.Text = "";
            ddlLinha.Text = "";
            txtFornecedor.Text = "";
            txtfiltromercadoria.Text = "";
            txtMarca.Text = "";
        }
        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTodos = (CheckBox)sender;
            foreach (GridViewRow item in gridMercadoria1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    chk.Checked = chkTodos.Checked;
                    //incluirMercadoria(chk);
                }
            }


        }
        protected void GridMercadoriaSelecionado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument) + 1;
            ArrayList selecionados = (ArrayList)Session["selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            selecionados.RemoveAt(index);
            Session.Remove("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("selecionados" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), selecionados);


        }
        protected void GridMercadoriaSelecionado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            object o = ViewState["gridLinha"];
            if (o != null)
            {
                int indexSelecionado = (int)o;

                if (e.Row.RowIndex.Equals(indexSelecionado))
                {
                    e.Row.RowState = DataControlRowState.Selected;
                }

                if (e.Row.RowState == DataControlRowState.Selected)
                {
                    //e.Row.Cells[0].Focus();
                    e.Row.FindControl("txtQtd").Focus();

                }
            }
        }

        protected void imgBtnIncluirSelecionados_Click(object sender, ImageClickEventArgs e)
        {
            verificaSelecionados();
        }

        protected void imgBtnImpEncerrado_Click(object sender, ImageClickEventArgs e)
        {

            abrirImpressao("ENCERRADO");
        }

        protected void imgBtnConferencia_Click(object sender, ImageClickEventArgs e)
        {
            abrirImpressao("CONFERENCIA");
        }
        private void abrirImpressao(String tipoRelatorio)
        {
            User usr = (User)Session["User"];
            String endereco = (usr.filial.ip.Equals("::1") ? "c:" : "\\\\" + usr.filial.ip);
            if (ddTipo.Text.Equals("DEVOLUCAO") && Funcoes.existePasta(endereco + "\\imprimePedido"))
            {
                inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
                StreamWriter ArqImprime = new StreamWriter(endereco + "\\imprimePedido\\Pedido" + obj.Codigo_inventario.Trim() + ".txt", false, Encoding.ASCII);
                ArqImprime.Write(GerarArquivo(obj));
                ArqImprime.Close();
            }
            else
            {
                inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
                Session.Remove("objImprimir");
                Session.Add("objImprimir", obj);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refdrts", "window.open('OutrasMovimentacoesPrint.aspx?TIPO=" + tipoRelatorio + "','_blank');", true);

            }

        }
        protected void imgBtnImprimir_Click(object sender, ImageClickEventArgs e)
        {

            abrirImpressao("CONTAGEM");


        }


        protected void checaArquivosImportar()
        {
            User usr = (User)Session["User"];
            if (usr != null)
            {
                //Limpar conteúdo do ListBox ltbArquivosParaImportacao
                ddlArquivos.Items.Clear();
                //Preenche ltbArquivosParaImportacao com conteudo do diretorio.
                String endereco = (usr.filial.ip.Equals("::1") ? "c:" : @"\\" + usr.filial.ip) + "\\Coletor";
                if (Funcoes.existePasta(endereco))
                {
                    string[] txtList = Directory.GetFiles(@endereco, "*.txt");
                    foreach (string f in txtList)
                    {
                        string strNomeArquivo = f.Substring(@endereco.Length + 1);
                        ddlArquivos.Items.Add(strNomeArquivo);
                    }

                    if (ddlArquivos.Items.Count <= 0)
                    {
                        ddlArquivos.Visible = false;

                        lblIdentificarArquivo.Visible = true;
                        lblIdentificarArquivo.Text = "Sem Arquivos Identificados na pasta " + endereco;

                    }
                    else
                    {
                        ddlArquivos.Visible = true;
                        lblIdentificarArquivo.Visible = false;
                    }

                }
                else
                {
                    ddlArquivos.Visible = false;

                    lblIdentificarArquivo.Visible = true;
                    lblIdentificarArquivo.Text = "Não Encontrado o diretorio:" + endereco;

                }
            }
        }

        protected void imgBuscaArquivo_Click(object sender, ImageClickEventArgs e)
        {
            checaArquivosImportar();
        }

        protected void ddTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddTipo.Text.Equals("INV.AGRUPAMENTO"))
            {
                if (verificaInvAgrupamentoNoMes())
                {
                    lblError.Text = "Já existe uma contagem deste tipo no mês.";
                    conteudo.Visible = false;
                    return;
                }
            }

            carregarDadosObj();
            carregarDados();
            gridCampos();
            divInventarioCompleto.Visible = ddTipo.Text.ToUpper().Trim().Equals("INVENTARIO");
            btnCriarInventarioCompleto.Visible = ddTipo.Text.ToUpper().Trim().Equals("INVENTARIO");
            txtPlu.Enabled = !ddTipo.Text.ToUpper().Trim().Equals("");

            if (ddTipo.Text.Equals(""))
            {
                divAvisoTipo.Visible = true;
                conteudo.Visible = false;

            }
            else
            {
                divAvisoTipo.Visible = false;
                conteudo.Visible = true;
            }
        }

        protected void chkDepartamento_CheckedChanged(object sender, EventArgs e)
        {
            inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
            obj.quebraDepartamento = chkDepartamento.Checked;
            obj.ordernarItens();
            gridItens.DataSource = obj.Itens;
            gridItens.DataBind();
        }



        protected void btnCriarInventarioCompleto_Click(object sender, EventArgs e)
        {

            modalIventarioCompleto.Show();
        }
        private void gerarIventarioCompleto()
        {
            lblError.Text = "";
            txtDescricao.BackColor = System.Drawing.Color.White;

            try
            {
                if (txtDescricao_inventario.Text.Equals(""))
                {
                    txtDescricao_inventario.BackColor = System.Drawing.Color.Red;
                    throw new Exception("O campo Descrição é obrigatorio");
                }

                carregarDadosObj();
                inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
                obj.gerarInventarioCompleto();

                Response.Redirect("OutrasMovimentacoesDetalhes.aspx?campoIndex=" + obj.Codigo_inventario);

            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }

        }

        protected void btnConfirmaIventarioCompleto_Click(object sender, ImageClickEventArgs e)
        {

            gerarIventarioCompleto();

        }
        protected void btnCancelaIventarioCompleto_Click(object sender, ImageClickEventArgs e)
        {
            modalIventarioCompleto.Hide();

        }

        protected void btnPag_Click(object sender, EventArgs e)
        {
            if (!status.Equals("visualizar"))
            {
                carregarDadosObj();
            }
            inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
            Button btn = (Button)sender;
            if (btn.ID.Equals("btnPagInicio"))
            {
                obj.gridInicio = 0;
                obj.gridFim = 100;
            }
            else if (btn.ID.Equals("btnPagAnterio"))
            {
                obj.gridInicio -= 100;
                obj.gridFim = obj.gridInicio + 100;
                if (obj.gridInicio < 0)
                {
                    obj.gridInicio = 0;
                    obj.gridFim = 100;
                }


            }
            else if (btn.ID.Equals("btnPagProximo"))
            {

                obj.gridInicio += 100;
                obj.gridFim = obj.gridInicio + 100;
                if (obj.gridFim > obj.arrItens.Count)
                {
                    decimal pagInicio = (decimal.Truncate(obj.arrItens.Count / 100) * 100);
                    obj.gridInicio = Convert.ToInt32(pagInicio);
                    obj.gridFim = obj.arrItens.Count;
                }
            }
            else if (btn.ID.Equals("btnPagFim"))
            {
                decimal pagInicio = (decimal.Truncate(obj.arrItens.Count / 100) * 100);
                obj.gridInicio = Convert.ToInt32(pagInicio);
                obj.gridFim = obj.arrItens.Count;
            }

            Session.Remove("objInventario" + urlSessao());
            Session.Add("objInventario" + urlSessao(), obj);
            carregarDados();
            if (status.Equals("visualizar"))
            {
                EnabledControls(gridItens, false);
            }


        }

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            int tab = TabContainer1.ActiveTabIndex;
            if (tab == 3)
            {
                //if (gridMercadoria1.Rows.Count <= 0)
                //{
                carregarGrupos("", "", "");
                carregarLinhas();
                //}
            }
        }


        protected void TimerImportaColetor_Tick(object sender, EventArgs e)
        {
            String resultado = (String)Session["resultadoImportaColetor"];
            String error = (String)Session["erroImportaColetor"];
            String aborta = (String)Session["abortaImportaColetor"];

            if (aborta != null)
            {
                TimerImportaColetor.Enabled = false;
                Session.Remove("abortaImportaColetor");
            }
            else if (resultado != null)
            {

                if (resultado.Contains("Importados com sucesso"))
                {
                    TimerImportaColetor.Enabled = false;
                    carregarDados();
                    lblError.Text = resultado;
                    lblError.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    lblDetalhesAguarde.Text = resultado;
                    lblDetalhesAguarde.ForeColor = System.Drawing.Color.Blue;
                }

            }



            if (error != null)
            {
                Session.Remove("erroImportaColetor");
                TimerImportaColetor.Enabled = false;
                lblError.Text = "Erro" + error;
                lblError.ForeColor = System.Drawing.Color.Red;
            }

            if (TimerImportaColetor.Enabled)
            {
                //int registros = 0;
                //if (Session["contItens"] != null)
                //{
                //    registros += 10;

                //}
                //else
                //{
                //    registros = (int)Session["contItens"];
                //    registros += 10;
                //}
                //Session.Remove("contItens");
                //Session.Add("contItens", registros);
                //Label3.Text = registros.ToString() + " Atualizados! Não Feche a Tela ate o fim da atualização! ";
                modalaguarde.Show();
            }
            else
            {
                modalaguarde.Hide();
            }


        }
        protected void imgLerComanda_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                TimerImportaColetor.Interval = 450;
                TimerImportaColetor.Enabled = true;

                System.Threading.Thread thComanda = new System.Threading.Thread(LerComanda);
                thComanda.Start();

                modalaguarde.Show();

            }
            catch (Exception err)
            {

                lblError.Text = "Erro na importacao do arquivo :" + err.Message;
            }
        }
        private void LerComanda()
        {

            try
            {
                //Coletor coletor = new Coletor();
                //System.Threading.Thread.Sleep(1000);
                carregarDadosObj();

                User usr = (User)Session["User"];

                String strSQL = "";
                inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
                Hashtable tbPlus = new Hashtable();
                //int vdecimal = int.Parse(txtDecimaisContado.Text) + 1;
                String strPlus = "";
                //int inicioPlu = int.Parse(txtinicioPlu.Text) - 1;
                //int tamPlu = 0;
                //int inicioContado = int.Parse(txtInicioContado.Text) - 1;
                //int tamContado = 0;
                //if (!coletor.coletorFixo)
                //{
                //    tamPlu = (int.Parse(txtFimPlu.Text)) - inicioPlu;
                //    tamContado = (int.Parse(txtFimContado.Text)) - inicioContado;
                //}
                String contado = "0";
                String plu = "";
                int iLinha = 0;

                //if (txtComandasImportadas.Text.IndexOf(txtNumeroComanda.Text + ",") > 0)
                //{
                //    lblError.Text = "Não é permitido LER a mesma comanda durante o lançamento.";
                //    return;
                //}

                if (Conexao.countSql("SELECT * FROM lerComandaImporta WHERE Comanda = " + txtNumeroComanda.Text, null) > 0)
                {
                    lblError.Text = "Comanda: " + txtNumeroComanda.Text + " já foi importada. Favor escolher outra comanda.";
                    return;
                }

                Conexao.executarSql("delete from lerArquivoImporta ");

                DataTable dtComanda;
                dtComanda = Conexao.GetTable("sp_m_consulta_comanda_visual " + txtNumeroComanda.Text + ", 'MATRIZ', 0", usr, false);

                if (dtComanda.Rows.Count > 0)
                {
                    Conexao.executarSql("INSERT INTO lerComandaImporta VALUES (" + txtNumeroComanda.Text + ")");
                    foreach (DataRow row in dtComanda.Rows)
                    {

                        if (isnumero(row[0].ToString()))
                        {
                            iLinha++;

                            plu = row[0].ToString();
                            contado = row[2].ToString();

                            long lPlu = 0;
                            long.TryParse(plu.Trim(), out lPlu);
                            String sqlImpo = "";
                            if (!tbPlus.ContainsKey(lPlu.ToString()))
                            {
                                tbPlus.Add(lPlu.ToString(), contado.Trim());
                                sqlImpo = "insert into lerArquivoImporta values ('" + lPlu.ToString() + "');";
                                Conexao.executarSql(sqlImpo);
                            }
                            else
                            {
                                String contPlu = tbPlus[lPlu.ToString()].ToString();
                                contado = (Decimal.Parse(contPlu) + Decimal.Parse(contado)).ToString();
                                tbPlus[lPlu.ToString()] = contado;
                            }

                            Session.Remove("resultadoImportaColetor");
                            Session.Add("resultadoImportaColetor", "lido " + iLinha.ToString() + " linhas do arquivo");
                        }
                    }

                }

                strSQL = "Select EAN = ISNULL((SELECT TOP 1 EAN.EAN FROM EAN WHERE EAN.PLU = MERCADORIA.PLU), ''), ";
                strSQL += "mercadoria.PLU,mercadoria.descricao, isnull(mercadoria_loja.saldo_atual,0)saldo_atual , ";
                strSQL += "isnull(mercadoria.Ref_Fornecedor, '') as Referencia ";
                strSQL += ",mercadoria_loja.preco, mercadoria_loja.preco_custo ";
                strSQL += " from mercadoria inner join mercadoria_loja on mercadoria.plu=mercadoria_loja.plu ";
                strSQL += "where (mercadoria_loja.filial='" + usr.getFilial() + "') AND  (mercadoria.plu in(Select codigo from lerArquivoImporta) ";
                strSQL += "or mercadoria.plu in(SELECT  EAN.PLU FROM EAN WHERE convert(numeric,EAN.EAN  ) IN(Select codigo from lerArquivoImporta))) ";
                strSQL += "group by mercadoria.PLU,mercadoria.descricao, isnull(mercadoria_loja.saldo_atual,0),isnull(mercadoria.Ref_Fornecedor, ''),mercadoria_loja.preco, mercadoria_loja.preco_custo ";


                SqlDataReader rsplu = null;
                try
                {
                    rsplu = Conexao.consulta(strSQL, null, false);

                    while (rsplu.Read())
                    {

                        inventario_itensDAO item = new inventario_itensDAO(usr);
                        item.PLU = rsplu["plu"].ToString();

                        if (rsplu["ean"].ToString().Trim().Length > 6)
                        {
                            item.EAN = rsplu["ean"].ToString().Trim();
                        }
                        else
                        {
                            item.EAN = "";
                        }
                        item.Referencia = rsplu["Referencia"].ToString();
                        item.Saldo_atual = Decimal.Parse(rsplu["saldo_atual"].ToString());

                        if (tbPlus.ContainsKey(item.PLU.Trim()))
                            contado = tbPlus[item.PLU.Trim()].ToString();
                        else
                        {

                            long loEan = 0;

                            long.TryParse(rsplu["ean"].ToString().Trim(), out loEan);

                            String strEan = loEan.ToString();

                            if (!tbPlus.ContainsKey(strEan))
                            {
                                SqlDataReader rsEans = Conexao.consulta("Select ean from ean where plu ='" + item.PLU + "' and ean <>'" + strEan + "'", usr, false);
                                while (rsEans.Read())
                                {
                                    long.TryParse(rsEans["ean"].ToString().Trim(), out loEan);


                                    if (tbPlus.ContainsKey(loEan.ToString()))
                                    {

                                        strEan = loEan.ToString();
                                        item.EAN = strEan;
                                        break;
                                    }

                                }
                                if (rsEans != null)
                                    rsEans.Close();
                            }
                            try
                            {

                                contado = tbPlus[strEan].ToString();
                            }
                            catch (Exception err)
                            {

                                throw new Exception(err.Message + " não encontrado" + strEan);
                            }
                        }

                        //int casaDec = (contado.Length-vdecimal);
                        //item.Contada = Decimal.Parse(contado.Substring(0, casaDec) + '.' + contado.Substring(casaDec, vdecimal));

                        item.Contada = Decimal.Parse(contado); // / int.Parse("1".PadRight(coletor.contadoDecimal, '0'));
                        if (obj.tSaidaEntrada == 1)
                        {

                            Decimal.TryParse(rsplu["preco"].ToString(), out item.Custo);
                        }
                        else
                        {
                            Decimal.TryParse(rsplu["preco_custo"].ToString(), out item.Custo);
                        }


                        obj.addItens(item);
                        Session.Remove("resultadoImportaColetor");
                        Session.Add("resultadoImportaColetor", obj.arrItens.Count.ToString() + " itens Importados");
                    }
                }
                catch (Exception err)
                {

                    throw new Exception(err.Message); ;
                }
                finally
                {
                    if (rsplu != null)
                        rsplu.Close();
                }
                
                Session.Remove("objInventario" + urlSessao());
                Session.Add("objInventario" + urlSessao(), obj);
                carregarDados();
                TabContainer1.ActiveTabIndex = 0;
                Session.Remove("resultadoImportaColetor");
                Session.Add("resultadoImportaColetor", "Todos " + obj.arrItens.Count.ToString() + " itens Importados com sucesso");
            }
            catch (Exception err)
            {
                Session.Remove("erroImportaColetor");
                Session.Add("erroImportaColetor", err.Message);


            }


        }

        protected void txtGridContado_textChanged(object sender, EventArgs e)
        {
            //
            calculaTudo();
            carregarDadosObj();
        }

        protected void txtGirdContado_Enter(object sender, System.EventArgs e)
        {
            linhaAtivaGridView = gridItens.SelectedIndex;
        }

        public event EventHandler Enter;

        private void calculaTudo()
        {

            Decimal dblqtdeSaldo = 0;
            Decimal dblqtdeContada = 0;
            Decimal dbldivergenciaApurada = 0;

            for (int i = 0; i < gridItens.Rows.Count; i++)
            {
                TextBox txtBox1 = (TextBox)gridItens.Rows[i].FindControl("txtGridContado");
                if (txtBox1.Enabled)
                {
                    dblqtdeContada = Decimal.Parse((txtBox1.Text.Equals("") ? "0" : txtBox1.Text));
                    Decimal.TryParse(gridItens.Rows[i].Cells[5].Text, out dblqtdeSaldo);

                    dbldivergenciaApurada = dblqtdeContada - dblqtdeSaldo;
                    gridItens.Rows[i].Cells[9].Text  = String.Format("{0:N3}", dbldivergenciaApurada);
                }
            }

        }

        protected void txtcodListaPadrao_TextChanged(object sender, EventArgs e)
        {
            if (!txtcodListaPadrao.Text.Equals(""))
            {
                txtDescricaoListaPadrao.Text = Conexao.retornaUmValor("Select descricao from LISTA_PADRAO WHERE ID =" + txtcodListaPadrao.Text + " AND TIPO ='INV.AGRUPA'", null);

                carregarMercadorias(false);

            }
            else
            {
                carregarMercadorias(true);
                txtDescricaoListaPadrao.Text = "";
            }

        }

        protected void imgBtnListaPadrao_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("ListaPadrao");
        }

        protected bool verificaInvAgrupamentoNoMes()
        {
            string sql = "SELECT COUNT(*) AS REG FROM inventario i inner join inventario_itens ii ON i.Codigo_inventario = ii.Codigo_inventario AND";
            sql += " i.tipoMovimentacao = 'INV.AGRUPAMENTO'"; //-- AND i.STATUS = 'ENCERRADO'";
            sql += " AND SUBSTRING(CONVERT(VARCHAR, i.data, 102),1, 7)  = '" + DateTime.Now.ToString("yyyy.MM") + "'";

            try
            {
                if (Funcoes.intTry(Conexao.retornaUmValor(sql, null)) > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            int progresso = (int)(Session[ProgressKey] ?? 0);
            if (progresso < 100)
            {
                progressBar.Style["width"] = progresso + "%";
                litPercentual.Text = "<span style='color: black; font-weight: bold;'>" +  progresso.ToString() + "%</span> ";
            }
            else
            {
                progressBar.Style["width"] = "100%";
                litPercentual.Text = "<span style='color: black; font-weight: bold;'>CONCLUÍDO</span> "; ;

                Timer1.Enabled = false;

                String resultado = (String)Session["resultadoMovimento"];

                lblError.Text = resultado;
                lblError.ForeColor = System.Drawing.Color.Blue;
                visualizar(pnBtn);
                carregarDados();
                HabilitarCampos(false);

            }
        }
    }
}