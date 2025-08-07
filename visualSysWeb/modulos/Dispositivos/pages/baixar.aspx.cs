using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Dispositivos.pages
{
    public partial class baixar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (usr != null)
            {
                String desmarca = (String)Session["DesmarcaEtiqueta"];
                if (desmarca != null)
                {
                    ArrayList imprimir = (ArrayList)Session["imprimir"];
                    if (imprimir != null)
                    {
                        foreach (ArrayList linha in imprimir)
                        {
                            String sql = "";
                            if (linha[0].ToString().IndexOf("F") >= 0)
                                sql = " update familia set imprime_etiqueta=0 where codigo_familia ='" + linha[0].ToString().Substring(1).Trim() + "'";
                            else
                                sql = " update mercadoria set imprime_etiqueta =0 where plu ='" +linha[0].ToString()+"'";
                            Conexao.executarSql(sql);
                        }


                    }

                    
                }

                //Limpa Etiquetas marcadas com mais de 7 dias 
                String SqlLimpaEt = "update mercadoria set  imprime_etiqueta = 0 where  Data_Alteracao <'"+DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd")+"' and Imprime_etiqueta=1";
                Conexao.executarSql(SqlLimpaEt);

                String SqlLimpaFamilia = "update familia set  imprime_etiqueta = 0 where  codigo_familia in (select codigo_familia from mercadoria where Data_Alteracao <'" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "'and Codigo_familia is not null and Codigo_familia <>'' group by Codigo_familia) and familia.imprime_etiqueta=1  ";
                Conexao.executarSql(SqlLimpaFamilia);


                FileInfo fInfo = new FileInfo(Server.MapPath("") + "\\impressao\\Imprimir" + usr.getId().ToString() + ".txt");
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fInfo.Name + "\"");
                HttpContext.Current.Response.AddHeader("Content-Length", fInfo.Length.ToString());
                HttpContext.Current.Response.WriteFile(fInfo.FullName);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
            }
        }
    }
}

//Sinto Muito Me Perdoe Agradeço Eu Te Amo.