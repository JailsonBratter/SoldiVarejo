using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using visualSysWeb.code;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class CardapioDAO
    {
        public bool Existe { get; set; }
        public string Filial { get; set; }
        public int ID { get; set; }
        public DateTime DtCadastro { get; set; }
        public DateTime DtUltAlteracao { get; set; }
        public string UsuarioCadastro { get; set; }
        public string UsuarioUltAlteracao { get; set; }
        public string UrlPadrao { get; set; }
        public string Token { get; set; }
        public List<Cardapio_CategoriaDAO> Categorias { get; set; } = new List<Cardapio_CategoriaDAO>();
        public string Titulo { get; set; }
        public string Catalogo { get; set; }

        private User usr = null;

        public CardapioDAO(String filial, User usr, int IDCardapio)
        {
            this.Existe = Funcoes.intTry(Conexao.retornaUmValor("Select count(*) from cardapio where filial ='" + filial + "'", null)) > 0;
            this.Filial = filial;
            this.ID = IDCardapio; 
            this.usr = usr;
            if (Existe)
            {
                carregarDados(this.ID);
            }
        }

        private void carregarDados(int ID)
        {

            SqlDataReader rs = null;
            string sql = "Select * from cardapio where filial ='" + this.Filial + "' AND ID = " + ID.ToString();
            try
            {
                rs = Conexao.consulta(sql, null, false);
                if (rs.Read())
                {
                    DtCadastro = Funcoes.dtTry(rs["data_cadastro"].ToString());
                    DtUltAlteracao = Funcoes.dtTry(rs["data_ultima_alteracao"].ToString());
                    UsuarioCadastro = rs["usuario_cadastro"].ToString();
                    UsuarioUltAlteracao = rs["usuario_alteracao"].ToString();
                    UrlPadrao = rs["url_padrao_cardapio"].ToString();
                    Token = rs["token"].ToString();
                    Titulo = rs["Titulo"].ToString();
                    Catalogo = rs["Catalog"].ToString();

                    CarregarCategorias(this.ID);
                    CarregarProdutos(this.ID);
                }

            }
            catch (Exception err)
            {
                throw new Exception("Erro - Cardapio - " + err.Message);
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
        }



        private void CarregarProdutos(int IDCardapio)
        {
            SqlDataReader rs = null;
            string sql = "Select cp.*," +
                "preco = CASE WHEN ml.promocao =1 and convert(date,GETDATE()) between ml.Data_Inicio and ml.data_fim then ml.Preco_Promocao else  ml.preco  end" +
                ",m.descricao,m.Descricao_Comercial " +
                         "   from cardapio_produtos as cp " +
                         "      inner join mercadoria as m on cp.plu=m.plu " +
                         "      inner join mercadoria_loja as ml on cp.plu=ml.plu  and ml.filial=cp.filial "+
                         "where cp.filial ='" + this.Filial + "' AND cp.IDCardapio = " + IDCardapio.ToString();
            try
            {
                rs = Conexao.consulta(sql, null, false);
                while (rs.Read())
                {
                    Cardapio_produtosDAO produto = new Cardapio_produtosDAO(this.Filial, IDCardapio);
                    produto.Plu = rs["plu"].ToString();
                    produto.Descricao = rs["descricao"].ToString();
                    produto.DescricaoComercial = rs["Descricao_Comercial"].ToString();
                    produto.IdCategoria = Funcoes.intTry(rs["idCategoria"].ToString());
                    produto.idCardapio = Funcoes.intTry(rs["idCardapio"].ToString());
                    produto.Ativo = rs["Ativo"].ToString().Equals("1");
                    int PrecoPorObs = Funcoes.intTry(rs["precoPorObs"].ToString());
                    produto.PrecoPorObs = (PrecoPorObs == 0 ? 1 : PrecoPorObs);
                    produto.Preco = Funcoes.decTry(rs["Preco"].ToString());
                    produto.CarregarObs();
                    Cardapio_CategoriaDAO categoria = this.Categorias.Find(c => c.Id.Equals(produto.IdCategoria));
                    if (categoria != null)
                        categoria.Produtos.Add(produto);
                }
            }
            catch (Exception err)
            {
                throw new Exception("Produtos - " + err.Message);
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
        }
        public Cardapio_CategoriaDAO pluIncluido(String plu)
        {
            foreach (Cardapio_CategoriaDAO categoria in Categorias)
            {
                if (categoria.Produtos.Count(p => p.Plu.Equals(plu)) > 0)
                    return categoria;
            }
            return null;
        }

        private void CarregarCategorias(int ID)
        {
            SqlDataReader rs = null;
            string sql = "Select * from cardapio_Categorias where filial ='" + this.Filial + "' AND IDCardapio = " + ID.ToString();
            try
            {
                rs = Conexao.consulta(sql, null, false);
                this.Categorias.Clear();
                while (rs.Read())
                {
                    Cardapio_CategoriaDAO categoria = new Cardapio_CategoriaDAO(this.Filial, ID);
                    
                    categoria.Id = Funcoes.intTry(rs["id"].ToString());
                    categoria.idCardapio = Funcoes.intTry(rs["idCardapio"].ToString());
                    categoria.Categoria = rs["categoria"].ToString();
                    categoria.Pizza = Funcoes.intTry(rs["pizza"].ToString());
                    categoria.CategoriaMeia = Funcoes.intTry(rs["categoriaMeia"].ToString());
                    categoria.CategoriaTerco = Funcoes.intTry(rs["categoriaTerco"].ToString());
                    categoria.Status = rs["status"].ToString();
                    this.Categorias.Add(categoria);
                }


            }
            catch (Exception err)
            {
                throw new Exception("Categorias - " + err.Message);
            }
            finally
            {
                if (rs != null)
                    rs.Close();
            }
        }


        public void Insert()
        {

            string sql = "insert into Cardapio (";
            string values = ") values (";

            sql += "filial";
            values += "'" + this.Filial + "'";

            sql += ",data_cadastro";
            values += ",getdate()";

            sql += ",data_ultima_alteracao";
            values += ",getdate()";

            sql += ",usuario_cadastro";
            values += ",'" + this.usr.getUsuario() + "'";

            sql += ",usuario_alteracao";
            values += ",'" + this.usr.getUsuario() + "'";

            sql += ",url_padrao_cardapio";
            values += ",''";
            
            sql += ",token";
            values += ",''";

            sql += values + ");";

            Conexao.executarSql(sql);

        }

        internal void salvar()
        {
            string sql = "update cardapio set ";

            sql += "usuario_alteracao ='" + UsuarioUltAlteracao + "'";
            sql += ", data_ultima_alteracao=" + Funcoes.dateSql(DtUltAlteracao);
            sql += ",url_padrao_cardapio='" + UrlPadrao + "'";
            sql += ",token='" + Token+ "'";

            sql += " where filial ='" + Filial + "' AND ID = " + this.ID;
            SqlConnection conn = Conexao.novaConexao();
            SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {

                Conexao.executarSql(sql,conn,tran);

                SalvarCategorias(conn, tran);
                tran.Commit();
            }
            catch (Exception err)
            {

                tran.Rollback();
                throw new Exception("Erro - Categoria -"+err.Message);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

        }

        private void SalvarCategorias(SqlConnection conn, SqlTransaction tran)
        {
            String sql = "delete from cardapio_produtos where filial ='" + this.Filial + "';";
            sql += "delete from cardapio_categorias where filial = '" + this.Filial + "';";
            Conexao.executarSql(sql, conn, tran);

            sql = "";
            foreach (Cardapio_CategoriaDAO cat in this.Categorias)
            {
                sql += cat.Salvar();
            }

            if(sql.Length>0)
                Conexao.executarSql(sql, conn, tran);

        }
    }
}