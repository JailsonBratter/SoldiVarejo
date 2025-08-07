using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Pedidos.pages
{
    public partial class PedidoOrcamento : visualSysWeb.code.PagePadrao     //inicio da classe 
    {

        static String UltimaOrdem = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];

            if (!IsPostBack)
            {
                pesquisar();
            }
            pesquisar(pnBtn);

            if (!IsPostBack)
            {
                txtDataAte.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                txtDataAte.MaxLength = 10;
                txtDataDe.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                txtDataDe.MaxLength = 10;
            }



        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/pedidos/pages/pedidoOrcamentoDetalhes.aspx?novo=true");
        }


        protected void gridPesquisa_Sorting(object sender, GridViewSortEventArgs e)
        {

            pesquisar(e.SortExpression);

        }
        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
          pesquisar();
        }
        protected void pesquisar()
        {

            pesquisar(UltimaOrdem);

        }

        protected void pesquisar(string ordem)
        {
            DataTable tb = null;
            String sqlGrid = "select pedido," +
                                "simples= case when pedido_simples=1 then 'SIM' ELSE 'NAO' END, " +
                                "cliente_fornec,cliente.nome_cliente," +
                                "Status =  case " +
                                         "when status = 1 then 'ABERTO' " +
                                         "when status = 2 then 'FECHADO' " +
                                         "when status = 3 then 'CANCELADO' " +
                                         "when status = 4 then 'PENDENTE ENTREGA' " +
                                         "when status = 4 then 'TRANSITO' " +
                                 "END," +
                                "convert(varchar,pedido.data_Cadastro,103) Data_Cadastro," +
                                "Total " +
                            "from pedido inner join cliente on cliente.codigo_cliente = pedido.cliente_fornec " +
                            "where tipo=8";//colocar os campos no select que ser?o apresentados na tela
            String strOrdem = "";

            try
            {

                String sql = "";

                if (!txtPedido.Text.Equals("")) //colocar nome do campo de pesquisa
                {
                    sql = " and pedido like '" + txtPedido.Text + "%'";
                }

                if (!txtCliente.Text.Equals(""))
                {
                    sql += " and (cliente_fornec = '" + txtCliente.Text + "' or cliente.nome_cliente like '%" + txtCliente.Text + "%')";
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
                    else if (ordem.ToUpper().Contains("DATA_CADASTRO"))
                    {
                        strOrdem = " order by convert(varchar,pedido.Data_Cadastro,102) ";
                    }
                    else
                    {
                        strOrdem = " order By " + ordem;
                    }
                    if (ordem.Equals(UltimaOrdem))
                    {
                        strOrdem += " Desc";
                        ordem += " Desc";
                    }
                    UltimaOrdem = ordem;
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
                case "Cliente":
                    sqlLista = "select codigo_cliente, nome_cliente from cliente where codigo_cliente like '%" + TxtPesquisaLista.Text + "%' or nome_cliente like '%" + TxtPesquisaLista.Text + "%'"; ;
                    lbllista.Text = "Cliente";
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
                case "imgBtnCliente":
                    Session.Add("camporecebe", "Cliente");
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

                if (listaAtual.Equals("Cliente"))
                {

                    txtCliente.Text = ListaSelecionada(1);


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

        protected void ddlPedidoSimples_SelectedIndexChanged(object sender, EventArgs e)
        {
            pesquisar();
        }

        protected void gridPesquisa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = 0;
            if (int.TryParse(e.CommandArgument.ToString(), out index))
            {
                if (e.CommandName.Equals("EmitirPedido"))
                {
                    HyperLink mPed = (HyperLink)gridPesquisa.Rows[index].Cells[0].Controls[0];
                    String strPed = mPed.Text;
                    Response.Redirect("~/modulos/pedidos/pages/PedidoVendaDetalhes.aspx?tela=PD001&novo=true&orcamentoImporta=" + strPed);

                }
            }
        }


    }
}