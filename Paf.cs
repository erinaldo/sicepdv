using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using System.Data.EntityClient;
using System.Data;
using System.Data.Objects;

namespace SICEpdv
{
    class Paf:FuncoesECF
    {
       public string arquivoDestino = "";
        /// <summary>
        /// Para o registro do PAF é usado é usado o ATO COTEPE 06/08 http://www.fazenda.gov.br/confaz/confaz/atos/atos_cotepe/2008/ac006_08.htm
        /// Para o movimento
        /// </summary>
        /// <returns></returns>
        
        public bool GravarRelatorioR(bool movimentoAnterior=false)
        {            // Código Nacional de Identificação do ECF 
            // http://www.fazenda.mg.gov.br/empresas/ecf/informacoes/TABNCIEE_por_marca.pdf
            siceEntities entidade = Conexao.CriarEntidade();

            // Abrir Configuração da Identificação da SHOUSE
            //XDocument idSHouse = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\sHouse.xml");

            //var dados = (from n in idSHouse.Descendants("sHouse").Elements("Dados")
            //             select n).First();

            //if (dados.IsEmpty)
            //    throw new Exception("Não foi possível ler o arquivo de Identificação da SHouse");

            if (ConfiguracoesECF.NFC == true)
                return true;

            XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");

            string codECF = "";
            try
            {              
                var config = from n in doc.Descendants("ecf")
                             where n.Attribute("numeroFabricacaoCriptografado").Value == (Funcoes.CriptografarComSenha(ConfiguracoesECF.nrFabricacaoECF, GlbVariaveis.glbSenhaIQ)) || n.Attribute("numeroFabricacaoCriptografado").Value == Funcoes.CriptografarComSenha(ConfiguracoesECF.nrFabricacaoECF.Substring(0, 15), GlbVariaveis.glbSenhaIQ)  
                             select n;

                 codECF = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(config.Attributes("codigoNacionalECF").First().Value), GlbVariaveis.glbSenhaIQ);
            }
            catch (Exception erro)
            {
                throw new Exception("Erro obtendo a série do ECF, exclua o arquivo ArquivoCadastroECF.xml e refaça-o com o configpaf: "+erro.Message);
            }
                       

            StringBuilder conteudo = new StringBuilder();
  
            // Obtendo Informações Gerais do ECF 

            

            string nrFabricacaoECF = ConfiguracoesECF.nrFabricacaoECF; // NumeroFabricacaoECF();
            string mfAdicional = DataHoraGravacaoUsuarioMF("MFAdicional").PadRight(1, '1').Substring(0, 1);
            LogSICEpdv.Registrarlog("DataHoraGravacaoUsuarioMF('MFAdicional').PadRight(1, '1').Substring(0, 1)", mfAdicional, "Paf.cs");

            if (string.IsNullOrEmpty(mfAdicional.Trim()))
                mfAdicional = "1";

            string tipoEcf = MarcaModeloTipoECF("Tipo") ?? " ";
            string marcaECF = MarcaModeloTipoECF("Marca") ?? " ";
            string modeloECF = MarcaModeloTipoECF("Modelo") ?? " ";

            string versaoSB = VersaoSoftwareECF() ?? " ";
            string numeroECF = FuncoesECF.NumeroECF(ConfiguracoesECF.idECF).ToString();
            List<String> aliquotasECFICMS = RetornarAliquotasECF("ICMS");

            string dadosSoftwareSB = FuncoesECF.DataHoraGravacaoUsuarioMF("Software") ?? " ";
            string dataInstalacaoSB = Funcoes.SetLength(8);
            string horaInstalacaoSB = Funcoes.SetLength(6);
            if (dadosSoftwareSB != null && dadosSoftwareSB != "")
            {
               LogSICEpdv.Registrarlog("if (dadosSoftwareSB != null && dadosSoftwareSB != '')", "true", "Paf.cs");
                try
                {
                    dataInstalacaoSB = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(dadosSoftwareSB).Date);
                    horaInstalacaoSB = string.Format("{0:hhmmss}", Convert.ToDateTime(dadosSoftwareSB).TimeOfDay);
                }
                catch
                {
                    dataInstalacaoSB = String.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                    horaInstalacaoSB = string.Format("{0:hhmmss}", DateTime.Now.Date);
                }
            }

            LogSICEpdv.Registrarlog("if (dadosSoftwareSB != null && dadosSoftwareSB != '')", "false", "Paf.cs");

            string cnpjECF = CNPJIEUsuarioECF("CNPJ") ?? " ";
            string ieECF = CNPJIEUsuarioECF("IE") ?? " ";
            string numeroUsuarioSubECF = UsuarioSubstituicaoECF().PadLeft(2).Substring(0, 2) ?? " ";
            decimal totalizadorAcrescimoICMS = 0;
            decimal totalizadorAcrescimoISS = 0;
            decimal OPNF = 0; //Totalizador de operações Não Fiscais ! 
            // dados60M -> de uma List na seguinte ordem como 
            /*
             * 0 - COO Inicial
             * 1 - COO Final
             * 2 - ContadorRelatorioGerencial de Reduções
             * 3 - Reinicio de Operacao
             * 4 - VEnda Bruta
             * 5 - Totalizador Geral
             */

            DateTime dataUltimaReducao = DateTime.Now.Date;
            TimeSpan horaUltimaReducao = DateTime.Now.TimeOfDay;


            try
            {
                 dataUltimaReducao = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy hh:mm:ss}", DataUltimaReduzacaoZ()));
                 horaUltimaReducao = Convert.ToDateTime(dataUltimaReducao).TimeOfDay;

                LogSICEpdv.Registrarlog("dataUltimaReducao = Convert.ToDateTime(String.Format('{0:dd/MM/yyyy hh:mm:ss}', DataUltimaReduzacaoZ()));", dataUltimaReducao.ToString(), "Paf.cs");

