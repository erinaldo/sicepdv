using System;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Data.EntityClient;
using System.Data;
using ACBrFramework.ECF;
using System.Data.Objects;



namespace SICEpdv
{
    // Atenção: Havendo algum erro de inicialização o teclado virtual não será exibido por que existem função que retornam false:
    // Exemplo: Caso a pasta c:\iqsistemas\tef\confirmadas não existir haverá uma interupção do fluxo, por que o erro não foi tratado
    // ********** Explicao para TEF Discado e TEF dedidicado
    /// <summary>
    /// As transação de confirmação e não confirmação para o TEF Discado são feitas uma a uma 
    /// para o TEF Dedicado são feitas todas de uma única vez. Por isso ao chamar confirmarTransacao no TEF discado
    /// é passado o parametro com o nome da transação a ser confirmada. No caso do TEF discado é passado no parametro todas
    /// o parametro pode ser passado vazio também. Pois as transação serão pega no laço while de todas as transação em andamento
    /// </summary>
    public class FuncoesECF
    {
       [DllImport("user32.dll", CharSet=CharSet.Auto)]         
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

       [DllImport("user32.dll")]
       public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow); 

        
        #region Constants
        private const string TRANSACAO_APROVADA = "0";
        private const string CONSULTA_CDC = "41";
        private const string CONSULTA_CHEQUE = "70";
        #endregion Constants

        public int documento;
        public int numeroPreVenda = 0;
        public int numeroDAV = 0;        
        public string pathSolicitacaoTEF { get; set; }
        public string pathRespostaTEF { get; set; }
        public string statusTransacaoTEF { get; set; }
        public static bool verificarGaveta { get; set; }
        public static List<formaPagamentoAcbr> listaFormapagamentoAcbr = new List<formaPagamentoAcbr>();
        public static List<formaPagamentoAcbr> listaComprovanteNaoFiscal = new List<formaPagamentoAcbr>();
        //public static string conteudoImpressao = "";

        public static ACBrFramework.ECF.ACBrECF ecfAcbr = new ACBrFramework.ECF.ACBrECF();
        public static ACBrFramework.BAL.ACBrBAL balancaAcbr = new ACBrFramework.BAL.ACBrBAL();
      
       
       
        

        //Construtor
        public FuncoesECF()
        {           
            pathSolicitacaoTEF = @"C:\TEF_DIAL\req"; 
            pathRespostaTEF = @"C:\TEF_DIAL\resp";
        }        

        public void detectarBalanca()
        {
            balancaAcbr.Modelo = ACBrFramework.BAL.ModeloBal.Filizola;
        }

        //internal static ImpressoraFiscal printer = ImpressoraFiscal.Construir();

