using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using System.Windows.Forms;
using System.Data;
using System.Data.EntityClient;
using System.IO;
using System.Data.Objects;

namespace SICEpdv
{
    class StandAlone
    {
        static IEnumerable<StandAloneControneSincronizacao> listDocumentosSincrozizar;

        public static bool CarregarTabelas()
        {
            ApagarTabelasDados();
            if (GlbVariaveis.glb_standalone == true)
            {
                CriarTabelaUsuario();
                CriarTabelaProdutos();
                CriarTabelaDadosEmpresa();
                CriarTabelaConfigfinanc();
            }
            return true;
        }

        public static bool ApagarTabelasDados()
        {            
            ///Verifica conexão antes de apagar os arquiov
            ///
            if (!Conexao.onLine)
                return false;
            try
            {                           
                if (File.Exists("configfinanc.yap"))
            System.IO.File.Delete("configfinanc.yap");
                
                if (File.Exists("senhas.yap"))
            System.IO.File.Delete("senhas.yap");
                
              
                
                if (File.Exists("filiais.yap"))
            System.IO.File.Delete("filiais.yap");
            return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        public static bool CriarTabelaUsuario()
        {            
            IObjectContainer tabela = Db4oFactory.OpenFile(@"senhas.yap");
            siceEntities entidade = Conexao.CriarEntidade();
            var u = from n in entidade.senhas                    
                    select new
                    {
                        codigo = n.codigo,
                        operador = n.operador,
                        senha = n.senha,
                        descontoFinalizacao = n.vendesconto,
                        arredondar = n.vendarredondamento,
                        excluirDocumento = n.venexcluir,
                        operadorcaixa = n.rotcaixa,
                        venderEstoqueNegativo = n.estnegativo
                    };            

            List<StandAloneUsuario> lstUsuarios = new List<StandAloneUsuario>();
            foreach (var item in u)
            {
                StandAloneUsuario usuario = new StandAloneUsuario();
                usuario.codigo = item.codigo;
                usuario.operador = item.operador;
                usuario.senha = item.senha;
                usuario.descontoFinalizacao = item.descontoFinalizacao=="S" ? true : false  ;
                usuario.arredondar = item.arredondar == "S" ? true : false;
                usuario.excluirDocumento = item.excluirDocumento == "S" ? true : false;
                usuario.operadorCaixa = item.operadorcaixa=="S" ? true : false;
                usuario.venderEstoqueNegativo = item.venderEstoqueNegativo == "S" ? true : false;
                lstUsuarios.Add(usuario);
            }

            tabela.Store(lstUsuarios);
            tabela.Close();            
            return true;
        }

        public static bool CriarTabelaProdutos()
        {
            // Atenção criar o arquivo na homologação
            if (!File.Exists("produtos.yap"))
                return true;

            if (File.Exists("produtos.yap"))
            {
                FileInfo arquivo = new FileInfo("produtos.yap");
                var dataArquivo = arquivo.LastWriteTime.Date;

                if (dataArquivo == GlbVariaveis.Sys_Data.Date && arquivo.Length>4000)
                    return true;
            }

            try
            {
                Funcoes.ProcedureAjuste("AjustarCamposNulos");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível executar procedure de ajuste " + ex.Message);

            }

            if (File.Exists("produtos.yap"))
                System.IO.File.Delete("produtos.yap");
            List<StandAloneProdutos> lstProdutos = new List<StandAloneProdutos>();
            IObjectContainer tabela = Db4oFactory.OpenFile(@"produtos.yap");
            siceEntities entidade = Conexao.CriarEntidade();

            try
            {
                if (GlbVariaveis.glb_filial == "00001")
                {

                    var dados = from p in entidade.produtos
                                where p.CodigoFilial == GlbVariaveis.glb_filial
                                && p.situacao != "Inativo" && p.situacao != "Excluído"
                                select new
                                {
                                    codigoFilial = p.CodigoFilial,
                                    codigo = p.codigo,
                                    codigoBarras = p.codigobarras,
                                    descricao = p.descricao,
                                    situacao = p.situacao,
                                    unidade = p.unidade,
                                    embalagem = p.embalagem,
                                    quantidade = p.quantidade,
                                    qtdDisponivel = p.quantidade - p.qtdprevenda,
                                    icms = p.icms,
                                    tributacao = p.tributacao,
                                    preco = p.precovenda,
                                    precoatacado = p.precoatacado,
                                    IAT = "A",
                                    IPPT = "T",
                                    descontoPromocao = p.descontopromocao,
                                    tipo = p.tipo,
                                    ativacompdesc = p.ativacompdesc,
                                    grade = p.grade,
                                    descontomaximo = p.descontomaximo,
                                    aliquotaIPI = p.aliquotaIPI,
                                    pis = p.pis,
                                    cofins = p.cofins
                                };

                    foreach (var item in dados)
                    {
                        StandAloneProdutos produto = new StandAloneProdutos();
                        produto.codigoFilial = item.codigoFilial;
                        produto.codigo = item.codigo;
                        produto.codigoBarras = item.codigoBarras;
                        produto.descricao = item.descricao;
                        produto.situacao = item.situacao;
                        produto.unidade = item.unidade;
                        produto.embalagem = item.embalagem;
                        produto.quantidade = item.quantidade;
                        produto.quantidadeDisponivel = item.qtdDisponivel;
                        produto.preco = item.preco;
                        produto.precoatacado = item.precoatacado;
                        produto.icms = item.icms;
                        produto.tribuacao = item.tributacao;
                        produto.indicadorTrucancamento = item.IAT;
                        produto.indicadorProducao = item.IPPT;
                        produto.descontoPromocao = item.descontoPromocao;
                        produto.tipo = item.tipo;
                        produto.ativacompdesc = item.ativacompdesc;
                        produto.grade = item.grade;
                        produto.descontoMaximo = item.descontomaximo;
                        produto.aliquotaIPI = item.aliquotaIPI.GetValueOrDefault();
                        produto.pis = item.pis.GetValueOrDefault();
                        produto.cofins = item.cofins.Value;
                        lstProdutos.Add(produto);
                    };

                }
                else
                {
                    var dados = from p in entidade.produtosfilial
                                where p.CodigoFilial == GlbVariaveis.glb_filial
                                && p.situacao != "Inativo" && p.situacao != "Excluído"
                                select new
                                {
                                    codigoFilial = p.CodigoFilial,
                                    codigo = p.codigo,
                                    codigoBarras = p.codigobarras,
                                    descricao = p.descricao,
                                    situacao = p.situacao,
                                    unidade = p.unidade,
                                    embalagem = p.embalagem,
                                    quantidade = p.quantidade,
                                    qtdDisponivel = p.quantidade - p.qtdprevenda,
                                    icms = p.icms,
                                    tributacao = p.tributacao,
                                    preco = p.precovenda,
                                    precoatacado = p.precoatacado,
                                    IAT = "A",
                                    IPPT = "T",
                                    descontoPromocao = p.descontopromocao,
                                    tipo = p.tipo,
                                    ativacompdesc = p.ativacompdesc,
                                    grade = p.grade,
                                    descontomaximo = p.descontomaximo,
                                    aliquotaIPI = p.aliquotaIPI,
                                    pis = p.pis,
                                    cofins = p.cofins
                                };

                    foreach (var item in dados)
                    {

                        StandAloneProdutos produto = new StandAloneProdutos();
                        produto.codigoFilial = item.codigoFilial;
                        produto.codigo = item.codigo;
                        produto.codigoBarras = item.codigoBarras;
                        produto.descricao = item.descricao;
                        produto.situacao = item.situacao;
                        produto.unidade = item.unidade;
                        produto.embalagem = item.embalagem.Value;
                        produto.quantidade = item.quantidade;
                        produto.quantidadeDisponivel = item.qtdDisponivel;
                        produto.preco = item.preco;
                        produto.precoatacado = item.precoatacado;
                        produto.icms = item.icms;
                        produto.tribuacao = item.tributacao;
                        produto.indicadorTrucancamento = item.IAT;
                        produto.indicadorProducao = item.IPPT;
                        produto.descontoPromocao = item.descontoPromocao;
                        produto.tipo = item.tipo;
                        produto.ativacompdesc = item.ativacompdesc;
                        produto.grade = item.grade;
                        produto.descontoMaximo = item.descontomaximo;
                        produto.aliquotaIPI = item.aliquotaIPI.GetValueOrDefault();
                        produto.pis = item.pis.GetValueOrDefault();
                        produto.cofins = item.cofins.GetValueOrDefault();
                        lstProdutos.Add(produto);
                    };
                };

                tabela.Store(lstProdutos);
                tabela.Close();
                return true;
            }
            catch 
            {
                return false;
            }
            

        }

        public static bool CriarTabelaConfigfinanc()
        {
            List<StandAloneConfigfinanc> lstconfig = new List<StandAloneConfigfinanc>();
            IObjectContainer tabela = Db4oFactory.OpenFile(@"configfinanc.yap");
            siceEntities entidade = Conexao.CriarEntidade();

            var dados = from c in entidade.configfinanc
                    where c.CodigoFilial == GlbVariaveis.glb_filial
                    select new
                    {
                        codigofilial = c.CodigoFilial,
                        descontoMaxVenda = c.fatmaiordesvenda,
                        valorMaxVenda = c.limitevalor,
                        arredondamento = c.maxarredondamento,
                        diasPrimeiroVenc = c.diasvencimento,
                        abaterCRcompraCH = c.abatercreditocompraCH == "S" ? true : false,
                        reservarEstoquePreVenda = c.abaterestoqueprevenda == "S" ? true : false,
                        mudarPrecoVenda = c.alterarpreco == "S" ? true : false,
                        taxaJurosDiario = c.fatjurdia,
                        posicaocodBarrasBalanca = c.posicaocodigobalanca,
                        vendaporClasse = c.PerClasse,
                        digitoVerCodBarras = c.digitoIniBal,
                        totalFinalCodBarrasBalanca = c.totalnofinalcodbalanca == "S" ? true : false
                    };

            foreach (var item in dados)
            {
                StandAloneConfigfinanc config = new StandAloneConfigfinanc();
                config.codigofilial = item.codigofilial;
                config.descontoMaxVenda = item.descontoMaxVenda;
                config.valorMaxVenda = item.valorMaxVenda;
                config.arredondamento = item.arredondamento;
                config.diasPrimeiroVenc = item.diasPrimeiroVenc;
                config.abaterCRcompraCH = item.abaterCRcompraCH;
                config.reservarEstoquePreVenda = item.reservarEstoquePreVenda;
                config.mudarPrecoVenda = item.mudarPrecoVenda;
                config.taxaJurosDiario = item.taxaJurosDiario;
                config.posicaoCodBarrasBalanca = item.posicaocodBarrasBalanca;
                config.vendaPorClasse = item.vendaporClasse == "S" ? true : false;
                config.digitoVerificador = item.digitoVerCodBarras ?? "2";
                config.totalnoFinalCodBalanca = item.totalFinalCodBarrasBalanca;
                lstconfig.Add(config);
            }
            tabela.Store(lstconfig);
            tabela.Close();
            return true;
        }

        public static bool CriarTabelaDadosEmpresa()
        {
            List<StandAloneDadosEmpresa> lstDados = new List<StandAloneDadosEmpresa>();
            IObjectContainer tabela = Db4oFactory.OpenFile(@"filiais.yap");
            siceEntities entidade = Conexao.CriarEntidade();
            var dados = from d in entidade.filiais
                        where d.CodigoFilial == GlbVariaveis.glb_filial
                        select new
                        {
                            codigofilial = d.CodigoFilial,
                            razaoSocial = d.empresa,
                            cnpj = d.cnpj,
                            insricao = d.inscricao,
                            inscricaoMunicipal = d.inscricaomunicipal
                        };
            foreach (var item in dados)
            {
                StandAloneDadosEmpresa dadoEmpresa = new StandAloneDadosEmpresa();
                dadoEmpresa.codigofilial = item.codigofilial;
                dadoEmpresa.razaoSocial = item.razaoSocial;
                dadoEmpresa.cnpj = item.cnpj;
                dadoEmpresa.inscricao = item.insricao;
                dadoEmpresa.inscricaoMunicipal = item.inscricaoMunicipal;
                lstDados.Add(dadoEmpresa);
            }
            tabela.Store(lstDados);
            tabela.Close();

            return true;
        }

        public static int SequenciaDoc()
        {
            try
            {
               siceEntities entidade = Conexao.CriarEntidade();
                int? doc = (from n in entidade.contdocs
                              select (Int32?)n.documento).Max();
                return doc.GetValueOrDefault();
            }
            catch (Exception)
            {
                throw new Exception("Sem comunicação com a base de dados");
            }
        }

        public static int QuantidadeRegistro()
        {
            using (IObjectContainer tabela = Db4oFactory.OpenFile("contdocs.yap"))
            {
                int? qtdReg = (from StandAloneContdocs n in tabela
                                 select (Int32?)n.documento).Count();
                return qtdReg.GetValueOrDefault();
            };
        }

        public bool Sincronizar(int documento = 0)
        {

            if (Conexao.tipoConexao == 1)
            {
                siceEntities entidade = Conexao.CriarEntidade();
                #region
                int seqDoc = SequenciaDoc();
                try
                {
                    // Sincronizando o Saldo Inicial do Caixa por que documento = 0
                    using (IObjectContainer tabCaixaSaldo = Db4oFactory.OpenFile("caixa.yap"))
                    {
                        var dadosSaldo = (from StandAloneCaixa n in tabCaixaSaldo
                                          where n.documento == 0
                                          select n).ToList();

                        foreach (var itemCaixa in dadosSaldo)
                        {
                            try
                            {
                                caixa registroCaixa = new caixa();
                                registroCaixa.documento = (int)itemCaixa.documento + seqDoc;
                                registroCaixa.data = itemCaixa.data;
                                registroCaixa.operador = itemCaixa.operador;
                                registroCaixa.vendedor = itemCaixa.vendedor == null ? "000" : itemCaixa.vendedor;
                                registroCaixa.tipopagamento = itemCaixa.tipoPagamento;
                                registroCaixa.valor = itemCaixa.valor;
                                registroCaixa.historico = "*";
                                registroCaixa.EnderecoIP = GlbVariaveis.glb_IP;
                                registroCaixa.dpfinanceiro = itemCaixa.dpFinanceiro;
                                registroCaixa.id = 0;
                                registroCaixa.CodigoFilial = GlbVariaveis.glb_filial;
                                registroCaixa.filialorigem = GlbVariaveis.glb_filial;
                                entidade.AddTocaixa(registroCaixa);
                                entidade.SaveChanges();
                            }
                            catch (Exception erro)
                            {
                                throw new Exception("Atenção: Exceção ao sincronizar caixa:" + erro.InnerException.ToString());
                            }
                        };
                    };

                    using (IObjectContainer tabDocs = Db4oFactory.OpenFile("contdocs.yap"))
                    {
                        var dadosDoc = (from StandAloneContdocs n in tabDocs
                                        select n).ToList();

                        foreach (var item in dadosDoc)
                        {
                            contdocs doc = new contdocs();
                            doc.documento = item.documento + seqDoc;
                            doc.ip = GlbVariaveis.glb_IP;
                            doc.id = GlbVariaveis.glb_IP;
                            doc.data = item.data;
                            doc.operador = item.operador;
                            doc.CodigoFilial = GlbVariaveis.glb_filial;
                            doc.Totalbruto = item.valorBruto;
                            doc.desconto = item.desconto;
                            doc.total = item.total;
                            doc.custos = item.custoItens;
                            doc.valorservicos = item.valorServico;
                            doc.descontoservicos = item.descontoServico;
                            doc.dpfinanceiro = item.dpFinanceiro;
                            doc.hora = item.hora;
                            doc.ncupomfiscal = item.COOCupomFiscal;
                            doc.ecfnumero = item.numeroECF;
                            doc.ecfcontadorcupomfiscal = item.CCFCupomFiscal;
                            doc.ecfConsumidor = item.consumidorECF;
                            doc.ecfCPFCNPJconsumidor = item.cpfCnpjConsumidor;
                            doc.ecfEndConsumidor = item.enderecoConsumidor;
                            doc.ecftotalliquido = item.totalLiquidoECF;
                            doc.contadordebitocreditoCDC = item.COODebitoCredito;
                            doc.totalICMScupomfiscal = item.icmsCupomFiscal;
                            doc.troco = item.troco;
                            doc.ecffabricacao = item.ecfFabricao;
                            doc.ecfMFadicional = item.ecfMFAdicional;
                            doc.ecftipo = item.ecfTipo;
                            doc.ecfmarca = ConfiguracoesECF.marcaECF; // item.ecfMarca;
                            doc.ecfmodelo = item.ecfModelo;
                            doc.historico = "OFF";
                            // Importante este item aqui por que o estoque dos
                            // Cupoms off line será atualizado através da Store Procedure
                            doc.estoqueatualizado = "N";

                            doc.bordero = "S";
                            doc.cartaofidelidade = "0";
                            doc.concluido = "N";
                            doc.entregaconcluida = "N";
                            doc.estadoentrega = "  ";
                            doc.estornado = "N";
                            doc.NF_e = "N";
                            doc.numeroentrega = "0";
                            doc.responsavelreceber = "";
                            doc.romaneio = "N";
                            doc.TEF = "N";
                            doc.prevendanumero = "0";
                            doc.davnumero = 0;
                            doc.tipopagamento = "DH";
                            doc.devolucaonumero = 0;
                            doc.serienf = "1";
                            doc.subserienf = "1";
                            doc.modeloDOCFiscal = "2D";
                            doc.COOGNF = "";
                            doc.contadornaofiscalGNF = " ";
                            doc.devolucaonumero = 0;
                            doc.tipopagamentoECF = "DH";

                            entidade.AddTocontdocs(doc);
                            entidade.SaveChanges();


                            using (IObjectContainer tabCaixa = Db4oFactory.OpenFile("caixa.yap"))
                            {
                                var dadosCaixa = (from StandAloneCaixa n in tabCaixa
                                                  where n.documento == item.documento
                                                  select n).ToList();

                                foreach (var itemCaixa in dadosCaixa)
                                {
                                    try
                                    {
                                        caixa registroCaixa = new caixa();
                                        registroCaixa.documento = (int)itemCaixa.documento + seqDoc;
                                        registroCaixa.data = itemCaixa.data;
                                        registroCaixa.operador = itemCaixa.operador;
                                        registroCaixa.vendedor = itemCaixa.vendedor == null ? "000" : itemCaixa.vendedor;
                                        registroCaixa.tipopagamento = itemCaixa.tipoPagamento;
                                        registroCaixa.valor = itemCaixa.valor;
                                        registroCaixa.historico = "*";
                                        registroCaixa.EnderecoIP = GlbVariaveis.glb_IP;
                                        registroCaixa.dpfinanceiro = itemCaixa.dpFinanceiro;
                                        registroCaixa.id = 0;
                                        registroCaixa.CodigoFilial = GlbVariaveis.glb_filial;
                                        registroCaixa.filialorigem = GlbVariaveis.glb_filial;
                                        entidade.AddTocaixa(registroCaixa);
                                        entidade.SaveChanges();
                                    }
                                    catch (Exception erro)
                                    {
                                        throw new Exception(" Exceção sincronizando caixa:" + erro.InnerException.ToString());
                                    }
                                };
                            }

                            using (IObjectContainer tabVenda = Db4oFactory.OpenFile("venda.yap"))
                            {

                                var dadosVenda = (from StandAloneVenda n in tabVenda
                                                  where n.documento == item.documento
                                                  select n).ToList();

                                foreach (var itemVenda in dadosVenda)
                                {
                                    venda registroVenda = new venda();
                                    registroVenda.documento = item.documento + seqDoc;
                                    registroVenda.aentregar = "N";
                                    registroVenda.codigofilial = GlbVariaveis.glb_filial;
                                    registroVenda.classe = "0000";
                                    registroVenda.codigobarras = "0";
                                    registroVenda.codigofiscal = "000";
                                    registroVenda.comissao = "A";
                                    registroVenda.grade = "nenhuma";
                                    registroVenda.id = GlbVariaveis.glb_IP;
                                    registroVenda.lote = "0";
                                    registroVenda.romaneio = "S";
                                    registroVenda.tipo = "0 - Produto";
                                    registroVenda.operador = item.operador;
                                    registroVenda.data = item.data.Date;
                                    registroVenda.Ecfnumero = item.numeroECF;
                                    registroVenda.codigocliente = 0;
                                    registroVenda.embalagem = 1;
                                    registroVenda.nrcontrole = itemVenda.nrcontrole;
                                    registroVenda.codigo = itemVenda.codigo;
                                    registroVenda.produto = itemVenda.descricao;
                                    registroVenda.quantidade = itemVenda.quantidade;
                                    registroVenda.preco = itemVenda.preco;
                                    registroVenda.acrescimototalitem = itemVenda.acrescimo;
                                    registroVenda.custo = itemVenda.custo;
                                    registroVenda.precooriginal = itemVenda.precoOriginal;
                                    registroVenda.unidade = itemVenda.unidade;
                                    registroVenda.Descontoperc = itemVenda.descontoperc;
                                    registroVenda.descontovalor = itemVenda.descontovalor;
                                    registroVenda.vendedor = itemVenda.vendedor;
                                    registroVenda.icms = Convert.ToInt16(itemVenda.icms);
                                    registroVenda.tributacao = itemVenda.tributacao;
                                    registroVenda.total = itemVenda.total;
                                    registroVenda.acrescimototalitem = itemVenda.acrescimo;
                                    registroVenda.cfop = "5.102";
                                    registroVenda.cstpis = "01";
                                    registroVenda.cstcofins = "01";
                                    registroVenda.serieNF = "1";
                                    registroVenda.subserienf = "1";
                                    registroVenda.modelodocfiscal = "2D";
                                    registroVenda.cancelado = itemVenda.cancelado;
                                    registroVenda.ecffabricacao = ConfiguracoesECF.nrFabricacaoECF;
                                    registroVenda.pcredsn = 0;
                                    registroVenda.vUnidIPI = 0;
                                    registroVenda.qUnidIPI = 0;
                                    registroVenda.ratfrete = 0;
                                    registroVenda.ratseguro = 0;
                                    registroVenda.ratdespesas = 0;
                                    registroVenda.origem = "0";
                                    registroVenda.cstpis = "01";
                                    registroVenda.cstcofins = "01";
                                    registroVenda.cstipi = "99";
                                    registroVenda.origem = "0";
                                    registroVenda.itemDAV = "S";
                                    registroVenda.canceladoECF = itemVenda.cancelado;
                                    entidade.AddTovenda(registroVenda);
                                    entidade.SaveChanges();
                                }
                            }
                            tabDocs.Delete(item);
                        } // Foreach contdocs.yap
                    }

                    using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                    {
                        conn.Open();
                        EntityCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "siceEntities.AtualizarEstoqueOff";
                        cmd.CommandType = CommandType.StoredProcedure;

                        EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                        filial.Direction = ParameterDirection.Input;
                        filial.Value = GlbVariaveis.glb_filial;
                        cmd.ExecuteNonQuery();
                    }


                    using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                    {
                        conn.Open();
                        EntityCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "siceEntities.AtualizarDadosOff";
                        cmd.CommandType = CommandType.StoredProcedure;

                    }

                    System.IO.File.Delete("venda.yap");
                    System.IO.File.Delete("caixa.yap");

                    return true;
                }
                catch (Exception erro)
                {
                    throw new Exception("Não foi possível completar a sincronização: " + erro.InnerException.ToString());
                }
                #endregion
            }
            else
            {
                try
                {
                    //IEnumerable<StandAloneControneSincronizacao> listDocumentos;
                    //siceEntities entidadeLocal = Conexao.CriarEntidade(false);
                    string crediario = "-";
                    string caixa = "-";
                    string venda = "-";
                    string cartao = "-";
                    string cheque = "-";
                    string produtos = "-";
                    string vendanf = "-";
                    string SQL = "";
                    StringBuilder SQLServidor = new StringBuilder();
                  
                    var listDocumentos = documentosSincrolizar(documento);

                    foreach (var item in listDocumentos)
                    {
                        siceEntities conexao;
                        if (Conexao.tipoConexao == 2)
                            conexao = Conexao.CriarEntidade(false);
                        else
                            conexao = Conexao.CriarEntidade();//conexao banco local quanto tipo = 3



                        bool online = true;
                        if (Conexao.tipoConexao == 3)
                            online = false;
                        else
                            online = true;


                        #region venda

                        var contDocs = (from d in conexao.contdocs
                                    where d.documento == item.documentoOrigem
                                    select d).FirstOrDefault();

                        int codigoCliente = 0;

                        if (contDocs.codigocliente > 0)
                        {
                            var codigoClienteOn = Conexao.CriarEntidade(false).ExecuteStoreQuery<int>("select codigosincronizacao from clientes where codigo = " + contDocs.codigocliente);
                            codigoCliente = codigoClienteOn.FirstOrDefault();
                        }

                        contDocs.codigocliente = codigoCliente;

                        SQLServidor.AppendLine("INSERT INTO contdocs" +
                                                "(ip, DATA, Totalbruto, dpfinanceiro, desconto, total, NrParcelas, vendedor, operador, Observacao, classe, codigocliente, nome, CodigoFilial, historico, vrjuros, " +
                                                "tipopagamento, encargos, id, estornado, enderecoentrega, custos, devolucaovenda, devolucaorecebimento, nrboletobancario, nrnotafiscal, classedevolucao, responsavelreceber," +
                                                "numeroentrega, cidadeentrega, cepentrega, bairroentrega, horaentrega, dataentrega, obsentrega,concluido, cartaofidelidade, bordero, valorservicos, descontoservicos, romaneio," +
                                                "hora, entregaconcluida, dataentregaconcluida, operadorentrega, ncupomfiscal, nreducaoz, ecfnumero, TEF, ecfValorCancelamentos, NF_e, estadoentrega, ecfConsumidor," +
                                                "ecfCPFCNPJconsumidor, ecfEndConsumidor, prevendanumero, ecfcontadorcupomfiscal, ecftotalliquido, contadornaofiscalGNF, contadordebitocreditoCDC, totalICMScupomfiscal," +
                                                "troco, davnumero, ecffabricacao, ecfMFadicional, ecftipo, ecfmarca, ecfmodelo, estoqueatualizado, serienf, tipopagamentoECF, modeloDOCFiscal, " +
                                                "subserienf, COOGNF, devolucaonumero, dependente, ecfusuariosubstituicao, chaveNFC, protocolo, dataAutorizacao, numerocheque, numeroServico ) VALUES (" +
                                                "'" + contDocs.ip + "','" + string.Format("{0:yyyy-MM-dd}", contDocs.data) + "','" + Funcoes.FormatarValorMysql(contDocs.Totalbruto.Value) + "', '" + contDocs.dpfinanceiro + "','" + Funcoes.FormatarValorMysql(contDocs.desconto.Value) + "','" + Funcoes.FormatarValorMysql(contDocs.total.Value) + "','" + contDocs.NrParcelas + "','" + contDocs.vendedor + "', " +
                                                "'" + contDocs.operador + "', '" + contDocs.Observacao + "', '" + contDocs.classe + "', '" + contDocs.codigocliente + "', '" + contDocs.nome + "', '" + contDocs.CodigoFilial + "', '" + contDocs.historico + "', '" + Funcoes.FormatarValorMysql(contDocs.vrjuros) + "', " +
                                                "'" + contDocs.tipopagamento + "', '" + Funcoes.FormatarValorMysql(contDocs.encargos) + "', '" + contDocs.id + "', '" + contDocs.estornado + "', '" + contDocs.enderecoentrega + "', '" + Funcoes.FormatarValorMysql(contDocs.custos) + "', " +
                                                "'" + Funcoes.FormatarValorMysql(contDocs.devolucaovenda) + "', '" + Funcoes.FormatarValorMysql(contDocs.devolucaorecebimento) + "', '" + contDocs.nrboletobancario + "', '" + contDocs.nrnotafiscal + "', '" + contDocs.classedevolucao + "', " +
                                                "'" + contDocs.responsavelreceber + "', '" + contDocs.numeroentrega + "', '" + contDocs.cidadeentrega + "', '" + contDocs.cepentrega + "', '" + contDocs.bairroentrega + "', '" + contDocs.horaentrega + "', " +
                                                "'" + Funcoes.FormatarDataMysql(contDocs.dataentrega == null ? DateTime.Parse("01-01-1900") : contDocs.dataentrega.Value) + "', '" + contDocs.obsentrega + "', '" + contDocs.concluido + "', '" + contDocs.cartaofidelidade + "', '" + contDocs.bordero + "', '" + Funcoes.FormatarValorMysql(contDocs.valorservicos) + "', '" + Funcoes.FormatarValorMysql(contDocs.descontoservicos) + "', '" + contDocs.romaneio + "', " +
                                                "'" + contDocs.hora + "', '" + contDocs.entregaconcluida + "', '" + Funcoes.FormatarDataMysql(contDocs.dataentregaconcluida == null ? DateTime.Parse("01-01-1900") : contDocs.dataentregaconcluida.Value) + "', '" + contDocs.operadorentrega + "', '" + contDocs.ncupomfiscal + "', '" + contDocs.nreducaoz + "', '" + contDocs.ecfnumero + "', '" + contDocs.TEF + "', " +
                                                "'" + Funcoes.FormatarValorMysql(contDocs.ecfValorCancelamentos) + "', '" + contDocs.NF_e + "', '" + contDocs.estadoentrega + "', '" + contDocs.ecfConsumidor + "', '" + contDocs.ecfCPFCNPJconsumidor + "', '" + contDocs.ecfEndConsumidor + "', " +
                                                "'" + contDocs.prevendanumero + "', '" + contDocs.ecfcontadorcupomfiscal + "', '" + Funcoes.FormatarValorMysql(contDocs.ecftotalliquido) + "', '" + contDocs.contadornaofiscalGNF + "', '" + contDocs.contadordebitocreditoCDC + "', " +
                                                "'" + Funcoes.FormatarValorMysql(contDocs.totalICMScupomfiscal) + "', '" + Funcoes.FormatarValorMysql(contDocs.troco) + "', '" + contDocs.davnumero + "', '" + contDocs.ecffabricacao + "', '" + contDocs.ecfMFadicional + "', '" + contDocs.ecftipo + "', '" + contDocs.ecfmarca + "', " +
                                                "'" + contDocs.ecfmodelo + "', 'N', '" + contDocs.serienf + "', '" + contDocs.tipopagamentoECF + "', '" + contDocs.modeloDOCFiscal + "', '" + contDocs.subserienf + "', " +
                                                "'" + contDocs.COOGNF + "', '" + contDocs.devolucaonumero + "', '" + contDocs.dependente + "', '" + contDocs.ecfusuariosubstituicao + "', '" + contDocs.chaveNFC + "', '" + contDocs.protocolo + "', " +
                                                "'" + Funcoes.FormatarDataMysql(contDocs.dataAutorizacao == null ? DateTime.Parse("01-01-1900") : contDocs.dataAutorizacao.Value) + "', '" + contDocs.numerocheque+"', '" + contDocs.numeroServico + "');");

                        SQLServidor.AppendLine("#fim contDocs");


                        var listVenda = (from c in conexao.venda
                                        where c.documento == item.documentoOrigem
                                        select c).ToList();


                        foreach (var produto in listVenda)
                        {


                           
                            SQLServidor.AppendLine("INSERT INTO venda" +
                                                        "(codigofilial, operador, DATA, codigo, produto, quantidade, preco, custo, precooriginal, Descontoperc, id, descontovalor, total, vendedor," +
                                                        "nrcontrole, documento, grupo, subgrupo, comissao, ratdesc, rateioencargos, situacao, customedio, Ecfnumero, fornecedor, fabricante, NotaFiscal, " +
                                                        "icms, classe, secao, lote, tributacao, aentregar,quantidadeanterior, quantidadeatualizada, codigofiscal, customedioanterior, codigocliente, " +
                                                        "numerodevolucao, codigobarras, ipi, unidade, embalagem, grade, romaneio, tipo, cofins, pis, despesasacessorias, percentualRedBaseCalcICMS, serieNF," +
                                                        "cfop, acrescimototalitem, cstpis, cstcofins, icmsst, percentualRedBaseCalcICMSST, mvast, subserienf, modelodocfiscal, aliquotaIPI, ecffabricacao, coo," +
                                                        "cancelado, ccf, pcredsn, qUnidIPI, vUnidIPI, origem, ncm, nbm, ncmespecie, cstipi, ratfrete, ratseguro, ratdespesas, datafabricacao," +
                                                        "vencimentoproduto, modalidadeDetBaseCalcICMS, modalidadeDetBaseCalcICMSst, idfornecedor, vencimento, dataalteracao, horaalteracao, tipoalteracao, itemDAV," +
                                                        "canceladoECF, vendaatacado, pautaICMS, pautaICMSST, cenqipi, cest, dpfinanceiro, vbcICMSST, vICMSST, custogerencial) VALUES (" +
                                                        "'" + produto.codigofilial + "', '" + produto.operador + "', '" + string.Format("{0:yyyy-MM-dd}", produto.data) + "', '" + produto.codigo + "', '" + produto.produto + "', '" + 
                                                        Funcoes.FormatarValorMysql(produto.quantidade) + "', '" + Funcoes.FormatarValorMysql(produto.preco.Value) + "', '" + Funcoes.FormatarValorMysql(produto.custo.Value) + "', '" + 
                                                        Funcoes.FormatarValorMysql(produto.precooriginal) + "', '" + Funcoes.FormatarValorMysql(produto.Descontoperc) + "', '" + produto.id + "', '" + Funcoes.FormatarValorMysql(produto.descontovalor) + "', " +
                                                        "'" + Funcoes.FormatarValorMysql(produto.total) + "', '" + produto.vendedor + "', '" + produto.nrcontrole + "', (select max(documento) from contdocs where ip = '" + produto.id + 
                                                        "' and codigoFilial ='" + produto.codigofilial + "'), '" + produto.grupo + "', '" + produto.subgrupo + "', '" + produto.comissao + "', '" + Funcoes.FormatarValorMysql(produto.ratdesc) + 
                                                        "', '" + Funcoes.FormatarValorMysql(produto.rateioencargos) + "', '" + produto.situacao + "', '" + Funcoes.FormatarValorMysql(produto.customedio.Value) + "', '" + produto.Ecfnumero + "', '" 
                                                        + produto.fornecedor + "', '" + produto.fabricante + "', '" + produto.NotaFiscal + "', '" + Funcoes.FormatarValorMysql(produto.icms) + "', '" + produto.classe + "', '" + produto.secao + "', '" + 
                                                        produto.lote + "', '" + produto.tributacao + "', '" + produto.aentregar + "', '" + Funcoes.FormatarValorMysql(produto.quantidadeanterior) + "', '" + 
                                                        Funcoes.FormatarValorMysql(produto.quantidadeatualizada) + "', '" + produto.codigofiscal + "', '" + Funcoes.FormatarValorMysql(produto.customedioanterior) + "', '" + produto.codigocliente + "', " +
                                                        "'" + produto.numerodevolucao + "', '" + produto.codigobarras + "', '" + produto.ipi + "', '" + produto.unidade + "', '" + produto.embalagem + "', '" + produto.grade + "', '" + produto.romaneio + "', " +
                                                        "'" + produto.tipo + "', '" + Funcoes.FormatarValorMysql(produto.cofins.Value) + "', '" + Funcoes.FormatarValorMysql(produto.pis.Value) + "', '" + Funcoes.FormatarValorMysql(produto.despesasacessorias) + 
                                                        "', '" + Funcoes.FormatarValorMysql(produto.percentualRedBaseCalcICMSST) + "', '" + produto.serieNF + "','" + produto.cfop + "', '" + Funcoes.FormatarValorMysql(produto.acrescimototalitem) + 
                                                        "', '" + produto.cstpis + "', '" + produto.cstcofins + "', '" + Funcoes.FormatarValorMysql(produto.icmsst) + "', '" + Funcoes.FormatarValorMysql(produto.percentualRedBaseCalcICMSST) + "', " +
                                                        "'" + Funcoes.FormatarValorMysql(produto.mvast) + "', '" + produto.subserienf + "', '" + produto.modelodocfiscal + "', '" + Funcoes.FormatarValorMysql(produto.aliquotaIPI.Value) + "', '" + 
                                                        produto.ecffabricacao + "', '" + produto.coo + "', '" + produto.cancelado + "', '" + produto.ccf + "', '" + Funcoes.FormatarValorMysql(produto.pcredsn) + "', '" + 
                                                        Funcoes.FormatarValorMysql(produto.qUnidIPI) + "', '" + Funcoes.FormatarValorMysql(produto.vUnidIPI) + "', '" + produto.origem + "', '" + produto.ncm + "', " +
                                                        "'" + produto.nbm + "', '" + produto.ncmespecie + "', '" + produto.cstipi + "', '" + Funcoes.FormatarValorMysql(produto.ratfrete) + "', '" + Funcoes.FormatarValorMysql(produto.ratseguro) + 
                                                        "', '" + Funcoes.FormatarValorMysql(produto.ratdespesas) + "', '" + Funcoes.FormatarDataMysql(produto.datafabricacao == null ? DateTime.Parse("01-01-1900") : produto.datafabricacao.Value) + "', " +
                                                        "'" + Funcoes.FormatarDataMysql(produto.vencimentoproduto == null ? DateTime.Parse("01-01-1900") : produto.vencimentoproduto.Value) + "', '" + produto.modalidadeDetBaseCalcICMS + "', '" + 
                                                        produto.modalidadeDetBaseCalcICMSst + "', '" + (produto.idfornecedor == null ? '0': produto.idfornecedor) + "', '" + Funcoes.FormatarDataMysql(produto.vencimento == null ? DateTime.Parse("01-01-1900") : produto.vencimento.Value) + "', " +
                                                        "'" + Funcoes.FormatarDataMysql(produto.dataalteracao == null ? DateTime.Parse("01-01-1900") : produto.dataalteracao.Value) + "', '" + Funcoes.FormatarHoraMysql(produto.horaalteracao == null ? TimeSpan.Parse("00:00:00") : produto.horaalteracao.Value) + "', '" + produto.tipoalteracao + "', '" + 
                                                        produto.itemDAV + "', '" + produto.canceladoECF + "', '" + produto.vendaatacado + "', '" + Funcoes.FormatarValorMysql(produto.pautaICMS) + "', '" + 
                                                        Funcoes.FormatarValorMysql(produto.pautaICMSST) + "', '" + produto.cenqipi + "', '" + produto.cest + "', '" + produto.dpfinanceiro + "', '" + 
                                                        Funcoes.FormatarValorMysql(produto.vbcICMSST == null ? Decimal.Parse("0.00"):produto.vbcICMSST.Value) + "', '" + Funcoes.FormatarValorMysql(produto.vICMSST == null ? Decimal.Parse("0.00") : produto.vICMSST.Value) + "', '" +
                                                        Funcoes.FormatarValorMysql(produto.custogerencial == null ? Decimal.Parse("0.00") : produto.custogerencial.Value) + "');");

                            SQLServidor.AppendLine("#Fim Venda");

                        }
                        venda = "S";
                        #endregion

                        #region caixa

                        var transacoesCaixa = (from c in conexao.caixa
                                            where c.documento == item.documentoOrigem
                                            select c).ToList();

                        var listdinheiro = (from c in transacoesCaixa where c.tipopagamento == "DH" select c).ToList();
                        var listcrediario = (from c in transacoesCaixa where c.tipopagamento == "CR" select c).ToList();
                        var listcartao = (from c in transacoesCaixa where c.tipopagamento == "CA" select c).ToList();
                        var listcheque = (from c in transacoesCaixa where c.tipopagamento == "CH" select c).ToList();

                        foreach (var c in transacoesCaixa)
                        {
                            c.codigocliente = codigoCliente;

                            SQLServidor.AppendLine("INSERT INTO caixa " +
                                                    "(horaabertura, EnderecoIP, documento, tipopagamento, valor, dataexe, DATA, CodigoFilial, VrJuros, jurosch, vrdesconto, vendedor, datapagamento," +
                                                    "vencimento, nome, sequencia, caixa, financeira, CrInicial, CrFinal, banco, cheque, agencia, valorCheque, Cartao, numeroCartao, Nrparcela, encargos," +
                                                    "NomeCheque, classe, codigocliente, operador, historico, dpfinanceiro, custos, ocorrencia, filialorigem, valortarifabloquete, cobrador, contacorrentecheque," +
                                                    "jurosfactoring, versao, valorservicos, descontoservicos, jurosCA, cpfcnpjch, coo, gnf, ccf, estornado, ecffabricacao, ecfmodelo, ecfnumero, descricaopag," +
                                                    "tipodoc) VALUES (" +
                                                    "'" + Funcoes.FormatarHoraMysql(c.horaabertura == null ? TimeSpan.Parse("00:00:00") : c.horaabertura.Value) + "', '" + c.EnderecoIP + "', (select max(documento) from contdocs where ip = '" + c.EnderecoIP + "' and codigoFilial ='" + c.CodigoFilial + "'), '" + c.tipopagamento + "', '" + Funcoes.FormatarValorMysql(c.valor) + "', '" + Funcoes.FormatarDataMysql(c.dataexe == null ? DateTime.Parse("01-01-1900") : c.dataexe.Value) + "', '" + string.Format("{0:yyyy-MM-dd}", c.data)+ "', '" + c.CodigoFilial + "', " +
                                                    "'" + Funcoes.FormatarValorMysql(c.VrJuros) + "', '" + Funcoes.FormatarValorMysql(c.jurosch) + "', '" + Funcoes.FormatarValorMysql(c.vrdesconto) + "', '" + c.vendedor + "', '" + Funcoes.FormatarDataMysql(c.datapagamento == null ? DateTime.Parse("01-01-1900") : c.datapagamento.Value) + "', '" + Funcoes.FormatarDataMysql(c.vencimento == null ? DateTime.Parse("01-01-1900") : c.vencimento.Value) + "', '" + c.nome + "', '" + (c.sequencia == null ? 0 : c.sequencia).ToString() + "', " +
                                                    "'" + Funcoes.FormatarValorMysql(c.caixa1 == null ? decimal.Parse("0.00") : c.caixa1.Value) + "', '" + c.financeira + "', '" + Funcoes.FormatarValorMysql(c.CrInicial) + "', '" + Funcoes.FormatarValorMysql(c.CrFinal) + "', '" + c.banco + "', '" + (c.cheque == null ? 0 : c.cheque).ToString() + "', '" + c.agencia + "', '" + Funcoes.FormatarValorMysql(c.valorCheque) + "', " +
                                                    "'" + c.Cartao + "', '" + c.numeroCartao + "', '" + c.Nrparcela + "', '" + Funcoes.FormatarValorMysql(c.encargos) + "', '" + c.NomeCheque + "', '" + c.classe + "', '" + c.codigocliente + "', '" + c.operador + "', "+
                                                    "'" + c.historico+ "', '" + c.dpfinanceiro + "', '" + Funcoes.FormatarValorMysql(c.custos) + "', '" + c.ocorrencia + "', '" + c.filialorigem + "', '" + Funcoes.FormatarValorMysql(c.valortarifabloquete) + "', '" + c.cobrador + "', '" + c.contacorrentecheque + "', '" + Funcoes.FormatarValorMysql(c.jurosfactoring) + "', " +
                                                    "'" + c.versao + "', '" + Funcoes.FormatarValorMysql(c.valorservicos) + "', " +
                                                    "'" + Funcoes.FormatarValorMysql(c.descontoservicos) + "', '" + Funcoes.FormatarValorMysql(c.jurosCA) + "', '" + c.cpfcnpjch + "', '" + c.coo + "', '" + c.gnf + "', '" + c.ccf + "', '" + c.estornado + "', '" + c.ecffabricacao + "', " +
                                                    "'" + c.ecfmodelo + "', '" + c.ecfnumero + "', '" + c.descricaopag + "', '" + c.tipodoc + "');");

                            SQLServidor.AppendLine("#fim caixa DH");

                        }
                        caixa = "S";

                        #region  crediario
                        if (listcrediario.Count() > 0)
                        {

                            /*
                            var parcelasCR = (from c in conexao.crmovclientes
                                            where c.documento == item.documentoOrigem
                                            select c).ToList();
                            

                            foreach (var p in parcelasCR)
                            {
                                using (entidade = Conexao.CriarEntidade(online))
                                {
                                    p.codigo = codigoCliente;


                                    entidade.AddTocrmovclientes(new crmovclientes
                                    {
                                        #region crmovclientes
                                        nome = p.nome,
                                        codigo = p.codigo,
                                        documento = documentoOnline,
                                        datacompra = p.datacompra,
                                        datarenegociacao = p.datarenegociacao,
                                        vencimento = p.vencimento,
                                        jurospago = p.jurospago,
                                        datapagamento = p.datapagamento,
                                        Ultjurospago = p.Ultjurospago,
                                        ultvencimento = p.ultvencimento,
                                        parcela = p.parcela,
                                        Valor = p.Valor,
                                        percmulta = p.percmulta,
                                        vrmulta = p.vrmulta,
                                        percjuro = p.percjuro,
                                        vrjuros = p.vrjuros,
                                        valoratual = p.valoratual,
                                        ultvaloratual = p.ultvaloratual,
                                        VrCapitalRec = p.VrCapitalRec,
                                        DescontoCap = p.DescontoCap,
                                        UltCapRec = p.UltCapRec,
                                        DescontoJur = p.DescontoJur,
                                        TotalDescontos = p.TotalDescontos,
                                        ValorliquidoRec = p.ValorliquidoRec,
                                        ultpagamento = p.ultpagamento,
                                        usuario = p.usuario,
                                        vendedor = p.vendedor,
                                        Diasdecorrido = p.Diasdecorrido,
                                        sequencia = p.sequencia,
                                        Observacao = p.Observacao,
                                        nrParcela = p.nrParcela,
                                        encargos = p.encargos,
                                        bloquete = p.bloquete,
                                        classe = p.classe,
                                        vrultpagamento = p.vrultpagamento,
                                        quitado = p.quitado,
                                        datacalcjuros = p.datacalcjuros,
                                        sequenciainc = p.sequenciainc,
                                        valorcorrigido = p.valorcorrigido,
                                        CodigoFilial = p.CodigoFilial,
                                        porconta = p.porconta,
                                        valorpago = p.valorpago,
                                        porcontd = p.porcontd,
                                        jurosacumulado = p.jurosacumulado,
                                        ultjurosacumulado = p.ultjurosacumulado,
                                        desconto = p.desconto,
                                        dependente = p.dependente,
                                        tipopagamento = p.tipopagamento,
                                        ultporconta = p.ultporconta,
                                        dpfinanceiro = p.dpfinanceiro,
                                        cpfcnpj = p.cpfcnpj,
                                        cobrador = p.cobrador,
                                        filialpagamento = p.filialpagamento,
                                        ip = p.ip,
                                        comissaopaga = p.comissaopaga,
                                        interpolador = p.interpolador,
                                        mes = p.mes,
                                        titulo = p.titulo,
                                        nfe = p.nfe,
                                        spc = p.spc,
                                        dataspc = p.dataspc
                                        #endregion
                                    });
                                    entidade.SaveChanges();
                                }
                            }
                            crediario = "S";
                            */
                        }
                        #endregion

                        #region cartao
                        if (listcartao.Count() > 0)
                        {
                            var parcelasCA = (from c in conexao.movcartoes
                                            where c.documento == item.documentoOrigem
                                            select c).ToList();

                            foreach (var p in parcelasCA)
                            {
                                SQLServidor.AppendLine("INSERT INTO movcartoes" +
                                                        "(cartao, numero, DATA, vencimento, nome, taxadesconto, valor, documento, operador, CodigoFilial, Historico," +
                                                        "dpfinanceiro, depositado, marcado, encargos, interpolador, autorizacao, datadeposito, tipo, ip, vendedor, comissaopaga)" +
                                                        "VALUES(" +
                                                        "'" + p.cartao + "', '" + p.numero + "', '" + Funcoes.FormatarDataMysql(p.data == null ? DateTime.Parse("01-01-1900"): p.data.Value)+ "', '" + Funcoes.FormatarDataMysql(p.vencimento == null ? DateTime.Parse("01-01-1900") : p.vencimento.Value) + "','" + p.nome + "','" + Funcoes.FormatarValorMysql(p.taxadesconto == null ? decimal.Parse("0.00") : p.taxadesconto.Value) + "', '" + Funcoes.FormatarValorMysql(p.valor == null ? decimal.Parse("0.00") : p.valor.Value) + "', (select max(documento) from contdocs where ip = '" + p.id + "' and codigoFilial ='" + p.CodigoFilial + "')," +
                                                        "'" + p.operador + "','" + p.CodigoFilial + "','" + p.Historico + "','" + p.dpfinanceiro + "','" + p.depositado + "','" + p.marcado + "','" + Funcoes.FormatarValorMysql(p.encargos) + "'," +
                                                        "'" + p.interpolador + "', '" + p.autorizacao + "','" + Funcoes.FormatarDataMysql(p.datadeposito == null ? DateTime.Parse("01-01-1900") : p.datadeposito.Value) + "','" + p.tipo + "','" + p.ip + "','" + p.vendedor + "','" + p.comissaopaga + "');");

                                SQLServidor.AppendLine("#fim Movcartoes");

                                #region antes Caixa
                                /*
                                using (entidade = Conexao.CriarEntidade(online))
                                {
                                    entidade.AddTomovcartoes(new movcartoes
                                    {
                                        cartao = p.cartao,
                                        numero = p.numero,
                                        data = p.data,
                                        vencimento = p.vencimento,
                                        nome = p.nome,
                                        taxadesconto = p.taxadesconto,
                                        valor = p.valor,
                                        documento = documentoOnline,
                                        operador = p.operador,
                                        CodigoFilial = p.CodigoFilial,
                                        Historico = p.Historico,
                                        dpfinanceiro = p.dpfinanceiro,
                                        depositado = p.depositado,
                                        marcado = p.marcado,
                                        encargos = p.encargos,
                                        interpolador = p.interpolador,
                                        autorizacao = p.autorizacao,
                                        datadeposito = p.datadeposito,
                                        tipo = p.tipo,
                                        ip = p.vendedor,
                                        comissaopaga = p.comissaopaga
                                    });
                                    entidade.SaveChanges();
                                }
                                */
                                #endregion

                                //entidade.AddTomovcartoes(p);
                            }

                            cartao = "S";
                        }
                        #endregion

                        #region cheque
                        if (listcheque.Count() > 0)
                        {
                            var parcelasCH = (from c in conexao.cheques
                                            where c.documento == item.documentoOrigem
                                            select c).ToList();

                            foreach (var p in parcelasCH)
                            {
                                p.codigocliente = codigoCliente;

                                SQLServidor.AppendLine("INSERT INTO cheques" +
                                                        "(destinatario, repassado, datarepasse, banco, cheque, agencia, documento, valor, ValorCheque, cliente," +
                                                        "nomecheque, DATA, cpf, telefone, vencimento, observacao, marcado, semfundo, taxadesconto, datadesconto, codigocliente," +
                                                        "CodigoFilial, Historico, dpfinanceiro, cpfcheque, depositado, encargos, operador, datadevolvido, interpolador, interpoladorant," +
                                                        "contabancaria, ip, tipo, vencimentooriginal, operadorprorrogacao, dataprorrogacao, datatroca, valordesconto, vendedor," +
                                                        "comissaopaga, descontado, recebido, dataRecebimento)" +
                                                        "VALUES(" +
                                                        "'" + p.destinatario + "', '" + p.repassado + "', '" + Funcoes.FormatarDataMysql(p.datarepasse == null ? DateTime.Parse("01-01-1900") : p.datarepasse.Value) + "', '"
                                                        + p.banco + "', '" + p.cheque + "', '" + p.agencia + "', (select max(documento) from contdocs where ip = '" + p.id + "' and codigoFilial ='" + p.CodigoFilial + "'), " +
                                                        "'" + Funcoes.FormatarValorMysql(p.valor == null ? decimal.Parse("0.00") : p.valor.Value) + "', '" + Funcoes.FormatarValorMysql(p.ValorCheque.Value) + "', '" + p.cliente +
                                                        "', '" + p.nomecheque + "', '" + Funcoes.FormatarDataMysql(p.data == null ? DateTime.Parse("01-01-1900") : p.data.Value) + "', '" + p.cpf + "', '" + p.telefone + "', '" +
                                                        Funcoes.FormatarDataMysql(p.vencimento == null ? DateTime.Parse("01-01-1900") : p.vencimento.Value) + "', " +
                                                        "'" + p.observacao + "', '" + p.marcado + "', '" + p.semfundo + "', '" + Funcoes.FormatarValorMysql(p.taxadesconto == null ? decimal.Parse("0.00") : p.taxadesconto.Value) + "', '" +
                                                        Funcoes.FormatarDataMysql(p.datadesconto == null ? DateTime.Parse("01-01-1900") : p.datadesconto.Value) + "', '" + p.codigocliente + "', '" + p.CodigoFilial + "', " +
                                                        "'" + p.Historico + "', '" + p.dpfinanceiro + "', '" + p.cpfcheque + "', '" + p.depositado + "', '" + Funcoes.FormatarValorMysql(p.encargos) + "', '" + p.operador + "', '" +
                                                        Funcoes.FormatarDataMysql(p.datadevolvido == null ? DateTime.Parse("01-01-1900") : p.datadevolvido.Value) + "', " +
                                                        "'" + p.interpolador + "', '" + p.interpoladorant + "', '" + p.contabancaria + "', '" + p.ip + "', '" + p.tipo + "', '" + Funcoes.FormatarDataMysql(p.vencimentooriginal == null ? DateTime.Parse("01-01-1900") : p.vencimentooriginal.Value) + "', '" + 
                                                        p.operadorprorrogacao + "', '" + Funcoes.FormatarDataMysql(p.dataprorrogacao == null ? DateTime.Parse("01-01-1900") : p.dataprorrogacao.Value) + "', '" + 
                                                        Funcoes.FormatarDataMysql(p.datatroca == null ? DateTime.Parse("01-01-1900") : p.datatroca.Value) + "', '" + Funcoes.FormatarValorMysql(p.valordesconto) + "', '" + 
                                                        p.vendedor + "', '" + p.comissaopaga + "', '" + p.descontado + "', '" + p.recebido + "', '" + 
                                                        Funcoes.FormatarDataMysql(p.dataRecebimento == null ? DateTime.Parse("01-01-1900") : p.dataRecebimento.Value) + "'); ");

                                SQLServidor.AppendLine("#fim cheque");
                                
                                #region antes cheques
                                /*
                                using (entidade = Conexao.CriarEntidade(online))
                                {
                                    entidade.AddTocheques(new cheques
                                    {
                                        #region
                                        agencia = p.agencia,
                                        banco = p.banco,
                                        cheque = p.cheque,
                                        cliente = p.cliente,
                                        codigocliente = p.codigocliente,
                                        CodigoFilial = p.CodigoFilial,
                                        comissaopaga = p.comissaopaga,
                                        contabancaria = p.contabancaria,
                                        cpf = p.cpf,
                                        cpfcheque = p.cpfcheque,
                                        data = p.data,
                                        datadesconto = p.datadesconto,
                                        datadevolvido = p.datadevolvido,
                                        dataprorrogacao = p.dataprorrogacao,
                                        datarepasse = p.datarepasse,
                                        datatroca = p.datatroca,
                                        depositado = p.depositado,
                                        destinatario = p.destinatario,
                                        documento = documentoOnline,
                                        dpfinanceiro = p.dpfinanceiro,
                                        encargos = p.encargos,
                                        Historico = p.Historico,
                                        interpolador = p.interpolador,
                                        interpoladorant = p.interpoladorant,
                                        ip = p.ip,
                                        marcado = p.marcado,
                                        nomecheque = p.nomecheque,
                                        observacao = p.observacao == null ? "" : p.observacao,
                                        operador = p.operador,
                                        operadorprorrogacao = p.operadorprorrogacao,
                                        repassado = p.repassado,
                                        semfundo = p.semfundo,
                                        taxadesconto = p.taxadesconto,
                                        telefone = p.telefone,
                                        tipo = p.tipo,
                                        valor = p.valor,
                                        ValorCheque = p.ValorCheque,
                                        valordesconto = p.valordesconto == null ? 0 : p.valordesconto,
                                        vencimento = p.vencimento,
                                        vencimentooriginal = p.vencimentooriginal,
                                        vendedor = p.vendedor,
                                        descontado = p.descontado
                                        #endregion
                                    });
                                    entidade.SaveChanges();
                                }
                                */
                                #endregion
                                //entidade.AddTocheques(p);
                            }
                            cheque = "S";

                        }
                        #endregion

                        #endregion

                        #region auditoria 

                        IEnumerable<auditoria> listAuditoria;
                        listAuditoria = conexao.ExecuteStoreQuery<auditoria>("SELECT * FROM auditoria WHERE documento = '" + item.documentoOrigem + "' AND codigofilial = '" + GlbVariaveis.glb_filial + "'");

                        foreach (var d in listAuditoria)
                        {

                            SQLServidor.AppendLine("INSERT INTO auditoria" +
                                                    "(usuario, usuariosolicitante, hora, DATA, tabela, acao, documento, observacao, CodigoFilial," +
                                                    " LOCAL, codigoproduto)" +
                                                    "VALUES("+
                                                    "'"+d.usuario+ "', '" + d.usuariosolicitante + "', '" + Funcoes.FormatarHoraMysql(d.hora == null ? TimeSpan.Parse("00:00:00") : d.hora.Value) + "', '" + Funcoes.FormatarDataMysql(d.data == null ? DateTime.Parse("01-01-1900"): d.data.Value) + "', '" + d.tabela + "', '" + d.acao + "', '" + d.documento + "', "+
                                                    "'" + d.observacao + "', '" + d.CodigoFilial + "', '" + d.local + "', '" + d.codigoproduto + "');");

                            SQLServidor.AppendLine("#fim Auditoria");
                            
                            #region antes auditoria
                            /*
                            using (entidade = Conexao.CriarEntidade(online))
                            {
                                entidade.AddToauditoria(new auditoria
                                {
                                    acao = d.acao,
                                    CodigoFilial = d.CodigoFilial,
                                    codigoproduto = d.codigoproduto,
                                    data = d.data,
                                    documento = documentoOnline,
                                    hora = d.hora,
                                    local = d.local,
                                    observacao = d.observacao,
                                    tabela = d.tabela,
                                    usuario = d.usuario,
                                    usuariosolicitante = d.usuariosolicitante
                                });
                                entidade.SaveChanges();
                            }
                            */
                            #endregion

                            //entidade.AddToauditoria(d);
                        }

#endregion

                        //Conexao.CriarEntidade().SaveChanges();
                        SQLServidor.AppendLine("CALL AtualizarEstoqueOffline('" + GlbVariaveis.glb_filial + "',(select max(documento) from contdocs where ip = '" +GlbVariaveis.glb_IP + "' and codigoFilial ='" + GlbVariaveis.glb_filial + "'));");

                       /* if(GlbVariaveis.glb_filial == "00001")
                            SQLServidor.AppendLine("UPDATE venda AS v, produtos AS p SET p.sincronizar = 'S' WHERE p.codigo = v.codigo AND v.id = '" + GlbVariaveis.glb_IP + "' AND v.documento = (SELECT MAX(documento) FROM contdocs WHERE ip = '"+ GlbVariaveis.glb_IP + "' AND codigoFilial = '"+ GlbVariaveis.glb_filial+ "') AND p.CodigoFilial = '" + GlbVariaveis.glb_filial + "';");
                        else
                            SQLServidor.AppendLine("UPDATE venda AS v, produtosfilial AS p SET p.sincronizar = 'S' WHERE p.codigo = v.codigo AND v.id = '" + GlbVariaveis.glb_IP + "' AND v.documento = (SELECT MAX(documento) FROM contdocs WHERE ip = '" + GlbVariaveis.glb_IP + "' AND codigoFilial = '" + GlbVariaveis.glb_filial + "') AND p.CodigoFilial = '"+ GlbVariaveis.glb_filial + "';");

                        SQLServidor.AppendLine("CALL GravarControleSincronia('"+ GlbVariaveis.glb_filial + "');");*/

                        string SQLComit = SQLServidor.ToString();
                        Conexao.CriarEntidade(online).ExecuteStoreCommand(SQLComit);

                        //SQL = "CALL AtualizarEstoqueOffline('" + GlbVariaveis.glb_filial + "','" + documentoOnline + "')";
                        //conexao.ExecuteStoreCommand(SQL);

                        string documentoOnline = Conexao.CriarEntidade(online).ExecuteStoreQuery<string>("select max(documento) from contdocs where ip = '" + GlbVariaveis.glb_IP + "' and codigoFilial ='" + GlbVariaveis.glb_filial + "'").FirstOrDefault();

                        SQL = "UPDATE controlesincronizacao SET crediario = '" + crediario + "',caixa = '" + caixa + "', venda = '" + venda + "', cartao = '" + cartao + "', cheque = '" + cheque + "', produtos = '" + produtos + "', vendanf = '" + vendanf + "', documentoDestino = '" + documentoOnline + "', dataSincronizacao = current_date WHERE documentoOrigem = '" + item.documentoOrigem + "'";
                        if (Conexao.tipoConexao == 2)
                            Conexao.CriarEntidade(false).ExecuteStoreCommand(SQL);
                        else
                            Conexao.CriarEntidade().ExecuteStoreCommand(SQL);

                        string sql = "CALL AtualizarQdtRegistros()";
                        Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                    }

                    return true;
                    
                }
                catch (Exception erro)
                {
                    if (erro.Message.Contains("O provedor subjacente falhou em Open.") == false)
                    {
                        MessageBox.Show(erro.ToString());
                    }

                    MessageBox.Show(erro.ToString());

                    return false;
                }
            }
        }




        //+============+
        //| Marckvaldo |
        //+============+



        public static IEnumerable<StandAloneControneSincronizacao> documentosSincrolizar(int documento = 0)
        {
            string SQL;

            siceEntities conexao;
            if (Conexao.tipoConexao == 2)
                conexao = Conexao.CriarEntidade(false);
            else
                conexao = Conexao.CriarEntidade();

            if (documento == 0)
            {
                SQL = "SELECT documentoOrigem,documentoDestino,crediario,caixa,venda,cartao,cheque,produtos,vendanf,contnfsaida,datasincronizacao FROM controlesincronizacao WHERE documentoDestino = 0";
            }
            else
            {
                SQL = "SELECT documentoOrigem,documentoDestino,crediario,caixa,venda,cartao,cheque,produtos,vendanf,contnfsaida,datasincronizacao FROM controlesincronizacao WHERE documentoOrigem = '" + documento + "'";
            }

            //entidade = Conexao.CriarEntidade(false);
            listDocumentosSincrozizar = conexao.ExecuteStoreQuery<StandAloneControneSincronizacao>(SQL);
            return listDocumentosSincrozizar;
        }

        public static int quantidadeDocumentoSincronizar()
        {

            try
            {
                string SQL;
                SQL = "SELECT count(1)  FROM controlesincronizacao WHERE documentoDestino = 0";

                siceEntities conexao;
                if (Conexao.tipoConexao == 2)
                    conexao = Conexao.CriarEntidade(false);
                else
                    conexao = Conexao.CriarEntidade();

                var qtdDocumentos = conexao.ExecuteStoreQuery<int>(SQL);
 
                long quantidade = qtdDocumentos.FirstOrDefault();
                if (quantidade > 0)
                {
                    SQL = "SELECT documentoOrigem,documentoDestino,crediario,caixa,venda,cartao,cheque,produtos,vendanf,contnfsaida,datasincronizacao FROM controlesincronizacao WHERE documentoDestino = 0";
                    listDocumentosSincrozizar = conexao.ExecuteStoreQuery<StandAloneControneSincronizacao>(SQL);
                    return listDocumentosSincrozizar.Count();
                }
                return 0;
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.ToString());
                return 0;
            }
                
        }

        public static bool salvarHistorico(string tabela)
        {
            try
            {
                siceEntities entidade;
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);
                else
                    entidade = Conexao.CriarEntidade();

                //siceEntities entidade = Conexao.CriarEntidade(false);
                string SQL = "";
                if (tabela == "caixa")
                {
                    SQL = "SELECT IFNULL(MAX(id),1) FROM caixa WHERE enderecoip = '" + GlbVariaveis.glb_IP + "'";
                    var chaveOrigem = entidade.ExecuteStoreQuery<string>(SQL);
                    SQL = "INSERT INTO historico_sincronizacao (chave_origem,chave_destino,tabela,codigoFilial,data_sincronizacao) VALUES ('"+chaveOrigem.FirstOrDefault()+"', '0','"+tabela+"','"+GlbVariaveis.glb_filial+"','1990-01-01')";
                }

                if (tabela == "movimento")
                {
                    SQL = "SELECT IFNULL(MAX(inc),0) FROM movimento WHERE id = '" + GlbVariaveis.glb_IP +"'";
                    var chaveOrigem = entidade.ExecuteStoreQuery<string>(SQL);
                    SQL = "INSERT INTO historico_sincronizacao (chave_origem,chave_destino,tabela,codigoFilial,data_sincronizacao) VALUES ('" + chaveOrigem.FirstOrDefault() + "', '0','" + tabela + "','" + GlbVariaveis.glb_filial + "','1990-01-01')";
                }

                if (tabela == "sangria")
                {
                    SQL = "SELECT IFNULL(MAX(id_inc),0) FROM movdespesas WHERE id = '" + GlbVariaveis.glb_IP + "' AND sangria = 'S'";
                    var chaveOrigem = entidade.ExecuteStoreQuery<string>(SQL);
                    SQL = "INSERT INTO historico_sincronizacao (chave_origem,chave_destino,tabela,codigoFilial,data_sincronizacao) VALUES ('" + chaveOrigem.FirstOrDefault() + "', '0','" + tabela + "','" + GlbVariaveis.glb_filial + "','1990-01-01')";
                }
                entidade.ExecuteStoreCommand(SQL);
                entidade.SaveChanges();
                return true;
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.ToString());
                return false;
            }
        }

