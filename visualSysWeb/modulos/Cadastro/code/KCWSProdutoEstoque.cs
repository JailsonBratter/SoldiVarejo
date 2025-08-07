using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Xml;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class KCWSProdutoEstoque
    {
        private string Acao = "";
        private User usr = null;
        public KCWSProdutoEstoque(string strAcao, string CodigoInternoProduto, int Estoque,User usr)
        {
            this.usr = usr;
            Acao = strAcao;

            String A1 = Funcoes.valorParametro("KCW_KEY_A1", usr);
            String A2 = Funcoes.valorParametro("KCW_KEY_A2", usr);
            if (Acao.Equals("Salvar"))
            {
                Salvar(A1, A2, CodigoInternoProduto, Estoque);
            }
        }
        public HttpWebRequest CreateSOAPProduto()
        {
            String urlServer = Funcoes.valorParametro("KCW_URL_SERVER", usr);
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(urlServer+"estoque.asmx");
            Req.Headers.Add("SOAPAction:http://www.ikeda.com.br/" + Acao);
            Req.ContentType = "text/xml; charset=utf-8";
            Req.Accept = "text/xml";
            Req.Method = "POST";
            return Req;
        }
        private void Salvar(string A1, string A2, string CodigoInternoProduto, int Estoque)
        {
            string strXML = "";
            HttpWebRequest request = CreateSOAPProduto();
            XmlDocument SOAPReqBody = new XmlDocument();
            strXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            strXML += "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">";
            strXML += "<soap:Header>";
            strXML += "<clsSoapHeader xmlns=\"http://www.ikeda.com.br\">";
            strXML += "<A1>" + A1 + "</A1>";
            strXML += "<A2>" + A2 + "</A2>";
            strXML += "</clsSoapHeader></soap:Header>";
            strXML += "<soap:Body>";
            strXML += "<" + Acao + " xmlns=\"http://www.ikeda.com.br\">";
            strXML += "<LojaCodigo>0</LojaCodigo>";
            strXML += "<CodigoProdutoInterno>"+CodigoInternoProduto+"</CodigoProdutoInterno>";
            strXML += "<PartNumber>" + CodigoInternoProduto + "</PartNumber>";
            strXML += "<QtdEstoque>"+ Estoque.ToString()  +"</QtdEstoque>";
            strXML += "<QtdMinima>0</QtdMinima>";
            strXML += "<TipoAlteracao>3</TipoAlteracao>";
            strXML += "</" + Acao + ">";
            strXML += "</soap:Body>";
            strXML += "</soap:Envelope>";
            SOAPReqBody.LoadXml(strXML);
            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    var ServiceResult = rd.ReadToEnd();
                    //Console.WriteLine(ServiceResult);
                    //Console.ReadLine();
                }
            }
        }
    }
}