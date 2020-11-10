using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SICEpdv
{
    class IqCard
    {
        public static List<ServiceReference1.ItensPedido> itensPedidoIQCard = new List<ServiceReference1.ItensPedido>();
        public static ServiceReference1.Pedido DadosPedido = new ServiceReference1.Pedido();
        public static ServiceReference1.ClienteCartao dadosCartao = new ServiceReference1.ClienteCartao();
        public static int contadorUsuarios = 0;
        public static int contadorItensDelivery = 0;
        public static DateTime horaVerificadoPedido;
        public static DateTime horaVerificadoPromocao;
        public static ServiceReference1.Parametros parametros = new ServiceReference1.Parametros();
        public static int saldoPontos = 0;
        public static bool mostrarPainelIQCARD = true;
        public static bool saldoInsuficiente = false;
        public static int usuariosProcurandoPromo = 0;
        public static int pontosIQCARD = 0;
        public static string idRegistroPontosIQCARD { get; set; }
        public static string voucherDesconto { get; set; }


        public static bool VerificarNumeroCartao(string creditCardNumber)
        {
            try
            {

                //Aqui passa se näao for o número com 16 se comecar com 0359 e a quantidade de caracteres for 16 então verificar sequencia do cartão.
                if (creditCardNumber.Length != 16)
                {
                    return true;
                }

                if (creditCardNumber.Length == 5 && creditCardNumber.Contains("#"))
                {
                    return true;
                };

                var reversedNumber = creditCardNumber.ToCharArray().Reverse();

                int mod10Count = 0;
                for (int i = 0; i < reversedNumber.Count(); i++)
                {
                    int augend = Convert.ToInt32(reversedNumber.ElementAt(i).ToString());

                    if (((i + 1) % 2) == 0)
                    {
                        string productstring = (augend * 2).ToString();
                        augend = 0;
                        for (int j = 0; j < productstring.Length; j++)
                        {
                            augend += Convert.ToInt32(productstring.ElementAt(j).ToString());
                        }
                    }
                    mod10Count += augend;
                }

                if ((mod10Count % 10) == 0)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public static string GerarCupomSorteio(int pontos)
        {


            string formatarPontos = Funcoes.FormatarZerosEsquerda(pontos, 5, false, 0);


            var cupomAleatorio = Guid.NewGuid().ToString();

            var criptar = Funcoes.CriptografarMD5(GlbVariaveis.glb_chaveIQCard + cupomAleatorio + formatarPontos);

            string senha = "";
            foreach (var item in criptar)
            {
                if (Char.IsNumber(item))
                {
                    senha += item.ToString();
                }
            }

            string numeroCupomSorteio = GlbVariaveis.glb_chaveIQCard + cupomAleatorio + formatarPontos + senha.Substring(0, 2);

            return numeroCupomSorteio;

        }



        public static string GerarCodigoPromocao()
        {

            var codigoAleatorio = Guid.NewGuid().ToString();

            var chave = "";

            if (!string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                chave = GlbVariaveis.glb_chaveIQCard;
            else
                chave = GlbVariaveis.idCliente.PadLeft(4, '0');




            var criptar = Funcoes.CriptografarMD5(codigoAleatorio);

            string codigo = "";
            foreach (var item in criptar)
            {
                if (Char.IsNumber(item))
                {
                    codigo += item.ToString();
                }
            }
            // Aqui por que sempre os primeiros 4 digitos sao o código do cliente IQ Sistemas
            codigo = GlbVariaveis.idCliente.PadLeft(4, '0') + codigo.Substring(0, 6);

            string codSeguranca = Funcoes.CriptografarMD5(codigo + chave);

            string codigoVerificador = "";
            foreach (var item in codSeguranca)
            {
                if (Char.IsNumber(item))
                {
                    codigoVerificador += item.ToString();
                }
            }

            string codigoGerado = codigo + codigoVerificador.Substring(0, 2);

            return codigoGerado;

        }

        public static ServiceReference1.Imagens DadosImagem(string codigo)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();

                var dados = WSIQCard.ImagemPorCodigo(GlbVariaveis.chavePrivada, codigo);
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string FormatarCartao(string cartao)
        {
            return cartao.Substring(0, 4) + ' ' + cartao.Substring(4, 4) + ' ' + cartao.Substring(8, 4) + ' ' + cartao.Substring(12, 4);
        }


        public static List<ServiceReference1.Pedido> PedidosIQCard(string idCartao)
        {
            ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
            var dados = WSIQCard.ListagemPedido(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, idCartao, 90);
            var abertos = (from n in dados where n.totalOrcamento > 0 orderby n.data select n);
            try
            {
                dadosCartao = WSIQCard.DadosCartao(GlbVariaveis.chavePrivada, idCartao);
            }
            catch (Exception)
            {

            }


            return abertos.ToList();
        }


        public static List<ServiceReference1.Pedido> PedidosIQCardPorStatus(string status)
        {
            ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
            var dados = WSIQCard.ListagemPedidoStatus(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, status);
            var abertos = (from n in dados orderby n.data select n);
            return abertos.ToList();
        }




        public static List<ServiceReference1.ItensPedido> AbrirPedido(string idPedido)
        {
            ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
            var dados = WSIQCard.ListagemItensPedido(GlbVariaveis.chavePrivada, idPedido);
            itensPedidoIQCard = dados.ToList();

            DadosPedido = WSIQCard.PegarPedido(GlbVariaveis.chavePrivada, idPedido);
            dadosCartao = WSIQCard.DadosCartao(GlbVariaveis.chavePrivada, DadosPedido.idCartao);
            WSIQCard.PedidoVisualizado(GlbVariaveis.chavePrivada, idPedido);
            return dados.ToList();
        }

        public static List<ServiceReference1.Pedido> PegarPedido(string idPedido)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var n = WSIQCard.PegarPedido(GlbVariaveis.chavePrivada, idPedido);
                List<ServiceReference1.Pedido> pedidoLista = new List<ServiceReference1.Pedido>();
                pedidoLista.Add(new ServiceReference1.Pedido { idCartao = n.idCartao, nomeCartao = n.nomeCartao, data = n.data, totalOrcamento = n.totalOrcamento, entrega = n.entrega, observacao = n.observacao, RowKey = n.RowKey, status = n.status.ToUpper() });
                return pedidoLista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        //public bool AlterarQuantidadeAtendia(string id,double quantidade)
        //{
        //    try
        //    {
        //        ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
        //        WSIQCard.AtualizarQuantidadeAtendidaItemPedido(GlbVariaveis.chavePrivada, id, quantidade);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return true;
        //}


        public List<ServiceReference1.PedidoEntrega> DadosMovimentacaoEntregaPedido(string idPedido)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.DadosMovimentacaoEntregaPedido(GlbVariaveis.chavePrivada, idPedido, "", Funcoes.CriptografarComSenha(GlbVariaveis.glb_chaveIQCard, GlbVariaveis.chavePrivada));
                return dados.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public List<ServiceReference1.ClienteCartao> UsuariosLocalizacao()
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.UsuariosLocalizacao(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, "", 0, 0, "", 0.5, "N");
                return dados.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }



        public int ContadorLocalizacao()
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.ContadorUsuariosLocalizacao(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, "", 0, 0, "", 3);
                contadorUsuarios = dados;
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public int ContadorComentarios()
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCARD = new ServiceReference1.WSIQPassClient();
                var resultado = WSIQCARD.ContadorComentarios(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, "N");
                return resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<ServiceReference1.Comentarios> ListagemComentarios()
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCARD = new ServiceReference1.WSIQPassClient();
                var resultado = WSIQCARD.ListagemComentariosEmpresa(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, "N").ToList();
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool AlterarEnderecoPedido(string endereco)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                WSIQCard.AlterarLocalizacao(GlbVariaveis.chavePrivada, DadosPedido.RowKey, endereco);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }


        public bool GerarVenda(bool verificarQuantidadesDisponivel = false)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            //Apagando os itens e forma de pagamentos pendentes
            string sql = "DELETE FROM vendas WHERE id = '" + GlbVariaveis.glb_IP + "';" +
            "DELETE from caixas WHERE enderecoip='" + GlbVariaveis.glb_IP + "'";
            entidade.ExecuteStoreCommand(sql);

            FuncoesECF fecf = new FuncoesECF();
            Produtos produto = new Produtos();
            Venda venda = new Venda();
            try
            {
                if (verificarQuantidadesDisponivel)
                    VerificarItens();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            foreach (var item in itensPedidoIQCard)
            {
                var dadosPrd = produto.ProcurarCodigo(item.referenciaItem, GlbVariaveis.glb_filial);

                try
                {
                    Thread.Sleep(100);
                    //fecf.VenderItemECF(item.referenciaItem, produto.descricao, Convert.ToDecimal(item.preco), 0, 0, 0, acrescimoValor, Convert.ToDecimal(item.quantidade), produto.unidade, produto.tributacao, Convert.ToInt16(produto.icms), produto.tipo, produto.ncm, Produtos.codigocestProdutos(item.referenciaItem, GlbVariaveis.glb_filial));
                    venda.InserirItem(false, produto.codigo, produto.descricao, "", produto.quantidadeDisponivel, produto.quantidadePrat, Convert.ToDecimal(item.quantidadeAtendida), Convert.ToDecimal(item.preco), Convert.ToDecimal(item.preco), produto.custo, produto.unidade, produto.embalagem, 0, 0, Venda.vendedor, Convert.ToInt16(produto.icms), produto.tributacao, 0, "0 - Produto", "", produto.grade, true);
                    Thread.Sleep(300);
                }
                catch (Exception ex)
                {
                    throw new Exception(item.referenciaItem + " " + item.descricao + "   " + ex.Message);
                }
            }


            // Adicionando pagamento se for para finalizar
            caixas pagamento = new caixas();

            pagamento.EnderecoIP = GlbVariaveis.glb_IP;
            pagamento.CodigoFilial = GlbVariaveis.glb_filial;
            pagamento.valor = Convert.ToDecimal(DadosPedido.totalOrcamento);
            pagamento.caixa = 0;
            pagamento.vencimento = GlbVariaveis.Sys_Data;
            pagamento.data = GlbVariaveis.Sys_Data;
            pagamento.tipopagamento = "PF";
            pagamento.descricaopag = "PONTOS IQCARD";
            pagamento.operador = GlbVariaveis.glb_Usuario;
            pagamento.dpfinanceiro = "Venda";
            pagamento.filialorigem = GlbVariaveis.glb_filial;
            pagamento.Cartao = "IQ CARD";
            pagamento.numeroCartao = DadosPedido.idCartao;
            pagamento.nome = DadosPedido.nomeCartao;
            pagamento.historico = "Pedido IQCARD: " + DadosPedido.RowKey;
            pagamento.vendedor = "000";
            entidade.AddTocaixas(pagamento);
            entidade.SaveChanges();
            //MudarStatus("Aguardando retirada");
            Venda.IQCard = IqCard.dadosCartao.idCartao;
            Venda.idPedidoIQCARD = IqCard.DadosPedido.RowKey;
            return true;
        }


        public bool VerificarItens()
        {
            FuncoesECF fecf = new FuncoesECF();
            Produtos produto = new Produtos();



            foreach (var item in itensPedidoIQCard)
            {
                try
                {
                    var dadosPrd = produto.ProcurarCodigo(item.referenciaItem, GlbVariaveis.glb_filial);
                    if (!dadosPrd)
                        throw new Exception("O item incluído no pedido do cliente não foi encontrado no seu sistema de venda: Item" + item.referenciaItem + " " + item.descricao);

                    if (!Permissoes.venderQtdNegativa && (Convert.ToDecimal(produto.quantidadeDisponivel) - Convert.ToDecimal(item.quantidade)) < 0)
                    {
                        throw new Exception("O item nâo está disponível no estoque, ajuste a quantidade para prosseguir. " + produto.codigo + " " + produto.descricao);
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Restrições de itens: " + ex.Message);
                }

            }
            return true;
        }

        public bool MudarStatus(string status, string documento)
        {
            try
            {
                string obs = "Finalizado pelo operador: " + GlbVariaveis.glb_Usuario;
                if (string.IsNullOrEmpty(documento))
                    obs = "";

                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.AlterarStatusPedido(GlbVariaveis.chavePrivada, DadosPedido.RowKey, status, documento, obs);
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public string MudarStatusEntrega(string status)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.MovimentacaoEntregaPedido(GlbVariaveis.chavePrivada, DadosPedido.RowKey, status, GlbVariaveis.glb_Usuario);
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public int ContadorPedido(string status)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.ContadorPedido(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, status);
                horaVerificadoPedido = DateTime.Now;
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public bool ExcluirItensAplicativo(bool somentePromocao, bool definitivo)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.ExcluirItensDeliveryLote(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, somentePromocao, definitivo);
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public bool AtualizarItensDelivery(bool marcados, bool quantidade, bool codigobarras, bool promocao, bool marketPlace)
        {

            ProdutosDelivery produtos = new ProdutosDelivery();
            IqCard iqcard = new IqCard();
            var listagemProdutos = produtos.ListagemProdutosLocal(marcados, quantidade, codigobarras, promocao, marketPlace);
            List<ServiceReference1.ItensDelivery> listagemNuvem = new List<ServiceReference1.ItensDelivery>();

            try
            {
                listagemNuvem = iqcard.ListagemItensDelivery();
            }
            catch
            {
                throw new Exception("Erro ao pegar a lista");
            }



            try
            {
                int x = 0;
                List<ServiceReference1.ItensDelivery> listaEnvio = new List<ServiceReference1.ItensDelivery>();
                foreach (var item in listagemProdutos)
                {
                    var verificarInclusao = (from n in listagemNuvem where n.codigo == item.codigo && n.preco == item.preco select n).FirstOrDefault();

                    if (verificarInclusao == null)
                    {
                        listaEnvio.Add(item);
                    }


                    if (x >= 100 && listaEnvio.Count() > 0)
                    {
                        ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                        WSIQCard.IncluirItemDeliveryLote(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, listaEnvio.ToArray());
                        listaEnvio.Clear();
                        x = 0;
                    }
                    x++;
                }

                // Aqui faz o envio do restante caso a lista seja menor que 100;

                if (listaEnvio.Count() > 0)
                {
                    ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                    WSIQCard.IncluirItemDeliveryLote(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, listaEnvio.ToArray());
                    listaEnvio.Clear();
                    x = 0;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("aqui" + ex.Message);
            }

        }

        public List<ServiceReference1.ItensDelivery> ListagemItensDelivery()
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.ListagemItensDelivery(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, true, "", false, 9999999, "", GlbVariaveis.glb_chaveIQCard);
                return dados.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public int ContadorItensDelivery(string idEmpresa)
        {
            try
            {
                if (contadorItensDelivery > 0)
                    return contadorItensDelivery;

                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.ContadorItensDelivery(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, true);
                contadorItensDelivery = dados;
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public bool Autorizado(string id, string idCartao)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                return WSIQCard.VerificarAutorizacao(GlbVariaveis.chavePrivada, id, idCartao);
            }
            catch (Exception)
            {
                return false;
            }

        }


        public static ServiceReference1.Parametros Parametros()
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.Parametros(GlbVariaveis.chavePrivada, "");
                parametros = dados;
                return dados;
            }
            catch (Exception)
            {
                return null;
            }

        }



        public int SaldoEticket()
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.DadosEmpresa(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard);
                saldoPontos = dados.saldoEticket;
                return dados.saldoEticket;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public ServiceReference1.Empresa DadosEmpresa(string idEmpresa)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.DadosEmpresa(GlbVariaveis.chavePrivada, idEmpresa);
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public bool AtualizarCoeficiente(double Coeficiente)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.AtualizarCoeficientePontuacao(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, Coeficiente);
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public string SolicitarDeposito()
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.SolicitarDeposito(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard);
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool TransformarCreditoEmPontos(double valor)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.CreditarValorSaldoEticket(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, valor, GlbVariaveis.glb_Usuario + " Converteu os créditos em pontos");
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int PontosRecentes()
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.TotalPontosAcumuladosPeriodo(GlbVariaveis.chavePrivada, "", GlbVariaveis.glb_chaveIQCard, DateTime.Now.AddDays(-7), DateTime.Now.Date);
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ServiceReference1.PontosAcumulados> PontosRecentesListagem()
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.TotalPontosAcumuladosPeriodoListagem(GlbVariaveis.chavePrivada, "", GlbVariaveis.glb_chaveIQCard, DateTime.Now.AddDays(-7), DateTime.Now.Date);
                return dados.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public string SolicitarRecargaPontos(int pontos)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.SolicitarRecargaPontos(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, pontos, GlbVariaveis.glb_Usuario + " FEZ A RECARGA");
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string UrlImagem(string codigo)
            {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                return WSIQCard.ImagemPorCodigo(GlbVariaveis.chavePrivada, codigo).urlImage1;
            }
            catch (Exception)
            {
                return "";
                
            }

            }

        public bool UploadFile(byte[] arquivo, string idPromocao,string drive="logos",string nomeArquivo="")
        {
            try
            {
                if (string.IsNullOrEmpty(nomeArquivo))
                {
                    nomeArquivo = GlbVariaveis.glb_chaveIQCard + "_promocao1";
                }
                               

                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();                                              
                //if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                //    nomeArquivo = GlbVariaveis.idCliente + "_promocao1";

                var dados = WSIQCard.UploadFile(arquivo, drive, nomeArquivo, "","");

                try
                {
                    if (!string.IsNullOrEmpty(idPromocao))
                        WSIQCard.SinalizarImagemPromocao(GlbVariaveis.chavePrivada, idPromocao, "S");
                }
                catch (Exception)
                {                    
                }
               
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string CadastrarImagem(string codigo,string descricao)
        {
            try
            {
                if (string.IsNullOrEmpty(codigo) || codigo == "_image1")
                {
                    throw new Exception("Escolha um codigo valido");
                }
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                string urlImage1 = "https://iqcard.blob.core.windows.net/imagens/" + codigo + "_image1";
                ServiceReference1.Imagens imagem = new ServiceReference1.Imagens()
                {
                    codigo = codigo,
                    descricao = descricao,
                    data = DateTime.Now,
                    marketplace = "N",
                    operador = GlbVariaveis.glb_Usuario,
                    RowKey = codigo,
                    urlImage1=urlImage1,
                    urlImage2=""
                };

               return WSIQCard.IncluirImagens(GlbVariaveis.chavePrivada, imagem);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ApagarArquivo(string drive="logos",string nomeArquivo="",string id="")
        {
            ServiceReference1.WSIQPassClient WSIQCARD = new ServiceReference1.WSIQPassClient();
            if(string.IsNullOrEmpty(nomeArquivo))
                nomeArquivo = GlbVariaveis.glb_chaveIQCard + "_promocao1";

            if (string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                nomeArquivo = GlbVariaveis.idCliente + "_promocao1";

            try
            {
                WSIQCARD.ApagarImagem(GlbVariaveis.chavePrivada, id);
            }
            catch (Exception)
            {
                
            }
           

            var resultado = WSIQCARD.ApagarArquivo(drive, nomeArquivo);

            return resultado;
        }


        public List<ServiceReference1.ListaCompraCompartilhamento> ListaCompras(int listaNumero)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.ListagemListaCompraCompartilhada(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, GlbVariaveis.glb_chaveIQCard, listaNumero);
                return dados.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int ContadorProcuraPromocao()
        {
            try
            {
                if (usuariosProcurandoPromo > 0)
                    return usuariosProcurandoPromo;

                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.ContadorProcuraPromocaoDia(GlbVariaveis.chavePrivada, Configuracoes.cidade, GlbVariaveis.glb_chaveIQCard, GlbVariaveis.idCliente);
                usuariosProcurandoPromo = dados;
                horaVerificadoPromocao = DateTime.Now;

                return dados;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<ItensListaCompra> ListaComprasItens(string idCartao, int numeroLista)
        {
            Produtos produto = new Produtos();
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var dados = WSIQCard.ListaInteresseArtigoUsuario(GlbVariaveis.chavePrivada, idCartao, "S", "S", idCartao, numeroLista,"","");

                List<ItensListaCompra> listagem = new List<ItensListaCompra>();
                foreach (var item in dados)
                {
                    try
                    {
                        produto.ProcurarCodigo(item.codigo, GlbVariaveis.glb_filial, true);
                        listagem.Add(new ItensListaCompra { codigo = item.codigo, descricao = item.descricao, quantidade = item.quantidade, preco = produto.preco, total = item.quantidade * produto.preco });
                    }
                    catch (Exception)
                    {
                        listagem.Add(new ItensListaCompra { codigo = item.codigo, descricao = item.descricao, quantidade = item.quantidade, preco = 0, total = 0 });
                    }

                }
                return listagem;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string EnviarDAVIQCARD(string idCartao, long davNumero)
        {

            ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
            siceEntities entidade = Conexao.CriarEntidade();
            var dadosDAV = (from n in entidade.contdav
                            where n.numeroDAVFilial == davNumero
                            && n.codigofilial == GlbVariaveis.glb_filial
                            select n).First();


            ServiceReference1.Orcamento dados = new ServiceReference1.Orcamento()
            {
                data = DateTime.Now,
                aprovado = true,
                descricao = "Orcamento DAV: " + davNumero.ToString(),
                idCartao = idCartao,
                nomeEmpresa = GlbVariaveis.nomeEmpresa,
                idEmpresa = GlbVariaveis.glb_chaveIQCard,
                excluido = false,
                observacao = dadosDAV.observacao,
                encerrado = true,
                RowKey = Guid.NewGuid().ToString(),
                visualizado = true,
                status = "Encerrado",
                totalOrcamento = Convert.ToDouble(dadosDAV.valor)
            };


            var dadosItens = from n in entidade.vendadav
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

            string idOrcamento = "";
            try
            {
                idOrcamento = WSIQCard.IncluirOrcamento(GlbVariaveis.chavePrivada, dados);

                List<ServiceReference1.ItensOrcamento> listaEnvio = new List<ServiceReference1.ItensOrcamento>();

                int x = 0;
                foreach (var item in dadosItens)
                {
                    ServiceReference1.ItensOrcamento novo = new ServiceReference1.ItensOrcamento()
                    {
                        descricao = item.produto,
                        idOrcamento = idOrcamento,
                        observacao = "",
                        preco = Convert.ToDouble(item.precooriginal),
                        quantidade = Convert.ToDouble(item.quantidade),
                        total = Convert.ToDouble(item.total),
                        referenciaItem = item.codigo
                    };
                    listaEnvio.Add(novo);

                    if (x >= 50 && listaEnvio.Count() > 0)
                    {
                        WSIQCard.IncluirItemOrcamentoLote(GlbVariaveis.chavePrivada, listaEnvio.ToArray());
                        listaEnvio.Clear();
                        x = 0;
                    }
                    x++;
                }

                // Aqui faz o envio do restante caso a lista seja menor que 100;

                if (listaEnvio.Count() > 0)
                {
                    WSIQCard.IncluirItemOrcamentoLote(GlbVariaveis.chavePrivada, listaEnvio.ToArray());
                    listaEnvio.Clear();
                    x = 0;
                }

                try
                {
                    string sql = "UPDATE contdav SET cartaofidelidade='" + idCartao + "'" + " WHERE numero='" + davNumero.ToString() + "'" + " AND codigofilial='" + GlbVariaveis.glb_filial + "'";
                    Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                }
                catch (Exception)
                {

                }

                //if(!string.IsNullOrEmpty(idOrcamento))
                //{
                //    WSIQCard.IncluirItemOrcamento()
                //}

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return idOrcamento;
        }

        public int PegarCodigoDesconto(string codigo)
        {
            try
            {
                ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
                var desconto = WSIQCard.PegarCupomDesconto(GlbVariaveis.chavePrivada, GlbVariaveis.glb_chaveIQCard, codigo);
                if (desconto != null)
                    return desconto.pontos;
                if (desconto == null)
                    throw new Exception("CODE Voucher não foi encontrado");
                if (desconto.validade.Date < DateTime.Now.Date)
                {
                    throw new Exception("Voucher expirado");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return 0;
        }

        public string LancarPontos(string idCartao, int documento, decimal desconto, decimal valorLiquido, decimal valorCA, decimal valorCR, decimal valorPF)
        {
            // Aqui lança os pontos para o IQCard se o cartão foi usado Ação de PayBack

            try
            {
                IqCard.pontosIQCARD = 0;
                IqCard.idRegistroPontosIQCARD = "";

                if (!string.IsNullOrWhiteSpace(idCartao) && !string.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard))
                {
                    if (IqCard.VerificarNumeroCartao(Venda.IQCard) == true)
                    {
                        double pontuacao = Convert.ToDouble(valorLiquido);

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

                        if (Configuracoes.coefecientePontosIQCard > 0 && Configuracoes.valorcomprafidelizacao == 0)
                        {
                            pontuacao = pontuacao * Convert.ToDouble(Configuracoes.coefecientePontosIQCard);
                        }

                        // A configuração de pontos de acordo com a compra tem
                        //precedência sobre a config. de percentual
                        if (Configuracoes.qtdpontosfidelizacao > 0)
                        {
                            pontuacao = pontuacao * Convert.ToDouble((Configuracoes.qtdpontosfidelizacao));

                        }

                        if (pontuacao > Configuracoes.pontuacaoMaxIQCard)
                        {
                            pontuacao = Configuracoes.pontuacaoMaxIQCard;
                        }


                        if (pontuacao >= 1)
                        {
                            IqCard.pontosIQCARD = Convert.ToInt32(pontuacao);
                            IqCard.saldoPontos -= Convert.ToInt32(pontuacao);
                            var resultado = card.RegistrarCompra(GlbVariaveis.chavePrivada, idCartao, GlbVariaveis.glb_chaveIQCard, GlbVariaveis.nomeEmpresa, pontuacao, Convert.ToDouble(desconto), pontuacao, documento.ToString());
                            IqCard.idRegistroPontosIQCARD = resultado;
                            Conexao.CriarEntidade().ExecuteStoreCommand("UPDATE contdocs set idpontuacaoIQCARD='" + resultado + "' WHERE documento='" + documento + "'");
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return "";
        }

        public int EnviarImagensCloud()
        {
            int x = 0;
            try
            {
                string diretorioImagens = @"C:\iqsistemas\SICEpdv\imagensupcloud";

                string[] dirs = Directory.GetFiles(diretorioImagens);
                Produtos prd = new Produtos();

                foreach (var item in dirs)
                {
                    string nomeArquivo = Path.GetFileName(item);
                    string codigoUpload = Path.GetFileName(item).Replace(".jpg","").Replace(".jpeg","");
                    string descricaoPrd = "";
                    string codigoPrd = ""; ;

                    try
                    {
                        if (prd.ProcurarCodigo(codigoUpload, GlbVariaveis.glb_filial, true))
                        {
                            descricaoPrd = prd.descricao;
                            codigoPrd = prd.codigo;
                        }

                    }
                    catch (Exception)
                    {
                        
                    }
                    


                    byte[] buffer = File.ReadAllBytes(item);


                    bool enviar = true;
                    try
                    {
                        CadastrarImagem(codigoUpload, descricaoPrd);
                    }
                    catch (Exception)
                    {
                        enviar = false;
                        
                    }
                    

                    try
                    {
                        if (enviar)
                        {
                            if (UploadFile(buffer, codigoUpload, "imagens", codigoUpload + "_image1"))
                            {
                                x++;
                                File.Delete(item);
                            }
                        }

                    }
                    catch (Exception)
                    {
                        
                    }
                   
                    try
                    {

                        try
                        {
                            string sqlApagar = "DELETE FROM produtosimagens WHERE codigoprd='" + codigoPrd + "' AND codigofilial='" + GlbVariaveis.glb_filial + "' LIMIT 1";
                            Conexao.CriarEntidade().ExecuteStoreCommand(sqlApagar);

                        }
                        catch (Exception)
                        {

                        }

                        siceEntities entidade = Conexao.CriarEntidade();
                        produtosimagens imagem = new produtosimagens()
                        {
                            codigofilial = GlbVariaveis.glb_filial,
                            codigoprod = codigoUpload,
                            imagem = buffer,

                        };

                        entidade.AddToprodutosimagens(imagem);
                        entidade.SaveChanges();
                    }
                    catch (Exception)
                    {
                       
                    }


                }

                return x;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }

    class ProdutosDelivery
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public double preco { get; set; }
        public double precoPromocao { get; set; }
        public string categoria { get; set; }
        public string detalhesProduto { get; set; }
        public string marketplace { get; set; }
        public string link { get; set; }
        public string mostrarlojavirtual { get; set; }
        public int quantidade { get; set; }
        public string urlImagem { get; set; }
        public DateTime? validadePromocao { get; set; }



        public List<ServiceReference1.ItensDelivery> ListagemProdutosLocal(bool marcados, bool quantidade, bool codigobarras, bool somentePromocao, bool marketPlace)
        {

            string tabela = "produtos";
            if (GlbVariaveis.glb_filial != "00001")
                tabela = "produtosfilial";

            string filtro = " AND quantidade>0";

            if (marcados)
                filtro = " AND marcado='X'";

            if (quantidade)
                filtro += " AND quantidade>0";

            if (codigobarras)
                filtro += " AND codigobarras<>''";

            if (somentePromocao)
                filtro += " AND situacao='Promoção'";

            if (marketPlace)
                filtro += " AND vendainternet='S'";


            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            string sql = "SELECT IF(codigobarras<>'',codigobarras,codigo) AS codigo,descricao,precovenda as preco,IF(descontopromocao>0, ROUND((precovenda+precovenda*(-descontopromocao/100)),2),0) as precoPromocao,grupo as categoria,detalhetecnico as detalhesProduto, vendainternet as marketplace,link,mostrarlojavirtual,quantidade,urlimagem,validadepromoc as validadePromocao FROM " + tabela + " WHERE (situacao<>'Inativo' AND situacao<>'Excluído') " + filtro;

            var dados = entidade.ExecuteStoreQuery<ProdutosDelivery>(sql).AsQueryable();

            List<ServiceReference1.ItensDelivery> listagem = new List<ServiceReference1.ItensDelivery>();
            foreach (var item in dados)
            {
                int quantidadeDisponivel = Convert.ToInt32(item.quantidade);
                if (quantidadeDisponivel < 0)
                    quantidadeDisponivel = 0;
                string lojaVirtual = item.marketplace;
                if (item.preco != item.precoPromocao)
                {
                    lojaVirtual = "S";
                }

                ServiceReference1.ItensDelivery novo = new ServiceReference1.ItensDelivery()
                {
                    ativo = "S",
                    descricao = item.descricao,
                    codigo = item.codigo,
                    preco = item.preco,
                    data = DateTime.Now,
                    categoria = item.categoria,
                    volume = "",
                    precoPromocao = item.precoPromocao,
                    detalhesProduto = item.detalhesProduto,
                    marketplace = "N",// item.marketplace,
                    link = item.link,
                    lojaVirtual = lojaVirtual,
                    pontosIQCARD = 0,
                    quantidadeDisponivel = quantidadeDisponivel,
                    cidade = "",
                    idEmpresa = "",
                    nomeEmpresa = "",
                    temImagem = "",
                    urlImage1 = item.urlImagem,
                    urlImage2 = "",
                    RowKey = "",
                    validadePromocao=item.validadePromocao
                };
                listagem.Add(novo);
            }

            return listagem.ToList();
        }

        public int AtualizarUrlImagens()
        {

            string tabela = "produtos";
            if (GlbVariaveis.glb_filial != "00001")
                tabela = "produtosfilial";

            string filtro = " AND urlImagem is NULL";


            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            string sql = "SELECT IF(codigobarras<>'',codigobarras,codigo) AS codigo,descricao,precovenda as preco,IF(descontopromocao>0, ROUND((precovenda+precovenda*(-descontopromocao/100)),2),0) as precoPromocao,grupo as categoria,detalhetecnico as detalhesProduto, vendainternet as marketplace,link,mostrarlojavirtual,quantidade FROM " + tabela + " WHERE (situacao<>'Inativo' AND situacao<>'Excluído') AND vendainternet='S'" + filtro;


            var dados = entidade.ExecuteStoreQuery<ProdutosDelivery>(sql).AsQueryable();

            List<ServiceReference1.ItensDelivery> listagem = new List<ServiceReference1.ItensDelivery>();
            foreach (var item in dados)
            {
                int quantidadeDisponivel = Convert.ToInt32(item.quantidade);
                if (quantidadeDisponivel < 0)
                    quantidadeDisponivel = 0;

                ServiceReference1.ItensDelivery novo = new ServiceReference1.ItensDelivery()
                {
                    ativo = "S",
                    descricao = item.descricao,
                    codigo = item.codigo,                    
                    preco = item.preco,
                    data = DateTime.Now,
                    categoria = item.categoria,
                    volume = "",
                    precoPromocao = item.precoPromocao,
                    detalhesProduto = item.detalhesProduto,
                    marketplace = "N",// item.marketplace,
                    link = item.link,
                    lojaVirtual = item.marketplace,
                    pontosIQCARD = 0,
                    quantidadeDisponivel = quantidadeDisponivel,
                    cidade = "",
                    idEmpresa = "",
                    nomeEmpresa = "",
                    temImagem = "",
                    urlImage1 = "",
                    urlImage2 = "",
                    RowKey = ""

                };
                listagem.Add(novo);
            }

            ServiceReference1.WSIQPassClient WSIQCard = new ServiceReference1.WSIQPassClient();
            var imagensNuvem = WSIQCard.ListagemImagens(GlbVariaveis.chavePrivada, "", 99999,9999999).ToList();

            int x = 0;

            try
            {
                var sqlFilial = "UPDATE produtosimagens SET codigofilial='00001' WHERE codigofilial IS NULL";
                Conexao.CriarEntidade().ExecuteStoreCommand(sqlFilial);
            }
            catch (Exception)
            {
                
            }
           


            foreach (var item in listagem)
            {

                string codigoUpload = item.codigo.Trim();

                if (string.IsNullOrEmpty(item.codigo.Trim()) || codigoUpload.Length != 13)
                {
                    codigoUpload = item.codigo + GlbVariaveis.idCliente + GlbVariaveis.glb_filial;
                }

                //if (item.codigo.Length == 13)
                //{
                    try
                    {
                       
                        var urlBancoImagem = imagensNuvem.Where(c => c.codigo == codigoUpload).FirstOrDefault();

                        if (urlBancoImagem != null)
                        {
                            sql = "UPDATE produtos SET urlImagem='" + urlBancoImagem.urlImage1 + "' WHERE (codigobarras='" + item.codigo + " OR codigo=''"+item.codigo+"')";
                            Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                            sql = "UPDATE produtosfilial SET urlImagem='" + urlBancoImagem.urlImage1 + "' WHERE (codigobarras='" + item.codigo + "' OR codigo='"+item.codigo+"')";
                            Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                            x++;
                        }
                        else
                        {
                            //if (item.codigo.Length == 13)
                            //{
                                string cod = (from n in Conexao.CriarEntidade().produtos where (n.codigo == item.codigo || n.codigobarras==item.codigo) select n.codigo).FirstOrDefault();
                                if (GlbVariaveis.glb_filial != "00001")
                                {
                                    cod = (from n in Conexao.CriarEntidade().produtosfilial where (n.codigo == item.codigo || n.codigobarras==item.codigo) select n.codigo).FirstOrDefault();
                                }

                                var imagem = (from n in Conexao.CriarEntidade().produtosimagens where n.codigoprod==cod /*&& n.codigofilial==GlbVariaveis.glb_filial*/ select n.imagem).FirstOrDefault();
                                if (imagem != null)
                                {
                                    IqCard iqcard = new IqCard();
                                    iqcard.CadastrarImagem(codigoUpload, item.descricao);
                                    iqcard.UploadFile(imagem, "", "imagens", codigoUpload + "_image1");
                                    string urlImagem = @"https://iqcard.blob.core.windows.net/imagens/" + codigoUpload + "_image1";
                                    sql = "UPDATE " + tabela + " SET urlImagem='" + urlImagem+ "' WHERE (codigo='" + item.codigo + "' OR codigobarras='"+item.codigo+"')";
                                    Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                                    x++;
                                }

                            
                        }

                    }
                    catch (Exception ex)
                    {
                    };

                //}
            }

            return x;
        }

    }

    class lembrete
    {
        public string nome { get; set; }
        public string cpf { get; set; }
    };

    class ItensListaCompra
    {
        public string idCartao { get; set; }
        public string nomeCartao { get; set; }
        public string codigo { get; set; }
        public string descricao { get; set; }
        public int quantidade { get; set; }
        public decimal preco { get; set; }
        public decimal total { get; set; }
    }
}
