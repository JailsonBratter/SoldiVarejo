using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Cadastro.code
{
    public class WSCEP
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string ibge { get; set; }

        public WSCEP()
        {

        }

        public WSCEP(string CEP)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = (s, c, n, p) => { return true; };

            Uri uri = new Uri("https://viacep.com.br/ws/" + CEP + "/json/");

            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            
            //request.Method = "GET";


            string strResponseValue = String.Empty;
            try
            {


                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new ApplicationException("error code: " + response.StatusCode);
                    }
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                strResponseValue = reader.ReadToEnd();
                            }
                            if (!strResponseValue.Equals(""))
                            {
                                var cep = JsonConvert.DeserializeObject<WSCEP>(strResponseValue.ToString());
                                this.cep = cep.cep.Replace("-", "");
                                this.logradouro = Funcoes.RemoverAcentos(cep.logradouro).ToUpper();
                                this.complemento = Funcoes.RemoverAcentos(cep.complemento).ToUpper();
                                this.bairro = Funcoes.RemoverAcentos(cep.bairro).ToUpper();
                                this.localidade = Funcoes.RemoverAcentos(cep.localidade).ToUpper();
                                this.uf = Funcoes.RemoverAcentos(cep.uf).ToUpper();
                                this.ibge = cep.ibge;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}