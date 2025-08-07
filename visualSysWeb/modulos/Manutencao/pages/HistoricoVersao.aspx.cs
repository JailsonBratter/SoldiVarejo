using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace visualSysWeb.modulos.Manutencao.pages
{
    public partial class HistoricoVersao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Encoding utf8 = new UTF8Encoding(true);
                using (StreamReader r = new StreamReader(@"/Bratter/Soldi/modulos/Manutencao/pages/historicoVersao.json", utf8))
                {
                    string json = r.ReadToEnd();
                    historicoVersao historico = new historicoVersao();
                    historico = JsonConvert.DeserializeObject<historicoVersao>(json);
                    TreeNode menu = new TreeNode("Histórico de versões");
                    
                    foreach (Historico h in historico.Historico)
                    {
                        TreeNode n1 = new TreeNode(h.Versao);
                        foreach (Conteudo c in h.Conteudo)
                        {
                            //Correções
                            TreeNode n2 = new TreeNode("Correções");
                            for (int i = 0; i < c.Correcoes.Count; i++)
                            {
                                TreeNode n3 = new TreeNode(c.Correcoes[i].dados);
                                n2.ChildNodes.Add(n3);
                            }

                            //Novidades
                            TreeNode n4 = new TreeNode("Novidades");
                            for (int i = 0; i < c.Novidades.Count; i++)
                            {
                                TreeNode n5 = new TreeNode(c.Novidades[i].dados);
                                n4.ChildNodes.Add(n5);
                            }
                            n1.ChildNodes.Add(n2);
                            n1.ChildNodes.Add(n4);
                        }
                        menu.ChildNodes.Add(n1);
                    }
                    tvw01.Nodes.Add(menu);
                }
            }
        }

    }

    public class historicoVersao
    {
        public List<Historico> Historico { get; set; }
    }

    public class Historico
    {
        public string Versao { get; set; }
        public List<Conteudo> Conteudo { get; set; }
    }

    public class Conteudo
    {
        public List<Novidade> Novidades { get; set; }
        public List<Correcao> Correcoes { get; set; }
    }

    public class Novidade
    {
        public string dados { get; set; }
    }

    public class Correcao
    {
        public string dados { get; set; }
    }

}