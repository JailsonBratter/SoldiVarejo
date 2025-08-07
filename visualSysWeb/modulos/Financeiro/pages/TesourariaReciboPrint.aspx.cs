using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Financeiro.pages
{
    public partial class TesourariaReciboPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            User usr = (User)Session["User"];
            if (usr != null)
            {
                String id = Request.Params["id"];
                String emissao = Request.Params["emissao"];
                String pdv = Request.Params["pdv"];
                String id_fechamento = Request.Params["idFechamento"];
                DateTime Dt;
                DateTime.TryParse(emissao, out Dt);


                lblOperador.Text = id + '-' + Conexao.retornaUmValor("Select Nome from operadores where ID_Operador =" + id, usr);
                lblData.Text = Dt.ToString("dd/MM/yyyy");
                lblpdv.Text = pdv;
                lblUsuario.Text = usr.getNome();
                lblIdMovimento.Text = id_fechamento;

               String sql = 
                   
                   "Select finalizadora AS COD_FINALIZADORA , id_finalizadora  AS FINALIZADORA, convert( decimal(18,2),Sum(Valor)) as REGISTRADO ,convert(decimal(18,2),SUM(digitado)) as VALOR,convert(decimal(18,2),(SUM(digitado)- Sum(Valor) ))AS DIFERENCA" +
                     " from (" +
                     " Select CR.finalizadora,  cr.id_finalizadora, 	Sum(isnull(cr.Total,0)) AS Valor  , 0 AS Digitado " +
                     "     from Tesouraria_detalhes cr " +
                     " where cr.id_fechamento =" + id_fechamento + " and cr.pdv=" + pdv + " and cr.operador = " + id +
                     " group by CR.finalizadora,  cr.id_finalizadora " +
                     " UNION " +
                     " SELECT Finalizadora, id_finalizadora,0,T.Total_Entregue FROM  Tesouraria t " +
                     " WHERE T.ID_OPERADOR = " + id + " AND T.id_fechamento= " + id_fechamento+ " AND T.PDV= " + pdv +
                     " )as a " +
                     " GROUP BY finalizadora, id_finalizadora " +
                     " order by finalizadora";
               gridFinalizadoras.DataSource = Conexao.GetTable(sql, usr, false);
               gridFinalizadoras.DataBind();
            }
        }

        protected void gridFinalizadoras_DataBound(object sender, EventArgs e)
        {
                Decimal vTotalRegistrado = 0;
                Decimal vTotalValor = 0;
                Decimal vTotalDivergencia = 0;
                
                foreach (GridViewRow rw in gridFinalizadoras.Rows)
                {

                    Decimal vReg = 0;
                    Decimal.TryParse(rw.Cells[2].Text, out vReg);
                    vTotalRegistrado += vReg;

                    Decimal vValor = 0;
                    Decimal.TryParse(rw.Cells[3].Text, out vValor);
                    vTotalValor += vValor;

                }

                GridViewRow footer = gridFinalizadoras.FooterRow;
                if (footer != null)
                {
                    footer.Cells[0].Text = "TOTAIS";
                    footer.Cells[0].HorizontalAlign = HorizontalAlign.Right;

                    footer.Cells[2].Text = vTotalRegistrado.ToString("N2");
                    footer.Cells[2].HorizontalAlign = HorizontalAlign.Right;

                    footer.Cells[3].Text = vTotalValor.ToString("N2");
                    footer.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                    vTotalDivergencia = vTotalValor - vTotalRegistrado;
                    footer.Cells[4].Text = vTotalDivergencia.ToString("N2");
                    footer.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                }
            
        }
    }
}