using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Configuration;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data.Common;

namespace SICEpdv
{
    public class FuncoesPAFECF
    {
        public static string LerMD5(string tipo="xml")
        {
            if (!ConfiguracoesECF.pdv || ConfiguracoesECF.NFC == true)
                return ConfiguracoesECF.md5Geral;

            if (tipo == "Arquivo")
            {
                string md5 = " ";
                try
                {
                    using (StreamReader ler = new StreamReader(@Application.StartupPath + @"\md5.txt"))
                    {

                        string linha = null;
                        while ((linha = ler.ReadLine()) != null)
                        {
                            md5 = (linha.ToString());
                        }
                    }
                    ConfiguracoesECF.md5Geral = md5;
                    return md5;
                }
                catch
                {
                    ConfiguracoesECF.md5Geral = new string(' ', 33);
                    return ConfiguracoesECF.md5Geral;
                }
            }

            try
            {
                if (!File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml"))
                {
                    return "";
                }

                XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");
                IEnumerable<XElement> listaECF = doc.Element("configuracoes-pafecf").Elements("ecf");

                var config = from n in doc.Descendants("ecf")                            
                             select n;
                {
                    string md5 = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(config.Attributes("md5").First().Value), GlbVariaveis.glbSenhaIQ);
                   ConfiguracoesECF.md5Geral = md5.ToString();
                    return ConfiguracoesECF.md5Geral;
                }
            }
            catch 
            {
                throw new Exception("MD5 não foi identificado");
            }


        }

