using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Diagnostics;

namespace SICEpdv
{
    public static class Configuracoes
    {
        // Dados da Empresa
        public static string fantasia { get; set; }
        public static string razaoSocial { get; set; }
        public static string cnpj { get; set; }
        public static string inscricao { get; set; }
        public static string inscricaoMunicipal { get; set; }
        public static string telefone { get; set; }
        public static string endereco { get; set; }
        public static string cidade { get; set; }
        public static string bairro { get; set; }
        public static string estado { get; set; }
        public static string crt { get; set; }

        //Contabilidade
        public static string nomeContador { get; set; }
        public static string crccontador { get; set; }
        public static string emailContador { get; set; }
        public static string tipoEmpresa { get; set; }
        public static string cnae { get; set; }



        // Venda
        public static decimal descontoMaxVenda { get; set; }
        public static string formaDescontoMaxVenda { get; set; } //apensas nos desconto de configuração dos itens botão desconto maximo PDV
        public static decimal valorMaxVenda { get; set; }
        public static decimal arredondamento { get; set; }
        public static int diasPrimeiroVenc { get; set; }
        public static bool diasCorridos { get; set; }
        public static bool abaterCRcompraCH { get; set; }
        public static bool reservarEstoquePreVenda { get; set; }
        public static bool digitarSenhaFinalizacaoPreVenda { get; set; }
        public static string mensagemPDV { get; set; }
        public static string mensagemRodapeCupom { get; set; }
        public static bool vendaPorclasse { get; set; }
        public static bool restricoesPreVenda { get; set; }
        public static decimal descontoMaxGerencial { get; set; }
        public static bool descontoAtacado { get; set; }
        public static decimal descontoCartaoFidelidade { get; set; }
        public static bool controleLote { get; set; }
        public static bool cancelarvendarejeicaocartao { get; set; }
        //public static bool restringirQuantidadePrateleira { get;set; }
        public static bool gerarTransferenciaVenda { get; set; }
        public static bool romaneiroVenda { get; set; }
        public static decimal limitevendasemidentificacao { get; set; }
        public static bool gerarNFeVenda { get; set; }

        public static bool entradaDH { get; set; }
        public static bool entradaCA { get; set; }
        public static bool entradaCH { get; set; }
        public static bool entradaCR { get; set; }

        //PDV
        public static bool transportarSaldo { get; set; }
        public static bool mostrarsaldoliquido { get; set; }
        public static bool saldoDoOperador { get; set; }
        public static bool AbrirCupomcomCliente { get; set; }
        public static decimal alertaSangria { get; set; }
        public static decimal resolucaoHeight { get; set; }
        public static decimal resolucaoWidth { get; set; }

        // Venda Entrada
        public static bool mudarPrecoVenda { get; set; }
        public static bool usarQtdPrateleira { get; set; }
        public static bool vendasemprecomvendedor { get; set; }

        // Recebimento
        public static decimal taxaJurosDiario { get; set; }
        public static decimal taxaJurosAnt { get; set; }
        public static decimal descontoMaxRecCapital { get; set; }
        public static decimal descontoMaxRecJuros { get; set; }
        public static int diasLiberadosSemJuros { get; set; }

        // Produtos
        public static int posicaoCodBarrasBalanca { get; set; }
        public static int tamanhacodBarrasBalanca { get; set; }
        public static bool totalnoFinalCodBarrasBalanca { get; set; }
        public static string digitoVerificadorCodBarras { get; set; }
        public static bool procuraAutomaticaPrd { get; set; }
        public static bool procuraAutomaticaCli { get; set; }
        public static int limiteRegistroProdutos { get; set; }
        public static bool mostrarPrecoMinimo { get; set; }
        // Serviços
        public static string textoGarantia { get; set; }
        public static bool configuracaoSetada { get; set; }
        public static DateTime dataValidade { get; set; }

        // IQCARD

        public static int pontuacaoMaxIQCard { get; set; }
        public static string pontuacaoCR { get; set; }
        public static string pontuacaoCA { get; set; }
        public static decimal coefecientePontosIQCard { get; set; }
        public static bool promocaoIQCardAtiva = false;
        public static decimal valorcomprafidelizacao { get; set; }
        public static int qtdpontosfidelizacao { get; set; }
        public static string fidelizarRecebimento { get; set; }
        public static DateTime GlbDataGerada { get; set; }
        public static string cfgarquivardados { get; set; }
        