                LogSICEpdv.Registrarlog("horaUltimaReducao = Convert.ToDateTime(dataUltimaReducao).TimeOfDay", horaUltimaReducao.ToString(), "Paf.cs");
            }
            catch
            {
                 dataUltimaReducao = DateTime.Now.Date;
                 horaUltimaReducao = DateTime.Now.TimeOfDay;

                 LogSICEpdv.Registrarlog("dataUltimaReducao = DateTime.Now.Date", dataUltimaReducao.ToString(), "Paf.cs");

                 LogSICEpdv.Registrarlog("horaUltimaReducao = DateTime.Now.TimeOfDay", horaUltimaReducao.ToString(), "Paf.cs");
            }

            decimal gtInicial = Convert.ToDecimal(FuncoesECF.GrandeTotal());
            LogSICEpdv.Registrarlog(" decimal gtInicial = Convert.ToDecimal(FuncoesECF.GrandeTotal());", gtInicial.ToString(), "Paf.cs");
            decimal vendaLiquida = 0;
            try
            {
                vendaLiquida = FuncoesECF.VendaLiquidaDiaECF();
                LogSICEpdv.Registrarlog("vendaLiquida = FuncoesECF.VendaLiquidaDiaECF();", vendaLiquida.ToString(), "Paf.cs");
            }
            catch
            {
                vendaLiquida = 0;
                LogSICEpdv.Registrarlog("vendaLiquida = 0;", "0", "Paf.cs");
            }

            // Sempre retornará a data da última redução antes de abrir qualquer outro moivmenot
            DateTime dataMovimento = dataUltimaReducao.Date;

            string arquivo = codECF.Trim() + ConfiguracoesECF.nrFabricacaoECF.PadLeft(20, '0').Substring(6, 14) + String.Format("{0:ddMMyyyy}", dataMovimento) + ".txt";
            LogSICEpdv.Registrarlog("Criar arquivo " + arquivo, "true", "Paf.cs");
            

            
            if (ConfiguracoesECF.zPendente)
            {
                LogSICEpdv.Registrarlog(" if (ConfiguracoesECF.zPendente)", "true", "Paf.cs");
                movimentoAnterior = true;
            }
            LogSICEpdv.Registrarlog(" if (ConfiguracoesECF.zPendente)", "false", "Paf.cs");


            if (movimentoAnterior)
            {
                try
                {

                    var dataUltDoc = (from n in Conexao.CriarEntidade().contdocs
                                      where n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                                      orderby n.data descending
                                      select n).ToList().Take(5);


                    //DateTime data = dataUltDoc.FirstOrDefault().data.Value;


                    //LogSICEpdv.Registrarlog((dataUltDoc as ObjectQuery).ToTraceString(), dataUltDoc.FirstOrDefault().data.ToString(), "Paf.cs");
                    //dataUltDoc.FirstOrDefault().data.HasValue

                    if (dataUltDoc.FirstOrDefault() != null)
                    {


                        LogSICEpdv.Registrarlog("if (dataUltDoc.FirstOrDefault().data.HasValue)", "true", "Paf.cs");
                        dataMovimento = dataUltDoc.FirstOrDefault().data.Value;
                        //dataMovimento = ConfiguracoesECF.dataUltMovimentoECF;
                        arquivo = codECF.Trim() + ConfiguracoesECF.nrFabricacaoECF.Trim().PadLeft(20, '0').Substring(6, 14).Trim() + String.Format("{0:ddMMyyyy}", dataMovimento.Date) + ".txt";
                        LogSICEpdv.Registrarlog("Criar arquivo " + arquivo, "true", "Paf.cs");

                    }
                    else
                    {
                        dataMovimento = DateTime.Now.Date;
                    }

                    LogSICEpdv.Registrarlog("if (dataUltDoc.FirstOrDefault().data.HasValue)", "false", "Paf.cs");

                }
                catch(Exception erro)
                {
                    MessageBox.Show(erro.ToString());
                }
            }


            
            // Para evitar erro no nome do arquivo com espaco
            arquivo = arquivo.Replace(" ", "");
            
            
            if (ConfiguracoesECF.idECF == 2 && GlbVariaveis.glb_Acbr == false)
            {
                GravarSintegra("", dataMovimento.Date);
                LogSICEpdv.Registrarlog("GravarSintegra('', dataMovimento.Date)", "", "Paf.cs");
            }            

            // Usando essa outra função por na Daruma Framework nao existe mais os métodos 60M Metres e 60Analitico por isso 
            // o arquivo é gerado apartir do Sintegra. E não é usado o mesmo método por que nos outros fabricante o sintegra é gerando apenas mensalmente.
            if ((ConfiguracoesECF.idECF != 2 && GlbVariaveis.glb_Acbr == false) || GlbVariaveis.glb_Acbr == true)
            {

                LogSICEpdv.Registrarlog(" if ((ConfiguracoesECF.idECF != 2 && GlbVariaveis.glb_Acbr == false) || GlbVariaveis.glb_Acbr == true)", "true", "Paf.cs");

                var dados60M = Relatorio60M();
                var dados60A = Relatorio60A();

                string cOOInicial = "";
                string cOOFinal = "";
                string contadorReducao = "";
                string contadorReinicioOperacao = "";
                decimal vendaBrutaDoDia = 0;
                decimal totalizadorGeral = 0;
                decimal valorICMS = 0;
                decimal totalICMSDebitado = 0;

                foreach (var item in dados60M)
                {
                    /* Alteracao Feita Por Marckvaldo no dia 11/10/2012
                     * O Arquivo de retorno gerado Pela Elgin e diferente das outras impressoras
                     * po isso foi criado essa tomada de desisao onde 3 e a impressora elgin
                     * */

                    if (GlbVariaveis.glb_Acbr == false)
                    {
                        #region
                        if (ConfiguracoesECF.idECF == 3)
                        {
                            if (item.Contains("COO I"))
                                cOOInicial = item.ToString().Substring(7, 8).Trim();
                            if (item.Contains("COO F"))
                                cOOFinal = item.ToString().Substring(7, 8).Trim();
                            if (item.Contains("Con Red"))
                                contadorReducao = item.ToString().Substring(7, 8).Trim();
                            if (item.Contains("Rei Ope"))
                                contadorReinicioOperacao = item.ToString().Substring(7, 8).Trim();
                            if (item.Contains("V Bruta"))
                                vendaBrutaDoDia = Convert.ToDecimal(item.Substring(7, 8).Replace(" ", ""));
                            if (item.Contains("T Geral"))
                                totalizadorGeral = Convert.ToDecimal(item.Substring(7, 13));
                        }
                        else
                        {

                            if (item.Contains("COO I"))
                                cOOInicial = item.ToString().Substring(7, 24).Trim();
                            if (item.Contains("COO F"))
                                cOOFinal = item.ToString().Substring(7, 24).Trim();
                            if (item.Contains("Con Red"))
                                contadorReducao = item.ToString().Substring(7, 24).Trim();
                            if (item.Contains("Rei Ope"))
                                contadorReinicioOperacao = item.ToString().Substring(7, 24).Trim();
                            if (item.Contains("V Bruta"))
                                vendaBrutaDoDia = Convert.ToDecimal(item.Substring(7, 24));
                            if (item.Contains("T Geral"))
                                totalizadorGeral = Convert.ToDecimal(item.Substring(7, 24));
                        }
                        #endregion
                    }
                    else
                    {
                        
                        if (item.Contains("COO I"))
                            cOOInicial = item.ToString().Replace("COO Ini", "").Trim();
                        if (item.Contains("COO F"))
                            cOOFinal = item.ToString().Replace("COO Fin", "").Trim();
                        if (item.Contains("Con Red"))
                            contadorReducao = item.ToString().Replace("Con Red", "").Trim();
                        if (item.Contains("Rei Ope"))
                            contadorReinicioOperacao = item.ToString().Replace("Rei Ope", "").Trim();
                        if (item.Contains("V Bruta"))
                            vendaBrutaDoDia = Convert.ToDecimal(item.Replace("V Bruta","").Trim());
                        if (item.Contains("T Geral"))
                            totalizadorGeral = Convert.ToDecimal(item.Replace("T Geral", "").Trim());

                        LogSICEpdv.Registrarlog("Lendo Arquivo " + arquivo, "true", "Paf.cs");
                    }
                }
                //cOOInicial = dados60M[0].ToString().Substring(7,24);
                //string cOOFinal = dados60M[1].ToString().Substring(7, 24);
                //string contadorReducao = dados60M[2].ToString().Substring(7, 24);
                //string contadorReinicioOperacao = dados60M[3].ToString().Substring(7, 24);            
                //decimal vendaBrutaDoDia = Convert.ToDecimal(dados60M[4].Substring(7, 24));
                //decimal totalizadorGeral = Convert.ToDecimal(dados60M[5].Substring(7, 24));
                System.Threading.Thread.Sleep(500);
                string cooReducaoZ = "";
                cooReducaoZ = FuncoesECF.ultimaReducaoZ("COO");
                // O COO da Bematech após a redução Z
                //if (ConfiguracoesECF.idECF == 1)

                LogSICEpdv.Registrarlog("cooReducaoZ = cOOFinal;", cooReducaoZ, "Paf.cs");

                //cooReducaoZ = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF);

                var espelhoReducaoZ = FuncoesECF.DownloadDaMFD(cooReducaoZ, cooReducaoZ);

                LogSICEpdv.Registrarlog("var espelhoReducaoZ = FuncoesECF.DownloadDaMFD(cooReducaoZ, cooReducaoZ);", espelhoReducaoZ.ToString(), "Paf.cs");

                foreach (var item in espelhoReducaoZ)
                {
                    if (GlbVariaveis.glb_Acbr == false)
                    {
                        if (item.Contains("Total Oper N") && !item.Contains("***"))
                            OPNF = Convert.ToDecimal(item.Substring(item.Length - 15, 15));
                        if (item.Contains("ACRÉSCIMO ICMS") && !item.Contains("***"))
                            totalizadorAcrescimoICMS = Convert.ToDecimal(item.Substring(item.Length - 15, 15));
                        if (item.Contains("ACRÉSCIMO ISSQN") && !item.Contains("***"))
                            totalizadorAcrescimoISS = Convert.ToDecimal(item.Substring(item.Length - 15, 15));
                        //if (item.Contains("Total ICMS") && !item.Contains("***"))
                        //{
                        //    valorICMS = Convert.ToDecimal(item.Substring(item.Length - 15, 15));
                        //    totalICMSDebitado = Convert.ToDecimal(item.Substring(12, 18));
                        //}
                    }
                    else
                    {
                        if (item.Contains("Total Oper N") && !item.Contains("***") && !item.Contains("-1"))
                            OPNF = Convert.ToDecimal(item.Replace("Total Oper N", "").Trim());
                        if (item.Contains("ACRÉSCIMO ICMS") && !item.Contains("***") && !item.Contains("-1"))
                            totalizadorAcrescimoICMS = Convert.ToDecimal(item.Replace("ACRÉSCIMO ICMS", "").Trim());
                        if (item.Contains("ACRÉSCIMO ISSQN") && !item.Contains("***") && !item.Contains("-1"))
                            totalizadorAcrescimoISS = Convert.ToDecimal(item.Replace("ACRÉSCIMO ISSQN", "").Trim());
                    }
                };



                foreach (var item in dados60A)
                {
                    /*Alteracao Feita Por Marckvaldo  no Dia 11/10/2012*/

                    if (GlbVariaveis.glb_Acbr == false)
                    {
                        if (ConfiguracoesECF.idECF == 3)
                        {
                            if (item.Contains("V ICMS "))
                                valorICMS = Convert.ToDecimal(item.Substring(6, 18));
                            if (item.Contains("T ICMS "))
                                totalICMSDebitado = Convert.ToDecimal(item.Substring(6, 18));
                        }
                        else
                        {

                            if (item.Contains("V ICMS "))
                                valorICMS = Convert.ToDecimal(item.Substring(7, 24));
                            if (item.Contains("T ICMS "))
                                totalICMSDebitado = Convert.ToDecimal(item.Substring(7, 24));
                        }
                    }
                    else
                    {
                        if (item.Contains("V ICMS "))
                            valorICMS = Convert.ToDecimal(item.Replace("V ICMS", "").Trim());
                        if (item.Contains("T ICMS "))
                            totalICMSDebitado = Convert.ToDecimal(item.Replace("T ICMS", "").Trim());
                    }
                }

                // Arquivo de Criacao            

                // Gravando SEF / Sintegra
                // 60 M - Mapa Resumo Mestre
                C60m ecf60m = new C60m();
                ecf60m.codigofilial = GlbVariaveis.glb_filial;
                ecf60m.tipo = "60";
                ecf60m.subtipo = "M";
                ecf60m.origem = "ECF";
                ecf60m.data = dataMovimento;
                ecf60m.hora = horaUltimaReducao;
                ecf60m.ECFnumeroserie = nrFabricacaoECF;
                ecf60m.ECFnumero = numeroECF;
                ecf60m.modeloDocFiscal = "2D";
                ecf60m.contadorinicial = cOOInicial.Trim().PadLeft(6, '0'); ;
                ecf60m.contadorfinal = cOOFinal.Trim().PadLeft(6, '0'); ;
                ecf60m.numeroreducaoZ = contadorReducao.Trim().PadLeft(4, '0');
                ecf60m.contadorreinicio = contadorReinicioOperacao.Trim().PadLeft(4, '0'); ;
                ecf60m.vendabruta = vendaBrutaDoDia;
                ecf60m.totalgeralECF = totalizadorGeral;
                ecf60m.gtinicialdia = gtInicial;
                ecf60m.vendaliquida = vendaLiquida;
                ecf60m.TotalICMSdebitado = totalICMSDebitado;
                ecf60m.ValorICMS = valorICMS;
                entidade.AddToC60m(ecf60m);

                // Gravando SEF / Sintegra
                // 60 A - Mapa Resumo Analítico situação tributária
                string aliquota = "";
                decimal totalAliquota = 0M;
                foreach (var item in dados60A)
                {
                    int idFinal = item.Length - 7;//<-------- Alteracao feito por Marckvaldo no dia 11/10/2012

                    aliquota = item.ToString().Substring(0, 7); // Aqui pega os 7 primeiros digite Exemp Canc-T
                    totalAliquota = Convert.ToDecimal(item.Substring(7, idFinal)); // Aqui pega depois dos 7 primeiros digitos para transformar a string em decimal
                    aliquota = "";
                    if (item.Contains("Can-T  "))
                        aliquota = "CANC";
                    if (item.Contains("DT     "))
                        aliquota = "DESC";
                    if (item.Contains("F1     "))
                        aliquota = "F";
                    if (item.Contains("I1     "))
                        aliquota = "I";
                    if (item.Contains("N1     "))
                        aliquota = "N";
                    if ((item.PadLeft(1, ' ').Substring(0, 1).Contains("0") || item.PadLeft(1, ' ').Substring(0, 1).Contains("1") || 
                        item.PadLeft(1, ' ').Substring(0, 1).Contains("2") || item.PadLeft(1, ' ').Substring(0, 1).Contains("3") ||
                        item.PadLeft(1, ' ').Substring(0, 1).Contains("4") || item.PadLeft(1, ' ').Substring(0, 1).Contains("5") ||
                        item.PadLeft(1, ' ').Substring(0, 1).Contains("6") || item.PadLeft(1, ' ').Substring(0, 1).Contains("6")) && (item != "" && item != null))
                    {
                        if (GlbVariaveis.glb_Acbr == false)
                            aliquota = item.Substring(3, 4);
                        else
                            aliquota = item.Substring(2, 4);
                        
                    }
                    if (aliquota != "")
                    {
                        C60a ecf60a = new C60a();
                        ecf60a.codigofilial = GlbVariaveis.glb_filial;
                        ecf60a.tipo = "60";
                        ecf60a.subtipo = "A";
                        ecf60a.data = dataMovimento;
                        ecf60a.hora = horaUltimaReducao;
                        ecf60a.ECFnumeroserie = nrFabricacaoECF;
                        ecf60a.ecfnumero = numeroECF;
                        ecf60a.aliquotaICMS = aliquota;
                        ecf60a.acumuladoTotalizadorParcial = totalAliquota;
                        entidade.AddToC60a(ecf60a);
                    };
                };


                #region R01

                var dadosECF = (from n in Conexao.CriarEntidade().r01
                                where n.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                select n);

                if (dadosECF.Count()==0 || dadosECF==null)
                {

                r01 r1 = new r01();
                r1.codigofilial = GlbVariaveis.glb_filial;
                r1.tipo = "R01"; //01
                r1.fabricacaoECF = nrFabricacaoECF; //02
                r1.MFAdicional = mfAdicional; //03
                r1.tipoECF = tipoEcf; //05
                r1.marcaECF = marcaECF; //06
                r1.modeloECF = modeloECF; //07
                r1.versaoSB = versaoSB;
                if (dataInstalacaoSB != Funcoes.SetLength(8))
                {
                    try
                    {
                        r1.datainstalacaoSB = Convert.ToDateTime(dadosSoftwareSB).Date; // Convert.ToDateTime(string.Format("{0:yyyy/MM/dd}", dataInstalacaoSB));
                        r1.horainstalacaoSB = Convert.ToDateTime(dadosSoftwareSB).TimeOfDay; // Convert.ToDateTime(horaInstalacaoSB).TimeOfDay;
                    }
                    catch
                    {
                        r1.datainstalacaoSB = DateTime.Now.Date;
                        r1.horainstalacaoSB = DateTime.Now.TimeOfDay;
                    }
                }
                r1.numeroECF = FuncoesECF.NumeroECF(ConfiguracoesECF.idECF).ToString();
                r1.numeroUsuarioSubstituicaoECF = numeroUsuarioSubECF;
                r1.cnpj = Funcoes.RetirarFormatacaoCNPJ_CPF_IE(cnpjECF);
                r1.inscricao = Funcoes.RetirarFormatacaoCNPJ_CPF_IE(ieECF);

                r1.cnpjdesenvolvedora = GlbVariaveis.cnpjSH;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("cnpj").First().Value), GlbVariaveis.glbSenhaIQ);
                r1.inscricaodesenvolvedora = GlbVariaveis.IESH;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("inscricao").First().Value), GlbVariaveis.glbSenhaIQ);
                r1.inscricaomunicipaldesenvolvedora = GlbVariaveis.IEMunicipalSH;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("inscricaoMunicipal").First().Value), GlbVariaveis.glbSenhaIQ);
                r1.razaosocialdesenvolvedora = GlbVariaveis.razaoSH;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("razaoSocial").First().Value), GlbVariaveis.glbSenhaIQ);
                r1.aplicativo = GlbVariaveis.nomeAplicativo;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("aplicativo").First().Value), GlbVariaveis.glbSenhaIQ);
                r1.versao = GlbVariaveis.glb_Versao; // Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("versao").First().Value), GlbVariaveis.glbSenhaIQ);
                r1.md5 = ConfiguracoesECF.md5Geral;// (dados.Elements("md5").First().Value); // Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("md5").First().Value), GlbVariaveis.glbSenhaIQ);
                r1.md5exe = ConfiguracoesECF.md5PrincipalEXE;
                r1.versaoERPAF = GlbVariaveis.versaoPAF;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("versaoERPAF").First().Value), GlbVariaveis.glbSenhaIQ);
                r1.data = dataMovimento;
                entidade.AddTor01(r1);
                entidade.SaveChanges();
                    }



                #endregion

                LogSICEpdv.Registrarlog("Salvo R01", "true", "Paf.cs");

                #region R02
                r02 r2 = new r02();
                r2.tipo = "R02";
                r2.codigofilial = GlbVariaveis.glb_filial;
                r2.data = dataMovimento;
                r2.datamovimento = dataMovimento;
                r2.fabricacaoECF = nrFabricacaoECF;
                r2.MFadicional = mfAdicional;
                r2.modeloECF = modeloECF;
                r2.numeroECF = FuncoesECF.NumeroECF(ConfiguracoesECF.idECF).ToString();
                r2.numeroUsuarioSubstituicaoECF = numeroUsuarioSubECF;
                r2.crz = contadorReducao.Trim().PadLeft(6, '0');
                r2.coo = cooReducaoZ.Trim().PadLeft(6, '0'); //cOOFinal.Trim().PadLeft(6, '0');
                r2.cro = contadorReinicioOperacao.Trim().PadLeft(6, '0');
                r2.dataemissaoreducaoz = dataUltimaReducao;
                r2.horaemissaoreducaoz = horaUltimaReducao;
                r2.vendabrutadiaria = vendaBrutaDoDia;
                r2.gtfinal = totalizadorGeral;
                r2.parametroISSQNdesconto = "N";
                r2.EADdados = Funcoes.CriptografarMD5(r2.fabricacaoECF + r2.crz + r2.coo + r2.cro + string.Format("{0:yyyy-MM-dd}", r2.data) + string.Format("{0:yyyy-MM-dd}", r2.dataemissaoreducaoz) + r2.horaemissaoreducaoz + Funcoes.FormatarDecimal(r2.vendabrutadiaria.ToString(),2).ToString().Replace(".","").Replace(",", "."));
                entidade.AddTor02(r2);
                entidade.SaveChanges();

                //conteudo.AppendLine("R02" +
                //    r2.fabricacaoECF.Trim().PadRight(20, ' ').Substring(0, 20) +
                //    r2.MFadicional.Trim().PadRight(1, ' ').Substring(0, 1) +
                //    r2.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20) +
                //    r2.numeroUsuarioSubstituicaoECF.Trim().PadRight(2, '0').Substring(0, 2) +
                //    r2.crz.Trim().PadRight(6, '0').Substring(0, 6) +
                //    r2.coo.Trim().PadRight(6, '0').Substring(0, 6) +
                //    r2.cro.Trim().PadRight(6, '0').Substring(0, 6) + //08 Contador reinicio de operacao
                //    String.Format("{0:yyyyMMdd}", r2.data) +
                //    String.Format("{0:yyyyMMdd}", r2.dataemissaoreducaoz) +
                //    String.Format("{0:hhmmss}", r2.horaemissaoreducaoz).Replace(":","") +
                //    Funcoes.FormatarZerosEsquerda(r2.vendabrutadiaria,14,true)+
                //    "N");            
                #endregion R02

                LogSICEpdv.Registrarlog("Salvo R02", "true", "Paf.cs");

                #region R03
                //Adicionando as informações restante
                List<string> complementoR03 = new List<string>();
                complementoR03.Add("OPNF                    " + OPNF.ToString());
                complementoR03.Add("AT                      " + totalizadorAcrescimoICMS.ToString());
                complementoR03.Add("AS                      " + totalizadorAcrescimoISS.ToString());

                var filtroR03 = (from n in dados60A
                                 where !n.Contains("V ICMS")
                                 && !n.Contains("T ICMS")
                                 select n).ToList();

                filtroR03.AddRange(complementoR03);

                foreach (var item in filtroR03)
                {
                    r03 r3 = new r03();
                    r3.data = dataMovimento;
                    r3.codigofilial = GlbVariaveis.glb_filial;
                    r3.tipo = "R03";
                    r3.fabricacaoECF = nrFabricacaoECF;
                    r3.MFAdicional = mfAdicional;
                    r3.modeloECF = modeloECF;
                    r3.numeroECF = FuncoesECF.NumeroECF(ConfiguracoesECF.idECF).ToString();
                    r3.numeroUsuarioSubstituicaoECF = numeroUsuarioSubECF;
                    r3.CRZ = contadorReducao.Trim().PadLeft(6, '0');
                    r3.totalizadorParcial = item.ToString().Substring(0, 7); // Aqui pega os 7 primeiros digite Exemp Canc-T
                    r3.valoracumulado = Convert.ToDecimal(item.Substring(7, item.Length - 7)); // Aqui pega depois dos 7 primeiros digitos para transformar a string em decimal                
                    r3.EADdados = Funcoes.CriptografarMD5(r3.fabricacaoECF + r3.CRZ);
                    entidade.AddTor03(r3);
                    entidade.SaveChanges();

                    //conteudo.AppendLine("R03" +
                    //    r3.fabricacaoECF.Trim().PadRight(20, ' ').Substring(0, 20) +
                    //    r3.MFAdicional.Trim().PadRight(1, ' ').Substring(0, 1) +
                    //    r3.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20) +
                    //    r3.numeroUsuarioSubstituicaoECF.Trim().PadRight(2, '0').Substring(0, 2) +
                    //    r3.CRZ.Trim().PadRight(6, '0').Substring(0, 6) +
                    //    r3.totalizadorParcial.Trim().PadRight(7, ' ').Substring(0, 7) +
                    //    Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(r3.valoracumulado), 13,true));
                }
                entidade.SaveChanges();
                #endregion

                LogSICEpdv.Registrarlog("Salvo R03", "true", "Paf.cs");
            }

            LogSICEpdv.Registrarlog(" if ((ConfiguracoesECF.idECF != 2 && GlbVariaveis.glb_Acbr == false) || GlbVariaveis.glb_Acbr == true)", "false", "Paf.cs");

