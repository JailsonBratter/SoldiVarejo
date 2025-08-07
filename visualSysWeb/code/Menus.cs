using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml.XPath;
using visualSysWeb.dao;
using System.Data.SqlClient;
using senha;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
/// <summary>
/// Summary description for Menus
/// </summary>
/// 
namespace visualSysWeb.code
{
    public class Menus
    {
        User user;
        static String serverPath = "";
        static String strAtualVersao = "";

        XPathNavigator rs;
        XPathDocument docNav;

        XPathNavigator rsMod;
        XPathDocument docModulos;
        Senha senha ;

        public Menus(User user, string server)
        {
            try
            {
                this.user = user;
                this.senha = new Senha(user.filial.chave_senha);
                if (serverPath.Equals(""))
                {
                    serverPath = server;
                }
                if ((serverPath.IndexOf("modulos") < 0) && (serverPath.IndexOf("Account") < 0))
                {
                    docNav = new XPathDocument(serverPath + "../modulos/modulos.xml");
                }
                else
                {
                    if (serverPath.IndexOf("Account") >= 0)
                    {
                        docNav = new XPathDocument(serverPath + "../../modulos/modulos.xml");
                    }
                }
               
            }
            catch (Exception)
            {


            }
        }
        public static void zeraPath()
        {
            serverPath = "";
        }

        public Menus() { }
        public String versao()
        {


            String strVersao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString();
            strVersao += "." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
            strVersao += "." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();
            String correcao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString().Trim();
            if (!correcao.Equals("") && !correcao.Equals("0"))
            {
                if (System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision > 1000)
                {
                    int BetaVersao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision - 1000;
                    strVersao += ".b" + BetaVersao.ToString().PadLeft(2, '0');
                }
                else
                {
                    strVersao += ".c" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString().PadLeft(2, '0');
                }
            }


            String versaoAtual = "";
            if (strAtualVersao.Equals(""))
            {
                strAtualVersao = Conexao.retornaUmValor("Select top 1 versao_atual from filial ", null);
            }

            versaoAtual = strAtualVersao;
            if (!strVersao.Equals(versaoAtual))
            {
                Conexao.executarSql("update filial set ultima_versao='" + versaoAtual + "' , versao_atual='" + strVersao + "'");
                strAtualVersao = strVersao;
            }



            return "Versão:" + strVersao;



        }

        private ArrayList ordemMenus()
        {

          
            ArrayList aryMenus = new ArrayList();

            rsMod.MoveToRoot();
            rsMod.MoveToFirstChild();

            rsMod.MoveToChild("ordem", "");
            rsMod.MoveToFirstChild();
            do
            {
                aryMenus.Add(rsMod.Value);
            } while (rsMod.MoveToNext());

            return aryMenus;
        }


        public bool moduloAtivo(String nomeModulo)
        {

            rs = docNav.CreateNavigator();
            rs.MoveToChild("modulos", "");
            rs.MoveToFirstChild();
            do
            {
                if (rs.Name.Equals("a") || rs.Name.Equals("A"))
                {
                    try
                    {
                        //if (this.user.adm())
                            return true;

                        //return senha.ModuloAcesso(nomeModulo);
                    }
                    catch (Exception)
                    {
                    }

                }

            } while (rs.MoveToNext());



            return false;

       
        }
        public List<ModuloSenha> modulosSenha()
        {

            List<ModuloSenha> modulos = new List<ModuloSenha>();
            rs = docNav.CreateNavigator();
            rs.MoveToChild("modulos", "");
            rs.MoveToFirstChild();
            do
            {
                if (rs.Name.Equals("a") || rs.Name.Equals("A"))
                {
                    try
                    {
                        ModuloSenha m = new ModuloSenha()
                        {
                            Nome = rs.Value,
                            Acesso = false
                        };
                        modulos.Add(m);
                    }
                    catch (Exception)
                    {
                    }

                }

            } while (rs.MoveToNext());



            return modulos;

        }

        public void montarMenu(Menu mprincipal)
        {
            try
            {

                MenuItem outros = new MenuItem("+");
              


                rs = docNav.CreateNavigator();
                rs.MoveToChild("modulos", "");
                rs.MoveToFirstChild();
                do
                {
                    if (rs.Name.Equals("a") || rs.Name.Equals("A"))
                    {

                        try
                        {
                            //if (senha.ModuloAcesso(rs.Value))
                            //{
                                if (mprincipal.Items.Count < 9)
                                    mprincipal.Items.Add(carregaMenu(rs.Value));
                                else
                                    outros.ChildItems.Add(carregaMenu(rs.Value));
                            //}
                        }
                        catch (Exception)
                        {
                        }

                    }

                } while (rs.MoveToNext());

                if (outros.ChildItems.Count > 0)
                    mprincipal.Items.Add(outros);
            }
            catch (Exception err)
            {

                String erro = err.Message;
            }
        }

