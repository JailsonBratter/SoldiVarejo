using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class CTeEntradaDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            NF_CTeDAO obj = null;
            status = "sopesquisa";

            Session.Remove("User");
            Session.Add("User", usr);
            carregabtn(pnBtn);
            if (Request.Params["chave"] != null)
            {
                if (!IsPostBack)
                {
                    String chave = Request.Params["chave"].ToString();// colocar o campo index da tabela
                    obj = new NF_CTeDAO(chave, usr);
                    status = "sopesquisa";
                    Session.Remove("objCTeEntrada" + urlSessao());
                    Session.Add("objCTeEntrada" + urlSessao(), obj);
                    carregarDados();
                }
            }
            habilitarCampos(false);


        }

        private void habilitarCampos(bool enable)
        {
            EnabledControls(conteudo, enable);
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
        }

        protected override bool campoDesabilitado(Control campo)
        {
            return true;
        }

        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }
        protected void exibeLista(string campoLista = "")
        {
            lblErroPesquisa.Text = "";
            carregarDadosObj();
            String campo = "";

            if (campoLista.Equals(""))
            {
                campo = (String)Session["campoLista" + urlSessao()];
            }
            else
            {
                campo = campoLista;
            }


            String sqlLista = "";
            User usr = (User)Session["User"];

            switch (campo)
            {
                case "txtFornecedor_CNPJ":
                case "txtCTeCodigoFornecedor":
                    lbllista.Text = "Escolha um Fornecedor";
                    sqlLista = "select replace(replace(replace(cnpj,'.',''),'-',''),'/','') as CNPJ,Fornecedor from fornecedor where replace(replace(replace(cnpj,'.',''),'-',''),'/','') like '%" + TxtPesquisaLista.Text + "%' or FORNECEDOR like '%" + TxtPesquisaLista.Text + "%'  group by CNPJ,fornecedor";

                    break;
                case "txtCTeMunicipioOrigem":
                case "txtCTeMunicipioDestino":
                    lbllista.Text = "Escolha a cidade";
                    sqlLista = "SELECT munic AS CodigoIBGE, nome_munic AS Municipio from Unidade_Federacao ";
                    break;
            }

            GridLista.DataSource = Conexao.GetTable(sqlLista, null, true);
            GridLista.DataBind();
            if (lbllista.Text.Equals("Escolha um Produto"))
            {
                foreach (GridViewRow row in GridLista.Rows)
                {
                    if (row.Cells[5].Text.Equals("1"))
                        row.ForeColor = System.Drawing.Color.Red;
                }
                //GridLista.Columns[6].Visible = false;
            }
            if (GridLista.Rows.Count == 1)
            {
                if (!GridLista.Rows[0].Cells[1].Text.Equals("------"))
                {
                    RadioButton rdo = (RadioButton)GridLista.Rows[0].FindControl("RdoListaItem");
                    rdo.Checked = true;
                }
            }
            modalLista.Show();
            TxtPesquisaLista.Focus();
        }
        protected void imgLista_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;


            String or = btn.ID.Substring(7);

            Session.Remove("campoLista" + urlSessao());
            Session.Add("campoLista" + urlSessao(), or);
            TxtPesquisaLista.Text = "";
            exibeLista();
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void GridLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }



        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }

        protected String ListaSelecionada(int index)
        {
            foreach (GridViewRow item in GridLista.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoListaItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {
                        return item.Cells[index].Text;
                    }
                }
            }

            return "";
        }

        protected void btnFechar_Click(object sender, ImageClickEventArgs e)
        {
        }
        private void carregarDados()
        {
            NF_CTeDAO obj = (NF_CTeDAO)Session["objCTeEntrada" + urlSessao()];
            ddlCTeSitDOC.SelectedValue = obj.Situacao.ToString();
            txtCTeCodigoFornecedor.Text = obj.Fornecedor;
            txtCTeNumeroDocumento.Text = obj.Numero_Documento;
            txtCTeSerie.Text = obj.Serie.ToString();
            ddlTipoCTeBPe.SelectedValue = obj.Tipo_CTe.ToString();
            ddlTipoCTeFrete.SelectedValue = obj.Tipo_Frete.ToString();
            txtCTeEmissao.Text = obj.Emissao.ToString("dd/MM/yyyy");
            txtCTeAquisicao.Text = obj.Aquisicao.ToString("dd/MM/yyyy");
            txtCTeChave.Text = obj.Chave;
            txtCTeChaveSubstituicao.Text = obj.Chave_Substituicao;
            txtCTeBCICMS.Text = string.Format("{0:0,0.00}", obj.ICMS_Base);
            txtCTeReducao.Text = string.Format("{0:0,0.00}", obj.ICMS_Reducao);
            txtCTeAliquota.Text = string.Format("{0:0,0.00}", obj.ICMS_Aliquota);
            txtCTeValorICMS.Text = string.Format("{0:0,0.00}", obj.ICMS_Valor);
            txtCTeValorDocFiscal.Text = string.Format("{0:0,0.00}", obj.Valor_Documento);
            txtCTeValorNaoTributado.Text = string.Format("{0:0,0.00}", obj.Valor_Documento - obj.ICMS_Base);
            txtCTeValorTotalPrestServico.Text = string.Format("{0:0,0.00}", obj.Valor_Documento);
            txtCTeValorTotalDesconto.Text = string.Format("{0:0,0.00}", obj.Valor_Desconto);
            txtCTeMunicipioOrigem.Text = obj.IBGE_Origem.ToString();
            txtCTeMunicipioDestino.Text = obj.IBGE_Destino.ToString();

        }
        private void carregarDadosObj()
        {
        }
        protected void GridLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoListaItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridLista.*GrlistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }
    }
}