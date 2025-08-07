using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using visualSysWeb.modulos.Cadastro.dao;
using System.Data;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.code
{
    public class APIiFood
    {
        WebClient client = new WebClient();
        string IDLoja = "";
        string accessToken = "";
        string url = "https://merchant-api.ifood.com.br/catalog/v1.0/merchants/";

        public APIiFood()
        {
            IDLoja = Conexao.retornaUmValor("SELECT ID_Loja_iFood FROM IFoodConfLojas", null);
            accessToken = Conexao.retornaUmValor("SELECT accessToken FROM IFoodConfig", null);
        }
       
        public List<Cardapio_iFood_Categoria> GETCategorias(string IDCatalog, string IDCategoria)
        {
            List<Cardapio_iFood_Categoria> categorias = new List<Cardapio_iFood_Categoria>();

            HttpWebRequest request = WebRequest.Create(url + IDLoja + "/catalogs/" + IDCatalog + "/categories/" + IDCategoria ) as HttpWebRequest;
            request.Headers[HttpRequestHeader.Authorization] = "Baren " + accessToken;
            request.Headers[HttpRequestHeader.Accept] = "application/json";
            request.Method = "GET";
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
                                var categoria = JsonConvert.DeserializeObject<Cardapio_iFood_Categoria>(strResponseValue.ToString());
                                categorias.Add(categoria);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return categorias;
        }

        //Pesquisa todos os produtos do cardapio e retornar uma lista de produtos
        public List<Cardapio_iFood_Produto> produtosCardapio(int IDCardapio, string pathImagens)
        {
            List<Cardapio_iFood_Produto> lista = new List<Cardapio_iFood_Produto>();
            try
            {
                Cardapio_iFood_Produto prod = new Cardapio_iFood_Produto();
                lista = prod.produtosCardapio(IDCardapio, pathImagens);
                return lista;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool PostProduto(Cardapio_iFood_Produto produto)
        {
            Cardapio_iFood_Produto prod = new Cardapio_iFood_Produto();
            try
            {
                Uri urlString = new Uri(url + IDLoja + "/products?merchantid=" + IDLoja);
                var produtoJson = new
                {
                    id = produto.id,
                    name = produto.name,
                    description = produto.description,
                    externalCode = produto.PLU,
                    image = produto.image,
                    serving = produto.serve,
                    dietaryRestrictions = produto.restricaoAlimentar,
                    ean = produto.ean
                };

                string produtoPOST = JsonConvert.SerializeObject(produtoJson);
                //byte[] data = Encoding.ASCII.GetBytes(produtoPOST);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.UploadString(urlString, "POST", produtoPOST);
                    //client.UploadData( urlString, "PUT", data);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool PutProduto(string IDCatalog, Cardapio_iFood_Produto produto)
        {
            try
            {
                Uri urlString = new Uri(url + IDLoja + "/products/" + produto.id.ToString());
                var produtoJson = new
                {
                    name = produto.name,
                    description = produto.description,
                    externalCode = produto.PLU,
                    image = produto.image,
                    serving = produto.serve,
                    dietaryRestrictions =produto.restricaoAlimentar,
                    ean = "" //produto.ean
                };

                string produtoPUT = JsonConvert.SerializeObject(produtoJson);
                //byte[] data = Encoding.ASCII.GetBytes(produtoPUT);
                byte[] data = Encoding.UTF8.GetBytes(produtoPUT);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers[HttpRequestHeader.Accept] = "*/*";
                    client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate, br";
                    client.Headers[HttpRequestHeader.AcceptCharset] = "UTF-8";
                    client.UploadData(urlString, "PUT", data);
                    //client.UploadString(urlString, "PUT", produtoPUT);
                    //client.UploadData( urlString, "PUT", data);
                }
                return true;

            }
            catch (Exception e)
            {
                //PostProduto(produto);
                return false;
            }
        }

        public void GetProduto(string IDCatalog, Cardapio_iFood_Produto produto)
        {
            Uri urlString = new Uri(url + IDLoja + "/products/" + produto.id.ToString());

        }

        public bool PostItem(string Categoria, Cardapio_iFood_Produto produto)
        {
            string StringURL = url + IDLoja + "/categories/" + Categoria + "/products/" + produto.id + "?";
            StringURL += "merchantId=" + IDLoja;
            StringURL += "&categoryId=" + Categoria;
            StringURL += "&productId=" + produto.id;

            string itemPOST = "";

            try
            {
                //Inclusão do Item
                var item = new
                {
                    status = "AVAILABLE",
                    price = new
                    {
                        value = 1.50
                    }
                };

                itemPOST = JsonConvert.SerializeObject(item);

                Uri urlString = new Uri(StringURL);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.UploadString(urlString, "POST", itemPOST);
                    //client.UploadData( urlString, "PUT", data);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public bool PatchItem(string Categoria, Cardapio_iFood_Produto produto)
        {
            string StringURL = url + IDLoja + "/categories/" + Categoria + "/products/" + produto.id + "?";
            StringURL += "merchantId=" + IDLoja;
            StringURL += "&categoryId=" + Categoria;
            StringURL += "&productId=" + produto.id;

            string itemPATCH = "";
            List<disponibilidade> shiftD = new List<disponibilidade>();
            //Adiconar o Shigt
            disponibilidade sd = new disponibilidade();
            sd.startTime = produto.horaInicio;
            sd.endTime = produto.horaFim;
            sd.monday = produto.dia2;
            sd.tuesday = produto.dia3;
            sd.wednesday = produto.dia4;
            sd.thursday = produto.dia5;
            sd.friday = produto.dia6;
            sd.saturday = produto.dia7;
            sd.sunday = produto.dia1;
            shiftD.Add(sd);

            try
            {
                //Inclusão do Item
                var item = new
                {
                    status = (produto.status ? "AVAILABLE" : "UNAVAILABLE"),
                    price = new
                    {
                        value = produto.preco
                    },
                    externalCode = produto.PLU,
                    shifts = shiftD
                };

                itemPATCH = JsonConvert.SerializeObject(item);

                Uri urlString = new Uri(StringURL);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.UploadString(urlString, "PATCH", itemPATCH);
                    //client.UploadData( urlString, "PUT", data);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        class disponibilidade
        {
            public string startTime = "";
            public string endTime = "";
            public bool monday = false;
            public bool tuesday = false;
            public bool wednesday = false;
            public bool thursday = false;
            public bool friday = false;
            public bool saturday = false;
            public bool sunday = false;

            public disponibilidade()
            {

            }
        }


    }
}