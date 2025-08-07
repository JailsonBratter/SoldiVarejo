using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;
using System.Data.SqlClient;
using visualSysWeb.modulos.Cadastro.dao;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class FuncionarioDetalhes : visualSysWeb.code.PagePadrao
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Conexao.preencherDDL(ddlFuncao, "Select Funcao from Funcao");
            }
            User usr = (User)Session["User"];
            if (Request.Params["novo"] != null)
            {
                if (!IsPostBack)
                {
                    funcionarioDAO obj = new funcionarioDAO(usr);
                    Session.Remove("funcionario" + urlSessao());
                    Session.Add("funcionario" + urlSessao(), obj);
                    status = "incluir";
                    habilitarCampos(true);
                }
            }
            else
            {
                if (Request.Params["codigo"] != null && Request.Params["nome"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            String codigo = Request.Params["codigo"].ToString();
                            String nome = Request.Params["nome"].ToString();
                            status = "visualizar";
                            funcionarioDAO obj = new funcionarioDAO(nome,codigo, usr);
                            Session.Remove("funcionario" + urlSessao());
                            Session.Add("funcionario" + urlSessao(), obj);
                   
                            carregarDados();
                        }
                        if (status.Equals("visualizar"))
                        {
                            habilitarCampos(false);
                        }
                        else
                        {
                            habilitarCampos(true);
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
        private void habilitarCampos(bool enable)
        {
            EnabledControls(conteudo, enable);
            EnabledControls(cabecalho, enable);
           
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
            String[] campos = { "txtNome", 
                                    "ddlFuncao", 
                                    "", 
                                    "" 
                                     };
            return existeNoArray(campos, campo.ID + "");
        }

        protected override bool campoDesabilitado(Control campo)
        {// colocar os nomes dos campos Desabilitados no Array
            if (campo.ID == null)
                return false;

            String[] campos = { "txtcodigo", 
                                    "", 
                                    "", 
                                    "" 
                                     };
            if (!status.Equals("incluir"))
            {
                if (campo.ID.Equals("txtNome"))
                    return true;
            }
            return existeNoArray(campos, campo.ID + "");
        }
        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FuncionarioDetalhes.aspx?novo=true"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            editar(pnBtn);
            carregarDados();
            habilitarCampos(true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Funcionario.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            modalExcluir.Show();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();
                    funcionarioDAO obj = (funcionarioDAO)Session["funcionario" + urlSessao()];

                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    Session.Remove("funcionario" + urlSessao());
                    Session.Add("funcionario" + urlSessao(), obj);
                    visualizar(pnBtn);
                    
                    carregarDados();
                    habilitarCampos(false);

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
            Response.Redirect("Funcionario.aspx");//colocar endereco pagina de pesquisa
        }
        private void carregarDados()
        {
            try
            {

            
            funcionarioDAO obj = (funcionarioDAO)Session["funcionario" + urlSessao()];
            txtNome.Text = obj.Nome.ToString();
            ddlFuncao.Text = obj.Funcao.Trim().ToString();
            txtInicio.Text = obj.Inicio.ToString();
            txtFim.Text = obj.Fim.ToString();
            txtComissao.Text = obj.Comissao.ToString("N2");
            chkusa_palm.Checked = obj.usa_palm;
            chkcancela_item.Checked = obj.cancela_item;
            txtsp_mercadoria_plu.Text = obj.sp_mercadoria_plu.ToString();
            txtsp_mercadoria_dpto.Text = obj.sp_mercadoria_dpto.ToString();
            txtsp_mercadoria_descricao.Text = obj.sp_mercadoria_descricao.ToString();
            txtsp_grupo.Text = obj.sp_grupo.ToString();
            txtsp_sgrupo.Text = obj.sp_sgrupo.ToString();
            txtsp_departamento.Text = obj.sp_departamento.ToString();
            txtsenha.Text = obj.senha.ToString();
            txtcodigo.Text = obj.codigo.ToString();
            txtpraca.Text = obj.praca.ToString();
            txtNome2.Text = obj.Nome2.ToString();
            txtEndereco.Text = obj.Endereco.ToString();
            txtCONTA.Text = obj.CONTA.ToString();
            txtbanco.Text = obj.banco.ToString();
            txtAgencia.Text = obj.Agencia.ToString();
            txtNome_correntista.Text = obj.Nome_correntista.ToString();
            txtcidade.Text = obj.cidade.ToString();
            txtestado.Text = obj.estado.ToString();
            txttelefone.Text = obj.telefone.ToString();
            txtdata_nascimento.Text = obj.data_nascimentoBr();
            txtsobrenome.Text = obj.sobrenome.ToString();
            txtcep.Text = obj.cep.ToString();
            txtcelular.Text = obj.celular.ToString();
            txtbairro.Text = obj.bairro.ToString();
            chkUtiliza_agenda.Checked = obj.utiliza_agenda;
            chkUsa_Terminal.Checked = obj.Usa_Terminal;
            gridDepartamentos.DataSource = obj.metas;
            gridDepartamentos.DataBind();
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
            }
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            funcionarioDAO obj = (funcionarioDAO)Session["funcionario" + urlSessao()];

            obj.Nome = txtNome.Text;
            obj.Funcao = ddlFuncao.Text;
            obj.Inicio = txtInicio.Text;
            obj.Fim = txtFim.Text;
            obj.Comissao = (txtComissao.Text.Equals("")?0: Decimal.Parse(txtComissao.Text));
            obj.usa_palm = chkusa_palm.Checked;
            obj.cancela_item = chkcancela_item.Checked;
            obj.sp_mercadoria_plu = txtsp_mercadoria_plu.Text;
            obj.sp_mercadoria_dpto = txtsp_mercadoria_dpto.Text;
            obj.sp_mercadoria_descricao = txtsp_mercadoria_descricao.Text;
            obj.sp_grupo = txtsp_grupo.Text;
            obj.sp_sgrupo = txtsp_sgrupo.Text;
            obj.sp_departamento = txtsp_departamento.Text;
            obj.senha = txtsenha.Text;
            obj.codigo = txtcodigo.Text;
            obj.praca = txtpraca.Text;
            obj.Nome2 = txtNome2.Text;
            obj.Endereco = txtEndereco.Text;
            obj.CONTA = txtCONTA.Text;
            obj.banco = txtbanco.Text;
            obj.Agencia = txtAgencia.Text;
            obj.Nome_correntista = txtNome_correntista.Text;
            obj.cidade = txtcidade.Text;
            obj.estado = txtestado.Text;
            obj.telefone = txttelefone.Text;
            obj.data_nascimento =(txtdata_nascimento.Text.Equals("")?new DateTime(): DateTime.Parse(txtdata_nascimento.Text));
            obj.sobrenome = txtsobrenome.Text;
            obj.cep = txtcep.Text;
            obj.celular = txtcelular.Text;
            obj.bairro = txtbairro.Text;
            obj.utiliza_agenda = chkUtiliza_agenda.Checked;
            obj.Usa_Terminal = chkUsa_Terminal.Checked;

            foreach (GridViewRow row in gridDepartamentos.Rows)
            {
                TextBox txtMeta = (TextBox)row.FindControl("txtMeta");
                Funcionario_metasDAO meta =  obj.metas.Find(i => i.Codigo_departamento.Equals(row.Cells[1].Text));
                meta.Meta = Funcoes.decTry(txtMeta.Text);
            }

            Session.Remove("funcionario" + urlSessao());
            Session.Add("funcionario" + urlSessao(), obj);
                   
        }


        protected void lista_click(object sender, ImageClickEventArgs e)
        {
            /*
            ImageButton btn = (ImageButton)sender;
            String sqlLista = "";

            switch (btn.ID)
            {
                case "idBotao":
                    sqlLista = "Query de pesquisa com no minimo 2campos";
                    //lbllista.Text = "Pagamentos";
                    camporeceber = "txtPagamento";
                    break;
            }
            User usr = (User)Session["User"];
            SqlDataReader lista = Conexao.consulta(sqlLista, usr);

            while (lista.Read())
            {
                ListItem item = new ListItem();
                item.Value = lista[0].ToString();
                item.Text = lista[1].ToString();
                chkLista.Items.Add(item);
            }
            if (lista != null)
                lista.Close();
             * 
             */
        }

        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                funcionarioDAO obj = (funcionarioDAO)Session["funcionario" + urlSessao()];
                obj.excluir();
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                pesquisar(pnBtn);
                modalExcluir.Hide();
            }
            catch (Exception err)
            {
                lblError.Text = "Não foi possivel Excluir o registro error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            modalExcluir.Hide();
        }

        protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
        {
            funcionarioDAO obj = (funcionarioDAO)Session["funcionario" + urlSessao()];
            String campo = (String)Session["campoLista" + urlSessao()];
            if(campo.Equals("Departamento"))
            {
                Funcionario_metasDAO meta = new Funcionario_metasDAO() {
                    Codigo_departamento=ListaSelecionada(1),
                    Descricao_departamento=ListaSelecionada(4)
                };
                obj.addMeta(meta);
            }
            carregarDados();
            modalPnFundo.Hide();
        }

        protected String ListaSelecionada(int campo)
        {
            foreach (GridViewRow item in GridLista.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoListaItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {
                        return item.Cells[campo].Text;
                    }
                }
            }

            return "";
        }


        protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
        {
            modalPnFundo.Hide();
        }

        protected void gridDepartamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            lblDepartamentoExcluir.Text = gridDepartamentos.Rows[index].Cells[1].Text;
            lblDescricaoDepartamento.Text = gridDepartamentos.Rows[index].Cells[2].Text;
            ModalExcluirDepartamento.Show();
        }

        protected void imgBtnExcluirDepartamento_Click(object sender, ImageClickEventArgs e)
        {
            funcionarioDAO obj = (funcionarioDAO)Session["funcionario" + urlSessao()];
            obj.metas.RemoveAll(i => i.Codigo_departamento.Equals(lblDepartamentoExcluir.Text));
            carregarDados();
            ModalExcluirDepartamento.Hide();
        }
        

        protected void imgBtnCancelaExcluirDepartamento_Click(object sender, ImageClickEventArgs e)
        {
            ModalExcluirDepartamento.Hide();
        }


        protected void exibeLista()
        {
            lblErroPesquisa.Text = "";
            String campo = (String)Session["campoLista" +urlSessao()];
            String sqlLista = "";

            switch (campo)
            {
              
                case "Departamento":
                    lbllista.Text = "Escolha o Departamento";
                    sqlLista = "Select   cod= codigo_departamento," +
                                       " Grupo= Descricao_grupo," +
                                       " SubGrupo = descricao_subgrupo," +
                                       " Departamento=descricao_departamento " +

                                   " from W_BR_CADASTRO_DEPARTAMENTO " +
                                   " where  " +
                                       " codigo_departamento Like '%" + TxtPesquisaLista.Text.Trim() + "%'" +
                                       " or descricao_grupo like '%" + TxtPesquisaLista.Text.Trim() + "%'" +
                                       " or descricao_subgrupo like '%" + TxtPesquisaLista.Text.Trim() + "%'" +
                                       " or descricao_departamento like '%" + TxtPesquisaLista.Text.Trim() + "%'";
                    break;
              
            }
            User usr = (User)Session["User"];
            GridLista.DataSource = Conexao.GetTable(sqlLista, usr, true);
            GridLista.DataBind();
            if (GridLista.Rows.Count == 1)
            {
                if (!GridLista.Rows[0].Cells[1].Text.Equals("------"))
                {
                    RadioButton rdo = (RadioButton)GridLista.Rows[0].FindControl("RdoListaItem");
                    rdo.Checked = true;
                }
            }

            modalPnFundo.Show();
            TxtPesquisaLista.Focus();
        }

        protected void imgAddDepartamento_Click(object sender, ImageClickEventArgs e)
        {
            carregarDadosObj();
            Session.Remove("campoLista" + urlSessao());
            Session.Add("campoLista" + urlSessao(), "Departamento");
            exibeLista();
        }

        protected void ImgPesquisaLista_Click(object sender, ImageClickEventArgs e)
        {
            exibeLista();
        }
        protected void GridLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoListaItem");

            if (rdo == null)
            {
                return;//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
            }
            string script = "SetUniqueRadioButton('GridLista.*GrlistaItem',this)";
            rdo.Attributes.Add("onclick", script);
        }
    }
}