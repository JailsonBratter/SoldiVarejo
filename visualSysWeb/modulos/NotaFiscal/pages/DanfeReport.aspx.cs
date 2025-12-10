using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using visualSysWeb.dao;
using visualSysWeb.code;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class DanfeReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            User usr = (User)Session["User"];
            if (usr == null)
            {
                Response.Redirect("~");
            }

            bool danfeGerado = false;
            String strArquivo = "";
            String strPasta = "";
            String strPath = "";
            string diretorioGravacao = usr.filial.diretorio_exporta;

            if (Request.Params["soEmail"] != null)
            {
                soEmail();
            }
            else
            {

                String clienteFornecedor = "";
                String numero = "";
                int tipoNf = 1;
                String tipoOrigem = "N";
                bool enviaEmail = false;

                if (Request.Params["cliente_Fornecedor"] != null)
                {
                    clienteFornecedor = Request.Params["cliente_Fornecedor"].ToString();
                }
                if (Request.Params["numero"] != null)
                {
                    numero = Request.Params["numero"].ToString();
                }
                if (Request.Params["tipoNf"] != null)
                {
                    int.TryParse(Request.Params["tipoNf"].ToString(), out tipoNf);
                }
                if (Request.Params["tipoOrigem"] != null)
                {
                    tipoOrigem = Request.Params["tipoOrigem"].ToString();
                }


                if (Request.Params["enviaEmail"] != null)
                {
                    enviaEmail = Request.Params["enviaEmail"].ToString().ToUpper().Equals("TRUE");
                }
             

                String strId = "";

                DateTime dtEmissao = new DateTime();
                String strStatus = "";

                //if (tipoOrigem.ToUpper().Equals("P"))
                //{
                //    SqlDataReader rsPedido = null;
                //    try
                //    {
                //        rsPedido = Conexao.consulta("Select cod_nota,Status from pedido where pedido ='" + numero + "' and tipo=1 ", usr, false);
                //        if (rsPedido.Read())
                //        {
                //            if (rsPedido["status"].ToString().Equals("7"))
                //            {
                //                numero = rsPedido["cod_nota"].ToString();
                //                danfeGerado = true;
                //                tipoOrigem = "N";
                //            }
                //        }
                //    }
                //    catch (Exception)
                //    {

                //        throw;
                //    }
                //    finally
                //    {
                //        if (rsPedido != null)
                //            rsPedido.Close();
                //    }
                //}

                if (tipoOrigem.ToUpper().Equals("N"))
                {
                    SqlDataReader rsNF = null;
                    try
                    {
                        rsNF = Conexao.consulta("Select id, Emissao,status from nf where codigo='" + numero + "' and tipo_nf="+tipoNf+" and cliente_Fornecedor='" + clienteFornecedor + "'", usr, false);
                        if (rsNF.Read())
                        {
                            strId = rsNF["id"].ToString();
                            strStatus = rsNF["status"].ToString();
                            DateTime.TryParse(rsNF["emissao"].ToString(), out dtEmissao);

                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        if (rsNF != null)
                            rsNF.Close();

                    }


                    //strId = Conexao.retornaUmValor("Select Id from nf where codigo='" + numero + "' and tipo_nf=1 and cliente_Fornecedor='" + clienteFornecedor + "'", usr);





                    if (!danfeGerado)
                    {
                        if (strId.Equals(""))
                        {
                            strId = "0000000000000000000000";
                        }

                        
                         

                        if (!Directory.Exists(@"C:\temp"))
                        {    try
                            {
                                Directory.CreateDirectory(@"C:\temp");
                                
                            }
                            catch (Exception)
                            {

                                  throw new Exception("Não Foi encontrado a Pasta C:/temp e o sistema não tem permissão para criar, Crie a pasta e execute novamente a operação!");
                            }
                            
                        }
                        String strEndeCodBarra = "C:/temp/" + strId + ".jpg";
                        
                        
                        BarcodeLib.Barcode Barcode = new BarcodeLib.Barcode(strId, BarcodeLib.TYPE.CODE128C);
                        System.Drawing.Image x = null;
                        x = Barcode.Encode(BarcodeLib.TYPE.CODE128C, strId, 300, 100);
                        //Image1 = x;

                        x.Save(strEndeCodBarra);



                        String strSql = "UPDATE NF SET NF.IDBARRA = @MyImagem where nf.codigo ='" +
                                              numero + "' and cliente_fornecedor='" + clienteFornecedor + "' and tipo_nf="+tipoNf;

                        salvarImagem(strSql, strEndeCodBarra);
                    }

                }

                // Verifico se já foi copiado o Arquivo pdf do Danfe para a Pasta Autorizados .
                // * como IIS não aceita abrir um aquivo que não esteja localizado na maquina servidor abro o arquivo da Danfe que foi gerado no servidor
                if (strStatus.Equals("AUTORIZADO") && tipoNf != 3)
                {
                    diretorioGravacao += (diretorioGravacao.Substring(diretorioGravacao.Length - 1) == "\\" ? "" : "");
                    if (File.Exists(diretorioGravacao +  dtEmissao.ToString("yyyyMM") + "/" + strId + "_danfe.pdf"))
                    {
                        danfeGerado = true;
                    }
                }
                if (!danfeGerado)
                {
                    DataTable dtNota = new DataTable();
                    ReportDocument crystalreport = new ReportDocument();
                        if (tipoNf == 3)
                        {
                            crystalreport.Load(Server.MapPath("~/modulos/notaFiscal/pages/CrystalNotaPedido.rpt"));
                        }
                        else
                        {
                            crystalreport.Load(Server.MapPath("~/modulos/notaFiscal/pages/CrystalReport1.rpt"));
                        }
                    String sql = " exec sp_NF_DANFE @Filial='" + usr.getFilial() + "' , " +
                                                   "@cliente_fornecedor='" + clienteFornecedor.Trim() + "' ," +
                                                   "@numero='" + numero + "'," +
                                                   "@tipoNF='" + tipoNf.ToString() + "'," +
                                                   "@tipoOrigem='" + tipoOrigem + "'";
                    dtNota = Conexao.GetTable(sql, null, false);
                    crystalreport.SetDataSource(dtNota);
                    //Exportando

                    if (strId.Equals(""))
                    {
                        strPasta = Server.MapPath("~/modulos/notafiscal/pages/" + DateTime.Now.ToString("yyyyMM"));
                        strArquivo = strPasta + "/" + numero + "_danfe.pdf";
                        strPath = "~/modulos/notafiscal/pages/" + DateTime.Now.ToString("yyyyMM") + "/" + numero + "_danfe.pdf";
                    }
                    else
                    {
                        strPasta = Server.MapPath("~/modulos/notafiscal/pages/" + dtEmissao.ToString("yyyyMM"));
                        strArquivo = strPasta + "/" + strId + "_danfe.pdf";
                        strPath = "~/modulos/notafiscal/pages/" + dtEmissao.ToString("yyyyMM") + "/" + strId + "_danfe.pdf"; 
                    }

                  
                        if (!Directory.Exists(strPasta))
                        {
                            Directory.CreateDirectory(strPasta);
                        }

                        CrystalDecisions.Shared.DiskFileDestinationOptions DanfeFile = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                        //DanfeFile.DiskFileName = "@c:\danfe.pdf";
                        crystalreport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, strArquivo);

                        if (strStatus.Equals("AUTORIZADO"))
                        {
                            if (!Directory.Exists(diretorioGravacao + dtEmissao.ToString("yyyyMM")))
                            {
                                Directory.CreateDirectory(diretorioGravacao + dtEmissao.ToString("yyyyMM"));
                            }

                            if (!File.Exists(diretorioGravacao+ dtEmissao.ToString("yyyyMM") + "/" + strId + "_danfe.pdf"))
                            {
                                File.Copy(strArquivo, diretorioGravacao + dtEmissao.ToString("yyyyMM") + "/" + strId + "_danfe.pdf");
                            }
                        }

                        if (usr.filial.producaoNfe)
                        {
                            if (enviaEmail)
                            {
                                enviaEmailDanfe(dtEmissao, strId);
                            }
                        }
                        Response.Redirect(strPath);
                   
                }
                else
                {
                    strPasta = Server.MapPath("~/modulos/notafiscal/pages/" + dtEmissao.ToString("yyyyMM"));
                    strPath = "~/modulos/notafiscal/pages/" + dtEmissao.ToString("yyyyMM") + "/" + strId + "_danfe.pdf";
                    Response.Redirect(strPath);
                }
            }
                 }
                    catch (Exception err)
                    {

                        lblErro.Text = err.Message;

                    }

        }


        public void soEmail()
        {


            User usr = (User)Session["User"];
            if (usr.filial.producaoNfe)
            {
                String strId = "";
                DateTime dtEmissao = new DateTime();
                String clienteFornecedor = "";
                String numero = "";
                String tipoNf = "1";

                if (Request.Params["cliente_Fornecedor"] != null)
                {
                    clienteFornecedor = Request.Params["cliente_Fornecedor"].ToString();
                }
                if (Request.Params["numero"] != null)
                {
                    numero = Request.Params["numero"].ToString();
                }
                if(Request.Params["tipoNf"] !=null)
                {

                    tipoNf = Request.Params["tipoNf"].ToString().Trim();
                }

                SqlDataReader rsNF = null;
                try
                {
                    rsNF = Conexao.consulta("Select id, Emissao,status from nf where codigo='" + numero + "' and tipo_nf="+tipoNf+" and cliente_Fornecedor='" + clienteFornecedor + "'", usr, false);
                    if (rsNF.Read())
                    {
                        strId = rsNF["id"].ToString();
                        DateTime.TryParse(rsNF["emissao"].ToString(), out dtEmissao);

                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (rsNF != null)
                        rsNF.Close();

                }

                enviaEmailDanfe(dtEmissao, strId);

                ScriptManager.RegisterClientScriptBlock(
                            Page,
                            typeof(Page),
                            "fecha435",
                            "window.close();",
                            true);
            }

        }

        public void enviaEmailDanfe(DateTime dtEmissao, String strId)
        {
            String emailEnvio = "";
            User usr = (User)Session["User"];
            string diretorioGravacao = usr.filial.diretorio_exporta;
            diretorioGravacao += (diretorioGravacao.Substring(diretorioGravacao.Length - 1) == "\\" ? "" : "\\");
            String caminhoAutorizado = diretorioGravacao + dtEmissao.ToString("yyyyMM") + "/" + strId;

            String strNumeroNota = "";
            String strSerie = "";
            String strNomeEmitente = "";
            String strTPNF = "";
            Decimal vNF = 0;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(caminhoAutorizado + "-procNFe.xml");

                XmlNodeList xnList = xmlDoc.GetElementsByTagName("ide");
                foreach (XmlNode xn in xnList)
                {
                    strSerie = xn["serie"].InnerText;
                    strNumeroNota = xn["nNF"].InnerText;
                    strTPNF = xn["tpNF"].InnerText;
                }

                //Processo para gravar documento no DB
                try
                {
                    if (xmlDoc != null)
                    {
                        Documento_EletronicoDAO docE = new Documento_EletronicoDAO();
                        if (!docE.exists(strId))
                        {
                            docE.filial = usr.getFilial();
                            docE.tipo = (strTPNF.Equals("1") ? 2 : 3);
                            docE.data = dtEmissao;
                            docE.caixa = 0;
                            docE.documento = strNumeroNota;
                            docE.id_chave = strId;
                            docE.id_chave_cancelamento = "";
                            docE.nro_serie_equipamento = "";
                            docE.operador = "0";
                            docE.cfe_xml = xmlDoc.InnerXml.ToString();
                            docE.cfe_xml_cancelamento = "";
                            docE.insert();
                        }
                    }
                }
                catch
                {

                }


                xnList = xmlDoc.GetElementsByTagName("emit");
                foreach (XmlNode xn in xnList)
                {
                    strNomeEmitente = xn["xNome"].InnerText;
                }


                //xnList = xmlDoc.GetElementsByTagName("dest");
                //foreach (XmlNode xn in xnList)
                //{
                //    emailEnvio = xn["email"].InnerText;
                //}
                String clienteFornecedor = "";
                if (Request.Params["cliente_Fornecedor"] != null)
                {
                    clienteFornecedor = Request.Params["cliente_Fornecedor"].ToString();
                }
                bool emitForn = Conexao.retornaUmValor("Select Dest_fornec from nf where id ='"+ strId + "' ",null).Equals("1");

                if (emitForn)
                {
                    fornecedorDAO objForn = new fornecedorDAO(clienteFornecedor, usr);
                    emailEnvio = objForn.email;
                }
                else
                {
                    ClienteDAO objCliente = new ClienteDAO(clienteFornecedor, usr);
                    emailEnvio = objCliente.email();
                }


                xnList = xmlDoc.GetElementsByTagName("ICMSTot");
                foreach (XmlNode xn in xnList)
                {
                    Decimal.TryParse(xn["vNF"].InnerText.Replace(".", ","), out vNF);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }


            String msg = "Prezado cliente, <br>" +
                         "<br>" +
                         "Você está recebendo  a NOTA FISCAL ELETRÔNICA (NF-e) número: <b>" + strNumeroNota + "</b> Série: <b>" + strSerie + "</b> Emitida por: <b>" + strNomeEmitente + " </b>, no valor de <b> R$ " + vNF.ToString("N2") + ". </b>" +
                         "<br>" +
                         "<br>" +
                         " Anexo encontram-se arquivo XML da Nota Fiscal Eletrônica ( Este arquivo deverá ser armazenado eletronicamente por sua empresa pelo prazo de 05 (cinco) anos, conforme Lei Art. 173 do Código Tributário Nacional e 4º da Lei 5.172 de 25/10/1966) junto com o Documento Auxiliar da Nota Fiscal Eletrônica (DANFE)." +
                         "<br>" +
                         "<br>" +
                         "  Para checar a validade da NFe, favor consultar no site nacional do projeto NF-e www.nfe.fazenda.gov.br. Inserindo a chave de acesso: " + strId + " ." +
                         "<br>" +
                         "<br>" +
                    "Atenciosamente," +
                    "<br>" +
                    "<b>" + strNomeEmitente + " </b>";



            String[] anexos ={ caminhoAutorizado + "_danfe.pdf"
                              ,caminhoAutorizado + "-procNFe.xml"
                             };

            String emailCC = "";
            if(Funcoes.valorParametro("EMAIL_COPIA",usr).ToUpper().Equals("TRUE"))
            {
                emailCC += usr.email;
            }
            Funcoes.enviarEmailAnexos(usr, emailEnvio, emailCC, " NOTA FISCAL ELETRÔNICA (NF-e) N." + strNumeroNota, msg, anexos);
        }


        public byte[] ConverterImagemParaBytes(string caminhoImagem)
        {
            byte[] arraybytes = null;

            FileInfo informacoesFicnheiro = new FileInfo(caminhoImagem);
            long numeroBytes = informacoesFicnheiro.Length;

            FileStream fStream = new FileStream(caminhoImagem, FileMode.Open, FileAccess.Read);

            BinaryReader br = new BinaryReader(fStream);

            arraybytes = br.ReadBytes((int)numeroBytes);
            br.Close();
            return arraybytes;
        }

        public void salvarImagem(String sql, String caminhoImagem)
        {
            SqlConnection con = null;
            try
            {

                byte[] arrImg = ConverterImagemParaBytes(caminhoImagem);

                con = Conexao.novaConexao();

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add(new SqlParameter("@MyImagem", (object)arrImg));

                cmd.ExecuteNonQuery();
                System.IO.File.Delete(caminhoImagem);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

        }
    }
}