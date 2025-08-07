using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using visualSysWeb.modulos.Manutencao.dao;

namespace visualSysWeb.modulos.Manutencao.pages
{
    public partial class ConfiguracaoDepartamentosCE : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            status = "visualizar";
            carregabtn(pnBtn,null,"NAO","NAO","NAO","NAO","NAO");
            if(!IsPostBack)
            {
                atualizarDados();
            }
        }

        private void atualizarDados()
        {
            String sql = "Select Grupo_grafico, Descricao, Codigo_departamento,Dep_Ativa_CE=Case when isnull(Dep_Ativa_CE,0) =1 then 'SIM' ELSE 'NAO' END " +
                 " FROM Soldi_Gusto_CE_Departamento ";

            gridPesquisa.DataSource = Conexao.GetTable(sql, null, false);
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
            Session.Remove("objDep" + urlSessao());
            LimparCampos(pnDetalhesGrupo);
            modalDetalhesDepartamento.Show();
            txtGrupoGrafico.Focus();
            divExcluir.Visible = false;
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override bool campoDesabilitado(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override bool campoObrigatorio(Control campo)
        {
            throw new NotImplementedException();
        }

        private Soldi_Gusto_CE_DepartamentoDAO objDetalhes()
        {
            Soldi_Gusto_CE_DepartamentoDAO obj = (Soldi_Gusto_CE_DepartamentoDAO)Session["objDep"+urlSessao()];
            if(obj == null)
                obj  = new Soldi_Gusto_CE_DepartamentoDAO();


            obj.grupo_grafico = txtGrupoGrafico.Text;
            obj.descricao = txtDescricao.Text;
            obj.codigo_departamento = txtCodDepartamento.Text;
            obj.Dep_ativa_CE = chkDepAtivaCE.Checked;

            Session.Remove("objDep" + urlSessao());
            Session.Add("objDep" + urlSessao(), obj);

            return obj;
        }
        protected void btnConfirmaDepartamentoCE_Click(object sender, EventArgs e)
        {

            try
            {

                Soldi_Gusto_CE_DepartamentoDAO obj = objDetalhes();
                obj.salvar();
                atualizarDados();
                modalDetalhesDepartamento.Hide();
            }
            catch (Exception err)
            {
                showMessagem(err.Message, true);
            }
        }
        private void showMessagem(String msg , bool erro)
        {
            lblErroPanel.Text = msg;
            if (erro)
                lblErroPanel.ForeColor = System.Drawing.Color.Red;
            else
                lblErroPanel.ForeColor = System.Drawing.Color.Blue;
            modalError.Show();
        }

        protected void btnCancelaDep_Click(object sender, EventArgs e)
        {
            modalDetalhesDepartamento.Hide();
        }
        protected void imgBtnExcluir_Click(object sender, EventArgs e)
        {
            try
            {

                Soldi_Gusto_CE_DepartamentoDAO obj = objDetalhes();
                obj.exclui();
                atualizarDados();
                modalDetalhesDepartamento.Hide();
            }
            catch (Exception err)
            {
                showMessagem(err.Message, true);
            }
        }

        protected void gridPesquisa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Session.Remove("objDep" + urlSessao());
            int index = Convert.ToInt32(e.CommandArgument);

            GridViewRow row =  gridPesquisa.Rows[index];

            txtGrupoGrafico.Text = row.Cells[1].Text;
            txtDescricao.Text = row.Cells[2].Text;
            txtCodDepartamento.Text = row.Cells[3].Text;
            chkDepAtivaCE.Checked = row.Cells[4].Text.Equals("SIM");
            divExcluir.Visible = true;
            objDetalhes();
            modalDetalhesDepartamento.Show();


        }

        protected void btnOkError_Click(object sender, EventArgs e)
        {
            modalError.Hide();
        }

        private void exibeLista(String tipo)
        {
            String sql = "";
            switch (tipo)
            {
                case "GrupoGrafico":
                    lbllista.Text = "ESCOLHA O GRUPO GRAFICO";
                    sql = "Select[Grupo Grafico]= Grupo_grafico" +
                            " FROM Soldi_Gusto_CE_Departamento" +
                            " where Grupo_grafico like '%"+TxtPesquisaLista.Text+"%'"+
                            " GROUP BY Grupo_grafico";
                    break;
                case "Departamento":
                    lbllista.Text = "ESCOLHA O DEPARTAMENTO";
                    sql = " Select Cod= Codigo_Departamento,Grupo= Descricao_grupo, SubGrupo= Descricao_SubGrupo , Departamento= Descricao_Departamento "+
                          "  from Departamento as d "+
                          "          inner join subgrupo as sg on d.codigo_subgrupo = sg.codigo_subgrupo "+
                          "          inner join grupo as g on sg.Codigo_Grupo = g.Codigo_Grupo" +
                        " where d.Codigo_departamento like '" + TxtPesquisaLista.Text + "%'" +
                        " or Descricao_Departamento like '%" + TxtPesquisaLista.Text +"%'" +
                        " or Descricao_grupo like '%"+TxtPesquisaLista.Text+"%'" +
                        " or Descricao_SubGrupo like '%" + TxtPesquisaLista.Text + "%'";
                    break;
            }

            GridLista.DataSource = Conexao.GetTable(sql, null,false);
            GridLista.DataBind();
            modalPnFundo.Show();
            Session.Remove("lista" + urlSessao());
            Session.Add("lista" + urlSessao(), tipo);
        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
            String tipo = (String)Session["lista" + urlSessao()];
            switch (tipo)
            {
                case "GrupoGrafico":
                    txtGrupoGrafico.Text = ListaSelecionada(1);
                    break;
                case "Departamento":
                    txtCodDepartamento.Text = ListaSelecionada(1);
                    break;
            }
            modalPnFundo.Hide();
            modalDetalhesDepartamento.Show();
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

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
            modalDetalhesDepartamento.Show();
        }

        protected void imgBtnGrupoGrafico_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("GrupoGrafico");
        }

        protected void imgBtnDepartamento_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista("Departamento");
        }

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            String tipo = (String)Session["lista" + urlSessao()];
            exibeLista(tipo);
        }
        protected void GridLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoListaItem");

            if (rdo == null)
            {
                return;//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            }
            string script = "SetUniqueRadioButton('GridLista.*GrlistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }
    }
}