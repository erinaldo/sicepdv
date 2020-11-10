using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Xml.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using System.Diagnostics;
using System.Data.Objects;

namespace SICEpdv
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            try
            {
                // Aqui para colocar automaticamente no app.config a configuração do WebService do IQCARD para evitar suporte!
                // Dentro de um catch  para evitar erros
                Configuracoes.AcrescentarConfigIQCARD();                
            }
            catch (Exception )
            {
              
            }

            try
            {
                // Aqui para colocar automaticamente no app.config a configuração do WebService do SICE para mostrar o suporte
                // associado
                Configuracoes.AcrescentarConfigIQWSBinding();
            }
            catch (Exception)
            {
              
            }

            try
            {
                // Aqui para colocar automaticamente no app.config a configuração do WebService do SICE para mostrar os produtos
                // associado ao contador
                Configuracoes.AcrescentarConfigWSProdutosBinding();
            }
            catch (Exception)
            {

            }



            bool podeIniciar;            
            System.Threading.Mutex primeiraInstancia = new System.Threading.Mutex(true, "SICEpdv", out podeIniciar);

            var processo = from n in Process.GetProcesses()
                            where n.ProcessName.Contains("SICEpdv")     
                            && !n.ProcessName.Contains("SICEpdv.vs")
                            select n;

            if (processo.Count() > 1)
                podeIniciar = false;

            if (!podeIniciar)
            {
                MessageBox.Show("Já existe uma instância em execução !");
                return;
            }

            
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FrmMsgOperador msgEntrada = new FrmMsgOperador("", "A IQ Sistemas lhe deseja um bom trabalho. \r\n\r\n  Estamos iniciando a sua estação de trabalho.");
            msgEntrada.Show();
            Application.DoEvents();
            Configuracoes.configuracaoSetada = false;

            try
            {
                //MessageBox.Show(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"]);
                Configuracoes.capturarResolucao();

                if (@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] == "" || @ConfigurationManager.AppSettings["dirArquivoAuxiliar"] == null)
                {
                    //MessageBox.Show("sentado");
                    Configuracoes.setarAppConfig();
                }

                //MessageBox.Show(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"]);


                // Login do Sistema
                GlbVariaveis.glbSenhaIQ = "p@ssw0rd"; //ConfigurationManager.AppSettings["senhaIQ"];
                XDocument doc = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Terminal.xml");
                

                //ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Terminal.xml";

                var filial = (from n in doc.Descendants("Terminal").Elements("ECF")
                              select n).First();

                GlbVariaveis.glb_filial = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(filial.Element("filial").Value), GlbVariaveis.glbSenhaIQ);
                GlbVariaveis.glb_grupo = "";
                GlbVariaveis.glb_standalone = false;
                FuncoesPAFECF.DadosSoftwareHouse();


            }
            catch(Exception erro)
            {
                MessageBox.Show("1 - "+erro.ToString());
            }

            try
            {
                ConfiguracoesECF.lerXML();
            }
            catch (Exception erro)
            {
                //MessageBox.Show("2 - "+erro.ToString());
                ConfiguracoesECF.migrarXML();
                ConfiguracoesECF.lerXML();
            }

            if (!Conexao.VerificaConexaoDB())
            {
                MessageBox.Show("Modo de operação Standalone (Sem comunicação com a base de dados) ");
                try
                {
                    var testeConexao = (from n in Conexao.CriarEntidade().filiais
                                        select n.CodigoFilial).First();
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.Message.ToString() ?? "");
                    MessageBox.Show(erro.InnerException.ToString() ?? "");
                }                                
            }
            //if (!Conexao.onLine)
            //{
            //    MessageBox.Show("Modo de operação StandAlone (Sem comunicação com a base de dados)");
            //};
            msgEntrada.Dispose();

            if (!File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Terminal.xml"))
            {
                MessageBox.Show("Arquivo Terminal.xml não foi encontrado. Solicite suporte IQ Sistemas", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                System.Diagnostics.Process.Start("configPAF.exe");                    
                return;
            };

            Application.Run(new FrmLogon());
            if (!Operador.autorizado)
            {
                Application.Exit();
                return;
            };

            
            /// Inicilizando as Variavies Globais
            ///         


            GlbVariaveis.glb_IP = Funcoes.IDTerminal();
            FrmMsgOperador msgCNF = new FrmMsgOperador("", "1 - Carregando Configurações !");
            try
            {

                msgCNF.Show();
                Application.DoEvents();
                Configuracoes.carregar(GlbVariaveis.glb_filial);
                Configuracoes.VerificaSICECarga();
                if (Configuracoes.dataValidade <= DateTime.Now.Date)
                {

                    siceEntities entidade;
                    if (Conexao.tipoConexao == 2 && Conexao.ConexaoOnline() == false)
                        entidade = Conexao.CriarEntidade(false);
                    else
                        entidade = Conexao.CriarEntidade();
                }

                Configuracoes.verificaTerminalECF();              

                msgCNF.Dispose();
            }
            catch (Exception erro)
            {

                MessageBox.Show("1 - Carregando configurações: " + erro.Message);
            }
            finally
            {
                msgCNF.Dispose();
            }


            FrmMsgOperador msgPermissoes = new FrmMsgOperador("", "2 - Carregando permissões do usuário !");
            try
            {
                msgEntrada.Dispose();
                msgPermissoes.Show();
                Application.DoEvents();
                Permissoes.Carregar(GlbVariaveis.glb_Usuario);
                if(Configuracoes.dataDiferente()==true)
                {
                    return;
                }
                msgPermissoes.Dispose();
            }
            catch (Exception erro)
            {
                MessageBox.Show("2 - Carregando permissões: " + erro.Message);
            }
            finally
            {
                msgPermissoes.Dispose();
            }


            FrmMsgOperador msgCnFECF = new FrmMsgOperador("", "3 - Carregando Configurações NFC-e");
            try
            {
                msgEntrada.Dispose();
                msgCnFECF.Show();
                Application.DoEvents();

                ConfiguracoesECF.Carregar();
                Configuracoes.versaoSICENFCe();
                FuncoesNFC.verificarGerenciadorNFCe();
                Configuracoes.GravarArquivoVersoes();
                Configuracoes.GerarHoraAtualizacao();
                msgCnFECF.Dispose();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
                ConfiguracoesECF.idECF = 0;
            }
            finally
            {
                msgCnFECF.Dispose();
            }

            

            FrmMsgOperador msgCnFBalanca = new FrmMsgOperador("", "4 - Carregando Configurações da Balança !");
            try
            {
                msgEntrada.Dispose();
                msgCnFBalanca.Show();
                Application.DoEvents();

                    if (File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\Balanca.xml"))
                        GlbVariaveis.glb_balanca = FuncoesBalanca.ativarBalanca();
                

                msgCnFBalanca.Dispose();
            }
            catch (Exception erro)
            {
                DialogResult r = MessageBox.Show("Não foi possivel carregar configuração da Balanca! Deseja Detalhar o erro?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
                if (DialogResult.OK == r)
                {
                    MessageBox.Show(erro.ToString());
                }
            }
            finally
            {
                msgCnFBalanca.Dispose();
            }



            //Paf.AcrescentarRegistro61Sintegra(@"C:\IQSistemas\SICEpdv\IQKiosk\bin\Release\ArquivosPAF\sintegra.txt");
            if (!File.Exists("log_teste.txt") && ConfiguracoesECF.pdv && ConfiguracoesECF.idECF != 0)
            {
                try
                {
                    //MessageBox.Show("1");
                    //FuncoesPAFECF.GerarArquivoIDPAF();
                    //MessageBox.Show("2");
                }
                catch(Exception erro)
                {
                    MessageBox.Show(erro.ToString());
                }
            }

            //MessageBox.Show("3");

            if (Conexao.onLine && ConfiguracoesECF.pdv && ConfiguracoesECF.idECF != 0)
            {
                msgEntrada.Dispose();
                FrmMsgOperador msg = new FrmMsgOperador("", "5 - Atualizando dados do Servidor !");
                msg.Show();
                Application.DoEvents();
                if (Conexao.tipoConexao == 1 && !File.Exists("PDVlog.txt"))
                {
                    StandAlone.ApagarTabelasDados();
                    //StandAlone.CarregarTabelas();
                }
                Application.DoEvents();
                
                msg.Dispose();
            };

            if (!Permissoes.operadorCaixa && ConfiguracoesECF.pdv)
            {
                MessageBox.Show("Operador não possui função de caixa.","atenção",MessageBoxButtons.OK);
                ConfiguracoesECF.pdv = false;
                ConfiguracoesECF.idECF = 0;                                
                Application.Exit();
                return;
            }
            // Se for PDV então verifica o saldo


            if (ConfiguracoesECF.pdv && ConfiguracoesECF.idECF != 0)
            {
                /// If operandus modus = StandAlone
                /// 


                siceEntities entidade;
                if (Conexao.tipoConexao == 2 && Conexao.ConexaoOnline() == false)
                    entidade = Conexao.CriarEntidade(false);
                else
                    entidade = Conexao.CriarEntidade();

                #region StandAlone
                if (!Conexao.onLine && Conexao.tipoConexao == 1)
                {
                    if (Conexao.tipoConexao == 1)
                    {
                        IObjectContainer tabela = Db4oFactory.OpenFile("caixa.yap");

                        var queryOff = (from StandAloneCaixa q in tabela
                                        where q.tipoPagamento == "SI"
                                        && q.data == GlbVariaveis.Sys_Data
                                        && q.encerrado == false
                                        && q.operador == GlbVariaveis.glb_Usuario
                                        select q);
                        if (queryOff.Count() == 0 && !FuncoesECF.ZPendente())
                        {
                            if (MessageBox.Show("Modo Stand-Alone. Iniciar saldo de caixa ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                tabela.Close();
                                Suprimento.tipoPagamento = "SI";
                                Application.Run(new Suprimento());
                            };
                        }
                        tabela.Close();
                    }
                    else
                    {
                        var query = (from q in entidade.caixa
                                     where q.tipopagamento == "SI"
                                     && q.data == GlbVariaveis.Sys_Data
                                     && q.operador == GlbVariaveis.glb_Usuario
                                     select q);

                        if (query.Count() == 0 && !FuncoesECF.ZPendente())
                        {
                            if (MessageBox.Show("Modo Stand-Alone. Iniciar saldo de caixa ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                Suprimento.tipoPagamento = "SI";
                                Application.Run(new Suprimento());
                            }
                        }

                    }
                }
            #endregion

                else
                {

                    var query = (from q in entidade.caixa
                                 where q.tipopagamento == "SI"
                                 && q.data == GlbVariaveis.Sys_Data
                                 && q.operador == GlbVariaveis.glb_Usuario
                                 select q);


                    var caixaAberto = (from q in entidade.caixa
                                       where q.operador == GlbVariaveis.glb_Usuario
                                       && q.data != GlbVariaveis.Sys_Data
                                       select q);


                    var caixaAbertoFiliais = (from q in entidade.caixa
                                             where q.operador == GlbVariaveis.glb_Usuario
                                             && q.CodigoFilial != GlbVariaveis.glb_filial
                                             select q);

                    if(caixaAbertoFiliais.Count() != 0 && query.Count() == 0)
                    {
                        MessageBox.Show("Aviso: Já Existe Caixa aberto na Filial.: " + caixaAbertoFiliais.FirstOrDefault().CodigoFilial.ToString()+" não é possivel abrir mais de um caixa!");
                        return;
                    }

                    if (caixaAberto.Count() != 0 && query.Count() == 0 && ConfiguracoesECF.NFC == true)
                    {
                        
                        MessageBox.Show("Aviso: Existe Caixa aberto no dia.:" + caixaAberto.FirstOrDefault().data.Value.Date + " Filial.: " + caixaAberto.FirstOrDefault().CodigoFilial.ToString());
                        
                        ConfiguracoesECF.caixaPendente = true;
                    }
                    else
                    {
                        ConfiguracoesECF.caixaPendente = false;
                    }

                    var movimentoAnte = (from m in entidade.movimento
                                         where m.finalizado != "X"
                                         && m.codigofilial == GlbVariaveis.glb_filial
                                         && m.data != GlbVariaveis.Sys_Data
                                         select m).ToList();

                    if(movimentoAnte.Count() != 0 && caixaAberto.Count() == 0 && ConfiguracoesECF.NFC == true)
                    {
                        MessageBox.Show("Aviso: Movimento Anterior não encerrado!.:" + movimentoAnte.FirstOrDefault().data.Value.Date + " Filial.: " + movimentoAnte.FirstOrDefault().codigofilial.ToString());

                        MessageBox.Show("Por Favor, entre no sice.net finalize todos os caixa e depois o movimento diário!");
                        return;
                    }


                   var CRZReducao = (from r in entidade.r02
                                    where r.codigofilial == GlbVariaveis.glb_filial && r.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                    select r).Count();
            

                    string CRZ = "";
                    string coo = "";

                    if (GlbVariaveis.glb_Acbr == true)
                    {
                        CRZ = FuncoesECF.ultimaReducaoZ("CRZ");
                        //coo = coo.PadLeft(6,'0');
                        CRZ = CRZ.PadLeft(6, '0');
                        /*var reducao = (from r in entidade.r02
                                       where r.coo == coo && r.codigofilial == GlbVariaveis.glb_filial
                                       select r);*/

                        CRZReducao = (from r in entidade.r02
                                          where r.crz == CRZ && r.codigofilial == GlbVariaveis.glb_filial && r.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                          select r).Count();
                    }
                    else
                    {
                        coo = FuncoesECF.ultimaReducaoZ("COO");
                        coo = coo.PadLeft(6, '0');

                        CRZReducao = (from r in entidade.r02
                                          where r.coo == coo && r.codigofilial == GlbVariaveis.glb_filial && r.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                          select r).Count();
                    }


                    if (GlbVariaveis.glb_Acbr == true)
                    {
                        #region

                        if (!FuncoesECF.ZPendente() && CRZReducao == 0 && CRZ != "000000")
                        {
                            FrmMsgOperador msg = new FrmMsgOperador("", "Não desligue o ECF. Gravando dados última redução.");
                            msg.Show();
                            Application.DoEvents();
                            try
                            {
                                if (Conexao.ConexaoOnline() == true)
                                    FuncoesECF.GravarDadosReducaoAnterior();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Aviso: Não foi possível gravar dados " + ex.Message);
                            }
                            finally
                            {
                                msg.Dispose();
                            }
                        }


                        var movimento = (from n in entidade.movimento
                                         where n.data < GlbVariaveis.Sys_Data
                                         && n.finalizado == " " && n.codigofilial == GlbVariaveis.glb_filial
                                         select n.finalizado);

                        if (movimento.Count() > 0)
                        {
                            MessageBox.Show("Aviso: Movimento anterior não foi encerrado.");
                        }



                        if (query.Count() == 0 && !FuncoesECF.ZPendente() && FuncoesECF.estadoECF() != ACBrFramework.ECF.EstadoECF.Bloqueada && ConfiguracoesECF.caixaPendente == false)
                        {
                            
                            /*-var movimento = (from n in entidade.movimento
                                             where n.data < GlbVariaveis.Sys_Data
                                             && n.finalizado == " "
                                             select n.finalizado);

                            if (movimento.Count() > 0)
                            {
                                MessageBox.Show("Aviso: Movimento anterior não foi encerrado.");
                            }*/

                            Suprimento.tipoPagamento = "SI";
                            Application.Run(new Suprimento());
                        }

                        #endregion
                    }
                    else
                    {                        
                        #region
                        
                        if (!FuncoesECF.ZPendente() && !FuncoesECF.VerificaReducaZDia() && CRZReducao == 0)
                        {
                            FrmMsgOperador msg = new FrmMsgOperador("", "Não desligue o ECF. Gravando dados última redução.");
                            msg.Show();
                            Application.DoEvents();
                            try
                            {
                                if (Conexao.ConexaoOnline() == true)
                                    FuncoesECF.GravarDadosReducaoAnterior();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Aviso: Não foi possível gravar dados " + ex.Message);
                            }
                            finally
                            {
                                msg.Dispose();
                            }
                        }

                        var movimento = (from n in entidade.movimento
                                         where n.data < GlbVariaveis.Sys_Data
                                         && n.finalizado == " " && n.codigofilial == GlbVariaveis.glb_filial
                                         select n.finalizado);

                        if (movimento.Count() > 0)
                        {
                            MessageBox.Show("Aviso: Movimento anterior não foi encerrado.");
                        }

                        if (query.Count() == 0 && !FuncoesECF.ZPendente() && !FuncoesECF.VerificaReducaZDia() && ConfiguracoesECF.caixaPendente == false)
                        {
                            Suprimento.tipoPagamento = "SI";
                            Application.Run(new Suprimento());
                        }

                        #endregion
                    }
                }

            };
            
            Application.Run(new _pdv());
            
        }
    }

}
