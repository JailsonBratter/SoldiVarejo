using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Data;
using visualSysWeb.code;
using visualSysWeb.modulos.Cadastro.dao;
using visualSysWeb.modulos.Dispositivos.dao;

namespace visualSysWeb.modulos.Dispositivos.pages
{
    public partial class CargaBalanca : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargaDAO carga = null;
            status = "editar";
            carregabtn(pnBtn);
            User usr = (User)Session["user"];
            // carga = new CargaDAO(usr);
            Session.Remove("carga" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""));
            Session.Add("carga" + Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", ""), carga);


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
            throw new NotImplementedException();
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                User usr = (User)Session["user"];
                int tipoCarga = 101 + int.Parse(rdoBalanca.SelectedValue);

                CargaDAO carga = new CargaDAO(usr, tipoCarga, 0);
                CargaDAO.limpaTemp();
               

                if (rdoBalanca.SelectedValue.Equals("7"))
                {
                    foreach (GridViewRow item in gridEmbaladoras.Rows)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                        if (chk.Checked)
                        {
                            EmbaladoraDAO Emb = new EmbaladoraDAO(Funcoes.intTry(item.Cells[1].Text), usr);

                            try
                            {
                                FtpCode.EnviarArquivoFTP(usr.filial.diretorio_balanca+"\\PLU0DALL.CSV","FTP://" +Emb.End_FTP+ "/PLU0DALL.CSV", Emb.Usuario, Emb.Senha);
                                item.ForeColor = System.Drawing.Color.Green;
                            }
                            catch (Exception err)
                            {
                                item.ForeColor = System.Drawing.Color.Red;
                                throw new Exception("Embaladora "+Emb.ID+"-"+ Emb.Descricao+":"+err.Message);
                            }

                        }

                    }
                    MsgShow("Arquivo enviado com sucesso para embaladora",false); 
                }
                else
                {
                    MsgShow("Arquivo de Carga Gerado com Sucesso! Gerado em:" + usr.filial.diretorio_balanca, false);
                }
            }
            catch (Exception err)
            {
                MsgShow("Não foi possivel Gerar o Arquivo de Carga. Erro:" + err.Message, true);

            }//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected override bool campoDesabilitado(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override bool campoObrigatorio(Control campo)
        {
            throw new NotImplementedException();
        }

        protected void rdoBalanca_SelectedIndexChanged(object sender, EventArgs e)
        {
            imgBalanca.ImageUrl = "~\\modulos\\Dispositivos\\imgs\\balanca" + rdoBalanca.SelectedValue + ".jpg";
            divEmbaladoras.Visible = rdoBalanca.SelectedValue.Equals("7");
            if (divEmbaladoras.Visible)
                carregarEmbaladoras();
        }

        protected void carregarEmbaladoras()
        {
            User usr = (User)Session["User"];
            String sql = "Select * from embaladoras ";
            try
            {
                gridEmbaladoras.DataSource = Conexao.GetTable(sql, usr, false);
                gridEmbaladoras.DataBind();
                CheckBox chkTodos = (CheckBox)gridEmbaladoras.HeaderRow.FindControl("chkSeleciona");
                chkTodos.Checked = true;
                foreach (GridViewRow item in gridEmbaladoras.Rows)
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                    chk.Checked = true;
                }
            }
            catch (Exception err)
            {
                MsgShow(err.Message, true);

            }







        }
        protected void chkSeleciona_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTodos = (CheckBox)gridEmbaladoras.HeaderRow.FindControl("chkSeleciona");
            foreach (GridViewRow item in gridEmbaladoras.Rows)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkSelecionaItem");
                //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                if (chk != null)
                {
                    chk.Checked = chkTodos.Checked;
                }
            }
        }

        protected void MsgShow(String mensagem, bool erro)
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

        protected void btnOkError_Click(object sender, EventArgs e)
        {
            modalError.Hide();
        }
    }
}