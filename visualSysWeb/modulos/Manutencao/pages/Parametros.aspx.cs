using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using visualSysWeb.modulos.Manutencao.dao;

namespace visualSysWeb.modulos.Manutencao.pages
{
    public partial class Parametros : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                carregarPesquisa();
        }

        private void carregarPesquisa()
        {
            String sql = "Select *, Str_Valor = substring(VALOR_ATUAL,1,30)  from parametros where parametro like '%" + txtFiltro.Text.Replace(' ', '%') + "%' order by ULT_ATUALIZACAO desc";
            gridParametros.DataSource = Conexao.GetTable(sql, null, false);
            gridParametros.DataBind();
            if (Session["PARAMETRO"] != null)
            {
                selecionaItem((ParametroDao)Session["PARAMETRO"]);
            }
                
        }

        private void selecionaItem(ParametroDao v)
        {
            foreach (GridViewRow row in gridParametros.Rows)
            {
                RadioButton rdo = (RadioButton)row.FindControl("rdoItem");
                if (row.Cells[1].Text.Equals(v.Parametro))
                {
                    rdo.Checked = true;
                    row.RowState = DataControlRowState.Selected;
                    rdo.Focus();
                    break;
                }

            }
        }

        protected void ImgBtnFiltrar_Click(object sender, ImageClickEventArgs e)
        {
            Session.Remove("PARAMETRO");
            carregarPesquisa();
        }

        protected void gridParametros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("rdoItem");

            if (rdo == null)
            {
                return;//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            }
            string script = "SetUniqueRadioButton('gridParametros.*GrlistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }

        protected void rdoItem_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rdo.Parent.Parent;
            row.RowState = DataControlRowState.Selected;
            rdo.Focus();
            ParametroDao parametro = new ParametroDao(row.Cells[1].Text);
            Session.Remove("PARAMETRO");
            Session.Add("PARAMETRO", parametro);
            lblParametro.Text = parametro.Parametro;
            txtDetalhes.Text = parametro.Descricao;
            if (parametro.Tipo_dado.Equals("L"))
            {
                ddlValorAtual.Visible = true;
                txtValorAtual.Visible = false;
                ddlValorAtual.Text = (parametro.Valor_Atual.Trim().Equals("TRUE")|| parametro.Valor_Atual.Trim().Equals(".T.")?"TRUE":"FALSE");
            }    
            else 
            {
                ddlValorAtual.Visible = false;
                txtValorAtual.Visible = true;
                txtValorAtual.Text = parametro.Valor_Atual.Trim();
                if(parametro.Tipo_dado.Equals("N") || parametro.Tipo_dado.Equals("D"))
                {
                    txtValorAtual.Attributes.Add("OnKeyPress", "javascript:return formataDouble(this,event);");
                }
                else
                {
                    txtValorAtual.Attributes.Remove("OnKeyPress");
                }
            }


        }

        protected void txtValorAtual_TextChanged(object sender, EventArgs e)
        {
            salvarParametro(txtValorAtual.Text.Trim());
        }

        protected void ddlValorAtual_SelectedIndexChanged(object sender, EventArgs e)
        {
            salvarParametro(ddlValorAtual.SelectedItem.Value);
        }

        protected void salvarParametro(String valorAtual)
        {
            ParametroDao Parametro = (ParametroDao)Session["PARAMETRO"];
            Parametro.Valor_Atual= valorAtual;
            Parametro.salvar();
            carregarPesquisa();
        }
    }
}