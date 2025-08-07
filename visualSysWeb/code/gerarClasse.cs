using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.dao;



namespace visualSysWeb.code
{
    public static  class gerarClasse
    {
       static StringBuilder GridPesquisa = new StringBuilder();
        static int qtdCol = 0;
        static string nometabela ="";
        static StringBuilder atualizarDaoForm = new StringBuilder();
        static StringBuilder atualizarFormDao = new StringBuilder();
        static StringBuilder cabecalho = new StringBuilder();
        static StringBuilder propriedades = new StringBuilder();
        static StringBuilder construtor = new StringBuilder();
        static StringBuilder carregarDados = new StringBuilder();
        static StringBuilder update = new StringBuilder();
        static StringBuilder insert = new StringBuilder();
        static StringBuilder insertinto = new StringBuilder();
        static StringBuilder insertvalues = new StringBuilder();
        static bool primeiroCampo = true;

        static StringBuilder camposForm = new StringBuilder();




        public static void limpaStrings()
        {
             GridPesquisa.Clear(); 
             qtdCol = 0;
             nometabela ="";
             atualizarDaoForm.Clear();
             atualizarFormDao.Clear();

             cabecalho.Clear();
             propriedades.Clear();
             construtor.Clear();
             carregarDados.Clear();
             update.Clear();
             insert.Clear();
             insertinto.Clear();
             insertvalues.Clear();
             primeiroCampo = true;

             camposForm.Clear();
        }

        public static void gerar(String tabela,String pathsrv){
            limpaStrings();
                nometabela = tabela;
               
                String sql = "";
                string Colcoluna = "";
                string Coltipo = "";
                 //SQL SERVER
                  sql = "SELECT " +
                              "     COLUNAS.NAME AS COLUNA," +
                              "     TIPOS.NAME AS TIPO," +
                              "     COLUNAS.LENGTH AS TAMANHO," +
                              "     COLUNAS.ISNULLABLE AS EH_NULO" +
                              " FROM " +
                              "     SYSOBJECTS AS TABELAS," +
                              "     SYSCOLUMNS AS COLUNAS," +
                              "     SYSTYPES   AS TIPOS" +
                              " WHERE " +
                              "     TABELAS.ID = COLUNAS.ID" +
                              "     AND COLUNAS.USERTYPE = TIPOS.USERTYPE" +
                              "     AND TABELAS.NAME = '" + tabela + "'"+
                               " ORDER BY colorder ";
                  Colcoluna="COLUNA";
                  Coltipo="TIPO";
               
            //Cria Arquivo
                StreamWriter valor = new StreamWriter(pathsrv+"\\..\\dao\\gerado\\"+tabela + "DAO.cs", false, Encoding.ASCII);
            //Escreve no Arquivo  
                
                cabecalho.AppendLine("using System;");
                cabecalho.AppendLine("using System.Collections.Generic;");
                cabecalho.AppendLine("using System.Linq;");
                cabecalho.AppendLine("using System.Web;");
                cabecalho.AppendLine("using System.Data;");
                cabecalho.AppendLine("using System.Data.SqlClient;");
                cabecalho.AppendLine("");

                cabecalho.AppendLine("namespace visualSysWeb.dao");
                cabecalho.AppendLine("");

                cabecalho.AppendLine("{");
                cabecalho.AppendLine("  public class " + tabela + "DAO");
                cabecalho.AppendLine("   {");
                
                construtor.AppendLine("         public " + tabela + "DAO(){ }");
              
                construtor.AppendLine("         public " + tabela + "DAO(String campoIndex,User usr){ //colocar campo index da tabela");
                construtor.AppendLine("             String sql=\"Select * from  " + tabela + " where campoIndex =\"+ campoIndex  ;");
                construtor.AppendLine("             SqlDataReader rs = Conexao.consulta(sql, usr,true);");

                construtor.AppendLine("             carregarDados(rs);");
                
                construtor.AppendLine("         }");
                construtor.AppendLine(" ");


                carregarDados.AppendLine("          private String dataBr(DateTime dt) {");
                carregarDados.AppendLine("              if (dt.ToString(\"dd/MM/yyyy\").Equals(\"01/01/0001\")){");
                carregarDados.AppendLine("                  return \"\";");
                carregarDados.AppendLine("              }else{");
                carregarDados.AppendLine("                  return dt.ToString(\"dd/MM/yyyy\");");
                carregarDados.AppendLine("              }");
                carregarDados.AppendLine("          }");

                

                carregarDados.AppendLine("          public void carregarDados(SqlDataReader rs){");
                carregarDados.AppendLine("              if(rs.Read()){");

                update.AppendLine("        private void update(){");
                update.AppendLine("        try{");
                update.AppendLine("         String sql= \"update  " + tabela + " set \"+");
                

                insert.AppendLine(      "     private void insert(){");
                insert.AppendLine(      "         try{");
                insertinto.AppendLine(  "             String sql = \" insert into "+ tabela+" (\" +");
                insertvalues.AppendLine("                  \" )values( \"+");
                

  
                atualizarDaoForm.AppendLine(" //--Atualizar DaoForm ");
                atualizarDaoForm.AppendLine("      private void carregarDados()");
                atualizarDaoForm.AppendLine("      {");
                
            
                atualizarFormDao.AppendLine("// --Atualizar FormDao ");
                atualizarFormDao.AppendLine("     private void carregarDadosObj()");
                atualizarFormDao.AppendLine("     {");

                camposForm.AppendLine("/*--Campos Form");


              
                SqlDataReader rs =null;
                try
                {
                    
                    rs = Conexao.consulta(sql, new User(),true);
                    while (rs.Read()){
                        propriedades.AppendLine(    "      "+propriedade(rs[Colcoluna].ToString(),rs[Coltipo].ToString()));            
                          carregarDados.AppendLine( "        "+tipoCarregar(rs[Colcoluna].ToString(),tipoDados(rs[Coltipo].ToString())));
                          update.AppendLine(        "               " + updateTipo(rs[Colcoluna].ToString(), rs[Coltipo].ToString()));
                        insertinto.AppendLine(    "                       \""+ (!primeiroCampo?",":"")+rs[Colcoluna].ToString()+"\"+");
                          insertvalues.AppendLine(  "               " + insertTipo(rs[Colcoluna].ToString(), rs[Coltipo].ToString()));
                          atualizarDaoForm.AppendLine("                      " + daoForm(rs[Colcoluna].ToString(), rs[Coltipo].ToString()));
                          atualizarFormDao.AppendLine("                      " + formDao(rs[Colcoluna].ToString(), rs[Coltipo].ToString()));
                          camposForm.AppendLine("                   " + campos(rs[Colcoluna].ToString(), rs[Coltipo].ToString()));
                          GridPesquisa.AppendLine(campoGridpesquisa(rs[Colcoluna].ToString(),rs[Coltipo].ToString()));
                          primeiroCampo = false;

                    }
                    if (rs != null)
                        rs.Close();


                    insert.AppendLine(insertinto.ToString() +
                                    insertvalues.ToString() + "                      \");\";");
                    insert.AppendLine("              ");
                    insert.AppendLine("         Conexao.executarSql(sql);");
                    insert.AppendLine("         }catch (Exception err){");
                    insert.AppendLine("                 throw new Exception(\"nao foi possivel Inserir os valores erro:\"+err.Message );");
                    insert.AppendLine("         }");
                    insert.AppendLine("      }");
                    insert.AppendLine("    }");


                    carregarDados.AppendLine("              }");
                    carregarDados.AppendLine("              if(rs != null)");
                    carregarDados.AppendLine("                  rs.Close();");
                   
                    carregarDados.AppendLine("          }");
                    
                    update.AppendLine("             \"  where = \" //ARRUMAR CAMPO INDEX");   

                    update.AppendLine("                 ;");
                    update.AppendLine("             Conexao.executarSql(sql);");
                    
                    update.AppendLine("             }catch (Exception err){");
                    update.AppendLine("                 throw new Exception(\"nao foi possivel Atualizar os valores erro:\"+err.Message );");
                    update.AppendLine("             }");
                    update.AppendLine("         }");
                    update.AppendLine("         public bool salvar(bool novo) {");
                    update.AppendLine("                 if (novo)");
                    update.AppendLine("                 {");
                    update.AppendLine("                      insert();");
                    update.AppendLine("                  }");
                    update.AppendLine("                  else");
                    update.AppendLine("                  {");
                    update.AppendLine("                      update();");
                    update.AppendLine("                  }");
                    update.AppendLine("                  return true;");
                    update.AppendLine("         }");
                    update.AppendLine("         ");
                    update.AppendLine("         public bool excluir() {  ");
                    update.AppendLine("             String sql = \"delete from "+nometabela+"  where campoIndex= \"+ campoIndex ; //colocar campo index");
                    update.AppendLine("             Conexao.executarSql(sql);");
                    update.AppendLine("             return true;");
                    update.AppendLine("         }");
                    update.AppendLine("         ");
                    
                   
                    atualizarDaoForm.AppendLine("   }");
                    

                    atualizarFormDao.AppendLine("   }");
                   



                


                    //Grava no arquivo
                    valor.Write("");
                    valor.WriteLine(cabecalho.ToString()+
                                    propriedades.ToString()+
                                    construtor.ToString()+
                                    carregarDados.ToString()+
                                    update.ToString()+
                                    insert.ToString()+
                                    "}"+
                                    "/*"+
                                    
                                    metodosTelaPesquisa() +
                                    
                                    metodosPadroes()
                                    
                                    
                        );
                 

                }
                catch (Exception)
                {

                }
                finally {
                    valor.Close();
                    if (rs != null)
                    {
                        rs.Close();
                    }
                }
               

        }

