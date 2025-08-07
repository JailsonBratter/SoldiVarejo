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
using System.Xml.XPath;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class KCWSPedido
    {
        private static string Acao = "";
        private pedidoDAO Pedido = null;
        private pedido_itensDAO PedidoItens = null;
        private User usrKCWS = null;
        private bool bolClienteNovo = false;
        private string token1 = "", token2 = "";
        public String strError = "";

        public KCWSPedido(string strAcao, User usr)
        {
            //    Produto = merc;
            usrKCWS = usr;
            Acao = strAcao;

            token1 = Funcoes.valorParametro("KCW_KEY_A1", usr);
            token2 = Funcoes.valorParametro("KCW_KEY_A2", usr);

            switch (strAcao)
            {
                case "ListarNovos":
                    ListarNovos(token1, token2);
                    break;
            }
        }
        public HttpWebRequest CreateSOAPPedido()
        {
            String urlServer = Funcoes.valorParametro("KCW_URL_SERVER", usrKCWS);
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(urlServer + "pedido.asmx");
            Req.Headers.Add("SOAPAction:http://www.ikeda.com.br/" + Acao);
            Req.ContentType = "text/xml; charset=utf-8";
            Req.Accept = "text/xml";
            Req.Method = "POST";
            return Req;
        }
        public void ListarNovos(string A1, string A2)
        {
            try
            {
                string strXML = "";
                HttpWebRequest request = CreateSOAPPedido();
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
                strXML += "<StatusPedido>7</StatusPedido>";
                strXML += "<StatusInternoPedido></StatusInternoPedido>";
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
                        InserePedidos(ServiceResult.ToString());
                    }
                }
            }
            catch (Exception err)
            {
                strError += err.Message;
            }
        }
        private void InserePedidos(string strXML)
        {
            string strArquivo = "";
            string strDataGeral = "";
            string strUsoGeral = "";
            //Define nome do arquivo
            strArquivo = Path.GetTempPath().ToString() + "SOAPXML" + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + ".XML";
            try
            {
                //Grava conteúdo retornado em arquivo definido acima
                StreamWriter writer = new StreamWriter(@strArquivo, true);
                using (writer)
                {
                    writer.WriteLine(strXML);
                }

                XmlDocument docXML = new XmlDocument();
                //**Carrega arquivo XML no objeto
                docXML.Load(@strArquivo);



                //Deleta arquivo lido
                File.Delete(strArquivo);
                XmlNodeList elemPedidos = docXML.GetElementsByTagName("Lista");


                foreach (XmlNode xn in  elemPedidos.Item(0).ChildNodes )
                {
                    pedidoDAO ped = new pedidoDAO(usrKCWS);
                    ClienteDAO cli = new ClienteDAO(usrKCWS);
                    strUsoGeral = xn["PedidoCodigo"].InnerText.ToString();
                    if (!strUsoGeral.Equals("") && !strUsoGeral.Equals("0"))
                    {
                        ////Cadastro de Clientes - Inclusão ou Alteração
                        if (xn["CNPJ"].InnerText.ToString() != "")
                        {
                            cli.Pessoa_Juridica = true;
                            cli.CNPJ = xn["CNPJ"].InnerText.ToString();
                            cli.IE = xn["IE"].InnerText.ToString();
                            if (cli.IE.Equals(""))
                            {
                                cli.indIEDest = 9;
                            }
                            else
                            {
                                cli.indIEDest = 3;
                            }
                        }
                        else
                        {
                            cli.Pessoa_Juridica = false;
                            cli.CNPJ = xn["CPF"].InnerText.ToString();
                            cli.indIEDest = 2;
                        }

                        strUsoGeral = cli.CNPJ;

                        strUsoGeral = Conexao.retornaUmValor("SELECT Codigo_Cliente FROM Cliente WHERE cnpj ='" + strUsoGeral.Trim() + "'", usrKCWS);



                        if (!strUsoGeral.Trim().Equals(""))
                        {
                            //cli.Codigo_Cliente =  strUsoGeral;
                            cli = new ClienteDAO(strUsoGeral, usrKCWS);
                            bolClienteNovo = false;
                        }
                        else
                        {
                            //cli.Codigo_Cliente = code.Funcoes.sequencia("CLIENTE.CODIGO_CLIENTE", usrKCWS);
                            bolClienteNovo = true;
                        }

                        if (cli.CNPJ.Equals(""))
                        {
                            break;
                        }

                        cli.Nome_Cliente = (xn["Nome"].InnerText.ToString().ToUpper() + " " + xn["Sobrenome"].InnerText.ToString()).Trim().ToUpper(); //Nome e sobrenome
                        cli.nome_fantasia = code.Funcoes.RemoverAcentos(xn["Nome"].InnerText.ToString().ToUpper());
                        cli.Endereco = code.Funcoes.RemoverAcentos((xn["TipoLogradouro"].InnerText.ToString().Trim() + " " + xn["Logradouro"].InnerText.ToString()).Trim().ToUpper());
                        cli.complemento_end = code.Funcoes.RemoverAcentos(xn["Complemento"].InnerText.ToString().Trim().ToUpper());
                        cli.endereco_nro = xn["Numero"].InnerText.ToString().Trim().ToUpper();
                        cli.Bairro = code.Funcoes.RemoverAcentos(xn["Bairro"].InnerText.ToString()).Trim().ToUpper();
                        cli.Cidade = code.Funcoes.RemoverAcentos(xn["Cidade"].InnerText.ToString().Trim().ToUpper());
                        cli.CEP = xn["CEP"].InnerText.ToString().Trim().ToUpper();
                        cli.UF = xn["Estado"].InnerText.ToString().Trim().ToUpper();

                        cli.Estado_civil = "";
                        strDataGeral = xn["DataNascimento"].InnerText.ToString().Trim().ToUpper();
                        cli.Data_Nascimento = Convert.ToDateTime(strDataGeral);
                        cli.Naturalidade = "";
                        cli.Nome_conjuge = "";
                        cli.Contato = "";
                        cli.Renda_Mensal = 0;
                        cli.Limite_Credito = 0;
                        cli.Utilizado = 0;
                        cli.ICM_Isento = true;
                        cli.Historico = "";
                        cli.estado_cliente = true;
                        cli.Codigo_tabela = "NORMAL";
                        cli.vendedor = "";
                        cli.Endereco_ent = "";
                        cli.Cep_ent = "";
                        cli.Bairro_ent = "";
                        cli.Cidade_ent = "";
                        cli.Uf_ent = "";
                        cli.complemento_ent = "";
                        cli.Iva_descricao = false;
                        cli.endereco_ent_nro = "";
                        cli.inativo = false;
                        cli.habilita_f9 = false;
                        cli.Situacao = "OK";
                        //Cliente Meio de Comunicacao
                        //Telefone 1
                        string telefone1 = xn["Telefone1"].InnerText.ToString().Trim().Replace("-", "").Replace(" ", "");
                        if (!telefone1.Equals("") && !cli.existeMeioComunica("FONE", telefone1))
                        {
                             cli.addMeioComunicacao("FONE", telefone1, "O MESMO");
                        }
                        //Telefone 2
                        string telefone2 = xn["Telefone2"].InnerText.ToString().Trim().Replace("-", "").Replace(" ", "");
                        if (!telefone2.Equals("") && !cli.existeMeioComunica("FONE",telefone2))
                        {
                            cli.addMeioComunicacao("FONE", telefone2, "O MESMO");
                            
                        }
                        //Telefone 3
                        string telefone3 = xn["Telefone3"].InnerText.ToString().Trim().Replace("-", "").Replace(" ", "");
                        if (!telefone3.Equals("") && !cli.existeMeioComunica("FONE",telefone3))
                        {
                             cli.addMeioComunicacao("FONE", telefone3, "O MESMO");
                        }
                        //Celular
                        string celular = xn["Celular"].InnerText.ToString().Trim().Replace("-", "").Replace(" ", "");
                        if (!celular.Equals("") && !cli.existeMeioComunica("CELULAR",celular))
                        {
                             cli.addMeioComunicacao("CELULAR", celular, "O MESMO");
                        }
                        //eMail
                        string eMail = xn["Email"].InnerText.ToString().Trim().Replace("-", "").Replace(" ", "");
                        if (!eMail.Equals("")&& !cli.existeMeioComunica("EMAIL",eMail))
                        {
                             cli.addMeioComunicacao("EMAIL", eMail, "O MESMO");
                        }
                        try
                        {
                            cli.salvar(bolClienteNovo);
                        }
                        catch (Exception err)
                        {
                            strError += err.Message;
                        }
                        ped.Pedido = xn["PedidoCodigo"].InnerText.ToString().Trim();
                        ped.Tipo = 1;
                        ped.Status = 1;
                        ped.Cliente_Fornec = cli.Codigo_Cliente;
                        strDataGeral = xn["Data"].InnerText.ToString().Trim();
                        ped.Data_cadastro = Convert.ToDateTime(strDataGeral);
                        ped.Data_entrega = Convert.ToDateTime(strDataGeral);
                        ped.hora = Convert.ToDateTime(strDataGeral).ToString("hh:MM");
                        ped.Desconto = 0;
                        ped.Frete = Convert.ToDecimal(xn["ValorFreteCobrado"].InnerText.ToString().Trim().Replace(".", ",")); //Frete

                        ped.Despesas = Funcoes.decTry(xn["ValorJuros"].InnerText.ToString().Trim().Replace(".", ","));
                        ped.Total = Convert.ToDecimal(xn["ValorTotal"].InnerText.ToString().Trim().Replace(".", ",")); //Total
                        ped.Usuario = usrKCWS.getNome();
                        ped.Obs = "";
                        ped.CFOP = 5102;
                        ped.orcamento = "";
                        ped.funcionario = usrKCWS.getNome();
                        ped.hora_fim = Convert.ToDateTime(strDataGeral).ToString("hh:MM");
                        ped.ID = false;
                        ped.cotacao = 0;
                        ped.impresso = false;
                        ped.TabelaPreco = "NORMAL";
                        ped.pedido_simples = false;
                        ped.centro_custo = "";

                        foreach (XmlNode xni in xn["Itens"].ChildNodes)
                        {
                            pedido_itensDAO item = new pedido_itensDAO(usrKCWS);
                            item.PLU = xni["CodigoInterno"].InnerText.ToString().Trim();
                            item.Qtde = Decimal.Parse(xni["ItemQtde"].InnerText.ToString().Replace(".", ","));
                            item.Descricao = xni["ItemNome"].InnerText.ToString().Trim().ToUpper();
                            item.Embalagem = 1;
                            item.unitario = Decimal.Parse(xni["ItemValor"].InnerText.ToString().Replace(".", ","));
                            item.inserido = true;
                            item.Desconto = 0;
                            item.vPrecoMinimo = Decimal.Parse(xni["ItemValor"].InnerText.ToString().Replace(".", ","));
                            ped.addItens(item);
                        }
                        ped.salvar(true);
                        Validar(xn["PedidoCodigo"].InnerText.ToString().Trim());
                    }
                }
            }
            catch (Exception err)
            {
                strError = err.Message;
            }
        }
        private bool Validar(string Pedido)
        {
            try
            {
                string strXML = "";
                Acao = "Validar";
                HttpWebRequest request = CreateSOAPPedido();
                XmlDocument SOAPReqBody = new XmlDocument();
                strXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                strXML += "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">";
                strXML += "<soap:Header>";
                strXML += "<clsSoapHeader xmlns=\"http://www.ikeda.com.br\">";
                strXML += "<A1>" + token1 + "</A1>";
                strXML += "<A2>" + token2 + "</A2>";
                strXML += "</clsSoapHeader></soap:Header>";
                strXML += "<soap:Body>";
                strXML += "<Validar xmlns=\"http://www.ikeda.com.br\">";
                strXML += "<LojaCodigo>0</LojaCodigo>";
                strXML += "<CodigoPedido>" + Pedido + "</CodigoPedido>";
                strXML += "<CodigoInternoPedido></CodigoInternoPedido>";
                strXML += "</Validar>";
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
                        if (ServiceResult.IndexOf("Sucesso") > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                return false;
            }
        }
        private String retornaValorNoAtual(XPathNavigator rs, String no)
        {
            string valor = "";
            if (rs.MoveToChild(no, ""))
            {
                valor = rs.Value;
                rs.MoveToParent();
            }
            return valor;
        }
    }
}