        public static void carregar(string filial)
        {
            /// If operandus modus = StandAlone
            /// 
            GlbVariaveis.chavePrivada = "C9E15B94BA0E28D1DE04099FE21C9370E23A083DAA65616FCA0D68BB1176B17CEBF5627BC1D9D788BD27120A0FE8C2418AC4B625FD47ACA2E3E98CA8D148A34DC28BDF92D82E0EB31649FAC61DB98EB42C5A2967E8A95173512732B13D2C2F9A149B438DB7A0602288EEFCA869E495C3D89F70E4D30B835E19B144A26060A407";

            entradaCA = true;
            entradaCH = true;
            entradaCR = true;
            entradaDH = true;

            try
            {
                if (AcrescentarConfigIQCARD())
                {
                    Application.Exit();
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    ConfigurationManager.RefreshSection("bindings");
                }
            }
            catch (Exception)
            {

            }

            try
            {
                if (AcrescentarConfigIQWS())
                {
                    Application.Exit();
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    ConfigurationManager.RefreshSection("bindings");
                }
            }
            catch (Exception)
            {

            }

            try
            {
                if (AcrescentarConfigWSProdutos())
                {
                    Application.Exit();
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    ConfigurationManager.RefreshSection("bindings");
                }
            }
            catch (Exception)
            {

            }




            totalnoFinalCodBarrasBalanca = true;
            mensagemPDV = "Seja bem-vindo !";
            mensagemRodapeCupom = "Ter você como cliente é um privilégio. Obrigado e volte sempre !";
            #region StandAlone            
            if (!Conexao.onLine)
            {
                IObjectContainer tabela = Db4oFactory.OpenFile("configfinanc.yap");
                var configOff = from StandAloneConfigfinanc p in tabela
                                where p.codigofilial == GlbVariaveis.glb_filial
                                select p;

                foreach (var item in configOff)
                {
                    descontoMaxVenda = item.descontoMaxVenda;
                    descontoMaxGerencial = item.descontoMaxVenda;
                    valorMaxVenda = item.valorMaxVenda != 0 ? item.valorMaxVenda : 250M;
                    arredondamento = item.arredondamento;
                    diasPrimeiroVenc = item.diasPrimeiroVenc;
                    abaterCRcompraCH = item.abaterCRcompraCH;
                    reservarEstoquePreVenda = item.reservarEstoquePreVenda;
                    mudarPrecoVenda = item.mudarPrecoVenda;
                    taxaJurosDiario = item.taxaJurosDiario;
                    posicaoCodBarrasBalanca = Convert.ToInt16(item.posicaoCodBarrasBalanca.Substring(0, 1));
                    tamanhacodBarrasBalanca = Convert.ToInt16(item.posicaoCodBarrasBalanca.Substring(1, 1));
                    vendaPorclasse = item.vendaPorClasse;
                    digitoVerificadorCodBarras = item.digitoVerificador ?? "2";
                    totalnoFinalCodBarrasBalanca = item.totalnoFinalCodBalanca;
                    transportarSaldo = false;

                    if (string.IsNullOrWhiteSpace(digitoVerificadorCodBarras))
                        digitoVerificadorCodBarras = "2";

                }

                tabela.Close();
                IObjectContainer tabelaDados = Db4oFactory.OpenFile("filiais.yap");
                var dadosEmpresaOFF = from StandAloneDadosEmpresa p in tabelaDados
                                      where p.codigofilial == GlbVariaveis.glb_filial
                                      select p;
                foreach (var item in dadosEmpresaOFF)
                {
                    razaoSocial = item.razaoSocial;
                    cnpj = item.cnpj;
                    inscricao = item.inscricao;
                    inscricaoMunicipal = item.inscricaoMunicipal;
                }
                // Aqui retorna para não processar as tabelas on line 
                // senão acontece erro
                tabela.Close();
                tabelaDados.Close();
                return;
            };
            #endregion StandAlone

            siceEntities entidade;
            if (Conexao.tipoConexao == 2 && Conexao.ConexaoOnline() == false)
                entidade = Conexao.CriarEntidade(false);
            else
                entidade = Conexao.CriarEntidade();

            try
            {

                var config = (from p in entidade.configfinanc
                              where p.CodigoFilial == GlbVariaveis.glb_filial
                              select p).ToList();

                if (config.Count() == 0)
                    throw new Exception("Nenhuma configuração encontrada para a filial: " + GlbVariaveis.glb_filial);


                string sql = "SELECT mostrarsaldoliquido FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";

                try
                {
                    mostrarsaldoliquido = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }
                catch
                {
                    mostrarsaldoliquido = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }

                sql = "SELECT saldoporoperador FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";

                try
                {
                    saldoDoOperador = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }
                catch
                {
                    saldoDoOperador = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }


                sql = "SELECT usarcontrolelote FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";

                try
                {
                    controleLote = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }
                catch
                {
                    controleLote = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }


                sql = "SELECT validadepromocaoiqcard FROM filiais WHERE  codigofilial='" + GlbVariaveis.glb_filial + "'";

                try
                {
                    var validade = Conexao.CriarEntidade().ExecuteStoreQuery<DateTime>(sql).FirstOrDefault();
                    if (validade >= DateTime.Now.Date)
                        promocaoIQCardAtiva = true;
                    else
                        promocaoIQCardAtiva = false;
                }
                catch
                {

                }

                sql = "SELECT cancelarvendarejeicaocartao FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";

                try
                {
                    cancelarvendarejeicaocartao = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }
                catch
                {
                    cancelarvendarejeicaocartao = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }

                sql = "SELECT gerartransfnavenda FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                try
                {
                    gerarTransferenciaVenda = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }
                catch
                {
                    gerarTransferenciaVenda = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }


                sql = "SELECT romaneiovenda FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                try
                {
                    romaneiroVenda = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }
                catch
                {
                    romaneiroVenda = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }


                sql = "SELECT limitevendasemidentificacao FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                try
                {
                    limitevendasemidentificacao = Conexao.CriarEntidade().ExecuteStoreQuery<decimal>(sql).FirstOrDefault();
                }
                catch
                {
                    limitevendasemidentificacao = Conexao.CriarEntidade(false).ExecuteStoreQuery<decimal>(sql).FirstOrDefault();
                }

                sql = "SELECT gerarNFeTransfVenda FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                try
                {
                    gerarNFeVenda = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }
                catch
                {
                    gerarNFeVenda = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>(sql).FirstOrDefault() == "S" ? true : false;
                }

                sql = "SELECT formaPagamentoDescontoMaximo FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                try
                {
                    formaDescontoMaxVenda = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();
                }
                catch
                {
                    formaDescontoMaxVenda = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>(sql).FirstOrDefault();
                }

                sql = "SELECT validade FROM iqsistemas LIMIT 1";
                try
                {
                    dataValidade = Conexao.CriarEntidade().ExecuteStoreQuery<DateTime>(sql).FirstOrDefault();
                }
                catch
                {
                    dataValidade = Conexao.CriarEntidade(false).ExecuteStoreQuery<DateTime>(sql).FirstOrDefault();
                }

                sql = "SELECT cfgarquivardados FROM filiais WHERE codigofilial='" + GlbVariaveis.glb_filial + "' limit 1";
                try
                {
                    cfgarquivardados = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();
                }
                catch
                {
                    cfgarquivardados = "N";
                }

                sql = "UPDATE configfinanc SET baixaratualizacao = rodarscripts WHERE (baixaratualizacao IS NULL OR baixaratualizacao = '') and codigoFilial = '"+GlbVariaveis.glb_filial+"'";
                try
                {
                    Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.ToString());
                }
                                
                sql = "SELECT horaatualizacao FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' limit 1";
                try
                {
                    GlbVariaveis.glb_horaAtualizacao = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();
                }
                catch
                {
                    GlbVariaveis.glb_horaAtualizacao = "";
                }

                sql = "SELECT rodarscripts FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' limit 1";
                try
                {
                    GlbVariaveis.glb_rodarScript = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();
                }
                catch
                {
                    GlbVariaveis.glb_rodarScript = "N";
                }

                sql = "SELECT baixaratualizacao FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' limit 1";
                try
                {
                    GlbVariaveis.glb_atualizar = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();
                }
                catch
                {
                    GlbVariaveis.glb_atualizar = "N";
                }


                sql = "SELECT idcliente FROM iqsistemas LIMIT 1";
                try
                {
                    GlbVariaveis.glb_idCliente = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();
                }
                catch
                {
                    GlbVariaveis.glb_idCliente = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>(sql).FirstOrDefault();
                }


                sql = "SELECT modoatualizacao FROM iqsistemas LIMIT 1";
                try
                {
                    GlbVariaveis.glb_modoatualizacao = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();
                }
                catch
                {
                    GlbVariaveis.glb_modoatualizacao = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>(sql).FirstOrDefault();
                }

                /*
                sql = "SELECT IFNULL(alertasangria, 0) as alertaSangria FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                try
                {
                    alertaSangria = Conexao.CriarEntidade().ExecuteStoreQuery<decimal>(sql).FirstOrDefault();
                }
                catch
                {
                    alertaSangria = Conexao.CriarEntidade(false).ExecuteStoreQuery<decimal>(sql).FirstOrDefault();
                    alertaSangria = 0;

                }
                */

                sql = "SELECT IFNULL(gateway,'N') FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' limit 1";
                try
                {
                    GlbVariaveis.glb_gateway = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();
                }
                catch
                {
                    GlbVariaveis.glb_gateway = "N";
                }


                foreach (var item in config)
                {
                    descontoMaxVenda = item.fatmaiordesvenda;
                    valorMaxVenda = item.limitevalor != 0 ? item.limitevalor : 250M;
                    arredondamento = item.maxarredondamento;
                    diasPrimeiroVenc = item.diasvencimento;
                    diasCorridos = item.diascorridosvencimentos == "S" ? true : false;
                    abaterCRcompraCH = item.abatercreditocompraCH == "S" ? true : false;
                    reservarEstoquePreVenda = item.abaterestoqueprevenda == "S" ? true : false;
                    mudarPrecoVenda = item.alterarpreco == "S" ? true : false;
                    digitarSenhaFinalizacaoPreVenda = item.operadordigitarsenhanaprevenda == "S" ? true : false;
                    taxaJurosDiario = item.fatjurdia;
                    descontoMaxRecCapital = item.fatmaiordescrec;
                    descontoMaxRecJuros = item.fatmaiordescrecjur;
                    posicaoCodBarrasBalanca = Convert.ToInt16(item.posicaocodigobalanca.Substring(0, 1));
                    tamanhacodBarrasBalanca = Convert.ToInt16(item.posicaocodigobalanca.Substring(1, 1));
                    mensagemPDV = item.msg2;
                    mensagemRodapeCupom = item.msgrodapecupom;
                    vendaPorclasse = item.PerClasse == "S" ? true : false;
                    textoGarantia = item.textogarantia;
                    digitoVerificadorCodBarras = item.digitoIniBal ?? "";
                    procuraAutomaticaPrd = item.buscaautomaticaprd == "S" ? true : false;
                    procuraAutomaticaCli = item.buscaautomatica == "S" ? true : false;
                    taxaJurosAnt = item.fatjurant;
                    usarQtdPrateleira = item.qtdprateleiras == "S" ? true : false;
                    restricoesPreVenda = item.restricaoprevenda == "S" ? true : false;
                    limiteRegistroProdutos = item.qtditenstabela == 0 ? 50 : item.qtditenstabela;
                    vendasemprecomvendedor = item.vendasemprecomvendedor == "S" ? true : false;
                    totalnoFinalCodBarrasBalanca = item.totalnofinalcodbalanca == "S" ? true : false;
                    descontoMaxGerencial = item.maximodescontogerencial;
                    transportarSaldo = item.transportarsaldocaixa == "S" ? true : false;
                    diasLiberadosSemJuros = item.fatnrdias == null ? 0 : item.fatnrdias.Value;
                    AbrirCupomcomCliente = item.sempreabrircupomcomcliente == "S" ? true : false;
                    descontoAtacado = item.aceitardescontoatacado == "S" ? true : false;
                    descontoCartaoFidelidade = item.descontocartaofidelidade;
                    mostrarPrecoMinimo = item.mostrarprecominimo == "S" ? true : false;

                    if (string.IsNullOrWhiteSpace(digitoVerificadorCodBarras))
                        digitoVerificadorCodBarras = "2";

                }


            }
            catch (Exception erro)
            {
                MessageBox.Show("Não foi possível acessar configurações: " + erro.Message);
            }

