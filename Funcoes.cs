using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using Ionic.Zip;
using System.Data.EntityClient;
using System.Data;
using System.Drawing;

namespace SICEpdv
{
     public class Funcoes
    {
         [return: MarshalAs(UnmanagedType.Bool)]
         [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
         public static extern bool BlockInput([In, MarshalAs(UnmanagedType.Bool)] bool fBlockIt);

        public static string IDTerminal()
        {
            string id = DateTime.Now.ToString();

            try
            {
                if (!Directory.Exists(@"c:\tempData"))
                    Directory.CreateDirectory(@"C:\tempData");

                Guid idTerminal = Guid.NewGuid();
                if (!File.Exists(@"c:\tempData\idTerminalPDV.txt"))
                {
                    using (FileStream fs = File.Create(@"c:\tempData\idTerminalPDV.txt"))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                        {
                            sw.Write(idTerminal.ToString());
                        }
                    }
                }
               
                using (StreamReader ler = new StreamReader(@"c:\tempData\idTerminalPDV.txt", Encoding.GetEncoding("ISO-8859-1")))
                {
                    while (ler.Peek() != -1)
                    {
                        id = ler.ReadLine();
                    }
                }
            }
            catch
            {
                id = IP();
            }
            
            return id.Substring(0,15);
        }


        public static string IP()
        {
            IPHostEntry local = Dns.GetHostEntry(Dns.GetHostName());
            var ipLocal = (from p in local.AddressList
                           where p.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                           select p).FirstOrDefault();
            return ipLocal.ToString();
        }

