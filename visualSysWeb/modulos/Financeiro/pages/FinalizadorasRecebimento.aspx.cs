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

namespace visualSysWeb.modulos.Financeiro.pages
{
    public partial class FinalizadorasRecebimento : visualSysWeb.code.PagePadrao
    {
        int fechamento = 0;
        int pdv = 0;
        int operador = 0;
        decimal valor = 0;
        decimal valorRecebido = 0;
        int sequencia = 0;

        List<AlteracaoFinalizadoraDAO> finalizadoras;
        protected void Page_Load(object sender, EventArgs e)
        {

            int.TryParse(Request.Params["fechamento"], out fechamento);
            int.TryParse(Request.Params["pdv"], out pdv);
            int.TryParse(Request.Params["operador"], out operador);

            User usr = (User)Session["User"];

            if (!IsPostBack)
            {
                pesquisar();
                finalizadoras = new List<AlteracaoFinalizadoraDAO>();

                Session.Remove("objAlteraFin" + urlSessao());
                Session.Add("objAlteraFin" + urlSessao(), finalizadoras);

                status = "editar";
                Conexao.preencherDDL1Branco(ddlFinalizadora, "Select finalizadora,Nro_finalizadora from Finalizadora", "finalizadora", "Nro_finalizadora", usr);
            }
            else
            {
                finalizadoras = (List<AlteracaoFinalizadoraDAO>)Session["objAlteraFin" + urlSessao()];
            }
            status = "sopesquisa";
            carregabtn(pnBtn);
        }

        protected void pesquisar()
        {
            User usr = (User)Session["User"];
            if (usr != null)
            {
                string sql = "select cupom, hora_Venda, ISNULL(sequencia, 0) As Sequencia, finalizadora, id_finalizadora, total, id_cartao, autorizacao, id_bandeira, ";
                sql += " rede_cartao from tesouraria_detalhes WHERE";
                sql += " Filial = '" + usr.getFilial() +"' AND ";
                sql += " Id_Fechamento = " + fechamento.ToString() + " AND";
                sql += " Operador = " + operador.ToString() + " AND";
                sql += " PDV = " + pdv.ToString();

                gridFinalizadora.DataSource = Conexao.GetTable(sql, usr, false);
                gridFinalizadora.DataBind();
            }
        }

        protected override bool campoDesabilitado(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override bool campoObrigatorio(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "fechar", "fecharJanela();", true);
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
        protected void ImgBtnVoltar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
            decimal valorValida = 0;

            finalizadoras = (List<AlteracaoFinalizadoraDAO>)Session["objAlteraFin" + urlSessao()];
            OrigemFinalizadoraDAO origem = (OrigemFinalizadoraDAO)Session["dadosFinalizadoraOrigem" + urlSessao()];

            foreach (AlteracaoFinalizadoraDAO row in finalizadoras)
            {
                valorValida += row.valor;
            }

            decimal.TryParse(lblValor.Text, out valor);

            if (valorValida != valor)
            {
                lblErro.Text = "A soma dos valores não podem ser diferente do valor pago.";
                lblErro.ForeColor = System.Drawing.Color.Red;
                lblErro.Visible = true;
                modalPnFundo.Show();
                return;
            }

            if (!origem.salvar(finalizadoras))
            {
                lblError.Text = "Erro ao tentar SALVAR";
                lblError.Visible = true;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                modalPnFundo.Hide();
                pesquisar();
            }

        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            finalizadoras = (List<AlteracaoFinalizadoraDAO>)Session["objAlteraFin" + urlSessao()];
            finalizadoras.Clear();
            ddlAutorizadora.Items.Clear();
            ddlCartao.Items.Clear();
            txtCodAutorizacao.Text = "";
            txtValor.Text = "";
            lblErro.Text = "";
            modalPnFundo.Hide();
        }

        protected void imgBtnCartao_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Show();
        }

        protected void ddlFinalizadora_SelectedIndexChanged(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            DropDownList ddlFinalizadora = (DropDownList)sender;
            carregarDadosCartao(ddlFinalizadora.SelectedItem.Text, "");
        }

        protected void ddlAutorizadora_SelectedIndexChanged(object sender, EventArgs e)
        {
            carregarDadosCartao(ddlFinalizadora.SelectedItem.Text, ddlAutorizadora.SelectedItem.Text);
        }

