using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Sped.pages
{
    public partial class RegistrosFiscaisDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                pesquisar();
            }

            sopesquisa(pnBtn);
        }

        protected void pesquisar()
        {

            User usr = (User)Session["User"];
            if (usr != null)
            {
                DateTime dtDe = new DateTime();
                DateTime dtAte = new DateTime();
                String cst = "";
                Decimal aliquota = 0;
                String cfop = "";
                int tipoNF = 0;
                string origemInf = "";

                if (Request.Params["dtDe"] != null)
                {
                    DateTime.TryParse(Request.Params["dtDe"].ToString(), out dtDe);

                }
                if (Request.Params["dtAte"] != null)
                {
                    DateTime.TryParse(Request.Params["dtAte"].ToString(), out dtAte);
                }
                if (Request.Params["cst"] != null)
                {
                    cst = Request.Params["cst"].ToString();
                }

                if (Request.Params["cfop"] != null)
                {
                    cfop = Request.Params["cfop"].ToString();
                }

                if (Request.Params["aliquota"] != null)
                {
                    Decimal.TryParse(Request.Params["aliquota"].ToString(), out aliquota);
                }

                if (Request.Params["tipoNF"] != null)
                {
                    tipoNF = (Request.Params["tipoNF"].ToString().Equals("E") ? 2 : 1);
                    //int.TryParse(Request.Params["tipoNF"].ToString(), out tipoNF);
                }

                if (Request.Params["Origem"] != null)
                {
                    origemInf = Request.Params["Origem"].ToString();
                }

                String strWhere = "";
                txtDataDe.Text = dtDe.ToString("dd/MM/yyyy");
                txtDataAte.Text = dtAte.ToString("dd/MM/yyyy");

                txtCSTICMS.Text = cst;
                txtCodOperacao.Text = cfop;
                txtAliquotaIcms.Text = aliquota.ToString("N2");
                txtTipoNF.Text = (tipoNF == 1 ? "SAÍDA" : "ENTRADA");
                txtOrigem.Text = origemInf;

                if (!txtNota.Text.Equals(""))
                {
                    strWhere += " and nf.codigo='" + txtNota.Text + "'";
                }
                if (!txtFornecedor.Text.Equals(""))
                {
                    strWhere += " and fornecedor like '" + txtFornecedor.Text + "%'";
                }
                if (!txtPLU.Text.Equals(""))
                {
                    strWhere += " and nf_item.plu ='" + txtPLU.Text + "'";
                }

                String sql = "";

                if (origemInf.ToUpper().Equals("NFE"))
                {
                    sql = "Select	NF.Codigo AS NOTA," +
                                          " ISNULL(NF.Serie, 0) AS Serie, " +
                                          " NF.Cliente_Fornecedor," +
                                          " NF_ITEM.Num_Item," +
                                          " NF_ITEM.PLU," +
                                          " CONVERT(VARCHAR,ISNULL(mercadoria.origem,'0'))+" +
                                          " CONVERT(VARCHAR,ISNULL(NF_ITEM.cst_icms,'0')) as CST_ICMS," +
                                          " nf_item.codigo_operacao," +
                                          " aliquota_icms," +
                                          " (nf_item.total  - isnull(nf_item.desconto_valor, 0) +  isnull(nf_item.frete, 0) +  isnull(nf_item.despesas, 0)  + isnull(nf_item.iva, 0) + isnull(nf_item.ipiv, 0) ) AS TOTAL_OPERACAO," +
                                          " base_icms AS BASE_ICMS," +
                                          " CASE WHEN ISNULL(NF_ITEM.vCredICMSSN, 0) <= 0 THEN icmsv ELSE ISNULL(NF_Item.vCredICMSSN, 0) END AS TOTAL_ICMS, " +
                                          " base_iva AS BASE_ICMS_ST, " +
                                          " iva AS TOTAL_ICMS_ST," +
                                          " IPIV AS TOTAL_IPI " +
                                          ",  CASE WHEN NF_Item.Tipo_NF = 1 THEN 'S' ELSE 'E' END AS TipoNF" +

                                   " from NF_Item " +
                                   " inner join nf on nf.Codigo=NF_Item.codigo and " +
                                                    " nf.Tipo_NF = nf_item.Tipo_NF and " +
                                                    " nf.Filial = nf_item.Filial " +
                                                    " and NF.Cliente_Fornecedor = Nf_item.Cliente_Fornecedor " +
                                   " inner join mercadoria on nf_item.plu = mercadoria.plu " +
                                   " inner join Natureza_Operacao Nat ON Nat.codigo_Operacao = Nf.Codigo_Operacao" +
                                   " WHERE NF.Filial = '" + usr.getFilial() + "'";
                    if (tipoNF.ToString().Equals("1"))
                    {
                        sql += " AND NF.Tipo_NF = 1 AND nf.Status = 'AUTORIZADO'";
                    }
                    else
                    {
                        sql += " AND ((NF.Tipo_NF = 2 AND Nat.Imprime_NF = 0) OR (NF.Tipo_NF = 2 AND Nat.Imprime_NF = 1 AND NF.Status = 'AUTORIZADO'))";
                    }
                    sql += " AND (nf.data between '" + dtDe.ToString("yyyyMMdd") + "' and '" + dtAte.ToString("yyyyMMdd") + "') " +
                                   " AND (CONVERT(VARCHAR,ISNULL(RTRIM(LTRIM(mercadoria.origem)),'0'))+" +
                                   //" CONVERT(VARCHAR,ISNULL(NF_ITEM.cst_icms,'0')) = '" + cst + "') and nf_item.codigo_operacao='" + cfop + "' and aliquota_icms=" + aliquota.ToString().Replace(",", ".") + " AND nf_item.Tipo_NF = " + tipoNF.ToString() +
                                   " CONVERT(VARCHAR,ISNULL(NF_ITEM.cst_icms,'0')) = '" + cst + "') and nf_item.codigo_operacao='" + cfop + "'  AND nf_item.Tipo_NF = " + tipoNF.ToString() +
                                   strWhere;
                    sql += " ORDER BY 3, 1, 2, 4 ";

                }
                else
                {
                    sql = "Select	S.Documento AS NOTA," +
                                          " '59' AS Serie, " +
                                          " ISNULL(S.CAIXA_SAIDA,'') AS Cliente_Fornecedor," +
                                          " S.Sequencia AS Num_Item," +
                                          " S.PLU," +
                                          " CONVERT(VARCHAR,ISNULL(m.origem,'0'))+" +
                                          " ISNULL(S.cst_icms,'0') as CST_ICMS," +
                                          " CASE WHEN S.CST_ICMS = '60' THEN '5405' ELSE '5102' END AS codigo_operacao," +
                                          " S.aliquota_icms," +
                                          " CONVERT(DECIMAL(12,2), (S.Vlr  - isnull(s.Desconto, 0) +  isnull(s.Acrescimo, 0))) AS TOTAL_OPERACAO," +
                                          " CONVERT(DECIMAL(12,2), CASE WHEN S.Aliquota_ICMS > 0  THEN  (S.Vlr  - isnull(s.Desconto, 0) +  isnull(s.Acrescimo, 0)) ELSE 0 END) AS BASE_ICMS," +
                                          " CONVERT(DECIMAL(12,2), CASE WHEN S.Aliquota_ICMS > 0  THEN  (S.Vlr  - isnull(s.Desconto, 0) +  isnull(s.Acrescimo, 0)) * (S.Aliquota_ICMS / 100)  ELSE 0 END) AS TOTAL_ICMS, " +
                                          " 0 AS BASE_ICMS_ST, " +
                                          " 0 AS TOTAL_ICMS_ST," +
                                          " 0 AS TOTAL_IPI " +
                                          ", 'S' AS TipoNF" +

                                   " FROM SAIDA_ESTOQUE S WITH (INDEX=IX_SAIDA_ESTOQUE, NOLOCK)  " +
                                   " INNER JOIN Mercadoria M ON S.PLU = M.PLU " +
                                   " WHERE S.Filial = '" + usr.getFilial() + "'" +
                                   " AND Data_Movimento BETWEEN '" + dtDe.ToString("yyyyMMdd") + "' AND '" + dtAte.ToString("yyyyMMdd") + "' AND DATA_CANCELAMENTO IS NULL";
                    //Tratamento CST
                    if (!cst.Equals(""))
                    {
                        sql += " AND CONVERT(VARCHAR,ISNULL(RTRIM(LTRIM(M.origem)),'0'))+ CONVERT(VARCHAR,ISNULL(S.cst_icms,'0')) = '" + cst + "'";
                    }
                    //Tratamento Aliquota
                    if (aliquota > 0)
                    {
                        sql += " AND S.ALIQUOTA_ICMS = " + aliquota.ToString().Replace(",",".");
                    }
                    sql += " ORDER BY 3, 1, 4";
                }


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

            LinkButton lkNota = (LinkButton)gridPesquisa.Rows[index].Cells[0].Controls[0];
            String nota = lkNota.Text;

            LinkButton lkSerie = (LinkButton)gridPesquisa.Rows[index].Cells[1].Controls[0];
            String serie = lkSerie.Text.Trim();

            LinkButton lkFornecedor = (LinkButton)gridPesquisa.Rows[index].Cells[2].Controls[0];
            String fornecedor = lkFornecedor.Text.Trim();

            LinkButton lkTipoNF = (LinkButton)gridPesquisa.Rows[index].Cells[11].Controls[0];
            String tipoNF = lkTipoNF.Text.Trim();

            if (txtOrigem.Text.Equals("PDV"))
            {

            }
            else
            {
                if (tipoNF.Equals("E"))
                {
                    RedirectNovaAba("~/modulos/NotaFiscal/pages/NfEntradaDetalhes.aspx?codigo=" + nota + "&fornecedor=" + fornecedor + "&serie=" + serie);
                }
                else
                {
                    RedirectNovaAba("~/modulos/NotaFiscal/pages/NfSaidaDetalhes.aspx?codigo=" + nota + "&cliente=" + fornecedor);
                }
            }
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

        protected void ImgBtnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("RegistrosFiscais.aspx");
        }
    }
}