        public static bool lancarDocumentoSincronia(int documento)
        {
            string SQL = ("INSERT INTO controlesincronizacao(documentoOrigem,documentoDestino,codigoFilial,dataOperacao) VALUES ('" + documento + "',0,'" + GlbVariaveis.glb_filial + "',current_date)");

            if (Conexao.tipoConexao == 2)
            {
                using (siceEntities conexaoStand = new siceEntities(Conexao.ObterStringConexao(false)))
                {
                    conexaoStand.ExecuteStoreCommand(SQL);
                    conexaoStand.SaveChanges();
                }
            }
            else
            {
                using (siceEntities conexaoStand = new siceEntities(Conexao.ObterStringConexao(true)))
                {
                    conexaoStand.ExecuteStoreCommand(SQL);
                    conexaoStand.SaveChanges();
                    
                }
            }
                //siceEntities entidade;
                /*if (Conexao.tipoConexao == 2)
                    Conexao.CriarEntidade(false).ExecuteStoreCommand(SQL);
                else
                    Conexao.CriarEntidade().ExecuteStoreCommand(SQL);*/

            

            //entidade.ExecuteStoreCommand(SQL);
            return true;
        }

    }
}

public class StandAloneUsuario
{
    public int codigo;
    public string operador;
    public string senha;        