        private void VerificaConexaoECF(bool verificarImpressoraLigada=true)
        {

            if (ConfiguracoesECF.idNFC > 0 )
                return;

            if (ConfiguracoesECF.pdv == false)
                return;


            if (!File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml") && ConfiguracoesECF.pdv)
            {
                throw new Exception("Arquivo de cadastro de ECFs não foi encontrado");
            }

            if ( ConfiguracoesECF.idECF == 0  && ConfiguracoesECF.pdv)
                throw new Exception("Equipamento Fiscal sem comunicação, desligada, sem papel ou data e hora diferentes do ECF!");

            try
            {
                if (verificarImpressoraLigada)
                {
                    System.Threading.Thread.Sleep(400);
                    VerificaImpressoraLigada();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static bool VerificaFabricacaoGrandeTotal(bool obterECF=true)
        {

            if (ConfiguracoesECF.idNFC > 0)
                return true;

            if (ConfiguracoesECF.pdv == false)
                return true;

            if (ConfiguracoesECF.prevenda == true || ConfiguracoesECF.davporImpressoraNaoFiscal || ConfiguracoesECF.davPorECF)
                return true;

            if (ConfiguracoesECF.pdv == true && ConfiguracoesECF.idECF == 0)
                return false;

            if (FuncoesECF.VerificarR02("CRZ") == false || FuncoesECF.VerificarR02("VendaBruta") == false)
                return true;

            if (!File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml") && ConfiguracoesECF.pdv)
            {
                ConfiguracoesECF.idECF = 0;
                return false;
            }

            string nrFabricacaoECF = ConfiguracoesECF.nrFabricacaoECF; // NumeroFabricacaoECF();
            decimal gtECF = 0;
            if (obterECF)
               gtECF = GrandeTotal();
            if (!obterECF)
                gtECF = ConfiguracoesECF.grandeTotal;

            try
            {
                XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");
                var config = from n in doc.Descendants("ecf")
                             where ((n.Attribute("numeroFabricacaoCriptografado").Value == Funcoes.CriptografarComSenha(nrFabricacaoECF, GlbVariaveis.glbSenhaIQ)) || n.Attribute("numeroFabricacaoCriptografado").Value == Funcoes.CriptografarComSenha(nrFabricacaoECF.Substring(0, 15), GlbVariaveis.glbSenhaIQ))
                             && n.Attribute("gtCriptografado").Value == Funcoes.CriptografarComSenha(gtECF.ToString(), GlbVariaveis.glbSenhaIQ)
                             select n;
                if (config.Count() == 0)
                {
                    return RecomporArquivoCadastroECF();
                }

            }
            catch (Exception)
            {
               
                 File.Delete(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");
                 RecomporArquivoCadastroECF(true);
                 FuncoesPAFECF.SalvarMd5ArquivoECF();
                 return false;
            }
            return true;
        }

        public static bool RecomporArquivoCadastroECF(bool novo = false, bool fabricao = true, bool GTFabricacao = true)
        {

            if (ConfiguracoesECF.idNFC > 0)
                return true;

            if (ConfiguracoesECF.pdv == false)
                return true;

            bool recompor = true;

            if (novo == false)
            {
                XDocument docGT = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");
                var config = from n in docGT.Descendants("ecf")
                             select n;
            }

            if (!Conexao.onLine)
                recompor = true;

            if (Conexao.onLine)
            {
                //FuncoesECF fecf = new FuncoesECF();
                //string croECF = fecf.CRO();
                //string crzECF = fecf.CRZ();


                // TESTE
                //string sql = "SELECT cro from r02 WHERE id=(SELECT MAX(id) from r02) AND fabricacaoECF='" + ConfiguracoesECF.nrFabricacaoECF + "'";
                //string cro = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();
                //sql = "SELECT crz from r02 WHERE id=(SELECT MAX(id) from r02) AND fabricacaoECF='" + ConfiguracoesECF.nrFabricacaoECF + "'";
                //string crz = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();
                //sql = "SELECT vendabrutadiaria from r02 WHERE id=(SELECT MAX(id) from r02) AND fabricacaoECF='" + ConfiguracoesECF.nrFabricacaoECF + "'";
                //string vendaBrutaDiaria = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();

                //FuncoesECF fecf = new FuncoesECF();
                //string croECF = fecf.CRO();
                //string crzECF = fecf.CRZ();

                //sql = "SELECT usuario from agenda limit 1";
                //string sqlReco = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();

                //if (sqlReco == "S")
                //    recompor = true;
                //if (sqlReco == "N")
                //    recompor = false;
            }

            //if (config.Attributes("gtAutomatico").First().Value == "Sim")
            string numeroFabricacaoCriptografado = "";
            string granteTotalCriptografado = "";

            string nrFabricacaoECF = ConfiguracoesECF.nrFabricacaoECF;

            if (fabricao == true && GTFabricacao == false)
            {
                try
                {
                    XDocument docGT = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");
                    var config = from n in docGT.Descendants("ecf")
                                 select n;
                    numeroFabricacaoCriptografado = Funcoes.CriptografarComSenha(ConfiguracoesECF.nrFabricacaoECF, GlbVariaveis.glbSenhaIQ);
                    granteTotalCriptografado = config.FirstOrDefault().Attribute("gtCriptografado").Value.ToString();
                }
                catch (Exception erro)
                {
                    MessageBox.Show("Erro" + MessageBox.Show(erro.Message));
                }
            }
            else if (fabricao == false && GTFabricacao == true)
            {
                try
                {
                    XDocument docGT = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");
                    var config = from n in docGT.Descendants("ecf")
                                 select n;
                    granteTotalCriptografado = Funcoes.CriptografarComSenha(FuncoesECF.GrandeTotal().ToString(), GlbVariaveis.glbSenhaIQ);
                    numeroFabricacaoCriptografado = config.FirstOrDefault().Attribute("numeroFabricacaoCriptografado").Value.ToString();
                }
                catch (Exception erro)
                {
                    MessageBox.Show("Erro" + MessageBox.Show(erro.Message));
                }
            }
            else
            {
                numeroFabricacaoCriptografado = Funcoes.CriptografarComSenha(ConfiguracoesECF.nrFabricacaoECF, GlbVariaveis.glbSenhaIQ);
                granteTotalCriptografado = Funcoes.CriptografarComSenha(FuncoesECF.GrandeTotal().ToString(), GlbVariaveis.glbSenhaIQ);
            }


            if (recompor)
            {                

                // Recompondo
                //string numeroFabricacaoCriptografado = Funcoes.CriptografarComSenha(ConfiguracoesECF.nrFabricacaoECF, GlbVariaveis.glbSenhaIQ);
                //string granteTotalCriptografado = Funcoes.CriptografarComSenha(FuncoesECF.GrandeTotal().ToString(), GlbVariaveis.glbSenhaIQ);

                XDocument doc = new XDocument(
               new XDeclaration("1.0", "utf-8", "true"),
               new XElement("configuracoes-pafecf",
                   new XElement("ecf",
                       new XAttribute("numeroFabricacaoCriptografado", numeroFabricacaoCriptografado),
                       new XAttribute("gtCriptografado", granteTotalCriptografado),
                       new XAttribute("codigoNacionalECF", Funcoes.CriptografarComSenha("0001", GlbVariaveis.glbSenhaIQ)),
                       new XAttribute("md5", Funcoes.CriptografarComSenha(FuncoesPAFECF.LerMD5(), GlbVariaveis.glbSenhaIQ)),                       
                       new XAttribute("gtAutomatico", "Sim")
                       )));

                doc.Save(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");


                FuncoesECF.GravarGtECF();
                return true;
            }

            return false;

        }

        public static void AbrirPorta(int ecf)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return;

            if (ConfiguracoesECF.pdv == false)
                return ;

            switch (ecf)
            {
                case 4:
                    Sweda32.iRetorno = Sweda32.ECF_AbrePortaSerial();
                    break;
            }
        }

        public static void FecharPorta(int ecf)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return;

            switch (ConfiguracoesECF.idECF)
            {
                case 4:
                    Sweda32.iRetorno = Sweda32.ECF_FechaPortaSerial();
                    break;
            }
        }

        public bool AbrirCupom(string vendedor,string nomeConsumidor,string cpfcnjpConsumidor,string endConsumidor,bool verificarPendente=false)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;

            if (ConfiguracoesECF.pdv == false)
                return true;

            Venda.dadosConsumidor =
                          new DadosConsumidorCupom
                          {
                              cpfCnpjConsumidor = "",
                              nomeConsumidor = "",
                              endConsumidor = "",
                              endNumero = "",
                              endBairro = "",
                              endCEP = "",
                              endCidade = "",
                              endEstado = "",
                              idConsumidor = ""
                          };
                                    
            nomeConsumidor = nomeConsumidor.PadRight(30, ' ').Substring(0, 30) ?? " ";

            // Aqui passa a usar o Emissor Daruma Migrate
            if (ConfiguracoesECF.idNFC==1)
            {

                return true;
            }



            if (ConfiguracoesECF.zPendente && ConfiguracoesECF.pdv && verificarPendente)            
                throw new Exception("Redução Z pendente. Tecle F6 e emita redução Z.");
            if (ConfiguracoesECF.reducaoZEmitida && ConfiguracoesECF.pdv && verificarPendente)
                throw new Exception("Redução Z do dia já foi emitida");

            try
            {
                if (File.Exists("versaoframework.txt"))
                    VerificaConexaoECF(true);
                else
                    VerificaConexaoECF(false);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            ComprovanteNaoFiscalVinculado("fechar", "", "", "", "");
            
            if (!VerificaFabricacaoGrandeTotal(false))
                throw new Exception("GT e ou número de Fabricação não conferem com o cadastro das Ecf's.");

            try
            {
                FuncoesPAFECF.LerMD5();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {

                    case 1:
                        if (CupomFiscalAberto())
                            return true;

                        int iRetorno = BemaFI32.Bematech_FI_AbreCupomMFD(cpfcnjpConsumidor ?? " ", nomeConsumidor.PadRight(30, ' ').Substring(0, 30) ?? " ", endConsumidor ?? "");

                        if (iRetorno != 1)
                            throw new Exception("Não foi possível abrir cupom. " + iRetorno.ToString());
                        break;


                    case 2:
                        if (CupomFiscalAberto())
                            return true;


                        DARUMA_FW.iRetorno = DARUMA_FW.iCFAbrir_ECF_Daruma(cpfcnjpConsumidor, nomeConsumidor, endConsumidor);
                        if (DARUMA_FW.iRetorno != 1)
                            throw new Exception("Não foi possível abrir cupom. Erro ECF Nr: " + DARUMA_FW.iRetorno.ToString());
                        return true;

                    case 3:
                        if (CupomFiscalAberto())
                            return true;


                        Elgin32.Int_Retorno = Elgin32.Elgin_AbreCupomMFD(cpfcnjpConsumidor, nomeConsumidor, endConsumidor);
                        if (Elgin32.Int_Retorno != 1)
                            throw new Exception("Não foi possível abrir cupom. Erro ECF NR: " + Elgin32.Int_Retorno.ToString());
                        break;

                    case 4:
                        if (CupomFiscalAberto())
                            return true;


                        Sweda32.iRetorno = Sweda32.ECF_AbreCupom(cpfcnjpConsumidor);
                        if (Sweda32.iRetorno != 1)
                            throw new Exception("Não foi possível abrir cupom. Erro ECF Nr: " + Sweda32.iRetorno.ToString());
                        break;


                }
            }
            else
            {
                if (!CupomFiscalAberto())
                {
                    if (cpfcnjpConsumidor != "")
                        ecfAcbr.AbreCupom(cpfcnjpConsumidor, nomeConsumidor.PadRight(30, ' ').Substring(0, 30) ?? " ", endConsumidor ?? "", false);
                    else
                        ecfAcbr.AbreCupom();

                    return true;
                }
                    
                    throw new Exception("Não foi possível abrir cupom. Erro ECF");

            }

            return true;
        }
                                
        public bool VenderItemECF(string codigo,string descricao, decimal preco,decimal descontoPerc,decimal descontoValor,decimal acrescimentoPerc,decimal acrescimoValor, decimal quantidade, string unidade,string tributacao,int aliquota, string tipo, string ncm, string cest)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;

            if (ConfiguracoesECF.pdv == false)
                return true;

            VerificaConexaoECF(false);

            try
            {
                FuncoesPAFECF.LerMD5();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           

            string tipoDesconto = "%";
            decimal desconto = 0;
            string tipoDescontoAcrecimoAcbr = "D";
            decimal descontoAcrescimoAcbr = 0;

            string tipoAliquota = "ICMS";       
            if (tipo.Substring(0,1)=="1")
            tipoAliquota="ISS";


            bool tributado = false;
            #region Aliquota ICMS ou ISS
            // Aqui atribui os valores do ICMS a variavel tributacao
            if (tributacao == "00" || tributacao == "02" || tributacao == "20" || tributacao=="101")
            {
                tributado = true;
                switch (aliquota)
                {
                    case 0:
                        tributacao = ConfiguracoesECF.icmsIsencaoII;
                        break;
                    default:
                        if (GlbVariaveis.glb_Acbr == true)
                            tributacao = Funcoes.FormatarZerosEsquerda(aliquota, 4,true)+"T"; // aliquota.ToString() + "00";
                        else
                            tributacao = Funcoes.FormatarZerosEsquerda(aliquota, 4,true); // aliquota.ToString() + "00";
                        break;
                }
            }
            
            // Tributacao do Produto tem Precedencia 
            // Se for mudado alguma coisa aqui alterrar as funcoes 
            // fiscais. Paf.cs metodo Relatorios R
            if (!tributado)
            {
                switch (tributacao)
                {
                    case "01":
                    case "10":
                    case "06":
                    case "60":
                    case "03":
                    case "30":
                    case "70":
                    case "07":
                        // SIMPLES NACIONAL
                    case "500":
                        tributacao = tipoAliquota == "ICMS" ? ConfiguracoesECF.icmsSubFF : ConfiguracoesECF.issSubSF;
                        break;
                    case "04":
                    case "40":
                        tributacao = tipoAliquota == "ICMS" ? ConfiguracoesECF.icmsIsencaoII : ConfiguracoesECF.issIsencaoSI;
                        break;
                    case "41":
                        //Simples Nacional
                        tributacao = tipoAliquota == "ICMS" ? ConfiguracoesECF.icmsNaoIncNN : ConfiguracoesECF.issNaoIncSN;
                        break;
                    case "300":
                    case "102":
                    case "400":                    
                        tributacao = tipoAliquota == "ICMS" ? ConfiguracoesECF.icmsIsencaoII : ConfiguracoesECF.issIsencaoSI;
                        break;
                    case "08":
                    case "80":
                        tributacao = tipoAliquota == "ICMS" ? ConfiguracoesECF.icmsNaoIncNN : ConfiguracoesECF.issNaoIncSN;
                        break;
                    case "50":
                        tributacao = tipoAliquota == "ICMS" ? ConfiguracoesECF.icmsNaoIncNN : ConfiguracoesECF.issNaoIncSN;
                        break;                    
                    default:
                        break;
                }
            }
            #endregion


            descricao = cest+"#"+ncm+"#"+descricao;

            if (GlbVariaveis.glb_Acbr == false)
            {
                #region
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        desconto = descontoPerc / 10;

                        if (descontoValor > 0)
                        {
                            desconto = descontoValor;
                            tipoDesconto = "$";
                        }

                        int iRetorno = 0;
                        iRetorno = BemaFI32.Bematech_FI_AumentaDescricaoItem(descricao);//.Trim().PadRight(200, ' ').Substring(0, 200)
                        if (iRetorno != 1)
                        {
                            if (iRetorno == 0)
                            {
                                if (VerificaImpressoraLigada(true) == false)
                                    throw new Exception("Não é possível aumentar a descrição do item. Retorno.:" + iRetorno.ToString());
                            }
                            else
                            {
                                    throw new Exception("Não é possível aumentar a descrição do item. Retorno.:" + iRetorno.ToString());
                            }
                        }
                           
                        iRetorno = 0;

                        iRetorno = BemaFI32.Bematech_FI_VendeItem(codigo, descricao, tributacao, "F", string.Format("{0:N3}", quantidade), 3, string.Format("{0:N2}", preco * 10), tipoDesconto, string.Format("{0:N2}", desconto));
                        if (iRetorno != 1)
                        {
                            if (descricao.Length > 29)
                                descricao = descricao.Substring(0, 29);  

                            if(iRetorno == -2)
                                iRetorno = BemaFI32.Bematech_FI_VendeItem(codigo, descricao, tributacao, "F", string.Format("{0:N3}", quantidade), 3, string.Format("{0:N2}", preco * 10), tipoDesconto, string.Format("{0:N2}", desconto));

                            if(iRetorno != 1)
                                throw new Exception("Não foi possível imprimir item: " + iRetorno.ToString());
                        }


                        if (acrescimoValor > 0)
                        {
                            string numeroItem = new string(' ', 4);
                            BemaFI32.Bematech_FI_UltimoItemVendido(ref numeroItem);
                            System.Threading.Thread.Sleep(100);
                            iRetorno = BemaFI32.Bematech_FI_AcrescimoDescontoItemMFD(Convert.ToInt16(numeroItem).ToString(), "A", "$", string.Format("{0:N2}", acrescimoValor));
                        }

                        break;

                    case 2:
                        /*
                         * Importante! Caso seja passado um sinal de subtração
                         antes do valor exemplo: (-1,00), será aplicada a lógica inversa e ao invéz de um desconto
                         será feito um acréscimo.
                         */

                        try
                        {
                            desconto = descontoPerc;
                            tipoDesconto = "D%";
                            if (descontoValor > 0)
                            {
                                desconto = descontoValor;
                                tipoDesconto = "D$";
                            }


                            //if (!VerificaFabricacaoGrandeTotal())
                            //    throw new Exception("GT e ou número de Fabricação não conferem com o cadastro das Ecf's");

                            if (acrescimoValor > 0)
                            {
                                tipoDesconto = "A$";
                                desconto = Convert.ToDecimal(string.Format("{0:N2}", (acrescimoValor) * -1));
                            }

                            DARUMA_FW.iRetorno = DARUMA_FW.iCFVender_ECF_Daruma(tributacao, string.Format("{0:N3}", quantidade),
                                string.Format("{0:N2}", preco), tipoDesconto, string.Format("{0:N2}", desconto), codigo, unidade, descricao);

                            if (DARUMA_FW.iRetorno != 1)
                                throw new Exception("Não foi possível imprimir item " + DARUMA_FW.iRetorno.ToString());
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Não foi possível imprimir item " + ex.Message);
                        }
                        break;

                    case 3:
                        desconto = descontoPerc / 10;
                        if (descontoValor > 0)
                        {
                            desconto = descontoValor;
                            tipoDesconto = "$";
                        }


                        Elgin32.Int_Retorno = Elgin32.Elgin_VendeItem(codigo, descricao.PadLeft(200, ' ').Substring(0, 200).Trim(), tributacao, "F", string.Format("{0:N3}", quantidade), 3, string.Format("{0:N2}", preco * 10), tipoDesconto, string.Format("{0:N2}", desconto));
                        if (Elgin32.Int_Retorno != 1)
                            throw new Exception("Não foi possível imprimir item: " + Elgin32.Int_Retorno.ToString());

                        if (acrescimoValor > 0)
                        {

                            string numeroItem = new string(' ', 4);
                            Elgin32.Elgin_UltimoItemVendido(ref numeroItem);
                            System.Threading.Thread.Sleep(100);
                            Elgin32.Int_Retorno = Elgin32.Elgin_AcrescimoDescontoItemMFD(Convert.ToInt16(numeroItem.Trim()).ToString(), "A", "$", string.Format("{0:N2}", acrescimoValor));

                        }
                        break;

                        break;
                    case 4:
                        desconto = descontoPerc / 10;

                        if (descontoValor > 0)
                        {
                            desconto = descontoValor;
                            tipoDesconto = "$";
                        }
                        Sweda32.iRetorno = Sweda32.ECF_VendeItem(codigo, descricao.PadLeft(30, ' ').Substring(0, 30), tributacao, "F", string.Format("{0:N3}", quantidade), 3, string.Format("{0:N2}", preco * 10), tipoDesconto, string.Format("{0:N2}", desconto));
                        if (Sweda32.iRetorno != 1)
                            throw new Exception("Não foi possível imprimir item: " + Sweda32.iRetorno.ToString());
                        break;
                }
                #endregion
            }
            else
            {
                try
                {

                    if (ConfiguracoesECF.pdv == true)
                    {

                        if (acrescimoValor > 0)
                        {
                            tipoDescontoAcrecimoAcbr = "A";
                            descontoAcrescimoAcbr = acrescimoValor;
                        }
                        else
                        {
                            tipoDescontoAcrecimoAcbr = "D";
                            descontoAcrescimoAcbr = descontoValor;
                        }
                        ecfAcbr.DescricaoGrande = true;
                        ecfAcbr.VendeItem(codigo, descricao.PadRight(200, ' ').Substring(0, 200), tributacao.Replace("0", ""), quantidade, preco, descontoAcrescimoAcbr, unidade, "$", tipoDescontoAcrecimoAcbr, 0);
                    }

                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.ToString());
                }
            }
            GravarGtECF();
            return true;
        }

        public bool ApagarItemECF(int itemNr)
        {
            if (ConfiguracoesECF.idNFC > 0 || ConfiguracoesECF.pdv == false)
                return true;

            VerificaConexaoECF();
            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        //try
                        //{
                        //    printer.Cupom.CancelarItem(itemNr);
                        //    GravarGtECF();
                        //}
                        //catch (BematechException erro)
                        //{
                        //    throw new Exception(erro.Message);
                        //}                    
                        //break;

                        int iRetorno = BemaFI32.Bematech_FI_CancelaItemGenerico(itemNr.ToString());
                        if (iRetorno != 1)
                            throw new Exception(itemNr.ToString());
                        GravarGtECF();
                        break;
                    case 2:
                        DARUMA_FW.iRetorno = DARUMA_FW.iCFCancelarItem_ECF_Daruma(itemNr.ToString());
                        return true;
                    case 3:
                        Elgin32.Elgin_CancelaItemGenerico(itemNr.ToString());
                        return true;
                    case 4:
                        Sweda32.ECF_CancelaItemGenerico(itemNr.ToString());
                        return true;
                }
            }
            else
            {
                try
                {
                    ecfAcbr.CancelaItemVendido(itemNr);
                    return true;
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    return false;
                }
            }
            return true;
        }

        public bool IniciarFechamentoECF(decimal valorDesconto,decimal valorAcrescimo)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;

            if (ConfiguracoesECF.pdv == false)
                return true;

            decimal valor = valorDesconto;

            string tipoAD = "D";
           


            if (valorDesconto == valorAcrescimo)
            {
                valor = 0;
            }
            else if (valorDesconto > valorAcrescimo)
            {
                if(GlbVariaveis.glb_Acbr == true)
                    valor = -(valorDesconto - valorAcrescimo);
                else
                    valor = (valorDesconto - valorAcrescimo);
                    
                tipoAD = "D";
            }
            else
            {
                valor = (valorAcrescimo - valorDesconto);
                tipoAD = "A";
            }

           


            if (TotalPagoCupomECF() > 0)
                return true;

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        int iRetorno = BemaFI32.Bematech_FI_IniciaFechamentoCupom(tipoAD, "$", string.Format("{0:N2}", valor).ToString());
                        if (iRetorno != 1)
                            throw new Exception("Não foi possível inciar fechamento: Retorno: " + iRetorno.ToString());
                        break;

                    case 2:
                        DARUMA_FW.iRetorno = DARUMA_FW.iCFTotalizarCupom_ECF_Daruma(tipoAD + "$", valor.ToString());
                        //if (DARUMA32.Int_Retorno!=1)
                        //   throw new Exception("Não foi possível inciar fechamento");
                        break;
                    case 3:
                        Elgin32.Int_Retorno = Elgin32.Elgin_IniciaFechamentoCupom(tipoAD, "$", string.Format("{0:N2}", valor).ToString());
                        if (Elgin32.Int_Retorno != 1)
                            throw new Exception("Não foi possível iniciar fechamento");
                        break;
                    case 4:

                        Sweda32.iRetorno = Sweda32.ECF_IniciaFechamentoCupom(tipoAD, "$", valor.ToString());
                        if (Sweda32.iRetorno != 1)
                            throw new Exception("Não foi possível iniciar fechamento");
                        break;
                }
            }
            else
            {
                try
                {
                    if (ConfiguracoesECF.pdv == true)
                    {
                       
                        ecfAcbr.SubtotalizaCupom(valor);
                    }
                  
                    
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }
            }


            return true;
        }

        public bool ChamarPagamentoECF(string formaPagamento,int idCartao,decimal valor,bool imprimirECF=true,string descricaoPgtImpressao="",bool chamarGerenciadorTEF=true)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;

            if (ConfiguracoesECF.pdv == false)
                return true;
            
            string descricaoPagamento =" ";          
            
            switch (formaPagamento)
            {
                case "DH":
                    descricaoPagamento = ConfiguracoesECF.DH;                
                    break;
                case "AV":
                    descricaoPagamento = ConfiguracoesECF.AV;
                    break;
                case "CA":
                case "FN":
                case "PF":
                case "CP":
                    if (formaPagamento == "PF")
                        descricaoPgtImpressao = "Fidelidade";

                    if (formaPagamento == "CP") 
                    descricaoPgtImpressao = "Cartao Presente";

                    if (descricaoPgtImpressao == "")
                        descricaoPagamento = ConfiguracoesECF.CA;
                    else
                    {
                        descricaoPagamento = descricaoPgtImpressao;
                        ConfiguracoesECF.CA = descricaoPgtImpressao;
                    }

                    break;
                case "CH":
                    descricaoPagamento = ConfiguracoesECF.CH;
                    break;
                case "CR":
                    descricaoPagamento = ConfiguracoesECF.CR;
                    break;
                case "TI":
                    descricaoPagamento = ConfiguracoesECF.TI;
                    break;
                case "DV":
                    descricaoPagamento = ConfiguracoesECF.DV;
                    break;
            }

            string msgRodapeCupom = "";

            #region Mensagem Rodapé Cupom

            msgRodapeCupom = MensagemCupomECF(valor,numeroPreVenda,numeroDAV);

            #endregion

            switch (ConfiguracoesECF.idECF)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    try
                    {
                        if(GlbVariaveis.glb_Acbr == false)
                            System.Threading.Thread.Sleep(400);

                        if (imprimirECF)
                        VerificaImpressoraLigada(false);
                    }
                    catch (Exception erro)
                    {

                        MessageBox.Show(erro.Message, "SICE.pv", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    try
                    {
                        decimal valorPagamento = valor;


                        if (!imprimirECF)
                            return true;

                        // tratamento de desligamento da impressora no cupom fiscal
                        while (true)
                        {
                            Funcoes.TravarTeclado(true);
                            try
                            {

                                if (EfetuarPagamentoECF(descricaoPagamento, valor))
                                    break;
                                else
                                    Funcoes.TravarTeclado(false);
                                if (MessageBox.Show("Impressora não responde. Tentar imprimir novamente ?", "SICEpdv.net", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
                                {
                                    NaoConfirmarTransacao();
                                    return false;
                                }
                                else
                                {
                                    Funcoes.TravarTeclado(true);
                                    if (EfetuarPagamentoECF(descricaoPagamento, valor))
                                        break;
                                }
                            }
                            catch (Exception erro)
                            {
                                Funcoes.TravarTeclado(false);
                                frmOperadorTEF formOperador = new frmOperadorTEF(erro.Message, false);
                                formOperador.ShowDialog();
                                formOperador.Dispose();
                                return false;
                            }
                        }

                        return true;

                    }
                    catch (Exception erro)
                    {
                        Funcoes.TravarTeclado(false);
                        frmOperadorTEF formOperador = new frmOperadorTEF(erro.Message, false);
                        formOperador.ShowDialog();
                        formOperador.Dispose();
                        return false;
                    }                  
            }

            return true;
        }

        public static string md5()
        {
            string line = "";

            if (System.IO.File.Exists(@"PDVlog.txt"))
            {
                StreamReader file = new StreamReader(@"PDVlog.txt");
                
                while ((line = file.ReadLine()) != null)
                {
                    return line;
                }
            }
            return line;
        }

        public static string MensagemCupomECF(decimal valor,int numeroPreVenda, int numeroDAV, int documento = 0)
        {
            //if (ConfiguracoesECF.idNFC > 0)
            //{
            //    return Configuracoes.mensagemRodapeCupom.Trim();
            //}

            string msgRodapeCupom = "";


            if (ConfiguracoesECF.idNFC == 0 && ConfiguracoesECF.pdv)
            {

                if (!File.Exists("PDVlog.txt"))
                    msgRodapeCupom = "MD-5: " + ConfiguracoesECF.md5Geral.PadRight(32, ' ').Substring(0, 32) + Environment.NewLine;
                else
                    msgRodapeCupom = "MD-5: " + md5().PadRight(32, ' ').Substring(0, 32) + Environment.NewLine;
            }
            
            if(!string.IsNullOrEmpty(Venda.IQCard))
            {
                string aviso = "";
                if (IqCard.saldoInsuficiente)
                {
                    aviso = "(Local sem pontos IQCARD - Fale com o gerente )";
                }
                string infoPontuacao = "";

                if(!string.IsNullOrEmpty(IqCard.idRegistroPontosIQCARD))
                {
                    infoPontuacao = " PONTOS( " + IqCard.pontosIQCARD + ")" + "id: " + IqCard.idRegistroPontosIQCARD;
                };

                msgRodapeCupom += "IQCARD: " + Venda.IQCard+" "+aviso +" "+infoPontuacao+ Environment.NewLine;
                
            }
            //Renomeado por que toda venda sai um codigo para ser usado no IQCARD
            //if(Configuracoes.promocaoIQCardAtiva)
            //{
            //    DateTime validade = DateTime.Now;
            //    try
            //    {
            //        var sql = "SELECT validadepromocaoiqcard FROM filiais WHERE  codigofilial='" + GlbVariaveis.glb_filial + "'";
            //         validade = Conexao.CriarEntidade().ExecuteStoreQuery<DateTime>(sql).FirstOrDefault();
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    string codPromocao = IqCard.GerarCodigoPromocao();
            //    msgRodapeCupom += "USE CÓD. NO APLICATIVO IQCARD: " + codPromocao.Substring(0, 4) + " " + codPromocao.Substring(4, 4)+" "+codPromocao.Substring(8,4) +" Validade: "+string.Format("{0:dd/MM/yyyy}",validade) ;
            //};
            try
            {
                string codPromocao = IqCard.GerarCodigoPromocao();
                msgRodapeCupom += "CÓD. PROMOCIONAL IQCARD: " + codPromocao.Substring(0, 4) + " " + codPromocao.Substring(4, 4) + " " + codPromocao.Substring(8, 4)+ Environment.NewLine;
            }
            catch (Exception)
            {
                             
            }
        

           if (Venda.numeroPED > 0)
           {
               msgRodapeCupom += "NF: " + Venda.numeroPED.ToString().PadLeft(9, '0').Substring(0, 9) + " ";
           };


            string cpf= Clientes.ultCPF;

           msgRodapeCupom += Venda.CalculaTributosCupom() + Environment.NewLine;

            string tipoDAV = "DAV";
            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                tipoDAV = "DAV-OS Nr";

            if (numeroPreVenda > 0)
                msgRodapeCupom += ("PV" + Funcoes.FormatarZerosEsquerda(numeroPreVenda, 10, false));
            if (numeroDAV > 0)
                msgRodapeCupom += (tipoDAV + Funcoes.FormatarZerosEsquerda(numeroDAV, 10, false));

            msgRodapeCupom += Configuracoes.mensagemRodapeCupom.Trim() + Environment.NewLine ; // "Ter você como cliente é um privilégio. Obrigado e volte sempre.";
            msgRodapeCupom += "Vendedor: " + Vendedor.nomeVendedor + " Oper: " + GlbVariaveis.glb_Usuario + Environment.NewLine;


            if (Configuracoes.estado == "RJ")
            {
                msgRodapeCupom += "CUPOM MANIA, CONCORRA A PRÊMIOS " + Environment.NewLine +
                    "ENVIE SMS P/ 6789: " + Configuracoes.inscricao.PadLeft(8, '0').Substring(0, 8) +
                    string.Format("{0:ddMMyy}", DateTime.Now.Date).ToString() +
                    FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).PadLeft(6, '0') + ConfiguracoesECF.numeroECF;
            }

            if (Configuracoes.estado == "MG")
            {
                msgRodapeCupom += "MINAS LEGAL: " + Environment.NewLine +
                    Configuracoes.cnpj.PadLeft(14, '0').Substring(0, 14) + " " +
                    string.Format("{0:ddMMyyyy}", DateTime.Now.Date).ToString() + " " +
                    valor.ToString().Replace(".", "").Replace(",", "");
            }

            if (!string.IsNullOrEmpty(Venda.dependente))
            {
                msgRodapeCupom += "Compra pelo Dep.: " + Venda.dependente;
            }

            if (Configuracoes.estado=="PB")
            {                                
                msgRodapeCupom += "PARAÍBA LEGAL - RECEITA CIDADÃ:  " + Environment.NewLine +
                    "TORPEDO PREMIADO:" + Environment.NewLine +
                   Configuracoes.inscricao.PadLeft(9, '0').Substring(0, 9) + " " +
                   string.Format("{0:ddMMyyyy}", DateTime.Now.Date).ToString() + " " +
                   FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).PadLeft(6, '0') + " " + valor.ToString().Replace(".", "").Replace(",", "");                   
            }

            if(!string.IsNullOrEmpty(Venda.dadosEntrega.endereco))
            {
                msgRodapeCupom += "END.ENTREGA RECEBEDOR :"+  Venda.dadosEntrega.recebedor+Environment.NewLine +
                "END.: "+Venda.dadosEntrega.endereco+" NR.:"+Venda.dadosEntrega.numero + Environment.NewLine+
                Venda.dadosEntrega.cep +" BAIRRO : "+ Venda.dadosEntrega.bairro + Environment.NewLine +
                Venda.dadosEntrega.cidade+" ESTADO: "+ Venda.dadosEntrega.estado + Environment.NewLine +
                Venda.dadosEntrega.data + " HORA: "+Venda.dadosEntrega.hora +
                Venda.dadosEntrega.observacao;
            }

            if (documento > 0)
                msgRodapeCupom = msgRodapeCupom + Environment.NewLine + "Documento Gerencial de Venda.: "+documento.ToString();

            return msgRodapeCupom;
        }
        
        public bool ImprimirTEF(bool gerencial=false)
        {
            /*if (ConfiguracoesECF.idNFC > 0)
                return true;*/

            if (GlbVariaveis.glb_TEFAcbr == false)
            {


                if (ImprimirComprovantes(gerencial))
                {
                    // If os comprovantes foram impressos sem problemas
                    // Limpa todas as transações TEF
                    //ConfirmarTransacao(); // confirma a impressão dos comprovantses                
                    //if (ConfiguracoesECF.idECF==1)
                    //printer.TEF.ClearTransacoes(); // limpa a coleção de transações aprovadas
                    TEF.Transacoes("limpar");
                    return true;
                }
                else
                {
                    NaoConfirmarTransacao(); // não confirma a última transação
                    CancelarTransacao(); // cancela as transações confirmadas
                                         //printer.TEF.ClearTransacoes(); // limpa a coleção de transações aprovadas                
                    return false;
                }
            }

            return true;
        }

        private void ConfirmarTransacao(string @transacaoOrigem)
        {
            int idECF = 0;

            if (ConfiguracoesECF.idNFC > 0)
            {
                idECF = 5;
            }
            else
            {
                idECF = ConfiguracoesECF.idECF;
            }

            switch (idECF)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    try
                    {

                        if(System.IO.File.Exists(@"C:\iqsistemas\INTPOS_BKP.BKP"))
                            System.IO.File.Delete(@"C:\iqsistemas\INTPOS_BKP.BKP");

                        System.IO.File.Copy(@"C:\iqsistemas\INTPOS.BKP", @"C:\iqsistemas\INTPOS_BKP.BKP");

                        while (!TEF.VerificaGerenciadorTEF())
                        {
                            MessageBox.Show(TEF.mensagemGerenciadorInativo); // Gerenciador não está ativo
                        }

                        
                        if (!ConfiguracoesECF.tefDedicado)
                        {
                            TEF.ConfirmaTef(@transacaoOrigem);
                        }

                        // Lendo arquivos das transações TEF e os Espelhos de Impressão TEF
                        // Quando o TEF for dedicado confirma todas as transações de uma única vez
                        if (ConfiguracoesECF.tefDedicado)
                        {
                            var arquivosRespostaTEF = ArquivosRespTransacaoTEF();
                            for (int nEspelhos = 0; nEspelhos < arquivosRespostaTEF.Count(); nEspelhos++)
                            {
                                TEF.ConfirmaTef(@arquivosRespostaTEF[nEspelhos]);
                            }
                        }

                        TEF.EncerraTEF();
                        //TEF.Transacoes("limpar");
                        break;
                    }
                    catch (Exception erro)
                    {
                        TEF.EncerraTEF();
                        MessageBox.Show(erro.Message, "PDV.net", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Application.DoEvents();

                    break;
            }
        }

        private bool ImprimirComprovantes(bool gerencial,bool cancelamento=false)
        {
            
            int idECF = 0;
            
            if (ConfiguracoesECF.idNFC > 0)
            {
                idECF = 5;
            }
            else
            {
                idECF = ConfiguracoesECF.idECF;
            }

            // Forma de pagamento para o TEF essas forma de pagamento tem que está gravado no ECF


            frmImpressaoTEF impressao = new frmImpressaoTEF(); 

            switch (idECF)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    Funcoes.TravarTeclado(true);
                    impressao.lblMensagem.Text = TEF.mensagemOperador;
                    impressao.Show();
                    Application.DoEvents();                    
                     
                 
                    FuncoesECF.RelatorioGerencial("Fechar", "","",false);
                    // Obtem os arquivos das transações não confirmadas e as confirmadas 
                    // para juntar numa única lista 

                    List<StructRespotasTEF> respostasTEF = LerRespostasTEF(cancelamento);
                    List<int> nvias = new List<int>();

                    List<List<string>> guardaEspelho = new List<List<string>>();
                    for (int i = 0; i < respostasTEF.Count(); i++)
                    {
                        TEF.espelhoTEF.Clear();
                        TEF.ImprimeTEF(respostasTEF[i].arquivo, "", gerencial); // Aqui se obtem o List do EspelhoTEF                                                                          
                        nvias.Add(TEF.numeroVias);
                        if (TEF.espelhoTEF.Count > 0)
                        {
                            List<string> comprovante = TEF.espelhoTEF.ToList();
                            guardaEspelho.Add(comprovante);
                        }
                    }
                    var nCupom = COONumeroCupomFiscal(ConfiguracoesECF.idECF);
                    for (int nEspelhos = 0; nEspelhos < guardaEspelho.Count(); nEspelhos++)
                    {                        
                        if (!gerencial)
                        {
                            System.Threading.Thread.Sleep(200);    
                            if(TEF.lstPagamento.ToList().Count() > 0)                        
                                FuncoesECF.ComprovanteNaoFiscalVinculado("Abrir",TEF.lstPagamento[nEspelhos].formaPagamento,TEF.lstPagamento[nEspelhos].valor.ToString(),nCupom,"");
                        }

                        //TEF.espelhoTEF.Clear();
                        //TEF.ImprimeTEF(respostasTEF[nEspelhos].arquivo, "", gerencial); // Aqui se obtem o List do EspelhoTEF                                                  


                        // Aqui inicia-se a contagem do número de vias do comprovante
                        for (int ncopia = 0; ncopia < nvias[nEspelhos] ; ncopia++)
                        {
                            if (ncopia == 1)
                            {
                                if (gerencial)
                                    FuncoesECF.RelatorioGerencial("Imprimir", "\r\n\r\n\r\n\r\n\r\n");
                                else
                                    FuncoesECF.ComprovanteNaoFiscalVinculado("Imprimir", "", "","", "\r\n\r\n\r\n\r\n\r\n");

                                impressao.lblMensagem.Text = "Retire a 1a. via do cliente ";
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(3000);
                            }

                            // Aqui começa a ler as linhas da matriz que contem o espelho TEF
                            int linhaVerificaECF = 0;
                            impressao.lblMensagem.Text = TEF.mensagemOperador;
                            Application.DoEvents();
                            StringBuilder dadosImpressao = new StringBuilder();
                            for (int i = 0; i < guardaEspelho[nEspelhos].Count; )
                            {

                                try
                                {
                                    dadosImpressao.AppendLine(guardaEspelho[nEspelhos][i]);

                                    if (linhaVerificaECF == 0 || linhaVerificaECF == 10)
                                    {
                                        Application.DoEvents();
                                        if (VerificaImpressoraLigada() == false)
                                        {
                                            bool loop = true;
                                            while (loop == true)
                                            {
                                                if (MessageBox.Show("Impressora não responde. Tentar imprimir novamente !?", "SICEpv.net", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                                {
                                                    if (VerificaImpressoraLigada() == true)
                                                        loop = false;
                                                    else
                                                        loop = true;
                                                }
                                                else
                                                {
                                                    Funcoes.TravarTeclado(false);
                                                    impressao.Dispose();
                                                    Application.DoEvents();
                                                    return false;
                                                }
                                            }
                                        }
                                        else
                                        {

                                            // transferido para cá {*1} em 22.062012
                                            if (gerencial)
                                                FuncoesECF.RelatorioGerencial("Imprimir", dadosImpressao.ToString());
                                            else
                                                FuncoesECF.ComprovanteNaoFiscalVinculado("Imprimir", "", "", "", dadosImpressao.ToString());
                                        }

                                        linhaVerificaECF = 1;
                                        dadosImpressao.Clear();
                                    }
                                    linhaVerificaECF++;

                                    // Mudando para {*1} em 22.06.2012 para turbinar a impressão do comprovante
                                    //if (gerencial)
                                    //    FuncoesECF.RelatorioGerencial("Imprimir", guardaEspelho[nEspelhos][i]);
                                    //else
                                    //     FuncoesECF.ComprovanteNaoFiscalVinculado("Imprimir", "", "","",guardaEspelho[nEspelhos][i]);
                                       
                                    i += 1;
                                }
                                catch
                                {
                                    Funcoes.TravarTeclado(false);
                                    if (MessageBox.Show("Impressora não responde. Tentar imprimir novamente !?", "SICEpv.net", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                    {
                                        Funcoes.TravarTeclado(true); 
                                        Application.DoEvents();
                                        gerencial = true;
                                        i = 0;
                                        ncopia = 0;
                                        nEspelhos = 0;
                                        FuncoesECF.RelatorioGerencial("Fechar", "");
                                        dadosImpressao.Clear();
                                    }
                                    else
                                    {
                                        Funcoes.TravarTeclado(false);
                                        impressao.Dispose();
                                        Application.DoEvents();
                                        return false;
                                    }
                                }
                            }

                            if (gerencial)
                                FuncoesECF.RelatorioGerencial("Imprimir", dadosImpressao.ToString());
                            else
                                FuncoesECF.ComprovanteNaoFiscalVinculado("Imprimir", "", "", "", dadosImpressao.ToString());
                            dadosImpressao.Clear();

                            if (gerencial)
                                FuncoesECF.RelatorioGerencial("Imprimir", "\r\n\r\n");
                            else
                                FuncoesECF.ComprovanteNaoFiscalVinculado("Imprimir", "", "","","\r\n\r\n");                            
                        };
                        // Para não fechar o relatório gerencial quando for impressão
                        // de comprovantes de 2 cartões que deverá sair 2 comprovantes num único
                        // relatório gerencial se houver falha na impressão do comprovante não fiscal.

                        if (gerencial==false)
                         FuncoesECF.RelatorioGerencial("Fechar", "","",false);                        

                        Funcoes.TravarTeclado(false);
                        //Confirma a Transação depois de Imprimir o Comprovantes
                        // O TEF Discado confirma o comprovante quando impresso por isso 
                        // esta condição no final da impressão da 2o. via


                        if (!ConfiguracoesECF.tefDedicado)
                        {
                            if (!respostasTEF[nEspelhos].transacaoConfirmada)
                            {
                                ConfirmarTransacao(respostasTEF[nEspelhos].arquivo);
                            }
                        }
                    };

                    // Aqui fecha se for relatório gerencial
                    if (gerencial==true)
                        FuncoesECF.RelatorioGerencial("Fechar", "", "", false);

                    if(!gerencial)
                        ComprovanteNaoFiscalVinculado("Fechar", "", "", "", "");

                    if (ConfiguracoesECF.tefDedicado)
                    {
                        ConfirmarTransacao("todas");
                    }                    
                        Funcoes.TravarTeclado(false);
                        impressao.Dispose();
                        Application.DoEvents();                                                
                    break;
            }            
            return true;
        }

        private static List<StructRespotasTEF> LerRespostasTEF(bool cancelamento=false)
        {
            // Juntando as respotas confirmadas e as nao confirmadas numa única coleção para imprimir
            List<StructRespotasTEF> respostasTEF = new List<StructRespotasTEF>();

            if (GlbVariaveis.glb_TEFAcbr == false)
            {
                // Lendo arquivos das transações TEF Não confirmadas e Confirmadas e os Espelhos de Impressão TEF

                var arquivosRespTEFConfirmadas = ArquivosRespTransacaoTEFConfirmadas();

                // Se for um cancelamento então não é necessário imprimir os comprovantes
                // das transações já aprovadas.
                if (cancelamento == true)
                    arquivosRespTEFConfirmadas.Clear();

                var arquivosRespTEF = ArquivosRespTransacaoTEF();


                foreach (var item in arquivosRespTEFConfirmadas)
                {
                    StructRespotasTEF tefResp;
                    tefResp.arquivo = item.ToString();
                    tefResp.transacaoConfirmada = true;
                    respostasTEF.Add(tefResp);
                }


                foreach (var item in arquivosRespTEF)
                {
                    StructRespotasTEF tefResp;
                    tefResp.arquivo = item.ToString();
                    tefResp.transacaoConfirmada = false;
                    respostasTEF.Add(tefResp);
                }
                
            }

            return respostasTEF;
        }

        public static List<string> ArquivosRespTransacaoTEF()
        {
            List<string> arquivosImpresaoTEF = new List<string>();

            if (GlbVariaveis.glb_TEFAcbr == false)
            {

                var arquivosEspelhoTEF = Directory.GetFiles(TEF.@pathTransacoesTEF);
                arquivosImpresaoTEF = (from n in arquivosEspelhoTEF
                                        where n.ToLower().Contains("intpos")
                                        select n).ToList();
                // Verificando arquivo do RESP
                if (arquivosEspelhoTEF.Count() == 0)
                {
                    arquivosEspelhoTEF = Directory.GetFiles(TEF.PathResp);
                    arquivosImpresaoTEF = (from n in arquivosEspelhoTEF
                                           where n.ToLower().Contains("intpos")
                                           select n).ToList();
                }

                
            }

            return arquivosImpresaoTEF;
        }

        private static List<string> ArquivosRespTransacaoTEFConfirmadas()
        {
            var arquivosEspelhoTEF = Directory.GetFiles(TEF.@pathTransacoesTEFConfirmadas);
            var arquivosImpresaoTEF = (from n in arquivosEspelhoTEF
                                       where n.ToLower().Contains("intpos")
                                       select n).ToList();
            return arquivosImpresaoTEF;
        }

        public static bool CancelarCupomECF()
        {
            // somente teste por ivan
            //try
            //{
            //    ServiceReference1.WSIQPassClient card = new ServiceReference1.WSIQPassClient();
            //    var resultado = card.EstornarDebitarBitCoin(GlbVariaveis.chavePrivada, "673fa1a8-266a-46a7-ab1c-35e1ddb6556d");
            //    MessageBox.Show(resultado.ToString());

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            

            //Aqui deve ser tratado o cancelamento do Cupom NFCe
            if (ConfiguracoesECF.idNFC > 0)
                return true;
            if (ConfiguracoesECF.pdv == false)
                return true;

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:

                        int iRetorno = BemaFI32.Bematech_FI_CancelaCupom();
                        if (iRetorno != 1)
                            throw new Exception("Não é possível cancelar o Cupom");
                        GravarGtECF();
                        break;

                    case 2:
                        try
                        {
                            DARUMA_FW.iRetorno = DARUMA_FW.iCFCancelar_ECF_Daruma();
                            if (DARUMA_FW.iRetorno != 1)
                                throw new Exception("Não foi possível excluir cupom no ecf:" + DARUMA_FW.iRetorno.ToString());
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Exceção, Não foi possível excluir cupom: " + ex.InnerException.ToString());
                        }
                        break;

                    case 3:
                        Elgin32.Int_Retorno = Elgin32.Elgin_CancelaCupom();
                        if (Elgin32.Int_Retorno != 1)
                            throw new Exception("Não é possível cancelar o Cupom");
                        GravarGtECF();
                        break;
                    case 4:
                        Sweda32.iRetorno = Sweda32.ECF_CancelaCupom();
                        if (Sweda32.iRetorno != 1)
                            throw new Exception("Não é possível cancelar o Cupom");
                        GravarGtECF();
                        break;
                }
            }
            else
            {
                try
                {
                    if (ConfiguracoesECF.pdv == true)
                    {
                        ecfAcbr.CancelaCupom();
                    }
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }

                GravarGtECF();
            }
            return true;
        }

        public bool AberturaDoDiaECF(decimal valor)
        {
            if (File.Exists("log_teste.txt"))
            {
                return true;
            }

            if (ConfiguracoesECF.idNFC > 0)
            {
                /*conteudoImpressao =  "Suprimento.:" + valor.ToString("N2")+"\n";
                conteudoImpressao += "TOTAL.:     " + valor.ToString("N2")+"\n";
                FuncoesImpressao.conteudo = conteudoImpressao;
                FuncoesImpressao.impressaoDialog();
                conteudoImpressao = "";*/

                    FuncoesNFC.conteudoImpressao.Append("Suprimento.:" + valor.ToString("N2")+"\n");
                    FuncoesNFC.conteudoImpressao.Append("TOTAL.:     " + valor.ToString("N2")+"\n");
                    FuncoesNFC Impressao = new FuncoesNFC();
                    Impressao.RelatorioGerencial();
                    FuncoesNFC.conteudoImpressao.Clear();
              
                return true;

            }

            VerificaConexaoECF();

            if (GlbVariaveis.glb_Acbr == false)
            {
                #region
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                       
                            int iRetorno = BemaFI32.Bematech_FI_AberturaDoDia(valor.ToString(), "Dinheiro");
                            if (iRetorno != 1)
                                return false;
                            //PreVenda.Cancelar();
                            return true;
                       

                    case 2:
                        
                            DARUMA_FW.iRetorno = DARUMA_FW.iSuprimento_ECF_Daruma(valor.ToString(), "Dinheiro");
                            if (DARUMA_FW.iRetorno != 1)
                                return false;

                            if (FuncoesECF.VerificaReducaZDia())
                                return false;

                            ///
                            /// PreVenda.Cancelar();
                            return true;
                        

                    case 3:
                        
                            Elgin32.Int_Retorno = Elgin32.Elgin_AberturaDoDia(valor.ToString(), "Dinheiro");
                            if (Elgin32.Int_Retorno != 1)
                                return false;
                            //PreVenda.Cancelar();
                            return true;
                        
                    case 4:
                        
                            Sweda32.iRetorno = Sweda32.ECF_AberturaDoDia(valor.ToString(), "Dinheiro");
                            if (Sweda32.iRetorno != 1)
                                return false;
                            else
                                return true;

                }
                #endregion
            }
            else
            {
                #region
                try
                {
                    
                    if(valor > 0)
                    ecfAcbr.Suprimento(valor, "", "SUPRIMENTO", "Dinheiro", 0);

                    ecfAcbr.LeituraX();
                    return true;
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    return false;
                }
                #endregion

            }
            return true;
        }


        public bool SuprimentoECF(decimal valor)
        {
            FuncoesNFC.conteudoImpressao.Append("Suprimento.:" + valor.ToString("N2") + "\n");
            FuncoesNFC.conteudoImpressao.Append("TOTAL.:     " + valor.ToString("N2") + "\n");
            FuncoesNFC Impressao = new FuncoesNFC();
            Impressao.RelatorioGerencial();
            FuncoesNFC.conteudoImpressao.Clear();

            if (ConfiguracoesECF.idNFC > 0)
                return true;

            if (File.Exists("log_teste.txt"))
                return true;

            VerificaConexaoECF();

            if (GlbVariaveis.glb_Acbr == false)
            {
                #region
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:

                        int iRetorno = BemaFI32.Bematech_FI_Suprimento(string.Format("{0:N2}", valor), "Dinheiro");
                        if (iRetorno != 1)
                            return false;
                        return true;

                    //try
                    //{
                    //    printer.RelatoriosFiscais.ImprimirLeituraX(60); // timeout 1 min
                    //    printer.OperacaoNaoFiscal.ExecutarSuprimento(valor);
                    //    return true;
                    //}
                    //catch (BematechException erro)
                    //{
                    //    throw new Exception (erro.Message);                        
                    //}   
                    case 2:
                        DARUMA_FW.iRetorno = DARUMA_FW.iSuprimento_ECF_Daruma(valor.ToString(), "Dinheiro");
                        if (DARUMA_FW.iRetorno != 1)
                            return false;
                        return true;
                    case 3:
                        Elgin32.Int_Retorno = Elgin32.Elgin_Suprimento(string.Format("{0:N2}", valor), "Dinheiro");
                        if (Elgin32.Int_Retorno != 1)
                            return false;
                        return true;
                    case 4:
                        Sweda32.iRetorno = Sweda32.ECF_Suprimento(string.Format("{0:N2}", valor), "Dinheiro");
                        if (Sweda32.iRetorno != 1)
                            return false;
                        else
                            return true;

                }
                #endregion
            }
            else
            {
                try
                {
                    ecfAcbr.Suprimento(valor, "", "SUPRIMENTO", "Dinheiro");
                    return true;
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    return false;
                }
            }
            return false;
        }
        #region Menu Fiscal PAF-ECF
        public bool LeituraX()
        {
            
                if (GlbVariaveis.glb_Acbr == false)
                {
                    #region
                    switch (ConfiguracoesECF.idECF)
                    {
                        case 1:

                            int iRetorno = BemaFI32.Bematech_FI_LeituraX();
                            if (iRetorno != 1)
                                throw new Exception("Não foi possível emitir leitura X " + DARUMA_FW.iRetorno.ToString());
                            break;

                        //try
                        //{
                        //    printer.RelatoriosFiscais.ImprimirLeituraX();
                        //}
                        //catch (BematechException erro)
                        //{
                        //    throw new Exception(erro.Message);
                        //}
                        //break;


                        case 2:

                            try
                            {

                                DARUMA_FW.iRetorno = DARUMA_FW.iLeituraX_ECF_Daruma();
                                if (DARUMA_FW.iRetorno != 1)
                                    throw new Exception();

                            }
                            catch
                            {
                                throw new Exception("Não foi possível emitir leitura X " + DARUMA_FW.iRetorno.ToString());
                            }
                            break;

                        case 3:

                            Elgin32.Int_Retorno = Elgin32.Elgin_LeituraX();
                            if (Elgin32.Int_Retorno != 1)
                                throw new Exception("Não foi possível emitir leitura X " + DARUMA_FW.iRetorno.ToString());
                            break;


                        case 4:


                            Sweda32.iRetorno = Sweda32.ECF_LeituraX();
                            if (Sweda32.iRetorno != 1)
                                throw new Exception("Não foi possível emitir leitura X " + DARUMA_FW.iRetorno.ToString());
                            break;
                    }
                    #endregion
                }
                else
                {
                    try
                    {
                        ecfAcbr.LeituraX();
                        return true;
                    }
                    catch (Exception erro)
                    {
                        throw new Exception("Não foi possível emitir leitura X " + erro.Message);
                    }
                } 

            return true;
        }

        public void LeituraXNFC(DateTime dataIncio, DateTime dataFinal, bool autorizado = false)
        {
            Application.DoEvents();
            FuncoesECF.RelatorioGerencial("abrir", "");
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            var Relatorio = FuncoesNFC.valoresCaixaNFCe(dataIncio, dataFinal, autorizado).ToList();
            var Encargos = FuncoesNFC.encargosCaixaNFCe(dataIncio, dataFinal);
            var RelatorioCancelado = FuncoesNFC.valoresCaixaCanceladoNFCe(dataIncio, dataFinal, autorizado).ToList();
            decimal totalImposto = 0;
            decimal totalBase = 0;
            decimal totalVenda = 0;

            if(autorizado == false)
                FuncoesECF.RelatorioGerencial("imprimir", "                                      ICMS" + Environment.NewLine);
            else
                FuncoesECF.RelatorioGerencial("imprimir", "                           ICMS AUTORIZADO" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Data Movimentação " + dataIncio.ToString("dd/MM/yyyy")+" a " + dataFinal.ToString("dd/MM/yyyy")+ Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "Tributação | Aliquota | Base Calculo(R$)| Imposto(R$) " + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            foreach (var item in Relatorio)
            {
                totalImposto = totalImposto + item.imposto;
                totalVenda = totalVenda + item.valorProdutos;
                if (item.imposto > 0)
                    totalBase = totalBase + item.baseICMS;

                FuncoesECF.RelatorioGerencial("imprimir", item.tributacao.PadRight(9, '*') + "|" + item.icms.ToString("N2").PadLeft(8, '*') + "|" + item.baseICMS.ToString("N2").PadLeft(14, '*') + '|' + item.imposto.ToString("N2").PadLeft(10, '*') + Environment.NewLine);

            }


            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total liquido       .: (R$)" + totalVenda.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total Base(ICMS).: (R$)" + totalBase.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total Imposto     .: (R$)" + totalImposto.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Acréscimo          .: (R$)" + Encargos.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "-----------------------------------------------------------------" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);


            totalImposto = 0;
            totalBase = 0;
            totalVenda = 0;
            FuncoesECF.RelatorioGerencial("imprimir", "                          ICMS CANCELADO" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "Tributação | Aliquota | Base Calculo(R$)| Imposto(R$) " + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            foreach (var item in RelatorioCancelado)
            {
                totalImposto = totalImposto + item.imposto;
                totalVenda = totalVenda + item.valorProdutos;
                if (item.imposto > 0)
                    totalBase = totalBase + item.baseICMS;

                FuncoesECF.RelatorioGerencial("imprimir", item.tributacao.PadRight(9, '*') + "|" + item.icms.ToString("N2").PadLeft(8, '*') + "|" + item.baseICMS.ToString("N2").PadLeft(14, '*') + '|' + item.imposto.ToString("N2").PadLeft(10, '*') + Environment.NewLine);

            }
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total liquido       .: (R$)" + totalVenda.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total Base(ICMS).: (R$)" + totalBase.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total Imposto     .: (R$)" + totalImposto.ToString("N2") + Environment.NewLine);





            FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
            FuncoesECF.RelatorioGerencial("fechar", "");
        }

        public void LeituraXNFCDetalhada(DateTime dataIncio, DateTime dataFinal, bool autorizado = false)
        {
            Application.DoEvents();
            List<DateTime> datas = new List<DateTime>();

            decimal totalImpostoTot = 0;
            decimal totalBaseTot = 0;
            decimal totalVendaTot = 0;
            decimal EncargosTot = 0;

            decimal totalImpostoCANTot = 0;
            decimal totalBaseCANTot = 0;
            decimal totalVendaCANTot = 0;
            decimal EncargosCANTot = 0;


            FuncoesECF.RelatorioGerencial("abrir", "");
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            datas = FuncoesNFC.datasNFCe(dataIncio, dataFinal, autorizado).ToList();

            foreach (DateTime itemData in datas)
            {
                var Relatorio = FuncoesNFC.valoresCaixaNFCe(itemData, itemData, autorizado).ToList();
                var Encargos = FuncoesNFC.encargosCaixaNFCe(itemData, itemData);
                var RelatorioCancelado = FuncoesNFC.valoresCaixaCanceladoNFCe(itemData, itemData, autorizado).ToList();
                decimal totalImposto = 0;
                decimal totalBase = 0;
                decimal totalVenda = 0;

                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "====================================" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

                if (autorizado == false)
                    FuncoesECF.RelatorioGerencial("imprimir", "                                      ICMS" + Environment.NewLine);
                else
                    FuncoesECF.RelatorioGerencial("imprimir", "                           ICMS AUTORIZADO" + Environment.NewLine);

                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Data Movimentação "+itemData.ToString("dd/MM/yyyy") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

                FuncoesECF.RelatorioGerencial("imprimir", "Tributação | Aliquota | Base Calculo(R$)| Imposto(R$) " + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

                foreach (var item in Relatorio)
                {
                    totalImposto = totalImposto + item.imposto;
                    totalVenda = totalVenda + item.valorProdutos;
                    if (item.imposto > 0)
                        totalBase = totalBase + item.baseICMS;

                    FuncoesECF.RelatorioGerencial("imprimir", item.tributacao.PadRight(9, '*') + "|" + item.icms.ToString("N2").PadLeft(8, '*') + "|" + item.baseICMS.ToString("N2").PadLeft(14, '*') + '|' + item.imposto.ToString("N2").PadLeft(10, '*') + Environment.NewLine);

                }

                totalImpostoTot = totalImpostoTot + totalImposto;
                totalBaseTot = totalBaseTot + totalBase;
                totalVendaTot = totalVendaTot + totalVenda;
                EncargosTot = EncargosTot + Encargos;


                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Total liquido       .: (R$)" + totalVenda.ToString("N2") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Total Base(ICMS).: (R$)" + totalBase.ToString("N2") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Total Imposto     .: (R$)" + totalImposto.ToString("N2") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Acréscimo          .: (R$)" + Encargos.ToString("N2") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "-----------------------------------------------------------------" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);


                totalImposto = 0;
                totalBase = 0;
                totalVenda = 0;
                FuncoesECF.RelatorioGerencial("imprimir", "                          ICMS CANCELADO" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

                FuncoesECF.RelatorioGerencial("imprimir", "Tributação | Aliquota | Base Calculo(R$)| Imposto(R$) " + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

                foreach (var item in RelatorioCancelado)
                {
                    totalImposto = totalImposto + item.imposto;
                    totalVenda = totalVenda + item.valorProdutos;
                    if (item.imposto > 0)
                        totalBase = totalBase + item.baseICMS;

                    FuncoesECF.RelatorioGerencial("imprimir", item.tributacao.PadRight(9, '*') + "|" + item.icms.ToString("N2").PadLeft(8, '*') + "|" + item.baseICMS.ToString("N2").PadLeft(14, '*') + '|' + item.imposto.ToString("N2").PadLeft(10, '*') + Environment.NewLine);

                }

                totalImpostoCANTot = totalImpostoCANTot + totalImposto;
                totalBaseCANTot = totalBaseCANTot + totalBase;
                totalVendaCANTot = totalVendaCANTot + totalVenda;
                EncargosCANTot = EncargosCANTot + Encargos;
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Total liquido       .: (R$)" + totalVenda.ToString("N2") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Total Base(ICMS).: (R$)" + totalBase.ToString("N2") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Total Imposto     .: (R$)" + totalImposto.ToString("N2") + Environment.NewLine);
            }


            //Totalizadores 
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "                                        FIM" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "====================================" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "                                        RESUMO" + Environment.NewLine);

            if (autorizado == false)
                FuncoesECF.RelatorioGerencial("imprimir", "                                      ICMS" + Environment.NewLine);
            else
                FuncoesECF.RelatorioGerencial("imprimir", "                           ICMS AUTORIZADO" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Data Movimentação " + dataIncio.ToString("dd/MM/yyyy") + " a " + dataFinal.ToString("dd/MM/yyyy") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);


            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total liquido       .: (R$)" + totalVendaTot.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total Base(ICMS).: (R$)" + totalBaseTot.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total Imposto     .: (R$)" + totalImpostoTot.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Acréscimo          .: (R$)" + EncargosTot.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "-----------------------------------------------------------------" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "                          ICMS CANCELADO" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total liquido       .: (R$)" + totalVendaCANTot.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total Base(ICMS).: (R$)" + totalBaseCANTot.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total Imposto     .: (R$)" + totalImpostoCANTot.ToString("N2") + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
            FuncoesECF.RelatorioGerencial("fechar", "");


            FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
            FuncoesECF.RelatorioGerencial("fechar", "");
        }

        public void LeituraXNFCSimplificada(DateTime dataIncio, DateTime dataFinal, bool autorizado = false)
        {
            Application.DoEvents();
            List<DateTime> datas = new List<DateTime>();

            decimal totalImpostoTot = 0;
            decimal totalBaseTot = 0;
            decimal totalVendaTot = 0;
            decimal EncargosTot = 0;

            decimal totalImpostoCANTot = 0;
            decimal totalBaseCANTot = 0;
            decimal totalVendaCANTot = 0;
            decimal EncargosCANTot = 0;


            FuncoesECF.RelatorioGerencial("abrir", "");
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            datas = FuncoesNFC.datasNFCe(dataIncio, dataFinal, autorizado).ToList();

            foreach (DateTime itemData in datas)
            {
                var Relatorio = FuncoesNFC.valoresCaixaNFCe(itemData, itemData, autorizado).ToList();
                var Encargos = FuncoesNFC.encargosCaixaNFCe(itemData, itemData);
                var RelatorioCancelado = FuncoesNFC.valoresCaixaCanceladoNFCe(itemData, itemData, autorizado).ToList();
                decimal totalImposto = 0;
                decimal totalBase = 0;
                decimal totalVenda = 0;

                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "====================================" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

                if (autorizado == false)
                    FuncoesECF.RelatorioGerencial("imprimir", "                                      ICMS" + Environment.NewLine);
                else
                    FuncoesECF.RelatorioGerencial("imprimir", "                           ICMS AUTORIZADO" + Environment.NewLine);

                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Data Movimentação " + itemData.ToString("dd/MM/yyyy") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

                foreach (var item in Relatorio)
                {
                    totalImposto = totalImposto + item.imposto;
                    totalVenda = totalVenda + item.valorProdutos;
                    if (item.imposto > 0)
                        totalBase = totalBase + item.baseICMS;
                }

                totalImpostoTot = totalImpostoTot + totalImposto;
                totalBaseTot = totalBaseTot + totalBase;
                totalVendaTot = totalVendaTot + totalVenda;
                EncargosTot = EncargosTot + Encargos;

                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Total liquido       .: (R$)" + totalVenda.ToString("N2") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Total Base(ICMS).: (R$)" + totalBase.ToString("N2") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Total Imposto     .: (R$)" + totalImposto.ToString("N2") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Acréscimo          .: (R$)" + Encargos.ToString("N2") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "-----------------------------------------------------------------" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);


                totalImposto = 0;
                totalBase = 0;
                totalVenda = 0;
                FuncoesECF.RelatorioGerencial("imprimir", "                          ICMS CANCELADO" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

                foreach (var item in RelatorioCancelado)
                {
                    totalImposto = totalImposto + item.imposto;
                    totalVenda = totalVenda + item.valorProdutos;
                    if (item.imposto > 0)
                        totalBase = totalBase + item.baseICMS;

                }

                totalImpostoCANTot = totalImpostoCANTot + totalImposto;
                totalBaseCANTot = totalBaseCANTot + totalBase;
                totalVendaCANTot = totalVendaCANTot + totalVenda;
                EncargosCANTot = EncargosCANTot + Encargos;

                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Total liquido       .: (R$)" + totalVenda.ToString("N2") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Total Base(ICMS).: (R$)" + totalBase.ToString("N2") + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "Total Imposto     .: (R$)" + totalImposto.ToString("N2") + Environment.NewLine);
            }


            //Totalizadores 
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "                                        FIM" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "====================================" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "                                        RESUMO" + Environment.NewLine);

            if (autorizado == false)
                FuncoesECF.RelatorioGerencial("imprimir", "                                      ICMS" + Environment.NewLine);
            else
                FuncoesECF.RelatorioGerencial("imprimir", "                           ICMS AUTORIZADO" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Data Movimentação " + dataIncio.ToString("dd/MM/yyyy")+ " a "+dataFinal.ToString("dd/MM/yyyy") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);


            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total liquido       .: (R$)" + totalVendaTot.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total Base(ICMS).: (R$)" + totalBaseTot.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total Imposto     .: (R$)" + totalImpostoTot.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Acréscimo          .: (R$)" + EncargosTot.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "-----------------------------------------------------------------" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "                          ICMS CANCELADO" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total liquido       .: (R$)" + totalVendaCANTot.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total Base(ICMS).: (R$)" + totalBaseCANTot.ToString("N2") + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "Total Imposto     .: (R$)" + totalImpostoCANTot.ToString("N2") + Environment.NewLine);

            FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
            FuncoesECF.RelatorioGerencial("fechar", "");
        }

        public bool LMFC(string tipoSaida,DateTime dataInicial,DateTime dataFinal,int CRZInicial,int CRZFinal,string tipoRelatorio,bool gravarEAD)
        {
            //if (File.Exists(@"c:\retorno.txt"))
            //    System.IO.File.Delete(@"c:\retorno.txt");
            // tipoRelatorio c= Leitura Completa
            // tipoRelatorio S = Leitura Simplificada

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        if (tipoSaida == "I" && CRZInicial == 0)
                            BemaFI32.Bematech_FI_LeituraMemoriaFiscalDataMFD(string.Format("{0:dd/MM/yyyy}", dataInicial.Date), string.Format("{0:dd/MM/yyyy}", dataFinal.Date), tipoRelatorio);
                        //BemaFI32.Bematech_FI_LeituraMemoriaFiscalDataMFD(string.Format("{0:dd/MM/yyyy}", dataInicial.Date), string.Format("{0:dd/MM/yyyy}", dataFinal.Date), tipoRelatorio);
                        if (tipoSaida == "A" && CRZInicial == 0)
                            // printer.RelatoriosFiscais.ReceberLeituraMemoriaFiscal(dataInicial, dataFinal, false);
                            BemaFI32.Bematech_FI_LeituraMemoriaFiscalSerialDataMFD(string.Format("{0:dd/MM/yyyy}", dataInicial.Date), string.Format("{0:dd/MM/yyyy}", dataFinal.Date), tipoRelatorio);
                        // CRZ
                        if (tipoSaida == "I" && CRZInicial > 0)
                            BemaFI32.Bematech_FI_LeituraMemoriaFiscalReducaoMFD(CRZInicial.ToString(), CRZFinal.ToString(), tipoRelatorio);
                        if (tipoSaida == "A" && CRZInicial > 0)
                            BemaFI32.Bematech_FI_LeituraMemoriaFiscalSerialReducaoMFD(CRZInicial.ToString(), CRZFinal.ToString(), tipoRelatorio);
                        break;

                    case 2:
                        if (tipoRelatorio.ToLower() == "c")
                            DARUMA_FW.regAlterarValor_Daruma(@"ECF\LMFCompleta", "1");
                        else
                            DARUMA_FW.regAlterarValor_Daruma(@"ECF\LMFCompleta", "0");

                        if (tipoSaida == "I" && CRZInicial == 0)
                        {
                            DARUMA_FW.iRetorno = DARUMA_FW.iMFLer_ECF_Daruma(string.Format("{0:ddMMyyyy}", dataInicial.Date), string.Format("{0:dd/MM/yyyy}", dataFinal.Date));
                        }

                        if (tipoSaida == "I" && CRZInicial > 0)
                        {
                            DARUMA_FW.iRetorno = DARUMA_FW.iMFLer_ECF_Daruma(CRZInicial.ToString(), CRZFinal.ToString());
                        }


                        if (tipoSaida == "A" && CRZInicial == 0)
                        {
                            DARUMA_FW.iRetorno = DARUMA_FW.iMFLerSerial_ECF_Daruma(string.Format("{0:ddMMyyyy}", dataInicial.Date), string.Format("{0:dd/MM/yyyy}", dataFinal.Date));
                        }


                        if (tipoSaida == "A" && CRZInicial > 0)
                        {
                            // ATenção no dia da homologação se estava colocando um 
                            //CRZ grande que nao existia e a dll retornava um erro de execacao
                            DARUMA_FW.iRetorno = DARUMA_FW.iMFLerSerial_ECF_Daruma(CRZInicial.ToString(), CRZFinal.ToString());
                        }
                        break;

                    case 3:
                        if (tipoSaida == "I" && CRZInicial == 0)
                            Elgin32.Elgin_LeituraMemoriaFiscalData(string.Format("{0:dd/MM/yyyy}", dataInicial.Date), string.Format("{0:dd/MM/yyyy}", dataFinal.Date), tipoRelatorio);
                        //BemaFI32.Bematech_FI_LeituraMemoriaFiscalDataMFD(string.Format("{0:dd/MM/yyyy}", dataInicial.Date), string.Format("{0:dd/MM/yyyy}", dataFinal.Date), tipoRelatorio);
                        if (tipoSaida == "A" && CRZInicial == 0)
                            // printer.RelatoriosFiscais.ReceberLeituraMemoriaFiscal(dataInicial, dataFinal, false);
                            Elgin32.Elgin_LeituraMemoriaFiscalSerialData(string.Format("{0:dd/MM/yyyy}", dataInicial.Date), string.Format("{0:dd/MM/yyyy}", dataFinal.Date), tipoRelatorio);
                        // CRZ
                        if (tipoSaida == "I" && CRZInicial > 0)
                            Elgin32.Elgin_LeituraMemoriaFiscalReducao(CRZInicial.ToString(), CRZFinal.ToString(), tipoRelatorio);
                        if (tipoSaida == "A" && CRZInicial > 0)
                            Elgin32.Elgin_LeituraMemoriaFiscalSerialReducao(CRZInicial.ToString(), CRZFinal.ToString(), tipoRelatorio);
                        break;

                    case 4:
                        if (tipoSaida == "I" && CRZInicial == 0)
                            Sweda32.ECF_LeituraMemoriaFiscalData(string.Format("{0:dd/MM/yyyy}", dataInicial.Date), string.Format("{0:dd/MM/yyyy}", dataFinal.Date));
                        //BemaFI32.Bematech_FI_LeituraMemoriaFiscalDataMFD(string.Format("{0:dd/MM/yyyy}", dataInicial.Date), string.Format("{0:dd/MM/yyyy}", dataFinal.Date), tipoRelatorio);
                        if (tipoSaida == "A" && CRZInicial == 0)
                            // printer.RelatoriosFiscais.ReceberLeituraMemoriaFiscal(dataInicial, dataFinal, false);
                            Sweda32.ECF_LeituraMemoriaFiscalSerialData(string.Format("{0:dd/MM/yyyy}", dataInicial.Date), string.Format("{0:dd/MM/yyyy}", dataFinal.Date));
                        // CRZ
                        if (tipoSaida == "I" && CRZInicial > 0)
                            Sweda32.ECF_LeituraMemoriaFiscalReducao(CRZInicial.ToString(), CRZFinal.ToString());
                        if (tipoSaida == "A" && CRZInicial > 0)
                            Sweda32.ECF_LeituraMemoriaFiscalSerialDataMFD(CRZInicial.ToString(), CRZFinal.ToString(), tipoRelatorio);
                        break;

                };
            }
            else
            {
                try
                {
                    if (tipoRelatorio == "c")
                        ecfAcbr.LeituraMemoriaFiscal(dataInicial, dataFinal, false);
                    else
                        ecfAcbr.LeituraMemoriaFiscal(dataInicial, dataFinal, true);
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }
            }
            

            if (tipoSaida == "A")
            {

                if (File.Exists(ConfiguracoesECF.pathRetornoECF+"retorno.txt"))
                {
                    if (File.Exists(@ConfigurationManager.AppSettings["dirEspelhoECF"] + "\\LMF.txt"))
                    {
                        System.IO.File.Delete(@ConfigurationManager.AppSettings["dirEspelhoECF"] + "\\LMF.txt");
                    }

                    System.IO.File.Copy(ConfiguracoesECF.pathRetornoECF + "retorno.txt", @ConfigurationManager.AppSettings["dirEspelhoECF"] + "\\LMF.txt");
                }


                if (gravarEAD)
                {
                    string ead = Funcoes.SetLength(256);
                    try
                    {
                        AssinarArquivo(@ConfigurationManager.AppSettings["dirEspelhoECF"] + "\\LMF.txt", false);
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }

            return true;
        }

 

        public static string AssinarArquivo(string arquivo,bool AdicionarQuebraLinha,bool usarOpenSSL=true,bool binario=false)
        {          

            Thread.Sleep(500);
            string quebra = "";
            if (AdicionarQuebraLinha==true)
                quebra = "\r\n";
            
            string ead = new string(' ', 256);
            string chavePublica = "996B15128A8700DF24B2F49E0197B62A0ED8674210206ECEF49ED3545B6FCC1F93F2E060C5444754F2470D86F95F9C9985039C0A84E1E0B92E710166953B28AEE5E79E6F1D4AF5889F7FBFDD8604490D5BA86812A95C076FE2FF7ACFAF77C291C98A0F12EA03C80B51C024C8B7BB395EE6D51A1F29F721A70FDD81E45E79CB1B";
            string chavePrivada = GlbVariaveis.chavePrivada ;

            string chavePrivadaOpenSSL = "-----BEGIN RSA PRIVATE KEY-----"+Environment.NewLine+
                    "MIICXQIBAAKBgQDDM+78CLXflMqKq7Uw75bA1Kh+wv90Bje2tfbdbW0dstBmzbwM"+Environment.NewLine+
                    "R9d4kUSQR1rXjuR45rRrhplT32J5MiuHOLVr2x/uU0iYZyBGb+Vul451KDHACGha"+Environment.NewLine+
                    "pcBux2P16AD3EQUn5fCPwX/rEer7+P8lXrTGmrUx+W4PU4I9GsPzH1hDqQIDAQAB"+Environment.NewLine+
                    "AoGBAKef8ztu529lwEAwj1nEhHp2o3KnVOJ3bGR0AdfH3gnAwgFl7nPFRZz9chlL"+Environment.NewLine+
                    "jRDKXhwMUSZ84TILA+77TDHEWtUPQjn/hyrm4NO4s3esisWcgqH8Z4F5MvEKkrrD"+Environment.NewLine+
                    "vQhzQCulptkkz5myN9YOquVvVEMzyFaBRdnCIT+5hLMt46JhAkEA+xYxLL2Z2bX4"+Environment.NewLine+
                    "QyBnmF9OIYKaDVpnIYvwrP6yEW8lCUYVrMABbqywu5ogKGtN89lujYESOuvVOSOg"+Environment.NewLine+
                    "vjweiXjTvQJBAMcFyznOeqUNY01Hhd+KVoKe9hhE/iqnkXcXz2wEsjbeni7hNhrQ"+Environment.NewLine+
                    "+ns1iXp0U5vnYwqYO9SdTl0YuRRR/LHKOF0CQCifV4dgiYKioS7jED+js7VSNvBQ"+Environment.NewLine+
                    "Hv1bca3bax3M+JX+g8U2L0UjpVhEwE0pxyvbkrpMFpH308Bx1jDQ8zPUm5UCQQCE"+Environment.NewLine+
                    "jyxcNVXlB3TDrUbSrG6Qk9YwNgvgVzoBS7+hH8Of3kkXynNiCx064V7PTBnANq72"+Environment.NewLine+
                    "CI2ZQKlIQsZLbYxU0u8VAkBRNufo0VDSDA/s+KcRr4bXz3E7yVbM0rb8k+X7gj7R"+Environment.NewLine+
                    "XFJkG9fp6oyeAvxvqhGbD0qvEV6QrfaK7J1HAKSts9U7"+Environment.NewLine+
                    "-----END RSA PRIVATE KEY-----";
            
            // Chave que deverá ser usada no eCCF para validar os arquivos.
            string chavePublicaOpenSSL = "C333EEFC08B5DF94CA8AABB530EF96C0D4A87EC2FF740637B6B5F6DD6D6D1DB2D066CDBC0C47D778914490475AD78EE478E6B46B869953DF6279322B8738B56BDB1FEE5348986720466FE56E978E752831C008685AA5C06EC763F5E800F7110527E5F08FC17FEB11EAFBF8FF255EB4C69AB531F96E0F53823D1AC3F31F5843A9";
            

            if (usarOpenSSL)
            {
                FrmMsgOperador msg = new FrmMsgOperador("", "Gerando EAD ");
                msg.Show();
                Application.DoEvents();
                // openssl genrsa -out chavePrivada.txt 1024 -  GErar Chave privada
                // openssl rsa -modulus -in ChavePrivada.txt -inform PEM -text -pubout -out ChavePublica.txt - GErar chave publica
                // openssl dgst -md5 -sign ChavePrivada.txt -out ead.txt -hex arquivo.txt - Assinar Arquivo

                try
                {
                    using (FileStream fs = File.Create(@Application.StartupPath + @"\chaveprivada.pem"))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                        {
                            sw.Write(chavePrivadaOpenSSL.Trim());
                        }
                    };

                    Thread.Sleep(1000);                                        
                    System.Diagnostics.Process.Start(@"openssl", "dgst -md5 -sign chaveprivada.pem -out ead.txt -hex " + arquivo);                                                                
                    Thread.Sleep(4000);                   
                    
                    using (StreamReader ler = new StreamReader(@Application.StartupPath + @"\ead.txt"))
                    {

                        string linha = null;
                        //while ((linha = ler.ReadLine()) != null)
                        //{                        
                        //    ead = (linha.ToString().Substring(linha.Count()-256,256) );
                        //}
                        linha = ler.ReadLine();
                        ead = (linha.ToString().Substring(linha.Count() - 256, 256));

                    }

                    if (binario)
                        return quebra + "EAD" + ead.Trim();

                    
                    using (StreamWriter sw = File.AppendText(arquivo.Trim()))
                    {
                        sw.Write(quebra + "EAD" + ead.Trim());
                    }

                    if (File.Exists(@Application.StartupPath + @"\ead.txt"))
                    {
                        File.Delete((@Application.StartupPath + @"\ead.txt"));
                    }

                    if (File.Exists(@Application.StartupPath + @"\chaveprivada.pem"))
                    {
                        File.Delete(@Application.StartupPath + @"\chaveprivada.pem");
                    }
                    return ead;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exceção ao gerar EAD: "+arquivo+" " + ex.Message);
                }
                finally
                {
                    msg.Dispose();
                }
            }

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        // int ret = BemaFI32.generateEAD(arquivo, ConfigurationManager.AppSettings["chavePublica"],Funcoes.DesCriptografarComSenha(Convert.FromBase64String(ConfigurationManager.AppSettings["chavePrivada"]), senhaIQ), ref ead, 1);

                        //int ret = BemaFI32.generateEAD(arquivo, ConfigurationManager.AppSettings["chavePublica"], ConfigurationManager.AppSettings["chavePrivada"] , ref ead, 1);

                        int ret = BemaFI32.generateEAD(arquivo, chavePublica, chavePrivada, ref ead, 1);
                        if (ret == 0)
                            throw new Exception("Não foi possível assinar o arquivo: " + ret.ToString());
                        return ead;

                }
            }
            return "";
        }



        public static bool EspelhoMFD(string arquivo,string arquivoDestino,string tipo,DateTime dataInicial,DateTime dataFinal, int intervaloInicial, int intervaloFinal)
        {

            if (ConfiguracoesECF.idNFC > 0)
                return true;

            if (GlbVariaveis.glb_Acbr == false)
            {
                string usuario = "1";
                int ret = BemaFI32.Bematech_FI_NumeroSubstituicoesProprietario(ref usuario);


                if (File.Exists(@arquivo))
                {
                    System.IO.File.Delete(@arquivo);
                }
                if (File.Exists(@arquivoDestino))
                {
                    System.IO.File.Delete(@arquivoDestino);
                }
                // Tipo de Refere a saida de dados 0-Total, 1-Por data,2-Por COO
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        switch (tipo)
                        {
                            case "0":
                                BemaFI32.Bematech_FI_DownloadMFD(arquivo, "0", "", "", "");
                                System.Threading.Thread.Sleep(500);
                                BemaFI32.Bematech_FI_FormatoDadosMFD(arquivo, arquivoDestino, "0", "0", "", "", "");
                                break;
                            case "1":
                                BemaFI32.Bematech_FI_DownloadMFD(arquivo, "1", string.Format("{0:dd/MM/yyyy}", dataInicial), string.Format("{0:dd/MM/yyyy}", dataFinal), "");
                                BemaFI32.Bematech_FI_FormatoDadosMFD(arquivo, arquivoDestino, "0", "1", string.Format("{0:dd/MM/yyyy}", dataInicial), string.Format("{0:dd/MM/yyyy}", dataFinal), "");
                                break;
                            case "2":
                                BemaFI32.Bematech_FI_DownloadMFD(arquivo, "2", Convert.ToString(intervaloInicial), Convert.ToString(intervaloFinal), usuario);
                                BemaFI32.Bematech_FI_FormatoDadosMFD(arquivo, arquivoDestino, "0", "2", Convert.ToString(intervaloInicial), Convert.ToString(intervaloFinal), usuario);
                                break;
                        }
                        break;
                    case 2:
                        switch (tipo)
                        {
                            case "1":
                                DARUMA_FW.iRetorno = DARUMA_FW.rGerarEspelhoMFD_ECF_Daruma("1", string.Format("{0:ddMMyy}", dataInicial), string.Format("{0:ddMMyy}", dataFinal));
                                break;
                            case "2":
                                // DARUMA32.Int_Retorno =DARUMA32.Daruma_FIMFD_DownloadDaMFD(Convert.ToString(intervaloInicial), Convert.ToString(intervaloFinal));
                                DARUMA_FW.iRetorno = DARUMA_FW.rGerarEspelhoMFD_ECF_Daruma("2", intervaloInicial.ToString(), intervaloFinal.ToString());
                                break;
                        }

                        if (File.Exists(@ConfiguracoesECF.pathRetornoECF + "Espelho_MFD.txt"))
                        {
                            File.Copy(@ConfiguracoesECF.pathRetornoECF + "Espelho_MFD.txt", arquivoDestino);
                        }
                        break;
                    case 3:
                        switch (tipo)
                        {
                            case "0":
                                Elgin32.Elgin_DownloadMFD(arquivo, "0", "", "", "");
                                System.Threading.Thread.Sleep(500);
                                Elgin32.Elgin_FormatoDadosMFD(arquivo, arquivoDestino, "0", "0", "", "", "");
                                break;
                            case "1":
                                Elgin32.Elgin_DownloadMFD(arquivo, "1", string.Format("{0:dd/MM/yyyy}", dataInicial), string.Format("{0:dd/MM/yyyy}", dataFinal), "");
                                Elgin32.Elgin_FormatoDadosMFD(arquivo, arquivoDestino, "0", "1", string.Format("{0:dd/MM/yyyy}", dataInicial), string.Format("{0:dd/MM/yyyy}", dataFinal), "");
                                break;
                            case "2":
                                Elgin32.Elgin_DownloadMFD(arquivo, "2", Convert.ToString(intervaloInicial), Convert.ToString(intervaloFinal), usuario);
                                Elgin32.Elgin_FormatoDadosMFD(arquivo, arquivoDestino, "0", "2", Convert.ToString(intervaloInicial), Convert.ToString(intervaloFinal), usuario);
                                break;
                        }
                        break;
                    case 4:
                        switch (tipo)
                        {
                            case "0":
                                Sweda32.ECF_ReproduzirMemoriaFiscalMFD("0", string.Format("{0:dd/MM/yyyy}", dataInicial), string.Format("{0:dd/MM/yyyy}", dataFinal), @arquivoDestino, @"c:\sweda\mf.bin");
                                System.Threading.Thread.Sleep(500);
                                break;
                            case "1":
                                Sweda32.ECF_ReproduzirMemoriaFiscalMFD("1", string.Format("{0:dd/MM/yyyy}", dataInicial), string.Format("{0:dd/MM/yyyy}", dataFinal), @arquivoDestino, @"c:\sweda\mf.bin");
                                break;
                            case "2":
                                Sweda32.ECF_ReproduzirMemoriaFiscalMFD("3", intervaloInicial.ToString(), intervaloFinal.ToString(), @arquivoDestino, "");
                                break;
                        }
                        break;
                };
            }

            else
            {
                try
                {
                    if (tipo == "1")
                        ecfAcbr.PafMF_MFD_Espelho(dataInicial.Date, dataFinal.Date, arquivoDestino);
                    else
                        ecfAcbr.PafMF_MFD_Espelho(intervaloInicial, intervaloFinal, arquivoDestino);
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    return false;
                }

            }

            return true;
        }

        #endregion PAF-ECF Menu Fiscal

        public static bool ReducaoZ()
        {
            if (ConfiguracoesECF.idNFC > 0 || ConfiguracoesECF.idECF==9999)
                return true;

            DavOSFinalizados();

            if (GlbVariaveis.glb_Acbr == false)
            {
                #region
                
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        // Cancelando as pré-venda caso existam. 
                        // Ação requerida pelo PAF-ECF

                        if (ZPendente() == true)
                        {
                            BemaFI32.Bematech_FI_ReducaoZ("", "");
                            if (Conexao.tipoConexao == 2)
                            {
                                Conexao.tipoConexao = 1;
                                PreVenda.Cancelar(-2);
                                Conexao.tipoConexao = 2;
                            }
                            else
                                PreVenda.Cancelar(-2);
                        }
                        else
                        {
                            if (Conexao.tipoConexao == 2)
                            {
                                Conexao.tipoConexao = 1;
                                PreVenda.Cancelar(-1);
                                Conexao.tipoConexao = 2;
                            }
                            else
                                PreVenda.Cancelar(-1);

                            BemaFI32.Bematech_FI_ReducaoZ("", "");
                        }

                        //BemaFI32.Bematech_FI_ReducaoZ("", "");
                        if (ConfiguracoesECF.zPendente)
                        {
                            ConfiguracoesECF.idECF = 1;
                            FuncoesECF fecf = new FuncoesECF();
                            if (fecf.AberturaDoDiaECF(decimal.Parse("0,00")) == true)
                            {
                                try
                                {
                                    siceEntities entidade = Conexao.CriarEntidade();

                                    var numeroCupom = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF);
                                    // Lancar para sair no R07
                                    caixa lancar = new caixa();
                                    lancar.tipopagamento = "SI";
                                    lancar.valor = 0;
                                    lancar.CodigoFilial = GlbVariaveis.glb_filial;
                                    lancar.filialorigem = GlbVariaveis.glb_filial;
                                    lancar.operador = GlbVariaveis.glb_Usuario;
                                    lancar.EnderecoIP = GlbVariaveis.glb_IP;
                                    lancar.dpfinanceiro = "Saldo inicial";
                                    lancar.data = DateTime.Now.Date;
                                    lancar.horaabertura = DateTime.Now.TimeOfDay;
                                    lancar.versao = GlbVariaveis.glb_Versao;
                                    lancar.historico = "*";
                                    lancar.vendedor = "000";
                                    // Aqui é usado o histórico para gravar o número do ECF para tirar o 
                                    // relatório R07 com os documentos não fiscais.
                                    // Onde: E=NumeroEcF g=Contador Nao Fiscal GNF e C=COO numero do cupom emitido
                                    lancar.historico = "E:" + ConfiguracoesECF.numeroECF.PadRight(3, ' ') + "G:" + FuncoesECF.ContadorNaoFiscalGNF().PadRight(6, ' ') + "C:" + numeroCupom;
                                    lancar.ecffabricacao = ConfiguracoesECF.nrFabricacaoECF;
                                    lancar.ecfnumero = ConfiguracoesECF.numeroECF;
                                    lancar.ecfmodelo = ConfiguracoesECF.modeloECF;
                                    lancar.gnf = FuncoesECF.ContadorNaoFiscalGNF().PadRight(6, ' ');
                                    lancar.ccf = " ";
                                    lancar.estornado = "N";
                                    lancar.coo = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).PadRight(6, ' ');
                                    lancar.eaddados = Funcoes.CriptografarMD5(lancar.ecffabricacao + lancar.coo + lancar.ccf + lancar.gnf + lancar.ecfmodelo + lancar.valor.ToString().Replace(",", ".") + lancar.tipopagamento);
                                    entidade.AddTocaixa(lancar);
                                    entidade.SaveChanges();


                                    EntityConnection conn = new EntityConnection(Conexao.stringConexao);

                                    using (conn)
                                    {
                                        conn.Open();
                                        EntityCommand cmd = conn.CreateCommand();
                                        cmd.CommandText = "siceEntities.AberturaDia";
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                                        filial.Direction = ParameterDirection.Input;
                                        filial.Value = GlbVariaveis.glb_filial;

                                        EntityParameter data = cmd.Parameters.Add("dataAtual", DbType.Date);
                                        data.Direction = ParameterDirection.Input;
                                        data.Value = GlbVariaveis.Sys_Data.Date;

                                        EntityParameter hora = cmd.Parameters.Add("hora", DbType.Time);
                                        hora.Direction = ParameterDirection.Input;
                                        hora.Value = DateTime.Now.TimeOfDay;

                                        EntityParameter ecf = cmd.Parameters.Add("nrFabricacaoECF", DbType.String);
                                        ecf.Direction = ParameterDirection.Input;
                                        ecf.Value = ConfiguracoesECF.nrFabricacaoECF;

                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception erro)
                                {
                                    MessageBox.Show(erro.Message);
                                }

                                
                            }
                            else
                            {
                                MessageBox.Show("Não foi possível abrir o movimento diário !", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        //printer.RelatoriosFiscais.ImprimirReducaoZ();
                        return true;

                    case 2:

                        System.Threading.Thread.Sleep(1000);
                        if (ZPendente() == true)
                        {
                            DARUMA_FW.iRetorno = DARUMA_FW.iReducaoZ_ECF_Daruma("", "");
                            if (Conexao.tipoConexao == 2)
                            {
                                Conexao.tipoConexao = 1;
                                PreVenda.Cancelar(-2);
                                Conexao.tipoConexao = 2;
                            }
                            else
                                PreVenda.Cancelar(-2);
                        }
                        else
                        {
                            if (Conexao.tipoConexao == 2)
                            {
                                Conexao.tipoConexao = 1;
                                PreVenda.Cancelar(-1);
                                Conexao.tipoConexao = 2;
                            }
                            else
                                PreVenda.Cancelar(-1);

                            DARUMA_FW.iRetorno = DARUMA_FW.iReducaoZ_ECF_Daruma("", "");
                        }


                        //DARUMA32.Int_Retorno = DARUMA32.Daruma_FI_FechamentoDoDia();
                        

                        if (ConfiguracoesECF.zPendente)
                        {
                            try
                            {
                                ConfiguracoesECF.idECF = 2;
                                System.Threading.Thread.Sleep(1000);
                                // Iniciar aqui para cancelar as Pre-Vendas
                                FuncoesECF fecf = new FuncoesECF();
                                if (fecf.AberturaDoDiaECF(decimal.Parse("0,00")) == false)
                                {
                                    MessageBox.Show("Não foi possível abrir o movimento diário !", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {

                                    siceEntities entidade = Conexao.CriarEntidade();

                                    var numeroCupom = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF);
                                    // Lancar para sair no R07
                                    caixa lancar = new caixa();
                                    lancar.tipopagamento = "SI";
                                    lancar.valor = 0;
                                    lancar.CodigoFilial = GlbVariaveis.glb_filial;
                                    lancar.filialorigem = GlbVariaveis.glb_filial;
                                    lancar.operador = GlbVariaveis.glb_Usuario;
                                    lancar.EnderecoIP = GlbVariaveis.glb_IP;
                                    lancar.dpfinanceiro = "Saldo inicial";
                                    lancar.data = DateTime.Now.Date;
                                    lancar.horaabertura = DateTime.Now.TimeOfDay;
                                    lancar.versao = GlbVariaveis.glb_Versao;
                                    lancar.historico = "*";
                                    lancar.vendedor = "000";
                                    // Aqui é usado o histórico para gravar o número do ECF para tirar o 
                                    // relatório R07 com os documentos não fiscais.
                                    // Onde: E=NumeroEcF g=Contador Nao Fiscal GNF e C=COO numero do cupom emitido
                                    lancar.historico = "E:" + ConfiguracoesECF.numeroECF.PadRight(3, ' ') + "G:" + FuncoesECF.ContadorNaoFiscalGNF().PadRight(6, ' ') + "C:" + numeroCupom;
                                    lancar.ecffabricacao = ConfiguracoesECF.nrFabricacaoECF;
                                    lancar.ecfnumero = ConfiguracoesECF.numeroECF;
                                    lancar.ecfmodelo = ConfiguracoesECF.modeloECF;
                                    lancar.gnf = FuncoesECF.ContadorNaoFiscalGNF().PadRight(6, ' ');
                                    lancar.ccf = " ";
                                    lancar.estornado = "N";
                                    lancar.coo = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).PadRight(6, ' ');
                                    lancar.eaddados = Funcoes.CriptografarMD5(lancar.ecffabricacao + lancar.coo + lancar.ccf + lancar.gnf + lancar.ecfmodelo + lancar.valor.ToString().Replace(",", ".") + lancar.tipopagamento);
                                    entidade.AddTocaixa(lancar);
                                    entidade.SaveChanges();

                                    EntityConnection conn = new EntityConnection(Conexao.stringConexao);

                                    using (conn)
                                    {
                                        conn.Open();
                                        EntityCommand cmd = conn.CreateCommand();
                                        cmd.CommandText = "siceEntities.AberturaDia";
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                                        filial.Direction = ParameterDirection.Input;
                                        filial.Value = GlbVariaveis.glb_filial;

                                        EntityParameter data = cmd.Parameters.Add("dataAtual", DbType.Date);
                                        data.Direction = ParameterDirection.Input;
                                        data.Value = GlbVariaveis.Sys_Data.Date;

                                        EntityParameter hora = cmd.Parameters.Add("hora", DbType.Time);
                                        hora.Direction = ParameterDirection.Input;
                                        hora.Value = DateTime.Now.TimeOfDay;

                                        EntityParameter ecf = cmd.Parameters.Add("nrFabricacaoECF", DbType.String);
                                        ecf.Direction = ParameterDirection.Input;
                                        ecf.Value = ConfiguracoesECF.nrFabricacaoECF;

                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (Exception erro)
                            {
                                MessageBox.Show(erro.Message);
                            }
                        }
                        return true;
                    case 3:

                        if (ZPendente() == true)
                        {
                            Elgin32.Elgin_ReducaoZ("", "");
                            if (Conexao.tipoConexao == 2)
                            {
                                Conexao.tipoConexao = 1;
                                PreVenda.Cancelar(-2);
                                Conexao.tipoConexao = 2;
                            }
                            else
                                PreVenda.Cancelar(-2);
                        }
                        else
                        {
                            if (Conexao.tipoConexao == 2)
                            {
                                Conexao.tipoConexao = 1;
                                PreVenda.Cancelar(-1);
                                Conexao.tipoConexao = 2;
                            }
                            else
                                PreVenda.Cancelar(-1);

                            Elgin32.Elgin_ReducaoZ("", "");
                        }

                       
                        //printer.RelatoriosFiscais.ImprimirReducaoZ();

                        if (ConfiguracoesECF.zPendente)
                        {
                            try
                            {
                                ConfiguracoesECF.idECF = 2;
                                System.Threading.Thread.Sleep(1000);
                                // Iniciar aqui para cancelar as Pre-Vendas
                                FuncoesECF fecf = new FuncoesECF();
                                if (fecf.AberturaDoDiaECF(decimal.Parse("0,00")) == false)
                                {
                                    MessageBox.Show("Não foi possível abrir o movimento diário !", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {

                                    siceEntities entidade = Conexao.CriarEntidade();

                                    var numeroCupom = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF);
                                    // Lancar para sair no R07
                                    caixa lancar = new caixa();
                                    lancar.tipopagamento = "SI";
                                    lancar.valor = 0;
                                    lancar.CodigoFilial = GlbVariaveis.glb_filial;
                                    lancar.filialorigem = GlbVariaveis.glb_filial;
                                    lancar.operador = GlbVariaveis.glb_Usuario;
                                    lancar.EnderecoIP = GlbVariaveis.glb_IP;
                                    lancar.dpfinanceiro = "Saldo inicial";
                                    lancar.data = DateTime.Now.Date;
                                    lancar.horaabertura = DateTime.Now.TimeOfDay;
                                    lancar.versao = GlbVariaveis.glb_Versao;
                                    lancar.historico = "*";
                                    lancar.vendedor = "000";
                                    // Aqui é usado o histórico para gravar o número do ECF para tirar o 
                                    // relatório R07 com os documentos não fiscais.
                                    // Onde: E=NumeroEcF g=Contador Nao Fiscal GNF e C=COO numero do cupom emitido
                                    lancar.historico = "E:" + ConfiguracoesECF.numeroECF.PadRight(3, ' ') + "G:" + FuncoesECF.ContadorNaoFiscalGNF().PadRight(6, ' ') + "C:" + numeroCupom;
                                    lancar.ecffabricacao = ConfiguracoesECF.nrFabricacaoECF;
                                    lancar.ecfnumero = ConfiguracoesECF.numeroECF;
                                    lancar.ecfmodelo = ConfiguracoesECF.modeloECF;
                                    lancar.gnf = FuncoesECF.ContadorNaoFiscalGNF().PadRight(6, ' ');
                                    lancar.ccf = " ";
                                    lancar.estornado = "N";
                                    lancar.coo = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).PadRight(6, ' ');
                                    lancar.eaddados = Funcoes.CriptografarMD5(lancar.ecffabricacao + lancar.coo + lancar.ccf + lancar.gnf + lancar.ecfmodelo + lancar.valor.ToString().Replace(",", ".") + lancar.tipopagamento);
                                    entidade.AddTocaixa(lancar);
                                    entidade.SaveChanges();

                                    EntityConnection conn = new EntityConnection(Conexao.stringConexao);

                                    using (conn)
                                    {
                                        conn.Open();
                                        EntityCommand cmd = conn.CreateCommand();
                                        cmd.CommandText = "siceEntities.AberturaDia";
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                                        filial.Direction = ParameterDirection.Input;
                                        filial.Value = GlbVariaveis.glb_filial;

                                        EntityParameter data = cmd.Parameters.Add("dataAtual", DbType.Date);
                                        data.Direction = ParameterDirection.Input;
                                        data.Value = GlbVariaveis.Sys_Data.Date;

                                        EntityParameter hora = cmd.Parameters.Add("hora", DbType.Time);
                                        hora.Direction = ParameterDirection.Input;
                                        hora.Value = DateTime.Now.TimeOfDay;

                                        EntityParameter ecf = cmd.Parameters.Add("nrFabricacaoECF", DbType.String);
                                        ecf.Direction = ParameterDirection.Input;
                                        ecf.Value = ConfiguracoesECF.nrFabricacaoECF;

                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (Exception erro)
                            {
                                MessageBox.Show(erro.Message);
                            }
                        }

  
                        return true;
                    case 4:

                        if (ZPendente() == true)
                        {
                            Sweda32.ECF_ReducaoZ("", "");
                            if (Conexao.tipoConexao == 2)
                            {
                                Conexao.tipoConexao = 1;
                                PreVenda.Cancelar(-2);
                                Conexao.tipoConexao = 2;
                            }
                            else
                                PreVenda.Cancelar(-2);
                        }
                        else
                        {
                            if (Conexao.tipoConexao == 2)
                            {
                                Conexao.tipoConexao = 1;
                                PreVenda.Cancelar(-1);
                                Conexao.tipoConexao = 2;
                            }
                            else
                                PreVenda.Cancelar(-1);

                            Sweda32.ECF_ReducaoZ("", "");
                        }

                        

                        return true;
                }
                return false;

                #endregion

            }
            else
            {
                try
                {
                    if (ZPendente() == true)
                    {
                        ecfAcbr.ReducaoZ();
                        if (Conexao.tipoConexao == 2)
                        {
                            Conexao.tipoConexao = 1;
                            PreVenda.Cancelar(-2);
                            Conexao.tipoConexao = 2;
                        }
                        else
                            PreVenda.Cancelar(-2);
                    }
                    else
                    {
                        if (Conexao.tipoConexao == 2)
                        {
                            Conexao.tipoConexao = 1;
                            PreVenda.Cancelar(-1);
                            Conexao.tipoConexao = 2;
                        }
                        else
                            PreVenda.Cancelar(-1);

                        ecfAcbr.ReducaoZ();
                    }

                    if (ConfiguracoesECF.zPendente)
                    {
                        //ConfiguracoesECF.idECF = 2;
                        System.Threading.Thread.Sleep(1000);
                        // Iniciar aqui para cancelar as Pre-Vendas
                        FuncoesECF fecf = new FuncoesECF();
                        if (fecf.AberturaDoDiaECF(decimal.Parse("0,00")) == false)
                        {
                            MessageBox.Show("Não foi possível abrir o movimento diário !", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                                siceEntities entidade = Conexao.CriarEntidade();

                                var movimento = (from n in entidade.movimento
                                     where n.codigofilial == GlbVariaveis.glb_filial
                                     && n.finalizado == " "
                                     select n.finalizado);

                                if (movimento.Count() == 0)
                                {

                                    // Abrindo o movimento diário                        
                                    movimento novo = new movimento();
                                    novo.codigofilial = GlbVariaveis.glb_filial;
                                    novo.data = GlbVariaveis.Sys_Data;
                                    novo.finalizado = " ";
                                    novo.SaldoCaixa = 0;
                                    novo.Credito = 0;
                                    novo.Debito = 0;
                                    novo.Saldocrediario = 0;
                                    novo.creditocr = 0;
                                    novo.debitocr = 0;
                                    novo.saldocartao = 0;
                                    novo.creditoca = 0;
                                    novo.creditoch = 0;
                                    novo.debitoca = 0;
                                    novo.saldocheques = 0;
                                    novo.debitoch = 0;
                                    novo.custofinalestoque = 0;
                                    novo.customediofinalestoque = 0;
                                    novo.id = GlbVariaveis.glb_IP;
                                    entidade.AddTomovimento(novo);
                                    entidade.SaveChanges();
                                }

                                var numeroCupom = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF);
                                // Lancar para sair no R07
                                caixa lancar = new caixa();
                                lancar.tipopagamento = "SI";
                                lancar.valor = 0;
                                lancar.CodigoFilial = GlbVariaveis.glb_filial;
                                lancar.filialorigem = GlbVariaveis.glb_filial;
                                lancar.operador = GlbVariaveis.glb_Usuario;
                                lancar.EnderecoIP = GlbVariaveis.glb_IP;
                                lancar.dpfinanceiro = "Saldo inicial";
                                lancar.data = DateTime.Now.Date;
                                lancar.horaabertura = DateTime.Now.TimeOfDay;
                                lancar.versao = GlbVariaveis.glb_Versao;
                                lancar.historico = "*";
                                lancar.vendedor = "000";
                                // Aqui é usado o histórico para gravar o número do ECF para tirar o 
                                // relatório R07 com os documentos não fiscais.
                                // Onde: E=NumeroEcF g=Contador Nao Fiscal GNF e C=COO numero do cupom emitido
                                lancar.historico = "E:" + ConfiguracoesECF.numeroECF.PadRight(3, ' ') + "G:" + FuncoesECF.ContadorNaoFiscalGNF().PadRight(6, ' ') + "C:" + numeroCupom;
                                lancar.ecffabricacao = ConfiguracoesECF.nrFabricacaoECF;
                                lancar.ecfnumero = ConfiguracoesECF.numeroECF;
                                lancar.ecfmodelo = ConfiguracoesECF.modeloECF;
                                lancar.gnf = FuncoesECF.ContadorNaoFiscalGNF().PadRight(6, ' ');
                                lancar.ccf = " ";
                                lancar.estornado = "N";
                                lancar.coo = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).PadRight(6, ' ');
                                lancar.eaddados = Funcoes.CriptografarMD5(lancar.ecffabricacao + lancar.coo + lancar.ccf + lancar.gnf + lancar.ecfmodelo + lancar.valor.ToString().Replace(",", ".") + lancar.tipopagamento);
                                entidade.AddTocaixa(lancar);
                                entidade.SaveChanges();

                                EntityConnection conn = new EntityConnection(Conexao.stringConexao);

                                using (conn)
                                {
                                    conn.Open();
                                    EntityCommand cmd = conn.CreateCommand();
                                    cmd.CommandText = "siceEntities.AberturaDia";
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                                    filial.Direction = ParameterDirection.Input;
                                    filial.Value = GlbVariaveis.glb_filial;

                                    EntityParameter data = cmd.Parameters.Add("dataAtual", DbType.Date);
                                    data.Direction = ParameterDirection.Input;
                                    data.Value = GlbVariaveis.Sys_Data.Date;

                                    EntityParameter hora = cmd.Parameters.Add("hora", DbType.Time);
                                    hora.Direction = ParameterDirection.Input;
                                    hora.Value = DateTime.Now.TimeOfDay;

                                    EntityParameter ecf = cmd.Parameters.Add("nrFabricacaoECF", DbType.String);
                                    ecf.Direction = ParameterDirection.Input;
                                    ecf.Value = ConfiguracoesECF.nrFabricacaoECF;

                                    cmd.ExecuteNonQuery();
                                }
                        }

                    }

                    return true;
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    return false;
                }

            }
        }

        public bool SangriaECF(decimal valor)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;


            VerificaConexaoECF();

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        try
                        {
                            BemaFI32.Bematech_FI_Sangria(Funcoes.FormatarZerosEsquerda(valor, 14, true));
                            return true;
                        }
                        catch
                        {
                            throw new Exception("Não foi possível efetuar a sangria !");
                        }
                    case 2:
                        DARUMA_FW.iRetorno = DARUMA_FW.iSangria_ECF_Daruma(valor.ToString(), "SANGRIA");
                        if (DARUMA_FW.iRetorno != 1)
                        {
                            throw new Exception("Não foi possível efetuar a sangria !");
                        };
                        return true;
                    case 3:
                        try
                        {
                            Elgin32.Elgin_Sangria(Funcoes.FormatarZerosEsquerda(valor, 14, true));
                            return true;
                        }
                        catch
                        {
                            throw new Exception("Não foi possível efetuar a sangria !");
                        }
                    case 4:
                        try
                        {

                            int iRetornoSW = Sweda32.ECF_Sangria(string.Format("{0:N2}", valor.ToString()));
                            return true;
                            //if (iRetornoSW != 1)
                            //    throw new Exception("Não foi possível efetuar a sangria ! " + iRetornoSW.ToString());

                        }
                        catch (Exception ex)
                        {

                            throw new Exception("Não foi possível efetuar a sangria ! " + ex.Message);
                        }
                }
            }
            else
            {
                try
                {
                    ecfAcbr.Sangria(valor, "", "SANGRIA", "DINHEIRO", 0);
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }
            }
            return false;
        }

        private static void GravarContadorRelatorioGerencial(string tipo="RG")
        {

            if (ConfiguracoesECF.idNFC > 0)
                return;

            if (ConfiguracoesECF.pdv == false)
                return;

            try
            {
                if (!Conexao.onLine)
                    return;

                siceEntities entidade = Conexao.CriarEntidade();

                if (Conexao.ConexaoOnline() == false && Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                
                contrelatoriogerencial crg = new contrelatoriogerencial();

                var dataHoraCupomECF = DataHoraUltDocumentoECF().ToString();
                var numeroCupomECF = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).ToString();
                var numeroECF = ConfiguracoesECF.numeroECF;
                Thread.Sleep(200);
                var contadorCupomECF = FuncoesECF.CCFContadorCupomECF().ToString();
                Thread.Sleep(200);
                var contadorDebitoCreditoECF = FuncoesECF.ContadorDebitoCredidoCDC();
                Thread.Sleep(200);
                var contadorGNF = FuncoesECF.ContadorNaoFiscalGNF();
                Thread.Sleep(200);
                var contadorGRG = ContadorRelatorioGerencial(); // Contador Relatório Gerencial
                if (tipo == "CC")
                    contadorGRG = " ";
                if (tipo == "RG")
                    contadorDebitoCreditoECF = "0000";

                // Gravar no contador de relatório gerencial
                crg.codigofilial = GlbVariaveis.glb_filial;
                crg.data = dataHoraCupomECF == null ? DateTime.Now.Date : Convert.ToDateTime(dataHoraCupomECF).Date;
                crg.ecfnumero = numeroECF;
                crg.coo = numeroCupomECF; // Contador de Ordem de Operação
                crg.gnf = contadorGNF;
                crg.grg = contadorGRG;
                crg.cdc = contadorDebitoCreditoECF;
                crg.horaemissao = dataHoraCupomECF == null ? DateTime.Now.TimeOfDay : Convert.ToDateTime(dataHoraCupomECF).TimeOfDay;
                crg.tipopagamentoECF = tipo;
                crg.denominacao = tipo;
                crg.ecffabricacao = ConfiguracoesECF.nrFabricacaoECF;
                crg.EADDados = Funcoes.CriptografarMD5(ConfiguracoesECF.nrFabricacaoECF + numeroCupomECF + contadorGNF +tipo+string.Format("{0:yyyy-MM-dd}", dataHoraCupomECF)+contadorDebitoCreditoECF+tipo);
                entidade.AddTocontrelatoriogerencial(crg);
                entidade.SaveChanges();

                string stringConexao = "";

                if (Conexao.ConexaoOnline() == false && Conexao.tipoConexao == 2)
                    stringConexao = Conexao.stringConexaoRemoto;
                else
                    stringConexao = Conexao.stringConexao;


                using (EntityConnection conn = new EntityConnection(stringConexao))
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "siceEntities.AtualizarContRelGerencial";
                    cmd.CommandType = CommandType.StoredProcedure;

                    EntityParameter coo = cmd.Parameters.Add("cooRG", DbType.String);
                    coo.Direction = ParameterDirection.Input;
                    coo.Value = numeroCupomECF;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }


            }
            catch (Exception erro)
            {
                throw new Exception("Inserindo dados no controle de relatório gerencial: "+erro.Message.ToString());
            }
        }
        


// Métodos de Informação do ECF
        public static DateTime DataHoraDoECF(int ecf)
        {

            if (ConfiguracoesECF.idNFC>0)
            {
                return DateTime.Now;
            };


            string Str_Data = new string(' ', 6);
            string Str_Hora = new string(' ', 6);
            string dataHora = "";

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ecf)
                {
                    case 1:
                        
                            BemaFI32.Bematech_FI_DataHoraImpressora(ref Str_Data, ref Str_Hora);
                            dataHora = Str_Data + Str_Hora;
                            return Convert.ToDateTime(dataHora.Substring(0, 2) + "/" + dataHora.Substring(2, 2) + "/" + dataHora.Substring(4, 2) + " " +
                            dataHora.Substring(6, 2) + ":" + dataHora.Substring(8, 2) + ":" + dataHora.Substring(10, 2));

                            //MessageBox.Show("Tratado"+dataHora.Substring(0, 2) + "/" + dataHora.Substring(2, 2) + "/" + dataHora.Substring(4, 2) + " " +
                            //dataHora.Substring(6, 2) + ":" + dataHora.Substring(8, 2) + ":" + dataHora.Substring(10, 2));

                            //MessageBox.Show("ECF" + dataHora.ToString());
                       
                    // return printer.Informacao.DataHoraAtual;
                    case 2:
                        
                            StringBuilder dataECF = new StringBuilder(8);
                            StringBuilder horaECF = new StringBuilder(6);
                            DARUMA_FW.iRetorno = DARUMA_FW.rDataHoraImpressora_ECF_Daruma(dataECF, horaECF);
                            dataHora = dataECF.ToString().Substring(0, 8) + horaECF.ToString().Substring(0, 6);
                            return Convert.ToDateTime(dataHora.Substring(0, 2) + "/" + dataHora.Substring(2, 2) + "/" + dataHora.Substring(4, 4) + " " +
                            dataHora.Substring(8, 2) + ":" + dataHora.Substring(10, 2) + ":" + dataHora.Substring(12, 2));
                        
                    case 3:
                       
                            Elgin32.Elgin_DataHoraImpressora(ref Str_Data, ref Str_Hora);
                            dataHora = Str_Data + Str_Hora;
                            return Convert.ToDateTime(dataHora.Substring(0, 2) + "/" + dataHora.Substring(2, 2) + "/" + dataHora.Substring(4, 2) + " " +
                            dataHora.Substring(6, 2) + ":" + dataHora.Substring(8, 2) + ":" + dataHora.Substring(10, 2));
                        
                    case 4:
                       
                            return DateTime.Now;
                        
                }
            }
            else
            {
                return Convert.ToDateTime(ecfAcbr.DataHora.ToString());
            }
            return DateTime.Now;
        }

        public static string MarcaModeloTipoECF(string tipo)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return "NFCe";

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:

                       
                            string sMarca = new string(' ', 15);
                            string sModelo = new string(' ', 20);
                            string sTipo = new string(' ', 7);

                            int iRetorno = BemaFI32.Bematech_FI_MarcaModeloTipoImpressoraMFD(ref sMarca, ref sModelo, ref sTipo);

                            switch (tipo)
                            {
                                case "Marca": // Marca
                                    return "BEMATECH";
                                case "Modelo": // Modelo
                                    return sModelo.Replace("\0", "");
                                case "Tipo": // Tipo
                                    return "ECF-IF";
                            }
                            break;
                       
                    case 2:
                       

                            switch (tipo)
                            {
                                case "Marca":
                                    StringBuilder Str_Marca = new StringBuilder(' ', 2200);
                                    DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("80", Str_Marca);
                                    return Str_Marca.ToString().Substring(0, 20);
                                case "Modelo":
                                    StringBuilder Str_Modelo = new StringBuilder(' ', 2200);
                                    DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("81", Str_Modelo);
                                    return Str_Modelo.ToString().Substring(0, 20);
                                case "Tipo":
                                    StringBuilder Str_Tipo = new StringBuilder(' ', 2200);
                                    DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("79", Str_Tipo);
                                    return Str_Tipo.ToString().Substring(0, 7);
                            }
                            break;
                        

                    case 3:

                       

                            string sMarcaElgin = new string(' ', 15);
                            string sModeloElgin = new string(' ', 20);
                            string sTipoElgin = new string(' ', 7);

                            Elgin32.Int_Retorno = Elgin32.Elgin_MarcaModeloTipoImpressoraMFD(ref sMarcaElgin, ref sModeloElgin, ref sTipoElgin);

                            switch (tipo)
                            {
                                case "Marca": // Marca
                                    return "ELGIN";
                                case "Modelo": // Modelo
                                    return sModeloElgin;
                                case "Tipo": // Tipo
                                    return "ECF-IF";
                            }
                            break;
                        
                    case 4:


                        
                            string sMarcaSw = new string(' ', 15);
                            string sModeloSw = new string(' ', 20);
                            string sTipoSw = new string(' ', 7);

                            Sweda32.iRetorno = Sweda32.ECF_MarcaModeloTipoImpressoraMFD(sMarcaSw, sModeloSw, sTipoSw);

                            switch (tipo)
                            {
                                case "Marca": // Marca
                                    return "SWEDA";
                                case "Modelo": // Modelo
                                    return sModeloSw;
                                case "Tipo": // Tipo
                                    return "ECF-IF";
                            }
                            break;
                        
                }
            }
            else
            {
                string sModelo = new string(' ', 20);


                sModelo = ecfAcbr.Modelo.ToString();


                switch (tipo)
                {
                    case "Marca": // Marca
                        return "BEMATECH";
                    case "Modelo": // Modelo
                        return sModelo.Replace("\0", "");
                    case "Tipo": // Tipo
                        return "ECF-IF";
                }
               
            }
            return "";
        }


        public string CRZ()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return "0000";

            if (ConfiguracoesECF.pdv == false)
                return "0000";

            switch (ConfiguracoesECF.idECF)
            {
                case 1:
                    try
                    {
                        string sVersao = Funcoes.SetLength(4);                        
                        return sVersao.Replace("\0", "");
                    }
                    catch
                    {
                        throw new Exception("Não foi possível ler a versão ");
                    }
                case 2:
                    StringBuilder Str_Informacao = new StringBuilder(' ', 2200);
                    DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("24", Str_Informacao);
                    return Str_Informacao.ToString().Substring(0, 4);
                case 3:
                    try
                    {
                        string sVersaoElgin = Funcoes.SetLength(4);                        
                        return sVersaoElgin;
                    }
                    catch
                    {
                        throw new Exception("Não foi possível ler a versão ");
                    }

            }
            return "";

        }

        public string CRO()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return "000";

            if (ConfiguracoesECF.pdv == false)
                return "000";

            switch (ConfiguracoesECF.idECF)
            {
                case 1:
                    try
                    {
                        string sVersao = Funcoes.SetLength(4);
                        return sVersao.Replace("\0", "");
                    }
                    catch
                    {
                        throw new Exception("Não foi possível ler a versão ");
                    }
                case 2:
                    StringBuilder Str_Informacao = new StringBuilder(' ', 2200);
                    DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("23", Str_Informacao);
                    return Str_Informacao.ToString().Substring(0, 3);
                case 3:
                    try
                    {
                        string sVersaoElgin = Funcoes.SetLength(4);
                        return sVersaoElgin;
                    }
                    catch
                    {
                        throw new Exception("Não foi possível ler a versão ");
                    }

            }
            return "";

        }

         
        public string VersaoSoftwareECF()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return "000000";

            if (ConfiguracoesECF.pdv == false)
                return "000000";

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        try
                        {
                            string sVersao = Funcoes.SetLength(6);
                            BemaFI32.Bematech_FI_VersaoFirmware(ref sVersao);
                            return sVersao.Replace("\0", "");
                        }
                        catch
                        {
                            throw new Exception("Não foi possível ler a versão ");
                        }
                    case 2:
                        StringBuilder Str_Informacao = new StringBuilder(' ', 2200);
                        DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("83", Str_Informacao);
                        return Str_Informacao.ToString().Substring(0, 6);

                    case 3:
                        try
                        {
                            string sVersaoElgin = Funcoes.SetLength(6);
                            Elgin32.Elgin_VersaoFirmware(ref sVersaoElgin);
                            return sVersaoElgin;
                        }
                        catch
                        {
                            throw new Exception("Não foi possível ler a versão ");
                        }

                }
            }
            else
            {
                ecfAcbr.NumVersao.ToString();
            }
            return "";

        }

        public static DateTime DataUltimaReduzacaoZ()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return DateTime.Now;

            if (ConfiguracoesECF.pdv == false)
                return DateTime.Now;

            string Str_Data_ReducaoZ = new string(' ', 6);
            string Str_Hora_ReducaoZ = new string(' ', 6);
            string data = "";

            if (GlbVariaveis.glb_Acbr == false)
            {
                // Formato dd/mm/yyyy hh:mm:ss
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:

                        
                            Str_Data_ReducaoZ = new string(' ', 6);
                            Str_Hora_ReducaoZ = new string(' ', 6);
                            // return printer.Informacao.DataHoraUltimaReducao;                    

                            int iRetorno = BemaFI32.Bematech_FI_DataHoraReducao(ref Str_Data_ReducaoZ, ref Str_Hora_ReducaoZ);
                            data = Str_Data_ReducaoZ + Str_Hora_ReducaoZ;

                            if (Str_Data_ReducaoZ == "000000" || Str_Data_ReducaoZ == "" || Str_Data_ReducaoZ == null)
                                return DateTime.Now.AddDays(-1);
                            try
                            {
                                return Convert.ToDateTime(data.Substring(0, 2) + "/" + data.Substring(2, 2) + "/" + data.Substring(4, 2) + " " +
                                data.Substring(6, 2) + ":" + data.Substring(8, 2) + ":" + data.Substring(10, 2));
                            }
                            catch
                            {
                                return DateTime.Now.AddDays(-1);
                            }
                       

                    case 2:


                        
                            try
                            {
                                StringBuilder Str_Informacao = new StringBuilder(' ', 2200);
                                DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("154", Str_Informacao);
                                string dataHora = Str_Informacao.ToString().Substring(0, 14);
                                return Convert.ToDateTime(dataHora.Substring(0, 2) + "/" + dataHora.Substring(2, 2) + "/" + dataHora.Substring(4, 4) + " " +
                                dataHora.Substring(8, 2) + ":" + dataHora.Substring(10, 2) + ":" + dataHora.Substring(12, 2));
                            }
                            catch (Exception erro)
                            {
                                MessageBox.Show("aviso3" + DARUMA_FW.iRetorno.ToString() + " " + erro.ToString());
                                return DateTime.Now.AddDays(-1); ;
                            }
                        


                    case 3:

                       

                            Str_Data_ReducaoZ = new string(' ', 6);
                            Str_Hora_ReducaoZ = new string(' ', 6);
                            // return printer.Informacao.DataHoraUltimaReducao;                    

                            Elgin32.Int_Retorno = Elgin32.Elgin_DataHoraReducao(ref Str_Data_ReducaoZ, ref Str_Hora_ReducaoZ);
                            data = Str_Data_ReducaoZ + Str_Hora_ReducaoZ;

                            if (Str_Data_ReducaoZ == "000000" || Str_Data_ReducaoZ == "" || Str_Data_ReducaoZ == null)
                                return DateTime.Now.AddDays(-1);
                            try
                            {
                                return Convert.ToDateTime(data.Substring(0, 2) + "/" + data.Substring(2, 2) + "/" + data.Substring(4, 2) + " " +
                                data.Substring(6, 2) + ":" + data.Substring(8, 2) + ":" + data.Substring(10, 2));
                            }
                            catch
                            {
                                return DateTime.Now.AddDays(-1);
                            }
                        

                    case 4:

                       
                            return DateTime.Now;
                       


                }
            }
            else
            {
                try
                {
                    data = ecfAcbr.DataHoraUltimaReducaoZ.ToString();
                    return Convert.ToDateTime(data);
                }
                catch
                {
                    return DateTime.Now.AddDays(-1);
                }
            }
            return DateTime.Now;
        }

        public static string NumeroFabricacaoECF()
        {
            if (ConfiguracoesECF.idNFC>0)
            {
                return "";
            }

            if (ConfiguracoesECF.pdv == false)
                return "";

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                       
                            string nserie = new string(' ', 20);
                            int iRetorno = BemaFI32.Bematech_FI_NumeroSerieMFD(ref nserie);
                            return nserie.Substring(0, 20).Replace("\0", "");
                       
                    case 2:
                                           
                            StringBuilder Str_Informacao = new StringBuilder(2200);
                            DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("78", Str_Informacao);
                            //MessageBox.Show(Str_Informacao.ToString());
                            if (DARUMA_FW.iRetorno == 1)
                                return Str_Informacao.ToString().Substring(0, 20);
                            else
                                return "00000000000000000000";
                        

                    case 3:
                        
                            string sSerie = new string(' ', 21);
                            Elgin32.Elgin_NumeroSerie(ref sSerie);
                            return sSerie.Substring(0, 20);
                       
                    case 4:
                        
                            string iniciarVariavel = new string(' ', 20);
                            StringBuilder nSerieSW = new StringBuilder(iniciarVariavel);
                            Sweda32.iRetorno = Sweda32.ECF_NumeroSerieMFD(nSerieSW);
                            return nSerieSW.ToString();
                        
                }
            }
            else
            {
                return ecfAcbr.NumSerie;
            }
            return "";
        }

        public static decimal GrandeTotal()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return 0;

            if (ConfiguracoesECF.pdv == false)
                return 0;

            string cTotal = Funcoes.SetLength(18);

            if (GlbVariaveis.glb_Acbr == false)
            {

                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        
                            BemaFI32.Bematech_FI_GrandeTotal(ref cTotal);
                            ConfiguracoesECF.grandeTotal = Convert.ToDecimal(cTotal) / 100;
                            return Convert.ToDecimal(cTotal) / 100;
                       
                    case 2:

                       
                            StringBuilder Str_Informacao = new StringBuilder(2200);

                            for (int i = 0; i < 11; i++)
                            {
                                DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("1", Str_Informacao);

                                if (Str_Informacao.Length == 18)
                                    i = 11;

                            }
                            ConfiguracoesECF.grandeTotal = Convert.ToDecimal(Str_Informacao.ToString().Substring(0, 18)) / 100;
                            return Convert.ToDecimal(Str_Informacao.ToString().Substring(0, 18)) / 100;
                       

                    case 3:
                       
                            Elgin32.Int_Retorno = Elgin32.Elgin_GrandeTotal(ref cTotal);
                            ConfiguracoesECF.grandeTotal = Convert.ToDecimal(cTotal) / 100;
                            return Convert.ToDecimal(cTotal) / 100;
                       

                    case 4:

                            string iniciarVariavel = new string(' ', 18);
                            StringBuilder gtotal = new StringBuilder(iniciarVariavel);
                            Sweda32.iRetorno = Sweda32.ECF_GrandeTotal(gtotal);
                            ConfiguracoesECF.grandeTotal = Convert.ToDecimal(gtotal.ToString()) / 100;
                            return Convert.ToDecimal(gtotal.ToString()) / 100;
                       

                }
            }
            else
            {
                return ecfAcbr.GrandeTotal;
            }
            return 0;
        }


        public static decimal GrandeTotalInicioDia(bool retonarGTInicial,bool retornarGTFinal)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return 0;

            if (ConfiguracoesECF.pdv == false)
                return 0;

            string gtInicial = Funcoes.SetLength(19);
            string gtFinal = Funcoes.SetLength(19);
            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        BemaFI32.Bematech_FI_InicioFimGTsMFD(gtInicial, gtFinal);

                        if (retonarGTInicial)
                            return Convert.ToDecimal(gtInicial) / 100;
                        if (retornarGTFinal)
                            return Convert.ToDecimal(gtFinal) / 100;
                        break;
                    case 2:
                        StringBuilder Str_Informacao = new StringBuilder(2200);
                        DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("2", Str_Informacao);
                        return Convert.ToDecimal(Str_Informacao.ToString().Substring(0, 18)) / 100;
                    case 3:
                        Elgin32.Int_Retorno = Elgin32.Elgin_InicioFimGTsMFD(ref gtInicial, ref gtFinal);

                        if (retonarGTInicial)
                            return Convert.ToDecimal(gtInicial) / 100;
                        if (retornarGTFinal)
                            return Convert.ToDecimal(gtFinal) / 100;
                        break;
                    case 4:
                        return 0;
                }
            }
            else
            {
                ecfAcbr.DadosUltimaReducaoZ();
                if(retonarGTInicial)
                return (ecfAcbr.DadosReducaoZClass.ValorGrandeTotal - ecfAcbr.DadosReducaoZClass.ValorVendaBruta);

                if(retornarGTFinal)
                return ecfAcbr.GrandeTotal;

            }
            return 0;

           
        }

        public static int EstadoGaveta()
        {
            int estado = 0;
            if (ConfiguracoesECF.idNFC > 0 && ConfiguracoesECF.NFC == true)
            {
                if (ConfiguracoesECF.idNFC == 1 && ConfiguracoesECF.NFC == true)
                {
                    if (!ConfiguracoesECF.verificarGaveta)
                        return 1;

                    DARUMA_FW.iRetorno = DARUMA_FW.rStatusGaveta_DUAL_DarumaFramework(ref estado);
                    return estado;
                }
            }


            if (GlbVariaveis.glb_Acbr == false)
            {
               
                if (!ConfiguracoesECF.verificarGaveta)
                    return 1;

                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        // estado = 0 : Aberta
                        // estado = 1 : Fechada

                        int iRetorno = BemaFI32.Bematech_FI_VerificaEstadoGaveta(out estado);
                        return estado;
                    case 2:
                        DARUMA_FW.iRetorno = DARUMA_FW.rStatusGaveta_DUAL_DarumaFramework(ref estado);
                        return estado;
                    case 3:
                        Elgin32.Int_Retorno = Elgin32.Elgin_VerificaEstadoGaveta(ref estado);
                        return estado;
                    case 4:
                        Sweda32.ECF_VerificaEstadoGaveta(out estado);
                        return estado;
                }
            }
            else
            {
                if (GlbVariaveis.glb_modoGavetaAcbr == true && ConfiguracoesECF.idECF > 0 && ConfiguracoesECF.pdv == true)
                {
                    if (verificarGaveta == true && ecfAcbr.GavetaAberta == true)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            return 1;
        }

        public static void AbrirGaveta()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return ;

            if (ConfiguracoesECF.pdv == false)
                return;

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        int iRetorno = BemaFI32.Bematech_FI_AcionaGaveta();
                        break;
                    case 2:
                        DARUMA_FW.iRetorno = DARUMA_FW.eAbrirGaveta_ECF_Daruma();
                        break;
                    case 3:
                        Elgin32.Elgin_AcionaGaveta();
                        break;
                    case 4:
                        Sweda32.ECF_AcionaGaveta();
                        break;
                }
            }
            else
            {
                if (GlbVariaveis.glb_modoGavetaAcbr == true)
                {
                    try
                    {
                        ecfAcbr.AbreGaveta();
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.Message);
                    }
                }
            }
        }

        public static string DataHoraGravacaoUsuarioMF(string tipo)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return "";
            if (ConfiguracoesECF.pdv == false)
                return "";

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                       
                            string dataUsuario = new string(' ', 20);
                            string dataSwBasico = new string(' ', 20);
                            string mfAdicional = new string(' ', 1);

                            int iRetorno = BemaFI32.Bematech_FI_DataHoraGravacaoUsuarioSWBasicoMFAdicional(ref dataUsuario, ref dataSwBasico, ref mfAdicional);
                            switch (tipo)
                            {
                                case "Usuario": // Retornar Data e Hora do Usuario
                                    return dataUsuario.Trim() != "" ? string.Format("{0:dd/MM/yyyy HH:mm:ss}", dataUsuario) : null;
                                case "Software":
                                    return dataSwBasico.Trim() != "" ? string.Format("{0:dd/MM/yyyy HH:mm:ss}", dataSwBasico) : null;
                                case "MFAdicional":
                                    return mfAdicional == null ? "" : mfAdicional;
                            }
                            break;
                        

                    case 2:
                        #region explicacao
                        //Quero deixar uma dica para auxiliar no
                        //desenvolvimento do PAF-ECF nas impressoras Daruma, 
                        //    no registro R03 é pedido no item de nº 03 o MF 
                        //    Adicional, na impressora daruma iremos usar o método Daruma_FIMFD_RetornaInformação 
                        //    passando 2 parâmetros que são o indice 78 e o tamanho que é 20+1. 
                        //        Para saber se tem ou não MF Adicional será quando a 21º posição 
                        //        for branco então não temos a MF Adicional, se retornar a letra 
                        //        indicativa na 21º então tem MF Adicional. 
                        //Exemplo de MF Adicional:
                        //DR0105BR000000054098A
                        //Exemplo sem a MF Adicional:
                        //DR0105BR000000054098
                        //Luiz Canguini
                        //Suporte ao Desenvolvedor da Daruma
                        //Ligação Gratuita: 0800-770-3320
                        #endregion

                        
                            switch (tipo)
                            {
                                case "Usuario":
                                    StringBuilder str_Usuario = new StringBuilder(' ', 2200);
                                    DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("94", str_Usuario);
                                    return str_Usuario.ToString().Substring(0, 2);

                                case "Software":
                                    StringBuilder Str_Informacao = new StringBuilder(' ', 2200);
                                    DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("85", Str_Informacao);
                                    string dataHora = Str_Informacao.ToString().Substring(0, 14);
                                    return Convert.ToDateTime(dataHora.Substring(0, 2) + "/" + dataHora.Substring(2, 2) + "/" + dataHora.Substring(4, 4) + " " +
                                    dataHora.Substring(8, 2) + ":" + dataHora.Substring(10, 2) + ":" + dataHora.Substring(12, 2)).ToString();

                                case "MFAdicional":
                                    StringBuilder Str_Valor = new StringBuilder(' ', 2200);
                                    DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("78", Str_Valor);
                                    if (Str_Valor.Length == 20)
                                        return "";
                                    else
                                        return Str_Valor.ToString().Substring(20, 1);
                            }
                            break;
                       

                    case 3:


                        /*
                         Alteracao Feita Por Marckvaldo no dia 11/10/2012 
                         * 
                         * 
                         string dataUsuarioElgin = new string(' ', 20);
                       string dataSwBasicoElgin = new string(' ', 20);
                       string mfAdicionalElgin = new string(' ',1);
                    
                       Elgin32.Int_Retorno = Elgin32.Elgin_DataHoraSoftwareBasico(ref dataUsuarioElgin,ref dataSwBasicoElgin);
                       switch (tipo)
                       {
                           case "Usuario": // Retornar Data e Hora do Usuario
                               return dataUsuarioElgin.Trim() != "" ? string.Format("{0:dd/MM/yyyy HH:mm:ss}", dataUsuarioElgin) : null;
                           case "Software":
                               return dataSwBasicoElgin.Trim() != "" ? string.Format("{0:dd/MM/yyyy HH:mm:ss}",dataSwBasicoElgin):null;
                           case "MFAdicional":
                               return mfAdicionalElgin == null ? "" : mfAdicionalElgin;
                       }*/

                       
                            string dataSwBasicoElgin = new string(' ', 20);
                            string horaSwBasicoElgin = new string(' ', 20);
                            string mfAdicionalElgin = new string(' ', 1);

                            Elgin32.Int_Retorno = Elgin32.Elgin_DataHoraSoftwareBasico(ref dataSwBasicoElgin, ref horaSwBasicoElgin);
                            dataSwBasicoElgin = dataSwBasicoElgin.Substring(0, 6);
                            horaSwBasicoElgin = horaSwBasicoElgin.Substring(0, 6);

                            string dataElgin = FormatDataElgin(dataSwBasicoElgin + "" + horaSwBasicoElgin).ToString();



                            switch (tipo)
                            {
                                case "Usuario": // Retornar Data e Hora do Usuario
                                    return dataElgin;//dataSwBasicoElgin.Trim() != "" ? string.Format("{0:dd/MM/yyyy HH:mm:ss}", dataSwBasicoElgin) : null;
                                case "Software":
                                    return dataElgin;//horaSwBasicoElgin.Trim() != "" ? string.Format("{0:dd/MM/yyyy HH:mm:ss}", horaSwBasicoElgin) : null;
                                case "MFAdicional":
                                    return mfAdicionalElgin == null ? "" : mfAdicionalElgin;
                            }





                            break;
                       

                    case 4:

                        
                            string iniciarVariavel = new string(' ', 20);
                            string iniciarVariavel1 = new string(' ', 20);
                            string iniciarVariavel2 = new string(' ', 5);
                            StringBuilder dataUsuarioSw = new StringBuilder(iniciarVariavel);
                            StringBuilder dataSB = new StringBuilder(iniciarVariavel1);
                            StringBuilder mfAdicionalSw = new StringBuilder(iniciarVariavel2);


                            Sweda32.iRetorno = Sweda32.ECF_DataHoraGravacaoUsuarioSWBasicoMFAdicional(dataUsuarioSw, dataSB, mfAdicionalSw);
                            switch (tipo)
                            {
                                case "Usuario": // Retornar Data e Hora do Usuario
                                    return dataUsuarioSw.ToString().Trim() != "" ? string.Format("{0:dd/MM/yyyy HH:mm:ss}", dataUsuarioSw) : null;
                                case "Software":
                                    return dataSB.ToString().Trim() != "" ? string.Format("{0:dd/MM/yyyy HH:mm:ss}", dataSB) : null;
                                case "MFAdicional":
                                    return mfAdicionalSw.ToString().Trim() == null ? "" : mfAdicionalSw.ToString();
                            }
                            break;
                        
                        


                }
            }
            else
            {
                string dataUsuario = new string(' ', 20);
                string dataSwBasico = new string(' ', 20);
                string mfAdicional = new string(' ', 1);

                switch (tipo)
                {
                    case "Usuario": // Retornar Data e Hora do Usuario
                        dataUsuario = ecfAcbr.UsuarioAtual;
                        return dataUsuario;
                    case "Software":
                        dataSwBasico = ecfAcbr.DataHoraSB.ToString();
                        return dataSwBasico.Trim() != "" ? string.Format("{0:dd/MM/yyyy HH:mm:ss}", dataSwBasico) : null;
                    case "MFAdicional":
                        mfAdicional = ecfAcbr.MFAdicional.ToString();
                        return mfAdicional == null ? "" : mfAdicional;
                }

                
            }
            return "";
        }

        public string CNPJIEUsuarioECF(string tipo)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return "";

            if (ConfiguracoesECF.pdv == false)
                return "";

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        try
                        {
                            string cnpj = Funcoes.SetLength(18);
                            string ie = Funcoes.SetLength(15);
                            BemaFI32.Bematech_FI_CGC_IE(ref cnpj, ref ie);
                            switch (tipo)
                            {
                                case "CNPJ":
                                    return cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
                                case "IE":
                                    return ie.Replace(".", "").Replace("/", "").Replace("-", "");
                            }
                            return "";
                        }
                        catch
                        {
                            throw new Exception("Não foi possível ler a informação de CNPJ ou IE");
                        }
                    case 2:
                        switch (tipo)
                        {
                            case "CNPJ":
                                StringBuilder Str_CNPJ = new StringBuilder(' ', 2200);
                                DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("90", Str_CNPJ);
                                return Str_CNPJ.ToString().Substring(0, 20).Trim();
                            case "IE":
                                StringBuilder Str_IE = new StringBuilder(' ', 2200);
                                DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("91", Str_IE);
                                return Str_IE.ToString().Substring(0, 20).Trim();
                        }
                        break;
                    case 3:
                        try
                        {
                            string cnpj = Funcoes.SetLength(18);
                            string ie = Funcoes.SetLength(15);
                            Elgin32.Elgin_CGC_IE(ref cnpj, ref ie);
                            switch (tipo)
                            {
                                case "CNPJ":
                                    return cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
                                case "IE":
                                    return ie.Replace(".", "").Replace("/", "").Replace("-", "");
                            }
                            return "";
                        }
                        catch
                        {
                            throw new Exception("Não foi possível ler a informação ");
                        }
                    case 4:
                        try
                        {
                            string cnpjsw = Funcoes.SetLength(18);
                            string iesw = Funcoes.SetLength(15);
                            Sweda32.ECF_CGC_IE(cnpjsw, iesw);
                            switch (tipo)
                            {
                                case "CNPJ":
                                    return cnpjsw.Replace(".", "").Replace("/", "").Replace("-", "");
                                case "IE":
                                    return iesw.Replace(".", "").Replace("/", "").Replace("-", "");
                            }
                            return "";
                        }
                        catch
                        {
                            throw new Exception("Não foi possível ler a informação de CNPJ ou IE");
                        }





                }
            }
            else
            {
                try
                {
                    if (tipo == "CNPJ")
                        return ecfAcbr.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "");
                    if (tipo == "IE")
                        return ecfAcbr.IE.Replace(".", "").Replace("/", "").Replace("-", "");

                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    return "";
                }


                
            }
            return "";
        }

        public string UsuarioSubstituicaoECF()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return "";

            if (ConfiguracoesECF.pdv == false)
                return "";

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        try
                        {
                            string sUsuario = Funcoes.SetLength(4);
                            BemaFI32.Bematech_FI_NumeroSubstituicoesProprietario(ref sUsuario);
                            return sUsuario;
                        }
                        catch
                        {
                            throw new Exception("Não foi possível ler a versão ");
                        }
                    case 2:
                        StringBuilder Str_Informacao = new StringBuilder(' ', 2200);
                        DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("94", Str_Informacao);
                        return Str_Informacao.ToString().Substring(0, 2);
                    case 3:
                        try
                        {
                            string sUsuarioElgin = Funcoes.SetLength(4);
                            Elgin32.Elgin_NumeroSubstituicoesProprietario(ref sUsuarioElgin);
                            return sUsuarioElgin;
                        }
                        catch
                        {
                            throw new Exception("Não foi possível ler a versão ");
                        }

                    case 4:
                        string uUsuarioSw = new string(' ', 4);
                        Sweda32.ECF_NumeroSubstituicoesProprietario(uUsuarioSw);
                        return uUsuarioSw;
                }
            }
            else
            {
                return ecfAcbr.UsuarioAtual;
            }
            return "";

        }

        public List<string> Relatorio60M(bool lerDoECF=true,string arquivoOrigem="")
        {           
            List<string> dados = new List<string>();

            if (ConfiguracoesECF.idNFC > 0)
                return dados;

            string linha;
            string arquivo = ConfiguracoesECF.pathRetornoECF+"Retorno.txt";

            if (GlbVariaveis.glb_Acbr == false)
            {
                if (lerDoECF == false)
                    arquivo = arquivoOrigem;

                if (lerDoECF)
                {
                    if (File.Exists(arquivo))
                        File.Delete(arquivo);


                    switch (ConfiguracoesECF.idECF)
                    {
                        case 1:
                            BemaFI32.Bematech_FI_RelatorioTipo60Mestre();
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 2:
                            //DARUMA32.Int_Retorno = DARUMA32.Daruma_FI_RelatorioTipo60Mestre();

                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 3:
                            Elgin32.Elgin_RelatorioTipo60Mestre();
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 4:
                            Sweda32.ECF_RegistrosTipo60();
                            break;
                    }

                    //if (!File.Exists(@"c:\iqsistemas\Retorno.txt"))
                    //{
                    //    File.Copy(@"c:\Retorno.txt", @"c:\iqsistemas\Retorno.txt", true);
                    //}
                }

                System.IO.StreamReader file = new System.IO.StreamReader(arquivo, Encoding.GetEncoding("ISO-8859-1"));
                while ((linha = file.ReadLine()) != null)
                {
                    /* Alteracao feita Por Marckvlado no dia 11/10/2012
                     O Aquivo Retorno Criado pela ELgin E Diferente 
                     * por isso foi criado essa tomada de decisao
                     */
                    if (ConfiguracoesECF.idECF == 3)
                    {
                        if (linha.Contains("COO Inicial"))
                            dados.Add("COO Ini" + linha.Substring(40, 8));
                        if (linha.Contains("COO Final"))
                            dados.Add("COO Fin" + linha.Substring(40, 8));
                        if (linha.Contains("Contador de reducoes"))
                            dados.Add("Con Red" + linha.Substring(40, 8));
                        if (linha.Contains("Reinicio de Operação") || linha.Contains("Reinicio de Operacao"))
                            dados.Add("Rei Ope" + linha.Substring(40, 8));
                        if (linha.Contains("Venda Bruta"))
                            dados.Add("V Bruta" + linha.Substring(40, 8));
                        if (linha.Contains("Totalizador geral"))
                            dados.Add("T Geral" + linha.Substring(35, 13));

                    }
                    else
                    {

                        linha  = linha.PadLeft(52,' ').ToString();

                        if (linha.Contains("COO inicial"))
                            dados.Add("COO Ini" + linha.Substring(28, 24));
                        if (linha.Contains("COO final"))
                            dados.Add("COO Fin" + linha.Substring(28, 24));
                        if (linha.Contains("Contador de reduções"))
                            dados.Add("Con Red" + linha.Substring(28, 24));
                        if (linha.Contains("Reinicio de Operação") || linha.Contains("Reinicio de Operacao"))
                            dados.Add("Rei Ope" + linha.Substring(28, 24));
                        if (linha.Contains("Venda Bruta"))
                            dados.Add("V Bruta" + linha.Substring(28, 24));
                        if (linha.Contains("Totalizador geral"))
                            dados.Add("T Geral" + linha.Substring(28, 24));


                    }
                }
                file.Close();
            }
            else
            {
                

                //ecfAcbr.ReducaoZ();
                ecfAcbr.DadosUltimaReducaoZ();
                

                         
                dados.Add("COO Ini" + ecfAcbr.DadosReducaoZClass.NumeroCOOInicial);
                      
                dados.Add("COO Fin" + ecfAcbr.DadosReducaoZClass.COO);
                        
                dados.Add("Con Red" + ecfAcbr.DadosReducaoZClass.CRZ);
                       
                dados.Add("Rei Ope" + ecfAcbr.DadosReducaoZClass.CRO);
                       
                dados.Add("V Bruta" + ecfAcbr.DadosReducaoZClass.ValorVendaBruta);
                        
                dados.Add("T Geral" + ecfAcbr.DadosReducaoZClass.ValorGrandeTotal);
                
                
            }
           
            return dados;
        }

        public List<string> Relatorio60A(bool lerDoECF = true, string arquivoOrigem = "")
        {
            List<string> dados = new List<string>();


            if (ConfiguracoesECF.idNFC > 0)
                return dados;

            string linha;
            int seqICMS = 1;
            string arquivo = ConfiguracoesECF.pathRetornoECF+"Retorno.txt";



            if (GlbVariaveis.glb_Acbr == false)
            {

                if (lerDoECF == false)
                    arquivo = arquivoOrigem;


                //Essas varíaveis definem onde começa a possição dos valores dentro do arquivo
                // Exemplo: Bematech Cancelamento ICMS............:                     0,00 a posicao comeca depois do : (dois pontos)


                int posicaoValores = 0; ;
                int posicaoFinalValores = 0;

                if (lerDoECF)
                {
                    switch (ConfiguracoesECF.idECF)
                    {
                        case 1:
                            posicaoValores = 31;
                            try
                            {
                                BemaFI32.Bematech_FI_RelatorioTipo60AnaliticoMFD();
                                System.Threading.Thread.Sleep(1000);
                                break;
                            }
                            catch
                            {
                                throw new Exception("Não foi possível gerar o relatório 60M");
                            }
                        case 2:
                            posicaoValores = 28;
                            //DARUMA32.Int_Retorno = DARUMA32.Daruma_FI_RelatorioTipo60Analitico();
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 3:
                            posicaoValores = 31;
                            try
                            {
                                Elgin32.Elgin_RelatorioTipo60AnaliticoMFD();
                                System.Threading.Thread.Sleep(1000);
                                break;
                            }
                            catch
                            {
                                throw new Exception("Não foi possível gerar o relatório 60M");
                            }
                        case 4:
                            // A função para a SWEDA já sai o arquivo 60M e 60A aqui não executa nenhuma ação.
                            // O arquivo já foi gerado pelo comando 60M acima
                            break;
                    }
                };
                //if (!File.Exists(@"c:\iqsistemas\Retorno.txt"))
                //{
                //    File.Copy(@"c:\Retorno.txt", @"c:\iqsistemas\Retorno.txt", true);
                //}            
                System.IO.StreamReader file = new System.IO.StreamReader(arquivo, Encoding.GetEncoding("ISO-8859-1"));
                decimal totalICMS = 0;
                decimal icmsDebitado = 0;
                while ((linha = file.ReadLine()) != null)
                {
                    //if (posicaoFinalValores == 0)
                        posicaoFinalValores = linha.Length - posicaoValores;
                    
                    if (linha.Contains("Cancelamento ICMS") || linha.Contains("Cancelamentos"))
                        dados.Add("Can-T  " + linha.Substring(posicaoValores, posicaoFinalValores).PadLeft(posicaoValores - 7, ' '));
                    if (linha.Contains("Cancelamento ISSQN") || linha.Contains("Cancelamentos ISS"))// <-------Alteracao Feita Por Marckvald no Dia 11/10/2012
                        dados.Add("Can-S  " + linha.Substring(posicaoValores, posicaoFinalValores).PadLeft(posicaoValores - 7, ' '));
                    if (linha.Contains("Desconto ICMS") || linha.Contains("Descontos"))
                        dados.Add("DT     " + linha.Substring(posicaoValores, posicaoFinalValores).PadLeft(posicaoValores - 7, ' '));
                    if (linha.Contains("Desconto ISSQN"))
                        dados.Add("DS     " + linha.Substring(posicaoValores, posicaoFinalValores).PadLeft(posicaoValores - 7, ' '));
                    if (linha.Contains("Substituicao Tributaria ICMS") || linha.Contains("F..."))
                        dados.Add("F1     " + linha.Substring(posicaoValores, posicaoFinalValores).PadLeft(posicaoValores - 7, ' '));
                    if (linha.Contains("Substituicao Tributaria ISSQN"))
                        dados.Add("FS1    " + linha.Substring(posicaoValores, posicaoFinalValores).PadLeft(posicaoValores - 7, ' '));
                    if (linha.Contains("Isencao ICMS") || linha.Contains("I..."))
                        dados.Add("I1     " + linha.Substring(posicaoValores, posicaoFinalValores).PadLeft(posicaoValores - 7, ' '));
                    if (linha.Contains("Isencao ISSQN"))
                        dados.Add("IS1    " + linha.Substring(posicaoValores, posicaoFinalValores).PadLeft(posicaoValores - 7, ' '));
                    if (linha.Contains("Nao Incidencia ICMS") || linha.Contains("N..."))
                        dados.Add("N1     " + linha.Substring(posicaoValores, posicaoFinalValores).PadLeft(posicaoValores - 7, ' '));
                    if (linha.Contains("Nao Incidencia ISSQN"))
                        dados.Add("NS1    " + linha.Substring(posicaoValores, posicaoFinalValores).PadLeft(posicaoValores- 7, ' '));
                    if ((linha.PadLeft(1, ' ').Substring(0, 1).Contains("0") || linha.PadLeft(1, ' ').Substring(0, 1).Contains("1") || linha.PadLeft(1, ' ').Substring(0, 1).Contains("2")) && (linha != "" && linha != null))
                    {
                        // Só retorna as Aliquotas que nao sejam 0000
                        if (Convert.ToDecimal(linha.Substring(0, 4) + linha.Substring(posicaoValores, posicaoFinalValores).Trim()) > 0)
                        {
                            icmsDebitado += Convert.ToDecimal(linha.Substring(posicaoValores, posicaoFinalValores));
                            var valorICMS = Convert.ToDecimal(linha.Substring(posicaoValores, posicaoFinalValores)) * ((Convert.ToDecimal(linha.Substring(0, 4))) / 100) / 100;
                            valorICMS = (Math.Truncate(valorICMS * 100) / 100);
                            totalICMS += valorICMS;
                            dados.Add(string.Format("{0:00}", seqICMS) + "T" + linha.Substring(0, 4) + linha.Substring(posicaoValores, posicaoFinalValores).PadLeft(posicaoValores - 7));
                            seqICMS++;
                        }
                    }
                    
                    
                }

               /* foreach (string t in dados)
                {
                    MessageBox.Show(t.ToString()+"-"+t.Length.ToString());
                }*/
                // Nunca deve ser mudado da penúltima posição do array
                dados.Add("V ICMS " + totalICMS.ToString().Trim().PadLeft(posicaoValores - 7, ' '));
                // Nunca deve ser mudado da última posição do array
                dados.Add("T ICMS " + icmsDebitado.ToString().Trim().PadLeft(posicaoValores - 7, ' '));
                file.Close();
            }
            else
            {
                    //ecfAcbr.ReducaoZ();
                    ecfAcbr.DadosUltimaReducaoZ();

                    dados.Add("Can-T  " + ecfAcbr.DadosReducaoZClass.CancelamentoICMS);

                    dados.Add("Can-S  " + ecfAcbr.DadosReducaoZClass.CancelamentoISSQN);
                
                    dados.Add("DT     " + ecfAcbr.DadosReducaoZClass.DescontoICMS);

                    dados.Add("DS     " + ecfAcbr.DadosReducaoZClass.DescontoISSQN);

                    dados.Add("F1     " + ecfAcbr.DadosReducaoZClass.SubstituicaoTributariaICMS);

                    dados.Add("FS1    " + ecfAcbr.DadosReducaoZClass.SubstituicaoTributariaISSQN);

                    dados.Add("I1     " + ecfAcbr.DadosReducaoZClass.IsentoICMS);

                    dados.Add("IS1    " + ecfAcbr.DadosReducaoZClass.IsentoISSQN);

                    dados.Add("N1     " + ecfAcbr.DadosReducaoZClass.NaoTributadoICMS);

                    dados.Add("NS1    " + ecfAcbr.DadosReducaoZClass.NaoTributadoISSQN);

                    decimal valorTotalICMS = 0; 
                    foreach(var aliquota in ecfAcbr.DadosReducaoZClass.ICMS)
                    {
                        dados.Add(aliquota.Sequencia + aliquota.Tipo + aliquota.ValorAliquota.ToString().PadRight(4,'0')+"  " + aliquota.Total);

                        var valorICMS = (Convert.ToDecimal(aliquota.ValorAliquota * aliquota.Total) / 100);
                        valorICMS = (Math.Truncate(valorICMS * 100) / 100);
                        valorTotalICMS += valorICMS;
                    }


                    dados.Add("V ICMS " + valorTotalICMS.ToString());
                    // Nunca deve ser mudado da última posição do array
                    dados.Add("T ICMS " + ecfAcbr.DadosReducaoZClass.TotalICMS);
            }
            return dados;
        }

        public static decimal VendaLiquidaDiaECF()
        {

            if (ConfiguracoesECF.idNFC > 0)
                return 0;
            if (ConfiguracoesECF.pdv == false)
                return 0;

            string linha;
            string arquivo = ConfiguracoesECF.pathRetornoECF+"Retorno.txt";
            List<string> dados = new List<string>();
            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        try
                        {
                            BemaFI32.Bematech_FI_MapaResumo();
                            System.Threading.Thread.Sleep(1000);
                            break;

                        }
                        catch
                        {
                            throw new Exception("Não foi possível gerar o Mapa Resumo");
                        }
                    case 2:
                        StringBuilder vendaLiq = new StringBuilder(' ', 18);
                        DARUMA_FW.iRetorno = DARUMA_FW.rRetornarVendaLiquida_ECF_Daruma(vendaLiq);
                        System.Threading.Thread.Sleep(100);
                        return Convert.ToDecimal(vendaLiq.ToString().Substring(0, 18)) / 100;

                    case 3:
                        try
                        {
                            Elgin32.Elgin_MapaResumo();
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                        catch
                        {
                            throw new Exception("Não foi possível gerar o Mapa Resumo");
                        }
                    case 4:
                        Sweda32.ECF_MapaResumoMFD();
                        System.Threading.Thread.Sleep(1000);
                        break;
                }

                System.IO.StreamReader file = new System.IO.StreamReader(arquivo, Encoding.GetEncoding("ISO-8859-1"));
                while ((linha = file.ReadLine()) != null)
                {
                    if (linha.Contains("Venda Líquida"))
                        dados.Add("V Liq  " + linha.Substring(25, 15));
                    /* Alteracao feita Por Marckvaldo no dia 11/10/2012
                     * A impressoa Elgin retorna " Venda Liguida" sem o acento e tambem o espaco do substring maior do que as outras
                     */
                    if (linha.Contains("Venda Liquida"))
                        dados.Add("V Liq  " + linha.Substring(40, 7));
                }

                file.Close();
                //return Convert.ToDecimal(dados[0].Substring(7, 15));
                /*Alteracao Feita Por Marckvaldo no dia 11/10/2012*/

                if (ConfiguracoesECF.idECF == 3)
                {
                    return Convert.ToDecimal(dados[0].Substring(7, 7));
                }
                else
                {
                    return Convert.ToDecimal(dados[0].Substring(7, 15));
                }
            }
            else
            {
                ecfAcbr.DadosUltimaReducaoZ();
                return ecfAcbr.DadosReducaoZClass.VendaLiquida;
         
            }
         
            
        }

    // Rotinas para o PAF

        public static bool GravarGtECF()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;

            if (ConfiguracoesECF.pdv == false)
                return true;

            if (ConfiguracoesECF.idECF == 0 || string.IsNullOrEmpty(ConfiguracoesECF.nrFabricacaoECF) )
                return false;

            string gt = "0";
            try
            {                
                try
                {
                    
                        gt = GrandeTotal().ToString();
                    
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro obtendo GT Total do ECF, "+ex.Message);
                }

                try
                {
                    ConfiguracoesECF.grandeTotal = Convert.ToDecimal(gt);
                    XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");
                    var dados = from n in doc.Descendants("ecf")
                                where n.Attribute("numeroFabricacaoCriptografado").Value == (Funcoes.CriptografarComSenha(ConfiguracoesECF.nrFabricacaoECF, GlbVariaveis.glbSenhaIQ)) || n.Attribute("numeroFabricacaoCriptografado").Value == Funcoes.CriptografarComSenha(ConfiguracoesECF.nrFabricacaoECF.Substring(0, 15), GlbVariaveis.glbSenhaIQ)
                                select n;
                    if (dados != null)
                    {
                        dados.Attributes("gtCriptografado").First().Value = Funcoes.CriptografarComSenha(gt, GlbVariaveis.glbSenhaIQ);
                        doc.Save(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ArquivoCadastroECF.xml");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro gravando dados no arquivo criptografado, "+ex.Message);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro gravando GT criptografado: " + ex.Message);                
            }
            
        }

        #region Funcoes TEF
        public void VerificarStatusTEF()
        {
            if (!ConfiguracoesECF.tefDiscado && !ConfiguracoesECF.tefDedicado)
                return;
            if (!ConfiguracoesECF.pdv)
                return;

            // Retirado na homologacao
            //if (!ConfiguracoesECF.pdv)
            //    return;

            int idECF = 0;

            if (ConfiguracoesECF.idNFC > 0)
            {
                idECF = 5;
            }
            else
            {
                idECF = ConfiguracoesECF.idECF;
            }

            switch (idECF)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                            if (!ConfiguracoesECF.tefDedicado)
                            {
                                if (TEF.Transacoes("ntransacao") > 0 || TEF.Transacoes("ntransacaoConfirmadas") > 0)
                                {
                                    if (CupomFiscalAberto())
                                    {
                                        CancelarCupomECF();
                                        Thread.Sleep(500);
                                    };
                                    //Venda.ApagarItensFormaPagamento("Itens");                                    
                                    CancelarTransacaoTEFPendente();
                                }
                            }
                            if (ConfiguracoesECF.tefDedicado)
                            {
                                if (TEF.Transacoes("ntransacao") > 0)
                                {
                                    ConfirmarTransacao("todas");
                                    MessageBox.Show("Última transação foi confirmada. Favor reimprimir o último cupom","SICEpdv.net",MessageBoxButtons.OK,MessageBoxIcon.Information);
                                }
                            }

                            //if (TEF.VerificaGerenciadorTEF())
                            //{
                            //    MessageBox.Show("Inicialização OK");
                            //}
                            //else
                            //    MessageBox.Show(TEF.mensagemGerenciadorInativo);                            
                            break;

            }
        }

        public void CancelarTransacaoTEFPendente()
        {
            // verifica se tem transação TEF não confirmada se tiver envia 
            // uma não confirmação

            int idECF = 0;

            if (ConfiguracoesECF.idNFC > 0)
            {
                idECF = 5;
            }
            else
            {
                idECF = ConfiguracoesECF.idECF;
            }

            switch (idECF)
            {
                case 0:
                case 1:
                case 2: // Implantar Cancelar transacoes confirmadas
                case 3:
                case 4:
                case 5:
                   
                    if (TEF.Transacoes("ntransacao") > 0)
                    {
                        NaoConfirmarTransacao();
                    }

                    if (TEF.Transacoes("ntransacaoConfirmadas") > 0)
                    {
                        CancelarTransacao();
                    }                    
                    break;
            }
        }

        public void NaoConfirmarTransacao()
        {
            int idECF = 0;

            if (ConfiguracoesECF.idNFC > 0)
            {
                idECF = 5;
            }
            else
            {
                idECF = ConfiguracoesECF.idECF;
            }

            switch (idECF)
            {
                case 0:
                case 1:
                case 2:   
                case 3:
                case 4:
                case 5:
                    //while (true)
                    //{
                    //    try
                    //    {
                    //        SolicitacaoNaoConfirmacao NCN = printer.TEF.CriarSolicitacaoNaoConfirmacao();
                    //        string mensagem = printer.TEF.NaoConfirmarTransacao(NCN);
                    //        frmOperadorTEF formOperadorTEF = new frmOperadorTEF(mensagem);
                    //        formOperadorTEF.ShowDialog();
                    //        //MessageBox.Show (mensagem, "PDV-CSharp", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //        break;
                    //    }
                    //    catch (GerenciadorInativoException)
                    //    {
                    //        MessageBox.Show("Gerenciador Padrão não está ativo!", "Gerenciador Inativo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //    }
                    //    catch (TEFException erro)
                    //    {
                    //        MessageBox.Show(erro.Message, "PDV.net", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //    }
                    //}
                    //break;                
                    while (true)
                    {
                        Funcoes.TravarTeclado(false);              
                        try
                        {
                            Funcoes.TravarTeclado(false);
                            while (!TEF.VerificaGerenciadorTEF())
                            {
                                MessageBox.Show(TEF.mensagemGerenciadorInativo);
                            }

                            var arquivosRespTEF = ArquivosRespTransacaoTEF();
                            for (int nEspelhos = 0; nEspelhos < arquivosRespTEF.Count(); nEspelhos++)
                            {
                                if (TEF.VerificaUltimaTransacao(@arquivosRespTEF[nEspelhos]))
                                {                                    
                                    TEF.CancelaTef(@arquivosRespTEF[nEspelhos]);
                                    File.Delete(@arquivosRespTEF[nEspelhos]);
                                    File.Delete(@arquivosRespTEF[nEspelhos].Replace("INTPOS", "IMP").Replace("BKP", "TXT"));
                                    TEF.EncerraTEF();
                                    frmOperadorTEF frm = new frmOperadorTEF(TEF.mensagemOperador,false);
                                    frm.ShowDialog();                                   
                                    Funcoes.TravarTeclado(false);
                                }
                            };                            
                            break;
                        }
                        catch (Exception erro)
                        {
                            MessageBox.Show(erro.Message, "PDV.net", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Confirma a última transação aprovada.
        /// </summary>
        /// <remarks>Se o gerenciador padrão estiver inativo, fica em loop até que o mesmo
        /// seja ativado. </remarks>


        private void CancelarTransacao()
        {
            bool cancelamentoOk = false;

            int idECF = 0;

            if (ConfiguracoesECF.idNFC > 0)
            {
                idECF = 5;
            }
            else
            {
                idECF = ConfiguracoesECF.idECF;
            }

            switch (idECF)
            {
                case 0:
                case 1:                
                case 2:
                case 3:
                case 4:
                case 5:

                var arquivosRespTEF = ArquivosRespTransacaoTEFConfirmadas();
                                                
                for (int nEspelhos = 0; nEspelhos < arquivosRespTEF.Count(); nEspelhos++)
                {
                    if (TEF.CancelaTransacao(@arquivosRespTEF[nEspelhos]))
                    {
                      //  while (!cancelamentoOk)
                      //  {
                        cancelamentoOk = ImprimirComprovantes(true,true); // ImprimirCancelamento();
                                if (!cancelamentoOk)
                                {
                                    cancelamentoOk=false;
                                    NaoConfirmarTransacao(); // não confirma a última transação   
                                    nEspelhos--;
                                };                            
                      //  }
                    }
                    else
                    {
                        nEspelhos--;
                        frmOperadorTEF formOperadorTEF = null;
                        TEF.EncerraTEF();
                        formOperadorTEF = new frmOperadorTEF("!"+TEF.mensagemOperador,false);
                        formOperadorTEF.ShowDialog();
                       
                        formOperadorTEF.Dispose();  
                    }
                  };                               

                if (cancelamentoOk)
                    TEF.Transacoes("limpar");

            break;

            }

        }       
		
		public void AdministrativoTEF(string rede)
		{
            try
            {
                VerificaImpressoraLigada(true,false);
            }
            catch (Exception erro)
            {                
               MessageBox.Show(erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
               return;
            }

            if (FuncoesECF.CupomFiscalAberto())
            {
                MessageBox.Show("Cupom fiscal aberto", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var contadorItens = (from n in Conexao.CriarEntidade().vendas
                                 where n.cancelado == "N"
                                 && n.id == GlbVariaveis.glb_IP
                                 select n.inc).Count();

            if (contadorItens > 0)
            {
                MessageBox.Show("Cupom fiscal com itens", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            int idECF = 0;

            if (ConfiguracoesECF.idNFC > 0)
            {
                idECF = 5;
            }
            else
            {
                idECF = ConfiguracoesECF.idECF;
            }


            //ImprimirComprovantes(true);

            switch (idECF)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    try
                    {
                        DetectarECF(ConfiguracoesECF.idECF);
                        if (!TEF.VerificaGerenciadorTEF())
                        {
                            MessageBox.Show(TEF.mensagemGerenciadorInativo, "SICEpdv.net", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        if (TEF.Administrativo()==1)
                        {
                                if (ImprimirComprovantes(true))
                                {                                    
                                    TEF.EncerraTEF();
                                    TEF.Transacoes("limpar");
                                    break;
                                }
                                else
                                {
                                    NaoConfirmarTransacao(); 
                                    CancelarTransacao(); //? Falta
                                    TEF.EncerraTEF();
                                    TEF.Transacoes("limpar");
                                    break;
                                }                            
                        }
                        else
                        {
                            TEF.EncerraTEF();
                            TEF.Transacoes("limpar");
                            FindWindow(null, "SICEpdv.exe"); 
                            frmOperadorTEF formOperador = new frmOperadorTEF("!!"+TEF.mensagemOperador,true);
                            formOperador.Focus();
                            formOperador.ShowDialog();
                            formOperador.Dispose();
                        }
                    }
                    catch (Exception erro)
                    {
                        TEF.EncerraTEF();
                        TEF.Transacoes("limpar");
                        FindWindow(null, "SICEpdv.exe"); 
                        frmOperadorTEF formOperador = new frmOperadorTEF("! "+erro.Message,true);
                        formOperador.ShowDialog();
                        formOperador.Dispose();
                    }
                    break;
            };
		}


        private bool ImprimirCancelamento()
        {
            int idECF = 0;

            if (ConfiguracoesECF.idNFC > 0)
            {
                idECF = 5;
            }
            else
            {
                idECF = ConfiguracoesECF.idECF;
            }

            Funcoes.TravarTeclado(true);
            frmImpressaoTEF impressao = new frmImpressaoTEF();
            switch (idECF)
            {
//#region Bematech
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    if (ConfiguracoesECF.idECF>0 && CupomFiscalAberto())
                    {
                        Venda.ApagarItensFormaPagamento("Itens");
                        CancelarCupomECF();
                    }
                    FuncoesECF.RelatorioGerencial("Fechar", "","",false);
                    var arquivosRespTEF = ArquivosRespTransacaoTEF();
                    for (int nEspelhos = 0; nEspelhos < arquivosRespTEF.Count(); nEspelhos++)
                    {
                        TEF.espelhoTEF.Clear();
                        TEF.ImprimeTEF(arquivosRespTEF[nEspelhos], "", false); // Aqui se obtem o List do EspelhoTEF 

                        Funcoes.TravarTeclado(true);
                        impressao.lblMensagem.Text = TEF.mensagemOperador;
                        impressao.Show();
                        Application.DoEvents();


                        for (int ncopia = 0; ncopia < 2; ncopia++)
                        {
                            if (ncopia == 1)
                            {
                                FuncoesECF.RelatorioGerencial("Imprimir", "\r\n\r\n\r\n\r\n\r\n");
                                impressao.lblMensagem.Text = "Retire a 1a. via do cliente ";
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(3000);
                            }

                            for (int i = 0; i < TEF.espelhoTEF.Count; )
                            {
                                try
                                {
                                    impressao.lblMensagem.Text = TEF.mensagemOperador;
                                    try
                                    {
                                        VerificaImpressoraLigada();
                                    }
                                    catch (Exception)
                                    {
                                        Funcoes.TravarTeclado(false);
                                        i = 0;
                                        throw new Exception("");
                                    }
                                    FuncoesECF.RelatorioGerencial("Imprimir", TEF.espelhoTEF[i]);
                                    i++;
                                }
                                catch
                                {
                                    Funcoes.TravarTeclado(false);
                                    if (MessageBox.Show("Impressora não responde. Tentar imprimir novamente ??", "SICEpdv.net", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                    {
                                        Application.DoEvents();
                                        i = 0;
                                        ncopia = 0;
                                        try
                                        {
                                            DetectarECF(ConfiguracoesECF.idECF);
                                            VerificaImpressoraLigada();
                                        }
                                        catch (Exception)
                                        {
                                            i = 0;
                                        };
                                        DetectarECF(ConfiguracoesECF.idECF);
                                        FuncoesECF.RelatorioGerencial("Fechar", "");
                                        Funcoes.TravarTeclado(true);
                                    }
                                    else
                                    {
                                        impressao.Dispose();
                                        Application.DoEvents();
                                        return false;
                                    }
                                }
                                finally
                                {
                                    Funcoes.TravarTeclado(false);
                                }
                            }
                        };
                        if (!ConfiguracoesECF.tefDedicado)
                        {
                            ConfirmarTransacao(arquivosRespTEF[nEspelhos]);
                            TEF.EncerraTEF();
                        };
                    };
                    // Confirma a impressão completa do cancelamento                    
                    FuncoesECF.RelatorioGerencial("Fechar", "");
                    Funcoes.TravarTeclado(false);
                    TEF.EncerraTEF();
                    impressao.Dispose();                 
                    // Apaga a transação
                    //File.Delete(@transacaoOrigem);
                    break;
            }

            return true;
        }        
        #endregion Fim Funcoes TEF

        public static string COONumeroCupomFiscal(int modeloECF) // COO Cupom Fiscal 
        {

            if (ConfiguracoesECF.idNFC > 0)
                return "000000";
            if (ConfiguracoesECF.pdv == false)
                return "000000";

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                       
                            try
                            {
                                string nCupom = Funcoes.SetLength(6);
                                BemaFI32.Bematech_FI_NumeroCupom(ref nCupom);
                                return nCupom;
                            }
                            catch (Exception erro)
                            {
                                throw erro;
                            }
                        
                    case 2:
                        
                            try
                            {
                                StringBuilder Str_Informacao = new StringBuilder(' ', 2220);
                                DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("26", Str_Informacao);
                                if (DARUMA_FW.iRetorno != 1)
                                    throw new Exception("Não foi possível obter o COO do Cupom Fiscal ");
                                return Convert.ToString(Convert.ToInt32(Str_Informacao.ToString().Substring(0, 6))).PadLeft(6, '0');

                            }
                            catch (Exception)
                            {
                                throw new Exception("Não foi possível obter o COO do Cupom Fiscal");
                            }
                        
                    case 3:
                        
                            try
                            {
                                string nCupomElgin = Funcoes.SetLength(6);
                                Elgin32.Elgin_NumeroCupom(ref nCupomElgin);
                                return nCupomElgin;
                            }
                            catch (Exception erro)
                            {
                                throw erro;
                            }
                       
                    case 4:
                        
                            try
                            {
                                string iniciarVariavel = new string(' ', 6);
                                StringBuilder nCupomSw = new StringBuilder(iniciarVariavel);
                                Sweda32.ECF_RetornaCOO(nCupomSw);
                                return nCupomSw.ToString();
                            }
                            catch (Exception erro)
                            {
                                throw erro;
                            }
                        
                }
            }
            else
            {
                try
                {
                    return ecfAcbr.NumCupom.ToString();
                }
                catch (Exception erro)
                {
                    throw erro;
                }
            }
            return "0000";
        }

        public static string CCFContadorCupomECF() //CCF contador cupom Fiscal
        {

            if (ConfiguracoesECF.idNFC > 0)
                return "000000";

            if (ConfiguracoesECF.pdv == false)
                return "000000";

            if (GlbVariaveis.glb_Acbr == false)
            {
                string numeroCupom = new string(' ', 6);
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        try
                        {
                            int iRetorno = BemaFI32.Bematech_FI_ContadorCupomFiscalMFD(ref numeroCupom);
                            if (iRetorno != 1)
                                return "";
                            else
                                return numeroCupom;
                        }
                        catch (Exception erro)
                        {
                            throw erro;
                        }
                    case 2:
                        StringBuilder Str_Informacao = new StringBuilder(' ', 2200);
                        DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("30", Str_Informacao);
                        return Str_Informacao.ToString().Substring(0, 6);
                    case 3:
                        try
                        {
                            Elgin32.Int_Retorno = Elgin32.Elgin_ContadorCupomFiscalMFD(ref numeroCupom);
                            if (Elgin32.Int_Retorno != 1)
                                return "";
                            else
                                return numeroCupom;
                        }
                        catch (Exception erro)
                        {
                            throw erro;
                        }
                    case 4:
                        try
                        {
                            string iniciarVariavel = new string(' ', 6);
                            StringBuilder ncontadorSw = new StringBuilder(iniciarVariavel);
                            Sweda32.ECF_ContadorCupomFiscalMFD(ncontadorSw);
                            return ncontadorSw.ToString();
                        }
                        catch (Exception erro)
                        {
                            throw erro;
                        }

                }
                return numeroCupom;
            }
            else
            {
                try
                {
                    return ecfAcbr.NumCCF.ToString();
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    return "0";
                }

            }
        }

        public static string NumeroECF(int modeloECF)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return "000";

            if (ConfiguracoesECF.pdv == false)
                return "000";

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (modeloECF)
                {
                    case 1:
                        try
                        {
                            
                                string sNumero = Funcoes.SetLength(4);
                                BemaFI32.Bematech_FI_NumeroCaixa(ref sNumero);
                                return sNumero.Substring(1, 3).Replace("\0", "");
                            

                        }
                        catch (Exception erro)
                        {
                            throw erro;
                        }
                    case 2:
                        
                            StringBuilder Str_Informacao = new StringBuilder(' ', 2200);
                            DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("107", Str_Informacao);
                            return Str_Informacao.ToString().Substring(0, 3);
                       


                    case 3:

                            try
                            {
                                string sNumeroElgin = Funcoes.SetLength(4);
                                Elgin32.Elgin_NumeroCaixa(ref sNumeroElgin);
                                return sNumeroElgin.Substring(1, 3);
                            }
                            catch (Exception erro)
                            {
                                throw erro;
                            }
                        


                    case 4:

                        
                            try
                            {
                                string nEcfSw = new string(' ', 4);
                                Sweda32.ECF_NumeroCaixa(nEcfSw);
                                return nEcfSw.Substring(1, 3).ToString();
                            }
                            catch (Exception erro)
                            {
                                throw erro;
                            }
                       



                }
            }
            else
            {
                string numeroECF = ecfAcbr.NumECF.ToString();

                if (numeroECF.Length > 3)
                    numeroECF = numeroECF.Substring(numeroECF.Length - 3, 3);

                return numeroECF;
            }

            return "";
        }

        public static bool DetectarECF(int modeloECF)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;

            if (!ConfiguracoesECF.pdv)
                return false;

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (modeloECF)
                {
                    case 1:
                        try
                        {
                            VerificaImpressoraLigada();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    case 2:
                        if (!File.Exists("DarumaFrameWork.dll"))
                            return false;
                        try
                        {
                            try
                            {
                                DARUMA_FW.iRetorno = DARUMA_FW.eDefinirProduto("ECF");
                                if (!EqualizarVelocidade(2))
                                {
                                    return false;
                                }                                
                                VerificaImpressoraLigada();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message);
                            }


                            // EqualizarVelocidade(2);
                            //MessageBox.Show("Retorno do metodo = " + Str_EqualizaVelocidade.ToString(), "Daruma", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //DARUMA32.Daruma_Mostrar_Retorno();
                            if (DARUMA_FW.iRetorno == 1)
                                return true;
                            else
                                return false;
                        }
                        catch
                        {
                            return false;
                            //throw new Exception("Não foi possível encontrar o ECF !");
                        }

                    case 3:
                        try
                        {
                            VerificaImpressoraLigada();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    case 4:
                        try
                        {
                            VerificaImpressoraLigada();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }

                }

                return false;
            }
            else
            {
                VerificaImpressoraLigada();
                return true;
            }
        }

        public static bool EqualizarVelocidade(int modeloECF)
        {

            if (ConfiguracoesECF.idNFC > 0)
                return true;

            if (ConfiguracoesECF.pdv == false)
                return true;

            switch (modeloECF)
            {
              case  1 :
                    return true;                    
                case 2:
                 //StringBuilder Str_EqualizaVelocidade = new StringBuilder(6);
                 //DARUMA32.Int_Retorno = DARUMA32.Daruma_FIMFD_EqualizarVelocidade(Str_EqualizaVelocidade);
                 DARUMA_FW.iRetorno = DARUMA_FW.eBuscarPortaVelocidade_ECF_Daruma();
                 if (DARUMA_FW.iRetorno == 1)
                     return true;
                 else                     
                     return false;
            }
            return true;            
        }

        public static string ContadorNaoFiscalGNF()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return "000000";

            if (ConfiguracoesECF.pdv == false)
                return "000000";

            if (GlbVariaveis.glb_Acbr == false)
            {

                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        
                            string sContador = Funcoes.SetLength(6);
                            BemaFI32.Bematech_FI_NumeroOperacoesNaoFiscais(ref sContador);
                            return sContador;
                        

                    case 2:
                        
                            StringBuilder Str_Informacao = new StringBuilder(' ', 2200);
                            DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("28", Str_Informacao);
                            if (DARUMA_FW.iRetorno != 1)
                                throw new Exception("Não foi possível obter o GNF");
                            return Str_Informacao.ToString().Substring(0, 6);
                        
                    case 3:
                        
                            string sContadorElgin = Funcoes.SetLength(6);
                            Elgin32.Elgin_NumeroOperacoesNaoFiscais(ref sContadorElgin);
                            return sContadorElgin;
                        
                    case 4:
                        
                            StringBuilder sContadorSW = new StringBuilder(new string(' ', 6));
                            Sweda32.ECF_NumeroOperacoesNaoFiscais(sContadorSW);
                            return sContadorSW.ToString();
                       
                }
            }
            else
            {
                return ecfAcbr.NumGNF;
            }
            return "";
        }

        public static string  ContadorRelatorioGerencial()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return "000000";

            if (ConfiguracoesECF.pdv == false)
                return "000000";

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:

                        string sContador = Funcoes.SetLength(6);
                        BemaFI32.Bematech_FI_ContadorRelatoriosGerenciaisMFD(sContador);
                        return sContador;

                    case 2:
                        StringBuilder Str_Informacao = new StringBuilder(' ', 2200);
                        DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("33", Str_Informacao);
                        if (DARUMA_FW.iRetorno != 1)
                            throw new Exception("Não foi possível obter o ContadorRelatorioGerencial Geral não Fiscal");
                        return Str_Informacao.ToString().Substring(0, 6);
                    case 3:
                        string sContadorElgin = Funcoes.SetLength(6);
                        Elgin32.Elgin_ContadorRelatoriosGerenciaisMFD(ref sContadorElgin);
                        return sContadorElgin;
                    case 4:
                        break;
                }
            }
            else
            {
                return ecfAcbr.NumGNF;
            }
            
            return "";
        }

        public static List<string> RetornarAliquotasECF(string tipo )
        {

            if (ConfiguracoesECF.idNFC > 0)
                return null;


            if (tipo == "ICMS")
            {

                if (GlbVariaveis.glb_Acbr == false)
                {
                    switch (ConfiguracoesECF.idECF)
                    {
                        case 1:
                            string sAliquotas = Funcoes.SetLength(79);
                            BemaFI32.Bematech_FI_RetornoAliquotas(ref sAliquotas);
                            var arrayAliquotas = sAliquotas.Split(",".ToCharArray());
                            List<string> aliquotas = new List<string>(arrayAliquotas.ToList());
                            return aliquotas;
                        case 2:
                            StringBuilder Str_Informacao = new StringBuilder(300);
                            DARUMA_FW.iRetorno = DARUMA_FW.rLerAliquotas_ECF_Daruma(Str_Informacao);
                            var arrayAliquotas2 = Str_Informacao.ToString().Split(",".ToCharArray());
                            List<string> aliquotas2 = new List<string>(arrayAliquotas2.ToList());
                            var aliquotaICMS = (from n in aliquotas2
                                                where n.StartsWith("T")
                                                select n).ToList();
                            return aliquotaICMS.ToList();
                        case 3:
                            
                                string sAliquotasElgin = Funcoes.SetLength(79);
                                Elgin32.Elgin_RetornoAliquotas(ref sAliquotasElgin);
                                var arrayAliquotas3 = sAliquotasElgin.Split(",".ToCharArray());
                                List<string> aliquotas3 = new List<string>(arrayAliquotas3.ToList());
                                return aliquotas3;
                    }
                }
                else
                {
                    ecfAcbr.CarregaAliquotas();
                    var aliquota = ecfAcbr.Aliquotas.ToList();

                    List<string> aliquotas3 = new List<string>();

                    foreach (var aliq in aliquota)
                    {
                        if (aliq.Tipo == "T")
                            aliquotas3.Add(aliq.ValorAliquota.ToString());

                    }

                    return aliquotas3;
                }
            };

            if (tipo == "ISS")
            {
                if (GlbVariaveis.glb_Acbr == false)
                {
                    switch (ConfiguracoesECF.idECF)
                    {
                        case 1:
                            string sAliquotas = Funcoes.SetLength(79);
                            BemaFI32.Bematech_FI_VerificaAliquotasIss(ref sAliquotas);
                            var arrayAliquotas = sAliquotas.Split(",".ToCharArray());
                            List<string> aliquotas = new List<string>(arrayAliquotas.ToList());
                            return aliquotas;
                        case 2:
                            StringBuilder Str_Informacao = new StringBuilder(300);
                            DARUMA_FW.iRetorno = DARUMA_FW.rLerAliquotas_ECF_Daruma(Str_Informacao);
                            var arrayAliquotas2 = Str_Informacao.ToString().Split(",".ToCharArray());
                            List<string> aliquotas2 = new List<string>(arrayAliquotas2.ToList());

                            var aliquotaISS = (from n in aliquotas2
                                               where n.StartsWith("S")
                                               select n).ToList();
                            return aliquotaISS.ToList();

                        case 3:
                            string sAliquotasISSElgin = Funcoes.SetLength(79);
                            Elgin32.Elgin_VerificaAliquotasIss(ref sAliquotasISSElgin);
                            var arrayAliquotas3 = sAliquotasISSElgin.Split(",".ToCharArray());
                            List<string> aliquotas3 = new List<string>(arrayAliquotas3.ToList());
                            return aliquotas3;
                    }
                }
                else
                {
                    ecfAcbr.CarregaAliquotas();
                    var aliquota = ecfAcbr.Aliquotas.ToList();

                    List<string> aliquotas3 = new List<string>();

                    foreach (var aliq in aliquota)
                    {
                        if (aliq.Tipo == "S")
                            aliquotas3.Add(aliq.ValorAliquota.ToString());

                    }

                    return aliquotas3;
                }
            }
            return null;
        }

        public static string ContadorDebitoCredidoCDC()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return "0000";

            if (ConfiguracoesECF.pdv == false)
                return "0000";

            string sContador = Funcoes.SetLength(4);
            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:

                        BemaFI32.Bematech_FI_ContadorComprovantesCreditoMFD(sContador);
                        // return sContador;
                        break;

                    case 2:
                        StringBuilder contadorCDC = new StringBuilder(' ', 2200);
                        DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("45", contadorCDC);
                        sContador = contadorCDC.ToString().Substring(0, 4);
                        if (DARUMA_FW.iRetorno != 1)
                            throw new Exception("Não foi possível obter o CDC.");
                        break;
                    case 3:
                        Elgin32.Elgin_ContadorComprovantesCreditoMFD(ref sContador);
                        break;
                }
            }
            else
            {
                try
                {
                    if (ecfAcbr.NumCDC.Length == 5)
                        return ecfAcbr.NumCDC.Substring(1, 4);
                    else if (ecfAcbr.NumCDC.Length == 6)
                        return ecfAcbr.NumCDC.Substring(2, 4);
                    else
                        return ecfAcbr.NumCDC;
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    return "0000";

                }
            }

            
            // Para gravar o contador de debito e credito por que é tirado depois do 
            // fechamento do cupom. Questäo do PAF-ECF.
           
            
            return sContador;          
        }

        public static string DataHoraUltDocumentoECF()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return string.Format("{0:dd/MM/yyyy hh:mm:ss}", DateTime.Now.ToString());

            if (ConfiguracoesECF.pdv == false)
                return string.Format("{0:dd/MM/yyyy hh:mm:ss}", DateTime.Now.ToString()); ;

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                       
                            string cDataHora = new string(' ', 12);
                            int iRetorno = BemaFI32.Bematech_FI_DataHoraUltimoDocumentoMFD(cDataHora);
                            if (cDataHora.Trim() == "")
                                return DateTime.Now.ToString();
                            else
                                return string.Format("{0:dd/MM/yyyy hh:mm:ss}", cDataHora.ToString());
                       
                    case 2:
                        

                            StringBuilder Str_Informacao = new StringBuilder(' ', 2200);
                            DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("73", Str_Informacao);
                            string dataHora = Str_Informacao.ToString().Substring(0, 14);
                            return Convert.ToDateTime(dataHora.Substring(0, 2) + "/" + dataHora.Substring(2, 2) + "/" + dataHora.Substring(4, 4) + " " +
                           dataHora.Substring(8, 2) + ":" + dataHora.Substring(10, 2) + ":" + dataHora.Substring(12, 2)).ToString();
                       
                    case 3:

                        
                            string cDataHoraElgin = new string(' ', 12);



                            Elgin32.Int_Retorno = Elgin32.Elgin_DataHoraUltimoDocumentoMFD(ref cDataHoraElgin);
                            if (cDataHoraElgin.Trim() == "")
                            {
                                return DateTime.Now.ToString();
                            }
                            else
                            {
                                // Alteracao Feita Por Marckvlado no dia 11/10/2012
                                return FormatDataElgin(cDataHoraElgin).ToString();
                                // return string.Format("{0:dd/MM/yyyy hh:mm:ss}", cDataHoraElgin.ToString());
                            }
                        


                    case 4:
                       

                            return string.Format("{0:dd/MM/yyyy hh:mm:ss}", DateTime.Now);
                        
                }
            }
            else
            {
                // Ivan 26.02.2016 - Mudei o parametro de datamovimento para datahora.
                string data = string.Format("{0:dd/MM/yyyy hh:mm:ss}", ecfAcbr.DataHora.ToString());

                /*
                 * quanto não á suprimento na ecf DARUMA a mesma retorna a data Movimento com o valor de "0" do tipo inteiro quando o framework do acbr converte para tipo date retorna 
                 * a data 30/12/1988 isso está acontecendo na simoes rocha marckvaldo 14-04-2015 versão 3.1
                 * */


                // 26.02.2016 Ivan retirado por que o formate deve retonar data e hora por que os dados de hora na abertura do caixa vai 00:00:00 e as 
                // informaceos devem ir no PAF
                /*
                if (data.Length > 9)
                {
                    data = data.Substring(0, 10);
                }
                 */


                    if (data == "30/12/1899" || data == "0")
                    {
                        return DateTime.Now.Date.ToShortDateString();
                    }
                    else
                    {
                        return data;
                    }
                
            }
            return DateTime.Now.Date.ToShortDateString();      
        }

        /*Foi Necessaria A  criacao desta Function por que ela e Chamado pelas funcoes DataHoraUltDocumentoECF, DataHoraGravacaoUsuarioMF 
         * Alteracao Feita Por Marckvaldo no dia 11/10/2012
         */
        public static string FormatDataElgin(string cDataHoraElgin)
        {
            string dia = cDataHoraElgin.Substring(0, 2);
            string mes = cDataHoraElgin.Substring(2, 2);
            string ano = "20" + cDataHoraElgin.Substring(4, 2);
            string horas = cDataHoraElgin.Substring(6, 2);
            string minutos = cDataHoraElgin.Substring(8, 2);
            string segundos = cDataHoraElgin.Substring(10, 2);

            return cDataHoraElgin = dia + "/" + mes + "/" + ano + " " + horas + ":" + minutos + ":" + segundos;

        }

        public static string DataMovimentoECF()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return GlbVariaveis.Sys_Data.Date.ToString(); 

            if (ConfiguracoesECF.pdv == false)
                return GlbVariaveis.Sys_Data.Date.ToString(); 

            string cDataMovimento = Funcoes.SetLength(6);
            switch (ConfiguracoesECF.idECF)
            {
                case 1:                                                
                        BemaFI32.Bematech_FI_DataMovimento(ref cDataMovimento);
                        if (cDataMovimento.Trim() == "" || cDataMovimento=="000000" || cDataMovimento==null)
                            return GlbVariaveis.Sys_Data.Date.ToString();
                        else
                            return cDataMovimento.Insert(2, "/").Insert(5, "/20");

                case 2:
                    
                        StringBuilder data = new StringBuilder(2200);
                        DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("70", data);
                        return data.ToString().Substring(0, 2) + "/" + data.ToString().Substring(2, 2) + "/" + data.ToString().Substring(4, 4);
                 
                case 3:                        
                    Elgin32.Elgin_DataMovimento(ref cDataMovimento);
                        if (cDataMovimento.Trim() == "")
                            return GlbVariaveis.Sys_Data.Date.ToString();
                        else                            
                            return cDataMovimento.Insert(2,"/").Insert(5, "/20");
                case 4:
                    StringBuilder dataSW = new StringBuilder(new string(' ',6));
                    Sweda32.ECF_DataMovimento(dataSW);
                    if (dataSW.ToString().Trim() == "")
                        return GlbVariaveis.Sys_Data.Date.ToString();
                    else
                        return dataSW.ToString().Insert(2, "/").Insert(5, "/20");
            }
            return null;     
        }

        public static int GerarSintegraECF(int mes,int ano,DateTime dataInicial,DateTime dataFinal)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return 1;


                        siceEntities entidade = Conexao.CriarEntidade();                                
                        var dados = (from n in entidade.filiais
                                    where n.CodigoFilial == GlbVariaveis.glb_filial
                                    select new
                                    {
                                        rSocial = n.empresa,
                                        endereco = n.endereco,
                                        numero = n.numero,
                                        complemento = n.complemento == null ? "":n.complemento,
                                        bairro = n.bairro,
                                        cidade = n.cidade,
                                        cep = n.cep,
                                        telefone = n.telefone1,
                                        fax = n.telefone2,
                                        contato = n.nomeCobranca
                                    }).First();

            switch (ConfiguracoesECF.idECF)
            {
                case 1:
                    System.Diagnostics.Process.Start("registra.bat", "");
                    System.Threading.Thread.Sleep(2000);
                    int iRetorno = 0;

                     // EspelhoMFD(ConfiguracoesECF.pathRetornoECF+"download.mfd", ConfiguracoesECF.pathRetornoECF+"retorno.txt", "1", dataInicial.Date, dataFinal.Date,0,0);
                    //iRetorno = BemaFI32.BemaGeraRegistrosTipoE(@ConfiguracoesECF.pathRetornoECF + "download.mfd", @ConfiguracoesECF.pathRetornoECF + "sintegra.txt", String.Format("{0:ddMMyyyy}", dataInicial.Date), String.Format("{0:ddMMyyyy}", dataFinal.Date), dados.rSocial, dados.endereco, null, "2", null, null, null, null, null, null, null, null, null, null, null, null, null);
                    
                       
                        iRetorno = BemaFI32.Bematech_FI_RelatorioSintegraMFD(63,@ConfiguracoesECF.pathRetornoECF+"SINTEGRA.TXT",mes.ToString(),ano.ToString(), dados.rSocial, dados.endereco,
                        dados.numero, dados.complemento, dados.bairro, dados.cidade, dados.cep, dados.telefone, dados.fax, dados.contato);                                                                              

                        //iRetorno = BemaFI32.Bematech_FI_RelatorioSintegraMFD(63, @"C:\SINTEGRA.TXT", "02", "2010", "BEMATECH S/A", "Estrada de Santa Candida", "263", "Industria", "Santa Candida", "Curitiba", "82630490", "41 351-2700", "41 351-2863", "Fulano de Tal");
                        return iRetorno;                        

                case 2:
                    //DARUMA32.Int_Retorno = DARUMA32.Daruma_Sintegra_GerarRegistrosArq("010810", "050810", "ARCOVERDE", "(87)38212715", "3", "3", "1", "RUA OITO", "03", "1 andar", "CENTRO", "56512560", "MAX CONSTANTIN", "(87)38212715");
                   // DARUMA32.Daruma_Registry_SintegraPath(@ConfiguracoesECF.pathRetornoECF);                    
                    System.Threading.Thread.Sleep(300);

                    DARUMA_FW.iRetorno = DARUMA_FW.rGerarRelatorio_ECF_Daruma("SINTEGRA","DATAM", String.Format("{0:ddMMyyyy}",dataInicial.Date) ,String.Format("{0:ddMMyyyy}",dataFinal.Date));

                    //if (DARUMA_FW.iRetorno==105)
                    //    DARUMA_FW.iRetorno = DARUMA_FW.rGerarRelatorio_ECF_Daruma("SINTEGRA", "DATAM", String.Format("{0:ddMMyyyy}", dataInicial.Date.AddDays(-1)), String.Format("{0:ddMMyyyy}", dataFinal.Date.AddDays(-1)));


                    //DARUMA32.Int_Retorno = DARUMA32.Daruma_Sintegra_GerarRegistrosArq(String.Format("{0:ddMMyy}",dataInicial.Date) ,String.Format("{0:ddMMyy}",dataFinal.Date),dados.cidade,dados.fax,"3","3","1",dados.endereco,
                    //dados.numero,dados.complemento,dados.bairro,dados.cep,dados.contato,dados.telefone);                    
                    return DARUMA_FW.iRetorno;
                case 3:                     
                    System.Threading.Thread.Sleep(2000);                                           
                      // iRetorno = BemaFI32.Bematech_FI_RelatorioSintegraMFD(63,@"C:\SINTEGRA.TXT",mes.ToString(),ano.ToString(), dados.rSocial, dados.endereco,
                      //  dados.numero, dados.complemento, dados.bairro, dados.cidade, dados.cep, dados.telefone, dados.fax, dados.contato);                                                                              
                         Elgin32.Int_Retorno = Elgin32.Elgin_RelatorioSintegraMFD(0,@"c:\sintegra.txt",dataInicial.Month.ToString(),dataInicial.Year.ToString(), dados.rSocial,dados.endereco, "","2", "", "", "", "", "", "");
                        //iRetorno = BemaFI32.Bematech_FI_RelatorioSintegraMFD(63, @"C:\SINTEGRA.TXT", "02", "2010", "BEMATECH S/A", "Estrada de Santa Candida", "263", "Industria", "Santa Candida", "Curitiba", "82630490", "41 351-2700", "41 351-2863", "Fulano de Tal");
                         return Elgin32.Int_Retorno;  
           }
            return 0;
        }

        public static int GerarAtoCotepe1704(string tipo,DateTime dataInicial, DateTime dataFinal,int cooInicial,int cooFinal)
        {

            if (ConfiguracoesECF.idNFC > 0)
                return 1; 

            //Tipo 1 = Data 2 = COO
            siceEntities entidade = Conexao.CriarEntidade();
            var dados = (from n in entidade.filiais
                         where n.CodigoFilial == GlbVariaveis.glb_filial
                         select new
                         {
                             rSocial = n.empresa,
                             endereco = n.endereco,
                             numero = n.numero,
                             complemento = n.complemento,
                             bairro = n.bairro,
                             cidade = n.cidade,
                             cep = n.cep,
                             telefone = n.telefone1,
                             fax = n.telefone2,
                             contato = n.nomeCobranca
                         }).First();

            if (GlbVariaveis.glb_Acbr == false)
            {
                #region
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        System.Diagnostics.Process.Start("registra.bat", "");
                        System.Threading.Thread.Sleep(2000);
                        EspelhoMFD(ConfiguracoesECF.pathRetornoECF + "download.mfd", ConfiguracoesECF.pathRetornoECF + "retorno.txt", tipo, dataInicial, dataFinal, cooInicial, cooFinal);
                        System.Threading.Thread.Sleep(200);
                        int iRetorno = 0;

                        // iRetorno = BemaFI32.Bematech_FI_RelatorioSintegraMFD(63,@"C:\SINTEGRA.TXT",mes.ToString(),ano.ToString(), dados.rSocial, dados.endereco,
                        //  dados.numero, dados.complemento, dados.bairro, dados.cidade, dados.cep, dados.telefone, dados.fax, dados.contato);                                                                              
                        iRetorno = BemaFI32.BemaGeraRegistrosTipoE(ConfiguracoesECF.pathRetornoECF + "download.mfd", ConfiguracoesECF.pathRetornoECF + "sintegra.txt", String.Format("{0:ddMMyyyy}", dataInicial.Date), String.Format("{0:ddMMyyyy}", dataFinal.Date), dados.rSocial, dados.endereco, "", "2", "", "", "", "", "", "", "", "", "", "", "", "", "");

                        //iRetorno = BemaFI32.Bematech_FI_RelatorioSintegraMFD(63, @"C:\SINTEGRA.TXT", "02", "2010", "BEMATECH S/A", "Estrada de Santa Candida", "263", "Industria", "Santa Candida", "Curitiba", "82630490", "41 351-2700", "41 351-2863", "Fulano de Tal");
                        return iRetorno;

                    case 2:

                        if (tipo == "1")
                        {
                            // DARUMA32.Int_Retorno = DARUMA32.Daruma_FIMFD_GerarMFPAF_Data(String.Format("{0:ddMMyyyy}", dataInicial), String.Format("{0:ddMMyyyy}", dataFinal));
                            DARUMA_FW.iRetorno = DARUMA_FW.rGerarRelatorio_ECF_Daruma("MFD", "DATAM", String.Format("{0:ddMMyyyy}", dataInicial), String.Format("{0:ddMMyyyy}", dataFinal));

                        }
                        if (tipo == "2")
                        {
                            // DARUMA32.Int_Retorno = DARUMA32.Daruma_FIMFD_GerarMFPAF_CRZ(crzInicial.ToString(), crzFinal.ToString());
                            DARUMA_FW.iRetorno = DARUMA_FW.rGerarRelatorio_ECF_Daruma("MFD", "COO", cooInicial.ToString(), cooFinal.ToString());
                        }


                        if (DARUMA_FW.iRetorno != 1)
                        {

                            if (DARUMA_FW.iRetorno == -109)
                                throw new Exception("-109: Data final é maior que a data do movimento da última Redução Z emitida.");
                            if (DARUMA_FW.iRetorno == -110)
                                throw new Exception("-110: Período sem movimento");
                            if (DARUMA_FW.iRetorno == -108)
                                throw new Exception("-108: Data inicial e data final é menor que o 1º documento emitido.");

                            if (DARUMA_FW.iRetorno == -100)
                                throw new Exception("-100 Arquivo .MFD Corrompido.");

                            if (DARUMA_FW.iRetorno == -2)
                                throw new Exception("-2 Parâmetro Inválido. Ex.: Data Inicial maior que Data Final.");

                            throw new Exception(DARUMA_FW.iRetorno.ToString());
                        }

                        return DARUMA_FW.iRetorno;
                }
                #endregion
            }
            else
            {
                try
                {
                    if (tipo == "1")
                        ecfAcbr.PafMF_MFD_Cotepe1704(dataInicial, dataFinal, Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\download.txt");
                    else
                        ecfAcbr.PafMF_MFD_Cotepe1704(cooInicial, cooFinal, Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\download.txt");


                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }
            }
            return 0;
        }

        public static int GerarAtoCotepe1704LMFC(string tipo, DateTime dataInicial, DateTime dataFinal, int crzInicial, int crzFinal)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return 1;

            //Tipo 1 = Data 2 = COO
            siceEntities entidade = Conexao.CriarEntidade();
            var dados = (from n in entidade.filiais
                         where n.CodigoFilial == GlbVariaveis.glb_filial
                         select new
                         {
                             rSocial = n.empresa,
                             endereco = n.endereco,
                             numero = n.numero,
                             complemento = n.complemento,
                             bairro = n.bairro,
                             cidade = n.cidade,
                             cep = n.cep,
                             telefone = n.telefone1,
                             fax = n.telefone2,
                             contato = n.nomeCobranca
                         }).First();

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        System.Diagnostics.Process.Start("registra.bat", "");
                        System.Threading.Thread.Sleep(2000);
                        EspelhoMFD(ConfiguracoesECF.pathRetornoECF + "download.mfd", ConfiguracoesECF.pathRetornoECF + "retorno.txt", tipo, dataInicial, dataFinal, crzInicial, crzFinal);
                        System.Threading.Thread.Sleep(200);
                        int iRetorno = 0;

                        // iRetorno = BemaFI32.Bematech_FI_RelatorioSintegraMFD(63,@"C:\SINTEGRA.TXT",mes.ToString(),ano.ToString(), dados.rSocial, dados.endereco,
                        //  dados.numero, dados.complemento, dados.bairro, dados.cidade, dados.cep, dados.telefone, dados.fax, dados.contato);                                                                              
                        iRetorno = BemaFI32.BemaGeraRegistrosTipoE(ConfiguracoesECF.pathRetornoECF + "download.mfd", ConfiguracoesECF.pathRetornoECF + "sintegra.txt", String.Format("{0:ddMMyyyy}", dataInicial.Date), String.Format("{0:ddMMyyyy}", dataFinal.Date), dados.rSocial, dados.endereco, "", "2", "", "", "", "", "", "", "", "", "", "", "", "", "");

                        //iRetorno = BemaFI32.Bematech_FI_RelatorioSintegraMFD(63, @"C:\SINTEGRA.TXT", "02", "2010", "BEMATECH S/A", "Estrada de Santa Candida", "263", "Industria", "Santa Candida", "Curitiba", "82630490", "41 351-2700", "41 351-2863", "Fulano de Tal");
                        return iRetorno;

                    case 2:

                        if (tipo == "1")
                        {
                            // DARUMA32.Int_Retorno = DARUMA32.Daruma_FIMFD_GerarMFPAF_Data(String.Format("{0:ddMMyyyy}", dataInicial), String.Format("{0:ddMMyyyy}", dataFinal));
                            DARUMA_FW.iRetorno = DARUMA_FW.rGerarRelatorio_ECF_Daruma("MFD", "DATAM", String.Format("{0:ddMMyyyy}", dataInicial), String.Format("{0:ddMMyyyy}", dataFinal));

                        }
                        if (tipo == "2")
                        {
                            // DARUMA32.Int_Retorno = DARUMA32.Daruma_FIMFD_GerarMFPAF_CRZ(crzInicial.ToString(), crzFinal.ToString());
                            DARUMA_FW.iRetorno = DARUMA_FW.rGerarRelatorio_ECF_Daruma("MFD", "COO", crzInicial.ToString(), crzFinal.ToString());
                        }


                        if (DARUMA_FW.iRetorno != 1)
                        {

                            if (DARUMA_FW.iRetorno == -109)
                                throw new Exception("-109: Data final é maior que a data do movimento da última Redução Z emitida.");
                            if (DARUMA_FW.iRetorno == -110)
                                throw new Exception("-110: Período sem movimento");
                            if (DARUMA_FW.iRetorno == -108)
                                throw new Exception("-108: Data inicial e data final é menor que o 1º documento emitido.");

                            if (DARUMA_FW.iRetorno == -100)
                                throw new Exception("-100 Arquivo .MFD Corrompido.");

                            if (DARUMA_FW.iRetorno == -2)
                                throw new Exception("-2 Parâmetro Inválido. Ex.: Data Inicial maior que Data Final.");

                            throw new Exception(DARUMA_FW.iRetorno.ToString());
                        }

                        return DARUMA_FW.iRetorno;
                }
            }
            else
            {
                /*if (tipo == "1")
                    ecfAcbr.ArquivoMFD_DLL(dataInicial, dataFinal, ConfiguracoesECF.pathRetornoECF + "download.mf", ACBrFramework.ECF.FinalizaArqMFD.MF);
                else
                    ecfAcbr.ArquivoMFD_DLL(crzInicial, crzFinal, ConfiguracoesECF.pathRetornoECF + "download.mf", ACBrFramework.ECF.FinalizaArqMFD.MF, ACBrFramework.ECF.TipoContador.COO, ACBrFramework.ECF.TipoDocumento);
                */
                
                ecfAcbr.PafMF_LMFC_Cotepe1704(dataInicial, dataFinal, ConfiguracoesECF.pathRetornoECF + "download.mf");
            }
            return 0;
        }

        public static int GerarArqMF(string tipo, DateTime dataInicial, DateTime dataFinal, int crzInicial, int crzFinal)
        {

            if (ConfiguracoesECF.idNFC > 0)
                return 1;

            //Tipo 1 = Data 2 = COO
            siceEntities entidade = Conexao.CriarEntidade();
            var dados = (from n in entidade.filiais
                         where n.CodigoFilial == GlbVariaveis.glb_filial
                         select new
                         {
                             rSocial = n.empresa,
                             endereco = n.endereco,
                             numero = n.numero,
                             complemento = n.complemento,
                             bairro = n.bairro,
                             cidade = n.cidade,
                             cep = n.cep,
                             telefone = n.telefone1,
                             fax = n.telefone2,
                             contato = n.nomeCobranca
                         }).First();

            if (GlbVariaveis.glb_Acbr == false)
            {
                #region
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        System.Diagnostics.Process.Start("registra.bat", "");
                        System.Threading.Thread.Sleep(2000);

                        System.Threading.Thread.Sleep(200);
                        int iRetorno = 0;

                        //iRetorno = BemaFI32.Bematech_FI_DownloadMFD("download.mf");
                        if (tipo == "1")
                            tipo = "D";
                        if (tipo == "2")
                            tipo = "C";

                        if (tipo == "D")
                            iRetorno = BemaFI32.Bematech_FI_FormatoDadosMFD("download.mf", "arquivoMF.txt", "0", "C", tipo, String.Format("{0:ddMMyyyy}", dataInicial.Date), String.Format("{0:ddMMyyyy}", dataFinal.Date));
                        if (tipo == "C")
                            iRetorno = BemaFI32.Bematech_FI_FormatoDadosMFD("download.mf", "arquivoMF.txt", "0", "C", tipo, crzInicial.ToString(), crzFinal.ToString());

                        //iRetorno = BemaFI32.Bematech_FI_RelatorioSintegraMFD(63, @"C:\SINTEGRA.TXT", "02", "2010", "BEMATECH S/A", "Estrada de Santa Candida", "263", "Industria", "Santa Candida", "Curitiba", "82630490", "41 351-2700", "41 351-2863", "Fulano de Tal");
                        return iRetorno;

                    case 2:

                        if (tipo == "1")
                        {
                            // DARUMA32.Int_Retorno = DARUMA32.Daruma_FIMFD_GerarMFPAF_Data(String.Format("{0:ddMMyyyy}", dataInicial), String.Format("{0:ddMMyyyy}", dataFinal));
                            DARUMA_FW.iRetorno = DARUMA_FW.rGerarRelatorio_ECF_Daruma("MF", "DATAM", String.Format("{0:ddMMyyyy}", dataInicial), String.Format("{0:ddMMyyyy}", dataFinal));

                        }
                        if (tipo == "2")
                        {
                            // DARUMA32.Int_Retorno = DARUMA32.Daruma_FIMFD_GerarMFPAF_CRZ(crzInicial.ToString(), crzFinal.ToString());
                            DARUMA_FW.iRetorno = DARUMA_FW.rGerarRelatorio_ECF_Daruma("MF", "COO", crzInicial.ToString(), crzFinal.ToString());
                        }


                        if (DARUMA_FW.iRetorno != 1)
                        {

                            if (DARUMA_FW.iRetorno == -109)
                                throw new Exception("-109: Data final é maior que a data do movimento da última Redução Z emitida.");
                            if (DARUMA_FW.iRetorno == -110)
                                throw new Exception("-110: Período sem movimento");
                            if (DARUMA_FW.iRetorno == -108)
                                throw new Exception("-108: Data inicial e data final é menor que o 1º documento emitido.");

                            if (DARUMA_FW.iRetorno == -100)
                                throw new Exception("-100 Arquivo .MFD Corrompido.");

                            if (DARUMA_FW.iRetorno == -2)
                                throw new Exception("-2 Parâmetro Inválido. Ex.: Data Inicial maior que Data Final.");

                            throw new Exception(DARUMA_FW.iRetorno.ToString());
                        }

                        return DARUMA_FW.iRetorno;
                }

                #endregion 
            }
            else
            {
                try
                {
                    /*
                    //MessageBox.Show("1");
                    if (tipo == "1")
                        ecfAcbr.ArquivoMFD_DLL(dataInicial, dataFinal, Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\download.bin", ACBrFramework.ECF.FinalizaArqMFD.MF);
                    else
                        ecfAcbr.ArquivoMFD_DLL(crzInicial, crzFinal, Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\download.bin", ACBrFramework.ECF.FinalizaArqMFD.MF, ACBrFramework.ECF.TipoContador.COO, ACBrFramework.ECF.TipoDocumento.Todos);
                    */
                    
                    //ecfAcbr.PafMF_ArqMF(Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\download.bin");
                    ecfAcbr.PafMF_ArqMF(Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\Daruma.mf");
                    //ecfAcbr.ArquivoMFD_DLL(crzInicial, crzFinal, Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\download.bin", ACBrFramework.ECF.FinalizaArqMFD.MF, ACBrFramework.ECF.TipoContador.COO, ACBrFramework.ECF.TipoDocumento.Todos);
                    

                    /*if (File.Exists(ConfiguracoesECF.pathRetornoECF + "download.mf"))
                    {
                        File.Copy(@ConfiguracoesECF.pathRetornoECF + "download.mf", Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\arquivoMF.mf", true);
                    }*/
                    
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    return 0;
                }

               
            }
            
            return 0;
        }

        public static int GerarArqMFD()
        {

            if (ConfiguracoesECF.idNFC > 0)
                return 1;

            ecfAcbr.PafMF_ArqMFD(Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"] + @"\download.bin");

            return 0;
        }



        public static decimal TotalICMSCupomECF()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return 0;

            if (ConfiguracoesECF.pdv == false)
                return 0;

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:

                        string cTotal = Funcoes.SetLength(14);
                        BemaFI32.Bematech_FI_TotalIcmsCupom(ref cTotal);
                        return Convert.ToDecimal(cTotal + "0") / 100;

                    case 2:
                        StringBuilder str_ICMS = new StringBuilder(12);
                        StringBuilder str_ISS = new StringBuilder(12);
                        DARUMA_FW.iRetorno = DARUMA_FW.rCMEfetuarCalculo_ECF_Daruma(str_ISS, str_ICMS);
                        return Convert.ToDecimal(str_ICMS.ToString().Substring(0, 12)) / 100;
                }
            }
            else
            {
                return 0;
            }
            return 0;                 
        }

        /// <summary>
        /// Descrição dos relatórios gerenciais padrão
       
        /// 06 DAV. EMITIDOS
        /// 07 ID. PAF-ECF
        /// 08 MEIOS PGT
        /// 09 INDICE PRD
        /// 10 PAR. CONFIG
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="descricao"></param>
        /// <returns></returns>
        public static bool GravarRelatorioGerencial(string indice, string descricao)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;



            switch (ConfiguracoesECF.idECF)
            {
                case 1:
                    int iRetorno = BemaFI32.Bematech_FI_NomeiaRelatorioGerencialMFD(indice, descricao); 
                         if (iRetorno != 1)
                           return false;
                    break;
                case 2:
                    DARUMA_FW.iRetorno = DARUMA_FW.confCadastrar_ECF_Daruma("RG",descricao,"|");
                        if (DARUMA_FW.iRetorno != 1)
                           return false;
                    break;
                case 3:
                    Elgin32.Int_Retorno = Elgin32.Elgin_NomeiaRelatorioGerencialMFD(indice,descricao);
                     break;
                case 4:
                    Sweda32.iRetorno = Sweda32.ECF_NomeiaRelatorioGerencialMFD(indice,descricao);
                    break;

            }
            return true;
        }

        public static bool GravarComprovantesNaoFiscais(int indice, string descricao)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;


            switch (ConfiguracoesECF.idECF)
            {
                case 1:
                    int iRetorno = BemaFI32.Bematech_FI_NomeiaTotalizadorNaoSujeitoIcms(indice, descricao);
                    if (iRetorno != 1)
                        return false;
                    break;
                case 2:
                    DARUMA_FW.iRetorno = DARUMA_FW.confCadastrar_ECF_Daruma("TNF",descricao,"|");
                        if (DARUMA_FW.iRetorno != 1)
                           return false;
                    break;

                case 3:
                    Elgin32.Int_Retorno = Elgin32.Elgin_NomeiaTotalizadorNaoSujeitoIcms(indice, descricao);
                    break;
                case 4:
                    Sweda32.iRetorno = Sweda32.ECF_NomeiaTotalizadorNaoSujeitoIcms(indice, descricao);
                    break;
            }
            return true;
        }

        public static void corrigirEstadoECF()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return ;


            if (ecfAcbr.Estado == ACBrFramework.ECF.EstadoECF.NaoFiscal)
            {
                ecfAcbr.CancelaNaoFiscal();
            }
            else if (ecfAcbr.Estado == ACBrFramework.ECF.EstadoECF.Relatorio)
            {
                ecfAcbr.CorrigeEstadoErro();
            }
            else if (ecfAcbr.Estado == ACBrFramework.ECF.EstadoECF.NaoFiscal)
            {
                ecfAcbr.CorrigeEstadoErro();              
            }
            
        }

        public static ACBrFramework.ECF.EstadoECF estadoECFacbr()
        {
            return ecfAcbr.Estado;
        }

        public static bool RelatorioGerencial(string acao, string conteudo,string codigoRelatorio="",bool gravar=true)
        {
            if (ConfiguracoesECF.idNFC > 0 || (ConfiguracoesECF.davImpressaoCarta == false && ConfiguracoesECF.pdv == false))
            {

                if (acao.ToUpper()  == "IMPRIMIR")
                {
                    FuncoesNFC.conteudoImpressao.Append(conteudo);
                }

                if (acao.ToUpper() == "FECHAR")
                {
                    //FuncoesImpressao.conteudo = conteudoImpressao;
                    //FuncoesImpressao.impressaoDialog();
                    
                    FuncoesNFC Impressao = new FuncoesNFC();
                    Impressao.RelatorioGerencial();
                    FuncoesNFC.conteudoImpressao.Clear();
                }
                                
                return true;
            }

            string indiceRelatorio = "";
            string descricaoRelatorio = "";
            if (codigoRelatorio != "")
            {
                try
                {
                    if (File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\RelatorioGerencial.xml"))
                    {
                        XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\RelatorioGerencial.xml");
                        var dados = (from n in doc.Descendants("relatorio")
                                     where n.Attribute("codigo").Value == codigoRelatorio
                                     select new
                                         {
                                             indice = n.Attribute("indice").Value,
                                             descricao = n.Attribute("descricao").Value
                                         }
                                     ).First();
                        indiceRelatorio = dados.indice;
                        descricaoRelatorio = dados.descricao;
                    }
                }
                catch (Exception erro)
                {
                    MessageBox.Show("Não foi possivel encontrar o Relatorio Gerencial" + codigoRelatorio + " cadastrado na ECF");
                    MessageBox.Show(erro.Message);
                }
            
            }

            if (GlbVariaveis.glb_Acbr == false)
            {
                acao = acao.ToLower();
                if (acao == "imprimir" || acao == "abrir")
                {
                    switch (ConfiguracoesECF.idECF)
                    {
                        case 1:
                            if (indiceRelatorio != "" && acao == "abrir")
                            {
                                BemaFI32.Bematech_FI_AbreRelatorioGerencialMFD(indiceRelatorio);
                            };
                            int iRetorno = BemaFI32.Bematech_FI_RelatorioGerencial(conteudo);
                            if (iRetorno != 1)
                                return false;
                            break;

                        case 2:
                            if (indiceRelatorio != "" && acao == "abrir")
                            {
                                DARUMA_FW.iRGAbrir_ECF_Daruma(descricaoRelatorio);
                            };
                            DARUMA_FW.iRetorno = DARUMA_FW.iRGImprimirTexto_ECF_Daruma(conteudo);
                            if (DARUMA_FW.iRetorno != 1)
                                return false;
                            break;

                        case 3:
                            if (indiceRelatorio != "" && acao == "abrir")
                            {
                                Elgin32.Elgin_AbreRelatorioGerencial();
                            };
                            Elgin32.Int_Retorno = Elgin32.Elgin_RelatorioGerencial(conteudo);
                            if (Elgin32.Int_Retorno != 1)
                                return false;
                            break;

                        case 4:
                            if (indiceRelatorio != "" && acao == "abrir")
                            {
                                int iRetornoSW = Sweda32.ECF_AbreRelatorioGerencialMDF(indiceRelatorio);
                                if (iRetornoSW != 1)
                                    return false;
                            }
                            Sweda32.ECF_RelatorioGerencial(conteudo);
                            break;
                    }
                };

                if (acao == "fechar")
                {
                    switch (ConfiguracoesECF.idECF)
                    {
                        case 1:
                            int iRetorno = BemaFI32.Bematech_FI_FechaRelatorioGerencial();
                            if (iRetorno != 1)
                                return false;
                            if (gravar)
                                GravarContadorRelatorioGerencial();
                            break;

                        case 2:
                            DARUMA_FW.iRetorno = DARUMA_FW.iRGFechar_ECF_Daruma();
                            if (DARUMA_FW.iRetorno != 1)
                                return false;
                            if (gravar)
                                GravarContadorRelatorioGerencial();
                            break;

                        case 3:
                            Elgin32.Int_Retorno = Elgin32.Elgin_FechaRelatorioGerencial();
                            if (Elgin32.Int_Retorno != 1)
                                return false;
                            if (gravar)
                                GravarContadorRelatorioGerencial();
                            break;

                        case 4:
                            Sweda32.ECF_FechaRelatorioGerencial();
                            if (gravar)
                                GravarContadorRelatorioGerencial();
                            break;
                    }
                }
            }
            else
            {
                
                    if (acao.ToLower() == "abrir")
                    {
                        ecfAcbr.AbreRelatorioGerencial(1);
                    }

                    if (conteudo.Length > 0)
                    {
                        if (ecfAcbr.Estado != ACBrFramework.ECF.EstadoECF.Relatorio)
                        {
                            ecfAcbr.AbreRelatorioGerencial(1);
                            ecfAcbr.LinhaRelatorioGerencial(conteudo);
                        }
                        else
                        {
                            ecfAcbr.LinhaRelatorioGerencial(conteudo);
                        }
                    }

                    if(acao.ToLower() == "fechar")
                    {
                        if (ConfiguracoesECF.idECF == 1)
                        {
                            if (ecfAcbr.Estado == ACBrFramework.ECF.EstadoECF.Relatorio)
                                ecfAcbr.FechaRelatorio();
                        }
                        else
                        {
                            ecfAcbr.FechaRelatorio();

                        }

                        if (gravar)
                            GravarContadorRelatorioGerencial();
                    }
               

                
            }
            return true;
        }

        public static bool ComprovanteNaoFiscal(string acao,string codigoTotalizador, string cpfCliente,string cliente,string enderecoCliente, string tipoPagamento, string valor, string desconto, string conteudo)
        {

            if (ConfiguracoesECF.idNFC > 0)
                return true;




            string descricaoPagamento = " ";

            switch (tipoPagamento)
            {
                case "DH":
                    descricaoPagamento = ConfiguracoesECF.DH;
                    break;
                case "AV":
                    descricaoPagamento = ConfiguracoesECF.AV;
                    break;
                case "CA":
                case "FN":
                    descricaoPagamento = ConfiguracoesECF.CA;
                    break;
                case "CH":
                    descricaoPagamento = ConfiguracoesECF.CH;
                    break;
                case "CR":
                    descricaoPagamento = ConfiguracoesECF.CR;
                    break;
                case "TI":
                    descricaoPagamento = ConfiguracoesECF.TI;
                    break;
                case "DV":
                    descricaoPagamento = ConfiguracoesECF.DV;
                    break;
            }


            // Acao = Abrir
            // Acao = pagar
            // Acao = Fechar
            string indiceTotalizador = "";
            string descricaoToalizador = "RECEBIMENTO";

            try
            {
                if (File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ComprovanteNaoFiscal.xml"))
                {
                    XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\ComprovanteNaoFiscal.xml");
                    var dados = (from n in doc.Descendants("comprovante")
                                 where n.Attribute("codigo").Value.ToUpper().Contains(codigoTotalizador)
                                 select new
                                 {
                                     indice = n.Attribute("indice").Value,
                                     descricao = n.Attribute("descricao").Value
                                 }).First();
                    indiceTotalizador = dados.indice;
                    descricaoToalizador = dados.descricao;
                }
            }
            catch
            {
               
            }

            if (GlbVariaveis.glb_Acbr == false)
            {
                #region
                if (string.IsNullOrEmpty(indiceTotalizador))
                    return true;

                acao = acao.ToLower();
                if (acao == "abrir")
                {
                    switch (ConfiguracoesECF.idECF)
                    {
                        case 1:
                            int iRetorno = BemaFI32.Bematech_FI_AbreRecebimentoNaoFiscalMFD(cpfCliente, cliente, enderecoCliente);
                            if (iRetorno != 1)
                                return false;
                            break;
                        case 2:

                            DARUMA_FW.iRetorno = DARUMA_FW.iCNFAbrir_ECF_Daruma(cpfCliente, cliente, enderecoCliente);
                            if (DARUMA_FW.iRetorno != 1)
                                return false;
                            break;
                        case 3:
                            Elgin32.Int_Retorno = Elgin32.Elgin_AbreRecebimentoNaoFiscalMFD(cpfCliente, cliente, enderecoCliente);
                            if (Elgin32.Int_Retorno != 1)
                                return false;
                            break;
                        case 4:
                            Sweda32.iRetorno = Sweda32.ECF_AbreRecebimentoNaoFiscal(indiceTotalizador, "A", "$", desconto, valor, conteudo);
                            if (Sweda32.iRetorno != 1)
                                return false;
                            break;
                    }
                }

                if (acao == "pagar")
                {
                    switch (ConfiguracoesECF.idECF)
                    {
                        case 1:
                            int iRetorno = BemaFI32.Bematech_FI_EfetuaRecebimentoNaoFiscalMFD(indiceTotalizador, valor);
                            if (iRetorno != 1)
                                return false;
                            break;

                        case 2:
                            DARUMA_FW.iRetorno = DARUMA_FW.iCNFReceber_ECF_Daruma("03", valor.ToString(), "D%", "A%");
                            //Declaracoes.iRetorno = Declaracoes.iCNFReceber_ECF_Daruma(Str_Indice, Str_Valor_Recebimento, Str_Tipo_Desc_Acresc, Str_Valor_Desc_Acresc);
                            DARUMA_FW.iRetorno = DARUMA_FW.iCNFTotalizarComprovante_ECF_Daruma("D%", "0,00");
                            DARUMA_FW.iRetorno = DARUMA_FW.iCNFEfetuarPagamento_ECF_Daruma("Dinheiro", valor.ToString(), conteudo);
                            if (DARUMA_FW.iRetorno != 1)
                                return false;
                            break;

                        case 3:
                            break;
                        case 4:
                            int iRetornoSW = Sweda32.ECF_EfetuaRecebimentoNaoFiscalMFD(indiceTotalizador, valor);
                            if (iRetornoSW != 1)
                                return false;
                            break;

                    }
                }

                if (acao == "fechar")
                {
                    switch (ConfiguracoesECF.idECF)
                    {
                        case 1:
                            BemaFI32.Bematech_FI_IniciaFechamentoRecebimentoNaoFiscalMFD("D", "$", "0,00", desconto.ToString());

                            int iRetorno = BemaFI32.Bematech_FI_FechaRecebimentoNaoFiscalMFD(conteudo);
                            if (iRetorno != 1)
                                return false;
                            break;

                        case 2:
                            DARUMA_FW.iRetorno = DARUMA_FW.iCNFEncerrar_ECF_Daruma(conteudo);
                            break;
                        case 3:
                            break;
                        case 4:
                            int iRetornoSW = Sweda32.ECF_FechaRecebimentoNaoFiscalMFD(conteudo);
                            if (iRetornoSW != 1)
                                return false;
                            break;
                    }
                }
                #endregion
            }
            else
            {
                #region
                acao = acao.ToLower();
                if (acao == "abrir")
                {
                    try
                    {
                        ecfAcbr.AbreNaoFiscal(cpfCliente, cliente, enderecoCliente);
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.Message);
                    }
                }

                if (acao == "pagar")
                {

                    try
                    {
                        var comprovante = (from b in listaComprovanteNaoFiscal
                                            where b.descricao.ToLower().Contains(descricaoToalizador.ToLower())
                                            select b).FirstOrDefault();

                        var forma = (from f in listaFormapagamentoAcbr
                                     where f.descricao.ToLower().Contains(descricaoPagamento.ToLower())
                                     select f).FirstOrDefault();



                        if (forma == null)
                        {
                            MessageBox.Show("Não foi Possivel localizar a forma de pagamento " + descricaoPagamento + " no ECF ","Atenção",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return false;
                        }


                        if (comprovante == null)
                        {
                            MessageBox.Show("Não foi Possivel localizar o Comprovante não Fiscal " + descricaoPagamento + " no ECF ", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        if (forma != null && comprovante != null)
                        {
                            try
                            {
                                ecfAcbr.RegistraItemNaoFiscal(comprovante.codigo, decimal.Parse(valor), "");

                                ecfAcbr.SubtotalizaNaoFiscal(0, "");

                                ecfAcbr.EfetuaPagamentoNaoFiscal(forma.codigo, decimal.Parse(valor), "", false);
                            }
                            catch (Exception erro)
                            {
                                MessageBox.Show(erro.ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show("Não foi possivel encontrar o Totalizador não fiscal " + descricaoToalizador.ToString() + " cadastrado na ECF");
                        }
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.ToString());
                    }
                   
                }

                if (acao == "fechar")
                {
                    try
                    {
                        ecfAcbr.FechaNaoFiscal();
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.ToString());
                    }
                }

                if (acao == "cancelar")
                {
                    try
                    {
                        if (ecfAcbr.Estado == ACBrFramework.ECF.EstadoECF.NaoFiscal)
                        {
                            ecfAcbr.CancelaNaoFiscal();
                        }
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.ToString());
                    }
                }

                #endregion
            }

            return true;
        }


        public static bool ComprovanteNaoFiscalVinculado(string acao,string pagamento,string valor,string nCupom,string conteudo)
        {
            if (ConfiguracoesECF.idNFC > 0)
            {
                if (ConfiguracoesECF.idNFC > 0)
                {

                   if (acao == "Imprimir")
                    {
                        FuncoesNFC.conteudoImpressao.Append(conteudo);
                    }

                    if (acao == "Fechar")
                    {
                        //FuncoesImpressao.conteudo = conteudoImpressao;
                        //FuncoesImpressao.impressaoDialog();
                    
                        FuncoesNFC Impressao = new FuncoesNFC();
                        Impressao.RelatorioGerencial();
                        FuncoesNFC.conteudoImpressao.Clear();
                    }
                    return true;
                }

            }
               

            if (GlbVariaveis.glb_Acbr == false)
            {
                // Acao = Abrir
                // Acao = Imprimir
                // Acao = Fechar
                acao = acao.ToLower();
                if (acao == "abrir")
                {
                    if (nCupom == "" || nCupom == null)
                        nCupom = COONumeroCupomFiscal(ConfiguracoesECF.idECF);

                    switch (ConfiguracoesECF.idECF)
                    {
                        case 1:


                            int iRetorno = BemaFI32.Bematech_FI_AbreComprovanteNaoFiscalVinculadoMFD(pagamento, valor, nCupom, "", "", "");
                            if (iRetorno != 1)
                                return false;
                            break;

                        case 2:
                            DARUMA_FW.iRetorno = DARUMA_FW.iCCDAbrir_ECF_Daruma(pagamento, "1", nCupom, valor, "", "", "");
                            if (DARUMA_FW.iRetorno != 1)
                                return false;
                            break;
                        case 3:
                            Elgin32.Int_Retorno = Elgin32.Elgin_AbreComprovanteNaoFiscalVinculado(pagamento, valor, nCupom);
                            if (Elgin32.Int_Retorno != 1)
                                return false;
                            break;
                        case 4:
                            int iRetornoSW = Sweda32.ECF_AbreComprovanteNaoFiscalVinculadoMFD(pagamento, valor, nCupom, "", "", "");
                            if (iRetornoSW != 1)
                                return false;
                            break;
                    }
                    return true;
                }
                if (acao == "imprimir")
                {
                    switch (ConfiguracoesECF.idECF)
                    {
                        case 1:
                            int iRetorno = BemaFI32.Bematech_FI_UsaComprovanteNaoFiscalVinculado(conteudo);
                            if (iRetorno != 1)
                                return false;
                            break;
                        case 2:
                            DARUMA_FW.iRetorno = DARUMA_FW.iCCDImprimirTexto_ECF_Daruma(conteudo);
                            if (DARUMA_FW.iRetorno != 1)
                                return false;
                            break;
                        case 3:
                            Elgin32.Int_Retorno = Elgin32.Elgin_UsaComprovanteNaoFiscalVinculado(conteudo + "\r\n");
                            if (Elgin32.Int_Retorno != 1)
                                return false;
                            break;
                        case 4:
                            int iRetornoSw = Sweda32.ECF_UsaComprovanteNaoFiscalVinculado(conteudo + "\r\n");
                            if (iRetornoSw != 1)
                                return false;
                            break;
                    }
                    return true;
                }

                if (acao == "fechar")
                {
                    switch (ConfiguracoesECF.idECF)
                    {
                        case 1:
                            int iRetorno = BemaFI32.Bematech_FI_FechaRelatorioGerencial();
                            if (iRetorno != 1)
                                return false;
                            break;
                        case 2:
                            DARUMA_FW.iRetorno = DARUMA_FW.iCCDFechar_ECF_Daruma();
                            if (DARUMA_FW.iRetorno != 1)
                                return false;
                            break;
                        case 3:
                            Elgin32.Int_Retorno = Elgin32.Elgin_FechaRelatorioGerencial();
                            if (Elgin32.Int_Retorno != 1)
                                return false;
                            break;
                        case 4:
                            int iRetornoSW = Sweda32.ECF_FechaRelatorioGerencial();
                            if (iRetornoSW != 1)
                                return false;
                            break;

                    }
                }
            }
            else
            {

                if (acao.ToUpper() == "ABRIR")
                {
                    try
                    {

                         var forma = (from b in listaFormapagamentoAcbr
                                      where b.descricao.ToLower() == pagamento.ToLower()
                                            select b).FirstOrDefault();
                         if (forma.codigo != "" && forma.codigo != null)
                         {
                             ecfAcbr.AbreCupomVinculado(nCupom, forma.codigo, decimal.Parse(valor));
                         }
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.Message);
                    }
                }

                if (acao.ToUpper() == "IMPRIMIR")
                {
                    try
                    {
                        ecfAcbr.LinhaCupomVinculado(conteudo.ToString());
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.Message);
                    }
                }

                if (acao.ToUpper() == "FECHAR")
                {
                    try
                    {
                        if(ecfAcbr.Estado == ACBrFramework.ECF.EstadoECF.Relatorio)
                        ecfAcbr.FechaCupom("");
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.Message);
                    }
                }
            }
            return true;
        }

        public static bool VerificarStatusPapel(bool mensagem = false)
        {
            if (GlbVariaveis.glb_Acbr == false)
                return false;

                verificarGaveta = false;
                //ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                if (mensagem == true && ecfAcbr.PoucoPapel == true)
                    MessageBox.Show("Impressoa pouco papel!", "Anteção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                verificarGaveta = true;
                return ecfAcbr.PoucoPapel;
        }

        public static bool VerificaImpressoraLigada(bool ligada = true, bool verificarPapel = false)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;

            switch (ConfiguracoesECF.idECF)
            {
                case 1:
                    try
                    {
                        #region Bematech
                        if (GlbVariaveis.glb_Acbr == false)
                        {
                            #region DLL Fabricante
                            int iRetorno = BemaFI32.Bematech_FI_VerificaImpressoraLigada();
                            if (iRetorno != 1)
                                throw new Exception("Impressora desligada !");
                            else
                            {
                                int Int_ACK = 0;
                                int Int_ST1 = 0;
                                int Int_ST2 = 0;
                                BemaFI32.Bematech_FI_VerificaEstadoImpressora(ref Int_ACK, ref Int_ST1, ref Int_ST2);
                                if (Int_ST1 >= 128)
                                    throw new Exception("Falta de papel !");
                                return true;
                            }
                            #endregion
                        }
                        else
                        {
                            if (ligada == false)
                            {
                                #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                                if (ecfAcbr.Ativo == false)
                                {
                                    try
                                    {
                                        ecfAcbr.Desativar();
                                        ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Bematech;
                                        ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                                        ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                                        ecfAcbr.Device.TimeOut = 3000;//GlbVariaveis.glb_tempo;
                                        ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                                        ecfAcbr.Device.HardFlow = GlbVariaveis.glb_hardFlow;
                                        ecfAcbr.Device.SoftFlow = GlbVariaveis.glb_softFlow;
                                        ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;
                                        ecfAcbr.Ativar();

                                        if (ecfAcbr.Ativo == false)
                                        {
                                            MessageBox.Show("Impressora desligada ou sem papel");
                                            return false;
                                        }
                                        //ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                                    }
                                    catch(Exception erro)
                                    {
                                        MessageBox.Show("Impressora desligada ou sem papel");
                                        return false;
                                    }
                                    return true;

                                }
                                else
                                {
                                    if (verificarPapel == true)
                                        VerificarStatusPapel(true);


                                    //if (verificarPapel == true && ecfAcbr.PoucoPapel == true)
                                    //{
                                       // MessageBox.Show("Impressora com Pouco Papel");
                                        //return false;
                                    //}
                                    //else
                                    //{
                                        return true;
                                    //}
                                }
                                #endregion
                            }
                            else
                            {
                                #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.

                                    
                                    ecfAcbr.Desativar();
                                    ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Bematech;
                                    ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                                    ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                                    ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                                    ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                                    ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;
                                    ecfAcbr.Ativar();

                                    if (ecfAcbr.Ativo == false)
                                    {

                                        MessageBox.Show("Impressora desligada ou sem papel");
                                        return false;
                                    }
                                    
                                    if(verificarPapel == true)
                                        VerificarStatusPapel(true);


                                   // if (verificarPapel == true && ecfAcbr.PoucoPapel == true)
                                    //{
                                        //MessageBox.Show("Impressora com pouco Papel");
                                        //return true;
                                        //return false;
                                    //}
                                    //else
                                    //{
                                        return true;
                                    //}
                               
                                #endregion
                            }
                        }
                        #endregion
                    }
                    catch
                    {
                        throw new Exception("Impressora desligada ou sem papel");
                    }
                    
                case 2:
                   try
                   {
                       #region #Daruma

                       if (GlbVariaveis.glb_Acbr == false)
                       {
                           #region #DLL Fafricante
                           int iErro = 0;
                           int iAviso = 0;

                           DARUMA_FW.iRetorno = DARUMA_FW.rStatusUltimoCmdInt_ECF_Daruma(ref iErro, ref iAviso);
                           if (iErro == 72)
                           {
                               DARUMA_FW.iRetorno = DARUMA_FW.rVerificarImpressoraLigada_ECF_Daruma();
                               throw new Exception("Falta de papel !");
                           }

                           DARUMA_FW.iRetorno = 0;
                           DARUMA_FW.iRetorno = DARUMA_FW.rVerificarImpressoraLigada_ECF_Daruma();

                           if (DARUMA_FW.iRetorno != 1)
                               throw new Exception("Impressora desligada ou sem papel !");
                           else
                           {
                               DARUMA_FW.iRetorno = 0;
                               DARUMA_FW.iRetorno = DARUMA_FW.rVerificarImpressoraLigada_ECF_Daruma();

                               iErro = 0;
                               iAviso = 0;
                               DARUMA_FW.iRetorno = DARUMA_FW.rStatusUltimoCmdInt_ECF_Daruma(ref iErro, ref iAviso);
                               if (iErro == 72)
                                   throw new Exception("Falta de papel !");

                               return true;
                           }
                           #endregion
                       }
                       else
                       {

                           if(ligada == false)
                           {
                               #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                               if (ecfAcbr.Ativo == false)
                               {                                   
                                   ecfAcbr.Desativar();
                                   ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Daruma;
                                   ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                                   ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                                   ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                                   ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                                   ecfAcbr.Device.HardFlow = GlbVariaveis.glb_hardFlow;
                                   ecfAcbr.Device.SoftFlow = GlbVariaveis.glb_softFlow;
                                   ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;
                                   ecfAcbr.Ativar();
                                   if (ecfAcbr.Ativo == false)
                                   {
                                       MessageBox.Show("Impressora desligada ou sem papel");
                                       return false;
                                   }

                                       return true;
                               }
                               else
                               {

                                  
                                   if (verificarPapel == true)
                                       VerificarStatusPapel(true);

                                   return true;
                               }
                               #endregion
                           }
                           else
                           {
                               #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                              
                               ecfAcbr.Ativar();
                               if (ecfAcbr.Ativo == false)
                               {
                                   MessageBox.Show("Impressora desligada ou sem papel");
                                   return false;
                               }

                               if (verificarPapel == true)
                                   VerificarStatusPapel(true);

                               return true;

                               #endregion
                           }

                       }
                       #endregion
                   }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }
                case 3:
                   try
                   {
                       #region Elgin K
                       if (GlbVariaveis.glb_Acbr == false)
                       {
                           #region DLL Fabricante
                           Elgin32.Int_Retorno = Elgin32.Elgin_VerificaImpressoraLigada();
                           if (Elgin32.Int_Retorno != 1)
                               throw new Exception("Impressora desligada");
                           else
                           {
                               int Int_ACK = 0;
                               int Int_ST1 = 0;
                               int Int_ST2 = 0;
                               Elgin32.Elgin_VerificaEstadoImpressora(ref Int_ACK, ref Int_ST1, ref Int_ST2);
                               if (Int_ST1 >= 128)
                                   throw new Exception("Falta de papel !");
                               return true;
                           }
                           #endregion
                       }
                       else
                       {

                           
                           if (ligada == false)
                           {
                               #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                               if (ecfAcbr.Ativo == false)
                               {

                                   ecfAcbr.Desativar();
                                   ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.FiscNET;
                                   ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                                   ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                                   ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                                   ecfAcbr.Ativar();
                                   if (ecfAcbr.Ativo == false)
                                   {
                                       MessageBox.Show("Impressora desligada ou sem papel");
                                       return false;
                                   }

                                   if (verificarPapel == true)
                                       VerificarStatusPapel(true);

                                   return true;
                               }
                               else
                               {
                                   if (verificarPapel == true)
                                       VerificarStatusPapel(true);

                                   return true;
                               }
                               #endregion
                           }
                           else
                           {
                               #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                               
                               ecfAcbr.Ativar();
                               if (ecfAcbr.Ativo == false)
                               {
                                   MessageBox.Show("Impressora desligada ou sem papel");
                                   return false;
                               }

                               if (verificarPapel == true)
                                   VerificarStatusPapel(true);

                               return true;
                               #endregion
                           }
                       }
                       #endregion
                   }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }
                case 4:
                   try
                   {
                       #region Sewda
                       if (GlbVariaveis.glb_Acbr == false)
                       {
                           #region DLL Fabricante
                           Sweda32.iRetorno = Sweda32.ECF_VerificaImpressoraLigada();
                           if (Sweda32.iRetorno == 0)
                               throw new Exception("Impressora desligada");
                           else
                               return true;
                           #endregion
                       }
                       else
                       {
                           if (ligada == false)
                           {
                               #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                               if (ecfAcbr.Ativo == false)
                               {

                                   ecfAcbr.Desativar();
                                   ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Sweda;
                                   ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                                   ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                                   ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                                   ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                                   if (ecfAcbr.AcharPorta() == true)
                                       ecfAcbr.Ativar();
                                   else
                                   {
                                       MessageBox.Show("Impressora desligada ou sem papel");
                                       return false;
                                   }
                                   ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                                   return true;
                               }
                               else
                               {
                                   ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                                   if (ecfAcbr.PoucoPapel == true)
                                   {
                                       MessageBox.Show("Impressora com pouco papel");
                                       return false;
                                   }
                                   else
                                   {
                                       return true;
                                   }
                               }
                               #endregion
                           }
                           else
                           {
                               #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                               ecfAcbr.Desativar();
                               ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Sweda;
                               ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                               ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                               ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                               ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                               if (ecfAcbr.AcharPorta() == true)
                                   ecfAcbr.Ativar();
                               else
                               {
                                   MessageBox.Show("Impressora desligada ou sem papel");
                                   return false;
                               }
                               ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                               if (ecfAcbr.PoucoPapel == true)
                               {
                                   MessageBox.Show("Impressora com pouco papel");
                                   return false;
                               }
                               else
                               {
                                   return true;
                               }
                               #endregion
                           }
                       }
                       #endregion
                   }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }

                case 5:
                   try
                   {
                       #region Schalter
                       if (ligada == false)
                            {
                                #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                                if (ecfAcbr.Ativo == false)
                                {

                                    ecfAcbr.Desativar();
                                    ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Schalter;
                                    ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                                    ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                                    ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                                    ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                                    if (ecfAcbr.AcharPorta() == true)
                                        ecfAcbr.Ativar();
                                    else
                                    {
                                        MessageBox.Show("Impressora desligada ou sem papel");
                                        return false;
                                    }
                                    ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                                    return true;
                                }
                                else
                                {
                                    

                                    if (ecfAcbr.PoucoPapel == true)
                                    {
                                        MessageBox.Show("Impressora com pouco papel");
                                        return false;
                                    }
                                    else
                                    {
                                        return true;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                                ecfAcbr.Desativar();
                                ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Schalter;
                                ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                                ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                                ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                                ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                                if (ecfAcbr.AcharPorta() == true)
                                    ecfAcbr.Ativar();
                                else
                                {
                                    MessageBox.Show("Impressora desligada ou sem papel");
                                    return false;
                                }
                                ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                                if (ecfAcbr.PoucoPapel == true)
                                {
                                    MessageBox.Show("Impressora com pouco papel");
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                                #endregion
                            }
                       #endregion
                   }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }
                case 6:
                   try
                   {
                       #region Mecaf
                       if (ligada == false)
                       {
                           #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                           if (ecfAcbr.Ativo == false)
                           {

                               ecfAcbr.Desativar();
                               ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Mecaf;
                               ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                               ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                               ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                               ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                               if (ecfAcbr.AcharPorta() == true)
                                   ecfAcbr.Ativar();
                               else
                               {
                                   MessageBox.Show("Impressora desligada ou sem papel");
                                   return false;
                               }
                               ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                               return true;
                           }
                           else
                           {


                               if (ecfAcbr.PoucoPapel == true)
                               {
                                   MessageBox.Show("Impressora com pouco papel");
                                   return false;
                               }
                               else
                               {
                                   return true;
                               }
                           }
                           #endregion
                       }
                       else
                       {
                           #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                           ecfAcbr.Desativar();
                           ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Mecaf;
                           ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                           ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                           ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                           ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                           if (ecfAcbr.AcharPorta() == true)
                               ecfAcbr.Ativar();
                           else
                           {
                               MessageBox.Show("Impressora desligada ou sem papel");
                               return false;
                           }
                           ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                           if (ecfAcbr.PoucoPapel == true)
                           {
                               MessageBox.Show("Impressora com pouco papel");
                               return false;
                           }
                           else
                           {
                               return true;
                           }
                           #endregion
                       }
                       #endregion
                   }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }

                case 7:
                   try
                   {
                       #region Yanco
                       if (ligada == false)
                       {
                           #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                           if (ecfAcbr.Ativo == false)
                           {

                               ecfAcbr.Desativar();
                               ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Yanco;
                               ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                               ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                               ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                               ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                               if (ecfAcbr.AcharPorta() == true)
                                   ecfAcbr.Ativar();
                               else
                               {
                                   MessageBox.Show("Impressora desligada ou sem papel");
                                   return false;
                               }
                               ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                               return true;
                           }
                           else
                           {


                               if (ecfAcbr.PoucoPapel == true)
                               {
                                   MessageBox.Show("Impressora com pouco papel");
                                   return false;
                               }
                               else
                               {
                                   return true;
                               }
                           }
                           #endregion
                       }
                       else
                       {
                           #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                           ecfAcbr.Desativar();
                           ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Yanco;
                           ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                           ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                           ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                           ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                           if (ecfAcbr.AcharPorta() == true)
                               ecfAcbr.Ativar();
                           else
                           {
                               MessageBox.Show("Impressora desligada ou sem papel");
                               return false;
                           }
                           ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                           if (ecfAcbr.PoucoPapel == true)
                           {
                               MessageBox.Show("Impressora com pouco papel");
                               return false;
                           }
                           else
                           {
                               return true;
                           }
                           #endregion
                       }
                       #endregion
                   }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }

                case 8:

                   try
                   {
                       #region DataRegis
                       if (ligada == false)
                       {
                           #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                           if (ecfAcbr.Ativo == false)
                           {

                               ecfAcbr.Desativar();
                               ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.DataRegis;
                               ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                               ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                               ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                               ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                               if (ecfAcbr.AcharPorta() == true)
                                   ecfAcbr.Ativar();
                               else
                               {
                                   MessageBox.Show("Impressora desligada ou sem papel");
                                   return false;
                               }
                               ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                               return true;
                           }
                           else
                           {


                               if (ecfAcbr.PoucoPapel == true)
                               {
                                   MessageBox.Show("Impressora com pouco papel");
                                   return false;
                               }
                               else
                               {
                                   return true;
                               }
                           }
                           #endregion
                       }
                       else
                       {
                           #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                           ecfAcbr.Desativar();
                           ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.DataRegis;
                           ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                           ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                           ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                           ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                           if (ecfAcbr.AcharPorta() == true)
                               ecfAcbr.Ativar();
                           else
                           {
                               MessageBox.Show("Impressora desligada ou sem papel");
                               return false;
                           }
                           ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                           if (ecfAcbr.PoucoPapel == true)
                           {
                               MessageBox.Show("Impressora com pouco papel");
                               return false;
                           }
                           else
                           {
                               return true;
                           }
                           #endregion
                       }
                       #endregion
                   }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }
                case 9:
                   try
                   {
                       #region Urano
                       if (ligada == false)
                       {
                           #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                           if (ecfAcbr.Ativo == false)
                           {

                               ecfAcbr.Desativar();
                               ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Urano;
                               ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                               ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                               ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                               ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                               if (ecfAcbr.AcharPorta() == true)
                                   ecfAcbr.Ativar();
                               else
                               {
                                   MessageBox.Show("Impressora desligada ou sem papel");
                                   return false;
                               }
                               ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                               return true;
                           }
                           else
                           {


                               if (ecfAcbr.PoucoPapel == true)
                               {
                                   MessageBox.Show("Impressora com pouco papel");
                                   return false;
                               }
                               else
                               {
                                   return true;
                               }
                           }
                           #endregion
                       }
                       else
                       {
                           #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                           ecfAcbr.Desativar();
                           ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Urano;
                           ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                           ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                           ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                           ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                           if (ecfAcbr.AcharPorta() == true)
                               ecfAcbr.Ativar();
                           else
                           {
                               MessageBox.Show("Impressora desligada ou sem papel");
                               return false;
                           }
                           ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                           if (ecfAcbr.PoucoPapel == true)
                           {
                               MessageBox.Show("Impressora com pouco papel");
                               return false;
                           }
                           else
                           {
                               return true;
                           }
                           #endregion
                       }
                       #endregion
                   }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }
                case 10:

                   try
                   {
                       #region ICash
                       if (ligada == false)
                       {
                           #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                           if (ecfAcbr.Ativo == false)
                           {

                               ecfAcbr.Desativar();
                               ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.ICash;
                               ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                               ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                               ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                               ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                               if (ecfAcbr.AcharPorta() == true)
                                   ecfAcbr.Ativar();
                               else
                               {
                                   MessageBox.Show("Impressora desligada ou sem papel");
                                   return false;
                               }
                               ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                               return true;
                           }
                           else
                           {


                               if (ecfAcbr.PoucoPapel == true)
                               {
                                   MessageBox.Show("Impressora com pouco papel");
                                   return false;
                               }
                               else
                               {
                                   return true;
                               }
                           }
                           #endregion
                       }
                       else
                       {
                           #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                           ecfAcbr.Desativar();
                           ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.ICash;
                           ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                           ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                           ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                           ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                           if (ecfAcbr.AcharPorta() == true)
                               ecfAcbr.Ativar();
                           else
                           {
                               MessageBox.Show("Impressora desligada ou sem papel");
                               return false;
                           }
                           ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                           if (ecfAcbr.PoucoPapel == true)
                           {
                               MessageBox.Show("Impressora com pouco papel");
                               return false;
                           }
                           else
                           {
                               return true;
                           }
                           #endregion
                       }
                       #endregion
                   }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }
                case 11:
                   try
                   {
                       #region Quattro
                       if (ligada == false)
                       {
                           #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                           if (ecfAcbr.Ativo == false)
                           {

                               ecfAcbr.Desativar();
                               ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Quattro;
                               ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                               ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                               ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                               ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                               if (ecfAcbr.AcharPorta() == true)
                                   ecfAcbr.Ativar();
                               else
                               {
                                   MessageBox.Show("Impressora desligada ou sem papel");
                                   return false;
                               }
                               ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                               return true;
                           }
                           else
                           {


                               if (ecfAcbr.PoucoPapel == true)
                               {
                                   MessageBox.Show("Impressora com pouco papel");
                                   return false;
                               }
                               else
                               {
                                   return true;
                               }
                           }
                           #endregion
                       }
                       else
                       {
                           #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                           ecfAcbr.Desativar();
                           ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Quattro;
                           ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                           ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                           ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                           ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                           if (ecfAcbr.AcharPorta() == true)
                               ecfAcbr.Ativar();
                           else
                           {
                               MessageBox.Show("Impressora desligada ou sem papel");
                               return false;
                           }
                           ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                           if (ecfAcbr.PoucoPapel == true)
                           {
                               MessageBox.Show("Impressora com pouco papel");
                               return false;
                           }
                           else
                           {
                               return true;
                           }
                           #endregion
                       }
                       #endregion
                       
                   }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }

                case 12:
                   try
                   {
                       #region Epson
                       if (ligada == false)
                       {
                           #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                           if (ecfAcbr.Ativo == false)
                           {

                               ecfAcbr.Desativar();
                               ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Epson;
                               ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                               ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                               ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                               ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                               if (ecfAcbr.AcharPorta() == true)
                                   ecfAcbr.Ativar();
                               else
                               {
                                   MessageBox.Show("Impressora desligada ou sem papel");
                                   return false;
                               }
                               ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                               return true;
                           }
                           else
                           {


                               if (ecfAcbr.PoucoPapel == true)
                               {
                                   MessageBox.Show("Impressora com pouco papel");
                                   return false;
                               }
                               else
                               {
                                   return true;
                               }
                           }
                           #endregion
                       }
                       else
                       {
                           #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                           ecfAcbr.Desativar();
                           ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.Epson;
                           ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                           ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                           ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                           ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                           if (ecfAcbr.AcharPorta() == true)
                               ecfAcbr.Ativar();
                           else
                           {
                               MessageBox.Show("Impressora desligada ou sem papel");
                               return false;
                           }
                           ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                           if (ecfAcbr.PoucoPapel == true)
                           {
                               MessageBox.Show("Impressora com pouco papel");
                               return false;
                           }
                           else
                           {
                               return true;
                           }
                           #endregion
                       }
                       #endregion
                   }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }
                case 13:
                   try
                   {
                       #region NCR
                       if (ligada == false)
                       {
                           #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                           if (ecfAcbr.Ativo == false)
                           {

                               ecfAcbr.Desativar();
                               ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.NCR;
                               ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                               ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                               ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                               ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                               if (ecfAcbr.AcharPorta() == true)
                                   ecfAcbr.Ativar();
                               else
                               {
                                   MessageBox.Show("Impressora desligada ou sem papel");
                                   return false;
                               }
                               ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                               return true;
                           }
                           else
                           {


                               if (ecfAcbr.PoucoPapel == true)
                               {
                                   MessageBox.Show("Impressora com pouco papel");
                                   return false;
                               }
                               else
                               {
                                   return true;
                               }
                           }
                           #endregion
                       }
                       else
                       {
                           #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                           ecfAcbr.Desativar();
                           ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.NCR;
                           ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                           ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                           ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                           ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                           if (ecfAcbr.AcharPorta() == true)
                               ecfAcbr.Ativar();
                           else
                           {
                               MessageBox.Show("Impressora desligada ou sem papel");
                               return false;
                           }
                           ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                           if (ecfAcbr.PoucoPapel == true)
                           {
                               MessageBox.Show("Impressora com pouco papel");
                               return false;
                           }
                           else
                           {
                               return true;
                           }
                           #endregion
                       }
                       #endregion
                   
                    }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }
                case 14:
                   try
                   {

                       #region SwedaSTX
                       if (ligada == false)
                       {
                           #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                           if (ecfAcbr.Ativo == false)
                           {

                               ecfAcbr.Desativar();
                               ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.SwedaSTX;
                               ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                               ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                               ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                               ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                               if (ecfAcbr.AcharPorta() == true)
                                   ecfAcbr.Ativar();
                               else
                               {
                                   MessageBox.Show("Impressora desligada ou sem papel");
                                   return false;
                               }
                               ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                               return true;
                           }
                           else
                           {


                               if (ecfAcbr.PoucoPapel == true)
                               {
                                   MessageBox.Show("Impressora com pouco papel");
                                   return false;
                               }
                               else
                               {
                                   return true;
                               }
                           }
                           #endregion
                       }
                       else
                       {
                           #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                           ecfAcbr.Desativar();
                           ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.SwedaSTX;
                           ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                           ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                           ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                           ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                           if (ecfAcbr.AcharPorta() == true)
                               ecfAcbr.Ativar();
                           else
                           {
                               MessageBox.Show("Impressora desligada ou sem papel");
                               return false;
                           }
                           ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                           if (ecfAcbr.PoucoPapel == true)
                           {
                               MessageBox.Show("Impressora com pouco papel");
                               return false;
                           }
                           else
                           {
                               return true;
                           }
                           #endregion
                       }
                       #endregion
                   }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }
                case 15:
                   try
                   {
                       #region EscECF
                       if (ligada == false)
                       {
                           #region #não verifica impressora ligada so entra aqui nas formas de pagamentos quando está tortalizando o cupom
                           if (ecfAcbr.Ativo == false)
                           {

                               ecfAcbr.Desativar();
                               ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.EscECF;
                               ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                               ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                               ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                               ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                               if (ecfAcbr.AcharPorta() == true)
                                        ecfAcbr.Ativar();
                                    else
                                    {
                                        MessageBox.Show("Impressora desligada ou sem papel");
                                        return false;
                                    }
                               ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                               return true;
                           }
                           else
                           {


                               if (ecfAcbr.PoucoPapel == true)
                               {
                                   MessageBox.Show("Impressora com pouco papel");
                                   return false;
                               }
                               else
                               {
                                   return true;
                               }
                           }
                           #endregion
                       }
                       else
                       {
                           #region #verifica impressora ligada entra aqui em todas as acões do sistema com excessão na forma de pagamento.
                           ecfAcbr.Desativar();
                           ecfAcbr.Modelo = ACBrFramework.ECF.ModeloECF.EscECF;
                           ecfAcbr.Device.Baud = GlbVariaveis.glb_velocidadeAcbr;
                           ecfAcbr.Device.Porta = GlbVariaveis.glb_portaAcbr;
                           ecfAcbr.Device.TimeOut = GlbVariaveis.glb_tempo;
                           ecfAcbr.DescricaoGrande = GlbVariaveis.glb_descricaoGrande;
                           if (ecfAcbr.AcharPorta() == true)
                                        ecfAcbr.Ativar();
                                    else
                                    {
                                        MessageBox.Show("Impressora desligada ou sem papel");
                                        return false;
                                    }
                           ecfAcbr.OnMsgPoucoPapel += ecfAcbr_OnMsgPoucoPapel;

                           if (ecfAcbr.PoucoPapel == true)
                           {
                               MessageBox.Show("Impressora com pouco papel");
                               return false;
                           }
                           else
                           {
                               return true;
                           }
                           #endregion
                       }
                       #endregion
                   }
                   catch
                   {
                       throw new Exception("Impressora desligada ou sem papel");
                   }
            }
            return true;
        }


        private static void ecfAcbr_OnMsgPoucoPapel(object sender, EventArgs e)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return ;

            //MessageBox.Show("Impressora com pouco papel");
        }

        private static void ecfAcbr_OnErrorSemPapel(object sender, EventArgs e)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return;

            MessageBox.Show("Impressora Desligada ou Sem Papel");
        }


        public static decimal TotalLiquidoCupomECF()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return 0;

            if (ConfiguracoesECF.pdv == false)
                return 0;

            if (GlbVariaveis.glb_Acbr == false)
            {
                string subtotal = new string(' ', 14);
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:

                        BemaFI32.Bematech_FI_SubTotal(ref subtotal);
                        return Convert.ToDecimal(subtotal) / 100;
                    case 2:
                        StringBuilder subTotal = new StringBuilder();
                        DARUMA_FW.iRetorno = DARUMA_FW.rCFSubTotal_ECF_Daruma(subTotal);
                        return Convert.ToDecimal(subTotal.ToString().Substring(0, 12)) / 100;
                    case 3:
                        Elgin32.Int_Retorno = Elgin32.Elgin_SubTotal(ref subtotal);
                        return Convert.ToDecimal(subtotal) / 100;
                    case 4:
                        Sweda32.iRetorno = Sweda32.ECF_SubTotal(subtotal);
                        if (subtotal.Trim() == "" || subtotal == null)
                            subtotal = "100";
                        return Convert.ToDecimal(subtotal) / 100;
                };
            }
            else
            {
                return ecfAcbr.SubTotal;
            }
            return 0;
        }
        /// <summary>
        /// Esse método retonar o valor já efetuado das forma de pagamento do ECF
        /// </summary>
        /// <returns></returns>
        public static decimal TotalPagoCupomECF()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return 0;
            if (ConfiguracoesECF.pdv == false)
                return 0;

            string valorPago = new string(' ', 14);

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        BemaFI32.Bematech_FI_ValorPagoUltimoCupom(ref valorPago);
                        return Convert.ToDecimal(valorPago) / 100;
                    case 2:
                        StringBuilder subTotal = new StringBuilder(' ', 2200);
                        DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("48", subTotal);
                        return Convert.ToDecimal(subTotal.ToString().Substring(0, 12)) / 100;
                    case 3:
                        Elgin32.Elgin_ValorPagoUltimoCupom(ref valorPago);
                        return Convert.ToDecimal(valorPago.Trim()) / 100;
                    case 4:

                        Sweda32.ECF_ValorPagoUltimoCupom(ref valorPago);
                        if (valorPago.Trim() == "" || valorPago == null)
                            valorPago = "100";

                        return Convert.ToDecimal(valorPago) / 100;
                };
            }
            else
            {
                return ecfAcbr.TotalPago;
            }
            return 0;
        }

        public static bool CupomFiscalAberto()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return false;

            if (ConfiguracoesECF.pdv == false || ConfiguracoesECF.idECF == 0)
                return false;

            int Int_ACK = 0;
            int Int_ST1 = 0;
            int Int_ST2 = 0;

            if (GlbVariaveis.glb_Acbr == false)
            {

                switch (ConfiguracoesECF.idECF)
                {
                    case 1:

                        VerificaImpressoraLigada();
                        
                            BemaFI32.Bematech_FI_RetornoImpressora(ref Int_ACK, ref Int_ST1, ref Int_ST2);
                            if (Int_ST1 == 2)
                                return true;
                            else
                                return false;
                        
                    case 2:
                        
                            StringBuilder Str_Informacao = new StringBuilder();
                            DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("56", Str_Informacao);
                            if (int.Parse(Str_Informacao.ToString().Trim().Substring(0, 1)) == 0)
                                return false;
                            if (int.Parse(Str_Informacao.ToString().Trim().Substring(0, 1)) == 1)
                                return true;
                            break;
                       

                    case 3: //Elgin
                        VerificaImpressoraLigada();
                       
                            int iFlag = 0;
                            Elgin32.Elgin_FlagsFiscais(ref iFlag);
                            // Flag 33 colocada 
                            if (iFlag == 1 || iFlag == 33)
                                return true;
                            else
                                return false;
                       

                    case 4:
                           StringBuilder status = new StringBuilder();
                            Sweda32.iRetorno = Sweda32.ECF_StatusCupomFiscal(status);
                           if (status.ToString() == "1")
                                return true;
                            else
                                return false;
                       

                }
            }
            else
            {
                if (File.Exists("versaoframework.txt"))
                    VerificaImpressoraLigada();

                try
                {
                    if (ecfAcbr.Estado == ACBrFramework.ECF.EstadoECF.Livre || ecfAcbr.Estado == ACBrFramework.ECF.EstadoECF.RequerZ)
                        return false;
                    else
                        return true;
                }
                catch (Exception erro)
                {
                    return false;
                }
            }
            return false;
        }

        public static ACBrFramework.ECF.EstadoECF estadoECF()
        {          
           return ecfAcbr.Estado;
        }

        public static bool EfetuarPagamentoECF(string formaPagamento, decimal valor)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;

            if (ConfiguracoesECF.pdv == false)
                return true;
            try
            {
                if(GlbVariaveis.glb_Acbr == false)
                    System.Threading.Thread.Sleep(400);

                VerificaImpressoraLigada(false);
            }
            catch 
            {
                Funcoes.TravarTeclado(false);
                return false;
            }


            if (GlbVariaveis.glb_Acbr == false)
            {
                #region
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        int iRetorno = BemaFI32.Bematech_FI_EfetuaFormaPagamento(formaPagamento, valor.ToString());
                        if (iRetorno != 1)
                            throw new Exception("Verifique a Forma de pagamento " + formaPagamento + " na ECF");
                        else
                            return true;
                    case 2:
                        try
                        {
                            VerificaImpressoraLigada();
                        }
                        catch
                        {
                            return false;
                        };
                        if (!VerificaImpressoraLigada())
                            DetectarECF(2);
                        DARUMA_FW.iRetorno = DARUMA_FW.iCFEfetuarPagamento_ECF_Daruma(formaPagamento, valor.ToString(), "");


                        if (DARUMA_FW.iRetorno != 1)
                            return false; //throw new Exception("");
                        else
                            return true;
                    case 3:

                        Elgin32.Int_Retorno = Elgin32.Elgin_EfetuaFormaPagamento(formaPagamento, valor.ToString());
                        if (Elgin32.Int_Retorno != 1)
                            throw new Exception("");
                        else
                            return true;
                    case 4:
                        Sweda32.iRetorno = Sweda32.ECF_EfetuaFormaPagamento(formaPagamento, valor.ToString());
                        if (Sweda32.iRetorno != 1)
                            throw new Exception("");
                        else
                            return true;
                }
                #endregion
            }
            else
            {
                try
                {

                    if(ecfAcbr.Estado == EstadoECF.Pagamento)
                    {
                        if (ecfAcbr.TotalPago < ecfAcbr.SubTotal)
                        {

                            var forma = (from b in listaFormapagamentoAcbr
                                         where b.descricao.ToLower() == formaPagamento.ToLower()
                                         select b).FirstOrDefault();

                            if (forma.codigo != "" && forma.codigo != null)
                            {
                                try
                                {
                                    ecfAcbr.EfetuaPagamento(forma.codigo, valor, "", forma.vinculado);
                                }
                                catch (Exception erro)
                                {
                                    return false;
                                }

                                //MessageBox.Show(ecfAcbr.SubTotal.ToString() + " - " + ecfAcbr.TotalPago.ToString());
                                return true;
                            }
                            else
                                return false;
                        }
                        else if (ecfAcbr.TotalPago == ecfAcbr.SubTotal)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;

                    }
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                    return false;
                }
            }
            return false;
        }

        public static bool formapagamentoACBR()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;

            try
            {
                if (ecfAcbr.Ativo == true)
                {
                    ecfAcbr.CarregaFormasPagamento();

                    var formas = ecfAcbr.FormasPagamento.ToList();


                    foreach (var f in formas)
                    {
                        formaPagamentoAcbr n = new formaPagamentoAcbr();
                        n.codigo = f.Indice.Trim();
                        n.descricao = f.Descricao.Trim();
                        n.vinculado = f.PermiteVinculado;

                        listaFormapagamentoAcbr.Add(n);

                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);

                return false;
            }

           
        }

        public static bool ComprovanteNaoFiscalACBR()
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;

            try
            {
                if (ecfAcbr.Ativo == true)
                {
                    ecfAcbr.CarregaComprovantesNaoFiscais();

                    var formas = ecfAcbr.ComprovantesNaoFiscais;


                    foreach (var f in formas)
                    {
                        formaPagamentoAcbr n = new formaPagamentoAcbr();
                        n.codigo = f.Indice.Trim();
                        n.descricao = f.Descricao.Trim();
                        n.vinculado = f.PermiteVinculado;

                        listaComprovanteNaoFiscal.Add(n);

                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);

                return false;
            }


        }

        public static bool FecharCupomECF(string mensagem)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;

            if (ConfiguracoesECF.pdv == false)
                return true;

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        BemaFI32.Bematech_FI_TerminaFechamentoCupom(mensagem);
                        break;
                    case 2:
                        DARUMA_FW.iRetorno = DARUMA_FW.eAbrirGaveta_ECF_Daruma();

                        DARUMA_FW.iCFIdentificarConsumidor_ECF_Daruma
                        (Venda.dadosConsumidor.nomeConsumidor, Venda.dadosConsumidor.endConsumidor, Venda.dadosConsumidor.cpfCnpjConsumidor);

                        DARUMA_FW.iRetorno = DARUMA_FW.iCFEncerrarConfigMsg_ECF_Daruma(mensagem);
                        if (DARUMA_FW.iRetorno != 1)
                            throw new Exception("");
                        else
                            return true;
                    case 3:
                        AbrirGaveta();
                        Elgin32.Elgin_TerminaFechamentoCupom(mensagem);
                        break;
                    case 4:
                        Sweda32.ECF_TerminaFechamentoCupom(mensagem);
                        break;
                }
            }
            else
            {
                try
                {
                    if (ConfiguracoesECF.pdv == true)
                    {
                        AbrirGaveta();
                        ecfAcbr.FechaCupom(mensagem.Replace("|", "\n"));
                    }
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }
            }
            return true;

        }
        
        // Essa função retorna o Espelho da última redução Z
        public static List<string> DownloadDaMFD(string COOIncial, string COOFinal)
        {
            List<string> dados = new List<string>();

            if (ConfiguracoesECF.idNFC > 0)
                return dados;



            string linha = "";
            string arquivo = ConfiguracoesECF.pathRetornoECF + "Retorno.txt";

            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        System.Diagnostics.Process.Start("registra.bat", "");
                        int iRetorno = BemaFI32.Bematech_FI_DownloadMFD("DOWNLOAD.MFD", "2", COOIncial, COOFinal, "1");
                        System.Threading.Thread.Sleep(500);
                        BemaFI32.Bematech_FI_FormatoDadosMFD("DOWNLOAD.MFD", @ConfiguracoesECF.pathRetornoECF + "retorno.txt", "0", "2", COOIncial, COOFinal, "1");
                        System.IO.StreamReader file = new System.IO.StreamReader(arquivo, Encoding.GetEncoding("ISO-8859-1"));
                        while ((linha = file.ReadLine()) != null)
                        {
                            dados.Add(linha);
                        }
                        file.Close();
                        return dados;

                    case 2:
                        DARUMA_FW.iRetorno = DARUMA_FW.rGerarEspelhoMFD_ECF_Daruma("2", COOIncial, COOFinal);
                        Thread.Sleep(1000);
                        if (File.Exists(@ConfiguracoesECF.pathRetornoECF + "Espelho_MFD.txt"))
                        {
                            if (File.Exists(arquivo))
                                File.Delete(arquivo);
                            File.Copy(@ConfiguracoesECF.pathRetornoECF + "Espelho_MFD.txt", arquivo);
                        }



                        if (DARUMA_FW.iRetorno != 1)
                        {
                            string erro = "";
                            if (DARUMA_FW.iRetorno == -110)
                                erro = "Período sem movimento";

                            throw new Exception("Erro retornado:" + DARUMA_FW.iRetorno.ToString() + " : " + erro);
                        }

                        System.IO.StreamReader fileMFD = new System.IO.StreamReader(arquivo, Encoding.GetEncoding("ISO-8859-1"));
                        while ((linha = fileMFD.ReadLine()) != null)
                        {
                            dados.Add(linha);
                        }
                        fileMFD.Close();
                        return dados;
                    case 3:
                        //System.Diagnostics.Process.Start("registra.bat", "");
                        Elgin32.Int_Retorno = Elgin32.Elgin_DownloadMFD("DOWNLOAD.MFD", "2", COOIncial, COOFinal, "1");
                        System.Threading.Thread.Sleep(500);
                        Elgin32.Elgin_FormatoDadosMFD("DOWNLOAD.MFD", @ConfiguracoesECF.pathRetornoECF + "retorno.txt", "0", "2", COOIncial, COOFinal, "1");
                        System.IO.StreamReader fileElgin = new System.IO.StreamReader(arquivo, Encoding.GetEncoding("ISO-8859-1"));
                        while ((linha = fileElgin.ReadLine()) != null)
                        {
                            dados.Add(linha);
                        }
                        fileElgin.Close();
                        return dados; 
                }
            }
            else
            {
                ecfAcbr.DadosUltimaReducaoZ();
                dados.Add("Total Oper N " + ecfAcbr.DadosReducaoZClass.TotalOperacaoNaoFiscal);
                dados.Add("ACRÉSCIMO ICMS " + ecfAcbr.DadosReducaoZClass.AcrescimoICMS);
                dados.Add("ACRÉSCIMO ISSQN " + ecfAcbr.DadosReducaoZClass.AcrescimoISSQN);

                //dados.Add(linha);

                return dados; 
            }
          
                            
            
            return null;
        }

        public static decimal TotalItemECF()
        {

            if (ConfiguracoesECF.idNFC > 0)
                return 0;

            if (ConfiguracoesECF.pdv == false)
                return 0;

            switch (ConfiguracoesECF.idECF)
            {
                case 1:
                    break;
                case 2:
                    StringBuilder total = new StringBuilder(' ', 2200);                    
                        DARUMA_FW.iRetorno = DARUMA_FW.rRetornarInformacao_ECF_Daruma("62", total);
                        return Convert.ToDecimal(total.ToString().Substring(0,11)) / 100;                    
            }
            return 0;
        }

        public static bool ZPendente()
        {

            if (ConfiguracoesECF.idNFC > 0)
                return false;


            if (GlbVariaveis.glb_Acbr == false)
            {
                #region
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        //? 
                        
                            if (Convert.ToDateTime(DataMovimentoECF()) < GlbVariaveis.Sys_Data)
                                return true;
                            break;
                        
                    case 2:

                       
                            StringBuilder str_Informacao = new StringBuilder();
                            DARUMA_FW.iRetorno = DARUMA_FW.rVerificarReducaoZ_ECF_Daruma(str_Informacao);
                            if (DARUMA_FW.iRetorno != 1)
                                return true;
                            if (str_Informacao.ToString().Trim().Substring(0, 1) == "1")
                                return true;
                            break;
                        
                    case 3:
                        
                            int indicador = 0;
                            Elgin32.Elgin_LeIndicadores(ref indicador);
                            if (indicador == 128 || indicador == 14528)
                                return true;
                            else
                                return false;
                       

                    case 4:
                        
                            string variavelInicial = new string(' ', 2);
                            StringBuilder zpendente = new StringBuilder(variavelInicial);
                            Sweda32.ECF_VerificaZPendente(zpendente);
                            if (zpendente.ToString() == "1")
                                return true;
                            else
                                return false;

                }
                #endregion
            }
            else
            {
                if (ecfAcbr.Estado == ACBrFramework.ECF.EstadoECF.RequerZ)
                    return true;
                else
                    return false;

                      
            }
            return false;
        }
        /// <summary>
        /// Verifica se o ECF já emitiu a redução Z do dia
        /// </summary>
        /// <returns></returns>
        public static bool VerificaReducaZDia()
        {

            if (ConfiguracoesECF.idNFC > 0)
                return false;


            if (GlbVariaveis.glb_Acbr == false)
            {
                int informacao = 0;
                switch (ConfiguracoesECF.idECF)
                {



                    case 1:
                        //if (DataUltimaReduzacaoZ().Date == GlbVariaveis.Sys_Data.Date)
                        //   return true;
                        
                            int iRetorno = BemaFI32.Bematech_FI_FlagsFiscais(ref informacao);

                            if (informacao == 8 || informacao == 12)
                                return true;
                            break;
                        

                    case 2:

                           StringBuilder dataZ = new StringBuilder(2200);
                            DARUMA_FW.rRetornarInformacao_ECF_Daruma("71", dataZ);
                            if (Convert.ToDateTime(dataZ.ToString().Substring(0, 2) + "/" + dataZ.ToString().Substring(2, 2) + "/" + dataZ.ToString().Substring(4, 4)) >= DateTime.Now.Date)
                                return true;
                            break;
                        

                    case 3:

                       
                            Elgin32.Int_Retorno = Elgin32.Elgin_FlagsFiscais(ref informacao);
                            if (informacao == 8)
                                return true;
                            break;
                       
                    case 4:
                        
                            return false;
                       



                }
            }
            else
            {
                try
                {
                    ecfAcbr.DadosUltimaReducaoZ();

                    if (Convert.ToDateTime(ecfAcbr.DadosReducaoZClass.DataDoMovimento.ToString()) == DateTime.Now.Date)
                        return true;
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }
            }
            return false;
        }

        public static string ultimaReducaoZ(string relatorio)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return "";

            
            if (GlbVariaveis.glb_Acbr == false)
            {
                #region
                string informacao = new string(' ', 1278);

                switch (ConfiguracoesECF.idECF)
                {
                    case 1:

                        int iRetorno = BemaFI32.Bematech_FI_DadosUltimaReducao(ref informacao);
                        
                        string[] dadosB = informacao.Split(',');
                        if (informacao == "")
                            return "";

                        if (relatorio == "COO")
                        {
                            if (dadosB.Length > 9)
                                return dadosB[10];
                            else
                                return "";
                        }

                        return "";

                    case 2:

                        StringBuilder retorno = new StringBuilder(1209);

                        DARUMA_FW.iRetorno = DARUMA_FW.rRetornarDadosReducaoZ_ECF_Daruma(retorno);
                        string[] dadosD = retorno.ToString().Split(';');

                        if (retorno.ToString() == "")
                            return "";

                        if (dadosD.Length > 30)
                            return dadosD[30];
                        return "";
                        

                    case 3:

                        Elgin32.Elgin_DadosUltimaReducao(ref informacao);
                        string[] dadosE = informacao.Split(',');
                        if (informacao == "")
                            return "";

                        if (dadosE.Length > 9)
                            return dadosE[10];
                        else
                            return "";



                }
            #endregion
            }
            else
            {
                try
                {
                    ecfAcbr.DadosUltimaReducaoZ();

                    if (relatorio == "COO")
                    {
                        return ecfAcbr.DadosReducaoZClass.COO.ToString();
                    }
                    else if (relatorio == "CRZ")
                    {
                        return ecfAcbr.DadosReducaoZClass.CRZ.ToString();
                    }
                    else if (relatorio == "CRO")
                    {
                        return ecfAcbr.DadosReducaoZClass.CRO.ToString();
                    }
                    else if (relatorio == "VendaBruta")
                    {
                        return ecfAcbr.DadosReducaoZClass.ValorVendaBruta.ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch(Exception erro)
                {
                    MessageBox.Show("não foi possivel capturar ultima redução Z", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            return "";

        }

        public static void DavOSFinalizados()
        {
           if (GlbVariaveis.glb_TipoPAF != GlbVariaveis.tipoPAF.Serviço.ToString() )
           {
               return;
           }

           if (ConfiguracoesECF.idNFC > 0)
               return ;


             siceEntities entidade = Conexao.CriarEntidade();

            var dados = from n in entidade.contdocs
                        where n.data >= GlbVariaveis.Sys_Data.Date
                        && n.davnumero > 0
                        select new
                        {
                            numeroFabECF = n.ecffabricacao,
                            MFAdicional = n.ecfMFadicional,
                            tipoECF = n.ecftipo,
                            marcaECF = n.ecfmarca,
                            modeloECF = n.ecfmodelo,
                            COO = n.ncupomfiscal, // O número do cupom fiscal COO
                            CCF = n.ecfcontadorcupomfiscal,
                            DAVNumero = n.davnumero,
                            data = n.data,
                            titulo = "DAV OS",
                            valor = n.total,
                            COOVinculado = n.contadordebitocreditoCDC,
                            ead = n.EADRegistroDAV
                        };

            if (dados.Count()==0)
                return;

                FuncoesECF.RelatorioGerencial("abrir", "");
                string conteudo = "";
                foreach (var item in dados)
                {
                    conteudo = item.DAVNumero.ToString()+"  "+ string.Format("{0:n2}", item.valor) + Environment.NewLine;
                    FuncoesECF.RelatorioGerencial("imprimir", conteudo);                    
                }
                FuncoesECF.RelatorioGerencial("fechar", "");

        }

        public static bool GravarDadosReducaoAnterior()
        {

            if (ConfiguracoesECF.idNFC > 0)
                return true;


            if (ConfiguracoesECF.idECF == 0)
                return true;

            string data = "";
            string data2 = "";

            try
            {
                DateTime dataReducaoZ = FuncoesECF.DataUltimaReduzacaoZ().Date; // verificar se essa data é a mesma do mevimento ou a data da emissão da reducao Z
                data = dataReducaoZ.ToString();
                var dadoDataReducaoZ = (from n in Conexao.CriarEntidade().r02
                                        where n.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                        orderby n.data descending
                                        select n.data);

                data2 = dadoDataReducaoZ.ToString();



               /*if (!dadoDataReducaoZ.FirstOrDefault().HasValue)
                {
                    MessageBox.Show("1 - false");
                    return false;
                }*/

                if (dadoDataReducaoZ.FirstOrDefault().HasValue)
                {

                    if (dataReducaoZ == dadoDataReducaoZ.FirstOrDefault())
                    {

                        return true;
                    }
                }

            }
            catch (Exception erro)
            {
                MessageBox.Show("Aviso2 dataReducaoZ = " + data + " dadoDataReducaoZ = " + data2 + erro.ToString());
            }
                
                try
                {

                    Paf paf = new Paf();
                    paf.GravarRelatorioR(true);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                            
            return true;
        }

        public static bool GerarCat52(string arquivoOrigem, DateTime dataInicial, DateTime dataFinal, string arquivoDestino)
        {
            if (ConfiguracoesECF.idNFC > 0)
                return true;


            if (GlbVariaveis.glb_Acbr == false)
            {
                switch (ConfiguracoesECF.idECF)
                {
                    case 1:
                        //if (DataUltimaReduzacaoZ().Date == GlbVariaveis.Sys_Data.Date)
                        //   return true;

                        if (File.Exists(@ConfiguracoesECF.pathRetornoECF + "download.mfd"))
                            arquivoOrigem = @ConfiguracoesECF.pathRetornoECF + "download.mfd";


                        int iRetorno = BemaFI32.Bematech_FI_GeraRegistrosCAT52MFDEx(arquivoOrigem, string.Format("{0:dd/MM/yyyy}", dataInicial), ref arquivoDestino);

                        if (!Directory.Exists("CAT52"))
                            Directory.CreateDirectory("CAT52");
                        if (File.Exists(arquivoDestino + "D"))
                        {
                            if (File.Exists(@"CAT52\" + arquivoDestino.ToLower().Replace(@"c:\iqsistemas\", "")))
                            {
                                File.Delete(@"CAT52\" + arquivoDestino.ToLower().Replace(@"c:\iqsistemas\", ""));
                            }
                            File.Move(arquivoDestino + "D", @"CAT52\" + arquivoDestino.ToLower().Replace(@"c:\iqsistemas\", ""));
                        }


                        if (iRetorno == 1)
                            return true;
                        MessageBox.Show(iRetorno.ToString());

                        return false;
                    case 2:
                        break;
                    case 3:
                        break;
                }
            }
            else
            {
                try
                {
                    ecfAcbr.PafMF_GerarCAT52(dataInicial, dataFinal, arquivoDestino);
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message);
                }
            }
            return false;
        }

        public static bool GerarSPEDECF(string arquivoOrigem, DateTime dataInicial, DateTime dataFinal)
        {

            if (ConfiguracoesECF.idNFC > 0)
                return true;


            var dadosEmpresa = (from n in Conexao.CriarEntidade().filiais
                              where n.CodigoFilial== GlbVariaveis.glb_filial
                              select new {n.empresa, n.estado,n.cidade}).First();
            string codMunicipio= Funcoes.RetornaCodigoMunIBGE("",dadosEmpresa.cidade,dadosEmpresa.estado);

            var aliquotasPISCOFINS = (from n in Conexao.CriarEntidade().configfinanc
                                      where n.CodigoFilial == GlbVariaveis.glb_filial
                                      select new { n.pis, n.cofins }).First();
            string arquivoDestino = @"c:\iqsistemas\SPED_ECF_" + ConfiguracoesECF.nrFabricacaoECF + ".txt";
            switch (ConfiguracoesECF.idECF)
            {
                case 1:
                    //if (DataUltimaReduzacaoZ().Date == GlbVariaveis.Sys_Data.Date)
                    //   return true;

                    if (File.Exists(@ConfiguracoesECF.pathRetornoECF + "download.mfd"))
                        arquivoOrigem = @ConfiguracoesECF.pathRetornoECF + "download.mfd";


                    int iRetorno = BemaFI32.Bematech_FI_GeraRegistrosSpedCompleto(arquivoOrigem,arquivoDestino, string.Format("{0:dd/MM/yyyy}", dataInicial), string.Format("{0:dd/MM/yyyy}", dataFinal), "A", "5929", " ", "0" + string.Format("{0:N2}", aliquotasPISCOFINS.pis.Value),"0"+ string.Format("{0:N2}", aliquotasPISCOFINS.cofins.Value), dadosEmpresa.empresa, codMunicipio);



                    if (iRetorno == 1)
                        return true;
                    else
                        MessageBox.Show("Retorno do ECF: " + iRetorno.ToString());


                    return false;
                case 2:
                    break;
                case 3:
                    break;
            }
            return false;
        }

        public static bool VerificarR02(string tipo)
        {
            if (GlbVariaveis.glb_clienteDAV == false)
                return true;

            if (Conexao.ConexaoOnline() == false)
                return true;

            if (ConfiguracoesECF.pdv == false)
                return true;

            if (ConfiguracoesECF.idECF == 0)
                return true;

            if (ConfiguracoesECF.idNFC > 0)
                return true;


            if (GlbVariaveis.glb_Acbr == true)
            {
                if (tipo == "CRO")
                {
                    var R02 = (from r in Conexao.CriarEntidade().r02
                               where r.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                   && r.codigofilial == GlbVariaveis.glb_filial
                               orderby r.id descending
                               select r.cro).FirstOrDefault();

                    if (R02 != null)
                    {
                        int CRO = int.Parse(ultimaReducaoZ("CRO"));
                        if (CRO < int.Parse(R02))
                        {
                            FuncoesECF.RecomporArquivoCadastroECF(false, true, true);
                            return false;
                        }
                        else if (CRO > int.Parse(R02))
                        {
                            FuncoesECF.RecomporArquivoCadastroECF(false, true, false);
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    return true;
                }
                else if (tipo == "CRZ")
                {
                    var R02 = (from r in Conexao.CriarEntidade().r02
                               where r.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                   && r.codigofilial == GlbVariaveis.glb_filial
                               orderby r.id descending
                               select r.crz).FirstOrDefault();

                    if (R02 != null)
                    {
                        int CRZ = int.Parse(ultimaReducaoZ("CRZ"));
                        if (CRZ != int.Parse(R02))
                        {
                            FuncoesECF.RecomporArquivoCadastroECF(false, true, false);
                            return false;
                        }
                        else
                        {
                            return true;
                        }


                    }
                }
                else if (tipo == "VendaBruta")
                {
                    var R02 = (from r in Conexao.CriarEntidade().r02
                               where r.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                   && r.codigofilial == GlbVariaveis.glb_filial
                               orderby r.id descending
                               select r.vendabrutadiaria).FirstOrDefault();

                    if (R02 > 0)
                    {
                        decimal vendaBruta = decimal.Parse(ultimaReducaoZ("VendaBruta"));
                        if (vendaBruta != R02)
                        {
                            FuncoesECF.RecomporArquivoCadastroECF(false, true, false);
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            return true;
        }
    }


    public struct StructRespotasTEF
    {
        public string arquivo;
        public bool transacaoConfirmada; // true false        
    }

    public class formaPagamentoAcbr
    {
        public string descricao { get; set; }
        public string codigo { get; set; }
        public bool vinculado { get; set; }
    }


}
