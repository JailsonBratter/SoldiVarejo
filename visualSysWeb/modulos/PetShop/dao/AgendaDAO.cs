using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace visualSysWeb.dao
{
    public class AgendaDAO
    {
        public String Filial { get; set; }
        public String Nome { get; set; }
        public String Inicio { get; set; }
        public String Nome_Pet { get; set; }
        public bool Cancelado { get; set; }
        public DateTime Data { get; set; }
        public String DataBr()
        {
            return dataBr(Data);
        }

        public String Sigla = "";
        public String Codigo_Cliente { get; set; }
        public String NomeCliente {
            get
            {
                return Conexao.retornaUmValor("Select Nome_Cliente from cliente where codigo_cliente ='"+Codigo_Cliente+"'", usr);
            }
        }
        public String Fim { get; set; }
        public bool Gera_pedido { get; set; }
        public bool delivery { get; set; }
        public String Pedido { get; set; }
        public String Obs { get; set; }
        public String Obs_Veterinario { get; set; }
        public String Hora_retirada { get; set; }
        public String Hora_entrega_prevista { get; set; }
        public String Hora_entrega_real { get; set; }
        public int Saida_KM { get; set; }
        public int Chegada_KM { get; set; }
        public String Funcionario_entrega { get; set; }
        public String Funcionario_retira = "";
        public String usuario_cadastro = "";
        private User usr = null;
        public AgendaDAO(User usr)
        {
            Filial = usr.getFilial();
            this.usr = usr;
        }
        public AgendaDAO(String codigoPedido, User usr)
        { //colocar campo index da tabela

            String sql = "Select * from  Agenda where pedido =" + codigoPedido;
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
            Filial = usr.getFilial();
            this.usr = usr;
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
                Filial = rs["Filial"].ToString();
                Nome = rs["Nome"].ToString();
                Inicio = rs["Inicio"].ToString();
                Nome_Pet = rs["Nome_Pet"].ToString();
                Cancelado = (rs["Cancelado"].ToString().Equals("1") ? true : false);
                Data = (rs["Data"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data"].ToString()));
                Sigla = rs["Sigla"].ToString();
                Codigo_Cliente = rs["Codigo_Cliente"].ToString();
                Fim = rs["Fim"].ToString();
                Gera_pedido = (rs["Gera_pedido"].ToString().Equals("1") ? true : false);
                delivery = (rs["delivery"].ToString().Equals("1") ? true : false);
                Pedido = rs["Pedido"].ToString();
                Obs = rs["Obs"].ToString();
                Obs_Veterinario = rs["Obs_Veterinario"].ToString();
                Hora_retirada = rs["Hora_retirada"].ToString();
                Hora_entrega_prevista = rs["Hora_entrega_prevista"].ToString();
                Hora_entrega_real = rs["Hora_entrega_real"].ToString();
                Saida_KM = (rs["Saida_KM"] == null ? 0 : int.Parse(rs["Saida_KM"].ToString()));
                Chegada_KM = (rs["Chegada_KM"] == null ? 0 : int.Parse(rs["Chegada_KM"].ToString()));
                Funcionario_entrega = rs["Funcionario_entrega"].ToString();
                Funcionario_retira = rs["Funcionario_retira"].ToString();
                usuario_cadastro = rs["usuario_cadastro"].ToString();
            }
            if (rs != null)
                rs.Close();
        }
        private void update()
        {
            try
            {
                String strLivre = Conexao.retornaUmValor(
                    "select COUNT(*) from agenda " +
                      " where  ("+
                                "('" + Inicio + "' >= inicio AND '" + Inicio + "'  < fim )"+
                                " or('" + Fim + "' > inicio AND '" + Fim + "'  < fim )"+
                                " OR (Inicio>='"+Inicio+"' AND Fim <='"+Fim+"' ) " +
                               ")and  DATA ='" + Data.ToString("yyyy-MM-dd") + "'"+
                               "and pedido<>'"+Pedido+"' and nome='"+Nome.Trim()+"'", null);

                if (!strLivre.Equals("0"))
                    throw new Exception("O Horario já esta Reservado");

                String sql = "update  Agenda set " +
                              "Filial='" + Filial + "'" +
                              ",Nome='" + Nome + "'" +
                              ",Inicio='" + Inicio + "'" +
                              ",Nome_Pet='" + Nome_Pet + "'" +
                              ",Cancelado=" + (Cancelado ? "1" : "0") +
                              ",Data=" + (Data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data.ToString("yyyy-MM-dd") + "'")  +
                              ",Sigla='" + Sigla + "'" +
                              ",Codigo_Cliente='" + Codigo_Cliente + "'" +
                              ",Fim='" + Fim + "'" +
                              ",Gera_pedido=" + (Gera_pedido ? "1" : "0") +
                              ",delivery=" + (delivery ? "1" : "0") +

                              ",Obs='" + Obs + "'" +
                              ",Obs_Veterinario='" + Obs_Veterinario +"'"+
                              ",Hora_retirada='" + Hora_retirada + "'" +
                              ",Hora_entrega_prevista='" + Hora_entrega_prevista + "'" +
                              ",Hora_entrega_real='" + Hora_entrega_real + "'" +
                              ",Saida_KM=" + Saida_KM +
                              ",Chegada_KM=" + Chegada_KM +
                              ",Funcionario_entrega='" + Funcionario_entrega.Trim() + "'" +
                              ",Funcionario_retira='"+Funcionario_retira.Trim()+"'"+
                              ",usuario_cadastro='"+usr.getNome()+"'"+
                    "  where Pedido='" + Pedido + "'"
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
            String sql = "delete from Agenda  where Pedido='" + Pedido.Trim() + "'";
            Conexao.executarSql(sql);
            return true;
        }

        private void insert()
        {
            try
            {
                Pedido = visualSysWeb.code.Funcoes.sequencia("AGENDA.PEDIDO", usr);

                String strLivre = Conexao.retornaUmValor("select COUNT(*) from agenda " +
                      " where  (" +
                                "('" + Inicio + "' >= inicio AND '" + Inicio + "'  < fim )" +
                                " or('" + Fim + "' > inicio AND '" + Fim + "'  < fim )" +
                                " OR (Inicio>='" + Inicio + "' AND Fim <='" + Fim + "' ) " +
                               ")and  DATA ='" + Data.ToString("yyyy-MM-dd") + "'" +
                               "and pedido<>'" + Pedido + "' and nome='" + Nome.Trim() + "'", null);

                if (!strLivre.Equals("0"))
                    throw new Exception("O Horario já esta Reservado");


                String sql = " insert into Agenda( " +
                          "Filial," +
                          "Nome," +
                          "Inicio," +
                          "Nome_Pet," +
                          "Cancelado," +
                          "Data," +
                          "Sigla," +
                          "Codigo_Cliente," +
                          "Fim," +
                          "Gera_pedido," +
                          "delivery," +
                          "Pedido," +
                          "Obs," +
                          "Obs_Veterinario," +
                          "Hora_retirada," +
                          "Hora_entrega_prevista," +
                          "Hora_entrega_real," +
                          "Saida_KM," +
                          "Chegada_KM," +
                          "Funcionario_entrega" +
                          ",Funcionario_retira"+
                          ",usuario_cadastro"+
                     " )values( " +
                          "'" + Filial + "'" +
                          "," + "'" + Nome + "'" +
                          "," + "'" + Inicio + "'" +
                          "," + "'" + Nome_Pet + "'" +
                          "," + (Cancelado ? 1 : 0) +
                          "," + (Data.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + Sigla + "'" +
                          "," + "'" + Codigo_Cliente + "'" +
                          "," + "'" + Fim + "'" +
                          "," + (Gera_pedido ? 1 : 0) +
                          "," + (delivery ? 1 : 0) +
                          "," + "'" + Pedido + "'" +
                          "," + "'" + Obs + "'" +
                          ",'" + Obs_Veterinario+"'"+
                          "," + "'" + Hora_retirada + "'" +
                          "," + "'" + Hora_entrega_prevista + "'" +
                          "," + "'" + Hora_entrega_real + "'" +
                          "," + Saida_KM +
                          "," + Chegada_KM +
                          "," + "'" + Funcionario_entrega + "'" +
                          ",'"+Funcionario_retira+"'"+
                          ",'"+usr.getNome()+"'"+
                          
                         ");";

                Conexao.executarSql(sql);
                visualSysWeb.code.Funcoes.salvaProximaSequencia("AGENDA.PEDIDO", usr);
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
                  static String sqlGrid = ""select * from Agenda";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ AgendaDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>Agenda</h1></center>
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
                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Nome" Text="Nome" Visible="true" 
                    HeaderText="Nome" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Inicio" Text="Inicio" Visible="true" 
                    HeaderText="Inicio" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Nome_Pet" Text="Nome_Pet" Visible="true" 
                    HeaderText="Nome_Pet" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Cancelado" Text="Cancelado" Visible="true" 
                    HeaderText="Cancelado" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Data" Text="Data" Visible="true" 
                    HeaderText="Data" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Sigla" Text="Sigla" Visible="true" 
                    HeaderText="Sigla" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Codigo_Cliente" Text="Codigo_Cliente" Visible="true" 
                    HeaderText="Codigo_Cliente" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Fim" Text="Fim" Visible="true" 
                    HeaderText="Fim" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Gera_pedido" Text="Gera_pedido" Visible="true" 
                    HeaderText="Gera_pedido" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="delivery" Text="delivery" Visible="true" 
                    HeaderText="delivery" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Pedido" Text="Pedido" Visible="true" 
                    HeaderText="Pedido" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Obs" Text="Obs" Visible="true" 
                    HeaderText="Obs" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Obs_Veterinario" Text="Obs_Veterinario" Visible="true" 
                    HeaderText="Obs_Veterinario" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Hora_retirada" Text="Hora_retirada" Visible="true" 
                    HeaderText="Hora_retirada" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Hora_entrega_prevista" Text="Hora_entrega_prevista" Visible="true" 
                    HeaderText="Hora_entrega_prevista" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Hora_entrega_real" Text="Hora_entrega_real" Visible="true" 
                    HeaderText="Hora_entrega_real" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Saida_KM" Text="Saida_KM" Visible="true" 
                    HeaderText="Saida_KM" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Chegada_KM" Text="Chegada_KM" Visible="true" 
                    HeaderText="Chegada_KM" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Funcionario_entrega" Text="Funcionario_entrega" Visible="true" 
                    HeaderText="Funcionario_entrega" DataNavigateUrlFormatString="~/modulos/Coloca Nome Do Modulo/AgendaDetalhes.aspx?campoIndex={0}" 
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
                 protected static AgendaDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new AgendaDAO();
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
                                        obj = new AgendaDAO(index,usr);
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
                                         txtFilial.Text=obj.Filial.ToString();
                                         txtNome.Text=obj.Nome.ToString();
                                         txtInicio.Text=obj.Inicio.ToString();
                                         txtNome_Pet.Text=obj.Nome_Pet.ToString();
                                         chkCancelado.Checked =obj.Cancelado;
                                         txtData.Text=obj.DataBr();
                                         txtSigla.Text=obj.Sigla.ToString();
                                         txtCodigo_Cliente.Text=obj.Codigo_Cliente.ToString();
                                         txtFim.Text=obj.Fim.ToString();
                                         chkGera_pedido.Checked =obj.Gera_pedido;
                                         chkdelivery.Checked =obj.delivery;
                                         txtPedido.Text=obj.Pedido.ToString();
                                         txtObs.Text=obj.Obs.ToString();
                                         chkObs_Veterinario.Checked =obj.Obs_Veterinario;
                                         txtHora_retirada.Text=obj.Hora_retirada.ToString();
                                         txtHora_entrega_prevista.Text=obj.Hora_entrega_prevista.ToString();
                                         txtHora_entrega_real.Text=obj.Hora_entrega_real.ToString();
                                         txtSaida_KM.Text=obj.Saida_KM.ToString();
                                         txtChegada_KM.Text=obj.Chegada_KM.ToString();
                                         txtFuncionario_entrega.Text=obj.Funcionario_entrega.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Filial=txtFilial.Text;
                                         obj.Nome=txtNome.Text;
                                         obj.Inicio=txtInicio.Text;
                                         obj.Nome_Pet=txtNome_Pet.Text;
                                         obj.Cancelado=chkCancelado.Checked ;
                                         obj.Data=(txtData.Text.Equals("")?new DateTime():DateTime.Parse(txtData.Text));
                                         obj.Sigla=txtSigla.Text;
                                         obj.Codigo_Cliente=txtCodigo_Cliente.Text;
                                         obj.Fim=txtFim.Text;
                                         obj.Gera_pedido=chkGera_pedido.Checked ;
                                         obj.delivery=chkdelivery.Checked ;
                                         obj.Pedido=txtPedido.Text;
                                         obj.Obs=txtObs.Text;
                                         obj.Obs_Veterinario=chkObs_Veterinario.Checked ;
                                         obj.Hora_retirada=txtHora_retirada.Text;
                                         obj.Hora_entrega_prevista=txtHora_entrega_prevista.Text;
                                         obj.Hora_entrega_real=txtHora_entrega_real.Text;
                                         obj.Saida_KM=int.Parse(txtSaida_KM.Text);
                                         obj.Chegada_KM=int.Parse(txtChegada_KM.Text);
                                         obj.Funcionario_entrega=txtFuncionario_entrega.Text;
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
       <center> <h1>Detalhes do Agenda</h1></center>                  
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
                                      <td >                   <p>Filial</p>
                   <asp:TextBox ID="txtFilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Nome</p>
                   <asp:TextBox ID="txtNome" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Inicio</p>
                   <asp:TextBox ID="txtInicio" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Nome_Pet</p>
                   <asp:TextBox ID="txtNome_Pet" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Cancelado</p>
                   <td><asp:CheckBox ID="chkCancelado" runat="server" Text="Cancelado"/>
                   </td>

                                      <td >                   <p>Data</p>
                   <asp:TextBox ID="txtData" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Sigla</p>
                   <asp:TextBox ID="txtSigla" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Codigo_Cliente</p>
                   <asp:TextBox ID="txtCodigo_Cliente" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Fim</p>
                   <asp:TextBox ID="txtFim" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Gera_pedido</p>
                   <td><asp:CheckBox ID="chkGera_pedido" runat="server" Text="Gera_pedido"/>
                   </td>

                                      <td >                   <p>delivery</p>
                   <td><asp:CheckBox ID="chkdelivery" runat="server" Text="delivery"/>
                   </td>

                                      <td >                   <p>Pedido</p>
                   <asp:TextBox ID="txtPedido" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Obs</p>
                   <asp:TextBox ID="txtObs" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Obs_Veterinario</p>
                   <td><asp:CheckBox ID="chkObs_Veterinario" runat="server" Text="Obs_Veterinario"/>
                   </td>

                                      <td >                   <p>Hora_retirada</p>
                   <asp:TextBox ID="txtHora_retirada" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Hora_entrega_prevista</p>
                   <asp:TextBox ID="txtHora_entrega_prevista" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Hora_entrega_real</p>
                   <asp:TextBox ID="txtHora_entrega_real" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Saida_KM</p>
                   <asp:TextBox ID="txtSaida_KM" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Chegada_KM</p>
                   <asp:TextBox ID="txtChegada_KM" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Funcionario_entrega</p>
                   <asp:TextBox ID="txtFuncionario_entrega" runat="server" ></asp:TextBox>
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

