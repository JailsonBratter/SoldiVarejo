using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace visualSysWeb.modulos.Cadastro.code
{
    public class ApiCardapio
    {
        HttpWebRequest http = null;
        public ApiCardapio(string url, string token)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            http = (HttpWebRequest)WebRequest.Create(url);
            http.ContentType = "application/json";
            
            if (token != null && !token.Trim().Equals(""))
            {
                http.PreAuthenticate = true;
                http.Headers.Add("Authorization", "Bearer " + token);
            }
        }

        public string EnviarPost(Object Obj_json)
        {
            try
            {
                http.Method = "POST";
                using (var streamWriter = new StreamWriter(http.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(Obj_json);
                    streamWriter.Write(json);
                }
              
                
                var httpResponse = (HttpWebResponse)http.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}