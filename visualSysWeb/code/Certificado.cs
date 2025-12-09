using DFe.Classes.Assinatura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using NFe.Utils.Assinatura;
using System.IO;
using System.Web;
using visualSysWeb.dao;

namespace visualSysWeb.code
{
    public class Certificado
    {
        public static X509Certificate2 certificado;
        
        public static void AssinarXML(XmlDocument xmlDoc, int tpEmissao = 1, filialDAO Loja = null)
        {
            certificado =  new X509Certificate2(Loja.certificado_arquivo, Loja.certificado_senha);

            if (tpEmissao == 9)
            {
                xmlDoc.PreserveWhitespace = true;
            }

            //Processo para checar se há o certificado
            //incluído por Jalson em 28/10/2025 14:15
            try
            {
                //throw new Exception("ASSINAXML(). Dados certificado: " + certificado.SerialNumber.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("ASSINARXML(). Erro ao tentar pegar o serial do certificado :" + ex.Message);
            }


            //var assinaturaXml = Assinador.ObterAssinatura(xmlDoc, "infNFe", certificado);

            var signedXml = new SignedXml(xmlDoc);
            signedXml.SigningKey = certificado.GetRSAPrivateKey();

            //Alterado para SHA1 de acordo com o ChatGPT
            // Define explicitamente o algoritmo de assinatura SHA1
            signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";


            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");


            XmlNode node = xmlDoc.SelectSingleNode("//nfe:infNFe", ns);
            var id = node.Attributes["Id"].Value;


            // Referência à tag que será assinada (geralmente infNFe)
            var reference = new System.Security.Cryptography.Xml.Reference();
            reference.Uri = "#" + id; // Deve bater com o atributo ID de <infNFe>
                                      //Alterado para SHA1 de acordo com o ChatGPT
                                      // Define explicitamente o algoritmo de digest SHA1
            reference.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";
            // Enveloping transforms
            var env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            var c14 = new XmlDsigC14NTransform();
            reference.AddTransform(c14);

            signedXml.AddReference(reference);

            // Informações do certificado
            var keyInfo = new System.Security.Cryptography.Xml.KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();

            // Gera o XML da assinatura
            var xmlDigitalSignature = signedXml.GetXml();

            // Insere no documento (logo após infNFe)
            var nodePai = xmlDoc.GetElementsByTagName("NFe")[0];
            nodePai.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
        }
        public static string AssinarXMLD(string xmlDocRec, int tpEmissao = 1)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.LoadXml(xmlDocRec);

            //var assinaturaXml = Assinador.ObterAssinatura(xmlDoc, "infNFe", certificado);

            var signedXml = new SignedXml(xmlDoc);
            signedXml.SigningKey = certificado.GetRSAPrivateKey();

            //Alterado para SHA1 de acordo com o ChatGPT
            // Define explicitamente o algoritmo de assinatura SHA1
            signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            ns.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");


            XmlNode node = xmlDoc.SelectSingleNode("//nfe:infNFe", ns);
            var id = node.Attributes["Id"].Value;

            // Referência à tag que será assinada (geralmente infNFe)
            var reference = new System.Security.Cryptography.Xml.Reference();
            reference.Uri = "#" + id; // Deve bater com o atributo ID de <infNFe>
                                      //Alterado para SHA1 de acordo com o ChatGPT
                                      // Define explicitamente o algoritmo de digest SHA1
            reference.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";
            // Enveloping transforms
            var env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            var c14 = new XmlDsigC14NTransform();
            reference.AddTransform(c14);

            signedXml.AddReference(reference);

            // Informações do certificado
            var keyInfo = new System.Security.Cryptography.Xml.KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();

            // Gera o XML da assinatura
            var xmlDigitalSignature = signedXml.GetXml();

            node.ParentNode.InsertAfter(xmlDoc.ImportNode(xmlDigitalSignature, true), node);

            //using (var sw = new StringWriter())
            //using (var xw = XmlWriter.Create(sw, new XmlWriterSettings { Encoding = Encoding.UTF8, OmitXmlDeclaration = false }))
            //{
            //    xmlDoc.Save(xw);
            //    return sw.ToString();
            //}
            using (var ms = new MemoryStream())
            using (var xw = XmlWriter.Create(ms, new XmlWriterSettings { Encoding = Encoding.UTF8, OmitXmlDeclaration = false, Indent = false }))
            {
                xmlDoc.Save(xw);
                xw.Flush();
                return Encoding.UTF8.GetString(ms.ToArray());
            }

        }
        public static XmlDocument assinaXMLTpEmis9(string xml, X509Certificate2 cert)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(xml);

            // Pega o nó infNFe
            XmlElement infNFe = (XmlElement)doc.GetElementsByTagName("infNFe")[0];
            if (infNFe == null)
                throw new Exception("Elemento <infNFe> não encontrado!");

            // Cria assinatura
            SignedXml signedXml = new SignedXml(infNFe);
            signedXml.SigningKey = cert.PrivateKey;

            // Reference URI deve bater com o Id do infNFe
            System.Security.Cryptography.Xml.Reference reference = new System.Security.Cryptography.Xml.Reference();
            reference.Uri = "#" + infNFe.GetAttribute("Id");

            // Transformação padrão para canonicalization
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            signedXml.AddReference(reference);

            // Método de assinatura SHA1 ou SHA256 (depende do certificado e SEFAZ)
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;

            signedXml.KeyInfo.AddClause(new KeyInfoX509Data(cert));

            // Computa a assinatura
            signedXml.ComputeSignature();

            // Anexa a assinatura ao elemento infNFe
            XmlElement xmlDigitalSignature = signedXml.GetXml();
            infNFe.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

            return doc;
        }
    }
}