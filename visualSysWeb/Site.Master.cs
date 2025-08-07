using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;
using visualSysWeb.code;
using senha;


namespace visualSysWeb
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Funcoes.diretorioServ.Equals(""))
                Funcoes.diretorioServ = Server.MapPath("~");

            NavigationMenu.Visible = false;
            lnkLogout.Visible = false;
            
            Ajax.Utility.RegisterTypeForAjax(typeof(PagePadrao));
            String strPaginaAtual = Request.Url.AbsoluteUri;
            strPaginaAtual = strPaginaAtual.Remove(strPaginaAtual.IndexOf(".aspx"));
            strPaginaAtual = strPaginaAtual.Remove(0, strPaginaAtual.LastIndexOf("/") + 1);
            Page.Title ="SOLDI VAREJO - "+ strPaginaAtual;
            ImgBtnVoltar.Visible = (Request.Params["antigapg"] != null);
            Menus  mn = new Menus(new User(), Server.MapPath(""));


            lblVersao.Text = mn.versao();

            if (Session["user"] == null)
            {
                if (Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").IndexOf("Login.aspx") < 0)
                {
                    Response.Redirect("~/Account/Login.aspx");


                    //int i = 0;
                }
                else
                {
                    NavigationMenu.Visible = false;
                    lnkLogout.Visible = false;
                }
            }
            else {
                if (Request.Url.ToString().Replace(" ", "").Replace("+", "").Replace("%20", "").IndexOf("Login.aspx") > 0)
                {
                    NavigationMenu.Visible = false;
                    lnkLogout.Visible = false;
                }
                User user = (User)Session["user"];
                lblLogin.Text = "Usuario :" + user.getUsuario() + " Nome:" + user.getNome() + "<br/>" +
                                "Filial  :" + user.getFilial();
                String dt="Sem Validade";
                try
                {

                
                Senha senha = new Senha(user.filial.chave_senha);
                 dt= senha.dataValidade.ToString("dd/MM/yyyy");
                 if (dt.Equals("01/01/0001"))
                     dt = "Sem Validade";
                 else
                     dt = "Valido até o dia :" + dt;
                }
                catch (Exception)
                {
                    dt = "não foi possivel verificar a Data";
                }
                lblIpAcessado.Text = "<center>Ip:" + (user.filial.ip.Equals("::1") ? "LocalHost" : user.filial.ip) + "  <br/>Banco:" + Conexao.nomeBanco() + " <br/>EMPRESA:" + user.filial.Razao_Social +" - <b>Data fechamento do estoque: " + user.filial.dtFechamentoEstoque.ToString("dd-MM-yyyy") +  "</b><br/> Versão anterior:" + user.filial.ultima_versao +"<br/>"+dt +"</center>";


                 mn = new Menus(user, Server.MapPath(""));

                //NavigationMenu.Items.Clear();
                mn.montarMenu(NavigationMenu);

                NavigationMenu.Visible = true;
                lnkLogout.Visible = true;
                /* NavigationMenu.Visible = true;
                MenuItem menu = new MenuItem("Teste");
                      menu.ChildItems.Add(new MenuItem("teste1"));       
                NavigationMenu.Items.Add(menu);
                */
            }
            Session.Remove("menu");
            Session.Add("menu", mn);
            


        }


       


        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
            Page.Header.DataBind();


        }

        

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            User user = (User)Session["user"];
            Session["user"] = null;
            Response.Redirect("~/Account/Login.aspx?filial="+user.getFilial());
        }
        //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
        protected void ImgBtnVoltar_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.Params["antigapg"] != null)
            {
                String antigapg = Request.Params["antigapg"];
                Response.Redirect(antigapg);
            }
        }

        
    }
}
