using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Newtonsoft.Json;

using visualSysWeb.dao;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Mobile
{
    public partial class SmartPhone_Espelho : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["User"] == null)
                Response.Redirect("Mobile_Login.aspx");

            User usr = (User)Session["User"];

            using (WebClient client = new WebClient())
            {
                var jsonResponse = client.DownloadString("http://breeds.bratter.com.br:19745/soldiapicloud/api/notas?filial=" + usr.filial.CNPJ);
                //var jsonResponse = client.DownloadString("http://localhost:10291/api/Notas?filial=" + usr.filial.CNPJ);

                List<Nota> notas = JsonConvert.DeserializeObject<List<Nota>>(jsonResponse);
                
                gridPesquisa.DataSource = notas; // Conexao.GetTable(sql, null, false);
                gridPesquisa.DataBind();
            }
        }
    }
}