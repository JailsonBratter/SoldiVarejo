using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using visualSysWeb.code;
using System.Data.SqlClient;
using senha;
using static visualSysWeb.Account.Login;
using System.IO;
using System.Text;

namespace visualSysWeb.modulos.mobile.pages
{
    public partial class Mobile_Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Conexao.preencherDDL(ddfilial, "select Filial f,Filial from filial", "Filial", "f", new User());
                if (Request.Params["filial"] != null)
                {
                    String ultimaAcessada = Request["filial"].ToString();
                    ddfilial.Text = ultimaAcessada;
                }
                else
                {
                    try
                    {
                        ddfilial.Text = "MW";
                    }
                    catch (Exception)
                    {
                    }
                }
                txtUsuario.Focus();
            }

            txtUsuario.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtSenha.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            //gerarClasse.gerar("pedido_venda");

        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            logar();
        }


        protected void logar()
        {
            if (txtUsuario.Text.Equals(""))
            {
                lblerros.Text = " - Informe o Usuario";
                txtUsuario.Focus();
            }
            else if (txtSenha.Text.Equals(""))
            {
                lblerros.Text = " - Informe sua senha";
                txtSenha.Focus();
            }
            else
            {
                try
                {
                    User user = new User(txtUsuario.Text, txtSenha.Text, ddfilial.SelectedItem.Value);
                    user.filial.caminhoServidor = Server.MapPath("../");
                    Session.Add("User", user);
                    Response.Redirect("~/modulos/mobile/SmartPhone_Menu.aspx");
                }
                catch (Exception err)
                {
                    lblerros.Text = err.Message;

                }
            }
        }
        protected void txtSenha_TextChanged(object sender, EventArgs e)
        {
            logar();
        }

        protected void ddfilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            verificaCNPJ();
            txtUsuario.Focus();
        }
        protected void verificaCNPJ()
        {
            btnEntrar.Visible = true;
            txtUsuario.Enabled = true;
            txtSenha.Enabled = true;

            lblerros.Text = "";
            String chave = "";
            String cnpj = "";
            SqlDataReader rsFilial = null;
            try
            {
                rsFilial = Conexao.consulta("select cnpj,chave_senha from filial where filial='" + ddfilial.Text + "'", null, false);


                if (rsFilial.Read())
                {
                    chave = rsFilial["chave_senha"].ToString();
                    cnpj = rsFilial["cnpj"].ToString();
                }


                Senha senha = new Senha(chave);
                if (!senha.cnpj.Equals("") && !cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Equals(senha.cnpj.Replace(".", "").Replace("-", "").Replace("/", "")))
                {
                    throw new Exception();
                }

                if (!senha.dentroPrazo())
                {
                    throw new Exception();
                }

                if (senha.avisovencimento())
                {
                    servSenha srv = new servSenha(verificaServerSenha);
                    srv.BeginInvoke(cnpj, ddfilial.Text, null, null);
                    //System.Threading.Thread th = new System.Threading.Thread(verificaServerSenha);
                    //th.Start();
                    TimeSpan qtdDias = senha.dataValidade - DateTime.Now.AddDays(-1);
                    lblerros.Text = "SUA LICENÇA VAI VENCER DIA " + senha.dataValidade.ToString("dd/MM/yyyy") + "<br/> FALTAM " + qtdDias.Days + " DIAS";


                }



            }
            catch (Exception err)
            {

                servSenha srv = new servSenha(verificaServerSenha);
                srv.BeginInvoke(cnpj, ddfilial.Text, null, null);

                btnEntrar.Visible = false;
                txtUsuario.Enabled = false;
                txtSenha.Enabled = false;




                lblerros.Text = " NÃO FOI POSSIVEL VALIDAR O SOFTWARE ! <br/>RECARREGUE A PAGINA APERTANDO A TECLA F5 SE O PROBLEMA PERSISTIR POR FAVOR ENTRAR EM CONTATO COM A BRATTER!!! <br/> OU DIGITE UMA CHAVE VÁLIDA ";

                if (!Directory.Exists("c:\\logVSW"))
                    Directory.CreateDirectory("c:\\logVSW");


                StreamWriter ArqLog = new StreamWriter("c:\\logVSW\\" + DateTime.Now.ToString("yyyyMMddhhmmss") + "logservidor.txt", false, Encoding.ASCII); ArqLog.Write(err.Message);
                ArqLog.Close();

                //System.Threading.Thread th = new System.Threading.Thread(verificaServerSenha);
                //th.Start();

            }
            finally
            {
                if (rsFilial != null)
                    rsFilial.Close();
            }
        }
        protected void verificaServerSenha(String cnpj, String filial)
        {
            try
            {



                ServerSenha.IService1 srvSenha = new ServerSenha.Service1Client();
                String retornoServ = srvSenha.GetChave(cnpj);
                //throw new Exception(retornoServ);

                if (!retornoServ.Equals(""))
                {
                    Conexao.executarSql("update filial set chave_senha='" + retornoServ + "' where filial ='" + filial + "'");
                }
            }
            catch (Exception err)
            {
                if (!Directory.Exists("c:\\logVSW"))
                    Directory.CreateDirectory("c:\\logVSW");

                StreamWriter ArqLog = new StreamWriter("c:\\logVSW\\" + DateTime.Now.ToString("yyyyMMddhhmmss") + "logservidor.txt", false, Encoding.ASCII);
                ArqLog.Write(err.Message);
                ArqLog.Close();
            }
        }

    }
}