    public bool descontoFinalizacao;
    public bool arredondar;
    public bool excluirDocumento;
    public bool operadorCaixa;
    public bool venderEstoqueNegativo;
    public bool vendexcluiritem;
    public bool vendaatacado;
}

public class StandAloneProdutos
{
    public string codigoFilial;
    public string codigo;
    public string codigoBarras;
    public string descricao;
    public string situacao;
    public string unidade;
    public int embalagem;
    public string unidadeembalagem;
    public decimal quantidade;
    public decimal quantidadeDisponivel;
    public decimal qtdPrateleiras;
    public decimal preco;
    public decimal precoatacado;
    public decimal icms;
    public string tribuacao;
    public string indicadorTrucancamento;
    public string indicadorProducao;
    public decimal descontoPromocao;
    public string tipo;
    public string ativacompdesc;
    public string grade;
    public decimal descontoMaximo;
    public decimal pis { get; set; }
    public decimal cofins { get; set; }
    public decimal aliquotaIPI { get; set; }

}

public class StandAloneConfigfinanc
{
    public string codigofilial;
    public decimal descontoMaxVenda ;
    public decimal valorMaxVenda;
    public decimal arredondamento;
    public int diasPrimeiroVenc;
    public bool abaterCRcompraCH;
    public bool reservarEstoquePreVenda;
    public bool mudarPrecoVenda ;
    public decimal taxaJurosDiario;
    public string posicaoCodBarrasBalanca;
    public bool vendaPorClasse;
    public string digitoVerificador;
    public bool totalnoFinalCodBalanca;
}