        private MenuItem carregaMenu(string modulo)
        {
            MenuItem menuModulo = new MenuItem();
            try
            {



                docModulos = new XPathDocument(serverPath + "../modulos/" + modulo + "/mconfig.xml");
                rsMod = docModulos.CreateNavigator();

                rsMod.MoveToRoot();
                rsMod.MoveToFirstChild();

                rsMod.MoveToChild("nome", "");
                menuModulo.Text = rsMod.Value;

                ArrayList ordem = ordemMenus();




                for (int i = 0; i < ordem.Count; i++)
                {
                    rsMod.MoveToRoot();
                    rsMod.MoveToFirstChild();
                    try
                    {


                        rsMod.MoveToChild(ordem[i].ToString(), "");
                        MenuItem item = tMenu(modulo);

                        menuModulo.ChildItems.Add(item);

                    }
                    catch (Exception)
                    {
                    }
                }

            }
            catch (Exception)
            {

            }
            if (menuModulo.ChildItems.Count > 0)
                return menuModulo;
            else
                throw new Exception("Sem permissao");


        }

        private MenuItem tMenu(String modulo)
        {
            rsMod.MoveToFirstChild();
            ArrayList subMenus = new ArrayList();
            MenuItem item = new MenuItem();
            do
            {
                switch (rsMod.Name)
                {

                    case "codigo":
                        if (rsMod.Value.Equals("C031"))//promocoes
                        {
                            if (user.filial.pdv != 2) //só permitido para o Soldi PDV 2.0
                                throw new Exception("Sem Acesso");
                        }
                        if (!user.acesso(rsMod.Value))
                        {
                            throw new Exception("Sem Acesso");
                        }

                        item.Value = rsMod.Value;
                        break;
                    case "nome":
                        item.Text = rsMod.Value;
                        break;
                    case "page":
                        item.NavigateUrl = "~/modulos/" + modulo + "/pages/" + rsMod.Value + (rsMod.Value.IndexOf("?") > 0 ? "&" : "?") + "tela=" + item.Value;
                        break;
                    case "imgMenu":
                        if (!rsMod.Value.Equals(""))
                        {
                            item.ImageUrl = "~/modulos/" + modulo + "/imgs/" + rsMod.Value;
                        }


                        break;
                    case "subMenu":
                        rsMod.MoveToFirstChild();
                        do
                        {
                            subMenus.Add(rsMod.Name);
                        } while (rsMod.MoveToNext());
                        break;
                }
            } while (rsMod.MoveToNext());

            foreach (String strSubMenu in subMenus)
            {
                try
                {

                    rsMod.MoveToRoot();
                    rsMod.MoveToFirstChild();
                    rsMod.MoveToChild(strSubMenu, "");
                    MenuItem sbitem = tMenu(modulo);

                    item.ChildItems.Add(sbitem);
                }
                catch (Exception)
                {

                }

            }

            if (subMenus.Count > 0 && item.ChildItems.Count == 0)
                throw new Exception();

            return item;
        }

        public void carregaInical(Panel panel)
        {
            try
            {

                Panel pleft = new Panel();
                pleft.CssClass = "panel";

                Panel pRigth = new Panel();
                pRigth.CssClass = "panel";

                panel.Controls.Add(pleft);
                panel.Controls.Add(pRigth);

                rs = docNav.CreateNavigator();
                rs.MoveToChild("modulos", "");
                rs.MoveToFirstChild();

                do
                {
                    if (rs.Name.Equals("a") || rs.Name.Equals("A"))
                    {
                        try
                        {
                            //if (senha.ModuloAcesso(rs.Value))
                            //{
                                if (pleft.Controls.Count == pRigth.Controls.Count)
                                    pleft.Controls.Add(inicialModulo(rs.Value));
                                else
                                    pRigth.Controls.Add(inicialModulo(rs.Value));
                            //}
                        }
                        catch (Exception) { }
                    }

                } while (rs.MoveToNext());

            }
            catch (Exception)
            {


            }



        }
        public Panel inicialModulo(String modulo)
        {

            
            docModulos = new XPathDocument(serverPath + "../modulos/" + modulo + "/mconfig.xml");
            rsMod = docModulos.CreateNavigator();

            rsMod.MoveToRoot();
            rsMod.MoveToFirstChild();
            rsMod.MoveToChild("inicial", "");
            if (!rsMod.Value.Equals("true"))
            {
                throw new Exception("Não habilitado");
            }


            rsMod.MoveToRoot();
            rsMod.MoveToFirstChild();

            //Sinto Muito Me Perdoe Agradeço Eu Te Amo.

            rsMod.MoveToChild("nome", "");
            Panel pn1 = new Panel();
            pn1.CssClass = "panelInterno";
            Label title = new Label();
            title.Text = rsMod.Value;
            title.CssClass = "cabMenu";


            pn1.Controls.Add(title);

            ArrayList ordem = ordemMenus();

            ImagensInicial(pn1,modulo, ordem);

            if (pn1.Controls.Count > 1)
            {

                return pn1;
            }
            else
            {
                throw new Exception("Sem Acesso");
            }
        }

