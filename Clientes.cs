using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace SICEpdv
{
    class Clientes
    {
        public int idCliente { get; set; }
        public string nome { get; set; }        
        public string cpf { get; set; }
        public string cnpj { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public bool ativo { get; set; }
        public static string ultCPF { get; set; }
        public string restritiva { get; set; }
        /// <summary>
        /// Regra de Negócio Financeiro do Cliente
        /// por isso as variáveis são static
        /// </summary>
        /// 
        public decimal credito { get; set; }
        public decimal debito { get; set; }
        public decimal saldoAtual { get; set; }
        public decimal saldo { get; set; }
        public string situacao { get; set; }
        public bool situacaoRestritiva { get; set; }

        public static bool inadimplente { get; set; }
        public static bool limiteUltrapassado { get; set; }
        public static bool restricao { get; set; }

        public bool Procura(string valor)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2 && Conexao.ConexaoOnline() == false)
                entidade = Conexao.CriarEntidade(false);
       
            if (valor.Length == 0 || valor=="0")  return false;
                       
            Int64 procuraCod = 0;
            if (valor.Length <= 10)
                procuraCod = Convert.ToInt64(valor);
            //else
                //procuraCod = int.Parse(valor);

                    var dados = from c in entidade.clientes
                                where (c.Codigo == procuraCod || c.cpf == valor || c.cnpj == valor)
                                && c.ativo == "S"
                                select new
                                {
                                    codigo = c.Codigo,
                                    nome = c.Nome,
                                    cpf = c.cpf,
                                    cnpj = c.cnpj,
                                    endereco = c.endereco,
                                    numero = c.numero,
                                    bairro = c.bairro,
                                    cidade = c.cidade,
                                    estado = c.estado,
                                    cep = c.cep,
                                    situacao = c.situacao,
                                    ativo = c.ativo == "S" ? true : false,
                                    credito = c.credito,
                                    creditoProv = c.creditoprovisorio,
                                    debito = c.debito,
                                    debitoCH = c.debitoch,
                                    saldo = c.saldo,                                    
                                    restritiva = c.restritiva =="S" ? true : false
                                };

                    if (dados.Count() == 0)
                        return false;

                    foreach (var item in dados)
                    {
                        idCliente = item.codigo;
                        nome = item.nome;
                        cpf = item.cpf;
                        cnpj = item.cnpj;
                        endereco = item.endereco;
                        numero = item.numero;
                        cidade = item.cidade;
                        estado = item.estado;
                        cep = item.cep;
                        bairro = item.bairro;                        

                        situacaoRestritiva = item.restritiva;
                        credito = item.credito.GetValueOrDefault();
                        debito = item.debito.GetValueOrDefault();
                        saldoAtual = (credito - debito);//item.saldo.GetValueOrDefault();
                        if (!Configuracoes.abaterCRcompraCH)
                            saldoAtual = Convert.ToDecimal((item.credito + item.creditoProv) - (item.debito));

                        saldo = saldoAtual;
                        situacao = item.situacao;
                        restritiva = item.restritiva == true ? "S" : "N";
                    }                         
            return  true;            
        }


        public static IQueryable<crmovclientes> Extrato(int codigoCliente)
        {
            var dados = from n in Conexao.CriarEntidade().crmovclientes
                        where n.codigo == codigoCliente
                        && n.valoratual>0
                        select n;
            return dados;
        }

        public static int QuitarDebito(int codigoCliente,List<PagamentoParcelas> parcelas,decimal desconto,int devolucaoNumero)
        {

  
            siceEntities entidade = Conexao.CriarEntidade();
            string nomeCliente = (from n in entidade.clientes
                                 where n.Codigo == codigoCliente
                                 select n.Nome).FirstOrDefault();

            string tipoPagamento = (from n in Conexao.CriarEntidade().caixas 
                                    where n.EnderecoIP == GlbVariaveis.glb_IP 
                                    select n.tipopagamento).FirstOrDefault();

            // Imprimindo o doc não fiscal

            var pagamentos = (from n in Conexao.CriarEntidade().caixas
                             where n.EnderecoIP == GlbVariaveis.glb_IP
                             select new { n.tipopagamento, n.valor });

           

            if (TEF.Transacoes("ntransacao") > 0 && tipoPagamento.Contains("CA"))
            {
                System.Threading.Thread.Sleep(300);
                FuncoesECF fecf = new FuncoesECF();
                if (!fecf.ImprimirTEF(true))
                    return 0;
            }

            int documento = 0;
            decimal jurosRecebido = 0;
            decimal encargos = 0;
            decimal totalPagamento = parcelas.Sum(c => c.valorPagamento);
            string conteudoRecebimento = "REC. CONTA: "+codigoCliente.ToString()+" " +nomeCliente;

            FuncoesECF.ComprovanteNaoFiscal("abrir", "RECEBIMENTO", "", nomeCliente, "", tipoPagamento, totalPagamento.ToString(), desconto.ToString(), conteudoRecebimento);
            if (FuncoesECF.ComprovanteNaoFiscal("pagar", "RECEBIMENTO", "", nomeCliente, "", tipoPagamento, (totalPagamento - desconto).ToString(), "0,00", conteudoRecebimento) == false)
            {
                FuncoesECF.ComprovanteNaoFiscal("cancelar", "RECEBIMENTO", "", nomeCliente, "", tipoPagamento, totalPagamento.ToString(), desconto.ToString(), conteudoRecebimento);
            }
            System.Threading.Thread.Sleep(300);
            FuncoesECF.ComprovanteNaoFiscal("fechar", "RECEBIMENTO", "", "", "", "", "", desconto.ToString(), conteudoRecebimento);

            string vendedor = "000";
            decimal totalDevolucao = 0;
            if (devolucaoNumero > 0)
            {
                var valorDevolucao = (from n in entidade.devolucao
                                  where n.numero == devolucaoNumero
                                  select n.quantidade * n.preco).Sum();
                totalDevolucao = valorDevolucao.Value;
            };

            try
            {
                Funcoes.TravarTeclado(true);
                foreach (var item in parcelas)
                {
                    var update = (from n in entidade.crmovclientes
                                  where n.sequenciainc == item.idParcela
                                  && n.ip == GlbVariaveis.glb_IP
                                  select n).FirstOrDefault();
                    
                    update.quitado = "S";
                    update.ip = GlbVariaveis.glb_IP;
                    update.valorpago = item.valorPagamento;
                    update.vrjuros = update.vrjuros + update.jurosacumulado;
                    // Pegando valores
                    jurosRecebido += update.vrjuros;
                    encargos += update.encargos;
                    vendedor = update.vendedor;
                }
                entidade.SaveChanges();

                // Inserindo Documento
                contdocs registroDoc = new contdocs();
                registroDoc.bordero = "S";
                registroDoc.cartaofidelidade = "0";
                registroDoc.concluido = "N";
                registroDoc.entregaconcluida = "N";
                registroDoc.estadoentrega = "  ";
                registroDoc.estornado = "N";
                registroDoc.NF_e = "N";
                registroDoc.numeroentrega = "0";
                registroDoc.responsavelreceber = "";
                registroDoc.romaneio = "N";
                registroDoc.estoqueatualizado = "S";
                registroDoc.TEF = "N";
                registroDoc.prevendanumero = "";
                registroDoc.davnumero = 0;

                // Valores
                registroDoc.ip = GlbVariaveis.glb_IP;
                registroDoc.id = GlbVariaveis.glb_IP;
                registroDoc.CodigoFilial = GlbVariaveis.glb_filial;
                registroDoc.data = GlbVariaveis.Sys_Data;
                registroDoc.dataexe = GlbVariaveis.Sys_Data;
                registroDoc.Totalbruto = totalPagamento;
                registroDoc.desconto = desconto;
                registroDoc.encargos = encargos;
                registroDoc.total = totalPagamento - desconto;
                registroDoc.nome = nomeCliente;
                registroDoc.codigocliente = codigoCliente;
                registroDoc.NrParcelas = 0;
                registroDoc.vendedor = vendedor ?? "000" ;
                registroDoc.operador = GlbVariaveis.glb_Usuario;
                registroDoc.dpfinanceiro = "Recebimento";
                registroDoc.vrjuros = jurosRecebido;

                registroDoc.tipopagamento = tipoPagamento;
                registroDoc.tipopagamentoECF = tipoPagamento;
                registroDoc.devolucaorecebimento = totalDevolucao;
                registroDoc.classedevolucao = "";
                registroDoc.historico = "";
                registroDoc.hora = DateTime.Now.TimeOfDay;
                registroDoc.modeloDOCFiscal = "";
                registroDoc.COOGNF = FuncoesECF.ContadorNaoFiscalGNF();
                registroDoc.ncupomfiscal =  FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF);
                registroDoc.serienf = "";
                registroDoc.subserienf = "";
                registroDoc.devolucaonumero = devolucaoNumero;

                // Info fiscais;
                registroDoc.ecffabricacao = ConfiguracoesECF.nrFabricacaoECF;
                registroDoc.ecfmodelo = ConfiguracoesECF.modeloECF;
                registroDoc.ecfMFadicional = ConfiguracoesECF.mfAdicionalECF;
                registroDoc.contadordebitocreditoCDC = "";
                registroDoc.tipopagamentoECF = "RC";
                registroDoc.ecfnumero = ConfiguracoesECF.numeroECF;

                entidade.AddTocontdocs(registroDoc);
                entidade.SaveChanges();

                documento = (from n in entidade.contdocs
                             where n.ip == GlbVariaveis.glb_IP
                             select n.documento).Max();


                //Aqui para pegar apenas o valor das parcelas com vencimento em dias
                decimal? totalPontuacaoIQCard = 0;
                try
                {
                    if (Configuracoes.fidelizarRecebimento == "S" && !string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                    {
                        totalPontuacaoIQCard = Conexao.CriarEntidade().ExecuteStoreQuery<decimal?>("SELECT SUM(valorpago) FROM crmovclientes WHERE codigo='" + codigoCliente + "' AND quitado='S' AND ip='" + GlbVariaveis.glb_IP + "' AND vencimento>=CURRENT_DATE").FirstOrDefault();
                    }
                }
                catch (Exception erro)
                {

                }

                //MessageBox.Show("QuitarDebitoCliente - 1");
                using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "siceEntities.QuitarDebitoCliente";
                    cmd.CommandType = CommandType.StoredProcedure;

                    EntityParameter codCliente = cmd.Parameters.Add("codigoCliente", DbType.Int32);
                    codCliente.Direction = ParameterDirection.Input;
                    codCliente.Value = codigoCliente;

                    EntityParameter doc = cmd.Parameters.Add("doc", DbType.Int32);
                    doc.Direction = ParameterDirection.Input;
                    doc.Value = documento;

                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;

                    EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                    ip.Direction = ParameterDirection.Input;
                    ip.Value = GlbVariaveis.glb_IP;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                Venda.ultimoDocumento = documento;
                ///MessageBox.Show("QuitarDebitoCliente - 2");
                ///MessageBox.Show(documento.ToString());

                // Processando Devolucao
                if (devolucaoNumero > 0)
                {
                    using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                    {
                        conn.Open();
                        EntityCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "siceEntities.ProcessarDevolucao";
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
                        nrDevolucao.Value = devolucaoNumero;

                        EntityParameter operador = cmd.Parameters.Add("operadorAcao", DbType.String);
                        operador.Direction = ParameterDirection.Input;
                        operador.Value = GlbVariaveis.glb_Usuario;

                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                Funcoes.TravarTeclado(false);

                if (Configuracoes.fidelizarRecebimento == "S" && !string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                {
                    string IQCard = "";
                    if (Configuracoes.fidelizarRecebimento == "S")
                    {
                        IQCard = (from n in entidade.clientes
                                  where n.Codigo == codigoCliente
                                  select n.cartaofidelidade).FirstOrDefault();
                    }

                    if (string.IsNullOrEmpty(IQCard))
                    {
                        LancarPontosIQCARD.idCliente = codigoCliente;
                        LancarPontosIQCARD lancar = new LancarPontosIQCARD();
                        lancar.ShowDialog();                        
                        // Aqui atribui a variavel novamente por que foi gravada no evento gravar do form
                        IQCard = (from n in entidade.clientes
                                  where n.Codigo == codigoCliente
                                  select n.cartaofidelidade).FirstOrDefault();
                    }
                        

                    
                        IqCard iqcard = new IqCard();
                        try
                        {
                            var resultado = iqcard.LancarPontos(IQCard,documento,desconto,totalPontuacaoIQCard.GetValueOrDefault(), 0, 0, 0);
                        }
                        catch (Exception ex)
                        {
                            if (ex.ToString().ToLower().Contains("saldo insuficiente"))
                            {
                                MessageBox.Show("Saldo insuficiente de crédito para lançar pontos para o usuário. Acesse www.iqcard.com.br com sua conta e compre mais créditos");
                                if (ex.ToString().ToLower().Contains("O cliente não acumulou pontos")) ;
                                IqCard.saldoInsuficiente = true;
                            }

                            if (File.Exists("debugiqcard.txt"))
                            {
                                MessageBox.Show(ex.Message.ToString());

                            }

                        }
                    
                }


                if (MessageBox.Show("Imprimir Recibo ?", "SICEpdv", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    FrmMsgOperador msg = new FrmMsgOperador("", "Imprimindo recibo");
                    msg.Show();
                    Application.DoEvents();
                    try
                    {

                        if (!File.Exists(@"log_teste.txt"))
                        {
                            ImprimirRecibo(documento);
                        }
                        else
                        {
                            Process myProcess = System.Diagnostics.Process.Start(@"C:\iqsistemas\SICEPDV\ImpressorCupom.exe", " " + documento.ToString());
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

                    return documento;

                }                
            }
            catch (Exception erro)
            {

                throw new Exception(erro.Message);
            }
            finally
            {
                Funcoes.TravarTeclado(false);
            }



            return documento;
        }

        public static void ImprimirRecibo(int documento)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            string DocString = "s" + documento.ToString() + "s";

            var dadosPagamento = (from n in entidade.contdocs
                                  where n.documento == documento
                                  select new { n.Totalbruto, n.desconto,n.vrjuros,n.total }).FirstOrDefault();

            var dados = from n in entidade.crmovclientes
                        where n.sequencia.Contains(DocString)
                        select new {n.codigo,n.nome, n.documento, n.nrParcela, n.vencimento, n.Valor, n.vrultpagamento, n.valoratual,n.jurospago };

            StringBuilder sb = new StringBuilder();
            StringBuilder cb = new StringBuilder();
            StringBuilder rb = new StringBuilder();

            cb.AppendLine(Configuracoes.fantasia);
            cb.AppendLine("CNPJ: " + Configuracoes.cnpj);
            cb.AppendLine("Tel.: " + Configuracoes.telefone);
            cb.AppendLine("Data: " + DateTime.Now.ToString());
            cb.AppendLine("Oper: " + GlbVariaveis.glb_Usuario);
            cb.AppendLine("DOC.: " + documento.ToString() + "\n\r");
            cb.AppendLine("      R E C I B O ........... " + string.Format("{0:C2}", dadosPagamento.Totalbruto - dadosPagamento.desconto));
            cb.AppendLine("Valor recebido ref. a  situação dos títulos");
            cb.AppendLine("abaixo relacionados.");
            cb.AppendLine("Cliente: " + dados.First().codigo + " " + dados.First().nome + "\r\n");
            cb.AppendLine("DOC.PARC. VENC.  V.DOC. V.PAGO REST.");

            foreach (var item in dados)
            {
                sb.AppendLine(item.documento.ToString() + " " + item.nrParcela + " " + string.Format("{0:dd/MM/yyy}", item.vencimento) + " " + item.Valor.ToString() + " " + item.vrultpagamento.ToString() + " " + item.valoratual.ToString());
            }
            rb.AppendLine("---------------------------");
            rb.AppendLine("TOTAL BRUTO : " + string.Format("{0:C2}", dadosPagamento.Totalbruto));
            rb.AppendLine("(*)JUROS    : " + string.Format("{0:C2}", dadosPagamento.vrjuros));
            rb.AppendLine("(-)DESCONTOS: " + string.Format("{0:C2}", dadosPagamento.desconto));
            rb.AppendLine("VALOR PAGO  : " + string.Format("{0:C2}", dadosPagamento.total));

            FuncoesECF.RelatorioGerencial("abrir", "");

            for (int i = 0; i < 2; i++)
            {                            
           
            if (i == 1)
            {
                FuncoesECF.RelatorioGerencial("imprimir", " \r\n\r\n\r\n\r\n");            
                FuncoesECF.RelatorioGerencial("imprimir", "2a. via - Cliente \r\n\r\n");            
            }

            FuncoesECF.RelatorioGerencial("imprimir", cb.ToString());
            FuncoesECF.RelatorioGerencial("imprimir", sb.ToString()); 
            FuncoesECF.RelatorioGerencial("imprimir", rb.ToString());
                   
            }
            FuncoesECF.RelatorioGerencial("fechar", "");
        }

        public static bool AtualizarDebito(int codigo, decimal taxaJurosDiario)
        {
            try
            {
                using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "siceEntities.AtualizarDebitoCliente";
                    cmd.CommandType = CommandType.StoredProcedure;

                    EntityParameter codCliente = cmd.Parameters.Add("codCliente", DbType.Int32);
                    codCliente.Direction = ParameterDirection.Input;
                    codCliente.Value = codigo;

                    EntityParameter taxaJuros = cmd.Parameters.Add("taxaJuros", DbType.Double);
                    taxaJuros.Direction = ParameterDirection.Input;
                    taxaJuros.Value = taxaJurosDiario;

                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Atualizando Débito: " + ex.Message);
            }
        }
        
    }

    public struct PagamentoParcelas
    {
        public int codigoCliente;
        public int idParcela;
        public decimal valorPagamento;
        public decimal valorJuros;

    }
}
