using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml.XPath;
using visualSysWeb.dao;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.IO;

namespace visualSysWeb.code
{
    public class Coletor
    {
        public bool coletorFixo = false;
        public String delimitador = "";

        public int pluInicio = -1;
        public int pluFim = -1;
       
        public int contadoInicio = -1;
        public int contadoFim = -1;
        public int contadoDecimal = -1;

        public int precoInicio = -1;
        public int precoFim = -1;
        public int precoDecimal = -1;



        public dynamic jColetor = null;


        public int tamPlu
        {
            get
            {
                return pluFim - (pluInicio-1);
            }
        }

        public int tamContado
        {
            get
            {
                return contadoFim - (contadoInicio-1);

            }
        }

        public int tamPreco
        {
            get
            {
                return precoFim - (precoInicio-1);
            }
        }
        public Coletor()
        {


            jColetor = Funcoes.leJson("/code/json/coletor.json");

            //posicaoFixa =================================
            try
            {

                coletorFixo = jColetor.ColetorPosicaoFixa;

            }
            catch (Exception)
            {
                throw new Exception("Campo ColetorPosicaoFixa Configurado de forma incorreta");
            }
            //Delimitador ============================
            try
            {

                delimitador = jColetor.ColetorDelimitador;

            }
            catch (Exception)
            {
                throw new Exception("Campo ColetorDelimitador Configurado de forma incorreta");
            }
            //plu ======================================
            try
            {

                pluInicio = jColetor.ColetorPluInicio;
            }
            catch (Exception)
            {

                throw new Exception("Campo ColetorPluInicio Configurado de forma incorreta");
            }
            try
            {
                pluFim = jColetor.ColetorPluFinal;
            }
            catch (Exception)
            {

                throw new Exception("Campo ColetorPluFinal Configurado de forma incorreta");
            }
            //Contado================================
            try
            {
                contadoInicio = jColetor.ColetorContadoInicio ;

            }
            catch (Exception)
            {

                throw new Exception("Campo ColetorContadoInicio Configurado de forma incorreta");
            }
            try
            {
                if (null != jColetor.ColetorContadoFinal)
                {
                    contadoFim = jColetor.ColetorContadoFinal;
                }
            }
            catch (Exception)
            {

                throw new Exception("Campo ColetorContadoFinal Configurado de forma incorreta");
            }
            try
            {
                if (null != jColetor.ColetorContadoDecimal)
                {
                    contadoDecimal = jColetor.ColetorContadoDecimal ;
                }
            }
            catch (Exception)
            {

                throw new Exception("Campo ColetorContadoDecimal Configurado de forma incorreta");
            }


            //Preco==================================
            try
            {
                if (null != jColetor.ColetorPrecoInicio)
                {
                    precoInicio = jColetor.ColetorPrecoInicio;
                }
            }
            catch (Exception)
            {

                throw new Exception("Campo ColetorPrecoInicio Configurado de forma incorreta");
            }
            try
            {
                if (null != jColetor.ColetorPrecoFinal)
                {
                    precoFim = jColetor.ColetorPrecoFinal;
                }
            }
            catch (Exception)
            {

                throw new Exception("Campo ColetorPrecoFinal Configurado de forma incorreta");
            }
            try
            {
                if (null != jColetor.ColetorPrecoDecimal)
                {
                    precoDecimal = jColetor.ColetorPrecoDecimal ;
                }
            }
            catch (Exception)
            {

                throw new Exception("Campo ColetorPrecoDecimal Configurado de forma incorreta");
            }


        }

        public void salvar()
        {

            jColetor.ColetorPosicaoFixa = coletorFixo;
            jColetor.ColetorDelimitador = delimitador;
            jColetor.ColetorPluInicio = pluInicio;
            jColetor.ColetorPluFinal = pluFim;
            jColetor.ColetorContadoInicio = contadoInicio;
            jColetor.ColetorContadoFinal = contadoFim;
            //contadoDecimal = jColetor.ColetorContadoDecimal;
            jColetor.ColetorContadoDecimal = contadoDecimal ;

            jColetor.ColetorPrecoInicio = precoInicio;
            jColetor.ColetorPrecoFinal = precoFim;
            jColetor.ColetorPrecoDecimal = precoDecimal ;
         

            String json = System.Web.Helpers.Json.Encode(jColetor);
            String endereco = Funcoes.diretorioServ + "/code/json/coletor.json";
            StreamWriter valor = new StreamWriter(endereco, false, System.Text.Encoding.ASCII);
            valor.Write(json.ToString());
            valor.Close();

        }
    }
}