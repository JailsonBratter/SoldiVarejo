using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using visualSysWeb.code;
using senha;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Net;
using System.Configuration;

namespace visualSysWeb.Account
{
    public partial class Login : System.Web.UI.Page
    {
        public delegate void servSenha(String cnpj, String filial);
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();
            Funcoes.limpaParametros();
            if (!IsPostBack)
            {
                Conexao.preencherDDL(ddfilial, "select Filial f,Filial from filial", "Filial", "f", new User());
                txtUsuario.Focus();
                if (Request.Params["filial"] != null)
                {
                    String ultimaAcessada = Request["filial"].ToString();
                    ddfilial.Text = ultimaAcessada;
                }
                else
                {
                    try
                    {
                        ddfilial.Text = "MATRIZ";
                    }
                    catch (Exception)
                    {
                    }
                }

                verificaCNPJ();
            }
            txtUsuario.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtSenha.Attributes.Add("OnKeyPress", "javascript:return autoTab(this,event);");
            txtUsuario.Attributes.Add("autocomplete", "off");
            txtSenha.Attributes.Add("autocomplete", "off");

        }

        protected void verificaServerSenha(String cnpj, String filial)
        {
            try
            {

                String retornoServ = "";
                try
                {
                    //ServerSenha.IService1 srvSenha = new ServerSenha.Service1Client();
                    // retornoServ = srvSenha.GetChave(cnpj);

                    var appSettings = ConfigurationManager.AppSettings;
                    string servidorSenha = appSettings["ServidorSenha"];

                    using (WebClient client = new WebClient())
                    {
                        //retornoServ = client.DownloadString("http://34.123.130.117:19745/jga/api/licenca?cnpj=" + cnpj.Replace(".", "").Replace("-", "").Replace("/", ""));
                        retornoServ = client.DownloadString("http://" + servidorSenha + "/jga/api/licenca?cnpj=" + cnpj.Replace(".", "").Replace("-", "").Replace("/", ""));
                        retornoServ = retornoServ.Replace("\"", "");
                    }

                    //throw new Exception(retornoServ);
                }
                catch (Exception e)
                {
                    ServiceReferenceLocal.IService1 srvSenhaLocal = new ServiceReferenceLocal.Service1Client();
                    retornoServ = srvSenhaLocal.GetChave(cnpj);
                }
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

        protected void verificaCNPJ()
        {
            btnEntrar.Visible = true;
            txtUsuario.Enabled = true;
            txtSenha.Enabled = true;
            pnLogin.Visible = true;
            PnChave.Visible = false;
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
                servSenha srv = new servSenha(verificaServerSenha);
                srv.BeginInvoke(cnpj, ddfilial.Text, null, null);

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
                  
                    //System.Threading.Thread th = new System.Threading.Thread(verificaServerSenha);
                    //th.Start();
                    TimeSpan qtdDias = senha.dataValidade - DateTime.Now.AddDays(-1);
                    lblerros.Text = "SUA LICENÇA VAI VENCER DIA " + senha.dataValidade.ToString("dd/MM/yyyy") + "<br/> FALTAM " + qtdDias.Days + " DIAS";
                    lnkChave.Visible = true;

                }
               


            }
            catch (Exception err)
            {

                btnEntrar.Visible = false;
                txtUsuario.Enabled = false;
                txtSenha.Enabled = false;
                pnLogin.Visible = false;
                PnChave.Visible = true;



                lblerros.Text = " NÃO FOI POSSIVEL VALIDAR O SOFTWARE ! <br/>RECARREGUE A PAGINA APERTANDO A TECLA F5 SE O PROBLEMA PERSISTIR POR FAVOR ENTRAR EM CONTATO COM A BRATTER!!! <br/> OU DIGITE UMA CHAVE VÁLIDA ";

                if (!Directory.Exists("c:\\logVSW"))
                    Directory.CreateDirectory("c:\\logVSW");

                StreamWriter ArqLog = new StreamWriter("c:\\logVSW\\" + DateTime.Now.ToString("yyyyMMddhhmmss") + "logConexaoBanco.txt", false, Encoding.ASCII);
                ArqLog.Write(err.Message);
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

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            logar();
        }


        protected void logar()
        {

            if (txtSenha.Text.Equals("gerar"))
            {
                try
                {
                    gerarClasse.gerar(txtUsuario.Text, Server.MapPath(""));
                    lblerros.Text = "Classe gerada com sucesso";

                }
                catch (Exception er)
                {

                    lblerros.Text = er.Message;
                }

            }
            else
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
                        User user = new User(txtUsuario.Text, txtSenha.Text, ddfilial.Text);
                        user.filial.caminhoServidor = Server.MapPath("../");
                        user.filial.ip = Request.UserHostAddress;
                        Session.Add("User", user);
                        Response.Redirect("../Default.aspx");
                    }
                    catch (Exception err)
                    {
                        lblerros.Text = err.Message;

                    }
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

        protected void btnValidaChave_Click(object sender, EventArgs e)
        {
            if (!txtChave.Text.Trim().Equals(""))
            {
                Conexao.executarSql("UPDATE FILIAL SET CHAVE_SENHA='" + txtChave.Text.Trim() + "' where filial='" + ddfilial.Text + "'");

            }
            Response.Redirect("Login.aspx?filial=" + ddfilial.Text);
        }

        protected void lnkChave_Click(object sender, EventArgs e)
        {
            pnLogin.Visible = false;
            PnChave.Visible = true;
        }


    }
}