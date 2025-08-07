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
    public class KCWCategorias //: CategoriaSoap
    {
        public int LojaCodigo { get; set; }
        public string CodigoInternoCategoria { get; set; }
        public string NomeCategoria { get; set; }
        public int CategoriaStatus { get; set; }
        public int CategoriaPai { get; set; }
        private User usr = null;
        public bool Inserir()
        {
            grupoDAO grupo = new grupoDAO("");
            SubGrupoDAO subgrupo = new SubGrupoDAO("");
            DepartamentoDAO dpto = new DepartamentoDAO("");

            try
            {

                String A1 = Funcoes.valorParametro("KCW_KEY_A1", usr);
                String A2 = Funcoes.valorParametro("KCW_KEY_A2", usr);

                while (grupo.drGrupo.Read())
                {
                    InserirCategoria(A1, A2, 1, grupo.drGrupo["codigo_Grupo"].ToString(), grupo.drGrupo["Descricao_Grupo"].ToString().ToUpper(), 1, "");
                }

                while (subgrupo.drSubGrupo.Read())
                {
                    InserirCategoria(A1, A2, 1, subgrupo.drSubGrupo["codigo_SubGrupo"].ToString(), subgrupo.drSubGrupo["Descricao_SubGrupo"].ToString().ToUpper(), 1, subgrupo.drSubGrupo["codigo_Grupo"].ToString());
                }

                while (dpto.drDepartamento.Read())
                {
                    InserirCategoria(A1, A2, 1, dpto.drDepartamento["codigo_departamento"].ToString(), dpto.drDepartamento["Descricao_Departamento"].ToString().ToUpper(), 1, dpto.drDepartamento["Codigo_SubGrupo"].ToString());
                }
            }
            catch (Exception err)
            {

                throw err;
            }
            finally
            {
                if (grupo.drGrupo != null)
                    grupo.drGrupo.Close();
                
                if (subgrupo.drSubGrupo != null)
                    subgrupo.drSubGrupo.Close();

                if (dpto.drDepartamento != null)
                    dpto.drDepartamento.Close();


 
            }

            return true;
            /*
            HttpClient client = new HttpClient();
            try
            {
                client.BaseAddress = new System.Uri("http://34.232.249.168:8086//");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return true;
            }
            catch
            {
                return false;
            }
             * */
        }
        public KCWCategorias(User usr)
        {
            this.usr = usr;
        }

        public HttpWebRequest CreateSOAPCategoria()
        {
            String urlServer = Funcoes.valorParametro("KCW_URL_SERVER", usr);
            
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(urlServer+"categoria.asmx");
            Req.Headers.Add("SOAPAction:http://www.ikeda.com.br/Salvar");
            Req.ContentType = "text/xml; charset=utf-8";
            Req.Accept = "text/xml";
            Req.Method = "POST";
            return Req;
        }
        public void InserirCategoria(string A1, string A2, int loja, string CodigoCategoria, string NomeCategoria, int StatusCategoria, string CategoriaPai)
        {
            string strXML = "";
            HttpWebRequest request = CreateSOAPCategoria();
            XmlDocument SOAPReqBody = new XmlDocument();
            strXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            strXML += "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">";
            strXML += "<soap:Header>";
            strXML += "<clsSoapHeader xmlns=\"http://www.ikeda.com.br\">";
            strXML += "<A1>" + A1 + "</A1>";
            strXML += "<A2>" + A2 + "</A2>";
            strXML += "</clsSoapHeader></soap:Header><soap:Body><Salvar xmlns=\"http://www.ikeda.com.br\">";
            strXML += "<LojaCodigo>0</LojaCodigo>";
            strXML += "<CodigoInternoCategoria>" + CodigoCategoria + "</CodigoInternoCategoria>";
            strXML += "<NomeCategoria>"+NomeCategoria+"</NomeCategoria>";
            strXML += "<CategoriaStatus>"+StatusCategoria+"</CategoriaStatus>";
            strXML += "<CategoriaPai>"+CategoriaPai+"</CategoriaPai>";
            strXML += "</Salvar></soap:Body></soap:Envelope>";
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