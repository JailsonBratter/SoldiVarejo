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
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Manutencao.pages
{
    public partial class OutrasMovimentacoesDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];

            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    inventarioDAO obj = new inventarioDAO(usr);
                    Session.Remove("objInventario" + urlSessao());
                    Session.Add("objInventario" + urlSessao(), obj);
                    status = "incluir";
                    txtstatus.Text = "INICIADO";
                    incluir(pnBtn);
                    HabilitarCampos(true);

                    txtData.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtUsuario.Text = usr.getNome();

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
                            Session.Remove("objInventario" + urlSessao());
                            Session.Add("objInventario" + urlSessao(), obj);

                            carregarDados();
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

           

        }

        protected void HabilitarCampos(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
            btnEncerrar.Visible = enable;
            btnImportarArquivo.Visible = enable;
            AjaxFileUpload1.Visible = enable;
            RdoTipoDeArquivo.Enabled = enable;
            if (enable)
            {
                BtnImgTipoMovimentacao.Visible = txtTipoMovimentacao.Text.Trim().Equals("");
                txtPlu.Enabled = !txtTipoMovimentacao.Text.Trim().Equals("");
                txtCusto.Enabled = true;
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

            txtTipoMovimentacao.BackColor = System.Drawing.Color.White;
            if (txtTipoMovimentacao.Text.Trim().Equals(""))
            {
                txtTipoMovimentacao.BackColor = System.Drawing.Color.Red;
                throw new Exception("Selecione um tipo de movimentacao");
            }
            if (validaCampos(cabecalho) && validaCampos(conteudo))
                return true;
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "txtTipoMovimentacao", 
                                    "", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            if (campo.ID != null)
            {
                
                String[] campos = { "txtCodigo_inventario", 
                                   "txtUsuario", 
                                   "txtData", 
                                    "txtDescricao",
                                    "TxtSaldoAtual",
                                    "txtstatus",
                                    "txtTipoMovimentacao"
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


        private void salvarMovimentacao(String strStatus)
        {
            try
            {
                if (validaCamposObrigatorios())
                {
                    txtstatus.Text = strStatus;
                    carregarDadosObj();
                    inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
                    
                    obj.salvar(status.Equals("incluir")); // se for incluir true se não falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    HabilitarCampos(false);
                    visualizar(pnBtn);
                    carregarDados();
                }
                else
                {
                    lblError.Text = "Campo Obrigatorio não preenchido";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("OutrasMovimentacoes.aspx");//colocar endereco pagina de pesquisa
        }

        //--Atualizar DaoForm 
        private void carregarDados()
        {
            inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
            txtTipoMovimentacao.Text = obj.tipoMovimentacao.ToString();
            txtCodigo_inventario.Text = obj.Codigo_inventario.ToString();
            txtDescricao_inventario.Text = obj.Descricao_inventario.ToString();
            txtData.Text = obj.DataBr();
            txtUsuario.Text = obj.Usuario.ToString();
            txtstatus.Text = obj.status.ToString();
            gridItens.DataSource = obj.Itens;
            gridItens.DataBind();
            if (obj.tSaidaEntrada == 2)
            {
                lblContado.Text = "Contado";
                gridItens.Columns[5].Visible = true;
                gridItens.Columns[7].Visible = true;

                gridItens.Columns[6].Visible = false;
                gridItens.Columns[8].Visible = false;
            }
            else
            {
                lblContado.Text = "Qtde";

                gridItens.Columns[5].Visible = false;
                gridItens.Columns[7].Visible = false;

                gridItens.Columns[6].Visible = true;
                gridItens.Columns[8].Visible = true;
            }

        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
            obj.tipoMovimentacao = txtTipoMovimentacao.Text;
            obj.Codigo_inventario = txtCodigo_inventario.Text;
            obj.Descricao_inventario = txtDescricao_inventario.Text;
            obj.Data = DateTime.Parse(txtData.Text);
            obj.Usuario = txtUsuario.Text;
            obj.status = txtstatus.Text;
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
                    item.Saldo_atual = Decimal.Parse(TxtSaldoAtual.Text);
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

                    addItens.DefaultButton = "imgPlu";
                    txtPlu.Focus();
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (isnumero(e.Row.Cells[i].Text))
                    {
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                        e.Row.Cells[i].Text = Decimal.Parse(e.Row.Cells[i].Text).ToString("N2");

                    }
                }
            }
        }
        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.Equals("Alterar"))
            {
                txtPlu.Text = gridItens.Rows[index].Cells[1].Text;
                txtDescricao.Text = gridItens.Rows[index].Cells[2].Text;
                TxtSaldoAtual.Text = gridItens.Rows[index].Cells[3].Text;
                txtContado.Text = gridItens.Rows[index].Cells[5].Text;
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

        protected void carregarPlu()
        {
            User usr = (User)Session["User"];
            SqlDataReader rsplu = Conexao.consulta("Select top 1 mercadoria.PLU,mercadoria.descricao, isnull(mercadoria_loja.saldo_atual,0)saldo_atual,isnull(mercadoria_loja.preco_custo,0)custo from mercadoria inner join mercadoria_loja on mercadoria.plu=mercadoria_loja.plu left join ean on mercadoria.plu=ean.plu where mercadoria_loja.filial='"+usr.getFilial()+"' and ( mercadoria.plu='" + txtPlu.Text + "' or ean.EAN='" + txtPlu.Text + "')", null, false);
            if (rsplu.Read())
            {
                txtPlu.Text = rsplu["plu"].ToString();
                txtDescricao.Text = rsplu["descricao"].ToString();
                TxtSaldoAtual.Text = Decimal.Parse(rsplu["saldo_atual"].ToString()).ToString("N2");
                txtCusto.Text = Decimal.Parse(rsplu["custo"].ToString()).ToString("N2");
                txtContado.Focus();
                addItens.DefaultButton = "ImgBtnAddItens";
            }
            else
            {
                lblError.Text = "Produto não encontrado!!";
                lblError.ForeColor = System.Drawing.Color.Red;
                txtPlu.Focus();

            }

            if (rsplu != null)
                rsplu.Close();
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
                    sqlLista = "select mercadoria.PLU, ISNULL(mercadoria.Ref_Fornecedor,'') as REFERENCIA, DESCRICAO,isnull(ean.ean,'') EAN from mercadoria left join ean on ean.plu=mercadoria.plu where ";
                    sqlLista = sqlLista + " mercadoria.plu like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%'  or mercadoria.Ref_fornecedor like '%" + TxtPesquisaLista.Text + "%' or (ean like '%" + TxtPesquisaLista.Text + "%') ORDER BY DESCRICAO";

                    break;
                case "TipoMovimentacao":
                    lbltituloLista.Text = "Escolha um  Tipo de Movimentação";
                    sqlLista = "select Movimentacao from Tipo_movimentacao where (Movimentacao like '%" + TxtPesquisaLista.Text + "%') order by Movimentacao ";

                    break;
            }
            User usr = (User)Session["User"];
            GridLista.DataSource = Conexao.GetTable(sqlLista, null, true);
            GridLista.DataBind();

            modalLista.Show();
            TxtPesquisaLista.Focus();
        }

        protected void btnEncerrar_Click(object sender, EventArgs e)
        {
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
                case "TipoMovimentacao":
                    txtTipoMovimentacao.Text = ListaSelecionada(1);
                    BtnImgTipoMovimentacao.Visible = false;
                    txtPlu.Enabled = true;
                    carregarDadosObj();
                    carregarDados();
                    break;
            }

            modalLista.Hide();
        }

        private void Ler()
        {
            carregarDadosObj();
            String arquivo = Server.MapPath("") + "\\importacao\\ArqImportado.txt";
            User usr = (User)Session["User"];
            string[] lines = System.IO.File.ReadAllLines(arquivo);
            inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];

            foreach (string line in lines)
            {
                if (line.Length > 0)
                {
                    int inicioPlu = int.Parse(txtinicioPlu.Text);
                    int tamPlu = 0;
                    int inicioContado = int.Parse(txtInicioContado.Text);
                    int tamContado = 0;
                    int vdecimal = int.Parse(txtDecimaisContado.Text) + 1;
                    String contado = "";
                    String plu = "";
                    if (RdoTipoDeArquivo.SelectedValue.Equals("0"))
                    {
                        tamPlu = int.Parse(txtFimPlu.Text) - inicioPlu;
                        tamContado = int.Parse(txtFimContado.Text) - inicioContado;
                        plu = line.Substring(inicioPlu, tamPlu);
                        contado = line.Substring(inicioContado, tamContado);
                    }
                    else
                    {
                        string[] arrLinha = line.Split(txtDemilitador.Text[0]);
                        plu = arrLinha[inicioPlu];
                        contado = arrLinha[inicioContado];
                    }

                    int t = line.Length;


                    SqlDataReader rsplu = Conexao.consulta("Select top 1 mercadoria.PLU,mercadoria.descricao, isnull(mercadoria_loja.saldo_atual,0)saldo_atual from mercadoria inner join mercadoria_loja on mercadoria.plu=mercadoria_loja.plu left join ean on mercadoria.plu=ean.plu where mercadoria.plu=" + plu + " or ean.EAN='" + plu + "'", null, false);
                    if (rsplu.Read())
                    {
                        inventario_itensDAO item = new inventario_itensDAO(usr);
                        item.PLU = rsplu["plu"].ToString();
                        item.Saldo_atual = Decimal.Parse(rsplu["saldo_atual"].ToString());

                        //int casaDec = (contado.Length-vdecimal);
                        //item.Contada = Decimal.Parse(contado.Substring(0, casaDec) + '.' + contado.Substring(casaDec, vdecimal));
                        item.Contada = Decimal.Parse(contado) / int.Parse("1".PadRight(vdecimal, '0'));
                        obj.addItens(item);
                    }

                    if (rsplu != null)
                        rsplu.Close();
                }
            }
            Session.Remove("objInventario" + urlSessao());
            Session.Add("objInventario" + urlSessao(), obj);
            carregarDados();
            TabContainer1.ActiveTabIndex = 0;
            System.IO.File.Delete(arquivo);

        }

        protected void AjaxFileUpload1_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            try
            {

                String caminho = Server.MapPath("") + "\\importacao\\ArqImportado.txt";
                AjaxFileUpload1.SaveAs(caminho);


            }
            catch (Exception err)
            {

                lblError.Text = "Erro de importação erro: " + err.Message;
            }
        }

        protected void RdoTipoDeArquivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RdoTipoDeArquivo.SelectedValue.Equals("0"))
            {
                txtDemilitador.Text = "";
                txtDemilitador.Visible = false;
                lblDelimitador.Visible = false;
                lblFimPlu.Visible = true;
                txtFimPlu.Visible = true;
                lblFimContado.Visible = true;
                txtFimContado.Visible = true;
                lblInicioPlu.Text = "Inicio";
                lblInicioContado.Text = "Inicio";
                txtinicioPlu.Focus();
            }
            else
            {
                lblDelimitador.Visible = true;
                txtDemilitador.Visible = true;
                txtDemilitador.Focus();
                txtFimPlu.Text = "";
                lblFimPlu.Visible = false;
                txtFimPlu.Visible = false;
                lblFimContado.Visible = false;
                txtFimContado.Text = "";
                txtFimContado.Visible = false;
                lblInicioPlu.Text = "Posição";
                lblInicioContado.Text = "Posição";
            }
        }

        protected void btnImportarArquivo_Click(object sender, EventArgs e)
        {
            try
            {
                Ler();
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
            salvarMovimentacao("ENCERRADO");
            modalEncerrar.Hide();
        }

        protected void btnCancelarEncerrar_Click(object sender, ImageClickEventArgs e)
        {
            modalEncerrar.Hide();
        }

        protected void ImgBtnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            User usr = (User)Session["User"];
            String endereco = (usr.filial.ip.Equals("::1") ? "c:" : "\\\\" + usr.filial.ip);
            if (Funcoes.existePasta(endereco + "\\imprimePedido"))
            {
                inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
                StreamWriter ArqImprime = new StreamWriter(endereco + "\\imprimePedido\\Pedido" + obj.Codigo_inventario.Trim() + ".txt", false, Encoding.ASCII);
                ArqImprime.Write(GerarArquivo(obj));
                ArqImprime.Close();
            }

        }

        protected String GerarArquivo(inventarioDAO  inv)
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
                strLinhaReg = "**** OUTRAS MOVIMENTACOES ***************************************************" +(char)13 + (char)10;
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
            User usr = (User)Session["User"];
            String endereco = (usr.filial.ip.Equals("::1") ? "c:" : "\\\\" + usr.filial.ip);
            if (Funcoes.existePasta(endereco + "\\imprimePedido"))
            {
                inventarioDAO obj = (inventarioDAO)Session["objInventario" + urlSessao()];
                StreamWriter ArqImprime = new StreamWriter(endereco + "\\imprimePedido\\Pedido" + obj.Codigo_inventario.Trim() + ".txt", false, Encoding.ASCII);
                ArqImprime.Write(GerarArquivo(obj));
                ArqImprime.Close();
            }

        }
    }
}