using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Diagnostics;

namespace SICEpdv
{
    public static class ConfiguracoesECF
    {
        public static int idECF { get; set; }
        public static int idNFC { get; set; }
        public static string NFCchave { get; set; }
        public static bool NFC { get; set; }
        public static string NFCserie { get; set; }
        public static int NFCSequencia { get; set; }
        public static bool NFCtransacaobanco { get; set; }
        public static string NFCmodImpressao { get; set; }
        public static bool NFCenviarEmail { get; set; }
        public static string NFCEmail { get; set; }
        public static string NFCSenhaEmail { get; set; }
        public static string NFCPortaEmail { get; set; }
        public static string NFCHost { get; set; }
        public static string NFCTitulo { get; set; }
        public static string NFCMensagem { get; set; }
        public static bool NFCUsarSSL { get; set; }
        public static int NFCHambiente { get; set; }
        public static bool NFCImpressaoDireta { get; set; }
        public static string NFCNomeImpressora { get; set; }
        public static string NFCModeloImpressao { get; set; }
        public static bool caixaPendente { get; set; }
        public static string NFCcrt { get; set; }



        public static int idECFHoraDivergente { get; set; } // Para pegar a hora do ECF quando houver divergência
        public static string numeroECF { get; set; }
        public static string nrFabricacaoECF { get; set; }
        public static string mfAdicionalECF { get; set; }        
        public static string marcaECF { get; set; }
        public static string modeloECF { get; set; }
        public static string tipoECF { get; set; }
        public static string icmsIsencaoII { get; set; }
        public static string icmsSubFF { get; set; }
        public static string icmsNaoIncNN { get; set; }
        public static string issIsencaoSI { get; set; }
        public static string issSubSF { get; set; }
        public static string issNaoIncSN { get; set; }
        public static string DH { get; set; }
        public static string CA { get; set; }
        public static string CH { get; set; }
        public static string TI { get; set; }
        public static string DV { get; set; }
        public static string CR { get; set; }
        public static string AV { get; set; }
        public static string PF { get; set; }
        public static string crediario { get; set; }
        public static string recebimento { get; set; }
        public static string recebimentoGerencial { get; set; }
        public static string mensagem { get; set; }
        public static bool verificarGaveta { get; set; }
        public static bool styleMetro { get; set; }
        public static bool tecladoVirtual { get; set; }
        
        
        public static bool tefDiscado { get; set; }
        public static bool tefDedicado { get; set; }
        public static string pathRetornoECF { get; set; }  
        /// <summary>
        /// Variavies usada pelo PAF
        /// </summary>
        public static bool prevenda { get; set; }
        public static bool pdv { get; set; }
        public static bool davPorECF { get; set; }
        public static bool davporImpressoraNaoFiscal { get; set; }
        public static bool davImpressaoCarta { get; set; }
        public static string md5PrincipalEXE { get; set; }
        public static string md5Geral { get; set; }
        public static DateTime dataUltimaReducaoZ { get; set; }
        public static TimeSpan horaUltimaReducaoZ { get; set; }
        public static bool zPendente { get; set; }
        public static DateTime dataUltMovimentoECF { get; set; }
        public static bool reducaoZEmitida { get; set; }
        public static bool horaDivergenteECF  { get; set; }
        public static decimal grandeTotal { get; set; }
        public static string  perfil{ get; set; }
        public static int ultIDECF { get; set; }
  

        
       
        

