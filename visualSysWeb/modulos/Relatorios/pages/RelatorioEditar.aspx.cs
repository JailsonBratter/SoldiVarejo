using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.modulos.Relatorios.code;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Relatorios.pages
{
    public partial class RelatorioEditar : PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RelatorioIO rel= new RelatorioIO(Server.MapPath(""),null);
            if (Request.Params["Relatorio"] !=null){

            rel.CriarPasta(Request.Params["relatorio"].ToString());
            }
           // pnFiltros.Controls.Add( criarBotoes());
        }


        

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        protected override bool campoObrigatorio(Control campo)
        {
            String[] campos = { "", 
                           "", 
                           "",
                           "",
                           ""
                          };

            for (int i = 0; i < campos.Length; i++)
            {
                if (campo.ClientID.Equals(campos[i]))
                {
                    //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                    return true;
                }
            }
            return false;

        }




        protected override bool campoDesabilitado(Control campo)
        {
            String[] campos = { "", 
                           ""
                          };

            for (int i = 0; i < campos.Length; i++)
            {
                if (campo.ClientID.Equals(campos[i]))
                {

                    return true;
                }
            }
            return false;
        }
    }
}