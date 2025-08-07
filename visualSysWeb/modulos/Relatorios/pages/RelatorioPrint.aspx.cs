using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.modulos.Relatorios.code;
using System.Collections;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Relatorios.pages
{
    public partial class RelatorioPrint : System.Web.UI.Page
    {
        static Relatorio rel = null;
        String CampoAgrupado = "";
        int intSubTotalIndex = 1;

        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["user"] == null || Session["rel"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                Relatorio rel = (Relatorio)Session["rel"];
                imgLog.ImageUrl = rel.enderecoImg;
                lblCabecalho.Text = rel.cabecalho;
                imgLog.Width = 40;
                User usr = (User)Session["User"];

                if (Request.Params["Simplificado"] != null)
                {
                    Gridrelatorio.DataSource = rel.ReltableSimples(usr);
                }
                else
                {


                    Gridrelatorio.DataSource = rel.Reltable(usr);
                }
                
                Gridrelatorio.DataBind();

                foreach (GridViewRow grRow in Gridrelatorio.Rows)
                {
                    //grRow = Gridrelatorio.Rows[indexRow];
                    for (int i = 0; i < grRow.Cells.Count; i++)
                    {

                        String cell = grRow.Cells[i].Text.Replace("||", "").Replace("|-", "").Replace("-|", "");

                        if (grRow.Cells[i].Text.IndexOf("|-") < 0)
                        {
                            if (cell.ToUpper().IndexOf("PLU") >= 0)
                            {
                                grRow.Cells[i].Text = cell.ToUpper().Replace("PLU", ""); // CAMPO PLU SEM FORMATACAO
                            }
                            if (cell.ToUpper().IndexOf("SFT_") >= 0) // CAMPO SEM FORMATACAO
                            {
                                grRow.Cells[i].Text = cell.ToUpper().Replace("SFT_", "");
                            }
                        }
                        else
                        {
                            grRow.Cells[i].Text = grRow.Cells[i].Text.Replace("||", "<br>");
                            grRow.Cells[i].Text = grRow.Cells[i].Text.Replace("|-", "<b>");
                            grRow.Cells[i].Text = grRow.Cells[i].Text.Replace("-|", "</b>");

                            if (isnumero(cell))
                            {
                                grRow.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                            }
                        }

                    }
                }


                lblordem.Text = rel.ordem;
                lblfiltros.Text = rel.strfiltros;
                LblRodape.Text = rel.rodape;




            }


        }

        protected void Gridrelatorio_DataBound(object sender, EventArgs e)
        {

            User usr = (User)Session["User"];
            Relatorio rel = (Relatorio)Session["Rel"];
            ArrayList arrTotais ;
            if (Request.Params["Simplificado"] != null)
            {
                arrTotais = rel.getTotaisSimples(usr);
            }
            else
            {

                arrTotais = rel.getTotais(usr);
            }


            if (arrTotais.Count > 0)
            {
                GridViewRow footer = Gridrelatorio.FooterRow;
                if (footer != null)
                {
                    footer.Cells[0].Text = "TOTAIS";
                    footer.Cells[0].HorizontalAlign = HorizontalAlign.Right;

                    foreach (Total to in arrTotais)
                    {
                        try
                        {
                            if ((!rel.semFormatacao(to.index)))
                            {
                                footer.Cells[to.index].Text = to.total.ToString("N2");
                                if (footer.Cells[to.index].Text.Equals("-1,00"))
                                    footer.Cells[to.index].Text = "";
                            }
                            else
                            {
                                footer.Cells[to.index].Text = to.total.ToString("N0");
                            }
                            footer.Cells[to.index].HorizontalAlign = HorizontalAlign.Right;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                if (!rel.campoAgrupaTotais.Trim().Equals(""))
                {
                    Gridrelatorio.AlternatingRowStyle.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    Gridrelatorio.AlternatingRowStyle.BackColor = System.Drawing.Color.LightGray;
                }

            }



        }

        protected void Gridrelatorio_RowCreated(object sender, GridViewRowEventArgs e)
        {



            Relatorio rel = (Relatorio)Session["Rel"];
            bool AddNovoTotal = false;

            if (!rel.campoAgrupaTotais.Trim().Equals("") && (DataBinder.Eval(e.Row.DataItem, rel.campoAgrupaTotais) != null))
            {
                if ((!CampoAgrupado.Trim().Equals("")))

                    if (!CampoAgrupado.Equals(DataBinder.Eval(e.Row.DataItem, rel.campoAgrupaTotais).ToString()))
                    {
                        AddNovoTotal = true;
                    }

            }
            if ((!CampoAgrupado.Trim().Equals("")) && (DataBinder.Eval(e.Row.DataItem, rel.campoAgrupaTotais) == null))
            {
                AddNovoTotal = true;
                intSubTotalIndex = 0;
            }

            if (AddNovoTotal)
            {

                GridView grdViewProducts = (GridView)sender;

                // Creating a Row
                GridViewRow SubTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

                for (int i = 0; rel.qtdCampos > i; i++)
                {
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.CssClass = "subtotal";
                    if (i == 0)
                    {
                        HeaderCell.Text = "Sub Total";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Left;

                    }
                    else
                    {
                        HeaderCell.Text = rel.getTotalGrupo(i);
                        HeaderCell.HorizontalAlign = HorizontalAlign.Right;

                    }
                    SubTotalRow.Cells.Add(HeaderCell);

                }
                SubTotalRow.BackColor = System.Drawing.Color.LightGray;
                SubTotalRow.ForeColor = System.Drawing.Color.Black;
                grdViewProducts.Controls[0].Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, SubTotalRow);
                intSubTotalIndex++;
                rel.zerarTotaisGrupo();
            }

            //Session.Remove("Rel");
            // Session.Add("Rel", rel);



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
            try
            {
                Relatorio rel = (Relatorio)Session["Rel"];
                if (rel.formataRelatorio)
                {
                    if (e.Row.DataItem != null && e.Row.RowType == DataControlRowType.DataRow)
                    {

                        for (int i = 0; i < e.Row.Cells.Count; i++)
                        {
                            String cell = e.Row.Cells[i].Text.Replace("||", "").Replace("|-", "").Replace("-|", "");

                            if (isnumero(cell))
                            {
                                e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                                // e.Row.Cells[i].Text = Decimal.Parse(e.Row.Cells[i].Text).ToString("N2");

                            }

                            e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("||", "<br/>");
                            e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("|-", "<b>");
                            e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("-|", "</b>");


                            if (e.Row.Cells[i].Text.ToUpper().IndexOf("TOTAL") >= 0)
                            {
                                e.Row.CssClass = "subtotal";
                            }


                        }



                        //Relatorio rel = (Relatorio)Session["Rel"];
                        if (!rel.campoAgrupaTotais.Equals(""))
                        {
                            CampoAgrupado = DataBinder.Eval(e.Row.DataItem, rel.campoAgrupaTotais).ToString();
                            ArrayList arrTotais = rel.getTotalGrupo();
                            foreach (GrupoSubtotal item in arrTotais)
                            {
                                rel.setTotalGrupo(item.Campo, Decimal.Parse(DataBinder.Eval(e.Row.DataItem, item.Campo).ToString()));
                                rel.setTotal(item.posicao, DataBinder.Eval(e.Row.DataItem, item.Campo).ToString());

                            }

                        }
                        Session.Remove("Rel");
                        Session.Add("Rel", rel);

                    }
                }
            }
            catch (Exception)
            {


            }

        }

        protected void Gridrelatoiro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            User usr = (User)Session["User"];
            Gridrelatorio.DataSource = rel.Reltable(usr);
            Gridrelatorio.PageIndex = e.NewPageIndex;
            Gridrelatorio.DataBind();

        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {

            rel.ordem = e.SortExpression;
            lblordem.Text = e.SortExpression;
            //visualizarRelatorio();
        }
    }
}