public class StandAloneDadosEmpresa
{
    public string codigofilial;
    public string razaoSocial;
    public string cnpj;
    public string inscricao;
    public string inscricaoMunicipal;  
}

public class StandAloneVenda
{
    public Guid id;
    public long documento;
    public string ip;
    public string operador;
    public int nrcontrole;
    public string codigo;
    public string descricao;
    public string unidade;
    public decimal quantidade;
    public decimal preco;
    public decimal precoOriginal;
    public decimal custo;
    public decimal acrescimo;
    public decimal total;
    public decimal descontovalor;
    public decimal icms;
    public string tributacao;
    public decimal descontoperc;
    public string vendedor;
    public int embalagem;
    public string tipo;
    public decimal ratdesc;
    public DateTime data;
    public string cancelado;
    public decimal pis { get; set; }
    public decimal cofins { get; set; }
    public decimal aliquotaIPI { get; set; }
    public string itemDAV { get; set; }
    public string canceladoECF { get; set; }
}

public class StandAloneCaixa
{
    public Guid id;
    public string codigofilial { get; set; }    
    public long documento;
    public string ip;
    public string operador;
    public string vendedor { get; set; }
    public DateTime data;
    public TimeSpan horaabertura { get; set; }
    public decimal valor;
    public string tipoPagamento;
    public string dpFinanceiro;
    public string historico { get; set; }
    public bool encerrado = false;
}

