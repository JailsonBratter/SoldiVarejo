using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.code;

namespace visualSysWeb.dao

{
    public class grupoDAO
    {
        public String Codigo_Grupo = "";
        public String Filial = "";
        public String Descricao_Grupo = "";
        public Decimal Codigo_tributacao = 0;
        public String Tecla = "";
        public Decimal comissao = 0;
        public String estado_grupo = "";
        private User usr;
        public SqlDataReader drGrupo = null;
        public grupoDAO()
        {
            Filial = "MATRIZ";

        }
        public grupoDAO(String codigoGrupo)
        { //colocar campo index da tabela



            String sql = "";
            if (!codigoGrupo.Equals(""))
            {
                sql = "Select * from  grupo where Codigo_Grupo =" + codigoGrupo + " and Filial='MATRIZ'";
                SqlDataReader rs = Conexao.consulta(sql, null, true);
                carregarDados(rs);
            }
            else
            {
                sql = "Select * from  grupo";
                drGrupo = Conexao.consulta(sql, null, true);

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
            if (rs.Read())
            {
                Codigo_Grupo = rs["Codigo_Grupo"].ToString();
                Filial = rs["Filial"].ToString();
                Descricao_Grupo = rs["Descricao_Grupo"].ToString();
                Codigo_tributacao = (Decimal)(rs["Codigo_tributacao"].ToString().Equals("") ? new Decimal() : rs["Codigo_tributacao"]);
                Tecla = rs["Tecla"].ToString();
                comissao = (Decimal)(rs["comissao"].ToString().Equals("") ? new Decimal() : rs["comissao"]);
                estado_grupo = rs["estado_grupo"].ToString();
            }

            if (rs != null)
                rs.Close();
        }
        private void update()
        {
            try
            {
                String sql = "update  grupo set " +
                              "Filial='" + Filial + "'" +
                              ",Descricao_Grupo='" + Descricao_Grupo + "'" +
                              ",Codigo_tributacao=" + Codigo_tributacao.ToString("N0") +
                              ",Tecla=" + (Tecla.Trim().Equals("") ? "null" : Tecla) +
                              ",comissao=" + comissao.ToString().Replace(',', '.') +
                              ",estado_grupo=" + (estado_grupo.Trim().Equals("") ? "null" : estado_grupo) +
                    " where Codigo_Grupo= " + Codigo_Grupo.ToString();

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
            int cont = Conexao.countSql("select codigo_grupo from subgrupo where codigo_grupo='" + Codigo_Grupo + "'", null);
            if (cont > 0)
            {
                throw new Exception("NÃO É PERMITIDO EXCLUIR UM GRUPO QUE CONTENHA SUBGRUPOS CADASTRADOS");
            }
            String sql = "delete from grupo  where Codigo_grupo= " + Codigo_Grupo + " AND FILIAL='MATRIZ'"; //colocar campo index
            Conexao.executarSql(sql);
            return true;
        }

        private void insert()
        {
            try
            {
                String sql = " insert into grupo (" +
                          "Codigo_Grupo," +
                          "Filial," +
                          "Descricao_Grupo," +
                          "Codigo_tributacao," +
                          "Tecla," +
                          "comissao," +
                          "estado_grupo" +
                     " )values (" +
                          Codigo_Grupo.ToString() +
                          ",'" + Filial + "'" +
                          ",'" + Descricao_Grupo + "'" +
                          "," + (Codigo_tributacao == 0 ? "null" : Codigo_tributacao.ToString().Replace(",", ".")) +
                          "," + (Tecla.Trim().Equals("") ? "null" : Tecla) +
                          "," + comissao.ToString().Replace(",", ".") +
                          "," + (estado_grupo.Trim().Equals("") ? "null" : estado_grupo) +
                          ")";

                Conexao.executarSql(sql);
                Funcoes.salvaProximaSequencia("grupo.codigo_grupo", usr);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }
    }
}/*/* 
/*================================Metodos tela de Pesq==========================================
using System.Data; 
using visualSysWeb.dao;
           :visualSysWeb.code.PagePadrao     //inicio da classe 
{ 
                  static DataTable tb;
                  static String sqlGrid = ""select * from grupo";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ grupoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                 
                  
/*================================html tela de Pesq==========================================
                  
   <center><h1>grupo</h1></center>
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
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="7"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Codigo_Grupo" Text="Codigo_Grupo" Visible="true" 
                    HeaderText="Codigo_Grupo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Descricao_Grupo" Text="Descricao_Grupo" Visible="true" 
                    HeaderText="Descricao_Grupo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  
//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                    <asp:HyperLinkField DataTextField="Codigo_tributacao" Text="Codigo_tributacao" Visible="true" 
                    HeaderText="Codigo_tributacao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Tecla" Text="Tecla" Visible="true" 
                    HeaderText="Tecla" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="comissao" Text="comissao" Visible="true" 
                    HeaderText="comissao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="estado_grupo" Text="estado_grupo" Visible="true" 
                    HeaderText="estado_grupo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
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
                  : visualSysWeb.code.PagePadrao
   {
                  protected static grupoDAO obj= null ;
                  static String camporeceber = "";
                  protected void Page_Load(object sender, EventArgs e)     
                  {
                       User usr = (User)Session["User"];
                       obj = new grupoDAO(usr);
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
                                         obj = new grupoDAO(clie,usr);
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
                              desabilitaControles(cabecalho);
                              desabilitaControles(conteudo);
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
                    /* --Atualizar DaoForm 
       private void carregarDados()
       {
                                          txtCodigo_Grupo.Text=string.Format("{0:0,0.00}",obj.Codigo_Grupo);
                                          txtFilial.Text=obj.Filial.ToString();
                                          txtDescricao_Grupo.Text=obj.Descricao_Grupo.ToString();
                                          txtCodigo_tributacao.Text=string.Format("{0:0,0.00}",obj.Codigo_tributacao);
                                          chkTecla.Checked =obj.Tecla;
                                          txtcomissao.Text=string.Format("{0:0,0.00}",obj.comissao);
                                          chkestado_grupo.Checked =obj.estado_grupo;
    }

                    /* --Atualizar FormDao 
      private void carregarDadosObj()
      {
                                          obj.Codigo_Grupo=Decimal.Parse(txtCodigo_Grupo.Text);
                                          obj.Filial=txtFilial.Text;
                                          obj.Descricao_Grupo=txtDescricao_Grupo.Text;
                                          obj.Codigo_tributacao=Decimal.Parse(txtCodigo_tributacao.Text);
                                          obj.Tecla=chkTecla.Checked ;
                                          obj.comissao=Decimal.Parse(txtcomissao.Text);
                                          obj.estado_grupo=chkestado_grupo.Checked ;
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
 */