        public static void Carregar(bool entradaInicial=true)
        {
            // Tributação Padrões
            //ICMS            
            icmsIsencaoII = "II";
            icmsNaoIncNN = "NN";
            icmsSubFF = "FF";                
            //ISS
            issIsencaoSI = "SI";
            issNaoIncSN = "SN";
            issSubSF = "SF";
            mfAdicionalECF = " ";
            // Forma de pagamento padrao
            DH = "Dinheiro";
            CH = "Cheque";
            CA = "Cartão";
            TI = "Ticket";
            CR = "Crediário";
            DV = "Devolução";
            AV = "AV";
            PF = "PF";
            zPendente = false;
            reducaoZEmitida = false;
            verificarGaveta = true;
            GlbVariaveis.glb_TipoPAF  = GlbVariaveis.tipoPAF.Geral.ToString();
            //ConfiguracoesECF.idNFC = 1; // 1-Daruma Migrate 2 - ACbr 
            //entradaInicial = true;

            if (!File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Terminal.xml"))
                throw new Exception("Configuração do Terminal não encontrada. Solicite geração do arquivo a IQ Sistemas !");

            if (entradaInicial)
            {
                #region ler o terminal
                try
                {
                    lerXML();
                }
                catch (Exception erro)
                {
                    migrarXML();
                    lerXML();
                }

                #endregion
            }

            if (entradaInicial)
            {
                #region ler NFC
                if (File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\NFC.xml"))
                {
                    try
                    {
                        XDocument xmlTerminal = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\NFC.xml");
                        var config = from c in xmlTerminal.Descendants("Servico")
                                     select new
                                     {
                                         NFC = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("NFC").Value), GlbVariaveis.glbSenhaIQ),
                                         Solucao = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Solucao").Value), GlbVariaveis.glbSenhaIQ),
                                         Serie = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Serie").Value), GlbVariaveis.glbSenhaIQ),
                                         Chave = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Chave").Value), GlbVariaveis.glbSenhaIQ),
                                         Impressao = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Impressao").Value), GlbVariaveis.glbSenhaIQ),
                                         ImpressaoDireta = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("ImpressaoDireta").Value), GlbVariaveis.glbSenhaIQ),
                                         Host = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Host").Value), GlbVariaveis.glbSenhaIQ),
                                         Email = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Email").Value), GlbVariaveis.glbSenhaIQ),
                                         SenhaEmail = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("SenhaEmail").Value), GlbVariaveis.glbSenhaIQ),
                                         Titulo = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Titulo").Value), GlbVariaveis.glbSenhaIQ),
                                         Porta = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Porta").Value), GlbVariaveis.glbSenhaIQ),
                                         UsarSSL = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("UsarSSL").Value), GlbVariaveis.glbSenhaIQ),
                                         Mensagem = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Mensagem").Value), GlbVariaveis.glbSenhaIQ),
                                         EnviarEmail = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("EnviarEmail").Value), GlbVariaveis.glbSenhaIQ),
                                         //Hambiente = "2 - homologação"
                                         NomeImpressora = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("NomeImpressora").Value), GlbVariaveis.glbSenhaIQ),
                                         ModeloImpressao = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("ModeloImpressao").Value), GlbVariaveis.glbSenhaIQ),

                                     };

                        foreach (var item in config)
                        {

                            NFC = item.NFC == "Sim" ? true : false;
                            idNFC = int.Parse(item.Solucao.Substring(0, 1));
                            nrFabricacaoECF = item.Solucao.Substring(0, 1) == "1" ? "Migrate" : "SICENFCe";
                            NFCchave = item.Chave;
                            NFCserie = item.Serie;
                            NFCmodImpressao = item.Impressao.ToString().Substring(0, 1);
                            NFCEmail = item.Email;
                            NFCSenhaEmail = item.SenhaEmail;
                            NFCTitulo = item.Titulo;
                            NFCPortaEmail = item.Porta;
                            NFCUsarSSL = item.UsarSSL == "S" ? true : false;
                            NFCMensagem = item.Mensagem;
                            NFCenviarEmail = item.EnviarEmail == "S" ? true : false;
                            //NFCHambiente = int.Parse(item.Hambiente.Substring(0, 1));
                            NFCImpressaoDireta = item.ImpressaoDireta == "S" ? true : false;
                            NFCHost = item.Host;
                            NFCNomeImpressora = item.NomeImpressora.ToString();
                            NFCModeloImpressao = item.ModeloImpressao.ToString().Substring(0,1);

                            if (pdv == false)
                                NFC = false;

                            if (NFC == false)
                                idNFC = 0;

                            

                            if (NFC == true &&(NFCserie.Length < 2 || NFCserie == null) && pdv == true)
                                MessageBox.Show("Não foi possivel verificar a serie no configPAF!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            else if(NFC == true)
                            {
                                var qtd = (from i in Conexao.CriarEntidade().serienf
                                           where i.codigofilial == GlbVariaveis.glb_filial && i.serie == NFCserie
                                           select i).Count();
                                
                                if(qtd == 0)
                                    MessageBox.Show("Não foi possivel localizar a Serie na Filial.:"+ GlbVariaveis.glb_filial, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }

                        }
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show("não foi possivel carregar o NFC.xml "+erro.ToString());
                    }
                }
                else
                {
                    NFC =  false;
                    idNFC = 0;
                    nrFabricacaoECF = "";
                    NFCchave = "";
                    NFCserie = "0";
                    NFCmodImpressao = "";
                    NFCEmail = "";
                    NFCSenhaEmail = "";
                    NFCTitulo = "";
                    NFCPortaEmail = "";
                    NFCUsarSSL = false;
                    NFCMensagem = "";
                    NFCenviarEmail = false;
                    NFCHambiente = 2;
                    NFCImpressaoDireta = false;
                    NFCHost = "";

                }
                #endregion

                if (ConfiguracoesECF.NFC == true)
                {
                    //ConfiguracoesECF.NFCtransacaobanco = false;
                    var retorno = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT nfcetransacaobanco FROM configfinanc WHERE codigofilial ='"+GlbVariaveis.glb_filial+"'");
                    ConfiguracoesECF.NFCtransacaobanco = retorno.FirstOrDefault().ToString() == "S" ? true : false;
                }

                //verificar para excluir
                if (ConfiguracoesECF.NFC == true)
                {
                    FuncoesNFC NFCe = new FuncoesNFC();
                    if (NFCe.LerSequenciaNFCGuardada() == 0)
                    {
                        Venda.ObterSequencialNFC();
                        if (File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\SequenciaNFC.xml"))
                        {
                            NFCe.GuardarSequenciaNFC(ConfiguracoesECF.NFCSequencia.ToString());
                        }
                        else
                        {
                            NFCe.CriarSequnciaNFC(ConfiguracoesECF.NFCSequencia.ToString());
                        }
                    }

                    NFCSequencia = NFCe.LerSequenciaNFCGuardada();
                }

               
            }

            #region ACBR
            try
            {

              
                if (System.IO.File.Exists(@"acbr.txt") && pdv == true && ConfiguracoesECF.idNFC==0 )
                {
                    GlbVariaveis.glb_Acbr = true;
                    string line = "";
                    string ecf = "";
                    StreamReader file = new StreamReader(@"acbr.txt");

                    while ((line = file.ReadLine()) != null)
                    {
                        #region
                        if (line.Contains("ecf"))
                        {
                            ecf = line.Replace(" ", "").Replace("[ecf]", "").Replace("=", "").Replace(";", "");
                            try
                            {
                                idECF = int.Parse(ecf);
                            }
                            catch (Exception)
                            {
                                idECF = 0;

                            }
                        }

                        if (line.Contains("velocidade"))
                        {
                            try
                            {
                                GlbVariaveis.glb_velocidadeAcbr = int.Parse(line.Replace(" ", "").Replace("[velocidade]", "").Replace("=", "").Replace(";", ""));
                            }
                            catch (Exception)
                            {
                                GlbVariaveis.glb_velocidadeAcbr = 9600;
                                MessageBox.Show("Não Foi Possivel ler a Velocidade da ECF no acbr.txt");
                            }
                        }

                        if (line.Contains("porta"))
                        {
                            try
                            {
                                GlbVariaveis.glb_portaAcbr = line.Replace(" ", "").Replace("[porta]", "").Replace("=", "").Replace(";", "");
                            }
                            catch (Exception)
                            {
                                GlbVariaveis.glb_portaAcbr = "Procurar";
                            }
                        }

                        if (line.Contains("modoGaveta"))
                        {
                            try
                            {
                               

                                if (line.Replace(" ", "").Replace("[modoGaveta]", "").Replace("=", "").Replace(";", "") == "true")
                                {
                                    GlbVariaveis.glb_modoGavetaAcbr = true;
                                }
                                else
                                {
                                    GlbVariaveis.glb_modoGavetaAcbr = false;
                                }
                            }
                            catch (Exception)
                            {
                                GlbVariaveis.glb_modoGavetaAcbr = false;
                            }
                        }

                        

                        if (line.Contains("tempo"))
                        {
                            
                                try
                                {
                                    int tempo = int.Parse(line.Replace(" ", "").Replace("[tempo]", "").Replace("=", "").Replace(";", ""));

                                     GlbVariaveis.glb_tempo = tempo;
                                }
                                catch(Exception)
                                {
                                     GlbVariaveis.glb_tempo = 3;
                                }   
                        }

                        if (line.Contains("descricaoGrande"))
                        {

                            try
                            {
                                bool descricaoGrande = bool.Parse(line.Replace(" ", "").Replace("[descricaoGrande]", "").Replace("=", "").Replace(";", ""));

                                GlbVariaveis.glb_descricaoGrande = descricaoGrande;
                            }
                            catch (Exception)
                            {
                                GlbVariaveis.glb_descricaoGrande = false;
                            }
                        }

                        if (line.Contains("softFlow"))
                        {

                            try
                            {
                                bool softFlow = bool.Parse(line.Replace(" ", "").Replace("[softFlow]", "").Replace("=", "").Replace(";", ""));
                                GlbVariaveis.glb_softFlow = true;

                            }
                            catch (Exception erro)
                            {
                                GlbVariaveis.glb_softFlow = false;
                            }
                        }


                        if (line.Contains("hardFlow"))
                        {

                            try
                            {
                                bool hardFlow = bool.Parse(line.Replace(" ", "").Replace("[hardFlow]", "").Replace("=", "").Replace(";", ""));
                                GlbVariaveis.glb_hardFlow = true;

                            }
                            catch (Exception erro)
                            {
                                GlbVariaveis.glb_hardFlow = false;
                            }
                        }

                        #endregion
                    }

                    file.Close();

                }
                else
                {
                    GlbVariaveis.glb_Acbr = false;
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }

            #endregion
            
            
            if (!Directory.Exists(@"c:\iqsitemas\tef"))
            {
                Directory.CreateDirectory(@"c:\iqsistemas\TEF");
                Directory.CreateDirectory(@"c:\iqsistemas\TEF\CONFIRMADAS");        
            }

            if (!Directory.Exists(@"c:\tef_dial"))
            {
                Directory.CreateDirectory(@"c:\tef_dial");
                Directory.CreateDirectory(@"c:\tef_dial\req");
                Directory.CreateDirectory(@"c:\tef_dial\resp");
            }
            
            if(GlbVariaveis.glb_Acbr == false)
                idECF = 0;
            
            
            string ipLocal = Funcoes.IDTerminal();
            pathRetornoECF = @ConfigurationManager.AppSettings["pathRetornoECF"];

            

            if (!File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml") && ConfiguracoesECF.pdv && ConfiguracoesECF.idNFC==0)
            {
                throw new Exception("Arquivo de cadastro de ECFs não foi encontrado");
            }
            
            // Abrir Configuração da Identificação da SHOUSE
            //XDocument idSHouse = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\sHouse.xml");

            //var dados = (from n in idSHouse.Descendants("sHouse").Elements("Dados")
            //             select n).First();

            
            //if (dados.IsEmpty)
            //    throw new Exception("Não foi possível ler o arquivo de Identificação da SHouse");

           // hashMD5Exe = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(dados.Elements("md5").First().Value), GlbVariaveis.glbSenhaIQ);           

            // Detectando ECF DAruma
            // Ativando a entidade venda para nao demorar na inclusao do 1 iten

            if (Conexao.onLine)
            {
                try
                {
                    #region venda teste

                    siceEntities entidade = Conexao.CriarEntidade();
                    if (Conexao.tipoConexao == 2)
                        entidade = Conexao.CriarEntidade(false);

                    vendas item = new vendas();
                    item.aentregar = "N";
                    item.codigofilial = GlbVariaveis.glb_filial;
                    item.classe = "0000";
                    item.codigobarras = "12";
                    item.codigofiscal = "000";
                    item.comissao = "A";
                    item.grade = "nenhuma";
                    item.id = "teste";
                    item.lote = "0";
                    item.romaneio = "S";
                    item.tipo = "0 - Produto";
                    item.embalagem = 1;
                    item.nrcontrole = 0;
                    item.codigo = "teste";
                    item.produto = "teste";
                    item.quantidade = 0;
                    item.preco = 0;
                    item.custo = 0;
                    item.precooriginal = 0;
                    item.acrescimototalitem = 0;
                    item.unidade = "";
                    item.Descontoperc = 0;
                    item.descontovalor = 0;
                    item.vendedor = "000";
                    item.icms = 0;
                    item.tributacao = "00";
                    item.total = 0; // Math.Round( quantidade * precooriginal);
                    item.cfop = "5.102";
                    item.cstpis = "01";
                    item.cstcofins = "01";
                    item.modelodocfiscal = "2D";
                    item.cancelado = "N";
                    item.operador = GlbVariaveis.glb_Usuario;
                    item.ratfrete = 0;
                    item.ratseguro = 0;
                    item.ratdespesas = 0;
                    item.cstipi = "99";
                    item.qUnidIPI = 0;
                    item.vUnidIPI = 0;
                    item.ncm = "";
                    item.nbm = "";
                    item.ncmespecie = "";
                    item.origem = "0";
                    item.itemDAV = "S";
                    item.canceladoECF = "N";
                    item.vendaatacado = "N";
                    item.cenqipi = "";
                    item.vendaatacado = "N";
                    entidade.AddTovendas(item);
                    entidade.SaveChanges();
                    entidade.DeleteObject(item);
                    entidade.SaveChanges();

                    #endregion
                }
                catch (Exception erro)
                {
                    throw new Exception("Ativando tabela venda:" + erro.ToString());
                }
            };

            
            // Pegando formas de pagamento padrao
            if (File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\PagamentoECF.xml"))
            {
                XDocument xPagamento = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\PagamentoECF.xml");
                var pagamento = from c in xPagamento.Descendants("Forma")
                             select new
                             {
                                 dinheiro = c.Element("DH").Value,
                                 cheque = c.Element("CH").Value,
                                 cartao = c.Element("CA").Value,
                                 crediario = c.Element("CR").Value,
                                 ticket = c.Element("TI").Value,
                                 devolucao = c.Element("DV").Value
                             };
                foreach (var item in pagamento)
                {
                    DH = item.dinheiro;
                    CH = item.cheque;
                    CA = item.cartao;
                    CR = item.crediario;
                    TI = item.ticket;
                    DV = item.devolucao;
                }
            }

            // Pegando o Tipo do PAF
            if (File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\TipoPAF.xml"))
            {                
                XDocument xtipoPAF = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\TipoPAF.xml");
                var getTipoPAF = (from c in xtipoPAF.Descendants("Tipo")
                                select new
                                {
                                    tipoPAF = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("descricao").Value),GlbVariaveis.glbSenhaIQ)                                    
                                }).FirstOrDefault();


                GlbVariaveis.glb_TipoPAF = getTipoPAF.tipoPAF.ToString();
                
                //GlbVariaveis.tipoPAF.Geral.ToString();                

                //switch (getTipoPAF.tipoPAF.Substring(0,1))
                //{
                //    case "0":
                //        GlbVariaveis.glb_TipoPAF = GlbVariaveis.tipoPAF.Geral.ToString();
                //        break;
                //    case "1":
                //        GlbVariaveis.glb_TipoPAF = GlbVariaveis.tipoPAF.Serviço.ToString();                    
                //        break;
                //    case "2":
                //        GlbVariaveis.glb_TipoPAF = GlbVariaveis.tipoPAF.Combustível.ToString();                    
                //        break;
                //}

            }

            SetarPerfil();

            if (File.Exists("log_teste.txt"))
            {
                idECF = 9999;
                pdv = true;
                idNFC = 0;
                GlbVariaveis.glb_Acbr = false;
            }

            //marckvaldo 2015-08-01;
            #region
            /*
            if (File.Exists("ultimaECF.xml") && pdv && ConfiguracoesECF.idNFC==0)
            {
                try
                {
                XDocument doc = XDocument.Load("UltimaECF.xml");
                var dados = (from n in doc.Descendants("ecf")
                             select new 
                             {
                                 _idECF = n.Element("idECF").Value,
                                 _numeroECF = n.Element("numeroECF").Value,
                                 _fabricacaoECF = n.Element("fabricacaoECF").Value
                                 
                             }).First();        
                    idECF = Convert.ToInt16(dados._idECF);
                    nrFabricacaoECF = dados._fabricacaoECF;
                    numeroECF = dados._numeroECF;
                    // Para detectar o ECF e não dar erro de comunicação
                    if (GlbVariaveis.glb_Acbr == false)
                    {
                        if (idECF == 2)
                        {

                            //DARUMA_FW.iRetorno = DARUMA_FW.regAlterarValor_Daruma(@"START\LocalArquivosRelatorios", @ConfiguracoesECF.pathRetornoECF);
                            //DARUMA_FW.iRetorno = DARUMA_FW.regAlterarValor_Daruma(@"START\LocalArquivos", @ConfiguracoesECF.pathRetornoECF);
                            //DARUMA_FW.iRetorno = DARUMA_FW.regAlterarValor_Daruma(@"ECF\AtoCotepe\LocalArquivos",@ConfiguracoesECF.pathRetornoECF);
                            //DARUMA_FW.iRetorno = DARUMA_FW.regAlterarValor_Daruma(@"ECF\ReducaoZAutomatica", "0");

                            //marcaECF = "DARUMA";
                            //FuncoesECF.EqualizarVelocidade(2);

                            //int estado = 1;
                            //DARUMA_FW.iRetorno = DARUMA_FW.rStatusGaveta_DUAL_DarumaFramework(ref estado);
                            //if (DARUMA_FW.iRetorno != 1)
                            //    ConfiguracoesECF.verificarGaveta = false;
                            idECF = 0;
                        }
                    }

                    FuncoesECF.VerificaImpressoraLigada();

                }
                catch (Exception ex)
                {
                    idECF = 0;
                    File.Delete("ultimaECF.xml");
                    MessageBox.Show("Erro xml última ECF, arquivo apagado, Enter para continuar: "+ex.Message);
                
                }
            }
            */
            #endregion

            if (GlbVariaveis.glb_Acbr == false)
            {
                #region Detectar Daruma


                if (idECF == 0 && pdv && ConfiguracoesECF.idNFC==0)
                {

                    if ((File.Exists("DarumaFrameWork.dll") && FuncoesECF.DetectarECF(2) && pdv))
                    {
                        DARUMA_FW.iRetorno = DARUMA_FW.regAlterarValor_Daruma(@"START\LocalArquivosRelatorios", @ConfiguracoesECF.pathRetornoECF);
                        DARUMA_FW.iRetorno = DARUMA_FW.regAlterarValor_Daruma(@"START\LocalArquivos", @ConfiguracoesECF.pathRetornoECF);
                        DARUMA_FW.iRetorno = DARUMA_FW.regAlterarValor_Daruma(@"ECF\AtoCotepe\LocalArquivos", @ConfiguracoesECF.pathRetornoECF);
                        DARUMA_FW.iRetorno = DARUMA_FW.regAlterarValor_Daruma(@"ECF\ReducaoZAutomatica", "0");
                        marcaECF = "DARUMA";
                        idECF = 2;
                                              
                       int estado = 0;
                       // dar erro quando vai capiturar o nrfabricao da ecf
                       DARUMA_FW.iRetorno = DARUMA_FW.rStatusGaveta_DUAL_DarumaFramework(ref estado);
                       if (DARUMA_FW.iRetorno != 1)
                           ConfiguracoesECF.verificarGaveta = false;
                        
                    }
                    else
                        idECF = 0;
                }



                #endregion

                #region Detectar Bematech
                if (idECF == 0 && pdv && ConfiguracoesECF.idNFC == 0)
                {

                    //Detectanto o ECF Bematech automaticamente 
                    
                    try
                    {

                        if (FuncoesECF.NumeroECF(1).Trim() != "")
                        {
                            idECF = 1;
                            marcaECF = "BEMATECH";
                            // Modificando o arquivo bemafi32 para mudar o local de retorno dos arquivos

                            bool modificar = false;
                            using (FileStream fs = new FileStream("bemafi32.ini", FileMode.Open))
                            {
                                using (StreamReader sr = new StreamReader(fs))
                                {
                                    string linha = null;
                                    while ((linha = sr.ReadLine()) != null)
                                    {
                                        if (linha.IndexOf("Path=") > -1)
                                        {
                                            if (linha != "Path=" + pathRetornoECF)
                                                modificar = true;
                                        }
                                    }
                                }
                            };

                            if (modificar)
                            {
                                using (FileStream fs = new FileStream("bemafi32.ini", FileMode.Open))
                                {
                                    using (StreamReader sr = new StreamReader(fs))
                                    {
                                        using (FileStream fsTemp = new FileStream("bemafi32.tmp", FileMode.Create, FileAccess.Write))
                                        {
                                            using (StreamWriter sw = new StreamWriter(fsTemp))
                                            {
                                                string linha = null;
                                                while ((linha = sr.ReadLine()) != null)
                                                {
                                                    if (linha.IndexOf("Path=") > -1)
                                                    {
                                                        linha = "Path=" + pathRetornoECF;
                                                    }
                                                    sw.Write(linha + "\r\n");
                                                }
                                            }
                                        }
                                    }
                                }
                                File.Delete("bemafi32.ini");
                                File.Move("bemafi32.tmp", "bemafi32.ini");
                            };


                            // Configuração para que o número de série retorne os 20 caracteres.

                            bool modificarSerie = false;
                            using (FileStream fs = new FileStream("bemafi32.ini", FileMode.Open))
                            {
                                using (StreamReader sr = new StreamReader(fs))
                                {
                                    string linha = null;
                                    while ((linha = sr.ReadLine()) != null)
                                    {
                                        if (linha.IndexOf("Impressora=0") > -1)
                                        {
                                            if (linha == "Impressora=0")
                                                modificarSerie = true;
                                        }
                                    }
                                }
                            };

                            if (modificarSerie)
                            {
                                using (FileStream fs = new FileStream("bemafi32.ini", FileMode.Open))
                                {
                                    using (StreamReader sr = new StreamReader(fs))
                                    {
                                        using (FileStream fsTemp = new FileStream("bemafi32.tmp", FileMode.Create, FileAccess.Write))
                                        {
                                            using (StreamWriter sw = new StreamWriter(fsTemp))
                                            {
                                                string linha = null;
                                                while ((linha = sr.ReadLine()) != null)
                                                {
                                                    if (linha.IndexOf("Impressora=0") > -1)
                                                    {
                                                        linha = "Impressora=1";
                                                    }
                                                    sw.Write(linha + "\r\n");
                                                }
                                            }
                                        }
                                    }
                                }
                                File.Delete("bemafi32.ini");
                                File.Move("bemafi32.tmp", "bemafi32.ini");
                            };


                        }
                    } // IF ECF Bematech
                    catch
                    {
                        idECF = 0;
                    }
                }

                #endregion

                #region Detectar Elgin
                if (idECF == 0 && pdv && ConfiguracoesECF.idNFC == 0)
                {
                    //Detectanto o ECF Elgin automaticamente  
                    try
                    {

                        string numeroECFDetectado = FuncoesECF.NumeroECF(3);

                        if (numeroECFDetectado.Trim() != "" && numeroECFDetectado != "000")
                        {
                            idECF = 3;
                            marcaECF = "ELGIN";
                            // Modificando o arquivo bemafi32 para mudar o local de retorno dos arquivos

                            bool modificar = false;
                            using (FileStream fs = new FileStream("elgin.ini", FileMode.Open))
                            {
                                using (StreamReader sr = new StreamReader(fs))
                                {
                                    string linha = null;
                                    while ((linha = sr.ReadLine()) != null)
                                    {
                                        if (linha.IndexOf("Path=") > -1)
                                        {
                                            if (linha != "Path=" + pathRetornoECF)
                                                modificar = true;
                                        }
                                    }
                                }
                            };

                            if (modificar)
                            {
                                using (FileStream fs = new FileStream("elgin.ini", FileMode.Open))
                                {
                                    using (StreamReader sr = new StreamReader(fs))
                                    {
                                        using (FileStream fsTemp = new FileStream("elgin.tmp", FileMode.Create, FileAccess.Write))
                                        {
                                            using (StreamWriter sw = new StreamWriter(fsTemp))
                                            {
                                                string linha = null;
                                                while ((linha = sr.ReadLine()) != null)
                                                {
                                                    if (linha.IndexOf("Path=") > -1)
                                                    {
                                                        linha = "Path=" + pathRetornoECF;
                                                    }
                                                    sw.Write(linha + "\r\n");
                                                }
                                            }
                                        }
                                    }
                                }
                                File.Delete("elgin.ini");
                                File.Move("elgin.tmp", "elgin.ini");
                            };
                        };

                    } // IF ECF Elgin
                    catch
                    {
                        idECF = 0;
                    }
                }




                #endregion


                #region Detectar Sweda
                if (idECF == 0 && pdv && System.IO.File.Exists(@"c:\tempdata\sweda.ini") && ConfiguracoesECF.idNFC == 0)
                {

                    //Detectanto o ECF Sweda automaticamente  
                    try
                    {

                        string numeroECFDetectado = FuncoesECF.NumeroECF(4);

                        if (numeroECFDetectado != "" && numeroECFDetectado != "000")
                        {
                            idECF = 4;
                            marcaECF = "SWEDA";
                            // Modificando o arquivo bemafi32 para mudar o local de retorno dos arquivos

                            //bool modificar = false;
                            //using (FileStream fs = new FileStream("SWC.ini", FileMode.Open))
                            //{
                            //    using (StreamReader sr = new StreamReader(fs))
                            //    {
                            //        string linha = null;
                            //        while ((linha = sr.ReadLine()) != null)
                            //        {
                            //            if (linha.IndexOf("DIRETORIO=") > -1)
                            //            {
                            //                if (linha != "DIRETORIO=" + pathRetornoECF)
                            //                    modificar = true;
                            //            }
                            //        }
                            //    }
                            //};

                            //if (modificar)
                            //{
                            //    using (FileStream fs = new FileStream("SWC.ini", FileMode.Open))
                            //    {
                            //        using (StreamReader sr = new StreamReader(fs))
                            //        {
                            //            using (FileStream fsTemp = new FileStream("Sweda.tmp", FileMode.Create, FileAccess.Write))
                            //            {
                            //                using (StreamWriter sw = new StreamWriter(fsTemp,Encoding.GetEncoding("ISO-8859-1")))
                            //                {
                            //                    string linha = null;
                            //                    while ((linha = sr.ReadLine()) != null)
                            //                    {
                            //                        if (linha.IndexOf("DIRETORIO=") > -1)
                            //                        {
                            //                            linha = "DIRETORIO=" + pathRetornoECF;
                            //                        }
                            //                        sw.Write(linha + "\r\n");
                            //                    }
                            //                }
                            //            }
                            //        }
                            //    }
                            //    File.Delete("SWC.ini");
                            //    File.Move("Sweda.tmp", "SWC.ini");
                            //};
                        };

                    } // IF ECF Sweda
                    catch
                    {
                        FuncoesECF.FecharPorta(4);
                        idECF = 0;
                    }
                }



                #endregion
            }
            else if (pdv && GlbVariaveis.glb_Acbr == true && ConfiguracoesECF.idNFC == 0)
            {
                if (FuncoesECF.VerificaImpressoraLigada(false,false) == false)
                {
                    idECF = 0;
                }

                FuncoesECF.VerificarStatusPapel(true);

                if (FuncoesECF.formapagamentoACBR() == false && idECF > 0)
                {
                    MessageBox.Show("impressora Desligada ou sem Papel");
                    MessageBox.Show("Não é possivel Carregar Formas de Pagamento ECF");
                    idECF = 0;
                    //Application.Exit();
                }

                if (FuncoesECF.ComprovanteNaoFiscalACBR() == false && idECF > 0)
                {
                    MessageBox.Show("impressora Desligada ou sem Papel");
                    MessageBox.Show("Não é possivel Carregar Comprovante não Fiscal ECF");
                    idECF = 0;
                    //Application.Exit();
                }

            }





            if (idNFC>0)
            {
                idECF = 9999; /// Aqui passa um número para manter a variável idECF e ativar as funções de PDV  
                GlbVariaveis.glb_Acbr = false;
                
                /// Aqui define as variaveis por que o EMissor NFC é Daruma 
                if (ConfiguracoesECF.idNFC==1)
                {
                    DARUMA_FW.iRetorno = DARUMA_FW.regAlterarValor_Daruma(@"START\LocalArquivosRelatorios", @ConfiguracoesECF.pathRetornoECF);
                    DARUMA_FW.iRetorno = DARUMA_FW.regAlterarValor_Daruma(@"START\LocalArquivos", @ConfiguracoesECF.pathRetornoECF);
                    DARUMA_FW.iRetorno = DARUMA_FW.regAlterarValor_Daruma(@"ECF\AtoCotepe\LocalArquivos", @ConfiguracoesECF.pathRetornoECF);
                    DARUMA_FW.iRetorno = DARUMA_FW.regAlterarValor_Daruma(@"ECF\ReducaoZAutomatica", "0");
                    marcaECF = "DARUMA";

                    //string Str_Chave = "Tuls//4iiwZHiNnhfnHBPw==";

                    //iRetorno = Declaracoes.regAlterarValor_Daruma("FRAMEWORKGNE\\CONFIGURACAO\\EmpPK",Str_Chave);
                    DARUMA_FW.iRetorno = DARUMA_FW.regAlterarValor_NFCe_Daruma("CONFIGURACAO\\EmpPK", ConfiguracoesECF.NFCchave);


                };
            }


            if (idECF != 0)
            {
                verificarGaveta = true;
                ultIDECF = idECF;
                davporImpressoraNaoFiscal = false;
                davPorECF = false;
                FuncoesECF.FecharPorta(4);
                if (tefDedicado)
                {
                    TEF.PathReq = "C:\\client\\REQ\\";
                    TEF.PathResp = "C:\\client\\RESP\\";
                    TEF.mensagensRetorno();
                }

                Thread.Sleep(150);

               if(ConfiguracoesECF.NFC == false)
                    nrFabricacaoECF = FuncoesECF.NumeroFabricacaoECF().ToString();
             
                                               
                try
                {
                Thread.Sleep(150);
                mfAdicionalECF = FuncoesECF.DataHoraGravacaoUsuarioMF("MFAdicional");
                    if (string.IsNullOrEmpty(mfAdicionalECF.Trim()))
                        mfAdicionalECF=" ";
                }
                catch (Exception)
                {
                    throw new Exception("Não foi possível obter dados do Usuário MF");
                }
                
                try
                {
                    //até aqui está tudo ok

                Thread.Sleep(150);
                tipoECF = FuncoesECF.MarcaModeloTipoECF("Marca");
                modeloECF = FuncoesECF.MarcaModeloTipoECF("Modelo");
                tipoECF = FuncoesECF.MarcaModeloTipoECF("Tipo");
                }
                catch (Exception)
                {
                    throw new Exception("Não foi possível obter Marca,modelo e tipo do ECF !");
                }
                
                try
                {
                    Thread.Sleep(150);
                    dataUltimaReducaoZ = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy hh:mm:ss}", FuncoesECF.DataUltimaReduzacaoZ()));
                    horaUltimaReducaoZ = Convert.ToDateTime(dataUltimaReducaoZ).TimeOfDay;
                    if (FuncoesECF.ZPendente())
                    {
                        zPendente = true;
                        dataUltMovimentoECF = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", FuncoesECF.DataHoraUltDocumentoECF()));
                    }
                }
                catch (Exception)
                {
                    dataUltimaReducaoZ = DateTime.Now;
                    horaUltimaReducaoZ = DateTime.Now.TimeOfDay;
                    // REtirado para evitar erro quando a ECF está com redução Z automática e o operador não encerrou o caixa
                    //throw new Exception("Erro obtendo data e hora última redução Z");
                }
            }

            idECF = pdv == true ? idECF : 0;

            
            if (pdv)
            {
                // Para Elgin Por que dar erro
                if (idECF==3)
                Thread.Sleep(5000);

                if (idECF == 0)
                {
                    File.Delete("ultimaECF.xml");
                    throw new Exception("Equipamento Fiscal sem comunicação, desligada, sem papel ou data e hora diferentes do ECF!");
                }

                try
                {
                    numeroECF = FuncoesECF.NumeroECF(idECF);
                }
                catch
                {
                    File.Delete("ultimaECF.xml");
                    throw new Exception("Equipamento fiscal não está conectado ou está sem papel  !");
                }

                    if (!FuncoesECF.VerificaFabricacaoGrandeTotal())
                        throw new Exception("Número de fabricação do ECF e Grande Total não conferem com o arquivo de cadastro dos ECF's. Solicite suporte IQ Sistemas. ");
                

                DiferencaDataHoraECF();


                /*if (idECF > 0)
                {
                    XElement ecf = new XElement("ecf");
                    ecf.Add(new XElement("idECF", idECF),
                        new XElement("numeroECF", numeroECF),
                            new XElement("fabricacaoECF", nrFabricacaoECF));
                    ecf.Save("ultimaECF.xml");
                }*/

            }           
        }

        public static void SetarPerfil()
        {

            // Pegando o pefil
            if (!Conexao.onLine)
                perfil = "W";

            if (Conexao.onLine)
            {
                siceEntities entidade;
                if (Conexao.tipoConexao == 2 && Conexao.ConexaoOnline() == false)
                    entidade = Conexao.CriarEntidade(false);
                else
                    entidade = Conexao.CriarEntidade();

                var dadosEmpresa = (from n in entidade.filiais
                                    where n.CodigoFilial == GlbVariaveis.glb_filial
                                    select new { n.empresa, n.cidade, n.estado }).FirstOrDefault();
                perfil = "Não definido";

                if (dadosEmpresa.estado == "AM" || dadosEmpresa.estado == "GO" || dadosEmpresa.estado == "MA" || dadosEmpresa.estado == "MS"
                    || dadosEmpresa.estado == "PE" || dadosEmpresa.estado == "RJ" || dadosEmpresa.estado == "RR" || dadosEmpresa.estado == "SC"
                    || dadosEmpresa.estado == "TO" || dadosEmpresa.estado == "DF")
                {
                    perfil = "W";
                }

                if (dadosEmpresa.estado == "MG")
                    perfil = "X";

                if (dadosEmpresa.estado == "ES" || dadosEmpresa.estado == "PB")
                {
                    perfil = "Y";
                }

                if (dadosEmpresa.estado == "BA")
                    perfil = "Z";

            }
        }

        // Comparação da Hora do ECF com o Terminal
        private static void DiferencaDataHoraECF()
        {
            horaDivergenteECF = false;
            FuncoesECF fecf = new FuncoesECF();
            DateTime horaECF = FuncoesECF.DataHoraDoECF(idECF);            
            DateTime horaTerminal = DateTime.Now;
            TimeSpan span = horaECF.Subtract(horaTerminal);

            if (Math.Abs(span.TotalMinutes) > 60)
            {
                idECFHoraDivergente = idECF;
                fecf.LeituraX();
                horaDivergenteECF = true;
                throw new Exception("Existe diferença de data e hora entre o Terminal e o ECF. Ajuste a data e a hora do terminal. Hora ECF: "+horaECF.ToString()+"  Hora Computador: "+DateTime.Now.ToString() );
            }         
        }

        public static void migrarXML()
        {
            XDocument xmlTerminal = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Terminal.xml");
            var config = from c in xmlTerminal.Descendants("ECF")
                         select new
                         {
                             prevenda = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("preVenda").Value), GlbVariaveis.glbSenhaIQ),
                             pdv = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("pdv").Value), GlbVariaveis.glbSenhaIQ),
                             davPorECF = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("davPorECF").Value), GlbVariaveis.glbSenhaIQ),
                             davPorImpressoraNaoFiscal = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("davPorImpressoraNaoFiscal").Value), GlbVariaveis.glbSenhaIQ),
                             filial = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("filial").Value), GlbVariaveis.glbSenhaIQ),
                             tef = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("tef").Value), GlbVariaveis.glbSenhaIQ),
                             tefDedicado = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("tefDedicado").Value), GlbVariaveis.glbSenhaIQ),
                         };

            foreach (var item in config)
            {
                if (File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Terminal.xml"))
                    File.Delete(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Terminal.xml");

                XDocument doc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "true"),
                    new XElement("Terminal",
                        new XElement("ECF",
                            new XElement("filial", Funcoes.CriptografarComSenha(item.filial, GlbVariaveis.glbSenhaIQ)),
                            new XElement("preVenda", Funcoes.CriptografarComSenha(item.prevenda, GlbVariaveis.glbSenhaIQ)),
                            new XElement("pdv", Funcoes.CriptografarComSenha(item.pdv, GlbVariaveis.glbSenhaIQ)),
                            new XElement("davPorECF", Funcoes.CriptografarComSenha(item.davPorECF, GlbVariaveis.glbSenhaIQ)),
                            new XElement("davPorImpressoraNaoFiscal", Funcoes.CriptografarComSenha(item.davPorImpressoraNaoFiscal, GlbVariaveis.glbSenhaIQ)),
                            new XElement("tef", Funcoes.CriptografarComSenha(item.tef, GlbVariaveis.glbSenhaIQ)),
                            new XElement("tefDedicado", Funcoes.CriptografarComSenha(item.tefDedicado, GlbVariaveis.glbSenhaIQ)),
                            new XElement("modoGaveta", Funcoes.CriptografarComSenha("S", GlbVariaveis.glbSenhaIQ)),
                            new XElement("Metro", Funcoes.CriptografarComSenha("S", GlbVariaveis.glbSenhaIQ)),
                            new XElement("Teclado", Funcoes.CriptografarComSenha("N", GlbVariaveis.glbSenhaIQ)),
                            new XElement("tipoConexao", Funcoes.CriptografarComSenha("1", GlbVariaveis.glbSenhaIQ)),
                            new XElement("tefACBR", Funcoes.CriptografarComSenha("N", GlbVariaveis.glbSenhaIQ)),
                            new XElement("davImpressoaCarta", Funcoes.CriptografarComSenha("S", GlbVariaveis.glbSenhaIQ))
                            )));
                doc.Save(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Terminal.xml");

            }
        }

        public static void lerXML()
        {
            //MessageBox.Show(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Terminal.xml");

            XDocument xmlTerminal = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Terminal.xml");
            var config = from c in xmlTerminal.Descendants("ECF")
                         select new
                         {
                             prevenda = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("preVenda").Value), GlbVariaveis.glbSenhaIQ),
                             pdv = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("pdv").Value), GlbVariaveis.glbSenhaIQ),
                             davPorECF = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("davPorECF").Value), GlbVariaveis.glbSenhaIQ),
                             davPorImpressoraNaoFiscal = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("davPorImpressoraNaoFiscal").Value), GlbVariaveis.glbSenhaIQ),
                             tef = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("tef").Value), GlbVariaveis.glbSenhaIQ),
                             tefDedicado = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("tefDedicado").Value), GlbVariaveis.glbSenhaIQ),
                             modoGaveta = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("modoGaveta").Value), GlbVariaveis.glbSenhaIQ),
                             Metro = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Metro").Value), GlbVariaveis.glbSenhaIQ),
                             tecladoVirtual = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("Teclado").Value), GlbVariaveis.glbSenhaIQ),
                             tipoConexao = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("tipoConexao").Value), GlbVariaveis.glbSenhaIQ),
                             davImpressaoCarta = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("davImpressoaCarta").Value), GlbVariaveis.glbSenhaIQ),
                             tefACBR = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(c.Element("tefACBR").Value), GlbVariaveis.glbSenhaIQ),
                         };

            foreach (var item in config)
            {
                prevenda = item.prevenda == "Sim" ? true : false;
                pdv = item.pdv == "Sim" ? true : false;
                davPorECF = item.davPorECF == "Sim" ? true : false;
                davporImpressoraNaoFiscal = item.davPorImpressoraNaoFiscal == "Sim" ? true : false;
                tefDiscado = item.tef == "Sim" ? true : false;
                tefDedicado = item.tefDedicado == "Sim" ? true : false;
                verificarGaveta = item.modoGaveta == "Sim" ? true : false;
                styleMetro = item.Metro == "S" ? true : false;
                tecladoVirtual = item.tecladoVirtual == "S" ? true : false;
                Conexao.tipoConexao = int.Parse(item.tipoConexao);
                verificarGaveta = item.modoGaveta == "N" ? false : true;
                davImpressaoCarta = item.davImpressaoCarta == "N" ? false : true;
                GlbVariaveis.glb_TEFAcbr = item.tefACBR == "N" ? false : true;
                //MessageBox.Show(pdv.ToString());
            }
        }
    }
}

