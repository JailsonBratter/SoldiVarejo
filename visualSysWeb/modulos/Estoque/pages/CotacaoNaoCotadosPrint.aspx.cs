using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Estoque.pages
{
    public partial class CotacaoNaoCotadosPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User usr = (User)Session["User"];
            if (usr == null)
            {
                Response.Redirect("~");
                return;
            }
                
            String codigo = Request.Params["cotacao"].ToString();
            if ( codigo != null)
            {
                cotacaoDAO cotacao = new cotacaoDAO(codigo, usr);
                lblCotacao.Text = cotacao.Cotacao.ToString();
                lblDescricao.Text = cotacao.descricao;
                lblData.Text = cotacao.DataBr();
                lblUsuario.Text = cotacao.Usuario;
                gridItensNaoCotados.DataSource = cotacao.itensNaoCotados();
                gridItensNaoCotados.DataBind();
            }

        }

        protected void gridItensNaoCotados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}