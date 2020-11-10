using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq;
using System.Diagnostics;
namespace SICEpdv
{  
    /// <summary>
    /// Atenção: Todos os nomes de arquivos devem ser maiúsculos. Existe sensitive case em nome de arquivos    
    /// </summary>
    public class TEF    {             
        public static string PathReq = "C:\\tef_dial\\REQ\\"; 
        public static string PathResp = "C:\\tef_dial\\RESP\\";
        
        public static string arquivoTmpTEF = "C:\\IQSISTEMAS\\TEFTEMP.TXT";
        public static string arquivoBackupTEF = @"C:\IQSISTEMAS\INTPOS.BKP";
        public static string arquivoImpressaoTEF = "C:\\IQSISTEMAS\\ImpressaoTEF.TXT";

        public static string pathTransacoesTEF = @"c:\IQSISTEMAS\TEF";
        public static string pathTransacoesTEFConfirmadas = @"c:\IQSISTEMAS\TEF\CONFIRMADAS";

        // Variavéis que precisam serem zeradas no cancelamento e na confirmação
        public static string mensagemOperador = "";
        public static string numeroAutorizacao = "";
        public static List<string> espelhoTEF = new List<string>();        
        public static string transacaoAprovada = ""; //0 = Aprovada 1 = Reprovada 3 = Transacao Diferente da Anterior
        public static decimal valorAprovadoTEF = 0;
        public static decimal valorDescontoTEF = 0;
        public static decimal valorSaqueTEF = 0;
        public static int idTransacao = 0;
        public static List<pagamentos> lstPagamento = new List<pagamentos>();
        public static int numeroVias = 2;

        
        //Mensagems
        public static string mensagemGerenciadorInativo = "Gerenciador não está ativo. Favor ativá-lo";
        
        [DllImport("kernel32")]       
        public static extern void Sleep(int dwMilliseconds);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        public TEF()        
        {
        
        }        
        
