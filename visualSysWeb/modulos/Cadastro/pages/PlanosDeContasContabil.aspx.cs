using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;
using visualSysWeb.modulos.Cadastro.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class PlanoContasContabil : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
             
                carregarDados();
                EnabledControls(PnDetalhes, true);
                User usr = (User)Session["User"];
                if (usr == null)
                    return;

                if (Request.Params["tela"] != null)
                {
                    usr.tela = Request.Params["tela"].ToString();
                }
              
            }



        }

        protected void carregarDados()
        {
            String sql = "Select * from plano_de_contas_contabil ";
            User usr = (User)Session["User"];
            SqlDataReader rs = null;
            List<PlanoDeContasContabilDAO> PLANOS = new List<PlanoDeContasContabilDAO>();
            try
            {
                rs = Conexao.consulta(sql, usr, false);
                while (rs.Read())
                {
                    PlanoDeContasContabilDAO plano = new PlanoDeContasContabilDAO()
                    {
                        id = rs["id"].ToString(),
                        codigoPlanoPai = rs["codigo_plano_pai"].ToString(),
                        codigo = rs["codigo"].ToString(),
                        descricao = rs["descricao"].ToString()
                    };
                    PLANOS.Add(plano);
                }
            }
            catch (Exception err)
            {
                showMessage(err.Message, true);
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
            Session.Remove("listaPlanos" + urlSessao());
            Session.Add("listaPlanos" + urlSessao(),PLANOS);

            carregarNiveis(GridNivel1,"0");
            String codigo1 = selecionado(GridNivel1, 2);
            carregarNiveis(GridNivel2, codigo1);
            String codigo2 = selecionado(GridNivel2, 2);
            carregarNiveis(GridNivel3, codigo2);
            String codigo3 = selecionado(GridNivel3, 2);
            carregarNiveis(GridNivel4, codigo3);
            String codigo4 = selecionado(GridNivel4, 2);
            carregarNiveis(GridNivel5, codigo4);

        }
        protected void carregarNiveis(GridView grid, string codigoPai)
        {
            List<PlanoDeContasContabilDAO> PLANOS = (List<PlanoDeContasContabilDAO>)Session["listaPlanos" + urlSessao()];
            grid.DataSource = PLANOS.FindAll(p => p.codigoPlanoPai.Equals(codigoPai));
            grid.DataBind();
            selecionarItem(grid, "",  true);
        }



        protected String selecionado(GridView grid, int campo)
        {
            foreach (GridViewRow item in grid.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoItem");

                if (rdo != null)
                {
                    if (rdo.Checked)
                    {
                        rdo.Focus();
                        return item.Cells[campo].Text;
                    }
                }
            }

            return "";
        }

        protected void focus(String select, GridView grid)
        {
           
            foreach (GridViewRow item in grid.Rows)
            {
                RadioButton rdo = (RadioButton)item.FindControl("RdoItem");
                if (rdo != null)
                {
                    if ( item.Cells[1].Text.Equals(select))
                    {

                        item.RowState = DataControlRowState.Selected;
                        rdo.Checked = true;
                        rdo.Focus();
                        break;
                    }
                    else
                    {

                        if (rdo.Checked)
                        {
                            item.RowState = DataControlRowState.Selected;
                            rdo.Focus();
                            break;
                        }
                    }
                }
            }
        }
      
       
        

       
        public bool selecionarItem(GridView grid, String select, bool primeiro)
        {
            if (grid.Rows.Count > 0)
            {
                String strSelec = (String)Session[select];
                if (strSelec == null)
                {

                    int index = 0;
                    if (!primeiro)
                    {
                        index = grid.Rows.Count - 1;
                    }
                    RadioButton rdo = (RadioButton)grid.Rows[index].FindControl("rdoItem");
                    if (rdo != null)
                    {
                        grid.Rows[index].RowState = DataControlRowState.Selected;
                        rdo.Checked = true;
                    }
                    return true;
                }
                else
                {
                    focus(select, grid);
                    Session.Remove(select);
                }
            }
            return false;

        }

        public void showDetalhes(GridView grid,String codigoPai)
        {
            String id = selecionado(grid, 1);
            String codigo = selecionado(grid, 2);
            String descricao = selecionado(grid, 3);

            txtCodigoPlanoPaiDetalhe.Text = codigoPai;
            if (codigoPai.Equals(""))
            {
                divCodigoPaiDetalhes.Visible = false;
            }
            else
            {
                divCodigoPaiDetalhes.Visible = true;   
            }
            
            txtCodigoDetalhe.Text = codigo;
            txtIdDetalhe.Text = id;
            txtDescricaoDetalhe.Text = descricao;
            modalDetalhe.Show();

        }
        public void addNovoDetalhe(string codigoPai,string novo)
        {
            status = "incluir";
            
            if (codigoPai.Equals(""))
            {
                txtCodigoPlanoPaiDetalhe.Text = "0";
                divCodigoPaiDetalhes.Visible = false;
            }
            else
            {
                txtCodigoPlanoPaiDetalhe.Text = codigoPai;
                divCodigoPaiDetalhes.Visible = true;
            }

            txtCodigoDetalhe.Text = novo;
            txtIdDetalhe.Text = "";
            txtDescricaoDetalhe.Text = "";
            modalDetalhe.Show();
            txtCodigoDetalhe.Focus();
        }
       
        protected void ImgBtnNivel1Editar_Click(object sender, EventArgs e)
        {
            status = "editar";
            showDetalhes(GridNivel1, "");
        }

        protected void imgBtnNivel1Excluir_Click(object sender, EventArgs e)
        {
            lblExcluirTitle.Text = selecionado(GridNivel1, 1);
            modalExcluirPlano.Show();
        }

        protected string proximo(GridView grid,int num,string codPai)
        {
            if(grid.Rows.Count == 0)
            {
                return codPai+"001";
            }
            String prx = grid.Rows[grid.Rows.Count - 1].Cells[2].Text;
            if (!prx.Equals("------"))
            {
                return (int.Parse(prx) + 1).ToString().PadLeft(num,'0');
            }
            else
            {
                return codPai+"001";
            }
        }
        protected void ImgBtnNivel1Add_Click(object sender, EventArgs e)
        {
            string prox = proximo(GridNivel1, 3,"");
            addNovoDetalhe("",prox);
        }

        protected void ImgBtnNivel2Pesquisar_Click(object sender, EventArgs e)
        {

        }

        protected void ImgBtnNivel2Editar_Click(object sender, EventArgs e)
        {
            string codigoPai = selecionado(GridNivel1, 2);
            showDetalhes(GridNivel2, codigoPai);
        }

        protected void imgBtnNivel2Excluir_Click(object sender, EventArgs e)
        {
            lblExcluirTitle.Text = selecionado(GridNivel2, 1);
            modalExcluirPlano.Show();

        }

        protected void ImgBtnNivel2Add_Click(object sender, EventArgs e)
        {
            string codigoPai = selecionado(GridNivel1, 2);
            string prox = proximo(GridNivel2, 6,codigoPai);
            addNovoDetalhe(codigoPai,prox);
        }

        protected void ImgBtnNivel3Pesquisar_Click(object sender, EventArgs e)
        {

        }

        protected void ImgBtnNivel3Editar_Click(object sender, EventArgs e)
        {
            status = "editar";
            string codigoPai = selecionado(GridNivel2, 2);
            showDetalhes(GridNivel3, codigoPai);
        }

        protected void imgBtnNivel3Excluir_Click(object sender, EventArgs e)
        {
            lblExcluirTitle.Text = selecionado(GridNivel3, 1);
            modalExcluirPlano.Show();

        }

        protected void ImgBtnNivel3Add_Click(object sender, EventArgs e)
        {
            status = "incluir";
            string codigoPai = selecionado(GridNivel2, 2);
            string prox = proximo(GridNivel3, 9,codigoPai);
            addNovoDetalhe(codigoPai,prox);
        }

        protected void ImgBtnNivel4Pesquisar_Click(object sender, EventArgs e)
        {

        }

        protected void ImgBtnNivel4Editar_Click(object sender, EventArgs e)
        {
            status = "editar";
            string codigoPai = selecionado(GridNivel3, 2);
            showDetalhes(GridNivel4, codigoPai);
        }

        protected void imgBtnNivel4Excluir_Click(object sender, EventArgs e)
        {
            lblExcluirTitle.Text = selecionado(GridNivel4, 1);
            modalExcluirPlano.Show();

        }

        protected void ImgBtnNivel4Add_Click(object sender, EventArgs e)
        {
            status = "incluir";
            string codigoPai = selecionado(GridNivel3, 2);
            string prox = proximo(GridNivel4, 12,codigoPai);
            addNovoDetalhe( codigoPai,prox);
        }

        protected void ImgBtnNivel5Pesquisar_Click(object sender, EventArgs e)
        {

        }

        protected void ImgBtnNivel5Editar_Click(object sender, EventArgs e)
        {
            status = "editar";
            string codigoPai = selecionado(GridNivel4, 2);
            showDetalhes(GridNivel5, codigoPai);
        }

        protected void imgBtnNivel5Excluir_Click(object sender, EventArgs e)
        {
            lblExcluirTitle.Text = selecionado(GridNivel5, 1);
            modalExcluirPlano.Show();

        }

        protected void ImgBtnNivel5Add_Click(object sender, EventArgs e)
        {
            status = "incluir";
            string codigoPai = selecionado(GridNivel4, 2);
            string prox = proximo(GridNivel5, 3,codigoPai);
            addNovoDetalhe(codigoPai,prox);
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {

        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {

        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {

        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override bool campoDesabilitado(Control campo)
        {
            String[] campos = {"txtCodGrupoSubGrupo",
                               "txtDescricaoGrupoSubGrupo",
                               "txtCodSubGrupoDepartamento",
                               "txtDescricaoSubGrupoDepartamento",
                               "txtCodGrupoDepartamento",
                               "txtDescricaoGrupoDepartamento",
                               "txtCodigoPlanoPaiDetalhe"

                                     };


            return existeNoArray(campos, campo.ID + "");

        }

        protected override bool campoObrigatorio(Control campo)
        {
            return false;
        }

        protected void GridNivel1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridNivel1.*GrGrupo',this)";
            rdo.Attributes.Add("onclick", script);

        }
        protected void GridNivel2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridNivel2.*GrGrupo',this)";
            rdo.Attributes.Add("onclick", script);

        }
        protected void GridNivel3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridNivel3.*GrGrupo',this)";
            rdo.Attributes.Add("onclick", script);

        }
        protected void GridNivel4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridNivel4.*GrGrupo',this)";
            rdo.Attributes.Add("onclick", script);

        }
        protected void GridNivel5_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RadioButton rdo = (RadioButton)e.Row.FindControl("RdoItem");

            if (rdo == null)
            {
                return;
            }
            string script = "SetUniqueRadioButton('GridNivel5.*GrGrupo',this)";
            rdo.Attributes.Add("onclick", script);

        }
        protected void btnOkError_Click(object sender, EventArgs e)
        {
            modalError.Hide();
        }

        protected void showMessage(String mensagem, bool erro)
        {
            lblErroPanel.Text = mensagem;
            if (erro)
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblErroPanel.ForeColor = System.Drawing.Color.Blue;
            }
            modalError.Show();
        }
        protected void btnConfirmaDetalhes_Click(object sender,EventArgs e)
        {
            nivelAtual();
            if (txtIdDetalhe.Text.Equals(""))
            {
                User usr = (User)Session["User"];
                txtIdDetalhe.Text = Funcoes.sequencia("plano_de_contas_contabil.id", usr);
                Funcoes.salvaProximaSequencia("plano_de_contas_contabil.id", usr);
            }
            PlanoDeContasContabilDAO plano = new PlanoDeContasContabilDAO();
            plano.codigoPlanoPai = txtCodigoPlanoPaiDetalhe.Text;
            plano.id = txtIdDetalhe.Text;
            plano.codigo = txtCodigoDetalhe.Text;
            plano.descricao = txtDescricaoDetalhe.Text;
            
            plano.salvar((status == "incluir"));
            status = "visualizar";
            modalDetalhe.Hide();
            carregarDados();
            carregarNivelAtual();
        }

        protected void btnCancelaDetalhes_Click(object sender, EventArgs e)
        {
            modalDetalhe.Hide();
        }

        protected void RdoNivel1Item_CheckedChanged(object sender, EventArgs e)
        {

            RadioButton rdo = (RadioButton)sender;
            
            String codigo1 = selecionado(GridNivel1, 2);
            carregarNiveis(GridNivel2, codigo1);
            String codigo2 = selecionado(GridNivel2, 2);
            carregarNiveis(GridNivel3, codigo2);
            String codigo3 = selecionado(GridNivel3, 2);
            carregarNiveis(GridNivel4, codigo3);
            String codigo4 = selecionado(GridNivel4, 2);
            carregarNiveis(GridNivel5, codigo4);
            rdo.Focus();

        }

        protected void RdoNivel2Item_CheckedChanged(object sender, EventArgs e)
        {

            RadioButton rdo = (RadioButton)sender;
           
            String codigo2 = selecionado(GridNivel2, 2);
            carregarNiveis(GridNivel3, codigo2);
            String codigo3 = selecionado(GridNivel3, 2);
            carregarNiveis(GridNivel4, codigo3);
            String codigo4 = selecionado(GridNivel4, 2);
            carregarNiveis(GridNivel5, codigo4);
            rdo.Focus();
        }

        protected void RdoNivel3Item_CheckedChanged(object sender, EventArgs e)
        {

            RadioButton rdo = (RadioButton)sender;
            
            String codigo3 = selecionado(GridNivel3, 2);
            carregarNiveis(GridNivel4, codigo3);
            String codigo4 = selecionado(GridNivel4, 2);
            carregarNiveis(GridNivel5, codigo4);
            rdo.Focus();
        }

        protected void RdoNivel4Item_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            
            String codigo4 = selecionado(GridNivel4, 2);
            carregarNiveis(GridNivel5, codigo4);
            rdo.Focus();
        }

        protected void RdoNivel5Item_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            rdo.Focus();
        }
        protected void nivelAtual()
        {
            string[] nivel = new string[5];
                
            
            nivel[0] = selecionado(GridNivel1, 2);
            nivel[1] = selecionado(GridNivel2, 2);
            nivel[2] = selecionado(GridNivel3, 2);
            nivel[3] = selecionado(GridNivel4, 2);
            nivel[4] = selecionado(GridNivel5, 2);

            Session.Remove("nivelAtual");
            Session.Add("nivelAtual", nivel);


        }
        protected void carregarNivelAtual()
        {
            string[] nivel = (string[])Session["nivelAtual"];
            carregarNiveis(GridNivel1, "0");
            focus(nivel[0], GridNivel1);
            carregarNiveis(GridNivel2, nivel[0]);
            focus(nivel[1], GridNivel2);
            carregarNiveis(GridNivel3, nivel[1]);
            focus(nivel[2], GridNivel3);
            carregarNiveis(GridNivel4, nivel[2]);
            focus(nivel[3], GridNivel4);
            carregarNiveis(GridNivel5, nivel[3]);
            focus(nivel[4], GridNivel5);
        }
        protected void btnConfirmaExcluir_Click(object sender, EventArgs e)
        {
            nivelAtual();

            List<PlanoDeContasContabilDAO> PLANOS = (List<PlanoDeContasContabilDAO>)Session["listaPlanos" + urlSessao()];
            PlanoDeContasContabilDAO pl = PLANOS.Find((p) => p.id == lblExcluirTitle.Text);
            pl.delete();
            PLANOS.Remove(pl);
            carregarNivelAtual();
            
         
        }

        protected void btnCancelarExcluir_Click(object sender, EventArgs e)
        {
            modalExcluirPlano.Hide();
        }
    }
}