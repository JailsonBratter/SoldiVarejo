using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using visualSysWeb;
using System.Data;
using System.Data.SqlClient;
using visualSysWeb.dao;
using visualSysWeb.code;

namespace visualSysWeb.modulos.Cadastro.dao
{
    public class Cardapio_iFood_Produto
    {
        public string id { get; set; }
        public bool status { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string externalCode { get; set; }
        public String image { get; set; }
        public string serving { get; set; } //SERVES_1
        public string ean { get; set; }
        public string PLU { get; set; }
        public string serve { get; set; }
        public string horaInicio { get; set; }
        public string horaFim { get; set; }
        public bool dia1 { get; set; }
        public bool dia2 { get; set; }
        public bool dia3 { get; set; }
        public bool dia4 { get; set; }
        public bool dia5 { get; set; }
        public bool dia6 { get; set; }
        public bool dia7 { get; set; }
        public List<String> restricaoAlimentar = new List<string>();
        public decimal preco = 0;
        public string IDCategoria { get; set; }

        private string pathCaminho = "";

        public Cardapio_iFood_Produto()
        {

        }

        public Cardapio_iFood_Produto(int IDCardapio, string PLU, string Caminho)
        {
            pathCaminho = Caminho + PLU + "\\";
            string sqliFood = "sp_Cons_iFood_Produtos " + IDCardapio.ToString() + ", '" + PLU + "'";
            SqlDataReader dr = Conexao.consulta(sqliFood, null, false);
            if (dr.HasRows)
            {
                CarregarDados(dr);
            }
        }

        public string getImagem64(string PLU, string Caminho )
        {
            //String path = server.MapPath("~/modulos/Cadastro/imgs/uploads/" + PLU + "/");
            string path = pathCaminho;
            string ID = "";
            string ImagePath = "";
            String base64String = "";
            if (Directory.Exists(@path))
            {
                DirectoryInfo Dir = new DirectoryInfo(@path);
                FileInfo[] Files = Dir.GetFiles("*").OrderByDescending(f => f.CreationTime).ToArray();
                foreach (FileInfo File in Files)
                {
                    if (File.Extension.ToString().ToUpper().Equals(".PNG") || File.Extension.ToString().ToUpper().Equals(".JPG") || File.Extension.ToString().ToUpper().Equals(".JPEG"))
                    {
                        ID = File.Name.ToString();
                        //ImagePath = "~/modulos/Cadastro/imgs/uploads/" + PLU + "/" + File.Name;
                        using (Image image = Image.FromFile(path + File.Name))
                        {
                            using (MemoryStream m = new MemoryStream())
                            {
                                image.Save(m, image.RawFormat);
                                byte[] imageBytes = m.ToArray();

                                // Convert byte[] to Base64 String
                                base64String = Convert.ToBase64String(imageBytes);
                                this.image = base64String;
                            }
                        }
                        break;
                    }
                }
            }
            return base64String;

        }

        //Pesquisa todos os produtos do cardapio e retornar uma lista de produtos
        public List<Cardapio_iFood_Produto> produtosCardapio(int IDCardapio, string pathImagens)
        {
            List<Cardapio_iFood_Produto> lista = new List<Cardapio_iFood_Produto>();
            try
            {
                SqlDataReader dr;
                dr = Conexao.consulta("sp_Cons_iFood_Produtos " + IDCardapio.ToString(), null, false);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Cardapio_iFood_Produto produto = new Cardapio_iFood_Produto();
                        produto.status = (dr["status"].ToString().Equals("1") ? true : false);
                        produto.name = dr["nome"].ToString();
                        produto.id = dr["ProductID"].ToString();
                        produto.description = dr["Descricao_Comercial"].ToString();
                        produto.ean = dr["EAN"].ToString();
                        produto.PLU = dr["PLU"].ToString();
                        produto.image = "data:image/png;base64," + produto.getImagem64(produto.PLU, pathImagens);
                        if (produto.image.Length < 25)
                        {
                            produto.image = "";
                        }
                        produto.serve = dr["serve"].ToString();
                        produto.horaInicio = dr["horaInicio"].ToString();
                        produto.horaFim = dr["horaFim"].ToString();
                        produto.dia1 = (dr["dia1"].ToString().Equals("1") ? true : false);
                        produto.dia2 = (dr["dia2"].ToString().Equals("1") ? true : false);
                        produto.dia3 = (dr["dia3"].ToString().Equals("1") ? true : false);
                        produto.dia4 = (dr["dia4"].ToString().Equals("1") ? true : false);
                        produto.dia5 = (dr["dia5"].ToString().Equals("1") ? true : false);
                        produto.dia6 = (dr["dia6"].ToString().Equals("1") ? true : false);
                        produto.dia7 = (dr["dia7"].ToString().Equals("1") ? true : false);
                        if (!dr["restricaoAlimentar"].ToString().Trim().Equals(""))
                        {
                            produto.restricaoAlimentar.Add(dr["restricaoAlimentar"].ToString());
                        }
                        Decimal.TryParse(dr["preco"].ToString(), out produto.preco);
                        produto.IDCategoria = dr["IDCategoria"].ToString();
                        lista.Add(produto);
                    }
                }
                return lista;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CarregarDados(SqlDataReader dr)
        {
            try
            {
                while (dr.Read())
                {
                    this.status = (dr["status"].ToString().Equals("1") ? true : false);
                    this.name = dr["nome"].ToString();
                    this.id = dr["ProductID"].ToString();
                    this.description = dr["Descricao_Comercial"].ToString();
                    this.ean = dr["EAN"].ToString();
                    this.PLU = dr["PLU"].ToString();
                    this.image = "data:image/png;base64," + this.getImagem64(this.PLU, pathCaminho);
                    if (this.image.Length < 25)
                    {
                        this.image = "";
                    }
                    this.serve = dr["serve"].ToString();
                    this.horaInicio = dr["horaInicio"].ToString();
                    this.horaFim = dr["horaFim"].ToString();
                    this.dia1 = (dr["dia1"].ToString().Equals("1") ? true : false);
                    this.dia2 = (dr["dia2"].ToString().Equals("1") ? true : false);
                    this.dia3 = (dr["dia3"].ToString().Equals("1") ? true : false);
                    this.dia4 = (dr["dia4"].ToString().Equals("1") ? true : false);
                    this.dia5 = (dr["dia5"].ToString().Equals("1") ? true : false);
                    this.dia6 = (dr["dia6"].ToString().Equals("1") ? true : false);
                    this.dia7 = (dr["dia7"].ToString().Equals("1") ? true : false);
                    if (!dr["restricaoAlimentar"].ToString().Trim().Equals(""))
                    {
                        this.restricaoAlimentar.Add(dr["restricaoAlimentar"].ToString());
                    }
                    Decimal.TryParse(dr["preco"].ToString(), out this.preco);
                    this.IDCategoria = dr["IDCategoria"].ToString();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }


    }
}