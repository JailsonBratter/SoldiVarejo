using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace visualSysWeb.dao
{
    public class chequeDAO
    {
        public String Codigo_Cliente { get; set; }
        public String Nome_Cliente { get; set; }
        public String Lancamento_Cheque = "";
        public DateTime Emissao_Cheque { get; set; }
        public String Emissao_ChequeBr()
        {
            return dataBr(Emissao_Cheque);
        }

        public DateTime Deposito_Cheque { get; set; }
        public String Deposito_ChequeBr()
        {
            return dataBr(Deposito_Cheque);
        }

        public String Banco_Cheque { get; set; }
        public String Agencia_Cheque { get; set; }
        public String Numero_Cheque { get; set; }
        public String Documento_Cheque { get; set; }
        public Decimal Total_Cheque { get; set; }
        public String Devolvido_Cheque = "N";
        public String Compensado_Cheque { get; set; }
        public Decimal utilizado_cheque = 0;
        public int index = 0;
        public DateTime Data_cadastro = new DateTime();
        public String Responsavel_Cheque { get; set; }
        public String Responsavel_Telefone { get; set; }
        public String Observacao { get; set; }

        public chequeDAO() { }
        public chequeDAO(String codigoCliente, String numeroCheque, User usr)
        { //colocar campo index da tabela
            String sql = "Select * from  cheque where Codigo_Cliente ='" + codigoCliente + "' and numero_cheque='" + numeroCheque + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
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

        public ArrayList ArrToString()
        {
            ArrayList item = new ArrayList();
            item.Add(Codigo_Cliente.ToString());
            item.Add(Nome_Cliente.ToString());
            item.Add(Lancamento_Cheque.ToString());
            item.Add(Emissao_Cheque.ToString("dd/MM/yyyy"));
            item.Add(Deposito_Cheque.ToString("dd/MM/yyyy"));
            item.Add(Banco_Cheque.ToString());
            item.Add(Agencia_Cheque.ToString());
            item.Add(Numero_Cheque.ToString());
            item.Add(Documento_Cheque.ToString());
            item.Add(Total_Cheque.ToString("N2"));
            item.Add(Devolvido_Cheque.ToString().ToUpper().Equals("S") ? "SIM" : "NAO");
            item.Add(Compensado_Cheque.ToString().ToUpper().Equals("S") ? "SIM" : "NAO");
            item.Add(utilizado_cheque.ToString().ToUpper().Equals("S") ? "SIM" : "NAO");
            item.Add(Data_cadastro.ToString("dd/MM/yyyy"));
            item.Add(Responsavel_Cheque.ToString());
            item.Add(Responsavel_Telefone.ToString());
            item.Add(Observacao.ToString());
            return item;
        }
        public void carregarDados(SqlDataReader rs)
        {
            if (rs.Read())
            {
                Codigo_Cliente = rs["Codigo_Cliente"].ToString();
                Nome_Cliente = rs["Nome_Cliente"].ToString();
                Lancamento_Cheque = rs["Lancamento_Cheque"].ToString();
                Emissao_Cheque = (rs["Emissao_Cheque"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Emissao_Cheque"].ToString()));
                Deposito_Cheque = (rs["Deposito_Cheque"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Deposito_Cheque"].ToString()));
                Banco_Cheque = rs["Banco_Cheque"].ToString();
                Agencia_Cheque = rs["Agencia_Cheque"].ToString();
                Numero_Cheque = rs["Numero_Cheque"].ToString();
                Documento_Cheque = rs["Documento_Cheque"].ToString();
                Total_Cheque = (Decimal)(rs["Total_Cheque"].ToString().Equals("") ? new Decimal() : rs["Total_Cheque"]);
                Devolvido_Cheque = rs["Devolvido_Cheque"].ToString();
                Compensado_Cheque = rs["Compensado_Cheque"].ToString();
                utilizado_cheque = (Decimal)(rs["utilizado_cheque"].ToString().Equals("") ? new Decimal() : rs["utilizado_cheque"]);
                Data_cadastro = (rs["Data_Cadastro"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_Cadastro"].ToString()));
                Responsavel_Cheque = rs["Responsavel_Cheque"].ToString();
                Responsavel_Telefone = rs["Responsavel_Telefone"].ToString();
                if (rs["Observacao"].ToString().Length > 250)
                {
                    Observacao = rs["Observacao"].ToString().Substring(0, 249);
                }
                else
                {
                    Observacao = rs["Observacao"].ToString();
                }
            }
            if (rs != null)
                rs.Close();
        }
        private void update()
        {
            try
            {
                String sql = "update  cheque set " +
                              "Nome_Cliente='" + Nome_Cliente + "'" +
                              ",Lancamento_Cheque='" + Lancamento_Cheque + "'" +
                              ",Emissao_Cheque=" + (Emissao_Cheque.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Emissao_Cheque.ToString("yyyy-MM-dd") + "'") +
                              ",Deposito_Cheque=" + (Deposito_Cheque.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Deposito_Cheque.ToString("yyyy-MM-dd") + "'") +
                              ",Banco_Cheque='" + Banco_Cheque + "'" +
                              ",Agencia_Cheque='" + Agencia_Cheque + "'" +
                              ",numero_cheque='" + Numero_Cheque + "'" +
                              ",Documento_Cheque='" + Documento_Cheque + "'" +
                              ",Total_Cheque=" + Total_Cheque.ToString().Replace(",", ".") +
                              ",Devolvido_Cheque='" + Devolvido_Cheque + "'" +
                              ",Compensado_Cheque='" + Compensado_Cheque + "'" +
                              ",utilizado_cheque=" + utilizado_cheque.ToString().Replace(",", ".") +
                              ",Data_Cadastro=" + (Data_cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_cadastro.ToString("yyyy-MM-dd") + "'") +
                              ",Responsavel_Cheque='" + Responsavel_Cheque + "'" +
                              ",Responsavel_Telefone='" + Responsavel_Telefone + "'" +
                              ",Observacao='" + Observacao + "'" +

                    "  where Codigo_Cliente ='" + Codigo_Cliente + "' and numero_cheque='" + Numero_Cheque + "'";
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
            String sql = "delete from cheque where Codigo_Cliente ='" + Codigo_Cliente + "' and numero_cheque='" + Numero_Cheque + "'";
            Conexao.executarSql(sql);
            return true;
        }

        private void insert()
        {
            try
            {
                String sql = " insert into cheque(" +
                          "Codigo_Cliente," +
                          "Nome_Cliente," +
                          "Lancamento_Cheque," +
                          "Emissao_Cheque," +
                          "Deposito_Cheque," +
                          "Banco_Cheque," +
                          "Agencia_Cheque," +
                          "Numero_Cheque," +
                          "Documento_Cheque," +
                          "Total_Cheque," +
                          "Devolvido_Cheque," +
                          "Compensado_Cheque," +
                          "utilizado_cheque," +
                          "data_cadastro," +
                          "Responsavel_Cheque," +
                          "Responsavel_Telefone," +
                          "Observacao" +
                     " )values (" +
                          "'" + Codigo_Cliente + "'" +
                          "," + "'" + Nome_Cliente + "'" +
                          "," + "'" + Lancamento_Cheque + "'" +
                          "," + (Emissao_Cheque.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Emissao_Cheque.ToString("yyyy-MM-dd") + "'") +
                          "," + (Deposito_Cheque.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Deposito_Cheque.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + Banco_Cheque + "'" +
                          "," + "'" + Agencia_Cheque + "'" +
                          "," + "'" + Numero_Cheque + "'" +
                          "," + "'" + Documento_Cheque + "'" +
                          "," + Total_Cheque.ToString().Replace(",", ".") +
                          "," + "'" + Devolvido_Cheque + "'" +
                          "," + "'" + Compensado_Cheque + "'" +
                          "," + utilizado_cheque.ToString().Replace(",", ".") +
                          "," + (Data_cadastro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_cadastro.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + Responsavel_Cheque + "'" +
                          "," + "'" + Responsavel_Telefone + "'" +
                          "," + "'" + Observacao + "'" +
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
                  static String sqlGrid = ""select * from cheque";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ chequeDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>cheque</h1></center>
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
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="13"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Codigo_Cliente" Text="Codigo_Cliente" Visible="true" 
                    HeaderText="Codigo_Cliente" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Nome_Cliente" Text="Nome_Cliente" Visible="true" 
                    HeaderText="Nome_Cliente" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Lancamento_Cheque" Text="Lancamento_Cheque" Visible="true" 
                    HeaderText="Lancamento_Cheque" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Emissao_Cheque" Text="Emissao_Cheque" Visible="true" 
                    HeaderText="Emissao_Cheque" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Deposito_Cheque" Text="Deposito_Cheque" Visible="true" 
                    HeaderText="Deposito_Cheque" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Banco_Cheque" Text="Banco_Cheque" Visible="true" 
                    HeaderText="Banco_Cheque" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Agencia_Cheque" Text="Agencia_Cheque" Visible="true" 
                    HeaderText="Agencia_Cheque" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Numero_Cheque" Text="Numero_Cheque" Visible="true" 
                    HeaderText="Numero_Cheque" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Documento_Cheque" Text="Documento_Cheque" Visible="true" 
                    HeaderText="Documento_Cheque" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Total_Cheque" Text="Total_Cheque" Visible="true" 
                    HeaderText="Total_Cheque" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Devolvido_Cheque" Text="Devolvido_Cheque" Visible="true" 
                    HeaderText="Devolvido_Cheque" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Compensado_Cheque" Text="Compensado_Cheque" Visible="true" 
                    HeaderText="Compensado_Cheque" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="utilizado_cheque" Text="utilizado_cheque" Visible="true" 
                    HeaderText="utilizado_cheque" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
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
                 protected static chequeDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new chequeDAO();
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
                                        obj = new chequeDAO(index,usr);
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
                                         txtNome_Cliente.Text=obj.Nome_Cliente.ToString();
                                         txtLancamento_Cheque.Text=obj.Lancamento_Cheque.ToString();
                                         txtEmissao_Cheque.Text=obj.Emissao_ChequeBr();
                                         txtDeposito_Cheque.Text=obj.Deposito_ChequeBr();
                                         txtBanco_Cheque.Text=obj.Banco_Cheque.ToString();
                                         txtAgencia_Cheque.Text=obj.Agencia_Cheque.ToString();
                                         txtNumero_Cheque.Text=obj.Numero_Cheque.ToString();
                                         txtDocumento_Cheque.Text=obj.Documento_Cheque.ToString();
                                         txtTotal_Cheque.Text=string.Format("{0:0,0.00}",obj.Total_Cheque);
                                         txtDevolvido_Cheque.Text=obj.Devolvido_Cheque.ToString();
                                         txtCompensado_Cheque.Text=obj.Compensado_Cheque.ToString();
                                         txtutilizado_cheque.Text=string.Format("{0:0,0.00}",obj.utilizado_cheque);
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Codigo_Cliente=txtCodigo_Cliente.Text;
                                         obj.Nome_Cliente=txtNome_Cliente.Text;
                                         obj.Lancamento_Cheque=txtLancamento_Cheque.Text;
                                         obj.Emissao_Cheque=DateTime.Parse(txtEmissao_Cheque.Text);
                                         obj.Deposito_Cheque=DateTime.Parse(txtDeposito_Cheque.Text);
                                         obj.Banco_Cheque=txtBanco_Cheque.Text;
                                         obj.Agencia_Cheque=txtAgencia_Cheque.Text;
                                         obj.Numero_Cheque=txtNumero_Cheque.Text;
                                         obj.Documento_Cheque=txtDocumento_Cheque.Text;
                                         obj.Total_Cheque=Decimal.Parse(txtTotal_Cheque.Text);
                                         obj.Devolvido_Cheque=txtDevolvido_Cheque.Text;
                                         obj.Compensado_Cheque=txtCompensado_Cheque.Text;
                                         obj.utilizado_cheque=Decimal.Parse(txtutilizado_cheque.Text);
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
       <center> <h1>Detalhes do cheque</h1></center>                  
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

                                      <td >                   <p>Nome_Cliente</p>
                   <asp:TextBox ID="txtNome_Cliente" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Lancamento_Cheque</p>
                   <asp:TextBox ID="txtLancamento_Cheque" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Emissao_Cheque</p>
                   <asp:TextBox ID="txtEmissao_Cheque" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Deposito_Cheque</p>
                   <asp:TextBox ID="txtDeposito_Cheque" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Banco_Cheque</p>
                   <asp:TextBox ID="txtBanco_Cheque" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Agencia_Cheque</p>
                   <asp:TextBox ID="txtAgencia_Cheque" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Numero_Cheque</p>
                   <asp:TextBox ID="txtNumero_Cheque" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Documento_Cheque</p>
                   <asp:TextBox ID="txtDocumento_Cheque" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Total_Cheque</p>
                   <asp:TextBox ID="txtTotal_Cheque" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Devolvido_Cheque</p>
                   <asp:TextBox ID="txtDevolvido_Cheque" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Compensado_Cheque</p>
                   <asp:TextBox ID="txtCompensado_Cheque" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>utilizado_cheque</p>
                   <asp:TextBox ID="txtutilizado_cheque" runat="server"  CssClass="numero" ></asp:TextBox>
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

