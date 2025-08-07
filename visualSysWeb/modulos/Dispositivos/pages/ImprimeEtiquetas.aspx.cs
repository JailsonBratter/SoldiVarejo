using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using visualSysWeb.code;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Text;


namespace visualSysWeb.modulos.Dispositivos.pages
{
    public class EtiquetasOrdem : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            return ((new CaseInsensitiveComparer()).Compare(((ArrayList)x)[6].ToString(), ((ArrayList)y)[6].ToString()));
        }
    }


    public partial class ImprimeEtiquetas : visualSysWeb.code.PagePadrao
    {
        static DataTable tb;
        //ArrayList imprimir = new ArrayList();
        static String sqlGrid = "select  distinct a.PLU,isnull(b.ean,'')EAN,a.Descricao, l.Preco, " +
                                        "CASE WHEN l.Data_Inicio<=CONVERT(VARCHAR,GETDATE(),102) AND l.Data_Fim >= CONVERT(VARCHAR,GETDATE(),102) THEN l.Preco_Promocao ELSE 0 END as Preco_Promocao ," +
                                        " a.codigo_departamento as departamento, a.und " +
                                " from Mercadoria_Loja l inner join  mercadoria a on a.PLU = l.PLU left join ean b on a.plu=b.plu " +
                                "   INNER JOIN departamento on a.codigo_departamento = departamento.codigo_departamento " +
                                                " INNER JOIN subgrupo on departamento.codigo_subgrupo = subgrupo.codigo_subgrupo " +
                                                " INNER JOIN grupo on subgrupo.codigo_grupo = grupo.codigo_grupo   ";

        static String sqlGrid2 = "select  distinct a.PLU,isnull(b.ean,'')EAN,a.Descricao, l.Preco," +
                                        " CASE WHEN l.Data_Inicio<=CONVERT(VARCHAR,GETDATE(),102) AND l.Data_Fim >= CONVERT(VARCHAR,GETDATE(),102) THEN l.Preco_Promocao ELSE 0 END as Preco_Promocao ," +
                                        "a.codigo_departamento as departamento, a.und " +
                                    " from Mercadoria_Loja l inner join  mercadoria a on a.PLU = l.PLU left join ean b on a.plu=b.plu ";


        protected void Page_Load(object sender, EventArgs e)
        {
            ArrayList imprimir = new ArrayList();
            try
            {

                if (!IsPostBack)
                {

                    if (Session["imprimir"] == null)
                    {
                        imprimir = new ArrayList();
                        Session.Add("imprimir", imprimir);
                    }
                    else
                    {
                        imprimir = (ArrayList)Session["imprimir"];
                    }
                    if (imprimir.Count == 0)
                    {
                        ArrayList cab = new ArrayList();
                        cab.Add("PLU");
                        cab.Add("EAN");
                        cab.Add("DESCRICAO");
                        cab.Add("preco");
                        cab.Add("preco_promocao");
                        cab.Add("qtd");
                        cab.Add("Departamento");
                        cab.Add("Und");


                        imprimir.Add(cab);

                        //    atualizarArquivo(false);
                    }
                    else
                    {
                        //  atualizarArquivo(true);
                    }

                    Session.Add("imprimir", imprimir);
                    gridImpressao.DataSource = Conexao.GetArryTable(imprimir);
                    gridImpressao.DataBind();
                }
                sopesquisa(pnBtn);
                txtDtCadastro.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
                txtDtalteracao.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
            }
            catch (Exception err)
            {
                lblPesquisaErro.Text = err.Message;
            }
            verificarItensImportar();
        }

        private void verificarItensImportar()
        {
            int qtde = Funcoes.intTry(Conexao.retornaUmValor("Select count(*) from  etiqueta", null));
            pnImportar.Visible = (qtde > 0);

        }

        private void camposnumericos()
        {
            txtPluEan.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
            txtNF.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
            txtfamilia.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
        }




        protected override void btnIncluir_Click(object sender, EventArgs e)
        { }



        protected void atualizarArquivo(bool conteudo, bool elgin = false)
        {
            try
            {

                if (rdoEtiquetas.SelectedValue.Equals("13"))
                {
                    RedirectNovaAba("EtiquetaA4.aspx");
                }
                else
                {


                    lblPesquisaErro.Text = "";
                    User usr = (User)Session["User"];
                    ArrayList imprimir = (ArrayList)Session["imprimir"];
                    if (usr != null)
                    {
                        String endereco = (usr.filial.diretorio_etiqueta);
                        if (Funcoes.existePasta(endereco))
                        {
                            if (!elgin)
                            {
                                endereco += "\\Etiq-" + Funcoes.RemoverAcentos(rdoEtiquetas.SelectedItem.Text.Replace("/", ".")) + "-" + DateTime.Now.ToString("yyyyMMdd-HH-mm-ss") + ".txt";
                            }
                            else
                            {
                                endereco += "\\ELGIN\\LINKPLUS.txt";
                            }

                            StreamWriter valor = new StreamWriter(endereco, false, Encoding.ASCII);

                            if (conteudo)
                            {
                                int nlinhaElginRow = 0;
                                foreach (ArrayList linha in imprimir)
                                {
                                    if (!elgin)
                                    {
                                        valor.Write(rdoEtiquetas.SelectedValue + "|");
                                        foreach (String cel in linha)
                                        {
                                            valor.Write(cel.Trim() + "|");
                                        }
                                        valor.WriteLine();
                                    }
                                    else
                                    {
                                        if (nlinhaElginRow > 0)
                                        {
                                            int nlinhaElgin = 0;
                                            string strplu = "";
                                            int elginQtde = 1;
                                            int.TryParse(linha[5].ToString(), out elginQtde);
                                            for (int i = 1; i <= elginQtde; i++)
                                            {
                                                foreach (String cel in linha)
                                                {
                                                    if (nlinhaElgin == 0)
                                                    {
                                                        strplu = cel.ToString();
                                                    }
                                                    valor.Write((nlinhaElgin == 1 && cel.ToString().Length == 0 ? strplu : cel.Trim().ToString()).Replace("\\","").Replace("/"," ").Replace(",", ".").Replace(";", " "));

                                                    if (nlinhaElgin == 3)
                                                    {
                                                        nlinhaElgin = 0;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        valor.Write(";");
                                                        nlinhaElgin++;
                                                    }
                                                }
                                                valor.WriteLine();
                                            }
                                        }
                                    }
                                    nlinhaElginRow++;
                                }

                                Session.Remove("imprimir");
                                Session.Add("imprimir", imprimir);
                            }
                            else
                            {
                                valor.Write("");
                            }
                            valor.Close();

                        }

                        lblPesquisaErro.Text += "Arquivo Gerado com Sucesso em " + endereco;
                        lblPesquisaErro.ForeColor = System.Drawing.Color.Blue;

                        //  ArrayList imprimir = (ArrayList)Session["imprimir"];
                        if (chkDesmarcarImprime.Checked)
                        {
                            if (imprimir != null)
                            {
                                foreach (ArrayList linha in imprimir)
                                {
                                    String sql = "";
                                    if (linha[0].ToString().IndexOf("F") >= 0)
                                        sql = " update familia set imprime_etiqueta=0 where codigo_familia ='" + linha[0].ToString().Substring(1).Trim() + "'";
                                    else
                                        sql = " update mercadoria set imprime_etiqueta =0 where plu ='" + linha[0].ToString() + "'";
                                    Conexao.executarSql(sql);
                                }


                            }


                        }

                        //Limpa Etiquetas marcadas com mais de 7 dias 
                        String SqlLimpaEt = "update mercadoria set  imprime_etiqueta = 0 where  Data_Alteracao <'" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "' and Imprime_etiqueta=1";
                        Conexao.executarSql(SqlLimpaEt);

                        String SqlLimpaFamilia = "update familia set  imprime_etiqueta = 0 where  codigo_familia in (select codigo_familia from mercadoria where Data_Alteracao <'" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "'and Codigo_familia is not null and Codigo_familia <>'' group by Codigo_familia) and familia.imprime_etiqueta=1  ";
                        Conexao.executarSql(SqlLimpaFamilia);

                    }


                }
            }
            catch (Exception err)
            {

                throw new Exception("Erro Arquivos de impressão: Detalhes :" + err.Message);
            }


        }

        protected void imgLista_Click(object sender, ImageClickEventArgs e)
        {

            ImageButton btn = (ImageButton)sender;
            Session.Remove("campoRecebe" + urlSessao());
            Session.Add("campoRecebe" + urlSessao(), btn.ID);


            TxtPesquisaLista.Text = "";

            exibeLista();
        }

        private void exibeLista()
        {

            lblErroPesquisa.Text = "";
            String strRecebe = (String)Session["campoRecebe" + urlSessao()];

            String sql = "";
            if (strRecebe.Equals("btnimg_txtFornecedor"))
            {
                lbltituloLista.Text = "ESCOLHA O FORNECEDOR";
                sql = "Select Fornecedor , Razao_Social ,CNPJ FROM FORNECEDOR  where (FORNECEDOR LIKE '%" + TxtPesquisaLista.Text + "%' or razao_social like '%" + TxtPesquisaLista.Text + "%' or  replace(replace(replace(cnpj,'.',''),'-',''),'/','') ='" + TxtPesquisaLista.Text + "') and filial ='MATRIZ'";
            }
            else if (strRecebe.Equals("imgBtnFamilia"))
            {
                lbltituloLista.Text = "ESCOLHA A FAMILIA";
                sql = "Select Codigo_familia,Descricao_Familia from Familia where codigo_familia like '%" + TxtPesquisaLista.Text + "%' or descricao_familia like '%" + TxtPesquisaLista.Text + "%'";
            }
            else if (strRecebe.Equals("ImgBtnGrupo"))
            {
                lbltituloLista.Text = "ESCOLHA O GRUPO";
                sql = "Select  Descricao_grupo as Grupo from grupo where codigo_grupo like '%" + TxtPesquisaLista.Text + "%' or descricao_grupo like '%" + TxtPesquisaLista.Text + "%'";

            }
            else if (strRecebe.Equals("ImgBtnSubGrupo"))
            {
                lbltituloLista.Text = "ESCOLHA A SubGrupo";
                sql = "Select Descricao_SubGrupo as SubGrupo, Descricao_Grupo as grupo  from SubGrupo inner join grupo on subGrupo.codigo_grupo = grupo.codigo_grupo" +
                    " where (codigo_SubGrupo like '%" + TxtPesquisaLista.Text + "%' or descricao_SubGrupo like '%" + TxtPesquisaLista.Text + "%')";

                if (txtGrupo.Text.Trim().Length > 0)
                    sql += " and grupo.descricao_grupo ='" + txtGrupo.Text + "'";

            }
            else if (strRecebe.Equals("ImgBtnDepartamento"))
            {
                lbltituloLista.Text = "ESCOLHA O DEPARTAMENTO";
                sql = "Select " +
                        " Descricao_departamento as Departamento " +
                        ",Descricao_subGrupo as SubGrupo " +
                        ",Descricao_grupo as Grupo" +
                    " from Departamento inner join subgrupo on departamento.codigo_subgrupo = subgrupo.codigo_subgrupo" +
                    "                   inner join grupo  on subgrupo.codigo_grupo = grupo.codigo_grupo " +
                    " where (codigo_departamento like '%" + TxtPesquisaLista.Text + "%' or descricao_Departamento like '%" + TxtPesquisaLista.Text + "%') ";

                if (txtGrupo.Text.Trim().Count() > 0)
                {
                    sql += " and grupo.descricao_grupo ='" + txtGrupo.Text + "'";
                }
                if (txtSubGrupo.Text.Trim().Count() > 0)
                {
                    sql += " and subgrupo.descricao_subgrupo = '" + txtSubGrupo.Text + "'";

                }

            }

            GridFornecedor.DataSource = Conexao.GetTable(sql, null, !TxtPesquisaLista.Text.Equals(""));
            GridFornecedor.DataBind();
            if (GridFornecedor.Rows.Count == 1)
            {
                if (!GridFornecedor.Rows[0].Cells[1].Text.Equals("------"))
                {
                    RadioButton rdo = (RadioButton)GridFornecedor.Rows[0].FindControl("RdoFornecedorItem");
                    rdo.Checked = true;
                }
            }
            modalListafornecedor.Show();

        }

        protected void GridLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoFornecedorItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridFornecedo.*GrFornecedorItem',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                User usr = (User)Session["User"];
                String strfiltros = "";
                String strJoins = "";
                String strSqlFinal = "";
                lblPesquisaFiltro.Text = "";
                lblPesquisaErro.Text = "";
                String sql = "";

                if (!txtfamilia.Text.Trim().Equals(""))
                {
                    strfiltros += " FAMILIA:" + txtfamilia.Text;

                    //strSqlFinal = "select 'F'+Codigo_familia ,'',Descricao_Familia,preco,0 from Familia where codigo_familia='" + txtfamilia.Text.Trim() + "'";
                    strSqlFinal = "SELECT TOP 1  PLU='F'+FAMILIA.Codigo_familia ,EAN='',DESCRICAO=FAMILIA.Descricao_Familia,FAMILIA.preco," +
                                       " CASE WHEN Mercadoria_loja.Data_Inicio<=CONVERT(VARCHAR,GETDATE(),102) AND mercadoria_loja.Data_Fim >= CONVERT(VARCHAR,GETDATE(),102) THEN mercadoria_loja.Preco_Promocao ELSE 0 END as Preco_Promocao   " +
                                      " FROM Mercadoria " +
                                                   " INNER JOIN Familia ON MERCADORIA.Codigo_familia = FAMILIA.Codigo_familia " +
                                                   " INNER JOIN Mercadoria_Loja ON MERCADORIA.PLU = Mercadoria_Loja.PLU AND Mercadoria_Loja.Filial ='" + usr.getFilial() + "' " +

                                      " WHERE MERCADORIA.codigo_familia='" + txtfamilia.Text.Trim() + "' and isnull(FAMILIA.Imprimir_Etiqueta_itens,0)<>1 " +
                                      " union all " +
                                      "select  distinct a.PLU,isnull(b.ean,'')EAN,a.Descricao, l.Preco," +
                                      " CASE WHEN l.Data_Inicio>=CONVERT(VARCHAR,GETDATE(),102) AND l.Data_Fim >= CONVERT(VARCHAR,GETDATE(),102) THEN l.Preco_Promocao ELSE 0 END as Preco_Promocao " +
                                        " from Mercadoria_Loja l inner join  mercadoria a on a.PLU = l.PLU left join ean b on a.plu=b.plu  inner join familia on a.codigo_familia = familia.codigo_familia " +
                                        " Where  a.codigo_familia='" + txtfamilia.Text.Trim() + "' and  FAMILIA.Imprimir_Etiqueta_itens=1  and l.filial='" + usr.getFilial() + "'"

                                      ;


                }
                else
                {


                    if (!txtPluEan.Text.Equals("")) //colocar nome do campo de pesquisa
                    {
                        if (Funcoes.isnumero(txtPluEan.Text.Trim()) && txtPluEan.Text.Trim().Length > 6)
                        {
                            sql = " (b.ean = '" + txtPluEan.Text.Trim() + "')";
                        }
                        else
                        {
                            sql = " (a.plu = '" + txtPluEan.Text + "')"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                        }

                        strfiltros = "PLU/EAN: " + txtPluEan.Text;
                    }

                    if (!txtReferencia.Text.Equals(""))
                    {
                        if (!sql.Equals(""))
                        {
                            sql += " and ";
                        }
                        sql += " a.Ref_fornecedor = '" + txtReferencia.Text + "'";
                        strfiltros += " REFERENCIA ";
                    }

                    if (chkImprimeEtiqueta.Checked || !txtDtalteracao.Text.Equals(""))
                    {
                        if (!sql.Equals(""))
                        {
                            sql += " and ";
                        }
                        sql += " a.imprime_etiqueta =1";
                        strfiltros += " SÓ ALTERADOS";
                    }
                    if (!txtDescricao.Text.Equals("")) //colocar nome do campo de pesquisa3
                    {
                        if (!sql.Equals(""))
                        {
                            sql += " and ";
                        }
                        sql += "a.descricao like '%" + txtDescricao.Text + "%'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                        strfiltros += " DESCRIÇÃO: " + txtDescricao.Text;
                    }
                    if (!txtNF.Text.Equals(""))
                    {
                        if (txtFornecedor.Text.Equals(""))
                        {
                            throw new Exception("Informe o Fornecedor para poder pesquisar Por Nota Fiscal!");
                        }

                        if (!sql.Equals(""))
                        {
                            sql += " and ";
                        }
                        strJoins = " left join nf_item c on a.plu=c.plu  left join nf  on c.cliente_fornecedor = nf.cliente_fornecedor AND nf.Codigo = c.Codigo AND NF.Filial = L.Filial ";
                        sql += " ltrim(rTrim(NF.codigo))= '" + txtNF.Text + "'AND NF.TIPO_NF=2 AND NF.FILIAL='" + usr.getFilial() + "' AND NF.CLIENTE_FORNECEDOR ='" + txtFornecedor.Text + "' ";
                        strfiltros += " NF: " + txtNF.Text;
                    }
                    if (!txtGrupo.Text.Trim().Equals(""))
                    {
                        if (!sql.Equals(""))
                        {
                            sql += " and ";
                        }

                        sql += " grupo.descricao_grupo='" + txtGrupo.Text + "'";
                    }
                    if (!txtSubGrupo.Text.Trim().Equals(""))
                    {
                        if (!sql.Equals(""))
                        {
                            sql += " and ";
                        }

                        sql += " subgrupo.descricao_subgrupo='" + txtSubGrupo.Text + "'";
                    }
                    if (!txtDepartamento.Text.Trim().Equals(""))
                    {
                        if (!sql.Equals(""))
                        {
                            sql += " and ";
                        }

                        sql += " departamento.descricao_departamento='" + txtDepartamento.Text + "'";
                    }


                    if (!txtDtCadastro.Text.Equals(""))
                    {
                        if (!sql.Equals(""))
                        {
                            sql += " and ";
                        }
                        sql += " a.data_cadastro = '" + DateTime.Parse(txtDtCadastro.Text).ToString("yyyy-MM-dd") + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                        strfiltros += " DATA CADASTRO: " + txtDtCadastro.Text;
                    }
                    if (!txtDtalteracao.Text.Equals(""))
                    {
                        if (!sql.Equals(""))
                        {
                            sql += " and ";
                        }
                        sql += " convert(varchar ,l.data_alteracao,102) = '" + DateTime.Parse(txtDtalteracao.Text).ToString("yyyy.MM.dd") + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                        strfiltros += " DATA ALTERÃÇÃO: " + txtDtalteracao.Text;
                    }
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += " l.Filial = '" + usr.getFilial() + "' ";


                    //Processo para não incluir INNER JOIN nos GRUPO, SUBGRUPO E DEPARTAMENTO
                    if (txtGrupo.Text.Trim().Equals("") && txtSubGrupo.Text.Trim().Equals("") && txtDepartamento.Text.Trim().Equals(""))
                    {
                        strSqlFinal = sqlGrid2 + strJoins + (!sql.Equals("") ? " where " + sql : "");
                    }
                    else
                    {
                        strSqlFinal = sqlGrid + strJoins + (!sql.Equals("") ? " where " + sql : "");
                    }


                    if (chkImprimeEtiqueta.Checked)
                        strSqlFinal += "union " +
                            //"select 'F'+Codigo_familia ,'',Descricao_Familia,preco,0 from Familia where imprime_etiqueta=1 ";
                            "SELECT TOP 1 'F'+FAMILIA.Codigo_familia ,'',FAMILIA.Descricao_Familia,FAMILIA.preco," +
                                       " CASE WHEN Mercadoria_loja.Data_Inicio<=CONVERT(VARCHAR,GETDATE(),102) AND Mercadoria_loja.Data_Fim >= CONVERT(VARCHAR,GETDATE(),102) THEN Mercadoria_loja.Preco_Promocao ELSE 0 END as Preco_Promocao  ," +
                                       "mercadoria.codigo_departamento as departamento  " +
                                       ",und" +
                                       " FROM Mercadoria " +
                                                   " INNER JOIN Familia ON MERCADORIA.Codigo_familia = FAMILIA.Codigo_familia " +
                                                   " INNER JOIN Mercadoria_Loja ON MERCADORIA.PLU = Mercadoria_Loja.PLU AND Mercadoria_Loja.Filial ='" + usr.getFilial() + "' " +
                                      "WHERE familia.imprime_etiqueta = 1 and isnull(FAMILIA.Imprimir_Etiqueta_itens,0)<>1 ";
                    if (!txtDtalteracao.Text.Equals(""))
                    {
                        strSqlFinal += "union " +
                               //"select 'F'+Codigo_familia ,'',Descricao_Familia,preco,0 from Familia where codigo_familia in(select codigo_familia from mercadoria where Data_Alteracao ='" + DateTime.Parse(txtDtalteracao.Text).ToString("yyyy-MM-dd") + "' and Codigo_familia is not null and Codigo_familia <>'' group by Codigo_familia) and familia.imprime_etiqueta=1  ";
                               "SELECT TOP 1 'F'+FAMILIA.Codigo_familia ,'',FAMILIA.Descricao_Familia,FAMILIA.preco," +
                                       " CASE WHEN Mercadoria_loja.Data_Inicio<=CONVERT(VARCHAR,GETDATE(),102) AND Mercadoria_loja.Data_Fim >= CONVERT(VARCHAR,GETDATE(),102) THEN Mercadoria_loja.Preco_Promocao ELSE 0 END as Preco_Promocao ," +
                                       "mercadoria.codigo_departamento as departamento  " +
                                       ",und " +
                                      " FROM Mercadoria " +
                                                   " INNER JOIN Familia ON MERCADORIA.Codigo_familia = FAMILIA.Codigo_familia " +
                                                   " INNER JOIN Mercadoria_Loja ON MERCADORIA.PLU = Mercadoria_Loja.PLU AND Mercadoria_Loja.Filial ='" + usr.getFilial() + "' " +
                                      "WHERE Mercadoria_Loja.Data_Alteracao ='" + DateTime.Parse(txtDtalteracao.Text).ToString("yyyy-MM-dd") + "' and MERCADORIA.Codigo_familia is not null and MERCADORIA.Codigo_familia <>'' and familia.imprime_etiqueta = 1 and isnull(FAMILIA.Imprimir_Etiqueta_itens,0)<>1";
                    }
                }
                GridLista.DataSource = Conexao.GetTable(strSqlFinal, usr, false);
                GridLista.DataBind();



                if (GridLista.Rows.Count > 1)
                {
                    modalEtiquetas.Show();
                }
                else
                {
                    if (GridLista.Rows.Count == 1 && !GridLista.Rows[0].Cells[1].Text.Replace("-", "").Trim().Equals(""))
                    {
                        incluirEtiqueta(GridLista.Rows[0].Cells[1].Text, GridLista.Rows[0].Cells[2].Text);
                        // atualizarArquivo(true);
                        ArrayList imprimir = (ArrayList)Session["imprimir"];
                        gridImpressao.DataSource = Conexao.GetArryTable(imprimir);
                        gridImpressao.DataBind();
                        gridImpressao.Visible = true;

                    }
                    else
                    {
                        throw new Exception("Mercadoria não encontrada!");
                    }

                }






                if (!txtNF.Text.Equals(""))
                { txtNF.Focus(); }
                txtNF.Text = "";

                if (!txtPluEan.Text.Equals(""))
                { txtPluEan.Focus(); }
                txtPluEan.Text = "";

                if (!txtReferencia.Text.Equals(""))
                {
                    txtReferencia.Focus();
                }
                txtReferencia.Text = "";

                if (!txtDescricao.Text.Equals(""))
                { txtDescricao.Focus(); }
                txtDescricao.Text = "";

                if (!txtDtalteracao.Text.Equals(""))
                { txtDtalteracao.Focus(); }
                txtDtalteracao.Text = "";

                if (!txtDtCadastro.Text.Equals(""))
                { txtDtCadastro.Focus(); }
                txtDtCadastro.Text = "";

                if (!txtFornecedor.Text.Equals(""))
                { txtFornecedor.Focus(); }
                txtFornecedor.Text = "";

                if (!txtfamilia.Text.Equals(""))
                { txtfamilia.Focus(); }
                txtfamilia.Text = "";

                if (strfiltros.Trim().Length > 0)
                {

                    lblPesquisaFiltro.Text = " FILTRO(S): " + strfiltros;
                }
            }
            catch (Exception err)
            {
                lblPesquisaErro.Text = err.Message;
                lblPesquisaErro.ForeColor = System.Drawing.Color.Red;
            }

        }
        protected override void btnEditar_Click(object sender, EventArgs e) { }
        protected override void btnExcluir_Click(object sender, EventArgs e) { }
        protected override void btnConfirmar_Click(object sender, EventArgs e) { }
        protected override void btnCancelar_Click(object sender, EventArgs e) { }


        protected void gridPesquisa_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridImpressao.DataSource = tb;
            gridImpressao.PageIndex = e.NewPageIndex;//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            gridImpressao.DataBind();

        }
        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected override bool campoDesabilitado(Control campo)
        {
            return false;
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gridImpressao.Rows)
            {
                CheckBox ch = (CheckBox)row.FindControl("CheckBox2");
                if (ch != null)
                {
                    ch.Checked = (sender as CheckBox).Checked;
                }
            }
        }

        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {

            foreach (GridViewRow item in GridLista.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                if (chk != null)
                {

                    chk.Checked = (sender as CheckBox).Checked;
                }
            }
            modalEtiquetas.Show();

        }
        private void incluirEtiqueta(String plu, String ean)
        {
            incluirEtiqueta(plu, ean, 1);
        }
        private void incluirEtiqueta(String plu, String ean, int qtd)
        {
            User usr = (User)Session["User"];
            ArrayList imprimir = (ArrayList)Session["imprimir"];

            ArrayList lsItem = new ArrayList();
            SqlDataReader item = null;
            String sqlConsulta = "";
            if (plu.IndexOf("F") >= 0)
            {
                //sqlConsulta = "select 'F'+Codigo_familia plu,'' ean,Descricao_Familia descricao,preco,0 preco_promocao,qtd_etiquetas from Familia where codigo_familia='" + plu.Substring(1) + "'";
                sqlConsulta = "SELECT TOP 1 'F'+FAMILIA.Codigo_familia plu ,'' ean,FAMILIA.Descricao_Familia descricao,FAMILIA.preco," +
                                       " CASE WHEN MERCADORIA_LOJA.Data_Inicio>=CONVERT(VARCHAR,GETDATE(),102) AND MERCADORIA_LOJA.Data_Fim >= CONVERT(VARCHAR,GETDATE(),102) THEN MERCADORIA_LOJA.Preco_Promocao ELSE 0 END as Preco_Promocao  , " +
                                       "isnull(familia.qtd_etiquetas,0) as qtd_etiquetas, '' departamento,mercadoria.und " +
                                      " FROM Mercadoria " +
                                                   " INNER JOIN Familia ON MERCADORIA.Codigo_familia = FAMILIA.Codigo_familia " +
                                                   " INNER JOIN Mercadoria_Loja ON MERCADORIA.PLU = Mercadoria_Loja.PLU AND Mercadoria_Loja.Filial ='" + usr.getFilial() + "' " +
                                      " WHERE MERCADORIA.codigo_familia='" + plu.Substring(1) + "'";

            }
            else
            {
                //Sistema incluirá apenas o produto cujo EAN foi digitado.
                String strWhere = "";
                if (Funcoes.lngTry(ean) > 999999)
                {
                    strWhere = " where b.ean='" + ean + "' and l.filial='" + usr.getFilial() + "'";

                }
                else
                {
                    strWhere = " where (a.plu=" + plu +  (ean.Trim().Equals("") ? "" : " or  b.ean='" + ean + "'") + ") and l.filial='" + usr.getFilial() + "'";
                }


                if (txtDepartamento.Text.Equals("") && txtGrupo.Text.Equals("") && txtSubGrupo.Text.Equals(""))
                {
                    sqlConsulta = sqlGrid2 + strWhere;
                }
                else
                {
                    sqlConsulta = sqlGrid + strWhere;
                }
            }
            item = Conexao.consulta(sqlConsulta, usr, false);
            bool existe = false;
            if (item.Read())
            {

                foreach (ArrayList arrI in imprimir)
                {
                    if (item["plu"].ToString().Equals(arrI[0].ToString()))
                        existe = true;
                }
                if (!existe)
                {
                    lsItem.Add(item["plu"].ToString());
                    lsItem.Add(item["ean"].ToString());
                    lsItem.Add(item["descricao"].ToString());
                    lsItem.Add(item["preco"].ToString());
                    lsItem.Add(item["preco_promocao"].ToString());
                    if (plu.IndexOf("F") >= 0)
                        lsItem.Add(item["qtd_etiquetas"].ToString());
                    else
                        lsItem.Add(qtd.ToString());
                    lsItem.Add(item["departamento"].ToString());
                    lsItem.Add(item["und"].ToString());

                    imprimir.Add(lsItem);
                    Session.Add("imprimir", imprimir);
                }
                //  gridImpressao.DataSource = Conexao.GetArryTable(imprimir);
                // gridImpressao.DataBind();
                // gridImpressao.Visible = true;
            }

            if (item != null)
                item.Close();

            OrdenarItens();
            //atualizarArquivo(true);
            if (existe)
            {
                lblPesquisaErro.Text = "mercadoria " + plu + " Já foi incluida Anteriormente";
                lblPesquisaErro.ForeColor = System.Drawing.Color.Red;
            }
            else
            {

                lblPesquisaErro.Text = "mercadoria " + plu + " incluida com sucesso ";
                lblPesquisaErro.ForeColor = System.Drawing.Color.Blue;
            }
        }

        private void OrdenarItens()
        {
            ArrayList imprimir = (ArrayList)Session["imprimir"];
            ArrayList Itens = new ArrayList();
            ArrayList cabecalho = new ArrayList();
            bool bCAb = true;
            foreach (ArrayList item in imprimir)
            {
                if (bCAb)
                {
                    cabecalho = item;
                    bCAb = false;
                }
                else
                {
                    Itens.Add(item);
                }

            }

            IComparer cp = new EtiquetasOrdem();
            Itens.Sort(cp);
            imprimir.Clear();
            imprimir.Add(cabecalho);
            foreach (ArrayList item in Itens)
            {
                imprimir.Add(item);
            }
            Session.Remove("imprimir");
            Session.Add("imprimir", imprimir);

        }


        protected void btnConfirmaFornecedor_Click(object sender, ImageClickEventArgs e)
        {

            String strRecebe = (String)Session["campoRecebe" + urlSessao()];

            String selecionado = ListaSelecionada();

            if (!selecionado.Equals("") && !selecionado.Equals("------"))
            {
                if (strRecebe.Equals("btnimg_txtFornecedor"))
                {
                    txtFornecedor.Text = ListaSelecionada();

                }
                else if (strRecebe.Equals("imgBtnFamilia"))
                {
                    txtfamilia.Text = ListaSelecionada();

                }
                else if (strRecebe.Equals("ImgBtnGrupo"))
                {
                    txtGrupo.Text = ListaSelecionada();
                }
                else if (strRecebe.Equals("ImgBtnSubGrupo"))
                {
                    txtSubGrupo.Text = ListaSelecionada();
                }
                else if (strRecebe.Equals("ImgBtnDepartamento"))
                {
                    txtDepartamento.Text = ListaSelecionada();
                }
                modalListafornecedor.Hide();
            }
            else
            {
                lblErroPesquisa.Text = "<br>Selecione Uma Opção";
                modalListafornecedor.Show();
            }
        }

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }


        protected void btnCancelaFornecedor_Click(object sender, ImageClickEventArgs e)
        {
            modalListafornecedor.Hide();

        }

        protected String ListaSelecionada()
        {
            foreach (GridViewRow item in GridFornecedor.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoFornecedorItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {
                        return item.Cells[1].Text;
                    }
                }
            }

            return "";
        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {

            ArrayList imprimir = (ArrayList)Session["imprimir"];
            for (int i = 0; i < GridLista.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)GridLista.Rows[i].FindControl("chkSelecionaItem");

                if (chk != null && chk.Checked)
                {
                    incluirEtiqueta(GridLista.Rows[i].Cells[1].Text, GridLista.Rows[i].Cells[2].Text);
                }
            }
            //atualizarArquivo(true);
            gridImpressao.DataSource = Conexao.GetArryTable(imprimir);
            gridImpressao.DataBind();
            gridImpressao.Visible = true;
            modalEtiquetas.Hide();

        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            GridLista.DataSource = null;
            GridLista.DataBind();
            modalEtiquetas.Hide();
        }

        protected void rdoEtiquetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImgEtiqueta.ImageUrl = "~\\modulos\\Dispositivos\\imgs\\etiqueta" + rdoEtiquetas.SelectedValue + ".jpg";
            //atualizarArquivo(true);

        }



        protected void ImgBtnLimpar_Click(object sender, ImageClickEventArgs e)
        {
            chkColetor.Checked = false;
            divArquivos.Visible = false;
            ddlArquivos.Items.Clear();
            ddlArquivos.Visible = false;
            lblIdentificarArquivo.Visible = true;
            ArrayList imprimir = (ArrayList)Session["imprimir"];
            imprimir.Clear();
            ArrayList cab = new ArrayList();
            cab.Add("PLU");
            cab.Add("EAN");
            cab.Add("DESCRICAO");
            cab.Add("preco");
            cab.Add("preco_promocao");
            cab.Add("Qtd");
            cab.Add("Departamento");
            cab.Add("Und");


            imprimir.Add(cab);
            Session.Remove("imprimir");
            Session.Add("imprimir", imprimir);
            gridImpressao.DataSource = Conexao.GetArryTable(imprimir);
            gridImpressao.DataBind();
            try
            {


                // atualizarArquivo(false);
            }
            catch (Exception err)
            {

                lblPesquisaErro.Text = err.Message;
            }
        }
        /*
        protected void imgImprimir_Click(object sender, ImageClickEventArgs e)
        {
            String endereco = Server.MapPath("")+ "\\impressao\\Imprimir.txt";
            StreamWriter valor = new StreamWriter(endereco, false, Encoding.ASCII);
            //StringBuilder valor = new StringBuilder();
            foreach(ArrayList linha in imprimir)
            {
                valor.Write(rdoEtiquetas.SelectedValue + "|");
                foreach (String cel in linha)
                {

                    valor.Write(cel + "|");
                }
                valor.WriteLine();
            }
            valor.Close();
            lblPesquisaErro.Text = "Arquivo de Impressão gerado com sucesso";
            Response.Redirect("baixar.aspx");
            

           // HttpExtensions.ForceDownload(Response, Server.MapPath("") + "\\impressao\\Imprimir.txt", "Imprimir.txt");
            
            
        }
        */

        protected void txtQtde_textChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.Parent.Parent;
            int index = row.RowIndex;
            AtualizaQtdeEtiqueta(index, int.Parse(txt.Text));
            txt.Focus();
        }

        protected void gridImpressao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ArrayList imprimir = (ArrayList)Session["imprimir"];
            if (imprimir.Count > 1)
            {
                int index = Convert.ToInt32(e.CommandArgument) + 1;
                ArrayList linha = (ArrayList)imprimir[index];

                if (e.CommandName == "mais")
                {
                    linha[5] = (int.Parse(linha[5].ToString()) + 1).ToString();
                }
                else if (e.CommandName == "menos" && !linha[5].ToString().Equals("0"))
                {
                    linha[5] = (int.Parse(linha[5].ToString()) + -1).ToString();

                }

                if (e.CommandName == "excluir")
                {
                    imprimir.RemoveAt(index);
                }
                else
                {
                    imprimir[index] = linha;
                }
                Session.Remove("imprimir");
                Session.Add("imprimir", imprimir);
                AtualizarGrid();


            }

        }

        private void AtualizarGrid()
        {
            ArrayList imprimir = (ArrayList)Session["imprimir"];
            gridImpressao.DataSource = Conexao.GetArryTable(imprimir);
            gridImpressao.DataBind();
        }

        private void AtualizaQtdeEtiqueta(int index, int qtde)
        {
            ArrayList imprimir = (ArrayList)Session["imprimir"];
            if (imprimir.Count > 1)
            {
                ArrayList linha = (ArrayList)imprimir[index + 1];

                linha[5] = qtde.ToString();

                Session.Remove("imprimir");
                Session.Add("imprimir", imprimir);

                gridImpressao.DataSource = Conexao.GetArryTable(imprimir);
                gridImpressao.DataBind();
            }
        }

        protected void chkDesmarcarImprime_CheckedChanged(object sender, EventArgs e)
        {

            if (chkDesmarcarImprime.Checked)
            {
                Session.Add("DesmarcaEtiqueta", "Sim");
            }
            else
            {
                Session.Remove("DesmarcaEtiqueta");
            }
        }

        protected void chkColetor_CheckedChanged(object sender, EventArgs e)
        {
            //btnBuscarArquivo.Enabled = chkColetor.Checked;
            //btnImportarArquivo.Enabled = chkColetor.Checked;

            divArquivos.Visible = chkColetor.Checked;
        }

        protected void imgBtnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            atualizarArquivo(true, (chkElgin.Checked ? true : false));

        }

        protected void imgBuscaArquivo_Click(object sender, ImageClickEventArgs e)
        {
            checaArquivosImportar();
        }
        protected void imgLeArquivo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Ler();
            }
            catch (Exception err)
            {
                lblErroPesquisa.Visible = true;
                lblErroPesquisa.Text = "Erro na importacao do arquivo :" + err.Message;
                lblErroPesquisa.ForeColor = System.Drawing.Color.Red;

            }

        }

        protected void btnBuscarArquivo_Click(object sender, EventArgs e)
        {
            checaArquivosImportar();

        }
        protected void checaArquivosImportar()
        {


            User usr = (User)Session["User"];
            if (usr != null)
            {
                //Limpar conteúdo do ListBox ltbArquivosParaImportacao
                ddlArquivos.Items.Clear();
                lblIdentificarArquivo.Visible = false;
                ddlArquivos.Visible = true;
                //Preenche ltbArquivosParaImportacao com conteudo do diretorio.
                String endereco = (usr.filial.ip.Equals("::1") ? "c:" : @"\\" + usr.filial.ip) + "\\Coletor";
                if (Funcoes.existePasta(endereco))
                {
                    string[] txtList = Directory.GetFiles(@endereco, "*.txt");
                    foreach (string f in txtList)
                    {
                        string strNomeArquivo = f.Substring(@endereco.Length + 1);
                        ddlArquivos.Items.Add(strNomeArquivo);
                    }

                    if (ddlArquivos.Items.Count <= 0)
                    {

                        lblIdentificarArquivo.Visible = true;
                        lblIdentificarArquivo.Text = "Sem Arquivos Identificados na pasta " + endereco;

                    }
                    else
                    {
                        lblIdentificarArquivo.Visible = false;
                    }

                }
                else
                {

                    lblIdentificarArquivo.Visible = true;
                    lblIdentificarArquivo.Text = "Não Encontrado o diretorio:" + endereco;

                }
            }
            //modalColetor.Show();
        }


        private void Ler()
        {


            Coletor coletor = new Coletor();
            //carregarDadosObj();
            //String arquivo = Server.MapPath("") + "\\importacao\\ArqImportado.txt";

            User usr = (User)Session["User"];
            String endereco = (usr.filial.ip.Equals("::1") ? "c:" : @"\\" + usr.filial.ip) + "\\Coletor";
            String arquivo = ddlArquivos.Text;

            String strSQL = "";
            arquivo = endereco + "\\" + arquivo;
            string[] lines = System.IO.File.ReadAllLines(arquivo);
            String strPlus = "";
            String contado = "";
            String plu = "";

            foreach (string line in lines)
            {
                if (line.Length > 0)
                {

                    if (coletor.coletorFixo)
                    {
                        plu = line.Substring(coletor.pluInicio, coletor.tamPlu);
                        contado = line.Substring(coletor.contadoInicio, coletor.tamContado);
                    }
                    else
                    {
                        string[] arrLinha = line.Split(coletor.delimitador[0]);
                        plu = arrLinha[coletor.pluInicio];
                        contado = arrLinha[coletor.contadoInicio];
                    }

                    incluirEtiqueta(plu, plu, int.Parse(contado));



                }
            }




            //carregarDados();
            //TabContainer1.ActiveTabIndex = 0;
            if (!Funcoes.existePasta(endereco + "//importado"))
            {
                System.IO.Directory.CreateDirectory(endereco + "//importado");
            }

            System.IO.File.Move(arquivo, endereco + "//importado//" + DateTime.Now.ToString("yyyy.MM.dd.hh.mm") + ddlArquivos.SelectedValue);
            checaArquivosImportar();
            ArrayList imprimir = (ArrayList)Session["imprimir"];
            gridImpressao.DataSource = Conexao.GetArryTable(imprimir);
            gridImpressao.DataBind();
            //gridImpressao.Visible = true;
        }

        protected void chkElgin_CheckedChanged(object sender, EventArgs e)
        {
            if (chkElgin.Checked == true)
            {
                rdoEtiquetas.Enabled = false;
            }
            else
            {
                rdoEtiquetas.Enabled = true;
            }
        }


        protected void bntImportar_Click(Object sender, EventArgs e)
        {
            SqlDataReader rs = null;
            try
            {
                int qtdeImportados = 0;
                rs = Conexao.consulta("Select * from etiqueta", null, false);
                while (rs.Read())
                {
                    qtdeImportados++;
                    incluirEtiqueta(rs["plu"].ToString(), "", Funcoes.intTry(rs["Quantidade"].ToString()));
                }
                lblQtdeImportado.Text = qtdeImportados.ToString();
                modalPnConfirma.Show();
            }
            catch (Exception err)
            {
                lblErroPesquisa.Text = err.Message;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
            AtualizarGrid();


        }
        protected void btnConfirmaLimparTabela_Click(object sender, ImageClickEventArgs e)
        {
            Conexao.executarSql("Delete from etiqueta");
            modalPnConfirma.Hide();
            pnImportar.Visible = false;
        }

        protected void btnNaoLimparTabela_Click(object sender, ImageClickEventArgs e)
        {
            modalPnConfirma.Hide();

        }
    }
}