using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class Cardapio_CategoriaDAO
    {
        public string Filial { get; set; }
        public int idCardapio { get; set; }
        public int Id { get; set; }
        public string Categoria { get; set; }
        public int Pizza { get; set; }
        public string Status { get; set; }
        public string PizzaStr { 
            get
            {
                if (Pizza == 2)
                    return "MEIA";
                else if (Pizza == 3)
                    return "TERCO";
                else 
                    return "NAO";

            } }
        public int CategoriaMeia { get; set; }
        public int CategoriaTerco { get; set; }
        public List<Cardapio_produtosDAO> Produtos { get; set; }= new List<Cardapio_produtosDAO>();
        public List<Cardapio_produtosDAO> ProdutosExcluir { get; set; }= new List<Cardapio_produtosDAO>();
        public string codigoUUID { get; set; }

        public Cardapio_CategoriaDAO(String filial, int ID )
        {
            this.Filial = filial;
            this.idCardapio = ID;
        }
        internal string Salvar()
        {
            string sql = "insert into cardapio_categorias (";
            string values = ") values(";

            sql += "filial";
            values += "'" + Filial + "'";

            sql += ",id";
            values += "," + Id.ToString();

            sql += ",categoria";
            values += ",'" + Categoria+ "'";
            
            sql += ",pizza";
            values += "," + Pizza.ToString();
            
            sql += ",categoriaMeia";
            values += "," + CategoriaMeia.ToString();
            
            sql += ",categoriaTerco";
            values += "," + CategoriaTerco.ToString();
            
            sql += ",status";
            values += ",'" + Status+"'";

            sql += ",IDCardapio";
            values += ", " + idCardapio.ToString();


            sql += values + ");";

            foreach (Cardapio_produtosDAO prod in Produtos)
            {
                prod.IdCategoria = this.Id;
                sql += prod.sql();
            }
             
            return sql;





        }

        internal void addProdutosInativos(Cardapio_produtosDAO produto)
        {
            ProdutosExcluir.Add(produto);
        }
    }
}