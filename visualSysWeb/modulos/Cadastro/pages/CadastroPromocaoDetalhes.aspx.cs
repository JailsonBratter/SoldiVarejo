using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;
using visualSysWeb.modulos.Cadastro.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class CadastroPromocaoDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];

            if (usr == null)
                return;

            if (!IsPostBack)
            {
                PromocaoDAO obj = null;
                if (Request.Params["novo"] != null)
                {
                    obj = new PromocaoDAO(usr);
                    obj.Inicio = DateTime.Now;
                    obj.Fim = DateTime.Now;
                    status = "incluir";
                }
                else
                {
                    status = "visualizar";
                    if (Request.Params["codigo"] != null)
                    {
                        String codPromocao = Request["codigo"].ToString();
                        obj = new PromocaoDAO(Funcoes.intTry(codPromocao), usr);
                    }
                    else
                    {
                        return;
                    }

                }

                Session.Remove("promocao" + urlSessao());
                Session.Add("promocao" + urlSessao(), obj);
                CarregarDados();
                HabilitarCampos(!status.Equals("visualizar"));


            }
            carregabtn(pnBtn);
        }

        private void HabilitarCampos(bool enable)
        {
            EnabledControls(cabecalho, enable);
            EnabledControls(conteudo, enable);
            tiposTela();
        }

        private void CarregarDados()
        {
            PromocaoDAO obj = (PromocaoDAO)Session["promocao" + urlSessao()];
            txtCodigo.Text = obj.Codigo.ToString();
            txtDescricao.Text = obj.Descricao;
            txtDtInicio.Text = obj.Inicio.ToString("dd/MM/yyyy");
            txtDtFim.Text = obj.Fim.ToString("dd/MM/yyyy");
            txtParam_Base.Text = obj.Param_Base.ToString();
            txtParam_Brinde.Text = obj.Param_Brinde.ToString(); 
            ddlTipo.SelectedValue = obj.Tipo.ToString();
            carregarGrids();

        }

        protected void carregarGrids()
        {
            PromocaoDAO obj = (PromocaoDAO)Session["promocao" + urlSessao()];
            gridItens.DataSource = obj.itensBase;
            gridItens.DataBind();

            gridBrindes.DataSource = obj.itensBrinde;
            gridBrindes.DataBind();
        }
        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            Response.Redirect("CadastroPromocao.aspx?tela=" + usr.tela);
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {

                CarregarDadosObj();
                if (validaCamposObrigatorios())
                {
                    
                    PromocaoDAO obj = (PromocaoDAO)Session["promocao" + urlSessao()];
                    obj.Salvar(status.Equals("incluir"));
                    status = "visualizar";
                    CarregarDados();
                    HabilitarCampos(false);
                    showMessagem("Salvo Com Sucesso !", false);

                }
            }
            catch (Exception err)
            {

                showMessagem(err.Message, true);
            }
        }

        private void CarregarDadosObj()
        {
            PromocaoDAO obj = (PromocaoDAO)Session["promocao" + urlSessao()];
            obj.Codigo = Funcoes.intTry(txtCodigo.Text);
            obj.Tipo = Funcoes.intTry(ddlTipo.SelectedItem.Value);
            obj.Inicio = Funcoes.dtTry(txtDtInicio.Text);
            obj.Fim = Funcoes.dtTry(txtDtFim.Text);
            obj.Param_Base = Funcoes.decTry(txtParam_Base.Text);
            obj.Param_Brinde = Funcoes.decTry(txtParam_Brinde.Text);
            obj.Descricao = txtDescricao.Text;
        }

        protected bool validaCamposObrigatorios()
        {
            int intParamBase = Funcoes.intTry(txtParam_Base.Text);
            int intParamBrinde = Funcoes.intTry(txtParam_Brinde.Text);

            if (intParamBrinde >= intParamBase)
            {
                txtParam_Brinde.BackColor = System.Drawing.Color.Red;
                throw new Exception("O Desconto/Brinde não pode ser maior ou igual a qtde de Referencia");

            }
            DateTime dtInicio = Funcoes.dtTry(txtDtInicio.Text);
            DateTime dtFim = Funcoes.dtTry(txtDtFim.Text);

            
            if(dtInicio < DateTime.Now.Date)
            {
                txtDtInicio.BackColor = System.Drawing.Color.Red;
                throw new Exception("Data de Inicio não pode ser menor que a data atual");
            }

            if(dtFim < dtInicio)
            {
                txtDtFim.BackColor = System.Drawing.Color.Red;
                throw new Exception("Data Fim não pode ser menor que a data Inicio");
            }

           


            if (!validaCampos(cabecalho) || !validaCampos(conteudo))
                return false;



            PromocaoDAO obj = (PromocaoDAO)Session["promocao" + urlSessao()];
            if(obj.Tipo==2 || obj.Tipo ==3)
            {
                if (obj.itensBase.Count == 0)
                    throw new Exception("Itens não incluidos");
            }
            
            if(obj.Tipo ==3)
            {
                if (obj.itensBrinde.Count == 0)
                    throw new Exception("Brindes não incluidos");
            }
            obj.validaItensPromocao();


            return true;
        }
        protected override void btnEditar_Click(object sender, EventArgs e)
        {
           
            editar(pnBtn);
            HabilitarCampos(true);
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalExcluir.Show();

        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            Response.Redirect("CadastroPromocaoDetalhes.aspx?novo=true&tela=" + usr.tela);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            Response.Redirect("CadastroPromocao.aspx?tela=" + usr.tela);
        }

        protected override bool campoDesabilitado(Control campo)
        {
            if (campo.ID == null)
            {
                return false;
            }
            switch (campo.ID)
            {
                case "txtCodigo":
                case "txtDescricaoItem":
                case "txtDescricaoBrinde":
                    return true;
                default:
                    return false;
            }
        }

        protected override bool campoObrigatorio(Control campo)
        {
            if (campo.ID == null)
            {
                return false;
            }
            switch (campo.ID)
            {
                case "txtDescricao":
                case "txtDtInicio":
                case "txtDtFim": 
                case "txtParam_Base":
                case "txtParam_Brinde":
                    return true;
                default:
                    return false;
            }
        }

        protected void gridItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            lblPluExcluirItem.Text = gridItens.Rows[index].Cells[1].Text;
            lblPluExcluirBrinde.Text = "";
            modalExcluirItem.Show();
        }

        protected void gridBrindes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            lblPluExcluirItem.Text = "";
            lblPluExcluirBrinde.Text = gridBrindes.Rows[index].Cells[1].Text;
            modalExcluirItem.Show();

        }

        protected void imgPlu_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                ImageButton btn = (ImageButton)sender;
                TextBox txt = null;
                if (btn.ID.Contains("Brinde"))
                {
                    txt = txtPluBrinde;
                }
                else
                {
                    txt = txtPlu;
                }
                if (txt.Text.Equals(""))
                {
                    Session.Remove("lista" + urlSessao());
                    Session.Add("lista" + urlSessao(), btn.ID);

                    exibeLista();
                }
                else
                {

                    pesquisaItem(txt);

                }

            }
            catch (Exception err)
            {

                showMessagem(err.Message, true);
            }
        }

        private void pesquisaItem(TextBox txt)
        {
            String sql = "Select descricao from mercadoria where plu ='" + txt.Text + "'";
            String descricao = Conexao.retornaUmValor(sql, null);
            if (descricao.Equals(""))
                throw new Exception("Item Não Encontrado");

            if (!txt.ID.Contains("Brinde"))
            {
                addItensDig.DefaultButton = "ImgBtnAddItens";
                txtDescricaoItem.Text = descricao;
                ImgBtnAddItens.Focus();
            }
            else
            {
                addItensDigBrinde.DefaultButton = "imgBtnAddBrinde";
                txtDescricaoBrinde.Text = descricao;
                imgBtnAddBrinde.Focus();
            }

        }

        private void exibeLista()
        {
            String ID = (String)Session["lista" + urlSessao()];
            String sql = "";
            switch (ID)
            {
                case "imgPlu":
                case "imgBtnPluBrinde":
                    sql = " Select top 300 PLU, Descricao from mercadoria where plu like '%" + TxtPesquisaLista.Text + "%' or Descricao like '%" + TxtPesquisaLista.Text + "%'";
                    lbltituloLista.Text = "Escolha o item";
                    break;

            }
            GridLista.DataSource = Conexao.GetTable(sql, null, false);
            GridLista.DataBind();

            modalLista.Show();
        }

        protected void ImgBtnAddItens_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            TextBox txtplu = null;
            TextBox txtdescricao = null;
            List<Promocao_itensDAO> lista = null;
            PromocaoDAO obj = (PromocaoDAO)Session["promocao" + urlSessao()];
            if (btn.ID.Contains("Brinde"))
            {
                txtplu = txtPluBrinde;
                txtdescricao = txtDescricaoBrinde;
                lista = obj.itensBrinde;
                addItensDigBrinde.DefaultButton = "imgBtnPluBrinde"; 
            }
            else
            {
                txtplu = txtPlu;
                txtdescricao = txtDescricaoItem;
                lista = obj.itensBase;
                addItensDig.DefaultButton = "imgPlu";
            }

            if (!txtplu.Text.Trim().Equals(""))
            {
                if (lista.Count(i => i.Plu.Equals(Funcoes.intTry(txtplu.Text))) == 0)
                {
                    Promocao_itensDAO item = new Promocao_itensDAO();
                    item.Plu = Funcoes.intTry(txtplu.Text);
                    item.Descricao = txtdescricao.Text;
                    lista.Add(item);
                }

            }
            txtplu.Text = "";
            txtdescricao.Text = "";
            txtplu.Focus();
            carregarGrids();




        }

        protected void btnOkError_Click(object sender, EventArgs e)
        {
            modalError.Hide();
        }

        protected void showMessagem(String msg, bool err)
        {
            lblErroPanel.Text = msg;
            if (err)
                lblErroPanel.ForeColor = System.Drawing.Color.Red;
            else
                lblErroPanel.ForeColor = System.Drawing.Color.Blue;
            modalError.Show();
        }

        protected void imgBntnExcluirPromocao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                PromocaoDAO obj = (PromocaoDAO)Session["promocao" + urlSessao()];
                obj.excluir();
                modalExcluir.Hide();
                showMessagem("Excluido Com Sucesso", false);
            }
            catch (Exception err)
            {
                showMessagem(err.Message, true);
            }

        }

        protected void imgBtnCancelarExcluir_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluir.Hide();
        }

        protected void imgBtnConfirmaExcluirItem_Click(object sender, ImageClickEventArgs e)
        {
            PromocaoDAO obj = (PromocaoDAO)Session["promocao" + urlSessao()];
            if (!lblPluExcluirItem.Text.Equals(""))
            {
                obj.itensBase.RemoveAll(i => i.Plu == Funcoes.intTry(lblPluExcluirItem.Text));
            }

            if (!lblPluExcluirBrinde.Text.Equals(""))
            {
                obj.itensBrinde.RemoveAll(i => i.Plu == Funcoes.intTry(lblPluExcluirBrinde.Text));
            }
            carregarGrids();
            modalExcluirItem.Hide();
        }

        protected void imgBtnCancelaExcluirItem_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluirItem.Hide();
        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
            String ID = (String)Session["lista" + urlSessao()];

            switch (ID)
            {
                case "imgPlu":
                    txtPlu.Text = ListaSelecionada(1);
                    txtDescricaoItem.Text = ListaSelecionada(2);

                    break;
                case "imgBtnPluBrinde":
                    txtPluBrinde.Text = ListaSelecionada(1);
                    txtDescricaoBrinde.Text = ListaSelecionada(2);
                    break;

            }

            modalLista.Hide();
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

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
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

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            tiposTela();

        }

        protected void tiposTela()
        {
            PromocaoDAO obj = (PromocaoDAO)Session["promocao" + urlSessao()];
            if (ddlTipo.SelectedItem.Value.Equals("1"))
            {
                lblParam_Base.Text = "Acima";
                lblParam_Brinde.Text = "Desconto";
                TabContainer1.Visible = false;
                obj.itensBase.Clear();
                obj.itensBrinde.Clear();
            }
            else if (ddlTipo.SelectedItem.Value.Equals("2"))
            {
                lblParam_Base.Text = "Qtde";
                lblParam_Brinde.Text = "Brinde";
                TabContainer1.Visible = true;
                tabItens.Visible = true;
                tabBrindes.Visible = false;
                obj.itensBrinde.Clear();
            }
            else
            {
                lblParam_Base.Text = "Qtde";
                lblParam_Brinde.Text = "Brinde";
                TabContainer1.Visible = true;
                tabItens.Visible = true;
                tabBrindes.Visible = true;

            }
        }
    }
}