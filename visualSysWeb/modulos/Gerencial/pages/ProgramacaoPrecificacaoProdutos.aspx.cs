using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;
using visualSysWeb.modulos.Gerencial.code;

namespace visualSysWeb.modulos.Gerencial.pages
{
    public partial class ProgramacaoPrecificacaoProdutos : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
                pesquisar(pnBtn);
                if (!IsPostBack)
                {
                    PrecificacaoFiltro filtro = (PrecificacaoFiltro)Session["filtro" + urlSessao()];
                    if (filtro != null)
                    {
                        txtId.Text = filtro.Id;
                        txtFilial.Text = filtro.Filial;
                        txtDescricao.Text = filtro.Descricao;
                        txtPlu.Text = filtro.Plu;
                        txtUsuario.Text = filtro.Usuario;
                        txtDe.Text = filtro.DtDe;
                        txtAte.Text = filtro.DtAte;
                        DdlTipoPesquisa.SelectedValue = filtro.PesqPor;

                    }

                    carregarPesquisa();
                }

        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("~/modulos/gerencial/pages/ProgramacaoPrecificacaoProdutosDetalhes.aspx?novo=true"); 
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            carregarPesquisa();
        }

        protected void carregarPesquisa()
        {

            PrecificacaoFiltro filtro = new PrecificacaoFiltro();
            filtro.Id = txtId.Text;
            filtro.Filial = txtFilial.Text;
            filtro.Descricao = txtDescricao.Text;
            filtro.Plu = txtPlu.Text;
            filtro.Usuario = txtUsuario.Text;
            filtro.DtDe = txtDe.Text;
            filtro.DtAte = txtAte.Text;
            filtro.PesqPor = DdlTipoPesquisa.SelectedItem.Value;
          


            Session.Remove("filtro" + urlSessao());
            Session.Add("filtro" + urlSessao(), filtro);


            String sql = "Select *, data_cadastroBr = convert(varchar,data_cadastro,103), data_iniciobr=Convert(varchar,data_inicio,103) from Programacao_Precificacao ";
            String where = " where isnull(excluido,0) =0 ";

            if (!txtId.Text.Equals("")) //colocar nome do campo de pesquisa
            {
                where += " and   id = " + txtId.Text; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
            }
            if(!txtFilial.Text.Equals(""))
            {
                    where += " AND  FILIAL = '" + txtFilial.Text + "'";
            }


            if (!txtDescricao.Text.Equals("")) 
            {
                    where += " AND  descricao like '" + txtDescricao.Text + "%'";
            }

            if (!txtPlu.Text.Equals("")) 
            {
                    where += " AND  id in (Select id_precificacao from Programacao_precificacao_itens where plu ='"+txtPlu.Text+"') ";
            }

            if (!txtUsuario.Text.Equals("")) 
            {
                where += " and usuario_cadastro = '"+txtUsuario.Text+"'";
            }
            
            if(!txtDe.Text.Equals(""))
            {
                where += " AND ";
                where += DdlTipoPesquisa.SelectedItem.Value + " between " +Funcoes.dateSql(Funcoes.dtTry(txtDe.Text)) + " and " + Funcoes.dateSql(Funcoes.dtTry(txtAte.Text)) ;
            }


            try
            {
                bool limitar = true;
                if (where.Length > 0)
                {
                    sql +=  where;
                    limitar = false;
                }


                User usr = (User)Session["User"];

                gridPesquisa.DataSource = Conexao.GetTable(sql + " order by id desc ", null, limitar);
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

        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe"];
            String sqlLista = "";
            bool limitar = false;

            switch (or)
            {
                case "Plu":
                    sqlLista = "select Plu, Descricao from Mercadoria where plu like '%" + TxtPesquisaLista.Text + "%' or descricao like '%" + TxtPesquisaLista.Text + "%'"; ;
                    lbllista.Text = "Plu";
                    limitar = true;
                    break;
                case "Filial":
                    sqlLista = "select Filial='TODAS' UNION ALL  select Filial from Filial where filial like '%" + TxtPesquisaLista.Text + "%' " ;
                    lbllista.Text = "Filiais";
                    break;
                case "Usuario":
                    sqlLista = "select Usuario,Nome from usuarios_web where Usuario like '%" + TxtPesquisaLista.Text + "%' or Nome Like '%" + TxtPesquisaLista.Text + "%'";
                    lbllista.Text = "Usuarios";
                    
                    break;


            }
            GridLista.DataSource = Conexao.GetTable(sqlLista, null, limitar);
            GridLista.DataBind();
            if (GridLista.Rows.Count == 1)
            {
                if (!GridLista.Rows[0].Cells[1].Text.Equals("------"))
                {
                    RadioButton rdo = (RadioButton)GridLista.Rows[0].FindControl("RdoListaItem");
                    rdo.Checked = true;
                }
            }

            TxtPesquisaLista.Focus();
            modalPnFundo.Show();
        }

        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            TxtPesquisaLista.Text = "";
            Session.Remove("camporecebe");

            switch (btn.ID)
            {
                case "imgBtnPlu":
                    Session.Add("camporecebe", "Plu");
                    break;
                case "imgBtnFilial":
                    Session.Add("camporecebe", "Filial");
                    break;
                case "ImgBtnUsuario":
                    Session.Add("camporecebe", "Usuario");
                    break;
            }

            exibeLista();


        }
        protected String ListaSelecionada(int campo)
        {
            foreach (GridViewRow item in GridLista.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoListaItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {
                        return item.Cells[campo].Text;
                    }
                }
            }

            return "";
        }
        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            String selecionado = ListaSelecionada(1);

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {
                String listaAtual = (String)Session["camporecebe"];
                Session.Remove("camporecebe");

                if (listaAtual.Equals("Plu"))
                {

                    txtPlu.Text = ListaSelecionada(1);


                }
                if (listaAtual.Equals("Filial"))
                {
                    txtFilial.Text = ListaSelecionada(1);
                }
                if (listaAtual.Equals("Usuario"))
                {
                    txtUsuario.Text = ListaSelecionada(1);
                }


                modalPnFundo.Hide();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalPnFundo.Show();
            }


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



        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            pnFundo.Visible = false;
        }
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }

        protected void txtDe_TextChanged(object sender, EventArgs e)
        {
            txtAte.Text = txtDe.Text;
        }
    }
}