using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class Comandas : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        static DataTable tb;
        static String sqlGrid = " Select comanda," +
                                 "      Status = case when status=0 then 'LIVRE'ELSE CASE WHEN STATUS=2 THEN 'ABERTA' ELSE  'BLOQUEADA' END END, " +
                                 "      Qtd_itens =isnull((SELECT COUNT(COMANDA)FROM Comanda_item WITH (INDEX(index_comanda_cupom)) WHERE COMANDA_ITEM.comanda = Comanda_controle.comanda AND COMANDA_ITEM.CUPOM=0 and comanda_item.data_cancelamento is null),0) ," +
                                 "      Valor = ISNULL((SELECT SUM(Comanda_item.total)FROM Comanda_item WITH (INDEX(index_comanda_cupom)) WHERE COMANDA_ITEM.comanda = Comanda_controle.comanda AND COMANDA_ITEM.CUPOM=0 and comanda_item.data_cancelamento is null),0)    " +
                                 "   from Comanda_Controle WITH (INDEX(index_comanda_status)) ";//colocar os campos no select que ser?o apresentados na tela
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pesquisa();
            }
            pesquisar(pnBtn);
            camposnumericos();
        }


        private void camposnumericos()
        {
            txtPESQ1.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/Cadastro/pages/ComandasDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }

        private void pesquisa()
        {
            String sql = "";
            String strSqltotalFiltro = "select count(*) from Comanda_Controle WITH (INDEX(index_comanda_status)) ";
            String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

            if (!txtPESQ1.Text.Equals("")) //colocar nome do campo de pesquisa
            {
                sql = " Comanda_Controle.Comanda = '" + txtPESQ1.Text + "'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
            }
            if (!ddlStatus.SelectedValue.Equals(""))
            {
                if (!sql.Equals(""))
                    sql += " and ";

                sql += " Comanda_Controle.status=" + ddlStatus.SelectedValue;
            }

            try
            {
                User usr = (User)Session["User"];
                if (!sql.Equals(""))
                {
                    tb = Conexao.GetTable(sqlGrid + " where " + sql, null, false);
                    strSqltotalFiltro += " where " + sql;
                }
                else//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                {
                    tb = Conexao.GetTable(sqlGrid, null, false);
                  
                }
                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();
                lblPesquisaErro.Text = "";

                String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
                lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";
            }
            catch (Exception err)
            {
                lblPesquisaErro.Text = err.Message;
            }
        }
        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisa();
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