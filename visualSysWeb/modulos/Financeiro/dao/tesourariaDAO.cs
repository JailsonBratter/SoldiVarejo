using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace visualSysWeb.dao
{
    public class tesourariaDAO
    {
        public int id_fechamento = 0;
        public String id_finalizadora = "";
        public Decimal Total_Sistema = 0;
        public Decimal Total_Entregue = 0;
        public int op_Financeira = 0;
        public DateTime DATA_ABERTURA = new DateTime();
        public String DATA_ABERTURABr()
        {
            return dataBr(DATA_ABERTURA);
        }

        public int PDV =0;
        public int ID_OPERADOR =0;
        public String FILIAL="";
        public int FINALIZADORA =0;

        public tesourariaDAO(User usr) {
            this.FILIAL = usr.getFilial();
        }
        public tesourariaDAO(int pdv,int id_operador,int finalizadora, DateTime dtAbertura  , User usr)
        {
            this.DATA_ABERTURA = dtAbertura;
            this.PDV = pdv;
            this.ID_OPERADOR = id_operador;
            this.FILIAL = usr.getFilial();
            this.FINALIZADORA = finalizadora;
            String sql = "Select * from  tesouraria   where id_fec=" + (DATA_ABERTURA.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + DATA_ABERTURA.ToString("yyyy-MM-dd") + "'") + "," +
                              " and PDV=" + PDV +
                              " and ID_OPERADOR=" + ID_OPERADOR +
                              " and FILIAL='" + FILIAL + "'" +
                              " and FINALIZADORA=" + FINALIZADORA;
            SqlDataReader rs = null;
            try
            {


                rs = Conexao.consulta(sql, usr, true);
                carregarDados(rs);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (rs != null)
                {
                    rs.Close();
                }
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
                    id_finalizadora = rs["id_finalizadora"].ToString();
                    Total_Sistema = (Decimal)(rs["Total_Sistema"].ToString().Equals("") ? new Decimal() : rs["Total_Sistema"]);
                    Total_Entregue = (Decimal)(rs["Total_Entregue"].ToString().Equals("") ? new Decimal() : rs["Total_Entregue"]);
                    op_Financeira = (rs["op_Financeira"] == null ? 0 : int.Parse(rs["op_Financeira"].ToString()));
                    DATA_ABERTURA = (rs["DATA_ABERTURA"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["DATA_ABERTURA"].ToString()));
                    PDV = (rs["PDV"] == null ? 0 : int.Parse(rs["PDV"].ToString()));
                    ID_OPERADOR = (rs["ID_OPERADOR"] == null ? 0 : int.Parse(rs["ID_OPERADOR"].ToString()));
                    FILIAL = rs["FILIAL"].ToString();
                    FINALIZADORA = (rs["FINALIZADORA"] == null ? 0 : int.Parse(rs["FINALIZADORA"].ToString()));
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
        private void update(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = "update  tesouraria set " +
                              "id_finalizadora='" + id_finalizadora + "'" +
                              ",Total_Sistema=" + Total_Sistema.ToString().Replace(",", ".") +
                              ",Total_Entregue=" + Total_Entregue.ToString().Replace(",", ".") +
                              ",op_Financeira=" + op_Financeira +
                    "  where DATA_ABERTURA=" + (DATA_ABERTURA.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + DATA_ABERTURA.ToString("yyyy-MM-dd") + "'") + "," +
                              " and PDV=" + PDV +
                              " and ID_OPERADOR=" + ID_OPERADOR +
                              " and FILIAL='" + FILIAL + "'" +
                              " and FINALIZADORA=" + FINALIZADORA
                        ;
                Conexao.executarSql(sql,conn,tran);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }

        public bool salvar(bool novo)
        {
            SqlConnection cnn = Conexao.novaConexao();
            SqlTransaction trans = cnn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                salvar(novo, cnn, trans);

                trans.Commit();

            }
            catch (Exception err)
            {
                trans.Rollback();
                throw err;
            }
            finally
            {
                if (cnn != null)
                    cnn.Close();
            }

            return true;
        }

        public bool salvar(bool novo, SqlConnection conn, SqlTransaction tran)
        {
            if (novo)
            {
                insert(conn, tran);
            }
            else
            {
                update(conn, tran);
            }
            return true;
        }

        public bool excluir()
        {
            String sql = "delete from tesouraria  where DATA_ABERTURA=" + (DATA_ABERTURA.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + DATA_ABERTURA.ToString("yyyy-MM-dd") + "'") + "," +
                              " and PDV=" + PDV +
                              " and ID_OPERADOR=" + ID_OPERADOR +
                              " and FILIAL='" + FILIAL + "'" +
                              " and FINALIZADORA=" + FINALIZADORA;


            Conexao.executarSql(sql);
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = " insert into tesouraria (" +
                          "id_finalizadora," +
                          "Total_Sistema," +
                          "Total_Entregue," +
                          "op_Financeira," +
                          "DATA_ABERTURA," +
                          "PDV," +
                          "ID_OPERADOR," +
                          "FILIAL," +
                          "FINALIZADORA" +
                          ",id_fechamento"+
                     ") values (" +
                          "'" + id_finalizadora + "'" +
                          "," + Total_Sistema.ToString().Replace(",", ".") +
                          "," + Total_Entregue.ToString().Replace(",", ".") +
                          "," + op_Financeira +
                          "," + (DATA_ABERTURA.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + DATA_ABERTURA.ToString("yyyy-MM-dd") + "'") +
                          "," + PDV +
                          "," + ID_OPERADOR +
                          "," + "'" + FILIAL + "'" +
                          "," + FINALIZADORA +
                          ","+id_fechamento+
                         ");";

                Conexao.executarSql(sql,conn,tran);
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
                  static String sqlGrid = ""select * from tesouraria";//colocar os campos no select que ser?o apresentados na tela
                  protected void Page_Load(object sender, EventArgs e)
                  {
                     if (!IsPostBack)
                     {   
                       User usr = (User)Session["User"];
                       tb = Conexao.GetTable(sqlGrid ,usr); 
                       gridPesquisa.DataSource = tb;
                       gridPesquisa.DataBind();
                      }
                      pesquisar(pnBtn);
                  }
                  
                  protected override void btnIncluir_Click(object sender, EventArgs e)
                  {
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ tesourariaDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                        }catch (Exception err)
                         {
                                      lblPesquisaErro.Text = err.Message;
                         }
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
                 
*/

/*================================html tela de Pesquisa==========================================
                  
   <center><h1>tesouraria</h1></center>
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
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id" Text="id" Visible="true" 
                    HeaderText="id" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id_finalizadora" Text="id_finalizadora" Visible="true" 
                    HeaderText="id_finalizadora" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Total_Sistema" Text="Total_Sistema" Visible="true" 
                    HeaderText="Total_Sistema" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Total_Entregue" Text="Total_Entregue" Visible="true" 
                    HeaderText="Total_Entregue" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="op_Financeira" Text="op_Financeira" Visible="true" 
                    HeaderText="op_Financeira" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="DATA_ABERTURA" Text="DATA_ABERTURA" Visible="true" 
                    HeaderText="DATA_ABERTURA" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="PDV" Text="PDV" Visible="true" 
                    HeaderText="PDV" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="ID_OPERADOR" Text="ID_OPERADOR" Visible="true" 
                    HeaderText="ID_OPERADOR" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="FILIAL" Text="FILIAL" Visible="true" 
                    HeaderText="FILIAL" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="FINALIZADORA" Text="FINALIZADORA" Visible="true" 
                    HeaderText="FINALIZADORA" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/tesourariaDetalhes.aspx?campoIndex={0}" 
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
        </div>          
                  
*/
/*================================Metodos tela detalhes==========================================
using System.Data; 
using visualSysWeb.dao;
using System.Data.SqlClient;
                 : visualSysWeb.code.PagePadrao
  {
                 protected static tesourariaDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new tesourariaDAO();
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
                                        obj = new tesourariaDAO(index,usr);
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
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                                         txtid.Text=obj.id.ToString();
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                                         txtid_finalizadora.Text=obj.id_finalizadora.ToString();
                                         txtTotal_Sistema.Text=string.Format("{0:0,0.00}",obj.Total_Sistema);
                                         txtTotal_Entregue.Text=string.Format("{0:0,0.00}",obj.Total_Entregue);
                                         txtop_Financeira.Text=obj.op_Financeira.ToString();
                                         txtDATA_ABERTURA.Text=obj.DATA_ABERTURABr();
                                         txtPDV.Text=obj.PDV.ToString();
                                         txtID_OPERADOR.Text=obj.ID_OPERADOR.ToString();
                                         txtFILIAL.Text=obj.FILIAL.ToString();
                                         txtFINALIZADORA.Text=obj.FINALIZADORA.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                                         obj.id=txtid.Text;
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                      --------------->TIPO N?O ENCONTRADO!!!!!!!!!!!!!!!!!!!!!
                                         obj.id_finalizadora=txtid_finalizadora.Text;
                                         obj.Total_Sistema=(txtTotal_Sistema.Text.Equals("")?0:Decimal.Parse(txtTotal_Sistema.Text));
                                         obj.Total_Entregue=(txtTotal_Entregue.Text.Equals("")?0:Decimal.Parse(txtTotal_Entregue.Text));
                                         obj.op_Financeira=int.Parse(txtop_Financeira.Text);
                                         obj.DATA_ABERTURA=(txtDATA_ABERTURA.Text.Equals("")?new DateTime():DateTime.Parse(txtDATA_ABERTURA.Text));
                                         obj.PDV=int.Parse(txtPDV.Text);
                                         obj.ID_OPERADOR=int.Parse(txtID_OPERADOR.Text);
                                         obj.FILIAL=txtFILIAL.Text;
                                         obj.FINALIZADORA=int.Parse(txtFINALIZADORA.Text);
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
       <center> <h1>Detalhes do tesouraria</h1></center>                  
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
                  
<div id="conteudo" runat="server" class="conteudo" >                  
           <table>              
                <tr>    
/*--Campos Form
                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id</p>
                   <asp:TextBox ID="txtid" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>id_finalizadora</p>
                   <asp:TextBox ID="txtid_finalizadora" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Total_Sistema</p>
                   <asp:TextBox ID="txtTotal_Sistema" runat="server" CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Total_Entregue</p>
                   <asp:TextBox ID="txtTotal_Entregue" runat="server" CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>op_Financeira</p>
                   <asp:TextBox ID="txtop_Financeira" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>DATA_ABERTURA</p>
                   <asp:TextBox ID="txtDATA_ABERTURA" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>PDV</p>
                   <asp:TextBox ID="txtPDV" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>ID_OPERADOR</p>
                   <asp:TextBox ID="txtID_OPERADOR" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>FILIAL</p>
                   <asp:TextBox ID="txtFILIAL" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>FINALIZADORA</p>
                   <asp:TextBox ID="txtFINALIZADORA" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>


                </tr>    
           </table>          
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