        private static String tipoDados(String tipo) {
            switch (tipo)
            {
                case "varchar": case "nvarchar": case "char":
                    return "String";
                case "numeric":
                    return "Decimal";
                case "datetime":
                    return "DateTime";  
                case "tinyint":
                    return "bool";
                case "text":
                    return "String";
                case "int":
                    return "int";
                case "decimal":
                    return "Decimal";
                case "double":
                    return "Decimal";
                case "float":
                    return "Decimal";
                default:
                    return "--------------->TIPO NÃO ENCONTRADO!!!!!!!!!!!!!!!!!!!!!";
                
            }
        

        }

        private static String tipoCarregar(String campo,String tipo){
            switch (tipo)
            {
                case "String":
                case "nvarchar":
                case "char":
                    return "      " + campo + " = rs[\"" + campo + "\"].ToString();";
                case "Decimal":
                    return "      " + campo + " = (Decimal)(rs[\"" + campo + "\"].ToString().Equals(\"\")? new Decimal():rs[\"" + campo + "\"]);";
                    
                case "DateTime":
                    return "      " + campo + " = (rs[\"" + campo + "\"].ToString().Equals(\"\")? new DateTime():DateTime.Parse(rs[\"" + campo + "\"].ToString()));";
                    
                case "bool":
                    return "      " + campo + " = (rs[\"" + campo + "\"].ToString().Equals(\"1\")?true:false);";
                    
                case "int":
                    return "      " + campo + " = (rs[\"" + campo + "\"]==null?0:int.Parse(rs[\"" + campo + "\"].ToString()));";
                    
                default:
                    return "" + campo + "--------------->TIPO NÃO ENCONTRADO ";
                    


            }
        }

