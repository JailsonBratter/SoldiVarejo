using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Dispositivos.pages
{
    public partial class EtiquetaA4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"]==null)
            {
                Response.Redirect("~");
                return;
            }

            divEtiquetas.InnerHtml = "";
            ArrayList imprimir = (ArrayList)Session["imprimir"];
            if (imprimir != null)
            {
                foreach (ArrayList linha in imprimir)
                {
                    int qtde = 0;
                    int.TryParse(linha[5].ToString(), out qtde);
                    for (int i = 0; i < qtde; i++)
                    {
                        String plu = linha[0].ToString();
                        String strDescricao = linha[2].ToString().Trim();
                        String referencia = Conexao.retornaUmValor("Select Ref_fornecedor from mercadoria where plu ='" + plu + "'", null);
                        if (strDescricao.Length > 25)
                        {
                            strDescricao = strDescricao.Substring(0, 25);
                        }

                        Decimal vPrecoPromocao = 0;
                        Decimal vPreco = 0;
                        Decimal.TryParse(linha[4].ToString(), out vPrecoPromocao);
                        if (vPrecoPromocao > 0)
                        {
                            vPreco = vPrecoPromocao;
                        }
                        else
                        {
                            Decimal.TryParse(linha[3].ToString(), out vPreco);
                        }
                        String ean = linha[1].ToString();
                        String und = linha[7].ToString();

                        divEtiquetas.InnerHtml += "<div style=\"width: 320px; height: 120px; float:left; border-style:solid; border-width:1px; margin-left:10px; margin-bottom:25px; padding:5px; \"> ";
                        divEtiquetas.InnerHtml += " <div style = \"font-size:18px;  font-weight:bold;float:left;\">" + strDescricao + "</div>";
                        divEtiquetas.InnerHtml += "    <br/>";
                         divEtiquetas.InnerHtml += "    <div style = \"width:100%; float:left;\" >";
                        divEtiquetas.InnerHtml += "         <div style = \"width:100%; float:left;\" >";

                        divEtiquetas.InnerHtml += "           <div style = \"font-size:30px; font-weight:bold; float:right; padding:2px; \" > R$ " + vPreco.ToString("N2") + "</div>";
                        divEtiquetas.InnerHtml += "           <div style = \"font-size:30px; font-weight:normal; float:left; padding:2px; \" > "+und+" </div>";
                        divEtiquetas.InnerHtml += "          </div >";

                        divEtiquetas.InnerHtml += "           <div style = \"font-size:15px; font-weight:normal;\" >Ref:"+ referencia + " </div>";
                        divEtiquetas.InnerHtml += "           <div style = \"font-size:20px; font-weight:normal;\" > "+ean+" - "+plu+" - "+DateTime.Now.ToString("yyMMdd")+" </div>";
                        divEtiquetas.InnerHtml += "   </div >";
                        divEtiquetas.InnerHtml += " </div > ";                           
                        divEtiquetas.InnerHtml += "</div > ";                           
                            
                            
                            
                        //"<div style=\" width:320px; height:120px; float:left; border-style:solid; border-width:1px; margin-left:10px; margin-bottom:25px; \">" +
                        //"<div style=\"font-size:18px;  font-weight:bold;\">" + strDescricao + "</div> " +
                        //"<br /> " +
                        //"<div style=\"width:100%\">" +
                        //"<div style=\"float:right; font-weight:bold; font-size:40px\">" + vPreco.ToString("N2") + "</div><br /><div style=\"float:right; font-size:20px; margin-right:20px;\">R$  </div>" +
                        //"</div>" +
                        //"<br />" +
                        //"<br />" +
                        //"<div style=\"font-size:20px; font-weight:bold;\">" + ean + "</div>" +
                        //"</div>";
                    }
                }
            }

        }
    }
}