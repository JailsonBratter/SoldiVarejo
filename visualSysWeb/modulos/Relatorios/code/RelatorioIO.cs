using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.dao;
using System.Web.UI.WebControls;


namespace visualSysWeb.modulos.Relatorios.code
{
    public  class RelatorioIO
    {
        String path;
        User usr = null;

        public RelatorioIO(String path)
        {
            this.path = path;

        }
        public RelatorioIO(String path, User usr)
        {
            this.path = path;
            this.usr = usr;
        }
        public void CriarPasta(String paste) { 
            if(!Directory.Exists(path+"../RelatoriosXml/"+paste)){
                Directory.CreateDirectory(path + "../RelatoriosXml/" + paste);
            }
   
        }

        public bool verificarPasta(String paste) {
            return Directory.Exists(path + "../RelatoriosXml/" + paste);
        }


        public bool verificaRelatorio(String rel)
        {
            return File.Exists(path +  rel.Replace(" ","_")+".xml");
        }
        public  void preencherDDL(RadioButtonList rdo,string paste)
        {
            DirectoryInfo dir = new DirectoryInfo(path + "../RelatoriosXml/" + paste);
            FileInfo[] relatorios = dir.GetFiles("*.xml");
           
            foreach (FileInfo rel in relatorios)
             {
                 string nome = rel.Name.Replace("_", " ");
                 nome = nome.Replace(".xml", "");
                if (usr.adm() || usr.telaPermissao(rel.Name.Replace(".xml", "")))
                {
                    rdo.Items.Add(nome);
                }
            }
            //Sinto Muito Me Perdoe Agradeço Eu Te Amo.

            
        }

        


   
    }
}