using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace visualSysWeb.dao
{
    public class natureza_operacaoDAO
    {
        public Decimal Codigo_operacao { get; set; }
        public String Descricao { get; set; }
        public bool Gera_apagar_receber { get; set; }
        public bool Gera_venda { get; set; }
        public bool Baixa_estoque { get; set; }
        public bool Incide_ICMS = true;
        public bool Incide_IPI = true;
        public bool Imprime_NF { get; set; }
        public bool Permite_Desconto { get; set; }
        public bool Gera_caderneta { get; set; }
        public bool NF_devolucao { get; set; }
        public bool Gera_custo { get; set; }
        public bool Saida { get; set; }
        public bool Tipo_movimentacao { get; set; }
        public bool incide_ST { get; set; }
        public bool incide_PisCofins = false;
        public bool Preco_Venda = true;
        public String filial { get; set; }
        public String cst_pis_cofins = "";
        public String Tributacao_padrao = "";
        public bool ipi_base { get; set; }
        public bool despesas_base { get; set; }
        public string cfop { get; set; } = "";
        public string cfop_st { get; set; } = "";
        public bool utilizaCFOP { get; set; } = false;
        public bool IncideCustoMedio { get; set; }  = false;
        public bool Precificacao { get; set; } = false;
        public bool Inativa { get; set; } = false;
        public string cst_ICMS { get; set; } = "";
        public bool CNPJDestOrigem { get; set; } = false;
        public bool Difal { get; set; } = false;

        public natureza_operacaoDAO()
        {

        }
        public natureza_operacaoDAO(String codigoOperacao, User usr, bool soAtiva = false)
        { //colocar campo index da tabela
            if (codigoOperacao.Equals(""))
            {
                throw new Exception("Natureza de operação invalida");

            }

            String sql = "Select * from natureza_operacao where codigo_operacao =" + codigoOperacao.ToString().Replace(',', '.') ;
            if (soAtiva)
            {
                sql += " AND ISNULL(Natureza_Operacao.Inativa, 0) = 0";
            }
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }

        public static bool existeNaturezaOperacaocfop(String codigoOperacao, User usr)
        {
            SqlDataReader rs = null;
            try
            {


                String sql = "Select * from  natureza_operacao where codigo_operacao =" + codigoOperacao.ToString().Replace(',', '.');
                rs = Conexao.consulta(sql, usr, true);

                if (rs.Read())
                    return true;
                else
                {
                    sql = "Select * from  cfop where cfop =" + codigoOperacao.ToString().Replace(',', '.');
                    rs = Conexao.consulta(sql, usr, true);
                    if (rs.Read())
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
            finally
            {
                if (rs != null)
                    rs.Close();

            }
        }
        private String dataBr(DateTime dt)
        {
            if (dt.ToString("dd/MM/yyyy").Equals("01/01/0001"))
            {
                return "";
            }
            else
            {
                return dt.ToString("dd/MM/yyyy");
            }
        }
        public void carregarDados(SqlDataReader rs)
        {
            try
            {

                if (rs.Read())
                {
                    Codigo_operacao = (Decimal)(rs["Codigo_operacao"].ToString().Equals("") ? new Decimal() : rs["Codigo_operacao"]);
                    Descricao = rs["Descricao"].ToString();
                    Gera_apagar_receber = (rs["Gera_apagar_receber"].ToString().Equals("1") ? true : false);
                    Gera_venda = (rs["Gera_venda"].ToString().Equals("1") ? true : false);
                    Baixa_estoque = (rs["Baixa_estoque"].ToString().Equals("1") ? true : false);
                    Incide_ICMS = (rs["Incide_ICMS"].ToString().Equals("1") ? true : false);
                    incide_PisCofins = (rs["incide_PisCofins"].ToString().Equals("1") ? true : false);

                    Incide_IPI = (rs["Incide_IPI"].ToString().Equals("1") ? true : false);
                    Imprime_NF = (rs["Imprime_NF"].ToString().Equals("1") ? true : false);
                    Permite_Desconto = (rs["Permite_Desconto"].ToString().Equals("1") ? true : false);
                    Gera_caderneta = (rs["Gera_caderneta"].ToString().Equals("1") ? true : false);
                    NF_devolucao = (rs["NF_devolucao"].ToString().Equals("1") ? true : false);
                    Gera_custo = (rs["Gera_custo"].ToString().Equals("1") ? true : false);
                    Saida = (rs["Saida"].ToString().Equals("1") ? true : false);
                    Tipo_movimentacao = (rs["Tipo_movimentacao"].ToString().Equals("1") ? true : false);
                    incide_ST = rs["incide_ST"].ToString().Equals("1") ;
                    Preco_Venda = rs["Preco_Venda"].ToString().Equals("1") ;

                    filial = rs["filial"].ToString();
                    cst_pis_cofins = rs["cst_pis_cofins"].ToString();
                    ipi_base = rs["ipi_base"].ToString().Equals("1");
                    despesas_base = rs["despesas_base"].ToString().Equals("1");
                    Tributacao_padrao = rs["tributacao_padrao"].ToString();
                  
                    cfop = rs["cfop"].ToString();
                    cfop_st = rs["cfop_st"].ToString();

                    utilizaCFOP = rs["utilizaCFOP"].ToString().Equals("1");
                    IncideCustoMedio = (rs["Incide_Custo_Medio"].ToString().Equals("1") ? true : false);
                    Precificacao = (rs["Precificacao"].ToString().Equals("1") ? true : false);
                    Inativa = (rs["Inativa"].ToString().Equals("1") ? true : false);
                    cst_ICMS = rs["cst_icms"].ToString();
                    CNPJDestOrigem = (rs["Destinatario_Origem"].ToString().Equals("1") ? true : false);
                    Difal = (rs["Difal"].ToString().Equals("1") ? true : false);
                }
                else
                {
                    throw new Exception("Natureza de Operação invalida");
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
        private void update()
        {
            try
            {
                String sql = "update  natureza_operacao set " +

                              "Descricao='" + Descricao + "'" +
                              ",Gera_apagar_receber=" + (Gera_apagar_receber ? "1" : "0") +
                              ",Gera_venda=" + (Gera_venda ? "1" : "0") +
                              ",Baixa_estoque=" + (Baixa_estoque ? "1" : "0") +
                              ",Incide_ICMS=" + (Incide_ICMS ? "1" : "0") +
                              ",Incide_IPI=" + (Incide_IPI ? "1" : "0") +
                              ",Imprime_NF=" + (Imprime_NF ? "1" : "0") +
                              ",Permite_Desconto=" + (Permite_Desconto ? "1" : "0") +
                              ",Gera_caderneta=" + (Gera_caderneta ? "1" : "0") +
                              ",NF_devolucao=" + (NF_devolucao ? "1" : "0") +
                              ",Gera_custo=" + (Gera_custo ? "1" : "0") +
                              ",Saida=" + (Saida ? "1" : "0") +
                              ",Tipo_movimentacao=" + (Tipo_movimentacao ? "1" : "0") +
                              ",incide_ST=" + (incide_ST ? "1" : "0") +
                              ",filial='MATRIZ'" +
                              ",incide_PisCofins=" + (incide_PisCofins ? "1" : "0") +
                              ",Preco_Venda=" + (Preco_Venda ? "1" : "0") +
                              ",cst_pis_cofins='" + cst_pis_cofins + "'" +
                              ",ipi_base=" + (ipi_base ? "1" : "0") +
                              ",despesas_base =" + (despesas_base ? "1" : "0") +
                              ",tributacao_padrao='" + Tributacao_padrao + "'" +
                              ",cfop ='" + cfop + "'" +
                              ",cfop_st='" + cfop_st + "'" +
                              ",utilizaCFOP=" + (utilizaCFOP ? "1" : "0") +
                              ",Incide_Custo_Medio=" + (IncideCustoMedio ? "1" : "0") +
                              ",Precificacao = " + (Precificacao ? "1" : "0") +
                              ",Inativa = " + (Inativa ? "1" : "0") +
                              ",CST_ICMS = '" + cst_ICMS+ "'" +
                              ", Destinatario_Origem = " + (CNPJDestOrigem ? "1" : "0") +
                              ", Difal = " + (Difal ? "1" : "0") +
                    "  where codigo_operacao = " + Codigo_operacao.ToString().Replace(",", ".") //ARRUMAR CAMPO INDEX
                        ;
                Conexao.executarSql(sql);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public bool salvar(bool novo)
        {

            if (novo)
            {
                insert();
            }
            else
            {
                update();
            }
            return true;
        }

        public bool excluir()
        {
            String sql = "delete from natureza_operacao  where codigo_operacao = " + Codigo_operacao.ToString().Replace(",", "."); //colocar campo index
            Conexao.executarSql(sql);
            return true;
        }

        private void insert()
        {
            try
            {
                String sql = " insert into natureza_operacao (" +
                          "Codigo_operacao," +
                          "Descricao," +
                          "Gera_apagar_receber," +
                          "Gera_venda," +
                          "Baixa_estoque," +
                          "Incide_ICMS," +
                          "Incide_IPI," +
                          "Imprime_NF," +
                          "Permite_Desconto," +
                          "Gera_caderneta," +
                          "NF_devolucao," +
                          "Gera_custo," +
                          "Saida," +
                          "Tipo_movimentacao," +
                          "incide_ST," +
                          "filial," +
                          "incide_PisCofins" +
                          ",Preco_Venda" +
                          ",cst_pis_cofins"+
                          ",ipi_base"+
                          ",despesas_base"+
                          ",tributacao_padrao" +
                          ",cfop"+
                          ",cfop_st"+
                          ",utilizaCFOP"+
                          ",Incide_Custo_Medio"+
                          ",Precificacao"+
                          ",Inativa" + 
                          ",cst_icms " +
                          ", Destinatario_Origem"+
                          ", Difal" +
                     ") values (" +
                          Codigo_operacao.ToString().Replace(",", ".") +
                          "," + "'" + Descricao + "'" +
                          "," + (Gera_apagar_receber ? 1 : 0) +
                          "," + (Gera_venda ? 1 : 0) +
                          "," + (Baixa_estoque ? 1 : 0) +
                          "," + (Incide_ICMS ? 1 : 0) +
                          "," + (Incide_IPI ? 1 : 0) +
                          "," + (Imprime_NF ? 1 : 0) +
                          "," + (Permite_Desconto ? 1 : 0) +
                          "," + (Gera_caderneta ? 1 : 0) +
                          "," + (NF_devolucao ? 1 : 0) +
                          "," + (Gera_custo ? 1 : 0) +
                          "," + (Saida ? 1 : 0) +
                          "," + (Tipo_movimentacao ? 1 : 0) +
                          "," + (incide_ST ? 1 : 0) +
                          "," + "'MATRIZ'" +
                          "," + (incide_PisCofins ? 1 : 0) +
                          "," + (Preco_Venda ? 1 : 0) +
                          ",'"+cst_pis_cofins+"'"+
                          ","+(ipi_base?"1":"0")+
                          ","+(despesas_base?"1":"0")+
                          ",'"+Tributacao_padrao+"'"+
                          ",'"+cfop+"'"+
                          ",'"+cfop_st+"'"+
                          ","+(utilizaCFOP?"1":"0")+
                          ","+(IncideCustoMedio ? "1" : "0")+
                          ","+(Precificacao ? "1" : "0")+
                          ","+(Inativa ? "1" : "0")+
                          ", '" + cst_ICMS + "'"+
                          ", " + (CNPJDestOrigem ? "1" : "0")+
                          ", " + (Difal ? "1" : "0")+
                         ");";

                Conexao.executarSql(sql);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}/* 
/*================================Metodos tela de Pesquisa==========================================
using System.Data; 
using visualSysWeb.dao;
           :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  static DataTable tb;
                  static String sqlGrid = ""select * from natureza_operacao";//colocar os campos no select que ser?o apresentados na tela
                  protected void Page_Load(object sender, EventArgs e)
                  {
                     if (!IsPostBack)
                     {   
                       User usr = (User)Session["User"];
                       tb = Conexao.GetTable(sqlGrid ,usr); 
                       gridPesquisa.DataSource = tb;
                       gridPesquisa.DataBind();
                       Lblindex.Text = "1/" + gridPesquisa.PageCount;
                      }
                      pesquisar(pnBtn);
                  }
                  
                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ natureza_operacaoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
                  }
                  
                  protected override void btnPesquisar_Click(object sender, EventArgs e)
                  {
                      String sql = "";
                      if (!txtPESQ1.Text.Equals("")) //colocar nome do campo de pesquisa
                      {
                          sql = " campoPesquisa1 like '" + txtPESQ1.Text + "%'"; // preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa
                      }
                      if (!txtPESQ2.Text.Equals("")) //colocar nome do campo de pesquisa2
                      {
                          if (!sql.Equals(""))
                          {
                              sql += " and ";     
                          }
                         sql += "campoPesquisa2 = '" + txtPESQ2.Text + "'";//preencher com os campos que ser?o apresentados na grid e o campo que ser? feito a pesquisa 
                      }
                         try
                         {
                            User usr = (User)Session["User"];
                            if (!sql.Equals(""))
                            {
                               tb = Conexao.GetTable(sqlGrid+" where "+sql, usr);
                             }
                             else
                             {
                               tb = Conexao.GetTable(sqlGrid, usr);
                              }
                               gridPesquisa.DataSource = tb;
                               gridPesquisa.DataBind();
                               lblPesquisaErro.Text = "";
                               Lblindex.Text = "1/" + gridPesquisa.PageCount;
                        }catch (Exception err)
                         {
                                      lblPesquisaErro.Text = err.Message;
                         }
                  }
                  protected override void btnEditar_Click(object sender, EventArgs e){}
                  protected override void btnExcluir_Click(object sender, EventArgs e) {}
                  protected override void btnConfirmar_Click(object sender, EventArgs e){}
                  protected override void btnCancelar_Click(object sender, EventArgs e){}   
                  
                  
                  protected void gridPesquisa_PageIndexChanging(object sender, GridViewPageEventArgs e)
                  {
                    gridPesquisa.DataSource = tb;
                    gridPesquisa.PageIndex = e.NewPageIndex;
                    Lblindex.Text = (e.NewPageIndex+1)+"/" + gridPesquisa.PageCount;
                    gridPesquisa.DataBind();
                  }
                 protected override bool campoObrigatorio(Control campo)
                 { 
                       return false;
                 }
                 
                 protected override bool campoDesabilitado(Control campo)
                 {
                       return false;
                 }
                 
*/

/*================================html tela de Pesquisa==========================================
                  
   <center><h1>natureza_operacao</h1></center>
    <hr />              
       <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">               
       </asp:Panel>           
       <br />           
       <div class="filter" id="filtrosPesq" runat="server" visible="false">           
         <table>           
           <asp:Label ID="lblPesquisaErro" runat="server" Text="" ForeColor="Red"></asp:Label>           
            <tr>           
                <td>           
                <p>CAMPO DE PESQUISA 1</p>   
                <asp:TextBox ID="txtPESQ1" runat="server" ></asp:TextBox></asp:TextBox>  
                </td>  
                <td>  
                   <p>CAMPO DE PESQUISA 2</p>  
                   <asp:TextBox ID="txtPESQ2" runat="server" > </asp:TextBox>
                </td>  
            </tr>      
                  
                  
                  
         </table>           
        </div>            
        <div class="gridTable">          
            <asp:GridView ID="gridPesquisa" runat="server" AutoGenerateColumns="False"           
                 AllowPaging="True" 
                 PageSize="20"  
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="16"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Codigo_operacao" Text="Codigo_operacao" Visible="true" 
                    HeaderText="Codigo_operacao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Descricao" Text="Descricao" Visible="true" 
                    HeaderText="Descricao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Gera_apagar_receber" Text="Gera_apagar_receber" Visible="true" 
                    HeaderText="Gera_apagar_receber" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Gera_venda" Text="Gera_venda" Visible="true" 
                    HeaderText="Gera_venda" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Baixa_estoque" Text="Baixa_estoque" Visible="true" 
                    HeaderText="Baixa_estoque" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Incide_ICMS" Text="Incide_ICMS" Visible="true" 
                    HeaderText="Incide_ICMS" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Incide_IPI" Text="Incide_IPI" Visible="true" 
                    HeaderText="Incide_IPI" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Imprime_NF" Text="Imprime_NF" Visible="true" 
                    HeaderText="Imprime_NF" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Permite_Desconto" Text="Permite_Desconto" Visible="true" 
                    HeaderText="Permite_Desconto" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Gera_caderneta" Text="Gera_caderneta" Visible="true" 
                    HeaderText="Gera_caderneta" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="NF_devolucao" Text="NF_devolucao" Visible="true" 
                    HeaderText="NF_devolucao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Gera_custo" Text="Gera_custo" Visible="true" 
                    HeaderText="Gera_custo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Saida" Text="Saida" Visible="true" 
                    HeaderText="Saida" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Tipo_movimentacao" Text="Tipo_movimentacao" Visible="true" 
                    HeaderText="Tipo_movimentacao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="incide_ST" Text="incide_ST" Visible="true" 
                    HeaderText="incide_ST" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="filial" Text="filial" Visible="true" 
                    HeaderText="filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  


                 </Columns> 
                 <EditRowStyle BackColor="#999999" /> 
                 <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" /> 
                 <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" /> 
                 <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" /> 
                 <RowStyle BackColor="#F7F6F3" ForeColor="#333333" /> //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                 <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" /> 
                 <SortedAscendingCellStyle BackColor="#E9E7E2" /> 
                 <SortedAscendingHeaderStyle BackColor="#506C8C" /> 
                 <SortedDescendingCellStyle BackColor="#FFFDF8" /> 
                  <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
           </asp:GridView>           
           <br />       
           <center><asp:Label ID="Lblindex" runat="server" Text="1/.."></asp:Label></center>       
        </div>          
                  
*/
/*================================Metodos tela detalhes==========================================
using System.Data; 
using visualSysWeb.dao;
using System.Data.SqlClient;
                 : visualSysWeb.code.PagePadrao
  {
                 protected static natureza_operacaoDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new natureza_operacaoDAO();
                      tabMenu.Items[MultiView1.ActiveViewIndex].Selected = true;
                      if (Request.Params["novo"] != null) 
                      {
                        status = "incluir";
                                         EnabledControls(conteudo, true);
                                         EnabledControls(cabecalho, true);
                      }
                      else
                      {
                           if (Request.Params["campoIndex"] != null)  // colocar o campo index da tabela
                           {
                              try
                              {
                                   if (!IsPostBack)
                                   {
                                        String index = Request.Params["campoIndex"].ToString();// colocar o campo index da tabela
                                        status = "visualizar";
                                        obj = new natureza_operacaoDAO(index,usr);
                                        carregarDados();
                                    }
                                    if (status.Equals("visualizar"))
                                    {
                                         EnabledControls(conteudo, false);
                                         EnabledControls(cabecalho, false);
                                    }else{
                                         EnabledControls(conteudo, true);
                                         EnabledControls(cabecalho, true);
                                    }
                                }
                                catch (Exception err)
                                {
                                   lblError.Text = err.Message;                 
                                }
                           }
                       }
                    carregabtn(pnBtn);
                  }
                 
                 private void limparCampos(){
                    LimparCampos(cabecalho);          
                    LimparCampos(conteudo);             
                 }
                 
                 protected bool validaCamposObrigatorios() {
                    if (validaCampos(cabecalho) && validaCampos(conteudo))
                             return true;
                    else
                             return false;
                 }
                 
                 protected override bool campoObrigatorio(Control campo)
                 {// colocar os nomes dos campos obrigarios no Array
                     String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
                       return existeNoArray(campos, campo.ID+"");
                 }
                 
                 protected override bool campoDesabilitado(Control campo)
                 {// colocar os nomes dos campos Desabilitados no Array
                     String[] campos = { "", 
                                    "", 
                                    "", 
                                    "" 
                                     };
                       return existeNoArray(campos, campo.ID+"");
                 }
                 protected override void btnIncluir_Click(object sender, EventArgs e)
                 {
                    incluir(pnBtn);
                 }
                 
                 protected override void btnEditar_Click(object sender, EventArgs e)
                 {
                    editar(pnBtn);
                    EnabledControls(cabecalho, true);
                    EnabledControls(conteudo, true);
                 }
                  
                 protected override void btnPesquisar_Click(object sender, EventArgs e)
                 {
                 Response.Redirect("nomepaginapesquisa.aspx"); //colocar o endereco da tela de pesquisa
                 }
                  
                 protected override void btnExcluir_Click(object sender, EventArgs e)
                 {
                     pnConfima.Visible = true;
                  }
                  
                  protected override void btnConfirmar_Click(object sender, EventArgs e)
                  {
                     try
                     {
                       if (validaCamposObrigatorios())
                       {
                  
                             carregarDadosObj();
                             obj.salvar(status.Equals("incluir")); // se for incluir true se n?o falso;
                             lblError.Text = "Salvo com Sucesso";
                             lblError.ForeColor = System.Drawing.Color.Blue;
                             EnabledControls(cabecalho, false);
                             EnabledControls(conteudo, false);
                             visualizar(pnBtn);
                       }
                       else
                       {
                            lblError.Text = "Campo Obrigatorio n?o preenchido";
                            lblError.ForeColor = System.Drawing.Color.Red;
                        }
                     }
                     catch (Exception err)
                     {
                         lblError.Text = err.Message;
                         lblError.ForeColor = System.Drawing.Color.Red;
                     }
                  }
                  
                  protected override void btnCancelar_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("nomepaginapesquisa.aspx");//colocar endereco pagina de pesquisa
                  }
                  protected void tabMenu_MenuItemClick(object sender, MenuEventArgs e)
                  {
                      switch (e.Item.Value)
                      {
                          case "tab1":
                          MultiView1.ActiveViewIndex = 0;
                          break;
                       }
                   }
                    //--Atualizar DaoForm 
      private void carregarDados()
      {
                                         txtCodigo_operacao.Text=string.Format("{0:0,0.00}",obj.Codigo_operacao);
                                         txtDescricao.Text=obj.Descricao.ToString();
                                         chkGera_apagar_receber.Checked =obj.Gera_apagar_receber;
                                         chkGera_venda.Checked =obj.Gera_venda;
                                         chkBaixa_estoque.Checked =obj.Baixa_estoque;
                                         chkIncide_ICMS.Checked =obj.Incide_ICMS;
                                         chkIncide_IPI.Checked =obj.Incide_IPI;
                                         chkImprime_NF.Checked =obj.Imprime_NF;
                                         chkPermite_Desconto.Checked =obj.Permite_Desconto;
                                         chkGera_caderneta.Checked =obj.Gera_caderneta;
                                         chkNF_devolucao.Checked =obj.NF_devolucao;
                                         chkGera_custo.Checked =obj.Gera_custo;
                                         chkSaida.Checked =obj.Saida;
                                         chkTipo_movimentacao.Checked =obj.Tipo_movimentacao;
                                         chkincide_ST.Checked =obj.incide_ST;
                                         txtfilial.Text=obj.filial.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Codigo_operacao=Decimal.Parse(txtCodigo_operacao.Text);
                                         obj.Descricao=txtDescricao.Text;
                                         obj.Gera_apagar_receber=chkGera_apagar_receber.Checked ;
                                         obj.Gera_venda=chkGera_venda.Checked ;
                                         obj.Baixa_estoque=chkBaixa_estoque.Checked ;
                                         obj.Incide_ICMS=chkIncide_ICMS.Checked ;
                                         obj.Incide_IPI=chkIncide_IPI.Checked ;
                                         obj.Imprime_NF=chkImprime_NF.Checked ;
                                         obj.Permite_Desconto=chkPermite_Desconto.Checked ;
                                         obj.Gera_caderneta=chkGera_caderneta.Checked ;
                                         obj.NF_devolucao=chkNF_devolucao.Checked ;
                                         obj.Gera_custo=chkGera_custo.Checked ;
                                         obj.Saida=chkSaida.Checked ;
                                         obj.Tipo_movimentacao=chkTipo_movimentacao.Checked ;
                                         obj.incide_ST=chkincide_ST.Checked ;
                                         obj.filial=txtfilial.Text;
   }

                  
                  protected void lista_click(object sender, ImageClickEventArgs e)
                  {
                      ImageButton btn = (ImageButton)sender;
                      pnFundo.Visible = true;
                      chkLista.Items.Clear();
                      String sqlLista = "";
                  
                      switch (btn.ID)
                      {
                          case "idBotao":
                              sqlLista = "Query de pesquisa com no minimo 2campos";
                              lbllista.Text = "Pagamentos";
                              camporeceber = "txtPagamento";
                              break;
                      }
                      User usr = (User)Session["User"];
                      SqlDataReader lista = Conexao.consulta(sqlLista, usr);
                  
                      while (lista.Read())
                      {
                          ListItem item = new ListItem();
                          item.Value = lista[0].ToString();
                          item.Text = lista[1].ToString();
                          chkLista.Items.Add(item);
                       }
                  }
                  
                  protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)
                  {
                      try
                      {
                          obj.excluir();
                          pnConfima.Visible = false;
                          lblError.Text = "Registro Excluido com sucesso";
                          limparCampos();
                          pesquisar(pnBtn);
                       }
                       catch (Exception err)
                        {
                               lblError.Text = "N?o foi possivel Excluir o registro error:" +err.Message;
                         }
                  }
                  
                  protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)
                  {
                      pnConfima.Visible = false;
                  }
                  
                  protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)
                  {
                      TextBox txt = (TextBox)conteudo.FindControl(camporeceber);
                      txt.Text = "";
                      for (int i = 0; i < chkLista.Items.Count; i++)
                      {
                          if (chkLista.Items[i].Selected)
                          {
                              txt.Text += chkLista.Items[i].Value;
                         }
                     }
                     pnFundo.Visible = false;
                  }
                  
                  protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)
                  {
                      pnFundo.Visible = false;
                  }
                  
                  
                  
/*================================HTML Pagina Detalhes==========================================
<div class="cabMenu">                  
       <center> <h1>Detalhes do natureza_operacao</h1></center>                  
</div>                  
    <asp:Panel ID="pnBtn" runat="server" CssClass="cabMenu">                  
    </asp:Panel>                  
    <br />              
     <asp:Label ID="lblError" runat="server" Text="" ForeColor=Red></asp:Label>              
                  
    <div id="cabecalho" runat="server" class="frame" >               
     <!--Coloque aqui os campos do cabe?alho    -->         
        <table>              
              <tr>    
                  <td></td>
              </tr>    
        </table>          
    </div>              
<div class="opcoes">                  
    <asp:Menu ID="tabMenu" runat="server" Orientation="Horizontal"               
                 OnMenuItemClick="tabMenu_MenuItemClick" Visible="true" > 
                  
       <Items>              
           <asp:MenuItem Text="Primeira Tab" Value="tab1" />         
       </Items>             
       <StaticMenuStyle CssClass="tab" />              
       <StaticMenuItemStyle CssClass="item" />             
       <staticselectedstyle backcolor="Beige" ForeColor="#465c71" />            
    </asp:Menu>              
</div>                  
                  
<div id="conteudo" runat="server" class="conteudo" enableviewstate="false">                  
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">              
       <asp:View ID="view1" runat="server" >              
           <table>              
                <tr>    
/*--Campos Form
                                      <td >                   <p>Codigo_operacao</p>
                   <asp:TextBox ID="txtCodigo_operacao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Descricao</p>
                   <asp:TextBox ID="txtDescricao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Gera_apagar_receber</p>
                   <td><asp:CheckBox ID="chkGera_apagar_receber" runat="server" Text="Gera_apagar_receber"/>
                   </td>

                                      <td >                   <p>Gera_venda</p>
                   <td><asp:CheckBox ID="chkGera_venda" runat="server" Text="Gera_venda"/>
                   </td>

                                      <td >                   <p>Baixa_estoque</p>
                   <td><asp:CheckBox ID="chkBaixa_estoque" runat="server" Text="Baixa_estoque"/>
                   </td>

                                      <td >                   <p>Incide_ICMS</p>
                   <td><asp:CheckBox ID="chkIncide_ICMS" runat="server" Text="Incide_ICMS"/>
                   </td>

                                      <td >                   <p>Incide_IPI</p>
                   <td><asp:CheckBox ID="chkIncide_IPI" runat="server" Text="Incide_IPI"/>
                   </td>

                                      <td >                   <p>Imprime_NF</p>
                   <td><asp:CheckBox ID="chkImprime_NF" runat="server" Text="Imprime_NF"/>
                   </td>

                                      <td >                   <p>Permite_Desconto</p>
                   <td><asp:CheckBox ID="chkPermite_Desconto" runat="server" Text="Permite_Desconto"/>
                   </td>

                                      <td >                   <p>Gera_caderneta</p>
                   <td><asp:CheckBox ID="chkGera_caderneta" runat="server" Text="Gera_caderneta"/>
                   </td>

                                      <td >                   <p>NF_devolucao</p>
                   <td><asp:CheckBox ID="chkNF_devolucao" runat="server" Text="NF_devolucao"/>
                   </td>

                                      <td >                   <p>Gera_custo</p>
                   <td><asp:CheckBox ID="chkGera_custo" runat="server" Text="Gera_custo"/>
                   </td>

                                      <td >                   <p>Saida</p>
                   <td><asp:CheckBox ID="chkSaida" runat="server" Text="Saida"/>
                   </td>

                                      <td >                   <p>Tipo_movimentacao</p>
                   <td><asp:CheckBox ID="chkTipo_movimentacao" runat="server" Text="Tipo_movimentacao"/>
                   </td>

                                      <td >                   <p>incide_ST</p>
                   <td><asp:CheckBox ID="chkincide_ST" runat="server" Text="incide_ST"/>
                   </td>

                                      <td >                   <p>filial</p>
                   <asp:TextBox ID="txtfilial" runat="server" ></asp:TextBox>
                   </td>


                </tr>    
           </table>          
       </asp:View>              
    </asp:MultiView>                
</div>                  
        <asp:Panel ID="pnFundo" runat="server" CssClass="fundo" Visible =false>          
              <asp:Label ID="lbllista" runat="server" Text="Label" CssClass="cabMenu"></asp:Label>           
                    <table class="frame">
                       <tr>
                           <td>          
                             <asp:ImageButton ID="btnConfirmaLista" runat="server" ImageUrl="~/img/confirm.png"                    <td>       
                              Width="25px" onclick="btnConfirmaLista_Click"   />           
                              <asp:Label ID="Label4" runat="server" Text="Seleciona" ></asp:Label>          
                           </td>           
                           <td>          
                                    <asp:ImageButton ID="btnCancelaLista" runat="server" ImageUrl="~/img/cancel.png" 
                                       Width="25px" onclick="btnCancelaLista_Click"  />                   
                                    <asp:Label ID="Label5" runat="server" Text="Cancela" ></asp:Label>     
                           </td>           
                       </tr>
                     </table>
                  
                      <asp:Panel ID="Panel1" runat="server" CssClass="lista" >   
                             <asp:RadioButtonList ID="chkLista" runat="server" Height=50 Width=200>
                             </asp:RadioButtonList>
                      </asp:Panel>
         </asp:Panel>      
                  
         <asp:Panel ID="pnConfima" runat="server" CssClass="fundo" Visible =false>         
           <asp:Label ID="Label1" runat="server" Text="Confirma Exclus?o" CssClass="cabMenu"></asp:Label>         
             <table class="frame">          
                  <tr>     
                      <td>             
                             <asp:ImageButton ID="btnConfirmaExclusao" runat="server" ImageUrl="~/img/confirm.png" 
                                     Width="25px" onclick="btnConfirmaExclusao_Click"  /> 
                                     <asp:Label ID="Label2" runat="server" Text="Confirma" ></asp:Label>
                      </td>
                      <td>
                                    <asp:ImageButton ID="btnCancelaExclusao" runat="server" ImageUrl="~/img/cancel.png" 
                                     Width="25px" onclick="btnCancelaExclusao_Click"  /> 
                                     <asp:Label ID="Label3" runat="server" Text="Cancela" ></asp:Label>
                      </td>
                  </tr>
              </table>     
         </asp:Panel>         
*/

