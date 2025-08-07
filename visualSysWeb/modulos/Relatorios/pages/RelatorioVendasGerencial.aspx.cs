using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using visualSysWeb.dao;
using System.Data;
using System.Data.SqlClient;


namespace visualSysWeb.modulos.Relatorios.pages
{
    public partial class RelatorioVendasPorAliquota : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDe.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtAte.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        protected void RdoRelatorios_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ImgBtnVoltar_Click(object sender, EventArgs e)
        {
            divPage.Visible = true;
            divRelatorio.Visible = false;
        }
        protected void btnVisualizar_Click(object sender, EventArgs e)
        {
            if (RdoRelatorios.SelectedValue.Equals("01-Aliquota"))
            {
                vendasAliquota();
            }
            else if (RdoRelatorios.SelectedValue.Equals("02-Finalizadora"))
            {
                VendasFinalizadora();
            }

        }


        private void vendasAliquota()
        {
            SqlDataReader rs = null;
            
            try
            {
                User usr= (User) Session["user"];
                lblFiltros.Text = "De: " + txtDe.Text + " Ate: " + txtAte.Text;
                lbltituloRelatorio.Text = "RELATORIO DE VENDAS POR ALIQUOTA";
                ArrayList cabecalho = new ArrayList();
                rs = Conexao.consulta("select Saida_ICMS from Tributacao group by Saida_ICMS order by Saida_ICMS", null, false);
                while (rs.Read())
                {
                    cabecalho.Add(rs["saida_icms"].ToString().Replace(",","."));
                }
                String SqlTotal = "Select Data =convert(varchar,Data_movimento,103)";
        


                foreach (String cab in cabecalho)
                {
                    SqlTotal += ",[" + (cab.Equals("0.00") ? "INSENTO" : cab) + "] =ISNULL((select SUM(ISNULL(vlr,0)-isnull(desconto,0)) from saida_estoque with(index(ix_Rel_Venda_Aliquota)) where Filial='" + usr.getFilial() + "'  and data_movimento =se.Data_movimento and Aliquota_ICMS =" + cab + " AND data_cancelamento IS NULL),0)";

                }

                SqlTotal += " from saida_estoque se with(index(ix_Rel_Venda_Aliquota)) " +
                    " where Filial='" + usr.getFilial() + "'  and Data_movimento between '" + DateTime.Parse(txtDe.Text).ToString("yyyyMMdd") + "'  and '" + DateTime.Parse(txtAte.Text).ToString("yyyyMMdd") + "' " +
                    " group by Data_movimento  order by convert(varchar,data_movimento,102) ";

                GridRelatorio.DataSource = Conexao.GetTable(SqlTotal, null, false);
                GridRelatorio.DataBind();

                divPage.Visible = false;
                divRelatorio.Visible = true;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                    rs.Close();

                
            }


        }
        protected void Gridrelatorio_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridRelatorio.FooterRow;

            foreach (GridViewRow  item in GridRelatorio.Rows)
            {
                for (int i = 1; i < item.Cells.Count; i++)
                {
                    Decimal valor = (footer.Cells[i].Text.Replace("&nbsp;", "").Equals("") ? 0 : Decimal.Parse(footer.Cells[i].Text));
                    valor += (item.Cells[i].Text.Equals("")||item.Cells[i].Text.Equals("------") ? 0 : Decimal.Parse(item.Cells[i].Text));
                    item.Cells[i].Text = (item.Cells[i].Text.Equals("") || item.Cells[i].Text.Equals("------") ? 0 : Decimal.Parse(item.Cells[i].Text)).ToString("N2");
                    footer.Cells[i].Text = valor.ToString("N2");
                    footer.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                    item.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                }

            }



        }
        private void VendasFinalizadora()
        {
            SqlDataReader rs = null;

            try
            {
                User usr = (User)Session["user"];
                lblFiltros.Text = "De: " + txtDe.Text + " Ate: " + txtAte.Text;
                lbltituloRelatorio.Text = "RELATORIO DE VENDAS POR FINALIZADORA";
                ArrayList cabecalho = new ArrayList();
                rs = Conexao.consulta("SELECT Finalizadora FROM Finalizadora GROUP BY Finalizadora", null, false);
                while (rs.Read())
                {
                    cabecalho.Add(rs["Finalizadora"].ToString().Trim());
                }
                String SqlTotal = "Select Data =convert(varchar,Emissao,103)";



                foreach (String cab in cabecalho)
                {
                    SqlTotal += ",[" + (cab) + "] =ISNULL((select SUM(ISNULL(total,0))from Lista_finalizadora where Filial='" + usr.getFilial() + "'  and emissao =lf.emissao and id_finalizadora ='" + cab + "' AND Cancelado is null),0)";

                }

                SqlTotal += " from Lista_finalizadora lf  " +
                    " where Filial='" + usr.getFilial() + "'  and Emissao between '" + DateTime.Parse(txtDe.Text).ToString("yyyyMMdd") + "'  and '" + DateTime.Parse(txtAte.Text).ToString("yyyyMMdd") + "' " +
                    " group by Emissao  order by convert(varchar,emissao,102)";

                GridRelatorio.DataSource = Conexao.GetTable(SqlTotal, null, false);
                GridRelatorio.DataBind();

                divPage.Visible = false;
                divRelatorio.Visible = true;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                    rs.Close();


            }

        }
    }
}