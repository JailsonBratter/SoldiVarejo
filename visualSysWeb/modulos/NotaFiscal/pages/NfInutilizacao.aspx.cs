using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using visualSysWeb.modulos.NotaFiscal.code;
using visualSysWeb.code;
using visualSysWeb.modulos.NotaFiscal.NFeRT;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class NfInutilizacao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User usr = (User)Session["User"];
                if (usr != null)
                    txtSerie.Text = usr.filial.serie_nfe.ToString();

                lblError.Text = "";
            }
            gridItens.DataSource = Conexao.GetTable("Select data=CONVERT(varchar,data,103),serie,N_inicio,N_fim,protocolo,justificativa,usuario     from nf_inutilizadas order by CONVERT(varchar,data,102)desc ", null, false);
            gridItens.DataBind();


        }



        protected void BtnInutilizar_Click1(object sender, EventArgs e)
        {
            txtSerie.BackColor = System.Drawing.Color.White;
            txtNumeroInicial.BackColor = System.Drawing.Color.White;
            txtNumeroInicial.BackColor = System.Drawing.Color.White;
            lblError.Text = "";


            if (txtSerie.Text.Trim().Equals(""))
            {
                txtSerie.BackColor = System.Drawing.Color.Red;
                lblError.Text = "O Campo Serie é obrigatorio";
            }
            else if (txtNumeroInicial.Text.Trim().Equals("") || !Funcoes.isnumeroint(txtNumeroInicial.Text))
            {
                txtNumeroInicial.BackColor = System.Drawing.Color.Red;
                lblError.Text = "O Campo Numero Inicial é obrigatorio e tem que ser um Numero Valido";

            }
            else if (txtNumeroFinal.Text.Trim().Equals("") || !Funcoes.isnumeroint(txtNumeroFinal.Text))
            {
                txtNumeroInicial.BackColor = System.Drawing.Color.Red;
                lblError.Text = "O Campo Numero Final é obrigatorio e tem que ser um Numero Valido";
            }
            else if (txtJustificativa.Text.Trim().Equals("") || txtJustificativa.Text.Length < 15)
            {
                txtJustificativa.BackColor = System.Drawing.Color.Red;
                lblError.Text = "O Campo Justificativa é obrigatorio e tem que ter no minimo 15 Caracteres";
            }
            else
            {
                modalPnConfirma.Show();
            }
        }

        protected void executar()
        {
            try
            {


                User usr = (User)Session["User"];

                NFCeOperacoes nfe = new NFCeOperacoes(usr);

                var retorno = nfe.InutilizarNumeracao(DateTime.Now.Year, usr.filial.CNPJ, txtJustificativa.Text, int.Parse(txtNumeroInicial.Text), int.Parse(txtNumeroFinal.Text), int.Parse(txtSerie.Text));

                if (retorno.Retorno.infInut.cStat == 102)
                {
                    String strProtocolo = retorno.Retorno.infInut.nProt.ToString(); //  Resposta.Substring(Resposta.IndexOf("Protocolo :") + 11, 15);
                    String sqlInsert = "insert into nf_inutilizadas (serie,N_inicio,N_fim,protocolo,justificativa,data,usuario)" +
                                                " values('" + txtSerie.Text + "'," + txtNumeroInicial.Text.Trim() + "," + txtNumeroFinal.Text + ",'" + strProtocolo + "','" + txtJustificativa.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + usr.getUsuario() + "')";

                    Conexao.executarSql(sqlInsert);

                    Conexao.executarSql("Update nf  set status='INUTILIZADO' WHERE CODIGO >=" + txtNumeroInicial.Text + " AND CODIGO <=" + txtNumeroFinal.Text + " AND TIPO_NF=1");



                    lblRespostaInutilizacao.Text = "Sucesso. " + retorno.Retorno.infInut.cStat + " " + retorno.Retorno.infInut.xMotivo;
                    lblRespostaInutilizacao.ForeColor = System.Drawing.Color.Blue;
                    //modalPnConfirma.Hide();
                    Session.Remove("Resposta-Inutilizacao");
                    Session.Add("Resposta-Inutilizacao", retorno.Retorno.infInut.xMotivo);

                }
                else
                {
                    lblRespostaInutilizacao.Text = "FALHA. " + retorno.Retorno.infInut.cStat + " " + retorno.Retorno.infInut.xMotivo;
                    lblRespostaInutilizacao.ForeColor = System.Drawing.Color.Red;
                    new Exception("Erro na inutilização: codigo " + retorno.Retorno.infInut.cStat + " " + retorno.Retorno.infInut.xMotivo);
                }


               // xmlNFE xml = new xmlNFE(usr);
               //xml.inutilizarNumero(txtSerie.Text, txtNumeroInicial.Text, txtNumeroFinal.Text, txtJustificativa.Text, usr);
               // int cont = 0;
               // bool encerra = false;
               // String Resposta = "";
               // while (!encerra)
               // {
               //     if (cont >= 50)
               //         encerra = true;
               //     try
               //     {
               //         Resposta = xml.respostaInutilizacao(txtSerie.Text, txtNumeroInicial.Text, txtNumeroFinal.Text, DateTime.Now, encerra);

               //         encerra = true;
               //         String strProtocolo = Resposta.Substring(Resposta.IndexOf("Protocolo :") + 11, 15);
               //         String sqlInsert = "insert into nf_inutilizadas (serie,N_inicio,N_fim,protocolo,justificativa,data,usuario)" +
               //                                     " values('" + txtSerie.Text + "'," + txtNumeroInicial.Text.Trim() + "," + txtNumeroFinal.Text + ",'" + strProtocolo + "','" + txtJustificativa.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + usr.getUsuario() + "')";

               //         Conexao.executarSql(sqlInsert);

               //         Conexao.executarSql("Update nf  set status='INUTILIZADO' WHERE CODIGO >=" + txtNumeroInicial.Text + " AND CODIGO <=" + txtNumeroFinal.Text +" AND TIPO_NF=1");

               //         //modalPnConfirma.Hide();
               //         Session.Remove("Resposta-Inutilizacao");
               //         Session.Add("Resposta-Inutilizacao", Resposta);
                      
               //         //modalResposta.Show();

               //     }
               //     catch (Exception err)
               //     {
               //         if (err.Message.IndexOf("Erro-Inutilizar:") >= 0 || encerra)
               //         {
               //             Session.Remove("Erro-Inutilizacao");
               //             Session.Add("Erro-Inutilizacao",err.Message);
               //             encerra = true;
               //         }
               //     }
               //     cont++;
               // }
            }
            catch (Exception err)
            {
                Session.Remove("Erro-Inutilizacao");
                Session.Add("Erro-Inutilizacao", err.Message);
            }
        }

        protected void btnConfirmaInutilizacao_Click(object sender, EventArgs e)
        {
            try
            {
                User usr = (User)Session["User"];
                //TimerXml.Interval = 450;
                //TimerXml.Enabled = true;
                //System.Threading.Thread th = new System.Threading.Thread(executar);
                //th.Start();
                executar();
                
                modalPnConfirma.Hide();
                modalResposta.Show();
                //lblRespostaInutilizacao.Text = "AGUARDE .....";
                //lblRespostaInutilizacao.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception err)
            {
                modalPnConfirma.Hide();
                lblRespostaInutilizacao.Text = "Erro" + err.Message;
                lblRespostaInutilizacao.ForeColor = System.Drawing.Color.Red;
                modalResposta.Show();
            }

        }
        protected void btnCancelaInutilizacao_Click(object sender, EventArgs e)
        {

            modalPnConfirma.Hide();
        }

        protected void TimerXml_Tick(object sender, EventArgs e)
        {
            String resultado = (String)Session["Resposta-Inutilizacao"];
            String error = (String)Session["Erro-Inutilizacao"];
            String aborta = (String)Session["aborta"];
            if (aborta != null)
            {
                TimerXml.Enabled = false;
                Session.Remove("aborta");
            }

            if (resultado != null)
            {
                txtJustificativa.Text = "";
                txtNumeroInicial.Text = "";
                txtNumeroFinal.Text = "";

                TimerXml.Enabled = false;
                lblRespostaInutilizacao.Text = resultado;
                lblRespostaInutilizacao.ForeColor = System.Drawing.Color.Blue;
                Session.Remove("Resposta-Inutilizacao");
            }
            else if (error != null)
            {
                TimerXml.Enabled = false;
                lblRespostaInutilizacao.Text = "Erro" + error;
                lblRespostaInutilizacao.ForeColor = System.Drawing.Color.Red;
                Session.Remove("Erro-Inutilizacao");
            }
            else
            {
                modalPnConfirma.Hide();
                lblRespostaInutilizacao.Text = "AGUARDE .....";
                lblRespostaInutilizacao.ForeColor = System.Drawing.Color.Green;

            }
            modalResposta.Show();
        }
    }
}