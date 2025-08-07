using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class ImageBase64 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string plu = Request.Params["plu"].ToString();
            string ordem = Request.Params["ordem"].ToString();
            string base64 = Conexao.retornaUmValor("Select base from mercadoria_media where plu='" + plu + "' and ordem=" + ordem, null);
            string tipo = Conexao.retornaUmValor("Select tipo from mercadoria_media where plu='" + plu + "' and ordem=" + ordem, null);


            String path = Server.MapPath("~/modulos/Cadastro/imgs/uploads/" + plu + "/");
            if (!Directory.Exists(@path))
                Directory.CreateDirectory(@path);

            if (!File.Exists(path + plu + ".jpg"))
            {
                var imagem = System.Drawing.Image.FromStream(new MemoryStream(Convert.FromBase64String(base64)));
                imagem.Save(path + plu + ".JPG");
            }

            Image1.ImageUrl = "~/modulos/Cadastro/imgs/uploads/" + plu + "/" + plu + ".JPG";
            //Image1.ImageUrl = "data:image/" + tipo + ";base64," + base64;
        }
    }
}