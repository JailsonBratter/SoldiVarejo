using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.NotaFiscal.pages
{
    public partial class DanfeHtmlPdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlDataReader rs = null;
            try
            {


                rs = Conexao.consulta("Select logo from filial where filial = 'MATRIZ'", null, false);
                if (rs.Read())
                {
                    byte[] bytes = (byte[])rs["logo"];
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    string url = "data:image/png;base64," + base64String;
                    //imgLogo.ImageUrl = url;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }


        }
    }
}