        public static string CriptografarMD5(string input)
        {
            MD5 mdhash = MD5.Create();
            byte[] data = mdhash.ComputeHash(Encoding.Default.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static void DigitarNumerosPositivos(object sender, KeyPressEventArgs e)
        {
            // Aqui sempre coloca (,)Virugula no lugar do (.)ponto
            if (e.KeyChar == Convert.ToChar(46))
                e.KeyChar = Convert.ToChar(44);
            if (e.KeyChar ==  Convert.ToChar(45))            
                e.Handled = true;                            

            if (char.IsLetter(e.KeyChar))
                e.Handled = true;
            //if (!char.IsNumber(evento.KeyChar) && evento.KeyChar != (char)Keys.Back)
            //    evento.KeyChar = Convert.ToChar(0);   
        }

        public static string FormatarDecimal(string valor,int casaDecimal=2)
        {
            try
            {
                if (casaDecimal == 3)
                    return string.Format("{0:N3}", Convert.ToDecimal(valor));

                return string.Format("{0:n2}", Convert.ToDecimal(valor));

            }
            catch
            {
                return "0,00";
            }
        }


        public static string FormatarValorMysql(decimal valor)
        {
           return string.Format("{0:N2}", valor).Replace(".", "").Replace(",", ".");
        }

        public static string FormatarDataMysql(DateTime data)
        {
            if (data == null)
            {
                return "";
            }
            else
            {
                return string.Format("{0:yyyy-MM-dd}", data);
            }
        }
        public static string FormatarValorPositivo(string Valor)
        {
            return Valor.Replace("-", "");
        }

        public static string FormatarHoraMysql(TimeSpan hora)
        {
            if(hora == null)
            {
                return "00:00:00";
            }
            return hora.ToString();
        }

        public static string FormatarZerosEsquerda(decimal valor, int tamanho,bool colocarDecimal,int casasDecimais=2)
        {
            //Exemplo com o paramento colocarDecimal 
            // If for true 
            // variavel valor sendo 17 ou  17,00 retorna 1700
            // if for false
            // valor 17 ou 17,00 retorna 0017

            string mascara = "";

            if (!colocarDecimal)
                tamanho += casasDecimais;

            for (int i = casasDecimais ; i < tamanho; i++)
            {
                mascara += "0";
            }

            if (colocarDecimal)
            {
                mascara += ".";
                string ncasasDecimais = "";
                    for (int i = 0; i < casasDecimais; i++)
			        {
                        ncasasDecimais += "0";			 
			        }
                    mascara += ncasasDecimais;
            }
             

                return valor.ToString(mascara).Replace(",", "");
        }
        

        public static string CriptografarComSenha(string dados, string senha)
        {
            byte[] b = Encoding.UTF8.GetBytes(dados);
            byte[] pw = Encoding.UTF8.GetBytes(senha);

            RijndaelManaged rm = new RijndaelManaged();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(senha, new MD5CryptoServiceProvider().ComputeHash(pw));
            rm.Key = pdb.GetBytes(32);
            rm.IV = pdb.GetBytes(16);
            rm.BlockSize = 128;
            rm.Padding = PaddingMode.PKCS7;

            MemoryStream ms = new MemoryStream();
            CryptoStream cS = new CryptoStream(ms, rm.CreateEncryptor(rm.Key, rm.IV), CryptoStreamMode.Write);
            cS.Write(b, 0, b.Length);
            cS.FlushFinalBlock();
            return System.Convert.ToBase64String(ms.ToArray());
        }

        public static string DesCriptografarComSenha(byte[] dados, string senha)
        {
            byte[] pw = Encoding.UTF8.GetBytes(senha);

            RijndaelManaged rm = new RijndaelManaged();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(senha, new MD5CryptoServiceProvider().ComputeHash(pw));
            rm.Key = pdb.GetBytes(32);
            rm.IV = pdb.GetBytes(16);
            rm.BlockSize = 128;
            rm.Padding = PaddingMode.PKCS7;
            MemoryStream ms = new MemoryStream(dados, 0, dados.Length);
            CryptoStream Cs = new CryptoStream(ms, rm.CreateDecryptor(rm.Key, rm.IV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(Cs);
            return sr.ReadToEnd();
        }

        public static string SetLength(int Tamanho)
        {
            int i;
            string Retorno = "";
            for (i = 0; i < Tamanho; i++)
                Retorno = Retorno + " ";
            return (Retorno);
        }

        public static  bool ValidaCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);

            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        public static bool ValidarCPF(string cpf)
        {
            try
            {
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                string tempCpf;
                string digito;
                int soma;
                int resto;

                cpf = cpf.Trim();
                cpf = cpf.Replace(".", "").Replace("-", "");

                if (cpf.Length != 11)
                    return false;

                tempCpf = cpf.Substring(0, 9);
                soma = 0;
                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();

                tempCpf = tempCpf + digito;

                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();

                return cpf.EndsWith(digito);

            }
            catch (Exception erro)
            {
                return false;
            }
        }

        public static void TravarTeclado(bool travar)
        {
            BlockInput(travar);
        }

        public static string RetirarFormatacaoCNPJ_CPF_IE(string conteudo)
        {
            return conteudo.Replace(".","").Replace("/","").Replace("-","");
        }

        public static string RetirarAcentos(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return "";
            /** Troca os caracteres acentuados por não acentuados **/
            string[] acentos = new string[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
            string[] semAcento = new string[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };


            for (int i = 0; i < acentos.Length; i++)
            {
                texto = texto.Replace(acentos[i], semAcento[i]);
            }
            return texto;
        }

        public static string RetornaCodigoMunIBGE(string tipo, string cidade, string uf)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            cidade = Funcoes.RetirarAcentos(cidade);
            var dados = (from p in entidade.tab_municipios
                         where p.nome == cidade
                         from r in entidade.estados
                         where r.id == p.iduf && r.uf == uf
                         select new { codigo = p.id }).FirstOrDefault();
            if (dados == null)
                return "";

            return dados.codigo.ToString() == null ? "" : dados.codigo.ToString();
        }

        public static List<string> RetonaCidade(string uf)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            var dados = from p in entidade.estados
                        where p.uf == uf
                        from r in entidade.tab_municipios
                        where r.iduf == p.id 
                        orderby r.nome
                        select new { cidade = r.nome };

            
            List<string> cidades = new List<string>();
            foreach (var item in dados)
            {
                cidades.Add(item.cidade);
            }

            return cidades;
        }

        public static string FormatarTelefone(string conteudo)
        {
            if (string.IsNullOrEmpty(conteudo))
                return conteudo;
           
            conteudo = conteudo.Trim().PadRight(11, ' ');
            if (conteudo.Substring(0, 1) == "0")
                conteudo = conteudo.Substring(1, conteudo.Length - 1);


            if (conteudo.Length == 11)
                return "(" + conteudo.Substring(0, 2) + ") " + conteudo.Substring(2, 1)+ " "+ conteudo.Substring(3, 4) + "-" + conteudo.Substring(7, 4);

            if (conteudo.Length > 11)
                return "(" + conteudo.Substring(0, 2) + ")" + conteudo.Substring(2, 4) + "-" + conteudo.Substring(6, 5);
            else
                return "(" + conteudo.Substring(0, 2) + ")" + conteudo.Substring(2, 4) + "-" + conteudo.Substring(6, 4);


        }

        public static string FormatarCartao(string cartao)
        {
            return cartao.Substring(0, 4) + ' ' + cartao.Substring(4, 4) + ' ' + cartao.Substring(8, 4) + ' ' + cartao.Substring(12, 4);
        }

        public static string FormatarCNPJ(string conteudo)
        {
            //03 590 277 0001 00
            conteudo = conteudo.PadRight(14, ' ');
            return conteudo.Substring(0, 2) + "." + conteudo.Substring(2, 3) + "." + conteudo.Substring(5, 3) + "/" + conteudo.Substring(8, 4) + "-" + conteudo.Substring(12, 2);
        }

        public static string FormatarCPF(string conteudo)
        {
            //023 716 924 09
            conteudo = conteudo.PadRight(11, ' ');
            return conteudo.Substring(0, 3) + "." + conteudo.Substring(3, 3) + "." + conteudo.Substring(6, 3) + "-" + conteudo.Substring(9, 2);
        }

       

        public static bool CompactarDiretorio(List<string> origem, string destino)
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    foreach (var folder in origem)
                    {
                        zip.AddDirectory(@folder);
                    }
                    zip.Save(@destino);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return true;
        }

        public static bool CriarTabelaTmp(string nomeTabela, DateTime dataInicial, DateTime dataFinal, string filial, bool executarSP = true)
        {
            EntityConnection conn = new EntityConnection(Conexao.stringConexao);

            /*if (Conexao.tipoConexao == 2)
                conn = new EntityConnection(Conexao.stringConexaoRemoto);*/

            //nomeTabela venda = copia a tabela de vendaarquivo
            if (!FuncoesFiscais.camposAjustado && executarSP)
            {
                try
                {
                    using (conn)
                    {
                        conn.Open();
                        EntityCommand cmd = conn.CreateCommand();
                        cmd.CommandTimeout = 3600;
                        cmd.CommandText = "siceEntities.AjustarCamposNulosManual";
                        cmd.CommandType = CommandType.StoredProcedure;

                        EntityParameter parFilial = cmd.Parameters.Add("filial", DbType.String);
                        parFilial.Direction = ParameterDirection.Input;
                        parFilial.Value = filial;

                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    FuncoesFiscais.camposAjustado = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Exceção ao processar SP camposnulosManual " + ex.Message);
                }

            }

            using (conn)
            {
                conn.Open();
                EntityCommand cmd = conn.CreateCommand();
                cmd.CommandTimeout = 3600;
                cmd.CommandText = "siceEntities.CriarTabelasTemp";
                cmd.CommandType = CommandType.StoredProcedure;

                EntityParameter tabela = cmd.Parameters.Add("tabela", DbType.String);
                tabela.Direction = ParameterDirection.Input;
                tabela.Value = nomeTabela;

                EntityParameter parFilial = cmd.Parameters.Add("filial", DbType.String);
                parFilial.Direction = ParameterDirection.Input;
                parFilial.Value = filial;

                EntityParameter inicio = cmd.Parameters.Add("dataInicial", DbType.Date);
                inicio.Direction = ParameterDirection.Input;
                inicio.Value = dataInicial.Date;

                EntityParameter final = cmd.Parameters.Add("dataFinal", DbType.Date);
                final.Direction = ParameterDirection.Input;
                final.Value = dataFinal.Date;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return true;
        }


        public static bool ProcedureAjuste(string procedureNome)
        {
            EntityConnection conn = new EntityConnection(Conexao.stringConexao);

            if (Conexao.tipoConexao == 2)
                conn = new EntityConnection(Conexao.stringConexaoRemoto);

            using (conn)
            {
                conn.Open();
                EntityCommand cmd = conn.CreateCommand();
                cmd.CommandTimeout = 3600;
                cmd.CommandText = "siceEntities."+procedureNome;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return true;
        }

         public static DateTime FormatarData(string data)
         {                          
                          
             DateTime dataConvertida = Convert.ToDateTime(data.Substring(0,4)+"/"+data.Substring(4,2)+"/"+data.Substring(6,2));
             return dataConvertida;
         }

         public static int DetectarBitOS()
         {

             int Bits = IntPtr.Size * 8;

             try
             {
                 if (Bits == 64)
                     return 64;
                 else
                     return 32;
             }
             catch
             {
                 return 32;
             }

         }


        public static bool VerificarConexaoInternet()
        {            
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);

                return true;
            }
            catch (Exception)
            {               
                return false;

            }
        }


        public static string ByteString(byte[] arquivo)
         {
            string str; // String que irá receber a conversão
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            str = enc.GetString(arquivo);
            return str;
         }


        public static bool VerificarLicencaUso(out int diasRestante, out string mensagem, string produto = "SICE.net")
        {
            try
            {
                siceEntities entidade = Conexao.CriarEntidade();

                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                DateTime dataServidor = GetDataServidor();

                
                var validade = Conexao.CriarEntidade().ExecuteStoreQuery<DateTime>("SELECT validade from iqsistemas").FirstOrDefault();
                var ultimoacesso = Conexao.CriarEntidade().ExecuteStoreQuery<DateTime>("SELECT ultimoacesso from iqsistemas").FirstOrDefault();                
                var idcliente = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT idcliente from iqsistemas").FirstOrDefault();
                var nomeliberacao = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT nomeliberacao from iqsistemas").FirstOrDefault();

                var validadecripto = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT validadecripto from iqsistemas").FirstOrDefault();
                var ultimoacessocripto = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT ultimoacessocripto from iqsistemas").FirstOrDefault();
                

                string criptografiaValidade = Funcoes.CriptografarMD5(idcliente.ToString().Trim() + nomeliberacao.Trim() + string.Format("{0:yyyy-MM-dd}", validade) + GlbVariaveis.chavePrivada);
                string criptografiaUltAcesso = Funcoes.CriptografarMD5(idcliente.ToString().Trim() + nomeliberacao.Trim() + string.Format("{0:yyyy-MM-dd}", ultimoacesso) + GlbVariaveis.chavePrivada);
                diasRestante = Convert.ToInt16(validade.Subtract(DateTime.Now.Date).Days);

                if (ultimoacesso != null && DateTime.Now.Date < ultimoacesso)
                {
                    mensagem = "Data do terminal inferior a data do último acesso";
                    throw new Exception("Ajuste a data do terminal para a data do servidor " + dataServidor.ToShortDateString());

                }
                //if (dados == null)
                //{
                //    throw new Exception("Não foram encontrados os dados do Cliente IQ Sistemas.");
                //}

              

                // Aqui para verificar o WS
                if (diasRestante < 7)
                {
                    if (VerificarConexaoInternet())
                    {
                        mensagem = "Liberado via WS.";
                        return LiberarLicencaUso(Convert.ToInt16(idcliente), nomeliberacao, DateTime.Now.Date, "", "SICE.net", true);
                    }

                }

                if (diasRestante <= 0)
                {
                    mensagem = "Licença de uso expirada há " + diasRestante.ToString() + " dia(s). Ajuste a data do computador ou digita o PIN de liberação.";
                    throw new Exception(mensagem);
                }

                if (criptografiaValidade != validadecripto || (ultimoacesso != null && criptografiaUltAcesso.ToLower() != ultimoacessocripto.ToLower()))
                {
                    throw new Exception("Dados de licença violado. Solicite um novo PIN !");
                }

                string criptoAcesso = Funcoes.CriptografarMD5(idcliente.ToString().Trim() + nomeliberacao.Trim() + string.Format("{0:yyyy-MM-dd}", dataServidor) + GlbVariaveis.chavePrivada);
                Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE iqsistemas SET ultimoacesso=CURRENT_DATE");
                Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE iqsistemas SET ultimoacessocripto='" + criptoAcesso + "'");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            mensagem = "";
            return true;
        }

        public static bool LiberarLicencaUso(int idCliente, string nomeLiberacao, DateTime validade, string senhaLiberacao, string produto = "SICE.net", bool VerLicencaWS = false)
        {
            string cripto = Funcoes.CriptografarMD5(idCliente.ToString().Trim() + nomeLiberacao.Trim() + string.Format("{0:yyyy-MM-dd}", validade) + GlbVariaveis.chavePrivada);

            DateTime dataServidor = GetDataServidor();

            string senha = "";
            foreach (var item in cripto)
            {
                if (Char.IsNumber(item))
                {
                    senha += item.ToString();
                }
            }
            bool WSLiberado = false;
            if (VerLicencaWS)
            {
                ServiceReference2.WSClientesClient WS = new ServiceReference2.WSClientesClient();
                if (WS.QtdParcelasAtrasadas(idCliente) <= 1)
                {
                    DateTime dataLiberacao = Convert.ToDateTime("07/" + DateTime.Now.Month + "/" + DateTime.Now.Year);
                    dataLiberacao = dataLiberacao.AddMonths(1);
                    validade = dataLiberacao.Date;
                    cripto = Funcoes.CriptografarMD5(idCliente.ToString().Trim() + nomeLiberacao.Trim() + string.Format("{0:yyyy-MM-dd}", validade) + GlbVariaveis.chavePrivada);

                    var dados = (WS.DadosCliente(idCliente));

                    if (dados.fantasia == nomeLiberacao)
                    {
                        WSLiberado = true;
                    }
                }
            }


            if (senha.Substring(0, 6) == senhaLiberacao || WSLiberado)
            {
                //siceEntities entidade = Conexao.CriarEntidade();
                //var dados = (from n in entidade.iqsistemas
                //             where n.produto == produto
                //             select n).FirstOrDefault();
                //dados.validade = validade.Date;
                //dados.validadecripto = cripto;
                //dados.ultimoacesso = dataServidor;
                //dados.ultimoacessocripto = Funcoes.CriptografarMD5(dados.idcliente.ToString().Trim() + dados.nomeliberacao.Trim() + string.Format("{0:yyyy-MM-dd}", dataServidor) + GlbVariaveis.chavePrivada);
                //entidade.SaveChanges();
                string ultimoacessocripto = Funcoes.CriptografarMD5(idCliente.ToString() + nomeLiberacao + string.Format("{0:yyyy-MM-dd}", dataServidor) + GlbVariaveis.chavePrivada);
                string sql = "UPDATE iqsistemas SET validade='" + string.Format("{0:yyyy-MM-dd}", validade.Date) + "',validadecripto='" + cripto + "',ultimoacesso='" + string.Format("{0:yyyy-MM-dd}", dataServidor) + "',ultimoacessocripto='" + ultimoacessocripto + "'";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                return true;
            }

            return false;
        }

        public static DateTime GetDataServidor()
        {
            string sql = @"SELECT CURRENT_DATE";
            var resultado = Conexao.CriarEntidade().ExecuteStoreQuery<DateTime>(sql);
            DateTime dataServidor = resultado.FirstOrDefault();
            return dataServidor;
        }


        public static Bitmap GerarQRCode(int largura, int altura, string texto)
        {
            try
            {
                var bw = new ZXing.BarcodeWriter();
                var encOptions = new ZXing.Common.EncodingOptions() { Width = largura, Height = altura, Margin = 0 };
                bw.Options = encOptions;
                bw.Format = ZXing.BarcodeFormat.QR_CODE;
                var resultado = new Bitmap(bw.Write(texto));
                return resultado;
            }
            catch
            {
                throw;
            }

        }

        static public bool escreveArquivo(string arquivo, string conteudo)
        {
            try
            {
                StreamWriter x;
                x = File.CreateText(arquivo);
                x.WriteLine(conteudo);
                x.Close();

                return true;
            }
            catch (Exception erro)
            {
                throw new Exception(erro.ToString());
            }
        }

        



    }
}
