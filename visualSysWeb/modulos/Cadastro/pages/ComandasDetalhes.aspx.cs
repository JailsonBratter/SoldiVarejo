using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class ComandasDetalhes : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];

            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    ComandaDAO obj = new ComandaDAO();
                    status = "incluir";
                    Session.Remove("comanda" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("comanda" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                    EnabledControls(conteudo, true);
                    //EnabledControls(cabecalho, true);
                }
            }
            else
            {
                if (Request.Params["campoIndex"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            String index = Request.Params["campoIndex"].ToString();// colocar o campo index da tabela
                            status = "visualizar";
                            ComandaDAO obj = new ComandaDAO(index, usr);
                            Session.Remove("comanda" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                            Session.Add("comanda" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                            carregarDados();
                        }
                        if (status.Equals("visualizar"))
                        {
                            EnabledControls(conteudo, false);
                            //EnabledControls(cabecalho, false);
                        }
                        else
                        {
                            EnabledControls(conteudo, true);
                            //EnabledControls(cabecalho, true);
                        }
                    }
                    catch (Exception err)
                    {
                        lblError.Text = err.Message;
                    }
                }
            } 
            
            if(ddlStatus.Text.Equals("4"))
                carregabtn(pnBtn, null, null, null, null, "Desbloquear Comanda", null);
            else
                carregabtn(pnBtn, null, null, null, null, "Bloquear Comanda", null);

            txtComanda.Attributes.Add("OnKeyPress", "javascript:return formataInteiro(this,event);");
        }

        private void limparCampos()
        {
            //LimparCampos(cabecalho);
            LimparCampos(conteudo);
        }

        protected bool validaCamposObrigatorios()
        {
            if (validaCampos(conteudo))
                return true;
            else
                return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "txtComanda", 
                                    "txtObservao", 
                                    "chkStatus", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("ComandasDetalhes.aspx?novo=true");
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            lblError.Text = "NÃO É PERMITIDO ALTERAÇÕES";
            /*

            editar(pnBtn);
            EnabledControls(conteudo, true);
             */
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Comandas.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {

            if (ddlStatus.Text.Equals("4"))
                Label14.Text = "Tem Certeza que gostaria de Desbloquear a COMANDA?";
            else
                Label14.Text = "Tem Certeza que gostaria de Bloquear a COMANDA?";

                modalExluirComanda.Show();//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    ComandaDAO obj = (ComandaDAO)Session["comanda" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
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
            Response.Redirect("Comandas.aspx");//colocar endereco pagina de pesquisa
        }

        //--Atualizar DaoForm 
        private void carregarDados()
        {
            ComandaDAO obj = (ComandaDAO)Session["comanda" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            txtComanda.Text = string.Format("{0:0000}", obj.Comanda);
            txtOBSERVACAO.Text = (obj.Observacao == null ? "" : obj.Observacao.ToString());

            ddlStatus.SelectedValue = obj.Status.ToString();
            User usr = (User)Session["User"];
            gridItensComanda.DataSource = Conexao.GetTable("select  i.plu,m.descricao,i.origem,i.usuario,data=convert(varchar,i.data,103),hora = convert(varchar,i.hora_evento,108),i.qtde,i.unitario,i.total from Comanda_item  i WITH (INDEX(index_comanda_cupom)) inner join mercadoria m on i.plu=m.PLU" +
                                                            " WHERE i.comanda=" + obj.Comanda + " and i.cupom=0 ", usr, false);
            gridItensComanda.DataBind();

        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            ComandaDAO obj = (ComandaDAO)Session["comanda" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
            obj.Comanda = Decimal.Parse(txtComanda.Text);
            obj.Observacao = txtOBSERVACAO.Text;
            obj.Status = int.Parse(ddlStatus.SelectedValue);
            Session.Remove("comanda" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("comanda" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
        }


        protected void lista_click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                User usr = (User) Session["user"];
                ComandaDAO obj = (ComandaDAO)Session["comanda" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "")];
                
                 if(obj.Status == 4)
                {
                    String sqlItens = "select  count(*) from Comanda_item  i WITH (INDEX(index_comanda_cupom)) " +
                                " WHERE i.comanda=" + obj.Comanda + " and i.cupom=0 ";

                    String valor = Conexao.retornaUmValor(sqlItens, usr);
                    if (int.Parse(valor) > 0)
                    {
                        obj.Status = 2;
                        obj.excluir("02");
                    }
                    else
                    {
                        obj.Status = 0;
                        obj.excluir("00");
                    }

                   
                    lblError.Text = "Comanda Desbloqueada com sucesso";
                    Session.Remove("comanda" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
                    Session.Add("comanda" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), obj);
                    carregarDados();

                }
                else
                {
                    obj.excluir("04");
                    lblError.Text = "Comanda Bloqueada com sucesso";
                    limparCampos();
                    Session.Remove("comanda" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));

                }
                pesquisar(pnBtn);
                modalExluirComanda.Hide();
                
            }
            catch (Exception err)
            {
                lblError.Text = "Não foi possivel Excluir o registro error:" + err.Message;
                modalExluirComanda.Show();

            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalExluirComanda.Hide();
        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            pnFundo.Visible = false;
        }

        protected void gridItensComanda_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gridItensComanda.FooterRow;
            Decimal total = 0;
                foreach (GridViewRow item in gridItensComanda.Rows)
                {
                    if (!item.Cells[8].Text.Equals("------"))
                        total += Decimal.Parse(item.Cells[8].Text);

                }
            
            if (footer != null)
            {
                footer.Cells[7].Text = "TOTAL:";
                footer.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                footer.Cells[8].Text = total.ToString("N2");
                footer.Cells[8].HorizontalAlign = HorizontalAlign.Right;

            }

        }

    }
}