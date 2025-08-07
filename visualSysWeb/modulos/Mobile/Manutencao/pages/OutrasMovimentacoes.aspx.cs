using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Manutencao.pages
{
    public partial class OutrasMovimentacoes : visualSysWeb.code.PagePadrao     //inicio da classe 
    {
        
        static String sqlGrid = "select codigo_inventario,descricao_inventario,data=convert(varchar,data,103),usuario,status,tipoMovimentacao from inventario";//colocar os campos no select que ser?o apresentados na tela
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User usr = (User)Session["User"];
                Conexao.preencherDDL(ddTipo, "Select Movimentacao='' ,Movimentacao1='' union Select Movimentacao ,Movimentacao1= Movimentacao from tipo_movimentacao  ", "Movimentacao", "Movimentacao1", usr);

                gridPesquisa.DataSource = Conexao.GetTable(sqlGrid +" order by CONVERT(varchar,data,102) desc", usr, true); 
                gridPesquisa.DataBind();

            }
            pesquisar(pnBtn);
            formatarCampos();
        }

        private void formatarCampos()
        {
            txtCodigo.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
            txtDataAte.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");
            txtDataDe.Attributes.Add("OnKeyPress", "javascript:return formataData(this,event);");


        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/modulos/Manutencao/pages/OutrasMovimentacoesDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            lblPesquisaErro.Text = "";
            String sql = "";
            try
            {

                if (!txtCodigo.Text.Equals("")) //colocar nome do campo de pesquisa
                {
                    sql = " codigo_inventario = '" + txtCodigo.Text + "'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                }
                if (!txtDescricao.Text.Equals("")) //colocar nome do campo de pesquisa2
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += "Descricao_inventario like '" + txtDescricao.Text + "%'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                }
                if (!txtDataDe.Text.Equals(""))
                {
                    if (IsDate(txtDataDe.Text) && IsDate(txtDataAte.Text))
                    {
                        if (!sql.Equals(""))
                        {
                            sql += " and ";
                        }
                        sql += "Data >='" + DateTime.Parse(txtDataDe.Text).ToString("yyyy-MM-dd") + "' and Data <= '" + DateTime.Parse(txtDataAte.Text).ToString("yyyy-MM-dd") + "'";
                    }
                    else
                    {
                        throw new Exception("Datas Invalidas"); 
                    }
                }

                if (!ddTipo.Text.Equals(""))
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += " TipoMovimentacao = '"+ddTipo.Text+"'";
                }

                if (!txtPLU.Text.Equals(""))
                {
                    if (!sql.Equals(""))
                    {
                        sql += " and ";
                    }
                    sql += " Inventario.Codigo_Inventario in (select inventario_itens.Codigo_Inventario from inventario_itens where inventario_itens.plu = '" + txtPLU.Text + "')";
                }

                User usr = (User)Session["User"];
                DataTable tb;
                if (!sql.Equals(""))
                {
                    tb = Conexao.GetTable(sqlGrid + " where " + sql +" order by CONVERT(varchar,data,102) desc", usr, false);
                }
                else
                {
                    tb = Conexao.GetTable(sqlGrid +" order by CONVERT(varchar,data,102) desc", usr, false);
                }
                gridPesquisa.DataSource = tb;
                gridPesquisa.DataBind();
                
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