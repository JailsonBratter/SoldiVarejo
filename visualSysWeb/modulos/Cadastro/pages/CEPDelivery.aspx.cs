using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.pages
{
    public partial class CEPDelivery :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  
                  static String sqlGrid = "select * from CEP_Brasil_Delivery";//colocar os campos no select que ser?o apresentados na tela
                  protected void Page_Load(object sender, EventArgs e)
                  {
                     if (!IsPostBack)
                     {   
                       
                       consulta(true);
                      }
                      pesquisar(pnBtn);
                  }

                  private void consulta(bool limitar)
                  {
                      User usr = (User)Session["User"];

                      String sql = "";
                      if (!txtCEP.Text.Equals("")) //colocar nome do campo de pesquisa
                      {
                          sql = " CEP like '" + txtCEP.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                      }

                      if (!txtLogradouro.Text.Equals(""))
                      {
                          if(sql.Length>0)
                          {
                              sql += " and ";
                          }
                          sql += " logradouro like '%'" + txtLogradouro.Text + "%' ";
                      }
                      
                      try
                      {
                          DataTable tb;
                          if (!sql.Equals(""))
                          {
                              tb = Conexao.GetTable(sqlGrid + " where " + sql, usr,limitar);
                          }
                          else
                          {
                              tb = Conexao.GetTable(sqlGrid, usr,limitar);
                          }
                          gridPesquisa.DataSource = tb;
                          gridPesquisa.DataBind();
                          lblPesquisaErro.Text = "";
                      }
                      catch (Exception err)
                      {
                          lblPesquisaErro.Text = err.Message;
                      }


                  }
                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("~/modulos/Cadastro/pages/CEPDeliveryDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
                  }
                  
                  protected override void btnPesquisar_Click(object sender, EventArgs e)
                  {

                      consulta(false);
                      

                  }
                  protected override void btnEditar_Click(object sender, EventArgs e){}
                  protected override void btnExcluir_Click(object sender, EventArgs e) {}
                  protected override void btnConfirmar_Click(object sender, EventArgs e){}
                  protected override void btnCancelar_Click(object sender, EventArgs e){}   
                  
                  
                 protected override bool campoObrigatorio(Control campo)
                 { 
                       return false;
                 }
                 
                 protected override bool campoDesabilitado(Control campo)
                 {
                       return false;
                 }


                
    }
}