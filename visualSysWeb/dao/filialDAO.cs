using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.code;

namespace visualSysWeb.dao
{
    public class filialDAO
    {

        public String ip = "";
        public String Filial = "";
        public String Razao_Social { get; set; }
        public String Fantasia { get; set; }
        public String CNPJ { get; set; }
        public String IE { get; set; }
        public String Cidade { get; set; }
        public String UF { get; set; }
        public String Endereco { get; set; }
        public String CEP { get; set; }
        public String PLU_inicial { get; set; }
        public String diretorio_gera { get; set; }
        public String conversor { get; set; }
        public bool dismarca_alteracoes { get; set; }
        public String diretorio_balanca { get; set; }
        public bool baixa_caderneta { get; set; }
        public int loja = 0;
        public bool gera_vendedor { get; set; }
        public Decimal pis = 0;
        public Decimal cofins = 0;
        public String cst_pis_cofins = "";
        public String Reg_Estadual { get; set; }
        public String Reg_Federal { get; set; }
        public String RPA { get; set; }
        public Decimal IR = 0;
        public Decimal CSLL = 0;
        public String Aliquota_est { get; set; }
        public String Aliquota_fed { get; set; }
        public String bairro { get; set; }
        public String diretorio_busca_preco { get; set; }
        public String diretorio_multiloja { get; set; }
        public String telefone { get; set; }
        public int serie_nfe = 0;
        public String fone { get; set; }
        private String vDiretorio_exporta = "";
        public String diretorio_exporta
        {
            get
            {
                //if (tipo_certificado.Equals("A3"))
                //{
                //    if (ip.Equals("::1"))
                //        return vDiretorio_exporta;
                //    else
                //        return "\\\\" + ip + "\\" + CNPJ.Replace(".", "").Replace("/", "");
                //}
                //else
                //{
                    return vDiretorio_exporta;
                //}
            }

            set { vDiretorio_exporta = value; }
        }
        public String diretorio_etiqueta = "";
        public String endereco_nro { get; set; }
        public String numero { get; set; }
        public String ICMSSN { get; set; }
        public String CSOSN { get; set; }
        public String CRT { get; set; }
        public String chave_XML { get; set; }
        public String certificado { get; set; }
        public String certificado_senha { get; set; }
        public String certificado_arquivo { get; set; }

        public String caminhoServidor = "";
        public bool producaoNfe { get; set; }
        public DateTime dtFechamentoEstoque = new DateTime();
        public String dtFechamentoEstoqueBr
        {
            get
            {
                return dataBr(dtFechamentoEstoque);
            }
        }
        public DateTime dtFechamentoFinanceiro = new DateTime();
        public String dtFechamentoFinanceiroBr
        {
            get
            {
                return dataBr(dtFechamentoFinanceiro);
            }
        }
        public String tipo_certificado = "";
        public String gerar_bpreco = "";
        public bool inibe_marcacao_familia = false;
        public String ultima_versao = "";
        public DateTime data_retira_oferta = new DateTime();
        public String versao_atual = "";
        public String chave_senha = "";
        public String mBusca_preco = "";
        public String inicio_periodo = "";
        public String fim_periodo = "";
        public int dias_periodo = 0;
        public bool produtora { get; set; }
        public int pdv = 1;
        public List<FilialIEDAO> IEs;
        public String codigo_IBGE { get; set; }
        public string diretorio_Schemas { get; set; }