        private static String updateTipo(String campo, String tipo) {
            String resultado = "        \"";

            if (!primeiroCampo)
            {
                resultado += ",";
            }
            switch (tipo)
            {
                case "varchar":
                case "nvarchar":
                case "char":
                    resultado += campo+"='\"+"+campo+"+\"'\"+";
                    break;
                case "numeric":
                    resultado +=campo+"=\"+"+campo+".ToString().Replace(\",\", \".\")+";
                    break;
                case "datetime":
                    resultado += campo + "=\"+(" + campo + ".ToString(\"yyyy-MM-dd\").Equals(\"0001-01-01\") ? \"null\" : \"'\" + " + campo + ".ToString(\"yyyy-MM-dd\") + \"'\")+\",\"+";
                    break;
                case "tinyint":
                    resultado +=  campo + "=\"+(" + campo + "?\"1\":\"0\")+";
                    break;
                case "text":
                    resultado +=  campo + "='\"+" + campo + "+\"'\"+";
                    break;
                case "int":
                    resultado +=  campo + "=\"+" + campo + "+";
                    break;
                case "decimal":
                    resultado += campo + "=\"+" + campo + ".ToString().Replace(\",\", \".\")+";
                    break;
                case "double":
                resultado += campo + "=\"+" + campo + ".ToString().Replace(\",\", \".\")+";
                    break;
                case "float":
                    resultado += campo + "=\"+" + campo + ".ToString().Replace(\",\", \".\")+";
                    break;
                default:
                    return "--------------->TIPO NÃO ENCONTRADO!!!!!!!!!!!!!!!!!!!!!";
                    
                    
            }
            return resultado;
        }
        private static String insertTipo(String campo, String tipo)
        {
            String resultado = "        ";

            if (!primeiroCampo)
            {
                resultado += "\",\"+";
            }
            switch (tipo)
            {
                case "varchar":
                case "nvarchar":
                case "char":
                    resultado += "\"'\"+" + campo + "+\"'\"+";
                    break;
                case "numeric":
                    resultado += campo + ".ToString().Replace(\",\",\".\")+";
                    break;
                case "datetime":
                    resultado +=  "("+campo + ".ToString(\"yyyy-MM-dd\").Equals(\"0001-01-01\")?\"null\": \"'\"+" + campo + ".ToString(\"yyyy-MM-dd\")+\"'\")+";
                    break;
                case "tinyint":
                    resultado += "(" + campo + "?1:0)+";
                    break;
                case "text":
                    resultado += "\"'\"+" + campo + "+\"'\"+";
                    break;
                case "int":
                    resultado +=  campo + "+";
                    break;
                case "decimal":
                    resultado +=  campo + ".ToString().Replace(\",\",\".\")+";
                    break;
                case "double":
                    resultado += campo + ".ToString().Replace(\",\",\".\")+";
                    break;
                case "float":
                    resultado += campo + ".ToString().Replace(\",\",\".\")+";
                    break;

                default:
                    return "--------------->TIPO NÃO ENCONTRADO!!!!!!!!!!!!!!!!!!!!!";

            }
            return resultado;
        }

        private static String propriedade(String campo, String tipo)
        {


            String strDefault = "";

            switch (tipo)
            {
                case "double":
                case "decimal":
                case "numeric":
                case "int":
                case "float":
                
                    strDefault = "0";
                    break;
                case "tinyint":
                    strDefault = "false";
                    break;
                case "datetime":
                    strDefault = " new DateTime()";
                    break;
                default:
                    strDefault = "\"\"";
                    break;
            }

            StringBuilder propri = new StringBuilder();

            propri.Append ("        public " + tipoDados(tipo) + " " +campo + " ="+strDefault+";");
            if (tipo.Equals("datetime") ){
                propri.AppendLine("");
                propri.AppendLine("                 public String " + campo + "Br() ");
                propri.AppendLine("                 {");
                propri.AppendLine("                     return dataBr("+campo+");");
                propri.AppendLine("                  }");
            }
            
            return propri.ToString();
        }

