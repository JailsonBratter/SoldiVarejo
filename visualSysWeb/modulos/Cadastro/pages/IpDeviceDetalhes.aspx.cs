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
    public partial class IpDeviceDetalhes : visualSysWeb.code.PagePadrao
    {
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            
            //tabMenu.Items[MultiView1.ActiveViewIndex].Selected = true;
            if (Request.Params["novo"] != null)
            {
                ipDeviceDAO obj = new ipDeviceDAO(usr);
                status = "incluir";
                Session.Remove("ipDevice" + urlSessao());
                Session.Add("ipDevice" + urlSessao(), obj);
                EnabledControls(conteudo, true);
                //EnabledControls(cabecalho, true);
            }
            else
            {
                if (Request.Params["id"] != null)  // colocar o campo index da tabela
                {
                    try
                    {
                        if (!IsPostBack)
                        {
                            String index = Request.Params["id"].ToString();// colocar o campo index da tabela
                            status = "visualizar";
                            ipDeviceDAO obj = new ipDeviceDAO(index, usr);
                            Session.Remove("ipDevice" + urlSessao());
                            Session.Add("ipDevice" + urlSessao(), obj);

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
            carregabtn(pnBtn);
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
            String[] campos = { "txtid", 
                                    "txtip", 
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
            //EnabledControls(cabecalho, true);
            EnabledControls(conteudo, true);
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ipDevice.aspx"); //colocar o endereco da tela de pesquisa
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            lblError.Text = "Não é possivel Excluir o Registro";
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaCamposObrigatorios())
                {

                    carregarDadosObj();

                    ipDeviceDAO obj = (ipDeviceDAO)Session["ipDevice" + urlSessao()];
                    obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                    lblError.Text = "Salvo com Sucesso";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                    //EnabledControls(cabecalho, false);
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
            Response.Redirect("ipDevice.aspx");//colocar endereco pagina de pesquisa
        }
       
        //--Atualizar DaoForm 
        private void carregarDados()
        {

            ipDeviceDAO obj = (ipDeviceDAO)Session["ipDevice" + urlSessao()];
            txtip.Text = obj.ip.ToString();
            txttipo.Text = obj.tipo.ToString();
            txtBalanca_Serial.Text = obj.balanca_serial;
            txtid.Text = obj.id.ToString();
        }

        // --Atualizar FormDao 
        private void carregarDadosObj()
        {
            ipDeviceDAO obj = (ipDeviceDAO)Session["ipDevice" + urlSessao()];   
            obj.ip = txtip.Text;
            obj.tipo = txttipo.Text;
            obj.balanca_serial = txtBalanca_Serial.Text;
            obj.id = int.Parse(txtid.Text);
            Session.Remove("ipDevice" + urlSessao());
            Session.Add("ipDevice" + urlSessao(), obj);

        }



        protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                ipDeviceDAO obj = (ipDeviceDAO)Session["ipDevice" + urlSessao()];
                obj.excluir();
                //pnConfima.Visible = false;
                lblError.Text = "Registro Excluido com sucesso";
                limparCampos();
                pesquisar(pnBtn);
            }
            catch (Exception err)
            {
                lblError.Text = "N?o foi possivel Excluir o registro error:" + err.Message;
            }
        }

        protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
        {
            //pnConfima.Visible = false;
        }

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