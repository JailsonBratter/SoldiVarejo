using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Tesouraria.pages
{
    public partial class Tesouraria : visualSysWeb.code.PagePadrao
    {
        decimal total = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            sopesquisa(pnBtn);

            if (!IsPostBack)
            {

                String emissao = "";
                String dtAte = "";
                if (Request.Params["emissao"]!=null)
                {
                    emissao = Request.Params["emissao"].ToString();
                    dtAte = emissao;
                }
                else
                {
                    emissao = (String)Session["dtDeTesouraria"];
                    dtAte = (String)Session["dtAteTesouraria"];
                }
                
                if (emissao != null)
                {
                    TxtData.Text = emissao;
                }
                if (TxtData.Text.Equals(""))
                {
                    TxtData.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (dtAte != null)
                {
                    txtDataAte.Text = dtAte;
                }

                if (txtDataAte.Text.Equals(""))
                {
                    txtDataAte.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }

                String sqlOp = "Select '' AS ID ,'TODOS' AS NOME , ''AS ORDEM " +
                               " UNION ALL " +
                               " SELECT ID_OPERADOR, NOME, Nome AS ORDEM FROM Operadores  ORDER BY ORDEM ";

                Conexao.preencherDDL(ddlOperador, sqlOp, "NOME", "ID", usr);

                carregarAbertos();
            }
            TxtData.Attributes.Add("OnKeyUp", "javascript:return formataData(this,event);");
            txtDataAte.Attributes.Add("OnKeyUp", "javascript:return formataData(this,event);");
            analisarGrid();
        }


        private void formatarCampos()
        {




        }



        protected void carregarAbertos()
        {



            String status = "";
            User usr = (User)Session["User"];



            if (!ddlStatus.Text.Equals("TODOS"))
            {
                status = " and isnull(sp.Status,'OPERANDO') = '" + ddlStatus.Text + "'";
            }
           

            String operador = "";
            if (usr != null)
            {
                if (usr.adm())
                {
                    if (!ddlOperador.Text.Equals("0"))
                    {
                        operador = " and op.id_operador=" + ddlOperador.SelectedValue;
                    }
                    divOperador.Visible = true;
                    divStatus.Visible = true;
                    //if (ddlStatus.Text.Equals("TODOS"))
                    //{   
                    //    status = " and isnull(sp.Status,'OPERANDO') <> 'OPERANDO'";
                    //}
                }
                else
                {   
                    divOperador.Visible = false;
                    divStatus.Visible = false;
                    operador = " and op.id_operador=" + usr.id_operador;

                   

                    
                }
            }


            DateTime dt;
            DateTime dtAte;
            String strDt = TxtData.Text.PadLeft(10,'0');
            if (IsDate(strDt))
                dt = DateTime.Parse(strDt);
            else
                dt = DateTime.Now;


            String strDtAte = txtDataAte.Text.PadLeft(10, '0');
            if (IsDate(strDtAte))
                dtAte = DateTime.Parse(strDtAte);
            else
                dtAte = DateTime.Now;


            Session.Remove("dtDeTesouraria");
            Session.Remove("dtAteTesouraria");
            Session.Add("dtDeTesouraria", dt.ToString("dd/MM/yyyy"));
            Session.Add("dtAteTesouraria", dtAte.ToString("dd/MM/yyyy"));


            String sql = "Select id_fechamento = isnull(sp.id_fechamento,0)" +
                                    ",Operador= sp.id_operador " +
                                    " ,Nome= convert(varchar,sp.id_operador) +'-'+op.Nome " +
                                    ",data_abertura = convert(varchar,sp.data_abertura,103)+'-' + Replace(substring(convert(varchar,sp.data_abertura,108),1,5),':','&#58;')" +
                                    ",data_encerramento = convert(varchar,sp.data_fechamento_movimento,103)+'-'+ Replace(substring(convert(varchar,sp.data_fechamento_movimento,108),1,5),':','&#58;') " +
                                    ",data_fechamento = convert(varchar,sp.data_fechamento,103)+'-'+ Replace(substring(convert(varchar,sp.data_fechamento,108),1,5),':','&#58;') " +
                                    
                                    ", sp.pdv " +
                                    ", Cancelados =(SELECT SUM(SE.VLR-isnull(SE.desconto,0)) " +
                                                 " FROM SAIDA_ESTOQUE AS SE WITH(INDEX(ix_tesouraria)) " +
                                                 " WHERE  FILIAL = sp.filial AND Data_Cancelamento IS NOT NULL	and id_movimento = sp.id_fechamento  " +
                                                 " AND SE.caixa_saida =sp.pdv AND se.codigo_funcionario =sp.id_operador"+
                                                 ")" +
                                    ", Status = isnull(sp.Status,'OPERANDO')      " +
                                    " ,Total = sum(isnull(lf.total,0))"+
                                    ", Terceiros = ISNULL((select top 1 cfp.pdv_terceiros from controle_filial_pdv cfp WHERE CFP.caixa = sp.pdv),'')"+
                           " from   Status_Pdv as  sp " +
                           " inner join Operadores as op on  op.ID_Operador=sp.id_operador " +
                           " left join Lista_finalizadora  as lf WITH(INDEX(ix_lf_tesouraria))  on sp.Id_Operador = lf.Operador " +
                                                               " and sp.pdv= lf.pdv " +
                                                               " and sp.id_fechamento = lf.id_movimento " +
                          " where isnull(lf.Cancelado,0) <>1 AND ( convert(date,sp.Data_Abertura)  between '" + dt.ToString("yyyy-MM-dd") + "' and '" + dtAte.ToString("yyyy-MM-dd") + "' ) " + status + operador +
                          "  group by sp.filial,sp.Id_Operador, op.Nome, sp.pdv, sp.Status,sp.id_fechamento, sp.data_fechamento  ,sp.data_abertura, sp.data_fechamento_movimento  " +
                          " order by sp.id_fechamento desc, sp.pdv, op.Nome";


            gridHistorico.DataSource = Conexao.GetTable(sql, usr, false);
            gridHistorico.DataBind();

            analisarGrid();
            
        }

        protected void calData_SelectionChanged(object sender, EventArgs e)
        {
            String btnDataAtual = (String)Session["btnDataAtual" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];

            //TxtData.Text = calData.SelectedDate.ToString("dd/MM/yyyy");
            //PnData.Visible = false;
            //carregarhistorico();
        }

        protected void imgCancelaCalendar_Click(object sender, ImageClickEventArgs e)
        {//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            //PnData.Visible = false;
        }
        //protected void tabMenu_MenuItemClick(object sender, MenuEventArgs e)
        //{
        //    switch (e.Item.Value)
        //    {
        //        case "tab1":
        //            MultiView1.ActiveViewIndex = 0;
        //            break;
        //        case "tab2":
        //            MultiView1.ActiveViewIndex = 1;
        //            break;
        //    }
        //}

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
            carregarAbertos();
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

        protected void img_TxtData_Click(object sender, ImageClickEventArgs e)
        {
            //PnData.Visible = true;
            //calData.SelectedDate = DateTime.Parse(TxtData.Text);
        }

        protected void TxtData_TextChanged(object sender, EventArgs e)
        {
            DateTime value;
            if (DateTime.TryParse(TxtData.Text, out value))
            {
                txtDataAte.Text = TxtData.Text;

                carregarAbertos();
            }
        }
        protected void TxtDataAte_TextChanged(object sender, EventArgs e)
        {
            DateTime value;
            if (DateTime.TryParse(txtDataAte.Text, out value))
            {
                carregarAbertos();
            }
        }

        protected void imgBtnRelatorio_Click(object sender, ImageClickEventArgs e)
        {
            string strOperador = "TODOS";
            if (!ddlOperador.Text.Equals("0"))
            {
                strOperador = ddlOperador.SelectedValue;
            }

            RedirectNovaAba("TesourariaReport.aspx?dtDe=" + TxtData.Text +
                                                 "&dtAte=" + txtDataAte.Text +
                                                 "&operador=" + strOperador +
                                                 "&statusPdv=" + ddlStatus.Text);
        }

        protected void gridHistorico_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            User usr = (User)Session["User"];

            int index = Convert.ToInt32(e.CommandArgument);

            HyperLink lkFechamento = (HyperLink)gridHistorico.Rows[index].Cells[0].Controls[0];
            HyperLink lkpdv = (HyperLink)gridHistorico.Rows[index].Cells[4].Controls[0];
            HyperLink lkOperador = (HyperLink)gridHistorico.Rows[index].Cells[9].Controls[0];

            string fechamento = lkFechamento.Text;
            string pdv = lkpdv.Text;
            string operador = lkOperador.Text;

            if (e.CommandName == "Finalizadoras")
            {
                RedirectNovaAba("~/modulos/Financeiro/pages/FinalizadorasRecebimento.aspx?fechamento=" + fechamento + "&pdv=" + pdv + "&operador=" + operador);
            }
            else if (e.CommandName == "Help")
            {
                string sql = "select hp.Tipo_Pagamento, hp.Valor_Total FROM Help_Fechamento  hf INNER JOIN Help_Pagamentos hp on hf.Id_Sessao = hp.Id_Sessao WHERE ";
                sql += "hf.Id_Operador = " + operador + " AND HF.Num_Serie_Nfe = " + pdv + " AND HF.id_Fechamento = " + fechamento;

                GridLista.DataSource = Conexao.GetTable(sql, usr, false);
                GridLista.DataBind();

                modalPnFundo.Show();

                //int index = Convert.ToInt32(e.CommandArgument);

                //HyperLink lkFechamento = (HyperLink)gridHistorico.Rows[index].Cells[0].Controls[0];
                //HyperLink lkpdv = (HyperLink)gridHistorico.Rows[index].Cells[4].Controls[0];
                //HyperLink lkOperador = (HyperLink)gridHistorico.Rows[index].Cells[9].Controls[0];

                //string fechamento = lkFechamento.Text;
                //string pdv = lkpdv.Text;
                //string operador = lkOperador.Text;

            }

        }

        protected void gridHistorico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //int index = Convert.ToInt32(e.Row.RowIndex);
            //if (index >= 0)
            //{
            //    string status = gridHistorico.Rows[index].Cells[6].Text;
            //}
        }

        private void analisarGrid()
        {
            foreach (GridViewRow row in gridHistorico.Rows)
            {
                HyperLink lkFechamento = (HyperLink)row.Cells[6].Controls[0];
                string status = lkFechamento.Text;
                if (status.Equals("FECHADO"))
                {
                    row.Cells[10].Controls.Clear();
                    //row[]
                }

                var pdvterceiros = row.Cells[12].Text;
                if (!pdvterceiros.Equals("HELP"))
                {
                    row.Cells[11].Controls.Clear();
                }
            }


            User usr = (User)Session["User"];

            if (Funcoes.intTry(Conexao.retornaUmValor("SELECT COUNT(*) AS Reg  FROM Controle_Filial_PDV WHERE ISNULL(PDV_Terceiros ,'') <> ''", usr)) <= 0)
            {
                gridHistorico.Columns[11].Visible = false;
                gridHistorico.Columns[12].Visible = false;
            }

        }

        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
        }

        protected void GridLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Somar valores
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal valor = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Valor_Total"));
                total += valor;
            }

            // Mostrar total no rodapé
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total:";
                e.Row.Cells[1].Text = total.ToString("N2"); // separador de milhar
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }

        }
    }
}