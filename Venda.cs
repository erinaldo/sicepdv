using System;
using System.Linq;
using System.Data.EntityClient;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using System.Data.Linq.SqlClient;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace SICEpdv
{
    class Venda : FuncoesECF
    {
        //ivan
        /// <summary>
        /// Varíaveis da Venda
        /// </summary>                
        public string dpFinanceiro { get; set; } // Venda, Servico,
        public decimal valorBruto { get; set; }
        public decimal desconto { get; set; }
        public decimal encargos { get; set; }
        public decimal valorLiquido { get; set; }
        public bool vendaFinalizada { get; set; }
        public decimal troco { get; private set; }
        public int numeroDevolucao { get; set; }
        public decimal totalDevolucao { get; set; }
        public static string vendedor { get; set; }
        public int idCliente { get; set; }
        public static string dependente { get; set; }
        public int idCartao { get; set; }
        //IQCARD
        public static string IQCard { get; set; }
        public static string idTransacaoIQCARD { get; set; }
        public static string idPedidoIQCARD { get; set; }

        public bool sincronizada { get; set; }
        public static List<auditoriaVenda> listAuditoriaVenda = new List<auditoriaVenda>();
        
        public static DadosConsumidorCupom dadosConsumidor { get; set; }

        public static DadosEntrega dadosEntrega { get; set; }
        public static DadosDAVOS dadosDAVOS { get; set; }
        public static DadosCheque dadosCheque { get; set; }        
        public static int ultimoDocumento { get; set; }
        public string classeVenda { get; set; }
        public int parcelamentoMaximo { get; set; }
        public static int numeroPED { get; set; }
        public static string RetornoTEF { get; set; }
        private int davAuditoria = 0;
        public decimal restante { get; set; }

        // VAriavel criad em FuncoesECF public int numeroPrevenda { get; set; }
        // Variavel Criada em Funcoes ECF public int numeroDAV { get; set; }        
        // Construtores da Classe;

        public Venda()
        {
            
            //dadosConsumidor =
            //    new DadosConsumidorCupom {
            //        cpfCnpjConsumidor = "", nomeConsumidor = "", endConsumidor = "", endNumero = "", endBairro = "", endCEP = "", endCidade = "", endEstado = "" 
            //    };

            // A variável vendaFinalizada passa a ser verdadeira 
            // quando os valores são zerados na forma de pagamento e
            // não tenha ocorrido nenhum erro.
            vendaFinalizada = false;
          
            dependente = "";
          
          
            valorLiquido = Math.Round(valorLiquido, 2);
            numeroPreVenda = 0;
            numeroDAV = 0;
            classeVenda = "0000";
            encargos = 0;
            parcelamentoMaximo = 0;

        }

        #region InserirItem
        // Função que insere o item passando pelo ECF
        public bool InserirItem(bool imprimirECF, string codigo, string descricao, string complementoDescricao, decimal qtdDisponivel,decimal qtdPrateleira, decimal quantidade, decimal preco,
                    decimal precooriginal, decimal custo, string unidade, int embalagem, decimal descontoPerc, decimal descontoValor, string vendedor,
                     int icms, string tributacao, decimal redBaseCalcICMS, string tipo, string lote, string grade,bool aceitaDesconto,string cfop="5.102",decimal pis=0,decimal cofins=0, 
                     decimal aliquotaIPI = 0, string ncm = "",string tipoRegistro=null,bool verificarPendente=false, string cest = "", produtoLote dadoslotes = null, string situacao = "")
        {


            //if(Configuracoes

            string classe = "0000";
            decimal acrescimoValor = 0;
            decimal descontoValorCalculo = 0;

            if (preco > precooriginal)
                acrescimoValor = decimal.Round((preco - precooriginal) * quantidade, 2);

            if (preco < precooriginal)
                descontoValor = decimal.Round((precooriginal - preco) * quantidade, 2);


            if ((acrescimoValor / quantidade) >= precooriginal)
            {
                throw new Exception("Acréscimo em produto deve ser menor que o preço de venda.");
            }

            if (!aceitaDesconto && (descontoValor > 0 || descontoPerc>0) && situacao != "Promoção")
            {
                throw new Exception("Configuração do item não aceita desconto");
            }
            if (SequenciaItem() == 1 && imprimirECF && !CupomFiscalAberto())
            {
                ///Sumary
                ///Verifica a Conexão antes de inserir o primeiro Item
                ///Se não tiver conectado com o Servidor então usa local
                ///      
                FrmMsgOperador msg = new FrmMsgOperador("", "Abrindo cupom");
                msg.Show();
                Application.DoEvents();

                if (Conexao.onLine)
                {
                    Conexao.stringConexao = null; // Força a procurar a nova stringconexao
                    Conexao.VerificaConexaoDB();
                }

                try
                {
                    AbrirCupom(vendedor, Venda.dadosConsumidor.nomeConsumidor.Trim() + "Dp:" + dependente, Venda.dadosConsumidor.cpfCnpjConsumidor, Venda.dadosConsumidor.endConsumidor, verificarPendente);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Index"))
                    {
                        throw new Exception("ECF Desligado, sem comunicação ou sem papel.");
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    msg.Dispose();
                }

            }

            if (quantidade == 0 || preco == 0)
                return false;


            if ((quantidade * preco) > Configuracoes.valorMaxVenda)
            {
                throw new Exception("Valor máximo por Item é: " + Convert.ToString(Configuracoes.valorMaxVenda));
            }


            // Aqui para ficar certo entre o ECF e o sistemas
            quantidade = Math.Round(quantidade, 3);
            //Truncando para ficar igual tanto no ECF como no Sistema


            if (descontoPerc > 0)
            {
                descontoValorCalculo = (quantidade * precooriginal) * descontoPerc / 100;
                descontoValorCalculo = /*Math.Truncate*/(descontoValorCalculo * 100) / 100;
                // Aqui ajusta o preco para que nao haja diferenca entre o ECF e o Sistemas
                preco = (precooriginal - (descontoValorCalculo / quantidade));
                preco = Math.Truncate(preco * 100) / 100;

                /*if(((precooriginal - preco) * quantidade) != descontoValor)
                {
                    descontoValor = ((precooriginal - preco) * quantidade);
                }*/

            }

            if (Math.Round(preco,2) < precooriginal && descontoPerc==0 && situacao != "Promoção")
            {
                decimal descontoMaxPermitido = Math.Round(precooriginal * (Configuracoes.descontoMaxVenda/100),2);

                if (precooriginal - preco > descontoMaxPermitido)
                {
                    throw new Exception("Preço de venda abaixo da configuração permitida ! " );
                }
            };
            
            var calculoDesconto = decimal.Round((descontoValor / (precooriginal * quantidade) * 100), 2);

            if (((descontoPerc > Configuracoes.descontoMaxVenda) || (calculoDesconto > Configuracoes.descontoMaxVenda)) && situacao != "Promoção")
            {
                Produtos prd = new Produtos();
                prd.ProcurarCodigo(codigo, GlbVariaveis.glb_filial);                
                    if (descontoPerc > prd.descontoMaximo && situacao != "Promoção")
                    {
                        throw new Exception("Desconto máximo no item : " + string.Format("{0:N2}", Configuracoes.descontoMaxVenda) + "%");
                    }                
            };

            if (situacao == "Promoção" && descontoValor > 0)
            {
                decimal descontoCalculado = ((((preco / precooriginal) -1) * 100) * -1);
                descontoCalculado = Math.Truncate(descontoCalculado * 100) / 100;
                decimal dif = (descontoCalculado - descontoPerc);
                if (dif < 0)
                    dif = 0;

                if (descontoCalculado > descontoPerc && dif > decimal.Parse("0.05"))
                    throw new Exception("Desconto maior do que o permitido para o item em promoção : " + string.Format("{0:N2}", descontoPerc) + "%");
                
            }

            if (Produtos.tabelaPreco=="atacado" && calculoDesconto>0 && !Configuracoes.descontoAtacado)
            {
                throw new Exception("Preço no atacado não aceita desconto");
            }


            if (Configuracoes.vendaPorclasse)
            {
                Produtos prd = new Produtos();
                prd.ProcurarCodigo(codigo, GlbVariaveis.glb_filial);
                classe = prd.classe;
            };

            //decimal total = quantidade * preco;

            //var totalSemDesconto = precooriginal+acrescimoValor*quantidade;
            //totalSemDesconto = Math.Truncate(totalSemDesconto * 100) / 100;

            //var descontoSobreTotal = totalSemDesconto * descontoPerc / 100;
            //descontoSobreTotal = Math.Truncate(descontoSobreTotal * 100) / 100;            

            // Aqui para fazer o truncamento usado pelos ECFs - truncar
            // Exemplo: (5.78*2.52)+0.03 = 14.5956 Sem o truncamento seria 14.60 com o truncamento fica 14.59 
            decimal total = (precooriginal * quantidade) + acrescimoValor - descontoValor;
            total = (Math.Truncate(total * 100) / 100);
            if((descontoValor + acrescimoValor) > 0) 
                total = (preco * quantidade);
            

            /*
            if (!Permissoes.venderQtdNegativa && qtdDisponivel - quantidade < 0)
            {
                #region
                FrmLogon logon = new FrmLogon();
                logon.campo = "estnegativo";
                logon.txtDescricao.Text = "Produto negativo: " + codigo + " " + descricao + " Qtd: " + qtdDisponivel.ToString();
                logon.ShowDialog();
                if (!Operador.autorizado)
                    throw new Exception("Sem permissão para vender com estoque negativo !");
                else
                {
                    siceEntities entidade = Conexao.CriarEntidade();

                    if (Conexao.tipoConexao == 2)
                        entidade = Conexao.CriarEntidade(false);

                    auditoria objAuditoria = new auditoria();
                    objAuditoria.acao = "Venda";
                    objAuditoria.CodigoFilial = GlbVariaveis.glb_filial;
                    objAuditoria.codigoproduto = codigo;
                    objAuditoria.data = DateTime.Now.Date;
                    objAuditoria.documento = 0;
                    objAuditoria.hora = DateTime.Now.TimeOfDay;
                    objAuditoria.local = "SICE.pdv";
                    objAuditoria.observacao = "Produto negativo: " + codigo + " " + descricao + " Qtd: " + qtdDisponivel.ToString();
                    objAuditoria.usuario = Operador.ultimoOperadorAutorizado;
                    entidade.AddToauditoria(objAuditoria);
                    entidade.SaveChanges();

                    try
                    {
                        var lisCodigo = (from a in entidade.auditoria
                                         where a.acao == "Venda" && a.usuario == Operador.ultimoOperadorAutorizado && a.local == "SICE.pdv" && a.data == objAuditoria.data
                                         select a.id).ToList().Max();

                        auditoriaVenda n = new auditoriaVenda();
                        n.inc = int.Parse(lisCodigo.ToString());
                        Venda.listAuditoriaVenda.Add(n);
                    }
                    catch (Exception erro)
                    {

                    }
                }
                #endregion
            }
            */

            var quantidadeVendida = this.verificarQuantidadeDigitada(codigo);


            // Aqui gerar a transferência 

            bool aplicarRestrincoesQuantidade = true;

            if (Configuracoes.gerarTransferenciaVenda && (qtdDisponivel - (quantidade + quantidadeVendida)) < 0 /*&& this.verificarVendaIternet(codigo,GlbVariaveis.glb_filial) == true*/)
            {
                FrmPosicaoEstoqueFiliais.gerarTransfVenda = true;
                FrmPosicaoEstoqueFiliais.quantidadeTransf = (quantidade);
                FrmPosicaoEstoqueFiliais.codigo = codigo;
                FrmPosicaoEstoqueFiliais transf = new FrmPosicaoEstoqueFiliais();
                transf.ShowDialog();
                if (!string.IsNullOrEmpty(FrmPosicaoEstoqueFiliais.filialEscolhida))
                {
                    aplicarRestrincoesQuantidade = false;
                }
            }

            if (aplicarRestrincoesQuantidade)
            {

                if (!Permissoes.venderQtdNegativa && (qtdDisponivel - (quantidade + quantidadeVendida)) < 0 && Configuracoes.usarQtdPrateleira == false)
                {
                    #region
                    FrmLogon logon = new FrmLogon();
                    logon.campo = "estnegativo";
                    logon.txtDescricao.Text = "Quantidade Digitada é maior que a disponível no estoque " + codigo + " " + descricao + " Qtd Estoque: " + qtdDisponivel.ToString();
                    logon.ShowDialog();
                    if (!Operador.autorizado)
                        throw new Exception("Sem permissão para vender com estoque negativo !");
                    else
                    {
                        siceEntities entidade = Conexao.CriarEntidade();
                        if (Conexao.tipoConexao == 2)
                            entidade = Conexao.CriarEntidade(false);

                        auditoria objAuditoria = new auditoria();
                        objAuditoria.acao = "Liberação de estoque negativo";
                        objAuditoria.CodigoFilial = GlbVariaveis.glb_filial;
                        objAuditoria.codigoproduto = codigo;
                        objAuditoria.data = DateTime.Now.Date;
                        objAuditoria.documento = 0;
                        objAuditoria.hora = DateTime.Now.TimeOfDay;
                        objAuditoria.local = "SICE.pdv";
                        objAuditoria.observacao = "Produto negativo: " + codigo + " " + descricao + " Qtd: " + qtdDisponivel.ToString();
                        objAuditoria.usuariosolicitante = Operador.ultimoOperadorAutorizado;
                        objAuditoria.usuario = GlbVariaveis.glb_Usuario;
                        objAuditoria.tabela = "Venda";
                        entidade.AddToauditoria(objAuditoria);
                        entidade.SaveChanges();

                        try
                        {
                            var lisCodigo = (from a in entidade.auditoria
                                             where a.tabela == "Venda" && a.usuariosolicitante == Operador.ultimoOperadorAutorizado && a.local == "SICE.pdv" && a.data == objAuditoria.data
                                             select a.id).ToList().Max();

                            auditoriaVenda n = new auditoriaVenda();
                            n.inc = int.Parse(lisCodigo.ToString());
                            Venda.listAuditoriaVenda.Add(n);
                        }
                        catch (Exception erro)
                        {

                        }
                    }
                    #endregion
                }

                if (!Permissoes.venderQtdNegativa && (qtdPrateleira - (quantidade + quantidadeVendida)) < 0 && Configuracoes.usarQtdPrateleira == true)
                {
                    #region
                    FrmLogon logon = new FrmLogon();
                    logon.campo = "estnegativo";
                    logon.txtDescricao.Text = "Quantidade Digitada é maior que a Qtd Prateleira na loja " + codigo + " " + descricao + " Qtd Prateleira: " + qtdDisponivel.ToString();
                    logon.ShowDialog();
                    if (!Operador.autorizado)
                        throw new Exception("Sem permissão para vender com estoque negativo !");
                    else
                    {
                        siceEntities entidade = Conexao.CriarEntidade();
                        if (Conexao.tipoConexao == 2)
                            entidade = Conexao.CriarEntidade(false);

                        auditoria objAuditoria = new auditoria();
                        objAuditoria.acao = "Liberação de estoque negativo";
                        objAuditoria.CodigoFilial = GlbVariaveis.glb_filial;
                        objAuditoria.codigoproduto = codigo;
                        objAuditoria.data = DateTime.Now.Date;
                        objAuditoria.documento = 0;
                        objAuditoria.hora = DateTime.Now.TimeOfDay;
                        objAuditoria.local = "SICE.pdv";
                        //objAuditoria.observacao = "Produto negativo: " + codigo + " " + descricao + " Qtd: " + qtdDisponivel.ToString();
                        objAuditoria.observacao = "Quantidade Digitada é maior que a Qtd Prateleira na loja " + codigo + " " + descricao + " Qtd Prateleira: " + qtdDisponivel.ToString();
                        objAuditoria.usuariosolicitante = Operador.ultimoOperadorAutorizado;
                        objAuditoria.usuario = GlbVariaveis.glb_Usuario;
                        objAuditoria.tabela = "Venda";
                        entidade.AddToauditoria(objAuditoria);
                        entidade.SaveChanges();

                        try
                        {
                            var lisCodigo = (from a in entidade.auditoria
                                             where a.tabela == "Venda" && a.usuariosolicitante == Operador.ultimoOperadorAutorizado && a.local == "SICE.pdv" && a.data == objAuditoria.data
                                             select a.id).ToList().Max();

                            auditoriaVenda n = new auditoriaVenda();
                            n.inc = int.Parse(lisCodigo.ToString());
                            Venda.listAuditoriaVenda.Add(n);
                        }
                        catch (Exception erro)
                        {

                        }
                    }
                    #endregion
                }


                if (Configuracoes.usarQtdPrateleira && (qtdPrateleira - quantidade) < 0)
                    throw new Exception("Quantidade está restrita as mercadorias em exposição (prateleiras)! Alimente as prateleiras !");


                if (quantidade != Math.Round(quantidade) && (unidade == "UNI" || unidade == "UN" || unidade == "UND"))
                    throw new Exception("Não é permitido fracionar produto com cadastro definido como unidade");

                if (!Permissoes.lancarItemduplos && Conexao.onLine)
                {
                    siceEntities entidade = Conexao.CriarEntidade();
                    if (Conexao.tipoConexao == 2)
                        entidade = Conexao.CriarEntidade(false);

                    var procuraItem = from n in entidade.vendas
                                      where n.id == GlbVariaveis.glb_IP && n.codigo == codigo && n.cancelado == "N"
                                      select n;
                    if (procuraItem.Count() > 0)
                        throw new Exception("Item já incluído e configuração do operador não permite lançar 2 itens com o mesmo código");
                }
            }

            if (imprimirECF)
                {

                if (!VenderItemECF(codigo, descricao + " " + complementoDescricao.Trim(), precooriginal, descontoPerc, descontoValor, 0, acrescimoValor, quantidade, unidade, tributacao, icms, tipo,ncm,cest))
                    return false;
                // Aqui pega o mesmo total do ECF para que não haja divergência !
                // A Bematech não tem essa função
                /*if (ConfiguracoesECF.idECF == 2)
                    total = FuncoesECF.TotalItemECF();*/

                };

            try
            {
                //if (Conexao.onLine)
                //{
                    #region Gravando Item na tabela online

                    siceEntities entidade = Conexao.CriarEntidade();

                    if (Conexao.tipoConexao == 2)
                        entidade = Conexao.CriarEntidade(false);

                    vendas item = new vendas();
                    item.aentregar = Configuracoes.romaneiroVenda == true ? "S" : "N" ;
                    item.romaneio = Configuracoes.romaneiroVenda == true ? "S" : "N";
                    item.codigofilial = GlbVariaveis.glb_filial;
                    item.classe = classe;
                    item.codigobarras = "12";
                    item.codigofiscal = "000";
                    item.comissao = "A";
                    item.grade = "nenhuma";
                    item.id = GlbVariaveis.glb_IP;
                    item.tipo = tipo == null ? "0 - Produtos" : tipo;
                    item.embalagem = embalagem;
                    item.nrcontrole = SequenciaItem();
                    item.codigo = codigo;
                    //item.produto = (descricao.Trim() + " " + complementoDescricao.Trim()).Trim().PadRight(49, ' ').Substring(0, 49);
                    item.produto = (descricao.Trim()).Trim().PadRight(49, ' ').Substring(0, 49);
                    item.quantidade = quantidade;
                    item.preco = preco;
                    item.custo = custo;
                    item.precooriginal = precooriginal;
                    item.acrescimototalitem = acrescimoValor;
                    item.unidade = unidade;
                    item.Descontoperc = descontoPerc;
                    item.descontovalor = descontoValor;
                    item.vendedor = vendedor;
                    item.icms = Convert.ToInt16(icms);
                    item.tributacao = tributacao;
                    item.total = total; // Math.Round( quantidade * precooriginal);
                    item.cfop = cfop;
                    item.cstcofins = "01";
                    item.cstpis = "01";
                    item.serieNF = "1";
                    item.subserienf = "1";
                    item.modelodocfiscal = "2D";                    
                    item.cancelado = "N";
                    item.data = GlbVariaveis.Sys_Data.Date;
                    item.operador = GlbVariaveis.glb_Usuario;
                    item.grade = grade;
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
                    item.ncm = ncm;
                    item.ncmespecie = ncm.Length > 2 ? ncm.Substring(0, 2) : "";
                    item.pis = pis;
                    item.cofins = cofins;
                    item.aliquotaIPI = aliquotaIPI;
                    item.tipoalteracao = tipoRegistro;
                    item.itemDAV = "S";
                    item.canceladoECF = "N";
                    item.vendaatacado = Produtos.tabelaPreco == "atacado" ? "S" : "N";
                    item.cenqipi = "";
                    item.situacao = situacao;

                    if (dadoslotes != null)
                    {
                        item.lote = dadoslotes.lote;
                        item.vencimento = dadoslotes.vencimento;
                        item.datafabricacao = dadoslotes.fabricacao;
                        item.idfornecedor = dadoslotes.codigoFornecedor;
                    }
                    else
                    {
                        item.lote = "";
                        item.vencimento = null;
                        item.datafabricacao = null;
                        item.idfornecedor = 0;
                    }
                    if(item.tributacao == "20") 
                        item.percentualRedBaseCalcICMS = redBaseCalcICMS;

                    entidade.AddTovendas(item);
                    entidade.SaveChanges();

                    // Aqui passando a descricao complementar. Feito por Ivan 
                    // Está por sql por que nao quis dar um update no EDX na máquina
                    // de Marckvaldo
                    if (!string.IsNullOrEmpty(complementoDescricao))
                    {
                        try
                        {
                            string sql = "START TRANSACTION;"+
                                         "UPDATE vendas SET infadprod='" + complementoDescricao + "' WHERE nrcontrole='" + item.nrcontrole + "' AND id='" + item.id + "';"+
                                         "COMMIT;";
                            Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                        }
                        catch (Exception)
                        {
                            
                        }
                      
                    }


                    try
                    {
                        string sql = "START TRANSACTION;" +
                                        "UPDATE vendas as v SET custogerencial = IFNULL(IF(v.codigoFilial = '00001',(SELECT custogerencial FROM produtos WHERE codigo = v.codigo AND codigoFilial = v.codigofilial limit 1),(SELECT custogerencial FROM produtosfilial WHERE codigo = v.codigo AND codigoFilial = v.codigofilial limit 1)),0) WHERE codigo='" + item.codigo + "' AND id='" + item.id + "' AND codigoFilial = '"+item.codigofilial+"';" +
                                        "COMMIT;";
                        Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                    }
                    catch (Exception)
                    {

                    }

                    
                    #endregion


                // Aqui inserindo na tabela para gerar transferência automática
                if (!string.IsNullOrEmpty(FrmPosicaoEstoqueFiliais.filialEscolhida) && Configuracoes.gerarTransferenciaVenda && FrmPosicaoEstoqueFiliais.gerarTransfVenda == true)
                    {
                        try
                        {
                            using (siceEntities conn = Conexao.CriarEntidade())
                            {
                                try
                                {
                                    string sql = "START TRANSACTION;" +
                                                 "INSERT INTO transfvendatemp(ip,data,operador,codigo,descricao,quantidade,preco,custo,filialdestino,filialorigem,numeroDAV,cancelado,numeroItem) " +
                                                 "VALUES ('" + GlbVariaveis.glb_IP + "'," + "CURRENT_DATE" + ",'" + GlbVariaveis.glb_Usuario + "','" + codigo + "','" + descricao + "',truncate(" + quantidade.ToString("N3").Replace(".", "").Replace(",", ".") + ",3),truncate('" + preco.ToString("N2").Replace(".", "").Replace(",", ".") + "',2),truncate('" + custo.ToString("N2").Replace(".", "").Replace(",", ".") + "',2),'" + GlbVariaveis.glb_filial + "','" + FrmPosicaoEstoqueFiliais.filialEscolhida + "', '0','N','"+ item.nrcontrole + "');"+
                                                 "COMMIT;";

                                    conn.ExecuteStoreCommand(sql);
                                }
                                catch(Exception erro )
                                {
                                    throw new Exception(erro.ToString());
                                }                                
                            }

                        }
                        catch (Exception erro)
                        {
                            throw new Exception(erro.ToString());
                        }


                        try
                        {
                            using (siceEntities conn = Conexao.CriarEntidade())
                            {
                                string sql = "SELECT IFNULL(COUNT(1),0) as quantidade FROM transfvendatemp WHERE ip = '" + GlbVariaveis.glb_IP + "' AND codigo = '" + codigo + "' AND filialdestino = '" + GlbVariaveis.glb_filial + "' AND filialorigem = '" + FrmPosicaoEstoqueFiliais.filialEscolhida + "' AND numeroitem = '" + item.nrcontrole + "' AND data = CURRENT_DATE";
                                var qtdTransferencia = conn.ExecuteStoreQuery<int>(sql).FirstOrDefault();

                                if (qtdTransferencia == 0)
                                {

                                    sql = "START TRANSACTION;" +
                                    "DELETE FROM vendas WHERE  id = '" + item.id + "' AND codigoFilial = '" + item.codigofilial + "' AND codigo = '" + item.codigo + "' AND nrcontrole = '" + item.nrcontrole + "' AND cancelado = 'N' AND inc = '" + item.inc + "';" +
                                    "COMMIT;";

                                    conn.ExecuteStoreCommand(sql);

                                    throw new Exception("Não foi possivel criar a transferencia desse produto! Refaça a operação novamente por favor.");
                                }
                            }

                        }
                        catch (Exception erro)
                        {
                            throw new Exception(erro.ToString());
                        }
                    }

                   

                    FrmPosicaoEstoqueFiliais.filialEscolhida = "";

                //}

                

                #region Gravando Item na tabela Local
                /*try
                {
                    if (Conexao.tipoConexao == 1)
                    {
                        List<StandAloneVenda> dadosItem = new List<StandAloneVenda>();
                        StandAloneVenda itemAtual = new StandAloneVenda();
                        itemAtual.id = Guid.NewGuid();
                        itemAtual.ip = GlbVariaveis.glb_IP;
                        itemAtual.nrcontrole = SequenciaItem();
                        itemAtual.codigo = codigo;
                        itemAtual.descricao = descricao;
                        itemAtual.quantidade = quantidade;
                        itemAtual.preco = preco;
                        itemAtual.custo = custo;
                        itemAtual.precoOriginal = precooriginal;
                        itemAtual.acrescimo = acrescimoValor;
                        itemAtual.total = total;
                        itemAtual.unidade = unidade;
                        itemAtual.icms = icms;
                        itemAtual.tributacao = tributacao;
                        itemAtual.descontoperc = descontoPerc;
                        itemAtual.descontovalor = descontoValor;
                        itemAtual.vendedor = vendedor;
                        itemAtual.embalagem = embalagem;
                        itemAtual.documento = 0;
                        itemAtual.operador = GlbVariaveis.glb_Usuario;
                        itemAtual.tipo = tipo == null ? "0 - Produtos" : tipo;
                        itemAtual.ratdesc = 0;
                        itemAtual.data = GlbVariaveis.Sys_Data;
                        itemAtual.cancelado = "N";
                        itemAtual.pis = pis;
                        itemAtual.cofins = cofins;
                        itemAtual.aliquotaIPI = aliquotaIPI;
                        itemAtual.canceladoECF = "N";
                        itemAtual.itemDAV = "N";
                        dadosItem.Add(itemAtual);
                        IObjectContainer tabelaVenda = Db4oFactory.OpenFile("vendas.yap");
                        tabelaVenda.Store(dadosItem);
                        tabelaVenda.Close();
                    }
                }
                catch
                {
                    //MORTO: Catch morto se houver erro no try nao retornar nenhum erro 
                    // e nem causará exceção catch dessa forma deverá ser usado com atenção.
                };

                if (!Conexao.onLine)
                    return true;

                */
                #endregion


                // Fazendo reserva de Estoque da Pre-Venda
                ReservarEstoquePreVenda(codigo, quantidade);
            }
            catch (Exception erro)
            {
                if (imprimirECF)
                {
                    FuncoesECF fecf = new FuncoesECF();
                    int seqItem = SequenciaItem()-1;

                    if (seqItem == 0)
                        seqItem = 1;


                    fecf.ApagarItemECF(seqItem);
                }
                throw new Exception("Inserindo registro: " + erro.Message);
            }
            return true;
        }

        public static void ReservarEstoquePreVenda(string codigo, decimal quantidade)
        {
            if (!ConfiguracoesECF.pdv && Configuracoes.reservarEstoquePreVenda)
            {
                using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "siceEntities.ReservarPreVenda";
                    cmd.CommandType = CommandType.StoredProcedure;

                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;

                    EntityParameter codigoProduto = cmd.Parameters.Add("codigoProduto", DbType.String);
                    codigoProduto.Direction = ParameterDirection.Input;
                    codigoProduto.Value = codigo;

                    EntityParameter qtdPreVenda = cmd.Parameters.Add("quantidade", DbType.Decimal);
                    qtdPreVenda.Direction = ParameterDirection.Input;
                    qtdPreVenda.Value = quantidade;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        #endregion InserirItem

        #region Seleciona Itens da Venda
        public IQueryable<vendas> SelectionaItensVenda()
        {

            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            var v = from p in entidade.vendas
                    where p.id == GlbVariaveis.glb_IP
                    && p.cancelado == "N"
                    orderby p.nrcontrole
                    select p;
            return v;


        }
        #endregion Seleciona Itens da Venda

        #region SequenciaItem da Venda
        public int SequenciaItem()
        {
            int sequencia = 1;

            /// If operandus modus = StandAlone
            #region StandAlone
            if (!Conexao.onLine && Conexao.tipoConexao == 1)
            {
                IObjectContainer tabela = Db4oFactory.OpenFile("vendas.yap");
                int? ordemItem = (from StandAloneVenda p in tabela
                          where p.ip == GlbVariaveis.glb_IP
                          select (int?)p.nrcontrole).Max();
                tabela.Close();

                return ordemItem.GetValueOrDefault() + 1;

            };
            #endregion

            siceEntities entidade = Conexao.CriarEntidade();

            if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

            //using (siceEntities entidade = Conexao.CriarEntidade())
            using(entidade)
            {
                var v = from p in entidade.vendas
                        where p.id == GlbVariaveis.glb_IP
                        group p by p.id into g
                        select new { sequencia = g.Max(p => p.nrcontrole) };
                foreach (var item in v)
                {
                    sequencia = item.sequencia + 1;
                }
                return sequencia;
            };
        }
        #endregion

        #region Soma Todos os itens da venda
        public decimal SomaItens()
        {

            /// If operandus modus = StandAlone
            #region StandAlone
            if (!Conexao.onLine && Conexao.tipoConexao == 1)
            {
                IObjectContainer tabela = Db4oFactory.OpenFile("vendas.yap");

                decimal? totalVendaOff = (from StandAloneVenda p in tabela
                        where p.ip == GlbVariaveis.glb_IP
                        && p.cancelado == "N"                        
                        select (decimal?)p.total).Sum();
                tabela.Close();
                
                return totalVendaOff.GetValueOrDefault();
            }
            #endregion

            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            decimal? total = (from n in entidade.vendas
                                   where n.id == GlbVariaveis.glb_IP
                                   && n.cancelado == "N"
                                   select (decimal?)n.total).Sum();
            
            return total.GetValueOrDefault();
        }

        public decimal SomaDescontoItens()
        {

            /// If operandus modus = StandAlone
           
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            decimal? total = (from n in entidade.vendas
                              where n.id == GlbVariaveis.glb_IP
                              && n.cancelado == "N"
                              select (decimal?)(n.ratdesc + n.descontovalor)).Sum();

            return total.GetValueOrDefault();
        }

        public decimal SomaItensSemDescontoItens()
        {

            /// If operandus modus = StandAlone

            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            decimal? total = (from n in entidade.vendas
                              where n.id == GlbVariaveis.glb_IP
                              && n.cancelado == "N"
                              && n.descontovalor == 0
                              select (decimal?)(n.total)).Sum();

            return total.GetValueOrDefault();
        }
        #endregion

        public static string CalculaTributosCupom()
        {
            decimal valorTotalTributosEstadual = 0;
            decimal valorTotalTributosFederal = 0;
            decimal valorTotalMunicipal = 0;
            string chave = "";
            bool servico = false;

            if (Conexao.onLine)
            {

                siceEntities entidade = Conexao.CriarEntidade();
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                var aliquotaTributos = (from p in entidade.vendas
                                        where p.id == GlbVariaveis.glb_IP
                                        && p.cancelado == "N"
                                        select p).ToList();

                decimal? totalLiquido = (from p in entidade.vendas
                                    where p.id == GlbVariaveis.glb_IP
                                    && p.cancelado == "N"
                                    select (decimal?)p.total).Sum();

                decimal percentualTributos;

                foreach (var valoresItens in aliquotaTributos)
                {
                    /*if (valoresItens.ncm == "" && valoresItens.ncm == null)
                    {
                        //percentualTributos = decimal.Parse(valoresItens.icms.ToString()) + decimal.Parse(valoresItens.aliquotaIPI.ToString()) + decimal.Parse(valoresItens.pis.ToString()) + decimal.Parse(valoresItens.cofins.ToString());
                       /* percentualTributos = decimal.Parse((valoresItens.icms == null ? 0 : valoresItens.icms).ToString()) + decimal.Parse((valoresItens.aliquotaIPI == null ? 0 : valoresItens.aliquotaIPI).ToString()) + decimal.Parse((valoresItens.pis == null ? 0 : valoresItens.pis).ToString()) + decimal.Parse((valoresItens.cofins == null ? 0 : valoresItens.cofins).ToString());
                        valorTotalTributos = +(valoresItens.total * percentualTributos) / 100;
                    }*/
                    if (valoresItens.ncm != "" && valoresItens.ncm != null)
                    {
                        if (valoresItens.tipo.Substring(0,1) != "1")
                        {

                            //var ncm = valoresItens.ncm;
                           

                            var tributos = (from t in entidade.tabelaibpt
                                            where t.codigo.Equals(valoresItens.ncm) && t.tabela == 0
                                            select new
                                            {
                                                aliqNacFederal = t.aliqNac,
                                                aliqImpFederal = t.aliqImp,
                                                aliqEstadual = t.estadual,
                                                aliqMunicipal = t.municipal,
                                                chave = t.chave,
                                                versao = t.versao
                                            }).FirstOrDefault();

                           
                            if (tributos != null)
                            {

                                if (valoresItens.origem == "0" || valoresItens.origem == "3" || valoresItens.origem == "4" || valoresItens.origem == "5")
                                {
                                    percentualTributos = decimal.Parse(tributos.aliqNacFederal.ToString());
                                    valorTotalTributosFederal =+ (valoresItens.total * percentualTributos) / 100;
                                    
                                }
                                else
                                {
                                    percentualTributos = decimal.Parse(tributos.aliqImpFederal.ToString());
                                    valorTotalTributosFederal =+ (valoresItens.total * percentualTributos) / 100;
                                }

                                percentualTributos = decimal.Parse(tributos.aliqEstadual.ToString());
                                valorTotalTributosEstadual = (valoresItens.total * percentualTributos) / 100;

                                chave = tributos.chave.ToString();
                            }
                            
                        }
                        else
                        {
                            string nbm = valoresItens.nbm == null ? "" : valoresItens.nbm.ToString();

                            var tributos = (from t in entidade.tabelaibpt
                                            where t.codigo.Equals(valoresItens.nbm) && t.tabela == 1
                                            select new
                                            {
                                                aliqNacFederal = t.aliqNac,
                                                aliqImpFederal = t.aliqImp,
                                                aliqEstadual = t.estadual,
                                                aliqMunicipal = t.municipal,
                                                chave = t.chave,
                                                versao = t.versao
                                            }).FirstOrDefault();

                            if (tributos != null)
                            {

                                if (valoresItens.origem == "0" || valoresItens.origem == "3" || valoresItens.origem == "4" || valoresItens.origem == "5")
                                {
                                    percentualTributos = decimal.Parse(tributos.aliqNacFederal.ToString());
                                    valorTotalTributosFederal =+ (valoresItens.total * percentualTributos) / 100;
                                    
                                }
                                else
                                {
                                    percentualTributos = decimal.Parse(tributos.aliqImpFederal.ToString());
                                    valorTotalTributosFederal =+ (valoresItens.total * percentualTributos) / 100;
                                }

                                percentualTributos = decimal.Parse(tributos.aliqEstadual.ToString());
                                valorTotalTributosEstadual = (valoresItens.total * percentualTributos) / 100;

                                percentualTributos = decimal.Parse(tributos.aliqMunicipal.ToString());
                                valorTotalMunicipal = (valoresItens.total * percentualTributos) / 100;

                                chave = tributos.chave.ToString();

                               servico = true;
                            }
                        }
                    }
                }

                //string.Format("{0:N2}",preco*10)
                if (!totalLiquido.HasValue)
                    return "";
                //Trib aprox R$: 4,45 Fed, 5,40 Est e 2,00 Mun
                if(servico == false)
                    return "Trib aprox R$:   " + string.Format("{0:N2}", valorTotalTributosFederal) + " Federal e   " + valorTotalTributosEstadual.ToString("N2") + " Estadual \n Fonte: IBPT/FECOMERCIO " + Configuracoes.estado + "                " + chave;
                else
                    return "Trib aprox R$:   " + string.Format("{0:N2}", valorTotalTributosFederal) + " Fed,  " + valorTotalTributosEstadual.ToString("N2") + " Est e " + valorTotalMunicipal.ToString("N2") + " Mun \n  R$ " + string.Format("{0:N2}", (totalLiquido - (valorTotalTributosEstadual + valorTotalTributosFederal + valorTotalMunicipal))) + " pelos produtos/serviços \n Fonte: IBPT/FECOMERCIO " + Configuracoes.estado + "                " + chave;

            }

           /* if (!Conexao.onLine)
            {
                IObjectContainer tabela = Db4oFactory.OpenFile("vendas.yap");             

                var aliquotaTributosOff = (from StandAloneVenda p in tabela
                                        where p.ip == GlbVariaveis.glb_IP
                                        && p.cancelado == "N"
                                           select new { p.icms, p.aliquotaIPI, p.pis, p.cofins, p.quantidade, p.total }).ToList();

                var totalLiquidoOff = (from StandAloneVenda p in tabela
                                    where p.ip == GlbVariaveis.glb_IP
                                    && p.cancelado == "N"
                                    select p.total).Sum();

                decimal percentualTributos;

                foreach (var valoresItens in aliquotaTributosOff)
                {
                    percentualTributos = decimal.Parse(valoresItens.icms.ToString()) + decimal.Parse(valoresItens.aliquotaIPI.ToString()) + decimal.Parse(valoresItens.pis.ToString()) + decimal.Parse(valoresItens.cofins.ToString());
                    valorTotalTributos = +(valoresItens.total * percentualTributos) / 100;
                }
                tabela.Close();

                //string.Format("{0:N2}",preco*10)
                return "Val Aprox Tributos R$ " + string.Format("{0:N2}", valorTotalTributos) + " (" + string.Format("{0:N2}", ((valorTotalTributos / totalLiquidoOff) * 100)) + "%) Fonte:IBPT";


            }*/
            return "";
        }

        #region Excluir Item da Venda

        public bool MarcarEntrega(int id)
        {
            siceEntities entidade = Conexao.CriarEntidade();

            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);
            vendas excluir = (from c in entidade.vendas
                              where c.inc == id                              
                              select c).First();

            if (excluir.aentregar == "S")
            {
                excluir.aentregar = "N";
                excluir.romaneio = "N";
            }
            else
            {
                excluir.aentregar = "S";
                excluir.romaneio = "S";
            }


            entidade.SaveChanges();

            return true;


        }

        public static int quantidadeAentregar(string ip)
        {
            siceEntities entidade = Conexao.CriarEntidade();

            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);
            int quantidade = (from c in entidade.vendas
                              where c.id == ip
                              && c.aentregar == "S"
                              && c.romaneio == "S"
                              select c).ToList().Count();

            return quantidade;
        }


        public static void ExclulirItensPDV()
        {
            string SQL = "START TRANSACTION;" +
                         "delete FROM vendas WHERE id = '" + GlbVariaveis.glb_IP + "';" +
                         "delete FROM transfvendatemp WHERE ip = '" + GlbVariaveis.glb_IP + "';" +
                         "COMMIT;";


            Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
        }

        public bool ExcluirItem(int itemNr, int id)
        {


            #region StandAlone
            if (!Conexao.onLine && Conexao.tipoConexao == 1)
            {
                if (!ApagarItemECF(itemNr)) return false;

                IObjectContainer tabela = Db4oFactory.OpenFile("vendas.yap");
                var dados = (from StandAloneVenda n in tabela
                             where n.nrcontrole == itemNr
                             && n.ip == GlbVariaveis.glb_IP
                             select n).FirstOrDefault();
                dados.cancelado="S";
                tabela.Store(dados);   
                tabela.Close();              

                return true;
            }
            #endregion


            if (!Permissoes.excluirItemVenda && ConfiguracoesECF.pdv)
            {
                Funcoes.TravarTeclado(false);
                FrmLogon logon = new FrmLogon();
                logon.campo = "vendexcluiritem";
                logon.txtDescricao.Text = "Excluir item da venda";
                logon.ShowDialog();
                if (!Operador.autorizado)
                    return false;
            }

            if (!ApagarItemECF(itemNr)) return false;


            siceEntities entidade = Conexao.CriarEntidade();

            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            // Desfazendo reserva de Estoque da Pre-Venda
            if (!ConfiguracoesECF.pdv && Configuracoes.reservarEstoquePreVenda)
            {
                string codigo = "";
                decimal quantidade = 0;
                var dados = (from v in entidade.vendas
                             where v.nrcontrole == itemNr
                             && v.id == GlbVariaveis.glb_IP
                             && v.cancelado == "N"
                             select new
                             {
                                 codigo = v.codigo,
                                 quantidade = v.quantidade,
                                 v.cancelado
                             }).FirstOrDefault();
                codigo = dados.codigo;
                quantidade = dados.quantidade;

                if (dados.cancelado=="S")
                    return true;

                using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "siceEntities.ReservarPreVenda";
                    cmd.CommandType = CommandType.StoredProcedure;

                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;

                    EntityParameter codigoProduto = cmd.Parameters.Add("codigoProduto", DbType.String);
                    codigoProduto.Direction = ParameterDirection.Input;
                    codigoProduto.Value = codigo;

                    EntityParameter qtdPreVenda = cmd.Parameters.Add("quantidade", DbType.Decimal);
                    qtdPreVenda.Direction = ParameterDirection.Input;
                    qtdPreVenda.Value = -quantidade;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            vendas excluir = (from c in entidade.vendas
                              where c.inc == id
                              && c.id == GlbVariaveis.glb_IP
                              select c).First();

            if (excluir.cancelado == "S")
                return true;
            // if (ConfiguracoesECF.davporImpressoraNaoFiscal || ConfiguracoesECF.prevenda)
            //  {
            excluir.cancelado = "S";
            excluir.canceladoECF = "S";
            // }
            /// else
            //    entidade.DeleteObject(excluir);

            entidade.SaveChanges();

            try
            {                
                string sql = "START TRANSACTION;" +
                "UPDATE transfvendatemp AS v SET v.cancelado = 'S' WHERE v.ip = '" + GlbVariaveis.glb_IP + "' AND v.codigo = '" + excluir.codigo + "'" +
                "AND v.numeroItem = '"+excluir.nrcontrole+"';" +
                "COMMIT;";

                /*string sql = "START TRANSACTION;" +
                "UPDATE transfvendatemp AS v SET v.cancelado = 'S' WHERE v.ip = '"+GlbVariaveis.glb_IP+ "' AND v.codigo = '" + excluir.codigo + "'"+
                "AND v.id = (SELECT id FROM(SELECT t.id FROM transfvendatemp AS t  WHERE t.cancelado = 'N' AND t.ip = '" + GlbVariaveis.glb_IP + "' AND t.codigo = '" + excluir.codigo + "' AND t.quantidade = '"+excluir.quantidade+"' LIMIT 1) AS id);" +
                "COMMIT;";*/

                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
            }
            catch (Exception)
            {
            }

            return true;
        }
        #endregion

        #region Apagar Itens e Formas de Pagamento Limpar venda
        public static bool ApagarItensFormaPagamento(string acao, string pagamento = "")
        {
            /*
             * Acao ItensPagamento = Apaga tanto itens (vendas) como pagamento (caixas)
             * 
              */
            #region StandAlone
            if (!Conexao.onLine && Conexao.tipoConexao == 1)
            {
                if (acao == "ItensPagamentos" || acao == "Itens")
                {
                    System.IO.File.Delete("vendas.yap");
                };
                if (acao == "ItensPagamentos" || acao == "Pagamentos")
                {
                    System.IO.File.Delete("caixas.yap");
                }
                return true;
            }
            #endregion

            if (acao.ToLower() == "pagamentos" && pagamento != "")
            {
                siceEntities entidadeApagar = Conexao.CriarEntidade();

                if (Conexao.tipoConexao == 2)
                    entidadeApagar = Conexao.CriarEntidade(false);

                var excluirPgt = from n in entidadeApagar.caixas
                                 where n.EnderecoIP == GlbVariaveis.glb_IP
                                 && n.tipopagamento == pagamento
                                 select n;
                foreach (var item in excluirPgt)
                {
                    entidadeApagar.DeleteObject(item);
                }
                entidadeApagar.SaveChanges();

                return true;
            }


            siceEntities entidade = Conexao.CriarEntidade();

            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);
            try
            {
                if (acao.ToLower() == "itenspagamentos" || acao.ToLower() == "itens")
                {
                    var objItens = (from v in entidade.vendas
                                    where v.id == GlbVariaveis.glb_IP
                                    select v);
                    foreach (var item in objItens)
                    {
                        entidade.DeleteObject(item);
                    }
                };

                if (acao.ToLower() == "itenspagamentos" || acao.ToLower() == "pagamentos")
                {
                    var objFormaPagamento = (from p in entidade.caixas
                                             where p.EnderecoIP == GlbVariaveis.glb_IP
                                             select p);

                    foreach (var item in objFormaPagamento)
                    {
                        entidade.DeleteObject(item);
                    }
                };
                entidade.SaveChanges();
            }
            catch
            {
                return false;
            }


            if (acao.ToLower() == "transferenciatmp")
            {
                try
                {
                    if (Conexao.tipoConexao == 2)
                        entidade = Conexao.CriarEntidade(false);

                    string SQL = "DELETE FROM transfvendatemp WHERE ip='" + GlbVariaveis.glb_IP + "'";
                    entidade.ExecuteStoreCommand(SQL);
                }
                catch (Exception erro)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Zerar Numero do DAV
        public static bool ZerarNumeroDAVTransfTemp()
        {
            
            try
            {
                siceEntities entidade = Conexao.CriarEntidade();

                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                string SQL =
                    "START TRANSACTION;"+
                        "UPDATE transfvendatemp SET numeroDAV = 0 WHERE ip ='" + GlbVariaveis.glb_IP + "' AND filialdestino = '" + GlbVariaveis.glb_filial+"';"+
                    "COMMIT;";

                entidade.ExecuteStoreCommand(SQL);
            }
            catch (Exception erro)
            {
                //return false;
                MessageBox.Show(erro.ToString());
            }
            
            return true;
        }
        #endregion

        #region Pagamento
        public bool EfetuarPagamento(string dpFinanceiro, string formaPagamento, decimal valor, int idCliente,
                              int idCartao, string nrCartao, string nomeCartao, int parcelas, DateTime vencimentoInicial,
                              int intervalo, DadosCheque dadosCheque, bool imprimirECF = true, bool chamarFinalizacao = true,
                              bool alterarParcelas = false,bool concomitante = true)
        {
            


            if (dpFinanceiro == "Recebimento" || concomitante == false)
                imprimirECF = false;
            // Para pegar apenas a data e não a hora.  

            
            vencimentoInicial = vencimentoInicial.Date;
            decimal valorParcela = 0, restoDivisao = 0;
            vendaFinalizada = false;
            string tipoPagamentoCartao = "CR";

            Clientes cli = new Clientes();
            if (idCliente > 0)
            {
                // REtirado em 04.06.2012 depois que foi detectado que ao fazer a 
                // dispensa de juros e dar um por conta o valor dos juros ficava como 
                // débito por causa da atualização dos débitos por esta função.
               // Clientes.AtualizarDebito(idCliente,Configuracoes.taxaJurosDiario);

                cli.Procura(Convert.ToString(idCliente));

                string sql = "SELECT ifNULL(clienteCrediario,'S')  FROM clientes WHERE codigo like '" + idCliente + "'";
                string liberado = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();

                if (liberado == "N" && (dpFinanceiro == "Venda" || dpFinanceiro == "venda"))
                {
                    MessageBox.Show("Não permitido vender no crediario para esse cliente", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }


            if (parcelas == 0) parcelas = 1;


            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2 && dpFinanceiro == "Venda")
                entidade = Conexao.CriarEntidade(false);


            #region Forma de Pagamento
            /// DH = Dinheiro
            /// CA = Cartão
            /// CH = Cheque
            /// TI = Ticket
            /// CR = Crediário
            /// DV = Devolução
            /// PF = Fidelidade
            #endregion

            if (valorLiquido <= 0)
                throw new Exception("Não foi repassado o total da venda.");
            if (valor < 0)
                throw new Exception("Pagamento não pode ser negativo.");
            /// Subtotalizar o Cupom Fiscal
            /// 

            // Iniciar o Fechamento do Cupom Fiscal
            Funcoes.TravarTeclado(false);
            if (imprimirECF)
            if (!IniciarFechamentoECF(desconto, encargos)) return false;
            // EfetuarPagamento            

            decimal devolucaoCaixas = 0;
            try
            {
                devolucaoCaixas = Conexao.CriarEntidade().ExecuteStoreQuery<decimal>("select IFNULL(SUM(IFNULL(valor,0)),0) as valor FROM caixas WHERE enderecoip ='" + GlbVariaveis.glb_IP + "' AND codigoFilial ='" + GlbVariaveis.glb_filial + "' AND tipopagamento = 'DV' AND data = CURRENT_DATE").FirstOrDefault();
            }
            catch
            {
                devolucaoCaixas = 0;
            }

            switch (formaPagamento)
            {
                case "DV":
                case "DH":
                case "AV":
                        #region
                    if (imprimirECF)
                    {
                        if (!ChamarPagamentoECF(formaPagamento, 0, valor))
                            return false;
                    };

                    bool troco = false;
                    if (valor >= (valorLiquido - devolucaoCaixas))
                    {
                        this.troco = valor - (valorLiquido - devolucaoCaixas);
                        // TotalDevolucao entra aqui por que ja foi dado como forma de pagamento
                        // pois é a primeira forma de pagamento a ser lancada
                        valor = valor - this.troco;
                        troco = true;
                    }

                    if ((restante > 0) && (valor > restante) && troco == false)
                    {
                        this.troco = (valor - restante);
                        valor = restante;
                    }

                    /*
                    if (totalDevolucao > 0 && formaPagamento == "DH")
                    {
                        valor = valorLiquido - totalDevolucao;
                    }
                    */

                    /// If operandus modus = StandAlone
                    /// 
                    #region StandAlone
                    if (!Conexao.onLine && Conexao.tipoConexao == 1)
                    {
                        IObjectContainer tabela = Db4oFactory.OpenFile("caixas.yap");
                        StandAloneCaixa registro = new StandAloneCaixa();
                        registro.id = Guid.NewGuid();
                        registro.ip = GlbVariaveis.glb_IP;
                        registro.data = GlbVariaveis.Sys_Data;
                        registro.dpFinanceiro = dpFinanceiro;
                        registro.valor = valorLiquido; //(!)Atenção Valor líquido aqui por que nao existe outra forma de pagamento
                        registro.operador = GlbVariaveis.glb_Usuario;
                        registro.tipoPagamento = formaPagamento;
                        registro.vendedor = "000";
                        tabela.Store(registro);
                        tabela.Close();
                        IniciarFechamentoECF(desconto, encargos);
                        ChamarPagamentoECF("DH", 0, valorLiquido);
                        FecharCupomECF(FuncoesECF.MensagemCupomECF(valorLiquido, numeroPreVenda, numeroDAV));
                        Finalizar(false, false, false);
                        this.vendaFinalizada = true;
                        return true;
                    };
                    #endregion

                    try
                    {

                        caixas pagamento = new caixas();
                        pagamento.EnderecoIP = GlbVariaveis.glb_IP;
                        pagamento.valor = valor;
                        pagamento.caixa = 0;
                        pagamento.data = GlbVariaveis.Sys_Data;
                        pagamento.tipopagamento = formaPagamento;
                        pagamento.descricaopag = "Dinheiro";
                        pagamento.operador = GlbVariaveis.glb_Usuario;
                        pagamento.dpfinanceiro = dpFinanceiro;
                        pagamento.filialorigem = GlbVariaveis.glb_filial;
                        pagamento.CodigoFilial = GlbVariaveis.glb_filial;
                        if (this.valorLiquido != valor)
                            pagamento.historico = "Entrada";
                        else
                            pagamento.historico = "*";

                        pagamento.vendedor = "000";
                        entidade.AddTocaixas(pagamento);
                        entidade.SaveChanges();

                    }
                    catch (Exception erro)
                    {
                        ApagarItensFormaPagamento("Pagamentos", "DH");
                        ApagarItensFormaPagamento("Pagamentos", "DV");
                        throw new Exception(erro.InnerException + ". Não foi possível lançar a forma de pagamento");
                    }
                    #endregion
                    break;
                case "CA":
                case "FN":
                case "TI":
                case "PF": // Programa Fideliade IQCARD
                case "CP": // Cartão presente
                    try
                    {
                        #region
                        PermissaoVendaClasse();
                                                

                        string redeCartao = "0"; // Amex,Visa,Master


                        var cartoesDados = from c in entidade.cartoes
                                      where c.id == idCartao
                                      select c;
                       
                        var cartoes = cartoesDados.ToList();

                        if (cartoes.Count() == 0)
                            throw new Exception("Cartão não encontrado !");

                        foreach (var item in cartoes)
                        {
                            tipoPagamentoCartao = item.tipopagamento;
                            redeCartao = item.rede.ToString();
                            formaPagamento = item.tipo;

                            if (item.tipopagamento == "DB" && parcelas > 1)
                            {
                                return false;
                            }

                        }
                        // Usando a variavel imprimirECF por que é usado para recebimento também.
                        bool chamarTEF = true;

                        string SQL = "SELECT TEF FROM cartoes WHERE id = '" + idCartao + "'";

                        string flagTEF = entidade.ExecuteStoreQuery<string>(SQL).FirstOrDefault();
  

                        if (formaPagamento == "FN" || flagTEF == "N")
                        {
                            chamarTEF = false;
                            TEF.valorAprovadoTEF = 0;
                        }

                        if(formaPagamento=="PF" && !nrCartao.StartsWith("0359"))
                        {
                            formaPagamento = "CA";
                        }
                        
                        //iq Card
                        if(nrCartao.StartsWith("0359"))
                        {
                            #region

                            IqCard.Parametros();

                            chamarTEF = false;
                            TEF.valorAprovadoTEF = 0;
                            formaPagamento = "PF";


                           if(!IqCard.VerificarNumeroCartao(nrCartao))
                            {
                                MessageBox.Show("Nr. IQCARD não pode ser verificado");
                                throw new Exception("Nr. IQCARD não pode ser verificado");
                            }

                           if(nrCartao.Length==12)
                            {
                                formaPagamento = "CP"; // Aqui transforma a venda                                
                            };

                            ServiceReference1.WSIQPassClient card = new ServiceReference1.WSIQPassClient();
                            double saldo = 0;

                            if(formaPagamento=="PF")
                             saldo = card.SomarSaldoBitCoin(GlbVariaveis.chavePrivada, nrCartao);

                            //if(saldo+0.01<Convert.ToDouble(valor))
                            //{
                            //    MessageBox.Show("IQCARD não possui moeda virtual neste valor");
                            //    throw new Exception("IQCARD não possui moeda virtual suficiente");                                
                            //}

                            FrmMsgOperador msg3 = new FrmMsgOperador("", "Aguardando confirmação pelo usuário");
                            msg3.Show();
                            Application.DoEvents();

                            if (IqCard.parametros.solicitarAutorizacaoResgateBitCoin=="S" && formaPagamento=="PF" && saldo + 0.01 > Convert.ToDouble(valor))
                            {

                              

                                ServiceReference1.Resgate autorizacao = new ServiceReference1.Resgate()
                                {
                                    idCartao = nrCartao,
                                    idEmpresa = GlbVariaveis.glb_chaveIQCard,
                                    mensagem = GlbVariaveis.nomeEmpresa + " Solicita pagamento com moeda virtual. Valor R$: " + valor.ToString(),
                                    resgateConfirmado = false,
                                    pontoResgate = 0,
                                    idBrinde=""                                    
                                };
                                string idAutorizacao = card.IncluirResgate(GlbVariaveis.chavePrivada, autorizacao);
                                int tentativas = 0;
                                while (true)
                                {

                                    if (tentativas>10)
                                    {
                                        MessageBox.Show("Transação não foi permitida.");
                                        msg3.Dispose();
                                        throw new Exception("Transação não foi permitida. É necessário que o usuário tenha conexão a internet e esteja com o aplicativo instalado.");
                                    }

                                    if (card.VerificarAutorizacao(GlbVariaveis.chavePrivada, idAutorizacao, nrCartao))
                                        {
                                        break;
                                        }

                                    Thread.Sleep(2000);
                                    tentativas++;
                                }

                            }


                            if (ConfiguracoesECF.pdv == true)
                            {
                                try
                                {
                                    // A variavel nrCartao é adicionado o id do Pedio para que a funcao 
                                    // do Web Service entenda que existe um idPedio de um pedido e verifique 
                                    // se já teve a forma de pagamento liberada
                                    idTransacaoIQCARD = card.DebitarBitCoin(GlbVariaveis.chavePrivada, nrCartao+idPedidoIQCARD, GlbVariaveis.glb_chaveIQCard, Convert.ToDouble(valor), "0","","");


                                    if (string.IsNullOrEmpty(Venda.IQCard))
                                    {
                                        Venda.IQCard = nrCartao;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    msg3.Dispose();
                                    MessageBox.Show(ex.Message);
                                    return false;
                                }
                            }


                            msg3.Dispose();
                            #endregion
                        };                        

                        // Para processar quando não existir TEF.

                            valorParcela = Math.Round((valor / parcelas), 2);
                            restoDivisao = valor - Math.Round((valorParcela * parcelas), 2);

                        if (ConfiguracoesECF.tefDiscado && (formaPagamento == "CA" || formaPagamento == "CH" || formaPagamento == "TI") && chamarTEF && idCartao > 0 && ConfiguracoesECF.idECF>0)
                        {
                            FrmMsgOperador msg = new FrmMsgOperador("", "Processando TEF! " + GlbVariaveis.glb_Usuario + " o que você acha de tomar um cafezinho!");
                            msg.Show();
                            Application.DoEvents();

                            if (TEF.ChamarGerenciador(formaPagamento, valor, idCartao, dpFinanceiro) == 0)
                            {
                                TEF.valorAprovadoTEF = 0;
                                msg.Dispose();
                                return false;
                            }

                            if (TEF.valorAprovadoTEF == 0)
                            {
                                msg.Dispose();
                                return false;
                            }
                            msg.Dispose();
                        }

                        
                        if ((ConfiguracoesECF.tefDiscado || ConfiguracoesECF.tefDedicado) && TEF.valorAprovadoTEF > 0 && ConfiguracoesECF.idECF>0 )
                        {
                            decimal saqueTEF = 0;
                            if (valor < TEF.valorAprovadoTEF)
                                saqueTEF = TEF.valorAprovadoTEF - valor;

                            valorParcela = Math.Round((TEF.valorAprovadoTEF / parcelas), 2);
                            restoDivisao = TEF.valorAprovadoTEF - Math.Round((valorParcela * parcelas), 2);
                  

                           // valorParcela = TEF.valorAprovadoTEF;
                            desconto = desconto + TEF.valorDescontoTEF;
                            valorLiquido = valorLiquido - TEF.valorDescontoTEF + saqueTEF ;


                        }

                        // For do Parcelamento
                        for (int i = 0; i < parcelas; i++)
                        {
                            caixas pagamento = new caixas();
                            if (i == 0)
                                valorParcela += restoDivisao;

                            pagamento.EnderecoIP = GlbVariaveis.glb_IP;
                            pagamento.CodigoFilial = GlbVariaveis.glb_filial;
                            pagamento.valor = valorParcela;
                            pagamento.caixa = 0;
                            pagamento.vencimento = tipoPagamentoCartao == "DB" ? GlbVariaveis.Sys_Data.AddDays(1) : vencimentoInicial.AddMonths(i);
                            pagamento.data = GlbVariaveis.Sys_Data;
                            pagamento.tipopagamento = formaPagamento;
                            pagamento.descricaopag = "Cartão";
                            pagamento.operador = GlbVariaveis.glb_Usuario;
                            pagamento.dpfinanceiro = dpFinanceiro;
                            pagamento.filialorigem = GlbVariaveis.glb_filial;
                            pagamento.Cartao = cartoes.FirstOrDefault().descricao;
                            pagamento.numeroCartao = nrCartao.ToString();
                            pagamento.nome = nomeCartao;
                            // pagamento.codigocartao = idCartao.ToString();
                            if (this.valorLiquido != valor)
                                pagamento.historico = "Entrada";
                            else
                                pagamento.historico = "*";

                            pagamento.vendedor = "000";
                            entidade.AddTocaixas(pagamento);


                            entidade.SaveChanges();

                            var tabela = "";

                            /*if (ConfiguracoesECF.pdv)
                            {
                                tabela = "caixas";
                            }
                            else
                            {
                                tabela = "caixadav";
                            }*/

                            string update = "UPDATE caixas set codigocartao = '" + idCartao.ToString() + "' WHERE EnderecoIP  = '" + GlbVariaveis.glb_IP + "' and operador = '" + GlbVariaveis.glb_Usuario + "' and codigofilial =  '" + GlbVariaveis.glb_filial +  "';";
                            try
                            {
                                entidade.ExecuteStoreCommand(update);
                            }
                            catch(Exception)
                            {

                            }

                            if (i == 0)
                            {
                                valorParcela -= restoDivisao;
                                restoDivisao = 0;
                            }
                        } // Fim do For
                    }
                    catch (Exception erro)
                    {                        
                        ApagarItensFormaPagamento("Pagamentos", "CA");
                        ApagarItensFormaPagamento("Pagamentos", "FN");
                        ApagarItensFormaPagamento("Pagamentos", "TI");
                        ApagarItensFormaPagamento("Pagamentos", "PF");
                        throw new Exception(erro.InnerException + "Restrição na forma de pagamento. Verifica parcelas e tipo de cartão Débito ou Crédito !!");
                    }
                    #endregion
                    break;

                case "CR":
                    try
                    {
                        #region
                        PermissaoVendaClasse();
                        if (idCliente == 0) return false;
                        
                        Clientes.ultCPF = cli.cpf;

                        int inadimplente = (from n in entidade.crmovclientes
                                            where n.codigo == idCliente
                                            && n.vencimento < GlbVariaveis.Sys_Data
                                            select n.vencimento).Count();
                        if (inadimplente > 0)
                            Clientes.inadimplente = true;
                        else
                            Clientes.inadimplente = false;

                        int parcelaPrd = Produtos.ParcelamentoMaxVenda();

                        if (parcelaPrd>0)
                        parcelamentoMaximo = parcelaPrd;

                        // Obter dias do Primeiro Vencimento para Comparar com as configuracoes
                        int diasVencimento = vencimentoInicial.Subtract(GlbVariaveis.Sys_Data).Days;

                        if (vencimentoInicial < GlbVariaveis.Sys_Data)
                        {
                            throw new Exception("Vencimento não pode ser menor que a data atual");
                        }

                        if (parcelas > parcelamentoMaximo && parcelamentoMaximo > 0)
                        {
                            if (parcelaPrd>0)
                                throw new Exception("Produto com parcelamento máximo de parcelas: " + parcelamentoMaximo.ToString());

                            throw new Exception("Parcelamento não permitido, máximo de parcelas: " + parcelamentoMaximo.ToString());
                        }

                        if (diasVencimento > Configuracoes.diasPrimeiroVenc)
                        {
                            FrmLogon Logon = new FrmLogon();
                            Operador.autorizado = false;
                            Logon.idCliente = idCliente;
                            Logon.campo = "vendaaltvenc";
                            Logon.lblDescricao.Text = "EXCEDEU 1º VENCIMENTO";
                            Logon.txtDescricao.Text = "Vencimento da 1º parcela ultrapassou " +
                                Environment.NewLine + "a configuração permitida !";
                            Logon.ShowDialog();
                            if (!Operador.autorizado)
                            {
                                return false;
                            }
                            else
                            {
                                string observacao = "Cliente.: "+cli.nome + Environment.NewLine + Environment.NewLine + 
                                                    "Vencimento da 1º parcela ultrapassou " +
                                                     Environment.NewLine + "a configuração permitida !";

                                //siceEntities entidade = Conexao.CriarEntidade();
                                auditoria objAuditoria = new auditoria();
                                objAuditoria.acao = "Venda Crediario";
                                objAuditoria.CodigoFilial = GlbVariaveis.glb_filial;
                                objAuditoria.codigoproduto = Venda.dadosConsumidor.idConsumidor;
                                objAuditoria.data = DateTime.Now.Date;
                                objAuditoria.documento = 0;
                                objAuditoria.hora = DateTime.Now.TimeOfDay;
                                objAuditoria.local = "SICE.pdv";
                                objAuditoria.observacao = observacao;
                                objAuditoria.usuariosolicitante = Operador.ultimoOperadorAutorizado;
                                objAuditoria.usuario = GlbVariaveis.glb_Usuario;
                                objAuditoria.tabela = "Clientes";
                                entidade.AddToauditoria(objAuditoria);
                                entidade.SaveChanges();

                                try
                                {
                                    var lisCodigo = (from a in entidade.auditoria
                                                     where a.tabela == "Clientes" && a.usuariosolicitante == Operador.ultimoOperadorAutorizado && a.local == "SICE.pdv" && a.data == GlbVariaveis.Sys_Data
                                                     select a.id).ToList().Max();

                                    auditoriaVenda n = new auditoriaVenda();
                                    n.inc = int.Parse(lisCodigo.ToString());
                                    Venda.listAuditoriaVenda.Add(n);
                                }
                                catch (Exception erro)
                                {

                                }
                            }
                        };

                       // Clientes cli = new Clientes();
                        if (idCliente > 0)
                        {
                            // REtirado em 04.06.2012 depois que foi detectado que ao fazer a 
                            // dispensa de juros e dar um por conta o valor dos juros ficava como 
                            // débito por causa da atualização dos débitos por esta função.
                            // Clientes.AtualizarDebito(idCliente,Configuracoes.taxaJurosDiario);

                            cli.Procura(Convert.ToString(idCliente));

                            

                        }
                        // Se o cliente tiver com o crédito 0 então o cliente tem um crédito ilimitado
                        // PAra restringir defina um limite para o cliente
                        if (cli.credito != 0 && (cli.saldoAtual < valor))
                        {
                            FrmLogon Logon = new FrmLogon();
                            Operador.autorizado = false;
                            Logon.idCliente = idCliente;
                            Logon.campo = "clisaldo";
                            Logon.lblDescricao.Text = "SALDO INSUFICIENTE";
                            Logon.txtDescricao.Text =
                             cli.nome + Environment.NewLine + Environment.NewLine +
                            "SALDO DISPONÍVEL        : " + string.Format("{0:C2}", cli.saldoAtual) + Environment.NewLine +
                            "VALOR ULTRAPASSADO : " + string.Format("{0:C2}", valor - cli.saldoAtual);
                            Logon.ShowDialog();
                            Clientes.limiteUltrapassado = true;
                            if (!Operador.autorizado)
                            {
                                return false;
                            }
                            else
                            {
                                string observacao = "Cliente.: "+cli.nome + Environment.NewLine + Environment.NewLine +
                                                    "SALDO DISPONÍVEL        : " + string.Format("{0:C2}", cli.saldoAtual) + Environment.NewLine +
                                                    "VALOR ULTRAPASSADO : " + string.Format("{0:C2}", valor - cli.saldoAtual);
                                //siceEntities entidade = Conexao.CriarEntidade();
                                auditoria objAuditoria = new auditoria();
                                objAuditoria.acao = "Venda Crediario";
                                objAuditoria.CodigoFilial = GlbVariaveis.glb_filial;
                                objAuditoria.codigoproduto = Venda.dadosConsumidor.idConsumidor; 
                                objAuditoria.data = DateTime.Now.Date;
                                objAuditoria.documento = 0;
                                objAuditoria.hora = DateTime.Now.TimeOfDay;
                                objAuditoria.local = "SICE.pdv";
                                objAuditoria.observacao = observacao;
                                objAuditoria.usuariosolicitante = Operador.ultimoOperadorAutorizado;
                                objAuditoria.usuario = GlbVariaveis.glb_Usuario;
                                objAuditoria.tabela = "Clientes";
                                entidade.AddToauditoria(objAuditoria);
                                entidade.SaveChanges();

                                try
                                {
                                    
                                    var lisCodigo = (from a in entidade.auditoria
                                                     where a.tabela == "Clientes" && a.usuariosolicitante == Operador.ultimoOperadorAutorizado && a.local == "SICE.pdv" && a.data == GlbVariaveis.Sys_Data
                                                     select a.id).ToList().Max();

                                    auditoriaVenda n = new auditoriaVenda();
                                    n.inc = int.Parse(lisCodigo.ToString());
                                    Venda.listAuditoriaVenda.Add(n);
                                }
                                catch (Exception erro)
                                {
                                    
                                }
                            }
                        };

                        valorParcela = Math.Round((valor / parcelas), 2);
                        restoDivisao = valor - Math.Round((valorParcela * parcelas), 2);
                        Application.DoEvents();
                        if (cli.situacaoRestritiva)
                        {

                            FrmLogon Logon = new FrmLogon();
                            Operador.autorizado = false;
                            Logon.idCliente = idCliente;
                            Logon.campo = "clirestricao";
                            Logon.lblDescricao.Text = "CLIENTE C/ RESTRIÇÃO";
                            Logon.txtDescricao.Text =
                            cli.nome + Environment.NewLine + Environment.NewLine +
                            "Situação           : " + cli.situacao;
                            
                            Logon.ShowDialog();

                            Clientes.restricao = true;
                            if (!Operador.autorizado)
                            {
                                throw new Exception("Sem autorização, cliente com restrição: " + cli.situacao);
                            }
                            else
                            {
                                string observacao = "Cliente.: "+cli.nome + Environment.NewLine + Environment.NewLine +
                                                    " Situação           : " + cli.situacao;

                                //siceEntities entidade = Conexao.CriarEntidade();
                                auditoria objAuditoria = new auditoria();
                                objAuditoria.acao = "Venda Crediario";
                                objAuditoria.CodigoFilial = GlbVariaveis.glb_filial;
                                objAuditoria.codigoproduto = Venda.dadosConsumidor.idConsumidor;
                                objAuditoria.data = DateTime.Now.Date;
                                objAuditoria.documento = 0;
                                objAuditoria.hora = DateTime.Now.TimeOfDay;
                                objAuditoria.local = "SICE.pdv";
                                objAuditoria.observacao = observacao;
                                objAuditoria.usuariosolicitante = Operador.ultimoOperadorAutorizado;
                                objAuditoria.usuario = GlbVariaveis.glb_Usuario;
                                objAuditoria.tabela = "Clientes";
                                entidade.AddToauditoria(objAuditoria);
                                entidade.SaveChanges();
                            }

                            try
                            {
                                var lisCodigo = (from a in entidade.auditoria
                                                 where a.tabela == "Clientes" && a.usuariosolicitante == Operador.ultimoOperadorAutorizado && a.local == "SICE.pdv" && a.data == GlbVariaveis.Sys_Data
                                                 select a.id).ToList().Max();

                                auditoriaVenda n = new auditoriaVenda();
                                n.inc = int.Parse(lisCodigo.ToString());
                                Venda.listAuditoriaVenda.Add(n);
                            }
                            catch (Exception erro)
                            {

                            }
                            
                        }

                        // For do Parcelamento                        
                        for (int i = 0; i < parcelas; i++)
                        {
                            caixas pagamento = new caixas();
                            if (i == 0)
                                valorParcela += restoDivisao;

                            pagamento.EnderecoIP = GlbVariaveis.glb_IP;
                            pagamento.CodigoFilial = GlbVariaveis.glb_filial;
                            pagamento.valor = valorParcela;
                            pagamento.caixa = 0;
                            if (i == 0)
                                pagamento.vencimento = vencimentoInicial;
                            else
                            {
                                if (intervalo == 30 && !Configuracoes.diasCorridos)
                                    pagamento.vencimento = vencimentoInicial.AddMonths(i);
                                if (intervalo != 30 || Configuracoes.diasCorridos)
                                    pagamento.vencimento = vencimentoInicial.AddDays(intervalo * i);
                            }
                            pagamento.codigocliente = idCliente;
                            pagamento.nome = cli.nome;
                            pagamento.data = GlbVariaveis.Sys_Data;
                            pagamento.tipopagamento = formaPagamento;
                            pagamento.descricaopag = "Crediário";
                            pagamento.operador = GlbVariaveis.glb_Usuario;
                            pagamento.dpfinanceiro = dpFinanceiro;
                            pagamento.filialorigem = GlbVariaveis.glb_filial;
                            pagamento.historico = "*";
                            pagamento.vendedor = vendedor == null ? "000" : vendedor;
                            pagamento.Nrparcela = (i + 1).ToString() + @"/" + parcelas.ToString();
                            entidade.AddTocaixas(pagamento);
                            entidade.SaveChanges();

                            if (i == 0)
                            {
                                valorParcela -= restoDivisao;
                                restoDivisao = 0;
                            }
                        } // Fim do For

                        if (alterarParcelas)
                        {
                            FrmAlterarParcela alterar = new FrmAlterarParcela(formaPagamento, valor, idCliente);
                            alterar.ShowDialog();
                            
                            if (FrmAlterarParcela.totalParcelas== 0)
                            throw new Exception("Cancelada. ");
                            
                            alterar.Dispose();
                        }

                        if (imprimirECF)
                        {
                            if (!ChamarPagamentoECF(formaPagamento, 0, valor))
                            {
                                ApagarItensFormaPagamento("Pagamentos", "CR");
                                throw new Exception(" Verifique se a forma de pagamento está cadastrada no ECF.");
                            }
                        }

                    }
                    catch (Exception erro)
                    {
                        ApagarItensFormaPagamento("Pagamentos", "CR");
                        Funcoes.TravarTeclado(false);
                        throw new Exception(erro.Message + " Restrição na forma de pagamento.");
                    }
                    #endregion
                    break;

                case "CH":
                    try
                    {
                        #region
                        if (idCliente == 0) return false;

                        // Obter dias do Primeiro Vencimento para Comparar com as configuracoes
                        int diasVencimento = vencimentoInicial.Subtract(GlbVariaveis.Sys_Data).Days;


                        if (vencimentoInicial < GlbVariaveis.Sys_Data)
                        {
                            throw new Exception("Vencimento não pode ser menor que a data atual");
                        }

                        if (diasVencimento > Configuracoes.diasPrimeiroVenc)
                        {
                            FrmLogon Logon = new FrmLogon();
                            Operador.autorizado = false;
                            Logon.idCliente = idCliente;
                            Logon.campo = "vendaaltvenc";
                            Logon.lblDescricao.Text = "EXCEDEU 1º VENC. CHEQUE";
                            Logon.txtDescricao.Text = "Vencimento da 1º parcela com cheque ultrapassou" + Environment.NewLine + "a configuração permitida !";
                            Logon.ShowDialog();
                            if (!Operador.autorizado) return false;
                        };

                        if (cli.saldoAtual < valor)
                        {
                            FrmLogon Logon = new FrmLogon();
                            Operador.autorizado = false;
                            Logon.idCliente = idCliente;
                            Logon.campo = "clisaldo";
                            Logon.lblDescricao.Text = "SALDO INSUFICIENTE";
                            Logon.txtDescricao.Text =
                             cli.nome + Environment.NewLine + Environment.NewLine +
                            "SALDO DISPONÍVEL        : " + string.Format("{0:C2}", cli.saldoAtual) + Environment.NewLine +
                            "VALOR ULTRAPASSADO : " + string.Format("{0:C2}", valor - cli.saldoAtual);
                            Logon.ShowDialog();
                            if (!Operador.autorizado) return false;

                        };
                       
                        valorParcela = Math.Round((valor / parcelas), 2);
                        restoDivisao = valor - Math.Round((valorParcela * parcelas), 2);


                        // For do Parcelamento
                        for (int i = 0; i < parcelas; i++)
                        {
                            caixas pagamento = new caixas();
                            if (i == 0)
                                valorParcela += restoDivisao;

                            pagamento.EnderecoIP = GlbVariaveis.glb_IP;
                            pagamento.CodigoFilial = GlbVariaveis.glb_filial;
                            pagamento.valor = valorParcela;
                            pagamento.caixa = 0;
                            if (i == 0)
                                pagamento.vencimento = vencimentoInicial;
                            else
                            {
                                if (intervalo == 30) pagamento.vencimento = vencimentoInicial.AddMonths(i);
                                if (intervalo != 30) pagamento.vencimento = vencimentoInicial.AddDays(intervalo * i);
                            }
                            pagamento.codigocliente = idCliente;
                            pagamento.nome = cli.nome;
                            pagamento.data = GlbVariaveis.Sys_Data;
                            pagamento.tipopagamento = formaPagamento;
                            pagamento.descricaopag = "Cheque";
                            pagamento.operador = GlbVariaveis.glb_Usuario;
                            pagamento.dpfinanceiro = dpFinanceiro;
                            pagamento.filialorigem = GlbVariaveis.glb_filial;
                            pagamento.historico = "*";
                            pagamento.vendedor = vendedor;
                            // Dados do Cheque
                            pagamento.valorCheque = valorParcela;
                            pagamento.banco = dadosCheque.codBanco.ToString();
                            pagamento.agencia = dadosCheque.agencia;
                            pagamento.cheque = dadosCheque.numeroCheque + i;
                            pagamento.NomeCheque = dadosCheque.nomeCheque;
                            pagamento.cpfcnpjch = dadosCheque.cpfCheque;

                            //?? FAlta telefone
                            entidade.AddTocaixas(pagamento);
                            entidade.SaveChanges();
                            if (i == 0)
                            {
                                valorParcela -= restoDivisao;
                                restoDivisao = 0;
                            }
                        }


                        if (alterarParcelas)
                        {
                            FrmAlterarParcela alterar = new FrmAlterarParcela(formaPagamento, valor, idCliente);
                            alterar.ShowDialog();
                            alterar.Dispose();
                        }

                        if (imprimirECF)
                        {
                            if (!ChamarPagamentoECF(formaPagamento, idCartao, valor))
                            {
                                ApagarItensFormaPagamento("pagamentos", "CH");
                                throw new Exception(" Verifique se a forma de pagamento está cadastrada no ECF.");
                            }
                        }

                    }
                    catch (Exception erro)
                    {
                        ApagarItensFormaPagamento("Pagamentos", "CH");
                        Funcoes.TravarTeclado(false);
                        throw new Exception(erro.InnerException + "Restrição na forma de pagamento.");
                    }
                    #endregion

                    break;

            }

            ///Somando as Formas de Pagamento
            ///Se a soma for igual ao total da venda
            ///então a venda será encaminhada para finalização     
            ///           

            Funcoes.TravarTeclado(true);

             decimal? soma = (from n in entidade.caixas
                               where n.EnderecoIP == GlbVariaveis.glb_IP                               
                                && n.CodigoFilial == GlbVariaveis.glb_filial
                               select (decimal?)n.valor).Sum();


             if (!chamarFinalizacao)
             {
                 Funcoes.TravarTeclado(false);
                 return true;
             }             

            //foreach (var item in soma)
            //{

                if (soma > this.valorLiquido)
                {
                    this.valorLiquido = soma.Value;
                   // Funcoes.TravarTeclado(false);
                   //  this.vendaFinalizada = false;
                   //  throw new Exception("Forma de pagamentos maior que o total da venda");
                };

            if (soma.GetValueOrDefault() != this.valorLiquido && dpFinanceiro == "Venda")
            {
                        decimal totalItens = 0;

                        if (numeroDAV != 0)
                        {
                            totalItens = (from t in entidade.vendas
                                        where t.documento == numeroDAV
                                        select t.total).Sum();
                        }
                        else
                        {
                            totalItens = (from t in entidade.vendas
                                        where t.id == GlbVariaveis.glb_IP
                                        select t.total).Sum();
                        }

                        this.valorLiquido = totalItens;
                }

                if (soma.GetValueOrDefault() == this.valorLiquido)
                {

                    try
                    {
                        if (dpFinanceiro == "Venda")
                        {

                            Application.DoEvents();
                            // Criado esta função para o CIELO PREMIA por que retorna um desconto
                            // e o cupom nao deve ter sido iniciado o fechamento
                            if (concomitante == false)
                            {
                                FrmMsgOperador msg = new FrmMsgOperador("", "Encerrando cupom.");                                    

                                while (true)
                                {
                                    
                                    
                                    if (troco==0)
                                    msg.Show();

                                    Application.DoEvents();

                                    try
                                    {
                                        Funcoes.TravarTeclado(true);                                                                               
                                        VerificaImpressoraLigada();
                                        System.Threading.Thread.Sleep(100);                                        
                                        AbrirGaveta();
                                        System.Threading.Thread.Sleep(100);
                                        break;
                                    }

                                    catch (Exception erro)
                                    {
                                        Funcoes.TravarTeclado(false);
                                        if (MessageBox.Show(erro.Message + "Impressora não responde. Tentar imprimir novamente?", "SICEpdv.net", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
                                        {
                                            NaoConfirmarTransacao();
                                            return false;
                                        }
                                    }
                                }


                                IniciarFechamentoECF(desconto, encargos);

                                var pagamentos = SelecionaPagamentoVenda();
                                if (ConfiguracoesECF.tefDiscado)
                                {
                                    pagamentos = from n in pagamentos
                                                 where n.tipopagamento != "CA"
                                                 select n;
                                }

                                try
                                {   
                                    foreach (var itemPag in pagamentos)
                                    {

                                        decimal valorFormaPgt = itemPag.valor;

                                        if (ConfiguracoesECF.pdv == true)
                                            ChamarPagamentoECF(itemPag.tipopagamento, 0, valorFormaPgt + troco);
                                    }
                                }
                                catch (Exception erro)
                                {
                                    MessageBox.Show(erro.ToString());
                                }

                                if (TEF.lstPagamento.Count > 0)
                                {
                                    foreach (var itemCA in TEF.lstPagamento)
                                    {
                                        string tpPagamento = "CA";
                                        if (itemCA.formaPagamento.ToLower() == "cheque")
                                            tpPagamento = "CH";
                                        ChamarPagamentoECF(tpPagamento, 0, itemCA.valor);
                                    }
                                }

                                try
                                {
                                    if(ConfiguracoesECF.pdv == true)
                                        FecharCupomECF(FuncoesECF.MensagemCupomECF(valorLiquido, numeroPreVenda, numeroDAV));

                                    Application.DoEvents();

                                    if (TEF.Transacoes("ntransacao") > 0)// && idCartao > 0) Para imprimir o comprovante TEF se houver transacao. Isso foi 
                                    // colocado devido a venda com 2 carteos quando 1 nao era aprovado e era finalizado com outra forma de pagamento.
                                    {
                                        System.Threading.Thread.Sleep(300);
                                        msg.Visible = false;                                        
                                        ImprimirTEF();                                        
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("Erro finalizando Venda no ECF: " + ex.Message);
                                }
                                finally
                                {
                                    msg.Dispose();
                                    System.Threading.Thread.Sleep(100);
                                    GravarGtECF();
                                    Funcoes.TravarTeclado(false);
                                }
                            }

                            Finalizar(false, false, false);
                            this.vendaFinalizada = true;
                            if (_pdv.numeroDAV > 0)
                                ConfiguracoesECF.lerXML();
                            

                        }

                        if (dpFinanceiro == "Recebimento")
                        {
                            
                            Clientes.QuitarDebito(idCliente, FrmExtratoCliente.parcelas.ToList(), desconto, numeroDevolucao);
                            troco = 0;
                            this.vendaFinalizada = true;
                        }
                    }
                    catch (Exception erro)
                    {
                        Funcoes.TravarTeclado(false);
                        MessageBox.Show(erro.ToString());
                    }
                    
                };
           

            return true;
        }

        private void PermissaoVendaClasse()
        {

            if (Conexao.onLine && Configuracoes.vendaPorclasse && classeVenda != "0000")
            {
                var dados = Produtos.PermissaoClasseVenda(classeVenda).ToList();

                if (dados.Count() > 0)
                {
                    foreach (var item in dados)
                    {
                        MessageBox.Show(item.codigo + " " + item.produto.Trim() + ". Somente na classificação: " + item.classe);
                    }
                    throw new Exception("Escolha os itens com o mesmo tipo de classificação. Ou Finalize a venda em Dinheiro.");
                }

            }
        }

        
        #endregion

        #region Excluir forma de Pagamento
        public static void ExcluirPagamento(string formaPagamento)
        {

            /// If operandus modus = StandAlone
            #region StandAlone
            if (!Conexao.onLine && Conexao.tipoConexao == 1)
            {
                if (System.IO.File.Exists("caixas.yap"))
                    System.IO.File.Delete("caixas.yap");
                return;
            };
            #endregion

            try
            {
                siceEntities entidade = Conexao.CriarEntidade();
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                var linhas = (from c in entidade.caixas
                              where c.tipopagamento.Contains(formaPagamento)
                              && c.EnderecoIP == GlbVariaveis.glb_IP
                              select c);

                foreach (var forma in linhas)
                {
                    entidade.DeleteObject(forma);
                }
                entidade.SaveChanges();

            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message + " Não foi possível limpar as formas de pagamentos!");
            }
        }
        #endregion

        #region Excluir itens venda tarns temp
        public static void ExcluirVendaTransf()
        {

            /// If operandus modus = StandAlone

            try
            {
                siceEntities entidade = Conexao.CriarEntidade();
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);


                string sql = "DELETE FROM transfvendatemp where ip = '"+GlbVariaveis.glb_IP+"' AND filialdestino = '"+GlbVariaveis.glb_filial+"'";

                Conexao.CriarEntidade().ExecuteStoreCommand(sql);

            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message + " Não foi possível limpar as venda transf!");
            }
        }
        #endregion

        #region Finalização da Venda
        public int Finalizar(bool gerarPreVenda, bool gerarDAV, bool PED, bool VerificarPagamentos = true, bool verificarVenda = true)
        {
            string serieNF = "1";
            string subserieNF = "1";
            string modDocFiscal = "2D";
            DateTime dataAutorizacao = DateTime.Now.Date;
            string chaveNFC = null;
            string protocolo = null;
            
            // Verificando Estado da venda
            if(vendaFinalizada) return 0;

           
            if (FuncoesECF.CupomFiscalAberto())
            {               
                //Funcoes.TravarTeclado(false);
                
                throw new Exception("Finalizando. Cupom fiscal não foi totalizado !");
            }

            FuncoesNFC.verificarGerenciadorNFCe();

            //Obtendo os dados do Consumidor apartir da ECF
            //var consumidorECF = DadosConsumidor(ConfiguracoesECF.idECF);
            // Obtendo Informações do ECF;             

            var dataHoraCupomECF = FuncoesECF.DataHoraUltDocumentoECF().ToString();


            string numeroCupomECF = "";
            string numeroECF = "";
            decimal totalIcmsCupom = 0;
            string contadorCupomECF = "";
            decimal totalLiquidoCupomECF = 0;
            string contadorDebitoCreditoECF = "";
            string contadorGNF = "";
            string COOGNF = "";

            if (!PED)
            {                
                System.Threading.Thread.Sleep(100);                
                numeroCupomECF = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).ToString(); //COO
                numeroECF = FuncoesECF.NumeroECF(ConfiguracoesECF.idECF).ToString();
                contadorCupomECF = FuncoesECF.CCFContadorCupomECF().ToString(); //CCF                                
                totalLiquidoCupomECF = Convert.ToDecimal(FuncoesECF.TotalLiquidoCupomECF());
                totalIcmsCupom = FuncoesECF.TotalICMSCupomECF();
                contadorDebitoCreditoECF = FuncoesECF.ContadorDebitoCredidoCDC();
                contadorGNF = FuncoesECF.ContadorNaoFiscalGNF();
                COOGNF = numeroCupomECF;                
            }

            if (PED)
            {
                // Para nao gravar informações do ECF quando for PED Lancamento manual
                serieNF = "D";
                subserieNF = "1";
                //dataHoraCupomECF = DateTime.Now.TimeOfDay.ToString();
                dataHoraCupomECF = FuncoesECF.DataHoraDoECF(ConfiguracoesECF.idECFHoraDivergente).ToString();
                modDocFiscal = "02";
                numeroCupomECF = "";
                numeroECF = "";
                totalIcmsCupom = 0;
                contadorCupomECF = "";
                totalLiquidoCupomECF = 0;
                contadorDebitoCreditoECF = "";
                contadorGNF = "";
                COOGNF = "";
            }


            decimal rateioDescItens = 0;
            decimal valorServicos = 0, descontoServicos = 0;
            int parcelas = 0;
            decimal valorDH = 0, valorCA = 0,valorPF=0, valorCR = 0, valorCH = 0, ValorTI = 0;
            decimal custoItens = 0;
            string nomeCliente = "";

            ///Sumary
            ///Verifica a Conexão antes de finalizar
            ///Se não tiver conectado com o Servidor então usa local para o PAF
            ///

             //Conexao.VerificaConexaoDB();


            #region On-Line
            //if (Conexao.onLine)
            //{

                siceEntities entidade = Conexao.CriarEntidade();

                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                /// Obtendo Valores de Pagamento e Parcelamentos
                /// 
                var pagamentos = from p in entidade.caixas
                                 where p.EnderecoIP == GlbVariaveis.glb_IP
                                 select p;

                foreach (var item in pagamentos)
                {
                    switch (item.tipopagamento)
                    {
                        case "CR":
                            parcelas += 1;
                            valorCR += item.valor;
                            nomeCliente = item.nome;
                            break;
                        case "CA":
                            valorCA += item.valor;
                            break;
                        case "DH":
                            valorDH += item.valor;
                            break;
                        case "CH":
                            valorCH += item.valor;
                            break;
                        case "TI":
                            ValorTI += item.valor;
                            break;
                    case "CP:":
                        case "PF":
                            valorPF += item.valor;
                            break;

                    };
                };

                

                ///Obtendo valores rateiodesconto
                ///
                var v = (from p in entidade.vendas
                         where p.id == GlbVariaveis.glb_IP
                         && p.cancelado == "N"
                         select p).Count();
                if (v == 0) v = 1;
                rateioDescItens = this.desconto / v;

                /// Obtendo valores dos itens da Venda
                /// 
                var somaCustos = (from p in entidade.vendas
                         where p.id == GlbVariaveis.glb_IP
                         && p.cancelado == "N"
                         select (p.quantidade * p.custo)).Sum();

                custoItens = somaCustos.Value;

                /// Obtendo valores dos Servicos
                /// 
                try
                {
                    var servico = (from p in entidade.vendas
                                   where p.id == GlbVariaveis.glb_IP
                                    && p.tipo == "1 - Servico"
                                    && p.cancelado == "N"
                                   group p by p.id into g
                                   select new { total = g.Sum(p => p.total), desconto = g.Sum(p => p.descontovalor + p.ratdesc) }).ToList();

                    foreach (var soma in servico)
                    {
                        valorServicos = soma.total;
                        descontoServicos = soma.desconto;
                    }
                }
                catch (Exception erro)
                {
                    MessageBox.Show(erro.ToString());
                }

                // Rateio de Desconto e Acréscimo
                #region rateio

                if (desconto>0 || encargos>0)
                {
                    var idItem = (from n in entidade.vendas
                                      where n.id == GlbVariaveis.glb_IP
                                      && n.cancelado == "N"
                                      select n.inc);

                    foreach (var item in idItem)
                    {
                        siceEntities entidadeRat = Conexao.CriarEntidade();

                        if (Conexao.tipoConexao == 2)
                            entidadeRat = Conexao.CriarEntidade(false);

                        var dadosItem = (from n in entidadeRat.vendas
                                        where n.inc == item
                                        select n).First();
                        if (desconto > 0)
                        {
                            var ratDesconto =(dadosItem.total / valorBruto )*desconto;
                            ratDesconto = Math.Truncate(ratDesconto * 100) / 100;
                            dadosItem.ratdesc = ratDesconto;
                            entidadeRat.SaveChanges();
                        }

                        if (encargos > 0)
                        {
                            var ratEncargos = (dadosItem.total / valorBruto) * encargos;
                            ratEncargos = Math.Truncate(ratEncargos * 100) / 100;
                            dadosItem.rateioencargos = ratEncargos;
                            entidadeRat.SaveChanges();
                        }

                    }

                }
                #endregion
                // Finalizando a Pré-Venda
                #region Pre-venda
                if (ConfiguracoesECF.prevenda == true || gerarPreVenda == true)
                {
                    long seqPrevenda = 0;
                    int seqFilial = 0;
                    //ObterSequencial(ref seqFilial, "PRE");

                    try
                    {
                        contprevendaspaf prevenda = new contprevendaspaf();
                        prevenda.codigofilial = GlbVariaveis.glb_filial;
                        prevenda.enderecoip = GlbVariaveis.glb_IP;
                        prevenda.data = GlbVariaveis.Sys_Data.Date;
                        prevenda.operador = GlbVariaveis.glb_Usuario;
                        prevenda.cliente = nomeCliente != null ? nomeCliente : dadosConsumidor.nomeConsumidor;
                        prevenda.ecfCPFCNPJconsumidor = dadosConsumidor.cpfCnpjConsumidor;
                        prevenda.codigocliente = idCliente;
                        prevenda.valor = this.valorBruto - this.desconto;
                        prevenda.vendedor = vendedor;
                        prevenda.desconto = this.desconto;
                        prevenda.cancelada = "N";
                        //Dados Adicionais
                        prevenda.responsavelreceber = dadosEntrega.recebedor ?? "";
                        prevenda.enderecoentrega = dadosEntrega.endereco ?? "";
                        prevenda.cepentrega = dadosEntrega.cep ?? "";
                        prevenda.bairroentrega = dadosEntrega.bairro ?? "";
                        prevenda.numeroentrega = dadosEntrega.numero ?? "";
                        prevenda.cidadeentrega = dadosEntrega.cidade ?? "";
                        prevenda.estadoentrega = dadosEntrega.estado ?? "";
                        prevenda.horaentrega = dadosEntrega.hora.TimeOfDay == null ? Convert.ToDateTime("00:00:00").TimeOfDay : dadosEntrega.hora.TimeOfDay;
                        prevenda.observacao = dadosEntrega.observacao ?? "";                        
                        //Dados OS
                        prevenda.osnrfabricacao = dadosDAVOS.nrfabricacao ?? "";
                        prevenda.marca = dadosDAVOS.marca ?? "";
                        prevenda.modelo = dadosDAVOS.modelo ?? "";
                        prevenda.ano = dadosDAVOS.anoFabricacao + 0;
                        prevenda.placa = dadosDAVOS.placa ?? "";
                        prevenda.renavam = dadosDAVOS.renavam ?? "";

                        prevenda.ncupomfiscal = "0";
                        prevenda.finalizada = "N";
                        prevenda.origem = "";
                        prevenda.devolucao = numeroDevolucao;
                        prevenda.classe = classeVenda;
                        prevenda.encargos = encargos;
                        prevenda.numeroDAVFilial = seqFilial;
                        prevenda.contadorRGECF = " ";
                        prevenda.marcado = "N";
                        entidade.AddTocontprevendaspaf(prevenda);
                        entidade.SaveChanges();
                        // Obtendo a sequencia da Pré-venda
                        //seqPrevenda = (from p in entidade.contprevendaspaf
                        //               where p.enderecoip == GlbVariaveis.glb_IP
                        //               && p.codigofilial == GlbVariaveis.glb_filial
                        //               select p.numeroDAVFilial).Max();

                        ObterSequencial(ref seqPrevenda, "PRE");
                    }
                    catch (Exception erro)
                    {
                        throw new Exception("Não foi possível gerar a pré-venda. " + erro.InnerException.ToString());
                    }


                    using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                    {
                        try
                        {
                            conn.Open();
                            EntityCommand cmd = conn.CreateCommand();
                            cmd.CommandTimeout = 3600;
                            cmd.CommandText = "siceEntities.FinalizarPreVenda";
                            cmd.CommandType = CommandType.StoredProcedure;

                            EntityParameter doc = cmd.Parameters.Add("preVendaNumero", DbType.Int32);
                            doc.Direction = ParameterDirection.Input;
                            doc.Value = seqPrevenda;

                            EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                            filial.Direction = ParameterDirection.Input;
                            filial.Value = GlbVariaveis.glb_filial;

                            EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                            ip.Direction = ParameterDirection.Input;
                            ip.Value = GlbVariaveis.glb_IP;
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception erro)
                        {
                            throw new Exception("Não foi possível gerar a pré-venda. " + erro.InnerException.ToString());
                        }
                        finally
                        {
                            Funcoes.TravarTeclado(false);
                        }


                    }
                    if (ConfiguracoesECF.prevenda)
                    {
                        frmOperadorTEF operador = new frmOperadorTEF("PV" + Funcoes.FormatarZerosEsquerda(seqPrevenda, 10, false), false);
                        operador.ShowDialog();
                        operador.Dispose();
                    };
                    return int.Parse(seqPrevenda.ToString());
                }
                // Fim da Pre-Venda
                #endregion

                // Finalizando o DAV
                #region DAV
                if ((ConfiguracoesECF.davporImpressoraNaoFiscal == true || ConfiguracoesECF.davPorECF) || gerarDAV == true)
                {
                    string procedure = "siceEntities.FinalizarDAV";
                    long seqDAV = 0;
                    int seqFilial = 0;
                    try
                    {
                        if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Geral.ToString())
                        {
                            FrmMsgOperador msg = new FrmMsgOperador("", "Processando DAV");
                            msg.Show();
                            Application.DoEvents();

                            //ObterSequencial(ref seqFilial, "DAV");
                            contdav dav = new contdav();
                            dav.codigofilial = GlbVariaveis.glb_filial;
                            dav.enderecoip = GlbVariaveis.glb_IP;
                            dav.data = GlbVariaveis.Sys_Data.Date;
                            dav.operador = GlbVariaveis.glb_Usuario;
                            dav.cliente = nomeCliente.Trim() != "" ? nomeCliente : dadosConsumidor.nomeConsumidor;

                            /*if (_pdv.identificarClienteNFCe == true)
                            {
                                dav.ecfCPFCNPJconsumidor = dadosConsumidor.ecfCNPJCPFConsumidor;
                            }*/

                            dav.codigocliente = idCliente;
                            dav.valor = this.valorBruto - this.desconto;
                            dav.vendedor = vendedor;
                            dav.desconto = this.desconto;
                            dav.cancelada = "N";
                            

                            //Dados Adicionais
                            dav.responsavelreceber = dadosEntrega.recebedor ?? "";
                            dav.enderecoentrega = dadosEntrega.endereco ?? "";
                            dav.cepentrega = dadosEntrega.cep ?? "";
                            dav.bairroentrega = dadosEntrega.bairro ?? "";
                            dav.numeroentrega = dadosEntrega.numero ?? "";
                            dav.cidadeentrega = dadosEntrega.cidade ?? "";
                            dav.estadoentrega = dadosEntrega.estado ?? "";
                            dav.horaentrega = dadosEntrega.hora.TimeOfDay == null ? Convert.ToDateTime("00:00:00").TimeOfDay : dadosEntrega.hora.TimeOfDay;
                            dav.observacao = dadosEntrega.observacao ?? "";

                            //Dados OS
                            dav.osnrfabricacao = dadosDAVOS.nrfabricacao ?? "";
                            dav.marca = dadosDAVOS.marca ?? "";
                            dav.modelo = dadosDAVOS.modelo ?? "";
                            dav.ano = dadosDAVOS.anoFabricacao + 0;
                            dav.placa = dadosDAVOS.placa ?? "";
                            dav.renavam = dadosDAVOS.renavam ?? "";


                            dav.ncupomfiscal = "0";
                            dav.finalizada = "N";
                            dav.origem = "";
                            dav.classe = classeVenda;
                            dav.encargos = encargos;
                            dav.devolucao = numeroDevolucao;
                            dav.numeroDAVFilial = seqFilial;
                            dav.numeroECF = "001";
                            dav.contadorRGECF = " ";
                            dav.marcado = "N";
                            dav.origem = "SICEpdv";

                            if (!string.IsNullOrWhiteSpace(Venda.IQCard) && !string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                            {
                                dav.cartaofidelidade = Venda.IQCard;
                            };


                            if (ConfiguracoesECF.perfil == "Y")
                            {
                                FuncoesECF fecf = new FuncoesECF();
                                dav.marca = ConfiguracoesECF.marcaECF;
                                dav.numeroECF = ConfiguracoesECF.numeroECF;
                                dav.modelo = ConfiguracoesECF.modeloECF;                                
                                dav.ecffabricacao = ConfiguracoesECF.nrFabricacaoECF;                                
                            }

                            entidade.AddTocontdav(dav);
                            entidade.SaveChanges();

                            
                            // Obtendo a sequencia do DAV
                            
                            ObterSequencial(ref seqDAV, "DAV");

                            //seqDAV = int.Parse(numerodav.FirstOrDefault());
                            if(_pdv.identificarClienteNFCe == true)
                                Conexao.CriarEntidade().ExecuteStoreCommand("update contdav set ecfConsumidor = '"+ dadosConsumidor.ecfConsumidor + "', ecfCPFCNPJconsumidor = '"+ dadosConsumidor.ecfCNPJCPFConsumidor + "' where numerodavfilial = '" + seqDAV + "' ");

                            System.Threading.Thread.Sleep(100);

                            /*seqDAV = (from p in entidade.contdav
                                      where p.enderecoip == GlbVariaveis.glb_IP
                                      && p.codigofilial == GlbVariaveis.glb_filial
                                      select p.numeroDAVFilial).Max();*/

                            msg.Dispose();
                        };

                        // DAV Serviço
                        if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                        {
                            procedure = "siceEntities.FinalizarDAVOS";
                            //ObterSequencial(ref seqFilial, "DAVOS");
                            contdavos dav = new contdavos();
                            dav.codigofilial = GlbVariaveis.glb_filial;
                            dav.enderecoip = GlbVariaveis.glb_IP;
                            dav.data = GlbVariaveis.Sys_Data.Date;
                            dav.operador = GlbVariaveis.glb_Usuario;
                            dav.cliente = nomeCliente.Trim() != "" ? nomeCliente : dadosConsumidor.nomeConsumidor;
                            dav.ecfCPFCNPJconsumidor = dadosConsumidor.cpfCnpjConsumidor;
                            dav.codigocliente = idCliente;
                            dav.valor = this.valorBruto - this.desconto;
                            dav.vendedor = vendedor;
                            dav.desconto = this.desconto;
                            dav.cancelada = "N";
                            

                            //Dados Adicionais
                            dav.responsavelreceber = dadosEntrega.recebedor ?? "";
                            dav.enderecoentrega = dadosEntrega.endereco ?? "";
                            dav.cepentrega = dadosEntrega.cep ?? "";
                            dav.bairroentrega = dadosEntrega.bairro ?? "";
                            dav.numeroentrega = dadosEntrega.numero ?? "";
                            dav.cidadeentrega = dadosEntrega.cidade ?? "";
                            dav.estadoentrega = dadosEntrega.estado ?? "";
                            dav.horaentrega = dadosEntrega.hora.TimeOfDay == null ? Convert.ToDateTime("00:00:00").TimeOfDay : dadosEntrega.hora.TimeOfDay;
                            dav.observacao = dadosEntrega.observacao ?? "";

                            //Dados OS
                            dav.osnrfabricacao = dadosDAVOS.nrfabricacao ?? "";
                            dav.marca = dadosDAVOS.marca ?? "";
                            dav.modelo = dadosDAVOS.modelo ?? "";
                            dav.ano = dadosDAVOS.anoFabricacao + 0;
                            dav.placa = dadosDAVOS.placa ?? "";
                            dav.renavam = dadosDAVOS.renavam ?? "";


                            dav.ncupomfiscal = "0";
                            dav.finalizada = "N";
                            dav.origem = "";
                            dav.classe = classeVenda;
                            dav.encargos = encargos;
                            dav.devolucao = numeroDevolucao;
                            dav.numeroDAVFilial = seqFilial;
                            dav.numeroECF = "001";
                            dav.contadorRGECF = " ";
                            entidade.AddTocontdavos(dav);
                            entidade.SaveChanges();
                            // Obtendo a sequencia do DAV
                            ObterSequencial(ref seqDAV, "DAVOS");
                        };

                    }
                    catch (Exception erro)
                    {
                        throw new Exception("Não foi possível gerar o DAV: " + erro.InnerException.ToString());
                    }
                    finally
                    {
                        Funcoes.TravarTeclado(false);
                    }

                    using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                    {
                        try
                        {

                            /*conn.Open();
                            EntityCommand cmd = conn.CreateCommand();
                            cmd.CommandTimeout = 3600;
                            cmd.CommandText = procedure; // "siceEntities.FinalizarDAV";
                            cmd.CommandType = CommandType.StoredProcedure;

                            EntityParameter doc = cmd.Parameters.Add("DAVNumero", DbType.Int32);
                            doc.Direction = ParameterDirection.Input;
                            doc.Value = seqDAV;

                            EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                            filial.Direction = ParameterDirection.Input;
                            filial.Value = GlbVariaveis.glb_filial;

                            EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                            ip.Direction = ParameterDirection.Input;
                            ip.Value = GlbVariaveis.glb_IP;
                            cmd.ExecuteNonQuery();*/

                            string SQL = "START TRANSACTION;" +
                                         "CALL finalizarDAV('"+ seqDAV + "','"+ GlbVariaveis.glb_filial + "','"+ GlbVariaveis.glb_IP + "');" +
                                         "COMMIT";
                            Conexao.CriarEntidade().ExecuteStoreCommand(SQL);

                        }
                        catch (Exception erro)
                        {
                            throw new Exception("Não foi possível gerar o DAV: " + erro.InnerException.ToString());
                        }
                        finally
                        {
                            Funcoes.TravarTeclado(false);
                        }


                    }
                   
                                         
                        if (seqDAV>0 && ConfiguracoesECF.perfil=="Y")
                        {
                            ImprimirDAV(seqDAV);
                            // Aqui para gravar o COO depois da impressao;                                                      
                            string sql = "UPDATE contdav SET contadorRGECF=IFNULL( (SELECT coo FROM contrelatoriogerencial WHERE inc=(SELECT MAX(inc) FROM contrelatoriogerencial)),' ') WHERE numeroDAVFilial = '" + seqDAV.ToString() + "'";
                            Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                            sql ="UPDATE contdav SET EADRegistroDAV=MD5(CONCAT(ncupomfiscal,numeroDAVFilial,DATA,valor,IFNULL(numeroECF,'001'),IFNULL(contadorRGECF,''),IFNULL(cliente,''),IFNULL(ecfCPFCNPJconsumidor,'')))"+
                            "WHERE numeroDAVFilial='" + seqDAV.ToString() + "'";
                            Conexao.CriarEntidade().ExecuteStoreCommand(sql);                           
                        }

                        //if (MessageBox.Show("Imprimir DAV ?", "SICEpdv", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        //{

                        if ((ConfiguracoesECF.davporImpressoraNaoFiscal || ConfiguracoesECF.davPorECF) && ConfiguracoesECF.perfil!="Y")
                        {
                            frmOperadorTEF.EnviarIQCARD = true;
                            frmOperadorTEF.davNumero = seqDAV;
                            frmOperadorTEF operador = new frmOperadorTEF("DAV: " + Funcoes.FormatarZerosEsquerda(seqDAV, 10, false), false);
                            operador.mostarIQCARD = true;
                            operador.ShowDialog();
                            operador.Dispose();

                            FrmMsgOperador msg = new FrmMsgOperador("", "Gerando DAV");
                            msg.Show();
                            Application.DoEvents();
                            try
                            {
                                if (ConfiguracoesECF.davImpressaoCarta == false)
                                {
                                    DialogResult result = MessageBox.Show("Imprimir DAV?", "Atenção", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                                    if(result == DialogResult.OK)
                                           this.ImprimirDAV(seqDAV);
                                }
                                else
                                {
                                    // FrmCupomDAV cupom = new FrmCupomDAV(Convert.ToInt32(seqDAV));
                                    ImpressaoDAV cupom = new ImpressaoDAV(Convert.ToInt32(seqDAV));
                                    cupom.ShowDialog();
                                    cupom.Dispose();
                                }
                               
                            }
                            catch (Exception erro)
                            {
                                MessageBox.Show(erro.Message);
                            }
                            finally
                            {
                                msg.Dispose();
                            }
                        }

                        

                        //}

                    
                    return int.Parse(seqDAV.ToString());
                }
                // Fim do DAV
                #endregion

                // Por causa do EAD que não faz se tiver null
                // e se tiver GNF para voltar o numero do COO do Cupom senao pega o 
                // COO do comprovante nao fiscal
                if (contadorGNF == null)
                    contadorGNF = "";

             
                contadorGNF = contadorGNF.Trim();

                if (valorCA == 0 || ConfiguracoesECF.NFC == true)
                {
                    contadorGNF = "";
                    contadorDebitoCreditoECF = "";
                    COOGNF = "";
                }

                if (!ConfiguracoesECF.tefDedicado && !ConfiguracoesECF.tefDiscado)
                    contadorGNF = "";


                if (contadorGNF.Trim() != "" )
                {
                    numeroCupomECF = Funcoes.FormatarZerosEsquerda(Convert.ToInt32(numeroCupomECF) - 1, 6, false);
                }

            //} // If StandAlone
            #endregion

            #region StandAlone
            /*if (GlbVariaveis.glb_standalone == true && !Conexao.onLine && Conexao.tipoConexao == 1)
            {
                    return FinalizarVendaOff(dataHoraCupomECF, numeroCupomECF, numeroECF, totalIcmsCupom, contadorCupomECF, totalLiquidoCupomECF, contadorDebitoCreditoECF, ref rateioDescItens, ref valorServicos, ref descontoServicos, custoItens);
            }*/
            #endregion standalone
        


            try
            {                
                string tipoPagamento = "";

                entidade = Conexao.CriarEntidade();
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                try
                {
                     tipoPagamento = (from n in entidade.caixas
                                            where n.EnderecoIP == GlbVariaveis.glb_IP
                                            select n.tipopagamento).FirstOrDefault();

                    if (VerificarPagamentos == true)
                    {
                        if (tipoPagamento == null || string.IsNullOrEmpty(tipoPagamento))
                            throw new Exception("Não foi encontrado uma forma de pagamento correspondente");
                    }
                }
                catch(Exception erro)
                {
                    //Conexao.VerificaConexaoDB();
                    //if (GlbVariaveis.glb_standalone == true && !Conexao.onLine && Conexao.tipoConexao == 1)
                    //{
                    //return FinalizarVendaOff(dataHoraCupomECF, numeroCupomECF, numeroECF, totalIcmsCupom, contadorCupomECF, totalLiquidoCupomECF, contadorDebitoCreditoECF, ref rateioDescItens, ref valorServicos, ref descontoServicos, custoItens);
                    //}
                    throw new Exception(erro.ToString());
                }

                contdocs cupom = new contdocs();

                FrmMsgOperador msg3 = new FrmMsgOperador("", "Gerando Documento da base local");
                msg3.Show();
                Application.DoEvents();

                try
                {
                    #region
                    /*
                    if (ConfiguracoesECF.idNFC == 1 && ConfiguracoesECF.NFC == true)
                    {                        
                        try
                        {

                            FrmMsgOperador msg = new FrmMsgOperador("", "Gerando NFC-e");
                            msg.Show();
                            Application.DoEvents();

                            FuncoesNFC NFCe = new FuncoesNFC();
                            NFCe.pagamentos = (from p in entidade.caixas
                                               where p.EnderecoIP == GlbVariaveis.glb_IP
                                               select p).ToList();

                            NFCe.itens = ((from p in entidade.vendas
                                           where p.id == GlbVariaveis.glb_IP
                                            && p.cancelado == "N"
                                           select p).ToList());

                            NFCe.descontoVenda = this.desconto;
                            NFCe.encargosVenda = encargos;
                            NFCe.serie = ConfiguracoesECF.NFCserie;
                            NFCe.numeroNFCe = NFCe.LerSequenciaNFCGuardada();

                            msg.Dispose();

                            FrmMsgOperador msg2 = new FrmMsgOperador("", "Autorizando NFCe");
                            msg2.Show();
                            Application.DoEvents();

                            NFCe.FinalizarVenda();

                            msg2.Dispose();

                            numeroCupomECF = ConfiguracoesECF.NFCSequencia.ToString().PadLeft(6,'0');
                            contadorCupomECF = ConfiguracoesECF.NFCserie.PadLeft(6, '0');
                            modDocFiscal = "65";
                            chaveNFC = NFCe.informacaoUltimaNFCe("3");
                            protocolo = NFCe.informacaoUltimaNFCe("4");
                            dataAutorizacao = DateTime.Parse(NFCe.informacaoUltimaNFCe("6") == "" ? DateTime.Now.TimeOfDay.ToString() : NFCe.informacaoUltimaNFCe("6"));

                            ObterSequencialNFC();
                            NFCe.GuardarSequenciaNFC(ConfiguracoesECF.NFCSequencia.ToString());
                                
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);                                                      
                        }
                    }
                    */
                    #endregion

                    if (this.numeroDAV > 0)
                    {
                        string ecfconsumidor = Conexao.CriarEntidade().ExecuteStoreQuery<string>("select ifnull(ecfConsumidor,'') from contdav where numerodavfilial = '" + this.numeroDAV + "' and codigoFilial = '" + GlbVariaveis.glb_filial + "'").FirstOrDefault();
                        string ecfCPFCNPJConsumidor = Conexao.CriarEntidade().ExecuteStoreQuery<string>("select ifnull(ecfCPFCNPJconsumidor,'') from contdav where numerodavfilial = '" + this.numeroDAV + "' and codigoFilial = '" + GlbVariaveis.glb_filial + "'").FirstOrDefault();
                        if (ecfconsumidor.Trim() != "")
                            _pdv.identificarClienteNFCe = true;

                        Venda.dadosConsumidor =
                               new DadosConsumidorCupom
                               {
                                   cpfCnpjConsumidor = ecfCPFCNPJConsumidor,
                                   nomeConsumidor = ecfconsumidor,
                                   endConsumidor = "",
                                   endNumero = "",
                                   endBairro = "",
                                   endCEP = "",
                                   endCidade = "",
                                   endEstado = "",
                                   idConsumidor = "",
                                   ecfCNPJCPFConsumidor = ecfCPFCNPJConsumidor,
                                   ecfConsumidor = ecfconsumidor
                               };

                    }

                    nomeCliente = Venda.dadosConsumidor.nomeConsumidor;

                    if (nomeCliente == null || nomeCliente == "" && idCliente > 0)
                    {
                        nomeCliente = (from e in entidade.clientes
                                       where e.Codigo == idCliente
                                       select e.Nome).FirstOrDefault();
                    }

                    if (idCliente == 0)
                        nomeCliente = "";
                   

                    cupom.ip = GlbVariaveis.glb_IP;
                    cupom.CodigoFilial = GlbVariaveis.glb_filial;
                    cupom.operador = GlbVariaveis.glb_Usuario;
                    cupom.data = dataHoraCupomECF == null ? DateTime.Now.Date : Convert.ToDateTime(dataHoraCupomECF).Date;
                    if (cupom.data == Convert.ToDateTime("01/01/0001"))
                        cupom.data = GlbVariaveis.Sys_Data;

                    if (cupom.data.Value == Convert.ToDateTime("01/01/0001"))
                        cupom.data = GlbVariaveis.Sys_Data;

                    cupom.dataexe = GlbVariaveis.Sys_Data;
                    cupom.Totalbruto = this.valorBruto;
                    cupom.desconto = this.desconto;
                    cupom.encargos = (encargos + _pdv.taxaServicoIqChef);
                    cupom.total = this.valorBruto + this.encargos - (this.desconto + this.totalDevolucao);
                    cupom.codigocliente = idCliente;
                    cupom.nome = nomeCliente;
                    cupom.dependente = dependente;
                    cupom.NrParcelas = parcelas;
                    cupom.vendedor = vendedor;
                    cupom.operador = GlbVariaveis.glb_Usuario;
                    cupom.dpfinanceiro = dpFinanceiro ?? "Venda";
                    cupom.id = GlbVariaveis.glb_IP;
                    cupom.custos = custoItens;
                    cupom.valorservicos = valorServicos;
                    cupom.descontoservicos = descontoServicos;
                    cupom.bordero = "S";
                    cupom.cartaofidelidade = Venda.IQCard == null?"":Venda.IQCard;
                    cupom.concluido = "N";
                    cupom.entregaconcluida = "N";
                    cupom.estadoentrega = "  ";
                    cupom.estornado = "N";
                    cupom.NF_e = "N";

                    //Dados Adicionais
                    cupom.responsavelreceber = dadosEntrega.recebedor ?? "";
                    cupom.enderecoentrega = dadosEntrega.endereco ?? "";
                    cupom.cepentrega = dadosEntrega.cep ?? "";
                    cupom.bairroentrega = dadosEntrega.bairro ?? "";
                    cupom.numeroentrega = dadosEntrega.numero ?? "";
                    cupom.cidadeentrega = dadosEntrega.cidade ?? "";
                    cupom.estadoentrega = dadosEntrega.estado ?? "";
                    cupom.horaentrega = dadosEntrega.hora.TimeOfDay == null ? Convert.ToDateTime("00:00:00").TimeOfDay : dadosEntrega.hora.TimeOfDay;
                    cupom.Observacao = dadosEntrega.observacao ?? "";
                    cupom.obsentrega = dadosEntrega.observacao ?? "";

                    if (RetornoTEF != "")
                        cupom.Observacao = RetornoTEF;

                    cupom.romaneio = "N";
                    cupom.classe = classeVenda ?? " ";
                    cupom.hora = dataHoraCupomECF == null ? DateTime.Now.TimeOfDay : Convert.ToDateTime(dataHoraCupomECF).TimeOfDay;
                    if ((valorCA + ValorTI) > 0)
                    {
                        cupom.TEF = "S";
                        cupom.tipopagamentoECF = "CC";
                    }
                    else
                    {
                        cupom.TEF = "N";
                        cupom.tipopagamentoECF = "";
                    }

                    cupom.tipopagamento = tipoPagamento ?? "RV";
                    //Dados do ECF
                    cupom.ncupomfiscal = numeroCupomECF;
                    cupom.ecfnumero = numeroECF == null ? "001" : numeroECF;
                    cupom.ecfcontadorcupomfiscal = ConfiguracoesECF.NFCserie;
                    if(dadosConsumidor.ecfCNPJCPFConsumidor != null && dadosConsumidor.ecfCNPJCPFConsumidor.Length > 0 && _pdv.identificarClienteNFCe == true)
                    // (_pdv.identificarClienteNFCe == true)
                    {
                        cupom.ecfConsumidor = dadosConsumidor.ecfConsumidor.PadRight(30, ' ').Substring(0, 30) ?? " ";// consumidorECF.Nome;
                        cupom.ecfCPFCNPJconsumidor = dadosConsumidor.ecfCNPJCPFConsumidor ?? " "; // consumidorECF.CpfCgc;                
                        cupom.ecfEndConsumidor = dadosConsumidor.endConsumidor ?? " ";// consumidorECF.Endereco;
                    }
                    else
                    {
                        cupom.ecfConsumidor = "";// consumidorECF.Nome;
                        cupom.ecfCPFCNPJconsumidor = ""; // consumidorECF.CpfCgc;                
                        cupom.ecfEndConsumidor = "";// consumidorECF.Endereco;
                    }
                    cupom.ecftotalliquido = totalLiquidoCupomECF;
                    cupom.prevendanumero = numeroPreVenda.ToString();
                    cupom.contadordebitocreditoCDC = contadorDebitoCreditoECF;
                    cupom.totalICMScupomfiscal = totalIcmsCupom;
                    cupom.troco = troco;
                    cupom.davnumero = this.numeroDAV;
                    cupom.ecffabricacao = ConfiguracoesECF.nrFabricacaoECF;
                    cupom.ecfMFadicional = ConfiguracoesECF.mfAdicionalECF;
                    cupom.ecftipo = ConfiguracoesECF.tipoECF;
                    cupom.ecfmarca = ConfiguracoesECF.marcaECF == null ? "" : ConfiguracoesECF.marcaECF;
                    cupom.ecfmodelo = ConfiguracoesECF.modeloECF;
                    cupom.estoqueatualizado = "S";
                    //Dados Entrega
                    if (!string.IsNullOrEmpty(dadosEntrega.recebedor))
                    {
                        cupom.responsavelreceber = dadosEntrega.recebedor;
                        cupom.enderecoentrega = dadosEntrega.endereco;
                        cupom.numeroentrega = dadosEntrega.numero;
                        cupom.cepentrega = dadosEntrega.cep;
                        cupom.estadoentrega = dadosEntrega.estado;
                        cupom.cidadeentrega = dadosEntrega.cidade;
                        cupom.data = dadosEntrega.data;
                        cupom.horaentrega = dadosEntrega.hora.TimeOfDay;
                        cupom.Observacao = dadosEntrega.observacao ?? "";
                        cupom.obsentrega = dadosEntrega.observacao ?? "";
                    };
                    cupom.contadornaofiscalGNF = contadorGNF;

                    //se a venda for NFC tem que ser de todo jeito modelo 65  pois em finalizar existe um flag muito importando!
                      if (ConfiguracoesECF.NFC == true)
                        modDocFiscal = "65";

                    if(ConfiguracoesECF.NFC == false && ConfiguracoesECF.idECF == 9999)
                        modDocFiscal = "02";


                    cupom.modeloDOCFiscal = modDocFiscal;

                    // transferido para a SP FinalizarVenda
                    //cupom.EADRegistroDAV = Funcoes.CriptografarMD5(cupom.ncupomfiscal + cupom.davnumero.ToString() + string.Format("{0:yyyy-MM-dd}", cupom.data.Value) + cupom.total.ToString().Replace(",","."));
                    //cupom.EADr06 = Funcoes.CriptografarMD5(cupom.ecffabricacao + cupom.ncupomfiscal + cupom.contadornaofiscalGNF == null ? "":cupom.contadornaofiscalGNF + cupom.contadordebitocreditoCDC + string.Format("{0:yyyy-MM-dd}", cupom.data.Value) + cupom.COOGNF+ cupom.tipopagamento);

                    cupom.serienf = serieNF;
                    cupom.subserienf = subserieNF;

                    cupom.COOGNF = COOGNF;
                    cupom.devolucaonumero = numeroDevolucao;
                    cupom.chaveNFC = chaveNFC;
                    cupom.protocolo = protocolo;
                    //cupom.dataAutorizacao = dataAutorizacao;
                    cupom.numeroServico = 0;



                    if (Venda.numeroPED > 0)
                    {
                        cupom.numeroPED = Venda.numeroPED;
                        string sql = "UPDATE vendas set notafiscal='" + Venda.numeroPED.ToString() + "' WHERE id='" + GlbVariaveis.glb_IP + "'";
                        entidade.ExecuteStoreCommand(sql);
                    }
                    
                    entidade.AddTocontdocs(cupom);
                    entidade.SaveChanges();
                    RetornoTEF = "";

                }
                catch (Exception ex)
                {

                    //MessageBox.Show(ex.ToString());
                    throw new Exception(ex.ToString());

                    //Conexao.VerificaConexaoDB();
                    //if (GlbVariaveis.glb_standalone == true && !Conexao.onLine && Conexao.tipoConexao == 1)
                    //{
                        //return FinalizarVendaOff(dataHoraCupomECF, numeroCupomECF, numeroECF, totalIcmsCupom, contadorCupomECF, totalLiquidoCupomECF, contadorDebitoCreditoECF, ref rateioDescItens, ref valorServicos, ref descontoServicos, custoItens);
                    //}
                    //else
                    //{
                        //entidade.AddTocontdocs(cupom);
                      //  entidade.SaveChanges();
                    //}
                    
                    //IVAN - Renomei essa função VerificaVenda(), para não executar automaticamente. Pois em teste com devolução e acréscimo de juros houve inconsistência.
                    // e foi analisado em banco de dados reais num universo de 800.000 documentos há apenas 24 docs inconsistentes então não se justifica 
                    // executar automaticamente essa função. A SP será mantida para o uso dos técnicos e eventual desfazimento da venda manualmente.
                    //verificaVenda(true,false);
                    msg3.Dispose();
                    return 0;


                }

                
                // Obter o Documento

                if(Conexao.tipoConexao == 1 || Conexao.tipoConexao == 3)
                   documento = Venda.ObterUltimoDocumento(true);
                else
                    documento = Venda.ObterUltimoDocumento(false);

                Venda.ultimoDocumento = documento;

                EntityConnection conn = new EntityConnection(Conexao.stringConexao);

                if (Conexao.tipoConexao == 2 || Conexao.tipoConexao == 3)
                {
                    //conn = new EntityConnection(Conexao.stringConexaoRemoto);
                    //StandAlone objStand = new StandAlone();
                    //objStand.lancarDocumentoSincronia(documento);
                    StandAlone.lancarDocumentoSincronia(documento);
                }

                /*using (conn)
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandTimeout = 3600;
                    cmd.CommandText = "siceEntities.FinalizarVenda";
                    cmd.CommandType = CommandType.StoredProcedure;

                    EntityParameter doc = cmd.Parameters.Add("doc", DbType.Int32);
                    doc.Direction = ParameterDirection.Input;
                    doc.Value = documento;

                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;

                    EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                    ip.Direction = ParameterDirection.Input;
                    ip.Value = GlbVariaveis.glb_IP;

                    EntityParameter nrDevolucao = cmd.Parameters.Add("devolucaoNR", DbType.Int32);
                    nrDevolucao.Direction = ParameterDirection.Input;
                    nrDevolucao.Value = numeroDevolucao;

                    EntityParameter nrPrevenda = cmd.Parameters.Add("preVenda", DbType.Int32);
                    nrPrevenda.Direction = ParameterDirection.Input;
                    nrPrevenda.Value = numeroPreVenda;

                    EntityParameter nrDAV = cmd.Parameters.Add("DAVNumero", DbType.Int32);
                    nrDAV.Direction = ParameterDirection.Input;
                    nrDAV.Value = numeroDAV;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }*/



                string SQL = "START TRANSACTION;" +
                             "CALL FinalizarVenda(" + documento + ",'" + GlbVariaveis.glb_filial + "','" + GlbVariaveis.glb_IP + "'," + numeroDevolucao + "," + numeroPreVenda + "," + numeroDAV + ");" +
                             "COMMIT;";


                siceEntities siceEntidade = Conexao.CriarEntidade();
                siceEntidade.ExecuteStoreCommand(SQL);
                siceEntidade.SaveChanges();

                if (TEF.valorAprovadoTEF > 0)
                {
                    //siceEntities entidade = Conexao.CriarEntidade();
                    if (Conexao.tipoConexao == 2)
                        entidade = Conexao.CriarEntidade(false);

                    try
                    {
                        SQL = "UPDATE movcartoes SET autorizacao = '" + TEF.numeroAutorizacao.Trim() + "' WHERE documento = '" + documento + "' AND codigofilial ='" + GlbVariaveis.glb_filial + "'";
                        entidade.ExecuteStoreCommand(SQL);
                    }
                    catch (Exception erro)
                    {
                        SQL = "UPDATE movcartoes SET autorizacao = '"+documento.ToString()+"' WHERE documento = '" + documento + "' AND codigofilial ='" + GlbVariaveis.glb_filial + "'";
                        entidade.ExecuteStoreCommand(SQL);
                    }
                }


                // Aqui lança os pontos para o IQCard se o cartão foi usado Ação de PayBack
                #region
                try
                {
                    IqCard.pontosIQCARD = 0;
                    IqCard.idRegistroPontosIQCARD = "";

                    if (!string.IsNullOrWhiteSpace(Venda.IQCard) && !string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                    {
                        if (IqCard.VerificarNumeroCartao(Venda.IQCard) == true)
                        {
                            double pontuacao = Convert.ToDouble(this.valorLiquido);

                            ServiceReference1.WSIQPassClient card = new ServiceReference1.WSIQPassClient();


                            if (Configuracoes.pontuacaoCA == "N")
                            {
                                pontuacao = pontuacao - Convert.ToDouble(valorCA);

                            };

                            if (Configuracoes.pontuacaoCR == "N")
                            {
                                pontuacao = pontuacao - Convert.ToDouble(valorCR);

                            };

                            pontuacao = pontuacao - Convert.ToDouble(valorPF);


                            //if (this.valorLiquido > Configuracoes.pontuacaoMaxIQCard)
                            //{
                            //    pontuacao = Configuracoes.pontuacaoMaxIQCard;
                            //}

                            if (Configuracoes.coefecientePontosIQCard > 0 && Configuracoes.valorcomprafidelizacao==0)
                            {
                                pontuacao = pontuacao * Convert.ToDouble(Configuracoes.coefecientePontosIQCard);
                            }

                            // A configuração de pontos de acordo com a compra tem
                            //precedência sobre a config. de percentual
                            if(Configuracoes.qtdpontosfidelizacao>0)
                            {
                                pontuacao = pontuacao *Convert.ToDouble( (Configuracoes.qtdpontosfidelizacao));

                            }

                            if (pontuacao > Configuracoes.pontuacaoMaxIQCard)
                            {
                                pontuacao = Configuracoes.pontuacaoMaxIQCard;
                            }

                            
                            if (pontuacao >= 1)
                            {
                                IqCard.pontosIQCARD = Convert.ToInt32(pontuacao);
                                IqCard.saldoPontos -= Convert.ToInt32(pontuacao);
                                var resultado = card.RegistrarCompra(GlbVariaveis.chavePrivada, Venda.IQCard, GlbVariaveis.glb_chaveIQCard, GlbVariaveis.nomeEmpresa, pontuacao, Convert.ToDouble(this.desconto), pontuacao, documento.ToString());
                                IqCard.idRegistroPontosIQCARD = resultado;
                                Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE contdocs set idpontuacaoIQCARD='" + resultado + "' WHERE documento='" + documento + "'");
                            }
                           

                        }
                    }
                }
                catch (Exception ex)
                {
                    IqCard.saldoInsuficiente = false;
                    // Não alterar saldo insuficiente código de erro retornado do webservice.
                    if (ex.ToString().ToLower().Contains("saldo insuficiente"))
                    {
                        MessageBox.Show("Saldo insuficiente de crédito para lançar pontos para o usuário. Acesse www.iqcard.com.br com sua conta e compre mais créditos");
                        if (ex.ToString().ToLower().Contains("O cliente não acumulou pontos")) 
                        IqCard.saldoInsuficiente = true;
                    }

                    if(File.Exists("debugiqcard.txt"))
                    {
                        MessageBox.Show(ex.Message.ToString());

                    }
                    // renomeado por que é inrelevante 
                    //  MessageBox.Show("deu erro"+ex.Message);
                }
                #endregion
                // Aqui coloca o id da transação


                // Aqui Finaliza o Pedido no APP STore IQCARD
                try
                {
                    if (!string.IsNullOrEmpty(idPedidoIQCARD))
                    {
                        IqCard iqcard = new IqCard();
                        iqcard.MudarStatus("Encerrado",documento.ToString());
                    }

                }
                catch (Exception)
                {
                    
                }

                if (!string.IsNullOrEmpty(Venda.idTransacaoIQCARD))
                {
                    Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE contdocs SET idTransacaoIQCARD='" + Venda.idTransacaoIQCARD + "' WHERE documento='" + documento + "'");                   
                }

                if (!string.IsNullOrEmpty(Venda.idPedidoIQCARD))
                {
                    Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE contdocs SET idTransacaoIQCARD='" + Venda.idPedidoIQCARD + "' WHERE documento='" + documento + "'");
                }

                // Aqui eleiminar o CODE Voucher do controle e grava no doc
                if (!string.IsNullOrEmpty(IqCard.voucherDesconto))
                {
                    try
                    {
                        ServiceReference1.WSIQPassClient voucher = new ServiceReference1.WSIQPassClient();
                        voucher.ApagarEticketTemporarioPontosExtra(GlbVariaveis.chavePrivada, IqCard.voucherDesconto);
                        Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE contdocs SET idvoucherdesconto='" + IqCard.voucherDesconto + "' WHERE documento='" + documento + "'");                        
                        IqCard.voucherDesconto = "";
                    }
                    catch (Exception)
                    {
                    }
                }


                msg3.Dispose();
                //Migrate
                if (ConfiguracoesECF.idNFC == 1 && ConfiguracoesECF.NFC == true)
                {
                    #region
                    try
                    {

                        FrmMsgOperador msg = new FrmMsgOperador("", "Gerando NFC-e");
                        msg.Show();
                        Application.DoEvents();

                        FuncoesNFC NFCe = new FuncoesNFC();
                        NFCe.pagamentos = (from p in entidade.caixa
                                           where p.EnderecoIP == GlbVariaveis.glb_IP && p.documento == documento && p.CodigoFilial == GlbVariaveis.glb_filial
                                           select p).ToList();

                        NFCe.itens = ((from p in entidade.venda
                                       where p.id == GlbVariaveis.glb_IP && p.documento == documento && p.codigofilial == GlbVariaveis.glb_filial
                                        && p.cancelado == "N"
                                       select p).ToList());

                        NFCe.descontoVenda = this.desconto;
                        NFCe.encargosVenda = encargos;
                        NFCe.serie = ConfiguracoesECF.NFCserie;
                        NFCe.numeroNFCe = NFCe.LerSequenciaNFCGuardada();

                        msg.Dispose();

                        FrmMsgOperador msg2 = new FrmMsgOperador("", "Autorizando NFCe (TROCO) R$"+troco);
                        //msg2.StartPosition
                        msg2.Show();
                        Application.DoEvents();

                        NFCe.FinalizarVenda();

                        msg2.Dispose();

                        numeroCupomECF = ConfiguracoesECF.NFCSequencia.ToString().PadLeft(6, '0');
                        contadorCupomECF = ConfiguracoesECF.NFCserie.PadLeft(6, '0');
                        modDocFiscal = "65";
                        chaveNFC = NFCe.informacaoUltimaNFCe("3");
                        protocolo = NFCe.informacaoUltimaNFCe("4");
                        dataAutorizacao = DateTime.Parse(NFCe.informacaoUltimaNFCe("6") == "" ? DateTime.Now.TimeOfDay.ToString() : NFCe.informacaoUltimaNFCe("6"));

                        ObterSequencialNFC();
                        NFCe.GuardarSequenciaNFC(ConfiguracoesECF.NFCSequencia.ToString());

                        var dadosDocumento = (from d in entidade.contdocs
                                              where d.documento == documento && d.id == GlbVariaveis.glb_IP
                                              select d).FirstOrDefault();

                        dadosDocumento.dataAutorizacao = dataAutorizacao;
                        dadosDocumento.protocolo = protocolo;
                        dadosDocumento.chaveNFC = chaveNFC;
                        dadosDocumento.ncupomfiscal = numeroCupomECF;
                        dadosDocumento.modeloDOCFiscal = "65";
                        dadosDocumento.ecfcontadorcupomfiscal = contadorCupomECF;
                        dadosDocumento.ecffabricacao = "Migrate";

                        entidade.SaveChanges();


                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    #endregion
                }

                //SICEGNFCe
                if (ConfiguracoesECF.idNFC == 2 && ConfiguracoesECF.NFC == true)
                {
                    #region
                    FrmMsgOperador msg = new FrmMsgOperador("", "Gerando NFC-e. (Troco R$" + troco + ")");
                    msg.Show();
                    Application.DoEvents();

                    int crt = 1;
                    try
                    {
                        crt = int.Parse(ConfiguracoesECF.NFCcrt);
                    }
                    catch(Exception erro)
                    {

                    }

                    string tipopagamento = "0";
                    contadorCupomECF = ConfiguracoesECF.NFCserie.PadLeft(3, '0');

                    NFe NotaFiscal = new NFe();
                   
                    var ResumoDocumento = (from d in Conexao.CriarEntidade().contdocs
                                           where d.documento == documento
                                           select new
                                           {
                                               codigoCliente = d.codigocliente,
                                               parcelas = d.NrParcelas,
                                               TotalDocumento = d.Totalbruto,
                                               tipopagamento = d.tipopagamento,
                                               DataDocumento = d.data,
                                               Desconto = d.desconto,
                                               nfe = d.nrnotafiscal,
                                               total = d.total,
                                               davnumero = d.davnumero,
                                               ecfconsumidor = d.ecfConsumidor,
                                               ecfcpfCNPJ = d.ecfEndConsumidor,
                                               encargos = d.encargos
                                           }).FirstOrDefault();

                    if (ResumoDocumento.nfe > 0)
                    {
                        if (MessageBox.Show("Já foi emitida NF modelo 55 para este documento. Emitir novamente ?", "Continua", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            msg.Dispose();
                            //return;
                        }
                    }

                    FuncoesNFC NFCe = new FuncoesNFC();

                  
                    if (ResumoDocumento.codigoCliente > 0 && _pdv.identificarClienteNFCe == true)
                    {
                        if (NFCe.validaCliente(ResumoDocumento.codigoCliente.Value) == false)
                        {
                            MessageBox.Show("Dados do Cliente Incompleto");
                        }
                    }

                   

                    NotaFiscal.MontarNotaFiscal(documento,"5.102");
                    NotaFiscal.SalvarItensTabela();


                    //NotaFiscal.numeroNFe = int.Parse(numeroCupomECF);
                    NotaFiscal.modeloNFe = "65";
                    NotaFiscal.cfopNFe = "5.102";
                    NotaFiscal.cfopTransporteNFE = "";
                    NotaFiscal.chaveAcessoRefNFe = "";
                    NotaFiscal.colocarDataHoraNFe = "S";
                    NotaFiscal.criarNF = "S";
                    NotaFiscal.dadosComplementarNFe = FuncoesECF.MensagemCupomECF(ResumoDocumento.total.Value, 0, ResumoDocumento.davnumero.Value, documento); 
                    NotaFiscal.descontoNFe = this.desconto; // não entendo? mas tem que implementar
                    NotaFiscal.despesasNFe = ResumoDocumento.encargos;
                    NotaFiscal.doc = documento;
                    NotaFiscal.especieVolumeNFe = "";
                    NotaFiscal.filial = GlbVariaveis.glb_filial;
                    NotaFiscal.finalidadeNFe = "1";//normal
                    NotaFiscal.freteNFe = 0;
                    NotaFiscal.gerarICMS = "S";
                    NotaFiscal.idInfoComplementarNFe = 0; // pegar id do combox da observação
                    NotaFiscal.idTransportadoraNFe = 0;
                    NotaFiscal.idVeiculoNFe = 0;
                    NotaFiscal.indPag = tipopagamento;
                    NotaFiscal.ipTerminal = GlbVariaveis.glb_IP;
                    NotaFiscal.marcavolume = "";
                    NotaFiscal.naturezaOperacaoNFe = "Venda";
                    NotaFiscal.NFeEntradaAdEstoque = "N";
                    NotaFiscal.NFeOrigem = 0;
                    NotaFiscal.operadorNFe = GlbVariaveis.glb_Usuario;
                    NotaFiscal.qtdVolumeNFe = 0;
                    NotaFiscal.seguroNFe = 0;
                    NotaFiscal.serieNFe = int.Parse((contadorCupomECF.Trim() == "0" || contadorCupomECF.Trim() == null || contadorCupomECF.Trim() == "" || contadorCupomECF.Trim() == "000") ? ConfiguracoesECF.NFCserie : contadorCupomECF.Trim());
                    NotaFiscal.situacaoNFe = "00"; // 00-Documento Regular
                    NotaFiscal.tipoEmissaoNFe = "1"; //1-Normal;
                    NotaFiscal.tipoFreteNFe = "9";//9 - sem frete
                    NotaFiscal.tipoNFe = "1";//1-Saida
                    NotaFiscal.volumeNFe = 0;
                    NotaFiscal.crt = crt;
                    NotaFiscal.TotalPesoBrutoNFe = 0;
                    NotaFiscal.TotalPesoLiquidoNFe = 0;

                    if (_pdv.identificarClienteNFCe == true)
                        NotaFiscal.idCliente = int.Parse(Venda.dadosConsumidor.idConsumidor == "" ? "0" : Venda.dadosConsumidor.idConsumidor);
                    else
                        NotaFiscal.idCliente = 0;




                    //FrmMsgOperador msg1 = new FrmMsgOperador("", "Autorizando NFC-e");
                    try
                    {
                        int nf = NotaFiscal.GerarNFe();

                        SQL = "UPDATE contdocs AS c SET c.ncupomfiscal = '" + nf.ToString() + "', c.ecfcontadorcupomfiscal = '" + contadorCupomECF + "', c.ecffabricacao = 'SICENFCe', c.ecftipo = 'NFCe', c.modeloDOCFiscal = '65' WHERE documento ='" + documento.ToString() + "'";
                        Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                        Conexao.CriarEntidade().SaveChanges();


                        SQL = "UPDATE cbd001 AS c, contdocs AS d " +
                        " SET c.CbdCPF_dest = if(LENGTH(d.ecfCPFCNPJconsumidor) < 12,d.ecfCPFCNPJconsumidor,'')," +
                        " c.CbdCNPJ_dest = if(LENGTH(d.ecfCPFCNPJconsumidor) > 12,d.ecfCPFCNPJconsumidor,'')," +
                        " c.CbdxNome_dest = d.ecfConsumidor," +
                        " c.cbdnro_dest = 'SN'" +
                        " WHERE c.CbdNtfNumero = abs(d.ncupomfiscal)" +
                        " AND c.CbdNtfSerie = abs(d.ecfcontadorcupomfiscal)" +
                        " AND c.CbdCodigoFilial = d.CodigoFilial" +
                        " AND c.Cbdmod = d.modeloDOCFiscal" +
                        " AND d.ecffabricacao = '" + ConfiguracoesECF.nrFabricacaoECF + "'" +
                        " AND c.CbdNtfNumero = '" + nf.ToString() + "'" +
                        " AND c.CbdNtfSerie = '" + int.Parse(contadorCupomECF).ToString() + "'" +
                        " AND d.ecftipo = 'NFCe'" +
                        " AND c.CbdCodigoFilial = '" + GlbVariaveis.glb_filial + "'";


                        Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                        Conexao.CriarEntidade().SaveChanges();

                        msg.Dispose();
                        this.autorizarNota(((contadorCupomECF.Trim() == null || contadorCupomECF.Trim() == "" || contadorCupomECF.Trim() == "0" || contadorCupomECF.Trim() == "000") ? ConfiguracoesECF.NFCserie : contadorCupomECF.Trim()), documento, nf, true, " (Troco R$" + troco.ToString("N2") + ")",true);

                        if (Configuracoes.cfgarquivardados == "S")
                        {
                            try
                            {
                                SQL = "call atualizarNFe('0','0','65','" + GlbVariaveis.glb_filial + "','" + documento.ToString() + "')";
                                string retorno = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                                if (retorno != "1")
                                    MessageBox.Show(retorno.ToString());
                                
                                //Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                                //Conexao.CriarEntidade().SaveChanges();
                            }
                            catch (Exception)
                            {
                                SQL = "call atualizarNFe('0','0','65','" + GlbVariaveis.glb_filial + "','" + documento.ToString() + "','S')";
                                string retorno = Conexao.CriarEntidade().ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                                if (retorno != "1")
                                    MessageBox.Show(retorno.ToString());

                                //Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                                //Conexao.CriarEntidade().SaveChanges();
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Conexao.CriarEntidade().ExecuteStoreCommand("delete from transacoesnfce where ip ='" + GlbVariaveis.glb_IP + "' and codigoFilial = '" + GlbVariaveis.glb_filial + "'");
                        MessageBox.Show("O PDV corrigiu o problema " + GlbVariaveis.glb_Usuario + ". Vamos tentar novamente ?", "Antenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    
                    msg.Dispose();
                        
                    #endregion
                }

                var documentoNFc = (from p in Conexao.CriarEntidade().contdocs
                                 where p.documento == documento && p.CodigoFilial == GlbVariaveis.glb_filial
                                 select p).FirstOrDefault();

                var protocoloNFC = documentoNFc.protocolo;

                int xml = entidade.ExecuteStoreQuery<int>("SELECT COUNT(1) FROM nfe012 WHERE CbdNtfNumero = '" + int.Parse(documentoNFc.ncupomfiscal) + "' AND CbdNtfSerie = '" + int.Parse(documentoNFc.ecfcontadorcupomfiscal)+"' AND cbdcodigofilial = '"+documentoNFc.CodigoFilial+"' AND CbdMod = '65' AND CbdXML IS NULL").FirstOrDefault();

                if(xml > 0)
                {
                    NFe objNFe = new NFe();
                    objNFe.reenviar(int.Parse(documentoNFc.ncupomfiscal), documentoNFc.documento, true);

                    documentoNFc = (from p in Conexao.CriarEntidade().contdocs
                                        where p.documento == documento && p.CodigoFilial == GlbVariaveis.glb_filial
                                        select p).FirstOrDefault();


                    protocoloNFC = documentoNFc.protocolo;
                }

                //imprimir Cupom
                if (((ConfiguracoesECF.idNFC == 1 && ConfiguracoesECF.NFCmodImpressao != "M" && ConfiguracoesECF.NFC == true) ||
                    (ConfiguracoesECF.idNFC == 2 && ConfiguracoesECF.NFC == true)) && (protocoloNFC != "" && protocoloNFC != null && protocoloNFC != "Erro"))
                {
                    #region
                        FrmMsgOperador msg4 = new FrmMsgOperador("", "Gerando DANFE NFCe (Troco R$"+troco.ToString("N2")+")");
                        msg4.Show();

                        Application.DoEvents();
                        FuncoesNFC NFCe = new FuncoesNFC();

                        if (ConfiguracoesECF.NFCtransacaobanco == false && ConfiguracoesECF.NFCmodImpressao != "S")
                        {
                            NFCe.pagamentos = (from p in entidade.caixa
                                               where p.EnderecoIP == GlbVariaveis.glb_IP && p.documento == documento
                                               select p).ToList();

                            NFCe.descontoVenda = this.desconto;

                            NFCe.itens = ((from p in entidade.venda
                                           where p.id == GlbVariaveis.glb_IP && p.documento == documento
                                            && p.cancelado == "N"
                                           select p).ToList());

                        }

                        if (ConfiguracoesECF.NFCtransacaobanco == true)
                            NFCe.imprimirCupom(documento,"PDV",false);
                        

                        msg4.Dispose();
                    #endregion
                }
            }
            catch (EntityException erro)
            {
                MessageBox.Show(erro.ToString());
                Conexao.VerificaConexaoDB();
                    if (GlbVariaveis.glb_standalone == true && !Conexao.onLine && Conexao.tipoConexao == 1)
                    {

                        return FinalizarVendaOff(dataHoraCupomECF, numeroCupomECF, numeroECF, totalIcmsCupom, contadorCupomECF, totalLiquidoCupomECF, contadorDebitoCreditoECF, ref rateioDescItens, ref valorServicos, ref descontoServicos, custoItens);
                    }

                    verificaVenda(true, true);

                throw new Exception("Não foi possível criar o Documento! " + erro.InnerException);
            }
            finally
            {
                Funcoes.TravarTeclado(false);
                Application.DoEvents();
                davAuditoria = numeroDAV;
                numeroDAV = 0;
            }

            // Gravando auditoria
            if (Clientes.limiteUltrapassado)
            {
                entidade = Conexao.CriarEntidade();

                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                decimal? valorUltrapassado = (from n in entidade.clientes
                                             where n.Codigo == idCliente
                                             select n.saldo).FirstOrDefault();

                auditoria nova = new auditoria();
                nova.CodigoFilial = GlbVariaveis.glb_filial;
                nova.data = GlbVariaveis.Sys_Data;
                nova.hora = DateTime.Now.TimeOfDay;
                nova.usuario = GlbVariaveis.glb_Usuario;
                nova.tabela = "Clientes";
                nova.observacao = "Valor acima do crédito : " + string.Format("{0:N2}",valorUltrapassado);
                nova.acao = "Venda Crediario";
                nova.documento = documento;
                nova.local = "SICE.pdv";//nomeCliente;
                nova.codigoproduto = Venda.dadosConsumidor.idConsumidor;
                nova.usuariosolicitante = Operador.ultimoOperadorAutorizado;
                entidade.AddToauditoria(nova);
                entidade.SaveChanges();                
            }

            if (Clientes.restricao)
            {
                entidade = Conexao.CriarEntidade();
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

               string situacao = (from n in entidade.clientes
                         where n.Codigo == idCliente
                         select n.situacao).FirstOrDefault();

                auditoria nova = new auditoria();
                nova.CodigoFilial = GlbVariaveis.glb_filial;
                nova.data = GlbVariaveis.Sys_Data;
                nova.hora = DateTime.Now.TimeOfDay;
                nova.usuario = GlbVariaveis.glb_Usuario;
                nova.usuariosolicitante = Operador.ultimoOperadorAutorizado;
                nova.tabela = "Clientes";
                nova.acao = "Venda Crediario";
                nova.documento = documento;
                nova.local = "SICE.pdv";//nomeCliente;
                nova.codigoproduto = Venda.dadosConsumidor.idConsumidor;
                nova.observacao = "Cliente com Restrição.: "+nomeCliente+" Situacao.:" + situacao ?? " ";
                entidade.AddToauditoria(nova);
                entidade.SaveChanges();
            }

            if (Clientes.inadimplente)
            {
                entidade = Conexao.CriarEntidade();
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                auditoria nova = new auditoria();
                nova.CodigoFilial = GlbVariaveis.glb_filial;
                nova.data = GlbVariaveis.Sys_Data;
                nova.hora = DateTime.Now.TimeOfDay;
                nova.usuario = GlbVariaveis.glb_Usuario;
                nova.usuariosolicitante = Operador.ultimoOperadorAutorizado;
                nova.tabela = "Clientes";
                nova.observacao = "Cliente inadimplente.: "+nomeCliente;
                nova.documento = documento;
                nova.local = "SICE.pdv";
                nova.acao = "Cliente inadimplente";
                nova.codigoproduto = Venda.dadosConsumidor.idConsumidor;
                entidade.AddToauditoria(nova);
                entidade.SaveChanges();
            }

            if(davAuditoria > 0 && _pdv.listFormaPagamentoAuditoria.Count() > 0)
            {
                entidade = Conexao.CriarEntidade();
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                auditoria nova = new auditoria();
                nova.CodigoFilial = GlbVariaveis.glb_filial;
                nova.data = GlbVariaveis.Sys_Data;
                nova.hora = DateTime.Now.TimeOfDay;
                nova.usuario = GlbVariaveis.glb_Usuario;
                nova.usuariosolicitante = Operador.ultimoOperadorAutorizado;
                nova.tabela = "Venda";
                nova.observacao = "Finalizando DAV Alteração de Pagamentos";
                nova.documento = documento;
                nova.local = "SICE.pdv";
                nova.acao = "Finalizando DAV: "+ davAuditoria.ToString()+ " Alteração de Pagamentos. "+ "\r\n";

                foreach(var item in _pdv.listFormaPagamentoAuditoria)
                {
                    nova.acao = nova.acao + " -> Alterado p/  "+item.tipopagamento.ToString() +" / " + item.valor + Environment.NewLine;
                }

                nova.codigoproduto = davAuditoria.ToString();
                entidade.AddToauditoria(nova);
                entidade.SaveChanges();
            }

            ///Zerando variávies
            // Aqui para não deixar na tabela standalone
            if (System.IO.File.Exists("vendas.yap"))
                System.IO.File.Delete("vendas.yap");

            dadosEntrega = new DadosEntrega();
            numeroDevolucao = 0;
            totalDevolucao = 0;
            ultimoDocumento = documento;
            troco = 0;
            RetornoTEF = "";            
            auditoriaDocumento(documento);

            ImprimirComprovanteEntrega(documento);

            StandAlone objStandAlone = new StandAlone();
            if (Conexao.tipoConexao == 2 || Conexao.tipoConexao == 3)
            {
                FrmMsgOperador msg1 = new FrmMsgOperador("", "Sincronizando Dados Com o Servidor!");  
                msg1.Show();
                Application.DoEvents();
                if (Conexao.tipoConexao == 3)
                {
                    DialogResult resp = MessageBox.Show("Deseja Sincronizar documento com a matriz?", "Anteção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resp == DialogResult.Yes)
                    {
                        sincronizada = objStandAlone.Sincronizar(documento);
                    }
                }
                else
                {
                    sincronizada = objStandAlone.Sincronizar(documento);
                }
                msg1.Dispose();
            }
            else
                sincronizada = true;

            documento = Venda.ObterUltimoDocumento(sincronizada);

            if (verificarVenda == true)
                verificaVenda(true, true);



             if (File.Exists(@"log_teste.txt"))
             {
                try
                {
                    string path = System.IO.File.ReadAllText(@"log_teste.txt");

                    if (string.IsNullOrEmpty(path))
                    {
                        path = Environment.CurrentDirectory;
                    };
                    //path += @"\ultimodoc.txt";
                    //File.AppendAllLines(@path, new[] { documento.ToString() });
                    //File.Create(@path);
                    //using (StreamWriter writer = new StreamWriter(@path, true))
                    //{
                        //writer.WriteLine(documento.ToString());
                        //writer.Close();
                    //}

                    //Process myProcess = System.Diagnostics.Process.Start(@"C:\iqsistemas\SICEPDV\ImpressorCupom.exe", " " + documento.ToString());
                    Process myProcess = System.Diagnostics.Process.Start(@"ImpressorCupom.exe", " " + documento.ToString());

                }
                catch (Exception erro)
                {
                    MessageBox.Show("erro->"+erro.ToString());
                }
            };


            // Imprimir o comprovante IQCARD quando foi pago com pontos

            if (!string.IsNullOrEmpty(idTransacaoIQCARD))
            {
                FuncoesECF.RelatorioGerencial("abrir", "");
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "OPERADOR: " + GlbVariaveis.glb_Usuario + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "PAGAMENTO FIDELIDADE ID.TRANS: " + idTransacaoIQCARD + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "    IQCARD NR.: " + IQCard + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "          Data: " + string.Format("{0:dd/MM/yyyy}", DateTime.Now) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "     DOCUMENTO: " + documento.ToString() + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "      VALOR R$: " + string.Format("{0:N2}", valorPF) + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "    ASSINATURA." + "______________________" + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", Environment.NewLine);
                FuncoesECF.RelatorioGerencial("fechar", "");
            }

            Venda.IQCard = "";
            Venda.idTransacaoIQCARD = "";
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
            _pdv.identificarClienteNFCe = false;
            return documento;            

            // falzer aqui a sincronizacao de dados
        }

        public void autorizarNota(string contadorCupomECF, int documento, int nf, bool mostrarMsg = true, string msgComplementar = "",bool gerar = true)
        {
            FrmMsgOperador msg = new FrmMsgOperador("", "Autorizando NFC-e "+msgComplementar.ToString());
            NFe NotaFiscal = new NFe();
            FuncoesNFC NFCe = new FuncoesNFC();
            siceEntities entidade = Conexao.CriarEntidade();
            msg.Show();
            Application.DoEvents();

            try
            {
                #region
                    FuncoesNFC.verificarGerenciadorNFCe();

                    var dadosDocumento = (from d in entidade.contdocs
                                          where d.documento == documento /*&& d.id == GlbVariaveis.glb_IP*/
                                          select d).FirstOrDefault();

                    dadosDocumento.ncupomfiscal = nf.ToString();
                    dadosDocumento.ecfcontadorcupomfiscal = int.Parse(contadorCupomECF).ToString();
                    dadosDocumento.ecffabricacao = "SICENFCe";
                    dadosDocumento.modeloDOCFiscal = "65";
                    entidade.SaveChanges();

                    dadosNFCe nfc = NFCe.verificaResp();
                    if (nfc.numeroNF != "0" && nfc.numeroNF != "")
                        this.atualizarDadosNFCe(nfc);

                    string mensagem = null;
                    string acao = "E";
                    if (gerar == false)
                        acao = "R";

                    NFCe.GerarReq(nf.ToString(), contadorCupomECF.ToString(), documento.ToString(), acao, GlbVariaveis.glb_IP); //E - Emissão C-Cancelamento
                    nfc = NFCe.LerResp("Venda", mostrarMsg);

                
                    if (nfc.chaveNFe == "Erro")
                    {
                        nfc.numeroNF = nf.ToString();
                        nfc.serieNF = contadorCupomECF.ToString();

                        
                        if (nfc.codigoProduto != "")
                        {
                            var codigoProduto = entidade.ExecuteStoreQuery<string>("SELECT concat(c.CbdcProd,' - ',c.CbdxProd) FROM cbd001det AS c WHERE c.CbdNtfNumero = '" + int.Parse(nfc.numeroNF).ToString() + "' AND c.CbdNtfSerie = '" + int.Parse(nfc.serieNF).ToString() + "' AND c.CbdCodigoFilial = '" + GlbVariaveis.glb_filial + "' AND c.CbdnItem = '" + nfc.codigoProduto.Trim() + "' LIMIT 1").FirstOrDefault();
                            if (codigoProduto != null)
                            {
                                mensagem = "[nItem:" + nfc.codigoProduto + "] no XML é produto " + codigoProduto.ToString();

                                if (mostrarMsg == true)
                                {
                                    frmOperadorTEF erro = new frmOperadorTEF(mensagem, false);
                                    erro.ShowDialog();
                                    erro.Dispose();
                                }
                            }
                        }
                    }

                    dadosDocumento = (from d in entidade.contdocs
                                  where d.documento == documento /*&& d.id == GlbVariaveis.glb_IP*/
                                  select d).FirstOrDefault();

                    if (mensagem != null)
                    {
                        string SQL = "UPDATE nfe012 AS n SET n.CbdStsRetNome = CONCAT(CbdStsRetNome, ' - ','" + mensagem + "') WHERE cbdDocumento = '" + documento + "' AND cbdmod = '65' and cbdCodigoFilial = '" + GlbVariaveis.glb_filial + "'";
                        Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
                    }
                    else if(mensagem == null && nfc.chaveNFe == "Erro")
                    {

                        string SQL = @"SELECT CONCAT(codigo,' - ',produto) FROM venda WHERE documento = '"+ documento + "' AND cstpis IN ('04','06','07','08','09') "+
                                        "AND pis > 0 "+
                                        "UNION " +
                                        "SELECT CONCAT(codigo,' - ',produto) FROM venda WHERE documento = '" + documento+ "' AND cstcofins IN ('04','06','07','08','09') " +
                                        "AND cofins > 0  " +
                                        "UNION " +
                                        "SELECT CONCAT(codigo,' - ',produto) FROM vendaarquivo WHERE documento = '" + documento + "' AND cstpis IN ('04','06','07','08','09') " +
                                        "AND pis > 0 " +
                                        "UNION " +
                                        "SELECT CONCAT(codigo,' - ',produto) FROM vendaarquivo WHERE documento = '" + documento + "' AND cstcofins IN ('04','06','07','08','09') " +
                                        "AND cofins > 0 " +
                                        "UNION " +
                                        "SELECT CONCAT(codigo, ' - ', produto) FROM venda WHERE documento = '" + documento + "' AND cstpis IN('01', '02', '03', '05') " +
                                        "AND pis = 0 " +
                                        "UNION " +
                                        "SELECT CONCAT(codigo, ' - ', produto) FROM venda WHERE documento = '" + documento + "' AND cstcofins IN('01', '02', '03', '05') " +
                                        "AND cofins = 0 " +
                                        "UNION " +
                                        "SELECT CONCAT(codigo, ' - ', produto) FROM vendaarquivo WHERE documento = '" + documento + "' AND cstpis IN('01', '02', '03', '05') " +
                                        "AND pis = 0 " +
                                        "UNION " +
                                        "SELECT CONCAT(codigo, ' - ', produto) FROM vendaarquivo WHERE documento = '" + documento + "' AND cstcofins IN('01', '02', '03', '05') " +
                                        "AND cofins = 0 " +
                                        "LIMIT 1";


                        var dadosProdutos = entidade.ExecuteStoreQuery<string>(SQL).FirstOrDefault();

                        SQL = "UPDATE nfe012 AS n SET n.CbdStsRetNome = CONCAT(CbdStsRetNome, ' - ','" + dadosProdutos + "') WHERE cbdDocumento = '" + documento + "' AND cbdmod = '65' and cbdCodigoFilial = '" + GlbVariaveis.glb_filial + "'";
                        Conexao.CriarEntidade().ExecuteStoreCommand(SQL);

                        if (dadosProdutos != null)
                            mensagem = "Produto " + dadosProdutos.ToString();
                        else
                            mostrarMsg = false;

                        if (mostrarMsg == true)
                        {
                            frmOperadorTEF erro = new frmOperadorTEF(mensagem, false);
                            erro.ShowDialog();
                            erro.Dispose();
                        }


                    }

                    dadosDocumento.dataAutorizacao = nfc.dataAutorizacao;
                    dadosDocumento.protocolo = nfc.protocolo;
                    dadosDocumento.chaveNFC = nfc.chaveNFe;
                    dadosDocumento.ncupomfiscal = nf.ToString().PadLeft(9,'0');
                    dadosDocumento.modeloDOCFiscal = "65";

                    if(nfc.serieNF != null && nfc.serieNF != "" && nfc.serieNF != "0" && nfc.serieNF != "000000")
                        dadosDocumento.ecfcontadorcupomfiscal = int.Parse(nfc.serieNF).ToString();

                    dadosDocumento.ecffabricacao = "SICENFCe";
                    entidade.SaveChanges();

                    if (Configuracoes.gerarTransferenciaVenda == true)
                    {
                        NFe objNFe = new NFe();
                        objNFe.Gerartransferencia(documento);
                    }

                    if (ConfiguracoesECF.NFCtransacaobanco == false)
                        File.Delete(@"C:\iqsistemas\SICENFC-e\Resp\resp.txt");
                    else
                        Conexao.CriarEntidade().ExecuteStoreCommand("delete from transacoesnfce where codigofilial = '" + GlbVariaveis.glb_filial + "' and ip = '" + GlbVariaveis.glb_IP + "'");
                
                #endregion
            }
            catch (Exception erro)
            {
                msg.Dispose();
                throw new Exception(erro.ToString());
            }

            msg.Dispose();
        }

        public bool verificaVenda(bool cancelarCupom, bool documentofinalizado)
        {

            siceEntities entidade;
            if (Conexao.tipoConexao == 2 && !Conexao.ConexaoOnline())
                entidade = Conexao.CriarEntidade(false);
            else
                entidade = Conexao.CriarEntidade();

            string SQL = "SELECT verificarvenda FROM configfinanc WHERE codigofilial = '" +GlbVariaveis.glb_filial+"'" ;

            var verificarVenda = entidade.ExecuteStoreQuery<string>(SQL).FirstOrDefault();

            string cancelar = cancelarCupom == true ? "S" : "N";

            if (verificarVenda == "S")
            {

                if (documentofinalizado == true)
                {
                    SQL = "SELECT IFNULL(MAX(documento),0) FROM contdocs WHERE ip='" + GlbVariaveis.glb_IP + "' and data = current_date";

                    var documento = entidade.ExecuteStoreQuery<int>(SQL).FirstOrDefault();


                    string sql = "CALL verificaVenda('" + documento + "','" + GlbVariaveis.glb_IP + "','" + GlbVariaveis.glb_filial + "','" + cancelar+ "')";
                    string aprovada = entidade.ExecuteStoreQuery<string>(sql).FirstOrDefault();

                    if (aprovada == "N" && cancelarCupom == true)
                    {
                        FuncoesECF.CancelarCupomECF();
                        MessageBox.Show("Inconsistência na venda por segurança a mesma foi cancelada", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (aprovada == "N" && cancelarCupom == false)
                    {
                        MessageBox.Show("Inconsistência na venda por segurança a mesma foi cancelada no sice.net \n para cancelar na ECF faça uma Nota Fiscal eletronica! ", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                        return false;
                    }
                    else
                    {

                    }
                }
                else
                {
                    string sql = "CALL verificaVenda('0','" + GlbVariaveis.glb_IP + "','" + GlbVariaveis.glb_filial + "','" + cancelar + "')";
                    string aprovada = entidade.ExecuteStoreQuery<string>(sql).FirstOrDefault();
                    if (aprovada == "N" && cancelarCupom == true)
                    {
                        FuncoesECF.CancelarCupomECF();
                        MessageBox.Show("Inconsistência na venda por segurança a mesma foi cancelada", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (aprovada == "N" && cancelarCupom == false)
                    {
                        MessageBox.Show("Inconsistência na venda por segurança a mesma foi cancelada no sice.net \n para cancelar na ECF faça uma Nota Fiscal eletronica! ", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {

                    }
                }
            }

            return true;

        }

        private int FinalizarVendaOff(string dataHoraCupomECF, string numeroCupomECF, string numeroECF, decimal totalIcmsCupom, string contadorCupomECF, decimal totalLiquidoCupomECF, string contadorDebitoCreditoECF, ref decimal rateioDescItens, ref decimal valorServicos, ref decimal descontoServicos, decimal custoItens)
        {
            IObjectContainer tabela = Db4oFactory.OpenFile("vendas.yap");

            ///Obtendo valores rateiodesconto
            ///
            var qtdItens = (from StandAloneVenda n in tabela
                            where n.ip == GlbVariaveis.glb_IP
                            select n).Count();

            rateioDescItens = this.desconto / qtdItens;

            /// Obtendo valores dos Servicos
            /// 
            var servico = from StandAloneVenda p in tabela
                          where p.ip == GlbVariaveis.glb_IP
                           && p.tipo == "1 - Servico"
                          group p by p.id into g
                          select new { total = g.Sum(p => p.quantidade * p.preco), desconto = g.Sum(p => p.descontovalor + p.ratdesc) };
            foreach (var soma in servico)
            {
                valorServicos = soma.total;
                descontoServicos = soma.desconto;
            }
            tabela.Close();

            IObjectContainer sequencia = Db4oFactory.OpenFile("contdocs.yap");
            int? seqDoc = ((from StandAloneContdocs p in sequencia
                            select (int?)p.documento).Max()) + 1;
            seqDoc = seqDoc == null ? 1 : seqDoc;
            sequencia.Close();

            IObjectContainer tabelaDoc = Db4oFactory.OpenFile("contdocs.yap");
            StandAloneContdocs registro = new StandAloneContdocs();
            registro.documento = seqDoc.GetValueOrDefault();
            registro.ip = GlbVariaveis.glb_IP;
            registro.data = dataHoraCupomECF == null ? DateTime.Now.Date : Convert.ToDateTime(dataHoraCupomECF).Date;
            registro.operador = GlbVariaveis.glb_Usuario;
            registro.codigofilial = GlbVariaveis.glb_filial;
            registro.valorBruto = this.valorBruto;
            registro.desconto = this.desconto;
            registro.total = this.valorBruto - this.desconto;
            registro.custoItens = custoItens;
            registro.valorServico = valorServicos;
            registro.descontoServico = descontoServicos;
            registro.dpFinanceiro = dpFinanceiro;
            registro.hora = dataHoraCupomECF == null ? DateTime.Now.TimeOfDay : Convert.ToDateTime(dataHoraCupomECF).TimeOfDay;
            registro.COOCupomFiscal = numeroCupomECF;
            registro.numeroECF = numeroECF;
            registro.CCFCupomFiscal = contadorCupomECF;
            registro.consumidorECF = dadosConsumidor.nomeConsumidor; // consumidorECF.Nome;
            registro.cpfCnpjConsumidor = dadosConsumidor.cpfCnpjConsumidor;// consumidorECF.CpfCgc;
            registro.enderecoConsumidor = dadosConsumidor.endConsumidor; // consumidorECF.Endereco;
            registro.totalLiquidoECF = totalLiquidoCupomECF;
            registro.COODebitoCredito = contadorDebitoCreditoECF;
            registro.icmsCupomFiscal = totalIcmsCupom;
            registro.troco = troco;
            registro.ecfFabricao = ConfiguracoesECF.nrFabricacaoECF;
            registro.ecfMFAdicional = ConfiguracoesECF.mfAdicionalECF;
            registro.ecfTipo = ConfiguracoesECF.tipoECF;
            registro.ecfMarca = ConfiguracoesECF.marcaECF;
            registro.ecfModelo = ConfiguracoesECF.modeloECF;
            tabelaDoc.Store(registro);
            tabelaDoc.Close();
            /// Atualizandas caixa e venda com o documento
            /// 
            IObjectContainer tabelaCaixa = Db4oFactory.OpenFile("caixas.yap");
            var dados = (from StandAloneCaixa n in tabelaCaixa
                         where n.ip == GlbVariaveis.glb_IP
                         select n).ToList();
            if (dados.Count==0)
            {
                IObjectContainer tabelaCx = Db4oFactory.OpenFile("caixa.yap");
                StandAloneCaixa registroCx = new StandAloneCaixa();
                registroCx.id = Guid.NewGuid();
                registroCx.ip = GlbVariaveis.glb_IP;
                registroCx.data = GlbVariaveis.Sys_Data;
                registroCx.dpFinanceiro = dpFinanceiro;
                registroCx.valor = valorLiquido; //(!)Atenção Valor líquido aqui por que nao existe outra forma de pagamento
                registroCx.operador = GlbVariaveis.glb_Usuario;
                registroCx.tipoPagamento = "DH";
                registroCx.vendedor = "000";
                tabelaCx.Store(registroCx);
                tabelaCx.Close();                
            }


            foreach (var item in dados)
            {
                item.documento = seqDoc.Value;
                tabelaCaixa.Store(item);
            }
            tabelaCaixa.Close();

            IObjectContainer CaixaInserir = Db4oFactory.OpenFile("caixa.yap");
            CaixaInserir.Store(dados);
            CaixaInserir.Close();

            IObjectContainer tabelaVenda = Db4oFactory.OpenFile("vendas.yap");
            var dadosVendas = (from StandAloneVenda n in tabelaVenda
                               where n.ip == GlbVariaveis.glb_IP
                               select n).ToList();
            foreach (var item in dadosVendas)
            {
                item.documento = seqDoc.Value;
                tabelaVenda.Store(item);
            }
            tabelaVenda.Close();

            IObjectContainer VendaInserir = Db4oFactory.OpenFile("venda.yap");
            VendaInserir.Store(dadosVendas);
            VendaInserir.Close();

            Venda.ApagarItensFormaPagamento("ItensPagamentos");

            return seqDoc.GetValueOrDefault();
        }

        private static void ObterSequencial(ref long seqFilial, string tipo)
        {            
            
            string tabela = "";
            siceEntities entidadeSEQ = Conexao.CriarEntidade();

            if (tipo == "PRE")
                tabela = "contprevendaspaf";
            if (tipo == "DAV")
                tabela = "contdav";
            if (tipo=="DAVOS")
                tabela="contdavos";

           string sql = @"
                        SET session transaction isolation level read committed;
                        SET autocommit = 0;
                        begin work;
                        UPDATE " + tabela + @" SET numeroDAVFilial = 
                        (
                            SELECT numeroDAVFilial FROM
                            (
                            SELECT IFNULL(MAX(numeroDAVFilial),0) + 1 AS numeroDAVFilial FROM " + tabela + @" WHERE codigofilial = " + GlbVariaveis.glb_filial + @"
                            ) AS 	tmptable
                        )
                            WHERE enderecoip = '" + GlbVariaveis.glb_IP + @"' AND numero =
                        (
                            SELECT numero FROM
                            (
                            SELECT IFNULL(MAX(numero),1) AS numero FROM " + tabela + @" WHERE enderecoip = '" + GlbVariaveis.glb_IP + @"'
                            ) AS 	tmptable1
                        );
                        COMMIT;                                                                                 
                        SELECT IFNULL(MAX(numeroDAVFilial),1) FROM " + tabela + @" WHERE enderecoip = '" + GlbVariaveis.glb_IP + @"' and codigofilial = " + GlbVariaveis.glb_filial + @";";

                    var numerodav = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql);       
                    seqFilial = int.Parse(numerodav.FirstOrDefault());                
            }

        public static void ObterSequencialNFC()
        {
            try
            {
                string sql = @"
                                            SET session transaction isolation level read committed;
                                            SET autocommit = 0;
                                            begin work;
                                            update serienf set sequencial = sequencial + 1 where serie = '" + ConfiguracoesECF.NFCserie + "';" +
                                                "SELECT sequencial FROM serienf WHERE serie = '" + ConfiguracoesECF.NFCserie + "';" +
                                            @"COMMIT;";

                var sequenciaNFC = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql);
                ConfiguracoesECF.NFCSequencia = int.Parse(sequenciaNFC.FirstOrDefault());
            }
            catch (Exception erro)
            {
                
            }
            
        }

        public static int ObterUltimoDocumento(bool sinconizado = true)
        {
            int documento = 0;

            if (sinconizado == true)
            {
                siceEntities entidade = Conexao.CriarEntidade();
                documento = (from doc in entidade.contdocs
                             where doc.ip == GlbVariaveis.glb_IP
                             select doc.documento).Max();
            }
            else
            {
                siceEntities entidade;
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);
                else
                    entidade = Conexao.CriarEntidade();

                documento = (from doc in entidade.contdocs
                             where doc.ip == GlbVariaveis.glb_IP
                             select doc.documento).Max();
            }

            return documento;
        }

        public void atualizarDadosNFCe(dadosNFCe nfc)
        {

            siceEntities entidade = Conexao.CriarEntidade();

            var dadosDocumento = (from d in entidade.contdocs
                                  where d.ncupomfiscal == nfc.numeroNF && d.ecfcontadorcupomfiscal == nfc.serieNF && d.id == GlbVariaveis.glb_IP
                                  select d).FirstOrDefault();

            if (dadosDocumento == null)
            {
                var documento = (from d in entidade.contdocs
                                      where d.id == GlbVariaveis.glb_IP && d.CodigoFilial == GlbVariaveis.glb_filial
                                      select d.documento).Max();

                dadosDocumento = (from d in entidade.contdocs
                                      where d.documento == documento && d.id == GlbVariaveis.glb_IP
                                      select d).FirstOrDefault();
            }

            dadosDocumento.dataAutorizacao = nfc.dataAutorizacao;
            dadosDocumento.protocolo = nfc.protocolo;
            dadosDocumento.chaveNFC = nfc.chaveNFe;
            dadosDocumento.ncupomfiscal = nfc.numeroNF;
            dadosDocumento.modeloDOCFiscal = "65";
            dadosDocumento.ecfcontadorcupomfiscal = nfc.serieNF;
            dadosDocumento.ecffabricacao = "SICENFCe";

            entidade.SaveChanges();

            File.Delete(@"C:\iqsistemas\SICENFC-e\Resp\resp.txt");
        }

        public bool AtualizarDocumentoExcluidoNFCe(dadosNFCe nfc)
        {
            try
            {
                siceEntities entidade = Conexao.CriarEntidade();

                var dadosDocumento = (from d in entidade.contdocs
                                      where d.ncupomfiscal == nfc.numeroNF && d.ecfcontadorcupomfiscal == nfc.serieNF && d.id == GlbVariaveis.glb_IP && d.estornado == "S" && d.total < 0
                                      select d).FirstOrDefault();


                dadosDocumento.dataAutorizacao = nfc.dataAutorizacao;
                dadosDocumento.protocolo = nfc.protocolo;
                dadosDocumento.chaveNFC = nfc.chaveNFe;
                dadosDocumento.ncupomfiscal = nfc.numeroNF;
                dadosDocumento.modeloDOCFiscal = "65";
                dadosDocumento.ecfcontadorcupomfiscal = nfc.serieNF;
                dadosDocumento.ecffabricacao = "SICENFCe";

                entidade.SaveChanges();

                File.Delete(@"C:\iqsistemas\SICENFC-e\Resp\resp.txt");
                return true;
            }
            catch (Exception erro)
            {
                return false;
            }
        }

        public void auditoriaDocumento(int documento)
        {
            try
            {
                siceEntities entidade = Conexao.CriarEntidade();
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                foreach (var linha in Venda.listAuditoriaVenda)
                {
                    entidade.ExecuteStoreCommand("update auditoria set documento = '"+documento+"' where id = '"+linha.inc+"'");
                }
                Venda.listAuditoriaVenda.Clear();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.ToString());
                Venda.listAuditoriaVenda.Clear();
            }
        }
        #endregion

        public static decimal ObterDescontoMaximo(string dpFinanceiro,string IQCard)
        {
            decimal valorDesconto = 0, maximoDesconto = 0;

            if (dpFinanceiro == "Recebimento")
            {
                var descontoJuros = FrmExtratoCliente.parcelas.Sum(c => c.valorJuros) * Configuracoes.descontoMaxRecJuros / 100;
                var descontoCapital = FrmExtratoCliente.parcelas.Sum(c => (c.valorPagamento - c.valorJuros)) * Configuracoes.descontoMaxRecCapital / 100;
                return (descontoJuros + descontoCapital);
            }

            #region StandAlone
            if (!Conexao.onLine)
            {
                using (IObjectContainer tabela = Db4oFactory.OpenFile("vendas.yap"))
                {
                    var v = from StandAloneVenda p in tabela
                            where p.ip == GlbVariaveis.glb_IP
                            && p.descontoperc == 0
                            group p by p.id into g
                            select new { totalSemDesconto = g.Sum(p => p.quantidade * p.preco) };

                    foreach (var soma in v)
                    {
                        valorDesconto = soma.totalSemDesconto;
                    }
                };

            #endregion
            }
            else
            {
                siceEntities entidade = Conexao.CriarEntidade();
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                using (entidade)
                {
                    var v = from p in entidade.vendas
                            where p.id == GlbVariaveis.glb_IP
                            && p.Descontoperc == 0 && p.descontovalor == 0
                            && p.cancelado == "N"
                            group p by p.id into g
                            select new { totalSemDesconto = g.Sum(p => p.quantidade * p.preco) };

                    foreach (var soma in v)
                    {
                        valorDesconto = soma.totalSemDesconto.Value;
                    }
                };
            };

            maximoDesconto = (Math.Truncate((valorDesconto * Configuracoes.descontoMaxVenda / 100) * 100) / 100);// +Configuracoes.arredondamento;

            if (!string.IsNullOrEmpty(IQCard) && Configuracoes.descontoCartaoFidelidade>0)
            {
                maximoDesconto = (Math.Truncate((valorDesconto * Configuracoes.descontoCartaoFidelidade / 100) * 100) / 100);// +Configuracoes.arredondamento;
            }
                                 
            return maximoDesconto;

        }

        public static decimal ObterValorDesconto()
        {
            decimal valorDesconto = 0;

            #region StandAlone
            if (!Conexao.onLine)
            {
                IObjectContainer tabela = Db4oFactory.OpenFile("vendas.yap");
                var v = from StandAloneVenda p in tabela
                        where p.ip == GlbVariaveis.glb_IP
                        && p.descontoperc == 0
                        group p by p.id into g
                        select new { totalSemDesconto = g.Sum(p => p.quantidade * p.preco) };

                foreach (var soma in v)
                {
                    valorDesconto = soma.totalSemDesconto;
                }
                tabela.Close();
                return valorDesconto;
            }
            #endregion

            siceEntities entidade;

            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);
            else
                entidade = Conexao.CriarEntidade();

            //decimal valorDesconto = 0;

            using (entidade)
            {
                
                    if (Configuracoes.descontoAtacado == true)
                    {

                        string sql = "SELECT TRUNCATE(IFNULL(SUM(v.quantidade * v.preco),0),2) AS total FROM vendas AS v, " + GlbVariaveis.glb_estoque + " AS p " +
                            "WHERE v.codigo = p.codigo AND v.CodigoFilial = p.codigofilial AND " +
                            "v.id = '" + GlbVariaveis.glb_IP + "' " +
                            "AND p.codigofilial = '" + GlbVariaveis.glb_filial + "' " +
                            "AND v.Descontoperc = 0 " +
                            "AND  v.cancelado = 'N' " +
                            "AND p.aceitadesconto = 'S' ";

                        valorDesconto = entidade.ExecuteStoreQuery<decimal>(sql).FirstOrDefault();

                    }
                    else
                    {
                        string sql = "SELECT TRUNCATE(IFNULL(SUM(v.quantidade * v.preco  ),0),2) AS total FROM vendas AS v, " + GlbVariaveis.glb_estoque + " AS p " +
                            "WHERE v.codigo = p.codigo AND v.CodigoFilial = p.codigofilial AND " +
                            "v.id = '" + GlbVariaveis.glb_IP + "' " +
                            "AND p.codigofilial = '" + GlbVariaveis.glb_filial + "' " +
                            "AND v.vendaatacado = 'N' " +
                            "AND v.Descontoperc = 0 " +
                            "AND  v.cancelado = 'N' " +
                            "AND p.aceitadesconto = 'S' ";

                        valorDesconto = entidade.ExecuteStoreQuery<decimal>(sql).FirstOrDefault();
                    }
                
            }
            return valorDesconto;
        }

        public static decimal ObterValorAcrescimo()
        {
            decimal valorDesconto = 0;

            #region StandAlone
            if (!Conexao.onLine)
            {
                IObjectContainer tabela = Db4oFactory.OpenFile("vendas.yap");
                var v = from StandAloneVenda p in tabela
                        where p.ip == GlbVariaveis.glb_IP
                        && p.descontoperc == 0
                        group p by p.id into g
                        select new { totalSemDesconto = g.Sum(p => p.quantidade * p.preco) };

                foreach (var soma in v)
                {
                    valorDesconto = soma.totalSemDesconto;
                }
                tabela.Close();
                return valorDesconto;
            }
            #endregion

            siceEntities entidade;

            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);
            else
                entidade = Conexao.CriarEntidade();

            //decimal valorDesconto = 0;

            using (entidade)
            {

                
                    string sql = "SELECT TRUNCATE(IFNULL(SUM(v.quantidade * v.preco),0),2) AS total FROM vendas AS v, " + GlbVariaveis.glb_estoque + " AS p " +
                        "WHERE v.codigo = p.codigo AND v.CodigoFilial = p.codigofilial AND " +
                        "v.id = '" + GlbVariaveis.glb_IP + "' " +
                        "AND p.codigofilial = '" + GlbVariaveis.glb_filial + "' " +
                        //"AND v.Descontoperc = 0 " +
                        "AND  v.cancelado = 'N'";

                    valorDesconto = entidade.ExecuteStoreQuery<decimal>(sql).FirstOrDefault();

            }
            return valorDesconto;
        }

        public IQueryable<caixas> SelecionaPagamentoVenda()
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            var pagamento = from p in entidade.caixas
                            where p.EnderecoIP == GlbVariaveis.glb_IP
                            && p.CodigoFilial == GlbVariaveis.glb_filial
                            select p;
            return pagamento;
        }

        public bool ExcluirDocumento(string numeroCupomFiscal,string ccfCupom, int documento,string motivo="")
        {
            if (numeroCupomFiscal == "0") return false;

            #region StandAlone
            if (!Conexao.onLine)
            {
                IObjectContainer tabela = Db4oFactory.OpenFile("contdocs.yap");

                var dado = (from StandAloneContdocs n in tabela
                            where n.COOCupomFiscal == numeroCupomFiscal
                            select n);                

                if (dado.Count() > 0)
                    documento = dado.First().documento;

                if (!Permissoes.excluirDocumento && documento > 0)
                {
                    FrmLogon Logon = new FrmLogon();
                    Operador.autorizado = false;
                    Logon.campo = "venexcluir";
                    Logon.lblDescricao.Text = "Excluir Documento";
                    Logon.txtDescricao.Text = "Excluir o Documento: " + documento.ToString();
                    Logon.ShowDialog();
                    if (!Operador.autorizado) return false;

                    if (MessageBox.Show("Excluir o cupom: Documento de referência: " + documento.ToString() + "  ?", "Confirma", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return false;
                }

                if (!CancelarCupomECF())
                {
                    tabela.Close();
                    return false;
                }

                if (documento == 0)
                {
                    ApagarItensFormaPagamento("ItensPagamentos");
                    tabela.Close();
                    return true;
                };


                var docExcluir = (from StandAloneContdocs n in tabela
                                  where n.documento == documento
                                  select n).ToList();
                tabela.Delete(docExcluir[0]);
                tabela.Close();

                IObjectContainer tabelaCaixa = Db4oFactory.OpenFile("caixa.yap");
                var docCaixa = (from StandAloneCaixa n in tabelaCaixa
                                where n.documento == documento
                                select n).ToList();
                foreach (var item in docCaixa)
                {
                    tabelaCaixa.Delete(item);
                }
                tabelaCaixa.Close();

                IObjectContainer tabelaVenda = Db4oFactory.OpenFile("venda.yap");
                var docVenda = (from StandAloneCaixa n in tabelaVenda
                                where n.documento == documento
                                select n).ToList();
                foreach (var item in docVenda)
                {
                    tabelaVenda.Delete(item);
                }
                tabelaVenda.Close();

                ApagarItensFormaPagamento("ItensPagamentos");
                return true;
            }
            #endregion standalone
            

            if (documento > 0)
            {
                string conexao = Conexao.stringConexao;
                if (Conexao.tipoConexao == 2)
                    conexao = Conexao.stringConexaoRemoto;

                // Estorno crédito IQCARD
                try
                {

                     
                    siceEntities entidadeEstorno;
                    if (Conexao.tipoConexao == 2 && !Conexao.ConexaoOnline())
                        entidadeEstorno = Conexao.CriarEntidade(false);
                    else
                        entidadeEstorno = Conexao.CriarEntidade();

                    string sql = "SELECT idTransacaoIQCARD FROM contdocs WHERE documento='" + documento.ToString() + "'";
                    var idTransacao = entidadeEstorno.ExecuteStoreQuery<string>(sql).FirstOrDefault();


                    if (!string.IsNullOrEmpty(idTransacao))
                    {
                        MessageBox.Show(idTransacao);
                        ServiceReference1.WSIQPassClient card = new ServiceReference1.WSIQPassClient();
                        card.EstornarDebitarBitCoin(GlbVariaveis.chavePrivada, idTransacao);
                    }

                }
                catch (Exception)
                {

                    //throw;
                }


                using (EntityConnection conn = new EntityConnection(conexao))
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "siceEntities.ExcluirDocumento";

                    EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                    ip.Direction = ParameterDirection.Input;
                    ip.Value = GlbVariaveis.glb_IP;


                    EntityParameter doc = cmd.Parameters.Add("nrDocumento", DbType.Int32);
                    doc.Direction = ParameterDirection.Input;
                    doc.Value = documento;

                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;

                    EntityParameter operador = cmd.Parameters.Add("operador", DbType.String);
                    operador.Direction = ParameterDirection.Input;
                    operador.Value = GlbVariaveis.glb_Usuario; 
                                       
                    EntityParameter cooDoc = cmd.Parameters.Add("cooECF", DbType.String);
                    cooDoc.Direction = ParameterDirection.Input;
                    cooDoc.Value = numeroCupomFiscal;

                    EntityParameter ccfDOC = cmd.Parameters.Add("ccfECF", DbType.String);
                    ccfDOC.Direction = ParameterDirection.Input;
                    ccfDOC.Value = ccfCupom;

                    EntityParameter obs = cmd.Parameters.Add("motivoObs", DbType.String);
                    obs.Direction = ParameterDirection.Input;
                    obs.Value = motivo;

                    EntityParameter operadorSol = cmd.Parameters.Add("usuarioSolicitante", DbType.String);
                    operadorSol.Direction = ParameterDirection.Input;
                    operadorSol.Value = Operador.ultimoOperadorAutorizado;


                    cmd.ExecuteNonQuery();
                    conn.Close();
                }


                siceEntities entidadeControle;
                if (Conexao.tipoConexao == 2 && !Conexao.ConexaoOnline())
                    entidadeControle = Conexao.CriarEntidade(false);
                else
                    entidadeControle = Conexao.CriarEntidade();

                entidadeControle.ExecuteStoreCommand("UPDATE controlesincronizacao SET cancelado = '-' WHERE documentoOrigem = '"+documento+"'");
            }

            if (documento == 0)
            {
                vendaFinalizada = false;
                var docExcluir = Finalizar(false, false, false, false, false);
                using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "siceEntities.ExcluirDocumento";

                    EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                    ip.Direction = ParameterDirection.Input;
                    ip.Value = GlbVariaveis.glb_IP;

                    EntityParameter doc = cmd.Parameters.Add("nrDocumento", DbType.Int32);
                    doc.Direction = ParameterDirection.Input;
                    doc.Value = docExcluir;

                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;

                    EntityParameter operador = cmd.Parameters.Add("operador", DbType.String);
                    operador.Direction = ParameterDirection.Input;
                    operador.Value = GlbVariaveis.glb_Usuario;

                    EntityParameter cooDoc = cmd.Parameters.Add("cooECF", DbType.String);
                    cooDoc.Direction = ParameterDirection.Input;
                    cooDoc.Value = numeroCupomFiscal;

                    EntityParameter ccfDOC = cmd.Parameters.Add("ccfECF", DbType.String);
                    ccfDOC.Direction = ParameterDirection.Input;
                    ccfDOC.Value = ccfCupom;

                    EntityParameter obs = cmd.Parameters.Add("motivoObs", DbType.String);
                    obs.Direction = ParameterDirection.Input;
                    obs.Value = motivo;

                    EntityParameter operadorSol = cmd.Parameters.Add("usuarioSolicitante", DbType.String);
                    operadorSol.Direction = ParameterDirection.Input;
                    operadorSol.Value = Operador.ultimoOperadorAutorizado;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

            }


            ApagarItensFormaPagamento("ItensPagamentos");


            //fazer o cancelamento de cupom off



            return true;
        }

        public bool ImprimirComprovante(int idCliente, string nomeCliente, int documento, bool sincronizado = true)
        {
            try
            {
                siceEntities entidade = Conexao.CriarEntidade();
                if (sincronizado == false)
                    entidade = Conexao.CriarEntidade(false);

                StringBuilder msgRodapeCupom = new StringBuilder();
                msgRodapeCupom.AppendLine("Estou ciente do débito abaixo relacionado. ");
                msgRodapeCupom.AppendLine("Pelo qual pagarei no vencimento");

                var dados = from n in entidade.crmovclientes
                            orderby n.vencimento
                            where n.documento == documento && n.codigo == idCliente
                            select n;

                if (nomeCliente == null)
                    nomeCliente = dados.First().nome;

                var dadosCLientes = (from c in Conexao.CriarEntidade().clientes
                                     where c.Codigo == idCliente
                                     select new { debito = c.debito + c.debitoch, credito = c.credito, saldo = c.saldo, cpf = c.cpf, cnpf = c.cnpj, endereco = c.endereco, numero = c.numero, bairro = c.bairro, cidade = c.cidade, estado = c.estado}).FirstOrDefault();

                decimal totalDebito = 0;
                foreach (var item in dados)
                {
                    msgRodapeCupom.AppendLine(string.Format("{0:dd/MM/yyyy}", item.datacompra) + " " + string.Format("{0:dd/MM/yyy}", item.vencimento) + " " + string.Format("{0:C2}", item.valoratual));
                    totalDebito += item.valoratual;
                }
                msgRodapeCupom.AppendLine("===============================");
                msgRodapeCupom.AppendLine("TOTAL R$:" + string.Format("{0:N2}", totalDebito));
                msgRodapeCupom.AppendLine("===============================");
                msgRodapeCupom.AppendLine("Historico de Cliente");
                msgRodapeCupom.AppendLine("Debito  R$:" + string.Format("{0:N2}", dadosCLientes.debito));
                msgRodapeCupom.AppendLine("Credito R$:" + string.Format("{0:N2}", dadosCLientes.credito));
                msgRodapeCupom.AppendLine("Saldo   R$:" + string.Format("{0:N2}", dadosCLientes.saldo));
                msgRodapeCupom.AppendLine("" + Environment.NewLine);
                msgRodapeCupom.AppendLine("Ass:___________________________________");
                msgRodapeCupom.AppendLine(idCliente.ToString() + " " + nomeCliente);
                msgRodapeCupom.AppendLine("CPF.:" + " " + dadosCLientes.cpf);
                msgRodapeCupom.AppendLine("Endereco.:" + " " + dadosCLientes.endereco+", "+dadosCLientes.numero);
                msgRodapeCupom.AppendLine("Bairro.:" + " " + dadosCLientes.bairro);
                msgRodapeCupom.AppendLine("Cidade.:" + " " + dadosCLientes.cidade+" - "+dadosCLientes.estado);



                FuncoesECF.RelatorioGerencial("abrir", "");
                FuncoesECF.RelatorioGerencial("imprimir", msgRodapeCupom.ToString());
                FuncoesECF.RelatorioGerencial("fechar", "", "");


                return true;

            }
            catch (Exception erro)
            {
                throw new Exception(erro.ToString());
            }
        }

        public void ImprimirCarne(int documento, bool sincronizada = true)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (sincronizada == false)
                entidade = Conexao.CriarEntidade(false);
            
            var dados = from n in entidade.crmovclientes
                        orderby n.vencimento
                        where n.documento == documento
                        select new { n.codigo, n.nome, n.documento, n.nrParcela,n.datacompra, n.vencimento, n.Valor};

            decimal? entrada = (from n in entidade.caixa
                           where n.documento == documento
                           && n.tipopagamento != "CR"
                           select (decimal?)n.valor).Sum();


            StringBuilder sb = new StringBuilder();
            FuncoesECF.RelatorioGerencial("abrir", "");  
            if (entrada.HasValue)
            {
                sb.AppendLine(Configuracoes.fantasia);
                sb.AppendLine("CNPJ: " + Configuracoes.cnpj);
                sb.AppendLine("Cliente: " + dados.First().codigo + " " + dados.First().nome + "\r\n");

                sb.AppendLine("       ***** ENTRADA ***** ");
                if (sincronizada == true)
                    sb.AppendLine("DOCUMENTO : " + documento.ToString());
                else
                    sb.AppendLine("DOCUMENTO : " + documento.ToString()+" ->Modo Off-Line <-");

                sb.AppendLine("VR.ENTRADA: " + entrada.ToString());                
                sb.AppendLine("");
                sb.AppendLine("-----------------------------------------------");

                    FuncoesECF.RelatorioGerencial("imprimir", sb.ToString());
                    sb.Clear();
               
            }

  
            foreach (var item in dados)
            {
                
                sb.AppendLine(Configuracoes.fantasia);
                sb.AppendLine("CNPJ: " + Configuracoes.cnpj);
                sb.AppendLine("Cliente: " + item.codigo + " " + item.nome + "\r\n");

                //sb.AppendLine("DOCUMENTO : " + item.documento.ToString());
                if (sincronizada == true)
                    sb.AppendLine("DOCUMENTO : " + documento.ToString());
                else
                    sb.AppendLine("DOCUMENTO : " + documento.ToString() + " ->Modo Off-Line <-");

                sb.AppendLine("VALOR.... : " + item.Valor.ToString());
                sb.AppendLine("D. COMPRA : " + string.Format("{0:dd/MM/yyy}", item.datacompra));
                sb.AppendLine("VENCIMENTO: " + string.Format("{0:dd/MM/yyy}", item.vencimento));
                sb.AppendLine("PARCELA   : " + item.nrParcela.ToString());
                sb.AppendLine("");
                sb.AppendLine("JUROS     :____________VR. PAG   :___________");
                sb.AppendLine("após "+Configuracoes.diasLiberadosSemJuros.ToString()+" dias do venc. Serão cobrados "+string.Format("{0:N2}", Configuracoes.taxaJurosDiario) +"% ao dia de juros." );
                sb.AppendLine("");
                sb.AppendLine("-----------------------------------------------");

               
                    FuncoesECF.RelatorioGerencial("imprimir", sb.ToString());
                    FuncoesECF.RelatorioGerencial("imprimir", " ");
                    FuncoesECF.RelatorioGerencial("imprimir", sb.ToString());

                    sb.Clear();
             
                
            }

           /* if (GlbVariaveis.glb_Acbr == true)
            {
                FuncoesECF.RelatorioGerencial("imprimir", sb.ToString(),"",true,2);
            }*/

                FuncoesECF.RelatorioGerencial("fechar", "");
            
                               

        }

        public bool ImprimirDAV(long davNumero)
        {
            ConfiguracoesECF.idECF = ConfiguracoesECF.ultIDECF;
            siceEntities entidade = Conexao.CriarEntidade();
            var dadosDAV = (from n in entidade.contdav
                            where n.numeroDAVFilial == davNumero
                            && n.codigofilial == GlbVariaveis.glb_filial
                            select n).First();


            var dados = from n in entidade.vendadav
                        where n.documento == davNumero
                        && n.codigofilial == GlbVariaveis.glb_filial
                        //&& n.cancelado == "N"
                        select new
                        {
                            codigo = n.codigo,
                            produto = n.produto,
                            unidade = n.unidade,
                            quantidade = n.quantidade,
                            precooriginal = n.precooriginal,
                            Descontoperc = n.Descontoperc,
                            descontovalor = n.descontovalor,
                            acrescimototalitem = n.acrescimototalitem, //(n.preco - n.precooriginal) <= 0 ? 0 : n.preco - n.precooriginal,
                            total = n.total,
                            tipo = n.tipo,
                            cancelado = n.cancelado
                        };
            var pagamentos = from n in entidade.caixadav
                             where n.documento == davNumero
                             && n.CodigoFilial == GlbVariaveis.glb_filial
                             orderby n.tipopagamento, n.vencimento
                             select new
                             {
                                 Nrparcela = n.Nrparcela,
                                 tipopagamento = n.tipopagamento,
                                 vencimento = n.vencimento,
                                 valor = n.valor,
                             };
            decimal? totalServicos = (from n in dados
                                      where n.tipo == "1 - Servico"
                                      select (decimal?)n.total).Sum();
            decimal? descontoServicos = (from n in dados
                                         where n.tipo == "1 - Servico"
                                         select (decimal?)n.descontovalor).Sum();
            string dadosIdcliente = "";

            dadosIdcliente = Funcoes.FormatarCPF(dadosDAV.ecfCPFCNPJconsumidor ?? " ");

            var sql = "select IFNULL(ecfconsumidor, '') as cliente from contdav where numerodavfilial = " + davNumero;
            var nomeCliente = Conexao.CriarEntidade().ExecuteStoreQuery<String>(sql).FirstOrDefault();


            dadosIdcliente = nomeCliente + " - " + Funcoes.FormatarCPF(dadosDAV.ecfCPFCNPJconsumidor ?? " ");

            decimal? totalBruto = dadosDAV.valor+dadosDAV.desconto;
            decimal? totalLiquido = dadosDAV.valor+dadosDAV.encargos;

            var vendedor = (from v in Conexao.CriarEntidade().vendedores
                           where v.codigo == dadosDAV.vendedor && v.CodigoFilial == dadosDAV.codigofilial
                           select v).FirstOrDefault();



            if (dadosDAV.codigocliente > 0)
            {
                var dadosClientes = (from n in entidade.clientes
                                     where n.Codigo == dadosDAV.codigocliente
                                     select new { n.endereco, n.numero, n.cep, n.bairro, n.cidade, n.estado, n.telefone, n.celular }).FirstOrDefault();
                string end = dadosClientes.endereco ?? "";
                string num = dadosClientes.numero ?? "";
                string cep = dadosClientes.cep ?? "";
                string bai = dadosClientes.bairro ?? "";
                string cid = dadosClientes.cidade ?? "";
                string est = dadosClientes.estado ?? "";
                string tel = dadosClientes.telefone.Replace("_", "").Replace("-", "") ?? "";
                string cel = dadosClientes.celular.Replace("_", "").Replace("-", "") ?? "";

                dadosIdcliente = Funcoes.FormatarCPF(dadosDAV.ecfCPFCNPJconsumidor ?? " ") +
                 end + " " + num + Environment.NewLine +
                 cep + " " + bai + Environment.NewLine +
                 cid + " " + est + Environment.NewLine +
                 "Tel: " + Funcoes.FormatarTelefone(tel) + " " + Funcoes.FormatarTelefone(cel);
            }

            string infoServico = "";
            string infoEntrega = "";
            if (totalServicos.HasValue)
                infoServico = "Produtos/Peças : " + string.Format("{0:N2}", dadosDAV.valor - (totalServicos == null ? 0 : totalServicos.GetValueOrDefault())) + "       Serviços R$: " + string.Format("{0:N2}", (totalServicos == null ? 0 : totalServicos.GetValueOrDefault())) + "   Desc.Serviço R$: " + string.Format("{0:N2}", descontoServicos.GetValueOrDefault());

            if (dadosDAV.enderecoentrega != "")
            {
                infoEntrega = "Dados Adicionais / End.Entrega" + Environment.NewLine +
                    dadosDAV.responsavelreceber + Environment.NewLine +
                    dadosDAV.enderecoentrega + " " + dadosDAV.numeroentrega + Environment.NewLine +
                    dadosDAV.cepentrega + " " + dadosDAV.bairroentrega + " " + dadosDAV.cidadeentrega + dadosDAV.estadoentrega + Environment.NewLine +
                    dadosDAV.horaentrega.ToString();
            }
            string dadosDAVOS = " ";

            if (!string.IsNullOrEmpty(dadosDAV.osnrfabricacao))
            {
                dadosDAVOS = "PRODUTO OBJETO DO CONSERTO " + Environment.NewLine + Environment.NewLine +
                             "NR. FABRICAÇÃO:" + dadosDAV.osnrfabricacao + Environment.NewLine + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(dadosDAV.placa))
            {
                dadosDAVOS += "PRODUTO OBJETO DO CONSERTO" + Environment.NewLine + Environment.NewLine +
                    "Marca  : " + dadosDAV.marca + " Modelo: " + dadosDAV.modelo + Environment.NewLine +
                    "Ano    : " + dadosDAV.osnrfabricacao + " PLACA : " + dadosDAV.placa + Environment.NewLine +
                    "RENAVAM: " + dadosDAV.renavam;
            }

            StringBuilder cabecalho = new StringBuilder();
            StringBuilder cabecalhoDAV = new StringBuilder();
            StringBuilder Itens = new StringBuilder();
            StringBuilder Itens2 = new StringBuilder();
            StringBuilder rodape = new StringBuilder();

            cabecalho.AppendLine("DOCUMENTO AUXILIAR DE VENDA - ORÇAMENTO" + Environment.NewLine +
            "NÃO É DOCUMENTO FISCAL" + Environment.NewLine +
            "NÃO É VÁLIDO COMO RECIBO E" + Environment.NewLine +
            "COMO GARANTIA DE MERCADORIA" + Environment.NewLine +
            "NÃO  COMPROVA PAGAMENTO"+Environment.NewLine);


            //cabecalho.AppendLine("IDENTIFICAÇÃO DO DAV(DOCUMENTO AUXILIAR DE VENDA)" );

            //cabecalho.AppendLine(Configuracoes.razaoSocial);
            //cabecalho.AppendLine(Funcoes.FormatarCNPJ(Configuracoes.cnpj));
            //cabecalho.AppendLine(Configuracoes.endereco);
            //cabecalho.AppendLine(Configuracoes.bairro + " " + Configuracoes.cidade + " " + Configuracoes.estado );
            //cabecalhoDAV.AppendLine("");
            //cabecalho.AppendLine("Tel:" + Funcoes.FormatarTelefone(Configuracoes.telefone + Environment.NewLine));

           

            cabecalho.AppendLine("--------------------------------------------------------" + Environment.NewLine);
            cabecalho.AppendLine("IDENTIFICAÇÃO DO DESTINATÁRIO");
            cabecalho.AppendLine(dadosDAV.codigocliente + " - " + dadosDAV.cliente);
            cabecalho.AppendLine(dadosIdcliente+Environment.NewLine);
            cabecalhoDAV.AppendLine("Nº do Documento: " + Funcoes.FormatarZerosEsquerda(davNumero, 10, false));
            cabecalhoDAV.AppendLine("");
            cabecalhoDAV.AppendLine("Nº do Documento Fiscal:_____________________");
            cabecalhoDAV.AppendLine("");
            cabecalho.AppendLine("Operador: " + dadosDAV.operador.ToString());
            cabecalhoDAV.AppendLine("");
            if (dadosDAV.vendedor != "0" && dadosDAV.vendedor != "00" && dadosDAV.vendedor != "000" && dadosDAV.vendedor != "")
                cabecalho.AppendLine("Vendedor: " + vendedor.codigo + " - " + vendedor.nome + Environment.NewLine);
            else
                cabecalho.AppendLine("Vendedor: 000 - GERAL" + Environment.NewLine);

            if (dadosDAV.observacao != "")
            {
                cabecalhoDAV.AppendLine(dadosDAV.observacao);
                cabecalhoDAV.AppendLine("");
            }
            if (infoEntrega != "")
            {
                cabecalhoDAV.AppendLine(infoEntrega);
                cabecalhoDAV.AppendLine("");
            }
            FuncoesECF.RelatorioGerencial("imprimir", cabecalho.ToString());
            FuncoesECF.RelatorioGerencial("imprimir", cabecalhoDAV.ToString());

            FuncoesECF.RelatorioGerencial("imprimir", "Cód.  Des.      Unid.   Qtd.  Preço   Desc   Total " + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "--------------------------------------------------------" + Environment.NewLine);
            foreach (var item in dados)
            {
                if (item.cancelado == "N")
                {
                    FuncoesECF.RelatorioGerencial("imprimir", item.codigo + " " + item.produto.TrimEnd() + Environment.NewLine
                    + "                   " + item.unidade + " " + item.quantidade.ToString() + " " + item.precooriginal.ToString() + " " + item.descontovalor.ToString() + " " + item.total.ToString() + Environment.NewLine + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", " ");
                    cabecalhoDAV.AppendLine("");
                }
                if (item.cancelado == "S")
                {
                    FuncoesECF.RelatorioGerencial("imprimir", item.codigo + "" + item.produto + " **CANCELADO** "
                    + "                  " + item.unidade + " " + item.quantidade.ToString() + " " + item.precooriginal.ToString() + " " + item.descontovalor.ToString() + " " + item.total.ToString() + Environment.NewLine + Environment.NewLine);
                    FuncoesECF.RelatorioGerencial("imprimir", " ");
                    cabecalhoDAV.AppendLine("");
                }
            }

            FuncoesECF.RelatorioGerencial("imprimir", "--------------------------------------------------------" + Environment.NewLine);
            FuncoesECF.RelatorioGerencial("imprimir", "F. Pagamento  Parcela   Valor.  Vencimento " + Environment.NewLine);
            foreach (var item in pagamentos)
            {
                string parcela = item.Nrparcela;
                if (parcela == "" || parcela == null)
                    parcela = "1/1";

                FuncoesECF.RelatorioGerencial("imprimir", item.tipopagamento + " - " + parcela + " - "+ item.valor+ " - "+item.vencimento + Environment.NewLine);
            }


            rodape.AppendLine("---------------------------------------------------------" + Environment.NewLine);
            rodape.AppendLine("TOTAL BRUTO   R$:" + totalBruto.ToString());
            rodape.AppendLine("DESCONTO      R$:" + dadosDAV.desconto.ToString());
            rodape.AppendLine("TOTAL LÍQUIDO R$:" + totalLiquido.ToString());

            rodape.AppendLine("");
            rodape.AppendLine(" ");

            Itens.AppendLine("É vedada a autenticação deste documento.");         
            
                        
            FuncoesECF.RelatorioGerencial("imprimir", rodape.ToString());
            FuncoesECF.RelatorioGerencial("fechar", "");

            if (!ConfiguracoesECF.pdv)
                ConfiguracoesECF.idECF = 0;
            return true;           
        }

        public bool ImprimirComprovanteEntrega(int documento)
        {
            siceEntities entidade = Conexao.CriarEntidade();

            var vendaRomaneio = (from v in entidade.venda
                                  where v.aentregar == "S"
                                  && v.romaneio == "S"
                                  && v.cancelado == "N"
                                  && v.documento == documento
                                  select v.inc).ToList();

            var vendaEntrega = (from v in entidade.venda
                                  where v.aentregar == "N"
                                  && v.romaneio == "N"
                                  && v.cancelado == "N"
                                  && v.documento == documento
                                  select v.inc).ToList();


            if (vendaRomaneio.Count() > 0 && vendaEntrega.Count() > 0)
            {
                ConfiguracoesECF.idECF = ConfiguracoesECF.ultIDECF;

                var contdoc = (from n in entidade.contdocs
                               where n.documento == documento
                               && n.CodigoFilial == GlbVariaveis.glb_filial
                               select n).First();


                var dados = from n in entidade.venda
                            where n.documento == documento
                            && n.codigofilial == GlbVariaveis.glb_filial
                            && n.cancelado == "N"
                            && n.aentregar == "N"
                            && n.romaneio == "N"
                            select new
                            {
                                codigo = n.codigo,
                                produto = n.produto,
                                unidade = n.unidade,
                                quantidade = n.quantidade,
                                precooriginal = n.precooriginal,
                                Descontoperc = n.Descontoperc,
                                descontovalor = n.descontovalor,
                                acrescimototalitem = n.acrescimototalitem, //(n.preco - n.precooriginal) <= 0 ? 0 : n.preco - n.precooriginal,
                                total = n.total,
                                tipo = n.tipo,
                                cancelado = n.cancelado,
                                rateioDesconto = n.ratdesc
                            };

                var pagamentos = from n in entidade.caixa
                                 where n.documento == documento
                                 && n.CodigoFilial == GlbVariaveis.glb_filial
                                 orderby n.tipopagamento, n.vencimento
                                 select new
                                 {
                                     Nrparcela = n.Nrparcela,
                                     tipopagamento = n.tipopagamento,
                                     vencimento = n.vencimento,
                                     valor = n.valor,
                                 };

                decimal? totalServicos = (from n in dados
                                          where n.tipo == "1 - Servico"
                                          select (decimal?)n.total).Sum();
                decimal? descontoServicos = (from n in dados
                                             where n.tipo == "1 - Servico"
                                             select (decimal?)n.descontovalor).Sum();
                string dadosIdcliente = "";

                dadosIdcliente = Funcoes.FormatarCPF(contdoc.ecfCPFCNPJconsumidor ?? " ");

                decimal? totalBruto = 0;
                decimal? totalLiquido = 0;

                var vendedor = (from v in Conexao.CriarEntidade().vendedores
                                where v.codigo == contdoc.vendedor && v.CodigoFilial == contdoc.CodigoFilial
                                select v).FirstOrDefault();



                if (contdoc.codigocliente > 0)
                {
                    var dadosClientes = (from n in entidade.clientes
                                         where n.Codigo == contdoc.codigocliente
                                         select new { n.endereco, n.numero, n.cep, n.bairro, n.cidade, n.estado, n.telefone, n.celular }).FirstOrDefault();
                    string end = dadosClientes.endereco ?? "";
                    string num = dadosClientes.numero ?? "";
                    string cep = dadosClientes.cep ?? "";
                    string bai = dadosClientes.bairro ?? "";
                    string cid = dadosClientes.cidade ?? "";
                    string est = dadosClientes.estado ?? "";
                    string tel = dadosClientes.telefone.Replace("_", "").Replace("-", "") ?? "";
                    string cel = dadosClientes.celular.Replace("_", "").Replace("-", "") ?? "";

                    dadosIdcliente = Funcoes.FormatarCPF(contdoc.ecfCPFCNPJconsumidor ?? " ") +
                     end + " " + num + Environment.NewLine +
                     cep + " " + bai + Environment.NewLine +
                     cid + " " + est + Environment.NewLine +
                     "Tel: " + Funcoes.FormatarTelefone(tel) + " " + Funcoes.FormatarTelefone(cel);
                }

                string infoServico = "";
                string infoEntrega = "";
                if (totalServicos.HasValue)
                    infoServico = "Produtos/Peças : " + string.Format("{0:N2}", contdoc.total - (totalServicos == null ? 0 : totalServicos.GetValueOrDefault())) + "       Serviços R$: " + string.Format("{0:N2}", (totalServicos == null ? 0 : totalServicos.GetValueOrDefault())) + "   Desc.Serviço R$: " + string.Format("{0:N2}", descontoServicos.GetValueOrDefault());


                StringBuilder cabecalho = new StringBuilder();
                StringBuilder cabecalhoDAV = new StringBuilder();
                StringBuilder Itens = new StringBuilder();
                StringBuilder Itens2 = new StringBuilder();
                StringBuilder rodape = new StringBuilder();

                cabecalho.AppendLine("RECIDO DE ENTREGA" + Environment.NewLine +
                "NÃO É DOCUMENTO FISCAL" + Environment.NewLine +
                "NÃO É VÁLIDO COMO GARANTIA DE MERCADORIA" + Environment.NewLine +
                "NÃO  COMPROVA PAGAMENTO" + Environment.NewLine);



                cabecalho.AppendLine("--------------------------------------------------------" + Environment.NewLine);
                cabecalho.AppendLine("IDENTIFICAÇÃO DO DESTINATÁRIO");
                cabecalho.AppendLine(contdoc.codigocliente + " - " + contdoc.nome);
                cabecalho.AppendLine(dadosIdcliente + Environment.NewLine);
                cabecalhoDAV.AppendLine("Nº do Documento: " + Funcoes.FormatarZerosEsquerda(documento, 10, false));
                cabecalhoDAV.AppendLine("");
                cabecalhoDAV.AppendLine("Nº do Documento Fiscal:" + contdoc.ncupomfiscal + "/" + contdoc.ecfcontadorcupomfiscal);
                cabecalhoDAV.AppendLine("");
                cabecalho.AppendLine("Operador: " + contdoc.operador.ToString());
                cabecalhoDAV.AppendLine("");
                if (contdoc.vendedor != "0" && contdoc.vendedor != "00" && contdoc.vendedor != "000" && contdoc.vendedor != "")
                    cabecalho.AppendLine("Vendedor: " + vendedor.codigo + " - " + vendedor.nome + Environment.NewLine);
                else
                    cabecalho.AppendLine("Vendedor: 000 - GERAL" + Environment.NewLine);

                if (contdoc.Observacao != "")
                {
                    cabecalhoDAV.AppendLine(contdoc.Observacao);
                    cabecalhoDAV.AppendLine("");
                }
                if (infoEntrega != "")
                {
                    cabecalhoDAV.AppendLine(infoEntrega);
                    cabecalhoDAV.AppendLine("");
                }
                FuncoesECF.RelatorioGerencial("imprimir", cabecalho.ToString());
                FuncoesECF.RelatorioGerencial("imprimir", cabecalhoDAV.ToString());

                FuncoesECF.RelatorioGerencial("imprimir", "Cód.  Des.      Unid.   Qtd.  Preço   Desc   Total " + Environment.NewLine);
                FuncoesECF.RelatorioGerencial("imprimir", "--------------------------------------------------------" + Environment.NewLine);
                foreach (var item in dados)
                {
                    if (item.cancelado == "N")
                    {
                        FuncoesECF.RelatorioGerencial("imprimir", item.codigo + " " + item.produto.TrimEnd() + Environment.NewLine
                        + "                   " + item.unidade + " " + item.quantidade.ToString() + " " + item.precooriginal.ToString() + " " + item.descontovalor.ToString() + " " + item.total.ToString() + Environment.NewLine + Environment.NewLine);
                        FuncoesECF.RelatorioGerencial("imprimir", " ");
                        cabecalhoDAV.AppendLine("");
                    }
                    if (item.cancelado == "S")
                    {
                        FuncoesECF.RelatorioGerencial("imprimir", item.codigo + "" + item.produto + " **CANCELADO** "
                        + "                  " + item.unidade + " " + item.quantidade.ToString() + " " + item.precooriginal.ToString() + " " + item.descontovalor.ToString() + " " + item.total.ToString() + Environment.NewLine + Environment.NewLine);
                        FuncoesECF.RelatorioGerencial("imprimir", " ");
                        cabecalhoDAV.AppendLine("");
                    }

                    totalBruto = totalBruto + item.total;
                    totalLiquido = totalLiquido + item.total - item.rateioDesconto;
                }

                rodape.AppendLine("---------------------------------------------------------" + Environment.NewLine);
                rodape.AppendLine("TOTAL BRUTO   R$:" + totalBruto.ToString());
                rodape.AppendLine("DESCONTO      R$:" + contdoc.desconto.ToString());
                rodape.AppendLine("TOTAL LÍQUIDO R$:" + totalLiquido.ToString());

                rodape.AppendLine("");
                rodape.AppendLine(" ");

                rodape.AppendLine("Data Entrega.:" + contdoc.data.Value.ToString("dd-MM-yyyy") + Environment.NewLine + Environment.NewLine);
                rodape.AppendLine("Ass.:______________________________________");

                //Itens.AppendLine("É vedada a autenticação deste documento.");


                FuncoesECF.RelatorioGerencial("imprimir", rodape.ToString());
                FuncoesECF.RelatorioGerencial("fechar", "");

                if (!ConfiguracoesECF.pdv)
                    ConfiguracoesECF.idECF = 0;
                return true;
            }
            else
            {
                return true;
            }
        }

        public decimal verificarQuantidadeDigitada(string codigo)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            var quantidade = (from q in entidade.vendas
                              where q.codigo == codigo && q.codigofilial == GlbVariaveis.glb_filial /*&& q.id == GlbVariaveis.glb_IP*/ && q.cancelado == "N"
                              select q.quantidade).ToList().Sum();
            return quantidade;
        }

        public static bool atualizarEstoqueDAV()
        {
            try
            {
                siceEntities entidade = Conexao.CriarEntidade();
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                var itensVendas = (from c in entidade.vendas where c.codigofilial == GlbVariaveis.glb_filial && c.id == GlbVariaveis.glb_IP && c.cancelado == "N" select c).ToList();
                var qtdEstoque = (from p in entidade.produtos select p.quantidade).FirstOrDefault();
                    
                foreach (var item in itensVendas)
                {
                    var quantidadeProdutoVenda = (from v in entidade.vendas where v.codigo == item.codigo && v.codigofilial == item.codigofilial && v.id == GlbVariaveis.glb_IP && v.cancelado == "N" && v.nrcontrole == item.nrcontrole select v.quantidade).Sum();
                    var produtoVenda = (from v in entidade.vendas where v.inc == item.inc select v).FirstOrDefault();
                    var produtosTransferencia = entidade.ExecuteStoreQuery<decimal>("SELECT ifNULL(quantidade,0) FROM transfvendatemp WHERE codigo = '" + item.codigo + "' AND filialdestino = '" + GlbVariaveis.glb_filial +"' AND numeroItem = '"+item.nrcontrole+"' AND ip ='"+GlbVariaveis.glb_IP+"' and cancelado = 'N'").FirstOrDefault();


                    if(GlbVariaveis.glb_filial == "00001")
                        qtdEstoque = (from p in entidade.produtos where p.codigo == item.codigo && p.CodigoFilial == item.codigofilial select p.quantidade).FirstOrDefault();
                    else
                        qtdEstoque = (from p in entidade.produtosfilial where p.codigo == item.codigo && p.CodigoFilial == item.codigofilial select p.quantidade).FirstOrDefault();

                    produtoVenda.quantidadeatualizada = ((qtdEstoque + produtosTransferencia) - quantidadeProdutoVenda);
                    entidade.SaveChanges();
                }


                if (Configuracoes.gerarTransferenciaVenda == true)
                {
                    var itensTransferencia = entidade.ExecuteStoreQuery<transfvenda>("SELECT * FROM transfvendatemp WHERE filialdestino = '" + GlbVariaveis.glb_filial + "' AND ip ='" + GlbVariaveis.glb_IP + "' and cancelado = 'N'").ToList();

                    foreach (var item in itensTransferencia)
                    {
                        if (item.filialorigem != "00001")
                            qtdEstoque = (from p in entidade.produtosfilial where p.codigo == item.codigo && p.CodigoFilial == item.filialorigem select p.quantidade).FirstOrDefault();
                        else
                            qtdEstoque = (from p in entidade.produtos where p.codigo == item.codigo && p.CodigoFilial == item.filialorigem select p.quantidade).FirstOrDefault();


                        string SQL = "UPDATE transfvendatemp SET quantidadeatualizada = " + Funcoes.FormatarDecimal((qtdEstoque - item.quantidade).ToString()).Replace(",",".") + ", quantidaestoqueorigem = "+ qtdEstoque.ToString().Replace(",",".") + " WHERE id = '" + item.id+"'";
                        entidade.ExecuteStoreCommand(SQL);

                    }

                    

                }

                return true;
            }
            catch(Exception erro)
            {
                //throw new Exception(erro.ToString());
                MessageBox.Show(erro.ToString());
                return false;
            }            
        }


        public bool verificarVendaIternet(string codigo, string codigoFilial)
        {
            siceEntities entidade = Conexao.CriarEntidade();

            if (codigoFilial == "00001")
            {
                var produtos = (from p in entidade.produtos
                                where p.codigo == codigo && p.CodigoFilial == codigoFilial
                                select p.vendainternet).FirstOrDefault();

                return produtos == "S" ? true : false; 
            }
            else
            {
                var produtos = (from p in entidade.produtosfilial
                                where p.codigo == codigo && p.CodigoFilial == codigoFilial
                                select p.vendainternet).FirstOrDefault();

                return produtos == "S" ? true : false;
            }


        }

        public static string TributacaoCupom(string tributacao,int aliquota,string tipoAliquota="ICMS")
        {
                        
            bool tributado = false;

            #region Aliquota ICMS ou ISS
            // Aqui atribui os valores do ICMS a variavel tributacao
            if (tributacao == "00" || tributacao == "02" || tributacao == "20" || tributacao == "101")
            {
                tributado = true;
                switch (aliquota)
                {
                    case 0:
                        tributacao = ConfiguracoesECF.icmsIsencaoII;
                        break;
                    default:
                        tributacao = Funcoes.FormatarZerosEsquerda(aliquota, 4, true); // aliquota.ToString() + "00";
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
                    case "41":
                    //Simples Nacional
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

            return tributacao;

        }

    }
}

public struct DadosEntrega
{
    public string recebedor;
    public string endereco;
    public string numero;
    public string cep;
    public string bairro;
    public string cidade;
    public string estado;
    public DateTime data;
    public DateTime hora;
    public string observacao;

}

public struct DadosConsumidorCupom
{
    public  string nomeConsumidor { get; set; }
    public  string cpfCnpjConsumidor { get; set; }
    public  string endConsumidor { get; set; }
    public  string endNumero { get; set; }
    public  string endBairro { get; set; }
    public  string endCidade { get; set; }
    public  string endEstado { get; set; }
    public  string endCEP { get; set; }
    public string idConsumidor { get; set; }
    public string ecfConsumidor { get; set; }
    public string ecfCNPJCPFConsumidor { get; set; }
}

public struct DadosDAVOS
{
    public string nrfabricacao;
    public string marca;
    public string modelo { get; set; }
    public int anoFabricacao { get; set; }
    public string placa { get; set; }
    public string renavam { get; set; }
}

public class auditoriaVenda
{
    public int inc { get; set; }
}