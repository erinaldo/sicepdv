using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data;
using System.Threading;
using System.Windows.Forms;
namespace SICEpdv
{
    class PreVenda
    {
        #region Pre-Venda

        public IQueryable<contprevendaspaf> RetornaPreVenda(int numeroPreVenda)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            var dados = from n in entidade.contprevendaspaf
                        where n.numeroDAVFilial==numeroPreVenda && n.codigofilial == GlbVariaveis.glb_filial
                        select n;
            return dados;

        }

        public static bool MontarPreVenda(int numeroPreVenda)
        {
            try
            {
                using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "siceEntities.ProcessarPreVenda";
                    cmd.CommandType = CommandType.StoredProcedure;

                    EntityParameter numero = cmd.Parameters.Add("numeroPreVenda", DbType.Int32);
                    numero.Direction = ParameterDirection.Input;
                    numero.Value = numeroPreVenda;

                    EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                    ip.Direction = ParameterDirection.Input;
                    ip.Value = GlbVariaveis.glb_IP;

                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível processar a pré-venda: "+e.Message.ToString());
            }
            

            return true;
        }

        public int FinalizarPreVenda(int numeroPreVenda,decimal total,decimal desconto,int idCliente,bool gerarPreVenda,bool processarPagamento=true)
        {
            string classe = "";
            int nrDevolucao = 0;
            decimal davEncargos = 0;
            string nomeConsumidor = " ";
            string docConsumidor = " ";
                string endConsumidor = " ";

            if (numeroPreVenda > 0)
            {
                var dadosPrevenda = (from n in Conexao.CriarEntidade().contprevendaspaf
                                     where n.numeroDAVFilial == numeroPreVenda 
                                     && n.codigofilial == GlbVariaveis.glb_filial
                                     select n).First();

                // Construindo a classe venda 
                DadosEntrega entrega = new DadosEntrega();
                entrega.recebedor = dadosPrevenda.responsavelreceber ?? "";
                entrega.endereco = dadosPrevenda.enderecoentrega ?? "";
                entrega.cep = dadosPrevenda.cepentrega ?? "";
                entrega.bairro = dadosPrevenda.bairroentrega ?? "";
                entrega.numero = dadosPrevenda.numeroentrega ?? "";
                entrega.cidade = dadosPrevenda.cidadeentrega ?? "";
                entrega.estado = dadosPrevenda.estadoentrega ?? "";                
                entrega.hora = dadosPrevenda.horaentrega == null ? Convert.ToDateTime(string.Format("{0:HH:mm}", DateTime.Now.TimeOfDay.ToString())) : Convert.ToDateTime(string.Format("{0:HH:mm}", dadosPrevenda.horaentrega.ToString()));
                entrega.observacao = dadosPrevenda.observacao ?? "";
                classe = dadosPrevenda.classe;
                nrDevolucao = dadosPrevenda.devolucao ?? 0;
                davEncargos = dadosPrevenda.encargos;
                Venda.dadosEntrega = entrega;
                Vendedor.VendaVendedor(dadosPrevenda.vendedor);      
                // dados Consumidor
                nomeConsumidor = dadosPrevenda.ecfCPFCNPJconsumidor;
                docConsumidor = dadosPrevenda.ecfCPFCNPJconsumidor;
                endConsumidor = dadosPrevenda.enderecoentrega ?? "";
            }
             
            Venda venda = new Venda();
                       
            venda.dpFinanceiro = "Venda";
            venda.valorBruto = total+desconto;
            venda.desconto = desconto;
            venda.idCliente = idCliente;
            venda.valorLiquido = total;
            venda.numeroPreVenda = numeroPreVenda;
            venda.classeVenda = classe;
            venda.encargos = davEncargos;
            venda.numeroDevolucao = nrDevolucao;     
            

            if (gerarPreVenda == true)
            {
                
                Venda objVenda = new Venda();
                // Executa o método da Venda.Finalizar para gerar a nova pré-venda    
                numeroPreVenda = objVenda.Finalizar(true,false,false);
                if (numeroPreVenda == 0)
                {
                    throw new Exception("Não foi possível gerar a nova pré-venda");
                }
                // Desativa a geração da Pré-venda e assume o novo número da pré-venda
                gerarPreVenda = false;
                venda.numeroPreVenda = numeroPreVenda;
                MontarPreVenda(numeroPreVenda);
            }

            try
            {                

                if (idCliente > 0)
                {
                    var dadosCliente = (from n in Conexao.CriarEntidade().clientes
                                        where n.Codigo == idCliente
                                        select new { n.Nome, n.cnpj, n.cpf, n.endereco, n.situacao, n.restritiva, n.credito, n.saldo }).First();
                    nomeConsumidor = dadosCliente.Nome;
                    docConsumidor = dadosCliente.cpf.Trim() ?? " " + dadosCliente.cnpj ?? " ";
                    endConsumidor = dadosCliente.endereco ?? " ";

                    if (!Permissoes.venderClienteRestricao && dadosCliente.restritiva == "S")
                    {

                        FrmLogon Logon = new FrmLogon();
                        Operador.autorizado = false;
                        Logon.idCliente = idCliente;
                        Logon.campo = "clirestricao";
                        Logon.lblDescricao.Text = "CLIENTE C/ RESTRIÇÃO";
                        Logon.txtDescricao.Text =
                        dadosCliente.Nome + Environment.NewLine + Environment.NewLine +
                        "Situação           : " + dadosCliente.situacao;

                        Logon.ShowDialog();
                        if (!Operador.autorizado)
                        {
                            throw new Exception("Sem autorização, cliente com restrição: " + dadosCliente.situacao);
                        }
                        else
                        {
                            siceEntities entidade = Conexao.CriarEntidade();
                            auditoria objAuditoria = new auditoria();
                            objAuditoria.acao = "Finalizando Pre-Venda";
                            objAuditoria.CodigoFilial = GlbVariaveis.glb_filial;
                            objAuditoria.codigoproduto = "0";
                            objAuditoria.data = DateTime.Now.Date;
                            objAuditoria.documento = 0;
                            objAuditoria.hora = DateTime.Now.TimeOfDay;
                            objAuditoria.local = "SICE.pdv";
                            objAuditoria.observacao = "CLIENTE C/ RESTRIÇÃO";
                            Logon.txtDescricao.Text = dadosCliente.Nome + Environment.NewLine + Environment.NewLine + "Situação           : " + dadosCliente.situacao;
                            objAuditoria.usuario = Operador.ultimoOperadorAutorizado;
                            objAuditoria.tabela = "Geral";
                            objAuditoria.usuariosolicitante = GlbVariaveis.glb_Usuario;
                            entidade.AddToauditoria(objAuditoria);
                            entidade.SaveChanges();

                            try
                            {
                                var lisCodigo = (from a in entidade.auditoria
                                                 where a.acao == "Finalizando Pre-Venda" && a.usuario == Operador.ultimoOperadorAutorizado && a.local == "SICE.pdv" && a.data == objAuditoria.data
                                                 select a.id).ToList().Max();

                                auditoriaVenda n = new auditoriaVenda();
                                n.inc = int.Parse(lisCodigo.ToString());
                                Venda.listAuditoriaVenda.Add(n);
                            }
                            catch (Exception erro)
                            {

                            }
                        }

                    }

                    if (dadosCliente.credito != 0 && (dadosCliente.saldo < total))
                    {
                        FrmLogon Logon = new FrmLogon();
                        Operador.autorizado = false;
                        Logon.idCliente = idCliente;
                        Logon.campo = "clisaldo";
                        Logon.lblDescricao.Text = "SALDO INSUFICIENTE";
                        Logon.txtDescricao.Text =
                         dadosCliente.Nome + Environment.NewLine + Environment.NewLine +
                        "SALDO DISPONÍVEL        : " + string.Format("{0:C2}", dadosCliente.saldo) + Environment.NewLine +
                        "VALOR ULTRAPASSADO : " + string.Format("{0:C2}", total - dadosCliente.saldo);
                        Logon.ShowDialog();
                        if (!Operador.autorizado)
                        {
                            throw new Exception("Saldo não autorizado");
                        }
                        else
                        {
                            siceEntities entidade = Conexao.CriarEntidade();
                            auditoria objAuditoria = new auditoria();
                            objAuditoria.acao = "Finalizando Pre-Venda";
                            objAuditoria.CodigoFilial = GlbVariaveis.glb_filial;
                            objAuditoria.codigoproduto = "0";
                            objAuditoria.data = DateTime.Now.Date;
                            objAuditoria.documento = 0;
                            objAuditoria.hora = DateTime.Now.TimeOfDay;
                            objAuditoria.local = "SICE.pdv";
                            objAuditoria.observacao = "CLIENTE C/ RESTRIÇÃO";
                            Logon.txtDescricao.Text = dadosCliente.Nome + Environment.NewLine + Environment.NewLine + "SALDO DISPONÍVEL        : " + string.Format("{0:C2}", dadosCliente.saldo) + Environment.NewLine + "VALOR ULTRAPASSADO : " + string.Format("{0:C2}", total - dadosCliente.saldo);
                            objAuditoria.usuario = Operador.ultimoOperadorAutorizado;
                            objAuditoria.tabela = "Geral";
                            objAuditoria.usuariosolicitante = GlbVariaveis.glb_Usuario;
                            entidade.AddToauditoria(objAuditoria);
                            entidade.SaveChanges();

                            try
                            {
                                var lisCodigo = (from a in entidade.auditoria
                                                 where a.acao == "Finalizando Pre-Venda" && a.usuario == Operador.ultimoOperadorAutorizado && a.local == "SICE.pdv" && a.data == objAuditoria.data
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

                }


                FuncoesECF fecf = new FuncoesECF();
                try
                {
                fecf.AbrirCupom("" ,nomeConsumidor,docConsumidor,endConsumidor);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                var itens = venda.SelectionaItensVenda();
                bool erroImpressao = false;
                foreach (var item in itens)
                {
                    decimal acrescimoValor = 0;
                    if (item.preco > item.precooriginal)
                        acrescimoValor = decimal.Round((item.preco.Value - item.precooriginal) * item.quantidade, 2);



                    try
                    {
                        fecf.VenderItemECF(item.codigo, item.produto, item.precooriginal, item.Descontoperc, item.descontovalor, 0, acrescimoValor, item.quantidade, item.unidade, item.tributacao, item.icms, item.tipo, item.ncm, Produtos.codigocestProdutos(item.codigo,item.codigofilial));
                        Thread.Sleep(200);
                        if (item.cancelado == "S")
                            fecf.ApagarItemECF(item.nrcontrole);
                    }                    
                    catch (Exception ex)
                    {
                        siceEntities entidade = Conexao.CriarEntidade();
                        var dados = (from n in entidade.vendas
                                     where n.id == GlbVariaveis.glb_IP
                                     && n.nrcontrole == item.nrcontrole
                                     select n).FirstOrDefault();
                        dados.cancelado = "S";                        
                        entidade.SaveChanges();
                        MessageBox.Show(item.codigo+" "+item.produto + ex.Message);
                        erroImpressao = true;
                    }                    

                }

                if (erroImpressao)
                    throw new Exception("Erro na impressão dos itens. exclua o cupom ! ");
                
                
                // Aqui imprime os itens mas volta para o PDV para lançamento das formas de pagamentos
                if (processarPagamento == false)
                    return numeroPreVenda;

                //fecf.IniciarFechamentoECF(desconto,venda.encargos);
                Thread.Sleep(200);

                var formaPagamento = venda.SelecionaPagamentoVenda().ToList();

                var pagCartao = (from n in formaPagamento
                                 where n.tipopagamento == "CA"
                                 select n).FirstOrDefault();


                int? idCartao = 0;
                decimal? valor = 0;

                if (pagCartao != null)
                {
                    idCartao = (from n in Conexao.CriarEntidade().cartoes
                                where n.descricao == pagCartao.Cartao
                                select n.id).FirstOrDefault();

                    valor = (from n in venda.SelecionaPagamentoVenda()
                             where n.tipopagamento == "CA"
                             select n.valor).Sum();
                    TEF.ChamarGerenciador("CA", valor.Value, idCartao.Value);

                    if (TEF.valorAprovadoTEF == 0)
                        return 0;
                }

                venda.EfetuarPagamento("Venda", "Todas", 0, 0, 0, "", "", 0, DateTime.Now, 0, null, true, true, false, false);
                //venda.Finalizar(false, false,false);
            }
            catch (Exception erro)
            {
                Funcoes.TravarTeclado(false);
                MessageBox.Show(erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }


            return numeroPreVenda;
        }

        public bool CancelarPreVenda(int numeroPreVenda)
        {            
            siceEntities entidade = Conexao.CriarEntidade();

            var dadosPreVenda = (from n in entidade.contprevendaspaf
                                 where n.numeroDAVFilial == numeroPreVenda
                                 && n.codigofilial == GlbVariaveis.glb_filial
                                 select new {n.numeroDAVFilial,n.valor, n.desconto, n.encargos,n.codigocliente }).First();

            
            Venda objVenda = new Venda();
            PreVenda objPreVenda = new PreVenda();
            PreVenda.MontarPreVenda(dadosPreVenda.numeroDAVFilial);

            objPreVenda.FinalizarPreVenda(dadosPreVenda.numeroDAVFilial,dadosPreVenda.valor, 0, 0, false, true);

            var COO = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).ToString();
            var CCF = FuncoesECF.CCFContadorCupomECF();


            contprevendaspaf dados = (from n in entidade.contprevendaspaf
                                      where n.numeroDAVFilial == numeroPreVenda
                                      select n).First();
            dados.cancelada = "S";
            dados.numeroECF = ConfiguracoesECF.numeroECF;
            dados.ecfcontadorcupomfiscal = CCF;
            //?
            dados.datafinalizacao = GlbVariaveis.Sys_Data.Date;
            dados.ncupomfiscal = COO;
            entidade.SaveChanges();
            string sql = "SELECT documento FROM contdocs WHERE documento=(SELECT MAX(documento) from contdocs)";
            int doc = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql).FirstOrDefault();
            FuncoesECF.CancelarCupomECF();
            COO = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).ToString();
            CCF = FuncoesECF.CCFContadorCupomECF();
            objVenda.ExcluirDocumento(COO, CCF, doc, "Cancelamento PV");
            return true;
        }
        // Aqui estou usando sobrecarga de método 
        // Pois existe 2 métodos um com parametro e outro sem para excluir todas as pré-vendas
        public static bool Cancelar(int dias = -1)
        {

            // Verifica se existem pré-venda anteriores nao encerradas
            try
            {

                siceEntities entidade = Conexao.CriarEntidade();
                PreVenda objprevenda = new PreVenda();

                DateTime dataAnterior = GlbVariaveis.Sys_Data.AddDays(dias);
                
                var prevendas = from n in Conexao.CriarEntidade().contprevendaspaf
                                where n.data <= dataAnterior
                                && n.cancelada == "N" && n.finalizada == "N"
                                select n;

                foreach (var item in prevendas)
                {                  
                    objprevenda.CancelarPreVenda(Convert.ToInt32(item.numeroDAVFilial));
                    System.Threading.Thread.Sleep(200);
                }                
            }
            catch
            {
                Funcoes.TravarTeclado(false);
                return false;
            }
            Funcoes.TravarTeclado(false);

            return true;
        }
        #endregion

        #region DAV
        public IQueryable<contdav> RetornarDAV(int numeroDAV)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Geral.ToString())
            {
                var dados = from n in entidade.contdav
                            where n.numeroDAVFilial == numeroDAV && n.codigofilial == GlbVariaveis.glb_filial
                            select n;
                return dados;
            }           

            return null;

        }

        public IQueryable<contdavos> RetornarDAVOS(int numeroDAV)
        {
            siceEntities entidade = Conexao.CriarEntidade();

            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
            {
                var dados = from n in entidade.contdavos
                            where n.numeroDAVFilial == numeroDAV && n.codigofilial == GlbVariaveis.glb_filial
                            select n;
                return dados;
            }

            return null;

        }

        public static bool MontarDAV(int numeroPreVenda)
        {
            string procedure = "siceEntities.ProcessarDAV";
            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                procedure = "siceEntities.ProcessarDAVOS";

            try
            {
                /*using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                {                    
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandText = procedure;
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    EntityParameter numero = cmd.Parameters.Add("numeroDAV", DbType.Int32);
                    numero.Direction = ParameterDirection.Input;
                    numero.Value = numeroPreVenda;

                    EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                    ip.Direction = ParameterDirection.Input;
                    ip.Value = GlbVariaveis.glb_IP;

                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }*/

                string SQL = "START TRANSACTION;" +
                             "CALL ProcessarDAV('" + numeroPreVenda + "','" + GlbVariaveis.glb_IP + "','" + GlbVariaveis.glb_filial + "');" +
                             "COMMIT";
                Conexao.CriarEntidade().ExecuteStoreCommand(SQL);
            }
            catch(Exception erro)
            {
                //throw new Exception("Não foi possível processar o DAV");
                MessageBox.Show(erro.ToString());
            }


            return true;
        }

        public int FinalizarDAV(int numeroDAV, decimal total, decimal desconto, int idCliente, bool gerarDAV,bool processarPagamento=true, int numeroDAVEntrega = 0)
        {
            string classe = "";
            int nrDevolucao = 0;
            decimal davEncargos = 0;

            string nomeConsumidor = " ";
            string docConsumidor = " ";
            string endConsumidor = " ";
            
            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Geral.ToString())
            {
                if (numeroDAV > 0 || numeroDAVEntrega > 0)
                {
                    var dadosDAV = (from n in Conexao.CriarEntidade().contdav
                                    where (n.numeroDAVFilial == numeroDAV || n.numeroDAVFilial == numeroDAVEntrega)
                                    && n.codigofilial == GlbVariaveis.glb_filial
                                    select n).First();
                    // Construindo a classe venda

                    DadosEntrega entrega = new DadosEntrega();
                    entrega.recebedor = dadosDAV.responsavelreceber ?? "";
                    entrega.endereco = dadosDAV.enderecoentrega ?? "";
                    entrega.cep = dadosDAV.cepentrega ?? "";
                    entrega.bairro = dadosDAV.bairroentrega ?? "";
                    entrega.numero = dadosDAV.numeroentrega ?? "";
                    entrega.cidade = dadosDAV.cidadeentrega ?? "";
                    entrega.estado = dadosDAV.estadoentrega ?? "";
                    entrega.hora = dadosDAV.horaentrega == null ? Convert.ToDateTime(string.Format("{0:HH:mm}", DateTime.Now.TimeOfDay.ToString())) : Convert.ToDateTime(string.Format("{0:HH:mm}", dadosDAV.horaentrega.ToString()));
                    entrega.observacao = dadosDAV.observacao ?? "";
                    nomeConsumidor = dadosDAV.cliente ?? " ";
                    docConsumidor = dadosDAV.ecfCPFCNPJconsumidor ?? " ";
                    endConsumidor = Venda.dadosConsumidor.endConsumidor;
                    Vendedor.VendaVendedor(dadosDAV.vendedor);
                    Venda.dadosEntrega = entrega;
                    nrDevolucao = dadosDAV.devolucao ?? 0;
                    davEncargos = dadosDAV.encargos;
                    if(string.IsNullOrEmpty(Venda.IQCard))
                    Venda.IQCard = dadosDAV.cartaofidelidade;
                }
            }

            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
            {
                if (numeroDAV > 0)
                {
                    var dadosDAV = (from n in Conexao.CriarEntidade().contdavos
                                    where n.numeroDAVFilial == numeroDAV
                                    && n.codigofilial == GlbVariaveis.glb_filial
                                    select n).First();
                    // Construindo a classe venda

                    DadosEntrega entrega = new DadosEntrega();
                    entrega.recebedor = dadosDAV.responsavelreceber ?? "";
                    entrega.endereco = dadosDAV.enderecoentrega ?? "";
                    entrega.cep = dadosDAV.cepentrega ?? "";
                    entrega.bairro = dadosDAV.bairroentrega ?? "";
                    entrega.numero = dadosDAV.numeroentrega ?? "";
                    entrega.cidade = dadosDAV.cidadeentrega ?? "";
                    entrega.estado = dadosDAV.estadoentrega ?? "";
                    entrega.hora = dadosDAV.horaentrega == null ? Convert.ToDateTime(string.Format("{0:HH:mm}", DateTime.Now.TimeOfDay.ToString())) : Convert.ToDateTime(string.Format("{0:HH:mm}", dadosDAV.horaentrega.ToString()));
                    entrega.observacao = dadosDAV.observacao ?? "";
                    Vendedor.VendaVendedor(dadosDAV.vendedor);
                    Venda.dadosEntrega = entrega;                    
                    nrDevolucao = dadosDAV.devolucao ?? 0;
                    davEncargos = dadosDAV.encargos;
                   
                }
            }

            Venda venda = new Venda();           
            venda.dpFinanceiro = "Venda";
            venda.valorBruto = total + desconto;
            venda.desconto = desconto;
            venda.idCliente = idCliente;
            venda.valorLiquido = total;
            venda.numeroDAV = numeroDAV;
            venda.classeVenda = classe;
            venda.encargos = davEncargos;
            venda.numeroDevolucao = nrDevolucao;
            

            if (gerarDAV == true)
            {                
                Venda objVenda = new Venda();                
                // Executa o método da Venda.Finalizar para gerar o novo DAV  
                 
                numeroDAV = objVenda.Finalizar(false,true,false);
                if (numeroDAV == 0)
                {
                    throw new Exception("Não foi possível gerar o novo DAV.");
                }
                // Desativa a geração do DAV e assume o novo número do DAV
                gerarDAV = false;
                venda.numeroDAV = numeroDAV;
                MontarDAV(numeroDAV);

            }

            try
            {
                if (idCliente > 0)
                {
                    /*var dadosCliente = (from n in Conexao.CriarEntidade().clientes
                                        where n.Codigo == idCliente
                                        select new { n.Nome,
                                            n.cnpj,
                                            n.cpf,
                                            n.endereco,
                                            n.situacao,
                                            n.restritiva,
                                            n.credito, n.saldo }).First();
                    */
                    Clientes dadosCliente = new Clientes();
                   dadosCliente.Procura(Convert.ToString(idCliente));

                    nomeConsumidor = dadosCliente.nome;
                    docConsumidor = dadosCliente.cpf.Trim() ?? " " + dadosCliente.cnpj ?? " ";
                    endConsumidor = dadosCliente.endereco ?? " ";
                    if (dadosCliente.restritiva == "S")
                    {
                        FrmLogon Logon = new FrmLogon();
                        Operador.autorizado = false;
                        Logon.idCliente = idCliente;
                        Logon.campo = "clirestricao";
                        Logon.lblDescricao.Text = "CLIENTE COM RESTRIÇÃO";
                        Logon.txtDescricao.Text =
                         dadosCliente.nome + Environment.NewLine + Environment.NewLine +
                        "SITUAÇÃO        : " + dadosCliente.situacao + Environment.NewLine;                        
                        Logon.ShowDialog();
                        if (!Operador.autorizado)
                        {
                            throw new Exception("Cliente com restrição: " + dadosCliente.situacao);
                        }
                        else
                        {
                            siceEntities entidade = Conexao.CriarEntidade();
                            auditoria objAuditoria = new auditoria();
                            objAuditoria.acao = "Finalizando DAV";
                            objAuditoria.CodigoFilial = GlbVariaveis.glb_filial;
                            objAuditoria.codigoproduto = "0";
                            objAuditoria.data = DateTime.Now.Date;
                            objAuditoria.documento = 0;
                            objAuditoria.hora = DateTime.Now.TimeOfDay;
                            objAuditoria.local = "SICE.pdv";
                            objAuditoria.observacao = dadosCliente.nome + Environment.NewLine + Environment.NewLine + "SITUAÇÃO        : " + dadosCliente.situacao + Environment.NewLine;  
                            objAuditoria.usuario = Operador.ultimoOperadorAutorizado;
                            objAuditoria.tabela = "Geral";
                            objAuditoria.usuariosolicitante = GlbVariaveis.glb_Usuario;
                            entidade.AddToauditoria(objAuditoria);
                            entidade.SaveChanges();

                            try
                            {
                                var lisCodigo = (from a in entidade.auditoria
                                                 where a.acao == "Finalizando DAV" && a.usuario == Operador.ultimoOperadorAutorizado && a.local == "SICE.pdv" && a.data == objAuditoria.data
                                                 select a.id).ToList().Max();

                                auditoriaVenda n = new auditoriaVenda();
                                n.inc = int.Parse(lisCodigo.ToString());
                                Venda.listAuditoriaVenda.Add(n);
                            }
                            catch (Exception erro)
                            {

                            }
                        }

                    }
                        

                    if (dadosCliente.credito != 0 && (dadosCliente.saldo < total))
                    {
                        FrmLogon Logon = new FrmLogon();
                        Operador.autorizado = false;
                        Logon.idCliente = idCliente;
                        Logon.campo = "clisaldo";
                        Logon.lblDescricao.Text = "SALDO INSUFICIENTE";
                        Logon.txtDescricao.Text =
                         dadosCliente.nome + Environment.NewLine + Environment.NewLine +
                        "SALDO DISPONÍVEL        : " + string.Format("{0:C2}", dadosCliente.saldo) + Environment.NewLine +
                        "VALOR ULTRAPASSADO : " + string.Format("{0:C2}", total - dadosCliente.saldo);
                        Logon.ShowDialog();
                        if (!Operador.autorizado)
                        {
                            throw new Exception("Saldo não autorizado.");
                        }
                        else
                        {
                            siceEntities entidade = Conexao.CriarEntidade();
                            auditoria objAuditoria = new auditoria();
                            objAuditoria.acao = "Finalizando DAV";
                            objAuditoria.CodigoFilial = GlbVariaveis.glb_filial;
                            objAuditoria.codigoproduto = "0";
                            objAuditoria.data = DateTime.Now.Date;
                            objAuditoria.documento = 0;
                            objAuditoria.hora = DateTime.Now.TimeOfDay;
                            objAuditoria.local = "SICE.pdv";
                            objAuditoria.observacao = Environment.NewLine + Environment.NewLine + "SALDO DISPONÍVEL        : " + string.Format("{0:C2}", dadosCliente.saldo) + Environment.NewLine + "VALOR ULTRAPASSADO : " + string.Format("{0:C2}", total - dadosCliente.saldo);
                            objAuditoria.usuario = Operador.ultimoOperadorAutorizado;
                            objAuditoria.tabela = "Geral";
                            objAuditoria.usuariosolicitante = GlbVariaveis.glb_Usuario;
                            entidade.AddToauditoria(objAuditoria);
                            entidade.SaveChanges();

                            try
                            {
                                var lisCodigo = (from a in entidade.auditoria
                                                 where a.acao == "Finalizando DAV" && a.usuario == Operador.ultimoOperadorAutorizado && a.local == "SICE.pdv" && a.data == objAuditoria.data
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
                }

                FuncoesECF fecf = new FuncoesECF();                

                try
                {
                    FuncoesECF.VerificaImpressoraLigada();
                    fecf.AbrirCupom("", nomeConsumidor, docConsumidor, endConsumidor);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                var itens = venda.SelectionaItensVenda();
                
                bool erroImpressao = false;
                foreach (var item in itens)
                {
                    decimal acrescimoValor = 0;
                    if (item.preco > item.precooriginal)
                        acrescimoValor = decimal.Round((item.preco.Value - item.precooriginal) * item.quantidade, 2);

                    try
                    {
                        Thread.Sleep(100);
                        fecf.VenderItemECF(item.codigo, item.produto, item.precooriginal, item.Descontoperc, item.descontovalor, 0, acrescimoValor, item.quantidade, item.unidade, item.tributacao, item.icms, item.tipo, item.ncm,Produtos.codigocestProdutos(item.codigo,item.codigofilial));
                        Thread.Sleep(300);
                        if (item.cancelado == "S")
                            fecf.ApagarItemECF(item.nrcontrole);                        
                    }
                    catch (Exception ex)
                    {
                        siceEntities entidade = Conexao.CriarEntidade();
                        var dados = (from n in entidade.vendas
                                     where n.id == GlbVariaveis.glb_IP
                                     && n.nrcontrole == item.nrcontrole
                                     select n).FirstOrDefault();
                        dados.cancelado = "S";                        
                        entidade.SaveChanges();
                        MessageBox.Show(item.codigo+" "+item.produto +"   "+ ex.Message);
                        erroImpressao = true;
                    }

                       
                }

                if (erroImpressao)
                    throw new Exception("Erro na impressão dos itens. exclua o cupom ! ");
                    

                // Aqui imprime os itens mas volta para o PDV para lançamento das formas de pagamentos
                if (processarPagamento == false)
                {
                    return numeroDAV;
                }


               // fecf.IniciarFechamentoECF(desconto,venda.encargos);
                Thread.Sleep(200);

                var formaPagamento = venda.SelecionaPagamentoVenda().ToList();

                var pagCartao = (from n in formaPagamento
                                where n.tipopagamento == "CA"
                                select n).FirstOrDefault();
               

                int? idCartao = 0;
                decimal? valor = 0;

                if (pagCartao!=null )
                {
                    idCartao = (from n in Conexao.CriarEntidade().cartoes
                                where n.descricao == pagCartao.Cartao
                                select n.id).FirstOrDefault();

                    valor = (from n in venda.SelecionaPagamentoVenda()
                             where n.tipopagamento == "CA"
                             select n.valor).Sum();

                    try
                    {
                        if (ConfiguracoesECF.tefDiscado && TEF.ChamarGerenciador("CA", valor.Value, idCartao.Value) == 0)
                        {
                            throw new Exception("Transação zerada, finalize com outra forma de pagamento");
                        }
                    }
                    catch 
                    {
                        if (Configuracoes.cancelarvendarejeicaocartao == true)
                        {
                            _pdv frmPDV = new _pdv();
                            frmPDV.perguntarCancelamento = false;
                            frmPDV.btnCancelarCupom_Click(this, null);
                        }
                        return 0;
                    }

                    if (ConfiguracoesECF.tefDiscado && TEF.valorAprovadoTEF == 0)
                        return 0;
                }

                try
                {
                    venda.EfetuarPagamento("Venda", "Todas", 0, 0, idCartao.Value, "", "", 0, DateTime.Now, 0, null, true, true, false, false);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                //venda.Finalizar(false, false,false);
                
            }
            catch (Exception erro)
            {
                Funcoes.TravarTeclado(false);
                MessageBox.Show(erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }

            return numeroDAV;
        }

        public bool CancelarDAV(int numeroDAV)
        {
            //Update
            siceEntities entidade = Conexao.CriarEntidade();

            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Geral.ToString())
            {
                contdav dados = (from n in entidade.contdav
                                 where n.numeroDAVFilial == numeroDAV
                                 && n.codigofilial == GlbVariaveis.glb_filial
                                 select n).First();
                dados.finalizada = "S";
                dados.datafinalizacao = GlbVariaveis.Sys_Data.Date;
                entidade.SaveChanges();
            }

            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
            {
                contdavos dados = (from n in entidade.contdavos
                                 where n.numeroDAVFilial == numeroDAV
                                 && n.codigofilial == GlbVariaveis.glb_filial
                                 select n).First();
                dados.finalizada = "S";
                dados.datafinalizacao = GlbVariaveis.Sys_Data.Date;
                entidade.SaveChanges();
            }

            return true;
        }
    }
        #endregion
}
