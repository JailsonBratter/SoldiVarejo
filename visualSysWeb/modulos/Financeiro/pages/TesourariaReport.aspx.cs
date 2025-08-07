using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Financeiro.pages
{
    public partial class TesourariaReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                carregarRelatorio();
            }
        }

        private void carregarRelatorio()
        {
            User usr = (User) Session["User"];
            if (usr == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            DateTime dtDe  = new DateTime();
            DateTime dtAte = new DateTime();
            String operador ="";
            String statusPdv = "";

            if(Request["dtDe"] !=null)
            {
              DateTime.TryParse( Request["dtDe"].ToString(), out dtDe);
            }

            if(Request["dtAte"] !=null)
            {
              DateTime.TryParse(Request["dtAte"].ToString(), out dtAte);
            }

            if (Request["operador"] != null)
            {
                operador = Request["operador"].ToString();
            }

            if (Request["statusPdv"] != null)
            {
                statusPdv = Request["statusPdv"].ToString();
            }

            lblfiltros.Text = "Data: " + dtDe.ToString("dd/MM/yyyy") +
                               " ate " + dtAte.ToString("dd/MM/yyyy") +
                               " Operador: " + operador +
                               " Status: " + statusPdv;


            String sql = "Exec sp_rel_Tesouraria " +
                                            " @Filial='" + usr.getFilial() + "'" +
                                            ", @DataDe='" + dtDe.ToString("yyyyMMdd") + "'" +
                                            ", @DataAte= '" + dtAte.ToString("yyyyMMdd") + "'" +
                                            ", @Operador= '" + operador + "'" +
                                            ", @Status_Pdv= '" + statusPdv + "'";

            Gridrelatorio.DataSource = Conexao.GetTable(sql, usr,false);
            Gridrelatorio.DataBind();

        }


        protected void Gridrelatorio_DataBound(object sender, EventArgs e)
        {
            Decimal vlrRegi = 0;
            Decimal vlrDig = 0;


            foreach (GridViewRow linha in Gridrelatorio.Rows)
            {
                Decimal vlrLinhaReg = 0;
                Decimal.TryParse(linha.Cells[2].Text, out vlrLinhaReg);

                Decimal vlrLinhaDig = 0;
                Decimal.TryParse(linha.Cells[3].Text, out vlrLinhaDig);

                vlrRegi += vlrLinhaReg;
                vlrDig += vlrLinhaDig;

            }
            if (Gridrelatorio.FooterRow != null)
            {
                GridViewRow footer = Gridrelatorio.FooterRow;
                footer.Cells[2].Text = vlrRegi.ToString("N2");
                footer.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                footer.Cells[3].Text = vlrDig.ToString("N2");
                footer.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                Decimal vlrDif = 0;
                if (vlrRegi > 0 || vlrDig > 0)
                {
                    vlrDif = (vlrDig - vlrRegi);
                }
                footer.Cells[4].Text = vlrDif.ToString("N2");
                footer.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            }
        }

        protected void Gridrelatorio_RowCreated(object sender, GridViewRowEventArgs e)
        {


            


        }
        private bool isnumero(String numero)
        {
            try
            {
                decimal number3 = 0;
                bool resultado = Decimal.TryParse(numero, out number3);
                return resultado;

            }
            catch (Exception)
            {

                return false;
            }

        }

        protected void Gridrelatorio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null && e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    String cell = e.Row.Cells[i].Text.Replace("||", "").Replace("|-", "").Replace("-|", "");

                    if (isnumero(cell))
                    {
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                        e.Row.Cells[i].Text = Decimal.Parse(cell).ToString("N2");

                    }
                }
            }

        }

        protected void Gridrelatoiro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            

        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {

            
        }
    }
}