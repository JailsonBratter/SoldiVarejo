using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class DepartamentosManutencao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                pesquisa();
        }

        private void pesquisa()
        {
            String sql = " Select mercadoria.plu " +
                            ", ean=(Select top 1 ean from ean where plu = mercadoria.plu)" +
                            ",ref=ref_fornecedor" +
                            ",mercadoria.descricao" +
                            ",preco = mercadoria_loja.preco " +
                        " from mercadoria " +
                            " inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu " +
                        " Where isnull(mercadoria.inativo,0) =0 ";
            String where1 = "";
            String where2 = "";
            //if(txtCodGrupo1.Text.Trim().Length>0)
            //    where1 += " and  codigo_departamento like  '" + txtCodGrupo1.Text.Trim().PadLeft(3, '0') + "%'";

            //if (txtCodSubGrupo1.Text.Trim().Length>0)
            //    where1 += " and codigo_departamento like '" + txtCodSubGrupo1.Text.Trim() + "%'";

            if (txtCodDepartamento1.Text.Trim().Length > 0)
                where1 += " and codigo_departamento = '" + txtCodDepartamento1.Text + "'";


            if (txtDescricao1.Text.Trim().Length > 0)
                where1 += " and mercadoria.descricao like '%" + txtDescricao1.Text + "%'";

            //if(txtCodGrupo2.Text.Trim().Length>0)
            //    where2 += " and  codigo_departamento like  '" + txtCodGrupo2.Text.Trim().PadLeft(3, '0') + "%'";

            //if (txtCodSubGrupo2.Text.Trim().Length>0)
            //    where2 += " and codigo_departamento like '" + txtCodSubGrupo2.Text.Trim() + "%'";

            if (txtCodDepartamento2.Text.Trim().Length > 0)
                where2 += " and codigo_departamento = '" + txtCodDepartamento2.Text + "'";

            if (txtDescricao2.Text.Trim().Length > 0)
                where2 += " and mercadoria.descricao like '%" + txtDescricao2.Text + "%'";

            //quando nao tiver nenhum filtro fazer a consulta em branco;
            if (where1.Length == 0)
                where1 = " and 1=2";

            if (where2.Length == 0)
                where2 = " and 1=2";

            gridProdutos1.DataSource = Conexao.GetTable(sql + where1, null, false);
            gridProdutos1.DataBind();

            gridProdutos2.DataSource = Conexao.GetTable(sql + where2, null, false);
            gridProdutos2.DataBind();
        }
        protected void chkSeleciona1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTodos = (CheckBox)sender;

            foreach (GridViewRow item in gridProdutos1.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    chk.Checked = chkTodos.Checked;
                    //incluirMercadoria(chk);
                }
            }


        }
        protected void chkSeleciona2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTodos = (CheckBox)sender;

            foreach (GridViewRow item in gridProdutos2.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");

                if (chk != null)
                {
                    chk.Checked = chkTodos.Checked;
                    //incluirMercadoria(chk);
                }
            }


        }

        protected void imgLista_Click(object sender, EventArgs e)
        {
            String idNome = ((ImageButton)sender).ID;

            Session.Remove("listaManu");
            Session.Add("listaManu", idNome);
            TxtPesquisaLista.Text = "";
            exibirLista();

        }

        protected void exibirLista()
        {


            lblErroPesquisa.Text = "";
            String idNome = (String)Session["listaManu"];
            String sql = "";
            switch (idNome)
            {
                case "imgBtnGrupo1":
                case "imgBtnGrupo2":
                    lbltituloLista.Text = "Escolha o Grupo";
                    sql = "Select Codigo =codigo_grupo, Descricao= descricao_grupo " +
                        "from grupo " +
                        "where codigo_grupo like '" + TxtPesquisaLista.Text + "%' or descricao_grupo like '%" + TxtPesquisaLista.Text + "%'";

                    break;
                case "imgBtnSubGrupo1":
                case "imgBtnSubGrupo2":
                    lbltituloLista.Text = "Escolha o SubGrupo";
                    sql = "Select Codigo =codigo_subgrupo, Descricao=descricao_subgrupo, Grupo = descricao_grupo " +
                        "from subgrupo inner join grupo on subgrupo.codigo_grupo = grupo.codigo_grupo " +
                        "where (codigo_subgrupo like '" + TxtPesquisaLista.Text + "%' " +
                        "or descricao_subgrupo like '%" + TxtPesquisaLista.Text + "%')";

                    if (idNome.Contains("1") && txtCodGrupo1.Text.Trim().Length > 0)
                    {
                        sql += " and  subgrupo.codigo_grupo =" + txtCodGrupo1.Text;
                    }
                    else if (idNome.Contains("2") && txtCodGrupo2.Text.Trim().Length > 0)
                    {
                        sql += " and  subgrupo.codigo_grupo =" + txtCodGrupo2.Text;
                    }

                    break;
                case "imgBtnDepartamento1":
                case "imgBtnDepartamento2":
                    lbltituloLista.Text = "Escolha o Departamento";
                    sql = "Select Codigo =codigo_departamento" +
                                ", Descricao= Descricao_departamento" +
                                ", SubGrupo=subGrupo.descricao_subgrupo " +
                                ", Grupo = grupo.descricao_grupo" +
                          " from departamento inner join subgrupo on departamento.codigo_subgrupo = subgrupo.codigo_subgrupo" +
                                            " inner join grupo on subgrupo.codigo_grupo = grupo.codigo_grupo" +
                          " where (codigo_departamento like '" + TxtPesquisaLista.Text + "%' or descricao_departamento like '%" + TxtPesquisaLista.Text + "%')";

                    if (idNome.Contains("1"))
                    {
                        if (txtCodGrupo1.Text.Trim().Length > 0)
                            sql += " and  subgrupo.codigo_grupo =" + txtCodGrupo1.Text;
                        if (txtCodSubGrupo1.Text.Trim().Length > 0)
                            sql += "  and departamento.codigo_subgrupo = '" + txtCodSubGrupo1.Text + "'";
                    }
                    else if (idNome.Contains("2"))

                    {
                        if (txtCodGrupo2.Text.Trim().Length > 0)
                            sql += " and  subgrupo.codigo_grupo =" + txtCodGrupo2.Text;
                        if (txtCodSubGrupo2.Text.Trim().Length > 0)
                            sql += "  and departamento.codigo_subgrupo = '" + txtCodSubGrupo2.Text + "'";
                    }

                    break;

            }

            GridLista.DataSource = Conexao.GetTable(sql, null, false);
            GridLista.DataBind();
            modalLista.Show();
        }

        protected void btnConfirmaLista_Click(object sender, EventArgs e)
        {
            String idNome = (String)Session["listaManu"];

            if (ListaSelecionada(1).Equals(""))
            {

                lblErroPesquisa.ForeColor = System.Drawing.Color.Red;
                lblErroPesquisa.Text = "Selecione uma Opção";
                modalLista.Show();
            }
            else
            {


                switch (idNome)
                {
                    case "imgBtnGrupo1":
                        txtCodGrupo1.Text = ListaSelecionada(1);
                        txtDescricaoGrupo1.Text = ListaSelecionada(2);
                        txtCodSubGrupo1.Text = "";
                        txtDescricaoSubGrupo1.Text = "";
                        txtCodDepartamento1.Text = "";
                        txtDescricaoDepartamento1.Text = "";
                        break;
                    case "imgBtnSubGrupo1":
                        txtCodGrupo1.Text = ListaSelecionada(1).Substring(0, 3);
                        txtDescricaoGrupo1.Text = ListaSelecionada(3);
                        txtCodSubGrupo1.Text = ListaSelecionada(1);
                        txtDescricaoSubGrupo1.Text = ListaSelecionada(2);
                        txtCodDepartamento1.Text = "";
                        txtDescricaoDepartamento1.Text = "";
                        break;
                    case "imgBtnDepartamento1":
                        txtCodGrupo1.Text = ListaSelecionada(1).Substring(0, 3);
                        txtDescricaoGrupo1.Text = ListaSelecionada(4);
                        txtCodSubGrupo1.Text = ListaSelecionada(1).Substring(0, 6); ;
                        txtDescricaoSubGrupo1.Text = ListaSelecionada(3);
                        txtCodDepartamento1.Text = ListaSelecionada(1);
                        txtDescricaoDepartamento1.Text = ListaSelecionada(2);
                        break;
                    case "imgBtnGrupo2":
                        txtCodGrupo2.Text = ListaSelecionada(1);
                        txtDescricaoGrupo2.Text = ListaSelecionada(2);
                        txtCodSubGrupo2.Text = "";
                        txtDescricaoSubGrupo2.Text = "";
                        txtCodDepartamento2.Text = "";
                        txtDescricaoDepartamento2.Text = "";
                        break;
                    case "imgBtnSubGrupo2":
                        txtCodGrupo2.Text = ListaSelecionada(1).Substring(0, 3);
                        txtDescricaoGrupo2.Text = ListaSelecionada(3);
                        txtCodSubGrupo2.Text = ListaSelecionada(1);
                        txtDescricaoSubGrupo2.Text = ListaSelecionada(2);
                        txtCodDepartamento2.Text = "";
                        txtDescricaoDepartamento2.Text = "";
                        break;
                    case "imgBtnDepartamento2":
                        txtCodGrupo2.Text = ListaSelecionada(1).Substring(0, 3);
                        txtDescricaoGrupo2.Text = ListaSelecionada(4);
                        txtCodSubGrupo2.Text = ListaSelecionada(1).Substring(0, 6); ;
                        txtDescricaoSubGrupo2.Text = ListaSelecionada(3);
                        txtCodDepartamento2.Text = ListaSelecionada(1);
                        txtDescricaoDepartamento2.Text = ListaSelecionada(2);
                        break;

                }

                modalLista.Hide();
                pesquisa();
            }

        }
        protected void btnCancelaLista_Click(object sender, EventArgs e)
        {
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
        protected void ImgPesquisaLista_Click(object sender, EventArgs e)
        {
            exibirLista();
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

        protected void btnEsquerda_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCodDepartamento1.Text.Trim().Length == 0)
                    throw new Exception("Departamento de Origem não Selecionado");

                StringBuilder sql = new StringBuilder();

                foreach (GridViewRow item in gridProdutos2.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                    if (chk.Checked)
                    {
                        sql.AppendLine("update mercadoria set " +
                                    " codigo_departamento = '" + txtCodDepartamento1.Text + "' " +
                                    "where plu ='" + item.Cells[1].Text + "';");
                    }
                }
                if (sql.Length == 0)
                    throw new Exception("Escolha os itens a serem transferidos!");

                Conexao.executarSql(sql.ToString());
                pesquisa();
            }
            catch (Exception err)
            {

                showMessagem(err.Message, true);
            }


        }

        protected void btnDireita_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCodDepartamento2.Text.Trim().Length == 0)
                    throw new Exception("Departamento de Origem não Selecionado");
                StringBuilder sql = new StringBuilder();

                foreach (GridViewRow item in gridProdutos1.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                    if (chk.Checked)
                    {
                        sql.AppendLine("update mercadoria set " +
                                    " codigo_departamento = '" + txtCodDepartamento2.Text + "' " +
                                    "where plu ='" + item.Cells[1].Text + "';");
                    }
                }
                if (sql.Length == 0)
                    throw new Exception("Escolha os itens a serem transferidos!");

                Conexao.executarSql(sql.ToString());
                pesquisa();
            }
            catch (Exception err)
            {

                showMessagem(err.Message, true);
            }

        }

        private void showMessagem(String msg, bool erro)
        {
            lblErroPanel.Text = msg;
            if (erro)
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Blue;
            }
            btnOkError.Focus();
            modalError.Show();

        }
        protected void btnOkError_Click(object sender, EventArgs e)
        {
            modalError.Hide();
            //pesquisa();
        }

        protected void txtDescricao2_TextChanged(object sender, EventArgs e)
        {
            pesquisa();
        }

        protected void txtDescricao1_TextChanged(object sender, EventArgs e)
        {
            pesquisa();
        }


        protected void txt_TextChanged(object sender, EventArgs e)
        {



            TextBox txt = (TextBox)sender;
            String vChangeAtual = txt.ID + txt.Text;
            String txtChange = (String)Session["txtChange"];
            if (!txt.Text.Trim().Equals(""))
            {
                if (txtChange != null && vChangeAtual.Equals(txtChange))
                    return;
            }
            Session.Remove("txtChange");
            Session.Add("txtChange", vChangeAtual);

            if (txt.Text.Trim().Equals(""))
            {
                switch (txt.ID)
                {
                    case "txtCodGrupo1":
                        txtDescricaoGrupo1.Text = "";
                        txtCodSubGrupo1.Text = "";
                        txtDescricaoSubGrupo1.Text = "";
                        txtCodDepartamento1.Text = "";
                        txtDescricaoDepartamento1.Text = "";
                        break;
                    case "txtCodSubGrupo1":

                        txtDescricaoSubGrupo1.Text = "";
                        txtCodDepartamento1.Text = "";
                        txtDescricaoDepartamento1.Text = "";
                        break;
                    case "txtCodDepartamento1":
                        txtDescricaoDepartamento1.Text = "";
                        break;
                    case "txtCodGrupo2":
                        txtDescricaoGrupo2.Text = "";
                        txtCodSubGrupo2.Text = "";
                        txtDescricaoSubGrupo2.Text = "";
                        txtCodDepartamento2.Text = "";
                        txtDescricaoDepartamento2.Text = "";
                        break;
                    case "txtCodSubGrupo2":
                        txtDescricaoSubGrupo2.Text = "";
                        txtCodDepartamento2.Text = "";
                        txtDescricaoDepartamento1.Text = "";
                        break;
                    case "txtCodDepartamento2":
                        txtDescricaoDepartamento2.Text = "";

                        break;

                }
            }
            else
            {
                switch (txt.ID)
                {
                    case "txtCodGrupo1":
                        txtDescricaoGrupo1.Text = Conexao.retornaUmValor("Select Descricao_grupo from grupo where codigo_grupo=" + txt.Text, null);
                        txtCodSubGrupo1.Text = "";
                        txtDescricaoSubGrupo1.Text = "";
                        txtCodDepartamento1.Text = "";
                        txtDescricaoDepartamento1.Text = "";
                        txtCodSubGrupo1.Focus();
                        break;
                    case "txtCodSubGrupo1":
                        txtCodGrupo1.Text = txt.Text.Substring(0, 3);
                        txtDescricaoGrupo1.Text = Conexao.retornaUmValor("Select Descricao_grupo from grupo where codigo_grupo=" + txt.Text.Substring(0, 3), null);
                        txtDescricaoSubGrupo1.Text = Conexao.retornaUmValor("Select Descricao_subgrupo from subgrupo where codigo_subgrupo='" + txt.Text + "'", null);
                        txtCodDepartamento1.Text = "";
                        txtDescricaoDepartamento1.Text = "";
                        txtCodDepartamento1.Focus();
                        break;
                    case "txtCodDepartamento1":
                        txtCodGrupo1.Text = txt.Text.Substring(0, 3);
                        txtDescricaoGrupo1.Text = Conexao.retornaUmValor("Select Descricao_grupo from grupo where codigo_grupo=" + txt.Text.Substring(0, 3), null);
                        txtCodSubGrupo1.Text = txt.Text.Substring(0, 6);
                        txtDescricaoSubGrupo1.Text = Conexao.retornaUmValor("Select Descricao_subgrupo from subgrupo where codigo_subgrupo='" + txt.Text.Substring(0, 6) + "'", null);
                        txtDescricaoDepartamento1.Text = Conexao.retornaUmValor("Select Descricao_departamento from departamento where codigo_departamento='" + txt.Text + "'", null);
                        txtDescricao1.Focus();
                        break;

                    case "txtCodGrupo2":
                        txtDescricaoGrupo2.Text = Conexao.retornaUmValor("Select Descricao_grupo from grupo where codigo_grupo=" + txt.Text, null);
                        txtCodSubGrupo2.Text = "";
                        txtDescricaoSubGrupo2.Text = "";
                        txtCodDepartamento2.Text = "";
                        txtDescricaoDepartamento2.Text = "";
                        txtCodSubGrupo2.Focus();
                        break;
                    case "txtCodSubGrupo2":
                        txtCodGrupo2.Text = txt.Text.Substring(0, 3);
                        txtDescricaoGrupo2.Text = Conexao.retornaUmValor("Select Descricao_grupo from grupo where codigo_grupo=" + txt.Text.Substring(0, 3), null);
                        txtDescricaoSubGrupo2.Text = Conexao.retornaUmValor("Select Descricao_subgrupo from subgrupo where codigo_subgrupo='" + txt.Text + "'", null);
                        txtCodDepartamento2.Text = "";
                        txtDescricaoDepartamento2.Text = "";
                        txtCodDepartamento2.Focus();
                        break;
                    case "txtCodDepartamento2":
                        txtCodGrupo2.Text = txt.Text.Substring(0, 3);
                        txtDescricaoGrupo2.Text = Conexao.retornaUmValor("Select Descricao_grupo from grupo where codigo_grupo=" + txt.Text.Substring(0, 3), null);
                        txtCodSubGrupo2.Text = txt.Text.Substring(0, 6);
                        txtDescricaoSubGrupo2.Text = Conexao.retornaUmValor("Select Descricao_subgrupo from subgrupo where codigo_subgrupo='" + txt.Text.Substring(0, 6) + "'", null);
                        txtDescricaoDepartamento2.Text = Conexao.retornaUmValor("Select Descricao_departamento from departamento where codigo_departamento='" + txt.Text + "'", null);
                        txtDescricao2.Focus();
                        break;

                }

            }
            pesquisa();
        }
    }
}