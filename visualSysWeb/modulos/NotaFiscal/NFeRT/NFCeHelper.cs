using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace visualSysWeb.modulos.NotaFiscal.NFeRT
{
    public class NFCeHelper
    {
        // Serializa objeto NFC-e para XML (mantendo whitespaces)
        public static string SerializeToXML<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false), // UTF-8 sem BOM
                Indent = false,
                OmitXmlDeclaration = false
            };

            using (StringWriter sw = new StringWriter())
            using (XmlWriter xw = XmlWriter.Create(sw, settings))
            {
                serializer.Serialize(xw, obj);
                return sw.ToString();
            }
        }

        // Assina apenas o elemento <infNFe> usando o certificado
        public static XmlDocument SignXML(string xml, X509Certificate2 cert)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(xml);

            // Pega o nó infNFe
            XmlElement nfeNode = (XmlElement)doc.GetElementsByTagName("NFe")[0];
            XmlElement infNFe = (XmlElement)doc.GetElementsByTagName("infNFe")[0];

            if (infNFe == null)
                throw new Exception("Elemento <infNFe> não encontrado!");

            // Cria assinatura
            SignedXml signedXml = new SignedXml(infNFe);
            signedXml.SigningKey = cert.PrivateKey;

            // Reference URI deve bater com o Id do infNFe
            Reference reference = new Reference();
            reference.Uri = "#" + infNFe.GetAttribute("Id");

            // Transformação padrão para canonicalization
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1"; //novo
            reference.AddTransform(env);

            signedXml.AddReference(reference);

            // Método de assinatura SHA1 ou SHA256 (depende do certificado e SEFAZ)
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;

            signedXml.KeyInfo.AddClause(new KeyInfoX509Data(cert));

            // Computa a assinatura
            signedXml.ComputeSignature();

            // Anexa a assinatura ao elemento infNFe
            XmlElement xmlDigitalSignature = signedXml.GetXml();
            //infNFe.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
            nfeNode.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
            return doc;
        }

        // Salva XML assinado em UTF-8 sem BOM
        public static void SaveXml(XmlDocument doc, string caminho)
        {
            using (var writer = new XmlTextWriter(caminho, new UTF8Encoding(false)))
            {
                writer.Formatting = Formatting.None;
                doc.WriteTo(writer);
            }
        }
    }
}
