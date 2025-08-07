using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using visualSysWeb.code;
using visualSysWeb.dao;
using visualSysWeb.modulos.Sped.code;
using visualSysWeb.modulos.Sped.dao;

namespace visualSysWeb.modulos.Sped.pages
{
    public partial class DocEletronicosAuditor : visualSysWeb.code.PagePadrao
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDataDe.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
                txtDataAte.Text = DateTime.Now.ToString("dd/MM/yyyy");
                pesquisar();
            }

            sopesquisa(pnBtn);
        }
        protected void pesquisar()
        {
            User usr = (User)Session["User"];
            String sql = "";

            sql = "sp_Cons_Documentos_Auditar '" + usr.getFilial() + "', '" + Funcoes.dtTry(txtDataDe.Text).ToString("yyyyMMdd") + "'" +
                    ", '" + Funcoes.dtTry(txtDataAte.Text).ToString("yyyyMMdd") + "', " + DllTipo.Text;

            SqlDataReader rs = null;

            try
            {
                rs = Conexao.consulta(sql, usr, false);
                List<docEletronico> lista = new List<docEletronico>();
                int nroPDV = 0;
                int nroExtrato = 0;
                while (rs.Read())
                {
                    if (nroPDV != 0)
                    {
                        if (int.Parse(rs["pdv"].ToString()) == nroPDV)
                        {
                            if ((int.Parse(rs["NroExtrato"].ToString()) - nroExtrato) > 1)
                            {
                                docEletronico doc = new docEletronico();
                                doc.pdv = int.Parse(rs["pdv"].ToString());
                                doc.emissao = DateTime.Parse(rs["emissao"].ToString());
                                doc.documento = rs["documento"].ToString();
                                doc.chave = rs["id_chave"].ToString();
                                doc.nroExtrato = int.Parse(rs["NroExtrato"].ToString());
                                doc.vlr = Funcoes.decTry(rs["vlr"].ToString());
                                lista.Add(doc);
                            }
                        }
                    }
                    nroPDV = int.Parse(rs["pdv"].ToString());
                    nroExtrato = int.Parse(rs["NroExtrato"].ToString());
                }

                gridDocumentos.DataSource = lista;
                gridDocumentos.DataBind();
            }
            catch (Exception erro)
            {
                throw erro;
            }
            finally
            {
                if (rs != null)
                    rs.Close();

            }
        }

        protected override void btnIncluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnEditar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisar();
        }

        protected override void btnExcluir_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnConfirmar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void btnCancelar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override bool campoDesabilitado(Control campo)
        {
            throw new NotImplementedException();
        }

        protected override bool campoObrigatorio(Control campo)
        {
            throw new NotImplementedException();
        }

        protected void imgBtnLimpar_Click(object sender, EventArgs e)
        {
            Session.Remove("filtro" + urlSessao());
            Response.Redirect("DocumentosEletronicos.aspx");
        }

        class docEletronico
        {
            public int pdv { get; set; }
            public DateTime emissao { get; set; }
            public string documento { get; set; }
            public string chave { get; set; }
            public int nroExtrato { get; set; }
            public decimal vlr { get; set; }
        }
    }
}