        private static String campos(String campo, String tipo) { 
                StringBuilder cp = new StringBuilder();
                       cp.Append("                   <td >");
                       cp.AppendLine("                   <p>" + campo + "</p>");
                       switch (tipo)
                       {
                           case "double":
                               cp.AppendLine("                   <asp:TextBox ID=\"txt" + campo + "\" runat=\"server\" CssClass=\"numero\" ></asp:TextBox>");
                               break;
                           case "decimal":
                               cp.AppendLine("                   <asp:TextBox ID=\"txt" + campo + "\" runat=\"server\" CssClass=\"numero\" ></asp:TextBox>");
                               break;
                           
                           case "numeric":
                               cp.AppendLine("                   <asp:TextBox ID=\"txt" + campo + "\" runat=\"server\"  CssClass=\"numero\" ></asp:TextBox>");
                               break;
                           case "int":
                               cp.AppendLine("                   <asp:TextBox ID=\"txt" + campo + "\" runat=\"server\"  CssClass=\"numero\" ></asp:TextBox>");
                               break;

                           case "tinyint":
                               cp.AppendLine("                   <td><asp:CheckBox ID=\"chk" + campo + "\" runat=\"server\" Text=\"" + campo + "\"/>");
                               break;
                           case "float":
                               cp.AppendLine("                   <td><asp:CheckBox ID=\"chk" + campo + "\" runat=\"server\" Text=\"" + campo + "\"/>");
                               break;
                           default:
                               cp.AppendLine("                   <asp:TextBox ID=\"txt" + campo + "\" runat=\"server\" ></asp:TextBox>");
                                    break;
                       }


                       cp.AppendLine("                   </td>");
                       return cp.ToString();
        
        }
        private static String daoForm(String campo, String tipo){
                
                 switch (tipo)
                {
                    case "varchar":
                    case "nvarchar":
                    case "char":
                        return "                   txt"+campo+".Text=obj."+campo+".ToString();";
                    
                     
                    case "datetime":
                        return "                   txt" + campo + ".Text=obj." + campo + "Br();";

                    case "tinyint":
                        return "                   chk" + campo + ".Checked =obj." + campo + ";";
                    case "text":
                        return "                   txt" + campo + ".Text=obj." + campo + ".ToString();";
                    case "int":
                        return "                   txt" + campo + ".Text=obj." + campo + ".ToString();";
                    case "decimal":
                    case "double":
                    case "float":
                    case "numeric":
                    
                        return "                   txt" + campo + ".Text=obj." + campo + ".ToString(\"N2\");";
                    
                     default:
                        return "--------------->TIPO NÃO ENCONTRADO!!!!!!!!!!!!!!!!!!!!!";

                 }
        }
        //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
        private static String formDao(String campo,String tipo){

            switch (tipo)
            {
                case "varchar":
                case "nvarchar":
                case "char":
                    return "                   obj." + campo + "=txt" + campo + ".Text;";
                case "decimal":
                case "double":
                case "float":
                case "numeric":
                    return "                   Decimal.TryParce(txt" + campo + ".Text , out obj." + campo + ");";
                case "datetime":
                    return "                   obj." + campo + "=(txt" + campo + ".Text.Equals(\"\")?new DateTime():DateTime.Parse(txt" + campo + ".Text));";

                case "tinyint":
                    return "                   obj." + campo + "=chk" + campo + ".Checked ;";
                case "text":
                    return "                   obj." + campo + "=txt" + campo + ".Text;";
                case "int":
                    return "                   obj." + campo + "=int.Parse(txt" + campo + ".Text);";
               
                default:
                    return "--------------->TIPO NÃO ENCONTRADO!!!!!!!!!!!!!!!!!!!!!";
                    
            }
            
        }
        private static String metodosPadroes()
        {
            StringBuilder metodosPadroes = new StringBuilder() ;
            metodosPadroes.AppendLine("/*================================Metodos tela detalhes==========================================");
            metodosPadroes.AppendLine("using System.Data; ");
            metodosPadroes.AppendLine("using visualSysWeb.dao;");
            metodosPadroes.AppendLine("using System.Data.SqlClient;");
            metodosPadroes.AppendLine("                 : visualSysWeb.code.PagePadrao");
            metodosPadroes.AppendLine("  {");
            metodosPadroes.AppendLine("                 protected static " + nometabela + "DAO obj= null ;");
            metodosPadroes.AppendLine("                 static String camporeceber = \"\";");
            metodosPadroes.AppendLine("                 protected void Page_Load(object sender, EventArgs e)     ");
            metodosPadroes.AppendLine("                 {");
            metodosPadroes.AppendLine("                      User usr = (User)Session[\"User\"];");
            metodosPadroes.AppendLine("                      obj = new "+nometabela+"DAO();");
            metodosPadroes.AppendLine("                      tabMenu.Items[MultiView1.ActiveViewIndex].Selected = true;");
            metodosPadroes.AppendLine("                      if (Request.Params[\"novo\"] != null) ");
            metodosPadroes.AppendLine("                      {");
            metodosPadroes.AppendLine("                        status = \"incluir\";");
            metodosPadroes.AppendLine("                                         EnabledControls(conteudo, true);");
            metodosPadroes.AppendLine("                                         EnabledControls(cabecalho, true);");
            metodosPadroes.AppendLine("                      }");
            metodosPadroes.AppendLine("                      else");
            metodosPadroes.AppendLine("                      {");
            metodosPadroes.AppendLine("                           if (Request.Params[\"campoIndex\"] != null)  // colocar o campo index da tabela");
            metodosPadroes.AppendLine("                           {");
            metodosPadroes.AppendLine("                              try");
            metodosPadroes.AppendLine("                              {");
            metodosPadroes.AppendLine("                                   if (!IsPostBack)");
            metodosPadroes.AppendLine("                                   {");
            metodosPadroes.AppendLine("                                        String index = Request.Params[\"campoIndex\"].ToString();// colocar o campo index da tabela");
            metodosPadroes.AppendLine("                                        status = \"visualizar\";");
            metodosPadroes.AppendLine("                                        obj = new "+nometabela+"DAO(index,usr);");
            metodosPadroes.AppendLine("                                        carregarDados();");
            metodosPadroes.AppendLine("                                    }");
            metodosPadroes.AppendLine("                                    if (status.Equals(\"visualizar\"))");
            metodosPadroes.AppendLine("                                    {");
            metodosPadroes.AppendLine("                                         EnabledControls(conteudo, false);");
            metodosPadroes.AppendLine("                                         EnabledControls(cabecalho, false);");
            metodosPadroes.AppendLine("                                    }else{");
            metodosPadroes.AppendLine("                                         EnabledControls(conteudo, true);");
            metodosPadroes.AppendLine("                                         EnabledControls(cabecalho, true);");
            metodosPadroes.AppendLine("                                    }");
            metodosPadroes.AppendLine("                                }");
            metodosPadroes.AppendLine("                                catch (Exception err)");
            metodosPadroes.AppendLine("                                {");
            metodosPadroes.AppendLine("                                   lblError.Text = err.Message;                 ");
            metodosPadroes.AppendLine("                                }");
            metodosPadroes.AppendLine("                           }");
            metodosPadroes.AppendLine("                       }");
            metodosPadroes.AppendLine("                    carregabtn(pnBtn);");
            metodosPadroes.AppendLine("                  }");
            metodosPadroes.AppendLine("                 ");
            metodosPadroes.AppendLine("                 private void limparCampos(){");
            metodosPadroes.AppendLine("                    LimparCampos(cabecalho);          ");
            metodosPadroes.AppendLine("                    LimparCampos(conteudo);             ");
            metodosPadroes.AppendLine("                 }");
            metodosPadroes.AppendLine("                 ");
            metodosPadroes.AppendLine("                 protected bool validaCamposObrigatorios() {");
            metodosPadroes.AppendLine("                    if (validaCampos(cabecalho) && validaCampos(conteudo))");
            metodosPadroes.AppendLine("                             return true;");
            metodosPadroes.AppendLine("                    else");
            metodosPadroes.AppendLine("                             return false;");
            metodosPadroes.AppendLine("                 }");
            metodosPadroes.AppendLine("                 ");
            metodosPadroes.AppendLine("                 protected override bool campoObrigatorio(Control campo)");
            metodosPadroes.AppendLine("                 {// colocar os nomes dos campos obrigarios no Array");
            metodosPadroes.AppendLine("                     String[] campos = { \"\", ");
            metodosPadroes.AppendLine("                                    \"\", ");
            metodosPadroes.AppendLine("                                    \"\", ");
            metodosPadroes.AppendLine("                                    \"\" ");
            metodosPadroes.AppendLine("                                     };");
            metodosPadroes.AppendLine("                       return existeNoArray(campos, campo.ID+\"\");");
            metodosPadroes.AppendLine("                 }");
            metodosPadroes.AppendLine("                 ");
            metodosPadroes.AppendLine("                 protected override bool campoDesabilitado(Control campo)");
            metodosPadroes.AppendLine("                 {// colocar os nomes dos campos Desabilitados no Array");
            metodosPadroes.AppendLine("                     String[] campos = { \"\", ");
            metodosPadroes.AppendLine("                                    \"\", ");
            metodosPadroes.AppendLine("                                    \"\", ");
            metodosPadroes.AppendLine("                                    \"\" ");
            metodosPadroes.AppendLine("                                     };");
            metodosPadroes.AppendLine("                       return existeNoArray(campos, campo.ID+\"\");"); 
            metodosPadroes.AppendLine("                 }");
            metodosPadroes.AppendLine("                 protected override void btnIncluir_Click(object sender, EventArgs e)");
            metodosPadroes.AppendLine("                 {");
            metodosPadroes.AppendLine("                    incluir(pnBtn);");
            metodosPadroes.AppendLine("                 }");
            metodosPadroes.AppendLine("                 ");
            metodosPadroes.AppendLine("                 protected override void btnEditar_Click(object sender, EventArgs e)");
            metodosPadroes.AppendLine("                 {");
            metodosPadroes.AppendLine("                    editar(pnBtn);");
            metodosPadroes.AppendLine("                    EnabledControls(cabecalho, true);");
            metodosPadroes.AppendLine("                    EnabledControls(conteudo, true);");
            metodosPadroes.AppendLine("                 }");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                 protected override void btnPesquisar_Click(object sender, EventArgs e)");
            metodosPadroes.AppendLine("                 {");
            metodosPadroes.AppendLine("                 Response.Redirect(\"nomepaginapesquisa.aspx\"); //colocar o endereco da tela de pesquisa");
            metodosPadroes.AppendLine("                 }");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                 protected override void btnExcluir_Click(object sender, EventArgs e)");
            metodosPadroes.AppendLine("                 {");
            metodosPadroes.AppendLine("                     pnConfima.Visible = true;");
            metodosPadroes.AppendLine("                  }");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  protected override void btnConfirmar_Click(object sender, EventArgs e)");
            metodosPadroes.AppendLine("                  {");
            metodosPadroes.AppendLine("                     try");
            metodosPadroes.AppendLine("                     {");
            metodosPadroes.AppendLine("                       if (validaCamposObrigatorios())");
            metodosPadroes.AppendLine("                       {");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                             carregarDadosObj();");
            metodosPadroes.AppendLine("                             obj.salvar(status.Equals(\"incluir\")); // se for incluir true se não falso;");
            metodosPadroes.AppendLine("                             lblError.Text = \"Salvo com Sucesso\";");
            metodosPadroes.AppendLine("                             lblError.ForeColor = System.Drawing.Color.Blue;");

            metodosPadroes.AppendLine("                             EnabledControls(cabecalho, false);");
            metodosPadroes.AppendLine("                             EnabledControls(conteudo, false);");
            metodosPadroes.AppendLine("                             visualizar(pnBtn);");
            metodosPadroes.AppendLine("                       }");
            metodosPadroes.AppendLine("                       else");
            metodosPadroes.AppendLine("                       {");
            metodosPadroes.AppendLine("                            lblError.Text = \"Campo Obrigatorio não preenchido\";");
            metodosPadroes.AppendLine("                            lblError.ForeColor = System.Drawing.Color.Red;");
            metodosPadroes.AppendLine("                        }");
            metodosPadroes.AppendLine("                     }");
            metodosPadroes.AppendLine("                     catch (Exception err)");
            metodosPadroes.AppendLine("                     {");
            metodosPadroes.AppendLine("                         lblError.Text = err.Message;");
            metodosPadroes.AppendLine("                         lblError.ForeColor = System.Drawing.Color.Red;");
            metodosPadroes.AppendLine("                     }");
            metodosPadroes.AppendLine("                  }");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  protected override void btnCancelar_Click(object sender, EventArgs e)");
            metodosPadroes.AppendLine("                  {");
            metodosPadroes.AppendLine("                      Response.Redirect(\"nomepaginapesquisa.aspx\");//colocar endereco pagina de pesquisa");
            metodosPadroes.AppendLine("                  }");
            metodosPadroes.AppendLine("                   "+atualizarDaoForm.ToString());
            metodosPadroes.AppendLine("                   "+atualizarFormDao.ToString());
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  protected void lista_click(object sender, ImageClickEventArgs e)");
            metodosPadroes.AppendLine("                  {");
            metodosPadroes.AppendLine("                      ImageButton btn = (ImageButton)sender;");
            metodosPadroes.AppendLine("                      pnFundo.Visible = true;");
            metodosPadroes.AppendLine("                      chkLista.Items.Clear();");
            metodosPadroes.AppendLine("                      String sqlLista = \"\";");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                      switch (btn.ID)");
            metodosPadroes.AppendLine("                      {");
            metodosPadroes.AppendLine("                          case \"idBotao\":");
            metodosPadroes.AppendLine("                              sqlLista = \"Query de pesquisa com no minimo 2campos\";");
            metodosPadroes.AppendLine("                              lbllista.Text = \"Pagamentos\";");
            metodosPadroes.AppendLine("                              camporeceber = \"txtPagamento\";");
            metodosPadroes.AppendLine("                              break;");
            metodosPadroes.AppendLine("                      }");
            metodosPadroes.AppendLine("                      User usr = (User)Session[\"User\"];");
            metodosPadroes.AppendLine("                      SqlDataReader lista = Conexao.consulta(sqlLista, usr);");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                      while (lista.Read())");
            metodosPadroes.AppendLine("                      {");
            metodosPadroes.AppendLine("                          ListItem item = new ListItem();");
            metodosPadroes.AppendLine("                          item.Value = lista[0].ToString();");
            metodosPadroes.AppendLine("                          item.Text = lista[1].ToString();");
            metodosPadroes.AppendLine("                          chkLista.Items.Add(item);");
            metodosPadroes.AppendLine("                       }");
            metodosPadroes.AppendLine("                       if (lista != null)");
            metodosPadroes.AppendLine("                          lista.Close();");
            metodosPadroes.AppendLine("                  }");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  protected void btnConfirmaExclusao_Click(object sender, ImageClickEventArgs e)");
            metodosPadroes.AppendLine("                  {");
            metodosPadroes.AppendLine("                      try");
            metodosPadroes.AppendLine("                      {");
            metodosPadroes.AppendLine("                          obj.excluir();");
            metodosPadroes.AppendLine("                          pnConfima.Visible = false;");
            metodosPadroes.AppendLine("                          lblError.Text = \"Registro Excluido com sucesso\";");
            metodosPadroes.AppendLine("                          limparCampos();");
            metodosPadroes.AppendLine("                          pesquisar(pnBtn);");
            metodosPadroes.AppendLine("                       }");
            metodosPadroes.AppendLine("                       catch (Exception err)");
            metodosPadroes.AppendLine("                        {");
            metodosPadroes.AppendLine("                               lblError.Text = \"Não foi possivel Excluir o registro error:\" +err.Message;");
            metodosPadroes.AppendLine("                         }");
            metodosPadroes.AppendLine("                  }");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  protected void btnCancelaExclusao_Click(object sender, ImageClickEventArgs e)");
            metodosPadroes.AppendLine("                  {");
            metodosPadroes.AppendLine("                      pnConfima.Visible = false;");
            metodosPadroes.AppendLine("                  }");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  protected void btnConfirmaLista_Click(object sender, ImageClickEventArgs e)");
            metodosPadroes.AppendLine("                  {");
            metodosPadroes.AppendLine("                      TextBox txt = (TextBox)conteudo.FindControl(camporeceber);");
            metodosPadroes.AppendLine("                      txt.Text = \"\";");
            metodosPadroes.AppendLine("                      for (int i = 0; i < chkLista.Items.Count; i++)");
            metodosPadroes.AppendLine("                      {");
            metodosPadroes.AppendLine("                          if (chkLista.Items[i].Selected)");
            metodosPadroes.AppendLine("                          {");
            metodosPadroes.AppendLine("                              txt.Text += chkLista.Items[i].Value;");
            metodosPadroes.AppendLine("                         }");
            metodosPadroes.AppendLine("                     }");
            metodosPadroes.AppendLine("                     pnFundo.Visible = false;");
            metodosPadroes.AppendLine("                  }");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  protected void btnCancelaLista_Click(object sender, ImageClickEventArgs e)");
            metodosPadroes.AppendLine("                  {");
            metodosPadroes.AppendLine("                      pnFundo.Visible = false;");
            metodosPadroes.AppendLine("                  }");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("/*================================HTML Pagina Detalhes==========================================");
            metodosPadroes.AppendLine("<div class=\"cabMenu\">                  ");
            metodosPadroes.AppendLine("       <center> <h1>Detalhes do "+nometabela+"</h1></center>                  ");
            metodosPadroes.AppendLine("</div>                  ");
            metodosPadroes.AppendLine("    <asp:Panel ID=\"pnBtn\" runat=\"server\" CssClass=\"cabMenu\">                  ");
            metodosPadroes.AppendLine("    </asp:Panel>                  ");
            metodosPadroes.AppendLine("    <br />              ");
            metodosPadroes.AppendLine("     <asp:Label ID=\"lblError\" runat=\"server\" Text=\"\" ForeColor=Red></asp:Label>              ");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("    <div id=\"cabecalho\" runat=\"server\" class=\"frame\" >               ");
            metodosPadroes.AppendLine("     <!--Coloque aqui os campos do cabeçalho    -->         ");
            metodosPadroes.AppendLine("        <table>              ");
            metodosPadroes.AppendLine("              <tr>    ");
            metodosPadroes.AppendLine("                  <td></td>");
            metodosPadroes.AppendLine("              </tr>    ");
            metodosPadroes.AppendLine("        </table>          ");
            metodosPadroes.AppendLine("    </div>              ");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("<div id=\"conteudo\" runat=\"server\" class=\"conteudo\" >                  ");
            metodosPadroes.AppendLine("           <table>              ");
            metodosPadroes.AppendLine("                <tr>    ");
            metodosPadroes.AppendLine(camposForm.ToString() );
            metodosPadroes.AppendLine("                </tr>    ");
            metodosPadroes.AppendLine("           </table>          ");
            metodosPadroes.AppendLine("</div>                  ");

            metodosPadroes.AppendLine("        <asp:Panel ID=\"pnFundo\" runat=\"server\" CssClass=\"fundo\" Visible =false>          ");
            metodosPadroes.AppendLine("              <asp:Label ID=\"lbllista\" runat=\"server\" Text=\"Label\" CssClass=\"cabMenu\"></asp:Label>           ");
            metodosPadroes.AppendLine("                    <table class=\"frame\">");
            metodosPadroes.AppendLine("                       <tr>");
            metodosPadroes.AppendLine("                           <td>          ");
            metodosPadroes.AppendLine("                             <asp:ImageButton ID=\"btnConfirmaLista\" runat=\"server\" ImageUrl=\"~/img/confirm.png\"                    <td>       ");
            metodosPadroes.AppendLine("                              Width=\"25px\" onclick=\"btnConfirmaLista_Click\"   />           ");
            metodosPadroes.AppendLine("                              <asp:Label ID=\"Label4\" runat=\"server\" Text=\"Seleciona\" ></asp:Label>          ");
            metodosPadroes.AppendLine("                           </td>           ");
            metodosPadroes.AppendLine("                           <td>          ");
            metodosPadroes.AppendLine("                                    <asp:ImageButton ID=\"btnCancelaLista\" runat=\"server\" ImageUrl=\"~/img/cancel.png\" ");
            metodosPadroes.AppendLine("                                       Width=\"25px\" onclick=\"btnCancelaLista_Click\"  />                   ");
            metodosPadroes.AppendLine("                                    <asp:Label ID=\"Label5\" runat=\"server\" Text=\"Cancela\" ></asp:Label>     ");
            metodosPadroes.AppendLine("                           </td>           ");
            metodosPadroes.AppendLine("                       </tr>");
            metodosPadroes.AppendLine("                     </table>");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                      <asp:Panel ID=\"Panel1\" runat=\"server\" CssClass=\"lista\" >   ");
            metodosPadroes.AppendLine("                             <asp:RadioButtonList ID=\"chkLista\" runat=\"server\" Height=50 Width=200>");
            metodosPadroes.AppendLine("                             </asp:RadioButtonList>");
            metodosPadroes.AppendLine("                      </asp:Panel>");
            metodosPadroes.AppendLine("         </asp:Panel>      ");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("         <asp:Panel ID=\"pnConfima\" runat=\"server\" CssClass=\"fundo\" Visible =false>         ");
            metodosPadroes.AppendLine("           <asp:Label ID=\"Label1\" runat=\"server\" Text=\"Confirma Exclusão\" CssClass=\"cabMenu\"></asp:Label>         ");
            metodosPadroes.AppendLine("             <table class=\"frame\">          ");
            metodosPadroes.AppendLine("                  <tr>     ");
            metodosPadroes.AppendLine("                      <td>             ");
            metodosPadroes.AppendLine("                             <asp:ImageButton ID=\"btnConfirmaExclusao\" runat=\"server\" ImageUrl=\"~/img/confirm.png\" ");
            metodosPadroes.AppendLine("                                     Width=\"25px\" onclick=\"btnConfirmaExclusao_Click\"  /> ");
            metodosPadroes.AppendLine("                                     <asp:Label ID=\"Label2\" runat=\"server\" Text=\"Confirma\" ></asp:Label>");
            metodosPadroes.AppendLine("                      </td>");
            metodosPadroes.AppendLine("                      <td>");
            metodosPadroes.AppendLine("                                    <asp:ImageButton ID=\"btnCancelaExclusao\" runat=\"server\" ImageUrl=\"~/img/cancel.png\" ");
            metodosPadroes.AppendLine("                                     Width=\"25px\" onclick=\"btnCancelaExclusao_Click\"  /> ");
            metodosPadroes.AppendLine("                                     <asp:Label ID=\"Label3\" runat=\"server\" Text=\"Cancela\" ></asp:Label>");
            metodosPadroes.AppendLine("                      </td>");
            metodosPadroes.AppendLine("                  </tr>");
            metodosPadroes.AppendLine("              </table>     ");
            metodosPadroes.AppendLine("         </asp:Panel>         ");


