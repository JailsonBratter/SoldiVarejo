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
    public class KCWSProduto
    {
        private static string Acao = "";
        private MercadoriaDAO Produto = null;
        //private mercadoria_lojaDAO ProdutoLoja = null;
        public  User usr = null;
        public KCWSProduto(MercadoriaDAO merc, string strAcao,User usr)
        {
            this.usr= usr;
            Produto = merc;
            Acao = strAcao;
            String A1 = Funcoes.valorParametro("KCW_KEY_A1", usr);
            String A2 = Funcoes.valorParametro("KCW_KEY_A2", usr);

            switch (strAcao.ToUpper())
            {
                case "SALVAR":
                    Salvar(A1, A2);
                    break;
                case "EXCLUIR":
                    Excluir(A1, A2);
                    break;

            }
        }
        public  HttpWebRequest CreateSOAPProduto()
        {
            String urlServer = Funcoes.valorParametro("KCW_URL_SERVER", usr);
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(urlServer+"produto.asmx");
            Req.Headers.Add("SOAPAction:http://www.ikeda.com.br/" + Acao  );
            Req.ContentType = "text/xml; charset=utf-8";
            Req.Accept = "text/xml";
            Req.Method = "POST";
            return Req;
        }
        public void Salvar(string A1, string A2)
        {
            try
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

                strXML += "<CodigoInternoProduto>" + Produto.PLU + "</CodigoInternoProduto>";
                strXML += "<CodigoInternoFornecedor>" + Funcoes.RemoverAcentos(Produto.Marca).ToUpper().Trim() + "</CodigoInternoFornecedor>";
                strXML += "<NomeProduto>" + Produto.Descricao + "</NomeProduto>";
                strXML += "<TituloProduto>" + Produto.Descricao_resumida + "</TituloProduto>";
                strXML += "<SubTituloProduto>" + Produto.Descricao_resumida + "</SubTituloProduto>";
                strXML += "<DescricaoProduto>" + (Produto.Descricao_Comercial.Equals("") ? Produto.Descricao : Produto.Descricao_Comercial) + "</DescricaoProduto>";
                strXML += "<CaracteristicaProduto><![CDATA[" + Produto.strEcommerce + "]]></CaracteristicaProduto>";
                strXML += "<Texto1Produto></Texto1Produto>";
                strXML += "<Texto2Produto></Texto2Produto>";
                strXML += "<Texto3Produto></Texto3Produto>";
                strXML += "<Texto4Produto></Texto4Produto>";
                strXML += "<Texto5Produto></Texto5Produto>";
                strXML += "<Texto6Produto></Texto6Produto>";
                strXML += "<Texto7Produto></Texto7Produto>";
                strXML += "<Texto8Produto></Texto8Produto>";
                strXML += "<Texto9Produto></Texto9Produto>";
                strXML += "<Texto10Produto></Texto10Produto>";
                strXML += "<Numero1Produto>0</Numero1Produto>";
                strXML += "<Numero2Produto>0</Numero2Produto>";
                strXML += "<Numero3Produto>0</Numero3Produto>";
                strXML += "<Numero4Produto>0</Numero4Produto>";
                strXML += "<Numero5Produto>0</Numero5Produto>";
                strXML += "<Numero6Produto>0</Numero6Produto>";
                strXML += "<Numero7Produto>0</Numero7Produto>";
                strXML += "<Numero8Produto>0</Numero8Produto>";
                strXML += "<Numero9Produto>0</Numero9Produto>";
                strXML += "<Numero10Produto>0</Numero10Produto>";
                strXML += "<CodigoInternoEnquadramento></CodigoInternoEnquadramento>";
                strXML += "<ModeloProduto></ModeloProduto>";
                strXML += "<PesoProduto>" + (Produto.peso_liquido <= 0 ? 1 : Produto.peso_liquido).ToString().Replace(",",".") + "</PesoProduto>";
                strXML += "<PesoEmbalagemProduto>" + (Produto.peso_bruto <= 0 ? 1 : Produto.peso_bruto).ToString().Replace(",",".") + "</PesoEmbalagemProduto>";
                strXML += "<AlturaProduto>0</AlturaProduto>";
                strXML += "<AlturaEmbalagemProduto>0</AlturaEmbalagemProduto>";
                strXML += "<LarguraProduto>0</LarguraProduto>";
                strXML += "<LarguraEmbalagemProduto>0</LarguraEmbalagemProduto>";
                strXML += "<ProfundidadeProduto>0</ProfundidadeProduto>";
                strXML += "<ProfundidadeEmbalagemProduto>0</ProfundidadeEmbalagemProduto>";
                strXML += "<EntregaProduto>0</EntregaProduto>";
                strXML += "<QuantidadeMaximaPorVenda>999</QuantidadeMaximaPorVenda>";
                strXML += "<StatusProduto>"+(Produto.Ativo_Ecommerce?"1":"0")+"</StatusProduto>";
                strXML += "<TipoProduto>1</TipoProduto>";
                strXML += "<Presente>2</Presente>";
                strXML += "<PrecoCheioProduto>" + Produto.Preco.ToString().Replace(",", ".") + "</PrecoCheioProduto>";
                strXML += "<PrecoPor>" + Produto.Preco.ToString().Replace(",", ".") + "</PrecoPor>";
                strXML += "<PersonalizacaoExtra>2</PersonalizacaoExtra>";
                strXML += "<PersonalizacaoLabel></PersonalizacaoLabel>";
                strXML += "<ISBN></ISBN>";
                strXML += "<EAN13>"+Produto.eanPrimeiro+"</EAN13>";
                strXML += "<YouTubeCode></YouTubeCode>";

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
            catch (Exception err)
            {
                throw err;
            }
        }

        public void Excluir(string A1, string A2)
        {
            try
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
                strXML += "<Excluir xmlns=\"http://www.ikeda.com.br\">";
                strXML += "<LojaCodigo>0</LojaCodigo>";
                strXML += "<CodigoInternoProduto>" + Produto.PLU + "</CodigoInternoProduto>";
                strXML += "</Excluir>";
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
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}