        private void ImagensInicial(Panel pn1 ,String modulo ,ArrayList ordem)
        {
           
            for (int i = 0; i < ordem.Count; i++)
            {
                ArrayList subMenus = new ArrayList();
                rsMod.MoveToRoot();
                rsMod.MoveToFirstChild();

                try
                {

                    rsMod.MoveToChild(ordem[i].ToString(), "");
                    rsMod.MoveToFirstChild();

                    Panel pn2 = new Panel();
                    pn2.CssClass = "imgButton";

                    Label lblbtn = new Label();

                    lblbtn.CssClass = "titulobtn";
                    pn2.Controls.Add(lblbtn);

                    ImageButton img = new ImageButton();

                    img.Width = 70;
                    pn2.Controls.Add(img);
                    String codigo = "";
                    do
                    {


                        switch (rsMod.Name)
                        {

                            case "codigo":
                                if (rsMod.Value.Equals("C031"))//promocoes
                                {
                                    if (user.filial.pdv != 2) //só permitido para o Soldi PDV 2.0
                                        throw new Exception("Sem Acesso");
                                }
                                if (!user.inicial(rsMod.Value))
                                {
                                    throw new Exception("Sem Acesso");
                                }
                                codigo = rsMod.Value;
                                break;
                            case "nome":
                                lblbtn.Text = rsMod.Value;
                                break;
                            case "page":
                                img.PostBackUrl = "~/modulos/" + modulo + "/pages/" + rsMod.Value + (rsMod.Value.IndexOf("?") > 0 ? "&" : "?") + "tela=" + codigo;
                                break;
                            case "img":
                                if (!rsMod.Value.Equals(""))
                                {
                                    img.ImageUrl = "~/modulos/" + modulo + "/imgs/" + rsMod.Value;

                                }
                                else
                                {
                                    img.ImageUrl = "~/img/form.png";
                                }
                                break;
                            case "inicial":
                                //if (!rsMod.Value.Equals("true"))
                                //{
                                //    throw new Exception("Não habilitado");
                                //}
                                break;
                            case "subMenu":
                                rsMod.MoveToFirstChild();
                                do
                                {
                                    subMenus.Add(rsMod.Name);
                                } while (rsMod.MoveToNext());
                                break;
                        }

                    } while (rsMod.MoveToNext());
                    if(subMenus.Count==0)
                        pn1.Controls.Add(pn2);
                }
                catch (Exception)
                {

                }

                if (subMenus.Count > 0)
                {
                    ImagensInicial(pn1, modulo, subMenus);
                }



                

            }
            
        }


        //================================
        // carregar permissoes 
        //================================

        public void carregaPermissoes(Panel panel, usuarios_webDAO usuario, bool carregarValores)
        {
            if (docNav != null)
            {
                panel.Controls.Clear();
                rs = docNav.CreateNavigator();
                rs.MoveToChild("modulos", "");
                rs.MoveToFirstChild();
                do
                {
                    if (rs.Name.Equals("a") || rs.Name.Equals("A"))
                    {
                        try
                        {
                            panel.Controls.Add(permissoesModulo(rs.Value, usuario, carregarValores));
                        }
                        catch (Exception) { }
                    }

                } while (rs.MoveToNext());


            }


        }

