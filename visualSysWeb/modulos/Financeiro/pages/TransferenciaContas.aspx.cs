using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using visualSysWeb.modulos.Financeiro.code;
using System.Data.SqlClient;
using System.Collections;

namespace visualSysWeb.modulos.Financeiro.pages
{
    public partial class TransferenciaContas : visualSysWeb.code.PagePadrao  
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TransferenciaFiltro filtro = (TransferenciaFiltro)Session["FiltroTransf"];
                if(filtro !=null)
                {
                    txtId.Text = filtro.id;
                    txtDescricao.Text = filtro.descricao;
                    txtDtDe.Text = filtro.dtDe;
                    txtDtAte.Text = filtro.dtAte;
                    txtOrigem.Text = filtro.origem;
                    txtDestino.Text = filtro.destino;
                    ddlTipo.Text = filtro.tipo;
                    ddlStatus.Text = filtro.status;
                    txtCentroDeCusto.Text = filtro.centroCusto;

                }
                pesquisa(true);
            }

            pesquisar(pnBtn);
        }

        protected void imgBtnLimpar_Click(object sender, EventArgs e)
        {
            txtId.Text = "";
            txtDescricao.Text = "";
            txtDtDe.Text = "";
            txtDtAte.Text = "";
            txtOrigem.Text = "";
            txtDestino.Text = "";
            ddlTipo.Text = "TODOS";
            ddlStatus.Text = "TODOS";
            txtCentroDeCusto.Text = "";
            pesquisa(true);
        }

        protected void pesquisa(bool limitar)
        {
            User usr = (User)Session["User"];
            if (usr == null)
                return;

            TransferenciaFiltro filtro = new TransferenciaFiltro();
            filtro.id = txtId.Text;
            filtro.descricao = txtDescricao.Text;
            filtro.dtDe = txtDtDe.Text;
            filtro.dtAte = txtDtAte.Text;
            filtro.origem = txtOrigem.Text;
            filtro.destino = txtDestino.Text;
            filtro.tipo = ddlTipo.Text;
            filtro.status = ddlStatus.Text;
            filtro.centroCusto = txtCentroDeCusto.Text;

            Session.Remove("FiltroTransf");
            Session.Add("FiltroTransf", filtro);


            String sql = "Select dataBr =convert(varchar,data,103), Centro_custo=centro_custo.descricao_centro_custo , transferencias_contas.* from transferencias_contas inner join centro_custo on transferencias_contas.codigo_centro_custo= centro_custo.codigo_centro_custo ";
            String strWhere = "";
            if (!txtId.Text.Equals(""))
            {
                strWhere = " id = " + txtId.Text;
            }
            if (!txtDescricao.Text.Trim().Equals(""))
            {
                if(strWhere.Length>0)
                {
                    strWhere += " and ";
                }
                strWhere += " descricao like '%" + txtDescricao.Text.Trim() + "%'"; 
            }

            if (!txtDtDe.Text.Equals(""))
            {
                if(txtDtAte.Equals(""))
                {
                    txtDtAte.Text = txtDtDe.Text;
                }

                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }
                DateTime dtDe =new DateTime();
                DateTime dtAte = new DateTime();

                DateTime.TryParse(txtDtDe.Text, out dtDe);
                DateTime.TryParse(txtDtAte.Text , out dtAte);

                strWhere += " ( data between '" + dtDe.ToString("yyyy-MM-dd") + "' and '" + dtAte.ToString("yyyy-MM-dd") + "')";
  
            }

            if (!txtOrigem.Text.Trim().Equals(""))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }

                strWhere += " conta_origem ='" + txtOrigem.Text + "'";
            }

            if (!txtDestino.Text.Trim().Equals(""))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }

                strWhere += " conta_destino='" + txtDestino.Text + "'";
            }

            if (!ddlStatus.Text.Equals("TODOS"))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }

                strWhere += " status ='" + ddlStatus.Text+"'";

            }

            if(!ddlTipo.Text.Equals("TODOS"))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }

                strWhere += " tipo ='"+ddlTipo.Text +"'";

            }

            if (!txtCentroDeCusto.Text.Equals(""))
            {
                if (strWhere.Length > 0)
                {
                    strWhere += " and ";
                }
                strWhere += " centro_custo.codigo_centro_custo ='" + txtCentroDeCusto.Text + "'";
            }


            if(!strWhere.Equals(""))
            {
                sql += " where "+ strWhere;
            }

           

            String totalItens = Conexao.retornaUmValor("select COUNT(*) from transferencias_contas ", usr);
            String totalItensFiltrados = Conexao.retornaUmValor("select COUNT(*) from (" + sql + ") as a", usr);
            if (limitar && int.Parse(totalItensFiltrados) > 100)
            {
                totalItensFiltrados = "100";
            }

            lblRegistros.Text = totalItensFiltrados +" de "+totalItens+ " Cadastrados";
            usr.consultaTodasFiliais = true;
            sql += " order by id desc ";
            gridPesquisa.DataSource = Conexao.GetTable(sql, usr, limitar);
            gridPesquisa.DataBind();
            usr.consultaTodasFiliais = false;

            if (!ddlTipo.Text.Equals("TODOS"))
            {
                Decimal vltTotal = 0;
                foreach (GridViewRow linha in gridPesquisa.Rows)
                {
                    Decimal vlrItem = 0;
                    HyperLink linkTotl = (HyperLink)linha.Cells[7].Controls[0];
                    Decimal.TryParse(linkTotl.Text.Replace("R$","").Trim(), out vlrItem);
                    vltTotal += vlrItem;
                }

                divTotal.Visible = true;
                lblTotalValores.Text = vltTotal.ToString("N2");
            }
            else
            {
                divTotal.Visible = false;
            }

        }


        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransferenciaContasDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisa(false);

            HyperLink meuLink = (HyperLink)gridPesquisa.Rows[0].Cells[0].Controls[0];
            if (gridPesquisa.Rows.Count == 1 && !meuLink.Text.Equals("------"))
            {
                Response.Redirect("TransferenciaContasDetalhes.aspx?id=" + meuLink.Text);
            }
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
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


        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe"];
            String sqlLista = "";


            switch (or)
            {
                case "Destino":
                case "Origem":
                    sqlLista = "Select id_cc," +
                                       " Agencia= Agencia +' '+ dig_agencia , " +
                                       " conta= conta +' '+ dig_conta  " +
                               " from conta_corrente" +
                               " where id_cc like '%" + TxtPesquisaLista.Text + "%' or Agencia like '%" + TxtPesquisaLista.Text + "%' or conta like '%" + TxtPesquisaLista.Text + "'";
                    lbllista.Text = "Fornecedor";
                    break;
                case "CentroCusto":
                    usr.consultaTodasFiliais = true;
                    sqlLista = "select Codigo = Codigo_centro_custo,Grupo=descricao_grupo, SubGrupo= Descricao_subgrupo, [Centro custo] = Descricao_centro_custo " +
                               " from centro_custo " +
                                   " inner join subgrupo_cc on subgrupo_cc.codigo_subgrupo = centro_custo.codigo_subgrupo " +
                                   " inner join grupo_cc on grupo_cc.codigo_grupo = subgrupo_cc.codigo_grupo " +

                               " where ( codigo_centro_custo like '%" + TxtPesquisaLista.Text + "%' or Descricao_centro_custo like '" + TxtPesquisaLista.Text + "%' or Descricao_subgrupo like '%" + TxtPesquisaLista.Text + "%' or descricao_grupo like '%" + TxtPesquisaLista.Text + "')";
                    
                    lbllista.Text = "Centro de custo ";

                    break;
                

            }
            GridLista.DataSource = Conexao.GetTable(sqlLista, usr, true);
            GridLista.DataBind();
            usr.consultaTodasFiliais = false;
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
                case "imgBtnOrigem":
                    Session.Add("camporecebe", "Origem");
                    break;
                case "imgBtnDestino":
                    Session.Add("camporecebe", "Destino");
                    break;
                case "imgBtnCentroDeCusto":
                    Session.Add("camporecebe", "CentroCusto");
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

                if (listaAtual.Equals("Origem"))
                {

                    txtOrigem.Text = ListaSelecionada(1);


                }
                if (listaAtual.Equals("Destino"))
                {
                    txtDestino.Text = ListaSelecionada(1);
                }
                if (listaAtual.Equals("CentroCusto"))
                {
                    txtCentroDeCusto.Text = ListaSelecionada(1);
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
            modalPnFundo.Hide();
        }
        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }

        protected void txtDtDe_Change(object sender, EventArgs e)
        {
            txtDtAte.Text = txtDtDe.Text;

        }

    }
}