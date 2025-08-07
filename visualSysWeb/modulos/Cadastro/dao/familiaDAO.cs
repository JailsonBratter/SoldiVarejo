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
    public class familiaDAO
    {
        public String Codigo_familia { get; set; }
        public String Filial { get; set; }
        public String Tipo = "";
        public String Codigo_Portaria = "";
        public Decimal Codigo_Tributacao { get; set; }
        public String Codigo_departamento { get; set; }
        public String Descricao_Familia { get; set; }
        public Decimal Margem { get; set; }
        public Decimal Estoque_minimo { get; set; }
        public String Peso_variavel { get; set; }
        public String Etiqueta { get; set; }
        public Decimal Validade { get; set; }
        public Decimal Fator_conversao { get; set; }
        public bool Promocao { get; set; }
        public bool Promocao_Automatica { get; set; }
        public bool imprimeEtiquetaItens = false;
        public DateTime Data_inicio { get; set; }
        public String Data_inicioBr()
        {
            return dataBr(Data_inicio);
        }

        public DateTime Data_fim { get; set; }
        public String Data_fimBr()
        {
            return dataBr(Data_fim);
        }

        public String Descricao_Departamento { get; set; }

        private ArrayList arrExcluidos = new ArrayList();
        private ArrayList arrItens = new ArrayList();
        public DataTable itens
        {
            get
            {
                ArrayList conteudo = new ArrayList();

                ArrayList cabecalho = new ArrayList();
                cabecalho.Add("PLU");
                cabecalho.Add("DESCRICAO");
                cabecalho.Add("CUSTO");
                cabecalho.Add("MARGEM");
                cabecalho.Add("PRECO");
                cabecalho.Add("PROMOCAO");


                conteudo.Add(cabecalho);

                foreach (ArrayList item in arrItens)
                {
                    conteudo.Add(item);
                }
                return Conexao.GetArryTable(conteudo);
            }

        }

        public void addItens(String plu, String descricao, String custo, String margem, String preco,String PrecoPromocao)
        {
            ArrayList newItem = new ArrayList();
            newItem.Add(plu);
            newItem.Add(descricao);
            newItem.Add(custo);
            newItem.Add(margem);
            newItem.Add(preco);
            newItem.Add(PrecoPromocao);
            arrItens.Add(newItem);

        }

        public void excluirItem(int index)
        {


            arrExcluidos.Add(arrItens[index]);
            arrItens.RemoveAt(index);



        }
        public bool aplicarTodasFiliais = false;
        public Decimal preco = 0;

        public int qtd_Etiqueta = 0;


        private User usr = null;


        public familiaDAO(User usr)
        {
            Filial = usr.getFilial();
            this.usr = usr;
        }
        public familiaDAO(String codigoFamilia, User usr)
        { //colocar campo index da tabela
            this.usr = usr;
            this.Codigo_familia = codigoFamilia;

            carregarDados();
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
        public void carregarDados()
        {
            String sql = "Select * from  familia where codigo_familia ='" + Codigo_familia + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            if (rs.Read())
            {

                Filial = rs["Filial"].ToString();
                Tipo = rs["Tipo"].ToString();
                Codigo_Portaria = rs["Codigo_Portaria"].ToString();
                Codigo_Tributacao = (Decimal)(rs["Codigo_Tributacao"].ToString().Equals("") ? new Decimal() : rs["Codigo_Tributacao"]);
                Codigo_departamento = rs["Codigo_departamento"].ToString();
                Descricao_Familia = rs["Descricao_Familia"].ToString();
                Margem = (Decimal)(rs["Margem"].ToString().Equals("") ? new Decimal() : rs["Margem"]);
                Estoque_minimo = (Decimal)(rs["Estoque_minimo"].ToString().Equals("") ? new Decimal() : rs["Estoque_minimo"]);
                Peso_variavel = rs["Peso_variavel"].ToString();
                Etiqueta = rs["Etiqueta"].ToString();
                Validade = (Decimal)(rs["Validade"].ToString().Equals("") ? new Decimal() : rs["Validade"]);
                Fator_conversao = (Decimal)(rs["Fator_conversao"].ToString().Equals("") ? new Decimal() : rs["Fator_conversao"]);
                Promocao = (rs["Promocao"].ToString().Equals("1") ? true : false);
                Promocao_Automatica = (rs["Promocao_Automatica"].ToString().Equals("1") ? true : false);
                Data_inicio = (rs["Data_inicio"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_inicio"].ToString()));
                Data_fim = (rs["Data_fim"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["Data_fim"].ToString()));
                Descricao_Departamento = rs["Descricao_Departamento"].ToString();
                preco = ((rs["preco"].ToString().Equals("") ? 0 : Decimal.Parse(rs["preco"].ToString())));
                imprimeEtiquetaItens = (rs["Imprimir_Etiqueta_itens"].ToString().Equals("1") ? true : false);
                qtd_Etiqueta = (rs["qtd_etiquetas"].ToString().Equals("") ? 0 : int.Parse(rs["qtd_etiquetas"].ToString()));

                SqlDataReader rsMerc = Conexao.consulta("Select a.plu,a.descricao,b.Preco_Custo,b.margem,b.preco,preco_promocao = case when b.promocao=1then b.preco_promocao else 0 end from mercadoria a inner join Mercadoria_Loja b on a.PLU=b.PLU  where a.Codigo_familia = '" + Codigo_familia + "' and b.Filial='" + usr.getFilial() + "'", null, false);
                while (rsMerc.Read())
                {
                    Decimal vpreco = ((rsMerc["preco"].ToString().Equals("") ? 0 : Decimal.Parse(rsMerc["preco"].ToString())));
                    Decimal vcusto = ((rsMerc["Preco_Custo"].ToString().Equals("") ? 0 : Decimal.Parse(rsMerc["Preco_Custo"].ToString())));
                    Decimal vmargem = ((rsMerc["margem"].ToString().Equals("") ? 0 : Decimal.Parse(rsMerc["margem"].ToString())));
                    Decimal vPrecoPromocao =((rsMerc["preco_promocao"].ToString().Equals("") ? 0 : Decimal.Parse(rsMerc["preco_promocao"].ToString())));
                    addItens(rsMerc["plu"].ToString(), rsMerc["descricao"].ToString(), vcusto.ToString("N2"), vmargem.ToString("N4"), vpreco.ToString("N2"),vPrecoPromocao.ToString("N2"));
                }

                if (rsMerc != null)
                    rsMerc.Close();

            }

            if (rs != null)
                rs.Close();
        }
        private void update()
        {
            try
            {



                String sql = "update  familia set " +
                              "Filial='" + Filial + "'" +
                              ",Tipo=" + (Tipo.Trim().Equals("") ? "null" : "'" + Tipo + "'") +
                              ",Codigo_Portaria=" + (Codigo_Portaria.Trim().Equals("") ? "null" : "'" + Codigo_Portaria + "'") +
                              ",Codigo_Tributacao=" + Codigo_Tributacao.ToString().Replace(",", ".") +
                              ",Codigo_departamento='" + Codigo_departamento + "'" +
                              ",Descricao_Familia='" + Descricao_Familia + "'" +
                              ",Margem=" + Margem.ToString().Replace(",", ".") +
                              ",Estoque_minimo=" + Estoque_minimo.ToString().Replace(",", ".") +
                              ",Peso_variavel='" + Peso_variavel + "'" +
                              ",Etiqueta='" + Etiqueta + "'" +
                              ",Validade=" + Validade.ToString().Replace(",", ".") +
                              ",Fator_conversao=" + Fator_conversao.ToString().Replace(",", ".") +
                              ",Promocao=" + (Promocao ? "1" : "0") +
                              ",Promocao_Automatica=" + (Promocao_Automatica ? "1" : "0") +
                              ",Data_inicio=" + (Data_inicio.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_inicio.ToString("yyyy-MM-dd") + "'") +
                              ",Data_fim=" + (Data_fim.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_fim.ToString("yyyy-MM-dd") + "'") +
                              ",Descricao_Departamento='" + Descricao_Departamento + "'" +
                              ",preco=" + preco.ToString().Replace(",", ".") +
                              ",qtd_etiquetas=" + qtd_Etiqueta.ToString() +
                              ",imprime_etiqueta=1" +
                              ",Imprimir_Etiqueta_itens="+(imprimeEtiquetaItens ? "1" : "0")+
                    "  where Codigo_familia='" + Codigo_familia + "'";
                Conexao.executarSql(sql);


                atualizarItens();

            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }

        private void atualizarItens()
        {
            if (arrExcluidos.Count > 0)
            {
                foreach (ArrayList item in arrExcluidos)
                {
                    Conexao.executarSql("update mercadoria set codigo_familia='', Descricao_familia='' where plu ='" + ((String)item[0]) + "'");
                }
            }


            if (arrItens.Count > 0)
            {
                
                foreach (ArrayList item in arrItens)
                {

                    Decimal precoAntigo = Decimal.Parse(Conexao.retornaUmValor("select isnull(preco,0) from mercadoria_loja where plu ='" + (String)item[0] + "' and filial = '" + usr.getFilial() + "'", null));
                    Decimal custo = Decimal.Parse(Conexao.retornaUmValor("select isnull(preco_custo,0) from mercadoria_loja where plu ='" + (String)item[0] + "' and filial = '" + usr.getFilial() + "'", null)); ;

                    if (precoAntigo != preco || aplicarTodasFiliais)
                    {
                        String sqlLog = "insert into log_preco(plu,descricao,data,usuario,preco_old,preco_new,filial)" +
                                            "values ('" + (String)item[0] + "'," +
                                            " 'Alteracao preco da familia " + Codigo_familia + "'," +
                                            " GETDATE()," +
                                            "'" + usr.getNome() + "'," +
                                            precoAntigo.ToString().Replace(",", ".") + "," +
                                            preco.ToString().Replace(",", ".") + ",";
                        if (aplicarTodasFiliais)
                            sqlLog += " 'TODAS' ";
                        else
                            sqlLog += " '" + usr.getFilial() + "'";
                        sqlLog += ")";

                        Conexao.executarSql(sqlLog);
                    }
                   
                    Decimal margem = Funcoes.verificamargem(custo, preco, 0, 0);


                    item[2] = custo.ToString("N4");
                    item[3] = margem.ToString("N4");
                    item[4] = preco.ToString("N2");
                    Conexao.executarSql("update mercadoria set codigo_familia='" + Codigo_familia + "' ,Descricao_familia='" + Descricao_Familia + "',margem=" + margem.ToString().Replace(",", ".") + ", preco=" + preco.ToString().Replace(',', '.') + ", Data_alteracao='" + DateTime.Now.ToString("yyyy-MM-dd") + "', Estado_Mercadoria = 1   where plu ='" + ((String)item[0]) + "'");

                    String sqlFilial = "update mercadoria_loja set preco =" + preco.ToString().Replace(",", ".") + ",margem=" + margem.ToString().Replace(",", ".") + " where plu='" + (String)item[0] + "' and filial='" + usr.getFilial() + "'";

                    Conexao.executarSql(sqlFilial);

                    if (aplicarTodasFiliais)
                    {
                        SqlDataReader rsFiliais = Conexao.consulta("Select preco_custo,filial from mercadoria_loja where plu='" + (String)item[0] + "' and filial <>'" + usr.getFilial() + "'", null, false);
                        while (rsFiliais.Read())
                        {
                            Decimal pCusto = rsFiliais["preco_custo"].ToString().Equals("") ? 0 : Decimal.Parse(rsFiliais["preco_custo"].ToString());
                            Decimal vMargem = Funcoes.verificamargem(pCusto, preco, 0, 0);

                            Conexao.executarSql("update mercadoria_loja set Preco =" + preco.ToString().Replace(",", ".") +", margem=" + vMargem.ToString().Replace(",", ".") + " where plu='" + (String)item[0] + "' and Filial='" + rsFiliais["filial"].ToString() + "'");

                        }
                        if (rsFiliais != null)
                            rsFiliais.Close();

                    }
                }
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
            String sql = "delete from familia  where codigo_familia= '" + Codigo_familia + "'"; //colocar campo index
            Conexao.executarSql(sql);
            return true;
        }

        private void insert()
        {
            try
            {
                Codigo_familia = Funcoes.sequencia("FAMILIA.CODIGO_FAMILIA", usr);

                String sql = " insert into familia (" +
                          "Codigo_familia," +
                          "Filial," +
                          "Tipo," +
                          "Codigo_Portaria," +
                          "Codigo_Tributacao," +
                          "Codigo_departamento," +
                          "Descricao_Familia," +
                          "Margem," +
                          "Estoque_minimo," +
                          "Peso_variavel," +
                          "Etiqueta," +
                          "Validade," +
                          "Fator_conversao," +
                          "Promocao," +
                          "Promocao_Automatica," +
                          "Data_inicio," +
                          "Data_fim," +
                          "Descricao_Departamento," +
                          "preco," +
                          "qtd_etiquetas," +
                          "imprime_etiqueta," +
                          "Imprimir_Etiqueta_itens"+
                     " )values (" +
                          "'" + Codigo_familia + "'" +
                          "," + "'MATRIZ'" +
                          "," + (Tipo.Trim().Equals("") ? "null" : "'" + Tipo + "'") +
                          "," + (Codigo_Portaria.Trim().Equals("") ? "null" : "'" + Codigo_Portaria + "'") +
                          "," + Codigo_Tributacao.ToString().Replace(",", ".") +
                          "," + "'" + Codigo_departamento + "'" +
                          "," + "'" + Descricao_Familia + "'" +
                          "," + Margem.ToString().Replace(",", ".") +
                          "," + Estoque_minimo.ToString().Replace(",", ".") +
                          "," + "'" + Peso_variavel + "'" +
                          "," + "'" + Etiqueta + "'" +
                          "," + Validade.ToString().Replace(",", ".") +
                          "," + Fator_conversao.ToString().Replace(",", ".") +
                          "," + (Promocao ? 1 : 0) +
                          "," + (Promocao_Automatica ? 1 : 0) +
                          "," + (Data_inicio.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_inicio.ToString("yyyy-MM-dd") + "'") +
                          "," + (Data_fim.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + Data_fim.ToString("yyyy-MM-dd") + "'") +
                          "," + "'" + Descricao_Departamento + "'" +
                          "," + preco.ToString().Replace(",", ".") +
                          "," + qtd_Etiqueta +
                          ",1" +
                          ","+(imprimeEtiquetaItens?1:0)+
                         ");";

                Conexao.executarSql(sql);
                Funcoes.salvaProximaSequencia("FAMILIA.CODIGO_FAMILIA", usr);

                atualizarItens();
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
                  static String sqlGrid = ""select * from familia";//colocar os campos no select que ser?o apresentados na tela
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
                      Response.Redirect("~/modulos/nome Do Modulo/pages/ familiaDetalhes.aspx?novo=true"); // colocar caminho da pagina de Detalhes verificar Case sensitive
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
                  
   <center><h1>familia</h1></center>
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
                 onpageindexchanging="gridPesquisa_PageIndexChanging" CellPadding="18"  
                 ForeColor="#333333" GridLines="None"  
                 > 
                 <AlternatingRowStyle BackColor="#BEBEBE" ForeColor="#284775" /> 
                 <Columns> 
                    <asp:HyperLinkField DataTextField="Codigo_familia" Text="Codigo_familia" Visible="true" 
                    HeaderText="Codigo_familia" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Filial" Text="Filial" Visible="true" 
                    HeaderText="Filial" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Tipo" Text="Tipo" Visible="true" 
                    HeaderText="Tipo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Codigo_Portaria" Text="Codigo_Portaria" Visible="true" 
                    HeaderText="Codigo_Portaria" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Codigo_Tributacao" Text="Codigo_Tributacao" Visible="true" 
                    HeaderText="Codigo_Tributacao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Codigo_departamento" Text="Codigo_departamento" Visible="true" 
                    HeaderText="Codigo_departamento" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Descricao_Familia" Text="Descricao_Familia" Visible="true" 
                    HeaderText="Descricao_Familia" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Margem" Text="Margem" Visible="true" 
                    HeaderText="Margem" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Estoque_minimo" Text="Estoque_minimo" Visible="true" 
                    HeaderText="Estoque_minimo" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Peso_variavel" Text="Peso_variavel" Visible="true" 
                    HeaderText="Peso_variavel" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Etiqueta" Text="Etiqueta" Visible="true" 
                    HeaderText="Etiqueta" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Validade" Text="Validade" Visible="true" 
                    HeaderText="Validade" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Fator_conversao" Text="Fator_conversao" Visible="true" 
                    HeaderText="Fator_conversao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Promocao" Text="Promocao" Visible="true" 
                    HeaderText="Promocao" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Promocao_Automatica" Text="Promocao_Automatica" Visible="true" 
                    HeaderText="Promocao_Automatica" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Data_inicio" Text="Data_inicio" Visible="true" 
                    HeaderText="Data_inicio" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Data_fim" Text="Data_fim" Visible="true" 
                    HeaderText="Data_fim" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
                        DataNavigateUrlFields="//colocar o campo Index que fara a pesquisar" />  

                    <asp:HyperLinkField DataTextField="Descricao_Departamento" Text="Descricao_Departamento" Visible="true" 
                    HeaderText="Descricao_Departamento" DataNavigateUrlFormatString="~/modulos/colocar endere?o da pagina de detalhes .aspx?campoIndex={0}" 
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
                 protected static familiaDAO obj= null ;
                 static String camporeceber = "";
                 protected void Page_Load(object sender, EventArgs e)     
                 {
                      User usr = (User)Session["User"];
                      obj = new familiaDAO();
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
                                        status = "visualizar"; //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
                                        obj = new familiaDAO(index,usr);
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
                                         txtCodigo_familia.Text=obj.Codigo_familia.ToString();
                                         txtFilial.Text=obj.Filial.ToString();
                                         txtTipo.Text=obj.Tipo.ToString();
                                         txtCodigo_Portaria.Text=obj.Codigo_Portaria.ToString();
                                         txtCodigo_Tributacao.Text=string.Format("{0:0,0.00}",obj.Codigo_Tributacao);
                                         txtCodigo_departamento.Text=obj.Codigo_departamento.ToString();
                                         txtDescricao_Familia.Text=obj.Descricao_Familia.ToString();
                                         txtMargem.Text=string.Format("{0:0,0.00}",obj.Margem);
                                         txtEstoque_minimo.Text=string.Format("{0:0,0.00}",obj.Estoque_minimo);
                                         txtPeso_variavel.Text=obj.Peso_variavel.ToString();
                                         txtEtiqueta.Text=obj.Etiqueta.ToString();
                                         txtValidade.Text=string.Format("{0:0,0.00}",obj.Validade);
                                         txtFator_conversao.Text=string.Format("{0:0,0.00}",obj.Fator_conversao);
                                         chkPromocao.Checked =obj.Promocao;
                                         chkPromocao_Automatica.Checked =obj.Promocao_Automatica;
                                         txtData_inicio.Text=obj.Data_inicioBr();
                                         txtData_fim.Text=obj.Data_fimBr();
                                         txtDescricao_Departamento.Text=obj.Descricao_Departamento.ToString();
   }

                   // --Atualizar FormDao 
     private void carregarDadosObj()
     {
                                         obj.Codigo_familia=txtCodigo_familia.Text;
                                         obj.Filial=txtFilial.Text;
                                         obj.Tipo=txtTipo.Text;
                                         obj.Codigo_Portaria=txtCodigo_Portaria.Text;
                                         obj.Codigo_Tributacao=Decimal.Parse(txtCodigo_Tributacao.Text);
                                         obj.Codigo_departamento=txtCodigo_departamento.Text;
                                         obj.Descricao_Familia=txtDescricao_Familia.Text;
                                         obj.Margem=Decimal.Parse(txtMargem.Text);
                                         obj.Estoque_minimo=Decimal.Parse(txtEstoque_minimo.Text);
                                         obj.Peso_variavel=txtPeso_variavel.Text;
                                         obj.Etiqueta=txtEtiqueta.Text;
                                         obj.Validade=Decimal.Parse(txtValidade.Text);
                                         obj.Fator_conversao=Decimal.Parse(txtFator_conversao.Text);
                                         obj.Promocao=chkPromocao.Checked ;
                                         obj.Promocao_Automatica=chkPromocao_Automatica.Checked ;
                                         obj.Data_inicio=DateTime.Parse(txtData_inicio.Text);
                                         obj.Data_fim=DateTime.Parse(txtData_fim.Text);
                                         obj.Descricao_Departamento=txtDescricao_Departamento.Text;
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
       <center> <h1>Detalhes do familia</h1></center>                  
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
                                      <td >                   <p>Codigo_familia</p>
                   <asp:TextBox ID="txtCodigo_familia" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Filial</p>
                   <asp:TextBox ID="txtFilial" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Tipo</p>
                   <asp:TextBox ID="txtTipo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Codigo_Portaria</p>
                   <asp:TextBox ID="txtCodigo_Portaria" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Codigo_Tributacao</p>
                   <asp:TextBox ID="txtCodigo_Tributacao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Codigo_departamento</p>
                   <asp:TextBox ID="txtCodigo_departamento" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Descricao_Familia</p>
                   <asp:TextBox ID="txtDescricao_Familia" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Margem</p>
                   <asp:TextBox ID="txtMargem" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Estoque_minimo</p>
                   <asp:TextBox ID="txtEstoque_minimo" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Peso_variavel</p>
                   <asp:TextBox ID="txtPeso_variavel" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Etiqueta</p>
                   <asp:TextBox ID="txtEtiqueta" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Validade</p>
                   <asp:TextBox ID="txtValidade" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Fator_conversao</p>
                   <asp:TextBox ID="txtFator_conversao" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Promocao</p>
                   <td><asp:CheckBox ID="chkPromocao" runat="server" Text="Promocao"/>
                   </td>

                                      <td >                   <p>Promocao_Automatica</p>
                   <td><asp:CheckBox ID="chkPromocao_Automatica" runat="server" Text="Promocao_Automatica"/>
                   </td>

                                      <td >                   <p>Data_inicio</p>
                   <asp:TextBox ID="txtData_inicio" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Data_fim</p>
                   <asp:TextBox ID="txtData_fim" runat="server" ></asp:TextBox>
                   </td>

                                      <td >                   <p>Descricao_Departamento</p>
                   <asp:TextBox ID="txtDescricao_Departamento" runat="server" ></asp:TextBox>
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

