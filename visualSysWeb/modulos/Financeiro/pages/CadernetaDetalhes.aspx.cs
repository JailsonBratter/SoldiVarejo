using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;


namespace visualSysWeb.modulos.Financeiro.pages
{
    public partial class CadernetaDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (!IsPostBack)
            {
                status = "sopesquisa";
                if (Request["campoIndex"] != null)
                {
                    String codCliente = Request.Params["campoIndex"];

                    if (!IsPostBack)
                    {
                        String ndias = Funcoes.valorParametro("DIAS_CADERNETA", usr);
                        if (ndias.Equals(""))
                            txtDe.Text = DateTime.Now.AddDays(-120).ToString("dd/MM/yyyy");
                        else
                            txtDe.Text = DateTime.Now.AddDays(int.Parse("-" + ndias)).ToString("dd/MM/yyyy");

                        txtAte.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    carregarDados();
                    habilitar(true);
                }
            }


            carregabtn(pnBtn, null, null, null, null, null, "Voltar");
            //EnabledControls(conteudo, true);

        }

        private void limparCampos()
        {
            LimparCampos(cabecalho);
            LimparCampos(conteudo);
        }

        protected bool validaCamposObrigatorios()
        {
            if (validaCampos(cabecalho) && validaCampos(conteudo))
                return true;
            else
                return false;
        }