        public filialDAO() { }
        public filialDAO(String Filial)
        { //colocar campo index da tabela
            String sql = "Select * from  filial where filial ='" + Filial + "'";
            SqlDataReader rs = Conexao.consulta(sql, new User(), true);
            carregarDados(rs);
            try
            {
                IEs = FilialIEDAO.buscarIE(Filial);
            }
            catch
            {

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
                    Filial = rs["Filial"].ToString();
                    Razao_Social = rs["Razao_Social"].ToString();
                    Fantasia = rs["Fantasia"].ToString();
                    CNPJ = rs["CNPJ"].ToString();
                    IE = rs["IE"].ToString();
                    Cidade = rs["Cidade"].ToString();
                    UF = rs["UF"].ToString();
                    Endereco = rs["Endereco"].ToString();
                    CEP = rs["CEP"].ToString();
                    PLU_inicial = rs["PLU_inicial"].ToString();
                    diretorio_gera = rs["diretorio_gera"].ToString();
                    conversor = rs["conversor"].ToString();
                    dismarca_alteracoes = (rs["dismarca_alteracoes"].ToString().Equals("1") ? true : false);
                    diretorio_balanca = rs["diretorio_balanca"].ToString();
                    baixa_caderneta = (rs["baixa_caderneta"].ToString().Equals("1") ? true : false);
                    loja = (rs["loja"].ToString().Equals("") ? 0 : int.Parse(rs["loja"].ToString())); ;
                    gera_vendedor = (rs["gera_vendedor"].ToString().Equals("1") ? true : false);
                    pis = (Decimal)(rs["pis"].ToString().Equals("") ? new Decimal() : rs["pis"]);
                    cofins = (Decimal)(rs["cofins"].ToString().Equals("") ? new Decimal() : rs["cofins"]);
                    cst_pis_cofins = rs["cst_pis_cofins"].ToString();

                    Reg_Estadual = rs["Reg_Estadual"].ToString();
                    Reg_Federal = rs["Reg_Federal"].ToString();
                    RPA = rs["RPA"].ToString();
                    IR = (Decimal)(rs["IR"].ToString().Equals("") ? new Decimal() : rs["IR"]);
                    CSLL = (Decimal)(rs["CSLL"].ToString().Equals("") ? new Decimal() : rs["CSLL"]);
                    Aliquota_est = rs["Aliquota_est"].ToString();
                    Aliquota_fed = rs["Aliquota_fed"].ToString();
                    bairro = rs["bairro"].ToString();
                    diretorio_busca_preco = rs["diretorio_busca_preco"].ToString();
                    diretorio_multiloja = rs["diretorio_multiloja"].ToString();
                    telefone = rs["telefone"].ToString();
                    serie_nfe = (rs["serie_nfe"].ToString().Equals("") ? 0 : int.Parse(rs["serie_nfe"].ToString()));
                    fone = rs["fone"].ToString();
                    diretorio_exporta = rs["diretorio_exporta"].ToString();
                    endereco_nro = rs["endereco_nro"].ToString();
                    numero = rs["numero"].ToString();
                    ICMSSN = rs["ICMSSN"].ToString();
                    CSOSN = rs["CSOSN"].ToString();
                    CRT = rs["CRT"].ToString();
                    chave_XML = rs["chave_XML"].ToString();
                    certificado = rs["certificado"].ToString();
                    certificado_senha = rs["certificado_senha"].ToString();
                    producaoNfe = (rs["producao_nfe"].ToString().Equals("1") ? true : false);
                    dtFechamentoEstoque = (rs["dt_fechamento_estoque"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["dt_fechamento_estoque"].ToString()));
                    dtFechamentoFinanceiro = (rs["dt_fechamento_financeiro"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["dt_fechamento_financeiro"].ToString()));
                    tipo_certificado = rs["tipo_certificado"].ToString();
                    gerar_bpreco = rs["gerar_bpreco"].ToString();
                    inibe_marcacao_familia = (rs["inibe_marcacao_familia"].ToString().Equals("1") ? true : false);
                    versao_atual = rs["versao_atual"].ToString();
                    ultima_versao = rs["ultima_versao"].ToString();
                    chave_senha = rs["chave_senha"].ToString();
                    mBusca_preco = (rs["Gerar_bpreco"].ToString().Equals("") ? "" : rs["Gerar_bPreco"].ToString());
                    produtora = rs["produtora"].ToString().Equals("1");
                    try
                    {
                        pdv = Funcoes.intTry(rs["pdv"].ToString());
                    }
                    catch (Exception)
                    {

                        throw new Exception("O Campo PDV (int) da tabela Filial não foi criado");
                    }
                    
                    try
                    {


                        inicio_periodo = rs["inicio_periodo"].ToString();
                        fim_periodo = rs["fim_periodo"].ToString();
                        int.TryParse(rs["dias_periodo"].ToString(), out dias_periodo);
                    }
                    catch (Exception)
                    {

                        throw new Exception("O Campos inicio_periodo,fim_periodo e dias_periodo  da tabela Filial não foram criados no banco ");
                    }
                    try
                    {
                        data_retira_oferta = (rs["data_retira_oferta"].ToString().Equals("") ? new DateTime() : DateTime.Parse(rs["data_retira_oferta"].ToString()));
                    }
                    catch (Exception)
                    {

                        throw new Exception("O Campo data_retira_oferta da tabela Filial não foi criado no banco ");
                    }
                    try
                    {
                        diretorio_etiqueta = rs["diretorio_etiqueta"].ToString();
                    }
                    catch (Exception)
                    {

                        throw new Exception("O Campo diretorio_etiqueta Varchar(50) da tabela Filial não foi criado no banco ");
                    }
                    certificado_arquivo = rs["certificado_arquivo"].ToString();
                    codigo_IBGE = rs["codigo_IBGE"].ToString();
                    diretorio_Schemas = rs["Diretorio_Schemas"].ToString();

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
                String sql= "update  filial set "+
                      
                       "Razao_Social='"+Razao_Social+"'"+
                       ",Fantasia='"+Fantasia+"'"+
                       ",CNPJ='"+CNPJ+"'"+
                       ",IE='"+IE+"'"+
                       ",Cidade='"+Cidade+"'"+
                       ",UF='"+UF+"'"+
                       ",Endereco='"+Endereco+"'"+
                       ",CEP='"+CEP+"'"+
                       ",PLU_inicial='"+PLU_inicial+"'"+
                       ",diretorio_gera='"+diretorio_gera+"'"+
                       ",conversor='"+conversor+"'"+
                       ",dismarca_alteracoes="+(dismarca_alteracoes?"1":"0")+
                       ",diretorio_balanca='"+diretorio_balanca+"'"+
                       ",baixa_caderneta="+(baixa_caderneta?"1":"0")+
                       ",loja="+loja.ToString()+
                       ",gera_vendedor="+(gera_vendedor?"1":"0")+
                       ",pis="+pis.ToString().Replace(",", ".")+
                       ",cofins="+cofins.ToString().Replace(",", ".")+
                       ",Reg_Estadual='"+Reg_Estadual+"'"+
                       ",Reg_Federal='"+Reg_Federal+"'"+
                       ",RPA='"+RPA+"'"+
                       ",IR="+IR.ToString().Replace(",", ".")+
                       ",CSLL="+CSLL.ToString().Replace(",", ".")+
                       ",Aliquota_est='"+Aliquota_est+"'"+
                       ",Aliquota_fed='"+Aliquota_fed+"'"+
                       ",bairro='"+bairro+"'"+
                       ",diretorio_busca_preco='"+diretorio_busca_preco+"'"+
                       ",diretorio_multiloja='"+diretorio_multiloja+"'"+
                       ",telefone='"+telefone+"'"+
                       ",serie_nfe="+serie_nfe+
                       ",fone='"+fone+"'"+
                       ",diretorio_exporta='"+diretorio_exporta+"'"+
                       ",endereco_nro='"+endereco_nro+"'"+
                       ",numero='"+numero+"'"+
                       ",ICMSSN='" +ICMSSN+"'"+
                       ",CSOSN='"+ CSOSN+"'"+
                       ",CRT='" +CRT+"'"+
                       ",dt_fechamento_estoque= " + (dtFechamentoEstoque.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + dtFechamentoEstoque.ToString("yyyy-MM-dd") + "'") +
                       ",dt_fechamento_financeiro =" + (dtFechamentoFinanceiro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + dtFechamentoFinanceiro.ToString("yyyy-MM-dd") + "'") +
                       ",tipo_certificado='" +tipo_certificado+"'"+
                       ",gerar_bpreco='" + gerar_bpreco+"'"+
                       ",inibe_marcacao_familia=" +(inibe_marcacao_familia?"1":"0")+
                       ",inicio_periodo='" + inicio_periodo + "'" +
                       ",fim_periodo='" + fim_periodo + "'" +
                       ",dias_periodo=" + dias_periodo.ToString() +
                       ",cst_pis_cofins='"+cst_pis_cofins+"'"+
                       ",produtora=" + (produtora ? "1" : "0") +
                       ",pdv="+pdv+
             "  where Filial='" +Filial+"'";
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
            String sql = "delete from filial  where campoIndex= "; //colocar campo index
            Conexao.executarSql(sql);
            return true;
        }

        private void insert()
        {
            try
            {
                String sql = " insert into filial( " +
                      "Filial," +
                      "Razao_Social," +
                      "Fantasia," +
                      "CNPJ," +
                      "IE," +
                      "Cidade," +
                      "UF," +
                      "Endereco," +
                      "CEP," +
                      "PLU_inicial," +
                      "diretorio_gera," +
                      "conversor," +
                      "dismarca_alteracoes," +
                      "diretorio_balanca," +
                      "baixa_caderneta," +
                      "loja," +
                      "gera_vendedor," +
                      "pis," +
                      "cofins," +
                      "Reg_Estadual," +
                      "Reg_Federal," +
                      "RPA," +
                      "IR," +
                      "CSLL," +
                      "Aliquota_est," +
                      "Aliquota_fed," +
                      "bairro," +
                      "diretorio_busca_preco," +
                      "diretorio_multiloja," +
                      "telefone," +
                      "serie_nfe," +
                      "fone," +
                      "diretorio_exporta," +
                      "endereco_nro," +
                      "numero," +
                      "ICMSSN," +
                      "CSOSN," +
                      "CRT," +
                      "dt_fechamento_estoque," +
                      "dt_fechamento_financeiro," +
                      "tipo_certificado," +
                      "gerar_bpreco," +
                      "inibe_marcacao_familia" +
                      ",inicio_periodo" +
                      ",fim_periodo" +
                      ",dias_periodo" +
                      ",cst_pis_cofins"+
                       ",produtora" +
                       ",pdv"+
                 ") values(" +
                      "'" + Filial + "'" +
                      "," + "'" + Razao_Social + "'" +
                      "," + "'" + Fantasia + "'" +
                      "," + "'" + CNPJ + "'" +
                      "," + "'" + IE + "'" +
                      "," + "'" + Cidade + "'" +
                      "," + "'" + UF + "'" +
                      "," + "'" + Endereco + "'" +
                      "," + "'" + CEP + "'" +
                      "," + "'" + PLU_inicial + "'" +
                      "," + "'" + diretorio_gera + "'" +
                      "," + "'" + conversor + "'" +
                      "," + (dismarca_alteracoes ? 1 : 0) +
                      "," + "'" + diretorio_balanca + "'" +
                      "," + (baixa_caderneta ? 1 : 0) +
                      "," + loja.ToString() +
                      "," + (gera_vendedor ? 1 : 0) +
                      "," + pis.ToString().Replace(",", ".") +
                      "," + cofins.ToString().Replace(",", ".") +
                      "," + "'" + Reg_Estadual + "'" +
                      "," + "'" + Reg_Federal + "'" +
                      "," + "'" + RPA + "'" +
                      "," + IR.ToString().Replace(",", ".") +
                      "," + CSLL.ToString().Replace(",", ".") +
                      "," + "'" + Aliquota_est + "'" +
                      "," + "'" + Aliquota_fed + "'" +
                      "," + "'" + bairro + "'" +
                      "," + "'" + diretorio_busca_preco + "'" +
                      "," + "'" + diretorio_multiloja + "'" +
                      "," + "'" + telefone + "'" +
                      "," + serie_nfe +
                      "," + "'" + fone + "'" +
                      "," + "'" + diretorio_exporta + "'" +
                      "," + "'" + endereco_nro + "'" +
                      "," + "'" + numero + "'" +
                      ",'" + ICMSSN + "'" +
                      ",'" + CSOSN + "'" +
                      ",'" + CRT + "'," +
                      (dtFechamentoEstoque.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + dtFechamentoEstoque.ToString("yyyy-MM-dd") + "'") + "," +
                      (dtFechamentoFinanceiro.ToString("yyyy-MM-dd").Equals("0001-01-01") ? "null" : "'" + dtFechamentoFinanceiro.ToString("yyyy-MM-dd") + "'") + "," +
                      "'" + tipo_certificado + "'," +
                      "'" + gerar_bpreco + "'," +
                      (inibe_marcacao_familia ? "1" : "0") +
                      ",'" + inicio_periodo + "'" +
                      ",'" + fim_periodo + "'" +
                      "," + dias_periodo.ToString() +
                      ",'"+cst_pis_cofins+"'"+
                      "," + (produtora ? "1" : "0") +
                      ","+pdv+
                     ");";

                Conexao.executarSql(sql);

            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Inserir os valores erro:" + err.Message);
            }
        }

        public DateTime GetDataFechamentoEstoque(String Filial)
        { //colocar campo index da tabela

            String sql = "Select * from  filial where filial ='" + Filial + "'";
            SqlDataReader rs = Conexao.consulta(sql, new User(), true);

            try
            {
                DateTime dt = new DateTime();
                rs.Read();
                DateTime.TryParse(rs["Dt_Fechamento_Estoque"].ToString(), out dt);
                return dt;
            }
            catch
            {
                return DateTime.MinValue;
            }
            finally
            {
                if (rs != null)
                {
                    rs.Close();
                }
            }
        }
    }
}