        public Panel carregaTelaPermissao(String tela, String modulo, bool carregarValores, usuarios_webDAO usuario)
        {

            rsMod.MoveToChild(tela, "");
            rsMod.MoveToFirstChild();
            ArrayList subMenus = new ArrayList();
            Panel pn2 = new Panel();
            pn2.CssClass = "framepermissao";



            Label lblbtn = new Label();

            lblbtn.CssClass = "tituloPermissoes";



            pn2.Controls.Add(lblbtn);
            ImageButton img = new ImageButton();

            img.Width = 40;
            img.ImageUrl = "~/img/form.png";
            pn2.Controls.Add(img);


            String codigo = "";
            do
            {


                switch (rsMod.Name)
                {

                    case "codigo":
                        codigo = rsMod.Value;
                        break;
                    case "nome":
                        lblbtn.Text = rsMod.Value;
                        break;
                    case "img":
                        if (rsMod.Value.Equals(""))
                        {
                            img.ImageUrl = "~/img/form.png";
                        }
                        else
                        {
                            img.ImageUrl = "~/modulos/" + modulo + "/imgs/" + rsMod.Value;
                        }
                        break;
                    case "subMenu":
                        rsMod.MoveToFirstChild();
                        do
                        {
                            subMenus.Add(rsMod.Name);
                        } while (rsMod.MoveToNext());
                        break;

                }

            } while (rsMod.MoveToNext());

            if (subMenus.Count > 0)
            {
                img.Visible = false;

                foreach (String strSubMenu in subMenus)
                {
                    rsMod.MoveToRoot();
                    rsMod.MoveToFirstChild();
                    // rsMod.MoveToChild(strSubMenu, "");
                    Panel sbitem = carregaTelaPermissao(strSubMenu, modulo, carregarValores, usuario);

                    pn2.Controls.Add(sbitem);

                }
            }
            else
            {
                CheckBox chkIncluir = new CheckBox();
                chkIncluir.Text = "Incluir";
                chkIncluir.ID = codigo + "_" + lblbtn.Text + "__Incluir";
                pn2.Controls.Add(chkIncluir);

                CheckBox chkEditar = new CheckBox();
                chkEditar.Text = "Editar";
                chkEditar.ID = codigo + "_" + lblbtn.Text + "__Editar";
                pn2.Controls.Add(chkEditar);

                CheckBox chkExcluir = new CheckBox();
                chkExcluir.Text = "Excluir";
                chkExcluir.ID = codigo + "_" + lblbtn.Text + "__Excluir";
                pn2.Controls.Add(chkExcluir);

                CheckBox chkVisualizar = new CheckBox();
                chkVisualizar.Text = "Menu";
                chkVisualizar.ID = codigo + "_" + lblbtn.Text + "__Visualizar";
                pn2.Controls.Add(chkVisualizar);

                CheckBox chkAdm = new CheckBox();
                chkAdm.Text = "Adm";
                chkAdm.ID = codigo + "_" + lblbtn.Text + "__Adm";
                pn2.Controls.Add(chkAdm);

                CheckBox chkInicial = new CheckBox();
                chkInicial.Text = "Inicial";
                chkInicial.ID = codigo + "_" + lblbtn.Text + "__tela_inicial";
                pn2.Controls.Add(chkInicial);


                if (carregarValores)
                {
                    SqlDataReader rsTela = Conexao.consulta("Select * from usuarios_web_telas where cod='" + codigo + "' and usuario=" + usuario.id, user, true);
                    if (rsTela.Read())
                    {
                        chkIncluir.Checked = (rsTela["incluir"].ToString().Equals("1") ? true : false);
                        chkEditar.Checked = (rsTela["editar"].ToString().Equals("1") ? true : false);
                        chkExcluir.Checked = (rsTela["excluir"].ToString().Equals("1") ? true : false);
                        chkVisualizar.Checked = (rsTela["visualizar"].ToString().Equals("1") ? true : false);
                        chkAdm.Checked = (rsTela["adm"].ToString().Equals("1") ? true : false);
                        chkInicial.Checked = (rsTela["tela_inicial"].ToString().Equals("1") ? true : false);

                    }
                    if (rsTela != null)
                        rsTela.Close();
                }
            }
            return pn2;
        }


        public Panel permissoesModulo(String modulo, usuarios_webDAO usuario, bool carregarValores)
        {

            docModulos = new XPathDocument(serverPath + "../modulos/" + modulo + "/mconfig.xml");
            rsMod = docModulos.CreateNavigator();

            rsMod.MoveToRoot();
            rsMod.MoveToFirstChild();
            rsMod.MoveToChild("inicial", "");
            if (!rsMod.Value.Equals("true"))
            {
                throw new Exception("Não habilitado");
            }


            rsMod.MoveToRoot();
            rsMod.MoveToFirstChild();


            rsMod.MoveToChild("nome", "");
            Panel pn1 = new Panel();
            pn1.CssClass = "panel";
            Label title = new Label();
            title.Text = rsMod.Value;
            title.CssClass = "cabMenu";


            pn1.Controls.Add(title);

            ArrayList ordem = ordemMenus();



            for (int i = 0; i < ordem.Count; i++)
            {
                rsMod.MoveToRoot();
                rsMod.MoveToFirstChild();

                try
                {


                    pn1.Controls.Add(carregaTelaPermissao(ordem[i].ToString(), modulo, carregarValores, usuario));

                }
                catch (Exception)
                {

                }


            }
            if (pn1.Controls.Count > 1)
            {

                return pn1;
            }
            else
            {
                throw new Exception("Sem Acesso");
            }
        }


    }
}