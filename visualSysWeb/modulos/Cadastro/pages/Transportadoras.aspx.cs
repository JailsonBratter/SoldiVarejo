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
    public partial class Transportadoras :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  static DataTable tb;
                  static String sqlGrid = "select *,dpadrao = case when padrao =1 then 'SIM' ELSE 'NAO' END from Transportadora";//colocar os campos no select que ser?o apresentados na tela
                  protected void Page_Load(object sender, EventArgs e)
                  {
                     if (!IsPostBack)
                     {
                         pesquisar();
                      
                      }
                      pesquisar(pnBtn);
                      txtCNPJ.Attributes.Add("OnKeyPress", "javascript:return formataCNPJ(this,event);");
                  }
                  
                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("TransportadorasDetalhes.aspx?novo=true"); 
                  }
                  private void pesquisar()
                  {
                      String sql = "";
                      String strSqltotalFiltro = "select count(*) from Transportadora ";
                      String totalRegistro = Conexao.retornaUmValor(strSqltotalFiltro, null);

                      if (!txtNome.Text.Equals(""))
                      {
                          sql = " (Nome_transportadora like '%" + txtNome.Text + "%' or Razao_social like '%" + txtNome.Text + "%')";
                      }
                      if (!txtCNPJ.Text.Equals(""))
                      {
                          if (!sql.Equals(""))
                          {
                              sql += " and ";
                          }
                          sql += " replace(replace(replace(cnpj,'.',''),'-',''),'/','') like '%" + txtCNPJ.Text.Replace(".", "").Replace("-", "").Replace("/", "") + "%'";
                      }
                      try
                      {
                          User usr = (User)Session["User"];
                          if (!sql.Equals(""))
                          {
                              tb = Conexao.GetTable(sqlGrid + " where " + sql, usr, false);
                              strSqltotalFiltro += " where " + sql;
                          }
                          else
                          {
                              tb = Conexao.GetTable(sqlGrid, usr, true);
                          }
                          gridPesquisa.DataSource = tb;
                          gridPesquisa.DataBind();
                          lblPesquisaErro.Text = "";
                          String totalFiltro = Conexao.retornaUmValor(strSqltotalFiltro, usr);
                          lblRegistros.Text = totalFiltro + " Registros de  " + totalRegistro + " Cadastrados ";
                    
                      }
                      catch (Exception err)
                      {
                          lblPesquisaErro.Text = err.Message;
                      }
                  }
                  protected override void btnPesquisar_Click(object sender, EventArgs e)
                  {
                      pesquisar();
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