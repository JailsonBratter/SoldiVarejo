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
    public partial class SpoolImpressoraDetalhes : visualSysWeb.code.PagePadrao
    {
        protected static Spool_impressorasDAO obj = null;
        static String camporeceber = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            obj = new Spool_impressorasDAO(usr);
            
            if (Request.Params["novo"] != null)
            {
                status = "incluir";
                EnabledControls(conteudo, true);
                EnabledControls(cabecalho, true);
            }
            else
            {
                if (Request.Params["idTrm"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            String index = Request.Params["idTrm"].ToString();// colocar o campo index da tabela
                            status = "visualizar";
                            obj = new Spool_impressorasDAO(int.Parse(index), usr);
                            carregarDados();
                        }
                        if (status.Equals("visualizar"))
                        {
                            EnabledControls(conteudo, false);
                            EnabledControls(cabecalho, false);
                        }
                        else
                        {
                            EnabledControls(conteudo, true);
                            EnabledControls(cabecalho, true);
                        }
                    }
                    catch (Exception err)
                    {
                        lblError.Text = err.Message;
                    }
                }
            }
            carregabtn(pnBtn);
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

        protected override bool campoObrigatorio(Control campo)
        {// colocar os nomes dos campos obrigarios no Array
            String[] campos = { "txtid_trm", 
                                    "", 
                                    "", 
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
            Response.Redirect("SpoolImpressora.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            //pnConfima.Visible = true;
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    EnabledControls(cabecalho, false);
                    EnabledControls(conteudo, false);
                    visualizar(pnBtn);
                }
                else
                {
                    lblError.Text = "Campo Obrigatorio n?o preenchido";
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
            Response.Redirect("SpoolImpressora.aspx");//colocar endereco pagina de pesquisa
        }
     
        //--Atualizar DaoForm 
        private void carregarDados()
        {
            txtid_trm.Text = obj.id_trm.ToString();
            txtimpressoraRemota.Text = obj.impressora_remota.ToString();
            txtDescricao.Text = obj.Descricao.ToString();
            txtativo.Text = obj.Ativo.ToString();
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            obj.id_trm = int.Parse(txtid_trm.Text);
            obj.impressora_remota = int.Parse(txtimpressoraRemota.Text);
            obj.Descricao = txtDescricao.Text;
            obj.Ativo = int.Parse(txtativo.Text);
        }


        //protected void lista_click(object sender, ImageClickEventArgs e)
        //{
        //    ImageButton btn = (ImageButton)sender;
        //    pnFundo.Visible = true;
        //    chkLista.Items.Clear();
        //    String sqlLista = "";

        //    switch (btn.ID)
        //    {
        //        case "idBotao":
        //            sqlLista = "Query de pesquisa com no minimo 2campos";
        //            lbllista.Text = "Pagamentos";
        //            camporeceber = "txtPagamento";
        //            break;
        //    }
        //    User usr = (User)Session["User"];
        //    SqlDataReader lista = Conexao.consulta(sqlLista, usr);

        //    while (lista.Read())
        //    {
        //        ListItem item = new ListItem();
        //        item.Value = lista[0].ToString();
        //        item.Text = lista[1].ToString();
        //        chkLista.Items.Add(item);
        //    }
        //    if (lista != null)
        //        lista.Close();
        //}

        //protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        obj.excluir();
        //        pnConfima.Visible = false;
        //        lblError.Text = "Registro Excluido com sucesso";
        //        limparCampos();
        //        pesquisar(pnBtn);
        //    }
        //    catch (Exception err)
        //    {
        //        lblError.Text = "N?o foi possivel Excluir o registro error:" + err.Message;
        //    }
        //}

        //protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        //{
        //    pnConfima.Visible = false;
        //}

        //protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        //{
        //    TextBox txt = (TextBox)conteudo.FindControl(camporeceber);
        //    txt.Text = "";
        //    for (int i = 0; i < chkLista.Items.Count; i++)
        //    {
        //        if (chkLista.Items[i].Selected)
        //        {
        //            txt.Text += chkLista.Items[i].Value;
        //        }
        //    }
        //    pnFundo.Visible = false;
        //}

        //protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        //{
        //    pnFundo.Visible = false;
        //}
                  
                  
    }
}