        protected void gridFinalizadora_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("editar"))
            {
                User usr = (User)Session["User"];
                string dataEmissao = Conexao.retornaUmValor("SELECT TOP 1 Emissao  FROM Tesouraria_Detalhes with (index=ix_Filial_Op_Fec_Fin) WHERE FILIAL = '" + usr.getFilial() + "' AND Operador = " + operador.ToString() + " AND id_fechamento = " + fechamento.ToString() + " AND PDV = " + pdv.ToString(), usr);

                int index = Convert.ToInt32(e.CommandArgument);
                finalizadoras = new List<AlteracaoFinalizadoraDAO>();

                Conexao.preencherDDL1Branco(ddlFinalizadora, "SELECT finalizadora, Nro_finalizadora FROM Finalizadora", "finalizadora", "Nro_finalizadora", usr);

                GridLista.DataSource = finalizadoras;
                GridLista.DataBind();

                lblValorFinalizadora.Text = "";

                valor = Funcoes.decTry(gridFinalizadora.Rows[index].Cells[5].Text);
                lblValor.Text = valor.ToString("N2");

                OrigemFinalizadoraDAO origem = new OrigemFinalizadoraDAO();
                origem.emissao = Convert.ToDateTime(dataEmissao);
                origem.hora = gridFinalizadora.Rows[index].Cells[1].Text;
                origem.cupom = gridFinalizadora.Rows[index].Cells[0].Text;
                origem.sequencia = int.Parse(gridFinalizadora.Rows[index].Cells[2].Text);
                origem.codFinalizadora = int.Parse(gridFinalizadora.Rows[index].Cells[3].Text);
                origem.totalFinalizadora = valor;
                //Dados passado por parametros
                origem.pdv = pdv;
                origem.operador = operador;
                origem.fechamento = fechamento;


                Session.Remove("dadosFinalizadoraOrigem" + urlSessao());
                Session.Add("dadosFinalizadoraOrigem" + urlSessao(), origem);

                modalPnFundo.Show();
            }
        }

        private void carregarDadosCartao(string finalizadora, String autorizadora)
        {
            User usr = (User)Session["User"];
            if (ddlFinalizadora.Items.Count == 0)
                Conexao.preencherDDL1Branco(ddlFinalizadora, "Select finalizadora,Nro_finalizadora from Finalizadora", "finalizadora", "Nro_finalizadora", usr);

            Funcoes.SetDDLText(ddlFinalizadora, finalizadora);
            ddlFinalizadora.Visible = true;
            String sqlCartao = "Select id_cartao  " +
                               "from Cartao inner join Finalizadora on Cartao.nro_Finalizadora = Finalizadora.Nro_Finalizadora " +
                               "where cartao.nro_finalizadora = '" + ddlFinalizadora.SelectedItem.Value + "'";

            Conexao.preencherDDL1Branco(ddlAutorizadora, "Select * from Autorizadora", "Descricao", "id", null);
            if (autorizadora.Equals(""))
            {
                autorizadora = Conexao.retornaUmValor("Select descricao from autorizadora where padrao =1", null);
            }
            Funcoes.SetDDLText(ddlAutorizadora, autorizadora);
            ddlAutorizadora.Visible = true;

            if (!autorizadora.Equals(""))
                sqlCartao += " and id_rede=" + ddlAutorizadora.SelectedItem.Value;

            Conexao.preencherDDL1Branco(ddlCartao, sqlCartao + " order by id_cartao ", "id_cartao", "id_cartao", null);

            if (ddlCartao.Items.Count > 1)
            {
                //Funcoes.SetDDLValue(ddlCartao, lblCartao.Text);
                ddlCartao.Visible = true;
                ddlAutorizadora.Visible = true;
                txtCodAutorizacao.Visible = true;
            }
            else
            {
                ddlAutorizadora.Visible = false;
                ddlAutorizadora.SelectedItem.Text  = "";
                txtCodAutorizacao.Text = "";
                txtCodAutorizacao.Visible = false;
                ddlCartao.Visible = false;
            }

            modalPnFundo.Show();
            ddlFinalizadora.Focus();
        }

        protected void btnAdicionar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ddlAutorizadora.SelectedItem.Text.Equals(""))
                {

                }
            }
            catch
            {
                ddlAutorizadora.Items.Clear();
                ddlAutorizadora.Items.Add("");
            }

            //Se selecionou cartão, é necessário validar os demais campos
            if (!ddlAutorizadora.SelectedItem.Text.Equals(""))
            {
                if (ddlCartao.SelectedItem.Text.Equals("") || txtCodAutorizacao.Text.Trim().Equals(""))
                {
                    lblErro.Text = "Obrigatório selecionar CARTÃO e CODIGO DE AUTORIZAÇÃO.";
                    lblErro.ForeColor = System.Drawing.Color.Red;
                    lblErro.Visible = true;
                    modalPnFundo.Show();
                }
            }
            decimal.TryParse(lblValor.Text, out valor);
            //Valida valor
            if ( (valor +(Funcoes.decTry(lblValorFinalizadora.Text)) > (Funcoes.decTry(txtValor.Text)) ))
            {
                lblErro.Text = "A totalidade do valor deve ser maior ou igual que 0 e inferior ao valor da finalizadora:" + lblValor.Text;
                lblErro.ForeColor = System.Drawing.Color.Red;
                lblErro.Visible = true;
                modalPnFundo.Show();
            }

            if (finalizadoras == null)
            {
                finalizadoras = new List<AlteracaoFinalizadoraDAO>();
            }
            else
            {
                finalizadoras = (List<AlteracaoFinalizadoraDAO>)Session["objAlteraFin" + urlSessao()];
            }

            finalizadoras.Add(new AlteracaoFinalizadoraDAO
            {
                finalizadora = ddlFinalizadora.SelectedItem.Text,
                autorizadora = ddlAutorizadora.SelectedItem.Text,
                cartao = ddlCartao.Text,
                codigoAutorizacao = txtCodAutorizacao.Text,
                valor = Funcoes.decTry(txtValor.Text)
            });

            Session.Remove("objAlteraFin" + urlSessao());
            Session.Add("objAlteraFin" + urlSessao(), finalizadoras);

            //Limpar dados
            //ddlFinalizadora.SelectedItem.Text = "";
            ddlFinalizadora.SelectedIndex = 0;
            ddlAutorizadora.SelectedItem.Text = "";
            ddlCartao.SelectedItem.Text = "";
            txtCodAutorizacao.Text = "";
            valorRecebido += Funcoes.decTry(txtValor.Text);
            lblValorFinalizadora.Text = valorRecebido.ToString();
            txtValor.Text = "";


            GridLista.DataSource = finalizadoras;
            GridLista.DataBind();
            modalPnFundo.Show();
        }

        protected void GridLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                if (index >= 0)
                {
                    finalizadoras.RemoveAt(index);
                }

                GridLista.DataSource = finalizadoras;
                GridLista.DataBind();
                modalPnFundo.Hide();
                modalPnFundo.Show();
            }
        }
    }
}