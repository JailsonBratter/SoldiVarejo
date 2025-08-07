using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class tributacaoDAO
    {
        public Decimal Codigo_Tributacao = 0;
        public String Descricao_Tributacao { get; set; }
        public String Filial = "MATRIZ";
        public Decimal Saida_ICMS { get; set; }
        public Decimal Nro_ECF { get; set; }
        public bool Gera_Mapa { get; set; }
        public String Indice_ST { get; set; }
        public String csosn = "";
        public Decimal Entrada_ICMS { get; set; }
        public Decimal Redutor { get; set; }
        public bool Incide_ICMS { get; set; }
        public bool Incide_ICM_Subistituicao { get; set; }
        public Decimal ICMS_Efetivo { get; set; }
        public Decimal cfop = 0;
        public Decimal cfop_entrada = 0;
        public String cst_sped = "";
        public bool ipi_EmOutrasDespesas = false;
        public bool icmsst_emOutrasDespesas = false;
       


        public tributacaoDAO() { }
        public tributacaoDAO(String campoIndex, User usr)
        { //colocar campo index da tabela
            String sql = "Select * from  tributacao where codigo_tributacao =" + campoIndex;
            SqlDataReader rs = null;
            try
            {

            
            rs =Conexao.consulta(sql, null, true);
            carregarDados(rs);
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

        public static bool existe(Decimal codigoTributacao, User usr)
        {
            String sql = "Select * from  tributacao where codigo_tributacao =" + codigoTributacao.ToString();
            SqlDataReader rs= null;
            try
            {

                rs = Conexao.consulta(sql, null, true);
                if (rs.Read())
                {
                    return true;
                }
                else
                {
                    return false;
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
                    Codigo_Tributacao = (Decimal)(rs["Codigo_Tributacao"].ToString().Equals("") ? new Decimal() : rs["Codigo_Tributacao"]);
                    Descricao_Tributacao = rs["Descricao_Tributacao"].ToString();
                    Filial = rs["Filial"].ToString();
                    Saida_ICMS = (Decimal)(rs["Saida_ICMS"].ToString().Equals("") ? new Decimal() : rs["Saida_ICMS"]);
                    Nro_ECF = (Decimal)(rs["Nro_ECF"].ToString().Equals("") ? new Decimal() : rs["Nro_ECF"]);
                    Gera_Mapa = (rs["Gera_Mapa"].ToString().Equals("1") ? true : false);
                    Indice_ST = rs["Indice_ST"].ToString();
                    Entrada_ICMS = (Decimal)(rs["Entrada_ICMS"].ToString().Equals("") ? new Decimal() : rs["Entrada_ICMS"]);
                    Redutor = (Decimal)(rs["Redutor"].ToString().Equals("") ? new Decimal() : rs["Redutor"]);
                    Incide_ICMS = (rs["Incide_ICMS"].ToString().Equals("1") ? true : false);
                    Incide_ICM_Subistituicao = (rs["Incide_ICM_Subistituicao"].ToString().Equals("1") ? true : false);
                    ICMS_Efetivo = (Decimal)(rs["ICMS_Efetivo"].ToString().Equals("") ? new Decimal() : rs["ICMS_Efetivo"]);
                    csosn = rs["csosn"].ToString();
                    Decimal.TryParse(rs["cfop"].ToString(), out cfop);
                    cfop_entrada = Funcoes.decTry(rs["CFOP_entrada"].ToString());
                    cst_sped = rs["cst_sped"].ToString();
                    ipi_EmOutrasDespesas = rs["ipi_emOutrasDespesas"].ToString().Equals("1");
                    icmsst_emOutrasDespesas = rs["icmsst_emoutrasdespesas"].ToString().Equals("1");
                    
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
                String sql = "update  tributacao set " +
                              "Descricao_Tributacao='" + Descricao_Tributacao + "'" +
                              ",Filial='" + Filial + "'" +
                              ",Saida_ICMS=" + Saida_ICMS.ToString().Replace(",", ".") +
                              ",Nro_ECF=" + Nro_ECF.ToString().Replace(",", ".") +
                              ",Gera_Mapa=" + (Gera_Mapa ? "1" : "0") +
                              ",Indice_ST='" + Indice_ST + "'" +
                              ",Entrada_ICMS=" + Entrada_ICMS.ToString().Replace(",", ".") +
                              ",Redutor=" + Redutor.ToString().Replace(",", ".") +
                              ",Incide_ICMS=" + (Incide_ICMS ? "1" : "0") +
                              ",Incide_ICM_Subistituicao=" + (Incide_ICM_Subistituicao ? "1" : "0") +
                              ",ICMS_Efetivo=" + ICMS_Efetivo.ToString().Replace(",", ".") +
                              ",csosn='"+csosn+"'"+
                              ",cfop="+Funcoes.decimalPonto(cfop.ToString())+
                              ",cst_sped='"+cst_sped+"'"+
                              ",ipi_EmOutrasDespesas="+(ipi_EmOutrasDespesas?"1":"0")+
                              ",icmsst_emoutrasdespesas="+(icmsst_emOutrasDespesas?"1":"0")+
                              ",CFOP_Entrada="+Funcoes.decimalPonto(cfop_entrada.ToString())+
                            

                    "  where Codigo_Tributacao=" + Codigo_Tributacao.ToString().Replace(",", ".");
                Conexao.executarSql(sql);
            }
            catch (Exception err)
            {
                throw new Exception("Não foi possivel Atualizar os valores erro:" + err.Message);
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
            String sql = "delete from tributacao  where Codigo_Tributacao=" + Codigo_Tributacao.ToString().Replace(",", ".");
            Conexao.executarSql(sql);
            return true;
        }

        private void insert()
        {

            Codigo_Tributacao = decimal.Parse(Funcoes.sequencia("TRIBUTACAO.CODIGO_TRIBUTACAO", null));

            try
            {
                String sql = " insert into tributacao " +
                          "(Codigo_Tributacao," +
                          "Descricao_Tributacao," +
                          "Filial," +
                          "Saida_ICMS," +
                          "Nro_ECF," +
                          "Gera_Mapa," +
                          "Indice_ST," +
                          "Entrada_ICMS," +
                          "Redutor," +
                          "Incide_ICMS," +
                          "Incide_ICM_Subistituicao," +
                          "ICMS_Efetivo," +
                          "csosn"+
                          ",cfop"+
                          ",cst_sped"+
                          ",ipi_emOutrasDespesas"+
                          ",icmsst_emoutrasdespesas"+
                          ",cfop_entrada"+
                          
                     " )values (" +
                          Codigo_Tributacao.ToString().Replace(",", ".") +
                          "," + "'" + Descricao_Tributacao + "'" +
                          "," + "'" + Filial + "'" +
                          "," + Saida_ICMS.ToString().Replace(",", ".") +
                          "," + Nro_ECF.ToString().Replace(",", ".") +
                          "," + (Gera_Mapa ? 1 : 0) +
                          "," + "'" + Indice_ST + "'" +
                          "," + Entrada_ICMS.ToString().Replace(",", ".") +
                          "," + Redutor.ToString().Replace(",", ".") +
                          "," + (Incide_ICMS ? 1 : 0) +
                          "," + (Incide_ICM_Subistituicao ? 1 : 0) +
                          "," + ICMS_Efetivo.ToString().Replace(",", ".") +
                          ",'"+csosn+"'"+
                          ","+Funcoes.decimalPonto(cfop.ToString())+
                          ",'"+cst_sped+"'"+
                          ","+ (ipi_EmOutrasDespesas?"1":"0")+
                          ","+(icmsst_emOutrasDespesas?"1":"0")+
                          ","+Funcoes.decimalPonto(cfop_entrada.ToString())+
                          ");";

                Conexao.executarSql(sql);
                Funcoes.salvaProximaSequencia("TRIBUTACAO.CODIGO_TRIBUTACAO", null);
            }
            catch (Exception err)
            {
                throw new Exception("Não foi possivel Inserir os valores erro:" + err.Message);
            }
        }

        public bool tributacaoUtilizada()
        {
            int merc = int.Parse(Conexao.retornaUmValor("select COUNT(codigo_tributacao) from mercadoria where Codigo_Tributacao="+Codigo_Tributacao+" or Codigo_Tributacao_ent="+Codigo_Tributacao, null));
            int nf = int.Parse(Conexao.retornaUmValor("select COUNT( Codigo_Tributacao) from nf_item  where Codigo_Tributacao =" + Codigo_Tributacao , null));
            return (merc > 0 || nf > 0);
        }
    }
}/* 
/*================================Metodos tela de Pesquisa==========================================
using System.Data; 
using visualSysWeb.dao;
           :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  static DataTable tb;
                  static String sqlGrid = ""select * from tributacao";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ tributacaoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>tributacao</h1></center>
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
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="12"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Codigo_Tributacao" Text="Codigo_Tributacao" Visible="true" 
                    HeaderText="Codigo_Tributacao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Descricao_Tributacao" Text="Descricao_Tributacao" Visible="true" 
                    HeaderText="Descricao_Tributacao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Saida_ICMS" Text="Saida_ICMS" Visible="true" 
                    HeaderText="Saida_ICMS" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Nro_ECF" Text="Nro_ECF" Visible="true" 
                    HeaderText="Nro_ECF" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Gera_Mapa" Text="Gera_Mapa" Visible="true" 
                    HeaderText="Gera_Mapa" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Indice_ST" Text="Indice_ST" Visible="true" 
                    HeaderText="Indice_ST" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Entrada_ICMS" Text="Entrada_ICMS" Visible="true" 
                    HeaderText="Entrada_ICMS" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Redutor" Text="Redutor" Visible="true" 
                    HeaderText="Redutor" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Incide_ICMS" Text="Incide_ICMS" Visible="true" 
                    HeaderText="Incide_ICMS" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Incide_ICM_Subistituicao" Text="Incide_ICM_Subistituicao" Visible="true" 
                    HeaderText="Incide_ICM_Subistituicao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="ICMS_Efetivo" Text="ICMS_Efetivo" Visible="true" 
                    HeaderText="ICMS_Efetivo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  


                 </Columns> 
                 <EditRowStyle BackColor="#999999" /> 
                 <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" /> 
                 <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" /> 
                 <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" /> 
                 <RowStyle BackColor="#F7F6F3" ForeColor="#333333" /> 
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
                 protected static tributacaoDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new tributacaoDAO();
                      tabMenu.Items[MultiView1.ActiveViewIndex].Selected = true;
                      if (Request.Params["novo"] != null) 
                      {
                        status = "incluir";
                                         EnabledControls(conteudo, true);
                                         EnabledControls(cabecalho, true);
                      }
                      else //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                      {
                           if (Request.Params["campoIndex"] != null)  // colocar o campo index da tabela
                           {
                              try
                              {
                                   if (!IsPostBack)
                                   {
                                        String index = Request.Params["campoIndex"].ToString();// colocar o campo index da tabela
                                        status = "visualizar";
                                        obj = new tributacaoDAO(index,usr);
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
                                         txtCodigo_Tributacao.Text=string.Format("{0:0,0.00}",obj.Codigo_Tributacao);
                                         txtDescricao_Tributacao.Text=obj.Descricao_Tributacao.ToString();
                                         txtFilial.Text=obj.Filial.ToString();
                                         txtSaida_ICMS.Text=string.Format("{0:0,0.00}",obj.Saida_ICMS);
                                         txtNro_ECF.Text=string.Format("{0:0,0.00}",obj.Nro_ECF);
                                         chkGera_Mapa.Checked =obj.Gera_Mapa;
                                         txtIndice_ST.Text=obj.Indice_ST.ToString();
                                         txtEntrada_ICMS.Text=string.Format("{0:0,0.00}",obj.Entrada_ICMS);
                                         txtRedutor.Text=string.Format("{0:0,0.00}",obj.Redutor);
                                         chkIncide_ICMS.Checked =obj.Incide_ICMS;
                                         chkIncide_ICM_Subistituicao.Checked =obj.Incide_ICM_Subistituicao;
                                         txtICMS_Efetivo.Text=string.Format("{0:0,0.00}",obj.ICMS_Efetivo);
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Codigo_Tributacao=Decimal.Parse(txtCodigo_Tributacao.Text);
                                         obj.Descricao_Tributacao=txtDescricao_Tributacao.Text;
                                         obj.Filial=txtFilial.Text;
                                         obj.Saida_ICMS=Decimal.Parse(txtSaida_ICMS.Text);
                                         obj.Nro_ECF=Decimal.Parse(txtNro_ECF.Text);
                                         obj.Gera_Mapa=chkGera_Mapa.Checked ;
                                         obj.Indice_ST=txtIndice_ST.Text;
                                         obj.Entrada_ICMS=Decimal.Parse(txtEntrada_ICMS.Text);
                                         obj.Redutor=Decimal.Parse(txtRedutor.Text);
                                         obj.Incide_ICMS=chkIncide_ICMS.Checked ;
                                         obj.Incide_ICM_Subistituicao=chkIncide_ICM_Subistituicao.Checked ;
                                         obj.ICMS_Efetivo=Decimal.Parse(txtICMS_Efetivo.Text);
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
       <center> <h1>Detalhes do tributacao</h1></center>                  
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
                                      <td >                   <p>Codigo_Tributacao</p>
                   <asp:TextBox ID="txtCodigo_Tributacao" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Descricao_Tributacao</p>
                   <asp:TextBox ID="txtDescricao_Tributacao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Filial</p>
                   <asp:TextBox ID="txtFilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Saida_ICMS</p>
                   <asp:TextBox ID="txtSaida_ICMS" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Nro_ECF</p>
                   <asp:TextBox ID="txtNro_ECF" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Gera_Mapa</p>
                   <td><asp:CheckBox ID="chkGera_Mapa" runat="server" Text="Gera_Mapa"/>
                   </td>

                                      <td >                   <p>Indice_ST</p>
                   <asp:TextBox ID="txtIndice_ST" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Entrada_ICMS</p>
                   <asp:TextBox ID="txtEntrada_ICMS" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Redutor</p>
                   <asp:TextBox ID="txtRedutor" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Incide_ICMS</p>
                   <td><asp:CheckBox ID="chkIncide_ICMS" runat="server" Text="Incide_ICMS"/>
                   </td>

                                      <td >                   <p>Incide_ICM_Subistituicao</p>
                   <td><asp:CheckBox ID="chkIncide_ICM_Subistituicao" runat="server" Text="Incide_ICM_Subistituicao"/>
                   </td>

                                      <td >                   <p>ICMS_Efetivo</p>
                   <asp:TextBox ID="txtICMS_Efetivo" runat="server"  CssClass="numero" ></asp:TextBox>
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

