using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class CadastroPromocao : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if(!IsPostBack)
            {
                AtualizaPesquisa();

            }
            pesquisar(pnBtn);

        }

        private void AtualizaPesquisa()
        {
            User usr = (User)Session["User"];

            if (usr == null)
                return;

            String sql = "Select * ,strTipo = case when tipo =1 then 'Desconto' " +
                "                                  when tipo =2 then 'Leve X pague Y'" +
                                                 " when tipo =3 then 'Brinde' end" +
                                                 " from Promocao ";
            String where = "";

            if(txtCodigo.Text.Trim().Length>0)
            {
                where += " codigo =" + txtCodigo.Text;
            }
            else 
            {
                if (txtDescricao.Text.Trim().Length > 0)
                    where += " Descricao like '%" + txtDescricao.Text + "%'";

                if(IsDate(txtDataDe.Text))
                {
                    if (where.Length > 0)
                        where += " and ";
                    DateTime Inicio = Funcoes.dtTry(txtDataDe.Text);
                    DateTime Fim = Funcoes.dtTry(txtDataAte.Text);
                    where += " (('" + Inicio.ToString("yyyy-MM-dd") + "' between promocao.Inicio and promocao.fim )";
                    where += "  or ('" + Fim.ToString("yyyy-MM-dd") + "' between  promocao.Inicio and promocao.fim)" +
                              " or ('" + Inicio.ToString("yyyy-MM-dd") + "' <= Promocao.Inicio and '" + Fim.ToString("yyyy-MM-dd") + "'>= promocao.fim ))";
                }

                if(!ddlTipo.SelectedItem.Value.Equals("0"))
                {
                    if (where.Length > 0)
                        where += " and ";

                    where += " TIPO =" + ddlTipo.SelectedItem.Value;
                }
            }

            if (where.Length > 0)
                sql += " where " + where ;




            gridPesquisa.DataSource = Conexao.GetTable(sql+ " Order by codigo desc", usr, false);
            gridPesquisa.DataBind();

        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("CadastroPromocaoDetalhes.aspx?novo=true&tela=C031");
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            AtualizaPesquisa();
        }

        protected override bool campoDesabilitado(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override bool campoObrigatorio(Control campo)
        {
            throw new NotImplementedException();
        }

       
    }
}