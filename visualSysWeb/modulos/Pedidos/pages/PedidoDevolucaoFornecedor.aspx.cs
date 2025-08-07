using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Data;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Pedidos.pages
{
    public partial class PedidoDevolucaoFornecedor : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        static DataTable tb;
        static String sqlGrid = "select  pedido," +
                                "        cliente_fornec," +
                                "        Fornecedor.Razao_social," +
                                "        Status = case when status=1 then 'ABERTO'" +
                                "					  when status=2 then 'FECHADO'" +
                                "					  when status=3 then 'CANCELADO'" +
                                "					  when status=4 then 'PENDENTE ENTREGA'" +
                                "					  when status=4 then 'TRANSITO' end ," +
                                "		convert(varchar,pedido.data_Cadastro,103) Data_Cadastro," +
                                "		Total " +
                                " from pedido inner join Fornecedor " +
                                "			on Fornecedor.Fornecedor = pedido.cliente_fornec " +
                                "	 inner join Natureza_operacao np on pedido.cfop =  np.Codigo_operacao " +
                                "where tipo=4";
        static String UltimaOrdem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];

            if (!IsPostBack)
            {
                if (usr != null && usr.adm("PD002"))
                {
                    tb = Conexao.GetTable(sqlGrid + " order by CONVERT(int ,pedido.pedido) desc ", usr, true);
                    gridPesquisa.DataSource = tb;
                    gridPesquisa.DataBind();

                }
                else
                {
                    if (Funcoes.valorParametro("PEDIDOS_NAOLISTAR_GRID", usr) != "TRUE")
                    {
                        tb = Conexao.GetTable(sqlGrid + " order by CONVERT(VARCHAR ,pedido.data_cadastro,102) desc ", usr, true);
                        gridPesquisa.DataSource = tb;
                        gridPesquisa.DataBind();
                    }
                }
            }
            pesquisar(pnBtn);

            //String pedidoSimple = Funcoes.valorParametro("PEDIDO_SIMPLES", usr).ToUpper();
            //if (pedidoSimple.Equals("TRUE"))
            //{
            //    //pnSimples.Visible = true;
            //    //gridPesquisa.Columns[1].Visible = true;
            //}
            //else
            //{
            //    //pnSimples.Visible = false;
            //    gridPesquisa.Columns[1].Visible = false;
            //}
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/pedidos/pages/pedidoDevolucaoFornecedorDetalhes.aspx?novo=true");
        }


        protected void gridPesquisa_Sorting(object sender, GridViewSortEventArgs e)
        {

            pesquisar(e.SortExpression);

        }
        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (usr.adm("PD002"))
            {
                pesquisar();
            }
            else if (Funcoes.valorParametro("PEDIDOS_NAOLISTAR_GRID", usr) != "TRUE")
            {
                pesquisar();
            }

        }
        protected void pesquisar()
        {

            pesquisar(UltimaOrdem);

        }

        protected void pesquisar(string ordem)
        {
            String strOrdem = "";



            try
            {

                String sql = "";

                if (!txtPedido.Text.Equals("")) //colocar nome do campo de pesquisa
                {

                    sql = " and pedido like '" + txtPedido.Text + "%'";

                }

                if (!txtFornecedor.Text.Equals(""))
                {
                    sql += " and (cliente_fornec = '" + txtFornecedor.Text + "' or fornecedor.Razao_social like '%" + txtFornecedor.Text + "%' or fornecedor.nome_fantasia like '%" + txtFornecedor.Text + "%')";
                }

                if (!txtDataDe.Text.Equals(""))
                {



                    if (IsDate(txtDataDe.Text) && IsDate(txtDataAte.Text))
                    {

                        sql += " and  pedido.Data_Cadastro >= '" + DateTime.Parse(txtDataDe.Text).ToString("yyyy-MM-dd") + "' and pedido.Data_Cadastro <='" + DateTime.Parse(txtDataAte.Text).ToString("yyyy-MM-dd") + "'";

                    }

                    else
                    {

                        throw new Exception("Datas Invalidas");

                    }

                }



                //if (!ddlPedidoSimples.SelectedValue.Equals(""))
                //{

                //    sql += " and isnull(pedido_simples,0)=" + ddlPedidoSimples.SelectedValue;

                //}

                if (!ddlStatus.SelectedValue.Equals("0"))
                {
                    sql += " and isnull(status,0)=" + ddlStatus.SelectedValue;
                }


                User usr = (User)Session["User"];

                if (!ordem.Equals(""))
                {

                    if (ordem.Equals("Pedido"))
                    {

                        strOrdem = " order by convert(int,pedido.pedido) ";

                    }

                    else if (ordem.Equals("Data_Cadastro"))
                    {

                        strOrdem = " order by convert(varchar,pedido.Data_Cadastro,102) ";

                    }

                    else
                    {

                        strOrdem = " order By " + ordem;

                    }



                    //Checa se foi escolhido a mesma ordem, assim, o sistema apenas inverte a ordem da coluna

                    if (ordem.Equals(UltimaOrdem))
                    {

                        strOrdem += " Desc";

                    }

                    else
                    {

                        UltimaOrdem = ordem;

                    }



                    tb = Conexao.GetTable(sqlGrid + sql + strOrdem, usr, true);

                }

                else
                {

                    if (!sql.Equals(""))
                    {

                        tb = Conexao.GetTable(sqlGrid + sql + " order by pedido.pedido desc", usr, false);

                    }

                    else
                    {

                        tb = Conexao.GetTable(sqlGrid + " order by pedido.pedido desc ", usr, true);

                    }

                }

                gridPesquisa.DataSource = tb;

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

        protected void txtDataDe_TextChanged(object sender, EventArgs e)
        {
            if (IsDate(txtDataDe.Text))
            {
                txtDataAte.Text = txtDataDe.Text;
            }
        }

        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            User usr = (User)Session["User"];
            String or = (String)Session["camporecebe"];
            String sqlLista = "";


            switch (or)
            {
                case "Fornecedor":
                    sqlLista = "Select Fornecedor,Razao_social,Nome_Fantasia from Fornecedor where Fornecedor like '%" + TxtPesquisaLista.Text + "%' or Razao_social like '%" + TxtPesquisaLista.Text + "%' or nome_fantasia like '%"+TxtPesquisaLista.Text+"%'" ;
                    lbllista.Text = "Fornecedor";
                    break;

                //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            }
            GridLista.DataSource = Conexao.GetTable(sqlLista, usr, true);
            GridLista.DataBind();
            if (GridLista.Rows.Count == 1)
            {
                if (!GridLista.Rows[0].Cells[1].Text.Equals("------"))
                {
                    RadioButton rdo = (RadioButton)GridLista.Rows[0].FindControl("RdoListaItem");
                    rdo.Checked = true;
                }
            }


            modalPnFundo.Show();
            TxtPesquisaLista.Focus();
        }

        protected void Img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            TxtPesquisaLista.Text = "";
            switch (btn.ID)
            {
                case "imgBtnFornecedor":
                    Session.Add("camporecebe", "Fornecedor");
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
                        return item.Cells[campo].Text.Trim();
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

                if (listaAtual.Equals("Fornecedor"))
                {

                    txtFornecedor.Text = ListaSelecionada(1);


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

        protected void ddlPedidoSimples_SelectedIndexChanged(object sender, EventArgs e)
        {
            pesquisar();
        }

    }
}