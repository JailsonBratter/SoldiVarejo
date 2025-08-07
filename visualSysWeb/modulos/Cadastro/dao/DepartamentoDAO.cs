using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace visualSysWeb.dao
{
    public class DepartamentoDAO
    {
        public String Codigo_departamento = "";
        public String Filial = "";
        public String Codigo_Portaria = "";
        public Decimal Codigo_Tributacao = 0;
        public String Codigo_SubGrupo = "";
        public String Descricao_departamento = "";
        public Decimal Margem = 0;
        public String tecla = "";
        public String impressora_remota = "";
        public String id_trm = "";
        public String descricao_impressora = "";
        public Decimal desconto = 0;
        public String diasSemana = "";
        public bool cardapio = false;
        public SqlDataReader drDepartamento = null;
        public DepartamentoDAO()
        {
            Filial = "MATRIZ";
        }
        public DepartamentoDAO(String CodigoDepartamento)
        {
            String sql = "";
            Filial = "MATRIZ";
            if (!CodigoDepartamento.Equals(""))
            {
                sql = "Select * from  Departamento where Codigo_departamento='" + CodigoDepartamento + "' AND FILIAL='MATRIZ'";
                SqlDataReader rs = Conexao.consulta(sql, null, true);
                carregarDados(rs);
            }
            else
            {
                sql = "Select * from  Departamento";
                drDepartamento = Conexao.consulta(sql, null, true);
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
                    Codigo_departamento = rs["Codigo_departamento"].ToString();
                    Filial = rs["Filial"].ToString();
                    Codigo_Portaria = rs["Codigo_Portaria"].ToString();
                    Codigo_Tributacao = (Decimal)(rs["Codigo_Tributacao"].ToString().Equals("") ? new Decimal() : rs["Codigo_Tributacao"]);
                    Codigo_SubGrupo = rs["Codigo_SubGrupo"].ToString();
                    Descricao_departamento = rs["Descricao_departamento"].ToString();
                    Margem = (Decimal)(rs["Margem"].ToString().Equals("") ? new Decimal() : rs["Margem"]);
                    tecla = rs["tecla"].ToString();
                    impressora_remota = rs["impressora_remota"].ToString();
                    id_trm = rs["id_trm"].ToString();
                    descricao_impressora = rs["descricao_impressora"].ToString();
                    desconto = (Decimal)(rs["desconto"].ToString().Equals("") ? new Decimal() : rs["desconto"]);
                    diasSemana = rs["dias_Semana"].ToString();
                    cardapio = rs["cardapio"].ToString().Equals("1");
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
                String sql = "update  Departamento set " +
                              "Codigo_Portaria='" + Codigo_Portaria + "'" +
                              ",Codigo_Tributacao=" + Codigo_Tributacao.ToString().Replace(",", ".") +
                              ",Codigo_SubGrupo='" + Codigo_SubGrupo + "'" +
                              ",Descricao_departamento='" + Descricao_departamento + "'" +
                              ",Margem=" + Margem.ToString().Replace(",", ".") +
                              ",tecla=" + (tecla.Trim().Equals("") ? "null" : tecla) +
                              ",impressora_remota=" + (impressora_remota.Trim().Equals("") ? "null" : impressora_remota) +
                              ",id_trm=" + (id_trm.Trim().Equals("") ? "null" : id_trm) +
                              ",descricao_impressora='" + descricao_impressora + "'" +
                              ",desconto=" + desconto.ToString().Replace(",", ".") +
                              ",dias_semana='" + diasSemana.ToString() + "'" +
                              ",cardapio="+(cardapio?"1":"0")+
                    "  where Codigo_departamento='" + Codigo_departamento + "' AND FILIAL = 'MATRIZ'";
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
            int cont = Conexao.countSql("select codigo_Departamento from mercadoria where codigo_Departamento='" + Codigo_departamento + "'", null);
            if (cont > 0)
            {
                throw new Exception("NÃO É PERMITIDO EXCLUIR UM DEPARTAMENTO QUE CONTENHA MERCADORIAS RELACIONADAS ");
            }


            String sql = "delete from Departamento  where Codigo_departamento='" + Codigo_departamento + "' AND FILIAL='MATRIZ'"; //colocar campo index
            Conexao.executarSql(sql);

            Conexao.executarSql("delete from fornecedor_departamento where codigo_departamento ='" + Codigo_departamento + "'");

            return true;
        }

        private void insert()
        {
            try
            {
                String sql = " insert into Departamento( " +
                          "Codigo_departamento," +
                          "Filial," +
                          "Codigo_Portaria," +
                          "Codigo_Tributacao," +
                          "Codigo_SubGrupo," +
                          "Descricao_departamento," +
                          "Margem," +
                          "tecla," +
                          "impressora_remota," +
                          "id_trm," +
                          "descricao_impressora," +
                          "desconto," +
                          "dias_semana" +
                          ",cardapio"+

                     " )values( " +
                          "'" + Codigo_departamento + "'" +
                          "," + "'" + Filial + "'" +
                          "," + "'" + Codigo_Portaria + "'" +
                          "," + Codigo_Tributacao.ToString().Replace(",", ".") +
                          "," + "'" + Codigo_SubGrupo + "'" +
                          "," + "'" + Descricao_departamento + "'" +
                          "," + Margem.ToString().Replace(",", ".") +
                          "," + (tecla.Trim().Equals("") ? "null" : tecla) +
                          "," + (impressora_remota.Trim().Equals("") ? "null" : impressora_remota) +
                          "," + (id_trm.Trim().Equals("") ? "null" : id_trm) +
                          "," + "'" + descricao_impressora + "'" +
                          "," + desconto.ToString().Replace(",", ".") +
                          ",'" + diasSemana + "'" +
                          ","+(cardapio?"1":"0")+
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
                  static String sqlGrid = ""select * from Departamento";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ DepartamentoDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>Departamento</h1></center>
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
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="11"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Codigo_departamento" Text="Codigo_departamento" Visible="true" 
                    HeaderText="Codigo_departamento" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Codigo_Portaria" Text="Codigo_Portaria" Visible="true" 
                    HeaderText="Codigo_Portaria" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Codigo_Tributacao" Text="Codigo_Tributacao" Visible="true" 
                    HeaderText="Codigo_Tributacao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Codigo_SubGrupo" Text="Codigo_SubGrupo" Visible="true" 
                    HeaderText="Codigo_SubGrupo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Descricao_departamento" Text="Descricao_departamento" Visible="true" 
                    HeaderText="Descricao_departamento" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Margem" Text="Margem" Visible="true" 
                    HeaderText="Margem" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="tecla" Text="tecla" Visible="true" 
                    HeaderText="tecla" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="impressora_remota" Text="impressora_remota" Visible="true" 
                    HeaderText="impressora_remota" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="id_trm" Text="id_trm" Visible="true" 
                    HeaderText="id_trm" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="descricao_impressora" Text="descricao_impressora" Visible="true" 
                    HeaderText="descricao_impressora" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
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
                 protected static DepartamentoDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new DepartamentoDAO();
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
                                        obj = new DepartamentoDAO(index,usr);
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
                           }//Sinto Muito Me Perdoe Agradeço Eu Te Amo.
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
                                         txtCodigo_departamento.Text=obj.Codigo_departamento.ToString();
                                         txtFilial.Text=obj.Filial.ToString();
                                         txtCodigo_Portaria.Text=obj.Codigo_Portaria.ToString();
                                         txtCodigo_Tributacao.Text=string.Format("{0:0,0.00}",obj.Codigo_Tributacao);
                                         txtCodigo_SubGrupo.Text=obj.Codigo_SubGrupo.ToString();
                                         txtDescricao_departamento.Text=obj.Descricao_departamento.ToString();
                                         txtMargem.Text=string.Format("{0:0,0.00}",obj.Margem);
                                         chktecla.Checked =obj.tecla;
                                         chkimpressora_remota.Checked =obj.impressora_remota;
                                         chkid_trm.Checked =obj.id_trm;
                                         txtdescricao_impressora.Text=obj.descricao_impressora.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Codigo_departamento=txtCodigo_departamento.Text;
                                         obj.Filial=txtFilial.Text;
                                         obj.Codigo_Portaria=txtCodigo_Portaria.Text;
                                         obj.Codigo_Tributacao=Decimal.Parse(txtCodigo_Tributacao.Text);
                                         obj.Codigo_SubGrupo=txtCodigo_SubGrupo.Text;
                                         obj.Descricao_departamento=txtDescricao_departamento.Text;
                                         obj.Margem=Decimal.Parse(txtMargem.Text);
                                         obj.tecla=chktecla.Checked ;
                                         obj.impressora_remota=chkimpressora_remota.Checked ;
                                         obj.id_trm=chkid_trm.Checked ;
                                         obj.descricao_impressora=txtdescricao_impressora.Text;
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
       <center> <h1>Detalhes do Departamento</h1></center>                  
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
                                      <td >                   <p>Codigo_departamento</p>
                   <asp:TextBox ID="txtCodigo_departamento" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Filial</p>
                   <asp:TextBox ID="txtFilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Codigo_Portaria</p>
                   <asp:TextBox ID="txtCodigo_Portaria" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Codigo_Tributacao</p>
                   <asp:TextBox ID="txtCodigo_Tributacao" runat="server"  CssClass="numero" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Codigo_SubGrupo</p>
                   <asp:TextBox ID="txtCodigo_SubGrupo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Descricao_departamento</p>
                   <asp:TextBox ID="txtDescricao_departamento" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Margem</p>
                   <asp:TextBox ID="txtMargem" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>tecla</p>
                   <td><asp:CheckBox ID="chktecla" runat="server" Text="tecla"/>
                   </td>

                                      <td >                   <p>impressora_remota</p>
                   <td><asp:CheckBox ID="chkimpressora_remota" runat="server" Text="impressora_remota"/>
                   </td>

                                      <td >                   <p>id_trm</p>
                   <td><asp:CheckBox ID="chkid_trm" runat="server" Text="id_trm"/>
                   </td>

                                      <td >                   <p>descricao_impressora</p>
                   <asp:TextBox ID="txtdescricao_impressora" runat="server" ></asp:TextBox>
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

