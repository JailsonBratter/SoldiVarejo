using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using visualSysWeb.modulos.Estoque.code;

namespace visualSysWeb.modulos.Estoque.pages
{
    public partial class ListaPadraoProdutos : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["tipo"] != null)
            {
                pesquisar(pnBtn);
                if (!IsPostBack)
                {
                    ListaCompraPrecoFiltro filtro = (ListaCompraPrecoFiltro)Session["filtro" + urlSessao()];
                    if (filtro != null)
                    {
                        txtCodigo.Text = filtro.Codigo;
                        txtDescricao.Text = filtro.Descricao;

                    }

                    carregarPesquisa();
                }

            }
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            String tipo = Request["tipo"].ToString();
            Response.Redirect("~/modulos/estoque/pages/ListaPadraoProdutosDetalhes.aspx?novo=true&tipo="+tipo); 
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            carregarPesquisa();
        }

        protected void carregarPesquisa()
        {

            ListaCompraPrecoFiltro filtro = new ListaCompraPrecoFiltro();
            filtro.Codigo = txtCodigo.Text;
            filtro.Descricao = txtDescricao.Text;

            String tipo = Request["tipo"].ToString();
            filtro.tipo = tipo;
            lblTipoLista.Text = tipo;


            Session.Remove("filtro" + urlSessao());
            Session.Add("filtro" + urlSessao(), filtro);


            String sql = "Select * from lista_padrao  where tipo ='"+tipo+"'";


            if (!txtCodigo.Text.Equals("")) //colocar nome do campo de pesquisa
            {
                sql += " AND id = " + txtCodigo.Text; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
            }
            if (!txtDescricao.Text.Equals("")) //colocar nome do campo de pesquisa2
            {

                sql += " AND descricao like '" + txtDescricao.Text + "%'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
            }



            try
            {
                User usr = (User)Session["User"];

                gridPesquisa.DataSource = Conexao.GetTable(sql + " order by id desc ", usr, false);
                gridPesquisa.DataBind();
                lblPesquisaErro.Text = "";
            }
            catch (Exception err)
            {
                lblPesquisaErro.Text = err.Message;
            }
        }
        protected override void btnEditar_Click(object sender, EventArgs e) { }
        protected override void btnExcluir_Click(object sender, EventArgs e) { }
        protected override void btnConfirmar_Click(object sender, EventArgs e) { }
        protected override void btnCancelar_Click(object sender, EventArgs e) { }


        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected override bool campoDesabilitado(Control campo)
        {
            return false;
        }



    }
}