        private void habilitar(bool enable)
        {
            EnabledControls(pnItens, enable);
            EnabledControls(cabecalho, enable);
            addItens.Visible = enable;

        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            String[] campos = { "txtCodigo_Cliente", 
                                    "txtNomeCliente", 
                                    "txtSituacao", 
                                    "txtCnpj" ,
                                    "txtUtilizado",
                                    "txtLimite",
                                    "txtUtilizadoContaReceber",
                                    "txtTotalCardeneta"
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            incluir(pnBtn);
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            EnabledControls(cabecalho, true);
            EnabledControls(conteudo, true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Caderneta.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {

        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    //       obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    EnabledControls(cabecalho, false);
                    EnabledControls(conteudo, false);
                    visualizar(pnBtn);
                }
                else
                {
                    lblError.Text = "Campo Obrigatorio não preenchido";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Caderneta.aspx");//colocar endereco pagina de pesquisa
        }
        //--Atualizar DaoForm 
        private void carregarDados()
        {
            try
            {
                if (Request.Params["campoIndex"] != null)
                {
                    User usr = (User)Session["User"];
                    String codCliente = Request.Params["campoIndex"];
                    ClienteDAO cliente = new ClienteDAO(codCliente, usr);
                    txtCodigo_Cliente.Text = cliente.Codigo_Cliente;
                    txtNomeCliente.Text = cliente.Nome_Cliente;
                    txtSituacao.Text = cliente.Situacao;
                    txtCnpj.Text = cliente.CNPJ;
                    txtLimite.Text = cliente.Limite_Credito.ToString("N2");
                    decimal vlrUtilizadoContasAReceber = cliente.totalUtilizado();
                    decimal saldoCaderneta = cliente.cadernetaSaldo();
                    txtTotalCardeneta.Text = saldoCaderneta.ToString("N2"); 
                    txtUtilizadoContaReceber.Text = vlrUtilizadoContasAReceber.ToString("N2");
                    txtUtilizado.Text = (vlrUtilizadoContasAReceber+saldoCaderneta).ToString("N2");

                    String Sql = "Select Emissao_Caderneta =CONVERT(varchar,emissao_caderneta,103),Tipo,Documento_Caderneta,Historico_Caderneta,Total_Caderneta,Caixa_Caderneta,lancamento,usuario,data_inclusao=CONVERT(varchar,data_inclusao,103) from Caderneta with( index(IX_ID_Caderneta))   where codigo_cliente ='" + txtCodigo_Cliente.Text.Trim() + "'";
                    if (!ddlTipoFiltro.SelectedValue.Equals(""))
                    {
                        Sql += " and tipo ='" + ddlTipoFiltro.SelectedValue + "' ";
                    }

                    if (!txtDe.Text.Equals(""))
                    {
                        Sql += " and convert(dateTime ,convert(varchar,emissao_caderneta,102)) between '" + DateTime.Parse(txtDe.Text).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(txtAte.Text).ToString("yyyy-MM-dd") + "'";
                    }


                    gridCaderneta.DataSource = Conexao.GetTable(Sql + " order by CONVERT(varchar,emissao_caderneta,102) desc", usr, false);
                    gridCaderneta.DataBind();

                    foreach (GridViewRow item in gridCaderneta.Rows)
                    {
                        if (item.Cells[2].Text.Substring(0, 1).Equals("C"))
                        {

                            //ImageButton img = (ImageButton)item.Cells[0].Controls[0];
                            //if (img != null)
                            //{
                            //    img.ImageUrl = "~/Images/icon2.png";
                            //}
                            ////((ImageButton)item.Cells[0].Controls[0]).ImageUrl = "";
                            item.ForeColor= System.Drawing.Color.Red;
                            
                        }
                         
                    }
                }

            }
            catch (Exception err)
            {
                lblError.Text = err.Message;
            }



        }



        // --Atualizar FormDao 
        private cadernetaDAO carregarDadosObj()
        {
            User usr = (User)Session["User"];
            cadernetaDAO caderneta = new cadernetaDAO();
            caderneta.Emissao_Caderneta = DateTime.Parse(txtEmissao.Text);
            caderneta.Codigo_Cliente = txtCodigo_Cliente.Text;
            caderneta.Tipo = ddlTipo.Text;
            caderneta.Documento_Caderneta = txtDocumento.Text;
            caderneta.Historico_Caderneta = txtHistorico.Text;
            caderneta.Total_Caderneta = Decimal.Parse(txtTotal.Text);
            caderneta.Caixa_Caderneta = int.Parse((txtCaixa.Text.Equals("") ? "0" : txtCaixa.Text));
            caderneta.lancamento = txtLancamento.Text;
            caderneta.filial = usr.getFilial();
            caderneta.usuario = usr.getNome();
            caderneta.data_inclusao = DateTime.Now;
            return caderneta;
        }


        protected void lista_click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            String sqlLista = "";

            switch (btn.ID)
            {
                case "idBotao":
                    sqlLista = "Query de pesquisa com no minimo 2campos";
                    break;
            }
            User usr = (User)Session["User"];
        }

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {
                lblError.Text = "Não foi possivel Excluir o registro error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            //pnConfima.Visible = false;
        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void ImgBtnAddItens_Click(object sender, ImageClickEventArgs e)
        {
            LimparCampos(pnItensFrame);
            txtEmissao.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ModalItens.Show();

        }

        protected void validaCampos()
        {
            txtEmissao.BackColor = System.Drawing.Color.White;
            txtTotal.BackColor = System.Drawing.Color.White;
            txtCaixa.BackColor = System.Drawing.Color.White;


            try
            {
                DateTime.Parse(txtEmissao.Text);
            }
            catch (Exception)
            {
                txtEmissao.Focus();
                txtEmissao.BackColor = System.Drawing.Color.Red;

                throw new Exception("DATA DE EMISSAO INVALIDA");

            }

            try
            {
                Decimal.Parse(txtTotal.Text);

            }
            catch (Exception)
            {
                txtTotal.Focus();
                txtTotal.BackColor = System.Drawing.Color.Red;

                throw new Exception("VALOR TOTAL INVALIDO!");
            }

            try
            {
                if (!txtCaixa.Text.Equals(""))
                {
                    int.Parse(txtCaixa.Text);
                }
            }
            catch (Exception)
            {
                txtCaixa.Focus();
                txtCaixa.BackColor = System.Drawing.Color.Red;
                throw new Exception("CAIXA INVALIDO");
            }



        }
        protected void btnConfirmaItens_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                cadernetaDAO caderneta = carregarDadosObj();
                caderneta.salvar(true);
                carregarDados();
            }
            catch (Exception err)
            {

                lblErrorItens.Text = "Erro :" + err.Message;
                ModalItens.Show();
            }

        }
        protected void btnCancelaItem_Click(object sender, ImageClickEventArgs e)
        {
            ModalItens.Hide();
        }
        protected void btnConfirmaExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                User usr = (User)Session["User"];
                cadernetaDAO cad = new cadernetaDAO();
                cad.filial = usr.getFilial();
                cad.Codigo_Cliente = txtCodigo_Cliente.Text;
                cad.Emissao_Caderneta = DateTime.Parse(lblEmissao.Text);
                cad.Documento_Caderneta = lblDocumento.Text;
                cad.Tipo = lblTipo.Text;
                cad.Historico_Caderneta = lblHistorico.Text;
                cad.Total_Caderneta = decimal.Parse(lbltotal.Text);
                cad.Caixa_Caderneta = (lblCaixa.Text.Equals("") ? -1 : int.Parse(lblCaixa.Text));
                cad.lancamento = lblLancamento.Text;

                cad.excluir(usr);
                lblError.Text = "Registro Excluido com Sucesso ";
                lblError.ForeColor = System.Drawing.Color.Blue;
                carregarDados();
            }
            catch (Exception err)
            {

                lblError.Text = "Erro:" + err.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }


        }

        protected void btnCancelaExcluir_Click(object sender, ImageClickEventArgs e)
        {
            ModalConfirmaExlusao.Hide();
        }


        protected void gridCaderneta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            lblEmissao.Text = gridCaderneta.Rows[index].Cells[0].Text;
            lblTipo.Text = gridCaderneta.Rows[index].Cells[1].Text;
            lblDocumento.Text = gridCaderneta.Rows[index].Cells[2].Text;
            lblHistorico.Text = gridCaderneta.Rows[index].Cells[3].Text;
            lbltotal.Text = gridCaderneta.Rows[index].Cells[4].Text;
            lblCaixa.Text = gridCaderneta.Rows[index].Cells[5].Text;
            lblLancamento.Text = gridCaderneta.Rows[index].Cells[6].Text;
            ModalConfirmaExlusao.Show();

        }

        protected void txtDe_TextChanged(object sender, EventArgs e)
        {
            if (txtAte.Text.Equals(""))
                txtAte.Text = txtDe.Text;

            carregarDados();
        }

        protected void imgBtnFiltrar_Click(object sender, ImageClickEventArgs e)
        {
            carregarDados();
        }

        protected void imgBtnImpressao_Click(object sender, ImageClickEventArgs e)
        {

            String Tipo = (ddlTipoFiltro.SelectedValue.Equals("") ? "" : "&tipo=" + ddlTipoFiltro.SelectedValue);
            String de = (txtDe.Text.Equals("") ? "" : "&de=" + txtDe.Text);
            String ate = (txtAte.Text.Equals("") ? "" : "&ate=" + txtAte.Text);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refdopen", "window.open('CadernetaPrint.aspx?" +
                                                                                                    "codCliente=" + txtCodigo_Cliente.Text.Trim() +
                                                                                                    de +
                                                                                                    ate +
                                                                                                    Tipo +
                                                                                                    "','_blank');", true);
        }

        protected void lnkReceber_Click(object sender, EventArgs e)
        {
            RedirectNovaAba("~/modulos/financeiro/pages/ContaReceber.aspx?codigo_cliente=" + txtCodigo_Cliente.Text + "&tela=FN002");
        }
    }

}