// Gravando a EAD


            using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
            {
                conn.Open();
                EntityCommand cmd = conn.CreateCommand();
                cmd.CommandText = "siceEntities.GravarR";
                cmd.CommandType = CommandType.StoredProcedure;

                EntityParameter dataGravar = cmd.Parameters.Add("dataMovimento", DbType.Date);
                dataGravar.Direction = ParameterDirection.Input;
                dataGravar.Value = dataMovimento.Date;

                cmd.ExecuteNonQuery();
                conn.Close();

                LogSICEpdv.Registrarlog("siceEntities.GravarR Paramentro" + dataMovimento.Date.ToShortDateString(), "true", "Paf.cs");
            };

           // Gerando o txt da movimentacao  
            bool criarTabelaVenda = false;
            if (movimentoAnterior)
            {
                LogSICEpdv.Registrarlog("if (movimentoAnterior)", movimentoAnterior.ToString(), "Paf.cs");
                criarTabelaVenda = true;
                LogSICEpdv.Registrarlog("if (movimentoAnterior)", movimentoAnterior.ToString(), "Paf.cs");
            }

            LogSICEpdv.Registrarlog("if (movimentoAnterior)", movimentoAnterior.ToString(), "Paf.cs");

            GerarMovimentoPorECF(criarTabelaVenda, dataMovimento.Date, dataMovimento.Date, numeroECF, @ConfigurationManager.AppSettings["dirMovimentoECF"] + @"\" + @arquivo);

            return true;
        }
        /// <summary>
        /// Como se acha o número de ECF no caixa
        /// o número de ECF bem como o GNF e o CCO estao no historico dessa forma
        /// E:001G:000518C:001001 para para obter o numero do ECF por exemplo use historico.Substring(2, 3)
        /// </summary>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        /// <param name="numerodoECF"></param>
        /// <returns></returns>
        /// GerarMovimentoPorECF Existe tanto por data e pelo numero do ECF como apenas por Data e pegando todos
        /// os ECFS. Estou usando sobrecarga de métodos por isso tem 2 métodos com o mesmo nome GerarMovimentoPorECF mas
        /// com parametros diferentes
        public bool GerarMovimentoPorECF(bool criarTabelaVenda, DateTime dataInicial,DateTime dataFinal,string numerodoECF,string nomeArquivoDestino)
        {

            arquivoDestino = nomeArquivoDestino;
            StringBuilder conteudo = new StringBuilder();
            PAFArquivos registrosPAF = new PAFArquivos();

            FrmMsgOperador status = new FrmMsgOperador("", "Gerando Arquivo U1");
            status.Show();
            Application.DoEvents();
            conteudo = registrosPAF.RegistroU1("");
            status.Dispose();

            status = new FrmMsgOperador("", "Gerando Arquivo A2");
            status.Show();
            Application.DoEvents();

            if(GlbVariaveis.glb_clienteDAV == true)
                conteudo.Append(registrosPAF.A2(dataInicial.Date, dataFinal.Date));

            status.Dispose();

            status = new FrmMsgOperador("", "Gerando Arquivo P2 Varejo");
            status.Show();
            Application.DoEvents();

            if (GlbVariaveis.glb_clienteDAV == true)
                conteudo.Append(registrosPAF.P2("varejo"));

            status.Dispose();

            status = new FrmMsgOperador("", "Gerando Arquivo P2 Atacado");
            status.Show();
            Application.DoEvents();

            if (GlbVariaveis.glb_clienteDAV == true)
                conteudo.Append(registrosPAF.P2("atacado"));

            status.Dispose();

            status = new FrmMsgOperador("", "Gerando Arquivo E2");
            status.Show();
            Application.DoEvents();

            if (GlbVariaveis.glb_clienteDAV == true)
                conteudo.Append(registrosPAF.E2("total"));

            status.Dispose();

            status = new FrmMsgOperador("", "Gerando Arquivo E3");
            status.Show();
            Application.DoEvents();

            if (GlbVariaveis.glb_clienteDAV == true)
                conteudo.Append(registrosPAF.E3());

            status.Dispose();

            status = new FrmMsgOperador("", "Gerando Arquivo D2");
            status.Show();
            Application.DoEvents();

            if (GlbVariaveis.glb_clienteDAV == true)
                conteudo.Append(registrosPAF.D2(dataInicial.Date, dataFinal.Date));

            status.Dispose();

            status = new FrmMsgOperador("", "Gerando Arquivo D4 Varejo");
            status.Show();
            Application.DoEvents();

            if (GlbVariaveis.glb_clienteDAV == true)
                conteudo.Append(registrosPAF.D4(dataInicial.Date, dataFinal.Date));

            status.Dispose();

            status = new FrmMsgOperador("", "Gerando Arquivo Gerar R");
            status.Show();
            Application.DoEvents();
            conteudo.Append(registrosPAF.GerarR(criarTabelaVenda, dataInicial.Date, dataFinal.Date, numerodoECF, nomeArquivoDestino));
            status.Dispose();


            try
            {

                using (FileStream fs = File.Create(@nomeArquivoDestino))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        sw.Write(@conteudo);
                    }
                };

                LogSICEpdv.Registrarlog("Arquivo movimento gerado com sucesso!", "true", "Paf.cs");
            }
            catch (Exception ex)
            {
                LogSICEpdv.Registrarlog("Arquivo movimento gerado com sucesso!", "false", "Paf.cs");
                throw new Exception("Erro gravando arquivo do movimento " + ex.Message);
            }

            FuncoesECF.AssinarArquivo(@nomeArquivoDestino, false);
            LogSICEpdv.Registrarlog("Arquivo movimento gerado Assinado com sucesso!", "true", "Paf.cs");

            return true;

        }

        public static bool RetirarEAD(string arquivo)
        {
            using (FileStream fs = new FileStream(arquivo, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    using (FileStream fsTemp = new FileStream("arqMDF.tmp", FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fsTemp))
                        {
                            string linha = null;
                            while ((linha = sr.ReadLine()) != null)
                            {
                                if (linha.IndexOf("EAD") > -1)
                                {
                                    linha = "";
                                }
                                if (linha!="")
                                sw.Write(linha + "\r\n");
                            }
                        }
                    }
                }
            }
            File.Delete(arquivo);
            File.Move("arqMDF.tmp", arquivo);
            return true;
        }

        public static bool AcrescentarRegistro61Sintegra(string arquivo)
        {
            int qtdlinhaReg61 = 0;
            int qtdlinhaArquivo = 0;
            StringBuilder conteudoReg61 = new StringBuilder();
            string caminhoArquivo61 = @Application.StartupPath + @"\" + ConfigurationManager.AppSettings["dirArquivosPAF"] + @"\registro61.txt";

            if (!File.Exists(@caminhoArquivo61))
                return true;

            FileStream Arquivo61 = new FileStream(caminhoArquivo61, FileMode.Open);

            using (StreamReader ler = new StreamReader(Arquivo61))
            {
                using (FileStream fsTemp = new FileStream("registro61.tmp", FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fsTemp))
                    {
                        string linha = null;
                        while ((linha = ler.ReadLine()) != null)
                        {
                            qtdlinhaReg61++;
                            conteudoReg61.AppendLine(linha.ToString());
                        }
                    }
                }
            }


            using (FileStream fs = new FileStream(arquivo, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    using (FileStream fsTemp = new FileStream("registro61.tmp", FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fsTemp))
                        {
                            string linha = null;
                            while ((linha = sr.ReadLine()) != null)
                            {
                                qtdlinhaArquivo++;
                                if (linha.Substring(0,2) =="75" && conteudoReg61 !=null )
                                {
                                    sw.Write(conteudoReg61);
                                    conteudoReg61 = null;
                                    //linha = "";
                                }
                                //if (linha.Substring(0, 2) == "60")
                                  //  qtdlinhaReg60++;

                                if (linha.Substring(0, 2) == "90")
                                {
                                   // string linhareg60 = Funcoes.FormatarZerosEsquerda(qtdlinhaReg60 + qtdlinhaReg61,8 , false);

                                    //Quantidade Linha do Arquivo

                                    string linha1 = linha.Substring(0, 40);
                                    linha1 += "61" + Funcoes.FormatarZerosEsquerda(qtdlinhaReg61, 8, false);

                                    string linha2 = linha.Substring(40, 12) + Funcoes.FormatarZerosEsquerda(qtdlinhaArquivo+qtdlinhaReg61, 8, false); 
                                    string linha3 = linha.Substring(70, 56);


                                    linha = linha1 + linha2 + linha3; // linha.Substring(0, 33) + linhareg60 + linha.Substring(0, 54) + qtdlinhas + reg902;                                   
                                }

                                    
                                if (linha != "")
                                    sw.Write(linha + "\r\n");
                            }
                        }
                    }
                }
            }
            File.Delete(arquivo);
            File.Move("registro61.tmp", arquivo);
            return true;
        }

        public bool GravarSintegra(string arquivoSintegra,DateTime data)
        {

            //essa função só será chamada quando for uma impressora daruma é quando o pdv não estiver em modo acbr

            siceEntities entidade = Conexao.CriarEntidade();

            string linha;
            C60m ecf60m = new C60m();


            DateTime dataMovimento = data.Date ;
            string numeroSerieECF = "";
            string seqECF = "";
            string modeloDOC = "";
            string COOInicial = "";
            string COOFinal = "";
            string CRZ = "";
            string COOReinicio = "";
            decimal vendaBruta = 0;
            decimal totalizadorGeral = 0;
            string tributacao = "";
            decimal valorACumTotParc = 0;
            decimal GTInicial = FuncoesECF.GrandeTotalInicioDia(true, false);
            decimal vendaLiquida = 0;
            decimal BCICMS = 0;
            decimal valorICMS = 0;

            DateTime dataUltimaReducao = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy hh:mm:ss}", DataUltimaReduzacaoZ()));
            TimeSpan horaUltimaReducao = Convert.ToDateTime(dataUltimaReducao).TimeOfDay;
            
            FuncoesECF.GerarSintegraECF(0, 0, data.Date, data.Date);
            
            
            if (string.IsNullOrEmpty(arquivoSintegra))
                arquivoSintegra = @ConfiguracoesECF.pathRetornoECF + "sintegra.txt";

            // Obtendo Informações Gerais do ECF 

            string nrFabricacaoECF = ConfiguracoesECF.nrFabricacaoECF; // NumeroFabricacaoECF();
            string mfAdicional = DataHoraGravacaoUsuarioMF("MFAdicional").PadRight(1, '1').Substring(0, 1);

            string tipoEcf = MarcaModeloTipoECF("Tipo") ?? " ";
            string marcaECF = MarcaModeloTipoECF("Marca") ?? " ";
            string modeloECF = MarcaModeloTipoECF("Modelo") ?? " ";

            string versaoSB = VersaoSoftwareECF() ?? " ";
            string numeroECF = FuncoesECF.NumeroECF(ConfiguracoesECF.idECF).ToString();
            List<String> aliquotasECFICMS = RetornarAliquotasECF("ICMS");

            string dadosSoftwareSB = FuncoesECF.DataHoraGravacaoUsuarioMF("Software") ?? " ";
            string dataInstalacaoSB = Funcoes.SetLength(8);
            string horaInstalacaoSB = Funcoes.SetLength(6);
            if (dadosSoftwareSB != null && dadosSoftwareSB != "")
            {
                dataInstalacaoSB = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(dadosSoftwareSB).Date);
                horaInstalacaoSB = string.Format("{0:hhmmss}", Convert.ToDateTime(dadosSoftwareSB).TimeOfDay);
            }

            string cnpjECF = CNPJIEUsuarioECF("CNPJ") ?? " ";
            string ieECF = CNPJIEUsuarioECF("IE") ?? " ";
            string numeroUsuarioSubECF = UsuarioSubstituicaoECF().PadLeft(2).Substring(0, 2) ?? " ";
            decimal OPNF = 0;
            decimal totalizadorAcrescimoICMS = 0;
            decimal totalizadorAcrescimoISS = 0;


            string cooReducaoZ = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF);

            string cooInicial = (from n in entidade.contdocs
                                 where n.data == data
                                 && n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                                 select n.ncupomfiscal).Max();

            if (cooInicial == null)
            {

                cooInicial = (from n in entidade.contdocs                             
                              where n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                              select n.ncupomfiscal).Max();
            };

                      
            var espelhoReducaoZ = FuncoesECF.DownloadDaMFD(cooInicial, cooReducaoZ);
            
            
            foreach (var item in espelhoReducaoZ)
            {                
                    if (item.Contains("Total Oper N") && !item.Contains("***"))
                        OPNF = Convert.ToDecimal(item.Substring(item.Length - 15, 15));
                    if (item.Contains("ACRÉSCIMO ICMS") && !item.Contains("***"))
                        totalizadorAcrescimoICMS = Convert.ToDecimal(item.Substring(item.Length - 15, 15));
                    if (item.Contains("ACRÉSCIMO ISSQN") && !item.Contains("***"))
                        totalizadorAcrescimoISS = Convert.ToDecimal(item.Substring(item.Length - 15, 15));
                    //if (item.Contains("Total ICMS") && !item.Contains("***"))
                    //{
                    //    valorICMS = Convert.ToDecimal(item.Substring(item.Length - 15, 15));
                    //    totalICMSDebitado = Convert.ToDecimal(item.Substring(12, 18));
                    //}
               
            };

            System.IO.StreamReader file = new System.IO.StreamReader(arquivoSintegra, Encoding.GetEncoding("ISO-8859-1"));
            while ((linha = file.ReadLine()) != null)
            {
                /* Alteracao feita Por Marckvlado no dia 11/10/2012
                 O Aquivo Retorno Criado pela ELgin E Diferente 
                 * por isso foi criado essa tomada de decisao
                 */
                if (linha.Substring(0, 3) == "60M")
                {
                    dataMovimento = Funcoes.FormatarData(linha.Substring(3, 8));
                    numeroSerieECF = linha.Substring(11, 20);
                    seqECF = linha.Substring(31, 3);
                    modeloDOC = linha.Substring(34, 2);
                    COOInicial = linha.Substring(36, 6);
                    COOFinal = linha.Substring(42, 6);
                    CRZ = linha.Substring(48, 6);
                    COOReinicio = linha.Substring(54, 3);
                    vendaBruta = Convert.ToDecimal(linha.Substring(57, 16)) / 100; ;
                    totalizadorGeral = Convert.ToDecimal(linha.Substring(73, 16)) / 100;


                    ecf60m.codigofilial = GlbVariaveis.glb_filial;
                    ecf60m.tipo = "60";
                    ecf60m.subtipo = "M";
                    ecf60m.origem = "ECF";
                    ecf60m.data = dataMovimento;
                    ecf60m.hora = ConfiguracoesECF.horaUltimaReducaoZ; // horaUltimaReducao;
                    ecf60m.ECFnumeroserie = numeroSerieECF;
                    ecf60m.ECFnumero = seqECF;
                    ecf60m.modeloDocFiscal = "2D";
                    ecf60m.contadorinicial = COOInicial;
                    ecf60m.contadorfinal = COOFinal;
                    ecf60m.numeroreducaoZ = CRZ;
                    ecf60m.contadorreinicio = COOReinicio;
                    ecf60m.vendabruta = vendaBruta;
                    ecf60m.totalgeralECF = totalizadorGeral;
                    ecf60m.gtinicialdia = GTInicial;


                }

                if (linha.Substring(0, 3) == "60A")
                {
                    tributacao = linha.Substring(31, 4);
                    valorACumTotParc = Convert.ToDecimal(linha.Substring(35, 12)) / 100;
                    if (tributacao != "CANC" && tributacao != "DESC")
                        vendaLiquida += valorACumTotParc;

                    if (tributacao.StartsWith("0"))
                    {
                        BCICMS += valorACumTotParc;
                        decimal ICMS = (valorACumTotParc * Convert.ToInt16(tributacao) / 100) / 100;
                        ICMS = (Math.Truncate(ICMS * 100) / 100);
                        valorICMS += ICMS;
                    }

                    C60a ecf60a = new C60a();
                    ecf60a.codigofilial = GlbVariaveis.glb_filial;
                    ecf60a.tipo = "60";
                    ecf60a.subtipo = "A";
                    ecf60a.data = dataMovimento;
                    ecf60a.hora = ConfiguracoesECF.horaUltimaReducaoZ;
                    ecf60a.ECFnumeroserie = numeroSerieECF;
                    ecf60a.ecfnumero = seqECF;
                    ecf60a.aliquotaICMS = tributacao;
                    ecf60a.acumuladoTotalizadorParcial = valorACumTotParc;
                    entidade.AddToC60a(ecf60a);


                    // Para gravar a SOma
                    ecf60m.vendaliquida = vendaLiquida;
                    ecf60m.TotalICMSdebitado = BCICMS;
                    ecf60m.ValorICMS = valorICMS;
                }
            }
            
            #region R01
            r01 r1 = new r01();
            r1.codigofilial = GlbVariaveis.glb_filial;
            r1.tipo = "R01"; //01
            r1.fabricacaoECF = nrFabricacaoECF; //02
            r1.MFAdicional = mfAdicional; //03
            r1.tipoECF = tipoEcf; //05
            r1.marcaECF = marcaECF; //06
            r1.modeloECF = modeloECF; //07
            r1.versaoSB = versaoSB;
            if (dataInstalacaoSB != Funcoes.SetLength(8))
            {
                r1.datainstalacaoSB = Convert.ToDateTime(dadosSoftwareSB).Date; // Convert.ToDateTime(string.Format("{0:yyyy/MM/dd}", dataInstalacaoSB));
                r1.horainstalacaoSB = Convert.ToDateTime(dadosSoftwareSB).TimeOfDay; // Convert.ToDateTime(horaInstalacaoSB).TimeOfDay;
            }
            r1.numeroECF = FuncoesECF.NumeroECF(ConfiguracoesECF.idECF).ToString();
            r1.numeroUsuarioSubstituicaoECF = numeroUsuarioSubECF;
            r1.cnpj = Funcoes.RetirarFormatacaoCNPJ_CPF_IE(cnpjECF);
            r1.inscricao = Funcoes.RetirarFormatacaoCNPJ_CPF_IE(ieECF);

            r1.cnpjdesenvolvedora = GlbVariaveis.cnpjSH;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("cnpj").First().Value), GlbVariaveis.glbSenhaIQ);
            r1.inscricaodesenvolvedora = GlbVariaveis.IESH;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("inscricao").First().Value), GlbVariaveis.glbSenhaIQ);
            r1.inscricaomunicipaldesenvolvedora = GlbVariaveis.IEMunicipalSH;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("inscricaoMunicipal").First().Value), GlbVariaveis.glbSenhaIQ);
            r1.razaosocialdesenvolvedora = GlbVariaveis.razaoSH;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("razaoSocial").First().Value), GlbVariaveis.glbSenhaIQ);
            r1.aplicativo = GlbVariaveis.nomeAplicativo;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("aplicativo").First().Value), GlbVariaveis.glbSenhaIQ);
            r1.versao = GlbVariaveis.glb_Versao; // Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("versao").First().Value), GlbVariaveis.glbSenhaIQ);
            r1.md5 = ConfiguracoesECF.md5Geral;// (dados.Elements("md5").First().Value); // Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("md5").First().Value), GlbVariaveis.glbSenhaIQ);
            r1.md5exe = ConfiguracoesECF.md5PrincipalEXE;
            r1.versaoERPAF = GlbVariaveis.versaoPAF;// Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("versaoERPAF").First().Value), GlbVariaveis.glbSenhaIQ);
            r1.data = dataMovimento;
            entidade.AddTor01(r1);
            entidade.SaveChanges();
            #endregion

            #region R02
            r02 r2 = new r02();
            r2.tipo = "R02";
            r2.codigofilial = GlbVariaveis.glb_filial;
            r2.data = dataMovimento;
            r2.datamovimento = dataMovimento;
            r2.fabricacaoECF = nrFabricacaoECF;
            r2.MFadicional = mfAdicional;
            r2.modeloECF = modeloECF;
            r2.numeroECF = r1.numeroECF = FuncoesECF.NumeroECF(ConfiguracoesECF.idECF).ToString();
            r2.numeroUsuarioSubstituicaoECF = numeroUsuarioSubECF;
            r2.crz = CRZ.Trim().PadLeft(6, '0');
            r2.coo = COOFinal.Trim().PadLeft(6, '0');
            r2.cro = COOReinicio.Trim().PadLeft(6, '0');
            r2.dataemissaoreducaoz = dataUltimaReducao;
            r2.horaemissaoreducaoz = horaUltimaReducao;
            r2.vendabrutadiaria = vendaBruta;
            r2.gtfinal = totalizadorGeral;
            r2.parametroISSQNdesconto = "N";
            r2.EADdados = Funcoes.CriptografarMD5(r2.fabricacaoECF + r2.crz + r2.coo + r2.cro + string.Format("{0:yyyy-MM-dd}", r2.data) + string.Format("{0:yyyy-MM-dd}", r2.dataemissaoreducaoz) + r2.horaemissaoreducaoz + Funcoes.FormatarDecimal(r2.vendabrutadiaria.ToString(), 2).ToString().Replace(".", "").Replace(",", "."));
            entidade.AddTor02(r2);
            entidade.SaveChanges();

            //conteudo.AppendLine("R02" +
            //    r2.fabricacaoECF.Trim().PadRight(20, ' ').Substring(0, 20) +
            //    r2.MFadicional.Trim().PadRight(1, ' ').Substring(0, 1) +
            //    r2.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20) +
            //    r2.numeroUsuarioSubstituicaoECF.Trim().PadRight(2, '0').Substring(0, 2) +
            //    r2.crz.Trim().PadRight(6, '0').Substring(0, 6) +
            //    r2.coo.Trim().PadRight(6, '0').Substring(0, 6) +
            //    r2.cro.Trim().PadRight(6, '0').Substring(0, 6) + //08 Contador reinicio de operacao
            //    String.Format("{0:yyyyMMdd}", r2.data) +
            //    String.Format("{0:yyyyMMdd}", r2.dataemissaoreducaoz) +
            //    String.Format("{0:hhmmss}", r2.horaemissaoreducaoz).Replace(":","") +
            //    Funcoes.FormatarZerosEsquerda(r2.vendabrutadiaria,14,true)+
            //    "N");            
            #endregion R02

            #region R03
            //Adicionando as informações restante
            List<string> complementoR03 = new List<string>();
            complementoR03.Add("OPNF                    " + OPNF.ToString());
            complementoR03.Add("AT                      " + totalizadorAcrescimoICMS.ToString());
            complementoR03.Add("AS                      " + totalizadorAcrescimoISS.ToString());

            foreach (var item in complementoR03)
            {
                r03 r3 = new r03();
                r3.data = dataMovimento;
                r3.codigofilial = GlbVariaveis.glb_filial;
                r3.tipo = "R03";
                r3.fabricacaoECF = nrFabricacaoECF;
                r3.MFAdicional = mfAdicional;
                r3.modeloECF = modeloECF;
                r3.numeroECF = r1.numeroECF = FuncoesECF.NumeroECF(ConfiguracoesECF.idECF).ToString();
                r3.numeroUsuarioSubstituicaoECF = numeroUsuarioSubECF;
                r3.CRZ = CRZ.Trim().PadLeft(6, '0');
                r3.totalizadorParcial = item.ToString().Substring(0, 7); // Aqui pega os 7 primeiros digite Exemp Canc-T
                r3.valoracumulado = Convert.ToDecimal(item.Substring(7, item.Length - 7)); // Aqui pega depois dos 7 primeiros digitos para transformar a string em decimal                
                r3.EADdados = Funcoes.CriptografarMD5(r3.fabricacaoECF + r3.CRZ);
                entidade.AddTor03(r3);
            }

            var filtroR03 = (from n in Conexao.CriarEntidade().C60a
                             where n.data == dataMovimento
                             && n.ECFnumeroserie == nrFabricacaoECF
                             select n).ToList();

            int contadorAliquota=0;
            foreach (var item in filtroR03)
            {                
                string totalizadorAliquota = item.aliquotaICMS.Trim();

                if (totalizadorAliquota=="CANC")
                    totalizadorAliquota="Can-T";
                if (totalizadorAliquota=="DESC")
                    totalizadorAliquota="DT";

                if (totalizadorAliquota.StartsWith("0") || totalizadorAliquota.StartsWith("1") || totalizadorAliquota.StartsWith("2"))
                {
                    contadorAliquota++;
                    totalizadorAliquota = "0" + contadorAliquota.ToString() + "T"+totalizadorAliquota;
                }
                if (totalizadorAliquota=="F")
                    totalizadorAliquota="F1";
                if (totalizadorAliquota=="I")
                    totalizadorAliquota="I1";
                if (totalizadorAliquota=="N")
                    totalizadorAliquota="N1";

                r03 r3 = new r03();
                r3.data = dataMovimento;
                r3.codigofilial = GlbVariaveis.glb_filial;
                r3.tipo = "R03";
                r3.fabricacaoECF = nrFabricacaoECF;
                r3.MFAdicional = mfAdicional;
                r3.modeloECF = modeloECF;
                r3.numeroECF = r1.numeroECF = FuncoesECF.NumeroECF(ConfiguracoesECF.idECF).ToString();
                r3.numeroUsuarioSubstituicaoECF = numeroUsuarioSubECF;
                r3.CRZ = CRZ.Trim().PadLeft(6, '0');
                r3.totalizadorParcial = totalizadorAliquota ; // Aqui pega os 7 primeiros digite Exemp Canc-T
                r3.valoracumulado = item.acumuladoTotalizadorParcial; // Aqui pega depois dos 7 primeiros digitos para transformar a string em decimal                
                r3.EADdados = Funcoes.CriptografarMD5(nrFabricacaoECF + CRZ);
                entidade.AddTor03(r3);


                //conteudo.AppendLine("R03" +
                //    r3.fabricacaoECF.Trim().PadRight(20, ' ').Substring(0, 20) +
                //    r3.MFAdicional.Trim().PadRight(1, ' ').Substring(0, 1) +
                //    r3.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20) +
                //    r3.numeroUsuarioSubstituicaoECF.Trim().PadRight(2, '0').Substring(0, 2) +
                //    r3.CRZ.Trim().PadRight(6, '0').Substring(0, 6) +
                //    r3.totalizadorParcial.Trim().PadRight(7, ' ').Substring(0, 7) +
                //    Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(r3.valoracumulado), 13,true));
            }
            #endregion

            entidade.AddToC60m(ecf60m);
            entidade.SaveChanges();
            file.Close();
            return true;
        }

       
        public bool ReducaoZXML(DateTime dataInicial,DateTime dataFinal)
        {


            return true;
        }


    }
    
    struct InfoECF
    {
        public string ecfNumero { get; set; }
        public string serieFabricacao { get; set; }
        public string MFAdicional { get; set; }
        public string modeloECF { get; set; }
        public string usuarioSubECF { get; set; }
    }
}