            try
            {
                if (Conexao.tipoConexao == 2 && Conexao.ConexaoOnline() == false)
                {
                    GlbVariaveis.glb_chaveIQCard = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>("SELECT IFNULL(tokeniqcard,'') FROM filiais WHERE codigofilial='" + GlbVariaveis.glb_filial + "'  LIMIT 1").FirstOrDefault();

                    if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                        GlbVariaveis.glb_chaveIQCard = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>("SELECT chaveiqcard FROM iqsistemas LIMIT 1").FirstOrDefault();
                    pontuacaoCA = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>("SELECT pontuacaoiqcardca FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault();
                    pontuacaoCR = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>("SELECT pontuacaoiqcardcr FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault();
                    coefecientePontosIQCard = Conexao.CriarEntidade(false).ExecuteStoreQuery<decimal>("SELECT coeficientepontosiqcard FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault();
                    valorcomprafidelizacao = Conexao.CriarEntidade(false).ExecuteStoreQuery<decimal>("SELECT valorcomprafidelizacao FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault();
                    qtdpontosfidelizacao = Conexao.CriarEntidade(false).ExecuteStoreQuery<int>("SELECT qtdpontosfidelizacao FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault();

                    try
                    {
                        fidelizarRecebimento = Conexao.CriarEntidade(false).ExecuteStoreQuery<string>("SELECT fidelizarrecebimento FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault();
                    }
                    catch (Exception)
                    {

                        fidelizarRecebimento = "N";
                    }


                    try
                    {
                        pontuacaoMaxIQCard = Convert.ToInt16(Conexao.CriarEntidade(false).ExecuteStoreQuery<string>("SELECT IFNULL(pontuacaomaxima,500) FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault());
                    }
                    catch (Exception)
                    {
                        pontuacaoMaxIQCard = 5000;
                    }

                }
                else
                {
                    GlbVariaveis.glb_chaveIQCard = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT IFNULL(tokeniqcard,'') FROM filiais WHERE codigofilial='" + GlbVariaveis.glb_filial + "'  LIMIT 1").FirstOrDefault();

                    if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                        GlbVariaveis.glb_chaveIQCard = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT chaveiqcard FROM iqsistemas LIMIT 1").FirstOrDefault();


                    //pontuacaoIQCARDAVista = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT descfidelidadeavista FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault();
                    pontuacaoCA = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT pontuacaoiqcardca FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault();
                    pontuacaoCR = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT pontuacaoiqcardcr FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault();
                    coefecientePontosIQCard = Conexao.CriarEntidade().ExecuteStoreQuery<decimal>("SELECT coeficientepontosiqcard FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault();
                    valorcomprafidelizacao = Conexao.CriarEntidade().ExecuteStoreQuery<decimal>("SELECT valorcomprafidelizacao FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault();
                    qtdpontosfidelizacao = Conexao.CriarEntidade().ExecuteStoreQuery<int>("SELECT qtdpontosfidelizacao FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault();
                    try
                    {
                        pontuacaoMaxIQCard = Convert.ToInt16(Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT IFNULL(pontuacaomaxima,500) FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault());
                    }
                    catch (Exception)
                    {
                        pontuacaoMaxIQCard = 5000;
                    }

                    try
                    {
                        fidelizarRecebimento = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT fidelizarrecebimento FROM configfinanc WHERE codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1").FirstOrDefault();
                    }
                    catch (Exception)
                    {
                        fidelizarRecebimento = "N";
                    }

                }

                //if (!string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard) && coefecientePontosIQCard <= 0.5M)
                //    coefecientePontosIQCard = 0.5M;

                if (qtdpontosfidelizacao > 7)
                    qtdpontosfidelizacao = 1;


                if (pontuacaoMaxIQCard <= 2000)
                    pontuacaoMaxIQCard = 2000;

            }
            catch (Exception)
            {

            }

            // dados da Empresa

            try
            {


                var dados = from p in entidade.filiais
                            where p.CodigoFilial == GlbVariaveis.glb_filial
                            select new { p.fantasia, p.empresa, p.cnpj, p.inscricao, p.inscricaomunicipal, p.telefone1, p.endereco, p.cidade, p.bairro, p.estado, p.crt, p.contador, p.crccontador, p.emailcontador, p.tipoempresa, p.CNAE, p.descricaoCNAE };
                foreach (var item in dados)
                {
                    fantasia = item.fantasia;
                    razaoSocial = item.empresa;
                    cnpj = item.cnpj;
                    inscricao = item.inscricao;
                    inscricaoMunicipal = item.inscricaomunicipal;
                    telefone = item.telefone1;
                    endereco = item.endereco;
                    cidade = item.cidade;
                    bairro = item.bairro;
                    cidade = item.cidade;
                    estado = item.estado;
                    telefone = item.telefone1;
                    crt = item.crt;
                    nomeContador = item.contador;
                    crccontador = item.crccontador;
                    emailContador = item.emailcontador;
                    tipoEmpresa = item.tipoempresa;
                    cnae = item.cidade + item.descricaoCNAE;

                }
                if (Conexao.tipoConexao == 2 && Conexao.ConexaoOnline() == false)
                    GlbVariaveis.idCliente = (from n in Conexao.CriarEntidade(false).iqsistemas select n.idcliente).FirstOrDefault().ToString();
                else
                    GlbVariaveis.idCliente = (from n in Conexao.CriarEntidade().iqsistemas select n.idcliente).FirstOrDefault().ToString();

                GlbVariaveis.nomeEmpresa = fantasia;
                GlbVariaveis.telefone = telefone;
                ConfiguracoesECF.NFCcrt = crt;

                if (GlbVariaveis.glb_filial == "00001")
                    GlbVariaveis.glb_estoque = "produtos";
                else
                    GlbVariaveis.glb_estoque = "produtosfilial";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.ToString());
            }

            // Verifica diretorio
            try
            {
                CriarDiretorios();
            }
            catch
            {
                throw new Exception("Diretório do SICEpdv não existem e não foi possível criar diretórios do SICEpdv. ecfmovimentodiario,espelhoecf,auxiliar. Para contorno a exceção crie manualmente ! ");
            }


            #region flag para sitar o cliente no DAV ECF
            try
            {
                if (System.IO.File.Exists(@"DAVCliente.txt"))//&& ConfiguracoesECF.pdv == false && ConfiguracoesECF.prevenda == false && ConfiguracoesECF.idNFC == 0)
                {
                    GlbVariaveis.glb_clienteDAV = true;
                }
                else
                {
                    GlbVariaveis.glb_clienteDAV = false;
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }

            #endregion

        }

        private static void CriarDiretorios()
        {
            if (Configuracoes.configuracaoSetada == false)
            {
                if (!Directory.Exists(@Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirMovimentoECF"]))
                {
                    Directory.CreateDirectory(@Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirMovimentoECF"]);
                }


                if (!Directory.Exists(@Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirArquivosPAF"]))
                {
                    Directory.CreateDirectory(@Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirArquivosPAF"]);
                }

                if (!Directory.Exists(@Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"]))
                {
                    Directory.CreateDirectory(@Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEspelhoECF"]);
                }

                if (!Directory.Exists(@Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirReducaoZEnvio"]))
                {
                    Directory.CreateDirectory(@Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirReducaoZEnvio"]);
                }

                if (!Directory.Exists(@Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEstoqueEnvio"]))
                {
                    Directory.CreateDirectory(@Application.StartupPath + @"\" + @ConfigurationManager.AppSettings["dirEstoqueEnvio"]);
                }

            }
            else
            {
                try
                {
                    if (!Directory.Exists(@ConfigurationManager.AppSettings["dirMovimentoECF"]))
                    {
                        Directory.CreateDirectory(@ConfigurationManager.AppSettings["dirMovimentoECF"]);
                    }


                    if (!Directory.Exists(@ConfigurationManager.AppSettings["dirArquivosPAF"]))
                    {
                        Directory.CreateDirectory(@ConfigurationManager.AppSettings["dirArquivosPAF"]);
                    }

                    if (!Directory.Exists(@ConfigurationManager.AppSettings["dirEspelhoECF"]))
                    {
                        Directory.CreateDirectory(@ConfigurationManager.AppSettings["dirEspelhoECF"]);
                    }

                    if (!Directory.Exists(@ConfigurationManager.AppSettings["dirReducaoZEnvio"]))
                    {
                        Directory.CreateDirectory(@ConfigurationManager.AppSettings["dirReducaoZEnvio"]);
                    }

                    if (!Directory.Exists(@ConfigurationManager.AppSettings["dirEstoqueEnvio"]))
                    {
                        Directory.CreateDirectory(@ConfigurationManager.AppSettings["dirEstoqueEnvio"]);
                    }
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.ToString());
                }
            }

        }

        public static void VerificaSICECarga()
        {
            //if (Conexao.tipoConexao == 2)
            //{

            if (File.Exists(@"C:\iqsistemas\siceCarga\SICECarga.exe"))
            {
                bool podeIniciar = false;

                System.Threading.Mutex primeiraInstanciaSICENFCe = new System.Threading.Mutex(true, "SICECarga", out podeIniciar);

                var processoNFCe = from n in Process.GetProcesses()
                                   where n.ProcessName.Contains("SICECarga")
                                   && !n.ProcessName.Contains("SICECarga.vs")
                                   select n;

                if (processoNFCe.Count() > 0)
                    podeIniciar = false;

                if (podeIniciar)
                {
                    try
                    {
                        Process myProcess = System.Diagnostics.Process.Start(@"C:\iqsistemas\siceCarga\SICECarga.exe");
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.Message);
                    }

                }
            }
            //}

        }

        public static bool AcrescentarConfigIQCARD()
        {
            string aspas = @"""";
            string arquivo = "SICEpdv.exe.config";

            if (File.Exists("SICEpdv.exe.configBKP") == false)
                File.Copy(arquivo, "SICEpdv.exe.configBKP");

            string novaConfig = "<system.serviceModel>" + Environment.NewLine +
    "<bindings>" + Environment.NewLine +
      "<basicHttpBinding>" + Environment.NewLine +
       "<binding name = " + aspas + "BasicHttpBinding_IWSIQPass" + aspas + " sendTimeout =" + aspas + "00:20:00" + aspas + Environment.NewLine +
        "maxReceivedMessageSize = " + aspas + "2147483647" + aspas + Environment.NewLine +
        "maxBufferSize = " + aspas + "2147483647" + aspas + "/>" + Environment.NewLine +
    "</basicHttpBinding>" + Environment.NewLine +
    "</bindings>" + Environment.NewLine +
    "<client>" + Environment.NewLine +
     " <endpoint address = " + aspas + "http://iqcard.cloudapp.net/WSIQPass.svc" + aspas + " binding = " + aspas + "basicHttpBinding" + aspas + Environment.NewLine +
       " bindingConfiguration = " + aspas + "BasicHttpBinding_IWSIQPass" + aspas + " contract = " + aspas + "ServiceReference1.IWSIQPass" + aspas + Environment.NewLine +
        "name = " + aspas + "BasicHttpBinding_IWSIQPass" + aspas + "/>" + Environment.NewLine +
    "</client>" + Environment.NewLine +
  "</system.serviceModel>";


            int qtdlinhaReg61 = 0;
            int qtdlinhaArquivo = 0;
            StringBuilder conteudoConfig = new StringBuilder();
            string arqConfig = @Application.StartupPath + @"\SICEpdv.exe.config";

            if (!File.Exists(@arqConfig))
                return true;

            FileStream Arquivo61 = new FileStream(arqConfig, FileMode.Open);

            using (StreamReader ler = new StreamReader(Arquivo61))
            {

                using (FileStream fsTemp = new FileStream("SICEpdv.exe.config2", FileMode.Create, FileAccess.Write))
                {

                    using (StreamWriter sw = new StreamWriter(fsTemp))
                    {

                        string linha = null;
                        while ((linha = ler.ReadLine()) != null)
                        {
                            if (linha.Contains("system.serviceModel"))
                            {
                                return false;
                            }
                            qtdlinhaReg61++;
                            conteudoConfig.AppendLine(linha.ToString());
                        }
                    }
                }
            }

            using (FileStream fs = new FileStream(arquivo, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    using (FileStream fsTemp = new FileStream("config.tmp", FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fsTemp))
                        {
                            string linha = null;
                            while ((linha = sr.ReadLine()) != null)
                            {
                                qtdlinhaArquivo++;
                                if (linha.Trim() == "</configuration>" && conteudoConfig != null)
                                {
                                    conteudoConfig.Replace("</configuration>", "");
                                    conteudoConfig.Append(novaConfig);
                                    conteudoConfig.AppendLine(Environment.NewLine);
                                    conteudoConfig.AppendLine("</configuration>");
                                    // sw.Write(conteudoConfig);

                                    //linha = "";
                                }
                                //if (linha.Substring(0, 2) == "60")
                                //  qtdlinhaReg60++;                                                             
                            }

                            sw.Write(conteudoConfig);
                        }
                    }
                }
            }


            File.Delete(arquivo);
            File.Move("config.tmp", arquivo);
            return true;
        }


        public static bool AcrescentarConfigIQWS()
        {
            string aspas = @"""";
            string arquivo = "SICEpdv.exe.config";

            if (File.Exists("SICEpdv.exe.configBKP") == false)
                File.Copy(arquivo, "SICEpdv.exe.configBKP");

            string novaConfig = " <endpoint address = " + aspas + "http://wssice.cloudapp.net/WSClientes.svc" + aspas + " binding = " + aspas + "basicHttpBinding" + aspas + Environment.NewLine +
       " bindingConfiguration = " + aspas + "BasicHttpBinding_IWSClientes" + aspas + " contract = " + aspas + "ServiceReference2.IWSClientes" + aspas + Environment.NewLine +
        "name = " + aspas + "BasicHttpBinding_IWSClientes" + aspas + "/>" + Environment.NewLine +
        "</client>" + Environment.NewLine;


            int qtdlinhaReg61 = 0;
            int qtdlinhaArquivo = 0;
            StringBuilder conteudoConfig = new StringBuilder();
            string arqConfig = @Application.StartupPath + @"\SICEpdv.exe.config";

            if (!File.Exists(@arqConfig))
                return true;

            FileStream Arquivo61 = new FileStream(arqConfig, FileMode.Open);

            using (StreamReader ler = new StreamReader(Arquivo61))
            {

                using (FileStream fsTemp = new FileStream("SICEpdv.exe.config2", FileMode.Create, FileAccess.Write))
                {

                    using (StreamWriter sw = new StreamWriter(fsTemp))
                    {

                        string linha = null;
                        while ((linha = ler.ReadLine()) != null)
                        {
                            if (linha.Contains("wssice.cloudapp.net/WSClientes.svc"))
                            {
                                return false;
                            }
                            qtdlinhaReg61++;
                            conteudoConfig.AppendLine(linha.ToString());
                        }
                    }
                }
            }

            using (FileStream fs = new FileStream(arquivo, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    using (FileStream fsTemp = new FileStream("config.tmp", FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fsTemp))
                        {
                            string linha = null;
                            while ((linha = sr.ReadLine()) != null)
                            {
                                qtdlinhaArquivo++;
                                if (linha.Trim() == "</client>" && conteudoConfig != null)
                                {
                                    conteudoConfig.Replace("</client>", novaConfig);
                                    //conteudoConfig.Append(novaConfig);
                                    //conteudoConfig.AppendLine(Environment.NewLine);
                                    //conteudoConfig.AppendLine("</client>");
                                    //// sw.Write(conteudoConfig);

                                    //linha = "";
                                }
                                //if (linha.Substring(0, 2) == "60")
                                //  qtdlinhaReg60++;                                                             
                            }

                            sw.Write(conteudoConfig);
                        }
                    }
                }
            }

            File.Delete(arquivo);
            File.Move("config.tmp", arquivo);
            return true;
        }


        public static bool AcrescentarConfigIQWSBinding()
        {
            string aspas = @"""";
            string arquivo = "SICEpdv.exe.config";

            if (File.Exists("SICEpdv.exe.configBKP") == false)
                File.Copy(arquivo, "SICEpdv.exe.configBKP");


            string novaConfig = "<binding name = " + aspas + "BasicHttpBinding_IWSClientes" + aspas + " sendTimeout =" + aspas + "00:20:00" + aspas + Environment.NewLine +
        "maxReceivedMessageSize = " + aspas + "2147483647" + aspas + Environment.NewLine +
        "maxBufferSize = " + aspas + "2147483647" + aspas + "/>" + Environment.NewLine +
        "</basicHttpBinding>" + Environment.NewLine;

            int qtdlinhaReg61 = 0;
            int qtdlinhaArquivo = 0;
            StringBuilder conteudoConfig = new StringBuilder();
            string arqConfig = @Application.StartupPath + @"\SICEpdv.exe.config";

            if (!File.Exists(@arqConfig))
                return true;

            FileStream Arquivo61 = new FileStream(arqConfig, FileMode.Open);

            using (StreamReader ler = new StreamReader(Arquivo61))
            {

                using (FileStream fsTemp = new FileStream("SICEpdv.exe.config2", FileMode.Create, FileAccess.Write))
                {

                    using (StreamWriter sw = new StreamWriter(fsTemp))
                    {
                        string procura = "<binding name = " + aspas + "BasicHttpBinding_IWSClientes";

                        string linha = null;
                        while ((linha = ler.ReadLine()) != null)
                        {
                            if (linha.Contains("binding name") && linha.Contains("BasicHttpBinding_IWSClientes"))
                            {
                                return false;
                            }
                            qtdlinhaReg61++;
                            conteudoConfig.AppendLine(linha.ToString());
                        }
                    }
                }
            }

            using (FileStream fs = new FileStream(arquivo, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    using (FileStream fsTemp = new FileStream("config.tmp", FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fsTemp))
                        {
                            string linha = null;
                            while ((linha = sr.ReadLine()) != null)
                            {
                                qtdlinhaArquivo++;
                                if (linha.Trim() == "</basicHttpBinding>" && conteudoConfig != null)
                                {
                                    conteudoConfig.Replace("</basicHttpBinding>", novaConfig);
                                    //conteudoConfig.Append(novaConfig);
                                    //conteudoConfig.AppendLine(Environment.NewLine);
                                    //conteudoConfig.AppendLine("</client>");
                                    //// sw.Write(conteudoConfig);

                                    //linha = "";
                                }

                                //if (linha.Substring(0, 2) == "60")
                                //  qtdlinhaReg60++;                                                             
                            }

                            sw.Write(conteudoConfig);
                        }
                    }
                }
            }

            File.Delete(arquivo);
            File.Move("config.tmp", arquivo);
            return true;
        }


        public static bool AcrescentarConfigWSProdutos()
        {
            string aspas = @"""";
            string arquivo = "SICEpdv.exe.config";

            if (File.Exists("SICEpdv.exe.configBKP") == false)
                File.Copy(arquivo, "SICEpdv.exe.configBKP");

            string novaConfig = " <endpoint address = " + aspas + "http://wssice.cloudapp.net/WSProdutos.svc" + aspas + " binding = " + aspas + "basicHttpBinding" + aspas + Environment.NewLine +
       " bindingConfiguration = " + aspas + "BasicHttpBinding_IWSProdutos" + aspas + " contract = " + aspas + "ServiceProdutos.IWSProdutos" + aspas + Environment.NewLine +
        "name = " + aspas + "BasicHttpBinding_IWSProdutos" + aspas + "/>" + Environment.NewLine +
        "</client>" + Environment.NewLine;


            int qtdlinhaReg61 = 0;
            int qtdlinhaArquivo = 0;
            StringBuilder conteudoConfig = new StringBuilder();
            string arqConfig = @Application.StartupPath + @"\SICEpdv.exe.config";

            if (!File.Exists(@arqConfig))
                return true;

            FileStream Arquivo61 = new FileStream(arqConfig, FileMode.Open);

            using (StreamReader ler = new StreamReader(Arquivo61))
            {

                using (FileStream fsTemp = new FileStream("SICEpdv.exe.config2", FileMode.Create, FileAccess.Write))
                {

                    using (StreamWriter sw = new StreamWriter(fsTemp))
                    {

                        string linha = null;
                        while ((linha = ler.ReadLine()) != null)
                        {
                            if (linha.Contains("wssice.cloudapp.net/WSProdutos.svc"))
                            {
                                return false;
                            }
                            qtdlinhaReg61++;
                            conteudoConfig.AppendLine(linha.ToString());
                        }
                    }
                }
            }

            using (FileStream fs = new FileStream(arquivo, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    using (FileStream fsTemp = new FileStream("config.tmp", FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fsTemp))
                        {
                            string linha = null;
                            while ((linha = sr.ReadLine()) != null)
                            {
                                qtdlinhaArquivo++;
                                if (linha.Trim() == "</client>" && conteudoConfig != null)
                                {
                                    conteudoConfig.Replace("</client>", novaConfig);
                                    //conteudoConfig.Append(novaConfig);
                                    //conteudoConfig.AppendLine(Environment.NewLine);
                                    //conteudoConfig.AppendLine("</client>");
                                    //// sw.Write(conteudoConfig);

                                    //linha = "";
                                }
                                //if (linha.Substring(0, 2) == "60")
                                //  qtdlinhaReg60++;                                                             
                            }

                            sw.Write(conteudoConfig);
                        }
                    }
                }
            }

            File.Delete(arquivo);
            File.Move("config.tmp", arquivo);
            return true;
        }


        public static bool AcrescentarConfigWSProdutosBinding()
        {
            string aspas = @"""";
            string arquivo = "SICEpdv.exe.config";

            if (File.Exists("SICEpdv.exe.configBKP") == false)
                File.Copy(arquivo, "SICEpdv.exe.configBKP");


            string novaConfig = "<binding name = " + aspas + "BasicHttpBinding_IWSProdutos" + aspas + " sendTimeout =" + aspas + "00:20:00" + aspas + Environment.NewLine +
        "maxReceivedMessageSize = " + aspas + "2147483647" + aspas + Environment.NewLine +
        "maxBufferSize = " + aspas + "2147483647" + aspas + "/>" + Environment.NewLine +
        "</basicHttpBinding>" + Environment.NewLine;

            int qtdlinhaReg61 = 0;
            int qtdlinhaArquivo = 0;
            StringBuilder conteudoConfig = new StringBuilder();
            string arqConfig = @Application.StartupPath + @"\SICEpdv.exe.config";

            if (!File.Exists(@arqConfig))
                return true;

            FileStream Arquivo61 = new FileStream(arqConfig, FileMode.Open);

            using (StreamReader ler = new StreamReader(Arquivo61))
            {

                using (FileStream fsTemp = new FileStream("SICEpdv.exe.config2", FileMode.Create, FileAccess.Write))
                {

                    using (StreamWriter sw = new StreamWriter(fsTemp))
                    {
                        string procura = "<binding name = " + aspas + "BasicHttpBinding_IWSProdutos";

                        string linha = null;
                        while ((linha = ler.ReadLine()) != null)
                        {
                            if (linha.Contains("binding name") && linha.Contains("BasicHttpBinding_IWSProdutos"))
                            {
                                return false;
                            }
                            qtdlinhaReg61++;
                            conteudoConfig.AppendLine(linha.ToString());
                        }
                    }
                }
            }

            using (FileStream fs = new FileStream(arquivo, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    using (FileStream fsTemp = new FileStream("config.tmp", FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fsTemp))
                        {
                            string linha = null;
                            while ((linha = sr.ReadLine()) != null)
                            {
                                qtdlinhaArquivo++;
                                if (linha.Trim() == "</basicHttpBinding>" && conteudoConfig != null)
                                {
                                    conteudoConfig.Replace("</basicHttpBinding>", novaConfig);
                                    //conteudoConfig.Append(novaConfig);
                                    //conteudoConfig.AppendLine(Environment.NewLine);
                                    //conteudoConfig.AppendLine("</client>");
                                    //// sw.Write(conteudoConfig);

                                    //linha = "";
                                }

                                //if (linha.Substring(0, 2) == "60")
                                //  qtdlinhaReg60++;                                                             
                            }

                            sw.Write(conteudoConfig);
                        }
                    }
                }
            }

            File.Delete(arquivo);
            File.Move("config.tmp", arquivo);
            return true;
        }



        static public bool setarAppConfig()
        {
            try
            {
                @ConfigurationManager.AppSettings["dirArquivoAuxiliar"] = @"C:\IQsistemas\SICEpdv\auxiliar";
                @ConfigurationManager.AppSettings["dirMovimentoECF"] = @"C:\IQsistemas\SICEpdv\EcfMovimentoDiario";
                @ConfigurationManager.AppSettings["dirEspelhoECF"] = @"C:\IQsistemas\SICEpdv\EspelhoECF";
                @ConfigurationManager.AppSettings["dirArquivosPAF"] = @"C:\IQsistemas\SICEpdv\ArquivosPAF";
                @ConfigurationManager.AppSettings["pathRetornoECF"] = @"c:\iqsistemas\";
                @ConfigurationManager.AppSettings["exePrincipal"] = @"C:\IQsistemas\SICEpdv\auxiliar";
                @ConfigurationManager.AppSettings["dirReducaoZEnvio"] = @"C:\IQsistemas\SICEpdv\ReduzacaoZEnvio";
                @ConfigurationManager.AppSettings["dirEstoqueEnvio"] = @"C:\IQsistemas\SICEpdv\EstoqueEnvio";

                Configuracoes.configuracaoSetada = true;
                return true;
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.ToString());
                return false;
            }
        }

        static public bool dataDiferente()
        {
            string sql = "SELECT CURRENT_DATE";
            string dataServidor = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();

            if (DateTime.Parse(dataServidor.ToString()).Date != DateTime.Now.Date)
            {
                MessageBox.Show("Horario ou data do servidor diferente do terminal!");
                return true;
            }

            return false;
        }

        static public void capturarResolucao()
        {

            try
            {
                resolucaoHeight = Screen.PrimaryScreen.Bounds.Height;
                resolucaoWidth = Screen.PrimaryScreen.Bounds.Width;
            }
            catch (Exception erro)
            {

            }
        }

        static public void verificaTerminalECF()
        {

            try
            {
                var SQL = "SELECT IFNULL(COUNT(1),0) FROM ecf WHERE codigoFilial = '" + GlbVariaveis.glb_filial + "' AND ecf.modeloECF = '65' AND id = '" + GlbVariaveis.glb_IP + "';";
                int ecf = Conexao.CriarEntidade().ExecuteStoreQuery<int>(SQL).FirstOrDefault();

                if (ecf == 0)
                {
                    Conexao.CriarEntidade().ExecuteStoreCommand("INSERT INTO ecf(modelo, id, aliquota07, aliquota12, aliquota17, aliquota25, aliquota27, isencao, substituicao, naoincide, dinheiro, cheque, cartao, crediario, recebimento, ticket, pdv, prevenda, modelocupom, tef, modelocarne, modeloimpressora," +
                    "impdiretolpt1, iniciarchat, impressoracheque, avancolinhas, modelorecibo, usargerenciador, preencherboleto, buscaautomatica, isencaoiss, substituicaoiss, naoincideiss, imprimepesocupom, impcabecalhocupom, modelopromiss," +
                    "modeloetiquetadora, tefdedicado, setupimpressora, consultapreco, modeloGaveta, modeloECF, imprimeprazodias, manteritensNFe, codigofilial, mantaritensNFe, verificarPrevenda)" +
                    "VALUES(0, '" + GlbVariaveis.glb_IP + "', 0, 0, 0, 0, 0, 'II', 'FF', 'NN', '0', '0', '0', '0', '0', '00', 'N', 'N', '01', 'N', '01', '0', 'N', 'N', '0 - Nenhuma', '01', '02', 'S', 'N', 'S', 'SI', 'SF', 'SN', 'N', 'S', '1', '0', 'N', 'N', 'S', '0', '65', 'N', 'S', '" + GlbVariaveis.glb_filial + "', 'S', 'N');");

                }
            }
            catch (Exception erro)
            {

            }
        }

        static public void dataAberturaCaixa(String filial, String operador)
        {
            try
            {
                var SQL = "SELECT IFNULL(MIN(DATA), CURRENT_DATE) FROM caixa WHERE codigofilial = '" + filial + "' AND operador = '" + operador + "'";
                GlbDataGerada = Conexao.CriarEntidade().ExecuteStoreQuery<DateTime>(SQL).FirstOrDefault();

            }
            catch (Exception erro)
            {

            }

        }

        static public void versaoSICENFCe()
        {
            try
            {
                if (ConfiguracoesECF.NFC == true && ConfiguracoesECF.idNFC == 2)
                {
                    System.Diagnostics.FileVersionInfo fvi;
                    fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(@"C:\iqsistemas\SICENFC-e\SICENFCe.exe");

                    System.Diagnostics.FileVersionInfo fviSICE;
                    fviSICE = System.Diagnostics.FileVersionInfo.GetVersionInfo(@"C:\iqsistemas\sice.net\sice.exe");

                    if (fvi.FileVersion.Length > 2)
                        GlbVariaveis.versaoSICENFCe = int.Parse(fvi.FileVersion.ToString().Replace(".", "").Substring(0, 2));



                    if (fviSICE.FileVersion.Length > 2)
                        GlbVariaveis.versaoSICENFCe = int.Parse(fviSICE.FileVersion.ToString().Replace(".", ""));

                }
            }
            catch (Exception erro)
            {

            }

        }

        static public void GravarArquivoVersoes()
        {
            if (File.Exists(@"C:\iqsistemas\IQSync\ToDo\fecharSync.txt"))
                File.Delete(@"C:\iqsistemas\IQSync\ToDo\fecharSync.txt");

            string vSicenet = GlbVariaveis.versaoSICENFCe.ToString();
            string VsicePDV = GlbVariaveis.glb_Versao.Replace(".", "").Substring(0,1)+"0"+GlbVariaveis.glb_Versao.Replace(".", "").Substring(1, 3);

            string Sync = "{\"key001\": {\"Group\": \"SICEnet\", \"Version\": \""+ vSicenet + "\"}, \"key002\": {\"Group\": \"SICEpdv\", \"Version\": \""+ VsicePDV + "\"}}";
            Funcoes.escreveArquivo(@"C:\iqsistemas\IQSync\Local\versaoLocal.txt", Sync);

        }

        static public void GerarHoraAtualizacao()
        {
            try
            {
                string sql = "";

                if (GlbVariaveis.glb_horaAtualizacao == "")
                {
                    List<string> horas = new List<string>();
                    horas.Add("09:00");
                    horas.Add("09:15");
                    horas.Add("09:30");
                    horas.Add("09:45");
                    horas.Add("10:00");
                    horas.Add("10:15");
                    horas.Add("10:30");
                    horas.Add("10:45");
                    horas.Add("11:00");
                    horas.Add("11:15");
                    horas.Add("11:30");
                    horas.Add("11:45");
                    horas.Add("12:00");
                    horas.Add("12:15");
                    horas.Add("12:30");
                    horas.Add("12:45");
                    horas.Add("13:00");
                    horas.Add("13:15");
                    horas.Add("13:30");
                    horas.Add("13:45");
                    horas.Add("14:00");
                    horas.Add("14:15");
                    horas.Add("14:30");
                    horas.Add("14:45");
                    horas.Add("15:00");
                    horas.Add("15:15");
                    horas.Add("15:30");
                    horas.Add("15:45");
                    horas.Add("16:00");

                    Random rnd = new Random();
                    var result = horas.OrderBy(p => rnd.Next()).First();


                    sql = "UPDATE configfinanc SET horaAtualizacao = '" + result + "' WHERE codigofilial='" + GlbVariaveis.glb_filial + "'";
                    try
                    {
                        Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                        GlbVariaveis.glb_horaAtualizacao = result;
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.ToString());
                    }

                }


                if (GlbVariaveis.glb_atualizar == "N" && GlbVariaveis.glb_rodarScript == "N")
                {
                    GlbVariaveis.glb_horaAtualizacao = "Off";
                }

                string Sync = "{\"key001\": {\"ID\": \"" + GlbVariaveis.glb_idCliente + "\", \"Hora\": \"" + GlbVariaveis.glb_horaAtualizacao + "\", \"Verificado\":\"N\"}}";
                Funcoes.escreveArquivo(@"C:\iqsistemas\IQSync\Local\configuracaoCliente.txt", Sync);

                if (GlbVariaveis.glb_modoatualizacao == "2")
                {
                    //ShellExecute(0, nil, 'C:\iqsistemas\IQSync\IQSync.exe', '2', nil, SW_SHOWNORMAL);
                    //Process myProcess = System.Diagnostics.Process.Start(@"C:\iqsistemas\SICENFC-e\SICENFCe.exe", " " + impressora);
                    Process myProcess = System.Diagnostics.Process.Start(@"C:\iqsistemas\IQSync\IQSync.exe", " " + "2" + " " + GlbVariaveis.glb_IP);
                }
                else
                {
                    //ShellExecute(0, nil, 'C:\iqsistemas\IQSync\IQSync.exe', '1', nil, SW_SHOWNORMAL);
                    Process myProcess = System.Diagnostics.Process.Start(@"C:\iqsistemas\IQSync\IQSync.exe", " " + "1" + " " + GlbVariaveis.glb_IP);
                }

            }
            catch (Exception erro)
            {

            }
        }
    }
}