            metodosPadroes.AppendLine("*/ ");
            
            
            
            
            
            return metodosPadroes.ToString();
        }

        private static String metodosTelaPesquisa()
        {
            StringBuilder metodosPadroes = new StringBuilder();
            metodosPadroes.AppendLine(" ");
            metodosPadroes.AppendLine("/*================================Metodos tela de Pesquisa==========================================");
            metodosPadroes.AppendLine("using System.Data; ");
            metodosPadroes.AppendLine("using visualSysWeb.dao;");
            metodosPadroes.AppendLine("           :visualSysWeb.code.PagePadrao     //inicio da classe ");
            metodosPadroes.AppendLine("{ ");
            metodosPadroes.AppendLine("                  static DataTable tb;");
            metodosPadroes.AppendLine("                  static String sqlGrid = \"\"select * from " + nometabela + "\";//colocar os campos no select que ser?o apresentados na tela");
            metodosPadroes.AppendLine("                  protected void Page_Load(object sender, EventArgs e)");
            metodosPadroes.AppendLine("                  {");
            metodosPadroes.AppendLine("                     if (!IsPostBack)");
            metodosPadroes.AppendLine("                     {   ");
            metodosPadroes.AppendLine("                       User usr = (User)Session[\"User\"];");
            metodosPadroes.AppendLine("                       tb = Conexao.GetTable(sqlGrid ,usr); ");
            metodosPadroes.AppendLine("                       gridPesquisa.DataSource = tb;");
            metodosPadroes.AppendLine("                       gridPesquisa.DataBind();");
            metodosPadroes.AppendLine("                      }");
            metodosPadroes.AppendLine("                      pesquisar(pnBtn);");
            metodosPadroes.AppendLine("                  }");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  protected override void btnIncluir_Click(object sender, EventArgs e)");
            metodosPadroes.AppendLine("                  {");
            metodosPadroes.AppendLine("                      Response.Redirect(\"~/modulos/nome Do Modulo/pages/ "+nometabela+"Detalhes.aspx?novo=true\"); // colocar caminho da pagina de Detalhes verificar Case sensitive");
            metodosPadroes.AppendLine("                  }");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  protected override void btnPesquisar_Click(object sender, EventArgs e)");
            metodosPadroes.AppendLine("                  {");
            metodosPadroes.AppendLine("                      String sql = \"\";");
            metodosPadroes.AppendLine("                      if (!txtPESQ1.Text.Equals(\"\")) //colocar nome do campo de pesquisa");
            metodosPadroes.AppendLine("                      {");
            metodosPadroes.AppendLine("                          sql = \" campoPesquisa1 like '\" + txtPESQ1.Text + \"%'\"; // preencher com os campos que serão apresentados na grid e o campo que será feito a pesquisa");
            metodosPadroes.AppendLine("                      }");
            metodosPadroes.AppendLine("                      if (!txtPESQ2.Text.Equals(\"\")) //colocar nome do campo de pesquisa2");
            metodosPadroes.AppendLine("                      {");
            metodosPadroes.AppendLine("                          if (!sql.Equals(\"\"))");
            metodosPadroes.AppendLine("                          {");
            metodosPadroes.AppendLine("                              sql += \" and \";     ");
            metodosPadroes.AppendLine("                          }");
            metodosPadroes.AppendLine("                         sql += \"campoPesquisa2 = '\" + txtPESQ2.Text + \"'\";//preencher com os campos que serão apresentados na grid e o campo que será feito a pesquisa ");
            metodosPadroes.AppendLine("                      }");
            metodosPadroes.AppendLine("                         try");
            metodosPadroes.AppendLine("                         {");
            metodosPadroes.AppendLine("                            User usr = (User)Session[\"User\"];");
            metodosPadroes.AppendLine("                            if (!sql.Equals(\"\"))");
            metodosPadroes.AppendLine("                            {");
            metodosPadroes.AppendLine("                               tb = Conexao.GetTable(sqlGrid+\" where \"+sql, usr);");
            metodosPadroes.AppendLine("                             }");
            metodosPadroes.AppendLine("                             else");
            metodosPadroes.AppendLine("                             {");
            metodosPadroes.AppendLine("                               tb = Conexao.GetTable(sqlGrid, usr);");
            metodosPadroes.AppendLine("                              }");
            metodosPadroes.AppendLine("                               gridPesquisa.DataSource = tb;");
            metodosPadroes.AppendLine("                               gridPesquisa.DataBind();");
            metodosPadroes.AppendLine("                               lblPesquisaErro.Text = \"\";");
            metodosPadroes.AppendLine("                        }catch (Exception err)");
            metodosPadroes.AppendLine("                         {");
            metodosPadroes.AppendLine("                                      lblPesquisaErro.Text = err.Message;");
            metodosPadroes.AppendLine("                         }");
            
            
            metodosPadroes.AppendLine("                  }");
            metodosPadroes.AppendLine("                  protected override void btnEditar_Click(object sender, EventArgs e){}");
            metodosPadroes.AppendLine("                  protected override void btnExcluir_Click(object sender, EventArgs e) {}");
            metodosPadroes.AppendLine("                  protected override void btnConfirmar_Click(object sender, EventArgs e){}");
            metodosPadroes.AppendLine("                  protected override void btnCancelar_Click(object sender, EventArgs e){}   ");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                 protected override bool campoObrigatorio(Control campo)");
            metodosPadroes.AppendLine("                 { ");
            metodosPadroes.AppendLine("                       return false;");
            metodosPadroes.AppendLine("                 }");
            metodosPadroes.AppendLine("                 ");
            metodosPadroes.AppendLine("                 protected override bool campoDesabilitado(Control campo)");
            metodosPadroes.AppendLine("                 {");
            metodosPadroes.AppendLine("                       return false;");
            metodosPadroes.AppendLine("                 }");
            metodosPadroes.AppendLine("                 ");
            metodosPadroes.AppendLine("*/ ");

            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("/*================================html tela de Pesquisa==========================================");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("   <center><h1>"+nometabela+"</h1></center>");
            metodosPadroes.AppendLine("    <hr />              "); 
            metodosPadroes.AppendLine("       <asp:Panel ID=\"pnBtn\" runat=\"server\" CssClass=\"cabMenu\">               ");
            metodosPadroes.AppendLine("       </asp:Panel>           ");
            metodosPadroes.AppendLine("       <br />           ");
            metodosPadroes.AppendLine("       <div class=\"filter\" id=\"filtrosPesq\" runat=\"server\" visible=\"false\">           ");
            metodosPadroes.AppendLine("         <table>           ");
            metodosPadroes.AppendLine("           <asp:Label ID=\"lblPesquisaErro\" runat=\"server\" Text=\"\" ForeColor=\"Red\"></asp:Label>           ");
            metodosPadroes.AppendLine("            <tr>           ");
            metodosPadroes.AppendLine("                <td>           ");
            metodosPadroes.AppendLine("                <p>CAMPO DE PESQUISA 1</p>   ");
            metodosPadroes.AppendLine("                <asp:TextBox ID=\"txtPESQ1\" runat=\"server\" ></asp:TextBox></asp:TextBox>  ");
            metodosPadroes.AppendLine("                </td>  ");
            metodosPadroes.AppendLine("                <td>  ");
            metodosPadroes.AppendLine("                   <p>CAMPO DE PESQUISA 2</p>  ");
            metodosPadroes.AppendLine("                   <asp:TextBox ID=\"txtPESQ2\" runat=\"server\" > </asp:TextBox>");
            metodosPadroes.AppendLine("                </td>  ");
            metodosPadroes.AppendLine("            </tr>      ");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("                  ");
            metodosPadroes.AppendLine("         </table>           ");
            metodosPadroes.AppendLine("        </div>            ");
            metodosPadroes.AppendLine("        <div class=\"gridTable\">          ");
            metodosPadroes.AppendLine("            <asp:GridView ID=\"gridPesquisa\" runat=\"server\" AutoGenerateColumns=\"False\"           ");
            metodosPadroes.AppendLine("                 ForeColor=\"#333333\" GridLines=\"None\"  ");
            metodosPadroes.AppendLine("                 > ");
            metodosPadroes.AppendLine("                 <AlternatingRowStyle BackColor=\"White\" ForeColor=\"#284775\" /> ");
            metodosPadroes.AppendLine("                 <Columns> ");
            metodosPadroes.AppendLine(GridPesquisa.ToString());
            metodosPadroes.AppendLine("                 </Columns> ");
            metodosPadroes.AppendLine("                 <EditRowStyle BackColor=\"#999999\" /> ");
            metodosPadroes.AppendLine("                 <FooterStyle BackColor=\"#5D7B9D\" Font-Bold=\"True\" ForeColor=\"White\" /> ");
            metodosPadroes.AppendLine("                 <HeaderStyle BackColor=\"#5D7B9D\" Font-Bold=\"True\" ForeColor=\"White\" /> ");
            metodosPadroes.AppendLine("                 <PagerStyle BackColor=\"#284775\" ForeColor=\"White\" HorizontalAlign=\"Center\" /> ");
            metodosPadroes.AppendLine("                 <RowStyle BackColor=\"#F7F6F3\" ForeColor=\"#333333\" /> ");
            metodosPadroes.AppendLine("                 <SelectedRowStyle BackColor=\"#E2DED6\" Font-Bold=\"True\" ForeColor=\"#333333\" /> ");
            metodosPadroes.AppendLine("                 <SortedAscendingCellStyle BackColor=\"#E9E7E2\" /> ");
            metodosPadroes.AppendLine("                 <SortedAscendingHeaderStyle BackColor=\"#506C8C\" /> ");
            metodosPadroes.AppendLine("                 <SortedDescendingCellStyle BackColor=\"#FFFDF8\" /> ");
            metodosPadroes.AppendLine("                  <SortedDescendingHeaderStyle BackColor=\"#6F8DAE\" />");
            metodosPadroes.AppendLine("           </asp:GridView>           ");
            metodosPadroes.AppendLine("           <br />       ");
            metodosPadroes.AppendLine("        </div>          ");
            metodosPadroes.AppendLine("                  ");

          
          
            metodosPadroes.AppendLine("*/ ");


            return metodosPadroes.ToString();
        }

        private static String campoGridpesquisa(String campo, String tipo)
        {
            StringBuilder col = new StringBuilder();
            qtdCol++;
            col.AppendLine("                    <asp:HyperLinkField DataTextField=\""+campo+"\" Text=\""+campo+"\" Visible=\"true\" ");
            col.AppendLine("                    HeaderText=\"" + campo + "\" DataNavigateUrlFormatString=\"~/modulos/Coloca Nome Do Modulo/" + nometabela + "Detalhes.aspx?campoIndex={0}\" ");
            col.AppendLine("                        DataNavigateUrlFields=\"//colocar o campo Index que fara a pesquisar\" />  ");
            return col.ToString();
        }



    }
}