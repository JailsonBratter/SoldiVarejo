using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class cadernetaDAO
    {
        public String Codigo_Cliente { get; set; }
        public DateTime Emissao_Caderneta { get; set; }
        public String Emissao_CadernetaBr()
        {
            return dataBr(Emissao_Caderneta);
        }

        public String Tipo { get; set; }
        public String Documento_Caderneta { get; set; }
        public String Historico_Caderneta { get; set; }
        public Decimal Total_Caderneta { get; set; }
        public int Caixa_Caderneta { get; set; }
        public String lancamento { get; set; }
        public String filial { get; set; }
        public String usuario { get; set; }
        public DateTime data_inclusao = new DateTime();
        public DateTime data_canc = new DateTime();
        public Decimal vlr_cancelado = 0;
        
        public cadernetaDAO() { }
        public cadernetaDAO(String documento_caderneta, String codigo_cliente, User usr)
        { //colocar campo index da tabela
            String sql = "Select * from  where caderneta Documento_Caderneta='" + Documento_Caderneta + "' and Codigo_Cliente='" + Codigo_Cliente + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }

        public ArrayList arrToString()
        {
            ArrayList linha = new ArrayList();
            linha.Add(Codigo_Cliente);
            linha.Add(Emissao_CadernetaBr());
            linha.Add(Tipo);
            linha.Add(Documento_Caderneta);
            linha.Add(Historico_Caderneta);
            linha.Add(Total_Caderneta.ToString("N2"));
            linha.Add(Caixa_Caderneta.ToString());
            linha.Add(lancamento);
            linha.Add(filial);
            linha.Add(usuario);
            return linha;

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
            if (rs.Read())
            {
                Codigo_Cliente = rs["Codigo_Cliente"].ToString();
                Emissao_Caderneta = (rs["Emissao_Caderneta"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Emissao_Caderneta"].ToString()));
                Tipo = rs["Tipo"].ToString();
                Documento_Caderneta = rs["Documento_Caderneta"].ToString();
                Historico_Caderneta = rs["Historico_Caderneta"].ToString();
                Total_Caderneta = (Decimal)(rs["Total_Caderneta"].ToString().Equals("") ? new Decimal() : rs["Total_Caderneta"]);
                Caixa_Caderneta = (rs["Caixa_Caderneta"] == null ? 0 : int.Parse(rs["Caixa_Caderneta"].ToString()));
                lancamento = rs["lancamento"].ToString();
                filial = rs["filial"].ToString();
                usuario = rs["usuario"].ToString();
                data_inclusao = (rs["data_inclusao"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["data_inclusao"].ToString()));
                DateTime.TryParse(rs["data_canc"].ToString(), out data_canc);
                Decimal.TryParse(rs["vlr_cancelado"].ToString(), out vlr_cancelado);

            }
            if (rs != null)
                rs.Close();
        }
        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = "update  caderneta set " +
                              ",Emissao_Caderneta=" + (Emissao_Caderneta.Equals("0001-01-01") ? "null" : "'" + Emissao_Caderneta.ToString("yyyy-MM-dd") + "'") + "," +
                              ",Tipo='" + Tipo + "'" +
                              ",Historico_Caderneta='" + Historico_Caderneta + "'" +
                              ",Total_Caderneta=" + Total_Caderneta.ToString().Replace(",", ".") +
                              ",Caixa_Caderneta=" + Caixa_Caderneta +
                              ",lancamento='" + lancamento + "'" +
                              ",usuario='" + usuario + "'" +
                              ",data_inclusao=" + (data_inclusao.Equals("0001'-01-01") ? "null" : "'" + data_inclusao.ToString("yyyy-MM-dd") + "'")  +
                              ",data_canc=" + (data_canc.Equals("0001'-01-01") ? "null" : "'" + data_canc.ToString("yyyy-MM-dd") + "'")  +
                              ",vlr_cancelado ="+vlr_cancelado.ToString().Replace(",",".")+
                    "  where  Documento_Caderneta='" + Documento_Caderneta + "' and filial='" + filial + "' and Codigo_Cliente='" + Codigo_Cliente + "'";

                Conexao.executarSql(sql, conn, tran);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }
        public bool salvar(bool novo)
        {
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {

                if (novo)
                {
                    insert(conn, tran);
                }
                else
                {
                    update(conn, tran);
                }
                tran.Commit();
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
            finally
            {

                if (conn != null)
                    conn.Close();
            }
            return true;
        }

        public bool excluir(User usr)
        {

            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                Documento_Caderneta = Documento_Caderneta.Replace("&nbsp;", "");



                String sql = "delete from caderneta   where Documento_Caderneta='" + Documento_Caderneta + "' and " +
                                " filial='" + filial + "' and " +
                                " Codigo_Cliente='" + Codigo_Cliente + "' and " +
                                " convert(varchar,Emissao_Caderneta,102)='" + Emissao_Caderneta.ToString("yyyy.MM.dd") + "' and " +
                                " tipo ='" + Tipo + "' and" +
                                " Historico_caderneta ='" + Historico_Caderneta.Trim().Replace("&nbsp;", "") + "' and " +
                                " total_caderneta =" + Total_Caderneta.ToString().Replace(".", "").Replace(",", ".") + " and " +
                                " caixa_caderneta =" + Caixa_Caderneta;



                Conexao.executarSql(sql,conn,tran);
                bool cancLanc = Funcoes.valorParametro("CARDENETA_CANC_MOV", usr).ToUpper().Equals("TRUE");
                if (cancLanc)
                {
                    String sqlCanc = $"update Saida_estoque set data_cancelamento = GETDATE() " +
                                       " where Documento = '" + Documento_Caderneta + "' " +
                                       "  and Caixa_Saida = " + Caixa_Caderneta +
                                       "  and Data_movimento = '" + Emissao_Caderneta.ToString("yyyy-MM-dd") + "' " +
                                       "  and Codigo_Cliente = '" + Codigo_Cliente + "'; " +

                                       "   update Lista_finalizadora set Cancelado = 1 " +
                                       "  where Documento = '" + Documento_Caderneta + "' " +
                                       "  and pdv = " + Caixa_Caderneta +
                                       "  and Emissao = '" + Emissao_Caderneta.ToString("yyyy-MM-dd") + "' " +
                                       "  and Codigo_Cliente = '" + Codigo_Cliente + "';";


                    Conexao.executarSql(sqlCanc, conn, tran);

                }

                Documento_Caderneta = "C" + Documento_Caderneta.Trim();
                vlr_cancelado = Total_Caderneta;
                data_canc = DateTime.Now;
                usuario = usr.getUsuario();
                Total_Caderneta = 0;
                insert(conn, tran);
                

                tran.Commit();
            }
            catch (Exception err)
            {
                tran.Rollback();
                throw err;
            }
            finally
            {

                if (conn != null)
                    conn.Close();
            }
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = " insert into caderneta (" +
                          "Codigo_Cliente," +
                          "Emissao_Caderneta," +
                          "Tipo," +
                          "Documento_Caderneta," +
                          "Historico_Caderneta," +
                          "Total_Caderneta," +
                          "Caixa_Caderneta," +
                          "lancamento," +
                          "filial," +
                          "usuario," +
                          "data_inclusao" +
                          ",data_canc"+
                          ",vlr_cancelado"+

                     ") values (" +
                          "'" + Codigo_Cliente + "'" +
                          "," + (Emissao_Caderneta.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Emissao_Caderneta.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + Tipo + "'" +
                          "," + "'" + Documento_Caderneta + "'" +
                          "," + "'" + Historico_Caderneta + "'" +
                          "," + Total_Caderneta.ToString().Replace(",", ".") +
                          "," + Caixa_Caderneta +
                          "," + "'" + lancamento + "'" +
                          "," + "'" + filial + "'" +
                          "," + "'" + usuario + "'" +
                          "," + (data_inclusao.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_inclusao.ToString("yyyy-MM-dd") + "'") +
                          "," + (data_canc.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + data_canc.ToString("yyyy-MM-dd") + "'") +
                          ","+vlr_cancelado.ToString().Replace(",",".")+

                         ");";


                Conexao.executarSql(sql, conn, tran);
                /*
                 Decimal ValorAtualizar = 0;
                 if (Tipo.Equals("CREDITO"))
                 {
                     ValorAtualizar = (Total_Caderneta) * -1;
                 }
                 else
                 {
                     ValorAtualizar = Total_Caderneta;
                 }

                 Conexao.executarSql("update cliente set utilizado =utilizado+("+ValorAtualizar+") where codigo_cliente='"+Codigo_Cliente+"'" );
                 */
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
                  static String sqlGrid = ""select * from caderneta";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ cadernetaDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>caderneta</h1></center>
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
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="10"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Codigo_Cliente" Text="Codigo_Cliente" Visible="true" 
                    HeaderText="Codigo_Cliente" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Emissao_Caderneta" Text="Emissao_Caderneta" Visible="true" 
                    HeaderText="Emissao_Caderneta" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Tipo" Text="Tipo" Visible="true" 
                    HeaderText="Tipo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Documento_Caderneta" Text="Documento_Caderneta" Visible="true" 
                    HeaderText="Documento_Caderneta" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Historico_Caderneta" Text="Historico_Caderneta" Visible="true" 
                    HeaderText="Historico_Caderneta" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Total_Caderneta" Text="Total_Caderneta" Visible="true" 
                    HeaderText="Total_Caderneta" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Caixa_Caderneta" Text="Caixa_Caderneta" Visible="true" 
                    HeaderText="Caixa_Caderneta" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="lancamento" Text="lancamento" Visible="true" 
                    HeaderText="lancamento" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="filial" Text="filial" Visible="true" 
                    HeaderText="filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="usuario" Text="usuario" Visible="true" 
                    HeaderText="usuario" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
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
                 protected static cadernetaDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new cadernetaDAO();
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
                                        obj = new cadernetaDAO(index,usr);
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
                                         txtCodigo_Cliente.Text=obj.Codigo_Cliente.ToString();
                                         txtEmissao_Caderneta.Text=obj.Emissao_CadernetaBr();
                                         txtTipo.Text=obj.Tipo.ToString();
                                         txtDocumento_Caderneta.Text=obj.Documento_Caderneta.ToString();
                                         txtHistorico_Caderneta.Text=obj.Historico_Caderneta.ToString();
                                         txtTotal_Caderneta.Text=string.Format("{0:0,0.00}",obj.Total_Caderneta);
                                         txtCaixa_Caderneta.Text=obj.Caixa_Caderneta.ToString();
                                         txtlancamento.Text=obj.lancamento.ToString();
                                         txtfilial.Text=obj.filial.ToString();
                                         txtusuario.Text=obj.usuario.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Codigo_Cliente=txtCodigo_Cliente.Text;
                                         obj.Emissao_Caderneta=DateTime.Parse(txtEmissao_Caderneta.Text);
                                         obj.Tipo=txtTipo.Text;
                                         obj.Documento_Caderneta=txtDocumento_Caderneta.Text;
                                         obj.Historico_Caderneta=txtHistorico_Caderneta.Text;
                                         obj.Total_Caderneta=Decimal.Parse(txtTotal_Caderneta.Text);
                                         obj.Caixa_Caderneta=int.Parse(txtCaixa_Caderneta.Text);
                                         obj.lancamento=txtlancamento.Text;
                                         obj.filial=txtfilial.Text;
                                         obj.usuario=txtusuario.Text;
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
                       if (lista != null)
                          lista.Close();
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
       <center> <h1>Detalhes do caderneta</h1></center>                  
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
                                      <td >                   <p>Codigo_Cliente</p>
                   <asp:TextBox ID="txtCodigo_Cliente" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Emissao_Caderneta</p>
                   <asp:TextBox ID="txtEmissao_Caderneta" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Tipo</p>
                   <asp:TextBox ID="txtTipo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Documento_Caderneta</p>
                   <asp:TextBox ID="txtDocumento_Caderneta" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Historico_Caderneta</p>
                   <asp:TextBox ID="txtHistorico_Caderneta" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Total_Caderneta</p>
                   <asp:TextBox ID="txtTotal_Caderneta" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Caixa_Caderneta</p>
                   <asp:TextBox ID="txtCaixa_Caderneta" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>lancamento</p>
                   <asp:TextBox ID="txtlancamento" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>filial</p>
                   <asp:TextBox ID="txtfilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>usuario</p>
                   <asp:TextBox ID="txtusuario" runat="server" ></asp:TextBox>
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

