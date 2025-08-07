using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using visualSysWeb.code;
using visualSysWeb.modulos.Sped.code;
using System.Data;

namespace visualSysWeb.modulos.Sped.pages
{
    public partial class RegistrosFiscais : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                FiltroRegistroFiscal filtro = (FiltroRegistroFiscal)Session["filtroRegistroFiscal"];

                if (filtro != null)
                {
                    txtDataDe.Text = filtro.dtDe;
                    txtDataAte.Text = filtro.dtAte;
                    txtCSTICMS.Text = filtro.cst;
                    txtCodOperacao.Text = filtro.codigoOperacao;
                    txtAliquotaIcms.Text = filtro.aliquota;
                }
                else
                {
                    txtDataDe.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtDataAte.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                pesquisar();
            }

            sopesquisa(pnBtn);
        }

        protected void pesquisar()
        {

            User usr = (User)Session["User"];
            if (usr != null)
            {
                FiltroRegistroFiscal filtro = new FiltroRegistroFiscal();
                filtro.dtDe = txtDataDe.Text;
                filtro.dtAte = txtDataAte.Text;
                filtro.cst = txtCSTICMS.Text;
                filtro.codigoOperacao = txtCodOperacao.Text;
                filtro.aliquota = txtAliquotaIcms.Text;

                Session.Remove("filtroRegistroFiscal");
                Session.Add("filtroRegistroFiscal", filtro);

                DateTime dtDe = new DateTime();
                DateTime dtAte = new DateTime();


                DateTime.TryParse(txtDataDe.Text, out dtDe);
                DateTime.TryParse(txtDataAte.Text, out dtAte);
                //String strWhere = "";

                String sql = "sp_Cons_Registros_Fiscais @fILIAL = '" + usr.getFilial() + "'";
                sql += ", @DataDe = '" + dtDe.ToString("yyyyMMdd") + "'";
                sql += ", @DataAte = '" + dtAte.ToString("yyyyMMdd") + "'";
                sql += ", @CST = '" + txtCSTICMS.Text + "'";
                sql += ", @CFOP = '" + txtCodOperacao.Text + "'";
                sql += ", @ALIQUOTA = '" + txtAliquotaIcms.Text + "'";

                //if (!txtCSTICMS.Text.Equals(""))
                //{
                //    strWhere += " and CONVERT(VARCHAR,ISNULL(RTRIM(LTRIM(mercadoria.origem)),'0'))+" +
                //               " CONVERT(VARCHAR,ISNULL(nf_item.cst_icms,'0')) = '" + txtCSTICMS.Text + "'";
                //}

                //if (!txtCodOperacao.Text.Equals(""))
                //{
                //    strWhere += " and nf_item.codigo_operacao ='" + txtCodOperacao.Text + "'";
                //}

                //if (!txtAliquotaIcms.Text.Equals(""))
                //{
                //    Decimal vlrAliquota = 0;
                //    Decimal.TryParse(txtAliquotaIcms.Text, out vlrAliquota);
                //    strWhere += " and aliquota_icms = " + vlrAliquota.ToString().Replace(",", ".");
                //}

                //String sql = "Select " +
                //               " CONVERT(VARCHAR,ISNULL(RTRIM(LTRIM(mercadoria.origem)),'0'))+" +
                //               " CONVERT(VARCHAR,ISNULL(nf_item.cst_icms,'0')) as CST_ICMS," +
                //               " nf_item.codigo_operacao," +
                //               " cast(isnull(aliquota_icms,0)as decimal(12,2)) as aliquota_icms," +
                //               " cast( SUM(isnull(nf_item.total,0) - isnull(nf_item.desconto_valor, 0) +  isnull(nf_item.frete, 0) +  isnull(nf_item.despesas, 0)  + isnull(nf_item.iva, 0) + isnull(nf_item.ipiv, 0) ) as decimal(12,2)) AS TOTAL_OPERACAO," +
                //               " cast( case when Tributacao.ICMSST_EmOutrasDespesas=1 then 0 else SUM(isnull(base_icms,0))end as decimal(12,2)) AS BASE_ICMS," +
                //               " cast( case when Tributacao.ICMSST_EmOutrasDespesas=1 then 0 else SUM(isnull(icmsv,0))end as decimal(12,2))  AS TOTAL_ICMS, " +
                //               " cast( case when Tributacao.ICMSST_EmOutrasDespesas=1 then 0 else  SUM(isnull(base_iva,0))end as decimal(12,2)) AS BASE_ICMS_ST, " +
                //               " cast( case when Tributacao.ICMSST_EmOutrasDespesas=1 then 0 else SUM(isnull(iva,0))end as decimal(12,2)) AS TOTAL_ICMS_ST," +
                //               " cast( case when Tributacao.IPI_EmOutrasDespesas=1 then 0 else SUM(isnull(IPIV,0))end as decimal(12,2)) AS TOTAL_IPI ," +

                //               " cast( " +
                //               " SUM(case when Tributacao.IPI_EmOutrasDespesas=1 then isnull(IPIV,0) else 0 end + " +
                //               "	case when Tributacao.ICMSST_EmOutrasDespesas=1 then isnull(iva,0) else  0 end " +
                //               " ) as decimal(12,2)) AS OUTRAS" +
                //               ", CASE WHEN NF_Item.Tipo_NF = 1 THEN 'S' ELSE 'E' END AS Tiponf "+	

                //       " from NF_Item " +
                //       " inner join nf on nf.Codigo=NF_Item.codigo and " +
                //                        " nf.Tipo_NF = nf_item.Tipo_NF and " +
                //                        " nf.Filial = nf_item.Filial " +
                //                        " and NF.Cliente_Fornecedor = Nf_item.Cliente_Fornecedor " +
                //       " inner join mercadoria on nf_item.plu = mercadoria.plu " +
                //       " inner join Tributacao on nf_item.Codigo_Tributacao = Tributacao.Codigo_Tributacao " +
                //       " inner join Natureza_Operacao Nat ON Nat.Codigo_Operacao = Nf.Codigo_Operacao" +
                //       " where nf.Filial='" + usr.getFilial() + "'" +
                //       " and ((nf.Tipo_NF = 2 AND ISNULL(nat.Imprime_NF,0) = 0) OR (nf.Tipo_NF = 2 AND ISNULL(nat.Imprime_NF,0) = 1 AND nf.Status = 'AUTORIZADO') OR (nf.Tipo_NF = 1 AND nf.Status = 'AUTORIZADO'))" +
                //       " and (nf.data between '" + dtDe.ToString("yyyyMMdd") + "' and '" + dtAte.ToString("yyyyMMdd") + "') " +
                //       strWhere +
                //       " GROUP BY mercadoria.origem,nf_item.cst_icms,nf_item.codigo_operacao, nf_item.Tipo_NF, aliquota_icms,Tributacao.ICMSST_EmOutrasDespesas,Tributacao.IPI_EmOutrasDespesas "+
                //        " ORDER BY 1, 2, 3";

                gridPesquisa.DataSource = Conexao.GetTable(sql, usr, false);
                gridPesquisa.DataBind();

            }
        }
        protected void txtDataDe_TextChanged(object sender, EventArgs e)
        {
            if (IsDate(txtDataDe.Text))
            {
                txtDataAte.Text = txtDataDe.Text;
            }
        }
        protected void gridPesquisa_Sorting(object sender, GridViewSortEventArgs e)
        {

            //pesquisar(e.SortExpression);

        }
        protected void gridPesquisa_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);

            LinkButton lkCst = (LinkButton)gridPesquisa.Rows[index].Cells[0].Controls[0];
            String cst = lkCst.Text;

            LinkButton lkCfop = (LinkButton)gridPesquisa.Rows[index].Cells[1].Controls[0];
            String cfop = lkCfop.Text;

            LinkButton lkAliq = (LinkButton)gridPesquisa.Rows[index].Cells[2].Controls[0];
            String aliquota = lkAliq.Text;

            LinkButton lkTipo = (LinkButton)gridPesquisa.Rows[index].Cells[10].Controls[0];
            String tipoNf = lkTipo.Text;

            LinkButton lkOrigem = (LinkButton)gridPesquisa.Rows[index].Cells[11].Controls[0];
            String Origem = lkOrigem.Text;

            Response.Redirect("RegistrosFiscaisDetalhes.aspx?dtDe=" + txtDataDe.Text +
                                                           "&dtAte=" + txtDataAte.Text +
                                                           "&cst=" + cst +
                                                           "&cfop=" + cfop +
                                                           "&aliquota=" + aliquota +
                                                           "&tipoNF=" + tipoNf +
                                                           "&origem=" + Origem
                                                           );
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisar();
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

        protected void imgBtnLimpar_Click(object sender, EventArgs e)
        {
            Session.Remove("filtroRegistroFiscal");
            Response.Redirect("RegistrosFiscais.aspx");


        }

        protected void gridPesquisa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowIndex >= 0)
            //{
            //    if (!e.Row.Cells[1].Text.Equals(""))
            //    {
            //        e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
            //    }
            //}
        }
    }
}