        public static bool SalvarMd5ArquivoECF()
        {

            if (!ConfiguracoesECF.pdv || ConfiguracoesECF.idECF == 0 || ConfiguracoesECF.idNFC > 0)
                return true; 


            if (!File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml") && ConfiguracoesECF.pdv)
            {
                throw new Exception("Arquivo de cadastro de ECFs não foi encontrado");
            }

            XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");

            var config = from n in doc.Descendants("ecf")
                         select n;

            foreach (var item in config)
            {
                var alterar = from n in doc.Descendants("ecf")
                              where n.Attributes("numeroFabricacaoCriptografado").First().Value == Funcoes.CriptografarComSenha(item.Attributes("numeroFabricacaoCriptografado").ToString(), GlbVariaveis.glbSenhaIQ)
                              select n;

                config.Attributes("md5").First().Value = Funcoes.CriptografarComSenha(ConfiguracoesECF.md5Geral, GlbVariaveis.glbSenhaIQ); 
                doc.Save(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");
            }

            return true;

        }

        public static string md5Arquivo(string arquivo)
        {
            MD5 md5has = MD5.Create();
            Byte[] bytes = File.ReadAllBytes(@arquivo);
            Byte[] data = md5has.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (var item in data)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }

 
        public static bool GerarArquivoIDPAF()
        {
            var destino = @Application.StartupPath + @"\listasArquivos.txt";
            StringBuilder conteudo = new StringBuilder();


            StringBuilder listaMD5 = new StringBuilder();

            //MessageBox.Show("1");

            var arquivos = from f in Directory.GetFiles(@Application.StartupPath)
                           where f.ToUpper().Contains("SICE")
                           select f;


            //MessageBox.Show("2");

            var filtroArquivo = from n in arquivos
                                // where n.EndsWith("exe") || n.EndsWith("dll")
                                where n.Contains("SICE")
                                select n;

            //MessageBox.Show("3");

            foreach (var item in filtroArquivo)
            {
                var nomeArquivo = item.ToString().Split(@"\".ToCharArray());
                if (nomeArquivo[nomeArquivo.Count() - 1].ToString().ToLower() == "sicepdv.exe")
                    File.Copy(item, @Application.StartupPath + @"\temp\" + nomeArquivo[nomeArquivo.Count() - 1], true);
            }

            //MessageBox.Show("4");
            var arquivosTMP = Directory.GetFiles(@Application.StartupPath + @"\temp\");

            var lista = (from n in arquivosTMP
                         // where n.EndsWith("exe") || n.EndsWith("dll")
                         select n).ToArray();

            foreach (var item in lista)
            {
                
                var nomeArquivo = item.ToString().Split(@"\".ToCharArray());
                //MessageBox.Show(nomeArquivo.ToString());

                //Atenção: Trecho renomeado funcionando corretamente e homologado
                // foi retirado devido a problema no carregamento da dll em alguns computadores
                //string md5 = new string(' ',33);                            
                //BemaFI32.md5FromFile(item, ref md5);
                //if (md5.Trim() == "")
                //{
                //    System.Threading.Thread.Sleep(200);
                //    BemaFI32.md5FromFile(item, ref md5);
                //}

                var md5 = FuncoesPAFECF.md5Arquivo(item.ToString()) + " ";


                listaMD5.AppendLine(nomeArquivo[nomeArquivo.Count() - 1] + " - " + md5);
            }
            //MessageBox.Show("5");
            using (FileStream fs = File.Create(@Application.StartupPath + @"\lstArquivos.txt"))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                {
                    sw.Write(listaMD5);
                }
            };


            //XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\sHouse.xml");

            //var dados = (from n in doc.Descendants("sHouse").Elements("Dados")
            //             select n).First();
    
                conteudo.AppendLine("N1"+
                     GlbVariaveis.cnpjSH.PadLeft(14, '0').Substring(0, 14) + //(Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("cnpj").First().Value), GlbVariaveis.glbSenhaIQ))
                     GlbVariaveis.IESH.PadRight(14, ' ').Substring(0, 14) + // (Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("inscricao").First().Value), GlbVariaveis.glbSenhaIQ))
                     GlbVariaveis.IEMunicipalSH.PadRight(14, ' ').Substring(0, 14) + // (Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("inscricaoMunicipal").First().Value), GlbVariaveis.glbSenhaIQ))
                     GlbVariaveis.razaoSH.PadRight(50, ' ').Substring(0, 50)); // (Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("razaoSocial").First().Value), GlbVariaveis.glbSenhaIQ))

                conteudo.AppendLine("N2"+
                    GlbVariaveis.laudoPAF.PadRight(10, ' ').Substring(0, 10) +
                    //(Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("laudo").First().Value), GlbVariaveis.glbSenhaIQ)).PadRight(10,' ').Substring(0,10) +                    
                    GlbVariaveis.nomeAplicativo.PadRight(50, ' ').Substring(0, 50) +// (Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("aplicativo").First().Value), GlbVariaveis.glbSenhaIQ)).PadRight(50,' ').Substring(0,50) +
                    GlbVariaveis.glb_Versao.PadRight(10, ' ').Substring(0, 10));
                    //(Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("versao").First().Value), GlbVariaveis.glbSenhaIQ)).PadRight(10,' ').Substring(0,10));
                

                StringBuilder lstArquivos = new StringBuilder();
                int contadorArquivo = 0;

            foreach (var item in lista)
            {
                var nomeArquivo = item.ToString().Split(@"\".ToCharArray());

                //Atenção: Trecho renomeado funcionando corretamente e homologado
                // foi retirado devido a problema no carregamento da dll em alguns computadores
                //string md5 = new string(' ',33);                            
                //BemaFI32.md5FromFile(item, ref md5);
                //if (md5.Trim() == "")
                //{
                //    System.Threading.Thread.Sleep(200);
                //    BemaFI32.md5FromFile(item, ref md5);
                //}

                var md5 = FuncoesPAFECF.md5Arquivo(item.ToString()) + " ";

                conteudo.AppendLine("N3" +
                   nomeArquivo[nomeArquivo.Count() - 1].PadRight(50, ' ').Substring(0, 50) + md5.ToString().PadRight(32, ' ').Substring(0, 32));              
                contadorArquivo++;
            }

            conteudo.AppendLine("N9"+
                GlbVariaveis.cnpjSH.PadLeft(14,'0').Substring(0,14) +
                GlbVariaveis.IESH.PadRight(14,' ').Substring(0,14) +
                Funcoes.FormatarZerosEsquerda(contadorArquivo,6,false));
                     
           
               using (FileStream fs = File.Create(@destino))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                {
                    sw.Write(conteudo);
                }
            };

            FuncoesECF.AssinarArquivo(@destino,false);
            var md5Geral = FuncoesPAFECF.md5Arquivo(@destino+" ");
            var md5ExePrincipal = FuncoesPAFECF.md5Arquivo(@Application.StartupPath + @"\temp\sicepdv.exe");
                       
            ConfiguracoesECF.md5PrincipalEXE = md5ExePrincipal;
            ConfiguracoesECF.md5Geral = md5Geral; ;

            using (FileStream fs = File.Create(@Application.StartupPath + @"\md5.txt"))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                {
                    sw.Write(md5Geral);
                }
            };

            FuncoesPAFECF.SalvarMd5ArquivoECF();

            return true;

        }


        public static bool VerificarQtdRegistro(string tabela)
        {
            //var contReg = (from n in Conexao.CriarEntidade().contdav
            //               select n.documento).Count();
            // TSQL para agilizar o retorno de registro. Pois o count do linq 
            // numa tabela grande demora muito e causa erro, testado no banco com 
            // 800.000 registro

            LogSICEpdv.Registrarlog("VerificarQtdRegistro(" + tabela + ")", "", "FuncoesPAFECF.cs");

            if (System.IO.File.Exists(@"PDVlog.txt"))
            {
                return true;
            }

           


            if (tabela == "contdav")
            {
                #region

                string sql = @"SELECT count(*) FROM contdav";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                                           select n.contdav).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;

                #endregion
            }

            if (tabela == "filiais")
            {
                #region

                string sql = @"SELECT COUNT(*) FROM filiais";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                sql = @"SELECT filiais FROM quantidaderegistros";
                var criptQtdReg = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql);

                string criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg.FirstOrDefault() != criptQtdregAtual)
                    return false;

                #endregion
            }

            if (tabela == "vendadav")
            {
                #region

                string sql = @"SELECT count(*) FROM vendadav";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                   select n.vendadav).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;

                #endregion
            }

            if (tabela == "contdavos")
            {
                #region

                string sql = @"SELECT count(*) FROM contdavos";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                   select n.contdav).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;

                #endregion
            }

