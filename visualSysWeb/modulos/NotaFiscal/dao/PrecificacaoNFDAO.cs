using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace visualSysWeb.dao
{
    public class PrecificaoNFDAO
    {
private   User usr;

public    PrecificaoNFDAO(User usr)
    {
        // TODO: Complete member initialization
        this.usr = usr;
    }
        public String Filial { get; set; }
        public String Codigo { get; set; }
        public String Cliente_Fornecedor { get; set; }
        public String PLU {get; set;}
        public String Tipo_NF { get; set; }
        public Decimal Sugestao { get; set; }
        public Decimal Preco_Custo {get; set;}
        public Decimal Preco_Atual { get; set; }
        public Decimal Margem { get; set; }
        public String Descricao { get; set;}

        public static ArrayList itens(String codigo, String tipoNf, String clienteFornecedor, User usr)
        {
            ArrayList ArrItens = new ArrayList();
            String sql = "Select distinct mercadoria.plu, mercadoria.descricao, mercadoria.preco_custo, mercadoria.margem, " + 
                "mercadoria.preco, convert(decimal(10,4), 0) as Sugestao from  nf_item inner join mercadoria on " + 
                "nf_item.plu = mercadoria.plu where codigo ='" + codigo + 
                "' and tipo_nf = " + tipoNf + 
                " and cliente_fornecedor = '" + clienteFornecedor + "'";

            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            while (rs.Read())
            {
                PrecificaoNFDAO nfItem = new PrecificaoNFDAO(usr);
                nfItem.Filial = rs["Filial"].ToString();
                nfItem.Codigo = rs["Codigo"].ToString();
                nfItem.Cliente_Fornecedor = rs["Cliente_Fornecedor"].ToString();
                nfItem.Tipo_NF = rs["Tipo_NF"].ToString();
                nfItem.PLU = rs["PLU"].ToString();
                ArrItens.Add(nfItem);

            }

            if (rs != null)
                rs.Close();
            return ArrItens;
        }

        public ArrayList ArrToString()
        {
            ArrayList item = new ArrayList();
            item.Add(Filial);
            item.Add(Codigo);
            item.Add(Cliente_Fornecedor);
            item.Add(Tipo_NF);
            item.Add(PLU);
            item.Add(Descricao);
            item.Add(Margem);
            item.Add(Preco_Atual);
            item.Add(Sugestao);
            return item;
        }

        public void PrecificacaoNFDAO(String plu, String codigo, String tipoNf, String clienteFornecedor, User usr)
        {
            this.usr = usr;
            this.PLU = plu;
            this.Codigo = codigo;
            this.Tipo_NF = tipoNf;
            this.Cliente_Fornecedor = clienteFornecedor;
            String sql = "Select * from  nf_item where plu=" + PLU + " and  codigo ='" + Codigo + "' and tipo_nf = " + Tipo_NF + " and cliente_fornecedor = '" + Cliente_Fornecedor + "'";
            SqlDataReader rs = Conexao.consulta(sql, usr, true);
            carregarDados(rs);
        }
        //Sinto Muito Me Perdoe Agradeço Eu Te Amo.
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
                Codigo = rs["Codigo"].ToString();
                Cliente_Fornecedor = rs["Cliente_Fornecedor"].ToString();
                Tipo_NF = rs["Tipo_NF"].ToString();
                PLU = rs["PLU"].ToString();
                Descricao = rs["Descricao"].ToString();
                //Margem = Decimal.Parse(rs["Margem"]);
               // Preco_Atual = rs["Preco"];
                //Sugestao = rs["Sugestao"];
            
            }

            if (rs != null)
                rs.Close();
        }
        private void update()
        {
            try
            {
                String sql = "UPDATE Mercadoria SET " +
                    // Demais campos aqui
                    "  where codigo =" + Codigo + " and tipo_nf = " + Tipo_NF + " and cliente_fornecedor = '" + Cliente_Fornecedor + "' and plu='" + PLU + "'";

                Conexao.executarSql(sql);
            }
            catch (Exception err)
            {
                throw new Exception("nao foi possivel Atualizar os valores erro:" + err.Message);
            }
        }



       // public bool salvar()
       // {
       //     update();
       // }

    }
}