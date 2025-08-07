using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Drawing;
using System.Data.SqlClient;
using visualSysWeb.code;
using System.Data;

namespace visualSysWeb.modulos.usuarios.pages
{
    public partial class PermissoesDetalhes : visualSysWeb.code.PagePadrao
    {


        static String camporeceber = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];


            usuarios_webDAO obj = (usuarios_webDAO)Session["objUsuario" + urlSessao()];


            if (obj == null)
            {
                obj = new usuarios_webDAO();
            }
            if (usr != null)
            {
                usr.consultaTodasFiliais = true;
            }
            status = "editar";
            carregabtn(pnBtn);
            Menus menu = new Menus(usr, Server.MapPath(""));

            if (!IsPostBack && Request.Params["campoIndex"] != null)
            {
                String index = Request.Params["campoIndex"].ToString();// colocar o campo index da tabela

                obj = new usuarios_webDAO(index, usr);
                Session.Remove("objUsuario" + urlSessao());
                Session.Add("objUsuario" + urlSessao(), obj);
                carregarDados();
                menu.carregaPermissoes(PnModulos, obj, true);

            }
            else
            {
                menu.carregaPermissoes(PnModulos, obj, false);
            }
        }
        private void carregarDados()
        {
            usuarios_webDAO obj = (usuarios_webDAO)Session["objUsuario" + urlSessao()];
            txtId.Text = obj.id.ToString();
            txtusuario.Text = obj.usuario.ToString();
            txtnome.Text = obj.nome.ToString();

            txtfilial.Text = obj.filial.ToString();
            chkadm.Checked = obj.adm;

            abasTela(gridProdutos, "C003", "javascript:selecionarProduto(this);");
            abasTela(gridFornecedor, "C002", "javascript:selecionarFornecedor(this);");
            abasTela(gridCliente, "C001", "javascript:selecionarCliente(this);");
            abasTela(gridDepartamento, "C008", "javascript:selecionarDepartamento(this);");
            abasTela(gridPlanosDeContas, "C014", "javascript:selecionarPlanoDecontas(this);");


            abasTela(gridCadastrais, "R001", "javascript:selecionarRelCadastrais(this);");
            abasTela(gridVendas, "R002", "javascript:selecionarRelVendas(this);");
            abasTela(gridFinanceiro, "R003", "javascript:selecionarRelFinanceiro(this);");
            abasTela(gridFiscal, "R004", "javascript:selecionarRelFiscal(this);");
            abasTela(gridEstoque, "R005", "javascript:selecionarRelEstoque(this);");
            abasTela(gridComanda, "R007", "javascript:selecionarRelComanda(this);");

            



        }

        public void abasTela(GridView grid, String codTela, String jsScript)
        {
            usuarios_webDAO obj = (usuarios_webDAO)Session["objUsuario" + urlSessao()];
            String sql = "Select web_telas.item, acesso= isnull(wtp.acesso,0) " +
                                 " from web_telas " +
                                          " left join usuarios_web_telas_permissoes as wtp on web_telas.cod = wtp.cod  and web_telas.item = wtp.item and usuario = " + obj.id.ToString() +
                                 " Where web_telas.cod='" + codTela + "' order by ordem";
            grid.DataSource = Conexao.GetTable(sql, null, false);
            grid.DataBind();
            bool chkSelTodo = true;
            foreach (GridViewRow item in grid.Rows)
            {
                CheckBox chkTodos = (CheckBox)item.FindControl("chkSelecionaItem");
                if (!chkTodos.Checked)
                {
                    chkSelTodo = false;
                    break;
                }
            }


            if (grid.HeaderRow != null)
            {
                CheckBox chkTodos = (CheckBox)grid.HeaderRow.FindControl("chkSeleciona");
                if (chkTodos != null)
                {
                    chkTodos.Attributes.Remove("OnClick");
                    chkTodos.Attributes.Add("OnClick", jsScript);
                    chkTodos.Checked = chkSelTodo;

                }
            }

        }

        private void carregarDadosObj()
        {
            usuarios_webDAO obj = (usuarios_webDAO)Session["objUsuario" + urlSessao()];
            obj.id = (txtId.Text.Trim().Equals("") ? 0 : int.Parse(txtId.Text));
            obj.usuario = txtusuario.Text;
            obj.nome = txtnome.Text;

            obj.filial = txtfilial.Text;
            obj.adm = chkadm.Checked;

            Session.Remove("objUsuario" + urlSessao());
            Session.Add("objUsuario" + urlSessao(), obj);


        }



        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                usuarios_webDAO obj = (usuarios_webDAO)Session["objUsuario" + urlSessao()];
                carregarDadosObj();
                salvarPermissoes(PnModulos);
                User usr = (User)Session["User"];
                Menus menu = new Menus(usr, Server.MapPath(""));
                menu.carregaPermissoes(PnModulos, obj, true);

                salvarPermissoesAba("C001", gridCliente);
                salvarPermissoesAba("C002", gridFornecedor);
                salvarPermissoesAba("C003", gridProdutos);
                salvarPermissoesAba("C008", gridDepartamento);
                salvarPermissoesAba("C014", gridPlanosDeContas);


                salvarPermissoesAba("R001", gridCadastrais);
                salvarPermissoesAba("R002", gridVendas);
                salvarPermissoesAba("R003", gridFinanceiro);
                salvarPermissoesAba("R004", gridFiscal);
                salvarPermissoesAba("R005", gridEstoque);
                salvarPermissoesAba("R007", gridComanda);





                lblError.Text = "Salvo com sucesso em " + DateTime.Now.ToString();
                lblError.ForeColor = System.Drawing.Color.Blue;
                Session.Remove("objUsuario" + urlSessao());
                Session.Add("objUsuario" + urlSessao(), obj);
            }
            catch (Exception err)
            {

                lblError.Text = err.Message;
            }
        }

        public void salvarPermissoesAba(String CodTela, GridView grid)
        {
            usuarios_webDAO obj = (usuarios_webDAO)Session["objUsuario" + urlSessao()];
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {

                Conexao.executarSql("Delete from usuarios_web_telas_permissoes where cod='" + CodTela + "' and usuario =" + obj.id.ToString(), conn, tran);

                foreach (GridViewRow item in grid.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                    if (chk != null)
                    {
                        String sql = "insert into usuarios_web_telas_permissoes values ( "
                                       + "'" + CodTela + "'"
                                       + "," + obj.id.ToString()
                                       + ",'" + item.Cells[1].Text + "'"
                                       + "," + (chk.Checked ? "1" : "0")
                                   + ")";
                        Conexao.executarSql(sql, conn, tran);

                    }
                }

                tran.Commit();
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
            finally
            {

                if (conn != null)
                    conn.Close();
            }


        }


        public void salvarPermissoes(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is CheckBox)
                {
                    usuarios_webDAO obj = (usuarios_webDAO)Session["objUsuario" + urlSessao()];
                    String id = ((CheckBox)c).ID;
                    String tela = id.Substring(0, id.IndexOf("_"));
                    String nomeTela = id.Substring(id.IndexOf("_") + 1, (id.IndexOf("__") - (id.IndexOf("_") + 1)));
                    String permissao = id.Substring(id.IndexOf("__") + 2);
                    //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                    obj.atualizatela(tela, nomeTela, permissao, ((CheckBox)c).Checked);
                    Session.Remove("objUsuario" + urlSessao());
                    Session.Add("objUsuario" + urlSessao(), obj);
                    //((ImageButton)(c)).Visible = campo;
                }

                salvarPermissoes(c);
            }
        }



        protected override void btnCancelar_Click(object sender, EventArgs e)
        {

            Response.Redirect("permissoes.aspx");

        }


        protected override void btnEditar_Click(object sender, EventArgs e) { }
        protected override void btnExcluir_Click(object sender, EventArgs e) { }


        protected override void btnIncluir_Click(object sender, EventArgs e)
        {

        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {

        }

        protected override bool campoDesabilitado(Control campo)
        {
            return false;
        }

        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }
    }
}