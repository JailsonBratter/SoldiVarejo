using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Text;
using WinSCP;

namespace visualSysWeb.code
{
    public class FTPEnviarArquivo
    {
        public string ftpServer { get; set; } = "";
        public string username { get; set; } = "";
        public string password { get; set; } = "";
        public string remotePath { get; set; } = ""; ///= "/specific/folder/"; // Caminho da pasta no servidor FTP
        public string localFilePath { get; set; } = ""; // = @"C:\path\to\your\file.txt"; // Caminho do arquivo local
        public int porta { get; set; } = 21;

        public FTPEnviarArquivo()
        {
        }


        public string EnviarArquivo()
        {
            try
            {

                //ServicePointManager.ServerCertificateValidationCallback =
                //(sender, certificate, chain, sslPolicyErrors) => true;

                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // Combina o servidor FTP e o caminho remoto para formar a URL completa
                //string ftpUrl = $"{ftpServer}{remotePath}{Path.GetFileName(localFilePath)}";
                string ftpUrl = $"{ftpServer}{remotePath}{Path.GetFileName(localFilePath)}";

                // Cria o objeto FtpWebRequest
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                // Fornece as credenciais de login
                request.Credentials = new NetworkCredential(username, password);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.EnableSsl = true;

                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                // Carrega o arquivo local em um array de bytes
                byte[] fileContents;
                using (StreamReader sourceStream = new StreamReader(localFilePath))
                {
                    fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                }
                request.ContentLength = fileContents.Length;

                WebResponse resposta = request.GetResponse();

                // Envia o arquivo para o servidor FTP
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                // Obtém a resposta do servidor FTP
                string retorno = "";
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    retorno = response.StatusCode.ToString();
                    Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
                }

                return retorno;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public string EnviarArquivoWEBClient()
        {
            try
            {
                CredentialCache credCach;

                Uri uri = new Uri($"{ftpServer}{remotePath}");
                NetworkCredential credencial= new NetworkCredential(username, password);
                Byte[] response;
                using (WebClient client = new WebClient())
                {
                    credCach = new CredentialCache();
                    credCach.Add(uri, AuthenticationSchemes.Basic.ToString(), credencial);
                    client.Credentials = credCach;
                    response = client.UploadFile(String.Format("{0}{1}", uri, "1033.jpg"), "PUT", localFilePath);
                    var x = Encoding.ASCII.GetString(response);
                }
                return "";
            }
            catch (WebException WEBeX)
            {
                throw WEBeX;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EnviarArquivoWinSCP()
        {
            //string destinationpath = "/html/villagrano/appM/static/img/";
            //string tkey = "-----BEGIN PUBLIC KEY-----MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArKIzvYrCm9L/+KJa5rsEZh64o3FP5m7PwNh7C3g4KKttD/nDXKPkmRVVPxoHCP3s95jD/oBawL3CouE6UXMARg0zCMlRAqFkaV/9GFhF/hNsv5BXHR529XXaX0JpAn+kjFKag8QX8S3TDRgoBkEk0slK6pT6mtRf8jn2KRZBOWm7JoOiOmNVUQJw2tr6Pbn8H8O696ai4V/XLI2VVYOMn7n6pn0eThu5jZqwkkmAYSo1Ev/tldLqJlnoGZc6LvQONLxM7ndeglGf6HjfrQBlz0uVcqTzljtBfgus77cIGm9fFwadJRnOlm/1IelRNIL0dfnYqIjMPpN+FCOe+unKNwIDAQAB-----END PUBLIC KEY-----";
            string tkey = "37:7E:D1:B2:4E:03:3F:13:89:D4:C0:1A:AB:F7:00:4A:14:F5:C7:AD";
            try
            {
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName=  ftpServer ,
                    FtpMode = FtpMode.Passive,
                    UserName = username,
                    Password = password,
                    PortNumber = porta,
                    FtpSecure = FtpSecure.Explicit,
                    GiveUpSecurityAndAcceptAnyTlsHostCertificate = true,
                    TlsHostCertificateFingerprint = tkey //aqui vai a chave pública que pode ser descoberta a qual o Filezila utiliza...
                };
                // FtpSecure = FtpSecure.Implicit,
                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult =
                        session.PutFiles(localFilePath, remotePath, false, transferOptions);

                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        Console.WriteLine("Upload efetuado {0} com sucesso:" ,transfer.FileName);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}