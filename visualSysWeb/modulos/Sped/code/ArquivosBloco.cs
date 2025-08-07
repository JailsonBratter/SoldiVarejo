using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using visualSysWeb.dao;

namespace visualSysWeb.modulos.Sped.code
{
    public class ArquivosBloco
    {
        DateTime dataInicio = new DateTime();
        DateTime dataFim = new DateTime();
        String tipoArquivo = "";
        User usr = null;

        public ArquivosBloco(User usrl, DateTime dtInicio, DateTime dtFim, String tArquivo)
        {
            this.dataInicio = dtInicio;
            this.dataFim = dtFim;
            this.tipoArquivo = tArquivo.ToUpper();
            this.usr = usrl;
            if (!Directory.Exists(@"c:\Sped\" + usr.getFilial() + @"\" + tipoArquivo + @"\" + dataInicio.ToString("yyyyMM")))
            {
                Directory.CreateDirectory(@"c:\Sped\" + usr.getFilial() + @"\" + tipoArquivo + @"\" + dataInicio.ToString("yyyyMM"));
            }

            if (!Directory.Exists(@"c:\Sped\" + usr.getFilial() + @"\" + tipoArquivo + @"\" + dataInicio.ToString("yyyyMM") + @"\Blocos"))
            {
                Directory.CreateDirectory(@"c:\Sped\" + usr.getFilial() + @"\" + tipoArquivo + @"\" + dataInicio.ToString("yyyyMM") + @"\Blocos");
            }
        }

        public bool gerarArquivo(String nomeArquivo, StringBuilder texto)
        {
            String endArquivo = @"c:\Sped\" + usr.getFilial() + @"\" + tipoArquivo + @"\" + dataInicio.ToString("yyyyMM") + @"\Blocos\" + nomeArquivo + ".txt";
            StreamWriter valor = new StreamWriter(endArquivo, false, Encoding.ASCII);
            valor.Write(texto.ToString().Trim());
            valor.Close();
            return true;
        }

        public void limparArquivos()
        {
            String end = @"c:\Sped\" + usr.getFilial() + @"\" + tipoArquivo + @"\" + dataInicio.ToString("yyyyMM") + @"\Blocos\";
            if (Directory.Exists(end))
            {
                string[] _files = Directory.GetFiles(end, "*", SearchOption.AllDirectories);

                foreach (string _file in _files)
                {
                    File.Delete(_file);
                }
            }
        }

        public bool arquivoGerado(String nomeArquivo)
        {
            String endArquivo = @"c:\Sped\" + usr.getFilial() + @"\" + tipoArquivo + @"\" + dataInicio.ToString("yyyyMM") + @"\Blocos\" + nomeArquivo + ".txt";
            bool exi = File.Exists(endArquivo);
            return exi;

        }
        public String BaixarArquivoFinal(String caminhoDowload)
        {

            String endArquivo = @"c:\Sped\" + usr.getFilial() + @"\" + tipoArquivo + @"\" + dataInicio.ToString("yyyyMM") + @"\" + tipoArquivo + dataInicio.ToString("yyyyMM") + "ArqFinal.txt";
            String endArquivoFinal = caminhoDowload + @"\" + tipoArquivo + dataInicio.ToString("yyyyMM") + "ArqFinal.txt";
            if (!Directory.Exists(caminhoDowload))
                Directory.CreateDirectory(caminhoDowload);
            else
            {
                DirectoryInfo Dir = new DirectoryInfo(caminhoDowload);
                FileInfo[] FilesXmlBaixados = Dir.GetFiles("*.txt");
                foreach (FileInfo Arq in FilesXmlBaixados)
                {
                    File.Delete(Arq.FullName);
                }
            }

            File.Copy(endArquivo, endArquivoFinal, true);
            return endArquivoFinal;


        }


        public bool unificarArquivos()
        {



            String[] arquivos = Directory.GetFiles(@"c:\Sped\" + usr.getFilial() + @"\" + tipoArquivo + @"\" + dataInicio.ToString("yyyyMM") + @"\Blocos", "*.txt");
            StreamWriter strWriter = new StreamWriter(@"c:\Sped\" + usr.getFilial() + @"\" + tipoArquivo + @"\" + dataInicio.ToString("yyyyMM") + @"\" + tipoArquivo + dataInicio.ToString("yyyyMM") + "ArqFinal.txt");
            int nLinha = 1;
            Array.Sort(arquivos);
            try
            {
                foreach (String arquivo in arquivos)
                {
                    if (File.ReadAllText(arquivo).Length > 0)
                    {

                        nLinha += File.ReadAllLines(arquivo).GetLength(0);
                        strWriter.WriteLine(File.ReadAllText(arquivo));
                    }
                }

                strWriter.WriteLine("|9999|" + nLinha.ToString() + "|");
                strWriter.Flush();
                strWriter.Dispose();


                return true;
            }
            catch (Exception)
            {

                return false;
            }
            finally
            {
                if (strWriter != null)
                    strWriter.Close();
            }

        }
    }


}