            if (tabela == "vendadavos")
            {

                #region

                string sql = @"SELECT count(*) FROM vendadavos";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                   select n.vendadav).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;

                #endregion
            }

            if (tabela == "contprevendaspaf")
            {

                #region

                string sql = @"SELECT count(*) FROM contprevendaspaf";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();
                                               

                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                   select n.contprevendaspaf).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;

                #endregion
            }


            if (tabela == "vendaprevendapaf")
            {
                #region

                string sql = @"SELECT count(*) FROM vendaprevendapaf";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();
                
                                                               

                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                   select n.vendaprevendapaf).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;

                #endregion
            }

            if (tabela == "produtos")
            {
                #region

                string sql = @"SELECT count(*) FROM produtos";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();


                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                   select n.produtos).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;

                #endregion
            }

            if (tabela == "contdocs")
            {

                //var contReg = (from n in Conexao.CriarEntidade().contdocs
                //               select n.documento).Count();
                #region

                string sql = @"SELECT count(*) FROM contdocs";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                   select n.contdocs).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;

                #endregion
            }



            if (tabela == "vendaarquivo")
            {
                #region

                string sql = @"SELECT count(*) FROM vendaarquivo";
                 var contador  = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                 int contReg = contador.FirstOrDefault();
                   
                
                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                   select n.vendaarquivo).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;

                #endregion
            }

            if (tabela == "caixaarquivo")
            {
                #region
                string sql = @"SELECT count(*) FROM caixaarquivo";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                   select n.caixaarquivo).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;

                #endregion
            }

            if (tabela == "caixaDH")
            {
                #region
                string sql = @"SELECT COUNT(*) FROM caixaarquivo WHERE tipopagamento = 'DH'";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                sql = @"SELECT caixaDH FROM quantidaderegistros";
                var criptQtdReg = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql);

                string criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg.FirstOrDefault() != criptQtdregAtual)
                    return false;
                #endregion
            }

            if (tabela == "caixaCR")
            {
                #region
                string sql = @"SELECT COUNT(*) FROM caixaarquivo WHERE tipopagamento = 'CR'";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                sql = @"SELECT caixaCR FROM quantidaderegistros";
                var criptQtdReg = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql);

                string criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg.FirstOrDefault() != criptQtdregAtual)
                    return false;
                #endregion
            }

            if (tabela == "caixaCA")
            {
                #region
                string sql = @"SELECT COUNT(*) FROM caixaarquivo WHERE tipopagamento = 'CA'";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                sql = @"SELECT caixaCA FROM quantidaderegistros";
                var criptQtdReg = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql);

                string criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg.FirstOrDefault() != criptQtdregAtual)
                    return false;

                #endregion
            }

            if (tabela == "caixaCH")
            {
                #region
                string sql = @"SELECT COUNT(*) FROM caixaarquivo WHERE tipopagamento = 'CH'";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                sql = @"SELECT caixaCH FROM quantidaderegistros";
                var criptQtdReg = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql);

                string criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg.FirstOrDefault() != criptQtdregAtual)
                    return false;
                #endregion
            }

            if (tabela == "caixaOutros")
            {
                #region
                string sql = @"SELECT COUNT(*) FROM caixaarquivo WHERE tipopagamento <> 'DH' AND tipopagamento <> 'CR' AND tipopagamento <> 'CA' AND tipopagamento <> 'CH' AND tipopagamento <> 'SI' AND tipopagamento <> 'SU'";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                sql = @"SELECT caixaOutros FROM quantidaderegistros";
                var criptQtdReg = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql);

                string criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg.FirstOrDefault() != criptQtdregAtual)
                    return false;
                #endregion
            }

            if (tabela == "contrelatoriogerencial")
            {
                #region
                var contReg = (from n in Conexao.CriarEntidade().contrelatoriogerencial
                               select n.inc).Count();

                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                   select n.contrelatoriogerencial).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((from n in Conexao.CriarEntidade().contrelatoriogerencial
                                                                select n.inc).Count().ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;
                #endregion
            }

            if (tabela == "r01")
            {
                #region
                string sql = @"SELECT count(*) FROM r01";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                   select n.r01).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;

                #endregion
            }

            if (tabela == "r02")
            {
                #region
                string sql = @"SELECT count(*) FROM r02";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                   select n.r02).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;

                #endregion
            }

            if (tabela == "r03")
            {
                #region
                string sql = @"SELECT count(*) FROM r03";
                var contador = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql);
                int contReg = contador.FirstOrDefault();

                var criptQtdReg = (from n in Conexao.CriarEntidade().quantidaderegistros
                                   select n.r03).First();

                var criptQtdregAtual = Funcoes.CriptografarMD5((contReg).ToString());

                if (criptQtdReg != criptQtdregAtual)
                    return false;
                #endregion
            }

            return true;
        }

        public static void DadosSoftwareHouse()
        {
            string versao = "0.0.0.0";
            try
            {
                System.Diagnostics.FileVersionInfo fvi;
                fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(@"sicepdv.exe");
                versao = fvi.FileVersion.ToString();
            }
            catch(Exception erro)
            {
                
            }

            GlbVariaveis.nomeAplicativo = "SICEpdv.net";
            GlbVariaveis.nomeSH = "IQ SISTEMAS";
            GlbVariaveis.razaoSH = "IVAN QUIRINO DE LIMA - ME";
            GlbVariaveis.cnpjSH = "03590277000100";
            GlbVariaveis.IESH = "ISENTO";
            GlbVariaveis.IEMunicipalSH="1505";
            GlbVariaveis.enderecoSH = "RUA HELENA RODRIGUES PORTO, 20 1o.ANDAR";
            GlbVariaveis.cepSH = "56510040";
            GlbVariaveis.cidadeSH = "ARCOVERDE";
            GlbVariaveis.estadoSH = "PE";
            GlbVariaveis.responsavelSH = "IVAN QUIRINO DE LIMA";
            GlbVariaveis.contatoSH = "WILLIAM SIQUEIRA ALEXANDRE";
            GlbVariaveis.bairroSH = "CENTRO";
            GlbVariaveis.telefoneSH = "8738212715";
            GlbVariaveis.glb_Versao = versao;
            GlbVariaveis.nomeAplicativo = "SICEpdv.net";
            GlbVariaveis.versaoPAF = "0203";
            GlbVariaveis.laudoPAF = "POL0602016";
        }

        public static void ChamarMenuFiscal()
        {
            FrmMenuFiscal frmFiscal = new FrmMenuFiscal();
            frmFiscal.ShowDialog();
        }

        public static string CodNacionaciolECF()
        {
            string codECF = "";
            try
            {
                XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");
                var config = from n in doc.Descendants("ecf")
                             where n.Attribute("numeroFabricacaoCriptografado").Value == (Funcoes.CriptografarComSenha(ConfiguracoesECF.nrFabricacaoECF, GlbVariaveis.glbSenhaIQ)) || n.Attribute("numeroFabricacaoCriptografado").Value == Funcoes.CriptografarComSenha(ConfiguracoesECF.nrFabricacaoECF.Substring(0, 15), GlbVariaveis.glbSenhaIQ)
                             select n;

                codECF = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(config.Attributes("codigoNacionalECF").First().Value), GlbVariaveis.glbSenhaIQ);
            }
            catch (Exception erro)
            {
                throw new Exception("Erro obtendo a série do ECF, exclua o arquivo ArquivoCadastroECF.xml e refaça-o com o configpaf: " + erro.Message);
            }

            return codECF.Trim();

        }
        
    }
}
