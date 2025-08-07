using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class MercadoriaUpload : System.Web.UI.Page
    {
        private String[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".tiff" };

        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (usr == null)
            {
                Response.Redirect("~");
            }

            if (Request.Params["plu"] != null)
            {
                String plu = Request.Params["plu"].ToString();

            }


        }

        protected void TimerImporta_Tick(object sender, EventArgs e)
        {
            String strProcesso = (String)Session["Processo"];
            if (strProcesso != null)
            {
                lblDetalhes.Text = strProcesso;
                lblDetalhes.ForeColor = System.Drawing.Color.Blue;
            }

            String strAborta = (String)Session["Aborta"];
            String strFim = (String)Session["Fim"];

            if (strAborta != null || strFim != null)
            {
                TimerImporta.Enabled = false;

                if (strFim != null)
                {
                    lblDetalhes.Text = strFim;
                }

                Session.Remove("Fim");
                Session.Remove("Aborta");

            }
            String strErro = (String)Session["Error"];
            if (strErro != null)
            {
                TimerImporta.Enabled = false;
                lblDetalhes.Text += strErro;
                lblDetalhes.ForeColor = System.Drawing.Color.Red;
                Session.Remove("Error");

            }
        }

        private void processarArquivos()
        {

            String strDetalhes = (String)Session["Processo"];

            strDetalhes += "Processandando";

            Session.Remove("Processo");
            Session.Add("Processo", strDetalhes);

            System.Threading.Thread.Sleep(5000);


            strDetalhes += ".... Concluido";

            Session.Remove("Fim");
            Session.Add("Fim", strDetalhes);




        }
        protected void btnCancela_Click(object sender, EventArgs e)
        {
            String plu = "";
            if (Request.Params["plu"] != null)
            {
                plu = Request.Params["plu"].ToString();

            }
            Response.Redirect("MercadoriaDetalhes.aspx?plu=" + plu + "&img=true");
        }
        protected void salvarBase64(String fileExtension, dynamic objUrl)
        {
            String base64 = this.imagemBase64(objUrl.File);
            String seqImagemBanco = Funcoes.sequencia("IMG_COUNT", null);
            Funcoes.salvaProximaSequencia("IMG_COUNT", null);

            String sqlMedia = "Insert into mercadoria_media (PLU,NOME_ARQUIVO,TIPO,ORDEM,BASE)" +
                " VALUES ('" + objUrl.PLU + "'," +
                    "'IMG_"+objUrl.PLU+ "_"+ seqImagemBanco + "'," +
                    "'" + fileExtension + "',(Select count(*) from mercadoria_media where plu = '" + objUrl.PLU + "'),'" + base64 + "');";

            Conexao.executarSql(sqlMedia);
            Conexao.executarSql(@"UPDATE mercadoria SET Data_Alteracao = GETDATE() where plu='" + objUrl.PLU + "' ");

            File.Delete(objUrl.File);

            Response.Redirect("MercadoriaDetalhes.aspx?plu=" + objUrl.PLU + "&img=true");
            //Session.Remove("Processo");
        }
        protected void btnArquivo_Click(object sender, EventArgs e)
        {
            bool bMenor = false;
            try
            {
                if (FileArquivo.HasFile)
                {

                    Boolean fileOK = false;
                    String plu = "";
                    String id = GerarID().ToString();
                    if (Request.Params["plu"] != null)
                    {
                        plu = Request.Params["plu"].ToString();

                    }
                    String path = Server.MapPath("~/modulos/Cadastro/imgs/uploads/" + plu + "/");
                    if (!Directory.Exists(@path))
                        Directory.CreateDirectory(@path);



                    String fileExtension = "";
                    
                    if (FileArquivo.HasFile)
                    {
                        fileExtension = System.IO.Path.GetExtension(FileArquivo.FileName).ToLower();
                        for (int i = 0; i < allowedExtensions.Length; i++)
                        {
                            if (fileExtension == allowedExtensions[i])
                            {
                                fileOK = true;
                                break;
                            }
                        }
                    }

                    if (fileOK)
                    {
                        try
                        {
                            var objUrl = endereco();
                            FileArquivo.SaveAs(objUrl.File);
                            validaImagem(objUrl);
                            salvarBase64(fileExtension, objUrl);
                        }
                        catch (Exception ex)
                        {
                            throw ex;

                        }
                    }
                    else
                    {
                        // MENSAGEM INFORMATIVA PARA O USUÁRIO
                        string msg = "Só poderá carregar Arquivos " + string.Join(", ", allowedExtensions) + " neste campo.";

                        throw new Exception(msg);
                    }
                }
                else
                {
                    // MENSAGEM INFORMATIVA PARA O USUÁRIO
                    string msg = "Limite máximo para a imagem é de " + (bMenor ? "250k" : "1M") + ".";
                    throw new Exception(msg);
                }





            }
            catch (Exception ex)
            {
                lblDetalhes.Text = ex.Message;
                lblDetalhes.ForeColor = System.Drawing.Color.Red;
            }

        }
        public Int64 GerarID()
        {
            try
            {
                DateTime data = new DateTime();
                data = DateTime.Now;
                string s = data.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                return Convert.ToInt64(s);

            }
            catch (Exception erro)
            {

                throw erro;
            }
        }
        protected void validaImagem(dynamic objUrl)
        {
            int maxFile = 0;

            bool bMenor = (Request.Params["menor"] != null);

            if (bMenor)
            {
                maxFile = (1024 * 250); // 250k
            }
            else
            {
                maxFile = (1024 * 1000); // 1M
            }
            FileInfo file = new FileInfo(objUrl.File);
            long fileLength = file.Length;
            file = null;
            if (fileLength > maxFile)
            {
                string msg = "Limite máximo para a imagem é de " + (bMenor ? "250k" : "1M") + ".";
                File.Delete(objUrl.File);
                throw new Exception(msg);
            }

            System.Drawing.Image img = System.Drawing.Image.FromFile(objUrl.File);

            int Width = img.Width;
            int Height = img.Height;
            img.Dispose();
            if (Width > 5000 || Height > 5000)
            {
                string msg = "A Imagem não pode ser maior que 5000x5000 pixels.";
                File.Delete(objUrl.File);
                throw new Exception(msg);
            }
        }

        protected void btnUrl_Click(object sender, EventArgs e)
        {
            try
            {

                String fileExtension = "";

                bool fileOK = false;
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (txtUrl.Text.ToLower().Contains(allowedExtensions[i]))
                    {
                        fileExtension = allowedExtensions[i];
                        fileOK = true;
                        break;
                    }
                }

                if (!fileOK)
                {
                    // MENSAGEM INFORMATIVA PARA O USUÁRIO
                    string msg = "Só poderá carregar Arquivos " + string.Join(", ", allowedExtensions) + " neste campo.";

                    throw new Exception(msg);
                }


                WebClient myWebClient = new WebClient();

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                byte[] myDataBuffer = myWebClient.DownloadData(txtUrl.Text);
                var objUrl = endereco();
                File.WriteAllBytes(objUrl.File, myDataBuffer);
                validaImagem(objUrl);
                salvarBase64(fileExtension, objUrl);

            }
            catch (Exception ex)
            {

                lblDetalhes.Text = ex.Message;
                lblDetalhes.ForeColor = System.Drawing.Color.Red;
            }


        }

        protected string imagemBase64(String path)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;
        }

        protected dynamic endereco()
        {
            bool bMenor = (Request.Params["menor"] != null);
            String plu = "";
            if (Request.Params["plu"] != null)
            {
                plu = Request.Params["plu"].ToString();
            }

            String path = Server.MapPath("~/modulos/Cadastro/imgs/uploads/" + plu + "/");
            if (!Directory.Exists(@path))
                Directory.CreateDirectory(@path);


            String id = GerarID().ToString();
            string end = @path + plu + id + (bMenor ? "_mini" : "") + ".jpg";
            String pathUrl = Server.UrlDecode("~/modulos/Cadastro/imgs/uploads/" + plu + "/" + plu + id + (bMenor ? "_mini" : "") + ".jpg");

            var objUrl = new
            {
                PLU = plu,
                File = end,
                Url = pathUrl
            };
            return objUrl;

        }
    }

}