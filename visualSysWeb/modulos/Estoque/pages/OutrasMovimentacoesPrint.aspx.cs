using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Estoque.pages
{
    public partial class OutrasMovimentacoesPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["user"];
            if (usr != null)
            {

                inventarioDAO obj = (inventarioDAO)Session["objImprimir"];

                lblCodigo.Text = obj.Codigo_inventario;
                lblTipoMovimentacao.Text = obj.tipoMovimentacao;
                lblDescricao.Text = obj.Descricao_inventario;
                lblStatus.Text = obj.status;
                lblData.Text = obj.DataBr();
                lblUsuario.Text = obj.Usuario;
                String tipoRelatorio = Request.Params["TIPO"];
                if (tipoRelatorio == null || tipoRelatorio.Equals("CONTAGEM"))
                {


                    gridMercadoriasSelecionadas.Columns[6].Visible = false; //SALDO ATUAL
                    gridMercadoriasSelecionadas.Columns[10].Visible = false; //CUSTO
                    gridMercadoriasSelecionadas.Columns[7].Visible = false; //CONTADO
                    gridMercadoriasSelecionadas.Columns[11].Visible = false; //TOTAL
                    gridMercadoriasSelecionadas.Columns[8].Visible = false; //DIVERGENCIA
                    gridMercadoriasSelecionadas.Columns[12].Visible = false; //TOTAL divergencia
                    gridMercadoriasSelecionadas.Columns[13].Visible = false; // Total Final
                    if (obj.tSaidaEntrada == 2)
                    {
                        gridMercadoriasSelecionadas.Columns[9].HeaderText = "Contado"; // TEXTO ITEMCONT CAMPO EM BRANCO

                    }
                    else
                    {
                        gridMercadoriasSelecionadas.Columns[9].HeaderText = "Qtde"; // TEXTO ITEMCONT CAMPO EM BRANCO
                    }

                }
                else if (tipoRelatorio.Equals("CONFERENCIA"))
                {
                    gridMercadoriasSelecionadas.Columns[10].Visible = false; //CUSTO
                    gridMercadoriasSelecionadas.Columns[11].Visible = false; //TOTAL atual
                    gridMercadoriasSelecionadas.Columns[12].Visible = false; //TOTAL divergencia
                    gridMercadoriasSelecionadas.Columns[13].Visible = false; // Total Final

                    gridMercadoriasSelecionadas.Columns[9].HeaderText = "Conferir"; // TEXTO ITEMCONT CAMPO EM BRANCO
                    if (obj.tSaidaEntrada == 2)
                    {
                        gridMercadoriasSelecionadas.Columns[7].HeaderText = "Contado"; // TEXTO ITEMCONT CAMPO EM BRANCO

                    }
                    else
                    {
                        gridMercadoriasSelecionadas.Columns[7].HeaderText = "Qtde"; // TEXTO ITEMCONT CAMPO EM BRANCO
                        gridMercadoriasSelecionadas.Columns[6].Visible = false; // divergencia
                        
                    }



                }
                else if (tipoRelatorio.Equals("ENCERRADO"))
                {
                    if (obj.tSaidaEntrada == 2)
                    {
                        gridMercadoriasSelecionadas.Columns[7].HeaderText = "Saldo Atual"; // TEXTO CONTADO
                        gridMercadoriasSelecionadas.Columns[13].Visible = false; // Total Final
                        
                    }
                    else
                    {
                        gridMercadoriasSelecionadas.Columns[7].HeaderText = "Qtde"; // TEXTO CONTADO
                        gridMercadoriasSelecionadas.Columns[8].Visible = false; // divergencia
                        gridMercadoriasSelecionadas.Columns[12].Visible = false; //  total divergencia
                        gridMercadoriasSelecionadas.Columns[11].HeaderText = "Total ajuste"; // TOTAL CONTADO
                        

                    }
                    gridMercadoriasSelecionadas.Columns[9].Visible = false; //CAMPO EM BRANCO
                    gridMercadoriasSelecionadas.Columns[6].HeaderText = "Saldo (Ant)"; // TEXTO CONTADO
                }



                gridMercadoriasSelecionadas.DataSource = obj.itensImprimir();
                gridMercadoriasSelecionadas.DataBind();
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void gridMercadoriasSelecionadas_DataBinding(object sender, EventArgs e)
        {
        }

        protected void gridMercadoriasSelecionadas_DataBound(object sender, EventArgs e)
        {
                String tipoRelatorio = Request.Params["TIPO"];
                if (tipoRelatorio != null && tipoRelatorio.Equals("ENCERRADO"))
                {
                    Decimal vTotalEstoque = 0;
                    Decimal vTotalDivergencia = 0;
                    Decimal vTotalFinal = 0;

                    foreach (GridViewRow rw in gridMercadoriasSelecionadas.Rows)
                    {
                        if (!rw.Cells[12].Text.Equals(""))
                        {
                            vTotalFinal += Decimal.Parse(rw.Cells[12].Text);
                        }
                        if (!rw.Cells[11].Text.Equals(""))
                        {
                            vTotalDivergencia += Decimal.Parse(rw.Cells[11].Text);
                        }
                        if (!rw.Cells[10].Text.Equals(""))
                        {
                            vTotalEstoque += Decimal.Parse(rw.Cells[10].Text);
                        }


                    }

                    GridViewRow footer = gridMercadoriasSelecionadas.FooterRow;
                    if (footer != null)
                    {
                        footer.Cells[0].Text = "TOTAIS";
                        footer.Cells[0].HorizontalAlign = HorizontalAlign.Right;

                        footer.Cells[10].Text = vTotalEstoque.ToString("N2");
                        footer.Cells[10].HorizontalAlign = HorizontalAlign.Right;

                        footer.Cells[11].Text = vTotalDivergencia.ToString("N2");
                        footer.Cells[11].HorizontalAlign = HorizontalAlign.Right;

                        footer.Cells[12].Text = vTotalFinal.ToString("N2");
                        footer.Cells[12].HorizontalAlign = HorizontalAlign.Right;


                    }
                }
                else
                {
                    gridMercadoriasSelecionadas.FooterRow.Visible = false;
                }
        }
    }
}