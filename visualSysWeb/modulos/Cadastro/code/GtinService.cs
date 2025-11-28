using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace visualSysWeb.modulos.Cadastro.code
{
    public class GtinService
    {
        private readonly string _url;

        public GtinService(bool homologacao = false)
        {
            _url = homologacao
                ? "https://hom.sefazvirtual.rs.gov.br/ws/ccgConsGtin/ccgConsGtin.asmx"
                : "https://dfe-servico.svrs.rs.gov.br/ws/ccgConsGTIN/ccgConsGTIN.asmx";
        }

        public CcgResult ConsultarGtin(string gtin)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var cert =  new X509Certificate2(@"C:\JGA-INFO\JGASoluções\Certificados\JGASolucoes2025.pfx", "123456", X509KeyStorageFlags.MachineKeySet |
    X509KeyStorageFlags.PersistKeySet |
    X509KeyStorageFlags.Exportable);
            string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                 xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                 xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
  <soap12:Body>
    <ccgConsGTIN xmlns=""http://www.portalfiscal.inf.br/nfe/wsdl/ccgConsGtin"">
      <nfeDadosMsg><consGTIN xmlns=""http://www.portalfiscal.inf.br/nfe"" versao=""1.00""><GTIN>{gtin}</GTIN></consGTIN></nfeDadosMsg>
    </ccgConsGTIN>
  </soap12:Body>
</soap12:Envelope>";
            var request = (HttpWebRequest)WebRequest.Create(_url);
            request.ClientCertificates.Add(cert);
            request.Headers.Add("SOAPAction", "http://www.portalfiscal.inf.br/nfe/wsdl/ccgConsGTIN/ccgConsGTIN");
            request.Method = "POST";
            request.ContentType = "application/soap+xml; charset=utf-8";
            request.Accept = "application/soap+xml";
            request.Timeout = 60000; // 60 segundos

            var encoding = new UTF8Encoding(false);
            byte[] data = encoding.GetBytes(soapEnvelope);

            // Enviar o XML para o webservice
            try
            {
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                // Ler a resposta
                using (WebResponse response = request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseXml = reader.ReadToEnd();
                    return ParseCcgResponse(responseXml);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public CcgResult ParseCcgResponse(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("soap", "http://www.w3.org/2003/05/soap-envelope");
            nsmgr.AddNamespace("resp", "http://www.portalfiscal.inf.br/nfe/wsdl/ccgConsGtin");
            nsmgr.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");

            XmlNode retNode = doc.SelectSingleNode(
                "/soap:Envelope/soap:Body/resp:ccgConsGTINResponse/resp:nfeResultMsg/nfe:retConsGTIN", nsmgr
            );

            if (retNode == null) return null;

            CcgResult result = new CcgResult
            {
                VerAplic = retNode["verAplic"]?.InnerText,
                CStat = retNode["cStat"]?.InnerText,
                XMotivo = retNode["xMotivo"]?.InnerText,
                DhResp = DateTime.Parse(retNode["dhResp"]?.InnerText),
                GTIN = retNode["GTIN"]?.InnerText,
                TpGTIN = retNode["tpGTIN"]?.InnerText,
                XProd = retNode["xProd"]?.InnerText,
                NCM = retNode["NCM"]?.InnerText,
                CEST = retNode["CEST"]?.InnerText
            };

            return result;
        }
    }

    public class CcgResult
    {
        public string VerAplic { get; set; }
        public string CStat { get; set; }
        public string XMotivo { get; set; }
        public DateTime DhResp { get; set; }
        public string GTIN { get; set; }
        public string TpGTIN { get; set; }
        public string XProd { get; set; }
        public string NCM { get; set; }
        public string CEST { get; set; }
    }
}