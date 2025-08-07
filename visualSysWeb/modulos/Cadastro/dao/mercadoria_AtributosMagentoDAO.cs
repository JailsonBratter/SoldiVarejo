using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace visualSysWeb.dao
{
    public class mercadoria_AtributosMagentoDAO
    {
        public string PLU { get; set; }
        public int tipoProduto = 0;
        public string sabor { get; set; }
        public string especie { get; set; }
        public string racas { get; set; }
        public string cuidados { get; set; }
        public string farmacos { get; set; }
        public string tipoAcessorios { get; set; }
        public string tipoPetiscos { get; set; }
        public string porte { get; set; }
        public string idade { get; set; }
        public string odor { get; set; }
        public string tipoAreia { get; set; }
        public string tipoHigienico { get; set; }
        public string cor { get; set; }
        public string qtdeUnidade { get; set; }
        public decimal pesoGramas = 0;
        public string dosagemRecomendada { get; set; }
        public int marca = 0;
        public int outlet = 0;

        private User usr;

        public mercadoria_AtributosMagentoDAO(string plu, User usr)
        {
            this.usr = usr;
            this.PLU = plu;
            String sql = "Select * from  mercadoria_Magento_Atributos where plu =" + plu;
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }

        private void carregarDados(SqlDataReader rs)
        {
            try
            {
                if (rs.Read())
                {
                     int.TryParse(rs["Tipo_Produto"].ToString(), out tipoProduto);
                    sabor = rs["Sabor"].ToString();
                    especie = rs["especie"].ToString();
                    racas = rs["racas"].ToString();
                    cuidados = rs["cuidados"].ToString();
                    farmacos = rs["farmacos"].ToString();
                    tipoAcessorios = rs["tipo_acessorios"].ToString();
                    tipoPetiscos = rs["tipo_petiscos"].ToString();
                    porte = rs["porte"].ToString();
                    idade = rs["idade"].ToString();
                    odor = rs["odor"].ToString();
                    tipoAreia = rs["tipo_areia"].ToString();
                    tipoHigienico = rs["tipo_higienico"].ToString();
                    cor = rs["cor"].ToString();
                    qtdeUnidade = rs["Quantidade_Por_Unidade"].ToString();
                    Decimal.TryParse(rs["Peso_Em_Gramas"].ToString(), out pesoGramas);
                    int.TryParse(rs["Marca"].ToString(), out marca);
                    dosagemRecomendada = rs["Dosagem_Recomendada"].ToString();
                    int.TryParse(rs["outlet"].ToString(), out outlet);
                }
            }
            catch
            {

            }
        }
        public bool salvar(bool novo, SqlConnection conn, SqlTransaction tran)
        {
            if (novo)
            {
                insert(conn, tran);
            }
            else
            {
                //update(conn, tran);
            }
            return true;
        }

        private void insert(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String sql = " insert into Mercadoria_Magento_Atributos (" +
                          "PLU" +
                            ", Tipo_Produto " +
                            ", Sabor " +
                            ", Especie " +
                            ", Racas " +
                            ", Cuidados " +
                            ", Farmacos " +
                            ", Tipo_Acessorios " +
                            ", Tipo_Petiscos " +
                            ", Porte " +
                            ", Idade " +
                            ", Odor " +
                            ", Tipo_Areia " +
                            ", Tipo_Higienico " +
                            ", Cor " +
                            ", Quantidade_Por_Unidade" +
                            ", Peso_Em_Gramas" +
                            ", Dosagem_Recomendada" + 
                            ", Marca" +
                            ", Outlet" +
                            " ) VALUES (" +
                            "'" + PLU + "'" +
                            ", " + tipoProduto.ToString() +
                            ", '" + sabor + "'" +
                            ", '" + especie + "'" +
                            ", '" + racas + "'" +
                            ", '" + cuidados + "'" +
                            ", '" + farmacos + "'" +
                            ", '" + tipoAcessorios + "'" +
                            ", '" + tipoPetiscos + "'" +
                            ", '" + porte + "'" +
                            ", '" + idade + "'" +
                            ", '" + odor + "'" +
                            ", '" + tipoAreia + "'" +
                            ", '" + tipoHigienico + "'" +
                            ", '" + cor + "'" + 
                            ", '" + qtdeUnidade + "'" + 
                            ", " + pesoGramas.ToString().Replace(",", ".") +
                            ", '" + dosagemRecomendada + "'" + 
                            ", " + marca.ToString() +
                            ", " + outlet.ToString() + ")";
                Conexao.executarSql(sql, conn, tran);
            }
            catch
            {

            }
        }
    }
}