public class StandAloneCaixasSoma
{
    public DateTime data;
    public TimeSpan horaabertura;
    public TimeSpan horafechamento;
    public string operador;
    public decimal saldoInicial;
    public decimal dinheiro;
    public decimal totalVenda;
    public decimal totalCusto;
    public decimal descontoVenda;
    public decimal saldoFinal;

    
}

public class StandAloneContdocs
{
    public int documento;
    public string codigofilial;
    public string ip;
    public DateTime data;
    public TimeSpan hora;
    public string operador;
    public string vendedor;    
    public decimal valorBruto;
    public decimal desconto;
    public decimal total;
    public string dpFinanceiro;
    public decimal custoItens;
    public decimal valorServico;
    public decimal descontoServico;
    public string COOCupomFiscal; // Contador Ordem Operacao Cupom Fiscal
    public string numeroECF;
    public string CCFCupomFiscal; // Sequencia cupom Fiscal
    public string consumidorECF;
    public string cpfCnpjConsumidor;
    public string enderecoConsumidor;
    public decimal totalLiquidoECF;
    public string COODebitoCredito; // COO CDC
    public decimal icmsCupomFiscal;
    public decimal troco;
    public string ecfFabricao;
    public string ecfMFAdicional;
    public string ecfTipo;
    public string ecfMarca;
    public string ecfModelo;
}

public class StandAloneControneSincronizacao
{
    public int documentoOrigem { get; set; }
    public int documentoDestino { get; set; }
    public string crediario { get; set; }
    public string caixa { get; set; }
    public string venda { get; set; }
    public string cartao { get; set; }
    public string cheque { get; set; }
    public string produtos { get; set; }
    public string vendanf { get; set; }
    public string contnfSaida { get; set; }
    public DateTime dataSincronizacao { get; set; }
    public DateTime dataOperacao { get; set; }
    public int devolucaoOrigem { get; set; }
    public int devolucaoDestino { get; set; }
}