        //EXECUTA TRANSACAO TEF        
        public static int TransacaoTEF(string tipo,string NumeroCupom, string cValorPago)
        {
            if (!ConfiguracoesECF.tefDedicado && !ConfiguracoesECF.tefDiscado)
                return 0;

            if (GlbVariaveis.glb_TEFAcbr == false)
            {
                #region

                if (tipo == "" || tipo == null)
                    tipo = "CRT";
                // Tipo = "CRT" Para Cartao
                // Tipo = "CHQ" PAra Cheque
                transacaoAprovada = "1";
                // 0 = Aprovada
                // 1 = Reprovada
                // 3 = Identicacao diferente apaga arquivo e fica em loop
                string cIdentificacao = Convert.ToDateTime(DateTime.Now.ToString()).ToString("HHmmss");
                bool bTransacao = false,
                    Imprime = false,
                    imprime710 = false,
                    imprime712 = false,
                    imprime714 = false;


                string Valor = "", NomeRede = "", Cupom = "", Ret = "", valorDesconto, valorSaque;
                try
                {
                    string String_Tef;
                    string conteudo711, conteudo713, conteudo715;


                    String_Tef = "000-000 = " + tipo.Trim() + "\r\n";
                    String_Tef += "001-000 = " + cIdentificacao + "\r\n";
                    String_Tef += "002-000 = " + NumeroCupom + "\r\n";
                    String_Tef += "003-000 = " + cValorPago + "\r\n";
                    String_Tef += "004-000 = 0\r\n";
                    if (tipo == "CHQ")
                    {
                        string tipoPessoa = "F";

                        if (Venda.dadosCheque.cpfCheque.Trim().Length > 11)
                            tipoPessoa = "J";

                        String_Tef += "006-000 = " + tipoPessoa + "\r\n";
                        String_Tef += "007-000 = " + Venda.dadosCheque.cpfCheque + "\r\n";
                        String_Tef += "033-000 = " + Venda.dadosCheque.codBanco + "\r\n";
                        String_Tef += "034-000 = " + Venda.dadosCheque.agencia + "\r\n";
                        String_Tef += "036-000 = " + Venda.dadosCheque.conta + "\r\n";
                        String_Tef += "038-000 = " + Venda.dadosCheque.numeroCheque + "\r\n";
                    }

                    String_Tef += "701-000 = " + GlbVariaveis.nomeAplicativo + " " + GlbVariaveis.versaoPAF + "\r\n";
                    String_Tef += "706-000 = 3\r\n";
                    String_Tef += "716-000 = " + GlbVariaveis.razaoSH + "\r\n";
                    String_Tef += "777-777 = TESTE REDECARD\r\n";
                    String_Tef += "999-999 = 0";
                    for (int iTentativas = 0; iTentativas <= 7; iTentativas++)
                    {
                        if ((CriaArquivoTEF(String_Tef) == true))
                        {
                            break;
                        }
                        Sleep(1000);
                        if (iTentativas == 7)
                        {
                            throw new Exception(mensagemGerenciadorInativo);
                        }
                    }

                    while ((File.Exists(PathResp + @"\INTPOS.001")) == false) ;
                    if (File.Exists(PathResp + @"\INTPOS.001"))
                    {
                        Sleep(1000);
                        File.Copy(PathResp + @"\INTPOS.001", arquivoBackupTEF, true);

                        while ((File.Exists(arquivoBackupTEF)) == false) ;
                        FileStream ArquivoTef = new FileStream(arquivoBackupTEF, FileMode.Open);
                        StreamReader Ler = new StreamReader(ArquivoTef);
                        String_Tef = "";
                        conteudo711 = "";
                        conteudo713 = "";
                        conteudo715 = "";

                        string Texto_Tef = Ler.ReadLine();

                        while (Texto_Tef != null)
                        {
                            #region
                            if (string.Compare((Texto_Tef.Substring(0, 3)), "001") == 0)
                            {
                                //if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), cIdentificacao) == 0)
                                if (Texto_Tef.Substring(10, Texto_Tef.Length - 10) != cIdentificacao)
                                {
                                    transacaoAprovada = "1";
                                    File.Delete(@PathResp + @"\INTPOS.001");
                                    return 3;
                                    //SE O ARQUIVO DE RETORNO O CAMPO VALOR E IDENTIFICACAO FOR DIFERENTE RETORNE E CANCELE O PENDENTE                                                               
                                }
                            }
                            if (string.Compare((Texto_Tef.Substring(0, 3)), "003") == 0)
                            {
                                Valor = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                                FormatarValor(Valor);
                            }

                            if (string.Compare((Texto_Tef.Substring(0, 3)), "708") == 0)
                            {
                                valorSaque = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                                FormatarSaque(valorSaque);
                            }


                            if (string.Compare((Texto_Tef.Substring(0, 3)), "709") == 0)
                            {
                                valorDesconto = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                                FormatarDesconto(valorDesconto);
                            }


                            if (string.Compare((Texto_Tef.Substring(0, 3)), "009") == 0)
                            {
                                Ret = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                                if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") == 0)
                                {
                                    transacaoAprovada = "0";
                                    bTransacao = true;
                                }
                                if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") == 1)
                                {
                                    transacaoAprovada = "1";
                                    bTransacao = false;
                                }
                            }
                            if (string.Compare((Texto_Tef.Substring(0, 3)), "010") == 0)
                            {
                                NomeRede = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                            }
                            if (string.Compare((Texto_Tef.Substring(0, 3)), "012") == 0)
                            {
                                Cupom = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                            }
                            if (string.Compare((Texto_Tef.Substring(0, 3)), "028") == 0)
                            {
                                if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") != 0)
                                {
                                    Imprime = true;
                                }
                            }

                            //// Novo para colocar o CIELO PREMIA  || string.Compare((Texto_Tef.Substring(0, 3)), "710") == 0 || string.Compare((Texto_Tef.Substring(0, 3)), "712") == 0 || string.Compare((Texto_Tef.Substring(0, 3)), "714") == 0  

                            if (string.Compare((Texto_Tef.Substring(0, 3)), "710") == 0)
                            {
                                if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") != 0)
                                {
                                    Imprime = true;
                                    imprime710 = true;
                                }
                            }


                            if (string.Compare((Texto_Tef.Substring(0, 3)), "712") == 0)
                            {
                                if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") != 0)
                                {
                                    Imprime = true;
                                    imprime712 = true;
                                }
                            }



                            if (string.Compare((Texto_Tef.Substring(0, 3)), "714") == 0)
                            {
                                if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") != 0)
                                {
                                    Imprime = true;
                                    imprime714 = true;
                                }
                            }

                            if (string.Compare((Texto_Tef.Substring(0, 3)), "029") == 0) //// Novo para colocar o CIELO PREMIA || string.Compare((Texto_Tef.Substring(0, 3)), "711") == 0 || string.Compare((Texto_Tef.Substring(0, 3)), "713") == 0 || string.Compare((Texto_Tef.Substring(0, 3)), "715") == 0
                            {

                                //if ((Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() == "")
                                //{
                                //    String_Tef += " " + "\r\n";
                                //}
                                if (Texto_Tef.Length > 12)
                                    String_Tef += (Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n";
                                else
                                    String_Tef += (Texto_Tef.Substring(Texto_Tef.Length, Texto_Tef.Length - Texto_Tef.Length)).ToString() + "\r\n";
                            }


                            //// Novo para colocar o CIELO PREMIA || string.Compare((Texto_Tef.Substring(0, 3)), "711") == 0 || string.Compare((Texto_Tef.Substring(0, 3)), "713") == 0 || string.Compare((Texto_Tef.Substring(0, 3)), "715") == 0

                            if (string.Compare((Texto_Tef.Substring(0, 3)), "711") == 0)
                            {

                                //if ((Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() == "")
                                //{
                                //    String_Tef += " " + "\r\n";
                                //}
                                if (Texto_Tef.Length >= 11)
                                {
                                    String_Tef += (Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n";
                                    conteudo711 += (Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n";
                                }
                                else
                                {
                                    String_Tef += (Texto_Tef.Substring(10, Texto_Tef.Length - 10)).ToString() + "\r\n";
                                    conteudo711 += (Texto_Tef.Substring(10, Texto_Tef.Length - 10)).ToString() + "\r\n";
                                }

                            }


                            if (string.Compare((Texto_Tef.Substring(0, 3)), "713") == 0)
                            {

                                //if ((Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() == "")
                                //{
                                //    String_Tef += " " + "\r\n";
                                //}
                                if (Texto_Tef.Length >= 11)
                                {
                                    String_Tef += (Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n";
                                    conteudo713 += (Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n";
                                }
                                else
                                {
                                    String_Tef += (Texto_Tef.Substring(10, Texto_Tef.Length - 10)).ToString() + "\r\n";
                                    conteudo713 += (Texto_Tef.Substring(10, Texto_Tef.Length - 10)).ToString() + "\r\n";
                                }
                            }


                            if (string.Compare((Texto_Tef.Substring(0, 3)), "715") == 0)
                            {

                                //if ((Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() == "")
                                //{
                                //    String_Tef += " " + "\r\n";
                                //}
                                if (Texto_Tef.Length >= 11)
                                {
                                    String_Tef += (Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n";
                                    conteudo715 += (Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n";
                                }
                                else
                                {
                                    String_Tef += (Texto_Tef.Substring(10, Texto_Tef.Length - 10)).ToString() + "\r\n";
                                    conteudo715 += (Texto_Tef.Substring(10, Texto_Tef.Length - 10)).ToString() + "\r\n";
                                }
                            }

                            if (string.Compare((Texto_Tef.Substring(0, 7)), "030-000") == 0)
                            {
                                //MENSAGEM DO CAMPO 30  
                                mensagemOperador = Texto_Tef.Substring(10, Texto_Tef.Length - 10) + "\r\n";

                                bool lerDados = false;

                                try
                                {
                                    Convert.ToInt64(numeroAutorizacao);
                                }
                                catch (Exception erro)
                                {
                                    lerDados = true;
                                }

                                if (lerDados == true)
                                    numeroAutorizacao = mensagemOperador.Replace("AUTORIZADA", "").Replace("autorizado", "").Replace("autoriz", "").Replace("AUTORIZ", "").Replace(":", "").Replace("\r", "").Replace("\n", "");


                                //if (Cupom.Trim() != "")
                                //{
                                //    mensagemOperador += "Rede : " + NomeRede + "\r\n";                                
                                //    mensagemOperador += "Cupom: " + Cupom + "\r\n";                             
                                //    mensagemOperador += "Valor: " + String.Format("{0:N2}", Convert.ToDecimal(0 + Valor) / 100) + "\r\n";
                                //}
                            }

                            if (string.Compare((Texto_Tef.Substring(0, 7)), "013-000") == 0)
                            {
                                numeroAutorizacao = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                            }

                            #endregion
                            Texto_Tef = Ler.ReadLine();
                        }
                        Ler.Close();
                        //CRIA ARQUIVO DE IMRESSAO
                        if (String_Tef != "")
                        {
                            #region
                            TEF.numeroVias = 2;
                            string conteudoImpressaoTEF = String_Tef;

                            if (imprime710 == true)
                            {
                                numeroVias = 1;
                                conteudoImpressaoTEF = conteudo711 + Environment.NewLine + Environment.NewLine + conteudo715;
                                if (!imprime714)
                                    conteudoImpressaoTEF += Environment.NewLine + Environment.NewLine + String_Tef;
                            }
                            if (imprime712 == true)
                            {
                                numeroVias = 1;
                                conteudoImpressaoTEF = conteudo713 + Environment.NewLine + Environment.NewLine + conteudo715;
                                if (!imprime714)
                                    conteudoImpressaoTEF += Environment.NewLine + Environment.NewLine + String_Tef;
                            }


                            FileStream Arquivo = new FileStream(@arquivoImpressaoTEF, FileMode.Create);
                            StreamWriter Escreve_Tef_Bkp = new StreamWriter(Arquivo);
                            Escreve_Tef_Bkp.Write(conteudoImpressaoTEF);
                            Escreve_Tef_Bkp.Close();
                            #endregion
                        }
                        if (bTransacao == true)
                        {
                            #region
                            //idTransacao = Convert.ToInt32(cIdentificacao);
                            File.Copy(arquivoBackupTEF, @pathTransacoesTEF + @"\INTPOS" + cIdentificacao + ".BKP", true);
                            if (Imprime == true)
                            {
                                //Cria o list dos pagamentos
                                pagamentos pag;
                                pag.formaPagamento = ConfiguracoesECF.CA;  // "Cartão";
                                if (tipo == "CHQ")
                                    pag.formaPagamento = "Cheque";

                                pag.valor = valorAprovadoTEF;
                                lstPagamento.Add(pag);

                                File.Copy(@arquivoImpressaoTEF, @pathTransacoesTEF + @"\IMP" + cIdentificacao + ".TXT", true);
                                //frmImpressaoTEF frm = new frmImpressaoTEF();
                                //frm.lblMensagem.Text = mensagemOperador;
                                //frm.Show();
                                //frm.bSaidaAutomatica = true;
                                //frm.botaoOkVisivel = false;
                                //frm.ShowDialog(); 
                                Sleep(1000);

                            }
                            else
                            {
                                return 0;
                                //? Atenção: Retirado para o TEF Dedicado
                                //?transacaoAprovada = "1";
                                //?throw new Exception(mensagemOperador);
                            }
                            #endregion
                            //SE ATRANSACAO FOR OK RETORNE 1
                            transacaoAprovada = "0";
                            return 0;
                        }
                        else
                        {
                            //frmOperadorTEF frm = new frmOperadorTEF(mensagemOperador);
                            //frm.ShowDialog();

                            //frm.bSaidaAutomatica = false;
                            //frm.botaoOkVisivel = true;
                            //frm.ShowDialog();
                            //SE ATRANSACAO NAO FOR OK RETORNE 2 
                            transacaoAprovada = "1";
                            return 1;
                        }
                    }
                    else
                    {
                        transacaoAprovada = "1";
                        return 2;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("O TEF Apresentou Erro! Tente Novamente." + ex.Message + " " + ex, "IQ Sistemas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 2;
                }
                #endregion
            }
            else
            {
                //acbr desenvolver
                return 2;
            }
        }

        private static void FormatarValor(string Valor)
        {
            try
            {
                if (!Valor.Contains(','))
                    valorAprovadoTEF = Convert.ToDecimal(String.Format("{0:N2}", Convert.ToDecimal(0 + Valor) / 100));
                else
                    valorAprovadoTEF = Convert.ToDecimal(String.Format("{0:N2}", Convert.ToDecimal("0" + Valor.Trim() )));
            }
            catch
            {
                throw new Exception("Erro ao converter valor para decimal: " + Valor);
            }
        }

        private static void FormatarDesconto(string Valor)
        {
            try
            {
                if (!Valor.Contains(','))
                    valorDescontoTEF = Convert.ToDecimal(String.Format("{0:N2}", Convert.ToDecimal(0 + Valor) / 100));
                else
                    valorDescontoTEF = Convert.ToDecimal(String.Format("{0:N2}", Convert.ToDecimal("0" + Valor.Trim())));
            }
            catch
            {
                throw new Exception("Erro ao converter valor desconto para decimal: " + Valor);
            }
        }

        private static void FormatarSaque(string Valor)
        {
            try
            {
                if (!Valor.Contains(','))
                    valorSaqueTEF = Convert.ToDecimal(String.Format("{0:N2}", Convert.ToDecimal(0 + Valor) / 100));
                else
                    valorSaqueTEF = Convert.ToDecimal(String.Format("{0:N2}", Convert.ToDecimal("0" + Valor.Trim())));
            }
            catch
            {
                throw new Exception("Erro ao converter valor desconto para decimal: " + Valor);
            }
        } 

        //EXCLUI OS ARQUIVOS TEMPORARIOS        
        public static bool EncerraTEF()
        {
            if (GlbVariaveis.glb_TEFAcbr == true)
                return true;

            Sleep(3000);
            TEF.numeroVias = 2; // Para retornar a opção default para o conteúdo 029
            try            
            {                //ELIMINA OS ARQUIVOS UTILIZADOS NO TEF 
                if (File.Exists("C:\\IQSistemas\\EM_OP_TEF.TXT") == true)
                    File.Delete("C:\\IQSistemas\\EM_OP_TEF.TXT"); 
                try                
                {
                    if (File.Exists(PathResp + @"\INTPOS.001") == true)
                        File.Delete(@PathResp + @"\INTPOS.001");
                }                
                catch                
                {                   
                    //    
                }         
                try          
                {
                    if (File.Exists(PathReq + @"\INTPOS.001") == true)
                        File.Delete(@PathReq+@"\INTPOS.001");
                }                
                catch                
                {
                    //                
                }                
                try                
                {
                    if (File.Exists(@arquivoImpressaoTEF) == true)
                        File.Delete(@arquivoImpressaoTEF);
                }                
                catch                
                {                    
                    //                
                }                
                try                
                {                    
                    if (File.Exists(arquivoBackupTEF) == true)
                        File.Delete(arquivoBackupTEF);
                }                
                catch
                {                    
                    //
                }                
                try                
                {                    
                    if (File.Exists(@arquivoTmpTEF) == true)
                        File.Delete(@arquivoTmpTEF);
                }                
                catch                
                {                    
                    //
                }                       
                espelhoTEF.Clear();
                return true;            
            }            
            catch            
            {                
                return false;
            }        
        }        
        
        //CANCELA TRANSAÇÃO INFORMANDO OS DADOS DA MESMA        
        public static bool CancelaTransacao(string @transacaoOrigem)
        {
            if (GlbVariaveis.glb_TEFAcbr == false)
            {
                #region
                transacaoOrigem = transacaoOrigem.ToUpper();
                string cValor = "";
                string cNomeRede = "";
                string cNumeroDOC = "";
                string cData = "";
                string cHora = "";

                bool Imprime = false;
                // Lendo os dados da Transação aprovada
                espelhoTEF.Clear();
                FileStream arquivoTransacao = new FileStream(@transacaoOrigem, FileMode.Open);
                StreamReader dados = new StreamReader(arquivoTransacao);
                string textoTransacao = dados.ReadLine();
                while (textoTransacao != null)
                {

                    if (string.Compare((textoTransacao.Substring(0, 3)), "003") == 0)
                        cValor = textoTransacao.Substring(10, textoTransacao.Length - 10);
                    if (string.Compare((textoTransacao.Substring(0, 3)), "010") == 0)
                        cNomeRede = textoTransacao.Substring(10, textoTransacao.Length - 10);
                    if (string.Compare((textoTransacao.Substring(0, 3)), "012") == 0)
                        cNumeroDOC = textoTransacao.Substring(10, textoTransacao.Length - 10);
                    if (string.Compare((textoTransacao.Substring(0, 3)), "022") == 0)
                        cData = textoTransacao.Substring(10, textoTransacao.Length - 10);
                    if (string.Compare((textoTransacao.Substring(0, 3)), "023") == 0)
                        cHora = textoTransacao.Substring(10, textoTransacao.Length - 10);
                    textoTransacao = dados.ReadLine();
                };
                dados.Close();


                bool bTransacao = true;
                string Valor = "", NomeRede = "", Ret = "";
                string String_Tef;
                string cIdentificacao = Convert.ToDateTime(DateTime.Now.ToString()).ToString("HHmmss");
                String_Tef = "000-000 = CNC\r\n";
                String_Tef += "001-000 = " + cIdentificacao + "\r\n";
                // retirado conforme instrucao do Homologador
                //..um outro detalhe e que no comando de ncn e de cnf, vc esta mandando o campo 003-000, nos comandos de ncn e de cnf o campo 003-000 nao deve ser enviado..
                // String_Tef += "003-000 = " + cValor + "\r\n";
                String_Tef += "010-000 = " + cNomeRede + "\r\n";
                String_Tef += "012-000 = " + cNumeroDOC + "\r\n";
                String_Tef += "022-000 = " + cData + "\r\n";
                String_Tef += "023-000 = " + cHora + "\r\n";
                String_Tef += "999-999 = 0";
                for (int iTentativas = 0; iTentativas <= 7; iTentativas++)
                {
                    if ((CriaArquivoTEF(String_Tef) == true))
                    {
                        break;
                    }
                    Sleep(1000);
                    if (iTentativas == 7)
                    {
                        return false;
                    }
                }
                while ((File.Exists(PathResp + @"\INTPOS.001")) == false) ;
                if (File.Exists(PathResp + @"\INTPOS.001"))
                {
                    Sleep(1000);
                    File.Copy(PathResp + @"\INTPOS.001", arquivoBackupTEF, true);
                    FileStream ArquivoTef = new FileStream(arquivoBackupTEF, FileMode.Open);
                    StreamReader Ler = new StreamReader(ArquivoTef);
                    String_Tef = "";
                    string Texto_Tef = Ler.ReadLine();
                    while (Texto_Tef != null)
                    {
                        if (string.Compare((Texto_Tef.Substring(0, 3)), "001") == 0)
                        {
                            if (Texto_Tef.Substring(10, Texto_Tef.Length - 10) != cIdentificacao)
                            {
                                transacaoAprovada = "1";
                                File.Delete(@PathResp + @"\INTPOS.001");
                                return false;
                                //SE O ARQUIVO DE RETORNO O CAMPO VALOR E IDENTIFICACAO FOR DIFERENTE RETORNE E CANCELE O PENDENTE                                                               
                            }
                        }
                        if (string.Compare((Texto_Tef.Substring(0, 3)), "003") == 0)
                            Valor = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                        if (string.Compare((Texto_Tef.Substring(0, 3)), "010") == 0)
                            NomeRede = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                        if (string.Compare((Texto_Tef.Substring(0, 3)), "009") == 0)
                        {
                            if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") == 0)
                            {
                                idTransacao = Convert.ToInt32(cIdentificacao);
                                bTransacao = true;
                            }
                            if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") == 1)
                            {
                                bTransacao = false;
                            }
                        }

                        if (string.Compare((Texto_Tef.Substring(0, 3)), "028") == 0)
                        {
                            if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") != 0)
                            {
                                Imprime = true;
                            }
                        }
                        if (string.Compare((Texto_Tef.Substring(0, 3)), "029") == 0)
                        {
                            String_Tef += (Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n";
                            espelhoTEF.Add((Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n");
                        }
                        if (string.Compare((Texto_Tef.Substring(0, 7)), "030-000") == 0)
                        {
                            if (!Imprime)
                                bTransacao = false;
                            //frmMsgOperador frm = new frmMsgOperador();
                            //frm.bSaidaAutomatica = false; 
                            //frm.botaoOkVisivel = true;

                            //frm.Msg = Texto_Tef.Substring(10, Texto_Tef.Length - 10) + " - " + Ret;
                            //frm.Rede = NomeRede;
                            //frm.Nsu = cNumeroDOC;
                            //frm.Valor = Valor;
                            //frm.ShowDialog();

                            mensagemOperador = Texto_Tef.Substring(10, Texto_Tef.Length - 10) + " - " + Ret + "\r\n";
                            //if (NomeRede!="")
                            //    mensagemOperador += "Rede : " + NomeRede + "\r\n";
                            //if (cNumeroDOC != "")
                            //    mensagemOperador += "NSU  : " + cNumeroDOC + "\r\n";
                            //if (Valor!="")
                            //    mensagemOperador += "Valor: " + String.Format("{0:N2}", Convert.ToDecimal(0+Valor) / 100) + "\r\n";                                                                     
                        }
                        Texto_Tef = Ler.ReadLine();
                    }

                    Ler.Close();

                    if (!bTransacao)
                        return false;


                    FileStream Arquivo = new FileStream(@arquivoImpressaoTEF, FileMode.Create);
                    StreamWriter Escreve_Tef_Bkp = new StreamWriter(Arquivo);
                    Escreve_Tef_Bkp.Write(String_Tef);
                    Escreve_Tef_Bkp.Close();



                    File.Copy(arquivoBackupTEF, @pathTransacoesTEF + @"\INTPOS" + cIdentificacao + ".BKP", true);
                    File.Copy(@arquivoImpressaoTEF, @pathTransacoesTEF + @"\IMP" + cIdentificacao + ".TXT", true);

                    return true;
                }
                else
                {
                    return false;
                }

                #endregion
            }
            else
            {
                //acbr desenvolver
                return true;
            }
        }        
        
        
        //CONFIRMA TRANSAÇÃO PENDENTE        
        public static bool ConfirmaTef(string transacaoOrigem)
        {
            if (GlbVariaveis.glb_TEFAcbr == true)
                return true;
                           
            transacaoOrigem = transacaoOrigem.ToUpper(); 

            string Confirma; 
            string cIdentificacao = Convert.ToDateTime(DateTime.Now.ToString()).ToString("HHmmss");

            if (File.Exists(@transacaoOrigem) == true)
            {                
                Confirma = "000-000 = CNF\r\n";
                FileStream ArquivoTef = new FileStream(@transacaoOrigem, FileMode.Open);
                StreamReader Ler = new StreamReader(ArquivoTef);
                string Texto_Tef = Ler.ReadLine();
                while (Texto_Tef != null)
                {
                    // Para mudar a IDentificacao e nao pegar do arquivo
                    if (Texto_Tef.Substring(0, 7).Trim() == "001-000")
                    {
                        Confirma += "001-000 = " + cIdentificacao + "\r\n";
                    }

                    if (// Retirado conforme orientacao do homolgoaro string.Compare((Texto_Tef.Substring(0, 3)), "003") == 0 ||
                        string.Compare((Texto_Tef.Substring(0, 3)), "010") == 0 ||
                        string.Compare((Texto_Tef.Substring(0, 3)), "012") == 0 ||
                        string.Compare((Texto_Tef.Substring(0, 3)), "027") == 0 ||
                        string.Compare((Texto_Tef.Substring(0, 3)), "999") == 0 
                        )                        
                        Confirma += Texto_Tef + "\r\n"; 

                    Texto_Tef = Ler.ReadLine();
                }                
                Ler.Close();
                for (int iTentativas = 0; iTentativas <= 7; iTentativas++)
                {                    
                    if ((CriaArquivoTEF(Confirma) == true))
                    {
                        // Atenção: Foi mudado o sleep de 2000 para 500 para agilizar a finalizacao
                        Sleep(500); 
                        File.Move(@transacaoOrigem,@transacaoOrigem.Replace("TEF",@"TEF\CONFIRMADAS\"));
                        // Aqui a condição por que a Consulta de Cheque nem sempre retorna um arquivo para imprimir
                        if (File.Exists(@transacaoOrigem.Replace("INTPOS", "IMP").Replace(".BKP", ".TXT")))
                        File.Move(@transacaoOrigem.Replace("INTPOS", "IMP").Replace(".BKP", ".TXT"), @transacaoOrigem.Replace("TEF", @"TEF\CONFIRMADAS\").Replace("INTPOS", "IMP").Replace(".BKP", ".TXT"));                                                
                        break;                        
                    }                    
                    Sleep(500);
                    if (iTentativas == 7)
                    {
                        throw new Exception(mensagemGerenciadorInativo);
                    }
                }
                //bool iRetorno = FuncoesECF.RelatorioGerencial("Fechar", "");
                //? FuncoesECF.VerificaRetornoFuncaoImpressora(iRetorno);
                return true;
            }            
            else
            {                
                //MessageBox.Show("Sem transações pendentes a confirmar!", "Mensagem TEF", MessageBoxButtons.OK, MessageBoxIcon.Information);
               // return false;  
                throw new Exception("Arquivo de resposta não encontrado !!");
            }        
        }        
        
        //VERIFICA SE EXISTE UMA TRANSACAO DAI CANCELA         
        public static bool VerificaUltimaTransacao(string @transacaoOrigem)
        {
            if (GlbVariaveis.glb_TEFAcbr == true)
                return true;

            string Valor = "";
            string Transacao = "";
            string Rede = "";

            transacaoOrigem = transacaoOrigem.ToUpper();

            if (File.Exists(@transacaoOrigem) == true)
            {
                FileStream Arquivo = new FileStream(@transacaoOrigem, FileMode.Open);
                StreamReader Stream = new StreamReader(Arquivo);
                string Texto_Tef = Stream.ReadLine();
                while (Texto_Tef != null)
                {                    
                    //if (string.Compare((Texto_Tef.Substring(0, 3)), "000") == 0) 
                    //{                                               
                    //    if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "NCN") != 0)
                    //    {                            
                    //        //SE O ARQUIVO DE RETORNO O CAMPO VALOR E IDENTIFICACAO FOR DIFERENTE RETORNE E CANCELE O PENDENTE 
                    //        return false;
                    //        ;
                    //    }                    
                    //}                    
                    if (string.Compare((Texto_Tef.Substring(0, 3)), "003") == 0)
                    {                        
                        Valor = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                        FormatarValor(Valor);
                    }                    
                    if (string.Compare((Texto_Tef.Substring(0, 3)), "010") == 0)
                    {                        Rede = Texto_Tef.Substring(10, Texto_Tef.Length - 10);

                    }                    
                    if (string.Compare((Texto_Tef.Substring(0, 3)), "012") == 0)
                    {                        
                        Transacao = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                    }
                    Texto_Tef = Stream.ReadLine();
                }                
                Stream.Close();
                //File.Delete(@arquivoBackupTEF);
                //frmMsgOperador frm = new frmMsgOperador();
                //frm.bSaidaAutomatica = false;                
                //frm.botaoOkVisivel = true;                
                //frm.Msg = "Última Transação TEF foi Cancelada";
                //frm.Rede = Rede;                
                //frm.Nsu = Transacao;
                //frm.Valor = Valor;
                //frm.ShowDialog();

                mensagemOperador = "Última Transação TEF foi Cancelada!" + "\r\n";
                mensagemOperador += "Rede : " + Rede + "\r\n";
                mensagemOperador += "NSU  : " + Transacao + "\r\n";
                if (Valor!="")
                mensagemOperador += "Valor: " + String.Format("{0:N2}", valorAprovadoTEF);

                if (ConfiguracoesECF.tefDedicado)
                {
                    mensagemOperador = "Transação não efetuada. Favor reter o cupom";
                }

                return true;           
            }            
            return false;
        }        
        
        //CANCELA ULTIMA TRANSAÇÃO
        public static bool CancelaTef(string @transacaoOrigem)        
        {

            if (GlbVariaveis.glb_TEFAcbr == true)
                return true;


            if (transacaoOrigem == "")
                transacaoOrigem = PathResp + @"\INTPOS.001";

            transacaoOrigem = transacaoOrigem.ToUpper();
            string cIdentificacao = Convert.ToDateTime(DateTime.Now.ToString()).ToString("HHmmss");

            string Valor = "";
            string Transacao = "";
            string Rede = "";
            string Confirma;
            string IntPos = null;
            if (File.Exists(@transacaoOrigem) == true)
                IntPos = @transacaoOrigem;
            if (File.Exists(@transacaoOrigem) == true)
                IntPos = @transacaoOrigem;
            if (IntPos != null)
            {                
                Confirma = "000-000 = NCN\r\n";
                FileStream ArquivoTef = new FileStream(IntPos, FileMode.Open);
                StreamReader Ler = new StreamReader(ArquivoTef);
                string Texto_Tef = Ler.ReadLine();
                while (Texto_Tef != null)
                {     
                    
                    if (string.Compare((Texto_Tef.Substring(0, 3)), "003") == 0) 
                    {                        
                        Valor = Texto_Tef.Substring(10, Texto_Tef.Length - 10);                        
                    }                    
                    if (string.Compare((Texto_Tef.Substring(0, 3)), "010") == 0)
                    {                        
                        Rede = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                    }                    
                    if (string.Compare((Texto_Tef.Substring(0, 3)), "012") == 0) 
                    {                        
                        Transacao = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                    }

                    if (Texto_Tef.Substring(0, 7).Trim() == "001-000")
                    {
                        Confirma += "001-000 = " + cIdentificacao + "\r\n";
                    }
                    
                    // Retirado conforme instrução do Homologador em 16.10.2012
                    if (//string.Compare((Texto_Tef.Substring(0, 3)), "003") == 0 ||
                        string.Compare((Texto_Tef.Substring(0, 3)), "010") == 0 ||
                        string.Compare((Texto_Tef.Substring(0, 3)), "012") == 0 ||
                        string.Compare((Texto_Tef.Substring(0, 3)), "027") == 0 ||
                        string.Compare((Texto_Tef.Substring(0, 3)), "999") == 0
                        )                        
                        Confirma += Texto_Tef + "\r\n";
                    Texto_Tef = Ler.ReadLine();
                }                Ler.Close(); 
                if (Transacao == "") 
                    return false;
                for (int iTentativas = 0; iTentativas <= 7; iTentativas++)
                {                    
                    if ((CriaArquivoTEF(Confirma) == true))
                    {                        
                        break;
                    }
                    Sleep(1000);
                    if (iTentativas == 7)
                    {
                        throw new Exception("Gerenciador padrão não está ativo");                        
                        ;
                    }                
                }                
                //frmMsgOperador frm = new frmMsgOperador();
                //frm.bSaidaAutomatica = false;
                //frm.botaoOkVisivel = true;
                //frm.Msg = "Cancelada a Transação";
                //frm.Rede = Rede;
                //frm.Nsu = Transacao;
                //frm.Valor = Valor;
                //frm.ShowDialog();
                    mensagemOperador = "Cancelada a Transação !" + "\r\n";
                    mensagemOperador += "Rede : " + Rede + "\r\n";
                    mensagemOperador += "NSU  : " + Transacao + "\r\n";
                    if (Valor != "")
                    {
                        FormatarValor(Valor);
                        mensagemOperador += valorAprovadoTEF.ToString();
                    }

                if (ConfiguracoesECF.tefDedicado)
                {
                    mensagemOperador = "Transação não efetuada. Favor reter o cupom";
                }
                //frmOperadorTEF frm = new frmOperadorTEF(mensagemOperador);
                //frm.ShowDialog();                
                return true;
            }            
            else
            {                
                //MessageBox.Show("Sem transações pendentes a cancelar!", "Mensagem TEF", MessageBoxButtons.OK, MessageBoxIcon.Information);
                throw new Exception("Conteúdo do arquivo nulo");                
            }        
        }        
        
        //CRIA ARQUIVO DE TEF                
        public static bool CriaArquivoTEF(string Texto)
        {
            if (GlbVariaveis.glb_TEFAcbr == true)
                return true;

            if (!Directory.Exists(@PathReq))
                return false;
            try            
            {
                if (File.Exists(@arquivoTmpTEF) == true)
                    File.Delete(@arquivoTmpTEF);
                FileStream Arquivo = new FileStream(@arquivoTmpTEF, FileMode.Create);
                StreamWriter Escrever = new StreamWriter(Arquivo);
                Escrever.Write(Texto);                
                Escrever.Close();
                Sleep(500);
                File.Copy(@arquivoTmpTEF, PathReq + @"\INTPOS.001", true);
                Sleep(1000);
                return true;            
            }            
            catch (Exception erro)             
            {                
                MessageBox.Show(erro.Message+" Erro ao Criar ou Copiar Arquivo\r\nO Arquivo Pode Estar sendo Utilizado Pelo Gerenciador Padrao", "Mensagem TEF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }        
        }
        
        //VERIFICA STATUS DO GERENCIADOR               
        public static bool VerificaGerenciadorTEF()
        {            

            if (!ConfiguracoesECF.tefDedicado && !ConfiguracoesECF.tefDiscado)
                return true;

            if (GlbVariaveis.glb_TEFAcbr == false)
            {


                if (File.Exists(@PathResp + @"\INTPOS.STS"))
                    File.Delete(@PathResp + @"\INTPOS.STS");

                CriaArquivoTEF("000-000 = ATV\r\n" + "001-000 = " + Convert.ToDateTime(DateTime.Now.ToString()).ToString("HHmmss") + "\r\n999-999 = 0");

                // Retirado na Homologacao TEF 2m 17.10.2012
                //var processo = from n in Process.GetProcesses()
                //               where n.ProcessName.Contains("tef_dial")
                //               select n;
                //if (processo.Count() > 0)
                //    return true;


                for (int iTentativas = 0; iTentativas <= 7; iTentativas++)
                {
                    //if ((File.Exists(@PathResp + @"\\ATIVO.001") == true) && (File.Exists(@PathResp + @"\INTPOS.STS") == true))
                    if (File.Exists(@PathResp + @"\INTPOS.STS") == true)
                    {
                        File.Delete(@PathResp + @"\INTPOS.STS");
                        return true;
                    }
                    Sleep(1000);
                    if (iTentativas == 7)
                    {
                        //MessageBox.Show("O Gerenciador Padrão Não está Ativo!", "SOFTNET TEF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //System.Diagnostics.Process.Start(@"C:\\TEF_DIAL\\TEF_DIAL.EXE");
                        return false;
                    }
                }
            }
            else
            {
                // acbr
            }



            return false;
            
        }        
        
        //IMPRESSAO DO ARQUIVO TEF
        public static bool ImprimeTEF(string transacaoOrigem,string FormaPagamento, bool Gerencial)
        {
            if (GlbVariaveis.glb_TEFAcbr == true)
                return true;
                    
            //arquivoOrigem = INTPOS.BKP
            transacaoOrigem = transacaoOrigem.ToUpper();
            bool Imprime = false,imprimir710 = false, imprimir712= false, imprimir714 = false;
            if (File.Exists(@transacaoOrigem) == true)                
            {
                FileStream Arquivo = new FileStream(transacaoOrigem, FileMode.Open);
                StreamReader Stream = new StreamReader(Arquivo);
                string Texto_Tef = Stream.ReadLine();                
                //string Conteudo = ""; 
                while (Texto_Tef != null && Texto_Tef.Trim()!="")
                {                                        
                    if (string.Compare((Texto_Tef.Substring(0, 3)), "028") == 0 )  
                    { 
                        if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") == 1)
                        { 
                        Imprime = true; 
                        } 
                    }

                    if (string.Compare((Texto_Tef.Substring(0, 3)), "710") == 0)
                    {
                        if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") == 1)
                        {
                            Imprime = true;
                            imprimir710 = true;
                        }
                    }

                    if (string.Compare((Texto_Tef.Substring(0, 3)), "712") == 0)
                    {
                        if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") == 1)
                        {
                            Imprime = true;
                            imprimir712 = true;
                        }
                    }

                    if (string.Compare((Texto_Tef.Substring(0, 3)), "714") == 0)
                    {
                        if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") == 1)
                        {
                            Imprime = true;
                            imprimir714 = true;
                        }
                    }

                    TEF.numeroVias = 2;
                    
                    if (imprimir710 || imprimir712 || imprimir714 )
                        TEF.numeroVias = 1;


                    Texto_Tef = Stream.ReadLine();
                }                
                Stream.Close();
                if (Imprime == true)
                {
                    // Acessa agora o arquivo onde contém o conteúdo a ser impressao.
                    espelhoTEF.Clear();
                    FileStream Arquivo_Tef_Bkp = new FileStream(transacaoOrigem.Replace("INTPOS", "IMP").Replace("BKP", "TXT"), FileMode.Open);
                            StreamReader Ler = new StreamReader(Arquivo_Tef_Bkp);
                            Texto_Tef = Ler.ReadLine();
                            while (Texto_Tef != null) 
                            {
                                espelhoTEF.Add(Texto_Tef.Replace("\n","").Replace("\r",""));
                               // Conteudo = Conteudo + Texto_Tef + "\r\n";                                                                                           
                                Texto_Tef = Ler.ReadLine();

                            }
                            Ler.Close();
                }            
            }            
            return true;
        }        
        
        //FUNCOES ADMINISTRATIVA        
        public static int Administrativo()
        {
            if (GlbVariaveis.glb_TEFAcbr == false)
            {
                #region
                bool bTransacao = false,
                    Imprime = false,
                    imprime712 = false;
                string Valor = "", NomeRede = "", Cupom = "", Ret = "";

                string cIdentificacao = Convert.ToDateTime(DateTime.Now.ToString()).ToString("HHmmss");
                for (int iTentativas = 0; iTentativas <= 7; iTentativas++)
                {


                    if (CriaArquivoTEF("000-000 = ADM\r\n" +
                        "001-000 = " + cIdentificacao + "\r\n" +
                        "701-000 = " + GlbVariaveis.nomeAplicativo + " " + GlbVariaveis.versaoPAF + "\r\n" +
                        "706-000 = 3\r\n" +
                        "716-000 = " + GlbVariaveis.razaoSH + "\r\n" +
                        "999-999 = 0"
                        ) == true)
                    {
                        break;
                    }
                    Sleep(1000);
                    if (iTentativas == 7)
                    {
                        throw new Exception(mensagemGerenciadorInativo);
                        //return 2;  Como estava antes ao invés da Excpetion
                    }
                }
                while ((File.Exists(PathResp + @"\INTPOS.001")) == false) ;
                if (File.Exists(PathResp + @"\INTPOS.001") == true)
                {
                    System.IO.StreamWriter config = System.IO.File.CreateText(arquivoBackupTEF);
                    config.Close();

                    Sleep(1000);
                    File.Copy(PathResp + @"\INTPOS.001", arquivoBackupTEF, true);
                    while ((File.Exists(arquivoBackupTEF)) == false) ;
                    FileStream ArquivoTef = new FileStream(arquivoBackupTEF, FileMode.Open);
                    StreamReader Ler = new StreamReader(ArquivoTef);
                    string String_Tef = "",
                        conteudo713 = "",
                        conteudo715 = "";

                    string Texto_Tef = Ler.ReadLine();
                    while (Texto_Tef != null)
                    {
                        if (string.Compare((Texto_Tef.Substring(0, 3)), "001") == 0)
                        {
                            if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), cIdentificacao) == 1)
                            {
                                //SE O ARQUIVO DE RETORNO O CAMPO VALOR E IDENTIFICACAO FOR DIFERENTE RETORNE E CANCELE O PENDENTE  
                                return 2;
                            }
                        }
                        if (string.Compare((Texto_Tef.Substring(0, 3)), "003") == 0)
                        {
                            Valor = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                        }
                        if (string.Compare((Texto_Tef.Substring(0, 3)), "009") == 0)
                        {
                            Ret = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                            if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") == 0)
                            {
                                bTransacao = true;
                            }
                            if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") == 1)
                            {
                                bTransacao = false;
                            }
                        }
                        if (string.Compare((Texto_Tef.Substring(0, 3)), "010") == 0)
                        {
                            NomeRede = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                        }
                        if (string.Compare((Texto_Tef.Substring(0, 3)), "012") == 0)
                        {
                            Cupom = Texto_Tef.Substring(10, Texto_Tef.Length - 10);
                        }
                        if (string.Compare((Texto_Tef.Substring(0, 3)), "028") == 0)
                        {
                            if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") != 0)
                            {
                                Imprime = true;
                            }
                        }

                        if (string.Compare((Texto_Tef.Substring(0, 3)), "712") == 0)
                        {
                            if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") != 0)
                            {
                                Imprime = true;
                                imprime712 = true;
                            }
                        }



                        if (string.Compare((Texto_Tef.Substring(0, 3)), "714") == 0)
                        {
                            if (string.Compare((Texto_Tef.Substring(10, Texto_Tef.Length - 10)), "0") != 0)
                            {
                                Imprime = true;
                            }
                        }


                        if (string.Compare((Texto_Tef.Substring(0, 3)), "029") == 0)
                        {


                            if (Texto_Tef.Length > 12)
                            {
                                if ((Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() == "")
                                {
                                    String_Tef += " " + "\r\n";
                                }

                                String_Tef += (Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n";
                            }
                            else
                            {
                                String_Tef += (Texto_Tef.Substring(Texto_Tef.Length, Texto_Tef.Length - Texto_Tef.Length)).ToString() + "\r\n";
                            }
                        }


                        if (string.Compare((Texto_Tef.Substring(0, 3)), "713") == 0)
                        {

                            //if ((Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() == "")
                            //{
                            //    String_Tef += " " + "\r\n";
                            //}

                            String_Tef += (Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n";
                            conteudo713 += (Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n";
                        }


                        if (string.Compare((Texto_Tef.Substring(0, 3)), "715") == 0)
                        {

                            //if ((Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() == "")
                            //{
                            //    String_Tef += " " + "\r\n";
                            //}

                            String_Tef += (Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n";
                            conteudo715 += (Texto_Tef.Substring(11, Texto_Tef.Length - 12)).ToString() + "\r\n";
                        }



                        if (string.Compare((Texto_Tef.Substring(0, 7)), "030-000") == 0)
                        {
                            //MENSAGEM DO CAMPO 30
                            //frm.Msg = Texto_Tef.Substring(10, Texto_Tef.Length - 10) + " - " + Ret;
                            //frm.Rede = NomeRede;
                            //frm.Nsu = Cupom;
                            //frm.Valor = Valor;
                            mensagemOperador = Texto_Tef.Substring(10, Texto_Tef.Length - 10) + "\r\n";
                            //if (Cupom.Trim() != "")
                            //{
                            //    mensagemOperador += "Rede : " + NomeRede + "\r\n";
                            //    mensagemOperador += "NSU  : " + Cupom + "\r\n";
                            //    mensagemOperador += "Valor: " + String.Format("{0:N2}", Convert.ToDecimal(0 + Valor) / 100) + "\r\n";
                            //};
                        }
                        Texto_Tef = Ler.ReadLine();
                    }
                    Ler.Close();

                    //CRIA ARQUIVO DE IMRESSAO
                    string conteudoImpressaoTEF = String_Tef;

                    if (imprime712)
                        conteudoImpressaoTEF = conteudo713 + Environment.NewLine + Environment.NewLine + conteudo715;


                    FileStream Arquivo = new FileStream(@arquivoImpressaoTEF, FileMode.Create);
                    StreamWriter Escreve_Tef_Bkp = new StreamWriter(Arquivo);
                    Escreve_Tef_Bkp.Write(conteudoImpressaoTEF);
                    Escreve_Tef_Bkp.Close();
                    if (bTransacao == true)
                    {
                        idTransacao = Convert.ToInt32(cIdentificacao);
                        File.Copy(arquivoBackupTEF, @pathTransacoesTEF + @"\INTPOS" + cIdentificacao + ".BKP", true);
                        if (Imprime == true)
                        {
                            File.Copy(@arquivoImpressaoTEF, @pathTransacoesTEF + @"\IMP" + cIdentificacao + ".TXT", true);

                            //frm.bSaidaAutomatica = true; 
                            //frm.botaoOkVisivel = false;
                            //frm.ShowDialog();

                            // frmImpressaoTEF frm = new frmImpressaoTEF();
                            //  frm.lblMensagem.Text = mensagemOperador;
                            // frm.Show();
                        }
                        else
                        {
                            throw new Exception(mensagemOperador);
                            //frm.bSaidaAutomatica = false;
                            //frm.botaoOkVisivel = true;
                            //frm.ShowDialog();

                            //frmOperadorTEF frm = new frmOperadorTEF(mensagemOperador);
                            //frm.ShowDialog();
                        }
                        //SE ATRANSACAO FOR OK RETORNE 1
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    return 2;
                }

#endregion
            }
            else
            {
                return 2;
            }
            
        }

        public static int Transacoes(string acao)
        {
            if (GlbVariaveis.glb_TEFAcbr == false)
            {
                #region
                acao = acao.ToLower();
                var transacoesPendentes = Directory.GetFiles(TEF.PathResp);
                var transacoes = Directory.GetFiles(TEF.@pathTransacoesTEF);
                var transacoesConfirmadas = Directory.GetFiles(TEF.pathTransacoesTEFConfirmadas);
                switch (acao)
                {
                    // Retorna o número de transações aprovadas e pendentes de impressão
                    case "ntransacao":
                        var ntransacaoPendente = (from n in transacoesPendentes
                                                  where n.ToLower().Contains("intpos")
                                                  select n).ToList();

                        var ntransacao = (from n in transacoes
                                          where n.ToLower().Contains("intpos")
                                          select n).ToList();

                        return ntransacao.Count() + ntransacaoPendente.Count();

                    case "ntransacaoconfirmadas":
                        var ntransacaoConfirmadas = (from n in transacoesConfirmadas
                                                     where n.ToLower().Contains("intpos")
                                                     select n).ToList();
                        return ntransacaoConfirmadas.Count();

                    case "limpar":

                        foreach (var item in transacoes)
                        {
                            Sleep(100);
                            if (File.Exists(@item))
                                File.Delete(@item);
                        }
                        foreach (var item in transacoesConfirmadas)
                        {
                            Sleep(100);
                            if (File.Exists(@item))
                                File.Delete(@item);
                        }
                        TEF.lstPagamento.Clear();
                        TEF.espelhoTEF.Clear();
                        break;
                };
                #endregion
            }

            return 0;
        }

        public static void mensagensRetorno()
        {
            if (ConfiguracoesECF.tefDedicado)
                mensagemGerenciadorInativo = "Erro de comunicação - Client SiTef. Favor ativá-lo";
        }


        //tudo comerça aqui
        public static decimal ChamarGerenciador(string formaPagamento, decimal valor, int idCartao, string dpFinanceiro = "Venda")
        {


            //FrmMsgOperador msg = new FrmMsgOperador("", "Processando TEF! "+GlbVariaveis.glb_Usuario+ " o que você acha de tomar um cafezinho!");
            //msg.Show();
            //Application.DoEvents();


            if (ConfiguracoesECF.idECF == 0 || !ConfiguracoesECF.pdv)
            {
                //msg.Dispose();
                return 0;
            }
                  
            if (!TEF.VerificaGerenciadorTEF())
            {
                MessageBox.Show(TEF.mensagemGerenciadorInativo, "SICE.pdv", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //msg.Dispose();
                return 0;
            }

            // Confirma a transação anterior antes de solicitar uma nova
            if (TEF.Transacoes("ntransacao") > 0)
            {
                var arquivosRespostaTEF = FuncoesECF.ArquivosRespTransacaoTEF();

                for (int nEspelhos = 0; nEspelhos < arquivosRespostaTEF.Count(); nEspelhos++)
                {
                    TEF.ConfirmaTef(arquivosRespostaTEF[nEspelhos]);
                    TEF.EncerraTEF();
                }
            }

            string COOCupom = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF);

            var tipoTransacaoTEF = "CRT";

            if (formaPagamento == "CA")
                tipoTransacaoTEF = "CRT";

            if (formaPagamento == "CH")
            {
                tipoTransacaoTEF = "CHQ";

            }


            if (idCartao > 0)
            {
                Application.DoEvents();
                siceEntities entidade;

                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);
                else
                    entidade = Conexao.CriarEntidade();

                    
                var dadosCartoes = (from n in entidade.cartoes
                                    where n.id == idCartao
                                    select n).FirstOrDefault();

                tipoTransacaoTEF = dadosCartoes.transacao;
                if (!ConfiguracoesECF.tefDedicado)
                {
                    TEF.PathReq = dadosCartoes.pathreq;
                    TEF.PathResp = dadosCartoes.pathresp;
                };
            }

            if (GlbVariaveis.glb_Acbr == true)
            {
                if ((FuncoesECF.estadoECF() == ACBrFramework.ECF.EstadoECF.NaoFiscal) || (FuncoesECF.estadoECF() == ACBrFramework.ECF.EstadoECF.Relatorio))
                {
                    if (MessageBox.Show("ECF em estado 'Não Fiscal'! Deseja cancelar o impressão Pendete na ECF?", "Atenção", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        FuncoesECF.corrigirEstadoECF();
                    }
                    else
                    {
                        //msg.Dispose();
                        return 0;
                    }
                }
                
            }
            // Aqui para sempre ficar em loop caso o arquivo de resposta tenha identificacao diferente do arquivo de solicitacao
            // se Identificacao for diferente retorna 3
            while (TEF.TransacaoTEF(tipoTransacaoTEF, COOCupom, String.Format("{0:0.00}", valor).Replace(",", "")) == 3)
            {
                return 0;
            }

            
            if (TEF.transacaoAprovada != "0" && idCartao > 0)
            {

                FindWindow(null, "SICEpdv");
                TEF.EncerraTEF();
                frmOperadorTEF formOperadorTEF = new frmOperadorTEF(TEF.mensagemOperador, false);
                formOperadorTEF.ShowDialog();
                Application.DoEvents();
                formOperadorTEF.Dispose();
                if(dpFinanceiro == "Venda")
                    Venda.RetornoTEF = "Historico TEF = "+TEF.mensagemOperador;

                //msg.Dispose();
                return 0;
            }


            //msg.Dispose();

            // Aqui atualiza a variável valor para que seja dado troco caso tenha havido saque no Cartao
            valor = TEF.valorAprovadoTEF;
            return valor;




        }
    }

    public struct pagamentos
    {
        public string formaPagamento;
        public decimal valor;
    }
   
}